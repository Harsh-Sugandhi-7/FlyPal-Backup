Imports System.Web.Services
Imports System.Text
Imports System.Collections.Generic
Imports System.Linq
Public Class wfGroupTrainingConfiguration_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Public mEmployeeList As EmployeeList
    Public mEmployeeTraining As EmployeeTraining
    Public mTraining As Training
    Public mTrainingOrgList As TrainingOrgList
    Public mEmployeeTrainningRegister As EmployeeTrainningRegister
    Dim EventLogID As Guid
    Dim mTrainingID As Guid
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    Public mMonthList As MonthList
    Dim mEmployeeTrainingList As EmployeeTrainingList
    Dim SkipNames As String
    Dim var As String
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mTraining = CType(Session("mTraining"), Training)
        mEmployeeList = CType(Session("mEmployeeList"), EmployeeList)
        mTrainingOrgList = CType(Session("mTrainingOrgList"), TrainingOrgList)
        mEmployeeTrainningRegister = CType(Session("mEmployeeTrainningRegister"), EmployeeTrainningRegister)
        mTrainingID = Session("mTrainingID")
        mMonthList = Session("mMonthList")
        mFileAttach = Session("mFileAttach")
        SkipNames = Session("SkipNames")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mTraining")
        Session.Remove("mEmployeeList")
        Session.Remove("mMonthList")
        Session.Remove("mEmployeeTrainningRegister")
        Session.Remove("mTrainingID")
        Session.Remove("mTrainingOrgList")
        Session.Remove("mFileAttach")
        Session.Remove("mEmployeeTraining")
        Session.Remove("SkipNames")
        Session.Remove("EditTrainingGroup")
        Session.Remove("DoneDate")
    End Sub
    Private Sub NewTrainingRecord()
        mEmployeeTraining = EmployeeTraining.NewEmployeeTraining
        Session("mEmployeeTraining") = mEmployeeTraining
    End Sub
    Private Sub EditTrainingRecord(ByVal mID As Guid)
        mEmployeeTraining = EmployeeTraining.GetEmployeeTraining(mID)
        Session("mEmployeeTraining") = mEmployeeTraining
    End Sub
    Private Sub SetObject(ByVal mEmployeeTraining As EmployeeTraining, ByVal EmpID As String)
        mEmployeeTraining.EmployeeID = New Guid(EmpID)
        mEmployeeTraining.TrainingID = mTraining.ID
        ' mEmployeeTraining.CertificateNo = Trim(txtCertificateNo.Text)
        'If mEmployeeTraining.Date.ToString = "" Then ''If from EmployeeTraining from employee master if date is assgined that should not change
        '    If Not IsDate(txtDate.Text) Then
        '        mEmployeeTraining.Date = System.DBNull.Value
        '    Else
        '        mEmployeeTraining.Date = CType(txtDate.Text, Object)
        '    End If
        'End If
        'mEmployeeTraining.Duration = txtDuration.Text
        'mEmployeeTraining.TrainingOrgID = New Guid(cmbTrainingOrgList.SelectedValue)
        'mEmployeeTraining.MonthOfTrainingID = CInt(cmbMonthList.SelectedValue)
        'mEmployeeTraining.YearOfTraining = Val(Trim(txtYearOfTraining.Text))
        'mEmployeeTraining.Remark = Trim(txtRemark.Text)
        mEmployeeTraining.FreqInMonths = Trim(txtFreqInMonths.Text)
        mEmployeeTraining.WarningDays = txtWarningDays.Text
        mEmployeeTraining.RecurringStatus = chkRecurringStatus.Checked
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                mEmployeeTraining.IsAttachmentAdded = True
            Else
                mEmployeeTraining.IsAttachmentAdded = False
            End If
        End If
    End Sub
    Private Sub SaveAttachment(ByVal mEmployeeTraining As EmployeeTraining) '
        Dim mTempFileAttach As FileAttach
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                Try

                    mTempFileAttach = FileAttach.GetAttachment(mEmployeeTraining.ID)
                    If mTempFileAttach.Size <= 0 Then
                        mTempFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mEmployeeTraining.ID)
                        mTempFileAttach.Size = mFileAttach.Size
                        mTempFileAttach.ImageFile = mFileAttach.ImageFile
                        mTempFileAttach.Extension = mFileAttach.Extension
                        mTempFileAttach.Save()
                    Else
                        If mEmployeeTraining.IsAttachmentAdded Then
                            mTempFileAttach.Size = mFileAttach.Size
                            mTempFileAttach.ImageFile = mFileAttach.ImageFile
                            mTempFileAttach.Extension = mFileAttach.Extension
                            mTempFileAttach.Save()
                        Else
                            mTempFileAttach.DeleteAttachment(mTempFileAttach.ID, mEmployeeTraining.ID)
                        End If
                    End If

                Catch ex As Exception
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
                End Try
            Else
                mTempFileAttach = FileAttach.GetAttachment(mEmployeeTraining.ID)
                If mTempFileAttach.Size > 0 Then
                    mTempFileAttach.DeleteAttachment(mTempFileAttach.ID, mEmployeeTraining.ID)
                End If
                'If (Not mEmployeeTraining.IsNew) And IsAttachmentDeleted Then
                '    FileAttach.DeleteAttachment(mFileAttach.ID, mEmployeeTraining.ID)
                'End If
                'IsAttachmentDeleted = False
                'Session("IsAttachmentDeleted") = IsAttachmentDeleted
            End If
        Else
            mTempFileAttach = FileAttach.GetAttachment(mEmployeeTraining.ID)
            If mTempFileAttach.Size > 0 Then
                mTempFileAttach.DeleteAttachment(mTempFileAttach.ID, mEmployeeTraining.ID)
            End If
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        Dim msgCount As Integer = 0

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Save" Then
                        Try
                            For i As Integer = 0 To chkEmployeeList.Items.Count - 1
                                If chkEmployeeList.Items(i).Selected Then
                                    If mEmployeeTrainningRegister.Contains(New Guid(chkEmployeeList.Items(i).Value), "") Then
                                        EditTrainingRecord(mEmployeeTrainningRegister(New Guid(chkEmployeeList.Items(i).Value)).ID)
                                    Else
                                        NewTrainingRecord()
                                    End If
                                    SetObject(mEmployeeTraining, chkEmployeeList.Items(i).Value)
                                    'Çheck  For Duplicate
                                    mEmployeeTraining.Save()
                                    SaveAttachment(mEmployeeTraining)
                                    MarkLog(Flypal.Util.Action.Save, "EmployeeGroupTrainingAllocation", "Emp : " + chkEmployeeList.Items(i).Text + " Training : " + mTraining.Name + " Allocate From Group Training", Flypal.Util.ErrorType.NoError, mEmployeeTraining.ID, EventLogID)
                                ElseIf mEmployeeTrainningRegister.Contains(New Guid(chkEmployeeList.Items(i).Value), "") Then
                                    mEmployeeTraining = EmployeeTraining.GetEmployeeTraining(mEmployeeTrainningRegister(New Guid(chkEmployeeList.Items(i).Value)).ID)
                                    EmployeeTraining.DeleteEmployeeTraining(mEmployeeTraining.ID)
                                    MarkLog(Flypal.Util.Action.Delete, "EmployeeGroupTrainingAllocation", "Emp : " + chkEmployeeList.Items(i).Text + " Training : " + mTraining.Name + " Deallocate or Delete From Group Training", Flypal.Util.ErrorType.NoError, Guid.Empty, EventLogID)
                                End If
                            Next
                            mEmployeeTrainningRegister = EmployeeTrainningRegister.GetEmployeeTrainningRegister(TrainningID:=mTrainingID.ToString)
                            Session("mEmployeeTrainningRegister") = mEmployeeTrainningRegister



                            mEmployeeList = EmployeeList.GetEmployeeList(IsEmployeeWorking:=1, Name:=txtSearchEmpName.Text, Designation:=txtSearchDesignation.Text.Trim, SkipNames:=SkipNames)
                            Session("mEmployeeList") = mEmployeeList
                            chkEmployeeList.DataSource = mEmployeeList
                            chkEmployeeList.DataBind()

                            SetCheckBoxList()
                            upnlRenewalInfo.Update()
                            MSGBoxCtrl.show("Success!", "Training allocated to selected employees successfully", "", MsgBoxStyle.OkOnly, "OkMsg")
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

                        End Try
                   
                    End If
                Case MsgBoxResult.No
                    Session("sender") = ""

                Case MsgBoxResult.Ok
                    Session("sender") = ""
                    If MSGBoxCtrl.Sender = "OkMsg" Then
                        RemoveSession()
                        Dim mopenas As String = Request.QueryString("Type")
                        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
                            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
                            Exit Sub
                        End If
                        Response.Redirect("index.aspx")
                    End If
                Case MsgBoxResult.Ok And MSGBoxCtrl.Sender = "Authorization"
                    Session("sender") = ""
                    DataFieldBind()
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
            DataFieldBind()
        ElseIf Result1 = 0 And MSGBoxCtrl.Sender = "Authorization" Then
            Session("sender") = ""
            DataFieldBind()
        End If
    End Sub
    Private Sub ControlVisibilityForAttachment()
        If mFileAttach.Size > 0 Then
            '   ImageButton1.Visible = True
            '  btnDelAttach.Enabled = True
        Else
            '  ImageButton1.Visible = False
            ' btnDelAttach.Enabled = False
        End If
    End Sub

