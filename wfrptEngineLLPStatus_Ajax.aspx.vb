Imports System.Collections.Generic
Imports System.Linq

Public Class wfrptEngineLLPStatus_Ajax
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Public mMachineNameValueList As MachineNameValueList
    Public mAssemblylist As AssemblyList
    Dim EventLogID As Guid
    Public mtmpInstalledCompList As tmpInstalledCompList
    Public mCompanyDetail As New CompanyDetail
    Dim EventLogDetail As String
    Dim searchstr7 As String = ""
    Dim ReportStatusList As New rptStatusList
    Dim ReportMaintenanceDetails As New ReportMaintenanceDetailList
#End Region

#Region "Business Methods"
    Private Sub GetSession()
        mMachineNameValueList = CType(Session("mMachineNameValueList"), MachineNameValueList)
        mAssemblylist = CType(Session("mAssemblylist"), AssemblyList)
        mtmpInstalledCompList = CType(Session("mtmpInstalledCompList"), tmpInstalledCompList)
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mMachineNameValueList")
        Session.Remove("mAssemblylist")
        Session.Remove("mtmpInstalledCompList")
    End Sub
    Private Sub SetAssemblyCombo()
        If cmbAircraft.SelectedIndex > 0 Then
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(2, cmbAircraft.SelectedValue.ToString, txtAsOnDate.Text, "(SELECT)", True)
            Session("mAssemblylist") = mAssemblylist
            cmbAssembly.DataSource = mAssemblylist
        Else
            cmbAssembly.DataSource = Nothing
        End If
        cmbAssembly.DataBind()
    End Sub
    Private Sub DataFieldBind()
        mMachineNameValueList = MachineNameValueList.GetMachineList(txtAsOnDate.Text, , , , , , , True, "(SELECT)", , True)
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataSource = mMachineNameValueList
        cmbAircraft.DataBind()

        SetAssemblyCombo()
    End Sub
    Private Sub ControlVisibility()
        cmbAssembly.Enabled = IIf(cmbAircraft.SelectedIndex > 0, True, False)
    End Sub
    Public Function ReportDetail(mMachineList As MachineList) As ReportMaintenanceDetailList
        Dim ObjMachine As MachineInfo
        Dim ObjAssemblyStatus As AssemblyStatusInfo
        Dim ObjCompStatus As CompStatusInfo
        Dim ObjCompStatusPeriod As CompStatusPeriodInfo
        Dim ObjCompMonitorInspStatus As CompMonitorInspStatusInfo
        Dim ObjCompMonitorInspStatusPeriod As CompMonitorInspStatusPeriodInfo
        Dim ObjCompMonitorServiceStatus As CompMonitorServiceStatusInfo
        Dim ObjCompMonitorServiceStatusPeriod As CompMonitorServiceStatusPeriodInfo
        Dim RemoveAt2, SerialNoPostion, DoneRemrk As String
        Dim Periodcount As Integer
        Dim Count As Integer
        Dim AssemblyID As Guid

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


        Dim ATAChapter As String = ""
        Dim RegNo As String
        Dim AssemblyType As String
        Dim Model As String
        Dim AssemblySerialNo As String
        Dim PartNo As String
        Dim CompSerialNo As String
        Dim Position As String
        Dim MonitorTypeCode As String = ""
        Dim MonitorType As String = ""
        Dim Note As String = ""
        Dim Description As String = ""
        Dim EstimatedDate As String = ""
        Dim Freq1 As String
        Dim Freq2 As String
        Dim Freq3 As String
        Dim ElapsedTime As String
        Dim ElapsedTime1 As String
        Dim ElapsedTime2 As String
        Dim RemainingTime As String
        Dim RemainingTime1 As String
        Dim RemainingTime2 As String
        Dim DueAsof As String
        Dim DueAsof1 As String
        Dim DueAsof2 As String
        Dim AssemblyModel As String
        Dim ATACode As Integer = 0
        Dim InstalledAt As String
        Dim InstalledAt1 As String
        Dim InstalledAt2 As String
        Dim TSO As String
        Dim TSN As String
        Dim TSO1 As String
        Dim TSO2 As String
        Dim RemoveAt As String
        Dim RemoveAt1 As String

        Dim InstalledAtDate As SmartDate = New SmartDate(True)
        Dim RemoveAtDate As SmartDate = New SmartDate(True)
        Dim DoneOnValue As String
        Dim DoneOnDate As SmartDate = New SmartDate(True)
        Dim AirframeDueAsof As String

        Dim IsExcel As Boolean = False
        Dim Report As Integer = 0
        Dim MPDReference As String = ""

        searchstr7 = ""
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

                LHData9 = ""
                LHData10 = ""
                'Added by Saylee on 31-Aug-2018, to show TSO for "NOVO" : NOVO31082018
                Dim mTSOMachineList As ListOfAircraftCurrentStatus
                If AppSettings("ClientCode") = "Novo" Then mTSOMachineList = ListOfAircraftCurrentStatus.GetListOfAircraftCurrentStatus("", ObjMachine.RegNo, ObjAssemblyStatus.ModelID.ToString, , , txtAsOnDate.Text)
                '******************************************************

                For Count = 0 To Periodcount - 1
                    If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID <> 2 Then
                        LHLabel2 = CType(IIf(LHLabel2 = "", LHLabel2, LHLabel2 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                        LHData2 = CType(IIf(LHData2 = "", LHData2, LHData2 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyCurrentValue
                    End If


                    If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 2 Then
                        LHLabel3 = CType(IIf(LHLabel3 = "", LHLabel3, LHLabel3 + vbNewLine), String) + "Date"
                        LHData3 = CType(IIf(LHData3 = "", LHData3, LHData3 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyInstallationValueFormatted
                    Else
                        LHLabel3 = CType(IIf(LHLabel3 = "", LHLabel3, LHLabel3 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                        LHData3 = CType(IIf(LHData3 = "", LHData3, LHData3 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyInstallationValueFormatted
                    End If

                    If AppSettings("ClientCode") = "STR" Then 'Added by Saylee on 4-Oct-2018
                        'For Airframe
                        If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID <> 2 Then
                            LHLabel5 = CType(IIf(LHLabel5 = "", LHLabel5, LHLabel5 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                            LHData5 = CType(IIf(LHData5 = "", LHData5, LHData5 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyCurrentValueByAirFrame
                        End If
                        'Added by Saylee on 28-Jan-2021, as StarAir needs to skip Hours value for LAnding Gear assembly
                        If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 1 And ObjAssemblyStatus.AssemblyTypeID = 6 Then
                            LHLabel2 = ""
                            LHData2 = ""
                            LHLabel3 = ""
                            LHData3 = ""
                        End If
                        '******************
                    End If


                    'Added by Saylee on 31-Aug-2018, to show TSO for "NOVO" : NOVO31082018
                    ''for TSO

                    If AppSettings("ClientCode") = "Novo" Then
                        For i As Integer = 0 To mTSOMachineList.Count - 1
                            If mTSOMachineList(i).SerialNo = ObjAssemblyStatus.SerialNo Then
                                If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 1 Then
                                    If Not LHData4.Contains(mTSOMachineList(i).TSO) Then
                                        If mTSOMachineList(i).TSO <> "" Then LHLabel4 = CType(IIf(LHLabel4 = "", LHLabel4, LHLabel4 + vbNewLine), String) + "TSO"
                                        LHData4 = CType(IIf(LHData4 = "", LHData4, LHData4 + vbNewLine), String) + mTSOMachineList(i).TSO

                                        'Added by Saylee on 10-Feb-2021 for NOVO1002021
                                        If mTSOMachineList(i).TSOFreq <> "" Then LHData9 = CType(IIf(LHData9 = "", LHData9, LHData9 + vbNewLine), String) + "Hours"
                                        LHData10 = CType(IIf(LHData10 = "", LHData10, LHData10 + vbNewLine), String) + mTSOMachineList(i).TSOFreq
                                        '***************
                                    End If
                                ElseIf ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 2 Then
                                    If Not LHData4.Contains(mTSOMachineList(i).DateSO) Then
                                        If mTSOMachineList(i).DateSO <> "" Then LHLabel4 = CType(IIf(LHLabel4 = "", LHLabel4, LHLabel4 + vbNewLine), String) + "Date"
                                        LHData4 = CType(IIf(LHData4 = "", LHData4, LHData4 + vbNewLine), String) + mTSOMachineList(i).DateSO

                                        'Added by Saylee on 10-Feb-2021 for NOVO1002021
                                        If mTSOMachineList(i).DateSOFreq <> "" Then LHData9 = CType(IIf(LHData9 = "", LHData9, LHData9 + vbNewLine), String) + mTSOMachineList(i).PeriodUnitName
                                        LHData10 = CType(IIf(LHData10 = "", LHData10, LHData10 + vbNewLine), String) + mTSOMachineList(i).DateSOFreq
                                        '***************
                                    End If
                                ElseIf ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 3 Then
                                    If Not LHData4.Contains(mTSOMachineList(i).CSO) Then
                                        If mTSOMachineList(i).CSO <> "" Then LHLabel4 = CType(IIf(LHLabel4 = "", LHLabel4, LHLabel4 + vbNewLine), String) + "CSO"
                                        LHData4 = CType(IIf(LHData4 = "", LHData4, LHData4 + vbNewLine), String) + mTSOMachineList(i).CSO

                                        'Added by Saylee on 10-Feb-2021 for NOVO1002021
                                        If mTSOMachineList(i).CSOFreq <> "" Then LHData9 = CType(IIf(LHData9 = "", LHData9, LHData9 + vbNewLine), String) + "Cycles"
                                        LHData10 = CType(IIf(LHData10 = "", LHData10, LHData10 + vbNewLine), String) + mTSOMachineList(i).CSOFreq
                                        '***************
                                    End If
                                End If
                            End If

                        Next
                    End If
                Next

                Dim ModelName As String = ""
                If ObjAssemblyStatus.Position = "" Then
                    SerialNoPostion = ObjAssemblyStatus.SerialNo
                    ModelName = ObjAssemblyStatus.Model
                Else
                    If AppSettings("ClientCode") = "STR" Then
                        SerialNoPostion = ObjAssemblyStatus.SerialNo
                        ModelName = ObjAssemblyStatus.Model + " (" + ObjAssemblyStatus.Position + ")" 'Added b7y saylee on 4-Oct-2018 for 
                    Else
                        SerialNoPostion = ObjAssemblyStatus.SerialNo + " (" + ObjAssemblyStatus.Position + ")"
                        ModelName = ObjAssemblyStatus.Model
                    End If
                End If
                searchstr7 = ObjMachine.Owner.ToString 'Added By Utkarsh On 07-Apr-2011 ' "Owner/Operator :- " + 
                AssemblyID = ObjAssemblyStatus.AssemblyID




                ReportStatusList.Add(New rptStatus(AssemblyID.ToString, ObjAssemblyStatus.AssemblyTypeID, , "Reg No.", ObjMachine.RegNo, ObjAssemblyStatus.AssemblyType + " " + "Model", ModelName,
                    "Serial No.", SerialNoPostion, "Due As of " & ObjAssemblyStatus.AssemblyType, LHLabel4, LHData4, "Position ", ObjAssemblyStatus.Position, ObjAssemblyStatus.AssemblyType, LHData9, LHData10, , , , , , LHLabel2, LHData2, LHLabel3, LHData3, RHData10:=LHLabel5, RHData11:=LHData5))
            Next
        Next




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
                    For Each ObjCompMonitorServiceStatus In ObjCompStatus.CompMonitorServiceStatusList
                        'Added By Prashant 22-July-2009 for records which are not applicable for Report = 0
                        If ((Report = 1 And ObjCompMonitorServiceStatus.MonitorType <> "No Frequency") And (ObjCompMonitorServiceStatus.IsApplicable = True)) Or
                            (Report = 0) Then

                            ATAChapter = ObjCompMonitorServiceStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorServiceStatus.ATANomenclature
                            ATACode = ObjCompMonitorServiceStatus.ATACode
                            Dim TaskNo As String = ""
                            'If AppSettings("ShowMaintenanceForNewClients") = "True" And ObjCompMonitorServiceStatus.TaskNo <> "" Then
                            '    TaskNo = "Task No. : " & ObjCompMonitorServiceStatus.TaskNo & IIf(IsExcel, Chr(10), vbCrLf)
                            'End If
                            TaskNo = ObjCompMonitorServiceStatus.TaskNo
                            'Description = TaskNo & ObjCompMonitorServiceStatus.Description
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

                            For Each ObjCompMonitorServiceStatusPeriod In ObjCompMonitorServiceStatus.CompMonitorServiceStatusPeriodList
                                If ObjCompMonitorServiceStatusPeriod.PeriodID = 1 Then
                                    Freq1 = Freq1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.FrequencyValue
                                    If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Then
                                        ElapsedTime = ""
                                        RemainingTime = ""
                                        DueAsof = ""
                                        AirframeDueAsof = "" 'Added By Saylee On 26-Jun-2014 For ALL26062014
                                    Else
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
                                            ElapsedTime = ElapsedTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AllElapsedValueFormatted
                                            RemainingTime = RemainingTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.RemainingValueFormatted

                                            If (AppSettings("ClientCode") IsNot Nothing) AndAlso ((AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet")) Then
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
                                        If DoneOnValue = "" Then
                                            DoneOnValue = ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                        Else
                                            DoneOnValue = DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
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
                                            ElapsedTime = ElapsedTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AllElapsedValueFormatted
                                            RemainingTime = RemainingTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.RemainingValueFormatted
                                            'DueAsof = DueAsof &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                            If (AppSettings("ClientCode") IsNot Nothing) AndAlso ((AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet")) Then
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
                                End If
								'Added PeriodID=11,15 By Vikrant For ALL 21062012
								'If ObjCompMonitorServiceStatusPeriod.PeriodID = 3 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 4 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 5 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 6 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 7 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 8 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 10 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 12 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 13 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 14 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 11 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 15 Then
								'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
								If ObjCompMonitorServiceStatusPeriod.PeriodID >= 3 Then
									If Freq1 = "" Then
										Freq1 = Freq1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.FrequencyValue
										If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Then
											ElapsedTime = ""
											RemainingTime = ""
											DueAsof = ""
											AirframeDueAsof = ""
										Else
											ElapsedTime = ElapsedTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AllElapsedValue
											RemainingTime = RemainingTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.RemainingValue
											If ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Then
												DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
											Else
												DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText
											End If
											AirframeDueAsof = AirframeDueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
										End If
										TSN = TSN & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").CompCurrentValue
										If ObjCompMonitorServiceStatus.PartMonitorServiceTypeID = 1 And (ObjCompMonitorServiceStatus.IsMaster) And ObjCompMonitorServiceStatus.DoneOnFormatted <> "" Then
											TSO = TSO & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedAtInstall
										Else
											TSO = TSO & IIf(IsExcel, Chr(10), vbCrLf) & ""
										End If
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
											ElapsedTime = ElapsedTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AllElapsedValue
											RemainingTime = RemainingTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.RemainingValue
											If ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Then
												DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
											Else
												DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText
											End If
											AirframeDueAsof = AirframeDueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
										End If
										TSN = TSN & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").CompCurrentValue
										If ObjCompMonitorServiceStatus.PartMonitorServiceTypeID = 1 And (ObjCompMonitorServiceStatus.IsMaster) And ObjCompMonitorServiceStatus.DoneOnFormatted <> "" Then
											TSO = TSO & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedAtInstall
										Else
											TSO = TSO & IIf(IsExcel, Chr(10), vbCrLf) & ""
										End If
										RemoveAt = RemoveAt & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DueOnValue
										DoneOnValue = DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DoneOnValue
									End If
								End If
							Next
                            AssemblyID = ObjAssemblyStatus.AssemblyID
                            Note = ObjCompMonitorServiceStatus.Notes
                            'CNDC
                            If (AppSettings("ClientCode") IsNot Nothing) AndAlso
                               (AppSettings("ClientCode") <> "APFT" Or
                                AppSettings("ClientCode") <> "AAP") Then DoneOnDate.Text = ObjCompMonitorServiceStatus.DoneOn


                            'DueAsof = IsAirframeDueChecked(DueAsof, AirframeDueAsof)
                            DueAsof = DueAsof
                            MPDReference = ObjCompMonitorServiceStatus.Reference  'Added by Saylee on 7-May-2014 for ALL07052015

                            If IsExcel Then
                                ATACode = ObjCompMonitorServiceStatus.ATACode
                                If ATACode.ToString.Length < 3 Then
                                    ATAChapter = ATACode.ToString.PadLeft(3, "0"c) + " " + "-" + " " + ObjCompMonitorServiceStatus.ATANomenclature
                                End If

                            End If
                            ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, MonitorType, MonitorTypeCode, Note, DoneRemrk, Description,
                                    , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, , , , , ,
                                    , , , , ATACode, InstalledAt, InstalledAt1, InstalledAt2, TSN, TSO, TSO1, TSO2, RemoveAt, RemoveAt1, RemoveAt2, InstalledAtDate.Date.ToString("g"), RemoveAtDate.Date.ToString("g"), , MPDReference, DoneOnValue, DoneOnDate.Date.ToString("g"), Zone:=TaskNo))

                        End If
                    Next
                Next
            Next
        Next

        'Inspection
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
                    For Each ObjCompMonitorInspStatus In ObjCompStatus.CompMonitorInspStatusList
                        'Added By Prashant 22-July-2009 for records which are not applicable for Report = 0
                        If ((Report = 1 And ObjCompMonitorInspStatus.MonitorType <> "No Frequency") And (ObjCompMonitorInspStatus.IsApplicable = True)) Or
                                (Report = 0) Then

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
                            For Each ObjCompMonitorInspStatusPeriod In ObjCompMonitorInspStatus.CompMonitorInspStatusPeriodList
                                If ObjCompMonitorInspStatusPeriod.PeriodID = 1 Then
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
                                        DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText
                                        AirframeDueAsof = AirframeDueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                    End If
                                    TSN = TSN & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").CompCurrentValue
                                    RemoveAt = RemoveAt & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DueOnValue
                                    DoneOnValue = DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DoneOnValue
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
                                            ElapsedTime = ElapsedTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AllElapsedValueFormatted
                                            RemainingTime = RemainingTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.RemainingValueFormatted
                                            If (AppSettings("ClientCode") IsNot Nothing) AndAlso ((AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet")) Then
                                                DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ""
                                                AirframeDueAsof = AirframeDueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ""
                                            Else
                                                DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                AirframeDueAsof = AirframeDueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                            End If

                                            RemoveAtDate.Text = ObjCompMonitorInspStatusPeriod.DueOnValue
                                            DoneOnDate.Text = ObjCompMonitorInspStatusPeriod.DoneOnValue
                                        End If
                                        TSN = TSN & IIf(IsExcel, Chr(10), vbCrLf) & ""
                                        RemoveAt = RemoveAt & IIf(IsExcel, Chr(10), vbCrLf) & ""
                                        If DoneOnValue = "" Then
                                            DoneOnValue = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                        Else
                                            DoneOnValue = DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
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
                                            ElapsedTime = ElapsedTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AllElapsedValueFormatted
                                            RemainingTime = RemainingTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.RemainingValueFormatted
                                            If (AppSettings("ClientCode") IsNot Nothing) AndAlso ((AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet")) Then
                                                DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ""
                                                AirframeDueAsof = AirframeDueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ""
                                            Else
                                                DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                AirframeDueAsof = AirframeDueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                            End If
                                            RemoveAtDate.Text = ObjCompMonitorInspStatusPeriod.DueOnValue
                                            DoneOnDate.Text = ObjCompMonitorInspStatusPeriod.DoneOnValue
                                        End If
                                        TSN = TSN & IIf(IsExcel, Chr(10), vbCrLf) & " "
                                        RemoveAt = RemoveAt & IIf(IsExcel, Chr(10), vbCrLf) & " "
                                        DoneOnValue = DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & " "
                                    End If
                                End If
								'Added PeriodID=11,15 By Vikrant For ALL 21062012
								'If ObjCompMonitorInspStatusPeriod.PeriodID = 3 Or ObjCompMonitorInspStatusPeriod.PeriodID = 4 Or ObjCompMonitorInspStatusPeriod.PeriodID = 5 Or ObjCompMonitorInspStatusPeriod.PeriodID = 6 Or ObjCompMonitorInspStatusPeriod.PeriodID = 7 Or ObjCompMonitorInspStatusPeriod.PeriodID = 8 Or ObjCompMonitorInspStatusPeriod.PeriodID = 9 Or ObjCompMonitorInspStatusPeriod.PeriodID = 10 Or ObjCompMonitorInspStatusPeriod.PeriodID = 12 Or ObjCompMonitorInspStatusPeriod.PeriodID = 13 Or ObjCompMonitorInspStatusPeriod.PeriodID = 14 Or ObjCompMonitorInspStatusPeriod.PeriodID = 15 Or ObjCompMonitorInspStatusPeriod.PeriodID = 11 Then
								'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
								If ObjCompMonitorInspStatusPeriod.PeriodID >= 3 Then
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
										TSN = TSN & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").CompCurrentValue
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
										TSN = TSN & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").CompCurrentValue
										RemoveAt = RemoveAt & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DueOnValue
										DoneOnValue = DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DoneOnValue
									End If
								End If
							Next
                            AssemblyID = ObjAssemblyStatus.AssemblyID
                            Note = ObjCompMonitorInspStatus.Notes
                            DoneOnDate.Text = ObjCompMonitorInspStatus.DoneOn

                            ''DueAsof = IsAirframeDueChecked(DueAsof, AirframeDueAsof)
                            DueAsof = DueAsof
                            MPDReference = ObjCompMonitorInspStatus.Reference  'Added by Saylee on 7-May-2014 for ALL07052015
                            If IsExcel Then
                                ATACode = ObjCompMonitorInspStatus.ATACode
                                If ATACode.ToString.Length < 3 Then
                                    ATAChapter = ATACode.ToString.PadLeft(3, "0"c) + " " + "-" + " " + ObjCompMonitorInspStatus.ATANomenclature
                                End If

                            End If
                            ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, MonitorType, MonitorTypeCode, Note, DoneRemrk, Description,
                                , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, , , , , ,
                                , , , , ATACode, InstalledAt, InstalledAt1, InstalledAt2, TSN, TSO, TSO1, TSO2, RemoveAt, RemoveAt1, RemoveAt2, InstalledAtDate.Date.ToString("g"), RemoveAtDate.Date.ToString("g"), , MPDReference, DoneOnValue, DoneOnDate.Date.ToString("g"), Zone:=""))

                        End If
                    Next
                Next
            Next
        Next

        Return ReportMaintenanceDetails

    End Function
    Private Sub SetReport(Optional ByVal IsExcel As Boolean = False)
        Dim da As New CSLA.Data.ObjectAdapter
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim rptHardTimeStatus As New CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim ds As New dsCompStatus
        Dim tempCompStatusList As List(Of tmpInstalledCompList.tmpInstalledCompInfo) = New List(Of tmpInstalledCompList.tmpInstalledCompInfo)
        Dim CurrentThrust As String = ""
        Dim AssemblyCurrentCycles As Decimal = 0

        Dim strArray As String()
        Dim MinB22RemainingValue, MinB24RemainingValue, MinB26RemainingValue As Decimal
        EventLogDetail = "AsOn Date : " + txtAsOnDate.Text.ToString + "," + " Aircraft :" + cmbAircraft.SelectedItem.ToString + "," + " Assembly : " + cmbAssembly.SelectedItem.Text  'Added by Shital on 18-Jan-2021
        mtmpInstalledCompList = tmpInstalledCompList.GetInstalledCompList(txtAsOnDate.Text, cmbAircraft.SelectedValue.ToString, "", "", New Guid(cmbAssembly.SelectedValue), IsThrustComponentsRequiredOnly:=1)

        'As no thrust comp on assembly then get other components by sending IsThrustComponentsRequiredOnly = 0
        Dim SetNoThrustComp As Boolean = False
        Dim mMachineList As MachineList
        If mtmpInstalledCompList.Count = 0 Then
            SetNoThrustComp = True
            ''  mtmpInstalledCompList = tmpInstalledCompList.GetInstalledCompList(txtAsOnDate.Text, cmbAircraft.SelectedValue.ToString, "", "", New Guid(cmbAssembly.SelectedValue), IsThrustComponentsRequiredOnly:=0)

            mMachineList = MachineList.GetMachineListMonitoringStatusForHardTimeAndDirective(txtAsOnDate.Text, cmbAircraft.SelectedValue, , , , , , , , , , True, True, , mAssemblylist(cmbAssembly.SelectedIndex).ID.ToString, , , , , , , , , , , , , , , , , , , , , , False, , False, , True, , , , , , , 0, True, True, , , SkipIsForInventoryAircarft:=True)
            ReportDetail(mMachineList)
        End If
        '''''''''''''''''''''''''''''''''''


        If mtmpInstalledCompList.Count <= 0 Then
            If Not mMachineList Is Nothing Then
                If mMachineList.Count <= 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                End If
            End If

        ElseIf (mtmpInstalledCompList.Count > 0 And IsExcel = False) Then
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1376)
        End If

        Dim mThrustTypeList As ThrustTypeList
        mThrustTypeList = ThrustTypeList.GetThrustTypeList()
        Session("mThrustTypeList") = mThrustTypeList

        Dim Report As ReportData
        If SetNoThrustComp = False Then
            If mtmpInstalledCompList.Count > 0 Then

                ''Commented by Saylee on 1-Dec-2022
                'If mtmpInstalledCompList(0).B22IsCurrentThrust Then
                '    CurrentThrust = "7B22"
                'ElseIf mtmpInstalledCompList(0).B24IsCurrentThrust Then
                '    CurrentThrust = "7B24"
                'ElseIf mtmpInstalledCompList(0).B26IsCurrentThrust Then
                '    CurrentThrust = "7B26"
                'End If

                ''Added by Saylee on 1-Dec-2022
                If mtmpInstalledCompList(0).B22IsCurrentThrust Then
                    CurrentThrust = mThrustTypeList(0).Name
                ElseIf mtmpInstalledCompList(0).B24IsCurrentThrust Then
                    CurrentThrust = mThrustTypeList(1).Name
                ElseIf mtmpInstalledCompList(0).B26IsCurrentThrust Then
                    CurrentThrust = mThrustTypeList(2).Name
                End If

                tempCompStatusList = (From child As tmpInstalledCompList.tmpInstalledCompInfo In mtmpInstalledCompList
                                      Order By child.B22RemainingValue
                                      Select child).ToList
                MinB22RemainingValue = tempCompStatusList(0).B22RemainingValue

                tempCompStatusList = (From child As tmpInstalledCompList.tmpInstalledCompInfo In mtmpInstalledCompList
                                      Order By child.B24RemainingValue
                                      Select child).ToList
                MinB24RemainingValue = tempCompStatusList(0).B24RemainingValue

                tempCompStatusList = (From child As tmpInstalledCompList.tmpInstalledCompInfo In mtmpInstalledCompList
                                      Order By child.B26RemainingValue
                                      Select child).ToList
                MinB26RemainingValue = tempCompStatusList(0).B26RemainingValue
            End If
            strArray = tempCompStatusList(0).AssemblyCurrentValueTextFormatted.Split(Chr(13))

            For Each Str As String In strArray
                If Str.Contains(" C") Then
                    Dim Value As String = Str.Split(" C")(0).Trim
                    If Value <> "" Then
                        AssemblyCurrentCycles = CDec(Value)
                    End If

                End If
            Next

        End If


        'If mtmpInstalledCompList.Count <= 0 Then
        '    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
        '    Exit Sub
        'ElseIf (mtmpInstalledCompList.Count > 0 And IsExcel = False) Then
        '    RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1376)
        'End If




        Dim OperatorName As String = ""
        Dim ServicesShortName As String = ""

        If SetNoThrustComp = False Then
            Report = New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
           mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
              mCompanyDetail.WebSite, "LIFE LIMITED PART STATUS", txtAsOnDate.Text, cmbAircraft.SelectedItem.ToString, cmbAssembly.SelectedItem.ToString, Today.Date.ToString(AppSettings("DateFormat")), CurrentThrust, AppSettings("Product Version"), AppSettings("SINote"), tempCompStatusList(0).AssemblyCurrentValueTextFormatted, MinB22RemainingValue + AssemblyCurrentCycles, MinB24RemainingValue + AssemblyCurrentCycles, Format(MinB26RemainingValue + AssemblyCurrentCycles, "#0"), AppSettings("Logo"), SearchStr11:=mThrustTypeList(0).Name, SearchStr12:=mThrustTypeList(1).Name, SearchStr13:=mThrustTypeList(2).Name)

            myReport = New crptEngineLLPStatusReport
            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mtmpInstalledCompList)
            da.Fill(ds, Report)
            da.Fill(ds, mrptImage)
            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport
            If IsExcel = False Then

                Dim Str1 As String
                Str1 = "openTranDetail();"
                MarkLog(Util.Action.Print, "EngineLLPStatus", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
            End If
        Else
            '''Current status report
            '''
            If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                rptHardTimeStatus = New crAircraftHTCompStFt2ForTaskNo
            Else
                rptHardTimeStatus = New crAircraftHTCompStFt2
            End If
            If (AppSettings("ClientCode") IsNot Nothing) AndAlso ((AppSettings("ClientCode") = "Indamer")) Then
                Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue))
                If mMachineOperatorName.OperatorName <> "" Then OperatorName = mMachineOperatorName.OperatorName
            ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
                OperatorName = searchstr7
            End If

            ServicesShortName = ""
            'For i As Integer = 0 To mServiceTypeList.Count - 1
            '    If ServicesShortName = "" Then
            '        ServicesShortName = IIf(Not mServiceTypeList(i, "").CodeType Is Nothing, mServiceTypeList(i, "").CodeType, "")
            '    Else
            '        ServicesShortName = ServicesShortName + IIf(Not mServiceTypeList(i, "").CodeType Is Nothing, "<br>" + mServiceTypeList(i, "").CodeType, "")
            '    End If

            'Next
            Dim InspsShortName As String = ""
            If AppSettings("ShowMaintenanceForNewClients") = "True" Then 'Added By Prashant on 27-Jul-2023
                InspsShortName = "" 'To hide inspection Legends : selction  set it as blank
            Else

                'For i As Integer = 0 To mInspectionTypeList.Count - 1
                '    If InspsShortName = "" Then
                '        InspsShortName = IIf(Not mInspectionTypeList(i, "").CodeType Is Nothing, mInspectionTypeList(i, "").CodeType, "")
                '    Else
                '        InspsShortName = InspsShortName + IIf(Not mInspectionTypeList(i, "").CodeType Is Nothing, "<br>" + mInspectionTypeList(i, "").CodeType, "")
                '    End If
                'Next

            End If
            Dim mPeriodNames As String
            Dim tmpPeriodUnitList As PeriodUnitList = PeriodUnitList.GetPeriodUnitList()
            For i As Integer = 0 To tmpPeriodUnitList.Count - 1
                If mPeriodNames = "" Then
                    mPeriodNames = tmpPeriodUnitList(i).Code + " : " + tmpPeriodUnitList(i).PeriodUnitName
                Else
                    mPeriodNames = mPeriodNames + "<br>" + tmpPeriodUnitList(i).Code + " : " + tmpPeriodUnitList(i).PeriodUnitName
                End If
            Next



            Report = New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
            mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
            mCompanyDetail.WebSite, "LIFE LIMITED PART STATUS", txtAsOnDate.Text.Trim, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", OperatorName, "Next Due", "Removal Planning (Assembly Val.)", AppSettings("Logo"), AppSettings("ClientCode"), ServicesShortName, InspsShortName, SearchStr16:=AppSettings("FormNo"))  'Changed by Utkarsh On 7-Apr-2011
            ds.Clear()
            '-----------Added by vikrant for Report Logo---------------
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            '----------------------------------------------------------
            da.Fill(ds, ReportMaintenanceDetails)
            da.Fill(ds, Report)
            da.Fill(ds, ReportStatusList)
            da.Fill(ds, mrptImage) 'Added by vikrant for Report Logo

            rptHardTimeStatus.SetDataSource(ds)
            Session("CrystalReport") = rptHardTimeStatus
            Dim Str1 As String
            If IsExcel = False Then

                Str1 = "openTranDetail();"
                MarkLog(Util.Action.Print, "EngineLLPStatus", EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str1, True)
            End If
        End If

        If IsExcel = True Then
            If SetNoThrustComp = False Then
                Report = New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
               mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
                  mCompanyDetail.WebSite, "LIFE LIMITED PART STATUS", txtAsOnDate.Text, cmbAircraft.SelectedItem.ToString, cmbAssembly.SelectedItem.ToString, Today.Date.ToString(AppSettings("DateFormat")), CurrentThrust, AppSettings("Product Version"), AppSettings("SINote"), tempCompStatusList(0).AssemblyCurrentValueTextFormatted, MinB22RemainingValue + AssemblyCurrentCycles, MinB24RemainingValue + AssemblyCurrentCycles, Format(MinB26RemainingValue + AssemblyCurrentCycles, "#0"), AppSettings("Logo"), SearchStr11:=mThrustTypeList(0).Name, SearchStr12:=mThrustTypeList(1).Name, SearchStr13:=mThrustTypeList(2).Name)

                ds.Clear()
                da.Fill(ds, mtmpInstalledCompList)
                da.Fill(ds, "ReportData", Report)



                Dim columnToRemove As String() = {"IsThrustMonitoringComp", "TSNFormatted", "AssemblyPeriodName", "CompCurrentCycles", "AssemblyCurrentValueTextFormatted", "AssemblyCurrentValueInDecimal", "AssemblyCurrentValueFormatted", "CompInfo", "TextFormatted", "ATACode", "IsAttachmentAdded", "TSOFormatted", "IsMaster", "AssemblyStatusID", "CompStatusID", "ValueFormatted", "Value", "PeriodNameForWeb", "PeriodName", "InstalledOnDBValue", "ATAChapter", "Code", "IsRemoved", "PartID", "CompSerialNo", "ModelID", "MachineID", "RemovedOn", "RemovedOnFormatted", "RemovedOnDBValue", "MachineInfo", "AssemblyTypeID", "AssemblyType", "AssemblyInfo", "InstalledOn", "InstalledOnFormatted", "B22IsCurrentThrust", "B24IsCurrentThrust", "B26IsCurrentThrust"}
                Dim columnToRemove1 As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "Website", "ReportName", "ProductVersion", "SINote", "ReportDate", "SearchStr6", "SearchStr7", "CurrencyName", "CurrencySymbol", "SearchStr8", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "ShortName", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25", "SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40", "SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47", "SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}
                For i As Integer = 0 To columnToRemove.Length - 1
                    If ds.Tables("tmpInstalledCompList").Columns.Contains(columnToRemove(i)) Then
                        ds.Tables("tmpInstalledCompList").Columns.Remove(columnToRemove(i))
                    End If
                Next

                For i As Integer = 0 To columnToRemove1.Length - 1
                    If ds.Tables("ReportData").Columns.Contains(columnToRemove1(i)) Then
                        ds.Tables("ReportData").Columns.Remove(columnToRemove1(i))
                    End If
                Next

                'set Column Sequence
                ds.Tables("tmpInstalledCompList").Columns("PartDescription").SetOrdinal(0)
                ds.Tables("tmpInstalledCompList").Columns("PartName").SetOrdinal(1)
                ds.Tables("tmpInstalledCompList").Columns("SerialNo").SetOrdinal(2)
                ds.Tables("tmpInstalledCompList").Columns("TSNFormattedForExcel").SetOrdinal(3)
                ds.Tables("tmpInstalledCompList").Columns("B22CurrentValue").SetOrdinal(4)
                ds.Tables("tmpInstalledCompList").Columns("B24CurrentValue").SetOrdinal(5)
                ds.Tables("tmpInstalledCompList").Columns("B26CurrentValue").SetOrdinal(6)
                ds.Tables("tmpInstalledCompList").Columns("B22LifeLimit").SetOrdinal(7)
                ds.Tables("tmpInstalledCompList").Columns("B24LifeLimit").SetOrdinal(8)
                ds.Tables("tmpInstalledCompList").Columns("B26LifeLimit").SetOrdinal(9)
                ds.Tables("tmpInstalledCompList").Columns("B22RemainingValue").SetOrdinal(10)
                ds.Tables("tmpInstalledCompList").Columns("B24RemainingValue").SetOrdinal(11)
                ds.Tables("tmpInstalledCompList").Columns("B26RemainingValue").SetOrdinal(12)

                ds.Tables("tmpInstalledCompList").Rows.Add("Engine Limited at:", "", "", System.DBNull.Value, System.DBNull.Value, System.DBNull.Value, System.DBNull.Value, System.DBNull.Value, System.DBNull.Value, System.DBNull.Value, MinB22RemainingValue + AssemblyCurrentCycles, MinB24RemainingValue + AssemblyCurrentCycles, MinB26RemainingValue + AssemblyCurrentCycles)
                ds.Tables("tmpInstalledCompList").Rows.Add("First Limited Part:", "", "", System.DBNull.Value, System.DBNull.Value, System.DBNull.Value, System.DBNull.Value, System.DBNull.Value, System.DBNull.Value, System.DBNull.Value, MinB22RemainingValue, MinB24RemainingValue, MinB26RemainingValue)
                Dim dsNew As New DataSet
                dsNew.Clear()

                dsNew.Merge(ds.Tables("ReportData"))
                dsNew.Merge(ds.Tables("tmpInstalledCompList"))




                dsNew.Tables("ReportData").Columns("SearchStr1").ColumnName = "As On Date"
                dsNew.Tables("ReportData").Columns("SearchStr2").ColumnName = "Aircraft"
                dsNew.Tables("ReportData").Columns("SearchStr3").ColumnName = "Assembly"
                dsNew.Tables("ReportData").Columns("SearchStr4").ColumnName = "Report Date"
                dsNew.Tables("ReportData").Columns("SearchStr5").ColumnName = "Current Thrust"

                dsNew.Tables("tmpInstalledCompList").Columns("PartDescription").ColumnName = "Description"
                dsNew.Tables("tmpInstalledCompList").Columns("PartName").ColumnName = "Part Number"
                dsNew.Tables("tmpInstalledCompList").Columns("SerialNo").ColumnName = "Serial Number"
                dsNew.Tables("tmpInstalledCompList").Columns("TSNFormattedForExcel").ColumnName = "Since New Values"
                dsNew.Tables("tmpInstalledCompList").Columns("B22CurrentValue").ColumnName = mThrustTypeList(0).Name + " Total Cycles"
                dsNew.Tables("tmpInstalledCompList").Columns("B24CurrentValue").ColumnName = mThrustTypeList(1).Name + " Total Cycles"
                dsNew.Tables("tmpInstalledCompList").Columns("B26CurrentValue").ColumnName = mThrustTypeList(2).Name + " Total Cycles"

                dsNew.Tables("tmpInstalledCompList").Columns("B22LifeLimit").ColumnName = mThrustTypeList(0).Name + " Life Limit (Cycles)"
                dsNew.Tables("tmpInstalledCompList").Columns("B24LifeLimit").ColumnName = mThrustTypeList(1).Name + " Life Limit (Cycles)"
                dsNew.Tables("tmpInstalledCompList").Columns("B26LifeLimit").ColumnName = mThrustTypeList(2).Name + " Life Limit (Cycles)"

                dsNew.Tables("tmpInstalledCompList").Columns("B22RemainingValue").ColumnName = mThrustTypeList(0).Name + " Remaining Cycles"
                dsNew.Tables("tmpInstalledCompList").Columns("B24RemainingValue").ColumnName = mThrustTypeList(1).Name + " Remaining Cycles"
                dsNew.Tables("tmpInstalledCompList").Columns("B26RemainingValue").ColumnName = mThrustTypeList(2).Name + " Remaining Cycles"

                dsNew.Tables("ReportData").TableName = "Searching Criteria"
                dsNew.Tables("tmpInstalledCompList").TableName = "LIFE LIMITED PART STATUS"
				Session("ExcelFileName") = "LIFE LIMITED PART STATUS"
				Session("dsNew") = dsNew
				Session("FormatReportTableInExcel") = "True"
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
                MarkLog(Util.Action.Print, "EngineLLPStatus", "Export To excel " + EventLogDetail, Util.ErrorType.NoError, Guid.Empty, EventLogID) 'Added by Shital on 18-Jan-2021
            Else
                ''COMP CURRENT STATUS
                Dim SearchingCriteria As ReportData
                'Used for showing searching criteria in Export To Excel
                SearchingCriteria = New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
            mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
            mCompanyDetail.WebSite, "LIFE LIMITED PART STATUS", txtAsOnDate.Text.Trim, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", OperatorName, "Next Due", "Removal Planning (Assembly Val.)", AppSettings("Logo"), AppSettings("ClientCode"), ServicesShortName, "", SearchStr16:=AppSettings("FormNo"))  'Changed by Utkarsh On 7-Apr-2011

                da.Fill(ds, "ExcelReportMaintenanceDetailList", ReportMaintenanceDetails)
                da.Fill(ds, "ExcelReport", SearchingCriteria)
                '   Dim ReportLabel As String

                Dim columnToRemove As String() = {
                                                "ID",
                                                "Code",
                                                "Name",
                                                "Model",
                                                "SerialNo",
                                                "MonitorType",
                                                "Freq2",
                                                "Freq3",
                                                "ElapsedTime1",
                                                "ElapsedTime2",
                                                "RemainingTime1",
                                                "RemainingTime2",
                                                "DueAsof1",
                                                "DueAsof2",
                                                "Note",
                                                "AssemblySerialNo",
                                                "EstimatedDate",
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
                                                "MinimumRemainingValue",
                                                "AssemblyTypeID",
                                                "MaintenanceEvent",
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
                                                "RemoveAt",
                                                "DoneONValueForAssembly",
                                                "MachineID", "ModelID", "DiffCompInstDoneOnValue", "MaintenanceOnExcel", "MaintenanceInformationExcel",
                                                "MaintenanceInfoExcel", "FrequencyExcel", "SinceNewAllExcel", "ElapsedAllExcel", "EffectiveFromAll", "EffectiveFromAllExcel",
                                                "DoneAtAllExcel", "ExtensionAllExcel", "DueAsofAllExcel", "AssDueAsofAllExcel", "RemainingTimeAllExcel", "DescriptionForExcel",
                                                "MaintenanceInformationForExcel", "EROQtyNosForMaterialMgmtReport", "POQtyNosForMaterialMgmtReport", "PONosForMaterialMgmtReport",
                                                "POQtyForMaterialMgmtReport", "ERONosForMaterialMgmtReport", "EROQtyForMaterialMgmtReport",
                                                "UnserviceableStockQty", "ServiceableStockQty", "BinCardTotalQty", "Area", "Zone", "RecordID", "IsMaster",
                                                "ApplicabilityForExcel", "ReferenceForExcel", "NoteForExcel", "ThresholdAccordingToTypeIDForExcel", "FrequencyAccordingToTypeIDForExcel", "DueAsOfAssemblyOrCompForExcel", "DueAsOfAirframeForExcel", "RemainingForExcel",
                                                "RemoveAtDate", "DoneOnDate", "ModelEstimatedManHours", "SourceDoc", "IsRII", "ReqNumber",
                                                "LinkedMaintenanceActivityCount", "HoursFreq", "CyclesFreq", "LandingsFreq", "DaysMnthsYrsName", "DaysMnthsYrsValue", "HoursDoneOnValue",
                                                "CyclesDoneOnValue", "LandingsDoneOnValue", "DaysMnthsYrsDoneOnValue", "Manufacturer", "InstallationWONo", "InstallationRemark", "InstPlace",
                                                "InstallationDoneBy", "TSNHours", "CSNCycles", "SinceNewDate", "SinceNewLandings", "InstCompHours", "InstCompStartDate",
                                                "InstCompCycles", "InstCompLandings", "AssemblyInstHours", "AssemblyInstCycles", "AssemblyInstStartDate", "AssemblyInstLandings",
                                                "PartMonitorCode", "PartDesc", "MonitorTypeWithCode", "PartNoSerialNoforExcel", "TSO1ForExcel", "TSOForExcel", "InstalledAtForExcel",
                                                "Freq1ForExcel", "TSNForExcel", "DoneOnValue", "RemainingTimeForExcel", "DueAsOfForExcel", "WONoExcel", "EstDate",
                                                "MaintenanceActivityType", "InstalledAt", "InstalledAtDate"
                }


                For i As Integer = 0 To columnToRemove.Length - 1
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains(columnToRemove(i)) Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns.Remove(columnToRemove(i))
                    End If
                Next
                Dim columnscnt As Integer = ds.Tables("ExcelReportMaintenanceDetailList").Columns.Count
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("Remark").SetOrdinal(columnscnt - 1)

                Dim DueLabel As String = "DueAsof"
                For i As Integer = 0 To ds.Tables("ExcelReportMaintenanceDetailList").Columns.Count - 1
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "ModificationNumber" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Directive No"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Freq1" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Frequency"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "MonitorTypeCode" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Type"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "StatusTypeName" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Status"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "DoneOnValueForExcel" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Last Carried Out"
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns("Last Carried Out").SetOrdinal(8)
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "CompSerialNo" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns("CompSerialNo").SetOrdinal(2)
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
                                                     "SearchStr14", "ShortName", "SearchStr4", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25", "SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40", "SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47", "SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"
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
                Dim dataview As DataView = ds.Tables("ExcelReportMaintenanceDetailList").DefaultView
                dataview.Sort = "ATAChapter"


                Dim dsNew As New DataSet
                dsNew.Clear()

                dsNew.Merge(ds.Tables("ExcelReport"))
                dsNew.Merge(ds.Tables("ExcelReportMaintenanceDetailList"))

                dsNew.Tables("ExcelReport").TableName = "Searching Criteria"
                dsNew.Tables("ExcelReportMaintenanceDetailList").TableName = "LIFE LIMITED PART STATUS"
				Session("ExcelFileName") = "LIFE LIMITED PART STATUS"
				Dim PeriodColumnsForExportToExcel As New List(Of String)
                PeriodColumnsForExportToExcel.AddRange(New String() {"Frequency", "ElapsedTime", "RemainingTime", "DueAsof", "Last Carried Out"})
                Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
                Session("dsNew") = dsNew
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
                'Added by Prashant on 19-Jan-2021
                MarkLog(Util.Action.Print, "ComponantCurrentStatus", "Export To Excel " + EventLogDetail + " " + "LIFE LIMITED PART STATUS", Util.ErrorType.NoError, Guid.Empty, EventLogID)

            End If
        End If

    End Sub
    'Public Sub SetValues()
    '    AsonDate = txtAsOnDate.Text.ToString                                     'Date
    '    If cmbAircraft.SelectedItem.Text = "(Select)" Then                       'Aircraft
    '        Aircraft = ""
    '    Else
    '        If cmbAssembly.SelectedItem.Text = "(All)" Then                          'Assembly
    '            AssemblyName = "{00000000-0000-0000-0000-000000000000}"
    '            Assembly1 = ""
    '            AssemblyType = "(All)"
    '            lblAssembly1.Text = "Assembly Name  : All"
    '        Else
    '            AssemblyType = mAssemblylist(cmbAssembly.SelectedIndex).AssemblyType
    '            AssemblyName = cmbAssembly.SelectedValue.ToString
    '            Assembly1 = cmbAssembly.SelectedItem.Text
    '            lblAssembly1.Text = "Assembly Name : " & Assembly1
    '        End If

    '        MachineName = cmbAircraft.SelectedValue.ToString
    '        Aircraft = cmbAircraft.SelectedItem.Text
    '        lblAircraft1.Text = "Aircraft Name : " & Aircraft
    '    End If

    '    If Not IsDate(txtAsOnDate.Text.Trim) Then            'Date  
    '        AsonDate = ""
    '    Else
    '        AsonDate = txtAsOnDate.Text.Trim
    '        lblDateRange.Text = "AsonDate : " & txtAsOnDate.Text.Trim
    '    End If
    '    'AssemblyType = mAssemblylist(cmbAssembly.SelectedIndex).AssemblyType

    '    'Set Service/Inspection checkbox list values
    '    'Service
    '    If chkService.Checked Then
    '        IsSerSelect = True
    '        ServiceTypeID = (From c In chkListServiceType.Items
    '                     Where c.Selected = True
    '                     Select CInt(c.Value)).ToArray
    '    End If
    '    'Inspection
    '    If chkInspection.Checked Then
    '        IsInsSelect = True

    '        InspectionTypeID = (From c In chkListInspectionType.Items
    '                     Where c.Selected = True
    '                     Select CInt(c.Value)).ToArray
    '    End If
    '    'End
    '    If cmbAircraft.SelectedIndex = 0 Then
    '        'do nothing
    '    Else

    '        If cmbAssembly.SelectedItem.Text = "(All)" Then
    '            ''do nothing
    '        Else
    '            If cmbComponent.SelectedItem.Text = "(All)" Then
    '                Component = "{00000000-0000-0000-0000-000000000000}"
    '                Component1 = ""
    '                lblComponent1.Text = "Component Name  : All"
    '            Else
    '                ComponentName = cmbAssembly.SelectedValue.ToString
    '                Component1 = cmbComponent.SelectedItem.Text

    '                If cmbSerialNo.SelectedItem.Text = "(All)" Then
    '                    SerialNo = "{00000000-0000-0000-0000-000000000000}"
    '                    SerialNo1 = ""
    '                    lblComponent1.Text = "Component Name : " & Component1
    '                Else
    '                    SerialNo = cmbSerialNo.SelectedValue.ToString
    '                    SerialNo1 = cmbSerialNo.SelectedItem.Text
    '                    lblComponent1.Text = "Component Name : " & Component1 & "-" & SerialNo1
    '                End If
    '            End If
    '        End If
    '    End If


    '    EventLogDetail = lblDateRange.Text + "," + lblAircraft1.Text + "," + lblAssembly1.Text + "," + lblComponent1.Text + "," + "Type of Report : " + TypeOfReport + "," + IIf(optHardTimeStatus.Checked, "Format : " + cmbFormat.SelectedItem.ToString, "") + "," + IIf(optSerializedComp.Checked, "Sort By : " + cmbSortBy.SelectedItem.ToString, "") + "," + searchstr2





    'End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal ByVale As System.EventArgs) Handles MyBase.Load
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            cmbAircraft.Focus()
            txtAsOnDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            DataFieldBind()
            ControlVisibility()
        End If
    End Sub
    Private Sub btnDisplay_Click(sender As Object, e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            SetReport(False)
        End If
    End Sub
    Private Sub btnExport_Click(sender As Object, e As System.EventArgs) Handles btnExport.Click
        If IsValid Then
            SetReport(True)
        End If
    End Sub
    Private Sub btnClose_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub txtAsOnDate_TextChanged(sender As Object, e As System.EventArgs) Handles txtAsOnDate.TextChanged
        DataFieldBind()
        ControlVisibility()
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        SetAssemblyCombo()
        ControlVisibility()
    End Sub
#End Region

End Class