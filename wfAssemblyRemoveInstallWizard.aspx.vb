
'Created By Saylee : 20-Apr-2022


Imports System.Linq.Enumerable
Imports System
Imports System.IO


Public Class wfAssemblyRemoveInstallWizard
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Public mMachineNameValueList As MachineNameValueList
    Public AircraftId As String
    Private mRemovedAssemblyList As tmpRemovedAssemblyList
    Private mInstalledAssemblyList As tmpInstalledAssemblyList
    Public mRemovalReasonList As RemovalReasonList

    Public mRegNo As String
    Public mAssemblyInfo As String
    Public mAssemblyType As String
    Dim mAssemblyDetail As String

    Dim mFileAttachRemoval As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    Dim IsAttachmentInstDeleted As Boolean = False

    'Assembly Removal
    Dim mAssemblyStatusForRemoval As AssemblyStatus
    Dim mMaintenanceDoneByEmployees As New MaintenanceDoneByEmployees
    Dim LicenseNo As String = String.Empty
    Dim EmpName As String = String.Empty
    Dim DoneByID As Guid = Guid.Empty
    Public Flag As Boolean = False
    Public mMachineMaintenanceForRemoval As MachineMaintenance
    Public mMachineMaintenanceListForRemoval As MachineMaintenanceList
    Public mFrom As From
    Dim mMachine As Machine
    Public mEmployeeList As EmployeeList
    Public mEmployeeStatus As EmployeeStatus

    'Assembly Inst
    Public mRemovedAssemblyStatusList As tmpRemovedAssemblyList
    Public mInstalledAssemblyStatusList As tmpInstalledAssemblyList
    Public mAssemblyStatus As AssemblyStatus
    Public mFileAttachInst As FileAttach
    Public mATAList As ATAList
    Public mFromType As From
    Public mMachineMaintenanceListForInst As MachineMaintenanceList
    Public mMachineMaintenanceForInst As MachineMaintenance
    Public previousSelectedForRemoval As Integer
    Public previousSelectedForInst As Integer


    ''Logs
    Public mLogList As LogList
#End Region

#Region " Enum "
    Public Enum From
        NewRemove = 1
        EditRemove = 2
        NewInstall = 3
        EditInstall = 4
    End Enum
#End Region

