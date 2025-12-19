
'AJAX Conversion By Saylee On 20-Apr-2015
Imports System.Linq
Imports System.Text 'Added By Vikrant On 17-Sep-2020 For Mismatch Value Mail Send
Public Class wfComplyCompMonitorModStatus_AJAX
    Inherits System.Web.UI.Page


#Region "Enumeration"
    Private Enum MaintenanceType
        AssemblyInstallation = 1
        AssemblyRemoval = 2
        ComponentInstallation = 3
        ComponentRemoval = 4
        AssemblyService = 5
        AssemblyInspection = 6
        AssemblyDirective = 7
        ComponentService = 8
        ComponentInspection = 9
        ComponentModification = 10
    End Enum
#End Region

#Region " Enum "
    Public Enum From
        NewRecord = 0
        EditRecord = 1
    End Enum
#End Region

#Region " Variable Declaration "
    Public mEnFrom As From
    Dim Flag As Int16
    Public mMachine As Machine
    Public mCompStatus As CompStatus
    Public mAssemblyStatus As AssemblyStatus
    Public mCompMonitorModStatus As CompMonitorModStatus
    Public mPrevCompMonitorModStatus As CompMonitorModStatus
    Public mCompInfo As String
    Public ComplyCompMonitorModInfo As String

    Public mMachineMaintenance As MachineMaintenance 'Added by Saylee on 9th-Oct-2009
    Public mMachineMaintenanceList As MachineMaintenanceList 'Added by Saylee on 9th-Oct-2009

    Dim EventLogID As Guid 'Added By Utkarsh On 28-Jul-2011 For All19072011
    Dim MaintDetail As String 'Added By Utkarsh On 28-Jul-2011 For All19072011
    Dim mEmployeeStatus As EmployeeStatus 'Added By Vikrant On 06-Aug-2013 For ALL01082013
    'Added By Prashant On 27-Nov-2014
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    'End

    Public mMonitorInfo As String
    Public mMonitorType As String
    Public mMonitorDesc As String
    Public mPart As String
    Public mSerialNo As String

    'MLNo
    Dim LicenseNo As String = String.Empty
    Dim EmpName As String = String.Empty
    Dim DoneByID As Guid = Guid.Empty
    Dim mMaintenanceDoneByEmployees As New MaintenanceDoneByEmployees
    Shared UserNameForLicenceList As String
    'End
    Public OverDueString As String = ""
    Public mIsSpareComponent As Integer 'Added By Vikrant On 27-Jul-2020 For ALL27072020
    Dim mHourType As Integer = 1 'Added By Vikrant On 30-Nov-2020 For Spare Comp FLow
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mEnFrom = CType(Session("EnFrom"), From)
        mMachine = CType(Session("mMachine"), Machine)
        mCompMonitorModStatus = CType(Session("mCompMonitorModStatus"), CompMonitorModStatus)
        mPrevCompMonitorModStatus = CType(Session("mPrevCompMonitorModStatus"), CompMonitorModStatus)
        mCompStatus = CType(Session("mCompStatus"), CompStatus)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)

        mCompInfo = Session("mCompInfo") 'Added by Saylee on 5-Aug-2009

        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 9th-Oct-2009
        mMachineMaintenanceList = CType(Session("mMachineMaintenanceList"), MachineMaintenanceList) 'Added by Saylee on 9th-Oct-2009
        'Added By Prashant  On 27-Nov-2014
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        'End
        'MLNo
        mMaintenanceDoneByEmployees = Session("mMaintenanceDoneByEmployees")
        UserNameForLicenceList = Session("UserNameForLicenceList")
        'End
        mIsSpareComponent = Session("mIsSpareComponent") 'Added By Vikrant On 27-Jul-2020 For ALL27072020
    End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
        Session("mPrevCompMonitorModStatus") = mPrevCompMonitorModStatus
        Session("mCompStatus") = mCompStatus
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("EnFrom") = mEnFrom

        Session("mCompInfo") = mCompInfo 'Added By Saylee on 5-Aug-2009

        Session("mMachineMaintenance") = mMachineMaintenance            'Added by Saylee on 9th-Oct-2009
        Session("mMachineMaintenanceList") = mMachineMaintenanceList    'Added by Saylee on 9th-Oct-2009
        Session("mFileAttach") = mFileAttach 'Added By Prashant  On 27-Nov-2014
        Session("IsAttachmentDeleted") = IsAttachmentDeleted 'Added By Prashant  On 27-Nov-2014
    End Sub
    Private Sub RemoveSession()
        mCompMonitorModStatus = Nothing
        Session.Remove("EnFrom")
        Session.Remove("mCompMonitorModStatus")

        Session.Remove("mMachineMaintenance")       'Added by Saylee on 9th-Oct-2009
        Session.Remove("mMachineMaintenanceList")   'Added by Saylee on 9th-Oct-2009
        'Added By Prashant On 27-Nov-2014
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        'End
        Session.Remove("ConsiderAssemblyInstValue")
        Session.Remove("mFirstLogDetailAfterAssemblyInstallation")

        'MLNo
        Session.Remove("mMaintenanceDoneByEmployees")
        Session.Remove("UserNameForLicenceList")
        'End
    End Sub

    Private Sub SetObject()
        If Not IsDate(txtDoneOnDate.Text) Then
            mCompMonitorModStatus.DoneOn = System.DBNull.Value
        Else
            mCompMonitorModStatus.DoneOn = txtDoneOnDate.Text
        End If
        mCompMonitorModStatus.DoneWONo = Trim(txtWorkOrderNo.Text)
        mCompMonitorModStatus.DoneRemark = Trim(txtRemark.Text)
        mCompMonitorModStatus.RequiredManHours = Trim(txtActualManHours.Text)

        'Added By Saylee on 28-07-2008=======================
        'CNDC
        If Not IsDate(txtExtensionDate.Text) Then
            mCompMonitorModStatus.ExtensionDate = System.DBNull.Value
        Else
            mCompMonitorModStatus.ExtensionDate = txtExtensionDate.Text
        End If

        mCompMonitorModStatus.ApprovalRemark = Trim(txtApprovalRemark.Text)
        '====================================================
        With mCompMonitorModStatus
            .IsApplicable = chkApplicable.Checked   'Added By Vaishali on 19-Nov-2008
        End With

        mCompMonitorModStatus.DoneBy = txtDoneBy.Text 'Added by Saylee On 23-Apr-2009

        ' Added By Utkarsh On 12-Jun-2012 FOR ALL08062012

        Dim LicenseNo As String = String.Empty
        Dim EmpName As String = String.Empty
        If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
            LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
            EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
        Else
            LicenseNo = Trim(txtLicenceNo.Text)
        End If
        mCompMonitorModStatus.LicenseNo = LicenseNo
        mCompMonitorModStatus.DoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID

        'End

        'Added by Saylee On 26-Apr-2012
        mCompMonitorModStatus.Place = txtPlace.Text.Trim
        '*********************************************
        'Added By Prashant On 27-Nov-2014
        If mFileAttach.Size > 0 Then
            mCompMonitorModStatus.IsAttachmentAdded = True
        Else
            mCompMonitorModStatus.IsAttachmentAdded = False
        End If
        'End

        mCompMonitorModStatus.SourceDoc = Trim(txtSourceDoc.Text)
        mCompMonitorModStatus.RevisionNo = Trim(txtRevisionNo.Text)
        mCompMonitorModStatus.BookNo = Trim(txtBookNo.Text)
        mCompMonitorModStatus.PageNo = Trim(txtPageNo.Text)
        mCompMonitorModStatus.MethodOfCompliance = Trim(txtMethodOfCompliance.Text)  'Added By Saylee on 10-Oct-2024
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
    End Sub
    Public Sub SetGridObject()
        Dim txtCurrentValue, txtExtensionValue As TextBox
        Dim j As Int32
        For j = 0 To Me.dgDoneOnValue.Rows.Count - 1
            txtCurrentValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtCurrentValue"), TextBox)
            'Added By Saylee on 28-07-2008
            txtExtensionValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtExtensionValue"), TextBox)
            With mCompMonitorModStatus.CompMonitorModStatusPeriods
                If .Item(j).PeriodID = 2 Then
                    If Not Period.IsDate(txtCurrentValue.Text.Trim) Then
                        .Item(j).CurrentValue = ""
                    Else
                        .Item(j).CurrentValueFormatted = Trim(txtCurrentValue.Text)
                    End If
                Else
                    .Item(j).CurrentValue = Trim(txtCurrentValue.Text)
                End If

                'Added By Saylee on 28-07-2008
                'ExtensionValue
                .Item(j).ExtensionValue = Trim(txtExtensionValue.Text)
            End With
        Next j
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
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
            With mPrevCompMonitorModStatus.CompMonitorModStatusPeriods ''mPrevCompMonitorModStatus object contains previous period values
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

        Session("mCompMonitorModStatus") = mCompMonitorModStatus

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
        For j = 0 To mCompMonitorModStatus.CompMonitorModStatusPeriods.Count - 1
            With mCompMonitorModStatus.CompMonitorModStatusPeriods
                If .Item(j).PeriodID = 2 Then
                    If Not Period.IsDate(mCompMonitorModStatus.CompMonitorModStatusPeriods(j).CurrentValueFormatted) Then
                        .Item(j).CurrentValue = ""
                    Else
                        .Item(j).CurrentValueFormatted = Trim(mCompMonitorModStatus.CompMonitorModStatusPeriods(j).CurrentValueFormatted)
                    End If
                Else
                    .Item(j).CurrentValue = Trim(mCompMonitorModStatus.CompMonitorModStatusPeriods(j).CurrentValueFormatted)
                End If

                'Added By Saylee on 28-07-2008
                'ExtensionValue
                .Item(j).ExtensionValue = Trim(mCompMonitorModStatus.CompMonitorModStatusPeriods(j).ExtensionValueFormatted)
            End With
        Next j
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
    End Sub
    Private Sub SetLog()
        If Val(Request.QueryString("Type")) = -1 Then
            'Dim LogId As Guid = New Guid(Request.QueryString("LogId"))
            'Dim LogDate = Request.QueryString("LogDate")

            Dim LogId As Guid = New Guid(CType(Session("LogID"), String))
            Dim LogDate = CType(Session("LogDate"), String)

            'If DateDiff(DateInterval.Day, SmartDate.StringToDate(mPrevCompMonitorModStatus.AsOnDate), SmartDate.StringToDate(LogDate)) > 0 Then
            '    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DoneOnDate, SIMsgBox.Message_text.DoneOnDate, "Compliance record only upto " & CStr(mPrevCompMonitorModStatus.AsOnDate) & " can be entered through Comp Installation screen", MsgBoxStyle.OKOnly)
            '    msg1.ReplacePage = "wfComplyCompMonitorModStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
            '    msg1.Show()
            '    Exit Sub
            'End If


            '******************************************************************
            'Added by Saylee on 11-Jan-2017
            'ConsiderCompInstValue=True only if Compliance date is less than Comp Inst Date then consider Current vlaue 
            'If False,then Comp Current Values will be calculated
            Dim ConsiderCompInstValue As Boolean = False
            If txtDoneOnDate.Text <> "" And mCompStatus.InstalledOn.ToString <> "" Then
                If CDate(mCompMonitorModStatus.DoneOn) < CDate(mCompStatus.InstalledOn) Then
                    ConsiderCompInstValue = True
                End If
            End If
            '******************************************************************

            Dim clnCompMonitorModStatus As CompMonitorModStatus = mCompMonitorModStatus.Clone
            If mEnFrom = From.NewRecord Then
                mCompMonitorModStatus = CompMonitorModStatus.NewComplyCompMonitorModStatus(Guid.NewGuid, mPrevCompMonitorModStatus.CompID, mPrevCompMonitorModStatus.AssemblyStatusID, LogDate, mCompStatus.Comp.PartID, mPrevCompMonitorModStatus.PartMonitorMod, LogId, mPrevCompMonitorModStatus.CompStatusID, mPrevCompMonitorModStatus.DoneOn.ToString, mHourType, , ConsiderCompInstValue)
            Else
                mCompMonitorModStatus = CompMonitorModStatus.GetComplyCompMonitorModStatus(mPrevCompMonitorModStatus.ID, mPrevCompMonitorModStatus.AssemblyStatusID, mPrevCompMonitorModStatus.CompStatusID, LogDate, LogId, mHourType, , ConsiderCompInstValue)
            End If
            mCompMonitorModStatus.DoneWONo = clnCompMonitorModStatus.DoneWONo
            mCompMonitorModStatus.DoneRemark = clnCompMonitorModStatus.DoneRemark
            mCompMonitorModStatus.DoneOn = clnCompMonitorModStatus.DoneOn
            mCompMonitorModStatus.RequiredManHours = clnCompMonitorModStatus.RequiredManHours
            'mCompMonitorModStatus.CompMonitorModStatusPeriods = clnCompMonitorModStatus.CompMonitorModStatusPeriods
            mCompMonitorModStatus.IsAttachmentAdded = clnCompMonitorModStatus.IsAttachmentAdded
            'Added By Vikrant on 15-Apr-2021 to solve issue: Licence No not getting saved after select log
            For j As Integer = mCompMonitorModStatus.MaintenanceDoneByEmployees.Count - 1 To 0 Step -1
                mCompMonitorModStatus.MaintenanceDoneByEmployees.RemoveAt(j)
            Next
            For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In clnCompMonitorModStatus.MaintenanceDoneByEmployees
                If Not mCompMonitorModStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.ID) Then
                    mCompMonitorModStatus.MaintenanceDoneByEmployees.Add(mCompMonitorModStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
                Else
                    If Not mCompMonitorModStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.EmployeeID) Then
                        mCompMonitorModStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).EmployeeID = mMaintenanceDoneByEmployee.EmployeeID
                        mCompMonitorModStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).LicenceNo = mMaintenanceDoneByEmployee.LicenceNo
                        mCompMonitorModStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).RequiredManHours = mMaintenanceDoneByEmployee.RequiredManHours
                        mCompMonitorModStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).EmployeeName = mMaintenanceDoneByEmployee.EmployeeName
                    End If
                End If
            Next
            'End
            If Not mFileAttach Is Nothing Then
                mFileAttach.ReferenceID = mCompMonitorModStatus.ID
                Session("mFileAttach") = mFileAttach
            End If
            Session("mCompMonitorModStatus") = mCompMonitorModStatus
            clnCompMonitorModStatus = Nothing

            'Added by Saylee on 9th-Oct-2009
            Dim mLog As Log
            mLog = Log.GetLog(New Guid(LogId.ToString))
            Session("mLog") = mLog
            '===================================
        Else
            Session.Remove("mLog")
        End If
    End Sub
    Private Sub NewRecord(ByVal LogID As Guid, ByVal LogDate As String)
        'Commented and Added By Vikrant On 08-May-2014 For ALL08052014

        ''----------------Added by Saylee on 04-July-2013 for ALL04072013-------------
        'Dim mAssemblyStatusList As AssemblyStatusList
        'mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(LogDate, mMachine.ID.ToString _
        ', , , , , , , , , , True, True, , mAssemblyStatus.AssemblyID.ToString, , , , , , , mPrevCompMonitorModStatus.CompID.ToString, , , , , , , _
        ', , ).Item(0), MachineInfo).AssemblyStatusList

        'If mAssemblyStatusList.Count = 0 Then
        '    mAssemblyStatusList = CType(MachineList.GetMachineListWithRemoval(LogDate, mMachine.ID.ToString _
        '           , , , , , , , , , , True, True, , mAssemblyStatus.AssemblyID.ToString, , , , , , , mPrevCompMonitorModStatus.CompID.ToString, , , , , , , _
        '           , ).Item(0), MachineInfo).AssemblyStatusList
        'End If
        ''-----------------------------

        Dim mAssemblyStatusList As AssemblyStatusList
        Dim mMachineList As MachineList
        Dim LatestRemovedOn As SmartDate
        Dim AssemblyStatusID As Guid = Guid.Empty
        Dim CompStatusID As Guid = Guid.Empty

        mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(LogDate, mMachine.ID.ToString _
        , , , , , , , , , , True, True, , mAssemblyStatus.AssemblyID.ToString, , , , , , , mPrevCompMonitorModStatus.CompID.ToString, , , , , , ,
        , , SkipIsForInventoryAircarft:=True, MonitoringInspRequired:=False, MonitoringModRequired:=False,
            MonitoringServiceRequired:=False, CompMonitoringInspRequired:=False, CompMonitoringModRequired:=False,
            CompMonitoringServiceRequired:=False).Item(0), MachineInfo).AssemblyStatusList

        If mAssemblyStatusList.Count = 0 Then
            mMachineList = MachineList.GetMachineListWithRemoval(LogDate, mMachine.ID.ToString _
                   , , , , , , , , , , True, True, , mAssemblyStatus.AssemblyID.ToString, , , , , , , mPrevCompMonitorModStatus.CompID.ToString, , , , , , ,
                       , SkipIsForInventoryAircarft:=True)
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
                    CompStatusID = mtempAssemblyList(0).CompStatusList(0).ID

                End If
            Next
        Else
            AssemblyStatusID = mAssemblyStatusList(0).ID
            CompStatusID = mAssemblyStatusList(0).CompStatusList(0).ID
        End If
        'End

        'Here instead of mPrevCompMonitorModStatus.AssemblyStatusID pass mAssemblyStatusList(0).ID  
        'Here instead of mPrevCompMonitorModStatus.CompStatusID pass mAssemblyStatusList(0).CompStatusList(0).ID

        'mCompMonitorModStatus = CompMonitorModStatus.NewComplyCompMonitorModStatus(Guid.NewGuid, mPrevCompMonitorModStatus.CompID, mPrevCompMonitorModStatus.AssemblyStatusID, LogDate, mCompStatus.Comp.PartID, mPrevCompMonitorModStatus.PartMonitorMod, LogID, mPrevCompMonitorModStatus.CompStatusID, mPrevCompMonitorModStatus.DoneOn.ToString, mHourType)
        mCompMonitorModStatus = CompMonitorModStatus.NewComplyCompMonitorModStatus(Guid.NewGuid, mPrevCompMonitorModStatus.CompID, AssemblyStatusID, LogDate, mCompStatus.Comp.PartID, mPrevCompMonitorModStatus.PartMonitorMod, LogID, CompStatusID, mPrevCompMonitorModStatus.DoneOn.ToString, mHourType)
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
        SetTitle()
    End Sub
    Private Sub EditRecord(ByVal LogID As Guid, ByVal DoneOnDate As String, ByVal FromEntry As Boolean)
        REM:-FromEntry is used for avoiding object Dirty at form load when we r coming thru' Edit.
        If FromEntry = False Then
            mCompMonitorModStatus = CompMonitorModStatus.GetComplyCompMonitorModStatus(mPrevCompMonitorModStatus.ID, mPrevCompMonitorModStatus.AssemblyStatusID, mPrevCompMonitorModStatus.CompStatusID, DoneOnDate, LogID, mHourType)
        Else
            mCompMonitorModStatus = CompMonitorModStatus.GetComplyCompMonitorModStatusFromEntry(mPrevCompMonitorModStatus.ID, mPrevCompMonitorModStatus.AssemblyStatusID, mPrevCompMonitorModStatus.CompStatusID, DoneOnDate, mHourType)
        End If
        mCompMonitorModStatus.BeginEdit()
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
        SetTitle()
    End Sub
    'Added By Vikrant On 17-Sep-2020 For Mismatch Value Mail Send
    Private Sub SendMail(ByVal ModStatus As CompMonitorModStatus, ByVal DoneOnValue As String)
        Dim str As New StringBuilder
        Try
            str.Append("Mismatch Details for <b>" & IIf(Session("From") = 1, "Edited", IIf(ModStatus.IsNew, "New", "New but Saved")) & "</b> record are as follows: ")
            str.Append("<p><b>Assembly Details: </b> " & mAssemblyStatus.Assembly.ModelName & " " & mAssemblyStatus.Assembly.SerialNo & "</p>")
            str.Append("<p><b>Component Details: </b> " & mCompStatus.Comp.PartName & " " & mCompStatus.Comp.SerialNo & "</p>")
            str.Append("<p><b>Modification ID: </b> " & ModStatus.ID.ToString & "</p>")
            str.Append("<p><b>Modification Description: </b> " & ModStatus.PartMonitorMod.Description & "</p>")
            str.Append("<p><b>Done On Date: </b> " & txtDoneOnDate.Text & "</p>")
            str.Append("<p><b>Done On Value: </b> " & DoneOnValue & "</p>")
            str.Append("<p><b>Saved By: </b> " & User.Identity.Name)

			SendMailFile.SendMailFile(Nothing, User.Identity.Name, "FAS: Component Modification Done on Date Done on Value Mismatch Details", "", Info:=str.ToString, VendorEmailID:="", ToMailID:="saylee@bytzsoft.com")
		Catch ex As Exception
            Dim Title As String = "Error Sending Mail"
            Dim Message As String = ex.InnerException.ToString
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(Title, Message, , False), True)
            Exit Sub
        End Try
    End Sub
    'End
    Private Function Save() As Boolean
        Dim clnCompMonitorModStatus As CompMonitorModStatus
        clnCompMonitorModStatus = CType(mCompMonitorModStatus.Clone, CompMonitorModStatus)
        SetObject()
        SetGridObject()
        SetMachineMaintenanceObject() 'Added by Saylee on 9th-Oct-2009
        If mCompMonitorModStatus.IsValid Then
            If mCompMonitorModStatus.CompMonitorModStatusPeriods.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodRequired, MSGBox.Message_text.PeriodRequired, "You are trying to save Component Modification Status.Component Modification Status can not be saved without period units.", MsgBoxStyle.OkOnly, "")
                Return False
            End If
            Try
                'Added By Vikrant On 06-Aug-2013 For ALL01082013
                If Not mCompMonitorModStatus.DoneByID.Equals(Guid.Empty) AndAlso Not mCompMonitorModStatus.DoneOn.Equals(System.DBNull.Value) Then
                    Dim title As String = "Save Alert !"
                    Dim message As String = ""
                    mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mCompMonitorModStatus.DoneByID.ToString, mCompMonitorModStatus.DoneOn)
                    If (mEmployeeStatus(0).Information <> "") Then
                        message = mEmployeeStatus(0).Information
                        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenAlertMessage", MessageBox.Show(title, message, IsTagRequired:=False), True)
                        MSGBoxCtrl.Show(title, message, "", MsgBoxStyle.OkOnly, "")
                        Return False
                    End If
                End If
                'End
                'Added By Vikrant On 17-Sep-2020 For Mismatch Value Mail Send
                If txtDoneOnDate.Text <> "" AndAlso mCompMonitorModStatus.CompMonitorModStatusPeriods.Contains(2, "") Then 'If date period conatins then only execute
                    Dim DoneOnValue As New StringBuilder
                    For j As Integer = 0 To Me.dgDoneOnValue.Rows.Count - 1
                        DoneOnValue.Append(CType(Me.dgDoneOnValue.Rows(j).FindControl("txtCurrentValue"), TextBox).Text + ", ")
                        If mCompMonitorModStatus.CompMonitorModStatusPeriods(j).PeriodID = 2 Then
                            If Not txtDoneOnDate.Text.Equals(CType(Me.dgDoneOnValue.Rows(j).FindControl("txtCurrentValue"), TextBox).Text) Then
                                Session("IsSendMail") = "True"
                            End If
                        End If

                    Next j
                    If Session("IsSendMail") = "True" Then
                        Session.Remove("IsSendMail")
                        SendMail(mCompMonitorModStatus, DoneOnValue.ToString.Trim.TrimEnd(","))
                    End If
                End If
                'End
                mCompMonitorModStatus.ApplyEdit()
                mCompMonitorModStatus = CType(mCompMonitorModStatus.Save(), CompMonitorModStatus)
                SaveAttachment() 'Added By Vikrant On 25-Nov-2014
                SaveMachineMaintenance()  'Added by Saylee on 9th-Oct-2009
                Session("mCompMonitorModStatus") = mCompMonitorModStatus
                mCompInfo = Session("mCompInfo")
                'Changed by Vikrant on 28-July-2011
                Dim mDoneOnValues As New System.Text.StringBuilder
                For i As Integer = 0 To mCompMonitorModStatus.CompMonitorModStatusPeriods.Count - 1
                    mDoneOnValues.Append(mCompMonitorModStatus.CompMonitorModStatusPeriods(i).DoneOnValueFormatted + ",")
                Next
                ''MarkLog(Util.Action.Save, "ComplyCompMonitorModStatus", mCompInfo + "   " + ComplyCompMonitorModInfo, Util.ErrorType.NoError, mCompMonitorModStatus.ID)

                'Commented By Utkarsh On 28-Jul-2011 For All19072011

                '     MarkLog(Util.Action.Save, "ComponentModifications", mCompInfo, Util.ErrorType.NoError, mCompMonitorModStatus.ID)
                'End
                Return True
            Catch ex As SqlException
                Session("mCompMonitorModStatus") = clnCompMonitorModStatus
                If ex.Number = 8114 Or ex.Number = 8115 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NumericOverFlow, MSGBox.Message_text.NumericOverFlow, " Rate or Qty or Conversion Factor. ", MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 547 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, ex.Procedure, MsgBoxStyle.OkOnly, "")
                End If
                Return False
            Finally
                clnCompMonitorModStatus = Nothing
                Dim mDoneOnValues As New System.Text.StringBuilder
                For i As Integer = 0 To mCompMonitorModStatus.CompMonitorModStatusPeriods.Count - 1
                    mDoneOnValues.Append(mCompMonitorModStatus.CompMonitorModStatusPeriods(i).DoneOnValueFormatted + ",")
                Next
                'MaintDetail = "Reg No. : " & mMachineMaintenance.RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorModStatus.PartMonitorMod.PartMonitorModTypeName & " Done On Date : " + mCompMonitorModStatus.DoneOnFormatted + " Done On Value : " + mDoneOnValues.ToString
                If mCompStatus.IsSpareComp = False Then
                    MaintDetail = "Reg No. : " & mMachineMaintenance.RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorModStatus.PartMonitorMod.PartMonitorModTypeName & " Done On Date : " + mCompMonitorModStatus.DoneOnFormatted.ToString + " Done On Values : " + mDoneOnValues.ToString.TrimEnd(",")
                Else
                    MaintDetail = "Stock Component : Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorModStatus.PartMonitorMod.PartMonitorModTypeName & " Done On Date : " + mCompMonitorModStatus.DoneOnFormatted + " Done On Values : " + mDoneOnValues.ToString.TrimEnd(",")
                End If
                MarkLog(Util.Action.Save, "ComponentModifications", MaintDetail, Util.ErrorType.NoError, mCompMonitorModStatus.ID, EventLogID)

            End Try
        Else
            Return False
        End If
    End Function
    Private Sub SetTitle()
        Dim CompInfo As String = "[Part: " & mCompStatus.PartName & " Serial No. : " & mCompStatus.SerialNo & " ]"
        If mCompMonitorModStatus.IsNew Then
            lblTitle.Text = IIf(mIsSpareComponent = 0, "", IIf(mCompStatus.IsSpareComp, "Stock ", "Removed ")) + "Comply Component Modification Status " & CompInfo & " [New]" 'mIsSpareAssembly Added By Vikrant On 27-Jul-2020 For ALL27072020
        Else
            lblTitle.Text = IIf(mIsSpareComponent = 0, "", IIf(mCompStatus.IsSpareComp, "Stock ", "Removed ")) + "Comply Component Modification Status" & CompInfo 'mIsSpareAssembly Added By Vikrant On 27-Jul-2020 For ALL27072020
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Save" Then
                        Save()
                    ElseIf (MSGBoxCtrl.Sender = "OverDue" Or MSGBoxCtrl.Sender = "ComplyOnSameDate") Then 'Added by Saylee on 26-Mar-2019 for ALL26032019
                        'ComplyOnSameDate Added By Prashant 19-Nov-2019 Alert if user is complying on same date 
                        If Save() Then
                            If MSGBoxCtrl.Sender = "OverDue" Then
                                MarkLog(Util.Action.Save, "ComponentModifications", User.Identity.Name & " saved OverDue record : " & Session("OverDueString") & " " & Session("DueString"), Util.ErrorType.HandledError, mCompMonitorModStatus.ID, EventLogID)
                            ElseIf MSGBoxCtrl.Sender = "ComplyOnSameDate" Then
                                MarkLog(Util.Action.Save, "ComponentModifications", User.Identity.Name & " Comply On Same Date : ", Util.ErrorType.HandledError, mCompMonitorModStatus.ID, EventLogID)
                            End If
                            'Added By Prashant On 27-Nov-2014
                            Session.Remove("mFileAttach")
                            Session.Remove("IsAttachmentDeleted")
                            'End

                            'MLNo
                            Session.Remove("mMaintenanceDoneByEmployees")
                            Session.Remove("UserNameForLicenceList")
                            'End

                            'Added by Saylee on 5-Apr-2019
                            Session.Remove("mDoneOn")
                            Session.Remove("LogID")
                            Session.Remove("FromLog")
                            '***************************************

                            'Added by Saylee on 9th-Jan-2008 ===============================
                            If Request.QueryString("GChildPage4") <> "" Then
                                Response.Redirect(Request.QueryString("GChildPage4") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3")) 'Added Code
                            ElseIf Request.QueryString("GChildPage2") <> "" Then
                                Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
                            End If
                            '===============================================================
                        End If
                    End If


                Case MsgBoxResult.No

                Case MsgBoxResult.Cancel

                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added

                    Session("sender") = ""
                    DataFieldBind()
                    ControlVisibilityForDatePeriod()
                    'Response.Redirect("wfComplyAssemblyMonitorModStatus_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
        End If
    End Sub
    Private Sub ControlVisibility()
        btnPrint.Enabled = Not mCompMonitorModStatus.IsNew
        dgCurrentValue.Columns(3).Visible = Not mCompMonitorModStatus.PartMonitorMod.MonitorTypeID = 3
        dgCurrentValue.Columns(4).Visible = Not mCompMonitorModStatus.PartMonitorMod.MonitorTypeID = 3
        'Added By Saylee on 28-08-2008
        dgDoneOnValue.Columns(2).Visible = Not mCompMonitorModStatus.PartMonitorMod.MonitorTypeID = 3
        '==========================
        dgDoneOnValue.Columns(3).Visible = Not mCompMonitorModStatus.PartMonitorMod.MonitorTypeID = 3
        'Added By Utkarsh ON 26-Jun-2013 FOR ALL26062013-1
        dgDoneOnValue.Columns(4).Visible = (mCompMonitorModStatus.PartMonitorMod.MonitorTypeID <> 3) AndAlso (mCompStatus.AssemblyTypeID <> 1 AndAlso mCompMonitorModStatus.PartMonitorMod.MonitorTypeID <> 3) AndAlso mIsSpareComponent <> 1 'mIsSpareComponent Added By Vikrant On 27-Jul-2020 For ALL27072020
        dgDoneOnValue.Columns(5).Visible = Not mCompMonitorModStatus.PartMonitorMod.MonitorTypeID = 3
        'End
        If mCompMonitorModStatus.PartMonitorMod.ReadOnlyFrequencyColumn Then
            'txtDoneOnDate.Enabled = False 'Commented by Saylee on 22-Nov-2019 as DoneOne should be open in all cases, 
            chkApplicable.Enabled = False
        End If
        ControlVisibilityForAttachment()
        btnSelectLog.Visible = (mIsSpareComponent <> 1) ' Added By Vikrant On 27-Jul-2020 For ALL27072020
        lnkPrintLogBookEntry.Visible = (mIsSpareComponent <> 1)
    End Sub
    Private Sub ControlVisibilityForGridBeforeBinding()
        dgCurrentValue.Columns(3).Visible = True
        dgCurrentValue.Columns(4).Visible = True
        dgDoneOnValue.Columns(2).Visible = True
        dgDoneOnValue.Columns(3).Visible = True
        dgDoneOnValue.Columns(4).Visible = True
        dgDoneOnValue.Columns(5).Visible = True
    End Sub
    Private Sub CopyFromClone(ByVal clnCompMonitorModStatus As CompMonitorModStatus)
        mCompMonitorModStatus.DoneWONo = clnCompMonitorModStatus.DoneWONo
        mCompMonitorModStatus.DoneRemark = clnCompMonitorModStatus.DoneRemark

        'Added by Saylee On 26-Apr-2012
        mCompMonitorModStatus.DoneByID = clnCompMonitorModStatus.DoneByID
        mCompMonitorModStatus.LicenseNo = clnCompMonitorModStatus.LicenseNo
        mCompMonitorModStatus.Place = clnCompMonitorModStatus.Place
        '*********************************************
        mCompMonitorModStatus.IsAttachmentAdded = clnCompMonitorModStatus.IsAttachmentAdded
        If Not mFileAttach Is Nothing Then
            mFileAttach.ReferenceID = mCompMonitorModStatus.ID
            Session("mFileAttach") = mFileAttach
        End If

        'Commented and Added By Vikrant on 15-Apr-2021 to solve issue: Licence No not getting saved after select log
        'For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In clnCompMonitorModStatus.MaintenanceDoneByEmployees
        '    mCompMonitorModStatus.MaintenanceDoneByEmployees.Add(mCompMonitorModStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
        'Next
        For j As Integer = mCompMonitorModStatus.MaintenanceDoneByEmployees.Count - 1 To 0 Step -1
            mCompMonitorModStatus.MaintenanceDoneByEmployees.RemoveAt(j)
        Next
        For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In clnCompMonitorModStatus.MaintenanceDoneByEmployees
            If Not mCompMonitorModStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.ID) Then
                mCompMonitorModStatus.MaintenanceDoneByEmployees.Add(mCompMonitorModStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
            Else
                If Not mCompMonitorModStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.EmployeeID) Then
                    mCompMonitorModStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).EmployeeID = mMaintenanceDoneByEmployee.EmployeeID
                    mCompMonitorModStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).LicenceNo = mMaintenanceDoneByEmployee.LicenceNo
                    'mCompMonitorModStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).RequiredManHours = mMaintenanceDoneByEmployee.RequiredManHours
                    mCompMonitorModStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).EmployeeName = mMaintenanceDoneByEmployee.EmployeeName
                End If
            End If

        Next
        'End
    End Sub
    Private Sub SetMachineMaintenanceObject()
        'Added by Saylee on 9th-Oct-2009

        Dim mMachineID As Guid = Guid.Empty
        Dim mAssemblyStatusID As Guid = Guid.Empty
        If Not mCompStatus.IsSpareComp Then
            mMachineID = mAssemblyStatus.MachineID
            mAssemblyStatusID = mAssemblyStatus.ID

        End If

        If Session("EnFrom") = 0 And Not (mMachineMaintenanceList.Contains(mCompMonitorModStatus.ID, MaintenanceType.ComponentModification, "")) Then
            mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(mMachineID, MaintenanceType.ComponentModification, txtDoneOnDate.Text, mCompMonitorModStatus.ID, Guid.Empty, 0, 0, mAssemblyStatusID)
        Else
            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mCompMonitorModStatus.ID, MaintenanceType.ComponentModification)
        End If

        With mMachineMaintenance
            ''.MachineID = mAssemblyStatus.MachineID
            ''.MaintenanceActivityTypeID =9
            .MaintenanceID = mCompMonitorModStatus.ID 'TransactionID
            ''.AssemblyStatusID = mAssemblyStatus.ID

            .Date = txtDoneOnDate.Text
            If mCompStatus.IsSpareComp = 0 Then 'Added by Saylee on 6-Nov-2020 for ALL27072020


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
                    'End
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
        End If

    End Sub

    'Added By Prashant On 27-Nov-2014
    Private Sub NewRecordAttachment()
        mFileAttach = FileAttach.NewAttachment(Guid.Empty, mCompMonitorModStatus.ID)
        Session("mFileAttach") = mFileAttach
    End Sub
    Private Sub ControlVisibilityForAttachment()
        If mFileAttach.Size > 0 Then 'change from  to current condition
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
        End If
    End Sub
    Private Sub GetAttachment()
        If mCompMonitorModStatus.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mCompMonitorModStatus.ID)
            Session("mFileAttach") = mFileAttach
        End If

        'If mFileAttach Is Nothing Then
        '    NewRecordAttachment()
        'End If
    End Sub
    Private Sub SaveAttachment() '
        If mFileAttach.Size > 0 Then
            Try
                mFileAttach.Save()
                'mEmployee.IsAttachmentAdded = True
            Catch ex As Exception
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "", MessageBox.Show(ex.InnerException.ToString, False), True)
            End Try
        Else
            If (Not mCompMonitorModStatus.IsNew) And IsAttachmentDeleted Then
                FileAttach.DeleteAttachment(mFileAttach.ID, mCompMonitorModStatus.ID)
            End If
            IsAttachmentDeleted = False
            Session("IsAttachmentDeleted") = IsAttachmentDeleted
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
    'MLNo
    Public Sub SetLicenceCount()
        If mCompMonitorModStatus.MaintenanceDoneByEmployees.Count > 1 Then
            lblLicenceCount.Text = "and " + (mCompMonitorModStatus.MaintenanceDoneByEmployees.Count - 1).ToString + " more"
        End If
        lblLicenceCount.DataBind()
        'lblAllLicenceNos.DataBind()
    End Sub
    Private Sub BindLicenceNo()
        If mCompMonitorModStatus.MaintenanceDoneByEmployees.Count > 0 Then
            txtLicenceNo.Text = mCompMonitorModStatus.MaintenanceDoneByEmployees(0).LicenceNo + " [" + mCompMonitorModStatus.MaintenanceDoneByEmployees(0).EmployeeName + "]"
        Else
            txtLicenceNo.Text = String.Empty
        End If
    End Sub
    'End
    Private Sub ControlVisibilityForDatePeriod()
        Dim txtDnOnDate As TextBox
        For j As Integer = 0 To Me.dgDoneOnValue.Rows.Count - 1
            txtDnOnDate = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtCurrentValue"), TextBox)
            With mCompMonitorModStatus.CompMonitorModStatusPeriods

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
#End Region

