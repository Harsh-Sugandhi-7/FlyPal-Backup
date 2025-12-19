Public Class wfManualRevision_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Protected mManual As Manual
    Dim mManualClone As Manual
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    Dim EventLogID As Guid

    Dim mFileAttachments As FileAttachments
    Dim mFileAttachment As FileAttach
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mManual = Session("mManual")
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        mFileAttachment = Session("mFileAttachment")
        mFileAttachments = Session("mFileAttachments")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        Session.Remove("mFileAttachment")
        Session.Remove("mFileAttachments")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "RemoveAttachment" Then
                        Try
                            Session("Sender") = ""
                            Dim mManual As Manual
                            mManual = CType(Session("mManual"), Manual)
                            'mManual.Revisions.CurrentItem.FileAttachments.Remove(mManual.Revisions.CurrentItem.FileAttachments.CurrentItem)
                            mFileAttachment = Session("mFileAttachment")
                            ' mFileAttachments.DeleteFileAttachment(mFileAttachment.ID, mManual.Revisions.CurrentItem.ID.ToString)
                            ' mFileAttachments = FileAttachments.GetFileAttachmentsByRefID(mManual.Revisions.CurrentItem.ID)
                            mFileAttachments.Remove(mFileAttachment)
                            dgManRevisionAttachment.DataSource = mFileAttachments 'mManual.Revisions.CurrentItem.FileAttachments
                            dgManRevisionAttachment.DataBind()
                            upnlManRevisionAttachment.Update()
                            Session("mnWO") = mManual
                        Catch ex As SqlException
                            If ex.Number = 8145 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 2627 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            ElseIf ex.Number = 547 Then
                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                            End If
                        End Try
                    End If
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "DuplicateRevision" Then
                        Try
                            calRevDate.Text = mManual.Revisions.CurrentItem.RevDate
                            calRevDate.DataBind()
                            upnlRevisionDetails.Update()
                            mManualClone = Nothing
                        Catch ex As SqlException
                            MSGBoxCtrl.show(MSGBox.Message_title.Alert, MSGBox.Message_text.Alert, ex.Message, MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        End Try
                    End If
                Case MsgBoxResult.No

            End Select
        End If
    End Sub
    Private Sub SetObject()
        mManual.Revisions.CurrentItem.No = Trim(txtNo.Text)
        mManual.Revisions.CurrentItem.RevNo = Trim(txtRevNo.Text)
        mManual.Revisions.CurrentItem.Frequency = txtFrequency.Text 'Added by Saylee on 10-nov-2009
        If calRevDate.Text = "" Then
            mManual.Revisions.CurrentItem.RevDate = ""
        Else
            mManual.Revisions.CurrentItem.RevDate = Format(CDate(calRevDate.Text), AppSettings("DateFormat"))
        End If
        If calNextRevisionDate.Text = "" Then
            mManual.Revisions.CurrentItem.EffectiveDate = ""
        Else
            mManual.Revisions.CurrentItem.EffectiveDate = Format(CDate(calNextRevisionDate.Text), AppSettings("DateFormat"))
        End If
        mManual.Revisions.CurrentItem.Note = txtNote.Text
        mManual.Revisions.CurrentItem.Remark = txtRemark.Text
        mManual.Revisions.CurrentItem.HardCopy = chkHardCopy.Checked 'Added by Saylee on 10-nov-2009
        mManual.Revisions.CurrentItem.SoftCopy = chkSoftCopy.Checked
        'If Not mFileAttach Is Nothing Then
        '    If mFileAttach.Size > 0 Then
        '        mManual.Revisions.CurrentItem.IsAttachmentAdded = True
        '    Else
        '        mManual.Revisions.CurrentItem.IsAttachmentAdded = False
        '    End If
        'End If
        ' For i As Integer = 0 To mManual.Revisions.CurrentItem.FileAttachments.Count - 1
        mFileAttachments = Session("mFileAttachments")
        For i As Integer = 0 To mFileAttachments.Count - 1
            Dim txtValue As TextBox
            txtValue = CType(Me.dgManRevisionAttachment.Rows(i).FindControl("txtFileName"), TextBox)
            ' mManual.Revisions.CurrentItem.FileAttachments(i).FileName = txtValue.Text.Trim
            mFileAttachments(i).FileName = txtValue.Text.Trim
        Next
        Session("mFileAttachments") = mFileAttachments
        mManual.Revisions.CurrentItem.IsAttachmentAdded = IIf(dgManRevisionAttachment.Rows.Count > 0, True, False) 'IIf(mManual.Revisions.CurrentItem.FileAttachments.Count > 0, True, False)
    End Sub
    Private Sub SaveAttachment() '
        If Not mFileAttachments Is Nothing Then

            Try
                mFileAttachments.UpdateAttachmentByRefID(mManual.Revisions.CurrentItem.ID)

            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
            End Try
        Else
            '    If (Not mManual.IsNew) And IsAttachmentDeleted Then
            '        FileAttach.DeleteAttachment(mFileAttach.ID, mManual.ID)
            '    End If
            '    IsAttachmentDeleted = False
            '    Session("IsAttachmentDeleted") = IsAttachmentDeleted
            'End If
        End If
    End Sub
    'Private Sub ControlVisibilityForAttachment()
    '    If mManual.Revisions.CurrentItem.IsAttachmentAdded Then
    '        ImageButton1.Visible = True
    '        btnDelAttach.Enabled = True
    '    Else
    '        ImageButton1.Visible = False
    '        btnDelAttach.Enabled = False
    '    End If
    'End Sub
    Private Sub DataFieldBind(Optional ByVal GetList As Boolean = True)
        calRevDate.Text = mManual.Revisions.CurrentItem.RevDate
        calNextRevisionDate.Text = mManual.Revisions.CurrentItem.EffectiveDate
        If mManual.Revisions.CurrentItem.IsNew Then
            If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
                lblTitle.Text = "Subscription [New]"
            Else
                lblTitle.Text = "Revision [New]"
            End If
        Else
            If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
                lblTitle.Text = "Subscription " & "[" & mManual.Revisions.CurrentItem.No & "]"
            Else
                lblTitle.Text = "Revision " & "[" & mManual.Revisions.CurrentItem.No & "]"
            End If
        End If

        mFileAttachments = FileAttachments.GetFileAttachmentsByRefID(mManual.Revisions.CurrentItem.ID)
        dgManRevisionAttachment.DataSource = mFileAttachments ' mManual.Revisions.CurrentItem.FileAttachments
        Session("mFileAttachments") = mFileAttachments
        DataBind()
    End Sub
    Private Sub addAttributes()
        txtFrequency.Attributes.Add("onKeyPress", "validateText(('N'),document.getElementById('txtFrequency').value,event)")
    End Sub
    Private Sub DeleteAttachment(ByVal Index As Int32)
        'MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem, MSGBox.Message_text.RemoveItem, "", MsgBoxStyle.YesNo, "RemoveAttachment")
        MSGBoxCtrl.show(MSGBox.Message_title.Delete, MSGBox.Message_text.Delete, "", MsgBoxStyle.YesNo, "RemoveAttachment")
        mFileAttachment = mFileAttachments(Index)
        Session("mFileAttachment") = mFileAttachment
        ' mManual.Revisions.CurrentItem.FileAttachments.CurrentIndex = Index
        ' Session("mManual") = mManual
    End Sub
    Private Sub AttachMyFile()
        Dim BackupPath As String = ""
        BackupPath = AppSettings("DOCPath") & "New.PDF"

        Try
            ''If Not mManual.Revisions.CurrentItem.FileAttachments.Contains(mManual.Revisions.CurrentItem.ID, CType(Session("FileUpload.FileName"), String)) Then

            ''    mManual.Revisions.CurrentItem.FileAttachments.Add(mManual.Revisions.CurrentItem.ID, CType(Session("FileUpload.FileName"), String))

            ''    mManual.Revisions.CurrentItem.FileAttachments.CurrentItem.ImageFile = CType(Session("ImageFile"), Byte())
            ''    mManual.Revisions.CurrentItem.FileAttachments.CurrentItem.Size = Session("Size")
            ''    mManual.Revisions.CurrentItem.FileAttachments.CurrentItem.Extension = Session("Extension")

            ''    Session("mManual") = mManual
            ''    mFileAttachments = FileAttachments.GetFileAttachmentsByRefID(mManual.Revisions.CurrentItem.ID)
            ''    dgManRevisionAttachment.DataSource = mFileAttachments  'mManual.Revisions.CurrentItem.FileAttachments
            ''    Session("mFileAttachments") = mFileAttachments
            ''    dgManRevisionAttachment.DataBind()

            ''    For i As Integer = 0 To mManual.Revisions.CurrentItem.FileAttachments.Count - 1
            ''        Dim txtValue As TextBox
            ''        txtValue = CType(Me.dgManRevisionAttachment.Rows(i).FindControl("txtFileName"), TextBox)
            ''        txtValue.Text = mManual.Revisions.CurrentItem.FileAttachments(i).FileName
            ''    Next

            ''    Session.Remove("Size")
            ''    Session.Remove("ImageFile")
            ''    Session.Remove("Extension")
            ''    Session.Remove("FileUpload.FileName")
            ''Else
            ''    Session("mManual") = mManual
            ''    MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
            ''    Exit Sub
            ''End If

            If Not mFileAttachments.Contains(mManual.Revisions.CurrentItem.ID, CType(Session("FileUpload.FileName"), String)) Then

                mFileAttachments.Add(mManual.Revisions.CurrentItem.ID, CType(Session("FileUpload.FileName"), String))

                mFileAttachments.CurrentItem.ImageFile = CType(Session("ImageFile"), Byte())
                mFileAttachments.CurrentItem.Size = Session("Size")
                mFileAttachments.CurrentItem.Extension = Session("Extension")

                Session("mFileAttachments") = mFileAttachments

                dgManRevisionAttachment.DataSource = mFileAttachments  'mManual.Revisions.CurrentItem.FileAttachments
                Session("mFileAttachments") = mFileAttachments
                dgManRevisionAttachment.DataBind()

                For i As Integer = 0 To mFileAttachments.Count - 1
                    Dim txtValue As TextBox
                    txtValue = CType(Me.dgManRevisionAttachment.Rows(i).FindControl("txtFileName"), TextBox)
                    txtValue.Text = mFileAttachments(i).FileName
                Next

                Session.Remove("Size")
                Session.Remove("ImageFile")
                Session.Remove("Extension")
                Session.Remove("FileUpload.FileName")
            Else
                Session("mManual") = mManual
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate, MSGBox.Message_text.Duplicate, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
        Catch ex As Exception
        End Try
    End Sub
#End Region

#Region " Events "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        addAttributes()
        EventLogID = CType(Session("EventLogID"), Guid)

        If Not Page.IsPostBack Then
            setFocus(txtNo)
            DataFieldBind()
            'ControlVisibilityForAttachment()
        End If
    End Sub
    Private Sub calRevDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calRevDate.TextChanged
        If calRevDate.Text <> "" Then
            mManualClone = mManual.Clone
            SetObject()
            If mManual.Revisions.Contains(mManual.Revisions.CurrentItem) Then
                MSGBoxCtrl.show("Duplicate Alert!", "You are trying to save the Revision entry.", "You can not add duplicate entry in Revision.", MsgBoxStyle.OkOnly, "DuplicateRevision")
                mManual = mManualClone
                Session("mManual") = mManual
                Exit Sub
            End If
            mManual.Revisions.CurrentItem.Frequency = txtFrequency.Text
            mManual.Revisions.CurrentItem.RevDate = Format(CDate(calRevDate.Text), AppSettings("DateFormat"))
            If txtFrequency.Text <> "0" Then
                mManual.Revisions.CurrentItem.EffectiveDate = Format(DateAdd(DateInterval.Month, mManual.Revisions.CurrentItem.Frequency, CDate(mManual.Revisions.CurrentItem.RevDate)), AppSettings("DateFormat"))
                calNextRevisionDate.Text = mManual.Revisions.CurrentItem.EffectiveDate
                calNextRevisionDate.DataBind()
            End If
        End If
    End Sub
    Private Sub txtFrequency_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFrequency.TextChanged
        If txtFrequency.Text = "" Or txtFrequency.Text = "0" Then
            txtFrequency.Text = 0
            calNextRevisionDate.Text = ""
        Else
            If calRevDate.Text <> "" Then
                mManual.Revisions.CurrentItem.Frequency = txtFrequency.Text
                mManual.Revisions.CurrentItem.RevDate = Format(CDate(calRevDate.Text), AppSettings("DateFormat"))
                mManual.Revisions.CurrentItem.EffectiveDate = Format(DateAdd(DateInterval.Month, mManual.Revisions.CurrentItem.Frequency, CDate(mManual.Revisions.CurrentItem.RevDate)), AppSettings("DateFormat"))
                calNextRevisionDate.Text = mManual.Revisions.CurrentItem.EffectiveDate
                calNextRevisionDate.DataBind()
            End If
        End If
    End Sub
    Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        mManualClone = mManual.Clone
        If IsValid Then
            SetObject()
            If mManual.Revisions.Contains(mManual.Revisions.CurrentItem) Then
                If calRevDate.Text = "" Then
                Else
                    MSGBoxCtrl.show("Duplicate Alert!", "You are trying to save the Revision entry.", "You can not add duplicate entry in Revision.", MsgBoxStyle.OkOnly, "DuplicateRevision")
                    mManual = mManualClone
                    Session("mManual") = mManual
                    Exit Sub
                End If
            End If
            Try
                'If mManual.Revisions.CurrentItem.IsDirty Then
                If mManual.Revisions.CurrentItem.IsSavable Or mFileAttachments.IsDirty Then
                    mManual.ApplyEdit()
                    SaveAttachment()
                    Session("mManual") = mManual
                    'ControlVisibilityForAttachment()
                    RemoveSession()
                    Dim mopenas As String = Request.QueryString("Type")
                    If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                        Exit Sub
                    End If
                Else
                    Dim strMsg As String = ""
                    For i As Integer = 0 To mManual.Revisions.CurrentItem.GetBrokenRulesCollection.Count - 1
                        strMsg = strMsg + mManual.Revisions.CurrentItem.GetBrokenRulesCollection(i).Description + "<Br>"
                    Next
                    cvControlValidator.ErrorMessage = strMsg
                    cvControlValidator.IsValid = mManual.Revisions.CurrentItem.IsValid
                    mManual = mManualClone
                    Session("mManual") = mManual
                    calRevDate.Text = mManual.Revisions.CurrentItem.RevDate
                    calRevDate.DataBind()
                    calNextRevisionDate.Text = mManual.Revisions.CurrentItem.EffectiveDate
                    calNextRevisionDate.DataBind()
                    SetObject()
                    upnlRevisionDetails.Update()
                    mManualClone = Nothing
                    upnlValidationSummary.Update()

                    Dim mopenas As String = Request.QueryString("Type")
                    If Not mopenas Is Nothing AndAlso mopenas = "pup" And strMsg="" Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                        Exit Sub
                    End If

                End If
                'Else
                'mManual.ApplyEdit()
                'Session("mManual") = mManual
                'Dim mopenas As String = Request.QueryString("Type")
                'If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                '    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                '    Exit Sub
                'End If
                'End If
            Catch ex As Exception
                MSGBoxCtrl.show("Duplicate Alert!", "You are trying to save the Revision entry.", "You can not add duplicate entry in Revision.", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End Try
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        AttachMyFile()
        'ControlVisibilityForAttachment()
        upnlManRevisionAttachment.Update()
    End Sub
    'Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
    '    If mManual.Revisions.CurrentItem.IsAttachmentAdded Then
    '        mFileAttach = FileAttach.GetAttachment(mManual.Revisions.CurrentItem.ID)
    '    Else
    '        mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mManual.Revisions.CurrentItem.ID)
    '    End If
    '    Session("mFileAttach") = mFileAttach
    'End Sub
    'Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
    '    '----------------------------------------------------------------------
    '    Dim No As New Random
    '    Dim StrName As String = "abc" & No.Next.ToString
    '    '----------------------------------------------------------------------
    '    If mManual.Revisions.CurrentItem.IsAttachmentAdded And mFileAttach Is Nothing Then
    '        mFileAttach = FileAttach.GetAttachment(mManual.Revisions.CurrentItem.ID)
    '        Session("mFileAttach") = mFileAttach
    '    End If

    '    If mFileAttach.Size > 0 Then
    '        Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttach.Extension
    '        Dim fs As FileStream
    '        If File.Exists(AppSettings("DOCPath")) = False Then
    '            'Delete File if exist
    '            System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
    '            ' Create the file.
    '            fs = File.Create(path)
    '            '' Add some information to the file.
    '            fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
    '            fs.Close()
    '            Session("DOCPath") = path
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
    '        End If
    '    End If
    'End Sub
    'Private Sub btnDelAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
    '    Dim fileSize1 As Integer = 0
    '    Dim file1(fileSize1) As Byte

    '    If mManual.Revisions.CurrentItem.IsAttachmentAdded And mFileAttach Is Nothing Then
    '        mFileAttach = FileAttach.GetAttachment(mManual.Revisions.CurrentItem.ID)
    '        Session("mFileAttach") = mFileAttach
    '    End If

    '    mFileAttach.ImageFile = file1
    '    mFileAttach.Size = 0

    '    ImageButton1.Visible = False
    '    btnDelAttach.Enabled = False

    '    IsAttachmentDeleted = True
    '    mManual.Revisions.CurrentItem.IsAttachmentAdded = False
    '    Session("IsAttachmentDeleted") = IsAttachmentDeleted
    'End Sub
    Private Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If Session("EditRevisions") = False Then Session.Remove("EditRevisions") : mManual.Revisions.Remove(mManual.Revisions.CurrentItem)
        Session("EditRevisions") = ""
        mManual.CancelEdit()
        RemoveSession()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub dgManRevisionAttachment_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgManRevisionAttachment.RowCommand

        Select Case e.CommandName
            Case "View"
                Dim Index As Integer = CInt(e.CommandArgument) '+ dgWOAttachment.PageSize * dgWOAttachment.PageIndex

                Dim No As New Random
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttachments = FileAttachments.GetFileAttachmentsByRefID(mManual.Revisions.CurrentItem.ID) ' mManual.Revisions.CurrentItem.FileAttachments
                'mFileAttachments.CurrentIndex = Index - 1
                Session("mFileAttachments") = mFileAttachments
                If mFileAttachments.Count = 1 Then
                    mFileAttachments.CurrentIndex = 0
                Else
                    mFileAttachments.CurrentIndex = Index - 1
                End If

                If mFileAttachments.CurrentItem.Size > 0 Then
                    Dim path As String = AppSettings("DOCPath") & StrName & mFileAttachments.CurrentItem.Extension
                    Dim fs As FileStream
                    If File.Exists(AppSettings("DOCPath")) = False Then
                        'Delete File if exist
                        System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttachments.CurrentItem.Extension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mFileAttachments.CurrentItem.ImageFile, 0, mFileAttachments.CurrentItem.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFilel();", True)
                        Dim Detail As String = "Manual Revision Attachment( " + mFileAttachments.CurrentItem.FileName + ") viewed by  " + User.Identity.Name
                        MarkLog(Util.Action.View, "ManualRevision", Detail, Util.ErrorType.HandledError, mManual.Revisions.CurrentItem.ID, EventLogID)
                    End If
                End If
                dgManRevisionAttachment.DataSource = mFileAttachments ' mManual.Revisions.CurrentItem.FileAttachments
                DataBind()
            Case "Remove"
                Dim Index As Integer = CInt(e.CommandArgument)
                ' DeleteAttachment(Index)
                mFileAttachments = Session("mFileAttachments")
                If mFileAttachments Is Nothing Then
                    mFileAttachments = FileAttachments.GetFileAttachmentsByRefID(mManual.Revisions.CurrentItem.ID) 'mManual.Revisions.CurrentItem.FileAttachments
                End If

                If mFileAttachments.Count = 1 Then
                    DeleteAttachment(0)
                Else
                    DeleteAttachment(Index - 1)
                End If
        End Select

    End Sub
    Private Sub btnSelectFiles_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSelectFiles.Click
        SetObject()
        Session("mManual") = mManual
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow();", True)
    End Sub
#End Region

   
End Class