#Region " Removal Helper Method "
    Private Sub GetSession()
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        mFileAttachRemoval = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        mInstalledAssemblyList = Session("mInstalledAssemblyList")
        mFileAttachRemoval = Session("mFileAttach")
        mAssemblyStatusForRemoval = Session("mAssemblyStatusForRemoval")
        mMachineMaintenanceListForRemoval = Session("mMachineMaintenanceListForRemoval")
        mMachineMaintenanceListForInst = Session("mMachineMaintenanceListForInst")
        mFromType = CType(Session("FromType"), From)
        mAssemblyStatus = Session("mAssemblyStatus")
        previousSelectedForRemoval = Session("previousSelectedForRemoval")
        previousSelectedForInst = Session("previousSelectedForInst")
        mRemovedAssemblyStatusList = Session("mRemovedAssemblyStatusList")
        mLogList = Session("mLogListWizard")
        mMachine = Session("mMachine")
        IsAttachmentInstDeleted = Session("IsAttachmentInstDeleted")
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfAssemblyRemoveInstallWizard.aspx?" Then

            Session.Remove("mAssemblyStatusForRemoval")
            Session.Remove("mRemovalReasonList")

            Session.Remove("mMachine")
            Session.Remove("From")

            Session.Remove("mMachineMaintenanceForRemoval")
            Session.Remove("mMachineMaintenanceListForRemoval")

            Session.Remove("mFileAttach")
            Session.Remove("IsAttachmentDeleted")

            Session.Remove("FromType")

            Session.Remove("mMachineMaintenanceForInst")
            Session.Remove("mMachineMaintenanceListForInst")
            Session.Remove("mAssemblyStatus")
            Session.Remove("mLogListWizard")
            Session.Remove("mRemovedAssemblyStatusList")
            Session.Remove("mMachine")
            Session.Remove("IsAttachmentInstDeleted")
        End If
    End Sub
    Private Sub setSession()
        Session("mAssemblyStatusForRemoval") = mAssemblyStatusForRemoval
        Session("mRemovalReasonList") = mRemovalReasonList

        Session("mMachine") = mMachine
        Session("From") = mFrom

        Session("mMachineMaintenanceForRemoval") = mMachineMaintenanceForRemoval
        Session("mMachineMaintenanceListForRemoval") = mMachineMaintenanceListForRemoval

        Session("mFileAttach") = mAssemblyStatusForRemoval
        Session("IsAttachmentDeleted") = IsAttachmentDeleted

        Session("FromType") = mFromType

        Session("mMachineMaintenanceForInst") = mMachineMaintenanceForInst
        Session("mMachineMaintenanceListForInst") = mMachineMaintenanceListForInst
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mLogListWizard") = mLogList
        Session("mRemovedAssemblyStatusList") = mRemovedAssemblyStatusList
        Session("mMachine") = mMachine
        Session("IsAttachmentInstDeleted") = IsAttachmentInstDeleted
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mRemovalReasonList")

        Session.Remove("mMachineMaintenanceForRemoval")
        Session.Remove("mMachineMaintenanceListForRemoval")

        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        Session.Remove("mAssemblyStatusForRemoval")
        mFromType = Nothing

        Session.Remove("mMachineMaintenanceForInst")
        Session.Remove("mMachineMaintenanceListForInst")
        Session.Remove("mAssemblyStatus")
        Session.Remove("previousSelectedForRemoval")
        Session.Remove("previousSelectedForInst")
        Session.Remove("mRemovedAssemblyStatusList")

        '''Log(s)
        Session.Remove("mLogListWizard")
        Session.Remove("mMachine")

        Session.Remove("IsAttachmentInstDeleted")
    End Sub
    Private Sub DataFieldBind()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToString, SkipIsForInventoryAircarft:=True, SkipReadOnlyAircrafts:=True, TagText:="(SELECT)", IsTagRequired:=True)
        cmbAircraft.DataSource = mMachineNameValueList


        If (IsNothing(AircraftId) Or AircraftId = Guid.Empty.ToString) Then
            'Do nothing
        Else
            cmbAircraft.SelectedValue = AircraftId
        End If
        cmbAircraft.DataBind()

        mRemovalReasonList = RemovalReasonList.GetRemovalReasonList("", "(SELECT REASON)")
        cmbReason.DataSource = mRemovalReasonList
        Session("mRemovalReasonList") = mRemovalReasonList
        cmbReason.DataBind()
        Session("AircraftId") = cmbAircraft.SelectedValue


        'Installation
        mATAList = ATAList.GetATAList("", "(SELECT)")
        Session("mATAList") = mATAList
        cmbATAChapter.DataSource = mATAList
        cmbATAChapter.DataBind()

        cmbATA.DataSource = mATAList
        cmbATA.DataBind()
    End Sub
    Public Sub customvalidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtNote" Then
            If Len(txtNote.Text) > 200 Then
                custValidator.ErrorMessage = "Max. length of Note should be 200 char"
                e.IsValid = False
            Else
                e.IsValid = True
            End If

        ElseIf custValidator.ControlToValidate = "txtLicenceNo" Then
            If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Or (txtLicenceNo.Text.Trim.IndexOf("[") < 0 And txtLicenceNo.Text.Trim.IndexOf("]") < 0) Then
                e.IsValid = True
            Else
                custValidator.ErrorMessage = "Enter Correct License No."
                e.IsValid = False
            End If
            'End
            'Commented by Amrita on 8-Jan-08 for Solving Bug No : - RA6 given by Pramod
            'ElseIf custValidator.ControlToValidate = "cmbReason" Then
            '    If cmbReason.SelectedIndex <= 0 Then
            '        custValidator.ErrorMessage = "Please select Reason from the list"
            '        e.IsValid = False
            '    Else
            '        e.IsValid = True
            '    End If
        End If
    End Sub
    Private Sub SetObjectRemoval()
        With mAssemblyStatusForRemoval
            .RemovalReasonID = New Guid(cmbReason.SelectedValue)
            .RemovalReasonName = cmbReason.SelectedItem.Text


            If calDate.Text <> "" Then
                .RemovedOn = calDate.Text
            Else
                .RemovedOn = System.DBNull.Value
            End If
            .RemovalWONO = Trim(txtWorkOrderNo.Text)
            .RemovalRemark = Trim(txtNote.Text)
            'Added By Vikrant On 12-Jun-2012 FOR ALL08062012
            '.RemDoneByID = New Guid(cmbRemovedBy.SelectedValue)
            '.RemLicenseNo = txtLicenceNo.Text.Trim
            Dim LicenseNo As String = String.Empty
            Dim EmpName As String = String.Empty
            If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
                LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
                EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
            Else
                LicenseNo = Trim(txtLicenceNo.Text)
            End If
            .RemLicenseNo = LicenseNo
            .RemDoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID
            'End

            .RemPlace = txtPlace.Text.Trim
            .IsRemUnschedule = chkIsRemUnscheduled.Checked 'Added By Vikrant On 22-Aug-2012 For ALL20082012
            .IsBackDatedRemoval = True


            Dim Remfile As HttpPostedFile = Request.Files("Myfile")
            If Remfile IsNot Nothing AndAlso Remfile.ContentLength > 0 Then
                Dim fname As String = Path.GetFileName(Remfile.FileName)
                '  File.SaveAs(Server.MapPath(Path.Combine("~/App_Data/", fname)))
            End If

            'If Not file.PostedFile Is Nothing Then
            '    mFileAttachRemoval.Extension = Mid(file.PostedFile.FileName, file.PostedFile.FileName.LastIndexOf(".") + 1)
            '    mFileAttachRemoval.Size = file.PostedFile.ContentLength
            '    ''  mFileAttachRemoval.im(Session("ImageFile"))
            'End If




            Session("mFileAttachRemoval") = mFileAttachRemoval


            'Added By Vikrant On 01-Dec-2014
            If mFileAttachRemoval.Size > 0 Then
                .IsAttachmentAdded = True
            Else
                .IsAttachmentAdded = False
            End If
            'End


        End With
        Session("mAssemblyStatusForRemoval") = mAssemblyStatusForRemoval
    End Sub
    Private Sub SetMachineMaintenanceObjectForRemoval()

        If mFrom = From.NewRemove And Not (mMachineMaintenanceListForRemoval.Contains(mAssemblyStatusForRemoval.ID, 2, "")) Then
            mMachineMaintenanceForRemoval = MachineMaintenance.NewMachineMaintenance(mAssemblyStatusForRemoval.MachineID, 2, calDate.Text, mAssemblyStatusForRemoval.ID, Guid.Empty, 0, 0, mAssemblyStatusForRemoval.ID)
        Else ''If mFrom = From.EditRemove Then
            mMachineMaintenanceForRemoval = MachineMaintenance.GetMachineMaintenance(mAssemblyStatusForRemoval.ID, 2)
            Session("mMachineMaintenanceForRemoval") = mMachineMaintenanceForRemoval
        End If

        With mMachineMaintenanceForRemoval
            .MachineID = mAssemblyStatusForRemoval.MachineID
            ''.MaintenanceActivityTypeID = 2
            .MaintenanceID = mAssemblyStatusForRemoval.ID 'TransactionID
            .AssemblyStatusID = mAssemblyStatusForRemoval.ID

            .Date = calDate.Text
            Dim mMaxLogNo As MaxLogNo
            mMaxLogNo = MaxLogNo.GetMaxLogNo(calDate.Text, mAssemblyStatusForRemoval.MachineID, mAssemblyStatusForRemoval.AssemblyID)
            If mMaxLogNo.Count <> 0 Then
                .LogNo = mMaxLogNo(0).LogNo
                .LogID = mMaxLogNo(0).LogId
                .LogPageNo = mMaxLogNo(0).LogPageNo
            End If
        End With

        Session("mMachineMaintenanceForRemoval") = mMachineMaintenanceForRemoval
    End Sub
    Private Sub SaveMachineMaintenanceRem()
        mMachineMaintenanceForRemoval = Session("mMachineMaintenanceForRemoval")
        If mMachineMaintenanceForRemoval.IsValid = True Then
            Try
                mMachineMaintenanceForRemoval.ApplyEdit()
                mMachineMaintenanceForRemoval.Save()
                Session("mMachineMaintenanceForRemoval") = mMachineMaintenanceForRemoval
            Catch ex As Exception

            End Try
        End If
        ''End If
    End Sub
    Public Sub SetLicenceCount()
        If mAssemblyStatusForRemoval.MaintenanceDoneByEmployees.Count > 1 Then
            lblLicenceCount.Text = "and " + (mAssemblyStatusForRemoval.MaintenanceDoneByEmployees.Count - 1).ToString + " more"
        End If
        lblLicenceCount.DataBind()
        'lblAllLicenceNos.DataBind()
    End Sub
    Private Sub BindLicenceNo()
        If mAssemblyStatusForRemoval.MaintenanceDoneByEmployees.Count > 0 Then
            txtLicenceNo.Text = mAssemblyStatusForRemoval.MaintenanceDoneByEmployees(0).LicenceNo + " [" + mAssemblyStatusForRemoval.MaintenanceDoneByEmployees(0).EmployeeName + "]"
        Else
            txtLicenceNo.Text = String.Empty
        End If
    End Sub
    Public Function CustomValidateRem() As Boolean
        Dim str As String = ""
       SetObjectRemoval()

        If Not mAssemblyStatusForRemoval.IsValid Then
            For i As Integer = 0 To mAssemblyStatusForRemoval.GetBrokenRulesCollection.Count - 1
                str = str + mAssemblyStatusForRemoval.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(dgRemovalValue.Rows.Count - 1)
            If Not mAssemblyStatusForRemoval.AssemblyStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mAssemblyStatusForRemoval.AssemblyStatusPeriods(i).GetBrokenRulesCollection.Count - 1
                    str = str + mAssemblyStatusForRemoval.AssemblyStatusPeriods(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next

        If str <> "" Then
            cvReason.ErrorMessage = str
            cvReason.IsValid = False
            Return False
        Else
            Return True
        End If
    End Function
    Public Sub CustomValidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        If Flag = 1 Then Exit Sub
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        'this is for grid validation
        SetObjectRemoval()
        Dim str As String = ""
        If Not mAssemblyStatusForRemoval.IsValid Then
            For i As Integer = 0 To mAssemblyStatusForRemoval.GetBrokenRulesCollection.Count - 1
                str = str + mAssemblyStatusForRemoval.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(dgRemovalValue.Rows.Count - 1)
            If Not mAssemblyStatusForRemoval.AssemblyStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mAssemblyStatusForRemoval.AssemblyStatusPeriods(i).GetBrokenRulesCollection.Count - 1
                    str = str + mAssemblyStatusForRemoval.AssemblyStatusPeriods(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next
        If str <> "" Then
            custValidator.ErrorMessage = str
            e.IsValid = False
        End If
        Flag = 1
    End Sub
    Private Sub ControlVisibilityForAttachmentRemoval()
        'If Not mFileAttachRemoval Is Nothing Then
        '    If mFileAttachRemoval.Size > 0 Then
        '        ImageButton1.Visible = True
        '        btnDelAttach.Enabled = True
        '    Else
        '        ImageButton1.Visible = False
        '    End If
        'Else
        '    ImageButton1.Visible = False
        'End If
    End Sub
    Private Sub ControlVisibilityForAttachmentInst()
        'If Not mFileAttachInst Is Nothing Then
        '    If mFileAttachInst.Size > 0 Then
        '        ImageButton3.Visible = True
        '        btnRemoveFileInst.Enabled = True
        '    Else
        '        ImageButton3.Visible = False
        '    End If
        'Else
        '    ImageButton3.Visible = False
        'End If

    End Sub
#End Region

#Region " Inst Helper Methods "
    Public Function CheckPeriodsForRemovedAssemblyStatus(ByVal RemovedAssemblyStatus As AssemblyStatus) As Boolean
        Dim i As Integer = 0
        Dim tmpIsPeriodExists As Boolean = False
        If RemovedAssemblyStatus.AssemblyTypeID = 2 Or RemovedAssemblyStatus.AssemblyTypeID = 4 Then Return True
        mMachine = Machine.GetMachine(New Guid(cmbAircraft.SelectedValue))
        While i <= RemovedAssemblyStatus.AssemblyStatusPeriods.Count - 1
            If mMachine.AssemblyStatus.AssemblyStatusPeriods.Contains(RemovedAssemblyStatus.AssemblyStatusPeriods(i).PeriodID) Then
                tmpIsPeriodExists = True
            Else
                tmpIsPeriodExists = False
                Exit While
            End If
            i = i + 1
        End While
        Return tmpIsPeriodExists
    End Function
    Private Sub DataBindGrid()
        mAssemblyStatus = Session("mAssemblyStatus")
        dgInstallationValue.DataSource = mAssemblyStatus.AssemblyStatusPeriods
        dgInstallationValue.DataBind()
        upnlInstallationValue.Update()
    End Sub
    Public Function CustomValidate2() As Boolean
        Dim str As String = ""
        SetGridObjectInst()
        SetObjectInst()
        If Not mAssemblyStatus.IsValid Then
            Dim x As Integer
            For x = 0 To mAssemblyStatus.GetBrokenRulesCollection.Count - 1
                str = str + mAssemblyStatus.GetBrokenRulesCollection(x).Description + "<BR>"
            Next
        End If

        For i As Integer = 0 To CShort(dgInstallationValue.Rows.Count - 1)
            If Not mAssemblyStatus.AssemblyStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mAssemblyStatus.AssemblyStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mAssemblyStatus.AssemblyStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next

        If str <> "" Then
            cvInstallationRemark.ErrorMessage = str
            cvInstallationRemark.IsValid = False
            Return False
        Else
            Return True
        End If
    End Function
    Public Sub SetLicenceCountInst()
        If mAssemblyStatus.MaintenanceDoneByEmployees.Count > 1 Then
            lblLicenceCount.Text = "and " + (mAssemblyStatus.MaintenanceDoneByEmployees.Count - 1).ToString + " more"
        End If
        lblLicenceCountInst.DataBind()
        'lblAllLicenceNos.DataBind()
    End Sub
    Private Sub BindLicenceNoInst()
        If mAssemblyStatus.MaintenanceDoneByEmployees.Count > 0 Then
            txtLicenceNoInst.Text = mAssemblyStatus.MaintenanceDoneByEmployees(0).LicenceNo + " [" + mAssemblyStatus.MaintenanceDoneByEmployees(0).EmployeeName + "]"
        Else
            txtLicenceNoInst.Text = String.Empty
        End If
    End Sub
    Private Sub SetObjectInst()
        mAssemblyStatusForRemoval = Session("mAssemblyStatusForRemoval")
        With mAssemblyStatus
            .Assembly.ModelID = mAssemblyStatusForRemoval.Assembly.ModelID
            .ATAID = New Guid(cmbATAChapter.SelectedValue)
            .MachineID = mAssemblyStatusForRemoval.MachineID
            .Position = txtPosition.Text.Trim
            .InstallationWONo = txtWorkOrNo.Text.Trim
            .InstallationRemark = txtNote.Text.Trim
            .Assembly.SerialNo = txtSerialNo.Text.Trim
            If txtInstalledOnDate.Text = "" Then
                .InstalledOn = DBNull.Value
            Else
                .InstalledOn = txtInstalledOnDate.Text
            End If
         
            Dim LicenseNo As String = String.Empty
            Dim EmpName As String = String.Empty
            If (txtLicenceNoInst.Text.Trim.IndexOf("[") > 0 And txtLicenceNoInst.Text.Trim.IndexOf("]") > 0) Then
                LicenseNo = txtLicenceNoInst.Text.Substring(0, txtLicenceNoInst.Text.Trim.IndexOf("[")).Trim
                EmpName = Mid(txtLicenceNoInst.Text.Trim, txtLicenceNoInst.Text.Trim.IndexOf("[") + 2, txtLicenceNoInst.Text.Trim.IndexOf("]") - txtLicenceNoInst.Text.Trim.IndexOf("[") - 1).Trim
            Else
                LicenseNo = Trim(txtLicenceNoInst.Text)
            End If
            .InstLicenseNo = LicenseNo
            .InstDoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID
            .InstPlace = txtPlace.Text.Trim

            .InstallationReason = Trim(txtInstallationReason.Text)
            .IsBackDatedInstallation = True


        

            'If FileUpload1.HasFile Then

            '    mFileAttachInst = FileAttach.NewAttachment(Guid.NewGuid, mAssemblyStatus.ID, Sort:=1) 'Sort = 1 : Installation
            '    mFileAttachInst.Extension = Mid(FileUpload1.PostedFile.FileName, FileUpload1.PostedFile.FileName.LastIndexOf(".") + 1)
            '    mFileAttachInst.Size = FileUpload1.PostedFile.ContentLength
            '    mFileAttachInst.ImageFile = FileUpload1.FileBytes
            'End If

            'If Not mFileAttachInst Is Nothing Then
            '    If mFileAttachInst.Size > 0 Then
            '        .IsAttachmentAdded = True
            '    Else
            '        .IsAttachmentAdded = False
            '    End If
            'End If
            'End
        End With
        Session("mAssemblyStatus") = mAssemblyStatus

    End Sub

    Private Sub SetMachineMaintenanceObjectInst()
        mMachineMaintenanceListForInst = Session("mMachineMaintenanceListForInst")
        'Added by Saylee on 6th-Oct-2009
        If mFromType = From.NewInstall And Not (mMachineMaintenanceListForInst.Contains(mAssemblyStatus.ID, 1, "")) Then
            mMachineMaintenanceForInst = MachineMaintenance.NewMachineMaintenance(mMachine.ID, 1, txtInstalledOnDate.Text, mAssemblyStatus.ID, Guid.Empty, 0, 0, mAssemblyStatus.ID)
        Else  ''If mFromType = From.EditInstall Then
            mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
            mMachineMaintenanceForInst = MachineMaintenance.GetMachineMaintenance(mAssemblyStatus.ID, 1)
            Session("mMachineMaintenanceForInst") = mMachineMaintenanceForInst
        End If

        With mMachineMaintenanceForInst
            .MachineID = mAssemblyStatus.MachineID
            ''.MaintenanceActivityTypeID = 1
            .MaintenanceID = mAssemblyStatus.ID 'TransactionID
            .AssemblyStatusID = mAssemblyStatus.ID

            .Date = txtInstalledOnDate.Text
            Dim mMaxLogNo As MaxLogNo
            mMaxLogNo = MaxLogNo.GetMaxLogNo_WhileAssemblyInstall(txtInstalledOnDate.Text, mAssemblyStatus.MachineID)
            If mMaxLogNo.Count <> 0 Then
                .LogNo = mMaxLogNo(0).LogNo
                .LogID = mMaxLogNo(0).LogId
                .LogPageNo = mMaxLogNo(0).LogPageNo
            End If
        End With

        Session("mMachineMaintenanceForInst") = mMachineMaintenanceForInst
    End Sub

    Private Sub SaveMachineMaintenanceInst()
        mMachineMaintenanceForInst = Session("mMachineMaintenanceForInst")
        If mMachineMaintenanceForInst.IsValid = True Then
            Try
                mMachineMaintenanceForInst.ApplyEdit()
                mMachineMaintenanceForInst.Save()
                Session("mMachineMaintenanceForInst") = mMachineMaintenanceForInst
            Catch ex As Exception

            End Try
        End If
        ''End If
    End Sub

    Private Sub SetGridObjectInst()
        For i As Integer = 0 To dgInstallationValue.Rows.Count - 1
            Dim txtAssemblyInstallationValue As TextBox = CType(Me.dgInstallationValue.Rows(i).FindControl("txtAssemblyInstallationValue"), TextBox)
            If mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 2 And txtAssemblyInstallationValue.Text.Trim = "" Then 'This If Condition added by vikrant on 19-Jun-2020 to save 0 instead of null if nothing enetered in TextBox
                mAssemblyStatus.AssemblyStatusPeriods(i).AssemblyInstallationValueFormatted = New Period(mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID, 0).Value
                mAssemblyStatus.AssemblyStatusPeriods(i).AssemblyCurrentValueFormatted = New Period(mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID, 0).Value
            Else
                mAssemblyStatus.AssemblyStatusPeriods(i).AssemblyInstallationValueFormatted = txtAssemblyInstallationValue.Text.Trim
                mAssemblyStatus.AssemblyStatusPeriods(i).AssemblyCurrentValueFormatted = txtAssemblyInstallationValue.Text.Trim
            End If
        Next
        Session("mAssemblyStatus") = mAssemblyStatus
    End Sub
    Private Sub GetAttachmentInst()
        If mAssemblyStatus.IsAttachmentAdded And mFileAttachInst Is Nothing Then
            mFileAttachInst = FileAttach.GetAttachment(mAssemblyStatus.ID)
            Session("mFileAttach") = mFileAttachInst
        End If
    End Sub
    Private Sub SaveAttachmentInst() '
        If Not mFileAttachInst Is Nothing Then
            mFileAttachInst.ReferenceID = mAssemblyStatus.ID
            If mFileAttachInst.Size > 0 Then
                Try
                    mFileAttachInst.Save()
                    'mFileAttach = Nothing
                    'Session("mFileAttach") = mFileAttach
                Catch ex As Exception
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
                End Try
            Else
                If (Not mAssemblyStatus.IsNew) And IsAttachmentInstDeleted Then
                    FileAttach.DeleteAttachment(mFileAttachInst.ID, mAssemblyStatus.ID, 1)
                End If
                IsAttachmentInstDeleted = False
                Session("IsAttachmentInstDeleted") = IsAttachmentInstDeleted
            End If
        End If

    End Sub
    Private Function SetInstallation() As Boolean
        If Not IsValid Then Exit Function
        Dim clnAssemblyStatus As AssemblyStatus = mAssemblyStatus.Clone
        SetObjectInst()
        SetGridObjectInst()
        SetMachineMaintenanceObjectInst() 'Added by Saylee on 6th-Oct-2009
        If mAssemblyStatus.IsValid = True Then
            Try

                If Not mAssemblyStatus.InstDoneByID.Equals(Guid.Empty) AndAlso Not mAssemblyStatus.InstalledOn.Equals(System.DBNull.Value) Then
                    Dim title As String = "Save Alert !"
                    Dim message As String = ""
                    mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mAssemblyStatus.InstDoneByID.ToString, mAssemblyStatus.InstalledOn)
                    If (mEmployeeStatus(0).Information <> "") Then
                        message = mEmployeeStatus(0).Information
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenAlertMessage", MessageBox.Show(title, message, , False), True)
                        Return False
                    End If
                End If
                'End
                ''''''mAssemblyStatus.ApplyEdit()
                ''''''mAssemblyStatus = CType(mAssemblyStatus.Save, AssemblyStatus)
                ''''''SaveAttachmentInst()
                ''''''SaveMachineMaintenanceInst()
                Session("mAssemblyStatus") = mAssemblyStatus
                ''''''mAssemblyDetail = "Reg No. : " + txtAircraft.Text + " Model : " + txtModel.Text + " Serial No. : " + txtSerialNo.Text & " Installed On :" & txtInstalledOnDate.Text
                ''''''MarkLog(Util.Action.Save, "AssemblyInstallation", mAssemblyDetail, Util.ErrorType.NoError, mAssemblyStatus.ID, EventLogID)
                Return True
            Catch ex As SqlException
                mAssemblyStatus = clnAssemblyStatus
                If ex.Number = 8114 Or ex.Number = 8115 Then

                    MSGBoxCtrlNEW.showbox(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")

                ElseIf ex.Number = 8145 Then
                    MSGBoxCtrlNEW.showbox(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    Dim tmpRemovedAssemblyStatusList As tmpRemovedAssemblyList = Session("mRemovedAssemblyStatusList")
                    'Dim mtmpAssemblyStatus As AssemblyStatus = AssemblyStatus

                    If AppSettings("InstallExistingAssemblyWithNewValue") = "True" And tmpRemovedAssemblyStatusList.Contains(mAssemblyStatus.Assembly.ModelID, mAssemblyStatus.Assembly.SerialNo) = True Then
                        MSGBoxCtrlNEW.show("Alert!!", "This Serial No. is already maintained in the system.", "Do you want to replace it?", MsgBoxStyle.YesNo, "InstallExistingAssemblyWithNewValue")
                    Else
                        MSGBoxCtrlNEW.showbox(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                    End If

                ElseIf InStr(ex.Message, "FKtabAssemblyStatustabAssembly", CompareMethod.Text) Or InStr(ex.Message, "Installation of Assembly is not possible as you can not change No. of assemblies of this type on this aircraft", CompareMethod.Text) Then
                    MSGBoxCtrlNEW.showbox(MSGBox.Message_title.DatabaseException, MSGBox.Message_text.DatabaseException, "Installation of Assembly is not possible as you can not change No. of assemblies of this type on this aircraft", MsgBoxStyle.OkOnly, "")
                End If
                Return False
            Finally
                clnAssemblyStatus = Nothing
            End Try
        Else
            Return False
        End If
    End Function
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrlNEW.Result

        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                   
                Case MsgBoxResult.No
                   

                Case MsgBoxResult.Ok
                    If MSGBoxCtrlNEW.Sender = "Success" Then
                        Response.Redirect("Dashboard.aspx")
                    End If

            End Select
        End If
    End Sub
#End Region

#Region "Events Removal"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ClearAll()
            GetSession()
            EventLogID = CType(Session("EventLogID"), Guid)
            'Page.Form.Attributes.Add("enctype", "multipart/form-data")
            If Not IsPostBack Then
                Session("MiddleFrame") = "wfAssemblyRemoveInstallWizard.aspx?"
                DataFieldBind()
                '  ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "SetWizardFirstStepTo();", "SetWizardFirstStepTo();", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "showfirstwiz", "showfirstwiz();", True)
                ''''btnRem.Visible = False
                ''''btnRem.Attributes.Add("style", "display:none;")
            End If

        Catch ex As Exception

        End Try

    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged, calDate.TextChanged

        If calDate.Text = "" Then
            MSGBoxCtrlNEW.show("Alert...!!!", "Please select Removal Date " + calDate.Text, "", MsgBoxStyle.OkOnly, "")
            cmbAircraft.SelectedIndex = 0
            Exit Sub
        End If



        mInstalledAssemblyList = tmpInstalledAssemblyList.GetInstalledAssemblyList(calDate.Text, cmbAircraft.SelectedValue.ToString, "", "")
        dgInstalledAssemblyList.DataSource = mInstalledAssemblyList
        Session("mInstalledAssemblyList") = mInstalledAssemblyList
        dgInstalledAssemblyList.DataBind()
        UpnlInstalledAssemblyList.Update()


        'Removed List: for Installation
        mRemovedAssemblyStatusList = tmpRemovedAssemblyList.GetRemovedAssemblyList(calDate.Text, Guid.Empty.ToString, "", "")

        dgRemovedAssemblyList.DataSource = mRemovedAssemblyStatusList
        Session("mRemovedAssemblyStatusList") = mRemovedAssemblyStatusList
        dgRemovedAssemblyList.DataBind()
        UpnlRemovedAssemblyList.Update()


        mAssemblyStatusForRemoval = Nothing
        Session.Remove("mAssemblyStatusForRemoval")

        mAssemblyStatus = Nothing
        Session.Remove("mAssemblyStatus")

        ScriptManager.RegisterStartupScript(Me, Me.GetType, "showfirstwiz", "showfirstwiz();", True)
    End Sub

    Private Sub dgInstalledAssemblyList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles dgInstalledAssemblyList.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim AssemblyType As String = (DataBinder.Eval(e.Row.DataItem, "AssemblyType"))
            If AssemblyType = "Airframe" Then
                ''
                e.Row.Cells(10).Visible = False
            Else
                e.Row.Cells(10).Visible = True
            End If
        End If
    End Sub
    Private Sub dgInstalledAssemblyList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgInstalledAssemblyList.RowCommand
        Select Case e.CommandName
            Case "DeleteRec"

                Dim InsatlledIndex As Integer = CInt(e.CommandArgument) + dgInstalledAssemblyList.PageSize * dgInstalledAssemblyList.PageIndex
                Dim mID As Guid = mInstalledAssemblyList.Item(InsatlledIndex).AssemblyStatusID
                If (Not User.IsInRole("AssemblyRemovalNew")) Then
                    mRegNo = mInstalledAssemblyList.Item(InsatlledIndex).MachineInfo
                    mAssemblyInfo = mInstalledAssemblyList.Item(InsatlledIndex).AssemblyInfo
                    mAssemblyType = mInstalledAssemblyList.Item(InsatlledIndex).AssemblyType
                    mAssemblyDetail = "Reg No. : " & mRegNo & " Assembly Type : " & mAssemblyType & " Assembly Info. : " & mAssemblyInfo
                    MarkLog(Util.Action.Delete, "AssemblyRemoval", User.Identity.Name & " is not Authorized User to delete " & mAssemblyDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'End
                    MSGBoxCtrlNEW.showbox(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
                If mInstalledAssemblyList.Item(InsatlledIndex).AssemblyTypeID = 1 Then
                    'Added by Vikrant on 26-July-2011
                    mRegNo = cmbAircraft.SelectedItem.Text
                    mAssemblyType = mInstalledAssemblyList(InsatlledIndex).AssemblyType
                    mAssemblyInfo = mInstalledAssemblyList(InsatlledIndex).AssemblyInfo
                    mAssemblyDetail = "Reg No. : " & mRegNo & " Assembly Type : " & mAssemblyType & " Assembly Info. : " & mAssemblyInfo
                    MarkLog(Util.Action.Remove, "AssemblyRemoval", "Can't Remove : Airframe " & mAssemblyDetail & " can not be removed ", Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    MSGBoxCtrlNEW.showbox(MSGBox.Message_title.AirframeDelete, MSGBox.Message_text.AirframeDelete, "You are trying to remove airframe.Airframe can not be removed", MsgBoxStyle.OkOnly, "Delete")
                    Exit Sub
                End If
                Dim checkRemovedAssemblyList As tmpRemovedAssemblyList
                checkRemovedAssemblyList = tmpRemovedAssemblyList.GetRemovedAssemblyList(Today.ToShortDateString, cmbAircraft.SelectedValue, "", "")
                If checkRemovedAssemblyList.Contains(mInstalledAssemblyList.Item(InsatlledIndex).AssemblyStatusID) Then
                    MSGBoxCtrlNEW.showbox(MSGBox.Message_title.SelectRestriction, MSGBox.Message_text.SelectRestriction, "You are trying to remove assembly.Selected " & mInstalledAssemblyList.Item(InsatlledIndex).AssemblyType & ", Already removed, cannot remove again", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                mAssemblyStatusForRemoval = AssemblyStatus.NewRemovalAssemblyStatus(mID, calDate.Text)
                mAssemblyStatusForRemoval.IsBackDatedRemoval = True

                Dim mPrevAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mID)
                Session("mPrevAssemblyStatus") = mPrevAssemblyStatus
                Session("From") = 1 'NewRemove 
                mFrom = From.NewRemove
                mMachine = Machine.GetMachine(mAssemblyStatusForRemoval.MachineID)
                Session("mMachine") = mMachine

                mFileAttachRemoval = FileAttach.NewAttachment(Guid.Empty, mAssemblyStatusForRemoval.ID, Sort:=2) 'Sort = 2 : Removal
                Session("mFileAttach") = mFileAttachRemoval

                mRegNo = cmbAircraft.SelectedItem.Text
                mAssemblyType = mInstalledAssemblyList(InsatlledIndex).AssemblyType
                mAssemblyInfo = mInstalledAssemblyList(InsatlledIndex).AssemblyInfo
                mAssemblyDetail = "Reg No. : " & mRegNo & " Assembly Type : " & mAssemblyType & " Assembly Info. : " & mAssemblyInfo
                MarkLog(Util.Action.Remove, "AssemblyRemoval", mAssemblyDetail, Util.ErrorType.NoError, mInstalledAssemblyList.Item(mInstalledAssemblyList.CurrentIndex).AssemblyStatusID, EventLogID)


                mAssemblyStatusForRemoval.Assembly.SerialNo = mPrevAssemblyStatus.Assembly.SerialNo
                mAssemblyStatusForRemoval.Position = mPrevAssemblyStatus.Position

                dgRemovalValue.DataSource = mAssemblyStatusForRemoval.AssemblyStatusPeriods
                dgRemovalValue.DataBind()

                txtReg.Text = mRegNo
                txtRemovedOn.Text = calDate.Text
                cmbATA.SelectedValue = mAssemblyStatusForRemoval.ATAID.ToString
                txtRemManufacturer.Text = mAssemblyStatusForRemoval.Assembly.Model.ManufacturerName
                txtRemModel.Text = mAssemblyStatusForRemoval.Assembly.Model.Name
                txtRemSerialNo.Text = mAssemblyStatusForRemoval.Assembly.SerialNo + " ( " + mAssemblyStatusForRemoval.Position + " )"
                '' txtRemoPosition.Text = mAssemblyStatusForRemoval.Position



                mMachineMaintenanceListForRemoval = MachineMaintenanceList.GetMachineMaintenanceList(MaintenanceActivityTypeID:=2) '2 for Removal
                Session("mMachineMaintenanceListForRemoval") = mMachineMaintenanceListForRemoval
                '====================================================================

                Session("mAssemblyStatusForRemoval") = mAssemblyStatusForRemoval

                If previousSelectedForRemoval <> InsatlledIndex Then
                    'reset reason combo
                    cmbReason.SelectedIndex = 0
                    txtWorkOrderNo.Text = ""
                    chkIsRemUnscheduled.Checked = False
                    txtPlace.Text = ""
                    txtLicenceNo.Text = ""
                    txtNote.Text = ""
                End If

                ''Installation Details
                'Removed List: for Installation
                mRemovedAssemblyStatusList = tmpRemovedAssemblyList.GetRemovedAssemblyList(calDate.Text, Guid.Empty.ToString, mAssemblyStatusForRemoval.Assembly.ModelName, "")
                dgRemovedAssemblyList.DataSource = mRemovedAssemblyStatusList
                Session("mRemovedAssemblyStatusList") = mRemovedAssemblyStatusList

                dgRemovedAssemblyList.DataBind()
                UpnlRemovedAssemblyList.Update()

                txtInstallationDate.Text = calDate.Text

                pnlAssemblyRem.Visible = True
                pnlRem.Visible = True
                upnlAssemblyRem.Update()
                mdlPopUpAssemblyRem.Show()
                Dim selectedRow As GridViewRow
                If previousSelectedForRemoval >= 0 Then
                    selectedRow = dgInstalledAssemblyList.Rows(previousSelectedForRemoval)
                    selectedRow.Style.Add("background-color", "#ffffff") 'change it back to original color
                End If

                selectedRow = dgInstalledAssemblyList.Rows(InsatlledIndex)
                selectedRow.Style.Add("background-color", "#FFCB60") 'change the color of the new row
                

                previousSelectedForRemoval = InsatlledIndex
                Session("previousSelectedForRemoval") = previousSelectedForRemoval
                '' selectedRow.Attributes.Add("class", "activerow")
        End Select
    End Sub

    Private Sub imgbtnEmployeeLicence_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnEmployeeLicence.Click
        If IsValid Then
            SetObjectRemoval()
            Session("mMaintenanceID") = mAssemblyStatusForRemoval.ID
            mMaintenanceDoneByEmployees = mAssemblyStatusForRemoval.MaintenanceDoneByEmployees
            Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
            Session("MaintenanceDoneOnDate") = mAssemblyStatusForRemoval.RemovedOn.ToString
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AddEmployeeLicNo", "AddEmployeeLicNo();", True)
        Else
            upnlValidationSummary.Update()
        End If

    End Sub



    Private Sub hdnBtnMaintDoneBy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnMaintDoneBy.Click
        If Session("mMaintenanceID") = mAssemblyStatusForRemoval.ID Then
            For i As Integer = 0 To mMaintenanceDoneByEmployees.Count - 1
                Dim ID As Guid = mMaintenanceDoneByEmployees(i).ID
                If Not mAssemblyStatusForRemoval.MaintenanceDoneByEmployees.Contains(ID) Then
                    mAssemblyStatusForRemoval.MaintenanceDoneByEmployees.Add(mMaintenanceDoneByEmployees(i))
                ElseIf mAssemblyStatusForRemoval.MaintenanceDoneByEmployees.Contains(ID) Then
                    mAssemblyStatusForRemoval.MaintenanceDoneByEmployees(ID).LicenceNo = mMaintenanceDoneByEmployees(i).LicenceNo
                    'mAssemblyStatusForRemoval.MaintenanceDoneByEmployees(ID).RequiredManHours = mMaintenanceDoneByEmployees(i).RequiredManHours
                    mAssemblyStatusForRemoval.MaintenanceDoneByEmployees(ID).EmployeeID = mMaintenanceDoneByEmployees(i).EmployeeID
                    mAssemblyStatusForRemoval.MaintenanceDoneByEmployees(ID).EmployeeName = mMaintenanceDoneByEmployees(i).EmployeeName
                End If
            Next

            For j As Integer = 0 To mAssemblyStatusForRemoval.MaintenanceDoneByEmployees.Count - 1
                If Not mMaintenanceDoneByEmployees.Contains(mAssemblyStatusForRemoval.MaintenanceDoneByEmployees(j).ID) Then
                    mAssemblyStatusForRemoval.MaintenanceDoneByEmployees.Remove(mAssemblyStatusForRemoval.MaintenanceDoneByEmployees(j).ID, "")
                End If
            Next
            Session("mAssemblyStatusForRemoval") = mAssemblyStatusForRemoval
            BindLicenceNo()
            SetLicenceCount() 'MLNo
            upnlLicenceNo.Update()
        ElseIf Session("mMaintenanceID") = mAssemblyStatus.ID Then
            For i As Integer = 0 To mMaintenanceDoneByEmployees.Count - 1
                Dim ID As Guid = mMaintenanceDoneByEmployees(i).ID
                If Not mAssemblyStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                    mAssemblyStatus.MaintenanceDoneByEmployees.Add(mMaintenanceDoneByEmployees(i))
                ElseIf mAssemblyStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                    mAssemblyStatus.MaintenanceDoneByEmployees(ID).LicenceNo = mMaintenanceDoneByEmployees(i).LicenceNo
                    'mAssemblyStatus.MaintenanceDoneByEmployees(ID).RequiredManHours = mMaintenanceDoneByEmployees(i).RequiredManHours
                    mAssemblyStatus.MaintenanceDoneByEmployees(ID).EmployeeID = mMaintenanceDoneByEmployees(i).EmployeeID
                    mAssemblyStatus.MaintenanceDoneByEmployees(ID).EmployeeName = mMaintenanceDoneByEmployees(i).EmployeeName
                End If
            Next

            For j As Integer = 0 To mAssemblyStatus.MaintenanceDoneByEmployees.Count - 1
                If Not mMaintenanceDoneByEmployees.Contains(mAssemblyStatus.MaintenanceDoneByEmployees(j).ID) Then
                    mAssemblyStatus.MaintenanceDoneByEmployees.Remove(mAssemblyStatus.MaintenanceDoneByEmployees(j).ID, "")
                End If
            Next
            Session("mAssemblyStatus") = mAssemblyStatus
            BindLicenceNoInst()
            SetLicenceCountInst() 'MLNo
            upnlLicenceNoInst.Update()
        End If

    End Sub
    Protected Sub txtLicenceNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'SetObject()
        If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
            LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
            EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
        Else
            LicenseNo = Trim(txtLicenceNo.Text)
        End If
        DoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID
        Session("LicenseNo") = LicenseNo
        Session("EmployeeID") = DoneByID
        If Not DoneByID.Equals(Guid.Empty) Then
            If mAssemblyStatusForRemoval.MaintenanceDoneByEmployees.Count > 0 Then
                mAssemblyStatusForRemoval.MaintenanceDoneByEmployees(0).EmployeeID = DoneByID
                mAssemblyStatusForRemoval.MaintenanceDoneByEmployees(0).LicenceNo = LicenseNo
                mAssemblyStatusForRemoval.MaintenanceDoneByEmployees(0).EmployeeName = EmpName
            Else
                mAssemblyStatusForRemoval.MaintenanceDoneByEmployees.Add(mAssemblyStatusForRemoval.ID, 2, DoneByID, LicenseNo, "", EmpName)
            End If

        Else
            If mAssemblyStatusForRemoval.MaintenanceDoneByEmployees.Count > 0 Then
                mAssemblyStatusForRemoval.MaintenanceDoneByEmployees.RemoveAt(0)
            End If
        End If
        Session("mAssemblyStatusForRemoval") = mAssemblyStatusForRemoval
        BindLicenceNo()
        SetLicenceCount()
    End Sub
    Private Sub btnRemClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemClose.Click

        mAssemblyStatusForRemoval = Nothing
        mdlPopUpAssemblyRem.Hide()
        pnlAssemblyRem.Visible = False
        pnlRem.Visible = False

        Dim selectedRow As GridViewRow = dgInstalledAssemblyList.Rows(previousSelectedForRemoval)
        selectedRow.Style.Add("background-color", "#ffffff")  'change it back to original color

        Session.Remove("mAssemblyStatusForRemoval")
        Session.Remove("mMachineMaintenanceForRemoval")
        hdnRemAssembly.Value = ""
        upnlAssemblyRem.Update()
    End Sub
    'Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
    '    Dim tempfile As FileAttach
    '    tempfile = CType(Session("mFileAttach"), FileAttach)
    '    If tempfile Is Nothing Then Exit Sub

    '    If tempfile.Sort = 1 Then
    '        mFileAttachInst = Session("mFileAttach")
    '        ControlVisibilityForAttachmentInst()
    '        ''  upnlAttachInst.Update()
    '    Else

    '        mFileAttachRemoval = Session("mFileAttach")
    '        ControlVisibilityForAttachmentRemoval()
    '        upnlAttach.Update()
    '    End If

    'End Sub
    'Private Sub btnSelectRemovalFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectRemovalFile.Click
    '    Session.Remove("mFileAttach")
    '    If mFileAttachRemoval Is Nothing Then
    '        mFileAttachRemoval = FileAttach.NewAttachment(Guid.Empty, mAssemblyStatusForRemoval.ID, Sort:=2) 'Sort = 2 : Removal
    '        Session("mFileAttach") = mFileAttachRemoval
    '    End If
    '    Session("mFileAttach") = mFileAttachRemoval
    '    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "OpenFileDialog", "OpenFileDialog();", True)
    'End Sub
    'Private Sub btnDelAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
    '    Dim fileSize1 As Integer = 0
    '    Dim file1(fileSize1) As Byte

    '    GetAttachmentRemoval()
    '    'mEmployee.ImageFile = file1
    '    'mEmployee.ImageSize = 0
    '    mFileAttachRemoval.ImageFile = file1
    '    mFileAttachRemoval.Size = 0

    '    ImageButton1.Visible = False
    '    btnDelAttach.Enabled = False
    '    IsAttachmentDeleted = True
    '    Session("IsAttachmentDeleted") = IsAttachmentDeleted
    'End Sub
    Private Sub GetAttachmentRemoval()
        If mAssemblyStatusForRemoval.IsAttachmentAdded And mFileAttachRemoval Is Nothing Then
            mFileAttachRemoval = FileAttach.GetAttachment(mAssemblyStatusForRemoval.ID)
            Session("mFileAttach") = mFileAttachRemoval
        End If
    End Sub
    Private Sub SaveAttachmentRemoval()
        If Not mFileAttachRemoval Is Nothing Then
            mFileAttachRemoval.ReferenceID = mAssemblyStatusForRemoval.ID
            If mFileAttachRemoval.Size > 0 Then
                Try
                    mFileAttachRemoval.Save()
                    'mFileAttach = Nothing
                    'Session("mFileAttach") = mFileAttach
                Catch ex As Exception
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
                End Try
            Else
                If (Not mAssemblyStatusForRemoval.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttachRemoval.ID, mAssemblyStatusForRemoval.ID)
                End If
                IsAttachmentDeleted = False
                Session("IsAttachmentDeleted") = IsAttachmentDeleted
            End If
        End If

    End Sub
    Private Sub btnRemOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemOk.Click

        If (Not User.IsInRole("AssemblyRemovalNew") And mAssemblyStatusForRemoval.IsNew) Or (Not User.IsInRole("AssemblyRemovalEdit") And Not mAssemblyStatusForRemoval.IsNew) Then
            SetObjectRemoval()
            setSession()
            'Changed by Vikrant on 26-July-2011
            mAssemblyDetail = "Reg No. : " & mMachine.RegNo & " Model : " & mAssemblyStatusForRemoval.ModelName & " Serial No. : " & mAssemblyStatusForRemoval.Assembly.SerialNo
            MarkLog(Util.Action.Save, "AssemblyRemoval", User.Identity.Name & " is not Authorized User to save" & mAssemblyDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.Authorization, SIMsgBox.Message_text.Authorization, "", MsgBoxStyle.OKOnly)
            'msg.ReplacePage = "wfRemovedAssembly.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage")
            'Session("sender") = "Authorization"
            'msg.Show()
            MSGBoxCtrlNEW.showbox(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "Authorization")
            Exit Sub
        End If

        If IsValid Then
            SetObjectRemoval()
            SetMachineMaintenanceObjectForRemoval()

            If mAssemblyStatusForRemoval.IsValid = True Then

                If Not mAssemblyStatusForRemoval.RemDoneByID.Equals(Guid.Empty) AndAlso Not mAssemblyStatusForRemoval.RemovedOn.Equals(System.DBNull.Value) Then
                    Dim title As String = "Save Alert !"
                    Dim message As String = ""
                    mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mAssemblyStatusForRemoval.RemDoneByID.ToString, mAssemblyStatusForRemoval.RemovedOn.ToString)
                    If (mEmployeeStatus(0).Information <> "") Then
                        message = mEmployeeStatus(0).Information
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenAlertMessage", MessageBox.Show(title, message, IsTagRequired:=False), True)
                    End If
                End If


                MSGBoxCtrlNEW.show("Alert..!!", "Removal of Assembly is now fixed till you Submit", "", MsgBoxStyle.OkOnly, "")
                hdnRemAssembly.Value = "Removal of Assembly"
                mdlPopUpAssemblyRem.Hide()
                pnlAssemblyRem.Visible = False
                pnlRem.Visible = False
                txtInstallationDate.Text = mAssemblyStatusForRemoval.RemovedOnFormatted.ToString
                upnlInstallationDate.Update()
                upnlAssemblyRem.Update()
                '''''btnRem.Visible = True
                '''''btnRem.Attributes.Add("style", "display:block;")
                '''''upnlRembtn.Update()
                Session("mAssemblyStatusForRemoval") = mAssemblyStatusForRemoval
            End If
            If CustomValidateRem() = False Then
                upnlValidationSummary.Update()
            End If
            Exit Sub
        End If


    End Sub
    Private Sub hdnSelectRemoveAssembly_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnSelectRemoveAssembly.Click
        MSGBoxCtrlNEW.show("Alert..!!", "Please select Assembly to be Removed", "", MsgBoxStyle.OkOnly, "")
    End Sub
