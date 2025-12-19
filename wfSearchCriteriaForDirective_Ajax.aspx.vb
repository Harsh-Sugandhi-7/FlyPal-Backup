Imports System.Collections.Generic
Imports System.Text

Public Class wfSearchCriteriaForDirective_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Dim ReportMaintenanceDetails As New ReportMaintenanceDetailList
    Dim mReportMaintenanceDetail As New ReportMaintenanceDetail
    Dim mAssemblylist As AssemblyList
    Dim mModificationTypeList As ModelMonitorModTypeList
    Dim ReportStatusList As New rptStatusList
    Dim mMachineList As MachineList
    Dim ReportLabel As String
    Dim Aircraft As String
    Dim Assembly1 As String
    Dim ReportType As String
    Dim AOnDate As String
    Dim Report As Integer = 1
    Dim ShowCofA As Boolean = False
    Dim AsonDate As String = ""
    Dim AsonDate1 As String = "" 'Added by Shital on 10-Aug-2020
    Dim Periodcount As Integer
    Dim Count, Count1 As Integer
    Dim AssemblyName As String
    Dim MachineName As String
    Dim AssemblyID As Guid
    Private ATAChapter As String
    Private RegNo As String
    Private AssemblyType As String
    Private Model As String
    Private AssemblySerialNo As String
    Private PartNo As String
    Private CompSerialNo As String
    Private Position As String
    Private MonitorTypeCode As String
    Private MonitorType As String
    Private Note As String
    Private Description, DoneRemark As String
    Private EstimatedDate As String
    Private Freq1 As String
    Private ElapsedTime As String
    Private RemainingTime As String
    Private DueAsof As String
    Private AssemblyModel As String
    Private Number As String
    Private Reference As String
    Private DoneOnValue As String
    Private DoneOnDate As String
    Private DoneWONo As String
    Private Remark As String
    Private DirectiveName As String
    Private Directive As String
    Private mModTypeList As ModTypeList
    Private Applicability As String
    Private ComplianceRequirement As String
    Private ModelMonitorModCode As String
    Private AssemblyTypeID As Integer
    Private Code As String
    Private IssueDate As SmartDate = New SmartDate(True)
    Private IsApplicable As Boolean
    Private SerialNoPostion As String
    Dim searchstr7 As String = "" 'Added By Utkarsh On 07-Apr-2011
    Private StatusType As String = "" 'Added By Vikrant on 22-Jun-2012 For ALL22062012
    Dim mMachineNameValueList As MachineNameValueList
    Dim mSearchCriteriaForEventLog As String = ""
    Dim EventLogID As Guid
    Private IsExcel As Boolean = False
    Dim ReportData As ReportData
    Dim ModShortName As String = ""
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    Dim ModTypeIds As New StringBuilder
    Dim ModTypeNames As New StringBuilder
    Dim ModelMonitorModTypeIds As New StringBuilder
    Dim ModelMonitorModTypeNames As New StringBuilder
    Private DirectiveType As String
    Private MonitorTypeID As Integer
    Private MethodOfCompliance As String
    Dim mIssuingAuthorityTypeList As IssuingAuthorityTypeList
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mAssemblylist = CType(Session("mAssemblylist"), AssemblyList)
        AOnDate = Session("AOnDate")
        Report = Session("Report")
        ShowCofA = Session("ShowCofA")
        mModTypeList = Session("mModTypeList")
        mMachineNameValueList = Session("mMachineNameValueList")
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
    End Sub
    Private Sub SetSession()
        Session("mAssemblylist") = mAssemblylist
        Session("AOnDate") = AOnDate
        Session("Report") = Report
        Session("ShowCofA") = ShowCofA
        Session("mModTypeList") = mModTypeList
        Session("mMachineNameValueList") = mMachineNameValueList
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfSearchCriteriaForDirective_Ajax.aspx?" Then
            Session.Remove("mAssemblylist")
            Session.Remove("AOnDate")
            Session.Remove("Report")
            Session.Remove("mMachineNameValueList")
        End If
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAssemblylist")
        Session.Remove("AOnDate")
        Session.Remove("Report")
        Session.Remove("mMachineNameValueList")
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub Display()
        lblAircraft1.Visible = True
        lblDateRange.Visible = True
        lblAssembly1.Visible = True
        lblType1.Visible = True
        lblDirType.Visible = True
        upnlSearchCriteria.Update()
    End Sub

    Private Sub SetValues()
        If cmbAircraft.SelectedItem.Text = "(Select)" Then
            Aircraft = ""
        Else
            AssemblyType = mAssemblylist(cmbAssembly.SelectedIndex).AssemblyType
            AssemblyName = cmbAssembly.SelectedValue.ToString
            Assembly1 = cmbAssembly.SelectedItem.Text
            lblAssembly1.Text = "Assembly Name : " & Assembly1
            MachineName = cmbAircraft.SelectedValue.ToString
            Aircraft = cmbAircraft.SelectedItem.Text
            lblAircraft1.Text = "Aircraft Name : " & Aircraft
        End If

        If Not IsDate(txtFromDate.Text) Then      'AsOnDate
            AsonDate = ""
        Else
            AsonDate = txtFromDate.Text
            lblDateRange.Text = "AsonDate : " & New SmartDate(txtFromDate.Text).FormattedText
        End If

        'If cmbType.SelectedItem.Text = "<SELECT>" Then 'Directive
        '    Directive = ""
        '    lblType1.Text = ""
        '    lblType1.Visible = False
        'Else
        '    DirectiveName = mModTypeList(cmbType.SelectedIndex).Name
        '    Directive = cmbType.SelectedItem.Text
        '    lblType1.Text = "Directive Name : " & Directive
        'End If
        For K As Integer = 0 To ListDirectiveType.Items.Count - 1
            If ListDirectiveType.Items.Item(K).Selected Then
                ModTypeNames.Append(ListDirectiveType.Items.Item(K).Text + ",")
            End If
        Next
        For K As Integer = 0 To ListDirectiveSubType.Items.Count - 1
            If ListDirectiveSubType.Items.Item(K).Selected Then
                ModelMonitorModTypeNames.Append(ListDirectiveSubType.Items.Item(K).Text + ",")
            End If
        Next
        DirectiveName = ModTypeNames.ToString.TrimEnd(",") 'To remove last ,
        Directive = ModTypeNames.ToString
        lblType1.Text = "Directive Name : " & Directive
        lblDirType.Text = "Directive Type : " & ModelMonitorModTypeNames.ToString.TrimEnd(",")
        'mSearchCriteriaForEventLog = lblDateRange.Text + "," + lblAircraft1.Text + "," + lblAssembly1.Text + "," + lblType1.Text + ", " + "Type : " + IIf(cmbType.SelectedIndex > 0, cmbType.SelectedItem.ToString, "All") + ", " + "Format : " + cmbFormat.SelectedItem.ToString + ", " + "Sort By : " + cmbSortBy.SelectedItem.ToString + ", " + "Order By : " + IIf(optAscending.Checked, "Ascending", "Descending")
        mSearchCriteriaForEventLog = lblDateRange.Text + "," + lblAircraft1.Text + "," + lblAssembly1.Text + "," + lblType1.Text + ", " + lblDirType.Text + ", " + "Format : " + cmbFormat.SelectedItem.ToString + ", " + "Sort By : " + cmbSortBy.SelectedItem.ToString + ", " + "Order By : " + IIf(optAscending.Checked, "Ascending", "Descending")
    End Sub
    Private Sub ResetValues()
        AssemblyName = "{00000000-0000-0000-0000-000000000000}"
        ShowCofA = False 'True
        Session("ShowCofA") = ShowCofA
        AssemblyType = ""
        MachineName = "{00000000-0000-0000-0000-000000000000}"
        txtFromDate.Text = AsonDate 'CHK
        If AsonDate <> "" Then
            txtFromDate.Text = AsonDate 'CHK
        End If
    End Sub
    Public Function ReportDetail() As ReportMaintenanceDetailList
        Dim ObjMachine As MachineInfo
        Dim ObjAssemblyStatus As AssemblyStatusInfo
        Dim ObjCompStatus As CompStatusInfo
        Dim ObjAssemblyMonitorModStatus As AssemblyMonitorModStatusInfo
        Dim ObjAssemblyMonitorModStatusPeriod As AssemblyMonitorModStatusPeriodInfo
        Dim ObjCompMonitorModStatus As CompMonitorModStatusInfo
        Dim ObjCompMonitorModStatusPeriod As CompMonitorModStatusPeriodInfo

        'mMachineList = MachineList.GetMachineListMonitoringStatus(New SmartDate(AsonDate).Text, MachineName, , , , , , , , , , , True, , AssemblyName, SkipIsForInventoryAircarft:=True)
        'mMachineList = MachineList.GetMachineListMonitoringStatusForHardTimeAndDirective(AsonDate, MachineName, , , , , , , , , , chkIsCompDirectivesRequired.Checked, chkIsAssemblyDirectivesRequired.Checked, , AssemblyName, , , , , , , , , , , ShowCofA, , , , True, , , , , , , False, , False, , True, , , , , , True, 6, , , True, SkipIsForInventoryAircarft:=True, ModTypeIDs:=cmbType.SelectedValue)
        For K As Integer = 0 To ListDirectiveType.Items.Count - 1
            If ListDirectiveType.Items.Item(K).Selected Then
                ModTypeIds.Append(ListDirectiveType.Items.Item(K).Value + ",")
            End If
        Next

        For P As Integer = 0 To ListDirectiveSubType.Items.Count - 1
            If ListDirectiveSubType.Items.Item(P).Selected Then
                ModelMonitorModTypeIds.Append(ListDirectiveSubType.Items.Item(P).Value + ",")
            End If
        Next
        mMachineList = MachineList.GetMachineListMonitoringStatus(AsonDate, MachineName, , , , , , , , , ,
                                                                  chkIsCompDirectivesRequired.Checked, True, ,
                                                                  AssemblyName, ShowInCofA:=ShowCofA, MonitoringModRequired:=True,
                                                                  IsAssemblyRemoved:=False, IsCompRemoved:=False, IsComplied:=True,
                                                                  IsAverageRequired:=True, AverageMonths:=6, CompMonitoringModRequired:=True,
                                                                  SkipIsForInventoryAircarft:=True,
                                                                  ModTypeIDs:=ModTypeIds.ToString.TrimEnd(","),
                                                                  MonitorModTypeIDs:=ModelMonitorModTypeIds.ToString.TrimEnd(","),
                                                                  IssuingAuthorityID:=IIf(cmbIssuingAuthority.SelectedIndex = 0, 0, CInt(cmbIssuingAuthority.SelectedValue))) 'IsAverageRequired:=mIsAverageRequired, ByPerDayLimit:=mByPerDayLimit, PerdayLimits:=mPerDayLimits, SkipIsForInventoryAircarft:=True)
        Dim LHLabel2 As String = ""
        Dim LHData2 As String = ""
        Dim RHLabel1 As String = ""
        Dim RHData1 As String = ""
        Dim RHLabel2 As String = ""
        Dim RHData2 As String = ""
        Dim RHData3 As String = ""
        Dim SearchStr8 As String = ""

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
                ReportStatusList.Add(New rptStatus(AssemblyID.ToString, ObjAssemblyStatus.AssemblyTypeID, , "Reg No.", ObjMachine.RegNo, ObjAssemblyStatus.AssemblyType + " " + "Model", ObjAssemblyStatus.Model,
                   "Serial No.", SerialNoPostion, "Due As of " & ObjAssemblyStatus.AssemblyType, , , , , , , , , , , , , LHLabel2, LHData2, RHLabel1, RHData1, RHLabel2, RHData2, RHData3, RHData10:=SearchStr8))
            Next
        Next

        mModificationTypeList = ModelMonitorModTypeList.GetModelMonitorModTypeList()

        For i As Integer = 0 To mModificationTypeList.Count - 1
            'Added by Prashant 13-Jun-2018 Suhan13062018
            If ModShortName = "" Then
                ModShortName = IIf(Not mModificationTypeList(i, "").CodeType Is Nothing, mModificationTypeList(i, "").CodeType, "")
            Else
                ModShortName = ModShortName + IIf(Not mModificationTypeList(i, "").CodeType Is Nothing, ", " + mModificationTypeList(i, "").CodeType, "")
            End If
        Next
        'End Added by Prashant 13-Jun-2018 Suhan13062018

        For Each ObjMachine In mMachineList
            For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                If chkIsAssemblyDirectivesRequired.Checked Then

                    For Each ObjAssemblyMonitorModStatus In ObjAssemblyStatus.AssemblyMonitorModStatusList

                        'Added by Saylee on 19-Dec-2023
                        If ObjAssemblyMonitorModStatus.IsApplicable = False And ObjAssemblyMonitorModStatus.ModelActivityCount >= 1 Then
                            GoTo nextRec
                        End If
                        '**********************************

                        ATAChapter = ObjAssemblyMonitorModStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorModStatus.ATANomenclature
                        Description = ObjAssemblyMonitorModStatus.Description
                        Position = ObjAssemblyStatus.Position
                        MonitorTypeCode = ObjAssemblyMonitorModStatus.Code
                        MonitorTypeID = ObjAssemblyMonitorModStatus.MonitorTypeID
                        If cmbFormat.SelectedValue = 0 Or cmbFormat.SelectedValue = 2 Then 'Format1 report  'Format 3 Added By Prashant on 1-Feb-2021 APFT01022021
                            MonitorType = ObjAssemblyMonitorModStatus.MonitorType '.Type
                            'Added By VIkrant on 22-Jun-2012 For ALL22062012
                            If (ObjAssemblyMonitorModStatus.IsApplicable = True) Then
                                If (ObjAssemblyMonitorModStatus.IsCompleted = True) And (ObjAssemblyMonitorModStatus.MonitorTypeID = 3 Or ObjAssemblyMonitorModStatus.MonitorTypeID = 1) Then
                                    StatusType = "CLOSED"
                                Else
                                    StatusType = "OPEN"
                                End If
                            Else
                                If AppSettings("ClientCode") = "GEP" Or AppSettings("ClientCode") = "SHR" Then
                                    StatusType = "CLOSED"
                                Else
                                    If (ObjAssemblyMonitorModStatus.ModelMonitorModTypeID = 28 Or ObjAssemblyMonitorModStatus.ModelMonitorModTypeID = 29) Then
                                        StatusType = "OPEN"
                                    ElseIf (ObjAssemblyMonitorModStatus.ModelMonitorModTypeID = 25 Or ObjAssemblyMonitorModStatus.ModelMonitorModTypeID = 26 Or ObjAssemblyMonitorModStatus.ModelMonitorModTypeID = 27) Then
                                        StatusType = "CLOSED"
                                    ElseIf (ObjAssemblyMonitorModStatus.ModelMonitorModTypeID = 21) Then 'ModelMonitorModTypeID = 21 added by Saylee on 4-Feb-2015 for Superseded
                                        StatusType = "S"
                                    ElseIf (AppSettings("ClientCode") = "APFT" Or
                                            AppSettings("ClientCode") = "AAP") And
                                        (ObjAssemblyMonitorModStatus.ModelMonitorModTypeID = 35 Or
                                         ObjAssemblyMonitorModStatus.ModelMonitorModTypeID = 36 Or
                                         ObjAssemblyMonitorModStatus.ModelMonitorModTypeID = 37 Or
                                         ObjAssemblyMonitorModStatus.ModelMonitorModTypeID = 38 Or
                                         ObjAssemblyMonitorModStatus.ModelMonitorModTypeID = 39) Then 'Added by Prashant on 28-Sep-2018 for Superseded
                                        StatusType = "Superseded"
                                        If ObjAssemblyMonitorModStatus.MonitorType = "No Frequency" Then
                                            MonitorType = "----"
                                        End If
                                    ElseIf (AppSettings("ClientCode") = "APFT" Or
                                            AppSettings("ClientCode") = "AAP") And
                                           (ObjAssemblyMonitorModStatus.ModelMonitorModTypeID = 20) Then 'Added by Prashant on 28-Sep-2018 for Superseded
                                        StatusType = "Terminated"
                                        If ObjAssemblyMonitorModStatus.MonitorType = "No Frequency" Then
                                            MonitorType = "----"
                                        End If
                                    ElseIf (AppSettings("ClientCode") = "APFT" Or
                                            AppSettings("ClientCode") = "AAP") And
                                           (ObjAssemblyMonitorModStatus.ModelMonitorModTypeID = 32) Then 'Added by Prashant on 28-Sep-2018 for Superseded
                                        StatusType = "Cancelled"
                                        If ObjAssemblyMonitorModStatus.MonitorType = "No Frequency" Then
                                            MonitorType = "----"
                                        End If
                                    ElseIf AppSettings("ClientCode") = "IND" Then
                                        If ObjAssemblyMonitorModStatus.MonitorType = "No Frequency" Then
                                            If (ObjAssemblyMonitorModStatus.ModelMonitorModTypeID = 40 Or ObjAssemblyMonitorModStatus.ModelMonitorModTypeID = 41 Or ObjAssemblyMonitorModStatus.ModelMonitorModTypeID = 42 Or ObjAssemblyMonitorModStatus.ModelMonitorModTypeID = 43) Then
                                                StatusType = "Cancelled"
                                            Else
                                                StatusType = "N/A"
                                            End If
                                        Else
                                            StatusType = "CLOSED"
                                        End If

                                    Else
                                        StatusType = "N/A"
                                    End If
                                End If
                            End If
                            'End
                        ElseIf cmbFormat.SelectedValue = 1 Then 'Format2 report
                            If (ObjAssemblyMonitorModStatus.IsApplicable = True) Then
                                If (ObjAssemblyMonitorModStatus.IsCompleted = True) And (ObjAssemblyMonitorModStatus.MonitorTypeID = 3 Or ObjAssemblyMonitorModStatus.MonitorTypeID = 1) Then
                                    MonitorType = "CLOSED"
                                Else
                                    MonitorType = "OPEN"
                                End If
                            Else
                                If AppSettings("ClientCode") = "GEP" Or AppSettings("ClientCode") = "SHR" Then
                                    MonitorType = "CLOSED"
                                Else
                                    If AppSettings("ClientCode") = "IND" Then
                                        If ObjAssemblyMonitorModStatus.MonitorType = "No Frequency" Then
                                            If (ObjAssemblyMonitorModStatus.ModelMonitorModTypeID = 40 Or ObjAssemblyMonitorModStatus.ModelMonitorModTypeID = 41 Or ObjAssemblyMonitorModStatus.ModelMonitorModTypeID = 42 Or ObjAssemblyMonitorModStatus.ModelMonitorModTypeID = 43) Then
                                                StatusType = "Cancelled"
                                            Else
                                                StatusType = "N/A"
                                            End If
                                        Else
                                            StatusType = "CLOSED"
                                        End If
                                    Else
                                        If (ObjAssemblyMonitorModStatus.ModelMonitorModTypeID = 28 Or ObjAssemblyMonitorModStatus.ModelMonitorModTypeID = 29) Then
                                            MonitorType = "OPEN"
                                        ElseIf (ObjAssemblyMonitorModStatus.ModelMonitorModTypeID = 25 Or ObjAssemblyMonitorModStatus.ModelMonitorModTypeID = 26 Or ObjAssemblyMonitorModStatus.ModelMonitorModTypeID = 27) Then
                                            MonitorType = "CLOSED"
                                        Else
                                            MonitorType = "N/A"
                                        End If
                                    End If
                                End If
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
                        DoneOnDate = ""
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
                                    'Comment removed by Saylee on 20-Apr-2010 to show value for PeriodID=2 also(Pramod's Requirement)
                                    If (AppSettings("ClientCode") IsNot Nothing) AndAlso ((AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "MID")) Then
                                        If cmbFormat.SelectedValue <> 1 Then DoneOnValue = DoneOnValue & ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted & IIf(IsExcel, Chr(10), vbCrLf)
                                    Else
                                        If ObjAssemblyMonitorModStatus.DoneOn <> "" And cmbFormat.SelectedValue <> 1 Then DoneOnValue = DoneOnValue & ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted & IIf(IsExcel, Chr(10), vbCrLf)
                                    End If


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
                                    If (AppSettings("ClientCode") IsNot Nothing) AndAlso ((AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "MID")) Then
                                        If cmbFormat.SelectedValue <> 1 Then DoneOnValue = DoneOnValue + ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted & IIf(IsExcel, Chr(10), vbCrLf)
                                    Else
                                        If ObjAssemblyMonitorModStatus.DoneOn <> "" And cmbFormat.SelectedValue <> 1 Then DoneOnValue = DoneOnValue + ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted & IIf(IsExcel, Chr(10), vbCrLf)
                                    End If

                                    'DoneOnValue = DoneOnValue + ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted & IIf(IsExcel, Chr(10), vbCrLf)
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
                        AsonDate1 = ObjAssemblyMonitorModStatus.AsOnDateFormatted 'Added by Shital on 10-Aug-2020
                        DoneWONo = ObjAssemblyMonitorModStatus.DoneWONo

                        If DoneWONo = "" Then
                            DoneWONo = "----"
                        End If
                        Remark = ObjAssemblyMonitorModStatus.DoneRemark
                        If Remark = "" Then
                            Remark = "----"
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

                        If IsExcel Then
                            Dim ATACode As Integer = ObjAssemblyMonitorModStatus.ATACode
                            If ATACode.ToString.Length < 3 Then
                                ATAChapter = ATACode.ToString.PadLeft(3, "0"c) + " " + "-" + " " + ObjAssemblyMonitorModStatus.ATANomenclature
                            End If

                        End If

                        MethodOfCompliance = ObjAssemblyMonitorModStatus.MethodOfCompliance
                        If MethodOfCompliance = "" Then
                            MethodOfCompliance = "----"
                        End If

                        mReportMaintenanceDetail = New ReportMaintenanceDetail(AssemblyID, , ObjAssemblyMonitorModStatus.Code, , AssemblySerialNo, ATAChapter, , , Position, MonitorType, MonitorTypeCode, Note, Remark, Description,
                                                           , EstimatedDate, , , Freq1, Freq1, Freq1, ElapsedTime, ElapsedTime, ElapsedTime, RemainingTime, RemainingTime, RemainingTime,
                                                          DueAsof, DueAsof, DueAsof, AssemblyModel, , , , , , , , AssemblyTypeID, , , , , , , , , , , , , , , Number, Reference, DoneOnValue, DoneOnDate,
                                                          DoneWONo, Applicability, ComplianceRequirement, , , , , , , , , , AsonDate1, Code, , , , IssueDate.Date.ToString("g"), IsApplicable, , MonitorTypeID, , , , , , , , StatusType, MethodOfCompliance:=MethodOfCompliance)

                        mReportMaintenanceDetail.ModelMonitorModCode = ModelMonitorModCode

                        If cnbAdType.SelectedValue = 0 Then  '' All
                            ReportMaintenanceDetails.Add(mReportMaintenanceDetail)
                        ElseIf cnbAdType.SelectedValue = 1 Then '' Opened
                            If (ObjAssemblyMonitorModStatus.IsApplicable = True) Then
                                If (ObjAssemblyMonitorModStatus.IsCompleted = True) And (ObjAssemblyMonitorModStatus.MonitorTypeID = 3 Or ObjAssemblyMonitorModStatus.MonitorTypeID = 1) Then
                                    'Do Nothing
                                Else
                                    ReportMaintenanceDetails.Add(mReportMaintenanceDetail)
                                End If
                            Else
                                'Do Nothing
                            End If
                        ElseIf cnbAdType.SelectedValue = 2 Then  '' closed
                            If (ObjAssemblyMonitorModStatus.IsApplicable = True) Then
                                If (ObjAssemblyMonitorModStatus.IsCompleted = True) And (ObjAssemblyMonitorModStatus.MonitorTypeID = 3 Or ObjAssemblyMonitorModStatus.MonitorTypeID = 1) Then
                                    ReportMaintenanceDetails.Add(mReportMaintenanceDetail)
                                Else
                                    'Do Nothing
                                End If
                            Else
                                If AppSettings("ClientCode") = "APFT" Or
                                   AppSettings("ClientCode") = "AAP" Then
                                    If StatusType = "CLOSED" Then
                                        ReportMaintenanceDetails.Add(mReportMaintenanceDetail)
                                    End If
                                Else
                                    ReportMaintenanceDetails.Add(mReportMaintenanceDetail)
                                End If

                            End If
                        End If
