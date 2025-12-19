'Created By :   Saylee
'Date       :   11-Sep-2017

Imports System.Collections.Generic
Imports System.Text

Public Class wfrptLastDoneNextDueReport
    Inherits Page

#Region "Enumeration"

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

#Region "Variable Declaration"

    Dim ReportMaintenanceDetails As New ReportMaintenanceDetailList
    Dim mReportMaintenanceDetail As New ReportMaintenanceDetail
    Dim mAssemblyList As AssemblyList
    Dim mServiceTypeList As PartMonitorServiceTypeList
    Dim mInspectionTypeList As ModelMonitorInspTypeList
    Dim ReportStatusList As New rptStatusList
    Dim mMachineList As MachineList
    Dim mTypeListForCofA As TypeListForCofA
    Dim SofAIndex As Integer
    Dim InspIndex As Integer
    Dim SerIndex As Integer
    Dim AircraftIndex As Integer
    Dim TypeCount As Boolean = False
    Dim Check As Boolean = False
    Dim ReportLabel As String
    Dim Aircraft As String
    Dim Assembly1 As String
    Dim ReportType As String
    Dim ServiceType As String
    Dim InspectionType As String
    Dim AOdate As String
    Dim AOnDate As String
    Dim ReportStatus As Integer = 1
    Dim Report As ReportData
    Dim ShowCofA As Boolean = False
    Dim AsonDate As String = ""
    Dim IsSerSelect As Boolean = False
    Dim IsInsSelect As Boolean = False
    Dim ServiceTypeID(50) As Integer
    Dim InspectionTypeID(50) As Integer
    Dim x As Integer
    Dim Periodcount As Integer
    Dim Count As Integer
    Dim AssemblyName As String
    Dim MachineName As String
    Dim Machine1 As String
    Dim AssemblyID As Guid
    Private ATAChapter As String
    Private RegNo As String
    Private AssemblyType As String
    Private Model As String
    Private AssemblySerialNo, SerialNoPostion As String
    Private PartNo As String
    Private CompSerialNo As String
    Private Position As String
    Private MonitorTypeCode As String
    Private MonitorType As String
    Private Note As String
    Private Description As String
    Private EstimatedDate As String
    Private Freq1 As String
    Private Freq2 As String
    Private Freq3 As String
    Private ElapsedTime As String
    Private ElapsedTime1 As String
    Private ElapsedTime2 As String
    Private RemainingTime As String
    Private RemainingTime1 As String
    Private RemainingTime2 As String
    Private DueAsof As String
    Private DueAsof1 As String
    Private DueAsof2, SinceNew2, DoneAt2 As String
    Private AssemblyModel As String
    Private Number As String
    Private Reference As String
    Private DoneOnValue As String
    Private DoneOnDate As String
    Private DoneWONo As String
    Private Remark As String
    Private Extension As String
    Private Extension1 As String
    Private Extension2 As String
    Private ExtensionDate As String
    Private ApprovalRemark, StartDateLabel, StartDateData As String
    Dim AssemblyDueAsof2 As String

    Public mATAList As ATAList          'Added by Saylee on 20-Apr-2010
    Private ATACode As Integer          'Added by Saylee on 20-Apr-2010
    Private ATANomenclature As String   'Added by Saylee on 20-Apr-2010
    Private TimeSinceNew As String      'Added by Saylee on 24-Feb-2010
    Dim searchstr7 As String = ""       'Added by Saylee on 8-Aug-2011
    Dim SearchStr4 As String = ""       'Added By Prashant 27-May-2013 ALL27052013
    'Added By Utkarsh ON 12-Sep-2013 FOR BA11092013
    Dim mPerDayLimits As PerDayLimits
    Dim mByPerDayLimit As Boolean = False
    Dim mIsAverageRequired As Boolean = False
    Dim Code As String = String.Empty
    'End
    Dim mCofASearchingCriteria As String = String.Empty
    Dim EventLogID As Guid 'Added by Prashant on 04-Dec-2013
    Dim PeriodLimt As String = String.Empty  'Added by Prashant on 04-Dec-2013
    Dim StatusMasterID As Guid 'vikrant
    Dim DoneONValueForNoPeriod As String = String.Empty
    Dim TSOValueForNoPeriod As String = String.Empty
    Dim DoneONValueForAssembly As String = String.Empty
    Private IsExcel As Boolean = False
    Private SourceDoc As String = ""
    Dim mtmpMachineList As tmpMachineList
    Dim DiffCompInstDoneOnValue As String = String.Empty
    Dim RecordOwnID As String = String.Empty
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
    'Added By Vikrant On 22-Dec-2020 For Star Air
    Dim nWONumber As String = ""
    Dim mnWOListForDueJobs As nWOListForDueJobs
    'End
    Dim TaskNo As String = ""
    Dim AMPNoStr As String = ""
    Dim mLastAMPRef As LastMPDAMPRef

#End Region

