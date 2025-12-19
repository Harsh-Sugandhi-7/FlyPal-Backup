'********************************************
'Added by vikrant on 28-Aug-2015
'Modified by Harsh Sugandhi on 20th September 2024 for FLYPAL-1906 Provision for Multiple Attachment on Audit's Finding Detail page
'********************************************

Public Class wfAuditExecutionTaskFinding_Ajax
    Inherits Page

#Region " Variable Declaration "

    Public mAuditExecution As AuditExecution
    Public mAuditPriorityList As AuditPriorityList
    Public mFindingStatusList As FindingStatusList
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    Dim mRootCauseList As RootCauseList

#End Region

#Region " Buisness Method And Properties "

    Private Sub GetSession()
        mAuditExecution = Session("mAuditExecution")
        mAuditPriorityList = Session("mAuditPriorityList")
        mFindingStatusList = Session("mFindingStatusList")
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
    End Sub

    Private Sub GetAttachment()

        If mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.IsAttachmentAdded And
           mFileAttach Is Nothing Then

            mFileAttach = FileAttach.GetAttachment(mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.ID)
            Session("mFileAttach") = mFileAttach

        End If

    End Sub

    Private Function Setobject() As Boolean

        mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.SrNo = mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentIndex + 1
        mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.Category = Trim(txtCategory.Text)
        mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.FindingNo = Trim(txtFindingNo.Text)
        mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.Finding = Trim(txtFinding.Text)
        mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.RootCause = Trim(txtRootCause.Text)
        mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.RootCauseID = New Guid(cmbRootCause.SelectedValue)
        mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.PriorityID = cmbPriority.SelectedValue
        mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.FindingStatusID = cmbFindingStatus.SelectedValue
        mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.Location = Trim(txtLocation.Text)
        mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.Reference = Trim(txtReference.Text)
        mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.KindAttention = Trim(txtKindAttention.Text)

        If txtDeadlineDate.Text <> "" Then
            mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.DeadlineDate = CDate(txtDeadlineDate.Text)
        Else
            mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.DeadlineDate = DBNull.Value
        End If

        mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.CAPA = Trim(txtCorrectiveAction.Text)
        mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.Preventive = Trim(txtPreventiveAction.Text)
        mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.ToMailID = Trim(txtToMailID.Text)
        mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.CCMailID = Trim(txtCCMailID.Text)

        If txtCorrectionDate.Text <> "" Then
            mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.CorrectionDate = CDate(txtCorrectionDate.Text)
        Else
            mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.CorrectionDate = DBNull.Value
        End If

        mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.Remark = Trim(txtRemark.Text)
        mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.HeadOfQualityRemark = Trim(TxtHeadRemark.Text) 'Ajay added 23-06-2023

        mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.IsAttachmentAdded = IIf(mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.FileAttachments.Count > 0,
                                                                                                                       True,
                                                                                                                       False)

        For I As Integer = 0 To mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.FileAttachments.Count - 1

            Dim FileName As TextBox
            FileName = CType(MultipleAttachment.Rows(I).FindControl("txtFileName"), TextBox)
            mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.FileAttachments(I).FileName = FileName.Text.Trim.ToString

        Next

        Session("mAuditExecution") = mAuditExecution

    End Function

    Private Sub ControlVisibility()

        If mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.IsNew Then
            btnPrint.Enabled = False
        Else
            btnPrint.Enabled = True
        End If

        If AppSettings("ClientCode") = "STR" Then
            lblLocation.InnerText = "Corrective Action Verification"
            lblAuditCategory.InnerText = "Containment Action"
            lblPriority.InnerText = "Level of Finding"
            txtCategory.Enabled = False
        ElseIf AppSettings("ClientCode") = "KAS" Then
            lblLocation.InnerText = "Corrective Action Verification"
        End If

        If AppSettings("ClientCode") = "KAS" Then 'Ajay 23-06-2023
            TxtHeadRemark.Visible = True
            lblHeadRemark.Visible = True
        Else
            TxtHeadRemark.Visible = False
            lblHeadRemark.Visible = False
        End If

        'added by Ajay 15-Nov-2022
        txtCategory.Enabled = Not (mAuditExecution.AuditStatusID = 2)
        txtCorrectiveAction.Enabled = Not (mAuditExecution.AuditStatusID = 2)
        txtCorrectionDate.Enabled = Not (mAuditExecution.AuditStatusID = 2)
        txtRemark.Enabled = Not (mAuditExecution.AuditStatusID = 2)
        txtKindAttention.Enabled = Not (mAuditExecution.AuditStatusID = 2)
        txtPreventiveAction.Enabled = Not (mAuditExecution.AuditStatusID = 2)
        btnOk.Enabled = Not (mAuditExecution.AuditStatusID = 2)
        cmbRootCause.Enabled = Not (mAuditExecution.AuditStatusID = 2)
        txtRootCause.Enabled = Not (mAuditExecution.AuditStatusID = 2)
        cmbFindingStatus.Enabled = Not (mAuditExecution.AuditStatusID = 2)
        txtFindingNo.Enabled = Not (mAuditExecution.AuditStatusID = 2)
        txtFinding.Enabled = Not (mAuditExecution.AuditStatusID = 2)
        cmbPriority.Enabled = Not (mAuditExecution.AuditStatusID = 2)
        txtDeadlineDate.Enabled = Not (mAuditExecution.AuditStatusID = 2)
        txtReference.Enabled = Not (mAuditExecution.AuditStatusID = 2)
        txtLocation.Enabled = Not (mAuditExecution.AuditStatusID = 2)
        imgbtnRootCause.Enabled = Not (mAuditExecution.AuditStatusID = 2)
        'btnSelectFile.Disabled = (mAuditExecution.AuditStatusID = 2)
        TxtHeadRemark.Enabled = Not (mAuditExecution.AuditStatusID = 2) 'Ajay added 23-06-2023

    End Sub

    Private Sub MessageBoxResult()

        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result

        If Result1 > 0 Then

            Select Case Result1
                Case MsgBoxResult.Yes

                    If MSGBoxCtrl.Sender = "Close" Then  '' Close confirmation

                        Session("sender") = ""
                        If (Not User.IsInRole("AuditExecutionNew") And
                            Not User.IsInRole("AuditExecutionEdit")) Then

                            MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
                                            MSGBox.Message_text.Authorization,
                                            "",
                                            MsgBoxStyle.OkOnly,
                                            "")
                            Exit Sub

                        End If

                        If mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.IsValid Then

                            Session.Remove("IsValid")
                            DataFieldBind()

                            If Save() Then

                                mAuditExecution = Session("mAuditExecution")
                                Setobject()
                                Session("mAuditExecution") = mAuditExecution
                                Session.Remove("FindingEdit")
                                Session.Remove("mFileAttach")
                                mFileAttach = Nothing
                                Dim mopenas As String = Request.QueryString("Type")

                                If mopenas IsNot Nothing AndAlso mopenas = "pup" Then
                                    ScriptManager.RegisterStartupScript(Me, [GetType], "onclose", "CallParentCallback();", True)
                                    Exit Sub
                                End If

                            End If

                        Else
                            Session.Remove("IsValid")
                        End If

                        'Added by Harsh Sugandhi on 20th September 2024 for FLYPAL-1906 Provision for Multiple Attachment on Audit's Finding Detail page
                    ElseIf MSGBoxCtrl.Sender = "RemoveAttachment" Then

                        Dim AuditExecution As AuditExecution
                        Try

                            Session("Sender") = ""
                            AuditExecution = CType(Session("mAuditExecution"), AuditExecution)
                            AuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.FileAttachments.Remove(AuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.FileAttachments.CurrentItem)
                            MultipleAttachment.DataSource = AuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.FileAttachments
                            MultipleAttachment.DataBind()
                            upnlMultipleAttachment.Update()
                            upnlGVMultipleAttachment.Update()

                            Session("mAuditExecution") = AuditExecution

                        Catch ex As SqlException

                            If ex.Number = 8145 Then

                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                                MSGBox.Message_text.ProcedureError,
                                                ex.Procedure,
                                                MsgBoxStyle.OkOnly,
                                                "")

                            ElseIf ex.Number = 2627 Then

                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                                MSGBox.Message_text.Duplicate,
                                                ex.Procedure,
                                                MsgBoxStyle.OkOnly,
                                                "")

                            ElseIf ex.Number = 547 Then

                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete,
                                                MSGBox.Message_text.ReferenceDelete,
                                                ex.Procedure,
                                                MsgBoxStyle.OkOnly, "")

                            End If

                        End Try

                    End If

                Case MsgBoxResult.No

                    If MSGBoxCtrl.Sender = "Close" Then

                        Session.Remove("IsValid")
                        Session("Sender") = ""

                        'Added by Saylee on 19-Sep-2022, in order to reset object when no rights
                        If (Not User.IsInRole("AuditExecutionNew") And
                            Not User.IsInRole("AuditExecutionEdit")) Then

                            mAuditExecution = AuditExecution.GetAuditExecution(mAuditExecution.ID)
                            Session("mAuditExecution") = mAuditExecution

                        End If

                        If (mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.IsNew) And
                           (Session("FindingEdit") IsNot Nothing And Not Session("FindingEdit") = True) Then mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.Remove(mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem)

                        Session("mAuditExecution") = mAuditExecution
                        Session.Remove("FindingEdit")
                        Dim mopenas As String = Request.QueryString("Type")
                        If mopenas IsNot Nothing AndAlso mopenas = "pup" Then

                            ScriptManager.RegisterStartupScript(Me,
                                                                [GetType],
                                                                "onclose",
                                                                "CallParentCallback();",
                                                                True)
                            Exit Sub

                        End If

                    Else
                        Session("Sender") = ""
                    End If

                Case MsgBoxResult.Ok
                    Session("Sender") = ""
                Case Else
                    Session("Sender") = ""
            End Select

        ElseIf Result1 = -1 Then
            Session("Sender") = ""
        End If

    End Sub

    Public Function Save() As Boolean

        Setobject()

        If mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.IsNew And
           mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.Contains(mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem) And
           (Not Session("FindingEdit") = True) Then

            MSGBoxCtrl.show(MSGBox.Message_title.Duplicate,
                            MSGBox.Message_text.Duplicate,
                            "Findings",
                            MsgBoxStyle.OkOnly,
                            "")
            mAuditExecution.CancelEdit()

            Exit Function

        Else

            Session("mAuditExecution") = mAuditExecution
            Session.Remove("FindingEdit")
            Return True

        End If

    End Function

    'Added by Harsh Sugandhi on 20th September 2024 for FLYPAL-1906 Provision for Multiple Attachment on Audit's Finding Detail page
    Private Sub DeleteAttachment(Index As Int32)

        MSGBoxCtrl.show(MSGBox.Message_title.RemoveItem,
                        MSGBox.Message_text.RemoveItem,
                        "",
                        MsgBoxStyle.YesNo,
                        "RemoveAttachment")

        mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.FileAttachments.CurrentIndex = Index
        Session("mAuditExecution") = mAuditExecution

    End Sub

    Private Sub AttachFile()

        Dim BackupPath As String = ""
        BackupPath = AppSettings("DOCPath") & "New.PDF"
        mAuditExecution = Session("mAuditExecution")

        Try

            If Not mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.FileAttachments.Contains(mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.ID, CType(Session("FileUpload.FileName"), String)) Then

                mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.FileAttachments.Add(mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.ID, CType(Session("FileUpload.FileName"), String))
                mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.FileAttachments.CurrentItem.ImageFile = CType(Session("ImageFile"), Byte())
                mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.FileAttachments.CurrentItem.Size = Session("Size")
                mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.FileAttachments.CurrentItem.Extension = Session("Extension")

                Session("mAuditExecution") = mAuditExecution

                MultipleAttachment.DataSource = mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.FileAttachments
                MultipleAttachment.DataBind()

                For i As Integer = 0 To mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.FileAttachments.Count - 1

                    Dim FileName As TextBox
                    FileName = CType(MultipleAttachment.Rows(i).FindControl("txtFileName"), TextBox)
                    FileName.Text = mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.FileAttachments(i).FileName

                Next

                Session.Remove("Size")
                Session.Remove("ImageFile")
                Session.Remove("Extension")
                Session.Remove("FileUpload.FileName")
                upnlMultipleAttachment.Update()
                upnlGVMultipleAttachment.Update()

            Else

                Session("mAuditExecution") = mAuditExecution
                MSGBoxCtrl.show(MSGBox.Message_title.Duplicate,
                                MSGBox.Message_text.Duplicate,
                                "",
                                MsgBoxStyle.OkOnly,
                                "")
                Exit Sub

            End If

        Catch ex As Exception
        End Try

    End Sub
    'End

