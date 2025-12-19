Imports System.Linq
Imports System.Collections.Generic
Imports System.Text
Public Class wfRequiredSpareasPerMaintenanceDue_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mDueLimits As DueLimits
    Dim mPerDayLimits As PerDayLimits
    Dim ReportStatusList As New rptStatusList
    Dim mMachineList As MachineList
    Dim mMachineNameValueList As MachineNameValueList
    Dim mtmpMachineList As tmpMachineList
    Dim ReportMaintenanceDetails As New ReportMaintenanceDetailList
    Private Flag As Int16
    Dim AOdate As String
    Dim AOnDate As String
    Dim Average As String
    Dim Aircraft As String
    Dim Periodcount As Integer
    Dim MachineName As String
    Dim AsonDate As String
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
    Dim AircraftIndex As Integer
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
    Dim Remark As String
    Dim Code As String
    Dim StatusMasterID As Guid
    Dim DocumentTypeForID As Integer
    Dim AssemblyDueAsof As String
    Dim AssemblyDueAsof1 As String
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
    Dim PerDayLimitForDaysPeriod As Integer = -1
    Dim mCompanyDetail As New CompanyDetail
    Private Zone, Area As String
    Private IsRII As Boolean
    Private mFAScsReportList As FAScsReportList
    Private mSpareListByMaintenanceActivity As SpareListByMaintenanceActivity
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
#End Region

