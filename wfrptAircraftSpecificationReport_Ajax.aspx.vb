'Ajax Conversion By Vikrant On 24-Jan-2014

Public Class wfrptAircraftSpecificationReport_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Private mMachineNameValueList As MachineNameValueList
    Dim mACSpecsRadioEquipmentList As ACSpecsRadioEquipmentList
    Dim mACSpecsAircraftIdentification As ACSpecsAircraftIdentification
    Dim mACSpecsCertificateOfRegistration As ACSpecsCertificateOfRegistration
    Dim mACSpecsAirframeStatus As ACSpecsAirframeStatus
    Dim mACSpecsAPU As ACSpecsAPU
    Dim mACSpecsInstalledEngines As ACSpecsInstalledEngines
    Dim mACSpecsPrincipalOperatingWeights As ACSpecsPrincipalOperatingWeights
    Dim mACSpecsAircraftFeatures As ACSpecsAircraftFeatures
    Dim mACSpecsMachineStructuralInsp As ACSpecsMachineStructuralInsp
    Dim mACSpecsMachineMaintPolicies As ACSpecsMachineMaintPolicies

    Public mtmpComplyAssemblyMonitorServiceStatusList As tmpComplyAssemblyMonitorServiceStatusList
    Public mtmpComplyAssemblyMonitorModStatusList As tmpComplyAssemblyMonitorModStatusList


    Dim mMachineList As MachineList
    Dim ReportMaintenanceDetails As New ReportMaintenanceDetailList
    Dim ReportMaintenanceDetailsForDirectives As New ReportMaintenanceDetailListForDirectives


    Private Description, ATAChapter, Position, MonitorTypeCode, StatusType, MonitorType As String
    Private Freq1 As String
    Private Freq2 As String
    Private Freq3 As String
    Private ElapsedTime As String
    Private ElapsedTime1 As String
    Private ElapsedTime2 As String
    Private RemainingTime As String
    Private RemainingTime1 As String
    Private RemainingTime2 As String
    Private DoneAt2 As String
    Private TimeSinceNew As String
    Dim Periodcount As Integer
    Dim Count, Count1 As Integer
    Private DoneOnValueDate, SerialNoPostion As String
    Dim AssemblyID As Guid
    Dim searchstr7 As String = ""
    'Dim mModificationTypeList As ModelMonitorModTypeList
    Private AssemblyModel, AssemblySerialNo, DueAsof, DoneOnValue, EstimatedDate, Code, Number, Reference, DoneOnDate, Note As String
    Private AssemblyTypeID As Integer
    Private IssueDate As SmartDate = New SmartDate(True)
    Private IsApplicable As Boolean
    Private Applicability, ComplianceRequirement, ModelMonitorModCode, PartNo, CompSerialNo, DoneWONo, Remark As String
    Dim Report As Integer = 1
    Dim ShowCofA As Boolean = False
    Dim EventLogDetail As String = String.Empty
#End Region

#Region "Business Methods"
    Private Sub SetSession()
        Session("mMachineNameValueList") = mMachineNameValueList
    End Sub
    Private Sub GetSession()
        mMachineNameValueList = Session("mMachineNameValueList")
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMachineNameValueList")
    End Sub
#End Region

