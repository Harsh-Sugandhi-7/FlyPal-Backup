Public Class wfConditionCheckItem_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Protected mConditionCheckItem As ConditionCheckItem
    Protected mConditionCheckItemChildList As ConditionCheckItemChildList
    Protected mConditionCheckItemChild As ConditionCheckItemChild
    Protected mItemList As ItemList
    Dim mGetSerialNos As GetEquipmentSerialNos
    Dim mFileAttach As FileAttach
    Dim mListOfItemServiceInspections As ListOfItemServiceInspections
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mConditionCheckItem = Session("mConditionCheckItem")
        mConditionCheckItemChild = Session("mConditionCheckItemChild")
        mConditionCheckItemChildList = Session("mConditionCheckItemChildList")
        mGetSerialNos = Session("mGetSerialNos")
        mFileAttach = Session("mFileAttach")
        mListOfItemServiceInspections = Session("mListOfItemServiceInspections")
    End Sub
    Private Sub SaveFormToObject()
        Try
            mConditionCheckItem.ItemID = New Guid(cmbItemList.SelectedValue)
            mConditionCheckItemChild.SerialNo = IIf(cmbSerialNo.SelectedIndex > 0, cmbSerialNo.SelectedItem.Text, "")
            mConditionCheckItemChild.ConditionCheckItemID = mConditionCheckItem.ID
            mConditionCheckItemChild.PreviousConditionCheckItemChildID = Guid.Empty
            mConditionCheckItemChild.ItemName = mConditionCheckItem.ItemName
            mConditionCheckItemChild.Description = mConditionCheckItem.Description
            mConditionCheckItemChild.Frequency = mConditionCheckItem.Frequency
            mConditionCheckItemChild.ConditionCheckNo = txtNo.Text
            mConditionCheckItemChild.DoneOnDate = txtDoneOnDate.Text
            mConditionCheckItemChild.IsApplicable = chkIsApplicable.Checked
            mConditionCheckItemChild.NextDueDate = txtNextDueDate.Text
            mConditionCheckItemChild.DonebyAgency = txtDoneByAgency.Text
            mConditionCheckItemChild.CertificateReference = txtCertRef.Text
            mConditionCheckItemChild.Remark = txtRemark.Text
            mConditionCheckItem.ReceiptItemID = New Guid(cmbSerialNo.SelectedValue)
            mConditionCheckItem.Location = txtLocation.Text
            mConditionCheckItem.ItemServiceInspectionsID = New Guid(cmbListOfItemServiceInspections.SelectedValue.ToString)
            Session("mConditionCheckItem") = mConditionCheckItem
            Session("mConditionCheckItemChild") = mConditionCheckItemChild
        Catch ex As Exception

        End Try
    End Sub
    Private Sub DataFieldBind()
        mConditionCheckItem = Session("mConditionCheckItem")
        mConditionCheckItemChild = Session("mConditionCheckItemChild")
        cmbItemList.DataSource = ItemList.GetItemsList(14, "", "", "", "", "", "", True, 1)
        cmbItemList.DataBind()
        Dim mGetSerialNos As GetEquipmentSerialNos
        mGetSerialNos = GetEquipmentSerialNos.GetEquipmentSerialNos(New Guid(cmbItemList.SelectedValue.ToString), True)
        cmbSerialNo.DataSource = mGetSerialNos
        cmbSerialNo.DataBind()

        mListOfItemServiceInspections = ListOfItemServiceInspections.GetServiceInspectionList(AddTopItem:="(SELECT)")
        cmbListOfItemServiceInspections.DataSource = mListOfItemServiceInspections
        cmbListOfItemServiceInspections.DataBind()

        Session("mGetSerialNos") = mGetSerialNos
        mConditionCheckItem.ItemID = New Guid(cmbItemList.SelectedValue.ToString)
        mConditionCheckItem.ReceiptItemID = New Guid(cmbSerialNo.SelectedValue.ToString)
        txtDoneOnDate.Text = mConditionCheckItemChild.DoneOnDateFormatted
        txtNextDueDate.Text = mConditionCheckItemChild.NextDueDate

        DataBind()
    End Sub
    Private Sub SetPage()
        If Not mConditionCheckItem.IsNew Then
            lblTitle.Text = Session("ConditionCheckItem") + "Equipment Maintenance Item  [" + CType(mConditionCheckItemChild.ItemName, String) + "]"
        Else
            lblTitle.Text = Session("ConditionCheckItem") + "Equipment Maintenance [New]"
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtDoneOnDate" Then
            If Len(txtDoneOnDate.Text.ToString) = 0 Then
                custValidator.ErrorMessage = " Please Select Done On Date."
                e.IsValid = False
                'Else
                '    e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "txtNo" Then
            If Len(txtNo.Text) > 50 Then
                custValidator.ErrorMessage = "Maximun Length of Condition Check No. should be 50."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtDoneByAgency" Then
            If Len(txtDoneByAgency.Text) > 150 Then
                custValidator.ErrorMessage = "Maximun Length of Note should be 150."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtCertRef" Then
            If Len(txtCertRef.Text) > 100 Then
                custValidator.ErrorMessage = "Maximun Length of Note should be 100."
                e.IsValid = False
            End If
        ElseIf custValidator.ControlToValidate = "txtRemark" Then
            If Len(txtRemark.Text) > 1000 Then
                custValidator.ErrorMessage = "Remark should not be greater than 1000 characters"
                e.IsValid = False
            End If
        End If
    End Sub
    Private Function CheckForDuplicate() As Boolean
        Dim mConditionCheckItemList As ConditionCheckItemList
        mConditionCheckItemList = ConditionCheckItemList.GetConditionCheckItemList("", "", "{00000000-0000-0000-0000-000000000000}", _
                                                                                    "{00000000-0000-0000-0000-000000000000}", "", "", "", 0, _
                                                                                    "{00000000-0000-0000-0000-000000000000}")
        If mConditionCheckItemList.Count > 0 Then
                If mConditionCheckItemList.Contains(mConditionCheckItem.ItemID, mConditionCheckItem.SerialNo, New Guid(cmbListOfItemServiceInspections.SelectedValue.ToString)) Then
                    Return False
                    Exit Function
                End If
        End If
        Return True
    End Function
    Private Sub Save()
        Try
            If txtRemark.Text.Length > 1000 Then
                ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", MessageBox.Show("Remark should not be greater than 1000 characters"))
                Exit Sub
            End If
            mConditionCheckItem = Session("mConditionCheckItem")
            mConditionCheckItemChild = Session("mConditionCheckItemChild")
            '''SaveFormToObject()
            If mConditionCheckItem.IsValid Then
                mConditionCheckItem.ApplyEdit()

                mConditionCheckItem = mConditionCheckItem.Save()
                If mConditionCheckItemChild.IsValid Then
                    mConditionCheckItemChild = mConditionCheckItemChild.Save()
                End If
                Dim mConditionCheckDetail As String = mConditionCheckItemChild.ConditionCheckNo + " Done On Date : " + mConditionCheckItemChild.DoneOnDate + " of " + "Part No. " + mConditionCheckItem.ItemName + " Serial No. " + mConditionCheckItem.SerialNo
                MarkLog(Util.Action.Save, "ConditionCheck", mConditionCheckDetail, Util.ErrorType.NoError, mConditionCheckItem.ID, EventLogID)

                Session("mConditionCheckItem") = mConditionCheckItem
                Session("mConditionCheckItemChild") = mConditionCheckItemChild
                Session("mConditionCheckItemChildList") = mConditionCheckItemChildList
                SetPage()
                DataBind()
            End If
        Catch ex As SqlException
            If ex.Number = 2627 Then
                MSGBoxCtrl.show("Alert!", "Save Alert ! ", "<strong> You are trying to save the duplicate entry. </strong> <p>" + "You can not add duplicate entry in Condition Check.", MsgBoxStyle.OkOnly, "")
                Session("ex.Number") = "ex.Number"
            End If
        End Try
        If Session("ex.Number") = "ex.Number" Then
            '
        Else
            Session.Remove("ex.Number")
            Dim mopenas As String = Request.QueryString("Type")
            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                Exit Sub
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
                        DataFieldBind()
                        If mConditionCheckItem.IsValid Then
                            If CheckForDuplicate() = False Then
                                MSGBoxCtrl.show("Alert!", "Save Alert ! ", "<strong> You are trying to save the duplicate entry. </strong> <p>" + "You can not add duplicate entry in Condition Check.", MsgBoxStyle.OkOnly, "")
                                'Session("Duplicate") = "Duplicate"
                                Exit Sub
                            End If
                            Save()
                            If Session("ex.Number") = "ex.Number" Then
                                Session.Remove("ex.Number")
                            Else
                                Session.Remove("ex.Number")
                                Dim mopenas As String = Request.QueryString("Type")
                                If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                                    Exit Sub
                                End If
                            End If
                        Else
                            If CustomValidate1() = False Then
                                upnlValidations.Update()
                                Exit Sub
                            End If
                        End If
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        If mConditionCheckItem.IsNew Then
                            lblTitle.Text = "Equipment Maintenance Item [New]"
                            Session.Remove("mConditionCheckItem")
                        End If
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
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        addAttributes()
        GetSession()
        If mConditionCheckItem Is Nothing Or mConditionCheckItemChild Is Nothing Then
            'Make new ConditionCheck
            mConditionCheckItem = ConditionCheckItem.NewConditionCheckItem()
            mConditionCheckItemChild = ConditionCheckItemChild.NewConditionCheckItemChild(mConditionCheckItem.ID, False)
            mConditionCheckItemChild.DoneOnDate = Today.Date
            Session("mConditionCheckItem") = mConditionCheckItem
            Session("mConditionCheckItemChild") = mConditionCheckItemChild
        End If

        If Not Page.IsPostBack Then
            setFocus(cmbItemList)
            DataFieldBind()
            ControlVisibilityForAttachment()
        End If
        If chkIsApplicable.Checked = False Then
            txtNextDueDate.Text = ""
        Else
            If txtDoneOnDate.Text <> "" Then
                mConditionCheckItemChild.DoneOnDate = txtDoneOnDate.Text
                If mConditionCheckItem.ConditionCheckIntervalIn = 1 Then 'Days
                    mConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddDays(mConditionCheckItem.Frequency)
                ElseIf mConditionCheckItem.ConditionCheckIntervalIn = 2 Then 'Month
                    mConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddMonths(mConditionCheckItem.Frequency)
                ElseIf mConditionCheckItem.ConditionCheckIntervalIn = 3 Then 'Year
                    mConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddYears(mConditionCheckItem.Frequency)
                End If
                txtNextDueDate.Text = mConditionCheckItemChild.NextDueDate
            End If
        End If
        SetPage()

    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If Not IsValid Then Exit Sub
            mConditionCheckItem = Session("mConditionCheckItem")
            mConditionCheckItemChild = Session("mConditionCheckItemChild")
            SaveFormToObject()

            If CheckForDuplicate() = False Then
                MSGBoxCtrl.show("Alert!", "Save Alert ! ", "<strong> You are trying to save the duplicate entry. </strong> <p>" + "You can not add duplicate entry in Condition Check.", MsgBoxStyle.OkOnly, "")
                'Session("Duplicate") = "Duplicate"
                Exit Sub
            End If


            If mConditionCheckItem.IsValid Then
                mConditionCheckItem.ApplyEdit()

                mConditionCheckItem = mConditionCheckItem.Save()
                If mConditionCheckItemChild.IsValid Then
                    mConditionCheckItemChild = mConditionCheckItemChild.Save()
                End If
                ControlVisibilityForAttachment()
                Dim mConditionCheckDetail As String = mConditionCheckItemChild.ConditionCheckNo + " Done On Date : " + mConditionCheckItemChild.DoneOnDateFormatted + " of " + "Part No. " + mConditionCheckItem.ItemName + " Serial No. " + mConditionCheckItem.SerialNo
                MarkLog(Util.Action.Save, "ConditionCheck", mConditionCheckDetail, Util.ErrorType.NoError, mConditionCheckItem.ID, EventLogID)

                Session("mConditionCheckItem") = mConditionCheckItem
                Session("mConditionCheckItemChild") = mConditionCheckItemChild
                Session("mConditionCheckItemChildList") = mConditionCheckItemChildList
                SetPage()
                ' DataFieldBind()
                DataBind()
            Else
                If CustomValidate1() = False Then
                    upnlValidations.Update()
                    Exit Sub
                End If
            End If
        Catch ex As SqlException
            If ex.Number = 2627 Then
                MSGBoxCtrl.show("Alert!", "Save Alert ! ", "<strong> You are trying to save the duplicate entry. </strong> <p>" + "You can not add duplicate entry in Condition Check.", MsgBoxStyle.OkOnly, "")
                Session("Duplicate") = "Duplicate"
            End If
        End Try
        If Session("Duplicate") = "Duplicate" Then
            Session.Remove("Duplicate")
        Else
            Session.Remove("Duplicate")
            Dim mopenas As String = Request.QueryString("Type")
            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                Exit Sub
            End If
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        mConditionCheckItem = Session("mConditionCheckItem")
        mConditionCheckItemChild = Session("mConditionCheckItemChild")
        SaveFormToObject()
        'Session("IsValid") = IsValid

        If mConditionCheckItemChild.IsDirty Then
            'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.CloseConfirm, SIMsgBox.Message_text.Save, "", MsgBoxStyle.YesNo)
            'msg1.ReplacePage = "wfConditionCheckItem.aspx?BackPage=" & Request.QueryString("BackPage")
            'Session("sender") = "Close"
            'msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.Save, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
            Exit Sub
        Else
            SaveFormToObject()
            Session("mConditionCheckItem") = mConditionCheckItem
            Session("mConditionCheckItemChild") = mConditionCheckItemChild
            Session.Remove("mConditionCheckItem")
            Session.Remove("mConditionCheckItemChild")
            Session.Remove("mOldConditionCheckItemChild")
            Session.Remove("mConditionCheckItemList")

            Dim mopenas As String = Request.QueryString("Type")
            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                Exit Sub
            End If
        End If
    End Sub
    Private Sub cmbItemList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbItemList.SelectedIndexChanged
        'Dim mGetSerialNos As GetEquipmentSerialNos

        mGetSerialNos = GetEquipmentSerialNos.GetEquipmentSerialNos(New Guid(cmbItemList.SelectedValue.ToString), True)
        cmbSerialNo.SelectedIndex = 0
        mConditionCheckItem.ReceiptItemID = Guid.Empty
        cmbSerialNo.DataSource = mGetSerialNos
        cmbSerialNo.DataBind()
        Session("mGetSerialNos") = mGetSerialNos


        mListOfItemServiceInspections = ListOfItemServiceInspections.GetServiceInspectionList(cmbItemList.SelectedValue.ToString, AddTopItem:="(SELECT)")
        cmbListOfItemServiceInspections.DataSource = mListOfItemServiceInspections
        cmbListOfItemServiceInspections.DataBind()
        Session("mListOfItemServiceInspections") = mListOfItemServiceInspections
        mConditionCheckItem.ItemID = New Guid(cmbItemList.SelectedValue.ToString)
        ' checkIsApplicable()
        DataBind()
        setFocus(cmbItemList)
        If chkIsApplicable.Checked = False Then
            txtNextDueDate.Text = ""
        Else
            If txtDoneOnDate.Text <> "" Then
                mConditionCheckItemChild.DoneOnDate = txtDoneOnDate.Text
                If mConditionCheckItem.ConditionCheckIntervalIn = 1 Then 'Days
                    mConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddDays(mConditionCheckItem.Frequency)
                ElseIf mConditionCheckItem.ConditionCheckIntervalIn = 2 Then 'Month
                    mConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddMonths(mConditionCheckItem.Frequency)
                ElseIf mConditionCheckItem.ConditionCheckIntervalIn = 3 Then 'Year
                    mConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddYears(mConditionCheckItem.Frequency)
                End If
                txtNextDueDate.Text = mConditionCheckItemChild.NextDueDate
            End If
        End If
    End Sub
    Private Sub txtDoneOnDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDoneOnDate.TextChanged
        mConditionCheckItem = Session("mConditionCheckItem")
        mConditionCheckItemChild = Session("mConditionCheckItemChild")
        If chkIsApplicable.Checked = False Then
            txtNextDueDate.Text = ""
        Else
            If txtDoneOnDate.Text <> "" Then
                mConditionCheckItemChild.DoneOnDate = txtDoneOnDate.Text
                If mConditionCheckItem.ConditionCheckIntervalIn = 1 Then 'Days
                    mConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddDays(Val(txtFrequency.Text))
                ElseIf mConditionCheckItem.ConditionCheckIntervalIn = 2 Then 'Month
                    mConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddMonths(Val(txtFrequency.Text))
                ElseIf mConditionCheckItem.ConditionCheckIntervalIn = 3 Then 'Year
                    mConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddYears(Val(txtFrequency.Text))
                End If
                txtNextDueDate.Text = mConditionCheckItemChild.NextDueDate
            End If
        End If
    End Sub
    'Private Sub txtDoneOnDate_CalendarVisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDoneOnDate.CalendarVisibleChanged
    '    chkIsApplicable.Visible = Not CType(sender, Boolean)
    'End Sub
    Private Sub cmbSerialNo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSerialNo.SelectedIndexChanged
        If cmbSerialNo.SelectedIndex > 0 Then
            mConditionCheckItem.Location = mGetSerialNos(cmbSerialNo.SelectedIndex, "").Location
            txtLocation.Text = mConditionCheckItem.Location
        Else
            mConditionCheckItem.Location = ""
            txtLocation.Text = mConditionCheckItem.Location
        End If
        mConditionCheckItem.ReceiptItemID = New Guid(cmbSerialNo.SelectedValue.ToString)
        mConditionCheckItem.SerialNo = IIf(cmbSerialNo.SelectedIndex > 0, cmbSerialNo.SelectedItem.Text, "")
        DataBind()
    End Sub
    Private Sub chkIsApplicable_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsApplicable.CheckedChanged
        If chkIsApplicable.Checked = False Then
            txtNextDueDate.Text = ""
        Else
            If txtDoneOnDate.Text <> "" Then
                mConditionCheckItemChild.DoneOnDate = txtDoneOnDate.Text
                If mConditionCheckItem.ConditionCheckIntervalIn = 1 Then 'Days
                    mConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddDays(Val(txtFrequency.Text))
                ElseIf mConditionCheckItem.ConditionCheckIntervalIn = 2 Then 'Month
                    mConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddMonths(Val(txtFrequency.Text))
                ElseIf mConditionCheckItem.ConditionCheckIntervalIn = 3 Then 'Year
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
                If mConditionCheckItem.ConditionCheckIntervalIn = 1 Then 'Days
                    mConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddDays(Val(txtFrequency.Text))
                ElseIf mConditionCheckItem.ConditionCheckIntervalIn = 2 Then 'Month
                    mConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddMonths(Val(txtFrequency.Text))
                ElseIf mConditionCheckItem.ConditionCheckIntervalIn = 3 Then 'Year
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

#Region " Show BrokenRules "
    Public Function CustomValidate1() As Boolean
        Dim strMsg As String = ""
        If mConditionCheckItem.IsValid = False Then
            For i As Integer = 0 To mConditionCheckItem.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mConditionCheckItem.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        If strMsg.Trim <> "" Then
            cvItem.ErrorMessage = strMsg
            cvItem.IsValid = False
            Return False
        End If
        Return True
    End Function
    Private Sub cmbListOfItemServiceInspections_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbListOfItemServiceInspections.SelectedIndexChanged
        mConditionCheckItem.ItemServiceInspectionsID = New Guid(cmbListOfItemServiceInspections.SelectedValue.ToString)
        mConditionCheckItem.Frequency = mListOfItemServiceInspections(New Guid(cmbListOfItemServiceInspections.SelectedValue.ToString)).Frequency
        mConditionCheckItem.ConditionCheckIntervalIn = mListOfItemServiceInspections(New Guid(cmbListOfItemServiceInspections.SelectedValue.ToString)).FrquencyPeriod
        DataBind()
        setFocus(cmbListOfItemServiceInspections)
        If chkIsApplicable.Checked = False Then
            txtNextDueDate.Text = ""
        Else
            If txtDoneOnDate.Text <> "" Then
                mConditionCheckItemChild.DoneOnDate = txtDoneOnDate.Text
                If mListOfItemServiceInspections(New Guid(cmbListOfItemServiceInspections.SelectedValue.ToString)).FrquencyPeriod = 1 Then 'Days
                    mConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddDays(mListOfItemServiceInspections(New Guid(cmbListOfItemServiceInspections.SelectedValue.ToString)).Frequency)
                ElseIf mConditionCheckItem.ConditionCheckIntervalIn = 2 Then 'Month
                    mConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddMonths(mListOfItemServiceInspections(New Guid(cmbListOfItemServiceInspections.SelectedValue.ToString)).Frequency)
                ElseIf mConditionCheckItem.ConditionCheckIntervalIn = 3 Then 'Year
                    mConditionCheckItemChild.NextDueDate = CDate(mConditionCheckItemChild.DoneOnDate).AddYears(mListOfItemServiceInspections(New Guid(cmbListOfItemServiceInspections.SelectedValue.ToString)).Frequency)
                End If
                txtNextDueDate.Text = mConditionCheckItemChild.NextDueDate
            End If
        End If
    End Sub
#End Region

End Class