#End Region

#Region " Data Binding "
    Private Sub DataFieldBind()
        mTraining = Training.GetTraining(mTrainingID)
        Session("mTraining") = mTraining

        mEmployeeTrainningRegister = EmployeeTrainningRegister.GetEmployeeTrainningRegister(TrainningID:=mTrainingID.ToString)
        Session("mEmployeeTrainningRegister") = mEmployeeTrainningRegister

        mEmployeeList = EmployeeList.GetEmployeeList(IsEmployeeWorking:=1, Name:=txtSearchEmpName.Text, Designation:=txtSearchDesignation.Text.Trim, SkipNames:="")
        Session("mEmployeeList") = mEmployeeList
        chkEmployeeList.DataSource = mEmployeeList

        'mMonthList = MonthList.GetMonthList("(SELECT)")
        'cmbMonthList.DataSource = mMonthList
        'Session("mMonthList") = mMonthList

        'mTrainingOrgList = TrainingOrgList.GetTrainingOrgList(, , , "(SELECT)")
        'Session("mTrainingOrgList") = mTrainingOrgList
        'cmbTrainingOrgList.DataSource = mTrainingOrgList

        DataBind()

        If Session("EditTrainingGroup") = "True" Then
            ''txtDate.Text = CType(Session("DoneDate"), String)
        End If
    End Sub
    Private Sub SetCheckBoxList()
        For i As Integer = 0 To chkEmployeeList.Items.Count - 1
            If mEmployeeTrainningRegister.Contains(New Guid(chkEmployeeList.Items(i).Value), "") Then
                chkEmployeeList.Items(i).Selected = True
            End If
        Next
    End Sub
    Private Sub addAttributes()
        '  txtYearOfTraining.Attributes.Add("onKeyPress", "validateText(('NUM'),document.getElementById('txtYearOfTraining').value,event)")
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        addAttributes()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            DataFieldBind()
            'ControlVisibilityForAttachment()
            ' txtDuration.Text = "0"
            SetCheckBoxList()
        End If
    End Sub
    Private Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        If IsValid Then
            ' we'll need a split to get the individual ids  
                Try
                MSGBoxCtrl.show("Save Alert", "You are about to Allocate Training for selected employees. Do you want to continue? ", "", MsgBoxStyle.YesNo, "Save")
                    Exit Sub
                Catch ex As Exception
                Finally

                End Try
        Else
            upnlValidationSummary.Update()
        End If



    End Sub
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        ControlVisibilityForAttachment()
        'SetGrid()
        'upnlGrid.Update()
        '  upnlFileupload.Update()
    End Sub
    'Private Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
    '    '----------------------------------------------------------------------
    '    Dim No As New Random
    '    Dim StrName As String = "abc" & No.Next.ToString
    '    '----------------------------------------------------------------------
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

    '    'IsAttachmentDeleted = True
    '    'Session("IsAttachmentDeleted") = IsAttachmentDeleted

    '    mFileAttach.ImageFile = file1
    '    mFileAttach.Size = 0

    '    ImageButton1.Visible = False
    '    btnDelAttach.Enabled = False

    '    ControlVisibilityForAttachment()
    'End Sub
    'Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
    '    mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, Guid.Empty)
    '    Session("mFileAttach") = mFileAttach
    'End Sub
    Private Sub btnClose_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Dim mopenas As String = Request.QueryString("Type")
        If Not mopenas Is Nothing AndAlso mopenas = "pup" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "onclose", "CallParentCallback();", True)
            Exit Sub
        End If
        Response.Redirect("index.aspx")

    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Protected Sub txtSearchDesignation_TextChanged(sender As Object, e As System.EventArgs)
        mEmployeeList = EmployeeList.GetEmployeeList(IsEmployeeWorking:=1, Name:=txtSearchEmpName.Text, Designation:=txtSearchDesignation.Text.Trim, SkipNames:=SkipNames)
        Session("mEmployeeList") = mEmployeeList
        chkEmployeeList.DataSource = mEmployeeList
        chkEmployeeList.DataBind()
        SetCheckBoxList()
    End Sub

    Protected Sub txtSearchEmpName_TextChanged(sender As Object, e As System.EventArgs)
        mEmployeeList = EmployeeList.GetEmployeeList(IsEmployeeWorking:=1, Name:=txtSearchEmpName.Text, Designation:=txtSearchDesignation.Text.Trim, SkipNames:=SkipNames)
        Session("mEmployeeList") = mEmployeeList
        chkEmployeeList.DataSource = mEmployeeList
        chkEmployeeList.DataBind()
        SetCheckBoxList()
    End Sub