#Region " Data Bindings "

    Private Sub DataFieldBind()
        dgCurrentValue.DataSource = mCompMonitorModStatus.CompMonitorModStatusPeriods
        dgDoneOnValue.DataSource = mCompMonitorModStatus.CompMonitorModStatusPeriods
        'Added On 28,May,2007 By Girish
        txtDoneOnDate.Text = mCompMonitorModStatus.DoneOnFormatted.ToString
        txtExtensionDate.Text = mCompMonitorModStatus.ExtensionDateFormatted.ToString

        'Added by Saylee on 9th-Oct-2009
        mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList()
        Session("mMachineMaintenanceList") = mMachineMaintenanceList

        If Val(mCompMonitorModStatus.PartMonitorMod.RequiredManHours) > 0 Then
            lblEstdManHours.Text = "(Estd. Man Hours : " + mCompMonitorModStatus.PartMonitorMod.RequiredManHours + ")"
        End If
        BindLicenceNo() 'MLNo
        DataBind()
        'Added By Vikrant On 30-Nov-2020 For Spare Comp FLow
        If mIsSpareComponent <> 1 Then
            mHourType = mMachine.HourType
        End If
        'End
    End Sub
    Private Sub DataBindGrid()
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
        dgCurrentValue.DataSource = mCompMonitorModStatus.CompMonitorModStatusPeriods
        dgDoneOnValue.DataSource = mCompMonitorModStatus.CompMonitorModStatusPeriods
        dgCurrentValue.DataBind()
        dgDoneOnValue.DataBind()
        ControlVisibilityForDatePeriod()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "txtRemark" Then
            If Len(txtRemark.Text) > 500 Then
                custValidator.ErrorMessage = "Max. length of Remark should be 500 char."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
            'Added By Utkarsh On 12-Jun-2012 FOR ALL08062012
        ElseIf custValidator.ControlToValidate = "txtLicenceNo" Then
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
        If Not mCompMonitorModStatus.IsValid Then
            For i As Integer = 0 To mCompMonitorModStatus.GetBrokenRulesCollection.Count - 1
                str = str + mCompMonitorModStatus.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(dgDoneOnValue.Rows.Count - 1)
            txtCurrentValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtCurrentValue"), TextBox)
            If Not mCompMonitorModStatus.CompMonitorModStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mCompMonitorModStatus.CompMonitorModStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mCompMonitorModStatus.CompMonitorModStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
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
            If Not mCompMonitorModStatus.CompMonitorModStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mCompMonitorModStatus.CompMonitorModStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mCompMonitorModStatus.CompMonitorModStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
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
        'Added by Vikrant on 26-July-2011
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            txtDoneOnDate.Focus()
            Session("mLogList") = Nothing
            DataFieldBind()
            SetLog()
            'GetAttachment() 'Added By Vikrant On 25-Nov-2014
            ControlVisibility()
            ControlVisibilityForDatePeriod()
            SetTitle()

            'MLNo
            SetLicenceCount()
            UserNameForLicenceList = User.Identity.Name
            Session("UserNameForLicenceList") = UserNameForLicenceList
            'End
        End If
    End Sub
    Private Sub dgDoneOnValue_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Select Case e.CommandName
            Case "CurrentValue"
                If Not IsValid Then Exit Sub
                Dim txtCurrentValue As TextBox
                For i As Integer = 0 To dgDoneOnValue.Rows.Count - 1
                    txtCurrentValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtCurrentValue"), TextBox)
                    With mCompMonitorModStatus.CompMonitorModStatusPeriods()
                        If .Item(i).PeriodID = 2 Then
                            If Period.IsDate(txtCurrentValue.Text) Then
                                .Item(i).CurrentValueFormatted = Trim(txtCurrentValue.Text)
                            Else
                                .Item(i).CurrentValueFormatted = ""
                            End If
                        Else
                            .Item(i).CurrentValue = Trim(txtCurrentValue.Text)
                        End If
                    End With
                Next


                'Added By Saylee on 28-07-2008
            Case "ExtensionValue"
                Dim txtExtensionValue As TextBox
                For i As Integer = 0 To mCompMonitorModStatus.CompMonitorModStatusPeriods.Count - 1
                    txtExtensionValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtExtensionValue"), TextBox)

                    With mCompMonitorModStatus.CompMonitorModStatusPeriods
                        .Item(i).ExtensionValue = Trim(txtExtensionValue.Text)
                    End With
                Next
                DataBindGrid()
        End Select
    End Sub
    Protected Sub txtCurrentValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim txtCurrentValue As TextBox
        For i As Integer = 0 To dgDoneOnValue.Rows.Count - 1
            txtCurrentValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtCurrentValue"), TextBox)
            With mCompMonitorModStatus.CompMonitorModStatusPeriods
                If .Item(i).PeriodID = 2 Then
                    If Period.IsDate(txtCurrentValue.Text) Then
                        .Item(i).CurrentValueFormatted = Trim(txtCurrentValue.Text)
                    Else
                        .Item(i).CurrentValueFormatted = ""
                    End If
                Else
                    .Item(i).CurrentValue = Trim(txtCurrentValue.Text)
                End If
            End With
        Next
        ControlVisibilityForGridBeforeBinding()
        DataBindGrid()
        ControlVisibility()
        'upnlDoneOnValueGrid.Update()
        upnlCurrentValueGrid.Update()
        upnlDoneOnValueGrid.Update()
    End Sub
    Protected Sub txtExtensionValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim txtExtensionValue As TextBox
        For i As Integer = 0 To mCompMonitorModStatus.CompMonitorModStatusPeriods.Count - 1
            txtExtensionValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtExtensionValue"), TextBox)

            With mCompMonitorModStatus.CompMonitorModStatusPeriods
                .Item(i).ExtensionValue = Trim(txtExtensionValue.Text)
            End With
        Next
        ControlVisibilityForGridBeforeBinding()
        DataBindGrid()
        ControlVisibility()
        'upnlDoneOnValueGrid.Update()
        upnlCurrentValueGrid.Update()
        upnlDoneOnValueGrid.Update()
    End Sub
    Private Sub txtDoneOnDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDoneOnDate.TextChanged
        If IsPostBack Then        'Added Code on May,29,2007
            SetObject()

            '******************************************************************
            'Added by Saylee on 11-Jan-2017
            'ConsiderCompInstValue=True only if Compliance date is less than Comp Inst Date then consider Current vlaue 
            'If False,then Comp Current Values will be calculated

            Dim ConsiderCompInstValue As Boolean = False
            If txtDoneOnDate.Text <> "" And mCompStatus.InstalledOn.ToString <> "" Then
                If CDate(mCompMonitorModStatus.DoneOn) < CDate(mCompStatus.InstalledOn) Then
                    ConsiderCompInstValue = True
                End If
            End If
            '******************************************************************

            Dim clnCompMonitorModStatus As CompMonitorModStatus = mCompMonitorModStatus.Clone
            If mEnFrom = From.NewRecord Then
                mCompMonitorModStatus = CompMonitorModStatus.NewComplyCompMonitorModStatus(Guid.NewGuid, mPrevCompMonitorModStatus.CompID, mPrevCompMonitorModStatus.AssemblyStatusID, txtDoneOnDate.Text, mCompStatus.Comp.PartID, mPrevCompMonitorModStatus.PartMonitorMod, Guid.Empty, mPrevCompMonitorModStatus.CompStatusID, mPrevCompMonitorModStatus.DoneOn.ToString, mHourType, , ConsiderCompInstValue)
            Else
                mCompMonitorModStatus = CompMonitorModStatus.GetComplyCompMonitorModStatus(mPrevCompMonitorModStatus.ID, mPrevCompMonitorModStatus.AssemblyStatusID, mPrevCompMonitorModStatus.CompStatusID, txtDoneOnDate.Text, Guid.Empty, mHourType, , ConsiderCompInstValue)
            End If
            CopyFromClone(clnCompMonitorModStatus)
            'DataBindGrid()
            Session.Remove("mLog") 'Added by Saylee on 9th-Oct-2009
            SetGridFromObject()
            DataBindGrid()
            upnlCurrentValueGrid.Update()
            upnlDoneOnValueGrid.Update()
            upnlTitle.Update()
        End If

    End Sub
    Private Sub btnSelectLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectLog.Click
        SetObject()
        SetGridObject()
        Session("mFromType") = 3
        Session("mMachineId") = mAssemblyStatus.MachineID.ToString
        Session("mAssemblyStatusId") = mCompMonitorModStatus.AssemblyStatusID.ToString
        Session("mAssemblyID") = mAssemblyStatus.AssemblyID.ToString
        Session("mDoneOn") = CStr(IIf(txtDoneOnDate.Text = "", Today.Date.ToShortDateString, txtDoneOnDate.Text))

        'Added by Saylee on 14-Mar-2016 for ALL11032016
        If mAssemblyStatus.InstalledOn.ToString <> "" Then
            If CDate(mCompMonitorModStatus.DoneOn) <= CDate(mAssemblyStatus.InstalledOn) Then 'if Compliance date is same or less than Assembly Inst. Date
                Dim mFirstLogDetailAfterAssemblyInstallation As FirstLogDetailAfterAssemblyInstallation = FirstLogDetailAfterAssemblyInstallation.GetFirstLogDetailAfterAssemblyInstallation(mAssemblyStatus)
                Session("mFirstLogDetailAfterAssemblyInstallation") = mFirstLogDetailAfterAssemblyInstallation
            End If
            '*************************************************
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSelectLogWindow", "OpenSelectLogWindow()", True)
        'Response.Redirect("wfSelectLog_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&BackPage6=wfComplyAssemblyMonitorModStatus_Ajax.aspx" & "&FromType=3&DoneOn=" & CStr(IIf(txtDoneOnDate.Text = "", Today.Date.ToShortDateString, txtDoneOnDate.Text)) & "&MachineId=" & mAssemblyStatus.MachineID.ToString & "&AssemblyStatusID=" & mCompMonitorModStatus.AssemblyStatusID.ToString & "&AssemblyID=" & mAssemblyStatus.AssemblyID.ToString)
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("ComponentModificationsNew") And mCompMonitorModStatus.IsNew) Or (Not User.IsInRole("ComponentModificationsEdit") And Not mCompMonitorModStatus.IsNew) Then
            'Changed by Vikrant on 28-July-2011
            mMonitorInfo = txtMonitorModType.Text
            mMonitorType = txtMonitorType.Text
            mPart = mCompStatus.PartName
            mSerialNo = mCompStatus.Comp.SerialNo
            'MaintDetail = "Reg No. : " + mMachine.RegNo + " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorModStatus.PartMonitorMod.PartMonitorModTypeName
            If mCompStatus.IsSpareComp = False Then
                MaintDetail = "Reg No. : " & mMachineMaintenance.RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorModStatus.PartMonitorMod.PartMonitorModTypeName & " Done On Date : " + mCompMonitorModStatus.DoneOnFormatted
            Else
                MaintDetail = "Stock Component : Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorModStatus.PartMonitorMod.PartMonitorModTypeName & " Done On Date : " + mCompMonitorModStatus.DoneOnFormatted
            End If
            MarkLog(Util.Action.Save, "ComponentModifications", User.Identity.Name & " is not Authorized User to save " & MaintDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        If Not IsValid Then
            upnlValidationSummary.Update()
            Exit Sub
        End If

        If IsValid Then

            'Code for OverDue 'Added by Saylee on 26-Mar-2019 for ALL26032019
            If Not mPrevCompMonitorModStatus.PartMonitorMod.MonitorTypeID = 3 Then  'No Frequency record not be checked for OverDue
                Dim DueString As String = ""
                DueString = CustomeValidateGridValuesForOverDue()
                If DueString <> "" Then
                    MSGBoxCtrl.Show("Alert!!!", "You are about to save Over Due Compliance, " + DueString, "Do you want to continue?", MsgBoxStyle.YesNo, "OverDue")
                    Session("DueString") = DueString
                    Exit Sub
                End If
            End If
            '*********************************************************************************
            'Added By Prashant 19-Nov-2019 Alert if user is complying on same date ALL19112019
            If mPrevCompMonitorModStatus.DoneOn.ToString <> "" Then
                If (CDate(txtDoneOnDate.Text) <= CDate(mPrevCompMonitorModStatus.DoneOn) And Session("EnFrom") <> 1) Then
                    MSGBoxCtrl.Show("Alert!!!", "Current compliance date is less than or equal to last compliance date ", "Do you want to continue?", MsgBoxStyle.YesNo, "ComplyOnSameDate")
                    Exit Sub
                End If
                'If CDate(txtDoneOnDate.Text) > CDate(mPrevCompMonitorModStatus.DoneOn) Then
                '    MSGBoxCtrl.show("Alert!!!", "Current compliance date is greater than last compliance date ", "Do you want to continue?", MsgBoxStyle.YesNo, "ComplyOnSameDate")
                '    Exit Sub
                'End If
            End If
            If (CDate(txtDoneOnDate.Text) > CDate(Today.Date) And Session("EnFrom") <> 1) Then
                MSGBoxCtrl.Show("Alert!!!", "Current compliance date is greater than today date  ", "Do you want to continue?", MsgBoxStyle.YesNo, "ComplyOnSameDate")
                Exit Sub
            End If
            'End of Added By Prashant 19-Nov-2019 Alert if user is complying on same date 

            If Save() Then
                'Added By Prashant On 27-Nov-2014
                Session.Remove("mFileAttach")
                Session.Remove("IsAttachmentDeleted")
                'End

                'MLNo
                Session.Remove("mMaintenanceDoneByEmployees")
                Session.Remove("UserNameForLicenceList")
                'End

                'Added by Saylee on 5-Apr-2019
                Session.Remove("mDoneOn")
                Session.Remove("LogID")
                Session.Remove("FromLog")
                '***************************************

                'Added by Saylee on 9th-Jan-2008 ===============================
                If Request.QueryString("GChildPage4") <> "" Then
                    Response.Redirect(Request.QueryString("GChildPage4") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3")) 'Added Code
                ElseIf Request.QueryString("GChildPage2") <> "" Then
                    Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
                End If
                '===============================================================
            End If
        Else
            upnlValidationSummary.Update()
        End If
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Changed by Vikrant on 28-July-2011
        MarkLog(Util.Action.Close, "ComponentModifications", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)

        RemoveSession()
        Session.Remove("FromLog")
        Session.Remove("IsBackFromCompliance") 'Added By Vikrant On 03-Jun-2016 For ALL03062016
        'Added by Saylee on 9th-Jan-2008 ===============================
        If Request.QueryString("GChildPage4") <> "" Then
            Response.Redirect(Request.QueryString("GChildPage4") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3")) 'Added Code
        ElseIf Request.QueryString("GChildPage2") <> "" Then
            Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
        End If
        '===============================================================
    End Sub
    'Added by Vikrant On 25-Nov-2014
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        mCompMonitorModStatus.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte

        If mCompMonitorModStatus.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mCompMonitorModStatus.ID)
            Session("mFileAttach") = mFileAttach
        End If

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        IsAttachmentDeleted = True
        mCompMonitorModStatus.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        ViewImage()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mCompMonitorModStatus.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mCompMonitorModStatus.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mCompMonitorModStatus.ID)
        End If
        Session("mFileAttach") = mFileAttach
    End Sub
    'End
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnSelectLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnSelectLog.Click
        If CType(Session("FromLog"), Boolean) = True Then
            'Dim LogId As Guid = New Guid(Request.QueryString("LogId"))
            'Dim LogDate = Request.QueryString("LogDate")
            Dim LogId As Guid = New Guid(CType(Session("LogID"), String))
            Dim LogDate = CType(Session("mDoneOn"), String)

            'If DateDiff(DateInterval.Day, SmartDate.StringToDate(mPrevCompMonitorModStatus.AsOnDate), SmartDate.StringToDate(LogDate)) > 0 Then
            '    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DoneOnDate, SIMsgBox.Message_text.DoneOnDate, "Compliance record only upto " & CStr(mPrevCompMonitorModStatus.AsOnDate) & " can be entered through Comp Installation screen", MsgBoxStyle.OKOnly)
            '    msg1.ReplacePage = "wfComplyCompMonitorModStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
            '    msg1.Show()
            '    Exit Sub
            'End If

            '******************************************************************
            'Added by Saylee on 11-Jan-2017
            'ConsiderCompInstValue=True only if Compliance date is less than Comp Inst Date then consider Current vlaue 
            'If False,then Comp Current Values will be calculated
            Dim ConsiderCompInstValue As Boolean = False
            If txtDoneOnDate.Text <> "" And mCompStatus.InstalledOn.ToString <> "" Then
                If CDate(mCompMonitorModStatus.DoneOn) < CDate(mCompStatus.InstalledOn) Then
                    ConsiderCompInstValue = True
                End If
            End If
            '******************************************************************

            Dim clnCompMonitorModStatus As CompMonitorModStatus = mCompMonitorModStatus.Clone
            If mEnFrom = From.NewRecord Then
                mCompMonitorModStatus = CompMonitorModStatus.NewComplyCompMonitorModStatus(Guid.NewGuid, mPrevCompMonitorModStatus.CompID, mPrevCompMonitorModStatus.AssemblyStatusID, LogDate, mCompStatus.Comp.PartID, mPrevCompMonitorModStatus.PartMonitorMod, LogId, mPrevCompMonitorModStatus.CompStatusID, mPrevCompMonitorModStatus.DoneOn.ToString, mHourType, CType(Session("ConsiderAssemblyInstValue"), Boolean), ConsiderCompInstValue)
            Else
                mCompMonitorModStatus = CompMonitorModStatus.GetComplyCompMonitorModStatus(mPrevCompMonitorModStatus.ID, mPrevCompMonitorModStatus.AssemblyStatusID, mPrevCompMonitorModStatus.CompStatusID, LogDate, LogId, mHourType, CType(Session("ConsiderAssemblyInstValue"), Boolean), ConsiderCompInstValue)
            End If
            mCompMonitorModStatus.DoneWONo = clnCompMonitorModStatus.DoneWONo
            mCompMonitorModStatus.DoneRemark = clnCompMonitorModStatus.DoneRemark
            mCompMonitorModStatus.DoneOn = clnCompMonitorModStatus.DoneOn
            mCompMonitorModStatus.RequiredManHours = clnCompMonitorModStatus.RequiredManHours
            'mCompMonitorModStatus.CompMonitorModStatusPeriods = clnCompMonitorModStatus.CompMonitorModStatusPeriods
            mCompMonitorModStatus.IsAttachmentAdded = clnCompMonitorModStatus.IsAttachmentAdded
            'Added By Vikrant on 15-Apr-2021 to solve issue: Licence No not getting saved after select log
            For j As Integer = mCompMonitorModStatus.MaintenanceDoneByEmployees.Count - 1 To 0 Step -1
                mCompMonitorModStatus.MaintenanceDoneByEmployees.RemoveAt(j)
            Next
            For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In clnCompMonitorModStatus.MaintenanceDoneByEmployees
                If Not mCompMonitorModStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.ID) Then
                    mCompMonitorModStatus.MaintenanceDoneByEmployees.Add(mCompMonitorModStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
                Else
                    If Not mCompMonitorModStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.EmployeeID) Then
                        mCompMonitorModStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).EmployeeID = mMaintenanceDoneByEmployee.EmployeeID
                        mCompMonitorModStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).LicenceNo = mMaintenanceDoneByEmployee.LicenceNo
                        mCompMonitorModStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).RequiredManHours = mMaintenanceDoneByEmployee.RequiredManHours
                        mCompMonitorModStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).EmployeeName = mMaintenanceDoneByEmployee.EmployeeName
                    End If
                End If
            Next
            'End
            If Not mFileAttach Is Nothing Then
                mFileAttach.ReferenceID = mCompMonitorModStatus.ID
                Session("mFileAttach") = mFileAttach
            End If
            Session("mCompMonitorModStatus") = mCompMonitorModStatus
            clnCompMonitorModStatus = Nothing
            'DataBindGrid()
            SetGridFromObject()
            DataBindGrid()
            'Added by Saylee on 9th-Oct-2009
            Dim mLog As Log
            mLog = Log.GetLog(New Guid(LogId.ToString))
            Session("mLog") = mLog
            '===================================
        Else
            Session.Remove("mLog")
        End If
        ControlVisibility()
        SetTitle()

        upnlCurrentValueGrid.Update()
        upnlDoneOnValueGrid.Update()
    End Sub
    'MLNo
    Private Sub imgbtnEmployeeLicence_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgbtnEmployeeLicence.Click
        If IsValid Then
            SetObject()
            Session("mMaintenanceID") = mCompMonitorModStatus.ID
            Session("MaintenanceDoneOnDate") = mCompMonitorModStatus.DoneOn.ToString
            mMaintenanceDoneByEmployees = mCompMonitorModStatus.MaintenanceDoneByEmployees
            Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AddEmployeeLicNo", "AddEmployeeLicNo();", True)
        Else
            upnlValidationSummary.Update()
        End If

    End Sub
    Private Sub hdnBtnMaintDoneBy_Click(sender As Object, e As System.EventArgs) Handles hdnBtnMaintDoneBy.Click
        For i As Integer = 0 To mMaintenanceDoneByEmployees.Count - 1
            Dim ID As Guid = mMaintenanceDoneByEmployees(i).ID
            If Not mCompMonitorModStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mCompMonitorModStatus.MaintenanceDoneByEmployees.Add(mMaintenanceDoneByEmployees(i))
            ElseIf mCompMonitorModStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mCompMonitorModStatus.MaintenanceDoneByEmployees(ID).LicenceNo = mMaintenanceDoneByEmployees(i).LicenceNo
                mCompMonitorModStatus.MaintenanceDoneByEmployees(ID).RequiredManHours = mMaintenanceDoneByEmployees(i).RequiredManHours
                mCompMonitorModStatus.MaintenanceDoneByEmployees(ID).EmployeeID = mMaintenanceDoneByEmployees(i).EmployeeID
                mCompMonitorModStatus.MaintenanceDoneByEmployees(ID).EmployeeName = mMaintenanceDoneByEmployees(i).EmployeeName
            End If
        Next

        For j As Integer = 0 To mCompMonitorModStatus.MaintenanceDoneByEmployees.Count - 1
            If Not mMaintenanceDoneByEmployees.Contains(mCompMonitorModStatus.MaintenanceDoneByEmployees(j).ID) Then
                mCompMonitorModStatus.MaintenanceDoneByEmployees.Remove(mCompMonitorModStatus.MaintenanceDoneByEmployees(j).ID, "")
            End If
        Next
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
        BindLicenceNo()
        SetLicenceCount() 'MLNo
        txtActualManHours.DataBind()
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
        Session("LicenseNo") = LicenseNo
        Session("EmployeeID") = DoneByID
        If Not DoneByID.Equals(Guid.Empty) Then
            If mCompMonitorModStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mCompMonitorModStatus.MaintenanceDoneByEmployees(0).EmployeeID = DoneByID
                mCompMonitorModStatus.MaintenanceDoneByEmployees(0).LicenceNo = LicenseNo
                If Not mCompMonitorModStatus.MaintenanceDoneByEmployees.Count > 1 Then 'If Condition added by Vikrant On 15-Apr-2021 to solve issue:Hours getting added for multiple licence no and if first licence no changed
                    mCompMonitorModStatus.MaintenanceDoneByEmployees(0).RequiredManHours = txtActualManHours.Text
                End If
                mCompMonitorModStatus.MaintenanceDoneByEmployees(0).EmployeeName = EmpName
            Else
                mCompMonitorModStatus.MaintenanceDoneByEmployees.Add(mCompMonitorModStatus.ID, MaintenanceType.ComponentModification, DoneByID, LicenseNo, txtActualManHours.Text, EmpName)
            End If

        Else
            If mCompMonitorModStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mCompMonitorModStatus.MaintenanceDoneByEmployees.RemoveAt(0)
            End If
        End If
        Session("mCompMonitorModStatus") = mCompMonitorModStatus
        BindLicenceNo()
        SetLicenceCount()
        txtActualManHours.DataBind()
    End Sub
    Protected Sub txtActualManHours_TextChanged(sender As Object, e As System.EventArgs)
        If mCompMonitorModStatus.MaintenanceDoneByEmployees.Count > 0 Then
            mCompMonitorModStatus.MaintenanceDoneByEmployees(0).RequiredManHours = txtActualManHours.Text
            upnlMonitoringStatusDetails.Update()
        End If
    End Sub
    'End
