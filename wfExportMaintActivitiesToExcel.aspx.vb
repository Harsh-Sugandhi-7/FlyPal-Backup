

'CREATED By : Saylee
'Dated      : 11-May-2021

Imports System.Linq
Imports System.Collections.Generic
Imports System.Text




Public Class wfExportMaintActivitiesToExcel
    Inherits System.Web.UI.Page



#Region " Variable Declaration "
    Dim ReportMaintenanceDetails As New ReportMaintenanceDetailList

    Dim FromDate As String

    Dim Directive As String
    Dim Aircraft As String
    Dim AircraftIndex As Integer
    Dim AssemblyName As String
    Dim Assembly1 As String
    Dim AssemblyType As String
    Dim ModelName As String
    Dim ModTypeName As String
    Dim MachineName As String
    Dim ModelID As String
    Dim Type As String = ""
    Private mMachineList As MachineList
    Dim mMachineNameValueList As MachineNameValueList
    Public mModificationTypeList As ModelMonitorModTypeList
    Public mInspectionTypesList As ModelMonitorInspTypeList
    Public mServiceTypeList As PartMonitorServiceTypeList

    Private mModTypeList As ModTypeList
    Private mAssemblyList As AssemblyList

    Dim IsSerSelect As Boolean = False
    Dim IsModSelect As Boolean = False
    Dim IsInsSelect As Boolean = False

    Dim ServiceTypeID(50) As Integer
    Dim InspectionTypeID(50) As Integer
    Dim ModificationTypeID As String

    Dim ArrCnt As Integer = 0

    Public EventLogID As Guid
    Public EventLogDetail As String = ""

    Dim ServiceTypeName(50) As String
    Dim InspectionTypeName(50) As String
    Dim ModificationTypeName(50) As String
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    Dim IsExcel As Boolean = False
    Dim Report As Integer = 0

    Private ATAChapter As String = ""
    Private RegNo As String
    Private Model As String
    Private AssemblySerialNo As String
    Private PartNo As String
    Private CompSerialNo As String
    Private Position As String
    Private MonitorTypeCode As String = ""
    Private MonitorType As String = ""
    Private Note As String = ""
    Private Description As String = ""
    Private EstimatedDate As String = ""
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
    Private DueAsof2 As String
    Private AssemblyModel As String
    Private AssemblyTypeID As Integer
    Private Code As String
    Private ATACode As Integer = 0
    Private InstalledAt As String
    Private InstalledAt1 As String
    Private InstalledAt2 As String
    Private TSO As String
    Private TSN As String
    Private TSO1 As String
    Private TSO2 As String
    Private RemoveAt As String
    Private RemoveAt1 As String
    Private RemoveAt2, SerialNoPostion, DoneRemrk As String
    Private InstalledAtDate As SmartDate = New SmartDate(True)
    Private RemoveAtDate As SmartDate = New SmartDate(True)
    Private DoneOnValue As String
    Private DoneOnDate As SmartDate = New SmartDate(True)
    Dim AirframeDueAsof As String
    Private Number As String
    Private Reference As String
    Private Applicability As String
    Private ComplianceRequirement As String
    Private ModelMonitorModCode As String
    Private IssueDate As SmartDate = New SmartDate(True)
    Private IsApplicable As Boolean
    Dim mHoursFreq, mCyclesFreq, mLandingsFreq, mDaysMnthsYrsName, mDaysMnthsYrsValue As String
    Dim mHoursDoneOnValue, mCyclesDoneOnValue, mLandingsDoneOnValue, mDaysMnthsYrsDoneOnValue, mPartDesc, mRequiredManHours, mPartMonitorModCode As String
    Dim mPartMonitorServiceCode As String = ""
    Dim mIsLater As Boolean = False
    Dim ReportLabel As String
    Dim ReportStatusList As New rptStatusList
    Dim ReportData As ReportData
    Dim AssemblyID As Guid
    Dim searchstr7 As String = ""
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        mAssemblyList = CType(Session("mAssemblyList"), AssemblyList)
        mServiceTypeList = CType(Session("mServiceTypeList"), PartMonitorServiceTypeList)
        mInspectionTypesList = CType(Session("mInspectionTypesList"), ModelMonitorInspTypeList)
        mModificationTypeList = CType(Session("mModificationTypeList"), ModelMonitorModTypeList)
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
    End Sub
    Private Sub SetSession()
        Session("mMachineList") = mMachineList
        Session("mAssemblyList") = mAssemblyList
        Session("mServiceTypeList") = mServiceTypeList
        Session("mInspectionTypesList") = mInspectionTypesList
        Session("mModificationTypeList") = mModificationTypeList
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfExportMaintActivitiesToExcel.aspx?" Then
            Session.Remove("mMachineList")
            Session.Remove("mAssemblyList")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType, "focusscript", str)
    End Sub
    Private Sub Display()
        lblAircraft1.Visible = True
        lblDateRangeFrom.Visible = True
        lblAssembly1.Visible = True
    End Sub
    Private Sub SetValues()
        If (cmbAircraft.SelectedItem.Text = "(All)") Or (cmbAircraft.SelectedItem.Text = "<SELECT>") Then
            MachineName = "{00000000-0000-0000-0000-000000000000}"
            AssemblyName = "{00000000-0000-0000-0000-000000000000}"
            Assembly1 = ""
            lblAssembly1.Text = ""
        Else
            MachineName = cmbAircraft.SelectedValue.ToString
            If cmbAssembly.SelectedItem.Text = "<SELECT>" Or cmbAssembly.SelectedItem.Text = "(All)" Then
                AssemblyName = "{00000000-0000-0000-0000-000000000000}"
                Assembly1 = ""
                AssemblyType = "(All)"
                lblAssembly1.Text = "Assembly Name  : All"
            Else
                mAssemblyList = Session("mAssemblyList")
                AssemblyType = mAssemblyList(cmbAssembly.SelectedIndex).AssemblyType
                AssemblyName = cmbAssembly.SelectedValue.ToString
                Assembly1 = cmbAssembly.SelectedItem.Text
                lblAssembly1.Text = "Assembly Name : " & Assembly1
            End If
        End If

        If Not IsDate(txtFromDate.Text) Then
            FromDate = ""
        Else
            FromDate = txtFromDate.Text.ToString
        End If
       

        Aircraft = IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "")

        lblDateRangeFrom.Text = "AsonDate : " & txtFromDate.Text.Trim


        lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "All")



        'Set Service/Inspection/Directive checkbox list values
        'Service
        If chkService.Checked Then
            IsSerSelect = True
            Type = "Services: "
            ServiceTypeID = (From c As System.Web.UI.WebControls.ListItem In ListServiceType.Items
                         Where c.Selected = True
                         Select CInt(c.Value)).ToArray
            ServiceTypeName = (From c As System.Web.UI.WebControls.ListItem In ListServiceType.Items
                         Where c.Selected = True
                         Select (c.Text)).ToArray

            For i As Integer = 0 To ServiceTypeName.Length - 1
                If i = InspectionTypeName.Length - 1 Then
                    Type = Type + ServiceTypeName(i)
                Else
                    Type = Type + ServiceTypeName(i) + " , "
                End If
            Next
        End If
        'Inspection
        If chkInspection.Checked Then
            IsInsSelect = True
            Type = "Inspections: "
            InspectionTypeID = (From c As System.Web.UI.WebControls.ListItem In ListInspectionType.Items
                         Where c.Selected = True
                         Select CInt(c.Value)).ToArray

            InspectionTypeName = (From c As System.Web.UI.WebControls.ListItem In ListInspectionType.Items
                         Where c.Selected = True
                         Select (c.Text)).ToArray

            For i As Integer = 0 To InspectionTypeName.Length - 1
                If i = InspectionTypeName.Length - 1 Then
                    Type = Type + InspectionTypeName(i)
                Else
                    Type = Type + InspectionTypeName(i) + " , "
                End If
            Next
        End If
        'Directive
        If chkDirective.Checked Then
            Dim tmpModificationTypeID As New StringBuilder
            IsModSelect = True
            Type = "Directives: "

            For i As Integer = 0 To ListDirectiveType.Items.Count - 1
                Dim appval As String = ""
                If i = ListDirectiveType.Items.Count - 1 Then
                    appval = ""
                Else
                    appval = ","
                End If

                If ListDirectiveType.Items(i).Selected = True Then
                    tmpModificationTypeID = tmpModificationTypeID.Append(ListDirectiveType.Items(i).Value + appval)
                End If

            Next

            'tmpModificationTypeID = tmpModificationTypeID.Append((From c As System.Web.UI.WebControls.ListItem In chkListDirectiveType.Items
            '             Where c.Selected = True
            '            Select CStr(c.Value) + ",").ToList)

            ModificationTypeName = (From c As System.Web.UI.WebControls.ListItem In ListDirectiveType.Items
                       Where c.Selected = True
                       Select (c.Text)).ToArray

            If tmpModificationTypeID.Length > 0 Then
                '' ModificationTypeID = IIf(tmpModificationTypeID.Length > 0, tmpModificationTypeID.ToString.Substring(0, tmpModificationTypeID.Length - 1), "")
                ModificationTypeID = IIf(tmpModificationTypeID.Length > 0, tmpModificationTypeID.ToString.Substring(0, tmpModificationTypeID.Length), "")
            Else
                ModificationTypeID = ""
            End If

            For i As Integer = 0 To ModificationTypeName.Length - 1
                If i = ModificationTypeName.Length - 1 Then
                    Type = Type + ModificationTypeName(i)
                Else
                    Type = Type + ModificationTypeName(i) + " , "
                End If
            Next


        End If
        EventLogDetail = lblDateRangeFrom.Text + " , " + lblAircraft1.Text + " , " + lblAssembly1.Text + Type
    End Sub
    Private Sub ResetValues()
        MachineName = "{00000000-0000-0000-0000-000000000000}"
        IsSerSelect = False
        IsInsSelect = False
        IsModSelect = False
        ServiceTypeID(0) = 0
        InspectionTypeID(0) = 0
        ModificationTypeID = ""
        AssemblyName = "{00000000-0000-0000-0000-000000000000}"
    End Sub
    Public Function ReportAssemblyDetail(Optional ByVal IsForTransfer As Boolean = False) As ReportMaintenanceDetailList
        Dim ObjMachine As MachineInfo
        Dim ObjAssemblyStatus As AssemblyStatusInfo

        Dim ObjAssemblyMonitorModStatus As AssemblyMonitorModStatusInfo
        Dim ObjAssemblyMonitorModStatusPeriod As AssemblyMonitorModStatusPeriodInfo
     
        Dim ObjAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatusInfo
        Dim ObjAssemblyMonitorServiceStatusPeriod As AssemblyMonitorServiceStatusPeriodInfo

        Dim ObjAssemblyMonitorInspStatus As AssemblyMonitorInspStatusInfo
        Dim ObjAssemblyMonitorInspStatusPeriod As AssemblyMonitorInspStatusPeriodInfo

        Dim mReportMaintenanceDetail As New ReportMaintenanceDetail

        Dim ModTypeIds As New StringBuilder
        Dim InspTypeIds As New StringBuilder
        Dim ServiceTypeIds As New StringBuilder

        If chkDirective.Checked Then
            For K As Integer = 0 To ListDirectiveType.Items.Count - 1
                If ListDirectiveType.Items.Item(K).Selected Then
                    ModTypeIds.Append(ListDirectiveType.Items.Item(K).Value + ",")
                End If
            Next

        End If
        If chkInspection.Checked Then
            For P As Integer = 0 To InspectionTypeID.Count - 1
                    InspTypeIds.Append(InspectionTypeID(P).ToString + ",")
            Next
        End If


        If chkService.Checked Then
            For P As Integer = 0 To ServiceTypeID.Count - 1
                ServiceTypeIds.Append(ServiceTypeID(P).ToString + ",")
            Next
        End If

        mMachineList = MachineList.GetMachineListMonitoringStatus(FromDate, MachineName, , , , , , , , , , False, True, , _
                                                                  AssemblyName, ShowInCofA:=False, MonitoringInspRequired:=True, MonitoringServiceRequired:=True, MonitoringModRequired:=True, _
                                                                  IsAssemblyRemoved:=False, IsCompRemoved:=False, IsComplied:=True, _
                                                                  IsAverageRequired:=True, AverageMonths:=6, CompMonitoringModRequired:=False, _
                                                                  SkipIsForInventoryAircarft:=True, ModTypeIDs:=ModTypeIds.ToString.TrimEnd(","), MonitorInspTypeIDs:=InspTypeIds.ToString.TrimEnd(","), MonitorServiceTypeIDs:=ServiceTypeIds.ToString.TrimEnd(",")) 'IsAverageRequired:=mIsAverageRequired, ByPerDayLimit:=mByPerDayLimit, PerdayLimits:=mPerDayLimits, SkipIsForInventoryAircarft:=True)
        Dim LHLabel2 As String = ""
        Dim LHData2 As String = ""
        Dim RHLabel1 As String = ""
        Dim RHData1 As String = ""
        Dim RHLabel2 As String = ""
        Dim RHData2 As String = ""
        Dim RHData3 As String = ""
        Dim SearchStr8 As String = ""
        Dim Periodcount As Integer
        Dim Count, Count1 As Integer
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
                If cmbAssembly.SelectedIndex = 0 Then
                    If ObjAssemblyStatus.AssemblyTypeID = 1 Then
                        Periodcount = ObjAssemblyStatus.AssemblyStatusPeriodList.Count()
                        For Count1 = 0 To Periodcount - 1
                            If ObjAssemblyStatus.AssemblyStatusPeriodList(Count1).PeriodID <> 2 Then
                                RHLabel1 = CType(IIf(RHLabel1 = "", RHLabel1, RHLabel1 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count1).PeriodName
                                RHData1 = CType(IIf(RHData1 = "", RHData1, RHData1 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count1).PeriodID, "").AssemblyCurrentValue
                            Else
                                RHLabel1 = CType(IIf(RHLabel1 = "", RHLabel1, RHLabel1 + vbNewLine), String) + "Mfg. Date"
                                RHData1 = CType(IIf(RHData1 = "", RHData1, RHData1 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count1).PeriodID, "").AssemblyStartValueFormatted
                                SearchStr8 = "<b>Date of Manufacture: </b>" + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count1).PeriodID, "").AssemblyStartValueFormatted
                            End If
                        Next
                        RHLabel2 = ObjAssemblyStatus.SerialNo
                        RHData2 = ObjAssemblyStatus.AssemblyType + " " + "Model"
                        RHData3 = ObjAssemblyStatus.Model
                    End If
                Else
                    Periodcount = ObjAssemblyStatus.AssemblyStatusPeriodList.Count()
                    For Count1 = 0 To Periodcount - 1
                        If ObjAssemblyStatus.AssemblyStatusPeriodList(Count1).PeriodID <> 2 Then
                            RHLabel1 = CType(IIf(RHLabel1 = "", RHLabel1, RHLabel1 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count1).PeriodName
                            RHData1 = CType(IIf(RHData1 = "", RHData1, RHData1 + vbNewLine + " : "), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count1).PeriodID, "").AssemblyCurrentValue
                        Else
                            RHLabel1 = CType(IIf(RHLabel1 = "", RHLabel1, RHLabel1 + vbNewLine), String) + "Mfg. Date"
                            RHData1 = CType(IIf(RHData1 = "", RHData1, RHData1 + vbNewLine + " : "), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count1).PeriodID, "").AssemblyStartValueFormatted
                            If ObjAssemblyStatus.AssemblyTypeID = 1 Then SearchStr8 = "<b>Date of Manufacture: </b>" + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count1).PeriodID, "").AssemblyStartValueFormatted
                        End If
                    Next
                    If ObjAssemblyStatus.Position = "" Then
                        RHLabel2 = ObjAssemblyStatus.SerialNo
                    Else
                        RHLabel2 = ObjAssemblyStatus.SerialNo + "(" + ObjAssemblyStatus.Position + ")"
                    End If
                    RHData2 = ObjAssemblyStatus.AssemblyType + " " + "Model"
                    RHData3 = ObjAssemblyStatus.Model
                End If
                '---------------------------------------------------------------------------------------------------------
                If ObjAssemblyStatus.Position = "" Then
                    SerialNoPostion = ObjAssemblyStatus.SerialNo
                Else
                    SerialNoPostion = ObjAssemblyStatus.SerialNo + "(" + ObjAssemblyStatus.Position + ")"
                End If
                searchstr7 = ObjMachine.Owner.ToString 'Added By Utkarsh On 08-Apr-2011 '"Owner/Operator :- " + 
                AssemblyID = ObjAssemblyStatus.AssemblyID
                ReportStatusList.Add(New rptStatus(AssemblyID.ToString, ObjAssemblyStatus.AssemblyTypeID, , "Reg No.", ObjMachine.RegNo, ObjAssemblyStatus.AssemblyType + " " + "Model", ObjAssemblyStatus.Model, _
                   "Serial No.", SerialNoPostion, "Due As of " & ObjAssemblyStatus.AssemblyType, , , , , , , , , , , , , LHLabel2, LHData2, RHLabel1, RHData1, RHLabel2, RHData2, RHData3, RHData10:=SearchStr8))
            Next
        Next

        '''' mModificationTypeList = ModelMonitorModTypeList.GetModelMonitorModTypeList()

        ''''For i As Integer = 0 To mModificationTypeList.Count - 1
        ''''    'Added by Prashant 13-Jun-2018 Suhan13062018
        ''''    If ModShortName = "" Then
        ''''        ModShortName = IIf(Not mModificationTypeList(i, "").CodeType Is Nothing, mModificationTypeList(i, "").CodeType, "")
        ''''    Else
        ''''        ModShortName = ModShortName + IIf(Not mModificationTypeList(i, "").CodeType Is Nothing, ", " + mModificationTypeList(i, "").CodeType, "")
        ''''    End If
        '''' Next
        'End Added by Prashant 13-Jun-2018 Suhan13062018

        For Each ObjMachine In mMachineList
            For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList

                If chkDirective.Checked Then


                    For Each ObjAssemblyMonitorModStatus In ObjAssemblyStatus.AssemblyMonitorModStatusList
                        ATAChapter = ObjAssemblyMonitorModStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorModStatus.ATANomenclature

                        Description = ObjAssemblyMonitorModStatus.Description
                        Position = ObjAssemblyStatus.Position
                        MonitorTypeCode = ObjAssemblyMonitorModStatus.Code
                        MonitorType = ObjAssemblyMonitorModStatus.MonitorType

                        AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID
                        AssemblyModel = ObjAssemblyStatus.Model
                        AssemblySerialNo = ObjAssemblyStatus.SerialNo
                        Freq1 = ""
                        ElapsedTime = ""
                        RemainingTime = ""
                        DueAsof = ""
                        DoneOnValue = ""
                        EstimatedDate = ""
                        DoneOnDate.Text = ""

                        Code = ObjAssemblyMonitorModStatus.ModelMonitorModCode

                        'Added By Saylee on 28-Apr-2021 for PreDefined transferred excel sheet
                        mHoursFreq = ""
                        mCyclesFreq = ""
                        mLandingsFreq = ""
                        mDaysMnthsYrsName = ""
                        mDaysMnthsYrsValue = ""
                        mHoursDoneOnValue = ""
                        mCyclesDoneOnValue = ""
                        mLandingsDoneOnValue = ""
                        mDaysMnthsYrsDoneOnValue = ""
                        mRequiredManHours = ObjAssemblyMonitorModStatus.RequiredManHours
                        mIsLater = ObjAssemblyMonitorModStatus.IsLater
                        ATACode = ObjAssemblyMonitorModStatus.ATACode
                        ''**************************************************************

                        If ObjAssemblyMonitorModStatus.IsApplicable = True And ObjAssemblyMonitorModStatus.IsCompleted = False Then
                            EstimatedDate = ObjAssemblyMonitorModStatus.EstimatedDateFormatted  'Added by Saylee on 10-June-2009
                        End If
                        IssueDate.Text = ObjAssemblyMonitorModStatus.IssueDateTextFormatted
                        IsApplicable = ObjAssemblyMonitorModStatus.IsApplicable
                        If ObjAssemblyMonitorModStatus.Number = "99-26-21" Or ObjAssemblyMonitorModStatus.Number = "99-08-23" Then
                            Dim a As Integer = 0
                        End If
                        For Each ObjAssemblyMonitorModStatusPeriod In ObjAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriodList
                            If ObjAssemblyMonitorModStatusPeriod.PeriodID = 2 Then
                                If Freq1 = "" Then
                                    If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True) Or (ObjAssemblyMonitorModStatus.IsApplicable = False) Then
                                        RemainingTime = ""
                                        DueAsof = ""
                                        'Commented & added by Saylee on 1-Nov-2018 , as per BINU Frequency should be visible
                                        'Freq1 = ""
                                        Freq1 = ObjAssemblyMonitorModStatusPeriod.FrequencyValueFormatted
                                        '***************************
                                        ElapsedTime = ""
                                    Else
                                        Freq1 = ObjAssemblyMonitorModStatusPeriod.FrequencyValueFormatted
                                        ElapsedTime = ObjAssemblyMonitorModStatusPeriod.ElapsedValueFormatted
                                        RemainingTime = ObjAssemblyMonitorModStatusPeriod.RemainingValueFormatted
                                        DueAsof = ObjAssemblyMonitorModStatusPeriod.DueOnValueFormatted
                                    End If

                                    DoneOnValue = DoneOnValue & ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted & IIf(IsExcel, Chr(10), vbCrLf)


                                    'DoneOnValue = DoneOnValue & ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted & IIf(IsExcel, Chr(10), vbCrLf)
                                    '=====================================================================
                                Else
                                    If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True) Or (ObjAssemblyMonitorModStatus.IsApplicable = False) Then
                                        RemainingTime = ""
                                        DueAsof = ""
                                        'Commented & added by Saylee on 1-Nov-2018 , as per BINU Frequency should be visible
                                        'Freq1 = ""
                                        Freq1 = Freq1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.FrequencyValueFormatted
                                        '*************************************
                                        ElapsedTime = ""
                                    Else
                                        Freq1 = Freq1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.FrequencyValueFormatted
                                        ElapsedTime = ElapsedTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.ElapsedValueFormatted
                                        RemainingTime = RemainingTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.RemainingValueFormatted
                                        DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.DueOnValueFormatted
                                    End If
                                    DoneOnValue = DoneOnValue + ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted & IIf(IsExcel, Chr(10), vbCrLf)
                                End If
                            Else
                                If Freq1 = "" Then
                                    If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True) Or (ObjAssemblyMonitorModStatus.IsApplicable = False) Then
                                        RemainingTime = ""
                                        DueAsof = ""
                                        'Commented & added by Saylee on 1-Nov-2018 , as per BINU Frequency should be visible
                                        'Freq1 = ""
                                        Freq1 = ObjAssemblyMonitorModStatusPeriod.FrequencyValue
                                        '*************************************
                                        ElapsedTime = ""
                                    Else
                                        Freq1 = ObjAssemblyMonitorModStatusPeriod.FrequencyValue
                                        ElapsedTime = ObjAssemblyMonitorModStatusPeriod.ElapsedValue
                                        RemainingTime = ObjAssemblyMonitorModStatusPeriod.RemainingValue
                                        DueAsof = ObjAssemblyMonitorModStatusPeriod.DueOnValue
                                    End If
                                    If ObjAssemblyMonitorModStatus.MonitorType = "No Frequency" Or ObjAssemblyMonitorModStatus.IsApplicable = False Then 'Added By Prashant 28-Sep-2018
                                        DoneOnValue = ""
                                    Else
                                        DoneOnValue = DoneOnValue + ObjAssemblyMonitorModStatusPeriod.DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf)
                                    End If
                                Else
                                    If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True) Or (ObjAssemblyMonitorModStatus.IsApplicable = False) Then
                                        RemainingTime = ""
                                        DueAsof = ""
                                        'Commented & added by Saylee on 1-Nov-2018 , as per BINU Frequency should be visible
                                        'Freq1 = ""
                                        Freq1 = Freq1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.FrequencyValue
                                        '*************************************
                                        ElapsedTime = ""
                                    Else
                                        Freq1 = Freq1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.FrequencyValue
                                        ElapsedTime = ElapsedTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.ElapsedValue
                                        RemainingTime = RemainingTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.RemainingValue
                                        DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.DueOnValue
                                    End If
                                    If ObjAssemblyMonitorModStatusPeriod.DoneOnValue = "" Then
                                        If ObjAssemblyMonitorModStatus.MonitorType = "No Frequency" Or ObjAssemblyMonitorModStatus.IsApplicable = False Then 'Added By Prashant 28-Sep-2018
                                            DoneOnValue = ""
                                        Else
                                            DoneOnValue = DoneOnValue & ObjAssemblyMonitorModStatusPeriod.DoneOnValue
                                        End If
                                    Else
                                        If ObjAssemblyMonitorModStatus.MonitorType = "No Frequency" Or ObjAssemblyMonitorModStatus.IsApplicable = False Then 'Added By Prashant 28-Sep-2018
                                            DoneOnValue = ""
                                        Else
                                            DoneOnValue = DoneOnValue + ObjAssemblyMonitorModStatusPeriod.DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf)
                                        End If
                                    End If
                                End If
                            End If

                            'Added By Saylee on 28-Apr-2021 for PreDefined transferred excel sheet
                            If ObjAssemblyMonitorModStatusPeriod.PeriodID = 1 Then
                                mHoursFreq = ObjAssemblyMonitorModStatusPeriod.FrequencyValue.ToString.Split(" ")(0)   'Added By Saylee on 28-Apr-2021 for PreDefined transferred excel sheet
                                mHoursDoneOnValue = ObjAssemblyMonitorModStatusPeriod.DoneOnValue.ToString.Split(" ")(0)
                            ElseIf ObjAssemblyMonitorModStatusPeriod.PeriodID = 2 Then
                                If ObjAssemblyMonitorModStatusPeriod.PeriodUnitID = 3 Then
                                    mDaysMnthsYrsValue = ObjAssemblyMonitorModStatusPeriod.FrequencyValue.ToString.Split(" ")(0)
                                    mDaysMnthsYrsName = "Days"
                                ElseIf ObjAssemblyMonitorModStatusPeriod.PeriodUnitID = 4 Then
                                    mDaysMnthsYrsValue = ObjAssemblyMonitorModStatusPeriod.FrequencyValue.ToString.Split(" ")(0)
                                    mDaysMnthsYrsName = "Months"
                                ElseIf ObjAssemblyMonitorModStatusPeriod.PeriodUnitID = 5 Then
                                    mDaysMnthsYrsValue = ObjAssemblyMonitorModStatusPeriod.FrequencyValue.ToString.Split(" ")(0)
                                    mDaysMnthsYrsName = "Years"
                                End If
                                mDaysMnthsYrsDoneOnValue = ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted.ToString.Split(" ")(0)
                            ElseIf ObjAssemblyMonitorModStatusPeriod.PeriodID = 3 Then
                                mCyclesFreq = ObjAssemblyMonitorModStatusPeriod.FrequencyValue.ToString.Split(" ")(0)
                                mCyclesDoneOnValue = ObjAssemblyMonitorModStatusPeriod.DoneOnValue.ToString.Split(" ")(0)
                            ElseIf ObjAssemblyMonitorModStatusPeriod.PeriodID = 7 Then
                                mLandingsFreq = ObjAssemblyMonitorModStatusPeriod.FrequencyValue.ToString.Split(" ")(0)
                                mLandingsDoneOnValue = ObjAssemblyMonitorModStatusPeriod.DoneOnValue.ToString.Split(" ")(0)
                            End If
                            '**************************




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

                        DoneOnDate.Text = ObjAssemblyMonitorModStatus.DoneOn


                        DoneRemrk = ObjAssemblyMonitorModStatus.DoneRemark
                        If DoneRemrk = "" Then
                            DoneRemrk = "----"
                        End If


                        Applicability = ObjAssemblyMonitorModStatus.Applicability
                        ''
                        If Applicability = "" Then
                            Applicability = "----"
                        End If
                        ''
                        ComplianceRequirement = ObjAssemblyMonitorModStatus.ComplianceRequirement
                        If ComplianceRequirement = "" Then
                            ComplianceRequirement = "----"
                        End If
                        ModelMonitorModCode = ObjAssemblyMonitorModStatus.ModelMonitorModCode
                        If ModelMonitorModCode = "" And IsForTransfer = False Then
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


                        If IsExcel Then
                            Dim ATACode As Integer = ObjAssemblyMonitorModStatus.ATACode
                            If ATACode.ToString.Length < 3 Then
                                ATAChapter = ATACode.ToString.PadLeft(3, "0"c) + " " + "-" + " " + ObjAssemblyMonitorModStatus.ATANomenclature
                            End If

                        End If

                        mReportMaintenanceDetail = New ReportMaintenanceDetail(AssemblyID, , ObjAssemblyMonitorModStatus.Code, , AssemblySerialNo, ATAChapter, , , Position, MonitorType, MonitorTypeCode, Note, DoneRemrk, Description, _
                                                           , EstimatedDate, , , Freq1, Freq1, Freq1, ElapsedTime, ElapsedTime, ElapsedTime, RemainingTime, RemainingTime, RemainingTime, _
                                                          DueAsof, DueAsof, DueAsof, AssemblyModel, , , , , , , , AssemblyTypeID, , ATACode, , , , , , , , , , , , , Number, Reference, DoneOnValue, DoneOnDate.FormattedText, _
                                                          , Applicability, ComplianceRequirement, , , , , , , , , , , Code, , , , IssueDate.Date.ToString("g"), IsApplicable, , , , , , , , , , , _
                                                           HoursFreq:=mHoursFreq, CyclesFreq:=mCyclesFreq, LandingsFreq:=mLandingsFreq, DaysMnthsYrsName:=mDaysMnthsYrsName, _
                                                          DaysMnthsYrsValue:=mDaysMnthsYrsValue, HoursDoneOnValue:=mHoursDoneOnValue, CyclesDoneOnValue:=mCyclesDoneOnValue, LandingsDoneOnValue:=mLandingsDoneOnValue, _
                                                          DaysMnthsYrsDoneOnValue:=mDaysMnthsYrsDoneOnValue)

                        mReportMaintenanceDetail.ModelMonitorModCode = ModelMonitorModCode

                        ReportMaintenanceDetails.Add(mReportMaintenanceDetail)

                    Next

                ElseIf chkService.Checked Then
                    For Each ObjAssemblyMonitorServiceStatus In ObjAssemblyStatus.AssemblyMonitorServiceStatusList
                        ATAChapter = ObjAssemblyMonitorServiceStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorServiceStatus.ATANomenclature

                        Description = ObjAssemblyMonitorServiceStatus.Description
                        Position = ObjAssemblyStatus.Position
                        MonitorTypeCode = ObjAssemblyMonitorServiceStatus.Code
                        MonitorType = ObjAssemblyMonitorServiceStatus.MonitorType

                        AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID
                        AssemblyModel = ObjAssemblyStatus.Model
                        AssemblySerialNo = ObjAssemblyStatus.SerialNo
                        Freq1 = ""
                        ElapsedTime = ""
                        RemainingTime = ""
                        DueAsof = ""
                        DoneOnValue = ""
                        EstimatedDate = ""
                        DoneOnDate.Text = ""

                        Code = ObjAssemblyMonitorServiceStatus.ModelMonitorServiceCode

                        'Added By Saylee on 28-Apr-2021 for PreDefined transferred excel sheet
                        mHoursFreq = ""
                        mCyclesFreq = ""
                        mLandingsFreq = ""
                        mDaysMnthsYrsName = ""
                        mDaysMnthsYrsValue = ""
                        mHoursDoneOnValue = ""
                        mCyclesDoneOnValue = ""
                        mLandingsDoneOnValue = ""
                        mDaysMnthsYrsDoneOnValue = ""
                        mRequiredManHours = ObjAssemblyMonitorServiceStatus.RequiredManHours
                        mIsLater = ObjAssemblyMonitorServiceStatus.IsLater
                        ATACode = ObjAssemblyMonitorServiceStatus.ATACode
                        ''**************************************************************

                        If ObjAssemblyMonitorServiceStatus.IsApplicable = True And ObjAssemblyMonitorServiceStatus.IsCompleted = False Then
                            EstimatedDate = ObjAssemblyMonitorServiceStatus.EstimatedDateFormatted  'Added by Saylee on 10-June-2009
                        End If

                        IsApplicable = ObjAssemblyMonitorServiceStatus.IsApplicable

                        For Each ObjAssemblyMonitorServiceStatusPeriod In ObjAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriodList
                            If ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 2 Then
                                If Freq1 = "" Then
                                    If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Or (ObjAssemblyMonitorServiceStatus.IsApplicable = False) Then
                                        RemainingTime = ""
                                        DueAsof = ""
                                        'Commented & added by Saylee on 1-Nov-2018 , as per BINU Frequency should be visible
                                        'Freq1 = ""
                                        Freq1 = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValueFormatted
                                        '***************************
                                        ElapsedTime = ""
                                    Else
                                        Freq1 = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValueFormatted
                                        ElapsedTime = ObjAssemblyMonitorServiceStatusPeriod.ElapsedValueFormatted
                                        RemainingTime = ObjAssemblyMonitorServiceStatusPeriod.RemainingValueFormatted
                                        DueAsof = ObjAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted
                                    End If

                                    DoneOnValue = DoneOnValue & ObjAssemblyMonitorServiceStatusPeriod.DoneOnValueFormatted & IIf(IsExcel, Chr(10), vbCrLf)


                                    'DoneOnValue = DoneOnValue & ObjAssemblyMonitorServiceStatusPeriod.DoneOnValueFormatted & IIf(IsExcel, Chr(10), vbCrLf)
                                    '=====================================================================
                                Else
                                    If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Or (ObjAssemblyMonitorServiceStatus.IsApplicable = False) Then
                                        RemainingTime = ""
                                        DueAsof = ""
                                        'Commented & added by Saylee on 1-Nov-2018 , as per BINU Frequency should be visible
                                        'Freq1 = ""
                                        Freq1 = Freq1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.FrequencyValueFormatted
                                        '*************************************
                                        ElapsedTime = ""
                                    Else
                                        Freq1 = Freq1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.FrequencyValueFormatted
                                        ElapsedTime = ElapsedTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.ElapsedValueFormatted
                                        RemainingTime = RemainingTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.RemainingValueFormatted
                                        DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted
                                    End If
                                    DoneOnValue = DoneOnValue + ObjAssemblyMonitorServiceStatusPeriod.DoneOnValueFormatted & IIf(IsExcel, Chr(10), vbCrLf)
                                End If
                            Else
                                If Freq1 = "" Then
                                    If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Or (ObjAssemblyMonitorServiceStatus.IsApplicable = False) Then
                                        RemainingTime = ""
                                        DueAsof = ""
                                        'Commented & added by Saylee on 1-Nov-2018 , as per BINU Frequency should be visible
                                        'Freq1 = ""
                                        Freq1 = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue
                                        '*************************************
                                        ElapsedTime = ""
                                    Else
                                        Freq1 = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue
                                        ElapsedTime = ObjAssemblyMonitorServiceStatusPeriod.ElapsedValue
                                        RemainingTime = ObjAssemblyMonitorServiceStatusPeriod.RemainingValue
                                        DueAsof = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
                                    End If
                                    If ObjAssemblyMonitorServiceStatus.MonitorType = "No Frequency" Or ObjAssemblyMonitorServiceStatus.IsApplicable = False Then 'Added By Prashant 28-Sep-2018
                                        DoneOnValue = ""
                                    Else
                                        DoneOnValue = DoneOnValue + ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf)
                                    End If
                                Else
                                    If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Or (ObjAssemblyMonitorServiceStatus.IsApplicable = False) Then
                                        RemainingTime = ""
                                        DueAsof = ""
                                        'Commented & added by Saylee on 1-Nov-2018 , as per BINU Frequency should be visible
                                        'Freq1 = ""
                                        Freq1 = Freq1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue
                                        '*************************************
                                        ElapsedTime = ""
                                    Else
                                        Freq1 = Freq1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue
                                        ElapsedTime = ElapsedTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.ElapsedValue
                                        RemainingTime = RemainingTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.RemainingValue
                                        DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
                                    End If
                                    If ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue = "" Then
                                        If ObjAssemblyMonitorServiceStatus.MonitorType = "No Frequency" Or ObjAssemblyMonitorServiceStatus.IsApplicable = False Then 'Added By Prashant 28-Sep-2018
                                            DoneOnValue = ""
                                        Else
                                            DoneOnValue = DoneOnValue & ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
                                        End If
                                    Else
                                        If ObjAssemblyMonitorServiceStatus.MonitorType = "No Frequency" Or ObjAssemblyMonitorServiceStatus.IsApplicable = False Then 'Added By Prashant 28-Sep-2018
                                            DoneOnValue = ""
                                        Else
                                            DoneOnValue = DoneOnValue + ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf)
                                        End If
                                    End If
                                End If
                            End If

                            'Added By Saylee on 28-Apr-2021 for PreDefined transferred excel sheet
                            If ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 1 Then
                                mHoursFreq = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue.ToString.Split(" ")(0)   'Added By Saylee on 28-Apr-2021 for PreDefined transferred excel sheet
                                mHoursDoneOnValue = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue.ToString.Split(" ")(0)
                            ElseIf ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 2 Then
                                If ObjAssemblyMonitorServiceStatusPeriod.PeriodUnitID = 3 Then
                                    mDaysMnthsYrsValue = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue.ToString.Split(" ")(0)
                                    mDaysMnthsYrsName = "Days"
                                ElseIf ObjAssemblyMonitorServiceStatusPeriod.PeriodUnitID = 4 Then
                                    mDaysMnthsYrsValue = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue.ToString.Split(" ")(0)
                                    mDaysMnthsYrsName = "Months"
                                ElseIf ObjAssemblyMonitorServiceStatusPeriod.PeriodUnitID = 5 Then
                                    mDaysMnthsYrsValue = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue.ToString.Split(" ")(0)
                                    mDaysMnthsYrsName = "Years"
                                End If
                                mDaysMnthsYrsDoneOnValue = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValueFormatted.ToString.Split(" ")(0)
                            ElseIf ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 3 Then
                                mCyclesFreq = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue.ToString.Split(" ")(0)
                                mCyclesDoneOnValue = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue.ToString.Split(" ")(0)
                            ElseIf ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 7 Then
                                mLandingsFreq = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue.ToString.Split(" ")(0)
                                mLandingsDoneOnValue = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue.ToString.Split(" ")(0)
                            End If
                            '**************************




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
                        Note = ObjAssemblyMonitorServiceStatus.Notes

                        If Note = "" Then
                            Note = "----"
                        End If


                        Reference = ObjAssemblyMonitorServiceStatus.Reference
                        If Reference = "" And AppSettings("ClientCode") <> "AVE" Then
                            Reference = "----"
                        End If

                        DoneOnDate.Text = ObjAssemblyMonitorServiceStatus.DoneOn


                        DoneRemrk = ObjAssemblyMonitorServiceStatus.DoneRemark
                        If DoneRemrk = "" Then
                            DoneRemrk = "----"
                        End If



                        ModelMonitorModCode = ObjAssemblyMonitorServiceStatus.ModelMonitorServiceCode
                        If ModelMonitorModCode = "" And IsForTransfer = False Then
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


                        If IsExcel Then
                            Dim ATACode As Integer = ObjAssemblyMonitorServiceStatus.ATACode
                            If ATACode.ToString.Length < 3 Then
                                ATAChapter = ATACode.ToString.PadLeft(3, "0"c) + " " + "-" + " " + ObjAssemblyMonitorServiceStatus.ATANomenclature
                            End If

                        End If

                        mReportMaintenanceDetail = New ReportMaintenanceDetail(AssemblyID, , ObjAssemblyMonitorServiceStatus.Code, , AssemblySerialNo, ATAChapter, , , Position, MonitorType, MonitorTypeCode, Note, DoneRemrk, Description, _
                                                           , EstimatedDate, , , Freq1, Freq1, Freq1, ElapsedTime, ElapsedTime, ElapsedTime, RemainingTime, RemainingTime, RemainingTime, _
                                                          DueAsof, DueAsof, DueAsof, AssemblyModel, , , , , , , , AssemblyTypeID, , ATACode, , , , , , , , , , , , , Number, Reference, DoneOnValue, DoneOnDate.FormattedText, _
                                                          , , , , , , , , , , , , , Code, , , , , IsApplicable, , , , , , , , , , , _
                                                           HoursFreq:=mHoursFreq, CyclesFreq:=mCyclesFreq, LandingsFreq:=mLandingsFreq, DaysMnthsYrsName:=mDaysMnthsYrsName, _
                                                          DaysMnthsYrsValue:=mDaysMnthsYrsValue, HoursDoneOnValue:=mHoursDoneOnValue, CyclesDoneOnValue:=mCyclesDoneOnValue, LandingsDoneOnValue:=mLandingsDoneOnValue, _
                                                          DaysMnthsYrsDoneOnValue:=mDaysMnthsYrsDoneOnValue)

                        mReportMaintenanceDetail.ModelMonitorModCode = ModelMonitorModCode

                        ReportMaintenanceDetails.Add(mReportMaintenanceDetail)

                    Next

                ElseIf chkInspection.Checked Then
                    For Each ObjAssemblyMonitorInspStatus In ObjAssemblyStatus.AssemblyMonitorInspStatusList
                        ATAChapter = ObjAssemblyMonitorInspStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorInspStatus.ATANomenclature

                        Description = ObjAssemblyMonitorInspStatus.Description
                        Position = ObjAssemblyStatus.Position
                        MonitorTypeCode = ObjAssemblyMonitorInspStatus.Code
                        MonitorType = ObjAssemblyMonitorInspStatus.MonitorType

                        AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID
                        AssemblyModel = ObjAssemblyStatus.Model
                        AssemblySerialNo = ObjAssemblyStatus.SerialNo
                        Freq1 = ""
                        ElapsedTime = ""
                        RemainingTime = ""
                        DueAsof = ""
                        DoneOnValue = ""
                        EstimatedDate = ""
                        DoneOnDate.Text = ""

                        Code = ObjAssemblyMonitorInspStatus.ModelMonitorInspCode

                        'Added By Saylee on 28-Apr-2021 for PreDefined transferred excel sheet
                        mHoursFreq = ""
                        mCyclesFreq = ""
                        mLandingsFreq = ""
                        mDaysMnthsYrsName = ""
                        mDaysMnthsYrsValue = ""
                        mHoursDoneOnValue = ""
                        mCyclesDoneOnValue = ""
                        mLandingsDoneOnValue = ""
                        mDaysMnthsYrsDoneOnValue = ""
                        mRequiredManHours = ObjAssemblyMonitorInspStatus.RequiredManHours
                        mIsLater = ObjAssemblyMonitorInspStatus.IsLater
                        ATACode = ObjAssemblyMonitorInspStatus.ATACode
                        ''**************************************************************

                        If ObjAssemblyMonitorInspStatus.IsApplicable = True And ObjAssemblyMonitorInspStatus.IsCompleted = False Then
                            EstimatedDate = ObjAssemblyMonitorInspStatus.EstimatedDateFormatted  'Added by Saylee on 10-June-2009
                        End If

                        IsApplicable = ObjAssemblyMonitorInspStatus.IsApplicable

                        For Each ObjAssemblyMonitorInspStatusPeriod In ObjAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriodList
                            If ObjAssemblyMonitorInspStatusPeriod.PeriodID = 2 Then
                                If Freq1 = "" Then
                                    If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False) Then
                                        RemainingTime = ""
                                        DueAsof = ""
                                        'Commented & added by Saylee on 1-Nov-2018 , as per BINU Frequency should be visible
                                        'Freq1 = ""
                                        Freq1 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValueFormatted
                                        '***************************
                                        ElapsedTime = ""
                                    Else
                                        Freq1 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValueFormatted
                                        ElapsedTime = ObjAssemblyMonitorInspStatusPeriod.ElapsedValueFormatted
                                        RemainingTime = ObjAssemblyMonitorInspStatusPeriod.RemainingValueFormatted
                                        DueAsof = ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
                                    End If

                                    DoneOnValue = DoneOnValue & ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted & IIf(IsExcel, Chr(10), vbCrLf)


                                    'DoneOnValue = DoneOnValue & ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted & IIf(IsExcel, Chr(10), vbCrLf)
                                    '=====================================================================
                                Else
                                    If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False) Then
                                        RemainingTime = ""
                                        DueAsof = ""
                                        'Commented & added by Saylee on 1-Nov-2018 , as per BINU Frequency should be visible
                                        'Freq1 = ""
                                        Freq1 = Freq1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.FrequencyValueFormatted
                                        '*************************************
                                        ElapsedTime = ""
                                    Else
                                        Freq1 = Freq1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.FrequencyValueFormatted
                                        ElapsedTime = ElapsedTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.ElapsedValueFormatted
                                        RemainingTime = RemainingTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.RemainingValueFormatted
                                        DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
                                    End If
                                    DoneOnValue = DoneOnValue + ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted & IIf(IsExcel, Chr(10), vbCrLf)
                                End If
                            Else
                                If Freq1 = "" Then
                                    If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False) Then
                                        RemainingTime = ""
                                        DueAsof = ""
                                        'Commented & added by Saylee on 1-Nov-2018 , as per BINU Frequency should be visible
                                        'Freq1 = ""
                                        Freq1 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValue
                                        '*************************************
                                        ElapsedTime = ""
                                    Else
                                        Freq1 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValue
                                        ElapsedTime = ObjAssemblyMonitorInspStatusPeriod.ElapsedValue
                                        RemainingTime = ObjAssemblyMonitorInspStatusPeriod.RemainingValue
                                        DueAsof = ObjAssemblyMonitorInspStatusPeriod.DueOnValue
                                    End If
                                    If ObjAssemblyMonitorInspStatus.MonitorType = "No Frequency" Or ObjAssemblyMonitorInspStatus.IsApplicable = False Then 'Added By Prashant 28-Sep-2018
                                        DoneOnValue = ""
                                    Else
                                        DoneOnValue = DoneOnValue + ObjAssemblyMonitorInspStatusPeriod.DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf)
                                    End If
                                Else
                                    If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False) Then
                                        RemainingTime = ""
                                        DueAsof = ""
                                        'Commented & added by Saylee on 1-Nov-2018 , as per BINU Frequency should be visible
                                        'Freq1 = ""
                                        Freq1 = Freq1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.FrequencyValue
                                        '*************************************
                                        ElapsedTime = ""
                                    Else
                                        Freq1 = Freq1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.FrequencyValue
                                        ElapsedTime = ElapsedTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.ElapsedValue
                                        RemainingTime = RemainingTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.RemainingValue
                                        DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.DueOnValue
                                    End If
                                    If ObjAssemblyMonitorInspStatusPeriod.DoneOnValue = "" Then
                                        If ObjAssemblyMonitorInspStatus.MonitorType = "No Frequency" Or ObjAssemblyMonitorInspStatus.IsApplicable = False Then 'Added By Prashant 28-Sep-2018
                                            DoneOnValue = ""
                                        Else
                                            DoneOnValue = DoneOnValue & ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
                                        End If
                                    Else
                                        If ObjAssemblyMonitorInspStatus.MonitorType = "No Frequency" Or ObjAssemblyMonitorInspStatus.IsApplicable = False Then 'Added By Prashant 28-Sep-2018
                                            DoneOnValue = ""
                                        Else
                                            DoneOnValue = DoneOnValue + ObjAssemblyMonitorInspStatusPeriod.DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf)
                                        End If
                                    End If
                                End If
                            End If

                            'Added By Saylee on 28-Apr-2021 for PreDefined transferred excel sheet
                            If ObjAssemblyMonitorInspStatusPeriod.PeriodID = 1 Then
                                mHoursFreq = ObjAssemblyMonitorInspStatusPeriod.FrequencyValue.ToString.Split(" ")(0)   'Added By Saylee on 28-Apr-2021 for PreDefined transferred excel sheet
                                mHoursDoneOnValue = ObjAssemblyMonitorInspStatusPeriod.DoneOnValue.ToString.Split(" ")(0)
                            ElseIf ObjAssemblyMonitorInspStatusPeriod.PeriodID = 2 Then
                                If ObjAssemblyMonitorInspStatusPeriod.PeriodUnitID = 3 Then
                                    mDaysMnthsYrsValue = ObjAssemblyMonitorInspStatusPeriod.FrequencyValue.ToString.Split(" ")(0)
                                    mDaysMnthsYrsName = "Days"
                                ElseIf ObjAssemblyMonitorInspStatusPeriod.PeriodUnitID = 4 Then
                                    mDaysMnthsYrsValue = ObjAssemblyMonitorInspStatusPeriod.FrequencyValue.ToString.Split(" ")(0)
                                    mDaysMnthsYrsName = "Months"
                                ElseIf ObjAssemblyMonitorInspStatusPeriod.PeriodUnitID = 5 Then
                                    mDaysMnthsYrsValue = ObjAssemblyMonitorInspStatusPeriod.FrequencyValue.ToString.Split(" ")(0)
                                    mDaysMnthsYrsName = "Years"
                                End If
                                mDaysMnthsYrsDoneOnValue = ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted.ToString.Split(" ")(0)
                            ElseIf ObjAssemblyMonitorInspStatusPeriod.PeriodID = 3 Then
                                mCyclesFreq = ObjAssemblyMonitorInspStatusPeriod.FrequencyValue.ToString.Split(" ")(0)
                                mCyclesDoneOnValue = ObjAssemblyMonitorInspStatusPeriod.DoneOnValue.ToString.Split(" ")(0)
                            ElseIf ObjAssemblyMonitorInspStatusPeriod.PeriodID = 7 Then
                                mLandingsFreq = ObjAssemblyMonitorInspStatusPeriod.FrequencyValue.ToString.Split(" ")(0)
                                mLandingsDoneOnValue = ObjAssemblyMonitorInspStatusPeriod.DoneOnValue.ToString.Split(" ")(0)
                            End If
                            '**************************




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
                        Note = ObjAssemblyMonitorInspStatus.Notes

                        If Note = "" Then
                            Note = "----"
                        End If


                        Reference = ObjAssemblyMonitorInspStatus.Reference
                        If Reference = "" And AppSettings("ClientCode") <> "AVE" Then
                            Reference = "----"
                        End If

                        DoneOnDate.Text = ObjAssemblyMonitorInspStatus.DoneOn


                        DoneRemrk = ObjAssemblyMonitorInspStatus.DoneRemark
                        If DoneRemrk = "" Then
                            DoneRemrk = "----"
                        End If



                        ''

                        ModelMonitorModCode = ObjAssemblyMonitorInspStatus.ModelMonitorInspCode
                        If ModelMonitorModCode = "" And IsForTransfer = False Then
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


                        If IsExcel Then
                            Dim ATACode As Integer = ObjAssemblyMonitorInspStatus.ATACode
                            If ATACode.ToString.Length < 3 Then
                                ATAChapter = ATACode.ToString.PadLeft(3, "0"c) + " " + "-" + " " + ObjAssemblyMonitorInspStatus.ATANomenclature
                            End If

                        End If

                        mReportMaintenanceDetail = New ReportMaintenanceDetail(AssemblyID, , ObjAssemblyMonitorInspStatus.Code, , AssemblySerialNo, ATAChapter, , , Position, MonitorType, MonitorTypeCode, Note, DoneRemrk, Description, _
                                                           , EstimatedDate, , , Freq1, Freq1, Freq1, ElapsedTime, ElapsedTime, ElapsedTime, RemainingTime, RemainingTime, RemainingTime, _
                                                          DueAsof, DueAsof, DueAsof, AssemblyModel, , , , , , , , AssemblyTypeID, , ATACode, , , , , , , , , , , , , Number, Reference, DoneOnValue, DoneOnDate.FormattedText, _
                                                          , , , , , , , , , , , , , Code, , , , , IsApplicable, , , , , , , , , , , _
                                                           HoursFreq:=mHoursFreq, CyclesFreq:=mCyclesFreq, LandingsFreq:=mLandingsFreq, DaysMnthsYrsName:=mDaysMnthsYrsName, _
                                                          DaysMnthsYrsValue:=mDaysMnthsYrsValue, HoursDoneOnValue:=mHoursDoneOnValue, CyclesDoneOnValue:=mCyclesDoneOnValue, LandingsDoneOnValue:=mLandingsDoneOnValue, _
                                                          DaysMnthsYrsDoneOnValue:=mDaysMnthsYrsDoneOnValue)

                        mReportMaintenanceDetail.ModelMonitorModCode = ModelMonitorModCode

                        ReportMaintenanceDetails.Add(mReportMaintenanceDetail)

                    Next

                End If


            Next
        Next
        ''''''' End If
        '''''''  Next
        Return ReportMaintenanceDetails
    End Function

    Public Function ReportDetail(Optional ByVal IsForTransfer As Boolean = False) As ReportMaintenanceDetailList
        Dim ObjMachine As MachineInfo
        Dim ObjAssemblyStatus As AssemblyStatusInfo
        Dim ObjCompStatus As CompStatusInfo
        Dim ObjCompStatusPeriod As CompStatusPeriodInfo
        Dim ObjCompMonitorInspStatus As CompMonitorInspStatusInfo
        Dim ObjCompMonitorInspStatusPeriod As CompMonitorInspStatusPeriodInfo
        Dim ObjCompMonitorServiceStatus As CompMonitorServiceStatusInfo
        Dim ObjCompMonitorServiceStatusPeriod As CompMonitorServiceStatusPeriodInfo
        Dim ObjCompMonitorModStatus As CompMonitorModStatusInfo
        Dim ObjCompMonitorModStatusPeriod As CompMonitorModStatusPeriodInfo

        Dim Periodcount As Integer
        Dim Count As Integer
        Dim AssemblyID As Guid
        Dim MonitorInspTypeIDs As New StringBuilder

        If chkInspection.Checked Then
            For P As Integer = 0 To InspectionTypeID.Count - 1
                MonitorInspTypeIDs.Append(InspectionTypeID(P).ToString + ",")
            Next

        End If

        Dim MonitorServiceTypeIDs As New StringBuilder

        If chkService.Checked Then
            For P As Integer = 0 To ServiceTypeID.Count - 1
                MonitorServiceTypeIDs.Append(ServiceTypeID(P).ToString + ",")
            Next
        End If

        If chkService.Checked Or chkInspection.Checked Then
            mMachineList = MachineList.GetMachineListMonitoringStatusForHardTimeAndDirective(FromDate, cmbAircraft.SelectedValue, , , , , , , , , , True, True, , New Guid(AssemblyName).ToString, , , , , , , , , , , , , , , , , , , , , , False, , False, , True, , , , , , , 0, IsSerSelect, IsInsSelect, , SkipIsForInventoryAircarft:=True, MonitorInspTypeIDs:=MonitorInspTypeIDs.ToString, MonitorServiceTypeIDs:=MonitorServiceTypeIDs.ToString.TrimEnd(","))
        ElseIf chkDirective.Checked Then
            mMachineList = MachineList.GetMachineListMonitoringStatus(FromDate, MachineName, , , , , , , , , , True, True, , _
                                                                AssemblyName, MonitoringModRequired:=True, _
                                                                IsAssemblyRemoved:=False, IsCompRemoved:=False, IsComplied:=True, _
                                                                IsAverageRequired:=True, AverageMonths:=6, CompMonitoringModRequired:=True, _
                                                                SkipIsForInventoryAircarft:=True, MonitorModTypeIDs:=ModificationTypeID.ToString.TrimEnd(",")) 'IsAverageRequired:=mIsAverageRequired, ByPerDayLimit:=mByPerDayLimit, PerdayLimits:=mPerDayLimits, SkipIsForInventoryAircarft:=True)

        End If

        Dim LHLabel2 As String = ""
        Dim LHData2 As String = ""
        Dim LHLabel3 As String = ""
        Dim LHData3 As String = ""
        Dim LHLabel4 As String = ""
        Dim LHData4 As String = ""

        Dim LHLabel5 As String = ""
        Dim LHData5 As String = ""

        Dim LHData9 As String = ""
        Dim LHData10 As String = ""
        '''''''For Each ObjMachine In mMachineList
        '''''''    For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
        '''''''        Periodcount = ObjAssemblyStatus.AssemblyStatusPeriodList.Count()
        '''''''        LHLabel2 = ""
        '''''''        LHData2 = ""
        '''''''        LHLabel3 = ""
        '''''''        LHData3 = ""

        '''''''        LHLabel4 = ""
        '''''''        LHData4 = ""

        '''''''        LHLabel5 = ""
        '''''''        LHData5 = ""

        '''''''        LHData9 = ""
        '''''''        LHData10 = ""
        '''''''Added by Saylee on 31-Aug-2018, to show TSO for "NOVO" : NOVO31082018
        ''''''Dim mTSOMachineList As ListOfAircraftCurrentStatus
        ''''''If AppSettings("ClientCode") = "Novo" Then mTSOMachineList = ListOfAircraftCurrentStatus.GetListOfAircraftCurrentStatus("", ObjMachine.RegNo, ObjAssemblyStatus.ModelID.ToString, , , AsonDate)
        '''''''******************************************************

        ''''''For Count = 0 To Periodcount - 1
        ''''''    If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID <> 2 Then
        ''''''        LHLabel2 = CType(IIf(LHLabel2 = "", LHLabel2, LHLabel2 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
        ''''''        LHData2 = CType(IIf(LHData2 = "", LHData2, LHData2 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyCurrentValue
        ''''''    End If

        ''''''    If cmbAssembly.SelectedIndex <> 1 Then 'Except air frame
        ''''''        If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 2 Then
        ''''''            LHLabel3 = CType(IIf(LHLabel3 = "", LHLabel3, LHLabel3 + vbNewLine), String) + "Date"
        ''''''            LHData3 = CType(IIf(LHData3 = "", LHData3, LHData3 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyInstallationValueFormatted
        ''''''        Else
        ''''''            LHLabel3 = CType(IIf(LHLabel3 = "", LHLabel3, LHLabel3 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
        ''''''            LHData3 = CType(IIf(LHData3 = "", LHData3, LHData3 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyInstallationValueFormatted
        ''''''        End If

        ''''''        If AppSettings("ClientCode") = "STR" Then 'Added by Saylee on 4-Oct-2018
        ''''''            'For Airframe
        ''''''            If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID <> 2 Then
        ''''''                LHLabel5 = CType(IIf(LHLabel5 = "", LHLabel5, LHLabel5 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
        ''''''                LHData5 = CType(IIf(LHData5 = "", LHData5, LHData5 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyCurrentValueByAirFrame
        ''''''            End If
        ''''''            'Added by Saylee on 28-Jan-2021, as StarAir needs to skip Hours value for LAnding Gear assembly
        ''''''            If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 1 And ObjAssemblyStatus.AssemblyTypeID = 6 Then
        ''''''                LHLabel2 = ""
        ''''''                LHData2 = ""
        ''''''                LHLabel3 = ""
        ''''''                LHData3 = ""
        ''''''            End If
        ''''''            '******************
        ''''''        End If
        ''''''    Else
        ''''''        LHLabel3 = ""
        ''''''        LHData3 = ""
        ''''''        LHLabel5 = ""
        ''''''        LHData5 = ""
        ''''''    End If

        'Added by Saylee on 31-Aug-2018, to show TSO for "NOVO" : NOVO31082018
        ''for TSO

        ''''''If AppSettings("ClientCode") = "Novo" Then
        ''''''    For i As Integer = 0 To mTSOMachineList.Count - 1
        ''''''        If mTSOMachineList(i).SerialNo = ObjAssemblyStatus.SerialNo Then
        ''''''            If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 1 Then
        ''''''                If Not LHData4.Contains(mTSOMachineList(i).TSO) Then
        ''''''                    If mTSOMachineList(i).TSO <> "" Then LHLabel4 = CType(IIf(LHLabel4 = "", LHLabel4, LHLabel4 + vbNewLine), String) + "TSO"
        ''''''                    LHData4 = CType(IIf(LHData4 = "", LHData4, LHData4 + vbNewLine), String) + mTSOMachineList(i).TSO

        ''''''                    'Added by Saylee on 10-Feb-2021 for NOVO1002021
        ''''''                    If mTSOMachineList(i).TSOFreq <> "" Then LHData9 = CType(IIf(LHData9 = "", LHData9, LHData9 + vbNewLine), String) + "Hours"
        ''''''                    LHData10 = CType(IIf(LHData10 = "", LHData10, LHData10 + vbNewLine), String) + mTSOMachineList(i).TSOFreq
        ''''''                    '***************
        ''''''                End If
        ''''''            ElseIf ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 2 Then
        ''''''                If Not LHData4.Contains(mTSOMachineList(i).DateSO) Then
        ''''''                    If mTSOMachineList(i).DateSO <> "" Then LHLabel4 = CType(IIf(LHLabel4 = "", LHLabel4, LHLabel4 + vbNewLine), String) + "Date"
        ''''''                    LHData4 = CType(IIf(LHData4 = "", LHData4, LHData4 + vbNewLine), String) + mTSOMachineList(i).DateSO

        ''''''                    'Added by Saylee on 10-Feb-2021 for NOVO1002021
        ''''''                    If mTSOMachineList(i).DateSOFreq <> "" Then LHData9 = CType(IIf(LHData9 = "", LHData9, LHData9 + vbNewLine), String) + mTSOMachineList(i).PeriodUnitName
        ''''''                    LHData10 = CType(IIf(LHData10 = "", LHData10, LHData10 + vbNewLine), String) + mTSOMachineList(i).DateSOFreq
        ''''''                    '***************
        ''''''                End If
        ''''''            ElseIf ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 3 Then
        ''''''                If Not LHData4.Contains(mTSOMachineList(i).CSO) Then
        ''''''                    If mTSOMachineList(i).CSO <> "" Then LHLabel4 = CType(IIf(LHLabel4 = "", LHLabel4, LHLabel4 + vbNewLine), String) + "CSO"
        ''''''                    LHData4 = CType(IIf(LHData4 = "", LHData4, LHData4 + vbNewLine), String) + mTSOMachineList(i).CSO

        ''''''                    'Added by Saylee on 10-Feb-2021 for NOVO1002021
        ''''''                    If mTSOMachineList(i).CSOFreq <> "" Then LHData9 = CType(IIf(LHData9 = "", LHData9, LHData9 + vbNewLine), String) + "Cycles"
        ''''''                    LHData10 = CType(IIf(LHData10 = "", LHData10, LHData10 + vbNewLine), String) + mTSOMachineList(i).CSOFreq
        ''''''                    '***************
        ''''''                End If
        ''''''            End If
        ''''''        End If

        ''''''    Next
        ''''''End If
        ''''''    Next

        '''''''Dim ModelName As String = ""
        '''''''Dim SerialNoPostion As String = ""
        '''''''Dim searchstr7 As String = ""

        '''''''If ObjAssemblyStatus.Position = "" Then
        '''''''    SerialNoPostion = ObjAssemblyStatus.SerialNo
        '''''''    ModelName = ObjAssemblyStatus.Model
        '''''''Else
        '''''''    If AppSettings("ClientCode") = "STR" Then
        '''''''        SerialNoPostion = ObjAssemblyStatus.SerialNo
        '''''''        ModelName = ObjAssemblyStatus.Model + " (" + ObjAssemblyStatus.Position + ")" 'Added b7y saylee on 4-Oct-2018 for 
        '''''''    Else
        '''''''        SerialNoPostion = ObjAssemblyStatus.SerialNo + " (" + ObjAssemblyStatus.Position + ")"
        '''''''        ModelName = ObjAssemblyStatus.Model
        '''''''    End If
        '''''''End If
        '''''''searchstr7 = ObjMachine.Owner.ToString 'Added By Utkarsh On 07-Apr-2011 ' "Owner/Operator :- " + 
        '''''''AssemblyID = ObjAssemblyStatus.AssemblyID




        '''''''ReportStatusList.Add(New rptStatus(AssemblyID.ToString, ObjAssemblyStatus.AssemblyTypeID, , "Reg No.", ObjMachine.RegNo, ObjAssemblyStatus.AssemblyType + " " + "Model", ModelName, _
        '''''''    "Serial No.", SerialNoPostion, IIf(rdbAirframeDue.Checked, "Next Due (Airframe Values)", "Due As of " & ObjAssemblyStatus.AssemblyType), LHLabel4, LHData4, "Position ", ObjAssemblyStatus.Position, ObjAssemblyStatus.AssemblyType, LHData9, LHData10, , , , , , LHLabel2, LHData2, LHLabel3, LHData3, RHData10:=LHLabel5, RHData11:=LHData5))
        '' Next
        '''''''Next

        ''mMachineList = MachineList.GetMachineListMonitoringStatusForHardTimeAndDirective(txtAsOnDate.Value.ToString, cmbAircraft.SelectedValue, , , , , , , , , , True, True, , mAssemblylist(cmbAssembly.SelectedIndex).ID.ToString, , , , , , , , , , , ShowCofA, , , , , , , , , , , False, , False, , True, , , , , , True, 6, True, True)
        Dim InstalledAt As String
        Dim TSO1 As String
        Dim mHoursFreq, mCyclesFreq, mLandingsFreq, mDaysMnthsYrsName, mDaysMnthsYrsValue As String
        Dim mHoursDoneOnValue, mCyclesDoneOnValue, mLandingsDoneOnValue, mDaysMnthsYrsDoneOnValue, mPartDesc As String
        Dim mPartMonitorServiceCode As String = ""
        Dim mPartMonitorModCode As String = ""
        Dim mIsLater As Boolean = False
        Dim mIsApplicable As Boolean = False


        If IsSerSelect = True Then
            For Each ObjMachine In mMachineList
                For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                    For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
                        'Added by Deven sir on 18-June-2009
                        InstalledAt = ""
                        TSO1 = ""
                        For Each ObjCompStatusPeriod In ObjCompStatus.CompStatusPeriodList
                            If Not ObjCompStatusPeriod.PeriodID = 2 Then
                                InstalledAt = InstalledAt & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompStatusPeriod.PeriodID, "").CompInstallationTextFormatted
                                TSO1 = TSO1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompStatusPeriod.PeriodID, "").AssemblyInstallationTextFormatted
                            Else
                                If InstalledAt = "" Then InstalledAt = InstalledAt & IIf(IsExcel, Chr(10), vbCrLf) & ""
                                If TSO1 = "" Then TSO1 = TSO1 & IIf(IsExcel, Chr(10), vbCrLf) & ""
                            End If
                        Next
                        '*************************************

                        'Added By Saylee on 28-Apr-2021 for PreDefined transferred excel sheet
                        Dim Manufacturer, InstallationWONo, InstallationRemark, InstPlace, InstallationDoneBy As String
                        Dim TSNHours, CSNCycles, SinceNewDate, SinceNewLandings As String
                        Dim InstCompHours, InstCompCycles, InstCompStartDate, InstCompLandings As String
                        Dim AssemblyInstHours, AssemblyInstCycles, AssemblyInstStartDate, AssemblyInstLandings As String



                        Manufacturer = ""
                        InstallationWONo = ""
                        InstallationRemark = ""
                        InstPlace = ""
                        InstallationDoneBy = ""
                        TSNHours = ""
                        CSNCycles = ""
                        SinceNewDate = ""
                        SinceNewLandings = ""
                        InstCompHours = ""
                        InstCompCycles = ""
                        InstCompStartDate = ""
                        InstCompLandings = ""
                        AssemblyInstHours = ""
                        AssemblyInstCycles = ""
                        AssemblyInstStartDate = ""
                        AssemblyInstLandings = ""
                        mPartDesc = ""

                        If IsForTransfer = True Then

                            Dim mInstallCompStatus As CompStatus = CompStatus.GetCompStatus(ObjCompStatus.ID, ObjAssemblyStatus.ID, txtFromDate.Text)
                            mPartDesc = mInstallCompStatus.Description
                            Manufacturer = mInstallCompStatus.ManufacturerName
                            InstallationWONo = mInstallCompStatus.InstallationWONo
                            InstallationRemark = mInstallCompStatus.InstallationRemark
                            InstPlace = mInstallCompStatus.InstPlace
                            InstallationDoneBy = mInstallCompStatus.InstDoneBy
                            If mInstallCompStatus.CompStatusPeriods.Contains(1) Then

                                InstCompHours = mInstallCompStatus.CompStatusPeriods(1, "").CompInstallationValue.ToString
                                AssemblyInstHours = mInstallCompStatus.CompStatusPeriods(1, "").AssemblyInstallationValue.ToString
                            End If
                            If mInstallCompStatus.CompStatusPeriods.Contains(2) Then
                                InstCompStartDate = mInstallCompStatus.CompStatusPeriods(2, "").CompInstallationValueFormatted.ToString
                                AssemblyInstStartDate = mInstallCompStatus.CompStatusPeriods(2, "").AssemblyInstallationValueFormatted.ToString
                                SinceNewDate = mInstallCompStatus.CompStatusPeriods(2, "").AssemblyInstallationValueFormatted.ToString 'New SmartDate(txtAsOnDate.Text.ToString).FormattedText  'Added By Saylee on 28-Apr-2021 for PreDefined transferred
                            End If
                            If mInstallCompStatus.CompStatusPeriods.Contains(3) Then

                                InstCompCycles = mInstallCompStatus.CompStatusPeriods(3, "").CompInstallationValue.ToString
                                AssemblyInstCycles = mInstallCompStatus.CompStatusPeriods(3, "").AssemblyInstallationValue.ToString
                            End If
                            If mInstallCompStatus.CompStatusPeriods.Contains(7) Then

                                InstCompLandings = mInstallCompStatus.CompStatusPeriods(7, "").CompInstallationValue.ToString
                                AssemblyInstLandings = mInstallCompStatus.CompStatusPeriods(7, "").AssemblyInstallationValue.ToString
                            End If

                        End If



                        For Each ObjCompMonitorServiceStatus In ObjCompStatus.CompMonitorServiceStatusList
                            'Commneted By Prashant 22-July-2009 
                            'If ((Report = 1 And ObjCompMonitorServiceStatus.MonitorType <> "No Frequency") Or (Report = 0 And ObjCompMonitorServiceStatus.MonitorType = "No Frequency")) And (ObjCompMonitorServiceStatus.IsApplicable = True) Then
                            '-------------------------------------------------------------------------------------
                            'Added By Prashant 22-July-2009 for records which are not applicable for Report = 0
                            If ((Report = 1 And ObjCompMonitorServiceStatus.MonitorType <> "No Frequency") And (ObjCompMonitorServiceStatus.IsApplicable = True)) Or _
                                (Report = 0) Then
                                '-----------------------------------------------------------------

                                ATAChapter = ObjCompMonitorServiceStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorServiceStatus.ATANomenclature
                                ATACode = ObjCompMonitorServiceStatus.ATACode
                                Description = ObjCompMonitorServiceStatus.Description
                                PartNo = ObjCompStatus.PartName
                                CompSerialNo = ObjCompStatus.CompSerialNo
                                Position = ObjCompStatus.Position
                                MonitorTypeCode = ObjCompMonitorServiceStatus.Code
                                EstimatedDate = ObjCompMonitorServiceStatus.EstimatedDateFormatted
                                MonitorType = ObjCompMonitorServiceStatus.Type
                                AssemblyModel = ObjAssemblyStatus.Model
                                AssemblySerialNo = ObjAssemblyStatus.SerialNo
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
                                ATACode = ObjCompMonitorServiceStatus.ATACode
                                'InstalledAt = ""  'Commented by Saylee on 18-June-2009
                                InstalledAt1 = ""
                                InstalledAt2 = ""
                                TSN = ""
                                TSO = ""
                                ' TSO1 = ""  'Commented by Saylee on 18-June-2009
                                TSO2 = ""
                                RemoveAt = ""
                                RemoveAt1 = ""
                                RemoveAt2 = ""
                                InstalledAtDate.Text = ObjCompStatus.InstalledOn
                                RemoveAtDate.Text = ""
                                DoneRemrk = ObjCompMonitorServiceStatus.DoneRemark
                                DoneOnValue = ""
                                DoneOnDate.Text = ""

                                'Added By Saylee On 26-Jun-2014 For ALL26062014
                                AirframeDueAsof = ""


                                'Added By Saylee on 28-Apr-2021 for PreDefined transferred excel sheet
                                mHoursFreq = ""
                                mCyclesFreq = ""
                                mLandingsFreq = ""
                                mDaysMnthsYrsName = ""
                                mDaysMnthsYrsValue = ""
                                mHoursDoneOnValue = ""
                                mCyclesDoneOnValue = ""
                                mLandingsDoneOnValue = ""
                                mDaysMnthsYrsDoneOnValue = ""
                                mPartMonitorServiceCode = ObjCompMonitorServiceStatus.PartMonitorServiceCode
                                mIsLater = ObjCompMonitorServiceStatus.IsLater
                                mIsApplicable = ObjCompMonitorServiceStatus.IsApplicable
                                ''**************************************************************

                                For Each ObjCompMonitorServiceStatusPeriod In ObjCompMonitorServiceStatus.CompMonitorServiceStatusPeriodList
                                    If ObjCompMonitorServiceStatusPeriod.PeriodID = 1 Then
                                        Freq1 = Freq1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.FrequencyValue
                                        mHoursFreq = ObjCompMonitorServiceStatusPeriod.FrequencyValue.ToString.Split(" ")(0)   'Added By Saylee on 28-Apr-2021 for PreDefined transferred excel sheet
                                        TSNHours = ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").CompCurrentValue.ToString.Split(" ")(0) 'Added By Saylee on 28-Apr-2021 for PreDefined transferred
                                        If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Then
                                            ElapsedTime = ""
                                            RemainingTime = ""
                                            DueAsof = ""
                                            AirframeDueAsof = "" 'Added By Saylee On 26-Jun-2014 For ALL26062014
                                        Else
                                            'Commented by Prashant on 24-July-2009  because we required ElapsedTime for MonitorTypeID=4 ie "Fixed Value" i.e "Expiry"
                                            'If ObjCompMonitorServiceStatus.MonitorTypeID = 4 Then
                                            '    ElapsedTime = ""
                                            'Else
                                            '    ElapsedTime = ElapsedTime &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedValue
                                            'End If
                                            '------------------------------------------------------------------------------------------------------------------------
                                            'ElapsedTime = ElapsedTime &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedValue
                                            ElapsedTime = ElapsedTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AllElapsedValue
                                            RemainingTime = RemainingTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.RemainingValue
                                            DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText
                                            'Added By Saylee On 26-Jun-2014 For ALL26062014
                                            AirframeDueAsof = AirframeDueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                        End If
                                        'Commented by Saylee on 18-Mar-2009
                                        ''InstalledAt = InstalledAt &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").CompInstallationTextFormatted
                                        TSN = TSN & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").CompCurrentValue

                                        'Commented by Saylee on 29-Mar-2010
                                        ''TSO = TSO &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedAtInstall
                                        If ObjCompMonitorServiceStatus.PartMonitorServiceTypeID = 1 And (ObjCompMonitorServiceStatus.IsMaster) And ObjCompMonitorServiceStatus.DoneOnFormatted <> "" Then
                                            TSO = TSO & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedAtInstall
                                        Else
                                            TSO = TSO & IIf(IsExcel, Chr(10), vbCrLf) & ""
                                        End If
                                        '****************************************
                                        'Commented by Saylee on 18-June-2009
                                        ''TSO1 = TSO1 &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").AssemblyInstallationTextFormatted
                                        RemoveAt = RemoveAt & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DueOnValue
                                        DoneOnValue = DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DoneOnValue
                                        mHoursDoneOnValue = ObjCompMonitorServiceStatusPeriod.DoneOnValue.ToString.Split(" ")(0)
                                    End If
                                    If ObjCompMonitorServiceStatusPeriod.PeriodID = 2 Then

                                        If Freq1 = "" Then
                                            Freq1 = Freq1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.FrequencyValueFormatted



                                            If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Then
                                                ElapsedTime = ""
                                                RemainingTime = ""
                                                DueAsof = ""
                                                RemoveAtDate.Text = ""
                                                DoneOnDate.Text = ""

                                                AirframeDueAsof = ""
                                            Else
                                                'Commented by Prashant on 24-July-2009  because we required ElapsedTime for MonitorTypeID=4 ie "Fixed Value" i.e "Expiry"
                                                'If ObjCompMonitorServiceStatus.MonitorTypeID = 4 Then
                                                '    ElapsedTime = ""
                                                'Else
                                                '    ElapsedTime = ElapsedTime &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedValueFormatted
                                                'End If
                                                '------------------------------------------------------------------------------------------------------------------------
                                                'ElapsedTime = ElapsedTime &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedValueFormatted
                                                ElapsedTime = ElapsedTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AllElapsedValueFormatted
                                                RemainingTime = RemainingTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.RemainingValueFormatted

                                                If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet")) Then
                                                    DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ""
                                                    AirframeDueAsof = AirframeDueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ""

                                                Else
                                                    DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                    AirframeDueAsof = AirframeDueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                End If

                                                RemoveAtDate.Text = ObjCompMonitorServiceStatusPeriod.DueOnValue
                                                DoneOnDate.Text = ObjCompMonitorServiceStatusPeriod.DoneOnValue

                                            End If
                                            If (AppSettings("ClientCode") IsNot Nothing) AndAlso
                                               (AppSettings("ClientCode") = "APFT" Or
                                                AppSettings("ClientCode") = "AAP") Then DoneOnDate.Text = ObjCompMonitorServiceStatusPeriod.DoneOnValue
                                            'Commented by Saylee on 18-June-2009
                                            ''InstalledAt = InstalledAt &  IIf(IsExcel, Chr(10), vbCrLf) & ""
                                            TSN = TSN & IIf(IsExcel, Chr(10), vbCrLf) & ""

                                            'Commented by Saylee on 29-Mar-2010
                                            ''TSO = TSO &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedAtInstall
                                            If ObjCompMonitorServiceStatus.PartMonitorServiceTypeID = 1 And (ObjCompMonitorServiceStatus.IsMaster) And ObjCompMonitorServiceStatus.DoneOnFormatted <> "" Then
                                                TSO = TSO & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedAtInstall
                                            Else
                                                TSO = TSO & IIf(IsExcel, Chr(10), vbCrLf) & ""
                                            End If
                                            '****************************************

                                            'Commented by Saylee on 18-June-2009
                                            ''TSO1 = TSO1 &  IIf(IsExcel, Chr(10), vbCrLf) & ""
                                            RemoveAt = RemoveAt & IIf(IsExcel, Chr(10), vbCrLf) & ""

                                            ''Çommented and Added by Saylee on 1-Jan-2020
                                            'DoneOnValue = DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & ""
                                            If DoneOnValue = "" Then
                                                DoneOnValue = IIf(AppSettings("ClientCode") = "STR", ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted, "")
                                            Else
                                                DoneOnValue = DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & IIf(AppSettings("ClientCode") = "STR", ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted, "")
                                            End If

                                        Else
                                            Freq1 = Freq1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.FrequencyValueFormatted
                                            If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Then
                                                ElapsedTime = ""
                                                RemainingTime = ""
                                                DueAsof = ""
                                                RemoveAtDate.Text = ""
                                                DoneOnDate.Text = ""
                                                AirframeDueAsof = ""
                                            Else
                                                'If ObjCompMonitorServiceStatus.MonitorTypeID = 4 Then
                                                '    ElapsedTime = ""
                                                'Else
                                                '    ElapsedTime = ElapsedTime &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedValueFormatted
                                                'End If
                                                'ElapsedTime = ElapsedTime &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedValueFormatted
                                                ElapsedTime = ElapsedTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AllElapsedValueFormatted
                                                RemainingTime = RemainingTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.RemainingValueFormatted
                                                'DueAsof = DueAsof &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet")) Then
                                                    DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ""
                                                    AirframeDueAsof = AirframeDueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ""
                                                Else
                                                    DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                    AirframeDueAsof = AirframeDueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                End If
                                                RemoveAtDate.Text = ObjCompMonitorServiceStatusPeriod.DueOnValue
                                                DoneOnDate.Text = ObjCompMonitorServiceStatusPeriod.DoneOnValue
                                            End If
                                            If (Not AppSettings("ClientCode") Is Nothing) AndAlso
                                               (AppSettings("ClientCode") = "APFT" Or
                                                AppSettings("ClientCode") = "TAAL" Or
                                                AppSettings("ClientCode") = "AAP") Then DoneOnDate.Text = ObjCompMonitorServiceStatusPeriod.DoneOnValue

                                            'Commented by Saylee on 18-June-2009
                                            ''InstalledAt = InstalledAt &  IIf(IsExcel, Chr(10), vbCrLf) & " "
                                            TSN = TSN & IIf(IsExcel, Chr(10), vbCrLf) & " "
                                            'Commented by Saylee on 29-Mar-2010
                                            ''TSO = TSO &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedAtInstall
                                            If ObjCompMonitorServiceStatus.PartMonitorServiceTypeID = 1 And (ObjCompMonitorServiceStatus.IsMaster) And ObjCompMonitorServiceStatus.DoneOnFormatted <> "" Then
                                                TSO = TSO & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedAtInstall
                                            Else
                                                TSO = TSO & IIf(IsExcel, Chr(10), vbCrLf) & ""
                                            End If
                                            '****************************************
                                            'Commented by Saylee on 18-June-2009
                                            ''TSO1 = TSO1 &  IIf(IsExcel, Chr(10), vbCrLf) & " "
                                            RemoveAt = RemoveAt & IIf(IsExcel, Chr(10), vbCrLf) & " "
                                            DoneOnValue = DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & " "
                                        End If
                                        'Added By Saylee on 28-Apr-2021 for PreDefined transferred excel sheet
                                        If ObjCompMonitorServiceStatusPeriod.PeriodUnitID = 3 Then
                                            mDaysMnthsYrsValue = ObjCompMonitorServiceStatusPeriod.FrequencyValue.ToString.Split(" ")(0)
                                            mDaysMnthsYrsName = "Days"
                                        ElseIf ObjCompMonitorServiceStatusPeriod.PeriodUnitID = 4 Then
                                            mDaysMnthsYrsValue = ObjCompMonitorServiceStatusPeriod.FrequencyValue.ToString.Split(" ")(0)
                                            mDaysMnthsYrsName = "Months"
                                        ElseIf ObjCompMonitorServiceStatusPeriod.PeriodUnitID = 5 Then
                                            mDaysMnthsYrsValue = ObjCompMonitorServiceStatusPeriod.FrequencyValue.ToString.Split(" ")(0)
                                            mDaysMnthsYrsName = "Years"
                                        End If
                                        mDaysMnthsYrsDoneOnValue = ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted.ToString.Split(" ")(0)
                                        '******************
                                    End If
									'Added PeriodID=11,15 By Vikrant For ALL 21062012
									'If ObjCompMonitorServiceStatusPeriod.PeriodID = 3 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 4 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 5 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 6 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 7 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 8 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 10 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 12 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 13 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 14 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 11 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 15 Then
									'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
									If ObjCompMonitorServiceStatusPeriod.PeriodID >= 3 Then
										If ObjCompMonitorServiceStatusPeriod.PeriodID = 3 Then
											CSNCycles = ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").CompCurrentValue.ToString.Split(" ")(0) 'Added By Saylee on 28-Apr-2021 for PreDefined transferred
										End If
										If ObjCompMonitorServiceStatusPeriod.PeriodID = 7 Then
											SinceNewLandings = ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").CompCurrentValue.ToString.Split(" ")(0) 'Added By Saylee on 28-Apr-2021 for PreDefined transferred
										End If
										If Freq1 = "" Then
											Freq1 = Freq1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.FrequencyValue
											If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Then
												ElapsedTime = ""
												RemainingTime = ""
												DueAsof = ""
												AirframeDueAsof = ""
											Else
												'If ObjCompMonitorServiceStatus.MonitorTypeID = 4 Then
												'    ElapsedTime = ""
												'Else
												'    ElapsedTime = ElapsedTime &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedValue
												'End If
												'ElapsedTime = ElapsedTime &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedValue
												ElapsedTime = ElapsedTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AllElapsedValue
												RemainingTime = RemainingTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.RemainingValue
												If ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Then
													DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
												Else
													DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText
												End If
												AirframeDueAsof = AirframeDueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
											End If
											'InstalledAt = InstalledAt &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").CompInstallationValue
											'Commented by Saylee on 18-June-2009
											''InstalledAt = InstalledAt &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").CompInstallationTextFormatted
											TSN = TSN & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").CompCurrentValue
											'Commented by Saylee on 29-Mar-2010
											''TSO = TSO &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedAtInstall
											If ObjCompMonitorServiceStatus.PartMonitorServiceTypeID = 1 And (ObjCompMonitorServiceStatus.IsMaster) And ObjCompMonitorServiceStatus.DoneOnFormatted <> "" Then
												TSO = TSO & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedAtInstall
											Else
												TSO = TSO & IIf(IsExcel, Chr(10), vbCrLf) & ""
											End If
											'****************************************
											'TSO1 = TSO1 &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").AssemblyInstallationValue
											'Commented by Saylee on 18-June-2009
											''TSO1 = TSO1 &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").AssemblyInstallationTextFormatted
											RemoveAt = RemoveAt & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DueOnValue
											DoneOnValue = DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DoneOnValue
										Else
											Freq1 = Freq1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.FrequencyValue
											If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Then
												ElapsedTime = ""
												RemainingTime = ""
												DueAsof = ""
												AirframeDueAsof = ""
											Else
												'Commented by Prashant on 24-July-2009  because we required ElapsedTime for MonitorTypeID=4 ie "Fixed Value" i.e "Expiry"
												'If ObjCompMonitorServiceStatus.MonitorTypeID = 4 Then
												'    ElapsedTime = ""
												'Else
												'ElapsedTime = ElapsedTime &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedValue
												'End If
												'-----------------------------------
												'ElapsedTime = ElapsedTime &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedValue
												ElapsedTime = ElapsedTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AllElapsedValue
												RemainingTime = RemainingTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.RemainingValue
												If ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Then
													DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
												Else
													DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText
												End If
												AirframeDueAsof = AirframeDueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
											End If
											'Commented by Saylee on 18-June-2009
											''InstalledAt = InstalledAt &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").CompInstallationTextFormatted
											TSN = TSN & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").CompCurrentValue
											'Commented by Saylee on 29-Mar-2010
											''TSO = TSO &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedAtInstall
											If ObjCompMonitorServiceStatus.PartMonitorServiceTypeID = 1 And (ObjCompMonitorServiceStatus.IsMaster) And ObjCompMonitorServiceStatus.DoneOnFormatted <> "" Then
												TSO = TSO & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedAtInstall
											Else
												TSO = TSO & IIf(IsExcel, Chr(10), vbCrLf) & ""
											End If
											'****************************************
											'Commented by Saylee on 18-June-2009
											''TSO1 = TSO1 &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").AssemblyInstallationTextFormatted
											RemoveAt = RemoveAt & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DueOnValue
											DoneOnValue = DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DoneOnValue
										End If

										'Added By Saylee on 28-Apr-2021 for PreDefined transferred excel sheet
										If ObjCompMonitorServiceStatusPeriod.PeriodID = 3 Then
											mCyclesFreq = ObjCompMonitorServiceStatusPeriod.FrequencyValue.ToString.Split(" ")(0)
											mCyclesDoneOnValue = ObjCompMonitorServiceStatusPeriod.DoneOnValue.ToString.Split(" ")(0)
										ElseIf ObjCompMonitorServiceStatusPeriod.PeriodID = 7 Then
											mLandingsFreq = ObjCompMonitorServiceStatusPeriod.FrequencyValue.ToString.Split(" ")(0)
											mLandingsDoneOnValue = ObjCompMonitorServiceStatusPeriod.DoneOnValue.ToString.Split(" ")(0)
										End If
										'**************************
									End If
								Next
                                AssemblyID = ObjAssemblyStatus.AssemblyID
                                Note = ObjCompMonitorServiceStatus.Notes
                                'CNDC
                                If (AppSettings("ClientCode") IsNot Nothing) AndAlso
                                   (AppSettings("ClientCode") <> "APFT" Or
                                    AppSettings("ClientCode") <> "AAP") Then DoneOnDate.Text = ObjCompMonitorServiceStatus.DoneOn

                                DoneOnDate.Text = ObjCompMonitorServiceStatus.DoneOn

                                If IsExcel Then
                                    Dim ATACode As Integer = ObjCompMonitorServiceStatus.ATACode
                                    If ATACode.ToString.Length < 3 Then
                                        ATAChapter = ATACode.ToString.PadLeft(3, "0"c) + " " + "-" + " " + ObjCompMonitorServiceStatus.ATANomenclature
                                    End If

                                End If

                                'If ServiceTypeID(i + 1) = ObjCompMonitorServiceStatus.PartMonitorServiceTypeID Then
                                ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, MonitorType, MonitorTypeCode, Note, DoneRemrk, Description, _
                                , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, , , , , , _
                                , , , , ATACode, InstalledAt, InstalledAt1, InstalledAt2, TSN, TSO, TSO1, TSO2, RemoveAt, RemoveAt1, RemoveAt2, InstalledAtDate.Date.ToString("g"), RemoveAtDate.Date.ToString("g"), , "", DoneOnValue, DoneOnDate.Date.ToString("g"), IsApplicable:=mIsApplicable, HoursFreq:=mHoursFreq, CyclesFreq:=mCyclesFreq, LandingsFreq:=mLandingsFreq, DaysMnthsYrsName:=mDaysMnthsYrsName, DaysMnthsYrsValue:=mDaysMnthsYrsValue, HoursDoneOnValue:=mHoursDoneOnValue, CyclesDoneOnValue:=mCyclesDoneOnValue, LandingsDoneOnValue:=mLandingsDoneOnValue, DaysMnthsYrsDoneOnValue:=mDaysMnthsYrsDoneOnValue, _
                                Manufacturer:=Manufacturer, InstallationWONo:=InstallationWONo, InstallationRemark:=InstallationRemark, InstPlace:=InstPlace, InstallationDoneBy:=InstallationDoneBy, _
                                TSNHours:=TSNHours, CSNCycles:=CSNCycles, SinceNewDate:=SinceNewDate, SinceNewLandings:=SinceNewLandings, _
                                InstCompHours:=InstCompHours, InstCompCycles:=InstCompCycles, InstCompStartDate:=InstCompStartDate, InstCompLandings:=InstCompLandings, _
                                AssemblyInstHours:=AssemblyInstHours, AssemblyInstCycles:=AssemblyInstCycles, AssemblyInstStartDate:=AssemblyInstStartDate, AssemblyInstLandings:=AssemblyInstLandings, PartMonitorCode:=mPartMonitorServiceCode, PartDesc:=mPartDesc))

                            End If
                        Next
                    Next
                Next
            Next
            'End If
            'Next
        End If

        If IsInsSelect = True Then
            ''Commented and added by Sylee on 13-Aug-2010
            ''mMachineList = MachineList.GetMachineListMonitoringStatusForHardTimeAndDirective(txtAsOnDate.Value.ToString, cmbAircraft.SelectedValue, , , , , , , , , , True, True, , mAssemblylist(cmbAssembly.SelectedIndex).ID.ToString, , , , , , , , , , , ShowCofA, , , , , , , , , , , False, , False, , True, , , , , , True, 6, , True)
            'mMachineList = MachineList.GetMachineListMonitoringStatusForHardTimeAndDirective(AsonDate, cmbAircraft.SelectedValue, , , , , , , , , , True, True, , mAssemblylist(cmbAssembly.SelectedIndex).ID.ToString, , , , , , , , , , , ShowCofA, , , , , , , , ComponentSerialNo, , , False, , False, PartID, True, , , , , , True, 6, , True)
            'For i As Integer = 0 To chkListInspectionType.Items.Count - 1
            'If cmbInspectionType.Items(i).Selected Then
            ''mMachineList = MachineList.GetMachineListMonitoringStatusForHardTimeAndDirective(txtAsOnDate.Value.ToString, cmbAircraft.SelectedValue, , , , , , , , , , True, True, , mAssemblylist(cmbAssembly.SelectedIndex).ID.ToString, , , , , , , , , , , ShowCofA, , , , , , , , , , , False, , False, , True, , , , InspectionTypeID(i + 1), , True, 6, ,  True)
            For Each ObjMachine In mMachineList
                For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                    For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
                        'Added by Deven sir on 18-June-2009
                        InstalledAt = ""
                        TSO1 = ""
                        For Each ObjCompStatusPeriod In ObjCompStatus.CompStatusPeriodList
                            If Not ObjCompStatusPeriod.PeriodID = 2 Then
                                InstalledAt = InstalledAt & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompStatusPeriod.PeriodID, "").CompInstallationTextFormatted
                                TSO1 = TSO1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompStatusPeriod.PeriodID, "").AssemblyInstallationTextFormatted
                            Else
                                If InstalledAt = "" Then InstalledAt = InstalledAt & IIf(IsExcel, Chr(10), vbCrLf) & ""
                                If TSO1 = "" Then TSO1 = TSO1 & IIf(IsExcel, Chr(10), vbCrLf) & ""
                            End If
                        Next
                        '*************************************


                        'Added By Saylee on 28-Apr-2021 for PreDefined transferred excel sheet
                        Dim Manufacturer, InstallationWONo, InstallationRemark, InstPlace, InstallationDoneBy As String
                        Dim TSNHours, CSNCycles, SinceNewDate, SinceNewLandings As String
                        Dim InstCompHours, InstCompCycles, InstCompStartDate, InstCompLandings As String
                        Dim AssemblyInstHours, AssemblyInstCycles, AssemblyInstStartDate, AssemblyInstLandings As String



                        Manufacturer = ""
                        InstallationWONo = ""
                        InstallationRemark = ""
                        InstPlace = ""
                        InstallationDoneBy = ""
                        TSNHours = ""
                        CSNCycles = ""
                        SinceNewDate = ""
                        SinceNewLandings = ""
                        InstCompHours = ""
                        InstCompCycles = ""
                        InstCompStartDate = ""
                        InstCompLandings = ""
                        AssemblyInstHours = ""
                        AssemblyInstCycles = ""
                        AssemblyInstStartDate = ""
                        AssemblyInstLandings = ""
                        mPartDesc = ""
                        If IsForTransfer = True Then

                            Dim mInstallCompStatus As CompStatus = CompStatus.GetCompStatus(ObjCompStatus.ID, ObjAssemblyStatus.ID, txtFromDate.Text)
                            mPartDesc = mInstallCompStatus.Description
                            Manufacturer = mInstallCompStatus.ManufacturerName
                            InstallationWONo = mInstallCompStatus.InstallationWONo
                            InstallationRemark = mInstallCompStatus.InstallationRemark
                            InstPlace = mInstallCompStatus.InstPlace
                            InstallationDoneBy = mInstallCompStatus.InstDoneBy
                            If mInstallCompStatus.CompStatusPeriods.Contains(1) Then

                                InstCompHours = mInstallCompStatus.CompStatusPeriods(1, "").CompInstallationValue.ToString
                                AssemblyInstHours = mInstallCompStatus.CompStatusPeriods(1, "").AssemblyInstallationValue.ToString
                            End If
                            If mInstallCompStatus.CompStatusPeriods.Contains(2) Then

                                InstCompStartDate = mInstallCompStatus.CompStatusPeriods(2, "").CompInstallationValueFormatted.ToString
                                AssemblyInstStartDate = mInstallCompStatus.CompStatusPeriods(2, "").AssemblyInstallationValueFormatted.ToString
                                SinceNewDate = mInstallCompStatus.CompStatusPeriods(2, "").AssemblyInstallationValueFormatted.ToString
                            End If
                            If mInstallCompStatus.CompStatusPeriods.Contains(3) Then

                                InstCompCycles = mInstallCompStatus.CompStatusPeriods(3, "").CompInstallationValue.ToString
                                AssemblyInstCycles = mInstallCompStatus.CompStatusPeriods(3, "").AssemblyInstallationValue.ToString
                            End If
                            If mInstallCompStatus.CompStatusPeriods.Contains(7) Then

                                InstCompLandings = mInstallCompStatus.CompStatusPeriods(7, "").CompInstallationValue.ToString
                                AssemblyInstLandings = mInstallCompStatus.CompStatusPeriods(7, "").AssemblyInstallationValue.ToString
                            End If

                        End If



                        'Added By Prashant 22-July-2009 for Components which has no Inspection
                        'If IsSerSelect = False And i = 0 And Report = 0 And ObjCompStatus.CompMonitorInspStatusList.Count = 0 Then
                        '    ATAChapter = ObjCompStatus.ATACode.ToString + " " + "-" + " " + ObjCompStatus.ATANomenclature
                        '    ATACode = ObjCompStatus.ATACode
                        '    Description = ObjCompStatus.Description
                        '    PartNo = ObjCompStatus.PartName
                        '    CompSerialNo = ObjCompStatus.CompSerialNo
                        '    Position = ObjCompStatus.Position
                        '    MonitorTypeCode = ObjCompStatus.Code
                        '    AssemblyModel = ObjAssemblyStatus.Model
                        '    AssemblySerialNo = ObjAssemblyStatus.SerialNo
                        '    Freq1 = ""
                        '    Freq2 = ""
                        '    Freq3 = ""
                        '    ElapsedTime = ""
                        '    ElapsedTime1 = ""
                        '    ElapsedTime2 = ""
                        '    RemainingTime = ""
                        '    RemainingTime1 = ""
                        '    RemainingTime2 = ""
                        '    DueAsof = ""
                        '    DueAsof1 = ""
                        '    DueAsof2 = ""
                        '    ATACode = ObjCompStatus.ATACode
                        '    InstalledAt1 = ""
                        '    InstalledAt2 = ""
                        '    TSN = ""
                        '    TSO = ""
                        '    TSO2 = ""
                        '    RemoveAt = ""
                        '    RemoveAt1 = ""
                        '    RemoveAt2 = ""
                        '    InstalledAtDate.Text = ObjCompStatus.InstalledOn
                        '    RemoveAtDate.Text = ""
                        '    DoneRemrk = ""
                        '    AssemblyID = ObjAssemblyStatus.AssemblyID
                        '    DoneOnValue = ""
                        '    DoneOnDate.Text = ""
                        '    ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, MonitorType, MonitorTypeCode, Note, , Description, _
                        '    , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, , , , , , _
                        '    , , , , ATACode, InstalledAt, InstalledAt1, InstalledAt2, TSN, TSO, TSO1, TSO2, RemoveAt, RemoveAt1, RemoveAt2, InstalledAtDate.Date.ToString("g"), RemoveAtDate.Date.ToString("g"), , , DoneOnValue, DoneOnDate.Date.ToString("g")))
                        'End If
                        '---------------------------------------------------------------------------------
                        For Each ObjCompMonitorInspStatus In ObjCompStatus.CompMonitorInspStatusList
                            'Commneted By Prashant 22-July-2009 
                            'If ((Report = 1 And ObjCompMonitorInspStatus.MonitorType <> "No Frequency") Or (Report = 0 And ObjCompMonitorInspStatus.MonitorType = "No Frequency")) And (ObjCompMonitorInspStatus.IsApplicable = True) Then
                            '-------------------------------------------------------------------------------------
                            'Added By Prashant 22-July-2009 for records which are not applicable for Report = 0
                            If ((Report = 1 And ObjCompMonitorInspStatus.MonitorType <> "No Frequency") And (ObjCompMonitorInspStatus.IsApplicable = True)) Or _
                                (Report = 0) Then
                                If InspectionTypeID.Contains(ObjCompMonitorInspStatus.PartMonitorInspTypeID) Then
                                    '-------------------------------------------------------------------
                                    ATAChapter = ObjCompMonitorInspStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorInspStatus.ATANomenclature
                                    ATACode = ObjCompMonitorInspStatus.ATACode
                                    Description = ObjCompMonitorInspStatus.Description
                                    PartNo = ObjCompStatus.PartName
                                    CompSerialNo = ObjCompStatus.CompSerialNo
                                    Position = ObjCompStatus.Position
                                    EstimatedDate = ObjCompMonitorInspStatus.EstimatedDateFormatted
                                    MonitorTypeCode = ObjCompMonitorInspStatus.Code
                                    MonitorType = ObjCompMonitorInspStatus.Type
                                    AssemblyModel = ObjAssemblyStatus.Model
                                    AssemblySerialNo = ObjAssemblyStatus.SerialNo
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
                                    ATACode = ObjCompMonitorInspStatus.ATACode
                                    InstalledAt1 = ""
                                    InstalledAt2 = ""
                                    TSN = ""
                                    TSO = ""
                                    TSO2 = ""
                                    RemoveAt = ""
                                    RemoveAt1 = ""
                                    RemoveAt2 = ""
                                    InstalledAtDate.Text = ObjCompStatus.InstalledOn
                                    RemoveAtDate.Text = ""
                                    DoneRemrk = ObjCompMonitorInspStatus.DoneRemark
                                    DoneOnValue = ""
                                    DoneOnDate.Text = ""
                                    AirframeDueAsof = ""

                                    'Added By Saylee on 28-Apr-2021 for PreDefined transferred excel sheet
                                    mHoursFreq = ""
                                    mCyclesFreq = ""
                                    mLandingsFreq = ""
                                    mDaysMnthsYrsName = ""
                                    mDaysMnthsYrsValue = ""
                                    mHoursDoneOnValue = ""
                                    mCyclesDoneOnValue = ""
                                    mLandingsDoneOnValue = ""
                                    mDaysMnthsYrsDoneOnValue = ""
                                    mPartMonitorServiceCode = ObjCompMonitorInspStatus.PartMonitorInspCode
                                    mIsLater = ObjCompMonitorInspStatus.IsLater
                                    mIsApplicable = ObjCompMonitorInspStatus.IsApplicable
                                    ''**************************************************************

                                    For Each ObjCompMonitorInspStatusPeriod In ObjCompMonitorInspStatus.CompMonitorInspStatusPeriodList
                                        If ObjCompMonitorInspStatusPeriod.PeriodID = 1 Then
                                            Freq1 = Freq1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.FrequencyValue
                                            mHoursFreq = ObjCompMonitorInspStatusPeriod.FrequencyValue.ToString.Split(" ")(0)   'Added By Saylee on 28-Apr-2021 for PreDefined transferred excel sheet
                                            TSNHours = ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").CompCurrentValue.ToString.Split(" ")(0) 'Added By Saylee on 28-Apr-2021 for PreDefined transferred


                                            If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Then
                                                ElapsedTime = ""
                                                RemainingTime = ""
                                                DueAsof = ""
                                                AirframeDueAsof = ""
                                            Else
                                                'ElapsedTime = ElapsedTime &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.ElapsedValue
                                                ElapsedTime = ElapsedTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AllElapsedValue
                                                RemainingTime = RemainingTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.RemainingValue
                                                DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText
                                                AirframeDueAsof = AirframeDueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                            End If
                                            'InstalledAt = InstalledAt &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").CompInstallationValue
                                            'Commented by Saylee on 18-June-2009
                                            ''InstalledAt = InstalledAt &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").CompInstallationTextFormatted
                                            TSN = TSN & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").CompCurrentValue
                                            'TSO1 = TSO1 &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").AssemblyInstallationValue
                                            'Commented by Saylee on 18-June-2009
                                            ''TSO1 = TSO1 &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").AssemblyInstallationTextFormatted
                                            RemoveAt = RemoveAt & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DueOnValue
                                            DoneOnValue = DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DoneOnValue
                                            mHoursDoneOnValue = ObjCompMonitorInspStatusPeriod.DoneOnValue.ToString.Split(" ")(0)  'Added By Saylee on 28-Apr-2021 for PreDefined transferred excel sheet
                                        End If
                                        If ObjCompMonitorInspStatusPeriod.PeriodID = 2 Then

                                            If Freq1 = "" Then
                                                Freq1 = Freq1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.FrequencyValueFormatted

                                                If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Then
                                                    ElapsedTime = ""
                                                    RemainingTime = ""
                                                    DueAsof = ""
                                                    RemoveAtDate.Text = ""
                                                    DoneOnDate.Text = ""
                                                    AirframeDueAsof = ""
                                                Else
                                                    'ElapsedTime = ElapsedTime &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.ElapsedValueFormatted
                                                    ElapsedTime = ElapsedTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AllElapsedValueFormatted
                                                    RemainingTime = RemainingTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.RemainingValueFormatted
                                                    If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet")) Then
                                                        DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ""
                                                        AirframeDueAsof = AirframeDueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ""
                                                    Else
                                                        DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                        AirframeDueAsof = AirframeDueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                    End If

                                                    RemoveAtDate.Text = ObjCompMonitorInspStatusPeriod.DueOnValue
                                                    DoneOnDate.Text = ObjCompMonitorInspStatusPeriod.DoneOnValue
                                                End If
                                                'Commented by Saylee on 18-June-2009
                                                '' InstalledAt = InstalledAt &  IIf(IsExcel, Chr(10), vbCrLf) & ""
                                                TSN = TSN & IIf(IsExcel, Chr(10), vbCrLf) & ""
                                                'Commented by Saylee on 18-June-2009
                                                '' TSO1 = TSO1 &  IIf(IsExcel, Chr(10), vbCrLf) & ""
                                                RemoveAt = RemoveAt & IIf(IsExcel, Chr(10), vbCrLf) & ""
                                                ''Çommented and Added by Saylee on 1-Jan-2020
                                                'DoneOnValue = DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & ""
                                                If DoneOnValue = "" Then
                                                    DoneOnValue = IIf(AppSettings("ClientCode") = "STR", ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted, "")
                                                Else
                                                    DoneOnValue = DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & IIf(AppSettings("ClientCode") = "STR", ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted, "")
                                                End If
                                            Else
                                                Freq1 = Freq1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.FrequencyValueFormatted

                                                If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Then
                                                    ElapsedTime = ""
                                                    RemainingTime = ""
                                                    DueAsof = ""
                                                    RemoveAtDate.Text = ""
                                                    DoneOnDate.Text = ""
                                                    AirframeDueAsof = ""
                                                Else
                                                    'ElapsedTime = ElapsedTime &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.ElapsedValueFormatted
                                                    ElapsedTime = ElapsedTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AllElapsedValueFormatted
                                                    RemainingTime = RemainingTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.RemainingValueFormatted
                                                    ''DueAsof = DueAsof &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                    If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet")) Then
                                                        DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ""
                                                        AirframeDueAsof = AirframeDueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ""
                                                    Else
                                                        DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                        AirframeDueAsof = AirframeDueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                    End If
                                                    RemoveAtDate.Text = ObjCompMonitorInspStatusPeriod.DueOnValue
                                                    DoneOnDate.Text = ObjCompMonitorInspStatusPeriod.DoneOnValue
                                                End If
                                                'Commented by Saylee on 18-June-2009
                                                ''  InstalledAt = InstalledAt &  IIf(IsExcel, Chr(10), vbCrLf) & " "
                                                TSN = TSN & IIf(IsExcel, Chr(10), vbCrLf) & " "
                                                'Commented by Saylee on 18-June-2009
                                                ''  TSO1 = TSO1 &  IIf(IsExcel, Chr(10), vbCrLf) & " "
                                                RemoveAt = RemoveAt & IIf(IsExcel, Chr(10), vbCrLf) & " "
                                                DoneOnValue = DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & " "
                                            End If

                                            'Added By Saylee on 28-Apr-2021 for PreDefined transferred excel sheet
                                            If ObjCompMonitorInspStatusPeriod.PeriodUnitID = 3 Then
                                                mDaysMnthsYrsValue = ObjCompMonitorInspStatusPeriod.FrequencyValue.ToString.Split(" ")(0)
                                                mDaysMnthsYrsName = "Days"
                                            ElseIf ObjCompMonitorInspStatusPeriod.PeriodUnitID = 4 Then
                                                mDaysMnthsYrsValue = ObjCompMonitorInspStatusPeriod.FrequencyValue.ToString.Split(" ")(0)
                                                mDaysMnthsYrsName = "Months"
                                            ElseIf ObjCompMonitorInspStatusPeriod.PeriodUnitID = 5 Then
                                                mDaysMnthsYrsValue = ObjCompMonitorInspStatusPeriod.FrequencyValue.ToString.Split(" ")(0)
                                                mDaysMnthsYrsName = "Years"
                                            End If
                                            mDaysMnthsYrsDoneOnValue = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted.ToString.Split(" ")(0)
                                            '******************
                                        End If
										'Added PeriodID=11,15 By Vikrant For ALL 21062012
										'If ObjCompMonitorInspStatusPeriod.PeriodID = 3 Or ObjCompMonitorInspStatusPeriod.PeriodID = 4 Or ObjCompMonitorInspStatusPeriod.PeriodID = 5 Or ObjCompMonitorInspStatusPeriod.PeriodID = 6 Or ObjCompMonitorInspStatusPeriod.PeriodID = 7 Or ObjCompMonitorInspStatusPeriod.PeriodID = 8 Or ObjCompMonitorInspStatusPeriod.PeriodID = 9 Or ObjCompMonitorInspStatusPeriod.PeriodID = 10 Or ObjCompMonitorInspStatusPeriod.PeriodID = 12 Or ObjCompMonitorInspStatusPeriod.PeriodID = 13 Or ObjCompMonitorInspStatusPeriod.PeriodID = 14 Or ObjCompMonitorInspStatusPeriod.PeriodID = 15 Or ObjCompMonitorInspStatusPeriod.PeriodID = 11 Then
										'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
										If ObjCompMonitorInspStatusPeriod.PeriodID >= 3 Then

											'Added By Saylee on 28-Apr-2021 for PreDefined transferred excel sheet
											If ObjCompMonitorInspStatusPeriod.PeriodID = 3 Then
												CSNCycles = ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").CompCurrentValue.ToString.Split(" ")(0) 'Added By Saylee on 28-Apr-2021 for PreDefined transferred
											End If
											If ObjCompMonitorInspStatusPeriod.PeriodID = 7 Then
												SinceNewLandings = ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").CompCurrentValue.ToString.Split(" ")(0) 'Added By Saylee on 28-Apr-2021 for PreDefined transferred
											End If
											'******************
											If Freq1 = "" Then
												Freq1 = Freq1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.FrequencyValue
												If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Then
													ElapsedTime = ""
													RemainingTime = ""
													DueAsof = ""
													AirframeDueAsof = ""
												Else
													'ElapsedTime = ElapsedTime &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.ElapsedValue
													ElapsedTime = ElapsedTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AllElapsedValue
													RemainingTime = RemainingTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.RemainingValue
													If ObjCompMonitorInspStatusPeriod.PeriodID = 9 Then
														DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
													Else
														DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText
													End If
													AirframeDueAsof = AirframeDueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame
												End If
												'Commented by Saylee on 18-June-2009
												'' InstalledAt = InstalledAt &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").CompInstallationTextFormatted
												TSN = TSN & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").CompCurrentValue
												'Commented by Saylee on 18-June-2009
												'' TSO1 = TSO1 &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").AssemblyInstallationTextFormatted
												RemoveAt = RemoveAt & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DueOnValue
												DoneOnValue = DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DueOnValue
											Else
												Freq1 = Freq1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.FrequencyValue
												If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Then
													ElapsedTime = ""
													RemainingTime = ""
													DueAsof = ""
													AirframeDueAsof = ""
												Else
													'ElapsedTime = ElapsedTime &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.ElapsedValue
													ElapsedTime = ElapsedTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AllElapsedValue
													RemainingTime = RemainingTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.RemainingValue
													If ObjCompMonitorInspStatusPeriod.PeriodID = 9 Then
														DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
													Else
														DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText
													End If
													AirframeDueAsof = AirframeDueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame
												End If
												'Commented by Saylee on 18-June-2009
												''InstalledAt = InstalledAt &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").CompInstallationTextFormatted
												TSN = TSN & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").CompCurrentValue
												'Commented by Saylee on 18-June-2009
												'' TSO1 = TSO1 &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").AssemblyInstallationTextFormatted
												RemoveAt = RemoveAt & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DueOnValue
												DoneOnValue = DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DoneOnValue
											End If
											'Added By Saylee on 28-Apr-2021 for PreDefined transferred excel sheet
											If ObjCompMonitorInspStatusPeriod.PeriodID = 3 Then
												mCyclesFreq = ObjCompMonitorInspStatusPeriod.FrequencyValue.ToString.Split(" ")(0)
												mCyclesDoneOnValue = ObjCompMonitorInspStatusPeriod.DoneOnValue.ToString.Split(" ")(0)
											ElseIf ObjCompMonitorInspStatusPeriod.PeriodID = 7 Then
												mLandingsFreq = ObjCompMonitorInspStatusPeriod.FrequencyValue.ToString.Split(" ")(0)
												mLandingsDoneOnValue = ObjCompMonitorInspStatusPeriod.DoneOnValue.ToString.Split(" ")(0)
											End If
											'**************************
										End If
									Next
                                    AssemblyID = ObjAssemblyStatus.AssemblyID
                                    Note = ObjCompMonitorInspStatus.Notes
                                    DoneOnDate.Text = ObjCompMonitorInspStatus.DoneOn


                                    If IsExcel Then
                                        Dim ATACode As Integer = ObjCompMonitorInspStatus.ATACode
                                        If ATACode.ToString.Length < 3 Then
                                            ATAChapter = ATACode.ToString.PadLeft(3, "0"c) + " " + "-" + " " + ObjCompMonitorInspStatus.ATANomenclature
                                        End If

                                    End If
                                    'If InspectionTypeID(i + 1) = ObjCompMonitorInspStatus.PartMonitorInspTypeID Then
                                    ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, MonitorType, MonitorTypeCode, Note, DoneRemrk, Description, _
                                , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, , , , , , _
                                , , , , ATACode, InstalledAt, InstalledAt1, InstalledAt2, TSN, TSO, TSO1, TSO2, RemoveAt, RemoveAt1, RemoveAt2, InstalledAtDate.Date.ToString("g"), RemoveAtDate.Date.ToString("g"), , "", DoneOnValue, DoneOnDate.Date.ToString("g"), _
                                 IsApplicable:=mIsApplicable, HoursFreq:=mHoursFreq, CyclesFreq:=mCyclesFreq, LandingsFreq:=mLandingsFreq, DaysMnthsYrsName:=mDaysMnthsYrsName, DaysMnthsYrsValue:=mDaysMnthsYrsValue, HoursDoneOnValue:=mHoursDoneOnValue, CyclesDoneOnValue:=mCyclesDoneOnValue, _
                                  LandingsDoneOnValue:=mLandingsDoneOnValue, DaysMnthsYrsDoneOnValue:=mDaysMnthsYrsDoneOnValue, _
                                    Manufacturer:=Manufacturer, InstallationWONo:=InstallationWONo, InstallationRemark:=InstallationRemark, InstPlace:=InstPlace, InstallationDoneBy:=InstallationDoneBy, _
                                    TSNHours:=TSNHours, CSNCycles:=CSNCycles, SinceNewDate:=SinceNewDate, SinceNewLandings:=SinceNewLandings, _
                                    InstCompHours:=InstCompHours, InstCompCycles:=InstCompCycles, InstCompStartDate:=InstCompStartDate, InstCompLandings:=InstCompLandings, _
                                    AssemblyInstHours:=AssemblyInstHours, AssemblyInstCycles:=AssemblyInstCycles, AssemblyInstStartDate:=AssemblyInstStartDate, AssemblyInstLandings:=AssemblyInstLandings, PartMonitorCode:=mPartMonitorServiceCode, PartDesc:=mPartDesc))
                                End If
                            End If
                        Next
                    Next
                Next
            Next
            'End If
            'Next
        End If

        If IsModSelect = True Then
            For Each ObjMachine In mMachineList
                For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                    For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
                        'Added by Deven sir on 18-June-2009
                        InstalledAt = ""
                        TSO1 = ""
                        For Each ObjCompStatusPeriod In ObjCompStatus.CompStatusPeriodList
                            If Not ObjCompStatusPeriod.PeriodID = 2 Then
                                InstalledAt = InstalledAt & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompStatusPeriod.PeriodID, "").CompInstallationTextFormatted
                                TSO1 = TSO1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompStatusPeriod.PeriodID, "").AssemblyInstallationTextFormatted
                            Else
                                If InstalledAt = "" Then InstalledAt = InstalledAt & IIf(IsExcel, Chr(10), vbCrLf) & ""
                                If TSO1 = "" Then TSO1 = TSO1 & IIf(IsExcel, Chr(10), vbCrLf) & ""
                            End If
                        Next
                        '*************************************

                        'Added By Saylee on 28-Apr-2021 for PreDefined transferred excel sheet
                        Dim Manufacturer, InstallationWONo, InstallationRemark, InstPlace, InstallationDoneBy As String
                        Dim TSNHours, CSNCycles, SinceNewDate, SinceNewLandings As String
                        Dim InstCompHours, InstCompCycles, InstCompStartDate, InstCompLandings As String
                        Dim AssemblyInstHours, AssemblyInstCycles, AssemblyInstStartDate, AssemblyInstLandings As String



                        Manufacturer = ""
                        InstallationWONo = ""
                        InstallationRemark = ""
                        InstPlace = ""
                        InstallationDoneBy = ""
                        TSNHours = ""
                        CSNCycles = ""
                        SinceNewDate = ""
                        SinceNewLandings = ""
                        InstCompHours = ""
                        InstCompCycles = ""
                        InstCompStartDate = ""
                        InstCompLandings = ""
                        AssemblyInstHours = ""
                        AssemblyInstCycles = ""
                        AssemblyInstStartDate = ""
                        AssemblyInstLandings = ""
                        mPartDesc = ""

                        If IsForTransfer = True Then

                            Dim mInstallCompStatus As CompStatus = CompStatus.GetCompStatus(ObjCompStatus.ID, ObjAssemblyStatus.ID, txtFromDate.Text)
                            mPartDesc = mInstallCompStatus.Description
                            Manufacturer = mInstallCompStatus.ManufacturerName
                            InstallationWONo = mInstallCompStatus.InstallationWONo
                            InstallationRemark = mInstallCompStatus.InstallationRemark
                            InstPlace = mInstallCompStatus.InstPlace
                            InstallationDoneBy = mInstallCompStatus.InstDoneBy
                            If mInstallCompStatus.CompStatusPeriods.Contains(1) Then

                                InstCompHours = mInstallCompStatus.CompStatusPeriods(1, "").CompInstallationValue.ToString
                                AssemblyInstHours = mInstallCompStatus.CompStatusPeriods(1, "").AssemblyInstallationValue.ToString
                            End If
                            If mInstallCompStatus.CompStatusPeriods.Contains(2) Then
                                InstCompStartDate = mInstallCompStatus.CompStatusPeriods(2, "").CompInstallationValueFormatted.ToString
                                AssemblyInstStartDate = mInstallCompStatus.CompStatusPeriods(2, "").AssemblyInstallationValueFormatted.ToString
                                SinceNewDate = mInstallCompStatus.CompStatusPeriods(2, "").AssemblyInstallationValueFormatted.ToString 'New SmartDate(txtAsOnDate.Text.ToString).FormattedText  'Added By Saylee on 28-Apr-2021 for PreDefined transferred
                            End If
                            If mInstallCompStatus.CompStatusPeriods.Contains(3) Then

                                InstCompCycles = mInstallCompStatus.CompStatusPeriods(3, "").CompInstallationValue.ToString
                                AssemblyInstCycles = mInstallCompStatus.CompStatusPeriods(3, "").AssemblyInstallationValue.ToString
                            End If
                            If mInstallCompStatus.CompStatusPeriods.Contains(7) Then

                                InstCompLandings = mInstallCompStatus.CompStatusPeriods(7, "").CompInstallationValue.ToString
                                AssemblyInstLandings = mInstallCompStatus.CompStatusPeriods(7, "").AssemblyInstallationValue.ToString
                            End If

                        End If



                        For Each ObjCompMonitorModStatus In ObjCompStatus.CompMonitorModStatusList
                            ATAChapter = ObjCompMonitorModStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorModStatus.ATANomenclature
                            ATACode = ObjCompMonitorModStatus.ATACode
                            Description = ObjCompMonitorModStatus.Description
                            PartNo = ObjCompStatus.PartName
                            CompSerialNo = ObjCompStatus.CompSerialNo
                            Position = ObjCompStatus.Position
                            EstimatedDate = ObjCompMonitorModStatus.EstimatedDateFormatted
                            MonitorTypeCode = ObjCompMonitorModStatus.Code
                            MonitorType = ObjCompMonitorModStatus.Type
                            AssemblyModel = ObjAssemblyStatus.Model
                            AssemblySerialNo = ObjAssemblyStatus.SerialNo
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
                            ATACode = ObjCompMonitorModStatus.ATACode
                            InstalledAt1 = ""
                            InstalledAt2 = ""
                            TSN = ""
                            TSO = ""
                            TSO2 = ""
                            RemoveAt = ""
                            RemoveAt1 = ""
                            RemoveAt2 = ""
                            InstalledAtDate.Text = ObjCompStatus.InstalledOn
                            RemoveAtDate.Text = ""
                            DoneRemrk = ObjCompMonitorModStatus.DoneRemark
                            DoneOnValue = ""
                            DoneOnDate.Text = ""
                            AirframeDueAsof = ""

                            'Added By Saylee on 28-Apr-2021 for PreDefined transferred excel sheet
                            mHoursFreq = ""
                            mCyclesFreq = ""
                            mLandingsFreq = ""
                            mDaysMnthsYrsName = ""
                            mDaysMnthsYrsValue = ""
                            mHoursDoneOnValue = ""
                            mCyclesDoneOnValue = ""
                            mLandingsDoneOnValue = ""
                            mDaysMnthsYrsDoneOnValue = ""
                            mPartMonitorModCode = ObjCompMonitorModStatus.PartMonitorModCode
                            mIsLater = ObjCompMonitorModStatus.IsLater
                            mIsApplicable = ObjCompMonitorModStatus.IsApplicable
                            ''**************************************************************
                            ''**************************************************************

                            If ObjCompMonitorModStatus.IsApplicable = True And ObjCompMonitorModStatus.IsCompleted = False Then
                                EstimatedDate = ObjCompMonitorModStatus.EstimatedDateFormatted  'Added by Saylee on 10-June-2009
                            End If
                            IssueDate.Text = ObjCompMonitorModStatus.IssueDateFormatted
                            IsApplicable = ObjCompMonitorModStatus.IsApplicable
                            If ObjCompMonitorModStatus.Number = "99-26-21" Or ObjCompMonitorModStatus.Number = "99-08-23" Then
                                Dim a As Integer = 0
                            End If
                            For Each ObjCompMonitorModStatusPeriod In ObjCompMonitorModStatus.CompMonitorModStatusPeriodList
                                If ObjCompMonitorModStatusPeriod.PeriodID = 2 Then
                                    If Freq1 = "" Then
                                        If (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted = True) Or (ObjCompMonitorModStatus.IsApplicable = False) Then
                                            RemainingTime = ""
                                            DueAsof = ""
                                            'Commented & added by Saylee on 1-Nov-2018 , as per BINU Frequency should be visible
                                            'Freq1 = ""
                                            Freq1 = ObjCompMonitorModStatusPeriod.FrequencyValueFormatted
                                            '***************************
                                            ElapsedTime = ""
                                        Else
                                            Freq1 = ObjCompMonitorModStatusPeriod.FrequencyValueFormatted
                                            ElapsedTime = ObjCompMonitorModStatusPeriod.ElapsedValueFormatted
                                            RemainingTime = ObjCompMonitorModStatusPeriod.RemainingValueFormatted
                                            DueAsof = ObjCompMonitorModStatusPeriod.DueOnValueFormatted
                                        End If

                                        If ObjCompMonitorModStatus.DoneOn <> "" Then DoneOnValue = DoneOnValue & ObjCompMonitorModStatusPeriod.DoneOnValueFormatted & IIf(IsExcel, Chr(10), vbCrLf)



                                    Else
                                        If (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted = True) Or (ObjCompMonitorModStatus.IsApplicable = False) Then
                                            RemainingTime = ""
                                            DueAsof = ""
                                            'Commented & added by Saylee on 1-Nov-2018 , as per BINU Frequency should be visible
                                            'Freq1 = ""
                                            Freq1 = Freq1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.FrequencyValueFormatted
                                            '*************************************
                                            ElapsedTime = ""
                                        Else
                                            Freq1 = Freq1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.FrequencyValueFormatted
                                            ElapsedTime = ElapsedTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.ElapsedValueFormatted
                                            RemainingTime = RemainingTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.RemainingValueFormatted
                                            DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.DueOnValueFormatted
                                        End If

                                        If ObjCompMonitorModStatus.DoneOn <> "" Then DoneOnValue = DoneOnValue + ObjCompMonitorModStatusPeriod.DoneOnValueFormatted & IIf(IsExcel, Chr(10), vbCrLf)

                                    End If
                                Else
                                    If Freq1 = "" Then
                                        If (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted = True) Or (ObjCompMonitorModStatus.IsApplicable = False) Then
                                            RemainingTime = ""
                                            DueAsof = ""
                                            'Commented & added by Saylee on 1-Nov-2018 , as per BINU Frequency should be visible
                                            'Freq1 = ""
                                            Freq1 = ObjCompMonitorModStatusPeriod.FrequencyValue
                                            '*************************************
                                            ElapsedTime = ""
                                        Else
                                            Freq1 = ObjCompMonitorModStatusPeriod.FrequencyValue
                                            ElapsedTime = ObjCompMonitorModStatusPeriod.ElapsedValue
                                            RemainingTime = ObjCompMonitorModStatusPeriod.RemainingValue
                                            DueAsof = ObjCompMonitorModStatusPeriod.DueOnValue
                                        End If
                                        If ObjCompMonitorModStatus.MonitorType = "No Frequency" Or ObjCompMonitorModStatus.IsApplicable = False Then 'Added By Prashant 28-Sep-2018
                                            DoneOnValue = ""
                                        Else
                                            DoneOnValue = DoneOnValue + ObjCompMonitorModStatusPeriod.DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf)
                                        End If
                                    Else
                                        If (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted = True) Or (ObjCompMonitorModStatus.IsApplicable = False) Then
                                            RemainingTime = ""
                                            DueAsof = ""
                                            'Commented & added by Saylee on 1-Nov-2018 , as per BINU Frequency should be visible
                                            'Freq1 = ""
                                            Freq1 = Freq1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.FrequencyValue
                                            '*************************************
                                            ElapsedTime = ""
                                        Else
                                            Freq1 = Freq1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.FrequencyValue
                                            ElapsedTime = ElapsedTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.ElapsedValue
                                            RemainingTime = RemainingTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.RemainingValue
                                            DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.DueOnValue
                                        End If
                                        If ObjCompMonitorModStatusPeriod.DoneOnValue = "" Then
                                            If ObjCompMonitorModStatus.MonitorType = "No Frequency" Or ObjCompMonitorModStatus.IsApplicable = False Then 'Added By Prashant 28-Sep-2018
                                                DoneOnValue = ""
                                            Else
                                                DoneOnValue = DoneOnValue & ObjCompMonitorModStatusPeriod.DoneOnValue
                                            End If
                                        Else
                                            If ObjCompMonitorModStatus.MonitorType = "No Frequency" Or ObjCompMonitorModStatus.IsApplicable = False Then 'Added By Prashant 28-Sep-2018
                                                DoneOnValue = ""
                                            Else
                                                DoneOnValue = DoneOnValue + ObjCompMonitorModStatusPeriod.DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf)
                                            End If
                                        End If
                                    End If
                                End If

                                'Added By Saylee on 28-Apr-2021 for PreDefined transferred excel sheet
                                If ObjCompMonitorModStatusPeriod.PeriodID = 1 Then
                                    mHoursFreq = ObjCompMonitorModStatusPeriod.FrequencyValue.ToString.Split(" ")(0)   'Added By Saylee on 28-Apr-2021 for PreDefined transferred excel sheet
                                    mHoursDoneOnValue = ObjCompMonitorModStatusPeriod.DoneOnValue.ToString.Split(" ")(0)
                                    TSNHours = ObjCompStatus.CompStatusPeriodList(ObjCompMonitorModStatusPeriod.PeriodID, "").CompCurrentValue.ToString.Split(" ")(0) 'Added By Saylee on 28-Apr-2021 for PreDefined transferred

                                ElseIf ObjCompMonitorModStatusPeriod.PeriodID = 2 Then
                                    If ObjCompMonitorModStatusPeriod.PeriodUnitID = 3 Then
                                        mDaysMnthsYrsValue = ObjCompMonitorModStatusPeriod.FrequencyValue.ToString.Split(" ")(0)
                                        mDaysMnthsYrsName = "Days"
                                    ElseIf ObjCompMonitorModStatusPeriod.PeriodUnitID = 4 Then
                                        mDaysMnthsYrsValue = ObjCompMonitorModStatusPeriod.FrequencyValue.ToString.Split(" ")(0)
                                        mDaysMnthsYrsName = "Months"
                                    ElseIf ObjCompMonitorModStatusPeriod.PeriodUnitID = 5 Then
                                        mDaysMnthsYrsValue = ObjCompMonitorModStatusPeriod.FrequencyValue.ToString.Split(" ")(0)
                                        mDaysMnthsYrsName = "Years"
                                    End If
                                    mDaysMnthsYrsDoneOnValue = ObjCompMonitorModStatusPeriod.DoneOnValueFormatted.ToString.Split(" ")(0)

                                ElseIf ObjCompMonitorModStatusPeriod.PeriodID = 3 Then
                                    mCyclesFreq = ObjCompMonitorModStatusPeriod.FrequencyValue.ToString.Split(" ")(0)
                                    mCyclesDoneOnValue = ObjCompMonitorModStatusPeriod.DoneOnValue.ToString.Split(" ")(0)
                                    CSNCycles = ObjCompStatus.CompStatusPeriodList(ObjCompMonitorModStatusPeriod.PeriodID, "").CompCurrentValue.ToString.Split(" ")(0) 'Added By Saylee on 28-Apr-2021 for PreDefined transferred

                                ElseIf ObjCompMonitorModStatusPeriod.PeriodID = 7 Then
                                    mLandingsFreq = ObjCompMonitorModStatusPeriod.FrequencyValue.ToString.Split(" ")(0)
                                    mLandingsDoneOnValue = ObjCompMonitorModStatusPeriod.DoneOnValue.ToString.Split(" ")(0)
                                    SinceNewLandings = ObjCompStatus.CompStatusPeriodList(ObjCompMonitorModStatusPeriod.PeriodID, "").CompCurrentValue.ToString.Split(" ")(0) 'Added By Saylee on 28-Apr-2021 for PreDefined transferred

                                End If
                                '**************************




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

                            DoneOnDate.Text = ObjCompMonitorModStatus.DoneOn
                            DoneRemrk = ObjCompMonitorModStatus.DoneRemark
                            If DoneRemrk = "" Then
                                DoneRemrk = "----"
                            End If
                            Applicability = ObjCompMonitorModStatus.Applicability
                            ''
                            If Applicability = "" Then
                                Applicability = "----"
                            End If
                            ''
                            ComplianceRequirement = ObjCompMonitorModStatus.ComplianceRequirement
                            If ComplianceRequirement = "" Then
                                ComplianceRequirement = "----"
                            End If
                            mPartMonitorModCode = ObjCompMonitorModStatus.PartMonitorModCode
                            If mPartMonitorModCode = "" And IsForTransfer = False Then
                                mPartMonitorModCode = "----"
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


                            If IsExcel Then
                                Dim ATACode As Integer = ObjCompMonitorModStatus.ATACode
                                If ATACode.ToString.Length < 3 Then
                                    ATAChapter = ATACode.ToString.PadLeft(3, "0"c) + " " + "-" + " " + ObjCompMonitorModStatus.ATANomenclature
                                End If

                            End If

                            ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, MonitorType, MonitorTypeCode, Note, DoneRemrk, Description, _
                                                                , EstimatedDate, , , Freq1, Freq1, Freq1, ElapsedTime, ElapsedTime, ElapsedTime, RemainingTime, RemainingTime, RemainingTime, _
                                                               DueAsof, DueAsof, DueAsof, AssemblyModel, , , , , , , , , , ATACode, , , , , , , , , , , , , Number, Reference, DoneOnValue, DoneOnDate.ToString, _
                                                               , Applicability, ComplianceRequirement, , , , , , , , , , , ObjCompMonitorModStatus.Code, , , , IssueDate.Date.ToString("g"), IsApplicable, , , , , , , , , , , _
                                                                HoursFreq:=mHoursFreq, CyclesFreq:=mCyclesFreq, LandingsFreq:=mLandingsFreq, DaysMnthsYrsName:=mDaysMnthsYrsName, _
                                                               DaysMnthsYrsValue:=mDaysMnthsYrsValue, HoursDoneOnValue:=mHoursDoneOnValue, CyclesDoneOnValue:=mCyclesDoneOnValue, LandingsDoneOnValue:=mLandingsDoneOnValue, _
                                                               DaysMnthsYrsDoneOnValue:=mDaysMnthsYrsDoneOnValue, PartMonitorCode:=ObjCompMonitorModStatus.PartMonitorModCode, _
                                                               TSNHours:=TSNHours, CSNCycles:=CSNCycles, SinceNewDate:=SinceNewDate, SinceNewLandings:=SinceNewLandings, _
                                                               InstCompHours:=InstCompHours, InstCompCycles:=InstCompCycles, InstCompStartDate:=InstCompStartDate, InstCompLandings:=InstCompLandings, _
                                                               AssemblyInstHours:=AssemblyInstHours, AssemblyInstCycles:=AssemblyInstCycles, AssemblyInstStartDate:=AssemblyInstStartDate, AssemblyInstLandings:=AssemblyInstLandings))

                        Next
                    Next
                Next
            Next
            'End If
            'Next
        End If
        Return ReportMaintenanceDetails
    End Function
    Private Sub SetAssemlbyActivityExcel()
        Dim PeriodColumnsForExportToExcel As New List(Of String)
        Dim mCompanyDetail As New CompanyDetail

        IsExcel = True
        ReportMaintenanceDetails = Nothing
        Report = Nothing
        ReportStatusList = Nothing
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsReportMaintenanceDetail

        ReportStatusList = New rptStatusList
        ReportMaintenanceDetails = New ReportMaintenanceDetailList


        ReportAssemblyDetail(IsForTransfer:=True)

        If ReportMaintenanceDetails.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub

        End If

        ds.Clear()

        ReportData = New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
      mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
      mCompanyDetail.WebSite, ReportLabel, New SmartDate(txtFromDate.Text).FormattedText, IIf(Aircraft = "", "ALL", Aircraft), IIf(Assembly1 = "", "ALL", Assembly1), Type.ToString.TrimEnd(","), "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", "", "", AppSettings("Logo")) 'Changed By Utkarsh On 08-Apr-2011



        da.Fill(ds, "ExcelReportMaintenanceDetailList", ReportMaintenanceDetails)
        da.Fill(ds, "ReportData", ReportData)

        Dim columnToRemove As String() = { _
                                            "ID", _
                                            "Code", _
                                            "Name", _
                                            "Model", _
                                            "SerialNo", _
                                            "Freq2", _
                                            "Freq3", _
                                            "ElapsedTime1", _
                                            "ElapsedTime2", _
                                            "RemainingTime1", _
                                            "RemainingTime2", _
                                            "DueAsof1", _
                                            "DueAsof2", _
                                            "Note", _
                                            "AssemblySerialNo", _
                                            "EstimatedDate", _
                                            "ComponentInfo", _
                                            "RegNo", _
                                            "AssemblyType", _
                                            "SinceNew", _
                                            "SinceNew1", _
                                            "SinceNew2", _
                                            "DoneAt", _
                                            "DoneAt1", _
                                            "DoneAt2", _
                                            "AssemblyModel", _
                                            "MinimumRemainingValue", _
                                            "AssemblyTypeID", _
                                            "MaintenanceEvent", _
                                            "InstalledAt1", _
                                            "InstalledAt2", _
                                            "TSO1", _
                                            "TSO2", _
                                            "RemoveAt1", _
                                            "RemoveAt2", _
                                            "DoneWONo", _
                                            "DetailID", _
                                            "AssemblyDueAsof", _
                                            "AssemblyDueAsof1", _
                                            "AssemblyDueAsof2", _
                                            "Extension", _
                                            "Extension1", _
                                            "Extension2", _
                                            "ExtensionDate", _
                                            "ApprovalRemark", _
                                            "Customer", _
                                            "MaintenanceTypeID", _
                                            "MaintenanceTypeName", _
                                            "DueStatus", _
                                            "TimeSinceNew", _
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
                                            "RemoveAt", _
                                            "MonitorType", _
                                            "DoneONValueForAssembly", _
                                            "MachineID", "ModelID", "DiffCompInstDoneOnValue", "MaintenanceOnExcel", "MaintenanceInformationExcel", _
                                            "MaintenanceInfoExcel", "FrequencyExcel", "SinceNewAllExcel", "ElapsedAllExcel", "EffectiveFromAll", "EffectiveFromAllExcel", _
                                            "DoneAtAllExcel", "ExtensionAllExcel", "DueAsofAllExcel", "AssDueAsofAllExcel", "RemainingTimeAllExcel", _
                                            "MaintenanceInformationForExcel", "EROQtyNosForMaterialMgmtReport", "POQtyNosForMaterialMgmtReport", "PONosForMaterialMgmtReport", _
                                            "POQtyForMaterialMgmtReport", "ERONosForMaterialMgmtReport", "EROQtyForMaterialMgmtReport", _
                                            "UnserviceableStockQty", "ServiceableStockQty", "BinCardTotalQty", "Area", "Zone", "RecordID", "IsMaster", _
                                            "ApplicabilityForExcel", "ReferenceForExcel", "Note", "ThresholdAccordingToTypeIDForExcel", "FrequencyAccordingToTypeIDForExcel", _
                                            "DueAsOfAssemblyOrCompForExcel", "DueAsOfAirframeForExcel", "RemainingForExcel", _
                                            "MaintenanceActivityType", "Freq1", "ElapsedTime", "RemainingTime", "DueAsof", "EstDate", "InstalledAt", "TSN", "TSO", "RemoveAtDate", _
                                            "ModelEstimatedManHours", "SourceDoc", "IsRII", "ReqNumber", "LinkedMaintenanceActivityCount", "WONoExcel", "ATAChapter", "Description", "MonitorTypeCode", "DoneOnValue", "MonitorTypeCode", _
                                            "Manufacturer", "InstalledAtDate", "InstallationWONo", "InstallationRemark", "InstallationDoneBy", "InstPlace", "TSNHours", "SinceNewDate", _
                                            "SinceNewLandings", "CSNCycles", "InstCompHours", "InstCompStartDate", "InstCompHours", "InstCompLandings", "InstCompCycles", "AssemblyInstHours", "AssemblyInstStartDate", _
                                            "AssemblyInstLandings", "AssemblyInstCycles", "PartNo", "CompSerialNo", "CompSerialNo", "PartDesc", "Position", "PartMonitorCode" _
           }

        For i As Integer = 0 To columnToRemove.Length - 1
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains(columnToRemove(i)) Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns.Remove(columnToRemove(i))
            End If
        Next
        Dim columnscnt As Integer = ds.Tables("ExcelReportMaintenanceDetailList").Columns.Count
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("Remark").SetOrdinal(columnscnt - 1)




        ds.Tables("ExcelReportMaintenanceDetailList").Columns("ModelMonitorModCode").SetOrdinal(0)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("ATACode").SetOrdinal(1)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("MonitorTypeWithCode").SetOrdinal(2)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("ModificationNumber").SetOrdinal(3)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("DescriptionForExcel").SetOrdinal(4)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("Reference").SetOrdinal(5)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("IssueDate").SetOrdinal(6)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("SupersededByADNumber").SetOrdinal(7)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("Applicability").SetOrdinal(8)

        ds.Tables("ExcelReportMaintenanceDetailList").Columns("ComplianceRequirement").SetOrdinal(9)

        ds.Tables("ExcelReportMaintenanceDetailList").Columns("NoteForExcel").SetOrdinal(10)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("RequiredManHours").SetOrdinal(11)

        ds.Tables("ExcelReportMaintenanceDetailList").Columns("HoursFreq").SetOrdinal(12)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("CyclesFreq").SetOrdinal(13)


        ds.Tables("ExcelReportMaintenanceDetailList").Columns("DaysMnthsYrsName").SetOrdinal(14)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("DaysMnthsYrsValue").SetOrdinal(15)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("LandingsFreq").SetOrdinal(16)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("DoneOnDate").SetOrdinal(17)

        ds.Tables("ExcelReportMaintenanceDetailList").Columns("HoursDoneOnValue").SetOrdinal(18)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("CyclesDoneOnValue").SetOrdinal(19)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("DaysMnthsYrsDoneOnValue").SetOrdinal(20)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("LandingsDoneOnValue").SetOrdinal(21)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("IsApplicable").SetOrdinal(22)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("IsLater").SetOrdinal(23)

        ds.Tables("ExcelReportMaintenanceDetailList").Columns("Remark").SetOrdinal(24)



        Dim DueLabel As String = "DueAsof"
        For i As Integer = 0 To ds.Tables("ExcelReportMaintenanceDetailList").Columns.Count - 1
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "ModelMonitorModCode" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Code"
            End If

            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "ModificationNumber" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Directive No"
            End If

            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "MonitorTypeCode" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Type"
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "StatusTypeName" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Status"
            End If

        Next

        If chkService.Checked Or chkInspection.Checked Then
            ds.Tables("ExcelReportMaintenanceDetailList").Columns.Remove("Directive No")
            ds.Tables("ExcelReportMaintenanceDetailList").Columns.Remove("IssueDate")
            ds.Tables("ExcelReportMaintenanceDetailList").Columns.Remove("SupersededByADNumber")
            ds.Tables("ExcelReportMaintenanceDetailList").Columns.Remove("Applicability")
            ds.Tables("ExcelReportMaintenanceDetailList").Columns.Remove("ComplianceRequirement")
        End If


        Dim columnToRemoveCriteria As String() = { _
                                                  "ReportDate", _
                                                  "ID", _
                                                  "CompanyName", _
                                                  "Address", _
                                                  "Tel1", _
                                                  "Tel2", _
                                                  "Fax", _
                                                  "Email", _
                                                  "WebSite", _
                                                  "ReportName", _
                                                  "SearchStr5", _
                                                  "SearchStr6", _
                                                  "SearchStr7", _
                                                  "SearchStr8", _
                                                  "SearchStr9", _
                                                  "ProductVersion", _
                                                  "SINote", _
                                                  "CurrencyName", _
                                                  "CurrencySymbol", _
                                                  "SearchStr10", _
                                                  "SearchStr11", _
                                                  "SearchStr12", _
                                                  "SearchStr13", _
                                                  "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", "ShortName", _
                                                 "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"
                                                 }

        For i As Integer = 0 To columnToRemoveCriteria.Length - 1
            If ds.Tables("ReportData").Columns.Contains(columnToRemoveCriteria(i)) Then
                ds.Tables("ReportData").Columns.Remove(columnToRemoveCriteria(i))
            End If
        Next

        For i As Integer = 0 To ds.Tables("ReportData").Columns.Count - 1
            If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr1" Then
                ds.Tables("ReportData").Columns(i).ColumnName = "AsOnDate"
            End If
            If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr2" Then
                ds.Tables("ReportData").Columns(i).ColumnName = "Reg No."
            End If
            If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr3" Then
                ds.Tables("ReportData").Columns(i).ColumnName = "Assembly"
            End If
            If ds.Tables("ReportData").Columns(i).ColumnName = "SearchStr4" Then
                ds.Tables("ReportData").Columns(i).ColumnName = "Directive"
            End If
        Next

        ''Dim dataview As DataView = ds.Tables("ExcelReportMaintenanceDetailList").DefaultView
        ''dataview.Sort = "Directive No"

        ds.Tables("ReportData").TableName = "Searching Criteria"
        'ds.Tables("ExcelReportMaintenanceDetailList").TableName = Assembly1
        'Session("DataTableToBeFormattedForExportToExcel") = Assembly1
        Dim dsNew As New DataSet
        dsNew.Clear()
		Session("ExcelFileName") = ReportLabel
		'dsNew.Merge(ds.Tables("Searching Criteria"))
		'dsNew.Merge(dataview.ToTable())
		dsNew.Merge(ds.Tables("ExcelReportMaintenanceDetailList"))

		PeriodColumnsForExportToExcel.AddRange(New String() {"Frequency", "ElapsedTime", "RemainingTime", "DueAsof", "Last Carried Out"})
		Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
		Session("dsNew") = dsNew
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
    End Sub
    Private Sub SetCompActivityExcel()

        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsReportMaintenanceDetail
        Dim SearchingCriteria As ReportData
        Dim mCompanyDetail As New CompanyDetail
        Dim ReportLabel As String

        Dim PeriodColumnsForExportToExcel As New List(Of String)
        ReportMaintenanceDetails = New ReportMaintenanceDetailList

        ReportDetail(IsForTransfer:=True)

        If ReportMaintenanceDetails.Count <= 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        ElseIf ReportMaintenanceDetails.Count > 0 Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1507)
        End If



        If AssemblyType = "(All)" Then
            ReportLabel = "Hard Time Status of Components"
        Else
            ReportLabel = AssemblyType + " Hard Time Status of Components"
        End If

        SearchingCriteria = New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
         mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
         mCompanyDetail.WebSite, ReportLabel, txtFromDate.Text.Trim, IIf(Aircraft = "", "ALL", Aircraft), IIf(Assembly1 = "", "ALL", Assembly1), "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", AppSettings("Logo"))  'Changed by Utkarsh On 7-Apr-2011



        If ReportMaintenanceDetails.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 726)
        End If

        ds.Clear()

        'Dim List = (From c In ReportMaintenanceDetails Order By ATAChapter
        '                             Select c Order By ATAChapter).ToList

        da.Fill(ds, "ExcelReportMaintenanceDetailList", ReportMaintenanceDetails)
        da.Fill(ds, "ExcelReport", SearchingCriteria)

        Dim columnToRemove As String()

        If chkService.Checked Or chkInspection.Checked Then


            columnToRemove = { _
                                                "ID", _
                                                "Code", _
                                                "Name", _
                                                "Model", _
                                                "SerialNo", _
                                                "Freq2", _
                                                "Freq3", _
                                                "ElapsedTime1", _
                                                "ElapsedTime2", _
                                                "RemainingTime1", _
                                                "RemainingTime2", _
                                                "DueAsof1", _
                                                "DueAsof2", _
                                                "Note", _
                                                "AssemblySerialNo", _
                                                "EstimatedDate", _
                                                "ComponentInfo", _
                                                "RegNo", _
                                                "AssemblyType", _
                                                "SinceNew", _
                                                "SinceNew1", _
                                                "SinceNew2", _
                                                "DoneAt", _
                                                "DoneAt1", _
                                                "DoneAt2", _
                                                "AssemblyModel", _
                                                "MinimumRemainingValue", _
                                                "AssemblyTypeID", _
                                                "MaintenanceEvent", _
                                                "InstalledAt1", _
                                                "InstalledAt2", _
                                                "TSO1", _
                                                "TSO2", _
                                                "RemoveAt1", _
                                                "RemoveAt2", _
                                                "ModificationNumber", _
                                                "DoneWONo", _
                                                "DetailID", _
                                                "Applicability", _
                                                "ComplianceRequirement", _
                                                "AssemblyDueAsof", _
                                                "AssemblyDueAsof1", _
                                                "AssemblyDueAsof2", _
                                                "Extension", _
                                                "Extension1", _
                                                "Extension2", _
                                                "ExtensionDate", _
                                                "ApprovalRemark", _
                                                "RequiredManHours", _
                                                "Customer", _
                                                "SupersededByADNumber", _
                                                "IssueDate", _
                                                "MaintenanceTypeID", _
                                                "MaintenanceTypeName", _
                                                "DueStatus", _
                                                "TimeSinceNew", _
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
                                                "RemoveAt", _
                                                "MonitorType", _
                                                "DoneONValueForAssembly", _
                                                "MachineID", "ModelID", "DiffCompInstDoneOnValue", "MaintenanceOnExcel", "MaintenanceInformationExcel", _
                                                "MaintenanceInfoExcel", "FrequencyExcel", "SinceNewAllExcel", "ElapsedAllExcel", "EffectiveFromAll", "EffectiveFromAllExcel", _
                                                "DoneAtAllExcel", "ExtensionAllExcel", "DueAsofAllExcel", "AssDueAsofAllExcel", "RemainingTimeAllExcel", _
                                                "MaintenanceInformationForExcel", "EROQtyNosForMaterialMgmtReport", "POQtyNosForMaterialMgmtReport", "PONosForMaterialMgmtReport", _
                                                "POQtyForMaterialMgmtReport", "ERONosForMaterialMgmtReport", "EROQtyForMaterialMgmtReport", _
                                                "UnserviceableStockQty", "ServiceableStockQty", "BinCardTotalQty", "Area", "Zone", "RecordID", "IsMaster", _
                                                "ApplicabilityForExcel", "ReferenceForExcel", "Note", "ThresholdAccordingToTypeIDForExcel", "FrequencyAccordingToTypeIDForExcel", "DueAsOfAssemblyOrCompForExcel", "DueAsOfAirframeForExcel", "RemainingForExcel", _
                                                "MaintenanceActivityType", "Freq1", "ElapsedTime", "RemainingTime", "DueAsof", "EstDate", "InstalledAt", "TSN", "TSO", "RemoveAtDate", "ModelMonitorModCode", "ModelEstimatedManHours", "SourceDoc", "IsRII", "ReqNumber", "LinkedMaintenanceActivityCount", "WONoExcel", "ATAChapter", "Description", "MonitorTypeCode", "DoneOnValue", "MonitorTypeCode"
                                           }

        ElseIf chkDirective.Checked Then
            columnToRemove = { _
                                                "ID", _
                                                "Code", _
                                                "Name", _
                                                "Model", _
                                                "SerialNo", _
                                                "Freq2", _
                                                "Freq3", _
                                                "ElapsedTime1", _
                                                "ElapsedTime2", _
                                                "RemainingTime1", _
                                                "RemainingTime2", _
                                                "DueAsof1", _
                                                "DueAsof2", _
                                                "Note", _
                                                "AssemblySerialNo", _
                                                "EstimatedDate", _
                                                "ComponentInfo", _
                                                "RegNo", _
                                                "AssemblyType", _
                                                "SinceNew", _
                                                "SinceNew1", _
                                                "SinceNew2", _
                                                "DoneAt", _
                                                "DoneAt1", _
                                                "DoneAt2", _
                                                "AssemblyModel", _
                                                "MinimumRemainingValue", _
                                                "AssemblyTypeID", _
                                                "MaintenanceEvent", _
                                                "InstalledAt1", _
                                                "InstalledAt2", _
                                                "TSO1", _
                                                "TSO2", _
                                                "RemoveAt1", _
                                                "RemoveAt2", _
                                                "DoneWONo", _
                                                "DetailID", _
                                                "AssemblyDueAsof", _
                                                "AssemblyDueAsof1", _
                                                "AssemblyDueAsof2", _
                                                "Extension", _
                                                "Extension1", _
                                                "Extension2", _
                                                "ExtensionDate", _
                                                "ApprovalRemark", _
                                                "Customer", _
                                                "MaintenanceTypeID", _
                                                "MaintenanceTypeName", _
                                                "DueStatus", _
                                                "TimeSinceNew", _
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
                                                "RemoveAt", _
                                                "MonitorType", _
                                                "DoneONValueForAssembly", _
                                                "MachineID", "ModelID", "DiffCompInstDoneOnValue", "MaintenanceOnExcel", "MaintenanceInformationExcel", _
                                                "MaintenanceInfoExcel", "FrequencyExcel", "SinceNewAllExcel", "ElapsedAllExcel", "EffectiveFromAll", "EffectiveFromAllExcel", _
                                                "DoneAtAllExcel", "ExtensionAllExcel", "DueAsofAllExcel", "AssDueAsofAllExcel", "RemainingTimeAllExcel", _
                                                "MaintenanceInformationForExcel", "EROQtyNosForMaterialMgmtReport", "POQtyNosForMaterialMgmtReport", "PONosForMaterialMgmtReport", _
                                                "POQtyForMaterialMgmtReport", "ERONosForMaterialMgmtReport", "EROQtyForMaterialMgmtReport", _
                                                "UnserviceableStockQty", "ServiceableStockQty", "BinCardTotalQty", "Area", "Zone", "RecordID", "IsMaster", _
                                                "ApplicabilityForExcel", "ReferenceForExcel", "Note", "ThresholdAccordingToTypeIDForExcel", "FrequencyAccordingToTypeIDForExcel", "DueAsOfAssemblyOrCompForExcel", "DueAsOfAirframeForExcel", "RemainingForExcel", _
                                                "MaintenanceActivityType", "ModelMonitorModCode", "Freq1", "ElapsedTime", "RemainingTime", "DueAsof", "EstDate", "InstalledAt", "TSN", "TSO", "RemoveAtDate", "ModelEstimatedManHours", "SourceDoc", "IsRII", "ReqNumber", "LinkedMaintenanceActivityCount", "WONoExcel", "ATAChapter", "Description", "MonitorTypeCode", "DoneOnValue", "MonitorTypeCode"
                                           }
        End If
        For i As Integer = 0 To columnToRemove.Length - 1
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains(columnToRemove(i)) Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns.Remove(columnToRemove(i))
            End If
        Next
        Dim columnscnt As Integer = ds.Tables("ExcelReportMaintenanceDetailList").Columns.Count
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("Remark").SetOrdinal(columnscnt - 1)



        ds.Tables("ExcelReportMaintenanceDetailList").Columns("ATACode").SetOrdinal(0)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("PartNo").SetOrdinal(1)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("PartDesc").SetOrdinal(2)


        ds.Tables("ExcelReportMaintenanceDetailList").Columns("CompSerialNo").SetOrdinal(3)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("Position").SetOrdinal(4)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("Manufacturer").SetOrdinal(5)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("InstalledAtDate").SetOrdinal(6)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("InstallationWONo").SetOrdinal(7)

        ds.Tables("ExcelReportMaintenanceDetailList").Columns("InstallationRemark").SetOrdinal(8)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("InstallationDoneBy").SetOrdinal(9)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("InstPlace").SetOrdinal(10)



        ds.Tables("ExcelReportMaintenanceDetailList").Columns("TSNHours").SetOrdinal(11)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("SinceNewDate").SetOrdinal(12)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("SinceNewLandings").SetOrdinal(13)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("CSNCycles").SetOrdinal(14)

        ds.Tables("ExcelReportMaintenanceDetailList").Columns("InstCompHours").SetOrdinal(15)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("InstCompStartDate").SetOrdinal(16)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("InstCompLandings").SetOrdinal(17)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("InstCompCycles").SetOrdinal(18)


        ds.Tables("ExcelReportMaintenanceDetailList").Columns("AssemblyInstHours").SetOrdinal(19)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("AssemblyInstStartDate").SetOrdinal(20)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("AssemblyInstLandings").SetOrdinal(21)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("AssemblyInstCycles").SetOrdinal(22)

        ds.Tables("ExcelReportMaintenanceDetailList").Columns("PartMonitorCode").SetOrdinal(23) '''

        ds.Tables("ExcelReportMaintenanceDetailList").Columns("Reference").SetOrdinal(24)
        If chkService.Checked = True Or chkInspection.Checked Then


            ''Services /Inspection
           
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("DescriptionForExcel").SetOrdinal(25)
            ' ds.Tables("ExcelReportMaintenanceDetailList").Columns("Description").SetOrdinal(26)

            ds.Tables("ExcelReportMaintenanceDetailList").Columns("MonitorTypeWithCode").SetOrdinal(26)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("NoteForExcel").SetOrdinal(27)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("HoursFreq").SetOrdinal(28)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("CyclesFreq").SetOrdinal(29)


            ds.Tables("ExcelReportMaintenanceDetailList").Columns("DaysMnthsYrsName").SetOrdinal(30)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("DaysMnthsYrsValue").SetOrdinal(31)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("LandingsFreq").SetOrdinal(32)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("DoneOnDate").SetOrdinal(33)

            ds.Tables("ExcelReportMaintenanceDetailList").Columns("HoursDoneOnValue").SetOrdinal(34)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("CyclesDoneOnValue").SetOrdinal(35)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("DaysMnthsYrsDoneOnValue").SetOrdinal(36)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("LandingsDoneOnValue").SetOrdinal(37)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("IsApplicable").SetOrdinal(38)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("IsLater").SetOrdinal(39)

        ElseIf chkDirective.Checked Then
            ''Directive
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("PartMonitorCode").SetOrdinal(25)

            ds.Tables("ExcelReportMaintenanceDetailList").Columns("MonitorTypeWithCode").SetOrdinal(26)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("ModificationNumber").SetOrdinal(27)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("DescriptionForExcel").SetOrdinal(28)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("Reference").SetOrdinal(29)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("IssueDate").SetOrdinal(30)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("SupersededByADNumber").SetOrdinal(31)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("Applicability").SetOrdinal(32)

            ds.Tables("ExcelReportMaintenanceDetailList").Columns("ComplianceRequirement").SetOrdinal(33)

            ds.Tables("ExcelReportMaintenanceDetailList").Columns("NoteForExcel").SetOrdinal(34)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("RequiredManHours").SetOrdinal(35)

            ds.Tables("ExcelReportMaintenanceDetailList").Columns("HoursFreq").SetOrdinal(36)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("CyclesFreq").SetOrdinal(37)


            ds.Tables("ExcelReportMaintenanceDetailList").Columns("DaysMnthsYrsName").SetOrdinal(38)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("DaysMnthsYrsValue").SetOrdinal(39)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("LandingsFreq").SetOrdinal(40)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("DoneOnDate").SetOrdinal(41)

            ds.Tables("ExcelReportMaintenanceDetailList").Columns("HoursDoneOnValue").SetOrdinal(42)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("CyclesDoneOnValue").SetOrdinal(43)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("DaysMnthsYrsDoneOnValue").SetOrdinal(44)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("LandingsDoneOnValue").SetOrdinal(45)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("IsApplicable").SetOrdinal(46)
            '  ds.Tables("ExcelReportMaintenanceDetailList").Columns("IsLater").SetOrdinal(47)

        End If

        'ds.Tables("ExcelReportMaintenanceDetailList").Columns("Remark").SetOrdinal(41)



        Dim DueLabel As String = "DueAsof"
        For i As Integer = 0 To ds.Tables("ExcelReportMaintenanceDetailList").Columns.Count - 1
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "ModificationNumber" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Directive No"
            End If

            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "MonitorTypeCode" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Type"
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "StatusTypeName" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Status"
            End If

        Next
        Dim columnToRemoveCriteria As String() = { _
                                                 "ReportDate", _
                                                 "ID", _
                                                 "CompanyName", _
                                                 "Address", _
                                                 "Tel1", _
                                                 "Tel2", _
                                                 "Fax", _
                                                 "Email", _
                                                 "WebSite", _
                                                 "ReportName", _
                                                 "SearchStr5", _
                                                 "SearchStr6", _
                                                 "SearchStr7", _
                                                 "SearchStr8", _
                                                 "SearchStr9", _
                                                 "ProductVersion", _
                                                 "SINote", _
                                                 "CurrencyName", _
                                                 "CurrencySymbol", _
                                                 "SearchStr10", _
                                                 "SearchStr11", _
                                                 "SearchStr12", _
                                                 "SearchStr13", _
                                                 "SearchStr14", "ShortName", "SearchStr4", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25","SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40","SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47","SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100" _
                                                }

        For i As Integer = 0 To columnToRemoveCriteria.Length - 1
            If ds.Tables("ExcelReport").Columns.Contains(columnToRemoveCriteria(i)) Then
                ds.Tables("ExcelReport").Columns.Remove(columnToRemoveCriteria(i))
            End If
        Next

        For i As Integer = 0 To ds.Tables("ExcelReport").Columns.Count - 1
            If ds.Tables("ExcelReport").Columns(i).ColumnName = "SearchStr1" Then
                ds.Tables("ExcelReport").Columns(i).ColumnName = "AsOnDate"
            End If
            If ds.Tables("ExcelReport").Columns(i).ColumnName = "SearchStr2" Then
                ds.Tables("ExcelReport").Columns(i).ColumnName = "Reg No."
            End If
            If ds.Tables("ExcelReport").Columns(i).ColumnName = "SearchStr3" Then
                ds.Tables("ExcelReport").Columns(i).ColumnName = "Assembly"
            End If
        Next
        'Dim dataview As DataView = ds.Tables("ExcelReportMaintenanceDetailList").DefaultView
        'dataview.Sort = "ATAChapter"


        Dim dsNew As New DataSet
        dsNew.Clear()

        ds.Tables("ExcelReport").TableName = "Searching Criteria"

        'dsNew.Merge(ds.Tables("ExcelReport"))
        dsNew.Merge(ds.Tables("ExcelReportMaintenanceDetailList"))

        'dsNew.Tables("ExcelReport").TableName = "Searching Criteria"
        dsNew.Tables("ExcelReportMaintenanceDetailList").TableName = ReportLabel
		Session("ExcelFileName") = ReportLabel
		PeriodColumnsForExportToExcel.AddRange(New String() {"Frequency", "ElapsedTime", "RemainingTime", "DueAsof", "Last Carried Out"})
        Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
        Session("dsNew") = dsNew
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        'Added by Prashant on 19-Jan-2021
        MarkLog(Util.Action.Print, "ComponantCurrentStatus", "Export To Excel " + EventLogDetail + " " + ReportLabel, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub SetReport(Optional ByVal ByExcel As Boolean = False)

        SetValues()

        If chkShowComponent.Checked Then
            SetCompActivityExcel()
        ElseIf chkShowAssembly.Checked Then
            SetAssemlbyActivityExcel()
        End If


        
        ResetValues()
    End Sub
    
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    '
                Case MsgBoxResult.No
                    '
                Case MsgBoxResult.Ok
                    Session("Sender") = ""
                    ''Response.Redirect("wfCompliedMaintenanceActivity.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
            ''Response.Redirect("wfCompliedMaintenanceActivity.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        End If
    End Sub

#End Region

#Region " Data Binding "
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbAircraft" Then
            If cmbAircraft.SelectedIndex = 0 Then
                custValidator.ErrorMessage = "Please select the Aircraft"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        ElseIf custValidator.ControlToValidate = "txtFromDate" Then
            If txtFromDate.Text = "" Then
                custValidator.ErrorMessage = "Please Enter Valid Date."
                e.IsValid = False
            Else
                e.IsValid = True
            End If
      
        End If
    End Sub
    Public Sub SetTypeCombo()
        If mServiceTypeList Is Nothing Then
            mServiceTypeList = PartMonitorServiceTypeList.GetPartMonitorServiceTypeListForNoFrequency(, , True)
        End If
        ListServiceType.DataSource = mServiceTypeList
        Session("mServiceTypeList") = mServiceTypeList

        If mInspectionTypesList Is Nothing Then
            mInspectionTypesList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList()
        End If
        ListInspectionType.DataSource = mInspectionTypesList
        Session("mInspectionTypesList") = mInspectionTypesList

        If mModificationTypeList Is Nothing Then
            mModificationTypeList = ModelMonitorModTypeList.GetModelMonitorModTypeListForNoFrequency(, , True)
        End If

        ListDirectiveType.DataSource = mModificationTypeList
        Session("mModificationTypeList") = mModificationTypeList
        DataBind()

        FillMonitorTypeList()

        ''''For i As Integer = 0 To chkListServiceType.Items.Count - 1
        ''''    chkListServiceType.Items(i).Selected = True
        ''''Next

        ''''For i As Integer = 0 To chkListInspectionType.Items.Count - 1
        ''''    chkListInspectionType.Items(i).Enabled = False
        ''''Next

        ''''For i As Integer = 0 To chkListDirectiveType.Items.Count - 1
        ''''    chkListDirectiveType.Items(i).Enabled = False
        ''''Next
    End Sub

    Public Sub SetCombo()
        ''GetSession()
        mMachineNameValueList = MachineNameValueList.GetMachineList(Now.ToShortDateString, , , , , , , True, "(SELECT)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        cmbAircraft.DataBind()
        Session("mMachineNameValueList") = mMachineNameValueList

        mServiceTypeList = PartMonitorServiceTypeList.GetPartMonitorServiceTypeListForNoFrequency(, , True)     'ServiceType
        mInspectionTypesList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList()          'Inspection Type 
        mModificationTypeList = ModelMonitorModTypeList.GetModelMonitorModTypeListForNoFrequency(, , True)     'Modification Type
    End Sub
    Private Sub FillMonitorTypeList()
        chkService.Checked = True
        For i As Integer = 0 To ListServiceType.Items.Count - 1
            ListServiceType.Items(i).Selected = True
        Next
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("Sender") = "" Then
            Session("MiddleFrame") = "wfExportMaintActivitiesToExcel.aspx?"
            ResetValues()
            SetCombo()
            cmbAssembly.Enabled = False
            txtFromDate.Text = Now.Date.ToString(AppSettings("DateFormat"))

            setFocus(cmbAircraft)
            SetTypeCombo()
            SetSession()
        End If

        '  MessageBoxResult()
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        If IsValid = True Then
            'If IsValid = True And CustomValidate() = True Then
            Display()
            SetValues()
            upnlCriteria.Update()
        Else
            upnlValidationSummary.Update()
        End If

    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mMachineList = Nothing
        mAssemblyList = Nothing
        mServiceTypeList = Nothing
        mInspectionTypesList = Nothing
        mModificationTypeList = Nothing
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        If cmbAircraft.SelectedIndex = 0 Then
            cmbAssembly.Enabled = False
            cmbAssembly.SelectedIndex = 0
        Else
            cmbAssembly.Enabled = True

            ' Dim mAssemblylist As AssemblyList
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, txtFromDate.Text.ToString, "(All)", True)
            Session("mAssemblyList") = mAssemblylist
            cmbAssembly.DataSource = mAssemblylist
            cmbAssembly.DataBind()
        End If

        If cmbAircraft.Enabled = True Then
            setFocus(cmbAircraft)
        End If
        upnlAssembly.Update()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

    Private Sub btnByExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnByExcel.Click
        If IsValid = True Then
            SetReport(True)
        Else
            upnlValidationSummary.Update()
        End If
    End Sub

#End Region
End Class