
'AJAX Conversion By Saylee On 17-Apr-2015
Imports System.Linq
Imports System.Collections.Generic
Imports System.Text 'Added By Vikrant On 17-Sep-2020 For Mismatch Value Mail Send

Public Class wfComplyCompMonitorInspStatus_AJAX
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
    Public mCompMonitorInspStatus As CompMonitorInspStatus
    Public mPrevCompMonitorInspStatus As CompMonitorInspStatus
    Public mCompInfo As String                   'Code Added 29,Jan,2007 
    Public ComplyCompMonitorInspInfo As String   'Code Added 29,Jan,2007

    Public mMachineMaintenance As MachineMaintenance 'Added by Saylee on 9th-Oct-2009
    Public mMachineMaintenanceList As MachineMaintenanceList 'Added by Saylee on 9th-Oct-2009

    Dim EventLogID As Guid 'Added By Utkarsh On 28-Jul-2011 For All19072011
    Dim MaintDetail As String 'Added By Utkarsh On 28-Jul-2011 For All19072011
    Dim mEmployeeStatus As EmployeeStatus 'Added By Vikrant On 06-Aug-2013 For ALL01082013
    'Added By Prashant On 27-Nov-2014
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    'End
    Public mInspectionDetail As String
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
    Public mIsSpareComponent As Integer 'Added By Prashant On 17-Sep-2020 For ALL27072020
    Dim mHourType As Integer = 1 'Added By Vikrant On 30-Nov-2020 For Spare Comp FLow
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mEnFrom = CType(Session("EnFrom"), From)
        mMachine = CType(Session("mMachine"), Machine)
        mCompMonitorInspStatus = CType(Session("mCompMonitorInspStatus"), CompMonitorInspStatus)
        mPrevCompMonitorInspStatus = CType(Session("mPrevCompMonitorInspStatus"), CompMonitorInspStatus)
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
        mIsSpareComponent = CType(Session("mIsSpareComponent"), Integer) 'Added By Prashant On 17-Sep-2020 For ALL27072020
    End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
        Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
        Session("mPrevCompMonitorInspStatus") = mPrevCompMonitorInspStatus
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
        mCompMonitorInspStatus = Nothing
        Session.Remove("EnFrom")
        Session.Remove("mCompMonitorInspStatus")

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
            mCompMonitorInspStatus.DoneOn = System.DBNull.Value
        Else
            mCompMonitorInspStatus.DoneOn = txtDoneOnDate.Text
        End If
        mCompMonitorInspStatus.DoneWONo = Trim(txtWorkOrderNo.Text)
        mCompMonitorInspStatus.DoneRemark = Trim(txtRemark.Text)
        mCompMonitorInspStatus.RequiredManHours = Trim(txtActualManHours.Text)

        'Added By Saylee on 28-07-2008=======================
        'CNDC
        If Not IsDate(txtExtensionDate.Text) Then
            mCompMonitorInspStatus.ExtensionDate = System.DBNull.Value
        Else
            mCompMonitorInspStatus.ExtensionDate = txtExtensionDate.Text
        End If

        mCompMonitorInspStatus.ApprovalRemark = Trim(txtApprovalRemark.Text)
        '====================================================
        With mCompMonitorInspStatus
            .IsApplicable = chkApplicable.Checked   'Added By Vaishali on 19-Nov-2008
        End With

        mCompMonitorInspStatus.DoneBy = txtDoneBy.Text 'Added by Saylee On 23-Apr-2009

        ' Added By Utkarsh On 12-Jun-2012 FOR ALL08062012

        Dim LicenseNo As String = String.Empty
        Dim EmpName As String = String.Empty
        If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
            LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
            EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
        Else
            LicenseNo = Trim(txtLicenceNo.Text)
        End If
        mCompMonitorInspStatus.LicenseNo = LicenseNo
        mCompMonitorInspStatus.DoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID

        'End

        'Added by Saylee On 26-Apr-2012
        mCompMonitorInspStatus.Place = txtPlace.Text.Trim
        '*********************************************
        'Added By Prashant On 27-Nov-2014
        If mFileAttach.Size > 0 Then
            mCompMonitorInspStatus.IsAttachmentAdded = True
        Else
            mCompMonitorInspStatus.IsAttachmentAdded = False
        End If
        'End

        mCompMonitorInspStatus.SourceDoc = Trim(txtSourceDoc.Text)
        mCompMonitorInspStatus.RevisionNo = Trim(txtRevisionNo.Text)
        mCompMonitorInspStatus.BookNo = Trim(txtBookNo.Text)
        mCompMonitorInspStatus.PageNo = Trim(txtPageNo.Text)
        Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
    End Sub
    Public Sub SetGridObject()
          Dim txtCurrentValue, txtExtensionValue As TextBox
        Dim j As Int32
        For j = 0 To Me.dgDoneOnValue.Rows.Count - 1
            txtCurrentValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtCurrentValue"), TextBox)
            'Added By Saylee on 28-07-2008
            txtExtensionValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtExtensionValue"), TextBox)
            With mCompMonitorInspStatus.CompMonitorInspStatusPeriods
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
        Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
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
            With mPrevCompMonitorInspStatus.CompMonitorInspStatusPeriods ''mPrevCompMonitorInspStatus object contains previous period values
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

        Session("mCompMonitorInspStatus") = mCompMonitorInspStatus

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
        For j = 0 To mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Count - 1
            With mCompMonitorInspStatus.CompMonitorInspStatusPeriods
                If .Item(j).PeriodID = 2 Then
                    If Not Period.IsDate(mCompMonitorInspStatus.CompMonitorInspStatusPeriods(j).CurrentValueFormatted) Then
                        .Item(j).CurrentValue = ""
                    Else
                        .Item(j).CurrentValueFormatted = Trim(mCompMonitorInspStatus.CompMonitorInspStatusPeriods(j).CurrentValueFormatted)
                    End If
                Else
                    .Item(j).CurrentValue = Trim(mCompMonitorInspStatus.CompMonitorInspStatusPeriods(j).CurrentValueFormatted)
                End If

                'Added By Saylee on 28-07-2008
                'ExtensionValue
                .Item(j).ExtensionValue = Trim(mCompMonitorInspStatus.CompMonitorInspStatusPeriods(j).ExtensionValueFormatted)
            End With
        Next j
        Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
    End Sub
    Private Sub SetLog()
         If Val(Request.QueryString("Type")) = -1 Then
            'Dim LogId As Guid = New Guid(Request.QueryString("LogId"))
            'Dim LogDate = Request.QueryString("LogDate")

            Dim LogId As Guid = New Guid(CType(Session("LogID"), String))
            Dim LogDate = CType(Session("LogDate"), String)

            'If DateDiff(DateInterval.Day, SmartDate.StringToDate(mPrevCompMonitorInspStatus.AsOnDate), SmartDate.StringToDate(LogDate)) > 0 Then
            '    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DoneOnDate, SIMsgBox.Message_text.DoneOnDate, "Compliance record only upto " & CStr(mPrevCompMonitorInspStatus.AsOnDate) & " can be entered through Comp Installation screen", MsgBoxStyle.OKOnly)
            '    msg1.ReplacePage = "wfComplyCompMonitorInspStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
            '    msg1.Show()
            '    Exit Sub
            'End If

            '******************************************************************
            'Added by Saylee on 11-Jan-2017
            'ConsiderCompInstValue=True only if Compliance date is less than Comp Inst Date then consider Current vlaue 
            'If False,then Comp Current Values will be calculated

            Dim ConsiderCompInstValue As Boolean = False
            If txtDoneOnDate.Text <> "" And mCompStatus.InstalledOn.ToString <> "" Then
                If CDate(mCompMonitorInspStatus.DoneOn) < CDate(mCompStatus.InstalledOn) Then
                    ConsiderCompInstValue = True
                End If
            End If
            '******************************************************************

            Dim clnCompMonitorInspStatus As CompMonitorInspStatus = mCompMonitorInspStatus.Clone
            If mEnFrom = From.NewRecord Then
                mCompMonitorInspStatus = CompMonitorInspStatus.NewComplyCompMonitorInspStatus(Guid.NewGuid, mPrevCompMonitorInspStatus.CompID, mPrevCompMonitorInspStatus.AssemblyStatusID, LogDate, mCompStatus.Comp.PartID, mPrevCompMonitorInspStatus.PartMonitorInsp, LogId, mPrevCompMonitorInspStatus.CompStatusID, mPrevCompMonitorInspStatus.DoneOn.ToString, mHourType, CType(Session("ConsiderAssemblyInstValue"), Boolean), ConsiderCompInstValue, IsForSpareComp:=mIsSpareComponent)
            Else
                mCompMonitorInspStatus = CompMonitorInspStatus.GetComplyCompMonitorInspStatus(mPrevCompMonitorInspStatus.ID, mPrevCompMonitorInspStatus.AssemblyStatusID, mPrevCompMonitorInspStatus.CompStatusID, LogDate, LogId, mHourType, CType(Session("ConsiderAssemblyInstValue"), Boolean), ConsiderCompInstValue, IsForSpareComp:=mIsSpareComponent)
            End If
            mCompMonitorInspStatus.DoneWONo = clnCompMonitorInspStatus.DoneWONo
            mCompMonitorInspStatus.DoneRemark = clnCompMonitorInspStatus.DoneRemark
            mCompMonitorInspStatus.DoneOn = clnCompMonitorInspStatus.DoneOn
            mCompMonitorInspStatus.RequiredManHours = clnCompMonitorInspStatus.RequiredManHours
            'mCompMonitorInspStatus.CompMonitorInspStatusPeriods = clnCompMonitorInspStatus.CompMonitorInspStatusPeriods
            mCompMonitorInspStatus.IsAttachmentAdded = clnCompMonitorInspStatus.IsAttachmentAdded
            If Not mFileAttach Is Nothing Then
                mFileAttach.ReferenceID = mCompMonitorInspStatus.ID
                Session("mFileAttach") = mFileAttach
            End If
            'Added By Vikrant on 15-Apr-2021 to solve issue: Licence No not getting saved after select log
            For j As Integer = mCompMonitorInspStatus.MaintenanceDoneByEmployees.Count - 1 To 0 Step -1
                mCompMonitorInspStatus.MaintenanceDoneByEmployees.RemoveAt(j)
            Next
            For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In clnCompMonitorInspStatus.MaintenanceDoneByEmployees
                If Not mCompMonitorInspStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.ID) Then
                    mCompMonitorInspStatus.MaintenanceDoneByEmployees.Add(mCompMonitorInspStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
                Else
                    If Not mCompMonitorInspStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.EmployeeID) Then
                        mCompMonitorInspStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).EmployeeID = mMaintenanceDoneByEmployee.EmployeeID
                        mCompMonitorInspStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).LicenceNo = mMaintenanceDoneByEmployee.LicenceNo
                        mCompMonitorInspStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).RequiredManHours = mMaintenanceDoneByEmployee.RequiredManHours
                        mCompMonitorInspStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).EmployeeName = mMaintenanceDoneByEmployee.EmployeeName
                    End If
                End If
            Next
            'End
            Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
            clnCompMonitorInspStatus = Nothing

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
        ', , , , , , , , , , True, True, , mAssemblyStatus.AssemblyID.ToString, , , , , , , mPrevCompMonitorInspStatus.CompID.ToString, , , , , , , _
        ', , ).Item(0), MachineInfo).AssemblyStatusList

        'If mAssemblyStatusList.Count = 0 Then
        '    mAssemblyStatusList = CType(MachineList.GetMachineListWithRemoval(LogDate, mMachine.ID.ToString _
        '           , , , , , , , , , , True, True, , mAssemblyStatus.AssemblyID.ToString, , , , , , , mPrevCompMonitorInspStatus.CompID.ToString, , , , , , , _
        '           , ).Item(0), MachineInfo).AssemblyStatusList
        'End If
        ''-----------------------------

        Dim mAssemblyStatusList As AssemblyStatusList
        Dim mMachineList As MachineList
        Dim LatestRemovedOn As SmartDate
        Dim AssemblyStatusID As Guid = Guid.Empty
        Dim CompStatusID As Guid = Guid.Empty

        mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(LogDate, mMachine.ID.ToString _
        , , , , , , , , , , True, True, , mAssemblyStatus.AssemblyID.ToString, , , , , , , mPrevCompMonitorInspStatus.CompID.ToString, , , , , , , _
        , , SkipIsForInventoryAircarft:=True, MonitoringInspRequired:=False, MonitoringModRequired:=False, _
            MonitoringServiceRequired:=False, CompMonitoringInspRequired:=False, CompMonitoringModRequired:=False, _
            CompMonitoringServiceRequired:=False).Item(0), MachineInfo).AssemblyStatusList

        If mAssemblyStatusList.Count = 0 Then
            mMachineList = MachineList.GetMachineListWithRemoval(LogDate, mMachine.ID.ToString _
                   , , , , , , , , , , True, True, , mAssemblyStatus.AssemblyID.ToString, , , , , , , mPrevCompMonitorInspStatus.CompID.ToString, , , , , , , _
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

        'Here instead of mPrevCompMonitorInspStatus.AssemblyStatusID pass mAssemblyStatusList(0).ID  
        'Here instead of mPrevCompMonitorInspStatus.CompStatusID pass mAssemblyStatusList(0).CompStatusList(0).ID

        'mCompMonitorInspStatus = CompMonitorInspStatus.NewComplyCompMonitorInspStatus(Guid.NewGuid, mPrevCompMonitorInspStatus.CompID, mPrevCompMonitorInspStatus.AssemblyStatusID, LogDate, mCompStatus.Comp.PartID, mPrevCompMonitorInspStatus.PartMonitorInsp, LogID, mPrevCompMonitorInspStatus.CompStatusID, mPrevCompMonitorInspStatus.DoneOn.ToString, mHourType)
        mCompMonitorInspStatus = CompMonitorInspStatus.NewComplyCompMonitorInspStatus(Guid.NewGuid, mPrevCompMonitorInspStatus.CompID, AssemblyStatusID, LogDate, mCompStatus.Comp.PartID, mPrevCompMonitorInspStatus.PartMonitorInsp, LogID, CompStatusID, mPrevCompMonitorInspStatus.DoneOn.ToString, mHourType, IsForSpareComp:=mIsSpareComponent)
        Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
        SetTitle()
    End Sub
    Private Sub EditRecord(ByVal LogID As Guid, ByVal DoneOnDate As String, ByVal FromEntry As Boolean)
        REM:-FromEntry is used for avoiding object Dirty at form load when we r coming thru' Edit.
        If FromEntry = False Then
            mCompMonitorInspStatus = CompMonitorInspStatus.GetComplyCompMonitorInspStatus(mPrevCompMonitorInspStatus.ID, mPrevCompMonitorInspStatus.AssemblyStatusID, mPrevCompMonitorInspStatus.CompStatusID, DoneOnDate, LogID, mHourType, IsForSpareComp:=mIsSpareComponent)
        Else
            mCompMonitorInspStatus = CompMonitorInspStatus.GetComplyCompMonitorInspStatusFromEntry(mPrevCompMonitorInspStatus.ID, mPrevCompMonitorInspStatus.AssemblyStatusID, mPrevCompMonitorInspStatus.CompStatusID, DoneOnDate, mHourType, IsForSpareComp:=mIsSpareComponent)
        End If
        mCompMonitorInspStatus.BeginEdit()
        Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
        SetTitle()
    End Sub
	'Added By Vikrant On 17-Sep-2020 For Mismatch Value Mail Send
	Private Sub SendMail(ByVal InspStatus As CompMonitorInspStatus, ByVal DoneOnValue As String, ByVal DoneOnValueObj As String, Optional ByVal OnlyEdited As Boolean = False, Optional ByVal ToMailIDs As String = "saylee@bytzsoft.com")
		Dim str As New StringBuilder
		Try
			If OnlyEdited = False Then
				str.Append("Mismatch Details for <b>" & IIf(Session("From") = 1, "Edited and Saved", IIf(InspStatus.IsNew, "New", "New but Saved")) & "</b> record are as follows: ")
			Else
				''  str.Append("Mismatch Details for <b>" & IIf(Session("From") = 1, "Only Edited", IIf(InspStatus.IsNew, "New", "New but Saved")) & "</b> record are as follows: ")
			End If

			str.Append("<p><b>Assembly Details: </b> " & mAssemblyStatus.Assembly.ModelName & " " & mAssemblyStatus.Assembly.SerialNo & "</p>")
			str.Append("<p><b>Component Details: </b> " & mCompStatus.Comp.PartName & " " & mCompStatus.Comp.SerialNo & "</p>")
			str.Append("<p><b>Inspection ID: </b> " & InspStatus.ID.ToString & "</p>")
			str.Append("<p><b>Inspection Description: </b> " & InspStatus.PartMonitorInsp.Description & "</p>")

			str.Append("<p><b>Done On Date: </b> " & txtDoneOnDate.Text & "</p>")
			str.Append("<p><b>Done On Value: </b> " & DoneOnValue & "</p>")
			str.Append("<p><b>Done On Date(obj.): </b> " & InspStatus.DoneOnFormatted.ToString & "</p>")
			str.Append("<p><b>Done On Value(obj.): </b> " & DoneOnValueObj & "</p>")

			str.Append("<p><b>Saved By: </b> " & User.Identity.Name)

			SendMailFile.SendMailFile(Nothing, User.Identity.Name, "FAS: Component Inspection Done on Date Done on Value Mismatch Details", "", Info:=str.ToString, VendorEmailID:="", ToMailID:=ToMailIDs)
		Catch ex As Exception
			Dim Title As String = "Error Sending Mail"
			Dim Message As String = ex.InnerException.ToString
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(Title, Message, , False), True)
			Exit Sub
		End Try
	End Sub
	'End
	Private Function Save() As Boolean
        Dim clnCompMonitorInspStatus As CompMonitorInspStatus
        clnCompMonitorInspStatus = CType(mCompMonitorInspStatus.Clone, CompMonitorInspStatus)
        SetObject()
        SetGridObject()
        SetMachineMaintenanceObject() 'Added by Saylee on 9th-Oct-2009
        If mCompMonitorInspStatus.IsValid Then
            If mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodRequired, MSGBox.Message_text.PeriodRequired, "You are trying to save Component Inspection Status.Component Inspection Status can not be saved without period units.", MsgBoxStyle.OkOnly, "")
                Return False
            End If
            Try
                'Added By Vikrant On 06-Aug-2013 For ALL01082013
                If Not mCompMonitorInspStatus.DoneByID.Equals(Guid.Empty) AndAlso Not mCompMonitorInspStatus.DoneOn.Equals(System.DBNull.Value) Then
                    Dim title As String = "Save Alert !"
                    Dim message As String = ""
                    mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mCompMonitorInspStatus.DoneByID.ToString, mCompMonitorInspStatus.DoneOn)
                    If (mEmployeeStatus(0).Information <> "") Then
                        message = mEmployeeStatus(0).Information
                        '  ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenAlertMessage", MessageBox.Show(title, message, IsTagRequired:=False), True)
                        MSGBoxCtrl.show(title, message, "", MsgBoxStyle.OkOnly, "")
                        Return False
                    End If
                End If
                'End
                'Added By Vikrant On 17-Sep-2020 For Mismatch Value Mail Send
                If txtDoneOnDate.Text <> "" AndAlso mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Contains(2, "") Then 'If date period conatins then only execute
                    Dim DoneOnValue As New StringBuilder
                    Dim DoneOnValueObj As New StringBuilder
                    For j As Integer = 0 To Me.dgDoneOnValue.Rows.Count - 1
                        DoneOnValue.Append(CType(Me.dgDoneOnValue.Rows(j).FindControl("txtCurrentValue"), TextBox).Text + ", ")
                        DoneOnValueObj.Append(mCompMonitorInspStatus.CompMonitorInspStatusPeriods(j).CurrentValueFormatted + ", ")
                        If mCompMonitorInspStatus.CompMonitorInspStatusPeriods(j).PeriodID = 2 Then
                            If Not txtDoneOnDate.Text.Equals(CType(Me.dgDoneOnValue.Rows(j).FindControl("txtCurrentValue"), TextBox).Text) Then
                                Session("IsSendMail") = "True"
                            End If
                        End If

                    Next j
                    If Session("IsSendMail") = "True" Then
                        Session.Remove("IsSendMail")
                        SendMail(mCompMonitorInspStatus, DoneOnValue.ToString.Trim.TrimEnd(","), DoneOnValueObj.ToString.Trim.TrimEnd(","), ToMailIDs:="")
                    End If
                End If
                'End 
                mCompMonitorInspStatus.ApplyEdit()
                mCompMonitorInspStatus = CType(mCompMonitorInspStatus.Save(), CompMonitorInspStatus)
                'Revise Activity
                If Not Session("mPrevCompMonitorInspStatusForRevise") Is Nothing Then
                    Dim mPrevCompMonitorInspStatusForRevise As CompMonitorInspStatus
                    mPrevCompMonitorInspStatusForRevise = Session("mPrevCompMonitorInspStatusForRevise")
                    mPrevCompMonitorInspStatusForRevise.IsApplicable = False
                    mPrevCompMonitorInspStatusForRevise.Save()
                    Session.Remove("mPrevCompMonitorInspStatusForRevise")
                End If
                'End
                SaveAttachment() 'Added By Vikrant On 25-Nov-2014
                SaveMachineMaintenance()  'Added by Saylee on 9th-Oct-2009
                Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
                mCompInfo = Session("mCompInfo")
                'Changed by Vikrant on 28-July-2011
                Dim mDoneOnValues As New System.Text.StringBuilder
                For i As Integer = 0 To mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Count - 1
                    mDoneOnValues.Append(mCompMonitorInspStatus.CompMonitorInspStatusPeriods(i).DoneOnValueFormatted + ",")
                Next
                ''MarkLog(Util.Action.Save, "ComplyCompMonitorInspStatus", mCompInfo + "   " + ComplyCompMonitorInspInfo, Util.ErrorType.NoError, mCompMonitorInspStatus.ID)

                'Commented By Utkarsh On 28-Jul-2011 For All19072011

                '     MarkLog(Util.Action.Save, "ComponentInspections", mCompInfo, Util.ErrorType.NoError, mCompMonitorInspStatus.ID)
                'End
                Return True
            Catch ex As SqlException
                Session("mCompMonitorInspStatus") = clnCompMonitorInspStatus
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
                clnCompMonitorInspStatus = Nothing
                Dim mDoneOnValues As New System.Text.StringBuilder
                For i As Integer = 0 To mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Count - 1
                    mDoneOnValues.Append(mCompMonitorInspStatus.CompMonitorInspStatusPeriods(i).DoneOnValueFormatted + ",")
                Next
                If mCompStatus.IsSpareComp = False Then
                    MaintDetail = "Reg No. : " & mMachineMaintenance.RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorInspStatus.PartMonitorInsp.PartMonitorInspTypeName & " Done On Date : " + mCompMonitorInspStatus.DoneOnFormatted + " Done On Values : " + mDoneOnValues.ToString.TrimEnd(",")
                Else
                    MaintDetail = "Stock Component : Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorInspStatus.PartMonitorInsp.PartMonitorInspTypeName & " Done On Date : " + mCompMonitorInspStatus.DoneOnFormatted + " Done On Values : " + mDoneOnValues.ToString.TrimEnd(",")

                End If
                MarkLog(Util.Action.Save, "ComponentInspections", MaintDetail, Util.ErrorType.NoError, mCompMonitorInspStatus.ID, EventLogID)

            End Try
        Else
            upnlValidationSummary.Update()
            Return False
        End If
    End Function
    Private Sub SetTitle()
        Dim CompInfo As String = "[Part: " & mCompStatus.PartName & " Serial No. : " & mCompStatus.SerialNo & " ]"
        If mCompMonitorInspStatus.IsNew Then
            lblTitle.Text = "Comply Component Inspection Status " & CompInfo & " [New]"

        Else
            lblTitle.Text = "Comply Component Inspection Status " & CompInfo
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    'Revise Activity
                    If MSGBoxCtrl.Sender = "ReviseActivity" Then
                        MarkLog(Util.Action.[New], "Part Inspection", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                        Dim mPartMonitorInsp As PartMonitorInsp
                        Dim ID As Guid = Guid.NewGuid
                        mPartMonitorInsp = PartMonitorInsp.NewPartMonitorInsp(mCompMonitorInspStatus.PartMonitorInsp, mHourType)
                        Session("mPartMonitorInsp") = mPartMonitorInsp
                        'RemoveSession()
                        mPartMonitorInsp.BeginEdit()
                        Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
                        Session("mPrevCompMonitorInspStatusForRevise") = mCompMonitorInspStatus
                        Dim GChildPage2, GChildPage4, GChildPage5, GChildPage6 As String 'Dim GChildPageTmp As String = Request.QueryString("GChildPage4")
                        'ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenInspMasterWindow", "OpenInspMasterWindow('" + GChildPageTmp + "');", True)
                        GChildPage2 = Trim(Request.QueryString("GChildPage2"))
                        GChildPage4 = Trim(Request.QueryString("GChildPage4"))
                        GChildPage5 = Trim(Request.QueryString("GChildPage5"))
                        GChildPage6 = Trim(Request.QueryString("GChildPage6"))
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenInspMasterWindow", "OpenInspMasterWindow('" + GChildPage2 + "','" + GChildPage4 + "','" + GChildPage5 + "','" + GChildPage6 + "');", True)
                    ElseIf (MSGBoxCtrl.Sender = "OverDue" Or MSGBoxCtrl.Sender = "ComplyOnSameDate") Then 'Added by Saylee on 26-Mar-2019 for ALL26032019
                        'ComplyOnSameDate Added By Prashant 19-Nov-2019 Alert if user is complying on same date 
                        If Save() Then
                            If MSGBoxCtrl.Sender = "OverDue" Then
                                MarkLog(Util.Action.Save, "ComponentInspections", User.Identity.Name & " saved OverDue record : " & Session("OverDueString") & " " & Session("DueString"), Util.ErrorType.HandledError, mCompMonitorInspStatus.ID, EventLogID)
                            ElseIf MSGBoxCtrl.Sender = "ComplyOnSameDate" Then
                                MarkLog(Util.Action.Save, "ComponentInspections", User.Identity.Name & " Comply On Same Date : ", Util.ErrorType.HandledError, mCompMonitorInspStatus.ID, EventLogID)
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
                    'End


                Case MsgBoxResult.No
                    'Revise Activity
                    If MSGBoxCtrl.Sender = "ReviseActivity" Then
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
                    End If
                    'End
                Case MsgBoxResult.Cancel

                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added

                    Session("sender") = ""
                    DataFieldBind()
                    ControlVisibilityForDatePeriod()
                    'Response.Redirect("wfComplyAssemblyMonitorInspStatus_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
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
        btnPrint.Enabled = Not mCompMonitorInspStatus.IsNew
        dgCurrentValue.Columns(3).Visible = Not mCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID = 3
        dgCurrentValue.Columns(4).Visible = Not mCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID = 3
        'Added By Saylee on 28-08-2008
        dgDoneOnValue.Columns(2).Visible = Not mCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID = 3
        '==========================
        dgDoneOnValue.Columns(3).Visible = Not mCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID = 3
        'Added By Utkarsh ON 26-Jun-2013 FOR ALL26062013-1
        'dgDoneOnValue.Columns(4).Visible = (mCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID <> 3) AndAlso (mCompStatus.AssemblyTypeID <> 1 AndAlso mCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID <> 3)
        dgDoneOnValue.Columns(4).Visible = (mCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID <> 3) AndAlso (mCompStatus.AssemblyTypeID <> 1 AndAlso mCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID <> 3) AndAlso mIsSpareComponent <> 1  'IsSpareComponent Added By Prashant On 17-Sep-2020 For
        dgDoneOnValue.Columns(5).Visible = Not mCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID = 3
        'End
        If mCompMonitorInspStatus.PartMonitorInsp.ReadOnlyFrequencyColumn Then
            'txtDoneOnDate.Enabled = False 'Commented by Saylee on 22-Nov-2019 as DoneOne should be open in all cases, 
            chkApplicable.Enabled = False
        End If
        btnRevise.Enabled = (mCompMonitorInspStatus.IsApplicable And Not mCompMonitorInspStatus.IsNew And Not ((mCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID = 1 Or mCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID = 4) And mCompMonitorInspStatus.DoneOnFormatted.ToString <> "")) 'Revise Activity
        btnSelectLog.Visible = (mIsSpareComponent <> 1) ' Added By Prashant On 17-Sep-2020 For ALL27072020
        lnkPrintLogBookEntry.Visible = (mIsSpareComponent <> 1)
        ControlVisibilityForAttachment()
    End Sub
    Private Sub CopyFromClone(ByVal clnCompMonitorInspStatus As CompMonitorInspStatus)
        mCompMonitorInspStatus.DoneWONo = clnCompMonitorInspStatus.DoneWONo
        mCompMonitorInspStatus.DoneRemark = clnCompMonitorInspStatus.DoneRemark

        'Added by Saylee On 26-Apr-2012
        mCompMonitorInspStatus.DoneByID = clnCompMonitorInspStatus.DoneByID
        mCompMonitorInspStatus.LicenseNo = clnCompMonitorInspStatus.LicenseNo
        mCompMonitorInspStatus.Place = clnCompMonitorInspStatus.Place
        '*********************************************
        mCompMonitorInspStatus.IsAttachmentAdded = clnCompMonitorInspStatus.IsAttachmentAdded
        If Not mFileAttach Is Nothing Then
            mFileAttach.ReferenceID = mCompMonitorInspStatus.ID
            Session("mFileAttach") = mFileAttach
        End If

        'Commented and Added By Vikrant on 15-Apr-2021 to solve issue: Licence No not getting saved after select log
        'For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In clnCompMonitorInspStatus.MaintenanceDoneByEmployees
        '    mCompMonitorInspStatus.MaintenanceDoneByEmployees.Add(mCompMonitorInspStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
        'Next
        For j As Integer = mCompMonitorInspStatus.MaintenanceDoneByEmployees.Count - 1 To 0 Step -1
            mCompMonitorInspStatus.MaintenanceDoneByEmployees.RemoveAt(j)
        Next
        For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In clnCompMonitorInspStatus.MaintenanceDoneByEmployees
            If Not mCompMonitorInspStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.ID) Then
                mCompMonitorInspStatus.MaintenanceDoneByEmployees.Add(mCompMonitorInspStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
            Else
                If Not mCompMonitorInspStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.EmployeeID) Then
                    mCompMonitorInspStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).EmployeeID = mMaintenanceDoneByEmployee.EmployeeID
                    mCompMonitorInspStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).LicenceNo = mMaintenanceDoneByEmployee.LicenceNo
                    'mCompMonitorInspStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).RequiredManHours = mMaintenanceDoneByEmployee.RequiredManHours
                    mCompMonitorInspStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).EmployeeName = mMaintenanceDoneByEmployee.EmployeeName
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
        If Session("EnFrom") = 0 And Not (mMachineMaintenanceList.Contains(mCompMonitorInspStatus.ID, MaintenanceType.ComponentInspection, "")) Then
            mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(mMachineID, MaintenanceType.ComponentInspection, txtDoneOnDate.Text, mCompMonitorInspStatus.ID, Guid.Empty, 0, 0, IIf(mAssemblyStatus Is Nothing, Guid.Empty, mAssemblyStatusID))
        Else
            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mCompMonitorInspStatus.ID, MaintenanceType.ComponentInspection)
        End If

        With mMachineMaintenance
            ''.MachineID = mAssemblyStatus.MachineID
            ''.MaintenanceActivityTypeID =9
            .MaintenanceID = mCompMonitorInspStatus.ID 'TransactionID
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
    Private Sub ControlVisibilityForGridBeforeBinding()
        dgCurrentValue.Columns(3).Visible = True
        dgCurrentValue.Columns(4).Visible = True
        dgDoneOnValue.Columns(2).Visible = True
        dgDoneOnValue.Columns(3).Visible = True
        dgDoneOnValue.Columns(4).Visible = True
        dgDoneOnValue.Columns(5).Visible = True
    End Sub
    'Added By Prashant On 27-Nov-2014
    Private Sub NewRecordAttachment()
        mFileAttach = FileAttach.NewAttachment(Guid.Empty, mCompMonitorInspStatus.ID)
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
        If mCompMonitorInspStatus.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mCompMonitorInspStatus.ID)
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
            If (Not mCompMonitorInspStatus.IsNew) And IsAttachmentDeleted Then
                FileAttach.DeleteAttachment(mFileAttach.ID, mCompMonitorInspStatus.ID)
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
        If mCompMonitorInspStatus.MaintenanceDoneByEmployees.Count > 1 Then
            lblLicenceCount.Text = "and " + (mCompMonitorInspStatus.MaintenanceDoneByEmployees.Count - 1).ToString + " more"
        End If
        lblLicenceCount.DataBind()
        'lblAllLicenceNos.DataBind()
    End Sub
    Private Sub BindLicenceNo()
        If mCompMonitorInspStatus.MaintenanceDoneByEmployees.Count > 0 Then
            txtLicenceNo.Text = mCompMonitorInspStatus.MaintenanceDoneByEmployees(0).LicenceNo + " [" + mCompMonitorInspStatus.MaintenanceDoneByEmployees(0).EmployeeName + "]"
        Else
            txtLicenceNo.Text = String.Empty
        End If
    End Sub
    'End
    Private Sub ControlVisibilityForDatePeriod()
        Dim txtDnOnDate As TextBox
        For j As Integer = 0 To Me.dgDoneOnValue.Rows.Count - 1
            txtDnOnDate = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtCurrentValue"), TextBox)
            With mCompMonitorInspStatus.CompMonitorInspStatusPeriods

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
        dgCurrentValue.DataSource = mCompMonitorInspStatus.CompMonitorInspStatusPeriods
        dgDoneOnValue.DataSource = mCompMonitorInspStatus.CompMonitorInspStatusPeriods
        'Added On 28,May,2007 By Girish
        txtDoneOnDate.Text = mCompMonitorInspStatus.DoneOnFormatted.ToString
        txtExtensionDate.Text = mCompMonitorInspStatus.ExtensionDateFormatted.ToString

        'Added by Saylee on 9th-Oct-2009
        mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList()
        Session("mMachineMaintenanceList") = mMachineMaintenanceList

        If Val(mCompMonitorInspStatus.PartMonitorInsp.RequiredManHours) > 0 Then
            lblEstdManHours.Text = "(Estd. Man Hours : " + mCompMonitorInspStatus.PartMonitorInsp.RequiredManHours + ")"
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
        Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
        dgCurrentValue.DataSource = mCompMonitorInspStatus.CompMonitorInspStatusPeriods
        dgDoneOnValue.DataSource = mCompMonitorInspStatus.CompMonitorInspStatusPeriods
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
        If Not mCompMonitorInspStatus.IsValid Then
            For i As Integer = 0 To mCompMonitorInspStatus.GetBrokenRulesCollection.Count - 1
                str = str + mCompMonitorInspStatus.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(dgDoneOnValue.Rows.Count - 1)
            txtCurrentValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtCurrentValue"), TextBox)
            If Not mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
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
            If Not mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
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

            'If Not mCompMonitorInspStatus.IsNew And Session("From") = 1 Then

            '    'Added By Saylee On 9-FEB-2021 For Mismatch Value Mail Send of Controls
            '    If txtDoneOnDate.Text <> "" AndAlso mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Contains(2, "") Then 'If date period conatins then only execute
            '        Dim DoneOnValue As New StringBuilder
            '        Dim DoneOnValueObj As New StringBuilder
            '        Dim ControlDoneOnValue As String = String.Empty
            '        For j As Integer = 0 To Me.dgDoneOnValue.Rows.Count - 1
            '            DoneOnValue.Append(CType(Me.dgDoneOnValue.Rows(j).FindControl("txtCurrentValue"), TextBox).Text + ", ")
            '            DoneOnValueObj.Append(mCompMonitorInspStatus.CompMonitorInspStatusPeriods(j).CurrentValueFormatted + ", ")
            '            If mCompMonitorInspStatus.CompMonitorInspStatusPeriods(j).PeriodID = 2 Then
            '                ControlDoneOnValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtCurrentValue"), TextBox).Text
            '                If Not txtDoneOnDate.Text.ToString.Equals(ControlDoneOnValue) Then
            '                    Session("IsSendMail") = "True"
            '                End If
            '            End If
            '        Next j
            '        If Session("IsSendMail") = "True" Then
            '            Session.Remove("IsSendMail")
            '            SendMail(mCompMonitorInspStatus, DoneOnValue.ToString.Trim.TrimEnd(","), DoneOnValueObj.ToString.Trim.TrimEnd(","), True, ToMailIDs:="deven@bytzsoft.com,saylee@bytzsoft.com")
            '        End If
            '    End If
            '    'End
            'End If
        End If
    End Sub
    Private Sub dgDoneOnValue_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Select Case e.CommandName
            Case "CurrentValue"
                If Not IsValid Then Exit Sub
                Dim txtCurrentValue As TextBox
                For i As Integer = 0 To dgDoneOnValue.Rows.Count - 1
                    txtCurrentValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtCurrentValue"), TextBox)
                    With mCompMonitorInspStatus.CompMonitorInspStatusPeriods()
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
                For i As Integer = 0 To mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Count - 1
                    txtExtensionValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtExtensionValue"), TextBox)

                    With mCompMonitorInspStatus.CompMonitorInspStatusPeriods
                        .Item(i).ExtensionValue = Trim(txtExtensionValue.Text)
                    End With
                Next
                DataBindGrid()
        End Select
    End Sub
    Protected Sub txtCurrentValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ' If Not IsValid Then Exit Sub
        Dim txtCurrentValue As TextBox
        For i As Integer = 0 To dgDoneOnValue.Rows.Count - 1
            txtCurrentValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtCurrentValue"), TextBox)
            With mCompMonitorInspStatus.CompMonitorInspStatusPeriods
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
    End Sub
    Protected Sub txtExtensionValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtExtensionValue As TextBox
        For i As Integer = 0 To mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Count - 1
            txtExtensionValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtExtensionValue"), TextBox)

            With mCompMonitorInspStatus.CompMonitorInspStatusPeriods
                .Item(i).ExtensionValue = Trim(txtExtensionValue.Text)
            End With
        Next
        ControlVisibilityForGridBeforeBinding()
        DataBindGrid()
        ControlVisibility()
        'upnlDoneOnValueGrid.Update()
        upnlCurrentValueGrid.Update()
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
                If CDate(mCompMonitorInspStatus.DoneOn) < CDate(mCompStatus.InstalledOn) Then
                    ConsiderCompInstValue = True
                End If
            End If
            '******************************************************************

            Dim clnCompMonitorInspStatus As CompMonitorInspStatus = mCompMonitorInspStatus.Clone
            If mEnFrom = From.NewRecord Then
                mCompMonitorInspStatus = CompMonitorInspStatus.NewComplyCompMonitorInspStatus(Guid.NewGuid, mPrevCompMonitorInspStatus.CompID, mPrevCompMonitorInspStatus.AssemblyStatusID, txtDoneOnDate.Text, mCompStatus.Comp.PartID, mPrevCompMonitorInspStatus.PartMonitorInsp, Guid.Empty, mPrevCompMonitorInspStatus.CompStatusID, mPrevCompMonitorInspStatus.DoneOn.ToString, mHourType, , ConsiderCompInstValue, IsForSpareComp:=mIsSpareComponent)
            Else
                mCompMonitorInspStatus = CompMonitorInspStatus.GetComplyCompMonitorInspStatus(mPrevCompMonitorInspStatus.ID, mPrevCompMonitorInspStatus.AssemblyStatusID, mPrevCompMonitorInspStatus.CompStatusID, txtDoneOnDate.Text, Guid.Empty, mHourType, , ConsiderCompInstValue, IsForSpareComp:=mIsSpareComponent)
            End If
            CopyFromClone(clnCompMonitorInspStatus)
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
        Session("mAssemblyStatusId") = mCompMonitorInspStatus.AssemblyStatusID.ToString
        Session("mAssemblyID") = mAssemblyStatus.AssemblyID.ToString
        Session("mDoneOn") = CStr(IIf(txtDoneOnDate.Text = "", Today.Date.ToShortDateString, txtDoneOnDate.Text))

        'Added by Saylee on 14-Mar-2016 for ALL11032016
        If mAssemblyStatus.InstalledOn.ToString <> "" Then
            If CDate(mCompMonitorInspStatus.DoneOn) <= CDate(mAssemblyStatus.InstalledOn) Then 'if Compliance date is same or less than Assembly Inst. Date
                Dim mFirstLogDetailAfterAssemblyInstallation As FirstLogDetailAfterAssemblyInstallation = FirstLogDetailAfterAssemblyInstallation.GetFirstLogDetailAfterAssemblyInstallation(mAssemblyStatus)
                Session("mFirstLogDetailAfterAssemblyInstallation") = mFirstLogDetailAfterAssemblyInstallation
            End If
            '*************************************************
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSelectLogWindow", "OpenSelectLogWindow()", True)
        'Response.Redirect("wfSelectLog_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&BackPage6=wfComplyAssemblyMonitorInspStatus_Ajax.aspx" & "&FromType=3&DoneOn=" & CStr(IIf(txtDoneOnDate.Text = "", Today.Date.ToShortDateString, txtDoneOnDate.Text)) & "&MachineId=" & mAssemblyStatus.MachineID.ToString & "&AssemblyStatusID=" & mCompMonitorInspStatus.AssemblyStatusID.ToString & "&AssemblyID=" & mAssemblyStatus.AssemblyID.ToString)
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("ComponentInspectionsNew") And mCompMonitorInspStatus.IsNew) Or (Not User.IsInRole("ComponentInspectionsEdit") And Not mCompMonitorInspStatus.IsNew) Then
            'Changed by Vikrant on 28-July-2011
            mMonitorInfo = txtMonitorInspType.Text
            mMonitorType = txtMonitorType.Text
            mPart = mCompStatus.PartName
            mSerialNo = mCompStatus.Comp.SerialNo

            If mCompStatus.IsSpareComp = False Then
                MaintDetail = "Reg No. : " + mMachine.RegNo + " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorInspStatus.PartMonitorInsp.PartMonitorInspTypeName
            Else
                MaintDetail = "Stock Component:  Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorInspStatus.PartMonitorInsp.PartMonitorInspTypeName
            End If
            MarkLog(Util.Action.Save, "ComponentInspections", User.Identity.Name & " is not Authorized User to save " & mInspectionDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        If Not IsValid Then
            upnlValidationSummary.Update()
            Exit Sub
        End If

        If IsValid Then

            'Code for OverDue 'Added by Saylee on 26-Mar-2019 for ALL26032019
            If Not mPrevCompMonitorInspStatus.PartMonitorInsp.MonitorTypeID = 3 Then  'No Frequency record not be checked for OverDue
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
            If mPrevCompMonitorInspStatus.DoneOn.ToString <> "" Then
                If (CDate(txtDoneOnDate.Text) <= CDate(mPrevCompMonitorInspStatus.DoneOn) And Session("EnFrom") <> 1) Then
                    MSGBoxCtrl.show("Alert!!!", "Current compliance date is less than or equal to last compliance date ", "Do you want to continue?", MsgBoxStyle.YesNo, "ComplyOnSameDate")
                    Exit Sub
                End If
                'If CDate(txtDoneOnDate.Text) > CDate(mPrevCompMonitorInspStatus.DoneOn) Then
                '    MSGBoxCtrl.show("Alert!!!", "Current compliance date is greater than last compliance date ", "Do you want to continue?", MsgBoxStyle.YesNo, "ComplyOnSameDate")
                '    Exit Sub
                'End If
            End If
            If (CDate(txtDoneOnDate.Text) > CDate(Today.Date) And Session("EnFrom") <> 1) Then
                MSGBoxCtrl.show("Alert!!!", "Current compliance date is greater than today date  ", "Do you want to continue?", MsgBoxStyle.YesNo, "ComplyOnSameDate")
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
        End If
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Changed by Vikrant on 28-July-2011
        MarkLog(Util.Action.Close, "ComponentInspections", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)

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
        mCompMonitorInspStatus.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte

        If mCompMonitorInspStatus.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mCompMonitorInspStatus.ID)
            Session("mFileAttach") = mFileAttach
        End If

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        IsAttachmentDeleted = True
        mCompMonitorInspStatus.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        ViewImage()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mCompMonitorInspStatus.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mCompMonitorInspStatus.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mCompMonitorInspStatus.ID)
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

            'If DateDiff(DateInterval.Day, SmartDate.StringToDate(mPrevCompMonitorInspStatus.AsOnDate), SmartDate.StringToDate(LogDate)) > 0 Then
            '    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DoneOnDate, SIMsgBox.Message_text.DoneOnDate, "Compliance record only upto " & CStr(mPrevCompMonitorInspStatus.AsOnDate) & " can be entered through Comp Installation screen", MsgBoxStyle.OKOnly)
            '    msg1.ReplacePage = "wfComplyCompMonitorInspStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
            '    msg1.Show()
            '    Exit Sub
            'End If

            '******************************************************************
            'Added by Saylee on 11-Jan-2017
            'ConsiderCompInstValue=True only if Compliance date is less than Comp Inst Date then consider Current vlaue 
            'If False,then Comp Current Values will be calculated

            Dim ConsiderCompInstValue As Boolean = False
            If txtDoneOnDate.Text <> "" And mCompStatus.InstalledOn.ToString <> "" Then
                If CDate(mCompMonitorInspStatus.DoneOn) < CDate(mCompStatus.InstalledOn) Then
                    ConsiderCompInstValue = True
                End If
            End If
            '******************************************************************

            Dim clnCompMonitorInspStatus As CompMonitorInspStatus = mCompMonitorInspStatus.Clone
            If mEnFrom = From.NewRecord Then
                mCompMonitorInspStatus = CompMonitorInspStatus.NewComplyCompMonitorInspStatus(Guid.NewGuid, mPrevCompMonitorInspStatus.CompID, mPrevCompMonitorInspStatus.AssemblyStatusID, LogDate, mCompStatus.Comp.PartID, mPrevCompMonitorInspStatus.PartMonitorInsp, LogId, mPrevCompMonitorInspStatus.CompStatusID, mPrevCompMonitorInspStatus.DoneOn.ToString, mHourType, CType(Session("ConsiderAssemblyInstValue"), Boolean), ConsiderCompInstValue, IsForSpareComp:=mIsSpareComponent)
            Else
                mCompMonitorInspStatus = CompMonitorInspStatus.GetComplyCompMonitorInspStatus(mPrevCompMonitorInspStatus.ID, mPrevCompMonitorInspStatus.AssemblyStatusID, mPrevCompMonitorInspStatus.CompStatusID, LogDate, LogId, mHourType, CType(Session("ConsiderAssemblyInstValue"), Boolean), ConsiderCompInstValue, IsForSpareComp:=mIsSpareComponent)
            End If
            mCompMonitorInspStatus.DoneWONo = clnCompMonitorInspStatus.DoneWONo
            mCompMonitorInspStatus.DoneRemark = clnCompMonitorInspStatus.DoneRemark
            mCompMonitorInspStatus.DoneOn = clnCompMonitorInspStatus.DoneOn
            mCompMonitorInspStatus.RequiredManHours = clnCompMonitorInspStatus.RequiredManHours
            'mCompMonitorInspStatus.CompMonitorInspStatusPeriods = clnCompMonitorInspStatus.CompMonitorInspStatusPeriods
            mCompMonitorInspStatus.IsAttachmentAdded = clnCompMonitorInspStatus.IsAttachmentAdded
            'Added By Vikrant on 15-Apr-2021 to solve issue: Licence No not getting saved after select log
            For j As Integer = mCompMonitorInspStatus.MaintenanceDoneByEmployees.Count - 1 To 0 Step -1
                mCompMonitorInspStatus.MaintenanceDoneByEmployees.RemoveAt(j)
            Next
            For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In clnCompMonitorInspStatus.MaintenanceDoneByEmployees
                If Not mCompMonitorInspStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.ID) Then
                    mCompMonitorInspStatus.MaintenanceDoneByEmployees.Add(mCompMonitorInspStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
                Else
                    If Not mCompMonitorInspStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.EmployeeID) Then
                        mCompMonitorInspStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).EmployeeID = mMaintenanceDoneByEmployee.EmployeeID
                        mCompMonitorInspStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).LicenceNo = mMaintenanceDoneByEmployee.LicenceNo
                        mCompMonitorInspStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).RequiredManHours = mMaintenanceDoneByEmployee.RequiredManHours
                        mCompMonitorInspStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).EmployeeName = mMaintenanceDoneByEmployee.EmployeeName
                    End If
                End If
            Next
            'End
            If Not mFileAttach Is Nothing Then
                mFileAttach.ReferenceID = mCompMonitorInspStatus.ID
                Session("mFileAttach") = mFileAttach
            End If
            Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
            clnCompMonitorInspStatus = Nothing
            'DataBindGrid()
            'Added by Saylee on 9th-Oct-2009
            Dim mLog As Log
            mLog = Log.GetLog(New Guid(LogId.ToString))
            Session("mLog") = mLog
            '===================================
        Else
            Session.Remove("mLog")
        End If
        SetGridFromObject()
        DataBindGrid()
        ControlVisibility()
        SetTitle()

        upnlCurrentValueGrid.Update()
        upnlDoneOnValueGrid.Update()
    End Sub
    'MLNo
    Private Sub imgbtnEmployeeLicence_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgbtnEmployeeLicence.Click
        If IsValid Then
            SetObject()
            Session("mMaintenanceID") = mCompMonitorInspStatus.ID
            Session("MaintenanceDoneOnDate") = mCompMonitorInspStatus.DoneOn.ToString
            mMaintenanceDoneByEmployees = mCompMonitorInspStatus.MaintenanceDoneByEmployees
            Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AddEmployeeLicNo", "AddEmployeeLicNo();", True)
        Else
            upnlValidationSummary.Update()
        End If

    End Sub
    Private Sub hdnBtnMaintDoneBy_Click(sender As Object, e As System.EventArgs) Handles hdnBtnMaintDoneBy.Click
        For i As Integer = 0 To mMaintenanceDoneByEmployees.Count - 1
            Dim ID As Guid = mMaintenanceDoneByEmployees(i).ID
            If Not mCompMonitorInspStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mCompMonitorInspStatus.MaintenanceDoneByEmployees.Add(mMaintenanceDoneByEmployees(i))
            ElseIf mCompMonitorInspStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mCompMonitorInspStatus.MaintenanceDoneByEmployees(ID).LicenceNo = mMaintenanceDoneByEmployees(i).LicenceNo
                mCompMonitorInspStatus.MaintenanceDoneByEmployees(ID).RequiredManHours = mMaintenanceDoneByEmployees(i).RequiredManHours
                mCompMonitorInspStatus.MaintenanceDoneByEmployees(ID).EmployeeID = mMaintenanceDoneByEmployees(i).EmployeeID
                mCompMonitorInspStatus.MaintenanceDoneByEmployees(ID).EmployeeName = mMaintenanceDoneByEmployees(i).EmployeeName
            End If
        Next

        For j As Integer = 0 To mCompMonitorInspStatus.MaintenanceDoneByEmployees.Count - 1
            If Not mMaintenanceDoneByEmployees.Contains(mCompMonitorInspStatus.MaintenanceDoneByEmployees(j).ID) Then
                mCompMonitorInspStatus.MaintenanceDoneByEmployees.Remove(mCompMonitorInspStatus.MaintenanceDoneByEmployees(j).ID, "")
            End If
        Next
        Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
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
            If mCompMonitorInspStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mCompMonitorInspStatus.MaintenanceDoneByEmployees(0).EmployeeID = DoneByID
                mCompMonitorInspStatus.MaintenanceDoneByEmployees(0).LicenceNo = LicenseNo
                If Not mCompMonitorInspStatus.MaintenanceDoneByEmployees.Count > 1 Then 'If Condition added by Vikrant On 15-Apr-2021 to solve issue:Hours getting added for multiple licence no and if first licence no changed
                    mCompMonitorInspStatus.MaintenanceDoneByEmployees(0).RequiredManHours = txtActualManHours.Text
                End If
                mCompMonitorInspStatus.MaintenanceDoneByEmployees(0).EmployeeName = EmpName
            Else
                mCompMonitorInspStatus.MaintenanceDoneByEmployees.Add(mCompMonitorInspStatus.ID, MaintenanceType.ComponentInspection, DoneByID, LicenseNo, txtActualManHours.Text, EmpName)
            End If

        Else
            If mCompMonitorInspStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mCompMonitorInspStatus.MaintenanceDoneByEmployees.RemoveAt(0)
            End If
        End If
        Session("mCompMonitorInspStatus") = mCompMonitorInspStatus
        BindLicenceNo()
        SetLicenceCount()
        txtActualManHours.DataBind()
        upnlMonitoringStatusDetails.Update()
    End Sub
    Protected Sub txtActualManHours_TextChanged(sender As Object, e As System.EventArgs)
        If mCompMonitorInspStatus.MaintenanceDoneByEmployees.Count > 0 Then
            mCompMonitorInspStatus.MaintenanceDoneByEmployees(0).RequiredManHours = txtActualManHours.Text
            upnlMonitoringStatusDetails.Update()
        End If
    End Sub
    'End
    'Revise Activity
    Private Sub btnRevise_Click(sender As Object, e As System.EventArgs) Handles btnRevise.Click
        MSGBoxCtrl.show("Alert!", "You are about to Revise Part Activity.After revision of Part activity this Status will become Not Applicable.", "Do you want to continue?", MsgBoxStyle.YesNo, "ReviseActivity")
    End Sub
    'End
