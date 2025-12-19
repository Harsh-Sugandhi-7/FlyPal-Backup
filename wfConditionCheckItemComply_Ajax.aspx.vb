Public Class wfConditionCheckItemComply_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Protected mConditionCheckItemChildList As ConditionCheckItemChildList
    Protected mConditionCheckItemChild As ConditionCheckItemChild
    Dim mFileAttach As FileAttach
    Public OpenFrom As String
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mConditionCheckItemChild = Session("mConditionCheckItemChild")
        mConditionCheckItemChildList = Session("mConditionCheckItemChildList")
        mFileAttach = Session("mFileAttach")
        OpenFrom = Session("OpenFrom")
    End Sub
    Private Sub SaveFormToObject()
        Try
            mConditionCheckItemChild.ConditionCheckNo = txtNo.Text
            mConditionCheckItemChild.DoneOnDate = txtDoneOnDate.Text
            mConditionCheckItemChild.IsApplicable = chkIsApplicable.Checked

            'If mConditionCheckItemChild.IsApplicable = False Then
            '    txtNextDueDate.Text = ""
            'Else
            '    If txtDoneOnDate.Text <> "" Then
            '        mConditionCheckItemChild.DoneOnDate = txtDoneOnDate.Text
            '        mConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddMonths(mConditionCheckItem.Frequency)
            '        txtNextDueDate.Text = mConditionCheckItemChild.NextDueDate
            '    End If
            'End If

            mConditionCheckItemChild.DonebyAgency = txtDoneByAgency.Text
            mConditionCheckItemChild.CertificateReference = txtCertRef.Text
            mConditionCheckItemChild.Remark = txtRemark.Text
        Catch ex As Exception

        End Try
    End Sub
    Private Sub DataFieldBind()
        txtDoneOnDate.Text = mConditionCheckItemChild.DoneOnDateFormatted
        txtNextDueDate.Text = mConditionCheckItemChild.NextDueDate
        DataBind()
    End Sub
    Private Sub SetPage()
        If mConditionCheckItemChild.IsNew Then
            lblTitle.Text = Session("ConditionCheckItem") + "Equipment Maintenance Item  [" + CType(mConditionCheckItemChild.ItemName, String) + "]"
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)

        If custValidator.ControlToValidate = "txtDoneOnDate" Then



            If Len(txtDoneOnDate.Text.ToString) = 0 Then
                custValidator.ErrorMessage = " Please Select Done On Date."
                e.IsValid = False
            Else
                'Added By Utkarsh On 24-May-2011

                If Not mConditionCheckItemChild.PreviousConditionCheckItemChildID.Equals(Guid.Empty) Then
                    Dim moldConditionCheckItemChild As ConditionCheckItemChild
                    moldConditionCheckItemChild = ConditionCheckItemChild.GetConditionCheckItemChild(mConditionCheckItemChild.PreviousConditionCheckItemChildID)
                    If Not mConditionCheckItemChild.DoneOnDate > CDate(moldConditionCheckItemChild.DoneOnDate) Then
                        custValidator.ErrorMessage = "Done On Date should be greater than Last Done On Date.(" + moldConditionCheckItemChild.DoneOnDateFormatted.ToString + ")"
                        e.IsValid = False
                    ElseIf mConditionCheckItemChild.DoneOnDate > Today.Date Then
                        custValidator.ErrorMessage = "Done On Date should not be greater than today's date"
                        e.IsValid = False
                    Else
                        e.IsValid = True
                    End If
                Else
                    '***************************************
                    e.IsValid = True
                End If
            End If

        ElseIf custValidator.ControlToValidate = "txtNo" Then
            If Len(txtNo.Text) > 50 Then
                custValidator.ErrorMessage = "Maximun Length of Condition Check No. should be 50."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "txtDoneByAgency" Then
            If Len(txtDoneByAgency.Text) > 150 Then
                custValidator.ErrorMessage = "Maximun Length of Note should be 150."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "txtCertRef" Then
            If Len(txtCertRef.Text) > 100 Then
                custValidator.ErrorMessage = "Maximun Length of Note should be 100."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "txtRemark" Then
            If Len(txtRemark.Text) > 1000 Then
                custValidator.ErrorMessage = "Remark should not be greater than 1000 characters"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Close" Then
                        'If txtRemark.Text.Length > 1000 Then
                        '    ScriptManager.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("Remark should not be greater than 1000 characters"))
                        '    Exit Sub
                        'End If
                        Save()
                        mConditionCheckItemChild.ApplyEdit()
                        Dim mopenas As String = Request.QueryString("Type")
                        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                            Exit Sub
                        End If
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        'If mConditionCheckItem.IsNew Then
                        '    lblTitle.Text = "Condition Check Item [New]"
                        '    Session.Remove("mConditionCheckItem")
                        'End If
                        Dim mopenas As String = Request.QueryString("Type")
                        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                            Exit Sub
                        End If
                    End If
                Case MsgBoxResult.Ok
            End Select
        End If
    End Sub
    Private Sub CheckIsapplicable()
        If chkIsApplicable.Checked = False Then
            txtNextDueDate.Text = ""
        Else
            If txtDoneOnDate.Text <> "" Then
                mConditionCheckItemChild.DoneOnDate = txtDoneOnDate.Text
                mConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddMonths(mConditionCheckItemChild.Frequency)
                txtNextDueDate.Text = mConditionCheckItemChild.NextDueDate
            End If
        End If
    End Sub
    Private Sub Save()
        mConditionCheckItemChild = Session("mConditionCheckItemChild")
        'SaveFormToObject()
        Try
            If mConditionCheckItemChild.IsValid Then
                mConditionCheckItemChild.ApplyEdit()
                mConditionCheckItemChild = mConditionCheckItemChild.Save()
                Session("mConditionCheckItemChild") = mConditionCheckItemChild
                Session("mConditionCheckItemChildList") = mConditionCheckItemChildList
                SetPage()
                DataFieldBind()

            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub ControlVisibilityForAttachment()
        If mConditionCheckItemChild.IsAttachmentAdded = True Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
        upnlFileupload.Update()
    End Sub
    Private Sub GetAttachment()
        If mConditionCheckItemChild.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mConditionCheckItemChild.ID)
        End If
    End Sub
    Private Sub ViewImage()
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        If mConditionCheckItemChild.IsAttachmentAdded = True Then
            Dim path As String = AppSettings("DOCPath") & "\" & StrName & mConditionCheckItemChild.FileAttachments(0).Extension
            Dim fs As FileStream
            If File.Exists(AppSettings("DOCPath")) = False Then
                'Delete File if exist
                System.IO.File.Delete(AppSettings("DOCPath") & StrName & mConditionCheckItemChild.FileAttachments(0).Extension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mConditionCheckItemChild.FileAttachments(0).ImageFile, 0, mConditionCheckItemChild.FileAttachments(0).ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                Dim Str As String
                Str = "openFile();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", Str, True)
            End If
        End If
    End Sub
    Private Sub addAttributes()
        txtFrequency.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtFrequency').value,event)")
    End Sub
    Private Sub ControlVisibility()
        If OpenFrom = "ServiceInspectionList" Then
            lblItemServiceInspections.Visible = True
            lblListOfItemServiceInspections.Visible = True
            lblListOfItemServiceInspections.Visible = True
        Else
            lblItemServiceInspections.Visible = False
            lblListOfItemServiceInspections.Visible = False
            lblListOfItemServiceInspections.Visible = False
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        addAttributes()
        GetSession()
        If Not Page.IsPostBack Then
            setFocus(txtNo)
            OpenFrom = Request.QueryString("OpenFrom")
            Session("OpenFrom") = OpenFrom
            ControlVisibility()
            DataFieldBind()
            ControlVisibilityForAttachment()
        End If
        If chkIsApplicable.Checked = False Then
            txtNextDueDate.Text = ""
        Else
            If txtDoneOnDate.Text <> "" Then
                mConditionCheckItemChild.DoneOnDate = txtDoneOnDate.Text
                'ConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddMonths(mConditionCheckItemChild.Frequency)
                If mConditionCheckItemChild.ConditionCheckIntervalIn = 1 Then 'Days
                    mConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddDays(mConditionCheckItemChild.Frequency)
                ElseIf mConditionCheckItemChild.ConditionCheckIntervalIn = 2 Then 'Month
                    mConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddMonths(mConditionCheckItemChild.Frequency)
                ElseIf mConditionCheckItemChild.ConditionCheckIntervalIn = 3 Then 'Year
                    mConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddYears(mConditionCheckItemChild.Frequency)
                End If
                txtNextDueDate.Text = mConditionCheckItemChild.NextDueDate
            End If
        End If
        SetPage()
        ''Attach File
        'If Not IsPostBack And Session("sender") = "" Then
        '    MyFile.Visible = True
        'End If
        'If MyFile.Value <> "" Then
        '    Dim BackupPath As String = ""
        '    BackupPath = AppSettings("DOCPath") & "New.PDF"

        '    Try
        '        MyFile.PostedFile.SaveAs(BackupPath)
        '        Dim fs As New FileStream(BackupPath, FileMode.OpenOrCreate, FileAccess.ReadWrite)
        '        Dim fileSize As Integer = CType(fs.Length, Integer)

        '        Dim fileBytes(fileSize) As Byte
        '        fs.Read(fileBytes, 0, fileSize)
        '        mConditionCheckItemChild.AttachFileName = fileBytes
        '        mConditionCheckItemChild.Size = fileSize
        '        mConditionCheckItemChild.FileExtension = MyFile.Value
        '        btnDelAttach.Enabled = True
        '        fs.Close()
        '        System.IO.File.Delete(BackupPath)

        '    Catch ex As Exception
        '    End Try
        'End If
        'If mConditionCheckItemChild.Size > 0 Then
        '    ImageButton1.Visible = True
        '    btnDelAttach.Enabled = True
        'Else
        '    ImageButton1.Visible = False
        'End If
        'MessageBoxResult()
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If Not IsValid Then Exit Sub 'Added By Utkarsh On 24-May-2011 
        mConditionCheckItemChild = Session("mConditionCheckItemChild")
        SaveFormToObject()
        Try
            If mConditionCheckItemChild.IsValid Then
                mConditionCheckItemChild.ApplyEdit()
                mConditionCheckItemChild = mConditionCheckItemChild.Save()
                Session("mConditionCheckItemChild") = mConditionCheckItemChild
                Dim mCalibrationDetail As String = mConditionCheckItemChild.ConditionCheckNo + " Done On Date : " + mConditionCheckItemChild.DoneOnDateFormatted + " of " + "Part No. " + mConditionCheckItemChild.ItemName + " Serial No. " + mConditionCheckItemChild.SerialNo
                MarkLog(Util.Action.Save, "ConditionCheck", mCalibrationDetail, Util.ErrorType.NoError, mConditionCheckItemChild.ID, EventLogID)
                Session("mConditionCheckItemChildList") = mConditionCheckItemChildList
                SetPage()
                DataFieldBind()

            End If
        Catch ex As Exception
            'lblTitle.Text = "ConditionCheckItem [New]"
            'Dim msg1 As New SIMsgBox(Page, "Duplicate Alert!<Br><Br><Br>You are trying to save the duplicate entry.", "<Br>You can not add duplicate entry in Calibration.", "", MsgBoxStyle.OKOnly)
            'msg1.ReplacePage = "wfComplyConditionCheckItem.aspx?BackPage=" & Request.QueryString("BackPage")
            'msg1.Show()
        End Try
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'Response.Redirect(BackPage.Pop(Session("BackPage")))
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        mConditionCheckItemChild = Session("mConditionCheckItemChild")
        SaveFormToObject()
        If mConditionCheckItemChild.IsDirty Then
            MSGBoxCtrl.show(MSGBox.Message_title.Save, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
            Exit Sub
        Else
            SaveFormToObject()
            Session("mConditionCheckItemChild") = mConditionCheckItemChild
            Session.Remove("mConditionCheckItem")
            Session.Remove("mConditionCheckItemChild")
            Session.Remove("mOldConditionCheckItemChild")
            'Response.Redirect(BackPage.Pop(Session("BackPage")))
            Dim mopenas As String = Request.QueryString("Type")
            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                Exit Sub
            End If
        End If
    End Sub
    'Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
    '    mConditionCheckItemChild = Session("mConditionCheckItemChild")
    '    SaveFormToObject()
    '    If mConditionCheckItemChild.IsDirty Then
    '        ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("Do you want to save this record.", MessageBox.MessageBoxButton.YesNo, mConditionCheckItemChild))
    '    Else
    '        SaveFormToObject()
    '        Session("mConditionCheckItemChild") = mConditionCheckItemChild
    '        Session.Remove("mConditionCheckItem")
    '        Session.Remove("mConditionCheckItemChild")
    '        Session.Remove("mOldConditionCheckItemChild")
    '        Response.Redirect(BackPage.Pop(Session("BackPage")))
    '    End If
    'End Sub
    Private Sub txtDoneOnDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDoneOnDate.TextChanged
        If chkIsApplicable.Checked = False Then
            txtNextDueDate.Text = ""
        Else
            If txtDoneOnDate.Text <> "" Then
                mConditionCheckItemChild.DoneOnDate = txtDoneOnDate.Text
                'mConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddMonths(mConditionCheckItemChild.Frequency)
                If mConditionCheckItemChild.ConditionCheckIntervalIn = 1 Then 'Days
                    mConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddDays(Val(txtFrequency.Text))
                ElseIf mConditionCheckItemChild.ConditionCheckIntervalIn = 2 Then 'Month
                    mConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddMonths(Val(txtFrequency.Text))
                ElseIf mConditionCheckItemChild.ConditionCheckIntervalIn = 3 Then 'Year
                    mConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddYears(Val(txtFrequency.Text))
                End If
                txtNextDueDate.Text = mConditionCheckItemChild.NextDueDate
            End If
        End If
    End Sub
    'Private Sub txtDoneOnDate_CalendarVisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDoneOnDate.CalendarVisibleChanged
    '    chkIsApplicable.Visible = Not CType(sender, Boolean)
    'End Sub
    Private Sub chkIsApplicable_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsApplicable.CheckedChanged
        mConditionCheckItemChild.IsApplicable = chkIsApplicable.Checked
        If chkIsApplicable.Checked = False Then
            txtNextDueDate.Text = ""
        Else
            If txtDoneOnDate.Text <> "" Then
                mConditionCheckItemChild.DoneOnDate = txtDoneOnDate.Text
                If mConditionCheckItemChild.ConditionCheckIntervalIn = 1 Then 'Days
                    mConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddDays(Val(txtFrequency.Text))
                ElseIf mConditionCheckItemChild.ConditionCheckIntervalIn = 2 Then 'Month
                    mConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddMonths(Val(txtFrequency.Text))
                ElseIf mConditionCheckItemChild.ConditionCheckIntervalIn = 3 Then 'Year
                    mConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddYears(Val(txtFrequency.Text))
                End If
                txtNextDueDate.Text = mConditionCheckItemChild.NextDueDate
            End If
        End If
    End Sub
    Private Sub txtFrequency_TextChanged(sender As Object, e As System.EventArgs) Handles txtFrequency.TextChanged
        If chkIsApplicable.Checked = False Then
            txtNextDueDate.Text = ""
        Else
            If txtDoneOnDate.Text <> "" And txtFrequency.Text <> "" Then
                mConditionCheckItemChild.DoneOnDate = txtDoneOnDate.Text
                If mConditionCheckItemChild.ConditionCheckIntervalIn = 1 Then 'Days
                    mConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddDays(Val(txtFrequency.Text))
                ElseIf mConditionCheckItemChild.ConditionCheckIntervalIn = 2 Then 'Month
                    mConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddMonths(Val(txtFrequency.Text))
                ElseIf mConditionCheckItemChild.ConditionCheckIntervalIn = 3 Then 'Year
                    mConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddYears(Val(txtFrequency.Text))
                End If
                txtNextDueDate.Text = mConditionCheckItemChild.NextDueDate
            End If
        End If
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        If mConditionCheckItemChild.IsAttachmentAdded Then
            mConditionCheckItemChild.FileAttachments(0).Size = mFileAttach.Size
            mConditionCheckItemChild.FileAttachments(0).ImageFile = mFileAttach.ImageFile
            mConditionCheckItemChild.FileAttachments(0).Extension = mFileAttach.Extension
        Else
            mConditionCheckItemChild.IsAttachmentAdded = True
            mConditionCheckItemChild.FileAttachments.Add(mFileAttach.ReferenceID, mFileAttach.ImageFile, mFileAttach.Size, mFileAttach.Extension, mFileAttach.Sort)
        End If
        ControlVisibilityForAttachment()
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        ViewImage()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mConditionCheckItemChild.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachmentChild(mConditionCheckItemChild.ID)
        Else
            mFileAttach = FileAttach.NewAttachmentChild(Guid.Empty, mConditionCheckItemChild.ID)
        End If
        Session("mFileAttach") = mFileAttach
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenFileUploadWindow", "OpenFileUploadWindow()", True)
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte
        GetAttachment()

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        mConditionCheckItemChild.IsAttachmentAdded = False
        mConditionCheckItemChild.FileAttachments.Remove(mConditionCheckItemChild.ID)
        Session("mConditionCheckItemChild") = mConditionCheckItemChild
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

End Class