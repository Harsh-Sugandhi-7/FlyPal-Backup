'Ajax Conversion By Vikrant On 27-Jan-2014
Imports System.Text
Public Class wfSearchCriteriaForService_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Dim ReportMaintenanceDetails As New ReportMaintenanceDetailList
    Dim mAssemblylist As AssemblyList
    Dim mServiceTypeList As ServiceTypeList
    Dim mServicesTypeList As ModelMonitorServiceTypeList
    Dim ReportStatusList As New rptStatusList
    Dim mMachineList As MachineList
    Dim mMachineNameValueList As MachineNameValueList
    Dim ReportLabel As String
    Dim AOdate As String
    Dim AOnDate As String
    Dim ShowCofA As Boolean = False
    Dim AsonDate As String = ""
    Dim Periodcount As Integer
    Dim Count As Integer
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
    Private Description As String
    Private EstimatedDate As String
    Private Freq3 As String
    Private ElapsedTime As String
    Private ElapsedTime1 As String
    Private ElapsedTime2 As String
    Private RemainingTime2 As String
    Private DueAsof2, SinceNew2 As String
    Private AssemblyModel As String
    Private Reference As String
    Private DoneOnValue, DoneOnRemark As String
    Private DoneOnDate As String
    Private DoneWONo As String
    Private Remark As String
    Private Extension As String
    Private Extension1 As String
    Private Extension2 As String
    Private ExtensionDate As String
    Private ApprovalRemark As String
    Dim AssemblyDueAsof2 As String
    Private Service As String
    Private ServiceName, SerialNoPostion As String
    Dim EventLogDetail As String = String.Empty

    'Added By Saylee On 10-Nov-2014
    Dim AirframeDueAsof As String
    Dim AirframeDueAsof1 As String
    Dim AirframeDueAsof2 As String
    'End
    Dim TaskNo As String = ""
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        mAssemblylist = CType(Session("mAssemblylist"), AssemblyList)
        AOnDate = Session("AOnDate")
        mServiceTypeList = Session("mServiceTypeList")
    End Sub
    Private Sub SetSession()
        Session("mMachineNameValueList") = mMachineNameValueList
        Session("mAssemblylist") = mAssemblylist
        Session("AOnDate") = AOnDate
        Session("mServiceTypeList") = mServiceTypeList
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfSearchCriteriaForService_Ajax.aspx?" Then
            Session.Remove("mMachineNameValueList")
            Session.Remove("mAssemblylist")
            Session.Remove("mServiceTypeList")
            Session.Remove("AOnDate")
        End If
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMachineNameValueList")
        Session.Remove("mAssemblylist")
        Session.Remove("mServiceTypeList")
        Session.Remove("AOnDate")
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
        upnlCurrentCriteria.Update()
    End Sub
    Private Sub SetValues()
        If cmbAircraft.SelectedIndex <= 0 Then
            lblAssembly1.Text = "Assembly Name : " & cmbAssembly.SelectedItem.ToString
            lblAircraft1.Text = "Aircraft Name : " & cmbAircraft.SelectedItem.ToString
        Else
            AssemblyType = mAssemblylist(cmbAssembly.SelectedIndex).AssemblyType
            AssemblyName = cmbAssembly.SelectedValue.ToString
            lblAssembly1.Text = "Assembly Name : " & cmbAssembly.SelectedItem.ToString
            MachineName = cmbAircraft.SelectedValue.ToString
            lblAircraft1.Text = "Aircraft Name : " & cmbAircraft.SelectedItem.ToString
        End If

        If Not IsDate(txtFromDate.Text) Then                     'AsOnDate
            AsonDate = ""
        Else
            AsonDate = txtFromDate.Text
            lblDateRange.Text = "AsonDate : " & New SmartDate(txtFromDate.Text).FormattedText
        End If

        If cmbServiceType.SelectedIndex = 0 Then     'Service
            Service = ""
            lblType1.Text = ""
        Else
            ServiceName = mServiceTypeList(cmbServiceType.SelectedIndex).Name
            Service = cmbServiceType.SelectedItem.Text
            'lblType1.Text = "Service Name : " & Service
            If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                lblType1.Text = "MPD Name : " & Service
            Else
                lblType1.Text = "Service Name : " & Service
            End If
        End If
        EventLogDetail = lblDateRange.Text + "," + lblAircraft1.Text + "," + lblAssembly1.Text + "," + lblType1.Text + "," + IIf(chkAirframeDueAsOf.Checked, chkAirframeDueAsOf.Text, "")
    End Sub
    Public Function ReportDetail() As ReportMaintenanceDetailList
        Dim ObjMachine As MachineInfo
        Dim ObjAssemblyStatus As AssemblyStatusInfo
        Dim ObjCompStatus As CompStatusInfo

        Dim ObjAssemblyMonitorServiceStatus As AssemblyMonitorServiceStatusInfo
        Dim ObjAssemblyMonitorServiceStatusPeriod As AssemblyMonitorServiceStatusPeriodInfo

        Dim ObjCompMonitorServiceStatus As CompMonitorServiceStatusInfo
        Dim ObjCompMonitorServiceStatusPeriod As CompMonitorServiceStatusPeriodInfo

        mMachineList = MachineList.GetMachineListMonitoringStatus(New SmartDate(AsonDate).Text, MachineName, , , , , , , , , , , True, , AssemblyName, SkipIsForInventoryAircarft:=True)
        Dim LHLabel2 As String = ""
        Dim LHData2 As String = ""
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
                If ObjAssemblyStatus.Position = "" Then
                    SerialNoPostion = ObjAssemblyStatus.SerialNo
                Else
                    SerialNoPostion = ObjAssemblyStatus.SerialNo + "(" + ObjAssemblyStatus.Position + ")"
                End If
                AssemblyID = ObjAssemblyStatus.AssemblyID
                ReportStatusList.Add(New rptStatus(AssemblyID.ToString, ObjAssemblyStatus.AssemblyTypeID, , "Reg No.", ObjMachine.RegNo, ObjAssemblyStatus.AssemblyType + " " + "Model", ObjAssemblyStatus.Model, _
                   "Serial No.", SerialNoPostion, IIf(chkAirframeDueAsOf.Checked, "Next Due (Airframe Values)", "Next Due"), , , , , , , , , , , , , LHLabel2, LHData2))
            Next
        Next

        mServicesTypeList = ModelMonitorServiceTypeList.GetModelMonitorServiceTypeList()

        For i As Integer = 0 To mServicesTypeList.Count - 1
            If mServicesTypeList.Item(mServicesTypeList(i, "").ID).ServiceTypeID = cmbServiceType.SelectedValue Then
                mMachineList = MachineList.GetMachineListMonitoringStatusForHardTimeAndDirective(AsonDate, MachineName, , , , , , , , , , True, True, , AssemblyName, , , , , , , , , , , ShowCofA, , True, , , , , , , , , False, , False, , True, , , mServicesTypeList(i, "").ID, , , True, 6, True, SkipIsForInventoryAircarft:=True)
                For Each ObjMachine In mMachineList
                    For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                        '********************************
                        For Each ObjAssemblyMonitorServiceStatus In ObjAssemblyStatus.AssemblyMonitorServiceStatusList
                            If (ObjAssemblyMonitorServiceStatus.IsApplicable = True) Then
                                ATAChapter = ObjAssemblyMonitorServiceStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorServiceStatus.ATANomenclature
                                If AppSettings("ShowMaintenanceForNewClients") = "True" And ObjAssemblyMonitorServiceStatus.TaskNo <> "" Then 'Added By Prasahnt on 6-Jun-2023
                                    TaskNo = "Task No. : " & ObjAssemblyMonitorServiceStatus.TaskNo & vbCrLf
                                End If
                                Description = TaskNo & ObjAssemblyMonitorServiceStatus.Description
                                DoneOnRemark = ObjAssemblyMonitorServiceStatus.DoneRemark
                                Position = ObjAssemblyStatus.Position
                                MonitorTypeCode = ObjAssemblyMonitorServiceStatus.Code
                                MonitorType = ObjAssemblyMonitorServiceStatus.Type
                                AssemblyModel = ObjAssemblyStatus.Model
                                AssemblySerialNo = ObjAssemblyStatus.SerialNo
                                Freq3 = ""
                                ElapsedTime = ""
                                ElapsedTime1 = ""
                                ElapsedTime2 = ""
                                RemainingTime2 = ""
                                DueAsof2 = ""
                                Extension = ""
                                Extension1 = ""
                                Extension2 = ""
                                DoneOnValue = ""
                                SinceNew2 = ""
                                EstimatedDate = ""
                                'Added By Saylee On 10-Nov-2017 
                                AirframeDueAsof = ""
                                AirframeDueAsof1 = ""
                                AirframeDueAsof2 = ""
                                'End
                                For Each ObjAssemblyMonitorServiceStatusPeriod In ObjAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriodList
                                    If ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 2 Then
                                        If Freq3 = "" Then
                                            Freq3 = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValueFormatted
                                            If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Or (ObjAssemblyMonitorServiceStatus.IsApplicable = False And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Then
                                                ElapsedTime2 = ""
                                                RemainingTime2 = ""
                                                DueAsof2 = ""
                                                AssemblyDueAsof2 = ""
                                                AirframeDueAsof2 = ""
                                                SinceNew2 = ""
                                            Else
                                                ElapsedTime2 = ObjAssemblyMonitorServiceStatusPeriod.ElapsedValueFormatted
                                                RemainingTime2 = ObjAssemblyMonitorServiceStatusPeriod.RemainingValueFormatted
                                                DueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted
                                                AssemblyDueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted
                                                AirframeDueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                SinceNew2 = ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValueFormatted
                                            End If
                                            Extension2 = ObjAssemblyMonitorServiceStatusPeriod.ExtensionValueFormatted
                                        Else
                                            Freq3 = Freq3 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.FrequencyValueFormatted
                                            If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Or (ObjAssemblyMonitorServiceStatus.IsApplicable = False And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Then
                                                ElapsedTime2 = ""
                                                RemainingTime2 = ""
                                                DueAsof2 = ""
                                                AssemblyDueAsof2 = ""
                                                AirframeDueAsof2 = ""
                                                SinceNew2 = ""
                                            Else
                                                ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.ElapsedValueFormatted
                                                RemainingTime2 = RemainingTime2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.RemainingValue
                                                DueAsof2 = DueAsof2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted
                                                AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted
                                                AirframeDueAsof2 = AirframeDueAsof2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                SinceNew2 = SinceNew2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValueFormatted
                                            End If
                                            Extension2 = Extension2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.ExtensionValueFormatted
                                        End If
                                    Else
                                        If Freq3 = "" Then
                                            Freq3 = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue
                                            If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Or (ObjAssemblyMonitorServiceStatus.IsApplicable = False And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Then
                                                ElapsedTime2 = ""
                                                RemainingTime2 = ""
                                                DueAsof2 = ""
                                                AssemblyDueAsof2 = ""
                                                AirframeDueAsof2 = ""
                                                SinceNew2 = ""
                                            Else
                                                ElapsedTime2 = ObjAssemblyMonitorServiceStatusPeriod.ElapsedValue
                                                RemainingTime2 = ObjAssemblyMonitorServiceStatusPeriod.RemainingValue
                                                DueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
                                                AssemblyDueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
                                                AirframeDueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                SinceNew2 = ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValue
                                            End If
                                            Extension2 = ObjAssemblyMonitorServiceStatusPeriod.ExtensionValue
                                            DoneOnValue = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
                                        Else
                                            Freq3 = Freq3 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue
                                            If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Or (ObjAssemblyMonitorServiceStatus.IsApplicable = False And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Then
                                                ElapsedTime2 = ""
                                                RemainingTime2 = ""
                                                DueAsof2 = ""
                                                AssemblyDueAsof2 = ""
                                                AirframeDueAsof2 = ""
                                                SinceNew2 = ""
                                            Else
                                                ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.ElapsedValue
                                                RemainingTime2 = RemainingTime2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.RemainingValue
                                                DueAsof2 = DueAsof2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
                                                AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
                                                AirframeDueAsof2 = AirframeDueAsof2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                SinceNew2 = SinceNew2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValue
                                            End If
                                            Extension2 = Extension2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.ExtensionValue
                                            DoneOnValue = DoneOnValue & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
                                        End If
                                    End If
                                Next
                                AssemblyID = ObjAssemblyStatus.AssemblyID
                                Note = ObjAssemblyMonitorServiceStatus.Notes
                                Remark = ObjAssemblyMonitorServiceStatus.DoneRemark
                                ExtensionDate = ObjAssemblyMonitorServiceStatus.ExtensionDate
                                DoneOnDate = ObjAssemblyMonitorServiceStatus.DoneOn


                                If mServicesTypeList(i, "").ID = 6 Then 'Estimated date will not be calculated in On Condition No Limit Service
                                    EstimatedDate = ""
                                Else
                                    EstimatedDate = ObjAssemblyMonitorServiceStatus.EstimatedDateFormatted
                                End If



                                'Added By Saylee On 10-Nov-2017 For ALL12022014
                                If chkAirframeDueAsOf.Checked Then
                                    DueAsof2 = AirframeDueAsof2
                                End If
                                'End

                                ''Added By Saylee on 2-Aug-2024 to show NonMonitoringPeriodDetails entered thru data transfer
                                If ObjAssemblyMonitorServiceStatus.NonMonitoringPeriodDetails <> "" Then
                                    DoneOnValue = DoneOnValue & vbCrLf & ObjAssemblyMonitorServiceStatus.NonMonitoringPeriodDetails.Replace("<br>", vbCrLf)
                                End If
                                '************************


                                ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, , AssemblySerialNo, Position, MonitorType, MonitorTypeCode, Note, Remark, Description,
        , EstimatedDate, , , , , Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, , , RemainingTime2,
        , , DueAsof2, AssemblyModel, , , SinceNew2, , , , , , , , , , , , , , , , , , , , , , DoneOnValue, DoneOnDate, , , , , , AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, DoneOnRemark))
                            End If
                        Next
                        '********************************
                        For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
                            For Each ObjCompMonitorServiceStatus In ObjCompStatus.CompMonitorServiceStatusList
                                If (ObjCompMonitorServiceStatus.IsApplicable = True) Then
                                    ATAChapter = ObjCompMonitorServiceStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorServiceStatus.ATANomenclature
                                    If AppSettings("ShowMaintenanceForNewClients") = "True" And ObjCompMonitorServiceStatus.TaskNo <> "" Then 'Added By Prasahnt on 6-Jun-2023
                                        TaskNo = "Task No. : " & ObjCompMonitorServiceStatus.TaskNo & vbCrLf
                                    End If
                                    Description = TaskNo & ObjCompMonitorServiceStatus.Description
                                    DoneOnRemark = ObjCompMonitorServiceStatus.DoneRemark
                                    PartNo = ObjCompStatus.PartName
                                    CompSerialNo = ObjCompStatus.CompSerialNo
                                    Position = ObjCompStatus.Position
                                    MonitorTypeCode = ObjCompMonitorServiceStatus.Code
                                    MonitorType = ObjCompMonitorServiceStatus.Type
                                    AssemblyModel = ObjAssemblyStatus.Model
                                    AssemblySerialNo = ObjAssemblyStatus.SerialNo
                                    Freq3 = ""
                                    ElapsedTime = ""
                                    ElapsedTime1 = ""
                                    ElapsedTime2 = ""
                                    RemainingTime2 = ""
                                    DueAsof2 = ""
                                    Extension = ""
                                    Extension1 = ""
                                    Extension2 = ""
                                    AssemblyDueAsof2 = ""
                                    AirframeDueAsof2 = ""
                                    DoneOnValue = ""
                                    SinceNew2 = ""
                                    For Each ObjCompMonitorServiceStatusPeriod In ObjCompMonitorServiceStatus.CompMonitorServiceStatusPeriodList
                                        If ObjCompMonitorServiceStatusPeriod.PeriodID = 2 Then
                                            If Freq3 = "" Then
                                                Freq3 = ObjCompMonitorServiceStatusPeriod.FrequencyValueFormatted
                                                If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Or (ObjCompMonitorServiceStatus.IsApplicable = False And ObjCompMonitorServiceStatus.IsCompleted = True) Then
                                                    ElapsedTime2 = ""
                                                    RemainingTime2 = ""
                                                    DueAsof2 = ""
                                                    AssemblyDueAsof2 = ""
                                                    AirframeDueAsof2 = ""
                                                    SinceNew2 = ""
                                                Else
                                                    ElapsedTime2 = ObjCompMonitorServiceStatusPeriod.ElapsedValueFormatted
                                                    RemainingTime2 = ObjCompMonitorServiceStatusPeriod.RemainingValueFormatted
                                                    DueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                    AssemblyDueAsof2 = ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                    AirframeDueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                    SinceNew2 = ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted
                                                End If
                                                Extension2 = ObjCompMonitorServiceStatusPeriod.ExtensionValueFormatted
                                            Else
                                                Freq3 = Freq3 & vbCrLf & ObjCompMonitorServiceStatusPeriod.FrequencyValueFormatted
                                                If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Or (ObjCompMonitorServiceStatus.IsApplicable = False And ObjCompMonitorServiceStatus.IsCompleted = True) Then
                                                    ElapsedTime2 = ""
                                                    RemainingTime2 = ""
                                                    DueAsof2 = ""
                                                    AssemblyDueAsof2 = ""
                                                    AirframeDueAsof2 = ""
                                                    SinceNew2 = ""
                                                Else
                                                    ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.ElapsedValueFormatted
                                                    RemainingTime2 = RemainingTime2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.RemainingValueFormatted
                                                    DueAsof2 = DueAsof2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                    AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                    AirframeDueAsof2 = AirframeDueAsof2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                    SinceNew2 = SinceNew2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted
                                                End If
                                                Extension2 = Extension2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.ExtensionValueFormatted
                                            End If
                                        Else
                                            If Freq3 = "" Then
                                                Freq3 = ObjCompMonitorServiceStatusPeriod.FrequencyValue
                                                If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Or (ObjCompMonitorServiceStatus.IsApplicable = False And ObjCompMonitorServiceStatus.IsCompleted = True) Then
                                                    ElapsedTime2 = ""
                                                    RemainingTime2 = ""
                                                    DueAsof2 = ""
                                                    AssemblyDueAsof2 = ""
                                                    AirframeDueAsof2 = ""
                                                    SinceNew2 = ""
                                                Else
                                                    ElapsedTime2 = ObjCompMonitorServiceStatusPeriod.ElapsedValue
                                                    RemainingTime2 = ObjCompMonitorServiceStatusPeriod.RemainingValue
                                                    If ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Then
                                                        DueAsof2 = ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                        AssemblyDueAsof2 = ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                        AirframeDueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                        SinceNew2 = ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted
                                                    Else
                                                        DueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                        AssemblyDueAsof2 = ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                        AirframeDueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                        SinceNew2 = ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted
                                                    End If
                                                End If
                                                Extension2 = ObjCompMonitorServiceStatusPeriod.ExtensionValue
                                                DoneOnValue = ObjCompMonitorServiceStatusPeriod.DoneOnValue
                                            Else
                                                Freq3 = Freq3 & vbCrLf & ObjCompMonitorServiceStatusPeriod.FrequencyValue
                                                If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Or (ObjCompMonitorServiceStatus.IsApplicable = False And ObjCompMonitorServiceStatus.IsCompleted = True) Then
                                                    ElapsedTime2 = ""
                                                    RemainingTime2 = ""
                                                    DueAsof2 = ""
                                                    AssemblyDueAsof2 = ""
                                                    AirframeDueAsof2 = ""
                                                    SinceNew2 = ""
                                                Else
                                                    ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.ElapsedValue
                                                    RemainingTime2 = RemainingTime2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.RemainingValue
                                                    If ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Then
                                                        DueAsof2 = DueAsof2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                        AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                        AirframeDueAsof2 = AirframeDueAsof2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                        SinceNew2 = SinceNew2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted
                                                    Else
                                                        DueAsof2 = DueAsof2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                        AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                        AirframeDueAsof2 = AirframeDueAsof2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                                        SinceNew2 = SinceNew2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted
                                                    End If
                                                End If
                                                Extension2 = Extension2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.ExtensionValue
                                                DoneOnValue = DoneOnValue & vbCrLf & ObjCompMonitorServiceStatusPeriod.DoneOnValue
                                            End If
                                        End If
                                    Next
                                    AssemblyID = ObjAssemblyStatus.AssemblyID
                                    Note = ObjCompMonitorServiceStatus.Notes
                                    Remark = ObjCompMonitorServiceStatus.DoneRemark
                                    ExtensionDate = ObjCompMonitorServiceStatus.ExtensionDate
                                    Reference = ObjCompMonitorServiceStatus.Reference
                                    DoneWONo = ObjCompMonitorServiceStatus.DoneOnWONo
                                    DoneOnDate = ObjCompMonitorServiceStatus.DoneOnFormatted

                                    If mServicesTypeList(i, "").ID = 6 Then 'Estimated date will not be calculated in On Condition No limit Service
                                        EstimatedDate = ""
                                    Else
                                        EstimatedDate = ObjCompMonitorServiceStatus.EstimatedDateFormatted
                                    End If



                                    'Added By Saylee On 10-Nov-2017 For ALL12022014
                                    If chkAirframeDueAsOf.Checked Then
                                        DueAsof2 = AirframeDueAsof2
                                    End If
                                    'End

                                    ''Added By Saylee on 2-Aug-2024 to show NonMonitoringPeriodDetails entered thru data transfer
                                    If ObjCompMonitorServiceStatus.NonMonitoringPeriodDetails <> "" Then
                                        DoneOnValue = DoneOnValue & vbCrLf & ObjCompMonitorServiceStatus.NonMonitoringPeriodDetails.Replace("<br>", vbCrLf)
                                    End If
                                    '************************


                                    ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, MonitorType, MonitorTypeCode, Note, Remark, Description,
                 , EstimatedDate, , , , , Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, , , RemainingTime2,
                 , , DueAsof2, AssemblyModel, , , SinceNew2, , , , , , , , , , , , , , , , , , , , , Reference, DoneOnValue, DoneOnDate, DoneWONo, , , , , AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, DoneOnRemark))
                                End If
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
        Dim RptServiceStatusList As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim OperatorName As String = ""

        RptServiceStatusList = New crServiceStatusList

        SetValues()
        ReportDetail()

        ReportLabel = AssemblyType + " " + ServiceName
        'Added By Vikrant On 27-Feb-2020 for showing Periods Code and their long forms at bottom of report
        Dim mPeriodUnitList As PeriodUnitList
        Dim PeriodsShortName As New StringBuilder

        mPeriodUnitList = PeriodUnitList.GetPeriodUnitList()
        For i As Integer = 0 To mPeriodUnitList.Count - 1
            PeriodsShortName.Append(mPeriodUnitList(i).Code + "-" + mPeriodUnitList(i).PeriodUnitName + ", ")
        Next
        'End

        Dim ServicesShortName As String = ""

        For i As Integer = 0 To mServicesTypeList.Count - 1
            If ServicesShortName = "" Then
                ServicesShortName = IIf(Not mServicesTypeList(i, "").CodeType Is Nothing, mServicesTypeList(i, "").CodeType, "")
            Else
                ServicesShortName = ServicesShortName + IIf(Not mServicesTypeList(i, "").CodeType Is Nothing, ", " + mServicesTypeList(i, "").CodeType, "")
            End If

        Next


        'Added by vikrant on 11-aug-2011
        If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "Indamer")) Then
            Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue))
            If mMachineOperatorName.OperatorName <> "" Then OperatorName = mMachineOperatorName.OperatorName
        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
                mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
                mCompanyDetail.WebSite, ReportLabel, New SmartDate(txtFromDate.Text).FormattedText, "", "", "", txtBottomLine.Text, AppSettings("Product Version"), AppSettings("SINote"), "", OperatorName, "", "", AppSettings("Logo"), , ServicesShortName, SearchStr17:=PeriodsShortName.ToString.Trim.TrimEnd(","))
        SetSession()
        If ReportMaintenanceDetails.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1155)
        End If
        ds.Clear()
        Dim mrptImage As rptImage = rptImage.GetImage(ds) 'Added by Shweta on 24-Feb-2012
        da.Fill(ds, ReportMaintenanceDetails)
        da.Fill(ds, Report)
        da.Fill(ds, mrptImage) 'Added by Shweta on 24-Feb-2012
        da.Fill(ds, ReportStatusList)

        RptServiceStatusList.SetDataSource(ds)
        Session("CrystalReport") = RptServiceStatusList
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "ServiceStatusReport", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID)
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
                    'SetSession()
                    'Response.Redirect("wfSearchCriteriaForService_Ajax.aspx?")
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
            ' Response.Redirect("wfSearchCriteriaForService_Ajax.aspx?")
        End If
    End Sub
