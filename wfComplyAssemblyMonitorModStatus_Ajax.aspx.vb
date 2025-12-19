Imports System.Linq
Imports System.Text 'Added By Vikrant On 17-Sep-2020 For Mismatch Value Mail Send
Public Class wfComplyAssemblyMonitorModStatus_Ajax
    Inherits System.Web.UI.Page

#Region " Enumeration "
    Enum MaintenanceActivityTypes
        RemovalComp = 1
        InstallComp = 2
        RemovalAssembly = 3
        InstallAssembly = 4
        AssemblyService = 5
        AssemblyInspection = 6
        AssemblyDirective = 7
        ComponentService = 8
        ComponentInspection = 9
        ComponentDirective = 10
    End Enum
#End Region

#Region " Variable Declaration "
    Public mAssemblyMonitorModStatus As AssemblyMonitorModStatus
    Public mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus
    Public mAssemblyStatus As AssemblyStatus
    Public mMachine As Machine
    Dim Flag As Int16
    Public mAssemblyInfo As String                              'Code Added Jan,25,2007
    Dim LogID As String
    Public mBoardInfo As AircraftInformationBoard.BoardInfo     'Added by Saylee on 22-May-2009
    Public mMachineMaintenance As MachineMaintenance            'Added by Saylee on 9th-Oct-2009
    Public mMachineMaintenanceList As MachineMaintenanceList    'Added by Saylee on 9th-Oct-2009
    Dim EventLogID As Guid                                      'Added by vikrant on 27-July-2011
    Public mRegNo As String
    Public mDirectiveDetail As String
    Public mAircraft As String
    Public mMonitorInfo As String
    Public mMonitorType As String
    Public mDirectiveNo As String
    Public mLinkMaintenanceList As LinkMaintenanceList          'Added By Utkarsh ON 07-Feb-2012 FOR Link Maintenance
    Public mLinkMaintenance As LinkMaintenance
    Public mMultiComplianceList As New MultiComplianceList
    Public mAssemblyMonitorServiceStatusForLM As AssemblyMonitorServiceStatus
    Public mAssemblyMonitorInspStatusForLM As AssemblyMonitorInspStatus
    Public mAssemblyMonitorModStatusForLM As AssemblyMonitorModStatus
    Public mLinkMaintenanceMonitorStatus As LinkMaintenaceMonitorStatus
    Public PeriodValues(,) As String                            'End 
    Public mEmployeeStatus As EmployeeStatus                    'Added By Vikrant On 06-Aug-2013 For ALL01082013
    Dim mFileAttach As FileAttach                               'Added By Vikrant On 25-Nov-2014
    Dim IsAttachmentDeleted As Boolean = False                  'End
    'MLNo
    Dim LicenseNo As String = String.Empty
    Dim EmpName As String = String.Empty
    Dim DoneByID As Guid = Guid.Empty
    Dim mMaintenanceDoneByEmployees As New MaintenanceDoneByEmployees
    Shared UserNameForLicenceList As String
    'End
    Public OverDueString As String = ""
    Dim mModuleList As ModuleList
    Public mIsSpareAssembly As Integer 'Added By Vikrant On 27-Jul-2020 For ALL27072020
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mAssemblyMonitorModStatus = CType(Session("mAssemblyMonitorModStatus"), AssemblyMonitorModStatus)
        mPrevAssemblyMonitorModStatus = CType(Session("mPrevAssemblyMonitorModStatus"), AssemblyMonitorModStatus)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mMachine = CType(Session("mMachine"), Machine)
        LogID = CType(Session("LogID"), String)
        mBoardInfo = Session("mBoardInfo")                                              'Added by Saylee on 22-May-2009
        mAssemblyInfo = Session("mAssemblyInfo")                                        'Added by Saylee on 04-Aug-2009
        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 9th-Oct-2009
        mMachineMaintenanceList = CType(Session("mMachineMaintenanceList"), MachineMaintenanceList) 'Added by Saylee on 9th-Oct-2009
        mMultiComplianceList = Session("mMultiComplianceList")                          'Added By Utkarsh ON 15-Mar-2012 FOR Link Maintenance
        mFileAttach = Session("mFileAttach")                                            'Added By Vikrant On 25-Nov-2014
        IsAttachmentDeleted = Session("IsAttachmentDeleted")                            'End
        'MLNo
        mMaintenanceDoneByEmployees = Session("mMaintenanceDoneByEmployees")
        UserNameForLicenceList = Session("UserNameForLicenceList")
        'End
        mModuleList = Session("mModuleList")
        mIsSpareAssembly = Session("mIsSpareAssembly") 'Added By Vikrant On 27-Jul-2020 For ALL27072020
    End Sub
    Private Sub SetSession()
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
        Session("mPrevAssemblyMonitorModStatus") = mPrevAssemblyMonitorModStatus
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mMachine") = mMachine
        Session("mBoardInfo") = mBoardInfo                              'Added by Saylee on 22-May-2009
        Session("mAssemblyInfo") = mAssemblyInfo                        'Added by Saylee on 04-Aug-2009
        Session("mMachineMaintenance") = mMachineMaintenance            'Added by Saylee on 9th-Oct-2009
        Session("mMachineMaintenanceList") = mMachineMaintenanceList    'Added by Saylee on 9th-Oct-2009
        Session("mFileAttach") = mFileAttach                            'Added By Vikrant On 25-Nov-2014
        Session("IsAttachmentDeleted") = IsAttachmentDeleted            'End
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAssemblyMonitorModStatus")
        Session.Remove("mMachineMaintenance")       'Added by Saylee on 9th-Oct-2009
        Session.Remove("mMachineMaintenanceList")   'Added by Saylee on 9th-Oct-2009
        Session.Remove("mMultiComplianceList")      'Added By Utkarsh ON 15-Mar-2012 FOR Link Maintenance
        Session.Remove("mFileAttach")               'Added By Vikrant On 25-Nov-2014
        Session.Remove("IsAttachmentDeleted")       'End
        'Added by Vikrant on 14-Mar-2016 for ALL11032016
        Session.Remove("ConsiderAssemblyInstValue")
        Session.Remove("mFirstLogDetailAfterAssemblyInstallation")
        'End
        Session.Remove("mLinkMaintenanceList")
        'MLNo
        Session.Remove("mMaintenanceDoneByEmployees")
        Session.Remove("UserNameForLicenceList")
        'End
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub

    Private Sub SetObject()

        Try

            With mAssemblyMonitorModStatus

                Dim LicenseNo As String = String.Empty
                Dim EmpName As String = String.Empty

                If Not IsDate(txtDoneOnDate.Text) Then
                    .DoneOn = System.DBNull.Value
                Else
                    .DoneOn = txtDoneOnDate.Text
                End If

                .DoneWONo = Trim(txtWorkOrderNo.Text)
                .DoneRemark = Trim(txtRemark.Text)
                .RequiredManHours = Trim(txtRequiredManHours.Text)

                If Not IsDate(txtExtensionDate.Text) Then
                    .ExtensionDate = System.DBNull.Value
                Else
                    .ExtensionDate = txtExtensionDate.Text
                End If

                .ApprovalRemark = txtApprovalRemark.Text
                .IsApplicable = chkApplicable.Checked   'Added By Vaishali on 19-Nov-2008

                If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
                    LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
                    EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2,
                                  txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
                Else
                    LicenseNo = Trim(txtLicenceNo.Text)
                End If

                .LicenseNo = LicenseNo
                .DoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID
                .Place = txtPlace.Text.Trim  'Added by Shweta on 26th-Apr-2012
                .SourceDoc = Trim(txtSourceDoc.Text)
                .RevisionNo = Trim(txtRevisionNo.Text)
                .BookNo = Trim(txtBookNo.Text)
                .PageNo = Trim(txtPageNo.Text)

                If Not mFileAttach Is Nothing Then

                    If mFileAttach.Size > 0 Then 'Added By Vikrant On 25-Nov-2014
                        .IsAttachmentAdded = True
                    Else
                        .IsAttachmentAdded = False
                    End If 'End

                End If

                .MethodOfCompliance = Trim(txtMethodOfCompliance.Text) 'Added By Harsh on 10-Oct-2024

            End With

            Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Public Sub SetGridObject()
        Dim txtCurrentValue, txtExtensionValue As TextBox
        Dim j As Int32
        ReDim PeriodValues(dgDoneOnValue.Rows.Count - 1, 1)             'Actual Size   (dgDoneOnValue.Rows.Count , 2)'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
        For j = 0 To Me.dgDoneOnValue.Rows.Count - 1
            txtCurrentValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtCurrentValue"), TextBox)
            txtExtensionValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtExtensionValue"), TextBox) 'Added By Saylee on 28-07-2008
            With mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
                If .Item(j).PeriodID = 2 Then
                    If Not Period.IsDate(txtCurrentValue.Text) Then
                        .Item(j).CurrentValue = ""
                    Else
                        .Item(j).CurrentValueFormatted = Trim(txtCurrentValue.Text)
                        PeriodValues(j, 0) = .Item(j).PeriodUnitID      'To Check same Period'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
                        PeriodValues(j, 1) = Trim(txtCurrentValue.Text) 'Period Value 'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
                    End If
                Else
                    .Item(j).CurrentValue = Trim(txtCurrentValue.Text)
                    PeriodValues(j, 0) = .Item(j).PeriodUnitID          'To Check same Period'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
                    PeriodValues(j, 1) = Trim(txtCurrentValue.Text)     'Period Value 'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
                End If
                .Item(j).ExtensionValue = Trim(txtExtensionValue.Text)  'Added By Saylee on 28-07-2008
            End With
        Next j
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
    End Sub
    Public Function CustomeValidateGridValuesForOverDue() As String   'Code for OverDue 'Added by Saylee on 26-Mar-2019 for ALL26032019
        Dim txtCurrentValue, txtExtensionValue As TextBox
        Dim j As Int32

        Dim OverDueString As String = ""
        Dim NextDueString As String = ""
        Dim DiffString As String = ""


        For j = 0 To Me.dgDoneOnValue.Rows.Count - 1
            txtCurrentValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtCurrentValue"), TextBox)
            'Added By Saylee on 28-07-2008
            txtExtensionValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtExtensionValue"), TextBox)
            With mPrevAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods ''mPrevAssemblyMonitorModStatus object contains previous period values
                If .Item(j).PeriodID = 2 Then
                    If Not Period.IsDate(txtCurrentValue.Text) Then

                    Else
                        Dim mCurrentValueOverDue As New Period(.Item(j).PeriodID, DBNull.Value, .Item(j).PeriodUnitID, True)
                        mCurrentValueOverDue.Value = Trim(txtCurrentValue.Text)

                        Dim mDueOnPrevious As New Period(.Item(j).PeriodID, DBNull.Value, .Item(j).PeriodUnitID, True)
                        mDueOnPrevious.Value = .Item(j).DueOnValue

                        If New SmartDate(txtCurrentValue.Text).Date > New SmartDate(.Item(j).DueOnValueFormatted).Date Then
                            'If OverDueString = "" Then
                            '    OverDueString = "Over due Date " + txtCurrentValue.Text + " as its due date was on " + .Item(j).DueOnValueFormatted
                            'Else
                            '    OverDueString = OverDueString + " ," + "Over Due Date " + txtCurrentValue.Text + " as its due date was on " + .Item(j).DueOnValueFormatted
                            'End If
                            If OverDueString = "" Then
                                OverDueString = txtCurrentValue.Text
                                NextDueString = .Item(j).DueOnValueFormatted
                                DiffString = New Period(.Item(j).PeriodID, mCurrentValueOverDue.DbValueDec - mDueOnPrevious.DbValueDec, mCurrentValueOverDue.PeriodUnitID, False, True).TextFormatted
                            Else
                                OverDueString = OverDueString + vbCrLf + txtCurrentValue.Text
                                NextDueString = NextDueString + vbCrLf + .Item(j).DueOnValueFormatted
                                DiffString = DiffString + vbCrLf + New Period(.Item(j).PeriodID, mCurrentValueOverDue.DbValueDec - mDueOnPrevious.DbValueDec, mCurrentValueOverDue.PeriodUnitID, False, True).TextFormatted
                            End If
                        End If
                    End If
                Else
                    Dim mCurrentValueOverDue As New Period(.Item(j).PeriodID, DBNull.Value, .Item(j).PeriodUnitID)
                    mCurrentValueOverDue.Value = Trim(txtCurrentValue.Text)

                    Dim mDueOnPrevious As New Period(.Item(j).PeriodID, DBNull.Value, .Item(j).PeriodUnitID)
                    mDueOnPrevious.Value = .Item(j).DueOnValue
                    If mCurrentValueOverDue.DbValueDec > mDueOnPrevious.DbValueDec Then

                        'If OverDueString = "" Then
                        '    OverDueString = "Over due " + mCurrentValueOverDue.PeriodName + " " + txtCurrentValue.Text + " as its due " + mCurrentValueOverDue.PeriodName + " was " + .Item(j).DueOnValueFormatted
                        'Else
                        '    OverDueString = OverDueString + " ," + " Over Due " + mCurrentValueOverDue.PeriodName + " " + txtCurrentValue.Text + " as its due " + mCurrentValueOverDue.PeriodName + " was " + .Item(j).DueOnValueFormatted
                        'End If
                        If OverDueString = "" Then
                            OverDueString = New Period(.Item(j).PeriodID, mCurrentValueOverDue.DbValueDec, mCurrentValueOverDue.PeriodUnitID).TextFormatted
                            NextDueString = New Period(.Item(j).PeriodID, mDueOnPrevious.DbValueDec, mDueOnPrevious.PeriodUnitID).TextFormatted
                            DiffString = New Period(.Item(j).PeriodID, mCurrentValueOverDue.DbValueDec - mDueOnPrevious.DbValueDec, mCurrentValueOverDue.PeriodUnitID).TextFormatted
                        Else
                            OverDueString = OverDueString + vbCrLf + New Period(.Item(j).PeriodID, mCurrentValueOverDue.DbValueDec, mCurrentValueOverDue.PeriodUnitID).TextFormatted
                            NextDueString = NextDueString + vbCrLf + New Period(.Item(j).PeriodID, mDueOnPrevious.DbValueDec, mDueOnPrevious.PeriodUnitID).TextFormatted
                            DiffString = DiffString + vbCrLf + New Period(.Item(j).PeriodID, mCurrentValueOverDue.DbValueDec - mDueOnPrevious.DbValueDec, mCurrentValueOverDue.PeriodUnitID).TextFormatted
                        End If

                    End If
                End If
            End With
        Next j

        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus

        'Dont change this format as its used in Over Due Report to show these values on report
        If OverDueString <> "" Then
            OverDueString = "Over Due: " + OverDueString
            Session("OverDueString") = OverDueString
            Return "Actual Due: " + NextDueString + "<br>" + "Cross Due: " + DiffString
        Else
            Return ""
        End If

    End Function
    Public Sub SetGridFromObject()
        Dim j As Int32
        ReDim PeriodValues(mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count - 1, 1)             'Actual Size   (dgDoneOnValue.Rows.Count , 2)'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
        For j = 0 To mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count - 1
            With mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
                If .Item(j).PeriodID = 2 Then
                    If Not Period.IsDate(mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted) Then
                        .Item(j).CurrentValue = ""
                    Else
                        .Item(j).CurrentValueFormatted = Trim(mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted)
                        PeriodValues(j, 0) = .Item(j).PeriodUnitID      'To Check same Period'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
                        PeriodValues(j, 1) = Trim(mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted) 'Period Value 'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
                    End If
                Else
                    .Item(j).CurrentValue = Trim(mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted)
                    PeriodValues(j, 0) = .Item(j).PeriodUnitID          'To Check same Period'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
                    PeriodValues(j, 1) = Trim(mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted)     'Period Value 'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
                End If
                .Item(j).ExtensionValue = Trim(mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted)  'Added By Saylee on 28-07-2008
            End With
        Next j
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
    End Sub
    Private Sub SetLog()
        If Val(Request.QueryString("Type")) = -1 Then
            Dim LogId As Guid = New Guid(Request.QueryString("LogId"))
            Dim LogDate = Request.QueryString("LogDate")
            Dim clnAssemblyMonitorModStatus As AssemblyMonitorModStatus = mAssemblyMonitorModStatus.Clone
            If Session("From") = 0 Then
                mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewComplyAssemblyMonitorModStatus(Guid.NewGuid, mPrevAssemblyMonitorModStatus.AssemblyID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, LogDate, mAssemblyStatus.Assembly.ModelID, mPrevAssemblyMonitorModStatus.ModelMonitorMod, LogId, mPrevAssemblyMonitorModStatus.DoneOn.ToString, mMachine.HourType)
            End If
            mAssemblyMonitorModStatus.DoneWONo = clnAssemblyMonitorModStatus.DoneWONo
            mAssemblyMonitorModStatus.DoneRemark = clnAssemblyMonitorModStatus.DoneRemark
            mAssemblyMonitorModStatus.DoneOn = clnAssemblyMonitorModStatus.DoneOn
            mAssemblyMonitorModStatus.RequiredManHours = clnAssemblyMonitorModStatus.RequiredManHours
            mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods = clnAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
            mAssemblyMonitorModStatus.DoneByID = clnAssemblyMonitorModStatus.DoneByID
            mAssemblyMonitorModStatus.LicenseNo = clnAssemblyMonitorModStatus.LicenseNo
            mAssemblyMonitorModStatus.Place = clnAssemblyMonitorModStatus.Place
            mAssemblyMonitorModStatus.IsAttachmentAdded = clnAssemblyMonitorModStatus.IsAttachmentAdded 'Added By Vikrant On 25-Nov-2014
            'Added By Vikrant on 15-Apr-2021 to solve issue: Licence No not getting saved after select log
            For j As Integer = mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Count - 1 To 0 Step -1
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.RemoveAt(j)
            Next
            For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In clnAssemblyMonitorModStatus.MaintenanceDoneByEmployees
                If Not mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.ID) Then
                    mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Add(mAssemblyMonitorModStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
                Else
                    If Not mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.EmployeeID) Then
                        mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).EmployeeID = mMaintenanceDoneByEmployee.EmployeeID
                        mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).LicenceNo = mMaintenanceDoneByEmployee.LicenceNo
                        mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).RequiredManHours = mMaintenanceDoneByEmployee.RequiredManHours
                        mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).EmployeeName = mMaintenanceDoneByEmployee.EmployeeName
                    End If
                End If
            Next
            'End
            If Not mFileAttach Is Nothing Then
                mFileAttach.ReferenceID = mAssemblyMonitorModStatus.ID
                Session("mFileAttach") = mFileAttach
            End If 'End
            Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
            clnAssemblyMonitorModStatus = Nothing
            Dim mLog As Log 'Added by Saylee on 9th-Oct-2009
            mLog = Log.GetLog(New Guid(LogId.ToString))
            Session("mLog") = mLog '===================================
        End If
    End Sub
    Private Sub NewRecord(ByVal LogID As Guid, ByVal LogDate As String)
        Dim mAssemblyStatusList As AssemblyStatusList
        Dim mMachineList As MachineList
        Dim LatestRemovedOn As SmartDate
        Dim AssemblyStatusID As Guid = Guid.Empty


        If mAssemblyStatus.IsSpareAssembly = False Then


            mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(LogDate, mAssemblyStatus.MachineID.ToString _
            , , , , , , , , , , True, , , mAssemblyStatus.AssemblyID.ToString, , , , , , , , , , , , , , _
            , , SkipIsForInventoryAircarft:=True, MonitoringInspRequired:=False, MonitoringModRequired:=False, _
            MonitoringServiceRequired:=False, CompMonitoringInspRequired:=False, CompMonitoringModRequired:=False, _
            CompMonitoringServiceRequired:=False).Item(0), MachineInfo).AssemblyStatusList

            If mAssemblyStatusList.Count = 0 Then
                mMachineList = MachineList.GetMachineListWithRemoval(LogDate, Guid.Empty.ToString _
                       , , , , , , , , , , True, , , mAssemblyStatus.AssemblyID.ToString, SkipIsForInventoryAircarft:=True)
                For i As Integer = 0 To mMachineList.Count - 1
                    If mMachineList(i).AssemblyStatusList.Count > 0 Then
                        Dim mtempAssemblyList = (From AssemblyStatusInfo As AssemblyStatusInfo In mMachineList(i).AssemblyStatusList
                                                            Order By CDate(AssemblyStatusInfo.RemovedOn) Descending
                                                            Select AssemblyStatusInfo).ToList
                        If AssemblyStatusID.Equals(Guid.Empty) Then
                            AssemblyStatusID = mtempAssemblyList(0).ID
                            LatestRemovedOn = New SmartDate(mtempAssemblyList(0).RemovedOn.ToString)
                        ElseIf LatestRemovedOn.CompareTo(New SmartDate(mtempAssemblyList(0).RemovedOn.ToString)) < 0 Then
                            AssemblyStatusID = mtempAssemblyList(0).ID
                            LatestRemovedOn = mtempAssemblyList(0).RemovedOn
                        End If
                    End If
                Next
            Else
                AssemblyStatusID = mAssemblyStatusList(0).ID
            End If
            'End

            'Here instead of mPrevAssemblyMonitorModStatus.AssemblyStatusID pass mAssemblyStatusList(0).ID  
            'mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewComplyAssemblyMonitorModStatus(Guid.NewGuid, mPrevAssemblyMonitorModStatus.AssemblyID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, LogDate, mAssemblyStatus.Assembly.ModelID, mPrevAssemblyMonitorModStatus.ModelMonitorMod, LogID, mPrevAssemblyMonitorModStatus.DoneOn.ToString, mMachine.HourType)

            'Commented on 07-Aug-2020 by Shital as previous(last) effective date carried forward for all nexts comply activity
            ' mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewComplyAssemblyMonitorModStatus(Guid.NewGuid, mPrevAssemblyMonitorModStatus.AssemblyID, AssemblyStatusID, LogDate, mAssemblyStatus.Assembly.ModelID, mPrevAssemblyMonitorModStatus.ModelMonitorMod, LogID, mPrevAssemblyMonitorModStatus.DoneOn.ToString, mMachine.HourType, CType(Session("ConsiderAssemblyInstValue"), Boolean))
            mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewComplyAssemblyMonitorModStatus(Guid.NewGuid, mPrevAssemblyMonitorModStatus.AssemblyID, AssemblyStatusID, LogDate, mAssemblyStatus.Assembly.ModelID, mPrevAssemblyMonitorModStatus.ModelMonitorMod, LogID, mPrevAssemblyMonitorModStatus.AsOnDate.ToString, mMachine.HourType, CType(Session("ConsiderAssemblyInstValue"), Boolean))
        Else
            mAssemblyMonitorModStatus = AssemblyMonitorModStatus.NewComplyAssemblyMonitorModStatus(Guid.NewGuid, mPrevAssemblyMonitorModStatus.AssemblyID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, LogDate, mAssemblyStatus.Assembly.ModelID, mPrevAssemblyMonitorModStatus.ModelMonitorMod, LogID, LogDate, mMachine.HourType)

        End If
        mAssemblyMonitorModStatus.BeginEdit()
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
        SetTitle()
    End Sub
    Private Sub EditRecord(ByVal LogID As Guid, ByVal DoneOnDate As String, ByVal FromEntry As Boolean)
        REM:-FromEntry is used for avoiding object Dirty at form load when we r coming thru' Edit.
        If FromEntry = False Then
            mAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetComplyAssemblyMonitorModStatus(mPrevAssemblyMonitorModStatus.ID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, DoneOnDate, LogID, mMachine.HourType, CType(Session("ConsiderAssemblyInstValue"), Boolean))
        Else
            mAssemblyMonitorModStatus = AssemblyMonitorModStatus.GetComplyAssemblyMonitorModStatusFromEntry(mPrevAssemblyMonitorModStatus.ID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, DoneOnDate, mMachine.HourType, CType(Session("ConsiderAssemblyInstValue"), Boolean))
        End If
        mAssemblyMonitorModStatus.BeginEdit()
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
        SetTitle()
    End Sub
    Private Sub SetFromClone(ByVal clnAssemblyMonitorModStatus As AssemblyMonitorModStatus)
        mAssemblyMonitorModStatus.DoneWONo = clnAssemblyMonitorModStatus.DoneWONo
        mAssemblyMonitorModStatus.DoneRemark = clnAssemblyMonitorModStatus.DoneRemark
        mAssemblyMonitorModStatus.DoneByID = clnAssemblyMonitorModStatus.DoneByID
        mAssemblyMonitorModStatus.LicenseNo = clnAssemblyMonitorModStatus.LicenseNo
        mAssemblyMonitorModStatus.Place = clnAssemblyMonitorModStatus.Place
        mAssemblyMonitorModStatus.IsAttachmentAdded = clnAssemblyMonitorModStatus.IsAttachmentAdded 'Added By Vikrant On 25-Nov-2014
        'MLNo
        For j As Integer = mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Count - 1 To 0 Step -1
            mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.RemoveAt(j)
        Next
        For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In clnAssemblyMonitorModStatus.MaintenanceDoneByEmployees
            If Session("From") = 0 Then 'New Record
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Add(mAssemblyMonitorModStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
            ElseIf Session("From") = 1 Then 'Edit Record
                If Not mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.EmployeeID) Then
                    mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Add(mAssemblyMonitorModStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
                End If
            End If
        Next
        'End
        If Not mFileAttach Is Nothing Then
            mFileAttach.ReferenceID = mAssemblyMonitorModStatus.ID
            Session("mFileAttach") = mFileAttach
        End If 'End
        clnAssemblyMonitorModStatus = Nothing
    End Sub
    Private Sub SaveBoardInfo() 'Added by Saylee on 22-May-2009
        Dim mAssemblyMonitorModStatusPeriod As AssemblyMonitorModStatusPeriod
        Dim DueOnValue As String
        'Condition added by Saylee on 29-June-2009 to show DueOnValue blank for One time record
        If (mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 1 And Not mAssemblyMonitorModStatus.DoneOn Is DBNull.Value) Or (mAssemblyMonitorModStatus.IsApplicable = False) Then
            DueOnValue = ""
        Else
            For Each mAssemblyMonitorModStatusPeriod In mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
                If mAssemblyMonitorModStatusPeriod.PeriodID = 2 Then
                    DueOnValue = DueOnValue + "<BR>" + mAssemblyMonitorModStatusPeriod.DueOnValueFormatted
                Else
                    DueOnValue = DueOnValue + "<BR>" + mAssemblyMonitorModStatusPeriod.DueOnValueTextFormatted
                End If
            Next
        End If
        mBoardInfo = Session("mBoardInfo")

        If Not mBoardInfo.MonitorID.Equals(Guid.Empty) Then
            mBoardInfo.MonitorID = mAssemblyMonitorModStatus.ID
            mBoardInfo.DueOnValue = DueOnValue
            mBoardInfo.ApplyEdit()
            mBoardInfo.Save()
            Session("mBoardInfo") = mBoardInfo
        End If
        Session("mAircraftInformationBoardList") = Nothing
    End Sub
	'Added By Vikrant On 17-Sep-2020 For Mismatch Value Mail Send
	Private Sub SendMail(ByVal ModStatus As AssemblyMonitorModStatus, ByVal DoneOnValue As String, ByVal DoneOnValueObj As String, Optional ByVal OnlyEdited As Boolean = False, Optional ByVal ToMailIDs As String = "saylee@bytzsoft.com")
		Dim str As New StringBuilder
		Try


			If OnlyEdited = False Then
				str.Append("Mismatch Details for <b>" & IIf(Session("From") = 1, "Edited and Saved", IIf(ModStatus.IsNew, "New", "New but Saved")) & "</b> record are as follows: ")
			Else
				'' str.Append("Mismatch Details for <b>" & IIf(Session("From") = 1, "Only Edited", IIf(ModStatus.IsNew, "New", "New but Saved")) & "</b> record are as follows: ")
			End If

			str.Append("<p><b>Assembly Details: </b> " & mAssemblyStatus.Assembly.ModelName & " " & mAssemblyStatus.Assembly.SerialNo & "</p>")
			str.Append("<p><b>Directive ID: </b> " & ModStatus.ID.ToString & "</p>")
			str.Append("<p><b>Directive Number: </b> " & ModStatus.ModelMonitorMod.Number & "</p>")
			str.Append("<p><b>Directive Description: </b> " & ModStatus.ModelMonitorMod.Description & "</p>")
			str.Append("<p><b>Done On Date: </b> " & txtDoneOnDate.Text & "</p>")
			str.Append("<p><b>Done On Value: </b> " & DoneOnValue & "</p>")
			str.Append("<p><b>Done On Date(obj.): </b> " & ModStatus.DoneOnFormatted.ToString & "</p>")
			str.Append("<p><b>Done On Value(obj.): </b> " & DoneOnValueObj & "</p>")
			str.Append("<p><b>Saved By: </b> " & User.Identity.Name)

			SendMailFile.SendMailFile(Nothing, User.Identity.Name, "FAS: Assembly Directive Done on Date Done on Value Mismatch Details", "", Info:=str.ToString, VendorEmailID:="", ToMailID:=ToMailIDs)
		Catch ex As Exception
			Dim Title As String = "Error Sending Mail"
			Dim Message As String = ex.InnerException.ToString
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(Title, Message, , False), True)
			Exit Sub
		End Try
	End Sub
	'End
	Private Function Save() As Boolean
        Dim clnAssemblyMonitorModStatus As AssemblyMonitorModStatus
        clnAssemblyMonitorModStatus = CType(mAssemblyMonitorModStatus.Clone, AssemblyMonitorModStatus)
        SetObject()
        SetGridObject()
        SetMachineMaintenanceObject() 'Added by Saylee on 9th-Oct-2009
        If mAssemblyMonitorModStatus.IsValid Then
            If mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodRequired, MSGBox.Message_text.PeriodRequired, "You are trying to save Assembly Directives Status.Assembly Directives Status can not be saved without period units.", MsgBoxStyle.OkOnly, "")
                Return False
            End If
            Try
                'Added By Vikrant On 06-Aug-2013 For ALL01082013
                If Not mAssemblyMonitorModStatus.DoneByID.Equals(Guid.Empty) AndAlso Not mAssemblyMonitorModStatus.DoneOn.Equals(System.DBNull.Value) Then
                    Dim title As String = "Save Alert !"
                    Dim message As String = ""
                    mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mAssemblyMonitorModStatus.DoneByID.ToString, mAssemblyMonitorModStatus.DoneOn)
                    If (mEmployeeStatus(0).Information <> "") Then
                        message = mEmployeeStatus(0).Information
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenAlertMessage", MessageBox.Show(title, message, IsTagRequired:=False), True)
                        Return False
                    End If
                End If
                'End
                'Added By Vikrant On 17-Sep-2020 For Mismatch Value Mail Send
                If txtDoneOnDate.Text <> "" AndAlso mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Contains(2, "") Then 'If date period conatins then only execute
                    Dim DoneOnValue As New StringBuilder
                    Dim DoneOnValueObj As New StringBuilder
                    For j As Integer = 0 To Me.dgDoneOnValue.Rows.Count - 1
                        DoneOnValue.Append(CType(Me.dgDoneOnValue.Rows(j).FindControl("txtCurrentValue"), TextBox).Text + ", ")
                        DoneOnValueObj.Append(mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted + ", ")
                        If mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(j).PeriodID = 2 Then
                            If Not txtDoneOnDate.Text.Equals(CType(Me.dgDoneOnValue.Rows(j).FindControl("txtCurrentValue"), TextBox).Text) Then
                                Session("IsSendMail") = "True"
                            End If
                        End If

                    Next j
                    If Session("IsSendMail") = "True" Then
                        Session.Remove("IsSendMail")
                        SendMail(mAssemblyMonitorModStatus, DoneOnValue.ToString.Trim.TrimEnd(","), DoneOnValueObj.ToString.Trim.TrimEnd(","), ToMailIDs:="")
                    End If
                End If
                'End
                mAssemblyMonitorModStatus.ApplyEdit()
                mAssemblyMonitorModStatus = CType(mAssemblyMonitorModStatus.Save(), AssemblyMonitorModStatus)
                'Revise Activity
                If Not Session("mPrevAssemblyMonitorModStatusForRevise") Is Nothing Then
                    Dim mPrevAssemblyMonitorModStatusForRevise As AssemblyMonitorModStatus
                    mPrevAssemblyMonitorModStatusForRevise = Session("mPrevAssemblyMonitorModStatusForRevise")
                    mPrevAssemblyMonitorModStatusForRevise.IsApplicable = False
                    mPrevAssemblyMonitorModStatusForRevise.Save()
                    Session.Remove("mPrevAssemblyMonitorModStatusForRevise")
                End If
                'End
                SaveAttachment() 'Added By Vikrant On 25-Nov-2014
                SaveBoardInfo() 'Added by Saylee on 22-May-2009
                SaveMachineMaintenance()  'Added by Saylee on 9th-Oct-2009
                Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
                mAssemblyInfo = Session("mAssemblyInfo")
                'Added by Vikrant
                Dim mDoneOnValues As New System.Text.StringBuilder
                For i As Integer = 0 To mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count - 1
                    mDoneOnValues.Append(mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(i).DoneOnValueFormatted + ",")
                Next
                mRegNo = mMachine.RegNo
                mDirectiveNo = txtModNumber.Text
                mMonitorType = txtMonitorType.Text
                mDirectiveDetail = "Reg No.: " & mRegNo & " Directive No.: " & mDirectiveNo & " Monitor Type: " & mMonitorType & " Done On Date: " + mAssemblyMonitorModStatus.DoneOnFormatted + " Done On Value: " + mDoneOnValues.ToString
                MarkLog(Util.Action.Save, "AssemblyModifications", mDirectiveDetail, Util.ErrorType.NoError, mAssemblyMonitorModStatus.ID, EventLogID)
                Return True
            Catch ex As SqlException
                Session("mAssemblyMonitorModStatus") = clnAssemblyMonitorModStatus
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
                Return False
            Finally
                clnAssemblyMonitorModStatus = Nothing
            End Try
        Else
            Return False
        End If
    End Function
    Private Sub SetTitle()
        Dim AssemblyInfo As String = "[Model: " & mAssemblyStatus.ModelName & " Serial No.: " & mAssemblyStatus.Assembly.SerialNo & " ]"
        If mAssemblyMonitorModStatus.IsNew Then
            lblTitle.Text = IIf(mIsSpareAssembly = 0, "", IIf(mAssemblyStatus.IsSpareAssembly, "Stock ", "Removed ")) + "Assembly Directives Status " & AssemblyInfo & " [New]" 'mIsSpareAssembly Added By Vikrant On 27-Jul-2020 For ALL27072020
        Else
            lblTitle.Text = IIf(mIsSpareAssembly = 0, "", IIf(mAssemblyStatus.IsSpareAssembly, "Stock ", "Removed ")) + "Assembly Directives Status" & AssemblyInfo 'mIsSpareAssembly Added By Vikrant On 27-Jul-2020 For ALL27072020
        End If
        lblAssemblyValues.InnerText = mAssemblyStatus.AssemblyTypeName & " Values"
    End Sub
    'Private Sub MessageBoxResult()
    '    Dim Result1 As MsgBoxResult
    '    If CStr(Request.QueryString("MsgResult")) = "0,-1" Then
    '        Result1 = -1
    '    Else
    '        Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
    '    End If
    '    If Result1 > 0 Then
    '        Select Case Result1
    '            Case MsgBoxResult.Yes
    '                If CType(Session("sender"), String) = "Save" Then
    '                    Session("sender") = ""
    '                    Save()
    '                    Response.Redirect("wfComplyAssemblyMonitorModStatus.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
    '                End If
    '            Case MsgBoxResult.No
    '                If CType(Session("sender"), String) = "Save" Then
    '                    Session("sender") = ""
    '                    Response.Redirect("wfComplyAssemblyMonitorModStatus.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
    '                End If
    '            Case MsgBoxResult.Cancel
    '                If CType(Session("sender"), String) = "Save" Then
    '                    Session("sender") = ""
    '                    Response.Redirect("wfComplyAssemblyMonitorModStatus.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
    '                End If
    '            Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
    '                'Added By Utkarsh On 30-May-2012 For Link Maintenance
    '                If CType(Session("sender"), String) = "LMAlert" Then
    '                    Session("sender") = ""
    '                    RemoveSession()
    '                    Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
    '                End If
    '                'End
    '                Session("sender") = ""
    '                DataFieldBind()
    '                Response.Redirect("wfComplyAssemblyMonitorModStatus.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
    '            Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
    '                Session("sender") = ""
    '                DataFieldBind()
    '                Response.Redirect("wfComplyAssemblyMonitorModStatus.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
    '        End Select
    '    ElseIf Result1 = -1 Then
    '        Session("sender") = ""
    '        Response.Redirect("wfComplyAssemblyMonitorModStatus.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
    '    ElseIf Result1 = 0 Then   'Code Added
    '        Session("sender") = ""
    '        '   DataFieldBind()
    '    End If
    'End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    'Revise Activity
                    If MSGBoxCtrl.Sender = "ReviseActivity" Then
                        MarkLog(Util.Action.[New], "Model Directive", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                        Dim mModelMonitorMod As ModelMonitorMod
                        Dim ID As Guid = Guid.NewGuid
                        mModelMonitorMod = ModelMonitorMod.NewModelMonitorMod(mAssemblyMonitorModStatus.ModelMonitorMod, mMachine.HourType)
                        'New
                        Dim tmpModelMonitorMod As ModelMonitorMod
                        tmpModelMonitorMod = ModelMonitorMod.GetModelMonitorMod(mAssemblyMonitorModStatus.ModelMonitorMod.ID)
                        'If mAssemblyMonitorModStatus.DoneOnFormatted.ToString = "" Then
                        '    mModelMonitorMod.IssueDate = mAssemblyMonitorModStatus.AsOnDateFormatted.ToString
                        'Else
                        '    mModelMonitorMod.IssueDate = mAssemblyMonitorModStatus.DoneOnFormatted.ToString
                        'End If
                        If Not tmpModelMonitorMod.IssueDateFormatted.ToString = "" Then
                            mModelMonitorMod.IssueDate = tmpModelMonitorMod.IssueDate
                        Else
                            mModelMonitorMod.IssueDate = System.DBNull.Value
                        End If

                        'End
                        Session("mModelMonitorMod") = mModelMonitorMod
                        RemoveSession()
                        mModelMonitorMod.BeginEdit()
                        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
                        Session("mPrevAssemblyMonitorModStatusForRevise") = mAssemblyMonitorModStatus
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModelModMasterWindow", "OpenModelModMasterWindow();", True)
                    ElseIf (MSGBoxCtrl.Sender = "OverDue" Or MSGBoxCtrl.Sender = "ComplyOnSameDate") Then 'Added by Saylee on 26-Mar-2019 for ALL26032019
                        'ComplyOnSameDate Added By Prashant 19-Nov-2019 Alert if user is complying on same date 
                        If Save() Then
                            If MSGBoxCtrl.Sender = "OverDue" Then
                                MarkLog(Util.Action.Save, "AssemblyModifications", User.Identity.Name & " saved OverDue record: " & Session("OverDueString") & " " & Session("DueString"), Util.ErrorType.HandledError, mAssemblyMonitorModStatus.ID, EventLogID)
                            ElseIf MSGBoxCtrl.Sender = "ComplyOnSameDate" Then
                                MarkLog(Util.Action.Save, "AssemblyModifications", User.Identity.Name & " Comply On Same Date: ", Util.ErrorType.HandledError, mAssemblyMonitorModStatus.ID, EventLogID)
                            End If

                            'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
                            If AppSettings("LinkMaintenance") = "True" Then
                                mMultiComplianceList = Session("mMultiComplianceList")
                                If Not mMultiComplianceList Is Nothing Then
                                    If mMultiComplianceList.Count > 0 Then
                                        If Session("From") = 1 Then 'Edit Record
                                            MSGBoxCtrl.show("Alert !", "<BR>Other Maintenance Activity(s) linked with this maintenance activity.To Edit/Delete individual Maintenance Activity go to respective activity.", "", MsgBoxStyle.OkOnly, "LMAlert")
                                            Exit Sub
                                        End If
                                    End If
                                    Dim Result As Boolean
                                    SetLinkedMaintenanceGridObject()
                                    Dim LinkMaintenanceEvents As LinkedMaintenanceActivityEvents = New LinkedMaintenanceActivityEvents
                                    LinkMaintenanceEvents.AssemblyLogInfo = "Assembly Directive: " & mDirectiveDetail 'setting Mark Log Detail ...

                                    'LicenseNo = Session("LicenseNo")
                                    'Dim EmployeeID As String = Session("EmpID")
                                    LicenseNo = IIf(Session("LicenseNo") Is Nothing, String.Empty, Session("LicenseNo"))
                                    Dim EmployeeID As String = IIf(Session("EmpID") Is Nothing, String.Empty, Session("EmpID"))

                                    'EmployeeID = EmployeeID.ToString.TrimEnd(",")

                                    EmpName = IIf(Session("EmpName") Is Nothing, String.Empty, Session("EmpName"))
                                    EmployeeID = IIf(EmployeeID Is Nothing, "", EmployeeID.ToString.TrimEnd(","))
                                    LicenseNo = IIf(LicenseNo Is Nothing, "", LicenseNo.ToString.TrimEnd(","))
                                    EmpName = IIf(EmpName Is Nothing, "", EmpName.ToString.TrimEnd(",")) 'EmpName.ToString.TrimEnd(",")


                                    Result = LinkMaintenanceEvents.SaveLinkedMaintenanceActivies(mMultiComplianceList, mAssemblyMonitorModStatus.DoneWONo, txtDoneOnDate.Text.ToString, mMachineMaintenance.LogID, mMachine.HourType, mMachine.ID, mAssemblyMonitorModStatus.AssemblyID, PeriodValues, mAssemblyMonitorModStatus.DoneRemark, LicenseNo, DoneByID.ToString, EmpName, Trim(txtPlace.Text))

                                    Session.Remove("EmpID")
                                    Session.Remove("LicenseNo")
                                    Session.Remove("EmpName")
                                    If LinkMaintenanceEvents.ErrorStr.Length > 0 Then
                                        Dim title As String = "Link Maintenance Alert !"
                                        Dim message As String = LinkMaintenanceEvents.ErrorStr
                                        MSGBoxCtrl.show(title, message, "", MsgBoxStyle.OkOnly, "")
                                        Exit Sub
                                    Else
                                        MSGBoxCtrl.show("Alert !", "<BR>Other Maintenance Activity(s) linked with this maintenance activity.To Edit/Delete individual Maintenance Activity go to respective activity.", "", MsgBoxStyle.OkOnly, "LMAlert")
                                        Exit Sub
                                    End If
                                End If
                            End If
                            'End
                            RemoveSession() 'Added By Vikrant on 25-Nov-2014
                            Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
                        Else
                            upnlValidationSummary.Update()
                        End If
                    End If
                    'End
                Case MsgBoxResult.No
                    'Revise Activity
                    If MSGBoxCtrl.Sender = "ReviseActivity" Then
                        RemoveSession()
                        Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
                    End If
                    'End
                Case MsgBoxResult.Ok
                    If MSGBoxCtrl.Sender = "LMAlert" Then
                        Session("sender") = ""
                        RemoveSession()
                        Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
                    End If
            End Select
        End If
    End Sub
    Private Sub ControlVisibility()
        btnPrint.Enabled = Not mAssemblyMonitorModStatus.IsNew
        dgCurrentValue.Columns(3).Visible = Not mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 3
        dgCurrentValue.Columns(4).Visible = Not mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 3
        dgDoneOnValue.Columns(2).Visible = Not mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 3
        txtExtensionDate.Visible = Not mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 3
        'Added By Shweta ON 28-Jun-2013 FOR ALL28062013
        dgDoneOnValue.Columns(4).Visible = (mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID <> 3) AndAlso (mAssemblyStatus.AssemblyTypeID <> 1 AndAlso mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID <> 3) AndAlso mIsSpareAssembly <> 1 'mIsSpareAssembly Added By Vikrant On 27-Jul-2020 For ALL27072020
        'End
        'Added By Utkarsh On 30-May-2012 For Link Maitenance
        If AppSettings("LinkMaintenance") = "True" Then
            If Not mMultiComplianceList Is Nothing Then
                If mMultiComplianceList.Count > 0 Then
                    pnlInner.Visible = True
                    dgMultiComplianceList.Columns(0).Visible = IIf(Session("From") = 1, False, True) 'Visible false on Record Edit
                Else
                    pnlInner.Visible = False
                End If
            End If
        Else
            pnlInner.Visible = False
        End If
        'End
        If mAssemblyMonitorModStatus.ModelMonitorMod.ReadOnlyFrequencyColumn Then
            'txtDoneOnDate.Enabled = False 'Commented by Saylee on 22-Nov-2019 as DoneOne should be open in all cases, 
            chkApplicable.Enabled = False
        End If
        btnRevise.Enabled = (mAssemblyMonitorModStatus.IsApplicable And Not mAssemblyMonitorModStatus.IsNew And Not ((mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 1 Or mAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 4) And mAssemblyMonitorModStatus.DoneOnFormatted.ToString <> "")) 'Revise Activity
        btnSelectLog.Visible = (mIsSpareAssembly <> 1) ' Added By Vikrant On 27-Jul-2020 For ALL27072020
        ControlVisibilityForAttachment() 'Added By Vikrant On 25-Nov-2014
    End Sub
    Private Sub SetMachineMaintenanceObject()
        'Added by Saylee on 9th-Oct-2009
        If Session("From") = 0 And Not (mMachineMaintenanceList.Contains(mAssemblyMonitorModStatus.ID, 7, "")) Then
            mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(mAssemblyStatus.MachineID, 7, txtDoneOnDate.Text, mAssemblyMonitorModStatus.ID, Guid.Empty, 0, 0, mAssemblyStatus.ID)
        Else
            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mAssemblyMonitorModStatus.ID, 7)
        End If
        With mMachineMaintenance
            .MaintenanceID = mAssemblyMonitorModStatus.ID 'TransactionID
            .Date = txtDoneOnDate.Text
            Dim mLog As Log = CType(Session("mLog"), Log)
            If Not mLog Is Nothing Then
                .LogNo = mLog.LogNo
                .LogID = mLog.ID
                .LogPageNo = mLog.LogPageNo
                Session.Remove("mLog")
            Else
                Dim mMaxLogNo As MaxLogNo
                mMaxLogNo = MaxLogNo.GetMaxLogNo(txtDoneOnDate.Text, mAssemblyStatus.MachineID, mAssemblyStatus.AssemblyID)
                If mMaxLogNo.Count <> 0 Then
                    .LogNo = mMaxLogNo(0).LogNo
                    .LogID = mMaxLogNo(0).LogId
                    .LogPageNo = mMaxLogNo(0).LogPageNo
                Else 'Else Condition Added By Vikrant On 09-Jun-2020 For ALL09062020
                    mMaxLogNo = MaxLogNo.GetMaxLogNo_WhileAssemblyInstall(txtDoneOnDate.Text, mAssemblyStatus.MachineID)
                    If mMaxLogNo.Count <> 0 Then
                        .LogNo = mMaxLogNo(0).LogNo
                        .LogID = mMaxLogNo(0).LogId
                        .LogPageNo = mMaxLogNo(0).LogPageNo
                    End If
                End If
            End If
        End With
        Session("mMachineMaintenance") = mMachineMaintenance
    End Sub
    Private Sub SaveMachineMaintenance()
        'Added by Saylee on 9th-Oct-2009
        If mMachineMaintenance.IsValid = True Then
            Try
                mMachineMaintenance.ApplyEdit()
                mMachineMaintenance.Save()
                Session("mMachineMaintenance") = mMachineMaintenance
            Catch ex As Exception

            End Try
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub ShowLinkedMaintenaceActivity() 'Added By Utkarsh On 07-Feb-2012 FOR Link Maintenance
        mMultiComplianceList = New MultiComplianceList
        Dim mPeriodUnitName As String
        Dim mFrequencyValue As String
        Dim mDoneOnValue As String
        Dim mCurrentValue As String
        Dim mDueOnValue As String
        Dim mElapsedValue As String
        Dim mRemainingValue As String
        Dim mDoneOn As String
        Dim mExtensionValue As String
        Dim mPeriodUnitList As PeriodUnitList = PeriodUnitList.GetPeriodUnitList()

        For i As Integer = 0 To mLinkMaintenanceList.Count - 1
            If Not i = 0 Then
                mPeriodUnitName = String.Empty
                mFrequencyValue = String.Empty
                mDoneOnValue = String.Empty
                mCurrentValue = String.Empty
                mDueOnValue = String.Empty
                mElapsedValue = String.Empty
                mRemainingValue = String.Empty
                mDoneOn = String.Empty
                mExtensionValue = String.Empty
            End If

            Select Case mLinkMaintenanceList(i).LinkedMaintenanceTypeID
                Case 1 'Assembly Service
                    mLinkMaintenanceMonitorStatus = LinkMaintenaceMonitorStatus.GetLinkedMaintenanceMonitorStatus(mMachine.ID, mLinkMaintenanceList(i).LinkedMaintenaceActivityID, mLinkMaintenanceList(i).LinkedMaintenanceTypeID, mAssemblyMonitorModStatus.AssemblyID)
                    If mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.Equals(Guid.Empty) Then
                        Exit Select
                    End If
                    Dim mPrevAssemblyMonitorSeviceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID, mLinkMaintenanceMonitorStatus.AssemblyStatusID, mMachine.HourType)
                    mAssemblyMonitorServiceStatusForLM = AssemblyMonitorServiceStatus.GetComplyAssemblyMonitorServiceStatusForLinkMaintenance(mPrevAssemblyMonitorSeviceStatus.ID, mPrevAssemblyMonitorSeviceStatus.AssemblyStatusID, txtDoneOnDate.Text, Guid.Empty, mMachine.HourType, mLinkMaintenanceMonitorStatus.ModelMonitorID, True)
                    Dim mAssemblyInfo As String = mAssemblyMonitorServiceStatusForLM.ModelMonitorService.Model.Name '& VbCrLf & mAssemblyMonitorServiceStatusForLm.

                    For j As Integer = 0 To mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods.Count - 1
                        If mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodID = 2 Then
                            Dim PeriodCode As String = mPeriodUnitList(3, "").Code
                            If j = 0 Then

                                mPeriodUnitName = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitName
                                mFrequencyValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).FrequencyValueFormatted & " " & mPeriodUnitList(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitID, "").Code
                                mDoneOnValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DoneOnValueFormatted
                                mCurrentValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).CurrentValueFormatted
                                mDueOnValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DueOnValueFormatted
                                mElapsedValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
                                mRemainingValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
                                mExtensionValue = IIf(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            Else
                                mPeriodUnitName = mPeriodUnitName & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitName
                                mFrequencyValue = mFrequencyValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).FrequencyValueFormatted & " " & mPeriodUnitList(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitID, "").Code
                                mDoneOnValue = mDoneOnValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DoneOnValueFormatted
                                mCurrentValue = mCurrentValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).CurrentValueFormatted
                                mDueOnValue = mDueOnValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DueOnValueFormatted
                                mElapsedValue = mElapsedValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
                                mRemainingValue = mRemainingValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
                                mExtensionValue = mExtensionValue & vbCrLf & IIf(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            End If
                        Else

                            Dim PeriodCode As String = mPeriodUnitList(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitID, "").Code

                            If j = 0 Then

                                mPeriodUnitName = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitName
                                mFrequencyValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).FrequencyValueFormatted & " " & PeriodCode
                                mDoneOnValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DoneOnValueFormatted & " " & PeriodCode
                                mCurrentValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).CurrentValueFormatted & " " & PeriodCode
                                mDueOnValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DueOnValueFormatted & " " & PeriodCode
                                mElapsedValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
                                mRemainingValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
                                mExtensionValue = IIf(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            Else
                                mPeriodUnitName = mPeriodUnitName & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitName
                                mFrequencyValue = mFrequencyValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).FrequencyValueFormatted & " " & PeriodCode
                                mDoneOnValue = mDoneOnValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DoneOnValueFormatted & " " & PeriodCode
                                mCurrentValue = mCurrentValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).CurrentValueFormatted & " " & PeriodCode
                                mDueOnValue = mDueOnValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DueOnValueFormatted & " " & PeriodCode
                                mElapsedValue = mElapsedValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
                                mRemainingValue = mRemainingValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
                                mExtensionValue = mExtensionValue & vbCrLf & IIf(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            End If
                        End If
                    Next
                    mMultiComplianceList.Add(mAssemblyMonitorServiceStatusForLM.ID, MaintenanceActivityTypes.AssemblyService, IIf(Session("From") = 1, False, True), mAssemblyMonitorServiceStatusForLM.ModelMonitorService.Reference, mAssemblyMonitorServiceStatusForLM.ModelMonitorService.MonitorTypeName, mAssemblyMonitorServiceStatusForLM.ModelMonitorService.ModelMonitorServiceTypeName, mAssemblyMonitorServiceStatusForLM.ModelMonitorService.Description, mAssemblyMonitorServiceStatusForLM.DoneOn.ToString, mAssemblyMonitorServiceStatusForLM.DoneWONo, mAssemblyMonitorServiceStatusForLM.DoneRemark, mPeriodUnitName, mFrequencyValue, mDoneOnValue, mCurrentValue, mElapsedValue, mExtensionValue, mDueOnValue, mRemainingValue, , , mMachine.RegNo, mAssemblyMonitorServiceStatusForLM.ModelMonitorService.Model.AssemblyTypeName, mAssemblyInfo, , , mPeriodUnitName, , mFrequencyValue, , mLinkMaintenanceMonitorStatus.AssemblyStatusID.ToString, , , , , mAssemblyMonitorServiceStatusForLM.ModelMonitorService.ModelID.ToString, , , , , mAssemblyMonitorServiceStatusForLM.ModelMonitorService.ATAChapter, , , , , mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.ToString, , , , , , , , , , mLinkMaintenanceList(i).MaintenanceActionID, mLinkMaintenanceList(i).MaintenanceActionName)
                    mLinkMaintenanceMonitorStatus = Nothing

                Case 2 'Assembly Inspection

                    mLinkMaintenanceMonitorStatus = LinkMaintenaceMonitorStatus.GetLinkedMaintenanceMonitorStatus(mMachine.ID, mLinkMaintenanceList(i).LinkedMaintenaceActivityID, mLinkMaintenanceList(i).LinkedMaintenanceTypeID, mAssemblyMonitorModStatus.AssemblyID)
                    If mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.Equals(Guid.Empty) Then
                        Exit Select
                    End If
                    Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID, mLinkMaintenanceMonitorStatus.AssemblyStatusID, mMachine.HourType)

                    mAssemblyMonitorInspStatusForLM = AssemblyMonitorInspStatus.GetComplyAssemblyMonitorInspStatusForLinkMaintenance(mPrevAssemblyMonitorInspStatus.ID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, txtDoneOnDate.Text.ToString, Guid.Empty, mMachine.HourType, mLinkMaintenanceMonitorStatus.ModelMonitorID, True)

                    Dim mAssemblyInfo As String = mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.Model.Name '& VbCrLf & mAssemblyMonitorServiceStatusForLm.

                    For j As Integer = 0 To mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods.Count - 1

                        If mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodID = 2 Then

                            Dim PeriodCode As String = mPeriodUnitList(3, "").Code
                            If j = 0 Then
                                mPeriodUnitName = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodUnitName
                                mFrequencyValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).FrequencyValueFormatted & " " & mPeriodUnitList(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodUnitID, "").Code
                                mDoneOnValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DoneOnValueFormatted
                                mCurrentValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted
                                mDueOnValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DueOnValueFormatted
                                mElapsedValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
                                mRemainingValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
                                mExtensionValue = IIf(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            Else
                                mPeriodUnitName = mPeriodUnitName & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodUnitName
                                mFrequencyValue = mFrequencyValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).FrequencyValueFormatted & " " & mPeriodUnitList(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodUnitID, "").Code
                                mDoneOnValue = mDoneOnValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DoneOnValueFormatted
                                mCurrentValue = mCurrentValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted
                                mDueOnValue = mDueOnValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DueOnValueFormatted
                                mElapsedValue = mElapsedValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
                                mRemainingValue = mRemainingValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
                                mExtensionValue = mExtensionValue & vbCrLf & IIf(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            End If
                        Else
                            Dim PeriodCode As String = mPeriodUnitList(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodUnitID, "").Code

                            If j = 0 Then
                                mPeriodUnitName = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodUnitName
                                mFrequencyValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).FrequencyValueFormatted & " " & PeriodCode
                                mDoneOnValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DoneOnValueFormatted & " " & PeriodCode
                                mCurrentValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted & " " & PeriodCode
                                mDueOnValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DueOnValueFormatted & " " & PeriodCode
                                mElapsedValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
                                mRemainingValue = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
                                mExtensionValue = IIf(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            Else
                                mPeriodUnitName = mPeriodUnitName & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodUnitName
                                mFrequencyValue = mFrequencyValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).FrequencyValueFormatted & " " & PeriodCode
                                mDoneOnValue = mDoneOnValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DoneOnValueFormatted & " " & PeriodCode
                                mCurrentValue = mCurrentValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted & " " & PeriodCode
                                mDueOnValue = mDueOnValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DueOnValueFormatted & " " & PeriodCode
                                mElapsedValue = mElapsedValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
                                mRemainingValue = mRemainingValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
                                mExtensionValue = mExtensionValue & vbCrLf & IIf(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            End If
                        End If
                    Next
                    mMultiComplianceList.Add(mAssemblyMonitorInspStatusForLM.ID, MaintenanceActivityTypes.AssemblyInspection, IIf(Session("From") = 1, False, True), mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.Reference, mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.MonitorTypeName, mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.ModelMonitorInspTypeName, mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.Description, mAssemblyMonitorInspStatusForLM.DoneOn.ToString, mAssemblyMonitorInspStatusForLM.DoneWONo, mAssemblyMonitorInspStatusForLM.DoneRemark, mPeriodUnitName, mFrequencyValue, mDoneOnValue, mCurrentValue, mElapsedValue, mExtensionValue, mDueOnValue, mRemainingValue, , , mMachine.RegNo, mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.Model.AssemblyTypeName, mAssemblyInfo, , , mPeriodUnitName, , mFrequencyValue, , mLinkMaintenanceMonitorStatus.AssemblyStatusID.ToString, , , , , mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.ModelID.ToString, , , , , mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.ATAChapter, , , , , , mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.ToString, , , , , , , , , mLinkMaintenanceList(i).MaintenanceActionID, mLinkMaintenanceList(i).MaintenanceActionName)
                    mLinkMaintenanceMonitorStatus = Nothing

                Case 3 'Assembly Directive
                    mLinkMaintenanceMonitorStatus = LinkMaintenaceMonitorStatus.GetLinkedMaintenanceMonitorStatus(mMachine.ID, mLinkMaintenanceList(i).LinkedMaintenaceActivityID, mLinkMaintenanceList(i).LinkedMaintenanceTypeID, mAssemblyMonitorModStatus.AssemblyID)
                    If mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.Equals(Guid.Empty) Then
                        Exit Select
                    End If
                    Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID, mLinkMaintenanceMonitorStatus.AssemblyStatusID, mMachine.HourType)

                    mAssemblyMonitorModStatusForLM = AssemblyMonitorModStatus.GetComplyAssemblyMonitorModStatusForLinkMaintenance(mPrevAssemblyMonitorModStatus.ID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, txtDoneOnDate.Text.ToString, Guid.Empty, mMachine.HourType, mLinkMaintenanceMonitorStatus.ModelMonitorID, True)

                    Dim mAssemblyInfo As String = mAssemblyMonitorModStatusForLM.ModelMonitorMod.Model.Name '& VbCrLf & mAssemblyMonitorServiceStatusForLm.

                    For j As Integer = 0 To mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods.Count - 1

                        If mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodID = 2 Then

                            Dim PeriodCode As String = mPeriodUnitList(3, "").Code

                            If j = 0 Then
                                mPeriodUnitName = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodUnitName
                                mFrequencyValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).FrequencyValueFormatted & " " & mPeriodUnitList(mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodUnitID, "").Code
                                mDoneOnValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DoneOnValueFormatted
                                mCurrentValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted
                                mDueOnValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DueOnValueFormatted
                                mElapsedValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
                                mRemainingValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
                                mExtensionValue = IIf(mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            Else
                                mPeriodUnitName = mPeriodUnitName & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodUnitName
                                mFrequencyValue = mFrequencyValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).FrequencyValueFormatted & " " & mPeriodUnitList(mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodUnitID, "").Code
                                mDoneOnValue = mDoneOnValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DoneOnValueFormatted
                                mCurrentValue = mCurrentValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted
                                mDueOnValue = mDueOnValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DueOnValueFormatted
                                mElapsedValue = mElapsedValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
                                mRemainingValue = mRemainingValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
                                mExtensionValue = mExtensionValue & vbCrLf & IIf(mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            End If

                        Else
                            Dim PeriodCode As String = mPeriodUnitList(mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodUnitID, "").Code

                            If j = 0 Then
                                mPeriodUnitName = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodUnitName
                                mFrequencyValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).FrequencyValueFormatted & " " & PeriodCode
                                mDoneOnValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DoneOnValueFormatted & " " & PeriodCode
                                mCurrentValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted & " " & PeriodCode
                                mDueOnValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DueOnValueFormatted & " " & PeriodCode
                                mElapsedValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
                                mRemainingValue = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
                                mExtensionValue = IIf(mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            Else
                                mPeriodUnitName = mPeriodUnitName & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodUnitName
                                mFrequencyValue = mFrequencyValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).FrequencyValueFormatted & " " & PeriodCode
                                mDoneOnValue = mDoneOnValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DoneOnValueFormatted & " " & PeriodCode
                                mCurrentValue = mCurrentValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted & " " & PeriodCode
                                mDueOnValue = mDueOnValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DueOnValueFormatted & " " & PeriodCode
                                mElapsedValue = mElapsedValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
                                mRemainingValue = mRemainingValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
                                mExtensionValue = mExtensionValue & vbCrLf & IIf(mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            End If
                        End If
                    Next
                    mMultiComplianceList.Add(mAssemblyMonitorModStatusForLM.ID, MaintenanceActivityTypes.AssemblyDirective, IIf(Session("From") = 1, False, True), mAssemblyMonitorModStatusForLM.ModelMonitorMod.Reference, mAssemblyMonitorModStatusForLM.ModelMonitorMod.MonitorTypeName, mAssemblyMonitorModStatusForLM.ModelMonitorMod.ModelMonitorModTypeName, mAssemblyMonitorModStatusForLM.ModelMonitorMod.Description, mAssemblyMonitorModStatusForLM.DoneOn.ToString, mAssemblyMonitorModStatusForLM.DoneWONo, mAssemblyMonitorModStatusForLM.DoneRemark, mPeriodUnitName, mFrequencyValue, mDoneOnValue, mCurrentValue, mElapsedValue, mExtensionValue, mDueOnValue, mRemainingValue, , , mMachine.RegNo, mAssemblyMonitorModStatusForLM.ModelMonitorMod.Model.AssemblyTypeName, mAssemblyInfo, , , mPeriodUnitName, , mFrequencyValue, , mLinkMaintenanceMonitorStatus.AssemblyStatusID.ToString, , , , , mAssemblyMonitorModStatusForLM.ModelMonitorMod.ModelID.ToString, , , , , mAssemblyMonitorModStatusForLM.ModelMonitorMod.ATAChapter, , , , , , , mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.ToString, , , , , , , , mLinkMaintenanceList(i).MaintenanceActionID, mLinkMaintenanceList(i).MaintenanceActionName)
                    mLinkMaintenanceMonitorStatus = Nothing
            End Select
        Next
        dgMultiComplianceList.DataSource = mMultiComplianceList
        Session("mMultiComplianceList") = mMultiComplianceList 'Added By Utkarsh ON 15-Mar-2012 FOR Link Maintenance
        lblResult.Text = "List of Linked Maintenance Activity: " & mMultiComplianceList.Count & " Record(s) found."
    End Sub
    Private Sub SetLinkedMaintenanceGridObject() 'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
        Dim chkSelect As CheckBox
        For i As Integer = 0 To dgMultiComplianceList.Rows.Count - 1
            chkSelect = CType(dgMultiComplianceList.Rows(i).FindControl("chkSelect"), CheckBox)
            mMultiComplianceList(i).IsSelect = chkSelect.Checked
        Next
    End Sub 'End
    Private Sub ControlVisibilityForAttachment()
        If mAssemblyMonitorModStatus.IsAttachmentAdded = True Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Private Sub GetAttachment()
        If mAssemblyMonitorModStatus.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mAssemblyMonitorModStatus.ID)
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
                If (Not mAssemblyMonitorModStatus.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mAssemblyMonitorModStatus.ID)
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
    'End
    Private Sub ControlVisibilityForDatePeriod()
        Dim txtDnOnDate As TextBox
        For j As Integer = 0 To Me.dgDoneOnValue.Rows.Count - 1
            txtDnOnDate = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtCurrentValue"), TextBox)
            With mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods

                'Commented by Saylee on 28-June-2018 for ALL28062018 for star air, to lock all period values while complaince
                'previously only date period was locked, but now all period values are locked
                'If .Item(j).PeriodID = 2 And txtDoneOnDate.Text <> "" Then
                '    txtDnOnDate.Enabled = False
                'Else
                '    txtDnOnDate.Enabled = True
                'End If
                If txtDoneOnDate.Text <> "" Then
                    txtDnOnDate.Enabled = False
                End If
            End With
        Next j
    End Sub
    Public Sub SetUserMailIDs()
        Session("UserEmailID") = mModuleList.Item("AssemblyModifications").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("AssemblyModifications").SendCCMailID
        Session("MailsRequire") = mModuleList.Item("AssemblyModifications").MailsRequire
        Session("SmtpHost") = mModuleList.Item("AssemblyModifications").SmtpHost
        Session("SmtpPort") = mModuleList.Item("AssemblyModifications").SmtpPort
        Session("SmtpUser") = mModuleList.Item("AssemblyModifications").SmtpUser
        Session("SmtpPassword") = mModuleList.Item("AssemblyModifications").SmtpPassword
    End Sub
