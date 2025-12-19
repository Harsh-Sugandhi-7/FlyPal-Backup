'AJAX Conversion By Vikrant On 15-Apr-2015
Imports System.Linq
Imports System.Collections.Generic
Imports System.Text 'Added By Vikrant On 17-Sep-2020 For Mismatch Value Mail Send
Public Class wfComplyAssemblyMonitorInspStatus_Ajax
    Inherits System.Web.UI.Page

    'Added By Utkarsh On 07-Feb-2012 FOR Link Maintenance

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
    'End

#Region " Variable Declaration "
    Public mAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
    Public mAssemblyStatus As AssemblyStatus
    Public mMachine As Machine
    Public mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
    Dim Flag As Int16
    Public mAssemblyInfo As String 'Code Added Jan,25,2007
    Dim LogID As String

    Public mBoardInfo As AircraftInformationBoard.BoardInfo 'Added by Saylee on 22-May-2009

    Public mMachineMaintenance As MachineMaintenance 'Added by Saylee on 9th-Oct-2009
    Public mMachineMaintenanceList As MachineMaintenanceList 'Added by Saylee on 9th-Oct-2009

    'Added by Vikrant on 28-July-2011
    Dim EventLogID As Guid
    Public mInspectionDetail As String
    Public mMonitorInfo As String
    Public mMonitorType As String
    Public mMonitorDesc As String
    Public mModel As String
    Public mSerialNo As String

    'Added By Utkarsh ON 07-Feb-2012 FOR Link Maintenance
    Public mLinkMaintenanceList As LinkMaintenanceList
    Public mLinkMaintenance As LinkMaintenance
    Public mMultiComplianceList As New MultiComplianceList
    Public mAssemblyMonitorServiceStatusForLM As AssemblyMonitorServiceStatus
    Public mAssemblyMonitorInspStatusForLM As AssemblyMonitorInspStatus
    Public mAssemblyMonitorModStatusForLM As AssemblyMonitorModStatus
    Public mLinkMaintenanceMonitorStatus As LinkMaintenaceMonitorStatus
    Public PeriodValues(,) As String
    'End 
    Public mEmployeeStatus As EmployeeStatus 'Added By Vikrant On 06-Aug-2013 For ALL01082013
    'Added By Vikrant On 25-Nov-2014
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
    Public mIsSpareAssembly As Integer 'Added By Vikrant On 27-Jul-2020 For ALL27072020
#End Region