nextRec:            Next
                End If
                If chkIsCompDirectivesRequired.Checked Then
                    For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
                        For Each ObjCompMonitorModStatus In ObjCompStatus.CompMonitorModStatusList
                            ATAChapter = ObjCompMonitorModStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorModStatus.ATANomenclature
                            Description = ObjCompMonitorModStatus.Description
                            PartNo = ObjCompStatus.PartName
                            CompSerialNo = ObjCompStatus.CompSerialNo
                            Position = ObjCompStatus.Position
                            MonitorTypeCode = ObjCompMonitorModStatus.Code
                            MonitorTypeID = ObjCompMonitorModStatus.MonitorTypeID
                            If cmbFormat.SelectedValue = 0 Or cmbFormat.SelectedValue = 2 Then 'Format1 report 'Format 3 Added By Prashant on 1-Feb-2021 APFT01022021
                                MonitorType = ObjCompMonitorModStatus.MonitorType '.Type
                                'Added By VIkrant on 22-Jun-2012 For ALL22062012
                                If (ObjCompMonitorModStatus.IsApplicable = True) Then
                                    If (ObjCompMonitorModStatus.IsCompleted = True) And (ObjCompMonitorModStatus.MonitorTypeID = 3 Or ObjCompMonitorModStatus.MonitorTypeID = 1) Then
                                        StatusType = "CLOSED"
                                    Else
                                        StatusType = "OPEN"
                                    End If
                                Else
                                    StatusType = "CLOSED"
                                End If
                                'End
                            ElseIf cmbFormat.SelectedValue = 1 Then 'Format2 report
                                If (ObjCompMonitorModStatus.IsApplicable = True) Then
                                    If (ObjCompMonitorModStatus.IsCompleted = True) And (ObjCompMonitorModStatus.MonitorTypeID = 3 Or ObjCompMonitorModStatus.MonitorTypeID = 1) Then
                                        MonitorType = "CLOSED"
                                    Else
                                        MonitorType = "OPEN"
                                    End If
                                Else
                                    MonitorType = "CLOSED"
                                End If
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
                                If ObjCompMonitorModStatusPeriod.PeriodID = 2 Then
                                    If Freq1 = "" Then

                                        If (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted = True) Or (ObjCompMonitorModStatus.IsApplicable = False) Then
                                            RemainingTime = ""
                                            DueAsof = ""
                                            'Commented & added by Saylee on 1-Nov-2018 , as per BINU Frequency should be visible
                                            'Freq1 = ""
                                            Freq1 = ObjCompMonitorModStatusPeriod.FrequencyValueFormatted
                                            '*************************************
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
                                        DoneOnValue = DoneOnValue + ObjCompMonitorModStatusPeriod.DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf)
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
                                        If ObjCompMonitorModStatusPeriod.DoneOnValue <> "" Then
                                            DoneOnValue = DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.DoneOnValue
                                        Else
                                            DoneOnValue = DoneOnValue & ObjCompMonitorModStatusPeriod.DoneOnValue
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
                            If MonitorType = "" Then ''
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

                            If IsExcel Then
                                Dim ATACode As Integer = ObjCompMonitorModStatus.ATACode
                                If ATACode.ToString.Length < 3 Then
                                    ATAChapter = ATACode.ToString.PadLeft(3, "0"c) + " " + "-" + " " + ObjCompMonitorModStatus.ATANomenclature
                                End If

                            End If

                            MethodOfCompliance = ObjCompMonitorModStatus.MethodOfCompliance
                            If MethodOfCompliance = "" Then
                                MethodOfCompliance = "----"
                            End If

                            mReportMaintenanceDetail = New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, , , Position, MonitorType, MonitorTypeCode, Note, Remark, Description,
                                                              , , , , Freq1, Freq1, Freq1, ElapsedTime, ElapsedTime, ElapsedTime, RemainingTime, RemainingTime, RemainingTime,
                                                              DueAsof, DueAsof, DueAsof, AssemblyModel, , , , , , , , AssemblyTypeID, , , , , , , , , , , , , , , Number, Reference, DoneOnValue, DoneOnDate, DoneWONo, Applicability, ComplianceRequirement, , , , , , , , , , , Code, , , , IssueDate.Date.ToString("g"), IsApplicable, , MonitorTypeID, , , , , , , , StatusType, MethodOfCompliance:=MethodOfCompliance)

                            mReportMaintenanceDetail.ModelMonitorModCode = ModelMonitorModCode

                            If cnbAdType.SelectedValue = 0 Then
                                ReportMaintenanceDetails.Add(mReportMaintenanceDetail)
                            ElseIf cnbAdType.SelectedValue = 1 Then ''OPen
                                If (ObjCompMonitorModStatus.IsApplicable = True) Then
                                    If (ObjCompMonitorModStatus.IsCompleted = True) And (ObjCompMonitorModStatus.MonitorTypeID = 3 Or ObjCompMonitorModStatus.MonitorTypeID = 1) Then
                                        'Do Nothing
                                    Else
                                        ReportMaintenanceDetails.Add(mReportMaintenanceDetail)
                                    End If
                                Else
                                    'Do Nothing
                                End If
                            ElseIf cnbAdType.SelectedValue = 2 Then ''Closed
                                If (ObjCompMonitorModStatus.IsApplicable = True) Then
                                    If (ObjCompMonitorModStatus.IsCompleted = True) And (ObjCompMonitorModStatus.MonitorTypeID = 3 Or ObjCompMonitorModStatus.MonitorTypeID = 1) Then
                                        ReportMaintenanceDetails.Add(mReportMaintenanceDetail)
                                    Else
                                        'Do Nothing
                                    End If
                                Else
                                    ' ReportMaintenanceDetails.Add(mReportMaintenanceDetail)
                                    If AppSettings("ClientCode") = "APFT" Or
                                       AppSettings("ClientCode") = "AAP" Then
                                        If StatusType = "CLOSED" Then
                                            ReportMaintenanceDetails.Add(mReportMaintenanceDetail)
                                        End If
                                    Else
                                        ReportMaintenanceDetails.Add(mReportMaintenanceDetail)
                                    End If
                                End If
                            End If
                        Next
                    Next
                End If

            Next
        Next
        ''''''' End If
        '''''''  Next
        Return ReportMaintenanceDetails
    End Function