#End Region

#Region "Report Variable"
    Dim mCompanyDetail As New CompanyDetail
    'Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
#End Region

#Region " Events "
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If (Not User.IsInRole("AssemblyInspectionsPrint")) Then
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
        LHCount = 5
        RHCount = Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            ReportDetails.Add(New rptStatus(, 0, "Monitoring Status Details", "Insp Type", _
                  txtMonitorInspType.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                  dgCurrentValue.Columns.Item(1).HeaderText, dgCurrentValue.Columns.Item(2).HeaderText, _
                    , dgCurrentValue.Columns.Item(3).HeaderText, , dgCurrentValue.Columns.Item(4).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 0, "Monitoring Status Details", "Insp Type", _
                            txtMonitorInspType.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                                  "", "", , "", , ""))
        End If
        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Status Details", "Monitor Type", _
                             txtMonitorType.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).PeriodUnitName, String), _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).FrequencyValueFormatted, String), , _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).ElapsedValueFormatted, String), , _
                 CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Status Details", "ATA Chapter", _
                            txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , ""))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Status Details", "ATA Chapter", _
                             txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).PeriodUnitName, String), _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).FrequencyValueFormatted, String), , _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).ElapsedValueFormatted, String), , _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Status Details", "ATA Chapter", _
                            txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , ""))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Status Details", "Reference", _
                             txtReference.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).PeriodUnitName, String), _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).FrequencyValueFormatted, String), , _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).ElapsedValueFormatted, String), , _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Status Details", "Reference", _
                                txtReference.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                                "", "", , "", , ""))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Status Details", "Description", _
                                   txtDescription.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).PeriodUnitName, String), _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).FrequencyValueFormatted, String), , _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).ElapsedValueFormatted, String), , _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Status Details", "Description", _
                                    txtDescription.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , ""))
                End If
            ElseIf I = 4 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Status Details", "", _
                                    "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).PeriodUnitName, String), _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).FrequencyValueFormatted, String), , _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).ElapsedValueFormatted, String), , _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).RemainingValueFormatted, String), , lblNote.Text))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Status Details", "", _
                                        "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , "", , lblNote.Text))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, "Monitoring Status Details", "", _
                                         "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).PeriodUnitName, String), _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).FrequencyValueFormatted, String), , _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).ElapsedValueFormatted, String), , _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(I).RemainingValueFormatted, String), , lblNote.Text))
            End If
        Next

        'For Done On Value Grid
        Dim TotalCount1 As Integer
        Dim LHCount1 As Integer
        Dim RHCount1 As Integer
        LHCount1 = 7
        RHCount1 = Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Count
        If LHCount1 > RHCount1 Then
            TotalCount1 = LHCount1
        Else
            TotalCount1 = RHCount1
        End If

        Dim temp1 As Integer
        temp1 = 0
        If temp1 < RHCount1 Then
            ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Done On", _
            New SmartDate(txtDoneOnDate.Text).FormattedText, , , , , , , , , , , , , , , , , "Component Values at Compliance of Inspection", _
            dgDoneOnValue.Columns.Item(0).HeaderText, dgDoneOnValue.Columns.Item(1).HeaderText, _
         , dgDoneOnValue.Columns.Item(2).HeaderText, , dgDoneOnValue.Columns.Item(3).HeaderText, RHData3:=dgDoneOnValue.Columns.Item(5).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Done On", _
                            New SmartDate(txtDoneOnDate.Text).FormattedText, , , , , , , , , , , , , , , , , "Component Values at Compliance of Inspection", _
                                  "", "", , "", , "", ""))
        End If


        Dim m As Integer
        For m = 0 To TotalCount1 - 1
            If m = 0 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Work Order No.", _
                    txtWorkOrderNo.Text, , , , , , , , , , , , , , , , , "Component Values at Compliance of Inspection", _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).CurrentValueFormatted, String), , _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).DueOnValueFormatted, String), RHData3:=CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Work Order No.", _
                            txtWorkOrderNo.Text, , , , , , , , , , , , , , , , , "Component Values at Compliance of Inspection", _
                                "", "", , "", , "", ""))
                End If
            ElseIf m = 1 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Remark", _
                     txtRemark.Text, , , , , , , , , , , , , , , , , "Component Values at Compliance of Inspection", _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).CurrentValueFormatted, String), , _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).DueOnValueFormatted, String), RHData3:=CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Remark", _
                            txtRemark.Text, , , , , , , , , , , , , , , , , "Component Values at Compliance of Inspection", _
                                "", "", , "", , "", ""))
                End If
                'ElseIf m = 2 Then
                '    If m < RHCount1 Then
                '        ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Extension Date", _
                '         txtExtensionDate.Text, , , , , , , , , , , , , , , , , "Component Values at Compliance of Inspection", _
                '    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                '    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).CurrentValueFormatted, String), , _
                '    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).DueOnValueFormatted, String), RHData3:=CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String)))
                '    Else
                '        ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Extension Date", _
                '                txtExtensionDate.Text, , , , , , , , , , , , , , , , , "Component Values at Compliance of Inspection", _
                '                    "", "", , "", , "", "", , ))
                '    End If
                'ElseIf m = 3 Then
                '    If m < RHCount1 Then
                '        ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Approval Remark", _
                '         txtApprovalRemark.Text, , , , , , , , , , , , , , , , , "Component Values at Compliance of Inspection", _
                '    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                '    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).CurrentValueFormatted, String), , _
                '    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).DueOnValueFormatted, String), RHData3:=CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String)))
                '    Else
                '        ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Approval Remark", _
                '                txtApprovalRemark.Text, , , , , , , , , , , , , , , , , "Component Values at Compliance of Inspection", _
                '                    "", "", , "", , "", "", , ))
                '    End If
            ElseIf m = 2 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Actual Man Hours", _
                     txtActualManHours.Text, , , , , , , , , , , , , , , , , "Component Values at Compliance of Inspection", _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).CurrentValueFormatted, String), , _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).DueOnValueFormatted, String), RHData3:=CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Actual Man Hours", _
                            txtActualManHours.Text, , , , , , , , , , , , , , , , , "Component Values at Compliance of Inspection", _
                                "", "", , "", , "", "", , ))
                End If
            ElseIf m = 3 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Done By Agency", _
                     txtDoneBy.Text, , , , , , , , , , , , , , , , , "Component Values at Compliance of Inspection", _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).CurrentValueFormatted, String), , _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).DueOnValueFormatted, String), RHData3:=CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Done By Agency", _
                            txtDoneBy.Text, , , , , , , , , , , , , , , , , "Component Values at Compliance of Inspection", _
                                "", "", , "", , "", "", , ))
                End If
            ElseIf m = 4 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "License No.", _
                     mCompMonitorInspStatus.AllLicenceNosWithEmpName, , , , , , , , , , , , , , , , , "Component Values at Compliance of Inspection", _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).CurrentValueFormatted, String), , _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).DueOnValueFormatted, String), RHData3:=CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "License No.", _
                            mCompMonitorInspStatus.AllLicenceNosWithEmpName, , , , , , , , , , , , , , , , , "Component Values at Compliance of Inspection", _
                                "", "", , "", , "", "", , ))
                End If
            ElseIf m = 5 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Place", _
                     txtPlace.Text, , , , , , , , , , , , , , , , , "Component Values at Compliance of Inspection", _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).CurrentValueFormatted, String), , _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).DueOnValueFormatted, String), RHData3:=CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Place", _
                            txtPlace.Text, , , , , , , , , , , , , , , , , "Component Values at Compliance of Inspection", _
                                "", "", , "", , "", "", , ))
                End If
            ElseIf m = 6 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "", _
                    "", , , , , , , , , , , , , , , , , "Component Values at Compliance of Inspection", _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).CurrentValueFormatted, String), , _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).DueOnValueFormatted, String), _
                            , lblNote1.Text))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "", _
                    "", , , , , , , , , , , , , , , , , "Component Values at Compliance of Inspection", _
                           "", "", , "", , "", "", lblNote1.Text, ))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "", _
                                   "", , , , , , , , , , , , , , , , , "Component Values at Compliance of Inspection", _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).CurrentValueFormatted, String), , _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(m).DueOnValueFormatted, String), _
                                           , lblNote1.Text))
            End If
        Next

        '***********************************************************************************************************************
        'For Document Details
        Dim TotalCount2 As Integer
        Dim LHCount2 As Integer
        Dim RHCount2 As Integer
        LHCount2 = 3
        RHCount2 = Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods.Count
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
            dgDoneOnValue.Columns.Item(4).HeaderText, dgDoneOnValue.Columns.Item(5).HeaderText))
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
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).PeriodUnitName, String), _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).CurrentValueFormatted, String), "Approval Remark", _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).ExtensionValueFormatted, String), txtApprovalRemark.Text, _
                    CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).DueOnValueFormatted, String), , ))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Page No.", _
                        txtPageNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                        "", txtApprovalRemark.Text, , "", , "", ""))
                End If
            ElseIf n = 1 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Book No.", _
                    txtBookNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).PeriodUnitName, String), _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).CurrentValueFormatted, String), , _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).ExtensionValueFormatted, String), , _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).DueOnValueFormatted, String), RHData3:=CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).AssemblyDueOnValueFormattedByAirFrame, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Book No.", _
                        txtBookNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    "", "", , "", , "", ""))
                End If
            ElseIf n = 2 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Source Doc ", _
                    txtSourceDoc.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).PeriodUnitName, String), _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).CurrentValueFormatted, String), , _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).ExtensionValueFormatted, String), , _
                CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).DueOnValueFormatted, String), RHData3:=CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).AssemblyDueOnValueFormattedByAirFrame, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Source Doc ", _
                        txtSourceDoc.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    "", "", , "", , "", ""))
                End If

            Else
                ReportDetails.Add(New rptStatus(, 2, "Document Details", "", _
                                 "", , , , , , , , , , , , , , , , , "Component Values at Compliance of Service", _
                  CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).PeriodUnitName, String), _
                  CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).CurrentValueFormatted, String), , _
                  CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).ExtensionValueFormatted, String), , _
                  CType(Me.mCompMonitorInspStatus.CompMonitorInspStatusPeriods(n).DueOnValueFormatted, String), _
                  , lblNote1.Text))
            End If
        Next
        '***********************************************************************************************************************

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Comply Component Inspection Status Detail Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        'MarkLog(Util.Action.Print, "ComplyAssemblyMonitorInspStatus", mAssemblyInfo + " -> " + "Comply Assembly Monitor Inspection Status Detail Report", Util.ErrorType.NoError, mCompMonitorInspStatus.ID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    Private Sub lnkPrintLogBookEntry_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPrintLogBookEntry.Click  'Added By Saylee On 18-May-2021 ALL07052021
        Dim RptCommonHistory As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mLogEntryFormat As New LogEntryFormat
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsReportHistoryList
        Dim mCompanyDetail As New CompanyDetail

        RptCommonHistory = New crptLogEntryFormat

        mLogEntryFormat = LogEntryFormat.GetHistoryList(mCompMonitorInspStatus.DoneOn, mCompMonitorInspStatus.DoneOn, "", mAssemblyStatus.AssemblyTypeName, _
                                                        mAssemblyStatus.ModelName, mAssemblyStatus.Assembly.SerialNo, "", "", "", "", _
                                                        mAssemblyStatus.MachineID.ToString, False, True, IsRemoved:=False, IsInstalled:=True, _
                                                        IsComplied:=False, AssemblyID:=mAssemblyStatus.AssemblyID.ToString, IsLogNo:=True, _
                                                        IsLogPageNo:=False, IsFlightNo:=False, IsMELRequired:=False, IsMaintenanceActivityRequired:=False, _
                                                        AssemblyTypeID:=mAssemblyStatus.AssemblyTypeID, CompStatusID:=mCompStatus.ID.ToString, _
                                                        ShowService:=False, ShowDir:=False, ShowInsp:=True, CompMonitorInspStatusID:=mCompMonitorInspStatus.ID.ToString)
        If mLogEntryFormat.Count = 0 Then
            Exit Sub
        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
           mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
           mCompanyDetail.WebSite, "LOG BOOK ENTRY", "", mCompMonitorInspStatus.DoneOnFormatted, Machine.GetMachine(mAssemblyStatus.MachineID).RegNo, _
           mAssemblyStatus.ModelName + "-" + mAssemblyStatus.Assembly.SerialNo, IIf(mAssemblyStatus.AssemblyTypeName.Equals("Airframe"), "AIRCRAFT", mAssemblyStatus.AssemblyTypeName.ToUpper), _
           AppSettings("Product Version"), AppSettings("SINote"), _
           "AVERAGE FUEL CONSUMPTION________LTR./HR & AVERAGE OIL CONSUMPTION________LTR./HR SINCE LAST SMI DONE.  BOTH THE FIGURES ARE BELOW THE ALERT VALUE.", _
           "True", mCompMonitorInspStatus.DoneOnFormatted, "", AppSettings("Logo"))

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