#End Region

#Region " DataBind Methods "

    Public Sub DataFieldBind()

        mAuditPriorityList = AuditPriorityList.GetAuditPriorityList("(SELECT)")
        cmbPriority.DataSource = mAuditPriorityList
        Session("mAuditPriorityList") = mAuditPriorityList

        mRootCauseList = RootCauseList.GetRootCauseList("(SELECT)")
        cmbRootCause.DataSource = mRootCauseList
        Session("mRootCauseList") = mRootCauseList

        mFindingStatusList = FindingStatusList.GetFindingStatusList()
        cmbFindingStatus.DataSource = mFindingStatusList
        Session("mFindingStatusList") = mFindingStatusList

        txtDeadlineDate.Text = mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.DeadlineDateFormatted.ToString
        txtCorrectionDate.Text = mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.CorrectionDateFormatted.ToString

        MultipleAttachment.DataSource = mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.FileAttachments

        DataBind()

    End Sub

    Private Sub SetTitle()

        If mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.IsNew Then
            lblTitle.Text = "Finding Detail [New]"
        Else

            If Len(mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.FindingNo) > 15 Then
                lblTitle.Text = "Finding Detail &nbsp;&nbsp;[ " & mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.FindingNo.Substring(0, 15) & "...]"
            Else
                lblTitle.Text = "Finding Detail &nbsp;&nbsp;[ " & mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.FindingNo & " ]"
            End If

        End If

    End Sub

    Private Sub ControlVisibilityForFileAttachment()

        If mFileAttach Is Nothing Then
            GetAttachment()
        End If

    End Sub

