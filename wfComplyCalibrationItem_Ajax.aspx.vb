Public Class wfComplyCalibrationItem_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Protected mCalibrationItem As CalibrationItem
    Protected mCalibrationItemChildList As CalibrationItemChildList
    Protected mCalibrationItemChild As CalibrationItemChild
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    Dim EventLogID As Guid
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mCalibrationItemChild = Session("mCalibrationItemChild")
        mCalibrationItemChildList = Session("mCalibrationItemChildList")
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        Session.Remove("mCalibrationItem")
        Session.Remove("mCalibrationItemChild")
        Session.Remove("mOldCalibrationItemChild")
    End Sub
    Private Sub addAttributes()
        txtFrequency.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtFrequency').value,event)")
    End Sub
    Private Sub SaveFormToObject()
        Try
            mCalibrationItemChild.CalibrationNo = txtNo.Text
            mCalibrationItemChild.DoneOnDate = txtDoneOnDate.Text
            mCalibrationItemChild.IsApplicable = chkIsApplicable.Checked
            mCalibrationItemChild.DonebyAgency = txtDoneByAgency.Text
            mCalibrationItemChild.CertificateReference = txtCertRef.Text
            mCalibrationItemChild.Remark = txtRemark.Text
            If Not mFileAttach Is Nothing Then
                If mFileAttach.Size > 0 Then
                    mCalibrationItemChild.IsAttachmentAdded = True
                Else
                    mCalibrationItemChild.IsAttachmentAdded = False
                End If
            End If
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
        txtDoneOnDate.Text = mCalibrationItemChild.DoneOnDateFormatted.ToString
        txtNextDueDate.Text = mCalibrationItemChild.NextDueDateFormatted.ToString
        DataBind()
    End Sub
    Private Sub SetPage()
        lblTitle.Text = Session("CalibrationItem") + "Calibration Item  [" + CType(mCalibrationItemChild.ItemName, String) + "]"
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)

        If custValidator.ControlToValidate = "txtDoneOnDate" Then



            If Len(txtDoneOnDate.Text) = 0 Then
                custValidator.ErrorMessage = " Please Select Done On Date."
                e.IsValid = False
            Else
                'Added By Utkarsh On 24-May-2011

                If Not mCalibrationItemChild.PreviousCalibrationItemChildID.Equals(Guid.Empty) Then
                    Dim moldCalibrationItemChild As CalibrationItemChild
                    moldCalibrationItemChild = CalibrationItemChild.GetCalibrationItemChild(mCalibrationItemChild.PreviousCalibrationItemChildID)
                    If Not mCalibrationItemChild.DoneOnDate > CDate(moldCalibrationItemChild.DoneOnDate) Then
                        custValidator.ErrorMessage = "Done On Date should be greater than Last Done On Date (" + moldCalibrationItemChild.DoneOnDateFormatted.ToString + ")"
                        e.IsValid = False
                    ElseIf mCalibrationItemChild.DoneOnDate > Today.Date Then
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
                        Save()
                        mCalibrationItemChild.ApplyEdit()
                        RemoveSession()
                        Dim mopenas As String = Request.QueryString("Type")
                        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                            Exit Sub
                        End If
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
                    'lblTitle.Text = "Calibration Item [New]"
                    Session("sender") = ""

                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    'DataFieldBind()
                    'Response.Redirect(BackPage.Pop(Session("BackPage")))
            End Select

        End If
    End Sub
    Private Sub CheckIsapplicable()
        If chkIsApplicable.Checked = False Then
            txtNextDueDate.Text = ""
        Else
            If txtDoneOnDate.Text <> "" Then
                mCalibrationItemChild.DoneOnDate = txtDoneOnDate.Text
                mCalibrationItemChild.NextDueDate = CDate(mCalibrationItemChild.DoneOnDate).AddMonths(mCalibrationItemChild.Frequency)
                txtNextDueDate.Text = mCalibrationItemChild.NextDueDate
            End If
        End If
    End Sub
    Private Sub Save()
        mCalibrationItemChild = Session("mCalibrationItemChild")
        'SaveFormToObject()
        Try
            If mCalibrationItemChild.IsValid Then
                mCalibrationItemChild.ApplyEdit()
                mCalibrationItemChild = mCalibrationItemChild.Save()
                SaveAttachment()
                Session("mCalibrationItemChild") = mCalibrationItemChild
                Session("mCalibrationItemChildList") = mCalibrationItemChildList
                SetPage()
                DataFieldBind()

            End If
        Catch ex As Exception
            'lblTitle.Text = "CalibrationItem [New]"
            'Dim msg1 As New SIMsgBox(Page, "Duplicate Alert!<Br><Br><Br>You are trying to save the duplicate entry.", "<Br>You can not add duplicate entry in Calibration.", "", MsgBoxStyle.OKOnly)
            'msg1.ReplacePage = "wfComplyCalibrationItem.aspx?BackPage=" & Request.QueryString("BackPage")
            'msg1.Show()
        End Try
    End Sub
    Private Sub SetNextDueDate()
        If chkIsApplicable.Checked = False Then
            txtNextDueDate.Text = ""
        Else
            If txtDoneOnDate.Text <> "" Then
                mCalibrationItemChild.DoneOnDate = txtDoneOnDate.Text
                'CalibrationItemChild.NextDueDate = CDate(mCalibrationItemChild.DoneOnDate).AddMonths(mCalibrationItemChild.Frequency)
                If mCalibrationItemChild.CalibrationPeriodInID = 1 Then 'Days
                    mCalibrationItemChild.NextDueDate = CDate(mCalibrationItemChild.DoneOnDate).AddDays(Val(txtFrequency.Text))
                ElseIf mCalibrationItemChild.CalibrationPeriodInID = 2 Then 'Month
                    mCalibrationItemChild.NextDueDate = CDate(mCalibrationItemChild.DoneOnDate).AddMonths(Val(txtFrequency.Text))
                ElseIf mCalibrationItemChild.CalibrationPeriodInID = 3 Then 'Year
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
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not Page.IsPostBack Then
            txtNo.Focus()
            DataFieldBind()
            SetNextDueDate()
            SetPage()
            ControlVisibilityForAttachment()
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If Not IsValid Then
            upnlValidationSummary.Update()
            Exit Sub 'Added By Utkarsh On 24-May-2011 
        End If
        mCalibrationItemChild = Session("mCalibrationItemChild")
        SaveFormToObject()
        Try
            If mCalibrationItemChild.IsValid Then
                mCalibrationItemChild.ApplyEdit()
                mCalibrationItemChild = mCalibrationItemChild.Save()
                SaveAttachment()
                Session("mCalibrationItemChild") = mCalibrationItemChild
                Dim mCalibrationDetail As String = mCalibrationItemChild.CalibrationNo + " Done On Date : " + mCalibrationItemChild.DoneOnDateFormatted + " of " + "Part No. " + mCalibrationItemChild.ItemName + " Serial No. " + mCalibrationItemChild.SerialNo
                MarkLog(Util.Action.Save, "Calibration", mCalibrationDetail, Util.ErrorType.NoError, mCalibrationItemChild.ID, EventLogID)
                Session("mCalibrationItemChildList") = mCalibrationItemChildList
                SetPage()
                DataFieldBind()
                RemoveSession()
                Dim mopenas As String = Request.QueryString("Type")
                If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            'lblTitle.Text = "CalibrationItem [New]"
            'Dim msg1 As New SIMsgBox(Page, "Duplicate Alert!<Br><Br><Br>You are trying to save the duplicate entry.", "<Br>You can not add duplicate entry in Calibration.", "", MsgBoxStyle.OKOnly)
            'msg1.ReplacePage = "wfComplyCalibrationItem.aspx?BackPage=" & Request.QueryString("BackPage")
            'msg1.Show()
        End Try
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
    Protected Sub txtDoneOnDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        SetNextDueDate()
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        ViewImage()
    End Sub
    Private Sub chkIsApplicable_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIsApplicable.CheckedChanged
        mCalibrationItemChild.IsApplicable = chkIsApplicable.Checked
        SetNextDueDate()
    End Sub
    Private Sub txtFrequency_TextChanged(sender As Object, e As System.EventArgs) Handles txtFrequency.TextChanged
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
    Private Sub btnPrintSticker_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintSticker.Click
        Dim ds As New dsCalibrationItemChild
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        If AppSettings("ClientCode") = "IRMI" Then
            myReport = New TagCalibrationForIRM
        End If
        Dim mCompanyDetail As New CompanyDetail
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        Dim mReport As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, _
                                    mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, "", "", "", "", "", "", " ", "", "")
        mCalibrationItemChild = Session("mCalibrationItemChild")

        da.Fill(ds, mrptImage)
        da.Fill(ds, mReport)
        da.Fill(ds, mCalibrationItemChild)
        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
#End Region

    
End Class