#Region "Old code"
    'Private Sub SetReport()
    '    ReportMaintenanceDetails = New ReportMaintenanceDetailList
    '    ReportStatusList = New rptStatusList
    '    Dim da As New CSLA.Data.ObjectAdapter
    '    Dim ds As New dsReportMaintenanceDetail
    '    Dim RptDirectiveStatusList As CrystalDecisions.CrystalReports.Engine.ReportClass
    '    Dim mCompanyDetail As New CompanyDetail

    '    If optAscending.Checked Then
    '        If cmbFormat.SelectedValue = 0 Then
    '            If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
    '                If chkDirectiveNo.Checked = True Then
    '                    RptDirectiveStatusList = New crDirectiveStatusOrderByModificationNoListIndamer  '' A 
    '                Else
    '                    RptDirectiveStatusList = New crDirectiveStatusListIndamer
    '                End If
    '                'Added By Utkarsh On 07-Apr-2011
    '            ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan") Then
    '                If chkDirectiveNo.Checked = True Then
    '                    RptDirectiveStatusList = New crDirectiveStatusOrderByModificationNoListForDeccan
    '                Else
    '                    RptDirectiveStatusList = New crDirectiveStatusListForDeccan
    '                End If
    '                '*******************************
    '                'Added By Saylee On 14-Apr-2011
    '            ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso ((AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet")) Then
    '                If chkDirectiveNo.Checked = True Then
    '                    RptDirectiveStatusList = New crDirectiveStatusOrderByModificationNoListTAAL  '' A by MOD NO
    '                Else
    '                    RptDirectiveStatusList = New crDirectiveStatusList 'Common report
    '                End If
    '                '*******************************
    '            Else
    '                If chkDirectiveNo.Checked = True Then
    '                    RptDirectiveStatusList = New crDirectiveStatusOrderByModificationNoList     '' A
    '                Else
    '                    RptDirectiveStatusList = New crDirectiveStatusList  'Common report
    '                End If
    '            End If
    '        ElseIf cmbFormat.SelectedValue = 1 Then
    '            If chkDirectiveNo.Checked = True Then
    '                'RptDirectiveStatusList = New crDirectiveStatusListOrderByModificationNoFormat2
    '                If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "KamAir" Then
    '                    RptDirectiveStatusList = New crDirectiveStatusListOrderByModificationNoKamAir
    '                    'Added By Utkarsh On 08-Apr-2011
    '                ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan") Then
    '                    RptDirectiveStatusList = New crDirectiveStatusListOrderByModificationNoFT2ForDeccan
    '                    '******************************
    '                Else
    '                    RptDirectiveStatusList = New crDirectiveStatusListOrderByModificationNoFormat2  '' A
    '                End If

    '            Else
    '                If AppSettings("ClientCode") = "AVE" Then
    '                    RptDirectiveStatusList = New crDirectiveStatusListFormat2forAVE
    '                    'Added By Utkarsh On 08-Apr-2011
    '                ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan")  Then
    '                    RptDirectiveStatusList = New crDirectiveStatusListFT2ForDeccan
    '                    '*******************************
    '                Else
    '                    RptDirectiveStatusList = New crDirectiveStatusListFormat2
    '                End If
    '            End If
    '        End If
    '    Else
    '        If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
    '            If chkDirectiveNo.Checked = True Then
    '                RptDirectiveStatusList = New crDirectiveStatusOrderByModificationNoListDescendingInd '' D
    '            Else
    '                RptDirectiveStatusList = New crDirectiveStatusListDescendingInd
    '            End If

    '            'Commented By Utkarsh On 08-Apr-2011

    '            'ElseIf cmbFormat.SelectedValue = 0 Then
    '            '    If chkDirectiveNo.Checked = True Then
    '            '        RptDirectiveStatusList = New crDirectiveStatusOrderByModificationNoListDescending   '' D
    '            '    Else
    '            '        RptDirectiveStatusList = New crDirectiveStatusListDescending
    '            '    End If

    '            'ElseIf cmbFormat.SelectedValue = 1 Then
    '            '    If chkDirectiveNo.Checked = True Then
    '            '        If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "KamAir" Then
    '            '            RptDirectiveStatusList = New crDirectiveStatusListOrderByModificationNoDescendingKamAir
    '            '        Else
    '            '            RptDirectiveStatusList = New crDirectiveStatusListDescendingByModificationNoFormat2   '' D
    '            '        End If
    '            '    Else
    '            '        RptDirectiveStatusList = New crDirectiveStatusListDescendingFormat2
    '            '    End If
    '            '******************************************************

    '            'Added By Utkarsh On 08-Apr-2011

    '        ElseIf cmbFormat.SelectedValue = 0 Then
    '            If chkDirectiveNo.Checked = True Then
    '                If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan") Then
    '                    RptDirectiveStatusList = New crDirectiveStatusOrderByModificationNoListDescendingForDeccan
    '                ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
    '                    RptDirectiveStatusList = New crDirectiveStatusOrderByModificationNoListDescendingTAAL   '' D
    '                Else
    '                    RptDirectiveStatusList = New crDirectiveStatusOrderByModificationNoListDescending   '' D
    '                End If

    '            Else
    '                If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan") Then
    '                    RptDirectiveStatusList = New crDirectiveStatusListDescendingForDeccan
    '                Else
    '                    RptDirectiveStatusList = New crDirectiveStatusListDescending
    '                End If
    '            End If

    '        ElseIf cmbFormat.SelectedValue = 1 Then
    '            If chkDirectiveNo.Checked = True Then
    '                If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "KamAir" Then
    '                    RptDirectiveStatusList = New crDirectiveStatusListOrderByModificationNoDescendingKamAir
    '                ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan") Then
    '                    RptDirectiveStatusList = New crDirectiveStatusListDescendingByModificationNoFT2ForDeccan
    '                Else
    '                    RptDirectiveStatusList = New crDirectiveStatusListDescendingByModificationNoFormat2   '' D
    '                End If
    '            Else
    '                If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan") Then
    '                    RptDirectiveStatusList = New crDirectiveStatusListDescendingFT2ForDeccan
    '                Else
    '                    RptDirectiveStatusList = New crDirectiveStatusListDescendingFormat2
    '                End If
    '            End If

    '        End If
    '    End If
    '    SetValues()
    '    ReportDetail()

    '    ReportLabel = AssemblyType + " " + DirectiveName

    '    'Added by Prashant on 11-Aug-2011
    '    Dim OperatorName As String = ""
    '    If (AppSettings("ClientCode") IsNot Nothing) AndAlso ((AppSettings("ClientCode") = "Indamer")) Then
    '        Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue))
    '        If mMachineOperatorName.OperatorName <> "" Then OperatorName = mMachineOperatorName.OperatorName
    '    ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan") Then
    '        OperatorName = searchstr7
    '    End If


    '    Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
    '    mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
    '    mCompanyDetail.WebSite, ReportLabel, New SmartDate(txtFromDate.Value.ToString).FormattedText, "", "", "", txtBottomLine.Text, AppSettings("Product Version"), AppSettings("SINote"), "", OperatorName, "", "", AppSettings("Logo")) 'Changed By Utkarsh On 08-Apr-2011
    '    SetSession()
    '    If ReportMaintenanceDetails.Count = 0 Then
    '        Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
    '        msg1.ReplacePage = "wfSearchCriteriaForDirective_Ajax.aspx?Open=" & mOpen
    '        msg1.Show()
    '        Exit Sub
    '    Else
    '        
    '       RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1016)
    '    End If

    '    ds.Clear()
    '    Dim mrptImage As rptImage = rptImage.GetImage(ds)
    '    da.Fill(ds, ReportMaintenanceDetails)
    '    da.Fill(ds, Report)
    '    da.Fill(ds, mrptImage)
    '    da.Fill(ds, ReportStatusList)

    '    RptDirectiveStatusList.SetDataSource(ds)
    '    Session("CrystalReport") = RptDirectiveStatusList
    '    Dim Str As String
    '    Str = "<script language=Javascript>openTranDetail();</script>"
    '     ClientScript.RegisterStartupScript(Me.GetType(),"openTranDetail", Str)
    '    ResetValues()
    'End Sub
