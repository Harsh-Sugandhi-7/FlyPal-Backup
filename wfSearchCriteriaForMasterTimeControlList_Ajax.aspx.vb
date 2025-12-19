'AJAX Conversion by Vikrant

Imports System.Linq
Imports System.Net
Imports System.Collections.Generic
Imports System.Text

Public Class wfSearchCriteriaForMasterTimeControlList_Ajax
    Inherits System.Web.UI.Page


#Region " Enumeration "
    Enum MachineMaintenanceActivity
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
    Dim mPerDayLimits As PerDayLimits
    Dim ReportStatusList As New rptStatusList
    Dim mMachineList As MachineList
    Dim ReportMaintenanceDetails As New ReportMaintenanceDetailList
    Dim AOdate As String
    Dim AOnDate As String
    Dim Average As String
    Dim Aircraft As String
    Dim Report As Integer = 1
    Dim Periodcount As Integer
    Dim MachineName As String
    Dim AsonDate As String
    Dim Type As Integer = 1
    Dim AssemblyID As Guid
    Dim Count As Integer
    Private ATAChapter As String
    Private RegNo As String
    Private AssemblyType As String
    Private Model As String
    Private AssemblySerialNo As String
    Private PartNo As String
    Private CompSerialNo As String
    Private Position As String
    Private MonitorTypeCode As String
    Private Note As String
    Private Description, ModificationNumber, SerialNoPostion, DoneOnDate As String
    Private SerialNo As String
    Private EstimatedDate As String
    Private Freq1 As String
    Private Freq2 As String
    Private Freq3 As String
    Private ElapsedTime As String
    Private ElapsedTime1 As String
    Private ElapsedTime2 As String
    Private SinceNew As String
    Private SinceNew1 As String
    Private SinceNew2 As String
    Private RemainingTime As String
    Private RemainingTime1 As String
    Private RemainingTime2 As String
    Private DueAsof As String
    Private DueAsof1 As String
    Private DueAsof2 As String
    Private DoneAt As String
    Private DoneAt1 As String
    Private DoneAt2 As String
    Private AssemblyModel As String
    Private MaintenanceEvent As String
    Private MinimumRemainingValue As Decimal
    Private AssemblyTypeID As Integer
    Private percent As String
    Dim mAssemblyList As AssemblyList
    Dim AssemblyName As String
    Dim Assembly1 As String
    Dim mServiceTypeList As PartMonitorServiceTypeList
    Dim mInspectionTypeList As ModelMonitorInspTypeList
    Dim mModificationTypeList As ModelMonitorModTypeList
    Dim Extension As String
    Dim Extension1 As String
    Dim Extension2 As String
    Dim ExtensionDate As String
    Dim ApprovalRemark As String
    Dim RequiredManHours As String
    Dim Customer As String
    Dim Remark, MaintenanceTypeName As String
    Dim Code As String
    Dim StatusMasterID As Guid
    Dim DocumentTypeForID, MaintenanceTypeID As Integer
    Dim AssemblyDueAsof As String
    Dim AssemblyDueAsof1 As String
    Dim AssemblyDueAsof2 As String
    Dim IsSerSelect As Boolean = False
    Dim IsModSelect As Boolean = False
    Dim IsInsSelect As Boolean = False
    Dim ServiceTypeID(50) As Integer
    Dim InspectionTypeID(50) As Integer
    Dim ModificationTypeID(50) As Integer
    Dim mMachineNameValueList As MachineNameValueList
    Dim mSearchCriteriaForEventLog As String = String.Empty
    Dim EventLogID As Guid
    'Added By Vikrant On 12-Feb-2014 For ALL12022014
    Dim AirframeDueAsof As String
    Dim AirframeDueAsof1 As String
    Dim AirframeDueAsof2 As String
    Dim PeriodLimt As String = String.Empty
    'End

    Dim InstalledAt As String
    Dim InstalledAt1 As String
    Dim InstalledAt2 As String
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    Private DoneAtAssembly As String
    Private IsExcel As Boolean = False
    Dim TaskNo As String = ""
    Dim ServicesShortName As String = ""
    Dim DirectiveShortName As String = ""
    Dim InspsShortName As String = ""
    Dim mLastAMPRef As LastMPDAMPRef 'Added by Ajay on 14-08-2023
    Dim AMPNo As String = ""
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mPerDayLimits = CType(Session("mPerDayLimits"), PerDayLimits)
        AOnDate = Session("AOnDate")
        Report = Session("Report")
        Type = Session("Type")
        mAssemblyList = CType(Session("mAssemblyList"), AssemblyList)
        mServiceTypeList = CType(Session("mServiceTypeList"), PartMonitorServiceTypeList)
        mInspectionTypeList = CType(Session("mInspectionTypesList"), ModelMonitorInspTypeList)
        mModificationTypeList = CType(Session("mModificationTypeList"), ModelMonitorModTypeList)
        mMachineNameValueList = Session("mMachineNameValueList")
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
    End Sub
    Private Sub SetSession()
        Session("mPerDayLimits") = mPerDayLimits
        Session("AOnDate") = AOnDate
        Session("Report") = Report
        Session("Type") = Type
        Session("mAssemblyList") = mAssemblyList
        Session("mServiceTypeList") = mServiceTypeList
        Session("mInspectionTypesList") = mInspectionTypeList
        Session("mModificationTypeList") = mModificationTypeList
        Session("mMachineNameValueList") = mMachineNameValueList
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mPerDayLimits")
        Session.Remove("AOnDate")
        Session.Remove("Report")
        Session.Remove("Type")
        Session.Remove("mAssemblyList")
        Session.Remove("mMachineNameValueList")
        Session.Remove("mServiceTypeList")
        Session.Remove("mInspectionTypesList")
        Session.Remove("mModificationTypeList")
    End Sub
    'Added By Vikrant On 12-Feb-2014 For ALL12022014
    Private Sub DataFieldBind()
        mPerDayLimits = PerDayLimits.GetPerDayLimits(New Guid(cmbAircraft.SelectedValue.ToString))
        gdPerDayLimit.DataSource = mPerDayLimits
        Session("mPerDayLimits") = mPerDayLimits
    End Sub
    Private Sub SetGridObject()
        Dim txtPerDatLimit As TextBox
        Dim i1 As Int32
        For i1 = 0 To Me.gdPerDayLimit.Rows.Count - 1
            txtPerDatLimit = CType(Me.gdPerDayLimit.Rows(i1).FindControl("txtLimitPerDay"), TextBox)
            'mPerDayLimits.Item(i1).PeriodLimit = CDec(Val(Trim(txtPerDatLimit.Text))) 'Commented by Saylee on 12-Nov-2012
            mPerDayLimits.Item(i1).PeriodLimit = Trim(txtPerDatLimit.Text)  'Added by Saylee on 12-Nov-2012
            PeriodLimt = PeriodLimt + ", " + Trim(txtPerDatLimit.Text)
        Next i1
        'Session("mPerDayLimits") = mPerDayLimits
    End Sub
    'End
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfSearchCriteriaForMasterTimeControlList_Ajax.aspx?" Then
            RemoveSession()
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub Display()
        lblAircraft1.Visible = True
        lblDateRangeFrom.Visible = True
        lblAssembly1.Visible = True
        upnlSearchingCriteria.Update()
    End Sub

    Private Sub SetValues()
        If (cmbAircraft.SelectedItem.Text = "(All)") Or (cmbAircraft.SelectedItem.Text = "<SELECT>") Then
            MachineName = "{00000000-0000-0000-0000-000000000000}"
            AssemblyName = "{00000000-0000-0000-0000-000000000000}"
            Assembly1 = ""
            lblAssembly1.Text = ""
        Else
            MachineName = cmbAircraft.SelectedValue.ToString
            If cmbAssembly.SelectedItem.Text = "<SELECT>" Then
                AssemblyName = "{00000000-0000-0000-0000-000000000000}"
                Assembly1 = ""
                AssemblyType = "(All)"
                lblAssembly1.Text = "Assembly Name  : All"          'Added Code
            Else
                AssemblyType = mAssemblyList(cmbAssembly.SelectedIndex).AssemblyType
                AssemblyName = cmbAssembly.SelectedValue.ToString
                Assembly1 = cmbAssembly.SelectedItem.Text
                lblAssembly1.Text = "Assembly Name : " & Assembly1  'Added Code
            End If
        End If
        'Average = txtAvgMnths.Text
        If Not IsDate(txtFromDate.Text) Then
            AsonDate = ""
        Else
            AsonDate = txtFromDate.Text
        End If
        Aircraft = IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "")
        If AsonDate <> "" Then
            lblDateRangeFrom.Text = "As On Date : " & New SmartDate(txtFromDate.Text).FormattedText
        Else
            lblDateRangeFrom.Text = "As On Date : " & "All"
        End If

        lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "All")
        lblAvgMnths1.Text = "Average Months : " & IIf(Average <> "", Average, "All")
        lblPercent.Text = "Percent : " & IIf(percent <> "", percent, "All")

        'Set Service/Inspection/Directive checkbox list values
        'Service
        If chkService.Checked Then
            IsSerSelect = True
            ServiceTypeID = (From c As System.Web.UI.WebControls.ListItem In ListServiceType.Items
                             Where c.Selected = True
                             Select CInt(c.Value)).ToArray

        End If
        For i As Integer = 0 To mServiceTypeList.Count - 1
            If ServicesShortName = "" Then
                ServicesShortName = IIf(Not mServiceTypeList(i, "").CodeType Is Nothing, mServiceTypeList(i, "").CodeType, "")
            Else
                ServicesShortName = ServicesShortName + IIf(Not mServiceTypeList(i, "").CodeType Is Nothing, ", " + mServiceTypeList(i, "").CodeType, "")
            End If

        Next
        'Inspection
        If chkInspection.Checked Then
            IsInsSelect = True

            InspectionTypeID = (From c In ListInspectionType.Items
                                Where c.Selected = True
                                Select CInt(c.Value)).ToArray

        End If
        For i As Integer = 0 To mInspectionTypeList.Count - 1
            If InspsShortName = "" Then
                InspsShortName = IIf(Not mInspectionTypeList(i, "").CodeType Is Nothing, mInspectionTypeList(i, "").CodeType, "")
            Else
                InspsShortName = InspsShortName + IIf(Not mInspectionTypeList(i, "").CodeType Is Nothing, ", " + mInspectionTypeList(i, "").CodeType, "")
            End If
        Next
        'Directive
        If chkDirective.Checked Then
            IsModSelect = True
            ModificationTypeID = (From c In ListDirectiveType.Items
                         Where c.Selected = True
                        Select CInt(c.Value)).ToArray

        End If
        'End
        For i As Integer = 0 To mModificationTypeList.Count - 1
            If DirectiveShortName = "" Then
                DirectiveShortName = IIf(Not mModificationTypeList(i, "").CodeType Is Nothing, mModificationTypeList(i, "").CodeType, "")
            Else
                DirectiveShortName = DirectiveShortName + IIf(Not mModificationTypeList(i, "").CodeType Is Nothing, ", " + mModificationTypeList(i, "").CodeType, "")
            End If

        Next
        SetGridObject() 'Added By Vikrant On 12-Feb-2014 For ALL12022014
        mSearchCriteriaForEventLog = lblDateRangeFrom.Text + "," + lblAircraft1.Text + "," + lblAssembly1.Text + "," + IIf(chkAirframeDueAsOf.Checked, chkAirframeDueAsOf.Text, "")
    End Sub
    Public Function ReportDetail(Optional ByVal IsExcel As Boolean = False) As ReportMaintenanceDetailList
        Dim ObjMachine As MachineInfo
        Dim ObjAssemblyStatus As AssemblyStatusInfo
        Dim ObjCompStatus As CompStatusInfo
        Dim ObjAssemblyMonitorInspStatus As AssemblyMonitorInspStatusInfo
        Dim ObjAssemblyMonitorInspStatusPeriod As AssemblyMonitorInspStatusPeriodInfo
        Dim ObjAssemblyMonitorModStatus As AssemblyMonitorModStatusInfo
        Dim ObjAssemblyMonitorModStatusPeriod As AssemblyMonitorModStatusPeriodInfo
        Dim ObjAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatusInfo
        Dim ObjAssemblyMonitorServiceStatusPeriod As AssemblyMonitorServiceStatusPeriodInfo
        Dim ObjCompMonitorInspStatus As CompMonitorInspStatusInfo
        Dim ObjCompMonitorInspStatusPeriod As CompMonitorInspStatusPeriodInfo
        Dim ObjCompMonitorModStatus As CompMonitorModStatusInfo
        Dim ObjCompMonitorModStatusPeriod As CompMonitorModStatusPeriodInfo
        Dim ObjCompMonitorServiceStatus As CompMonitorServiceStatusInfo
        Dim ObjCompMonitorServiceStatusPeriod As CompMonitorServiceStatusPeriodInfo

        mMachineList = MachineList.GetMachineListMonitoringStatus(New SmartDate(AsonDate).Text, MachineName, , , , , , , , , , , True, , AssemblyName, IsAverageRequired:=True, ByPerDayLimit:=True, PerdayLimits:=mPerDayLimits, SkipIsForInventoryAircarft:=True)
        Dim LHLabel2 As String = ""
        Dim LHData2 As String = ""
        If Not cmbAircraft.SelectedItem.ToString = "<SELECT>" Then
            For Each ObjMachine In mMachineList

                For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                    Periodcount = ObjAssemblyStatus.AssemblyStatusPeriodList.Count()
                    LHLabel2 = ""
                    LHData2 = ""
                    For Count = 0 To Periodcount - 1
                        If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID <> 2 Then
                            LHLabel2 = CType(IIf(LHLabel2 = "", LHLabel2, LHLabel2 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                            LHData2 = CType(IIf(LHData2 = "", LHData2, LHData2 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyCurrentValue
                        End If
                    Next
                    If ObjAssemblyStatus.Position <> "" Then
                        SerialNoPostion = ObjAssemblyStatus.SerialNo + " (" + ObjAssemblyStatus.Position + ")"
                    Else
                        SerialNoPostion = ObjAssemblyStatus.SerialNo
                    End If
                    AssemblyID = ObjAssemblyStatus.AssemblyID
                    ReportStatusList.Add(New rptStatus(AssemblyID.ToString, ObjAssemblyStatus.AssemblyTypeID, , ObjAssemblyStatus.AssemblyType + " " + "Model", ObjAssemblyStatus.Model,
                         "Serial No.", SerialNoPostion, "Reg. No.", ObjMachine.RegNo, IIf(chkAirframeDueAsOf.Checked, "Next Due (Airframe Values)", "Next Due"), , , , , , , , , , , , , LHLabel2, LHData2))
                Next
            Next
        End If


        If IsSerSelect = True Then
            For i As Integer = 0 To ListServiceType.Items.Count - 1
                If ListServiceType.Items.Item(i).Selected Then
                    mMachineList = MachineList.GetMachineListMonitoringStatus(AsonDate, MachineName, , , , , , , , , , True, True, , AssemblyName, , , , , , , , , , , , , True, , , , , , , , , False, , False, , True, , , CInt(ListServiceType.Items.Item(i).Value), , , True, IsAverageRequired:=True, ByPerDayLimit:=True, PerdayLimits:=mPerDayLimits, SkipIsForInventoryAircarft:=True)
                    For Each ObjMachine In mMachineList
                        For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                            For Each ObjAssemblyMonitorServiceStatus In ObjAssemblyStatus.AssemblyMonitorServiceStatusList

                                'chkNotApplicable checkbox added by Saylee on 17-Feb-2017 to show Not Applicable Records when checked
                                If (ObjAssemblyMonitorServiceStatus.IsApplicable = True And chkNotApplicable.Checked = False) Or (chkNotApplicable.Checked) Then 'Added By Vikrant On 22-May-2014 For All22052014-1
                                    If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                                        ATAChapter = ObjAssemblyMonitorServiceStatus.ATACode.ToString
                                    Else
                                        ATAChapter = ObjAssemblyMonitorServiceStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorServiceStatus.ATANomenclature
                                    End If
                                    'Added By Prashant On 6-Jun-2023
                                    TaskNo = ObjAssemblyMonitorServiceStatus.TaskNo
                                    Description = ObjAssemblyMonitorServiceStatus.Description
                                    AssemblyModel = ObjAssemblyStatus.Model
                                    AssemblySerialNo = ObjAssemblyStatus.SerialNo
                                    Position = ObjAssemblyStatus.Position
                                    MonitorTypeCode = ObjAssemblyMonitorServiceStatus.Code
                                    EstimatedDate = ObjAssemblyMonitorServiceStatus.EstimatedDateFormatted
                                    MinimumRemainingValue = ObjAssemblyMonitorServiceStatus.MinimumRemainingValue
                                    AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID
                                    StatusMasterID = ObjAssemblyMonitorServiceStatus.ModelMonitorServiceID
                                    DocumentTypeForID = 0
                                    Remark = ObjAssemblyMonitorServiceStatus.DoneRemark
                                    Code = ObjAssemblyMonitorServiceStatus.ModelMonitorServiceCode
                                    DoneOnDate = ObjAssemblyMonitorServiceStatus.DoneOn
                                    MaintenanceTypeID = 1
                                    MaintenanceTypeName = "Service"
                                    Freq1 = ""
                                    Freq2 = ""
                                    Freq3 = ""
                                    ElapsedTime = ""
                                    ElapsedTime1 = ""
                                    ElapsedTime2 = ""
                                    RemainingTime = ""
                                    RemainingTime1 = ""
                                    RemainingTime2 = ""
                                    DueAsof = ""
                                    DueAsof1 = ""
                                    DueAsof2 = ""
                                    AssemblyDueAsof = ""
                                    AssemblyDueAsof1 = ""
                                    AssemblyDueAsof2 = ""
                                    SinceNew = ""
                                    SinceNew1 = ""
                                    SinceNew2 = ""
                                    DoneAt = ""
                                    DoneAt1 = ""
                                    DoneAt2 = ""
                                    MaintenanceEvent = ""
                                    Extension = ""
                                    Extension1 = ""
                                    Extension2 = ""
                                    'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                    AirframeDueAsof = ""
                                    AirframeDueAsof1 = ""
                                    AirframeDueAsof2 = ""
                                    'End

                                    'Added By Saylee On 5-Dec-2018 For ALL05122018
                                    InstalledAt = ""
                                    InstalledAt1 = ""
                                    InstalledAt2 = ""
                                    'End
                                    DoneAtAssembly = ""
                                    Dim mDoneAtAssembly As Period = New Period(1, DBNull.Value)

                                    For Each ObjAssemblyMonitorServiceStatusPeriod In ObjAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriodList
                                        If Report = 1 Then  'Portarait
                                            If ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 1 Then
                                                Freq1 = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue
                                                ElapsedTime = ObjAssemblyMonitorServiceStatusPeriod.AllElapsedValue

                                                If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True And ObjAssemblyMonitorServiceStatus.IsApplicable = True) Or (ObjAssemblyMonitorServiceStatus.IsApplicable = False) Then
                                                    DueAsof = ""
                                                    RemainingTime = ""
                                                    AssemblyDueAsof = ""
                                                    AirframeDueAsof = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                Else
                                                    DueAsof = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
                                                    RemainingTime = ObjAssemblyMonitorServiceStatusPeriod.RemainingValue
                                                    AssemblyDueAsof = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
                                                    AirframeDueAsof = ObjAssemblyMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                End If

                                                SinceNew = ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValue
                                                DoneAt = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
                                                Extension = ObjAssemblyMonitorServiceStatusPeriod.ExtensionValue
                                                InstalledAt = ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyMonitorServiceStatusPeriod.PeriodID, "").AssemblyInstallationValue
                                            End If
                                            If ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 2 Then
                                                Freq2 = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValueFormatted
                                                ElapsedTime1 = ObjAssemblyMonitorServiceStatusPeriod.AllElapsedValueFormatted

                                                If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True And ObjAssemblyMonitorServiceStatus.IsApplicable = True) Or (ObjAssemblyMonitorServiceStatus.IsApplicable = False) Then
                                                    DueAsof1 = ""
                                                    RemainingTime1 = ""
                                                    AssemblyDueAsof1 = ""
                                                    AirframeDueAsof1 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                Else
                                                    DueAsof1 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted
                                                    RemainingTime1 = ObjAssemblyMonitorServiceStatusPeriod.RemainingValueFormatted
                                                    AssemblyDueAsof1 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted
                                                    AirframeDueAsof1 = ObjAssemblyMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame   'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                End If

                                                SinceNew1 = ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValueFormatted
                                                DoneAt1 = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                Extension1 = ObjAssemblyMonitorServiceStatusPeriod.ExtensionValueFormatted
                                                InstalledAt1 = ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyMonitorServiceStatusPeriod.PeriodID, "").AssemblyInstallationValueFormatted
                                            End If
											'Added PeriodID=11 By Vikrant For ALL 21062012
											'If ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 3 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 4 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 5 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 6 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 7 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 8 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 9 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 12 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 13 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 14 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 15 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 11 Then
											'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
											If ObjAssemblyMonitorServiceStatusPeriod.PeriodID >= 3 Then
												If Freq3 = "" Then
													Freq3 = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue
													ElapsedTime2 = ObjAssemblyMonitorServiceStatusPeriod.AllElapsedValue

													If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True And ObjAssemblyMonitorServiceStatus.IsApplicable = True) Or (ObjAssemblyMonitorServiceStatus.IsApplicable = False) Then
														DueAsof2 = ""
														RemainingTime2 = ""
														AssemblyDueAsof2 = ""
														AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
													Else
														DueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
														RemainingTime2 = ObjAssemblyMonitorServiceStatusPeriod.RemainingValue
														AssemblyDueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
														AirframeDueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame   'Added By Vikrant On 12-Feb-2014 For ALL12022014
													End If

													SinceNew2 = ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValue
													DoneAt2 = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
													Extension2 = ObjAssemblyMonitorServiceStatusPeriod.ExtensionValue
													InstalledAt2 = ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyMonitorServiceStatusPeriod.PeriodID, "").AssemblyInstallationValue
												Else
													Freq3 = Freq3 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue
													ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.AllElapsedValue

													If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True And ObjAssemblyMonitorServiceStatus.IsApplicable = True) Or (ObjAssemblyMonitorServiceStatus.IsApplicable = False) Then
														DueAsof2 = ""
														RemainingTime2 = ""
														AssemblyDueAsof2 = ""
														AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
													Else
														DueAsof2 = DueAsof2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
														RemainingTime2 = RemainingTime2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.RemainingValue
														AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
														AirframeDueAsof2 = AirframeDueAsof2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame    'Added By Vikrant On 12-Feb-2014 For ALL12022014
													End If

													SinceNew2 = SinceNew2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValue
													DoneAt2 = DoneAt2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
													Extension2 = Extension2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.ExtensionValue
													InstalledAt2 = InstalledAt2 & vbCrLf & ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyMonitorServiceStatusPeriod.PeriodID, "").AssemblyInstallationValue
												End If
											End If

										Else
                                            If ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 2 Then
                                                If Freq3 = "" Then
                                                    Freq3 = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValueFormatted
                                                    ElapsedTime2 = ObjAssemblyMonitorServiceStatusPeriod.AllElapsedValueFormatted

                                                    If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True And ObjAssemblyMonitorServiceStatus.IsApplicable = True) Or (ObjAssemblyMonitorServiceStatus.IsApplicable = False) Then
                                                        DueAsof2 = ""
                                                        RemainingTime2 = ""
                                                        AssemblyDueAsof2 = ""
                                                        AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    Else
                                                        DueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted
                                                        RemainingTime2 = ObjAssemblyMonitorServiceStatusPeriod.RemainingValueFormatted
                                                        AssemblyDueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted
                                                        AirframeDueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame   'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    End If

                                                    SinceNew2 = ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValueFormatted
                                                    DoneAt2 = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                    Extension2 = ObjAssemblyMonitorServiceStatusPeriod.ExtensionValueFormatted
                                                    InstalledAt2 = ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyMonitorServiceStatusPeriod.PeriodID, "").AssemblyInstallationValueFormatted
                                                Else
                                                    Freq3 = Freq3 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.FrequencyValueFormatted
                                                    ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.AllElapsedValueFormatted

                                                    If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True And ObjAssemblyMonitorServiceStatus.IsApplicable = True) Or (ObjAssemblyMonitorServiceStatus.IsApplicable = False) Then
                                                        DueAsof2 = ""
                                                        RemainingTime2 = ""
                                                        AssemblyDueAsof2 = ""
                                                        AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    Else
                                                        DueAsof2 = DueAsof2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted
                                                        RemainingTime2 = RemainingTime2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.RemainingValueFormatted
                                                        AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted
                                                        AirframeDueAsof2 = AirframeDueAsof2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame   'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    End If

                                                    SinceNew2 = SinceNew2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValueFormatted
                                                    DoneAt2 = DoneAt2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                    Extension2 = Extension2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.ExtensionValueFormatted
                                                    InstalledAt2 = InstalledAt2 & vbCrLf & ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyMonitorServiceStatusPeriod.PeriodID, "").AssemblyInstallationValueFormatted
                                                End If
                                            Else
                                                If Freq3 = "" Then
                                                    Freq3 = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue
                                                    ElapsedTime2 = ObjAssemblyMonitorServiceStatusPeriod.AllElapsedValue

                                                    If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True And ObjAssemblyMonitorServiceStatus.IsApplicable = True) Or (ObjAssemblyMonitorServiceStatus.IsApplicable = False) Then
                                                        DueAsof2 = ""
                                                        RemainingTime2 = ""
                                                        AssemblyDueAsof2 = ""
                                                        AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    Else
                                                        DueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
                                                        RemainingTime2 = ObjAssemblyMonitorServiceStatusPeriod.RemainingValue
                                                        AssemblyDueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
                                                        AirframeDueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame   'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    End If

                                                    SinceNew2 = ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValue
                                                    DoneAt2 = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
                                                    Extension2 = ObjAssemblyMonitorServiceStatusPeriod.ExtensionValue
                                                    InstalledAt2 = ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyMonitorServiceStatusPeriod.PeriodID, "").AssemblyInstallationValue
                                                Else
                                                    Freq3 = Freq3 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue
                                                    ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.AllElapsedValue

                                                    If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True And ObjAssemblyMonitorServiceStatus.IsApplicable = True) Or (ObjAssemblyMonitorServiceStatus.IsApplicable = False) Then
                                                        DueAsof2 = ""
                                                        RemainingTime2 = ""
                                                        AssemblyDueAsof2 = ""
                                                        AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    Else
                                                        DueAsof2 = DueAsof2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
                                                        RemainingTime2 = RemainingTime2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.RemainingValue
                                                        AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
                                                        AirframeDueAsof2 = AirframeDueAsof2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame   'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    End If

                                                    SinceNew2 = SinceNew2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValue
                                                    DoneAt2 = DoneAt2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
                                                    Extension2 = Extension2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.ExtensionValue
                                                    InstalledAt2 = InstalledAt2 & vbCrLf & ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyMonitorServiceStatusPeriod.PeriodID, "").AssemblyInstallationValue
                                                End If
                                            End If

                                        End If


                                        'Added By Saylee on 29-JUN-2021 for ALL29062021
                                        'Done On As of Assembly
                                        If ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 2 Then
                                            If DoneAtAssembly = "" Then
                                                DoneAtAssembly = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValueFormatted
                                            Else
                                                DoneAtAssembly = DoneAtAssembly & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.DoneOnValueFormatted
                                            End If
                                        Else
                                            If DoneAtAssembly = "" Then
                                                DoneAtAssembly = ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyMonitorServiceStatusPeriod.PeriodID, "").AssemblyInstallationValueFormatted
                                            Else
                                                DoneAtAssembly = DoneAtAssembly & vbCrLf & ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyMonitorServiceStatusPeriod.PeriodID, "").AssemblyInstallationValueFormatted
                                            End If
                                        End If
                                    Next
                                    AssemblyID = ObjAssemblyStatus.AssemblyID
                                    Note = ObjAssemblyMonitorServiceStatus.Notes + IIf(ObjAssemblyMonitorServiceStatus.IsApplicable = False, " NOT APPLICABLE", "")
                                    RegNo = ObjMachine.RegNo
                                    RequiredManHours = ObjAssemblyMonitorServiceStatus.RequiredManHours
                                    Customer = ObjMachine.Customer
                                    AssemblyType = ObjAssemblyStatus.AssemblyType
                                    MaintenanceEvent = ObjAssemblyMonitorServiceStatus.Type
                                    ExtensionDate = ObjAssemblyMonitorServiceStatus.ExtensionDate
                                    ApprovalRemark = ObjAssemblyMonitorServiceStatus.ApprovalRemark

                                    'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                    If chkAirframeDueAsOf.Checked Then
                                        If (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL") Then 'Added by Saylee on 17-Sep-2019 Heligo17072019 as Heligo and UHPL needs Comp/Assembly DueOnValue,here DueAsOf is getting overright by AirframeDueAsOf
                                            'Do Nothing
                                        Else

                                            DueAsof = AirframeDueAsof
                                            DueAsof1 = AirframeDueAsof1
                                            DueAsof2 = AirframeDueAsof2
                                        End If
                                    End If
                                    'End

                                    If (ObjAssemblyMonitorServiceStatus.DoneOn <> "" And ObjAssemblyMonitorServiceStatus.MonitorType = "One Time") Or ObjAssemblyMonitorServiceStatus.IsApplicable = False Then
                                        EstimatedDate = ""
                                    End If

                                    If ObjAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriodList.Count > 0 Then '2
                                        'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
                                        If ((Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL")) Then 'Added by Saylee 15-Sep-2010 
                                            If (ObjAssemblyMonitorServiceStatus.IsApplicable = True) Then
                                                If (DoneOnDate <> "" And (AssemblyDueAsof = "" And AssemblyDueAsof1 = "" And AssemblyDueAsof2 = "")) Then
                                                    'do nothing
                                                Else
                                                    ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, , AssemblySerialNo, ATAChapter, , , Position, , MonitorTypeCode, Note, Remark, Description,
                                                     , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel,
                                                      SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, AssemblyTypeID, MaintenanceEvent, , InstalledAt, InstalledAt1, InstalledAt2, , , , , , , , , , , , , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , , , , , , MaintenanceTypeID, MaintenanceTypeName, DoneONValueForAssembly:=DoneAtAssembly, Zone:=TaskNo))
                                                End If
                                            End If
                                        Else
                                            ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, , AssemblySerialNo, ATAChapter, , , Position, , MonitorTypeCode, Note, Remark, Description,
                                            , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel,
                                            SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, AssemblyTypeID, MaintenanceEvent, , InstalledAt, InstalledAt1, InstalledAt2, , , , , , , , , , , , , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , , , , , , MaintenanceTypeID, MaintenanceTypeName, DoneONValueForAssembly:=DoneAtAssembly, Zone:=TaskNo))
                                        End If
                                    End If
                                End If
                            Next
                            For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
                                For Each ObjCompMonitorServiceStatus In ObjCompStatus.CompMonitorServiceStatusList

                                    'chkNotApplicable checkbox added by Saylee on 17-Feb-2017 to show Not Applicable Records when checked
                                    If (ObjCompMonitorServiceStatus.IsApplicable = True And chkNotApplicable.Checked = False) Or (chkNotApplicable.Checked) Then 'Added By Vikrant On 22-May-2014 For All22052014-1
                                        'ATAChapter = ObjCompMonitorServiceStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorServiceStatus.ATANomenclature
                                        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                                            ATAChapter = ObjCompMonitorServiceStatus.ATACode.ToString
                                        Else
                                            ATAChapter = ObjCompMonitorServiceStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorServiceStatus.ATANomenclature
                                        End If
                                        'Added By Prashant On 6-Jun-2023
                                        TaskNo = ObjCompMonitorServiceStatus.TaskNo
                                        Description = ObjCompMonitorServiceStatus.Description
                                        PartNo = ObjCompStatus.PartName
                                        CompSerialNo = ObjCompStatus.CompSerialNo
                                        Position = ObjCompStatus.Position
                                        MonitorTypeCode = ObjCompMonitorServiceStatus.Code
                                        EstimatedDate = ObjCompMonitorServiceStatus.EstimatedDateFormatted
                                        AssemblyModel = ObjAssemblyStatus.Model
                                        AssemblySerialNo = ObjAssemblyStatus.SerialNo
                                        MinimumRemainingValue = ObjCompMonitorServiceStatus.MinimumRemainingValue
                                        AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID
                                        StatusMasterID = ObjCompMonitorServiceStatus.PartMonitorServiceID
                                        DocumentTypeForID = 0
                                        Remark = ObjCompMonitorServiceStatus.DoneRemark
                                        Code = ObjCompMonitorServiceStatus.PartMonitorServiceCode
                                        DoneOnDate = ObjCompMonitorServiceStatus.DoneOn
                                        MaintenanceTypeID = 1
                                        MaintenanceTypeName = "Service"
                                        Freq1 = ""
                                        Freq2 = ""
                                        Freq3 = ""
                                        ElapsedTime = ""
                                        ElapsedTime1 = ""
                                        ElapsedTime2 = ""
                                        RemainingTime = ""
                                        RemainingTime1 = ""
                                        RemainingTime2 = ""
                                        DueAsof = ""
                                        DueAsof1 = ""
                                        DueAsof2 = ""
                                        AssemblyDueAsof = ""
                                        AssemblyDueAsof1 = ""
                                        AssemblyDueAsof2 = ""
                                        SinceNew = ""
                                        SinceNew1 = ""
                                        SinceNew2 = ""
                                        DoneAt = ""
                                        DoneAt1 = ""
                                        DoneAt2 = ""
                                        MaintenanceEvent = ""
                                        Extension = ""
                                        Extension1 = ""
                                        Extension2 = ""
                                        'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                        AirframeDueAsof = ""
                                        AirframeDueAsof1 = ""
                                        AirframeDueAsof2 = ""
                                        'End

                                        'Added By Saylee On 05-Dec-2018 For ALL0122018
                                        InstalledAt = ""
                                        InstalledAt1 = ""
                                        InstalledAt2 = ""
                                        'End

                                        DoneAtAssembly = ""

                                        For Each ObjCompMonitorServiceStatusPeriod In ObjCompMonitorServiceStatus.CompMonitorServiceStatusPeriodList
                                            If Report = 1 Then 'Portarait
                                                If ObjCompMonitorServiceStatusPeriod.PeriodID = 1 Then
                                                    Freq1 = ObjCompMonitorServiceStatusPeriod.FrequencyValue
                                                    ElapsedTime = ObjCompMonitorServiceStatusPeriod.AllElapsedValue


                                                    If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True And ObjCompMonitorServiceStatus.IsApplicable = True) Or (ObjCompMonitorServiceStatus.IsApplicable = False) Then
                                                        DueAsof = ""
                                                        RemainingTime = ""
                                                        AssemblyDueAsof = ""
                                                        AirframeDueAsof = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    Else
                                                        DueAsof = ObjCompMonitorServiceStatusPeriod.DueOnValue
                                                        RemainingTime = ObjCompMonitorServiceStatusPeriod.RemainingValue
                                                        AssemblyDueAsof = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText
                                                        AirframeDueAsof = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    End If
                                                    SinceNew = ObjCompMonitorServiceStatusPeriod.CompCurrentValue
                                                    DoneAt = ObjCompMonitorServiceStatusPeriod.DoneOnValue
                                                    Extension = ObjCompMonitorServiceStatusPeriod.ExtensionValue
                                                    InstalledAt = ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").CompInstallationValue
                                                End If
                                                If ObjCompMonitorServiceStatusPeriod.PeriodID = 2 Then
                                                    Freq2 = ObjCompMonitorServiceStatusPeriod.FrequencyValueFormatted
                                                    ElapsedTime1 = ObjCompMonitorServiceStatusPeriod.AllElapsedValueFormatted


                                                    If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True And ObjCompMonitorServiceStatus.IsApplicable = True) Or (ObjCompMonitorServiceStatus.IsApplicable = False) Then
                                                        DueAsof1 = ""
                                                        RemainingTime1 = ""
                                                        AssemblyDueAsof1 = ""
                                                        AirframeDueAsof1 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    Else
                                                        DueAsof1 = ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                        RemainingTime1 = ObjCompMonitorServiceStatusPeriod.RemainingValueFormatted
                                                        AssemblyDueAsof1 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                        AirframeDueAsof1 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    End If
                                                    SinceNew1 = ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted
                                                    DoneAt1 = ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                    Extension1 = ObjCompMonitorServiceStatusPeriod.ExtensionValueFormatted
                                                    InstalledAt1 = ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").CompInstallationValueFormatted
                                                End If
												'Added PeriodID=11 By Vikrant For ALL 21062012
												'If ObjCompMonitorServiceStatusPeriod.PeriodID = 3 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 4 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 5 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 6 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 7 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 8 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 12 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 13 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 14 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 15 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 11 Then
												'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
												If ObjCompMonitorServiceStatusPeriod.PeriodID >= 3 Then
													If Freq3 = "" Then
														Freq3 = ObjCompMonitorServiceStatusPeriod.FrequencyValue
														ElapsedTime2 = ObjCompMonitorServiceStatusPeriod.AllElapsedValue


														If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True And ObjCompMonitorServiceStatus.IsApplicable = True) Or (ObjCompMonitorServiceStatus.IsApplicable = False) Then
															DueAsof2 = ""
															RemainingTime2 = ""
															AssemblyDueAsof2 = ""
															AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
														Else
															DueAsof2 = ObjCompMonitorServiceStatusPeriod.DueOnValue
															RemainingTime2 = ObjCompMonitorServiceStatusPeriod.RemainingValue
															If ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Then 'Added by Saylee on 20-Aug-2013 for ALL20082013,to show DueOnValue for Accumulated Cycles
																AssemblyDueAsof2 = ObjCompMonitorServiceStatusPeriod.DueOnValue
															Else
																AssemblyDueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText
															End If
															AirframeDueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
														End If
														SinceNew2 = ObjCompMonitorServiceStatusPeriod.CompCurrentValue
														DoneAt2 = ObjCompMonitorServiceStatusPeriod.DoneOnValue
														Extension2 = ObjCompMonitorServiceStatusPeriod.ExtensionValue
														InstalledAt2 = ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").CompInstallationValue
													Else
														Freq3 = Freq3 & vbCrLf & ObjCompMonitorServiceStatusPeriod.FrequencyValue
														ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.AllElapsedValueFormatted


														If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True And ObjCompMonitorServiceStatus.IsApplicable = True) Or (ObjCompMonitorServiceStatus.IsApplicable = False) Then
															DueAsof2 = ""
															RemainingTime2 = ""
															AssemblyDueAsof2 = ""
															AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
														Else
															DueAsof2 = DueAsof2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
															RemainingTime2 = RemainingTime2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.RemainingValue
															If ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Then 'Added by Saylee on 20-Aug-2013 for ALL20082013,to show DueOnValue for Accumulated Cycles
																AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
															Else
																AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText
															End If
															AirframeDueAsof2 = AirframeDueAsof2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
														End If
														SinceNew2 = SinceNew2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.CompCurrentValue
														DoneAt2 = DoneAt2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.DoneOnValue
														InstalledAt2 = InstalledAt2 & vbCrLf & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").CompInstallationValue
													End If
												End If
											Else
                                                If ObjCompMonitorServiceStatusPeriod.PeriodID = 2 Then
                                                    If Freq3 = "" Then
                                                        Freq3 = ObjCompMonitorServiceStatusPeriod.FrequencyValueFormatted
                                                        ElapsedTime2 = ObjCompMonitorServiceStatusPeriod.AllElapsedValueFormatted


                                                        If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True And ObjCompMonitorServiceStatus.IsApplicable = True) Or (ObjCompMonitorServiceStatus.IsApplicable = False) Then
                                                            DueAsof2 = ""
                                                            RemainingTime2 = ""
                                                            AssemblyDueAsof2 = ""
                                                            AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                        Else
                                                            DueAsof2 = ObjCompMonitorServiceStatusPeriod.DueOnValue
                                                            RemainingTime2 = ObjCompMonitorServiceStatusPeriod.RemainingValueFormatted
                                                            AssemblyDueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                            AirframeDueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                        End If
                                                        SinceNew2 = ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted
                                                        DoneAt2 = ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                        Extension2 = ObjCompMonitorServiceStatusPeriod.ExtensionValueFormatted
                                                        InstalledAt2 = ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").CompInstallationValueFormatted
                                                    Else
                                                        Freq3 = Freq3 & vbCrLf & ObjCompMonitorServiceStatusPeriod.FrequencyValueFormatted
                                                        ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.AllElapsedValueFormatted


                                                        If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True And ObjCompMonitorServiceStatus.IsApplicable = True) Or (ObjCompMonitorServiceStatus.IsApplicable = False) Then
                                                            DueAsof2 = ""
                                                            RemainingTime2 = ""
                                                            AssemblyDueAsof2 = ""
                                                            AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                        Else
                                                            DueAsof2 = DueAsof2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                            RemainingTime2 = RemainingTime2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.RemainingValueFormatted
                                                            AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                            AirframeDueAsof2 = AirframeDueAsof2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                        End If
                                                        SinceNew2 = SinceNew2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted
                                                        DoneAt2 = DoneAt2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                        Extension2 = Extension2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.ExtensionValueFormatted
                                                        InstalledAt2 = InstalledAt2 & vbCrLf & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").CompInstallationValueFormatted
                                                    End If
                                                Else
                                                    If Freq3 = "" Then
                                                        Freq3 = ObjCompMonitorServiceStatusPeriod.FrequencyValue
                                                        ElapsedTime2 = ObjCompMonitorServiceStatusPeriod.AllElapsedValue


                                                        If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True And ObjCompMonitorServiceStatus.IsApplicable = True) Or (ObjCompMonitorServiceStatus.IsApplicable = False) Then
                                                            DueAsof2 = ""
                                                            RemainingTime2 = ""
                                                            AssemblyDueAsof2 = ""
                                                            AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                        Else

                                                            DueAsof2 = ObjCompMonitorServiceStatusPeriod.DueOnValue
                                                            RemainingTime2 = ObjCompMonitorServiceStatusPeriod.RemainingValue
                                                            If ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Then 'Added by Saylee on 20-Aug-2013 for ALL20082013,to show DueOnValue for Accumulated Cycles
                                                                AssemblyDueAsof2 = ObjCompMonitorServiceStatusPeriod.DueOnValue
                                                            Else
                                                                AssemblyDueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText
                                                            End If
                                                            AirframeDueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                        End If
                                                        SinceNew2 = ObjCompMonitorServiceStatusPeriod.CompCurrentValue
                                                        DoneAt2 = ObjCompMonitorServiceStatusPeriod.DoneOnValue
                                                        Extension2 = ObjCompMonitorServiceStatusPeriod.ExtensionValue
                                                        InstalledAt2 = ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").CompInstallationValue
                                                    Else
                                                        Freq3 = Freq3 & vbCrLf & ObjCompMonitorServiceStatusPeriod.FrequencyValue
                                                        ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.ElapsedValue

                                                        If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True And ObjCompMonitorServiceStatus.IsApplicable = True) Or (ObjCompMonitorServiceStatus.IsApplicable = False) Then
                                                            DueAsof2 = ""
                                                            RemainingTime2 = ""
                                                            AssemblyDueAsof2 = ""
                                                            AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                        Else
                                                            DueAsof2 = DueAsof2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                            RemainingTime2 = RemainingTime2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.RemainingValue

                                                            If ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Then 'Added by Saylee on 20-Aug-2013 for ALL20082013,to show DueOnValue for Accumulated Cycles
                                                                AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                            Else
                                                                AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText
                                                            End If
                                                            AirframeDueAsof2 = AirframeDueAsof2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                        End If
                                                    End If
                                                    SinceNew2 = SinceNew2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.CompCurrentValue
                                                    DoneAt2 = DoneAt2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.DoneOnValue
                                                    Extension2 = Extension2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.ExtensionValue
                                                    InstalledAt2 = InstalledAt2 & vbCrLf & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").CompInstallationValue
                                                End If
                                            End If
                                            If ObjCompMonitorServiceStatusPeriod.PeriodID = 2 Then
                                                If DoneAtAssembly = "" Then
                                                    DoneAtAssembly = ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                Else
                                                    DoneAtAssembly = DoneAtAssembly & vbCrLf & ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                End If
                                            Else
                                                If DoneAtAssembly = "" Then
                                                    DoneAtAssembly = ObjCompMonitorServiceStatusPeriod.AssemblyDoneOnValueTextFormatted
                                                Else
                                                    DoneAtAssembly = DoneAtAssembly & vbCrLf & ObjCompMonitorServiceStatusPeriod.AssemblyDoneOnValueTextFormatted
                                                End If
                                            End If

                                        Next
                                        AssemblyID = ObjAssemblyStatus.AssemblyID
                                        AssemblyType = ObjAssemblyStatus.AssemblyType
                                        RegNo = ObjMachine.RegNo
                                        RequiredManHours = ObjCompMonitorServiceStatus.RequiredManHours
                                        Customer = ObjMachine.Customer
                                        Note = ObjCompMonitorServiceStatus.Notes + IIf(ObjCompMonitorServiceStatus.IsApplicable = False, " NOT APPLICABLE", "")
                                        MaintenanceEvent = ObjCompMonitorServiceStatus.Type
                                        ExtensionDate = ObjCompMonitorServiceStatus.ExtensionDate
                                        ApprovalRemark = ObjCompMonitorServiceStatus.ApprovalRemark

                                        'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                        If chkAirframeDueAsOf.Checked Then
                                            If (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL") Then 'Added by Saylee on 17-Sep-2019 Heligo17072019 as Heligo and UHPL needs Comp/Assembly DueOnValue,here DueAsOf is getting overright by AirframeDueAsOf
                                                'Do Nothing
                                            Else

                                                DueAsof = AirframeDueAsof
                                                DueAsof1 = AirframeDueAsof1
                                                DueAsof2 = AirframeDueAsof2
                                            End If
                                        End If
                                        'End

                                        If (ObjCompMonitorServiceStatus.DoneOn <> "" And ObjCompMonitorServiceStatus.MonitorType = "One Time") Or ObjCompMonitorServiceStatus.IsApplicable = False Then
                                            EstimatedDate = ""
                                        End If

                                        If ObjCompMonitorServiceStatus.CompMonitorServiceStatusPeriodList.Count > 0 Then '3
                                            'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
                                            If ((Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL")) Then 'Added by Saylee 15-Sep-2010
                                                If (ObjCompMonitorServiceStatus.IsApplicable = True) Then
                                                    If (DoneOnDate <> "" And (AssemblyDueAsof = "" And AssemblyDueAsof1 = "" And AssemblyDueAsof2 = "")) Then
                                                        'do nothing
                                                    Else
                                                        ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, MaintenanceEvent, AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, , MonitorTypeCode, Note, Remark, Description,
                                                                             , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue,
                                                                              AssemblyTypeID, MaintenanceEvent, , InstalledAt, InstalledAt1, InstalledAt2, , , , , , , , , , , , , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , , , , , , MaintenanceTypeID, MaintenanceTypeName, DoneONValueForAssembly:=DoneAtAssembly, Zone:=TaskNo))
                                                    End If
                                                End If
                                            Else
                                                ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, MaintenanceEvent, AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, , MonitorTypeCode, Note, Remark, Description,
                                                                     , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue,
                                                                     AssemblyTypeID, MaintenanceEvent, , InstalledAt, InstalledAt1, InstalledAt2, , , , , , , , , , , , , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , , , , , , MaintenanceTypeID, MaintenanceTypeName, DoneONValueForAssembly:=DoneAtAssembly, Zone:=TaskNo))
                                            End If
                                        End If
                                    End If
                                Next
                            Next
                        Next
                    Next
                End If
            Next
        End If

        If IsInsSelect = True Then
            For i As Integer = 0 To ListInspectionType.Items.Count - 1
                If ListInspectionType.Items.Item(i).Selected Then
                    mMachineList = MachineList.GetMachineListMonitoringStatus(AsonDate, MachineName, , , , , , , , , , True, True, , AssemblyName, , , , , , , , , , , , , , True, , , , , , , , False, , False, , True, , , , CInt(ListInspectionType.Items.Item(i).Value), , , True, IsAverageRequired:=True, ByPerDayLimit:=True, PerdayLimits:=mPerDayLimits, SkipIsForInventoryAircarft:=True)
                    For Each ObjMachine In mMachineList
                        For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                            For Each ObjAssemblyMonitorInspStatus In ObjAssemblyStatus.AssemblyMonitorInspStatusList

                                'chkNotApplicable checkbox added by Saylee on 17-Feb-2017 to show Not Applicable Records when checked
                                If (ObjAssemblyMonitorInspStatus.IsApplicable = True And chkNotApplicable.Checked = False) Or (chkNotApplicable.Checked) Then 'Added By Vikrant On 22-May-2014 For All22052014-1
                                    'ATAChapter = ObjAssemblyMonitorInspStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorInspStatus.ATANomenclature
                                    If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                                        ATAChapter = ObjAssemblyMonitorInspStatus.ATACode.ToString
                                    Else
                                        ATAChapter = ObjAssemblyMonitorInspStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorInspStatus.ATANomenclature
                                    End If
                                    Description = ObjAssemblyMonitorInspStatus.Description
                                    AssemblyModel = ObjAssemblyStatus.Model
                                    AssemblySerialNo = ObjAssemblyStatus.SerialNo
                                    Position = ""
                                    MonitorTypeCode = ObjAssemblyMonitorInspStatus.Code
                                    EstimatedDate = ObjAssemblyMonitorInspStatus.EstimatedDateFormatted
                                    MinimumRemainingValue = ObjAssemblyMonitorInspStatus.MinimumRemainingValue
                                    AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID
                                    StatusMasterID = ObjAssemblyMonitorInspStatus.ModelMonitorInspID
                                    DocumentTypeForID = 9
                                    Code = ObjAssemblyMonitorInspStatus.ModelMonitorInspCode
                                    Remark = ObjAssemblyMonitorInspStatus.DoneRemark
                                    DoneOnDate = ObjAssemblyMonitorInspStatus.DoneOn
                                    MaintenanceTypeID = 2
                                    MaintenanceTypeName = "Inspection"
                                    Freq1 = ""
                                    Freq2 = ""
                                    Freq3 = ""
                                    ElapsedTime = ""
                                    ElapsedTime1 = ""
                                    ElapsedTime2 = ""
                                    RemainingTime = ""
                                    RemainingTime1 = ""
                                    RemainingTime2 = ""
                                    DueAsof = ""
                                    DueAsof1 = ""
                                    DueAsof2 = ""
                                    AssemblyDueAsof = ""
                                    AssemblyDueAsof1 = ""
                                    AssemblyDueAsof2 = ""
                                    SinceNew = ""
                                    SinceNew1 = ""
                                    SinceNew2 = ""
                                    DoneAt = ""
                                    DoneAt1 = ""
                                    DoneAt2 = ""
                                    Extension = ""
                                    Extension1 = ""
                                    Extension2 = ""
                                    MaintenanceEvent = ""
                                    'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                    AirframeDueAsof = ""
                                    AirframeDueAsof1 = ""
                                    AirframeDueAsof2 = ""
                                    'End


                                    'Added By Saylee On 5-Dec-2018 For ALL05122018
                                    InstalledAt = ""
                                    InstalledAt1 = ""
                                    InstalledAt2 = ""
                                    'End

                                    DoneAtAssembly = ""
                                    For Each ObjAssemblyMonitorInspStatusPeriod In ObjAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriodList
                                        If Report = 1 Then 'Portarait
                                            If ObjAssemblyMonitorInspStatusPeriod.PeriodID = 1 Then
                                                Freq1 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValue
                                                ElapsedTime = ObjAssemblyMonitorInspStatusPeriod.ElapsedValue

                                                If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True And ObjAssemblyMonitorInspStatus.IsApplicable = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False) Then
                                                    DueAsof = ""
                                                    RemainingTime = ""
                                                    AssemblyDueAsof = ""
                                                    AirframeDueAsof = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                Else
                                                    DueAsof = ObjAssemblyMonitorInspStatusPeriod.DueOnValue
                                                    RemainingTime = ObjAssemblyMonitorInspStatusPeriod.RemainingValue
                                                    AssemblyDueAsof = ObjAssemblyMonitorInspStatusPeriod.DueOnValue
                                                    AirframeDueAsof = ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                End If

                                                SinceNew = ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue
                                                DoneAt = ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
                                                Extension = ObjAssemblyMonitorInspStatusPeriod.ExtensionValue
                                                InstalledAt = ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyMonitorInspStatusPeriod.PeriodID, "").AssemblyInstallationValue
                                            End If
                                            If ObjAssemblyMonitorInspStatusPeriod.PeriodID = 2 Then
                                                Freq2 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValueFormatted
                                                ElapsedTime1 = ObjAssemblyMonitorInspStatusPeriod.ElapsedValueFormatted

                                                If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True And ObjAssemblyMonitorInspStatus.IsApplicable = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False) Then
                                                    DueAsof1 = ""
                                                    RemainingTime1 = ""
                                                    AssemblyDueAsof1 = ""
                                                    AirframeDueAsof1 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                Else
                                                    DueAsof1 = ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
                                                    RemainingTime1 = ObjAssemblyMonitorInspStatusPeriod.RemainingValueFormatted
                                                    AssemblyDueAsof1 = ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
                                                    AirframeDueAsof1 = ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                End If

                                                SinceNew1 = ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValueFormatted
                                                DoneAt1 = ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted
                                                Extension1 = ObjAssemblyMonitorInspStatusPeriod.ExtensionValueFormatted
                                                InstalledAt1 = ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyMonitorInspStatusPeriod.PeriodID, "").AssemblyInstallationValueFormatted
                                            End If
											'Added PeriodID=11 By Vikrant For ALL 21062012
											'If ObjAssemblyMonitorInspStatusPeriod.PeriodID = 3 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 4 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 5 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 6 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 7 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 8 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 9 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 12 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 13 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 14 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 15 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 11 Then
											'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
											If ObjAssemblyMonitorInspStatusPeriod.PeriodID >= 3 Then
												If Freq3 = "" Then
													Freq3 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValue
													ElapsedTime2 = ObjAssemblyMonitorInspStatusPeriod.ElapsedValue

													If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True And ObjAssemblyMonitorInspStatus.IsApplicable = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False) Then
														DueAsof2 = ""
														RemainingTime2 = ""
														AssemblyDueAsof2 = ""
														AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
													Else
														DueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValue
														RemainingTime2 = ObjAssemblyMonitorInspStatusPeriod.RemainingValue
														AssemblyDueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValue
														AirframeDueAsof2 = ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
													End If

													SinceNew2 = ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue
													DoneAt2 = ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
													Extension2 = ObjAssemblyMonitorInspStatusPeriod.ExtensionValue
													InstalledAt2 = ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyMonitorInspStatusPeriod.PeriodID, "").AssemblyInstallationValue
												Else
													Freq3 = Freq3 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.FrequencyValue
													ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.ElapsedValue

													If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True And ObjAssemblyMonitorInspStatus.IsApplicable = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False) Then
														DueAsof2 = ""
														RemainingTime2 = ""
														AssemblyDueAsof2 = ""
														AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
													Else
														DueAsof2 = DueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DueOnValue
														RemainingTime2 = RemainingTime2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.RemainingValue
														AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DueOnValue
														AirframeDueAsof2 = AirframeDueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
													End If

													SinceNew2 = SinceNew2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue
													DoneAt2 = DoneAt2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
													Extension2 = Extension2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.ExtensionValue
													InstalledAt2 = InstalledAt2 & vbCrLf & ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyMonitorInspStatusPeriod.PeriodID, "").AssemblyInstallationValue
												End If
											End If
										Else
                                            If ObjAssemblyMonitorInspStatusPeriod.PeriodID = 2 Then
                                                If Freq3 = "" Then
                                                    Freq3 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValueFormatted
                                                    ElapsedTime2 = ObjAssemblyMonitorInspStatusPeriod.ElapsedValueFormatted

                                                    If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True And ObjAssemblyMonitorInspStatus.IsApplicable = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False) Then
                                                        DueAsof2 = ""
                                                        RemainingTime2 = ""
                                                        AssemblyDueAsof2 = ""
                                                        AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    Else
                                                        DueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
                                                        RemainingTime2 = ObjAssemblyMonitorInspStatusPeriod.RemainingValueFormatted
                                                        AssemblyDueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
                                                        AirframeDueAsof2 = ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    End If

                                                    SinceNew2 = ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValueFormatted
                                                    DoneAt2 = ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted
                                                    Extension2 = ObjAssemblyMonitorInspStatusPeriod.ExtensionValueFormatted
                                                    InstalledAt2 = InstalledAt2 & vbCrLf & ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyMonitorInspStatusPeriod.PeriodID, "").AssemblyInstallationValueFormatted
                                                Else
                                                    Freq3 = Freq3 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.FrequencyValueFormatted
                                                    ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.ElapsedValueFormatted

                                                    If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True And ObjAssemblyMonitorInspStatus.IsApplicable = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False) Then
                                                        DueAsof2 = ""
                                                        RemainingTime2 = ""
                                                        AssemblyDueAsof2 = ""
                                                        AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    Else
                                                        DueAsof2 = DueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
                                                        RemainingTime2 = RemainingTime2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.RemainingValueFormatted
                                                        AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
                                                        AirframeDueAsof2 = AirframeDueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    End If

                                                    SinceNew2 = SinceNew2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValueFormatted
                                                    DoneAt2 = DoneAt2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted
                                                    Extension2 = Extension2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.ExtensionValueFormatted
                                                    InstalledAt2 = InstalledAt2 & vbCrLf & ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyMonitorInspStatusPeriod.PeriodID, "").AssemblyInstallationValueFormatted
                                                End If
                                            Else
                                                If Freq3 = "" Then
                                                    Freq3 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValue
                                                    ElapsedTime2 = ObjAssemblyMonitorInspStatusPeriod.AllElapsedValue

                                                    If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True And ObjAssemblyMonitorInspStatus.IsApplicable = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False) Then
                                                        DueAsof2 = ""
                                                        RemainingTime2 = ""
                                                        AssemblyDueAsof2 = ""
                                                        AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    Else
                                                        DueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValue
                                                        RemainingTime2 = ObjAssemblyMonitorInspStatusPeriod.RemainingValue
                                                        AssemblyDueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValue
                                                        AirframeDueAsof2 = ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    End If

                                                    SinceNew2 = ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue
                                                    DoneAt2 = ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
                                                    Extension2 = ObjAssemblyMonitorInspStatusPeriod.ExtensionValue
                                                    InstalledAt2 = ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyMonitorInspStatusPeriod.PeriodID, "").AssemblyInstallationValue
                                                Else
                                                    Freq3 = Freq3 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.FrequencyValue
                                                    ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.AllElapsedValue

                                                    If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True And ObjAssemblyMonitorInspStatus.IsApplicable = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False) Then
                                                        DueAsof2 = ""
                                                        RemainingTime2 = ""
                                                        AssemblyDueAsof2 = ""
                                                        AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    Else
                                                        DueAsof2 = DueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DueOnValue
                                                        RemainingTime2 = RemainingTime2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.RemainingValue
                                                        AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DueOnValue
                                                        AirframeDueAsof2 = AirframeDueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    End If

                                                    SinceNew2 = SinceNew2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue
                                                    DoneAt2 = DoneAt2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
                                                    Extension2 = Extension2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.ExtensionValue
                                                    InstalledAt2 = InstalledAt2 & vbCrLf & ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyMonitorInspStatusPeriod.PeriodID, "").AssemblyInstallationValue
                                                End If
                                            End If
                                        End If
                                        If ObjAssemblyMonitorInspStatusPeriod.PeriodID = 2 Then
                                            If DoneAtAssembly = "" Then
                                                DoneAtAssembly = ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted
                                            Else
                                                DoneAtAssembly = DoneAtAssembly & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted
                                            End If
                                        Else
                                            If DoneAtAssembly = "" Then
                                                DoneAtAssembly = ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyMonitorInspStatusPeriod.PeriodID, "").AssemblyInstallationValueFormatted
                                            Else
                                                DoneAtAssembly = DoneAtAssembly & vbCrLf & ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyMonitorInspStatusPeriod.PeriodID, "").AssemblyInstallationValueFormatted
                                            End If
                                        End If

                                    Next
                                    AssemblyID = ObjAssemblyStatus.AssemblyID
                                    AssemblyType = ObjAssemblyStatus.AssemblyType
                                    RegNo = ObjMachine.RegNo
                                    RequiredManHours = ObjAssemblyMonitorInspStatus.RequiredManHours
                                    Customer = ObjMachine.Customer
                                    Note = ObjAssemblyMonitorInspStatus.Notes + IIf(ObjAssemblyMonitorInspStatus.IsApplicable = False, " NOT APPLICABLE", "")
                                    MaintenanceEvent = ObjAssemblyMonitorInspStatus.Type
                                    ExtensionDate = ObjAssemblyMonitorInspStatus.ExtensionDate
                                    ApprovalRemark = ObjAssemblyMonitorInspStatus.ApprovalRemark
                                    'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                    If chkAirframeDueAsOf.Checked Then
                                        If (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL") Then 'Added by Saylee on 17-Sep-2019 Heligo17072019 as Heligo and UHPL needs Comp/Assembly DueOnValue,here DueAsOf is getting overright by AirframeDueAsOf
                                            'Do Nothing
                                        Else

                                            DueAsof = AirframeDueAsof
                                            DueAsof1 = AirframeDueAsof1
                                            DueAsof2 = AirframeDueAsof2
                                        End If
                                    End If
                                    'End
                                    If (ObjAssemblyMonitorInspStatus.DoneOn <> "" And ObjAssemblyMonitorInspStatus.MonitorType = "One Time") Or ObjAssemblyMonitorInspStatus.IsApplicable = False Then
                                        EstimatedDate = ""
                                    End If
                                    If ObjAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriodList.Count > 0 Then '4
                                        'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
                                        If ((Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL")) Then  'Added by Saylee 15-Sep-2010
                                            If (ObjAssemblyMonitorInspStatus.IsApplicable = True) Then
                                                If (DoneOnDate <> "" And (AssemblyDueAsof = "" And AssemblyDueAsof1 = "" And AssemblyDueAsof2 = "")) Then
                                                    'do nothing
                                                Else
                                                    ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, , AssemblySerialNo, ATAChapter, , , Position, , MonitorTypeCode, Note, Remark, Description,
                                                      , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel,
                                                      SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue,
                                                      AssemblyTypeID, MaintenanceEvent, , InstalledAt, InstalledAt1, InstalledAt2, , , , , , , , , , , , , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , , , , , , MaintenanceTypeID, MaintenanceTypeName, DoneONValueForAssembly:=DoneAtAssembly))
                                                End If
                                            End If
                                        Else
                                            ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, , AssemblySerialNo, ATAChapter, , , Position, , MonitorTypeCode, Note, Remark, Description,
                                          , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel,
                                          SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue,
                                          AssemblyTypeID, MaintenanceEvent, , InstalledAt, InstalledAt1, InstalledAt2, , , , , , , , , , , , , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , , , , , , MaintenanceTypeID, MaintenanceTypeName, DoneONValueForAssembly:=DoneAtAssembly))
                                        End If
                                    End If
                                End If
                            Next
                            For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
                                For Each ObjCompMonitorInspStatus In ObjCompStatus.CompMonitorInspStatusList

                                    'chkNotApplicable checkbox added by Saylee on 17-Feb-2017 to show Not Applicable Records when checked
                                    If (ObjCompMonitorInspStatus.IsApplicable = True And chkNotApplicable.Checked = False) Or (chkNotApplicable.Checked) Then 'Added By Vikrant On 22-May-2014 For All22052014-1
                                        'ATAChapter = ObjCompMonitorInspStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorInspStatus.ATANomenclature
                                        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                                            ATAChapter = ObjCompMonitorInspStatus.ATACode.ToString
                                        Else
                                            ATAChapter = ObjCompMonitorInspStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorInspStatus.ATANomenclature
                                        End If
                                        Description = ObjCompMonitorInspStatus.Description
                                        PartNo = ObjCompStatus.PartName
                                        CompSerialNo = ObjCompStatus.CompSerialNo
                                        Position = ObjCompStatus.Position
                                        MonitorTypeCode = ObjCompMonitorInspStatus.Code
                                        EstimatedDate = ObjCompMonitorInspStatus.EstimatedDateFormatted
                                        AssemblyModel = ObjAssemblyStatus.Model
                                        AssemblySerialNo = ObjAssemblyStatus.SerialNo
                                        MinimumRemainingValue = ObjCompMonitorInspStatus.MinimumRemainingValue
                                        AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID
                                        StatusMasterID = ObjCompMonitorInspStatus.PartMonitorInspID
                                        DocumentTypeForID = 11
                                        Remark = ObjCompMonitorInspStatus.DoneRemark
                                        Code = ObjCompMonitorInspStatus.PartMonitorInspCode
                                        DoneOnDate = ObjCompMonitorInspStatus.DoneOn
                                        MaintenanceTypeID = 2
                                        MaintenanceTypeName = "Inspection"
                                        Freq1 = ""
                                        Freq2 = ""
                                        Freq3 = ""
                                        ElapsedTime = ""
                                        ElapsedTime1 = ""
                                        ElapsedTime2 = ""
                                        RemainingTime = ""
                                        RemainingTime1 = ""
                                        RemainingTime2 = ""
                                        DueAsof = ""
                                        DueAsof1 = ""
                                        DueAsof2 = ""
                                        AssemblyDueAsof = ""
                                        AssemblyDueAsof1 = ""
                                        AssemblyDueAsof2 = ""
                                        SinceNew = ""
                                        SinceNew1 = ""
                                        SinceNew2 = ""
                                        DoneAt = ""
                                        DoneAt1 = ""
                                        DoneAt2 = ""
                                        MaintenanceEvent = ""
                                        Extension = ""
                                        Extension1 = ""
                                        Extension2 = ""
                                        'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                        AirframeDueAsof = ""
                                        AirframeDueAsof1 = ""
                                        AirframeDueAsof2 = ""
                                        'End
                                        'Added By Saylee On 5-Dec-2018 For ALL05122018
                                        InstalledAt = ""
                                        InstalledAt1 = ""
                                        InstalledAt2 = ""
                                        'End

                                        DoneAtAssembly = ""
                                        For Each ObjCompMonitorInspStatusPeriod In ObjCompMonitorInspStatus.CompMonitorInspStatusPeriodList
                                            If Report = 1 Then 'Portarait
                                                If ObjCompMonitorInspStatusPeriod.PeriodID = 1 Then
                                                    Freq1 = ObjCompMonitorInspStatusPeriod.FrequencyValue
                                                    ElapsedTime = ObjCompMonitorInspStatusPeriod.AllElapsedValue


                                                    If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True And ObjCompMonitorInspStatus.IsApplicable = True) Or (ObjCompMonitorInspStatus.IsApplicable = False) Then
                                                        DueAsof = ""
                                                        RemainingTime = ""
                                                        AssemblyDueAsof = ""
                                                        AirframeDueAsof = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    Else
                                                        DueAsof = ObjCompMonitorInspStatusPeriod.DueOnValue
                                                        RemainingTime = ObjCompMonitorInspStatusPeriod.RemainingValue
                                                        AssemblyDueAsof = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText
                                                        AirframeDueAsof = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    End If
                                                    SinceNew = ObjCompMonitorInspStatusPeriod.CompCurrentValue
                                                    DoneAt = ObjCompMonitorInspStatusPeriod.DoneOnValue
                                                    Extension = ObjCompMonitorInspStatusPeriod.ExtensionValue
                                                    InstalledAt = ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").CompInstallationValue
                                                End If
                                                If ObjCompMonitorInspStatusPeriod.PeriodID = 2 Then
                                                    Freq2 = ObjCompMonitorInspStatusPeriod.FrequencyValueFormatted
                                                    ElapsedTime1 = ObjCompMonitorInspStatusPeriod.AllElapsedValueFormatted


                                                    If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True And ObjCompMonitorInspStatus.IsApplicable = True) Or (ObjCompMonitorInspStatus.IsApplicable = False) Then
                                                        DueAsof1 = ""
                                                        RemainingTime1 = ""
                                                        AssemblyDueAsof1 = ""
                                                        AirframeDueAsof1 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    Else
                                                        DueAsof1 = ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
                                                        RemainingTime1 = ObjCompMonitorInspStatusPeriod.RemainingValueFormatted
                                                        AssemblyDueAsof1 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                        AirframeDueAsof1 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    End If
                                                    SinceNew1 = ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted
                                                    DoneAt1 = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                    Extension1 = ObjCompMonitorInspStatusPeriod.ExtensionValueFormatted
                                                    InstalledAt1 = ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").CompInstallationValueFormatted
                                                End If
												'Added PeriodID=11 By Vikrant For ALL 21062012
												'If ObjCompMonitorInspStatusPeriod.PeriodID = 3 Or ObjCompMonitorInspStatusPeriod.PeriodID = 4 Or ObjCompMonitorInspStatusPeriod.PeriodID = 5 Or ObjCompMonitorInspStatusPeriod.PeriodID = 6 Or ObjCompMonitorInspStatusPeriod.PeriodID = 7 Or ObjCompMonitorInspStatusPeriod.PeriodID = 8 Or ObjCompMonitorInspStatusPeriod.PeriodID = 9 Or ObjCompMonitorInspStatusPeriod.PeriodID = 12 Or ObjCompMonitorInspStatusPeriod.PeriodID = 13 Or ObjCompMonitorInspStatusPeriod.PeriodID = 14 Or ObjCompMonitorInspStatusPeriod.PeriodID = 15 Or ObjCompMonitorInspStatusPeriod.PeriodID = 11 Then
												'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
												If ObjCompMonitorInspStatusPeriod.PeriodID >= 3 Then
													If Freq3 = "" Then
														Freq3 = ObjCompMonitorInspStatusPeriod.FrequencyValue
														ElapsedTime2 = ObjCompMonitorInspStatusPeriod.AllElapsedValue


														If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True And ObjCompMonitorInspStatus.IsApplicable = True) Or (ObjCompMonitorInspStatus.IsApplicable = False) Then
															DueAsof2 = ""
															RemainingTime2 = ""
															AssemblyDueAsof2 = ""
															AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
														Else
															DueAsof2 = ObjCompMonitorInspStatusPeriod.DueOnValue
															RemainingTime2 = ObjCompMonitorInspStatusPeriod.RemainingValue
															If ObjCompMonitorInspStatusPeriod.PeriodID = 9 Then 'Added by Saylee on 20-Aug-2013 for ALL20082013,to show DueOnValue for Accumulated Cycles
																AssemblyDueAsof2 = ObjCompMonitorInspStatusPeriod.DueOnValue
															Else
																AssemblyDueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText
															End If
															AirframeDueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
														End If
														SinceNew2 = ObjCompMonitorInspStatusPeriod.CompCurrentValue
														DoneAt2 = ObjCompMonitorInspStatusPeriod.DoneOnValue
														Extension2 = ObjCompMonitorInspStatusPeriod.ExtensionValue
														InstalledAt2 = ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").CompInstallationValue
													Else
														Freq3 = Freq3 & vbCrLf & ObjCompMonitorInspStatusPeriod.FrequencyValue
														ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjCompMonitorInspStatusPeriod.AllElapsedValue


														If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True And ObjCompMonitorInspStatus.IsApplicable = True) Or (ObjCompMonitorInspStatus.IsApplicable = False) Then
															DueAsof2 = ""
															RemainingTime2 = ""
															AssemblyDueAsof2 = ""
															AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
														Else
															DueAsof2 = DueAsof2 & vbCrLf & ObjCompMonitorInspStatusPeriod.DueOnValue
															RemainingTime2 = RemainingTime2 & vbCrLf & ObjCompMonitorInspStatusPeriod.RemainingValue
															If ObjCompMonitorInspStatusPeriod.PeriodID = 9 Then 'Added by Saylee on 20-Aug-2013 for ALL20082013,to show DueOnValue for Accumulated Cycles
																AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorInspStatusPeriod.DueOnValue
															Else
																AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText
															End If
															AirframeDueAsof2 = AirframeDueAsof2 & vbCrLf & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
														End If
														SinceNew2 = SinceNew2 & vbCrLf & ObjCompMonitorInspStatusPeriod.CompCurrentValue
														DoneAt2 = DoneAt2 & vbCrLf & ObjCompMonitorInspStatusPeriod.DoneOnValue
														Extension2 = Extension2 & vbCrLf & ObjCompMonitorInspStatusPeriod.ExtensionValue
														InstalledAt2 = InstalledAt2 & vbCrLf & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").CompInstallationValue
													End If
												End If
											Else
                                                If ObjCompMonitorInspStatusPeriod.PeriodID = 2 Then
                                                    If Freq3 = "" Then
                                                        Freq3 = ObjCompMonitorInspStatusPeriod.FrequencyValueFormatted
                                                        ElapsedTime2 = ObjCompMonitorInspStatusPeriod.AllElapsedValueFormatted


                                                        If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True And ObjCompMonitorInspStatus.IsApplicable = True) Or (ObjCompMonitorInspStatus.IsApplicable = False) Then
                                                            DueAsof2 = ""
                                                            RemainingTime2 = ""
                                                            AssemblyDueAsof2 = ""
                                                            AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                        Else
                                                            DueAsof2 = ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
                                                            RemainingTime2 = ObjCompMonitorInspStatusPeriod.RemainingValueFormatted
                                                            AssemblyDueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                            AirframeDueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                        End If
                                                        SinceNew2 = ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted
                                                        DoneAt2 = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                        Extension2 = ObjCompMonitorInspStatusPeriod.ExtensionValueFormatted
                                                        InstalledAt2 = InstalledAt2 & vbCrLf & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").CompInstallationValueFormatted
                                                    Else
                                                        Freq3 = Freq3 & vbCrLf & ObjCompMonitorInspStatusPeriod.FrequencyValueFormatted
                                                        ElapsedTime2 = ObjCompMonitorInspStatusPeriod.AllElapsedValueFormatted


                                                        If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True And ObjCompMonitorInspStatus.IsApplicable = True) Or (ObjCompMonitorInspStatus.IsApplicable = False) Then
                                                            DueAsof2 = ""
                                                            RemainingTime2 = ""
                                                            AssemblyDueAsof2 = ""
                                                            AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                        Else
                                                            DueAsof2 = DueAsof2 & vbCrLf & ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
                                                            RemainingTime2 = RemainingTime2 & vbCrLf & ObjCompMonitorInspStatusPeriod.RemainingValueFormatted
                                                            AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                            AirframeDueAsof2 = AirframeDueAsof2 & vbCrLf & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                        End If
                                                        SinceNew2 = SinceNew2 & vbCrLf & ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted
                                                        DoneAt2 = DoneAt2 & vbCrLf & ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                        Extension2 = Extension2 & vbCrLf & ObjCompMonitorInspStatusPeriod.ExtensionValueFormatted
                                                        InstalledAt2 = InstalledAt2 & vbCrLf & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").CompInstallationValueFormatted
                                                    End If
                                                Else
                                                    If Freq3 = "" Then
                                                        Freq3 = ObjCompMonitorInspStatusPeriod.FrequencyValue
                                                        ElapsedTime2 = ObjCompMonitorInspStatusPeriod.AllElapsedValue


                                                        If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True And ObjCompMonitorInspStatus.IsApplicable = True) Or (ObjCompMonitorInspStatus.IsApplicable = False) Then
                                                            DueAsof2 = ""
                                                            RemainingTime2 = ""
                                                            AssemblyDueAsof2 = ""
                                                            AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                        Else
                                                            DueAsof2 = ObjCompMonitorInspStatusPeriod.DueOnValue
                                                            RemainingTime2 = ObjCompMonitorInspStatusPeriod.RemainingValue
                                                            If ObjCompMonitorInspStatusPeriod.PeriodID = 9 Then 'Added by Saylee on 20-Aug-2013 for ALL20082013,to show DueOnValue for Accumulated Cycles
                                                                AssemblyDueAsof2 = ObjCompMonitorInspStatusPeriod.DueOnValue
                                                            Else
                                                                AssemblyDueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText
                                                            End If
                                                            AirframeDueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                        End If
                                                        SinceNew2 = ObjCompMonitorInspStatusPeriod.CompCurrentValue
                                                        DoneAt2 = ObjCompMonitorInspStatusPeriod.DoneOnValue
                                                        Extension2 = ObjCompMonitorInspStatusPeriod.ExtensionValue
                                                        InstalledAt2 = ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").CompInstallationValue
                                                    Else
                                                        Freq3 = Freq3 & vbCrLf & ObjCompMonitorInspStatusPeriod.FrequencyValue
                                                        ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjCompMonitorInspStatusPeriod.AllElapsedValue


                                                        If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True And ObjCompMonitorInspStatus.IsApplicable = True) Or (ObjCompMonitorInspStatus.IsApplicable = False) Then
                                                            DueAsof2 = ""
                                                            RemainingTime2 = ""
                                                            AssemblyDueAsof2 = ""
                                                            AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                        Else
                                                            DueAsof2 = DueAsof2 & vbCrLf & ObjCompMonitorInspStatusPeriod.DueOnValue
                                                            RemainingTime2 = RemainingTime2 & vbCrLf & ObjCompMonitorInspStatusPeriod.RemainingValue

                                                            If ObjCompMonitorInspStatusPeriod.PeriodID = 9 Then 'Added by Saylee on 20-Aug-2013 for ALL20082013,to show DueOnValue for Accumulated Cycles
                                                                AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorInspStatusPeriod.DueOnValue
                                                            Else
                                                                AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText
                                                            End If
                                                            AirframeDueAsof2 = AirframeDueAsof2 & vbCrLf & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                        End If
                                                        SinceNew2 = SinceNew2 & vbCrLf & ObjCompMonitorInspStatusPeriod.CompCurrentValue
                                                        DoneAt2 = DoneAt2 & vbCrLf & ObjCompMonitorInspStatusPeriod.DoneOnValue
                                                        Extension2 = Extension2 & vbCrLf & ObjCompMonitorInspStatusPeriod.ExtensionValue
                                                        InstalledAt2 = InstalledAt2 & vbCrLf & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").CompInstallationValue
                                                    End If
                                                End If
                                            End If
                                            If ObjCompMonitorInspStatusPeriod.PeriodID = 2 Then
                                                If DoneAtAssembly = "" Then
                                                    DoneAtAssembly = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                Else
                                                    DoneAtAssembly = DoneAtAssembly & vbCrLf & ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                End If
                                            Else
                                                If DoneAtAssembly = "" Then
                                                    DoneAtAssembly = ObjCompMonitorInspStatusPeriod.AssemblyDoneOnValueTextFormatted
                                                Else
                                                    DoneAtAssembly = DoneAtAssembly & vbCrLf & ObjCompMonitorInspStatusPeriod.AssemblyDoneOnValueTextFormatted
                                                End If
                                            End If

                                        Next
                                        AssemblyID = ObjAssemblyStatus.AssemblyID
                                        AssemblyType = ObjAssemblyStatus.AssemblyType
                                        RegNo = ObjMachine.RegNo
                                        RequiredManHours = ObjCompMonitorInspStatus.RequiredManHours
                                        Customer = ObjMachine.Customer
                                        Note = ObjCompMonitorInspStatus.Notes + IIf(ObjCompMonitorInspStatus.IsApplicable = False, " NOT APPLICABLE", "")
                                        MaintenanceEvent = ObjCompMonitorInspStatus.Type
                                        ExtensionDate = ObjCompMonitorInspStatus.ExtensionDate
                                        ApprovalRemark = ObjCompMonitorInspStatus.ApprovalRemark
                                        'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                        If chkAirframeDueAsOf.Checked Then
                                            If (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL") Then 'Added by Saylee on 17-Sep-2019 Heligo17072019 as Heligo and UHPL needs Comp/Assembly DueOnValue,here DueAsOf is getting overright by AirframeDueAsOf
                                                'Do Nothing
                                            Else

                                                DueAsof = AirframeDueAsof
                                                DueAsof1 = AirframeDueAsof1
                                                DueAsof2 = AirframeDueAsof2
                                            End If
                                        End If
                                        'End

                                        If (ObjCompMonitorInspStatus.DoneOn <> "" And ObjCompMonitorInspStatus.MonitorType = "One Time") Or ObjCompMonitorInspStatus.IsApplicable = False Then
                                            EstimatedDate = ""
                                        End If
                                        If ObjCompMonitorInspStatus.CompMonitorInspStatusPeriodList.Count > 0 Then '5
                                            'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
                                            If ((Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL")) Then 'Added by Saylee 15-Sep-2010
                                                If (ObjCompMonitorInspStatus.IsApplicable = True) Then
                                                    If (DoneOnDate <> "" And (AssemblyDueAsof = "" And AssemblyDueAsof1 = "" And AssemblyDueAsof2 = "")) Then
                                                        'do nothing
                                                    Else
                                                        ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, MaintenanceEvent, AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, , MonitorTypeCode, Note, Remark, Description,
                                                                             , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue,
                                                                             AssemblyTypeID, MaintenanceEvent, , InstalledAt, InstalledAt1, InstalledAt2, , , , , , , , , , , , , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , , , , , , MaintenanceTypeID, MaintenanceTypeName, DoneONValueForAssembly:=DoneAtAssembly))
                                                    End If
                                                End If
                                            Else
                                                ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, MaintenanceEvent, AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, , MonitorTypeCode, Note, Remark, Description,
                                                                , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue,
                                                                AssemblyTypeID, MaintenanceEvent, , InstalledAt, InstalledAt1, InstalledAt2, , , , , , , , , , , , , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , , , , , , MaintenanceTypeID, MaintenanceTypeName, DoneONValueForAssembly:=DoneAtAssembly))
                                            End If
                                        End If
                                    End If
                                Next
                            Next
                        Next
                    Next
                End If
            Next
        End If

        If IsModSelect = True Then
            For i As Integer = 0 To ListDirectiveType.Items.Count - 1
                If ListDirectiveType.Items.Item(i).Selected Then
                    ' mMachineList = MachineList.GetMachineListDueMonitoringStatus(AsonDate, mDueLimits, MachineName, AssemblyName, AvgMnths, , mPerDayLimits, , False, False, True, , , ModificationTypeID(i))
                    mMachineList = MachineList.GetMachineListMonitoringStatus(AsonDate, MachineName, , , , , , , , , , True, True, , AssemblyName, , , , , , , , , , , , , , , True, , , , , , , False, , False, , True, , , , , CInt(ListDirectiveType.Items.Item(i).Value), , , True, IsAverageRequired:=True, ByPerDayLimit:=True, PerdayLimits:=mPerDayLimits, SkipIsForInventoryAircarft:=True)
                    For Each ObjMachine In mMachineList
                        For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                            For Each ObjAssemblyMonitorModStatus In ObjAssemblyStatus.AssemblyMonitorModStatusList

                                'chkNotApplicable checkbox added by Saylee on 17-Feb-2017 to show Not Applicable Records when checked
                                If (ObjAssemblyMonitorModStatus.IsApplicable = True And chkNotApplicable.Checked = False) Or (chkNotApplicable.Checked) Then 'Added By Vikrant On 22-May-2014 For All22052014-1
                                    'ATAChapter = ObjAssemblyMonitorModStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorModStatus.ATANomenclature
                                    If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                                        ATAChapter = ObjAssemblyMonitorModStatus.ATACode.ToString
                                    Else
                                        ATAChapter = ObjAssemblyMonitorModStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorModStatus.ATANomenclature
                                    End If
                                    'Description = ObjAssemblyMonitorModStatus.Description '& vbCrLf & ObjAssemblyMonitorModStatus.Number & vbCrLf & ObjAssemblyMonitorModStatus.Reference
                                    '-------------------------------------------------------------------------------------------------------------------------------------------------------
                                    If ObjAssemblyMonitorModStatus.Code = "Sup" And ObjAssemblyMonitorModStatus.MonitorType = "No Frequency" Then
                                        Description = "Superseded" & vbCrLf & ObjAssemblyMonitorModStatus.Description
                                    ElseIf ObjAssemblyMonitorModStatus.Code = "Ter" And ObjAssemblyMonitorModStatus.MonitorType = "No Frequency" Then
                                        Description = "Terminated" & vbCrLf & ObjAssemblyMonitorModStatus.Description
                                    ElseIf ObjAssemblyMonitorModStatus.MonitorType = "No Frequency" Then
                                        Description = "N/A" & vbCrLf & ObjAssemblyMonitorModStatus.Description
                                    Else
                                        If (ObjAssemblyMonitorModStatus.IsApplicable = False And ObjAssemblyMonitorModStatus.DoneOn = "" And ObjAssemblyMonitorModStatus.MonitorType = "One Time") Then
                                            Description = "One Time-N/A" & vbCrLf & ObjAssemblyMonitorModStatus.Description
                                        ElseIf (ObjAssemblyMonitorModStatus.DoneOn <> "" And ObjAssemblyMonitorModStatus.MonitorType = "One Time") Then
                                            Description = "One Time-Incorporated" & vbCrLf & ObjAssemblyMonitorModStatus.Description
                                        ElseIf (ObjAssemblyMonitorModStatus.DoneOn = "" And ObjAssemblyMonitorModStatus.MonitorType = "One Time") Then
                                            Description = "One Time-Open" & vbCrLf & ObjAssemblyMonitorModStatus.Description
                                        ElseIf (ObjAssemblyMonitorModStatus.IsApplicable = False And ObjAssemblyMonitorModStatus.DoneOn = "" And ObjAssemblyMonitorModStatus.MonitorType = "Reccurring") Then
                                            Description = "Reccurring-N/A" & vbCrLf & ObjAssemblyMonitorModStatus.Description
                                        Else
                                            Description = ObjAssemblyMonitorModStatus.Description
                                        End If
                                    End If
                                    '-------------------------------------------------------------------------------------------------------------------------------------------------------
                                    AssemblyModel = ObjAssemblyStatus.Model
                                    AssemblySerialNo = ObjAssemblyStatus.SerialNo
                                    Position = ""
                                    MonitorTypeCode = ObjAssemblyMonitorModStatus.Code
                                    EstimatedDate = ObjAssemblyMonitorModStatus.EstimatedDateFormatted
                                    MinimumRemainingValue = ObjAssemblyMonitorModStatus.MinimumRemainingValue
                                    AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID
                                    StatusMasterID = ObjAssemblyMonitorModStatus.ModelMonitorModID
                                    DocumentTypeForID = 8
                                    Remark = ObjAssemblyMonitorModStatus.DoneRemark
                                    Code = ObjAssemblyMonitorModStatus.ModelMonitorModCode
                                    DoneOnDate = ObjAssemblyMonitorModStatus.DoneOn
                                    MaintenanceTypeID = 3
                                    MaintenanceTypeName = "Directives"
                                    Freq1 = ""
                                    Freq2 = ""
                                    Freq3 = ""
                                    ElapsedTime = ""
                                    ElapsedTime1 = ""
                                    ElapsedTime2 = ""
                                    RemainingTime = ""
                                    RemainingTime1 = ""
                                    RemainingTime2 = ""
                                    DueAsof = ""
                                    DueAsof1 = ""
                                    DueAsof2 = ""
                                    AssemblyDueAsof = ""
                                    AssemblyDueAsof1 = ""
                                    AssemblyDueAsof2 = ""
                                    SinceNew = ""
                                    SinceNew1 = ""
                                    SinceNew2 = ""
                                    DoneAt = ""
                                    DoneAt1 = ""
                                    DoneAt2 = ""
                                    MaintenanceEvent = ""
                                    Extension = ""
                                    Extension1 = ""
                                    Extension2 = ""
                                    'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                    AirframeDueAsof = ""
                                    AirframeDueAsof1 = ""
                                    AirframeDueAsof2 = ""
                                    'End

                                    'Added By Saylee On 5-Dec-2018 For ALL05122018
                                    InstalledAt = ""
                                    InstalledAt1 = ""
                                    InstalledAt2 = ""
                                    'End
                                    DoneAtAssembly = ""

                                    For Each ObjAssemblyMonitorModStatusPeriod In ObjAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriodList
                                        If Report = 1 Then 'Portarait
                                            If ObjAssemblyMonitorModStatusPeriod.PeriodID = 1 Then
                                                Freq1 = ObjAssemblyMonitorModStatusPeriod.FrequencyValue
                                                ElapsedTime = ObjAssemblyMonitorModStatusPeriod.ElapsedValue

                                                If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True And ObjAssemblyMonitorModStatus.IsApplicable = True) Or (ObjAssemblyMonitorModStatus.IsApplicable = False) Then
                                                    DueAsof = ""
                                                    RemainingTime = ""
                                                    AssemblyDueAsof = ""
                                                    AirframeDueAsof = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                Else
                                                    DueAsof = ObjAssemblyMonitorModStatusPeriod.DueOnValue
                                                    RemainingTime = ObjAssemblyMonitorModStatusPeriod.RemainingValue
                                                    AssemblyDueAsof = ObjAssemblyMonitorModStatusPeriod.DueOnValue
                                                    AirframeDueAsof = ObjAssemblyMonitorModStatusPeriod.AssemblyDueOnValueTextByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                End If

                                                SinceNew = ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValue
                                                DoneAt = ObjAssemblyMonitorModStatusPeriod.DoneOnValue
                                                Extension = ObjAssemblyMonitorModStatusPeriod.ExtensionValue
                                                InstalledAt = ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyMonitorModStatusPeriod.PeriodID, "").AssemblyInstallationValue
                                            End If
                                            If ObjAssemblyMonitorModStatusPeriod.PeriodID = 2 Then
                                                Freq2 = ObjAssemblyMonitorModStatusPeriod.FrequencyValueFormatted
                                                ElapsedTime1 = ObjAssemblyMonitorModStatusPeriod.ElapsedValueFormatted

                                                If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True And ObjAssemblyMonitorModStatus.IsApplicable = True) Or (ObjAssemblyMonitorModStatus.IsApplicable = False) Then
                                                    DueAsof1 = ""
                                                    RemainingTime1 = ""
                                                    AssemblyDueAsof1 = ""
                                                    AirframeDueAsof1 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                Else
                                                    DueAsof1 = ObjAssemblyMonitorModStatusPeriod.DueOnValueFormatted
                                                    RemainingTime1 = ObjAssemblyMonitorModStatusPeriod.RemainingValueFormatted
                                                    AssemblyDueAsof1 = ObjAssemblyMonitorModStatusPeriod.DueOnValueFormatted
                                                    AirframeDueAsof1 = ObjAssemblyMonitorModStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame   'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                End If

                                                SinceNew1 = ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValueFormatted
                                                DoneAt1 = ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted
                                                Extension1 = ObjAssemblyMonitorModStatusPeriod.ExtensionValueFormatted
                                                InstalledAt1 = ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyMonitorModStatusPeriod.PeriodID, "").AssemblyInstallationValueFormatted
                                            End If
											'Added PeriodID=11 By Vikrant For ALL 21062012 
											'If ObjAssemblyMonitorModStatusPeriod.PeriodID = 3 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 4 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 5 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 6 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 7 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 8 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 9 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 12 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 13 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 14 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 15 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 11 Then
											'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
											If ObjAssemblyMonitorModStatusPeriod.PeriodID >= 3 Then
												If Freq3 = "" Then
													Freq3 = ObjAssemblyMonitorModStatusPeriod.FrequencyValue
													ElapsedTime2 = ObjAssemblyMonitorModStatusPeriod.ElapsedValue

													If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True And ObjAssemblyMonitorModStatus.IsApplicable = True) Or (ObjAssemblyMonitorModStatus.IsApplicable = False) Then
														DueAsof2 = ""
														RemainingTime2 = ""
														AssemblyDueAsof2 = ""
														AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
													Else
														DueAsof2 = ObjAssemblyMonitorModStatusPeriod.DueOnValue
														RemainingTime2 = ObjAssemblyMonitorModStatusPeriod.RemainingValue
														AssemblyDueAsof2 = ObjAssemblyMonitorModStatusPeriod.DueOnValue
														AirframeDueAsof2 = ObjAssemblyMonitorModStatusPeriod.AssemblyDueOnValueTextByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
													End If

													SinceNew2 = ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValue
													DoneAt2 = ObjAssemblyMonitorModStatusPeriod.DoneOnValue
													Extension2 = ObjAssemblyMonitorModStatusPeriod.ExtensionValue
													InstalledAt2 = ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyMonitorModStatusPeriod.PeriodID, "").AssemblyInstallationValue
												Else
													Freq3 = Freq3 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.FrequencyValue
													ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.ElapsedValue

													If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True And ObjAssemblyMonitorModStatus.IsApplicable = True) Or (ObjAssemblyMonitorModStatus.IsApplicable = False) Then
														DueAsof2 = ""
														RemainingTime2 = ""
														AssemblyDueAsof2 = ""
														AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
													Else
														DueAsof2 = DueAsof2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.DueOnValue
														RemainingTime2 = RemainingTime2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.RemainingValue
														AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.DueOnValue
														AirframeDueAsof2 = AirframeDueAsof2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.AssemblyDueOnValueTextByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
													End If

													SinceNew2 = SinceNew2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValue
													DoneAt2 = DoneAt2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.DoneOnValue
													Extension2 = Extension2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.ExtensionValue
													InstalledAt2 = InstalledAt2 & vbCrLf & ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyMonitorModStatusPeriod.PeriodID, "").AssemblyInstallationValue
												End If
											End If
										Else
                                            If ObjAssemblyMonitorModStatusPeriod.PeriodID = 2 Then
                                                If Freq3 = "" Then
                                                    Freq3 = ObjAssemblyMonitorModStatusPeriod.FrequencyValueFormatted
                                                    ElapsedTime2 = ObjAssemblyMonitorModStatusPeriod.ElapsedValueFormatted

                                                    If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True And ObjAssemblyMonitorModStatus.IsApplicable = True) Or (ObjAssemblyMonitorModStatus.IsApplicable = False) Then
                                                        DueAsof2 = ""
                                                        RemainingTime2 = ""
                                                        AssemblyDueAsof2 = ""
                                                        AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    Else
                                                        DueAsof2 = ObjAssemblyMonitorModStatusPeriod.DueOnValueFormatted
                                                        RemainingTime2 = ObjAssemblyMonitorModStatusPeriod.RemainingValueFormatted
                                                        AssemblyDueAsof2 = ObjAssemblyMonitorModStatusPeriod.DueOnValueFormatted
                                                        AirframeDueAsof2 = ObjAssemblyMonitorModStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    End If

                                                    SinceNew2 = ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValueFormatted
                                                    DoneAt2 = ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted
                                                    Extension2 = ObjAssemblyMonitorModStatusPeriod.ExtensionValueFormatted
                                                    InstalledAt2 = ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyMonitorModStatusPeriod.PeriodID, "").AssemblyInstallationValueFormatted
                                                Else
                                                    Freq3 = Freq3 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.FrequencyValueFormatted
                                                    ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.ElapsedValueFormatted

                                                    If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True And ObjAssemblyMonitorModStatus.IsApplicable = True) Or (ObjAssemblyMonitorModStatus.IsApplicable = False) Then
                                                        DueAsof2 = ""
                                                        RemainingTime2 = ""
                                                        AssemblyDueAsof2 = ""
                                                        AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    Else
                                                        DueAsof2 = DueAsof2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.DueOnValueFormatted
                                                        RemainingTime2 = RemainingTime2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.RemainingValueFormatted
                                                        AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.DueOnValueFormatted
                                                        AirframeDueAsof2 = AirframeDueAsof2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    End If

                                                    SinceNew2 = SinceNew2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValueFormatted
                                                    DoneAt2 = DoneAt2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted
                                                    Extension2 = Extension2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.ExtensionValueFormatted
                                                    InstalledAt2 = InstalledAt2 & vbCrLf & ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyMonitorModStatusPeriod.PeriodID, "").AssemblyInstallationValueFormatted
                                                End If
                                            Else
                                                If Freq3 = "" Then
                                                    Freq3 = ObjAssemblyMonitorModStatusPeriod.FrequencyValue
                                                    ElapsedTime2 = ObjAssemblyMonitorModStatusPeriod.ElapsedValue

                                                    If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True And ObjAssemblyMonitorModStatus.IsApplicable = True) Or (ObjAssemblyMonitorModStatus.IsApplicable = False) Then
                                                        DueAsof2 = ""
                                                        RemainingTime2 = ""
                                                        AssemblyDueAsof2 = ""
                                                        AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    Else
                                                        DueAsof2 = ObjAssemblyMonitorModStatusPeriod.DueOnValue
                                                        RemainingTime2 = ObjAssemblyMonitorModStatusPeriod.RemainingValue
                                                        AssemblyDueAsof2 = ObjAssemblyMonitorModStatusPeriod.DueOnValue
                                                        AirframeDueAsof2 = ObjAssemblyMonitorModStatusPeriod.AssemblyDueOnValueTextByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    End If

                                                    SinceNew2 = ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValue
                                                    DoneAt2 = ObjAssemblyMonitorModStatusPeriod.DoneOnValue
                                                    Extension2 = ObjAssemblyMonitorModStatusPeriod.ExtensionValue
                                                    InstalledAt2 = ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyMonitorModStatusPeriod.PeriodID, "").AssemblyInstallationValue
                                                Else
                                                    Freq3 = Freq3 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.FrequencyValue
                                                    ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.ElapsedValue

                                                    If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True And ObjAssemblyMonitorModStatus.IsApplicable = True) Or (ObjAssemblyMonitorModStatus.IsApplicable = False) Then
                                                        DueAsof2 = ""
                                                        RemainingTime2 = ""
                                                        AssemblyDueAsof2 = ""
                                                        AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    Else
                                                        DueAsof2 = DueAsof2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.DueOnValue
                                                        RemainingTime2 = RemainingTime2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.RemainingValue
                                                        AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.DueOnValue
                                                        AirframeDueAsof2 = AirframeDueAsof2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.AssemblyDueOnValueTextByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    End If

                                                    SinceNew2 = SinceNew2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValue
                                                    DoneAt2 = DoneAt2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.DoneOnValue
                                                    Extension2 = Extension2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.ExtensionValue
                                                    InstalledAt2 = InstalledAt2 & vbCrLf & ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyMonitorModStatusPeriod.PeriodID, "").AssemblyInstallationValue
                                                End If
                                            End If
                                        End If

                                        If ObjAssemblyMonitorModStatusPeriod.PeriodID = 2 Then
                                            If DoneAtAssembly = "" Then
                                                DoneAtAssembly = ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted
                                            Else
                                                DoneAtAssembly = DoneAtAssembly & vbCrLf & ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted
                                            End If
                                        Else
                                            If DoneAtAssembly = "" Then
                                                DoneAtAssembly = ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyMonitorModStatusPeriod.PeriodID, "").AssemblyInstallationValueFormatted
                                            Else
                                                DoneAtAssembly = DoneAtAssembly & vbCrLf & ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyMonitorModStatusPeriod.PeriodID, "").AssemblyInstallationValueFormatted
                                            End If
                                        End If

                                    Next
                                    AssemblyID = ObjAssemblyStatus.AssemblyID
                                    AssemblyType = ObjAssemblyStatus.AssemblyType
                                    RegNo = ObjMachine.RegNo
                                    RequiredManHours = ObjAssemblyMonitorModStatus.RequiredManHours
                                    Customer = ObjMachine.Customer
                                    Note = ObjAssemblyMonitorModStatus.Notes + IIf(ObjAssemblyMonitorModStatus.IsApplicable = False, " NOT APPLICABLE", "")
                                    MaintenanceEvent = ObjAssemblyMonitorModStatus.Type
                                    ExtensionDate = ObjAssemblyMonitorModStatus.ExtensionDate
                                    ApprovalRemark = ObjAssemblyMonitorModStatus.ApprovalRemark
                                    ModificationNumber = ObjAssemblyMonitorModStatus.Number

                                    'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                    If chkAirframeDueAsOf.Checked Then
                                        If (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL") Then 'Added by Saylee on 17-Sep-2019 Heligo17072019 as Heligo and UHPL needs Comp/Assembly DueOnValue,here DueAsOf is getting overright by AirframeDueAsOf
                                            'Do Nothing
                                        Else

                                            DueAsof = AirframeDueAsof
                                            DueAsof1 = AirframeDueAsof1
                                            DueAsof2 = AirframeDueAsof2
                                        End If
                                    End If
                                    'End
                                    If (ObjAssemblyMonitorModStatus.DoneOn <> "" And ObjAssemblyMonitorModStatus.MonitorType = "One Time") Or ObjAssemblyMonitorModStatus.IsApplicable = False Then
                                        EstimatedDate = ""
                                    End If
                                    If ObjAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriodList.Count > 0 Then '6
                                        'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
                                        If ((Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL")) Then 'Added by Saylee 15-Sep-2010
                                            If (ObjAssemblyMonitorModStatus.IsApplicable = True) Then
                                                If (DoneOnDate <> "" And (AssemblyDueAsof = "" And AssemblyDueAsof1 = "" And AssemblyDueAsof2 = "")) Then
                                                    'do nothing
                                                Else
                                                    ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, , AssemblySerialNo, ATAChapter, , , Position, , MonitorTypeCode, Note, Remark, Description,
                                                       , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel,
                                                       SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, AssemblyTypeID, MaintenanceEvent, , InstalledAt, InstalledAt1, InstalledAt2, , , , , , , , , , ModificationNumber, , , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , , , , , , MaintenanceTypeID, MaintenanceTypeName, DoneONValueForAssembly:=DoneAtAssembly))
                                                End If
                                            End If
                                        Else
                                            ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, , AssemblySerialNo, ATAChapter, , , Position, , MonitorTypeCode, Note, Remark, Description,
                                          , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel,
                                          SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, AssemblyTypeID, MaintenanceEvent, , InstalledAt, InstalledAt1, InstalledAt2, , , , , , , , , , ModificationNumber, , , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , , , , , , MaintenanceTypeID, MaintenanceTypeName, DoneONValueForAssembly:=DoneAtAssembly))
                                        End If
                                    End If
                                End If
                            Next
                            For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
                                For Each ObjCompMonitorModStatus In ObjCompStatus.CompMonitorModStatusList

                                    'chkNotApplicable checkbox added by Saylee on 17-Feb-2017 to show Not Applicable Records when checked
                                    If (ObjCompMonitorModStatus.IsApplicable = True And chkNotApplicable.Checked = False) Or (chkNotApplicable.Checked) Then 'Added By Vikrant On 22-May-2014 For All22052014-1
                                        'ATAChapter = ObjCompMonitorModStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorModStatus.ATANomenclature
                                        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                                            ATAChapter = ObjCompMonitorModStatus.ATACode.ToString
                                        Else
                                            ATAChapter = ObjCompMonitorModStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorModStatus.ATANomenclature
                                        End If
                                        'Description = ObjCompMonitorModStatus.Description '& vbCrLf & ObjCompMonitorModStatus.Number & vbCrLf & ObjCompMonitorModStatus.Reference
                                        '-----------------------------------------------------------------------------------------------------------------------------------------
                                        If ObjCompMonitorModStatus.Code = "Sup" And ObjCompMonitorModStatus.MonitorType = "No Frequency" Then
                                            Description = "Superseded" & vbCrLf & ObjCompMonitorModStatus.Description
                                        ElseIf ObjCompMonitorModStatus.Code = "Ter" And ObjCompMonitorModStatus.MonitorType = "No Frequency" Then
                                            Description = "Terminated" & vbCrLf & ObjCompMonitorModStatus.Description
                                        ElseIf ObjCompMonitorModStatus.MonitorType = "No Frequency" Then
                                            Description = "N/A" & vbCrLf & ObjCompMonitorModStatus.Description
                                        Else
                                            If (ObjCompMonitorModStatus.IsApplicable = False And ObjCompMonitorModStatus.DoneOn = "" And ObjCompMonitorModStatus.MonitorType = "One Time") Then
                                                Description = "One Time-N/A" & vbCrLf & ObjCompMonitorModStatus.Description
                                            ElseIf (ObjCompMonitorModStatus.DoneOn <> "" And ObjCompMonitorModStatus.MonitorType = "One Time") Then
                                                Description = "One Time-Incorporated" & vbCrLf & ObjCompMonitorModStatus.Description
                                            ElseIf (ObjCompMonitorModStatus.DoneOn = "" And ObjCompMonitorModStatus.MonitorType = "One Time") Then
                                                Description = "One Time-Open" & vbCrLf & ObjCompMonitorModStatus.Description
                                            ElseIf (ObjCompMonitorModStatus.IsApplicable = False And ObjCompMonitorModStatus.DoneOn = "" And ObjCompMonitorModStatus.MonitorType = "Reccurring") Then
                                                Description = "Reccurring-N/A" & vbCrLf & ObjCompMonitorModStatus.Description
                                            Else
                                                Description = ObjCompMonitorModStatus.Description
                                            End If
                                        End If
                                        '-----------------------------------------------------------------------------------------------------------------------------------------
                                        PartNo = ObjCompStatus.PartName
                                        CompSerialNo = ObjCompStatus.CompSerialNo
                                        Position = ObjCompStatus.Position
                                        MonitorTypeCode = ObjCompMonitorModStatus.Code
                                        EstimatedDate = ObjCompMonitorModStatus.EstimatedDateFormatted
                                        AssemblyModel = ObjAssemblyStatus.Model
                                        AssemblySerialNo = ObjAssemblyStatus.SerialNo
                                        MinimumRemainingValue = ObjCompMonitorModStatus.MinimumRemainingValue
                                        AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID
                                        StatusMasterID = ObjCompMonitorModStatus.PartMonitorModID
                                        DocumentTypeForID = 10
                                        Remark = ObjCompMonitorModStatus.DoneRemark
                                        Code = ObjCompMonitorModStatus.PartMonitorModCode
                                        DoneOnDate = ObjCompMonitorModStatus.DoneOn
                                        MaintenanceTypeID = 3
                                        MaintenanceTypeName = "Directives"
                                        Freq1 = ""
                                        Freq2 = ""
                                        Freq3 = ""
                                        ElapsedTime = ""
                                        ElapsedTime1 = ""
                                        ElapsedTime2 = ""
                                        RemainingTime = ""
                                        RemainingTime1 = ""
                                        RemainingTime2 = ""
                                        DueAsof = ""
                                        DueAsof1 = ""
                                        DueAsof2 = ""
                                        AssemblyDueAsof = ""
                                        AssemblyDueAsof1 = ""
                                        AssemblyDueAsof2 = ""
                                        SinceNew = ""
                                        SinceNew1 = ""
                                        SinceNew2 = ""
                                        DoneAt = ""
                                        DoneAt1 = ""
                                        DoneAt2 = ""
                                        MaintenanceEvent = ""
                                        Extension = ""
                                        Extension1 = ""
                                        Extension2 = ""
                                        'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                        AirframeDueAsof = ""
                                        AirframeDueAsof1 = ""
                                        AirframeDueAsof2 = ""
                                        'End

                                        'Added By Saylee On 5-Dec-2018 For ALL05122018
                                        InstalledAt = ""
                                        InstalledAt1 = ""
                                        InstalledAt2 = ""
                                        'End
                                        DoneAtAssembly = ""
                                        For Each ObjCompMonitorModStatusPeriod In ObjCompMonitorModStatus.CompMonitorModStatusPeriodList
                                            If Report = 1 Then 'Portarait
                                                If ObjCompMonitorModStatusPeriod.PeriodID = 1 Then
                                                    Freq1 = ObjCompMonitorModStatusPeriod.FrequencyValue
                                                    ElapsedTime = ObjCompMonitorModStatusPeriod.ElapsedValue


                                                    If (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted = True And ObjCompMonitorModStatus.IsApplicable = True) Or (ObjCompMonitorModStatus.IsApplicable = False) Then
                                                        DueAsof = ""
                                                        RemainingTime = ""
                                                        AssemblyDueAsof = ""
                                                        AirframeDueAsof = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    Else
                                                        DueAsof = ObjCompMonitorModStatusPeriod.DueOnValue
                                                        RemainingTime = ObjCompMonitorModStatusPeriod.RemainingValue
                                                        AssemblyDueAsof = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueText
                                                        AirframeDueAsof = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    End If
                                                    SinceNew = ObjCompMonitorModStatusPeriod.CompCurrentValue
                                                    DoneAt = ObjCompMonitorModStatusPeriod.DoneOnValue
                                                    Extension = ObjCompMonitorModStatusPeriod.ExtensionValue
                                                    InstalledAt = ObjCompStatus.CompStatusPeriodList(ObjCompMonitorModStatusPeriod.PeriodID, "").CompInstallationValue
                                                End If
                                                If ObjCompMonitorModStatusPeriod.PeriodID = 2 Then
                                                    Freq2 = ObjCompMonitorModStatusPeriod.FrequencyValueFormatted
                                                    ElapsedTime1 = ObjCompMonitorModStatusPeriod.ElapsedValueFormatted


                                                    If (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted = True And ObjCompMonitorModStatus.IsApplicable = True) Or (ObjCompMonitorModStatus.IsApplicable = False) Then
                                                        DueAsof1 = ""
                                                        RemainingTime1 = ""
                                                        AssemblyDueAsof1 = ""
                                                        AirframeDueAsof1 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    Else
                                                        DueAsof1 = ObjCompMonitorModStatusPeriod.DueOnValueFormatted
                                                        RemainingTime1 = ObjCompMonitorModStatusPeriod.RemainingValueFormatted
                                                        AssemblyDueAsof1 = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormatted
                                                        AirframeDueAsof1 = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                    End If
                                                    SinceNew1 = ObjCompMonitorModStatusPeriod.CompCurrentValueFormatted
                                                    DoneAt1 = ObjCompMonitorModStatusPeriod.DoneOnValueFormatted
                                                    Extension1 = ObjCompMonitorModStatusPeriod.ExtensionValueFormatted
                                                    InstalledAt1 = ObjCompStatus.CompStatusPeriodList(ObjCompMonitorModStatusPeriod.PeriodID, "").CompInstallationValueFormatted
                                                End If
												'Added PeriodID=11 By Vikrant For ALL 21062012
												'If ObjCompMonitorModStatusPeriod.PeriodID = 3 Or ObjCompMonitorModStatusPeriod.PeriodID = 4 Or ObjCompMonitorModStatusPeriod.PeriodID = 5 Or ObjCompMonitorModStatusPeriod.PeriodID = 6 Or ObjCompMonitorModStatusPeriod.PeriodID = 7 Or ObjCompMonitorModStatusPeriod.PeriodID = 8 Or ObjCompMonitorModStatusPeriod.PeriodID = 9 Or ObjCompMonitorModStatusPeriod.PeriodID = 12 Or ObjCompMonitorModStatusPeriod.PeriodID = 13 Or ObjCompMonitorModStatusPeriod.PeriodID = 14 Or ObjCompMonitorModStatusPeriod.PeriodID = 15 Or ObjCompMonitorModStatusPeriod.PeriodID = 11 Then
												'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
												If ObjCompMonitorModStatusPeriod.PeriodID >= 3 Then
													If Freq3 = "" Then
														Freq3 = ObjCompMonitorModStatusPeriod.FrequencyValue
														ElapsedTime2 = ObjCompMonitorModStatusPeriod.ElapsedValue


														If (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted = True And ObjCompMonitorModStatus.IsApplicable = True) Or (ObjCompMonitorModStatus.IsApplicable = False) Then
															DueAsof2 = ""
															RemainingTime2 = ""
															AssemblyDueAsof2 = ""
															AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
														Else
															DueAsof2 = ObjCompMonitorModStatusPeriod.DueOnValue
															RemainingTime2 = ObjCompMonitorModStatusPeriod.RemainingValue
															If ObjCompMonitorModStatusPeriod.PeriodID = 9 Then 'Added by Saylee on 20-Aug-2013 for ALL20082013,to show DueOnValue for Accumulated Cycles
																AssemblyDueAsof2 = ObjCompMonitorModStatusPeriod.DueOnValue
															Else
																AssemblyDueAsof2 = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueText
															End If
															AirframeDueAsof2 = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
														End If
														SinceNew2 = ObjCompMonitorModStatusPeriod.CompCurrentValue
														DoneAt2 = ObjCompMonitorModStatusPeriod.DoneOnValue
														Extension2 = ObjCompMonitorModStatusPeriod.ExtensionValue
														InstalledAt2 = ObjCompStatus.CompStatusPeriodList(ObjCompMonitorModStatusPeriod.PeriodID, "").CompInstallationValue
													Else
														Freq3 = Freq3 & vbCrLf & ObjCompMonitorModStatusPeriod.FrequencyValue
														ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjCompMonitorModStatusPeriod.ElapsedValue


														If (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted = True And ObjCompMonitorModStatus.IsApplicable = True) Or (ObjCompMonitorModStatus.IsApplicable = False) Then
															DueAsof2 = ""
															RemainingTime2 = ""
															AssemblyDueAsof2 = ""
															AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
														Else
															DueAsof2 = DueAsof2 & vbCrLf & ObjCompMonitorModStatusPeriod.DueOnValue
															RemainingTime2 = RemainingTime2 & vbCrLf & ObjCompMonitorModStatusPeriod.RemainingValue
															If ObjCompMonitorModStatusPeriod.PeriodID = 9 Then 'Added by Saylee on 20-Aug-2013 for ALL20082013,to show DueOnValue for Accumulated Cycles
																AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorModStatusPeriod.DueOnValue
															Else
																AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorModStatusPeriod.AssemblyDueOnValueText
															End If
															AirframeDueAsof2 = AirframeDueAsof2 & vbCrLf & ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
														End If
														SinceNew2 = SinceNew2 & vbCrLf & ObjCompMonitorModStatusPeriod.CompCurrentValue
														DoneAt2 = DoneAt2 & vbCrLf & ObjCompMonitorModStatusPeriod.DoneOnValue
														Extension2 = Extension2 & vbCrLf & ObjCompMonitorModStatusPeriod.ExtensionValue
														InstalledAt2 = InstalledAt2 & vbCrLf & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorModStatusPeriod.PeriodID, "").CompInstallationValue
													End If
												End If
											Else
                                                If ObjCompMonitorModStatusPeriod.PeriodID = 2 Then
                                                    If Freq3 = "" Then
                                                        Freq3 = ObjCompMonitorModStatusPeriod.FrequencyValueFormatted
                                                        ElapsedTime2 = ObjCompMonitorModStatusPeriod.ElapsedValueFormatted


                                                        If (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted = True And ObjCompMonitorModStatus.IsApplicable = True) Or (ObjCompMonitorModStatus.IsApplicable = False) Then
                                                            DueAsof2 = ""
                                                            RemainingTime2 = ""
                                                            AssemblyDueAsof2 = ""
                                                            AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                        Else
                                                            DueAsof2 = ObjCompMonitorModStatusPeriod.DueOnValueFormatted
                                                            RemainingTime2 = ObjCompMonitorModStatusPeriod.RemainingValueFormatted
                                                            AssemblyDueAsof2 = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormatted
                                                            AirframeDueAsof2 = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                        End If
                                                        SinceNew2 = ObjCompMonitorModStatusPeriod.CompCurrentValueFormatted
                                                        DoneAt2 = ObjCompMonitorModStatusPeriod.DoneOnValueFormatted
                                                        Extension2 = ObjCompMonitorModStatusPeriod.ExtensionValueFormatted
                                                        InstalledAt2 = ObjCompStatus.CompStatusPeriodList(ObjCompMonitorModStatusPeriod.PeriodID, "").CompInstallationValueFormatted
                                                    Else
                                                        Freq3 = Freq3 & vbCrLf & ObjCompMonitorModStatusPeriod.FrequencyValueFormatted
                                                        ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjCompMonitorModStatusPeriod.ElapsedValueFormatted


                                                        If (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted = True And ObjCompMonitorModStatus.IsApplicable = True) Or (ObjCompMonitorModStatus.IsApplicable = False) Then
                                                            DueAsof2 = ""
                                                            RemainingTime2 = ""
                                                            AssemblyDueAsof2 = ""
                                                            AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                        Else
                                                            DueAsof2 = DueAsof2 & vbCrLf & ObjCompMonitorModStatusPeriod.DueOnValueFormatted
                                                            RemainingTime2 = RemainingTime2 & vbCrLf & ObjCompMonitorModStatusPeriod.RemainingValueFormatted
                                                            AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormatted
                                                            AirframeDueAsof2 = AirframeDueAsof2 & vbCrLf & ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                        End If
                                                        SinceNew2 = SinceNew2 & vbCrLf & ObjCompMonitorModStatusPeriod.CompCurrentValueFormatted
                                                        DoneAt2 = DoneAt2 & vbCrLf & ObjCompMonitorModStatusPeriod.DoneOnValueFormatted
                                                        Extension2 = Extension2 & vbCrLf & ObjCompMonitorModStatusPeriod.ExtensionValueFormatted
                                                        InstalledAt2 = InstalledAt2 & vbCrLf & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorModStatusPeriod.PeriodID, "").CompInstallationValueFormatted
                                                    End If

                                                Else
                                                    If Freq3 = "" Then
                                                        Freq3 = ObjCompMonitorModStatusPeriod.FrequencyValue
                                                        ElapsedTime2 = ObjCompMonitorModStatusPeriod.ElapsedValue


                                                        If (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted = True And ObjCompMonitorModStatus.IsApplicable = True) Or (ObjCompMonitorModStatus.IsApplicable = False) Then
                                                            DueAsof2 = ""
                                                            RemainingTime2 = ""
                                                            AssemblyDueAsof2 = ""
                                                            AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                        Else
                                                            DueAsof2 = ObjCompMonitorModStatusPeriod.DueOnValue
                                                            RemainingTime2 = ObjCompMonitorModStatusPeriod.RemainingValue
                                                            If ObjCompMonitorModStatusPeriod.PeriodID = 9 Then 'Added by Saylee on 20-Aug-2013 for ALL20082013,to show DueOnValue for Accumulated Cycles
                                                                AssemblyDueAsof2 = ObjCompMonitorModStatusPeriod.DueOnValue
                                                            Else
                                                                AssemblyDueAsof2 = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueText
                                                            End If
                                                            AirframeDueAsof2 = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                        End If
                                                        SinceNew2 = ObjCompMonitorModStatusPeriod.CompCurrentValue
                                                        DoneAt2 = ObjCompMonitorModStatusPeriod.DoneOnValue
                                                        Extension2 = ObjCompMonitorModStatusPeriod.ExtensionValue
                                                        InstalledAt2 = ObjCompStatus.CompStatusPeriodList(ObjCompMonitorModStatusPeriod.PeriodID, "").CompInstallationValue
                                                    Else
                                                        Freq3 = Freq3 & vbCrLf & ObjCompMonitorModStatusPeriod.FrequencyValue
                                                        ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjCompMonitorModStatusPeriod.ElapsedValue


                                                        If (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted = True And ObjCompMonitorModStatus.IsApplicable = True) Or (ObjCompMonitorModStatus.IsApplicable = False) Then
                                                            DueAsof2 = ""
                                                            RemainingTime2 = ""
                                                            AssemblyDueAsof2 = ""
                                                            AirframeDueAsof2 = "" 'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                        Else
                                                            DueAsof2 = DueAsof2 & vbCrLf & ObjCompMonitorModStatusPeriod.DueOnValue
                                                            RemainingTime2 = RemainingTime2 & vbCrLf & ObjCompMonitorModStatusPeriod.RemainingValue
                                                            If ObjCompMonitorModStatusPeriod.PeriodID = 9 Then 'Added by Saylee on 20-Aug-2013 for ALL20082013,to show DueOnValue for Accumulated Cycles
                                                                AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorModStatusPeriod.DueOnValue
                                                            Else
                                                                AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorModStatusPeriod.AssemblyDueOnValueText
                                                            End If
                                                            AirframeDueAsof2 = AirframeDueAsof2 & vbCrLf & ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextByAirFrame  'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                                        End If
                                                        SinceNew2 = SinceNew2 & vbCrLf & ObjCompMonitorModStatusPeriod.CompCurrentValue
                                                        DoneAt2 = DoneAt2 & vbCrLf & ObjCompMonitorModStatusPeriod.DoneOnValue
                                                        Extension2 = Extension2 & vbCrLf & ObjCompMonitorModStatusPeriod.ExtensionValue
                                                        InstalledAt2 = InstalledAt2 & vbCrLf & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorModStatusPeriod.PeriodID, "").CompInstallationValue
                                                    End If
                                                End If

                                            End If
                                            If ObjCompMonitorModStatusPeriod.PeriodID = 2 Then
                                                If DoneAtAssembly = "" Then
                                                    DoneAtAssembly = ObjCompMonitorModStatusPeriod.DoneOnValueFormatted
                                                Else
                                                    DoneAtAssembly = DoneAtAssembly & vbCrLf & ObjCompMonitorModStatusPeriod.DoneOnValueFormatted
                                                End If
                                            Else
                                                If DoneAtAssembly = "" Then
                                                    DoneAtAssembly = ObjCompMonitorModStatusPeriod.AssemblyDoneOnValueTextFormatted
                                                Else
                                                    DoneAtAssembly = DoneAtAssembly & vbCrLf & ObjCompMonitorModStatusPeriod.AssemblyDoneOnValueTextFormatted
                                                End If
                                            End If

                                        Next
                                        AssemblyID = ObjAssemblyStatus.AssemblyID
                                        AssemblyType = ObjAssemblyStatus.AssemblyType
                                        RegNo = ObjMachine.RegNo
                                        RequiredManHours = ObjCompMonitorModStatus.RequiredManHours
                                        Customer = ObjMachine.Customer
                                        Note = ObjCompMonitorModStatus.Notes + IIf(ObjCompMonitorModStatus.IsApplicable = False, " NOT APPLICABLE", "")
                                        MaintenanceEvent = ObjCompMonitorModStatus.Type
                                        ExtensionDate = ObjCompMonitorModStatus.ExtensionDate
                                        ApprovalRemark = ObjCompMonitorModStatus.ApprovalRemark
                                        ModificationNumber = ObjCompMonitorModStatus.Number

                                        'Added By Vikrant On 12-Feb-2014 For ALL12022014
                                        If chkAirframeDueAsOf.Checked Then
                                            If (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL") Then 'Added by Saylee on 17-Sep-2019 Heligo17072019 as Heligo and UHPL needs Comp/Assembly DueOnValue,here DueAsOf is getting overright by AirframeDueAsOf
                                                'Do Nothing
                                            Else

                                                DueAsof = AirframeDueAsof
                                                DueAsof1 = AirframeDueAsof1
                                                DueAsof2 = AirframeDueAsof2
                                            End If
                                        End If
                                        'End
                                        If (ObjCompMonitorModStatus.DoneOn <> "" And ObjCompMonitorModStatus.MonitorType = "One Time") Or ObjCompMonitorModStatus.IsApplicable = False Then
                                            EstimatedDate = ""
                                        End If
                                        If ObjCompMonitorModStatus.CompMonitorModStatusPeriodList.Count > 0 Then '1
                                            'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
                                            If ((Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL")) Then 'Added by Saylee 15-Sep-2010
                                                If (ObjCompMonitorModStatus.IsApplicable = True) Then
                                                    If (DoneOnDate <> "" And (AssemblyDueAsof = "" And AssemblyDueAsof1 = "" And AssemblyDueAsof2 = "")) Then
                                                        'do nothing
                                                    Else
                                                        ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, MaintenanceEvent, AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, , MonitorTypeCode, Note, Remark, Description,
                                                        , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue,
                                                        AssemblyTypeID, MaintenanceEvent, , InstalledAt, InstalledAt1, InstalledAt2, , , , , , , , , , ModificationNumber, , , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , , , , , , MaintenanceTypeID, MaintenanceTypeName, DoneONValueForAssembly:=DoneAtAssembly))
                                                    End If
                                                End If
                                            Else
                                                ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, MaintenanceEvent, AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, , MonitorTypeCode, Note, Remark, Description,
                                                , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue,
                                                AssemblyTypeID, MaintenanceEvent, , InstalledAt, InstalledAt1, InstalledAt2, , , , , , , , , , ModificationNumber, , , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , , , , , , MaintenanceTypeID, MaintenanceTypeName, DoneONValueForAssembly:=DoneAtAssembly))
                                            End If
                                        End If
                                    End If
                                Next
                            Next
                        Next
                    Next
                End If
            Next
        End If

        Return ReportMaintenanceDetails
    End Function
    Private Sub SetReport(Optional ByVal ByMail As Boolean = False, Optional ByVal IsExcel As Boolean = False)  'Parameter Added by Shital on 14-Sep-2016
        ReportMaintenanceDetails = New ReportMaintenanceDetailList
        ReportStatusList = New rptStatusList
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsReportMaintenanceDetail
        Dim rptMachineCertificates As MachineCertificateList
        Dim mCompanyDetail As New CompanyDetail
        Dim searchstr As String = ""
        Dim x As String
        Dim ReportLabel As String
        Dim OperatorName As String = ""
        Dim rptMasterTimeControlList As CrystalDecisions.CrystalReports.Engine.ReportClass

        SetValues()
        ReportDetail()
        Dim mloglist As LogList
        mloglist = LogList.GetLogList(New Guid(cmbAircraft.SelectedValue.ToString), , AsonDate)

        searchstr = New SmartDate(txtFromDate.Text).FormattedText

        'Added By Vikrant On 13-Feb-2014 For ALL12022014
        Dim searchstr1 As String
        Dim mPerDayLimit As PerDayLimit
        For Each mPerDayLimit In mPerDayLimits
            If CDec(Val(mPerDayLimit.PeriodLimit)) >= 0 Then
                If searchstr1 = "" Then
                    searchstr1 = "Estimated Due Date as" & " " & searchstr1 & " " & mPerDayLimit.PeriodLimit & " " & mPerDayLimit.PeriodName
                Else
                    searchstr1 = searchstr1 & ", " & mPerDayLimit.PeriodLimit & " " & mPerDayLimit.PeriodName
                End If
            End If
        Next
        searchstr1 = searchstr1 & " per Day "
        'End
        'Added by Ajay 14-08-2023
        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
            If cmbAircraft.SelectedIndex > 0 Then
                mLastAMPRef = LastMPDAMPRef.GetLastMPDAMPRefForMachine(MachineID:=New Guid(cmbAircraft.SelectedValue.ToLower))
                Session("mLastAMPRef") = mLastAMPRef
                If (mLastAMPRef.AMPNo <> "") Then AMPNo = "AMP No.: " + mLastAMPRef.AMPNo + ",Rev No.: " + mLastAMPRef.RevNo + ",Dated: " + mLastAMPRef.FromDateFormatted
            Else
                AMPNo = ""
            End If
        End If
        'If Not cmbAircraft.SelectedItem.ToString = "(All)" Then "<SELECT>"
        If Not cmbAircraft.SelectedItem.ToString = "<SELECT>" Then
            rptMachineCertificates = MachineCertificateList.GetMachineCertificateList(New Guid(cmbAircraft.SelectedValue.ToString), AsonDate)
            If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL") Then 'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
                rptMasterTimeControlList = New crMasterTimeControlListHeligo
                ReportLabel = "Service/Inspection/Modification status"
            Else
                If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                    searchstr1 = ""
                    rptMasterTimeControlList = New crMasterTimeControlListForTaskNo
                Else
                    rptMasterTimeControlList = New crMasterTimeControlList
                End If
                ReportLabel = "Master Time Control List"
            End If
        End If
        If mloglist.Count > 0 Then
            x = mloglist(0).LogDate.ToShortDateString
        Else
            x = txtFromDate.Text
        End If

        'Added by Prashant on 11-Aug-2011
        If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "Indamer")) Then
            Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue))
            If mMachineOperatorName.OperatorName <> "" Then OperatorName = mMachineOperatorName.OperatorName
        End If

        'Added By Vikrant On 27-Feb-2020 for showing Periods Code and their long forms at bottom of report
        Dim mPeriodUnitList As PeriodUnitList
        Dim PeriodsShortName As New StringBuilder

        mPeriodUnitList = PeriodUnitList.GetPeriodUnitList()
        For i As Integer = 0 To mPeriodUnitList.Count - 1
            PeriodsShortName.Append(mPeriodUnitList(i).Code + "-" + mPeriodUnitList(i).PeriodUnitName + ", ")
        Next
        'End

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
    mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
    mCompanyDetail.WebSite, ReportLabel, searchstr, searchstr1, Assembly1, "",
    "Aircraft is flown up to: " & New SmartDate(x).FormattedText, AppSettings("Product Version"), AppSettings("SINote"), "",
    OperatorName, "", cmbAircraft.SelectedItem.ToString, AppSettings("Logo"), SearchStr17:=PeriodsShortName.ToString.Trim.TrimEnd(","),
    SearchStr18:=ServicesShortName, SearchStr19:=InspsShortName, SearchStr20:=DirectiveShortName, SearchStr21:=AMPNo) 'Changed By Utkarsh For Report Logo.


        If ByMail = False Then  ' If case added by shital on 14-Sep-2016  
            If ReportMaintenanceDetails.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            ElseIf Not IsExcel Then
                RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 1158)
            End If
        End If

        ' added by shital on 14-Sep-2016  
        If (ByMail = True And ReportMaintenanceDetails.Count <= 0) Then
            SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, ReportLabel, "", "There is no record for this search criteria.", "", Session("ToSendMailIDs"), "", "", True, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                      SmtpHost:=mModuleList.Item("MasterTimeControlList").SmtpHost, SmtpPort:=mModuleList.Item("MasterTimeControlList").SmtpPort, _
                                      SmtpUser:=mModuleList.Item("MasterTimeControlList").SmtpUser, SmtpPassword:=mModuleList.Item("MasterTimeControlList").SmtpPassword)
            Exit Sub
        End If


        ds.Clear()
        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        '----------------------------------------------------------
        da.Fill(ds, ReportMaintenanceDetails)
        da.Fill(ds, Report)
        da.Fill(ds, ReportStatusList)
        da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
        rptMasterTimeControlList.SetDataSource(ds)
        Session("CrystalReport") = rptMasterTimeControlList

        'added by shital on 14-Sep-2016
        If (ByMail = True) Then
            SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, ReportLabel, "", " For " + lblAircraft1.Text, , Session("ToSendMailIDs"), "", "", True, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                      SmtpHost:=mModuleList.Item("MasterTimeControlList").SmtpHost, SmtpPort:=mModuleList.Item("MasterTimeControlList").SmtpPort, _
                                      SmtpUser:=mModuleList.Item("MasterTimeControlList").SmtpUser, SmtpPassword:=mModuleList.Item("MasterTimeControlList").SmtpPassword)
        Else
            If Not IsExcel Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
            Else
                Dim PeriodColumnsForExportToExcel As New List(Of String)
                ds.Clear()

                da.Fill(ds, "ExcelReportMaintenanceDetailList", ReportMaintenanceDetails)
                da.Fill(ds, "ExcelReport", Report)

                Dim columnToRemove As String() = { _
                                                    "ID", "Code", "Name", "Model", "EstDate", "SerialNo", "MonitorType", "Freq2", _
                                                    "Freq3", "ElapsedTime1", "ElapsedTime2", "RemainingTime1", "RemainingTime2", "DueAsof1", "DueAsof2", _
                                                     "ComponentInfo", "RegNo", "SinceNew", "SinceNew1", "DoneAt", _
                                                    "DoneAt1", "MinimumRemainingValue", "AssemblyTypeID", "MaintenanceEvent", "InstalledAt", _
                                                    "InstalledAt1", "InstalledAt2", "TSO1", "TSO2", "RemoveAt1", "RemoveAt2", "ModificationNumber", _
                                                    "DoneWONo", "DetailID", "Applicability", "ComplianceRequirement", "AssemblyDueAsof", "AssemblyDueAsof1", _
                                                    "AssemblyDueAsof2", "Extension", "Extension1", "Extension2", "ExtensionDate", "ApprovalRemark", _
                                                    "RequiredManHours", "Note", "Reference", "ReferenceForExcel", "IsRII", "SourceDoc", "ModelEstimatedManHours", "EstimatedDate", _
                                                    "Customer", "Description", "DoneAt2", "RemainingTime", "ElapsedTime", _
                                                    "SupersededByADNumber", _
                                                    "IssueDate", _
                                                    "IsApplicable", _
                                                    "MaintenanceTypeID", _
                                                    "MaintenanceTypeName", _
                                                    "IsLater", _
                                                    "DueStatus", _
                                                    "ModelMonitorModCode", _
                                                    "StatusTypeName", _
                                                    "WONumber", _
                                                    "StatusMasterID", _
                                                    "StatusID", _
                                                    "TypeID", _
                                                    "CompStatusID", _
                                                    "AssemblyStatusID", _
                                                    "DocumentTypeForID", _
                                                    "MaintenanceOn", _
                                                    "MaintenanceInformation", _
                                                    "MaintenanceInfo", _
                                                    "Frequency", _
                                                    "SinceNewAll", _
                                                    "ElapsedAll", _
                                                    "DoneAtAll", _
                                                    "ExtensionAll", _
                                                    "DueAsofAll", _
                                                    "AssDueAsofAll", _
                                                    "RemainingTimeAll", _
                                                    "LogBook", _
                                                    "DoneOnValue", _
                                                    "DoneOnDate", _
                                                    "RemoveAt", _
                                                    "ATACode", _
                                                    "InstalledAtDate", _
                                                    "RemoveAtDate", _
                                                    "TSO", _
                                                    "TSN", _
                                                    "DoneONValueForAssembly", _
                                                    "RecordID", _
                                                    "MachineID", _
                                                    "ModelID", _
                                                    "IsMaster", _
                                                    "DiffCompInstDoneOnValue", _
                                                    "EffectiveFromAll", _
                                                    "MaintenanceOnExcel", _
                                                    "MaintenanceInformationExcel", _
                                                    "MaintenanceInfoExcel", _
                                                    "SinceNewAllExcel", _
                                                    "EffectiveFromAllExcel", _
                                                    "ExtensionAllExcel", "MaintenanceInformationForExcel", "ApplicabilityForExcel", "Freq1", "TimeSinceNew", _
                                                    "DueAsof", _
                                                    "AssDueAsofAllExcel", _
                                                     "EROQtyNosForMaterialMgmtReport", "POQtyNosForMaterialMgmtReport", _
                                                    "PONosForMaterialMgmtReport", "POQtyForMaterialMgmtReport", "ERONosForMaterialMgmtReport", "SinceNew2", _
                                                    "EROQtyForMaterialMgmtReport", "UnserviceableStockQty", "ServiceableStockQty", "BinCardTotalQty", "Area", _
                                                    "Zone", "WONoExcel", "ThresholdAccordingToTypeIDForExcel", "FrequencyAccordingToTypeIDForExcel", "DueAsOfAssemblyOrCompForExcel", "DueAsOfAirframeForExcel", "RemainingForExcel"
 _
                                            }
                For i As Integer = 0 To columnToRemove.Length - 1
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains(columnToRemove(i)) Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns.Remove(columnToRemove(i))
                    End If
                Next


                ds.Tables("ExcelReportMaintenanceDetailList").Columns("AssemblyType").SetOrdinal(2)
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("AssemblyModel").SetOrdinal(3)
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("AssemblySerialNo").SetOrdinal(4)
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("PartNo").SetOrdinal(5)
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("CompSerialNo").SetOrdinal(6)
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("DoneAtAllExcel").SetOrdinal(12)


                Dim columnscnt As Integer = ds.Tables("ExcelReportMaintenanceDetailList").Columns.Count
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("Remark").SetOrdinal(columnscnt - 1)
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("NoteForExcel").SetOrdinal(columnscnt - 2)


                Dim DueLabel As String = "Due As of"
                For i As Integer = 0 To ds.Tables("ExcelReportMaintenanceDetailList").Columns.Count - 1
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "AssemblyModel" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Assembly Model"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "AssemblySerialNo" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Assembly Serial No."
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "AssemblyType" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Assembly"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "FrequencyExcel" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Frequency"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "NoteForExcel" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Note"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "RemainingTimeAllExcel" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Remaining Time"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "DescriptionForExcel" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Description"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "MonitorTypeCode" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Monitor Type"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "DoneAtAllExcel" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Done On Or Effective From"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "ElapsedAllExcel" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Elapsed"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "DueAsofAllExcel" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Due As of"
                    End If
                    If (AppSettings("ClientCode") = "RAL" Or AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then
                        If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Due As of" Then
                            ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Due As of Airframe"
                            DueLabel = "Due As of Airframe"
                        End If
                    End If
                Next

                Dim columnToRemoveCriteria As String() = {"ReportDate", "ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ShortName", _
                                                           "ReportName", "SearchStr2", "SearchStr8", "SearchStr4", "SearchStr5", "SearchStr6", "SearchStr7", _
                                                           "ProductVersion", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr10", "SearchStr11", _
                                                           "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", _
                                                          "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"
                                                          }

                For i As Integer = 0 To columnToRemoveCriteria.Length - 1
                    If ds.Tables("ExcelReport").Columns.Contains(columnToRemoveCriteria(i)) Then
                        ds.Tables("ExcelReport").Columns.Remove(columnToRemoveCriteria(i))
                    End If
                Next

                For i As Integer = 0 To ds.Tables("ExcelReport").Columns.Count - 1
                    If ds.Tables("ExcelReport").Columns(i).ColumnName = "SearchStr1" Then
                        ds.Tables("ExcelReport").Columns(i).ColumnName = "Report Date"
                    End If
                    If ds.Tables("ExcelReport").Columns(i).ColumnName = "SearchStr9" Then
                        ds.Tables("ExcelReport").Columns(i).ColumnName = "Reg No."
                    End If
                    If ds.Tables("ExcelReport").Columns(i).ColumnName = "SearchStr3" Then
                        ds.Tables("ExcelReport").Columns(i).ColumnName = "Assembly"
                    End If
                Next

                Dim dataview As DataView = ds.Tables("ExcelReportMaintenanceDetailList").DefaultView
                dataview.Sort = "ATAChapter"
                ds.Tables("ExcelReportMaintenanceDetailList").TableName = IIf(AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL", "Service Inspection Modification Status", "Master Time Control List")

                '  ds.Tables("ExcelReportStatusList").TableName = "Searching Criteria"
                ds.Tables("ExcelReport").TableName = "Searching Criteria"
                Session("DataTableToBeFormattedForExportToExcel") = IIf(AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL", "Service Inspection Modification Status", "Master Time Control List")
                Dim dsNew As New DataSet
                dsNew.Clear()

				Session("ExcelFileName") = ds.Tables("ExcelReportMaintenanceDetailList").TableName
				dsNew.Merge(ds.Tables("Searching Criteria"))
				dsNew.Merge(dataview.ToTable())



                PeriodColumnsForExportToExcel.AddRange(New String() {"Frequency", "ElapsedTime", "RemainingTime", DueLabel, "DoneOn Value", "Done On Or Effective From"})
                Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel


                'Dim list = (From c In ds.Tables("ExcelReportMaintenanceDetailList") Order By ATAChapter
                '                             Select c Order By ATAChapter).ToList

                Session("dsNew") = dsNew
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            End If

            'MarkLog(Util.Action.Print, ReportLabel, mSearchCriteriaForEventLog, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        If IsExcel Then 'Added by Prashant on 19-Jan-2021
            MarkLog(Util.Action.Print, "MasterTimeControlList", "Export To Excel " + mSearchCriteriaForEventLog & "," & searchstr1, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        Else
            MarkLog(Util.Action.Print, "MasterTimeControlList", mSearchCriteriaForEventLog & "," & searchstr1, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    '
                Case MsgBoxResult.No
                    '
                Case MsgBoxResult.Ok
                    Session("Sender") = ""
                    'Response.Redirect("wfSearchCriteriaForMasterTimeControlList_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
            'Response.Redirect("wfSearchCriteriaForMasterTimeControlList_Ajax.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        End If
    End Sub
#End Region

#Region " Data Binding "
    Public Sub SetTypeCombo()

        If mServiceTypeList Is Nothing Then
            mServiceTypeList = PartMonitorServiceTypeList.GetPartMonitorServiceTypeListForNoFrequency(, , True)
        End If
        ListServiceType.DataSource = mServiceTypeList
        Session("mServiceTypeList") = mServiceTypeList

        If mInspectionTypeList Is Nothing Then
            mInspectionTypeList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList()
        End If
        ListInspectionType.DataSource = mInspectionTypeList
        Session("mInspectionTypesList") = mInspectionTypeList

        If mModificationTypeList Is Nothing Then
            mModificationTypeList = ModelMonitorModTypeList.GetModelMonitorModTypeListForNoFrequency(, , True)
        End If

        ListDirectiveType.DataSource = mModificationTypeList
        Session("mModificationTypeList") = mModificationTypeList
        DataBind()
        FillMonitorTypeList()
    End Sub
    Public Sub SetComboOfMachine(ByVal AsonDate As String)
        mMachineNameValueList = MachineNameValueList.GetMachineList(AsonDate, , , , , , , True, "(SELECT)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataBind()
    End Sub
    Private Sub FillMonitorTypeList()
        chkService.Checked = True
        chkInspection.Checked = True
        chkDirective.Checked = True

        For i As Integer = 0 To ListServiceType.Items.Count - 1
            ListServiceType.Items(i).Selected = True
        Next

        For i As Integer = 0 To ListInspectionType.Items.Count - 1
            ListInspectionType.Items(i).Selected = True
        Next

        For i As Integer = 0 To ListDirectiveType.Items.Count - 1
            ListDirectiveType.Items(i).Selected = True
        Next

    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("Sender") = "" Then
            Session("MiddleFrame") = "wfSearchCriteriaForMasterTimeControlList_Ajax.aspx?"
            lblAssembly.Enabled = False
            cmbAssembly.Enabled = False
            txtFromDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            AOnDate = Now.Date.ToString(AppSettings("DateFormat"))
            SetComboOfMachine(AOnDate)
            DataFieldBind()
            setFocus(cmbAircraft)
            SetTypeCombo()
            Report = 1
            Session("Report") = Report

            SetSession()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        If IsValid Then
            Display()
            SetValues()
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid = True Then
            SetReport(False)
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub txtFromDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFromDate.TextChanged
        AOdate = txtFromDate.Text
        If AOnDate = AOdate Then
        Else
            SetComboOfMachine(AOdate)
        End If
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        If cmbAircraft.SelectedIndex = 0 Then
            lblAssembly.Enabled = False
            cmbAssembly.Enabled = False
            cmbAssembly.SelectedIndex = 0
        Else
            lblAssembly.Enabled = True
            cmbAssembly.Enabled = True

            Dim mAssemblylist As AssemblyList
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, txtFromDate.Text, "(All)", True)
            Session("mAssemblyList") = mAssemblylist
            cmbAssembly.DataSource = mAssemblylist
            cmbAssembly.DataBind()
        End If
        If cmbAircraft.Enabled = True Then
            setFocus(cmbAircraft)
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    'Added by Shital on 14-Sep-2016
    Private Sub btnByMail_Click(sender As Object, e As System.EventArgs) Handles btnByMail.Click
        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        ' Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail

        Session("UserEmailID") = mModuleList.Item("MasterTimeControlList").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("MasterTimeControlList").SendCCMailID
        '--------------------------
        Dim Str As String
        Str = "OpenByMaiWindow();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
    End Sub
    Private Sub hdnimgLogBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgLogBtnSendMail.Click
        Dim email As Thread
        Try
            email = New Thread(Sub() SetReport(True))
            email.IsBackground = True
            email.Start()
        Catch ex As Exception
            Dim Day, Month, Year As String
            Day = Format(Today.Date.Day, "0#")
            Month = Format(Today.Date.Month, "0#")
            Year = Format(Today.Date.Year, "0#")
            Dim todaydate As String = Day & Month & Year
            Dim Path As String = AppSettings("DOCPath") & todaydate
            FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (hdnimgMELBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        End Try
    End Sub
    Private Sub btnExport_Click(sender As Object, e As System.EventArgs) Handles btnExport.Click
        If IsValid = True Then
            SetReport(IsExcel:=True)
        End If
    End Sub
#End Region

   
End Class