#End Region

#Region " Events "

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        GetSession()

        If Not IsPostBack Then

            If txtFindingNo.Enabled Then
                txtFindingNo.Focus()
            End If
            DataFieldBind()
            SetTitle()
            ControlVisibility()
            ControlVisibilityForFileAttachment()

        End If

    End Sub

    Private Sub SaveRecord(sender As Object, e As EventArgs) Handles btnOk.Click


        Try

            If (Not User.IsInRole("AuditExecutionNew") And Not User.IsInRole("AuditExecutionEdit")) Then

                MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
                                MSGBox.Message_text.Authorization,
                                "",
                                MsgBoxStyle.OkOnly,
                                "")

                Exit Sub

            End If

            If IsValid Then

                Setobject()

                If mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.IsNew And
                   mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.Contains(mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem) And
                   (Not Session("FindingEdit") = True) Then

                    MSGBoxCtrl.show(MSGBox.Message_title.Duplicate,
                                    MSGBox.Message_text.Duplicate,
                                    "Findings",
                                    MsgBoxStyle.OkOnly,
                                    "")

                    mAuditExecution.CancelEdit()

                    Exit Sub

                Else

                    mAuditExecution.ApplyEdit()
                    Session("mAuditExecution") = mAuditExecution

                    If mAuditExecution.IsValid Then
                        mAuditExecution = mAuditExecution.Save()
                    End If

                    Session.Remove("FindingEdit")
                    Session.Remove("mFileAttach")
                    mFileAttach = Nothing
                    ControlVisibility()

                    MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully,
                                    MSGBox.Message_text.SavedSuccessFully,
                                    "",
                                    MsgBoxStyle.OkOnly,
                                    "")

                    Dim mopenas As String = Request.QueryString("Type")
                    If mopenas IsNot Nothing AndAlso mopenas = "pup" Then

                        ScriptManager.RegisterStartupScript(Me,
                                                            [GetType],
                                                            "onclose",
                                                            "CallParentCallback();",
                                                            True)
                        Exit Sub

                    End If

                End If

            End If

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    'Added by Harsh Sugandhi on 20th September 2024 for FLYPAL-1906 Provision for Multiple Attachment on Audit's Finding Detail page
    Private Sub SelectFile(sender As Object, e As EventArgs) Handles AddAttachment.Click

        Try

            Setobject()
            Session("mAuditExecution") = mAuditExecution
            Session("mFileAttach") = Nothing
            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "OpenFileUploadWindow",
                                                "OpenFileUploadWindow()",
                                                True)

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub HdnFileUpload_Click(sender As Object, e As EventArgs) Handles hdnBtnFileUpload.Click

        Try

            AttachFile()
            upnlMultipleAttachment.Update()
            upnlGVMultipleAttachment.Update()

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub Back_Click(sender As Object, e As EventArgs) Handles btnBack.Click

        If (mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.IsNew) And
           (Session("FindingEdit") IsNot Nothing And Not Session("FindingEdit") = True) Then
            mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.Remove(mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem)
        End If

        Session.Remove("FindingEdit")
        Session.Remove("mFileAttach")
        mFileAttach = Nothing
        Session("mAuditExecution") = mAuditExecution

        Dim mopenas As String = Request.QueryString("Type")

        If mopenas IsNot Nothing AndAlso mopenas = "pup" Then

            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "onclose",
                                                "CallParentCallback();",
                                                True)
            Exit Sub

        End If

    End Sub

    Private Sub PrintRecord(sender As Object, e As EventArgs) Handles btnPrint.Click

        If (Not User.IsInRole("AuditExecutionPrint")) Then

            MSGBoxCtrl.show(MSGBox.Message_title.Authorization,
                            MSGBox.Message_text.Authorization,
                            "",
                            MsgBoxStyle.OkOnly,
                            "")

            Exit Sub

        End If

        Dim myReport As Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim da As New ObjectAdapter
        Dim mrptAuditFindings As rptAuditFindings
        Dim dsrptAuditFindings As New dsrptAuditFindings
        Dim ReportName As String = ""
        Dim muser As User = SI.UTILITY.User.GetUser(HttpContext.Current.User.Identity.Name)

        If AppSettings("ClientCode") = "Heligo" Then
            myReport = New crFindingReportForHeligo
        Else
            myReport = New crFindingReport
        End If


        If AppSettings("ClientCode") = "KAS" Then
            ReportName = "Corrective Action Request Form"
        Else
            ReportName = "Audit Findings Report"
        End If


        mCompanyDetail = CompanyDetail.GetCompanyDetail("",
                                                        "",
                                                        "",
                                                        "",
                                                        "",
                                                        "",
                                                        "")

        Dim Report As New ReportData(mCompanyDetail.CompanyName,
                                     mCompanyDetail.Address,
                                     mCompanyDetail.Tel1,
                                     mCompanyDetail.Tel2,
                                     mCompanyDetail.Fax,
                                     mCompanyDetail.Email,
                                     mCompanyDetail.WebSite,
                                     ReportName,
                                     SearchStr1:=AppSettings("FormnNoInAudit"),
                                     SearchStr2:=AppSettings("IssueNoInAudit"),
                                     SearchStr3:=AppSettings("RevisionNoInAudit"),
                                     SearchStr4:=muser.EmployeeName,
                                     SearchStr5:="",
                                     ProductVersion:=AppSettings("Product Version"),
                                     SINote:=AppSettings("SINote"),
                                     SearchStr6:="",
                                     SearchStr7:="",
                                     SearchStr8:="",
                                     SearchStr9:="",
                                     SearchStr10:=AppSettings("Logo"),
                                     SearchStr11:=AppSettings("ClientCode")) 'Changed By Utkarsh For Report Logo.

        '----------------------------------------------------------

        mrptAuditFindings = rptAuditFindings.GetrptAuditFindings("1/1/1900",
                                                                 "1/1/2100",
                                                                 mAuditExecution.AuditNo, , ,
                                                                 mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.ID.ToString,
                                                                 UsedFromFindingEntry:=1)

        If mrptAuditFindings.Count <= 0 Then

            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound,
                            MSGBox.Message_text.NoRecordFound,
                            "There is no record for this search criteria",
                            MsgBoxStyle.OkOnly,
                            "")

            Exit Sub

        End If

        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(dsrptAuditFindings)
        '----------------------------------------------------------
        da.Fill(dsrptAuditFindings, mrptAuditFindings)
        da.Fill(dsrptAuditFindings, Report)
        da.Fill(dsrptAuditFindings, mrptImage) 'Added by Utkarsh for Report Logo
        myReport.SetDataSource(dsrptAuditFindings)
        Session("CrystalReport") = myReport

        ScriptManager.RegisterStartupScript(Me,
                                            [GetType],
                                            "openTranDetail",
                                            "openTranDetail();",
                                            True)

    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

    Private Sub Priority_Changed(sender As Object, e As EventArgs) Handles cmbPriority.SelectedIndexChanged

        If AppSettings("ClientCode") = "STR" Then

            If cmbPriority.SelectedIndex = 0 Then
                txtDeadlineDate.Text = ""
            ElseIf cmbPriority.SelectedIndex = 1 Then
                txtDeadlineDate.Text = CDate(mAuditExecution.StartDate).AddDays(mAuditPriorityList.Item(1).Days).ToString(AppSettings("DateFormat"))      'Level 1 (0 days)
            ElseIf cmbPriority.SelectedIndex = 2 Then
                txtDeadlineDate.Text = CDate(mAuditExecution.StartDate).AddDays(mAuditPriorityList.Item(2).Days).ToString(AppSettings("DateFormat"))      'Level 2 (20 days)
            ElseIf cmbPriority.SelectedIndex = 3 Then
                txtDeadlineDate.Text = CDate(mAuditExecution.StartDate).AddDays(mAuditPriorityList.Item(3).Days).ToString(AppSettings("DateFormat"))     'Level 3 (60 days)
            End If

        Else

            If cmbPriority.SelectedIndex = 0 Then
                txtDeadlineDate.Text = ""
            ElseIf cmbPriority.SelectedIndex = 1 Then
                txtDeadlineDate.Text = CDate(mAuditExecution.StartDate).AddDays(mAuditPriorityList.Item(1).Days).ToString(AppSettings("DateFormat"))      'Level 1 (7 days)
            ElseIf cmbPriority.SelectedIndex = 2 Then
                txtDeadlineDate.Text = CDate(mAuditExecution.StartDate).AddDays(mAuditPriorityList.Item(2).Days).ToString(AppSettings("DateFormat"))     'Level 2 (30 days)
            ElseIf cmbPriority.SelectedIndex = 3 Then
                txtDeadlineDate.Text = CDate(mAuditExecution.StartDate).AddDays(mAuditPriorityList.Item(3).Days).ToString(AppSettings("DateFormat"))     'Level 3 (0 days)
            End If

        End If

        upnlFindingDetails.Update()

    End Sub

    Private Sub BtnRootCause_Click(sender As Object, e As EventArgs) Handles imgbtnRootCause.Click

        Setobject()
        ScriptManager.RegisterStartupScript(Me,
                                            [GetType],
                                            "OpenRootCauseWindow",
                                            "OpenRootCauseWindow()",
                                            True)

    End Sub

    Private Sub HdnBtnRootCause_Click(sender As Object, e As EventArgs) Handles hdnimgBtnRootCause.Click

        Setobject()
        mRootCauseList = RootCauseList.GetRootCauseList("(SELECT)")
        cmbRootCause.DataSource = mRootCauseList
        Session("mRootCauseList") = mRootCauseList
        cmbRootCause.DataBind()
        upnlFindingDetails.Update()

    End Sub

    'Added by Harsh Sugandhi On 20th September 2024 For FLYPAL-1906 Provision For Multiple Attachment On Audit's Finding Detail page
    Private Sub GV_MultipleAttachment_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles MultipleAttachment.RowCommand

        Dim FileAttachments As New FileAttachments
        Try

            Select Case e.CommandName
                Case "View"

                    Dim Index As Integer = CInt(e.CommandArgument)

                    Dim No As New Random
                    Dim StrName As String = "abc" & No.Next.ToString
                    FileAttachments = mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.FileAttachments

                    If FileAttachments.Count = 1 Then
                        FileAttachments.CurrentIndex = 0
                    Else
                        FileAttachments.CurrentIndex = Index - 1
                    End If

                    If FileAttachments.CurrentItem.Size > 0 Then

                        Dim filepath As String = AppSettings("DOCPath") & StrName & FileAttachments.CurrentItem.Extension
                        Dim fs As FileStream

                        If File.Exists(AppSettings("DOCPath")) = False Then

                            'Delete File if exist
                            File.Delete(AppSettings("DOCPath") & StrName & FileAttachments.CurrentItem.Extension)
                            ' Create the file.
                            fs = File.Create(filepath)
                            '' Add some information to the file.
                            fs.Write(FileAttachments.CurrentItem.ImageFile,
                                     0,
                                     FileAttachments.CurrentItem.ImageFile.Length)

                            fs.Close()
                            Session("DOCPath") = filepath
                            ScriptManager.RegisterStartupScript(Me,
                                                                [GetType],
                                                                "Open File",
                                                                "openFile();",
                                                                True)

                        End If

                    End If

                    MultipleAttachment.DataSource = mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.FileAttachments
                    MultipleAttachment.DataBind()
                    ControlVisibility()
                    upnlMultipleAttachment.Update()
                    upnlMultipleAttachment.Update()

                Case "Remove"

                    Dim Index As Integer = CInt(e.CommandArgument)
                    FileAttachments = mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.FileAttachments

                    If FileAttachments.Count = 1 Then
                        DeleteAttachment(0)
                    Else
                        DeleteAttachment(Index - 1)
                    End If

            End Select

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

#End Region

End Class