#End Region

#Region "Report Variable"
    Dim mCompanyDetail As New CompanyDetail
    'Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
#End Region

#Region " Events "
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If (Not User.IsInRole("AssemblyModificationsPrint")) Then
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
        Rpt = New crDetComplyCompMonitorStatus
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Current Value Grid
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 6
        RHCount = Me.mCompMonitorModStatus.CompMonitorModStatusPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            ReportDetails.Add(New rptStatus(, 0, "Monitoring Status Details", "Mod Type",
                  txtMonitorModType.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                  dgCurrentValue.Columns.Item(1).HeaderText, dgCurrentValue.Columns.Item(2).HeaderText,
                    , dgCurrentValue.Columns.Item(3).HeaderText, , dgCurrentValue.Columns.Item(4).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 0, "Monitoring Status Details", "Mod Type",
                            txtMonitorModType.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                                  "", "", , "", , ""))
        End If
        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Status Details", "Monitor Type",
                             txtMonitorType.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).PeriodUnitName, String),
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).FrequencyValueFormatted, String), ,
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).ElapsedValueFormatted, String), ,
                 CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Status Details", "ATA Chapter",
                            txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                            "", "", , "", , ""))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Status Details", "ATA Chapter",
                             txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).PeriodUnitName, String),
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).FrequencyValueFormatted, String), ,
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).ElapsedValueFormatted, String), ,
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Status Details", "ATA Chapter",
                            txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                            "", "", , "", , ""))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Status Details", "Reference",
                             txtReference.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).PeriodUnitName, String),
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).FrequencyValueFormatted, String), ,
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).ElapsedValueFormatted, String), ,
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Status Details", "Reference",
                                txtReference.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                                "", "", , "", , ""))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Directive Number",
                                   txtModNumber.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).PeriodUnitName, String),
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).FrequencyValueFormatted, String), ,
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).ElapsedValueFormatted, String), ,
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).RemainingValueFormatted, String), , , ))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Directive Number",
                                    txtModNumber.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                            "", "", , "", , "", , , ))
                End If
            ElseIf I = 4 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Status Details", "Description",
                                   txtDescription.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).PeriodUnitName, String),
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).FrequencyValueFormatted, String), ,
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).ElapsedValueFormatted, String), ,
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Status Details", "Description",
                                    txtDescription.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                            "", "", , "", , ""))
                End If
            ElseIf I = 5 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Status Details", "",
                                    "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).PeriodUnitName, String),
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).FrequencyValueFormatted, String), ,
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).ElapsedValueFormatted, String), ,
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).RemainingValueFormatted, String), , lblNote.Text))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Status Details", "",
                                        "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                            "", "", , "", , "", , lblNote.Text))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, "Monitoring Status Details", "",
                                         "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).PeriodUnitName, String),
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).FrequencyValueFormatted, String), ,
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).ElapsedValueFormatted, String), ,
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(I).RemainingValueFormatted, String), , lblNote.Text))
            End If
        Next

        'For Done On Value Grid
        Dim TotalCount1 As Integer
        Dim LHCount1 As Integer
        Dim RHCount1 As Integer
        LHCount1 = 7
        RHCount1 = Me.mCompMonitorModStatus.CompMonitorModStatusPeriods.Count
        If LHCount1 > RHCount1 Then
            TotalCount1 = LHCount1
        Else
            TotalCount1 = RHCount1
        End If

        Dim temp1 As Integer
        temp1 = 0
        If temp1 < RHCount1 Then
            ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Done On",
            New SmartDate(txtDoneOnDate.Text).FormattedText, , , , , , , , , , , , , , , , , "Component Values",
            dgDoneOnValue.Columns.Item(0).HeaderText, dgDoneOnValue.Columns.Item(1).HeaderText,
         , dgDoneOnValue.Columns.Item(2).HeaderText, , dgDoneOnValue.Columns.Item(3).HeaderText, RHData3:=dgDoneOnValue.Columns.Item(5).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Done On",
                            New SmartDate(txtDoneOnDate.Text).FormattedText, , , , , , , , , , , , , , , , , "Component Values",
                                  "", "", , "", , "", ""))
        End If


        Dim m As Integer
        For m = 0 To TotalCount1 - 1
            If m = 0 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Work Order No.",
                    txtWorkOrderNo.Text, , , , , , , , , , , , , , , , , "Component Values",
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).CurrentValueFormatted, String), ,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DueOnValueFormatted, String), RHData3:=CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Work Order No.",
                            txtWorkOrderNo.Text, , , , , , , , , , , , , , , , , "Component Values",
                                "", "", , "", , "", ""))
                End If
            ElseIf m = 1 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Remark",
                     txtRemark.Text, , , , , , , , , , , , , , , , , "Component Values",
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).CurrentValueFormatted, String), ,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DueOnValueFormatted, String), RHData3:=CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Remark",
                            txtRemark.Text, , , , , , , , , , , , , , , , , "Component Values",
                                "", "", , "", , "", ""))
                End If
                'ElseIf m = 2 Then
                '    If m < RHCount1 Then
                '        ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Extension Date", _
                '         txtExtensionDate.Text, , , , , , , , , , , , , , , , , "Component Values", _
                '    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).PeriodUnitName, String), _
                '    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).CurrentValueFormatted, String), , _
                '    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).ExtensionValueFormatted, String), , _
                '    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DueOnValueFormatted, String), RHData3:=CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String)))
                '    Else
                '        ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Extension Date", _
                '                txtExtensionDate.Text, , , , , , , , , , , , , , , , , "Component Values", _
                '                    "", "", , "", , "", "", , ))
                '    End If
                'ElseIf m = 3 Then
                '    If m < RHCount1 Then
                '        ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Approval Remark", _
                '         txtApprovalRemark.Text, , , , , , , , , , , , , , , , , "Component Values", _
                '    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).PeriodUnitName, String), _
                '    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).CurrentValueFormatted, String), , _
                '    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).ExtensionValueFormatted, String), , _
                '    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DueOnValueFormatted, String), RHData3:=CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String)))
                '    Else
                '        ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Approval Remark", _
                '                txtApprovalRemark.Text, , , , , , , , , , , , , , , , , "Component Values", _
                '                    "", "", , "", , "", "", , ))
                '    End If
            ElseIf m = 2 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Actual Man Hours",
                     txtActualManHours.Text, , , , , , , , , , , , , , , , , "Component Values",
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).PeriodUnitName, String),
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).CurrentValueFormatted, String), ,
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).ExtensionValueFormatted, String), ,
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DueOnValueFormatted, String), RHData3:=CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Actual Man Hours",
                            txtActualManHours.Text, , , , , , , , , , , , , , , , , "Component Values",
                                "", "", , "", , "", "", , ))
                End If
            ElseIf m = 3 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Done By Agency",
                     txtDoneBy.Text, , , , , , , , , , , , , , , , , "Component Values",
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).PeriodUnitName, String),
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).CurrentValueFormatted, String), ,
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).ExtensionValueFormatted, String), ,
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DueOnValueFormatted, String), RHData3:=CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Done By Agency",
                            txtDoneBy.Text, , , , , , , , , , , , , , , , , "Component Values",
                                "", "", , "", , "", "", , ))
                End If
            ElseIf m = 4 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "License No.",
                    mCompMonitorModStatus.AllLicenceNosWithEmpName, , , , , , , , , , , , , , , , , "Component Values",
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).PeriodUnitName, String),
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).CurrentValueFormatted, String), ,
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).ExtensionValueFormatted, String), ,
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DueOnValueFormatted, String), RHData3:=CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "License No.",
                           mCompMonitorModStatus.AllLicenceNosWithEmpName, , , , , , , , , , , , , , , , , "Component Values",
                                "", "", , "", , "", "", , ))
                End If
            ElseIf m = 5 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Place",
                     txtPlace.Text, , , , , , , , , , , , , , , , , "Component Values",
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).PeriodUnitName, String),
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).CurrentValueFormatted, String), ,
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).ExtensionValueFormatted, String), ,
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DueOnValueFormatted, String), RHData3:=CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Place",
                            txtPlace.Text, , , , , , , , , , , , , , , , , "Component Values",
                                "", "", , "", , "", "", , ))
                End If
            ElseIf m = 6 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "",
                    "", , , , , , , , , , , , , , , , , "Component Values",
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).PeriodUnitName, String),
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).CurrentValueFormatted, String), ,
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).ExtensionValueFormatted, String), ,
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DueOnValueFormatted, String),
                            , lblNote1.Text))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "",
                    "", , , , , , , , , , , , , , , , , "Component Values",
                           "", "", , "", , "", "", lblNote1.Text, ))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "",
                                   "", , , , , , , , , , , , , , , , , "Component Values",
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).PeriodUnitName, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).CurrentValueFormatted, String), ,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).ExtensionValueFormatted, String), ,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(m).DueOnValueFormatted, String),
                    , lblNote1.Text))
            End If
        Next

        'For Document Details
        Dim TotalCount2 As Integer
        Dim LHCount2 As Integer
        Dim RHCount2 As Integer
        LHCount2 = 3
        RHCount2 = Me.mCompMonitorModStatus.CompMonitorModStatusPeriods.Count
        If LHCount2 > RHCount2 Then
            TotalCount2 = LHCount2
        Else
            TotalCount2 = RHCount2
        End If

        Dim temp2 As Integer
        temp2 = 0
        If temp2 < RHCount2 Then
            ReportDetails.Add(New rptStatus(, 2, "Document Details", "Revision No.",
            txtRevisionNo.Text, , , , , , , , , , , , , , , , , "Extension Details",
            dgDoneOnValue.Columns.Item(0).HeaderText, dgDoneOnValue.Columns.Item(1).HeaderText, "Extension Date ",
            dgDoneOnValue.Columns.Item(2).HeaderText, txtExtensionDate.Text, dgDoneOnValue.Columns.Item(3).HeaderText,
            dgDoneOnValue.Columns.Item(4).HeaderText, dgDoneOnValue.Columns.Item(5).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 2, "Document Details", "Revision No.",
                                txtRevisionNo.Text, , , , , , , , , , , , , , , , , "Extension Details",
                                      "", txtExtensionDate.Text, , "", , "", ""))
        End If
        Dim n As Integer
        For n = 0 To TotalCount2 - 1
            If n = 0 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Page No.",
                    txtPageNo.Text, , , , , , , , , , , , , , , , , "Extension Details",
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).PeriodUnitName, String),
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).CurrentValueFormatted, String), "Approval Remark",
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).ExtensionValueFormatted, String), txtApprovalRemark.Text,
                    CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).DueOnValueFormatted, String), , ))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Page No.",
                        txtPageNo.Text, , , , , , , , , , , , , , , , , "Extension Details",
                        "", txtApprovalRemark.Text, , "", , "", ""))
                End If
            ElseIf n = 1 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Book No.",
                    txtBookNo.Text, , , , , , , , , , , , , , , , , "Extension Details",
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).PeriodUnitName, String),
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).CurrentValueFormatted, String), ,
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).ExtensionValueFormatted, String), ,
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).DueOnValueFormatted, String), RHData3:=CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).AssemblyDueOnValueFormattedByAirFrame, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Book No.",
                        txtBookNo.Text, , , , , , , , , , , , , , , , , "Extension Details",
                    "", "", , "", , "", ""))
                End If
            ElseIf n = 2 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Source Doc ",
                    txtSourceDoc.Text, , , , , , , , , , , , , , , , , "Extension Details",
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).PeriodUnitName, String),
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).CurrentValueFormatted, String), ,
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).ExtensionValueFormatted, String), ,
                CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).DueOnValueFormatted, String), RHData3:=CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).AssemblyDueOnValueFormattedByAirFrame, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Source Doc ",
                        txtSourceDoc.Text, , , , , , , , , , , , , , , , , "Extension Details",
                    "", "", , "", , "", ""))
                End If

            Else
                ReportDetails.Add(New rptStatus(, 2, "Document Details", "",
                                 "", , , , , , , , , , , , , , , , , "Component Values at Compliance of Service",
                  CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).PeriodUnitName, String),
                  CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).CurrentValueFormatted, String), ,
                  CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).ExtensionValueFormatted, String), ,
                  CType(Me.mCompMonitorModStatus.CompMonitorModStatusPeriods(n).DueOnValueFormatted, String),
                  , lblNote1.Text))
            End If
        Next
        '***********************************************************************************************************************

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
        mCompanyDetail.WebSite, "Comply Component Modification Status Detail Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        'MarkLog(Util.Action.Print, "ComplyAssemblyMonitorModStatus", mAssemblyInfo + " -> " + "Comply Assembly Monitor Modification Status Detail Report", Util.ErrorType.NoError, mCompMonitorModStatus.ID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    Private Sub lnkPrintLogBookEntry_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPrintLogBookEntry.Click  'Added By Saylee On 18-May-2021 ALL07052021
        Dim RptCommonHistory As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mLogEntryFormat As New LogEntryFormat
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsReportHistoryList
        Dim mCompanyDetail As New CompanyDetail

        RptCommonHistory = New crptLogEntryFormat

        mLogEntryFormat = LogEntryFormat.GetHistoryList(mCompMonitorModStatus.DoneOn, mCompMonitorModStatus.DoneOn, "", mAssemblyStatus.AssemblyTypeName,
                                                        mAssemblyStatus.ModelName, mAssemblyStatus.Assembly.SerialNo, "", "", "", "",
                                                        mAssemblyStatus.MachineID.ToString, False, True, IsRemoved:=False, IsInstalled:=True,
                                                        IsComplied:=False, AssemblyID:=mAssemblyStatus.AssemblyID.ToString, IsLogNo:=True,
                                                        IsLogPageNo:=False, IsFlightNo:=False, IsMELRequired:=False, IsMaintenanceActivityRequired:=False,
                                                        AssemblyTypeID:=mAssemblyStatus.AssemblyTypeID, CompStatusID:=mCompStatus.ID.ToString,
                                                        ShowService:=False, ShowDir:=True, ShowInsp:=False, CompMonitorModStatusID:=mCompMonitorModStatus.ID.ToString)
        If mLogEntryFormat.Count = 0 Then
            Exit Sub
        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
           mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
           mCompanyDetail.WebSite, "LOG BOOK ENTRY", "", mCompMonitorModStatus.DoneOnFormatted, Machine.GetMachine(mAssemblyStatus.MachineID).RegNo,
           mAssemblyStatus.ModelName + "-" + mAssemblyStatus.Assembly.SerialNo, IIf(mAssemblyStatus.AssemblyTypeName.Equals("Airframe"), "AIRCRAFT", mAssemblyStatus.AssemblyTypeName.ToUpper),
           AppSettings("Product Version"), AppSettings("SINote"),
           "AVERAGE FUEL CONSUMPTION________LTR./HR & AVERAGE OIL CONSUMPTION________LTR./HR SINCE LAST SMI DONE.  BOTH THE FIGURES ARE BELOW THE ALERT VALUE.",
           "True", mCompMonitorModStatus.DoneOnFormatted, "", AppSettings("Logo"))

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