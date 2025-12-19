'Created By     :   Saylee
'Dated          :   19-Aug-2015

Public Class wfAudit_AJAX
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mAudit As Audit
    Public mAuditList As AuditList
    Public mAuditTypeList As AuditTypeList
    Public mAuditStandardList As AuditStandardList
    Protected mAuditSchedule As AuditSchedule
    Protected mPreviousAuditSchedule As PreviousAuditSchedule
    Protected mAuditScheduleList As AuditScheduleList
    Public strMsg As String = ""
    Dim EventLogID As Guid
    Dim mAuditDetail As String
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mAudit = CType(Session("mAudit"), Audit)
        mAuditList = CType(Session("mAuditList"), AuditList)
        mAuditTypeList = CType(Session("mAuditTypeList"), AuditTypeList)
        mAuditStandardList = Session("mAuditStandardList")
        mAuditSchedule = CType(Session("mAuditSchedule"), AuditSchedule)
        mAuditScheduleList = CType(Session("mAuditScheduleList"), AuditScheduleList)
        'mFileAttach = Session("mFileAttach")
        mFileAttach = Session("mFileAttachOnAudit")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
    End Sub
    Private Sub SetSession()
        Session("mAudit") = mAudit
        Session("mAuditList") = mAuditList
        Session("mAuditTypeList") = mAuditTypeList
        Session("mAuditStandardList") = mAuditStandardList
        Session("mAuditSchedule") = mAuditSchedule
        Session("mAuditScheduleList") = mAuditScheduleList
        'Session("mFileAttach") = mFileAttach
        Session("mFileAttachOnAudit") = mFileAttach
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub NewRecord()
        mAudit = Audit.NewAudit()
        Session("mAudit") = mAudit
    End Sub
    Private Function Save() As Boolean
        Try
            setObject()
            mAudit.Save()
            'Changed by Vikrant on 25-July-2011
            mAuditDetail = "Audit No : " + mAudit.AuditNo + " Audit Standard : " + cmbStandard.SelectedItem.Text + " Audit Type : " + cmbAuditTypeList.SelectedItem.Text
            MarkLog(Flypal.Util.Action.Save, "Audit", mAuditDetail, Flypal.Util.ErrorType.HandledError, mAudit.ID, EventLogID)
            mAudit = Audit.NewAudit()
            NewRecord()
            DataFieldBind()
            SetSession()
            SetTitle()
            If cmbAuditTypeList.Enabled = True Then
                setFocus(cmbAuditTypeList)
            End If
            Return True
        Catch ex As SqlException
            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2601 Or ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 547 Then
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure + "," + ex.Message, MsgBoxStyle.OkOnly, "")
            End If
            DataFieldBind()
        End Try
    End Function
    Private Sub setObject()
        mAudit.AuditNo = Trim(txtAuditNo.Text)
        mAudit.Reference = Trim(txtReferenceNo.Text)
        mAudit.Description = Trim(txtDescription.Text)
        mAudit.AuditTypeID = Val(cmbAuditTypeList.SelectedValue.ToString)
        mAudit.OtherInformation = Trim(txtOtherInformation.Text)
        mAudit.AuditStandardID = New Guid(cmbStandard.SelectedValue.ToString)
        mAudit.IsNextSchedule = chkIsScheduleNextAudit.Checked
        mAudit.Frequency = Trim(txtFrequency.Text)
        mAudit.ExePeriod = Trim(txtExePeriod.Text)

        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                mAudit.IsAttachmentAdded = True
            Else
                mAudit.IsAttachmentAdded = False
            End If
        End If
        Session("mAudit") = mAudit
    End Sub
    'Private Sub AttachMyFile()
    '    If MyFile.Value <> "" Then
    '        Dim BackupPath As String = ""
    '        BackupPath = AppSettings("DOCPath") & "New.PDF"

    '        Try
    '            MyFile.PostedFile.SaveAs(BackupPath)
    '            Dim fs As New FileStream(BackupPath, FileMode.OpenOrCreate, FileAccess.ReadWrite)
    '            Dim fileSize As Integer = CType(fs.Length, Integer)

    '            Dim fileBytes(fileSize) As Byte
    '            fs.Read(fileBytes, 0, fileSize)
    '            mAudit.ImageFile = fileBytes
    '            mAudit.ImageSize = fileSize
    '            mAudit.FileExtension = MyFile.Value
    '            btnDelAttach.Enabled = True
    '            fs.Close()
    '            System.IO.File.Delete(BackupPath)

    '        Catch ex As Exception
    '            Throw ex

    '        End Try
    '    End If
    '    If mAudit.ImageSize > 0 Then
    '        ImageButton1.Visible = True
    '        btnDelAttach.Enabled = True
    '    Else
    '        ImageButton1.Visible = False
    '    End If
    'End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim msgCount As Integer = 0


        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "DeleteAuditMasterTask" Then
                        Try
                            Session("Sender") = ""
                            mAudit = CType(Session("mAudit"), Audit)
                            mAudit.AuditMasterTasks.Remove(mAudit.AuditMasterTasks.CurrentItem)
                            Session("mAudit") = mAudit
                            DataFieldBind()
                            ControlVisibility()
                            SetTitle()
                            upnlAuditMasterTask.Update()
                            upnlGrid.Update()
                            ' Response.Redirect("wfAudit.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2601 Or ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")

                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure + "," + ex.Message, MsgBoxStyle.OkOnly, "")
                            End If
                            DataFieldBind()
                            msgCount = ex.Errors.Count
                        Finally
                            If msgCount = 0 Then
                                'Changed by Vikrant on 25-July-2011
                                mAuditDetail = "Audit No : " + mAudit.AuditNo + " Audit Standard : " + cmbStandard.SelectedItem.Text + " Audit Type : " + cmbAuditTypeList.SelectedItem.Text
                                MarkLog(Flypal.Util.Action.Delete, "Audit", mAuditDetail, Flypal.Util.ErrorType.NoError, mAudit.ID, EventLogID)
                            End If
                        End Try

                    ElseIf MSGBoxCtrl.Sender = "Close" Then  '' Close confirmation
                        Session("sender") = ""
                        If mAudit.IsValid = True Then
                            Session.Remove("IsValid")
                            DataFieldBind()

                            If (Not User.IsInRole("AuditNew") And Not User.IsInRole("AuditEdit")) Then
                                MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If

                            If Save() Then
                                mAudit = Session("mAudit")
                                setObject()
                                Session("mAudit") = mAudit
                                Session.Remove("mAudit")

                                'If Request.QueryString("ChildPage") <> "" Then
                                '    Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
                                'Else
                                '    Response.Redirect(Request.QueryString("BackPage"))
                                'End If
                                Response.Redirect("index.aspx")
                            End If
                        Else
                            Session.Remove("IsValid")
                            'Ajay 22-11-2023
                            If mAudit.IsValid = False Then
                                For j As Integer = 0 To mAudit.GetBrokenRulesCollection.Count - 1
                                    strMsg = strMsg + mAudit.GetBrokenRulesCollection(j).Description + "<BR>"
                                Next
                            End If

                            If strMsg.Trim <> "" Then
                                cvFrequency.ErrorMessage = strMsg
                                cvFrequency.IsValid = mAudit.IsValid
                            End If
                            upnlValidation.Update()
                            ' Response.Redirect("wfAudit.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                        End If
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        Session.Remove("IsValid")
                        Session("Sender") = ""
                        If mAudit.IsNew Then Session.Remove("mAudit")
                        mAudit = Session("mAudit")
                        '  setObject()
                        Session("mAudit") = mAudit
                        Session.Remove("mAudit")

                        'If Request.QueryString("ChildPage") <> "" Then
                        '    Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
                        'Else
                        '    Response.Redirect(Request.QueryString("BackPage"))
                        'End If
                        Response.Redirect("index.aspx")
                    Else
                        Session("sender") = ""

                    End If
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    DataFieldBind()

                Case MsgBoxResult.Ok And MSGBoxCtrl.Sender = "Authorization"  'Code Added
                    Session("sender") = ""
                    DataFieldBind()
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            DataFieldBind()
        ElseIf Result1 = 0 And MSGBoxCtrl.Sender = "Authorization" Then   'Code Added
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
    Private Sub SetTitle()
        If mAudit.IsNew Then
            lbltitle.Text = "Audit [New]"
        Else
            If Len(mAudit.AuditNo) > 15 Then
                lbltitle.Text = "Audit [" & mAudit.AuditNo.Substring(0, 15) & "...]"
            Else
                lbltitle.Text = "Audit [" & mAudit.AuditNo & "]"

            End If
        End If

        ''lblResult.Text = "Audit List: " & mAuditList.Count & " Record(s) Found."
    End Sub
    Private Sub ControlVisibilityForAttachment()
        If mAudit.IsAttachmentAdded Then
            ImageButton1.Visible = True
            ImageButton1.Enabled = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If

    End Sub
    Private Sub ControlVisibility()

        If Not mAudit.IsNew And mAuditScheduleList.Contains(mAudit.ID) Then
            chkIsScheduleNextAudit.Enabled = False
            txtFrequency.Enabled = False
            cmbStandard.Enabled = False
        Else
            If chkIsScheduleNextAudit.Checked = False Then
                txtFrequency.Enabled = False
            Else
                txtFrequency.Enabled = True
            End If
        End If
        btnAddTask.Enabled = Not mAudit.IsNew
        ControlVisibilityForAttachment()
    End Sub
    Private Sub addAttributes()
        txtFrequency.Attributes.Add("onKeyPress", "validateText(('D'), document.getElementById('txtFrequency').value,event)")
        txtExePeriod.Attributes.Add("onKeyPress", "validateText(('D'), document.getElementById('txtExePeriod').value,event)")
    End Sub
    Private Sub GetAttachment()

        If mAudit.IsAttachmentAdded = True And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mAudit.ID)
            'Session("mFileAttach") = mFileAttach
            Session("mFileAttachOnAudit") = mFileAttach
        End If
    End Sub
    Private Sub SaveAttachment() '

        If mFileAttach Is Nothing And mAudit.IsAttachmentAdded = True Then
            mFileAttach = FileAttach.GetAttachment(mAudit.ID)
            'Session("mFileAttach") = mFileAttach
            Session("mFileAttachOnAudit") = mFileAttach
        End If
        If Not mFileAttach Is Nothing Then
            mFileAttach.ReferenceID = mAudit.ID
            If mFileAttach.Size > 0 Then
                Try
                    mFileAttach.Save()
                Catch ex As Exception
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
                End Try
            Else
                If (Not mAudit.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mAudit.ID)
                End If
                IsAttachmentDeleted = False
                Session("IsAttachmentDeleted") = IsAttachmentDeleted
            End If
        End If
    End Sub
    Private Sub ViewImage()
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        ' GetAttachment()
        mFileAttach = FileAttach.GetAttachment(mAudit.ID)
        'Session("mFileAttach") = mFileAttach
        Session("mFileAttachOnAudit") = mFileAttach
        If Not mFileAttach Is Nothing Then


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
    End Sub

    Private Sub DeleteAuditMasterTask(ByVal Index As Int32)
        'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.Remove, SIMsgBox.Message_text.Remove, "", MsgBoxStyle.YesNo)
        'msg1.ReplacePage = "wfAuditSchedule.aspx?" & "&BackPage=" & Request.QueryString("BackPage")
        'Session("sender") = "DeleteAuditScheduleTask"
        'msg1.Show()
        MSGBoxCtrl.show(MSGBox.Message_title.Remove, MSGBox.Message_text.Remove, "", MsgBoxStyle.YesNo, "DeleteAuditMasterTask")
        mAudit.AuditMasterTasks.CurrentIndex = Index
        Session("mAudit") = mAudit
    End Sub
