
'CREATED By : Saylee
'Dated      : 27-Jan-2014


Public Class wfSummaryofDirectiveStatus_AJAX
    Inherits System.Web.UI.Page

#Region "Enumeration"
    Enum Open
        ModificationReport = 3
    End Enum

#End Region

#Region "Variable Declaration"

    Dim ReportMaintenanceDetails As New ReportMaintenanceDetailList
    Dim mReportMaintenanceDetail As New ReportMaintenanceDetail
    Dim mAssemblyList As AssemblyList
    Dim mModificationTypeList As ModelMonitorModTypeList
    Dim ReportStatusList As New rptStatusList
    Dim mMachineList As MachineList
    Dim mOpen As Open
    Dim SofAIndex As Integer
    Dim AircraftIndex As Integer
    Dim ReportLabel As String
    Dim Aircraft As String
    Dim Assembly1 As String
    Dim ReportType As String
    Dim AOdate As String
    Dim AOnDate As String
    Dim Report As Integer = 1
    Dim ShowCofA As Boolean = False
    Dim AsonDate As String = ""
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
    Private AssemblySerialNo As String
    Private PartNo As String
    Private CompSerialNo As String
    Private Position As String
    Private MonitorTypeCode As String
    Private MonitorType As String
    Private Note As String
    Private Description As String
    Private EstimatedDate, IssueDate As String
    Private Freq1 As String
    Private ElapsedTime As String
    Private RemainingTime As String
    Private DueAsof As String
    Private AssemblyModel As String
    Private AssemblyTypeID As Integer
    Private Number, SupersededByADNumber As String
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
    Private Code, SerialNoPostion As String
    Dim mSummaryofDirectiveStatusSearchingCriteria As String = String.Empty

    'Added By Abhishek On 23-OCT-2017
    Dim da As New CSLA.Data.ObjectAdapter
    Dim ds As New dsReportMaintenanceDetail
    Dim mCompanyDetail As New CompanyDetail
    Dim mCount As Integer
    Dim OperatorName As String = ""

