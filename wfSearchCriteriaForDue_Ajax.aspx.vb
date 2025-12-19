Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Public Class wfSearchCriteriaForDue_Ajax
    Inherits Page

#Region " Enumeration "                   'Added Code By Girish 25,April,2007

    Enum Open
        CofAReport = 1
        RoutineInspectionReport = 2
        ModificationReport = 3
        DueReport = 4
    End Enum

#End Region

#Region " Variable Declaration "

    Dim mDueLimits As DueLimits
    Dim mPerDayLimits As PerDayLimits
    Dim ReportStatusList As New rptStatusList
    Dim mMachineList As MachineList

    Dim mMachineNameValueList As MachineNameValueList

    '  Dim mtmpMachineList As tmpMachineList
    Dim ReportMaintenanceDetails As New ReportMaintenanceDetailList
    Dim mReportMaintenanceDetail As New ReportMaintenanceDetail

    Dim ObjMachine As MachineInfo
    Dim ObjAssemblyStatus As AssemblyStatusInfo
    Dim ObjAssemblyStatusPeriod As AssemblyStatusPeriodInfo
    Dim ObjCompStatus As CompStatusInfo
    Dim ObjCompStatusPeriod As CompStatusPeriodInfo

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

    Private Flag As Int16
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
    Dim mDueLimit As DueLimit
    Dim AvgMnths As Integer

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
    Private Description As String
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
    Private DueType As Integer

    Private mIsPreview As Boolean = False '11-Sep-2008

    'Added by Saylee on 12-Feb-2009
    Dim AircraftIndex As Integer
    Dim mAssemblyList As AssemblyList
    Dim AssemblyName As String
    Dim Assembly1 As String
    Dim TypeName As String
    Public mOpen As Open

    Dim mServiceTypeList As PartMonitorServiceTypeList
    Dim mInspectionTypeList As ModelMonitorInspTypeList
    Dim mModificationTypeList As ModelMonitorModTypeList
    Dim InspIndex As Integer
    Dim SerIndex As Integer
    Dim ModIndex As Integer
    Dim Extension As String
    Dim Extension1 As String
    Dim Extension2 As String
    Dim ExtensionDate As String
    Dim ApprovalRemark As String
    Dim RequiredManHours As String
    Dim Customer As String
    Dim Remark As String
    Dim Code As String
    Dim StatusMasterID As Guid
    Dim DocumentTypeForID As Integer
    Dim AssemblyDueAsof As String  'Added By DEVEN On 14/06/2008
    Dim AssemblyDueAsof1 As String 'Added By DEVEN On 14/06/2008
    Dim AssemblyDueAsof2 As String

    Dim IsSerSelect As Boolean = False
    Dim IsModSelect As Boolean = False
    Dim IsInsSelect As Boolean = False

    Dim ServiceTypeID(50) As Integer
    Dim InspectionTypeID(50) As Integer
    Dim ModificationTypeID(50) As Integer
    Dim DueStatus As Integer
    Dim searchstr7 As String = ""
    Dim DoneOnDate As String = ""

    Dim StatusID As Guid
    Dim nWONumber As String = ""
    Dim mnWOListForDueJobs As nWOListForDueJobs

    Dim mEventLogDetails As String = String.Empty
    Dim mIsExcel As Boolean
    Dim PerDayLimitForDaysPeriod As Integer = -1 'Added By Vikrant On 14-Jan-2016 For ALL14012016
    Dim mCompanyDetail As New CompanyDetail
    Private Zone, Area As String
    Private IsRII As Boolean
    Private mFAScsReportList As FAScsReportList
    'REQ
    Dim ReqNumber As New StringBuilder
    Dim mRequisitionListNew As RequisitionListNew
    'End
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    Dim LinkedMaintenanceActivityCount As Integer = 0   'Added by Prashant  9-Sep-2020 ALL09092020
    Dim CustomerName As String = "" 'Added By Prashant on 22-Jun-2023
    Dim mLastAMPRef As LastMPDAMPRef 'Added by Ajay on 14-08-2023
    Dim searchstr20 As String = ""

#End Region