#End Region

    Private Sub SetReport(Optional ByVal ByMail As Boolean = False, Optional ByVal IsExcel As Boolean = False)  'Parameter Added by Shital on 14-Sep-2016
        ReportMaintenanceDetails = New ReportMaintenanceDetailList
        ReportStatusList = New rptStatusList
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsReportMaintenanceDetail
        Dim RptDirectiveStatusList As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail

        'Modified By Vikrant on 26-July-2012 For All25072012-2
        If optAscending.Checked Then 'ASCENDING
            If cmbFormat.SelectedValue = 0 Then 'FORMAT 1
                If cmbSortBy.SelectedValue = 0 Then 'SORT BY DIRECTIVE NO
                    If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
                        RptDirectiveStatusList = New crDirectiveStatusOrderByModificationNoListIndamer
                    ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
                        RptDirectiveStatusList = New crDirectiveStatusOrderByModificationNoListForDeccan
                    ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso ((AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet")) Then
                        RptDirectiveStatusList = New crDirectiveStatusOrderByModificationNoListTAAL
                    ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso
                           (AppSettings("ClientCode") = "BSA" Or
                            AppSettings("ClientCode") = "APFT" Or
                            AppSettings("ClientCode") = "AAP") Then ' Added by Saylee :  Or AppSettings("ClientCode") = "APFT"): on 14-Sep-2018 for APFT12092018
                        RptDirectiveStatusList = New crDirectiveStatusOrderByModificationNoListBSA
                    ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso ((AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "MID")) Then
                        RptDirectiveStatusList = New crDirectiveStatusOrderByModificationNoListBA
                    ElseIf AppSettings("ClientCode") = "STR" Then
                        RptDirectiveStatusList = New crDirectiveStatusOrderByModificationNoListForSTR
                    ElseIf AppSettings("ClientCode") = "SUH" Then
                        RptDirectiveStatusList = New crDirectiveStatusOrderByModificationNoListSUH
                    ElseIf AppSettings("ClientCode") = "HSC" Then
                        RptDirectiveStatusList = New crDirectiveStatusOrderByModificationNoListHSC
                    ElseIf AppSettings("ClientCode") = "CAI" Then
                        RptDirectiveStatusList = New crDirectiveStatusOrderByModificationNoListForCAI
                    ElseIf AppSettings("ClientCode") = "7AR" Then
                        RptDirectiveStatusList = New crDirectiveStatusOrderByModificationNoList7Air
                    Else
                        RptDirectiveStatusList = New crDirectiveStatusOrderByModificationNoList
                    End If
                ElseIf cmbSortBy.SelectedValue = 1 Then 'SORT BY ISSUE DATE
                    If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
                        RptDirectiveStatusList = New crDirectiveStatusListIndamer
                    ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
                        RptDirectiveStatusList = New crDirectiveStatusListForDeccan
                    ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso ((AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet")) Then
                        RptDirectiveStatusList = New crDirectiveStatusList
                    ElseIf AppSettings("ClientCode") = "SUH" Then
                        RptDirectiveStatusList = New crDirectiveStatusListSUH
                    ElseIf AppSettings("ClientCode") = "7AR" Then
                        RptDirectiveStatusList = New crDirectiveStatusList7Air
                    Else
                        RptDirectiveStatusList = New crDirectiveStatusList
                    End If
                ElseIf cmbSortBy.SelectedValue = 2 Then 'SORT BY CODE, ONLY for IND code ---- Added by Saylee on 5-Feb-2019
                    RptDirectiveStatusList = New crDirectiveStatusOrderByCodeListIND
                End If
            ElseIf cmbFormat.SelectedValue = 1 Then 'FORMAT 2
                If cmbSortBy.SelectedValue = 0 Then 'SORT BY DIRECTIVE NO
                    If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
                        RptDirectiveStatusList = New crDirectiveStatusListOrderByModificationNoFT2ForDeccan
                    ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "KamAir" Then
                        RptDirectiveStatusList = New crDirectiveStatusListOrderByModificationNoKamAir
                    ElseIf AppSettings("ClientCode") = "SUH" Then
                        RptDirectiveStatusList = New crDirectiveStatusListOrderByModificationNoFormat2SUH
                    Else
                        RptDirectiveStatusList = New crDirectiveStatusListOrderByModificationNoFormat2
                    End If
                ElseIf cmbSortBy.SelectedValue = 1 Then 'SORT BY ISSUE DATE
                    If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
                        RptDirectiveStatusList = New crDirectiveStatusListFT2ForDeccan
                    ElseIf AppSettings("ClientCode") = "AVE" Then
                        RptDirectiveStatusList = New crDirectiveStatusListFormat2forAVE
                    ElseIf AppSettings("ClientCode") = "BSA" Then
                        RptDirectiveStatusList = New crDirectiveStatusListFormat2forBSA
                    ElseIf AppSettings("ClientCode") = "SUH" Then
                        RptDirectiveStatusList = New crDirectiveStatusListFormat2SUH
                    Else
                        RptDirectiveStatusList = New crDirectiveStatusListFormat2
                    End If
                End If
            ElseIf cmbFormat.SelectedValue = 2 Then 'FORMAT 3 'Added By Prashant on 1-Feb-2021 APFT01022021
                If cmbSortBy.SelectedValue = 0 Or cmbSortBy.SelectedItem.Text = "Code" Then 'SORT BY Code
                    RptDirectiveStatusList = New crDirectiveStatusOrderByModificationNoListBSAFormat3 'Same copy of Format 1 for APFT with effective date 
                ElseIf cmbSortBy.SelectedValue = 1 Or cmbSortBy.SelectedItem.Text = "Directive No." Then 'SORT BY DIRECTIVE NO Ajay 11-Nov-2022
                    RptDirectiveStatusList = New crDirectiveStatusOrderByModificationNoFormat3
                End If
            End If
        Else 'DESCENDING
            If cmbFormat.SelectedValue = 0 Then 'FORMAT 1
                If cmbSortBy.SelectedValue = 0 Then 'SORT BY DIRECTIVE NO
                    If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
                        RptDirectiveStatusList = New crDirectiveStatusOrderByModificationNoListDescendingInd
                    ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
                        RptDirectiveStatusList = New crDirectiveStatusOrderByModificationNoListDescendingForDeccan
                    ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso ((AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet")) Then
                        RptDirectiveStatusList = New crDirectiveStatusOrderByModificationNoListDescendingTAAL
                    ElseIf AppSettings("ClientCode") = "SUH" Then
                        RptDirectiveStatusList = New crDirectiveStatusOrderByModificationNoListDescendingSUH
                    Else
                        RptDirectiveStatusList = New crDirectiveStatusOrderByModificationNoListDescending
                    End If
                ElseIf cmbSortBy.SelectedValue = 1 Then 'SORT BY ISSUE DATE
                    If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
                        RptDirectiveStatusList = New crDirectiveStatusListDescendingInd
                    ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
                        RptDirectiveStatusList = New crDirectiveStatusListDescendingForDeccan
                    ElseIf AppSettings("ClientCode") = "SUH" Then
                        RptDirectiveStatusList = New crDirectiveStatusListDescendingSUH
                    Else
                        RptDirectiveStatusList = New crDirectiveStatusListDescending
                    End If
                End If
            ElseIf cmbFormat.SelectedValue = 1 Then 'FORMAT 2
                If cmbSortBy.SelectedValue = 0 Then 'SORT BY DIRECTIVE NO
                    If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
                        RptDirectiveStatusList = New crDirectiveStatusListDescendingByModificationNoFT2ForDeccan
                    ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "KamAir" Then
                        RptDirectiveStatusList = New crDirectiveStatusListOrderByModificationNoDescendingKamAir
                    ElseIf AppSettings("ClientCode") = "SUH" Then
                        RptDirectiveStatusList = New crDirectiveStatusListDescendingByModificationNoFormat2SUH
                    Else
                        RptDirectiveStatusList = New crDirectiveStatusListDescendingByModificationNoFormat2
                    End If
                ElseIf cmbSortBy.SelectedValue = 1 Then 'SORT BY ISSUE DATE
                    If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
                        RptDirectiveStatusList = New crDirectiveStatusListDescendingFT2ForDeccan
                    ElseIf AppSettings("ClientCode") = "SUH" Then
                        RptDirectiveStatusList = New crDirectiveStatusListDescendingFormat2SUH
                    Else
                        RptDirectiveStatusList = New crDirectiveStatusListDescendingFormat2
                    End If
                End If
            End If
        End If
        'End
        SetValues()
        ReportDetail()

        'Report Label change for KamAir : Added By Saylee on 14-apr-2016 |Mail dated:04-Apr-2016
        If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "KamAir" Then
            If AssemblyType <> "" Then
                AssemblyType = AssemblyType + " And Component"
            End If
        End If

        ReportLabel = AssemblyType + " " + DirectiveName

        'Added by Prashant on 11-Aug-2011
        Dim OperatorName As String = ""
        If (AppSettings("ClientCode") IsNot Nothing) AndAlso ((AppSettings("ClientCode") = "Indamer")) Then
            Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue))
            If mMachineOperatorName.OperatorName <> "" Then OperatorName = mMachineOperatorName.OperatorName
        ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "BSA" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
            OperatorName = searchstr7
        End If


        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
        mCompanyDetail.WebSite, ReportLabel, New SmartDate(txtFromDate.Text).FormattedText, "", "", "", txtBottomLine.Text, AppSettings("Product Version"), AppSettings("SINote"), "", OperatorName, IIf(cmbSortBy.SelectedValue = "0", "ModificationNumber", "IssueDate"), IIf(ModelMonitorModTypeNames.ToString.TrimEnd(",").Split(",").Length = ListDirectiveSubType.Items.Count, "", ModelMonitorModTypeNames.ToString.TrimEnd(",")), AppSettings("Logo"), SearchStr11:=AppSettings("ClientCode"), SearchStr12:=ModShortName,
        SearchStr13:=IIf(cmbIssuingAuthority.SelectedIndex = 0, "", cmbIssuingAuthority.SelectedItem.Text)) 'Changed By Utkarsh On 08-Apr-2011

        ReportData = New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
        mCompanyDetail.WebSite, ReportLabel, New SmartDate(txtFromDate.Text).FormattedText, IIf(Aircraft = "", "ALL", Aircraft), IIf(Assembly1 = "", "ALL", Assembly1), DirectiveName, txtBottomLine.Text, AppSettings("Product Version"), AppSettings("SINote"), "", OperatorName, IIf(cmbSortBy.SelectedValue = "0", "ModificationNumber", "IssueDate"), IIf(ModelMonitorModTypeNames.ToString.TrimEnd(",").Split(",").Length = ListDirectiveSubType.Items.Count, "", ModelMonitorModTypeNames.ToString.TrimEnd(",")), AppSettings("Logo"), SearchStr11:=AppSettings("ClientCode"),
        SearchStr13:=IIf(cmbIssuingAuthority.SelectedIndex = 0, "", cmbIssuingAuthority.SelectedItem.Text)) 'Changed By Utkarsh On 08-Apr-2011

        SetSession()

        If ByMail = False Then  ' If case added by shital on 14-Sep-2016
            If ReportMaintenanceDetails.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                If Not IsExcel Then RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 1016)
            End If
        End If

        'added by shital on 14-Sep-2016
        If (ByMail = True And ReportMaintenanceDetails.Count <= 0) Then
            SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, ReportLabel, "", "There is no record for this search criteria.", "",
                Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"),
                ReportGeneratedBy:=Session("ReportGenratedBy"),
                   SmtpHost:=mModuleList.Item("DirectiveReport").SmtpHost, SmtpPort:=mModuleList.Item("DirectiveReport").SmtpPort,
                SmtpUser:=mModuleList.Item("DirectiveReport").SmtpUser, SmtpPassword:=mModuleList.Item("DirectiveReport").SmtpPassword)
            Exit Sub
        End If

        If Not IsExcel Then
            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, ReportMaintenanceDetails)
            da.Fill(ds, Report)
            da.Fill(ds, mrptImage)
            da.Fill(ds, ReportStatusList)

            RptDirectiveStatusList.SetDataSource(ds)
            Session("CrystalReport") = RptDirectiveStatusList
        End If


        'added by shital on 14-Sep-2016
        If (ByMail = True) Then
            SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, ReportLabel, "", " For " + lblAircraft1.Text, ,
                                      Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"),
                                      ReportGeneratedBy:=Session("ReportGenratedBy"),
                   SmtpHost:=mModuleList.Item("DirectiveReport").SmtpHost, SmtpPort:=mModuleList.Item("DirectiveReport").SmtpPort,
                SmtpUser:=mModuleList.Item("DirectiveReport").SmtpUser, SmtpPassword:=mModuleList.Item("DirectiveReport").SmtpPassword)
        Else
            If Not IsExcel Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
                ' MarkLog(Util.Action.Print, "DirectiveStatus", mSearchCriteriaForEventLog, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                MarkLog(Util.Action.Print, "DirectiveReport", mSearchCriteriaForEventLog, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                ResetValues()
            End If
        End If

        'If Not IsExcel Then
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        '    ' MarkLog(Util.Action.Print, "DirectiveStatus", mSearchCriteriaForEventLog, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        '    MarkLog(Util.Action.Print, "DirectiveReport", mSearchCriteriaForEventLog, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        'End If


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
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
        End If
    End Sub
#End Region

#Region " Data Binding "
    Public Sub SetComboOfMachine(ByVal AOnDate As String)
        mMachineNameValueList = MachineNameValueList.GetMachineList(AOnDate, , , , , , , True, "(Select)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataBind()

        lblAssembly.Enabled = False
        cmbAssembly.Enabled = False
    End Sub
    Private Sub DataFieldBind()
        mModTypeList = ModTypeList.GetModelTypeList(False)
        'cmbType.DataSource = mModTypeList
        ListDirectiveType.DataSource = mModTypeList
        Session("mModTypeList") = mModTypeList
        cmbFormat.Enabled = (Not AppSettings("ClientCode") = "Indamer")

        mIssuingAuthorityTypeList = IssuingAuthorityTypeList.GetIssuingAuthorityTypeList(IsSelectTagRequired:=True)
        cmbIssuingAuthority.DataSource = mIssuingAuthorityTypeList

        DataBind()
    End Sub
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If custValidator.ControlToValidate = "cmbAssembly" Then
            'If ListDirectiveType.SelectedIndex = -1 Then
            '    custValidator.ErrorMessage = "Please select the Directive"
            '    e.IsValid = False
            'Else
            '    e.IsValid = True
            'End If
            If ListDirectiveType.SelectedIndex = -1 Then
                custValidator.ErrorMessage = "Please select the Directive"
                e.IsValid = False
            ElseIf ListDirectiveSubType.SelectedIndex = -1 Then
                custValidator.ErrorMessage = "Please select the Directive Type"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("Sender") = "" Then
            Session("MiddleFrame") = "wfSearchCriteriaForDirective_Ajax.aspx?"
            txtFromDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            AOnDate = Now.Date
            SetComboOfMachine(AOnDate)
            DataFieldBind()
            Report = 1
            Session("Report") = Report
            If (AppSettings("ClientCode") IsNot Nothing) Then
                If (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
                    txtBottomLine.Text = "I hereby certify that the data specified above has been verified throughout. License No.: __________ Date: _____________"
                    ' ElseIf (AppSettings("ClientCode") = "AVE") Then
                    '   txtBottomLine.Text = "Prepared by Engineering Department in accordance with FAA Bi-Weekly Report ____________________.I hereby certify that the data specified above has been verified throughout.Engineering Department Manager: __________________ Date: _____________ .Quality Assurance Manager: __________________ Date: _____________ "
                ElseIf (AppSettings("ClientCode") = "Indamer") Then
                    ''txtBottomLine.Text = "I hereby certify that the aircraft miscellaneous mandatory modifications/inspection specified above are as per DGCA listing and that no miscellaneous mandatory modification/inspection is outstanding as on this date."
                    txtBottomLine.Text = "Date:" + IIf(IsExcel, Chr(10), vbCrLf) + IIf(IsExcel, Chr(10), vbCrLf) + "Place:" + IIf(IsExcel, Chr(10), vbCrLf) + IIf(IsExcel, Chr(10), vbCrLf) + "Prepared By:                                                                                                      Checked By:                                                                                                                 Approved By:"
                ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo") Then 'Added By Vikrant On 14-March-2014 For All14032014
                    txtBottomLine.Text = "I hereby certify that the data specified above has been certified throughout : 									Technical Support Division: __________________ Date: _____________"
                ElseIf AppSettings("ClientCode") = "APFT" Or
                       AppSettings("ClientCode") = "AAP" Then 'Added By Saylee On 1-Oct-2018 
                    txtBottomLine.Text = "I hereby certify that the data specified above has been verified throughout. Continuing Airworthiness Manager: __________________ Date: _____________"
                ElseIf AppSettings("ClientCode") = "Dana" Then
                    txtBottomLine.Text = "The above AD list is updated to FAA AD Bi-weekly No __________________ and " + IIf(IsExcel, Chr(10), vbCrLf + vbCrLf) +
                    "Prepared by __________________ , 					Designation __________________, 					Signature and Date ______________________ " + IIf(IsExcel, Chr(10), vbCrLf + vbCrLf) +
                    "Checked by __________________, 					Designation __________________, 					Signature and Date _____________________"
                End If
            Else
                txtBottomLine.Text = "I hereby certify that the data specified above has been verified throughout. Planning Manager: __________________ License No.: __________ Date: _____________"
            End If
            'Ajay 10-Nov-2022
            If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "DirectiveReport") Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav();", True)
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
            End If
            '--------------------------
        End If
        'Added By Vikrant on 30-Aug-2012 For Dec30082012
        'Added by Saylee on 14-Sep-2018;
        If (AppSettings("ClientCode") = "Deccan" Or
            AppSettings("ClientCode") = "ADeccan" Or
            AppSettings("ClientCode") = "IIC" Or
            AppSettings("ClientCode") = "SPZ" Or
            ((AppSettings("ClientCode") = "APFT" Or
              AppSettings("ClientCode") = "AAP") And
             cmbFormat.SelectedValue = 0 And
             optAscending.Checked = True) Or cmbFormat.SelectedValue = 2) Then


            cmbSortBy.DataBind()
            'cmbType.DataBind()

            ListDirectiveType.DataBind()
            If cmbFormat.SelectedValue = 2 Then 'Added By Prashant on 1-Feb-2021 APFT01022021
                optDescending.Visible = False
                optAscending.Visible = False
                '  cmbSortBy.Enabled = False
                optAscending.Checked = True
                cmbSortBy.Items(0).Text = "Code"
                cmbSortBy.Items(1).Text = "Directive No."
            Else
                cmbSortBy.Enabled = True
                optDescending.Visible = True
                optAscending.Visible = True
                If cmbFormat.SelectedValue = 0 Or cmbFormat.SelectedValue = 1 Then
                    cmbSortBy.Items(0).Text = "Code"
                    cmbSortBy.Items(1).Text = "Issue Date"
                End If
            End If
        ElseIf (AppSettings("ClientCode") = "APFT") And (cmbFormat.SelectedValue = 1 Or optAscending.Checked = False) Then
            cmbSortBy.Items(0).Text = "Directive No."
            cmbSortBy.Items(1).Text = "Issue Date"
            cmbSortBy.DataBind()
            cmbSortBy.Enabled = True
            optDescending.Visible = True
            optAscending.Visible = True
            'cmbType.DataBind()
            ListDirectiveType.DataBind()
        ElseIf (AppSettings("ClientCode") = "IND") Then
            If (cmbFormat.SelectedValue = 0 And optAscending.Checked = True) Then
                cmbSortBy.Items.Add(New ListItem("Code", "2"))
                cmbSortBy.DataBind()
                'cmbType.DataBind()
                ListDirectiveType.DataBind()
            Else
                cmbSortBy.Items.RemoveAt(2)
                cmbSortBy.DataBind()
                'cmbType.DataBind()
                ListDirectiveType.DataBind()
            End If
        Else
            optDescending.Visible = True
            optAscending.Visible = True
            cmbSortBy.Items(0).Text = "Directive No."
            cmbSortBy.Items(1).Text = "Issue Date"
        End If
        'End
        SetSession()
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            IsExcel = False
            Dim mTemList As LogList  ''Added by Prashant 9-Nov-2010
            mTemList = LogList.GetLogList(New Guid(cmbAircraft.SelectedValue.ToString))    ''Added by Prashant 9-Nov-2010
            If mTemList.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.EnterFlightLog, MSGBox.Message_text.EnterFlightLog, "Enter at least one Flight Log for this Aircraft to view this report", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If
            SetReport(False, False)  'Parameter Added by Shital on 14-Sep-2016
        Else
            upnlValidationSummary.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub txtFromDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFromDate.TextChanged
        If IsDate(txtFromDate.Text) Or (txtFromDate.Text = "") Then
            If AOnDate.Equals(txtFromDate.Text) Then
            Else
                SetComboOfMachine(txtFromDate.Text)
            End If
        Else
            txtFromDate.Text = ""
        End If
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        If cmbAircraft.SelectedIndex = 0 Then
            lblAssembly.Enabled = False
            cmbAssembly.Enabled = False
        Else
            lblAssembly.Enabled = True
            cmbAssembly.Enabled = True

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

	Private Sub ExportToExcel(sender As Object, e As EventArgs) Handles btnExport.Click

		Dim dsNew As New DataSet
		Dim dataAdapter As New ObjectAdapter
		Dim dataSet As New dsReportMaintenanceDetail
		Dim PeriodColumnsForExportToExcel As New List(Of String)
		Try

			If IsValid Then

				IsExcel = True
				Report = Nothing
				ReportStatusList = Nothing
				ReportMaintenanceDetails = Nothing
				ReportStatusList = New rptStatusList
				ReportMaintenanceDetails = New ReportMaintenanceDetailList

				SetReport(IsExcel:=True)

				If ReportMaintenanceDetails.Count = 0 Then
					MSGBoxCtrl.Show(MSGBox.Message_Title.NoRecordFound,
									MSGBox.Message_Text.NoRecordFound,
									"There are no records for this Search Criteria.",
									MsgBoxStyle.OkOnly,
									"")
					Exit Sub
				End If

				dataSet.Clear()
				dataAdapter.Fill(dataSet, "ExcelReportMaintenanceDetailList", ReportMaintenanceDetails)
				dataAdapter.Fill(dataSet, "ReportData", ReportData)

				Dim columnToRemove As String() = {
												"ID",
												"Code",
												"Name",
												"Model",
												"SerialNo",
												"Freq2",
												"Freq3",
												"ElapsedTime1",
												"ElapsedTime2",
												"RemainingTime1",
												"RemainingTime2",
												"DueAsof1",
												"DueAsof2",
												"AssemblySerialNo",
												"EstimatedDate",
												"ComponentInfo",
												"RegNo",
												"AssemblyType",
												"SinceNew",
												"SinceNew1",
												"DoneAt",
												"DoneAt1",
												"AssemblyModel",
												"MinimumRemainingValue",
												"AssemblyTypeID",
												"MaintenanceEvent",
												"InstalledAt1",
												"InstalledAt2",
												"TSO1",
												"TSO2",
												"RemoveAt1",
												"RemoveAt2",
												"DoneWONo",
												"DetailID",
												"AssemblyDueAsof",
												"AssemblyDueAsof1",
												"AssemblyDueAsof2",
												"Extension",
												"Extension1",
												"Extension2",
												"ExtensionDate",
												"ApprovalRemark",
												"RequiredManHours",
												"Customer",
												"SupersededByADNumber",
												"IsApplicable",
												"MaintenanceTypeID",
												"MaintenanceTypeName",
												"IsLater",
												"DueStatus",
												"TimeSinceNew",
												"ModelMonitorModCode",
												"WONumber",
												"StatusMasterID",
												"StatusID",
												"TypeID",
												"CompStatusID",
												"AssemblyStatusID",
												"DocumentTypeForID",
												"MaintenanceOn",
												"MaintenanceInformation",
												"MaintenanceInfo",
												"Frequency",
												"SinceNewAll",
												"ElapsedAll",
												"DoneAtAll",
												"ExtensionAll",
												"DueAsofAll",
												"AssDueAsofAll",
												"RemainingTimeAll",
												"LogBook",
												"DoneOnDate",
												"RemoveAt",
												"ATACode",
												"InstalledAtDate",
												"RemoveAtDate",
												"EstDate",
												"PartNo",
												"CompSerialNo",
												"Position",
												"DoneONValueForAssembly",
												"SinceNew2",
												"DoneAt2",
												"InstalledAt",
												"TSN",
												"TSO",
												"ATAChapter",
												"MachineID",
												"ModelID",
												"MaintenanceOnExcel",
												"MaintenanceInformationExcel",
												"RemainingTimeAllExcel",
												"AssDueAsofAllExcel",
												"DueAsofAllExcel",
												"ExtensionAllExcel",
												"DoneAtAllExcel",
												"EffectiveFromAllExcel",
												"ElapsedAllExcel",
												"SinceNewAllExcel",
												"FrequencyExcel",
												"MaintenanceInfoExcel",
												"DiffCompInstDoneOnValue",
												"EROQtyNosForMaterialMgmtReport",
												"POQtyNosForMaterialMgmtReport",
												"PONosForMaterialMgmtReport",
												"POQtyForMaterialMgmtReport",
												"ERONosForMaterialMgmtReport",
												"EROQtyForMaterialMgmtReport",
												"UnserviceableStockQty",
												"ServiceableStockQty",
												"BinCardTotalQty",
												"Area",
												"Zone",
												"IsMaster",
												"RecordID",
												"EffectiveFromAll",
												"DueAsOfAssemblyOrCompForExcel",
												"DueAsOfAirframeForExcel",
												"RemainingForExcel",
												"MaintenanceActivityType",
												"HoursFreq",
												"CyclesFreq",
												"DaysMnthsYrsName",
												"DaysMnthsYrsValue",
												"LandingsFreq",
												"HoursDoneOnValue",
												"CyclesDoneOnValue",
												"DaysMnthsYrsDoneOnValue",
												"LandingsDoneOnValue",
												"Manufacturer",
												"InstallationWONo",
												"InstallationRemark",
												"InstallationDoneBy",
												"InstPlace",
												"TSNHours", "
                                                SinceNewDate",
												"SinceNewLandings",
												"CSNCycles",
												"InstCompHours",
												"InstCompStartDate",
												"InstCompLandings",
												"InstCompCycles",
												"AssemblyInstHours",
												"AssemblyInstStartDate",
												"AssemblyInstLandings",
												"AssemblyInstCycles",
												"PartMonitorCode",
												"PartDesc",
												"MonitorType",
												"Description",
												"MaintenanceInformationForExcel",
												"MaintenanceActivityType",
												"Note",
												"MethodOfCompliance",
												"IsRII",
												"ReqNumber",
												"LinkedMaintenanceActivityCount",
												"TSO1ForExcel",
												"InstalledAtForExcel",
												"Freq1",
												"TSNForExcel",
												"DoneOnValue",
												"RemainingTime",
												"TaskNo",
												"TaskNoExcel",
												"TaskReferenceForExcel",
												"Skill",
												"SkillID",
												"ModelEstimatedManHours",
												"SourceDoc",
												"TSOForExcel",
												"WONoExcel",
												"DueAsof",
												"Freq1ForExcel",
												"DescriptionSourceDocForExcel",
												"MonitorTypeCode",
												"PartNoSerialNoforExcel",
												"SinceNewDate",
												"Reference",
												"Applicability"
											}

				For i As Integer = 0 To columnToRemove.Length - 1
					If dataSet.Tables("ExcelReportMaintenanceDetailList").Columns.Contains(columnToRemove(i)) Then
						dataSet.Tables("ExcelReportMaintenanceDetailList").Columns.Remove(columnToRemove(i))
					End If
				Next

				Dim columnsCount As Integer = dataSet.Tables("ExcelReportMaintenanceDetailList").Columns.Count
				dataSet.Tables("ExcelReportMaintenanceDetailList").Columns("ModificationNumber").SetOrdinal(0)
				dataSet.Tables("ExcelReportMaintenanceDetailList").Columns("ReferenceForExcel").SetOrdinal(1)
				dataSet.Tables("ExcelReportMaintenanceDetailList").Columns("DescriptionForExcel").SetOrdinal(2)
				dataSet.Tables("ExcelReportMaintenanceDetailList").Columns("MonitorTypeWithCode").SetOrdinal(3)
				dataSet.Tables("ExcelReportMaintenanceDetailList").Columns("IssueDate").SetOrdinal(4)
				dataSet.Tables("ExcelReportMaintenanceDetailList").Columns("ApplicabilityForExcel").SetOrdinal(5)
				dataSet.Tables("ExcelReportMaintenanceDetailList").Columns("StatusTypeName").SetOrdinal(6)
				dataSet.Tables("ExcelReportMaintenanceDetailList").Columns("ComplianceRequirement").SetOrdinal(8)
				dataSet.Tables("ExcelReportMaintenanceDetailList").Columns("ThresholdAccordingToTypeIDForExcel").SetOrdinal(9)
				dataSet.Tables("ExcelReportMaintenanceDetailList").Columns("FrequencyAccordingToTypeIDForExcel").SetOrdinal(10)
				dataSet.Tables("ExcelReportMaintenanceDetailList").Columns("DoneOnValueForExcel").SetOrdinal(11)
				dataSet.Tables("ExcelReportMaintenanceDetailList").Columns("DueAsOfForExcel").SetOrdinal(12)
				dataSet.Tables("ExcelReportMaintenanceDetailList").Columns("ElapsedTime").SetOrdinal(13)
				dataSet.Tables("ExcelReportMaintenanceDetailList").Columns("RemainingTimeForExcel").SetOrdinal(14)
				dataSet.Tables("ExcelReportMaintenanceDetailList").Columns("NoteForExcel").SetOrdinal(15)
				dataSet.Tables("ExcelReportMaintenanceDetailList").Columns("Remark").SetOrdinal(columnsCount - 1)

				Dim DueLabel As String = "DueAsof"

				For i As Integer = 0 To dataSet.Tables("ExcelReportMaintenanceDetailList").Columns.Count - 1
					If dataSet.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "ModificationNumber" Then
						dataSet.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Directive No"
					End If
					If dataSet.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "ReferenceForExcel" Then
						dataSet.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Reference"
					End If
					If dataSet.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "DescriptionForExcel" Then
						dataSet.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Description"
					End If
					If dataSet.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Freq1ForExcel" Then
						dataSet.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Frequency"
					End If
					If dataSet.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "MonitorTypeWithCode" Then
						dataSet.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Type"
					End If

					If dataSet.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "StatusTypeName" Then
						dataSet.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Status"
					End If
					If dataSet.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "DoneOnValueForExcel" Then
						dataSet.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Last Carried Out"
					End If
					If dataSet.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "RemainingTimeForExcel" Then
						dataSet.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Remaining"
					End If
					If dataSet.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "DueAsOfForExcel" Then
						dataSet.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Due As Of"
					End If
					If dataSet.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "ComplianceRequirement" Then
						dataSet.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Method Of Compliance"
					End If
					If dataSet.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "NoteForExcel" Then
						dataSet.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Note"
					End If

					If dataSet.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("ThresholdAccordingToTypeIDForExcel") Then
						dataSet.Tables("ExcelReportMaintenanceDetailList").Columns("ThresholdAccordingToTypeIDForExcel").ColumnName = "Threshold"
					End If

					If dataSet.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("FrequencyAccordingToTypeIDForExcel") Then
						dataSet.Tables("ExcelReportMaintenanceDetailList").Columns("FrequencyAccordingToTypeIDForExcel").ColumnName = "Frequency"
					End If
					If dataSet.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "ApplicabilityForExcel" Then
						dataSet.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Applicability"
					End If

				Next

				Dim columnToRemoveCriteria As String() = {
													  "ReportDate",
													  "ID",
													  "CompanyName",
													  "Address",
													  "Tel1",
													  "Tel2",
													  "Fax",
													  "Email",
													  "WebSite",
													  "ReportName",
													  "SearchStr5",
													  "SearchStr6",
													  "SearchStr7",
													  "SearchStr8",
													  "SearchStr9",
													  "ProductVersion",
													  "SINote",
													  "CurrencyName",
													  "CurrencySymbol",
													  "SearchStr10",
													  "SearchStr11",
													  "SearchStr12",
													  "SearchStr13",
													  "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", "ShortName",
													 "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25", "SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40", "SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47", "SearchStr48", "SearchStr49", "SearchStr50", "SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55", "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60", "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65", "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70", "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95", "SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"
													 }

				For i As Integer = 0 To columnToRemoveCriteria.Length - 1
					If dataSet.Tables("ReportData").Columns.Contains(columnToRemoveCriteria(i)) Then
						dataSet.Tables("ReportData").Columns.Remove(columnToRemoveCriteria(i))
					End If
				Next

				For i As Integer = 0 To dataSet.Tables("ReportData").Columns.Count - 1
					If dataSet.Tables("ReportData").Columns(i).ColumnName = "SearchStr1" Then
						dataSet.Tables("ReportData").Columns(i).ColumnName = "AsOnDate"
					End If
					If dataSet.Tables("ReportData").Columns(i).ColumnName = "SearchStr2" Then
						dataSet.Tables("ReportData").Columns(i).ColumnName = "Reg No."
					End If
					If dataSet.Tables("ReportData").Columns(i).ColumnName = "SearchStr3" Then
						dataSet.Tables("ReportData").Columns(i).ColumnName = "Assembly"
					End If
					If dataSet.Tables("ReportData").Columns(i).ColumnName = "SearchStr4" Then
						dataSet.Tables("ReportData").Columns(i).ColumnName = "Directive"
					End If
				Next

				Dim DataView As DataView = dataSet.Tables("ExcelReportMaintenanceDetailList").DefaultView
				DataView.Sort = "Directive No"

				dataSet.Tables("ReportData").TableName = "Searching Criteria"
				dataSet.Tables("ExcelReportMaintenanceDetailList").TableName = ReportLabel
				Session("DataTableToBeFormattedForExportToExcel") = ReportLabel

				dsNew.Clear()

				dsNew.Merge(dataSet.Tables("Searching Criteria"))
				dsNew.Merge(DataView.ToTable())

				PeriodColumnsForExportToExcel.AddRange(New String() {"Frequency", "ElapsedTime", "RemainingTime", "DueAsof", "Last Carried Out"})

				Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
				Session("ExcelFileName") = ReportLabel.Replace("/", " ")
				Session("dsNew") = dsNew

				ScriptManager.RegisterStartupScript(Me,
													[GetType],
													"Display Report In Excel",
													"displayReportInExcel();",
													True)
				'Added by Prashant on 19-Jan-2021
				MarkLog(Action.Print,
						"DirectiveReport",
						"Export To Excel " + mSearchCriteriaForEventLog,
						ErrorType.NoError,
						Guid.Empty, EventLogID)

			Else
				upnlValidationSummary.Update()
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

	'Added by Shital on 14-Sep-2016
	Private Sub btnByMail_Click(sender As Object, e As System.EventArgs) Handles btnByMail.Click
        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        'Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail

        Session("UserEmailID") = mModuleList.Item("DirectiveReport").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("DirectiveReport").SendCCMailID
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

    Private Sub ListDirectiveType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ListDirectiveType.SelectedIndexChanged
        Dim DirectveTypeIDs As New StringBuilder
        For i As Integer = 0 To ListDirectiveType.Items.Count - 1
            If ListDirectiveType.Items(i).Selected Then
                If DirectveTypeIDs.ToString = "" Then
                    DirectveTypeIDs.Append("<ModTypeID>")
                End If
                DirectveTypeIDs.Append("<id>")
                DirectveTypeIDs.Append(ListDirectiveType.Items(i).Value)
                DirectveTypeIDs.Append("</id>")
            End If
        Next
        If DirectveTypeIDs.ToString <> "" Then
            DirectveTypeIDs.Append("</ModTypeID>")
        End If
        If DirectveTypeIDs.ToString = "" Then
            ListDirectiveSubType.Enabled = False
            ListDirectiveSubType.ClearSelection()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "disableDirectiveSubType", "disableDirectiveSubType();", True)
        Else
            ListDirectiveSubType.Enabled = True
            mModificationTypeList = ModelMonitorModTypeList.GetModelMonitorModTypeList(ModelMonitorModTypeIDs:=DirectveTypeIDs.ToString)
            ListDirectiveSubType.DataSource = mModificationTypeList
            ListDirectiveSubType.DataBind()
            For Each Item As ListItem In ListDirectiveSubType.Items
                Item.Selected = True
            Next
        End If

        upnlDirectiveSubType.Update()
    End Sub
    'Ajay 10-Nov-2022
    Private Sub hdnBtnMarkFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnMarkFav.Click 'Ajay 08-Nov-2022
        MarkFavourite(HttpContext.Current.User.Identity.Name, "DirectiveReport")
    End Sub

    Private Sub hdnBtnRemoveFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnRemoveFav.Click 'Ajay 08-Nov-2022
        RemoveFavourite(HttpContext.Current.User.Identity.Name, "DirectiveReport")
    End Sub
    '-----
End Class