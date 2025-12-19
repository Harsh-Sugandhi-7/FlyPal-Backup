'created by Utkarsh ON 19-Dec-2013
Imports System.Collections.Generic
Imports System.Linq

Public Class wfSearchCriteriaForComp_Ajax
    Inherits Page

#Region "Variable Declaration"

    Dim ReportMaintenanceDetails As New ReportMaintenanceDetailList
    Dim ReportStatusList As New rptStatusList
    Dim mAssemblylist As AssemblyList
    Dim mServiceTypeList As PartMonitorServiceTypeList

    'Commented and Added by Saylee on 24-8-2012
    ''Dim mInspectionTypeList As ModelMonitorInspTypeList             'Inspection
    Dim mInspectionTypeList As PartMonitorInspTypeList
    Dim mMachineList As MachineList
    Dim AOdate As String

    'Added Code By Girish
    Dim Aircraft As String
    Dim Assembly1, Component1, Component, SerialNo, SerialNo1 As String
    Dim CheckService As Boolean
    Dim CheckInspection As Boolean
    Dim ServiceType As String
    Dim InspectionType As String
    'End of Added Code

    Dim Report As Integer = 0
    Dim ShowCofA As Boolean
    Dim AsonDate As String
    Dim IsSerSelect As Boolean
    Dim IsInsSelect As Boolean
    Dim ServiceTypeID(25) As Integer
    Dim InspectionTypeID(25) As Integer
    Dim ReportLabel As String
    Dim ReportType As String
    Dim x As Integer
    Dim Periodcount As Integer
    Dim Count As Integer
    Dim AssemblyName As String
    Dim MachineName As String
    Dim Machine1 As String
    Dim AssemblyID As Guid
    Dim AircraftIndex As Integer

    Private ATAChapter As String = ""
    Private RegNo As String
    Private AssemblyType As String
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

    'Added by Saylee 13-Aug-2010
    Dim mPartListForCombo As PartListForCombo
    Dim AssemblyIndex As Integer
    Dim ComponentIndex As Integer
    Dim ComponentName As String
    Public mPartListForSerialNos As PartListForSerialNos
    Private DoneOnValue As String
    Private DoneOnDate As SmartDate = New SmartDate(True)

    Dim searchstr7 As String = "" 'Added By Utkarsh On 07-Apr-2011
    Dim mMachineNameValueList As MachineNameValueList

    'Added By Vikrant On 13-Feb-2014 For ALL13022014-1
    Dim PeriodLimt As String = String.Empty
    Dim mPerDayLimits As PerDayLimits
    Dim mByPerDayLimit As Boolean = False
    Dim mIsAverageRequired As Boolean = False
    Dim EventLogDetail As String = String.Empty
    Dim searchstr2 As String = String.Empty
    'End

    'Added By Saylee On 26-Jun-2014 For ALL26062014
    Dim AirframeDueAsof As String

    Public mATAList As ATAList          'Added by Saylee on 5-Nov-2014 for ALL05112014-1
    Private mATACode As Integer
    Private mATANomenclature As String   'Added by Saylee on 5-Nov-2014 for ALL05112014-1
    Private MPDReference As String = ""  'Added by Saylee on 7-May-2014 for ALL07052015
    Dim SearchingCriteria As ReportData
    Private IsExcel As Boolean = False
    Dim ServicesShortName As String = ""
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    Dim mLastAMPRef As LastMPDAMPRef 'Added by Ajay on 14-08-2023
    Dim AMPNo As String = ""
    Dim InstallationRemark As String = ""

#End Region

