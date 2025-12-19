'AJAX Conversion By Vikrant

Partial Class wfEmployeeService_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mEmployee As Employee
    Public mEmployeeService As EmployeeService

    Public mServiceList As ServiceList

    Public BackPage As String

    Dim EventLogID As Guid 'Added by Saylee on 20-July-2011
    Public mService As Service
#End Region

#Region " Helper Methods "
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Public Sub GetSession()
        mEmployee = Session("mEmployee")
        mEmployeeService = Session("mEmployeeService")
        'mEmployeeServiceList = Session("mEmployeeServiceList")
        mServiceList = Session("mServiceList")
        mService = Session("mService")
    End Sub
    Private Sub SetSession()
        Session("mEmployeeService") = mEmployeeService
        'Session("mEmployeeSkillList") = mEmployeeSkillList
        Session("mServiceList") = mServiceList
        Session("mEmployee") = mEmployee
    End Sub
    Private Sub SetSessionService()
        Session("mService") = mService
        Session("mServiceList") = mServiceList
    End Sub
    Private Sub RemoveSession()

    End Sub
    Private Sub RemoveSessionService()
        'Session.Remove("mServiceList")
        Session.Remove("mService")
    End Sub
    Public Sub Customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValid As CustomValidator
        CustValid = CType(s, CustomValidator)

        If CustValid.ControlToValidate = "txtName" Then
            If Len(Trim(txtName.Text)) > 50 Then
                CustValid.ErrorMessage = " Service Name too long "
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
    Private Sub DataFieldBind()
        mServiceList = ServiceList.GetServiceList(, "<SELECT>")
        cmbServiceList.DataSource = mServiceList
        Session("mServiceList") = mServiceList

        txtDate.Text = mEmployeeService.EmployeeServiceDateFormatted.ToString()

        upnlServiceDetails.DataBind()
    End Sub
    Private Sub DataFieldBindService()
        mServiceList = ServiceList.GetServiceList()
        dgService.DataSource = mServiceList
        Session("mServiceList") = mServiceList
        upnlService.DataBind()
    End Sub
    Private Sub EditRecordService(ByVal mID As Guid)
        mService = Service.GetService(mID)
        Session("mService") = mService
    End Sub
    Private Sub DeleteRecordService(ByVal mID As Guid)
        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Delete, SIMsgBox.Message_text.Delete, "", MsgBoxStyle.YesNo)
        'msg1.ReplacePage = "wfService.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2")
        'Session("sender") = "Delete"
        'msg1.Show()
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteService")
        mService = Service.GetService(mID)
        Session("mService") = mService
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "DeleteService" Then
                        Try
                            Session("sender") = ""
                            mService = Session("mService")
                            Service.DeleteService(mService.ID)
                            NewRecordService()
                            DataFieldBindService()
                            lblTitleService.Text = "Service Information [New]"
                            upnlService.Update()
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MarkLog(Flypal.Util.Action.Delete, "Service", "Can't delete : " + mService.Name + "  is Currently in use", Flypal.Util.ErrorType.NoError, mService.ID, EventLogID)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            NewRecordService()
                            txtName.DataBind()
                            lblTitleService.Text = "Service Information [New]"
                            upnlService.Update()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                MarkLog(Flypal.Util.Action.Delete, "Service", mService.Name, Flypal.Util.ErrorType.NoError, mService.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "DeleteService" Then
                        NewRecordService()
                        txtName.DataBind()
                        lblTitleService.Text = "Service Information [New]"
                        upnlService.Update()
                    End If
                    Session("sender") = ""
                    'Response.Redirect("wfEmployeeService_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type"))
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    'DataFieldBind()
                    'Response.Redirect("wfEmployeeService_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    'DataFieldBind()
                    'Response.Redirect("wfEmployeeService_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            'DataFieldBind()
            'Response.Redirect("wfEmployeeService_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type"))
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            'DataFieldBind()
        End If
    End Sub
    Private Sub SetTitle()
        If mEmployeeService.IsNew Then
            lblTitle.Text = "Employee Service Information [New]"
        Else
            If Len(mEmployeeService.ServiceName) > 15 Then
                lblTitle.Text = "Employee Service Information [" & mEmployeeService.ServiceName.Substring(0, 15) & "...]"
            Else
                lblTitle.Text = "Employee Service Information [" & mEmployeeService.ServiceName & "]"
            End If
        End If
    End Sub
    Private Sub NewRecordService()
        mService = Service.NewService
        Session("mService") = mService
    End Sub
    Private Sub SetObject()
        mEmployeeService.EmployeeID = mEmployee.ID
        mEmployeeService.Date = CType(txtDate.Text, Object)
        mEmployeeService.ServiceID = New Guid(cmbServiceList.SelectedValue)
    End Sub
    Private Sub SetObjectService()
        mService.Name = txtName.Text
    End Sub
    Private Sub AttachMyFile()
        Try
            mEmployeeService.ImageFile = CType(Session("FileUpload.FileContent"), Byte())
            mEmployeeService.ImageSize = Session("FileUpload.FileSize")
            mEmployeeService.FileExtension = Session("FileUpload.FileExtension")
            Session("mEmployeeService") = mEmployeeService
            Session.Remove("FileUpload.FileSize")
            Session.Remove("FileUpload.FileContent")
            Session.Remove("FileUpload.FileExtension")
            ControlVisibilityForServiceAttachment()
        Catch ex As Exception
            MSGBoxCtrl.show("Attachment Alert!", ex.Message, "", MsgBoxStyle.Information, "")
        End Try
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Saylee on 20-July-2011
        If Not IsPostBack And Session("sender") = "" Then
            setFocus(cmbServiceList)
            DataFieldBind()
            SetTitle()
            ControlVisibilityForServiceAttachment()
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("EmployeeServicesNew") And mEmployeeService.IsNew) Or (Not User.IsInRole("EmployeeServicesEdit") And Not mEmployeeService.IsNew) Then
            SetObject()
            SetSession()
            MarkLog(Flypal.Util.Action.Save, "Employee Service", User.Identity.Name & " is not Authorized User to save" + " Emp : " + mEmployee.EmpNoName + " Service : " + mEmployeeService.ServiceName, Flypal.Util.ErrorType.HandledError, mEmployeeService.ID, EventLogID)
            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
            'msg.ReplacePage = "wfEmployeeService_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type")
            'Session("sender") = "Authorization"
            'msg.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If IsValid Then
            Try
                SetObject()
                If mEmployeeService.IsValid Then
                    mEmployeeService.Save()
                    SetSession()
                    lblTitle.Text = "Employee Service Information [New]"
                    MarkLog(Flypal.Util.Action.Save, "Employee Service", "Emp : " + mEmployee.EmpNoName + " Service : " + mServiceList(mEmployeeService.ServiceID).Name, Flypal.Util.ErrorType.NoError, mEmployeeService.ID, EventLogID)
                    'Modal PopUP Close
                    Dim mopenas As String = Request.QueryString("Type")
                    If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                        RemoveSession()
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                        Exit Sub
                    End If
                    'End
                    Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                End If

            Catch ex As SqlException
                If ex.Number = 8145 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfEmployeeService_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type")
                    'Session("sender") = "Delete"
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DataBaseError, SIMsgBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfEmployeeService_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type")
                    'Session("sender") = "Delete"
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.ReferenceDelete, SIMsgBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OKOnly)
                    'msg1.ReplacePage = "wfEmployeeService_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type")
                    'Session("sender") = "Delete"
                    'msg1.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
            End Try
        Else
            upnlValidations.Update()
        End If
    End Sub
    Private Sub clearControlsForService()
        txtName.Text = ""
        lblTitleService.Text = "Service Information [New]"
    End Sub
    Private Sub imgService_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgService.Click
        SetObject()
        NewRecordService()
        DataFieldBindService()
        clearControlsForService()
        mdlPopUpService.Show()
        upnlService.Update()
        'Response.Redirect("wfService.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=wfEmployeeService_Ajax.aspx")
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        SetSession()
        'Response.Redirect("wfEmployeeDetails.aspx")
        If Not mEmployeeService.IsNew Then
            MarkLog(Flypal.Util.Action.Close, "Employee Service", "Emp : " + mEmployee.EmpNoName + " Service : " + mEmployeeService.ServiceName, Flypal.Util.ErrorType.NoError, mEmployeeService.ID, EventLogID)
        End If
        'Modal PopUP Close
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            RemoveSession()
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'End
        Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        '----------------------------------------------------------------------
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        '----------------------------------------------------------------------
        If mEmployeeService.ImageSize > 0 Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mEmployeeService.FileExtension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mEmployeeService.FileExtension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mEmployeeService.ImageFile, 0, mEmployeeService.ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                Dim Str As String
                Str = "openFile();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
            End If
        End If
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte
        mEmployeeService.ImageFile = file1
        mEmployeeService.ImageSize = 0
        mEmployeeService.FileExtension = ""
        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        AttachMyFile()
        upnlServiceDetails.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub ControlVisibilityForServiceAttachment()
        If mEmployeeService.ImageSize > 0 Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
        End If
    End Sub
    Private Sub txtDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDate.TextChanged
        If IsDate(txtDate.Text) Or (txtDate.Text = "") Then
            If txtDate.Text = "" Then
                mEmployeeService.Date = System.DBNull.Value
                txtDate.Text = mEmployeeService.Date.ToString
            Else
                mEmployeeService.Date = txtDate.Text
                txtDate.Text = mEmployeeService.EmployeeServiceDateFormatted
            End If
        Else
            txtDate.Text = ""
        End If
    End Sub
    Private Sub btnNewService_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewService.Click
        If txtName.Enabled = True Then
            setFocus(txtName)
        End If
        MarkLog(Flypal.Util.Action.[New], "Service", "", Flypal.Util.ErrorType.NoError, mService.ID, EventLogID)
        NewRecordService()
        clearControlsForService()
        DataFieldBindService()
    End Sub
    Private Sub dgService_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgService.RowCommand
        Dim mID As Guid
        Dim mName As String
        Select Case e.CommandName
            Case "EditRec"
                mID = CType(dgService.DataKeys(CInt(e.CommandArgument)).Value, Guid)
                If (Not User.IsInRole("EmployeeServicesView") And Not User.IsInRole("EmployeeServicesEdit")) Then
                    SetObject()
                    SetSession()
                    MarkLog(Flypal.Util.Action.Edit, "Service", User.Identity.Name & " is not Authorized User to edit " + mName, Flypal.Util.ErrorType.HandledError, mID, EventLogID)
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                EditRecordService(mID)
                txtName.DataBind()
                MarkLog(Flypal.Util.Action.Edit, "Service", mService.Name, Flypal.Util.ErrorType.NoError, mService.ID, EventLogID)
                If Len(mService.Name) > 15 Then
                    lblTitleService.Text = "Service [" & mService.Name.Substring(0, 15) & "... ]"
                Else
                    lblTitleService.Text = "Service [" & mService.Name & " ]"
                End If
                If txtName.Enabled = True Then
                    setFocus(txtName)
                End If
            Case "DeleteRec"
                mID = CType(dgService.DataKeys(CInt(e.CommandArgument)).Value, Guid)
                If (Not User.IsInRole("EmployeeServicesDelete")) Then
                    SetObject()
                    SetSession()
                    MarkLog(Flypal.Util.Action.Delete, "Service", User.Identity.Name & " is not Authorized User to delete " + mName, Flypal.Util.ErrorType.HandledError, mID, EventLogID)
                    'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly)
                    'msg.ReplacePage = "wfService.aspx?&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2")
                    'Session("sender") = "Authorization"
                    'msg.Show()
                    MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
                    Exit Sub
                End If
                DeleteRecordService(mID)
        End Select
    End Sub
    Private Sub btnCloseService_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseService.Click
        RemoveSessionService()
        DataFieldBind()
        upnlServiceDetails.Update()
        mdlPopUpService.Hide()
    End Sub
    Private Sub btnSaveService_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveService.Click
        If (Not User.IsInRole("EmployeeServicesNew") And mService.IsNew) Or (Not User.IsInRole("EmployeeServicesEdit") And Not mService.IsNew) Then
            SetObjectService()
            SetSessionService()
            MarkLog(Flypal.Util.Action.Save, "Service", User.Identity.Name & " is not Authorized User to save " + mService.Name, Flypal.Util.ErrorType.HandledError, mService.ID, EventLogID)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        If IsValid Then
            Try
                SetObjectService()
                mService.Save()
                If txtName.Enabled = True Then
                    setFocus(txtName)
                End If
                MarkLog(Flypal.Util.Action.Save, "Service", mService.Name, Flypal.Util.ErrorType.HandledError, mService.ID, EventLogID)
                NewRecordService()
                txtName.DataBind()
                DataFieldBindService()
                SetSessionService()
                lblTitleService.Text = "Service Information [New]"
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2601 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
            End Try
        End If
    End Sub

    Private Sub dgService_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgService.PageIndexChanging
        dgService.PageIndex = e.NewPageIndex
        dgService.DataSource = mServiceList
        Session("mServiceList") = mServiceList
        dgService.DataBind()
    End Sub
#End Region

End Class
