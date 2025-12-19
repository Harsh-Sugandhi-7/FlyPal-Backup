Partial Class wfEmployeeTraining_Ajax
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents rfvName As System.Web.UI.WebControls.RequiredFieldValidator

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region " Variable Declaration "
    Public mEmployee As Employee
    Public mEmployeeTraining As EmployeeTraining

    Public mTrainingList As TrainingList
    Public mTrainingOrgList As TrainingOrgList
    Public mMonthList As MonthList

    'Public mFreqInMonths As Integer = 0
    Public mIsRenew As Boolean = False
    Public BackPage As String
    Public IsFromRenewal As Boolean = False
    Public mEmployeeTrainingList As EmployeeTrainingList

    Dim EventLogID As Guid 'Added by Saylee on 20-July-2011
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False

    Dim mtmpTraining As Training
#End Region

#Region " Helper Methods "
    Public Sub GetSession()
        mEmployee = Session("mEmployee")
        mEmployeeTraining = Session("mEmployeeTraining")
        'mEmployeeTrainingList = Session("mEmployeeTrainingList")
        mTrainingList = Session("mTrainingList")
        mTrainingOrgList = Session("mTrainingOrgList")
        mMonthList = Session("mMonthList")
        'mFreqInMonths = Session("mFreqInMonths")
        mIsRenew = CType(Session("IsRenew"), Boolean)
        mEmployeeTrainingList = Session("mEmployeeTrainingList")
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
    End Sub
    Private Sub SetSession()
        Session("mEmployeeTraining") = mEmployeeTraining
        'Session("mEmployeeSkillList") = mEmployeeSkillList
        Session("mTrainingList") = mTrainingList
        Session("mTrainingOrgList") = mTrainingOrgList
        Session("mMonthList") = mMonthList
        Session("mEmployee") = mEmployee

    End Sub

	Private Sub ControlVisibilityForAttachment()
        If mEmployeeTraining.IsAttachmentAdded Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Private Sub DataFieldBind()
        mTrainingList = TrainingList.GetTrainingList(, , , "<SELECT>")
        cmbTrainingList.DataSource = mTrainingList
        Session("mTrainingList") = mTrainingList

        mTrainingOrgList = TrainingOrgList.GetTrainingOrgList(, , , "<SELECT>")
        cmbTrainingOrgList.DataSource = mTrainingOrgList
        Session("mTrainingOrgList") = mTrainingOrgList

        mMonthList = MonthList.GetMonthList("<SELECT>")
        cmbMonthList.DataSource = mMonthList
        Session("mMonthList") = mMonthList

        txtDate.Text = mEmployeeTraining.EmployeeTrainingDate.ToString

        DataBind()

       
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Dim msgCount As Integer = 0
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    
                Case MsgBoxResult.No
                    Session("sender") = ""
                    'Response.Redirect("wfEmployeeTraining_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1"))
                Case MsgBoxResult.OK ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    'DataFieldBind()
                    'Response.Redirect("wfEmployeeTraining_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1"))
                Case MsgBoxResult.OK And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    'DataFieldBind()
                    'Response.Redirect("wfEmployeeTraining_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            'DataFieldBind()
            'Response.Redirect("wfEmployeeTraining_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1"))
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            Session("sender") = ""
            'DataFieldBind()
        End If
    End Sub
    Private Sub SetTitle()
        If mEmployeeTraining.IsNew Then
            lblTitle.Text = "Employee Training Information [New]"
        Else
            If Len(mEmployeeTraining.TrainingName) > 15 Then
                lblTitle.Text = "Employee Training Information [" & mEmployeeTraining.TrainingName.Substring(0, 15) & "...]"
            Else
                lblTitle.Text = "Employee Training Information [" & mEmployeeTraining.TrainingName & "]"
            End If
        End If
        upnlTitle.Update()
    End Sub
    Private Sub Save()
        SetObject()
        mEmployee.Save()
        If cmbTrainingList.Enabled = True Then
            setFocus(cmbTrainingList)
        End If
        MarkLog(Flypal.Util.Action.Save, "Employee", mEmployee.EmpNo, Flypal.Util.ErrorType.HandledError, Guid.Empty, EventLogID)
        SetSession()
        SetTitle()
    End Sub
    Private Sub SetObject()
        mEmployeeTraining.EmployeeID = mEmployee.ID
        mEmployeeTraining.TrainingID = New Guid(cmbTrainingList.SelectedValue)
        mEmployeeTraining.CertificateNo = Trim(txtCertificateNo.Text)
        mEmployeeTraining.Date = CType(txtDate.Text, Object)
        mEmployeeTraining.Duration = txtDuration.Text
        mEmployeeTraining.TrainingOrgID = New Guid(cmbTrainingOrgList.SelectedValue)
        mEmployeeTraining.MonthOfTrainingID = CInt(cmbMonthList.SelectedValue)
        mEmployeeTraining.YearOfTraining = Trim(txtYearOfTraining.Text)
        mEmployeeTraining.Remark = Trim(txtRemark.Text)

        'Added by Saylee on 4-Oct-2021 for Heligo01102021
        mEmployeeTraining.IsNOTApplicable = chkIsNOTApplicable.Checked
        mEmployeeTraining.RecurringStatus = chkRecurringStatus.Checked
        If txtFreqInMonths.Text = "" Then
            mEmployeeTraining.FreqInMonths = 0
        Else
            mEmployeeTraining.FreqInMonths = Trim(txtFreqInMonths.Text)
        End If
        If txtWarningDays.Text = "" Then
            mEmployeeTraining.WarningDays = 0
        Else
            mEmployeeTraining.WarningDays = Trim(txtWarningDays.Text)
        End If

        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                mEmployeeTraining.IsAttachmentAdded = True
            Else
                mEmployeeTraining.IsAttachmentAdded = False
            End If
        End If
        '**********************************************
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
                If (Not mEmployeeTraining.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mEmployeeTraining.ID)
                End If
                IsAttachmentDeleted = False
                Session("IsAttachmentDeleted") = IsAttachmentDeleted
            End If
        End If
    End Sub
    Private Sub addAttributes()
		'Duration
		'txtDuration.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtDuration').value,event)")
		'Year of Training
		txtYearOfTraining.Attributes.Add("onKeyPress", "validateText('D',document.getElementById('txtYearOfTraining').value,event)")
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Saylee on 20-July-2011
        If Not IsPostBack And Session("sender") = "" Then
            If cmbTrainingList.Enabled = True Then
                setFocus(cmbTrainingList)
            End If
            DataFieldBind()
            'If mIsRenew = True Then 'Added by Archana on Dec,09,2009 - reported bug by Pramod
            '    Dim fileSize1 As Integer = 0
            '    Dim file1(fileSize1) As Byte
            '    mEmployeeTraining.ImageFile = file1
            '    mEmployeeTraining.ImageSize = 0
            '    ImageButton1.Visible = False
            '    btnDelAttach.Enabled = False
            'Else
            ControlVisibilityForAttachment()
            'End If
            SetTitle()
        End If

        'Added By Vikrant on 15-Oct-2012 For ALL11102012
        If mIsRenew = False And mEmployeeTraining.ReferenceID.Equals(Guid.Empty) Then
            cmbTrainingList.Enabled = True
        Else
            cmbTrainingList.Enabled = False
        End If
        'End
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("EmployeeTrainingNew") And mEmployeeTraining.IsNew) Or (Not User.IsInRole("EmployeeTrainingEdit") And Not mEmployeeTraining.IsNew) Then
            SetObject()
            SetSession()
            MarkLog(Flypal.Util.Action.Save, "Employee Training", User.Identity.Name & " is not Authorized User to save" + " Emp : " + mEmployee.EmpNoName + " Skill : " + mEmployeeTraining.TrainingName, Flypal.Util.ErrorType.HandledError, mEmployeeTraining.ID, EventLogID)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If
        If IsValid Then
            Try
                SetObject()

                If ((mEmployeeTrainingList.Contains(mEmployeeTraining.EmployeeID, mEmployeeTraining.TrainingID, mEmployeeTraining.ReferenceID)) And mEmployeeTraining.IsNew) Then
                    MSGBoxCtrl.show("Duplicate Alert!", "You are trying to save the duplicate entry.", "You can not add duplicate entry. ", MsgBoxStyle.OkOnly, "")
                Else

                    If mEmployeeTraining.IsValid Then


                        mEmployeeTraining.Save()
                        SetSession()
                        SaveAttachment()
                        lblTitle.Text = "Employee Training Information [New]"
                        MarkLog(Flypal.Util.Action.Save, "Employee Training", "Emp : " + mEmployee.EmpNoName + " Training : " + mTrainingList(mEmployeeTraining.TrainingID).Name, Flypal.Util.ErrorType.NoError, mEmployeeTraining.ID, EventLogID)
                        'Response.Redirect("wfEmployeeDetails.aspx")
                        Session.Remove("mEmployeeTrainingList")
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                        'If Request.QueryString("ChildPage1") = "wfEmployeeDetails.aspx" Then
                        '    Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
                        'Else
                        '    Response.Redirect(Request.QueryString("BackPage") & "?ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage=" & Request.QueryString("ChildPage"))
                        'End If
                    Else
                        Dim strMsg As String = ""
                        If Not mEmployeeTraining.IsValid Then
                            For j As Integer = 0 To mEmployeeTraining.GetBrokenRulesCollection.Count - 1
                                strMsg = strMsg + mEmployeeTraining.GetBrokenRulesCollection(j).Description + "<BR>"
                            Next
                        End If

                        If strMsg.Trim <> "" Then
                            'cvDate.ErrorMessage = strMsg
                            'cvDate.IsValid = mEmployeeTraining.IsValid
                        End If
                        upnlValidationSummary.Update()
                    End If
                End If
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 50000 Then
                    MSGBoxCtrl.show("Alert !", ex.Message, "", MsgBoxStyle.OkOnly, "")
                End If
            End Try
        End If
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mEmployeeTraining.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mEmployeeTraining.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mEmployeeTraining.ID)
        End If
        Session("mFileAttach") = mFileAttach
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        '----------------------------------------------------------------------
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString
        '----------------------------------------------------------------------
        If mEmployeeTraining.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mEmployeeTraining.ID)
            Session("mFileAttach") = mFileAttach
        End If
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
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            End If
        End If
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte

        If mEmployeeTraining.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mEmployeeTraining.ID)
            Session("mFileAttach") = mFileAttach
        End If

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False

        IsAttachmentDeleted = True
        mEmployeeTraining.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
        Session("mEmployeeTraining") = mEmployeeTraining
    End Sub
    Private Sub imgTraining_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgTraining.Click
        SetObject() 'Added Code
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenTrainingMasterWindow", "OpenTrainingMasterWindow()", True)
        'Response.Redirect("wfTraining.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=wfEmployeeTraining_Ajax.aspx" & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal"))
    End Sub
    Private Sub imgTrainingOrgName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles imgTrainingOrgName.Click
        SetObject() 'Added Code
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenTrainingOrgMasterWindow", "OpenTrainingOrgMasterWindow()", True)
        'Response.Redirect("wfTrainingOrg.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage2=wfEmployeeTraining_Ajax.aspx" & "&IsFromRenewal=" & Request.QueryString("IsFromRenewal"))
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        SetSession()
        Session.Remove("IsRenew") 'Added By Vikrant on 11-Oct-2012 For ALL11102012
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        'Response.Redirect("wfEmployeeDetails.aspx")
        If Not mEmployeeTraining.IsNew Then
            MarkLog(Flypal.Util.Action.Close, "Employee Training", "Emp : " + mEmployee.EmpNoName + " Training : " + mEmployeeTraining.TrainingName, Flypal.Util.ErrorType.NoError, mEmployeeTraining.ID, EventLogID)
        End If
        'Added by Vikrant on 28-nov-2013 for popup
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        'End
        If Request.QueryString("ChildPage1") = "wfEmployeeDetails_Ajax.aspx" Then
            Response.Redirect(Request.QueryString("ChildPage1") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage"))
        Else
            Response.Redirect(Request.QueryString("BackPage") & "?ChildPage1=" & Request.QueryString("ChildPage1") & "&ChildPage=" & Request.QueryString("ChildPage"))
        End If
    End Sub
  
    Private Sub txtDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDate.TextChanged
        If IsDate(txtDate.Text) Or (txtDate.Text = "") Then
            If txtDate.Text = "" Then
                mEmployeeTraining.Date = System.DBNull.Value
                txtDate.Text = mEmployeeTraining.EmployeeTrainingDate.ToString
            Else
                mEmployeeTraining.Date = txtDate.Text
                txtDate.Text = mEmployeeTraining.EmployeeTrainingDate.ToString
            End If
        Else
            txtDate.Text = ""
        End If
    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        mEmployeeTraining.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
        upnlTrainingDetails.Update()
        Session("mEmployeeTraining") = mEmployeeTraining
    End Sub
    Private Sub MsgBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region

    Private Sub hdnimgbtnTrainingMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgbtnTrainingMaster.Click
        mTrainingList = TrainingList.GetTrainingList(, , , "<SELECT>")
        cmbTrainingList.DataSource = mTrainingList
        cmbTrainingList.DataBind()
        Session("mTrainingList") = mTrainingList
        upnlTrainingDetails.Update()
    End Sub

    Private Sub hdnimgbtnTrainingOrgMaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnimgbtnTrainingOrgMaster.Click
        mTrainingOrgList = TrainingOrgList.GetTrainingOrgList(, , , "<SELECT>")
        cmbTrainingOrgList.DataSource = mTrainingOrgList
        cmbTrainingOrgList.DataBind()
        Session("mTrainingOrgList") = mTrainingOrgList
        upnlTrainingDetails.Update()
    End Sub

    Private Sub cmbTrainingList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTrainingList.SelectedIndexChanged
        If cmbTrainingList.SelectedIndex > 0 Then
            mtmpTraining = Training.GetTraining(New Guid(cmbTrainingList.SelectedValue.ToString))
            chkRecurringStatus.Checked = mtmpTraining.RecurringStatus
            txtFreqInMonths.Text = mtmpTraining.FreqInMonths
            txtWarningDays.Text = mtmpTraining.WarningDays

            upnlTrainingDetails.Update()

            mEmployeeTraining.RecurringStatus = chkRecurringStatus.Checked
            If txtFreqInMonths.Text = "" Then
                mEmployeeTraining.FreqInMonths = 0
            Else
                mEmployeeTraining.FreqInMonths = Trim(txtFreqInMonths.Text)
            End If
            If txtWarningDays.Text = "" Then
                mEmployeeTraining.WarningDays = 0
            Else
                mEmployeeTraining.WarningDays = Trim(txtWarningDays.Text)
            End If
            Session("mEmployeeTraining") = mEmployeeTraining
        Else
            chkRecurringStatus.Checked = False
            txtFreqInMonths.Text = "0"
            txtWarningDays.Text = "0"

            upnlTrainingDetails.Update()
        End If
    End Sub

  
End Class