#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineList = CType(Session("mMachineList"), MachineList)
        mAssemblyList = CType(Session("mAssemblyList"), AssemblyList)
        mModificationTypeList = CType(Session("mModificationTypeList"), ModelMonitorModTypeList)
        mOpen = CType(Session("mOpen"), Open)
        AOnDate = Session("AOnDate")
        AircraftIndex = Session("AircraftIndex")
        SofAIndex = Session("SofAIndex")
        Report = Session("Report")
        ShowCofA = Session("ShowCofA")
        mModTypeList = Session("mModTypeList")
    End Sub
    Private Sub SetSession()
        Session("mMachineList") = mMachineList
        Session("mAssemblyList") = mAssemblyList
        Session("mModificationTypeList") = mModificationTypeList
        Session("mOpen") = mOpen
        Session("AOnDate") = AOnDate
        Session("AircraftIndex") = AircraftIndex
        Session("SofAIndex") = SofAIndex
        Session("Report") = Report
        Session("ShowCofA") = ShowCofA
        Session("mModTypeList") = mModTypeList
    End Sub
    Private Sub ClearAll()
        mOpen = Session("mOpen")
        If Session("MiddleFrame") <> "wfSummaryofDirectiveStatus_AJAX.aspx?" Then
            Session.Remove("mMachineList")
            Session.Remove("mAssemblyList")
            Session.Remove("mServiceTypeList")
            Session.Remove("mInspectionTypeList")
            Session.Remove("mModificationTypeList")
            Session.Remove("AOnDate")
            Session.Remove("Check")
            Session.Remove("AircraftIndex")
            Session.Remove("SofAIndex")
            Session.Remove("Report")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub
    Private Sub Display()
        lblAircraft1.Visible = True
        lblReportType.Visible = True
        lblDateRange.Visible = True
        lblAssembly1.Visible = True
    End Sub
    Private Sub SetValues()
        If cmbAircraft.SelectedItem.Text = "<SELECT>" Then
            Aircraft = ""
        Else
            AssemblyType = mAssemblyList(cmbAssembly.SelectedIndex).AssemblyType
            AssemblyName = cmbAssembly.SelectedValue.ToString
            Assembly1 = cmbAssembly.SelectedItem.Text
            lblAssembly1.Text = "Assembly Name : " & Assembly1
            MachineName = cmbAircraft.SelectedValue.ToString
            Aircraft = cmbAircraft.SelectedItem.Text
            lblAircraft1.Text = "Aircraft Name : " & Aircraft
        End If

        If Not IsDate(txtFromDate.Text) Then              'AsOnDate
            AsonDate = ""
        Else
            AsonDate = txtFromDate.Text.ToString
            lblDateRange.Text = "As On Date : " & New SmartDate(txtFromDate.Text).FormattedText
        End If

        If cmbType.SelectedItem.Text = "<SELECT>" Then     'Directive
            Directive = ""
            lblType1.Text = ""
            lblType1.Visible = False
        Else
            DirectiveName = mModTypeList(cmbType.SelectedIndex).Name
            Directive = cmbType.SelectedItem.Text
            lblType1.Text = "Directive Name : " & Directive
        End If
        mSummaryofDirectiveStatusSearchingCriteria = lblDateRange.Text.Trim + ", " + lblAircraft1.Text.Trim + ", " + lblAssembly1.Text.Trim + ", " + lblType1.Text
    End Sub
    Private Sub ResetValues()
        cnbAdType.SelectedIndex = 0
        AssemblyName = "{00000000-0000-0000-0000-000000000000}"

        ShowCofA = False
        Session("ShowCofA") = ShowCofA

        AssemblyType = ""
        MachineName = "{00000000-0000-0000-0000-000000000000}"

        txtFromDate.Text = AsonDate
        If AsonDate <> "" Then
            txtFromDate.Text = AsonDate
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


        mMachineList = MachineList.GetMachineListMonitoringStatus(New SmartDate(AsonDate).Text, MachineName, , , , , , , , , , , True, , AssemblyName, SkipIsForInventoryAircarft:=True)
        Dim LHLabel2 As String = ""    'Hours
        Dim LHData2 As String = ""     'Hours
        Dim LHLabel3 As String = ""    'Cycles
        Dim LHData3 As String = ""     'Cycles 
        Dim LHLabel4 As String = ""    'Landings 
        Dim LHData4 As String = ""     'Landings
        Dim LHLabel5 As String = ""    'RINS
        Dim LHData5 As String = ""     'RINS
        Dim LHData6 As String = ""
        Dim LHData7 As String = ""
        Dim LHData8 As String = ""
        Dim LHData9 As String = ""
        Dim LHData10 As String = ""
        Dim LHData11 As String = ""
        Dim LHData12 As String = ""
        Dim LHData13 As String = ""
        Dim LHData14 As String = ""
        Dim RHCaption As String = ""
        For Each ObjMachine In mMachineList
            For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                Periodcount = ObjAssemblyStatus.AssemblyStatusPeriodList.Count()
                LHLabel2 = ""
                LHData2 = ""
                LHLabel3 = ""
                LHData3 = ""
                LHLabel4 = ""
                LHData4 = ""
                LHLabel5 = ""
                LHData5 = ""
                For Count = 0 To Periodcount - 1
                    If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID <> 2 Then
                        If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName = "Hours" Then
                            LHLabel2 = CType(IIf(LHLabel2 = "", LHLabel2, LHLabel2 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                            LHData2 = CType(IIf(LHData2 = "", LHData2, LHData2 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyCurrentValue
                        End If
                        If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName = "Cycles" Then
                            LHLabel3 = CType(IIf(LHLabel3 = "", LHLabel3, LHLabel3 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                            LHData3 = CType(IIf(LHData3 = "", LHData3, LHData3 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyCurrentValue
                        End If
                        If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName = "Landings" Then
                            LHLabel4 = CType(IIf(LHLabel4 = "", LHLabel4, LHLabel4 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                            LHData4 = CType(IIf(LHData4 = "", LHData4, LHData4 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyCurrentValue
                        End If
                        If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName = "RINS" Then
                            LHLabel5 = CType(IIf(LHLabel5 = "", LHLabel5, LHLabel5 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                            LHData5 = CType(IIf(LHData5 = "", LHData5, LHData5 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyCurrentValue
                        End If
                    End If
                    '---------------------------------------------------------------------------------------------------------
                    If cmbAssembly.SelectedIndex = 0 Then
                        If ObjAssemblyStatus.AssemblyTypeID = 1 Then
                            If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID <> 2 Then
                                If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName = "Hours" Then
                                    LHData6 = CType(IIf(LHData6 = "", LHData6, LHData6 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                                    LHData7 = CType(IIf(LHData7 = "", LHData7, LHData7 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyCurrentValue
                                End If
                                If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName = "Cycles" Then
                                    LHData8 = CType(IIf(LHData8 = "", LHData8, LHData8 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                                    LHData9 = CType(IIf(LHData9 = "", LHData9, LHData9 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyCurrentValue
                                End If
                                If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName = "Landings" Then
                                    LHData10 = CType(IIf(LHData10 = "", LHData10, LHData10 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                                    LHData11 = CType(IIf(LHData11 = "", LHData11, LHData11 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyCurrentValue
                                End If
                                If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName = "RINS" Then
                                    LHData12 = CType(IIf(LHData12 = "", LHData12, LHData12 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                                    LHData13 = CType(IIf(LHData13 = "", LHData13, LHData13 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyCurrentValue
                                End If
                            End If
                            LHData14 = ObjAssemblyStatus.SerialNo
                            RHCaption = ObjAssemblyStatus.Model
                        End If

                    Else
                        If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID <> 2 Then
                            If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName = "Hours" Then
                                LHData6 = CType(IIf(LHData6 = "", LHData6, LHData6 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                                LHData7 = CType(IIf(LHData7 = "", LHData7, LHData7 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyCurrentValue
                            End If
                            If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName = "Cycles" Then
                                LHData8 = CType(IIf(LHData8 = "", LHData8, LHData8 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                                LHData9 = CType(IIf(LHData9 = "", LHData9, LHData9 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyCurrentValue
                            End If
                            If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName = "Landings" Then
                                LHData10 = CType(IIf(LHData10 = "", LHData10, LHData10 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                                LHData11 = CType(IIf(LHData11 = "", LHData11, LHData11 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyCurrentValue
                            End If
                            If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName = "RINS" Then
                                LHData12 = CType(IIf(LHData12 = "", LHData12, LHData12 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                                LHData13 = CType(IIf(LHData13 = "", LHData13, LHData13 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyCurrentValue
                            End If

                            If ObjAssemblyStatus.Position = "" Then
                                LHData14 = ObjAssemblyStatus.SerialNo
                            Else
                                LHData14 = ObjAssemblyStatus.SerialNo + "(" + ObjAssemblyStatus.Position + ")"
                            End If
                            RHCaption = ObjAssemblyStatus.Model
                        End If
                    End If
                    '---------------------------------------------------------------------------------------------------------

                Next
                If ObjAssemblyStatus.Position = "" Then
                    SerialNoPostion = ObjAssemblyStatus.SerialNo
                Else
                    SerialNoPostion = ObjAssemblyStatus.SerialNo + "(" + ObjAssemblyStatus.Position + ")"
                End If
                AssemblyID = ObjAssemblyStatus.AssemblyID
                ReportStatusList.Add(New rptStatus(AssemblyID.ToString, ObjAssemblyStatus.AssemblyTypeID, , "Reg No.", ObjMachine.RegNo, ObjAssemblyStatus.AssemblyType + " " + "Model", ObjAssemblyStatus.Model, _
                   "Serial No.", SerialNoPostion, "Due As of " & ObjAssemblyStatus.AssemblyTypeID, , , LHData6, LHData7, LHData8, LHData9, LHData10, LHData11, LHData12, LHData13, LHData14, RHCaption, LHLabel2, LHData2, LHLabel3, LHData3, LHLabel4, LHData4, LHLabel5, LHData5, DirectiveName))
            Next
        Next

        mModificationTypeList = ModelMonitorModTypeList.GetModelMonitorModTypeList()

        For i As Integer = 0 To mModificationTypeList.Count - 1
            If mModificationTypeList(i, "").ModTypeID = cmbType.SelectedValue Then
                mMachineList = MachineList.GetMachineListMonitoringStatus(AsonDate, MachineName, , , , , , , , , , True, True, , AssemblyName, , , , , , , , , , , ShowCofA, , , , True, , , , , , , False, , False, , True, , , , , mModificationTypeList(i, "").ID, , , True, SkipIsForInventoryAircarft:=True)
                For Each ObjMachine In mMachineList
                    For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                        For Each ObjAssemblyMonitorModStatus In ObjAssemblyStatus.AssemblyMonitorModStatusList
                            ATAChapter = ObjAssemblyMonitorModStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorModStatus.ATANomenclature
                            Description = ObjAssemblyMonitorModStatus.Description
                            Position = ObjAssemblyStatus.Position
                            MonitorTypeCode = ObjAssemblyMonitorModStatus.Code
                            If ObjAssemblyMonitorModStatus.Code = "Sup" And ObjAssemblyMonitorModStatus.MonitorType = "No Frequency" Then
                                MonitorType = "Superseded"
                            ElseIf ObjAssemblyMonitorModStatus.Code = "Ter" And ObjAssemblyMonitorModStatus.MonitorType = "No Frequency" Then
                                MonitorType = "Terminated"
                            ElseIf ObjAssemblyMonitorModStatus.MonitorType = "No Frequency" Then
                                MonitorType = "N/A"
                            Else
                                If (ObjAssemblyMonitorModStatus.IsApplicable = False And ObjAssemblyMonitorModStatus.DoneOn = "" And ObjAssemblyMonitorModStatus.MonitorType = "One Time") Then
                                    MonitorType = "One Time-N/A"
                                ElseIf (ObjAssemblyMonitorModStatus.DoneOn <> "" And ObjAssemblyMonitorModStatus.MonitorType = "One Time") Then
                                    MonitorType = "One Time-Incorporated"
                                ElseIf (ObjAssemblyMonitorModStatus.DoneOn = "" And ObjAssemblyMonitorModStatus.MonitorType = "One Time") Then
                                    MonitorType = "One Time-Open"
                                ElseIf (ObjAssemblyMonitorModStatus.IsApplicable = False And ObjAssemblyMonitorModStatus.DoneOn = "" And ObjAssemblyMonitorModStatus.MonitorType = "Reccurring") Then
                                    MonitorType = "Reccurring-N/A"
                                Else
                                    MonitorType = ObjAssemblyMonitorModStatus.MonitorType
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

                            Code = ObjAssemblyMonitorModStatus.ModelMonitorModCode

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
                                        DoneOnValue = DoneOnValue + ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted & vbCrLf

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
								Else        'Report = 1 
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
                                            DoneOnValue = ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted
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
                                            DoneOnValue = DoneOnValue & vbCrLf & ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted '& vbCrLf
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
                                            DoneOnValue = ObjAssemblyMonitorModStatusPeriod.DoneOnValue
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
                                                DoneOnValue = DoneOnValue & vbCrLf & ObjAssemblyMonitorModStatusPeriod.DoneOnValue
                                            End If
                                        End If
                                    End If
                                End If
                            Next
                            AssemblyID = ObjAssemblyStatus.AssemblyID
                            Note = ObjAssemblyMonitorModStatus.Notes
                            Number = ObjAssemblyMonitorModStatus.Number
                            SupersededByADNumber = ObjAssemblyMonitorModStatus.SupersededByADNumber
                            IssueDate = ObjAssemblyMonitorModStatus.IssueDate
                            Reference = ObjAssemblyMonitorModStatus.Reference
                            DoneOnDate = ObjAssemblyMonitorModStatus.DoneOn
                            DoneWONo = ObjAssemblyMonitorModStatus.DoneWONo
                            Remark = ObjAssemblyMonitorModStatus.DoneRemark
                            Applicability = ObjAssemblyMonitorModStatus.Applicability
                            ComplianceRequirement = ObjAssemblyMonitorModStatus.ComplianceRequirement
                            ModelMonitorModCode = ObjAssemblyMonitorModStatus.ModelMonitorModCode

                            Dim mReportMaintenanceDetail As ReportMaintenanceDetail

                            mReportMaintenanceDetail = New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, , , Position, MonitorType, MonitorTypeCode, Note, Remark, Description, _
                                                              , , , , Freq1, Freq1, Freq1, ElapsedTime, ElapsedTime, ElapsedTime, RemainingTime, RemainingTime, RemainingTime, _
                                                              DueAsof, DueAsof, DueAsof, AssemblyModel, , , , , , , , AssemblyTypeID, , , , , , , , , , , , , , , Number, Reference, DoneOnValue, DoneOnDate, DoneWONo, Applicability, ComplianceRequirement, , , , , , , , , , , Code, , , SupersededByADNumber, IssueDate)

                            mReportMaintenanceDetail.ModelMonitorModCode = ModelMonitorModCode

                            'code Added for Open ANd closed Modification***************************************************
                            If cnbAdType.SelectedValue = 0 Then  '' All
                                ReportMaintenanceDetails.Add(mReportMaintenanceDetail)
                            ElseIf cnbAdType.SelectedValue = 1 Then '' Opened
                                If mReportMaintenanceDetail.DueAsof <> "----" Then
                                    ReportMaintenanceDetails.Add(mReportMaintenanceDetail)
                                End If
                            ElseIf cnbAdType.SelectedValue = 2 Then  '' closed
                                If mReportMaintenanceDetail.DueAsof = "----" Then
                                    ReportMaintenanceDetails.Add(mReportMaintenanceDetail)
                                End If

                            End If
                            '**********************************************************************************************
                        Next

                        For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
                            For Each ObjCompMonitorModStatus In ObjCompStatus.CompMonitorModStatusList
                                ATAChapter = ObjCompMonitorModStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorModStatus.ATANomenclature
                                Description = ObjCompMonitorModStatus.Description
                                PartNo = ObjCompStatus.PartName
                                CompSerialNo = ObjCompStatus.CompSerialNo
                                Position = ObjCompStatus.Position
                                MonitorTypeCode = ObjCompMonitorModStatus.Code
                                If ObjCompMonitorModStatus.Code = "Sup" And ObjCompMonitorModStatus.MonitorType = "No Frequency" Then
                                    MonitorType = "Superseded"
                                ElseIf ObjCompMonitorModStatus.Code = "Ter" And ObjCompMonitorModStatus.MonitorType = "No Frequency" Then
                                    MonitorType = "Terminated"
                                ElseIf ObjCompMonitorModStatus.MonitorType = "No Frequency" Then
                                    MonitorType = "N/A"
                                Else
                                    If (ObjCompMonitorModStatus.IsApplicable = False And ObjCompMonitorModStatus.DoneOn = "" And ObjCompMonitorModStatus.MonitorType = "One Time") Then
                                        MonitorType = "One Time-N/A"
                                    ElseIf (ObjCompMonitorModStatus.DoneOn <> "" And ObjCompMonitorModStatus.MonitorType = "One Time") Then
                                        MonitorType = "One Time-Incorporated"
                                    ElseIf (ObjCompMonitorModStatus.DoneOn = "" And ObjCompMonitorModStatus.MonitorType = "One Time") Then
                                        MonitorType = "One Time-Open"
                                    ElseIf (ObjCompMonitorModStatus.IsApplicable = False And ObjCompMonitorModStatus.DoneOn = "" And ObjCompMonitorModStatus.MonitorType = "Reccurring") Then
                                        MonitorType = "Reccurring-N/A"
                                    Else
                                        MonitorType = ObjCompMonitorModStatus.MonitorType
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
                                Code = ObjCompMonitorModStatus.PartMonitorModCode

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
                                            DoneOnValue = DoneOnValue + ObjCompMonitorModStatusPeriod.DoneOnValueFormatted & vbCrLf
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
                                                DoneOnValue = DoneOnValue + ObjCompMonitorModStatusPeriod.DoneOnValueFormatted & vbCrLf
                                                'r
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
                                                DoneOnValue = DoneOnValue + ObjCompMonitorModStatusPeriod.DoneOnValueFormatted & vbCrLf
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
                                Number = ObjCompMonitorModStatus.Number
                                Reference = ObjCompMonitorModStatus.Reference
                                DoneOnDate = ObjCompMonitorModStatus.DoneOnFormatted
                                DoneWONo = ObjCompMonitorModStatus.DoneOnWONo
                                Remark = ObjCompMonitorModStatus.DoneRemark
                                Applicability = ObjCompMonitorModStatus.Applicability
                                ComplianceRequirement = ObjCompMonitorModStatus.ComplianceRequirement


                                Dim mReportMaintenanceDetail As ReportMaintenanceDetail

                                mReportMaintenanceDetail = New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, , , Position, MonitorType, MonitorTypeCode, Note, Remark, Description, _
                                                                  , , , , Freq1, Freq1, Freq1, ElapsedTime, ElapsedTime, ElapsedTime, RemainingTime, RemainingTime, RemainingTime, _
                                                                  DueAsof, DueAsof, DueAsof, AssemblyModel, , , , , , , , AssemblyTypeID, , , , , , , , , , , , , , , Number, Reference, DoneOnValue, DoneOnDate, DoneWONo, Applicability, ComplianceRequirement, , , , , , , , , , , Code)

                                mReportMaintenanceDetail.ModelMonitorModCode = ModelMonitorModCode
                                'code Added for Open ANd closed Modification***************************************************
                                If cnbAdType.SelectedValue = 0 Then
                                    ReportMaintenanceDetails.Add(mReportMaintenanceDetail)
                                ElseIf cnbAdType.SelectedValue = 1 Then
                                    If mReportMaintenanceDetail.DueAsof <> "----" Then
                                        ReportMaintenanceDetails.Add(mReportMaintenanceDetail)
                                    End If
                                ElseIf cnbAdType.SelectedValue = 2 Then
                                    If mReportMaintenanceDetail.DueAsof = "----" Then
                                        ReportMaintenanceDetails.Add(mReportMaintenanceDetail)
                                    End If
                                End If
                                '***********************************************************************************************
                            Next
                        Next
                    Next
                Next
            End If
        Next

        Return ReportMaintenanceDetails
    End Function

    Private Sub SetReport()
        ReportMaintenanceDetails = New ReportMaintenanceDetailList
        ReportStatusList = New rptStatusList
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsReportMaintenanceDetail
        Dim RptDirectiveStatusList As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim mCount As Integer
        Dim OperatorName As String = ""

        SetValues()
        ReportDetail()
        If optSummary.Checked = True Then
            If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "KamAir" Then
                RptDirectiveStatusList = New crSummaryofDirectiveStatusForKamAir
            Else
                RptDirectiveStatusList = New crSummaryofDirectiveStatus
            End If

            ReportLabel = "Summary of " + DirectiveName
        Else
            'If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "KamAir" Then
            'RptDirectiveStatusList = New crDetailedofDirectiveStatusForKamAir
            'Else
            'RptDirectiveStatusList = New crDetailedofDirectiveStatus
            RptDirectiveStatusList = New crDetailedofDirectiveStatusForKamAir
            'End If

            ReportLabel = "Detail of " + DirectiveName
        End If
        mCount = ReportMaintenanceDetails.Count

        'Added by vikrant on 11-aug-2011
        If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "Indamer")) Then
            Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue))
            If mMachineOperatorName.OperatorName <> "" Then OperatorName = mMachineOperatorName.OperatorName
        End If


        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
mCompanyDetail.WebSite, ReportLabel, New SmartDate(txtFromDate.Text.ToString).FormattedText, DirectiveName, "", mCount.ToString, txtBottomLine.Text, AppSettings("Product Version"), AppSettings("SINote"), "", OperatorName, "", "", AppSettings("Logo"))

        SetSession()
        If ReportMaintenanceDetails.Count = 0 Then
            ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
            ''msg1.ReplacePage = "wfSummaryofDirectiveStatus.aspx?Open=" & mOpen
            ''msg1.Show()
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")

            Exit Sub
        Else

            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1152)
        End If
        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds) 'Added by Shweta on 15-Feb-2012
        da.Fill(ds, ReportMaintenanceDetails)
        da.Fill(ds, Report)
        da.Fill(ds, mrptImage)       'Added by Shweta on 15-Feb-2012
        da.Fill(ds, ReportStatusList)

        RptDirectiveStatusList.SetDataSource(ds)
        Session("CrystalReport") = RptDirectiveStatusList
        Dim Str As String
        Str = "openTranDetail();"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        MarkLog(Util.Action.Print, "SummaryofDirectiveStatus", mSummaryofDirectiveStatusSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
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
                    SetSession()
                    'Response.Redirect("wfSummaryofDirectiveStatus.aspx?Open=" & mOpen)
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
            'Response.Redirect("wfSummaryofDirectiveStatus.aspx?Open=" & mOpen)
        End If
    End Sub
#End Region

#Region " Data Binding "
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)

        If custValidator.ControlToValidate = "cmbAircraft" Then                      'Aircraft
            If cmbAircraft.SelectedIndex = 0 Then
                custValidator.ErrorMessage = "Please select the Aircraft and Assembly"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
        If custValidator.ControlToValidate = "cmbType" Then                          'Aircraft
            If cmbType.SelectedIndex = 0 Then
                custValidator.ErrorMessage = "Please select the Directive"
                e.IsValid = False
            Else
                e.IsValid = True
            End If
        End If
    End Sub
    Public Sub SetComboOfMachine(ByVal AOnDate As String)
        mMachineList = MachineList.GetMachineListMonitoringStatus(AOnDate, , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , True, "<SELECT>", SkipIsForInventoryAircarft:=True)
        cmbAircraft.DataSource = mMachineList
        Session("mMachineList") = mMachineList
        cmbAircraft.DataBind()
    End Sub
    Private Sub DataFieldBind()
        mModTypeList = ModTypeList.GetModelTypeList(True)
        cmbType.DataSource = mModTypeList
        Session("mModTypeList") = mModTypeList
        DataBind()
    End Sub

#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("Sender") = "" Then
            mOpen = Request.QueryString("Open")
            Session("mOpen") = mOpen
            Session("MiddleFrame") = "wfSummaryofDirectiveStatus_AJAX.aspx?"
            ResetValues()
            lblAssembly.Enabled = False
            cmbAssembly.Enabled = False
            txtFromDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            AOnDate = Now.Date.ToString(AppSettings("DateFormat"))
            SetComboOfMachine(AOnDate)
            DataFieldBind()

            Report = 1
            Session("Report") = Report
        End If
        SetSession()
        cmbType.DataBind()
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
        upnlCriteria.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If Not IsValid Then upnlValidation.Update() : Exit Sub
            SetReport()
        End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mMachineList = Nothing
        mAssemblyList = Nothing
        mModificationTypeList = Nothing
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
   Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        If cmbAircraft.SelectedIndex = 0 Then
            lblAssembly.Enabled = False
            cmbAssembly.Enabled = False
            AircraftIndex = cmbAircraft.SelectedIndex
            Session("AircraftIndex") = AircraftIndex
            cmbAssembly.DataSource = Nothing
            cmbAssembly.DataBind()
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
        upnlAssembly.Update()
        If cmbAircraft.Enabled = True Then
            setFocus(cmbAircraft)
        End If
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub
#End Region
    'Added By Abhishek On 23-OCT-2017
    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        If IsValid Then
            SetValues()
            ReportDetail()


            'Added by vikrant on 11-aug-2011
            If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "Indamer")) Then
                Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue))
                If mMachineOperatorName.OperatorName <> "" Then OperatorName = mMachineOperatorName.OperatorName
            End If


            Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
    mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
    mCompanyDetail.WebSite, ReportLabel, New SmartDate(txtFromDate.Text.ToString).FormattedText, DirectiveName, Aircraft, mCount.ToString, txtBottomLine.Text, AppSettings("Product Version"), AppSettings("SINote"), Assembly1, OperatorName, "", "", AppSettings("Logo"))
            da.Fill(ds, "ReportData", Report)
            da.Fill(ds, "rptStatusList", ReportStatusList)
            da.Fill(ds, "ExcelReportMaintenanceDetailList", ReportMaintenanceDetails)
            mCount = ReportMaintenanceDetails.Count
            SetSession()
            If ReportMaintenanceDetails.Count = 0 Then
                ''Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
                ''msg1.ReplacePage = "wfSummaryofDirectiveStatus.aspx?Open=" & mOpen
                ''msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else

                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1152)
            End If

            If optSummary.Checked = True Then
                If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "KamAir" Then

                    Dim columnToRemove1 As String() = {"ID", "RemainingTimeAllExcel", "ReferenceForExcel", "Applicability", "MaintenanceInformationExcel", "EffectiveFromAll", "FrequencyExcel", "SinceNewAllExcel", "EffectiveFromAllExcel", "ElaspedAllExcel", "DoneAtAllExcel", "ExtensionAllExcel", "ElapsedAll", "SupersededByADNumber", "ExtensionAll", "DueAsOfAll", "DueAsOfAllExcel", "AssDueAsOfAllExcel", "AssDueAsOfAll", "RemainingTimeAll", "Frequency", "SinceNewAll", "MaintenanceOn", "DoneAtAll", "MaintenanceInfo", "MaintenanceInformation", "ATAChapter", "ModelID", "IsMaster", "DocumentTypeForID", "MachineID", "AssemblyStatusID", "CompStatusID", "StatusID", "EstDate", "AssemblySerialNo", "CompSerialNo", "AssemblyType", "Code", "Name", "Description", "Model", "PartNo", "SerialNo", "Position", "MonitorTypeCode", "Freq1", "Freq2", "Freq3", "ElapsedTime", "ElapsedTime1", "ElapsedTime2", "RemainingTime", "RemainingTime1", "RemainingTime2", "DueAsof", "DueAsof1", "DueAsof2", "Remark", "Note", "EstimatedDate", "LogBook", "ComponentInfo", "RegNo", "SinceNew", "SinceNew1", "SinceNew2", "DoneAt", "DoneAt1", "DoneAt2", "AssemblyModel", "MinimumRemainingValue", "AssemblyTypeID", "MaintenanceEvent", "ATACode", "InstalledAt", "InstalledAt1", "InstalledAt2", "TSO", "TSO1", "TSO2", "RemoveAt", "RemoveAt1", "RemoveAt2", "InstalledAtDate", "RemoveAtDate", "TSN", "Reference", "DoneOnValue", "DoneOnDate", "DoneWONo", "DetailID", "AssemblyDueAsof", "AssemblyDueAsof1", "AssemblyDueAsof2", "Extension", "Extension1", "Extension2", "ExtensionDate", "ApprovalRemark", "RequiredManHours", "Customer", "IssueDate", "IsApplicable", "MaintenanceTypeID", "MaintenanceTypeName", "IsLater", "DueStatus", "TimeSinceNew", "ModelMonitorModCode", "StatusTypeName", "WONumber", "StatusMasterID", "DoneONValueForAssembly", "ModelEstimatedManHours", "SourceDoc", "DiffCompInstDoneOnValue", "RecordID", "Zone", "Area", "TypeID", "MaintenanceInfoExcel", "ElapsedAllExcel", "MaintenanceOnExcel", "EROQtyNosForMaterialMgmtReport", "POQtyNosForMaterialMgmtReport", "PONosForMaterialMgmtReport", "POQtyForMaterialMgmtReport", "ERONosForMaterialMgmtReport", "EROQtyForMaterialMgmtReport", "UnserviceableStockQty", "ServiceableStockQty", "BinCardTotalQty", "Area", "Zone", "ThresholdAccordingToTypeIDForExcel", "FrequencyAccordingToTypeIDForExcel", "DueAsOfAssemblyOrCompForExcel", "DueAsOfAirframeForExcel", "RemainingForExcel"}
                    For i As Integer = 0 To columnToRemove1.Length - 1
                        If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains(columnToRemove1(i)) Then
                            ds.Tables("ExcelReportMaintenanceDetailList").Columns.Remove(columnToRemove1(i))
                        End If
                    Next

                    Dim columnToRemove2 As String() = {"CompanyName", "CurrencyName", "Website", "Email", "ID", "ToDate", "Address", "Tel1", "Tel2", "Fax", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ReportDate", "FromStore", "WorkShop", "WorkOrderText", "WorkOrderNo", "SearchStr4", "SearchStr5", "ReportName", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "ShortName", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25", "SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40", "SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47", "SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
                    For i As Integer = 0 To columnToRemove2.Length - 1
                        If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                            ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                        End If
                    Next
                    If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
                        ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "As On Date"
                    End If
                    If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                        ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "Directive"
                    End If
                    If ds.Tables("ReportData").Columns.Contains("SearchStr3") Then
                        ds.Tables("ReportData").Columns("SearchStr3").ColumnName = "Aircraft"
                    End If
                    If ds.Tables("ReportData").Columns.Contains("SearchStr6") Then
                        ds.Tables("ReportData").Columns("SearchStr6").ColumnName = "Assembly"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("ModificationNumber") Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns("ModificationNumber").ColumnName = "AD Number"
                    End If


                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("MaintenanceInformationForExcel") Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns("MaintenanceInformationForExcel").ColumnName = "Title"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("MonitorType") Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns("MonitorType").ColumnName = "AD_Status"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("ApplicabilityForExcel") Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns("ApplicabilityForExcel").ColumnName = "Applicability"
                    End If

                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("ComplianceRequirement") Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns("ComplianceRequirement").ColumnName = "Method Of Compliance"
                    End If

                    Dim dsNew As New DataSet
                    dsNew.Clear()

                    dsNew.Merge(ds.Tables("ReportData"))
                    dsNew.Merge(ds.Tables("ExcelReportMaintenanceDetailList"))



                    dsNew.Tables("ReportData").TableName = "Searching Criteria"
                    dsNew.Tables("ExcelReportMaintenanceDetailList").TableName = "Summary Of Directive Status"


					Session("ExcelFileName") = "Summary Of Directive Status"

					Session("dsNew") = dsNew


                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
                    'RptDirectiveStatusList = New crSummaryofDirectiveStatusForKamAir
                    'Added by Prashant on 19-Jan-2021
                    MarkLog(Util.Action.Print, "SummaryofDirectiveStatus", "Export To Excel " + mSummaryofDirectiveStatusSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                Else
                    Dim columnToRemove1 As String() = {"ID", "RemainingTimeAllExcel", "Applicability", "ReferenceForExcel", "MaintenanceInformationExcel", "EffectiveFromAll", "FrequencyExcel", "SinceNewAllExcel", "EffectiveFromAllExcel", "ElaspedAllExcel", "DoneAtAllExcel", "ExtensionAllExcel", "ElapsedAll", "SupersededByADNumber", "ExtensionAll", "DueAsOfAll", "DueAsOfAllExcel", "AssDueAsOfAllExcel", "AssDueAsOfAll", "RemainingTimeAll", "Frequency", "SinceNewAll", "MaintenanceOn", "DoneAtAll", "MaintenanceInfo", "MaintenanceInformation", "ATAChapter", "ModelID", "IsMaster", "DocumentTypeForID", "MachineID", "AssemblyStatusID", "CompStatusID", "StatusID", "EstDate", "AssemblySerialNo", "CompSerialNo", "AssemblyType", "Code", "Name", "Description", "Model", "PartNo", "SerialNo", "Position", "MonitorTypeCode", "Freq1", "Freq2", "Freq3", "ElapsedTime", "ElapsedTime1", "ElapsedTime2", "RemainingTime", "RemainingTime1", "RemainingTime2", "DueAsof", "DueAsof1", "DueAsof2", "Remark", "Note", "EstimatedDate", "LogBook", "ComponentInfo", "RegNo", "SinceNew", "SinceNew1", "SinceNew2", "DoneAt", "DoneAt1", "DoneAt2", "AssemblyModel", "MinimumRemainingValue", "AssemblyTypeID", "MaintenanceEvent", "ATACode", "InstalledAt", "InstalledAt1", "InstalledAt2", "TSO", "TSO1", "TSO2", "RemoveAt", "RemoveAt1", "RemoveAt2", "InstalledAtDate", "RemoveAtDate", "TSN", "Reference", "DoneOnValue", "DoneOnDate", "DoneWONo", "DetailID", "AssemblyDueAsof", "AssemblyDueAsof1", "AssemblyDueAsof2", "Extension", "Extension1", "Extension2", "ExtensionDate", "ApprovalRemark", "RequiredManHours", "Customer", "IssueDate", "IsApplicable", "MaintenanceTypeID", "MaintenanceTypeName", "IsLater", "DueStatus", "TimeSinceNew", "ModelMonitorModCode", "StatusTypeName", "WONumber", "StatusMasterID", "DoneONValueForAssembly", "ModelEstimatedManHours", "SourceDoc", "DiffCompInstDoneOnValue", "RecordID", "Zone", "Area", "TypeID", "MaintenanceInfoExcel", "ElapsedAllExcel", "MaintenanceOnExcel"}
                    For i As Integer = 0 To columnToRemove1.Length - 1
                        If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains(columnToRemove1(i)) Then
                            ds.Tables("ExcelReportMaintenanceDetailList").Columns.Remove(columnToRemove1(i))
                        End If
                    Next

                    Dim columnToRemove2 As String() = {"CompanyName", "CurrencyName", "Website", "Email", "ID", "ToDate", "Address", "Tel1", "Tel2", "Fax", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ReportDate", "FromStore", "WorkShop", "WorkOrderText", "WorkOrderNo", "SearchStr4", "SearchStr5", "ReportName", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "ShortName", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25", "SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40", "SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47", "SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
                    For i As Integer = 0 To columnToRemove2.Length - 1
                        If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                            ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                        End If
                    Next
                    If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
                        ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "As On Date"
                    End If
                    If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                        ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "Directive"
                    End If
                    If ds.Tables("ReportData").Columns.Contains("SearchStr3") Then
                        ds.Tables("ReportData").Columns("SearchStr3").ColumnName = "Aircraft"
                    End If
                    If ds.Tables("ReportData").Columns.Contains("SearchStr6") Then
                        ds.Tables("ReportData").Columns("SearchStr6").ColumnName = "Assembly"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("ModificationNumber") Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns("ModificationNumber").ColumnName = "AD Number"
                    End If


                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("MaintenanceInformationForExcel") Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns("MaintenanceInformationForExcel").ColumnName = "Title"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("MonitorType") Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns("MonitorType").ColumnName = "AD_Status"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("ApplicabilityForExcel") Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns("ApplicabilityForExcel").ColumnName = "Applicability"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("ComplianceRequirement") Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns("ComplianceRequirement").ColumnName = "Method Of Compliance"
                    End If

                    Dim dsNew As New DataSet
                    dsNew.Clear()

                    dsNew.Merge(ds.Tables("ReportData"))
                    dsNew.Merge(ds.Tables("ExcelReportMaintenanceDetailList"))



                    dsNew.Tables("ReportData").TableName = "Searching Criteria"
                    dsNew.Tables("ExcelReportMaintenanceDetailList").TableName = "Summary Of Directive Status"

					Session("ExcelFileName") = "Summary Of Directive Status"


					Session("dsNew") = dsNew


                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
                    'RptDirectiveStatusList = New crSummaryofDirectiveStatus
                    'Added by Prashant on 19-Jan-2021
                    MarkLog(Util.Action.Print, "SummaryofDirectiveStatus", "Export To Excel " + mSummaryofDirectiveStatusSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
                End If

                ReportLabel = "Summary of " + DirectiveName
            Else
                'If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "KamAir" Then
                'RptDirectiveStatusList = New crDetailedofDirectiveStatusForKamAir
                'Else
                'RptDirectiveStatusList = New crDetailedofDirectiveStatus
                ''RptDirectiveStatusList = New crDetailedofDirectiveStatusForKamAir
                'End If
                Dim columnToRemove1 As String() = {"ID", "Applicability", "Note", "Description", "Reference", "RemainingTimeAllExcel", "MaintenanceInformationExcel", "EffectiveFromAll", "FrequencyExcel", "SinceNewAllExcel", "EffectiveFromAllExcel", "ElaspedAllExcel", "DoneAtAllExcel", "ExtensionAllExcel", "ElapsedAll", "ExtensionAll", "DueAsOfAll", "DueAsOfAllExcel", "AssDueAsOfAllExcel", "AssDueAsOfAll", "RemainingTimeAll", "Frequency", "SinceNewAll", "MaintenanceOn", "DoneAtAll", "MaintenanceInfo", "MaintenanceInformation", "ATAChapter", "ModelID", "IsMaster", "DocumentTypeForID", "MachineID", "AssemblyStatusID", "CompStatusID", "StatusID", "EstDate", "AssemblySerialNo", "CompSerialNo", "AssemblyType", "Code", "Name", "Model", "PartNo", "SerialNo", "Position", "MonitorTypeCode", "Freq1", "Freq2", "ElapsedTime", "ElapsedTime1", "ElapsedTime2", "RemainingTime", "RemainingTime1", "RemainingTime2", "DueAsof", "DueAsof1", "EstimatedDate", "LogBook", "ComponentInfo", "RegNo", "SinceNew", "SinceNew1", "SinceNew2", "DoneAt", "DoneAt1", "DoneAt2", "AssemblyModel", "MinimumRemainingValue", "AssemblyTypeID", "MaintenanceEvent", "ATACode", "InstalledAt", "InstalledAt1", "InstalledAt2", "TSO", "TSO1", "TSO2", "RemoveAt", "RemoveAt1", "RemoveAt2", "InstalledAtDate", "RemoveAtDate", "TSN", "DoneOnDate", "DoneWONo", "DetailID", "AssemblyDueAsof", "AssemblyDueAsof1", "AssemblyDueAsof2", "Extension", "Extension1", "Extension2", "ExtensionDate", "ApprovalRemark", "RequiredManHours", "Customer", "IsApplicable", "MaintenanceTypeID", "MaintenanceTypeName", "IsLater", "DueStatus", "TimeSinceNew", "ModelMonitorModCode", "StatusTypeName", "WONumber", "StatusMasterID", "DoneONValueForAssembly", "ModelEstimatedManHours", "SourceDoc", "DiffCompInstDoneOnValue", "RecordID", "Zone", "Area", "TypeID", "MaintenanceInfoExcel", "ElapsedAllExcel", "MaintenanceOnExcel"}
                For i As Integer = 0 To columnToRemove1.Length - 1
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains(columnToRemove1(i)) Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns.Remove(columnToRemove1(i))
                    End If
                Next

                Dim columnToRemove2 As String() = {"CompanyName", "CurrencyName", "Website", "Email", "ID", "ToDate", "Address", "Tel1", "Tel2", "Fax", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ReportDate", "FromStore", "WorkShop", "WorkOrderText", "WorkOrderNo", "SearchStr4", "SearchStr5", "ReportName", "SearchStr7", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "ShortName", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25", "SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40", "SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47", "SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
                For i As Integer = 0 To columnToRemove2.Length - 1
                    If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
                        ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
                    End If
                Next
                If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
                    ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "As On Date"
                End If
                If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
                    ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "Directive"
                End If
                If ds.Tables("ReportData").Columns.Contains("SearchStr3") Then
                    ds.Tables("ReportData").Columns("SearchStr3").ColumnName = "Aircraft"
                End If
                If ds.Tables("ReportData").Columns.Contains("SearchStr6") Then
                    ds.Tables("ReportData").Columns("SearchStr6").ColumnName = "Assembly"
                End If
                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("ModificationNumber") Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("ModificationNumber").ColumnName = "AD Number"
                End If


                'If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("MaintenanceInformationForExcel") Then
                '    ds.Tables("ExcelReportMaintenanceDetailList").Columns("MaintenanceInformationForExcel").ColumnName = "Title"
                'End If
                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("MonitorType") Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("MonitorType").ColumnName = "AD_Status"
                End If
                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("ApplicabilityForExcel") Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("ApplicabilityForExcel").ColumnName = "Applicability"
                End If
                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("ReferenceForExcel") Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("ReferenceForExcel").ColumnName = "Amendment"
                End If
                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("IssueDate") Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("IssueDate").ColumnName = "EFF Date"
                End If
                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("DescriptionForExcel") Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("DescriptionForExcel").ColumnName = "Title"
                End If
                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("SupersededByADNumber") Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("SupersededByADNumber").ColumnName = "SPSD By AD"
                End If
                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("Freq3") Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("Freq3").ColumnName = "AD Deadline"
                End If
                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("DoneOnValue") Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("DoneOnValue").ColumnName = "Last ACP"
                End If
                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("DueAsOf2") Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("DueAsOf2").ColumnName = "Next Due"
                End If
                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("Remark") Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("Remark").ColumnName = "Action"
                End If
                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("NoteForExcel") Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("NoteForExcel").ColumnName = "Note"
                End If
                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("ComplianceRequirement") Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("ComplianceRequirement").ColumnName = "Method Of Compliance"
                End If
                Dim dsNew As New DataSet
                dsNew.Clear()

                dsNew.Merge(ds.Tables("ReportData"))
                dsNew.Merge(ds.Tables("ExcelReportMaintenanceDetailList"))



                dsNew.Tables("ReportData").TableName = "Searching Criteria"
                dsNew.Tables("ExcelReportMaintenanceDetailList").TableName = "Summary Of Directive Status"


				Session("ExcelFileName") = "Summary Of Directive Status"

				Session("dsNew") = dsNew


                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
                ReportLabel = "Detail of " + DirectiveName
                'Added by Prashant on 19-Jan-2021
                MarkLog(Util.Action.Print, "SummaryofDirectiveStatus", "Export To Excel " + mSummaryofDirectiveStatusSearchingCriteria, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            End If
        Else
            upnlValidation.Update() : Exit Sub
        End If
    End Sub
End Class