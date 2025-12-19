'***************************
'AJAX Created By     :   Saylee
'Dated               :   27-Aug-2015
'***************************

Imports System.Text

Imports Flypal.DashBoardWO

Public Class wfAuditExecution_AJAX
    Inherits Page

#Region " Variable Declaration "

    Protected mAuditExecution As AuditExecution
    Protected mAuditExecutionList As AuditExecutionList

    Protected mAuditTypeList As AuditTypeList
    Protected mAuditorList As AuditorList
    Protected mDesignationList As DesignationList
    Protected mAuditStatusList As AuditStatusList
    Public mTaskStatusList As TaskStatusList
    Private Flag As Int16
    Dim EventLogID As Guid          'Added by Vikrant on 25-July-2011
    Dim mExecutionDetail As String

    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
    Dim OpenFrom As Integer 'Ajay 27-06-2023

#End Region

#Region " Helper Methods "

    Private Sub GetSession()

        mAuditExecution = Session("mAuditExecution")
        mAuditExecutionList = Session("mAuditExecutionList")
        mAuditTypeList = Session("mAuditTypeList")
        mAuditorList = Session("mAuditorList")
        mDesignationList = Session("mDesignationList")
        mAuditStatusList = Session("mAuditStatusList")
        'mFileAttach = Session("mFileAttach")
        mFileAttach = Session("mFileAttachOnAuditExecution")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        mTaskStatusList = Session("mTaskStatusList")
        mModuleList = Session("mModuleList")       'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        OpenFrom = Session("OpenFrom") 'Ajay 27-06-2023

    End Sub

    Private Sub SetSession()

        Session("mAuditExecution") = mAuditExecution
        Session("mAuditExecutionList") = mAuditExecutionList
        Session("mAuditTypeList") = mAuditTypeList
        Session("mAuditorList") = mAuditorList
        Session("mDesignationList") = mDesignationList
        Session("mAuditStatusList") = mAuditStatusList
        Session("mFileAttach") = mFileAttach
        Session("IsAttachmentDeleted") = IsAttachmentDeleted

    End Sub

    Private Sub SetPage()

        If Not mAuditExecution.IsNew Then
            lblTitle.Text = "Audit Compliance &nbsp;&nbsp;[ " + CType(mAuditExecution.AuditNo, String) + " ]"
        Else
            lblTitle.Text = "Audit Compliance [New]"
        End If

    End Sub

    Private Overloads Sub SetFocus(control As WebControl)

        If control.Enabled = False Or control.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + control.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript([GetType], "focusscript", str)

    End Sub

    Private Sub ContolVisibility()

        If AppSettings("ClientCode") = "KAS" Then
            btnPrint.Visible = True
        Else
            btnPrint.Visible = False
        End If

        If mAuditExecution.IsAttachmentAdded Then

            ImageButton1.Visible = True
            If mAuditExecution.AuditStatusID = 2 And Not mAuditExecution.EndDate Is DBNull.Value Then
                btnDelAttach.Enabled = False
            Else
                btnDelAttach.Enabled = True
            End If

        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If

        'Added by ajay 10-Nov-2022
        dgAuditExecutionTask.Columns(12).Visible = Not (mAuditExecution.AuditStatusID = 2)
        dgAuditExecutionTask.Columns(11).Visible = Not (mAuditExecution.AuditStatusID = 2)
        txtEndDate.Enabled = Not (mAuditExecution.AuditStatusID = 2)
        cmbAuditStatusList.Enabled = Not (mAuditExecution.AuditStatusID = 2)
        btnAddExecutionTask.Enabled = Not (mAuditExecution.AuditStatusID = 2)
        cmbAuditTypeList.Enabled = Not (mAuditExecution.AuditStatusID = 2)
        txtAuditIncharge.Enabled = Not (mAuditExecution.AuditStatusID = 2)
        cmbAuditorList.Enabled = Not (mAuditExecution.AuditStatusID = 2)
        txtlEntityManager.Enabled = Not (mAuditExecution.AuditStatusID = 2)
        txtOtherInformation.Enabled = Not (mAuditExecution.AuditStatusID = 2)
        txtNote.Enabled = Not (mAuditExecution.AuditStatusID = 2)
        txtOtherParticipants.Enabled = Not (mAuditExecution.AuditStatusID = 2)
        txtAuditors.Enabled = Not (mAuditExecution.AuditStatusID = 2)
        cmbDesignationList.Enabled = Not (mAuditExecution.AuditStatusID = 2)
        btnSave.Enabled = Not (mAuditExecution.AuditStatusID = 2)
        btnSelectFile.Disabled = (mAuditExecution.AuditStatusID = 2)
        imgbtnAuditor.Enabled = Not (mAuditExecution.AuditStatusID = 2)
        imgbtnDesignation.Enabled = Not (mAuditExecution.AuditStatusID = 2)

        'Added Ajay 15-Nov-2022
        Dim cmbTaskStatus As DropDownList
        Dim txtKindAttention, txtComplianceDetails As TextBox

        For i As Integer = 0 To dgAuditExecutionTask.Rows.Count - 1

            cmbTaskStatus = CType(Me.dgAuditExecutionTask.Rows(i).FindControl("cmbTaskStatus"), DropDownList)
            txtKindAttention = CType(Me.dgAuditExecutionTask.Rows(i).FindControl("txtKindAttention"), TextBox)
            txtComplianceDetails = CType(Me.dgAuditExecutionTask.Rows(i).FindControl("txtComplianceDetails"), TextBox)
            cmbTaskStatus.Enabled = Not (mAuditExecution.AuditStatusID = 2)
            txtKindAttention.Enabled = Not (mAuditExecution.AuditStatusID = 2)
            txtComplianceDetails.Enabled = Not (mAuditExecution.AuditStatusID = 2)

        Next
        '--------------------------------

    End Sub

    Private Sub SetObject()

        Try
            mAuditExecution.AuditNo = Trim(txtAuditNo.Text)
            mAuditExecution.StartDate = txtStartDate.Text

            If txtEndDate.Text.ToString <> "" Then
                mAuditExecution.EndDate = txtEndDate.Text
            Else
                mAuditExecution.EndDate = DBNull.Value
            End If

            mAuditExecution.Reference = Trim(txtReferenceNo.Text)
            mAuditExecution.Description = Trim(txtDescription.Text)
            mAuditExecution.Note = Trim(txtNote.Text)
            mAuditExecution.AuditorID = New Guid(cmbAuditorList.SelectedValue)
            mAuditExecution.DesignationID = New Guid(cmbDesignationList.SelectedValue)
            mAuditExecution.AuditStatusID = cmbAuditStatusList.SelectedValue
            mAuditExecution.AuditIncharge = Trim(txtAuditIncharge.Text)
            mAuditExecution.Auditors = Trim(txtAuditors.Text)
            mAuditExecution.EntityManager = Trim(txtlEntityManager.Text)
            mAuditExecution.OtherParticipants = Trim(txtOtherParticipants.Text)
            mAuditExecution.OtherInformation = Trim(txtOtherInformation.Text)

            If Not mFileAttach Is Nothing Then

                If mFileAttach.Size > 0 Then
                    mAuditExecution.IsAttachmentAdded = True
                Else
                    mAuditExecution.IsAttachmentAdded = False
                End If

            End If

            Session("mAuditExecution") = mAuditExecution

        Catch ex As Exception

        End Try

    End Sub

    Private Sub SetChildObject()

        Dim i As Integer
        Dim cmbTaskStatus As DropDownList
        Dim txtKindAttention, txtComplianceDetails As TextBox

        For i = 0 To Me.dgAuditExecutionTask.Rows.Count - 1

            cmbTaskStatus = CType(Me.dgAuditExecutionTask.Rows(i).FindControl("cmbTaskStatus"), DropDownList)
            txtKindAttention = CType(Me.dgAuditExecutionTask.Rows(i).FindControl("txtKindAttention"), TextBox)
            txtComplianceDetails = CType(Me.dgAuditExecutionTask.Rows(i).FindControl("txtComplianceDetails"), TextBox)
            mAuditExecution.AuditExecutionTasks(i).KindAttention = Trim(txtKindAttention.Text)
            mAuditExecution.AuditExecutionTasks(i).TaskStatusID = cmbTaskStatus.SelectedValue
            mAuditExecution.AuditExecutionTasks(i).ComplianceDetails = Trim(txtComplianceDetails.Text)

        Next

        Session("mAuditExecution") = mAuditExecution

    End Sub

    Private Sub DeleteAuditExecutionTask(index As Int32)

        MSGBoxCtrl.show(MSGBox.Message_title.Remove,
                        MSGBox.Message_text.Remove,
                        "",
                        MsgBoxStyle.YesNo,
                        "DeleteAuditExecutionTask")

        mAuditExecution.AuditExecutionTasks.CurrentIndex = index
        Session("mAuditExecution") = mAuditExecution

    End Sub

    Private Sub MessageBoxResult()

        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim msgCount As Integer = 0

        If Result1 > 0 Then

            Select Case Result1
                Case MsgBoxResult.Yes

                    If MSGBoxCtrl.Sender = "DeleteAuditExecutionTask" Then

                        Try

                            mAuditExecution = CType(Session("mAuditExecution"), AuditExecution)
                            mAuditExecution.AuditExecutionTasks.Remove(mAuditExecution.AuditExecutionTasks.CurrentItem)
                            Session("mAuditExecution") = mAuditExecution
                            DataFieldBind()
                            ContolVisibility()
                            SetPage()
                            upnlAuditDet.Update()
                            upnlGrid.Update()
                            upnlTitle.Update()

                        Catch ex As SqlException

                            If ex.Number = 8145 Then

                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                MSGBox.Message_text.ProcedureError,
                                ex.Procedure,
                                MsgBoxStyle.OkOnly,
                                "")

                            ElseIf ex.Number = 2601 Or ex.Number = 2627 Then

                                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                MSGBox.Message_text.Duplicate,
                                ex.Procedure,
                                MsgBoxStyle.OkOnly,
                                "")

                            ElseIf ex.Number = 547 Then

                                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete,
                                MSGBox.Message_text.ReferenceDelete,
                                ex.Procedure + "," + ex.Message,
                                MsgBoxStyle.OkOnly,
                                "")

                            End If

                        End Try

                    ElseIf MSGBoxCtrl.Sender = "Close" Then  '' Close confirmation

                        If Session("IsValid") Then

                            Session.Remove("IsValid")
                            DataFieldBind()
                            If (Not User.IsInRole("AuditExecutionNew") And Not User.IsInRole("AuditExecutionEdit")) Then
                                MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                                Exit Sub
                            End If

                            If Save() Then

                                mAuditExecution = Session("mAuditExecution")
                                SetObject()
                                SaveAttachment() 'Added by sachin for saving Attechment on click on yes of Cancel Button 
                                Session("mAuditExecution") = mAuditExecution
                                Session.Remove("mAuditExecution")

                                'Session.Remove("mFileAttach")
                                Session.Remove("mFileAttachOnAuditExecution")
                                mFileAttach = Nothing

                                Response.Redirect(Request.QueryString("BackPage"))
                            End If

                        Else
                            Session.Remove("IsValid")
                        End If

                    ElseIf MSGBoxCtrl.Sender = "Re-Schedule" Then

                        Session("mAuditExecution") = mAuditExecution
                        ScriptManager.RegisterStartupScript(Me,
                                                            [GetType],
                                                            "OpenAuditorReScheduleWindow",
                                                            "OpenAuditorReScheduleWindow()",
                                                            True)

                    End If

                Case MsgBoxResult.No

                    If MSGBoxCtrl.Sender = "Close" Then

                        Session.Remove("IsValid")
                        If mAuditExecution.IsNew Then Session.Remove("mAuditExecution")
                        mAuditExecution = Session("mAuditExecution")
                        SetObject()
                        Session("mAuditExecution") = mAuditExecution
                        Session.Remove("mAuditExecution")
                        Response.Redirect(Request.QueryString("BackPage"))

                    ElseIf MSGBoxCtrl.Sender = "Re-Schedule" Then
                        DataFieldBind()
                    End If

                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    DataFieldBind()
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    DataFieldBind()
            End Select

        ElseIf Result1 = -1 Then
            DataFieldBind()
        ElseIf Result1 = 0 And Session("sender") = "Authorization" Then   'Code Added
            DataFieldBind()
        End If

    End Sub

    Private Sub GetAttachment()

        If mAuditExecution.IsAttachmentAdded = True And mFileAttach Is Nothing Then

            mFileAttach = FileAttach.GetAttachment(mAuditExecution.ID)
            Session("mFileAttachOnAuditExecution") = mFileAttach

        End If
    End Sub

    Private Sub SaveAttachment() '

        If mFileAttach Is Nothing And mAuditExecution.IsAttachmentAdded = True Then

            mFileAttach = FileAttach.GetAttachment(mAuditExecution.ID)
            Session("mFileAttachOnAuditExecution") = mFileAttach

        End If


        If Not mFileAttach Is Nothing Then
            mFileAttach.ReferenceID = mAuditExecution.ID

            If mFileAttach.Size > 0 Then

                Try
                    mFileAttach.Save()
                Catch ex As Exception
                    ScriptManager.RegisterClientScriptBlock(Me,
                                                            [GetType],
                                                            "",
                                                            MessageBox.Show(ex.InnerException.ToString, False),
                                                            True)
                End Try

            Else

                If (Not mAuditExecution.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mAuditExecution.ID)
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
                File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                ' Create the file.
                fs = File.Create(path)
                '' Add some information to the file.
                fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                fs.Close()
                Session("DOCPath") = path
                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "openFile",
                                                    "openFile();",
                                                    True)

            End If

        End If

    End Sub

    Private Function Save() As Boolean

        Try

            mAuditExecution = Session("mAuditExecution")
            SetObject()
            Dim AuditExecutionClone As AuditExecution
            AuditExecutionClone = mAuditExecution.Clone

            If Not CustomValidate1() Then upnlValidation.Update() : Exit Function
            If Not mAuditExecution.AuditExecutionTasks.Count = 0 Then

                If mAuditExecution.IsValid Then

                    mAuditExecution = mAuditExecution.Save()
                    Session("mAuditExecution") = mAuditExecution
                    Session("mAuditExecutionList") = mAuditExecutionList
                    SetPage()
                    dgAuditExecutionTask.DataSource = mAuditExecution.AuditExecutionTasks
                    DataBind()
                    Return True

                Else
                    upnlValidation.Update()
                End If

            Else

                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert,
                                MSGBox.Message_text.saveAlert,
                                "Audit Compliance can not be saved without Tasks.",
                                MsgBoxStyle.OkOnly,
                                "")

                mAuditExecution = AuditExecutionClone
                SetObject()
                Session("mAuditExecution") = mAuditExecution
                DataFieldBind()

            End If

        Catch ex As SqlException

            If ex.Number = 8145 Then

                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                MSGBox.Message_text.ProcedureError,
                                ex.Procedure,
                                MsgBoxStyle.OkOnly,
                                "")

            ElseIf ex.Number = 2601 Or ex.Number = 2627 Then

                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                MSGBox.Message_text.Duplicate,
                                ex.Procedure,
                                MsgBoxStyle.OkOnly,
                                "")

            ElseIf ex.Number = 547 Then

                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete,
                                MSGBox.Message_text.ReferenceDelete,
                                ex.Procedure + "," + ex.Message,
                                MsgBoxStyle.OkOnly,
                                "")

            End If

        End Try

    End Function

    Private Sub SendMailCloser()

        If (cmbAuditStatusList.SelectedValue = 2) Then

            Dim SmtpHost, SmtpUser, SmtpPassword As String
            Dim SmtpPort As Integer = 0
            Dim str As New StringBuilder

            If OpenFrom = 1 Then

                SmtpHost = mModuleList.Item("FindingRectification").SmtpHost
                SmtpPort = mModuleList.Item("FindingRectification").SmtpPort
                SmtpUser = mModuleList.Item("FindingRectification").SmtpUser
                SmtpPassword = mModuleList.Item("FindingRectification").SmtpPassword

            ElseIf OpenFrom = 2 Then

                SmtpHost = mModuleList.Item("AuditClosing").SmtpHost
                SmtpPort = mModuleList.Item("AuditClosing").SmtpPort
                SmtpUser = mModuleList.Item("AuditClosing").SmtpUser
                SmtpPassword = mModuleList.Item("AuditClosing").SmtpPassword

            End If

            Try

                str.Append("<p>The following audit has been closed,no more findings are open:</p>")
                str.Append("<p><b>Audit No: </b> " & mAuditExecution.AuditNo & "</p>")
                str.Append("<p><b>Audited On: </b> " & mAuditExecution.AuditOnName & "-" & mAuditExecution.AuditOnNameDetail & "</p>")
                str.Append("<p><b>Audit Type: </b> " & cmbAuditTypeList.SelectedItem.Text & "</p>")
                str.Append("<p><b>Audit Scope:  </b> " & mAuditExecution.Description & "</p>")

                If (txtToMailID.Text = "" And txtCCMailID.Text = "") Then

                Else
                    SendMailFile.SendMailFile(Session("CrystalReport"),
                                              User.Identity.Name,
                                              "Audit Closed",
                                              mAuditExecution.AuditNo.Replace("/", "").Replace("\", ""),
                                              Info:=str.ToString,
                                              VendorEmailID:="",
                                              ToMailID:=txtToMailID.Text.Trim,
                                              CCMailID:=txtCCMailID.Text.Trim,
                                              FromAudit:=1,
                                              Remark:=Session("SendMailRemark"),
                                              ReportGeneratedBy:=Session("ReportGenratedBy"),
                                              SmtpHost:=SmtpHost,
                                              SmtpPort:=SmtpPort,
                                              SmtpUser:=SmtpUser,
                                              SmtpPassword:=SmtpPassword)

                    ScriptManager.RegisterStartupScript(Me,
                                                        [GetType],
                                                        "openTransDetail",
                                                        MessageBox.Show("Mail Sent Successfully", False),
                                                        True)
                End If

            Catch ex As Exception

                Dim Title As String = "Error Sending Mail"
                Dim Message As String = ex.InnerException.ToString

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "OpenScript",
                                                    MessageBox.Show(Title, Message, , False),
                                                    True)

                Exit Sub

            End Try

        End If

    End Sub

    Private Function IsInRole(CheckFor As Rights) As Boolean

        Dim IsInRoleString As String = "AuditExecution"

        Select Case CheckFor
            Case Rights.View
                Return User.IsInRole(IsInRoleString + "View")
            Case Rights.[New]
                Return User.IsInRole(IsInRoleString + "New")
            Case Rights.Edit
                Return User.IsInRole(IsInRoleString + "Edit")
            Case Rights.Save
                Return (User.IsInRole(IsInRoleString + "New") Or User.IsInRole(IsInRoleString + "Edit"))
            Case Rights.Delete
                Return User.IsInRole(IsInRoleString + "Delete")
            Case Rights.Print
                Return User.IsInRole(IsInRoleString + "Print")
            Case Rights.Authorized
                Return User.IsInRole(IsInRoleString + "Authorized")
        End Select

    End Function