#End Region

#Region "Events Inst"

    Private Sub calInstDate_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtInstallationDate.TextChanged
        If txtInstallationDate.Text = "" Then
            Exit Sub
        End If

        If CDate(calDate.Text) > CDate(txtInstallationDate.Text) Then
            MSGBoxCtrlNEW.show("Alert...!!!", "Please select Installation date greater than Removal Date " + calDate.Text, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        ''To check whether there are Log between Removal date and Inst Date
        If CDate(calDate.Text) < CDate(txtInstallationDate.Text) Then
            Dim tomorrow As DateTime = CType(calDate.Text, DateTime).AddDays(1)

            mLogList = LogList.GetLogList(New Guid(cmbAircraft.SelectedValue.ToString), tomorrow.ToString, txtInstallationDate.Text, False)
            If mLogList.Count > 0 And CDate(calDate.Text) < CDate(txtInstallationDate.Text) Then
                MSGBoxCtrlNEW.Show("Alert...!!!", "This Removal/Installation will not be possible as there are Logs between dates of Removal and Installation", "", MsgBoxStyle.OkOnly, "")
                Session.Remove("mRemovedAssemblyStatusList")
                Exit Sub
            End If
        End If
      

        mAssemblyStatusForRemoval = Session("mAssemblyStatusForRemoval")
        mRemovedAssemblyStatusList = tmpRemovedAssemblyList.GetRemovedAssemblyList(txtInstallationDate.Text, Guid.Empty.ToString, mAssemblyStatusForRemoval.Assembly.ModelName, "")
        dgRemovedAssemblyList.DataSource = mRemovedAssemblyStatusList
        Session("mRemovedAssemblyStatusList") = mRemovedAssemblyStatusList
        dgRemovedAssemblyList.DataBind()
        UpnlRemovedAssemblyList.Update()




        ScriptManager.RegisterStartupScript(Me, Me.GetType, "showsecondwiz", "showsecondwiz();", True)
    End Sub
    Private Sub MSGBoxCtrlnew_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrlNEW.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub imgbtnEmployeeLicenceInst_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnEmployeeLicenceInst.Click
        If IsValid Then
            Session("mMaintenanceID") = mAssemblyStatus.ID
            mMaintenanceDoneByEmployees = mAssemblyStatus.MaintenanceDoneByEmployees
            Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
            Session("MaintenanceDoneOnDate") = mAssemblyStatus.InstalledOnFormatted.ToString
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AddEmployeeLicNoInst", "AddEmployeeLicNoInst();", True)
        Else
            upnlValidationSummary.Update()
        End If

    End Sub
    Private Sub dgRemovedAssemblyList_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles dgRemovedAssemblyList.RowCommand
        Dim Index As Int32
        Select Case e.CommandName
            Case "InstallSelected"

                If txtInstallationDate.Text = "" Then
                    MSGBoxCtrlNEW.show("Alert..!!", "Please select Installation Date", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                If CDate(calDate.Text) > CDate(txtInstallationDate.Text) Then
                    MSGBoxCtrlNEW.show("Alert...!!!", "Please select Installation date greater than Removal Date " + calDate.Text, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If

                If CDate(calDate.Text) < CDate(txtInstallationDate.Text) Then
                    ''To check whether there are Log between Removal date and Inst Date
                    mLogList = LogList.GetLogList(New Guid(cmbAircraft.SelectedValue.ToString), calDate.Text, txtInstallationDate.Text, False)
                    If mLogList.Count > 0 Then
                        MSGBoxCtrlNEW.show("Alert...!!!", "This Removal/Installation will not be possible as there are Logs between dates of Removal and Installation", "", MsgBoxStyle.OkOnly, "")
                        Session.Remove("mRemovedAssemblyStatusList")
                        Exit Sub
                    End If
                End If



                Index = CInt(e.CommandArgument) + dgRemovedAssemblyList.PageSize * dgRemovedAssemblyList.PageIndex
                Dim Id As Guid = New Guid(dgRemovedAssemblyList.DataKeys(CInt(e.CommandArgument)).Value.ToString)
                mAssemblyStatusForRemoval = Session("mAssemblyStatusForRemoval")
                mMachine = Session("mMachine")

                mRemovedAssemblyStatusList = Session("mRemovedAssemblyStatusList")
                If (Not User.IsInRole("AssemblyInstallationNew")) Then
                    'Added by Vikrant on 28-July-2011
                    mRegNo = mRemovedAssemblyStatusList(Index).MachineInfo
                    mAssemblyType = mRemovedAssemblyStatusList(Index).AssemblyType
                    mAssemblyInfo = mRemovedAssemblyStatusList(Index).AssemblyInfo
                    mAssemblyDetail = "Reg No. : " + mRegNo + " Assembly Type : " + mAssemblyType + " Assembly Info : " + mAssemblyInfo
                    MarkLog(Util.Action.Install, "AssemblyInstallation", User.Identity.Name & " is not Authorized User to install " & mAssemblyDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
                    'End
                    MSGBoxCtrlNEW.showbox(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If



                mInstalledAssemblyStatusList = tmpInstalledAssemblyList.GetInstalledAssemblyList("1/1/2099", cmbAircraft.SelectedValue, "", "")
                If mInstalledAssemblyStatusList.Contains(mRemovedAssemblyStatusList.Item(Index).ModelID, mRemovedAssemblyStatusList.Item(Index).SerialNo) Then
                    MSGBoxCtrlNEW.showbox(MSGBox.Message_title.AssemblyAlreadyInstalled, MSGBox.Message_text.AssemblyAlreadyInstalled, "Selected " & mRemovedAssemblyStatusList.Item(Index).AssemblyType & " already installed. Can not be installed again.", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If



                If CheckPeriodsForRemovedAssemblyStatus(mAssemblyStatusForRemoval) = False Then
                    MSGBoxCtrlNEW.show("Assembly Status Installation Alert!", "Periods for selected " & mRemovedAssemblyStatusList.Item(Index).AssemblyType & " are mismatching with selected Installed On " & cmbAircraft.SelectedItem.Text & " Aircraft.Can not be installed.", "", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If


                Session("FromType") = 1 'NewInstall
                Session("IsExistingAssembly") = CType(True, Boolean)
                ''Installed Assembly Status
                mAssemblyStatus = AssemblyStatus.NewInstallAssemblyStatus(Guid.NewGuid, New Guid(cmbAircraft.SelectedValue), txtInstallationDate.Text, mAssemblyStatusForRemoval.AssemblyTypeID, True, Id.ToString)
                mAssemblyStatus.IsBackDatedInstallation = True

                Session("mAssemblyStatusForRemoval") = mAssemblyStatusForRemoval
                Session("mAssemblyStatus") = mAssemblyStatus
                Session("mMachine") = mMachine


                mMachineMaintenanceListForInst = MachineMaintenanceList.GetMachineMaintenanceList(MaintenanceActivityTypeID:=1) '1 for Inst
                Session("mMachineMaintenanceListForInst") = mMachineMaintenanceListForInst

                mFileAttachInst = FileAttach.NewAttachment(Guid.Empty, mAssemblyStatus.ID, Sort:=1) 'Sort = 1 : Installation
                Session("mFileAttachInst") = mFileAttachInst
                Session("mFileAttach") = mFileAttachInst

                mRegNo = mRemovedAssemblyStatusList(Index).MachineInfo
                mAssemblyType = mRemovedAssemblyStatusList(Index).AssemblyType
                mAssemblyInfo = mRemovedAssemblyStatusList(Index).AssemblyInfo
                mAssemblyDetail = "Reg No. : " + mRegNo + " Assembly Type : " + mAssemblyType + " Assembly Info : " + mAssemblyInfo
                MarkLog(Util.Action.Install, "AssemblyInstallation", mAssemblyDetail, Util.ErrorType.NoError, mRemovedAssemblyStatusList(Index).AssemblyStatusID, EventLogID)
                ''ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfInstallAssembly_Ajax.aspx?BackPage=Index.aspx');", True)

                txtAircraft.Text = mRegNo
                txtInstalledOnDate.Text = txtInstallationDate.Text
                cmbATAChapter.SelectedValue = mAssemblyStatusForRemoval.ATAID.ToString
                txtManufacturer.Text = mAssemblyStatusForRemoval.Assembly.Model.ManufacturerName
                txtModel.Text = mAssemblyStatusForRemoval.Assembly.Model.Name
                txtSerialNo.Text = mRemovedAssemblyStatusList(Index).SerialNo
                txtPosition.Text = mAssemblyStatusForRemoval.Position
                txtWorkOrNo.Text = ""
                dgInstallationValue.DataSource = mAssemblyStatus.AssemblyStatusPeriods
                dgInstallationValue.DataBind()
                dgRemovedAssemblyList.DataSource = mRemovedAssemblyStatusList
                Session("mRemovedAssemblyStatusList") = mRemovedAssemblyStatusList

                'Log(s)
                mLogList = LogList.GetLogList(New Guid(cmbAircraft.SelectedValue.ToString), txtInstallationDate.Text, Today.Date.ToString, False)
                Session("mLogListWizard") = mLogList
                dgLogList.DataSource = mLogList
                dgLogList.DataBind()
                UpnlLogList.Update()
                '****************************************
                Dim selectedRow As GridViewRow
                If Not previousSelectedForInst >= 0 Then
                    selectedRow = dgRemovedAssemblyList.Rows(previousSelectedForInst)
                    selectedRow.Style.Add("background-color", "#ffffff") 'change it back to original color

                End If

                selectedRow = dgRemovedAssemblyList.Rows(Index)
                selectedRow.Style.Add("background-color", "#FFCB60") 'change the color of the new row

                previousSelectedForInst = Index
                Session("previousSelectedForInst") = previousSelectedForInst

                '''  DataBind()



                txtSerialNo.ReadOnly = True
                txtSerialNo.BackColor = Color.FromName("#E0E0E0")


                If mMachine.IsUTC Then
                    dgLogList.Columns(6).Visible = True
                    dgLogList.Columns(9).Visible = True
                    dgLogList.Columns(5).Visible = False
                    dgLogList.Columns(8).Visible = False
                Else
                    dgLogList.Columns(6).Visible = False
                    dgLogList.Columns(9).Visible = False
                    dgLogList.Columns(5).Visible = True
                    dgLogList.Columns(8).Visible = True
                End If


                pnlInstallAssembly.Visible = True
                pnlInst.Visible = True
                upnlInstallAssembly.Update()
                mdlPopUpInstallAssembly.Show()
        End Select
    End Sub
    '''''Protected Sub txtAssemblyInstallationValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '''''    For i As Integer = 0 To mAssemblyStatus.AssemblyStatusPeriods.Count - 1
    '''''        Dim txtAssemblyInstallationValue As TextBox = CType(Me.dgInstallationValue.Rows(i).FindControl("txtAssemblyInstallationValue"), TextBox)


    '''''        If mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID = 2 Then
    '''''            If Period.IsDate(txtAssemblyInstallationValue.Text) Then
    '''''                mAssemblyStatus.AssemblyStatusPeriods.Item(i).AssemblyCurrentValueFormatted = Trim(txtAssemblyInstallationValue.Text)
    '''''            Else
    '''''                mAssemblyStatus.AssemblyStatusPeriods.Item(i).AssemblyCurrentValueFormatted = ""
    '''''            End If
    '''''        Else
    '''''            mAssemblyStatus.AssemblyStatusPeriods.Item(i).AssemblyCurrentValueFormatted = Trim(txtAssemblyInstallationValue.Text)
    '''''        End If
    '''''        'End of Added Code



    '''''    Next i
    '''''    Session("mAssemblyStatus") = mAssemblyStatus
    '''''    DataBindGrid()
    '''''End Sub
    ''Private Sub btnSelectFileInst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFileInst.Click
    ''    Session.Remove("mFileAttach")
    ''    If mFileAttachInst Is Nothing Then
    ''        mFileAttachInst = FileAttach.NewAttachment(Guid.Empty, mAssemblyStatus.ID, Sort:=1) 'Sort = 1 : Installation
    ''        Session("mFileAttachInst") = mFileAttachInst
    ''    End If
    ''    Session("mFileAttach") = mFileAttachInst
    ''    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "OpenFileDialog", "OpenFileDialog();", True)

    ''End Sub
    'Private Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
    '    Dim No As New Random
    '    Dim StrName As String = "abc" & No.Next.ToString

    '    GetAttachmentRemoval()

    '    If mFileAttachRemoval.Size > 0 Then
    '        Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttachRemoval.Extension
    '        Dim fs As FileStream
    '        If File.Exists(AppSettings("DOCPath")) = False Then
    '            'Delete File if exist
    '            System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttachRemoval.Extension)
    '            ' Create the file.
    '            fs = File.Create(path)
    '            '' Add some information to the file.
    '            fs.Write(mFileAttachRemoval.ImageFile, 0, mFileAttachRemoval.ImageFile.Length)
    '            fs.Close()
    '            Session("DOCPath") = path
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
    '        End If
    '    End If
    'End Sub
    'Private Sub ImageButton3_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton3.Click
    '    Dim No As New Random
    '    Dim StrName As String = "abc" & No.Next.ToString

    '    GetAttachmentInst()

    '    If mFileAttachInst.Size > 0 Then
    '        Dim path As String = AppSettings("DOCPath") & "\" & StrName & mFileAttachInst.Extension
    '        Dim fs As FileStream
    '        If File.Exists(AppSettings("DOCPath")) = False Then
    '            'Delete File if exist
    '            System.IO.File.Delete(AppSettings("DOCPath") & StrName & mFileAttachInst.Extension)
    '            ' Create the file.
    '            fs = File.Create(path)
    '            '' Add some information to the file.
    '            fs.Write(mFileAttachInst.ImageFile, 0, mFileAttachInst.ImageFile.Length)
    '            fs.Close()
    '            Session("DOCPath") = path
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
    '        End If
    '    End If
    'End Sub
    'Private Sub btnRemoveFileInst_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemoveFileInst.Click
    '    Dim fileSize1 As Integer = 0
    '    Dim file1(fileSize1) As Byte

    '    GetAttachmentInst()
    '    'mEmployee.ImageFile = file1
    '    'mEmployee.ImageSize = 0
    '    mFileAttachInst.ImageFile = file1
    '    mFileAttachInst.Size = 0

    '    ImageButton3.Visible = False
    '    btnRemoveFileInst.Enabled = False
    '    IsAttachmentInstDeleted = True
    '    Session("IsAttachmentInstDeleted") = IsAttachmentInstDeleted
    'End Sub
    Private Sub btnInstOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInstOk.Click
        If (Not User.IsInRole("AssemblyInstallationNew") And mAssemblyStatus.IsNew) Or (Not User.IsInRole("AssemblyInstallationEdit") And Not mAssemblyStatus.IsNew) Then
            'Added by Vikrant on 28-July-2011
            mAssemblyDetail = "Reg No. : " + cmbAircraft.SelectedItem.Text + " Model : " + txtModel.Text + " Serial No. : " + txtSerialNo.Text & " Installed On : " + txtInstalledOnDate.Text
            MarkLog(Util.Action.Save, "AssemblyInstallation", User.Identity.Name & " is not Authorized User to save " & mAssemblyDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            MSGBoxCtrlNEW.showbox(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        If IsValid Then
            If Not CustomValidate2() Then
                upnlValidationSummary.Update()
                Exit Sub
            End If

            'Added by Saylee on 18-Jul-2018 for ALL18072018-1 : Locking backdated installations on Comp and Assembly
            If mFromType = From.EditInstall And (mAssemblyStatus.IsRemoved = True) Then
                MSGBoxCtrlNEW.show("Installation Alert!", "Assembly detail(s) cannot be modified as it is removed. " & " Revert the Removal and then modify.", "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            '*******************************************************************

            'Added by Saylee on 19-Mar-2013 for ALL14032013-1
            If CheckPeriodsForRemovedAssemblyStatus(mAssemblyStatus) = False Then
                'Str = Str() + "Periods for selected " & mAssemblyStatus.AssemblyTypeName & " are mismatching with selected Installed On " & cmbMachineList.SelectedItem.Text & " Aircraft.Can not be installed."
                'Dim msg1 As New SIMsgBox(Page, "<BR>Assembly Status Installation Alert!", "<BR><BR>Periods for selected " & mAssemblyStatus.AssemblyTypeName & " are mismatching with selected Installed On " & cmbMachineList.SelectedItem.Text & " Aircraft.Can not be installed.", "", MsgBoxStyle.OKOnly)
                MSGBoxCtrlNEW.show("Assembly Status Installation Alert!", "Periods for selected " & mAssemblyStatus.AssemblyTypeName & " are mismatching with selected Installed On " & txtAircraft.Text & " Aircraft.Can not be installed.", "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            '***********************************

            If SetInstallation() = True Then

                Session("mAircraftInformationBoardList") = Nothing
                '*********************************
                MSGBoxCtrlNEW.show("Alert..!!", "Installation of Assembly is now fixed till you Submit", "", MsgBoxStyle.OkOnly, "")
                hdnInstAssembly.Value = "Installation of Assembly"
                mdlPopUpInstallAssembly.Hide()
                pnlInstallAssembly.Visible = False
                pnlInst.Visible = False

                upnlInstallAssembly.Update()

                'Log(s)
                mLogList = LogList.GetLogList(New Guid(cmbAircraft.SelectedValue.ToString), txtInstallationDate.Text, Today.Date.ToString, False)
                Session("mLogListWizard") = mLogList
                dgLogList.DataSource = mLogList
                dgLogList.DataBind()
                UpnlLogList.Update()
                '****************************************
            Else
                If CustomValidate2() = False Then
                    upnlValidationSummaryInst.Update()
                End If

            End If
        Else
            upnlValidationSummaryInst.Update()
        End If
    End Sub
    Private Sub lnkInstallNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkInstallNew.Click
        If Not IsValid Then Exit Sub



        If txtInstallationDate.Text = "" Then
            MSGBoxCtrlNEW.show("Alert..!!", "Please select Installation Date", "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        If CDate(calDate.Text) > CDate(txtInstallationDate.Text) Then
            MSGBoxCtrlNEW.show("Alert...!!!", "Please select Installation date greater than Removal Date " + calDate.Text, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        If CDate(calDate.Text) < CDate(txtInstallationDate.Text) Then
            ''To check whether there are Log between Removal date and Inst Date
            mLogList = LogList.GetLogList(New Guid(cmbAircraft.SelectedValue.ToString), calDate.Text, txtInstallationDate.Text, False)
            If mLogList.Count > 0 Then
                MSGBoxCtrlNEW.show("Alert...!!!", "This Removal/Installation will not be possible as there are Logs between dates of Removal and Installation", "", MsgBoxStyle.OkOnly, "")
                Session.Remove("mRemovedAssemblyStatusList")
                Exit Sub
            End If
        End If

        mMachine = Machine.GetMachine(New Guid(cmbAircraft.SelectedValue))
        Session("FromType") = 1 'NewInstall
        Session("IsExistingAssembly") = CType(False, Boolean)

        mAssemblyStatus = AssemblyStatus.NewInstallAssemblyStatus(Guid.NewGuid, New Guid(cmbAircraft.SelectedValue), txtInstallationDate.Text, mAssemblyStatusForRemoval.AssemblyTypeID, False)
        mAssemblyStatus.IsBackDatedInstallation = True

        Session("mMachine") = mMachine

        Session("mRemovedAssemblyStatus") = Nothing

        If mAssemblyStatus.AssemblyStatusPeriods.Count > 0 Then
            For i As Integer = mAssemblyStatus.AssemblyStatusPeriods.Count - 1 To 0 Step -1
                'If mAssemblyStatusPeriodtmp.PeriodID <> 1 And mAssemblyStatusPeriodtmp.PeriodID <> 2 Then
                '    mAssemblyStatus.AssemblyStatusPeriods.Remove(mAssemblyStatusPeriodtmp)
                'End If

                If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                    If mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 2 And mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 1 And mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 3 Then
                        mAssemblyStatus.AssemblyStatusPeriods.Remove(mAssemblyStatus.AssemblyStatusPeriods(i).ID)
                    End If
                Else
                    If mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 2 And mAssemblyStatus.AssemblyStatusPeriods(i).PeriodID <> 1 Then
                        mAssemblyStatus.AssemblyStatusPeriods.Remove(mAssemblyStatus.AssemblyStatusPeriods(i).ID)
                    End If
                End If
            Next
        End If

        For Each mAssemblyStatusPeriodtmp As AssemblyStatusPeriod In mAssemblyStatusForRemoval.AssemblyStatusPeriods
            If mAssemblyStatusPeriodtmp.PeriodID <> 1 And mAssemblyStatusPeriodtmp.PeriodID <> 2 Then
                Dim AssemblyStatusPeriodInst As AssemblyStatusPeriod
                AssemblyStatusPeriodInst = AssemblyStatusPeriod.NewChildAssemblyStatusPeriod(mAssemblyStatus.ID, mAssemblyStatus.MachineID, mAssemblyStatus.InstalledOnFormatted, mAssemblyStatus.AssemblyTypeID, mAssemblyStatusPeriodtmp.PeriodID, True)

                If Not mAssemblyStatus.AssemblyStatusPeriods.Contains(AssemblyStatusPeriodInst.PeriodID) Then
                    mAssemblyStatus.AssemblyStatusPeriods.Add(AssemblyStatusPeriodInst)
                    mAssemblyStatus.AssemblyStatusPeriods.Item(mAssemblyStatusPeriodtmp.PeriodID, "").AssemblyInstallationValueFormatted = ""
                End If

            End If

        Next

        Session("mAssemblyStatus") = mAssemblyStatus

        mFileAttachInst = FileAttach.NewAttachment(Guid.Empty, mAssemblyStatus.ID, Sort:=1) 'Sort = 1 : Installation
        Session("mFileAttachInst") = mFileAttachInst
        Session("mFileAttach") = mFileAttachInst

        If (Not User.IsInRole("AssemblyInstallationNew") And mAssemblyStatus.IsNew) Or (Not User.IsInRole("AssemblyInstallationEdit") And Not mAssemblyStatus.IsNew) Then
            MSGBoxCtrlNEW.showbox(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If


        MarkLog(Util.Action.[New], "AssemblyInstallation", mAssemblyStatusForRemoval.AssemblyTypeName, Util.ErrorType.NoError, Guid.Empty, EventLogID)

        mMachineMaintenanceListForInst = MachineMaintenanceList.GetMachineMaintenanceList(MaintenanceActivityTypeID:=1) '1 for Inst
        Session("mMachineMaintenanceListForInst") = mMachineMaintenanceListForInst

        txtAircraft.Text = cmbAircraft.SelectedItem.Text.ToString
        txtInstalledOnDate.Text = txtInstallationDate.Text
        cmbATAChapter.SelectedValue = mAssemblyStatusForRemoval.ATAID.ToString
        txtManufacturer.Text = mAssemblyStatusForRemoval.Assembly.Model.ManufacturerName
        txtModel.Text = mAssemblyStatusForRemoval.Assembly.Model.Name
        'txtSerialNo.Text = ""
        txtPosition.Text = mAssemblyStatusForRemoval.Position
        'txtWorkOrNo.Text = ""
        dgInstallationValue.DataSource = mAssemblyStatus.AssemblyStatusPeriods

        txtSerialNo.ReadOnly = False
        txtSerialNo.BackColor = Color.White

        DataBind()


        pnlInstallAssembly.Visible = True
        pnlInst.Visible = True
        upnlInstallAssembly.Update()
        mdlPopUpInstallAssembly.Show()
    End Sub
    Private Sub btnInstClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInstClose.Click

        mdlPopUpInstallAssembly.Hide()
        pnlInstallAssembly.Visible = False
        pnlInst.Visible = False

        upnlInstallAssembly.Update()

        If CType(Session("IsExistingAssembly"), Boolean) = True Then
            Dim selectedRow As GridViewRow = dgRemovedAssemblyList.Rows(previousSelectedForInst)
            selectedRow.Style.Add("background-color", "#ffffff")  'change it back to original color
        End If


        Session.Remove("mAssemblyStatusForRemoval")
        Session.Remove("mMachineMaintenanceForRemoval")
    End Sub
    Private Sub hdnSelectInstAssembly_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnSelectInstAssembly.Click
        MSGBoxCtrlNEW.show("Alert..!!", "Please Enter details for Assembly to be Installed", "", MsgBoxStyle.OkOnly, "")
    End Sub
#End Region


#Region " Submit "

    Private Sub btnsubmit_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsubmit.ServerClick


        If Not mAssemblyStatusForRemoval Is Nothing And Not mAssemblyStatus Is Nothing And Not mLogList Is Nothing Then
            Try
                '  Dim tmpmLogList As List(Of LogList.LogInfo) = New List(Of LogList.LogInfo)
                ' Dim tmpmLogList As LogList
                Dim tmpmLogList = (From c As LogList.LogInfo In mLogList
                               Select c Order By CDate(c.Date) Ascending, c.LogText Ascending, c.LogNo Ascending).ToList


                'Removal
                mAssemblyStatusForRemoval.Save()
                SaveMachineMaintenanceRem()
                SaveAttachmentRemoval()

                'New Inst
                mAssemblyStatus.Save()
                SaveMachineMaintenanceInst()
                SaveAttachmentInst()


                Dim i As Integer = 0

                Dim IsInsUpdated As Boolean = False
                Dim IsLogsUpdated As Boolean = False
                '  mLogList.Sort(CDate(DateFormatted), System.ComponentModel.ListSortDirection.Ascending)

               



                Dim PrevfinalValueWizard As Decimal = 0
                Dim tmpPrevLog As Log
                Dim isForfirstLog As Boolean = False

                For Each LogtempInfo As LogList.LogInfo In tmpmLogList
                    Dim tmplog As Log = Log.GetLog(LogtempInfo.ID)
                    mMachineMaintenanceForRemoval = Session("mMachineMaintenanceForRemoval")
                    If tmplog.LogNo > mMachineMaintenanceForRemoval.LogNo And tmplog.ID <> mMachineMaintenanceForRemoval.LogID Then

                        Dim tmpUpdateLogAssembly As LogAssembly
                        If Not tmplog.ID.Equals(mMachineMaintenanceForRemoval.LogID) Then
                            tmpUpdateLogAssembly = Nothing
                            '  Dim j As Integer = 0
                            i = i + 1

                            For Each tmpLogAssembly As LogAssembly In tmplog.ALL_LogAssemblies

                                If tmpLogAssembly.ModelName = mAssemblyStatusForRemoval.ModelNameWithPosition And tmpLogAssembly.SerialNo = mAssemblyStatusForRemoval.Assembly.SerialNo Then
                                    For Each tmpLogPeriod As LogPeriod In tmpLogAssembly.LogPeriods
                                        If mAssemblyStatusForRemoval.AssemblyStatusPeriods.Contains(tmpLogPeriod.AssemblyStatusPeriodID) Then
                                            tmpLogPeriod.SubmitLogPeriodForWizard(tmpLogPeriod, tmplog.ID, tmpLogPeriod.AssemblyStatusPeriodID, mAssemblyStatus.AssemblyStatusPeriods(tmpLogPeriod.PeriodID, "").ID)
                                        End If

                                    Next
                                    IsInsUpdated = True
                                    tmpUpdateLogAssembly = tmpLogAssembly
                                    Exit For
                                End If
                                ''j = j + 1
                            Next


                            If i = 1 Then
                                isForfirstLog = True
                            Else
                                isForfirstLog = False
                            End If

                            If IsInsUpdated = True Then
                                'UpdatePeriods
                                '' Dim tmpUpdateLogAssembly As LogAssembly = tmplog.ALL_LogAssemblies(j)
                                tmplog = Log.GetLog(LogtempInfo.ID)

                                For Each tmpLogAssembly As LogAssembly In tmplog.ALL_LogAssemblies
                                    If tmpLogAssembly.ModelName = mAssemblyStatus.ModelNameWithPosition And tmpLogAssembly.SerialNo = mAssemblyStatus.Assembly.SerialNo Then
                                        For Each tmpLogPeriod As LogPeriod In tmpLogAssembly.LogPeriods
                                            If mAssemblyStatus.AssemblyStatusPeriods.Contains(tmpLogPeriod.AssemblyStatusPeriodID) Then
                                                tmpLogPeriod.UpdateLogPeriodsForWizard(tmpLogPeriod, tmplog.ID, mAssemblyStatus.AssemblyID, mAssemblyStatus.AssemblyStatusPeriods(tmpLogPeriod.PeriodID, "").AssemblyCurrentValueDec, isForfirstLog)
                                                IsLogsUpdated = True
                                            End If
                                        Next
                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                    End If
                Next


                If IsInsUpdated = True And IsLogsUpdated = True Then
                    previousSelectedForRemoval = -1
                    Session("previousSelectedForRemoval") = previousSelectedForRemoval

                    previousSelectedForInst = -1
                    Session("previousSelectedForInst") = previousSelectedForInst
                    Session("MiddleFrame") = ""

                    MSGBoxCtrlNEW.Show("Successful...!!!", "Congratulation!!! You have successfully replaced Assembly..!!!", "", MsgBoxStyle.OkOnly, "")




                End If
            Catch ex As Exception
                Throw ex
            Finally
                mAssemblyStatusForRemoval = Nothing
                mAssemblyStatus = Nothing
                '  mLogList = Nothing
                ' Response.Redirect("Dashboard.aspx")
            End Try
        Else
            MSGBoxCtrlNEW.Show("Failed...!!!", "Sorry!!! Assembly replacement failed may be due to no Installation/Removal/Log Details provided..!!!", "", MsgBoxStyle.OkOnly, "")

        End If

    End Sub
#End Region


#Region "Service Methods"
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetLicenceList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        'Dim itemlist As ItemListAutoComplete
        'itemlist = ItemListAutoComplete.GetItemList(prefixText, False)

        Dim mLicenses As LicenseNoListWithEmployee = LicenseNoListWithEmployee.GetLicenseNoList(prefixText, "", , , False)
        If count = 0 Then
            Return (From c As LicenseNoListWithEmployee.LicenseNoListWithEmployeeInfo In mLicenses
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.LicenseNoEmpName, c.EmpID.ToString())).ToArray
        Else
            Return (From c As LicenseNoListWithEmployee.LicenseNoListWithEmployeeInfo In mLicenses
               Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.LicenseNoEmpName, c.EmpID.ToString())).Take(count).ToArray
        End If
    End Function
#End Region



 
  
   
  
End Class