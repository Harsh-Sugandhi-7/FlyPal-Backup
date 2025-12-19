'AJAX Conversion By Vikrant

Partial Class wfEmployeeDisciplinary_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mEmployee As Employee
    Public mEmployeeDisciplinary As EmployeeDisciplinary

    Public mDisciplinaryList As DisciplinaryList

    Public BackPage As String
    Dim EventLogID As Guid 'Added by Saylee on 20-July-2011
    Public mDisciplinary As Disciplinary
#End Region

#Region " Helper Methods "
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Public Sub GetSession()
        mEmployee = Session("mEmployee")
        mEmployeeDisciplinary = Session("mEmployeeDisciplinary")
        'mEmployeeDisciplinaryList = Session("mEmployeeDisciplinaryList")
        mDisciplinaryList = Session("mDisciplinaryList")
        mDisciplinary = Session("mDisciplinary")
    End Sub
    Private Sub SetSession()
        Session("mEmployeeDisciplinary") = mEmployeeDisciplinary
        'Session("mEmployeeDisciplinaryList") = mEmployeeDisciplinaryList
        Session("mDisciplinaryList") = mDisciplinaryList
        Session("mEmployee") = mEmployee
    End Sub
    Private Sub DataFieldBind()
        mDisciplinaryList = DisciplinaryList.GetDisciplinaryList("(SELECT)")
        cmbDisciplinaryList.DataSource = mDisciplinaryList
        Session("mDisciplinaryList") = mDisciplinaryList

        calIncidentDate.Text = mEmployeeDisciplinary.IncidentDateFormatted
        upnlDisciplinaryDetails.DataBind()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "DeleteDisciplinaryMaster" Then
                        Try
                            Session("sender") = ""
                            mDisciplinary = Session("mDisciplinary")
                            Disciplinary.DeleteDisciplinary(mDisciplinary.ID)
                            NewRecordDisciplinaryMaster()
                            DataFieldBindDisciplinaryMaster()
                            lblTitleDisciplinaryMaster.Text = "Disciplinary Information [New]"
                            upnlDisciplinaryMaster.Update()
                            'Response.Redirect("wfDisciplinary.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                'Uncommented & updated by Vikrant on 20-july-2011
                                MarkLog(Flypal.Util.Action.Delete, "Disciplinary", "Can't delete :" & mDisciplinary.Name & " is Currently in use", Flypal.Util.ErrorType.NoError, mDisciplinary.ID, EventLogID)
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                            NewRecordDisciplinaryMaster()
                            txtName.DataBind()
                            lblTitleDisciplinaryMaster.Text = "Disciplinary Information [New]"
                            upnlDisciplinaryMaster.Update()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'Uncommented & updated by Vikrant on 20-july-2011
                                MarkLog(Flypal.Util.Action.Delete, "Disciplinary", mDisciplinary.Name, Flypal.Util.ErrorType.NoError, mDisciplinary.ID, EventLogID)
                            End If
                        End Try
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "DeleteDisciplinaryMaster" Then
                        NewRecordDisciplinaryMaster()
                        txtName.DataBind()
                        lblTitleDisciplinaryMaster.Text = "Disciplinary Information [New]"
                        upnlDisciplinaryMaster.Update()
                    End If
                    Session("sender") = ""

                    'Response.Redirect("wfEmployeeDisciplinary_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1"))
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    'DataFieldBind()
                    'Response.Redirect("wfEmployeeDisciplinary_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    'DataFieldBind()
                    'Response.Redirect("wfEmployeeDisciplinary_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type") & "&ChildPage1=" & Request.QueryString("ChildPage1"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            'DataFieldBind()
            'Response.Redirect("wfEmployeeDisciplinary_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&BackPage1=" & Request.QueryString("BackPage1") & "&BackPage2=" & Request.QueryString("BackPage2") & "&BackPage3=" & Request.QueryString("BackPage3") & "&Type=" & Request.QueryString("Type"))
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            'DataFieldBind()
        End If
    End Sub
    Private Sub SetTitle()
        If mEmployeeDisciplinary.IsNew Then
            lblTitle.Text = "Employee Disciplinary Information [New]"
        Else
            If Len(mEmployeeDisciplinary.DisciplinaryName) > 15 Then
                lblTitle.Text = "Employee Disciplinary Information [" & mEmployeeDisciplinary.DisciplinaryName.Substring(0, 15) & "...]"
            Else
                lblTitle.Text = "Employee Disciplinary Information [" & mEmployeeDisciplinary.DisciplinaryName & "]"
            End If
        End If
        upnlTitle.Update()
    End Sub
    Private Sub SetObject()
        mEmployeeDisciplinary.EmployeeID = mEmployee.ID
        mEmployeeDisciplinary.IncidentDate = CType(calIncidentDate.Text, Object)
        mEmployeeDisciplinary.DisciplinaryID = New Guid(cmbDisciplinaryList.SelectedValue)
        mEmployeeDisciplinary.Description = Trim(txtDescription.Text)
        mEmployeeDisciplinary.ReportedBy = Trim(txtReportedBy.Text)
        mEmployeeDisciplinary.Comments = Trim(txtComments.Text)
        mEmployeeDisciplinary.FeedBack = Trim(txtFeedBack.Text)
    End Sub
    Private Sub ControlVisibilityForAttachment()
        If mEmployeeDisciplinary.ImageSize > 0 Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
        End If
    End Sub
    Private Sub AttachMyFile()
        Try
            mEmployeeDisciplinary.ImageFile = CType(Session("FileUpload.FileContent"), Byte())
            mEmployeeDisciplinary.ImageSize = Session("FileUpload.FileSize")
            mEmployeeDisciplinary.FileExtension = Session("FileUpload.FileExtension")
            Session("mEmployeeDisciplinary") = mEmployeeDisciplinary
            Session.Remove("FileUpload.FileSize")
            Session.Remove("FileUpload.FileContent")
            Session.Remove("FileUpload.FileExtension")
            ControlVisibilityForAttachment()
        Catch ex As Exception
            MSGBoxCtrl.show("Attachment Alert!", ex.Message, "", MsgBoxStyle.Information, "")
        End Try
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)

        ''If custValidator.ControlToValidate = "cmbDisciplinaryList" Then
        ''    If cmbDisciplinaryList.SelectedIndex <= 0 Then
        ''        custValidator.ErrorMessage = "Please Select the Disciplinary."
        ''        e.IsValid = False
        ''    Else
        ''        e.IsValid = True
        ''    End If
        ''Else
        If custValidator.ControlToValidate = "txtDescription" Then
            If Len(txtDescription.Text) > 500 Then
                custValidator.ErrorMessage = "Description cannot be greater than 500 characters."
                e.IsValid = False
            End If

        ElseIf custValidator.ControlToValidate = "txtComments" Then
            If Len(txtComments.Text) > 500 Then
                custValidator.ErrorMessage = "Comments cannot be greater than 500 characters."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtFeedBack" Then
            If Len(txtFeedBack.Text) > 500 Then
                custValidator.ErrorMessage = "FeedBack cannot be greater than 500 characters."
                e.IsValid = False
            End If
        End If
    End Sub
    Public Sub Customvalidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim CustValid As CustomValidator
        CustValid = CType(s, CustomValidator)

        If CustValid.ControlToValidate = "txtName" Then
            If Len(Trim(txtName.Text)) > 5000 Then
                CustValid.ErrorMessage = " Disciplinary Name too long."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
    Private Sub NewRecordDisciplinaryMaster()
        mDisciplinary = Disciplinary.NewDisciplinary()
        Session("mDisciplinary") = mDisciplinary
    End Sub
    Private Sub EditRecordDisciplinaryMaster(ByVal mID As Guid)
        mDisciplinary = Disciplinary.GetChildDisciplinary(mID)
        Session("mDisciplinary") = mDisciplinary
        setFocus(txtName)
    End Sub
    Private Sub DeleteRecordDisciplinaryMaster(ByVal mID As Guid)
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "DeleteDisciplinaryMaster")
        mDisciplinary = Disciplinary.GetChildDisciplinary(mID)
        Session("mDisciplinary") = mDisciplinary
    End Sub
    Private Sub SetObjectDisciplinaryMaster()
        mDisciplinary.Name = txtName.Text
    End Sub
    Private Sub DataFieldBindDisciplinaryMaster()
        mDisciplinaryList = DisciplinaryList.GetDisciplinaryList()
        dgDisciplinary.DataSource = mDisciplinaryList
        Session("mDisciplinaryList") = mDisciplinaryList
        upnlDisciplinaryMaster.DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Saylee on 20-July-2011
        If Not IsPostBack And Session("sender") = "" Then
            setFocus(txtDescription)
            calIncidentDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
            DataFieldBind()
            ControlVisibilityForAttachment()
            SetTitle()
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("EmployeeDisciplinaryNew") And mEmployeeDisciplinary.IsNew) Or (Not User.IsInRole("EmployeeDisciplinaryEdit") And Not mEmployeeDisciplinary.IsNew) Then
            SetObject()
            SetSession()
            'MarkLog(Flypal.Util.Action.Save, "EmployeeService", "Not Authorized User", Flypal.Util.ErrorType.HandledError, Guid.Empty)
            MarkLog(Flypal.Util.Action.Save, "Employee Disciplinary", User.Identity.Name & " is not Authorized User to save" + " Emp : " + mEmployee.EmpNoName + " Disciplinary : " + mEmployeeDisciplinary.Description, Flypal.Util.ErrorType.HandledError, mEmployeeDisciplinary.ID, EventLogID)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        If IsValid Then
            Try
                SetObject()
                mEmployeeDisciplinary.Save()
                SetSession()
                lblTitle.Text = "Employee Disciplinary Information [New]"
                setFocus(txtDescription)
                upnlDisciplinaryDetails.Update()
                MarkLog(Flypal.Util.Action.Save, "Employee Disciplinary", "Emp : " + mEmployee.EmpNoName + " Disciplinary : " + mEmployeeDisciplinary.Description, Flypal.Util.ErrorType.NoError, mEmployeeDisciplinary.ID, EventLogID)
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                'Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2601 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
                'CHK
            End Try
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub imgDisciplinary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgDisciplinary.Click
        SetObject()
        NewRecordDisciplinaryMaster()
        DataFieldBindDisciplinaryMaster()
        mdlPopUpDisciplinaryMaster.Show()
        upnlDisciplinaryMaster.Update()
        'Response.Redirect("wfDisciplinary.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=wfEmployeeDisciplinary_Ajax.aspx")
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        SetSession()
        If Not mEmployeeDisciplinary.IsNew Then
            MarkLog(Flypal.Util.Action.Close, "Employee Disciplinary", "Emp : " + mEmployee.EmpNoName + " Disciplinary : " + mEmployeeDisciplinary.Description, Flypal.Util.ErrorType.NoError, mEmployeeDisciplinary.ID, EventLogID)
        End If
        'Added by Vikrant for popup
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'End
        'Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        '----------------------------------------------------------------------
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        '----------------------------------------------------------------------
        If mEmployeeDisciplinary.ImageSize > 0 Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mEmployeeDisciplinary.FileExtension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mEmployeeDisciplinary.FileExtension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mEmployeeDisciplinary.ImageFile, 0, mEmployeeDisciplinary.ImageFile.Length)
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
        mEmployeeDisciplinary.ImageFile = file1
        mEmployeeDisciplinary.ImageSize = 0
        mEmployeeDisciplinary.FileExtension = ""
        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        AttachMyFile()
        upnlDisciplinaryDetails.Update()
    End Sub
    Private Sub btnCloseDisciplinaryMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseDisciplinaryMaster.Click
        DataFieldBind()
        mdlPopUpDisciplinaryMaster.Hide()
        upnlDisciplinaryDetails.Update()
    End Sub
    Private Sub dgDisciplinary_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgDisciplinary.RowCommand
        Dim Index As Int32
        Dim mID As Guid
        Select Case e.CommandName
            Case "EditRec"
                Index = CInt(e.CommandArgument) + dgDisciplinary.PageIndex * dgDisciplinary.PageSize
                mID = CType(dgDisciplinary.DataKeys(CInt(e.CommandArgument)).Value, Guid)
                EditRecordDisciplinaryMaster(mID)
                txtName.DataBind()
                'Uncommented & updated by Vikrant on 20-july-2011
                MarkLog(FlyPal.Util.Action.Edit, "Disciplinary", mDisciplinary.Name, FlyPal.Util.ErrorType.NoError, mDisciplinary.ID, EventLogID)
                If Len(mDisciplinary.Name) > 15 Then
                    lblTitleDisciplinaryMaster.Text = "Disciplinary Information [" & mDisciplinary.Name.Substring(0, 15) & "... ]"
                Else
                    lblTitleDisciplinaryMaster.Text = "Disciplinary Information [" & mDisciplinary.Name & " ]"
                End If
                If txtName.Enabled = True Then
                    setFocus(txtName)
                End If
            Case "DeleteRec"
                Index = CInt(e.CommandArgument) + dgDisciplinary.PageIndex * dgDisciplinary.PageSize
                mID = CType(dgDisciplinary.DataKeys(CInt(e.CommandArgument)).Value, Guid)

                DeleteRecordDisciplinaryMaster(mID)
        End Select
    End Sub
    Private Sub btnSaveDisciplinaryMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDisciplinaryMaster.Click
        If (Not User.IsInRole("EmployeeDisciplinaryNew") And mDisciplinary.IsNew) Or (Not User.IsInRole("EmployeeDisciplinaryEdit") And Not mDisciplinary.IsNew) Then
            SetObjectDisciplinaryMaster()
            SetSession()
        End If
        If IsValid Then
            SetObjectDisciplinaryMaster()
            Try
                mDisciplinary.Save()
                If txtName.Enabled = True Then
                    setFocus(txtName)
                End If
                'Uncommented & updated by Vikrant on 20-july-2011
                MarkLog(Flypal.Util.Action.Save, "Disciplinary", mDisciplinary.Name, Flypal.Util.ErrorType.HandledError, mDisciplinary.ID, EventLogID)

                NewRecordDisciplinaryMaster()
                txtName.Text = ""
                DataFieldBindDisciplinaryMaster()
                lblTitleDisciplinaryMaster.Text = "Disciplinary Information [New]"
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2601 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
            End Try
        End If
    End Sub

    Private Sub btnNewDisciplinaryMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewDisciplinaryMaster.Click
        If txtName.Enabled = True Then
            setFocus(txtName)
        End If
        NewRecordDisciplinaryMaster()
        txtName.Text = ""
        DataFieldBind()
        MarkLog(Flypal.Util.Action.[New], "Disciplinary", "", Flypal.Util.ErrorType.NoError, mDisciplinary.ID, EventLogID) 'Uncommented & updated by Vikrant on 20-july-2011
        lblTitleDisciplinaryMaster.Text = "Disciplinary Information [New]"
    End Sub
#End Region

End Class