#Region " Helper Methods "
    Private Sub addAttributes()
        txtAvgMnths.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtAvgMnths').value,event)")
        txtForecastingLimit.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtForecastingLimit').value,event)")
    End Sub
    Private Sub SetGridObject()
        Dim txtLimit As TextBox
        Dim i As Int32
        For i = 0 To Me.gdvDuePeriodLimits.Rows.Count - 1
            txtLimit = CType(Me.gdvDuePeriodLimits.Rows(i).FindControl("txtLimit"), TextBox)
            mDueLimits.Item(i).PeriodLimit = Trim(txtLimit.Text)
            If mDueLimits.Item(i).PeriodID = 2 Then
                PerDayLimitForDaysPeriod = CInt(IIf(mDueLimits.Item(i).PeriodLimit <> "", mDueLimits.Item(i).PeriodLimit, 0))
            End If
        Next i
        Session("mDueLimits") = mDueLimits
        Dim txtPerDatLimit As TextBox
        Dim i1 As Int32
        For i1 = 0 To Me.gdvPerDayLimit.Rows.Count - 1
            txtPerDatLimit = CType(Me.gdvPerDayLimit.Rows(i1).FindControl("txtLimitPerDay"), TextBox)
            mPerDayLimits.Item(i1).PeriodLimit = Trim(txtPerDatLimit.Text)  'Added by Saylee on 12-Nov-2012
        Next i1
        Session("mPerDayLimits") = mPerDayLimits
    End Sub
    Private Sub GetSession()
        mMachineList = CType(Session("mMachineList"), MachineList)
        mDueLimits = CType(Session("mDueLimits"), DueLimits)
        mPerDayLimits = CType(Session("mPerDayLimits"), PerDayLimits)
        AOnDate = Session("AOnDate")
        AvgMnths = Session("AvgMnths")
        mAssemblyList = CType(Session("mAssemblyList"), AssemblyList)
        mServiceTypeList = CType(Session("mServiceTypeList"), PartMonitorServiceTypeList)
        mInspectionTypeList = CType(Session("mInspectionTypeList"), ModelMonitorInspTypeList)
        mModificationTypeList = CType(Session("mModificationTypeList"), ModelMonitorModTypeList)
        mMachineNameValueList = Session("mMachineNameValueList")
        mCompanyDetail = Session("mCompanyDetail")
        mFAScsReportList = Session("mFAScsReportList")
        mModuleList = Session("mModuleList")       'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    End Sub
    Private Sub SetSession()
        Session("mMachineList") = mMachineList
        Session("mDueLimits") = mDueLimits
        Session("mPerDayLimits") = mPerDayLimits
        Session("AOnDate") = AOnDate
        Session("AvgMnths") = AvgMnths
        Session("mAssemblyList") = mAssemblyList
        Session("mServiceTypeList") = mServiceTypeList
        Session("mInspectionTypeList") = mInspectionTypeList
        Session("mModificationTypeList") = mModificationTypeList
        Session("mMachineNameValueList") = mMachineNameValueList
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfRequiredSpareasPerMaintenanceDue_Ajax.aspx" Then
            Session.Remove("mMachineList")
            Session.Remove("mDueLimits")
            Session.Remove("mPerDayLimits")
            Session.Remove("AOnDate")
            Session.Remove("AvgMnths")
            Session.Remove("mAssemblyList")
            Session.Remove("mMachineNameValueList")
            Session.Remove("mServiceTypeList")
            Session.Remove("mInspectionTypeList")
            Session.Remove("mModificationTypeList")
            Session.Remove("mFAScsReportList")
        End If
        mIsExcel = False
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
        If (cmbAircraft.SelectedItem.Text = "(All)") Or (cmbAircraft.SelectedItem.Text = "(SELECT)") Then
            MachineName = "{00000000-0000-0000-0000-000000000000}"
            AssemblyName = "{00000000-0000-0000-0000-000000000000}"
            Assembly1 = ""
            lblAssembly1.Text = ""
        Else
            MachineName = cmbAircraft.SelectedValue.ToString
            If cmbAssembly.SelectedItem.Text = "(All)" Then
                AssemblyName = "{00000000-0000-0000-0000-000000000000}"
                Assembly1 = ""
                AssemblyType = "(All)"
                lblAssembly1.Text = "Assembly Name  : " + "<b> All </b>"
            Else
                AssemblyType = mAssemblyList(cmbAssembly.SelectedIndex).AssemblyType
                AssemblyName = cmbAssembly.SelectedValue.ToString
                Assembly1 = cmbAssembly.SelectedItem.Text
                lblAssembly1.Text = "Assembly Name : " & "<b>" + Assembly1 + "</b>"
            End If
        End If
        Average = txtAvgMnths.Text
        If Not IsDate(txtFromDate.Text.Trim) Then
            AsonDate = ""
        Else
            AsonDate = txtFromDate.Text.Trim
        End If
        Aircraft = IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "")
        If AsonDate <> "" Then
            lblDateRangeFrom.Text = "As On Date : " & "<b>" + txtFromDate.Text.Trim + "</b>"
        Else
            lblDateRangeFrom.Text = "As On Date : " & "All"
        End If

        lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", "<b>" + Aircraft + "</b>", "All")
        lblAvgMnths1.Text = "Average Months : " & IIf(Average <> "", Average, "All")
        percent = txtPercentage.Text
        lblPercent.Text = "Percent : " & IIf(percent <> "", percent, "All")
        'Service
        If chkService.Checked Then
            IsSerSelect = True
            ServiceTypeID = (From c As System.Web.UI.WebControls.ListItem In ListServiceType.Items
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
        status = IIf(rbdAvrageMonths.Checked, rbdAvrageMonths.Text, rbdSpecifyValues.Text)
        If rbdSpecifyValues.Checked Then
            EstimatedFlyingHours = status & " : " & String.Join(", ", (From c As PerDayLimit In mPerDayLimits
                        Select c.PeriodName & " : " & c.PeriodLimitFormatted).ToArray)
        Else
            EstimatedFlyingHours = status & " : " & txtAvgMnths.Text.Trim
        End If
        mEventLogDetails = lblDateRangeFrom.Text + ", " + lblAircraft1.Text + ", " + lblAssembly1.Text + ", " + DueLimits + ", " + EstimatedFlyingHours + ", Format : " + Format
    End Sub
    Private Sub ResetValues()
        MachineName = "{00000000-0000-0000-0000-000000000000}"
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
    Public Function ReportDetail(IsExcel As Boolean, Optional ByVal IsPreviewClicked As Boolean = False) As ReportMaintenanceDetailList
        Try
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

            If rbdPercent.Checked Then mDueLimits.SetPercentageWise(True, CDec(Val(txtPercentage.Text)))

            mMachineList = MachineList.GetMachineListDueMonitoringStatus(AsonDate, mDueLimits, MachineName, AssemblyName, Val(txtAvgMnths.Text), rbdSpecifyValues.Checked, mPerDayLimits, , IsSerSelect, IsInsSelect, IsModSelect, , , , IsSerSelect, IsInsSelect, IsModSelect, Val(txtForecastingLimit.Text), True, SkipIsForInventoryAircarft:=True)
            Dim LHLabel2 As String = ""
            Dim LHData2 As String = ""

            If Not cmbAircraft.SelectedItem.ToString = "(All)" Or
               (AppSettings("ClientCode") = "APFT" Or
                AppSettings("ClientCode") = "Novo" Or
                AppSettings("ClientCode") = "AAP") Then
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
                        AssemblyID = ObjAssemblyStatus.AssemblyID
                    Next
                Next
            End If

            If Not cmbAircraft.SelectedItem.ToString = "(All)" Or
               (AppSettings("ClientCode") = "APFT" Or
                AppSettings("ClientCode") = "Novo" Or
                AppSettings("ClientCode") = "AAP") Then
                mtmpMachineList = tmpMachineList.GetMachineList(, Aircraft, , , , , True, AsonDate)
                Dim mOtherPeriodExists As String = "False"

                For i As Integer = 0 To mtmpMachineList.Count - 1
                    If mtmpMachineList(i).AllPeriods <> "" Then
                        mOtherPeriodExists = "True"
                        Exit For
                    End If
                Next

                For i As Integer = 0 To mtmpMachineList.Count - 1
                    searchstr7 = mtmpMachineList(i).Owner.ToString
                    ReportStatusList.Add(New rptStatus(mtmpMachineList(i).ID.ToString, 1, , , , , mtmpMachineList(i).TSO, , mtmpMachineList(i).CSO, , , , , , , , , mtmpMachineList(i).Cycles, mtmpMachineList(i).AllPeriods.Replace("<BR>", vbCrLf), mOtherPeriodExists, Year(txtFromDate.Text).ToString, , mtmpMachineList(i).RegNo, mtmpMachineList(i).ModelName, mtmpMachineList(i).Type, mtmpMachineList(i).SerialNo, mtmpMachineList(i).ManufacturerName, , mtmpMachineList(i).ManufacturingDate, mtmpMachineList(i).Hours, mtmpMachineList(i).Landings))
                    Session("AircraftAsOnDate") = mtmpMachineList(i).ManufacturingDateFormatted
                Next
            End If

            If IsSerSelect = True Then
                For Each ObjMachine In mMachineList
                    For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                        For Each ObjAssemblyMonitorServiceStatus In ObjAssemblyStatus.AssemblyMonitorServiceStatusList
                            'loop through selected monitory types
                            If ServiceTypeID.Contains(ObjAssemblyMonitorServiceStatus.ModelMonitorServiceTypeID) Then
                                If ObjAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriodList.Count > 0 Then
                                    If (ObjAssemblyMonitorServiceStatus.IsApplicable = True) And (Not (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True)) Then
                                        ATAChapter = ObjAssemblyMonitorServiceStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorServiceStatus.ATANomenclature
                                        Description = ObjAssemblyMonitorServiceStatus.Description
                                        AssemblyModel = ObjAssemblyStatus.Model
                                        AssemblySerialNo = ObjAssemblyStatus.SerialNo & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyStatus.Position
                                        Position = ""
                                        MonitorTypeCode = ObjAssemblyMonitorServiceStatus.Code
                                        EstimatedDate = ObjAssemblyMonitorServiceStatus.EstimatedDateFormatted
                                        MinimumRemainingValue = ObjAssemblyMonitorServiceStatus.MinimumRemainingValue
                                        AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID
                                        StatusMasterID = ObjAssemblyMonitorServiceStatus.ModelMonitorServiceID
                                        DueStatus = ObjAssemblyMonitorServiceStatus.DueStatus
                                        DocumentTypeForID = 0
                                        Remark = ObjAssemblyStatus.InstallationRemark + " " + ObjAssemblyMonitorServiceStatus.DoneRemark
                                        Code = ObjAssemblyMonitorServiceStatus.ModelMonitorServiceCode
                                        DoneOnDate = ObjAssemblyMonitorServiceStatus.DoneOn
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

                                        For Each ObjAssemblyMonitorServiceStatusPeriod In ObjAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriodList
                                            If ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 1 Then
                                                Freq1 = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue
                                                ElapsedTime = ObjAssemblyMonitorServiceStatusPeriod.ElapsedValue
                                                RemainingTime = ObjAssemblyMonitorServiceStatusPeriod.RemainingValue
                                                DueAsof = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
                                                DoneAt = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
                                                If ((Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL")) Then
                                                    AssemblyDueAsof = ObjAssemblyMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                    If DoneOnDate <> "" Then DoneAt = ObjAssemblyMonitorServiceStatusPeriod.AssemblyDoneOnValueTextByAirFrame
                                                ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
                                                    AssemblyDueAsof = ObjAssemblyMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                Else
                                                    AssemblyDueAsof = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
                                                End If
                                                SinceNew = ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValue
                                                Extension = ObjAssemblyMonitorServiceStatusPeriod.ExtensionValue
                                            End If
                                            If ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 2 Then
                                                Freq2 = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValueFormatted
                                                ElapsedTime1 = ObjAssemblyMonitorServiceStatusPeriod.ElapsedValueFormatted
                                                RemainingTime1 = ObjAssemblyMonitorServiceStatusPeriod.RemainingValueFormatted
                                                DueAsof1 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted
                                                AssemblyDueAsof1 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted
                                                SinceNew1 = ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValueFormatted
                                                DoneAt1 = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                Extension1 = ObjAssemblyMonitorServiceStatusPeriod.ExtensionValueFormatted
                                            End If
											'If ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 3 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 4 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 5 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 6 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 7 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 8 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 9 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 12 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 13 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 14 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 15 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 11 Then
											'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
											If ObjAssemblyMonitorServiceStatusPeriod.PeriodID >= 3 Then
												If Freq3 = "" Then
													Freq3 = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue
													ElapsedTime2 = ObjAssemblyMonitorServiceStatusPeriod.ElapsedValue
													RemainingTime2 = ObjAssemblyMonitorServiceStatusPeriod.RemainingValue
													DueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
													If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
														AssemblyDueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
													Else
														AssemblyDueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
													End If
													SinceNew2 = ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValue
													DoneAt2 = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
													Extension2 = ObjAssemblyMonitorServiceStatusPeriod.ExtensionValue
												Else
													Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue
													ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.ElapsedValue
													RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.RemainingValue
													DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
													If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
														AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
													Else
														AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
													End If
													SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValue
													DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
													Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.ExtensionValue
												End If
											End If
										Next
                                        AssemblyID = ObjAssemblyStatus.AssemblyID
                                        Note = ObjAssemblyMonitorServiceStatus.Notes
                                        RegNo = ObjMachine.RegNo
                                        If IsPreviewClicked Then
                                            RequiredManHours = ModelMonitorService.GetModelMonitorService(ObjAssemblyMonitorServiceStatus.ModelMonitorServiceID).RequiredManHours
                                        Else
                                            RequiredManHours = ObjAssemblyMonitorServiceStatus.RequiredManHours
                                        End If
                                        Customer = ObjMachine.Customer
                                        AssemblyType = ObjAssemblyStatus.AssemblyType
                                        If ObjAssemblyMonitorServiceStatus.Reference <> "" Then
                                            MaintenanceEvent = ObjAssemblyMonitorServiceStatus.Type & " (" & ObjAssemblyMonitorServiceStatus.MonitorType & ")" & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatus.Reference
                                        Else
                                            MaintenanceEvent = ObjAssemblyMonitorServiceStatus.Type & " (" & ObjAssemblyMonitorServiceStatus.MonitorType & ")"
                                        End If
                                        ExtensionDate = ObjAssemblyMonitorServiceStatus.ExtensionDate
                                        ApprovalRemark = ObjAssemblyMonitorServiceStatus.ApprovalRemark
                                        StatusID = ObjAssemblyMonitorServiceStatus.ID
                                        'If chkwithWONo.Checked = True Or IsPreviewClicked Then
                                        mnWOListForDueJobs = nWOListForDueJobs.GetWOListForDueJobs(StatusID)
                                        If mnWOListForDueJobs.Count > 0 Then
                                            nWONumber = mnWOListForDueJobs(0).WONumber + vbCrLf + mnWOListForDueJobs(0).WODateFormatted
                                        Else
                                            nWONumber = ""
                                        End If
                                        'End If
                                        Zone = ObjAssemblyMonitorServiceStatus.Zone
                                        Area = ObjAssemblyMonitorServiceStatus.Area
                                        IsRII = ObjAssemblyMonitorServiceStatus.IsRII
                                        If (AppSettings("ClientCode") = "APFT" Or
                                            AppSettings("ClientCode") = "AAP") And
                                           ObjAssemblyMonitorServiceStatus.ModelMonitorServiceTypeID = 5 Then
                                            DoneAt = "----"
                                            DoneAt1 = ""
                                            DoneAt2 = ""
                                            Freq1 = "----"
                                            Freq2 = ""
                                            Freq3 = ""
                                        End If
                                        ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, , AssemblySerialNo, ATAChapter, , , Position, ObjAssemblyMonitorServiceStatus.MonitorType, MonitorTypeCode, Note, Remark, Description, _
                  , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, _
                  SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, AssemblyTypeID, MaintenanceEvent, , , , , , , , , , , , , , , ObjAssemblyMonitorServiceStatus.Reference, , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, _
                  AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , ObjAssemblyMonitorServiceStatus.IsApplicable, StatusID.ToString _
                  , AssemblyStatusID:=ObjAssemblyStatus.ID.ToString, DueStatus:=DueStatus, WONumber:=nWONumber, MachineID:=ObjMachine.MachineID.ToString, ModelID:=ObjAssemblyStatus.ModelID.ToString, MaintenanceTypeID:=5, IsMaster:=ObjAssemblyMonitorServiceStatus.IsMaster, Zone:=Zone, Area:=Area, IsRII:=IsRII))
                                    End If
                                End If
                            End If
                        Next

                        For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
                            For Each ObjCompMonitorServiceStatus In ObjCompStatus.CompMonitorServiceStatusList
                                If ServiceTypeID.Contains(ObjCompMonitorServiceStatus.PartMonitorServiceTypeID) Then
                                    If ObjCompMonitorServiceStatus.CompMonitorServiceStatusPeriodList.Count > 0 Then
                                        If (ObjCompMonitorServiceStatus.IsApplicable = True) And (Not (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True)) Then
                                            ATAChapter = ObjCompMonitorServiceStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorServiceStatus.ATANomenclature
                                            Description = ObjCompMonitorServiceStatus.Description
                                            PartNo = ObjCompStatus.PartName
                                            CompSerialNo = ObjCompStatus.CompSerialNo
                                            Position = ObjCompStatus.Position
                                            MonitorTypeCode = ObjCompMonitorServiceStatus.Code
                                            EstimatedDate = ObjCompMonitorServiceStatus.EstimatedDateFormatted
                                            AssemblyModel = ObjAssemblyStatus.Model
                                            AssemblySerialNo = ObjAssemblyStatus.SerialNo & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyStatus.Position
                                            MinimumRemainingValue = ObjCompMonitorServiceStatus.MinimumRemainingValue
                                            AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID
                                            StatusMasterID = ObjCompMonitorServiceStatus.PartMonitorServiceID
                                            DueStatus = ObjCompMonitorServiceStatus.DueStatus
                                            DocumentTypeForID = 0
                                            Remark = ObjCompStatus.InstallationRemark + " " + ObjCompMonitorServiceStatus.DoneRemark
                                            Code = ObjCompMonitorServiceStatus.PartMonitorServiceCode
                                            DoneOnDate = ObjCompMonitorServiceStatus.DoneOn
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

                                            For Each ObjCompMonitorServiceStatusPeriod In ObjCompMonitorServiceStatus.CompMonitorServiceStatusPeriodList
                                                If ObjCompMonitorServiceStatusPeriod.PeriodID = 1 Then
                                                    Freq1 = ObjCompMonitorServiceStatusPeriod.FrequencyValue
                                                    ElapsedTime = ObjCompMonitorServiceStatusPeriod.ElapsedValue
                                                    RemainingTime = ObjCompMonitorServiceStatusPeriod.RemainingValue
                                                    DoneAt = ObjCompMonitorServiceStatusPeriod.DoneOnValue
                                                    If ((Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL")) Then
                                                        AssemblyDueAsof = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                        If DoneOnDate <> "" Then DoneAt = ObjCompMonitorServiceStatusPeriod.AssemblyDoneOnValueTextByAirFrame
                                                    ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
                                                        AssemblyDueAsof = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                    Else
                                                        AssemblyDueAsof = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText
                                                    End If
                                                    DueAsof = ObjCompMonitorServiceStatusPeriod.DueOnValue
                                                    SinceNew = ObjCompMonitorServiceStatusPeriod.CompCurrentValue
                                                    Extension = ObjCompMonitorServiceStatusPeriod.ExtensionValue
                                                End If
                                                If ObjCompMonitorServiceStatusPeriod.PeriodID = 2 Then
                                                    Freq2 = ObjCompMonitorServiceStatusPeriod.FrequencyValueFormatted
                                                    ElapsedTime1 = ObjCompMonitorServiceStatusPeriod.ElapsedValueFormatted
                                                    RemainingTime1 = ObjCompMonitorServiceStatusPeriod.RemainingValueFormatted
                                                    AssemblyDueAsof1 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                    DueAsof1 = ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                    SinceNew1 = ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted
                                                    DoneAt1 = ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                    Extension1 = ObjCompMonitorServiceStatusPeriod.ExtensionValueFormatted
                                                End If
												'If ObjCompMonitorServiceStatusPeriod.PeriodID = 3 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 4 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 5 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 6 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 7 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 8 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 12 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 13 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 14 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 15 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 11 Then
												'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
												If ObjCompMonitorServiceStatusPeriod.PeriodID >= 3 Then
													If Freq3 = "" Then
														Freq3 = ObjCompMonitorServiceStatusPeriod.FrequencyValue
														ElapsedTime2 = ObjCompMonitorServiceStatusPeriod.ElapsedValue
														RemainingTime2 = ObjCompMonitorServiceStatusPeriod.RemainingValue
														If ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Then
															AssemblyDueAsof2 = ""
														Else
															If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
																AssemblyDueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
															Else
																AssemblyDueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText
															End If
														End If
														DueAsof2 = ObjCompMonitorServiceStatusPeriod.DueOnValue
														SinceNew2 = ObjCompMonitorServiceStatusPeriod.CompCurrentValue
														DoneAt2 = ObjCompMonitorServiceStatusPeriod.DoneOnValue
														Extension2 = ObjCompMonitorServiceStatusPeriod.ExtensionValue
													Else
														Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.FrequencyValue
														ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedValue
														RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.RemainingValue
														If ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Then
															AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ""
														Else
															If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
																AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
															Else
																AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText
															End If
														End If
														DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
														SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.CompCurrentValue
														DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DoneOnValue
														Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ExtensionValue
													End If
												End If
											Next
                                            AssemblyID = ObjAssemblyStatus.AssemblyID
                                            AssemblyType = ObjAssemblyStatus.AssemblyType
                                            RegNo = ObjMachine.RegNo
                                            If IsPreviewClicked Then
                                                RequiredManHours = PartMonitorService.GetPartMonitorService(ObjCompMonitorServiceStatus.PartMonitorServiceID).RequiredManHours
                                            Else
                                                RequiredManHours = ObjCompMonitorServiceStatus.RequiredManHours
                                            End If
                                            Customer = ObjMachine.Customer
                                            Note = ObjCompMonitorServiceStatus.Notes
                                            If ObjCompMonitorServiceStatus.Reference <> "" Then
                                                MaintenanceEvent = ObjCompMonitorServiceStatus.Type & " (" & ObjCompMonitorServiceStatus.MonitorType & ")" & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatus.Reference
                                            Else
                                                MaintenanceEvent = ObjCompMonitorServiceStatus.Type & " (" & ObjCompMonitorServiceStatus.MonitorType & ")"
                                            End If
                                            ExtensionDate = ObjCompMonitorServiceStatus.ExtensionDate
                                            ApprovalRemark = ObjCompMonitorServiceStatus.ApprovalRemark
                                            StatusID = ObjCompMonitorServiceStatus.ID
                                            'If chkwithWONo.Checked = True Or IsPreviewClicked Then
                                            mnWOListForDueJobs = nWOListForDueJobs.GetWOListForDueJobs(StatusID)
                                            If mnWOListForDueJobs.Count > 0 Then
                                                nWONumber = mnWOListForDueJobs(0).WONumber + vbCrLf + mnWOListForDueJobs(0).WODateFormatted
                                            Else
                                                nWONumber = ""
                                            End If
                                            'End If

                                            Zone = ""
                                            Area = ""
                                            IsRII = False
                                            If (AppSettings("ClientCode") = "APFT" Or
                                                AppSettings("ClientCode") = "AAP") And
                                               ObjCompMonitorServiceStatus.PartMonitorServiceTypeID = 5 Then
                                                DoneAt = "----"
                                                DoneAt1 = ""
                                                DoneAt2 = ""
                                                Freq1 = "----"
                                                Freq2 = ""
                                                Freq3 = ""
                                            End If
                                            ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, MaintenanceEvent, AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, ObjCompMonitorServiceStatus.MonitorType, MonitorTypeCode, Note, Remark, Description, _
                                                                 , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, _
                                                                 AssemblyTypeID, MaintenanceEvent, , , , , , , , , , , , , , , ObjCompMonitorServiceStatus.Reference, , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , ObjCompMonitorServiceStatus.IsApplicable, StatusID.ToString, CompStatusID:=ObjCompStatus.ID.ToString, AssemblyStatusID:=ObjAssemblyStatus.ID.ToString, DueStatus:=DueStatus, WONumber:=nWONumber, MachineID:=ObjMachine.MachineID.ToString, ModelID:=ObjAssemblyStatus.ModelID.ToString, MaintenanceTypeID:=8, IsMaster:=ObjCompMonitorServiceStatus.IsMaster, Zone:=Zone, Area:=Area, IsRII:=IsRII))
                                        End If
                                    End If
                                End If
                            Next
                        Next
                    Next
                Next
            End If

            If IsInsSelect = True Then
                For Each ObjMachine In mMachineList
                    For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                        For Each ObjAssemblyMonitorInspStatus In ObjAssemblyStatus.AssemblyMonitorInspStatusList
                            If InspectionTypeID.Contains(ObjAssemblyMonitorInspStatus.ModelMonitorInspTypeID) Then
                                If ObjAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriodList.Count > 0 Then
                                    If (ObjAssemblyMonitorInspStatus.IsApplicable = True) And (Not (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True)) Then
                                        ATAChapter = ObjAssemblyMonitorInspStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorInspStatus.ATANomenclature
                                        Description = ObjAssemblyMonitorInspStatus.Description
                                        AssemblyModel = ObjAssemblyStatus.Model
                                        AssemblySerialNo = ObjAssemblyStatus.SerialNo & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyStatus.Position
                                        Position = ""
                                        MonitorTypeCode = ObjAssemblyMonitorInspStatus.Code
                                        EstimatedDate = ObjAssemblyMonitorInspStatus.EstimatedDateFormatted
                                        MinimumRemainingValue = ObjAssemblyMonitorInspStatus.MinimumRemainingValue
                                        AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID
                                        StatusMasterID = ObjAssemblyMonitorInspStatus.ModelMonitorInspID
                                        DueStatus = ObjAssemblyMonitorInspStatus.DueStatus
                                        DocumentTypeForID = 9
                                        DoneOnDate = ObjAssemblyMonitorInspStatus.DoneOn
                                        Code = ObjAssemblyMonitorInspStatus.ModelMonitorInspCode
                                        Remark = ObjAssemblyStatus.InstallationRemark + " " + ObjAssemblyMonitorInspStatus.DoneRemark

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
                                        For Each ObjAssemblyMonitorInspStatusPeriod In ObjAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriodList
                                            If ObjAssemblyMonitorInspStatusPeriod.PeriodID = 1 Then
                                                Freq1 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValue
                                                ElapsedTime = ObjAssemblyMonitorInspStatusPeriod.ElapsedValue
                                                RemainingTime = ObjAssemblyMonitorInspStatusPeriod.RemainingValue
                                                DueAsof = ObjAssemblyMonitorInspStatusPeriod.DueOnValue
                                                DoneAt = ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
                                                If ((Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL")) Then
                                                    AssemblyDueAsof = ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                    If DoneOnDate <> "" Then DoneAt = ObjAssemblyMonitorInspStatusPeriod.AssemblyDoneOnValueTextByAirFrame
                                                ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
                                                    AssemblyDueAsof = ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame

                                                Else
                                                    AssemblyDueAsof = ObjAssemblyMonitorInspStatusPeriod.DueOnValue
                                                End If
                                                SinceNew = ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue
                                                Extension = ObjAssemblyMonitorInspStatusPeriod.ExtensionValue
                                            End If
                                            If ObjAssemblyMonitorInspStatusPeriod.PeriodID = 2 Then
                                                Freq2 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValueFormatted
                                                ElapsedTime1 = ObjAssemblyMonitorInspStatusPeriod.ElapsedValueFormatted
                                                RemainingTime1 = ObjAssemblyMonitorInspStatusPeriod.RemainingValueFormatted
                                                DueAsof1 = ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
                                                AssemblyDueAsof1 = ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
                                                SinceNew1 = ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValueFormatted
                                                DoneAt1 = ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted

                                                Extension1 = ObjAssemblyMonitorInspStatusPeriod.ExtensionValueFormatted
                                            End If
											'If ObjAssemblyMonitorInspStatusPeriod.PeriodID = 3 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 4 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 5 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 6 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 7 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 8 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 9 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 12 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 13 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 14 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 15 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 11 Then
											'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
											If ObjAssemblyMonitorInspStatusPeriod.PeriodID >= 3 Then
												If Freq3 = "" Then
													Freq3 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValue
													ElapsedTime2 = ObjAssemblyMonitorInspStatusPeriod.ElapsedValue
													RemainingTime2 = ObjAssemblyMonitorInspStatusPeriod.RemainingValue
													DueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValue

													If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
														AssemblyDueAsof2 = ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame
													Else
														AssemblyDueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValue
													End If

													SinceNew2 = ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue
													DoneAt2 = ObjAssemblyMonitorInspStatusPeriod.DoneOnValue

													Extension2 = ObjAssemblyMonitorInspStatusPeriod.ExtensionValue
												Else
													Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.FrequencyValue
													ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.ElapsedValue
													RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.RemainingValue
													DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.DueOnValue

													If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
														AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueByAirFrame
													Else
														AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.DueOnValue
													End If

													SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue
													DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.DoneOnValue

													Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.ExtensionValue
												End If
											End If
										Next
                                        AssemblyID = ObjAssemblyStatus.AssemblyID
                                        AssemblyType = ObjAssemblyStatus.AssemblyType
                                        RegNo = ObjMachine.RegNo
                                        If IsPreviewClicked Then
                                            RequiredManHours = ModelMonitorInsp.GetModelMonitorInsp(ObjAssemblyMonitorInspStatus.ModelMonitorInspID).RequiredManHours
                                        Else
                                            RequiredManHours = ObjAssemblyMonitorInspStatus.RequiredManHours
                                        End If
                                        Customer = ObjMachine.Customer
                                        Note = ObjAssemblyMonitorInspStatus.Notes
                                        If ObjAssemblyMonitorInspStatus.Reference <> "" Then
                                            MaintenanceEvent = ObjAssemblyMonitorInspStatus.Type & " (" & ObjAssemblyMonitorInspStatus.MonitorType & ")" & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatus.Reference
                                        Else
                                            MaintenanceEvent = ObjAssemblyMonitorInspStatus.Type & " (" & ObjAssemblyMonitorInspStatus.MonitorType & ")"
                                        End If
                                        ExtensionDate = ObjAssemblyMonitorInspStatus.ExtensionDate
                                        ApprovalRemark = ObjAssemblyMonitorInspStatus.ApprovalRemark
                                        StatusID = ObjAssemblyMonitorInspStatus.ID
                                        'If chkwithWONo.Checked = True Or IsPreviewClicked Then
                                        mnWOListForDueJobs = nWOListForDueJobs.GetWOListForDueJobs(StatusID)
                                        If mnWOListForDueJobs.Count > 0 Then
                                            nWONumber = mnWOListForDueJobs(0).WONumber + vbCrLf + mnWOListForDueJobs(0).WODateFormatted
                                        Else
                                            nWONumber = ""
                                        End If
                                        'End If
                                        Zone = ObjAssemblyMonitorInspStatus.Zone
                                        Area = ObjAssemblyMonitorInspStatus.Area
                                        IsRII = ObjAssemblyMonitorInspStatus.IsRII
                                        ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, , AssemblySerialNo, ATAChapter, , , Position, ObjAssemblyMonitorInspStatus.MonitorType, MonitorTypeCode, Note, Remark, Description, _
                                                                                  , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, _
                                                                                  SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, _
                                                                                  AssemblyTypeID, MaintenanceEvent, , , , , , , , , , , , , , , ObjAssemblyMonitorInspStatus.Reference, , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , ObjAssemblyMonitorInspStatus.IsApplicable, StatusID.ToString, AssemblyStatusID:=ObjAssemblyStatus.ID.ToString, DueStatus:=DueStatus, WONumber:=nWONumber, MachineID:=ObjMachine.MachineID.ToString, ModelID:=ObjAssemblyStatus.ModelID.ToString, MaintenanceTypeID:=6, IsMaster:=ObjAssemblyMonitorInspStatus.IsMaster, Zone:=Zone, Area:=Area, IsRII:=IsRII))
                                    End If
                                End If
                            End If
                        Next
                        For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
                            For Each ObjCompMonitorInspStatus In ObjCompStatus.CompMonitorInspStatusList
                                If InspectionTypeID.Contains(ObjCompMonitorInspStatus.PartMonitorInspTypeID) Then
                                    If ObjCompMonitorInspStatus.CompMonitorInspStatusPeriodList.Count > 0 Then
                                        If (ObjCompMonitorInspStatus.IsApplicable = True) And (Not (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True)) Then
                                            ATAChapter = ObjCompMonitorInspStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorInspStatus.ATANomenclature
                                            Description = ObjCompMonitorInspStatus.Description
                                            PartNo = ObjCompStatus.PartName
                                            CompSerialNo = ObjCompStatus.CompSerialNo
                                            Position = ObjCompStatus.Position
                                            MonitorTypeCode = ObjCompMonitorInspStatus.Code
                                            EstimatedDate = ObjCompMonitorInspStatus.EstimatedDateFormatted
                                            AssemblyModel = ObjAssemblyStatus.Model
                                            AssemblySerialNo = ObjAssemblyStatus.SerialNo & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyStatus.Position
                                            MinimumRemainingValue = ObjCompMonitorInspStatus.MinimumRemainingValue
                                            AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID
                                            StatusMasterID = ObjCompMonitorInspStatus.PartMonitorInspID
                                            DueStatus = ObjCompMonitorInspStatus.DueStatus
                                            DocumentTypeForID = 11
                                            Remark = ObjCompStatus.InstallationRemark + " " + ObjCompMonitorInspStatus.DoneRemark
                                            Code = ObjCompMonitorInspStatus.PartMonitorInspCode
                                            DoneOnDate = ObjCompMonitorInspStatus.DoneOn

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

                                            For Each ObjCompMonitorInspStatusPeriod In ObjCompMonitorInspStatus.CompMonitorInspStatusPeriodList
                                                If ObjCompMonitorInspStatusPeriod.PeriodID = 1 Then
                                                    Freq1 = ObjCompMonitorInspStatusPeriod.FrequencyValue
                                                    ElapsedTime = ObjCompMonitorInspStatusPeriod.ElapsedValue
                                                    RemainingTime = ObjCompMonitorInspStatusPeriod.RemainingValue
                                                    DoneAt = ObjCompMonitorInspStatusPeriod.DoneOnValue
                                                    If ((Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL")) Then
                                                        AssemblyDueAsof = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                        If DoneOnDate <> "" Then DoneAt = ObjCompMonitorInspStatusPeriod.AssemblyDoneOnValueTextByAirFrame
                                                    ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
                                                        AssemblyDueAsof = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                    Else
                                                        AssemblyDueAsof = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText
                                                    End If
                                                    DueAsof = ObjCompMonitorInspStatusPeriod.DueOnValue
                                                    SinceNew = ObjCompMonitorInspStatusPeriod.CompCurrentValue
                                                    Extension = ObjCompMonitorInspStatusPeriod.ExtensionValue
                                                End If
                                                If ObjCompMonitorInspStatusPeriod.PeriodID = 2 Then
                                                    Freq2 = ObjCompMonitorInspStatusPeriod.FrequencyValueFormatted
                                                    ElapsedTime1 = ObjCompMonitorInspStatusPeriod.ElapsedValueFormatted
                                                    RemainingTime1 = ObjCompMonitorInspStatusPeriod.RemainingValueFormatted
                                                    AssemblyDueAsof1 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                    DueAsof1 = ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
                                                    SinceNew1 = ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted
                                                    DoneAt1 = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                    Extension1 = ObjCompMonitorInspStatusPeriod.ExtensionValueFormatted
                                                End If

												'If ObjCompMonitorInspStatusPeriod.PeriodID = 3 Or ObjCompMonitorInspStatusPeriod.PeriodID = 4 Or ObjCompMonitorInspStatusPeriod.PeriodID = 5 Or ObjCompMonitorInspStatusPeriod.PeriodID = 6 Or ObjCompMonitorInspStatusPeriod.PeriodID = 7 Or ObjCompMonitorInspStatusPeriod.PeriodID = 8 Or ObjCompMonitorInspStatusPeriod.PeriodID = 9 Or ObjCompMonitorInspStatusPeriod.PeriodID = 12 Or ObjCompMonitorInspStatusPeriod.PeriodID = 13 Or ObjCompMonitorInspStatusPeriod.PeriodID = 14 Or ObjCompMonitorInspStatusPeriod.PeriodID = 15 Or ObjCompMonitorInspStatusPeriod.PeriodID = 11 Then
												'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
												If ObjCompMonitorInspStatusPeriod.PeriodID >= 3 Then
													If Freq3 = "" Then
														Freq3 = ObjCompMonitorInspStatusPeriod.FrequencyValue
														ElapsedTime2 = ObjCompMonitorInspStatusPeriod.ElapsedValue
														RemainingTime2 = ObjCompMonitorInspStatusPeriod.RemainingValue
														If ObjCompMonitorInspStatusPeriod.PeriodID = 9 Then
															AssemblyDueAsof2 = ""
														Else
															If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
																AssemblyDueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame
															Else
																AssemblyDueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText
															End If

														End If
														DueAsof2 = ObjCompMonitorInspStatusPeriod.DueOnValue
														SinceNew2 = ObjCompMonitorInspStatusPeriod.CompCurrentValue
														DoneAt2 = ObjCompMonitorInspStatusPeriod.DoneOnValue
														Extension2 = ObjCompMonitorInspStatusPeriod.ExtensionValue
													Else
														Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.FrequencyValue
														ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.ElapsedValue
														RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.RemainingValue
														If ObjCompMonitorInspStatusPeriod.PeriodID = 9 Then
															AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ""
														Else
															If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
																AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame
															Else
																AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText
															End If

														End If
														DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DueOnValue
														SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.CompCurrentValue
														DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DoneOnValue
														Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.ExtensionValue
													End If
												End If
											Next
                                            AssemblyID = ObjAssemblyStatus.AssemblyID
                                            AssemblyType = ObjAssemblyStatus.AssemblyType
                                            RegNo = ObjMachine.RegNo
                                            If IsPreviewClicked Then
                                                RequiredManHours = PartMonitorInsp.GetPartMonitorInsp(ObjCompMonitorInspStatus.PartMonitorInspID).RequiredManHours
                                            Else
                                                RequiredManHours = ObjCompMonitorInspStatus.RequiredManHours
                                            End If
                                            Customer = ObjMachine.Customer
                                            Note = ObjCompMonitorInspStatus.Notes
                                            If ObjCompMonitorInspStatus.Reference <> "" Then
                                                MaintenanceEvent = ObjCompMonitorInspStatus.Type & " (" & ObjCompMonitorInspStatus.MonitorType & ")" & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatus.Reference
                                            Else
                                                MaintenanceEvent = ObjCompMonitorInspStatus.Type & " (" & ObjCompMonitorInspStatus.MonitorType & ")"
                                            End If
                                            ExtensionDate = ObjCompMonitorInspStatus.ExtensionDate
                                            ApprovalRemark = ObjCompMonitorInspStatus.ApprovalRemark
                                            StatusID = ObjCompMonitorInspStatus.ID
                                            'If chkwithWONo.Checked = True Or IsPreviewClicked Then
                                            mnWOListForDueJobs = nWOListForDueJobs.GetWOListForDueJobs(StatusID)
                                            If mnWOListForDueJobs.Count > 0 Then
                                                nWONumber = mnWOListForDueJobs(0).WONumber + vbCrLf + mnWOListForDueJobs(0).WODateFormatted
                                            Else
                                                nWONumber = ""
                                            End If
                                            'End If
                                            Zone = ""
                                            Area = ""
                                            IsRII = False
                                            ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, MaintenanceEvent, AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, ObjCompMonitorInspStatus.MonitorType, MonitorTypeCode, Note, Remark, Description, _
                                                                 , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, _
                                                                 AssemblyTypeID, MaintenanceEvent, , , , , , , , , , , , , , , ObjCompMonitorInspStatus.Reference, , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , ObjCompMonitorInspStatus.IsApplicable, StatusID.ToString, CompStatusID:=ObjCompStatus.ID.ToString, AssemblyStatusID:=ObjAssemblyStatus.ID.ToString, DueStatus:=DueStatus, WONumber:=nWONumber, MachineID:=ObjMachine.MachineID.ToString, ModelID:=ObjAssemblyStatus.ModelID.ToString, MaintenanceTypeID:=9, IsMaster:=ObjCompMonitorInspStatus.IsMaster, Zone:=Zone, Area:=Area, IsRII:=IsRII))
                                        End If
                                    End If
                                End If
                            Next
                        Next
                    Next
                Next
            End If
            If IsModSelect = True Then
                For Each ObjMachine In mMachineList
                    For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                        For Each ObjAssemblyMonitorModStatus In ObjAssemblyStatus.AssemblyMonitorModStatusList
                            If ModificationTypeID.Contains(ObjAssemblyMonitorModStatus.ModelMonitorModTypeID) Then
                                If ObjAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriodList.Count > 0 Then
                                    If (ObjAssemblyMonitorModStatus.IsApplicable = True) And (Not (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True)) Then
                                        ATAChapter = ObjAssemblyMonitorModStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorModStatus.ATANomenclature
                                        Description = ObjAssemblyMonitorModStatus.Number & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatus.Description
                                        AssemblyModel = ObjAssemblyStatus.Model
                                        AssemblySerialNo = ObjAssemblyStatus.SerialNo & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyStatus.Position
                                        Position = ""
                                        MonitorTypeCode = ObjAssemblyMonitorModStatus.Code
                                        EstimatedDate = ObjAssemblyMonitorModStatus.EstimatedDateFormatted
                                        MinimumRemainingValue = ObjAssemblyMonitorModStatus.MinimumRemainingValue
                                        AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID
                                        StatusMasterID = ObjAssemblyMonitorModStatus.ModelMonitorModID
                                        DueStatus = ObjAssemblyMonitorModStatus.DueStatus
                                        DocumentTypeForID = 8
                                        Remark = ObjAssemblyStatus.InstallationRemark + " " + ObjAssemblyMonitorModStatus.DoneRemark
                                        Code = ObjAssemblyMonitorModStatus.ModelMonitorModCode
                                        DoneOnDate = ObjAssemblyMonitorModStatus.DoneOn
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

                                        For Each ObjAssemblyMonitorModStatusPeriod In ObjAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriodList
                                            If ObjAssemblyMonitorModStatusPeriod.PeriodID = 1 Then
                                                Freq1 = ObjAssemblyMonitorModStatusPeriod.FrequencyValue
                                                ElapsedTime = ObjAssemblyMonitorModStatusPeriod.ElapsedValue
                                                RemainingTime = ObjAssemblyMonitorModStatusPeriod.RemainingValue
                                                DueAsof = ObjAssemblyMonitorModStatusPeriod.DueOnValue
                                                DoneAt = ObjAssemblyMonitorModStatusPeriod.DoneOnValue
                                                If ((Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL")) Then
                                                    AssemblyDueAsof = ObjAssemblyMonitorModStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                    If DoneOnDate <> "" Then DoneAt = ObjAssemblyMonitorModStatusPeriod.AssemblyDoneOnValueTextByAirFrame
                                                ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
                                                    AssemblyDueAsof = ObjAssemblyMonitorModStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                Else
                                                    AssemblyDueAsof = ObjAssemblyMonitorModStatusPeriod.DueOnValue

                                                End If
                                                SinceNew = ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValue
                                                Extension = ObjAssemblyMonitorModStatusPeriod.ExtensionValue
                                            End If
                                            If ObjAssemblyMonitorModStatusPeriod.PeriodID = 2 Then
                                                Freq2 = ObjAssemblyMonitorModStatusPeriod.FrequencyValueFormatted
                                                ElapsedTime1 = ObjAssemblyMonitorModStatusPeriod.ElapsedValueFormatted
                                                RemainingTime1 = ObjAssemblyMonitorModStatusPeriod.RemainingValueFormatted
                                                DueAsof1 = ObjAssemblyMonitorModStatusPeriod.DueOnValueFormatted
                                                AssemblyDueAsof1 = ObjAssemblyMonitorModStatusPeriod.DueOnValueFormatted
                                                SinceNew1 = ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValueFormatted
                                                DoneAt1 = ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted
                                                Extension1 = ObjAssemblyMonitorModStatusPeriod.ExtensionValueFormatted
                                            End If

											'If ObjAssemblyMonitorModStatusPeriod.PeriodID = 3 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 4 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 5 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 6 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 7 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 8 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 9 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 12 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 13 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 14 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 15 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 11 Then
											'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
											If ObjAssemblyMonitorModStatusPeriod.PeriodID >= 3 Then
												If Freq3 = "" Then
													Freq3 = ObjAssemblyMonitorModStatusPeriod.FrequencyValue
													ElapsedTime2 = ObjAssemblyMonitorModStatusPeriod.ElapsedValue
													RemainingTime2 = ObjAssemblyMonitorModStatusPeriod.RemainingValue
													DueAsof2 = ObjAssemblyMonitorModStatusPeriod.DueOnValue

													If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
														AssemblyDueAsof2 = ObjAssemblyMonitorModStatusPeriod.AssemblyDueOnValueByAirFrame
													Else
														AssemblyDueAsof2 = ObjAssemblyMonitorModStatusPeriod.DueOnValue
													End If
													SinceNew2 = ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValue
													DoneAt2 = ObjAssemblyMonitorModStatusPeriod.DoneOnValue
													Extension2 = ObjAssemblyMonitorModStatusPeriod.ExtensionValue
												Else
													Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.FrequencyValue
													ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.ElapsedValue
													RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.RemainingValue
													DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.DueOnValue

													If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
														AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.AssemblyDueOnValueByAirFrame
													Else
														AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.DueOnValue
													End If

													SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValue
													DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.DoneOnValue
													Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.ExtensionValue
												End If
											End If
										Next
                                        AssemblyID = ObjAssemblyStatus.AssemblyID
                                        AssemblyType = ObjAssemblyStatus.AssemblyType
                                        RegNo = ObjMachine.RegNo
                                        If IsPreviewClicked Then
                                            RequiredManHours = ModelMonitorMod.GetModelMonitorMod(ObjAssemblyMonitorModStatus.ModelMonitorModID).RequiredManHours
                                        Else
                                            RequiredManHours = ObjAssemblyMonitorModStatus.RequiredManHours
                                        End If
                                        Customer = ObjMachine.Customer
                                        Note = ObjAssemblyMonitorModStatus.Notes
                                        If ObjAssemblyMonitorModStatus.Reference <> "" Then
                                            MaintenanceEvent = ObjAssemblyMonitorModStatus.Type & " (" & ObjAssemblyMonitorModStatus.MonitorType & ")" & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatus.Reference
                                        Else
                                            MaintenanceEvent = ObjAssemblyMonitorModStatus.Type & " (" & ObjAssemblyMonitorModStatus.MonitorType & ")"
                                        End If
                                        ExtensionDate = ObjAssemblyMonitorModStatus.ExtensionDate
                                        ApprovalRemark = ObjAssemblyMonitorModStatus.ApprovalRemark
                                        StatusID = ObjAssemblyMonitorModStatus.ID
                                        'If chkwithWONo.Checked = True Or IsPreviewClicked Then
                                        mnWOListForDueJobs = nWOListForDueJobs.GetWOListForDueJobs(StatusID)
                                        If mnWOListForDueJobs.Count > 0 Then
                                            nWONumber = mnWOListForDueJobs(0).WONumber + vbCrLf + mnWOListForDueJobs(0).WODateFormatted
                                        Else
                                            nWONumber = ""
                                        End If
                                        'End If
                                        Zone = ObjAssemblyMonitorModStatus.Zone
                                        Area = ObjAssemblyMonitorModStatus.Area
                                        IsRII = ObjAssemblyMonitorModStatus.IsRII
                                        ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, , AssemblySerialNo, ATAChapter, , , Position, ObjAssemblyMonitorModStatus.MonitorType, MonitorTypeCode, Note, Remark, Description, _
                                           , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, _
                                           SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, AssemblyTypeID, MaintenanceEvent, , , , , , , , , , , , , , ObjAssemblyMonitorModStatus.Number, ObjAssemblyMonitorModStatus.Reference, , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , ObjAssemblyMonitorModStatus.IsApplicable, StatusID.ToString, AssemblyStatusID:=ObjAssemblyStatus.ID.ToString, DueStatus:=DueStatus, WONumber:=nWONumber, MachineID:=ObjMachine.MachineID.ToString, ModelID:=ObjAssemblyStatus.ModelID.ToString, MaintenanceTypeID:=7, IsMaster:=ObjAssemblyMonitorModStatus.IsMaster, Zone:=Zone, Area:=Area, IsRII:=IsRII))
                                    End If
                                End If
                            End If
                        Next
                        For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
                            For Each ObjCompMonitorModStatus In ObjCompStatus.CompMonitorModStatusList
                                If ModificationTypeID.Contains(ObjCompMonitorModStatus.PartMonitorModTypeID) Then
                                    If ObjCompMonitorModStatus.CompMonitorModStatusPeriodList.Count > 0 Then
                                        If (ObjCompMonitorModStatus.IsApplicable = True) And (Not (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted)) Then
                                            ATAChapter = ObjCompMonitorModStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorModStatus.ATANomenclature
                                            Description = ObjCompMonitorModStatus.Description & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatus.Number
                                            PartNo = ObjCompStatus.PartName
                                            CompSerialNo = ObjCompStatus.CompSerialNo
                                            Position = ObjCompStatus.Position
                                            MonitorTypeCode = ObjCompMonitorModStatus.Code
                                            EstimatedDate = ObjCompMonitorModStatus.EstimatedDateFormatted
                                            AssemblyModel = ObjAssemblyStatus.Model
                                            AssemblySerialNo = ObjAssemblyStatus.SerialNo & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyStatus.Position
                                            MinimumRemainingValue = ObjCompMonitorModStatus.MinimumRemainingValue
                                            AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID
                                            StatusMasterID = ObjCompMonitorModStatus.PartMonitorModID
                                            DueStatus = ObjCompMonitorModStatus.DueStatus
                                            DocumentTypeForID = 10
                                            Remark = ObjCompStatus.InstallationRemark + " " + ObjCompMonitorModStatus.DoneRemark
                                            Code = ObjCompMonitorModStatus.PartMonitorModCode
                                            DoneOnDate = ObjCompMonitorModStatus.DoneOn
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
                                            For Each ObjCompMonitorModStatusPeriod In ObjCompMonitorModStatus.CompMonitorModStatusPeriodList
                                                If ObjCompMonitorModStatusPeriod.PeriodID = 1 Then
                                                    Freq1 = ObjCompMonitorModStatusPeriod.FrequencyValue
                                                    ElapsedTime = ObjCompMonitorModStatusPeriod.ElapsedValue
                                                    RemainingTime = ObjCompMonitorModStatusPeriod.RemainingValue
                                                    DoneAt = ObjCompMonitorModStatusPeriod.DoneOnValue
                                                    If ((Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Heligo" Or AppSettings("ClientCode") = "UHPL")) Then
                                                        AssemblyDueAsof = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                        If DoneOnDate <> "" Then DoneAt = ObjCompMonitorModStatusPeriod.AssemblyDoneOnValueTextByAirFrame
                                                    ElseIf (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
                                                        AssemblyDueAsof = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                    Else
                                                        AssemblyDueAsof = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueText
                                                    End If
                                                    DueAsof = ObjCompMonitorModStatusPeriod.DueOnValue
                                                    SinceNew = ObjCompMonitorModStatusPeriod.CompCurrentValue
                                                    Extension = ObjCompMonitorModStatusPeriod.ExtensionValue
                                                End If
                                                If ObjCompMonitorModStatusPeriod.PeriodID = 2 Then
                                                    Freq2 = ObjCompMonitorModStatusPeriod.FrequencyValueFormatted
                                                    ElapsedTime1 = ObjCompMonitorModStatusPeriod.ElapsedValueFormatted
                                                    RemainingTime1 = ObjCompMonitorModStatusPeriod.RemainingValueFormatted
                                                    AssemblyDueAsof1 = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormatted
                                                    DueAsof1 = ObjCompMonitorModStatusPeriod.DueOnValueFormatted
                                                    SinceNew1 = ObjCompMonitorModStatusPeriod.CompCurrentValueFormatted
                                                    DoneAt1 = ObjCompMonitorModStatusPeriod.DoneOnValueFormatted
                                                    Extension1 = ObjCompMonitorModStatusPeriod.ExtensionValueFormatted
                                                End If

												'If ObjCompMonitorModStatusPeriod.PeriodID = 3 Or ObjCompMonitorModStatusPeriod.PeriodID = 4 Or ObjCompMonitorModStatusPeriod.PeriodID = 5 Or ObjCompMonitorModStatusPeriod.PeriodID = 6 Or ObjCompMonitorModStatusPeriod.PeriodID = 7 Or ObjCompMonitorModStatusPeriod.PeriodID = 8 Or ObjCompMonitorModStatusPeriod.PeriodID = 9 Or ObjCompMonitorModStatusPeriod.PeriodID = 12 Or ObjCompMonitorModStatusPeriod.PeriodID = 13 Or ObjCompMonitorModStatusPeriod.PeriodID = 14 Or ObjCompMonitorModStatusPeriod.PeriodID = 15 Or ObjCompMonitorModStatusPeriod.PeriodID = 11 Then
												'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
												If ObjCompMonitorModStatusPeriod.PeriodID >= 3 Then
													If Freq3 = "" Then
														Freq3 = ObjCompMonitorModStatusPeriod.FrequencyValue
														ElapsedTime2 = ObjCompMonitorModStatusPeriod.ElapsedValue
														RemainingTime2 = ObjCompMonitorModStatusPeriod.RemainingValue
														If ObjCompMonitorModStatusPeriod.PeriodID = 9 Then
															AssemblyDueAsof2 = ""
														Else
															If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
																AssemblyDueAsof2 = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextByAirFrame
															Else
																AssemblyDueAsof2 = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueText
															End If

														End If
														DueAsof2 = ObjCompMonitorModStatusPeriod.DueOnValue
														SinceNew2 = ObjCompMonitorModStatusPeriod.CompCurrentValue
														DoneAt2 = ObjCompMonitorModStatusPeriod.DoneOnValue
														Extension2 = ObjCompMonitorModStatusPeriod.ExtensionValue
													Else
														Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.FrequencyValue
														ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.ElapsedValue
														RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.RemainingValue
														If ObjCompMonitorModStatusPeriod.PeriodID = 9 Then
															AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & "" 'AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.AssemblyDueOnValueText 
														Else
															If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Then
																AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextByAirFrame
															Else
																AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.AssemblyDueOnValueText
															End If

														End If
														DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.DueOnValue
														SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.CompCurrentValue
														DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.DoneOnValue
														Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.ExtensionValue
													End If
												End If
											Next
                                            AssemblyID = ObjAssemblyStatus.AssemblyID
                                            AssemblyType = ObjAssemblyStatus.AssemblyType
                                            RegNo = ObjMachine.RegNo
                                            If IsPreviewClicked Then
                                                RequiredManHours = PartMonitorMod.GetPartMonitorMod(ObjCompMonitorModStatus.PartMonitorModID).RequiredManHours
                                            Else
                                                RequiredManHours = ObjCompMonitorModStatus.RequiredManHours
                                            End If
                                            Customer = ObjMachine.Customer
                                            Note = ObjCompMonitorModStatus.Notes
                                            If ObjCompMonitorModStatus.Reference <> "" Then
                                                MaintenanceEvent = ObjCompMonitorModStatus.Type & " (" & ObjCompMonitorModStatus.MonitorType & ")" & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatus.Reference
                                            Else
                                                MaintenanceEvent = ObjCompMonitorModStatus.Type & " (" & ObjCompMonitorModStatus.MonitorType & ")"
                                            End If
                                            ExtensionDate = ObjCompMonitorModStatus.ExtensionDate
                                            ApprovalRemark = ObjCompMonitorModStatus.ApprovalRemark
                                            StatusID = ObjCompMonitorModStatus.ID
                                            'If chkwithWONo.Checked = True Or IsPreviewClicked Then
                                            mnWOListForDueJobs = nWOListForDueJobs.GetWOListForDueJobs(StatusID)
                                            If mnWOListForDueJobs.Count > 0 Then
                                                nWONumber = mnWOListForDueJobs(0).WONumber + vbCrLf + mnWOListForDueJobs(0).WODateFormatted
                                            Else
                                                nWONumber = ""
                                            End If
                                            'End If
                                            Zone = ""
                                            Area = ""
                                            IsRII = False
                                            ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, MaintenanceEvent, AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, ObjCompMonitorModStatus.MonitorType, MonitorTypeCode, Note, Remark, Description, _
                                                                  , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, _
                                                                  AssemblyTypeID, MaintenanceEvent, , , , , , , , , , , , , , ObjCompMonitorModStatus.Number, ObjCompMonitorModStatus.Reference, , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , ObjCompMonitorModStatus.IsApplicable, StatusID.ToString, , CompStatusID:=ObjCompStatus.ID.ToString, AssemblyStatusID:=ObjAssemblyStatus.ID.ToString, DueStatus:=DueStatus, WONumber:=nWONumber, MachineID:=ObjMachine.MachineID.ToString, ModelID:=ObjAssemblyStatus.ModelID.ToString, MaintenanceTypeID:=10, IsMaster:=ObjCompMonitorModStatus.IsMaster, Zone:=Zone, Area:=Area, IsRII:=IsRII))
                                        End If
                                    End If
                                End If
                            Next
                        Next
                    Next
                Next
            End If
        Catch ex As Exception
            Dim Day, Month, Year As String
            Day = Format(Today.Date.Day, "0#")
            Month = Format(Today.Date.Month, "0#")
            Year = Format(Today.Date.Year, "0#")
            Dim todaydate As String = Day & Month & Year
            Dim Path As String = AppSettings("DOCPath") & todaydate
            FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (ReportDetail): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        End Try
        Return ReportMaintenanceDetails
    End Function
    Private Sub SetExcel(ReportMaintenanceDetails As ReportMaintenanceDetailList, SearchingCriteria As ReportData, ReportName As String)
        Dim PeriodColumnsForExportToExcel As New List(Of String)
        Dim da As New CSLA.Data.ObjectAdapter
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

        Dim columnToRemove As String() = { _
                                                  "ID", _
                                                  "Code", _
                                                  "Name", _
                                                  "Model", _
                                                  "SerialNo", _
                                                  "MonitorType", _
                                                  "Freq1",
                                                  "Freq2", _
                                                  "Freq3", _
                                                  "ElapsedTime", _
                                                  "ElapsedTime1", _
                                                  "ElapsedTime2", _
                                                  "RemainingTime", _
                                                  "RemainingTime1", _
                                                  "RemainingTime2", _
                                                  "DueAsof", _
                                                  "DueAsof1", _
                                                  "DueAsof2", _
                                                  "AssemblySerialNo", _
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
                                                  "AssemblyTypeID", _
                                                  "MaintenanceEvent", _
                                                  "ATACode", _
                                                  "InstalledAt1", _
                                                  "InstalledAt2", _
                                                  "TSO1", _
                                                  "TSO2", _
                                                  "RemoveAt1", _
                                                  "RemoveAt2", _
                                                  "ModificationNumber", _
                                                  "Reference", _
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
                                                  "IsApplicable", _
                                                  "MaintenanceTypeID", _
                                                  "MaintenanceTypeName", _
                                                  "IsLater", _
                                                  "DueStatus", _
                                                  "TimeSinceNew", _
                                                  "ModelMonitorModCode", _
                                                  "StatusTypeName", _
                                                  "StatusMasterID", _
                                                  "StatusID", _
                                                  "TypeID", _
                                                  "CompStatusID", _
                                                  "AssemblyStatusID", _
                                                  "DocumentTypeForID", _
                                                  "MaintenanceInformation", _
                                                  "LogBook", _
                                                  "RemoveAt", _
                                                  "DoneONValueForAssembly", _
                                                  "MonitorTypeCode", _
                                                  "ATAChapter", _
                                                  "StatusTypeName", _
                                                  "Description", _
                                                  "PartNo", _
                                                  "Position", _
                                                  "CompSerialNo", _
                                                  "InstalledAt", _
                                                  "TSN", _
                                                  "TSO", _
                                                  "InstalledAtDate", _
                                                  "RemoveAtDate", _
                                                  "DoneOnValue", _
                                                  "Frequency", _
                                                  "SinceNewAll", _
                                                  "ElapsedAll", _
                                                  "DoneAtAll", _
                                                  "ExtensionAll", _
                                                  "DueAsofAll", _
                                                  "AssDueAsofAll", _
                                                  "RemainingTimeAll", _
                                                  "MaintenanceInfo", _
                                                  "MaintenanceOn", _
                                                  "EstDate", _
                                                  "DoneOnDate", _
                                                  "ModelEstimatedManHours", _
                                                  "MaintenanceInformationExcel", _
                                                  "MinimumRemainingValue", _
                                                  "MachineID", _
                                                  "ModelID", _
                                                  "IsMaster", _
                                                  "DiffCompInstDoneOnValue", "ThresholdAccordingToTypeIDForExcel", "FrequencyAccordingToTypeIDForExcel", "DueAsOfAssemblyOrCompForExcel", "DueAsOfAirframeForExcel", "RemainingForExcel"
                                    }

        For i As Integer = 0 To columnToRemove.Length - 1
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains(columnToRemove(i)) Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns.Remove(columnToRemove(i))
            End If
        Next
        Dim columnscnt As Integer = ds.Tables("ExcelReportMaintenanceDetailList").Columns.Count

        'set Column Sequence
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("MaintenanceOnExcel").SetOrdinal(0)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("MaintenanceInfoExcel").SetOrdinal(1)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("FrequencyExcel").SetOrdinal(2)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("SinceNewAllExcel").SetOrdinal(3)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("ElapsedAllExcel").SetOrdinal(4)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("EffectiveFromAllExcel").SetOrdinal(5)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("DoneAtAllExcel").SetOrdinal(6)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("ExtensionAllExcel").SetOrdinal(7)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("DueAsofAllExcel").SetOrdinal(8)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("AssDueAsofAllExcel").SetOrdinal(9)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("RemainingTimeAllExcel").SetOrdinal(10)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("EstimatedDate").SetOrdinal(11)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("Note").SetOrdinal(12)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("Remark").SetOrdinal(13)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("WONumber").SetOrdinal(14)

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
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "WONumber" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "WO. No."
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
                                                 "SearchStr7", _
                                                 "SearchStr9", _
                                                 "ProductVersion", _
                                                 "SINote", _
                                                 "CurrencyName", _
                                                 "CurrencySymbol", _
                                                 "SearchStr10", _
                                                 "SearchStr4", _
                                                 "SearchStr12", _
                                                 "SearchStr11" _
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

        Dim dsNew As New DataSet
        dsNew.Clear()

        dsNew.Merge(ds.Tables("ExcelReport"))
        dsNew.Merge(ds.Tables("ExcelReportMaintenanceDetailList"))


        dsNew.Tables("ExcelReport").TableName = "Searching Criteria"
        dsNew.Tables("ExcelReportMaintenanceDetailList").TableName = ReportName
        Session("DataTableToBeFormattedForExportToExcel") = ReportName
		Session("ExcelFileName") = ReportName
		PeriodColumnsForExportToExcel.AddRange(New String() {"Frequency", "Since New", "Elapsed", "Remaining", "Due At", "Done At", "Effective From", "AssemblySerialNo", "Maintenance On", ColumnName, "Extension", "Maintenance Info"})
		Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
        Session("dsNew") = dsNew
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
    End Sub
    Private Sub SetReport(Optional ByVal ByMail As Boolean = False, Optional ByVal ByExcel As Boolean = False, Optional ByVal IsPreviewClicked As Boolean = False)
        Try
            ReportMaintenanceDetails = New ReportMaintenanceDetailList
            ReportStatusList = New rptStatusList
            Dim da As New CSLA.Data.ObjectAdapter
            Dim ds As New dsReportMaintenanceDetail
            Dim rptSnagCorrectiveActionListForDue As MELSnagCorrectiveActionListForDue

            Dim mCompanyDetail As New CompanyDetail
            Dim searchstr As String = ""
            Dim searchstr6 As String = ""
            Dim searchstr8 As String = ""
            Dim OperatorName As String = ""

            SetValues()
            mDueLimits = CType(mDueLimits.Save, DueLimits)
            Session("mDueLimits") = mDueLimits
            ReportDetail(mIsExcel, IsPreviewClicked)

            Dim mloglist As LogList
            mloglist = LogList.GetLogList(New Guid(cmbAircraft.SelectedValue.ToString), , AsonDate)

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
            searchstr = searchstr & ", " & "As On Date:" & txtFromDate.Text.Trim
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
            Dim ReportName As String
            Dim ReportNameForPDF As String
            'Dim rptDueDetail As CrystalDecisions.CrystalReports.Engine.ReportClass
            If AppSettings("TimeFormat") = "HH:mm" Or AppSettings("TimeFormat") = "hh:mm" Then
                rptSnagCorrectiveActionListForDue = MELSnagCorrectiveActionListForDue.GetMELSnagCorrectiveActionListForDue(AsonDate, New Guid(cmbAircraft.SelectedValue.ToString), Guid.Empty, 0, 0, "HH:mm")
            Else
                rptSnagCorrectiveActionListForDue = MELSnagCorrectiveActionListForDue.GetMELSnagCorrectiveActionListForDue(AsonDate, New Guid(cmbAircraft.SelectedValue.ToString), Guid.Empty, 0, 0)
            End If

            If ByMail = True Then
                SetGridObject() ' to set PerDayLimitForDaysPeriod value if is For Mail
            End If
            Dim rptDueDetail = New crSpareDueListReport
            'rptDueDetail = New crSpareDueListReport
            If chkSummary.Checked = True Then
                ReportName = "Required Spare Parts Summary"
                ReportNameForPDF = "Required Spare Parts Summary"
            Else
                ReportName = "Required Spare as Per Maintenance Due"
                ReportNameForPDF = "Required Spare as Per Maintenance Due"
            End If
            Dim x As String
            If mloglist.Count > 0 Then
                x = mloglist(0).LogDate.ToShortDateString
            Else
                x = txtFromDate.Text.Trim
            End If
            Dim LastFlownDate As String = ""
            Dim LastMaintenanceActivityDate As String = ""
            Dim mMaxLogNo As MaxLogNo = MaxLogNo.GetMaxLogNo(AsonDate, New Guid(MachineName), New Guid(AssemblyName))

            If mMaxLogNo.Count <> 0 Then
                LastFlownDate = mMaxLogNo(0).LogDate.ToString 'Last Flight Log Date
            Else
                LastFlownDate = CType(Session("AircraftAsOnDate"), String)
            End If
            ''Last Maintenance Activity
            If Not cmbAircraft.SelectedItem.ToString = "(All)" Then
                Dim mLastMachineMaintenanceActivity As LastMachineMaintenanceActivity = LastMachineMaintenanceActivity.GetLastMaintenanceActivity(AsonDate, New Guid(MachineName), New Guid(AssemblyName))
                If Not mLastMachineMaintenanceActivity.ID.Equals(Guid.Empty) Then
                    LastMaintenanceActivityDate = ", Last Maintenance Done on  " + "( " + mLastMachineMaintenanceActivity.Date.ToString + " )"
                    searchstr8 = mLastMachineMaintenanceActivity.Date.ToString
                End If
            End If

            If ((Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022 
                searchstr6 = "Flying Hours updated till " + "( " + LastFlownDate + " ) " + LastMaintenanceActivityDate + " & Work Order Number - _______________________"
            Else
                searchstr6 = LastFlownDate 'Mostly on Heligo Report
            End If

            If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "Indamer")) Then
                Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue))
                If cmbAircraft.SelectedIndex > 0 Then
                    If mMachineOperatorName.OperatorName <> "" Then OperatorName = mMachineOperatorName.OperatorName
                End If
            ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022 
                OperatorName = searchstr7
            End If
            Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
       mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
       mCompanyDetail.WebSite, ReportName, searchstr, searchstr1, Assembly1, AppSettings("ClientCode"), _
       "Aircraft is flown up to: " & New SmartDate(x).FormattedText, AppSettings("Product Version"), AppSettings("SINote"), searchstr6, OperatorName, _
       searchstr8, "", AppSettings("Logo"), AppSettings("FormNo"), mModuleList.Item("RequiredSpareasPerMaintenanceDue").FormRevisionNo, SearchStr13:=Aircraft, SearchStr14:=txtFromDate.Text, _
       SearchStr15:=IIf(chkSummary.Checked = True, "True", ""), SearchStr16:=Val(Trim(txtForecastingLimit.Text)).ToString)
            'Replace AppSettings("RevisionNo") with mModuleList.Item("RequiredSpareasPerMaintenanceDue").FormRevisionNo in Report Data  by Shital
            If ByMail = False Then
                If ReportMaintenanceDetails.Count = 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 719)
                    MarkLog(Util.Action.Print, "Due-PeriodWise", mEventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                End If
            End If
            If (ByMail = True And ReportMaintenanceDetails.Count <= 0) Then
                SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, ReportName, ReportNameForPDF, "There is no record for this search criteria.", "", _
                    Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"))
                Exit Sub
            End If

            mSpareListByMaintenanceActivity = SpareListByMaintenanceActivity.GetList(Today.Date.ToString)

            'Dim teamTotalScores = (From c In mSpareListByMaintenanceActivity
            '                      Group By ItemID = c.ItemID, StockBalanceQty = c.StockBalanceQty, LastPurchaseAmt = c.LastPurchaseAmt, MaintenanceID = c.MaintenanceID Into Group
            '                      Join bp In ReportMaintenanceDetails On bp.StatusMasterID Equals Group.FirstOrDefault().MaintenanceID
            '                      Select New With {.ItemID = ItemID, .StockBalanceQty = StockBalanceQty, .MaintenanceID = MaintenanceID, .LastPurchaseAmt = LastPurchaseAmt, .SpareQty = Group.Sum(Function(j) j.SpareQty), .ShortFall = Group.Sum(Function(j) j.SpareQty) - StockBalanceQty, .ShortFallAmount = (Group.Sum(Function(j) j.SpareQty) - StockBalanceQty) * LastPurchaseAmt, .ReceiptItemCollection = Group}).ToList


            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, ReportMaintenanceDetails)

            If Not cmbAircraft.SelectedItem.ToString = "(All)" Then
            Else
                If MachineName = "{00000000-0000-0000-0000-000000000000}" Then
                    If ByMail = True Then
                        SetGridObject()
                    End If
                End If
            End If

            da.Fill(ds, Report)
            da.Fill(ds, ReportStatusList)
            da.Fill(ds, mSpareListByMaintenanceActivity)
            da.Fill(ds, rptSnagCorrectiveActionListForDue)
            da.Fill(ds, mrptImage)
            rptDueDetail.SetDataSource(ds)

            With rptDueDetail
                If chkSummary.Checked = True Then
                    .ReportHeaderSection2.SectionFormat.EnableSuppress = True
                    .ReportHeaderSection1.SectionFormat.EnableSuppress = True
                    .Section27.SectionFormat.EnableSuppress = True
                    .Section29.SectionFormat.EnableSuppress = True
                    .Section6.SectionFormat.EnableSuppress = True
                    .PageHeaderSection1.SectionFormat.EnableSuppress = True
                    .Section3.SectionFormat.EnableSuppress = True
                    .Section7.SectionFormat.EnableSuppress = True
                    .DetailSection1.SectionFormat.EnableSuppress = True
                    .Section5.SectionFormat.EnableSuppress = True
                End If
            End With

            Session("CrystalReport") = rptDueDetail

            If ByMail Then
                SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, ReportName, ReportNameForPDF, lblDateRangeFrom.Text + ", " + lblAircraft1.Text + ", " + lblAssembly1.Text, _
                                          "", Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), ReportGeneratedBy:=Session("ReportGenratedBy"))
            ElseIf ByExcel Then
                SetExcel(ReportMaintenanceDetails, Report, ReportName)
                MarkLog(Util.Action.Print, "Due-PeriodWise", "Export To excel " + mEventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)  'Added by Shital on 18-Jan-2021
            Else
                Dim Str As String
                Str = "openTranDetail();"
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "openTranDetail", Str, True)
            End If
        Catch ex As Exception
            Dim Day, Month, Year As String
            Day = Format(Today.Date.Day, "0#")
            Month = Format(Today.Date.Month, "0#")
            Year = Format(Today.Date.Year, "0#")
            Dim todaydate As String = Day & Month & Year
            Dim Path As String = AppSettings("DOCPath") & todaydate
            FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (SetReport Sub Method): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        End Try
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
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
        End If
    End Sub
    Private Sub ControltovisibilityForDetails()
        upnlDetails.Update()
    End Sub
    Private Sub SetTitle()

    End Sub
    Private Sub ControlvisibilityForAvgPeriod()
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
        'If mCompanyDetail.ShortName = "TS" Then
        '    cmbFormat.Items.Add(New ListItem("Format 3(Enlarge Copy with Limited Columns)", "2"))
        'End If

        mFAScsReportList = FAScsReportList.GetFAScsReportList()
        Session("mFAScsReportList") = mFAScsReportList

        DataBind()
        'If mCompanyDetail.ShortName = "TS" Then
        '    cmbFormat.SelectedIndex = 2
        'End If
    End Sub
    Public Sub SetTypeCombo()
        If mServiceTypeList Is Nothing Then
            mServiceTypeList = PartMonitorServiceTypeList.GetPartMonitorServiceTypeList(, True)
        End If
        ListServiceType.DataSource = mServiceTypeList
        Session("mServiceTypeList") = mServiceTypeList

        If mInspectionTypeList Is Nothing Then
            mInspectionTypeList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList()
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
    Public Sub SetComboOfMachine(ByVal AsonDate As String)
        mMachineNameValueList = MachineNameValueList.GetMachineList(AsonDate, , , , , , , True, "(All)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataBind()
    End Sub
    Public Sub CustomValidate1(ByVal s As Object, ByVal e As ServerValidateEventArgs)
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
        Dim SearchCriteriaValues As New Hashtable
        SearchCriteriaValues.Add("AsonDate", txtFromDate.Text)
        SearchCriteriaValues.Add("MachineID", MachineName)
        SearchCriteriaValues.Add("DueLimitObj", mDueLimits)
        SearchCriteriaValues.Add("IsrbdPercentChecked", rbdPercent.Checked)
        SearchCriteriaValues.Add("Percentage", Val(txtPercentage.Text))
        SearchCriteriaValues.Add("AssemblyID", AssemblyName)
        SearchCriteriaValues.Add("AverageMonths", AvgMnths)
        SearchCriteriaValues.Add("IsSpecifyValuesChecked", rbdSpecifyValues.Checked)
        SearchCriteriaValues.Add("PerDayLimitsObj", mPerDayLimits)
        SearchCriteriaValues.Add("IsServiceRequired", IsSerSelect)
        SearchCriteriaValues.Add("IsModRequired", IsModSelect)
        SearchCriteriaValues.Add("IsInspRequired", IsInsSelect)
        SearchCriteriaValues.Add("ForDueStatus", Val(txtForecastingLimit.Text))
        SearchCriteriaValues.Add("SelectedAircraftText", cmbAircraft.SelectedItem.ToString)
        SearchCriteriaValues.Add("ServiceTypeID", ServiceTypeID)
        SearchCriteriaValues.Add("InspectionTypeID", InspectionTypeID)
        SearchCriteriaValues.Add("ModificationTypeID", ModificationTypeID)
        SearchCriteriaValues.Add("Aircraft", IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, ""))
        SearchCriteriaValues.Add("IschkwithWONoChecked", True)

        Session("SearchCriteriaValues") = SearchCriteriaValues
    End Sub
    'End
