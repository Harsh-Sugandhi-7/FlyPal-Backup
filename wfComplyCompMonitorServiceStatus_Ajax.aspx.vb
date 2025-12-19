'AJAX Conversion By Vikrant On 16-Apr-2015
Imports System.Linq
Imports System.Text 'Added By Vikrant On 17-Sep-2020 For Mismatch Value Mail Send
Public Class wfComplyCompMonitorServiceStatus_Ajax
    Inherits System.Web.UI.Page

#Region " Enum "
    Public Enum From
        NewRecord = 0
        EditRecord = 1
    End Enum
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

#Region " Variable Declaration "
    Public mEnFrom As From
    Public mMachine As Machine
    Public mCompStatus As CompStatus
    Public mAssemblyStatus As AssemblyStatus
    Public mPrevCompMonitorServiceStatus As CompMonitorServiceStatus
    Public mCompMonitorServiceStatus As CompMonitorServiceStatus
    Dim Flag As Int16
    Public mCompInfo As String                      'Code Added 29,Jan,2007
    Public ComplyCompMonitorServiceInfo As String   'Code Added 29,Jan,2007

    Public mMachineMaintenance As MachineMaintenance 'Added by Saylee on 9th-Oct-2009
    Public mMachineMaintenanceList As MachineMaintenanceList 'Added by Saylee on 9th-Oct-2009

    Dim EventLogID As Guid 'Added By Utkarsh On 27-Jul-2011 For All19072011
    Dim MaintDetail As String 'Added By Utkarsh On 27-Jul-2011 For All19072011
    Dim mEmployeeStatus As EmployeeStatus 'Added By Vikrant On 06-Aug-2013 For ALL01082013
    'Added By Prashant On 27-Nov-2014
    Dim mFileAttach As FileAttach
    Dim IsAttachmentDeleted As Boolean = False
    'End
    'MLNo
    Dim LicenseNo As String = String.Empty
    Dim EmpName As String = String.Empty
    Dim DoneByID As Guid = Guid.Empty
    Dim mMaintenanceDoneByEmployees As New MaintenanceDoneByEmployees
    Shared UserNameForLicenceList As String
    'End
    Public OverDueString As String = ""
    Public mIsSpareComponent As Integer 'Added By Saylee On 27-Jul-2020 For ALL27072020
    Dim mHourType As Integer = 1 'Added By Vikrant On 30-Nov-2020 For Spare Comp FLow
    Dim mLastAMPRef As LastMPDAMPRef 'Added by Ajay on 20-07-2023

