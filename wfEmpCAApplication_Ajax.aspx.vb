Imports System.Collections.Generic
Imports javax.transaction
Imports System.Text
Imports Image = System.Web.UI.WebControls.Image
Imports System.Text.RegularExpressions
Imports System.Security.Authentication.ExtendedProtection

Public Class wfEmpCAApplication_Ajax
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
    Public mEmpCAAuthorization As EmpCAAuthorization
    Public mEmpCAAuthorizationDetail As EmpCAAuthorizationDetail
    Public mEmpCAAuthorizationList As EmpCAAuthorizationList
    Public mEmployeeList As EmployeeList
    Dim EventLogID As Guid
    Dim mEmpCAAuthorizationDetail1 As String
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    Dim IsReopen As Boolean = False
    Dim mUser As User
    Dim email As Thread
    Dim mCAAuthorizationScopeList As CAAuthorizationScopeList
    Dim mCALimitationList As CALimitationList
    Dim mModuleList As ModuleList
    Public CAType As Integer = 0
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mEmpCAAuthorization = CType(Session("mEmpCAAuthorization"), EmpCAAuthorization)
        mEmployeeList = CType(Session("mEmployeeList"), EmployeeList)
        mFileAttach = Session("mFileAttachEmpCAAuthorization")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        mModuleList = Session("mModuleList") 'Added by Sachin
        mEmpCAAuthorizationList = Session("mEmpCAAuthorizationList")
        CAType = Session("CAType")
    End Sub
    Private Sub SetSession()
        Session("mEmpCAAuthorization") = mEmpCAAuthorization
        Session("mEmployeeList") = mEmployeeList
        Session("mFileAttachEmpCAAuthorization") = mFileAttach
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
        'End
    End Sub
    Private Sub RemoveSessions()
        Session.Remove("mEmpCAAuthorization")
        Session.Remove("mEmployeeList")
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        Session.Remove("mFileAttachEmpCAAuthorization")
        'End
    End Sub

    Private Sub Save(Optional CAStatusID As Integer = 0)
        'Authentication
        If Not mEmpCAAuthorization.EmpCAAuthorizationDate Is System.DBNull.Value Then
            Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
            If mCheck.WebAuthentication = True Then
                Dim mDays As Integer = 0
                mDays = mCheck.Number("Days")
                Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
                If DateDiff(DateInterval.Day, CDate(mEmpCAAuthorization.EmpCAAuthorizationDate), maxAllowableDate) < 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Goods Receipt. <br> Goods Receipt Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            End If
        End If
        Dim EmpCAAuthorizationClone As EmpCAAuthorization
        EmpCAAuthorizationClone = mEmpCAAuthorization.Clone
        Try

            'check whether min. one item & charge is present while saving
            If Not mEmpCAAuthorization.EmpCAAuthorizationDetails.Count = 0 Then
                'save the object

                SetStatusDetails(CAStatusID)

                'SetStatusDetails(CAStatusID)
                SetObject()
                setObjectAuthorizationDetail()

                If mEmpCAAuthorization.IsValid Then
                    Dim i As Integer
                    While i < mEmpCAAuthorization.EmpCAAuthorizationDetails.Count
                        i = i + 1
                    End While
                    mEmpCAAuthorization.ApplyEdit()

                    mEmpCAAuthorization.Save()

                    SaveAttachment()

                    'mEmpCAAuthorizationDetail1 = "Authorization No.: " + mEmpCAAuthorization.CANumber + " Dated: " + mEmpCAAuthorization.EmpCAAuthorizationDateFormatted + " Employee: " + mEmpCAAuthorization.EmployeeName
                    mEmpCAAuthorizationDetail1 = " Dated: " + mEmpCAAuthorization.EmpCAAuthorizationDateFormatted + " Employee: " + mEmpCAAuthorization.EmployeeName
                    MarkLog(Util.Action.Save, "EmpCAAuthorization", mEmpCAAuthorizationDetail1, Util.ErrorType.NoError, mEmpCAAuthorization.ID, EventLogID)
                    mEmpCAAuthorization.MarkClean()
                    Session("mEmpCAAuthorization") = mEmpCAAuthorization
                    DataFieldBind()

                    SetGrid()
                    SetControl()
                    ControlVisibility()
                    ControlVisibilityForFileAttachment()
                    SetControlStatus(mEmpCAAuthorization.StatusID, mEmpCAAuthorization.CAStatusID, CAType)

                    If mEmpCAAuthorization.IsNew Then
                        lblTitle.Text = "Company Authorization [New]"
                    Else
                        lblTitle.Text = "Company Authorization"

                    End If
                    upnlTitle.Update()
                    upnlEmpCAAuthorizationDetails.Update()
                    'upnlEmpCAAuthorization.Update()
                    upnlEmpCAAuthorizationDetail.Update()
                    upnlEmpCAAuthorizationTerms.Update()
                    upnlButtons.Update()
                    upnlStatusName.Update()
                    upnlEmpCAStatus.Update()

                    MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                Else
                    Dim mRule As String = ""
                    If mEmpCAAuthorization.GetBrokenRulesCollection.Count > 0 Then
                        mRule = mEmpCAAuthorization.GetBrokenRulesCollection(0).Description
                    ElseIf mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.GetBrokenRulesCollection.Count > 0 Then
                        mRule = mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.GetBrokenRulesCollection(0).Description
                    End If
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, mRule, MsgBoxStyle.OkOnly, "")
                    mRule = ""
                    mEmpCAAuthorization = EmpCAAuthorizationClone
                    SetObject()
                    setObjectAuthorizationDetail()
                    Session("mEmpCAAuthorization") = mEmpCAAuthorization
                    DataFieldBind()
                    Exit Sub
                End If
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Company Authorization can not be saved without details.", MsgBoxStyle.OkOnly, "")
                mEmpCAAuthorization = EmpCAAuthorizationClone
                SetObject()
                setObjectAuthorizationDetail()
                Session("mEmpCAAuthorization") = mEmpCAAuthorization
                DataFieldBind()
                Exit Sub
            End If
        Catch ex As SqlException
            Session("EmpCAAuthorizationClone") = EmpCAAuthorizationClone
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
                mEmpCAAuthorization = EmpCAAuthorizationClone
                SetObject()
                Session("mEmpCAAuthorization") = mEmpCAAuthorization
                DataFieldBind()
                Exit Sub
            ElseIf InStr(ex1.Message, "CCtabOrderItemReceiptBalanceQty", CompareMethod.Text) Then
                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Goods Receipt Qty can not be greater than Order Qty.</br><b>Please amend Purchase Order Quantity & make Goods Receipt again.</b>", MsgBoxStyle.OkOnly, "")
            Else
                MSGBoxCtrl.Show("Alert!", "Save Alert ! " + "</br>" + "There is some problem in Saving Goods Receipt. <BR> <BR>  Please Check the Entry and Try Again  !", "", MsgBoxStyle.OkOnly, "Status")
                mEmpCAAuthorization = EmpCAAuthorizationClone
                SetObject()
                Session("mEmpCAAuthorization") = mEmpCAAuthorization
                DataFieldBind()
                Exit Sub
            End If
            mEmpCAAuthorization = EmpCAAuthorizationClone
            Session("mEmpCAAuthorization") = mEmpCAAuthorization
        Finally
            EmpCAAuthorizationClone = Nothing
        End Try
    End Sub

    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub SetObject()
        If txtEmpCAAuthorizationDate.Text = "" Then
            mEmpCAAuthorization.EmpCAAuthorizationDate = Today.Date
        Else
            mEmpCAAuthorization.EmpCAAuthorizationDate = CDate(txtEmpCAAuthorizationDate.Text)
        End If
        mEmpCAAuthorization.Text = txtEmpCAAuthorizationText.Text.Trim
        mEmpCAAuthorization.No = Val(txtEmpCAAuthorizationNo.Text)
        mEmpCAAuthorization.CANumber = Trim(txtCANo.Text)
        mEmpCAAuthorization.RevisionNo = Trim(txtRevisionNo.Text)

        If (CAType < 2) Then
            mEmpCAAuthorization.EmployeeID = New Guid(cmbEmployee.SelectedValue)
            mEmpCAAuthorization.EmployeeCode = txtEmployeeCode.Text.Trim
        End If


        If txtFromDate.Text = "" Then
            mEmpCAAuthorization.CAInitialIssueDate = System.DBNull.Value
        Else
            mEmpCAAuthorization.CAInitialIssueDate = CDate(txtFromDate.Text)
        End If
        If txtToDate.Text = "" Then
            mEmpCAAuthorization.CAValidUpto = System.DBNull.Value
        Else
            mEmpCAAuthorization.CAValidUpto = CDate(txtToDate.Text)
        End If
        mEmpCAAuthorization.Remark = Trim(txtRemark.Text)

        If (CAType < 2) Then
            mEmpCAAuthorization.AMELCat = txtAMELCat.Text.Trim
        End If

        mEmpCAAuthorization.RevisionNo = txtRevisionNo.Text
        If txtRevisionDate.Text = "" Then
            mEmpCAAuthorization.RevisionDate = System.DBNull.Value
        Else
            mEmpCAAuthorization.RevisionDate = CDate(txtRevisionDate.Text)
        End If

        If (CAType < 2) Then
            mEmpCAAuthorization.AMELNo = txtAMELNo.Text
        End If

        If txtDateOfExpiry.Text = "" Then
            mEmpCAAuthorization.DateOfExpiry = System.DBNull.Value
        Else
            mEmpCAAuthorization.DateOfExpiry = CDate(txtDateOfExpiry.Text)
        End If

        If txtContinuationTrainingValidity.Text = "" Then
            mEmpCAAuthorization.ContinuationTrainingValidity = System.DBNull.Value
        Else
            mEmpCAAuthorization.ContinuationTrainingValidity = CDate(txtContinuationTrainingValidity.Text)
        End If

        If txtMeetingDate.Text = "" Then
            mEmpCAAuthorization.ScheduleMeetingDate = System.DBNull.Value
        Else
            'mEmpCAAuthorization.ScheduleMeetingDate = CDate(txtMeetingDate.Text)
            mEmpCAAuthorization.ScheduleMeetingDate = CType(txtMeetingDate.Text.ToString.Trim + " " + txtSchTime.Text.ToString.Trim, DateTime)
            'Dim MeetDatetime As String = txtMeetingDate.Text.ToString + " " + txtSchTime.Text.ToString.Trim
            'mEmpCAAuthorization.ScheduleMeetingDate = MeetDatetime
        End If

        'mEmpCAAuthorization.Remark = Trim(txtRemark.Text)
        mEmpCAAuthorization.Participants = Trim(txtParticipants.Text)
        mEmpCAAuthorization.MeetingMinutes = Trim(txtMeetingMinutes.Text)

    End Sub
    Private Sub SetStatusDetails(CAStatusID As Integer)
        If Not mEmpCAAuthorization.CAStatusChilds.Contains(CAStatusID:=CAStatusID) Then
            mEmpCAAuthorization.CAStatusChilds.Add(mEmpCAAuthorization.ID)
            mEmpCAAuthorization.CAStatusChilds.CurrentItem.UserID = SI.UTILITY.User.GetUser(User.Identity.Name).UserID
            mEmpCAAuthorization.CAStatusChilds.CurrentItem.UserName = SI.UTILITY.User.GetUser(User.Identity.Name).Name
            mEmpCAAuthorization.CAStatusChilds.CurrentItem.CAStatusID = CAStatusID
            If CAStatusID = 1 Then
                If txtEmpCAAuthorizationDate.Text <> "" Then
                    mEmpCAAuthorization.CAStatusChilds.CurrentItem.CAStatusDate = CDate(txtEmpCAAuthorizationDate.Text)
                End If
            Else
                mEmpCAAuthorization.CAStatusChilds.CurrentItem.CAStatusDate = Today.Date
            End If

            'mEmpCAAuthorization.CAStatusChilds.CurrentItem.CAStatusDate = Today.Date

        ElseIf IsReopen = True And mEmpCAAuthorization.CAStatusChilds.Contains(CAStatusID:=CAStatusID) Then
            mEmpCAAuthorization.CAStatusChilds.CurrentIndex = mEmpCAAuthorization.CAStatusChilds.Count - 1
            'mEmpCAAuthorization.CAStatusChilds(CAStatusID, "").UserID = SI.UTILITY.User.GetUser(User.Identity.Name).UserID
            'mEmpCAAuthorization.CAStatusChilds(CAStatusID, "").UserName = SI.UTILITY.User.GetUser(User.Identity.Name).Name
            'mEmpCAAuthorization.CAStatusChilds(CAStatusID, "").CAStatusID = CAStatusID
            'mEmpCAAuthorization.CAStatusChilds(CAStatusID, "").CAStatusDate = Today.Date

            mEmpCAAuthorization.CAStatusChilds.CurrentItem.UserID = SI.UTILITY.User.GetUser(User.Identity.Name).UserID
            mEmpCAAuthorization.CAStatusChilds.CurrentItem.UserName = SI.UTILITY.User.GetUser(User.Identity.Name).Name
            mEmpCAAuthorization.CAStatusChilds.CurrentItem.CAStatusID = CAStatusID
            mEmpCAAuthorization.CAStatusChilds.CurrentItem.CAStatusDate = Today.Date
        End If
        mEmpCAAuthorization.CAStatusID = CAStatusID
        Session("mEmpCAAuthorization") = mEmpCAAuthorization

    End Sub
    Private Sub setObjectAuthorizationDetail()
        Dim mEmpCAAuthorizationClone As EmpCAAuthorization
        mEmpCAAuthorizationClone = mEmpCAAuthorization.Clone
        Try
            Dim child As EmpCAAuthorizationDetail
            Dim ID As Guid
            For i As Integer = 0 To dgEmpCAAuthorizationDetail.Rows.Count - 1
                ID = New Guid(dgEmpCAAuthorizationDetail.DataKeys(i).Values("ID").ToString)
                child = mEmpCAAuthorization.EmpCAAuthorizationDetails.Item(ID)
                Dim txtAuthorizationDetails As TextBox = TryCast(dgEmpCAAuthorizationDetail.Rows(i).FindControl("txtAuthorizationDetails"), TextBox)
                Dim txtLimitations As TextBox = TryCast(dgEmpCAAuthorizationDetail.Rows(i).FindControl("txtLimitations"), TextBox)
                Dim txtRevNo As TextBox = TryCast(dgEmpCAAuthorizationDetail.Rows(i).FindControl("txtRevNo"), TextBox)
                Dim txtRevDate As TextBox = TryCast(dgEmpCAAuthorizationDetail.Rows(i).FindControl("txtRev"), TextBox)


                child.AuthorizationDetails = txtAuthorizationDetails.Text.Trim
                child.LimitationsDetails = txtLimitations.Text.Trim
                child.RevNo = txtRevNo.Text.Trim
                If txtRevDate.Text = "" Then
                    child.RevDate = System.DBNull.Value
                Else
                    child.RevDate = CDate(txtRevDate.Text)
                End If
            Next
        Catch ex As Exception

        End Try
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "Delete")
        mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentIndex = Index
        Session("mEmpCAAuthorization") = mEmpCAAuthorization
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            mEmpCAAuthorization = CType(Session("mEmpCAAuthorization"), EmpCAAuthorization)
                            mEmpCAAuthorization.EmpCAAuthorizationDetails.Remove(mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem)
                            mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentIndex = mEmpCAAuthorization.EmpCAAuthorizationDetails.Count - 1
                            dgEmpCAAuthorizationDetail.DataSource = mEmpCAAuthorization.EmpCAAuthorizationDetails
                            dgEmpCAAuthorizationDetail.DataBind()
                            SetGrid()
                            SetControl()
                            ControlVisibility()
                            upnlEmpCAAuthorizationDetail.Update()
                            Session("mEmpCAAuthorization") = mEmpCAAuthorization
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "Status" Then
                        Session("sender") = ""
                        If mEmpCAAuthorization.IsValid = True Then
                            Session.Remove("IsValid")
                            mEmpCAAuthorization.StatusID = 2
                            Save()
                            Session.Remove("IsValid")
                        Else
                            If CustomValidate1() = False Then
                                upnlValidationsummary.Update()
                                Exit Sub
                            End If
                        End If
                    End If

                    If MSGBoxCtrl.Sender = "CAStatus" Then
                        Session("sender") = ""
                        If mEmpCAAuthorization.IsValid = True Then
                            Session.Remove("IsValid")
                            'mEmpCAAuthorization.CAStatusID = 2
                            'SetStatusDetails(2)
                            'mEmpCAAuthorization.CAStatusID = 2
                            'Session("mEmpCAAuthorization") = mEmpCAAuthorization
                            Save(2)
                            'mEmpCAAuthorization.CAStatusID = 2
                            'Session("mEmpCAAuthorization") = mEmpCAAuthorization
                            MarkLog(Util.Action.Save, "EmpCAAuthorization", "Applied By Employee " + mEmpCAAuthorization.EmployeeName + " on " + Today.Date, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            Session.Remove("IsValid")
                        Else
                            If CustomValidate1() = False Then
                                upnlValidationsummary.Update()
                                Exit Sub
                            End If
                        End If
                    End If


                    If MSGBoxCtrl.Sender = "CAStatusValidate" Then
                        Session("sender") = ""
                        If mEmpCAAuthorization.IsValid = True Then
                            Session.Remove("IsValid")
                            'mEmpCAAuthorization.CAStatusID = 3
                            SetStatusDetails(3)
                            Save(3)
                            MarkLog(Util.Action.Save, "EmpCAAuthorization", "Validated By Employee " + SI.UTILITY.User.GetUser(User.Identity.Name).EmployeeName + " on " + Today.Date, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            Session.Remove("IsValid")
                            upnlButtons.Update()
                            pnlScheduleMeet.Visible = True
                            upnlScheduleMeeting.Update()
                            mdlPopUpScheduleMeeting.Show()


                        Else
                            If CustomValidate1() = False Then
                                upnlValidationsummary.Update()
                                Exit Sub
                            End If
                        End If
                    End If

                    If MSGBoxCtrl.Sender = "CAStatusApprove" Then
                        Session("sender") = ""
                        If mEmpCAAuthorization.IsValid = True Then
                            Session.Remove("IsValid")
                            'mEmpCAAuthorization.CAStatusID = 3
                            SetStatusDetails(5)
                            Save(5)

                            MarkLog(Util.Action.Save, "EmpCAAuthorization", "Approved By Employee " + SI.UTILITY.User.GetUser(User.Identity.Name).EmployeeName + " on " + Today.Date + " CA Authorization no " + mEmpCAAuthorization.CANumber, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            Session.Remove("IsValid")
                        Else
                            If CustomValidate1() = False Then
                                upnlValidationsummary.Update()
                                Exit Sub
                            End If
                        End If
                    End If


                    If MSGBoxCtrl.Sender = "CAStatusReopen" Then
                        Session("sender") = ""
                        If mEmpCAAuthorization.IsValid = True Then
                            Session.Remove("IsValid")
                            IsReopen = True
                            'mEmpCAAuthorization.CAStatusID = 3
                            'mEmpCAAuthorization.CAStatusChilds.Remove(mEmpCAAuthorization.ID)
                            mEmpCAAuthorization.CAStatusChilds.Remove(mEmpCAAuthorization.CAStatusChilds.Count - 1)
                            'mEmpCAAuthorization.CAStatusID = 1
                            Session("mEmpCAAuthorization") = mEmpCAAuthorization
                            Save(1)
                            MarkLog(Util.Action.Save, "EmpCAAuthorization", "Reopened By Employee " + SI.UTILITY.User.GetUser(User.Identity.Name).EmployeeName + " for employee " + mEmpCAAuthorization.EmployeeName + " on " + Today.Date, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                            Session.Remove("IsValid")
                            Response.Redirect("Index.aspx")
                            'Response.Redirect("wfEmpCAAuthorizationList_Ajax.aspx?CAType=" & Val(Request.QueryString("CAType")).ToString)
                        Else
                            If CustomValidate1() = False Then
                                upnlValidationsummary.Update()
                                Exit Sub
                            End If
                        End If
                    End If



                    If MSGBoxCtrl.Sender = "Close" Then

                        If mEmpCAAuthorization.EmpCAAuthorizationDetails.Count = 0 Then
                            MSGBoxCtrl.Show("Alert!", "Authorization cannot be Saved without Company Authorization Details ", "", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End If

                        If CustomValidateAuthorization() = False Then upnlValidationsummary.Update() : Exit Sub
                        If mEmpCAAuthorization.IsValid = True Then
                            Session.Remove("IsValid")

                            If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                                Exit Sub
                            End If
                            If mEmpCAAuthorization.IsNew Then
                                Save(1)
                            Else
                                Save(mEmpCAAuthorization.CAStatusID)
                            End If
                            DataFieldBind()
                            'Response.Redirect("wfEmpCAAuthorizationList_Ajax.aspx?CAType=" & Val(Request.QueryString("CAType")).ToString)
                            'Response.Redirect("wfEmpCAAuthorizationList_Ajax.aspx?CAType=1")
                            Response.Redirect("Index.aspx")
                        Else
                            Session.Remove("IsValid")
                            If CustomValidate1() = False Then
                                upnlValidationsummary.Update()
                                Exit Sub
                            End If
                        End If
                    End If
                    If MSGBoxCtrl.Sender = "DeleteDetailAttachment" Then
                        mFileAttach = FileAttach.GetAttachment(mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.ID)
                        mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.FileAttachments.CurrentIndex = 0
                        mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.FileAttachments.Remove(mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.FileAttachments.CurrentItem)
                        mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.IsAttachmentAdded = False
                        dgEmpCAAuthorizationDetail.DataSource = mEmpCAAuthorization.EmpCAAuthorizationDetails
                        dgEmpCAAuthorizationDetail.DataBind()
                        SetGrid()
                        SetControl()
                        ControlVisibility()
                        upnlEmpCAAuthorizationDetail.Update()
                    End If
                    If MSGBoxCtrl.Sender = "DeleteTerm" Then
                        Try
                            Session("Sender") = ""
                            Dim mEmpCAAuthorization As EmpCAAuthorization
                            mEmpCAAuthorization = CType(Session("mEmpCAAuthorization"), EmpCAAuthorization)
                            mEmpCAAuthorization.EmpCAAuthorizationTerms.Remove(mEmpCAAuthorization.EmpCAAuthorizationTerms.CurrentItem)
                            Session("mEmpCAAuthorization") = mEmpCAAuthorization
                            dgEmpCAAuthorizationTerms.DataSource = mEmpCAAuthorization.EmpCAAuthorizationTerms
                            dgEmpCAAuthorizationTerms.DataBind()
                            upnlEmpCAAuthorizationTerms.Update()
                        Catch ex As SqlException
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(ex.Message, False), True)
                            Exit Sub
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session.Remove("IsValid")
                        Session.Remove("mModuleName")
                        Session.Remove("mPendingItemList")
                        Session.Remove("mEmpCAAuthorization")
                        Session("Sender") = ""
                        Response.Redirect("Index.aspx")
                    End If

                Case MsgBoxResult.Ok
                    'If MSGBoxCtrl.Sender = "RCITransTextSeriesAlert" Then
                    '    Session("sender") = ""
                    '    Session("AddTransTextSeries") = "True"
                    '    Response.Redirect("wfTransTextSeries_Ajax.aspx?OpenFrmLnk=0")
                    'End If
                    If MSGBoxCtrl.Sender = "Status" Then
                        Session("sender") = ""
                        If mEmpCAAuthorization.StatusID = 2 Then
                            mEmpCAAuthorization.StatusID = 1

                        End If
                        Session("mEmpCAAuthorization") = mEmpCAAuthorization
                        DataFieldBind()
                    End If
            End Select
        End If
    End Sub
    Private Sub SetReceivedFromDetails(ByVal ToType As Int16)

    End Sub
    Private Sub addAttributes()
        txtEmpCAAuthorizationNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtEmpCAAuthorizationNo').value,event)")
    End Sub

    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = ""
        If CAType = 1 Then
            IsInRoleString = "EmpCAApplyForCA"
        ElseIf CAType = 2 Then
            IsInRoleString = "EmpCAValidateForCA"
        ElseIf CAType = 3 Then
            IsInRoleString = "EmpCAApproveForCA"
        Else
            IsInRoleString = "EmpCAAuthorization"
        End If
        'Dim IsInRoleString As String = "EmpCAAuthorization"
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
    Private Sub ControlVisibility()
        cmbEmployee.Enabled = False
        txtEmployeeCode.Enabled = False
        txtAMELNo.Enabled = False
        txtAMELCat.Enabled = False
        btnAuthorized.Visible = (Not mEmpCAAuthorization.EmpCAAuthorizationDetails.Count = 0) And (Not mEmpCAAuthorization.IsNew) And (mEmpCAAuthorization.CAStatusID = 1) And (CAType = 0)
        ImgDetailsAdd.Enabled = IIf(mEmpCAAuthorization.CAStatusID > 1, False, True)
        ImgDetailsAddBottom.Visible = IIf(mEmpCAAuthorization.CAStatusID = 1 And mEmpCAAuthorization.EmpCAAuthorizationDetails.Count > 5, True, False)
        imgTermsAdd.Visible = IIf(mEmpCAAuthorization.CAStatusID = 4, True, False)
        btnReopen.Visible = IIf(CAType = 2 And mEmpCAAuthorization.CAStatusID = 2, True, False)
        btnvalidate.Visible = IIf(CAType = 2 And mEmpCAAuthorization.CAStatusID = 2, True, False)
        'pnlCAAuthorizationDetails.Enabled = IIf(mEmpCAAuthorization.StatusID > 1, False, True)
        txtEmpCAAuthorizationDate.Enabled = IIf(mEmpCAAuthorization.CAStatusID > 1, False, True)
        txtEmpCAAuthorizationText.Enabled = IIf(mEmpCAAuthorization.CAStatusID > 1, False, True)
        txtEmpCAAuthorizationNo.Enabled = IIf(mEmpCAAuthorization.CAStatusID > 1, False, True)
        '   txtAMELNo.Enabled = IIf(mEmpCAAuthorization.StatusID > 1, False, True)
        '  txtAMELCat.Enabled = IIf(mEmpCAAuthorization.StatusID > 1, False, True)

        If mEmpCAAuthorization.CAStatusID > 1 Or Session("IsEmpCAAuthorizationForRenew") = "True" Then
            cmbEmployee.Enabled = False
            txtEmployeeCode.Enabled = False
            txtCANo.Enabled = False
            txtAMELNo.Enabled = False
            txtAMELCat.Enabled = False
            txtContinuationTrainingValidity.Enabled = False
        Else
            txtCANo.Enabled = True
            txtContinuationTrainingValidity.Enabled = True
        End If


        txtDateOfExpiry.Enabled = IIf(mEmpCAAuthorization.CAStatusID > 1, False, True)
        txtRevisionNo.Enabled = IIf(mEmpCAAuthorization.CAStatusID > 1, False, True)
        txtRevisionDate.Enabled = IIf(mEmpCAAuthorization.CAStatusID > 1, False, True)
        txtFromDate.Enabled = IIf(mEmpCAAuthorization.CAStatusID > 1, False, True)
        txtToDate.Enabled = IIf(mEmpCAAuthorization.CAStatusID > 1, False, True)
        'pnlCAAuthorizationDetails.Enabled = IIf(mEmpCAAuthorization.CAStatusID > 1, False, True)
        'btnDelAttach.Enabled = IIf(mEmpCAAuthorization.CAStatusID > 1, False, True)
        'ImageButton1.Enabled = IIf(mEmpCAAuthorization.CAStatusID > 1, False, True)
        'txtRemark.Enabled = IIf(mEmpCAAuthorization.CAStatusID > 1, False, True)

        '' pnlCompanyDetails.Enabled = IIf(mEmpCAAuthorization.CAStatusID > 1, False, True)
        pnlTermsDetails.Enabled = IIf(mEmpCAAuthorization.CAStatusID > 3, True, False)
        'dgEmpCAAuthorizationTerms.Columns(2).Visible = IIf(mEmpCAAuthorization.CAStatusID > 1, False, True)
        For j As Integer = 0 To mEmpCAAuthorization.EmpCAAuthorizationDetails.Count - 1
            Dim DeleteRecord As ImageButton = CType(dgEmpCAAuthorizationDetail.Rows.Item(j).Cells(10).FindControl("DeleteRecord"), ImageButton)
            DeleteRecord.Visible = IIf(mEmpCAAuthorization.CAStatusID > 1, False, True)

            Dim View As ImageButton = CType(dgEmpCAAuthorizationDetail.Rows.Item(j).Cells(10).FindControl("View"), ImageButton)
            View.Visible = IIf(mEmpCAAuthorization.EmpCAAuthorizationDetails(j).IsAttachmentAdded = True, True, False)

            Dim lnkArrow As Image = CType(dgEmpCAAuthorizationDetail.Rows.Item(j).Cells(10).FindControl("lnkArrow"), Image)

            If DeleteRecord.Visible = False And View.Visible = False Then
                lnkArrow.Visible = False
            End If


            Dim txtAuthorizationDetails As TextBox = CType(dgEmpCAAuthorizationDetail.Rows.Item(j).Cells(1).FindControl("txtAuthorizationDetails"), TextBox)
            txtAuthorizationDetails.Enabled = IIf(mEmpCAAuthorization.CAStatusID > 1, False, True)

            Dim lnkbtnAddSCOPE As LinkButton = CType(dgEmpCAAuthorizationDetail.Rows.Item(j).Cells(2).FindControl("lnkbtnAddSCOPE"), LinkButton)
            lnkbtnAddSCOPE.Enabled = IIf(mEmpCAAuthorization.CAStatusID > 1, False, True)

            Dim lnkbtnAddLICENSE As LinkButton = CType(dgEmpCAAuthorizationDetail.Rows.Item(j).Cells(4).FindControl("lnkbtnAddLICENSE"), LinkButton)
            lnkbtnAddLICENSE.Enabled = IIf(mEmpCAAuthorization.CAStatusID > 1, False, True)


            Dim txtLimitations As TextBox = CType(dgEmpCAAuthorizationDetail.Rows.Item(j).Cells(6).FindControl("txtLimitations"), TextBox)
            txtLimitations.Enabled = IIf(mEmpCAAuthorization.CAStatusID > 1, False, True)

            Dim txtRevNo As TextBox = CType(dgEmpCAAuthorizationDetail.Rows.Item(j).Cells(7).FindControl("txtRevNo"), TextBox)
            txtRevNo.Enabled = IIf(mEmpCAAuthorization.CAStatusID > 1, False, True)

            Dim txtRev As TextBox = CType(dgEmpCAAuthorizationDetail.Rows.Item(j).Cells(8).FindControl("txtRev"), TextBox)
            txtRev.Enabled = IIf(mEmpCAAuthorization.CAStatusID > 1, False, True)

        Next

        For k As Integer = 0 To mEmpCAAuthorization.EmpCAAuthorizationTerms.Count - 1
            Dim DeleteRecord As ImageButton = CType(dgEmpCAAuthorizationTerms.Rows.Item(k).Cells(2).FindControl("DeleteRecord"), ImageButton)
            DeleteRecord.Visible = IIf(mEmpCAAuthorization.CAStatusID > 4, False, True)
        Next

        If mEmpCAAuthorization.CAStatusID = 4 Or Session("IsEmpCAAuthorizationForRenew") = "True" Or CAType = 0 Then
            txtCANo.Enabled = True
            txtRevisionNo.Enabled = True
            txtRevisionDate.Enabled = True
            txtFromDate.Enabled = True
            txtToDate.Enabled = True
        ElseIf (mEmpCAAuthorization.CAStatusID > 4) Then
            txtCANo.Enabled = False
            txtRevisionNo.Enabled = False
            txtRevisionDate.Enabled = False
            txtFromDate.Enabled = False
            txtToDate.Enabled = False
        End If

    End Sub
    Private Sub ControlVisibilityForFileAttachment()
        If mEmpCAAuthorization.IsAttachmentAdded Then
            ImageButton1.Visible = True
            If mEmpCAAuthorization.CAStatusID = 1 Then
                btnDelAttach.Enabled = True
            Else
                btnDelAttach.Enabled = False
            End If
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Public Sub SetReport(Optional ByVal ByMail As Boolean = False)

    End Sub
    Private Sub DeleteCATerms(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveTerm, MSGBox.Message_text.RemoveTerm, "", MsgBoxStyle.YesNo, "DeleteTerm")
        mEmpCAAuthorization.EmpCAAuthorizationTerms.CurrentIndex = Index
        Session("mEmpCAAuthorization") = mEmpCAAuthorization
    End Sub

    Private Sub EmailValidation()
        Dim Participants As String
        Participants = txtParticipants.Text.ToString()
        IsEmail(Participants)
        If IsEmail(Participants) Then
            'e.IsValid = False
        End If
        Try
        Catch ex As Exception

            Exit Sub
        End Try
    End Sub

    'Private Sub IsEmail(ByVal email As String)
    '    'Static emailExpression As New Regex=("^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$")

    '    'emailExpression.IsMatch(email)

    '    Dim regex As Regex = New Regex("^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")
    '    Dim isValid As Boolean = regex.IsMatch(email)

    '    If regex.IsMatch(email) Then

    '    End If
    'End Sub
    Function IsEmail(ByVal email As String)
        Dim regex As Regex = New Regex("^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")
        Dim isValid As Boolean = regex.IsMatch(email)

        If regex.IsMatch(email) Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub SendMail()

        Dim mUser As SI.UTILITY.User = SI.UTILITY.User.GetUser(HttpContext.Current.User.Identity.Name)

        Dim str As New StringBuilder

        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        Dim SmtpHost, SmtpUser, SmtpPassword As String
        Dim SmtpPort As Integer = 0

        SmtpHost = mModuleList.Item("EmpCAValidateForCA").SmtpHost
        SmtpPort = mModuleList.Item("EmpCAValidateForCA").SmtpPort
        SmtpUser = mModuleList.Item("EmpCAValidateForCA").SmtpUser
        SmtpPassword = mModuleList.Item("EmpCAValidateForCA").SmtpPassword

        Dim EmpEmail As String = ""

        Dim Participantswithcomma As String = mEmpCAAuthorization.Participants.Replace(" ", ",").ToString().Replace(vbLf, ",").ToString().Replace(vbCr, ",").ToString().Replace(vbCrLf, ",").ToString().Replace(",,", ",").ToString()
        EmpEmail = IIf(mEmployeeList(mEmpCAAuthorization.EmployeeID).Email <> "", mEmployeeList(mEmpCAAuthorization.EmployeeID).Email & ",", "") & Participantswithcomma
        Try
            str.Append("Following employee has been scheduled for interview regarding company authorization. Kindly book your date as per following details. ")
            str.Append("<p>Meeting Details are as follows: </p>")
            str.Append("<p><b>Employee Name: </b> " & mEmpCAAuthorization.EmployeeName & "</p>")
            str.Append("<p><b>Employee Code: </b> " & mEmpCAAuthorization.EmployeeCode & "</p>")
            str.Append("<p><b>AMEL No.: </b> " & mEmpCAAuthorization.AMELNo & "</p>")
            str.Append("<p><b>AMEL Cat: </b> " & mEmpCAAuthorization.AMELCat & "</p>")
            str.Append("<p><b>Meeting Date & Time: </b> " & Format(CDate(mEmpCAAuthorization.mScheduleMeetingDateFormatted), AppSettings("DateFormat")) & "  " & Format(CDate(mEmpCAAuthorization.mScheduleMeetingDateFormatted), AppSettings("TimeFormat")) & "</p>")

            'SendMailFile.SendMailFile(Nothing, User.Identity.Name, "CA Authorizaion Meeting Details", "", Info:=str.ToString, VendorEmailID:="", ToMailID:=Participantswithcomma.ToString, FromAudit:=0, Remark:=mEmpCAAuthorization.MeetingMinutes,
            '                          SmtpHost:=SmtpHost, SmtpPort:=SmtpPort, SmtpUser:=SmtpUser, SmtpPassword:=SmtpPassword)

            SendMailFile.SendMailFile(Nothing, User.Identity.Name, "Company Authorization Meeting Details", "", Info:=str.ToString, VendorEmailID:="", ToMailID:=EmpEmail.ToString, FromAudit:=0, Remark:=mEmpCAAuthorization.MeetingMinutes,
                                      SmtpHost:=SmtpHost, SmtpPort:=SmtpPort, SmtpUser:=SmtpUser, SmtpPassword:=SmtpPassword)

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTransDetail", MessageBox.Show("Mail Sent Successfully", False), True)


        Catch ex As Exception
            Dim Title As String = "Error Sending Mail"
            Dim Message As String = ex.InnerException.ToString
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(Title, Message, , False), True)
            Exit Sub
        End Try
    End Sub

#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mEmployeeList = EmployeeList.GetEmployeeList("", "", "(SELECT)")
        cmbEmployee.DataSource = mEmployeeList
        Session("mEmployeeList") = mEmployeeList

        dgEmpCAAuthorizationDetail.DataSource = mEmpCAAuthorization.EmpCAAuthorizationDetails


        ''Added by Sachin
        dgEmpCAAuthorizationTerms.DataSource = mEmpCAAuthorization.EmpCAAuthorizationTerms

        dgEmpCAStatus.DataSource = mEmpCAAuthorization.CAStatusChilds
        ''
        Session("mEmpCAAuthorization") = mEmpCAAuthorization
        txtEmpCAAuthorizationDate.Text = mEmpCAAuthorization.EmpCAAuthorizationDateFormatted.ToString
        txtFromDate.Text = mEmpCAAuthorization.CAInitialIssueDateFormatted.ToString
        txtToDate.Text = mEmpCAAuthorization.CAValidUptoFormatted.ToString
        txtContinuationTrainingValidity.Text = mEmpCAAuthorization.ContinuationTrainingValidityFormatted.ToString()

        DataBind()
        cmbEmployee.SelectedValue = mEmpCAAuthorization.EmployeeID.ToString
    End Sub
    Private Sub SetGrid()

        For j As Integer = 0 To dgEmpCAAuthorizationDetail.Rows.Count - 1
            Dim lnkbtnAddSCOPE As LinkButton = CType(dgEmpCAAuthorizationDetail.Rows.Item(j).Cells(2).FindControl("lnkbtnAddSCOPE"), LinkButton)
            lnkbtnAddSCOPE.Text = dgEmpCAAuthorizationDetail.Rows.Item(j).Cells(3).Text

            Dim lnkbtnAddLICENSE As LinkButton = CType(dgEmpCAAuthorizationDetail.Rows.Item(j).Cells(4).FindControl("lnkbtnAddLICENSE"), LinkButton)
            lnkbtnAddLICENSE.Text = dgEmpCAAuthorizationDetail.Rows.Item(j).Cells(5).Text
        Next
    End Sub
    Private Sub SetControl()

        For j As Integer = 0 To mEmpCAAuthorization.EmpCAAuthorizationDetails.Count - 1
            Dim txtRev As TextBox = CType(dgEmpCAAuthorizationDetail.Rows.Item(j).Cells(8).FindControl("txtRev"), TextBox)
            txtRev.Text = mEmpCAAuthorization.EmpCAAuthorizationDetails(j).RevDateFormatted.ToString

            Dim DeleteRecord As ImageButton = CType(dgEmpCAAuthorizationDetail.Rows.Item(j).Cells(10).FindControl("DeleteRecord"), ImageButton)
            DeleteRecord.Visible = IIf(mEmpCAAuthorization.StatusID > 1, False, True)
        Next
    End Sub
    Private Sub SetControlStatus(ByVal StatusId As Int16, ByVal CAStatusId As Int16, ByVal CAType As Int16)
        btnSave.Visible = IIf(CAStatusId > 1, False, True)
        'btnApply.Visible = IIf(CAStatusId > 1, False, True)
        ImgDetailsAdd.Visible = IIf(CAStatusId > 1, False, True)
        'imgTermsAdd.Visible = IIf(StatusId > 1, False, True)
        dgEmpCAAuthorizationDetail.Columns(9).Visible = IIf(CAStatusId > 1, False, True)
        btnSelectFile.Disabled = IIf(CAStatusId > 1, True, False)
        btnvalidate.Visible = IIf(CAType = 2 And CAStatusId = 2, True, False)
        btnSchedule.Visible = IIf(CAStatusId = 3, True, False)
    End Sub

    Public Sub validateParticipants(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValid As CustomValidator
        CustValid = CType(s, CustomValidator)
        'If CustValid.ControlToValidate = "txtParticipants" Then
        e.IsValid = False
        'Dim result As String() = txtParticipants.Text.Split(",")
        Dim Participants As String
        Participants = txtParticipants.Text.ToString()

        Dim Participantswithcomma As String = Participants.Replace(" ", ",").ToString().Replace(vbLf, ",").ToString().Replace(vbCr, ",").ToString().Replace(vbCrLf, ",").ToString()

        Dim result As String() = Participantswithcomma.Split(",")

        For i As Integer = 0 To result.Length - 1
            If (result(i) <> "") Then
                If IsEmail(result(i)) Then
                    e.IsValid = True
                Else
                    e.IsValid = False
                End If
            End If

        Next

        'If IsEmail(Participants) Then
        '        e.IsValid = True
        '    Else
        '        e.IsValid = False
        '    End If

        'For i As Integer = 0 To result.Length - 1
        '    If result(i) <> "" Then
        '        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "validateEmailParticipants", "validateEmailParticipants('" + result(i).ToString + "');", True)
        '        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "validateEmailParticipants", "validateEmailParticipants();", True)
        '        If hdndummy.Value = "false" Then
        '            e.IsValid = False
        '        End If
        '    End If


        'Next
        ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "validateParticipantsEmails", "validateParticipantsEmails();", True)
        'End If




    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValid As CustomValidator
        CustValid = CType(s, CustomValidator)
        If CustValid.ControlToValidate = "cmbEmployee" Then


        ElseIf CustValid.ControlToValidate = "txtRemark" Then
            If Len(Trim(txtRemark.Text)) > 1000 Then
                CustValid.ErrorMessage = "Max. Length of Remark should be 1000."
                e.IsValid = False
            Else
                e.IsValid = True
            End If

        End If

        If CAType = 3 Then

            'If CustValid.ControlToValidate = "txtCANo" Then
            '    If Len(Trim(txtCANo.Text)) = "" Then
            '        CustValid.ErrorMessage = "Authorization No is required."
            '        e.IsValid = False
            '    Else
            '        e.IsValid = True
            '    End If
            'End If



            'If CustValid.ControlToValidate = "txtRevisionNo" Then

            '    If Len(Trim(txtRevisionNo.Text)) = "" Then
            '        CustValid.ErrorMessage = "Revision No is required."
            '        e.IsValid = False
            '    Else
            '        e.IsValid = True
            '    End If
            'End If

            If CustValid.ControlToValidate = "txtFromDate" Then

                If txtCANo.Text = "" Then
                    CustValid.ErrorMessage = "Authorization No is required."
                    e.IsValid = False
                ElseIf txtFromDate.Text = "" Then
                    CustValid.ErrorMessage = "Issue Date required."
                    e.IsValid = False
                ElseIf txtToDate.Text = "" Then
                    CustValid.ErrorMessage = "Valid Upto required."
                    e.IsValid = False
                ElseIf txtRevisionNo.Text = "" Then
                    CustValid.ErrorMessage = "Revision No is required."
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If

            End If


            'If CustValid.ControlToValidate = "txtToDate" Then

            '    If txtToDate.Text = "" Then
            '        CustValid.ErrorMessage = "Valid Upto required."
            '        e.IsValid = False
            '    Else
            '        e.IsValid = True
            '    End If

            'End If
        End If




    End Sub
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        addAttributes()
        If (CAType < 2) Then
            mEmpCAAuthorization.EmployeeID = SI.UTILITY.User.GetUser(User.Identity.Name).EmployeeID
        End If

        'cmbEmployee.SelectedValue = SI.UTILITY.User.GetUser(User.Identity.Name).EmployeeID.ToString()
        If Not IsPostBack And Session("Sender") = "" Then
            If CType(Session("AddTransTextSeries"), String) = "True" AndAlso (Not Session("TransText_ForTransSeries") Is Nothing) Then
                If mEmpCAAuthorization.IsNew Then
                    mEmpCAAuthorization.Text = Session("TransText_ForTransSeries")
                    Session("mEmpCAAuthorization") = mEmpCAAuthorization
                    Session("AddTransTextSeries") = "False"
                    Session.Remove("TransName_ForTransSeries")
                    Session.Remove("TransText_ForTransSeries")
                    Session.Remove("TransNo_ForTransSeries")
                End If
            End If
            'End
            DataFieldBind()
            SetControl()
            SetGrid()
            SetControlStatus(mEmpCAAuthorization.StatusID, mEmpCAAuthorization.CAStatusID, CAType)
            ControlVisibility()
            ControlVisibilityForFileAttachment()
        End If
        If mEmpCAAuthorization.IsNew Then
            lblTitle.Text = "Company Authorization [New]"
        Else
            lblTitle.Text = "Company Authorization"
        End If


    End Sub
    Public Function CustomValidateAuthorization() As Boolean
        Dim strError As String = String.Empty
        Dim strError1 As String = String.Empty
        Dim strError2 As String = String.Empty
        Dim strError3 As String = String.Empty
        Dim strError4 As String = String.Empty

        Dim txtAuthorizationDetails As TextBox
        Dim rfvAuthorizationDetails As RequiredFieldValidator
        Dim upnlAuthorizationDetailsValidate As UpdatePanel

        Dim txtLimitations As TextBox
        Dim rfvLimitations As RequiredFieldValidator
        Dim upnlLimitationsValidate As UpdatePanel


        For j As Integer = 0 To dgEmpCAAuthorizationDetail.Rows.Count - 1

            'AuthorizationDetails
            rfvAuthorizationDetails = CType(Me.dgEmpCAAuthorizationDetail.Rows(j).FindControl("rfvAuthorizationDetails"), RequiredFieldValidator)
            upnlAuthorizationDetailsValidate = CType(Me.dgEmpCAAuthorizationDetail.Rows(j).FindControl("upnlAuthorizationDetailsValidate"), UpdatePanel)
            txtAuthorizationDetails = CType(Me.dgEmpCAAuthorizationDetail.Rows(j).FindControl("txtAuthorizationDetails"), TextBox)

            If txtAuthorizationDetails.Text = "" Then
                rfvAuthorizationDetails.IsValid = False
                rfvAuthorizationDetails.Text = "* Authorization Details Required"
                strError2 = "* Authorization Details Required"
                upnlAuthorizationDetailsValidate.Update()
            End If

            'Limitations
            rfvLimitations = CType(Me.dgEmpCAAuthorizationDetail.Rows(j).FindControl("rfvLimitations"), RequiredFieldValidator)
            upnlLimitationsValidate = CType(Me.dgEmpCAAuthorizationDetail.Rows(j).FindControl("upnlLimitationsValidate"), UpdatePanel)
            txtLimitations = CType(Me.dgEmpCAAuthorizationDetail.Rows(j).FindControl("txtLimitations"), TextBox)

            If txtLimitations.Text = "" Then
                rfvLimitations.IsValid = False
                rfvLimitations.Text = "* Limitations Required"
                strError2 = "* Limitations Required"
                upnlLimitationsValidate.Update()
            End If

        Next



        If strError <> "" Or strError1 <> "" Or strError3 <> "" Or strError4 <> "" Or strError2 <> "" Then
            Return False
        Else

        End If

        Return True
    End Function

    Private Sub ImgDetailsAdd_Click(sender As Object, e As ImageClickEventArgs) Handles ImgDetailsAdd.Click, ImgDetailsAddBottom.Click
        'If IsValid Then
        '    SetObject()
        '    'mEmpCAAuthorization.EmpCAAuthorizationDetails.Add(mEmpCAAuthorization.ID)
        '    Session("mEmpCAAuthorization") = mEmpCAAuthorization
        '    Session("EditEmpCAAuthorizationDetail") = False
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpCAAuthorizationDetailWindow", "OpenEmpCAAuthorizationDetailWindow();", True)
        'Else
        '    upnlValidationsummary.Update()
        'End If

        If CustomValidateAuthorization() = False Then upnlValidationsummary.Update() : Exit Sub
        SetStatusDetails(1) ' Added by Sachin
        SetObject()
        setObjectAuthorizationDetail()
        mEmpCAAuthorization.EmpCAAuthorizationDetails.Add(mEmpCAAuthorization.ID)
        dgEmpCAAuthorizationDetail.DataSource = mEmpCAAuthorization.EmpCAAuthorizationDetails
        dgEmpCAAuthorizationDetail.DataBind()

        SetGrid()
        SetControl()
        ControlVisibility()
        Dim lblAuthorizationDetailsStar As Label
        lblAuthorizationDetailsStar = dgEmpCAAuthorizationDetail.HeaderRow().FindControl("lblAuthorizationDetailsStar")
        lblAuthorizationDetailsStar.Visible = True


        Dim lblLimitationsStar As Label
        lblLimitationsStar = dgEmpCAAuthorizationDetail.HeaderRow().FindControl("lblLimitationsStar")
        lblLimitationsStar.Visible = True


        upnlEmpCAAuthorizationDetail.Update()
        ScriptManager.RegisterStartupScript(Me, Me.GetType, " CallLostCAAuthorizationResize()", " CallLostCAAuthorizationResize();", True)

    End Sub

    Private Sub dgEmpCAAuthorizationDetail_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgEmpCAAuthorizationDetail.RowCommand
        Dim mQtyBalReceived As Decimal = 0
        Select Case e.CommandName
            'Case "EditView"
            '    Dim index As Int32 = CInt(e.CommandArgument) + dgEmpCAAuthorizationDetail.PageIndex * dgEmpCAAuthorizationDetail.PageSize
            '    'If mEmpCAAuthorization.EmpCAAuthorizationDetails.Item(Index:=index).OrderNumber <> "" Then
            '    '    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Can not edit as used in order " + mEmpCAAuthorization.EmpCAAuthorizationDetails.Item(Index:=index).OrderNumber, MsgBoxStyle.OkOnly, "")
            '    '    Exit Sub
            '    'End If
            '    'If mEmpCAAuthorization.EmpCAAuthorizationDetails.Item(Index:=index).WorkOrderNumber <> "" Then
            '    '    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Can not edit as used in work order " + mEmpCAAuthorization.EmpCAAuthorizationDetails.Item(Index:=index).WorkOrderNumber, MsgBoxStyle.OkOnly, "")
            '    '    Exit Sub
            '    'End If
            '    SetObject()
            '    setObjectAuthorizationDetail()
            '    mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentIndex = index
            '    Session("mEmpCAAuthorization") = mEmpCAAuthorization
            '    Session("EditEmpCAAuthorizationDetail") = True
            '    Dim tmpEmpCAAuthorization As EmpCAAuthorization = mEmpCAAuthorization.Clone
            '    Session("tmpEmpCAAuthorization") = tmpEmpCAAuthorization
            '    Session("ItemIndex") = mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentIndex
            '    ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenEmpCAAuthorizationDetailWindow", "OpenEmpCAAuthorizationDetailWindow();", True)
            Case "SCOPERec"

                If CustomValidateAuthorization() = False Or CustomValidate1() = False Then upnlValidationsummary.Update() : Exit Sub


                Dim index As Int32 = (CInt(e.CommandArgument) - 1) + dgEmpCAAuthorizationDetail.PageIndex * dgEmpCAAuthorizationDetail.PageSize
                mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentIndex = index

                If mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.IsNew Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Record need to be saved before adding Scope ", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                SetObject()
                setObjectAuthorizationDetail()
                Session("mEmpCAAuthorization") = mEmpCAAuthorization
                Session("mEmpCAAuthorizationDetail") = mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenScopeWindow", "OpenScopeWindow()", True)
            Case "LICENSRec"

                If CustomValidateAuthorization() = False Or CustomValidate1() = False Then upnlValidationsummary.Update() : Exit Sub


                Dim index As Int32 = (CInt(e.CommandArgument) - 1) + dgEmpCAAuthorizationDetail.PageIndex * dgEmpCAAuthorizationDetail.PageSize
                mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentIndex = index

                If mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.IsNew Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Record need to be saved before adding Limitation ", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                SetStatusDetails(1)
                SetObject()
                setObjectAuthorizationDetail()
                Session("mEmpCAAuthorization") = mEmpCAAuthorization
                Session("mEmpCAAuthorizationDetail") = mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenLimitationWindow", "OpenLimitationWindow()", True)

            Case "DeleteRecord"
                Dim index As Int32 = (CInt(e.CommandArgument) - 1) + dgEmpCAAuthorizationDetail.PageIndex * dgEmpCAAuthorizationDetail.PageSize
                'If mEmpCAAuthorization.EmpCAAuthorizationDetails.Item(Index:=index).OrderNumber <> "" Then
                '    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Can not remove as used in order " + mEmpCAAuthorization.EmpCAAuthorizationDetails.Item(Index:=index).OrderNumber, MsgBoxStyle.OkOnly, "")
                '    Exit Sub
                'End If
                'If mEmpCAAuthorization.EmpCAAuthorizationDetails.Item(Index:=index).WorkOrderNumber <> "" Then
                '    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Can not remove as used in work order " + mEmpCAAuthorization.EmpCAAuthorizationDetails.Item(Index:=index).WorkOrderNumber, MsgBoxStyle.OkOnly, "")
                '    Exit Sub
                'End If
                DeleteRecord(index)
            Case "SelectFile"
                SetObject()
                setObjectAuthorizationDetail()
                Dim index As Int32 = (CInt(e.CommandArgument) - 1) + dgEmpCAAuthorizationDetail.PageIndex * dgEmpCAAuthorizationDetail.PageSize
                mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentIndex = index

                'If mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.IsAttachmentAdded = True Then
                '    mFileAttach = FileAttach.GetAttachment(mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.ID)
                'Else
                '    mFileAttach = FileAttach.NewAttachment(Guid.Empty, mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.ID)
                'End If
                Session("mEmpCAAuthorization") = mEmpCAAuthorization
                Session("DetailAttachement") = "True"
                'Session("mFileAttach") = Nothing
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow()", True)
            Case "RemoveAttachRec"
                Dim index As Int32 = (CInt(e.CommandArgument) - 1) + dgEmpCAAuthorizationDetail.PageIndex * dgEmpCAAuthorizationDetail.PageSize
                mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentIndex = index
                'MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "DeleteDetailAttachment")
                MSGBoxCtrl.Show("Alert..!!", "Do you want to Remove Attachment", "", MsgBoxStyle.YesNo, "DeleteDetailAttachment")
            Case "ViewRec"
                Dim index As Int32 = (CInt(e.CommandArgument) - 1) + dgEmpCAAuthorizationDetail.PageIndex * dgEmpCAAuthorizationDetail.PageSize
                mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentIndex = index
                If mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.IsAttachmentAdded And mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.FileAttachments.Count > 0 Then
                    mFileAttach = FileAttach.GetAttachment(mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.ID)
                    'Session("mFileAttach") = mFileAttach

                    Dim No As New Random
                    Dim StrName As String = "abc" & No.Next.ToString
                    If mFileAttach.Size > 0 Then
                        Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
                        Dim fs As FileStream
                        If File.Exists(AppSettings("DOCPath")) = False Then
                            'Delete File if exist
                            System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                            ' Create the file.
                            fs = File.Create(path)
                            '' Add some information to the file.
                            fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                            fs.Close()
                            Session("DOCPath") = path
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
                        End If
                    End If
                End If
        End Select
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not IsInRole(Rights.Print) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim da As New CSLA.Data.ObjectAdapter
        Dim MyReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim obj As EmpCAAuthorization
        Dim mCompanyDetail As New CompanyDetail
        Dim ds As New dsEmpCAAuthorization
        Dim mrptImage As rptImage
        Dim mEmployeeImage As EmployeeImage = Nothing

        obj = EmpCAAuthorization.GetEmpCAAuthorization(mEmpCAAuthorization.ID)
        mCAAuthorizationScopeList = CAAuthorizationScopeList.GetCAAuthorizationScope()
        mCALimitationList = CALimitationList.GetCALimitation()
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim mReport As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax,
                                          mCompanyDetail.Email, WebSite:="", ReportName:="", SearchStr1:=New SmartDate(txtFromDate.Text).FormattedText,
                                          SearchStr2:=New SmartDate(txtToDate.Text).FormattedText,
                                          SearchStr3:=mModuleList.Item("EmpCAAuthorization").FormRevisionNo,
                                          SearchStr4:=mModuleList.Item("EmpCAAuthorization").FormRevisionDate, SearchStr5:=AppSettings("Government Authority"), ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"),
                                          SearchStr6:="", SearchStr7:="", SearchStr8:="", SearchStr9:=AppSettings("Logo"), SearchStr10:=AppSettings("ClientCode"), ApprovalNo:=mCompanyDetail.ApprovalNo)

        'Page 1 : First Page Emp CAAuthorization
        MyReport = New crptEmpCAAuthorization
        ds.Clear()
        mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
        mEmployeeImage = EmployeeImage.GetImage(ds, DataTableName:="EmployeeImage", ID:=mEmpCAAuthorization.ID.ToString)
        da.Fill(ds, "EmpCAAuthorization", obj)

        da.Fill(ds, "EmpCAAuthorizationDetails", obj.EmpCAAuthorizationDetails)
        da.Fill(ds, "EmpCAAuthorizationTerms", obj.EmpCAAuthorizationTerms)
        da.Fill(ds, mReport)
        da.Fill(ds, mrptImage)
        da.Fill(ds, mEmployeeImage)

        MyReport.SetDataSource(ds)
        Session("CrystalReport") = MyReport

        Dim PDFNo As Integer = 1
        Dim PDFNoChild As Integer = 1
        Dim tmp As Integer
        Dim a As New Random
        Dim pageCount As Integer = 0

        Dim pdfList As New System.Collections.ArrayList

        Dim MyFile1 = ""
        Dim myExportOption As CrystalDecisions.Shared.ExportOptions
        Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

        MyFile1 = "C:\Temp\" & "EmpCAAuthorization" & tmp & PDFNo.ToString & ".pdf"
        MyReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

        myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
        myDiskOption.DiskFileName = MyFile1
        myExportOption = MyReport.ExportOptions
        With myExportOption
            .DestinationOptions = myDiskOption
            .ExportDestinationType = ExportDestinationType.DiskFile
            .ExportFormatType = ExportFormatType.PortableDocFormat
        End With
        MyReport.Export()
        MyReport.Close()
        MyReport.Dispose()
        GC.Collect()
        pdfList.Add(MyFile1)
        PDFNo = PDFNo + 1
        'Page 1 End

        'Page 2 : Second Page Authorization Scope
        MyReport = New crptScopeOfAuthorization
        ds.Clear()
        mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
        da.Fill(ds, "CAAuthorizationScopeList", mCAAuthorizationScopeList)
        da.Fill(ds, "CALimitationList", mCALimitationList)
        da.Fill(ds, mReport)
        da.Fill(ds, mrptImage)

        MyReport.SetDataSource(ds)

        Session("CrystalReport") = MyReport

        MyFile1 = "C:\Temp\" & "CAAuthorizationScope" & tmp & PDFNo.ToString & ".pdf"
        MyReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

        myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
        myDiskOption.DiskFileName = MyFile1
        myExportOption = MyReport.ExportOptions
        With myExportOption
            .DestinationOptions = myDiskOption
            .ExportDestinationType = ExportDestinationType.DiskFile
            .ExportFormatType = ExportFormatType.PortableDocFormat
        End With
        MyReport.Export()
        MyReport.Close()
        MyReport.Dispose()
        GC.Collect()

        pdfList.Add(MyFile1)
        PDFNo = PDFNo + 1
        'Page 2 : End


        '''''END: Merge ALL reports
        Dim MergedPath As String = "C:\Temp\" & "temp_myMergedPdf.pdf"
        Dim MergedPath_WM As String = "C:\Temp\" & "ReliabilityReport.pdf"

        Dim filesByte As New List(Of Byte())()

        For Each file__1 As String In pdfList 'files
            filesByte.Add(File.ReadAllBytes(file__1))
        Next

        File.WriteAllBytes(MergedPath, Flypal.PDFMergers.MergeFiles(filesByte))
        AddWatermarkText(MergedPath, MergedPath_WM, "Page ", , , iTextSharp.text.BaseColor.BLACK, , 0.0, pageCount, ReportName:="EmpCAAuthorization")


        Session("CrystalReport") = MergedPath_WM
        Session("PrintReportWithAttachment") = "True"

        Dim DeleteThis As String = "Authorization"
        Dim Files As String() = Directory.GetFiles("C:\Temp\")

        For Each file__1 As String In Files
            If file__1.ToUpper().Contains(DeleteThis.ToUpper()) Then
                File.Delete(file__1)
            End If
        Next
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        MarkLog(Util.Action.Close, "EmpCAAuthorization", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If

        'SetObject()
        'setObjectAuthorizationDetail()
        Session("IsValid") = IsValid
        If (CAType <= 1) Then
            If mEmpCAAuthorization.IsDirty Then
                MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.CloseConfirm, "", MsgBoxStyle.YesNo, "Close")
                'If IsValid Then
                '    SetObject()
                '    setObjectAuthorizationDetail()
                'End If
            Else
                RemoveSessions()
                mEmployeeList = Nothing
                mEmpCAAuthorization = Nothing
                Session.Remove("IsEmpCAAuthorizationForRenew")
                Response.Redirect("Index.aspx")
            End If

        Else
            RemoveSessions()
            mEmployeeList = Nothing
            mEmpCAAuthorization = Nothing
            Session.Remove("IsEmpCAAuthorizationForRenew")
            Response.Redirect("Index.aspx")
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        'SetObject()  '''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'If IsValid Then

        If Session("IsEmpCAAuthorizationForRenew") = "True" Then
            'Do nothing
        Else
            If mEmpCAAuthorizationList.Contains(mEmpCAAuthorization.ID, New Guid(cmbEmployee.SelectedValue)) Then
                'MSGBoxCtrl.show("Alert..!!", "Employee Authorization already added for " + cmbEmployee.SelectedItem.ToString, MsgBoxStyle.YesNo, "")
                MSGBoxCtrl.Show("Alert!", "Authorization already added for " + cmbEmployee.SelectedItem.ToString, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        End If

        If mEmpCAAuthorization.EmpCAAuthorizationDetails.Count = 0 Then
            MSGBoxCtrl.Show("Alert!", "Authorization cannot be Saved without Company Authorization Details ", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        If CustomValidateAuthorization() = False Then upnlValidationsummary.Update() : Exit Sub

        If IsValid = True And CustomValidate1() = True Then
            Save(1)
        Else
            upnlValidationsummary.Update()

        End If
    End Sub
    'Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    '    SetReport()
    '    Dim Str1 As String
    '    Str1 = "openTranDetail();"
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
    'End Sub

    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        ViewImage()
    End Sub
    Private Sub GetAttachment()
        If mEmpCAAuthorization.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mEmpCAAuthorization.ID)
            Session("mFileAttachEmpCAAuthorization") = mFileAttach
        End If
    End Sub
    Private Sub SaveAttachment() '
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                Try
                    mFileAttach.Save()
                Catch ex As Exception
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
                End Try
            Else
                If (Not mEmpCAAuthorization.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mEmpCAAuthorization.ID)
                End If
                IsAttachmentDeleted = False
                Session("IsAttachmentDeleted") = IsAttachmentDeleted
            End If
        End If
    End Sub
    Private Sub ViewImage()
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        GetAttachment()
        If mFileAttach.Size > 0 Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            End If
        End If
    End Sub
    Private Function IsValidTime(ByVal TimeValue As String) As Boolean
        Dim TimeRegulerExpression As String = ""
        If (AppSettings("TimeFormat").IndexOf("tt") <> -1 Or AppSettings("TimeFormat").IndexOf("TT") <> -1) Then
            'TimeRegulerExpression = "^((0[0-9])|(1[0-2])|([0-9])):[0-5][0-9]( )*(AM|am|PM|pm)$"    '12 Hour Format
            TimeRegulerExpression = "^((0[0-9])|(1[0-2])|([0-9])):[0-5][0-9]( )*(AM|am|PM|pm|aM|pM)$"    '12 Hour Format
        Else
            TimeRegulerExpression = "^(([01][0-9])|(2[0-3])|([0-9])):[0-5][0-9]$"   '24 Hour Format
        End If

        If (System.Text.RegularExpressions.Regex.IsMatch(TimeValue, TimeRegulerExpression)) Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub btnDelAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        If (Not IsInRole(Rights.Authorized) And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte
        GetAttachment()
        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0
        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        IsAttachmentDeleted = True
        mEmpCAAuthorization.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
        ControlVisibility()
        ControlVisibilityForFileAttachment()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        If Session("DetailAttachement") = "False" Then
            mEmpCAAuthorization.IsAttachmentAdded = True
            Session("mFileAttachEmpCAAuthorization") = Session("mFileAttach")
            Session("mFileAttach") = Nothing
            Session.Remove("mFileAttach")
            ControlVisibilityForFileAttachment()
            upnlAttachFile.Update()
        ElseIf Session("DetailAttachement") = "True" Then

            AttachFileuthorizationDetails()

            dgEmpCAAuthorizationDetail.DataSource = mEmpCAAuthorization.EmpCAAuthorizationDetails
            dgEmpCAAuthorizationDetail.DataBind()
            SetGrid()
            SetControl()
            ControlVisibility()
            upnlEmpCAAuthorizationDetail.Update()
        End If
        Session.Remove("DetailAttachement")


    End Sub
    Private Sub AttachFileuthorizationDetails()
        Dim BackupPath As String = ""
        BackupPath = AppSettings("DOCPath") & "New.PDF"
        If Not Session("Extension") = ".pdf" Then
            MSGBoxCtrl.Show("Alert!", "Please attach in PDF only!", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Try
            mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.IsAttachmentAdded = True
            If Not mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.FileAttachments.Contains(mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.ID, CType(Session("FileUpload.FileName"), String)) Then
                mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.FileAttachments.Add(mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.ID, CType(Session("FileUpload.FileName"), String)) 'Added By Vikrant On 17-Apr-2013 For ALL17042013
                ' mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.FileAttachments.CurrentItem.FileName = mFileAttach.FileName
                mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.FileAttachments.CurrentItem.ImageFile = CType(Session("ImageFile"), Byte())
                mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.FileAttachments.CurrentItem.Size = Session("Size")
                mEmpCAAuthorization.EmpCAAuthorizationDetails.CurrentItem.FileAttachments.CurrentItem.Extension = Session("Extension")

                Session("mEmpCAAuthorization") = mEmpCAAuthorization
                Session.Remove("Size")
                Session.Remove("ImageFile")
                Session.Remove("Extension")
                Session.Remove("FileUpload.FileName")

            Else
                Session("mEmpCAAuthorization") = mEmpCAAuthorization
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
            End If
        Catch ex As Exception
        Finally
        End Try
    End Sub

    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If (Not IsInRole(Rights.Authorized) And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        If mEmpCAAuthorization.IsAttachmentAdded = True Then
            mFileAttach = FileAttach.GetAttachment(mEmpCAAuthorization.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.Empty, mEmpCAAuthorization.ID)
        End If

        Session("mFileAttach") = mFileAttach
        Session("DetailAttachement") = "False"
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow()", True)
    End Sub

    Private Sub cmbEmployee_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbEmployee.SelectedIndexChanged
        If cmbEmployee.SelectedIndex = 0 Then
            mEmpCAAuthorization.EmployeeCode = ""
            txtEmployeeCode.Text = ""
            mEmpCAAuthorization.AMELNo = ""
            txtAMELNo.Text = ""
        Else

            If mEmpCAAuthorizationList.Contains(mEmpCAAuthorization.ID, New Guid(cmbEmployee.SelectedValue)) Then
                'MSGBoxCtrl.show("Alert..!!", "Employee Authorization already added for " + cmbEmployee.SelectedItem.ToString, MsgBoxStyle.YesNo, "")
                MSGBoxCtrl.Show("Alert!", "Employee Authorization already added for " + cmbEmployee.SelectedItem.ToString, "", MsgBoxStyle.OkOnly, "")
                cmbEmployee.ClearSelection()
                upnlEmpCAAuthorizationDetails.Update()
                Exit Sub
            End If
            mEmpCAAuthorization.EmployeeCode = mEmployeeList(New Guid(cmbEmployee.SelectedValue)).EmpNo
            txtEmployeeCode.Text = mEmpCAAuthorization.EmployeeCode
            txtEmployeeCode.DataBind()
            mEmpCAAuthorization.AMELNo = mEmployeeList(New Guid(cmbEmployee.SelectedValue)).LicenseNo
            txtAMELNo.Text = mEmpCAAuthorization.AMELNo
            txtAMELNo.DataBind()
            mEmpCAAuthorization.AMELCat = Employee.GetEmployee(New Guid(cmbEmployee.SelectedValue)).CAT 'mEmployeeList(New Guid(cmbEmployee.SelectedValue)).ca
            txtAMELCat.Text = mEmpCAAuthorization.AMELCat
            txtAMELCat.DataBind()
        End If
    End Sub

    Private Sub imgTermsAdd_Click(sender As Object, e As ImageClickEventArgs) Handles imgTermsAdd.Click
        If IsValid Then
            SetObject()
            'mEmpCAAuthorization.EmpCAAuthorizationDetails.Add(mEmpCAAuthorization.ID)
            Session("mEmpCAAuthorization") = mEmpCAAuthorization
            Session("EditEmpCAAuthorizationDetail") = False
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenTermWindow", "OpenTermWindow()", True)
        Else
            upnlValidationsummary.Update()
        End If
    End Sub

    Private Sub dgEmpCAAuthorizationTerms_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgEmpCAAuthorizationTerms.RowCommand
        Select Case e.CommandName
            Case "DeleteRecord"
                Dim Index As Integer = CInt(e.CommandArgument) '+ dgEmpCAAuthorizationTerms.PageIndex * dgEmpCAAuthorizationTerms.PageSize
                DeleteCATerms(Index)
        End Select
    End Sub
    Private Sub hdnBtnEmpCAAuthorizationDetail_Click(sender As Object, e As EventArgs) Handles hdnBtnEmpCAAuthorizationDetail.Click, hdnBtnAuthorizationDetailsLimitation.Click
        mEmpCAAuthorization = EmpCAAuthorization.GetEmpCAAuthorization(mEmpCAAuthorization.ID)
        Session("mEmpCAAuthorization") = mEmpCAAuthorization
        'dgEmpCAAuthorizationDetail.DataSource = mEmpCAAuthorization.EmpCAAuthorizationDetails
        'dgEmpCAAuthorizationDetail.DataBind()
        DataFieldBind()
        SetGrid()
        SetControl()
        ControlVisibility()
        upnlEmpCAAuthorizationDetail.Update()
        upnlEmpCAAuthorizationDetails.Update()
        upnlEmpCAStatus.Update()
    End Sub
    Private Sub hdnBtnAuthorizationDetailsScope_Click(sender As Object, e As EventArgs) Handles hdnBtnAuthorizationDetailsScope.Click
        mEmpCAAuthorization = EmpCAAuthorization.GetEmpCAAuthorization(mEmpCAAuthorization.ID)
        Session("mEmpCAAuthorization") = mEmpCAAuthorization
        'dgEmpCAAuthorizationDetail.DataSource = mEmpCAAuthorization.EmpCAAuthorizationDetails
        'dgEmpCAAuthorizationDetail.DataBind()
        DataFieldBind()
        SetGrid()
        SetControl()
        ControlVisibility()
        upnlEmpCAAuthorizationDetail.Update()
        upnlEmpCAAuthorizationDetails.Update()
    End Sub
    Private Sub hdnimgBtnCAAuthorizationTerm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgBtnCAAuthorizationTerm.Click
        DataFieldBind()
        Session("mEmpCAAuthorization") = mEmpCAAuthorization
        SetGrid()
        SetControl()
        ControlVisibility()
        upnlEmpCAAuthorizationDetail.Update()
        upnlEmpCAAuthorizationTerms.Update()
    End Sub

#Region " Status "
    Private Sub btnAuthorized_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAuthorized.Click
        'If IsValid Then
        If CustomValidateAuthorization() = False Then upnlValidationsummary.Update() : Exit Sub
        If IsValid = True And CustomValidate1() = True Then
            MSGBoxCtrl.show(MSGBox.Message_title.StatusAuthorized, MSGBox.Message_text.StatusAuthorized, "<Strong> CA Authorization </Strong>", MsgBoxStyle.YesNo, "Status")
            Session("IsValid") = IsValid
            Session("mEmpCAAuthorization") = mEmpCAAuthorization
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
#End Region

#Region " Show BrokenRules "
    Public Function CustomValidate1() As Boolean
        Dim strMsg As String = ""
        'SetStatusDetails(1)
        SetObject()
        setObjectAuthorizationDetail()

        If mEmpCAAuthorization.IsValid = False Then
            For i As Integer = 0 To mEmpCAAuthorization.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mEmpCAAuthorization.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        Dim mEmpCAAuthorizationDetail As EmpCAAuthorizationDetail
        If mEmpCAAuthorization.EmpCAAuthorizationDetails.IsValid = False Then
            For Each mEmpCAAuthorizationDetail In mEmpCAAuthorization.EmpCAAuthorizationDetails
                For i As Integer = 0 To mEmpCAAuthorizationDetail.GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + mEmpCAAuthorizationDetail.GetBrokenRulesCollection(i).Description + "<Br>"
                Next
            Next
        End If

        Dim mEmpCAStatusChild As CAStatusChild
        If mEmpCAAuthorization.CAStatusChilds.IsValid = False Then
            For Each mEmpCAStatusChild In mEmpCAAuthorization.CAStatusChilds
                For i As Integer = 0 To mEmpCAStatusChild.GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + mEmpCAStatusChild.GetBrokenRulesCollection(i).Description + "<Br>"
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
    Private Sub btnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApply.Click
        'If IsValid Then
        If CustomValidateAuthorization() = False Then upnlValidationsummary.Update() : Exit Sub
        If IsValid = True And CustomValidate1() = True Then
            'MSGBoxCtrl.show(MSGBox.Message_title.StatusAuthorized, MSGBox.Message_text.StatusAuthorized, "<Strong> CA Applied </Strong>", MsgBoxStyle.YesNo, "CAStatus")
            MSGBoxCtrl.Show("Alert..!!", "Do you want to Apply for Company Authorization", "", MsgBoxStyle.YesNo, "CAStatus")
            Session("IsValid") = IsValid
            Session("mEmpCAAuthorization") = mEmpCAAuthorization
            upnlEmpCAStatus.Update()
        Else
            upnlValidationsummary.Update()
        End If
    End Sub


    Private Sub btnvalidate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnvalidate.Click
        'If IsValid Then
        'If CustomValidateAuthorization() = False Then upnlValidationsummary.Update() : Exit Sub
        If IsValid = True And CustomValidate1() = True Then
            'MSGBoxCtrl.show(MSGBox.Message_title.StatusAuthorized, MSGBox.Message_text.StatusAuthorized, "<Strong> CA Validate </Strong>", MsgBoxStyle.YesNo, "CAStatusValidate")
            MSGBoxCtrl.Show("Alert..!!", "Do you want to Validate Company Authorization", "", MsgBoxStyle.YesNo, "CAStatusValidate")
            Session("IsValid") = IsValid
            Session("mEmpCAAuthorization") = mEmpCAAuthorization
            upnlEmpCAStatus.Update()
        Else
            upnlValidationsummary.Update()
        End If
    End Sub

    Private Sub btnSchedule_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSchedule.Click
        'If IsValid Then
        'If CustomValidateAuthorization() = False Then upnlValidationsummary.Update() : Exit Sub
        pnlScheduleMeet.Visible = True
        upnlScheduleMeeting.Update()
        mdlPopUpScheduleMeeting.Show()
    End Sub

    Private Sub btnMeetClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMeetClose.Click
        'If IsValid Then
        'If CustomValidateAuthorization() = False Then upnlValidationsummary.Update() : Exit Sub
        mdlPopUpScheduleMeeting.Hide()
        pnlScheduleMeet.Visible = False
        upnlScheduleMeeting.Update()

    End Sub
    Private Sub btnScheduleMeet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScheduleMeet.Click

        ' If IsValid = True And CustomValidate1() = True Then
        If Not IsValid Then
            Exit Sub
        End If


        Save(4)
        SendMail()
        MarkLog(Util.Action.Save, "EmpCAAuthorization", "Scheduled By Employee " + SI.UTILITY.User.GetUser(User.Identity.Name).EmployeeName + " on " + Today.Date, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        mdlPopUpScheduleMeeting.Hide()
        pnlScheduleMeet.Visible = False
        upnlScheduleMeeting.Update()


    End Sub
    Private Sub btnApprove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApprove.Click
        If txtFromDate.Text = "" Then
            MSGBoxCtrl.Show("Alert!", "Issue date required. ", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If IsValid = True And CustomValidate1() = True Then
            'MSGBoxCtrl.show(MSGBox.Message_title.StatusAuthorized, MSGBox.Message_text.StatusAuthorized, "<Strong> CA Approve </Strong>", MsgBoxStyle.YesNo, "CAStatusApprove")
            MSGBoxCtrl.Show("Alert..!!", "Do you want to Approve Company Authorization", "", MsgBoxStyle.YesNo, "CAStatusApprove")
            Session("IsValid") = IsValid
            Session("mEmpCAAuthorization") = mEmpCAAuthorization
            upnlEmpCAStatus.Update()
        Else
            upnlValidationsummary.Update()
        End If

    End Sub

    Private Sub dgEmpCAAuthorizationDetail_RowDeleted(sender As Object, e As GridViewDeletedEventArgs) Handles dgEmpCAAuthorizationDetail.RowDeleted

    End Sub

    Private Sub btnReopen_Click(sender As Object, e As EventArgs) Handles btnReopen.Click

        'Session("mEmpCAAuthorization")


        If IsValid = True And CustomValidate1() = True Then
            MSGBoxCtrl.Show("Alert..!!", "Do you want to Reopen Company Authorization", "", MsgBoxStyle.YesNo, "CAStatusReopen")
            Session("IsValid") = IsValid
            Session("mEmpCAAuthorization") = mEmpCAAuthorization
            upnlEmpCAStatus.Update()
        Else
            upnlValidationsummary.Update()
        End If

    End Sub

    Private Sub txtSchTime_TextChanged(sender As Object, e As EventArgs) Handles txtSchTime.TextChanged
        If IsValidTime(txtSchTime.Text.ToString.Trim) = False Then
            txtSchTime.Text = Format(New DateTime(1753, 1, 1, 0, 0, 0), AppSettings("TimeFormat"))
        Else
            Dim DateTime As String = txtMeetingDate.Text.ToString + " " + txtSchTime.Text.ToString.Trim
            If DateDiff(DateInterval.Minute, SmartDate.StringToDate(mEmpCAAuthorization.mScheduleMeetingDateFormatted.ToString), New SmartDate(DateTime).Date) <> 0 Then
                mEmpCAAuthorization.ScheduleMeetingDate = DateTime
                ' DataFieldBind()
                Session("mEmpCAAuthorization") = mEmpCAAuthorization
            End If
        End If
    End Sub

#End Region
End Class