#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mAuditTypeList = AuditTypeList.GetAuditTypeList("(SELECT)")
        Session("mAuditTypeList") = mAuditTypeList
        cmbAuditTypeList.DataSource = mAuditTypeList

        mAuditStandardList = AuditStandardList.GetAuditStandardList("(SELECT)")
        Session("mAuditStandardList") = mAuditStandardList
        cmbStandard.DataSource = mAuditStandardList


        dgAuditMasterTask.DataSource = mAudit.AuditMasterTasks

        DataBind()

        mAuditScheduleList = AuditScheduleList.GetAuditScheduleList()
        Session("mAuditScheduleList") = mAuditScheduleList

        If mFileAttach Is Nothing Then
            If mAudit.IsAttachmentAdded = True Then
                mFileAttach = FileAttach.GetAttachment(mAudit.ID)
            Else
                mFileAttach = FileAttach.NewAttachment(Guid.Empty, mAudit.ID)
            End If
            Session("mFileAttach") = mFileAttach

        End If
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        ''If custValidator.ControlToValidate = "cmbAuditTypeList" Then
        ''    If cmbAuditTypeList.SelectedIndex <= 0 Then
        ''        custValidator.ErrorMessage = "Please select Audit Type"
        ''        e.IsValid = False
        ''    Else
        ''        e.IsValid = True
        ''    End If
        If custValidator.ControlToValidate = "txtDescription" Then
            If Len(txtDescription.Text) > 5000 Then
                custValidator.ErrorMessage = "Description should not be greater than 5000 characters."
                e.IsValid = False
            Else
                e.IsValid = True
            End If

        ElseIf custValidator.ControlToValidate = "cmbAuditTypeList" Then
            If cmbAuditTypeList.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Please Select Audit Type."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "cmbStandard" Then
            If cmbStandard.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Please Select Audit Standard."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        addAttributes()
        GetSession()
        If cmbAuditTypeList.Enabled = True Then
            setFocus(txtAuditNo)
        End If

        EventLogID = CType(Session("EventLogID"), Guid)
        If Not Page.IsPostBack Then
            DataFieldBind()
            ControlVisibility()
            SetTitle()
        End If

    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'If (Not User.IsInRole("AuditNew") And mAudit.IsNew) Or (Not User.IsInRole("AuditEdit") And Not mAudit.IsNew) Then
        '    setObject()
        '    SetSession()
        '    MarkLog(Flypal.Util.Action.Save, "Audit", "Not Authorized User", Flypal.Util.ErrorType.HandledError, Guid.Empty)
        '    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
        '    'msg.ReplacePage = "wfAudit.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage")
        '    msg.ReplacePage = "wfAudit.aspx?BackPage1=wfAudit.aspx" & "&BackPage=" & Request.QueryString("BackPage")
        '    Session("sender") = "Authorization"
        '    msg.Show()
        '    Exit Sub
        'End If
        If (Not User.IsInRole("AuditNew") And Not User.IsInRole("AuditEdit")) Then
            ' ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("You are not authorized user"))
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If Not IsValid Then upnlValidation.Update() : Exit Sub
        Try
            setObject()
            If mAudit.IsValid Then
                mAudit.Save()
                SaveAttachment()
                mAuditDetail = "Audit No : " + mAudit.AuditNo + " Audit Standard : " + cmbStandard.SelectedItem.Text + " Audit Type : " + cmbAuditTypeList.SelectedItem.Text
                MarkLog(Flypal.Util.Action.Save, "Audit", mAuditDetail, Flypal.Util.ErrorType.HandledError, mAudit.ID, EventLogID)
                SetSession()
                SetTitle()
                If cmbAuditTypeList.Enabled = True Then
                    setFocus(cmbAuditTypeList)
                End If
                ControlVisibility()
                upnlTitle.Update()
                upnlAuditMasterTask.Update()
                upnlGrid.Update()
                upnlIsNextSchedule.Update()
                upnlAuditMasterTask.Update()
                upnlAttachment.Update()
                MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
            Else
                If Not mAudit.IsValid Then
                    For j As Integer = 0 To mAudit.GetBrokenRulesCollection.Count - 1
                        strMsg = strMsg + mAudit.GetBrokenRulesCollection(j).Description + "<BR>"
                    Next
                End If

                If strMsg.Trim <> "" Then
                    cvFrequency.ErrorMessage = strMsg
                    cvFrequency.IsValid = mAudit.IsValid
                End If
                upnlValidation.Update()
            End If
        Catch ex As SqlException
            If ex.Number = 8145 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 2601 Or ex.Number = 2627 Then
                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
            ElseIf ex.Number = 547 Then
                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure + "," + ex.Message, MsgBoxStyle.OkOnly, "")
            End If
            DataFieldBind()
        End Try
    End Sub
    Private Sub btnAddTask_Click(sender As Object, e As System.EventArgs) Handles btnAddTask.Click
        setObject()
        Session("mAudit") = mAudit
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenTaskWindow", "OpenTaskWindow()", True)
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mAudit.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mAudit.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mAudit.ID)
        End If
        Session("mFileAttach") = mFileAttach
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        mFileAttach = Session("mFileAttach")
        Session("mFileAttachOnAudit") = Session("mFileAttach") ' mFileAttach
        mAudit.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlAttachment.Update()
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        ViewImage()
    End Sub
    Private Sub chkIsScheduleNextAudit_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsScheduleNextAudit.CheckedChanged
        If chkIsScheduleNextAudit.Checked = False Then
            txtFrequency.Text = "0"
            mAudit.Frequency = Val(txtFrequency.Text)
            txtFrequency.DataBind()
            Session("mAudit") = mAudit
            setFocus(txtFrequency)
            txtFrequency.Enabled = False
        Else
            txtFrequency.Enabled = True
        End If
        ControlVisibility()
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte

        GetAttachment()

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        IsAttachmentDeleted = True
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub imgbtnStandard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgbtnStandard.Click
        setObject()
        'Dim str As String
        'str = "<script language='javascript'>openledgersame('wfAuditStandard.aspx?ChildPage3=wfAudit.aspx" & "&BackPage=" & Request.QueryString("BackPage") & "&BackPage1=" & Request.QueryString("BackPage1") & "&ChildPage2=" & Request.QueryString("ChildPage2") & "&ChildPage=" & Request.QueryString("ChildPage") & "&BackPage2=" & Request.QueryString("BackPage2") & "');</script>"
        'ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenStandardWindow", "OpenStandardWindow()", True)
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        MarkLog(Flypal.Util.Action.Close, "Audit", "", Flypal.Util.ErrorType.NoError, Guid.Empty, EventLogID)
        setObject()
        If mAudit.IsDirty Then
            Session("IsValid") = "True"
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
        Else
            If Request.QueryString("ChildPage") <> "" Then
                Response.Redirect(Request.QueryString("ChildPage") & "?BackPage=" & Request.QueryString("BackPage"))
            Else
                mAudit = Session("mAudit")
                setObject()
                Session("mAudit") = mAudit
                Session.Remove("mAudit")
                Session.Remove("mFileAttach")
                Session.Remove("mFileAttachOnAudit") 'Ajay 21-11-2023
                ' Response.Redirect(Request.QueryString("BackPage"))
                Response.Redirect("index.aspx")
            End If
        End If
    End Sub
    'Private Sub txtFrequency_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFrequency.TextChanged
    '    If Trim(txtFrequency.Text) = "" Then txtFrequency.Text = 0
    'End Sub
    'Private Sub txtExePeriod_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtExePeriod.TextChanged
    '    If Trim(txtExePeriod.Text) = "" Then txtExePeriod.Text = 0
    'End Sub
    Private Sub dgAuditMasterTask_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgAuditMasterTask.RowCommand
        Select Case e.CommandName
            Case "RemoveRec"
                ' Dim Index As Int32 = e.CommandArgument.ToString + dgAuditMasterTask.PageIndex * dgAuditMasterTask.PageSize
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow) 'Ajay on 26-09-2023
                Dim Index As Int32 = gvr.RowIndex
                'If (Not User.IsInRole("AuditScheduleNew") And mAuditSchedule.IsNew) Or (Not User.IsInRole("AuditScheduleEdit") And Not mAuditSchedule.IsNew) Then
                '    SaveFormToObject()
                '    SetSession()
                '    Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
                '    msg.ReplacePage = "wfAuditSchedule.aspx?BackPage1=wfAuditSchedule.aspx" & "&BackPage=" & Request.QueryString("BackPage")
                '    Session("sender") = "Authorization"
                '    msg.Show()
                '    Exit Sub
                'End If
                DeleteAuditMasterTask(Index)
        End Select
    End Sub
    Private Sub hdnimgBtnTaskMaster_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnTaskMaster.Click
        dgAuditMasterTask.DataSource = mAudit.AuditMasterTasks
        dgAuditMasterTask.DataBind()
        upnlGrid.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnimgBtnAuditStandard_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnAuditStandard.Click
        mAuditStandardList = AuditStandardList.GetAuditStandardList("(SELECT)")
        Session("mAuditStandardList") = mAuditStandardList
        cmbStandard.DataSource = mAuditStandardList
        cmbStandard.DataBind()
        upnlStandard.Update()
    End Sub
#End Region

    
End Class