#Region "Data Binding"
    Private Sub SetCombo()
        If cmbYear.Items.Count = 0 Or cmbYear.SelectedValue = "" Then
            For i As Integer = -10 To 10
                cmbYear.Items.Add(DateAdd(DateInterval.Year, i, Today.Date).Year)
            Next
            cmbYear.SelectedIndex = 10
        End If

        For k As Integer = 1 To 12
            Dim mon As String = MonthName(k, False)
            cmbMonth.Items.Add(mon)
        Next
    End Sub
    Private Sub DataFieldBinding()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Today.Date.ToShortDateString, , , , , , , True, "(SELECT)", , True)
        cmbMachine.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbMachine.DataBind()
    End Sub
    Private Sub Display()
        lblSummary.Visible = True
        lblyear1.Visible = True
        lblModel1.Visible = True
        upnlCriteria.Update()
    End Sub
    Public Function ReportDetailForADs() As ReportMaintenanceDetailListForDirectives
        Dim ObjMachine As MachineInfo
        Dim ObjAssemblyStatus As AssemblyStatusInfo
        Dim ObjCompStatus As CompStatusInfo
        Dim ObjAssemblyMonitorModStatus As AssemblyMonitorModStatusInfo
        Dim ObjAssemblyMonitorModStatusPeriod As AssemblyMonitorModStatusPeriodInfo
        Dim ObjCompMonitorModStatus As CompMonitorModStatusInfo
        Dim ObjCompMonitorModStatusPeriod As CompMonitorModStatusPeriodInfo

        Dim mAssemblyList As AssemblyList = AssemblyList.GetAssemblyList(1, cmbMachine.SelectedValue.ToString)

        mMachineList = MachineList.GetMachineListMonitoringStatus(DateSerial(cmbYear.SelectedValue, cmbMonth.SelectedIndex + 1, 1).ToString, cmbMachine.SelectedValue.ToString, , , , , , , , , , , True, , mAssemblyList(0).ID.ToString, SkipIsForInventoryAircarft:=True)
        Dim LHLabel2 As String = ""
        Dim LHData2 As String = ""
        Dim RHLabel1 As String = ""
        Dim RHData1 As String = ""
        Dim RHLabel2 As String = ""
        Dim RHData2 As String = ""
        Dim RHData3 As String = ""

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
                '---------------------------------------------------------------------------------------------------------
                Periodcount = ObjAssemblyStatus.AssemblyStatusPeriodList.Count()
                For Count1 = 0 To Periodcount - 1
                    If ObjAssemblyStatus.AssemblyStatusPeriodList(Count1).PeriodID <> 2 Then
                        RHLabel1 = CType(IIf(RHLabel1 = "", RHLabel1, RHLabel1 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count1).PeriodName
                        RHData1 = CType(IIf(RHData1 = "", RHData1, RHData1 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count1).PeriodID, "").AssemblyCurrentValue
                    Else
                        RHLabel1 = CType(IIf(RHLabel1 = "", RHLabel1, RHLabel1 + vbNewLine), String) + "Mfg. Date"
                        RHData1 = CType(IIf(RHData1 = "", RHData1, RHData1 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count1).PeriodID, "").AssemblyStartValueFormatted
                    End If
                Next
                If ObjAssemblyStatus.Position = "" Then
                    RHLabel2 = ObjAssemblyStatus.SerialNo
                Else
                    RHLabel2 = ObjAssemblyStatus.SerialNo + "(" + ObjAssemblyStatus.Position + ")"
                End If
                RHData2 = ObjAssemblyStatus.AssemblyType + " " + "Model"
                RHData3 = ObjAssemblyStatus.Model
                If ObjAssemblyStatus.Position = "" Then
                    SerialNoPostion = ObjAssemblyStatus.SerialNo
                Else
                    SerialNoPostion = ObjAssemblyStatus.SerialNo + "(" + ObjAssemblyStatus.Position + ")"
                End If
                searchstr7 = ObjMachine.Owner.ToString
                AssemblyID = ObjAssemblyStatus.AssemblyID
                'ReportStatusList.Add(New rptStatus(AssemblyID.ToString, ObjAssemblyStatus.AssemblyTypeID, , "Reg No.", ObjMachine.RegNo, ObjAssemblyStatus.AssemblyType + " " + "Model", ObjAssemblyStatus.Model, _
                '   "Serial No.", SerialNoPostion, "Due As of " & ObjAssemblyStatus.AssemblyType, , , , , , , , , , , , , LHLabel2, LHData2, RHLabel1, RHData1, RHLabel2, RHData2, RHData3))
            Next
        Next

        'mModificationTypeList = ModelMonitorModTypeList.GetModelMonitorModTypeList()

        'For i As Integer = 0 To mModificationTypeList.Count - 1
        'If mModificationTypeList(mModificationTypeList(i, "").ID).ModTypeID = 2 Or mModificationTypeList(mModificationTypeList(i, "").ID).ModTypeID = 1 Then
        mMachineList = MachineList.GetMachineListMonitoringStatusForHardTimeAndDirective(DateSerial(cmbYear.SelectedValue, cmbMonth.SelectedIndex + 1, 1).ToString, cmbMachine.SelectedValue.ToString, , , , , , , , , , True, True, , mAssemblyList(0).ID.ToString, , , , , , , , , , , ShowCofA, , , , True, , , , , , , False, , False, , True, , , , , , True, 6, , , True, SkipIsForInventoryAircarft:=True, ModTypeIDs:="1,2")
        For Each ObjMachine In mMachineList
            For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                For Each ObjAssemblyMonitorModStatus In ObjAssemblyStatus.AssemblyMonitorModStatusList
                    ATAChapter = ObjAssemblyMonitorModStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorModStatus.ATANomenclature
                    Description = ObjAssemblyMonitorModStatus.Description
                    Position = ObjAssemblyStatus.Position
                    MonitorTypeCode = ObjAssemblyMonitorModStatus.Code

                    MonitorType = ObjAssemblyMonitorModStatus.MonitorType '.Type
                    If (ObjAssemblyMonitorModStatus.IsApplicable = True) Then
                        If (ObjAssemblyMonitorModStatus.IsCompleted = True) And (ObjAssemblyMonitorModStatus.MonitorTypeID = 3 Or ObjAssemblyMonitorModStatus.MonitorTypeID = 1) Then
                            StatusType = "CLOSED"
                        Else
                            StatusType = "OPEN"
                        End If
                    Else
                        If (ObjAssemblyMonitorModStatus.ModelMonitorModTypeID = 28 Or ObjAssemblyMonitorModStatus.ModelMonitorModTypeID = 29) Then
                            StatusType = "OPEN"
                        ElseIf (ObjAssemblyMonitorModStatus.ModelMonitorModTypeID = 25 Or ObjAssemblyMonitorModStatus.ModelMonitorModTypeID = 26 Or ObjAssemblyMonitorModStatus.ModelMonitorModTypeID = 27) Then
                            StatusType = "CLOSED"
                        Else
                            StatusType = "N/A"
                        End If
                    End If
                    AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID
                    AssemblyModel = ObjAssemblyStatus.Model
                    AssemblySerialNo = ObjAssemblyStatus.SerialNo
                    Freq1 = ""
                    ElapsedTime = ""
                    RemainingTime = ""
                    DueAsof = ""
                    DoneOnValue = ""
                    EstimatedDate = ""
                    Code = ObjAssemblyMonitorModStatus.ModelMonitorModCode

                    If ObjAssemblyMonitorModStatus.IsApplicable = True And ObjAssemblyMonitorModStatus.IsCompleted = False Then
                        EstimatedDate = ObjAssemblyMonitorModStatus.EstimatedDateFormatted  'Added by Saylee on 10-June-2009
                    End If
                    IssueDate.Text = ObjAssemblyMonitorModStatus.IssueDateTextFormatted
                    IsApplicable = ObjAssemblyMonitorModStatus.IsApplicable
                    If ObjAssemblyMonitorModStatus.Number = "99-26-21" Or ObjAssemblyMonitorModStatus.Number = "99-08-23" Then
                        Dim a As Integer = 0
                    End If
                    For Each ObjAssemblyMonitorModStatusPeriod In ObjAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriodList
                        If Report = 0 Then 'Landscape
                            If ObjAssemblyMonitorModStatusPeriod.PeriodID = 1 Then

                                If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True) Or (ObjAssemblyMonitorModStatus.IsApplicable = False) Then
                                    RemainingTime = ""
                                    DueAsof = ""
                                    Freq1 = ""
                                    ElapsedTime = ""
                                Else
                                    Freq1 = ObjAssemblyMonitorModStatusPeriod.FrequencyValue
                                    ElapsedTime = ObjAssemblyMonitorModStatusPeriod.ElapsedValue
                                    RemainingTime = ObjAssemblyMonitorModStatusPeriod.RemainingValue
                                    DueAsof = DueAsof + ObjAssemblyMonitorModStatusPeriod.DueOnValue & vbCrLf
                                End If
                                DoneOnValue = DoneOnValue + ObjAssemblyMonitorModStatusPeriod.DoneOnValue & vbCrLf
                            End If
                            If ObjAssemblyMonitorModStatusPeriod.PeriodID = 2 Then
                                If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True) Or (ObjAssemblyMonitorModStatus.IsApplicable = False) Then
                                    RemainingTime = ""
                                    DueAsof = ""
                                    Freq1 = ""
                                    ElapsedTime = ""
                                Else
                                    Freq1 = ObjAssemblyMonitorModStatusPeriod.FrequencyValueFormatted
                                    ElapsedTime = ObjAssemblyMonitorModStatusPeriod.ElapsedValueFormatted
                                    RemainingTime = ObjAssemblyMonitorModStatusPeriod.RemainingValueFormatted
                                    DueAsof = DueAsof + ObjAssemblyMonitorModStatusPeriod.DueOnValueFormatted & vbCrLf
                                End If
                                'Comment removed by Saylee on 20-Apr-2010 to show value for PeriodID=2
                                If ObjAssemblyMonitorModStatus.DoneOn <> "" Then DoneOnValue = DoneOnValue + ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted & vbCrLf
                                '=====================================================================
                            End If
							'Added PeriodID=9,11,12,13,14,15 By Vikrant For ALL 21062012
							'If ObjAssemblyMonitorModStatusPeriod.PeriodID = 3 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 4 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 5 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 6 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 7 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 8 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 9 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 11 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 12 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 13 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 14 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 15 Then
							'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
							If ObjAssemblyMonitorModStatusPeriod.PeriodID >= 3 Then
								If Freq1 = "" Then

									If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True) Or (ObjAssemblyMonitorModStatus.IsApplicable = False) Then
										RemainingTime = ""
										DueAsof = ""
										Freq1 = ObjAssemblyMonitorModStatusPeriod.FrequencyValue
										ElapsedTime = ObjAssemblyMonitorModStatusPeriod.ElapsedValue
									Else
										Freq1 = ObjAssemblyMonitorModStatusPeriod.FrequencyValue
										ElapsedTime = ObjAssemblyMonitorModStatusPeriod.ElapsedValue
										RemainingTime = ObjAssemblyMonitorModStatusPeriod.RemainingValue
										DueAsof = DueAsof + ObjAssemblyMonitorModStatusPeriod.DueOnValue & vbCrLf
									End If
									DoneOnValue = DoneOnValue + ObjAssemblyMonitorModStatusPeriod.DoneOnValue & vbCrLf
								Else

									If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True) Or (ObjAssemblyMonitorModStatus.IsApplicable = False) Then
										RemainingTime = ""
										DueAsof = ""
										Freq1 = ""
										ElapsedTime = ""
									Else
										Freq1 = Freq1 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.FrequencyValue
										ElapsedTime = ElapsedTime & vbCrLf & ObjAssemblyMonitorModStatusPeriod.ElapsedValue
										RemainingTime = RemainingTime & vbCrLf & ObjAssemblyMonitorModStatusPeriod.RemainingValue
										DueAsof = DueAsof + ObjAssemblyMonitorModStatusPeriod.DueOnValue & vbCrLf
									End If
									DoneOnValue = DoneOnValue + ObjAssemblyMonitorModStatusPeriod.DoneOnValue & vbCrLf
								End If
							End If
						Else
                            If ObjAssemblyMonitorModStatusPeriod.PeriodID = 2 Then
                                If Freq1 = "" Then
                                    If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True) Or (ObjAssemblyMonitorModStatus.IsApplicable = False) Then
                                        RemainingTime = ""
                                        DueAsof = ""
                                        Freq1 = ""
                                        ElapsedTime = ""
                                    Else
                                        Freq1 = ObjAssemblyMonitorModStatusPeriod.FrequencyValueFormatted
                                        ElapsedTime = ObjAssemblyMonitorModStatusPeriod.ElapsedValueFormatted
                                        RemainingTime = ObjAssemblyMonitorModStatusPeriod.RemainingValueFormatted
                                        DueAsof = ObjAssemblyMonitorModStatusPeriod.DueOnValueFormatted
                                    End If
                                    'Comment removed by Saylee on 20-Apr-2010 to show value for PeriodID=2 also(Pramod's Requirement)
                                    If ObjAssemblyMonitorModStatus.DoneOn <> "" Then DoneOnValue = DoneOnValue & ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted & vbCrLf
                                    '=====================================================================
                                Else
                                    If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True) Or (ObjAssemblyMonitorModStatus.IsApplicable = False) Then
                                        RemainingTime = ""
                                        DueAsof = ""
                                        Freq1 = ""
                                        ElapsedTime = ""
                                    Else
                                        Freq1 = Freq1 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.FrequencyValueFormatted
                                        ElapsedTime = ElapsedTime & vbCrLf & ObjAssemblyMonitorModStatusPeriod.ElapsedValueFormatted
                                        RemainingTime = RemainingTime & vbCrLf & ObjAssemblyMonitorModStatusPeriod.RemainingValueFormatted
                                        DueAsof = DueAsof & vbCrLf & ObjAssemblyMonitorModStatusPeriod.DueOnValueFormatted
                                    End If
                                    If ObjAssemblyMonitorModStatus.DoneOn <> "" Then DoneOnValue = DoneOnValue + ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted & vbCrLf
                                End If
                            Else
                                If Freq1 = "" Then
                                    If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True) Or (ObjAssemblyMonitorModStatus.IsApplicable = False) Then
                                        RemainingTime = ""
                                        DueAsof = ""
                                        Freq1 = ""
                                        ElapsedTime = ""
                                    Else
                                        Freq1 = ObjAssemblyMonitorModStatusPeriod.FrequencyValue
                                        ElapsedTime = ObjAssemblyMonitorModStatusPeriod.ElapsedValue
                                        RemainingTime = ObjAssemblyMonitorModStatusPeriod.RemainingValue
                                        DueAsof = ObjAssemblyMonitorModStatusPeriod.DueOnValue
                                    End If
                                    DoneOnValue = DoneOnValue + ObjAssemblyMonitorModStatusPeriod.DoneOnValue & vbCrLf
                                Else
                                    If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True) Or (ObjAssemblyMonitorModStatus.IsApplicable = False) Then
                                        RemainingTime = ""
                                        DueAsof = ""
                                        Freq1 = ""
                                        ElapsedTime = ""
                                    Else
                                        Freq1 = Freq1 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.FrequencyValue
                                        ElapsedTime = ElapsedTime & vbCrLf & ObjAssemblyMonitorModStatusPeriod.ElapsedValue
                                        RemainingTime = RemainingTime & vbCrLf & ObjAssemblyMonitorModStatusPeriod.RemainingValue
                                        DueAsof = DueAsof & vbCrLf & ObjAssemblyMonitorModStatusPeriod.DueOnValue
                                    End If
                                    If ObjAssemblyMonitorModStatusPeriod.DoneOnValue = "" Then
                                        DoneOnValue = DoneOnValue & ObjAssemblyMonitorModStatusPeriod.DoneOnValue
                                    Else
                                        DoneOnValue = DoneOnValue + ObjAssemblyMonitorModStatusPeriod.DoneOnValue & vbCrLf
                                    End If
                                End If
                            End If
                        End If
                    Next

                    If ATAChapter = "" Then
                        ATAChapter = "----"
                    End If
                    If Description = "" Then
                        Description = "----"
                    End If
                    If Position = "" Then
                        Position = "----"
                    End If
                    If MonitorTypeCode = "" Then
                        MonitorTypeCode = "----"
                    End If
                    If MonitorType = "" Then
                        MonitorType = "----"
                    End If
                    If AssemblyModel = "" Then
                        AssemblyModel = "----"
                    End If
                    If AssemblySerialNo = "" Then
                        AssemblySerialNo = "----"
                    End If

                    AssemblyID = ObjAssemblyStatus.AssemblyID
                    Note = ObjAssemblyMonitorModStatus.Notes

                    If Note = "" Then
                        Note = "----"
                    End If

                    Number = ObjAssemblyMonitorModStatus.Number
                    If Number = "" Then
                        Number = "----"
                    End If
                    Reference = ObjAssemblyMonitorModStatus.Reference
                    If Reference = "" And AppSettings("ClientCode") <> "AVE" Then
                        Reference = "----"
                    End If

                    DoneOnDate = ObjAssemblyMonitorModStatus.DoneOn
                    DoneWONo = ObjAssemblyMonitorModStatus.DoneWONo

                    If DoneWONo = "" Then
                        DoneWONo = "----"
                    End If
                    Remark = ObjAssemblyMonitorModStatus.DoneRemark
                    If Remark = "" Then
                        Remark = "----"
                    End If
                    Applicability = ObjAssemblyMonitorModStatus.Applicability

                    If Applicability = "" Then
                        Applicability = "----"
                    End If

                    ComplianceRequirement = ObjAssemblyMonitorModStatus.ComplianceRequirement
                    If ComplianceRequirement = "" Then
                        ComplianceRequirement = "----"
                    End If
                    ModelMonitorModCode = ObjAssemblyMonitorModStatus.ModelMonitorModCode
                    If ModelMonitorModCode = "" Then
                        ModelMonitorModCode = "----"
                    End If
                    If DueAsof = "" Then
                        DueAsof = "----"
                    End If
                    If Freq1 = "" Then
                        Freq1 = "----"
                    End If
                    If ElapsedTime = "" Then
                        ElapsedTime = "----"
                    End If
                    If RemainingTime = "" Then
                        RemainingTime = "----"
                    End If
                    If DoneOnValue = "" Then
                        DoneOnValue = "----"
                    End If
                    'If EstimatedDate = "" Then
                    '    EstimatedDate = "----"
                    'End If
                    If StatusType = "" Then 'Added By VIkrant on 22-Jun-2012 For ALL22062012
                        StatusType = "----"
                    End If

                    Dim mReportMaintenanceDetail As ReportMaintenanceDetail
                    'TempChange
                    'If ObjAssemblyMonitorModStatusPeriod.RemainingValue.IndexOf("-") <> -1 And Freq1 <> "----" Then
                    If Freq1 <> "----" Then
                        mReportMaintenanceDetail = New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, , , Position, MonitorType, MonitorTypeCode, Note, Remark, Description, _
                                                                               , EstimatedDate, , , Freq1, Freq1, Freq1, ElapsedTime, ElapsedTime, ElapsedTime, RemainingTime, RemainingTime, RemainingTime, _
                                                                               DueAsof, DueAsof, DueAsof, AssemblyModel, , , , , , , , AssemblyTypeID, , , , , , , , , , , , , , , Number, Reference, DoneOnValue, DoneOnDate, DoneWONo, Applicability, ComplianceRequirement, , , , , , , , , , , Code, , , , IssueDate.Date.ToString("g"), IsApplicable, , , , , , , , , , StatusType)

                        mReportMaintenanceDetail.ModelMonitorModCode = ModelMonitorModCode

                        ReportMaintenanceDetailsForDirectives.Add(mReportMaintenanceDetail)
                    End If


                Next
                For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
                    For Each ObjCompMonitorModStatus In ObjCompStatus.CompMonitorModStatusList
                        ATAChapter = ObjCompMonitorModStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorModStatus.ATANomenclature
                        Description = ObjCompMonitorModStatus.Description
                        PartNo = ObjCompStatus.PartName
                        CompSerialNo = ObjCompStatus.CompSerialNo
                        Position = ObjCompStatus.Position
                        MonitorTypeCode = ObjCompMonitorModStatus.Code
                        MonitorType = ObjCompMonitorModStatus.MonitorType '.Type
                        If (ObjCompMonitorModStatus.IsApplicable = True) Then
                            If (ObjCompMonitorModStatus.IsCompleted = True) And (ObjCompMonitorModStatus.MonitorTypeID = 3 Or ObjCompMonitorModStatus.MonitorTypeID = 1) Then
                                StatusType = "CLOSED"
                            Else
                                StatusType = "OPEN"
                            End If
                        Else
                            StatusType = "CLOSED"
                        End If
                        AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID
                        AssemblyModel = ObjAssemblyStatus.Model
                        AssemblySerialNo = ObjAssemblyStatus.SerialNo
                        Freq1 = ""
                        ElapsedTime = ""
                        RemainingTime = ""
                        DueAsof = ""
                        EstimatedDate = ""
                        DoneOnValue = ""
                        Code = ObjCompMonitorModStatus.PartMonitorModCode

                        If ObjCompMonitorModStatus.IsApplicable = True And ObjCompMonitorModStatus.IsCompleted = False Then
                            EstimatedDate = ObjCompMonitorModStatus.EstimatedDateFormatted
                        End If

                        IssueDate.Text = ObjCompMonitorModStatus.IssueDate
                        IsApplicable = ObjCompMonitorModStatus.IsApplicable

                        For Each ObjCompMonitorModStatusPeriod In ObjCompMonitorModStatus.CompMonitorModStatusPeriodList
                            If Report = 0 Then  'Landscape
                                If ObjCompMonitorModStatusPeriod.PeriodID = 1 Then
                                    If (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted = True) Or (ObjCompMonitorModStatus.IsApplicable = False) Then
                                        RemainingTime = ""
                                        DueAsof = ""
                                        Freq1 = ""
                                        ElapsedTime = ""
                                    Else
                                        Freq1 = ObjCompMonitorModStatusPeriod.FrequencyValue
                                        ElapsedTime = ObjCompMonitorModStatusPeriod.ElapsedValue
                                        RemainingTime = ObjCompMonitorModStatusPeriod.RemainingValue
                                        DueAsof = DueAsof + ObjCompMonitorModStatusPeriod.DueOnValue & vbCrLf
                                    End If
                                    DoneOnValue = DoneOnValue + ObjCompMonitorModStatusPeriod.DoneOnValue & vbCrLf
                                End If
                                If ObjCompMonitorModStatusPeriod.PeriodID = 2 Then
                                    If (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted = True) Or (ObjCompMonitorModStatus.IsApplicable = False) Then
                                        RemainingTime = ""
                                        DueAsof = ""
                                        Freq1 = ""
                                        ElapsedTime = ""
                                    Else
                                        Freq1 = ObjCompMonitorModStatusPeriod.FrequencyValueFormatted
                                        ElapsedTime = ObjCompMonitorModStatusPeriod.ElapsedValueFormatted
                                        RemainingTime = ObjCompMonitorModStatusPeriod.RemainingValueFormatted
                                        DueAsof = DueAsof + ObjCompMonitorModStatusPeriod.DueOnValueFormatted & vbCrLf
                                    End If
                                End If
								'Added PeriodID=9,11,12,13,14,15 By Vikrant For ALL 21062012
								'If ObjCompMonitorModStatusPeriod.PeriodID = 3 Or ObjCompMonitorModStatusPeriod.PeriodID = 4 Or ObjCompMonitorModStatusPeriod.PeriodID = 5 Or ObjCompMonitorModStatusPeriod.PeriodID = 6 Or ObjCompMonitorModStatusPeriod.PeriodID = 7 Or ObjCompMonitorModStatusPeriod.PeriodID = 8 Or ObjCompMonitorModStatusPeriod.PeriodID = 9 Or ObjCompMonitorModStatusPeriod.PeriodID = 11 Or ObjCompMonitorModStatusPeriod.PeriodID = 12 Or ObjCompMonitorModStatusPeriod.PeriodID = 13 Or ObjCompMonitorModStatusPeriod.PeriodID = 14 Or ObjCompMonitorModStatusPeriod.PeriodID = 15 Then
								'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
								If ObjCompMonitorModStatusPeriod.PeriodID >= 3 Then
									If Freq1 = "" Then
										If (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted = True) Or (ObjCompMonitorModStatus.IsApplicable = False) Then
											RemainingTime = ""
											DueAsof = ""
											Freq1 = ""
											ElapsedTime = ""
										Else
											Freq1 = ObjCompMonitorModStatusPeriod.FrequencyValue
											ElapsedTime = ObjCompMonitorModStatusPeriod.ElapsedValue
											RemainingTime = ObjCompMonitorModStatusPeriod.RemainingValue
											DueAsof = DueAsof + ObjCompMonitorModStatusPeriod.DueOnValue & vbCrLf
										End If
										DoneOnValue = DoneOnValue + ObjCompMonitorModStatusPeriod.DoneOnValue & vbCrLf

									Else

										If (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted = True) Or (ObjCompMonitorModStatus.IsApplicable = False) Then
											RemainingTime = ""
											DueAsof = ""
											Freq1 = ""
											ElapsedTime = ""
										Else
											Freq1 = Freq1 & vbCrLf & ObjCompMonitorModStatusPeriod.FrequencyValue
											ElapsedTime = ElapsedTime & vbCrLf & ObjCompMonitorModStatusPeriod.ElapsedValue
											RemainingTime = RemainingTime & vbCrLf & ObjCompMonitorModStatusPeriod.RemainingValue
											DueAsof = DueAsof + ObjCompMonitorModStatusPeriod.DueOnValue & vbCrLf
										End If
										DoneOnValue = DoneOnValue + ObjCompMonitorModStatusPeriod.DoneOnValue & vbCrLf

									End If
								End If
							Else
                                If ObjCompMonitorModStatusPeriod.PeriodID = 2 Then
                                    If Freq1 = "" Then

                                        If (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted = True) Or (ObjCompMonitorModStatus.IsApplicable = False) Then
                                            RemainingTime = ""
                                            DueAsof = ""
                                            Freq1 = ""
                                            ElapsedTime = ""
                                        Else
                                            Freq1 = ObjCompMonitorModStatusPeriod.FrequencyValueFormatted
                                            ElapsedTime = ObjCompMonitorModStatusPeriod.ElapsedValueFormatted
                                            RemainingTime = ObjCompMonitorModStatusPeriod.RemainingValueFormatted
                                            DueAsof = ObjCompMonitorModStatusPeriod.DueOnValueFormatted
                                        End If
                                    Else
                                        If (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted = True) Or (ObjCompMonitorModStatus.IsApplicable = False) Then
                                            RemainingTime = ""
                                            DueAsof = ""
                                            Freq1 = ""
                                            ElapsedTime = ""
                                        Else
                                            Freq1 = Freq1 & vbCrLf & ObjCompMonitorModStatusPeriod.FrequencyValueFormatted
                                            ElapsedTime = ElapsedTime & vbCrLf & ObjCompMonitorModStatusPeriod.ElapsedValueFormatted
                                            RemainingTime = RemainingTime & vbCrLf & ObjCompMonitorModStatusPeriod.RemainingValueFormatted
                                            DueAsof = DueAsof & vbCrLf & ObjCompMonitorModStatusPeriod.DueOnValueFormatted
                                        End If
                                    End If
                                Else
                                    If Freq1 = "" Then
                                        If (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted = True) Or (ObjCompMonitorModStatus.IsApplicable = False) Then
                                            RemainingTime = ""
                                            DueAsof = ""
                                            Freq1 = ""
                                            ElapsedTime = ""
                                        Else
                                            Freq1 = ObjCompMonitorModStatusPeriod.FrequencyValue
                                            ElapsedTime = ObjCompMonitorModStatusPeriod.ElapsedValue
                                            RemainingTime = ObjCompMonitorModStatusPeriod.RemainingValue
                                            DueAsof = ObjCompMonitorModStatusPeriod.DueOnValue
                                        End If
                                        DoneOnValue = DoneOnValue + ObjCompMonitorModStatusPeriod.DoneOnValue & vbCrLf
                                    Else
                                        If (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted = True) Or (ObjCompMonitorModStatus.IsApplicable = False) Then
                                            RemainingTime = ""
                                            DueAsof = ""
                                            Freq1 = ""
                                            ElapsedTime = ""
                                        Else
                                            Freq1 = Freq1 & vbCrLf & ObjCompMonitorModStatusPeriod.FrequencyValue
                                            ElapsedTime = ElapsedTime & vbCrLf & ObjCompMonitorModStatusPeriod.ElapsedValue
                                            RemainingTime = RemainingTime & vbCrLf & ObjCompMonitorModStatusPeriod.RemainingValue
                                            DueAsof = DueAsof & vbCrLf & ObjCompMonitorModStatusPeriod.DueOnValue
                                        End If
                                        If ObjCompMonitorModStatusPeriod.DoneOnValue <> "" Then
                                            DoneOnValue = DoneOnValue & vbCrLf & ObjCompMonitorModStatusPeriod.DoneOnValue
                                        Else
                                            DoneOnValue = DoneOnValue & ObjCompMonitorModStatusPeriod.DoneOnValue
                                        End If
                                    End If
                                End If

                            End If
                        Next
                        AssemblyID = ObjAssemblyStatus.AssemblyID
                        Note = ObjCompMonitorModStatus.Notes
                        If Note = "" Then
                            Note = "----"
                        End If
                        Number = ObjCompMonitorModStatus.Number
                        If Number = "" Then
                            Number = "----"
                        End If
                        Reference = ObjCompMonitorModStatus.Reference
                        If Reference = "" And AppSettings("ClientCode") <> "AVE" Then
                            Reference = "----"
                        End If
                        DoneOnDate = ObjCompMonitorModStatus.DoneOnFormatted
                        DoneWONo = ObjCompMonitorModStatus.DoneOnWONo
                        If DoneWONo = "" Then
                            DoneWONo = "----"
                        End If
                        Remark = ObjCompMonitorModStatus.DoneRemark
                        If Remark = "" Then
                            Remark = "----"
                        End If
                        If ATAChapter = "" Then
                            ATAChapter = "----"
                        End If
                        If Description = "" Then
                            Description = "----"
                        End If
                        If PartNo = "" Then
                            PartNo = "----"
                        End If
                        If CompSerialNo = "" Then
                            CompSerialNo = "----"
                        End If
                        If Position = "" Then
                            Position = "----"
                        End If
                        If MonitorTypeCode = "" Then
                            MonitorTypeCode = ""
                        End If
                        If MonitorType = "" Then
                            MonitorType = "----"
                        End If
                        If AssemblyModel = "" Then
                            AssemblyModel = "----"
                        End If
                        If AssemblySerialNo = "" Then
                            AssemblySerialNo = "----"
                        End If
                        If Freq1 = "" Then
                            Freq1 = "----"
                        End If
                        If ElapsedTime = "" Then
                            ElapsedTime = "----"
                        End If
                        If RemainingTime = "" Then
                            RemainingTime = "----"
                        End If
                        If DueAsof = "" Then
                            DueAsof = "----"
                        End If
                        Applicability = ObjCompMonitorModStatus.Applicability
                        If Applicability = "" Then
                            Applicability = "----"
                        End If
                        If DoneOnValue = "" Then
                            DoneOnValue = "----"
                        End If
                        'If EstimatedDate = "" Then
                        '    EstimatedDate = "----"
                        'End If
                        ComplianceRequirement = ObjCompMonitorModStatus.ComplianceRequirement
                        If ComplianceRequirement = "" Then
                            ComplianceRequirement = "----"
                        End If

                        If StatusType = "" Then 'Added By VIkrant on 22-Jun-2012 For ALL22062012
                            StatusType = "----"
                        End If

                        Dim mReportMaintenanceDetail As ReportMaintenanceDetail

                        'TempChange
                        'If ObjCompMonitorModStatusPeriod.RemainingValue.IndexOf("-") <> -1 And Freq1 <> "----" Then
                        If Freq1 <> "----" Then
                            mReportMaintenanceDetail = New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, , , Position, MonitorType, MonitorTypeCode, Note, Remark, Description, _
                                                           , , , , Freq1, Freq1, Freq1, ElapsedTime, ElapsedTime, ElapsedTime, RemainingTime, RemainingTime, RemainingTime, _
                                                           DueAsof, DueAsof, DueAsof, AssemblyModel, , , , , , , , AssemblyTypeID, , , , , , , , , , , , , , , Number, Reference, DoneOnValue, DoneOnDate, DoneWONo, Applicability, ComplianceRequirement, , , , , , , , , , , Code, , , , IssueDate.Date.ToString("g"), IsApplicable, , , , , , , , , , StatusType)

                            mReportMaintenanceDetail.ModelMonitorModCode = ModelMonitorModCode

                            ReportMaintenanceDetailsForDirectives.Add(mReportMaintenanceDetail)
                        End If
                    Next
                Next
            Next
        Next
        'End If
        'Next
        If ReportMaintenanceDetailsForDirectives.Count > 0 Then
            ReportMaintenanceDetailsForDirectives.Sort("ModificationNumber", ComponentModel.ListSortDirection.Ascending)
        End If
        Return ReportMaintenanceDetailsForDirectives
    End Function

    Public Function ReportDetail() As ReportMaintenanceDetailList
        Dim ObjMachine As MachineInfo
        Dim ObjAssemblyStatus As AssemblyStatusInfo
        Dim ObjCompStatus As CompStatusInfo
        Dim ObjAssemblyMonitorInspStatus As AssemblyMonitorInspStatusInfo
        Dim ObjAssemblyMonitorInspStatusPeriod As AssemblyMonitorInspStatusPeriodInfo
        Dim ObjCompMonitorInspStatus As CompMonitorInspStatusInfo
        Dim ObjCompMonitorInspStatusPeriod As CompMonitorInspStatusPeriodInfo
        Dim mAssemblylist As AssemblyList
        mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbMachine.SelectedValue, DateSerial(cmbYear.SelectedValue, cmbMonth.SelectedIndex + 1, 1).ToString, , True)
        Session("mAssemblyList") = mAssemblylist

        Dim mAssemblyID As Guid = Guid.Empty
        For i As Integer = 0 To mAssemblylist.Count - 1
            If mAssemblylist(i).AssemblyTypeID = 1 Then
                mAssemblyID = mAssemblylist(i).ID
            End If
        Next
        mMachineList = MachineList.GetMachineListMonitoringStatus(DateSerial(cmbYear.SelectedValue, cmbMonth.SelectedIndex + 1, 1), cmbMachine.SelectedValue.ToString, , , , , , , , , , True, True, , , , , , , , , , , , , , , , True, , , , , , , , False, , False, , True, , , , 6, , , True, SkipIsForInventoryAircarft:=True)         '6 CH* Check Recurring
        For Each ObjMachine In mMachineList
            For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                For Each ObjAssemblyMonitorInspStatus In ObjAssemblyStatus.AssemblyMonitorInspStatusList
                    If (ObjAssemblyMonitorInspStatus.IsApplicable = True) Then

                        Description = ObjAssemblyMonitorInspStatus.Description
                        Freq1 = ""
                        Freq2 = ""
                        Freq3 = ""
                        ElapsedTime = ""
                        ElapsedTime1 = ""
                        ElapsedTime2 = ""
                        RemainingTime = ""
                        RemainingTime1 = ""
                        RemainingTime2 = ""
                        DoneAt2 = ""
                        TimeSinceNew = ""
                        For Each ObjAssemblyMonitorInspStatusPeriod In ObjAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriodList
                            If ObjAssemblyMonitorInspStatusPeriod.PeriodID = 2 Then        'StartDate
                                If Freq3 = "" Then
                                    Freq3 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValueFormatted
                                    If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False And ObjAssemblyMonitorInspStatus.IsCompleted = True) Then
                                        ElapsedTime2 = ""
                                        RemainingTime2 = ""
                                        DoneAt2 = ""
                                    Else
                                        ElapsedTime2 = ObjAssemblyMonitorInspStatusPeriod.ElapsedValueFormatted
                                        RemainingTime2 = ObjAssemblyMonitorInspStatusPeriod.RemainingValueFormatted
                                        DoneAt2 = ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted
                                    End If
                                Else
                                    Freq3 = Freq3 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.FrequencyValueFormatted
                                    If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False And ObjAssemblyMonitorInspStatus.IsCompleted = True) Then
                                        ElapsedTime2 = ""
                                        RemainingTime2 = ""
                                        DoneAt2 = ""
                                    Else
                                        ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.ElapsedValueFormatted
                                        RemainingTime2 = RemainingTime2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.RemainingValue
                                        DoneAt2 = DoneAt2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted
                                    End If
                                End If
                            Else                                                           'PeriodID <> 2      
                                If Freq3 = "" Then
                                    Freq3 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValue
                                    If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False And ObjAssemblyMonitorInspStatus.IsCompleted = True) Then
                                        ElapsedTime2 = ""
                                        RemainingTime2 = ""
                                        DoneAt2 = ""
                                        TimeSinceNew = ""
                                    Else
                                        ElapsedTime2 = ObjAssemblyMonitorInspStatusPeriod.AllElapsedValue
                                        RemainingTime2 = ObjAssemblyMonitorInspStatusPeriod.RemainingValue
                                        DoneAt2 = ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
                                        TimeSinceNew = ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue
                                    End If
                                Else                                                       'Freq3 <> ""
                                    Freq3 = Freq3 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.FrequencyValue
                                    If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False And ObjAssemblyMonitorInspStatus.IsCompleted = True) Then
                                        ElapsedTime2 = ""
                                        RemainingTime2 = ""
                                        DoneAt2 = ""
                                        TimeSinceNew = ""
                                    Else
                                        ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.AllElapsedValue
                                        RemainingTime2 = RemainingTime2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.RemainingValue
                                        DoneAt2 = DoneAt2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
                                        TimeSinceNew = TimeSinceNew & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue
                                    End If
                                End If
                            End If
                            'End If
                        Next
                        ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(mAssemblyID, , , , , , , , , , , , , Description, _
, , , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, _
, , , , , , , , , DoneAt2, , 1, , , , , , , , , , , , , , , , , , , , , , , , , , , , , _
, , , , , , , , , , , , , , , , , TimeSinceNew))
                    End If
                Next
            Next
            'Next
            For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
                For Each ObjCompMonitorInspStatus In ObjCompStatus.CompMonitorInspStatusList
                    If (ObjCompMonitorInspStatus.IsApplicable = True) Then
                        Description = ObjCompMonitorInspStatus.Description
                        Freq1 = ""
                        Freq2 = ""
                        Freq3 = ""
                        ElapsedTime = ""
                        ElapsedTime1 = ""
                        ElapsedTime2 = ""
                        RemainingTime = ""
                        RemainingTime1 = ""
                        RemainingTime2 = ""
                        DoneAt2 = ""
                        TimeSinceNew = ""
                        For Each ObjCompMonitorInspStatusPeriod In ObjCompMonitorInspStatus.CompMonitorInspStatusPeriodList
                            If ObjCompMonitorInspStatusPeriod.PeriodID = 1 Then
                                Freq1 = ObjCompMonitorInspStatusPeriod.FrequencyValue
                                If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Or (ObjCompMonitorInspStatus.IsApplicable = False And ObjCompMonitorInspStatus.IsCompleted = True) Then
                                    ElapsedTime = ""
                                    RemainingTime = ""
                                    TimeSinceNew = ""
                                Else
                                    ElapsedTime = ObjCompMonitorInspStatusPeriod.AllElapsedValue
                                    RemainingTime = ObjCompMonitorInspStatusPeriod.RemainingValue
                                    TimeSinceNew = ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted
                                End If
                            End If
                            If ObjCompMonitorInspStatusPeriod.PeriodID = 2 Then
                                Freq2 = ObjCompMonitorInspStatusPeriod.FrequencyValueFormatted
                                If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Or (ObjCompMonitorInspStatus.IsApplicable = False And ObjCompMonitorInspStatus.IsCompleted = True) Then
                                    ElapsedTime1 = ""
                                    RemainingTime1 = ""
                                Else
                                    ElapsedTime1 = ObjCompMonitorInspStatusPeriod.AllElapsedValueFormatted
                                    RemainingTime1 = ObjCompMonitorInspStatusPeriod.RemainingValueFormatted
                                End If
                                If ObjCompMonitorInspStatusPeriod.PeriodID = 3 Or ObjCompMonitorInspStatusPeriod.PeriodID = 4 Or ObjCompMonitorInspStatusPeriod.PeriodID = 5 Or ObjCompMonitorInspStatusPeriod.PeriodID = 6 Or ObjCompMonitorInspStatusPeriod.PeriodID = 7 Or ObjCompMonitorInspStatusPeriod.PeriodID = 8 Or ObjCompMonitorInspStatusPeriod.PeriodID = 12 Or ObjCompMonitorInspStatusPeriod.PeriodID = 13 Or ObjCompMonitorInspStatusPeriod.PeriodID = 14 Then
                                    If Freq3 = "" Then
                                        Freq3 = ObjCompMonitorInspStatusPeriod.FrequencyValue
                                        If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Or (ObjCompMonitorInspStatus.IsApplicable = False And ObjCompMonitorInspStatus.IsCompleted = True) Then
                                            ElapsedTime2 = ""
                                            RemainingTime2 = ""
                                            DoneAt2 = ""
                                            TimeSinceNew = ""
                                        Else
                                            ElapsedTime2 = ObjCompMonitorInspStatusPeriod.AllElapsedValue
                                            RemainingTime2 = ObjCompMonitorInspStatusPeriod.RemainingValue
                                            DoneAt2 = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                            TimeSinceNew = ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted
                                        End If
                                    Else
                                        Freq3 = Freq3 & vbCrLf & ObjCompMonitorInspStatusPeriod.FrequencyValue
                                        If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Or (ObjCompMonitorInspStatus.IsApplicable = False And ObjCompMonitorInspStatus.IsCompleted = True) Then
                                            ElapsedTime2 = ""
                                            RemainingTime2 = ""
                                            DoneAt2 = ""
                                            TimeSinceNew = ""
                                        Else
                                            ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjCompMonitorInspStatusPeriod.AllElapsedValue
                                            RemainingTime2 = RemainingTime2 & vbCrLf & ObjCompMonitorInspStatusPeriod.RemainingValue
                                            DoneAt2 = DoneAt2 & vbCrLf & ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                            TimeSinceNew = TimeSinceNew & vbCrLf & ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted
                                        End If
                                    End If
                                End If
                            Else
                                If ObjCompMonitorInspStatusPeriod.PeriodID = 2 Then        'StartDate
                                    If Freq3 = "" Then
                                        Freq3 = ObjCompMonitorInspStatusPeriod.FrequencyValueFormatted
                                        If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Or (ObjCompMonitorInspStatus.IsApplicable = False And ObjCompMonitorInspStatus.IsCompleted = True) Then
                                            ElapsedTime2 = ""
                                            RemainingTime2 = ""
                                            DoneAt2 = ""
                                        Else
                                            ElapsedTime2 = ObjCompMonitorInspStatusPeriod.AllElapsedValueFormatted
                                            RemainingTime2 = ObjCompMonitorInspStatusPeriod.RemainingValueFormatted
                                            DoneAt2 = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                        End If
                                    Else                                                   'Freq3 <> ""
                                        Freq3 = Freq3 & vbCrLf & ObjCompMonitorInspStatusPeriod.FrequencyValueFormatted
                                        If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Or (ObjCompMonitorInspStatus.IsApplicable = False And ObjCompMonitorInspStatus.IsCompleted = True) Then
                                            ElapsedTime2 = ""
                                            RemainingTime2 = ""
                                            DoneAt2 = ""
                                        Else
                                            ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjCompMonitorInspStatusPeriod.AllElapsedValueFormatted
                                            RemainingTime2 = RemainingTime2 & vbCrLf & ObjCompMonitorInspStatusPeriod.RemainingValueFormatted
                                            DoneAt2 = DoneAt2 & vbCrLf & ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                        End If
                                    End If
                                Else                                                       'For PeriodID <> 2
                                    If Freq3 = "" Then
                                        Freq3 = ObjCompMonitorInspStatusPeriod.FrequencyValue
                                        If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Or (ObjCompMonitorInspStatus.IsApplicable = False And ObjCompMonitorInspStatus.IsCompleted = True) Then
                                            ElapsedTime2 = ""
                                            RemainingTime2 = ""
                                            DoneAt2 = ""
                                            TimeSinceNew = ""
                                        Else
                                            ElapsedTime2 = ObjCompMonitorInspStatusPeriod.AllElapsedValue
                                            RemainingTime2 = ObjCompMonitorInspStatusPeriod.RemainingValue
                                            If ObjCompMonitorInspStatusPeriod.PeriodID = 9 Then
                                                DoneAt2 = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                TimeSinceNew = ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted
                                            Else
                                                DoneAt2 = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                TimeSinceNew = ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted
                                            End If
                                        End If
                                    Else                                                   'Freq3 <> ""   
                                        Freq3 = Freq3 & vbCrLf & ObjCompMonitorInspStatusPeriod.FrequencyValue
                                        If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Or (ObjCompMonitorInspStatus.IsApplicable = False And ObjCompMonitorInspStatus.IsCompleted = True) Then
                                            ElapsedTime2 = ""
                                            RemainingTime2 = ""
                                            DoneAt2 = ""
                                            TimeSinceNew = ""
                                        Else
                                            ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjCompMonitorInspStatusPeriod.AllElapsedValue
                                            RemainingTime2 = RemainingTime2 & vbCrLf & ObjCompMonitorInspStatusPeriod.RemainingValue
                                            If ObjCompMonitorInspStatusPeriod.PeriodID = 9 Then
                                                DoneAt2 = DoneAt2 & vbCrLf & ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                            Else
                                                DoneAt2 = DoneAt2 & vbCrLf & ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                TimeSinceNew = TimeSinceNew & vbCrLf & ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        Next

                        ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(mAssemblyID, , , , , , , , , , , , , Description, _
     , , , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, _
         , , , , , , , , , DoneAt2, , 1, , , , , , , , , , , , , , , , , , , , , , , , , , , , , _
         , , , , , , , , , , , , , , , , , TimeSinceNew))
                    End If
                Next
            Next
        Next
        Return ReportMaintenanceDetails
    End Function
    Private Sub SetValues()
        lblyear1.Text = "Month and Year : " & IIf((cmbYear.SelectedIndex >= 0 And cmbMonth.SelectedIndex >= 0), cmbMonth.SelectedItem.Text + " , " + cmbYear.SelectedItem.Text, "")
        lblModel1.Text = "Aircraft : " & IIf(cmbMachine.SelectedIndex > 0, cmbMachine.SelectedItem.Text, "")
        EventLogDetail = lblyear1.Text + ";" + lblModel1.Text
    End Sub
    Private Sub SetReport()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim mCompanyDetail As CompanyDetail
        Dim ReportName As String = String.Empty
        Dim ds As New dsACSpecs

        ReportName = "Aircraft Specification Report"
        SetValues()

        Dim myReport = New crACSpecs
        ReportDetail()
        ReportDetailForADs()
        mACSpecsRadioEquipmentList = ACSpecsRadioEquipmentList.GetRadioEquipmentList(cmbMachine.SelectedValue.ToString, cmbMonth.SelectedIndex + 1, CInt(cmbYear.SelectedItem.ToString))
        mACSpecsAircraftIdentification = ACSpecsAircraftIdentification.GetACSpecsAircraftIdentification(cmbMonth.SelectedIndex + 1, CInt(cmbYear.SelectedItem.ToString), cmbMachine.SelectedValue.ToString)
        mACSpecsCertificateOfRegistration = ACSpecsCertificateOfRegistration.GetACSpecsCertificateOfRegistration(cmbMonth.SelectedIndex + 1, CInt(cmbYear.SelectedItem.ToString), cmbMachine.SelectedValue.ToString)
        mACSpecsAirframeStatus = ACSpecsAirframeStatus.GetAirframeStatus(cmbMachine.SelectedValue.ToString, cmbMonth.SelectedIndex + 1, CInt(cmbYear.SelectedItem.ToString))
        mACSpecsAPU = ACSpecsAPU.GetACSpecsAPU(cmbMonth.SelectedIndex + 1, CInt(cmbYear.SelectedItem.ToString), cmbMachine.SelectedValue.ToString)
        mACSpecsInstalledEngines = ACSpecsInstalledEngines.GetInstalledEnginesInfo(cmbMachine.SelectedValue.ToString, cmbMonth.SelectedIndex + 1, CInt(cmbYear.SelectedItem.ToString))
        mACSpecsPrincipalOperatingWeights = ACSpecsPrincipalOperatingWeights.GetACSpecsPrincipalOperatingWeights(cmbMonth.SelectedIndex + 1, CInt(cmbYear.SelectedItem.ToString), cmbMachine.SelectedValue.ToString)
        mtmpComplyAssemblyMonitorServiceStatusList = tmpComplyAssemblyMonitorServiceStatusList.GetDueMonitorServiceList(DateSerial(cmbYear.SelectedValue, cmbMonth.SelectedIndex + 1, 1).ToString, cmbMachine.SelectedValue.ToString, "", "", 32, , , 1, , , , , SortBy:="Model")
        mACSpecsAircraftFeatures = ACSpecsAircraftFeatures.GetAircraftFeaturesList(cmbMachine.SelectedValue.ToString, cmbMonth.SelectedIndex + 1, CInt(cmbYear.SelectedItem.ToString))
        mACSpecsMachineStructuralInsp = ACSpecsMachineStructuralInsp.GetACSpecsMachineStructuralInsp(cmbMonth.SelectedIndex + 1, CInt(cmbYear.SelectedItem.ToString), cmbMachine.SelectedValue.ToString)
        mACSpecsMachineMaintPolicies = ACSpecsMachineMaintPolicies.GetACSpecsMachineMaintPolicies(cmbMonth.SelectedIndex + 1, CInt(cmbYear.SelectedItem.ToString), cmbMachine.SelectedValue.ToString)


        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                 mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                 mCompanyDetail.WebSite, "", cmbMonth.SelectedItem.Text + " 1", cmbYear.SelectedItem.Text, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", cmbMachine.SelectedItem.ToString, cmbMonth.SelectedItem.Text + " , " + cmbYear.SelectedItem.Text)

        da.Fill(ds, mACSpecsRadioEquipmentList)
        da.Fill(ds, mACSpecsAircraftIdentification)
        da.Fill(ds, mACSpecsAirframeStatus)
        da.Fill(ds, mACSpecsCertificateOfRegistration)
        da.Fill(ds, ReportMaintenanceDetails)
        da.Fill(ds, mACSpecsInstalledEngines)
        da.Fill(ds, mtmpComplyAssemblyMonitorServiceStatusList)
        da.Fill(ds, mACSpecsAPU)
        da.Fill(ds, mACSpecsPrincipalOperatingWeights)
        da.Fill(ds, mACSpecsAircraftFeatures)
        da.Fill(ds, ReportMaintenanceDetailsForDirectives)
        da.Fill(ds, mACSpecsMachineStructuralInsp)
        da.Fill(ds, mACSpecsMachineMaintPolicies)
        da.Fill(ds, Report)
        myReport.SetDataSource(ds)
        With myReport
            If mACSpecsRadioEquipmentList.Count = 0 Then
                .Section8.SectionFormat.EnableSuppress = True
            End If
            If mACSpecsAircraftIdentification.Count = 0 Then
                .Section18.SectionFormat.EnableSuppress = True
            End If
            If mACSpecsAirframeStatus.Count = 0 Then
                .Section7.SectionFormat.EnableSuppress = True
            End If
            If mACSpecsCertificateOfRegistration.Count = 0 Then
                .Section16.SectionFormat.EnableSuppress = True
            End If
            If ReportMaintenanceDetails.Count = 0 Then
                .Section9.SectionFormat.EnableSuppress = True
            End If
            If mACSpecsInstalledEngines.Count = 0 Then
                .Section11.SectionFormat.EnableSuppress = True
            End If
            If mtmpComplyAssemblyMonitorServiceStatusList.Count = 0 Then
                .Section15.SectionFormat.EnableSuppress = True
            End If
            If mACSpecsAPU.Count = 0 Then
                .Section12.SectionFormat.EnableSuppress = True
            End If
            If mACSpecsPrincipalOperatingWeights.Count = 0 Then
                .Section13.SectionFormat.EnableSuppress = True
            End If
            If mACSpecsAircraftFeatures.Count = 0 Then
                .Section10.SectionFormat.EnableSuppress = True
            End If
            If ReportMaintenanceDetailsForDirectives.Count = 0 Then
                .Section20.SectionFormat.EnableSuppress = True
            End If
            If mACSpecsMachineStructuralInsp.Count = 0 Then
                .Section21.SectionFormat.EnableSuppress = True
            End If
            If mACSpecsMachineMaintPolicies.Count = 0 Then
                .Section17.SectionFormat.EnableSuppress = True
            End If
        End With
        Session("CrystalReport") = myReport
        RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1269)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "AircraftSpecificationReport", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not Page.IsPostBack Then
            SetCombo()
            DataFieldBinding()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Response.Redirect("Dashboard.aspx")
    End Sub
#End Region
End Class