#Region "Business Methods"

    Private Sub GetSession()
        mMachineList = CType(Session("mMachineList"), MachineList)
        mAssemblylist = CType(Session("mAssemblylist"), AssemblyList)
        mServiceTypeList = CType(Session("mServiceTypeList"), PartMonitorServiceTypeList)
        mInspectionTypeList = CType(Session("mInspectionTypeList"), PartMonitorInspTypeList)
        AircraftIndex = CType(Session("AircraftIndex"), Integer)
        mMachineNameValueList = Session("mMachineNameValueList")
        mPerDayLimits = Session("mPerDayLimits") 'Added By Vikrant On 13-Feb-2014 For ALL13022014-1
        mATAList = CType(Session("mATAList"), ATAList)
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
    End Sub

    Private Sub SetSession()
        Session("mMachineList") = mMachineList
        Session("mAssemblylist") = mAssemblylist
        Session("mServiceTypeList") = mServiceTypeList
        Session("mInspectionTypeList") = mInspectionTypeList
        Session("AircraftIndex") = AircraftIndex
        Session("mMachineNameValueList") = mMachineNameValueList
        Session("mATAList") = mATAList
    End Sub

    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfSearchCriteriaForComp_Ajax.aspx?" Then
            Session.Remove("mMachineList")
            Session.Remove("mAssemblylist")
            Session.Remove("mServiceTypeList")
            Session.Remove("mInspectionTypeList")
            Session.Remove("AircraftIndex")
            Session.Remove("mMachineNameValueList")
            Session.Remove("mPerDayLimits") 'Added By Vikrant On 13-Feb-2014 For ALL13022014-1
        End If
    End Sub

    Private Overloads Sub SetFocus(control As WebControl)
        If control.Enabled = False Or control.Visible = False Then Exit Sub
        control.Focus()
    End Sub

    Private Sub ResetValues()
        'dtpAsOnDate.Value = AsonDate
        AssemblyName = "{00000000-0000-0000-0000-000000000000}"
        ReportType = ""
        ReportLabel = ""
        ServiceTypeID(0) = 0
        InspectionTypeID(0) = 0
        'ShowCofA = True
        IsSerSelect = False
        IsInsSelect = False
        AssemblyType = ""
        AsonDate = ""
    End Sub

    Private Sub Display()
        lblAircraft1.Visible = True
        lblReportType.Visible = True
        lblDateRange.Visible = True
        lblAssembly1.Visible = True
        lblComponent1.Visible = True
        upnlSearchingCriteria.Update()
    End Sub

    Public Sub SetValues()
        AsonDate = txtAsOnDate.Text.ToString                                     'Date
        If cmbAircraft.SelectedItem.Text = "(Select)" Then                       'Aircraft
            Aircraft = ""
        Else
            If cmbAssembly.SelectedItem.Text = "(All)" Then                          'Assembly
                AssemblyName = "{00000000-0000-0000-0000-000000000000}"
                Assembly1 = ""
                AssemblyType = "(All)"
                lblAssembly1.Text = "Assembly Name  : All"
            Else
                If AppSettings("ClientCode") = "7AR" AndAlso mAssemblylist(cmbAssembly.SelectedIndex).AssemblyTypeID = 4 Then
                    AssemblyType = mAssemblylist(cmbAssembly.SelectedIndex).AssemblyTypeCode
                Else
                    AssemblyType = mAssemblylist(cmbAssembly.SelectedIndex).AssemblyType
                End If
                AssemblyName = cmbAssembly.SelectedValue.ToString
                Assembly1 = cmbAssembly.SelectedItem.Text
                lblAssembly1.Text = "Assembly Name : " & Assembly1
            End If

            MachineName = cmbAircraft.SelectedValue.ToString
            Aircraft = cmbAircraft.SelectedItem.Text
            lblAircraft1.Text = "Aircraft Name : " & Aircraft
        End If

        If Not IsDate(txtAsOnDate.Text.Trim) Then            'Date  
            AsonDate = ""
        Else
            AsonDate = txtAsOnDate.Text.Trim
            lblDateRange.Text = "AsonDate : " & txtAsOnDate.Text.Trim
        End If
        'AssemblyType = mAssemblylist(cmbAssembly.SelectedIndex).AssemblyType

        'Set Service/Inspection checkbox list values
        'Service
        If chkService.Checked Then
            IsSerSelect = True
            If optOCStatus.Checked Or optNavCompStatus.Checked Then ' optNavCompStatus.Checked Added By Vikrant On 02-Jul-2018 For KAM02072018
                Dim tmpPartServiceTypeList As PartMonitorServiceTypeList
                tmpPartServiceTypeList = PartMonitorServiceTypeList.GetPartMonitorServiceTypeListForNoFrequency(IsNoFrequency:=True)
                If optOCStatus.Checked Then
                    ServiceTypeID = (From c As PartMonitorServiceTypeList.PartMonitorServiceTypeInfo In tmpPartServiceTypeList
                                     Where c.MonitorTypeID = 3 And Not c.PartMonitorServiceTypeName.StartsWith("NAV")
                                     Select CInt(c.ID)).ToArray
                ElseIf optNavCompStatus.Checked Then
                    ServiceTypeID = (From c As PartMonitorServiceTypeList.PartMonitorServiceTypeInfo In tmpPartServiceTypeList
                                     Where c.PartMonitorServiceTypeName.StartsWith("NAV")
                                     Select CInt(c.ID)).ToArray
                End If

                For i As Integer = 0 To tmpPartServiceTypeList.Count - 1
                    If ServiceTypeID.Contains(tmpPartServiceTypeList(i).ID) Then
                        If ServicesShortName = "" Then
                            ServicesShortName = IIf(Not tmpPartServiceTypeList(i).CodeType Is Nothing, tmpPartServiceTypeList(i).CodeType, "")
                        Else
                            ServicesShortName = ServicesShortName + IIf(Not tmpPartServiceTypeList(i).CodeType Is Nothing, "<br>" + tmpPartServiceTypeList(i).CodeType, "")
                        End If
                    End If
                Next
            Else
                ServiceTypeID = (From c In chkListServiceType.Items
                                 Where c.Selected = True
                                 Select CInt(c.Value)).ToArray
            End If

        End If
        'Inspection
        If chkInspection.Checked And Not optOCStatus.Checked And Not optNavCompStatus.Checked Then
            IsInsSelect = True

            InspectionTypeID = (From c In chkListInspectionType.Items
                                Where c.Selected = True
                                Select CInt(c.Value)).ToArray
        End If
        'End
        If cmbAircraft.SelectedIndex = 0 Then
            'do nothing
        Else

            If cmbAssembly.SelectedItem.Text = "(All)" Then
                ''do nothing
            Else
                If cmbComponent.SelectedItem.Text = "(All)" Then
                    Component = "{00000000-0000-0000-0000-000000000000}"
                    Component1 = ""
                    lblComponent1.Text = "Component Name  : All"
                Else
                    ComponentName = cmbAssembly.SelectedValue.ToString
                    Component1 = cmbComponent.SelectedItem.Text

                    If cmbSerialNo.SelectedItem.Text = "(All)" Then
                        SerialNo = "{00000000-0000-0000-0000-000000000000}"
                        SerialNo1 = ""
                        lblComponent1.Text = "Component Name : " & Component1
                    Else
                        SerialNo = cmbSerialNo.SelectedValue.ToString
                        SerialNo1 = cmbSerialNo.SelectedItem.Text
                        lblComponent1.Text = "Component Name : " & Component1 & "-" & SerialNo1
                    End If
                End If
            End If
        End If

        'Added By Vikrant On 13-Feb-2014 For ALL13022014-1
        If optHardTimeStatus.Checked Then
            If cmbFormat.SelectedIndex = 1 Then
                SetGridObject()
                mByPerDayLimit = True
                mIsAverageRequired = True
            End If
        Else
            mPerDayLimits = Nothing
            mByPerDayLimit = False
            mIsAverageRequired = False
        End If
        'End
        Dim TypeOfReport = IIf(optHardTimeStatus.Checked, "Hard Time Components", IIf(optCompStatus.Checked, "All Components", "Serialized Components"))
        EventLogDetail = lblDateRange.Text + "," + lblAircraft1.Text + "," + lblAssembly1.Text + "," + lblComponent1.Text + "," + "Type of Report : " + TypeOfReport + "," + IIf(optHardTimeStatus.Checked, "Format : " + cmbFormat.SelectedItem.ToString, "") + "," + IIf(optSerializedComp.Checked, "Sort By : " + cmbSortBy.SelectedItem.ToString, "") + "," + searchstr2


        ''Added by Saylee on 5-Nov-2014 for ALL05112014-1
        If cmbATAChapter.SelectedItem.Text = "(All)" Then
            mATACode = 0
            mATANomenclature = ""
            lblATAChapter1.Text = "ATA Chapter  : All"
        Else
            mATACode = mATAList(cmbATAChapter.SelectedIndex).ATACode
            mATANomenclature = mATAList(cmbATAChapter.SelectedIndex).ATANomenclature
            lblATAChapter1.Text = "ATA Chapter : " & mATAList(cmbATAChapter.SelectedIndex).ATAChapter
        End If
        '****************************************

    End Sub

    'Added By Vikrant On 13-Feb-2014 For ALL13022014-1
    Private Sub DataFieldBind()
        mPerDayLimits = PerDayLimits.GetPerDayLimits(New Guid(cmbAircraft.SelectedValue.ToString))
        gdPerDayLimit.DataSource = mPerDayLimits
        gdPerDayLimit.DataBind()
        Session("mPerDayLimits") = mPerDayLimits

        'Added by Saylee on 5-Nov-2014 for ALL05112014-1
        mATAList = ATAList.GetATAList("", "(All)")
        Session("mATAList") = mATAList
        cmbATAChapter.DataSource = mATAList
        cmbATAChapter.DataBind()
        '***************************
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

    Private Sub SetPerDayLimitValues()
        Dim mPerDayLimit As PerDayLimit
        If cmbFormat.SelectedIndex = 1 Then
            For Each mPerDayLimit In mPerDayLimits
                If CDec(Val(mPerDayLimit.PeriodLimit)) >= 0 Then
                    If searchstr2 = "" Then
                        searchstr2 = "Estimated Due Date as" & " " & searchstr2 & " " & mPerDayLimit.PeriodLimit & " " & mPerDayLimit.PeriodName
                    Else
                        searchstr2 = searchstr2 & ", " & mPerDayLimit.PeriodLimit & " " & mPerDayLimit.PeriodName
                    End If
                End If
            Next
            searchstr2 = searchstr2 & " per Day "
        End If
    End Sub
    'End

    Public Function IsAirframeDueChecked(DueAsOf, AirframeDueAsOf) As String
        If rdbAirframeDue.Checked Then
            Return AirframeDueAsOf
        Else
            Return DueAsOf
        End If
    End Function

    Public Function ReportDetail() As ReportMaintenanceDetailList
        Dim ObjMachine As MachineInfo
        Dim ObjAssemblyStatus As AssemblyStatusInfo
        Dim ObjCompStatus As CompStatusInfo
        Dim ObjCompStatusPeriod As CompStatusPeriodInfo
        Dim ObjCompMonitorInspStatus As CompMonitorInspStatusInfo
        Dim ObjCompMonitorInspStatusPeriod As CompMonitorInspStatusPeriodInfo
        Dim ObjCompMonitorServiceStatus As CompMonitorServiceStatusInfo
        Dim ObjCompMonitorServiceStatusPeriod As CompMonitorServiceStatusPeriodInfo

        Dim ComponentSerialNo, PartID As String
        mPartListForCombo = Session("mPartListForCombo")
        mPartListForSerialNos = Session("mPartListForSerialNos")

        If cmbComponent.SelectedIndex > 0 Then
            PartID = mPartListForCombo(cmbComponent.SelectedIndex).ID.ToString
            'CompSerialNo = mPartListForSerialNos(cmbSerialNo.SelectedIndex).SerialNo
            If cmbSerialNo.SelectedIndex > 0 Then
                ComponentSerialNo = mPartListForSerialNos(New Guid(cmbSerialNo.SelectedValue.ToString)).SerialNo
            Else
                ComponentSerialNo = ""
            End If
        Else
            PartID = "{00000000-0000-0000-0000-000000000000}"
            ComponentSerialNo = ""
        End If
        'Added By Vikrant ON 13-Feb-2014 For ALL13022014-1  parameters (ByPerdayLimit,PerDayLimits ,IsAverageRequired)
        mMachineList = MachineList.GetMachineListMonitoringStatusForHardTimeAndDirective(AsonDate, cmbAircraft.SelectedValue, , , , , , , , , , True, True, , mAssemblylist(cmbAssembly.SelectedIndex).ID.ToString, , , , , , , , , mATACode, mATANomenclature, ShowCofA, , , , , , , , ComponentSerialNo, , , False, , False, PartID, True, , , , , , mIsAverageRequired, 0, IsSerSelect, IsInsSelect, ByPerDayLimit:=mByPerDayLimit, PerdayLimits:=mPerDayLimits, SkipIsForInventoryAircarft:=True)
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
                If AppSettings("ClientCode") = "Novo" Then mTSOMachineList = ListOfAircraftCurrentStatus.GetListOfAircraftCurrentStatus("", ObjMachine.RegNo, ObjAssemblyStatus.ModelID.ToString, , , AsonDate)
                '******************************************************
                'Modified By Harsh on 21st Feb 2024
                For Count = 0 To Periodcount - 1
                    If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID <> 2 Then
                        If AppSettings("ClientCode") = "7AR" Then
                            LHLabel2 = CType(IIf(LHLabel2 = "", LHLabel2, LHLabel2 + vbNewLine), String) + AssemblyType + " Total " + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName + ":"
                        Else
                            LHLabel2 = CType(IIf(LHLabel2 = "", LHLabel2, LHLabel2 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                        End If
                        LHData2 = CType(IIf(LHData2 = "", LHData2, LHData2 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyCurrentValue
                    End If

                    If cmbAssembly.SelectedIndex <> 1 Then 'Except air frame
                        If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 2 Then
                            If AppSettings("ClientCode") = "7AR" Then
                                LHLabel3 = CType(IIf(LHLabel3 = "", LHLabel3, LHLabel3 + vbNewLine), String) + "Date of Installation:"
                            Else
                                LHLabel3 = CType(IIf(LHLabel3 = "", LHLabel3, LHLabel3 + vbNewLine), String) + "Date"
                            End If
                            LHData3 = CType(IIf(LHData3 = "", LHData3, LHData3 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyInstallationValueFormatted
                        Else
                            If AppSettings("ClientCode") = "7AR" Then
                                LHLabel3 = CType(IIf(LHLabel3 = "", LHLabel3, LHLabel3 + vbNewLine), String) + AssemblyType + " " + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName + " At Install:"
                            Else
                                LHLabel3 = CType(IIf(LHLabel3 = "", LHLabel3, LHLabel3 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                            End If
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
                    Else
                        LHLabel3 = ""
                        LHData3 = ""
                        LHLabel5 = ""
                        LHData5 = ""
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
                If ObjAssemblyStatus.Position = "" Or AppSettings("ClientCode") = "7AR" Then 'Modified By Harsh on 21st Feb 2024
                    SerialNoPostion = ObjAssemblyStatus.SerialNo
                    ModelName = ObjAssemblyStatus.Model
                Else
                    If AppSettings("ClientCode") = "STR" Then
                        SerialNoPostion = ObjAssemblyStatus.SerialNo
                        ModelName = ObjAssemblyStatus.Model + " (" + ObjAssemblyStatus.Position + ")" 'Added by Saylee on 4-Oct-2018 for 
                    Else
                        SerialNoPostion = ObjAssemblyStatus.SerialNo + " (" + ObjAssemblyStatus.Position + ")"
                        ModelName = ObjAssemblyStatus.Model
                    End If
                End If
                searchstr7 = ObjMachine.Owner.ToString 'Added By Utkarsh On 07-Apr-2011 ' "Owner/Operator :- " + 
                AssemblyID = ObjAssemblyStatus.AssemblyID




                ReportStatusList.Add(New rptStatus(AssemblyID.ToString, ObjAssemblyStatus.AssemblyTypeID, , "Reg No.", ObjMachine.RegNo,
                                                   ObjAssemblyStatus.AssemblyType + " " + "Model", ModelName, "Serial No.", SerialNoPostion,
                                                   IIf(rdbAirframeDue.Checked, "Next Due (Airframe Values)", "Due As of " & ObjAssemblyStatus.AssemblyType),
                                                   LHLabel4, LHData4, "Pos. ", ObjAssemblyStatus.Position, ObjAssemblyStatus.AssemblyType,
                                                   LHData9, LHData10, , , , , , LHLabel2, LHData2, LHLabel3, LHData3, RHData10:=LHLabel5, RHData11:=LHData5))
            Next
        Next

        ''mMachineList = MachineList.GetMachineListMonitoringStatusForHardTimeAndDirective(txtAsOnDate.Value.ToString, cmbAircraft.SelectedValue, , , , , , , , , , True, True, , mAssemblylist(cmbAssembly.SelectedIndex).ID.ToString, , , , , , , , , , , ShowCofA, , , , , , , , , , , False, , False, , True, , , , , , True, 6, True, True)

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

                        'HERE for 7AR, OC components are included in this HT Component report 

                        For Each ObjCompMonitorServiceStatus In ObjCompStatus.CompMonitorServiceStatusList
                            'Added By Prashant 22-July-2009 for records which are not applicable for Report = 0
                            If ((Report = 1 And ((ObjCompMonitorServiceStatus.MonitorType <> "No Frequency") Or (ObjCompMonitorServiceStatus.MonitorType = "No Frequency" And AppSettings("ClientCode") = "7AR"))) And (ObjCompMonitorServiceStatus.IsApplicable = True)) Or
                                (Report = 0) Then
                                If ServiceTypeID.Contains(ObjCompMonitorServiceStatus.PartMonitorServiceTypeID) Then
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


                                    DueAsof = IsAirframeDueChecked(DueAsof, AirframeDueAsof)
                                    MPDReference = ObjCompMonitorServiceStatus.Reference  'Added by Saylee on 7-May-2014 for ALL07052015

                                    If ObjCompMonitorServiceStatus.MonitorType = "No Frequency" Then 'Added By Saylee on 27-Sep-2024 for 7AR
                                        Freq1 = "N/L"
                                        RemainingTime = "N/A"
                                        TSN = "N/L"
                                        DueAsof = "N/A"
                                    End If


                                    If IsExcel Then
                                        Dim ATACode As Integer = ObjCompMonitorServiceStatus.ATACode
                                        If ATACode.ToString.Length < 3 Then
                                            ATAChapter = ATACode.ToString.PadLeft(3, "0"c) + " " + "-" + " " + ObjCompMonitorServiceStatus.ATANomenclature
                                        End If
                                    End If
                                    ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, MonitorType, MonitorTypeCode, Note, DoneRemrk, Description,
                                    , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, , , , , ,
                                    , , , , ATACode, InstalledAt, InstalledAt1, InstalledAt2, TSN, TSO, TSO1, TSO2, RemoveAt, RemoveAt1, RemoveAt2, InstalledAtDate.Date.ToString("g"), RemoveAtDate.Date.ToString("g"), , MPDReference, DoneOnValue, DoneOnDate.Date.ToString("g"), TaskNo:=TaskNo, PartDesc:=ObjCompStatus.PartDescription))
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
                                If InspectionTypeID.Contains(ObjCompMonitorInspStatus.PartMonitorInspTypeID) Then
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

                                    DueAsof = IsAirframeDueChecked(DueAsof, AirframeDueAsof)
                                    MPDReference = ObjCompMonitorInspStatus.Reference  'Added by Saylee on 7-May-2014 for ALL07052015
                                    If IsExcel Then
                                        Dim ATACode As Integer = ObjCompMonitorInspStatus.ATACode
                                        If ATACode.ToString.Length < 3 Then
                                            ATAChapter = ATACode.ToString.PadLeft(3, "0"c) + " " + "-" + " " + ObjCompMonitorInspStatus.ATANomenclature
                                        End If

                                    End If
                                    ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, MonitorType, MonitorTypeCode, Note, DoneRemrk, Description,
                                , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, , , , , ,
                                , , , , ATACode, InstalledAt, InstalledAt1, InstalledAt2, TSN, TSO, TSO1, TSO2, RemoveAt, RemoveAt1, RemoveAt2, InstalledAtDate.Date.ToString("g"), RemoveAtDate.Date.ToString("g"), , MPDReference, DoneOnValue, DoneOnDate.Date.ToString("g"), Zone:=""))
                                End If
                            End If
                        Next
                    Next
                Next
            Next
            'End If
            'Next
        End If
        Return ReportMaintenanceDetails
    End Function

    Public Function ReportDetailForAllComponents() As ReportMaintenanceDetailList
        Dim ObjMachine As MachineInfo
        Dim ObjAssemblyStatus As AssemblyStatusInfo
        Dim ObjCompStatus As CompStatusInfo
        Dim ObjCompStatusPeriod As CompStatusPeriodInfo
        Dim ObjCompMonitorInspStatus As CompMonitorInspStatusInfo
        Dim ObjCompMonitorInspStatusPeriod As CompMonitorInspStatusPeriodInfo
        Dim ObjCompMonitorServiceStatus As CompMonitorServiceStatusInfo
        Dim ObjCompMonitorServiceStatusPeriod As CompMonitorServiceStatusPeriodInfo

        Dim ComponentSerialNo, PartID As String
        mPartListForCombo = Session("mPartListForCombo")
        mPartListForSerialNos = Session("mPartListForSerialNos")

        If cmbComponent.SelectedIndex > 0 Then
            PartID = mPartListForCombo(cmbComponent.SelectedIndex).ID.ToString
            If cmbSerialNo.SelectedIndex > 0 Then
                ComponentSerialNo = mPartListForSerialNos(New Guid(cmbSerialNo.SelectedValue.ToString)).SerialNo
            Else
                ComponentSerialNo = ""
            End If
        Else
            PartID = "{00000000-0000-0000-0000-000000000000}"
            ComponentSerialNo = ""
        End If
        'Added By Vikrant ON 13-Feb-2014 For ALL13022014-1  parameters (ByPerdayLimit,PerDayLimits ,IsAverageRequired)
        mMachineList = MachineList.GetMachineListMonitoringStatusForHardTimeAndDirective(AsonDate, cmbAircraft.SelectedValue, , , , , , , , , , True, True, , mAssemblylist(cmbAssembly.SelectedIndex).ID.ToString, , , , , , , , , mATACode, mATANomenclature, ShowCofA, , , , , , , , ComponentSerialNo, , , False, , False, PartID, True, , , , , , mIsAverageRequired, 0, True, True, ByPerDayLimit:=mByPerDayLimit, PerdayLimits:=mPerDayLimits, SkipIsForInventoryAircarft:=True)
        Dim LHLabel2 As String = ""
        Dim LHData2 As String = ""
        Dim LHLabel3 As String = ""
        Dim LHData3 As String = ""
        Dim LHLabel4 As String = ""
        Dim LHData4 As String = ""

        For Each ObjMachine In mMachineList
            For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                Periodcount = ObjAssemblyStatus.AssemblyStatusPeriodList.Count()
                LHLabel2 = ""
                LHData2 = ""
                LHLabel3 = ""
                LHData3 = ""
                For Count = 0 To Periodcount - 1
                    If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID <> 2 Then
                        LHLabel2 = CType(IIf(LHLabel2 = "", LHLabel2, LHLabel2 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                        LHData2 = CType(IIf(LHData2 = "", LHData2, LHData2 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyCurrentValue
                    End If
                    If cmbAssembly.SelectedIndex <> 1 Then 'Except air frame
                        If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 2 Then
                            LHLabel3 = CType(IIf(LHLabel3 = "", LHLabel3, LHLabel3 + vbNewLine), String) + "Date"
                            LHData3 = CType(IIf(LHData3 = "", LHData3, LHData3 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyInstallationValueFormatted
                        Else
                            LHLabel3 = CType(IIf(LHLabel3 = "", LHLabel3, LHLabel3 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                            LHData3 = CType(IIf(LHData3 = "", LHData3, LHData3 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyInstallationValueFormatted
                        End If
                    Else
                        LHLabel3 = ""
                        LHData3 = ""
                    End If

                    If AppSettings("ClientCode") = "STR" Then
                        'Added by Saylee on 28-Jan-2021, as StarAir needs to skip Hours value for LAnding Gear assembly
                        If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 1 And ObjAssemblyStatus.AssemblyTypeID = 6 Then
                            LHLabel2 = ""
                            LHData2 = ""
                            LHLabel3 = ""
                            LHData3 = ""
                        End If
                        '******************
                    End If
                Next
                If ObjAssemblyStatus.Position = "" Then
                    SerialNoPostion = ObjAssemblyStatus.SerialNo
                Else
                    If AppSettings("ClientCode") = "STR" Then
                        SerialNoPostion = ObjAssemblyStatus.SerialNo
                    Else
                        SerialNoPostion = ObjAssemblyStatus.SerialNo + "(" + ObjAssemblyStatus.Position + ")"
                    End If
                End If
                searchstr7 = ObjMachine.Owner.ToString 'Added By Utkarsh On 07-Apr-2011 '"Owner/Operator :- " +
                AssemblyID = ObjAssemblyStatus.AssemblyID
                ReportStatusList.Add(New rptStatus(AssemblyID.ToString, ObjAssemblyStatus.AssemblyTypeID, , "Reg No.", ObjMachine.RegNo, ObjAssemblyStatus.AssemblyType + " " + "Model", ObjAssemblyStatus.Model,
                    "Serial No.", SerialNoPostion, "Due As of " & ObjAssemblyStatus.AssemblyType, LHLabel4, LHData4, "Position ", ObjAssemblyStatus.Position, ObjAssemblyStatus.AssemblyType, , , , , , , , LHLabel2, LHData2, LHLabel3, LHData3))

            Next
        Next
        For Each ObjMachine In mMachineList
            For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
                    InstalledAt = ""
                    TSO1 = ""
                    TSN = ""
                    For Each ObjCompStatusPeriod In ObjCompStatus.CompStatusPeriodList
                        If Not ObjCompStatusPeriod.PeriodID = 2 Then
                            InstalledAt = InstalledAt & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompStatusPeriod.PeriodID, "").CompInstallationTextFormatted
                            TSO1 = TSO1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompStatusPeriod.PeriodID, "").AssemblyInstallationTextFormatted
                            TSN = TSN & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompStatusPeriod.PeriodID, "").CompCurrentValueFormatted
                        Else
                            If InstalledAt = "" Then InstalledAt = InstalledAt & IIf(IsExcel, Chr(10), vbCrLf) & ""
                            If TSO1 = "" Then TSO1 = TSO1 & IIf(IsExcel, Chr(10), vbCrLf) & ""
                            TSN = TSN & IIf(IsExcel, Chr(10), vbCrLf) & AsonDate
                        End If
                    Next
                    If (ObjCompStatus.CompMonitorServiceStatusList.Count = 0 And ObjCompStatus.CompMonitorInspStatusList.Count = 0) And ((cmbATAChapter.SelectedIndex > 0 And ObjCompStatus.ATACode = mATACode And ObjCompStatus.ATANomenclature = mATANomenclature) Or cmbATAChapter.SelectedIndex = 0) Then
                        ATAChapter = ObjCompStatus.ATACode.ToString + " " + "-" + " " + ObjCompStatus.ATANomenclature
                        ATACode = ObjCompStatus.ATACode

                        Dim TaskNo As String = ""
                        'If AppSettings("ShowMaintenanceForNewClients") = "True" And ObjCompMonitorServiceStatus.TaskNo <> "" Then
                        '    TaskNo = "Task No. : " & ObjCompMonitorServiceStatus.TaskNo & IIf(IsExcel, Chr(10), vbCrLf)
                        'End If
                        'TaskNo = ObjCompMonitorServiceStatus.TaskNo
                        'Description = TaskNo & ObjCompStatus.PartDescription
                        Description = ObjCompStatus.PartDescription
                        PartNo = ObjCompStatus.PartName
                        CompSerialNo = ObjCompStatus.CompSerialNo
                        Position = ObjCompStatus.Position
                        MonitorTypeCode = ""
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
                        ATACode = ObjCompStatus.ATACode
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
                        DoneRemrk = ""
                        AssemblyID = ObjAssemblyStatus.AssemblyID
                        DoneOnValue = ""
                        DoneOnDate.Text = ""
                        AirframeDueAsof = ""
                        DueAsof = IsAirframeDueChecked(DueAsof, AirframeDueAsof)
                        If IsExcel Then
                            Dim ATACode As Integer = ObjCompStatus.ATACode
                            If ATACode.ToString.Length < 3 Then
                                ATAChapter = ATACode.ToString.PadLeft(3, "0"c) + " " + "-" + " " + ObjCompStatus.ATANomenclature
                            End If

                        End If
                        ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, MonitorType, MonitorTypeCode, Note, , Description,
                        , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, , , , , ,
                        , , , , ATACode, InstalledAt, InstalledAt1, InstalledAt2, TSN, TSO, TSO1, TSO2, RemoveAt, RemoveAt1, RemoveAt2, InstalledAtDate.Date.ToString("g"), RemoveAtDate.Date.ToString("g"), , , DoneOnValue, DoneOnDate.Date.ToString("g"), TaskNo:=TaskNo, PartDesc:=ObjCompStatus.PartDescription))
                    End If
                    '---------------------------------------------------------------------------------
                    For Each ObjCompMonitorServiceStatus In ObjCompStatus.CompMonitorServiceStatusList
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
                        DoneRemrk = ObjCompMonitorServiceStatus.DoneRemark
                        DoneOnValue = ""
                        DoneOnDate.Text = ""
                        AirframeDueAsof = ""

                        For Each ObjCompMonitorServiceStatusPeriod In ObjCompMonitorServiceStatus.CompMonitorServiceStatusPeriodList
                            If ObjCompMonitorServiceStatusPeriod.PeriodID = 1 Then
                                Freq1 = Freq1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.FrequencyValue
                                If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Then
                                    ElapsedTime = ""
                                    RemainingTime = ""
                                    DueAsof = ""
                                    AirframeDueAsof = ""
                                Else
                                    ElapsedTime = ElapsedTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AllElapsedValue
                                    RemainingTime = RemainingTime & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.RemainingValue
                                    DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText
                                    AirframeDueAsof = AirframeDueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
                                End If
                                TSN = TSN & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").CompCurrentValue
                                TSO = TSO & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedAtInstall
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
                                        'DueAsof = DueAsof &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                        If (AppSettings("ClientCode") IsNot Nothing) AndAlso ((AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet")) Then
                                            DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ""
                                        Else
                                            DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                        End If
                                        AirframeDueAsof = AirframeDueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                        RemoveAtDate.Text = ObjCompMonitorServiceStatusPeriod.DueOnValue
                                        DoneOnDate.Text = ObjCompMonitorServiceStatusPeriod.DoneOnValue
                                    End If
                                    TSN = TSN & IIf(IsExcel, Chr(10), vbCrLf) & ""
                                    TSO = TSO & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedAtInstall
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
                                        Else
                                            DueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                        End If
                                        AirframeDueAsof = AirframeDueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                        RemoveAtDate.Text = ObjCompMonitorServiceStatusPeriod.DueOnValue
                                        DoneOnDate.Text = ObjCompMonitorServiceStatusPeriod.DoneOnValue
                                    End If
                                    TSN = TSN & IIf(IsExcel, Chr(10), vbCrLf) & " "
                                    TSO = TSO & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedAtInstall
                                    RemoveAt = RemoveAt & IIf(IsExcel, Chr(10), vbCrLf) & " "
                                    DoneOnValue = DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & " "
                                End If
                            End If
							'Added PeriodID=11,15 By Vikrant For ALL 21062012
							''If ObjCompMonitorServiceStatusPeriod.PeriodID = 3 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 4 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 5 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 6 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 7 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 8 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 10 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 12 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 13 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 14 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 11 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 15 Then
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
										AirframeDueAsof = DueAsof & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextByAirFrame
									End If
									TSN = TSN & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").CompCurrentValue
									TSO = TSO & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedAtInstall
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
									TSO = TSO & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedAtInstall
									RemoveAt = RemoveAt & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DueOnValue
									DoneOnValue = DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DoneOnValue
								End If
							End If
						Next
                        AssemblyID = ObjAssemblyStatus.AssemblyID
                        Note = ObjCompMonitorServiceStatus.Notes

                        DueAsof = IsAirframeDueChecked(DueAsof, AirframeDueAsof)
                        If IsExcel Then
                            Dim ATACode As Integer = ObjCompMonitorServiceStatus.ATACode
                            If ATACode.ToString.Length < 3 Then
                                ATAChapter = ATACode.ToString.PadLeft(3, "0"c) + " " + "-" + " " + ObjCompMonitorServiceStatus.ATANomenclature
                            End If
                        End If
                        ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, MonitorType, MonitorTypeCode, Note, DoneRemrk, Description,
                        , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, , , , , ,
                        , , , , ATACode, InstalledAt, InstalledAt1, InstalledAt2, TSN, TSO, TSO1, TSO2, RemoveAt, RemoveAt1, RemoveAt2, InstalledAtDate.Date.ToString("g"), RemoveAtDate.Date.ToString("g"), , , DoneOnValue, DoneOnDate.Date.ToString("g"), TaskNo:=TaskNo, PartDesc:=ObjCompStatus.PartDescription))
                    Next
                    '-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    For Each ObjCompMonitorInspStatus In ObjCompStatus.CompMonitorInspStatusList
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
                                        'DueAsof = DueAsof &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
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
                                        'DueAsof = DueAsof &  IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
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
							'If ObjCompMonitorInspStatusPeriod.PeriodID = 3 Or ObjCompMonitorInspStatusPeriod.PeriodID = 4 Or ObjCompMonitorInspStatusPeriod.PeriodID = 5 Or ObjCompMonitorInspStatusPeriod.PeriodID = 6 Or ObjCompMonitorInspStatusPeriod.PeriodID = 7 Or ObjCompMonitorInspStatusPeriod.PeriodID = 8 Or ObjCompMonitorInspStatusPeriod.PeriodID = 9 Or ObjCompMonitorInspStatusPeriod.PeriodID = 10 Or ObjCompMonitorInspStatusPeriod.PeriodID = 12 Or ObjCompMonitorInspStatusPeriod.PeriodID = 13 Or ObjCompMonitorInspStatusPeriod.PeriodID = 14 Or ObjCompMonitorInspStatusPeriod.PeriodID = 11 Or ObjCompMonitorInspStatusPeriod.PeriodID = 15 Then
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
								Else
									Freq1 = Freq1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.FrequencyValue
									If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Then
										ElapsedTime = ""
										RemainingTime = ""
										DueAsof = ""
										AirframeDueAsof = ""
									Else
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

                        DueAsof = IsAirframeDueChecked(DueAsof, AirframeDueAsof)
                        If IsExcel Then
                            Dim ATACode As Integer = ObjCompMonitorInspStatus.ATACode
                            If ATACode.ToString.Length < 3 Then
                                ATAChapter = ATACode.ToString.PadLeft(3, "0"c) + " " + "-" + " " + ObjCompMonitorInspStatus.ATANomenclature
                            End If

                        End If
                        ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, MonitorType, MonitorTypeCode, Note, DoneRemrk, Description,
                    , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, , , , , ,
                    , , , , ATACode, InstalledAt, InstalledAt1, InstalledAt2, TSN, TSO, TSO1, TSO2, RemoveAt, RemoveAt1, RemoveAt2, InstalledAtDate.Date.ToString("g"), RemoveAtDate.Date.ToString("g"), , , DoneOnValue, DoneOnDate.Date.ToString("g"), Zone:=""))
                    Next
                    '-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                Next
            Next
        Next
        Return ReportMaintenanceDetails
    End Function

    'Added new Report by Saylee on 29-June-2010
    Public Function ReportDetailForSerializedComponents() As ReportMaintenanceDetailList
        Dim ObjMachine As MachineInfo
        Dim ObjAssemblyStatus As AssemblyStatusInfo
        Dim ObjCompStatus As CompStatusInfo
        Dim ObjCompStatusPeriod As CompStatusPeriodInfo
        Dim ComponentSerialNo, PartID As String
        mPartListForCombo = Session("mPartListForCombo")
        mPartListForSerialNos = Session("mPartListForSerialNos")

        If cmbComponent.SelectedIndex > 0 Then
            PartID = mPartListForCombo(cmbComponent.SelectedIndex).ID.ToString
            'CompSerialNo = mPartListForSerialNos(cmbSerialNo.SelectedIndex).SerialNo
            If cmbSerialNo.SelectedIndex > 0 Then
                ComponentSerialNo = mPartListForSerialNos(New Guid(cmbSerialNo.SelectedValue.ToString)).SerialNo
            Else
                ComponentSerialNo = ""
            End If
        Else
            PartID = "{00000000-0000-0000-0000-000000000000}"
            ComponentSerialNo = ""
        End If
        'Added By Vikrant ON 13-Feb-2014 For ALL13022014-1  parameters (ByPerdayLimit,PerDayLimits ,IsAverageRequired)
        mMachineList = MachineList.GetMachineListMonitoringStatusForHardTimeAndDirective(AsonDate, cmbAircraft.SelectedValue, , , , , , , , , , True, True, , mAssemblylist(cmbAssembly.SelectedIndex).ID.ToString, , , , , , , , , mATACode, mATANomenclature, ShowCofA, , , , , , , , ComponentSerialNo, , , False, , False, PartID, True, , , , , , mIsAverageRequired, 0, True, True, ByPerDayLimit:=mByPerDayLimit, PerdayLimits:=mPerDayLimits, SkipIsForInventoryAircarft:=True)
        Dim LHLabel2 As String = ""
        Dim LHData2 As String = ""
        Dim LHLabel3 As String = ""
        Dim LHData3 As String = ""
        Dim LHLabel4 As String = ""
        Dim LHData4 As String = ""

        For Each ObjMachine In mMachineList
            For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                Periodcount = ObjAssemblyStatus.AssemblyStatusPeriodList.Count()
                LHLabel2 = ""
                LHData2 = ""
                LHLabel3 = ""
                LHData3 = ""
                For Count = 0 To Periodcount - 1
                    If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID <> 2 Then
                        LHLabel2 = CType(IIf(LHLabel2 = "", LHLabel2, LHLabel2 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                        LHData2 = CType(IIf(LHData2 = "", LHData2, LHData2 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyCurrentValue
                    End If
                    If cmbAssembly.SelectedIndex <> 1 Then 'Except air frame
                        If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 2 Then
                            LHLabel3 = CType(IIf(LHLabel3 = "", LHLabel3, LHLabel3 + vbNewLine), String) + "Date"
                            LHData3 = CType(IIf(LHData3 = "", LHData3, LHData3 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyInstallationValueFormatted
                        Else
                            LHLabel3 = CType(IIf(LHLabel3 = "", LHLabel3, LHLabel3 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                            LHData3 = CType(IIf(LHData3 = "", LHData3, LHData3 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyInstallationValueFormatted
                        End If
                    Else
                        LHLabel3 = ""
                        LHData3 = ""
                    End If

                    If AppSettings("ClientCode") = "STR" Then
                        'Added by Saylee on 28-Jan-2021, as StarAir needs to skip Hours value for LAnding Gear assembly
                        If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 1 And ObjAssemblyStatus.AssemblyTypeID = 6 Then
                            LHLabel2 = ""
                            LHData2 = ""
                            LHLabel3 = ""
                            LHData3 = ""
                        End If
                        '******************
                    End If

                Next
                If ObjAssemblyStatus.Position = "" Then
                    SerialNoPostion = ObjAssemblyStatus.SerialNo
                Else
                    If AppSettings("ClientCode") = "STR" Then
                        SerialNoPostion = ObjAssemblyStatus.SerialNo
                    Else
                        SerialNoPostion = ObjAssemblyStatus.SerialNo + "(" + ObjAssemblyStatus.Position + ")"
                    End If
                End If
                searchstr7 = ObjMachine.Owner.ToString 'Added By Utkarsh On 07-Apr-2011 '  "Owner/Operator :- " +
                AssemblyID = ObjAssemblyStatus.AssemblyID
                ReportStatusList.Add(New rptStatus(AssemblyID.ToString, ObjAssemblyStatus.AssemblyTypeID, , "Reg No.", ObjMachine.RegNo, ObjAssemblyStatus.AssemblyType + " " + "Model", ObjAssemblyStatus.Model,
                    "Serial No.", SerialNoPostion, "Due As of " & ObjAssemblyStatus.AssemblyType, LHLabel4, LHData4, "Position ", ObjAssemblyStatus.Position, ObjAssemblyStatus.AssemblyType, , , , , , , , LHLabel2, LHData2, LHLabel3, LHData3))
            Next
        Next
        For Each ObjMachine In mMachineList
            For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
                    InstalledAt = ""
                    TSO1 = ""
                    TSN = "" 'Added by Saylee on 22-July-2014 for ALL22072014
                    For Each ObjCompStatusPeriod In ObjCompStatus.CompStatusPeriodList
                        If Not ObjCompStatusPeriod.PeriodID = 2 Then
                            InstalledAt = InstalledAt & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompStatusPeriod.PeriodID, "").CompInstallationTextFormatted
                            TSO1 = TSO1 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompStatusPeriod.PeriodID, "").AssemblyInstallationTextFormatted
                            TSN = TSN & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompStatusPeriod.PeriodID, "").CompCurrentValueFormatted 'Added by Saylee on 22-July-2014 for ALL22072014
                        Else
                            If InstalledAt = "" Then InstalledAt = InstalledAt & IIf(IsExcel, Chr(10), vbCrLf) & ""
                            If TSO1 = "" Then TSO1 = TSO1 & IIf(IsExcel, Chr(10), vbCrLf) & ""
                            TSN = TSN & IIf(IsExcel, Chr(10), vbCrLf) & AsonDate 'Added by Saylee on 22-July-2014 for ALL22072014
                        End If
                    Next
                    If ObjCompStatus.CompMonitorServiceStatusList.Count = 0 And ObjCompStatus.CompMonitorInspStatusList.Count = 0 And ((cmbATAChapter.SelectedIndex > 0 And ObjCompStatus.ATACode = mATACode And ObjCompStatus.ATANomenclature = mATANomenclature) Or cmbATAChapter.SelectedIndex = 0) Then
                        ATAChapter = ObjCompStatus.ATACode.ToString + " " + "-" + " " + ObjCompStatus.ATANomenclature
                        ATACode = ObjCompStatus.ATACode
                        Description = ObjCompStatus.PartDescription
                        PartNo = ObjCompStatus.PartName
                        CompSerialNo = ObjCompStatus.CompSerialNo
                        Position = ObjCompStatus.Position
                        MonitorTypeCode = ""
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
                        ATACode = ObjCompStatus.ATACode
                        InstalledAt1 = ""
                        InstalledAt2 = ""
                        '   TSN = ""
                        TSO = ""
                        TSO2 = ""
                        RemoveAt = ""
                        RemoveAt1 = ""
                        RemoveAt2 = ""
                        InstalledAtDate.Text = ObjCompStatus.InstalledOn
                        RemoveAtDate.Text = ""
                        DoneRemrk = ""
                        AssemblyID = ObjAssemblyStatus.AssemblyID
                        DoneOnValue = ""
                        DoneOnDate.Text = ""
                        AirframeDueAsof = ""
                        InstallationRemark = ObjCompStatus.InstallationRemark

                        DueAsof = IsAirframeDueChecked(DueAsof, AirframeDueAsof)
                        If IsExcel Then
                            Dim ATACode As Integer = ObjCompStatus.ATACode
                            If ATACode.ToString.Length < 3 Then
                                ATAChapter = ATACode.ToString.PadLeft(3, "0"c) + " " + "-" + " " + ObjCompStatus.ATANomenclature
                            End If

                        End If
                        ' PartDesc added by Harsh on 30th April 2024 for FLYPAL-1586.
                        ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, MonitorType, MonitorTypeCode, Note, , Description,
                        , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, , , , , ,
                        , , , , ATACode, InstalledAt, InstalledAt1, InstalledAt2, TSN, TSO, TSO1, TSO2, RemoveAt, RemoveAt1, RemoveAt2, InstalledAtDate.Date.ToString("g"),
                        RemoveAtDate.Date.ToString("g"), , , DoneOnValue, DoneOnDate.Date.ToString("g"), Zone:="", InstallationRemark:=InstallationRemark, PartDesc:=ObjCompStatus.PartDescription))
                    End If
                Next
            Next
        Next
        Return ReportMaintenanceDetails
    End Function

    Public Function ReportDetailForOCComponents() As ReportMaintenanceDetailList
        Dim ObjMachine As MachineInfo
        Dim ObjAssemblyStatus As AssemblyStatusInfo
        Dim ObjCompStatus As CompStatusInfo
        Dim ObjCompStatusPeriod As CompStatusPeriodInfo
        Dim ObjCompMonitorServiceStatus As CompMonitorServiceStatusInfo
        Dim ObjCompMonitorServiceStatusPeriod As CompMonitorServiceStatusPeriodInfo

        Dim ComponentSerialNo, PartID As String
        mPartListForCombo = Session("mPartListForCombo")
        mPartListForSerialNos = Session("mPartListForSerialNos")

        If cmbComponent.SelectedIndex > 0 Then
            PartID = mPartListForCombo(cmbComponent.SelectedIndex).ID.ToString
            'CompSerialNo = mPartListForSerialNos(cmbSerialNo.SelectedIndex).SerialNo
            If cmbSerialNo.SelectedIndex > 0 Then
                ComponentSerialNo = mPartListForSerialNos(New Guid(cmbSerialNo.SelectedValue.ToString)).SerialNo
            Else
                ComponentSerialNo = ""
            End If
        Else
            PartID = "{00000000-0000-0000-0000-000000000000}"
            ComponentSerialNo = ""
        End If
        'Added By Vikrant ON 13-Feb-2014 For ALL13022014-1  parameters (ByPerdayLimit,PerDayLimits ,IsAverageRequired)
        mMachineList = MachineList.GetMachineListMonitoringStatusForHardTimeAndDirective(AsonDate, cmbAircraft.SelectedValue, , , , , , , , , , True, True, , mAssemblylist(cmbAssembly.SelectedIndex).ID.ToString, , , , , , , , , mATACode, mATANomenclature, ShowCofA, , , , , , , , ComponentSerialNo, , , False, , False, PartID, True, , , , , , mIsAverageRequired, 0, IsSerSelect, IsInsSelect, ByPerDayLimit:=mByPerDayLimit, PerdayLimits:=mPerDayLimits, SkipIsForInventoryAircarft:=True)
        Dim LHLabel2 As String = ""
        Dim LHData2 As String = ""
        Dim LHLabel3 As String = ""
        Dim LHData3 As String = ""
        Dim LHLabel4 As String = ""
        Dim LHData4 As String = ""

        For Each ObjMachine In mMachineList
            For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                Periodcount = ObjAssemblyStatus.AssemblyStatusPeriodList.Count()
                LHLabel2 = ""
                LHData2 = ""
                LHLabel3 = ""
                LHData3 = ""
                For Count = 0 To Periodcount - 1
                    If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID <> 2 Then
                        LHLabel2 = CType(IIf(LHLabel2 = "", LHLabel2, LHLabel2 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                        LHData2 = CType(IIf(LHData2 = "", LHData2, LHData2 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyCurrentValue
                    End If
                    If cmbAssembly.SelectedIndex <> 1 Then 'Except air frame
                        If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 2 Then
                            LHLabel3 = CType(IIf(LHLabel3 = "", LHLabel3, LHLabel3 + vbNewLine), String) + "Date"
                            LHData3 = CType(IIf(LHData3 = "", LHData3, LHData3 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyInstallationValueFormatted
                        Else
                            LHLabel3 = CType(IIf(LHLabel3 = "", LHLabel3, LHLabel3 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                            LHData3 = CType(IIf(LHData3 = "", LHData3, LHData3 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyInstallationValueFormatted
                        End If
                    Else
                        LHLabel3 = ""
                        LHData3 = ""
                    End If

                    If AppSettings("ClientCode") = "STR" Then
                        'Added by Saylee on 28-Jan-2021, as StarAir needs to skip Hours value for LAnding Gear assembly
                        If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 1 And ObjAssemblyStatus.AssemblyTypeID = 6 Then
                            LHLabel2 = ""
                            LHData2 = ""
                            LHLabel3 = ""
                            LHData3 = ""
                        End If
                        '******************
                    End If

                Next
                If ObjAssemblyStatus.Position = "" Then
                    SerialNoPostion = ObjAssemblyStatus.SerialNo
                Else
                    If AppSettings("ClientCode") = "STR" Then
                        SerialNoPostion = ObjAssemblyStatus.SerialNo
                    Else
                        SerialNoPostion = ObjAssemblyStatus.SerialNo + "(" + ObjAssemblyStatus.Position + ")"
                    End If
                End If
                searchstr7 = ObjMachine.Owner.ToString 'Added By Utkarsh On 07-Apr-2011 ' "Owner/Operator :- " + 
                AssemblyID = ObjAssemblyStatus.AssemblyID
                ReportStatusList.Add(New rptStatus(AssemblyID.ToString, ObjAssemblyStatus.AssemblyTypeID, , "Reg No.", ObjMachine.RegNo, ObjAssemblyStatus.AssemblyType + " " + "Model", ObjAssemblyStatus.Model,
                    "Serial No.", SerialNoPostion, "Due As of " & ObjAssemblyStatus.AssemblyType, LHLabel4, LHData4, "Position ", ObjAssemblyStatus.Position, ObjAssemblyStatus.AssemblyType, , , , , , , , LHLabel2, LHData2, LHLabel3, LHData3))
            Next
        Next
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

                        For Each ObjCompMonitorServiceStatus In ObjCompStatus.CompMonitorServiceStatusList
                            'Added By Prashant 22-July-2009 for records which are not applicable for Report = 0
                            If ((Report = 2 And ObjCompMonitorServiceStatus.MonitorType = "No Frequency") And (ObjCompMonitorServiceStatus.IsApplicable = True)) Then
                                If ServiceTypeID.Contains(ObjCompMonitorServiceStatus.PartMonitorServiceTypeID) Then
                                    ATAChapter = ObjCompMonitorServiceStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorServiceStatus.ATANomenclature
                                    ATACode = ObjCompMonitorServiceStatus.ATACode
                                    Dim TaskNo As String = ""
                                    TaskNo = ObjCompMonitorServiceStatus.TaskNo
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
                                            TSN = TSN & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompStatus.CompStatusPeriodList(ObjCompMonitorServiceStatusPeriod.PeriodID, "").CompCurrentValue
                                            If ObjCompMonitorServiceStatus.PartMonitorServiceTypeID = 1 And (ObjCompMonitorServiceStatus.IsMaster) And ObjCompMonitorServiceStatus.DoneOnFormatted <> "" Then
                                                TSO = TSO & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedAtInstall
                                            Else
                                                TSO = TSO & IIf(IsExcel, Chr(10), vbCrLf) & ""
                                            End If
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
                                                TSN = TSN & IIf(IsExcel, Chr(10), vbCrLf) & ""

                                                If ObjCompMonitorServiceStatus.PartMonitorServiceTypeID = 1 And (ObjCompMonitorServiceStatus.IsMaster) And ObjCompMonitorServiceStatus.DoneOnFormatted <> "" Then
                                                    TSO = TSO & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedAtInstall
                                                Else
                                                    TSO = TSO & IIf(IsExcel, Chr(10), vbCrLf) & ""
                                                End If
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
                                                TSN = TSN & IIf(IsExcel, Chr(10), vbCrLf) & " "
                                                If ObjCompMonitorServiceStatus.PartMonitorServiceTypeID = 1 And (ObjCompMonitorServiceStatus.IsMaster) And ObjCompMonitorServiceStatus.DoneOnFormatted <> "" Then
                                                    TSO = TSO & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ElapsedAtInstall
                                                Else
                                                    TSO = TSO & IIf(IsExcel, Chr(10), vbCrLf) & ""
                                                End If
                                                RemoveAt = RemoveAt & IIf(IsExcel, Chr(10), vbCrLf) & " "
                                                DoneOnValue = DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & " "
                                            End If
                                        End If
										'Added PeriodID=11,15 By Vikrant For ALL 21062012
										''If ObjCompMonitorServiceStatusPeriod.PeriodID = 3 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 4 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 5 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 6 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 7 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 8 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 10 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 12 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 13 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 14 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 11 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 15 Then
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
                                    If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") <> "APFT") Then DoneOnDate.Text = ObjCompMonitorServiceStatus.DoneOn
                                    DueAsof = IsAirframeDueChecked(DueAsof, AirframeDueAsof)
                                    MPDReference = ObjCompMonitorServiceStatus.Reference  'Added by Saylee on 7-May-2014 for ALL07052015

                                    If IsExcel Then
                                        Dim ATACode As Integer = ObjCompMonitorServiceStatus.ATACode
                                        If ATACode.ToString.Length < 3 Then
                                            ATAChapter = ATACode.ToString.PadLeft(3, "0"c) + " " + "-" + " " + ObjCompMonitorServiceStatus.ATANomenclature
                                        End If
                                    End If
                                    ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, MonitorType, MonitorTypeCode, Note, DoneRemrk, Description,
                                    , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, , , , , ,
                                    , , , , ATACode, InstalledAt, InstalledAt1, InstalledAt2, TSN, TSO, TSO1, TSO2, RemoveAt, RemoveAt1, RemoveAt2, InstalledAtDate.Date.ToString("g"), RemoveAtDate.Date.ToString("g"), , MPDReference, DoneOnValue, DoneOnDate.Date.ToString("g"), TaskNo:=TaskNo, PartDesc:=ObjCompStatus.PartDescription))
                                End If
                            End If
                        Next
                    Next
                Next
            Next
            'End If
            'Next
        End If
        Return ReportMaintenanceDetails
    End Function

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

    Public Sub SetTypeCombo()

        If mServiceTypeList Is Nothing Then
            If AppSettings("ClientCode") = "7AR" Then
                mServiceTypeList = PartMonitorServiceTypeList.GetPartMonitorServiceTypeList(, False)
            Else
                mServiceTypeList = PartMonitorServiceTypeList.GetPartMonitorServiceTypeList(, True)
            End If

        End If
        chkListServiceType.DataSource = mServiceTypeList
        Session("mServiceTypeList") = mServiceTypeList

        If mInspectionTypeList Is Nothing Then
            mInspectionTypeList = PartMonitorInspTypeList.GetPartMonitorInspTypeList()    ''ModelMonitorInspTypeList.serach.ExludingRoutineInspections)
        End If
        chkListInspectionType.DataSource = mInspectionTypeList
        Session("mInspectionTypeList") = mInspectionTypeList

        DataBind()
        FillMonitorTypeList()
    End Sub

    Private Sub FillMonitorTypeList()
        chkService.Checked = True
        chkInspection.Checked = True

        For i As Integer = 0 To chkListServiceType.Items.Count - 1
            chkListServiceType.Items(i).Selected = True
        Next

        For i As Integer = 0 To chkListInspectionType.Items.Count - 1
            chkListInspectionType.Items(i).Selected = True
        Next

    End Sub

    Private Sub ControlVisibilityForDetails()
        cmbAssembly.Enabled = False
        cmbComponent.Enabled = False
        cmbSerialNo.Enabled = False
        txtAsOnDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
        upnlDetails.Update()
    End Sub

#End Region

#Region "DataFieldBind"

    Public Sub SetComboOfMachine(AsODate As String)
        mMachineNameValueList = MachineNameValueList.GetMachineList(AsODate, , , , , , , True, "(Select)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataBind()
    End Sub

#End Region

#Region "Events"

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack Then
            Session("MiddleFrame") = "wfSearchCriteriaForComp_Ajax.aspx?"
            ResetValues()
            ControlVisibilityForDetails()
            SetTypeCombo()
            SetComboOfMachine(txtAsOnDate.Text.Trim)
            'Added By Vikrant On 14-Feb-2014 For ALL13022014-1
            DataFieldBind()
            'End
            'Added By Vikrant On 14-March-2014 For All14032014
            If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Then
                txtBottomLine.Text = "I hereby certify that the data specified above has been certified throughout : 									Technical Support Division: __________________ Date: _____________"
            ElseIf AppSettings("ClientCode") = "Novo" Then 'Added By Saylee on 29-Jan-2018 for NOVO29012018
                txtBottomLine.Text = ""
            ElseIf AppSettings("ClientCode") = "APFT" Or
                   AppSettings("ClientCode") = "AAP" Then 'Added By Saylee On 1-Oct-2018 
                txtBottomLine.Text = "I hereby certify that the data specified above has been verified throughout. Continuing Airworthiness Manager: __________________ Date: _____________"
            Else
                If cmbFormat.SelectedIndex = 0 Then
                    txtBottomLine.Text = "I hereby certify that the data specified above has been verified throughout : 									Planning Manager: __________________ License No.: __________ Date: _____________"
                ElseIf cmbFormat.SelectedIndex = 1 Then
                    txtBottomLine.Text = "I hereby certify that the data specified above has been certified throughout : 									Engineering Department Manager : ____________________   Date : __________"
                End If

            End If
            'End

            If (AppSettings("ClientCode") = "BSA" Or AppSettings("ClientCode") = "MID") Then
                cmbFormat.Items.Add("Format 3 (Sort By Position )")

            End If
        End If
        SetSession()
    End Sub

    Private Sub MSGBoxCtrl_UserControlEvent(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MessageBoxResult()
    End Sub

    Private Sub AircraftChanged(sender As Object, e As EventArgs) Handles cmbAircraft.SelectedIndexChanged
        If cmbAircraft.SelectedIndex = 0 Then
            cmbAssembly.Enabled = False
            cmbAssembly.ClearSelection()
            AircraftIndex = cmbAircraft.SelectedIndex
            Session("AircraftIndex") = AircraftIndex

            cmbComponent.Enabled = False

            cmbComponent.ClearSelection()

            cmbSerialNo.Enabled = False
            'cmbSerialNo.SelectedIndex = 0
            cmbSerialNo.ClearSelection()
        Else
            cmbAssembly.Enabled = True
            MachineName = cmbAircraft.SelectedValue.ToString
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, txtAsOnDate.Text.Trim, "(All)", True)
            cmbAssembly.DataSource = mAssemblylist
            Session("mAssemblylist") = mAssemblylist
            cmbAssembly.DataBind()
            AircraftIndex = cmbAircraft.SelectedIndex
            Session("AircraftIndex") = AircraftIndex
        End If

        cmbComponent.Enabled = False
        cmbComponent.ClearSelection()
        cmbSerialNo.Enabled = False
        'cmbSerialNo.SelectedIndex = 0
        cmbSerialNo.ClearSelection()
        If cmbAircraft.Enabled = True Then
            SetFocus(cmbAircraft)
        End If

    End Sub

    Private Sub AssemblyChanged(sender As Object, e As EventArgs) Handles cmbAssembly.SelectedIndexChanged
        If cmbAssembly.SelectedIndex = 0 Then
            cmbComponent.Enabled = False
            AssemblyIndex = cmbAssembly.SelectedIndex
            Session("AssemblyIndex") = AssemblyIndex
            cmbComponent.ClearSelection()
        Else
            mAssemblylist = Session("mAssemblylist")
            cmbComponent.Enabled = True
            AssemblyName = cmbAssembly.SelectedValue.ToString
            mPartListForCombo = PartListForCombo.GetPartListForCombo(mAssemblylist(New Guid(cmbAssembly.SelectedValue.ToString)).ModelID, mAssemblylist(New Guid(cmbAssembly.SelectedValue.ToString)).SerialNo, , , "(All)")
            cmbComponent.DataSource = mPartListForCombo
            Session("mPartListForCombo") = mPartListForCombo
            cmbComponent.DataBind()
            AssemblyIndex = cmbAssembly.SelectedIndex
            Session("AssemblyIndex") = AssemblyIndex
        End If
        cmbSerialNo.Enabled = False
        'cmbSerialNo.SelectedIndex = 0
        cmbSerialNo.ClearSelection()
    End Sub

    Private Sub ComponentChanged(sender As Object, e As EventArgs) Handles cmbComponent.SelectedIndexChanged
        If cmbComponent.SelectedIndex = 0 Then
            cmbSerialNo.Enabled = False
            'cmbSerialNo.SelectedIndex = 0
            cmbSerialNo.ClearSelection()
            ComponentIndex = cmbComponent.SelectedIndex
            Session("ComponentIndex") = ComponentIndex
        Else
            mPartListForCombo = Session("mPartListForCombo")
            mAssemblylist = Session("mAssemblylist")
            cmbSerialNo.Enabled = True
            ComponentName = cmbComponent.SelectedValue.ToString
            mPartListForSerialNos = PartListForSerialNos.GetPartListForSerialNosList(mPartListForCombo(New Guid(cmbComponent.SelectedValue.ToString)).Name, "", txtAsOnDate.Text, mAssemblylist(New Guid(cmbAssembly.SelectedValue.ToString)).AssemblyTypeID, "(All)")
            If mPartListForSerialNos.Count > 1 Then
                If Not mPartListForSerialNos(1).SerialNo = "" Then
                    cmbSerialNo.DataSource = mPartListForSerialNos
                    cmbSerialNo.DataBind()
                Else
                    cmbSerialNo.Items.Clear()
                    cmbSerialNo.Items.Add("(All)")
                    cmbSerialNo.DataBind()
                End If
            Else
                cmbSerialNo.Items.Clear()
                cmbSerialNo.Items.Add("(All)")
                cmbSerialNo.DataBind()
            End If
            Session("mPartListForSerialNos") = mPartListForSerialNos
            ComponentIndex = cmbComponent.SelectedIndex
            Session("ComponentIndex") = ComponentIndex
        End If
    End Sub

    Private Sub Close(sender As Object, e As EventArgs) Handles btnClose.Click
        mMachineList = Nothing
        mAssemblylist = Nothing
        mServiceTypeList = Nothing
        mInspectionTypeList = Nothing
        mMachineNameValueList = Nothing
        mPerDayLimits = Nothing  'Added By Vikrant On 13-Feb-2014 For ALL13022014-1
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub

#End Region

#Region "Reports"

    Public Sub SetReport(Optional ByMail As Boolean = False)    'Parameter Added by Shital on 14-Sep-2016

        Try
            ReportMaintenanceDetails = New ReportMaintenanceDetailList
            ReportStatusList = New rptStatusList

            Dim da As New ObjectAdapter
            Dim ds As New dsReportMaintenanceDetail
            Dim rptCompStatus As New Engine.ReportClass 'crCompStatus 'Changed By Utkarsh on 07-Apr-2011
            Dim rptSerializedComp As New Engine.ReportClass 'Added by Prashant on 30-June-2010
            Dim rptHardTimeStatus As New Engine.ReportClass
            Dim RptCofA As New crCofALandscapeForm 'U1
            Dim RptCofAPortrait As New crCofAPortraitForm 'U2
            Dim mCompanyDetail As New CompanyDetail
            Dim OperatorName As String = ""

            'Added by Ajay 14-08-2023
            If cmbAircraft.SelectedIndex > 0 Then
                mLastAMPRef = LastMPDAMPRef.GetLastMPDAMPRefForMachine(MachineID:=New Guid(cmbAircraft.SelectedValue.ToLower))
                Session("mLastAMPRef") = mLastAMPRef
                If (mLastAMPRef.AMPNo <> "") Then AMPNo = "AMP No.: " + mLastAMPRef.AMPNo + ",Rev No.: " + mLastAMPRef.RevNo + ",Dated: " + mLastAMPRef.FromDateFormatted
            Else
                AMPNo = ""
            End If

            If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                If AppSettings("ClientCode") = "7AR" Then
                    rptHardTimeStatus = New crAircraftHTCompStFt2ForTaskNo7Air
                Else
                    rptHardTimeStatus = New crAircraftHTCompStFt2ForTaskNo
                End If
            ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "DOL") Then
                If cmbFormat.SelectedIndex = 0 Then
                    rptHardTimeStatus = New crAircraftHardTimeCompStatusDol 'DDD
                ElseIf cmbFormat.SelectedIndex = 1 Then
                    rptHardTimeStatus = New crAircraftHTCompStFt2 '4DDD
                End If
            ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso ((AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet")) Then
                If cmbFormat.SelectedIndex = 0 Then
                    rptHardTimeStatus = New crAircraftHardTimeCompStatusTAAL ''Newly added report'DDD
                ElseIf cmbFormat.SelectedIndex = 1 Then
                    rptHardTimeStatus = New crAircraftHTCompStFt2 '6DDD
                End If
                'Added By Utkarsh On 07-Apr-2011
            ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
                If cmbFormat.SelectedIndex = 0 Then
                    rptHardTimeStatus = New crAircraftHardTimeCompStatusForDeccan '7DDD
                ElseIf cmbFormat.SelectedIndex = 1 Then
                    rptHardTimeStatus = New crAircraftHTCompStFt2ForDeccan '8DDD
                End If
                '*******************************
            ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso
                   (AppSettings("ClientCode") = "APFT" Or
                    AppSettings("ClientCode") = "BAMS" Or
                    AppSettings("ClientCode") = "AAP") Then 'Added by Saylee on 27-Nov-2017

                'BAMS Code added by Saylee to change format to show DoneON details on report
                If cmbFormat.SelectedIndex = 0 Then
                    rptHardTimeStatus = New crAircraftHardTimeCompStatusAPFT  ''Newly added report'DDD
                ElseIf cmbFormat.SelectedIndex = 1 Then
                    rptHardTimeStatus = New crAircraftHTCompStFt2 '6DDD
                End If
            ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso ((AppSettings("ClientCode") = "STR") Or AppSettings("ClientCode") = "CMX") Then 'Added by Saylee on 12-Apr-2018 for StarAir12042018
                If cmbFormat.SelectedIndex = 0 Then
                    If optOCStatus.Checked = True Or optNavCompStatus.Checked Then
                        rptHardTimeStatus = New crAircraftOCCompStatusStarAir
                    Else
                        rptHardTimeStatus = New crAircraftHardTimeCompStatusStarAir
                    End If
                ElseIf cmbFormat.SelectedIndex = 1 Then
                    rptHardTimeStatus = New crAircraftHardTimeCompStatusFt3 '6DDD 'Common Report
                End If
            Else
                If cmbFormat.SelectedIndex = 0 Then
                    rptHardTimeStatus = New crAircraftHardTimeCompStatus '9DDD
                ElseIf cmbFormat.SelectedIndex = 1 Then
                    If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Then
                        rptHardTimeStatus = New crAircraftHTCompStFt2ForBA '10DDD
                    Else
                        rptHardTimeStatus = New crAircraftHTCompStFt2 '10DDD
                    End If
                ElseIf (AppSettings("ClientCode") = "BSA" Or AppSettings("ClientCode") = "MID") And cmbFormat.SelectedIndex = 2 Then
                    rptHardTimeStatus = New crAircraftHardTimeCompStatusFt3 'Used to show Order by Position '11DDD
                End If
            End If
            SetPerDayLimitValues()
            SetValues()
            'ReportDetail()
            If optHardTimeStatus.Checked = True Then
                If IsSerSelect = False And IsInsSelect = False Then
                    MSGBoxCtrl.show(MSGBox.Message_title.SelectAtleastOne, MSGBox.Message_text.SelectAtleastOne, "Please at-least one Maintenance Type", MsgBoxStyle.OkOnly, "")
                    Exit Sub

                Else
                    ReportDetail()
                    If AssemblyType = "(All)" Then      'Modified By Harsh on 21st Feb 2024
                        If AppSettings("ClientCode") = "7AR" Then
                            ReportLabel = "Hard Time Status Report"
                        Else
                            ReportLabel = "Hard Time Status of Components"
                        End If
                    Else
                        If AppSettings("ClientCode") = "7AR" Then
                            If AssemblyType = "Airframe" Then
                                ReportLabel = "Hard Time Status Report"
                            Else
                                ReportLabel = AssemblyType + " LLP's Status Report"
                            End If
                        Else
                            ReportLabel = AssemblyType + " Hard Time Status of Components"
                        End If

                    End If
                End If
            ElseIf optCompStatus.Checked = True Then
                ReportDetailForAllComponents()
                ReportLabel = "Component and Inspection Status"
            ElseIf optSerializedComp.Checked = True Then  'Added new Report by Saylee on 29-June-2010
                ReportDetailForSerializedComponents() 'Added new Report by Saylee on 29-June-2010
                ReportLabel = "Serialized Component Status"
            ElseIf optOCStatus.Checked = True Or optNavCompStatus.Checked Then
                ReportDetailForOCComponents()
                If optOCStatus.Checked Then
                    ReportLabel = "OC. Component Status"
                Else
                    ReportLabel = "Navigation Component Status"
                End If

            End If

            'Added by Prashant on 11-Aug-2011
            If (AppSettings("ClientCode") IsNot Nothing) AndAlso ((AppSettings("ClientCode") = "Indamer")) Then
                Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue))
                If mMachineOperatorName.OperatorName <> "" Then OperatorName = mMachineOperatorName.OperatorName
            ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
                OperatorName = searchstr7
            End If

            'SetPerDayLimitValues()


            If IsSerSelect And Not optOCStatus.Checked And Not optNavCompStatus.Checked Then 'Not optOCStatus.Checked bcauz ServicesShortName set in ReportDetailForOCComponents

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
                            ServicesShortName = IIf(Not mServiceTypeList(i, "").CodeType Is Nothing, mServiceTypeList(i, "").CodeType, "")
                        Else
                            ServicesShortName = ServicesShortName + IIf(Not mServiceTypeList(i, "").CodeType Is Nothing, "<br>" + mServiceTypeList(i, "").CodeType, "")
                        End If

                    Next
                End If
            End If
            Dim InspsShortName As String = ""
            If AppSettings("ShowMaintenanceForNewClients") = "True" Then 'Added By Prashant on 27-Jul-2023
                InspsShortName = "" 'To hide inspection Legends : selction  set it as blank
            Else
                If IsInsSelect Then
                    For i As Integer = 0 To mInspectionTypeList.Count - 1
                        If InspsShortName = "" Then
                            InspsShortName = IIf(Not mInspectionTypeList(i, "").CodeType Is Nothing, mInspectionTypeList(i, "").CodeType, "")
                        Else
                            InspsShortName = InspsShortName + IIf(Not mInspectionTypeList(i, "").CodeType Is Nothing, "<br>" + mInspectionTypeList(i, "").CodeType, "")
                        End If
                    Next
                End If
            End If

            'Added by Saylee on 140Jun-2022, to show all periods as legends on report instead of hard fix
            Dim mPeriodNames As String
            Dim tmpPeriodUnitList As PeriodUnitList = PeriodUnitList.GetPeriodUnitList()
            For i As Integer = 0 To tmpPeriodUnitList.Count - 1
                If mPeriodNames = "" Then
                    mPeriodNames = tmpPeriodUnitList(i).Code + " : " + tmpPeriodUnitList(i).PeriodUnitName
                Else
                    mPeriodNames = mPeriodNames + "<br>" + tmpPeriodUnitList(i).Code + " : " + tmpPeriodUnitList(i).PeriodUnitName
                End If
            Next

            Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, mCompanyDetail.Tel1,
                                         mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
                                         mCompanyDetail.WebSite, ReportLabel, txtAsOnDate.Text.Trim, searchstr2, txtCMPRef.Text,
                                         "", txtBottomLine.Text, AppSettings("Product Version"),
                                         AppSettings("SINote"), "", OperatorName,
                                         IIf(rdbAirframeDue.Checked, "Next Due (Airframe Values)", "Next Due"),
                                         IIf(rdbAirframeDue.Checked, "Removal Planning (A/C Value)", "Removal Planning (Assembly Val.)"),
                                         AppSettings("Logo"), AppSettings("ClientCode"),
                                         ServicesShortName, InspsShortName,
                                         SearchStr16:=AppSettings("FormNo"), SearchStr17:=AMPNo, SearchStr18:=mPeriodNames)  'Changed by Utkarsh On 7-Apr-2011

            If ByMail = False Then
                If ReportMaintenanceDetails.Count = 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 726)
                End If
            End If


            If (ByMail = True And ReportMaintenanceDetails.Count <= 0) Then
                SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, ReportLabel, ReportLabel, "There is no record for this search criteria.", "",
                Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"),
                ReportGeneratedBy:=Session("ReportGenratedBy"),
                SmtpHost:=mModuleList.Item("ComponantCurrentStatus").SmtpHost, SmtpPort:=mModuleList.Item("ComponantCurrentStatus").SmtpPort,
                SmtpUser:=mModuleList.Item("ComponantCurrentStatus").SmtpUser, SmtpPassword:=mModuleList.Item("ComponantCurrentStatus").SmtpPassword)
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
            Try
                If optHardTimeStatus.Checked Or optOCStatus.Checked Or optNavCompStatus.Checked Then
                    rptHardTimeStatus.SetDataSource(ds)
                    Session("CrystalReport") = rptHardTimeStatus
                ElseIf optCompStatus.Checked = True Then
                    optCompStatus.Checked = True
                    'Added By Utkarsh On 07-Apr-2011
                    If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
                        rptCompStatus = New crCompStatusForDeccan '11DDD
                    ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "7AR") Then ' 7AR Code added by Saylee on 27-Sep-2024
                        rptCompStatus = New crCompStatus7AR
                    Else
                        rptCompStatus = New crCompStatus '12DDD
                    End If
                    '**********************************
                    rptCompStatus.SetDataSource(ds)
                    Session("CrystalReport") = rptCompStatus
                ElseIf optSerializedComp.Checked = True Then 'Added new Report by Saylee on 29-June-2010
                    optSerializedComp.Checked = True
                    'Added By Utkarsh On 07-Apr-2011
                    If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
                        If cmbSortBy.SelectedValue = 0 Then
                            rptSerializedComp = New crSerializedCompStatusByPartNoForDeccan '13DDD
                        Else
                            rptSerializedComp = New crSerializedCompStatusForDeccan '14DDD
                        End If
                        rptSerializedComp.SetDataSource(ds)
                        Session("CrystalReport") = rptSerializedComp
                        '****************************
                    Else
                        If cmbSortBy.SelectedValue = 0 Then      'Added by Prashant on 30-June-2010
                            rptSerializedComp = New crSerializedCompStatusByPartNo '15DDD
                        Else
                            rptSerializedComp = New crSerializedCompStatus '16DDD
                        End If
                        rptSerializedComp.SetDataSource(ds)
                        Session("CrystalReport") = rptSerializedComp
                    End If
                End If


                'added by shital on 14-Sep-2016
                If (ByMail = True) Then
                    SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, ReportLabel, ReportLabel, " For " + lblAircraft1.Text, ,
                                              Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"),
                                              ReportGeneratedBy:=Session("ReportGenratedBy"),
                    SmtpHost:=mModuleList.Item("ComponantCurrentStatus").SmtpHost, SmtpPort:=mModuleList.Item("ComponantCurrentStatus").SmtpPort,
                    SmtpUser:=mModuleList.Item("ComponantCurrentStatus").SmtpUser, SmtpPassword:=mModuleList.Item("ComponantCurrentStatus").SmtpPassword)
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
                    MarkLog(Action:=Action.Print,
                            ModuleName:="ComponentCurrentStatus",
                            Detail:=EventLogDetail + " " + ReportLabel,
                            ErrorType:=ErrorType.NoError,
                            TransID:=Guid.Empty, EventLogID)
                End If

                ReportMaintenanceDetails = Nothing
                Report = Nothing
                ReportStatusList = Nothing

            Catch ex As Exception
                Throw ex
            End Try
        Catch ex As Exception
            ex.GetBaseException()
        End Try

    End Sub

    Private Sub SearchCriteria(sender As Object, e As EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
    End Sub

    Private Sub DisplayReport(sender As Object, e As EventArgs) Handles btnDisplay.Click
        If IsValid = True Then
            IsExcel = False
            ReportMaintenanceDetails = Nothing
            Report = Nothing
            ReportStatusList = Nothing

            If optHardTimeStatus.Checked = True Then
                Report = 1
                Session("Report") = Report
            ElseIf optCompStatus.Checked = True Then
                Report = 0
                Session("Report") = Report
            ElseIf optOCStatus.Checked = True Or optNavCompStatus.Checked Then
                Report = 2
                Session("Report") = Report
            End If

            Dim mTempList As LogList  'Added by Prashant 9-Nov-2010
            mTempList = LogList.GetLogList(New Guid(cmbAircraft.SelectedValue.ToString))    'Added by Prashant 9-Nov-2010
            If mTempList.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.EnterFlightLog, MSGBox.Message_text.EnterFlightLog, "Enter at least one Flight Log for this Aircraft to view this report", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            'SetReport()
            SetReport(False) 'Added by Shital on 14-Sep-2016

        End If

    End Sub

    'Modified by Harsh on 24th April 2024 for FLYPAL-1586 ( FIT Air: Export To Excel Issue for Component Current Status Report )
    Private Sub ExportToExcel(sender As Object, e As EventArgs) Handles btnExport.Click

        Dim PeriodColumnsForExportToExcel As New List(Of String)
        Dim mCompanyDetail As New CompanyDetail
        Dim da As New ObjectAdapter
        Dim ds As New dsReportMaintenanceDetail
        Dim mTempList As LogList  'Added by Prashant 9-Nov-2010
        Dim DueLabel As String = "DueAsOf"
        Dim dsNew As New DataSet

        If IsValid = True Then
            IsExcel = True
            ReportMaintenanceDetails = Nothing
            Report = Nothing
            ReportStatusList = Nothing

            If optHardTimeStatus.Checked = True Then
                Report = 1
                Session("Report") = Report
            ElseIf optCompStatus.Checked = True Then
                Report = 0
                Session("Report") = Report
            ElseIf optOCStatus.Checked = True Or optNavCompStatus.Checked Then
                Report = 2
                Session("Report") = Report
            End If

            mTempList = LogList.GetLogList(New Guid(cmbAircraft.SelectedValue.ToString))    'Added by Prashant 9-Nov-2010
            If mTempList.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.EnterFlightLog, MSGBox.Message_text.EnterFlightLog, "Enter at least one Flight Log for this Aircraft to view this report", MsgBoxStyle.OkOnly, "")
                Exit Sub
            End If

            ReportStatusList = New rptStatusList
            ReportMaintenanceDetails = New ReportMaintenanceDetailList

            SetPerDayLimitValues()
            SetValues()

            'Used for showing searching criteria in Export To Excel
            SearchingCriteria = New ReportData(mCompanyDetail.CompanyName,
                                               mCompanyDetail.Address,
                                               mCompanyDetail.Tel1,
                                               mCompanyDetail.Tel2,
                                               mCompanyDetail.Fax,
                                               mCompanyDetail.Email,
                                               mCompanyDetail.WebSite,
                                               ReportLabel,
                                               txtAsOnDate.Text.Trim,
                                               IIf(Aircraft = "", "ALL", Aircraft),
                                               IIf(Assembly1 = "", "ALL", Assembly1),
                                               "", txtBottomLine.Text,
                                               AppSettings("Product Version"),
                                               AppSettings("SINote"), ,
                                               "", IIf(rdbAirframeDue.Checked, "Next Due (Airframe Values)", "Next Due"),
                                               IIf(rdbAirframeDue.Checked, "Removal Planning (A/C Value)", "Removal Planning (Assembly Value)"),
                                               AppSettings("Logo"))  'Changed by Utkarsh On 7-Apr-2011



            If optHardTimeStatus.Checked = True Then
                If IsSerSelect = False And IsInsSelect = False Then
                    MSGBoxCtrl.show(MessageTitle:=MSGBox.Message_title.SelectAtleastOne,
                                    MessageText:=MSGBox.Message_text.SelectAtleastOne,
                                    ExtraMessage:="Please select at least one Maintenance Type",
                                    ButtonToShow:=MsgBoxStyle.OkOnly,
                                    Sender:="")
                    Exit Sub
                Else
                    ReportDetail()
                    If AssemblyType = "(All)" Then
                        ReportLabel = "Hard Time Status of Components"
                    Else
                        ReportLabel = AssemblyType + " Hard Time Status of Components"
                    End If
                End If
            End If

            If optCompStatus.Checked = True Then
                ReportDetailForAllComponents()
                ReportLabel = "Component and Inspection Status"
            ElseIf optSerializedComp.Checked = True Then
                ReportDetailForSerializedComponents()
                ReportLabel = "Serialized Component Status"
            ElseIf optOCStatus.Checked = True Or optNavCompStatus.Checked Then
                ReportDetailForOCComponents()
                If optOCStatus.Checked Then
                    ReportLabel = "OC. Component Status"
                Else
                    ReportLabel = "Navigation Component Status"
                End If
            End If

            If ReportMaintenanceDetails.Count = 0 Then
                MSGBoxCtrl.show(MessageTitle:=MSGBox.Message_title.NoRecordFound,
                                MessageText:=MSGBox.Message_text.NoRecordFound,
                                ExtraMessage:="There is no record for this search criteria.",
                                ButtonToShow:=MsgBoxStyle.OkOnly,
                                Sender:="")
                Exit Sub
            Else
                RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 726)
            End If

            ds.Clear()

            da.Fill(ds, "ExcelReportMaintenanceDetailList", ReportMaintenanceDetails)
            da.Fill(ds, "ExcelReport", SearchingCriteria)

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
                                                "InstalledAt1",
                                                "InstalledAt2",
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
                                                "MaintenanceActivityType", "HoursFreq", "CyclesFreq", "DaysMnthsYrsValue", "LandingsFreq",
                                                "HoursDoneOnValue", "CyclesDoneOnValue", "DaysMnthsYrsDoneOnValue", "LandingsDoneOnValue",
                                                "Manufacturer", "InstallationWONo", "InstallationRemark", "InstallationDoneBy", "InstPlace",
                                                "TSNHours", "SinceNewDate", "SinceNewLandings", "CSNCycles", "InstCompHours", "InstCompStartDate",
                                                "InstCompLandings", "InstCompCycles", "AssemblyInstHours", "AssemblyInstStartDate",
                                                "AssemblyInstLandings", "AssemblyInstCycles", "PartMonitorCode", "DataColumn1", "ModelEstimatedManHours",
                                                "SourceDoc", "ReqNumber", "DaysMnthsYrsName", "EstDate", "TSOForExcel", "WONoExcel", "InstalledAtForExcel",
                                                "Skill", "IsRII",
                                                "MonitorTypeWithCode", "Description", "LinkedMaintenanceActivityCount", "PartNoSerialNoforExcel", "TSO1ForExcel",
                                                "DoneOnValueForExcel", "SkillID", "ATAChapter", "DueAsOfForExcel", "Frequency", "TSNForExcel", "Freq1",
                                                "RemainingTime", "StatusTypeName", "DescriptionSourceDocForExcel", "TaskNoExcel", "TaskReferenceForExcel"
                                            }


            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns.Remove(columnToRemove(i))
                End If
            Next

            Dim columnsCount As Integer = ds.Tables("ExcelReportMaintenanceDetailList").Columns.Count

            'Added by Harsh on 24th April 2024 for FLYPAL-1586.
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("ATACode").SetOrdinal(0)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("TaskNo").SetOrdinal(1)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("PartNo").SetOrdinal(2)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("PartDesc").SetOrdinal(3)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("CompSerialNo").SetOrdinal(4)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("Position").SetOrdinal(5)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("MonitorTypeCode").SetOrdinal(6)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("InstalledAtDate").SetOrdinal(7)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("TSO1").SetOrdinal(8)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("InstalledAt").SetOrdinal(9)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("TSN").SetOrdinal(10)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("ElapsedTime").SetOrdinal(11)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("Freq1ForExcel").SetOrdinal(12)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("DoneOnDate").SetOrdinal(13)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("TSO").SetOrdinal(15)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("RemainingTimeForExcel").SetOrdinal(16)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("DueAsof").SetOrdinal(17)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("RemoveAtDate").SetOrdinal(18)
            ds.Tables("ExcelReportMaintenanceDetailList").Columns("Remark").SetOrdinal(19)

            For i As Integer = 0 To ds.Tables("ExcelReportMaintenanceDetailList").Columns.Count - 1

                If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "ModificationNumber" Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Directive No"
                End If

                If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "MonitorTypeCode" Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Monitoring Type"
                End If

                If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "DoneOnValue" Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Last Carried Out"
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns("Last Carried Out").SetOrdinal(14)
                End If

                'Added by Harsh on 24th April 2024 for FLYPAL-1586.
                If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "ATACode" Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "ATA"
                End If

                If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "TaskNo" Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Task No."
                End If

                If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "PartNo" Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Part No."
                End If

                If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "PartDesc" Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Description"
                End If

                If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "CompSerialNo" Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Serial No."
                End If

                If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Position" Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Pos"
                End If

                If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "DoneOnDate" Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Done On Date"
                End If

                If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "InstalledAt" Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Component Installation Values"
                End If

                If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "TSO1" Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Assembly Installation Values"
                End If

                If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "InstalledAtDate" Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Installation Date"
                End If

                If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "TSN" Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Current Values"
                End If

                If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Freq1ForExcel" Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "TBO / LIMIT"
                End If

                If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "DueAsof" Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Removal Values"
                End If

                If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "RemoveAtDate" Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Removal Date"
                End If

                If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "RemainingTimeForExcel" Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Remaining"
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

            Dim dataView As DataView = ds.Tables("ExcelReportMaintenanceDetailList").DefaultView
            dataView.Sort = "ATA"

            dsNew.Clear()

            dsNew.Merge(ds.Tables("ExcelReport"))
            dsNew.Merge(dataView.ToTable())

            dsNew.Tables("ExcelReport").TableName = "Searching Criteria"
            dsNew.Tables("ExcelReportMaintenanceDetailList").TableName = ReportLabel
			Session("ExcelFileName") = ReportLabel.Replace("/", " ")
			PeriodColumnsForExportToExcel.AddRange(New String() {"Frequency", "ElapsedTime", "RemainingTime", "DueAsof", "Last Carried Out"})
			Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
            Session("dsNew") = dsNew
            ScriptManager.RegisterStartupScript(page:=Me,
                                                type:=[GetType],
                                                key:="openFile",
                                                script:="openFile();",
                                                addScriptTags:=True)
            'Added by Prashant on 19-Jan-2021
            MarkLog(Action:=Action.Print,
                    ModuleName:="ComponentCurrentStatus",
                    Detail:="Export To Excel " + EventLogDetail + " " + ReportLabel,
                    ErrorType:=ErrorType.NoError,
                    TransID:=Guid.Empty, EventLogID)

        End If

    End Sub

#End Region

#Region "Mail Functionality"

    'Added by Shital on 14-Sep-2016
    Private Sub SendMail(sender As Object, e As EventArgs) Handles btnByMail.Click

        If optHardTimeStatus.Checked = True Then
            Report = 1
            Session("Report") = Report
        ElseIf optCompStatus.Checked = True Then
            Report = 0
            Session("Report") = Report
        ElseIf optOCStatus.Checked = True Or optNavCompStatus.Checked Then
            Report = 2
            Session("Report") = Report
        End If
        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        ' Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail

        Session("UserEmailID") = mModuleList.Item("ComponantCurrentStatus").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("ComponantCurrentStatus").SendCCMailID
        '--------------------------
        Dim Str As String
        Str = "OpenByMaiWindow();"
        ScriptManager.RegisterStartupScript(page:=Me,
                                            type:=[GetType],
                                            key:="OpenByMaiWindow",
                                            script:=Str,
                                            addScriptTags:=True)

    End Sub

    Private Sub HdnBtnSendMail(sender As Object, e As EventArgs) Handles hdnimgLogBtnSendMail.Click
        Dim email As Thread

        Try
            Report = Session("Report")
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

#End Region

End Class