#Region " Helper Methods "

    Private Sub AddAttributes()
        txtAvgMnths.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtAvgMnths').value,event)")
        txtForecastingLimit.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtForecastingLimit').value,event)")
    End Sub

    Private Sub SetGridObject()
        Dim txtLimit As TextBox
        Dim i As Int32
        For i = 0 To Me.gdvDuePeriodLimits.Rows.Count - 1
            txtLimit = CType(Me.gdvDuePeriodLimits.Rows(i).FindControl("txtLimit"), TextBox)
            'mDueLimits.Item(i).PeriodLimit = CDec(Val(Trim(txtLimit.Text))) 'Commented by Saylee on 12-Nov-2012
            mDueLimits.Item(i).PeriodLimit = Trim(txtLimit.Text) 'Added by Saylee on 12-Nov-2012
            'Added By Vikrant On 14-Jan-2016 For ALL14012016
            If mDueLimits.Item(i).PeriodID = 2 Then
                PerDayLimitForDaysPeriod = CInt(IIf(mDueLimits.Item(i).PeriodLimit <> "", mDueLimits.Item(i).PeriodLimit, 0))
            End If
            'End
        Next i
        Session("mDueLimits") = mDueLimits

        Dim txtPerDatLimit As TextBox
        Dim i1 As Int32
        For i1 = 0 To Me.gdvPerDayLimit.Rows.Count - 1
            txtPerDatLimit = CType(Me.gdvPerDayLimit.Rows(i1).FindControl("txtLimitPerDay"), TextBox)
            'mPerDayLimits.Item(i1).PeriodLimit = CDec(Val(Trim(txtPerDatLimit.Text))) 'Commented by Saylee on 12-Nov-2012
            mPerDayLimits.Item(i1).PeriodLimit = Trim(txtPerDatLimit.Text)  'Added by Saylee on 12-Nov-2012
        Next i1
        Session("mPerDayLimits") = mPerDayLimits

    End Sub

    Private Sub GetSession()
        mMachineList = CType(Session("mMachineList"), MachineList)
        mDueLimits = CType(Session("mDueLimits"), DueLimits)
        mPerDayLimits = CType(Session("mPerDayLimits"), PerDayLimits)

        AOnDate = Session("AOnDate")
        Report = Session("Report")
        Type = Session("Type")
        AvgMnths = Session("AvgMnths")

        DueType = Session("DueType")

        'Added by Saylee on 12-Feb-2009
        mAssemblyList = CType(Session("mAssemblyList"), AssemblyList)
        mServiceTypeList = CType(Session("mServiceTypeList"), PartMonitorServiceTypeList)
        mInspectionTypeList = CType(Session("mInspectionTypeList"), ModelMonitorInspTypeList)
        mModificationTypeList = CType(Session("mModificationTypeList"), ModelMonitorModTypeList)

        mMachineNameValueList = Session("mMachineNameValueList")
        mCompanyDetail = Session("mCompanyDetail")
        mFAScsReportList = Session("mFAScsReportList")
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
    End Sub

    Private Sub SetSession()
        Session("mMachineList") = mMachineList
        Session("mDueLimits") = mDueLimits
        Session("mPerDayLimits") = mPerDayLimits
        Session("AOnDate") = AOnDate
        Session("Report") = Report
        Session("Type") = Type
        Session("AvgMnths") = AvgMnths
        Session("DueType") = DueType

        'Added by Saylee on 12-Feb-2009
        Session("mAssemblyList") = mAssemblyList
        Session("SerIndex") = SerIndex
        Session("InspIndex") = InspIndex
        Session("ModIndex") = ModIndex
        Session("mServiceTypeList") = mServiceTypeList
        Session("mInspectionTypeList") = mInspectionTypeList
        Session("mModificationTypeList") = mModificationTypeList

        Session("mMachineNameValueList") = mMachineNameValueList
    End Sub

    Private Sub ClearAll()
        DueType = Session("DueType")
        If Session("MiddleFrame") <> "wfSearchCriteriaForDue_Ajax.aspx?DueType=" & DueType Then
            Session.Remove("mMachineList")
            Session.Remove("mDueLimits")
            Session.Remove("mPerDayLimits")
            Session.Remove("AOnDate")
            Session.Remove("Report")
            Session.Remove("Type")
            Session.Remove("AvgMnths")

            'Added by Saylee on 12-Feb-2009
            Session.Remove("mAssemblyList")
            Session.Remove("SerIndex")
            Session.Remove("InspIndex")
            Session.Remove("ModIndex")

            Session.Remove("mMachineNameValueList")
            Session.Remove("mServiceTypeList")
            Session.Remove("mInspectionTypeList")
            Session.Remove("mModificationTypeList")
            Session.Remove("mFAScsReportList")
        End If
        mIsExcel = False
    End Sub

    Private Overloads Sub SetFocus(control As WebControl)
        If control.Enabled = False Or control.Visible = False Then Exit Sub
        control.Focus()
    End Sub

    Private Sub Display()
        lblAircraft1.Visible = True
        lblAvgMnths1.Visible = (DueType = 1)
        lblDateRangeFrom.Visible = True
        lblPercent.Visible = (DueType = 1)
        lblAssembly1.Visible = True
        ''lblType1.Visible = True
        upnlSearchingCriteria.Update()
    End Sub

    Private Sub SetValues()
        If (cmbAircraft.SelectedItem.Text = "(ALL)") Or (cmbAircraft.SelectedItem.Text = "(SELECT)") Then
            MachineName = "{00000000-0000-0000-0000-000000000000}"
            AssemblyName = "{00000000-0000-0000-0000-000000000000}"
            Assembly1 = ""
            lblAssembly1.Text = ""
        Else
            MachineName = cmbAircraft.SelectedValue.ToString

            'Added by Saylee on 12-Feb-2009
            If cmbAssembly.SelectedItem.Text = "(ALL)" Then
                AssemblyName = "{00000000-0000-0000-0000-000000000000}"
                Assembly1 = ""
                AssemblyType = "(ALL)"
                lblAssembly1.Text = "Assembly Name  : " + "<b> ALL </b>"         'Added Code
            Else
                AssemblyType = mAssemblyList(cmbAssembly.SelectedIndex).AssemblyType
                AssemblyName = cmbAssembly.SelectedValue.ToString
                Assembly1 = cmbAssembly.SelectedItem.Text
                lblAssembly1.Text = "Assembly Name : " & "<b>" + Assembly1 + "</b>"  'Added Code
            End If
        End If
        Average = txtAvgMnths.Text
        If Not IsDate(txtFromDate.Text.Trim) Then
            AsonDate = ""
        Else
            AsonDate = txtFromDate.Text.Trim
        End If
        ' Aircraft = IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "")
        Aircraft = cmbAircraft.SelectedItem.Text ' IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "")

        '' TypeName = IIf(cmbType.SelectedIndex > 0, cmbType.SelectedItem.Text, "")

        If AsonDate <> "" Then
            lblDateRangeFrom.Text = "As On Date : " & "<b>" + txtFromDate.Text.Trim + "</b>"
        Else
            lblDateRangeFrom.Text = "As On Date : " & "All"
        End If

        lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", "<b>" + Aircraft + "</b>", "All")
        lblAvgMnths1.Text = "Average Months : " & IIf(Average <> "", Average, "All")
        percent = txtPercentage.Text
        lblPercent.Text = "Percent : " & IIf(percent <> "", percent, "All")
        ''lblType1.Text = "Type : " & IIf(TypeName <> "", TypeName, "All")

        'Set Service/Inspection/Directive checkbox list values
        'Service
        If chkService.Checked Then
            IsSerSelect = True
            ServiceTypeID = (From c As ListItem In ListServiceType.Items
                             Where c.Selected = True
                             Select CInt(c.Value)).ToArray
        End If
        'Inspection
        If chkInspection.Checked Then
            IsInsSelect = True

            InspectionTypeID = (From c In ListInspectionType.Items
                                Where c.Selected = True
                                Select CInt(c.Value)).ToArray
        End If
        'Directive
        If chkDirective.Checked Then
            IsModSelect = True
            ModificationTypeID = (From c In ListDirectiveType.Items
                                  Where c.Selected = True
                                  Select CInt(c.Value)).ToArray
        End If
        'End

        'If cmbType.Items.Item(x).ToString = "All" Then
        '    IsSerSelect = True
        '    IsInsSelect = True
        '    IsModSelect = True
        '    ServiceTypeID(0) = 0
        '    InspectionTypeID(0) = 0
        '    ModificationTypeID(0) = 0
        'End If
        Dim DueLimits As String = String.Empty
        Dim EstimatedFlyingHours As String = String.Empty
        Dim status As String = String.Empty
        Dim Format As String = String.Empty
        'Due Limits
        status = IIf(rbdDueLimits.Checked, rbdDueLimits.Text, rbdPercent.Text)
        If rbdDueLimits.Checked Then
            DueLimits = status & " : " & String.Join(", ", (From c As DueLimit In mDueLimits
                                                            Select c.PeriodName & " : " & c.PeriodLimitFormatted).ToArray)
        Else
            DueLimits = status & " : " & txtPercentage.Text.Trim
        End If
        'Estimated Flying Hours
        status = IIf(rbdAvrageMonths.Checked, rbdAvrageMonths.Text, rbdSpecifyValues.Text)
        If rbdSpecifyValues.Checked Then
            EstimatedFlyingHours = status & " : " & String.Join(", ", (From c As PerDayLimit In mPerDayLimits
                                                                       Select c.PeriodName & " : " & c.PeriodLimitFormatted).ToArray)
        Else
            EstimatedFlyingHours = status & " : " & txtAvgMnths.Text.Trim
        End If
        Format = IIf(chkwithWONo.Checked, cmbFormat.SelectedItem.Text & " : " & chkwithWONo.Text, cmbFormat.SelectedItem.Text)
        mEventLogDetails = lblDateRangeFrom.Text + ", " + lblAircraft1.Text + ", " + lblAssembly1.Text + ", " + DueLimits + ", " + EstimatedFlyingHours + ", Format : " + Format
    End Sub

    Private Sub ResetValues()
        MachineName = "{00000000-0000-0000-0000-000000000000}"
        'CNDC
        'txtFromDate.Value = AsonDate
        If AsonDate <> "" Then
            txtFromDate.Text = Format(AsonDate, AppSettings("DateFormat"))
        End If
        AsonDate = ""
        AvgMnths = 0

        IsSerSelect = False
        IsInsSelect = False
        IsModSelect = False
        ServiceTypeID(0) = 0
        InspectionTypeID(0) = 0
        ModificationTypeID(0) = 0
        AssemblyName = "{00000000-0000-0000-0000-000000000000}"
        btnDisplay.Enabled = True
    End Sub

    Public Function ReportDetail(IsExcel As Boolean, Optional IsPreviewClicked As Boolean = False) As ReportMaintenanceDetailList

        Try

            Dim ObjMachine As MachineInfo
            Dim ObjAssemblyStatus As AssemblyStatusInfo
            Dim ObjCompStatus As CompStatusInfo
            Dim ObjAssemblyMonitorInspStatus As AssemblyMonitorInspStatusInfo
            Dim ObjAssemblyMonitorInspStatusPeriod As AssemblyMonitorInspStatusPeriodInfo
            Dim ObjAssemblyMonitorModStatus As AssemblyMonitorModStatusInfo
            Dim ObjAssemblyMonitorModStatusPeriod As AssemblyMonitorModStatusPeriodInfo
            Dim ObjAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatusInfo   '''''''''''''''''''''''''''''''''''''''''
            Dim ObjAssemblyMonitorServiceStatusPeriod As AssemblyMonitorServiceStatusPeriodInfo
            Dim ObjCompMonitorInspStatus As CompMonitorInspStatusInfo
            Dim ObjCompMonitorInspStatusPeriod As CompMonitorInspStatusPeriodInfo
            Dim ObjCompMonitorModStatus As CompMonitorModStatusInfo
            Dim ObjCompMonitorModStatusPeriod As CompMonitorModStatusPeriodInfo
            Dim ObjCompMonitorServiceStatus As CompMonitorServiceStatusInfo
            Dim ObjCompMonitorServiceStatusPeriod As CompMonitorServiceStatusPeriodInfo

            If rbdPercent.Checked Then mDueLimits.SetPercentageWise(True, CDec(Val(txtPercentage.Text)))

            mMachineList = MachineList.GetMachineListDueMonitoringStatus(CurrentDate:=AsonDate,
                                                                         DueLimits:=mDueLimits,
                                                                         MachineID:=MachineName,
                                                                         AssemblyID:=AssemblyName,
                                                                         AverageMonths:=Val(txtAvgMnths.Text),
                                                                         ByPerDayLimit:=rbdSpecifyValues.Checked,
                                                                         PerdayLimits:=mPerDayLimits,
                                                                         MonitoringServiceRequired:=IIf(chkAssembly.Checked And IsSerSelect, True, False),
                                                                         MonitoringInspRequired:=IIf(chkAssembly.Checked And IsInsSelect, True, False),
                                                                         MonitoringModRequired:=IIf(chkAssembly.Checked And IsModSelect, True, False),
                                                                         CompMonitoringServiceRequired:=IIf(chkComponent.Checked And IsSerSelect, True, False),
                                                                         CompMonitoringInspRequired:=IIf(chkComponent.Checked And IsInsSelect, True, False),
                                                                         CompMonitoringModRequired:=IIf(chkComponent.Checked And IsModSelect, True, False),
                                                                         ForDueStatus:=Val(txtForecastingLimit.Text),
                                                                         IsForDueReport:=True,
                                                                         SkipIsForInventoryAircarft:=True)
            Dim LHLabel2 As String = ""
            Dim LHData2 As String = ""

            If Not cmbAircraft.SelectedItem.ToString = "(ALL)" Or
               AppSettings("ClientCode") = "APFT" Or
               AppSettings("ClientCode") = "Novo" Or
               AppSettings("ClientCode") = "AAP" Then ' Added by Saylee on 20-Aug-2018 for ALL20082018,common report to show Current values

                For Each ObjMachine In mMachineList

                    CustomerName = ObjMachine.CustomerName

                    For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList

                        Periodcount = ObjAssemblyStatus.AssemblyStatusPeriodList.Count()
                        LHLabel2 = ""
                        LHData2 = ""

                        For Count = 0 To Periodcount - 1

                            If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID <> 2 Then

                                LHLabel2 = CType(IIf(LHLabel2 = "", LHLabel2, LHLabel2 + vbNewLine), String) +
                                           ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName

                                LHData2 = CType(IIf(LHData2 = "", LHData2, LHData2 + vbNewLine), String) +
                                           ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyCurrentValue
                            End If

                        Next

                        AssemblyID = ObjAssemblyStatus.AssemblyID

                    Next

                Next

            End If

            If Not cmbAircraft.SelectedItem.ToString = "(ALL)" Or
               AppSettings("ClientCode") = "APFT" Or
               AppSettings("ClientCode") = "Novo" Or
               AppSettings("ClientCode") = "AAP" Then  ' Added by Saylee on 20-Aug-2018 for ALL20082018,common report to show Current values

                Dim tmpAircraftList As AircraftCurrentStatusList = AircraftCurrentStatusList.GetAircraftDailyStatusMachineList(RegNo:=Aircraft,
                                                                                                                               CurrentDate:=AsonDate,
                                                                                                                               ShowValuesOnTopOfReport:=True)

                ' mtmpMachineList = tmpMachineList.GetMachineList(, Aircraft, , , , , True, AsonDate)
                Dim mOtherPeriodExists As String = "False"

                'For i As Integer = 0 To mtmpMachineList.Count - 1

                '    If mtmpMachineList(i).AllPeriods <> "" Then

                '        mOtherPeriodExists = "True"
                '        Exit For

                '    End If

                'Next
                For i As Integer = 0 To tmpAircraftList.Count - 1

                    If tmpAircraftList(i).AllPeriods <> "" Then

                        mOtherPeriodExists = "True"
                        Exit For

                    End If

                Next

                'For i As Integer = 0 To mtmpMachineList.Count - 1

                '    searchstr7 = mtmpMachineList(i).Owner.ToString  ' Changed By Utkarsh On 11-Apr-2011 '"Owner/Operator :- " +
                '    ReportStatusList.Add(New rptStatus(mtmpMachineList(i).ID.ToString,
                '                                       1, , , , ,
                '                                       mtmpMachineList(i).TSO, ,
                '                                       mtmpMachineList(i).CSO, , , , , , , , ,
                '                                       mtmpMachineList(i).Cycles,
                '                                       mtmpMachineList(i).AllPeriods.Replace("<BR>", vbCrLf),
                '                                       mOtherPeriodExists,
                '                                       Year(txtFromDate.Text).ToString, ,
                '                                       mtmpMachineList(i).RegNo,
                '                                       mtmpMachineList(i).ModelName,
                '                                       mtmpMachineList(i).Type,
                '                                       mtmpMachineList(i).SerialNo,
                '                                       mtmpMachineList(i).ManufacturerName, ,
                '                                       mtmpMachineList(i).ManufacturingDate,
                '                                       mtmpMachineList(i).Hours,
                '                                       mtmpMachineList(i).Landings))

                '    Session("AircraftAsOnDate") = mtmpMachineList(0).ManufacturingDateFormatted

                'Next
                For i As Integer = 0 To tmpAircraftList.Count - 1

                    searchstr7 = tmpAircraftList(i).Owner.ToString  ' Changed By Utkarsh On 11-Apr-2011 '"Owner/Operator :- " +
                    ReportStatusList.Add(New rptStatus(tmpAircraftList(i).ID.ToString,
                                                       1, , , , ,
                                                       tmpAircraftList(i).TSO, ,
                                                       tmpAircraftList(i).CSO, , , , , , , , ,
                                                       tmpAircraftList(i).Cycles,
                                                       tmpAircraftList(i).AllPeriods.Replace("<BR>", vbCrLf),
                                                       mOtherPeriodExists,
                                                       Year(txtFromDate.Text).ToString, ,
                                                       tmpAircraftList(i).RegNo,
                                                       tmpAircraftList(i).ModelName,
                                                       tmpAircraftList(i).Type,
                                                       tmpAircraftList(i).SerialNo,
                                                       tmpAircraftList(i).ManufacturerName, ,
                                                       tmpAircraftList(i).ManufacturingDate,
                                                       tmpAircraftList(i).Hours,
                                                       tmpAircraftList(i).Landings))

                    Session("AircraftAsOnDate") = tmpAircraftList(0).ManufacturingDateFormatted

                Next
            End If


            For Each ObjMachine In mMachineList

                For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList

                    'Services
                    If IsSerSelect = True Then

                        If chkAssembly.Checked Then

                            For Each ObjAssemblyMonitorServiceStatus In ObjAssemblyStatus.AssemblyMonitorServiceStatusList

                                'loop through selected monitory types
                                If ServiceTypeID.Contains(ObjAssemblyMonitorServiceStatus.ModelMonitorServiceTypeID) Then

                                    If ObjAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriodList.Count > 0 Then

                                        If (ObjAssemblyMonitorServiceStatus.IsApplicable = True) And
                                           (Not (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True)) Then

                                            ATAChapter = ObjAssemblyMonitorServiceStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorServiceStatus.ATANomenclature
											'Description = ObjAssemblyMonitorServiceStatus.Description
											Description = IIf(Expression:=AppSettings(name:="ClientCode") = "SHN", TruePart:=IIf(Expression:=ObjAssemblyMonitorServiceStatus.ModelMonitorServiceCode <> "", TruePart:=IIf(IsExcel, TruePart:="Task No.:" + ObjAssemblyMonitorServiceStatus.ModelMonitorServiceCode & Chr(10), FalsePart:="<b>Task No.:" + ObjAssemblyMonitorServiceStatus.ModelMonitorServiceCode & "</b><BR>"), FalsePart:=""), FalsePart:="") + ObjAssemblyMonitorServiceStatus.Description
											AssemblyModel = ObjAssemblyStatus.Model
                                            AssemblySerialNo = ObjAssemblyStatus.SerialNo & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyStatus.Position
                                            Position = ""
                                            MonitorTypeCode = ObjAssemblyMonitorServiceStatus.Code
                                            EstimatedDate = ObjAssemblyMonitorServiceStatus.EstimatedDateFormatted
                                            MinimumRemainingValue = ObjAssemblyMonitorServiceStatus.MinimumRemainingValue
                                            AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID
                                            StatusMasterID = ObjAssemblyMonitorServiceStatus.ModelMonitorServiceID  '11-Sep-2008
                                            DueStatus = ObjAssemblyMonitorServiceStatus.DueStatus
                                            DocumentTypeForID = 0
                                            Remark = ObjAssemblyStatus.InstallationRemark + " " + ObjAssemblyMonitorServiceStatus.DoneRemark
                                            Code = ObjAssemblyMonitorServiceStatus.ModelMonitorServiceCode  'Added By Saylee on 28-08-2008
                                            DoneOnDate = ObjAssemblyMonitorServiceStatus.DoneOn  'Added By Saylee 2-Aug-2012
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
                                            AssemblyDueAsof = "" 'Added By DEVEN On 14/06/2008
                                            AssemblyDueAsof1 = "" 'Added By DEVEN On 14/06/2008
                                            AssemblyDueAsof2 = "" 'Added By DEVEN On 14/06/2008
                                            SinceNew = ""
                                            SinceNew1 = ""
                                            SinceNew2 = ""
                                            DoneAt = ""
                                            DoneAt1 = ""
                                            DoneAt2 = ""
                                            MaintenanceEvent = ""
                                            'Added by Saylee 04-08-2008
                                            Extension = ""
                                            Extension1 = ""
                                            Extension2 = ""

                                            For Each ObjAssemblyMonitorServiceStatusPeriod In ObjAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriodList
                                                If Report = 1 Then  'Portarait
                                                    If ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 1 Then
                                                        Freq1 = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue
                                                        ElapsedTime = ObjAssemblyMonitorServiceStatusPeriod.ElapsedValue
                                                        RemainingTime = ObjAssemblyMonitorServiceStatusPeriod.RemainingValue
                                                        DueAsof = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
                                                        'Added By Shweta 7-June-2012
                                                        'AssemblyDueAsof = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
                                                        DoneAt = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
                                                        If ((Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL")) Then  'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
                                                            AssemblyDueAsof = ObjAssemblyMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                            If DoneOnDate <> "" Then DoneAt = ObjAssemblyMonitorServiceStatusPeriod.AssemblyDoneOnValueTextByAirFrame 'Added By Saylee 2-Aug-2012
                                                        ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Added By Prashant 26-Jun-2013 BA26062013
                                                            AssemblyDueAsof = ObjAssemblyMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                        Else
                                                            AssemblyDueAsof = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
                                                        End If
                                                        '**********************************
                                                        SinceNew = ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValue

                                                        'Added by Saylee 04-08-2008
                                                        Extension = ObjAssemblyMonitorServiceStatusPeriod.ExtensionValue
                                                    End If
                                                    If ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 2 Then
                                                        Freq2 = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValueFormatted
                                                        ElapsedTime1 = ObjAssemblyMonitorServiceStatusPeriod.ElapsedValueFormatted
                                                        RemainingTime1 = ObjAssemblyMonitorServiceStatusPeriod.RemainingValueFormatted
                                                        DueAsof1 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted
                                                        AssemblyDueAsof1 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted  'Added By DEVEN On 14/06/2008
                                                        SinceNew1 = ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValueFormatted
                                                        DoneAt1 = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                        'Added by Saylee 04-08-2008
                                                        Extension1 = ObjAssemblyMonitorServiceStatusPeriod.ExtensionValueFormatted
                                                    End If
													'Added PeriodID=11 By Vikrant For ALL 21062012
													'If ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 3 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 4 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 5 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 6 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 7 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 8 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 9 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 12 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 13 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 14 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 15 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 11 Then
													'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
													If ObjAssemblyMonitorServiceStatusPeriod.PeriodID >= 3 Then
														If Freq3 = "" Then
															Freq3 = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue
															ElapsedTime2 = ObjAssemblyMonitorServiceStatusPeriod.ElapsedValue
															RemainingTime2 = ObjAssemblyMonitorServiceStatusPeriod.RemainingValue
															DueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
															'Added By Prashant 26-Jun-2013 BA26062013
															If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
																AssemblyDueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
															Else
																AssemblyDueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue  'Added By DEVEN On 14/06/2008
															End If
															SinceNew2 = ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValue
															DoneAt2 = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
															'Added by Saylee 04-08-2008
															Extension2 = ObjAssemblyMonitorServiceStatusPeriod.ExtensionValue
														Else
															Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue
															ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.ElapsedValue
															RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.RemainingValue
															DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
															'Added By Prashant 26-Jun-2013 BA26062013
															If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
																AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
															Else
																AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
															End If

															SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValue
															DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
															'Added by Saylee 04-08-2008
															Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.ExtensionValue
														End If
													End If
												End If
                                            Next

                                            AssemblyID = ObjAssemblyStatus.AssemblyID
                                            Note = ObjAssemblyMonitorServiceStatus.Notes
                                            RegNo = ObjMachine.RegNo

                                            'Rajnish 08-08-2008
                                            If IsPreviewClicked Then
                                                RequiredManHours = ModelMonitorService.GetModelMonitorService(ObjAssemblyMonitorServiceStatus.ModelMonitorServiceID).RequiredManHours
                                            Else
                                                RequiredManHours = ObjAssemblyMonitorServiceStatus.RequiredManHours
                                            End If

                                            Customer = ObjMachine.Customer
                                            AssemblyType = ObjAssemblyStatus.AssemblyType

                                            Dim TaskNo As String = ""
                                            Dim TaskNoMaint As String = ""

                                            If AppSettings("ShowMaintenanceForNewClients") = True And ObjAssemblyMonitorServiceStatus.TaskNo <> "" Then

                                                TaskNoMaint = IIf(IsExcel, Chr(10), vbCrLf) & "Task No. : " & ObjAssemblyMonitorServiceStatus.TaskNo
                                                TaskNo = ObjAssemblyMonitorServiceStatus.TaskNo

                                            End If
                                            If AppSettings("ClientCode") = "FIT" Then
                                                TaskNoMaint = ""
                                            End If

                                            Dim MonitorTypeName As String = ObjAssemblyMonitorServiceStatus.Type & " (" & ObjAssemblyMonitorServiceStatus.MonitorType & ")"

                                            If AppSettings("ClientCode") = "7AR" Then
                                                MonitorTypeName = ObjAssemblyMonitorServiceStatus.Type
                                            End If

                                            If ObjAssemblyMonitorServiceStatus.Reference <> "" Then
                                                MaintenanceEvent = MonitorTypeName & TaskNoMaint & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatus.Reference & IIf(AppSettings("ClientCode") = "KamAir" Or AppSettings("ClientCode") = "MEL", IIf(ObjAssemblyMonitorServiceStatus.ModelMonitorServiceCode <> "", IIf(IsExcel, Chr(10), vbCrLf) & " (" & ObjAssemblyMonitorServiceStatus.ModelMonitorServiceCode & ")", ""), "")
                                            Else
                                                MaintenanceEvent = MonitorTypeName & TaskNoMaint & IIf(AppSettings("ClientCode") = "KamAir" Or AppSettings("ClientCode") = "MEL", IIf(ObjAssemblyMonitorServiceStatus.ModelMonitorServiceCode <> "", IIf(IsExcel, Chr(10), vbCrLf) & " (" & ObjAssemblyMonitorServiceStatus.ModelMonitorServiceCode & ")", ""), "")
                                            End If

                                            'Added by Saylee 04-08-2008
                                            ExtensionDate = ObjAssemblyMonitorServiceStatus.ExtensionDate
                                            ApprovalRemark = ObjAssemblyMonitorServiceStatus.ApprovalRemark
                                            StatusID = ObjAssemblyMonitorServiceStatus.ID  'Added by Saylee on 6-May-2013 for ALL06052013-1

                                            If chkwithWONo.Checked = True Or IsPreviewClicked Then 'Added by Saylee on 6-May-2013 for ALL06052013-1

                                                mnWOListForDueJobs = nWOListForDueJobs.GetWOListForDueJobs(StatusID)
                                                If mnWOListForDueJobs.Count > 0 Then

                                                    nWONumber = mnWOListForDueJobs(0).WONumber + vbCrLf + mnWOListForDueJobs(0).WODateFormatted
                                                    'REQ
                                                    mRequisitionListNew = RequisitionListNew.GetRequisitionList(WOID:=mnWOListForDueJobs(0).ID.ToString)
                                                    For i As Integer = 0 To mRequisitionListNew.Count - 1
                                                        ReqNumber.Append(mRequisitionListNew(i).RequisitionTextNo + ", ")
                                                    Next
                                                    'End

                                                Else
                                                    nWONumber = ""
                                                    ReqNumber.Clear()
                                                End If

                                            End If

                                            Zone = ObjAssemblyMonitorServiceStatus.Zone
                                            Area = ObjAssemblyMonitorServiceStatus.Area
                                            IsRII = ObjAssemblyMonitorServiceStatus.IsRII

                                            'Added by Saylee on 21-Sep-2018 , to show "----" in Freq & DoneAt for Expiry type

                                            If (AppSettings("ClientCode") = "APFT" Or
                                                AppSettings("ClientCode") = "AAP") And
                                               ObjAssemblyMonitorServiceStatus.ModelMonitorServiceTypeID = 5 Then '(Expiry Service)

                                                DoneAt = "----"
                                                DoneAt1 = ""
                                                DoneAt2 = ""
                                                Freq1 = "----"
                                                Freq2 = ""
                                                Freq3 = ""

                                            End If

                                            '*******************************************************************************
                                            LinkedMaintenanceActivityCount = ObjAssemblyMonitorServiceStatus.LinkedMaintenanceActivityCount   'Added by Prashant  9-Sep-2020 ALL09092020

                                            ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID,
                                                                                                     RegNo,
                                                                                                     AssemblyType, ,
                                                                                                     AssemblySerialNo,
                                                                                                     ATAChapter, , ,
                                                                                                     Position,
                                                                                                     ObjAssemblyMonitorServiceStatus.MonitorType,
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
                                                                                                     AssemblyModel,
                                                                                                     SinceNew,
                                                                                                     SinceNew1,
                                                                                                     SinceNew2,
                                                                                                     DoneAt,
                                                                                                     DoneAt1,
                                                                                                     DoneAt2,
                                                                                                     MinimumRemainingValue,
                                                                                                     AssemblyTypeID,
                                                                                                     MaintenanceEvent, , , , , , , , , , , , , , ,
                                                                                                     ObjAssemblyMonitorServiceStatus.Reference, ,
                                                                                                     DoneOnDate, , , ,
                                                                                                     AssemblyDueAsof,
                                                                                                     AssemblyDueAsof1,
                                                                                                      AssemblyDueAsof2,
                                                                                                      Extension,
                                                                                                      Extension1,
                                                                                                      Extension2,
                                                                                                      ExtensionDate,
                                                                                                      ApprovalRemark,
                                                                                                      RequiredManHours,
                                                                                                      Customer,
                                                                                                      Code,
                                                                                                      StatusMasterID.ToString,
                                                                                                      DocumentTypeForID, , ,
                                                                                                      ObjAssemblyMonitorServiceStatus.IsApplicable,
                                                                                                      StatusID.ToString,
                                                                                                      AssemblyStatusID:=ObjAssemblyStatus.ID.ToString,
                                                                                                      DueStatus:=DueStatus,
                                                                                                      WONumber:=nWONumber,
                                                                                                      MachineID:=ObjMachine.MachineID.ToString,
                                                                                                      ModelID:=ObjAssemblyStatus.ModelID.ToString,
                                                                                                      MaintenanceTypeID:=5,
                                                                                                      IsMaster:=ObjAssemblyMonitorServiceStatus.IsMaster,
                                                                                                      Zone:=Zone,
                                                                                                      Area:=Area,
                                                                                                      IsRII:=IsRII,
                                                                                                      ReqNumber:=ReqNumber.ToString.Trim.TrimEnd(","),
                                                                                                      LinkedMaintenanceActivityCount:=LinkedMaintenanceActivityCount,
                                                                                                      TaskNo:=TaskNo,
                                                                                                      SourceDoc:=ObjAssemblyMonitorServiceStatus.Source))

                                        End If

                                    End If

                                End If

                            Next

                        End If

                        If chkComponent.Checked Then

                            For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList

                                For Each ObjCompMonitorServiceStatus In ObjCompStatus.CompMonitorServiceStatusList

                                    If ServiceTypeID.Contains(ObjCompMonitorServiceStatus.PartMonitorServiceTypeID) Then

                                        If ObjCompMonitorServiceStatus.CompMonitorServiceStatusPeriodList.Count > 0 Then

                                            If (ObjCompMonitorServiceStatus.IsApplicable = True) And (Not (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True)) Then

                                                ATAChapter = ObjCompMonitorServiceStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorServiceStatus.ATANomenclature
												'Description = ObjCompMonitorServiceStatus.Description
												Description = IIf(Expression:=AppSettings(name:="ClientCode") = "SHN", TruePart:=IIf(Expression:=ObjCompMonitorServiceStatus.PartMonitorServiceCode <> "", TruePart:=IIf(IsExcel, TruePart:="Task No.:" + ObjCompMonitorServiceStatus.PartMonitorServiceCode & Chr(10), FalsePart:="<b>Task No.:" + ObjCompMonitorServiceStatus.PartMonitorServiceCode & "</b><BR>"), FalsePart:=""), FalsePart:="") + ObjCompMonitorServiceStatus.Description
												PartNo = ObjCompStatus.PartName
                                                CompSerialNo = ObjCompStatus.CompSerialNo
                                                Position = ObjCompStatus.Position
                                                MonitorTypeCode = ObjCompMonitorServiceStatus.Code
                                                EstimatedDate = ObjCompMonitorServiceStatus.EstimatedDateFormatted
                                                AssemblyModel = ObjAssemblyStatus.Model
                                                AssemblySerialNo = ObjAssemblyStatus.SerialNo & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyStatus.Position
                                                MinimumRemainingValue = ObjCompMonitorServiceStatus.MinimumRemainingValue
                                                AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID
                                                StatusMasterID = ObjCompMonitorServiceStatus.PartMonitorServiceID  '11-Sep-2008
                                                DueStatus = ObjCompMonitorServiceStatus.DueStatus
                                                DocumentTypeForID = 0
                                                'Remark = ObjCompMonitorServiceStatus.DoneRemark  'Added By Saylee on 20-08-2008
                                                Remark = ObjCompStatus.InstallationRemark + " " + ObjCompMonitorServiceStatus.DoneRemark  'Added By Saylee on 20-08-2008
                                                Code = ObjCompMonitorServiceStatus.PartMonitorServiceCode
                                                DoneOnDate = ObjCompMonitorServiceStatus.DoneOn  'Added By Saylee 2-Aug-2012
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

                                                AssemblyDueAsof = "" 'Added By DEVEN On 14/06/2008
                                                AssemblyDueAsof1 = "" 'Added By DEVEN On 14/06/2008
                                                AssemblyDueAsof2 = "" 'Added By DEVEN On 14/06/2008

                                                SinceNew = ""
                                                SinceNew1 = ""
                                                SinceNew2 = ""
                                                DoneAt = ""
                                                DoneAt1 = ""
                                                DoneAt2 = ""
                                                MaintenanceEvent = ""

                                                'Added by Saylee 04-08-2008
                                                Extension = ""
                                                Extension1 = ""
                                                Extension2 = ""

                                                For Each ObjCompMonitorServiceStatusPeriod In ObjCompMonitorServiceStatus.CompMonitorServiceStatusPeriodList
                                                    If Report = 1 Then 'Portarait
                                                        If ObjCompMonitorServiceStatusPeriod.PeriodID = 1 Then
                                                            Freq1 = ObjCompMonitorServiceStatusPeriod.FrequencyValue
                                                            ElapsedTime = ObjCompMonitorServiceStatusPeriod.ElapsedValue
                                                            RemainingTime = ObjCompMonitorServiceStatusPeriod.RemainingValue
                                                            'AssemblyDueAsof = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
                                                            'Added By Shweta 7-June-2012
                                                            DoneAt = ObjCompMonitorServiceStatusPeriod.DoneOnValue
                                                            If ((Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL")) Then  'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
                                                                AssemblyDueAsof = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                                If DoneOnDate <> "" Then DoneAt = ObjCompMonitorServiceStatusPeriod.AssemblyDoneOnValueTextByAirFrame 'Added By Saylee 2-Aug-2012
                                                            ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Added By Prashant 26-Jun-2013 BA26062013
                                                                AssemblyDueAsof = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                            Else
                                                                AssemblyDueAsof = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText
                                                            End If
                                                            '**********************************
                                                            DueAsof = ObjCompMonitorServiceStatusPeriod.DueOnValue
                                                            SinceNew = ObjCompMonitorServiceStatusPeriod.CompCurrentValue

                                                            'Added by Saylee 04-08-2008
                                                            Extension = ObjCompMonitorServiceStatusPeriod.ExtensionValue
                                                        End If
                                                        If ObjCompMonitorServiceStatusPeriod.PeriodID = 2 Then
                                                            Freq2 = ObjCompMonitorServiceStatusPeriod.FrequencyValueFormatted
                                                            ElapsedTime1 = ObjCompMonitorServiceStatusPeriod.ElapsedValueFormatted
                                                            RemainingTime1 = ObjCompMonitorServiceStatusPeriod.RemainingValueFormatted
                                                            AssemblyDueAsof1 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted 'Added By DEVEN On 14/06/2008
                                                            DueAsof1 = ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                            SinceNew1 = ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted
                                                            DoneAt1 = ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                            'Added by Saylee 04-08-2008
                                                            Extension1 = ObjCompMonitorServiceStatusPeriod.ExtensionValueFormatted
                                                        End If
														'Added PeriodID=11 By Vikrant For ALL 21062012
														'If ObjCompMonitorServiceStatusPeriod.PeriodID = 3 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 4 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 5 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 6 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 7 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 8 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 12 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 13 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 14 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 15 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 11 Then
														'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
														If ObjCompMonitorServiceStatusPeriod.PeriodID >= 3 Then
															If Freq3 = "" Then
																Freq3 = ObjCompMonitorServiceStatusPeriod.FrequencyValue
																ElapsedTime2 = ObjCompMonitorServiceStatusPeriod.ElapsedValue
																RemainingTime2 = ObjCompMonitorServiceStatusPeriod.RemainingValue
																'Added by Saylee on 11-Mar-2013 for ALL11032013 - 1
																'AssemblyDueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
																If ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Then
																	AssemblyDueAsof2 = "" 'ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
																Else
																	'Added By Prashant 26-Jun-2013 BA26062013
																	If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
																		AssemblyDueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
																	Else
																		AssemblyDueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
																	End If
																End If
																'***************
																DueAsof2 = ObjCompMonitorServiceStatusPeriod.DueOnValue
																SinceNew2 = ObjCompMonitorServiceStatusPeriod.CompCurrentValue
																DoneAt2 = ObjCompMonitorServiceStatusPeriod.DoneOnValue
																'Added by Saylee 04-08-2008
																Extension2 = ObjCompMonitorServiceStatusPeriod.ExtensionValue
															Else
																Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.FrequencyValue
																ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedValue
																RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.RemainingValue
																'Added by Saylee on 11-Mar-2013 for ALL11032013 - 1
																'AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
																If ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Then
																	AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ""  'AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
																Else
																	'Added By Prashant 26-Jun-2013 BA26062013
																	If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
																		AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
																	Else
																		AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
																	End If
																End If
																'****************************
																DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
																SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.CompCurrentValue
																DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DoneOnValue
																'Added by Saylee 04-08-2008
																Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ExtensionValue
															End If
														End If
													End If
                                                Next
                                                AssemblyID = ObjAssemblyStatus.AssemblyID
                                                AssemblyType = ObjAssemblyStatus.AssemblyType
                                                RegNo = ObjMachine.RegNo
                                                'Rajnish 08-08-2008
                                                If IsPreviewClicked Then
                                                    RequiredManHours = PartMonitorService.GetPartMonitorService(ObjCompMonitorServiceStatus.PartMonitorServiceID).RequiredManHours
                                                Else
                                                    RequiredManHours = ObjCompMonitorServiceStatus.RequiredManHours
                                                End If
                                                Customer = ObjMachine.Customer
                                                Note = ObjCompMonitorServiceStatus.Notes
                                                'Commented and Added by Saylee on 10-Oct-2013 for ALL10102013
                                                'MaintenanceEvent = ObjCompMonitorServiceStatus.Type
                                                Dim TaskNo As String = ""
                                                Dim TaskNoMaint As String = ""
                                                If AppSettings("ShowMaintenanceForNewClients") = True And ObjCompMonitorServiceStatus.TaskNo <> "" Then
                                                    TaskNoMaint = IIf(IsExcel, Chr(10), vbCrLf) & "Task No. : " & ObjCompMonitorServiceStatus.TaskNo
                                                    TaskNo = ObjCompMonitorServiceStatus.TaskNo
                                                End If

                                                If AppSettings("ClientCode") = "FIT" Then
                                                    TaskNoMaint = ""
                                                End If
                                                Dim MonitorTypeName As String = ObjCompMonitorServiceStatus.Type & " (" & ObjCompMonitorServiceStatus.MonitorType & ")"

                                                If AppSettings("ClientCode") = "7AR" Then
                                                    MonitorTypeName = ObjCompMonitorServiceStatus.Type
                                                End If

                                                If ObjCompMonitorServiceStatus.Reference <> "" Then
                                                    MaintenanceEvent = MonitorTypeName & TaskNoMaint & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatus.Reference & IIf(AppSettings("ClientCode") = "KamAir" Or AppSettings("ClientCode") = "MEL", IIf(ObjCompMonitorServiceStatus.PartMonitorServiceCode <> "", IIf(IsExcel, Chr(10), vbCrLf) & " (" & ObjCompMonitorServiceStatus.PartMonitorServiceCode & ")", ""), "")
                                                Else
                                                    MaintenanceEvent = MonitorTypeName & TaskNoMaint & IIf(AppSettings("ClientCode") = "KamAir" Or AppSettings("ClientCode") = "MEL", IIf(ObjCompMonitorServiceStatus.PartMonitorServiceCode <> "", IIf(IsExcel, Chr(10), vbCrLf) & " (" & ObjCompMonitorServiceStatus.PartMonitorServiceCode & ")", ""), "")
                                                End If

                                                'Added by Saylee 04-08-2008
                                                ExtensionDate = ObjCompMonitorServiceStatus.ExtensionDate
                                                ApprovalRemark = ObjCompMonitorServiceStatus.ApprovalRemark

                                                StatusID = ObjCompMonitorServiceStatus.ID  'Added by Saylee on 6-May-2013 for ALL06052013-1

                                                If chkwithWONo.Checked = True Or IsPreviewClicked Then 'Added by Saylee on 6-May-2013 for ALL06052013-1
                                                    mnWOListForDueJobs = nWOListForDueJobs.GetWOListForDueJobs(StatusID)
                                                    If mnWOListForDueJobs.Count > 0 Then
                                                        nWONumber = mnWOListForDueJobs(0).WONumber + vbCrLf + mnWOListForDueJobs(0).WODateFormatted
                                                        'REQ
                                                        mRequisitionListNew = RequisitionListNew.GetRequisitionList(WOID:=mnWOListForDueJobs(0).ID.ToString)
                                                        For i As Integer = 0 To mRequisitionListNew.Count - 1
                                                            ReqNumber.Append(mRequisitionListNew(i).RequisitionTextNo + ", ")
                                                        Next
                                                        'End
                                                    Else
                                                        nWONumber = ""
                                                        ReqNumber.Clear()
                                                    End If
                                                End If

                                                Zone = ""
                                                Area = ""
                                                IsRII = False

                                                'Added by Saylee on 21-Sep-2018 , to show "----" in Freq & DoneAt for Expiry type
                                                If (AppSettings("ClientCode") = "APFT" Or
                                                    AppSettings("ClientCode") = "AAP") And
                                                   ObjCompMonitorServiceStatus.PartMonitorServiceTypeID = 5 Then '(Expiry Service)
                                                    DoneAt = "----"
                                                    DoneAt1 = ""
                                                    DoneAt2 = ""
                                                    Freq1 = "----"
                                                    Freq2 = ""
                                                    Freq3 = ""
                                                End If
                                                '*******************************************************************************

                                                'If ObjCompMonitorServiceStatus.CompMonitorServiceStatusPeriodList.Count > 0 Then
                                                ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, MaintenanceEvent, AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, ObjCompMonitorServiceStatus.MonitorType, MonitorTypeCode, Note, Remark, Description,
                                                                      , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue,
                                                                      AssemblyTypeID, MaintenanceEvent, , , , , , , , , , , , , , , ObjCompMonitorServiceStatus.Reference, , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark,
                                                                      RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , ObjCompMonitorServiceStatus.IsApplicable,
                                                                      StatusID.ToString, CompStatusID:=ObjCompStatus.ID.ToString, AssemblyStatusID:=ObjAssemblyStatus.ID.ToString, DueStatus:=DueStatus,
                                                                      WONumber:=nWONumber, MachineID:=ObjMachine.MachineID.ToString, ModelID:=ObjAssemblyStatus.ModelID.ToString, MaintenanceTypeID:=8,
                                                                      IsMaster:=ObjCompMonitorServiceStatus.IsMaster, Zone:=Zone, Area:=Area, IsRII:=IsRII, ReqNumber:=ReqNumber.ToString.Trim.TrimEnd(","),
                                                                      TaskNo:=TaskNo, SourceDoc:=ObjCompMonitorServiceStatus.Source))

                                            End If

                                        End If

                                    End If

                                Next

                            Next

                        End If

                    End If

                    'Inspection
                    If IsInsSelect = True Then

                        If chkAssembly.Checked Then

                            For Each ObjAssemblyMonitorInspStatus In ObjAssemblyStatus.AssemblyMonitorInspStatusList

                                If InspectionTypeID.Contains(ObjAssemblyMonitorInspStatus.ModelMonitorInspTypeID) Then

                                    If ObjAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriodList.Count > 0 Then

                                        If (ObjAssemblyMonitorInspStatus.IsApplicable = True) And (Not (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True)) Then

                                            ATAChapter = ObjAssemblyMonitorInspStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorInspStatus.ATANomenclature
											'Description = ObjAssemblyMonitorInspStatus.Description
											Description = IIf(Expression:=AppSettings(name:="ClientCode") = "SHN", TruePart:=IIf(Expression:=ObjAssemblyMonitorInspStatus.ModelMonitorInspCode <> "", TruePart:=IIf(IsExcel, TruePart:="Task No.:" + ObjAssemblyMonitorInspStatus.ModelMonitorInspCode & Chr(10), FalsePart:="<b>Task No.:" + ObjAssemblyMonitorInspStatus.ModelMonitorInspCode & "</b><BR>"), FalsePart:=""), FalsePart:="") + ObjAssemblyMonitorInspStatus.Description
											AssemblyModel = ObjAssemblyStatus.Model
                                            AssemblySerialNo = ObjAssemblyStatus.SerialNo & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyStatus.Position
                                            Position = ""
                                            MonitorTypeCode = ObjAssemblyMonitorInspStatus.Code
                                            EstimatedDate = ObjAssemblyMonitorInspStatus.EstimatedDateFormatted
                                            MinimumRemainingValue = ObjAssemblyMonitorInspStatus.MinimumRemainingValue
                                            AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID

                                            StatusMasterID = ObjAssemblyMonitorInspStatus.ModelMonitorInspID  '11-Sep-2008
                                            DueStatus = ObjAssemblyMonitorInspStatus.DueStatus
                                            DocumentTypeForID = 9
                                            DoneOnDate = ObjAssemblyMonitorInspStatus.DoneOn  'Added By Saylee 2-Aug-2012
                                            Code = ObjAssemblyMonitorInspStatus.ModelMonitorInspCode
                                            'Remark = ObjAssemblyMonitorInspStatus.DoneRemark  'Added By Saylee on 20-08-2008
                                            Remark = ObjAssemblyStatus.InstallationRemark + " " + ObjAssemblyMonitorInspStatus.DoneRemark  'Added By Saylee on 20-08-2008

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

                                            AssemblyDueAsof = "" 'Added By DEVEN On 14/06/2008
                                            AssemblyDueAsof1 = "" 'Added By DEVEN On 14/06/2008
                                            AssemblyDueAsof2 = "" 'Added By DEVEN On 14/06/2008

                                            SinceNew = ""
                                            SinceNew1 = ""
                                            SinceNew2 = ""
                                            DoneAt = ""
                                            DoneAt1 = ""
                                            DoneAt2 = ""

                                            'Added by Saylee 04-08-2008
                                            Extension = ""
                                            Extension1 = ""
                                            Extension2 = ""
                                            MaintenanceEvent = ""
                                            For Each ObjAssemblyMonitorInspStatusPeriod In ObjAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriodList
                                                If Report = 1 Then 'Portarait
                                                    If ObjAssemblyMonitorInspStatusPeriod.PeriodID = 1 Then
                                                        Freq1 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValue
                                                        ElapsedTime = ObjAssemblyMonitorInspStatusPeriod.ElapsedValue
                                                        RemainingTime = ObjAssemblyMonitorInspStatusPeriod.RemainingValue
                                                        DueAsof = ObjAssemblyMonitorInspStatusPeriod.DueOnValue
                                                        'AssemblyDueAsof = ObjAssemblyMonitorInspStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
                                                        'Added By Shweta 7-June-2012

                                                        DoneAt = ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
                                                        If ((Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL")) Then  'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
                                                            AssemblyDueAsof = ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                            If DoneOnDate <> "" Then DoneAt = ObjAssemblyMonitorInspStatusPeriod.AssemblyDoneOnValueTextByAirFrame 'Added By Saylee 2-Aug-2012
                                                        ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Added By Prashant 26-Jun-2013 BA26062013
                                                            AssemblyDueAsof = ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame

                                                        Else
                                                            AssemblyDueAsof = ObjAssemblyMonitorInspStatusPeriod.DueOnValue
                                                        End If
                                                        '**********************************

                                                        SinceNew = ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue

                                                        'Added by Saylee 04-08-2008
                                                        Extension = ObjAssemblyMonitorInspStatusPeriod.ExtensionValue
                                                    End If
                                                    If ObjAssemblyMonitorInspStatusPeriod.PeriodID = 2 Then
                                                        Freq2 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValueFormatted
                                                        ElapsedTime1 = ObjAssemblyMonitorInspStatusPeriod.ElapsedValueFormatted
                                                        RemainingTime1 = ObjAssemblyMonitorInspStatusPeriod.RemainingValueFormatted
                                                        DueAsof1 = ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
                                                        AssemblyDueAsof1 = ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted 'Added By DEVEN On 14/06/2008
                                                        SinceNew1 = ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValueFormatted
                                                        DoneAt1 = ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted
                                                        'Added by Saylee 04-08-2008
                                                        Extension1 = ObjAssemblyMonitorInspStatusPeriod.ExtensionValueFormatted
                                                    End If
													'Added PeriodID=11 By Vikrant For ALL 21062012
													'If ObjAssemblyMonitorInspStatusPeriod.PeriodID = 3 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 4 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 5 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 6 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 7 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 8 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 9 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 12 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 13 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 14 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 15 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 11 Then
													'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
													If ObjAssemblyMonitorInspStatusPeriod.PeriodID >= 3 Then
														If Freq3 = "" Then
															Freq3 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValue
															ElapsedTime2 = ObjAssemblyMonitorInspStatusPeriod.ElapsedValue
															RemainingTime2 = ObjAssemblyMonitorInspStatusPeriod.RemainingValue
															DueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValue
															'Added By Prashant 26-Jun-2013 BA26062013
															If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
																AssemblyDueAsof2 = ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame 'AssemblyDueOnValueByAirFrame
															Else
																AssemblyDueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
															End If

															SinceNew2 = ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue
															DoneAt2 = ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
															'Added by Saylee 04-08-2008
															Extension2 = ObjAssemblyMonitorInspStatusPeriod.ExtensionValue
														Else
															Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.FrequencyValue
															ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.ElapsedValue
															RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.RemainingValue
															DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.DueOnValue
															'Added By Prashant 26-Jun-2013 BA26062013
															If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
																AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueByAirFrame
															Else
																AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
															End If

															SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue
															DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
															'Added by Saylee 04-08-2008
															Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.ExtensionValue
														End If
													End If
												End If
                                            Next
                                            AssemblyID = ObjAssemblyStatus.AssemblyID
                                            AssemblyType = ObjAssemblyStatus.AssemblyType
                                            RegNo = ObjMachine.RegNo
                                            'Rajnish 08-08-2008
                                            If IsPreviewClicked Then
                                                RequiredManHours = ModelMonitorInsp.GetModelMonitorInsp(ObjAssemblyMonitorInspStatus.ModelMonitorInspID).RequiredManHours
                                            Else
                                                RequiredManHours = ObjAssemblyMonitorInspStatus.RequiredManHours
                                            End If
                                            Customer = ObjMachine.Customer
                                            Note = ObjAssemblyMonitorInspStatus.Notes
                                            'Commented and Added by Saylee on 10-Oct-2013 for ALL10102013
                                            'MaintenanceEvent = ObjAssemblyMonitorInspStatus.Type
                                            If ObjAssemblyMonitorInspStatus.Reference <> "" Then
                                                MaintenanceEvent = ObjAssemblyMonitorInspStatus.Type & " (" & ObjAssemblyMonitorInspStatus.MonitorType & ")" & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatus.Reference & IIf(AppSettings("ClientCode") = "KamAir" Or AppSettings("ClientCode") = "MEL", IIf(ObjAssemblyMonitorInspStatus.ModelMonitorInspCode <> "", IIf(IsExcel, Chr(10), vbCrLf) & " (" & ObjAssemblyMonitorInspStatus.ModelMonitorInspCode & ")", ""), "")
                                            Else
                                                MaintenanceEvent = ObjAssemblyMonitorInspStatus.Type & " (" & ObjAssemblyMonitorInspStatus.MonitorType & ")" & IIf(AppSettings("ClientCode") = "KamAir" Or AppSettings("ClientCode") = "MEL", IIf(ObjAssemblyMonitorInspStatus.ModelMonitorInspCode <> "", IIf(IsExcel, Chr(10), vbCrLf) & " (" & ObjAssemblyMonitorInspStatus.ModelMonitorInspCode & ")", ""), "")
                                            End If


                                            'Added by Saylee 04-08-2008
                                            ExtensionDate = ObjAssemblyMonitorInspStatus.ExtensionDate
                                            ApprovalRemark = ObjAssemblyMonitorInspStatus.ApprovalRemark

                                            StatusID = ObjAssemblyMonitorInspStatus.ID 'Added by Saylee on 6-May-2013 for ALL06052013-1

                                            If chkwithWONo.Checked = True Or IsPreviewClicked Then 'Added by Saylee on 6-May-2013 for ALL06052013-1
                                                mnWOListForDueJobs = nWOListForDueJobs.GetWOListForDueJobs(StatusID)
                                                If mnWOListForDueJobs.Count > 0 Then
                                                    nWONumber = mnWOListForDueJobs(0).WONumber + vbCrLf + mnWOListForDueJobs(0).WODateFormatted
                                                    'REQ
                                                    mRequisitionListNew = RequisitionListNew.GetRequisitionList(WOID:=mnWOListForDueJobs(0).ID.ToString)
                                                    For i As Integer = 0 To mRequisitionListNew.Count - 1
                                                        ReqNumber.Append(mRequisitionListNew(i).RequisitionTextNo + ", ")
                                                    Next
                                                    'End
                                                Else
                                                    nWONumber = ""
                                                    ReqNumber.Clear()
                                                End If
                                            End If

                                            Zone = ObjAssemblyMonitorInspStatus.Zone
                                            Area = ObjAssemblyMonitorInspStatus.Area
                                            IsRII = ObjAssemblyMonitorInspStatus.IsRII
                                            LinkedMaintenanceActivityCount = ObjAssemblyMonitorInspStatus.LinkedMaintenanceActivityCount   'Added by Prashant  9-Sep-2020 ALL09092020
                                            'If ObjAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriodList.Count > 0 Then
                                            ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, , AssemblySerialNo, ATAChapter, , , Position, ObjAssemblyMonitorInspStatus.MonitorType, MonitorTypeCode, Note, Remark, Description,
                                                   , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel,
                                                   SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue,
                                                   AssemblyTypeID, MaintenanceEvent, , , , , , , , , , , , , , , ObjAssemblyMonitorInspStatus.Reference, ,
                                                   DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2,
                                                   ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , ,
                                                   ObjAssemblyMonitorInspStatus.IsApplicable, StatusID.ToString, AssemblyStatusID:=ObjAssemblyStatus.ID.ToString, DueStatus:=DueStatus,
                                                   WONumber:=nWONumber, MachineID:=ObjMachine.MachineID.ToString, ModelID:=ObjAssemblyStatus.ModelID.ToString, MaintenanceTypeID:=6,
                                                   IsMaster:=ObjAssemblyMonitorInspStatus.IsMaster, Zone:=Zone, Area:=Area, IsRII:=IsRII, ReqNumber:=ReqNumber.ToString.Trim.TrimEnd(","),
                                                   LinkedMaintenanceActivityCount:=LinkedMaintenanceActivityCount))

                                        End If

                                    End If

                                End If

                            Next

                        End If

                        If chkComponent.Checked Then

                            For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList

                                For Each ObjCompMonitorInspStatus In ObjCompStatus.CompMonitorInspStatusList

                                    If InspectionTypeID.Contains(ObjCompMonitorInspStatus.PartMonitorInspTypeID) Then

                                        If ObjCompMonitorInspStatus.CompMonitorInspStatusPeriodList.Count > 0 Then

                                            If (ObjCompMonitorInspStatus.IsApplicable = True) And (Not (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True)) Then

                                                ATAChapter = ObjCompMonitorInspStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorInspStatus.ATANomenclature
												'Description = ObjCompMonitorInspStatus.Description
												Description = IIf(Expression:=AppSettings(name:="ClientCode") = "SHN", TruePart:=IIf(Expression:=ObjCompMonitorInspStatus.PartMonitorInspCode <> "", TruePart:=IIf(IsExcel, TruePart:="Task No.:" + ObjCompMonitorInspStatus.PartMonitorInspCode & Chr(10), FalsePart:="<b>Task No.:" + ObjCompMonitorInspStatus.PartMonitorInspCode & "</b><BR>"), FalsePart:=""), FalsePart:="") + ObjCompMonitorInspStatus.Description
												PartNo = ObjCompStatus.PartName
                                                CompSerialNo = ObjCompStatus.CompSerialNo
                                                Position = ObjCompStatus.Position
                                                MonitorTypeCode = ObjCompMonitorInspStatus.Code
                                                EstimatedDate = ObjCompMonitorInspStatus.EstimatedDateFormatted
                                                AssemblyModel = ObjAssemblyStatus.Model
                                                AssemblySerialNo = ObjAssemblyStatus.SerialNo & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyStatus.Position
                                                MinimumRemainingValue = ObjCompMonitorInspStatus.MinimumRemainingValue
                                                AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID

                                                StatusMasterID = ObjCompMonitorInspStatus.PartMonitorInspID  '11-Sep-2008
                                                DueStatus = ObjCompMonitorInspStatus.DueStatus
                                                DocumentTypeForID = 11

                                                'Remark = ObjCompMonitorInspStatus.DoneRemark  'Added By Saylee on 20-08-2008
                                                Remark = ObjCompStatus.InstallationRemark + " " + ObjCompMonitorInspStatus.DoneRemark  'Added By Saylee on 20-08-2008
                                                Code = ObjCompMonitorInspStatus.PartMonitorInspCode
                                                DoneOnDate = ObjCompMonitorInspStatus.DoneOn  'Added By Saylee 2-Aug-2012

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

                                                AssemblyDueAsof = "" 'Added By DEVEN On 14/06/2008
                                                AssemblyDueAsof1 = "" 'Added By DEVEN On 14/06/2008
                                                AssemblyDueAsof2 = "" 'Added By DEVEN On 14/06/2008

                                                SinceNew = ""
                                                SinceNew1 = ""
                                                SinceNew2 = ""
                                                DoneAt = ""
                                                DoneAt1 = ""
                                                DoneAt2 = ""
                                                MaintenanceEvent = ""

                                                'Added By Saylee on 04-08-2008
                                                Extension = ""
                                                Extension1 = ""
                                                Extension2 = ""

                                                For Each ObjCompMonitorInspStatusPeriod In ObjCompMonitorInspStatus.CompMonitorInspStatusPeriodList
                                                    If Report = 1 Then 'Portarait
                                                        If ObjCompMonitorInspStatusPeriod.PeriodID = 1 Then
                                                            Freq1 = ObjCompMonitorInspStatusPeriod.FrequencyValue
                                                            ElapsedTime = ObjCompMonitorInspStatusPeriod.ElapsedValue
                                                            RemainingTime = ObjCompMonitorInspStatusPeriod.RemainingValue
                                                            'AssemblyDueAsof = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
                                                            'Added By Shweta 7-June-2012

                                                            DoneAt = ObjCompMonitorInspStatusPeriod.DoneOnValue
                                                            If ((Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL")) Then  'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
                                                                AssemblyDueAsof = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame   'Added By Saylee 2-Aug-2012
                                                                If DoneOnDate <> "" Then DoneAt = ObjCompMonitorInspStatusPeriod.AssemblyDoneOnValueTextByAirFrame
                                                            ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Added By Prashant 26-Jun-2013 BA26062013
                                                                AssemblyDueAsof = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                            Else
                                                                AssemblyDueAsof = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText
                                                            End If
                                                            '**********************************

                                                            DueAsof = ObjCompMonitorInspStatusPeriod.DueOnValue
                                                            SinceNew = ObjCompMonitorInspStatusPeriod.CompCurrentValue

                                                            'Added By Saylee on 04-08-2008
                                                            Extension = ObjCompMonitorInspStatusPeriod.ExtensionValue
                                                        End If
                                                        If ObjCompMonitorInspStatusPeriod.PeriodID = 2 Then
                                                            Freq2 = ObjCompMonitorInspStatusPeriod.FrequencyValueFormatted
                                                            ElapsedTime1 = ObjCompMonitorInspStatusPeriod.ElapsedValueFormatted
                                                            RemainingTime1 = ObjCompMonitorInspStatusPeriod.RemainingValueFormatted
                                                            AssemblyDueAsof1 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted 'Added By DEVEN On 14/06/2008
                                                            DueAsof1 = ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
                                                            SinceNew1 = ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted
                                                            DoneAt1 = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                            'Added By Saylee on 04-08-2008
                                                            Extension1 = ObjCompMonitorInspStatusPeriod.ExtensionValueFormatted
                                                        End If
														'Added PeriodID=11 By Vikrant For ALL 21062012
														'If ObjCompMonitorInspStatusPeriod.PeriodID = 3 Or ObjCompMonitorInspStatusPeriod.PeriodID = 4 Or ObjCompMonitorInspStatusPeriod.PeriodID = 5 Or ObjCompMonitorInspStatusPeriod.PeriodID = 6 Or ObjCompMonitorInspStatusPeriod.PeriodID = 7 Or ObjCompMonitorInspStatusPeriod.PeriodID = 8 Or ObjCompMonitorInspStatusPeriod.PeriodID = 9 Or ObjCompMonitorInspStatusPeriod.PeriodID = 12 Or ObjCompMonitorInspStatusPeriod.PeriodID = 13 Or ObjCompMonitorInspStatusPeriod.PeriodID = 14 Or ObjCompMonitorInspStatusPeriod.PeriodID = 15 Or ObjCompMonitorInspStatusPeriod.PeriodID = 11 Then
														'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
														If ObjCompMonitorInspStatusPeriod.PeriodID >= 3 Then
															If Freq3 = "" Then
																Freq3 = ObjCompMonitorInspStatusPeriod.FrequencyValue
																ElapsedTime2 = ObjCompMonitorInspStatusPeriod.ElapsedValue
																RemainingTime2 = ObjCompMonitorInspStatusPeriod.RemainingValue
																'Added by Saylee on 11-Mar-2013 for ALL11032013 - 1
																'AssemblyDueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
																If ObjCompMonitorInspStatusPeriod.PeriodID = 9 Then
																	AssemblyDueAsof2 = "" 'ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
																Else
																	'Added By Prashant 26-Jun-2013 BA26062013
																	If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
																		AssemblyDueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame
																	Else
																		AssemblyDueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
																	End If

																End If
																'*****************
																DueAsof2 = ObjCompMonitorInspStatusPeriod.DueOnValue
																SinceNew2 = ObjCompMonitorInspStatusPeriod.CompCurrentValue
																DoneAt2 = ObjCompMonitorInspStatusPeriod.DoneOnValue
																'Added By Saylee on 04-08-2008
																Extension2 = ObjCompMonitorInspStatusPeriod.ExtensionValue
															Else
																Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.FrequencyValue
																ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.ElapsedValue
																RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.RemainingValue
																'Added by Saylee on 11-Mar-2013 for ALL11032013 - 1
																'AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
																If ObjCompMonitorInspStatusPeriod.PeriodID = 9 Then
																	AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & "" 'AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
																Else
																	'Added By Prashant 26-Jun-2013 BA26062013
																	If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
																		AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame
																	Else
																		AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText  'Added By DEVEN On 14/06/2008
																	End If

																End If
																'**********************
																DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DueOnValue
																SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.CompCurrentValue
																DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DoneOnValue
																'Added By Saylee on 04-08-2008
																Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.ExtensionValue
															End If
														End If

													End If
                                                Next
                                                AssemblyID = ObjAssemblyStatus.AssemblyID
                                                AssemblyType = ObjAssemblyStatus.AssemblyType
                                                RegNo = ObjMachine.RegNo
                                                'Rajnish 08-08-2008
                                                If IsPreviewClicked Then
                                                    RequiredManHours = PartMonitorInsp.GetPartMonitorInsp(ObjCompMonitorInspStatus.PartMonitorInspID).RequiredManHours
                                                Else
                                                    RequiredManHours = ObjCompMonitorInspStatus.RequiredManHours
                                                End If
                                                Customer = ObjMachine.Customer

                                                Note = ObjCompMonitorInspStatus.Notes

                                                'Commented and Added by Saylee on 10-Oct-2013 for ALL10102013
                                                'MaintenanceEvent = ObjCompMonitorInspStatus.Type
                                                If ObjCompMonitorInspStatus.Reference <> "" Then
                                                    MaintenanceEvent = ObjCompMonitorInspStatus.Type & " (" & ObjCompMonitorInspStatus.MonitorType & ")" & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatus.Reference & IIf(AppSettings("ClientCode") = "KamAir" Or AppSettings("ClientCode") = "MEL", IIf(ObjCompMonitorInspStatus.PartMonitorInspCode <> "", IIf(IsExcel, Chr(10), vbCrLf) & " (" & ObjCompMonitorInspStatus.PartMonitorInspCode & ")", ""), "")
                                                Else
                                                    MaintenanceEvent = ObjCompMonitorInspStatus.Type & " (" & ObjCompMonitorInspStatus.MonitorType & ")" & IIf(AppSettings("ClientCode") = "KamAir" Or AppSettings("ClientCode") = "MEL", IIf(ObjCompMonitorInspStatus.PartMonitorInspCode <> "", IIf(IsExcel, Chr(10), vbCrLf) & " (" & ObjCompMonitorInspStatus.PartMonitorInspCode & ")", ""), "")
                                                End If

                                                '*********************************
                                                'Added By Saylee on 04-08-2008
                                                ExtensionDate = ObjCompMonitorInspStatus.ExtensionDate
                                                ApprovalRemark = ObjCompMonitorInspStatus.ApprovalRemark

                                                StatusID = ObjCompMonitorInspStatus.ID 'Added by Saylee on 6-May-2013 for ALL06052013-1

                                                If chkwithWONo.Checked = True Or IsPreviewClicked Then 'Added by Saylee on 6-May-2013 for ALL06052013-1
                                                    mnWOListForDueJobs = nWOListForDueJobs.GetWOListForDueJobs(StatusID)
                                                    If mnWOListForDueJobs.Count > 0 Then
                                                        nWONumber = mnWOListForDueJobs(0).WONumber + vbCrLf + mnWOListForDueJobs(0).WODateFormatted
                                                        'REQ
                                                        mRequisitionListNew = RequisitionListNew.GetRequisitionList(WOID:=mnWOListForDueJobs(0).ID.ToString)
                                                        For i As Integer = 0 To mRequisitionListNew.Count - 1
                                                            ReqNumber.Append(mRequisitionListNew(i).RequisitionTextNo + ", ")
                                                        Next
                                                        'End
                                                    Else
                                                        nWONumber = ""
                                                        ReqNumber.Clear()
                                                    End If
                                                End If

                                                Zone = ""
                                                Area = ""
                                                IsRII = False

                                                'If ObjCompMonitorInspStatus.CompMonitorInspStatusPeriodList.Count > 0 Then
                                                ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, MaintenanceEvent, AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, ObjCompMonitorInspStatus.MonitorType, MonitorTypeCode, Note, Remark, Description,
                                                                     , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue,
                                                                     AssemblyTypeID, MaintenanceEvent, , , , , , , , , , , , , , , ObjCompMonitorInspStatus.Reference, , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , ObjCompMonitorInspStatus.IsApplicable, StatusID.ToString, CompStatusID:=ObjCompStatus.ID.ToString, AssemblyStatusID:=ObjAssemblyStatus.ID.ToString, DueStatus:=DueStatus, WONumber:=nWONumber, MachineID:=ObjMachine.MachineID.ToString, ModelID:=ObjAssemblyStatus.ModelID.ToString, MaintenanceTypeID:=9, IsMaster:=ObjCompMonitorInspStatus.IsMaster, Zone:=Zone, Area:=Area, IsRII:=IsRII, ReqNumber:=ReqNumber.ToString.Trim.TrimEnd(",")))

                                            End If

                                        End If

                                    End If

                                Next

                            Next

                        End If

                    End If

                    'Directives
                    If IsModSelect = True Then

                        If chkAssembly.Checked Then

                            For Each ObjAssemblyMonitorModStatus In ObjAssemblyStatus.AssemblyMonitorModStatusList

                                If ModificationTypeID.Contains(ObjAssemblyMonitorModStatus.ModelMonitorModTypeID) Then

                                    If ObjAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriodList.Count > 0 Then

                                        If (ObjAssemblyMonitorModStatus.IsApplicable = True) And (Not (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True)) Then

                                            ATAChapter = ObjAssemblyMonitorModStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorModStatus.ATANomenclature
                                            'Commented and changed by Saylee on 10-Oct-2013 for ALL10102013
                                            'Description = ObjAssemblyMonitorModStatus.Description & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatus.Number & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatus.Reference
                                            If (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022 ''Added By Prashant On 11-may-2022  Deccan11052022
                                                Description = ObjAssemblyMonitorModStatus.Description
                                            Else
												'Description = ObjAssemblyMonitorModStatus.Number & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatus.Description
												'Description = "<b>" & ObjAssemblyMonitorModStatus.Number & "</b>" & IIf(IsExcel, Chr(10), "<br>") & ObjAssemblyMonitorModStatus.Description 'Added By Prashant on 27-Jul-2023
												Description = "<b>" & ObjAssemblyMonitorModStatus.Number & "</b>" + IIf(Expression:=AppSettings(name:="ClientCode") = "SHN", TruePart:=IIf(Expression:=ObjAssemblyMonitorModStatus.ModelMonitorModCode <> "", TruePart:=IIf(IsExcel, TruePart:=vbCrLf + "Task No.:" + ObjAssemblyMonitorModStatus.ModelMonitorModCode & Chr(10), FalsePart:="<BR><b>Task No.:" + ObjAssemblyMonitorModStatus.ModelMonitorModCode & "</b><BR>"), FalsePart:=""), FalsePart:="") & IIf(IsExcel, Chr(10), "<br>") & ObjAssemblyMonitorModStatus.Description  'Added By Prashant on 27-Jul-2023
											End If

                                            '****************************
                                            AssemblyModel = ObjAssemblyStatus.Model
                                            AssemblySerialNo = ObjAssemblyStatus.SerialNo & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyStatus.Position
                                            Position = ""
                                            MonitorTypeCode = ObjAssemblyMonitorModStatus.Code
                                            EstimatedDate = ObjAssemblyMonitorModStatus.EstimatedDateFormatted
                                            MinimumRemainingValue = ObjAssemblyMonitorModStatus.MinimumRemainingValue
                                            AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID

                                            StatusMasterID = ObjAssemblyMonitorModStatus.ModelMonitorModID  '11-Sep-2008                        
                                            DueStatus = ObjAssemblyMonitorModStatus.DueStatus
                                            DocumentTypeForID = 8

                                            'Remark = ObjAssemblyMonitorModStatus.DoneRemark  'Added By Saylee on 20-08-2008
                                            Remark = ObjAssemblyStatus.InstallationRemark + " " + ObjAssemblyMonitorModStatus.DoneRemark 'Added By Saylee on 20-08-2008
                                            Code = ObjAssemblyMonitorModStatus.ModelMonitorModCode
                                            DoneOnDate = ObjAssemblyMonitorModStatus.DoneOn  'Added By Saylee 2-Aug-2012
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

                                            AssemblyDueAsof = "" 'Added By DEVEN On 14/06/2008
                                            AssemblyDueAsof1 = "" 'Added By DEVEN On 14/06/2008
                                            AssemblyDueAsof2 = "" 'Added By DEVEN On 14/06/2008

                                            SinceNew = ""
                                            SinceNew1 = ""
                                            SinceNew2 = ""
                                            DoneAt = ""
                                            DoneAt1 = ""
                                            DoneAt2 = ""
                                            MaintenanceEvent = ""

                                            'Added By Saylee on 04-08-2008
                                            Extension = ""
                                            Extension1 = ""
                                            Extension2 = ""

                                            For Each ObjAssemblyMonitorModStatusPeriod In ObjAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriodList
                                                If Report = 1 Then 'Portarait
                                                    If ObjAssemblyMonitorModStatusPeriod.PeriodID = 1 Then
                                                        Freq1 = ObjAssemblyMonitorModStatusPeriod.FrequencyValue
                                                        ElapsedTime = ObjAssemblyMonitorModStatusPeriod.ElapsedValue
                                                        RemainingTime = ObjAssemblyMonitorModStatusPeriod.RemainingValue
                                                        DueAsof = ObjAssemblyMonitorModStatusPeriod.DueOnValue
                                                        'AssemblyDueAsof = ObjAssemblyMonitorModStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
                                                        'Added By Shweta 7-June-2012

                                                        DoneAt = ObjAssemblyMonitorModStatusPeriod.DoneOnValue
                                                        If ((Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL")) Then  'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
                                                            AssemblyDueAsof = ObjAssemblyMonitorModStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                            If DoneOnDate <> "" Then DoneAt = ObjAssemblyMonitorModStatusPeriod.AssemblyDoneOnValueTextByAirFrame 'Added By Saylee 2-Aug-2012
                                                        ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Added By Prashant 26-Jun-2013 BA26062013
                                                            AssemblyDueAsof = ObjAssemblyMonitorModStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                        Else
                                                            AssemblyDueAsof = ObjAssemblyMonitorModStatusPeriod.DueOnValue

                                                        End If
                                                        '**********************************
                                                        SinceNew = ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValue

                                                        'Added By Saylee on 04-08-2008
                                                        Extension = ObjAssemblyMonitorModStatusPeriod.ExtensionValue
                                                    End If
                                                    If ObjAssemblyMonitorModStatusPeriod.PeriodID = 2 Then
                                                        Freq2 = ObjAssemblyMonitorModStatusPeriod.FrequencyValueFormatted
                                                        ElapsedTime1 = ObjAssemblyMonitorModStatusPeriod.ElapsedValueFormatted
                                                        RemainingTime1 = ObjAssemblyMonitorModStatusPeriod.RemainingValueFormatted
                                                        DueAsof1 = ObjAssemblyMonitorModStatusPeriod.DueOnValueFormatted
                                                        AssemblyDueAsof1 = ObjAssemblyMonitorModStatusPeriod.DueOnValueFormatted 'Added By DEVEN On 14/06/2008
                                                        SinceNew1 = ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValueFormatted
                                                        DoneAt1 = ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted
                                                        'Added By Saylee on 04-08-2008
                                                        Extension1 = ObjAssemblyMonitorModStatusPeriod.ExtensionValueFormatted
                                                    End If
													'Added PeriodID=11 By Vikrant For ALL 21062012
													'If ObjAssemblyMonitorModStatusPeriod.PeriodID = 3 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 4 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 5 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 6 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 7 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 8 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 9 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 12 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 13 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 14 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 15 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 11 Then
													'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
													If ObjAssemblyMonitorModStatusPeriod.PeriodID >= 3 Then
														If Freq3 = "" Then
															Freq3 = ObjAssemblyMonitorModStatusPeriod.FrequencyValue
															ElapsedTime2 = ObjAssemblyMonitorModStatusPeriod.ElapsedValue
															RemainingTime2 = ObjAssemblyMonitorModStatusPeriod.RemainingValue
															DueAsof2 = ObjAssemblyMonitorModStatusPeriod.DueOnValue
															'Added By Prashant 26-Jun-2013 BA26062013
															If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
																AssemblyDueAsof2 = ObjAssemblyMonitorModStatusPeriod.AssemblyDueOnValueByAirFrame
															Else
																AssemblyDueAsof2 = ObjAssemblyMonitorModStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
															End If

															SinceNew2 = ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValue
															DoneAt2 = ObjAssemblyMonitorModStatusPeriod.DoneOnValue
															'Added By Saylee on 04-08-2008
															Extension2 = ObjAssemblyMonitorModStatusPeriod.ExtensionValue
														Else
															Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.FrequencyValue
															ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.ElapsedValue
															RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.RemainingValue
															DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.DueOnValue
															'Added By Prashant 26-Jun-2013 BA26062013
															If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
																AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.AssemblyDueOnValueByAirFrame
															Else
																AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
															End If

															SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValue
															DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.DoneOnValue
															'Added By Saylee on 04-08-2008
															Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.ExtensionValue
														End If
													End If
												End If
                                            Next
                                            AssemblyID = ObjAssemblyStatus.AssemblyID
                                            AssemblyType = ObjAssemblyStatus.AssemblyType
                                            RegNo = ObjMachine.RegNo
                                            'Rajnish 08-08-2008
                                            If IsPreviewClicked Then
                                                RequiredManHours = ModelMonitorMod.GetModelMonitorMod(ObjAssemblyMonitorModStatus.ModelMonitorModID).RequiredManHours
                                            Else
                                                RequiredManHours = ObjAssemblyMonitorModStatus.RequiredManHours
                                            End If
                                            Customer = ObjMachine.Customer

                                            Note = ObjAssemblyMonitorModStatus.Notes
                                            'Added by Saylee on 10-Oct-2013 for ALL10102013
                                            'MaintenanceEvent = ObjAssemblyMonitorModStatus.Type 
                                            If ObjAssemblyMonitorModStatus.Reference <> "" Then
                                                MaintenanceEvent = ObjAssemblyMonitorModStatus.Type & " (" & ObjAssemblyMonitorModStatus.MonitorType & ")" & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatus.Reference & IIf(AppSettings("ClientCode") = "KamAir" Or AppSettings("ClientCode") = "MEL", IIf(ObjAssemblyMonitorModStatus.ModelMonitorModCode <> "", IIf(IsExcel, Chr(10), vbCrLf) & " (" & ObjAssemblyMonitorModStatus.ModelMonitorModCode & ")", ""), "")
                                            Else
                                                MaintenanceEvent = ObjAssemblyMonitorModStatus.Type & " (" & ObjAssemblyMonitorModStatus.MonitorType & ")" & IIf(AppSettings("ClientCode") = "KamAir" Or AppSettings("ClientCode") = "MEL", IIf(ObjAssemblyMonitorModStatus.ModelMonitorModCode <> "", IIf(IsExcel, Chr(10), vbCrLf) & " (" & ObjAssemblyMonitorModStatus.ModelMonitorModCode & ")", ""), "")
                                            End If


                                            '*************************
                                            'Added By Saylee on 04-08-2008
                                            ExtensionDate = ObjAssemblyMonitorModStatus.ExtensionDate
                                            ApprovalRemark = ObjAssemblyMonitorModStatus.ApprovalRemark

                                            StatusID = ObjAssemblyMonitorModStatus.ID 'Added by Saylee on 6-May-2013 for ALL06052013-1


                                            If chkwithWONo.Checked = True Or IsPreviewClicked Then 'Added by Saylee on 6-May-2013 for ALL06052013-1
                                                mnWOListForDueJobs = nWOListForDueJobs.GetWOListForDueJobs(StatusID)
                                                If mnWOListForDueJobs.Count > 0 Then
                                                    nWONumber = mnWOListForDueJobs(0).WONumber + vbCrLf + mnWOListForDueJobs(0).WODateFormatted
                                                    'REQ
                                                    mRequisitionListNew = RequisitionListNew.GetRequisitionList(WOID:=mnWOListForDueJobs(0).ID.ToString)
                                                    For i As Integer = 0 To mRequisitionListNew.Count - 1
                                                        ReqNumber.Append(mRequisitionListNew(i).RequisitionTextNo + ", ")
                                                    Next
                                                    'End
                                                Else
                                                    nWONumber = ""
                                                    ReqNumber.Clear()
                                                End If
                                            End If

                                            Zone = ObjAssemblyMonitorModStatus.Zone
                                            Area = ObjAssemblyMonitorModStatus.Area
                                            IsRII = ObjAssemblyMonitorModStatus.IsRII
                                            LinkedMaintenanceActivityCount = ObjAssemblyMonitorModStatus.LinkedMaintenanceActivityCount   'Added by Prashant  9-Sep-2020 ALL09092020
                                            'If ObjAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriodList.Count > 0 Then
                                            ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, , AssemblySerialNo, ATAChapter, , , Position, ObjAssemblyMonitorModStatus.MonitorType, MonitorTypeCode, Note, Remark, Description,
                                           , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel,
                                           SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, AssemblyTypeID,
                                           MaintenanceEvent, , , , , , , , , , , , , , ObjAssemblyMonitorModStatus.Number,
                                           ObjAssemblyMonitorModStatus.Reference, , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2,
                                           Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code,
                                           StatusMasterID.ToString, DocumentTypeForID, , , ObjAssemblyMonitorModStatus.IsApplicable, StatusID.ToString,
                                           AssemblyStatusID:=ObjAssemblyStatus.ID.ToString, DueStatus:=DueStatus, WONumber:=nWONumber,
                                           MachineID:=ObjMachine.MachineID.ToString, ModelID:=ObjAssemblyStatus.ModelID.ToString, MaintenanceTypeID:=7,
                                           IsMaster:=ObjAssemblyMonitorModStatus.IsMaster, Zone:=Zone, Area:=Area, IsRII:=IsRII,
                                           ReqNumber:=ReqNumber.ToString.Trim.TrimEnd(","), LinkedMaintenanceActivityCount:=LinkedMaintenanceActivityCount))

                                        End If

                                    End If

                                End If

                            Next

                        End If

                        If chkComponent.Checked Then

                            For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList

                                For Each ObjCompMonitorModStatus In ObjCompStatus.CompMonitorModStatusList

                                    If ModificationTypeID.Contains(ObjCompMonitorModStatus.PartMonitorModTypeID) Then

                                        If ObjCompMonitorModStatus.CompMonitorModStatusPeriodList.Count > 0 Then

                                            If (ObjCompMonitorModStatus.IsApplicable = True) And (Not (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted)) Then
                                                ATAChapter = ObjCompMonitorModStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorModStatus.ATANomenclature
                                                'Commented and Added by Saylee on 10-Oct-2013 for ALL10102013
                                                'Description = ObjCompMonitorModStatus.Description & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatus.Number & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatus.Reference
                                                If (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022 ''Added By Prashant On 11-may-2022  Deccan11052022
                                                    Description = ObjCompMonitorModStatus.Description
                                                Else
													'Description = ObjCompMonitorModStatus.Description & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatus.Number
													'Description = ObjCompMonitorModStatus.Description & IIf(IsExcel, Chr(10), vbCrLf) & "<b>" & ObjCompMonitorModStatus.Number & "</b>" 'Added By Prashant on 27-Jul-2023
													Description = IIf(Expression:=AppSettings(name:="ClientCode") = "SHN", TruePart:=IIf(Expression:=ObjCompMonitorModStatus.PartMonitorModCode <> "", TruePart:=IIf(IsExcel, TruePart:="Task No.:" + ObjCompMonitorModStatus.PartMonitorModCode & Chr(10), FalsePart:="<b>Task No.:" + ObjCompMonitorModStatus.PartMonitorModCode & "</b><BR>"), FalsePart:=""), FalsePart:="") + ObjCompMonitorModStatus.Description & IIf(IsExcel, Chr(10), vbCrLf) & "<b>" & ObjCompMonitorModStatus.Number & "</b>"  'Added By Prashant on 27-Jul-2023
												End If

                                                '**********************************
                                                PartNo = ObjCompStatus.PartName
                                                CompSerialNo = ObjCompStatus.CompSerialNo
                                                Position = ObjCompStatus.Position
                                                MonitorTypeCode = ObjCompMonitorModStatus.Code
                                                EstimatedDate = ObjCompMonitorModStatus.EstimatedDateFormatted
                                                AssemblyModel = ObjAssemblyStatus.Model
                                                AssemblySerialNo = ObjAssemblyStatus.SerialNo & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyStatus.Position
                                                MinimumRemainingValue = ObjCompMonitorModStatus.MinimumRemainingValue
                                                AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID

                                                StatusMasterID = ObjCompMonitorModStatus.PartMonitorModID  '11-Sep-2008                        
                                                DueStatus = ObjCompMonitorModStatus.DueStatus
                                                DocumentTypeForID = 10

                                                'Remark = ObjCompMonitorModStatus.DoneRemark  'Added By Saylee on 20-08-2008
                                                Remark = ObjCompStatus.InstallationRemark + " " + ObjCompMonitorModStatus.DoneRemark    'Added By Saylee on 20-08-2008
                                                Code = ObjCompMonitorModStatus.PartMonitorModCode
                                                DoneOnDate = ObjCompMonitorModStatus.DoneOn  'Added By Saylee 2-Aug-2012
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

                                                AssemblyDueAsof = "" 'Added By DEVEN On 14/06/2008
                                                AssemblyDueAsof1 = "" 'Added By DEVEN On 14/06/2008
                                                AssemblyDueAsof2 = "" 'Added By DEVEN On 14/06/2008

                                                SinceNew = ""
                                                SinceNew1 = ""
                                                SinceNew2 = ""
                                                DoneAt = ""
                                                DoneAt1 = ""
                                                DoneAt2 = ""
                                                MaintenanceEvent = ""

                                                'Added By Saylee on 04-08-2008
                                                Extension = ""
                                                Extension1 = ""
                                                Extension2 = ""
                                                For Each ObjCompMonitorModStatusPeriod In ObjCompMonitorModStatus.CompMonitorModStatusPeriodList
                                                    If Report = 1 Then 'Portarait
                                                        If ObjCompMonitorModStatusPeriod.PeriodID = 1 Then
                                                            Freq1 = ObjCompMonitorModStatusPeriod.FrequencyValue
                                                            ElapsedTime = ObjCompMonitorModStatusPeriod.ElapsedValue
                                                            RemainingTime = ObjCompMonitorModStatusPeriod.RemainingValue
                                                            'AssemblyDueAsof = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
                                                            'Added By Shweta 7-June-2012
                                                            DoneAt = ObjCompMonitorModStatusPeriod.DoneOnValue
                                                            If ((Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL")) Then    'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
                                                                AssemblyDueAsof = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                                If DoneOnDate <> "" Then DoneAt = ObjCompMonitorModStatusPeriod.AssemblyDoneOnValueTextByAirFrame 'Added By Saylee 2-Aug-2012
                                                            ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Added By Prashant 26-Jun-2013 BA26062013
                                                                AssemblyDueAsof = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                            Else
                                                                AssemblyDueAsof = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
                                                            End If
                                                            '**********************************

                                                            DueAsof = ObjCompMonitorModStatusPeriod.DueOnValue
                                                            SinceNew = ObjCompMonitorModStatusPeriod.CompCurrentValue

                                                            'Added By Saylee on 04-08-2008
                                                            Extension = ObjCompMonitorModStatusPeriod.ExtensionValue
                                                        End If
                                                        If ObjCompMonitorModStatusPeriod.PeriodID = 2 Then
                                                            Freq2 = ObjCompMonitorModStatusPeriod.FrequencyValueFormatted
                                                            ElapsedTime1 = ObjCompMonitorModStatusPeriod.ElapsedValueFormatted
                                                            RemainingTime1 = ObjCompMonitorModStatusPeriod.RemainingValueFormatted
                                                            AssemblyDueAsof1 = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormatted 'Added By DEVEN On 14/06/2008
                                                            DueAsof1 = ObjCompMonitorModStatusPeriod.DueOnValueFormatted
                                                            SinceNew1 = ObjCompMonitorModStatusPeriod.CompCurrentValueFormatted
                                                            DoneAt1 = ObjCompMonitorModStatusPeriod.DoneOnValueFormatted
                                                            'Added By Saylee on 04-08-2008
                                                            Extension1 = ObjCompMonitorModStatusPeriod.ExtensionValueFormatted
                                                        End If
														'Added PeriodID=11 By Vikrant For ALL 21062012
														'If ObjCompMonitorModStatusPeriod.PeriodID = 3 Or ObjCompMonitorModStatusPeriod.PeriodID = 4 Or ObjCompMonitorModStatusPeriod.PeriodID = 5 Or ObjCompMonitorModStatusPeriod.PeriodID = 6 Or ObjCompMonitorModStatusPeriod.PeriodID = 7 Or ObjCompMonitorModStatusPeriod.PeriodID = 8 Or ObjCompMonitorModStatusPeriod.PeriodID = 9 Or ObjCompMonitorModStatusPeriod.PeriodID = 12 Or ObjCompMonitorModStatusPeriod.PeriodID = 13 Or ObjCompMonitorModStatusPeriod.PeriodID = 14 Or ObjCompMonitorModStatusPeriod.PeriodID = 15 Or ObjCompMonitorModStatusPeriod.PeriodID = 11 Then
														'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
														If ObjCompMonitorModStatusPeriod.PeriodID >= 3 Then
															If Freq3 = "" Then
																Freq3 = ObjCompMonitorModStatusPeriod.FrequencyValue
																ElapsedTime2 = ObjCompMonitorModStatusPeriod.ElapsedValue
																RemainingTime2 = ObjCompMonitorModStatusPeriod.RemainingValue
																'Added by Saylee on 11-Mar-2013 for ALL11032013 - 1
																'AssemblyDueAsof2 = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
																If ObjCompMonitorModStatusPeriod.PeriodID = 9 Then
																	AssemblyDueAsof2 = ""  'ObjCompMonitorModStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
																Else
																	'Added By Prashant 26-Jun-2013 BA26062013
																	If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
																		AssemblyDueAsof2 = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextByAirFrame
																	Else
																		AssemblyDueAsof2 = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
																	End If

																End If
																'******************
																DueAsof2 = ObjCompMonitorModStatusPeriod.DueOnValue
																SinceNew2 = ObjCompMonitorModStatusPeriod.CompCurrentValue
																DoneAt2 = ObjCompMonitorModStatusPeriod.DoneOnValue
																'Added By Saylee on 04-08-2008
																Extension2 = ObjCompMonitorModStatusPeriod.ExtensionValue
															Else
																Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.FrequencyValue
																ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.ElapsedValue
																RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.RemainingValue
																'Added by Saylee on 11-Mar-2013 for ALL11032013 - 1
																'AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
																If ObjCompMonitorModStatusPeriod.PeriodID = 9 Then
																	AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & "" 'AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
																Else
																	'Added By Prashant 26-Jun-2013 BA26062013
																	If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
																		AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextByAirFrame
																	Else
																		AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
																	End If

																End If
																'***********************
																DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.DueOnValue
																SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.CompCurrentValue
																DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.DoneOnValue
																'Added By Saylee on 04-08-2008
																Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.ExtensionValue
															End If
														End If

													End If
                                                Next
                                                AssemblyID = ObjAssemblyStatus.AssemblyID
                                                AssemblyType = ObjAssemblyStatus.AssemblyType
                                                RegNo = ObjMachine.RegNo
                                                'Rajnish 08-08-2008
                                                If IsPreviewClicked Then
                                                    RequiredManHours = PartMonitorMod.GetPartMonitorMod(ObjCompMonitorModStatus.PartMonitorModID).RequiredManHours
                                                Else
                                                    RequiredManHours = ObjCompMonitorModStatus.RequiredManHours
                                                End If
                                                Customer = ObjMachine.Customer

                                                Note = ObjCompMonitorModStatus.Notes

                                                'Commented and Added by Saylee on 10-Oct-2013 for ALL10102013
                                                'MaintenanceEvent = ObjCompMonitorModStatus.Type
                                                If ObjCompMonitorModStatus.Reference <> "" Then
                                                    MaintenanceEvent = ObjCompMonitorModStatus.Type & " (" & ObjCompMonitorModStatus.MonitorType & ")" & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatus.Reference & IIf(AppSettings("ClientCode") = "KamAir" Or AppSettings("ClientCode") = "MEL", IIf(ObjCompMonitorModStatus.PartMonitorModCode <> "", IIf(IsExcel, Chr(10), vbCrLf) & " (" & ObjCompMonitorModStatus.PartMonitorModCode & ")", ""), "")
                                                Else
                                                    MaintenanceEvent = ObjCompMonitorModStatus.Type & " (" & ObjCompMonitorModStatus.MonitorType & ")" & IIf(AppSettings("ClientCode") = "KamAir" Or AppSettings("ClientCode") = "MEL", IIf(ObjCompMonitorModStatus.PartMonitorModCode <> "", IIf(IsExcel, Chr(10), vbCrLf) & " (" & ObjCompMonitorModStatus.PartMonitorModCode & ")", ""), "")
                                                End If

                                                '***************************************
                                                'Added By Saylee on 04-08-2008
                                                ExtensionDate = ObjCompMonitorModStatus.ExtensionDate
                                                ApprovalRemark = ObjCompMonitorModStatus.ApprovalRemark

                                                StatusID = ObjCompMonitorModStatus.ID 'Added by Saylee on 6-May-2013 for ALL06052013-1


                                                If chkwithWONo.Checked = True Or IsPreviewClicked Then 'Added by Saylee on 6-May-2013 for ALL06052013-1
                                                    mnWOListForDueJobs = nWOListForDueJobs.GetWOListForDueJobs(StatusID)
                                                    If mnWOListForDueJobs.Count > 0 Then
                                                        nWONumber = mnWOListForDueJobs(0).WONumber + vbCrLf + mnWOListForDueJobs(0).WODateFormatted
                                                        'REQ
                                                        mRequisitionListNew = RequisitionListNew.GetRequisitionList(WOID:=mnWOListForDueJobs(0).ID.ToString)
                                                        For i As Integer = 0 To mRequisitionListNew.Count - 1
                                                            ReqNumber.Append(mRequisitionListNew(i).RequisitionTextNo + ", ")
                                                        Next
                                                        'End
                                                    Else
                                                        nWONumber = ""
                                                        ReqNumber.Clear()
                                                    End If
                                                End If

                                                Zone = ""
                                                Area = ""
                                                IsRII = False

                                                'If ObjCompMonitorModStatus.CompMonitorModStatusPeriodList.Count > 0 Then
                                                ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, MaintenanceEvent, AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, ObjCompMonitorModStatus.MonitorType, MonitorTypeCode, Note, Remark, Description,
                                                                      , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue,
                                                                      AssemblyTypeID, MaintenanceEvent, , , , , , , , , , , , , , ObjCompMonitorModStatus.Number, ObjCompMonitorModStatus.Reference, , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , ObjCompMonitorModStatus.IsApplicable, StatusID.ToString, , CompStatusID:=ObjCompStatus.ID.ToString, AssemblyStatusID:=ObjAssemblyStatus.ID.ToString, DueStatus:=DueStatus, WONumber:=nWONumber, MachineID:=ObjMachine.MachineID.ToString, ModelID:=ObjAssemblyStatus.ModelID.ToString, MaintenanceTypeID:=10, IsMaster:=ObjCompMonitorModStatus.IsMaster, Zone:=Zone, Area:=Area, IsRII:=IsRII, ReqNumber:=ReqNumber.ToString.Trim.TrimEnd(",")))
                                            End If

                                        End If
                                    End If

                                Next

                            Next

                        End If

                    End If

                Next

            Next

        Catch ex As Exception

            Dim Day, Month, Year As String
            Day = Format(Today.Date.Day, "0#")
            Month = Format(Today.Date.Month, "0#")
            Year = Format(Today.Date.Year, "0#")
            Dim TodaysDate As String = Day & Month & Year
            Dim Path As String = AppSettings("DOCPath") & TodaysDate
            FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
            WriteLine(1, Date.Now.ToString + " Mail service (ReportDetail): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)

        End Try

        Return ReportMaintenanceDetails

    End Function

    Private Sub SetReportWithWONo(Optional ByMail As Boolean = False, Optional ByExcel As Boolean = False) 'Added by Saylee on 6-May-2013 for ALL06052013-1
        ReportMaintenanceDetails = New ReportMaintenanceDetailList
        ReportStatusList = New rptStatusList
        Dim da As New ObjectAdapter
        Dim ds As New dsReportMaintenanceDetail
        Dim rptMachineCertificates As MachineCertificateList
        ''Dim rptSnagCorrectiveActionListForDue As SnagCorrectiveActionListForDue   'Added By Prashant 20-Nov-2009
        Dim rptSnagCorrectiveActionListForDue As MELSnagCorrectiveActionListForDue  'Changed By Saylee on 19-Oct-2010
        'Dim rptDueDetail As New crDueReportDetailPortrait

        Dim mCompanyDetail As New CompanyDetail
        Dim searchstr As String = ""
        Dim searchstr6 As String = ""
        Dim searchstr8 As String = ""
        Dim OperatorName As String = ""

        SetValues()

        ReportDetail(mIsExcel)


        'Code Added by Deven on 02-Mar-20098*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/***/*/*/*/*/*/*/*/
        Dim mloglist As LogList
        mloglist = LogList.GetLogList(New Guid(cmbAircraft.SelectedValue.ToString), , AsonDate)
        '*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/***/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/

        If rbdDueLimits.Checked = True Then
            For Each mDueLimit In mDueLimits
                If CDec(Val(mDueLimit.PeriodLimit)) >= 0 Then
                    If searchstr = "" Then
                        searchstr = "For Next" & " " & searchstr & " " & mDueLimit.PeriodLimit & " " & mDueLimit.PeriodName
                    Else
                        searchstr = searchstr & ", " & mDueLimit.PeriodLimit & " " & mDueLimit.PeriodName
                    End If
                End If
            Next
        Else
            searchstr = "For Next" & " " & CDec(Val(txtPercentage.Text)).ToString & "% of Frequency"
        End If

        'Added By Rajnish on 26-11-2007
        searchstr = searchstr & ", " & "As On Date:" & txtFromDate.Text.Trim
        '------------------------------

        'code added By Deven on 11-04-2008 ====================
        Dim searchstr1 As String
        Dim mPerDayLimit As PerDayLimit
        If rbdSpecifyValues.Checked = True Then
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
        Else
            If CDec(Val(txtAvgMnths.Text)).ToString <> "" Then
                searchstr1 = "Estimated Due Date as Per Average of Last" & " " & CDec(Val(txtAvgMnths.Text)).ToString & " Months"
            Else
                searchstr1 = ""
            End If
        End If
        '===========================================
        Dim ReportName As String
        Dim ReportNameForPDF As String
        'Code Added By Deven on 07/04/2008------------
        Dim rptDueDetail As Engine.ReportClass
        If DueType = 1 Then
            '' rptSnagCorrectiveActionListForDue = SnagCorrectiveActionListForDue.GetSnagCorrectiveActionListForDue(New Guid(cmbAircraft.SelectedValue.ToString), AsonDate)  'Added By Prashant 20-Nov-2009
            If AppSettings("TimeFormat") = "HH:mm" Or AppSettings("TimeFormat") = "hh:mm" Then
                rptSnagCorrectiveActionListForDue = MELSnagCorrectiveActionListForDue.GetMELSnagCorrectiveActionListForDue(AsonDate, New Guid(cmbAircraft.SelectedValue.ToString), Guid.Empty, 0, 0, "HH:mm")
            Else
                rptSnagCorrectiveActionListForDue = MELSnagCorrectiveActionListForDue.GetMELSnagCorrectiveActionListForDue(AsonDate, New Guid(cmbAircraft.SelectedValue.ToString), Guid.Empty, 0, 0)
            End If
            If Not cmbAircraft.SelectedItem.ToString = "(ALL)" Then
                'Added by Saylee on 25-Sep-2008 for showing Due Certificates
                If ByMail = True Then
                    SetGridObject() ' to set PerDayLimitForDaysPeriod value if is For Mail
                End If
                rptMachineCertificates = MachineCertificateList.GetMachineCertificateList(New Guid(cmbAircraft.SelectedValue.ToString), AsonDate, IsForDue:=True, Days:=IIf(AppSettings("ClientCode") = "Heligo", -1, PerDayLimitForDaysPeriod))
                If cmbFormat.SelectedIndex = 0 Then  'Format 1
                    If rptMachineCertificates.Count = 0 Then
                        If AppSettings("ClientCode") = "GEP" Then
                            rptDueDetail = New crDueReportDetailForWONoGEP
                        ElseIf AppSettings("ClientCode") = "ADeccan" Then
                            rptDueDetail = New crDueReportDetailForWONoAirDeccan 'Added by Saylee on 7-Feb-2018 for ADeccan07022018 : New report format as per mail 6-Feb-2018
                        ElseIf AppSettings("ClientCode") = "UHPL" Then       'Added by Shital on 30-May-2019  (UHPL30052019 -Forecast Due Report New Format)
                            rptDueDetail = New crDueReportDetailForWONoUHPL
                        Else
                            rptDueDetail = New crDueReportDetailForWONo
                        End If
                    Else
                        If AppSettings("ClientCode") = "GEP" Then
                            'Commneted and Added by Prashant 12-Nov-2019 GEP Do not want show Certificates so above format taken
                            rptDueDetail = New crDueReportDetailForWONoGEP 'crDueReportDetailAircraftCertificateswithWONoGEP
                        ElseIf AppSettings("ClientCode") = "ADeccan" Then
                            rptDueDetail = New crDueReportDetailAircraftCertificateswithWONoAirDeccan  'Added by Saylee on 7-Feb-2018 for ADeccan07022018 : New report format as per mail 6-Feb-2018
                        ElseIf AppSettings("ClientCode") = "UHPL" Then       'Added by Shital on 30-May-2019  (UHPL30052019 -Forecast Due Report New Format)
                            rptDueDetail = New crDueReportDetailAircraftCertificateswithWONoUHPL
                        Else
                            rptDueDetail = New crDueReportDetailAircraftCertificateswithWONo
                        End If
                    End If
                End If
            Else
                If cmbFormat.SelectedIndex = 0 Then  'Format 1
                    rptDueDetail = New crDueReportDetailLandscape
                End If
            End If
            If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then
                If AppSettings("ClientCode") = "SPZ" Then
                    If (cmbAircraft.SelectedItem.Text = "(ALL)") Or (cmbAircraft.SelectedItem.Text = "(SELECT)") Then
                        ReportNameForPDF = "Monthly Due List"
                        ReportName = "Monthly Due List"

                    Else
                        ReportNameForPDF = "Monthly Due List"
                        ReportName = "Monthly Due List Number " + "__________________" + " / " + cmbAircraft.SelectedItem.Text + " / " + MonthName(Month(New SmartDate(txtFromDate.Text.Trim).FormattedText), True).ToString + "." + " / " + Year(New SmartDate(txtFromDate.Text.Trim).FormattedText).ToString + " ."
                    End If
                Else
                    If (cmbAircraft.SelectedItem.Text = "(ALL)") Or (cmbAircraft.SelectedItem.Text = "(SELECT)") Then
                        ReportNameForPDF = "Work Order List"
                        ReportName = "Work Order List"

                    Else
                        ReportNameForPDF = "Work Order List"
                        ReportName = "Work Order List Number " + "__________________" + " / " + cmbAircraft.SelectedItem.Text + " / " + MonthName(Month(New SmartDate(txtFromDate.Text.Trim).FormattedText), True).ToString + "." + " / " + Year(New SmartDate(txtFromDate.Text.Trim).FormattedText).ToString + " ."

                    End If
                End If

            ElseIf ((AppSettings("ClientCode") = "Heligo")) Then
                If cmbFormat.SelectedIndex = 1 Then 'Added By Vikrant On 03-Jun-2016 For ALL03062016-1
                    ReportName = "Weekly Call Out"
                    ReportNameForPDF = "Weekly Call Out"
                Else
                    If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                        ReportName = "Maintenance Forecast"
                        ReportNameForPDF = "Maintenance Forecast"
                    Else
                        ReportName = "Maintenance Status Report"
                        ReportNameForPDF = "Maintenance Status Report"
                    End If

                End If
            Else
                If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                    ReportName = "Maintenance Forecast"
                    ReportNameForPDF = "Maintenance Forecast"
                Else
                    ReportName = "Maintenance Status Report"
                    ReportNameForPDF = "Maintenance Status Report"
                End If

            End If

        End If
        '-------------------------------------------
        Dim x As String
        If mloglist.Count > 0 Then
            x = mloglist(0).LogDate.ToShortDateString
        Else
            x = txtFromDate.Text.Trim
        End If

        '--------------------------------------------------------
        Dim LastFlownDate As String = ""
        Dim LastMaintenanceActivityDate As String = ""
        Dim mMaxLogNo As MaxLogNo = MaxLogNo.GetMaxLogNo(AsonDate, New Guid(MachineName), New Guid(AssemblyName))

        If mMaxLogNo.Count <> 0 Then

            If (AppSettings("ClientCode") = "ADeccan") Then
                LastFlownDate = mMaxLogNo(0).LogDate.ToString + " ( " + mMaxLogNo(0).LogPageNo.ToString + " ) " 'Last Flight Log Date
            Else
                LastFlownDate = mMaxLogNo(0).LogDate.ToString 'Last Flight Log Date
            End If

        Else
            'Commented By Saylee on 21-Aug-2020
            'LastFlownDate = CType(Session("AircraftAsOnDate"), String)  'New SmartDate(txtFromDate.Value.ToString).FormattedText
            LastFlownDate = ""
            '*******************************
        End If

        'Added by Saylee on 2-Aug-2011
        ''Last Maintenance Activity
        If Not cmbAircraft.SelectedItem.ToString = "(ALL)" Then
            Dim mLastMachineMaintenanceActivity As LastMachineMaintenanceActivity = LastMachineMaintenanceActivity.GetLastMaintenanceActivity(AsonDate, New Guid(MachineName), New Guid(AssemblyName))
            If Not mLastMachineMaintenanceActivity.ID.Equals(Guid.Empty) Then
                LastMaintenanceActivityDate = ", Last Maintenance Done on  " + "( " + mLastMachineMaintenanceActivity.Date.ToString + " )"
                searchstr8 = mLastMachineMaintenanceActivity.Date.ToString
            End If
            ''***************************************
        End If

        If ((Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022
            searchstr6 = "Flying Hours updated till " + "( " + LastFlownDate + " ) " + LastMaintenanceActivityDate + " & Work Order Number - __________________"
        Else
            searchstr6 = LastFlownDate 'Mostly on Heligo Report
        End If

        'Added by vikrant on 11-Aug-2011
        If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "Indamer")) Then
            Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue))
            If cmbAircraft.SelectedIndex > -1 Then
                If mMachineOperatorName.OperatorName <> "" Then OperatorName = mMachineOperatorName.OperatorName
            End If
        ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
            OperatorName = searchstr7
        End If
        '--------------------------------------------------------
        Dim ReferenceNo As String = Trim(txtRefNo.Text) 'Added by Vikrant For HLI11102011 
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
    mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
    mCompanyDetail.WebSite, ReportName, searchstr, searchstr1, Assembly1, AppSettings("ClientCode"),
    "Aircraft is flown up to: " & New SmartDate(x).FormattedText, AppSettings("Product Version"), AppSettings("SINote"), searchstr6, OperatorName,
    searchstr8, ReferenceNo, AppSettings("Logo"), SearchStr13:=Aircraft, SearchStr14:=txtFromDate.Text, SearchStr15:=IIf(chkSignature.Checked, "True", "False"), SearchStr16:=Val(Trim(txtForecastingLimit.Text)).ToString, SearchStr19:=CustomerName)

        If ByMail = False Then
            If ReportMaintenanceDetails.Count = 0 Then
                MSGBoxCtrl.Show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 719)
                MarkLog(Action.Print, "Due-PeriodWise", mEventLogDetails, ErrorType.NoError, Guid.Empty, EventLogID)
            End If
        End If
        If (ByMail = True And ReportMaintenanceDetails.Count <= 0) Then
            SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, ReportName, ReportNameForPDF, "There is no record for this search criteria.", "", Session("ToSendMailIDs"),
                Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"),
                 SmtpHost:=mModuleList.Item("Due-PeriodWise").SmtpHost, SmtpPort:=mModuleList.Item("Due-PeriodWise").SmtpPort,
                SmtpUser:=mModuleList.Item("Due-PeriodWise").SmtpUser, SmtpPassword:=mModuleList.Item("Due-PeriodWise").SmtpPassword)
            Exit Sub
        End If
        '11-Sep-2008-------------------------------
        If Not mIsPreview Then
            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, ReportMaintenanceDetails)

            'Added by Saylee on 25-Sep-2008 for showing Due Certificates
            If DueType = 1 Then
                If Not cmbAircraft.SelectedItem.ToString = "(ALL)" Then
                    If rptMachineCertificates.Count <> 0 Then da.Fill(ds, rptMachineCertificates)
                End If
            End If

            '===================================

            da.Fill(ds, Report)
            da.Fill(ds, ReportStatusList)
            da.Fill(ds, rptSnagCorrectiveActionListForDue) 'Added By Prashant 20-Nov-2009
            da.Fill(ds, mrptImage)
            rptDueDetail.SetDataSource(ds)
            Session("CrystalReport") = rptDueDetail

            If ByMail = True Then
                SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, ReportName + " " + cmbAircraft.SelectedItem.Text, ReportNameForPDF, lblDateRangeFrom.Text + ", " + lblAircraft1.Text + ", " + lblAssembly1.Text,
                                          "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"),
                                          ReportGeneratedBy:=Session("ReportGenratedBy"),
                 SmtpHost:=mModuleList.Item("Due-PeriodWise").SmtpHost, SmtpPort:=mModuleList.Item("Due-PeriodWise").SmtpPort,
                SmtpUser:=mModuleList.Item("Due-PeriodWise").SmtpUser, SmtpPassword:=mModuleList.Item("Due-PeriodWise").SmtpPassword, OtherInfo:=searchstr)
            ElseIf ByExcel = True Then
                SetExcel(ReportMaintenanceDetails, Report, ReportName)
            Else
                Dim Str As String
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, [GetType], "openTranDetail", Str, True)
            End If

            'ResetValues()

            'Saving Periods Limits
            Try
                SetGridObject()
                mDueLimits = CType(mDueLimits.Save, DueLimits)
                Session("mDueLimits") = mDueLimits
                DataFieldBind()
                ControlVisibility()
            Catch ex As Exception
                '
            End Try
        Else
            Session("ReportMaintenanceDetails") = ReportMaintenanceDetails
            Dim str As String
            str = "openledgersame('wfDueResult_Ajax.aspx?');"
            ScriptManager.RegisterStartupScript(Me, [GetType], "OpenScript", str, True)
        End If
    End Sub

    Private Sub SetExcel(ReportMaintenanceDetails As ReportMaintenanceDetailList, SearchingCriteria As ReportData, ReportName As String)
        Dim PeriodColumnsForExportToExcel As New List(Of String)
        Dim da As New ObjectAdapter
        Dim ds As New dsReportMaintenanceDetail

        Dim reportmaintdetailslist As List(Of ReportMaintenanceDetail) = New List(Of ReportMaintenanceDetail)

        reportmaintdetailslist = (From c As ReportMaintenanceDetail In ReportMaintenanceDetails.AsParallel
                                  Order By c.MinimumRemainingValue, c.RegNo, c.AssemblyType, c.Model, c.AssemblySerialNo, c.MaintenanceEvent, c.Description, c.PartNo
                                  Select c).ToList
        Session("ReportMaintenanceDetails") = ReportMaintenanceDetails
        Session("reportmaintdetailslist") = reportmaintdetailslist

        'da.Fill(ds, "ExcelReportMaintenanceDetailList", ReportMaintenanceDetails)
        da.Fill(ds, "ExcelReportMaintenanceDetailList", reportmaintdetailslist)
        da.Fill(ds, "ExcelReport", SearchingCriteria)


        Dim TskNoDetRemove As String = ""
        Dim TskNoDetReq As String = ""

        Dim MaintenanceInfo As String = ""
        Dim SinceNewAllExcelInfo As String = ""
        Dim AssDueAsofAllExcelInfo As String = ""
        Dim NoteInfo As String = ""
        If AppSettings("ClientCode") = "FIT" Then
            TskNoDetRemove = "MaintenanceOnExcel" 'removed in case of FIT Client
            MaintenanceInfo = "MaintenanceInfoExcel" 'Removed in case of other clients
            SinceNewAllExcelInfo = "SinceNewAllExcel" 'Removed in case of other clients
            AssDueAsofAllExcelInfo = "AssDueAsofAllExcel" 'Removed in case of other clients
            NoteInfo = "Note" 'Removed in case of other clients
            TskNoDetReq = "TaskNoExcel"  'Required in case of FIT Client
        Else
            TskNoDetRemove = "TaskNoExcel" 'Removed in case of other clients
            MaintenanceInfo = ""
            SinceNewAllExcelInfo = ""
            AssDueAsofAllExcelInfo = ""
            NoteInfo = ""
            TskNoDetReq = "MaintenanceOnExcel" 'required in case of other clients
        End If


        Dim columnToRemove As String() = {
                                                  "ID",
                                                  "Code",
                                                  "Name",
                                                  "Model",
                                                  "SerialNo",
                                                  "Freq1",
                                                  "Freq2",
                                                  "Freq3",
                                                  "ElapsedTime",
                                                  "ElapsedTime1",
                                                  "ElapsedTime2",
                                                  "RemainingTime",
                                                  "RemainingTime1",
                                                  "RemainingTime2",
                                                  "DueAsof",
                                                  "DueAsof1",
                                                  "DueAsof2",
                                                  "AssemblySerialNo",
                                                  "ComponentInfo",
                                                  "RegNo",
                                                  "AssemblyType",
                                                  "SinceNew",
                                                  "SinceNew1",
                                                  "SinceNew2",
                                                  "DoneAt",
                                                  "DoneAt1",
                                                  "DoneAt2",
                                                  "AssemblyModel",
                                                  "AssemblyTypeID",
                                                  "ATACode",
                                                  "InstalledAt1",
                                                  "InstalledAt2",
                                                  "TSO1",
                                                  "TSO2",
                                                  "RemoveAt1",
                                                  "RemoveAt2",
                                                  "ModificationNumber",
                                                  "Reference",
                                                  "DoneWONo",
                                                  "DetailID",
                                                  "Applicability",
                                                  "ComplianceRequirement",
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
                                                  "IssueDate",
                                                  "IsApplicable",
                                                  "MaintenanceTypeID",
                                                  "MaintenanceTypeName",
                                                  "IsLater",
                                                  "DueStatus",
                                                  "TimeSinceNew",
                                                  "ModelMonitorModCode",
                                                  "StatusTypeName",
                                                  "StatusMasterID",
                                                  "StatusID",
                                                  "TypeID",
                                                  "CompStatusID",
                                                  "AssemblyStatusID",
                                                  "DocumentTypeForID",
                                                  "MaintenanceInformation",
                                                  "LogBook",
                                                  "RemoveAt",
                                                  "DoneONValueForAssembly",
                                                  "MonitorTypeCode",
                                                  "ATAChapter",
                                                  "StatusTypeName",
                                                  "InstalledAt",
                                                  "TSN",
                                                  "TSO",
                                                  "InstalledAtDate",
                                                  "RemoveAtDate",
                                                  "DoneOnValue",
                                                  "Frequency",
                                                  "SinceNewAll",
                                                  "ElapsedAll",
                                                  "DoneAtAll",
                                                  "ExtensionAll",
                                                  "DueAsofAll",
                                                  "AssDueAsofAll",
                                                  "RemainingTimeAll",
                                                  "MaintenanceInfo",
                                                  "MaintenanceOn",
                                                  "EstDate",
                                                  "DoneOnDate",
                                                  "ModelEstimatedManHours",
                                                  "MaintenanceInformationExcel",
                                                  "MinimumRemainingValue",
                                                  "MachineID",
                                                  "ModelID",
                                                  "IsMaster",
                                                  "DiffCompInstDoneOnValue",
                                                  "MaintenanceInformationForExcel",
                                                  "ApplicabilityForExcel",
                                                  "NoteForExcel",
                                                  "SourceDoc",
                                                  "RecordID",
                                                  "Zone",
                                                  "Area",
                                                  "BinCardTotalQty",
                                                  "ServiceableStockQty",
                                                  "UnserviceableStockQty",
                                                  "EROQtyForMaterialMgmtReport",
                                                  "ERONosForMaterialMgmtReport",
                                                  "POQtyForMaterialMgmtReport",
                                                  "PONosForMaterialMgmtReport",
                                                  "POQtyNosForMaterialMgmtReport",
                                                  "EROQtyNosForMaterialMgmtReport",
                                                  "IsRII",
                                                  "EffectiveFromAll",
                                                  "Estimated Date",
                                                  "WO. No.",
                                                  "Description",
                                                  "WONumber",
                                                  "MonitorType",
                                                  "MaintenanceEvent",
                                                  "LinkedMaintenanceActivityCount",
                                                  "ThresholdAccordingToTypeIDForExcel",
                                                  "FrequencyAccordingToTypeIDForExcel",
                                                  "DueAsOfAssemblyOrCompForExcel",
                                                  "DueAsOfAirframeForExcel",
                                                  "RemainingForExcel",
                                                  "Req.No.",
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
                                                  "TSNHours",
                                                  "SinceNewDate",
                                                  "SinceNewLandings",
                                                  "CSNCycles",
                                                  "InstCompHours",
                                                  "InstCompStartDate",
                                                  "InstCompLandings",
                                                  "InstCompCycles",
                                                  "AssemblyInstHours",
                                                  "AssemblyInstStartDate",
                                                  "AssemblyInstLandings",
                                                  "AssemblyInstCycles", "PartMonitorCode",
                                                  "PartDesc",
                                                  "MonitorTypeWithCode",
                                                  "MethodOfCompliance",
                                                  "DescriptionSourceDocForExcel",
                                                  "PartNoSerialNoforExcel",
                                                  "TSO1ForExcel",
                                                  "TSOForExcel",
                                                  "InstalledAtForExcel",
                                                  "Freq1ForExcel",
                                                  "TSNForExcel",
                                                  "DoneOnValueForExcel",
                                                  "RemainingTimeForExcel",
                                                  "DueAsOfForExcel",
                                                  "TaskNo",
                                                  TskNoDetRemove,
                                                  "TaskReferenceForExcel",
                                                  "Skill",
                                                  "SkillID",
                                                  MaintenanceInfo,
                                                  SinceNewAllExcelInfo,
                                                  AssDueAsofAllExcelInfo,
                                                  NoteInfo
                                    }

        For i As Integer = 0 To columnToRemove.Length - 1
            If columnToRemove(i) <> "" Then
                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns.Remove(columnToRemove(i))
                End If
            End If

        Next


        Dim columnscnt As Integer = ds.Tables("ExcelReportMaintenanceDetailList").Columns.Count

        ds.Tables("ExcelReportMaintenanceDetailList").Columns(TskNoDetReq).SetOrdinal(0)
        If AppSettings("ClientCode") = "FIT" Then
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("DescriptionForExcel").SetOrdinal(1)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("ReferenceForExcel").SetOrdinal(2)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("MaintenanceActivityType").SetOrdinal(3)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("PartNo").SetOrdinal(4)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("CompSerialNo").SetOrdinal(5)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("Position").SetOrdinal(6)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("FrequencyExcel").SetOrdinal(7)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("ElapsedAllExcel").SetOrdinal(8)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("EffectiveFromAllExcel").SetOrdinal(9)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("DoneAtAllExcel").SetOrdinal(10)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("ExtensionAllExcel").SetOrdinal(11)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("DueAsofAllExcel").SetOrdinal(12)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("RemainingTimeAllExcel").SetOrdinal(13)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("EstimatedDate").SetOrdinal(14)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("Remark").SetOrdinal(15)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("WONoExcel").SetOrdinal(16)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("ReqNumber").SetOrdinal(17)



        Else
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("MaintenanceInfoExcel").SetOrdinal(1)
            'Added By Vikrant On 08-Jul-2020 For ALL08072020-1
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("MaintenanceActivityType").SetOrdinal(2)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("DescriptionForExcel").SetOrdinal(3)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("ReferenceForExcel").SetOrdinal(4)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("PartNo").SetOrdinal(5)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("CompSerialNo").SetOrdinal(6)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("Position").SetOrdinal(7)
            'End
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("FrequencyExcel").SetOrdinal(8)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("SinceNewAllExcel").SetOrdinal(9)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("ElapsedAllExcel").SetOrdinal(10)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("EffectiveFromAllExcel").SetOrdinal(11)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("DoneAtAllExcel").SetOrdinal(12)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("ExtensionAllExcel").SetOrdinal(13)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("DueAsofAllExcel").SetOrdinal(14)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("AssDueAsofAllExcel").SetOrdinal(15)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("RemainingTimeAllExcel").SetOrdinal(16)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("EstimatedDate").SetOrdinal(17)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("Note").SetOrdinal(18)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("Remark").SetOrdinal(19)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("WONoExcel").SetOrdinal(20)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("ReqNumber").SetOrdinal(21)
        End If
        'set Column Sequence



        Dim ColumnName As String = String.Empty
        For i As Integer = 0 To ds.Tables("ExcelReportMaintenanceDetailList").Columns.Count - 1
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "ModificationNumber" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Directive No"
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "FrequencyExcel" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Frequency"
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "SinceNewAllExcel" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Since New"
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "ElapsedAllExcel" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Elapsed"
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "DueAsofAllExcel" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Due At"
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "AssDueAsofAllExcel" Then
                If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
                    ColumnName = "Due At Airframe"
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = ColumnName
                Else
                    ColumnName = "Due At Assembly"
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = ColumnName
                End If

            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "RemainingTimeAllExcel" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Remaining"
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "DoneAtAllExcel" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Done At"
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "EffectiveFromAllExcel" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Effective From"
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "ExtensionAllExcel" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Extension"
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "MaintenanceOnExcel" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Maintenance On"
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "MaintenanceInfoExcel" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Maintenance Info"
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "EstimatedDate" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Estimated Date"
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "WONoExcel" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "WO. No."
            End If
            'REQ
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "ReqNumber" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Req. No."
            End If
            'End
            'Added By Vikrant On 08-Jul-2020 For ALL08072020-1
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "MaintenanceActivityType" Then

                If AppSettings("ClientCode") = "FIT" Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Task Type"
                Else
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Maintenance Activity Type"
                End If

            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "ReferenceForExcel" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Reference"
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "PartNo" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Part No."
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "CompSerialNo" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Comp. Serial No."
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "DescriptionForExcel" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Description"
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "TaskNoExcel" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Task No"
            End If
            'End
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
                                                 "SearchStr7",
                                                 "SearchStr9",
                                                 "ProductVersion",
                                                 "SINote",
                                                 "CurrencyName",
                                                 "CurrencySymbol",
                                                 "SearchStr10",
                                                 "SearchStr4",
                                                 "SearchStr12",
                                                 "SearchStr11",
                                                 "ShortName",
                                                 "SearchStr15",
                                                 "SearchStr16",
                                                 "SearchStr17",
                                                 "SearchStr18",
                                                 "SearchStr19",
                                                 "SearchStr20",
                                                 "SearchStr21",
                                                 "SearchStr22",
                                                 "SearchStr23",
                                                 "SearchStr24",
                                                 "SearchStr25",
                                                 "SearchStr26",
                                                 "SearchStr27",
                                                 "SearchStr28",
                                                 "SearchStr29",
                                                 "SearchStr30",
                                                 "SearchStr31",
                                                 "SearchStr32",
                                                 "SearchStr33",
                                                 "SearchStr34",
                                                 "SearchStr35",
                                                 "SearchStr36",
                                                 "SearchStr37",
                                                 "SearchStr38",
                                                 "SearchStr39",
                                                 "SearchStr40",
                                                 "SearchStr41",
                                                 "SearchStr42",
                                                 "SearchStr43",
                                                 "SearchStr44",
                                                 "SearchStr45",
                                                 "SearchStr46",
                                                 "SearchStr47",
                                                 "SearchStr48",
                                                 "SearchStr49",
                                                 "SearchStr50",
                                                 "ApprovalNo",
                                                 "SearchStr51",
                                                 "SearchStr52",
                                                 "SearchStr53",
                                                 "SearchStr54",
                                                 "SearchStr55",
                                                 "SearchStr56",
                                                 "SearchStr57",
                                                 "SearchStr58",
                                                 "SearchStr59",
                                                 "SearchStr60",
                                                 "SearchStr61",
                                                 "SearchStr62",
                                                 "SearchStr63",
                                                 "SearchStr64",
                                                 "SearchStr65",
                                                 "SearchStr66",
                                                 "SearchStr67",
                                                 "SearchStr68",
                                                 "SearchStr69",
                                                 "SearchStr70",
                                                 "SearchStr71",
                                                 "SearchStr72",
                                                 "SearchStr73",
                                                 "SearchStr74",
                                                 "SearchStr75",
                                                 "SearchStr76",
                                                 "SearchStr77",
                                                 "SearchStr78",
                                                 "SearchStr79",
                                                 "SearchStr80",
                                                 "SearchStr81",
                                                 "SearchStr82",
                                                 "SearchStr83",
                                                 "SearchStr84",
                                                 "SearchStr85",
                                                 "SearchStr86",
                                                 "SearchStr87",
                                                 "SearchStr88",
                                                 "SearchStr89",
                                                 "SearchStr90",
                                                 "SearchStr91",
                                                 "SearchStr92",
                                                 "SearchStr93",
                                                 "SearchStr94",
                                                 "SearchStr95",
                                                 "SearchStr96",
                                                 "SearchStr97",
                                                 "SearchStr98",
                                                 "SearchStr99",
                                                 "SearchStr100"
                                              }

        For i As Integer = 0 To columnToRemoveCriteria.Length - 1
            If ds.Tables("ExcelReport").Columns.Contains(columnToRemoveCriteria(i)) Then
                ds.Tables("ExcelReport").Columns.Remove(columnToRemoveCriteria(i))
            End If
        Next

        'set Column Sequence
        ds.Tables("ExcelReport").Columns("SearchStr14").SetOrdinal(0)
        ds.Tables("ExcelReport").Columns("SearchStr13").SetOrdinal(1)
        ds.Tables("ExcelReport").Columns("SearchStr3").SetOrdinal(2)
        ds.Tables("ExcelReport").Columns("SearchStr1").SetOrdinal(3)
        ds.Tables("ExcelReport").Columns("SearchStr2").SetOrdinal(4)
        ds.Tables("ExcelReport").Columns("SearchStr6").SetOrdinal(5)
        ds.Tables("ExcelReport").Columns("SearchStr8").SetOrdinal(6)


        For i As Integer = 0 To ds.Tables("ExcelReport").Columns.Count - 1
            If ds.Tables("ExcelReport").Columns(i).ColumnName = "SearchStr1" Then
                ds.Tables("ExcelReport").Columns(i).ColumnName = "Due Limit"
            End If
            If ds.Tables("ExcelReport").Columns(i).ColumnName = "SearchStr2" Then
                ds.Tables("ExcelReport").Columns(i).ColumnName = "Average Months"
            End If
            If ds.Tables("ExcelReport").Columns(i).ColumnName = "SearchStr13" Then
                ds.Tables("ExcelReport").Columns(i).ColumnName = "Reg No."
            End If
            If ds.Tables("ExcelReport").Columns(i).ColumnName = "SearchStr14" Then
                ds.Tables("ExcelReport").Columns(i).ColumnName = "As On Date"
            End If
            If ds.Tables("ExcelReport").Columns(i).ColumnName = "SearchStr3" Then
                ds.Tables("ExcelReport").Columns(i).ColumnName = "Assembly"
            End If
            If ds.Tables("ExcelReport").Columns(i).ColumnName = "SearchStr6" Then
                ds.Tables("ExcelReport").Columns(i).ColumnName = "Flight Log Updated Till"
            End If
            If ds.Tables("ExcelReport").Columns(i).ColumnName = "SearchStr8" Then
                ds.Tables("ExcelReport").Columns(i).ColumnName = "Last Maintenance Done On"
            End If
        Next
        'Dim dataview As DataView = ds.Tables("ExcelReportMaintenanceDetailList").DefaultView
        'dataview.Sort = "MinimumRemainingValue"


        Dim dsNew As New DataSet
        dsNew.Clear()

        dsNew.Merge(ds.Tables("ExcelReport"))
        dsNew.Merge(ds.Tables("ExcelReportMaintenanceDetailList"))


        dsNew.Tables("ExcelReport").TableName = "Searching Criteria"
        dsNew.Tables("ExcelReportMaintenanceDetailList").TableName = ReportName
        Session("DataTableToBeFormattedForExportToExcel") = ReportName
        Session("ExcelFileName") = ReportName.Replace("/", " ")
        PeriodColumnsForExportToExcel.AddRange(New String() {"Frequency", "Since New", "Elapsed", "Remaining", "Due At", "Done At", "Effective From", "AssemblySerialNo", "Maintenance On", ColumnName, "Extension", "Maintenance Info"})
        Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
        Session("dsNew") = dsNew
        ScriptManager.RegisterStartupScript(Me, [GetType](), "openFilel", "openFile();", True)
        'Added by Prashant on 19-Jan-2021
        MarkLog(Action.Print, "Due-PeriodWise", "Export To Excel " + mEventLogDetails, ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub

    Private Sub SetReport(Optional ByMail As Boolean = False,
                          Optional ByExcel As Boolean = False,
                          Optional IsPreviewClicked As Boolean = False,
                          Optional IsMaintStmt As Boolean = False)

        Try

            ReportMaintenanceDetails = New ReportMaintenanceDetailList
            ReportStatusList = New rptStatusList
            Dim dataAdapter As New ObjectAdapter
            Dim dataSet As New dsReportMaintenanceDetail
            Dim rptDueDetail As Engine.ReportClass
            Dim rptMachineCertificates As MachineCertificateList
            Dim rptSnagCorrectiveActionListForDue As MELSnagCorrectiveActionListForDue  'Changed By Saylee on 19-Oct-2010
            Dim mCompanyDetail As New CompanyDetail
            Dim SearchStr As String = ""
            Dim SearchStr1 As String
            Dim SearchStr6 As String = ""
            Dim SearchStr8 As String = ""
            Dim mPerDayLimit As PerDayLimit
            Dim OperatorName As String = ""
            Dim ReportName As String
            Dim ReportNameForPDF As String

            'Added by Ajay 14-08-2023
            If AppSettings("ShowMaintenanceForNewClients") = "True" Then

                If cmbAircraft.SelectedIndex > -1 Then

                    mLastAMPRef = LastMPDAMPRef.GetLastMPDAMPRefForMachine(MachineID:=New Guid(cmbAircraft.SelectedValue.ToLower))
                    Session("mLastAMPRef") = mLastAMPRef

                    If (mLastAMPRef.AMPNo <> "") Then searchstr20 = "AMP No.: " + mLastAMPRef.AMPNo +
                                                                    ", Rev No.: " + mLastAMPRef.RevNo +
                                                                    ", Dated: " + mLastAMPRef.FromDateFormatted


                Else
                    searchstr20 = ""
                End If

            End If

            SetValues()
            mDueLimits = CType(mDueLimits.Save, DueLimits)
            Session("mDueLimits") = mDueLimits
            ReportDetail(mIsExcel, IsPreviewClicked)

            If rbdDueLimits.Checked = True Then

                For Each mDueLimit In mDueLimits

                    If CDec(Val(mDueLimit.PeriodLimit)) >= 0 Then

                        If SearchStr = "" Then
                            SearchStr = "For Next" & " " & SearchStr & " " & mDueLimit.PeriodLimit & " " & mDueLimit.PeriodName
                        Else
                            SearchStr = SearchStr & ", " & mDueLimit.PeriodLimit & " " & mDueLimit.PeriodName
                        End If

                    End If

                Next

            Else
                SearchStr = "For Next" & " " & CDec(Val(txtPercentage.Text)).ToString & "% of Frequency"
            End If

            'Added By Rajnish on 26-11-2007
            SearchStr = SearchStr & ", " & "As On Date:" & txtFromDate.Text.Trim
            '------------------------------

            'Code added By Deven on 11-04-2008 ====================
            If rbdSpecifyValues.Checked = True Then

                For Each mPerDayLimit In mPerDayLimits

                    If CDec(Val(mPerDayLimit.PeriodLimit)) >= 0 Then

                        If SearchStr1 = "" Then
                            SearchStr1 = "Estimated Due Date as" & " " & SearchStr1 & " " & mPerDayLimit.PeriodLimit & " " & mPerDayLimit.PeriodName
                        Else
                            SearchStr1 = SearchStr1 & ", " & mPerDayLimit.PeriodLimit & " " & mPerDayLimit.PeriodName
                        End If

                    End If

                Next

                SearchStr1 = SearchStr1 & " per Day "
            Else

                If CDec(Val(txtAvgMnths.Text)).ToString <> "" Then
                    SearchStr1 = "Estimated Due Date as Per Average of Last" & " " & CDec(Val(txtAvgMnths.Text)).ToString & " Months"
                Else
                    SearchStr1 = ""
                End If

            End If
            '===========================================

            'Code Added By Deven on 07/04/2008------------

            If DueType = 1 Then

                If AppSettings("TimeFormat") = "HH:mm" Or AppSettings("TimeFormat") = "hh:mm" Then

                    rptSnagCorrectiveActionListForDue = MELSnagCorrectiveActionListForDue.
                                                            GetMELSnagCorrectiveActionListForDue(AsonDate,
                                                                                                 New Guid(cmbAircraft.SelectedValue.ToString),
                                                                                                 Guid.Empty,
                                                                                                 0,
                                                                                                 0,
                                                                                                 "HH:mm")
                Else

                    rptSnagCorrectiveActionListForDue = MELSnagCorrectiveActionListForDue.
                                                            GetMELSnagCorrectiveActionListForDue(AsonDate,
                                                                                                 New Guid(cmbAircraft.SelectedValue.ToString),
                                                                                                 Guid.Empty,
                                                                                                 0,
                                                                                                 0)
                End If

                If Not cmbAircraft.SelectedItem.ToString = "(ALL)" Then

                    If ByMail = True Then
                        SetGridObject() ' to set PerDayLimitForDaysPeriod value if is For Mail
                    End If

                    'Added by Saylee on 25-Sep-2008 for showing Due Certificates
                    rptMachineCertificates = MachineCertificateList.GetMachineCertificateList(New Guid(cmbAircraft.SelectedValue.ToString),
                                                                                              AsonDate,
                                                                                              IsForDue:=True,
                                                                                              Days:=IIf(AppSettings("ClientCode") = "Heligo",
                                                                                                        -1,
                                                                                                        PerDayLimitForDaysPeriod))

                    If AppSettings("ShowMaintenanceForNewClients") = "True" AndAlso (Not AppSettings("ClientCode") = "ARA") Then

                        SearchStr1 = ""
                        rptDueDetail = New crDueReportDetailLandscapePerAircraft

                    ElseIf cmbFormat.SelectedIndex = 0 Then  'Format 1

                        If rptMachineCertificates.Count = 0 Then

                            If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "Indamer")) Then
                                rptDueDetail = New crDueReportDetailLandscapePerAircraftIndamar 'This change is applied to Indamer

                            ElseIf ((Not AppSettings("ClientCode") Is Nothing) AndAlso
                                    (((AppSettings("ClientCode") = "TAAL" Or
                                       AppSettings("ClientCode") = "GlobalJet")))) Or
                                       ((Not AppSettings("ClientCode") Is Nothing) AndAlso
                                        ((AppSettings("ClientCode") = "KamAir"))) Then

                                If rptSnagCorrectiveActionListForDue.Count <> 0 Then
                                    rptDueDetail = New crDueReportDetailLandscapePerAircraftTaal
                                Else
                                    rptDueDetail = New crDueReportDetailLandscapePerAircraftWithoutSnagTaal
                                End If

                            ElseIf ((Not AppSettings("ClientCode") Is Nothing) AndAlso
                                   (AppSettings("ClientCode") = "Deccan" Or
                                    AppSettings("ClientCode") = "ADeccan" Or
                                    AppSettings("ClientCode") = "IIC" Or
                                    AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022

                                If AppSettings("ClientCode") = "SPZ" Then ''Added By Prashant on 23-Aug-2022 To Rmove AME Signature column
                                    rptDueDetail = New crDueReportDetailLandscapePerAircraftForSparzana  '--------------------------------
                                Else
                                    rptDueDetail = New crDueReportDetailLandscapePerAircraftForDeccan '--------------------------------
                                End If

                            ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso
                                   (AppSettings("ClientCode") = "Heligo") Then  'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
                                rptDueDetail = New crDueReportDetailLandscapePerAircraftHeligo  'This change is applied to Heligo

                            ElseIf (AppSettings("ClientCode") = "BA" Or
                                    AppSettings("ClientCode") = "PAS" Or
                                    AppSettings("ClientCode") = "YA" Or
                                    AppSettings("ClientCode") = "TA") Then

                                rptDueDetail = New crDueReportDetailLandscapePerAircraftBA
                            ElseIf (AppSettings("ClientCode") = "Novo") Then 'Same Copy as YA/TA with Assembly Info in ReportHeader instead of Page Header
                                rptDueDetail = New crDueReportDetailLandscapePerAircraftNOVO
                            ElseIf (AppSettings("ClientCode") = "EIH") Then
                                rptDueDetail = New crDueReportDetailLandscapePerAircraftEIH
                            ElseIf (AppSettings("ClientCode") = "Suhan") Then  'Added by Saylee on 27-Jul-2018 for Suhan27072018
                                rptDueDetail = New crDueReportDetailLandscapePerAircraftSUHAN
                            ElseIf AppSettings("ClientCode") = "UHPL" Then       'Added by Shital on 26-Apr-2019  (UHPL26042019 -Forecast Due Report New Format)
                                rptDueDetail = New crDueReportDetailLandscapePerAircraftUHPL
                            ElseIf AppSettings("ClientCode") = "STR" Then
                                rptDueDetail = New crDueReportDetailLandscapePerAircraftSTR
                            Else
                                rptDueDetail = New crDueReportDetailLandscapePerAircraft
                            End If

                        Else

                            If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "Indamer")) Then
                                rptDueDetail = New crDueReportDetailLandscapePerAircraftCertificatesIndamar 'This change is applied to Indamer and Heligo
                            ElseIf ((Not AppSettings("ClientCode") Is Nothing) AndAlso
                                   (((AppSettings("ClientCode") = "TAAL" Or
                                      AppSettings("ClientCode") = "GlobalJet")))) Or
                                      ((Not AppSettings("ClientCode") Is Nothing) AndAlso
                                       ((AppSettings("ClientCode") = "KamAir"))) Then

                                If rptSnagCorrectiveActionListForDue.Count <> 0 Then
                                    rptDueDetail = New crDueReportDetailLandscapePerAircraftCertificatesTaal
                                Else
                                    rptDueDetail = New crDueReportDetailLandscapePerAircraftCertificatesWithoutSnagTaal
                                End If

                            ElseIf ((Not AppSettings("ClientCode") Is Nothing) AndAlso
                                    (AppSettings("ClientCode") = "Deccan" Or
                                     AppSettings("ClientCode") = "ADeccan" Or
                                     AppSettings("ClientCode") = "IIC" Or
                                     AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022

                                rptDueDetail = New crDueReportDetailLandscapePerAircraftCertificatesForDeccan
                                If AppSettings("ClientCode") = "SPZ" Then
                                    rptDueDetail = New crDueReportDetailLandscapePerAircraftCertificatesForSparzana
                                End If

                            ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso
                                   (AppSettings("ClientCode") = "Heligo") Then  'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
                                rptDueDetail = New crDueReportDetailLandscapePerAircraftCertificatesHeligo  'This change is applied to Heligo
                            ElseIf (AppSettings("ClientCode") = "BA" Or
                                    AppSettings("ClientCode") = "PAS" Or
                                    AppSettings("ClientCode") = "YA" Or
                                    AppSettings("ClientCode") = "TA") Then
                                rptDueDetail = New crDueReportDetailLandscapePerAircraftCertificatesBA
                            ElseIf (AppSettings("ClientCode") = "Novo") Then 'Same Copy as YA/TA with Assembly Info in ReportHeader instead of Page Header
                                rptDueDetail = New crDueReportDetailLandscapePerAircraftCertificatesNOVO
                            ElseIf (AppSettings("ClientCode") = "EIH") Then
                                rptDueDetail = New crDueReportDetailLandscapePerAircraftCertificatesEIH
                            ElseIf (AppSettings("ClientCode") = "Suhan") Then   'Added by Saylee on 27-Jul-2018 for Suhan27072018
                                rptDueDetail = New crDueReportDetailLandscapePerAircraftCertificatesSUHAN
                            ElseIf AppSettings("ClientCode") = "UHPL" Then       'Added by Shital on 26-Apr-2019  (UHPL26042019 -Forecast Due Report New Format)
                                rptDueDetail = New crDueReportDetailLandscapePerAircraftCertificatesUHPL
                            ElseIf AppSettings("ClientCode") = "STR" Then
                                rptDueDetail = New crDueReportDetailLandscapePerAircraftCertificatesSTR
                            ElseIf AppSettings("ClientCode") = "GEP" Then 'Added by Prashant 12-Nov-2019 GEP Do not want show Certificates so above format taken
                                rptDueDetail = New crDueReportDetailLandscapePerAircraft
                            Else
                                rptDueDetail = New crDueReportDetailLandscapePerAircraftCertificates
                            End If

                        End If

                    ElseIf cmbFormat.SelectedIndex = 1 Then  'Format 2

                        If rptMachineCertificates.Count = 0 Then

                            If (Not AppSettings("ClientCode") Is Nothing) AndAlso
                                (AppSettings("ClientCode") = "Heligo" Or
                                 AppSettings("ClientCode") = "UHPL") Then  'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
                                rptDueDetail = New crDueDetailAircraftWithCommentHeligo
                            ElseIf (AppSettings("ClientCode") = "BA" Or
                                    AppSettings("ClientCode") = "PAS" Or
                                    AppSettings("ClientCode") = "YA" Or
                                    AppSettings("ClientCode") = "TA") Then
                                rptDueDetail = New crDueDetailLandscapePerAircraftWithCommentBA
                            ElseIf (AppSettings("ClientCode") = "Novo") Then 'Same Copy as YA/TA with Assembly Info in Report Header instead of Page Header
                                rptDueDetail = New crDueReportDetailLandscapePerAircraftNOVO
                            ElseIf AppSettings("ClientCode") = "STR" Then
                                rptDueDetail = New crDueDetailLandscapePerAircraftWithCommentSTR
                            ElseIf AppSettings("ClientCode") = "ARA" Then
                                rptDueDetail = New crDueDetailAircraftWithCommentARAirways 'Added by Harsh Sugandhi on 3rd Feb 2025 for FLYPAL-2176 CALLOUT Report for AR Airways                                
                            Else
                                rptDueDetail = New crDueDetailLandscapePerAircraftWithComment
                            End If

                        Else

                            If (Not AppSettings("ClientCode") Is Nothing) AndAlso
                                (AppSettings("ClientCode") = "Heligo" Or
                                 AppSettings("ClientCode") = "UHPL") Then  'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
                                rptDueDetail = New crDueDetailPerAircraftCertificatesWithCommentHeligo
                            ElseIf (AppSettings("ClientCode") = "BA" Or
                                    AppSettings("ClientCode") = "PAS" Or
                                    AppSettings("ClientCode") = "YA" Or
                                    AppSettings("ClientCode") = "TA") Then
                                rptDueDetail = New crDueDetailPerAircraftCertificatesWithCommentBA
                            ElseIf (AppSettings("ClientCode") = "Novo") Then 'Same Copy as YA/TA with Assembly Info in Report Header instead of Page Header
                                rptDueDetail = New crDueDetailPerAircraftCertificatesWithCommentNOVO
                            ElseIf AppSettings("ClientCode") = "STR" Then
                                rptDueDetail = New crDueDetailPerAircraftCertificatesWithCommentSTR
                            ElseIf AppSettings("ClientCode") = "GEP" Then 'Added by Prashant 12-Nov-2019 GEP Do not want show Certificates so above format taken
                                rptDueDetail = New crDueDetailLandscapePerAircraftWithComment
                            ElseIf AppSettings("ClientCode") = "ARA" Then
                                rptDueDetail = New crDueDetailAircraftWithCommentARAirways 'Added by Harsh Sugandhi on 3rd Feb 2025 for FLYPAL-2176 CALLOUT Report for AR Airways
                            Else
                                rptDueDetail = New crDueDetailPerAircraftCertificatesWithComment
                            End If

                        End If

                    ElseIf cmbFormat.SelectedIndex = 2 Then 'Format 3 For Tata Steel

                        If AppSettings("ClientCode") = "YA" Then
                            If rptMachineCertificates.Count = 0 Then
                                rptDueDetail = New crDueReportDetailLandscapePerAircraftSortByEstimatedDate
                            Else
                                rptDueDetail = New crDueReportDetailLandscapePerAircraftCertificatesSortByEstimatedDate
                            End If
                        Else
                            rptDueDetail = New crDueReportDetailLandscapeEnlargeCopyForTS
                        End If

                    End If

                Else

                    If cmbFormat.SelectedIndex = 0 Then  'Format 1
                        rptDueDetail = New crDueReportDetailLandscape
                    ElseIf cmbFormat.SelectedIndex = 1 Then 'Format 2
                        rptDueDetail = New crDueReportDetailLandscapeWithComment
                    ElseIf cmbFormat.SelectedIndex = 2 Then 'Format 3 For Tata Steel
                        rptDueDetail = New crDueReportDetailLandscapeEnlargeCopyForTS
                    End If

                End If

                ''Added by Saylee on 5-May-2022, for Shaurya
                If IsMaintStmt Then
                    rptDueDetail = New crDueReportMaintStatementShaurya
                End If

                'NextCode:
                If (AppSettings("ClientCode") IsNot Nothing) AndAlso
                   (AppSettings("ClientCode") = "Deccan" Or
                    AppSettings("ClientCode") = "ADeccan" Or
                    AppSettings("ClientCode") = "IIC" Or
                    AppSettings("ClientCode") = "SPZ") Then

                    If AppSettings("ClientCode") = "SPZ" Then

                        If (cmbAircraft.SelectedItem.Text = "(ALL)") Or (cmbAircraft.SelectedItem.Text = "(SELECT)") Then
                            ReportNameForPDF = "Monthly Due List"
                            ReportName = "Monthly Due List"
                        Else

                            ReportNameForPDF = "Monthly Due List"
                            ReportName = "Monthly Due List Number " +
                                         "__________________" + " / " +
                                         cmbAircraft.SelectedItem.Text + " / " +
                                         MonthName(Month(New SmartDate(txtFromDate.Text.Trim).FormattedText), True).ToString +
                                         "." + " / " +
                                         Year(New SmartDate(txtFromDate.Text.Trim).FormattedText).ToString + " ."

                        End If

                    Else

                        If (cmbAircraft.SelectedItem.Text = "(ALL)") Or (cmbAircraft.SelectedItem.Text = "(SELECT)") Then
                            ReportNameForPDF = "Work Order List"
                            ReportName = "Work Order List"
                        Else

                            ReportNameForPDF = "Work Order List"
                            ReportName = "Work Order List Number " +
                                         "__________________" + " / " +
                                         cmbAircraft.SelectedItem.Text + " / " +
                                         MonthName(Month(New SmartDate(txtFromDate.Text.Trim).FormattedText), True).ToString +
                                         "." + " / " +
                                         Year(New SmartDate(txtFromDate.Text.Trim).FormattedText).ToString + " ."

                        End If

                    End If

                ElseIf (AppSettings("ClientCode") = "Heligo" Or
                        AppSettings("ClientCode") = "ARA") Then

                    ReportName = IIf(cmbFormat.SelectedIndex = 1,
                                     IIf(AppSettings("ClientCode") = "ARA",
                                         "CAMO CALL OUT",
                                         "Weekly Call Out"),
                                     "Maintenance Forecast")

                    ReportNameForPDF = IIf(cmbFormat.SelectedIndex = 1,
                                           IIf(AppSettings("ClientCode") = "ARA",
                                               "CAMO CALL OUT",
                                               "Weekly Call Out"),
                                           "Maintenance Forecast")

                Else

                    ReportName = IIf(AppSettings("ShowMaintenanceForNewClients") = "True",
                                     "Maintenance Forecast",
                                     "Maintenance Status Report")

                    ReportNameForPDF = IIf(AppSettings("ShowMaintenanceForNewClients") = "True",
                                           "Maintenance Forecast",
                                           "Maintenance Status Report")

                End If

            End If

            Dim x As String
            Dim LastFlownDate As String = ""
            Dim LastMaintenanceActivityDate As String = ""
            Dim mMaxLogNo As MaxLogNo = MaxLogNo.GetMaxLogNo(AsonDate, New Guid(MachineName), New Guid(AssemblyName))

            If mMaxLogNo.Count <> 0 Then
                LastFlownDate = mMaxLogNo(0).LogDate.ToString 'Last Flight Log Date
                x = mMaxLogNo(0).LogDate.ToString
            Else
                LastFlownDate = ""
                x = txtFromDate.Text.Trim
            End If

            ''Last Maintenance Activity
            If Not cmbAircraft.SelectedItem.ToString = "(ALL)" Then

                Dim mLastMachineMaintenanceActivity As LastMachineMaintenanceActivity = LastMachineMaintenanceActivity.
                                                                                            GetLastMaintenanceActivity(AsonDate,
                                                                                                                       New Guid(MachineName),
                                                                                                                       New Guid(AssemblyName))

                If Not mLastMachineMaintenanceActivity.ID.Equals(Guid.Empty) Then
                    LastMaintenanceActivityDate = ", Last Maintenance Done on  " + "( " + mLastMachineMaintenanceActivity.Date.ToString + " )"
                    SearchStr8 = mLastMachineMaintenanceActivity.Date.ToString
                End If

            End If

            If ((AppSettings("ClientCode") IsNot Nothing) AndAlso
                (AppSettings("ClientCode") = "Deccan" Or
                 AppSettings("ClientCode") = "ADeccan" Or
                 AppSettings("ClientCode") = "IIC" Or
                 AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022

                SearchStr6 = "Flying Hours updated till " +
                             "( " + LastFlownDate + " ) " +
                             LastMaintenanceActivityDate +
                             " & Work Order Number - _______________________"

            Else
                SearchStr6 = LastFlownDate 'Mostly on Heligo Report
            End If

            'Added by Vikrant on 11-Aug-2011
            If (AppSettings("ClientCode") IsNot Nothing) AndAlso ((AppSettings("ClientCode") = "Indamer")) Then

                Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue))
                If cmbAircraft.SelectedIndex > -1 Then
                    If mMachineOperatorName.OperatorName <> "" Then OperatorName = mMachineOperatorName.OperatorName
                End If

            ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso
                   (AppSettings("ClientCode") = "Deccan" Or
                    AppSettings("ClientCode") = "ADeccan" Or
                    AppSettings("ClientCode") = "IIC" Or
                    AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022

                OperatorName = searchstr7

            End If

            Dim ReferenceNo As String = Trim(txtRefNo.Text) 'Added by Vikrant For HLI11102011 

            Dim Report As New ReportData(CompanyName:=mCompanyDetail.CompanyName,
                                         Address:=mCompanyDetail.Address,
                                         Tel1:=mCompanyDetail.Tel1,
                                         Tel2:=mCompanyDetail.Tel2,
                                         Fax:=mCompanyDetail.Fax,
                                         Email:=mCompanyDetail.Email,
                                         WebSite:=mCompanyDetail.WebSite,
                                         ReportName:=ReportName,
                                         SearchStr1:=SearchStr,
                                         SearchStr2:=SearchStr1,
                                         SearchStr3:=Assembly1,
                                         SearchStr4:=AppSettings("ClientCode"),
                                         SearchStr5:="Aircraft Is flown up to:  " & New SmartDate(x).FormattedText,
                                         ProductVersion:=mModuleList.Item("Due-PeriodWise").FormRevisionNo,
                                         SINote:=AppSettings("SINote"),
                                         SearchStr6:=SearchStr6,
                                         SearchStr7:=OperatorName,
                                         SearchStr8:=SearchStr8,
                                         SearchStr9:=ReferenceNo,
                                         SearchStr10:=AppSettings("Logo"),
                                         SearchStr11:=AppSettings("FormNo"),
                                         SearchStr12:=mModuleList.Item("Due-PeriodWise").FormRevisionNo,
                                         SearchStr13:=Aircraft,
                                         SearchStr14:=txtFromDate.Text,
                                         SearchStr15:=(ReportMaintenanceDetails.Count + 1).ToString, 'Count of WO Jobs, Change done for Call-Out Report A.R Airways by Harsh Sugandhi on 5th feb 2025
                                         SearchStr16:=Val(Trim(txtForecastingLimit.Text)).ToString,
                                         SearchStr17:=CType(Session("AircraftAsOnDate"), String),
                                         SearchStr18:=mAssemblyList(1).ModelSerialNo,
                                         SearchStr19:=CustomerName,
                                         SearchStr20:=searchstr20,
                                         SearchStr21:=txtPubRefNo.Text.Trim)

            'Added by Shital on 09-Nov-2020 For Add Print in Preview Button
            Session("rptMachineCertificates") = rptMachineCertificates
            Session("rptSnagCorrectiveActionListForDue") = rptSnagCorrectiveActionListForDue
            Session("ReportName") = ReportName
            Session("searchstr") = SearchStr
            Session("searchstr1") = SearchStr1
            Session("Assembly1") = Assembly1
            Session("searchstr6") = SearchStr6
            Session("searchstr8") = SearchStr8
            Session("searchstr16") = Val(Trim(txtForecastingLimit.Text)).ToString
            Session("X") = x
            Session("OperatorName") = OperatorName
            Session("ReferenceNo") = ReferenceNo
            Session("ReportStatusList") = ReportStatusList
            'End

            If ByMail = False Then

                If ReportMaintenanceDetails.Count = 0 Then

                    MSGBoxCtrl.Show(MSGBox.Message_title.NoRecordFound,
                                    MSGBox.Message_text.NoRecordFound,
                                    "There Is no record for this search criteria",
                                    MsgBoxStyle.OkOnly,
                                    "")

                    Exit Sub

                Else
                    RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 719)
                End If

            End If

            If (ByMail = True And ReportMaintenanceDetails.Count <= 0) Then

                SendMailFile.SendMailFile(,
                                          Thread.CurrentPrincipal.Identity.Name,
                                          ReportName,
                                          ReportNameForPDF,
                                          "There Is no record for this search criteria.", "",
                                          Session("ToSendMailIDs"),
                                          Session("CcSendMailIDs"),
                                          "",
                                          True,
                                          Remark:=Session("SendMailRemark"),
                                          ReportGeneratedBy:=Session("ReportGenratedBy"),
                                          SmtpHost:=mModuleList.Item("Due-PeriodWise").SmtpHost,
                                          SmtpPort:=mModuleList.Item("Due-PeriodWise").SmtpPort,
                                          SmtpUser:=mModuleList.Item("Due-PeriodWise").SmtpUser,
                                          SmtpPassword:=mModuleList.Item("Due-PeriodWise").SmtpPassword)

                Exit Sub

            End If

            '11-Sep-2008-------------------------------
            If Not mIsPreview Then

                dataSet.Clear()
                Dim mrptImage As rptImage = rptImage.GetImage(dataSet)
                dataAdapter.Fill(dataSet, ReportMaintenanceDetails)

                'Added by Saylee on 25-Sep-2008 for showing Due Certificates
                If DueType = 1 Then

                    If Not cmbAircraft.SelectedItem.ToString = "(ALL)" Then
                        If rptMachineCertificates.Count <> 0 Then dataAdapter.Fill(dataSet, rptMachineCertificates)
                    Else

                        If MachineName = "{00000000-0000-0000-0000-000000000000}" Then

                            'Added by Saylee on 02-Aug-2018 for showing Due Certificates, when "ALL", ALL03082018
                            If ByMail = True Then
                                SetGridObject() ' to set PerDayLimitForDaysPeriod value if is For Mail
                            End If

                            rptMachineCertificates = MachineCertificateList.GetMachineCertificateList(Guid.Empty,
                                                                                                      AsonDate,
                                                                                                      IsForDue:=True,
                                                                                                      Days:=IIf(AppSettings("ClientCode") = "Heligo",
                                                                                                                -1,
                                                                                                                PerDayLimitForDaysPeriod))
                            If rptMachineCertificates.Count <> 0 Then dataAdapter.Fill(dataSet, rptMachineCertificates)

                        End If

                    End If

                End If

                dataAdapter.Fill(dataSet, Report)
                dataAdapter.Fill(dataSet, ReportStatusList)
                dataAdapter.Fill(dataSet, rptSnagCorrectiveActionListForDue) 'Added By Prashant 20-Nov-2009
                dataAdapter.Fill(dataSet, mrptImage)
                rptDueDetail.SetDataSource(dataSet)
                Session("CrystalReport") = rptDueDetail

                'Added by Saylee on 10-Oct-2018 for ALL10102018, for Pdf merger of Due & MEL report
                If chkMEL.Checked Then
                    'Here use PDF Merger to merger Due report & MEL due reports
                    'Added by Saylee on 10-Oct-2018 for BIRD ,ALL10102018
                    If rptSnagCorrectiveActionListForDue.Count > 0 Then                 'If Condition added on 22-Mar-2019 By Shital
                        SetReportWithPDFMerge(ByMail, ReportName, ReportNameForPDF)
                        Exit Sub
                    End If

                End If

                If ByMail Then

                    SendMailFile.SendMailFile(Session("CrystalReport"),
                                              Thread.CurrentPrincipal.Identity.Name,
                                              ReportName + " " + cmbAircraft.SelectedItem.Text,
                                              ReportNameForPDF,
                                              lblDateRangeFrom.Text + ", " + lblAircraft1.Text + ", " + lblAssembly1.Text,
                                              "",
                                              Session("ToSendMailIDs"),
                                              Session("CcSendMailIDs"),
                                              "",
                                              True,
                                              Remark:=Session("SendMailRemark"),
                                              ReportGeneratedBy:=Session("ReportGenratedBy"),
                                              SmtpHost:=mModuleList.Item("Due-PeriodWise").SmtpHost,
                                              SmtpPort:=mModuleList.Item("Due-PeriodWise").SmtpPort,
                                              SmtpUser:=mModuleList.Item("Due-PeriodWise").SmtpUser,
                                              SmtpPassword:=mModuleList.Item("Due-PeriodWise").SmtpPassword,
                                              OtherInfo:=SearchStr)

                ElseIf ByExcel Then
                    SetExcel(ReportMaintenanceDetails, Report, ReportName)

                Else

                    Dim Str As String
                    Str = "openTranDetail();"
                    ScriptManager.RegisterStartupScript(Me,
                                                        [GetType],
                                                        "openTranDetail",
                                                        Str,
                                                        True)

                    MarkLog(Action.Print,
                            "Due-PeriodWise",
                            mEventLogDetails,
                            ErrorType.NoError,
                            Guid.Empty,
                            EventLogID)

                End If

            Else

                Dim reportmaintdetailslist As List(Of ReportMaintenanceDetail) = New List(Of ReportMaintenanceDetail)

                reportmaintdetailslist = (From c As ReportMaintenanceDetail In ReportMaintenanceDetails.AsParallel
                                          Order By c.MinimumRemainingValue, c.RegNo, c.AssemblyType, c.Model, c.AssemblySerialNo, c.MaintenanceEvent, c.Description, c.PartNo
                                          Select c).ToList
                Session("ReportMaintenanceDetails") = ReportMaintenanceDetails
                Session("reportmaintdetailslist") = reportmaintdetailslist
                GenerateSearchCriteriaString() 'Added By Vikrant On 03-Jun-2016 For ALL03062016
                'Added By Vikrant on 14-Jun-2018 For ALL14062018
                Session("AsOnDateForWOCreation") = txtFromDate.Text
                Session("MachineIDForWOCreation") = cmbAircraft.SelectedValue.ToString
                'End
                Dim str As String
                str = "openledgersame('wfDueResult_Ajax.aspx?');"

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "OpenScript",
                                                    str,
                                                    True)

            End If

        Catch ex As Exception

            Dim Day, Month, Year As String
            Day = Format(Today.Date.Day, "0#")
            Month = Format(Today.Date.Month, "0#")
            Year = Format(Today.Date.Year, "0#")
            Dim todaydate As String = Day & Month & Year
            Dim Path As String = AppSettings("DOCPath") & todaydate

            FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
            WriteLine(1, Date.Now.ToString + " Mail service (SetReport Sub Method): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)

        End Try

    End Sub

    'Added by Saylee on 10-Oct-2018 for ALL10102018, for Pdf merger of Due & MEL report
    Private Sub SetReportWithPDFMerge(Optional ByMail As Boolean = False,
                                      Optional ReportName As String = "",
                                      Optional ReportNameForPDF As String = "")

        Dim myReport As Engine.ReportClass
        Dim myReportChild As Engine.ReportClass
        Dim PDFNo As Integer = 1
        Dim PDFNoChild As Integer = 1
        Dim tmp As Integer
        Dim Random As New Random

        Try

            Aircraft = IIf(Aircraft = "", "ALL", Aircraft.Trim)
            tmp = Random.Next

            Dim FileName = "C:\Temp\" & Aircraft & tmp & PDFNo.ToString & ".pdf"

            myReport = CType(Session("CrystalReport"), Engine.ReportClass)

            Dim myExportOption As ExportOptions
            Dim myDiskOption As DiskFileDestinationOptions
            myDiskOption = New DiskFileDestinationOptions
            myDiskOption.DiskFileName = FileName
            myExportOption = myReport.ExportOptions

            With myExportOption
                .DestinationOptions = myDiskOption
                .ExportDestinationType = ExportDestinationType.DiskFile
                .ExportFormatType = ExportFormatType.PortableDocFormat
            End With

            myReport.Export()
            myReport.Close()
            myReport.Dispose()
            GC.Collect()

            Dim pageCount As Integer = 0
            Dim pdfList As New ArrayList From {
                FileName
            }
            PDFNo = PDFNo + 1

            'MEL Snag Due
            Dim da As New ObjectAdapter
            Dim ds As New dsMELSnagCorrectiveActionForDue
            Dim aChild As New Random
            Dim tmpChild As Integer
            PDFNoChild = 1

            Dim mMELSnagCorrectiveActionListForDue As MELSnagCorrectiveActionListForDue
            myReportChild = New crptMELSnagCorrectiveActionForDue

            If AppSettings("TimeFormat") = "HH:mm" Or
               AppSettings("TimeFormat") = "hh:mm" Then

				mMELSnagCorrectiveActionListForDue =
					MELSnagCorrectiveActionListForDue.GetMELSnagCorrectiveActionListForDue(AsOnDate:=txtFromDate.Text,
																						   MachineID:=New Guid(cmbAircraft.SelectedValue.ToString),
																						   ATAID:=Guid.Empty,
																						   MELCategoryID:=0,
																						   IsMajor:=0,
																						   TimeFormat:="HH:mm",
																						   IsPireps:=0,
																						   SkipIsForInventoryAircraft:=True,
																						   DueDaysLimit:=PerDayLimitForDaysPeriod)

			Else

				mMELSnagCorrectiveActionListForDue =
					MELSnagCorrectiveActionListForDue.GetMELSnagCorrectiveActionListForDue(AsOnDate:=txtFromDate.Text,
																						   MachineID:=New Guid(cmbAircraft.SelectedValue.ToString),
																						   ATAID:=Guid.Empty,
																						   MELCategoryID:=0,
																						   IsMajor:=0, ,
																						   IsPireps:=0,
																						   SkipIsForInventoryAircraft:=True,
																						   DueDaysLimit:=PerDayLimitForDaysPeriod)
			End If

            Dim SearchStr As String = "For Next " & PerDayLimitForDaysPeriod & " Days" '& "As On Date:" & txtFromDate.Text.Trim

            Dim Report As New ReportData(mCompanyDetail.CompanyName,
                                         mCompanyDetail.Address,
                                         mCompanyDetail.Tel1,
                                         mCompanyDetail.Tel2,
                                         mCompanyDetail.Fax,
                                         mCompanyDetail.Email,
                                         mCompanyDetail.WebSite,
                                         IIf(AppSettings("MELSnagNomenclature") = "True",
                                             "ADD Due Report",
                                             "MEL Due Report"),
                                         New SmartDate(txtFromDate.Text).FormattedText,
                                         Aircraft,
                                         ATAChapter,
                                         "",
                                         "",
                                         AppSettings("Product Version"),
                                         AppSettings("SINote"),
                                         SearchStr,
                                         "",
                                         "",
                                         "",
                                         AppSettings("Logo"),
                                         SearchStr16:=Val(Trim(txtForecastingLimit.Text)).ToString)

            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mMELSnagCorrectiveActionListForDue)
            da.Fill(ds, mrptImage)
            da.Fill(ds, Report)
            myReportChild.SetDataSource(ds)
            Session("myReportChild") = myReportChild

            'PDFNo = 1
            tmpChild = aChild.Next

            Dim MyFile1Child = "C:\Temp\" & tmpChild & PDFNoChild.ToString & ".pdf"

            myReportChild = CType(Session("myReportChild"), Engine.ReportClass)

            Dim myDiskOptionChild As DiskFileDestinationOptions


            myDiskOptionChild = New DiskFileDestinationOptions
            myDiskOptionChild.DiskFileName = MyFile1Child
            myExportOption = myReportChild.ExportOptions

            With myExportOption
                .DestinationOptions = myDiskOptionChild
                .ExportDestinationType = ExportDestinationType.DiskFile
                .ExportFormatType = ExportFormatType.PortableDocFormat
            End With

            Try

                myReportChild.Export()
                myReportChild.Close()
                myReportChild.Dispose()
                GC.Collect()

            Catch ex As Exception
                Throw ex
            End Try

            pageCount = 0
            pdfList.Add(MyFile1Child)
            PDFNo = PDFNo + 1
            PDFNoChild = 1

            Dim MyFile1Child_Ext As String = "C:\Temp\" & Aircraft & tmp & PDFNo.ToString & "_Ext" & ".pdf"
            Dim MergedPath As String = "C:\Temp\" & "DueWithMEL_myMergedPdf.pdf"
            Dim MergedPath_WM As String = "C:\Temp\" & "DueWithMEL_myMergedPdf_WM.pdf"

            Dim filesByte As New List(Of Byte())()

            For Each file__1 As String In pdfList 'files
                filesByte.Add(File.ReadAllBytes(file__1))
            Next

            File.WriteAllBytes(MergedPath,
                               PDFMergers.MergeFiles(filesByte))

            AddWatermarkText(MergedPath,
                             MergedPath_WM,
                             Aircraft, , ,
                             iTextSharp.text.BaseColor.GRAY, ,
                             0.0,
                             pageCount)

            Session("CrystalReport") = MergedPath_WM
            Session("PrintReportWithAttachment") = "True"

            If ByMail Then

                SendMailFile.SendMailFile(,
                                          Thread.CurrentPrincipal.Identity.Name,
                                          ReportName + " " + cmbAircraft.SelectedItem.Text,
                                          ReportNameForPDF, lblDateRangeFrom.Text + ", " + lblAircraft1.Text + ", " + lblAssembly1.Text,
                                          "",
                                          Session("ToSendMailIDs"),
                                          Session("CcSendMailIDs"),
                                          Session("CrystalReport"),
                                          True,
                                          Remark:=Session("SendMailRemark"),
                                          ReportGeneratedBy:=Session("ReportGenratedBy"),
                                          SmtpHost:=mModuleList.Item("Due-PeriodWise").SmtpHost,
                                          SmtpPort:=mModuleList.Item("Due-PeriodWise").SmtpPort,
                                          SmtpUser:=mModuleList.Item("Due-PeriodWise").SmtpUser,
                                          SmtpPassword:=mModuleList.Item("Due-PeriodWise").SmtpPassword)

            Else

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType](),
                                                    "openTranDetail",
                                                    "openTranDetail();",
                                                    True)

            End If

            Dim DeleteThis As String = Aircraft
            Dim Files As String() = Directory.GetFiles("C:\Temp\")

            For Each file__1 As String In Files

                If file__1.ToUpper().Contains(DeleteThis.ToUpper()) Then
                    File.Delete(file__1)
                End If

            Next
            'End

        Catch ex As Exception
            Throw ex.GetBaseException
        End Try

    End Sub

    Private Sub MessageBoxResult()

        Dim Result As MsgBoxResult
        Result = MSGBoxCtrl.Result

        If Result > 0 Then

            Select Case Result
                Case MsgBoxResult.Yes
                    '
                Case MsgBoxResult.No
                    '
                Case MsgBoxResult.Ok
                    Session("Sender") = ""
                Case Else
                    '
            End Select

        ElseIf Result = -1 Then
            Session("Sender") = ""
        End If

    End Sub

    Private Sub ControlVisibilityForDetails()

        If DueType = 1 Then

            If (AppSettings("ClientCode") IsNot Nothing) AndAlso
               (AppSettings("ClientCode") = "Deccan" Or
                AppSettings("ClientCode") = "ADeccan" Or
                AppSettings("ClientCode") = "IIC" Or
                AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022

                Label4.Visible = False
                lblLimit.Visible = False
                txtForecastingLimit.Visible = False
                lblStep7.Text = "Display Report"
                Label5.Text = "Format Selection"

            End If

        End If

        '---Added by Vikrant For HLI11102011 ---------------
        If (AppSettings("ClientCode") IsNot Nothing) AndAlso
           (AppSettings("ClientCode") = "Heligo" Or
            AppSettings("ClientCode") = "UHPL" Or
            AppSettings("ClientCode") = "ARA") Then  'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013

            If User.IsInRole("DuePeriodWiseView") = True Then

                lblStep8.Visible = True
                lblRefNo.Visible = True
                txtRefNo.Visible = True
                Label5.Text = "Format selection"
                lblStep7.Text = "Display Report"

            Else

                lblStep8.Visible = False
                lblRefNo.Visible = False
                txtRefNo.Visible = False

            End If

        End If
        '---------------------------------------------------

        btnByExcel.Enabled = IIf(chkMEL.Checked = True, False, True)
        upnlDetails.Update()

    End Sub

    Private Sub SetTitle()
        If DueType = 1 Then
            lbltitle.Text = "Search criteria for Due"
        End If
        upnlTitle.Update()
    End Sub

    Private Sub ControlVisibilityForAvgPeriod()

        If DueType = 1 Then

        Else
            rbdAvrageMonths.Visible = False
            rbdSpecifyValues.Visible = False
            lblAvgMnths.Visible = False
            txtAvgMnths.Visible = False
            lblMonths.Visible = False
            lblInfo.Visible = False
            lblAvgMnths1.Visible = False
            gdvPerDayLimit.Visible = False

        End If

        upnlAvrgperiod.Update()

    End Sub

#End Region

#Region " Data Binding "

    Public Sub DataFieldBind()

        mDueLimits = DueLimits.GetDueLimits(New Guid(cmbAircraft.SelectedValue.ToString))
        gdvDuePeriodLimits.DataSource = mDueLimits
        Session("mDueLimits") = mDueLimits
        upnlDueLimits.Update()

        mPerDayLimits = PerDayLimits.GetPerDayLimits(New Guid(cmbAircraft.SelectedValue.ToString))
        gdvPerDayLimit.DataSource = mPerDayLimits
        Session("mPerDayLimits") = mPerDayLimits
        upnlAvrgperiod.Update()

        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        Session("mCompanyDetail") = mCompanyDetail

        If mCompanyDetail.ShortName = "TS" Then
            cmbFormat.Items.Add(New ListItem("Format 3(Enlarge Copy with Limited Columns)", "2"))
        ElseIf AppSettings("ClientCode") = "YA" Then 'Added By Prashant 30-Nov-2023
            cmbFormat.Items.Add(New ListItem("Format 3(Sort by estimated date)", "2"))
        End If

        mFAScsReportList = FAScsReportList.GetFAScsReportList()
        Session("mFAScsReportList") = mFAScsReportList

        DataBind()
        If mCompanyDetail.ShortName = "TS" Then
            cmbFormat.SelectedIndex = 2
        End If

    End Sub

    Public Sub SetTypeCombo()
        If mServiceTypeList Is Nothing Then
            mServiceTypeList = PartMonitorServiceTypeList.GetPartMonitorServiceTypeList(, True)
        End If
        ListServiceType.DataSource = mServiceTypeList
        Session("mServiceTypeList") = mServiceTypeList

        If mInspectionTypeList Is Nothing Then
            mInspectionTypeList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList()    ''ModelMonitorInspTypeList.serach.ExludingRoutineInspections)
        End If
        ListInspectionType.DataSource = mInspectionTypeList
        Session("mInspectionTypeList") = mInspectionTypeList

        If mModificationTypeList Is Nothing Then
            mModificationTypeList = ModelMonitorModTypeList.GetModelMonitorModTypeList(, True)
        End If

        ListDirectiveType.DataSource = mModificationTypeList
        Session("mModificationTypeList") = mModificationTypeList
        DataBind()
        FillMonitorTypeList()
    End Sub

    Public Sub SetComboOfMachine(AsonDate As String)

        'Commented by Saylee on 21-Oct-2024 as per discussion with Deven sir
        ''If DueType = 1 Then
        ''    mMachineNameValueList = MachineNameValueList.GetMachineList(AsonDate, , , , , , , True, "(ALL)", , True)
        ''Else
        ''    mMachineNameValueList = MachineNameValueList.GetMachineList(AsonDate, , , , , , , True, "(SELECT)", , True)
        ''End If
        'Added by Saylee on 21-Oct-2024 as per discussion with Deven sir
        mMachineNameValueList = MachineNameValueList.GetMachineList(AsonDate, , , , , , , False, , , True)

        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataBind()
    End Sub

    Public Sub CustomValidate1(s As Object, e As ServerValidateEventArgs)
        Dim str As String = ""
        Dim Childs As Integer
        Dim child As DueLimit
        If Flag = 1 Then Exit Sub
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        'this is for grid validation
        SetGridObject()
        If Not mDueLimits.IsValid Then
            For Childs = 0 To mDueLimits.Count - 1
                child = mDueLimits(Childs)
                For i As Integer = 0 To child.GetBrokenRulesCollection.Count - 1
                    str = str + child.GetBrokenRulesCollection(i).Description + "<BR>"
                Next
            Next
        End If

        If str <> "" Then
            custValidator.ErrorMessage = str
            e.IsValid = False
        End If
        Flag = 1
    End Sub

    Private Sub FillMonitorTypeList()

        chkService.Checked = True
        chkDirective.Checked = True

        For i As Integer = 0 To ListServiceType.Items.Count - 1
            ListServiceType.Items(i).Selected = True
        Next

        'Modified by Harsh on 10th May 2024 -- Updated visibility condition for Inspection ListBox
        If AppSettings("ShowMaintenanceForNewClients").ToLower() = "true" Then

            chkInspection.Checked = False
            For i As Integer = 0 To ListInspectionType.Items.Count - 1

                ListInspectionType.Items(i).Selected = False

                If AppSettings("ShowNewDiscrepancyFlow").ToLower() = "true" Then

                    chkInspection.Checked = True
                    ListInspectionType.Items(i).Selected = True

                End If

            Next

        Else

            chkInspection.Checked = True

            For i As Integer = 0 To ListInspectionType.Items.Count - 1
                ListInspectionType.Items(i).Selected = True
            Next

        End If

        For i As Integer = 0 To ListDirectiveType.Items.Count - 1
            ListDirectiveType.Items(i).Selected = True
        Next

    End Sub

    Private Sub ControlVisibility()
        Dim txtLimit As TextBox
        Dim i As Int32
        For i = 0 To Me.gdvDuePeriodLimits.Rows.Count - 1
            txtLimit = CType(Me.gdvDuePeriodLimits.Rows(i).FindControl("txtLimit"), TextBox)
            If rbdDueLimits.Checked Then
                txtLimit.Enabled = True
            ElseIf rbdPercent.Checked Then
                txtLimit.Enabled = False
            End If
        Next i
    End Sub

    'Added By Vikrant On 03-Jun-2016 For ALL03062016
    Private Sub GenerateSearchCriteriaString()
        Dim SearchCriteriaValues As New Hashtable From {
            {"AsonDate", txtFromDate.Text},
            {"MachineID", MachineName},
            {"DueLimitObj", mDueLimits},
            {"IsrbdPercentChecked", rbdPercent.Checked},
            {"Percentage", Val(txtPercentage.Text)},
            {"AssemblyID", AssemblyName},
            {"AverageMonths", AvgMnths},
            {"IsSpecifyValuesChecked", rbdSpecifyValues.Checked},
            {"PerDayLimitsObj", mPerDayLimits},
            {"IsServiceRequired", IsSerSelect},
            {"IsModRequired", IsModSelect},
            {"IsInspRequired", IsInsSelect},
            {"ForDueStatus", Val(txtForecastingLimit.Text)},
            {"SelectedAircraftText", cmbAircraft.SelectedItem.ToString},
            {"ServiceTypeID", ServiceTypeID},
            {"InspectionTypeID", InspectionTypeID},
            {"ModificationTypeID", ModificationTypeID},
            {"Aircraft", IIf(cmbAircraft.SelectedIndex > -1, cmbAircraft.SelectedItem.Text, "")},
            {"IschkwithWONoChecked", chkwithWONo.Checked}
        }

        Session("SearchCriteriaValues") = SearchCriteriaValues
    End Sub
    'End

#End Region

#Region " Eventes "

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        ClearAll()
        GetSession()
        AddAttributes()
        EventLogID = CType(Session("EventLogID"), Guid)

        If Not IsPostBack And Session("Sender") = "" Then

            DueType = Request.QueryString("DueType")
            Session("DueType") = DueType
            Session("MiddleFrame") = "wfSearchCriteriaForDue_Ajax.aspx?DueType=" & DueType
            ResetValues()
            'lblAssembly.Enabled = False
            'cmbAssembly.Enabled = False
            txtFromDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            AOnDate = Now.Date.ToString(AppSettings("DateFormat"))
            SetComboOfMachine(AOnDate)
            SetFocus(cmbAircraft)

            'Prashant 12-Dec-2022
            If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "Due-PeriodWise") Then

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "MarkFav",
                                                    "MarkFav();",
                                                    True)

            Else

                ScriptManager.RegisterStartupScript(Me,
                                                    [GetType],
                                                    "RemoveFav",
                                                    "RemoveFav();",
                                                    True)

            End If
            '--------------------------

            'MachineID parameter passed by Saylee on 21-Oct-2024, as now "ALL" criteria of Aircraft removed for selection
            mAssemblyList = AssemblyList.GetAssemblyListForComboBox(0,
                                                                    cmbAircraft.SelectedValue.ToString,
                                                                    txtFromDate.Text.Trim.ToString,
                                                                    "(ALL)",
                                                                    True)
            Session("mAssemblyList") = mAssemblyList
            cmbAssembly.DataSource = mAssemblyList

            DataFieldBind()
            SetTypeCombo()
            Report = 1
            ControlVisibilityForDetails()
            ControlVisibilityForAvgPeriod()
            rbdAvrageMonths.Checked = True
            SetSession()

            'Added by Harsh on 10th May 2024 -- Updated visibility condition for Inspection ListBox
            If AppSettings("ShowMaintenanceForNewClients").ToLower() = "true" Then

                phInspection.Visible = False
                cvSelection.ErrorMessage = "Please select at least One Maintenance Event or Directive."

                If AppSettings("ShowNewDiscrepancyFlow").ToLower() = "true" Then

                    phInspection.Visible = True
                    cvSelection.ErrorMessage = "Please select at least One Maintenance Event, Inspection or Directive."

                End If
                chkAssembly.Text = IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Show Assembly AMPs / Directives", "Show Assembly Insps / Services / Directives")
                chkComponent.Text = IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Show Component AMPs / Directives", "Show Component Insps / Services / Directives")
            Else

                phInspection.Visible = True
                cvSelection.ErrorMessage = "Please select at least One Service, Inspection or Directive."

            End If

        End If

    End Sub

    Private Sub btnCurrentSearchCriteria_Click(sender As Object, e As EventArgs) Handles btnCurrentSearchCriteria.Click
        If IsValid = True Then
            Display()
            SetValues()
            btnByExcel.Enabled = IIf(chkMEL.Checked = True, False, True)
            upnlDetails.Update()
        End If
    End Sub

    Private Sub btnDisplay_Click(sender As Object, e As EventArgs) Handles btnDisplay.Click

        If IsValid = True Then
            mIsExcel = False
            If chkwithWONo.Checked = False Then
                SetReport(, mIsExcel)
            Else
                SetReportWithWONo(, mIsExcel) 'Added by Saylee on 6-May-2013 for ALL06052013-1
            End If
        Else
            upnlValidations.Update()
        End If
        btnByExcel.Enabled = IIf(chkMEL.Checked = True, False, True)
        upnlDetails.Update()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        mMachineList = Nothing
        mDueLimits = Nothing
        mAssemblyList = Nothing
        'Added By Saylee on 20-Feb-2009
        mServiceTypeList = Nothing
        mInspectionTypeList = Nothing
        mModificationTypeList = Nothing
        '=============================
        Session("MiddleFrame") = ""
        ClearAll()
        Response.Redirect("Dashboard.aspx")
    End Sub

    Private Sub rbdPercent_CheckedChanged(sender As Object, e As EventArgs) Handles rbdPercent.CheckedChanged
        txtPercentage.Enabled = True
        txtPercentage.Text = "10"
        mDueLimits.SetPercentageWise(True, CDec(Val(txtPercentage.Text)))
        Dim txtLimit As TextBox
        Dim i As Int32
        For i = 0 To Me.gdvDuePeriodLimits.Rows.Count - 1
            txtLimit = CType(Me.gdvDuePeriodLimits.Rows(i).FindControl("txtLimit"), TextBox)
            txtLimit.Enabled = False
        Next i
        upnlDueLimits.Update()
    End Sub

    Private Sub rbdDueLimits_CheckedChanged(sender As Object, e As EventArgs) Handles rbdDueLimits.CheckedChanged
        txtPercentage.Enabled = False
        txtPercentage.Text = ""
        mDueLimits.UnSetPercentageWise()
        Dim txtLimit As TextBox
        Dim i As Int32
        For i = 0 To Me.gdvDuePeriodLimits.Rows.Count - 1
            txtLimit = CType(Me.gdvDuePeriodLimits.Rows(i).FindControl("txtLimit"), TextBox)
            txtLimit.Enabled = True
        Next i
        upnlDueLimits.Update()
    End Sub

    '11-Sep-2008--------------------
    Private Sub btnPreview_Click(sender As Object, e As EventArgs) Handles btnPreview.Click
        mIsPreview = True
        If IsValid = True Then
            SetReport(IsPreviewClicked:=True)
        Else
            upnlValidations.Update()
        End If
        btnByExcel.Enabled = IIf(chkMEL.Checked = True, False, True)
        upnlDetails.Update()
    End Sub
    '-------------------------------

    Private Sub rbdAvrageMonths_CheckedChanged(sender As Object, e As EventArgs) Handles rbdAvrageMonths.CheckedChanged
        lblAvgMnths.Visible = True
        txtAvgMnths.Visible = True
        lblMonths.Visible = True
        pnlAvragePeriod.Visible = False
        lblInfo.Visible = False
        upnlAvrgperiod.Update()
    End Sub

    Private Sub rbdSpecifyValues_CheckedChanged(sender As Object, e As EventArgs) Handles rbdSpecifyValues.CheckedChanged
        lblAvgMnths.Visible = False
        txtAvgMnths.Visible = False
        lblMonths.Visible = False
        pnlAvragePeriod.Visible = True
        lblInfo.Visible = True
        upnlAvrgperiod.Update()
    End Sub

    Private Sub cmbAircraft_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbAircraft.SelectedIndexChanged

        lblAssembly.Enabled = True
        cmbAssembly.Enabled = True

        Dim mAssemblylist As AssemblyList
        mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, txtFromDate.Text.Trim.ToString, "(ALL)", True)
        Session("mAssemblyList") = mAssemblylist
        cmbAssembly.DataSource = mAssemblylist
        cmbAssembly.DataBind()

        AircraftIndex = cmbAircraft.SelectedIndex
        Session("AircraftIndex") = AircraftIndex
        ScriptManager.RegisterStartupScript(Me, [GetType], "WONoCheckBoxVisibility", "ControlVisibilityForWONo('True')", True)
        'End If
        If cmbAircraft.Enabled = True Then
            SetFocus(cmbAircraft)
        End If
        DataFieldBind()
        ControlVisibility()

    End Sub

    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

    Private Sub txtFromDate_TextChanged(sender As Object, e As EventArgs) Handles txtFromDate.TextChanged
        AOdate = txtFromDate.Text.Trim
        If AOnDate = AOdate Then
        Else
            Dim tmpdate As Date
            If Date.TryParse(txtFromDate.Text.Trim, tmpdate) Then
                SetComboOfMachine(AOdate)
                'lblAssembly.Enabled = False
                'cmbAssembly.Enabled = False
                'mAssemblyList = Nothing
                'Session("mAssemblyList") = mAssemblyList
                'cmbAssembly.ClearSelection()
                'cmbAssembly.DataSource = mAssemblyList
                'cmbAssembly.Controls.Clear()
                'cmbAssembly.DataBind()
                mAssemblyList = AssemblyList.GetAssemblyListForComboBox(0,
                                                                    cmbAircraft.SelectedValue.ToString,
                                                                    txtFromDate.Text.Trim.ToString,
                                                                    "(ALL)",
                                                                    True)
                Session("mAssemblyList") = mAssemblyList
                cmbAssembly.DataSource = mAssemblyList

                DataFieldBind()
                ControlVisibility()
                ScriptManager.RegisterStartupScript(Me, [GetType], "WONoCheckBoxVisibility", "ControlVisibilityForWONo('False')", True)
            End If
        End If
    End Sub

    Private Sub hdnimgBtnSendMail_Click(sender As Object, e As EventArgs) Handles hdnimgBtnSendMail.Click
        Dim email As Thread
        Try
            If chkwithWONo.Checked = False Then
                email = New Thread(Sub() SetReport(True))
            Else
                email = New Thread(Sub() SetReportWithWONo(True)) 'SetReportwithWONo() 'Added by Saylee on 6-May-2013 for ALL06052013-1
            End If
            mIsPreview = False
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
            WriteLine(1, Date.Now.ToString + " Mail service (hdnimgBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        End Try
        btnByExcel.Enabled = IIf(chkMEL.Checked = True, False, True)
        upnlDetails.Update()
    End Sub

    Protected Sub btnByMail_Click(sender As Object, e As EventArgs) Handles btnByMail.Click
        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        Session("UserEmailID") = mModuleList.Item("Due-PeriodWise").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("Due-PeriodWise").SendCCMailID
        '--------------------------
        btnByExcel.Enabled = IIf(chkMEL.Checked = True, False, True)
        upnlDetails.Update()
        Dim Str As String
        Str = "OpenByMaiWindow();"
        ScriptManager.RegisterStartupScript(Me, [GetType](), "OpenByMaiWindow", Str, True)
    End Sub

    Private Sub btnByExcel_Click(sender As Object, e As EventArgs) Handles btnByExcel.Click
        If IsValid = True Then
            mIsExcel = True
            If chkwithWONo.Checked = False Then
                SetReport(, mIsExcel)
            Else
                SetReportWithWONo(, mIsExcel) 'Added by Saylee on 6-May-2013 for ALL06052013-1
            End If
        End If
    End Sub

    Private Sub btnMaintStmt_Click(sender As Object, e As EventArgs) Handles btnMaintStmt.Click
        If IsValid = True Then
            SetReport(IsMaintStmt:=True)
        End If
    End Sub

    Private Sub hdnBtnMarkFav_Click(sender As Object, e As EventArgs) Handles hdnBtnMarkFav.Click 'Prashant 12-Dec-2022
        MarkFavourite(HttpContext.Current.User.Identity.Name, "Due-PeriodWise")
    End Sub

    Private Sub hdnBtnRemoveFav_Click(sender As Object, e As EventArgs) Handles hdnBtnRemoveFav.Click 'Prashant 12-Dec-2022
        RemoveFavourite(HttpContext.Current.User.Identity.Name, "Due-PeriodWise")
    End Sub

#End Region

End Class