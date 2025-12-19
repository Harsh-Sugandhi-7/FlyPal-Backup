Imports System.Collections.Generic
Imports System.Linq
Imports System.Web.Script.Serialization

Public Class wfMSP_Ajax
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
    Public mMSP As MSP
    Public mMSPAssembly As MSPAssembly
    Public mVendorList As VendorList
    Dim EventLogID As Guid
    Dim mMSPDetail As String
    Dim mMachineID As Guid
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    Dim mFileAttachments As FileAttachments
    'End
    Dim mUser As User
    Public mVendor As Vendor
    Public mIsAttachmentNotSave As Boolean = True
    Dim email As Thread
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mMSP = CType(Session("mMSP"), MSP)
        mVendorList = CType(Session("mVendorList"), VendorList)
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
    End Sub
    Private Sub SetSession()
        Session("mMSP") = mMSP
        Session("mVendorList") = mVendorList
        Session("mFileAttach") = mFileAttach
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
        'End
    End Sub
    Private Sub RemoveSessions()
        Session.Remove("mMSP")
        Session.Remove("mVendorList")
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        'End
    End Sub
    Private Sub SetPage()

    End Sub
    Private Sub Disable()
        txtMSPDate.Enabled = False
    End Sub
    Private Sub Enable()
        txtMSPDate.Enabled = True
    End Sub
    Private Sub Save()
        'Authentication
        If Not mMSP.MSPDate Is System.DBNull.Value Then
            Dim mCheck As New Authenticate.CheckAuthentication(True, Server.MapPath("bin\Authority.xml"))
            If mCheck.WebAuthentication = True Then
                Dim mDays As Integer = 0
                mDays = mCheck.Number("Days")
                Dim maxAllowableDate As DateTime = DateAdd(DateInterval.Day, mDays, mCheck.SubscriptionDate)
                If DateDiff(DateInterval.Day, CDate(mMSP.MSPDate), maxAllowableDate) < 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, " Your subscription has been expired. can not save Goods Receipt. <br> Goods Receipt Date can not be greater than " & maxAllowableDate.ToString(WebDateFormat), MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            End If
        End If
        Dim MSPClone As MSP
        MSPClone = mMSP.Clone
        Try

            'check whether min. one item & charge is present while saving
            If Not mMSP.MSPAssemblys.Count = 0 Then
                'save the object
                SetObject()
                If mMSP.IsValid Then
                    Dim i As Integer
                    While i < mMSP.MSPAssemblys.Count
                        i = i + 1
                    End While
                    mMSP.ApplyEdit()

                    mMSP.Save()
                    If mMSP.IsAttachmentAdded Then
                        If mMSP.FileAttachments(0).Size > 0 Then
                            ImageButton1.Visible = True
                        End If

                    End If

                    mMSPDetail = mMSP.MSPNo + " Dated: " + mMSP.MSPDateFormatted + " Plan Name: " + mMSP.PlanName
                    MarkLog(Util.Action.Save, "MSP", mMSPDetail, Util.ErrorType.NoError, mMSP.ID, EventLogID)
                    mMSP.MarkClean()
                    Session("mMSP") = mMSP
                    DataFieldBind()
                    ControlVisibility()
                    upnlTitle.Update()
                    upnlMSPDetails.Update()
                    'upnlMSP.Update()
                    upnlMSPAssembly.Update()
                    upnlButtons.Update()
                    MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                Else
                    Dim mRule As String = ""
                    If mMSP.GetBrokenRulesCollection.Count > 0 Then
                        mRule = mMSP.GetBrokenRulesCollection(0).Description
                    ElseIf mMSP.MSPAssemblys.CurrentItem.GetBrokenRulesCollection.Count > 0 Then
                        mRule = mMSP.MSPAssemblys.CurrentItem.GetBrokenRulesCollection(0).Description
                    End If
                    MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, mRule, MsgBoxStyle.OkOnly, "")
                    mRule = ""
                    mMSP = MSPClone
                    SetObject()
                    Session("mMSP") = mMSP
                    DataFieldBind()
                    Exit Sub
                End If
            Else
                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Maintenance Support Plan can not be saved without assembly.", MsgBoxStyle.OkOnly, "")
                mMSP = MSPClone
                SetObject()
                Session("mMSP") = mMSP
                DataFieldBind()
                Exit Sub
            End If
        Catch ex As SqlException
            Session("MSPClone") = MSPClone
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
                mMSP = MSPClone
                SetObject()
                Session("mMSP") = mMSP
                DataFieldBind()
                Exit Sub
            ElseIf InStr(ex1.Message, "CCtabOrderItemReceiptBalanceQty", CompareMethod.Text) Then
                MSGBoxCtrl.show(MSGBox.Message_title.PendingQty, MSGBox.Message_text.PendingQty, ex1.Message.Substring(ex1.Message.IndexOf("PartNo.:")) + "Goods Receipt Qty can not be greater than Order Qty.</br><b>Please amend Purchase Order Quantity & make Goods Receipt again.</b>", MsgBoxStyle.OkOnly, "")
            Else
                MSGBoxCtrl.Show("Alert!", "Save Alert ! " + "</br>" + "There is some problem in Saving Goods Receipt. <BR> <BR>  Please Check the Entry and Try Again  !", "", MsgBoxStyle.OkOnly, "Status")
                mMSP = MSPClone
                SetObject()
                Session("mMSP") = mMSP
                DataFieldBind()
                Exit Sub
            End If
            mMSP = MSPClone
            Session("mMSP") = mMSP
        Finally
            MSPClone = Nothing
        End Try
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub SetObject()
        If txtMSPDate.Text = "" Then
            mMSP.MSPDate = Today.Date
        Else
            mMSP.MSPDate = CDate(txtMSPDate.Text)
        End If
        mMSP.Text = txtMSPText.Text.Trim
        mMSP.No = Val(txtMSPNo.Text)
        mMSP.ContractNo = Trim(txtContractNo.Text)
        mMSP.PlanName = Trim(txtPlanName.Text)
        mMSP.VendorID = New Guid(cmbVendor.SelectedValue)
        If txtFromDate.Text = "" Then
            mMSP.FromDate = System.DBNull.Value
        Else
            mMSP.FromDate = CDate(txtFromDate.Text)
        End If
        If txtToDate.Text = "" Then
            mMSP.ToDate = System.DBNull.Value
        Else
            mMSP.ToDate = CDate(txtToDate.Text)
        End If
        mMSP.Remark = Trim(txtRemark.Text)
        mMSP.CreatedBy = User.Identity.Name
        mMSP.IsNotApplicable = chkNotApplicable.Checked
    End Sub
    Private Sub DeleteRecord(ByVal Index As Int32)
        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "Delete")
        mMSP.MSPAssemblys.CurrentIndex = Index
        Session("mMSP") = mMSP
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            mMSP = CType(Session("mMSP"), MSP)
                            mMSP.MSPAssemblys.Remove(mMSP.MSPAssemblys.CurrentItem)
                            mMSP.MSPAssemblys.CurrentIndex = mMSP.MSPAssemblys.Count - 1
                            dgMSPAssembly.DataSource = mMSP.MSPAssemblys
                            dgMSPAssembly.DataBind()
                            upnlMSPAssembly.Update()
                            Session("mMSP") = mMSP
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    End If
                    If MSGBoxCtrl.Sender = "Close" Then
                        If mMSP.IsValid = True Then
                            Session.Remove("IsValid")
                            DataFieldBind()
                            If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
                                Exit Sub
                            End If
                            Save()
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
    Private Sub SetReceivedFromDetails(ByVal ToType As Int16)

    End Sub
    Public Sub TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub
    Private Sub addAttributes()
        txtMSPNo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtMSPNo').value,event)")
    End Sub
    Private Sub ControlVisibility()

    End Sub
    Private Function IsInRole(ByVal CheckFor As Rights) As Boolean
        Dim IsInRoleString As String = "MSP"
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
        If mMSP.IsAttachmentAdded Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Public Sub SetReport(Optional ByVal ByMail As Boolean = False)

    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mVendorList = VendorList.GetVendortList(0, , , , , , True,    ,   )
        cmbVendor.DataSource = mVendorList
        Session("mVendorList") = mVendorList
        dgMSPAssembly.DataSource = mMSP.MSPAssemblys
        Session("mMSP") = mMSP
        txtMSPDate.Text = mMSP.MSPDateFormatted.ToString
        txtFromDate.Text = mMSP.FromDateFormatted.ToString
        txtToDate.Text = mMSP.ToDateFormatted.ToString
        DataBind()
        cmbVendor.SelectedValue = mMSP.VendorID.ToString
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValid As CustomValidator
        CustValid = CType(s, CustomValidator)
        If CustValid.ControlToValidate = "cmbVendor" Then

        ElseIf CustValid.ControlToValidate = "cmbVendor" Then

        ElseIf CustValid.ControlToValidate = "txtRemark" Then
            If Len(Trim(txtRemark.Text)) > 1000 Then
                CustValid.ErrorMessage = "Max. Length of Remark should be 1000."
                e.IsValid = False
            Else
                e.IsValid = True
            End If

        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        addAttributes()

        If Not IsPostBack And Session("Sender") = "" Then
            If CType(Session("AddTransTextSeries"), String) = "True" AndAlso (Not Session("TransText_ForTransSeries") Is Nothing) Then
                If mMSP.IsNew Then
                    mMSP.Text = Session("TransText_ForTransSeries")
                    Session("mMSP") = mMSP
                    Session("AddTransTextSeries") = "False"
                    Session.Remove("TransName_ForTransSeries")
                    Session.Remove("TransText_ForTransSeries")
                    Session.Remove("TransNo_ForTransSeries")
                End If

            End If
            'End
            DataFieldBind()
        End If
        SetPage()
        ControlVisibility()
        ControlVisibilityForFileAttachment()
        TextChanged(sender, e)
    End Sub
    Private Sub btnAssemblyAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAssemblyAdd.Click
        If IsValid Then
            SetObject()
            mMSP.MSPAssemblys.Add(mMSP.ID)
            Session("mMSP") = mMSP
            Session("EditMSPAssembly") = False
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenMSPAssemblyWindow", "OpenMSPAssemblyWindow();", True)
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub dgMSPAssembly_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgMSPAssembly.RowCommand
        Dim mQtyBalReceived As Decimal = 0
        Select Case e.CommandName
            Case "EditView"
                Dim index As Int32 = CInt(e.CommandArgument) + dgMSPAssembly.PageIndex * dgMSPAssembly.PageSize
                If mMSP.MSPAssemblys.Item(Index:=index).OrderNumber <> "" Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Can not edit as used in order " + mMSP.MSPAssemblys.Item(Index:=index).OrderNumber, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                If mMSP.MSPAssemblys.Item(Index:=index).WorkOrderNumber <> "" Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Can not edit as used in work order " + mMSP.MSPAssemblys.Item(Index:=index).WorkOrderNumber, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                SetObject()
                mMSP.MSPAssemblys.CurrentIndex = index
                Session("mMSP") = mMSP
                Session("EditMSPAssembly") = True
                Dim tmpMSP As MSP = mMSP.Clone
                Session("tmpMSP") = tmpMSP
                Session("ItemIndex") = mMSP.MSPAssemblys.CurrentIndex
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenMSPAssemblyWindow", "OpenMSPAssemblyWindow();", True)
            Case "DeleteRecord"
                Dim index As Int32 = CInt(e.CommandArgument) + dgMSPAssembly.PageIndex * dgMSPAssembly.PageSize
                If mMSP.MSPAssemblys.Item(Index:=index).OrderNumber <> "" Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Can not remove as used in order " + mMSP.MSPAssemblys.Item(Index:=index).OrderNumber, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                If mMSP.MSPAssemblys.Item(Index:=index).WorkOrderNumber <> "" Then
                    MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, "Can not remove as used in work order " + mMSP.MSPAssemblys.Item(Index:=index).WorkOrderNumber, MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                DeleteRecord(index)
        End Select
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        MarkLog(Util.Action.Close, "MSP", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        SetObject()
        Session("IsValid") = IsValid
        If mMSP.IsDirty Then
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.CloseConfirm, "", MsgBoxStyle.YesNo, "Close")
            If IsValid Then
                SetObject()
            End If
        Else
            RemoveSessions()
            mVendorList = Nothing
            mMSP = Nothing
            Response.Redirect("Index.aspx")
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not IsInRole(Rights.[New])) And (Not IsInRole(Rights.Edit)) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        SetObject()  '''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If IsValid Then
            Save()
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
        If (Not IsInRole(Rights.Authorized) And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        If mMSP.IsAttachmentAdded Then
            'mFileAttach = FileAttach.GetAttachment(mMSP.ID)
            'mFileAttach = FileAttach.GetAttachmentChild(mMSP.ID)
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mMSP.FileAttachments(0).Extension 'mFileAttach.Extension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mMSP.FileAttachments(0).Extension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mMSP.FileAttachments(0).ImageFile, 0, mMSP.FileAttachments(0).ImageFile.Length)
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
        mMSP.IsAttachmentAdded = False
        mMSP.FileAttachments.Remove(mMSP.ID)
        Session("mMSP") = mMSP
        Session("IsAttachmentNotSave") = mIsAttachmentNotSave
        ControlVisibilityForFileAttachment()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        Try
            If mFileAttach.ReferenceID.Equals(mMSP.ID) Then
                If mMSP.IsAttachmentAdded Then
                    mMSP.FileAttachments(0).Size = mFileAttach.Size
                    mMSP.FileAttachments(0).ImageFile = mFileAttach.ImageFile
                    mMSP.FileAttachments(0).Extension = mFileAttach.Extension
                Else
                    mMSP.IsAttachmentAdded = True
                    mMSP.FileAttachments.Add(mFileAttach.ReferenceID, mFileAttach.ImageFile, mFileAttach.Size, mFileAttach.Extension, mFileAttach.Sort)
                End If
            Else

            End If
        Catch ex As Exception
        End Try

        ControlVisibilityForFileAttachment()
        dgMSPAssembly.DataSource = mMSP.MSPAssemblys
        dgMSPAssembly.DataBind()
        Session("IsAttachmentNotSave") = True
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If (Not IsInRole(Rights.Authorized) And (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user", False), True)
            Exit Sub
        End If
        If mMSP.IsAttachmentAdded = True Then
            mFileAttach = FileAttach.GetAttachmentChild(mMSP.ID)
        Else
            mFileAttach = FileAttach.NewAttachmentChild(Guid.Empty, mMSP.ID)
        End If

        Session("mFileAttach") = mFileAttach
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow()", True)
    End Sub
    Private Sub hdnBtnMSPAssembly_Click(sender As Object, e As EventArgs) Handles hdnBtnMSPAssembly.Click
        dgMSPAssembly.DataSource = mMSP.MSPAssemblys
        dgMSPAssembly.DataBind()
        upnlMSPAssembly.Update()
    End Sub
#End Region

#Region " Status "
#End Region

#Region " Show BrokenRules "
    Public Function CustomValidate1() As Boolean
        Dim strMsg As String = ""
        SetObject()
        If mMSP.IsValid = False Then
            For i As Integer = 0 To mMSP.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mMSP.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        Dim mMSPAssembly As MSPAssembly
        If mMSP.MSPAssemblys.IsValid = False Then
            For Each mMSPAssembly In mMSP.MSPAssemblys
                For i As Integer = 0 To mMSPAssembly.GetBrokenRulesCollection.Count - 1
                    strMsg = strMsg + mMSPAssembly.AssemblyName + " : " + mMSPAssembly.GetBrokenRulesCollection(i).Description + "<Br>"
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


#End Region

End Class