#End Region

#Region " Eventes "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        addAttributes()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("Sender") = "" Then
            Session("MiddleFrame") = "wfRequiredSpareasPerMaintenanceDue_Ajax.aspx"
            ResetValues()
            lblAssembly.Enabled = False
            cmbAssembly.Enabled = False
            txtFromDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            AOnDate = Now.Date.ToString(AppSettings("DateFormat"))
            SetComboOfMachine(AOnDate)
            setFocus(cmbAircraft)
            DataFieldBind()
            SetTypeCombo()
            ControltovisibilityForDetails()
            ControlvisibilityForAvgPeriod()
            rbdAvrageMonths.Checked = True
            SetSession()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        If IsValid = True Then
            Display()
            SetValues()
            upnlDetails.Update()
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid = True Then
            mIsExcel = False
            SetReport(, mIsExcel)
        Else
            upnlValidations.Update()
        End If
        upnlDetails.Update()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mMachineList = Nothing
        mDueLimits = Nothing
        mAssemblyList = Nothing
        mServiceTypeList = Nothing
        mInspectionTypeList = Nothing
        mModificationTypeList = Nothing
        Session("MiddleFrame") = ""
        ClearAll()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub rbdPercent_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbdPercent.CheckedChanged
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
    Private Sub rbdDueLimits_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbdDueLimits.CheckedChanged
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
    Private Sub rbdAvrageMonths_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbdAvrageMonths.CheckedChanged
        lblAvgMnths.Visible = True
        txtAvgMnths.Visible = True
        lblMonths.Visible = True
        pnlAvragePeriod.Visible = False
        lblInfo.Visible = False
        upnlAvrgperiod.Update()
    End Sub
    Private Sub rbdSpecifyValues_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbdSpecifyValues.CheckedChanged
        lblAvgMnths.Visible = False
        txtAvgMnths.Visible = False
        lblMonths.Visible = False
        pnlAvragePeriod.Visible = True
        lblInfo.Visible = True
        upnlAvrgperiod.Update()
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        If cmbAircraft.SelectedIndex = 0 Then
            lblAssembly.Enabled = False
            cmbAssembly.Enabled = False
            AircraftIndex = cmbAircraft.SelectedIndex
            Session("AircraftIndex") = AircraftIndex
            cmbAssembly.SelectedIndex = 0
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "WONocheckboxvisibility", "ControlvisibilityForWONo('False')", True)
        Else
            lblAssembly.Enabled = True
            cmbAssembly.Enabled = True
            Dim mAssemblylist As AssemblyList
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, txtFromDate.Text.Trim.ToString, "(All)", True)
            Session("mAssemblyList") = mAssemblylist
            cmbAssembly.DataSource = mAssemblylist
            cmbAssembly.DataBind()
            AircraftIndex = cmbAircraft.SelectedIndex
            Session("AircraftIndex") = AircraftIndex
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "WONocheckboxvisibility", "ControlvisibilityForWONo('True')", True)
        End If
        If cmbAircraft.Enabled = True Then
            setFocus(cmbAircraft)
        End If
        DataFieldBind()
        ControlVisibility()
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
    Private Sub txtFromDate_TextChanged(sender As Object, e As System.EventArgs) Handles txtFromDate.TextChanged
        AOdate = txtFromDate.Text.Trim
        If AOnDate = AOdate Then
        Else
            Dim tmpdate As Date
            If Date.TryParse(txtFromDate.Text.Trim, tmpdate) Then
                SetComboOfMachine(AOdate)
                lblAssembly.Enabled = False
                cmbAssembly.Enabled = False
                mAssemblyList = Nothing
                Session("mAssemblyList") = mAssemblyList
                cmbAssembly.ClearSelection()
                cmbAssembly.DataSource = mAssemblyList
                cmbAssembly.Controls.Clear()
                cmbAssembly.DataBind()
                DataFieldBind()
                ControlVisibility()
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "WONocheckboxvisibility", "ControlvisibilityForWONo('False')", True)
            End If
        End If
    End Sub
    Private Sub hdnimgBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnSendMail.Click
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
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (hdnimgBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        End Try
        upnlDetails.Update()
    End Sub
    Protected Sub btnByMail_Click(sender As Object, e As EventArgs) Handles btnByMail.Click
        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        Session("UserEmailID") = mModuleList.Item("RequiredSpareasPerMaintenanceDue").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("RequiredSpareasPerMaintenanceDue").SendCCMailID
        '--------------------------
        upnlDetails.Update()
        Dim Str As String
        Str = "OpenByMaiWindow();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
    End Sub
    Private Sub btnByExcel_Click(sender As Object, e As System.EventArgs) Handles btnByExcel.Click
        If IsValid = True Then
            mIsExcel = True
            SetReport(, mIsExcel)
        End If
    End Sub
#End Region

End Class