#Region "Helper Methods"

    Private Sub GetSession()

        mMachineList = CType(Session("mMachineList"), MachineList)
        mAssemblyList = CType(Session("mAssemblyList"), AssemblyList)
        mServiceTypeList = CType(Session("mServiceTypeList"), PartMonitorServiceTypeList)
        mInspectionTypeList = CType(Session("mInspectionTypeList"), ModelMonitorInspTypeList)
        mTypeListForCofA = CType(Session("mTypeListForCofA"), TypeListForCofA)
        AOnDate = Session("AOnDate")
        TypeCount = Session("TypeCount")
        Check = Session("Check")
        AircraftIndex = Session("AircraftIndex")
        SerIndex = Session("SerIndex")
        SofAIndex = Session("SofAIndex")
        ReportStatus = Session("ReportStatus")
        ShowCofA = Session("ShowCofA")
        mATAList = CType(Session("mATAList"), ATAList)  'Added by Saylee on 20-Apr-2010
        mPerDayLimits = CType(Session("mPerDayLimits"), PerDayLimits) 'Added By Utkarsh ON 12-Sep-2013 FOR BA11092013
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 

    End Sub

    Private Sub SetSession()

        Session("mMachineList") = mMachineList
        Session("mAssemblyList") = mAssemblyList
        Session("mServiceTypeList") = mServiceTypeList
        Session("mInspectionTypeList") = mInspectionTypeList
        Session("mTypeListForCofA") = mTypeListForCofA
        Session("AOnDate") = AOnDate
        Session("TypeCount") = TypeCount
        Session("Check") = Check
        Session("AircraftIndex") = AircraftIndex
        Session("SerIndex") = SerIndex
        Session("InspIndex") = InspIndex
        Session("SofAIndex") = SofAIndex
        Session("ReportStatus") = ReportStatus
        Session("ShowCofA") = ShowCofA
        Session("mATAList") = mATAList 'Added by Saylee on 20-Apr-2010
        Session("mPerDayLimits") = mPerDayLimits 'Added By Utkarsh ON 12-Sep-2013 FOR BA11092013

    End Sub

    Public Sub ControlVisibility()

        ListServiceType.Enabled = False
        ListInspectionType.Enabled = False
        ListServiceType.Visible = True
        ListInspectionType.Visible = True
        lbllinkedAct.Visible = IIf(cmbFormat.SelectedValue = "2", True, False) 'Added By Vikrant On 08-Dec-2020 For ALL08122020-1

    End Sub

    Private Sub ClearAll()

        If Session("MiddleFrame") <> "wfrptLastDoneNextDueReport.aspx?" Then
            Session.Remove("mMachineList")
            Session.Remove("mAssemblyList")
            Session.Remove("mServiceTypeList")
            Session.Remove("mInspectionTypeList")
            Session.Remove("AOnDate")
            Session.Remove("TypeCount")
            Session.Remove("Check")
            Session.Remove("AircraftIndex")
            Session.Remove("SerIndex")
            Session.Remove("InspIndex")
            Session.Remove("SofAIndex")
            Session.Remove("Report")
            Session.Remove("mATAList")  'Added by Saylee on 20-Apr-2010
        End If

    End Sub

    Private Overloads Sub SetFocus(control As WebControl)

        If control.Enabled = False Or control.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + control.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)

    End Sub

    Private Sub Display()

        lblAircraft1.Visible = True
        lblDateRange.Visible = True
        lblAssembly1.Visible = True
        lblATAChapter1.Visible = True

    End Sub

    Private Sub SetValues()

        If cmbAircraft.SelectedItem.Text = "<SELECT>" Then
            Aircraft = ""
            lblAircraft1.Text = "Aircraft : All"
        Else
            If cmbAssembly.SelectedItem.Text = "(All)" Or cmbAssembly.SelectedItem.Text = "" Then
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

            MachineName = cmbAircraft.SelectedValue.ToString
            Aircraft = cmbAircraft.SelectedItem.Text
            lblAircraft1.Text = "Aircraft : " & Aircraft
        End If

        If (txtFromDate.Text.Trim = String.Empty) Then
            AsonDate = ""
        Else
            AsonDate = txtFromDate.Text.ToString
            lblDateRange.Text = "AsonDate : " & New SmartDate(txtFromDate.Text.ToString).FormattedText
        End If

        If cmbType.Items.Count <> 0 Then
            ' If so, loop through all checked items and print results.
            Dim x As Integer
            For x = 0 To cmbType.Items.Count - 1
                If cmbType.Items(x).Selected = True And (cmbType.Items(x).Text = "Service" Or cmbType.Items(x).Text = "MPD") Then
                    IsSerSelect = True
                    For K As Integer = 0 To ListServiceType.Items.Count - 1
                        If ListServiceType.Items.Item(K).Selected Then
                            ServiceTypeID(K) = ListServiceType.Items.Item(K).Value
                            ServiceType = ServiceType + ", " + ListServiceType.Items.Item(K).Text
                        End If
                    Next
                End If

                If cmbType.Items(x).Selected = True And cmbType.Items(x).Text = "Inspection" Then
                    IsInsSelect = True

                    For K As Integer = 0 To ListInspectionType.Items.Count - 1
                        If ListInspectionType.Items.Item(K).Selected Then
                            InspectionTypeID(K) = ListInspectionType.Items.Item(K).Value
                            InspectionType = InspectionType + ", " + ListInspectionType.Items.Item(K).Text
                        End If
                    Next
                End If

                If cmbType.Items.Item(x).ToString = "All" Then
                    IsSerSelect = True
                    IsInsSelect = True
                    ServiceTypeID(0) = 0
                    InspectionTypeID(0) = 0
                End If
            Next x
        End If

        'Added by Saylee on 20-Apr-2010
        If cmbATAChapter.SelectedItem.Text = "(All)" Then
            ATACode = 0
            ATANomenclature = ""
            lblATAChapter1.Text = "ATA Chapter  : All"
        Else
            ATACode = mATAList(cmbATAChapter.SelectedIndex).ATACode
            ATANomenclature = mATAList(cmbATAChapter.SelectedIndex).ATANomenclature
            lblATAChapter1.Text = "ATA Chapter : " & mATAList(cmbATAChapter.SelectedIndex).ATAChapter
        End If
        '****************************************
        'End
        mCofASearchingCriteria = lblDateRange.Text + ", " + lblAircraft1.Text + ", " + lblAssembly1.Text + ", " + lblATAChapter1.Text + ServiceType + InspectionType

    End Sub
    Public Sub SetTypeCombo()

        mServiceTypeList = PartMonitorServiceTypeList.GetPartMonitorServiceTypeList()
        ListServiceType.DataSource = mServiceTypeList
        Session("mServiceTypeList") = mServiceTypeList
        'If AppSettings("ClientCode") = "RAL" Or AppSettings("ClientCode") = "Indamer" Then  'Added By Prashant 20-Aug-2012/ "Indamer" added by Saylee on 30-04-2013 for Indamer30042013-1 
        '    mInspectionTypeList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList(ModelMonitorInspTypeList.serach.AllInspections)
        'Else
        '    mInspectionTypeList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList(ModelMonitorInspTypeList.serach.ExludingRoutineInspections)
        'End If
        mInspectionTypeList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList(ModelMonitorInspTypeList.serach.AllInspections)
        ListInspectionType.DataSource = mInspectionTypeList
        Session("mInspectionTypeList") = mInspectionTypeList
        upnType.Update()
        DataBind()

    End Sub

    Private Sub FillTypeCombo()
        Dim j As Integer
        For j = 0 To cmbType.Items.Count - 1
            cmbType.Items(j).Selected = True
        Next
        For j = 0 To cmbType.Items.Count - 1
            'ListServiceType Enabled
            If cmbType.Items(j).Selected = True And (cmbType.Items(j).Text = "Service" Or cmbType.Items(j).Text = "MPD") Then
                ListServiceType.Enabled = cmbType.Items.Item(cmbType.SelectedIndex).Selected
                For i As Integer = 0 To ListServiceType.Items.Count - 1
                    ListServiceType.Items.Item(i).Selected = ListServiceType.Enabled
                Next
                hdnService.Value = cmbType.Items.Item(cmbType.SelectedIndex).Selected
                upnlServiceType.Update()
            End If

            'ListInspectionType Enabled
            If cmbType.Items(j).Selected = True And cmbType.Items(j).Text = "Inspection" Then
                ListInspectionType.Enabled = cmbType.Items.Item(cmbType.SelectedIndex).Selected
                For i As Integer = 0 To ListInspectionType.Items.Count - 1
                    ListInspectionType.Items.Item(i).Selected = ListInspectionType.Enabled
                Next
                hdnInspection.Value = cmbType.Items.Item(cmbType.SelectedIndex).Selected
                upnlInspectionType.Update()
            End If
        Next

        Dim k As Integer
        For k = 0 To cmbType.Items.Count - 1
            'cmbService Disabled
            If cmbType.Items(k).Selected = False And (cmbType.Items(k).Text = "Service" Or cmbType.Items(k).Text = "MPD") Then
                ListServiceType.Enabled = False
                For l As Integer = 0 To ListServiceType.Items.Count - 1
                    ListServiceType.Items.Item(l).Selected = ListServiceType.Enabled = False
                Next
                hdnService.Value = cmbType.Items.Item(cmbType.SelectedIndex).Selected
                upnlServiceType.Update()
            End If

            'cmbInspection Disabled
            If cmbType.Items(k).Selected = False And cmbType.Items(k).Text = "Inspection" Then
                ListInspectionType.Enabled = False
                For l As Integer = 0 To ListInspectionType.Items.Count - 1
                    ListInspectionType.Items.Item(l).Selected = ListInspectionType.Enabled = False
                Next
                hdnInspection.Value = cmbType.Items.Item(cmbType.SelectedIndex).Selected
                upnlInspectionType.Update()
            End If
        Next
        upnlImgBtn.Update()
        ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "disableEnable", "disableEnable();", True)

    End Sub

    Public Function ReportDetail() As ReportMaintenanceDetailList

        Try

            Dim ObjMachine As MachineInfo
            Dim ObjAssemblyStatus As AssemblyStatusInfo
            Dim ObjCompStatus As CompStatusInfo
            Dim ObjAssemblyMonitorInspStatus As AssemblyMonitorInspStatusInfo
            Dim ObjAssemblyMonitorInspStatusPeriod As AssemblyMonitorInspStatusPeriodInfo
            Dim ObjAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatusInfo
            Dim ObjAssemblyMonitorServiceStatusPeriod As AssemblyMonitorServiceStatusPeriodInfo
            Dim ObjCompMonitorInspStatus As CompMonitorInspStatusInfo
            Dim ObjCompMonitorInspStatusPeriod As CompMonitorInspStatusPeriodInfo
            Dim ObjCompMonitorServiceStatus As CompMonitorServiceStatusInfo
            Dim ObjCompMonitorServiceStatusPeriod As CompMonitorServiceStatusPeriodInfo

            'Added By Utkarsh ON 12-Sep-2013 FOR BA11092013  parameters (ByPerdayLimit,PerDayLimits ,IsAverageRequired)
            mMachineList = MachineList.GetMachineListMonitoringStatus(New SmartDate(AsonDate).Text,
                                                                      MachineName, , , , , , , , , , ,
                                                                      True, ,
                                                                      AssemblyName,
                                                                      IsAverageRequired:=mIsAverageRequired,
                                                                      ByPerDayLimit:=mByPerDayLimit,
                                                                      PerdayLimits:=mPerDayLimits,
                                                                      SkipIsForInventoryAircarft:=True,
                                                                      MonitoringInspRequired:=False,
                                                                      MonitoringModRequired:=False,
                                                                      MonitoringServiceRequired:=False)

            Dim LHLabel2 As String = ""
            Dim LHData2 As String = ""

            'Added by Saylee on 25-May-2016
            If (AppSettings("ClientCode") = "RAL") Then

                If Not cmbAircraft.SelectedItem.ToString = "(All)" Then

                    mtmpMachineList = tmpMachineList.GetMachineList(,
                                                                    Aircraft, , , , ,
                                                                    True,
                                                                    AsonDate)
                    Dim mOtherPeriodExists As String = "False"
                    Dim mOtherPeriods As String = String.Empty

                    For i As Integer = 0 To mtmpMachineList.Count - 1

                        mOtherPeriods = CType(IIf(mtmpMachineList(i).RINS = "",
                                                  "",
                                                  mtmpMachineList(i).RINS & "(RI)" & vbCrLf), String) + vbCrLf +
                                        CType(IIf(mtmpMachineList(i).NGCycles = "",
                                                  "",
                                                  mtmpMachineList(i).NGCycles & "(NG)" & vbCrLf), String) + vbCrLf +
                                        CType(IIf(mtmpMachineList(i).NFCycles = "",
                                                  "",
                                                  mtmpMachineList(i).NFCycles & "(NF)" & vbCrLf), String)

                        If mOtherPeriods <> "" Then

                            mOtherPeriodExists = "True"
                            Exit For

                        End If

                    Next

                    For i As Integer = 0 To mtmpMachineList.Count - 1

                        searchstr7 = mtmpMachineList(i).Owner.ToString

                        mOtherPeriods = CType(IIf(mtmpMachineList(i).RINS = "",
                                                  "",
                                                  mtmpMachineList(i).RINS & "(RI)" & vbCrLf), String) + vbCrLf +
                                        CType(IIf(mtmpMachineList(i).NGCycles = "",
                                                  "",
                                                  mtmpMachineList(i).NGCycles & "(NG)" & vbCrLf), String) + vbCrLf +
                                        CType(IIf(mtmpMachineList(i).NFCycles = "",
                                                  "",
                                                  mtmpMachineList(i).NFCycles & "(NF)" & vbCrLf), String)

                        ReportStatusList.Add(New rptStatus(mtmpMachineList(i).ID.ToString,
                                                               1, , , , ,
                                                               mtmpMachineList(i).TSO, ,
                                                               mtmpMachineList(i).CSO, , , , , , , , ,
                                                               mtmpMachineList(i).Cycles,
                                                               mOtherPeriods,
                                                               mOtherPeriodExists,
                                                               Year(txtFromDate.Text).ToString, ,
                                                               mtmpMachineList(i).RegNo,
                                                               mtmpMachineList(i).ModelName, mtmpMachineList(i).Type,
                                                               mtmpMachineList(i).SerialNo,
                                                               mtmpMachineList(i).ManufacturerName, ,
                                                               mtmpMachineList(i).ManufacturingDate,
                                                               mtmpMachineList(i).Hours,
                                                               mtmpMachineList(i).Landings))

                    Next

                End If

            Else

                For Each ObjMachine In mMachineList

                    For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList

                        Periodcount = ObjAssemblyStatus.AssemblyStatusPeriodList.Count()
                        LHLabel2 = ""
                        LHData2 = ""
                        For Count = 0 To Periodcount - 1

                            If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID <> 2 Then
                                LHLabel2 = CType(IIf(LHLabel2 = "", LHLabel2, LHLabel2 + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                                LHData2 = CType(IIf(LHData2 = "", LHData2, LHData2 + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyCurrentValue
                            End If

                        Next

                        If ObjAssemblyStatus.Position <> "" Then
                            SerialNoPostion = ObjAssemblyStatus.SerialNo + "(" + ObjAssemblyStatus.Position + ")"
                        Else
                            SerialNoPostion = ObjAssemblyStatus.SerialNo
                        End If

                        AssemblyID = ObjAssemblyStatus.AssemblyID

                        If (AppSettings("ClientCode") = "BA" Or
                            AppSettings("ClientCode") = "PAS" Or
                            AppSettings("ClientCode") = "Novo" Or
                            AppSettings("ClientCode") = "YA" Or
                            AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013

                            ReportStatusList.Add(New rptStatus(AssemblyID.ToString,
                                                                   ObjAssemblyStatus.AssemblyTypeID, ,
                                                                   "Reg. No.",
                                                                   ObjMachine.RegNo,
                                                                   ObjAssemblyStatus.AssemblyType + " " + "Model",
                                                                   ObjAssemblyStatus.Model,
                                                                   "Serial No.",
                                                                   SerialNoPostion,
                                                                   "Due As of Airframe",
                                                                   "Done On", , , , , , , , , , , ,
                                                                   LHLabel2,
                                                                   LHData2))

                        Else

                            ReportStatusList.Add(New rptStatus(AssemblyID.ToString,
                                                                   ObjAssemblyStatus.AssemblyTypeID, ,
                                                                   "Reg. No.",
                                                                   ObjMachine.RegNo,
                                                                   ObjAssemblyStatus.AssemblyType + " " + "Model",
                                                                   ObjAssemblyStatus.Model,
                                                                   "Serial No.",
                                                                   SerialNoPostion,
                                                                   "Due As of " & ObjAssemblyStatus.AssemblyType,
                                                                   "Done On " & ObjAssemblyStatus.AssemblyType, , , , , , , , , , , ,
                                                                   LHLabel2,
                                                                   LHData2))


                        End If
                        '-------------------
                    Next

                    searchstr7 = ObjMachine.Customer.ToString  ' Changed By Saylee On 8-Aug-2011 '"Owner/Operator :- " +
                Next

            End If

            Dim ServiceTypeIds As New StringBuilder
            Dim InspTypeIds As New StringBuilder

            For K As Integer = 0 To ListServiceType.Items.Count - 1
                If ListServiceType.Items.Item(K).Selected Then
                    ServiceTypeIds.Append(ListServiceType.Items.Item(K).Value + ",")
                End If
            Next

            For K As Integer = 0 To ListInspectionType.Items.Count - 1
                If ListInspectionType.Items.Item(K).Selected Then
                    InspTypeIds.Append(ListInspectionType.Items.Item(K).Value + ",")
                End If
            Next

            'Added By Vikrant On 08-Dec-2020 For ALL08122020-1
            Dim mLinkMaintenanceList As LinkMaintenanceList
            mLinkMaintenanceList = LinkMaintenanceList.GetLinkMaintenanceList()

            mMachineList = MachineList.GetMachineListMonitoringStatus(AsonDate,
                                                                      MachineName, , , , , , , , , ,
                                                                      IIf(chkComponent.Checked, True, False),
                                                                      True, ,
                                                                      AssemblyName, , , , , , , , ,
                                                                      ATACode, ATANomenclature,
                                                                      ShowCofA, ,
                                                                      IIf(ServiceTypeIds.ToString <> "" And chkAssembly.Checked, True, False),
                                                                      IIf(InspTypeIds.ToString <> "" And chkAssembly.Checked, True, False), , , , , , , ,
                                                                      False, ,
                                                                      False, ,
                                                                      True, , ,
                                                                      0,
                                                                      0, ,
                                                                      IIf(ServiceTypeIds.ToString <> "" And chkComponent.Checked, True, False),
                                                                      IsAverageRequired:=mIsAverageRequired,
                                                                      ByPerDayLimit:=mByPerDayLimit,
                                                                      PerdayLimits:=mPerDayLimits,
                                                                      SkipIsForInventoryAircarft:=True,
                                                                      MonitorServiceTypeIDs:=ServiceTypeIds.ToString.TrimEnd(","),
                                                                      MonitorInspTypeIDs:=InspTypeIds.ToString.TrimEnd(","), MonitorModTypeIDs:="",
                                                                      CompMonitoringInspRequired:=IIf(InspTypeIds.ToString <> "" And chkComponent.Checked, True, False))
            'End

            If IsSerSelect = True Then

                For Each ObjMachine In mMachineList

                    For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList

                        If chkAssembly.Checked = True Then

                            For Each ObjAssemblyMonitorServiceStatus In ObjAssemblyStatus.AssemblyMonitorServiceStatusList

                                ''If (ObjAssemblyMonitorServiceStatus.IsApplicable = True) Or
                                ''   (ObjAssemblyMonitorServiceStatus.IsApplicable = False And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Or
                                ''   (cmbFormat.SelectedValue = "2" AndAlso
                                If (ObjAssemblyMonitorServiceStatus.IsApplicable = True And chkNotApplicable.Checked = False) Or
                                   (chkNotApplicable.Checked = True) Or
                                   (cmbFormat.SelectedValue = "2" AndAlso
                                         mLinkMaintenanceList.Contains(ObjAssemblyMonitorServiceStatus.ModelMonitorServiceID, "")) Then    'Checking Apllicablility

                                    ATAChapter = ObjAssemblyMonitorServiceStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorServiceStatus.ATANomenclature

                                    TaskNo = ObjAssemblyMonitorServiceStatus.TaskNo
                                    Description = ObjAssemblyMonitorServiceStatus.Description
                                    Position = ObjAssemblyStatus.Position

                                    MonitorType = ObjAssemblyMonitorServiceStatus.Type
                                    AssemblyModel = ObjAssemblyStatus.Model
                                    AssemblySerialNo = ObjAssemblyStatus.SerialNo
                                    DoneOnDate = ObjAssemblyMonitorServiceStatus.DoneOn
                                    'Added By Utkarsh ON 12-Sep-2013 FOR BA11092013
                                    EstimatedDate = ObjAssemblyMonitorServiceStatus.EstimatedDateFormatted
                                    Code = ObjAssemblyMonitorServiceStatus.ModelMonitorServiceCode
                                    If AppSettings("ClientCode") = "7AR" Then
                                        MonitorTypeCode = ObjAssemblyMonitorServiceStatus.ServiceTypeCode
                                        SourceDoc = ObjAssemblyMonitorServiceStatus.Source
                                    Else
                                        MonitorTypeCode = ObjAssemblyMonitorServiceStatus.Code
                                        SourceDoc = ObjAssemblyMonitorServiceStatus.SourceDoc
                                    End If



                                    'End
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
                                    Extension = ""
                                    Extension1 = ""
                                    Extension2 = ""
                                    SinceNew2 = ""
                                    DoneAt2 = ""
                                    StartDateData = ""
                                    Periodcount = ObjAssemblyStatus.AssemblyStatusPeriodList.Count()
                                    TimeSinceNew = ""
                                    DoneONValueForNoPeriod = ""
                                    DiffCompInstDoneOnValue = ""
                                    Dim IsPeriod2Exists As Boolean = False

                                    'Added by Saylee on 19-Sep-2014 for ALL19092014
                                    Dim mDoneONValueForNoPeriod As Period = New Period(1, DBNull.Value)
                                    Dim mTSOValueForNoPeriod As Period = New Period(1, DBNull.Value)
                                    Dim mPeriodID As Integer = 0

                                    DoneONValueForNoPeriod = String.Empty
                                    TSOValueForNoPeriod = String.Empty

                                    Dim mDoneONValueForAssembly As Period = New Period(1, DBNull.Value)
                                    DoneONValueForAssembly = String.Empty

                                    DiffCompInstDoneOnValue = String.Empty

                                    For Count = 0 To Periodcount - 1

                                        'Added By Saylee on 22-Jun-2011
                                        If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 2 Then

                                            If ObjAssemblyMonitorServiceStatus.DoneOn = "" Then

                                                For Each tmpObjAssemblyMonitorServiceStatusPeriod As AssemblyMonitorServiceStatusPeriodInfo In ObjAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriodList

                                                    If tmpObjAssemblyMonitorServiceStatusPeriod.PeriodID = 2 Then
                                                        IsPeriod2Exists = True
                                                        Exit For
                                                    End If

                                                Next

                                                If IsPeriod2Exists = True Then

                                                    For Each ObjAssemblyMonitorServiceStatusPeriod In ObjAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriodList

                                                        If ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 2 Then
                                                            StartDateLabel = CType(IIf(StartDateLabel = "", StartDateLabel, StartDateLabel + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                                                            StartDateData = CType(IIf(StartDateData = "", StartDateData, StartDateData + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjAssemblyMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                        End If

                                                    Next

                                                Else
                                                    StartDateLabel = CType(IIf(StartDateLabel = "", StartDateLabel, StartDateLabel + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                                                    StartDateData = CType(IIf(StartDateData = "", StartDateData, StartDateData + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyStartValueFormatted
                                                End If

                                            Else
                                                StartDateLabel = CType(IIf(StartDateLabel = "", StartDateLabel, StartDateLabel + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                                                StartDateData = CType(IIf(StartDateData = "", StartDateData, StartDateData + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjAssemblyMonitorServiceStatus.DoneOnFormatted
                                            End If

                                        End If

                                        'If no Cycle/Hour Period Present in Monitor Service       
                                        mDoneONValueForNoPeriod = New Period(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, DBNull.Value)
                                        mTSOValueForNoPeriod = New Period(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, DBNull.Value)

                                        If (ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 1 Or ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 3) And
                                            Not (ObjAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriodList.Contains(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID)) Then

                                            Dim mPeriodUnitID As Integer = 0

                                            If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 1 Then
                                                mPeriodUnitID = 1
                                            ElseIf ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 3 Then
                                                mPeriodUnitID = 6
                                            End If

                                            mPeriodID = ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID

                                            If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = False) Then
                                                DoneONValueForNoPeriod = ""
                                            Else

                                                If ObjAssemblyMonitorServiceStatus.DoneOn <> "" Then

                                                    Dim mMachineMaintenance As MachineMaintenance = MachineMaintenance.GetMachineMaintenance(ObjAssemblyMonitorServiceStatus.ID, MachineMaintenanceActivity.AssemblyService)

                                                    If CurrentValueOnAsOnDate.GetCurrentValue(mMachineMaintenance.LogAssemblyStatusID, ObjAssemblyMonitorServiceStatus.DoneOn, mMachineMaintenance.LogNo, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID)(0).CurrentValueDec > 0 Then

                                                        mDoneONValueForNoPeriod.Value = CurrentValueOnAsOnDate.GetCurrentValue(mMachineMaintenance.LogAssemblyStatusID, ObjAssemblyMonitorServiceStatus.DoneOn, mMachineMaintenance.LogNo, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID)(0).CurrentValue
                                                        mTSOValueForNoPeriod.Value = New Period(mPeriodID, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).AssemblyCurrentValueInDeciaml - mDoneONValueForNoPeriod.DbValueDec, mPeriodUnitID, , , 1).Value

                                                        If DoneONValueForNoPeriod = "" Then
                                                            DoneONValueForNoPeriod = mDoneONValueForNoPeriod.TextFormatted
                                                        Else
                                                            DoneONValueForNoPeriod = DoneONValueForNoPeriod + IIf(IsExcel, Chr(10), vbCrLf) + mDoneONValueForNoPeriod.TextFormatted
                                                        End If

                                                        If TSOValueForNoPeriod = "" Then
                                                            TSOValueForNoPeriod = mTSOValueForNoPeriod.TextFormatted
                                                        Else
                                                            TSOValueForNoPeriod = TSOValueForNoPeriod + IIf(IsExcel, Chr(10), vbCrLf) + mTSOValueForNoPeriod.TextFormatted
                                                        End If

                                                    End If

                                                End If

                                            End If

                                        End If

                                    Next

                                    For Each ObjAssemblyMonitorServiceStatusPeriod In ObjAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriodList

                                        If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = False) Then
                                            DoneONValueForAssembly = ""
                                        Else
                                            If ObjAssemblyMonitorServiceStatus.DoneOn <> "" Then

                                                mDoneONValueForAssembly = New Period(ObjAssemblyMonitorServiceStatusPeriod.PeriodID, DBNull.Value)

                                                If ObjAssemblyMonitorServiceStatusPeriod.PeriodID <> 2 Then

                                                    Dim mMachineMaintenance As MachineMaintenance = MachineMaintenance.GetMachineMaintenance(ObjAssemblyMonitorServiceStatus.ID, MachineMaintenanceActivity.AssemblyService)

                                                    If CurrentValueOnAsOnDate.GetCurrentValue(mMachineMaintenance.LogAssemblyStatusID, ObjAssemblyMonitorServiceStatus.DoneOn, mMachineMaintenance.LogNo, ObjAssemblyMonitorServiceStatusPeriod.PeriodID)(0).CurrentValueDec > 0 Then

                                                        mDoneONValueForAssembly.Value = CurrentValueOnAsOnDate.GetCurrentValue(mMachineMaintenance.LogAssemblyStatusID, ObjAssemblyMonitorServiceStatus.DoneOn, mMachineMaintenance.LogNo, ObjAssemblyMonitorServiceStatusPeriod.PeriodID)(0).CurrentValue

                                                        If DoneONValueForAssembly = "" Then
                                                            DoneONValueForAssembly = mDoneONValueForAssembly.TextFormatted
                                                        Else
                                                            DoneONValueForAssembly = DoneONValueForAssembly + IIf(IsExcel, Chr(10), vbCrLf) + mDoneONValueForAssembly.TextFormatted
                                                        End If

                                                    End If

                                                Else

                                                    If DoneONValueForAssembly = "" Then
                                                        DoneONValueForAssembly = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                    Else
                                                        DoneONValueForAssembly = DoneONValueForAssembly + IIf(IsExcel, Chr(10), vbCrLf) + ObjAssemblyMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                    End If

                                                End If

                                            End If

                                        End If

                                        '**************************
                                        If ReportStatus = 0 Then 'Landscape

                                            If ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 1 Then     'Hours

                                                Freq1 = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue

                                                If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Or
                                                   (ObjAssemblyMonitorServiceStatus.IsApplicable = False And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Then

                                                    ElapsedTime = ""
                                                    RemainingTime = ""
                                                    DueAsof = ""
                                                    'Added By Prashant 3-Aug-2009
                                                    SinceNew2 = ""
                                                    '----------------------------
                                                    TimeSinceNew = ""
                                                Else

                                                    ElapsedTime = ObjAssemblyMonitorServiceStatusPeriod.AllElapsedValue
                                                    RemainingTime = ObjAssemblyMonitorServiceStatusPeriod.RemainingValue
                                                    'Added By Prashant 17-Sep-2013
                                                    'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                    'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                    'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf
                                                    If (AppSettings("ClientCode") = "BA" Or
                                                        AppSettings("ClientCode") = "PAS" Or
                                                        AppSettings("ClientCode") = "Novo" Or
                                                        AppSettings("ClientCode") = "YA" Or
                                                        AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013

                                                        DueAsof = ObjAssemblyMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                    Else
                                                        DueAsof = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
                                                    End If

                                                    'Added By Prashant 3-Aug-2009
                                                    If ObjAssemblyMonitorServiceStatus.MonitorTypeID = 3 And ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue = "" Then
                                                        SinceNew2 = ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValue
                                                    Else
                                                        SinceNew2 = ObjAssemblyMonitorServiceStatusPeriod.DiffAssemblyCurrentDoneOnValueFormatted
                                                    End If
                                                    '----------------------------
                                                    TimeSinceNew = ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValue

                                                End If

                                                Extension = ObjAssemblyMonitorServiceStatusPeriod.ExtensionValue

                                            End If

                                            If ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 2 Then     'StartDate

                                                Freq2 = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValueFormatted

                                                If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Or
                                                   (ObjAssemblyMonitorServiceStatus.IsApplicable = False And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Then

                                                    ElapsedTime1 = ""
                                                    RemainingTime1 = ""
                                                    DueAsof1 = ""

                                                Else
                                                    ElapsedTime1 = ObjAssemblyMonitorServiceStatusPeriod.AllElapsedValueFormatted
                                                    RemainingTime1 = ObjAssemblyMonitorServiceStatusPeriod.RemainingValueFormatted
                                                    DueAsof1 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted
                                                End If

                                                Extension1 = ObjAssemblyMonitorServiceStatusPeriod.ExtensionValueFormatted

                                            End If

                                            If ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 3 Or
                                               ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 4 Or
                                               ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 5 Or
                                               ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 6 Or
                                               ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 7 Or
                                               ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 8 Or
                                               ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 12 Or
                                               ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 13 Or
                                               ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 14 Then

                                                If Freq3 = "" Then
                                                    Freq3 = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue

                                                    If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Then
                                                        ElapsedTime2 = ""
                                                        RemainingTime2 = ""
                                                        DueAsof2 = ""
                                                        AssemblyDueAsof2 = ""
                                                        SinceNew2 = ""
                                                        TimeSinceNew = ""

                                                    Else

                                                        ElapsedTime2 = ObjAssemblyMonitorServiceStatusPeriod.AllElapsedValue
                                                        RemainingTime2 = ObjAssemblyMonitorServiceStatusPeriod.RemainingValue

                                                        If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Or AppSettings("ClientCode") = "STR") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                            DueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                        Else
                                                            DueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
                                                        End If

                                                        AssemblyDueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue

                                                        If ObjAssemblyMonitorServiceStatus.MonitorTypeID = 3 And ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue = "" Then
                                                            SinceNew2 = ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValue
                                                        Else
                                                            SinceNew2 = ObjAssemblyMonitorServiceStatusPeriod.DiffAssemblyCurrentDoneOnValue
                                                        End If

                                                        TimeSinceNew = ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValue

                                                    End If

                                                    Extension2 = ObjAssemblyMonitorServiceStatusPeriod.ExtensionValue

                                                    If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = False) Then
                                                        DoneAt2 = ""
                                                    Else
                                                        DoneAt2 = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
                                                    End If

                                                Else

                                                    Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue

                                                    If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Or
                                                       (ObjAssemblyMonitorServiceStatus.IsApplicable = False And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Then

                                                        ElapsedTime2 = ""
                                                        RemainingTime2 = ""
                                                        DueAsof2 = ""
                                                        AssemblyDueAsof2 = ""
                                                        SinceNew2 = ""
                                                        TimeSinceNew = ""
                                                    Else

                                                        ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.AllElapsedValue
                                                        RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.RemainingValue

                                                        If (AppSettings("ClientCode") = "BA" Or
                                                            AppSettings("ClientCode") = "PAS" Or
                                                            AppSettings("ClientCode") = "Novo" Or
                                                            AppSettings("ClientCode") = "YA" Or
                                                            AppSettings("ClientCode") = "TA" Or
                                                            AppSettings("ClientCode") = "STR") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013

                                                            DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                        Else
                                                            DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
                                                        End If

                                                        AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.DueOnValue

                                                        If ObjAssemblyMonitorServiceStatus.MonitorTypeID = 3 And ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue = "" Then
                                                            SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValue
                                                        Else
                                                            SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.DiffAssemblyCurrentDoneOnValue
                                                        End If

                                                        TimeSinceNew = TimeSinceNew & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValue

                                                    End If

                                                    Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.ExtensionValue

                                                    If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = False) Then
                                                        DoneAt2 = ""
                                                    Else
                                                        DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
                                                    End If

                                                End If

                                            End If

                                        Else    '  Report = 1

                                            If ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 2 Then     'Start Date

                                                If Freq3 = "" Then

                                                    Freq3 = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValueFormatted
                                                    If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Or
                                                       (ObjAssemblyMonitorServiceStatus.IsApplicable = False And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Then

                                                        ElapsedTime2 = ""
                                                        RemainingTime2 = ""
                                                        DueAsof2 = ""
                                                        AssemblyDueAsof2 = ""

                                                    Else

                                                        ElapsedTime2 = ObjAssemblyMonitorServiceStatusPeriod.ElapsedValueFormatted
                                                        RemainingTime2 = ObjAssemblyMonitorServiceStatusPeriod.RemainingValueFormatted
                                                        DueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted
                                                        AssemblyDueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted

                                                    End If

                                                    Extension2 = ObjAssemblyMonitorServiceStatusPeriod.ExtensionValueFormatted

                                                    If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = False) Then
                                                        DoneAt2 = ""
                                                    Else
                                                        DoneAt2 = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                    End If

                                                Else

                                                    Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.FrequencyValueFormatted

                                                    If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Or
                                                       (ObjAssemblyMonitorServiceStatus.IsApplicable = False And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Then

                                                        ElapsedTime2 = ""
                                                        RemainingTime2 = ""
                                                        DueAsof2 = ""
                                                        AssemblyDueAsof2 = ""

                                                    Else

                                                        ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.AllElapsedValueFormatted
                                                        RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.RemainingValueFormatted
                                                        DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted
                                                        AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted

                                                    End If

                                                    Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.ExtensionValueFormatted

                                                    If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = False) Then
                                                        DoneAt2 = ""
                                                    Else
                                                        DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                    End If

                                                End If

                                            Else                                                           'For PeriodID <> 2

                                                If Freq3 = "" Then

                                                    Freq3 = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue

                                                    If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Or
                                                        (ObjAssemblyMonitorServiceStatus.IsApplicable = False And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Then

                                                        ElapsedTime2 = ""
                                                        RemainingTime2 = ""
                                                        DueAsof2 = ""
                                                        AssemblyDueAsof2 = ""
                                                        SinceNew2 = ""
                                                        TimeSinceNew = ""

                                                    Else

                                                        ElapsedTime2 = ObjAssemblyMonitorServiceStatusPeriod.AllElapsedValue
                                                        RemainingTime2 = ObjAssemblyMonitorServiceStatusPeriod.RemainingValue

                                                        If (AppSettings("ClientCode") = "BA" Or
                                                            AppSettings("ClientCode") = "PAS" Or
                                                            AppSettings("ClientCode") = "Novo" Or
                                                            AppSettings("ClientCode") = "YA" Or
                                                            AppSettings("ClientCode") = "TA" Or
                                                            AppSettings("ClientCode") = "STR") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013

                                                            DueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                        Else
                                                            DueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
                                                        End If

                                                        AssemblyDueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue

                                                        If ObjAssemblyMonitorServiceStatus.MonitorTypeID = 3 And ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue = "" Then
                                                            SinceNew2 = ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValue
                                                        Else
                                                            SinceNew2 = ObjAssemblyMonitorServiceStatusPeriod.DiffAssemblyCurrentDoneOnValue
                                                        End If

                                                        TimeSinceNew = ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValue

                                                    End If

                                                    Extension2 = ObjAssemblyMonitorServiceStatusPeriod.ExtensionValue

                                                    If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = False) Then
                                                        DoneAt2 = ""
                                                    Else
                                                        DoneAt2 = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
                                                    End If

                                                Else

                                                    Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue

                                                    If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Or
                                                       (ObjAssemblyMonitorServiceStatus.IsApplicable = False And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Then

                                                        ElapsedTime2 = ""
                                                        RemainingTime2 = ""
                                                        DueAsof2 = ""
                                                        AssemblyDueAsof2 = ""
                                                        SinceNew2 = ""
                                                        TimeSinceNew = ""

                                                    Else

                                                        ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.AllElapsedValue
                                                        RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.RemainingValue

                                                        If (AppSettings("ClientCode") = "BA" Or
                                                            AppSettings("ClientCode") = "PAS" Or
                                                            AppSettings("ClientCode") = "Novo" Or
                                                            AppSettings("ClientCode") = "YA" Or
                                                            AppSettings("ClientCode") = "TA" Or
                                                            AppSettings("ClientCode") = "STR") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013

                                                            DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                        Else
                                                            DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
                                                        End If

                                                        AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.DueOnValue

                                                        If ObjAssemblyMonitorServiceStatus.MonitorTypeID = 3 And ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue = "" Then
                                                            SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValue
                                                        Else
                                                            SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.DiffAssemblyCurrentDoneOnValue
                                                        End If

                                                        TimeSinceNew = TimeSinceNew & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValue

                                                    End If

                                                    Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.ExtensionValue

                                                    If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = False) Then
                                                        DoneAt2 = ""
                                                    Else
                                                        DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
                                                    End If

                                                End If

                                            End If

                                        End If

                                    Next

                                    AssemblyID = ObjAssemblyStatus.AssemblyID
                                    Note = ObjAssemblyMonitorServiceStatus.Notes
                                    Remark = ObjAssemblyMonitorServiceStatus.DoneRemark
                                    ExtensionDate = ObjAssemblyMonitorServiceStatus.ExtensionDate
                                    ApprovalRemark = ObjAssemblyMonitorServiceStatus.ApprovalRemark
                                    Reference = ObjAssemblyMonitorServiceStatus.Reference  'Added By Prashant 3-Apr-2013  'Indamer03042013
                                    Dim ATACode As Integer = ObjAssemblyMonitorServiceStatus.ATACode


                                    'Added By Vikrant On 22-Dec-2020 For Star Air
                                    mnWOListForDueJobs = nWOListForDueJobs.GetWOListForDueJobs(ObjAssemblyMonitorServiceStatus.ID)
                                    If mnWOListForDueJobs.Count > 0 Then
                                        nWONumber = mnWOListForDueJobs(0).WONumber
                                    Else
                                        nWONumber = ""
                                    End If
                                    'End

                                    If IsExcel Then

                                        If ATACode.ToString.Length < 3 Then
                                            ATAChapter = ATACode.ToString.PadLeft(3, "0"c) + " " + "-" + " " + ObjAssemblyMonitorServiceStatus.ATANomenclature
                                        End If

                                        If Freq1 <> "" Then
                                            Freq1 = Freq1 + IIf(Freq2 <> "", Chr(10) + Freq2, "") + IIf(Freq3 <> "", Chr(10) + Freq3, "")
                                        Else
                                            Freq1 = Freq2 + IIf(Freq3 <> "", Chr(10) + Freq3, "")
                                        End If

                                        If DueAsof <> "" Then
                                            DueAsof = DueAsof + IIf(DueAsof1 <> "", Chr(10) + DueAsof1, "") + IIf(DueAsof2 <> "", Chr(10) + DueAsof2, "")
                                        Else
                                            DueAsof = DueAsof1 + IIf(DueAsof2 <> "", Chr(10) + DueAsof2, "")
                                        End If

                                        If ElapsedTime <> "" Then
                                            ElapsedTime = ElapsedTime + IIf(ElapsedTime1 <> "", Chr(10) + ElapsedTime1, "") + IIf(ElapsedTime2 <> "", Chr(10) + ElapsedTime2, "")
                                        Else
                                            ElapsedTime = ElapsedTime1 + IIf(ElapsedTime2 <> "", Chr(10) + ElapsedTime2, "")
                                        End If

                                        If RemainingTime <> "" Then
                                            RemainingTime = RemainingTime + IIf(RemainingTime1 <> "", Chr(10) + RemainingTime1, "") + IIf(RemainingTime2 <> "", Chr(10) + RemainingTime2, "")
                                        Else
                                            RemainingTime = RemainingTime1 + IIf(RemainingTime2 <> "", Chr(10) + RemainingTime2, "")
                                        End If

                                    End If

                                    If ObjAssemblyMonitorServiceStatus.IsApplicable = False Then
                                        DueAsof = ""
                                        DueAsof1 = ""
                                        DueAsof2 = ""
                                        RemainingTime = ""
                                        RemainingTime1 = ""
                                        RemainingTime2 = ""
                                    End If


                                    ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , ,
                                                                                                 AssemblySerialNo,
                                                                                                 ATAChapter, , ,
                                                                                                 Position,
                                                                                                 MonitorType,
                                                                                                 MonitorTypeCode,
                                                                                                 Note,
                                                                                                 Remark,
                                                                                                 Description, ,
                                                                                                 EstimatedDate, , ,
                                                                                                 Freq1,
                                                                                                 Freq2,
                                                                                                 Freq3,
                                                                                                 ElapsedTime,
                                                                                                 ElapsedTime1,
                                                                                                 ElapsedTime2,
                                                                                                 RemainingTime,
                                                                                                 RemainingTime1,
                                                                                                 RemainingTime2,
                                                                                                 DueAsof,
                                                                                                 DueAsof1,
                                                                                                 DueAsof2,
                                                                                                 AssemblyModel, , ,
                                                                                                 SinceNew2, , ,
                                                                                                 DoneAt2, , , , , , , ,
                                                                                                 StartDateData, , , , , , , , , ,
                                                                                                 Reference, ,
                                                                                                 DoneOnDate, , , , , ,
                                                                                                 AssemblyDueAsof2,
                                                                                                 Extension,
                                                                                                 Extension1,
                                                                                                 Extension2,
                                                                                                 ExtensionDate,
                                                                                                 ApprovalRemark, , ,
                                                                                                 Code, , ,
                                                                                                 SupersededByADNumber:=TaskNo, ,
                                                                                                 ObjAssemblyMonitorServiceStatus.IsApplicable, ,
                                                                                                 ObjAssemblyMonitorServiceStatus.MonitorTypeID,  , , , , ,  ,
                                                                                                 TimeSinceNew:=TimeSinceNew,
                                                                                                 WONumber:=nWONumber,
                                                                                                 DoneONValueForAssembly:=DoneONValueForAssembly,
                                                                                                 SourceDoc:=SourceDoc,
                                                                                                 Zone:=ObjAssemblyMonitorServiceStatus.Zone,
                                                                                                 Area:=ObjAssemblyMonitorServiceStatus.Area,
                                                                                                 TaskNo:=TaskNo))

                                End If

                            Next

                        End If

                        If chkComponent.Checked Then

                            For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList

                                For Each ObjCompMonitorServiceStatus In ObjCompStatus.CompMonitorServiceStatusList

                                    ''If (ObjCompMonitorServiceStatus.IsApplicable = True) Or
                                    ''   (ObjCompMonitorServiceStatus.IsApplicable = False And ObjCompMonitorServiceStatus.IsCompleted = True) Then

                                    If (ObjCompMonitorServiceStatus.IsApplicable = True And chkNotApplicable.Checked = False) Or
                                         (chkNotApplicable.Checked = True) Then

                                        ATAChapter = ObjCompMonitorServiceStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorServiceStatus.ATANomenclature
                                        TaskNo = ObjCompMonitorServiceStatus.TaskNo
                                        Description = ObjCompMonitorServiceStatus.Description
                                        PartNo = ObjCompStatus.PartName
                                        CompSerialNo = ObjCompStatus.CompSerialNo
                                        Position = ObjCompStatus.Position

                                        MonitorType = ObjCompMonitorServiceStatus.Type
                                        AssemblyModel = ObjAssemblyStatus.Model
                                        AssemblySerialNo = ObjAssemblyStatus.SerialNo
                                        DoneOnDate = ObjCompMonitorServiceStatus.DoneOn
                                        'Added By Utkarsh ON 12-Sep-2013 FOR BA11092013
                                        EstimatedDate = ObjCompMonitorServiceStatus.EstimatedDateFormatted
                                        Code = ObjCompMonitorServiceStatus.PartMonitorServiceCode
                                        ''Code = ObjCompMonitorServiceStatus.PartMonitorServiceCode
                                        If AppSettings("ClientCode") = "7AR" Then
                                            MonitorTypeCode = ObjCompMonitorServiceStatus.ServiceTypeCode
                                            SourceDoc = ObjCompMonitorServiceStatus.Source
                                        Else
                                            MonitorTypeCode = ObjCompMonitorServiceStatus.Code
                                            SourceDoc = ObjCompMonitorServiceStatus.SourceDoc
                                        End If

                                        'End
                                        Periodcount = ObjCompStatus.CompStatusPeriodList.Count()
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
                                        Extension = ""
                                        Extension1 = ""
                                        Extension2 = ""
                                        SinceNew2 = ""
                                        DoneAt2 = ""
                                        StartDateData = ""
                                        TimeSinceNew = ""
                                        Dim IsPeriod2Exists As Boolean = False

                                        'Added by Saylee on 19-Sep-2014 for ALL19092014
                                        Dim mDoneONValueForNoPeriod As Period = New Period(1, DBNull.Value)
                                        Dim mTSOValueForNoPeriod As Period = New Period(1, DBNull.Value)
                                        Dim tmpCurrentValue As Decimal = 0
                                        Dim mPeriodID As Integer = 0
                                        DoneONValueForNoPeriod = String.Empty
                                        TSOValueForNoPeriod = String.Empty

                                        'Added By Saylee on 25-May-2015 for Taj25052015
                                        Dim mDoneONValueForAssembly As Period = New Period(1, DBNull.Value)
                                        DoneONValueForAssembly = String.Empty

                                        'Added by Saylee on 7-Jun-2016
                                        Dim mDiffCompInstDoneOnValue As Period = New Period(1, DBNull.Value)
                                        DiffCompInstDoneOnValue = String.Empty

                                        DiffCompInstDoneOnValue = ""

                                        For Count = 0 To Periodcount - 1

                                            If ObjCompStatus.CompStatusPeriodList(Count).PeriodID = 2 Then

                                                If ObjCompMonitorServiceStatus.DoneOn = "" Then

                                                    For Each tmpObjCompMonitorServiceStatusPeriod As CompMonitorServiceStatusPeriodInfo In ObjCompMonitorServiceStatus.CompMonitorServiceStatusPeriodList

                                                        If tmpObjCompMonitorServiceStatusPeriod.PeriodID = 2 Then
                                                            IsPeriod2Exists = True
                                                            Exit For

                                                        End If

                                                    Next

                                                    If IsPeriod2Exists = True Then

                                                        For Each ObjCompMonitorServiceStatusPeriod In ObjCompMonitorServiceStatus.CompMonitorServiceStatusPeriodList

                                                            If ObjCompMonitorServiceStatusPeriod.PeriodID = 2 Then
                                                                StartDateLabel = CType(IIf(StartDateLabel = "", StartDateLabel, StartDateLabel + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjCompStatus.CompStatusPeriodList(Count).PeriodName
                                                                StartDateData = CType(IIf(StartDateData = "", StartDateData, StartDateData + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjCompStatus.CompStatusPeriodList(ObjCompStatus.CompStatusPeriodList(Count).PeriodID, "").CompStartValueFormatted
                                                                StartDateData = CType(IIf(StartDateData = "", StartDateData, StartDateData + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                            End If

                                                        Next

                                                    Else
                                                        StartDateLabel = CType(IIf(StartDateLabel = "", StartDateLabel, StartDateLabel + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjCompStatus.CompStatusPeriodList(Count).PeriodName 'Added by Saylee on 31-May-2010
                                                        StartDateData = CType(IIf(StartDateData = "", StartDateData, StartDateData + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjCompStatus.CompStatusPeriodList(ObjCompStatus.CompStatusPeriodList(Count).PeriodID, "").CompStartValueFormatted 'Added by Saylee on 31-May-2010
                                                    End If

                                                Else
                                                    StartDateLabel = CType(IIf(StartDateLabel = "", StartDateLabel, StartDateLabel + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjCompStatus.CompStatusPeriodList(Count).PeriodName 'Added by Saylee on 31-May-2010
                                                    StartDateData = CType(IIf(StartDateData = "", StartDateData, StartDateData + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjCompMonitorServiceStatus.DoneOnFormatted 'Added by Saylee on 31-May-2010
                                                End If

                                            End If
                                            'Added by Saylee on 19-Sep-2014 for ALL19092014
                                            'If no Cycle Period or Hour Period Present in Monitor Service
                                            mDoneONValueForNoPeriod = New Period(ObjCompStatus.CompStatusPeriodList(Count).PeriodID, DBNull.Value)
                                            mTSOValueForNoPeriod = New Period(ObjCompStatus.CompStatusPeriodList(Count).PeriodID, DBNull.Value)

                                            If (ObjCompStatus.CompStatusPeriodList(Count).PeriodID = 1 Or ObjCompStatus.CompStatusPeriodList(Count).PeriodID = 3) And Not (ObjCompMonitorServiceStatus.CompMonitorServiceStatusPeriodList.Contains(ObjCompStatus.CompStatusPeriodList(Count).PeriodID)) Then

                                                Dim mPeriodUnitID As Integer = 0

                                                If ObjCompStatus.CompStatusPeriodList(Count).PeriodID = 1 Then

                                                    mPeriodUnitID = 1

                                                ElseIf ObjCompStatus.CompStatusPeriodList(Count).PeriodID = 3 Then

                                                    mPeriodUnitID = 6

                                                End If

                                                mPeriodID = ObjCompStatus.CompStatusPeriodList(Count).PeriodID

                                                If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = False) Then

                                                    DoneONValueForNoPeriod = ""

                                                Else

                                                    If ObjCompMonitorServiceStatus.DoneOn <> "" Then

                                                        Dim mMachineMaintenance As MachineMaintenance = MachineMaintenance.GetMachineMaintenance(ObjCompMonitorServiceStatus.ID, MachineMaintenanceActivity.ComponentService)
                                                        Dim mAssemblyCurrentValue As New Period(1, DBNull.Value)

                                                        If CurrentValueOnAsOnDate.GetCurrentValue(ObjAssemblyStatus.ID, ObjCompMonitorServiceStatus.DoneOn, mMachineMaintenance.LogNo, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID)(0).CurrentValueDec > 0 Then
                                                            mAssemblyCurrentValue = New Period(mPeriodID, CurrentValueOnAsOnDate.GetCurrentValue(ObjAssemblyStatus.ID, ObjCompMonitorServiceStatus.DoneOn, mMachineMaintenance.LogNo, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID)(0).CurrentValueDec, , , , ObjMachine.HourType)
                                                        Else
                                                            mAssemblyCurrentValue = New Period(mPeriodID, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).AssemblyCurrentValueInDeciaml, , , , ObjMachine.HourType)
                                                        End If

                                                        If CurrentValueOnAsOnDate.GetCompCurrentValue(ObjCompStatus.CompID, ObjAssemblyStatus.ID, ObjCompMonitorServiceStatus.DoneOn, mMachineMaintenance.LogNo, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID).Count > 0 Then
                                                            tmpCurrentValue = New Period(mPeriodID, Period.Add(ObjCompStatus.CompStatusPeriodList(Count).PeriodID, mPeriodUnitID, CurrentValueOnAsOnDate.GetCompCurrentValue(ObjCompStatus.CompID, ObjAssemblyStatus.ID, ObjCompMonitorServiceStatus.DoneOn, mMachineMaintenance.LogNo, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID)(0).CompCurrentValueDec, Period.Difference(mAssemblyCurrentValue.DBValue, CurrentValueOnAsOnDate.GetCompCurrentValue(ObjCompStatus.CompID, ObjAssemblyStatus.ID, ObjCompMonitorServiceStatus.DoneOn, mMachineMaintenance.LogNo, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID)(0).AssemblyCurrentValueDec)), mPeriodUnitID, False, , ObjMachine.HourType).DbValueDec
                                                        Else
                                                            tmpCurrentValue = New Period(mPeriodID, Period.Add(ObjCompStatus.CompStatusPeriodList(Count).PeriodID, mPeriodUnitID, ObjCompStatus.CompStatusPeriodList(Count).CompCurrentValueInDecimal, Period.Difference(mAssemblyCurrentValue.DBValue, ObjCompStatus.CompStatusPeriodList(Count).AssemblyCurrentValueInDeciaml)), mPeriodUnitID, False, , ObjMachine.HourType).DbValueDec
                                                        End If

                                                        mDoneONValueForNoPeriod = New Period(mPeriodID, tmpCurrentValue, mPeriodUnitID, , , ObjMachine.HourType)

                                                        mTSOValueForNoPeriod.Value = New Period(ObjCompStatus.CompStatusPeriodList(Count).PeriodID, ObjCompStatus.CompStatusPeriodList(Count).CompCurrentValueInDecimal - mDoneONValueForNoPeriod.DbValueDec, mPeriodUnitID, , , 1).Value

                                                        If DoneONValueForNoPeriod = "" Then
                                                            DoneONValueForNoPeriod = mDoneONValueForNoPeriod.TextFormatted
                                                        Else
                                                            DoneONValueForNoPeriod = DoneONValueForNoPeriod + IIf(IsExcel, Chr(10), vbCrLf) + mDoneONValueForNoPeriod.TextFormatted
                                                        End If

                                                        If TSOValueForNoPeriod = "" Then
                                                            TSOValueForNoPeriod = mTSOValueForNoPeriod.TextFormatted
                                                        Else
                                                            TSOValueForNoPeriod = TSOValueForNoPeriod + IIf(IsExcel, Chr(10), vbCrLf) + mTSOValueForNoPeriod.TextFormatted
                                                        End If

                                                    End If

                                                End If

                                            End If

                                        Next

                                        For Each ObjCompMonitorServiceStatusPeriod In ObjCompMonitorServiceStatus.CompMonitorServiceStatusPeriodList

                                            'Added By Saylee on 25-May-2015 for Taj25052015                                         
                                            If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = False) Then
                                                DoneONValueForAssembly = ""
                                            Else

                                                If ObjCompMonitorServiceStatus.DoneOn <> "" Then

                                                    mDoneONValueForAssembly = New Period(ObjCompMonitorServiceStatusPeriod.PeriodID, DBNull.Value)

                                                    If ObjCompMonitorServiceStatusPeriod.PeriodID <> 2 Then

                                                        Dim mMachineMaintenance As MachineMaintenance = MachineMaintenance.
                                                                                                            GetMachineMaintenance(ObjCompMonitorServiceStatus.ID,
                                                                                                                                  MachineMaintenanceActivity.ComponentService)
                                                        Dim NoLogInMaintTable As Boolean = False

                                                        If CDate(ObjCompMonitorServiceStatus.DoneOn.ToString) < CDate(ObjAssemblyStatus.AsOnDate.ToString) Then

                                                            mDoneONValueForAssembly.DBValue = 0

                                                        ElseIf mMachineMaintenance.LogNo <> 0 Then

                                                            If CurrentValueOnAsOnDate.GetCurrentValue(ObjAssemblyStatus.ID, ObjCompMonitorServiceStatus.DoneOn, mMachineMaintenance.LogNo, ObjCompMonitorServiceStatusPeriod.PeriodID)(0).CurrentValueDec > 0 Then

                                                                mDoneONValueForAssembly.Value = CurrentValueOnAsOnDate.GetCurrentValue(ObjAssemblyStatus.ID, ObjCompMonitorServiceStatus.DoneOn, mMachineMaintenance.LogNo, ObjCompMonitorServiceStatusPeriod.PeriodID)(0).CurrentValue
                                                            End If

                                                        Else
                                                            NoLogInMaintTable = True
                                                            mDoneONValueForAssembly.DBValue = 0
                                                        End If

                                                        If NoLogInMaintTable = False Then

                                                            If DoneONValueForAssembly = "" Then
                                                                DoneONValueForAssembly = mDoneONValueForAssembly.TextFormatted
                                                            Else
                                                                DoneONValueForAssembly = DoneONValueForAssembly + IIf(IsExcel, Chr(10), vbCrLf) + mDoneONValueForAssembly.TextFormatted
                                                            End If

                                                        Else

                                                            If DoneONValueForAssembly = "" Then
                                                                DoneONValueForAssembly = ""
                                                            Else
                                                                DoneONValueForAssembly = DoneONValueForAssembly + IIf(IsExcel, Chr(10), vbCrLf) + ""
                                                            End If

                                                        End If

                                                    Else

                                                        If DoneONValueForAssembly = "" Then
                                                            DoneONValueForAssembly = ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                        Else
                                                            DoneONValueForAssembly = DoneONValueForAssembly + IIf(IsExcel, Chr(10), vbCrLf) + ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                        End If

                                                    End If

                                                End If

                                            End If
                                            '**************************

                                            If ReportStatus = 0 Then 'Landscape

                                                If ObjCompMonitorServiceStatusPeriod.PeriodID = 1 Then

                                                    Freq1 = ObjCompMonitorServiceStatusPeriod.FrequencyValue

                                                    If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Or
                                                       (ObjCompMonitorServiceStatus.IsApplicable = False And ObjCompMonitorServiceStatus.IsCompleted = True) Then

                                                        ElapsedTime = ""
                                                        RemainingTime = ""
                                                        DueAsof = ""
                                                        'Added By Prashant 03-Aug-2009
                                                        SinceNew2 = ""
                                                        '----------------------------
                                                        TimeSinceNew = ""
                                                        DiffCompInstDoneOnValue = ""

                                                    Else

                                                        'Added By Prashant 03-Aug-2009
                                                        If ObjCompMonitorServiceStatus.MonitorTypeID = 3 And ObjCompMonitorServiceStatusPeriod.DoneOnValue = "" Then
                                                            SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted
                                                        Else
                                                            SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DiffCompCurrentDoneOnValueFormatted
                                                        End If
                                                        '----------------------------

                                                        ElapsedTime = ObjCompMonitorServiceStatusPeriod.AllElapsedValue
                                                        RemainingTime = ObjCompMonitorServiceStatusPeriod.RemainingValue

                                                        If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                            DueAsof = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                        Else
                                                            DueAsof = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                        End If
                                                        '----------------
                                                        TimeSinceNew = TimeSinceNew & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted

                                                    End If

                                                    Extension = ObjCompMonitorServiceStatusPeriod.ExtensionValue
                                                    DiffCompInstDoneOnValue = ObjCompMonitorServiceStatusPeriod.DiffCompInstDoneOnValue

                                                End If

                                                If ObjCompMonitorServiceStatusPeriod.PeriodID = 2 Then

                                                    Freq2 = ObjCompMonitorServiceStatusPeriod.FrequencyValueFormatted

                                                    If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Or
                                                       (ObjCompMonitorServiceStatus.IsApplicable = False And ObjCompMonitorServiceStatus.IsCompleted = True) Then

                                                        ElapsedTime1 = ""
                                                        RemainingTime1 = ""
                                                        DueAsof1 = ""

                                                    Else

                                                        ElapsedTime1 = ObjCompMonitorServiceStatusPeriod.AllElapsedValueFormatted
                                                        RemainingTime1 = ObjCompMonitorServiceStatusPeriod.RemainingValueFormatted

                                                        If (AppSettings("ClientCode") = "BA" Or
                                                            AppSettings("ClientCode") = "PAS" Or
                                                            AppSettings("ClientCode") = "Novo" Or
                                                            AppSettings("ClientCode") = "YA" Or
                                                            AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013

                                                            DueAsof1 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                        Else
                                                            DueAsof1 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                        End If
                                                        '-------------

                                                    End If

                                                    Extension1 = ObjCompMonitorServiceStatusPeriod.ExtensionValueFormatted
                                                    DiffCompInstDoneOnValue = ObjCompMonitorServiceStatusPeriod.DiffCompInstDoneOnValue

                                                End If

                                                If ObjCompMonitorServiceStatusPeriod.PeriodID = 3 Or
                                                   ObjCompMonitorServiceStatusPeriod.PeriodID = 4 Or
                                                   ObjCompMonitorServiceStatusPeriod.PeriodID = 5 Or
                                                   ObjCompMonitorServiceStatusPeriod.PeriodID = 6 Or
                                                   ObjCompMonitorServiceStatusPeriod.PeriodID = 7 Or
                                                   ObjCompMonitorServiceStatusPeriod.PeriodID = 8 Or
                                                   ObjCompMonitorServiceStatusPeriod.PeriodID = 12 Or
                                                   ObjCompMonitorServiceStatusPeriod.PeriodID = 13 Or
                                                   ObjCompMonitorServiceStatusPeriod.PeriodID = 14 Then

                                                    If Freq3 = "" Then

                                                        Freq3 = ObjCompMonitorServiceStatusPeriod.FrequencyValue

                                                        If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Or
                                                           (ObjCompMonitorServiceStatus.IsApplicable = False And ObjCompMonitorServiceStatus.IsCompleted = True) Then

                                                            ElapsedTime2 = ""
                                                            RemainingTime2 = ""
                                                            DueAsof2 = ""
                                                            AssemblyDueAsof2 = ""
                                                            SinceNew2 = ""
                                                            TimeSinceNew = ""
                                                        Else

                                                            If (ObjCompMonitorServiceStatus.MonitorTypeID = 4) Then

                                                                ElapsedTime2 = ObjCompMonitorServiceStatusPeriod.AllElapsedValue
                                                                RemainingTime2 = ObjCompMonitorServiceStatusPeriod.RemainingValue

                                                                If (AppSettings("ClientCode") = "BA" Or
                                                                    AppSettings("ClientCode") = "PAS" Or
                                                                    AppSettings("ClientCode") = "Novo" Or
                                                                    AppSettings("ClientCode") = "YA" Or
                                                                    AppSettings("ClientCode") = "TA" Or
                                                                    AppSettings("ClientCode") = "STR") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013

                                                                    DueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                                Else
                                                                    DueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                                End If

                                                                '------------
                                                                AssemblyDueAsof2 = ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                                SinceNew2 = ObjCompMonitorServiceStatusPeriod.DiffCompCurrentDoneOnValue
                                                                TimeSinceNew = ObjCompMonitorServiceStatusPeriod.CompCurrentValue

                                                            Else

                                                                ElapsedTime2 = ObjCompMonitorServiceStatusPeriod.AllElapsedValue
                                                                RemainingTime2 = ObjCompMonitorServiceStatusPeriod.RemainingValue

                                                                If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Or AppSettings("ClientCode") = "STR") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                                    DueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                                Else
                                                                    DueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                                End If
                                                                '-------------

                                                                AssemblyDueAsof2 = ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted

                                                                If ObjCompMonitorServiceStatus.MonitorTypeID = 3 And ObjCompMonitorServiceStatusPeriod.DoneOnValue = "" Then
                                                                    SinceNew2 = ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted
                                                                Else
                                                                    SinceNew2 = ObjCompMonitorServiceStatusPeriod.DiffCompCurrentDoneOnValueFormatted
                                                                End If

                                                                TimeSinceNew = ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted

                                                            End If

                                                        End If
                                                        Extension2 = ObjCompMonitorServiceStatusPeriod.ExtensionValue

                                                        If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = False) Then
                                                            DoneAt2 = ""
                                                        Else
                                                            DoneAt2 = ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                        End If

                                                        DiffCompInstDoneOnValue = ObjCompMonitorServiceStatusPeriod.DiffCompInstDoneOnValue

                                                    Else

                                                        Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.FrequencyValue

                                                        If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Or
                                                           (ObjCompMonitorServiceStatus.IsApplicable = False And ObjCompMonitorServiceStatus.IsCompleted = True) Then

                                                            ElapsedTime2 = ""
                                                            RemainingTime2 = ""
                                                            DueAsof2 = ""
                                                            AssemblyDueAsof2 = ""
                                                            SinceNew2 = ""
                                                            TimeSinceNew = ""

                                                        Else

                                                            If (ObjCompMonitorServiceStatus.MonitorTypeID = 4) Then

                                                                'ElapsedTime2 = "" 'Commneted By Prashant 29-July-2009 Because we have to show elapsed values for "Expiry" status also
                                                                ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AllElapsedValue
                                                                RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.RemainingValue

                                                                If (AppSettings("ClientCode") = "BA" Or
                                                                    AppSettings("ClientCode") = "PAS" Or
                                                                    AppSettings("ClientCode") = "Novo" Or
                                                                    AppSettings("ClientCode") = "YA" Or
                                                                    AppSettings("ClientCode") = "TA" Or
                                                                    AppSettings("ClientCode") = "STR") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013

                                                                    DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                                Else
                                                                    DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                                End If
                                                                '-----------------------------

                                                                AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                                SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DiffCompCurrentDoneOnValueFormatted
                                                                TimeSinceNew = TimeSinceNew & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted
                                                                DiffCompInstDoneOnValue = ObjCompMonitorServiceStatusPeriod.DiffCompInstDoneOnValue

                                                            Else

                                                                ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AllElapsedValue
                                                                RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.RemainingValue

                                                                If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Or AppSettings("ClientCode") = "STR") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                                    DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                                Else
                                                                    DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                                End If
                                                                '--------------
                                                                AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted

                                                                If ObjCompMonitorServiceStatus.MonitorTypeID = 3 And ObjCompMonitorServiceStatusPeriod.DoneOnValue = "" Then
                                                                    SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted
                                                                Else
                                                                    SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DiffCompCurrentDoneOnValueFormatted
                                                                End If

                                                                TimeSinceNew = TimeSinceNew & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted

                                                            End If

                                                        End If

                                                        Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ExtensionValue

                                                        If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = False) Then
                                                            DoneAt2 = ""
                                                        Else
                                                            DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                        End If

                                                        DiffCompInstDoneOnValue = DiffCompInstDoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DiffCompInstDoneOnValue

                                                    End If

                                                End If

                                            Else

                                                If ObjCompMonitorServiceStatusPeriod.PeriodID = 2 Then     'StartDate

                                                    If Freq3 = "" Then

                                                        Freq3 = ObjCompMonitorServiceStatusPeriod.FrequencyValueFormatted

                                                        If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Or
                                                           (ObjCompMonitorServiceStatus.IsApplicable = False And ObjCompMonitorServiceStatus.IsCompleted = True) Then

                                                            ElapsedTime2 = ""
                                                            RemainingTime2 = ""
                                                            DueAsof2 = ""
                                                            AssemblyDueAsof2 = ""

                                                        Else

                                                            If (ObjCompMonitorServiceStatus.MonitorTypeID = 4) Then

                                                                ElapsedTime2 = ObjCompMonitorServiceStatusPeriod.AllElapsedValueFormatted
                                                                RemainingTime2 = ObjCompMonitorServiceStatusPeriod.RemainingValueFormatted

                                                                If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Or AppSettings("ClientCode") = "STR") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                                    DueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                                Else
                                                                    DueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                                End If
                                                                '--------------
                                                                AssemblyDueAsof2 = ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted

                                                            Else

                                                                ElapsedTime2 = ObjCompMonitorServiceStatusPeriod.AllElapsedValueFormatted
                                                                RemainingTime2 = ObjCompMonitorServiceStatusPeriod.RemainingValueFormatted

                                                                If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Or AppSettings("ClientCode") = "STR") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                                    DueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                                Else
                                                                    DueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                                End If
                                                                '------------------
                                                                AssemblyDueAsof2 = ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted

                                                            End If

                                                        End If

                                                        Extension2 = ObjCompMonitorServiceStatusPeriod.ExtensionValueFormatted

                                                        If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = False) Then
                                                            DoneAt2 = ""
                                                        Else
                                                            DoneAt2 = ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                        End If

                                                        DiffCompInstDoneOnValue = ObjCompMonitorServiceStatusPeriod.DiffCompInstDoneOnValue

                                                    Else

                                                        Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.FrequencyValueFormatted

                                                        If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Or
                                                           (ObjCompMonitorServiceStatus.IsApplicable = False And ObjCompMonitorServiceStatus.IsCompleted = True) Then

                                                            ElapsedTime2 = ""
                                                            RemainingTime2 = ""
                                                            DueAsof2 = ""
                                                            AssemblyDueAsof2 = ""

                                                        Else

                                                            If (ObjCompMonitorServiceStatus.MonitorTypeID = 4) Then

                                                                ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AllElapsedValueFormatted
                                                                RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.RemainingValueFormatted

                                                                If (AppSettings("ClientCode") = "BA" Or
                                                                    AppSettings("ClientCode") = "PAS" Or
                                                                    AppSettings("ClientCode") = "Novo" Or
                                                                    AppSettings("ClientCode") = "YA" Or
                                                                    AppSettings("ClientCode") = "TA" Or
                                                                    AppSettings("ClientCode") = "STR") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013

                                                                    DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                                Else
                                                                    DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                                End If
                                                                '-------------
                                                                AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted

                                                            Else

                                                                ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AllElapsedValueFormatted
                                                                RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.RemainingValueFormatted

                                                                If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Or AppSettings("ClientCode") = "STR") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                                    DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                                Else
                                                                    DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                                End If
                                                                '---------------
                                                                AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted

                                                            End If

                                                        End If

                                                        Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ExtensionValueFormatted

                                                        If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = False) Then
                                                            DoneAt2 = ""
                                                        Else
                                                            DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                        End If
                                                        DiffCompInstDoneOnValue = DiffCompInstDoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DiffCompInstDoneOnValue

                                                    End If

                                                Else              'PeriodID <> 2

                                                    If Freq3 = "" Then

                                                        Freq3 = ObjCompMonitorServiceStatusPeriod.FrequencyValue

                                                        If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Or
                                                           (ObjCompMonitorServiceStatus.IsApplicable = False And ObjCompMonitorServiceStatus.IsCompleted = True) Then

                                                            ElapsedTime2 = ""
                                                            RemainingTime2 = ""
                                                            DueAsof2 = ""
                                                            AssemblyDueAsof2 = ""
                                                            SinceNew2 = ""
                                                            DoneAt2 = ""
                                                            TimeSinceNew = ""

                                                        Else

                                                            If (ObjCompMonitorServiceStatus.MonitorTypeID = 4) Then

                                                                ElapsedTime2 = ObjCompMonitorServiceStatusPeriod.AllElapsedValue
                                                                RemainingTime2 = ObjCompMonitorServiceStatusPeriod.RemainingValue

                                                                If ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Then

                                                                    DueAsof2 = ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                                    AssemblyDueAsof2 = ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                                    SinceNew2 = ObjCompMonitorServiceStatusPeriod.DiffCompCurrentDoneOnValueFormatted
                                                                    TimeSinceNew = ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted

                                                                Else

                                                                    If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Or AppSettings("ClientCode") = "STR") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                                        DueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                                    Else
                                                                        DueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                                    End If
                                                                    '--------------
                                                                    AssemblyDueAsof2 = ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                                    SinceNew2 = ObjCompMonitorServiceStatusPeriod.DiffCompCurrentDoneOnValueFormatted
                                                                    TimeSinceNew = ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted

                                                                End If

                                                            Else

                                                                ElapsedTime2 = ObjCompMonitorServiceStatusPeriod.AllElapsedValue
                                                                RemainingTime2 = ObjCompMonitorServiceStatusPeriod.RemainingValue

                                                                If ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Then

                                                                    DueAsof2 = ObjCompMonitorServiceStatusPeriod.DueOnValue
                                                                    AssemblyDueAsof2 = ObjCompMonitorServiceStatusPeriod.DueOnValue

                                                                    If ObjCompMonitorServiceStatus.MonitorTypeID = 3 And ObjCompMonitorServiceStatusPeriod.DoneOnValue = "" Then
                                                                        SinceNew2 = ObjCompMonitorServiceStatusPeriod.CompCurrentValue
                                                                    Else
                                                                        SinceNew2 = ObjCompMonitorServiceStatusPeriod.DiffCompCurrentDoneOnValue
                                                                    End If

                                                                    TimeSinceNew = ObjCompMonitorServiceStatusPeriod.CompCurrentValue

                                                                Else

                                                                    If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Or AppSettings("ClientCode") = "STR") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                                        DueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame '16-04-2009
                                                                    Else
                                                                        DueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                                    End If
                                                                    '--------------
                                                                    AssemblyDueAsof2 = ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted

                                                                    If ObjCompMonitorServiceStatus.MonitorTypeID = 3 And ObjCompMonitorServiceStatusPeriod.DoneOnValue = "" Then
                                                                        SinceNew2 = ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted
                                                                    Else
                                                                        SinceNew2 = ObjCompMonitorServiceStatusPeriod.DiffCompCurrentDoneOnValueFormatted
                                                                    End If

                                                                    TimeSinceNew = ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted

                                                                End If

                                                            End If

                                                        End If

                                                        Extension2 = ObjCompMonitorServiceStatusPeriod.ExtensionValue

                                                        If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = False) Then
                                                            DoneAt2 = ""
                                                        Else
                                                            DoneAt2 = ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                        End If

                                                        DiffCompInstDoneOnValue = ObjCompMonitorServiceStatusPeriod.DiffCompInstDoneOnValue

                                                    Else

                                                        Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.FrequencyValue

                                                        If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Or
                                                           (ObjCompMonitorServiceStatus.IsApplicable = False And ObjCompMonitorServiceStatus.IsCompleted = True) Then

                                                            ElapsedTime2 = ""
                                                            RemainingTime2 = ""
                                                            DueAsof2 = ""
                                                            AssemblyDueAsof2 = ""
                                                            SinceNew2 = ""
                                                            TimeSinceNew = ""

                                                        Else

                                                            If (ObjCompMonitorServiceStatus.MonitorTypeID = 4) Then

                                                                ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedValue
                                                                RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.RemainingValue

                                                                If ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Then

                                                                    DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                                    AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                                    SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DiffCompCurrentDoneOnValueFormatted
                                                                    TimeSinceNew = TimeSinceNew & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted

                                                                Else

                                                                    If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Or AppSettings("ClientCode") = "STR") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                                        DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                                    Else
                                                                        DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                                    End If
                                                                    '--------------
                                                                    AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                                    SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DiffCompCurrentDoneOnValueFormatted
                                                                    TimeSinceNew = TimeSinceNew & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted

                                                                End If

                                                            Else

                                                                ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedValue
                                                                RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.RemainingValue

                                                                If ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Then

                                                                    DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                                    AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted

                                                                    If ObjCompMonitorServiceStatus.MonitorTypeID = 3 And ObjCompMonitorServiceStatusPeriod.DoneOnValue = "" Then
                                                                        SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted
                                                                    Else
                                                                        SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DiffCompCurrentDoneOnValueFormatted
                                                                    End If

                                                                    TimeSinceNew = TimeSinceNew & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted

                                                                Else

                                                                    If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Or AppSettings("ClientCode") = "STR") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                                        DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                                    Else
                                                                        DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                                    End If
                                                                    '--------------
                                                                    AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted

                                                                    If ObjCompMonitorServiceStatus.MonitorTypeID = 3 And ObjCompMonitorServiceStatusPeriod.DoneOnValue = "" Then
                                                                        SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted
                                                                    Else
                                                                        SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DiffCompCurrentDoneOnValueFormatted
                                                                    End If

                                                                    TimeSinceNew = TimeSinceNew & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted

                                                                End If

                                                            End If

                                                        End If

                                                        Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ExtensionValue

                                                        If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = False) Then
                                                            DoneAt2 = ""
                                                        Else
                                                            DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                        End If

                                                        DiffCompInstDoneOnValue = DiffCompInstDoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DiffCompInstDoneOnValue

                                                    End If

                                                End If

                                            End If

                                        Next

                                        AssemblyID = ObjAssemblyStatus.AssemblyID
                                        Note = ObjCompMonitorServiceStatus.Notes
                                        Remark = ObjCompMonitorServiceStatus.DoneRemark
                                        ExtensionDate = ObjCompMonitorServiceStatus.ExtensionDate
                                        ApprovalRemark = ObjCompMonitorServiceStatus.ApprovalRemark
                                        Reference = ObjCompMonitorServiceStatus.Reference  'Added By Prashant 3-Apr-2013  'Indamer03042013
                                        Dim ATACode = ObjCompMonitorServiceStatus.ATACode


                                        'Added By Vikrant On 22-Dec-2020 For Star Air
                                        mnWOListForDueJobs = nWOListForDueJobs.GetWOListForDueJobs(ObjCompMonitorServiceStatus.ID)
                                        If mnWOListForDueJobs.Count > 0 Then
                                            nWONumber = mnWOListForDueJobs(0).WONumber
                                        Else
                                            nWONumber = ""
                                        End If
                                        'End

                                        If IsExcel Then

                                            If ATACode.ToString.Length < 3 Then
                                                ATAChapter = ATACode.ToString.PadLeft(3, "0"c) + " " + "-" + " " + ObjCompMonitorServiceStatus.ATANomenclature
                                            End If

                                            If Freq1 <> "" Then
                                                Freq1 = Freq1 + IIf(Freq2 <> "", Chr(10) + Freq2, "") + IIf(Freq3 <> "", Chr(10) + Freq3, "")
                                            Else
                                                Freq1 = Freq2 + IIf(Freq3 <> "", Chr(10) + Freq3, "")
                                            End If

                                            If DueAsof <> "" Then
                                                DueAsof = DueAsof + IIf(DueAsof1 <> "", Chr(10) + DueAsof1, "") + IIf(DueAsof2 <> "", Chr(10) + DueAsof2, "")
                                            Else
                                                DueAsof = DueAsof1 + IIf(DueAsof2 <> "", Chr(10) + DueAsof2, "")
                                            End If

                                            If ElapsedTime <> "" Then
                                                ElapsedTime = ElapsedTime + IIf(ElapsedTime1 <> "", Chr(10) + ElapsedTime1, "") + IIf(ElapsedTime2 <> "", Chr(10) + ElapsedTime2, "")
                                            Else
                                                ElapsedTime = ElapsedTime1 + IIf(ElapsedTime2 <> "", Chr(10) + ElapsedTime2, "")
                                            End If

                                            If RemainingTime <> "" Then
                                                RemainingTime = RemainingTime + IIf(RemainingTime1 <> "", Chr(10) + RemainingTime1, "") + IIf(RemainingTime2 <> "", Chr(10) + RemainingTime2, "")
                                            Else
                                                RemainingTime = RemainingTime1 + IIf(RemainingTime2 <> "", Chr(10) + RemainingTime2, "")
                                            End If

                                        End If

                                        If ObjCompMonitorServiceStatus.IsApplicable = False Then
                                            DueAsof = ""
                                            DueAsof1 = ""
                                            DueAsof2 = ""
                                            RemainingTime = ""
                                            RemainingTime1 = ""
                                            RemainingTime2 = ""
                                        End If

                                        ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , ,
                                                                                                     AssemblySerialNo,
                                                                                                     ATAChapter,
                                                                                                     PartNo,
                                                                                                     CompSerialNo,
                                                                                                     Position,
                                                                                                     MonitorType,
                                                                                                     MonitorTypeCode,
                                                                                                     Note,
                                                                                                     Remark,
                                                                                                     Description,,
                                                                                                     EstimatedDate, , ,
                                                                                                     Freq1,
                                                                                                     Freq2,
                                                                                                     Freq3,
                                                                                                     ElapsedTime,
                                                                                                     ElapsedTime1,
                                                                                                     ElapsedTime2,
                                                                                                     RemainingTime,
                                                                                                     RemainingTime1,
                                                                                                     RemainingTime2,
                                                                                                     DueAsof,
                                                                                                     DueAsof1,
                                                                                                     DueAsof2,
                                                                                                     AssemblyModel, , ,
                                                                                                     SinceNew2, , ,
                                                                                                     DoneAt2, , , ,
                                                                                                     ObjCompMonitorServiceStatus.ATACode, , , ,
                                                                                                     StartDateData, , , , , , , , , ,
                                                                                                     Reference, ,
                                                                                                     DoneOnDate, , , , , ,
                                                                                                     AssemblyDueAsof2,
                                                                                                     Extension,
                                                                                                     Extension1,
                                                                                                     Extension2,
                                                                                                     ExtensionDate,
                                                                                                     ApprovalRemark, , ,
                                                                                                     Code, , ,
                                                                                                     SupersededByADNumber:=TaskNo, ,
                                                                                                     ObjCompMonitorServiceStatus.IsApplicable, ,
                                                                                                     ObjCompMonitorServiceStatus.MonitorTypeID, , , , , , ,
                                                                                                     TimeSinceNew,
                                                                                                     WONumber:=nWONumber,
                                                                                                     DoneONValueForAssembly:=DoneONValueForAssembly,
                                                                                                     SourceDoc:=SourceDoc,
                                                                                                     DiffCompInstDoneOnValue:=DiffCompInstDoneOnValue,
                                                                                                     Zone:=ObjCompMonitorServiceStatus.Zone,
                                                                                                     Area:=ObjCompMonitorServiceStatus.Area,
                                                                                                     TaskNo:=TaskNo))

                                    End If

                                Next

                            Next

                        End If

                    Next

                Next

            End If

            If IsInsSelect = True Then

                For Each ObjMachine In mMachineList

                    For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList

                        If chkAssembly.Checked Then

                            For Each ObjAssemblyMonitorInspStatus In ObjAssemblyStatus.AssemblyMonitorInspStatusList

                                ''If (ObjAssemblyMonitorInspStatus.IsApplicable = True) Or
                                ''   (ObjAssemblyMonitorInspStatus.IsApplicable = False And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or
                                ''   (cmbFormat.SelectedValue = "2" AndAlso
                                ''   mLinkMaintenanceList.Contains(ObjAssemblyMonitorInspStatus.ModelMonitorInspID, "")) Then
                                If (ObjAssemblyMonitorInspStatus.IsApplicable = True And chkNotApplicable.Checked = False) Or
                                   (chkNotApplicable.Checked = True) Or
                                   (cmbFormat.SelectedValue = "2" AndAlso
                                   mLinkMaintenanceList.Contains(ObjAssemblyMonitorInspStatus.ModelMonitorInspID, "")) Then


                                    ATAChapter = ObjAssemblyMonitorInspStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorInspStatus.ATANomenclature
                                    Description = ObjAssemblyMonitorInspStatus.Description
                                    Position = ObjAssemblyStatus.Position
                                    MonitorTypeCode = ObjAssemblyMonitorInspStatus.Code
                                    MonitorType = ObjAssemblyMonitorInspStatus.Type
                                    AssemblyModel = ObjAssemblyStatus.Model
                                    AssemblySerialNo = ObjAssemblyStatus.SerialNo
                                    DoneOnDate = ObjAssemblyMonitorInspStatus.DoneOn
                                    'Added By Utkarsh ON 12-Sep-2013 FOR BA11092013
                                    EstimatedDate = ObjAssemblyMonitorInspStatus.EstimatedDateFormatted
                                    Code = ObjAssemblyMonitorInspStatus.ModelMonitorInspCode
                                    'end
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
                                    Extension = ""
                                    Extension1 = ""
                                    Extension2 = ""
                                    SinceNew2 = ""
                                    DoneAt2 = ""
                                    StartDateData = ""
                                    TimeSinceNew = ""
                                    Periodcount = ObjAssemblyStatus.AssemblyStatusPeriodList.Count()
                                    StatusMasterID = ObjAssemblyMonitorInspStatus.ModelMonitorInspID  'vikrant
                                    RecordOwnID = ObjAssemblyMonitorInspStatus.ID.ToString
                                    Dim IsPeriod2Exists As Boolean = False

                                    'Added by Saylee on 19-Sep-2014 for ALL19092014
                                    Dim mDoneONValueForNoPeriod As Period = New Period(1, DBNull.Value)
                                    Dim mTSOValueForNoPeriod As Period = New Period(1, DBNull.Value)
                                    DoneONValueForNoPeriod = String.Empty
                                    TSOValueForNoPeriod = String.Empty
                                    Dim tmpCurrentValue As Decimal = 0
                                    Dim mPeriodID As Integer = 0

                                    Dim mDoneONValueForAssembly As Period = New Period(1, DBNull.Value)
                                    DoneONValueForAssembly = String.Empty

                                    DiffCompInstDoneOnValue = ""

                                    For Count = 0 To Periodcount - 1

                                        'Added By Saylee on 22-Jun-2011
                                        If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 2 Then
                                            If ObjAssemblyMonitorInspStatus.DoneOn = "" Then
                                                For Each tmpObjAssemblyMonitorInspStatusPeriod As AssemblyMonitorInspStatusPeriodInfo In ObjAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriodList
                                                    If tmpObjAssemblyMonitorInspStatusPeriod.PeriodID = 2 Then
                                                        IsPeriod2Exists = True
                                                        Exit For
                                                    End If
                                                Next
                                                If IsPeriod2Exists = True Then
                                                    For Each ObjAssemblyMonitorInspStatusPeriod In ObjAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriodList
                                                        If ObjAssemblyMonitorInspStatusPeriod.PeriodID = 2 Then
                                                            StartDateLabel = CType(IIf(StartDateLabel = "", StartDateLabel, StartDateLabel + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                                                            StartDateData = CType(IIf(StartDateData = "", StartDateData, StartDateData + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted
                                                        End If
                                                    Next
                                                Else
                                                    StartDateLabel = CType(IIf(StartDateLabel = "", StartDateLabel, StartDateLabel + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                                                    StartDateData = CType(IIf(StartDateData = "", StartDateData, StartDateData + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyStartValueFormatted
                                                End If

                                            Else
                                                StartDateLabel = CType(IIf(StartDateLabel = "", StartDateLabel, StartDateLabel + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                                                StartDateData = CType(IIf(StartDateData = "", StartDateData, StartDateData + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjAssemblyMonitorInspStatus.DoneOnFormatted
                                            End If
                                        End If

                                        'Added by Saylee on 19-Sep-2014 for ALL19092014
                                        'If No Cycle/Hour Period Present in Monitor Insp       
                                        mDoneONValueForNoPeriod = New Period(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, DBNull.Value)
                                        mTSOValueForNoPeriod = New Period(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, DBNull.Value)

                                        If (ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 1 Or ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 3) And Not (ObjAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriodList.Contains(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID)) Then

                                            Dim mPeriodUnitID As Integer = 0
                                            If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 1 Then
                                                mPeriodUnitID = 1
                                            ElseIf ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 3 Then
                                                mPeriodUnitID = 6
                                            End If
                                            mPeriodID = ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID

                                            If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = False) Then
                                                DoneONValueForNoPeriod = ""
                                            Else
                                                If ObjAssemblyMonitorInspStatus.DoneOn <> "" Then
                                                    'If CurrentValue.GetCurrentValue(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).ID, ObjAssemblyMonitorInspStatus.DoneOn, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID).Count > 0 Then
                                                    Dim mMachineMaintenance As MachineMaintenance = MachineMaintenance.GetMachineMaintenance(ObjAssemblyMonitorInspStatus.ID, MachineMaintenanceActivity.AssemblyInspection)
                                                    If CurrentValueOnAsOnDate.GetCurrentValue(mMachineMaintenance.LogAssemblyStatusID, ObjAssemblyMonitorInspStatus.DoneOn, mMachineMaintenance.LogNo, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID)(0).CurrentValueDec > 0 Then

                                                        mDoneONValueForNoPeriod.Value = CurrentValueOnAsOnDate.GetCurrentValue(mMachineMaintenance.LogAssemblyStatusID, ObjAssemblyMonitorInspStatus.DoneOn, mMachineMaintenance.LogNo, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID)(0).CurrentValue
                                                        If DoneONValueForNoPeriod = "" Then
                                                            DoneONValueForNoPeriod = mDoneONValueForNoPeriod.TextFormatted
                                                        Else
                                                            DoneONValueForNoPeriod = DoneONValueForNoPeriod + IIf(IsExcel, Chr(10), vbCrLf) + mDoneONValueForNoPeriod.TextFormatted
                                                        End If

                                                    End If
                                                End If
                                            End If
                                        End If
                                    Next

                                    For Each ObjAssemblyMonitorInspStatusPeriod In ObjAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriodList

                                        If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = False) Then
                                            DoneONValueForAssembly = ""
                                        Else

                                            If ObjAssemblyMonitorInspStatus.DoneOn <> "" Then

                                                mDoneONValueForAssembly = New Period(ObjAssemblyMonitorInspStatusPeriod.PeriodID, DBNull.Value)

                                                If ObjAssemblyMonitorInspStatusPeriod.PeriodID <> 2 Then

                                                    Dim mMachineMaintenance As MachineMaintenance =
                                                        MachineMaintenance.GetMachineMaintenance(ObjAssemblyMonitorInspStatus.ID,
                                                                                                 MachineMaintenanceActivity.AssemblyInspection)

                                                    If CurrentValueOnAsOnDate.GetCurrentValue(mMachineMaintenance.LogAssemblyStatusID,
                                                                                              ObjAssemblyMonitorInspStatus.DoneOn,
                                                                                              mMachineMaintenance.LogNo,
                                                                                              ObjAssemblyMonitorInspStatusPeriod.PeriodID)(0).CurrentValueDec > 0 Then

                                                        mDoneONValueForAssembly.Value = CurrentValueOnAsOnDate.GetCurrentValue(mMachineMaintenance.LogAssemblyStatusID, ObjAssemblyMonitorInspStatus.DoneOn, mMachineMaintenance.LogNo, ObjAssemblyMonitorInspStatusPeriod.PeriodID)(0).CurrentValue

                                                        If DoneONValueForAssembly = "" Then
                                                            DoneONValueForAssembly = mDoneONValueForAssembly.TextFormatted
                                                        Else
                                                            DoneONValueForAssembly = DoneONValueForAssembly + IIf(IsExcel, Chr(10), vbCrLf) + mDoneONValueForAssembly.TextFormatted
                                                        End If

                                                    End If

                                                Else

                                                    If DoneONValueForAssembly = "" Then
                                                        DoneONValueForAssembly = ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted
                                                    Else
                                                        DoneONValueForAssembly = DoneONValueForAssembly + IIf(IsExcel, Chr(10), vbCrLf) + ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted
                                                    End If

                                                End If

                                            End If

                                        End If
                                        '**************************

                                        If ReportStatus = 0 Then 'Landscape

                                            If ObjAssemblyMonitorInspStatusPeriod.PeriodID = 1 Then

                                                Freq1 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValue

                                                If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or
                                                   (ObjAssemblyMonitorInspStatus.IsApplicable = False And ObjAssemblyMonitorInspStatus.IsCompleted = True) Then

                                                    ElapsedTime = ""
                                                    RemainingTime = ""
                                                    DueAsof = ""
                                                    'Added By Prashant 04-Aug-2009
                                                    SinceNew2 = ""
                                                    '----------------------------
                                                    TimeSinceNew = ""

                                                Else

                                                    ElapsedTime = ObjAssemblyMonitorInspStatusPeriod.ElapsedValue
                                                    RemainingTime = ObjAssemblyMonitorInspStatusPeriod.RemainingValue
                                                    'Added By Prashant 17-Sep-2013

                                                    If (AppSettings("ClientCode") = "BA" Or
                                                        AppSettings("ClientCode") = "PAS" Or
                                                        AppSettings("ClientCode") = "Novo" Or
                                                        AppSettings("ClientCode") = "YA" Or
                                                        AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013

                                                        DueAsof = ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                    Else
                                                        DueAsof = ObjAssemblyMonitorInspStatusPeriod.DueOnValue
                                                    End If

                                                    'Added By Prashant 04-Aug-2009
                                                    If ObjAssemblyMonitorInspStatus.MonitorTypeID = 3 And ObjAssemblyMonitorInspStatusPeriod.DoneOnValue = "" Then
                                                        SinceNew2 = ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue
                                                    Else
                                                        SinceNew2 = ObjAssemblyMonitorInspStatusPeriod.DiffAssemblyCurrentDoneOnValue
                                                    End If
                                                    '----------------------------
                                                    TimeSinceNew = ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue
                                                End If

                                                Extension = ObjAssemblyMonitorInspStatusPeriod.ExtensionValue

                                            End If

                                            If ObjAssemblyMonitorInspStatusPeriod.PeriodID = 2 Then

                                                Freq2 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValueFormatted

                                                If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or
                                                   (ObjAssemblyMonitorInspStatus.IsApplicable = False And ObjAssemblyMonitorInspStatus.IsCompleted = True) Then

                                                    ElapsedTime1 = ""
                                                    RemainingTime1 = ""
                                                    DueAsof1 = ""

                                                Else

                                                    ElapsedTime1 = ObjAssemblyMonitorInspStatusPeriod.ElapsedValueFormatted
                                                    RemainingTime1 = ObjAssemblyMonitorInspStatusPeriod.RemainingValueFormatted
                                                    DueAsof1 = ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted

                                                End If

                                                Extension1 = ObjAssemblyMonitorInspStatusPeriod.ExtensionValueFormatted

                                            End If

                                            If ObjAssemblyMonitorInspStatusPeriod.PeriodID = 3 Or
                                               ObjAssemblyMonitorInspStatusPeriod.PeriodID = 4 Or
                                               ObjAssemblyMonitorInspStatusPeriod.PeriodID = 5 Or
                                               ObjAssemblyMonitorInspStatusPeriod.PeriodID = 6 Or
                                               ObjAssemblyMonitorInspStatusPeriod.PeriodID = 7 Or
                                               ObjAssemblyMonitorInspStatusPeriod.PeriodID = 8 Or
                                               ObjAssemblyMonitorInspStatusPeriod.PeriodID = 12 Or
                                               ObjAssemblyMonitorInspStatusPeriod.PeriodID = 13 Or
                                               ObjAssemblyMonitorInspStatusPeriod.PeriodID = 14 Then

                                                If Freq3 = "" Then

                                                    Freq3 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValue

                                                    If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or
                                                       (ObjAssemblyMonitorInspStatus.IsApplicable = False And ObjAssemblyMonitorInspStatus.IsCompleted = True) Then

                                                        ElapsedTime2 = ""
                                                        RemainingTime2 = ""
                                                        DueAsof2 = ""
                                                        AssemblyDueAsof2 = ""
                                                        SinceNew2 = ""
                                                        TimeSinceNew = ""

                                                    Else

                                                        ElapsedTime2 = ObjAssemblyMonitorInspStatusPeriod.ElapsedValue
                                                        RemainingTime2 = ObjAssemblyMonitorInspStatusPeriod.RemainingValue

                                                        If (AppSettings("ClientCode") = "BA" Or
                                                            AppSettings("ClientCode") = "PAS" Or
                                                            AppSettings("ClientCode") = "Novo" Or
                                                            AppSettings("ClientCode") = "YA" Or
                                                            AppSettings("ClientCode") = "TA" Or
                                                            AppSettings("ClientCode") = "STR") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013

                                                            DueAsof2 = ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                        Else
                                                            DueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValue
                                                        End If

                                                        AssemblyDueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValue

                                                        If ObjAssemblyMonitorInspStatus.MonitorTypeID = 3 And ObjAssemblyMonitorInspStatusPeriod.DoneOnValue = "" Then
                                                            SinceNew2 = ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue
                                                        Else
                                                            SinceNew2 = ObjAssemblyMonitorInspStatusPeriod.DiffAssemblyCurrentDoneOnValue
                                                        End If

                                                        TimeSinceNew = ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue

                                                    End If

                                                    Extension2 = ObjAssemblyMonitorInspStatusPeriod.ExtensionValue

                                                    If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = False) Then
                                                        DoneAt2 = ""
                                                    Else
                                                        DoneAt2 = ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
                                                    End If

                                                Else

                                                    Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.FrequencyValue

                                                    If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or
                                                       (ObjAssemblyMonitorInspStatus.IsApplicable = False And ObjAssemblyMonitorInspStatus.IsCompleted = True) Then

                                                        ElapsedTime2 = ""
                                                        RemainingTime2 = ""
                                                        DueAsof2 = ""
                                                        AssemblyDueAsof2 = ""
                                                        SinceNew2 = ""
                                                        TimeSinceNew = ""

                                                    Else

                                                        ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.ElapsedValue
                                                        RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.RemainingValue

                                                        'Added By Prashant 17-Sep-2013
                                                        If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Or AppSettings("ClientCode") = "STR") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                            DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                        Else
                                                            DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.DueOnValue
                                                        End If

                                                        AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.DueOnValue

                                                        If ObjAssemblyMonitorInspStatus.MonitorTypeID = 3 And ObjAssemblyMonitorInspStatusPeriod.DoneOnValue = "" Then
                                                            SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue
                                                        Else
                                                            SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.DiffAssemblyCurrentDoneOnValue
                                                        End If

                                                        TimeSinceNew = TimeSinceNew & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue

                                                    End If

                                                    Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.ExtensionValue

                                                    If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = False) Then
                                                        DoneAt2 = ""
                                                    Else
                                                        DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
                                                    End If

                                                End If

                                            End If

                                        Else

                                            If ObjAssemblyMonitorInspStatusPeriod.PeriodID = 2 Then        'StartDate

                                                If Freq3 = "" Then

                                                    Freq3 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValueFormatted

                                                    If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or
                                                       (ObjAssemblyMonitorInspStatus.IsApplicable = False And ObjAssemblyMonitorInspStatus.IsCompleted = True) Then

                                                        ElapsedTime2 = ""
                                                        RemainingTime2 = ""
                                                        DueAsof2 = ""
                                                        AssemblyDueAsof2 = ""

                                                    Else

                                                        ElapsedTime2 = ObjAssemblyMonitorInspStatusPeriod.ElapsedValueFormatted
                                                        RemainingTime2 = ObjAssemblyMonitorInspStatusPeriod.RemainingValueFormatted
                                                        DueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
                                                        AssemblyDueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted

                                                    End If

                                                    Extension2 = ObjAssemblyMonitorInspStatusPeriod.ExtensionValueFormatted

                                                    If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = False) Then
                                                        DoneAt2 = ""
                                                    Else
                                                        DoneAt2 = ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted
                                                    End If

                                                Else

                                                    Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.FrequencyValueFormatted

                                                    If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or
                                                       (ObjAssemblyMonitorInspStatus.IsApplicable = False And ObjAssemblyMonitorInspStatus.IsCompleted = True) Then

                                                        ElapsedTime2 = ""
                                                        RemainingTime2 = ""
                                                        DueAsof2 = ""
                                                        AssemblyDueAsof2 = ""

                                                    Else

                                                        ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.ElapsedValueFormatted
                                                        RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.RemainingValue
                                                        DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
                                                        AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted

                                                    End If

                                                    Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.ExtensionValueFormatted

                                                    If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = False) Then
                                                        DoneAt2 = ""
                                                    Else
                                                        DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted
                                                    End If

                                                End If

                                            Else                                                           'PeriodID <> 2

                                                If Freq3 = "" Then

                                                    Freq3 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValue

                                                    If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or
                                                       (ObjAssemblyMonitorInspStatus.IsApplicable = False And ObjAssemblyMonitorInspStatus.IsCompleted = True) Then

                                                        ElapsedTime2 = ""
                                                        RemainingTime2 = ""
                                                        DueAsof2 = ""
                                                        AssemblyDueAsof2 = ""
                                                        SinceNew2 = ""
                                                        TimeSinceNew = ""

                                                    Else

                                                        ElapsedTime2 = ObjAssemblyMonitorInspStatusPeriod.AllElapsedValue
                                                        RemainingTime2 = ObjAssemblyMonitorInspStatusPeriod.RemainingValue

                                                        'Added By Prashant 17-Sep-2013
                                                        If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Or AppSettings("ClientCode") = "STR") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                            DueAsof2 = ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                        Else
                                                            DueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValue
                                                        End If

                                                        AssemblyDueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValue

                                                        If ObjAssemblyMonitorInspStatus.MonitorTypeID = 3 And ObjAssemblyMonitorInspStatusPeriod.DoneOnValue = "" Then
                                                            SinceNew2 = ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue
                                                        Else
                                                            SinceNew2 = ObjAssemblyMonitorInspStatusPeriod.DiffAssemblyCurrentDoneOnValue
                                                        End If

                                                        TimeSinceNew = ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue

                                                    End If

                                                    Extension2 = ObjAssemblyMonitorInspStatusPeriod.ExtensionValue

                                                    If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = False) Then
                                                        DoneAt2 = ""
                                                    Else
                                                        DoneAt2 = ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
                                                    End If

                                                Else                                                       'Freq3 <> ""

                                                    Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.FrequencyValue


                                                    If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or
                                                       (ObjAssemblyMonitorInspStatus.IsApplicable = False And ObjAssemblyMonitorInspStatus.IsCompleted = True) Then

                                                        ElapsedTime2 = ""
                                                        RemainingTime2 = ""
                                                        DueAsof2 = ""
                                                        AssemblyDueAsof2 = ""
                                                        SinceNew2 = ""
                                                        TimeSinceNew = ""

                                                    Else

                                                        ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.AllElapsedValue
                                                        RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.RemainingValue

                                                        'Added By Prashant 17-Sep-2013
                                                        If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Or AppSettings("ClientCode") = "STR") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                            DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                        Else
                                                            DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.DueOnValue
                                                        End If

                                                        AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.DueOnValue

                                                        If ObjAssemblyMonitorInspStatus.MonitorTypeID = 3 And ObjAssemblyMonitorInspStatusPeriod.DoneOnValue = "" Then
                                                            SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue
                                                        Else
                                                            SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.DiffAssemblyCurrentDoneOnValue
                                                        End If

                                                        TimeSinceNew = TimeSinceNew & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue

                                                    End If

                                                    Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.ExtensionValue

                                                    If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = False) Then
                                                        DoneAt2 = ""
                                                    Else
                                                        DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
                                                    End If

                                                End If

                                            End If

                                        End If

                                    Next

                                    AssemblyID = ObjAssemblyStatus.AssemblyID
                                    Note = ObjAssemblyMonitorInspStatus.Notes
                                    Remark = ObjAssemblyMonitorInspStatus.DoneRemark
                                    ExtensionDate = ObjAssemblyMonitorInspStatus.ExtensionDate
                                    ApprovalRemark = ObjAssemblyMonitorInspStatus.ApprovalRemark
                                    Reference = ObjAssemblyMonitorInspStatus.Reference  'Added By Prashant 3-Apr-2013  'Indamer03042013
                                    Dim ATAcode = ObjAssemblyMonitorInspStatus.ATACode
                                    SourceDoc = ObjAssemblyMonitorInspStatus.SourceDoc

                                    'Added By Vikrant On 22-Dec-2020 For Star Air
                                    mnWOListForDueJobs = nWOListForDueJobs.GetWOListForDueJobs(ObjAssemblyMonitorInspStatus.ID)

                                    If mnWOListForDueJobs.Count > 0 Then
                                        nWONumber = mnWOListForDueJobs(0).WONumber
                                    Else
                                        nWONumber = ""
                                    End If
                                    'End

                                    If IsExcel Then

                                        If ATAcode.ToString.Length < 3 Then
                                            ATAChapter = ATAcode.ToString.PadLeft(3, "0"c) + " " + "-" + " " + ObjAssemblyMonitorInspStatus.ATANomenclature
                                        End If

                                        If Freq1 <> "" Then
                                            Freq1 = Freq1 + IIf(Freq2 <> "", Chr(10) + Freq2, "") + IIf(Freq3 <> "", Chr(10) + Freq3, "")
                                        Else
                                            Freq1 = Freq2 + IIf(Freq3 <> "", Chr(10) + Freq3, "")
                                        End If

                                        If DueAsof <> "" Then
                                            DueAsof = DueAsof + IIf(DueAsof1 <> "", Chr(10) + DueAsof1, "") + IIf(DueAsof2 <> "", Chr(10) + DueAsof2, "")
                                        Else
                                            DueAsof = DueAsof1 + IIf(DueAsof2 <> "", Chr(10) + DueAsof2, "")
                                        End If

                                        If ElapsedTime <> "" Then
                                            ElapsedTime = ElapsedTime + IIf(ElapsedTime1 <> "", Chr(10) + ElapsedTime1, "") + IIf(ElapsedTime2 <> "", Chr(10) + ElapsedTime2, "")
                                        Else
                                            ElapsedTime = ElapsedTime1 + IIf(ElapsedTime2 <> "", Chr(10) + ElapsedTime2, "")
                                        End If

                                        If RemainingTime <> "" Then
                                            RemainingTime = RemainingTime + IIf(RemainingTime1 <> "", Chr(10) + RemainingTime1, "") + IIf(RemainingTime2 <> "", Chr(10) + RemainingTime2, "")
                                        Else
                                            RemainingTime = RemainingTime1 + IIf(RemainingTime2 <> "", Chr(10) + RemainingTime2, "")
                                        End If

                                    End If

                                    If ObjAssemblyMonitorInspStatus.IsApplicable = False Then
                                        DueAsof = ""
                                        DueAsof1 = ""
                                        DueAsof2 = ""
                                        RemainingTime = ""
                                        RemainingTime1 = ""
                                        RemainingTime2 = ""
                                    End If

                                    ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , ,
                                                                                                 AssemblySerialNo,
                                                                                                 ATAChapter, , ,
                                                                                                 Position,
                                                                                                 MonitorType,
                                                                                                 MonitorTypeCode,
                                                                                                 Note,
                                                                                                 Remark,
                                                                                                 Description,,
                                                                                                 EstimatedDate, , ,
                                                                                                 Freq1,
                                                                                                 Freq2,
                                                                                                 Freq3,
                                                                                                 ElapsedTime,
                                                                                                 ElapsedTime1,
                                                                                                 ElapsedTime2,
                                                                                                 RemainingTime,
                                                                                                 RemainingTime1,
                                                                                                 RemainingTime2,
                                                                                                 DueAsof,
                                                                                                 DueAsof1,
                                                                                                 DueAsof2,
                                                                                                 AssemblyModel, , ,
                                                                                                 SinceNew2, , ,
                                                                                                 DoneAt2, , , ,
                                                                                                 ObjAssemblyMonitorInspStatus.ATACode, , , ,
                                                                                                 StartDateData, , , , , , , , , ,
                                                                                                 Reference, , DoneOnDate, , , , , ,
                                                                                                 AssemblyDueAsof2,
                                                                                                 Extension,
                                                                                                 Extension1,
                                                                                                 Extension2,
                                                                                                 ExtensionDate,
                                                                                                 ApprovalRemark, , ,
                                                                                                 Code,
                                                                                                 StatusMasterID.ToString, , , ,
                                                                                                 ObjAssemblyMonitorInspStatus.IsApplicable, ,
                                                                                                 ObjAssemblyMonitorInspStatus.MonitorTypeID, , , , , , ,
                                                                                                 TimeSinceNew,
                                                                                                 WONumber:=nWONumber,
                                                                                                 DoneONValueForAssembly:=DoneONValueForAssembly,
                                                                                                 SourceDoc:=SourceDoc,
                                                                                                 RecordID:=RecordOwnID,
                                                                                                 Zone:=ObjAssemblyMonitorInspStatus.Zone,
                                                                                                 Area:=ObjAssemblyMonitorInspStatus.Area))

                                End If

                            Next

                        End If

                        If chkComponent.Checked Then

                            For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList

                                For Each ObjCompMonitorInspStatus In ObjCompStatus.CompMonitorInspStatusList

                                    'If (ObjCompMonitorInspStatus.IsApplicable = True) Or
                                    '   (ObjCompMonitorInspStatus.IsApplicable = False And ObjCompMonitorInspStatus.IsCompleted = True) Then
                                    If (ObjCompMonitorInspStatus.IsApplicable = True And chkNotApplicable.Checked = False) Or
                                        (chkNotApplicable.Checked = True) Then

                                        ATAChapter = ObjCompMonitorInspStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorInspStatus.ATANomenclature
                                        Description = ObjCompMonitorInspStatus.Description
                                        PartNo = ObjCompStatus.PartName
                                        CompSerialNo = ObjCompStatus.CompSerialNo
                                        Position = ObjCompStatus.Position
                                        MonitorTypeCode = ObjCompMonitorInspStatus.Code
                                        MonitorType = ObjCompMonitorInspStatus.Type
                                        AssemblyModel = ObjAssemblyStatus.Model
                                        AssemblySerialNo = ObjAssemblyStatus.SerialNo
                                        DoneOnDate = ObjCompMonitorInspStatus.DoneOn
                                        Periodcount = ObjCompStatus.CompStatusPeriodList.Count()
                                        'Added By Utkarsh ON 12-Sep-2013 FOR BA11092013
                                        EstimatedDate = ObjCompMonitorInspStatus.EstimatedDateFormatted
                                        Code = ObjCompMonitorInspStatus.PartMonitorInspCode
                                        'End
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
                                        Extension = ""
                                        Extension1 = ""
                                        Extension2 = ""
                                        AssemblyDueAsof2 = ""
                                        SinceNew2 = ""
                                        DoneAt2 = ""
                                        StartDateData = ""
                                        TimeSinceNew = ""
                                        StatusMasterID = ObjCompMonitorInspStatus.PartMonitorInspID   'vikrant
                                        RecordOwnID = ObjCompMonitorInspStatus.ID.ToString

                                        Dim IsPeriod2Exists As Boolean = False

                                        'Added by Saylee on 19-Sep-2014 for ALL19092014
                                        Dim mDoneONValueForNoPeriod As Period = New Period(1, DBNull.Value)
                                        Dim mTSOValueForNoPeriod As Period = New Period(1, DBNull.Value)
                                        Dim tmpCurrentValue As Decimal = 0
                                        Dim mtmpCurrentValue(4) As Period
                                        Dim mPeriodID As Integer = 0
                                        DoneONValueForNoPeriod = String.Empty
                                        TSOValueForNoPeriod = String.Empty

                                        Dim mDoneONValueForAssembly As Period = New Period(1, DBNull.Value)
                                        DoneONValueForAssembly = String.Empty
                                        DiffCompInstDoneOnValue = ""

                                        For Count = 0 To Periodcount - 1

                                            If ObjCompStatus.CompStatusPeriodList(Count).PeriodID = 2 Then

                                                If ObjCompMonitorInspStatus.DoneOn = "" Then

                                                    For Each tmpObjCompMonitorInspStatusPeriod As CompMonitorInspStatusPeriodInfo In ObjCompMonitorInspStatus.CompMonitorInspStatusPeriodList

                                                        If tmpObjCompMonitorInspStatusPeriod.PeriodID = 2 Then
                                                            IsPeriod2Exists = True
                                                            Exit For

                                                        End If

                                                    Next

                                                    If IsPeriod2Exists = True Then

                                                        For Each ObjCompMonitorInspStatusPeriod In ObjCompMonitorInspStatus.CompMonitorInspStatusPeriodList

                                                            If ObjCompMonitorInspStatusPeriod.PeriodID = 2 Then

                                                                StartDateLabel = CType(IIf(StartDateLabel = "", StartDateLabel, StartDateLabel + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjCompStatus.CompStatusPeriodList(Count).PeriodName
                                                                StartDateData = CType(IIf(StartDateData = "", StartDateData, StartDateData + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted

                                                            End If

                                                        Next

                                                    Else
                                                        StartDateLabel = CType(IIf(StartDateLabel = "", StartDateLabel, StartDateLabel + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjCompStatus.CompStatusPeriodList(Count).PeriodName 'Added by Saylee on 31-May-2010
                                                        StartDateData = CType(IIf(StartDateData = "", StartDateData, StartDateData + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjCompStatus.CompStatusPeriodList(ObjCompStatus.CompStatusPeriodList(Count).PeriodID, "").CompStartValueFormatted 'Added by Saylee on 31-May-2010
                                                    End If

                                                Else
                                                    StartDateLabel = CType(IIf(StartDateLabel = "", StartDateLabel, StartDateLabel + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjCompStatus.CompStatusPeriodList(Count).PeriodName 'Added by Saylee on 31-May-2010
                                                    StartDateData = CType(IIf(StartDateData = "", StartDateData, StartDateData + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjCompMonitorInspStatus.DoneOnFormatted 'Added by Saylee on 31-May-2010
                                                End If

                                            End If

                                            'Added by Saylee on 19-Sep-2014 for ALL19092014
                                            'If no Cycle Period or Hour Period Present in Monitor Service       
                                            mDoneONValueForNoPeriod = New Period(ObjCompStatus.CompStatusPeriodList(Count).PeriodID, DBNull.Value)
                                            mTSOValueForNoPeriod = New Period(ObjCompStatus.CompStatusPeriodList(Count).PeriodID, DBNull.Value)

                                            mDoneONValueForAssembly = New Period(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, DBNull.Value)

                                            If (ObjCompStatus.CompStatusPeriodList(Count).PeriodID = 1 Or ObjCompStatus.CompStatusPeriodList(Count).PeriodID = 3) And
                                               Not (ObjCompMonitorInspStatus.CompMonitorInspStatusPeriodList.Contains(ObjCompStatus.CompStatusPeriodList(Count).PeriodID)) Then

                                                Dim mPeriodUnitID As Integer = 0

                                                If ObjCompStatus.CompStatusPeriodList(Count).PeriodID = 1 Then
                                                    mPeriodUnitID = 1
                                                    mPeriodID = 1
                                                ElseIf ObjCompStatus.CompStatusPeriodList(Count).PeriodID = 3 Then
                                                    mPeriodUnitID = 6
                                                    mPeriodID = 3

                                                End If

                                                If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = False) Then
                                                    DoneONValueForNoPeriod = ""
                                                Else

                                                    If ObjCompMonitorInspStatus.DoneOn <> "" Then

                                                        Dim mMachineMaintenance As MachineMaintenance = MachineMaintenance.GetMachineMaintenance(ObjCompMonitorInspStatus.ID, MachineMaintenanceActivity.ComponentInspection)
                                                        Dim mAssemblyCurrentValue As New Period(1, DBNull.Value)

                                                        If CurrentValueOnAsOnDate.GetCurrentValue(mMachineMaintenance.LogAssemblyStatusID, ObjCompMonitorInspStatus.DoneOn, mMachineMaintenance.LogNo, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID)(0).CurrentValueDec > 0 Then
                                                            mAssemblyCurrentValue = New Period(56, CurrentValueOnAsOnDate.GetCurrentValue(mMachineMaintenance.LogAssemblyStatusID, ObjCompMonitorInspStatus.DoneOn, mMachineMaintenance.LogNo, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID)(0).CurrentValueDec, , , , ObjMachine.HourType)
                                                        Else
                                                            mAssemblyCurrentValue = New Period(mPeriodID, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).AssemblyCurrentValueInDeciaml, , , , ObjMachine.HourType)
                                                        End If

                                                        If CurrentValueOnAsOnDate.GetCompCurrentValue(ObjCompStatus.CompID, mMachineMaintenance.AssemblyStatusID, ObjCompMonitorInspStatus.DoneOn, mMachineMaintenance.LogNo, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID).Count > 0 Then
                                                            tmpCurrentValue = New Period(mPeriodID, Period.Add(ObjCompStatus.CompStatusPeriodList(Count).PeriodID, mPeriodUnitID, CurrentValueOnAsOnDate.GetCompCurrentValue(ObjCompStatus.CompID, mMachineMaintenance.AssemblyStatusID, ObjCompMonitorInspStatus.DoneOn, mMachineMaintenance.LogNo, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID)(0).CompCurrentValueDec, Period.Difference(mAssemblyCurrentValue.DBValue, CurrentValueOnAsOnDate.GetCompCurrentValue(ObjCompStatus.CompID, mMachineMaintenance.AssemblyStatusID, ObjCompMonitorInspStatus.DoneOn, mMachineMaintenance.LogNo, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID)(0).AssemblyCurrentValueDec)), mPeriodUnitID, False, , ObjMachine.HourType).DbValueDec
                                                        Else
                                                            tmpCurrentValue = New Period(mPeriodID, Period.Add(ObjCompStatus.CompStatusPeriodList(Count).PeriodID, mPeriodUnitID, ObjCompStatus.CompStatusPeriodList(Count).CompCurrentValueInDecimal, Period.Difference(mAssemblyCurrentValue.DBValue, ObjCompStatus.CompStatusPeriodList(Count).AssemblyCurrentValueInDeciaml)), mPeriodUnitID, False, , ObjMachine.HourType).DbValueDec
                                                        End If

                                                        mDoneONValueForNoPeriod = New Period(mPeriodID, tmpCurrentValue, , , , ObjMachine.HourType)

                                                        If DoneONValueForNoPeriod = "" Then
                                                            DoneONValueForNoPeriod = mDoneONValueForNoPeriod.TextFormatted
                                                        Else
                                                            DoneONValueForNoPeriod = DoneONValueForNoPeriod + IIf(IsExcel, Chr(10), vbCrLf) + mDoneONValueForNoPeriod.TextFormatted
                                                        End If

                                                    End If

                                                End If

                                            End If

                                        Next

                                        For Each ObjCompMonitorInspStatusPeriod In ObjCompMonitorInspStatus.CompMonitorInspStatusPeriodList

                                            If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = False) Then
                                                DoneONValueForAssembly = ""
                                            Else

                                                If ObjCompMonitorInspStatus.DoneOn <> "" Then

                                                    mDoneONValueForAssembly = New Period(ObjCompMonitorInspStatusPeriod.PeriodID, DBNull.Value)

                                                    If ObjCompMonitorInspStatusPeriod.PeriodID <> 2 Then

                                                        Dim mMachineMaintenance As MachineMaintenance = MachineMaintenance.GetMachineMaintenance(ObjCompMonitorInspStatus.ID, MachineMaintenanceActivity.ComponentInspection)

                                                        Dim NoLogInMaintTable As Boolean = False

                                                        If CDate(ObjCompMonitorInspStatus.DoneOn.ToString) < CDate(ObjAssemblyStatus.AsOnDate.ToString) Then
                                                            mDoneONValueForAssembly.DBValue = 0
                                                        ElseIf mMachineMaintenance.LogNo <> 0 Then
                                                            If CurrentValueOnAsOnDate.GetCurrentValue(ObjAssemblyStatus.ID, ObjCompMonitorInspStatus.DoneOn, mMachineMaintenance.LogNo, ObjCompMonitorInspStatusPeriod.PeriodID)(0).CurrentValueDec > 0 Then
                                                                mDoneONValueForAssembly.Value = CurrentValueOnAsOnDate.GetCurrentValue(ObjAssemblyStatus.ID, ObjCompMonitorInspStatus.DoneOn, mMachineMaintenance.LogNo, ObjCompMonitorInspStatusPeriod.PeriodID)(0).CurrentValue
                                                            End If
                                                        Else
                                                            mDoneONValueForAssembly.DBValue = 0
                                                            NoLogInMaintTable = True
                                                        End If

                                                        If NoLogInMaintTable = False Then

                                                            If DoneONValueForAssembly = "" Then
                                                                DoneONValueForAssembly = mDoneONValueForAssembly.TextFormatted
                                                            Else
                                                                DoneONValueForAssembly = DoneONValueForAssembly + IIf(IsExcel, Chr(10), vbCrLf) + mDoneONValueForAssembly.TextFormatted
                                                            End If

                                                        Else

                                                            If DoneONValueForAssembly = "" Then
                                                                DoneONValueForAssembly = ""
                                                            Else
                                                                DoneONValueForAssembly = DoneONValueForAssembly + IIf(IsExcel, Chr(10), vbCrLf) + ""
                                                            End If

                                                        End If


                                                    Else

                                                        If DoneONValueForAssembly = "" Then
                                                            DoneONValueForAssembly = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                        Else
                                                            DoneONValueForAssembly = DoneONValueForAssembly + IIf(IsExcel, Chr(10), vbCrLf) + ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                        End If

                                                    End If

                                                End If

                                            End If
                                            'End If


                                            If ReportStatus = 0 Then  'Landscape

                                                If ObjCompMonitorInspStatusPeriod.PeriodID = 1 Then

                                                    Freq1 = ObjCompMonitorInspStatusPeriod.FrequencyValue

                                                    If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Or
                                                       (ObjCompMonitorInspStatus.IsApplicable = False And ObjCompMonitorInspStatus.IsCompleted = True) Then

                                                        ElapsedTime = ""
                                                        RemainingTime = ""
                                                        DueAsof = ""
                                                        'Added By Prashant 04-Aug-2009
                                                        SinceNew2 = ""
                                                        '-----------------------------
                                                        TimeSinceNew = ""
                                                    Else

                                                        ElapsedTime = ObjCompMonitorInspStatusPeriod.AllElapsedValue
                                                        RemainingTime = ObjCompMonitorInspStatusPeriod.RemainingValue

                                                        If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                            DueAsof = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                        Else
                                                            DueAsof = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                        End If
                                                        '--------

                                                        'Added By Prashant 04-Aug-2009
                                                        If ObjCompMonitorInspStatus.MonitorTypeID = 3 And ObjCompMonitorInspStatusPeriod.DoneOnValue = "" Then
                                                            SinceNew2 = ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted
                                                        Else
                                                            SinceNew2 = ObjCompMonitorInspStatusPeriod.DiffCompCurrentDoneOnValueFormatted
                                                        End If
                                                        '-----------------------------
                                                        TimeSinceNew = ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted

                                                    End If

                                                    Extension = ObjCompMonitorInspStatusPeriod.ExtensionValue

                                                End If

                                                If ObjCompMonitorInspStatusPeriod.PeriodID = 2 Then

                                                    Freq2 = ObjCompMonitorInspStatusPeriod.FrequencyValueFormatted

                                                    If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Or
                                                       (ObjCompMonitorInspStatus.IsApplicable = False And ObjCompMonitorInspStatus.IsCompleted = True) Then

                                                        ElapsedTime1 = ""
                                                        RemainingTime1 = ""
                                                        DueAsof1 = ""

                                                    Else

                                                        ElapsedTime1 = ObjCompMonitorInspStatusPeriod.AllElapsedValueFormatted
                                                        RemainingTime1 = ObjCompMonitorInspStatusPeriod.RemainingValueFormatted

                                                        If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                            DueAsof1 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                        Else
                                                            DueAsof1 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                        End If
                                                        '------------

                                                    End If

                                                    Extension1 = ObjCompMonitorInspStatusPeriod.ExtensionValueFormatted

                                                End If

                                                If ObjCompMonitorInspStatusPeriod.PeriodID = 3 Or
                                                   ObjCompMonitorInspStatusPeriod.PeriodID = 4 Or
                                                   ObjCompMonitorInspStatusPeriod.PeriodID = 5 Or
                                                   ObjCompMonitorInspStatusPeriod.PeriodID = 6 Or
                                                   ObjCompMonitorInspStatusPeriod.PeriodID = 7 Or
                                                   ObjCompMonitorInspStatusPeriod.PeriodID = 8 Or
                                                   ObjCompMonitorInspStatusPeriod.PeriodID = 12 Or
                                                   ObjCompMonitorInspStatusPeriod.PeriodID = 13 Or
                                                   ObjCompMonitorInspStatusPeriod.PeriodID = 14 Then

                                                    If Freq3 = "" Then

                                                        Freq3 = ObjCompMonitorInspStatusPeriod.FrequencyValue

                                                        If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Or
                                                           (ObjCompMonitorInspStatus.IsApplicable = False And ObjCompMonitorInspStatus.IsCompleted = True) Then

                                                            ElapsedTime2 = ""
                                                            RemainingTime2 = ""
                                                            DueAsof2 = ""
                                                            AssemblyDueAsof2 = ""
                                                            SinceNew2 = ""
                                                            TimeSinceNew = ""

                                                        Else

                                                            ElapsedTime2 = ObjCompMonitorInspStatusPeriod.AllElapsedValue
                                                            RemainingTime2 = ObjCompMonitorInspStatusPeriod.RemainingValue

                                                            If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Or AppSettings("ClientCode") = "STR") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                                DueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                            Else
                                                                DueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                            End If
                                                            '--------------
                                                            AssemblyDueAsof2 = ObjCompMonitorInspStatusPeriod.DueOnValueFormatted

                                                            If ObjCompMonitorInspStatus.MonitorTypeID = 3 And ObjCompMonitorInspStatusPeriod.DoneOnValue = "" Then
                                                                SinceNew2 = ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted
                                                            Else
                                                                SinceNew2 = ObjCompMonitorInspStatusPeriod.DiffCompCurrentDoneOnValueFormatted
                                                            End If

                                                            TimeSinceNew = ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted

                                                        End If

                                                        Extension2 = ObjCompMonitorInspStatusPeriod.ExtensionValue

                                                        If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = False) Then
                                                            DoneAt2 = ""
                                                        Else
                                                            DoneAt2 = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                        End If

                                                        DiffCompInstDoneOnValue = ObjCompMonitorInspStatusPeriod.DiffCompInstDoneOnValue

                                                    Else

                                                        Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.FrequencyValue

                                                        If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Or
                                                           (ObjCompMonitorInspStatus.IsApplicable = False And ObjCompMonitorInspStatus.IsCompleted = True) Then

                                                            ElapsedTime2 = ""
                                                            RemainingTime2 = ""
                                                            DueAsof2 = ""
                                                            AssemblyDueAsof2 = ""
                                                            SinceNew2 = ""
                                                            TimeSinceNew = ""

                                                        Else

                                                            ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AllElapsedValue
                                                            RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.RemainingValue

                                                            If (AppSettings("ClientCode") = "BA" Or
                                                                AppSettings("ClientCode") = "PAS" Or
                                                                AppSettings("ClientCode") = "Novo" Or
                                                                AppSettings("ClientCode") = "YA" Or
                                                                AppSettings("ClientCode") = "TA" Or
                                                                AppSettings("ClientCode") = "STR") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013

                                                                DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame

                                                            Else

                                                                DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted

                                                            End If
                                                            '-------------
                                                            AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DueOnValueFormatted

                                                            If ObjCompMonitorInspStatus.MonitorTypeID = 3 And ObjCompMonitorInspStatusPeriod.DoneOnValue = "" Then
                                                                SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted
                                                            Else
                                                                SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DiffCompCurrentDoneOnValueFormatted
                                                            End If

                                                            TimeSinceNew = TimeSinceNew & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted

                                                        End If

                                                        Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.ExtensionValue

                                                        If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = False) Then
                                                            DoneAt2 = ""
                                                        Else
                                                            DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                        End If

                                                        DiffCompInstDoneOnValue = DiffCompInstDoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DiffCompInstDoneOnValue

                                                    End If

                                                End If

                                            Else

                                                If ObjCompMonitorInspStatusPeriod.PeriodID = 2 Then        'StartDate

                                                    If Freq3 = "" Then

                                                        Freq3 = ObjCompMonitorInspStatusPeriod.FrequencyValueFormatted

                                                        If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Or
                                                           (ObjCompMonitorInspStatus.IsApplicable = False And ObjCompMonitorInspStatus.IsCompleted = True) Then

                                                            ElapsedTime2 = ""
                                                            RemainingTime2 = ""
                                                            DueAsof2 = ""
                                                            AssemblyDueAsof2 = ""

                                                        Else

                                                            ElapsedTime2 = ObjCompMonitorInspStatusPeriod.AllElapsedValueFormatted
                                                            RemainingTime2 = ObjCompMonitorInspStatusPeriod.RemainingValueFormatted

                                                            If (AppSettings("ClientCode") = "BA" Or
                                                                AppSettings("ClientCode") = "PAS" Or
                                                                AppSettings("ClientCode") = "Novo" Or
                                                                AppSettings("ClientCode") = "YA" Or
                                                                AppSettings("ClientCode") = "TA" Or
                                                                AppSettings("ClientCode") = "STR") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013

                                                                DueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame

                                                            Else

                                                                DueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted

                                                            End If
                                                            '------------------
                                                            AssemblyDueAsof2 = ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
                                                            DoneAt2 = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted

                                                        End If

                                                        Extension2 = ObjCompMonitorInspStatusPeriod.ExtensionValueFormatted

                                                        If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = False) Then
                                                            DoneAt2 = ""
                                                        Else
                                                            DoneAt2 = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                        End If

                                                        DiffCompInstDoneOnValue = ObjCompMonitorInspStatusPeriod.DiffCompInstDoneOnValue

                                                    Else                                                   'Freq3 <> ""

                                                        Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.FrequencyValueFormatted

                                                        If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Or
                                                           (ObjCompMonitorInspStatus.IsApplicable = False And ObjCompMonitorInspStatus.IsCompleted = True) Then

                                                            ElapsedTime2 = ""
                                                            RemainingTime2 = ""
                                                            DueAsof2 = ""
                                                            AssemblyDueAsof2 = ""

                                                        Else

                                                            ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AllElapsedValueFormatted
                                                            RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.RemainingValueFormatted

                                                            If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Or AppSettings("ClientCode") = "STR") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                                DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                            Else
                                                                DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                            End If
                                                            '-----------------
                                                            AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DueOnValueFormatted

                                                        End If

                                                        Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.ExtensionValueFormatted

                                                        If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = False) Then
                                                            DoneAt2 = ""
                                                        Else
                                                            DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                        End If

                                                        DiffCompInstDoneOnValue = DiffCompInstDoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DiffCompInstDoneOnValue

                                                    End If

                                                Else 'For PeriodID <> 2

                                                    If Freq3 = "" Then

                                                        Freq3 = ObjCompMonitorInspStatusPeriod.FrequencyValue

                                                        If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Or
                                                           (ObjCompMonitorInspStatus.IsApplicable = False And ObjCompMonitorInspStatus.IsCompleted = True) Then

                                                            ElapsedTime2 = ""
                                                            RemainingTime2 = ""
                                                            DueAsof2 = ""
                                                            AssemblyDueAsof2 = ""
                                                            SinceNew2 = ""
                                                            TimeSinceNew = ""

                                                        Else

                                                            ElapsedTime2 = ObjCompMonitorInspStatusPeriod.AllElapsedValue
                                                            RemainingTime2 = ObjCompMonitorInspStatusPeriod.RemainingValue

                                                            If ObjCompMonitorInspStatusPeriod.PeriodID = 9 Then

                                                                DueAsof2 = ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
                                                                AssemblyDueAsof2 = ObjCompMonitorInspStatusPeriod.DueOnValueFormatted

                                                                If ObjCompMonitorInspStatus.MonitorTypeID = 3 And ObjCompMonitorInspStatusPeriod.DoneOnValue = "" Then
                                                                    SinceNew2 = ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted
                                                                Else
                                                                    SinceNew2 = ObjCompMonitorInspStatusPeriod.DiffCompCurrentDoneOnValueFormatted
                                                                End If

                                                                TimeSinceNew = ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted

                                                            Else

                                                                If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Or AppSettings("ClientCode") = "STR") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                                    DueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                                Else
                                                                    DueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                                End If
                                                                '--------------

                                                                AssemblyDueAsof2 = ObjCompMonitorInspStatusPeriod.DueOnValueFormatted

                                                                If ObjCompMonitorInspStatus.MonitorTypeID = 3 And ObjCompMonitorInspStatusPeriod.DoneOnValue = "" Then
                                                                    SinceNew2 = ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted
                                                                Else
                                                                    SinceNew2 = ObjCompMonitorInspStatusPeriod.DiffCompCurrentDoneOnValueFormatted
                                                                End If

                                                                TimeSinceNew = ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted

                                                            End If

                                                        End If

                                                        Extension2 = ObjCompMonitorInspStatusPeriod.ExtensionValue

                                                        If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = False) Then
                                                            DoneAt2 = ""
                                                        Else
                                                            DoneAt2 = DoneAt2 & ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                        End If

                                                        DiffCompInstDoneOnValue = ObjCompMonitorInspStatusPeriod.DiffCompInstDoneOnValue

                                                    Else

                                                        Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.FrequencyValue

                                                        If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Or
                                                           (ObjCompMonitorInspStatus.IsApplicable = False And ObjCompMonitorInspStatus.IsCompleted = True) Then

                                                            ElapsedTime2 = ""
                                                            RemainingTime2 = ""
                                                            DueAsof2 = ""
                                                            AssemblyDueAsof2 = ""
                                                            SinceNew2 = ""
                                                            TimeSinceNew = ""

                                                        Else

                                                            ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AllElapsedValue
                                                            RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.RemainingValue

                                                            If ObjCompMonitorInspStatusPeriod.PeriodID = 9 Then

                                                                DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
                                                                AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DueOnValueFormatted

                                                                If ObjCompMonitorInspStatus.MonitorTypeID = 3 And ObjCompMonitorInspStatusPeriod.DoneOnValue = "" Then
                                                                    SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted
                                                                Else
                                                                    SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DiffCompCurrentDoneOnValueFormatted
                                                                End If

                                                                TimeSinceNew = TimeSinceNew & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted

                                                            Else

                                                                If (AppSettings("ClientCode") = "BA" Or
                                                                    AppSettings("ClientCode") = "PAS" Or
                                                                    AppSettings("ClientCode") = "Novo" Or
                                                                    AppSettings("ClientCode") = "YA" Or
                                                                    AppSettings("ClientCode") = "TA" Or
                                                                    AppSettings("ClientCode") = "STR") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013z

                                                                    DueAsof2 = DueAsof2 &
                                                                               IIf(IsExcel, Chr(10), vbCrLf) &
                                                                               ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame

                                                                Else

                                                                    DueAsof2 = DueAsof2 &
                                                                               IIf(IsExcel, Chr(10), vbCrLf) &
                                                                               ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted

                                                                End If
                                                                '-----------------

                                                                AssemblyDueAsof2 = AssemblyDueAsof2 &
                                                                                   IIf(IsExcel, Chr(10), vbCrLf) &
                                                                                   ObjCompMonitorInspStatusPeriod.DueOnValueFormatted

                                                                If ObjCompMonitorInspStatus.MonitorTypeID = 3 And ObjCompMonitorInspStatusPeriod.DoneOnValue = "" Then

                                                                    SinceNew2 = SinceNew2 &
                                                                                IIf(IsExcel, Chr(10), vbCrLf) &
                                                                                ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted

                                                                Else
                                                                    SinceNew2 = SinceNew2 &
                                                                                IIf(IsExcel, Chr(10), vbCrLf) &
                                                                                ObjCompMonitorInspStatusPeriod.DiffCompCurrentDoneOnValueFormatted '--------------------------
                                                                End If

                                                                TimeSinceNew = TimeSinceNew &
                                                                               IIf(IsExcel, Chr(10), vbCrLf) &
                                                                               ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted

                                                            End If

                                                        End If

                                                        Extension2 = Extension2 &
                                                                     IIf(IsExcel, Chr(10), vbCrLf) &
                                                                     ObjCompMonitorInspStatusPeriod.ExtensionValue

                                                        If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = False) Then
                                                            DoneAt2 = ""
                                                        Else
                                                            DoneAt2 = DoneAt2 &
                                                                      IIf(IsExcel, Chr(10), vbCrLf) &
                                                                      ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                        End If

                                                        DiffCompInstDoneOnValue = DiffCompInstDoneOnValue &
                                                                                  IIf(IsExcel, Chr(10), vbCrLf) &
                                                                                  ObjCompMonitorInspStatusPeriod.DiffCompInstDoneOnValue

                                                    End If

                                                End If

                                            End If

                                        Next

                                        AssemblyID = ObjAssemblyStatus.AssemblyID
                                        Note = ObjCompMonitorInspStatus.Notes
                                        Remark = ObjCompMonitorInspStatus.DoneRemark
                                        ExtensionDate = ObjCompMonitorInspStatus.ExtensionDate
                                        ApprovalRemark = ObjCompMonitorInspStatus.ApprovalRemark
                                        Reference = ObjCompMonitorInspStatus.Reference  'Added By Prashant 3-Apr-2013  'Indamer03042013
                                        Dim ATACode = ObjCompMonitorInspStatus.ATACode
                                        SourceDoc = ObjCompMonitorInspStatus.SourceDoc

                                        'Added By Vikrant On 22-Dec-2020 For Star Air
                                        mnWOListForDueJobs = nWOListForDueJobs.GetWOListForDueJobs(ObjCompMonitorInspStatus.ID)

                                        If mnWOListForDueJobs.Count > 0 Then
                                            nWONumber = mnWOListForDueJobs(0).WONumber
                                        Else
                                            nWONumber = ""
                                        End If
                                        'End

                                        If IsExcel Then

                                            If ATACode.ToString.Length < 3 Then

                                                ATAChapter = ATACode.ToString.PadLeft(3, "0"c) + " " + "-" + " " +
                                                             ObjCompMonitorInspStatus.ATANomenclature

                                            End If

                                            If Freq1 <> "" Then

                                                Freq1 = Freq1 +
                                                        IIf(Freq2 <> "", Chr(10) + Freq2, "") +
                                                        IIf(Freq3 <> "", Chr(10) + Freq3, "")

                                            Else

                                                Freq1 = Freq2 +
                                                        IIf(Freq3 <> "", Chr(10) + Freq3, "")

                                            End If

                                            If DueAsof <> "" Then

                                                DueAsof = DueAsof +
                                                          IIf(DueAsof1 <> "", Chr(10) + DueAsof1, "") +
                                                          IIf(DueAsof2 <> "", Chr(10) + DueAsof2, "")

                                            Else

                                                DueAsof = DueAsof1 +
                                                          IIf(DueAsof2 <> "", Chr(10) + DueAsof2, "")

                                            End If

                                            If ElapsedTime <> "" Then

                                                ElapsedTime = ElapsedTime +
                                                              IIf(ElapsedTime1 <> "", Chr(10) + ElapsedTime1, "") +
                                                              IIf(ElapsedTime2 <> "", Chr(10) + ElapsedTime2, "")
                                            Else

                                                ElapsedTime = ElapsedTime1 +
                                                              IIf(ElapsedTime2 <> "", Chr(10) + ElapsedTime2, "")

                                            End If

                                            If RemainingTime <> "" Then

                                                RemainingTime = RemainingTime +
                                                                IIf(RemainingTime1 <> "", Chr(10) + RemainingTime1, "") +
                                                                IIf(RemainingTime2 <> "", Chr(10) + RemainingTime2, "")

                                            Else

                                                RemainingTime = RemainingTime1 +
                                                                IIf(RemainingTime2 <> "", Chr(10) + RemainingTime2, "")

                                            End If

                                        End If

                                        If ObjCompMonitorInspStatus.IsApplicable = False Then
                                            DueAsof = ""
                                            DueAsof1 = ""
                                            DueAsof2 = ""
                                            RemainingTime = ""
                                            RemainingTime1 = ""
                                            RemainingTime2 = ""
                                        End If

                                        ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , ,
                                                                                                     AssemblySerialNo,
                                                                                                     ATAChapter,
                                                                                                     PartNo,
                                                                                                     CompSerialNo,
                                                                                                     Position,
                                                                                                     MonitorType,
                                                                                                     MonitorTypeCode,
                                                                                                     Note,
                                                                                                     Remark,
                                                                                                     Description,,
                                                                                                     EstimatedDate, , ,
                                                                                                     Freq1,
                                                                                                     Freq2,
                                                                                                     Freq3,
                                                                                                     ElapsedTime,
                                                                                                     ElapsedTime1,
                                                                                                     ElapsedTime2,
                                                                                                     RemainingTime,
                                                                                                     RemainingTime1,
                                                                                                     RemainingTime2,
                                                                                                     DueAsof,
                                                                                                     DueAsof1,
                                                                                                     DueAsof2,
                                                                                                     AssemblyModel, , ,
                                                                                                     SinceNew2, , ,
                                                                                                     DoneAt2, , , ,
                                                                                                     ObjCompMonitorInspStatus.ATACode, , , ,
                                                                                                     StartDateData, , , , , , , , , ,
                                                                                                     Reference, ,
                                                                                                     DoneOnDate, , , , , ,
                                                                                                     AssemblyDueAsof2,
                                                                                                     Extension,
                                                                                                     Extension1,
                                                                                                     Extension2,
                                                                                                     ExtensionDate,
                                                                                                     ApprovalRemark, , ,
                                                                                                     Code,
                                                                                                     StatusMasterID.ToString, , , ,
                                                                                                     ObjCompMonitorInspStatus.IsApplicable, ,
                                                                                                     ObjCompMonitorInspStatus.MonitorTypeID, , , , , , ,
                                                                                                     TimeSinceNew,
                                                                                                     WONumber:=nWONumber,
                                                                                                     DoneONValueForAssembly:=DoneONValueForAssembly,
                                                                                                     SourceDoc:=SourceDoc,
                                                                                                     DiffCompInstDoneOnValue:=DiffCompInstDoneOnValue,
                                                                                                     RecordID:=RecordOwnID,
                                                                                                     Zone:=ObjCompMonitorInspStatus.Zone,
                                                                                                     Area:=ObjCompMonitorInspStatus.Area))

                                    End If

                                Next

                            Next

                        End If

                    Next

                Next

            End If

            Return ReportMaintenanceDetails

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Function

    Private Sub SetReport(Optional ByMail As Boolean = False,
                          Optional IsExcel As Boolean = False)  'Parameter Added by Shital on 14-Sep-2016

        ReportMaintenanceDetails = New ReportMaintenanceDetailList
        ReportStatusList = New rptStatusList
        Dim da As New ObjectAdapter
        Dim ds As New dsReportMaintenanceDetail
        Dim RptCofA As Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim OperatorName As String = ""
        Dim RptModificationStatusList As New crModificationStatusList '4

        Try

            SetValues()

            'Added by Saylee on 11-Aug-2011
            If cmbAircraft.SelectedIndex > 0 Then

                Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue))

                If mMachineOperatorName.OperatorName <> "" Then OperatorName = mMachineOperatorName.OperatorName

                If AppSettings("ShowMaintenanceForNewClients") = "True" Then

                    mLastAMPRef = LastMPDAMPRef.GetLastMPDAMPRefForMachine(MachineID:=New Guid(cmbAircraft.SelectedValue.ToString))

                    If (mLastAMPRef.AMPNo <> "") Then
                        AMPNoStr = "AMP No.: " + mLastAMPRef.AMPNo + ",Rev No.: " + mLastAMPRef.RevNo + ",Dated: " + mLastAMPRef.FromDateFormatted
                    Else
                        AMPNoStr = ""
                    End If

                Else
                    AMPNoStr = ""
                End If

            End If
            'End If

            If cmbFormat.SelectedValue = "2" Then 'Format 2 Added By Vikrant On 08-Dec-2020 For ALL08122020

                If AppSettings("ShowMaintenanceForNewClients") = "True" Then 'Ajay Added 30-06-2023
                    RptCofA = New crLDNDFormat2ForTaskNo
                Else
                    RptCofA = New crLDNDFormat2
                End If

            Else 'Existing Condition

                If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                    RptCofA = New crLDNDForTaskNo
                Else
                    RptCofA = New crLDND
                End If

            End If

            Dim LastFlownDate As String = ""

            Dim mMaxLogNo As MaxLogNo = MaxLogNo.GetMaxLogNo(AsonDate,
                                                             New Guid(MachineName),
                                                             New Guid(AssemblyName))

            If mMaxLogNo.Count <> 0 Then
                LastFlownDate = mMaxLogNo(0).LogDate.ToString 'Last Flight Log Date
            End If

            ReportDetail()

            'As per the suggestion by SayleeMame
            'Added By Abhishek ON 9-Sep-2017 
            Dim ServicesShortName As String = ""

            If IsSerSelect Then


                If AppSettings("ClientCode") = "7AR" Then
                    Dim mServiceTypeMasterList As ServiceTypeList = ServiceTypeList.GetServiceTypeList()
                    For i As Integer = 0 To mServiceTypeMasterList.Count - 1
                        If ServicesShortName = "" Then
                            ServicesShortName = IIf(Not mServiceTypeMasterList(i).CodeType Is Nothing, mServiceTypeMasterList(i).CodeType, "")
                        Else
                            ServicesShortName = ServicesShortName + IIf(Not mServiceTypeMasterList(i).CodeType Is Nothing, ", " + mServiceTypeMasterList(i).CodeType, "")
                        End If
                    Next
                Else
                    For i As Integer = 0 To mServiceTypeList.Count - 1

                        If ServicesShortName = "" Then

                            ServicesShortName = IIf(Not mServiceTypeList(i, "").CodeType Is Nothing,
                                                mServiceTypeList(i, "").CodeType,
                                                "")

                        Else

                            ServicesShortName = ServicesShortName + IIf(Not mServiceTypeList(i, "").CodeType Is Nothing,
                                                                    ", " + mServiceTypeList(i, "").CodeType,
                                                                    "")

                        End If

                    Next
                End If
            End If

            Dim InspsShortName As String = ""

            If IsInsSelect Then

                For i As Integer = 0 To mInspectionTypeList.Count - 1

                    If InspsShortName = "" Then

                        InspsShortName = IIf(Not mInspectionTypeList(i, "").CodeType Is Nothing,
                                             mInspectionTypeList(i, "").CodeType,
                                             "")

                    Else

                        InspsShortName = InspsShortName + IIf(Not mInspectionTypeList(i, "").CodeType Is Nothing,
                                                              ", " + mInspectionTypeList(i, "").CodeType,
                                                              "")

                    End If

                Next

            End If

            'Added By Vikrant On 27-Feb-2020 for showing Periods Code and their long forms at bottom of report
            Dim mPeriodUnitList As PeriodUnitList
            Dim PeriodsShortName As New StringBuilder

            mPeriodUnitList = PeriodUnitList.GetPeriodUnitList()
            For i As Integer = 0 To mPeriodUnitList.Count - 1
                PeriodsShortName.Append(mPeriodUnitList(i).Code + "-" + mPeriodUnitList(i).PeriodUnitName + ", ")
            Next
            'End

            'Added By Utkarsh ON 12-Sep-2013 FOR BA11092013 parameter searchstr1
            Report = New ReportData(mCompanyDetail.CompanyName,
                                    mCompanyDetail.Address,
                                    mCompanyDetail.Tel1,
                                    mCompanyDetail.Tel2,
                                    mCompanyDetail.Fax,
                                    mCompanyDetail.Email,
                                    mCompanyDetail.WebSite,
                                    "Last Done Next Due Report",
                                    New SmartDate(txtFromDate.Text.ToString).FormattedText,
                                    "LD/ND",
                                    LastFlownDate,
                                    SearchStr4,
                                    IIf(cmbATAChapter.SelectedIndex <= 0,
                                                  "ALL",
                                                  cmbATAChapter.SelectedItem.ToString),
                                    AppSettings("Product Version"),
                                    AppSettings("SINote"),
                                    "",
                                    OperatorName,
                                    IIf(Aircraft = "", "ALL", Aircraft),
                                    IIf(Assembly1 = "", "ALL", Assembly1),
                                    AppSettings("Logo"),
                                    AppSettings("ClientCode"),
                                    ServicesShortName,
                                    InspsShortName,
                                    SearchStr17:=PeriodsShortName.ToString.Trim.TrimEnd(","),
                                    SearchStr18:=cmbFormat.SelectedItem.ToString,
                                    SearchStr19:=AMPNoStr)

            SetSession()

            If ByMail = False Then    'If case added by shital on 14-Sep-2016

                If ReportMaintenanceDetails.Count = 0 Then

                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound,
                                    MSGBox.Message_text.NoRecordFound,
                                    "There is no record for this search criteria",
                                    MsgBoxStyle.OkOnly,
                                    "")
                    Exit Sub

                Else

                    If Not IsExcel Then RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name,
                                                                            718)
                End If

            End If

            'added by shital on 14-Sep-2016
            If (ByMail = True And ReportMaintenanceDetails.Count <= 0) Then

                SendMailFile.SendMailFile(,
                                          Thread.CurrentPrincipal.Identity.Name,
                                          ReportLabel,
                                          "",
                                          "There is no record for this search criteria.",
                                          "",
                                          Session("ToSendMailIDs"),
                                          Session("CcSendMailIDs"),
                                          "",
                                          True,
                                          Remark:=Session("SendMailRemark"),
                                          ReportGeneratedBy:=Session("ReportGenratedBy"),
                                          SmtpHost:=mModuleList.Item("LDND").SmtpHost,
                                          SmtpPort:=mModuleList.Item("LDND").SmtpPort,
                                          SmtpUser:=mModuleList.Item("LDND").SmtpUser,
                                          SmtpPassword:=mModuleList.Item("LDND").SmtpPassword)

                Exit Sub

            End If

            ds.Clear()
            '-----------Added by vikrant for Report Logo---------------
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            '----------------------------------------------------------
            da.Fill(ds, ReportMaintenanceDetails)
            da.Fill(ds, Report)
            da.Fill(ds, ReportStatusList)
            da.Fill(ds, mrptImage) 'Added by vikrant for Report Logo

            RptCofA.SetDataSource(ds)
            Session("CrystalReport") = RptCofA

            'added by shital on 14-Sep-2016
            If (ByMail = True) Then

                SendMailFile.SendMailFile(Session("CrystalReport"),
                                          Thread.CurrentPrincipal.Identity.Name,
                                          "Last Done Next Due Report",
                                          "",
                                          " For " + lblAircraft1.Text, ,
                                          Session("ToSendMailIDs"),
                                          Session("CcSendMailIDs"),
                                          "",
                                          True,
                                          Remark:=Session("SendMailRemark"),
                                          ReportGeneratedBy:=Session("ReportGenratedBy"),
                                          SmtpHost:=mModuleList.Item("LDND").SmtpHost,
                                          SmtpPort:=mModuleList.Item("LDND").SmtpPort,
                                          SmtpUser:=mModuleList.Item("LDND").SmtpUser,
                                          SmtpPassword:=mModuleList.Item("LDND").SmtpPassword)

            ElseIf Not IsExcel Then

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "openTranDetail",
                                                    "openTranDetail();",
                                                    True)

                MarkLog(Action.Print,
                        "LDND",
                        mCofASearchingCriteria,
                        ErrorType.NoError,
                        Guid.Empty,
                        EventLogID)
            End If

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Ok
                    DataFieldBind()
            End Select
        End If
    End Sub

#End Region

#Region "Data Binding"

    Public Sub CustomValidate(s As Object, e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbType" Or custValidator.ControlToValidate = "" Then                      'Aircraft
            Dim i As Integer
            Dim flag As Boolean = False
            For i = 0 To cmbType.Items.Count - 1
                If cmbType.Items(i).Selected Then
                    flag = True
                End If
            Next
            If flag = False Then
                custValidator.ErrorMessage = "Please select the Type"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub

    Public Sub SetComboOfMachine(AOnDate As String)
        mMachineList = MachineList.GetMachineListMonitoringStatus(AOnDate, , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , True, "<SELECT>", SkipIsForInventoryAircarft:=True)
        cmbAircraft.DataSource = mMachineList
        Session("mMachineList") = mMachineList
        cmbAircraft.DataBind()
    End Sub

    Public Sub SetCombo()                                         'Added Code

        GetSession()
        mTypeListForCofA = TypeListForCofA.GetTypeListForCofA(1, "")
        cmbType.DataSource = mTypeListForCofA
        cmbType.DataBind()


        mServiceTypeList = PartMonitorServiceTypeList.GetPartMonitorServiceTypeList() 'ServiceType
        mInspectionTypeList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList()  'Inspection Type 

    End Sub

    Private Sub DataFieldBind()

        mTypeListForCofA = TypeListForCofA.GetTypeListForCofA(1, "")
        cmbType.DataSource = mTypeListForCofA
        cmbType.DataBind()

        mServiceTypeList = PartMonitorServiceTypeList.GetPartMonitorServiceTypeList()
        ListServiceType.DataSource = mServiceTypeList
        Session("mServiceTypeList") = mServiceTypeList

        'If AppSettings("ClientCode") = "RAL" Or AppSettings("ClientCode") = "Indamer" Then  'Added By Prashant 20-Aug-2012/ "Indamer" added by Saylee on 30-04-2013 for Indamer30042013-1
        '    mInspectionTypeList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList(ModelMonitorInspTypeList.serach.AllInspections)
        'Else
        '    mInspectionTypeList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList(ModelMonitorInspTypeList.serach.ExludingRoutineInspections)
        'End If
        mInspectionTypeList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList(ModelMonitorInspTypeList.serach.AllInspections)

        ListInspectionType.DataSource = mInspectionTypeList
        Session("mInspectionTypeList") = mInspectionTypeList


        'Added by Saylee on 20-Apr-2010
        mATAList = ATAList.GetATAList("", "(All)")
        Session("mATAList") = mATAList
        cmbATAChapter.DataSource = mATAList
        '***************************
        DataBind()
    End Sub

#End Region

#Region "Events"

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Prashant on 04-Dec-2013
        If Not IsPostBack And Session("Sender") = "" Then
            Session("MiddleFrame") = "wfrptLastDoneNextDueReport.aspx?"
            ControlVisibility()
            SetCombo()                            'Added Code
            lblAssembly.Enabled = False
            cmbAssembly.Enabled = False
            txtFromDate.Text = New SmartDate(Now.Date.ToString).FormattedText
            AOnDate = Now.Date
            SetComboOfMachine(AOnDate)            'Added Code
            DataFieldBind()

            ReportStatus = 1
            Session("ReportStatus") = ReportStatus
            ListServiceType.Enabled = False
            ListInspectionType.Enabled = False

        End If
        SetSession()

        lbltitle.Text = "Search criteria for LD/ND"
        chkAssembly.Text = IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Show Assembly AMPs", "Show Assembly Insps/Services")  '"Show Assembly Insps/Services"
        chkComponent.Text = IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Show Component AMPs", "Show Component Insps/Services") '"Show Component Insps/Services"

        cmbType.DataBind()
    End Sub

    Private Sub btnCurrentSearchCriteria_Click(sender As Object, e As EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
        pnlCriteria.Visible = True
        upnlCurrentCriteria.Update()
    End Sub

    Private Sub DisplayReport(sender As Object, e As EventArgs) Handles btnDisplay.Click

        If IsValid Then
            IsExcel = False
            SetReport(False)
        Else
            upnlTitle.Update()
            Exit Sub
        End If

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        mMachineList = Nothing
        mAssemblyList = Nothing
        mServiceTypeList = Nothing
        mInspectionTypeList = Nothing
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub

    Private Sub txtFromDate_TextChanged(sender As Object, e As EventArgs)
        AOdate = txtFromDate.Text.ToString
        If AOnDate.Equals(AOdate) Then
        Else
            SetComboOfMachine(AOdate)
        End If
    End Sub

    Private Sub cmbAircraft_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbAircraft.SelectedIndexChanged
        If cmbAircraft.SelectedIndex = 0 Then
            lblAssembly.Enabled = False
            cmbAssembly.Enabled = False
            AircraftIndex = cmbAircraft.SelectedIndex
            Session("AircraftIndex") = AircraftIndex
        Else
            lblAssembly.Enabled = True
            cmbAssembly.Enabled = True

            Dim mAssemblylist As AssemblyList
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, txtFromDate.Text.ToString, "(All)", True)

            Session("mAssemblyList") = mAssemblylist
            cmbAssembly.DataSource = mAssemblylist
            cmbAssembly.DataBind()

            AircraftIndex = cmbAircraft.SelectedIndex
            Session("AircraftIndex") = AircraftIndex

        End If
        If cmbAircraft.Enabled = True Then
            SetFocus(cmbAircraft)
        End If
        SetTypeCombo()
        DataFieldBind() 'Added By Utkarsh ON 12-Sep-2013 FOR BA11092013
        FillTypeCombo()
        upnlTitle.Update()
    End Sub

    Private Sub cmbType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbType.SelectedIndexChanged
        Try
            Dim j As Integer
            For j = 0 To cmbType.Items.Count - 1

                'ListServiceType Enabled
                If cmbType.Items(j).Selected = True And (cmbType.Items(j).Text = "Service" Or cmbType.Items(j).Text = "MPD") Then
                    ListServiceType.Enabled = cmbType.Items.Item(cmbType.SelectedIndex).Selected
                    hdnService.Value = cmbType.Items.Item(cmbType.SelectedIndex).Selected
                    For i As Integer = 0 To ListServiceType.Items.Count - 1
                        ListServiceType.Items.Item(i).Selected = ListServiceType.Enabled
                    Next
                    upnlServiceType.Update()
                End If

                'ListInspectionType Enabled
                If cmbType.Items(j).Selected = True And cmbType.Items(j).Text = "Inspection" Then
                    ListInspectionType.Enabled = cmbType.Items.Item(cmbType.SelectedIndex).Selected
                    hdnInspection.Value = cmbType.Items.Item(cmbType.SelectedIndex).Selected
                    For i As Integer = 0 To ListInspectionType.Items.Count - 1
                        ListInspectionType.Items.Item(i).Selected = ListInspectionType.Enabled
                    Next
                    upnlInspectionType.Update()

                End If

            Next

            Dim k As Integer
            For k = 0 To cmbType.Items.Count - 1

                'cmbService Disabled
                If cmbType.Items(k).Selected = False And (cmbType.Items(k).Text = "Service" Or cmbType.Items(k).Text = "MPD") Then
                    ListServiceType.Enabled = False
                    hdnService.Value = False
                    For l As Integer = 0 To ListServiceType.Items.Count - 1
                        ListServiceType.Items.Item(l).Selected = False
                    Next
                    upnlServiceType.Update()
                End If

                'cmbInspection Disabled
                If cmbType.Items(k).Selected = False And cmbType.Items(k).Text = "Inspection" Then
                    ListInspectionType.Enabled = False
                    hdnInspection.Value = False
                    For l As Integer = 0 To ListInspectionType.Items.Count - 1
                        ListInspectionType.Items.Item(l).Selected = False
                    Next
                    upnlInspectionType.Update()
                End If
            Next
            upnlImgBtn.Update()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "disableEnable", "disableEnable();", True)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ListServiceType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListServiceType.SelectedIndexChanged
        Session("SerIndex") = SerIndex
    End Sub

    Private Sub ListInspectionType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListInspectionType.SelectedIndexChanged
        Session("InspIndex") = InspIndex
    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click

        Dim PeriodColumnsForExportToExcel As New List(Of String)

        Try

            If IsValid = True Then

                IsExcel = True
                ReportMaintenanceDetails = Nothing
                Report = Nothing
                ReportStatusList = Nothing
                Dim da As New ObjectAdapter
                Dim ds As New dsReportMaintenanceDetail
                ReportStatusList = New rptStatusList
                ReportMaintenanceDetails = New ReportMaintenanceDetailList
                SetReport(IsExcel:=True)

                If ReportMaintenanceDetails.Count = 0 Then

                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound,
                                    MSGBox.Message_text.NoRecordFound,
                                    "There is no record for this search criteria",
                                    MsgBoxStyle.OkOnly,
                                    "")

                    Exit Sub

                End If

                ds.Clear()
                da.Fill(ds, "ExcelReportMaintenanceDetailList", ReportMaintenanceDetails)
                da.Fill(ds, "ExcelReport", Report)

                Dim columnToRemove As String() = {"ID", "Code", "Name", "Model", "EstDate", "SerialNo", "MonitorType", "Freq2", "Freq3", "ElapsedTime1",
                                                  "ElapsedTime2", "RemainingTime1", "RemainingTime2", "DueAsof1", "DueAsof2", "AssemblySerialNo",
                                                  "ComponentInfo", "RegNo", "AssemblyType", "SinceNew", "SinceNew1", "DoneAt", "DoneAt1", "AssemblyModel",
                                                   "MinimumRemainingValue", "AssemblyTypeID", "MaintenanceEvent", "InstalledAt", "InstalledAt1",
                                                    "InstalledAt2", "TSO1", "TSO2", "RemoveAt1", "RemoveAt2", "ModificationNumber", "DoneWONo", "DetailID",
                                                    "Applicability", "ApplicabilityForExcel", "ComplianceRequirement", "AssemblyDueAsof", "AssemblyDueAsof1",
                                                    "AssemblyDueAsof2", "Extension1", "Extension2", "ExtensionDate", "ApprovalRemark", "RequiredManHours",
                                                    "Customer", "SupersededByADNumber", "IssueDate", "IsApplicable", "MaintenanceTypeID", "MaintenanceTypeName",
                                                    "IsLater", "DueStatus", "ModelMonitorModCode", "StatusTypeName", "WONumber", "StatusMasterID", "StatusID",
                                                    "TypeID", "CompStatusID", "AssemblyStatusID", "DocumentTypeForID", "MaintenanceOn", "MaintenanceInformation",
                                                    "MaintenanceInfo", "Frequency", "SinceNewAll", "ElapsedAll", "DoneAtAll", "ExtensionAll", "DueAsofAll",
                                                    "AssDueAsofAll", "RemainingTimeAll", "LogBook", "DoneOnValue", "DoneOnDate", "RemoveAt", "ATACode", "InstalledAtDate",
                                                    "RemoveAtDate", "TSO", "TSN", "DoneONValueForAssembly", "RecordID", "MachineID", "ModelID", "IsMaster",
                                                    "DiffCompInstDoneOnValue", "EffectiveFromAll", "MaintenanceOnExcel", "ReferenceForExcel", "MaintenanceInformationForExcel",
                                                    "Description", "MaintenanceInfoExcel", "FrequencyExcel", "SinceNewAllExcel", "ElapsedAllExcel", "EffectiveFromAllExcel",
                                                    "DoneAtAllExcel", "ExtensionAllExcel", "DueAsofAllExcel", "AssDueAsofAllExcel",
                                                    "RemainingTimeAllExcel", "EROQtyNosForMaterialMgmtReport", "POQtyNosForMaterialMgmtReport",
                                                    "PONosForMaterialMgmtReport", "POQtyForMaterialMgmtReport", "ERONosForMaterialMgmtReport",
                                                    "EROQtyForMaterialMgmtReport", "UnserviceableStockQty", "ServiceableStockQty", "BinCardTotalQty", "Area",
                                                    "Zone", "NoteForExcel", "MaintenanceActivityType", "LinkedMaintenanceActivityCount", "ModelEstimatedManHours",
                                                    "SourceDoc", "IsRII", "ReqNumber", "MaintenanceInformationExcel", "Freq1", "Position", "RemainingTime", "DueAsOf",
                                                    "ElapsedTime", "TimeSinceNew", "SinceNew2", "Extension", "HoursFreq", "CyclesFreq",
                                                    "DaysMnthsYrsName", "DaysMnthsYrsValue", "LandingsFreq", "HoursDoneOnValue", "CyclesDoneOnValue",
                                                    "DaysMnthsYrsDoneOnValue", "LandingsDoneOnValue", "Manufacturer", "InstallationWONo",
                                                    "InstallationRemark", "InstallationDoneBy", "InstPlace", "TSNHours", "SinceNewDate",
                                                    "SinceNewLandings", "CSNCycles", "InstCompHours", "InstCompStartDate", "InstCompLandings",
                                                    "InstCompCycles", "AssemblyInstHours", "AssemblyInstStartDate", "AssemblyInstLandings",
                                                    "AssemblyInstCycles", "PartMonitorCode", "PartDesc", "MonitorTypeWithCode", "DataColumn1",
                                                    "PartNoSerialNoforExcel", "TSO1ForExcel", "TSOForExcel", "InstalledAtForExcel", "Freq1ForExcel",
                                                    "TSNForExcel", "DoneOnValueForExcel", "RemainingTimeForExcel", "DueAsOfForExcel", "TaskNo", "Reference",
                                                     "Skill", "SkillID", "DescriptionForExcel"}

                For i As Integer = 0 To columnToRemove.Length - 1

                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains(columnToRemove(i)) Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns.Remove(columnToRemove(i))
                    End If

                Next
                ds.Tables("ExcelReportMaintenanceDetailList").Columns.Remove("EstimatedDate")

                If AppSettings("ClientCode") = "FIT" Then 'Added by Saylee on 8-Jul-2025, FLYPAL-2544 FIT: New requirement and suggestion in the LDND Excel format
                    columnToRemove = {"TaskReferenceForExcel",
                        "Note",
                        "MonitorTypeCode",
                        "DueAsOfAirframeForExcel",
                        "WONoExcel", "Remark", "MethodOfCompliance"
                        }

                    For i As Integer = 0 To columnToRemove.Length - 1

                        If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains(columnToRemove(i)) Then
                            ds.Tables("ExcelReportMaintenanceDetailList").Columns.Remove(columnToRemove(i))
                        End If

                    Next

                End If


                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("ATAChapter") Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("ATAChapter").ColumnName = "ATA Chapter"
                End If

                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("TaskReferenceForExcel") Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("TaskReferenceForExcel").ColumnName = "AMM Reference"
                End If

                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("DescriptionSourceDocForExcel") Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("DescriptionSourceDocForExcel").ColumnName = "Task Description"
                End If

                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("Note") Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("Note").ColumnName = "Manual Ref.(i.e. AMM Ref.)"
                End If

                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("MonitorTypeCode") Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("MonitorTypeCode").ColumnName = "Type"
                End If

                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("PartNo") Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("PartNo").ColumnName = "Comp P/N"
                End If

                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("CompSerialNo") Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("CompSerialNo").ColumnName = "Comp S/N"
                End If

                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("ThresholdAccordingToTypeIDForExcel") Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("ThresholdAccordingToTypeIDForExcel").ColumnName = "Threshold"
                End If

                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("FrequencyAccordingToTypeIDForExcel") Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("FrequencyAccordingToTypeIDForExcel").ColumnName = "Frequency"
                End If

                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("DoneAt2") Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("DoneAt2").ColumnName = "Last Done At"
                End If

                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("DueAsOfAssemblyOrCompForExcel") Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("DueAsOfAssemblyOrCompForExcel").ColumnName = "Next Due At"
                End If

                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("DueAsOfAirframeForExcel") Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("DueAsOfAirframeForExcel").ColumnName = "Due As Of Airframe"
                End If

                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("RemainingForExcel") Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("RemainingForExcel").ColumnName = "Remaining"
                End If

                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("WONoExcel") Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("WONoExcel").ColumnName = "Last Compliance W/O Number"
                End If

                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("TaskNoExcel") Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("TaskNoExcel").ColumnName = "Task No."
                End If

                If AppSettings("ClientCode") = "FIT" Then 'ClientCode Added by Saylee on 8-Jul-2025, FLYPAL-2544 FIT: New requirement and suggestion in the LDND Excel format
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("Task No.").SetOrdinal(0)
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("ATA Chapter").SetOrdinal(1)

                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("Task Description").SetOrdinal(2)
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("Threshold").SetOrdinal(3)
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("Frequency").SetOrdinal(4)
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("Last Done At").SetOrdinal(5)
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("Next Due At").SetOrdinal(6)

                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("Remaining").SetOrdinal(7)

                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("Comp P/N").SetOrdinal(8)
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("Comp S/N").SetOrdinal(9)

                Else
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("Task No.").SetOrdinal(0)
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("ATA Chapter").SetOrdinal(1)
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("AMM Reference").SetOrdinal(2)
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("Manual Ref.(i.e. AMM Ref.)").SetOrdinal(3)
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("Type").SetOrdinal(4)
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("Task Description").SetOrdinal(5)
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("Threshold").SetOrdinal(6)
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("Frequency").SetOrdinal(7)
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("Last Done At").SetOrdinal(8)
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("Next Due At").SetOrdinal(9)
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("Due As Of Airframe").SetOrdinal(10)
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("Remaining").SetOrdinal(11)
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("Last Compliance W/O Number").SetOrdinal(12)
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("Comp P/N").SetOrdinal(13)
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("Comp S/N").SetOrdinal(14)
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("Remark").SetOrdinal(15)
                End If



                Dim columnToRemoveCriteria As String() = {"ReportDate", "ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite",
                                                           "ReportName", "SearchStr2", "SearchStr3", "SearchStr4", "SearchStr6", "SearchStr7", "ProductVersion",
                                                           "SINote", "CurrencyName", "CurrencySymbol", "SearchStr10", "SearchStr11", "SearchStr14", "SearchStr15",
                                                           "SearchStr16", "SearchStr17", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23",
                                                           "SearchStr24", "SearchStr25", "ShortName", "SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29",
                                                           "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35",
                                                           "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40", "SearchStr41",
                                                           "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47",
                                                           "SearchStr48", "SearchStr49", "SearchStr50", "SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55", "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60", "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65", "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70", "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95", "SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}

                For i As Integer = 0 To columnToRemoveCriteria.Length - 1

                    If ds.Tables("ExcelReport").Columns.Contains(columnToRemoveCriteria(i)) Then
                        ds.Tables("ExcelReport").Columns.Remove(columnToRemoveCriteria(i))
                    End If

                Next

                If ds.Tables("ExcelReport").Columns.Contains("SearchStr1") Then
                    ds.Tables("ExcelReport").Columns("SearchStr1").ColumnName = "AsOnDate"
                End If
                If ds.Tables("ExcelReport").Columns.Contains("SearchStr12") Then
                    ds.Tables("ExcelReport").Columns("SearchStr12").ColumnName = "Services"
                End If
                If ds.Tables("ExcelReport").Columns.Contains("SearchStr13") Then
                    ds.Tables("ExcelReport").Columns("SearchStr13").ColumnName = "Inspections"
                End If
                If ds.Tables("ExcelReport").Columns.Contains("SearchStr8") Then
                    ds.Tables("ExcelReport").Columns("SearchStr8").ColumnName = "Reg No."
                End If
                If ds.Tables("ExcelReport").Columns.Contains("SearchStr9") Then
                    ds.Tables("ExcelReport").Columns("SearchStr9").ColumnName = "Assembly"
                End If
                If ds.Tables("ExcelReport").Columns.Contains("SearchStr5") Then
                    ds.Tables("ExcelReport").Columns("SearchStr5").ColumnName = "ATA Chapter"
                End If
                If ds.Tables("ExcelReport").Columns.Contains("SearchStr18") Then
                    ds.Tables("ExcelReport").Columns("SearchStr18").ColumnName = "Format"
                End If

                Dim dataview As DataView = ds.Tables("ExcelReportMaintenanceDetailList").DefaultView
                'dataview.Sort = "Task Reference"
                dataview.Sort = "Task No."

                ds.Tables("ExcelReportMaintenanceDetailList").TableName = "Last Done Next Due"

                ds.Tables("ExcelReport").TableName = "Searching Criteria"
                Session("DataTableToBeFormattedForExportToExcel") = "Last Done Next Due"
                Dim dsNew As New DataSet
                dsNew.Clear()

                dsNew.Merge(ds.Tables("Searching Criteria"))
                dsNew.Merge(dataview.ToTable())

                PeriodColumnsForExportToExcel.AddRange(New String() {"Frequency", "Threshold", "Remaining", "Next Due At", "Due As Of Airframe", "DoneOn Value", "Last Done At", "Done At"})
                Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
                Session("ExcelFileName") = "Last Done Next Due"
                Session("dsNew") = dsNew
                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "openFilel",
                                                    "openFile();",
                                                    True)

                'Added by Prashant on 19-Jan-2021
                MarkLog(Action.Print,
                        "LD / ND",
                        "Export To Excel " + mCofASearchingCriteria,
                        ErrorType.NoError,
                        Guid.Empty,
                        EventLogID)

            End If

        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub btnByMail_Click(sender As Object, e As EventArgs) Handles btnByMail.Click
        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        '  Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail

        Session("UserEmailID") = mModuleList.Item("LDND").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("LDND").SendCCMailID
        '--------------------------
        Dim Str As String
        If IsValid Then
            Str = "OpenByMaiWindow();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
        Else
            upnlTitle.Update()
            Exit Sub
        End If
    End Sub

    Private Sub hdnimgLogBtnSendMail_Click(sender As Object, e As EventArgs) Handles hdnimgLogBtnSendMail.Click
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

    'Added By Vikrant On 08-Dec-2020 For ALL08122020-1
    Private Sub cmbFormat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbFormat.SelectedIndexChanged
        ControlVisibility()
    End Sub
    'End

#End Region

End Class