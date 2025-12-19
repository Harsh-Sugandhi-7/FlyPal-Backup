Public Class wfCalibrationItem_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Protected mCalibrationItem As CalibrationItem
    Protected mCalibrationItemChild As CalibrationItemChild
    Protected mItemList As ItemList
    Dim EventLogID As Guid
    Dim mGetSerialNos As GetEquipmentSerialNos
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mCalibrationItem = Session("mCalibrationItem")
        mCalibrationItemChild = Session("mCalibrationItemChild")
        mGetSerialNos = Session("mGetSerialNos")
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        Session.Remove("mCalibrationItem")
        Session.Remove("mCalibrationItemChild")
        Session.Remove("mOldCalibrationItemChild")
        Session.Remove("mGetSerialNos")
    End Sub
    Private Sub SaveFormToObject()
        Try
            mCalibrationItem.ItemID = New Guid(cmbItemList.SelectedValue)
            mCalibrationItemChild.SerialNo = IIf(cmbSerialNo.SelectedIndex > 0, cmbSerialNo.SelectedItem.Text, "")
            mCalibrationItemChild.CalibrationItemID = mCalibrationItem.ID
            mCalibrationItemChild.PreviousCalibrationItemChildID = Guid.Empty
            mCalibrationItemChild.ItemName = mCalibrationItem.ItemName
            mCalibrationItemChild.Description = mCalibrationItem.Description
            mCalibrationItemChild.Frequency = mCalibrationItem.Frequency
            '---------------------Added By Prashant on 2-Sep-2021
            mCalibrationItemChild.CalibrationItemChildFrequency = mCalibrationItem.Frequency
            mCalibrationItemChild.CalibrationItemChildCalibrationPeriodInID = mCalibrationItem.CalibrationPeriodInID
            '---------------------End of Added By Prashant on 2-Sep-2021
            mCalibrationItemChild.CalibrationNo = txtNo.Text
            mCalibrationItemChild.DoneOnDate = txtDoneOnDate.Text
            mCalibrationItemChild.IsApplicable = chkIsApplicable.Checked
            mCalibrationItemChild.NextDueDate = txtNextDueDate.Text
            mCalibrationItemChild.DonebyAgency = txtDoneByAgency.Text
            mCalibrationItemChild.CertificateReference = txtCertRef.Text
            mCalibrationItemChild.Remark = txtRemark.Text
            mCalibrationItem.ReceiptItemID = New Guid(cmbSerialNo.SelectedValue) 'Guid.Empty 'New Guid(cmbSerialNo.SelectedValue)
            mCalibrationItem.Location = txtLocation.Text
            If Not mFileAttach Is Nothing Then
                If mFileAttach.Size > 0 Then
                    mCalibrationItemChild.IsAttachmentAdded = True
                Else
                    mCalibrationItemChild.IsAttachmentAdded = False
                End If
            End If
            Session("mCalibrationItem") = mCalibrationItem
            Session("mCalibrationItemChild") = mCalibrationItemChild
        Catch ex As Exception

        End Try
    End Sub
    Private Sub GetAttachment()
        If mCalibrationItemChild.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mCalibrationItemChild.ID)
            Session("mFileAttach") = mFileAttach
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
                If (Not mCalibrationItemChild.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mCalibrationItemChild.ID)
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
    Private Sub DataFieldBind()
        'Commented and added by Saylee on 5-Jan-2010
        ''cmbItemList.DataSource = ItemList.GetItemsList(10, "", "", "", "", "", "", True, , True)
        cmbItemList.DataSource = ItemList.GetItemsList(11, "", "", "", "", "", "", True, 1)
        ''***************************************

        cmbItemList.DataBind()
        'If mCalibrationItemChild.SerialNo <> "" Then
        Dim mGetSerialNos As GetEquipmentSerialNos
        mGetSerialNos = GetEquipmentSerialNos.GetEquipmentSerialNos(New Guid(cmbItemList.SelectedValue.ToString), True)
        cmbSerialNo.DataSource = mGetSerialNos
        cmbSerialNo.DataBind()
        Session("mGetSerialNos") = mGetSerialNos
        mCalibrationItem.ItemID = New Guid(cmbItemList.SelectedValue.ToString)
        mCalibrationItem.ReceiptItemID = New Guid(cmbSerialNo.SelectedValue.ToString)
        'End If
        txtDoneOnDate.Text = mCalibrationItemChild.DoneOnDateFormatted.ToString
        txtNextDueDate.Text = mCalibrationItemChild.NextDueDateFormatted.ToString

        DataBind()
    End Sub
    Private Sub SetPage()
        If Not mCalibrationItem.IsNew Then
            lblTitle.Text = Session("CalibrationItem") + "Calibration Item  [" + CType(mCalibrationItemChild.ItemName, String) + "]"
        Else
            lblTitle.Text = Session("CalibrationItem") + "Calibration Item [New]"
        End If
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)

        If custValidator.ControlToValidate = "txtDoneOnDate" Then
            If Len(txtDoneOnDate.Text) = 0 Then
                custValidator.ErrorMessage = " Please Select Done On Date."
                e.IsValid = False
                'Else
                '    e.IsValid = True
            End If
        End If
    End Sub
    Private Sub Save()
        Try
            If mCalibrationItem.IsValid Then
                mCalibrationItem.ApplyEdit()

                mCalibrationItem = mCalibrationItem.Save()
                If mCalibrationItemChild.IsValid Then
                    mCalibrationItemChild = mCalibrationItemChild.Save()
                    SaveAttachment()
                End If
                'MarkLog(Util.Action.Save, "Calibration Calibration", "Calibration" + "-> " + mCalibrationItem.ItemName, Util.ErrorType.NoError, mCalibrationItem.ID)
                Dim mCalibrationDetail As String = mCalibrationItemChild.CalibrationNo + " Done On Date : " + mCalibrationItemChild.DoneOnDate + " of " + "Part No. " + mCalibrationItem.ItemName + " Serial No. " + mCalibrationItem.SerialNo
                MarkLog(Util.Action.Save, "Calibration", mCalibrationDetail, Util.ErrorType.NoError, mCalibrationItem.ID, EventLogID)

                Session("mCalibrationItem") = mCalibrationItem
                Session("mCalibrationItemChild") = mCalibrationItemChild
                SetPage()
                ' DataFieldBind()
                DataBind()
            End If
        Catch ex As SqlException
            If ex.Number = 2627 Then
                MSGBoxCtrl.show("Duplicate Alert!", "You are trying to save the duplicate entry</br>You can not add duplicate entry in Calibration.", "", MsgBoxStyle.OkOnly, "Duplicate")
                Session("ex.Number") = "ex.Number"
                Exit Sub
            End If
        End Try
        If Session("ex.Number") = "ex.Number" Then
            '
        Else
            Session.Remove("ex.Number")
            RemoveSession()
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
                    If MSGBoxCtrl.Sender = "Close" Then  '' Close confirmation
                        Page.Validate()
                        If Not IsValid Then
                            upnlValidationSummary.Update()
                            Exit Sub 'Added By Utkarsh On 24-May-2011 
                        End If
                        If mCalibrationItem.IsValid Then
                            Save()
                            RemoveSession()
                            Dim mopenas As String = Request.QueryString("Type")
                            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                                Exit Sub
                            End If
                        Else
                            If CustomValidate1() = False Then
                                upnlValidationSummary.Update()
                                Exit Sub
                            End If
                        End If
                    Else
                    End If
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "Close" Then
                        RemoveSession()
                        Dim mopenas As String = Request.QueryString("Type")
                        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                            Exit Sub
                        End If
                    End If
                Case MsgBoxResult.Ok 'And Session("sender") = ""        'Code Added

                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    'DataFieldBind()
                    'Response.Redirect(BackPage.Pop(Session("BackPage")))
            End Select

        End If
    End Sub
    Private Sub addAttributes()
        txtFrequency.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtFrequency').value,event)")
    End Sub
    Private Sub SetNextDueDate()
        If chkIsApplicable.Checked = False Then
            txtNextDueDate.Text = ""
        Else
            If txtDoneOnDate.Text <> "" Then
                mCalibrationItemChild.DoneOnDate = txtDoneOnDate.Text
                'CalibrationItemChild.NextDueDate = CDate(mCalibrationItemChild.DoneOnDate).AddMonths(mCalibrationItemChild.Frequency)
                If mCalibrationItem.CalibrationPeriodInID = 1 Then 'Days
                    mCalibrationItemChild.NextDueDate = CDate(mCalibrationItemChild.DoneOnDate).AddDays(Val(txtFrequency.Text))
                ElseIf mCalibrationItem.CalibrationPeriodInID = 2 Then 'Month
                    mCalibrationItemChild.NextDueDate = CDate(mCalibrationItemChild.DoneOnDate).AddMonths(Val(txtFrequency.Text))
                ElseIf mCalibrationItem.CalibrationPeriodInID = 3 Then 'Year
                    mCalibrationItemChild.NextDueDate = CDate(mCalibrationItemChild.DoneOnDate).AddYears(Val(txtFrequency.Text))
                End If
                txtNextDueDate.Text = mCalibrationItemChild.NextDueDateFormatted.ToString
            End If
        End If
    End Sub
    Private Sub ControlVisibilityForAttachment()
        If mCalibrationItemChild.IsAttachmentAdded Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Private Sub NewRecord()
        If mCalibrationItem Is Nothing Or mCalibrationItemChild Is Nothing Then
            'Make new Calibration
            mCalibrationItem = CalibrationItem.NewCalibrationItem()
            mCalibrationItemChild = CalibrationItemChild.NewCalibrationItemChild(mCalibrationItem.ID, False)
            mCalibrationItemChild.DoneOnDate = Today.Date
            Session("mCalibrationItem") = mCalibrationItem
            Session("mCalibrationItemChild") = mCalibrationItemChild
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        addAttributes()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not Page.IsPostBack Then
            NewRecord()
            cmbItemList.Focus()
            DataFieldBind()
            SetNextDueDate()
            SetPage()
            ControlVisibilityForAttachment()
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If Not IsValid Then
                upnlValidationSummary.Update()
                Exit Sub 'Added By Utkarsh On 24-May-2011 
            End If
            SaveFormToObject()

            If mCalibrationItem.IsValid Then
                mCalibrationItem.ApplyEdit()

                mCalibrationItem = mCalibrationItem.Save()
                If mCalibrationItemChild.IsValid Then
                    mCalibrationItemChild = mCalibrationItemChild.Save()
                    SaveAttachment()
                End If
                'MarkLog(Util.Action.Save, "Calibration Calibration", "Calibration" + "-> " + mCalibrationItem.ItemName, Util.ErrorType.NoError, mCalibrationItem.ID)
                Dim mCalibrationDetail As String = mCalibrationItemChild.CalibrationNo + " Done On Date : " + mCalibrationItemChild.DoneOnDateFormatted + " of " + "Part No. " + mCalibrationItem.ItemName + " Serial No. " + mCalibrationItem.SerialNo
                MarkLog(Util.Action.Save, "Calibration", mCalibrationDetail, Util.ErrorType.NoError, mCalibrationItem.ID, EventLogID)

                Session("mCalibrationItem") = mCalibrationItem
                Session("mCalibrationItemChild") = mCalibrationItemChild
                SetPage()
                ' DataFieldBind()
                DataBind()
            Else
                If CustomValidate1() = False Then
                    upnlValidationSummary.Update()
                    Exit Sub
                End If
            End If
        Catch ex As SqlException
            If ex.Number = 2627 Then
                MSGBoxCtrl.show("Duplicate Alert!", "You are trying to save the duplicate entry</br>You can not add duplicate entry in Calibration.", "", MsgBoxStyle.OkOnly, "Duplicate")
                Session("Duplicate") = "Duplicate"
                Exit Sub
            End If
        End Try
        If Session("Duplicate") = "Duplicate" Then
            Session.Remove("Duplicate")
        Else
            Session.Remove("Duplicate")
            RemoveSession()
            Dim mopenas As String = Request.QueryString("Type")
            If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                Exit Sub
            End If
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        SaveFormToObject()
        If mCalibrationItemChild.IsDirty Then
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.CloseConfirm, "", MsgBoxStyle.YesNo, "Close")
            Exit Sub
        Else
            RemoveSession()
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
        cmbSerialNo.ClearSelection()
        mCalibrationItem.ReceiptItemID = Guid.Empty
        cmbSerialNo.DataSource = mGetSerialNos
        cmbSerialNo.DataBind()
        Session("mGetSerialNos") = mGetSerialNos
        mCalibrationItem.ItemID = New Guid(cmbItemList.SelectedValue.ToString)
        ' checkIsApplicable()
        DataBind()
        cmbItemList.Focus()
        SetNextDueDate()
    End Sub
    Protected Sub txtDoneOnDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        SetNextDueDate()
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        ViewImage()
    End Sub
    Private Sub cmbSerialNo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSerialNo.SelectedIndexChanged
        If cmbSerialNo.SelectedIndex > 0 Then
            mCalibrationItem.Location = mGetSerialNos(cmbSerialNo.SelectedIndex, "").Location
            txtLocation.Text = mCalibrationItem.Location
        Else
            mCalibrationItem.Location = ""
            txtLocation.Text = mCalibrationItem.Location
        End If
        mCalibrationItem.ReceiptItemID = New Guid(cmbSerialNo.SelectedValue.ToString)
        mCalibrationItem.SerialNo = IIf(cmbSerialNo.SelectedIndex > 0, cmbSerialNo.SelectedItem.Text, "")
        DataBind()
    End Sub
    Private Sub chkIsApplicable_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsApplicable.CheckedChanged
        SetNextDueDate()
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
        mCalibrationItemChild.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
#End Region

#Region " Show BrokenRules "
    Public Function CustomValidate1() As Boolean
        Dim strMsg As String = ""
        If mCalibrationItem.IsValid = False Then
            For i As Integer = 0 To mCalibrationItem.GetBrokenRulesCollection.Count - 1
                strMsg = strMsg + mCalibrationItem.GetBrokenRulesCollection(i).Description + "<Br>"
            Next
        End If
        If strMsg.Trim <> "" Then
            cvItem.ErrorMessage = strMsg
            cvItem.IsValid = False
            Return False
        End If
        Return True
    End Function
    Private Sub txtFrequency_TextChanged(sender As Object, e As System.EventArgs) Handles txtFrequency.TextChanged
        SetNextDueDate()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        mCalibrationItemChild.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mCalibrationItemChild.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mCalibrationItemChild.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mCalibrationItemChild.ID)
        End If
        Session("mFileAttach") = mFileAttach
    End Sub
#End Region

   

    
End Class