#End Region

#Region " Data Binding "

    Private Sub DataFieldBind()

        mAuditExecution = Session("mAuditExecution")

        mAuditTypeList = AuditTypeList.GetAuditTypeList("(SELECT)")
        cmbAuditTypeList.DataSource = mAuditTypeList
        Session("mAuditTypeList") = mAuditTypeList

        mAuditorList = AuditorList.GetAuditorList("(SELECT)")
        cmbAuditorList.DataSource = mAuditorList
        Session("mAuditorList") = mAuditorList

        mAuditStatusList = AuditStatusList.GetAuditStatusList()
        cmbAuditStatusList.DataSource = mAuditStatusList
        Session("mAuditStatusList") = mAuditStatusList

        mDesignationList = DesignationList.GetDesignationList(, "(SELECT)")
        cmbDesignationList.DataSource = mDesignationList
        Session("mDesignationList") = mDesignationList

        txtStartDate.Text = mAuditExecution.StartDateFormatted.ToString
        txtEndDate.Text = mAuditExecution.EndDateFormatted.ToString
        dgAuditExecutionTask.DataSource = mAuditExecution.AuditExecutionTasks

        If Not mAuditorList.Contains(mAuditExecution.AuditorID) Then
            mAuditExecution.AuditorID = Guid.Empty
        End If

        If Not mDesignationList.Contains(mAuditExecution.DesignationID) Then
            mAuditExecution.DesignationID = Guid.Empty
        End If
        If AppSettings("ClientCode") = "SAA" Then 'Added By Prashant on 28-Jun-2022
            mTaskStatusList = TaskStatusList.GetTaskStatusList() ''As Client need Satisfactory as defualt selected
        Else
            mTaskStatusList = TaskStatusList.GetTaskStatusList("(SELECT)")
        End If

        Session("mTaskStatusList") = mTaskStatusList

        DataBind()

    End Sub

    Public Sub CustomValidate(s As Object, e As ServerValidateEventArgs)

        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)

        If custValidator.ControlToValidate = "cmbAuditorList" Then

            If cmbAuditorList.SelectedIndex <= 0 Then
                custValidator.ErrorMessage = "Please Select the Lead Auditor"
                e.IsValid = False
            Else
                e.IsValid = True
            End If

        ElseIf custValidator.ControlToValidate = "txtStartDate" Then

            If Len(txtStartDate.Text) = 0 Then
                custValidator.ErrorMessage = "Please Select Start Date"
                e.IsValid = False
            Else
                e.IsValid = True
            End If

        ElseIf custValidator.ControlToValidate = "txtEndDate" Then

            If Len(txtStartDate.Text) <> 0 And Len(txtEndDate.Text) <> 0 Then

                If CDate(txtEndDate.Text) < CDate(txtStartDate.Text) Then
                    custValidator.ErrorMessage = "End Date should be greater or equal to Start date"
                    e.IsValid = False
                ElseIf (Len(txtEndDate.Text) = 0) And (cmbAuditStatusList.SelectedValue = 2) Then
                    custValidator.ErrorMessage = "Please Select End Date"
                    e.IsValid = False
                ElseIf (Len(txtEndDate.Text) <> 0) And (cmbAuditStatusList.SelectedValue <> 2) Then
                    custValidator.ErrorMessage = "Please Close the Audit by selecting Audit status"
                    e.IsValid = False
                Else

                    e.IsValid = True
                End If

            End If

        ElseIf custValidator.ControlToValidate = "cmbAuditStatusList" Then

            If (Len(txtEndDate.Text) = 0) And (cmbAuditStatusList.SelectedValue = 2) Then
                custValidator.ErrorMessage = "Please Select End Date"
                e.IsValid = False
            Else
                e.IsValid = True
            End If

        ElseIf custValidator.ControlToValidate = "txtAuditIncharge" Then

            If Len(txtAuditIncharge.Text) > 100 Then
                custValidator.ErrorMessage = "Audit Incharge should not be greater than 100 characters."
                e.IsValid = False
            Else
                e.IsValid = True
            End If

        ElseIf custValidator.ControlToValidate = "txtDescription" Then

            If Len(txtDescription.Text) > 5000 Then
                custValidator.ErrorMessage = "Description should not be greater than 5000 characters."
                e.IsValid = False
            Else
                e.IsValid = True
            End If

        End If

    End Sub

    Private Function CustomValidate1() As Boolean

        SetObject()
        Dim strMSG As String = ""

        If Not mAuditExecution.IsValid Then

            If Len(txtEndDate.Text) <> 0 And (cmbAuditStatusList.SelectedValue = 2) And
               mAuditExecution.AuditExecutionTasks.IsNotSatisfactory = True Then

                For i As Integer = 0 To mAuditExecution.GetBrokenRulesCollection.Count - 1
                    strMSG = strMSG + mAuditExecution.GetBrokenRulesCollection(i).Description + "<Br>"
                Next

            End If

            If mAuditExecution.GetChildBrokenRulesCollection() <> "" Then

                strMSG = "Execution Tasks: " + "<BR>"
                strMSG = strMSG + mAuditExecution.GetChildBrokenRulesCollection()

            End If

        End If

        If strMSG.Trim <> "" Then

            cvDescription.ErrorMessage = strMSG
            cvDescription.IsValid = False
            Return False

        End If

        Return True

    End Function

    Private Sub SendMail(AuditExecutionTaskID As Guid, Index As Integer,
                         Optional AuditExecutionID As String = "00000000-0000-0000-0000-000000000000")

        Dim myReport As Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim da As New ObjectAdapter
        Dim mrptAuditFindings As rptAuditFindings
        Dim dsrptAuditFindings As New dsrptAuditFindings

        If AppSettings("ClientCode") = "Heligo" Then
            myReport = New crFindingReportForHeligo
        Else
            myReport = New crFindingReport
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
                                     "Audit Findings Report",
                                     SearchStr1:=AppSettings("FormnNoInAudit"),
                                     SearchStr2:=AppSettings("IssueNoInAudit"),
                                     SearchStr3:=AppSettings("RevisionNoInAudit"),
                                     SearchStr4:="",
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

        If Guid.Empty.Equals(New Guid(AuditExecutionID)) Then

            mrptAuditFindings = rptAuditFindings.GetrptAuditFindings("1/1/1900",
                                                                     "1/1/2100",
                                                                     mAuditExecution.AuditNo, , ,
                                                                     mAuditExecution.AuditExecutionTasks(AuditExecutionTaskID).AuditExecutionTaskFindings(Index).ID.ToString,
                                                                     UsedFromFindingEntry:=1)

        Else

            mrptAuditFindings = rptAuditFindings.GetrptAuditFindings("1/1/1900",
                                                                     "1/1/2100",
                                                                     mAuditExecution.AuditNo, , , ,
                                                                     UsedFromFindingEntry:=1,
                                                                     AuditExecutionID:=AuditExecutionID)
        End If

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
        Dim str As New StringBuilder

        Try

            If Guid.Empty.Equals(New Guid(AuditExecutionID)) Then

                str.Append("Finding Details are as follows: ")
                str.Append("<p><b>Audit No.: </b> " & mrptAuditFindings(mAuditExecution.AuditExecutionTasks(AuditExecutionTaskID).AuditExecutionTaskFindings(Index).ID).AuditNo & "</p>")
                str.Append("<p><b>Task Category: </b> " & mrptAuditFindings(mAuditExecution.AuditExecutionTasks(AuditExecutionTaskID).AuditExecutionTaskFindings(Index).ID).AuditCategoryName & "</p>")
                str.Append("<p><b>Task Description: </b> " & mrptAuditFindings(mAuditExecution.AuditExecutionTasks(AuditExecutionTaskID).AuditExecutionTaskFindings(Index).ID).Description & "</p>")

                SendMailFile.SendMailFile(Session("CrystalReport"),
                                          User.Identity.Name, "Finding Details",
                                          "AuditFindings",
                                          Info:=str.ToString,
                                          VendorEmailID:="",
                                          ToMailID:=mrptAuditFindings(mAuditExecution.AuditExecutionTasks(AuditExecutionTaskID).AuditExecutionTaskFindings(Index).ID).ToMailID,
                                          CCMailID:=mrptAuditFindings(mAuditExecution.AuditExecutionTasks(AuditExecutionTaskID).AuditExecutionTaskFindings(Index).ID).CCMailID,
                                          FromAudit:=1,
                                          Remark:=Session("SendMailRemark"),
                                          ReportGeneratedBy:=Session("ReportGenratedBy"),
                                          SmtpHost:=mModuleList.Item("AuditExecution").SmtpHost,
                                          SmtpPort:=mModuleList.Item("AuditExecution").SmtpPort,
                                          SmtpUser:=mModuleList.Item("AuditExecution").SmtpUser,
                                          SmtpPassword:=mModuleList.Item("AuditExecution").SmtpPassword)

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "openTransDetail",
                                                    MessageBox.Show("Mail Sent Successfully", False),
                                                    True)

            Else

                str.Append("Finding Details of following Audit are enclosed in mail attachment: ")
                str.Append("<p><b>Audit No.: </b> " & mAuditExecution.AuditNo & "</p>")

                SendMailFile.SendMailFile(Session("CrystalReport"),
                                          User.Identity.Name,
                                          "Audit Finding Details",
                                          "AuditFindings",
                                          Info:=str.ToString,
                                          VendorEmailID:="",
                                          ToMailID:=txtToMailID.Text.Trim,
                                          CCMailID:=txtCCMailID.Text.Trim,
                                          FromAudit:=1,
                                          Remark:=Session("SendMailRemark"),
                                          ReportGeneratedBy:=Session("ReportGenratedBy"))

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "openTransDetail",
                                                    MessageBox.Show("Mail Sent Successfully", False),
                                                    True)

            End If

        Catch ex As Exception

            Dim Title As String = "Error Sending Mail"
            Dim Message As String = ex.InnerException.ToString

            ScriptManager.RegisterStartupScript(Me,
                                                [GetType],
                                                "OpenScript",
                                                MessageBox.Show(Title, Message, , False),
                                                True)

            Exit Sub

        End Try

    End Sub