#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mEnFrom = CType(Session("EnFrom"), From)
        mMachine = CType(Session("mMachine"), Machine)
        mCompMonitorServiceStatus = CType(Session("mCompMonitorServiceStatus"), CompMonitorServiceStatus)
        mPrevCompMonitorServiceStatus = CType(Session("mPrevCompMonitorServiceStatus"), CompMonitorServiceStatus)
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
        mIsSpareComponent = Session("mIsSpareComponent") 'Added By Saylee On 27-Jul-2020 For ALL27072020
    End Sub
    Private Sub SetSession()
        Session("mMachine") = mMachine
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
        Session("mPrevCompMonitorServiceStatus") = mPrevCompMonitorServiceStatus
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
        mCompMonitorServiceStatus = Nothing   'commented for a while
        Session.Remove("EnFrom")
        Session.Remove("mCompMonitorServiceStatus")
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
    Private Overloads Sub setFocus(ByVal cntrl As WebControls.WebControl)
        If cntrl.Visible = False Or cntrl.Enabled = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub Setobject()
        If Not IsDate(txtDoneOnDate.Text) Then
            mCompMonitorServiceStatus.DoneOn = System.DBNull.Value
        Else
            mCompMonitorServiceStatus.DoneOn = txtDoneOnDate.Text
        End If
        mCompMonitorServiceStatus.DoneWONo = Trim(txtWorkOrderNo.Text)
        mCompMonitorServiceStatus.DoneRemark = Trim(txtRemark.Text)
        mCompMonitorServiceStatus.RequiredManHours = Trim(txtActualManHours.Text)
        'Added By Saylee on 28-07-2008=======================
        'CNDC
        If Not IsDate(txtExtensionDate.Text) Then
            mCompMonitorServiceStatus.ExtensionDate = System.DBNull.Value
        Else
            mCompMonitorServiceStatus.ExtensionDate = txtExtensionDate.Text
        End If

        mCompMonitorServiceStatus.ApprovalRemark = Trim(txtApprovalRemark.Text)
        '====================================================
        With mCompMonitorServiceStatus
            .IsApplicable = chkApplicable.Checked   'Added By Vaishali on 19-Nov-2008
        End With

        mCompMonitorServiceStatus.DoneBy = Trim(txtDoneBy.Text) 'Added by Saylee On 23-Apr-2009

        ' Added By Utkarsh On 12-Jun-2012 FOR ALL08062012

        Dim LicenseNo As String = String.Empty
        Dim EmpName As String = String.Empty
        If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
            LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
            EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
        Else
            LicenseNo = Trim(txtLicenceNo.Text)
        End If
        mCompMonitorServiceStatus.LicenseNo = LicenseNo
        mCompMonitorServiceStatus.DoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID

        'End

        'Added by Saylee On 26-Apr-2012
        mCompMonitorServiceStatus.Place = txtPlace.Text.Trim
        '*********************************************

        mCompMonitorServiceStatus.SourceDoc = Trim(txtSourceDoc.Text)
        mCompMonitorServiceStatus.RevisionNo = Trim(txtRevisionNo.Text)
        mCompMonitorServiceStatus.BookNo = Trim(txtBookNo.Text)
        mCompMonitorServiceStatus.PageNo = Trim(txtPageNo.Text)

        'Added By Prashant On 27-Nov-2014
        If Not mFileAttach Is Nothing Then
            If mFileAttach.Size > 0 Then
                mCompMonitorServiceStatus.IsAttachmentAdded = True
            Else
                mCompMonitorServiceStatus.IsAttachmentAdded = False
            End If
            'Else
            '    .IsAttachmentAdded = False
        End If
        'End
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
    End Sub
    Public Sub SetGridObject()
        Dim txtCurrentValue, txtExtensionValue As TextBox
        For i As Integer = 0 To Me.dgDoneOnValue.Rows.Count - 1
            txtCurrentValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtCurrentValue"), TextBox)
            'Added By Saylee on 28-07-2008
            txtExtensionValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtExtensionValue"), TextBox)
            With mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
                If .Item(i).PeriodID = 2 Then
                    If Not Period.IsDate(txtCurrentValue.Text.Trim) Then
                        .Item(i).CurrentValue = ""
                    Else
                        .Item(i).CurrentValueFormatted = Trim(txtCurrentValue.Text)
                    End If
                Else
                    .Item(i).CurrentValue = Trim(txtCurrentValue.Text)
                End If

                'Added By Saylee on 28-07-2008
                'ExtensionValue
                .Item(i).ExtensionValue = Trim(txtExtensionValue.Text)
            End With
        Next i
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
    End Sub
    Public Function CustomeValidateGridValuesForOverDue() As String   'Code for OverDue 'Added by Saylee on 26-Mar-2019 for ALL26032019
        Dim txtCurrentValue, txtExtensionValue As TextBox
        Dim j As Int32


        Dim NextDueString As String = ""
        Dim DiffString As String = ""


        For j = 0 To Me.dgDoneOnValue.Rows.Count - 1
            txtCurrentValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtCurrentValue"), TextBox)
            'Added By Saylee on 28-07-2008
            txtExtensionValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtExtensionValue"), TextBox)
            With mPrevCompMonitorServiceStatus.CompMonitorServiceStatusPeriods ''mPrevCompMonitorServiceStatus object contains previous period values
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

        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus

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
        For i As Integer = 0 To Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count - 1
            With mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
                If .Item(i).PeriodID = 2 Then
                    If Not Period.IsDate(mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(i).CurrentValueFormatted) Then
                        .Item(i).CurrentValue = ""
                    Else
                        .Item(i).CurrentValueFormatted = Trim(mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(i).CurrentValueFormatted)
                    End If
                Else
                    .Item(i).CurrentValue = Trim(mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(i).CurrentValueFormatted)
                End If

                'Added By Saylee on 28-07-2008
                'ExtensionValue
                .Item(i).ExtensionValue = Trim(mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(i).ExtensionValueFormatted)
            End With
        Next i
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
    End Sub
    Private Sub SetLog()
        If CType(Session("FromLog"), Boolean) = True Then
            'Dim LogId As Guid = New Guid(Request.QueryString("LogId"))
            'Dim LogDate = Request.QueryString("LogDate")
            Dim LogId As Guid = New Guid(CType(Session("LogID"), String))
            Dim LogDate = CType(Session("mDoneOn"), String)
            'If DateDiff(DateInterval.Day, SmartDate.StringToDate(mPrevCompMonitorServiceStatus.AsOnDate), SmartDate.StringToDate(LogDate)) > 0 Then
            '    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DoneOnDate, SIMsgBox.Message_text.DoneOnDate, "Compliance record only upto " & CStr(mPrevCompMonitorServiceStatus.AsOnDate) & " can be entered through Comp Installation screen", MsgBoxStyle.OKOnly)
            '    msg1.ReplacePage = "wfComplyCompMonitorServiceStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
            '    msg1.Show()
            '    Exit Sub
            'End If

            '******************************************************************
            'Added by Saylee on 11-Jan-2017
            'ConsiderCompInstValue=True only if Compliance date is less than Comp Inst Date then consider Current vlaue 
            'If False,then Comp Current Values will be calculated

            Dim ConsiderCompInstValue As Boolean = False
            If txtDoneOnDate.Text <> "" And mCompStatus.InstalledOn.ToString <> "" Then
                If CDate(mCompMonitorServiceStatus.DoneOn) < CDate(mCompStatus.InstalledOn) Then
                    ConsiderCompInstValue = True
                End If
            End If
            '******************************************************************
            'Added By Vikrant on 27-Jan-2020 For getting Previous Assembly Status ID if Done on date is less than current Assembly Installation Date
            Dim tmpAssemblyStatusID As Guid = mPrevCompMonitorServiceStatus.AssemblyStatusID
            If txtDoneOnDate.Text <> "" And mAssemblyStatus.InstalledOn.ToString <> "" Then
                If CDate(txtDoneOnDate.Text) < CDate(mAssemblyStatus.InstalledOn) Then
                    Dim mAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtDoneOnDate.Text,
                            mMachine.ID.ToString, , , , , , , , , , True, , , mAssemblyStatus.AssemblyID.ToString, , , , , , , , , , , , , , , , MonitoringInspRequired:=False,
                            MonitoringModRequired:=False, MonitoringServiceRequired:=False, CompMonitoringInspRequired:=False,
                            CompMonitoringModRequired:=False, CompMonitoringServiceRequired:=False).Item(0), MachineInfo).AssemblyStatusList
                    If mAssemblyStatusList.Count > 0 Then
                        tmpAssemblyStatusID = mAssemblyStatusList(0).ID
                    End If
                End If
            End If
            'End
            Dim clnCompMonitorServiceStatus As CompMonitorServiceStatus = mCompMonitorServiceStatus.Clone
            If mEnFrom = From.NewRecord Then
                mCompMonitorServiceStatus = CompMonitorServiceStatus.NewComplyCompMonitorServiceStatus(Guid.NewGuid, mPrevCompMonitorServiceStatus.CompID, mPrevCompMonitorServiceStatus.AssemblyStatusID, LogDate, mCompStatus.Comp.PartID, mPrevCompMonitorServiceStatus.PartMonitorService, LogId, mPrevCompMonitorServiceStatus.CompStatusID, mPrevCompMonitorServiceStatus.DoneOn.ToString, mPrevCompMonitorServiceStatus.ID.ToString, , CType(Session("ConsiderAssemblyInstValue"), Boolean), ConsiderCompInstValue)
            Else
                mCompMonitorServiceStatus = CompMonitorServiceStatus.GetComplyCompMonitorServiceStatus(mPrevCompMonitorServiceStatus.ID, mPrevCompMonitorServiceStatus.AssemblyStatusID, mPrevCompMonitorServiceStatus.CompStatusID, LogDate, LogId, mHourType, CType(Session("ConsiderAssemblyInstValue"), Boolean), ConsiderCompInstValue)
            End If
            mCompMonitorServiceStatus.DoneWONo = clnCompMonitorServiceStatus.DoneWONo
            mCompMonitorServiceStatus.DoneRemark = clnCompMonitorServiceStatus.DoneRemark
            mCompMonitorServiceStatus.DoneOn = clnCompMonitorServiceStatus.DoneOn
            mCompMonitorServiceStatus.RequiredManHours = clnCompMonitorServiceStatus.RequiredManHours

            'Added by Saylee On 26-Apr-2012
            mCompMonitorServiceStatus.DoneByID = clnCompMonitorServiceStatus.DoneByID
            mCompMonitorServiceStatus.LicenseNo = clnCompMonitorServiceStatus.LicenseNo
            mCompMonitorServiceStatus.Place = clnCompMonitorServiceStatus.Place
            '*********************************************
            mCompMonitorServiceStatus.IsAttachmentAdded = clnCompMonitorServiceStatus.IsAttachmentAdded
            If Not mFileAttach Is Nothing Then
                mFileAttach.ReferenceID = mCompMonitorServiceStatus.ID
                Session("mFileAttach") = mFileAttach
            End If
            'commented By Deven on 09/05/2008 as this code is not required at all

            ''Dim i As Integer
            ''For i = 0 To clnCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count - 1
            ''    mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(i).DueOnValue = clnCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(i).DueOnValue
            ''Next
            'Added By Vikrant on 15-Apr-2021 to solve issue: Licence No not getting saved after select log
            For j As Integer = mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Count - 1 To 0 Step -1
                mCompMonitorServiceStatus.MaintenanceDoneByEmployees.RemoveAt(j)
            Next
            For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In clnCompMonitorServiceStatus.MaintenanceDoneByEmployees
                If Not mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.ID) Then
                    mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Add(mCompMonitorServiceStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
                Else
                    If Not mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.EmployeeID) Then
                        mCompMonitorServiceStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).EmployeeID = mMaintenanceDoneByEmployee.EmployeeID
                        mCompMonitorServiceStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).LicenceNo = mMaintenanceDoneByEmployee.LicenceNo
                        'mCompMonitorServiceStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).RequiredManHours = mMaintenanceDoneByEmployee.RequiredManHours
                        mCompMonitorServiceStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).EmployeeName = mMaintenanceDoneByEmployee.EmployeeName
                    End If
                End If
            Next
            'End

            Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
            clnCompMonitorServiceStatus = Nothing

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
        ', , , , , , , , , , True, True, , mAssemblyStatus.AssemblyID.ToString, , , , , , , mPrevCompMonitorServiceStatus.CompID.ToString, , , , , , , _
        ', , ).Item(0), MachineInfo).AssemblyStatusList

        'If mAssemblyStatusList.Count = 0 Then
        '    mAssemblyStatusList = CType(MachineList.GetMachineListWithRemoval(LogDate, mMachine.ID.ToString _
        '           , , , , , , , , , , True, True, , mAssemblyStatus.AssemblyID.ToString, , , , , , , mPrevCompMonitorServiceStatus.CompID.ToString, , , , , , , _
        '           , ).Item(0), MachineInfo).AssemblyStatusList
        'End If
        ''-----------------------------

        Dim mAssemblyStatusList As AssemblyStatusList
        Dim mMachineList As MachineList
        Dim LatestRemovedOn As SmartDate
        Dim AssemblyStatusID As Guid = Guid.Empty
        Dim CompStatusID As Guid = Guid.Empty

        mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(LogDate, mMachine.ID.ToString _
        , , , , , , , , , , True, True, , mAssemblyStatus.AssemblyID.ToString, , , , , , , mPrevCompMonitorServiceStatus.CompID.ToString, , , , , , ,
        , , SkipIsForInventoryAircarft:=True, MonitoringInspRequired:=False, MonitoringModRequired:=False,
            MonitoringServiceRequired:=False, CompMonitoringInspRequired:=False, CompMonitoringModRequired:=False,
            CompMonitoringServiceRequired:=False).Item(0), MachineInfo).AssemblyStatusList

        If mAssemblyStatusList.Count = 0 Then
            mMachineList = MachineList.GetMachineListWithRemoval(LogDate, mMachine.ID.ToString _
                   , , , , , , , , , , True, True, , mAssemblyStatus.AssemblyID.ToString, , , , , , , mPrevCompMonitorServiceStatus.CompID.ToString, , , , , , ,
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
        'Here instead of mPrevAssemblyMonitorServiceStatus.AssemblyStatusID pass mAssemblyStatusList(0).ID  
        'Here instead of mPrevAssemblyMonitorServiceStatus.CompStatusID pass mAssemblyStatusList(0).CompStatusList(0).ID

        'mCompMonitorServiceStatus = CompMonitorServiceStatus.NewComplyCompMonitorServiceStatus(Guid.NewGuid, mPrevCompMonitorServiceStatus.CompID, mPrevCompMonitorServiceStatus.AssemblyStatusID, LogDate, mCompStatus.Comp.PartID, mPrevCompMonitorServiceStatus.PartMonitorService, LogID, mPrevCompMonitorServiceStatus.CompStatusID, mPrevCompMonitorServiceStatus.DoneOn.ToString, mPrevCompMonitorServiceStatus.ID.ToString)
        mCompMonitorServiceStatus = CompMonitorServiceStatus.NewComplyCompMonitorServiceStatus(Guid.NewGuid, mPrevCompMonitorServiceStatus.CompID, AssemblyStatusID, LogDate, mCompStatus.Comp.PartID, mPrevCompMonitorServiceStatus.PartMonitorService, LogID, CompStatusID, mPrevCompMonitorServiceStatus.DoneOn.ToString, mPrevCompMonitorServiceStatus.ID.ToString)
        mCompMonitorServiceStatus.BeginEdit()
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
        SetTitle()
    End Sub
    Private Sub EditRecord(ByVal LogID As Guid, ByVal DoneOnDate As String, ByVal FromEntry As Boolean)
        REM:-FromEntry is used for avoiding object Dirty at form load when we r coming thru' Edit.
        If FromEntry = False Then
            mCompMonitorServiceStatus = CompMonitorServiceStatus.GetComplyCompMonitorServiceStatus(mPrevCompMonitorServiceStatus.ID, mPrevCompMonitorServiceStatus.AssemblyStatusID, mPrevCompMonitorServiceStatus.CompStatusID, DoneOnDate, LogID, mHourType)
        Else
            mCompMonitorServiceStatus = CompMonitorServiceStatus.GetComplyCompMonitorServiceStatusFromEntry(mPrevCompMonitorServiceStatus.ID, mPrevCompMonitorServiceStatus.AssemblyStatusID, mPrevCompMonitorServiceStatus.CompStatusID, DoneOnDate, mHourType)
        End If
        mCompMonitorServiceStatus.BeginEdit()
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
        SetTitle()
    End Sub
    Private Sub SetFromClone(ByVal clnCompMonitorServiceStatus As CompMonitorServiceStatus)
        mCompMonitorServiceStatus.DoneWONo = clnCompMonitorServiceStatus.DoneWONo
        mCompMonitorServiceStatus.DoneRemark = clnCompMonitorServiceStatus.DoneRemark

        'Added by Saylee On 26-Apr-2012
        mCompMonitorServiceStatus.DoneByID = clnCompMonitorServiceStatus.DoneByID
        mCompMonitorServiceStatus.LicenseNo = clnCompMonitorServiceStatus.LicenseNo
        mCompMonitorServiceStatus.Place = clnCompMonitorServiceStatus.Place
        '*********************************************
        mCompMonitorServiceStatus.IsAttachmentAdded = clnCompMonitorServiceStatus.IsAttachmentAdded
        If Not mFileAttach Is Nothing Then
            mFileAttach.ReferenceID = mCompMonitorServiceStatus.ID
            Session("mFileAttach") = mFileAttach
        End If
        'Added By Vikrant on 15-Apr-2021 to solve issue: Licence No not getting saved after select log
        For j As Integer = mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Count - 1 To 0 Step -1
            mCompMonitorServiceStatus.MaintenanceDoneByEmployees.RemoveAt(j)
        Next
        For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In clnCompMonitorServiceStatus.MaintenanceDoneByEmployees
            If Not mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.ID) Then
                mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Add(mCompMonitorServiceStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
            Else
                If Not mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.EmployeeID) Then
                    mCompMonitorServiceStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).EmployeeID = mMaintenanceDoneByEmployee.EmployeeID
                    mCompMonitorServiceStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).LicenceNo = mMaintenanceDoneByEmployee.LicenceNo
                    'mCompMonitorServiceStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).RequiredManHours = mMaintenanceDoneByEmployee.RequiredManHours
                    mCompMonitorServiceStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).EmployeeName = mMaintenanceDoneByEmployee.EmployeeName
                End If
            End If

        Next
        'End
        clnCompMonitorServiceStatus = Nothing
    End Sub
    Private Function SetObjectForRemComp() As Boolean
        Dim clnCompMonitorServiceStatus As CompMonitorServiceStatus
        clnCompMonitorServiceStatus = CType(mCompMonitorServiceStatus.Clone, CompMonitorServiceStatus)
        Setobject()
        SetGridObject()
        SetMachineMaintenanceObject() 'Added by Saylee on 9th-Oct-2009
        If mCompMonitorServiceStatus.IsValid Then
            If mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodRequired, MSGBox.Message_text.PeriodRequired, "You are trying to save Comp Service Status.Comp Service Status can not be saved without period units.", MsgBoxStyle.OkOnly, "")
                Return False
            End If
            'Added By Vikrant On 06-Aug-2013 For ALL01082013
            If Not mCompMonitorServiceStatus.DoneByID.Equals(Guid.Empty) AndAlso Not mCompMonitorServiceStatus.DoneOn.Equals(System.DBNull.Value) Then
                Dim title As String = "Save Alert !"
                Dim message As String = ""
                mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mCompMonitorServiceStatus.DoneByID.ToString, mCompMonitorServiceStatus.DoneOn)
                If (mEmployeeStatus(0).Information <> "") Then
                    message = mEmployeeStatus(0).Information
                    'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(message, False), True)
                    MSGBoxCtrl.Show(title, message, "", MsgBoxStyle.OkOnly, "")
                    Return False
                End If
            End If
            'End
            Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
            Return True
        Else
            Return False
        End If
    End Function
    Private Sub RemoveComp()
        ''Removal
        Dim mMachineID As Guid = Guid.Empty
        If Not mAssemblyStatus.IsSpareAssembly Then
            mMachineID = mMachine.ID
            Session("IsFromSpareWO") = "False"
        Else
            Session("IsFromSpareWO") = "True"
        End If

        Dim mtmpInstalledCompList As tmpInstalledCompList
        mtmpInstalledCompList = tmpInstalledCompList.GetInstalledCompList(mCompMonitorServiceStatus.DoneOn, mMachineID.ToString, mCompStatus.PartName, mCompStatus.SerialNo, mCompStatus.AssemblyID, IsSpareAssembly:=mAssemblyStatus.IsSpareAssembly)
        '' Session("mInstalledCompList") = mInstalledCompList

        Dim mRemCompStatus As CompStatus
        mRemCompStatus = CompStatus.NewRemovalCompStatus(mtmpInstalledCompList(mCompStatus.ID).CompStatusID, mCompMonitorServiceStatus.DoneOn.ToString, mtmpInstalledCompList(mCompStatus.ID).AssemblyStatusID, Guid.Empty.ToString)
        Session("From_Remove") = 1 'NewRemove
        Session("mRemCompStatus") = mRemCompStatus
        Dim mPrevCompStatus As CompStatus = CompStatus.GetCompStatus(mtmpInstalledCompList(mCompStatus.ID).CompStatusID, mtmpInstalledCompList(mCompStatus.ID).AssemblyStatusID, mtmpInstalledCompList(mCompStatus.ID).InstalledOnDBValue)
        Dim mRemAssemblyStatus As AssemblyStatus = AssemblyStatus.GetAssemblyStatus(mtmpInstalledCompList(mCompStatus.ID).AssemblyStatusID)
        Session("mRemAssemblyStatus") = mRemAssemblyStatus
        Session("mPrevCompStatus") = mPrevCompStatus
        Session("From_Remove") = 1
        Session("From_Inst") = 1
        Session("mtmpInstalledCompList") = mtmpInstalledCompList
        Response.Redirect("wfRemInstComp_AJAX.aspx?BackPage=" & Request.QueryString("GChildPage2"))

    End Sub
	'Added By Vikrant On 17-Sep-2020 For Mismatch Value Mail Send
	Private Sub SendMail(ByVal ServiceStatus As CompMonitorServiceStatus, ByVal DoneOnValue As String, ByVal DoneOnValueObj As String, Optional ByVal OnlyEdited As Boolean = False, Optional ByVal ToMailIDs As String = "saylee@bytzsoft.com")
		Dim str As New StringBuilder
		Try
			If OnlyEdited = False Then
				str.Append("Mismatch Details for <b>" & IIf(Session("From") = 1, "Edited and Saved", IIf(ServiceStatus.IsNew, "New", "New but Saved")) & "</b> record are as follows: ")
			Else
				''  str.Append("Mismatch Details for <b>" & IIf(Session("From") = 1, "Only Edited", IIf(ServiceStatus.IsNew, "New", "New but Saved")) & "</b> record are as follows: ")
			End If


			str.Append("<p><b>Assembly Details: </b> " & mAssemblyStatus.Assembly.ModelName & " " & mAssemblyStatus.Assembly.SerialNo & "</p>")
			str.Append("<p><b>Component Details: </b> " & mCompStatus.Comp.PartName & " " & mCompStatus.Comp.SerialNo & "</p>")
			str.Append("<p><b>Service ID: </b> " & ServiceStatus.ID.ToString & "</p>")
			str.Append("<p><b>Service Description: </b> " & ServiceStatus.PartMonitorService.Description & "</p>")
			str.Append("<p><b>Done On Date: </b> " & txtDoneOnDate.Text & "</p>")
			str.Append("<p><b>Done On Value: </b> " & DoneOnValue & "</p>")
			str.Append("<p><b>Done On Date(obj.): </b> " & ServiceStatus.DoneOnFormatted.ToString & "</p>")
			str.Append("<p><b>Done On Value(obj.): </b> " & DoneOnValueObj & "</p>")

			str.Append("<p><b>Saved By: </b> " & User.Identity.Name)

			SendMailFile.SendMailFile(Nothing, User.Identity.Name, "FAS: Component Service Done on Date Done on Value Mismatch Details", "", Info:=str.ToString, VendorEmailID:="", ToMailID:=ToMailIDs)
		Catch ex As Exception
			Dim Title As String = "Error Sending Mail"
			Dim Message As String = ex.InnerException.ToString
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(Title, Message, , False), True)
			Exit Sub
		End Try
	End Sub
	'End
	Private Function Save() As Boolean
        Dim clnCompMonitorServiceStatus As CompMonitorServiceStatus
        clnCompMonitorServiceStatus = CType(mCompMonitorServiceStatus.Clone, CompMonitorServiceStatus)
        Setobject()
        SetGridObject()
        SetMachineMaintenanceObject() 'Added by Saylee on 9th-Oct-2009
        If mCompMonitorServiceStatus.IsValid Then
            If mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodRequired, MSGBox.Message_text.PeriodRequired, "You are trying to save Comp Service Status.Comp Service Status can not be saved without period units.", MsgBoxStyle.OkOnly, "")
                Return False
            End If

            Try
                'Added By Vikrant On 06-Aug-2013 For ALL01082013
                If Not mCompMonitorServiceStatus.DoneByID.Equals(Guid.Empty) AndAlso Not mCompMonitorServiceStatus.DoneOn.Equals(System.DBNull.Value) Then
                    Dim title As String = "Save Alert !"
                    Dim message As String = ""
                    mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mCompMonitorServiceStatus.DoneByID.ToString, mCompMonitorServiceStatus.DoneOn)
                    If (mEmployeeStatus(0).Information <> "") Then
                        message = mEmployeeStatus(0).Information
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(title, message, , False), True)
                        Return False
                    End If
                End If
                'End
                'Added By Vikrant On 17-Sep-2020 For Mismatch Value Mail Send
                If txtDoneOnDate.Text <> "" AndAlso mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Contains(2, "") Then 'If date period conatins then only execute
                    Dim DoneOnValue As New StringBuilder
                    Dim DoneOnValueObj As New StringBuilder
                    For j As Integer = 0 To Me.dgDoneOnValue.Rows.Count - 1
                        DoneOnValue.Append(CType(Me.dgDoneOnValue.Rows(j).FindControl("txtCurrentValue"), TextBox).Text + ", ")
                        DoneOnValueObj.Append(mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(j).CurrentValueFormatted + ", ")
                        If mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(j).PeriodID = 2 Then
                            If Not txtDoneOnDate.Text.Equals(CType(Me.dgDoneOnValue.Rows(j).FindControl("txtCurrentValue"), TextBox).Text) Then
                                Session("IsSendMail") = "True"
                            End If
                        End If

                    Next j
                    If Session("IsSendMail") = "True" Then
                        Session.Remove("IsSendMail")
                        SendMail(mCompMonitorServiceStatus, DoneOnValue.ToString.Trim.TrimEnd(","), DoneOnValueObj.ToString.Trim.TrimEnd(","), ToMailIDs:="")
                    End If
                End If
                'End  
                mCompMonitorServiceStatus.ApplyEdit()
                mCompMonitorServiceStatus = CType(mCompMonitorServiceStatus.Save(), CompMonitorServiceStatus)
                Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
                'Revise Activity
                If Not Session("mPrevCompMonitorServiceStatusForRevise") Is Nothing Then
                    Dim mPrevCompMonitorServiceStatusForRevise As CompMonitorServiceStatus
                    mPrevCompMonitorServiceStatusForRevise = Session("mPrevCompMonitorServiceStatusForRevise")
                    mPrevCompMonitorServiceStatusForRevise.IsApplicable = False
                    mPrevCompMonitorServiceStatusForRevise.Save()
                    Session.Remove("mPrevCompMonitorServiceStatusForRevise")
                End If
                'End
                SaveAttachment() 'Added By Prashant On 27-Nov-2014
                SaveMachineMaintenance()  'Added by Saylee on 9th-Oct-2009
                mCompInfo = Session("mCompInfo")
                ''MarkLog(Util.Action.Save, "ComplyCompMonitorServiceStatus", mCompInfo + "   " + ComplyCompMonitorServiceInfo, Util.ErrorType.NoError, mCompMonitorServiceStatus.ID)

                ''Commented By Utkarsh On 27-Jul-2011 For All19072011
                '  MarkLog(Util.Action.Save, "ComponentServiceMonitor", mCompInfo, Util.ErrorType.NoError, mCompMonitorServiceStatus.ID)
                'End

                Return True
            Catch ex As SqlException
                Session("mCompMonitorServiceStatus") = clnCompMonitorServiceStatus
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
                clnCompMonitorServiceStatus = Nothing
                'Added By Utkarsh On 26-Jul-2011 For All19072011
                Dim mDoneOnValues As New System.Text.StringBuilder
                For i As Integer = 0 To mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count - 1
                    mDoneOnValues.Append(mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(i).DoneOnValueFormatted + ",")
                Next
                ' MaintDetail = "Reg No. : " & mMachineMaintenance.RegNo & " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeName & " Done On Date : " + mCompMonitorServiceStatus.DoneOnFormatted + " Done On Value : " + mDoneOnValues.ToString

                If mCompStatus.IsSpareComp = False Then
                    MaintDetail = "Reg No. : " + mMachine.RegNo + " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeName + " Done On Values : " + mDoneOnValues.ToString.TrimEnd(",")
                Else
                    MaintDetail = "Stock Component :  Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeName + " Done On Values : " + mDoneOnValues.ToString.TrimEnd(",")
                End If
                MarkLog(Util.Action.Save, "ComponentServiceMonitor", MaintDetail, Util.ErrorType.NoError, mCompMonitorServiceStatus.ID, EventLogID)
                'End
            End Try
        Else
            Return False
        End If
    End Function
    Private Sub SetTitle()
        If IsDate(mCompMonitorServiceStatus.DoneOn) Then
            ''   calDoneOn.Text = CDate(mCompMonitorServiceStatus.DoneOn)
            'calDoneOn.TitleText = CDate(mCompMonitorServiceStatus.DoneOn)
            'calDoneOn.DateToday = CDate(mCompMonitorServiceStatus.DoneOn)
            'calDoneOn.SelectedDate = CDate(mCompMonitorServiceStatus.DoneOn)
            'ElseIf IsDate(mCompStatus.AsOnDate) Then
            '    calDoneOn.Text = CDate(mCompStatus.AsOnDate)
            'calDoneOn.TitleText = CDate(mCompStatus.AsOnDate)
            'calDoneOn.DateToday = CDate(mCompStatus.AsOnDate)
            'calDoneOn.SelectedDate = CDate(mCompStatus.AsOnDate)
        End If
        Dim CompInfo As String = "[Part: " & mCompStatus.PartName & " Serial No. : " & mCompStatus.Comp.SerialNo & " ]"
        'If mCompMonitorServiceStatus.IsNew Then
        '    lblTitle.Text = "Comply Component Service Status " & CompInfo & " [New]"
        'Else
        '    lblTitle.Text = "Comply Component Service Status" & CompInfo
        'End If

        Dim ServiceMPDTitle As String = ""
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            ServiceMPDTitle = "Comply Maintenance Event"
            lblMonitorModType.InnerText = "Task Type"
        Else
            ServiceMPDTitle = "Comply Component Service Status"
            lblMonitorModType.InnerText = "Service Type"
        End If

        If mCompMonitorServiceStatus.IsNew Then
            lblTitle.Text = IIf(mIsSpareComponent = 0, "", IIf(mCompStatus.IsSpareComp, "Stock ", "Removed ")) + ServiceMPDTitle & " " & CompInfo & " [New]" 'mIsSpareAssembly Added By Saylee On 27-Jul-2020 For ALL27072020
        Else
            lblTitle.Text = IIf(mIsSpareComponent = 0, "", IIf(mCompStatus.IsSpareComp, "Stock ", "Removed ")) + ServiceMPDTitle & " " & CompInfo 'mIsSpareAssembly Added By Saylee On 27-Jul-2020 For ALL27072020
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "RemoveSLLComp" Then
                        Session("sender") = ""
                        Session("mLogList") = Nothing
                        SetLog()
                        DataFieldBind()
                        ControlVisibilityForDatePeriod()
                        If SetObjectForRemComp() Then
                            Session("FromLog") = False
                            'Commented by Saylee on 20-Jul-2018 for ALL20072018, as Compliance will be saved only if Component is removed & saved
                            'If Not Save() Then
                            '    Exit Sub
                            'End If
                            RemoveComp()
                        Else
                            Dim Str As String
                            For i As Integer = 0 To mCompMonitorServiceStatus.GetBrokenRulesCollection.Count - 1
                                Str = Str + mCompMonitorServiceStatus.GetBrokenRulesCollection(i).Description + "<BR>"
                            Next
                            upnlValidationSummary.Update()
                        End If
                    End If
                    'Revise Activity
                    If MSGBoxCtrl.Sender = "ReviseActivity" Then
                        MarkLog(Util.Action.[New], "Part Service", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                        Dim mPartMonitorService As PartMonitorService
                        Dim ID As Guid = Guid.NewGuid
                        mPartMonitorService = PartMonitorService.NewPartMonitorService(mCompMonitorServiceStatus.PartMonitorService, mHourType)
                        Session("mPartMonitorService") = mPartMonitorService
                        'RemoveSession()
                        mPartMonitorService.BeginEdit()
                        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
                        Session("mPrevCompMonitorServiceStatusForRevise") = mCompMonitorServiceStatus
                        Dim GChildPage2, GChildPage4, GChildPage5, GChildPage6 As String 'Dim GChildPageTmp As String = Request.QueryString("GChildPage4")
                        'ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenInspMasterWindow", "OpenInspMasterWindow('" + GChildPageTmp + "');", True)
                        GChildPage2 = Trim(Request.QueryString("GChildPage2"))
                        GChildPage4 = Trim(Request.QueryString("GChildPage4"))
                        GChildPage5 = Trim(Request.QueryString("GChildPage5"))
                        GChildPage6 = Trim(Request.QueryString("GChildPage6"))
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSeriviceMasterWindow", "OpenSeriviceMasterWindow('" + GChildPage2 + "','" + GChildPage4 + "','" + GChildPage5 + "','" + GChildPage6 + "');", True)
                    ElseIf (MSGBoxCtrl.Sender = "OverDue" Or MSGBoxCtrl.Sender = "ComplyOnSameDate") Then 'Added by Saylee on 26-Mar-2019 for ALL26032019
                        'ComplyOnSameDate Added By Prashant 19-Nov-2019 Alert if user is complying on same date 
                        If (mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeID = 2 Or mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeID = 6 Or mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeID = 5 Or mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeName.StartsWith("NAV")) And Session("EnFrom") = 0 And (mCompStatus.IsSpareComp = False) And (mCompStatus.IsRemoved = False) Then
                            'Added by Saylee on 20-Jul-2018 for ALL20072018, Only for OC and NAV
                            Dim ExtraMsg As String = IIf(mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeID = 6 Or mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeName.StartsWith("NAV"), "Click Yes to Remove Component or click No to just Comply the Service.", "")
                            '*************************************************************
                            Dim ServiceType As String = IIf(mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeID = 2, "SLL", IIf(mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeID = 6, "OC. i.e. On Condition (No Limit) ", IIf(mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeID = 5, "Expiry", "Navigation Component ")))
                            MSGBoxCtrl.Show("Alert!", "It’s a " + ServiceType + " service so Component will be removed. Do you want to continue?", ExtraMsg, MsgBoxStyle.YesNo, "RemoveSLLComp")
                        Else
                            If Save() Then
                                If MSGBoxCtrl.Sender = "OverDue" Then
                                    MarkLog(Util.Action.Save, "ComponentServiceMonitor", User.Identity.Name & " saved OverDue record : " & Session("OverDueString") & " " & Session("DueString"), Util.ErrorType.HandledError, mCompMonitorServiceStatus.ID, EventLogID)
                                ElseIf MSGBoxCtrl.Sender = "ComplyOnSameDate" Then
                                    MarkLog(Util.Action.Save, "ComponentServiceMonitor", User.Identity.Name & " Comply On Same Date : ", Util.ErrorType.HandledError, mCompMonitorServiceStatus.ID, EventLogID)
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

                                'Response.Redirect("wfComplyCompMonitorServiceStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
                                'Added by Saylee on 9th-Jan-2008======================================
                                If Request.QueryString("GChildPage4") <> "" Then
                                    Response.Redirect(Request.QueryString("GChildPage4") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3"))
                                ElseIf Request.QueryString("GChildPage2") <> "" Then
                                    Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
                                End If
                                '=====================================================================
                            End If
                        End If
                    End If
                    'End
                Case MsgBoxResult.No
                    If MSGBoxCtrl.Sender = "RemoveSLLComp" Then
                        Session("sender") = ""

                        'Compliance will be saved only for OC. Comp & NAV comp
                        If (mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeID = 6 Or mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeName.StartsWith("NAV")) And Session("EnFrom") = 0 Then
                            If Not Save() Then
                                Exit Sub
                            End If
                        End If

                        Session("FromLog") = False
                        'Response.Redirect("wfComplyCompMonitorServiceStatus.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
                        If Request.QueryString("GChildPage4") <> "" Then
                            Response.Redirect(Request.QueryString("GChildPage4") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3"))
                        ElseIf Request.QueryString("GChildPage2") <> "" Then
                            Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
                        End If
                    End If
                    'Revise Activity
                    If MSGBoxCtrl.Sender = "ReviseActivity" Then
                        MarkLog(Util.Action.Close, "ComponentServiceMonitor", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                        RemoveSession()
                        Session.Remove("FromLog")
                        Session.Remove("IsBackFromCompliance") 'Added By Vikrant On 03-Jun-2016 For ALL03062016
                        'Added by Saylee on 9th-Jan-2008======================================
                        If Request.QueryString("GChildPage4") <> "" Then
                            Response.Redirect(Request.QueryString("GChildPage4") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3"))
                        ElseIf Request.QueryString("GChildPage2") <> "" Then
                            Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
                        End If
                    End If
                    'End
                Case MsgBoxResult.Cancel

                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    Session("sender") = ""
                    'DataFieldBind()
                    'Response.Redirect("wfComplyCompMonitorServiceStatus.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
                Case MsgBoxResult.Ok And Session("sender") = "Authorization"  'Code Added
                    Session("sender") = ""
                    'DataFieldBind()
                    'Response.Redirect("wfComplyCompMonitorServiceStatus.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
            End Select
        ElseIf Result1 = -1 Then
            Session("sender") = ""
        ElseIf Result1 = 0 Then   'Code Added
            Session("sender") = ""
        End If
    End Sub
    Private Sub ControlVisibility()
        btnPrint.Enabled = Not mCompMonitorServiceStatus.IsNew
        dgCurrentValue.Columns(3).Visible = Not mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 3
        dgCurrentValue.Columns(4).Visible = Not mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 3

        'Added By Saylee on 28-08-2008
        dgDoneOnValue.Columns(2).Visible = Not mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 3
        '==========================
        dgDoneOnValue.Columns(3).Visible = Not mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 3
        'Added By Utkarsh ON 26-Jun-2013 FOR ALL26062013-1
        dgDoneOnValue.Columns(4).Visible = (mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID <> 3) AndAlso (mCompStatus.AssemblyTypeID <> 1 AndAlso mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID <> 3) AndAlso mIsSpareComponent <> 1 'mIsSpareAssembly Added By Saylee On 27-Jul-2020 For ALL27072020
        dgDoneOnValue.Columns(5).Visible = Not mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 3
        'End

        ' 'Commented by Saylee on 28-June-2018 for ALL28062018 for star air, to add DoneOn Date for OC Service'
        'If mCompMonitorServiceStatus.PartMonitorService.ReadOnlyFrequencyColumn Then
        '    txtDoneOnDate.Enabled = False
        '    chkApplicable.Enabled = False
        'End If
        '******************************************************************************
        btnRevise.Enabled = (mCompMonitorServiceStatus.IsApplicable And Not mCompMonitorServiceStatus.IsNew And Not ((mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 1 Or mCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 4) And mCompMonitorServiceStatus.DoneOnFormatted.ToString <> ""))  'Revise Activity
        btnSelectLog.Visible = (mIsSpareComponent <> 1) ' Added By Saylee On 27-Jul-2020 For 
        lnkPrintLogBookEntry.Visible = (mIsSpareComponent <> 1)
        ControlVisibilityForAttachment() 'Added By Prashant On 27-Nov-2014
    End Sub
    Private Sub CopyFromClone(ByVal ClonedCompMonitorServiceStatus As CompMonitorServiceStatus)
        mCompMonitorServiceStatus.DoneWONo = ClonedCompMonitorServiceStatus.DoneWONo
        mCompMonitorServiceStatus.DoneRemark = ClonedCompMonitorServiceStatus.DoneRemark

        'Added by Saylee On 26-Apr-2012
        mCompMonitorServiceStatus.DoneByID = ClonedCompMonitorServiceStatus.DoneByID
        mCompMonitorServiceStatus.LicenseNo = ClonedCompMonitorServiceStatus.LicenseNo
        mCompMonitorServiceStatus.Place = ClonedCompMonitorServiceStatus.Place
        '*********************************************

        'MLNo
        For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In ClonedCompMonitorServiceStatus.MaintenanceDoneByEmployees
            mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Add(mCompMonitorServiceStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
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

        If Session("EnFrom") = 0 And Not (mMachineMaintenanceList.Contains(mCompMonitorServiceStatus.ID, MaintenanceType.ComponentService, "")) Then
            mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(mMachineID, MaintenanceType.ComponentService, txtDoneOnDate.Text, mCompMonitorServiceStatus.ID, Guid.Empty, 0, 0, mAssemblyStatusID)
        Else
            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mCompMonitorServiceStatus.ID, MaintenanceType.ComponentService)
        End If

        With mMachineMaintenance
            ''.MachineID = mAssemblyStatus.MachineID
            ''.MaintenanceActivityTypeID =5
            .MaintenanceID = mCompMonitorServiceStatus.ID 'TransactionID
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
        Session("mComplyMachineMaintenance") = mMachineMaintenance
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
        ''  End If
    End Sub
    Private Sub ControlVisibilityForGridBeforeBinding()
        dgCurrentValue.Columns(3).Visible = True
        dgCurrentValue.Columns(4).Visible = True

        'Added By Saylee on 28-08-2008
        dgDoneOnValue.Columns(2).Visible = True
        '==========================
        dgDoneOnValue.Columns(3).Visible = True
        'Added By Utkarsh ON 26-Jun-2013 FOR ALL26062013-1
        dgDoneOnValue.Columns(4).Visible = True
        dgDoneOnValue.Columns(5).Visible = True
    End Sub
    'Added By Prashant On 27-Nov-2014
    Private Sub NewRecordAttachment()
        mFileAttach = FileAttach.NewAttachment(Guid.Empty, mCompMonitorServiceStatus.ID)
        Session("mFileAttach") = mFileAttach
    End Sub
    Private Sub ControlVisibilityForAttachment()
        If mCompMonitorServiceStatus.IsAttachmentAdded Then
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Private Sub GetAttachment()
        If mCompMonitorServiceStatus.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mCompMonitorServiceStatus.ID)
            Session("mFileAttach") = mFileAttach
        End If

        'If mFileAttach Is Nothing Then
        '    NewRecordAttachment()
        'End If
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
                If (Not mCompMonitorServiceStatus.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mCompMonitorServiceStatus.ID)
                End If
                IsAttachmentDeleted = False
                Session("IsAttachmentDeleted") = IsAttachmentDeleted
            End If
        End If
    End Sub
    Private Sub ViewImage()
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString

        If mCompMonitorServiceStatus.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mCompMonitorServiceStatus.ID)
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
    'End
    'MLNo
    Public Sub SetLicenceCount()
        If mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Count > 1 Then
            lblLicenceCount.Text = "and " + (mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Count - 1).ToString + " more"
        End If
        lblLicenceCount.DataBind()
        'lblAllLicenceNos.DataBind()
    End Sub
    Private Sub BindLicenceNo()
        If mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Count > 0 Then
            txtLicenceNo.Text = mCompMonitorServiceStatus.MaintenanceDoneByEmployees(0).LicenceNo + " [" + mCompMonitorServiceStatus.MaintenanceDoneByEmployees(0).EmployeeName + "]"
        Else
            txtLicenceNo.Text = String.Empty
        End If
    End Sub
    'End
    Private Sub ControlVisibilityForDatePeriod()
        Dim txtDnOnDate As TextBox
        For j As Integer = 0 To Me.dgDoneOnValue.Rows.Count - 1
            txtDnOnDate = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtCurrentValue"), TextBox)
            With mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
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
        dgCurrentValue.DataSource = mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
        dgDoneOnValue.DataSource = mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
        'Added On 28,May,2007 By Girish
        txtDoneOnDate.Text = mCompMonitorServiceStatus.DoneOnFormatted.ToString
        txtExtensionDate.Text = mCompMonitorServiceStatus.ExtensionDateFormatted.ToString

        'Added by Saylee on 9th-Oct-2009
        mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList()
        Session("mMachineMaintenanceList") = mMachineMaintenanceList

        If Val(mCompMonitorServiceStatus.PartMonitorService.RequiredManHours) > 0 Then
            lblEstdManHours.Text = "(Estd. Man Hours : " + mCompMonitorServiceStatus.PartMonitorService.RequiredManHours + ")"
        End If

        BindLicenceNo() 'MLNo
        'Added by Ajay 21-01-2023
        mLastAMPRef = LastMPDAMPRef.GetLastMPDAMPRefForMachine(mMachine.ID)
        Session("mLastAMPRef") = mLastAMPRef
        If (mLastAMPRef.AMPNo <> "") Then lblAMPNo.Text = "AMP No.: " + mLastAMPRef.AMPNo + ",Rev No.: " + mLastAMPRef.RevNo + ",Dated: " + mLastAMPRef.FromDateFormatted
        DataBind()

        'Added By Vikrant On 30-Nov-2020 For Spare Comp FLow
        If mIsSpareComponent <> 1 Then
            mHourType = mMachine.HourType
        End If
        'End
    End Sub
    Private Sub DataBindGrid()
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
        dgCurrentValue.DataSource = mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
        dgDoneOnValue.DataSource = mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
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
            If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Or (txtLicenceNo.Text.Trim.IndexOf("[") <0 And txtLicenceNo.Text.Trim.IndexOf("]") <0) Then
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
        Setobject()
        SetGridObject()
        Dim str As String = ""
        If Not mCompMonitorServiceStatus.IsValid Then
            For i As Integer = 0 To mCompMonitorServiceStatus.GetBrokenRulesCollection.Count - 1
                str = str + mCompMonitorServiceStatus.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(dgDoneOnValue.Rows.Count - 1)
            If Not mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Item(i).IsValid Then
                For x As Integer = 0 To mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
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
            If Not mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Item(i).IsValid Then
                For x As Integer = 0 To mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
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
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added By Utkarsh On 27-Jul-2011 For All19072011
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            txtDoneOnDate.Focus()
            Session("mLogList") = Nothing
            SetLog()
            DataFieldBind()
            'GetAttachment()
            ControlVisibility()
            ControlVisibilityForDatePeriod()
            SetTitle()
            'MLNo
            SetLicenceCount()
            UserNameForLicenceList = User.Identity.Name
            Session("UserNameForLicenceList") = UserNameForLicenceList
            'End

            ''If Not mCompMonitorServiceStatus.IsNew And Session("From") = 1 Then

            ''    'Added By Saylee On 9-FEB-2021 For Mismatch Value Mail Send of Controls
            ''    If txtDoneOnDate.Text <> "" AndAlso mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Contains(2, "") Then 'If date period conatins then only execute
            ''        Dim DoneOnValue As New StringBuilder
            ''        Dim DoneOnValueObj As New StringBuilder
            ''        Dim ControlDoneOnValue As String = String.Empty
            ''        For j As Integer = 0 To Me.dgDoneOnValue.Rows.Count - 1
            ''            DoneOnValue.Append(CType(Me.dgDoneOnValue.Rows(j).FindControl("txtCurrentValue"), TextBox).Text + ", ")
            ''            DoneOnValueObj.Append(mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(j).CurrentValueFormatted + ", ")
            ''            If mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(j).PeriodID = 2 Then
            ''                ControlDoneOnValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtCurrentValue"), TextBox).Text
            ''                If Not txtDoneOnDate.Text.ToString.Equals(ControlDoneOnValue) Then
            ''                    Session("IsSendMail") = "True"
            ''                End If
            ''            End If
            ''        Next j
            ''        If Session("IsSendMail") = "True" Then
            ''            Session.Remove("IsSendMail")
            ''            SendMail(mCompMonitorServiceStatus, DoneOnValue.ToString.Trim.TrimEnd(","), DoneOnValueObj.ToString.Trim.TrimEnd(","), True, ToMailIDs:="deven@bytzsoft.com,saylee@bytzsoft.com")
            ''        End If
            ''    End If
            ''    'End
            ''End If
        End If
    End Sub
    Private Sub dgDoneOnValue_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)
        Select Case e.CommandName
            Case "CurrentValue"
                Dim txtCurrentValue As TextBox
                For i As Integer = 0 To dgDoneOnValue.Rows.Count - 1
                    txtCurrentValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtCurrentValue"), TextBox)
                    With mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
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
                'Added By Saylee on 28-07-2008
            Case "ExtensionValue"
                Dim txtExtensionValue As TextBox
                For i As Integer = 0 To mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count - 1
                    txtExtensionValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtExtensionValue"), TextBox)

                    With mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
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
            With mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
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
        ControlVisibilityForGridBeforeBinding()
        DataBindGrid()
        ControlVisibility()
        'upnlDoneOnValueGrid.Update()
        upnlCurrentValueGrid.Update()
    End Sub
    Protected Sub txtExtensionValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtExtensionValue As TextBox
        For i As Integer = 0 To mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count - 1
            txtExtensionValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtExtensionValue"), TextBox)

            With mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods
                .Item(i).ExtensionValue = Trim(txtExtensionValue.Text)
            End With
        Next
        ControlVisibilityForGridBeforeBinding()
        DataBindGrid()
        ControlVisibility()
        'upnlDoneOnValueGrid.Update()
        upnlCurrentValueGrid.Update()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mCompMonitorServiceStatus.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mCompMonitorServiceStatus.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mCompMonitorServiceStatus.ID)
        End If
        Session("mFileAttach") = mFileAttach
    End Sub

    Private Sub hdnBtnSelectLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnSelectLog.Click
        SetLog()
        SetGridFromObject()
        DataBindGrid()
        ControlVisibility()
        SetTitle()
        upnlCurrentValueGrid.Update()
        upnlDoneOnValueGrid.Update()
    End Sub

    Private Sub txtDoneOnDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDoneOnDate.TextChanged
        'If mCompMonitorServiceStatus.DoneOn.ToString <> "" And calDoneOn.Text <> "" Then
        '    If DateDiff(DateInterval.Day, SmartDate.StringToDate(mCompMonitorServiceStatus.DoneOn.ToString), SmartDate.StringToDate(calDoneOn.Text)) <> 0 Then
        '        Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DoneOnDate, SIMsgBox.Message_text.DoneOnDate, "Compliance record only upto " & mCompMonitorServiceStatus.DoneOn.ToString & " can be entered through Comp Installation screen", MsgBoxStyle.OKOnly)
        '        msg1.ReplacePage = "wfComplyCompMonitorServiceStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
        '        msg1.Show()
        '        Exit Sub
        '    End If
        'End If
        If IsPostBack Then
            Setobject()
            '******************************************************************
            'Added by Saylee on 11-Jan-2017
            'ConsiderCompInstValue=True only if Compliance date is less than Comp Inst Date then consider Current vlaue 
            'If False,then Comp Current Values will be calculated

            Dim ConsiderCompInstValue As Boolean = False
            If txtDoneOnDate.Text <> "" And mCompStatus.InstalledOn.ToString <> "" Then
                If CDate(mCompMonitorServiceStatus.DoneOn) < CDate(mCompStatus.InstalledOn) Then
                    ConsiderCompInstValue = True
                End If
            End If
            '******************************************************************

            'Added By Vikrant on 27-Jan-2020 For getting Previous Assembly Status Values if Done on date is less than current Assembly Installation Date
            Dim tmpAssemblyStatusID As Guid = mPrevCompMonitorServiceStatus.AssemblyStatusID
            If mIsSpareComponent <> 1 Then
                If txtDoneOnDate.Text <> "" And mAssemblyStatus.InstalledOn.ToString <> "" Then
                    If CDate(txtDoneOnDate.Text) < CDate(mAssemblyStatus.InstalledOn) Then
                        Dim mAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtDoneOnDate.Text,
                                mMachine.ID.ToString, , , , , , , , , , True, , , mAssemblyStatus.AssemblyID.ToString, , , , , , , , , , , , , , , ,
                                 MonitoringInspRequired:=False, MonitoringModRequired:=False, MonitoringServiceRequired:=False,
                                 CompMonitoringInspRequired:=False, CompMonitoringModRequired:=False, CompMonitoringServiceRequired:=False).Item(0), MachineInfo).AssemblyStatusList
                        If mAssemblyStatusList.Count > 0 Then
                            tmpAssemblyStatusID = mAssemblyStatusList(0).ID
                        End If
                    End If
                End If
            End If
            'End

            Dim clnCompMonitorServiceStatus As CompMonitorServiceStatus = mCompMonitorServiceStatus.Clone
            If mEnFrom = From.NewRecord Then 'New Record
                mCompMonitorServiceStatus = CompMonitorServiceStatus.NewComplyCompMonitorServiceStatus(Guid.NewGuid, mPrevCompMonitorServiceStatus.CompID, mPrevCompMonitorServiceStatus.AssemblyStatusID, txtDoneOnDate.Text, mCompStatus.Comp.PartID, mPrevCompMonitorServiceStatus.PartMonitorService, Guid.Empty, mPrevCompMonitorServiceStatus.CompStatusID, mPrevCompMonitorServiceStatus.DoneOn.ToString, mPrevCompMonitorServiceStatus.ID.ToString, , , ConsiderCompInstValue)
            Else
                mCompMonitorServiceStatus = CompMonitorServiceStatus.GetComplyCompMonitorServiceStatus(mPrevCompMonitorServiceStatus.ID, mPrevCompMonitorServiceStatus.AssemblyStatusID, mPrevCompMonitorServiceStatus.CompStatusID, txtDoneOnDate.Text, Guid.Empty, mHourType, , ConsiderCompInstValue, IsForSpareComp:=mIsSpareComponent)
            End If
            SetFromClone(clnCompMonitorServiceStatus)
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
        Setobject()
        SetGridObject()
        Session("mFromType") = 3
        Session("mMachineId") = mAssemblyStatus.MachineID.ToString
        Session("mAssemblyStatusId") = mCompMonitorServiceStatus.AssemblyStatusID.ToString
        Session("mAssemblyID") = mAssemblyStatus.AssemblyID.ToString
        Session("mDoneOn") = CStr(IIf(txtDoneOnDate.Text = "", Today.Date.ToShortDateString, txtDoneOnDate.Text))

        'Added by Saylee on 14-Mar-2016 for ALL11032016
        If mAssemblyStatus.InstalledOn.ToString <> "" Then
            If CDate(mCompMonitorServiceStatus.DoneOn) <= CDate(mAssemblyStatus.InstalledOn) Then 'if Compliance date is same or less than Assembly Inst. Date
                Dim mFirstLogDetailAfterAssemblyInstallation As FirstLogDetailAfterAssemblyInstallation = FirstLogDetailAfterAssemblyInstallation.GetFirstLogDetailAfterAssemblyInstallation(mAssemblyStatus)
                Session("mFirstLogDetailAfterAssemblyInstallation") = mFirstLogDetailAfterAssemblyInstallation
            End If
        End If

        '*************************************************
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSelectLogWindow", "OpenSelectLogWindow()", True)
        'Response.Redirect("wfSelectLog.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3") & "&GChildPage4=" & Request.QueryString("GChildPage4") & "&BackPage6=wfComplyCompMonitorServiceStatus_Ajax.aspx" & "&FromType=3&DoneOn=" & txtDoneOnDate.Text & "&MachineId=" & mAssemblyStatus.MachineID.ToString & "&AssemblyStatusID=" & mCompMonitorServiceStatus.AssemblyStatusID.ToString & "&AssemblyID=" & mAssemblyStatus.AssemblyID.ToString)
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If (Not User.IsInRole("ComponentInstallationNew") And mCompStatus.IsNew) Or (Not User.IsInRole("ComponentInstallationEdit") And Not mCompStatus.IsNew) Then
            'Added By Utkarsh On 27-Jul-2011 For All19072011
            ' MaintDetail = "Reg No. : " + mMachine.RegNo + " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeName
            If mCompStatus.IsSpareComp = False Then
                MaintDetail = "Reg No. : " + mMachine.RegNo + " Assembly Info : " & mAssemblyStatus.ModelName + " " + mAssemblyStatus.Assembly.SerialNo & " Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeName
            Else
                MaintDetail = "Stock Component :  Part Info : " & mCompStatus.Comp.PartName + " " + mCompStatus.Comp.Description + " " + mCompStatus.Comp.SerialNo & " Monitor Info : " & mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeName
            End If
            MarkLog(Util.Action.Save, "ComponentServiceMonitor", User.Identity.Name & " is not Authorized User to save " & MaintDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            'End
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If
        If Not IsValid Then
            upnlValidationSummary.Update()
            Exit Sub
        End If

        'Code for OverDue 'Added by Saylee on 26-Mar-2019 for ALL26032019
        If Not mPrevCompMonitorServiceStatus.PartMonitorService.MonitorTypeID = 3 Then  'No Frequency record not be checked for OverDue
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
        If mPrevCompMonitorServiceStatus.DoneOn.ToString <> "" Then
            If (CDate(txtDoneOnDate.Text) <= CDate(mPrevCompMonitorServiceStatus.DoneOn) And Session("EnFrom") <> 1) Then
                MSGBoxCtrl.Show("Alert!!!", "Current compliance date is less than or equal to last compliance date ", "Do you want to continue?", MsgBoxStyle.YesNo, "ComplyOnSameDate")
                Exit Sub
            End If
            'If CDate(txtDoneOnDate.Text) > CDate(mPrevCompMonitorServiceStatus.DoneOn) Then
            '    MSGBoxCtrl.show("Alert!!!", "Current compliance date is greater than last compliance date ", "Do you want to continue?", MsgBoxStyle.YesNo, "ComplyOnSameDate")
            '    Exit Sub
            'End If
        End If
        If (CDate(txtDoneOnDate.Text) > CDate(Today.Date) And Session("EnFrom") <> 1) Then
            MSGBoxCtrl.Show("Alert!!!", "Current compliance date is greater than today date  ", "Do you want to continue?", MsgBoxStyle.YesNo, "ComplyOnSameDate")
            Exit Sub
        End If
        'End of Added By Prashant 19-Nov-2019 Alert if user is complying on same date 

        'mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeID = 6 Added by Prashant 16-Apr-2018 as per star air Requirement
        If (mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeID = 2 Or mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeID = 6 Or mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeID = 5 Or mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeName.StartsWith("NAV")) And Session("EnFrom") = 0 And (mCompStatus.IsSpareComp = False) And (mCompStatus.IsRemoved = False) Then
            'Added by Saylee on 20-Jul-2018 for ALL20072018, Only for OC and NAV
            Dim ExtraMsg As String = IIf(mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeID = 6 Or mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeName.StartsWith("NAV"), "Click Yes to Remove Component or click No to just Comply the Service.", "")
            '*************************************************************
            Dim ServiceType As String = IIf(mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeID = 2, "SLL", IIf(mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeID = 6, "OC. i.e. On Condition (No Limit) ", IIf(mCompMonitorServiceStatus.PartMonitorService.PartMonitorServiceTypeID = 5, "Expiry", "Navigation Component ")))
            MSGBoxCtrl.Show("Alert!", "It’s a " + ServiceType + " service so Component will be removed. Do you want to continue?", ExtraMsg, MsgBoxStyle.YesNo, "RemoveSLLComp")
        Else
            If IsValid Then


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

                    'Response.Redirect("wfComplyCompMonitorServiceStatus.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2"))
                    'Added by Saylee on 9th-Jan-2008======================================
                    If Request.QueryString("GChildPage4") <> "" Then
                        Response.Redirect(Request.QueryString("GChildPage4") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3"))
                    ElseIf Request.QueryString("GChildPage2") <> "" Then
                        Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
                    End If
                    '=====================================================================
                End If
            Else
                upnlValidationSummary.Update()
            End If
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Changed By Utkarsh On 27-Jul-2011 For All19072011
        MarkLog(Util.Action.Close, "ComponentServiceMonitor", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'End
        RemoveSession()
        'Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
        'Response.Redirect(Request.QueryString("GChildPage4") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3"))
        Session.Remove("FromLog")
        Session.Remove("IsBackFromCompliance") 'Added By Vikrant On 03-Jun-2016 For ALL03062016
        'Added by Saylee on 9th-Jan-2008======================================
        If Request.QueryString("GChildPage4") <> "" Then
            Response.Redirect(Request.QueryString("GChildPage4") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&GChildPage3=" & Request.QueryString("GChildPage3"))
        ElseIf Request.QueryString("GChildPage2") <> "" Then
            Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
        End If
        '=====================================================================
    End Sub
    'Added by Prashant On 27-Nov-2014
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        mCompMonitorServiceStatus.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        upnlFileupload.Update()
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte

        If mCompMonitorServiceStatus.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mCompMonitorServiceStatus.ID)
            Session("mFileAttach") = mFileAttach
        End If

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        IsAttachmentDeleted = True
        mCompMonitorServiceStatus.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        ViewImage()
    End Sub
    'End
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    'MLNo
    Private Sub imgbtnEmployeeLicence_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgbtnEmployeeLicence.Click
        If IsValid Then
            Setobject()
            Session("mMaintenanceID") = mCompMonitorServiceStatus.ID
            Session("MaintenanceDoneOnDate") = mCompMonitorServiceStatus.DoneOn.ToString
            mMaintenanceDoneByEmployees = mCompMonitorServiceStatus.MaintenanceDoneByEmployees
            Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AddEmployeeLicNo", "AddEmployeeLicNo();", True)
        Else
            upnlValidationSummary.Update()
        End If

    End Sub
    Private Sub hdnBtnMaintDoneBy_Click(sender As Object, e As System.EventArgs) Handles hdnBtnMaintDoneBy.Click
        For i As Integer = 0 To mMaintenanceDoneByEmployees.Count - 1
            Dim ID As Guid = mMaintenanceDoneByEmployees(i).ID
            If Not mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Add(mMaintenanceDoneByEmployees(i))
            ElseIf mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mCompMonitorServiceStatus.MaintenanceDoneByEmployees(ID).LicenceNo = mMaintenanceDoneByEmployees(i).LicenceNo
                mCompMonitorServiceStatus.MaintenanceDoneByEmployees(ID).RequiredManHours = mMaintenanceDoneByEmployees(i).RequiredManHours
                mCompMonitorServiceStatus.MaintenanceDoneByEmployees(ID).EmployeeID = mMaintenanceDoneByEmployees(i).EmployeeID
                mCompMonitorServiceStatus.MaintenanceDoneByEmployees(ID).EmployeeName = mMaintenanceDoneByEmployees(i).EmployeeName
            End If
        Next

        For j As Integer = 0 To mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Count - 1
            If Not mMaintenanceDoneByEmployees.Contains(mCompMonitorServiceStatus.MaintenanceDoneByEmployees(j).ID) Then
                mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Remove(mCompMonitorServiceStatus.MaintenanceDoneByEmployees(j).ID, "")
            End If
        Next
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
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
            If mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mCompMonitorServiceStatus.MaintenanceDoneByEmployees(0).EmployeeID = DoneByID
                mCompMonitorServiceStatus.MaintenanceDoneByEmployees(0).LicenceNo = LicenseNo
                If Not mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Count > 1 Then 'If Condition added by Vikrant On 15-Apr-2021 to solve issue:Hours getting added for multiple licence no and if first licence no changed
                    mCompMonitorServiceStatus.MaintenanceDoneByEmployees(0).RequiredManHours = txtActualManHours.Text
                End If

                mCompMonitorServiceStatus.MaintenanceDoneByEmployees(0).EmployeeName = EmpName
            Else
                mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Add(mCompMonitorServiceStatus.ID, MaintenanceType.ComponentService, DoneByID, LicenseNo, txtActualManHours.Text, EmpName)
            End If

        Else
            If mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mCompMonitorServiceStatus.MaintenanceDoneByEmployees.RemoveAt(0)
            End If
        End If
        Session("mCompMonitorServiceStatus") = mCompMonitorServiceStatus
        BindLicenceNo()
        SetLicenceCount()
        txtActualManHours.DataBind()
        upnlMonitoringStatusDetails.Update()
    End Sub
    Protected Sub txtActualManHours_TextChanged(sender As Object, e As System.EventArgs)
        If mCompMonitorServiceStatus.MaintenanceDoneByEmployees.Count > 0 Then
            mCompMonitorServiceStatus.MaintenanceDoneByEmployees(0).RequiredManHours = txtActualManHours.Text
            upnlMonitoringStatusDetails.Update()
        End If
    End Sub
    'End
    'Revise Activity
    Private Sub btnRevise_Click(sender As Object, e As System.EventArgs) Handles btnRevise.Click
        MSGBoxCtrl.Show("Alert!", "You are about to Revise Part Activity.After revision of Part activity this Status will become Not Applicable.", "Do you want to continue?", MsgBoxStyle.YesNo, "ReviseActivity")
    End Sub
    'End
    Private Sub lnkPrintLogBookEntry_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPrintLogBookEntry.Click  'Added By Saylee On 18-May-2021 ALL07052021
        Dim RptCommonHistory As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mLogEntryFormat As New LogEntryFormat
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsReportHistoryList
        Dim mCompanyDetail As New CompanyDetail

        RptCommonHistory = New crptLogEntryFormat

        mLogEntryFormat = LogEntryFormat.GetHistoryList(mCompMonitorServiceStatus.DoneOn, mCompMonitorServiceStatus.DoneOn, "", mAssemblyStatus.AssemblyTypeName,
                                                        mAssemblyStatus.ModelName, mAssemblyStatus.Assembly.SerialNo, "", "", "", "",
                                                        mAssemblyStatus.MachineID.ToString, False, True, IsRemoved:=False, IsInstalled:=True,
                                                        IsComplied:=False, AssemblyID:=mAssemblyStatus.AssemblyID.ToString, IsLogNo:=True,
                                                        IsLogPageNo:=False, IsFlightNo:=False, IsMELRequired:=False, IsMaintenanceActivityRequired:=False,
                                                        AssemblyTypeID:=mAssemblyStatus.AssemblyTypeID, CompStatusID:=mCompStatus.ID.ToString, ShowService:=True, ShowDir:=False, ShowInsp:=False, CompMonitorServiceStatusID:=mCompMonitorServiceStatus.ID.ToString)
        If mLogEntryFormat.Count = 0 Then
            Exit Sub
        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
           mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
           mCompanyDetail.WebSite, "LOG BOOK ENTRY", "", mCompMonitorServiceStatus.DoneOnFormatted, Machine.GetMachine(mAssemblyStatus.MachineID).RegNo,
           mAssemblyStatus.ModelName + "-" + mAssemblyStatus.Assembly.SerialNo, IIf(mAssemblyStatus.AssemblyTypeName.Equals("Airframe"), "AIRCRAFT", mAssemblyStatus.AssemblyTypeName.ToUpper),
           AppSettings("Product Version"), AppSettings("SINote"),
           "AVERAGE FUEL CONSUMPTION________LTR./HR & AVERAGE OIL CONSUMPTION________LTR./HR SINCE LAST SMI DONE.  BOTH THE FIGURES ARE BELOW THE ALERT VALUE.",
           "True", mCompMonitorServiceStatus.DoneOnFormatted, "", AppSettings("Logo"))

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
    'Created By :- Pallavi , Date -10/08/2006
#Region " Report Variable "
    Dim mCompanyDetail As New CompanyDetail
    Dim Rpt As CrystalDecisions.CrystalReports.Engine.ReportClass
#End Region

#Region " Event "
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If (Not User.IsInRole("ComponentInstallationPrint")) Then
            'Commented By Utkarsh On 27-Jul-2011 For All19072011
            '  MarkLog(Util.Action.Print, "ComplyCompMonitorServiceStatus", "Not Authorized User", Util.ErrorType.HandledError, Guid.Empty)
            'End
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        Rpt = New crDetComplyCompMonitorStatus
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Current Value Grid
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 5
        RHCount = Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Service Type",
                  txtPartMonitorServiceTypeName.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                  dgCurrentValue.Columns.Item(1).HeaderText, dgCurrentValue.Columns.Item(2).HeaderText,
                    , dgCurrentValue.Columns.Item(3).HeaderText, , dgCurrentValue.Columns.Item(4).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Service Type",
                            txtPartMonitorServiceTypeName.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                                  "", "", , "", , ""))
        End If
        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Monitor Type",
                 txtMonitorType.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).PeriodUnitName, String),
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).FrequencyValueFormatted, String), ,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).ElapsedValueFormatted, String), ,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "ATA Chapter",
                            txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                            "", "", , "", , ""))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "ATA Chapter",
                             txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).PeriodUnitName, String),
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).FrequencyValueFormatted, String), ,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).ElapsedValueFormatted, String), ,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "ATA Chapter",
                            txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                            "", "", , "", , ""))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Reference",
                             txtReference.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).PeriodUnitName, String),
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).FrequencyValueFormatted, String), ,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).ElapsedValueFormatted, String), ,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Reference",
                                txtReference.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                                "", "", , "", , ""))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Description",
                                   txtDescription.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).PeriodUnitName, String),
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).FrequencyValueFormatted, String), ,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).ElapsedValueFormatted, String), ,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Description",
                                    txtDescription.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                            "", "", , "", , ""))
                End If
            ElseIf I = 4 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "",
                                    "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).PeriodUnitName, String),
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).FrequencyValueFormatted, String), ,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).ElapsedValueFormatted, String), ,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).RemainingValueFormatted, String), , lblNote.Text))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "",
                                        "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                            "", "", , "", , "", , lblNote.Text))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "",
                                         "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values",
                                      CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).PeriodUnitName, String),
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).FrequencyValue, String), ,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).ElapsedValueFormatted, String), ,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(I).RemainingValueFormatted, String), , lblNote.Text))
            End If
        Next

        'For Done On Value Grid
        Dim TotalCount1 As Integer
        Dim LHCount1 As Integer
        Dim RHCount1 As Integer
        LHCount1 = 7
        RHCount1 = Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count
        If LHCount1 > RHCount1 Then
            TotalCount1 = LHCount1
        Else
            TotalCount1 = RHCount1
        End If

        Dim ServiceMPDTitle As String = ""
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            ServiceMPDTitle = "Maintenance Event"
        Else
            ServiceMPDTitle = "Service"
        End If

        Dim temp1 As Integer
        temp1 = 0
        If temp1 < RHCount1 Then
            ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Done On",
            txtDoneOnDate.Text, , , , , , , , , , , , , , , , , "Component Values at Compliance of " + ServiceMPDTitle,
              dgDoneOnValue.Columns.Item(0).HeaderText, dgDoneOnValue.Columns.Item(1).HeaderText,
             , dgDoneOnValue.Columns.Item(2).HeaderText, , dgDoneOnValue.Columns.Item(3).HeaderText, RHData3:=dgDoneOnValue.Columns.Item(5).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Done On",
                            txtDoneOnDate.Text, , , , , , , , , , , , , , , , , "Component Values at Compliance of " + ServiceMPDTitle,
                                  "", "", , "", , "", ""))
        End If

        Dim m As Integer
        For m = 0 To TotalCount1 - 1
            If m = 0 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Work Order No.",
                    txtWorkOrderNo.Text, , , , , , , , , , , , , , , , , "Component Values at Compliance of " + ServiceMPDTitle,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).PeriodUnitName, String),
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).CurrentValueFormatted, String), ,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DueOnValueFormatted, String), RHData3:=CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Work Order No.",
                            txtWorkOrderNo.Text, , , , , , , , , , , , , , , , , "Component Values at Compliance of " + ServiceMPDTitle,
                                "", "", , "", , "", "", , ))
                End If
            ElseIf m = 1 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Remark",
                     txtRemark.Text, , , , , , , , , , , , , , , , , "Component Values at Compliance of " + ServiceMPDTitle,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).PeriodUnitName, String),
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).CurrentValueFormatted, String), ,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DueOnValueFormatted, String), RHData3:=CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Remark",
                            txtRemark.Text, , , , , , , , , , , , , , , , , "Component Values at Compliance of " + ServiceMPDTitle,
                                "", "", , "", , "", "", , ))
                End If

            ElseIf m = 2 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Actual Man Hours",
                     txtActualManHours.Text, , , , , , , , , , , , , , , , , "Component Values at Compliance of " + ServiceMPDTitle,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).PeriodUnitName, String),
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).CurrentValueFormatted, String), ,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DueOnValueFormatted, String), RHData3:=CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Actual Man Hours",
                            txtActualManHours.Text, , , , , , , , , , , , , , , , , "Component Values at Compliance of " + ServiceMPDTitle,
                                "", "", , "", , "", "", , ))
                End If
            ElseIf m = 3 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Done By Agency",
                     txtDoneBy.Text, , , , , , , , , , , , , , , , , "Component Values at Compliance of " + ServiceMPDTitle,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).PeriodUnitName, String),
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).CurrentValueFormatted, String), ,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DueOnValueFormatted, String), RHData3:=CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Done By Agency",
                            txtDoneBy.Text, , , , , , , , , , , , , , , , , "Component Values at Compliance of " + ServiceMPDTitle,
                                "", "", , "", , "", "", , ))
                End If
            ElseIf m = 4 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "License No.",
                     mCompMonitorServiceStatus.AllLicenceNosWithEmpName, , , , , , , , , , , , , , , , , "Component Values at Compliance of " + ServiceMPDTitle,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).PeriodUnitName, String),
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).CurrentValueFormatted, String), ,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DueOnValueFormatted, String), RHData3:=CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "License No.",
                            mCompMonitorServiceStatus.AllLicenceNosWithEmpName, , , , , , , , , , , , , , , , , "Component Values at Compliance of " + ServiceMPDTitle,
                                "", "", , "", , "", "", , ))
                End If
            ElseIf m = 5 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Place",
                     txtPlace.Text, , , , , , , , , , , , , , , , , "Component Values at Compliance of " + ServiceMPDTitle,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).PeriodUnitName, String),
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).CurrentValueFormatted, String), ,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DueOnValueFormatted, String), CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Place",
                            txtPlace.Text, , , , , , , , , , , , , , , , , "Component Values at Compliance of " + ServiceMPDTitle,
                                "", "", , "", , "", "", , ))
                End If
            ElseIf m = 6 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "",
                    "", , , , , , , , , , , , , , , , , "Component Values at Compliance of " + ServiceMPDTitle,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).PeriodUnitName, String),
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).CurrentValueFormatted, String), ,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DueOnValueFormatted, String),
                            , lblNote1.Text))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "",
                    "", , , , , , , , , , , , , , , , , "Component Values at Compliance of " + ServiceMPDTitle,
                           "", "", , "", , "", "", lblNote1.Text, ))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "",
                                   "", , , , , , , , , , , , , , , , , "Component Values at Compliance of " + ServiceMPDTitle,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).PeriodUnitName, String),
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).CurrentValueFormatted, String), ,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(m).DueOnValueFormatted, String),
                                           , lblNote1.Text))
            End If
        Next

        '***********************************************************************************************************************
        'For Document Details
        Dim TotalCount2 As Integer
        Dim LHCount2 As Integer
        Dim RHCount2 As Integer
        LHCount2 = 3
        RHCount2 = Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods.Count
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
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).PeriodUnitName, String),
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).CurrentValueFormatted, String), "Approval Remark",
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).ExtensionValueFormatted, String), txtApprovalRemark.Text,
                    CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).DueOnValueFormatted, String), , ))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Page No.",
                        txtPageNo.Text, , , , , , , , , , , , , , , , , "Extension Details",
                        "", txtApprovalRemark.Text, , "", , "", ""))
                End If
            ElseIf n = 1 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Book No.",
                    txtBookNo.Text, , , , , , , , , , , , , , , , , "Extension Details",
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).PeriodUnitName, String),
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).CurrentValueFormatted, String), ,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).ExtensionValueFormatted, String), ,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).DueOnValueFormatted, String), RHData3:=CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).AssemblyDueOnValueFormattedByAirFrame, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Book No.",
                        txtBookNo.Text, , , , , , , , , , , , , , , , , "Extension Details",
                    "", "", , "", , "", ""))
                End If
            ElseIf n = 2 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Source Doc ",
                    txtSourceDoc.Text, , , , , , , , , , , , , , , , , "Extension Details",
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).PeriodUnitName, String),
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).CurrentValueFormatted, String), ,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).ExtensionValueFormatted, String), ,
                CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).DueOnValueFormatted, String), RHData3:=CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).AssemblyDueOnValueFormattedByAirFrame, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Source Doc ",
                        txtSourceDoc.Text, , , , , , , , , , , , , , , , , "Extension Details",
                    "", "", , "", , "", ""))
                End If

            Else
                ReportDetails.Add(New rptStatus(, 2, "Document Details", "",
                                 "", , , , , , , , , , , , , , , , , "Component Values at Compliance of " + ServiceMPDTitle,
                  CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).PeriodUnitName, String),
                  CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).CurrentValueFormatted, String), ,
                  CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).ExtensionValueFormatted, String), ,
                  CType(Me.mCompMonitorServiceStatus.CompMonitorServiceStatusPeriods(n).DueOnValueFormatted, String),
                  , lblNote1.Text))
            End If
        Next
        '***********************************************************************************************************************

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
        mCompanyDetail.WebSite, "Comply Component Service Status Detail Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        'MarkLog(Util.Action.Print, "ComplyCompMonitorServiceStatus", mCompInfo + " -> " + "Comply Component Monitor Service Status Detail Report", Util.ErrorType.NoError, mCompMonitorServiceStatus.ID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
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