#End Region

#Region " Data Bindings "
    'MLNo
    Public Sub SetLicenceCount()
        If mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Count > 1 Then
            lblLicenceCount.Text = "and " + (mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Count - 1).ToString + " more"
        End If
        lblLicenceCount.DataBind()
        'lblAllLicenceNos.DataBind()
    End Sub
    Private Sub BindLicenceNo()
        If mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Count > 0 Then
            txtLicenceNo.Text = mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(0).LicenceNo + " [" + mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(0).EmployeeName + "]"
        Else
            txtLicenceNo.Text = String.Empty
        End If
    End Sub
    'End
    Private Sub DataFieldBind()
        dgCurrentValue.DataSource = mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
        dgDoneOnValue.DataSource = mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
        txtDoneOnDate.Text = mAssemblyMonitorModStatus.DoneOnFormatted.ToString 'Added On 28,May,2007 By Girish
        txtExtensionDate.Text = mAssemblyMonitorModStatus.ExtensionDateFormatted.ToString
        mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList() 'Added by Saylee on 9th-Oct-2009
        Session("mMachineMaintenanceList") = mMachineMaintenanceList
        If AppSettings("LinkMaintenance") = "True" Then 'Added By Utkarsh On 07-Feb-2012 FOR Link Maintenance
            mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mPrevAssemblyMonitorModStatus.ModelMonitorModID.ToString)
            Session("mLinkMaintenanceList") = mLinkMaintenanceList
            If mLinkMaintenanceList.Count > 0 Then
                ShowLinkedMaintenaceActivity()
            End If
        End If 'End

        If mAssemblyMonitorModStatus.ModelMonitorMod.RequiredManHours <> "" Then lblEstdManHours.Text = "(Estd. Man Hours: " + mAssemblyMonitorModStatus.ModelMonitorMod.RequiredManHours + ")"
        BindLicenceNo() 'MLNo
        DataBind()
    End Sub
    Private Sub DataBindGrid()
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
        dgCurrentValue.DataSource = mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
        dgDoneOnValue.DataSource = mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
        dgCurrentValue.DataBind()
        dgDoneOnValue.DataBind()
        ControlVisibilityForDatePeriod()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        'If custValidator.ControlToValidate = "txtRemark" Then
        '    If Len(txtRemark.Text) > 500 Then
        '        custValidator.ErrorMessage = "Max. length of Remark should be 500 char."
        '        e.IsValid = False
        '    Else
        '        e.IsValid = True
        '    End If
        'Added By Utkarsh On 12-Jun-2012 FOR ALL08062012
        If custValidator.ControlToValidate = "txtLicenceNo" Then
            If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Or (txtLicenceNo.Text.Trim.IndexOf("[") < 0 And txtLicenceNo.Text.Trim.IndexOf("]") < 0) Then
                e.IsValid = True
            Else
                custValidator.ErrorMessage = "Enter Correct License No."
                e.IsValid = False
            End If
            'End
        End If
    End Sub
    Public Sub customvalidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        If Flag = 1 Then Exit Sub
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        SetObject()
        SetGridObject()
        Dim str As String = ""
        Dim txtCurrentValue As TextBox
        If Not mAssemblyMonitorModStatus.IsValid Then
            For i As Integer = 0 To mAssemblyMonitorModStatus.GetBrokenRulesCollection.Count - 1
                str = str + mAssemblyMonitorModStatus.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(dgDoneOnValue.Rows.Count - 1)
            txtCurrentValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtCurrentValue"), TextBox)
            If Not mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next
        If str <> "" Then
            custValidator.ErrorMessage = str
            e.IsValid = False
        End If
        Flag = 1
    End Sub
    Public Function CustomValidate2() As Boolean
        Dim str As String = ""
        For i As Integer = 0 To CShort(dgDoneOnValue.Rows.Count - 1)
            If Not mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
                Next
            End If
        Next
        If str <> "" Then
            cvRemark.ErrorMessage = str
            cvRemark.IsValid = False
            Return False
        End If
        Return True
    End Function
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by vikrant on 27-July-2011
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            If txtModelMonitorModTypeName.Enabled = True Then
                btnSelectLog.Focus()
            End If
            Session("mLogList") = Nothing
            SetLog()
            DataFieldBind()
            ControlVisibility()
            ControlVisibilityForDatePeriod()
            SetTitle()
            'MLNo
            SetLicenceCount()
            UserNameForLicenceList = User.Identity.Name
            Session("UserNameForLicenceList") = UserNameForLicenceList
            'End

            'If Not mAssemblyMonitorModStatus.IsNew And Session("From") = 1 Then

            '    'Added By Saylee On 9-FEB-2021 For Mismatch Value Mail Send of Controls
            '    If txtDoneOnDate.Text <> "" AndAlso mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Contains(2, "") Then 'If date period conatins then only execute
            '        Dim DoneOnValue As New StringBuilder
            '        Dim DoneOnValueObj As New StringBuilder
            '        Dim ControlDoneOnValue As String = String.Empty
            '        For j As Integer = 0 To Me.dgDoneOnValue.Rows.Count - 1
            '            DoneOnValue.Append(CType(Me.dgDoneOnValue.Rows(j).FindControl("txtCurrentValue"), TextBox).Text + ", ")
            '            DoneOnValueObj.Append(mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted + ", ")
            '            If mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(j).PeriodID = 2 Then
            '                ControlDoneOnValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtCurrentValue"), TextBox).Text
            '                If Not txtDoneOnDate.Text.ToString.Equals(ControlDoneOnValue) Then
            '                    Session("IsSendMail") = "True"
            '                End If
            '            End If
            '        Next j
            '        If Session("IsSendMail") = "True" Then
            '            Session.Remove("IsSendMail")
            '            SendMail(mAssemblyMonitorModStatus, DoneOnValue.ToString.Trim.TrimEnd(","), DoneOnValueObj.ToString.Trim.TrimEnd(","), True, ToMailIDs:="deven@bytzsoft.com,saylee@bytzsoft.com")
            '        End If
            '    End If
            '    'End
            'End If

        End If
    End Sub
    Protected Sub txtCurrentValue_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'If Not IsValid Then Exit Sub
        Dim txtCurrentValue As TextBox
        For i As Integer = 0 To dgDoneOnValue.Rows.Count - 1
            txtCurrentValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtCurrentValue"), TextBox)
            With mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
                If .Item(i).PeriodID = 2 Then
                    If Period.IsDate(txtCurrentValue.Text.Trim) Then
                        .Item(i).CurrentValueFormatted = Trim(txtCurrentValue.Text)
                    Else
                        .Item(i).CurrentValueFormatted = ""
                    End If
                Else
                    .Item(i).CurrentValue = Trim(txtCurrentValue.Text)
                End If
            End With
        Next
        DataBindGrid()
        'upnlDoneOnValueGrid.Update()
        upnlCurrentValueGrid.Update()
    End Sub
    Protected Sub txtExtensionValue_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim txtExtensionValue As TextBox
        For i As Integer = 0 To mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count - 1
            txtExtensionValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtExtensionValue"), TextBox)

            With mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods
                .Item(i).ExtensionValue = Trim(txtExtensionValue.Text)
            End With
        Next
        DataBindGrid()
        upnlCurrentValueGrid.Update()
        'upnlDoneOnValueGrid.Update()
    End Sub
    Private Sub txtDoneOnDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDoneOnDate.TextChanged
        If IsPostBack Then      'Added Code on May,29,2007
            If CStr(mAssemblyMonitorModStatus.DoneOn.ToString) <> "" And txtDoneOnDate.Text.ToString <> "" Then
                If DateDiff(DateInterval.Day, SmartDate.StringToDate(mAssemblyMonitorModStatus.DoneOn.ToString), SmartDate.StringToDate(txtDoneOnDate.Text)) <> 0 Then
                    SetObject()
                    Dim clnAssemblyMonitorModStatus As AssemblyMonitorModStatus = mAssemblyMonitorModStatus.Clone
                    If Session("From") = 0 Then 'New Record
                        NewRecord(Guid.Empty, txtDoneOnDate.Text.ToString)
                    Else
                        EditRecord(Guid.Empty, txtDoneOnDate.Text.ToString, False)
                    End If
                    SetFromClone(clnAssemblyMonitorModStatus)
                    'DataBindGrid()
                    Session.Remove("mLog") 'Added by Saylee on 9th-Oct-2009

                    If AppSettings("LinkMaintenance") = "True" Then 'Added By Utkarsh On 19-Mar-2012 FOR Link Maintenance
                        mLinkMaintenanceList = Session("mLinkMaintenanceList")
                        If Not mLinkMaintenanceList Is Nothing Then
                            If mLinkMaintenanceList.Count > 0 Then
                                ShowLinkedMaintenaceActivity()
                                dgMultiComplianceList.DataBind()
                            End If
                        End If
                    End If 'End
                    SetGridFromObject()
                    DataBindGrid()
                    upnlCurrentValueGrid.Update()
                    upnlDoneOnValueGrid.Update()
                    upnlLinkMaintenance.Update()
                    upnlTitle.Update()
                End If
            End If
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("AssemblyModificationsNew") And mAssemblyMonitorModStatus.IsNew) Or (Not User.IsInRole("AssemblyModificationsEdit") And Not mAssemblyMonitorModStatus.IsNew) Then
            SetObject()
            SetSession()
            'Added by Vikrant
            mRegNo = mMachine.RegNo
            mDirectiveNo = txtModNumber.Text
            mMonitorInfo = txtModelMonitorModTypeName.Text
            mMonitorType = txtMonitorType.Text
            mDirectiveDetail = "Reg No.: " & mRegNo & " Directive No.: " & mDirectiveNo & " Monitor Type: " & mMonitorType
            MarkLog(Util.Action.Save, "AssemblyModifications", User.Identity.Name & " is not Authorized User to save " & mDirectiveDetail, Util.ErrorType.HandledError, mAssemblyMonitorModStatus.ID, EventLogID)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        If IsValid Then

            'Code for OverDue 'Added by Saylee on 26-Mar-2019 for ALL26032019
            If Not mPrevAssemblyMonitorModStatus.ModelMonitorMod.MonitorTypeID = 3 Then 'No Frequency record not be checked for OverDue
                Dim DueString As String = ""
                DueString = CustomeValidateGridValuesForOverDue()
                If DueString <> "" Then
                    MSGBoxCtrl.show("Alert!!!", "You are about to save Over Due Compliance, " + DueString, "Do you want to continue?", MsgBoxStyle.YesNo, "OverDue")
                    Session("DueString") = DueString
                    Exit Sub
                End If
            End If
            '*********************************************************************************
            'Added By Prashant 19-Nov-2019 Alert if user is complying on same date ALL19112019
            If mPrevAssemblyMonitorModStatus.DoneOn.ToString <> "" Then
                If (CDate(txtDoneOnDate.Text) <= CDate(mPrevAssemblyMonitorModStatus.DoneOn) And Session("From") <> 1) Then
                    MSGBoxCtrl.show("Alert!!!", "Current compliance date is less than or equal to last compliance date ", "Do you want to continue?", MsgBoxStyle.YesNo, "ComplyOnSameDate")
                    Exit Sub
                End If
                'If CDate(txtDoneOnDate.Text) > CDate(mPrevAssemblyMonitorModStatus.DoneOn) Then
                '    MSGBoxCtrl.show("Alert!!!", "Current compliance date is greater than last compliance date ", "Do you want to continue?", MsgBoxStyle.YesNo, "ComplyOnSameDate")
                '    Exit Sub
                'End If
            End If
            If (CDate(txtDoneOnDate.Text) > CDate(Today.Date) And Session("From") <> 1) Then
                MSGBoxCtrl.show("Alert!!!", "Current compliance date is greater than today date  ", "Do you want to continue?", MsgBoxStyle.YesNo, "ComplyOnSameDate")
                Exit Sub
            End If
            'End of Added By Prashant 19-Nov-2019 Alert if user is complying on same date 
            If Save() Then
                'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
                If AppSettings("LinkMaintenance") = "True" Then
                    mMultiComplianceList = Session("mMultiComplianceList")
                    If Not mMultiComplianceList Is Nothing Then
                        If mMultiComplianceList.Count > 0 Then
                            If Session("From") = 1 Then 'Edit Record
                                MSGBoxCtrl.show("Alert !", "<BR>Other Maintenance Activity(s) linked with this maintenance activity.To Edit/Delete individual Maintenance Activity go to respective activity.", "", MsgBoxStyle.OkOnly, "LMAlert")
                                Exit Sub
                            End If
                        End If
                        Dim Result As Boolean
                        SetLinkedMaintenanceGridObject()
                        Dim LinkMaintenanceEvents As LinkedMaintenanceActivityEvents = New LinkedMaintenanceActivityEvents
                        LinkMaintenanceEvents.AssemblyLogInfo = "Assembly Directive: " & mDirectiveDetail 'setting Mark Log Detail ...

                        'LicenseNo = Session("LicenseNo")
                        'Dim EmployeeID As String = Session("EmployeeID")
                        'EmpName = Session("EmpName")

                        'EmployeeID = EmployeeID.ToString.TrimEnd(",")
                        'LicenseNo = LicenseNo.ToString.TrimEnd(",")
                        'EmpName = EmpName.ToString.TrimEnd(",")

                        Dim EmployeeID As String = Session("EmpID")
                        If Not EmployeeID Is Nothing Then
                            EmployeeID = EmployeeID.ToString.TrimEnd(",")
                        Else
                            EmployeeID = Guid.Empty.ToString
                        End If


                        EmpName = Session("EmpName")
                        If Not EmpName Is Nothing Then
                            EmpName = EmpName.ToString.TrimEnd(",")
                        Else
                            EmpName = ""
                        End If

                        LicenseNo = Session("LicenseNo")
                        If Not LicenseNo Is Nothing Then
                            LicenseNo = LicenseNo.ToString.TrimEnd(",")
                        Else
                            LicenseNo = ""
                        End If


                        Result = LinkMaintenanceEvents.SaveLinkedMaintenanceActivies(mMultiComplianceList, mAssemblyMonitorModStatus.DoneWONo, txtDoneOnDate.Text.ToString, mMachineMaintenance.LogID, mMachine.HourType, mMachine.ID, mAssemblyMonitorModStatus.AssemblyID, PeriodValues, mAssemblyMonitorModStatus.DoneRemark, LicenseNo, EmployeeID.ToString, EmpName, Trim(txtPlace.Text))
                        Session.Remove("EmpID")
                        Session.Remove("LicenseNo")
                        Session.Remove("EmpName")

                        If LinkMaintenanceEvents.ErrorStr.Length > 0 Then
                            Dim title As String = "Link Maintenance Alert !"
                            Dim message As String = LinkMaintenanceEvents.ErrorStr
                            MSGBoxCtrl.show(title, message, "", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        Else
                            MSGBoxCtrl.show("Alert !", "<BR>Other Maintenance Activity(s) linked with this maintenance activity.To Edit/Delete individual Maintenance Activity go to respective activity.", "", MsgBoxStyle.OkOnly, "LMAlert")
                            Exit Sub
                        End If
                    End If
                End If
                'End
                RemoveSession() 'Added By Vikrant on 25-Nov-2014
                Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
            Else
                upnlValidationSummary.Update()
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub btnSelectLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectLog.Click
        SetObject()
        SetGridObject()
        Session("mFromType") = 3
        Session("mMachineId") = mAssemblyStatus.MachineID.ToString
        Session("mAssemblyStatusId") = mAssemblyMonitorModStatus.AssemblyStatusID.ToString
        Session("mAssemblyID") = mAssemblyStatus.AssemblyID.ToString
        Session("mDoneOn") = CStr(IIf(txtDoneOnDate.Text = "", Today.Date.ToShortDateString, txtDoneOnDate.Text))
        'Added by Vikrant on 14-Mar-2016 for ALL11032016
        If mAssemblyStatus.InstalledOn.ToString <> "" Then
            If CDate(mAssemblyMonitorModStatus.DoneOn) <= CDate(mAssemblyStatus.InstalledOn) Then 'if Compliance date is same or less than Assembly Inst. Date
                Dim mFirstLogDetailAfterAssemblyInstallation As FirstLogDetailAfterAssemblyInstallation = FirstLogDetailAfterAssemblyInstallation.GetFirstLogDetailAfterAssemblyInstallation(mAssemblyStatus)
                Session("mFirstLogDetailAfterAssemblyInstallation") = mFirstLogDetailAfterAssemblyInstallation
            End If
        End If
        'End
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSelectLogWindow", "OpenSelectLogWindow();", True)
    End Sub
    Private Sub hdnBtnSelectLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnSelectLog.Click
        If CType(Session("FromLog"), Boolean) = True Then
            Dim clnAssemblyMonitorModStatus As AssemblyMonitorModStatus = mAssemblyMonitorModStatus.Clone
            If Session("From") = 0 Then 'New Record
                NewRecord(New Guid(LogID.ToString), txtDoneOnDate.Text)
            Else
                EditRecord(New Guid(LogID.ToString), txtDoneOnDate.Text, False)
            End If
            SetFromClone(clnAssemblyMonitorModStatus)
            'DataBindGrid()
            Session.Remove("FromLog")
            'Added by Saylee on 9th-Oct-2009
            Dim mLog As Log
            mLog = Log.GetLog(New Guid(LogID.ToString))
            Session("mLog") = mLog
            SetGridFromObject()
            DataBindGrid()
            ControlVisibility()
            SetTitle()
            upnlCurrentValueGrid.Update()
            upnlDoneOnValueGrid.Update()
            '===========================================
        Else
            Session.Remove("mLog")
        End If

    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        MarkLog(Util.Action.Close, "AssemblyModifications", "", Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Changed by vikrant on 27-July-2011
        RemoveSession()
        Session.Remove("FromLog")
        Session.Remove("IsBackFromCompliance") 'Added By Vikrant On 03-Jun-2016 For ALL03062016
        Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
    End Sub
    Private Sub dgMultiComplianceList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgMultiComplianceList.Sorting
        mMultiComplianceList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mMultiComplianceList") = mMultiComplianceList
        dgMultiComplianceList.DataSource = mMultiComplianceList
        dgMultiComplianceList.DataBind()
        upnlLinkMaintenance.Update()
    End Sub 'End
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click 'Added by Vikrant On 25-Nov-2014
        mAssemblyMonitorModStatus.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte
        GetAttachment()
        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0
        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        IsAttachmentDeleted = True
        mAssemblyMonitorModStatus.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mAssemblyMonitorModStatus.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mAssemblyMonitorModStatus.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mAssemblyMonitorModStatus.ID)
        End If
        Session("mFileAttach") = mFileAttach
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        ViewImage()
    End Sub 'End
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    'MLNo
    Private Sub imgbtnEmployeeLicence_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgbtnEmployeeLicence.Click
        If IsValid Then
            SetObject()
            Session("mMaintenanceID") = mAssemblyMonitorModStatus.ID
            mMaintenanceDoneByEmployees = mAssemblyMonitorModStatus.MaintenanceDoneByEmployees
            Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
            Session("MaintenanceDoneOnDate") = mAssemblyMonitorModStatus.DoneOn.ToString
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AddEmployeeLicNo", "AddEmployeeLicNo();", True)
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub hdnBtnMaintDoneBy_Click(sender As Object, e As System.EventArgs) Handles hdnBtnMaintDoneBy.Click
        For i As Integer = 0 To mMaintenanceDoneByEmployees.Count - 1
            Dim ID As Guid = mMaintenanceDoneByEmployees(i).ID
            If Not mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Add(mMaintenanceDoneByEmployees(i))
            ElseIf mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(ID).LicenceNo = mMaintenanceDoneByEmployees(i).LicenceNo
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(ID).RequiredManHours = mMaintenanceDoneByEmployees(i).RequiredManHours
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(ID).EmployeeID = mMaintenanceDoneByEmployees(i).EmployeeID
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(ID).EmployeeName = mMaintenanceDoneByEmployees(i).EmployeeName
            End If
            Session("EmpID") = Session("EmpID") + mMaintenanceDoneByEmployees(i).EmployeeID.ToString + ","
            Session("LicenseNo") = Session("LicenseNo") + mMaintenanceDoneByEmployees(i).LicenceNo.ToString + ","
            Session("EmpName") = Session("EmpName") + mMaintenanceDoneByEmployees(i).EmployeeName.ToString + ","
        Next
        For j As Integer = 0 To mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Count - 1
            If Not mMaintenanceDoneByEmployees.Contains(mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(j).ID) Then
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Remove(mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(j).ID, "")
            End If
        Next
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
        BindLicenceNo()
        SetLicenceCount() 'MLNo
        txtRequiredManHours.DataBind()
        upnlMonitoringStatusDetails.Update()
    End Sub
    Protected Sub txtLicenceNo_TextChanged(sender As Object, e As System.EventArgs)
        'SetObject()
        If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
            LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
            EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
        Else
            LicenseNo = Trim(txtLicenceNo.Text)
        End If
        DoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID
        Session("EmpID") = DoneByID.ToString
        Session("LicenseNo") = LicenseNo
        Session("EmpName") = EmpName

        If Not DoneByID.Equals(Guid.Empty) Then
            If mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(0).EmployeeID = DoneByID
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(0).LicenceNo = LicenseNo
                If Not mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Count > 1 Then 'If Condition added by Vikrant On 15-Apr-2021 to solve issue:Hours getting added for multiple licence no and if first licence no changed
                    mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(0).RequiredManHours = txtRequiredManHours.Text
                End If
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(0).EmployeeName = EmpName
            Else
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Add(mAssemblyMonitorModStatus.ID, 7, DoneByID, LicenseNo, txtRequiredManHours.Text, EmpName)
            End If

        Else
            If mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.RemoveAt(0)
            End If
        End If
        Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
        BindLicenceNo()
        SetLicenceCount()
        txtRequiredManHours.DataBind()
        upnlMonitoringStatusDetails.Update()
    End Sub
    Protected Sub txtRequiredManHours_TextChanged(sender As Object, e As System.EventArgs)
        If mAssemblyMonitorModStatus.MaintenanceDoneByEmployees.Count > 0 Then
            mAssemblyMonitorModStatus.MaintenanceDoneByEmployees(0).RequiredManHours = txtRequiredManHours.Text
            Session("mAssemblyMonitorModStatus") = mAssemblyMonitorModStatus
            upnlMonitoringStatusDetails.Update()
        End If
    End Sub
    'End
    'Revise Activity
    Private Sub btnRevise_Click(sender As Object, e As System.EventArgs) Handles btnRevise.Click
        MSGBoxCtrl.show("Alert!", "You are about to Revise Model Activity.After revision of model activity this Status will become Not Applicable.", "Do you want to continue?", MsgBoxStyle.YesNo, "ReviseActivity")
    End Sub
    'End

    Private Sub btnSendMail_Click(sender As Object, e As System.EventArgs) Handles btnSendMail.Click
        Dim Str As String

        SetUserMailIDs()

        Session("btnSendMail") = "btnSendMail"
        Str = "OpenByMaiWindow();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
    End Sub
    Private Sub hdnimgBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnSendMail.Click
        Dim email As Thread
        Try
            If (Not User.IsInRole("AssemblyModificationsPrint")) Then
                MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            Print(True)


            Dim str As String
            Dim mSendMailFile As New SendMailFile


            str = str + ("<html>" & "<head>" & "</head>" & "<body >" & "<P><font face=""Calibri"">Following Directive(s) has been Updated / Revised in FlyPal System and need your attentions." + "</font></P></br> ")
            str = str + ("<font face=""Calibri"">Please Login to FlyPal® for detailed information." + "</font> ")

            str = str + ("<p><font face=""Calibri"">")
            str = str + ("<b>Directive Type: " + "</b>" + mAssemblyMonitorModStatus.ModelMonitorMod.ModelMonitorModTypeName + "<b> Directive Number:</b> " + mAssemblyMonitorModStatus.ModelMonitorMod.Number + "<b>" + " Description: " + "</b>" + mAssemblyMonitorModStatus.ModelMonitorMod.Description)
            str = str + ("</font></p>")

            str = str + ("<p><font face=""Calibri"">")


            str = str + ("<b>" + " Effective Date: " + "</b>" + mAssemblyMonitorModStatus.ModelMonitorMod.IssueDateFormatted + "<b>" + " Done On Date: " + "</b>" + mAssemblyMonitorModStatus.DoneOnFormatted)
            str = str + ("</font></p>")
            Dim mRemarkNote As String = ""


            If mAssemblyMonitorModStatus.DoneRemark = "" And mAssemblyMonitorModStatus.ModelMonitorMod.Note = "" Then
                mRemarkNote = ""
            ElseIf mAssemblyMonitorModStatus.DoneRemark <> "" And mAssemblyMonitorModStatus.ModelMonitorMod.Note = "" Then
                mRemarkNote = "<b>Remark / Note: " + "</b>" + mAssemblyMonitorModStatus.DoneRemark
            ElseIf mAssemblyMonitorModStatus.DoneRemark = "" And mAssemblyMonitorModStatus.ModelMonitorMod.Note <> "" Then
                mRemarkNote = "<b>Remark / Note: " + "</b>" + mAssemblyMonitorModStatus.ModelMonitorMod.Note
            ElseIf mAssemblyMonitorModStatus.DoneRemark <> "" And mAssemblyMonitorModStatus.ModelMonitorMod.Note <> "" Then
                mRemarkNote = "<b>Remark / Note: " + "</b>" + mAssemblyMonitorModStatus.DoneRemark + "/ " + mAssemblyMonitorModStatus.ModelMonitorMod.Note
            Else
                mRemarkNote = ""
            End If

            str = str + ("<p><font face=""Calibri"">")
            str = str + mRemarkNote
            str = str + ("</font></p>")

            str = str + ("<p><font face=""Calibri"">")
            str = str + ("<b>Soft Copy Available: " + "</b>" + "Attached" + "<b>")
            str = str + ("</font></p>")

            str = str + ("</body></html>")

            SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, "Directive Revision Notification", , str, _
                                    "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                     SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))

            Dim mDirectiveDetail As String = "Directive Revision Notification sent successfully to " + Session("ToSendMailIDs") + " by " + User.Identity.Name
            MarkLog(Util.Action.SendMail, "Directive", mDirectiveDetail, Util.ErrorType.HandledError, mAssemblyMonitorModStatus.ID, EventLogID)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTransDetail", MessageBox.Show("Mail Sent Successfully", False), True)

        Catch ex As Exception
            Dim Day, Month, Year As String
            Day = Format(Today.Date.Day, "0#")
            Month = Format(Today.Date.Month, "0#")
            Year = Format(Today.Date.Year, "0#")
            Dim todaydate As String = Day & Month & Year
            Dim Path As String = AppSettings("DOCPath") & todaydate
            FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (hdnimgBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        End Try

    End Sub

    'Added by Shital on 18-May-2021
    Private Sub lnkPrintLogBookEntry_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPrintLogBookEntry.Click  'Added By Prashant On 7-May-2021 ALL07052021
        Dim RptCommonHistory As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mLogEntryFormat As New LogEntryFormat
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsReportHistoryList
        Dim mCompanyDetail As New CompanyDetail

        RptCommonHistory = New crptLogEntryFormat

        mLogEntryFormat = LogEntryFormat.GetHistoryList(mAssemblyMonitorModStatus.DoneOn, mAssemblyMonitorModStatus.DoneOn, "", mAssemblyStatus.AssemblyTypeName, _
                                                        mAssemblyStatus.ModelName, mAssemblyStatus.Assembly.SerialNo, "", "", "", "", _
                                                        mAssemblyStatus.MachineID.ToString, True, False, IsRemoved:=False, IsInstalled:=True, _
                                                        IsComplied:=False, AssemblyID:=mAssemblyStatus.AssemblyID.ToString, IsLogNo:=True, _
                                                        IsLogPageNo:=False, IsFlightNo:=False, IsMELRequired:=False, IsMaintenanceActivityRequired:=False, _
                                                        AssemblyTypeID:=mAssemblyStatus.AssemblyTypeID, CompStatusID:=mAssemblyStatus.ID.ToString, _
                                                        ShowService:=False, ShowDir:=True, ShowInsp:=False, AssemblyMonitorModStatusID:=mAssemblyMonitorModStatus.ID.ToString)
        If mLogEntryFormat.Count = 0 Then
            Exit Sub
        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
           mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
           mCompanyDetail.WebSite, "LOG BOOK ENTRY", "", mAssemblyMonitorModStatus.DoneOnFormatted, Machine.GetMachine(mAssemblyStatus.MachineID).RegNo, _
           mAssemblyStatus.ModelName + "-" + mAssemblyStatus.Assembly.SerialNo, IIf(mAssemblyStatus.AssemblyTypeName.Equals("Airframe"), "AIRCRAFT", mAssemblyStatus.AssemblyTypeName.ToUpper), _
           AppSettings("Product Version"), AppSettings("SINote"), _
           "AVERAGE FUEL CONSUMPTION________LTR./HR & AVERAGE OIL CONSUMPTION________LTR./HR SINCE LAST SMI DONE.  BOTH THE FIGURES ARE BELOW THE ALERT VALUE.", _
           "True", mAssemblyMonitorModStatus.DoneOnFormatted, "", AppSettings("Logo"))


        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, "LogEntryFormat", mLogEntryFormat)      'This is direct from object records 

        da.Fill(ds, Report)
        da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
        RptCommonHistory.SetDataSource(ds)
        Session("CrystalReport") = RptCommonHistory
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "LogEntryFormat", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
#End Region