#End Region

#Region " Events "

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        GetSession()
        txtStartDate.ReadOnly = True
        EventLogID = CType(Session("EventLogID"), Guid)          'Added by Vikrant on 25-July-2011

        If Not Page.IsPostBack Then

            SetFocus(txtAuditIncharge)
            If mAuditExecution.IsNew Then
                txtStartDate.Text = New SmartDate(Today.Date.ToString).FormattedText
            End If
            DataFieldBind()
            ContolVisibility()
            SetPage()

        End If

    End Sub

    Private Sub Save_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        If (Not User.IsInRole("AuditExecutionNew") And Not User.IsInRole("AuditExecutionEdit")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        If Not IsValid Then upnlValidation.Update() : Exit Sub

        Try

            mAuditExecution = Session("mAuditExecution")
            SetObject()
            SetChildObject()
            Dim AuditExecutionClone As AuditExecution
            AuditExecutionClone = mAuditExecution.Clone

            If Not CustomValidate1() Then upnlValidation.Update() : Exit Sub

            If Not mAuditExecution.AuditExecutionTasks.Count = 0 Then

                If mAuditExecution.IsValid Then

                    mAuditExecution = mAuditExecution.Save()
                    SaveAttachment()
                    'Changed by Vikrant on 25-July-2011
                    mExecutionDetail = "Audit No: " + mAuditExecution.AuditNo + " Start Date: " + mAuditExecution.StartDateFormatted + " Lead Auditor: " + cmbAuditorList.SelectedItem.Text
                    MarkLog(Action.Save, "Audit Compliance", mExecutionDetail, ErrorType.NoError, mAuditExecution.ID, EventLogID)
                    Session("mAuditExecution") = mAuditExecution
                    Session("mAuditExecutionList") = mAuditExecutionList
                    SetPage()
                    dgAuditExecutionTask.DataSource = mAuditExecution.AuditExecutionTasks
                    mTaskStatusList = Session("mTaskStatusList")
                    SendMailCloser() 'Ajay 28-06-2023
                    DataBind()
                    upnlTitle.Update()
                    Dim mScheduleList As AuditScheduleList = AuditScheduleList.GetAuditScheduleList(mAuditExecution.AuditScheduleDateFormatted, , , mAuditExecution.AuditID.ToString)

                    If mScheduleList.Count > 0 Then

                        If Not CDate(mScheduleList(0).ScheduleDate) > (CDate(mAuditExecution.AuditScheduleDateFormatted)) Then

                            If mAuditExecution.AuditStatusID = 2 And mAuditExecution.NextAuditSchedule = True Then

                                Session("mScheduleStartDate") = mAuditExecution.AuditScheduleDateFormatted
                                Session("mComplianceEndDate") = mAuditExecution.EndDateFormatted
                                MSGBoxCtrl.Show("Alert!!!", "This Audit is Scheduled for every " + mAuditExecution.Frequency.ToString + " months. ", "Do you want to Re-Schedule it for next " + mAuditExecution.Frequency.ToString + " months?", MsgBoxStyle.YesNo, "Re-Schedule")

                                Exit Sub

                            End If

                        End If

                    End If

                    MSGBoxCtrl.show(MSGBox.Message_title.SavedSuccessFully, MSGBox.Message_text.SavedSuccessFully, "", MsgBoxStyle.OkOnly, "")
                Else
                    upnlValidation.Update()
                End If

            Else

                MSGBoxCtrl.show(MSGBox.Message_title.SaveAlert, MSGBox.Message_text.saveAlert, "Audit Compliance can not be saved without Tasks.", MsgBoxStyle.OkOnly, "")
                mAuditExecution = AuditExecutionClone
                SetObject()
                Session("mAuditExecution") = mAuditExecution
                DataFieldBind()

            End If

        Catch ex As SqlException

            If ex.Number = 8145 Then

                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                MSGBox.Message_text.ProcedureError,
                                ex.Procedure,
                                MsgBoxStyle.OkOnly,
                                "")

            ElseIf ex.Number = 2601 Or ex.Number = 2627 Then

                MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError,
                                MSGBox.Message_text.Duplicate,
                                ex.Procedure,
                                MsgBoxStyle.OkOnly,
                                "")

            ElseIf ex.Number = 547 Then

                MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete,
                                MSGBox.Message_text.ReferenceDelete,
                                ex.Procedure + "," + ex.Message,
                                MsgBoxStyle.OkOnly,
                                "")

            End If

        End Try

    End Sub

    Private Sub AddExecutionTask(sender As Object, e As EventArgs) Handles btnAddExecutionTask.Click

        If (Not User.IsInRole("AuditExecutionNew") And Not User.IsInRole("AuditExecutionEdit")) Then

            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub

        End If

        SetObject()
        Session("Edit") = False
        Session("mAuditExecution") = mAuditExecution
        ScriptManager.RegisterStartupScript(Me, [GetType], "OpenTaskWindow", "OpenTaskWindow()", True)

    End Sub

    Private Sub HdnBtnTaskMaster_Click(sender As Object, e As EventArgs) Handles hdnimgBtnTaskMaster.Click

        mTaskStatusList = Session("mTaskStatusList")
        dgAuditExecutionTask.DataSource = mAuditExecution.AuditExecutionTasks
        dgAuditExecutionTask.DataBind()
        upnlGrid.Update()

    End Sub

    Public Sub GV_TaskFindings_RowCommand(sender As Object, e As GridViewCommandEventArgs)

        Dim AuditExecutionTaskID As Guid
        Dim AuditExecutionTaskFindingID As Guid
        Dim Idx As Integer

        Select Case e.CommandName
            Case "EditRecFinding"

                'Ajay h 03-10-2023
                'Ajay Added 03-10-2023
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                Idx = gvr.RowIndex
                AuditExecutionTaskID = New Guid(CType(sender, GridView).DataKeys(Idx).Item(1).ToString)
                '---------------
                mAuditExecution.AuditExecutionTasks.CurrentIndex = mAuditExecution.AuditExecutionTasks(AuditExecutionTaskID).SrNo - 1
                mAuditExecution.AuditExecutionTasks(AuditExecutionTaskID).AuditExecutionTaskFindings.CurrentIndex = Idx
                Dim AuditExecutionClone As AuditExecution
                AuditExecutionClone = mAuditExecution.Clone
                Session("mAuditExecution") = mAuditExecution
                Session("AuditExecutionClone") = AuditExecutionClone

                ScriptManager.RegisterStartupScript(Me, [GetType], "OpenFindingsWindow", "OpenFindingsWindow()", True)

            Case "RemoveRecFinding"

                'Ajay Added 03-10-2023
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                Idx = gvr.RowIndex
                AuditExecutionTaskFindingID = New Guid(CType(sender, GridView).DataKeys(Idx).Item(0).ToString) 'New Guid(CType(e.CommandSource, GridView).DataKeys(CInt(e.CommandArgument)).Values("ID").ToString)
                AuditExecutionTaskID = New Guid(CType(sender, GridView).DataKeys(Idx).Item(1).ToString)
                '---------------
                mAuditExecution.AuditExecutionTasks(AuditExecutionTaskID).AuditExecutionTaskFindings.CurrentIndex = Idx 'CInt(e.CommandArgument)

                If mAuditExecution.AuditExecutionTasks(AuditExecutionTaskID).AuditExecutionTaskFindings.CurrentItem.IsAttachmentAdded Then

                    mFileAttach = FileAttach.GetAttachment(mAuditExecution.AuditExecutionTasks(AuditExecutionTaskID).AuditExecutionTaskFindings.CurrentItem.ID)

                    If Not mFileAttach Is Nothing Then

                        If mFileAttach.Size > 0 Then
                            FileAttach.DeleteAttachment(mFileAttach.ID, mAuditExecution.AuditExecutionTasks(AuditExecutionTaskID).AuditExecutionTaskFindings.CurrentItem.ID)
                        End If

                    End If

                End If

                mAuditExecution.AuditExecutionTasks(AuditExecutionTaskID).AuditExecutionTaskFindings.Remove(AuditExecutionTaskFindingID, "")
                dgAuditExecutionTask.DataSource = mAuditExecution.AuditExecutionTasks
                dgAuditExecutionTask.DataBind()
                upnlGrid.Update()

            Case "ViewRecFinding"

                Dim No As New Random
                Dim mFileAttachmentForFinding As FileAttachments
                Dim AttachmentCount As Integer
                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                Idx = gvr.RowIndex
                AuditExecutionTaskID = New Guid(CType(sender, GridView).DataKeys(Idx).Item(1).ToString)
                Dim StrName As String = "abc" & No.Next.ToString
                mFileAttachmentForFinding = Nothing

                mAuditExecution.AuditExecutionTasks.CurrentIndex = mAuditExecution.AuditExecutionTasks(AuditExecutionTaskID).SrNo - 1
                mAuditExecution.AuditExecutionTasks(AuditExecutionTaskID).AuditExecutionTaskFindings.CurrentIndex = Idx

                mFileAttachmentForFinding = Nothing
                mFileAttachmentForFinding = FileAttachments.GetChildFileAttachments(mAuditExecution.AuditExecutionTasks(AuditExecutionTaskID).AuditExecutionTaskFindings.CurrentItem.ID)

                AttachmentCount = mFileAttachmentForFinding.Count

                If AttachmentCount > 1 Then 'Added By Harsh Sugandhi on 20th September 2024 for FLYPAL-1906 Provision for Multiple Attachment on Audit's Finding Detail page

                    Session("mFileAttachments") = mFileAttachmentForFinding
                    Session("TransactionNameMarkLog") = "Audit Compliance"
                    Session("TransactionName") = "Finding No & Status: "
                    Session("TransactionDetails") = mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.FindingNo +
                                                         "&nbsp;&nbsp;&nbsp;" + " [ " +
                                                         mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.CurrentItem.FindingStatusName +
                                                         " ]"

                    ScriptManager.RegisterStartupScript(Me,
                                                        [GetType],
                                                        "Open Attachment Window",
                                                        "OpenMultipleAttachmentWindow();",
                                                        True)

                Else

                    Dim mFileAttach As FileAttach

                    mFileAttach = Nothing

                    If mAuditExecution.AuditExecutionTasks(AuditExecutionTaskID).AuditExecutionTaskFindings.CurrentItem.IsAttachmentAdded And
                       mFileAttach Is Nothing Then

                        mFileAttach = FileAttach.GetAttachment(mAuditExecution.AuditExecutionTasks(AuditExecutionTaskID).AuditExecutionTaskFindings.CurrentItem.ID)
                        Session("mFileAttachmentForFinding") = mFileAttach

                    End If

                    Dim path As String = AppSettings("DOCPath") & StrName & mFileAttach.Extension
                    Dim fs As FileStream

                    If File.Exists(AppSettings("DOCPath")) = False Then

                        'Delete File if exist
                        File.Delete(AppSettings("DOCPath") & StrName & mFileAttach.Extension)
                        ' Create the file.
                        fs = File.Create(path)
                        '' Add some information to the file.
                        fs.Write(mFileAttach.ImageFile, 0, mFileAttach.ImageFile.Length)
                        fs.Close()
                        Session("DOCPath") = path
                        ScriptManager.RegisterStartupScript(Me, [GetType], "openFilel", "openFile();", True)

                    End If

                End If

            Case "SendMail"

                If User.Identity.Name.ToUpper = "BTPLADMIN" Or User.Identity.Name.ToUpper = "BYTZADMIN" Then ' BYTZADMIN For Deccan 'Added by Prashant 15-Oct-2019   
                    'Do nothing 
                Else

                    AuditExecutionTaskID = New Guid(CType(e.CommandSource, GridView).DataKeys(CInt(e.CommandArgument)).Values("AuditExecutionTaskID").ToString)
                    mAuditExecution.AuditExecutionTasks.CurrentIndex = mAuditExecution.AuditExecutionTasks(AuditExecutionTaskID).SrNo - 1
                    SendMail(AuditExecutionTaskID, CInt(e.CommandArgument))

                End If

        End Select

    End Sub

    Private Sub GV_AuditExecutionTask_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgAuditExecutionTask.RowCommand

        Select Case e.CommandName
            Case "EditRec"

                Dim Index As Int32 = e.CommandArgument.ToString + dgAuditExecutionTask.PageIndex * dgAuditExecutionTask.PageSize

                Session("Edit") = True
                SetObject()
                SetChildObject()
                mAuditExecution.AuditExecutionTasks.CurrentIndex = Index

                Dim AuditExecutionClone As AuditExecution
                AuditExecutionClone = mAuditExecution.Clone
                Session("mAuditExecution") = mAuditExecution
                Session("AuditExecutionClone") = AuditExecutionClone
                ScriptManager.RegisterStartupScript(Me, [GetType], "OpenAuditExecutionTaskWindow", "OpenAuditExecutionTaskWindow()", True)

            Case "RemoveRec"

                Dim gvr As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow) 'Ajay on 29-09-2023
                Dim Index As Int32 = gvr.RowIndex

                DeleteAuditExecutionTask(Index)

            Case "Findings" ', "EditRecFinding"

                Dim Index As Int32 = e.CommandArgument.ToString + dgAuditExecutionTask.PageIndex * dgAuditExecutionTask.PageSize

                Session("Edit") = True
                SetObject()
                mAuditExecution.AuditExecutionTasks.CurrentIndex = Index

                Dim AuditExecutionClone As AuditExecution
                AuditExecutionClone = mAuditExecution.Clone
                Session("mAuditExecution") = mAuditExecution
                Session("AuditExecutionClone") = AuditExecutionClone
                Session("AuditExecutionTaskFindingsNew") = True
                mAuditExecution.AuditExecutionTasks.CurrentItem.AuditExecutionTaskFindings.Add(mAuditExecution.AuditExecutionTasks.CurrentItem.ID)
                Session("mAuditExecution") = mAuditExecution
                Session("FindingEdit") = False
                ScriptManager.RegisterStartupScript(Me, [GetType], "OpenFindingsWindow", "OpenFindingsWindow()", True)

        End Select

    End Sub

    Private Sub RemoveAttachment(sender As Object, e As EventArgs) Handles btnDelAttach.Click

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

    Private Sub ViewAttachment(sender As Object, e As ImageClickEventArgs) Handles ImageButton1.Click

        ' mFileAttach = Nothing
        ViewImage()

    End Sub

    Private Sub BtnAuditor_Click(sender As Object, e As EventArgs) Handles imgbtnAuditor.Click

        SetObject()
        Session("mDesignationList") = mDesignationList
        ScriptManager.RegisterStartupScript(Me, [GetType], "OpenAuditorWindow", "OpenAuditorWindow()", True)

    End Sub

    Private Sub BtnDesignation_Click(sender As Object, e As EventArgs) Handles imgbtnDesignation.Click

        SetObject()
        ScriptManager.RegisterStartupScript(Me, [GetType], "OpenAuditDesignationWindow", "OpenAuditDesignationWindow()", True)

    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

    Private Sub SelectFile_ServerClick(sender As Object, e As EventArgs) Handles btnSelectFile.ServerClick

        If mAuditExecution.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mAuditExecution.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mAuditExecution.ID)
        End If
        Session("mFileAttach") = mFileAttach

    End Sub

    Private Sub HdnBtnFileUpload_Click(sender As Object, e As EventArgs) Handles hdnBtnFileUpload.Click

        mFileAttach = Session("mFileAttach")
        Session("mFileAttachOnAuditExecution") = Session("mFileAttach")
        mAuditExecution.IsAttachmentAdded = True
        ContolVisibility()
        upnlAttachment.Update()

    End Sub

    Private Sub Back_Click(sender As Object, e As EventArgs) Handles btnBack.Click

        SetObject()
        Session("IsValid") = IsValid

        If mAuditExecution.IsDirty Then
            MSGBoxCtrl.show(MSGBox.Message_title.CloseConfirm, MSGBox.Message_text.Save, "", MsgBoxStyle.YesNo, "Close")
        Else

            mAuditExecution = Session("mAuditExecution")
            SetObject()
            Session("mAuditExecution") = mAuditExecution
            Session.Remove("mAuditExecution")
            Session.Remove("mFileAttachOnAuditExecution")
            Response.Redirect(Request.QueryString("BackPage"))

        End If

    End Sub

    Private Sub GV_AuditExecutionTask_Sorting(sender As Object, e As GridViewSortEventArgs) Handles dgAuditExecutionTask.Sorting

        mAuditExecution.AuditExecutionTasks.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mAuditExecution") = mAuditExecution
        dgAuditExecutionTask.DataSource = mAuditExecution.AuditExecutionTasks
        dgAuditExecutionTask.DataBind()

    End Sub

    Private Sub HdnBtnAuditor_Click(sender As Object, e As EventArgs) Handles hdnimgBtnAuditor.Click

        mAuditorList = AuditorList.GetAuditorList("(SELECT)")
        cmbAuditorList.DataSource = mAuditorList
        Session("mAuditorList") = mAuditorList
        cmbAuditorList.DataBind()
        mDesignationList = Session("mDesignationList_Auditor")
        cmbDesignationList.DataSource = mDesignationList
        cmbDesignationList.DataBind()
        upnlAuditDet.Update()

        Session("mDesignationList") = Session("mDesignationList_Auditor")
        Session.Remove("mDesignationList_Auditor")

        upnlAuditDet.Update()
        upnlAuditDet.Update()

    End Sub

    Private Sub HdnBtnDesignation_Click(sender As Object, e As EventArgs) Handles hdnimgBtnDesignation.Click
        mDesignationList = DesignationList.GetDesignationList(, "(SELECT)")
        cmbDesignationList.DataSource = mDesignationList
        Session("mDesignationList") = mDesignationList
        cmbDesignationList.DataBind()
        upnlAuditDet.Update()
    End Sub

    Private Sub HdnBtnExecutionTask_Click(sender As Object, e As EventArgs) Handles hdnimgBtnExecutionTask.Click

        mTaskStatusList = Session("mTaskStatusList")
        dgAuditExecutionTask.DataSource = mAuditExecution.AuditExecutionTasks
        dgAuditExecutionTask.DataBind()
        upnlGrid.Update()

    End Sub

    Private Sub HdnBtnFindings_Click(sender As Object, e As EventArgs) Handles hdnimgBtnFindings.Click

        mTaskStatusList = Session("mTaskStatusList")
        dgAuditExecutionTask.DataSource = mAuditExecution.AuditExecutionTasks
        dgAuditExecutionTask.DataBind()
        ContolVisibility() 'Added by Ajay 16-Nov-2022
        upnlGrid.Update()

    End Sub

    Private Sub GV_AuditExecutionTask_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles dgAuditExecutionTask.RowDataBound

        If e.Row.RowType <> DataControlRowType.DataRow Then
            Return
        End If

        If (e.Row.RowType = DataControlRowType.DataRow) Then

            Dim ID As Guid = (DataBinder.Eval(e.Row.DataItem, "ID"))
            mAuditExecution = Session("mAuditExecution")
            Dim grdTaskFindings As GridView = DirectCast(e.Row.FindControl("grdTaskFindings"), GridView)
            Dim lblFindings As Label = DirectCast(e.Row.FindControl("lblAuditFindings"), Label)

            If mAuditExecution.AuditExecutionTasks(e.Row.RowIndex).AuditExecutionTaskFindings.Count > 0 Then
                e.Row.Cells(0).BackColor = Color.Yellow 'System.Drawing.ColorTranslator.FromHtml("#0000FF")
                lblFindings.Text = "Task Findings: " & mAuditExecution.AuditExecutionTasks(e.Row.RowIndex).AuditExecutionTaskFindings.Count & " Record(s)."
            Else
                lblFindings.Text = "Task Findings: 0 Record(s)."
            End If

            grdTaskFindings.DataSource = mAuditExecution.AuditExecutionTasks(e.Row.RowIndex).AuditExecutionTaskFindings
            grdTaskFindings.DataBind()

            Dim Attachment As Boolean
            Dim Mail As Boolean

            For j As Integer = 0 To grdTaskFindings.Rows.Count - 1
                Attachment = CType(grdTaskFindings.Rows(j).Cells(20).Text, Boolean) '22=20
                Mail = CType(grdTaskFindings.Rows(j).Cells(21).Text, Boolean) '23=21

                If Attachment = False Then
                End If

                If Mail = False Then
                    grdTaskFindings.Rows(j).Cells(19).Enabled = False '21=19
                End If

            Next
            'Ajay h 29-09-2023
        End If

    End Sub

    Private Sub AuditorList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbAuditorList.SelectedIndexChanged

        Dim ddl As DropDownList = CType(sender, DropDownList)
        For Each item As ListItem In ddl.Items
            ' Example: Highlight auditors who are not working
            If item.Text <> "(SELECT)" Then
                ' Example: Highlight auditors who are not working
                If mAuditorList(item.Text.Split("(")(0).ToString.Trim).IsNotWorking Then
                    item.Attributes.Add("style", "background-color: yellow;")
                Else
                    item.Attributes.Add("style", "background-color: white;")
                End If
            End If
        Next

        If mAuditorList(cmbAuditorList.SelectedItem.ToString.Split("(")(0).ToString.Trim).IsNotWorking Then
            MSGBoxCtrl.Show("Alert..!!!",
                           "Selected auditor is not working with this organization.",
                           "",
                           MsgBoxStyle.OkOnly,
                           "")
            cmbAuditorList.SelectedIndex = 0
            Exit Sub
        End If



        If mDesignationList.Count > 0 Then

            If mAuditorList.Item(cmbAuditorList.SelectedItem.ToString.Split("(")(0).ToString.Trim).DesignationID.ToString <> "" Then
                cmbDesignationList.SelectedValue = mAuditorList.Item(cmbAuditorList.SelectedItem.ToString.Split("(")(0).ToString.Trim).DesignationID.ToString
                upnlAuditDet.Update()
            End If

        End If

    End Sub

    Protected Sub cmbAuditorList_DataBound(sender As Object, e As EventArgs)
        mAuditorList = Session("mAuditorList")
        Dim ddl As DropDownList = CType(sender, DropDownList)
        For Each item As ListItem In ddl.Items
            If item.Text <> "(SELECT)" Then
                ' Example: Highlight auditors who are not working
                If mAuditorList(item.Text.Split("(")(0).ToString.Trim).IsNotWorking Then
                    item.Attributes.Add("style", "background-color: yellow;")
                Else
                    item.Attributes.Add("style", "background-color: white;")
                End If
            End If
        Next
    End Sub

    'Protected Sub cmbAuditorList_ItemDataBound(sender As Object, e As EventArgs)
    '    ' Assuming your Auditor object has a property called "IsNotWorking"
    '    Dim auditor As Auditor = CType(e.Item, Auditor)
    '    If auditor IsNot Nothing And auditor.IsNotWorking Then
    '        e.Item.Attributes.Add("style", "color: yellow;")
    '    End If
    'End Sub



    Private Sub SendEMail(sender As Object, e As EventArgs) Handles btnSendMail.Click

        If Not IsValid Then upnlValidation.Update() : Exit Sub

        If User.Identity.Name.ToUpper = "BTPLADMIN" Or User.Identity.Name.ToUpper = "BYTZADMIN" Then ' BYTZADMIN For Deccan 'Added by Prashant 15-Oct-2019   
            'Do nothing 
        Else
            SendMail(Guid.Empty, 0, mAuditExecution.ID.ToString)
        End If

    End Sub

    Private Sub PrintReport(sender As Object, e As EventArgs) Handles btnPrint.Click

        If (Not IsInRole(Rights.[Print])) Then

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
        Dim mPriorityLevel1ID As Integer = 0
        Dim mPriorityLevel2ID As Integer = 0
        Dim mPriorityLevel3ID As Integer = 0
        Dim mPriorityLevel4ID As Integer = 0
        Dim mOpen As Integer = 0
        Dim mClose As Integer = 0
        Dim mPriorityLevel1Name,
            mPriorityLevel2Name,
            mPriorityLevel3Name,
            mPriorityLevel4Name As String

        myReport = New crFindingReportForStarAir
        mrptAuditFindings = rptAuditFindings.GetrptAuditFindings("1/1/1900",
                                                                 "1/1/2100",
                                                                 mAuditExecution.AuditNo, , , ,
                                                                 1,
                                                                 0,
                                                                 mAuditExecution.ID.ToString)

        mPriorityLevel1Name = ""
        mPriorityLevel2Name = ""
        mPriorityLevel3Name = ""
        mPriorityLevel4Name = ""

        For i As Integer = 0 To mrptAuditFindings.Count - 1

            If mrptAuditFindings(i).PriorityID = 1 Then
                mPriorityLevel1ID = mPriorityLevel1ID + 1
            End If

            If mrptAuditFindings(i).PriorityID = 2 Then
                mPriorityLevel2ID = mPriorityLevel2ID + 1
            End If

            If mrptAuditFindings(i).PriorityID = 3 Then
                mPriorityLevel3ID = mPriorityLevel3ID + 1
            End If

            If mrptAuditFindings(i).PriorityID = 4 Then
                mPriorityLevel4ID = mPriorityLevel4ID + 1
            End If

            If mrptAuditFindings(i).FindingStatusID = 1 Then
                mOpen = mOpen + 1
            ElseIf mrptAuditFindings(i).FindingStatusID = 2 Then
                mClose = mClose + 1
            End If

        Next

        Dim mAuditPriorityList As AuditPriorityList = AuditPriorityList.GetAuditPriorityList

        For i As Integer = 0 To mAuditPriorityList.Count - 1

            If mAuditPriorityList(i).ID = 1 Then
                mPriorityLevel1Name = mAuditPriorityList(i).ShortName
            ElseIf mAuditPriorityList(i).ID = 2 Then
                mPriorityLevel2Name = mAuditPriorityList(i).ShortName
            ElseIf mAuditPriorityList(i).ID = 3 Then
                mPriorityLevel3Name = mAuditPriorityList(i).ShortName
            ElseIf mAuditPriorityList(i).ID = 4 Then
                mPriorityLevel4Name = mAuditPriorityList(i).ShortName
            End If

        Next

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
                                     SearchStr4:=mPriorityLevel1ID,
                                     SearchStr5:=mPriorityLevel2ID,
                                     ProductVersion:=AppSettings("Product Version"),
                                     SINote:=AppSettings("SINote"),
                                     SearchStr6:=mOpen,
                                     SearchStr7:=mClose,
                                     SearchStr8:=mPriorityLevel3ID,
                                     SearchStr9:=mPriorityLevel4ID,
                                     SearchStr10:=AppSettings("Logo"),
                                     SearchStr11:=mPriorityLevel1Name,
                                     SearchStr12:=mPriorityLevel2Name,
                                     SearchStr13:=mPriorityLevel3Name,
                                     SearchStr14:=mPriorityLevel4Name,
                                     SearchStr15:=AppSettings("ClientCode")) 'Changed By Utkarsh For Report Logo.
        '----------------------------------------------------------

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

#End Region

End Class