#End Region

#Region " Data Binding "
    Public Sub SetComboOfMachine(ByVal AOnDate As String)
        mMachineNameValueList = MachineNameValueList.GetMachineList(AOnDate, , , , , , , True, "(SELECT)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataBind()
    End Sub
    Private Sub DataFieldBind()
        mServiceTypeList = ServiceTypeList.GetServiceTypeList(True)
        cmbServiceType.DataSource = mServiceTypeList
        Session("mServiceTypeList") = mServiceTypeList
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("Sender") = "" Then
            Session("MiddleFrame") = "wfSearchCriteriaForService_Ajax.aspx?"
            lblAssembly.Enabled = False
            cmbAssembly.Enabled = False
            AOnDate = Now.Date
            txtFromDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            Session("AOnDate") = AOnDate
            SetComboOfMachine(AOnDate)
            DataFieldBind()
            'Added By Vikrant On 14-March-2014 For All14032014
            If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then
                txtBottomLine.Text = "I hereby certify that the data specified above has been certified throughout : 									Technical Support Division: __________________ Date: _____________"
            ElseIf AppSettings("ClientCode") = "APFT" Or
                   AppSettings("ClientCode") = "AAP" Then 'Added By Saylee On 1-Oct-2018 
                txtBottomLine.Text = "I hereby certify that the data specified above has been verified throughout. Continuing Airworthiness Manager: __________________ Date: _____________"
            Else
                txtBottomLine.Text = "I hereby certify that the data specified above has been certified throughout : 									Engineering Department Manager : ____________________   Date : __________"
            End If
            'End
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
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        If cmbAircraft.SelectedIndex <= 0 Then
            lblAssembly.Enabled = False
            cmbAssembly.Enabled = False
        Else
            lblAssembly.Enabled = True
            cmbAssembly.Enabled = True

            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue.ToString, txtFromDate.Text, "(All)", True)
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
#End Region

End Class