
Imports System.Linq
Imports System.Linq.Enumerable
Imports gnu.java.security.x509.ext.Extension

Public Class wfDueJobPlanning_Ajax
    Inherits System.Web.UI.Page
#Region " Enumeration "
    Private Enum Rights
        [New] = 1
        Edit = 2
        Delete = 3
        Save = 4
        View = 5
        Print = 6
        Authorized = 7
    End Enum
    Private Enum RequstFor
        Supplier = 0
        Customer = 1
    End Enum
#End Region

#Region " Variable Declaration "
    Public mDueJobPlanning As DueJobPlanning
    Public mDueJobPlanningItem As DueJobPlanningItem
    Dim EventLogID As Guid
    Dim mDueJobPlanningDetail As String
    Dim mMachineID As Guid
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    Dim mFileAttachments As FileAttachments
    'End
    Dim mUser As User
    Public mIsAttachmentNotSave As Boolean = True
    Dim email As Thread
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mDueJobPlanning = CType(Session("mDueJobPlanning"), DueJobPlanning)
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
    End Sub
    Private Sub SetSession()
        Session("mDueJobPlanning") = mDueJobPlanning
        Session("mFileAttach") = mFileAttach
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub RemoveSessions()
        Session.Remove("mDueJobPlanning")
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        'End
    End Sub
    Private Sub SetTitle()
        lblTitle.Text = "Maintenance Planning Of " + mDueJobPlanning.RegNo
    End Sub
    Private Sub Disable()
        txtDueJobPlanningDate.Enabled = False
    End Sub
    Private Sub Enable()
        txtDueJobPlanningDate.Enabled = True
    End Sub
    Private Sub Save()
        'Authentication
        If Not mDueJobPlanning.DueJobPlanningDate Is System.DBNull.Value Then
            Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
            If mCheck.WebAuthentication = True Then
                Dim mDays As Integer = 0
                mDays = mCheck.Number("Days")
                Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
                If DateDiff(DateInterval.Day, CDate(mDueJobPlanning.DueJobPlanningDate), maxAllowableDate) < 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Goods Receipt. <br> Goods Receipt Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            End If
        End If
        Dim DueJobPlanningClone As DueJobPlanning
        DueJobPlanningClone = mDueJobPlanning.Clone
        Try

            'check whether min. one item & charge is present while saving
            If Not mDueJobPlanning.DueJobPlanningItems.Count = 0 Then
                'save the object
                SetObject()
                If mDueJobPlanning.IsValid Then
                    Dim i As Integer
                    While i < mDueJobPlanning.DueJobPlanningItems.Count
                        i = i + 1
                    End While
                    mDueJobPlanning.ApplyEdit()

                    mDueJobPlanning.Save()
                    If mDueJobPlanning.IsAttachmentAdded Then
                        If mDueJobPlanning.FileAttachments(0).Size > 0 Then
                            ImageButton1.Visible = True
                        End If

                    End If

                    mDueJobPlanningDetail = mDueJobPlanning.DueJobPlanningNo + " Dated: " + mDueJobPlanning.DueJobPlanningDateFormatted + " Created By: " + mDueJobPlanning.CreatedBy
                    MarkLog(Util.Action.Save, "DueJobPlanning", mDueJobPlanningDetail, Util.ErrorType.NoError, mDueJobPlanning.ID, EventLogID)
                    mDueJobPlanning.MarkClean()
                    Session("mDueJobPlanning") = mDueJobPlanning
                    DataFieldBind()
                    upnlTitle.Update()
                    upnlDueJobPlanningDetails.Update()
                    'upnlDueJobPlanning.Update()
                    upnlDueJobPlanningItem.Update()
                    upnlButtons.Update()
                    MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                Else
                    Dim mRule As String = ""
                    If mDueJobPlanning.GetBrokenRulesCollection.Count > 0 Then
                        mRule = mDueJobPlanning.GetBrokenRulesCollection(0).Description
                    ElseIf mDueJobPlanning.DueJobPlanningItems.CurrentItem.GetBrokenRulesCollection.Count > 0 Then
                        mRule = mDueJobPlanning.DueJobPlanningItems.CurrentItem.GetBrokenRulesCollection(0).Description
                    End If
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, mRule, MsgBoxStyle.OkOnly, "")
                    mRule = ""
                    mDueJobPlanning = DueJobPlanningClone
                    SetObject()
                    Session("mDueJobPlanning") = mDueJobPlanning
                    DataFieldBind()
                    Exit Sub
                End If
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Maintenance Support Plan can not be saved without assembly.", MsgBoxStyle.OkOnly, "")
                mDueJobPlanning = DueJobPlanningClone
                SetObject()
                Session("mDueJobPlanning") = mDueJobPlanning
                DataFieldBind()
                Exit Sub
            End If
        Catch ex As SqlException
            Session("DueJobPlanningClone") = DueJobPlanningClone
            If ex.Number = 8114 Or ex.Number = 8115 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf ex.Number = 547 Then
                If InStr(ex.Message, "CCtabIssueItemLoanQty", CompareMethod.Text) Or InStr(ex.Message, "CCtabIssueItemReceiptBalanceQty", CompareMethod.Text) Or InStr(ex.Message, "CCtabIssueItemLoanQty", CompareMethod.Text) Or InStr(ex.Message, "*15-TB02-CX07*", CompareMethod.Text) Or InStr(ex.Message, "*17-TB02-CX06*", CompareMethod.Text) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex.Message.Substring(ex.Message.IndexOf("PartNo.:")) + " Goods Receipt Qty can not be greater than Order / Issue Qty.", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf InStr(ex.Message, "CCtabOrderItemReceiptBalanceQty", CompareMethod.Text) Then
                    MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex.Message.Substring(ex.Message.IndexOf("PartNo.:")) + "Goods Receipt Qty can not be greater than Order Qty.</br></br><b>Please amend Purchase Order for Receipt of excess quantity.</b>", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                ElseIf InStr(ex.Message, "FKtabInvoiceChargetabCharge", CompareMethod.Text) Then
                    MSGBoxCtrl.Show("Alert!", "Other Charge Deleted ! ", "Other charge Not Available<Br><BR>Selected Charge is no longer exist in the Database <BR><BR> Remove Charge and try Again", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    MSGBoxCtrl.Show("Alert!", "Save Alert ! " + "</br>" + "There is some problem in Saving Goods Receipt. <BR> <BR>  Please Check the Entry and Try Again  !", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            End If
        Catch ex1 As Exception
            If InStr(ex1.Message, "CCtabIssueItemLoanQty", CompareMethod.Text) Or InStr(ex1.Message, "CCtabIssueItemReceiptBalanceQty", CompareMethod.Text) Or InStr(ex1.Message, "CCtabIssueItemLoanQty", CompareMethod.Text) Or InStr(ex1.Message, "*15-TB02-CX07*", CompareMethod.Text) Or InStr(ex1.Message, "*17-TB02-CX06*", CompareMethod.Text) Then
                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + " Goods Receipt Qty can not be greater than Order / Issue Qty.", MsgBoxStyle.OkOnly, "Status")
                mDueJobPlanning = DueJobPlanningClone
                SetObject()
                Session("mDueJobPlanning") = mDueJobPlanning
                DataFieldBind()
                Exit Sub
            ElseIf InStr(ex1.Message, "CCtabOrderItemReceiptBalanceQty", CompareMethod.Text) Then
                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Goods Receipt Qty can not be greater than Order Qty.</br><b>Please amend Purchase Order Quantity & make Goods Receipt again.</b>", MsgBoxStyle.OkOnly, "")
            Else
                MSGBoxCtrl.Show("Alert!", "Save Alert ! " + "</br>" + "There is some problem in Saving Goods Receipt. <BR> <BR>  Please Check the Entry and Try Again  !", "", MsgBoxStyle.OkOnly, "Status")
                mDueJobPlanning = DueJobPlanningClone
                SetObject()
                Session("mDueJobPlanning") = mDueJobPlanning
                DataFieldBind()
                Exit Sub
            End If
            mDueJobPlanning = DueJobPlanningClone
            Session("mDueJobPlanning") = mDueJobPlanning
        Finally
            DueJobPlanningClone = Nothing
        End Try
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub SetObject()
        If txtDueJobPlanningDate.Text.Trim = "" Then
            mDueJobPlanning.DueJobPlanningDate = Today.Date
        Else
            mDueJobPlanning.DueJobPlanningDate = CDate(txtDueJobPlanningDate.Text)
        End If
        mDueJobPlanning.Text = txtDueJobPlanningText.Text.Trim
        mDueJobPlanning.No = Val(txtDueJobPlanningNo.Text)

        If txtFromDate.Text.Trim <> "" And IsDate(txtFromDate.Text.Trim) = True Then
            mDueJobPlanning.FromDate = CDate(txtFromDate.Text)
        Else
            mDueJobPlanning.FromDate = System.DBNull.Value
        End If
        If txtToDate.Text.Trim <> "" And IsDate(txtToDate.Text.Trim) = True Then
            mDueJobPlanning.ToDate = CDate(txtToDate.Text)
        Else
            mDueJobPlanning.ToDate = System.DBNull.Value
        End If
        mDueJobPlanning.Remark = Trim(txtRemark.Text)
        mDueJobPlanning.CreatedBy = User.Identity.Name.ToUpper

        Dim txtValue As TextBox
        For i As Integer = 0 To dgDueJobPlanningPeriod.Rows.Count - 1
            txtValue = CType(Me.dgDueJobPlanningPeriod.Rows(i).FindControl("txtValue"), TextBox)
            If mDueJobPlanning.DueJobPlanningPeriods(i).PeriodID = 2 Then
                If Not Period.IsDate(txtValue.Text) Then
                    mDueJobPlanning.DueJobPlanningPeriods(i).PlannedValue = ""
                Else
                    mDueJobPlanning.DueJobPlanningPeriods(i).PlannedValue = Trim(txtValue.Text)
                End If
            Else
                mDueJobPlanning.DueJobPlanningPeriods(i).PlannedValue = Trim(txtValue.Text)
            End If
        Next i

    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "Delete")
        mDueJobPlanning.DueJobPlanningItems.CurrentIndex = Index
        Session("mDueJobPlanning") = mDueJobPlanning
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            mDueJobPlanning = CType(Session("mDueJobPlanning"), DueJobPlanning)

                            Dim periodsList As String() = mDueJobPlanning.DueJobPlanningItems.CurrentItem.PeriodIDWithDecValue.Split(" ")

                            mDueJobPlanning.DueJobPlanningItems.Remove(mDueJobPlanning.DueJobPlanningItems.CurrentItem)
                            mDueJobPlanning.DueJobPlanningItems.CurrentIndex = mDueJobPlanning.DueJobPlanningItems.Count - 1
                            dgDueJobPlanningItem.DataSource = mDueJobPlanning.DueJobPlanningItems
                            dgDueJobPlanningItem.DataBind()
                            upnlDueJobPlanningItem.Update()
                            For i As Integer = 0 To periodsList.Length - 1
                                If periodsList(i).Trim(" ").Split(":")(0) = 2 Then
                                    Dim PValue As Period
                                    PValue = New Period(2, CDec(periodsList(i).Trim(" ").Split(":")(1)), 2, True, False, 1)

                                    If CDate(PValue.Value) = mDueJobPlanning.FromDate Then mDueJobPlanning.FromDate = DBNull.Value
                                    Session("mDueJobPlanning") = mDueJobPlanning
                                    Exit For
                                End If
                            Next
                            SetEstimatedValues()
                            Session("mDueJobPlanning") = mDueJobPlanning
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "Close" Then
                        If mDueJobPlanning.IsValid = True Then
                            Session.Remove("IsValid")
                            DataFieldBind()
                            If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                                Exit Sub
                            End If
                            Save()
                            Response.Redirect("Index.aspx")
                        Else
                            Session.Remove("IsValid")
                            If CustomValidate1() = False Then
                                upnlValidationsummary.Update()
                                Exit Sub
                            End If
                        End If
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session.Remove("IsValid")
                        Session.Remove("mModuleName")
                        Session.Remove("mPendingItemList")
                        Session("Sender") = ""
                        Response.Redirect("Index.aspx")
                    End If

                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "RCITransTextSeriesAlert" Then
                        Session("sender") = ""
                        Session("AddTransTextSeries") = "True"
                        Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
                    End If
            End Select
        End If
    End Sub
    Private Sub addAttributes()
        txtDueJobPlanningNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtDueJobPlanningNo').value,event)")
    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights, Optional IsInRoleStr As String = "DueJobPlanning") As Boolean
        'Dim IsInRoleString As String = "DueJobPlanning"
        Dim IsInRoleString As String = ""
        IsInRoleString = IsInRoleStr
        Select Case CheckFor
            Case Rights.View
                Return User.IsInRole(IsInRoleString + "View")
            Case Rights.[New]
                Return User.IsInRole(IsInRoleString + "New")
            Case Rights.Edit
                Return User.IsInRole(IsInRoleString + "Edit")
            Case Rights.Save
                Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
            Case Rights.Delete
                Return User.IsInRole(IsInRoleString + "Delete")
            Case Rights.Print
                Return User.IsInRole(IsInRoleString + "Print")
            Case Rights.Authorized
                Return User.IsInRole(IsInRoleString + "Authorized")
        End Select
    End Function
    Private Sub ControlVisibilityForFileAttachment()
        If mDueJobPlanning.IsAttachmentAdded Then
            ImageButton1.Visible = True
            If mDueJobPlanning.IsWOCreated = True Then
                btnDelAttach.Enabled = False
                btnSelectFile.Disabled = True
            Else
                btnDelAttach.Enabled = True
                btnSelectFile.Disabled = False
            End If
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
            If mDueJobPlanning.IsWOCreated = True Then
                btnSelectFile.Disabled = True
            Else
                btnSelectFile.Disabled = False
            End If
        End If
    End Sub
    Private Sub ControlVisibilityOrEnability()
        If mDueJobPlanning.IsWOCreated = True Then
            dgDueJobPlanningItem.Columns(7).Visible = False
        Else
            dgDueJobPlanningItem.Columns(7).Visible = True
        End If
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        dgDueJobPlanningItem.DataSource = mDueJobPlanning.DueJobPlanningItems
        Session("mDueJobPlanning") = mDueJobPlanning
        txtDueJobPlanningDate.Text = mDueJobPlanning.DueJobPlanningDateFormatted.ToString
        txtFromDate.Text = mDueJobPlanning.FromDateFormatted.ToString
        txtToDate.Text = mDueJobPlanning.ToDateFormatted.ToString
        If (AppSettings("ShowCAMOOnlyForNewClients") = "True" Or AppSettings("ShowMaintenanceForNewClients") = "True") Then
            dgDueJobPlanningItem.Columns(2).HeaderText = "Task No./Directive No."
        Else
            dgDueJobPlanningItem.Columns(2).HeaderText = "Code"
        End If

        If mDueJobPlanning.IsNew Then SetEstimatedValues()

        dgDueJobPlanningPeriod.DataSource = mDueJobPlanning.DueJobPlanningPeriods
        DataBind()

    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValid As CustomValidator
        CustValid = CType(s, CustomValidator)
        If CustValid.ControlToValidate = "txtRemark" Then
            If Len(Trim(txtRemark.Text)) > 1000 Then
                CustValid.ErrorMessage = "Max. Length of Remark should be 1000."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf CustValid.ControlToValidate = "txtFromDate" Then
            If CDate(txtFromDate.Text.ToString) < CDate(txtDueJobPlanningDate.Text.ToString) Then
                CustValid.ErrorMessage = "From Date should be greater than or Equal to Maintenance Planning Date."
                e.IsValid = False
            End If
        ElseIf CustValid.ControlToValidate = "txtToDate" Then
            If CDate(txtToDate.Text.ToString) < CDate(txtDueJobPlanningDate.Text.ToString) Then
                CustValid.ErrorMessage = "To Date should be greater than or Equal to Maintenance Planning Date."
                e.IsValid = False
                'ElseIf CDate(txtToDate.Text.ToString) < CDate(txtFromDate.Text.ToString) Then
                '    CustValid.ErrorMessage = "To Date should be greater than or Equal to From Date."
                '    e.IsValid = False
            End If
        End If
    End Sub
    Private Sub SetEstimatedValues()
        Dim mDueLimits As DueLimits
        mDueJobPlanning = Session("mDueJobPlanning")
        mDueLimits = DueLimits.GetDueLimits(mDueJobPlanning.MachineID)

        'Resetvalues
        For Each DueJobPlanningPeriod As DueJobPlanningPeriod In mDueJobPlanning.DueJobPlanningPeriods
            If DueJobPlanningPeriod.PeriodID <> 2 Then
                DueJobPlanningPeriod.EstimatedValue = ""
            Else
                DueJobPlanningPeriod.EstimatedValue = System.DBNull.Value.ToString
            End If
        Next
        txtFromDate.Text = ""
        '''mDueJobPlanning.FromDate = System.DBNull.Value
        ''''----------------Reset
        '''
        For Each duelimit As DueLimit In mDueLimits
            For Each DueItem As DueJobPlanningItem In mDueJobPlanning.DueJobPlanningItems


                Dim periodsList As String() = DueItem.PeriodIDWithDecValue.Split(" ")
                Dim value As Period

                For i As Integer = 0 To periodsList.Length - 1
                    If duelimit.PeriodID.ToString = periodsList(i).Trim(" ").Split(":")(0) Then
                        'check for periodid

                        If duelimit.PeriodID <> 2 Then
                            value = New Period(duelimit.PeriodID, CDec(periodsList(i).Trim(" ").Split(":")(1)), 1, False, False, 1)

                            If Not mDueJobPlanning.DueJobPlanningPeriods.Contains(duelimit.PeriodID) Then
                                mDueJobPlanning.DueJobPlanningPeriods.Add(mDueJobPlanning.ID)
                                mDueJobPlanning.DueJobPlanningPeriods.CurrentItem.PeriodID = duelimit.PeriodID

                            End If
                            If mDueJobPlanning.DueJobPlanningPeriods(duelimit.PeriodID, "").EstimatedValue = "" Then
                                mDueJobPlanning.DueJobPlanningPeriods(duelimit.PeriodID, "").EstimatedValue = value.Value
                                mDueJobPlanning.DueJobPlanningPeriods(duelimit.PeriodID, "").PlannedValue = value.Value
                            Else
                                If value.DbValueDec < mDueJobPlanning.DueJobPlanningPeriods(duelimit.PeriodID, "").EstimatedValueDec Then
                                    mDueJobPlanning.DueJobPlanningPeriods(duelimit.PeriodID, "").EstimatedValue = value.Value
                                    mDueJobPlanning.DueJobPlanningPeriods(duelimit.PeriodID, "").PlannedValue = value.Value
                                End If
                            End If
                        Else
                            value = New Period(duelimit.PeriodID, CDec(periodsList(i).Trim(" ").Split(":")(1)), 2, True, False, 1)

                            '''Date 
                            If mDueJobPlanning.FromDate.Equals(System.DBNull.Value) Then '
                                mDueJobPlanning.FromDate = CDate(value.Value)
                            Else
                                If CDate(value.Value) < CDate(mDueJobPlanning.FromDate) Then
                                    mDueJobPlanning.FromDate = CDate(value.Value)
                                End If
                            End If
                        End If

                        Exit For
                    End If
                Next

            Next

        Next
        Session("mDueJobPlanning") = mDueJobPlanning
        dgDueJobPlanningPeriod.DataSource = mDueJobPlanning.DueJobPlanningPeriods
        dgDueJobPlanningPeriod.DataBind()
        txtFromDate.Text = mDueJobPlanning.FromDateFormatted.ToString
        upnlDueJobPlanningDetails.Update()
        upnlDueJobPlanningPeriods.Update()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        addAttributes()
        Disable()

        If Not IsPostBack And Session("Sender") = "" Then
            If CType(Session("AddTransTextSeries"), String) = "True" AndAlso (Not Session("TransText_ForTransSeries") Is Nothing) Then
                If mDueJobPlanning.IsNew Then
                    mDueJobPlanning.Text = Session("TransText_ForTransSeries")
                    Session("mDueJobPlanning") = mDueJobPlanning
                    Session("AddTransTextSeries") = "False"
                    Session.Remove("TransName_ForTransSeries")
                    Session.Remove("TransText_ForTransSeries")
                    Session.Remove("TransNo_ForTransSeries")
                End If

            End If
            'End
            DataFieldBind()
            SetTitle()
            ControlVisibilityForFileAttachment()
            ControlVisibilityOrEnability()
        End If
    End Sub
    Private Sub btnAssemblyAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAssemblyAdd.Click
        If IsValid Then
            SetObject()
            mDueJobPlanning.DueJobPlanningItems.Add(mDueJobPlanning.ID)
            Session("mDueJobPlanning") = mDueJobPlanning
            Session("EditDueJobPlanningItem") = False
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenDueJobPlanningItemWindow", "OpenDueJobPlanningItemWindow();", True)
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub dgDueJobPlanningItem_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgDueJobPlanningItem.RowCommand
        Dim mQtyBalReceived As Decimal = 0
        Select Case e.CommandName
            Case "EditView"
                Dim index As Int32 = CInt(e.CommandArgument) + dgDueJobPlanningItem.PageIndex * dgDueJobPlanningItem.PageSize
                SetObject()
                mDueJobPlanning.DueJobPlanningItems.CurrentIndex = index
                Session("mDueJobPlanning") = mDueJobPlanning
                Session("EditDueJobPlanningItem") = True
                Dim tmpDueJobPlanning As DueJobPlanning = mDueJobPlanning.Clone
                Session("tmpDueJobPlanning") = tmpDueJobPlanning
                Session("ItemIndex") = mDueJobPlanning.DueJobPlanningItems.CurrentIndex
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenDueJobPlanningItemWindow", "OpenDueJobPlanningItemWindow();", True)
            Case "DeleteRecord"
                Dim index As Int32 = CInt(e.CommandArgument) + dgDueJobPlanningItem.PageIndex * dgDueJobPlanningItem.PageSize
                DeleteRecord(index)
        End Select
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        MarkLog(Util.Action.Close, "DueJobPlanning", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        SetObject()
        Session("IsValid") = IsValid
        If mDueJobPlanning.IsDirty Then
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.CloseConfirm, "", MsgBoxStyle.YesNo, "Close")
            If IsValid Then
                SetObject()
            End If
        Else
            RemoveSessions()
            mDueJobPlanning = Nothing

            Dim mopenas As String = Request.QueryString("Type")
            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                Exit Sub
            End If

            Response.Redirect("Index.aspx")
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
        '    Exit Sub
        'End If
        SetObject()  '''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If CustomValidate1() = False Then
            upnlValidationsummary.Update()
            Exit Sub
        End If
        If IsValid Then
            Save()
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        If (Not IsInRole(Rights.Authorized) And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        If mDueJobPlanning.IsAttachmentAdded Then
            'mFileAttach = FileAttach.GetAttachment(mDueJobPlanning.ID)
            'mFileAttach = FileAttach.GetAttachmentChild(mDueJobPlanning.ID)
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mDueJobPlanning.FileAttachments(0).Extension 'mFileAttach.Extension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mDueJobPlanning.FileAttachments(0).Extension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mDueJobPlanning.FileAttachments(0).ImageFile, 0, mDueJobPlanning.FileAttachments(0).ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                Dim Str As String
                Str = "openFile();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
            End If
        End If
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        If (Not IsInRole(Rights.Authorized) And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte
        mDueJobPlanning.IsAttachmentAdded = False
        mDueJobPlanning.FileAttachments.Remove(mDueJobPlanning.ID)
        Session("mDueJobPlanning") = mDueJobPlanning
        Session("IsAttachmentNotSave") = mIsAttachmentNotSave
        ControlVisibilityForFileAttachment()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        Try
            If mFileAttach.ReferenceID.Equals(mDueJobPlanning.ID) Then
                If mDueJobPlanning.IsAttachmentAdded Then
                    mDueJobPlanning.FileAttachments(0).Size = mFileAttach.Size
                    mDueJobPlanning.FileAttachments(0).ImageFile = mFileAttach.ImageFile
                    mDueJobPlanning.FileAttachments(0).Extension = mFileAttach.Extension
                Else
                    mDueJobPlanning.IsAttachmentAdded = True
                    mDueJobPlanning.FileAttachments.Add(mFileAttach.ReferenceID, mFileAttach.ImageFile, mFileAttach.Size, mFileAttach.Extension, mFileAttach.Sort)
                End If
            Else

            End If
        Catch ex As Exception
        End Try

        ControlVisibilityForFileAttachment()
        dgDueJobPlanningItem.DataSource = mDueJobPlanning.DueJobPlanningItems
        dgDueJobPlanningItem.DataBind()
        Session("IsAttachmentNotSave") = True
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If (Not IsInRole(Rights.Authorized) And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        If mDueJobPlanning.IsAttachmentAdded = True Then
            mFileAttach = FileAttach.GetAttachmentChild(mDueJobPlanning.ID)
        Else
            mFileAttach = FileAttach.NewAttachmentChild(Guid.Empty, mDueJobPlanning.ID)
        End If

        Session("mFileAttach") = mFileAttach
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow()", True)
    End Sub
    Private Sub hdnBtnDueJobPlanningItem_Click(sender As Object, e As EventArgs) Handles hdnBtnDueJobPlanningItem.Click
        dgDueJobPlanningItem.DataSource = mDueJobPlanning.DueJobPlanningItems
        dgDueJobPlanningItem.DataBind()
        SetEstimatedValues()
        upnlDueJobPlanningItem.Update()
    End Sub
#End Region

#Region " Show BrokenRules "
    Public Function CustomValidate1() As Boolean
        Dim strMsg As String = ""
        SetObject()
        If mDueJobPlanning.IsValid = False Then
            For i As Integer = 0 To mDueJobPlanning.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mDueJobPlanning.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        Dim mDueJobPlanningItem As DueJobPlanningItem
        If mDueJobPlanning.DueJobPlanningItems.IsValid = False Then
            For Each mDueJobPlanningItem In mDueJobPlanning.DueJobPlanningItems
                For i As Integer = 0 To mDueJobPlanningItem.GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + mDueJobPlanningItem.AssemblyName + " : " + mDueJobPlanningItem.GetBrokenRulesCollection(i).Description + "<Br>"
                Next
            Next
        End If

        Dim mDueJobPlanningPeriod As DueJobPlanningPeriod
        If mDueJobPlanning.DueJobPlanningPeriods.IsValid = False Then
            For Each mDueJobPlanningPeriod In mDueJobPlanning.DueJobPlanningPeriods
                For i As Integer = 0 To mDueJobPlanningPeriod.GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + mDueJobPlanningPeriod.GetBrokenRulesCollection(i).Description + "<Br>"
                Next
            Next
        End If

        If strMsg.Trim <> "" Then
            CustValidator.ErrorMessage = strMsg
            CustValidator.IsValid = False
            Return False
        End If
        Return True
    End Function
#End Region

#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetDistinctTextListAutoComplete(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim mDistinctTextAutoComplete As DistinctTextListAutoComplete
        Dim str As String() = contextKey.Split("¿")
        Dim mTransTypeID As Integer = CInt(str(0).Substring(str(0).IndexOf("=") + 1))
        Dim mOrderDate As String = str(1).Substring(str(1).IndexOf("=") + 1)
        mDistinctTextAutoComplete = DistinctTextListAutoComplete.GetDistinctTextList(prefixText, , True, mTransTypeID, mOrderDate)
        If count = 0 Then
            Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In mDistinctTextAutoComplete
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).ToArray
        Else
            Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In mDistinctTextAutoComplete
                    Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).Take(count).ToArray
        End If
    End Function

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If Not IsInRole(Rights.Print) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If

        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportDocument 'CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim ds As New dsDueJobPlanning
        mDueJobPlanning = DueJobPlanning.GetDueJobPlanning(mDueJobPlanning.ID) 'Session("mDueJobPlanning")
        Dim mDueJobSpareList As DueJobSpareToolList = DueJobSpareToolList.GetSpareList(mDueJobPlanning)
        Dim mDueJobToolList As DueJobSpareToolList = DueJobSpareToolList.GetToolsList(mDueJobPlanning)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
            mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
            mCompanyDetail.WebSite, "", AppSettings("ShowCAMOOnlyForNewClients"), AppSettings("ShowCAMOOnlyForNewClients") = "True", "", AppSettings("ClientCode"), "", AppSettings("Product Version"),
            AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))


        da.Fill(ds, "DueJobPlanning", mDueJobPlanning)
        da.Fill(ds, "DueJobPlanningItem", mDueJobPlanning.DueJobPlanningItems)
        da.Fill(ds, "DueJobPlanningPeriod", mDueJobPlanning.DueJobPlanningPeriods)
        da.Fill(ds, "DueJobSpareList", mDueJobSpareList)
        da.Fill(ds, "DueJobToolList", mDueJobToolList)
        da.Fill(ds, Report)

        da.Fill(ds, mrptImage)

        myReport = New crDueJobPlanning

        myReport.SetDataSource(ds)

        Session("CrystalReport") = myReport
        MarkLog(Util.Action.Print, "Due Job Planning", "Due Job Planning : " + mDueJobPlanning.DueJobPlanningNo, Util.ErrorType.NoError, Guid.Empty, EventLogID)

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub

    Private Sub imgCreateWO_Click(sender As Object, e As ImageClickEventArgs) Handles imgCreateWO.Click
        If (Not IsInRole(Rights.New, "CAMOWO")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Dim mnWO As nWO
        Dim tmpAssemblyStatusList As AssemblyStatusList
        Dim AssemblyStatusPeriodList As AssemblyStatusPeriodList
        Session("mDueJobPlanning") = mDueJobPlanning

        mnWO = nWO.NewWO(TransTypeID:=Trans.WOCAMO)
        mnWO.WODate = mDueJobPlanning.FromDateFormatted
        mnWO.WOPlanedDate = mDueJobPlanning.FromDateFormatted
        mnWO.MachineID = mDueJobPlanning.MachineID

        Dim mrptDueReport As rptDueReportForOnlyDueReport = rptDueReportForOnlyDueReport.GetList(Today.Date.ToString, mDueJobPlanning.RegNo)


        If (AppSettings("ClientCode") = "RAL" Or AppSettings("ClientCode") = "ADeccan") Then
            Dim TempRegNo As String = ""
            TempRegNo = mDueJobPlanning.RegNo
            mnWO.WOText = Replace(TempRegNo, "VT-", "")
            If AppSettings("ClientCode") = "ADeccan" Then 'ADeccan Code Added by Saylee on 11-May-2018 for ADeccan11052018
                mnWO.WOText = mnWO.WOText + "/" + Today.Date.ToString("yy")
            End If
        ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
            mnWO.WOText = "MJO# " & CStr(CDate(mDueJobPlanning.FromDateFormatted).Date.Year) & " - " & mnWO.ModelName
        ElseIf AppSettings("ClientCode") = "TP" Then
            mnWO.WOText = Replace(mDueJobPlanning.RegNo, "VT-", "") & "/" & CStr(CDate(mDueJobPlanning.FromDateFormatted).Date.Year)
        End If


        tmpAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(mDueJobPlanning.FromDateFormatted.ToString, mnWO.MachineID.ToString, , , , , , , , , , True, , , , "Airframe", , , , , , , , , , , , , , , , , , True, SkipIsForInventoryAircarft:=True, MonitoringServiceRequired:=False, MonitoringModRequired:=False, MonitoringInspRequired:=False).Item(0), MachineInfo).AssemblyStatusList
        AssemblyStatusPeriodList = tmpAssemblyStatusList(tmpAssemblyStatusList.FirstItem.ID).AssemblyStatusPeriodList

        mnWO.WOPeriods.SetWOPeriods(mnWO.ID, AssemblyStatusPeriodList, mnWO.HourType)

        For i As Integer = 0 To mDueJobPlanning.DueJobPlanningItems.Count - 1
            If mnWO.WOJobs.Contains(mDueJobPlanning.DueJobPlanningItems.Item(i).MaintenanceActivityID, "") = False And mrptDueReport.Contains(mDueJobPlanning.DueJobPlanningItems(i).MaintenanceActivityID, "") Then
                Dim MaintenanceActivityID As Guid = mDueJobPlanning.DueJobPlanningItems.Item(i).MaintenanceActivityID
                mnWO.WOJobs.Add(mnWO.ID, 2)
                Dim Description As String = ""
                mnWO.WOJobs.CurrentItem.PreviousTransID = MaintenanceActivityID

                mnWO.WOJobs.CurrentItem.OnTypeID = mDueJobPlanning.DueJobPlanningItems.Item(i).OnTypeID
                mnWO.WOJobs.CurrentItem.MonitorTypeID = mDueJobPlanning.DueJobPlanningItems.Item(i).MonitorTypeID

                Description = mDueJobPlanning.DueJobPlanningItems.Item(i).Description

                mnWO.WOJobs.CurrentItem.WOJobDescription = Description

                mnWO.WOJobs.CurrentItem.WOMaintenanceEvent = Description
                mnWO.WOJobs.CurrentItem.Zone = mrptDueReport.Item(MaintenanceActivityID).Zone
                mnWO.WOJobs.CurrentItem.AREA = mrptDueReport.Item(MaintenanceActivityID).Area
                mnWO.WOJobs.CurrentItem.IsRII = mrptDueReport.Item(MaintenanceActivityID).IsRII
                mnWO.WOJobs.CurrentItem.DueAsOf = mrptDueReport.Item(MaintenanceActivityID).DueAsof2
                mnWO.WOJobs.CurrentItem.WOJobEstimatedTime = mrptDueReport.Item(MaintenanceActivityID).EstimatedHours

                If AppSettings("ShowCAMOOnlyForNewClients") = "True" And mDueJobPlanning.DueJobPlanningItems.Item(i).MonitorTypeID = 1 Then '"Servicing"
                    mnWO.WOJobs.CurrentItem.TaskCardNo = mrptDueReport.Item(MaintenanceActivityID).TaskNo
                    mnWO.WOJobs.CurrentItem.TaskSourceRef = mrptDueReport.Item(MaintenanceActivityID).SourceDoc
                    mnWO.WOJobs.CurrentItem.Publication = mrptDueReport.Item(MaintenanceActivityID).Reference
                    mnWO.WOJobs.CurrentItem.Skill = mrptDueReport.Item(MaintenanceActivityID).Skill
                    mnWO.WOJobs.CurrentItem.SkillID = mrptDueReport.Item(MaintenanceActivityID).SkillID
                ElseIf AppSettings("ShowCAMOOnlyForNewClients") = "True" And mDueJobPlanning.DueJobPlanningItems.Item(i).MonitorTypeID = 3 Then '"Modification"
                    mnWO.WOJobs.CurrentItem.TaskCardNo = mrptDueReport.Item(MaintenanceActivityID).Number
                    mnWO.WOJobs.CurrentItem.InspCode = mrptDueReport.Item(MaintenanceActivityID).Code
                    mnWO.WOJobs.CurrentItem.TaskSourceRef = mrptDueReport.Item(MaintenanceActivityID).Reference
                Else
                    mnWO.WOJobs.CurrentItem.InspCode = mrptDueReport.Item(MaintenanceActivityID).Code 'Added by Saylee on 18-Feb-2018 for ASH18022019 
                    mnWO.WOJobs.CurrentItem.TaskSourceRef = mrptDueReport.Item(MaintenanceActivityID).Reference
                End If

                If mrptDueReport.Item(MaintenanceActivityID).AssemblyTypeID = 1 Then
                    mnWO.WOJobs.CurrentItem.AssemblyTypeWithPosition = mrptDueReport.Item(MaintenanceActivityID).AssemblyTypeName
                Else
                    mnWO.WOJobs.CurrentItem.AssemblyTypeWithPosition = mrptDueReport.Item(MaintenanceActivityID).AssemblyTypeName + IIf(mrptDueReport.Item(MaintenanceActivityID).Position = "", "", "(" + mrptDueReport.Item(MaintenanceActivityID).Position + ")")
                End If



                With mnWO.WOJobs.CurrentItem
                    'Added By Kalpesh for Getting Task and Kit in W.O.---------------------
                    'TASK(s):
                    Dim mMaintenanceTask As MaintenanceTask
                    Dim mMaintenanceTaskDetail As MaintenanceTaskDetail

                    If .OnTypeID = 1 Then        'Assembly
                        mMaintenanceTask = MaintenanceTask.GetMaintenanceTaskForWO(.MonitorTypeID, .PreviousTransID, True)
                    ElseIf .OnTypeID = 2 Then    'Componant
                        mMaintenanceTask = MaintenanceTask.GetMaintenanceTaskForWO(.MonitorTypeID, .PreviousTransID, False)
                    End If

                    For Each mMaintenanceTaskDetail In mMaintenanceTask.MaintenanceTaskDetails
                        mnWO.WOJobs.CurrentItem.WOJobTasks.Add(mnWO.WOJobs.CurrentItem.ID)

                        With mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem
                            '.TaskAction = "No action taken." 'mMaintenanceTaskDetail.Task 'Commented By Prashant 12-Mar-2010
                            .TaskAction = ""  'Added By Prashant 12-Mar-2010
                            .ActualStartDate = mnWO.WOJobs.CurrentItem.WOJobStartDate
                            .ActualEndDate = mnWO.WOJobs.CurrentItem.WOJobStartDate
                            .IsDone = False
                            .TaskCardID = mMaintenanceTaskDetail.TaskCardID  'Added By Prashant 29-Dec-2008

                            'Added By Utkarsh On 27-Apr-2011

                            Dim mTaskCard As TaskCard
                            mTaskCard = TaskCard.GetTaskCard(mMaintenanceTaskDetail.TaskCardID)
                            .TaskCardNo = mTaskCard.TaskCardNo
                            .TaskDescription = mTaskCard.TaskDesc
                            .RevNo = mTaskCard.RevNo
                            .RevDate = mTaskCard.RevDate
                            .IssueDate = mTaskCard.IssueDate

                            ''Added by Saylee on 4-Feb-2013
                            ''If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo"  Then
                            ''    .Reference = mSelectDueJobs.Item(i).Reference
                            ''Else
                            ''    .Reference = mTaskCard.Reference
                            ''End If
                            '***************************
                            ''Commentedby Saylee on 15-Feb-2013
                            .Reference = mTaskCard.Reference

                            .Equipment = mTaskCard.Equipment
                            .Material = mTaskCard.Material
                            .EstimatedHours = mTaskCard.EstimatedHours
                            .checks = mTaskCard.Check
                            .RelatedTaskCardsNo = mTaskCard.RelatedTaskCardsNo
                            .ImageSize = mTaskCard.ImageSize
                            .ImageFile = mTaskCard.ImageFile
                            .FileExtension = mTaskCard.FileExtension

                            'Added by Vikrant on 06-Sept-2013 For BA04092013
                            Dim mTaskCardSpare As TaskCardSpare
                            Dim mTaskCardStepsSpare As TaskCardSpare

                            For Each mTaskCardSpare In mTaskCard.TaskCardSpares
                                If mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskSpares.Contains(mTaskCardSpare.ItemID) Then 'If Condition added by Vikrant On 28-Jun-2021 to solve BA issue
                                    mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskSpares(mTaskCardSpare.ItemID, "").RequiredQty += mTaskCardSpare.RequiredQty
                                Else 'existing condition
                                    mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskSpares.Add(mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.ID)
                                    With mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskSpares.CurrentItem
                                        .ItemID = mTaskCardSpare.ItemID
                                        .RequiredQty = mTaskCardSpare.RequiredQty
                                        .PartNo = mTaskCardSpare.PartNo
                                        .Description = mTaskCardSpare.Description
                                        .Remark = mTaskCardSpare.Remark
                                        .OnSerialNo = mTaskCardSpare.OnSerialNo
                                        .OffSerialNo = mTaskCardSpare.OffSerialNo
                                        .IsForSteps = False
                                    End With
                                End If
                            Next

                            For Each mTaskCardStepsSpare In mTaskCard.TaskCardStepsSpares
                                If mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskStepsSpares.Contains(mTaskCardStepsSpare.ItemID) Then 'If Condition added by Vikrant On 28-Jun-2021 to solve BA issue
                                    mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskStepsSpares(mTaskCardStepsSpare.ItemID, "").RequiredQty += mTaskCardStepsSpare.RequiredQty
                                Else 'existing condition
                                    mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskStepsSpares.Add(mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.ID)
                                    With mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskStepsSpares.CurrentItem
                                        .ItemID = mTaskCardStepsSpare.ItemID
                                        .RequiredQty = mTaskCardStepsSpare.RequiredQty
                                        .PartNo = mTaskCardStepsSpare.PartNo
                                        .Description = mTaskCardStepsSpare.Description
                                        .Remark = mTaskCardStepsSpare.Remark
                                        .OnSerialNo = mTaskCardStepsSpare.OnSerialNo
                                        .OffSerialNo = mTaskCardStepsSpare.OffSerialNo
                                        .IsForSteps = True
                                    End With
                                End If
                            Next
                            'End
                            'Added By Vikrant on 03-Mar-2020 For ALL03032020
                            For Each mTaskCardSpare In mTaskCard.TaskCardPartRemovals
                                If mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskPartRemovals.Contains(mTaskCardSpare.ItemID) Then 'If Condition added by Vikrant On 28-Jun-2021 to solve BA issue
                                    mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskPartRemovals(mTaskCardSpare.ItemID, "").RequiredQty += mTaskCardSpare.RequiredQty
                                Else 'existing condition
                                    mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskPartRemovals.Add(mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.ID)
                                    With mnWO.WOJobs.CurrentItem.WOJobTasks.CurrentItem.WOJobTaskPartRemovals.CurrentItem
                                        .ItemID = mTaskCardSpare.ItemID
                                        .RequiredQty = mTaskCardSpare.RequiredQty
                                        .PartNo = mTaskCardSpare.PartNo
                                        .Description = mTaskCardSpare.Description
                                        .Remark = mTaskCardSpare.Remark
                                        .OnSerialNo = mTaskCardSpare.OnSerialNo
                                        .OffSerialNo = mTaskCardSpare.OffSerialNo
                                        .IsForSteps = False
                                        .IsPartRemoval = True
                                        .Position = mTaskCardSpare.Position
                                    End With
                                End If
                            Next
                            'End
                        End With
                    Next

                    'KIT(s):
                    Dim mMaintenanceKit As MaintenanceKit

                    If .OnTypeID = 1 Then        'Assembly
                        mMaintenanceKit = MaintenanceKit.GetMaintenanceKitForWO(.MonitorTypeID, .PreviousTransID, True)
                    ElseIf .OnTypeID = 2 Then    'Componant
                        mMaintenanceKit = MaintenanceKit.GetMaintenanceKitForWO(.MonitorTypeID, .PreviousTransID, False)
                    End If
                    'Commented and Added by Saylee on 23-July-2013 for BA22072013 	
                    ''''For Each mMaintenanceKitDetail In mMaintenanceKit.MaintenanceKitDetails
                    ''''    mnWO.WOJobs.CurrentItem.WOJobSpares.Add(mnWO.WOJobs.CurrentItem.ID)

                    ''''    With mnWO.WOJobs.CurrentItem.WOJobSpares.CurrentItem
                    ''''        .ItemID = mMaintenanceKitDetail.ItemID
                    ''''        .RequiredQty = mMaintenanceKitDetail.Qty
                    ''''        Dim mItem As Item = Item.GetItem(mMaintenanceKitDetail.ItemID)
                    ''''        .PartNo = mItem.Name
                    ''''        .Description = mItem.Description
                    ''''        mItem = Nothing
                    ''''    End With
                    ''''Next
                    '''''-----------------------------------------------------------------------
                    'Added by Saylee on 23-July-2013 for BA22072013 	
                    Dim mMaintenanceSpares As MaintenanceKit
                    Dim mMaintenanceSparesDetail As MaintenanceKitDetail

                    Dim mMaintenanceTools As MaintenanceKit
                    Dim mMaintenanceToolsDetail As MaintenanceKitDetail

                    If .OnTypeID = 1 Then        'Assembly
                        mMaintenanceSpares = MaintenanceKit.GetMaintenanceKitForWO(.MonitorTypeID, .PreviousTransID, True, False)
                        mMaintenanceTools = MaintenanceKit.GetMaintenanceKitForWO(.MonitorTypeID, .PreviousTransID, True, True)
                    ElseIf .OnTypeID = 2 Then    'Componant
                        mMaintenanceSpares = MaintenanceKit.GetMaintenanceKitForWO(.MonitorTypeID, .PreviousTransID, False, False)
                        mMaintenanceTools = MaintenanceKit.GetMaintenanceKitForWO(.MonitorTypeID, .PreviousTransID, False, True)
                    End If

                    For Each mMaintenanceSparesDetail In mMaintenanceSpares.MaintenanceKitDetails
                        If mnWO.WOJobs.CurrentItem.WOJobSpares.Contains(mMaintenanceSparesDetail.ItemID, "") Then 'If Condition added by Vikrant On 28-Jun-2021 to solve BA issue
                            mnWO.WOJobs.CurrentItem.WOJobSpares(mMaintenanceSparesDetail.ItemID).RequiredQty += mMaintenanceSparesDetail.Qty
                        Else 'existing condition
                            mnWO.WOJobs.CurrentItem.WOJobSpares.Add(mnWO.WOJobs.CurrentItem.ID)

                            With mnWO.WOJobs.CurrentItem.WOJobSpares.CurrentItem
                                .ItemID = mMaintenanceSparesDetail.ItemID
                                .RequiredQty = mMaintenanceSparesDetail.Qty
                                Dim mItem As Item = Item.GetItem(mMaintenanceSparesDetail.ItemID)
                                .PartNo = mItem.Name
                                .Description = mItem.Description
                                mItem = Nothing
                                .Remark = mMaintenanceSparesDetail.Remark 'Added By Vikrant On 04-Apr-2014 For ALL04042014
                            End With
                        End If

                    Next

                    For Each mMaintenanceToolsDetail In mMaintenanceTools.MaintenanceKitDetails
                        If Not mnWO.WOTools.Contains(mMaintenanceToolsDetail.ItemID) Then
                            mnWO.WOTools.Add(mnWO.ID)

                            With mnWO.WOTools.CurrentItem
                                .ItemID = mMaintenanceToolsDetail.ItemID
                                .RequiredQty = mMaintenanceToolsDetail.Qty
                                Dim mItem As Item = Item.GetItem(mMaintenanceToolsDetail.ItemID)
                                .PartNo = mItem.Name
                                .Description = mItem.Description
                                mItem = Nothing
                                .WOToolRemark = mMaintenanceToolsDetail.Remark 'Added By Vikrant On 04-Apr-2014 For ALL04042014
                            End With
                        Else
                            mnWO.WOTools.CurrentIndex = mnWO.WOTools(mMaintenanceToolsDetail.ItemID, "").SrNo - 1
                            If mnWO.WOTools(mMaintenanceToolsDetail.ItemID, "").RequiredQty = 0 Then

                            Else
                                If (mnWO.WOTools(mMaintenanceToolsDetail.ItemID, "").RequiredQty <= mMaintenanceToolsDetail.Qty) Or (mMaintenanceToolsDetail.Qty = 0) Then
                                    With mnWO.WOTools.CurrentItem
                                        .ItemID = mMaintenanceToolsDetail.ItemID
                                        .RequiredQty = mMaintenanceToolsDetail.Qty
                                        Dim mItem As Item = Item.GetItem(mMaintenanceToolsDetail.ItemID)
                                        .PartNo = mItem.Name
                                        .Description = mItem.Description
                                        mItem = Nothing
                                        .WOToolRemark = mMaintenanceToolsDetail.Remark 'Added By Vikrant On 04-Apr-2014 For ALL04042014
                                    End With
                                End If
                            End If
                        End If
                    Next
                    '-----------------------------------------------------------------------
                End With

            End If


        Next
        Session("mnWO") = mnWO

        Dim URLFromDueReportPreview As Stack = New Stack
        URLFromDueReportPreview.Push(Request.Url)
        Session("wfDueJobPlanning_Ajax") = "wfDueJobPlanning_Ajax"
        Session("URLFromDueReportPreview") = URLFromDueReportPreview
        Dim str As String
        str = "openledgersame('wfnWODetail_Ajax.aspx?BackPage=index.aspx');"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", str, True)
    End Sub


#End Region

End Class