#Region " Business Methods "
    Private Sub GetSession()
        mPrevAssemblyMonitorInspStatus = Session("mPrevAssemblyMonitorInspStatus")
        mAssemblyMonitorInspStatus = CType(Session("mAssemblyMonitorInspStatus"), AssemblyMonitorInspStatus)
        mAssemblyStatus = CType(Session("mAssemblyStatus"), AssemblyStatus)
        mMachine = CType(Session("mMachine"), Machine)
        LogID = CType(Session("LogID"), String)

        mBoardInfo = Session("mBoardInfo") 'Added by Saylee on 22-May-2009
        mAssemblyInfo = Session("mAssemblyInfo") 'Added by Saylee on 04-Aug-2009

        mMachineMaintenance = CType(Session("mMachineMaintenance"), MachineMaintenance) 'Added by Saylee on 9th-Oct-2009
        mMachineMaintenanceList = CType(Session("mMachineMaintenanceList"), MachineMaintenanceList) 'Added by Saylee on 9th-Oct-2009

        mMultiComplianceList = Session("mMultiComplianceList") 'Added By Utkarsh ON 15-Mar-2012 FOR Link Maintenance
        'Added By Vikrant On 25-Nov-2014
        mFileAttach = Session("mFileAttach")
        IsAttachmentDeleted = Session("IsAttachmentDeleted")
        'End
        'MLNo
        mMaintenanceDoneByEmployees = Session("mMaintenanceDoneByEmployees")
        UserNameForLicenceList = Session("UserNameForLicenceList")
        'End
        mIsSpareAssembly = Session("mIsSpareAssembly") 'Added By Vikrant On 27-Jul-2020 For ALL27072020
    End Sub
    Private Sub SetSession()
        Session("mPrevAssemblyMonitorInspStatus") = mPrevAssemblyMonitorInspStatus
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
        Session("mAssemblyStatus") = mAssemblyStatus
        Session("mMachine") = mMachine

        Session("mBoardInfo") = mBoardInfo 'Added by Saylee on 22-May-2009
        Session("mAssemblyInfo") = mAssemblyInfo 'Added by Saylee on 04-Aug-2009

        Session("mMachineMaintenance") = mMachineMaintenance            'Added by Saylee on 9th-Oct-2009
        Session("mMachineMaintenanceList") = mMachineMaintenanceList    'Added by Saylee on 9th-Oct-2009
        'Added By Vikrant On 25-Nov-2014
        Session("mFileAttach") = mFileAttach
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
        'End
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAssemblyMonitorInspStatus")
        Session.Remove("mMachineMaintenance")       'Added by Saylee on 9th-Oct-2009
        Session.Remove("mMachineMaintenanceList")   'Added by Saylee on 9th-Oct-2009
        Session.Remove("mMultiComplianceList") 'Added By Utkarsh ON 15-Mar-2012 FOR Link Maintenance
        'Added By Vikrant On 25-Nov-2014
        Session.Remove("mFileAttach")
        Session.Remove("IsAttachmentDeleted")
        'End
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
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub SetObject()
        With mAssemblyMonitorInspStatus
            If Not IsDate(txtDoneOnDate.Text) Then
                .DoneOn = System.DBNull.Value
            Else
                .DoneOn = txtDoneOnDate.Text
            End If
            .DoneWONo = Trim(txtWorkOrderNo.Text)
            .DoneRemark = Trim(txtRemark.Text)
            .RequiredManHours = Trim(txtRequiredManHours.Text)

            'Added By Saylee on 28-07-2008=======================
            'CNDC
            If Not IsDate(txtExtensionDate.Text) Then
                .ExtensionDate = System.DBNull.Value
            Else
                .ExtensionDate = txtExtensionDate.Text
            End If

            .ApprovalRemark = txtApprovalRemark.Text
            '====================================================
            .IsApplicable = chkApplicable.Checked   'Added By Vaishali on 19-Nov-2008

            'Added By Utkarsh On 11-Jun-2012 FOR ALL08062012
            LicenseNo = String.Empty
            Dim EmpName As String = String.Empty
            If (txtLicenceNo.Text.Trim.IndexOf("[") > 0 And txtLicenceNo.Text.Trim.IndexOf("]") > 0) Then
                LicenseNo = txtLicenceNo.Text.Substring(0, txtLicenceNo.Text.Trim.IndexOf("[")).Trim
                EmpName = Mid(txtLicenceNo.Text.Trim, txtLicenceNo.Text.Trim.IndexOf("[") + 2, txtLicenceNo.Text.Trim.IndexOf("]") - txtLicenceNo.Text.Trim.IndexOf("[") - 1).Trim
            Else
                LicenseNo = Trim(txtLicenceNo.Text)
            End If
            .LicenseNo = LicenseNo
            .DoneByID = EmployeeByLicenseNoName.GetEmployeeByLicenseNoName(LicenseNo, EmpName).EmpID
            'Session("LicenseNo") = LicenseNo
            'End
            .Place = txtPlace.Text.Trim  'Added by Shweta on 26th-Apr-2012
            .SourceDoc = Trim(txtSourceDoc.Text)
            .RevisionNo = Trim(txtRevisionNo.Text)
            .BookNo = Trim(txtBookNo.Text)
            .PageNo = Trim(txtPageNo.Text)
            
            'Added By Vikrant On 25-Nov-2014
            If Not mFileAttach Is Nothing Then
                If mFileAttach.Size > 0 Then
                    .IsAttachmentAdded = True
                Else
                    .IsAttachmentAdded = False
                End If
                'Else
                '    .IsAttachmentAdded = False
            End If
            'End
        End With
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
    End Sub
    Public Sub SetGridObject()
        Dim txtCurrentValue, txtExtensionValue As TextBox
        Dim j As Int32
        'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
        ReDim PeriodValues(dgDoneOnValue.Rows.Count - 1, 1)  'Actual Size   (dgDoneOnValue.Items.Count , 2)
        'End
        For j = 0 To Me.dgDoneOnValue.Rows.Count - 1
            txtCurrentValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtCurrentValue"), TextBox)
            'Added By Saylee on 28-07-2008
            txtExtensionValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtExtensionValue"), TextBox)
            With mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
                If .Item(j).PeriodID = 2 Then
                    If Not Period.IsDate(txtCurrentValue.Text) Then
                        .Item(j).CurrentValue = ""
                    Else
                        .Item(j).CurrentValueFormatted = Trim(txtCurrentValue.Text)
                        'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
                        PeriodValues(j, 0) = .Item(j).PeriodUnitID  'To Check same Period
                        PeriodValues(j, 1) = Trim(txtCurrentValue.Text) 'Period Value 
                        'End
                    End If
                Else
                    .Item(j).CurrentValue = Trim(txtCurrentValue.Text)
                    'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
                    PeriodValues(j, 0) = .Item(j).PeriodUnitID 'To Check same Period
                    PeriodValues(j, 1) = Trim(txtCurrentValue.Text) 'Period Value 
                    'End
                End If

                'Added By Saylee on 28-07-2008
                'ExtensionValue
                .Item(j).ExtensionValue = Trim(txtExtensionValue.Text)
            End With
        Next j
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
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
            With mPrevAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods ''mPrevAssemblyMonitorInspStatus object contains previous period values
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

        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus

        'Dont change this format as its used in Over Due Report to show these values on report
        If OverDueString <> "" Then
            OverDueString = "Over Due: " + OverDueString
            Session("OverDueString") = OverDueString
            Return "Actual Due: " + NextDueString + "<br>" + "Cross Due: " + DiffString
        Else
            Return ""
        End If

    End Function
    Public Sub SetGridObjectFromObject()
        Dim j As Int32
        'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
        ReDim PeriodValues(mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Count - 1, 1)  'Actual Size   (dgDoneOnValue.Items.Count , 2)
        'End
        For j = 0 To mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Count - 1

            With mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
                If .Item(j).PeriodID = 2 Then
                    If Not Period.IsDate(mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted) Then
                        .Item(j).CurrentValue = ""
                    Else
                        .Item(j).CurrentValueFormatted = Trim(mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted)
                        'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
                        PeriodValues(j, 0) = .Item(j).PeriodUnitID  'To Check same Period
                        PeriodValues(j, 1) = Trim(mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted) 'Period Value 
                        'End
                    End If
                Else
                    .Item(j).CurrentValue = Trim(mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted)
                    'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
                    PeriodValues(j, 0) = .Item(j).PeriodUnitID 'To Check same Period
                    PeriodValues(j, 1) = Trim(mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted) 'Period Value 
                    'End
                End If

                'Added By Saylee on 28-07-2008
                'ExtensionValue
                .Item(j).ExtensionValue = Trim(mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted)
            End With
        Next j
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
    End Sub
    Private Sub SetLog()
        If Val(Request.QueryString("Type")) = -1 Then
            Dim LogId As Guid = New Guid(Request.QueryString("LogId"))
            Dim LogDate = Request.QueryString("LogDate")
            'If DateDiff(DateInterval.Day, SmartDate.StringToDate(mPrevAssemblyMonitorInspStatus.AsOnDate), SmartDate.StringToDate(LogDate)) > 0 Then
            '    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DoneOnDate, SIMsgBox.Message_text.DoneOnDate, "Compliance record only upto " & CStr(mPrevAssemblyMonitorInspStatus.AsOnDate) & " can be entered through Assembly Installation screen", MsgBoxStyle.OKOnly)
            '    msg1.ReplacePage = "wfComplyAssemblyMonitorInspStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
            '    msg1.Show()
            '    Exit Sub
            'End If
            Dim clnAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = mAssemblyMonitorInspStatus.Clone
            If Session("From") = 0 Then
                mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewComplyAssemblyMonitorInspStatus(Guid.NewGuid, mPrevAssemblyMonitorInspStatus.AssemblyID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, LogDate, mAssemblyStatus.Assembly.ModelID, mPrevAssemblyMonitorInspStatus.ModelMonitorInsp, LogId, mPrevAssemblyMonitorInspStatus.DoneOn.ToString, mMachine.HourType, CType(Session("ConsiderAssemblyInstValue"), Boolean))
            End If
            mAssemblyMonitorInspStatus.DoneWONo = clnAssemblyMonitorInspStatus.DoneWONo
            mAssemblyMonitorInspStatus.DoneRemark = clnAssemblyMonitorInspStatus.DoneRemark
            mAssemblyMonitorInspStatus.DoneOn = clnAssemblyMonitorInspStatus.DoneOn
            mAssemblyMonitorInspStatus.RequiredManHours = clnAssemblyMonitorInspStatus.RequiredManHours
            mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods = clnAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
            mAssemblyMonitorInspStatus.DoneByID = clnAssemblyMonitorInspStatus.DoneByID
            mAssemblyMonitorInspStatus.LicenseNo = clnAssemblyMonitorInspStatus.LicenseNo
            mAssemblyMonitorInspStatus.Place = clnAssemblyMonitorInspStatus.Place
            'Added By Vikrant On 25-Nov-2014
            mAssemblyMonitorInspStatus.IsAttachmentAdded = clnAssemblyMonitorInspStatus.IsAttachmentAdded
            'Added By Vikrant on 15-Apr-2021 to solve issue: Licence No not getting saved after select log
            For j As Integer = mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Count - 1 To 0 Step -1
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.RemoveAt(j)
            Next
            For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In clnAssemblyMonitorInspStatus.MaintenanceDoneByEmployees
                If Not mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.ID) Then
                    mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Add(mAssemblyMonitorInspStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
                Else
                    If Not mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.EmployeeID) Then
                        mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).EmployeeID = mMaintenanceDoneByEmployee.EmployeeID
                        mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).LicenceNo = mMaintenanceDoneByEmployee.LicenceNo
                        mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).RequiredManHours = mMaintenanceDoneByEmployee.RequiredManHours
                        mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(mMaintenanceDoneByEmployee.ID).EmployeeName = mMaintenanceDoneByEmployee.EmployeeName
                    End If
                End If
            Next
            'End
            If Not mFileAttach Is Nothing Then
                mFileAttach.ReferenceID = mAssemblyMonitorInspStatus.ID
                Session("mFileAttach") = mFileAttach
            End If
            'End
            Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
            clnAssemblyMonitorInspStatus = Nothing

            'Added by Saylee on 9th-Oct-2009
            Dim mLog As Log
            mLog = Log.GetLog(New Guid(LogId.ToString))
            Session("mLog") = mLog
            '===================================
        End If
    End Sub
    Private Sub NewRecord(ByVal LogID As Guid, ByVal LogDate As String)
        'Commented and Added By Vikrant On 08-May-2014 For ALL08052014

        ''----------------Added by Saylee on 04-July-2013 for ALL04072013-------------
        'Dim mAssemblyStatusList As AssemblyStatusList
        'mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(LogDate, mMachine.ID.ToString _
        ', , , , , , , , , , True, , , mAssemblyStatus.AssemblyID.ToString, , , , , , , , , , , , , , _
        ', , ).Item(0), MachineInfo).AssemblyStatusList

        'If mAssemblyStatusList.Count = 0 Then
        '    mAssemblyStatusList = CType(MachineList.GetMachineListWithRemoval(LogDate, mMachine.ID.ToString _
        '           , , , , , , , , , , True, , , mAssemblyStatus.AssemblyID.ToString, , , , , , , , , , , , , , _
        '           , ).Item(0), MachineInfo).AssemblyStatusList
        'End If
        ''-----------------------------

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
                       , , , , , , , , , , True, , , mAssemblyStatus.AssemblyID.ToString, , , , , , , , , , , , , , _
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
                    End If
                Next
            Else
                AssemblyStatusID = mAssemblyStatusList(0).ID
            End If
            'End

            'Here instead of mPrevAssemblyMonitorInspStatus.AssemblyStatusID pass mAssemblyStatusList(0).ID  
            'mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewComplyAssemblyMonitorInspStatus(Guid.NewGuid, mPrevAssemblyMonitorInspStatus.AssemblyID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, LogDate, mAssemblyStatus.Assembly.ModelID, mPrevAssemblyMonitorInspStatus.ModelMonitorInsp, LogID, mPrevAssemblyMonitorInspStatus.DoneOn.ToString, mMachine.HourType)
            mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewComplyAssemblyMonitorInspStatus(Guid.NewGuid, mPrevAssemblyMonitorInspStatus.AssemblyID, AssemblyStatusID, LogDate, mAssemblyStatus.Assembly.ModelID, mPrevAssemblyMonitorInspStatus.ModelMonitorInsp, LogID, mPrevAssemblyMonitorInspStatus.DoneOn.ToString, mMachine.HourType, CType(Session("ConsiderAssemblyInstValue"), Boolean))
        Else
            mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.NewComplyAssemblyMonitorInspStatus(Guid.NewGuid, mPrevAssemblyMonitorInspStatus.AssemblyID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, LogDate, mAssemblyStatus.Assembly.ModelID, mPrevAssemblyMonitorInspStatus.ModelMonitorInsp, LogID, LogDate, mMachine.HourType)

        End If
        mAssemblyMonitorInspStatus.BeginEdit()
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
        SetTitle()
    End Sub
    Private Sub EditRecord(ByVal LogID As Guid, ByVal DoneOnDate As String, ByVal FromEntry As Boolean)
        REM:-FromEntry is used for avoiding object Dirty at form load when we r coming thru' Edit.
        If FromEntry = False Then
            mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetComplyAssemblyMonitorInspStatus(mPrevAssemblyMonitorInspStatus.ID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, DoneOnDate, LogID, mMachine.HourType, CType(Session("ConsiderAssemblyInstValue"), Boolean))
        Else
            mAssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetComplyAssemblyMonitorInspStatusFromEntry(mPrevAssemblyMonitorInspStatus.ID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, DoneOnDate, mMachine.HourType, CType(Session("ConsiderAssemblyInstValue"), Boolean))
        End If
        mAssemblyMonitorInspStatus.BeginEdit()
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
        SetTitle()
    End Sub
    Private Sub SetFromClone(ByVal clnAssemblyMonitorInspStatus As AssemblyMonitorInspStatus)
        mAssemblyMonitorInspStatus.DoneWONo = clnAssemblyMonitorInspStatus.DoneWONo
        mAssemblyMonitorInspStatus.DoneRemark = clnAssemblyMonitorInspStatus.DoneRemark
        mAssemblyMonitorInspStatus.DoneByID = clnAssemblyMonitorInspStatus.DoneByID
        mAssemblyMonitorInspStatus.LicenseNo = clnAssemblyMonitorInspStatus.LicenseNo
        mAssemblyMonitorInspStatus.Place = clnAssemblyMonitorInspStatus.Place
        'Added By Vikrant On 25-Nov-2014
        mAssemblyMonitorInspStatus.IsAttachmentAdded = clnAssemblyMonitorInspStatus.IsAttachmentAdded
        'MLNo
        For j As Integer = mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Count - 1 To 0 Step -1
            mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.RemoveAt(j)
        Next
        For Each mMaintenanceDoneByEmployee As MaintenanceDoneByEmployee In clnAssemblyMonitorInspStatus.MaintenanceDoneByEmployees
            If Session("From") = 0 Then 'New Record
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Add(mAssemblyMonitorInspStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
            ElseIf Session("From") = 1 Then 'Edit Record
                If Not mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Contains(mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.EmployeeID) Then
                    mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Add(mAssemblyMonitorInspStatus.ID, mMaintenanceDoneByEmployee.MaintenanceTypeID, mMaintenanceDoneByEmployee.EmployeeID, mMaintenanceDoneByEmployee.LicenceNo, mMaintenanceDoneByEmployee.RequiredManHours, mMaintenanceDoneByEmployee.EmployeeName)
                End If
            End If
        Next
        'End
        If Not mFileAttach Is Nothing Then
            mFileAttach.ReferenceID = mAssemblyMonitorInspStatus.ID
            Session("mFileAttach") = mFileAttach
        End If
        'End
        clnAssemblyMonitorInspStatus = Nothing
    End Sub
    Private Sub SaveBoardInfo() 'Added by Saylee on 22-May-2009
        Dim mAssemblyMonitorInspStatusPeriod As AssemblyMonitorInspStatusPeriod
        Dim DueOnValue As String
        'Condition added by Saylee on 29-June-2009 to show DueOnValue blank for One time record
        If (mAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 1 And Not mAssemblyMonitorInspStatus.DoneOn Is DBNull.Value) Or (mAssemblyMonitorInspStatus.IsApplicable = False) Then
            DueOnValue = ""
        Else
            For Each mAssemblyMonitorInspStatusPeriod In mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
                If mAssemblyMonitorInspStatusPeriod.PeriodID = 2 Then
                    DueOnValue = DueOnValue + "<BR>" + mAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
                Else
                    DueOnValue = DueOnValue + "<BR>" + mAssemblyMonitorInspStatusPeriod.DueOnValueTextFormatted
                End If

            Next
        End If

        mBoardInfo = Session("mBoardInfo")

        If Not mBoardInfo.MonitorID.Equals(Guid.Empty) Then
            mBoardInfo.MonitorID = mAssemblyMonitorInspStatus.ID
            mBoardInfo.DueOnValue = DueOnValue
            mBoardInfo.ApplyEdit()
            mBoardInfo.Save()
            Session("mBoardInfo") = mBoardInfo
        End If
        Session("mAircraftInformationBoardList") = Nothing
    End Sub
	'Added By Vikrant On 17-Sep-2020 For Mismatch Value Mail Send
	Private Sub SendMail(ByVal InspStatus As AssemblyMonitorInspStatus, ByVal DoneOnValue As String, ByVal DoneOnValueObj As String, Optional ByVal OnlyEdited As Boolean = False, Optional ByVal ToMailIDs As String = "saylee@bytzsoft.com") ''ToMailIDs:="deven@bytzsoft.com,saylee@bytzsoft.com"
		Dim str As New StringBuilder
		Try
			If OnlyEdited = False Then
				str.Append("Mismatch Details for <b>" & IIf(Session("From") = 1, "Edited and Saved", IIf(InspStatus.IsNew, "New", "New but Saved")) & "</b> record are as follows: ")
			Else
				''  str.Append("Mismatch Details for <b>" & IIf(Session("From") = 1, "Only Edited", IIf(InspStatus.IsNew, "New", "New but Saved")) & "</b> record are as follows: ")
			End If

			str.Append("<p><b>Assembly Details: </b> " & mAssemblyStatus.Assembly.ModelName & " " & mAssemblyStatus.Assembly.SerialNo & "</p>")
			str.Append("<p><b>Inspection ID: </b> " & InspStatus.ID.ToString & "</p>")
			str.Append("<p><b>Inspection Description: </b> " & InspStatus.ModelMonitorInsp.Description & "</p>")
			str.Append("<p><b>Done On Date: </b> " & txtDoneOnDate.Text & "</p>")
			str.Append("<p><b>Done On Value: </b> " & DoneOnValue & "</p>")
			str.Append("<p><b>Done On Date(obj.): </b> " & mAssemblyMonitorInspStatus.DoneOnFormatted.ToString & "</p>")
			str.Append("<p><b>Done On Value(obj.): </b> " & DoneOnValueObj & "</p>")

			str.Append("<p><b>Saved By: </b> " & User.Identity.Name)

			SendMailFile.SendMailFile(Nothing, User.Identity.Name, "FAS: Assembly Inspection Done on Date Done on Value Mismatch Details", "", Info:=str.ToString, VendorEmailID:="", ToMailID:=ToMailIDs)
		Catch ex As Exception
			Dim Title As String = "Error Sending Mail"
			Dim Message As String = ex.InnerException.ToString
			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", MessageBox.Show(Title, Message, , False), True)
			Exit Sub
		End Try
	End Sub
	'End
	Private Function Save() As Boolean
        Dim clnAssemblyMonitorInspStatus As AssemblyMonitorInspStatus
        clnAssemblyMonitorInspStatus = CType(mAssemblyMonitorInspStatus.Clone, AssemblyMonitorInspStatus)
        SetObject()
        SetGridObject()
        SetMachineMaintenanceObject() 'Added by Saylee on 9th-Oct-2009
        If mAssemblyMonitorInspStatus.IsValid Then
            If mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.PeriodRequired, MSGBox.Message_text.PeriodRequired, "You are trying to save Assembly Inspection Status.Assembly Inspection Status can not be saved without period units.", MsgBoxStyle.OkOnly, "")
                Return False
            End If
            Try
                'Added By Vikrant On 06-Aug-2013 For ALL01082013
                If Not mAssemblyMonitorInspStatus.DoneByID.Equals(Guid.Empty) AndAlso Not mAssemblyMonitorInspStatus.DoneOn.Equals(System.DBNull.Value) Then
                    Dim title As String = "Save Alert !"
                    Dim message As String = ""
                    mEmployeeStatus = EmployeeStatus.GetEmployeeWorkingStatus(mAssemblyMonitorInspStatus.DoneByID.ToString, mAssemblyMonitorInspStatus.DoneOn)
                    If (mEmployeeStatus(0).Information <> "") Then
                        message = mEmployeeStatus(0).Information
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenAlertMessage", MessageBox.Show(title, message, IsTagRequired:=False), True)
                        Return False
                    End If
                End If
                'End
                
                mAssemblyMonitorInspStatus.ApplyEdit()
                mAssemblyMonitorInspStatus = CType(mAssemblyMonitorInspStatus.Save(), AssemblyMonitorInspStatus)
                'Added By Vikrant On 17-Sep-2020 For Mismatch Value Mail Send
                If txtDoneOnDate.Text <> "" AndAlso mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Contains(2, "") Then 'If date period conatins then only execute
                    Dim DoneOnValue As New StringBuilder
                    Dim DoneOnValueObj As New StringBuilder
                    For j As Integer = 0 To Me.dgDoneOnValue.Rows.Count - 1
                        DoneOnValue.Append(CType(Me.dgDoneOnValue.Rows(j).FindControl("txtCurrentValue"), TextBox).Text + ", ")
                        DoneOnValueObj.Append(mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted + ", ")
                        If mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(j).PeriodID = 2 Then
                            If Not mAssemblyMonitorInspStatus.DoneOnFormatted.ToString.Equals(mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted) Then
                                Session("IsSendMail") = "True"
                            End If
                        End If
                    Next j
                    If Session(name:="IsSendMail") = "True" Then
                        Session.Remove("IsSendMail")
                        SendMail(InspStatus:=mAssemblyMonitorInspStatus, DoneOnValue.ToString.Trim.TrimEnd(","), DoneOnValueObj.ToString.Trim.TrimEnd(","), ToMailIDs:="")
                    End If
                End If
                'End
                'Revise Activity
                If Not Session("mPrevAssemblyMonitorInspStatusForRevise") Is Nothing Then
                    Dim mPrevAssemblyMonitorInspStatusForRevise As AssemblyMonitorInspStatus
                    mPrevAssemblyMonitorInspStatusForRevise = Session("mPrevAssemblyMonitorInspStatusForRevise")
                    mPrevAssemblyMonitorInspStatusForRevise.IsApplicable = False
                    mPrevAssemblyMonitorInspStatusForRevise.Save()
                    Session.Remove("mPrevAssemblyMonitorInspStatusForRevise")
                End If
                'End
                SaveAttachment() 'Added By Vikrant On 25-Nov-2014
                SaveBoardInfo() 'Added by Saylee on 22-May-2009
                SaveMachineMaintenance()  'Added by Saylee on 9th-Oct-2009
                Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
                mAssemblyInfo = Session("mAssemblyInfo")
                'Changed by Vikrant on 28-July-2011
                Dim mDoneOnValues As New System.Text.StringBuilder
                For i As Integer = 0 To mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Count - 1
                    mDoneOnValues.Append(mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(i).DoneOnValueFormatted + ",")
                Next
                mMonitorInfo = txtMonitorInspType.Text
                mMonitorType = txtMonitorType.Text
                mModel = mAssemblyStatus.ModelName
                mSerialNo = mAssemblyStatus.Assembly.SerialNo
                mInspectionDetail = "Model : " + mModel + " Serial No : " + mSerialNo + " Monitor Info. :- " + mMonitorInfo + " Description : " + txtDescription.Text.Trim + " Monitor Type : " + mMonitorType & " Done On Date : " + mAssemblyMonitorInspStatus.DoneOnFormatted + " Done On Value : " + mDoneOnValues.ToString
                MarkLog(Util.Action.Save, "AssemblyInspections", mInspectionDetail, Util.ErrorType.NoError, mAssemblyMonitorInspStatus.ID, EventLogID)
                Return True
            Catch ex As SqlException
                Session("mAssemblyMonitorInspStatus") = clnAssemblyMonitorInspStatus
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
                clnAssemblyMonitorInspStatus = Nothing
            End Try
        Else
            Return False
        End If
    End Function
    Private Sub SetTitle()
        If IsDate(mAssemblyMonitorInspStatus.DoneOn) Then
            '' calDoneOn.Text = CDate(mAssemblyMonitorInspStatus.DoneOn)
            'calDoneOn.TitleText = CDate(mAssemblyMonitorInspStatus.DoneOn)
            'calDoneOn.DateToday = CDate(mAssemblyMonitorInspStatus.DoneOn)
            'calDoneOn.SelectedDate = CDate(mAssemblyMonitorInspStatus.DoneOn)
            'ElseIf IsDate(mAssemblyStatus.AsOnDate) Then
            '    calDoneOn.Text = CDate(mAssemblyStatus.AsOnDate)
            'calDoneOn.TitleText = CDate(mAssemblyStatus.AsOnDate)
            'calDoneOn.DateToday = CDate(mAssemblyStatus.AsOnDate)
            'calDoneOn.SelectedDate = CDate(mAssemblyStatus.AsOnDate)
        End If
        Dim AssemblyInfo As String = "[Model: " & mAssemblyStatus.ModelName & " SerialNo: " & mAssemblyStatus.Assembly.SerialNo & " ]"
        If mAssemblyMonitorInspStatus.IsNew Then
            lblTitle.Text = IIf(mIsSpareAssembly = 0, "", IIf(mAssemblyStatus.IsSpareAssembly, "Stock ", "Removed ")) + "Assembly Inspection Status " & AssemblyInfo & " [New]"
        Else
            lblTitle.Text = IIf(mIsSpareAssembly = 0, "", IIf(mAssemblyStatus.IsSpareAssembly, "Stock ", "Removed ")) + "Assembly Inspection Status" & AssemblyInfo
        End If
        lblAssemblyValue.InnerText = mAssemblyStatus.AssemblyTypeName & " Values"
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    'Revise Activity
                    If MSGBoxCtrl.Sender = "ReviseActivity" Then
                        MarkLog(Util.Action.[New], "Model Inspection", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
                        Dim mModelMonitorInsp As ModelMonitorInsp
                        Dim ID As Guid = Guid.NewGuid
                        'Revise Activity New
                        Dim mModelMonitorInspList As ModelMonitorInspList
                        mModelMonitorInspList = ModelMonitorInspList.GetModelMonitorInspList(mAssemblyMonitorInspStatus.ModelMonitorInsp.ModelID, GetRecordsByPrevRefID:=True, PrevRefID:=mAssemblyMonitorInspStatus.ModelMonitorInsp.PrevRefID.ToString)
                        If mModelMonitorInspList.Count > 1 Then
                            For i As Integer = mModelMonitorInspList.Count - 1 To 0 Step -1
                                If mModelMonitorInspList(i).ID.Equals(mAssemblyMonitorInspStatus.ModelMonitorInsp.ID) Then
                                    Exit For
                                Else
                                    Session("ModelMonitorInspIDToBeLinked") = mModelMonitorInspList(i).ID.ToString
                                    Session("ModelMonitorInspPrevRefIDToBeLinked") = mModelMonitorInspList(i).PrevRefID.ToString
                                    Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
                                    Session("mPrevAssemblyMonitorInspStatusForRevise") = mAssemblyMonitorInspStatus
                                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfModelMonitorInspList_Ajax.aspx?BackPage=Index.aspx');", True)
                                    Exit Sub
                                End If
                            Next
                        End If
                        'END
                        mModelMonitorInsp = ModelMonitorInsp.NewModelMonitorInsp(mAssemblyMonitorInspStatus.ModelMonitorInsp, mMachine.HourType)
                        Session("mModelMonitorInsp") = mModelMonitorInsp
                        RemoveSession()
                        mModelMonitorInsp.BeginEdit()
                        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
                        Session("mPrevAssemblyMonitorInspStatusForRevise") = mAssemblyMonitorInspStatus
                        Session("IsLinkedActivitySelected") = True 'Revise Activity New
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenModelInspMasterWindow", "OpenModelInspMasterWindow();", True)
                    ElseIf (MSGBoxCtrl.Sender = "OverDue" Or MSGBoxCtrl.Sender = "ComplyOnSameDate") Then 'Added by Saylee on 26-Mar-2019 for ALL26032019
                        'ComplyOnSameDate Added By Prashant 19-Nov-2019 Alert if user is complying on same date 
                        If Save() Then
                            If MSGBoxCtrl.Sender = "OverDue" Then
                                MarkLog(Action.Save, "AssemblyInspections", User.Identity.Name & " saved OverDue record : " & Session("OverDueString") & " " & Session("DueString"), Util.ErrorType.HandledError, mAssemblyMonitorInspStatus.ID, EventLogID)
                            ElseIf MSGBoxCtrl.Sender = "ComplyOnSameDate" Then
                                MarkLog(Util.Action.Save, "AssemblyInspections", User.Identity.Name & " Comply On Same Date : ", Util.ErrorType.HandledError, mAssemblyMonitorInspStatus.ID, EventLogID)
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
                                        Dim Result As Boolean
                                        SetLinkedMaintenanceGridObject()
                                        Dim LinkMaintenanceEvents As LinkedMaintenanceActivityEvents = New LinkedMaintenanceActivityEvents
                                        LinkMaintenanceEvents.AssemblyLogInfo = "Assembly Inspection : " & mInspectionDetail 'setting Mark Log Detail ...

                                        LicenseNo = IIf(Session("LicenseNo") Is Nothing, String.Empty, Session("LicenseNo"))
                                        Dim EmployeeID As String = IIf(Session("EmpID") Is Nothing, String.Empty, Session("EmpID"))
                                        ' EmployeeID = EmployeeID.ToString.TrimEnd(",")

                                        EmpName = IIf(Session("EmpName") Is Nothing, String.Empty, Session("EmpName"))
                                        EmployeeID = IIf(EmployeeID Is Nothing, "", EmployeeID.ToString.TrimEnd(","))
                                        LicenseNo = IIf(LicenseNo Is Nothing, "", LicenseNo.ToString.TrimEnd(","))
                                        EmpName = IIf(EmpName Is Nothing, "", EmpName.ToString.TrimEnd(","))

                                        Result = LinkMaintenanceEvents.SaveLinkedMaintenanceActivies(mMultiComplianceList, mAssemblyMonitorInspStatus.DoneWONo, txtDoneOnDate.Text, mMachineMaintenance.LogID, mMachine.HourType, mMachine.ID, mAssemblyMonitorInspStatus.AssemblyID, PeriodValues, mAssemblyMonitorInspStatus.DoneRemark, LicenseNo, EmployeeID, EmpName, Trim(txtPlace.Text))

                                        Session.Remove("EmpID")
                                        Session.Remove("LicenseNo")
                                        Session.Remove("EmpName")
                                        If LinkMaintenanceEvents.ErrorStr.Length > 0 Then
                                            'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, LinkMaintenanceEvents.ErrorStr, MsgBoxStyle.OKOnly)
                                            'msg.ReplacePage = "wfComplyAssemblyMonitorInspStatus_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                                            'Commented & Added By Vikrant On 06-Aug-2013 For ALL01082013
                                            'lblAlertTitle.Text = "Link Maintenance Alert !"
                                            'lblAlertMessage.Text = LinkMaintenanceEvents.ErrorStr
                                            'ClientScript.RegisterStartupScript(Me.GetType(), "OpenAlertMessage", "<script type='text/javascript'>OpenAlert();</script>")
                                            Dim title As String = "Link Maintenance Alert !"
                                            Dim message As String = LinkMaintenanceEvents.ErrorStr
                                            MSGBoxCtrl.show(title, message, "", MsgBoxStyle.OkOnly, "")
                                            'End
                                            Exit Sub
                                        Else
                                            MSGBoxCtrl.show("Alert !", "<BR>Other Maintenance Activity(s) linked with this maintenance activity.To Edit/Delete individual Maintenance Activity go to respective activity.", "", MsgBoxStyle.OkOnly, "LMAlert")
                                            Exit Sub
                                        End If
                                    End If
                                End If
                            End If
                            'End
                            RemoveSession()
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
                    ElseIf MSGBoxCtrl.Sender = "OverDue" Then
                        'do nothing
                    End If
                    'End
                Case MsgBoxResult.Cancel
                   
                Case MsgBoxResult.Ok ''And Session("sender") = ""        'Code Added
                    'Added By Utkarsh On 01-Jun-2012 For Link Maintenance
                    If MSGBoxCtrl.Sender = "LMAlert" Then
                        Session("sender") = ""
                        RemoveSession()
                        Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
                    End If
                    'End
                    Session("sender") = ""
                    'DataFieldBind()
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
        btnPrint.Enabled = Not mAssemblyMonitorInspStatus.IsNew
        dgCurrentValue.Columns(3).Visible = Not mAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 3
        dgCurrentValue.Columns(4).Visible = Not mAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 3
        'Added By Saylee on 28-08-2008
        dgDoneOnValue.Columns(2).Visible = Not mAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 3
        '===================================
        dgDoneOnValue.Columns(3).Visible = Not mAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 3
        'Added By Shweta ON 28-Jun-2013 FOR ALL28062013
        dgDoneOnValue.Columns(4).Visible = (mAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID <> 3) AndAlso (mAssemblyStatus.AssemblyTypeID <> 1 AndAlso mAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID <> 3) AndAlso mIsSpareAssembly <> 1 'mIsSpareAssembly Added By Vikrant On 27-Jul-2020 For ALL27072020
        '
        txtExtensionDate.Visible = Not mAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 3

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
        If mAssemblyMonitorInspStatus.ModelMonitorInsp.ReadOnlyFrequencyColumn Then
            'txtDoneOnDate.Enabled = False 'Commented by Saylee on 22-Nov-2019 as DoneOne should be open in all cases, 
            chkApplicable.Enabled = False
        End If
        btnRevise.Enabled = (mAssemblyMonitorInspStatus.IsApplicable And Not mAssemblyMonitorInspStatus.IsNew And Not ((mAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 1 Or mAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 4) And mAssemblyMonitorInspStatus.DoneOnFormatted.ToString <> "")) 'Revise Activity
        btnSelectLog.Visible = (mIsSpareAssembly <> 1) ' Added By Vikrant On 27-Jul-2020 For ALL27072020
        lnkPrintLogBookEntry.Visible = (mIsSpareAssembly <> 1)
        ControlVisibilityForAttachment() 'Added By Vikrant On 25-Nov-2014
    End Sub
    Private Sub SetMachineMaintenanceObject()
        'Added by Saylee on 9th-Oct-2009

        If Session("From") = 0 And Not (mMachineMaintenanceList.Contains(mAssemblyMonitorInspStatus.ID, 6, "")) Then
            mMachineMaintenance = MachineMaintenance.NewMachineMaintenance(mAssemblyStatus.MachineID, 6, txtDoneOnDate.Text, mAssemblyMonitorInspStatus.ID, Guid.Empty, 0, 0, mAssemblyStatus.ID)
        Else
            mMachineMaintenance = MachineMaintenance.GetMachineMaintenance(mAssemblyMonitorInspStatus.ID, 6)
        End If

        With mMachineMaintenance
            ''.MachineID = mAssemblyStatus.MachineID
            ''.MaintenanceActivityTypeID =6
            .MaintenanceID = mAssemblyMonitorInspStatus.ID 'TransactionID
            ''.AssemblyStatusID = mAssemblyStatus.ID

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
                'End
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
        ''  End If
    End Sub
    'Added By Utkarsh On 07-Feb-2012 FOR Link Maintenance
    Private Sub ShowLinkedMaintenaceActivity()

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

                    mLinkMaintenanceMonitorStatus = LinkMaintenaceMonitorStatus.GetLinkedMaintenanceMonitorStatus(mMachine.ID, mLinkMaintenanceList(i).LinkedMaintenaceActivityID, mLinkMaintenanceList(i).LinkedMaintenanceTypeID, mAssemblyMonitorInspStatus.AssemblyID)
                    If mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.Equals(Guid.Empty) Then
                        Exit Select
                    End If
                    Dim mPrevAssemblyMonitorSeviceStatus As AssemblyMonitorServiceStatus = AssemblyMonitorServiceStatus.GetAssemblyMonitorServiceStatus(mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID, mLinkMaintenanceMonitorStatus.AssemblyStatusID, mMachine.HourType)

                    mAssemblyMonitorServiceStatusForLM = AssemblyMonitorServiceStatus.GetComplyAssemblyMonitorServiceStatusForLinkMaintenance(mPrevAssemblyMonitorSeviceStatus.ID, mPrevAssemblyMonitorSeviceStatus.AssemblyStatusID, txtDoneOnDate.Text, Guid.Empty, mMachine.HourType, mLinkMaintenanceMonitorStatus.ModelMonitorID, True)

                    Dim mAssemblyInfo As String = mAssemblyMonitorServiceStatusForLM.ModelMonitorService.Model.Name '& VbCrLf & mAssemblyMonitorServiceStatusForLm.

                    For j As Integer = 0 To mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods.Count - 1

                        If mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).periodid = 2 Then

                            Dim PeriodCode As String = mPeriodUnitList(3, "").Code

                            If j = 0 Then

                                mPeriodUnitName = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitName
                                mFrequencyValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).FrequencyValueFormatted & " " & mPeriodUnitList(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitID, "").Code
                                mDoneOnValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DoneOnValueFormatted
                                mCurrentValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).CurrentValueFormatted
                                mDueOnValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DueOnValueFormatted
                                mElapsedValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
                                mRemainingValue = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
                                'mDoneOn = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted
                                mExtensionValue = IIf(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            Else
                                mPeriodUnitName = mPeriodUnitName & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitName
                                mFrequencyValue = mFrequencyValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).FrequencyValueFormatted & " " & mPeriodUnitList(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitID, "").Code
                                mDoneOnValue = mDoneOnValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DoneOnValueFormatted
                                mCurrentValue = mCurrentValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).CurrentValueFormatted
                                mDueOnValue = mDueOnValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DueOnValueFormatted
                                mElapsedValue = mElapsedValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
                                mRemainingValue = mRemainingValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
                                'mDoneOn = mDoneOn & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted
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
                                'mDoneOn = mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted
                                mExtensionValue = IIf(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            Else
                                mPeriodUnitName = mPeriodUnitName & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).PeriodUnitName
                                mFrequencyValue = mFrequencyValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).FrequencyValueFormatted & " " & PeriodCode
                                mDoneOnValue = mDoneOnValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DoneOnValueFormatted & " " & PeriodCode
                                mCurrentValue = mCurrentValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).CurrentValueFormatted & " " & PeriodCode
                                mDueOnValue = mDueOnValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).DueOnValueFormatted & " " & PeriodCode
                                mElapsedValue = mElapsedValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
                                mRemainingValue = mRemainingValue & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
                                'mDoneOn = mDoneOn & vbCrLf & mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted
                                mExtensionValue = mExtensionValue & vbCrLf & IIf(mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorServiceStatusForLM.AssemblyMonitorServiceStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            End If


                        End If
                    Next
                    mMultiComplianceList.Add(  mAssemblyMonitorServiceStatusForLM.ID, MaintenanceActivityTypes.AssemblyService, IIf(Session("From") = 1, False, True), mAssemblyMonitorServiceStatusForLM.ModelMonitorService.Reference, mAssemblyMonitorServiceStatusForLM.ModelMonitorService.MonitorTypeName, mAssemblyMonitorServiceStatusForLM.ModelMonitorService.ModelMonitorServiceTypeName, mAssemblyMonitorServiceStatusForLM.ModelMonitorService.Description, mAssemblyMonitorServiceStatusForLM.DoneOn.ToString, mAssemblyMonitorServiceStatusForLM.DoneWONo, mAssemblyMonitorServiceStatusForLM.DoneRemark, mPeriodUnitName, mFrequencyValue, mDoneOnValue, mCurrentValue, mElapsedValue, mExtensionValue, mDueOnValue, mRemainingValue, , , mMachine.RegNo, mAssemblyMonitorServiceStatusForLM.ModelMonitorService.Model.AssemblyTypeName,  mAssemblyInfo, , , mPeriodUnitName, , mFrequencyValue, , mLinkMaintenanceMonitorStatus.AssemblyStatusID.ToString, , , , , mAssemblyMonitorServiceStatusForLM.ModelMonitorService.ModelID.ToString, , , , , mAssemblyMonitorServiceStatusForLM.ModelMonitorService.ATAChapter, , , , , mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.ToString, , , , , , , , , , mLinkMaintenanceList(i).MaintenanceActionID, mLinkMaintenanceList(i).MaintenanceActionName)
                    mLinkMaintenanceMonitorStatus = Nothing

                Case 2 'Assembly Inspection

                    mLinkMaintenanceMonitorStatus = LinkMaintenaceMonitorStatus.GetLinkedMaintenanceMonitorStatus(mMachine.ID, mLinkMaintenanceList(i).LinkedMaintenaceActivityID, mLinkMaintenanceList(i).LinkedMaintenanceTypeID, mAssemblyMonitorInspStatus.AssemblyID)
                    If mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.Equals(Guid.Empty) Then
                        Exit Select
                    End If
                    Dim mPrevAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = AssemblyMonitorInspStatus.GetAssemblyMonitorInspStatus(mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID, mLinkMaintenanceMonitorStatus.AssemblyStatusID, mMachine.HourType)

                    mAssemblyMonitorInspStatusForLM = AssemblyMonitorInspStatus.GetComplyAssemblyMonitorInspStatusForLinkMaintenance(mPrevAssemblyMonitorInspStatus.ID, mPrevAssemblyMonitorInspStatus.AssemblyStatusID, txtDoneOnDate.Text, Guid.Empty, mMachine.HourType, mLinkMaintenanceMonitorStatus.ModelMonitorID, True)

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
                                'mDoneOn = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted
                                mExtensionValue = IIf(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            Else
                                mPeriodUnitName = mPeriodUnitName & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodUnitName
                                mFrequencyValue = mFrequencyValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).FrequencyValueFormatted & " " & mPeriodUnitList(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodUnitID, "").Code
                                mDoneOnValue = mDoneOnValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DoneOnValueFormatted
                                mCurrentValue = mCurrentValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted
                                mDueOnValue = mDueOnValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DueOnValueFormatted
                                mElapsedValue = mElapsedValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
                                mRemainingValue = mRemainingValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
                                'mDoneOn = mDoneOn & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted
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
                                'mDoneOn = mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted
                                mExtensionValue = IIf(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            Else
                                mPeriodUnitName = mPeriodUnitName & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).PeriodUnitName
                                mFrequencyValue = mFrequencyValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).FrequencyValueFormatted & " " & PeriodCode
                                mDoneOnValue = mDoneOnValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DoneOnValueFormatted & " " & PeriodCode
                                mCurrentValue = mCurrentValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted & " " & PeriodCode
                                mDueOnValue = mDueOnValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).DueOnValueFormatted & " " & PeriodCode
                                mElapsedValue = mElapsedValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
                                mRemainingValue = mRemainingValue & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
                                'mDoneOn = mDoneOn & vbCrLf & mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted
                                mExtensionValue = mExtensionValue & vbCrLf & IIf(mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorInspStatusForLM.AssemblyMonitorInspStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            End If

                        End If

                    Next
                    mMultiComplianceList.Add(mAssemblyMonitorInspStatusForLM.ID, MaintenanceActivityTypes.AssemblyInspection, IIf(Session("From") = 1, False, True), mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.Reference, mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.MonitorTypeName, mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.ModelMonitorInspTypeName, mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.Description, mAssemblyMonitorInspStatusForLM.DoneOn.ToString, mAssemblyMonitorInspStatusForLM.DoneWONo, mAssemblyMonitorInspStatusForLM.DoneRemark, mPeriodUnitName, mFrequencyValue, mDoneOnValue, mCurrentValue, mElapsedValue, mExtensionValue, mDueOnValue, mRemainingValue, , , mMachine.RegNo, mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.Model.AssemblyTypeName, mAssemblyInfo, , , mPeriodUnitName, , mFrequencyValue, , mLinkMaintenanceMonitorStatus.AssemblyStatusID.ToString, , , , , mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.ModelID.ToString, , , , , mAssemblyMonitorInspStatusForLM.ModelMonitorInsp.ATAChapter, , , , , , mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.ToString, , , , , , , , , mLinkMaintenanceList(i).MaintenanceActionID, mLinkMaintenanceList(i).MaintenanceActionName)
                    mLinkMaintenanceMonitorStatus = Nothing

                Case 3 'Assembly Directive
                    mLinkMaintenanceMonitorStatus = LinkMaintenaceMonitorStatus.GetLinkedMaintenanceMonitorStatus(mMachine.ID, mLinkMaintenanceList(i).LinkedMaintenaceActivityID, mLinkMaintenanceList(i).LinkedMaintenanceTypeID, mAssemblyMonitorInspStatus.AssemblyID)
                    If mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID.Equals(Guid.Empty) Then
                        Exit Select
                    End If
                    Dim mPrevAssemblyMonitorModStatus As AssemblyMonitorModStatus = AssemblyMonitorModStatus.GetAssemblyMonitorModStatus(mLinkMaintenanceMonitorStatus.AssemblyMonitorStatusID, mLinkMaintenanceMonitorStatus.AssemblyStatusID, mMachine.HourType)

                    mAssemblyMonitorModStatusForLM = AssemblyMonitorModStatus.GetComplyAssemblyMonitorModStatusForLinkMaintenance(mPrevAssemblyMonitorModStatus.ID, mPrevAssemblyMonitorModStatus.AssemblyStatusID, txtDoneOnDate.Text, Guid.Empty, mMachine.HourType, mLinkMaintenanceMonitorStatus.ModelMonitorID, True)

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
                                'mDoneOn = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted
                                mExtensionValue = IIf(mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            Else
                                mPeriodUnitName = mPeriodUnitName & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodUnitName
                                mFrequencyValue = mFrequencyValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).FrequencyValueFormatted & " " & mPeriodUnitList(mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodUnitID, "").Code
                                mDoneOnValue = mDoneOnValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DoneOnValueFormatted
                                mCurrentValue = mCurrentValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted
                                mDueOnValue = mDueOnValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DueOnValueFormatted
                                mElapsedValue = mElapsedValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
                                mRemainingValue = mRemainingValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
                                'mDoneOn = mDoneOn & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted
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
                                'mDoneOn = mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted
                                mExtensionValue = IIf(mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted = "", "", mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted & " " & PeriodCode)
                            Else
                                mPeriodUnitName = mPeriodUnitName & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).PeriodUnitName
                                mFrequencyValue = mFrequencyValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).FrequencyValueFormatted & " " & PeriodCode
                                mDoneOnValue = mDoneOnValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DoneOnValueFormatted & " " & PeriodCode
                                mCurrentValue = mCurrentValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).CurrentValueFormatted & " " & PeriodCode
                                mDueOnValue = mDueOnValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).DueOnValueFormatted & " " & PeriodCode
                                mElapsedValue = mElapsedValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ElapsedValueFormatted & " " & PeriodCode
                                mRemainingValue = mRemainingValue & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).RemainingValueFormatted & " " & PeriodCode
                                'mDoneOn = mDoneOn & vbCrLf & mAssemblyMonitorModStatusForLM.AssemblyMonitorModStatusPeriods(j).ExtensionValueFormatted
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
        lblResult.Text = "List of Linked Maintenance Activity : " & mMultiComplianceList.Count & " Record(s) found."
    End Sub
    'Added By Utkarsh On 15-Mar-2012 FOR Link Maintenance
    Private Sub SetLinkedMaintenanceGridObject()
        Dim chkSelect As CheckBox
        For i As Integer = 0 To dgMultiComplianceList.Rows.Count - 1
            chkSelect = CType(dgMultiComplianceList.Rows(i).FindControl("chkSelect"), CheckBox)
            mMultiComplianceList(i).IsSelect = chkSelect.Checked
        Next
    End Sub
    'End
    'Added By Vikrant On 25-Nov-2014
    Private Sub NewRecordAttachment()
        mFileAttach = FileAttach.NewAttachment(Guid.Empty, mAssemblyMonitorInspStatus.ID)
        Session("mFileAttach") = mFileAttach
    End Sub
    Private Sub ControlVisibilityForAttachment()
        If mAssemblyMonitorInspStatus.IsAttachmentAdded Then 'change from  to current condition
            ImageButton1.Visible = True
            btnDelAttach.Enabled = True
        Else
            ImageButton1.Visible = False
            btnDelAttach.Enabled = False
        End If
    End Sub
    Private Sub GetAttachment()
        If mAssemblyMonitorInspStatus.IsAttachmentAdded And mFileAttach Is Nothing Then 'And Session("From") = 1
            'If Not mFileAttach Is Nothing Then
            '    If Not mFileAttach.IsDirty Then
            mFileAttach = FileAttach.GetAttachment(mAssemblyMonitorInspStatus.ID)
            Session("mFileAttach") = mFileAttach
            'End If
            '    End If

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
                If (Not mAssemblyMonitorInspStatus.IsNew) And IsAttachmentDeleted Then
                    FileAttach.DeleteAttachment(mFileAttach.ID, mAssemblyMonitorInspStatus.ID)
                End If
                IsAttachmentDeleted = False
                Session("IsAttachmentDeleted") = IsAttachmentDeleted
            End If
        End If
    End Sub
    Private Sub ViewImage()
        Dim No As New Random
        Dim StrName As String = "abc" & No.Next.ToString

        If mAssemblyMonitorInspStatus.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mAssemblyMonitorInspStatus.ID)
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
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFile", "openFile();", True)
            End If
        End If
    End Sub
    'End
    Private Sub ControlVisibilityForDatePeriod()
        Dim txtDnOnDate As TextBox
        For j As Integer = 0 To Me.dgDoneOnValue.Rows.Count - 1
            txtDnOnDate = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtCurrentValue"), TextBox)
            With mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
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
        dgCurrentValue.DataSource = mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
        dgDoneOnValue.DataSource = mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods

        'Added On 28,May,2007 By Girish
        txtDoneOnDate.Text = mAssemblyMonitorInspStatus.DoneOnFormatted.ToString

        txtExtensionDate.Text = mAssemblyMonitorInspStatus.ExtensionDateFormatted.ToString

        'Added by Saylee on 9th-Oct-2009
        mMachineMaintenanceList = MachineMaintenanceList.GetMachineMaintenanceList()
        Session("mMachineMaintenanceList") = mMachineMaintenanceList

        'Added By Utkarsh On 01-Feb-2012 FOR Link Maintenance

        If AppSettings("LinkMaintenance") = "True" Then
            mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList(mPrevAssemblyMonitorInspStatus.ModelMonitorInspID.ToString)
            Session("mLinkMaintenanceList") = mLinkMaintenanceList
            If mLinkMaintenanceList.Count > 0 Then
                ShowLinkedMaintenaceActivity()
            End If
        End If
        'End
        If mAssemblyMonitorInspStatus.ModelMonitorInsp.RequiredManHours <> "" Then lblEstdManHours.Text = "(Estd. Man Hours : " + mAssemblyMonitorInspStatus.ModelMonitorInsp.RequiredManHours + ")"

        BindLicenceNo() 'MLNo
        DataBind()
    End Sub
    Private Sub DataBindGrid()
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
        dgCurrentValue.DataSource = mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
        dgDoneOnValue.DataSource = mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
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
            'Added By Utkarsh On 11-Jun-2012 FOR ALL08062012
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
        If Not mAssemblyMonitorInspStatus.IsValid Then
            For i As Integer = 0 To mAssemblyMonitorInspStatus.GetBrokenRulesCollection.Count - 1
                str = str + mAssemblyMonitorInspStatus.GetBrokenRulesCollection(i).Description + "<BR>"
            Next
        End If
        For i As Integer = 0 To CShort(dgDoneOnValue.Rows.Count - 1)
            If Not mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Item(i).IsValid Then
                Dim x As Integer
                For x = 0 To mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
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
            If Not mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Item(i).IsValid Then
                For x As Integer = 0 To mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Item(i).GetBrokenRulesCollection.Count - 1
                    str = str + mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Item(i).GetBrokenRulesCollection(x).Description + "<BR>"
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
    'MLNo
    Public Sub SetLicenceCount()
        If mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Count > 1 Then
            lblLicenceCount.Text = "and " + (mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Count - 1).ToString + " more"
        End If
        lblLicenceCount.DataBind()
        'lblAllLicenceNos.DataBind()
    End Sub
    Private Sub BindLicenceNo()
        If mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Count > 0 Then
            txtLicenceNo.Text = mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(0).LicenceNo + " [" + mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(0).EmployeeName + "]"
        Else
            txtLicenceNo.Text = String.Empty
        End If
    End Sub
    'End
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetSession()
        'Added by Vikrant on 26-July-2011
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And CType(Session("sender"), String) = "" Then
            If txtMonitorInspType.Enabled = True Then
                'Changed by Yogita on 8-Jan-2008 to solve Bug No:-AMISD1
                'SetFocus(txtMonitorInspType)
                btnSelectLog.Focus()
            End If
            Session("mLogList") = Nothing
            SetLog()
            DataFieldBind()
            'GetAttachment() 'Added By Vikrant On 25-Nov-2014
            ControlVisibility()
            ControlVisibilityForDatePeriod()
            SetTitle()
            'MLNo
            SetLicenceCount()
            UserNameForLicenceList = User.Identity.Name
            Session("UserNameForLicenceList") = UserNameForLicenceList
            'End

            ''''If Not mAssemblyMonitorInspStatus.IsNew And Session("From") = 1 Then

            ''''    'Added By Saylee On 9-FEB-2021 For Mismatch Value Mail Send of Controls
            ''''    If txtDoneOnDate.Text <> "" AndAlso mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Contains(2, "") Then 'If date period conatins then only execute
            ''''        Dim DoneOnValue As New StringBuilder
            ''''        Dim DoneOnValueObj As New StringBuilder
            ''''        Dim ControlDoneOnValue As String = String.Empty
            ''''        For j As Integer = 0 To Me.dgDoneOnValue.Rows.Count - 1
            ''''            DoneOnValue.Append(CType(Me.dgDoneOnValue.Rows(j).FindControl("txtCurrentValue"), TextBox).Text + ", ")
            ''''            DoneOnValueObj.Append(mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(j).CurrentValueFormatted + ", ")
            ''''            If mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(j).PeriodID = 2 Then
            ''''                ControlDoneOnValue = CType(Me.dgDoneOnValue.Rows(j).FindControl("txtCurrentValue"), TextBox).Text
            ''''                If Not txtDoneOnDate.Text.ToString.Equals(ControlDoneOnValue) Then
            ''''                    Session("IsSendMail") = "True"
            ''''                End If
            ''''            End If
            ''''        Next j
            ''''        If Session("IsSendMail") = "True" Then
            ''''            Session.Remove("IsSendMail")
            ''''            SendMail(mAssemblyMonitorInspStatus, DoneOnValue.ToString.Trim.TrimEnd(","), DoneOnValueObj.ToString.Trim.TrimEnd(","), True, ToMailIDs:="deven@bytzsoft.com,saylee@bytzsoft.com")
            ''''        End If
            ''''    End If
            ''''    'End
            ''''End If
        End If
    End Sub
    Private Sub dgDoneOnValue_RowCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Select Case e.CommandName
            Case "CurrentValue"
                If Not IsValid Then Exit Sub
                Dim txtCurrentValue As TextBox
                For i As Integer = 0 To dgDoneOnValue.Rows.Count - 1
                    txtCurrentValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtCurrentValue"), TextBox)
                    With mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
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
                For i As Integer = 0 To mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Count - 1
                    txtExtensionValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtExtensionValue"), TextBox)

                    With mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
                        .Item(i).ExtensionValue = Trim(txtExtensionValue.Text)
                    End With
                Next
                DataBindGrid()
        End Select
    End Sub
    Protected Sub txtCurrentValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'If Not IsValid Then Exit Sub
        Dim txtCurrentValue As TextBox
        For i As Integer = 0 To dgDoneOnValue.Rows.Count - 1
            txtCurrentValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtCurrentValue"), TextBox)
            With mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
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
        DataBindGrid()
        'upnlDoneOnValueGrid.Update()
        upnlCurrentValueGrid.Update()
    End Sub
    Protected Sub txtExtensionValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim txtExtensionValue As TextBox
        For i As Integer = 0 To mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Count - 1
            txtExtensionValue = CType(Me.dgDoneOnValue.Rows(i).FindControl("txtExtensionValue"), TextBox)

            With mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods
                .Item(i).ExtensionValue = Trim(txtExtensionValue.Text)
            End With
        Next
        DataBindGrid()
        'upnlDoneOnValueGrid.Update()
        upnlCurrentValueGrid.Update()
    End Sub
    Private Sub txtDoneOnDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDoneOnDate.TextChanged
        If IsPostBack Then   'Added Code on May,29,2007
            If CStr(mAssemblyMonitorInspStatus.DoneOn.ToString) <> "" And txtDoneOnDate.Text <> "" Then
                If DateDiff(DateInterval.Day, SmartDate.StringToDate(mAssemblyMonitorInspStatus.DoneOn.ToString), SmartDate.StringToDate(txtDoneOnDate.Text)) <> 0 Then
                    'If DateDiff(DateInterval.Day, SmartDate.StringToDate(mAssemblyMonitorInspStatus.DoneOn.ToString), SmartDate.StringToDate(calDoneOn.Text)) > 0 Then
                    '    Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.DoneOnDate, SIMsgBox.Message_text.DoneOnDate, "Compliance record only upto " & mAssemblyMonitorInspStatus.DoneOn.ToString & " can be entered through Assembly Installation screen", MsgBoxStyle.OKOnly)
                    '    msg1.ReplacePage = "wfComplyAssemblyMonitorInspStatus_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                    '    msg1.Show()
                    '    Exit Sub
                    'End If
                    Dim clnAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = mAssemblyMonitorInspStatus.Clone
                    If Session("From") = 0 Then 'New Record
                        NewRecord(Guid.Empty, txtDoneOnDate.Text)
                    Else
                        EditRecord(Guid.Empty, txtDoneOnDate.Text, False)
                    End If
                    SetFromClone(clnAssemblyMonitorInspStatus)
                    'DataBindGrid()
                    Session.Remove("mLog") 'Added by Saylee on 9th-Oct-2009
                    'Added By Utkarsh On 19-Mar-2012 FOR Link Maintenance
                    If AppSettings("LinkMaintenance") = "True" Then
                        mLinkMaintenanceList = Session("mLinkMaintenanceList")
                        If Not mLinkMaintenanceList Is Nothing Then
                            If mLinkMaintenanceList.Count > 0 Then
                                ShowLinkedMaintenaceActivity()
                                dgMultiComplianceList.DataBind()
                            End If
                        End If
                    End If
                    'End
                    SetGridObjectFromObject()
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
        If (Not User.IsInRole("AssemblyInspectionsNew") And mAssemblyMonitorInspStatus.IsNew) Or (Not User.IsInRole("AssemblyInspectionsEdit") And Not mAssemblyMonitorInspStatus.IsNew) Then
            'Changed by Vikrant on 28-July-2011
            mMonitorInfo = txtMonitorInspType.Text
            mMonitorType = txtMonitorType.Text
            mModel = mAssemblyStatus.ModelName
            mSerialNo = mAssemblyStatus.Assembly.SerialNo
            mInspectionDetail = "Model : " + mModel + " Serial No : " + mSerialNo + " Monitor Info. : " + mMonitorInfo + " Monitor Type : " + mMonitorType
            MarkLog(Util.Action.Save, "AssemblyInspections", User.Identity.Name & " is not Authorized User to save " & mInspectionDetail, Util.ErrorType.HandledError, Guid.Empty, EventLogID)
            MSGBoxCtrl.show(MSGBox.Message_title.Authorization, MSGBox.Message_text.Authorization, "", MsgBoxStyle.OkOnly, "")
            Exit Sub
        End If

        If IsValid Then

            'Code for OverDue 'Added by Saylee on 26-Mar-2019 for ALL26032019
            If Not mPrevAssemblyMonitorInspStatus.ModelMonitorInsp.MonitorTypeID = 3 Then  'No Frequency record not be checked for OverDue
                Dim DueString As String = ""
                DueString = CustomeValidateGridValuesForOverDue()
                If DueString <> "" Then
                    MSGBoxCtrl.show("Alert!!!", "You are about to save Over Due Compliance, " + DueString, "Do you want to continue?", MsgBoxStyle.YesNo, "OverDue")
                    Session("DueString") = DueString
                    Exit Sub
                End If
            End If '*********************************************************************************
            'Added By Prashant 19-Nov-2019 Alert if user is complying on same date ALL19112019
            If mPrevAssemblyMonitorInspStatus.DoneOn.ToString <> "" Then
                If (CDate(txtDoneOnDate.Text) <= CDate(mPrevAssemblyMonitorInspStatus.DoneOn) And Session("From") <> 1) Then
                    MSGBoxCtrl.show("Alert!!!", "Current compliance date is less than or equal to last compliance date ", "Do you want to continue?", MsgBoxStyle.YesNo, "ComplyOnSameDate")
                    Exit Sub
                End If
                'If CDate(txtDoneOnDate.Text) > CDate(mPrevAssemblyMonitorInspStatus.DoneOn) Then
                '    MSGBoxCtrl.show("Alert!!!", "Current compliance date is greater than last compliance date or today date  ", "Do you want to continue?", MsgBoxStyle.YesNo, "ComplyOnSameDate")
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
                            Dim Result As Boolean
                            SetLinkedMaintenanceGridObject()
                            LicenseNo = IIf(Session("LicenseNo") Is Nothing, "", Session("LicenseNo"))
                            Dim EmployeeID As String = IIf(Session("EmpID") Is Nothing, "", Session("EmpID"))
                            EmployeeID = EmployeeID.ToString.TrimEnd(",")

                            EmpName = IIf(Session("EmpName") Is Nothing, "", Session("EmpName"))
                            EmployeeID = EmployeeID.ToString.TrimEnd(",")
                            LicenseNo = LicenseNo.ToString.TrimEnd(",")
                            EmpName = EmpName.ToString.TrimEnd(",")
                            Dim LinkMaintenanceEvents As LinkedMaintenanceActivityEvents = New LinkedMaintenanceActivityEvents
                            LinkMaintenanceEvents.AssemblyLogInfo = "Assembly Inspection : " & mInspectionDetail 'setting Mark Log Detail ...
                            Result = LinkMaintenanceEvents.SaveLinkedMaintenanceActivies(mMultiComplianceList, mAssemblyMonitorInspStatus.DoneWONo, txtDoneOnDate.Text, mMachineMaintenance.LogID, mMachine.HourType, mMachine.ID, mAssemblyMonitorInspStatus.AssemblyID, PeriodValues, mAssemblyMonitorInspStatus.DoneRemark, LicenseNo, EmployeeID.ToString, EmpName, Trim(txtPlace.Text))

                            Session.Remove("EmpID")
                            Session.Remove("LicenseNo")
                            Session.Remove("EmpName")
                            If LinkMaintenanceEvents.ErrorStr.Length > 0 Then
                                'Dim msg As New SIMsgBox(Page, SIMsgBox.Message_title.SaveAlert, SIMsgBox.Message_text.saveAlert, LinkMaintenanceEvents.ErrorStr, MsgBoxStyle.OKOnly)
                                'msg.ReplacePage = "wfComplyAssemblyMonitorInspStatus_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2")
                                'Commented & Added By Vikrant On 06-Aug-2013 For ALL01082013
                                'lblAlertTitle.Text = "Link Maintenance Alert !"
                                'lblAlertMessage.Text = LinkMaintenanceEvents.ErrorStr
                                'ClientScript.RegisterStartupScript(Me.GetType(), "OpenAlertMessage", "<script type='text/javascript'>OpenAlert();</script>")
                                Dim title As String = "Link Maintenance Alert !"
                                Dim message As String = LinkMaintenanceEvents.ErrorStr
                                MSGBoxCtrl.show(title, message, "", MsgBoxStyle.OkOnly, "")
                                'End
                                Exit Sub
                            Else
                                MSGBoxCtrl.show("Alert !", "<BR>Other Maintenance Activity(s) linked with this maintenance activity.To Edit/Delete individual Maintenance Activity go to respective activity.", "", MsgBoxStyle.OkOnly, "LMAlert")
                                Exit Sub
                            End If
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
        Session("mAssemblyStatusId") = mAssemblyMonitorInspStatus.AssemblyStatusID.ToString
        Session("mAssemblyID") = mAssemblyStatus.AssemblyID.ToString
        Session("mDoneOn") = CStr(IIf(txtDoneOnDate.Text = "", Today.Date.ToShortDateString, txtDoneOnDate.Text))
        'Added by Vikrant on 14-Mar-2016 for ALL11032016
        If mAssemblyStatus.InstalledOn.ToString <> "" Then
            If CDate(mAssemblyMonitorInspStatus.DoneOn) <= CDate(mAssemblyStatus.InstalledOn) Then 'if Compliance date is same or less than Assembly Inst. Date
                Dim mFirstLogDetailAfterAssemblyInstallation As FirstLogDetailAfterAssemblyInstallation = FirstLogDetailAfterAssemblyInstallation.GetFirstLogDetailAfterAssemblyInstallation(mAssemblyStatus)
                Session("mFirstLogDetailAfterAssemblyInstallation") = mFirstLogDetailAfterAssemblyInstallation
            End If
        End If
        'End
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenSelectLogWindow", "OpenSelectLogWindow();", True)
        'Response.Redirect("wfSelectLog_Ajax.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&BackPage6=wfComplyAssemblyMonitorInspStatus_Ajax.aspx" & "&FromType=3&DoneOn=" & CStr(IIf(txtDoneOnDate.Text = "", Today.Date.ToShortDateString, txtDoneOnDate.Text)) & "&MachineId=" & mAssemblyStatus.MachineID.ToString & "&AssemblyStatusID=" & mAssemblyMonitorInspStatus.AssemblyStatusID.ToString & "&AssemblyID=" & mAssemblyStatus.AssemblyID.ToString)
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Changed by Vikrant on 28-July-2011
        MarkLog(Util.Action.Close, "AssemblyInspections", "", Util.ErrorType.NoError, Guid.Empty, EventLogID)
        RemoveSession()
        Session.Remove("FromLog")
        Session.Remove("IsBackFromCompliance") 'Added By Vikrant On 03-Jun-2016 For ALL03062016
        Response.Redirect(Request.QueryString("GChildPage2") & "?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1"))
    End Sub
    'Added By Utkarsh On 01-Jun-2012 FOR Link Maintenance
    Private Sub dgMultiComplianceList_Sorting(ByVal source As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles dgMultiComplianceList.Sorting
        mMultiComplianceList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
        Session("mMultiComplianceList") = mMultiComplianceList
        dgMultiComplianceList.DataSource = mMultiComplianceList
        dgMultiComplianceList.DataBind()
    End Sub
    'End
    'Added by Vikrant On 25-Nov-2014
    Private Sub hdnBtnFileUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnFileUpload.Click
        mAssemblyMonitorInspStatus.IsAttachmentAdded = True
        ControlVisibilityForAttachment()
        'Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
        upnlFileupload.Update()
    End Sub
    Private Sub btnDelAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelAttach.Click
        Dim fileSize1 As Integer = 0
        Dim file1(fileSize1) As Byte

        If mAssemblyMonitorInspStatus.IsAttachmentAdded And mFileAttach Is Nothing Then
            mFileAttach = FileAttach.GetAttachment(mAssemblyMonitorInspStatus.ID)
            Session("mFileAttach") = mFileAttach
        End If

        mFileAttach.ImageFile = file1
        mFileAttach.Size = 0

        ImageButton1.Visible = False
        btnDelAttach.Enabled = False
        IsAttachmentDeleted = True
        mAssemblyMonitorInspStatus.IsAttachmentAdded = False
        Session("IsAttachmentDeleted") = IsAttachmentDeleted
    End Sub
    Private Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        ViewImage()
    End Sub
    Private Sub btnSelectFile_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectFile.ServerClick
        If mAssemblyMonitorInspStatus.IsAttachmentAdded Then
            mFileAttach = FileAttach.GetAttachment(mAssemblyMonitorInspStatus.ID)
        Else
            mFileAttach = FileAttach.NewAttachment(Guid.NewGuid, mAssemblyMonitorInspStatus.ID)
        End If
        Session("mFileAttach") = mFileAttach
    End Sub
    'End
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub hdnBtnSelectLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnSelectLog.Click
        If CType(Session("FromLog"), Boolean) = True Then
            Dim clnAssemblyMonitorInspStatus As AssemblyMonitorInspStatus = mAssemblyMonitorInspStatus.Clone
            If Session("From") = 0 Then 'New Record
                NewRecord(New Guid(LogID.ToString), txtDoneOnDate.Text)
            Else
                EditRecord(New Guid(LogID.ToString), txtDoneOnDate.Text, False)
            End If
            SetFromClone(clnAssemblyMonitorInspStatus)
            'DataBindGrid()
            Session.Remove("FromLog")

            'Added by Saylee on 9th-Oct-2009
            Dim mLog As Log
            mLog = Log.GetLog(New Guid(LogID.ToString))
            Session("mLog") = mLog
            SetGridObjectFromObject()
            DataBindGrid()
            ControlVisibility()
            SetTitle()
            upnlCurrentValueGrid.Update()
            upnlDoneOnValueGrid.Update()
            '===========================================
        Else
            Session.Remove("mLog") 'Added by Saylee on 10-Jan-2014
        End If
       
    End Sub
    'MLNo
    Private Sub imgbtnEmployeeLicence_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgbtnEmployeeLicence.Click
        If IsValid Then
            SetObject()
            Session("mMaintenanceID") = mAssemblyMonitorInspStatus.ID
            mMaintenanceDoneByEmployees = mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees
            Session("mMaintenanceDoneByEmployees") = mMaintenanceDoneByEmployees
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "AddEmployeeLicNo", "AddEmployeeLicNo();", True)
        Else
            upnlValidationSummary.Update()
        End If

    End Sub
    Private Sub hdnBtnMaintDoneBy_Click(sender As Object, e As System.EventArgs) Handles hdnBtnMaintDoneBy.Click
        Session("EmpID") = ""
        Session("LicenseNo") = ""
        Session("EmpName") = ""
        For i As Integer = 0 To mMaintenanceDoneByEmployees.Count - 1
            Dim ID As Guid = mMaintenanceDoneByEmployees(i).ID
            If Not mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Add(mMaintenanceDoneByEmployees(i))
            ElseIf mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Contains(ID) Then
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(ID).LicenceNo = mMaintenanceDoneByEmployees(i).LicenceNo
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(ID).RequiredManHours = mMaintenanceDoneByEmployees(i).RequiredManHours
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(ID).EmployeeID = mMaintenanceDoneByEmployees(i).EmployeeID
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(ID).EmployeeName = mMaintenanceDoneByEmployees(i).EmployeeName
            End If
            Session("EmpID") = Session("EmpID") + mMaintenanceDoneByEmployees(i).EmployeeID.ToString + ","
            Session("LicenseNo") = Session("LicenseNo") + mMaintenanceDoneByEmployees(i).LicenceNo.ToString + ","
            Session("EmpName") = Session("EmpName") + mMaintenanceDoneByEmployees(i).EmployeeName.ToString + ","
        Next

        For j As Integer = 0 To mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Count - 1
            If Not mMaintenanceDoneByEmployees.Contains(mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(j).ID) Then
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Remove(mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(j).ID, "")
            End If
        Next
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
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
            If mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(0).EmployeeID = DoneByID
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(0).LicenceNo = LicenseNo
                If Not mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Count > 1 Then 'If Condition added by Vikrant On 15-Apr-2021 to solve issue:Hours getting added for multiple licence no and if first licence no changed
                    mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(0).RequiredManHours = txtRequiredManHours.Text
                End If
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(0).EmployeeName = EmpName
            Else
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Add(mAssemblyMonitorInspStatus.ID, 6, DoneByID, LicenseNo, txtRequiredManHours.Text, EmpName)
            End If
          
        Else
            If mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Count > 0 Then
                mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.RemoveAt(0)
            End If
        End If
        Session("mAssemblyMonitorInspStatus") = mAssemblyMonitorInspStatus
        BindLicenceNo()
        SetLicenceCount()
        txtRequiredManHours.DataBind()
        upnlMonitoringStatusDetails.Update()
    End Sub
    Protected Sub txtRequiredManHours_TextChanged(sender As Object, e As System.EventArgs)
        If mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees.Count > 0 Then
            mAssemblyMonitorInspStatus.MaintenanceDoneByEmployees(0).RequiredManHours = txtRequiredManHours.Text
            upnlMonitoringStatusDetails.Update()
        End If
    End Sub
    'End
    'Revise Activity
    Private Sub btnRevise_Click(sender As Object, e As System.EventArgs) Handles btnRevise.Click
        MSGBoxCtrl.show("Alert!", "You are about to Revise Model Activity.After revision of model activity this Status will become Not Applicable.", "Do you want to continue?", MsgBoxStyle.YesNo, "ReviseActivity")
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
        Dim Rpt As New crDetComplyAssemblyMonitorStatus
        Dim ds As New dsCommon
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ReportDetails As New rptStatusList

        'For Current Value Grid
        Dim TotalCount As Integer
        Dim LHCount As Integer
        Dim RHCount As Integer
        LHCount = 5
        RHCount = Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Count
        If LHCount > RHCount Then
            TotalCount = LHCount
        Else
            TotalCount = RHCount
        End If

        Dim temp As Integer
        temp = 0
        If temp < RHCount Then
            ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Insp Type", _
                  txtMonitorInspType.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                  dgCurrentValue.Columns.Item(1).HeaderText, dgCurrentValue.Columns.Item(2).HeaderText, _
                    , dgCurrentValue.Columns.Item(3).HeaderText, , dgCurrentValue.Columns.Item(4).HeaderText))
        Else
            ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Monitor Type", _
                            txtMonitorInspType.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                                  "", "", , "", , "", , , ))
        End If
        Dim I As Integer
        For I = 0 To TotalCount - 1
            If I = 0 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Monitor Type", _
                             txtMonitorType.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
             CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).PeriodUnitName, String), _
             CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).FrequencyValueFormatted, String), , _
             CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).ElapsedValueFormatted, String), , _
             CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).RemainingValueFormatted, String)))


                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "ATA Chapter", _
                            txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , "", , , ))
                End If
            ElseIf I = 1 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "ATA Chapter", _
                             txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
             CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).PeriodUnitName, String), _
             CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).FrequencyValueFormatted, String), , _
             CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).ElapsedValueFormatted, String), , _
             CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).RemainingValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "ATA Chapter", _
                            txtATAChapter.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , "", , , ))
                End If
            ElseIf I = 2 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Reference", _
                             txtReference.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
            CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).PeriodUnitName, String), _
             CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).FrequencyValueFormatted, String), , _
             CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).ElapsedValueFormatted, String), , _
             CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).RemainingValueFormatted, String)))

                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Reference", _
                                txtReference.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                                "", "", , "", , "", , , ))
                End If
            ElseIf I = 3 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Description", _
                                   txtDescription.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
             CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).PeriodUnitName, String), _
             CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).FrequencyValueFormatted, String), , _
             CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).ElapsedValueFormatted, String), , _
             CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).RemainingValueFormatted, String)))

                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "Description", _
                                    txtDescription.Text, , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , "", , , ))
                End If
            ElseIf I = 4 Then
                If I < RHCount Then
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "", _
                                    "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
             CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).PeriodUnitName, String), _
             CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).FrequencyValueFormatted, String), , _
             CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).ElapsedValueFormatted, String), , _
             CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).RemainingValueFormatted, String), , lblNote.Text, ))

                Else
                    ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "", _
                                        "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
                            "", "", , "", , "", , lblNote.Text, ))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 0, "Monitoring Details", "", _
                                         "", , , , , , , , , , , , , , , , , "Elapsed and Remaining Values", _
             CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).PeriodUnitName, String), _
             CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).FrequencyValueFormatted, String), , _
             CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).ElapsedValueFormatted, String), , _
             CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(I).RemainingValueFormatted, String), , lblNote.Text, ))
            End If
        Next

        'For Done On Value Grid
        Dim TotalCount1 As Integer
        Dim LHCount1 As Integer
        Dim RHCount1 As Integer
        LHCount1 = 6
        RHCount1 = Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Count
        If LHCount1 > RHCount1 Then
            TotalCount1 = LHCount1
        Else
            TotalCount1 = RHCount1
        End If

        Dim temp1 As Integer
        temp1 = 0
        If temp1 < RHCount1 Then
            ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Done On", _
                                             txtDoneOnDate.Text, , , , , , , , , , , , , , , , , lblAssemblyValue.InnerText, _
                                           dgDoneOnValue.Columns.Item(0).HeaderText, dgDoneOnValue.Columns.Item(1).HeaderText, _
                         , dgDoneOnValue.Columns.Item(2).HeaderText, , dgDoneOnValue.Columns.Item(3).HeaderText, dgDoneOnValue.Columns.Item(4).HeaderText, , ))
        Else
            ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Done On", _
                            txtDoneOnDate.Text, , , , , , , , , , , , , , , , , lblAssemblyValue.InnerText, _
                                  "", "", , "", , "", "", , ))
        End If


        Dim m As Integer
        For m = 0 To TotalCount1 - 1
            If m = 0 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Work Order No.", _
                    txtWorkOrderNo.Text, , , , , , , , , , , , , , , , , lblAssemblyValue.InnerText, _
                 CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).CurrentValueFormatted, String), , _
                CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).DueOnValueFormatted, String), CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , ))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Work Order No.", _
                            txtWorkOrderNo.Text, , , , , , , , , , , , , , , , , lblAssemblyValue.InnerText, _
                                "", "", , "", , "", "", , ))
                End If
            ElseIf m = 1 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "License No.", _
                    mAssemblyMonitorInspStatus.AllLicenceNosWithEmpName, , , , , , , , , , , , , , , , , lblAssemblyValue.InnerText, _
                CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).CurrentValueFormatted, String), , _
                CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).DueOnValueFormatted, String), CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , ))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "License No.", _
                            mAssemblyMonitorInspStatus.AllLicenceNosWithEmpName, , , , , , , , , , , , , , , , , lblAssemblyValue.InnerText, _
                                "", "", , "", , "", "", , ))
                End If
            ElseIf m = 2 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Place ", _
                    txtPlace.Text, , , , , , , , , , , , , , , , , lblAssemblyValue.InnerText, _
                CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).CurrentValueFormatted, String), , _
                CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).DueOnValueFormatted, String), CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , ))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Place ", _
                            txtPlace.Text, , , , , , , , , , , , , , , , , lblAssemblyValue.InnerText, _
                                "", "", , "", , "", "", , ))
                End If
            ElseIf m = 3 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Actual Man Hours", _
                     txtRequiredManHours.Text, , , , , , , , , , , , , , , , , lblAssemblyValue.InnerText, _
                CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).CurrentValueFormatted, String), , _
                CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).DueOnValueFormatted, String), CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , ))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Actual Man Hours", _
                            txtRequiredManHours.Text, , , , , , , , , , , , , , , , , lblAssemblyValue.InnerText, _
                                "", "", , "", , "", "", , ))
                End If
            ElseIf m = 4 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Remark", _
                     txtRemark.Text, , , , , , , , , , , , , , , , , lblAssemblyValue.InnerText, _
                        CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).CurrentValueFormatted, String), , _
                CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).DueOnValueFormatted, String), CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), , ))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "Remark", _
                            txtRemark.Text, , , , , , , , , , , , , , , , , lblAssemblyValue.InnerText, _
                                "", "", , "", , "", "", , ))
                End If
            ElseIf m = 5 Then
                If m < RHCount1 Then
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "", _
                    "", , , , , , , , , , , , , , , , , lblAssemblyValue.InnerText, _
                CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).CurrentValueFormatted, String), , _
                CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).DueOnValueFormatted, String), CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), "Please Note: Started On/Current Values/Due On values for Days/Months/Years will be in Dates.  Extension Value for Calendar period should be entered in Days only.", ))
                Else
                    ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "", _
                    "", , , , , , , , , , , , , , , , , lblAssemblyValue.InnerText, _
                           "", "", , "", , "", "", "Please Note: Started On/Current Values/Due On values for Days/Months/Years will be in Dates.  Extension Value for Calendar period should be entered in Days only.", ))
                End If
            Else
                ReportDetails.Add(New rptStatus(, 1, "Monitoring Status Details", "", _
                                   "", , , , , , , , , , , , , , , , , lblAssemblyValue.InnerText, _
                CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).PeriodUnitName, String), _
                CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).CurrentValueFormatted, String), , _
                CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).ExtensionValueFormatted, String), , CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).DueOnValueFormatted, String), CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(m).AssemblyDueOnValueFormattedByAirFrame, String), "Please Note: Started On/Current Values/Due On values for Days/Months/Years will be in Dates.  Extension Value for Calendar period should be entered in Days only.", ))
            End If
        Next

        '***********************************************************************************************************************
        'For Document Details
        Dim TotalCount2 As Integer
        Dim LHCount2 As Integer
        Dim RHCount2 As Integer
        LHCount2 = 3
        RHCount2 = Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods.Count
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
            dgDoneOnValue.Columns.Item(1).HeaderText, dgDoneOnValue.Columns.Item(2).HeaderText, "Extension Date ", _
            dgDoneOnValue.Columns.Item(3).HeaderText, txtExtensionDate.Text, dgDoneOnValue.Columns.Item(4).HeaderText, _
            "", ""))
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
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).FrequencyValueFormatted, String), "Approval Remark", _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).DoneOnValueFormatted, String), txtApprovalRemark.Text, _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).CurrentValueFormatted, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Page No.", _
                        txtPageNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                        "", txtApprovalRemark.Text, , "", , "", ""))
                End If
            ElseIf n = 1 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Book No.", _
                    txtBookNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).DoneOnValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).CurrentValueFormatted, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Book No.", _
                        txtBookNo.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    "", "", , "", , "", ""))
                End If
            ElseIf n = 2 Then
                If n < RHCount2 Then
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Source Doc ", _
                    txtSourceDoc.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).PeriodUnitName, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).FrequencyValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).DoneOnValueFormatted, String), , _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).CurrentValueFormatted, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).ExtensionValueFormatted, String), _
                    CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).DueOnValueFormatted, String)))
                Else
                    ReportDetails.Add(New rptStatus(, 2, "Document Details", "Source Doc ", _
                        txtSourceDoc.Text, , , , , , , , , , , , , , , , , "Extension Details", _
                    "", "", , "", , "", ""))
                End If

            Else
                ReportDetails.Add(New rptStatus(, 2, "Document Details", "", _
                "", , , , , , , , , , , , , , , , , lblAssemblyValue.InnerText, _
                CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).PeriodUnitName, String), _
                CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).FrequencyValueFormatted, String), , _
                CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).DoneOnValueFormatted, String), , _
                CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).CurrentValueFormatted, String), _
                CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).ExtensionValueFormatted, String), _
                CType(Me.mAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriods(n).DueOnValueFormatted, String), lblNote1.Text))
            End If
        Next
        '***********************************************************************************************************************

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        mCompanyDetail.WebSite, "Comply Assembly Inspection Status Detail Report", lblTitle.Text, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))

        da.Fill(ds, ReportDetails)
        da.Fill(ds, Report)
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        Rpt.SetDataSource(ds)
        Session("CrystalReport") = Rpt
        'MarkLog(Util.Action.Print, "ComplyAssemblyMonitorInspStatus", mAssemblyInfo + " -> " + "Comply Assembly Monitor Inspection Status Detail Report", Util.ErrorType.NoError, mAssemblyMonitorInspStatus.ID)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    End Sub
    'Added by Shital on 18-May-2021
    Private Sub lnkPrintLogBookEntry_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkPrintLogBookEntry.Click  'Added By Prashant On 7-May-2021 ALL07052021
        Dim RptCommonHistory As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mLogEntryFormat As New LogEntryFormat
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsReportHistoryList
        Dim mCompanyDetail As New CompanyDetail

        RptCommonHistory = New crptLogEntryFormat

        mLogEntryFormat = LogEntryFormat.GetHistoryList(mAssemblyMonitorInspStatus.DoneOn, mAssemblyMonitorInspStatus.DoneOn, "", mAssemblyStatus.AssemblyTypeName, _
                                                        mAssemblyStatus.ModelName, mAssemblyStatus.Assembly.SerialNo, "", "", "", "", _
                                                        mAssemblyStatus.MachineID.ToString, True, False, IsRemoved:=False, IsInstalled:=True, _
                                                        IsComplied:=False, AssemblyID:=mAssemblyStatus.AssemblyID.ToString, IsLogNo:=True, _
                                                        IsLogPageNo:=False, IsFlightNo:=False, IsMELRequired:=False, IsMaintenanceActivityRequired:=False, _
                                                        AssemblyTypeID:=mAssemblyStatus.AssemblyTypeID, CompStatusID:=mAssemblyStatus.ID.ToString, _
                                                        ShowService:=False, ShowDir:=False, ShowInsp:=True, AssemblyMonitorInspStatusID:=mAssemblyMonitorInspStatus.ID.ToString)
        If mLogEntryFormat.Count = 0 Then
            Exit Sub
        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
           mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
           mCompanyDetail.WebSite, "LOG BOOK ENTRY", "", mAssemblyMonitorInspStatus.DoneOnFormatted, Machine.GetMachine(mAssemblyStatus.MachineID).RegNo, _
           mAssemblyStatus.ModelName + "-" + mAssemblyStatus.Assembly.SerialNo, IIf(mAssemblyStatus.AssemblyTypeName.Equals("Airframe"), "AIRCRAFT", mAssemblyStatus.AssemblyTypeName.ToUpper), _
           AppSettings("Product Version"), AppSettings("SINote"), _
           "AVERAGE FUEL CONSUMPTION________LTR./HR & AVERAGE OIL CONSUMPTION________LTR./HR SINCE LAST SMI DONE.  BOTH THE FIGURES ARE BELOW THE ALERT VALUE.", _
           "True", mAssemblyMonitorInspStatus.DoneOnFormatted, "", AppSettings("Logo"))


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