#Region " Report "
    'Created By :- Rajnish , Date -22/09/2006
#Region " Report Variable Declaration "
    Dim mCompanyDetail As New CompanyDetail
    Dim objStatus As rptStatus
#End Region

#Region " Event "
    Private Sub Print(Optional ByMail As Boolean = False)
        If (Not User.IsInRole("AssemblyModificationsPrint")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim Rpt As New crDetComplyAssemblyMonitorStatus
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList
        'For Current Value Grid
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 6 '5
        RHCount = Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Directive Type", _
                   txtModelMonitorModTypeName.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                   dgCurrentValue.Columns.Item(1).HeaderText, dgCurrentValue.Columns.Item(2).HeaderText, _
                    , dgCurrentValue.Columns.Item(3).HeaderText, , dgCurrentValue.Columns.Item(4).HeaderText, , , ))
        Else
            ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Directive Type", _
                            txtModelMonitorModTypeName.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                                  "", "", , "", , "", , , ))
        End If
        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Monitor Type", _
                             txtMonitorType.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).PeriodUnitName, String), _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).FrequencyValueFormatted, String), , _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).ElapsedValueFormatted, String), , _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).RemainingValueFormatted, String), , , ))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "ATA Chapter", _
                            txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , "", , , ))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "ATA Chapter", _
                             txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).PeriodUnitName, String), _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).FrequencyValueFormatted, String), , _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).ElapsedValueFormatted, String), , _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).RemainingValueFormatted, String), , , ))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "ATA Chapter", _
                            txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , "", , , ))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Reference", _
                             txtReference.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).PeriodUnitName, String), _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).FrequencyValueFormatted, String), , _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).ElapsedValueFormatted, String), , _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).RemainingValueFormatted, String), , , ))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Reference", _
                                txtReference.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                                "", "", , "", , "", , , ))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Directive Number", _
                                   txtModNumber.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).PeriodUnitName, String), _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).FrequencyValueFormatted, String), , _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).ElapsedValueFormatted, String), , _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).RemainingValueFormatted, String), , , ))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Directive Number", _
                                    txtModNumber.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , "", , , ))
                End If
            ElseIf I = 4 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Description", _
                                   txtDescription.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).PeriodUnitName, String), _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).FrequencyValueFormatted, String), , _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).ElapsedValueFormatted, String), , _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).RemainingValueFormatted, String), , , ))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Description", _
                                    txtDescription.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , "", , , ))
                End If
            ElseIf I = 5 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "", _
                                    "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).PeriodUnitName, String), _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).FrequencyValueFormatted, String), , _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).ElapsedValueFormatted, String), , _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).RemainingValueFormatted, String), , "Please Note: Elapsed and Remaining Values for Days/Months/Years will be in Days", ))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "", _
                                        "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , "", , "Please Note: Elapsed and Remaining Values for Days/Months/Years will be in Days", ))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "", _
                                         "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                                          CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).PeriodUnitName, String), _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).FrequencyValueFormatted, String), , _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).ElapsedValueFormatted, String), , _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(I).RemainingValueFormatted, String), , "Please Note: Elapsed and Remaining Values for Days/Months/Years will be in Days", ))
            End If
        Next

        'For Done On Value Grid
        Dim TotalCount1 As Integer
        Dim LHCount1 As Integer
        Dim RHCount1 As Integer
        LHCount1 = 6
        RHCount1 = Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count
        If LHCount1 > RHCount1 Then
            TotalCount1 = LHCount1
        Else
            TotalCount1 = RHCount1
        End If

        Dim temp1 As Integer
        temp1 = 0
        If temp1 < RHCount1 Then
            ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Done On", _
                                             New SmartDate(txtDoneOnDate.Text.ToString).FormattedText, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                                             dgDoneOnValue.Columns.Item(0).HeaderText, dgDoneOnValue.Columns.Item(1).HeaderText, _
                                          , dgDoneOnValue.Columns.Item(2).HeaderText, , dgDoneOnValue.Columns.Item(3).HeaderText, dgDoneOnValue.Columns.Item(4).HeaderText, , ))
        Else
            ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Done On", _
                            New SmartDate(txtDoneOnDate.Text.ToString).FormattedText, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                                  "", "", , "", , "", "", , ))
        End If
        Dim m As Integer
        For m = 0 To TotalCount1 - 1
            If m = 0 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Work Order No.", _
                    txtWorkOrderNo.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).PeriodUnitName, String), _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).CurrentValueFormatted, String), , _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).DueOnValueFormatted, String), CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , ))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Work Order No.", _
                            txtWorkOrderNo.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                                "", "", , "", , "", "", , ))
                End If
            ElseIf m = 1 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "License No.", _
                    mAssemblyMonitorModStatus.AllLicenceNosWithEmpName, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).PeriodUnitName, String), _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).CurrentValueFormatted, String), , _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).DueOnValueFormatted, String), CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , ))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "License No.", _
                            mAssemblyMonitorModStatus.AllLicenceNosWithEmpName, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                                "", "", , "", , "", "", , ))
                End If
            ElseIf m = 2 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Place ", _
                    txtPlace.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).PeriodUnitName, String), _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).CurrentValueFormatted, String), , _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).DueOnValueFormatted, String), CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , ))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Place ", _
                            txtPlace.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                                "", "", , "", , "", "", , ))
                End If
            ElseIf m = 3 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Actual Man Hours", _
                     txtRequiredManHours.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).PeriodUnitName, String), _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).CurrentValueFormatted, String), , _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).DueOnValueFormatted, String), CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , ))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Actual Man Hours", _
                            txtRequiredManHours.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                                "", "", , "", , "", "", , ))
                End If
            ElseIf m = 4 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Remark", _
                    txtRemark.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).PeriodUnitName, String), _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).CurrentValueFormatted, String), , _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).DueOnValueFormatted, String), CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , ))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Remark", _
                            txtRemark.Text, , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                                "", "", , "", , "", "", , ))
                End If
            ElseIf m = 5 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "", _
                    "", , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).PeriodUnitName, String), _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).CurrentValueFormatted, String), , _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).DueOnValueFormatted, String), CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), "Please Note: Started On/Current Values/Due On values for Days/Months/Years will be in Dates.  Extension Value for Calendar period should be entered in Days only.", ))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "", _
                    "", , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                           "", "", , "", , "", "", "Please Note: Started On/Current Values/Due On values for Days/Months/Years will be in Dates.  Extension Value for Calendar period should be entered in Days only.", ))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "", _
                                   "", , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).PeriodUnitName, String), _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).CurrentValueFormatted, String), , _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).DueOnValueFormatted, String), CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), "Please Note: Started On/Current Values/Due On values for Days/Months/Years will be in Dates.  Extension Value for Calendar period should be entered in Days only.", ))
            End If
        Next

        '***********************************************************************************************************************
        'For Document Details
        Dim TotalCount2 As Integer
        Dim LHCount2 As Integer
        Dim RHCount2 As Integer
        LHCount2 = 3
        RHCount2 = Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods.Count
        If LHCount2 > RHCount2 Then
            TotalCount2 = LHCount2
        Else
            TotalCount2 = RHCount2
        End If

        Dim temp2 As Integer
        temp2 = 0
        If temp2 < RHCount2 Then
            ReportDetails.Add(New rptStatus(, 2, "Document Details", "Revision No.", _
            txtRevisionNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
            dgDoneOnValue.Columns.Item(0).HeaderText, dgDoneOnValue.Columns.Item(1).HeaderText, "Extension Date ", _
            dgDoneOnValue.Columns.Item(2).HeaderText, txtExtensionDate.Text, dgDoneOnValue.Columns.Item(3).HeaderText, _
            dgDoneOnValue.Columns.Item(4).HeaderText, ))
        Else
            ReportDetails.Add(New rptStatus(, 2, "Document Details", "Revision No.", _
                                txtRevisionNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                                      "", txtExtensionDate.Text, , "", , "", ""))
        End If
        Dim n As Integer
        For n = 0 To TotalCount2 - 1
            If n = 0 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Page No.", _
                    txtPageNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).FrequencyValueFormatted, String), "Approval Remark", _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).DoneOnValueFormatted, String), txtApprovalRemark.Text, _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).CurrentValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Page No.", _
                        txtPageNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                        "", txtApprovalRemark.Text, , "", , "", ""))
                End If
            ElseIf n = 1 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Book No.", _
                    txtBookNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).DoneOnValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).CurrentValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Book No.", _
                        txtBookNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    "", "", , "", , "", ""))
                End If
            ElseIf n = 2 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Source Doc ", _
                    txtSourceDoc.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).DoneOnValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).CurrentValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Source Doc ", _
                        txtSourceDoc.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    "", "", , "", , "", ""))
                End If

            Else
                ReportDetails.Add(New rptStatus(, 2, "Document Details", "", _
                "", , , , , , , , , , , , , , , , , lblAssemblyValues.InnerText, _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).PeriodUnitName, String), _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).FrequencyValueFormatted, String), , _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).DoneOnValueFormatted, String), , _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).CurrentValueFormatted, String), _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).ExtensionValueFormatted, String), _
                CType(Me.mAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriods(n).DueOnValueFormatted, String), lblNote1.Text))
            End If
        Next
        '***********************************************************************************************************************

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Comply Assembly Directives Status Detail Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))
        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        Dim Str1 As String
        Str1 = "openTranDetail();"

        If ByMail = True Then 'Added By Prashant 1-Nov-2018  StarAir1112018
            'Do nothing 
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
        End If


    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Print()
    End Sub
#End Region

#End Region

#Region "Service Methods"
    'MLNo
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetLicenseNoList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim mLicenses As LicenseNoListWithEmployee
        mLicenses = LicenseNoListWithEmployee.GetLicenseNoList(prefixText, UserNameForLicenceList, , , False)

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