#End Region

#Region " Helper Methods "
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetEmpList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim mEmployeeListAutoComplete As EmployeeListAutoComplete = EmployeeListAutoComplete.GetEmployeeList(prefixText, 1)
        If count = 0 Then
            Return (From c As EmployeeListAutoComplete.EmployeeListAutoCompleteInfo In mEmployeeListAutoComplete
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Name, c.Name)).ToArray
        Else
            Return (From c As EmployeeListAutoComplete.EmployeeListAutoCompleteInfo In mEmployeeListAutoComplete
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Name, c.Name)).Take(count).ToArray
        End If
    End Function
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetEmpDesgList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim mEmployeeDesignationListAutoComplete As EmployeeDesignationListAutoComplete = EmployeeDesignationListAutoComplete.GetEmployeeList(prefixText)
        If count = 0 Then
            Return (From c As EmployeeDesignationListAutoComplete.EmployeeDesignationListAutoCompleteInfo In mEmployeeDesignationListAutoComplete
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Name, c.Name)).ToArray
        Else
            Return (From c As EmployeeDesignationListAutoComplete.EmployeeDesignationListAutoCompleteInfo In mEmployeeDesignationListAutoComplete
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Name, c.Name)).Take(count).ToArray
        End If
    End Function
#End Region

    
End Class