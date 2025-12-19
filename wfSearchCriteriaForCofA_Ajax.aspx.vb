'Added By Prashant

Imports System.Collections.Generic
Imports System.Text

Public Class wfSearchCriteriaForCofA_Ajax
    Inherits System.Web.UI.Page

#Region " Enumeration "
    Enum Open
        CofAReport = 1
        RoutineInspectionReport = 2
        ModificationReport = 3
        ServiceReport = 5 'APFT
    End Enum
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

#Region " Variable Declaration "
    Dim ReportMaintenanceDetails As New ReportMaintenanceDetailList
    Dim mReportMaintenanceDetail As New ReportMaintenanceDetail
    Dim mAssemblyList As AssemblyList
    Dim mServiceTypeList As PartMonitorServiceTypeList
    Dim mInspectionTypeList As ModelMonitorInspTypeList
    Dim mModificationTypeList As ModelMonitorModTypeList
    Dim ReportStatusList As New rptStatusList
    Dim mMachineList As MachineList
    Public mOpen As Open
    Dim mTypeListForCofA As TypeListForCofA
    Dim SofAIndex As Integer
    Dim InspIndex As Integer
    Dim SerIndex As Integer
    Dim ModIndex As Integer
    Dim AircraftIndex As Integer
    Dim TypeCount As Boolean = False
    Dim Check As Boolean = False
    Dim ReportLabel As String
    Dim Aircraft As String
    Dim Assembly1 As String
    Dim ReportType As String
    Dim ServiceType As String
    Dim InspectionType As String
    Dim ModificationType As String
    Dim AOdate As String
    Dim AOnDate As String
    Dim ReportStatus As Integer = 1
    Dim Report As ReportData
    Dim ShowCofA As Boolean = False
    Dim AsonDate As String = ""
    Dim IsSerSelect As Boolean = False
    Dim IsModSelect As Boolean = False
    Dim IsInsSelect As Boolean = False
    Dim ServiceTypeID(50) As Integer
    Dim InspectionTypeID(50) As Integer
    Dim ModificationTypeID(50) As Integer
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
    Dim AMPNoStr As String = ""
    Dim mLastAMPRef As LastMPDAMPRef
    Dim Zone As String 'Added By Prashant 19-Apr-2024
    Dim ModelEstimatedManHours As String 'Added By Prashant 19-Apr-2024
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineList = CType(Session("mMachineList"), MachineList)
        mAssemblyList = CType(Session("mAssemblyList"), AssemblyList)
        mServiceTypeList = CType(Session("mServiceTypeList"), PartMonitorServiceTypeList)
        mInspectionTypeList = CType(Session("mInspectionTypeList"), ModelMonitorInspTypeList)
        mModificationTypeList = CType(Session("mModificationTypeList"), ModelMonitorModTypeList)
        mTypeListForCofA = CType(Session("mTypeListForCofA"), TypeListForCofA)
        mOpen = CType(Session("mOpen"), Open)
        AOnDate = Session("AOnDate")
        TypeCount = Session("TypeCount")
        Check = Session("Check")
        AircraftIndex = Session("AircraftIndex")
        SerIndex = Session("SerIndex")
        ModIndex = Session("ModIndex")
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
        Session("mModificationTypeList") = mModificationTypeList
        Session("mTypeListForCofA") = mTypeListForCofA
        Session("mOpen") = mOpen
        Session("AOnDate") = AOnDate
        Session("TypeCount") = TypeCount
        Session("Check") = Check
        Session("AircraftIndex") = AircraftIndex
        Session("SerIndex") = SerIndex
        Session("InspIndex") = InspIndex
        Session("ModIndex") = ModIndex
        Session("SofAIndex") = SofAIndex
        Session("ReportStatus") = ReportStatus
        Session("ShowCofA") = ShowCofA
        Session("mATAList") = mATAList 'Added by Saylee on 20-Apr-2010
        Session("mPerDayLimits") = mPerDayLimits 'Added By Utkarsh ON 12-Sep-2013 FOR BA11092013
    End Sub
    Public Sub ControlVisibility()
        ListServiceType.Enabled = False
        ListDirectiveType.Enabled = False
        ListInspectionType.Enabled = False
        ListServiceType.Visible = True
        ListDirectiveType.Visible = True
        ListInspectionType.Visible = True
        'Ajay 10-Nov-2022
        If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "CofA") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav();", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
        End If
        '--------------------------
    End Sub
    Private Sub EnabledDisabledButons()
        Select Case mOpen
            Case Open.CofAReport
                'PpnlModificationType.Visible = False
                'lblStepFormat.Text = "Step VII.  Select Format of Report" 'Added By Vikrant On 30-APr-2014 For ALL30042014-1
                lblCMPRefHeader.Visible = False
                lblCMPREfLine.Visible = False
                txtCMPRef.Visible = False
                chkTaskCard.Visible = IIf(cmbFormat.SelectedIndex = 0, True, False)
            Case Open.RoutineInspectionReport, Open.ServiceReport
                'PpnlServiceType.Visible = False
                'PpnlModificationType.Visible = False
                'Added By Vikrant On 30-APr-2014 For ALL30042014-1
                chkTaskCard.Visible = IIf(cmbFormat.SelectedIndex = 0, True, False)

                If cmbFormat.SelectedIndex = 2 Then
                    txtBottomLine.Visible = True
                    Label2.Visible = True
                    Label3.Visible = True
                    'lblStepFormat.Text = "Step VIII.  Select Format of Report"
                Else
                    If AppSettings("ShowMaintenanceForNewClients") = "True" And mOpen = Open.ServiceReport Then
                        txtBottomLine.Visible = True
                        Label2.Visible = True
                        Label3.Visible = True
                        'lblStepFormat.Text = "Step VI.  Select Format of Report"
                    Else
                        txtBottomLine.Visible = False
                        Label2.Visible = False
                        Label3.Visible = False
                        'lblStepFormat.Text = "Step VII.  Select Format of Report"
                    End If


                End If
                'End
                lblCMPRefHeader.Visible = True
                lblCMPREfLine.Visible = True
                txtCMPRef.Visible = True

            Case Open.ModificationReport
                chkTaskCard.Visible = False
                'PpnlServiceType.Visible = False
                'PpnlInspectionType.Visible = False
                lblCMPRefHeader.Visible = False
                lblCMPREfLine.Visible = False
                txtCMPRef.Visible = False
        End Select
    End Sub
    Private Sub ClearAll()
        mOpen = Session("mOpen")
        If Session("MiddleFrame") <> "wfSearchCriteriaForCofA_Ajax.aspx?Open=" & mOpen Then
            Session.Remove("mMachineList")
            Session.Remove("mAssemblyList")
            Session.Remove("mServiceTypeList")
            Session.Remove("mInspectionTypeList")
            Session.Remove("mModificationTypeList")
            Session.Remove("AOnDate")
            Session.Remove("TypeCount")
            Session.Remove("Check")
            Session.Remove("AircraftIndex")
            Session.Remove("SerIndex")
            Session.Remove("InspIndex")
            Session.Remove("ModIndex")
            Session.Remove("SofAIndex")
            Session.Remove("Report")
            Session.Remove("mATAList")  'Added by Saylee on 20-Apr-2010
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
                'SearchStr4 = ""
            Else
                AssemblyType = mAssemblyList(cmbAssembly.SelectedIndex).AssemblyType
                AssemblyName = cmbAssembly.SelectedValue.ToString
                Assembly1 = cmbAssembly.SelectedItem.Text
                lblAssembly1.Text = "Assembly Name : " & Assembly1  'Added Code
                'Dim i As Integer = 0            'Added By Prashant 27-May-2013 ALL27052013
                'For i = 0 To mAssemblyList.Count - 1
                '    If mAssemblyList(i).AssemblyTypeID = 1 Then
                '        SearchStr4 = mAssemblyList(i).RevisionNo
                '    End If
                'Next
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

                If cmbType.Items(x).Selected = True And (cmbType.Items(x).Text = "Modification" Or cmbType.Items(x).Text = "Directive") Then
                    IsModSelect = True

                    For K As Integer = 0 To ListDirectiveType.Items.Count - 1
                        If ListDirectiveType.Items.Item(K).Selected Then
                            ModificationTypeID(K) = ListDirectiveType.Items.Item(K).Value
                            ModificationType = ModificationType + ", " + ListDirectiveType.Items.Item(K).Text
                        End If
                    Next
                End If

                If cmbType.Items.Item(x).ToString = "All" Then
                    IsSerSelect = True
                    IsInsSelect = True
                    IsModSelect = True
                    ServiceTypeID(0) = 0
                    InspectionTypeID(0) = 0
                    ModificationTypeID(0) = 0
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
        'Added By Utkarsh ON 12-Sep-2013 FOR BA11092013
        If (mOpen = 2 Or mOpen = Open.ServiceReport) AndAlso cmbFormat.SelectedValue = 2 Then 'APFT
            SetGridObject()
            mByPerDayLimit = True
            mIsAverageRequired = True
        Else
            mPerDayLimits = Nothing
            mByPerDayLimit = False
            mIsAverageRequired = False
        End If
        'End
        mCofASearchingCriteria = lblDateRange.Text + ", " + lblAircraft1.Text + ", " + lblAssembly1.Text + ", " + lblATAChapter1.Text + ", Format : " + cmbFormat.SelectedItem.Text.Trim + ServiceType + InspectionType + ModificationType + PeriodLimt
        SearchStr4 = txtCMPRef.Text
    End Sub
    Public Sub SetTypeCombo()
        mOpen = Session("mOpen")
        mTypeListForCofA = TypeListForCofA.GetTypeListForCofA(CType(mOpen, TypeListForCofA.Open), "")
        cmbType.DataSource = mTypeListForCofA
        cmbType.DataBind()
        Select Case mOpen
            Case Open.CofAReport
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

            Case Open.RoutineInspectionReport
                mInspectionTypeList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList(ModelMonitorInspTypeList.serach.AllInspections)
                ListInspectionType.DataSource = mInspectionTypeList
                Session("mInspectionTypeList") = mInspectionTypeList

            Case Open.ModificationReport
                mModificationTypeList = ModelMonitorModTypeList.GetModelMonitorModTypeList()
                ListDirectiveType.DataSource = mModificationTypeList
                Session("mModificationTypeList") = mModificationTypeList

                'APFT
            Case Open.ServiceReport
                mServiceTypeList = PartMonitorServiceTypeList.GetPartMonitorServiceTypeList()
                ListServiceType.DataSource = mServiceTypeList
                Session("mServiceTypeList") = mServiceTypeList
        End Select
        upnType.Update()

        DataBind()
        upnlServiceType.Update()
        upnlModificationType.Update()
        upnlInspectionType.Update()
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

            'ListDirectiveType Enabled
            If cmbType.Items(j).Selected = True And (cmbType.Items(j).Text = "Modification" Or cmbType.Items(j).Text = "Directive") Then
                ListDirectiveType.Enabled = cmbType.Items.Item(cmbType.SelectedIndex).Selected
                For i As Integer = 0 To ListDirectiveType.Items.Count - 1
                    ListDirectiveType.Items.Item(i).Selected = ListDirectiveType.Enabled
                Next
                hdnDirective.Value = cmbType.Items.Item(cmbType.SelectedIndex).Selected
                upnlModificationType.Update()
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

            'cmbModification Disabled
            If cmbType.Items(k).Selected = False And (cmbType.Items(k).Text = "Modification" Or cmbType.Items(k).Text = "Directive") Then
                ListDirectiveType.Enabled = False
                For l As Integer = 0 To ListDirectiveType.Items.Count - 1
                    ListDirectiveType.Items.Item(l).Selected = ListDirectiveType.Enabled = False
                Next
                hdnDirective.Value = cmbType.Items.Item(cmbType.SelectedIndex).Selected
                upnlModificationType.Update()
            End If
        Next
        upnlImgBtn.Update()
        ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "disableEnable", "disableEnable();", True)
    End Sub

    Public Function ReportDetail() As ReportMaintenanceDetailList
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

        'Added By Utkarsh ON 12-Sep-2013 FOR BA11092013  parameters (ByPerdayLimit,PerDayLimits ,IsAverageRequired)
        mMachineList = MachineList.GetMachineListMonitoringStatus(New SmartDate(AsonDate).Text, MachineName, , , , , , , , , , , True, , AssemblyName, IsAverageRequired:=mIsAverageRequired, ByPerDayLimit:=mByPerDayLimit, PerdayLimits:=mPerDayLimits, SkipIsForInventoryAircarft:=True)
        Dim LHLabel2 As String = ""
        Dim LHData2 As String = ""

        'Added by Saylee on 25-May-2016
        If (AppSettings("ClientCode") = "RAL" And cmbFormat.SelectedIndex = 2) Then
            If Not cmbAircraft.SelectedItem.ToString = "(All)" Then
                mtmpMachineList = tmpMachineList.GetMachineList(, Aircraft, , , , , True, AsonDate)
                Dim mOtherPeriodExists As String = "False"
                Dim mOtherPeriods As String = String.Empty
                For i As Integer = 0 To mtmpMachineList.Count - 1
                    mOtherPeriods = CType(IIf(mtmpMachineList(i).RINS = "", "", mtmpMachineList(i).RINS & "(RI)" & vbCrLf), String) _
                                 + vbCrLf + CType(IIf(mtmpMachineList(i).NGCycles = "", "", mtmpMachineList(i).NGCycles & "(NG)" & vbCrLf), String) + vbCrLf + CType(IIf(mtmpMachineList(i).NFCycles = "", "", mtmpMachineList(i).NFCycles & "(NF)" & vbCrLf), String)
                    If mOtherPeriods <> "" Then
                        mOtherPeriodExists = "True"
                        Exit For
                    End If
                Next

                For i As Integer = 0 To mtmpMachineList.Count - 1
                    searchstr7 = mtmpMachineList(i).Owner.ToString
                    mOtherPeriods = CType(IIf(mtmpMachineList(i).RINS = "", "", mtmpMachineList(i).RINS & "(RI)" & vbCrLf), String) _
                                     + vbCrLf + CType(IIf(mtmpMachineList(i).NGCycles = "", "", mtmpMachineList(i).NGCycles & "(NG)" & vbCrLf), String) + vbCrLf + CType(IIf(mtmpMachineList(i).NFCycles = "", "", mtmpMachineList(i).NFCycles & "(NF)" & vbCrLf), String)

                    ReportStatusList.Add(New rptStatus(mtmpMachineList(i).ID.ToString, 1, , , , , mtmpMachineList(i).TSO,
                                    , mtmpMachineList(i).CSO, , , , , , , , , mtmpMachineList(i).Cycles,
                                    mOtherPeriods, mOtherPeriodExists, Year(txtFromDate.Text).ToString, ,
                                    mtmpMachineList(i).RegNo, mtmpMachineList(i).ModelName, mtmpMachineList(i).Type, mtmpMachineList(i).SerialNo,
                                    mtmpMachineList(i).ManufacturerName, , mtmpMachineList(i).ManufacturingDate, mtmpMachineList(i).Hours,
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

                    '------Added on 18-Oct-2011------------

                    'Commented by Saylee on 7-July-2016 : Commented(AppSettings("ClientCode") = "RAL" And cmbFormat.SelectedIndex <> 2)
                    'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                    'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf


                    'If ((AppSettings("ClientCode") = "RAL" And cmbFormat.SelectedIndex <> 2) Or AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo"  Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                    If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                        ReportStatusList.Add(New rptStatus(AssemblyID.ToString, ObjAssemblyStatus.AssemblyTypeID, , "Reg. No.", ObjMachine.RegNo, ObjAssemblyStatus.AssemblyType + " " + "Model", ObjAssemblyStatus.Model,
                                                       "Serial No.", SerialNoPostion, "Due As of Airframe", "Done On", ObjAssemblyStatus.Position, , , , , , , , , , , LHLabel2, LHData2))
                    Else
                        ReportStatusList.Add(New rptStatus(AssemblyID.ToString, ObjAssemblyStatus.AssemblyTypeID, , "Reg. No.", ObjMachine.RegNo, ObjAssemblyStatus.AssemblyType + " " + "Model", ObjAssemblyStatus.Model,
                          "Serial No.", SerialNoPostion, "Due As of " & ObjAssemblyStatus.AssemblyType, "Done On " & ObjAssemblyStatus.AssemblyType, ObjAssemblyStatus.Position, , , , , , , , , , , LHLabel2, LHData2))

                        'Else

                        '    ReportStatusList.Add(New rptStatus(AssemblyID.ToString, ObjAssemblyStatus.AssemblyTypeID, , "Reg. No.", ObjMachine.RegNo, ObjAssemblyStatus.AssemblyType + " " + "Model", ObjAssemblyStatus.Model, _
                        '       "Serial No.", SerialNoPostion, "Due As of " & ObjAssemblyStatus.AssemblyType, "Done On", , , , , , , , , , , , LHLabel2, LHData2))
                    End If
                    '-------------------
                Next
                searchstr7 = ObjMachine.Customer.ToString  ' Changed By Saylee On 8-Aug-2011 '"Owner/Operator :- " +
            Next
        End If

        Dim ServiceTypeIds As New StringBuilder
        Dim InspTypeIds As New StringBuilder
        Dim ModTypeIds As New StringBuilder

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

        For K As Integer = 0 To ListDirectiveType.Items.Count - 1
            If ListDirectiveType.Items.Item(K).Selected Then
                ModTypeIds.Append(ListDirectiveType.Items.Item(K).Value + ",")
            End If
        Next

        If IsSerSelect = True Then
            mMachineList = MachineList.GetMachineListMonitoringStatus(AsonDate, MachineName, , , , , , , , , , IIf(chkComponent.Checked, True, False), True, , AssemblyName, , , , , , , , , ATACode, ATANomenclature, ShowCofA, , True, , , , , , , , , False, , False, , True, , , 0, , , True, IsAverageRequired:=mIsAverageRequired, ByPerDayLimit:=mByPerDayLimit, PerdayLimits:=mPerDayLimits, SkipIsForInventoryAircarft:=True, MonitorServiceTypeIDs:=ServiceTypeIds.ToString.TrimEnd(","), MonitorInspTypeIDs:=InspTypeIds.ToString.TrimEnd(","), MonitorModTypeIDs:=ModTypeIds.ToString.TrimEnd(","))
            For Each ObjMachine In mMachineList
                For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                    If chkAssembly.Checked = True Then
                        For Each ObjAssemblyMonitorServiceStatus In ObjAssemblyStatus.AssemblyMonitorServiceStatusList
                            If (ObjAssemblyMonitorServiceStatus.IsApplicable = True And chkNotApplicable.Checked = False) Or (chkNotApplicable.Checked = True) Then 'Or (ObjAssemblyMonitorServiceStatus.IsApplicable = False And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Then      'Checking Apllicablility
                                ATAChapter = ObjAssemblyMonitorServiceStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorServiceStatus.ATANomenclature

                                Dim TaskNo As String = ""
                                TaskNo = ObjAssemblyMonitorServiceStatus.TaskNo
                                Description = ObjAssemblyMonitorServiceStatus.Description
                                'If AppSettings("ShowMaintenanceForNewClients") = True And ObjAssemblyMonitorServiceStatus.TaskNo <> "" And mOpen = Open.ServiceReport Then
                                '    TaskNo = "Task No. : " & ObjAssemblyMonitorServiceStatus.TaskNo & Chr(10)
                                '    Description = TaskNo & ObjAssemblyMonitorServiceStatus.Description
                                'End If
                                Zone = ObjAssemblyMonitorServiceStatus.Zone 'Added By Prashant 19-Apr-2024
                                ModelEstimatedManHours = ObjAssemblyMonitorServiceStatus.ModelMonitorServiceRequiredManHours
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
                                StatusMasterID = ObjAssemblyMonitorServiceStatus.ModelMonitorServiceID     'vikrant
                                RecordOwnID = ObjAssemblyMonitorServiceStatus.ID.ToString
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

                                    If (ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 1 Or ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 3) And Not (ObjAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriodList.Contains(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID)) Then
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

                                    If ReportStatus = 0 Then 'Landscape

                                        If ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 1 Then     'Hours
                                            Freq1 = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue
                                            If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Or (ObjAssemblyMonitorServiceStatus.IsApplicable = False And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Then
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
                                                'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf
                                                If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
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
                                            If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Or (ObjAssemblyMonitorServiceStatus.IsApplicable = False And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Then
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
										'If ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 3 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 4 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 5 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 6 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 7 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 8 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 12 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 13 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 14 Then
										'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
										If ObjAssemblyMonitorServiceStatusPeriod.PeriodID >= 3 Then
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
													'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
													'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
													'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf
													If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
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
												DoneAt2 = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
											Else
												Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue
												If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Or (ObjAssemblyMonitorServiceStatus.IsApplicable = False And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Then
													ElapsedTime2 = ""
													RemainingTime2 = ""
													DueAsof2 = ""
													AssemblyDueAsof2 = ""
													SinceNew2 = ""
													TimeSinceNew = ""
												Else
													ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.AllElapsedValue
													RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.RemainingValue
													'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
													'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
													'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf
													If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
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
												'Commented By Saylee on 13-Nov-2017 as need to show Done On/Start Value also
												''If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = False) Then
												''    DoneAt2 = ""
												''Else
												''    DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
												''End If
												DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
												'***************
											End If
										End If
									Else    '  Report = 1
                                        If ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 2 Then     'Start Date
                                            If Freq3 = "" Then
                                                Freq3 = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValueFormatted

                                                If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Or (ObjAssemblyMonitorServiceStatus.IsApplicable = False And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Then
                                                    ElapsedTime2 = ""
                                                    RemainingTime2 = ""
                                                    DueAsof2 = ""
                                                    AssemblyDueAsof2 = ""
                                                    'SinceNew2 = ""
                                                Else
                                                    'ElapsedTime2 = ObjAssemblyMonitorServiceStatusPeriod.AllElapsedValueFormatted
                                                    ElapsedTime2 = ObjAssemblyMonitorServiceStatusPeriod.ElapsedValueFormatted
                                                    RemainingTime2 = ObjAssemblyMonitorServiceStatusPeriod.RemainingValueFormatted
                                                    DueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted
                                                    AssemblyDueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted
                                                    'SinceNew2 = ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValueFormatted
                                                    'SinceNew2 = (ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValueFormatted - ObjAssemblyMonitorServiceStatusPeriod.DoneOnValueFormatted)

                                                End If
                                                Extension2 = ObjAssemblyMonitorServiceStatusPeriod.ExtensionValueFormatted
                                                'Commented By Saylee on 13-Nov-2017 as need to show Done On/Start Value also
                                                'If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = False) Then
                                                '    DoneAt2 = ""
                                                'Else
                                                '    DoneAt2 = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                'End If
                                                DoneAt2 = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                '******************************************
                                            Else
                                                Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.FrequencyValueFormatted
                                                If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Or (ObjAssemblyMonitorServiceStatus.IsApplicable = False And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Then
                                                    ElapsedTime2 = ""
                                                    RemainingTime2 = ""
                                                    DueAsof2 = ""
                                                    AssemblyDueAsof2 = ""
                                                    'SinceNew2 = ""
                                                Else
                                                    ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.AllElapsedValueFormatted
                                                    RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.RemainingValueFormatted
                                                    DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted
                                                    AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted
                                                End If
                                                Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.ExtensionValueFormatted
                                                'Commented By Saylee on 13-Nov-2017 as need to show Done On/Start Value also
                                                ''If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = False) Then
                                                ''    DoneAt2 = ""
                                                ''Else
                                                ''    DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                ''End If
                                                DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                '**********************
                                            End If
                                        Else                                                           'For PeriodID <> 2
                                            If Freq3 = "" Then
                                                Freq3 = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue
                                                If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Or (ObjAssemblyMonitorServiceStatus.IsApplicable = False And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Then
                                                    ElapsedTime2 = ""
                                                    RemainingTime2 = ""
                                                    DueAsof2 = ""
                                                    AssemblyDueAsof2 = ""
                                                    SinceNew2 = ""
                                                    TimeSinceNew = ""
                                                Else
                                                    ElapsedTime2 = ObjAssemblyMonitorServiceStatusPeriod.AllElapsedValue
                                                    RemainingTime2 = ObjAssemblyMonitorServiceStatusPeriod.RemainingValue
                                                    'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                    'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                    'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf
                                                    If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
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
                                                'Commented By Saylee on 13-Nov-2017 as need to show Done On/Start Value also
                                                'If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = False) Then
                                                '    DoneAt2 = ""
                                                'Else
                                                '    DoneAt2 = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
                                                'End If
                                                DoneAt2 = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
                                                '******************************************
                                            Else
                                                Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue

                                                If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Or (ObjAssemblyMonitorServiceStatus.IsApplicable = False And ObjAssemblyMonitorServiceStatus.IsCompleted = True) Then
                                                    ElapsedTime2 = ""
                                                    RemainingTime2 = ""
                                                    DueAsof2 = ""
                                                    AssemblyDueAsof2 = ""
                                                    SinceNew2 = ""
                                                    TimeSinceNew = ""
                                                Else
                                                    ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.AllElapsedValue
                                                    RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.RemainingValue
                                                    'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                    'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                    'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf

                                                    If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
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

                                                'Commented By Saylee on 13-Nov-2017 as need to show Done On/Start Value also
                                                ''If (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = False) Then
                                                ''    DoneAt2 = ""
                                                ''Else
                                                ''    DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
                                                ''End If
                                                DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
                                            End If
                                        End If
                                    End If
                                Next

                                If chkNotMonitoredValues.Checked = True Then 'Added by Saylee on 11-May-2015 for ALL11052015
                                    If DoneONValueForNoPeriod <> "" Then 'Added by Saylee on 19-Sep-2014 for ALL19092014
                                        If DoneAt2 = "" Then
                                            DoneAt2 = DoneONValueForNoPeriod
                                        Else
                                            DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & DoneONValueForNoPeriod
                                        End If

                                    End If

                                    If TSOValueForNoPeriod <> "" And ObjAssemblyMonitorServiceStatus.ModelMonitorServiceTypeID = 1 Then 'Added by Saylee on 19-Sep-2014 for ALL19092014
                                        If SinceNew2 = "" Then
                                            SinceNew2 = TSOValueForNoPeriod
                                        Else
                                            SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & TSOValueForNoPeriod
                                        End If

                                    End If
                                End If
                                '''Added By Saylee on 25-May-2015 for Taj25052015
                                ''If (AppSettings("ClientCode") = "Taj" OR AppSettings("ClientCode") = "HSC" Or AppSettings("ClientCode") = "ASH" ) Then
                                ''    DoneAt2 = DoneONValueForAssembly
                                ''    DoneONValueForAssembly = ""
                                ''Else
                                ''    DoneAt2 = DoneAt2
                                ''End If
                                AssemblyID = ObjAssemblyStatus.AssemblyID
                                Note = ObjAssemblyMonitorServiceStatus.Notes
                                Remark = ObjAssemblyMonitorServiceStatus.DoneRemark
                                ExtensionDate = ObjAssemblyMonitorServiceStatus.ExtensionDate
                                ApprovalRemark = ObjAssemblyMonitorServiceStatus.ApprovalRemark
                                Reference = ObjAssemblyMonitorServiceStatus.Reference  'Added By Prashant 3-Apr-2013  'Indamer03042013
                                Dim ATACode As Integer = ObjAssemblyMonitorServiceStatus.ATACode


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

                                ''Added By Saylee on 2-Aug-2024
                                If ObjAssemblyMonitorServiceStatus.NonMonitoringPeriodDetails <> "" Then
                                    DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatus.NonMonitoringPeriodDetails.Replace("<br>", vbCrLf)
                                End If
                                '************************
                                If ObjAssemblyMonitorServiceStatus.IsApplicable = False Then
                                    DueAsof = ""
                                    DueAsof1 = ""
                                    DueAsof2 = ""
                                    RemainingTime = ""
                                    RemainingTime1 = ""
                                    RemainingTime2 = ""
                                End If

                                ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, , , Position, MonitorType, MonitorTypeCode, Note, Remark, Description,
                            , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, , ,
                            SinceNew2, , , DoneAt2, , , , , , , , StartDateData, , , , , , , , , , Reference, , DoneOnDate, , , , , , AssemblyDueAsof2, Extension, Extension1, Extension2,
                            ExtensionDate, ApprovalRemark, , , Code, StatusMasterID.ToString, , , , , , ObjAssemblyMonitorServiceStatus.MonitorTypeID, , , , , , , TimeSinceNew, DoneONValueForAssembly:=DoneONValueForAssembly, SourceDoc:=SourceDoc, RecordID:=RecordOwnID, Zone:=TaskNo, Area:=Zone, ModelEstimatedManHours:=ModelEstimatedManHours, TaskNo:=TaskNo))
                            End If
                        Next
                    End If
                    If chkComponent.Checked Then
                        For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
                            For Each ObjCompMonitorServiceStatus In ObjCompStatus.CompMonitorServiceStatusList
                                If (ObjCompMonitorServiceStatus.IsApplicable = True And chkNotApplicable.Checked = False) Or (chkNotApplicable.Checked = True) Then 'Or (ObjCompMonitorServiceStatus.IsApplicable = False And ObjCompMonitorServiceStatus.IsCompleted = True) Then
                                    ATAChapter = ObjCompMonitorServiceStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorServiceStatus.ATANomenclature
                                    'Description = ObjCompMonitorServiceStatus.Description
                                    Dim TaskNo As String = ""
                                    TaskNo = ObjCompMonitorServiceStatus.TaskNo
                                    Description = ObjCompMonitorServiceStatus.Description
                                    'If AppSettings("ShowMaintenanceForNewClients") = True And ObjCompMonitorServiceStatus.TaskNo <> "" And mOpen = Open.ServiceReport Then
                                    '    TaskNo = "Task No. : " & ObjCompMonitorServiceStatus.TaskNo & Chr(10)
                                    '    Description = TaskNo & ObjCompMonitorServiceStatus.Description
                                    'End If
                                    Zone = ObjCompMonitorServiceStatus.Zone 'Added By Prashant 19-Apr-2024
                                    ModelEstimatedManHours = ObjCompMonitorServiceStatus.PartMonitorServiceRequiredManHours
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
                                    If AppSettings("ClientCode") = "7AR" Then
                                        MonitorTypeCode = ObjCompMonitorServiceStatus.ServiceTypeCode
                                        SourceDoc = ObjCompMonitorServiceStatus.Source
                                    Else
                                        MonitorTypeCode = ObjCompMonitorServiceStatus.Code
                                        SourceDoc = ObjCompMonitorServiceStatus.SourceDoc
                                    End If


                                    'End
                                    Periodcount = ObjCompStatus.CompStatusPeriodList.Count()
                                    StatusMasterID = ObjCompMonitorServiceStatus.PartMonitorServiceID      'vikrant
                                    RecordOwnID = ObjCompMonitorServiceStatus.ID.ToString
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
                                                    'If CompCurrentValue.GetCompCurrentValue(ObjCompStatus.ID, ObjCompMonitorServiceStatus.DoneOn, ObjCompStatus.CompStatusPeriodList(Count).PeriodID).Count > 0 Then
                                                    Dim mMachineMaintenance As MachineMaintenance = MachineMaintenance.GetMachineMaintenance(ObjCompMonitorServiceStatus.ID, MachineMaintenanceActivity.ComponentService)

                                                    Dim mAssemblyCurrentValue As New Period(1, DBNull.Value)
                                                    If CurrentValueOnAsOnDate.GetCurrentValue(ObjAssemblyStatus.ID, ObjCompMonitorServiceStatus.DoneOn, mMachineMaintenance.LogNo, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID)(0).CurrentValueDec > 0 Then
                                                        mAssemblyCurrentValue = New Period(mPeriodID, CurrentValueOnAsOnDate.GetCurrentValue(ObjAssemblyStatus.ID, ObjCompMonitorServiceStatus.DoneOn, mMachineMaintenance.LogNo, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID)(0).CurrentValueDec, , , , ObjMachine.HourType)
                                                    Else
                                                        mAssemblyCurrentValue = New Period(mPeriodID, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).AssemblyCurrentValueInDeciaml, , , , ObjMachine.HourType)
                                                    End If

                                                    ' tmpCurrentValue = New Period(mPeriodID, Period.Add(ObjCompStatus.CompStatusPeriodList(Count).PeriodID, mPeriodUnitID, ObjCompStatus.CompStatusPeriodList(Count).CompCurrentValueInDecimal, Period.Difference(mAssemblyCurrentValue.DBValue, ObjCompStatus.CompStatusPeriodList(Count).AssemblyCurrentValueInDeciaml)), mPeriodUnitID, False, , ObjMachine.HourType).DbValueDec
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
                                        'Done On As of Assembly
                                        'If (AppSettings("ClientCode") = "Taj" OR AppSettings("ClientCode") = "HSC" Or AppSettings("ClientCode") = "ASH" ) Then
                                        If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = False) Then
                                            DoneONValueForAssembly = ""
                                        Else
                                            If ObjCompMonitorServiceStatus.DoneOn <> "" Then
                                                mDoneONValueForAssembly = New Period(ObjCompMonitorServiceStatusPeriod.PeriodID, DBNull.Value)
                                                If ObjCompMonitorServiceStatusPeriod.PeriodID <> 2 Then
                                                    Dim mMachineMaintenance As MachineMaintenance = MachineMaintenance.GetMachineMaintenance(ObjCompMonitorServiceStatus.ID, MachineMaintenanceActivity.ComponentService)
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
                                        ' End If
                                        '**************************
                                        If ReportStatus = 0 Then 'Landscape
                                            If ObjCompMonitorServiceStatusPeriod.PeriodID = 1 Then
                                                Freq1 = ObjCompMonitorServiceStatusPeriod.FrequencyValue
                                                If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Or (ObjCompMonitorServiceStatus.IsApplicable = False And ObjCompMonitorServiceStatus.IsCompleted = True) Then
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
                                                    'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                    'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                    'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf
                                                    If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
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

                                                If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Or (ObjCompMonitorServiceStatus.IsApplicable = False And ObjCompMonitorServiceStatus.IsCompleted = True) Then
                                                    ElapsedTime1 = ""
                                                    RemainingTime1 = ""
                                                    DueAsof1 = ""
                                                Else
                                                    ElapsedTime1 = ObjCompMonitorServiceStatusPeriod.AllElapsedValueFormatted
                                                    RemainingTime1 = ObjCompMonitorServiceStatusPeriod.RemainingValueFormatted
                                                    'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                    'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                    'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf
                                                    If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                        DueAsof1 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                    Else
                                                        DueAsof1 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                    End If
                                                End If
                                                Extension1 = ObjCompMonitorServiceStatusPeriod.ExtensionValueFormatted
                                                DiffCompInstDoneOnValue = ObjCompMonitorServiceStatusPeriod.DiffCompInstDoneOnValue
                                            End If
											'If ObjCompMonitorServiceStatusPeriod.PeriodID = 3 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 4 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 5 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 6 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 7 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 8 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 12 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 13 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 14 Then
											'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
											If ObjCompMonitorServiceStatusPeriod.PeriodID >= 3 Then
												If Freq3 = "" Then
													Freq3 = ObjCompMonitorServiceStatusPeriod.FrequencyValue

													If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Or (ObjCompMonitorServiceStatus.IsApplicable = False And ObjCompMonitorServiceStatus.IsCompleted = True) Then
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
															'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
															'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
															'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf
															If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
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
															'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
															'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
															'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf
															If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
																DueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
															Else
																DueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
															End If
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
													'Commented By Saylee on 13-Nov-2017 as need to show Done On/Start Value also
													'If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = False) Then
													'    DoneAt2 = ""
													'Else
													'    DoneAt2 = ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
													'End If
													DoneAt2 = ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
													DiffCompInstDoneOnValue = ObjCompMonitorServiceStatusPeriod.DiffCompInstDoneOnValue
												Else
													Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.FrequencyValue

													If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Or (ObjCompMonitorServiceStatus.IsApplicable = False And ObjCompMonitorServiceStatus.IsCompleted = True) Then
														ElapsedTime2 = ""
														RemainingTime2 = ""
														DueAsof2 = ""
														AssemblyDueAsof2 = ""
														SinceNew2 = ""
														TimeSinceNew = ""
													Else
														If (ObjCompMonitorServiceStatus.MonitorTypeID = 4) Then
															ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AllElapsedValue
															RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.RemainingValue
															'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
															'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
															'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf
															If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
																DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
															Else
																DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
															End If
															'-----------------------------
															AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
															SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DiffCompCurrentDoneOnValueFormatted
															' DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbcrlf) & ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
															TimeSinceNew = TimeSinceNew & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted
															DiffCompInstDoneOnValue = ObjCompMonitorServiceStatusPeriod.DiffCompInstDoneOnValue
														Else
															ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AllElapsedValue
															RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.RemainingValue
															'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
															'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
															'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf
															If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
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
															' DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbcrlf) & ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
															TimeSinceNew = TimeSinceNew & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted

														End If
													End If
													Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ExtensionValue
													'Commented By Saylee on 13-Nov-2017 as need to show Done On/Start Value also
													''If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = False) Then
													''    DoneAt2 = ""
													''Else
													''    DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
													''End If
													DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
													'********************************
													DiffCompInstDoneOnValue = DiffCompInstDoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DiffCompInstDoneOnValue
												End If
											End If
										Else
                                            If ObjCompMonitorServiceStatusPeriod.PeriodID = 2 Then     'StartDate
                                                If Freq3 = "" Then
                                                    Freq3 = ObjCompMonitorServiceStatusPeriod.FrequencyValueFormatted

                                                    If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Or (ObjCompMonitorServiceStatus.IsApplicable = False And ObjCompMonitorServiceStatus.IsCompleted = True) Then
                                                        ElapsedTime2 = ""
                                                        RemainingTime2 = ""
                                                        DueAsof2 = ""
                                                        AssemblyDueAsof2 = ""
                                                        'SinceNew2 = ""
                                                    Else
                                                        If (ObjCompMonitorServiceStatus.MonitorTypeID = 4) Then
                                                            'ElapsedTime2 = "" 'Commneted By Prashant 29-July-2009 Because we have to show elapsed values for "Expiry" status also
                                                            ElapsedTime2 = ObjCompMonitorServiceStatusPeriod.AllElapsedValueFormatted
                                                            RemainingTime2 = ObjCompMonitorServiceStatusPeriod.RemainingValueFormatted
                                                            '$$$$$$
                                                            'DueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                            'Added By Prashant 18-Oct-2011

                                                            'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                            'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                            'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf

                                                            If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                                DueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                            Else
                                                                DueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                            End If
                                                            '--------------
                                                            AssemblyDueAsof2 = ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                        Else
                                                            ElapsedTime2 = ObjCompMonitorServiceStatusPeriod.AllElapsedValueFormatted
                                                            RemainingTime2 = ObjCompMonitorServiceStatusPeriod.RemainingValueFormatted
                                                            'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                            'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                            'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf
                                                            If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                                DueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                            Else
                                                                DueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                            End If
                                                            AssemblyDueAsof2 = ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                        End If
                                                    End If
                                                    Extension2 = ObjCompMonitorServiceStatusPeriod.ExtensionValueFormatted
                                                    'Commented By Saylee on 13-Nov-2017 as need to show Done On/Start Value also
                                                    'If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = False) Then
                                                    '    DoneAt2 = ""
                                                    'Else
                                                    '    DoneAt2 = ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                    'End If
                                                    DoneAt2 = ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                    DiffCompInstDoneOnValue = ObjCompMonitorServiceStatusPeriod.DiffCompInstDoneOnValue
                                                Else
                                                    Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.FrequencyValueFormatted
                                                    If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Or (ObjCompMonitorServiceStatus.IsApplicable = False And ObjCompMonitorServiceStatus.IsCompleted = True) Then
                                                        ElapsedTime2 = ""
                                                        RemainingTime2 = ""
                                                        DueAsof2 = ""
                                                        AssemblyDueAsof2 = ""
                                                    Else
                                                        If (ObjCompMonitorServiceStatus.MonitorTypeID = 4) Then
                                                            ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AllElapsedValueFormatted
                                                            RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.RemainingValueFormatted
                                                            'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                            'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                            'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf
                                                            If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                                DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                            Else
                                                                DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                            End If
                                                            '-------------
                                                            AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                        Else
                                                            ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AllElapsedValueFormatted
                                                            RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.RemainingValueFormatted
                                                            'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                            'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                            'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf
                                                            If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                                DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                            Else
                                                                DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                            End If
                                                            '---------------
                                                            AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                        End If
                                                    End If
                                                    Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.ExtensionValueFormatted
                                                    'Commented By Saylee on 13-Nov-2017 as need to show Done On/Start Value also
                                                    'If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = False) Then
                                                    '    DoneAt2 = ""
                                                    'Else
                                                    '    DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                    'End If
                                                    DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                    '***************************************
                                                    DiffCompInstDoneOnValue = DiffCompInstDoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DiffCompInstDoneOnValue
                                                End If
                                            Else              'PeriodID <> 2
                                                If Freq3 = "" Then
                                                    Freq3 = ObjCompMonitorServiceStatusPeriod.FrequencyValue
                                                    If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Or (ObjCompMonitorServiceStatus.IsApplicable = False And ObjCompMonitorServiceStatus.IsCompleted = True) Then
                                                        ElapsedTime2 = ""
                                                        RemainingTime2 = ""
                                                        DueAsof2 = ""
                                                        AssemblyDueAsof2 = ""
                                                        SinceNew2 = ""
                                                        DoneAt2 = ""
                                                        TimeSinceNew = ""
                                                    Else
                                                        If (ObjCompMonitorServiceStatus.MonitorTypeID = 4) Then
                                                            'ElapsedTime2 = ""'Commneted By Prashant 29-July-2009 Because we have to show elapsed values for "Expiry" status also
                                                            ElapsedTime2 = ObjCompMonitorServiceStatusPeriod.AllElapsedValue
                                                            RemainingTime2 = ObjCompMonitorServiceStatusPeriod.RemainingValue
                                                            If ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Then
                                                                DueAsof2 = ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                                AssemblyDueAsof2 = ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                                SinceNew2 = ObjCompMonitorServiceStatusPeriod.DiffCompCurrentDoneOnValueFormatted
                                                                'DoneAt2 = ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                                TimeSinceNew = ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted
                                                            Else
                                                                'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                                'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                                'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf
                                                                If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                                    DueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                                Else
                                                                    DueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                                End If
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
                                                                'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                                'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                                'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf
                                                                If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
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
                                                    'Commented By Saylee on 13-Nov-2017 as need to show Done On/Start Value also
                                                    'If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = False) Then
                                                    '    DoneAt2 = ""
                                                    'Else
                                                    '    DoneAt2 = ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                    'End If
                                                    DoneAt2 = ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                    DiffCompInstDoneOnValue = ObjCompMonitorServiceStatusPeriod.DiffCompInstDoneOnValue
                                                Else
                                                    Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.FrequencyValue
                                                    If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True) Or (ObjCompMonitorServiceStatus.IsApplicable = False And ObjCompMonitorServiceStatus.IsCompleted = True) Then
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
                                                                'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                                'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                                'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf
                                                                If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
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
                                                                'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                                'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                                'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf
                                                                If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                                    DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                                Else
                                                                    DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted
                                                                End If
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
                                                    'Commented By Saylee on 13-Nov-2017 as need to show Done On/Start Value also
                                                    'If (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = False) Then
                                                    '    DoneAt2 = ""
                                                    'Else
                                                    '    DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                    'End If
                                                    DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                    '*******************
                                                    DiffCompInstDoneOnValue = DiffCompInstDoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatusPeriod.DiffCompInstDoneOnValue
                                                End If
                                            End If
                                        End If
                                    Next
                                    If chkNotMonitoredValues.Checked = True Then 'Added by Saylee on 11-May-2015 for ALL11052015
                                        If DoneONValueForNoPeriod <> "" Then 'Added by Saylee on 19-Sep-2014 for ALL19092014
                                            If DoneAt2 = "" Then
                                                DoneAt2 = DoneONValueForNoPeriod
                                            Else
                                                DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & DoneONValueForNoPeriod
                                            End If
                                        End If
                                        If TSOValueForNoPeriod <> "" And ObjCompMonitorServiceStatus.PartMonitorServiceTypeID = 1 Then 'Added by Saylee on 19-Sep-2014 for ALL19092014
                                            'SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbcrlf) & TSOValueForNoPeriod
                                            If SinceNew2 = "" Then
                                                SinceNew2 = TSOValueForNoPeriod
                                            Else
                                                SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & TSOValueForNoPeriod
                                            End If
                                        End If
                                    End If
                                    AssemblyID = ObjAssemblyStatus.AssemblyID
                                    Note = ObjCompMonitorServiceStatus.Notes
                                    Remark = ObjCompMonitorServiceStatus.DoneRemark
                                    ExtensionDate = ObjCompMonitorServiceStatus.ExtensionDate
                                    ApprovalRemark = ObjCompMonitorServiceStatus.ApprovalRemark
                                    Reference = ObjCompMonitorServiceStatus.Reference  'Added By Prashant 3-Apr-2013  'Indamer03042013
                                    Dim ATACode = ObjCompMonitorServiceStatus.ATACode


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

                                    ' ''Added By Saylee on 2-Aug-2024
                                    If ObjCompMonitorServiceStatus.NonMonitoringPeriodDetails <> "" Then
                                        DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatus.NonMonitoringPeriodDetails.Replace("<br>", vbCrLf)
                                    End If
                                    '*****************************

                                    If ObjCompMonitorServiceStatus.IsApplicable = False Then
                                        DueAsof = ""
                                        DueAsof1 = ""
                                        DueAsof2 = ""
                                        RemainingTime = ""
                                        RemainingTime1 = ""
                                        RemainingTime2 = ""
                                    End If

                                    ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, MonitorType, MonitorTypeCode, Note, Remark, Description,
                 , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2,
                DueAsof, DueAsof1, DueAsof2, AssemblyModel, , , SinceNew2, , , DoneAt2, , , , ObjCompMonitorServiceStatus.ATACode, , , , StartDateData, , , , , , , , , , Reference, , DoneOnDate, , , , , , AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate,
                ApprovalRemark, , , Code, StatusMasterID.ToString, , , , , , ObjCompMonitorServiceStatus.MonitorTypeID, , , , , , , TimeSinceNew, DoneONValueForAssembly:=DoneONValueForAssembly, SourceDoc:=SourceDoc, DiffCompInstDoneOnValue:=DiffCompInstDoneOnValue, RecordID:=RecordOwnID, Zone:=TaskNo, Area:=Zone, ModelEstimatedManHours:=ModelEstimatedManHours, TaskNo:=TaskNo))
                                End If
                            Next
                        Next
                    End If
                Next
            Next
            'End If
            'Next
        End If
        If IsInsSelect = True Then
            'For i As Integer = 0 To ListInspectionType.Items.Count - 1
            'If ListInspectionType.Items.Item(i).Selected Then
            'Added By Utkarsh ON 12-Sep-2013 FOR BA11092013  parameters (ByPerdayLimit,PerDayLimits ,IsAverageRequired)
            mMachineList = MachineList.GetMachineListMonitoringStatus(AsonDate, MachineName, , , , , , , , , , IIf(chkComponent.Checked, True, False), True, , AssemblyName, , , , , , , , , ATACode, ATANomenclature, ShowCofA, , , True, , , , , , , , False, , False, , True, , , , 0, , , True, IsAverageRequired:=mIsAverageRequired, ByPerDayLimit:=mByPerDayLimit, PerdayLimits:=mPerDayLimits, SkipIsForInventoryAircarft:=True, MonitorServiceTypeIDs:=ServiceTypeIds.ToString.TrimEnd(","), MonitorInspTypeIDs:=InspTypeIds.ToString.TrimEnd(","), MonitorModTypeIDs:=ModTypeIds.ToString.TrimEnd(","))
            For Each ObjMachine In mMachineList
                For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                    If chkAssembly.Checked Then
                        For Each ObjAssemblyMonitorInspStatus In ObjAssemblyStatus.AssemblyMonitorInspStatusList
                            If (ObjAssemblyMonitorInspStatus.IsApplicable = True And chkNotApplicable.Checked = False) Or (chkNotApplicable.Checked = True) Then 'Or (ObjAssemblyMonitorInspStatus.IsApplicable = False And ObjAssemblyMonitorInspStatus.IsCompleted = True) Then
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
                                    'Commented By Saylee on 22-Jun-2011
                                    ''If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 2 Then
                                    ''    If ObjAssemblyMonitorInspStatus.DoneOn = "" Then
                                    ''        StartDateLabel = CType(IIf(StartDateLabel = "", StartDateLabel, StartDateLabel +  IIf(IsExcel, Chr(10), vbcrlf)), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                                    ''        StartDateData = CType(IIf(StartDateData = "", StartDateData, StartDateData +  IIf(IsExcel, Chr(10), vbcrlf)), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyStartValueFormatted
                                    ''    Else
                                    ''        StartDateLabel = CType(IIf(StartDateLabel = "", StartDateLabel, StartDateLabel +  IIf(IsExcel, Chr(10), vbcrlf)), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName 'Added by Saylee on 31-May-2010
                                    ''        StartDateData = CType(IIf(StartDateData = "", StartDateData, StartDateData +  IIf(IsExcel, Chr(10), vbcrlf)), String) + ObjAssemblyMonitorInspStatus.DoneOnFormatted 'Added by Saylee on 31-May-2010
                                    ''    End If
                                    ''End If
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

                                    'Added By Saylee on 25-May-2015 for Taj25052015
                                    'Done On As of Assembly
                                    ' If (AppSettings("ClientCode") = "Taj" OR AppSettings("ClientCode") = "HSC" Or AppSettings("ClientCode") = "ASH" ) Then
                                    If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = False) Then
                                        DoneONValueForAssembly = ""
                                    Else
                                        If ObjAssemblyMonitorInspStatus.DoneOn <> "" Then
                                            mDoneONValueForAssembly = New Period(ObjAssemblyMonitorInspStatusPeriod.PeriodID, DBNull.Value)

                                            If ObjAssemblyMonitorInspStatusPeriod.PeriodID <> 2 Then
                                                Dim mMachineMaintenance As MachineMaintenance = MachineMaintenance.GetMachineMaintenance(ObjAssemblyMonitorInspStatus.ID, MachineMaintenanceActivity.AssemblyInspection)
                                                If CurrentValueOnAsOnDate.GetCurrentValue(mMachineMaintenance.LogAssemblyStatusID, ObjAssemblyMonitorInspStatus.DoneOn, mMachineMaintenance.LogNo, ObjAssemblyMonitorInspStatusPeriod.PeriodID)(0).CurrentValueDec > 0 Then
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
                                    ' End If
                                    '**************************

                                    If ReportStatus = 0 Then 'Landscape
                                        If ObjAssemblyMonitorInspStatusPeriod.PeriodID = 1 Then
                                            Freq1 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValue
                                            If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False And ObjAssemblyMonitorInspStatus.IsCompleted = True) Then
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


                                                If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
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


                                            If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False And ObjAssemblyMonitorInspStatus.IsCompleted = True) Then
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
										'If ObjAssemblyMonitorInspStatusPeriod.PeriodID = 3 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 4 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 5 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 6 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 7 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 8 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 12 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 13 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 14 Then
										'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
										If ObjAssemblyMonitorInspStatusPeriod.PeriodID >= 3 Then
											If Freq3 = "" Then
												Freq3 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValue


												If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False And ObjAssemblyMonitorInspStatus.IsCompleted = True) Then
													ElapsedTime2 = ""
													RemainingTime2 = ""
													DueAsof2 = ""
													AssemblyDueAsof2 = ""
													SinceNew2 = ""
													TimeSinceNew = ""
												Else
													ElapsedTime2 = ObjAssemblyMonitorInspStatusPeriod.ElapsedValue
													RemainingTime2 = ObjAssemblyMonitorInspStatusPeriod.RemainingValue
													'Added By Prashant 17-Sep-2013

													'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
													'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
													'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf

													If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
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
													' DoneAt2 = ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
													TimeSinceNew = ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue
												End If
												Extension2 = ObjAssemblyMonitorInspStatusPeriod.ExtensionValue

												'Commented By Saylee on 13-Nov-2017 as need to show Done On/Start Value also
												'If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = False) Then
												'    DoneAt2 = ""
												'Else
												'    DoneAt2 = ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
												'End If
												DoneAt2 = ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
											Else
												Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.FrequencyValue

												If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False And ObjAssemblyMonitorInspStatus.IsCompleted = True) Then
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

													'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
													'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
													'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf

													If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
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
													'DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbcrlf) & ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
													TimeSinceNew = TimeSinceNew & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue
												End If
												Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.ExtensionValue

												'Commented By Saylee on 13-Nov-2017 as need to show Done On/Start Value also
												'If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = False) Then
												'    DoneAt2 = ""
												'Else
												'    DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
												'End If
												DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
												'*************************************
											End If
										End If
									Else
                                        If ObjAssemblyMonitorInspStatusPeriod.PeriodID = 2 Then        'StartDate
                                            If Freq3 = "" Then
                                                Freq3 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValueFormatted

                                                If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False And ObjAssemblyMonitorInspStatus.IsCompleted = True) Then
                                                    ElapsedTime2 = ""
                                                    RemainingTime2 = ""
                                                    DueAsof2 = ""
                                                    AssemblyDueAsof2 = ""
                                                    'SinceNew2 = ""
                                                    '
                                                Else
                                                    ElapsedTime2 = ObjAssemblyMonitorInspStatusPeriod.ElapsedValueFormatted
                                                    RemainingTime2 = ObjAssemblyMonitorInspStatusPeriod.RemainingValueFormatted
                                                    DueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
                                                    AssemblyDueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
                                                    'SinceNew2 = ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValueFormatted
                                                    'SinceNew2 = (ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValueFormatted - ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted)
                                                    'DoneAt2 = ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted
                                                End If
                                                Extension2 = ObjAssemblyMonitorInspStatusPeriod.ExtensionValueFormatted

                                                'Commented By Saylee on 13-Nov-2017 as need to show Done On/Start Value also
                                                'If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = False) Then
                                                '    DoneAt2 = ""
                                                'Else
                                                '    DoneAt2 = ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted
                                                'End If
                                                DoneAt2 = ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted
                                            Else
                                                Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.FrequencyValueFormatted

                                                If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False And ObjAssemblyMonitorInspStatus.IsCompleted = True) Then
                                                    ElapsedTime2 = ""
                                                    RemainingTime2 = ""
                                                    DueAsof2 = ""
                                                    AssemblyDueAsof2 = ""
                                                    'SinceNew2 = ""
                                                Else
                                                    ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.ElapsedValueFormatted
                                                    RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.RemainingValue
                                                    DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
                                                    AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
                                                    'SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbcrlf) & ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValueFormatted
                                                    'SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbcrlf) & (ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValueFormatted - ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted)
                                                    'DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbcrlf) & ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted
                                                End If
                                                Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.ExtensionValueFormatted

                                                'Commented By Saylee on 13-Nov-2017 as need to show Done On/Start Value also
                                                'If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = False) Then
                                                '    DoneAt2 = ""
                                                'Else
                                                '    DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted
                                                'End If
                                                DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted
                                                '***************************
                                            End If
                                        Else                                                           'PeriodID <> 2      
                                            If Freq3 = "" Then
                                                Freq3 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValue

                                                If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False And ObjAssemblyMonitorInspStatus.IsCompleted = True) Then
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

                                                    'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                    'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                    'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf

                                                    If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
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
                                                    'DoneAt2 = ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
                                                    TimeSinceNew = ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue
                                                End If
                                                Extension2 = ObjAssemblyMonitorInspStatusPeriod.ExtensionValue

                                                'Commented By Saylee on 13-Nov-2017 as need to show Done On/Start Value also
                                                'If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = False) Then
                                                '    DoneAt2 = ""
                                                'Else
                                                '    DoneAt2 = ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
                                                'End If
                                                DoneAt2 = ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
                                                '***************************
                                            Else                                                       'Freq3 <> ""
                                                Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.FrequencyValue


                                                If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False And ObjAssemblyMonitorInspStatus.IsCompleted = True) Then
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

                                                    'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                    'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                    'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf


                                                    If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
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
                                                    'DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbcrlf) & ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
                                                    TimeSinceNew = TimeSinceNew & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue
                                                End If
                                                Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.ExtensionValue

                                                'Commented By Saylee on 13-Nov-2017 as need to show Done On/Start Value also
                                                'If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = False) Then
                                                '    DoneAt2 = ""
                                                'Else
                                                '    DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
                                                'End If
                                                DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
                                                '***************************
                                            End If
                                        End If
                                    End If
                                Next

                                If chkNotMonitoredValues.Checked = True Then 'Added by Saylee on 11-May-2015 for ALL11052015
                                    If DoneONValueForNoPeriod <> "" Then 'Added by Saylee on 19-Sep-2014 for ALL19092014
                                        If DoneAt2 = "" Then
                                            DoneAt2 = DoneONValueForNoPeriod
                                        Else
                                            DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & DoneONValueForNoPeriod
                                        End If
                                    End If
                                End If

                                ''Added By Saylee on 25-May-2015 for Taj25052015
                                'If (AppSettings("ClientCode") = "Taj" OR AppSettings("ClientCode") = "HSC" Or AppSettings("ClientCode") = "ASH" ) Then
                                '    DoneAt2 = DoneONValueForAssembly
                                '    DoneONValueForAssembly = ""
                                'Else
                                '    DoneAt2 = DoneAt2
                                'End If

                                AssemblyID = ObjAssemblyStatus.AssemblyID
                                Note = ObjAssemblyMonitorInspStatus.Notes
                                Remark = ObjAssemblyMonitorInspStatus.DoneRemark
                                ExtensionDate = ObjAssemblyMonitorInspStatus.ExtensionDate
                                ApprovalRemark = ObjAssemblyMonitorInspStatus.ApprovalRemark
                                Reference = ObjAssemblyMonitorInspStatus.Reference  'Added By Prashant 3-Apr-2013  'Indamer03042013
                                Dim ATAcode = ObjAssemblyMonitorInspStatus.ATACode
                                SourceDoc = ObjAssemblyMonitorInspStatus.SourceDoc

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

                                ''Added By Saylee on 2-Aug-2024
                                If ObjAssemblyMonitorInspStatus.NonMonitoringPeriodDetails <> "" Then
                                    DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatus.NonMonitoringPeriodDetails.Replace("<br>", vbCrLf)
                                End If
                                '*******************************************
                                If ObjAssemblyMonitorInspStatus.IsApplicable = False Then
                                    DueAsof = ""
                                    DueAsof1 = ""
                                    DueAsof2 = ""
                                    RemainingTime = ""
                                    RemainingTime1 = ""
                                    RemainingTime2 = ""
                                End If

                                ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, , , Position, MonitorType, MonitorTypeCode, Note, Remark, Description,
        , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2,
        DueAsof, DueAsof1, DueAsof2, AssemblyModel, , , SinceNew2, , , DoneAt2, , , , ObjAssemblyMonitorInspStatus.ATACode, , , , StartDateData, , , , , , , , , , Reference, , DoneOnDate, , , , , , AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate,
        ApprovalRemark, , , Code, StatusMasterID.ToString, , , , , , ObjAssemblyMonitorInspStatus.MonitorTypeID, , , , , , , TimeSinceNew, DoneONValueForAssembly:=DoneONValueForAssembly, SourceDoc:=SourceDoc, RecordID:=RecordOwnID))
                            End If
                        Next
                    End If
                    If chkComponent.Checked Then


                        For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
                            For Each ObjCompMonitorInspStatus In ObjCompStatus.CompMonitorInspStatusList
                                If (ObjCompMonitorInspStatus.IsApplicable = True And chkNotApplicable.Checked = False) Or (chkNotApplicable.Checked = True) Then 'Or (ObjCompMonitorInspStatus.IsApplicable = False And ObjCompMonitorInspStatus.IsCompleted = True) Then
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
                                                            'StartDateData = CType(IIf(StartDateData = "", StartDateData, StartDateData +  IIf(IsExcel, Chr(10), vbcrlf)), String) + ObjCompStatus.CompStatusPeriodList(ObjCompStatus.CompStatusPeriodList(Count).PeriodID, "").CompStartValueFormatted
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

                                        If (ObjCompStatus.CompStatusPeriodList(Count).PeriodID = 1 Or ObjCompStatus.CompStatusPeriodList(Count).PeriodID = 3) And Not (ObjCompMonitorInspStatus.CompMonitorInspStatusPeriodList.Contains(ObjCompStatus.CompStatusPeriodList(Count).PeriodID)) Then

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
                                                    'If CompCurrentValue.GetCompCurrentValue(ObjCompStatus.ID, ObjCompMonitorInspStatus.DoneOn, ObjCompStatus.CompStatusPeriodList(Count).PeriodID).Count > 0 Then
                                                    Dim mMachineMaintenance As MachineMaintenance = MachineMaintenance.GetMachineMaintenance(ObjCompMonitorInspStatus.ID, MachineMaintenanceActivity.ComponentInspection)
                                                    Dim mAssemblyCurrentValue As New Period(1, DBNull.Value)

                                                    If CurrentValueOnAsOnDate.GetCurrentValue(mMachineMaintenance.LogAssemblyStatusID, ObjCompMonitorInspStatus.DoneOn, mMachineMaintenance.LogNo, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID)(0).CurrentValueDec > 0 Then
                                                        mAssemblyCurrentValue = New Period(56, CurrentValueOnAsOnDate.GetCurrentValue(mMachineMaintenance.LogAssemblyStatusID, ObjCompMonitorInspStatus.DoneOn, mMachineMaintenance.LogNo, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID)(0).CurrentValueDec, , , , ObjMachine.HourType)
                                                    Else
                                                        mAssemblyCurrentValue = New Period(mPeriodID, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).AssemblyCurrentValueInDeciaml, , , , ObjMachine.HourType)
                                                    End If

                                                    'tmpCurrentValue = New Period(mPeriodID, Period.Add(ObjCompStatus.CompStatusPeriodList(Count).PeriodID, mPeriodUnitID, ObjCompStatus.CompStatusPeriodList(Count).CompCurrentValueInDecimal, Period.Difference(mAssemblyCurrentValue.DBValue, ObjCompStatus.CompStatusPeriodList(Count).AssemblyCurrentValueInDeciaml)), mPeriodUnitID, False, , ObjMachine.HourType).DbValueDec
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

                                        'Added By Saylee on 25-May-2015 for Taj25052015
                                        'Done On As of Assembly
                                        'If (AppSettings("ClientCode") = "Taj" OR AppSettings("ClientCode") = "HSC" Or AppSettings("ClientCode") = "ASH" ) Then
                                        If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = False) Then
                                            DoneONValueForAssembly = ""
                                        Else
                                            If ObjCompMonitorInspStatus.DoneOn <> "" Then
                                                mDoneONValueForAssembly = New Period(ObjCompMonitorInspStatusPeriod.PeriodID, DBNull.Value)
                                                If ObjCompMonitorInspStatusPeriod.PeriodID <> 2 Then
                                                    Dim mMachineMaintenance As MachineMaintenance = MachineMaintenance.GetMachineMaintenance(ObjCompMonitorInspStatus.ID, MachineMaintenanceActivity.ComponentInspection)

                                                    'Dim mAssemblyCurrentValue As New Period(1, DBNull.Value)
                                                    'If CurrentValueOnAsOnDate.GetCurrentValue(ObjAssemblyStatus.ID, ObjCompMonitorInspStatus.DoneOn, mMachineMaintenance.LogNo, ObjCompMonitorInspStatusPeriod.PeriodID)(0).CurrentValueDec > 0 Then
                                                    '    mAssemblyCurrentValue = New Period(ObjCompMonitorInspStatusPeriod.PeriodID, CurrentValueOnAsOnDate.GetCurrentValue(ObjAssemblyStatus.ID, ObjCompMonitorInspStatus.DoneOn, mMachineMaintenance.LogNo, ObjCompMonitorInspStatusPeriod.PeriodID)(0).CurrentValueDec, , , , ObjMachine.HourType)
                                                    'Else
                                                    '    mAssemblyCurrentValue = New Period(ObjCompMonitorInspStatusPeriod.PeriodID, ObjAssemblyStatus.AssemblyStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").AssemblyCurrentValueInDeciaml, , , , ObjMachine.HourType)
                                                    'End If

                                                    'Dim IsAsOnDateGreater As Boolean = False
                                                    'If CurrentValueOnAsOnDate.GetCompCurrentValue(ObjCompStatus.CompID, ObjAssemblyStatus.ID, ObjCompMonitorInspStatus.DoneOn, mMachineMaintenance.LogNo, ObjCompMonitorInspStatusPeriod.PeriodID).Count > 0 Then
                                                    '    tmpCurrentValue = New Period(ObjCompMonitorInspStatusPeriod.PeriodID, Period.Add(ObjCompMonitorInspStatusPeriod.PeriodID, ObjCompMonitorInspStatusPeriod.PeriodUnitID, CurrentValueOnAsOnDate.GetCompCurrentValue(ObjCompStatus.CompID, ObjAssemblyStatus.ID, ObjCompMonitorInspStatus.DoneOn, mMachineMaintenance.LogNo, ObjCompMonitorInspStatusPeriod.PeriodID)(0).CompCurrentValueDec, Period.Difference(mAssemblyCurrentValue.DBValue, CurrentValueOnAsOnDate.GetCompCurrentValue(ObjCompStatus.CompID, ObjAssemblyStatus.ID, ObjCompMonitorInspStatus.DoneOn, mMachineMaintenance.LogNo, ObjCompMonitorInspStatusPeriod.PeriodID)(0).AssemblyCurrentValueDec)), ObjCompMonitorInspStatusPeriod.PeriodUnitID, False, , ObjMachine.HourType).DbValueDec
                                                    'Else
                                                    '    'here check if compliance before AsOnDate
                                                    '    If CDate(ObjCompMonitorInspStatus.DoneOn.ToString) >= CDate(ObjAssemblyStatus.AsOnDate.ToString) Then
                                                    '        tmpCurrentValue = New Period(ObjCompMonitorInspStatusPeriod.PeriodID, Period.Add(ObjCompMonitorInspStatusPeriod.PeriodID, ObjCompMonitorInspStatusPeriod.PeriodUnitID, ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").CompCurrentValueInDecimal, Period.Difference(mAssemblyCurrentValue.DBValue, ObjCompStatus.CompStatusPeriodList(ObjCompMonitorInspStatusPeriod.PeriodID, "").AssemblyCurrentValueInDeciaml)), ObjCompMonitorInspStatusPeriod.PeriodUnitID, False, , ObjMachine.HourType).DbValueDec
                                                    '    Else
                                                    '        tmpCurrentValue = 0
                                                    '        IsAsOnDateGreater = True  'if compliance before AsOnDate
                                                    '    End If

                                                    'End If

                                                    'mDoneONValueForAssembly = New Period(ObjCompMonitorInspStatusPeriod.PeriodID, tmpCurrentValue, ObjCompMonitorInspStatusPeriod.PeriodUnitID, , , ObjMachine.HourType)

                                                    'If IsAsOnDateGreater = False Then
                                                    '    If DoneONValueForAssembly = "" Then
                                                    '        DoneONValueForAssembly = mDoneONValueForAssembly.TextFormatted
                                                    '    Else
                                                    '        DoneONValueForAssembly = DoneONValueForAssembly + IIf(IsExcel, Chr(10), vbcrlf) + mDoneONValueForAssembly.TextFormatted
                                                    '    End If
                                                    'End If

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
                                                If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Or (ObjCompMonitorInspStatus.IsApplicable = False And ObjCompMonitorInspStatus.IsCompleted = True) Then
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
                                                    '$$$$$$$$
                                                    'DueAsof = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                    'Added By Prashant 18-Oct-2011

                                                    'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                    'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                    'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf

                                                    If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
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
                                                If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Or (ObjCompMonitorInspStatus.IsApplicable = False And ObjCompMonitorInspStatus.IsCompleted = True) Then
                                                    ElapsedTime1 = ""
                                                    RemainingTime1 = ""
                                                    DueAsof1 = ""
                                                Else
                                                    ElapsedTime1 = ObjCompMonitorInspStatusPeriod.AllElapsedValueFormatted
                                                    RemainingTime1 = ObjCompMonitorInspStatusPeriod.RemainingValueFormatted
                                                    '$$$$$$$$$$$$
                                                    'DueAsof1 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                    'Added By Prashant 18-Oct-2011

                                                    'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                    'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                    'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf

                                                    If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                        DueAsof1 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                    Else
                                                        DueAsof1 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                    End If
                                                    '------------

                                                End If
                                                Extension1 = ObjCompMonitorInspStatusPeriod.ExtensionValueFormatted
                                            End If
                                            If ObjCompMonitorInspStatusPeriod.PeriodID = 3 Or ObjCompMonitorInspStatusPeriod.PeriodID = 4 Or ObjCompMonitorInspStatusPeriod.PeriodID = 5 Or ObjCompMonitorInspStatusPeriod.PeriodID = 6 Or ObjCompMonitorInspStatusPeriod.PeriodID = 7 Or ObjCompMonitorInspStatusPeriod.PeriodID = 8 Or ObjCompMonitorInspStatusPeriod.PeriodID = 12 Or ObjCompMonitorInspStatusPeriod.PeriodID = 13 Or ObjCompMonitorInspStatusPeriod.PeriodID = 14 Then
                                                If Freq3 = "" Then
                                                    Freq3 = ObjCompMonitorInspStatusPeriod.FrequencyValue
                                                    If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Or (ObjCompMonitorInspStatus.IsApplicable = False And ObjCompMonitorInspStatus.IsCompleted = True) Then
                                                        ElapsedTime2 = ""
                                                        RemainingTime2 = ""
                                                        DueAsof2 = ""
                                                        AssemblyDueAsof2 = ""
                                                        SinceNew2 = ""
                                                        TimeSinceNew = ""
                                                    Else
                                                        ElapsedTime2 = ObjCompMonitorInspStatusPeriod.AllElapsedValue
                                                        RemainingTime2 = ObjCompMonitorInspStatusPeriod.RemainingValue
                                                        '$$$$$$$$$$$$$$
                                                        'DueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                        'Added By Prashant 18-Oct-2011

                                                        'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                        'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                        'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf

                                                        If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
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
                                                        'DoneAt2 = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                        TimeSinceNew = ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted
                                                    End If
                                                    Extension2 = ObjCompMonitorInspStatusPeriod.ExtensionValue

                                                    'Commented By Saylee on 13-Nov-2017 as need to show Done On/Start Value also
                                                    'If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = False) Then
                                                    '    DoneAt2 = ""
                                                    'Else
                                                    '    DoneAt2 = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                    'End If
                                                    DoneAt2 = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted

                                                    DiffCompInstDoneOnValue = ObjCompMonitorInspStatusPeriod.DiffCompInstDoneOnValue
                                                Else
                                                    Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.FrequencyValue

                                                    If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Or (ObjCompMonitorInspStatus.IsApplicable = False And ObjCompMonitorInspStatus.IsCompleted = True) Then
                                                        ElapsedTime2 = ""
                                                        RemainingTime2 = ""
                                                        DueAsof2 = ""
                                                        AssemblyDueAsof2 = ""
                                                        SinceNew2 = ""
                                                        TimeSinceNew = ""
                                                    Else
                                                        ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AllElapsedValue
                                                        RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.RemainingValue
                                                        '$$$$$$$$$$$$$
                                                        'DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbcrlf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                        'Added By Prashant 18-Oct-2011

                                                        'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                        'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                        'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf

                                                        If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
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
                                                        ' DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbcrlf) & ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                        TimeSinceNew = TimeSinceNew & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted
                                                    End If
                                                    Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.ExtensionValue

                                                    'Commented By Saylee on 13-Nov-2017 as need to show Done On/Start Value also
                                                    'If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = False) Then
                                                    '    DoneAt2 = ""
                                                    'Else
                                                    '    DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                    'End If
                                                    DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                    '*********************************
                                                    DiffCompInstDoneOnValue = DiffCompInstDoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DiffCompInstDoneOnValue
                                                End If
                                            End If
                                        Else
                                            If ObjCompMonitorInspStatusPeriod.PeriodID = 2 Then        'StartDate
                                                If Freq3 = "" Then
                                                    Freq3 = ObjCompMonitorInspStatusPeriod.FrequencyValueFormatted
                                                    If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Or (ObjCompMonitorInspStatus.IsApplicable = False And ObjCompMonitorInspStatus.IsCompleted = True) Then
                                                        ElapsedTime2 = ""
                                                        RemainingTime2 = ""
                                                        DueAsof2 = ""
                                                        AssemblyDueAsof2 = ""
                                                        'SinceNew2 = ""
                                                    Else
                                                        ElapsedTime2 = ObjCompMonitorInspStatusPeriod.AllElapsedValueFormatted
                                                        RemainingTime2 = ObjCompMonitorInspStatusPeriod.RemainingValueFormatted
                                                        '$$$$$$$$$$$$$$$$$$
                                                        'DueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                        'Added By Prashant 18-Oct-2011

                                                        'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                        'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                        'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf

                                                        If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                            DueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                        Else
                                                            DueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                        End If
                                                        '------------------
                                                        AssemblyDueAsof2 = ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
                                                        'SinceNew2 = ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted
                                                        'SinceNew2 = (ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted - ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted)
                                                        DoneAt2 = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                    End If
                                                    Extension2 = ObjCompMonitorInspStatusPeriod.ExtensionValueFormatted

                                                    'Commented By Saylee on 13-Nov-2017 as need to show Done On/Start Value also
                                                    'If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = False) Then
                                                    '    DoneAt2 = ""
                                                    'Else
                                                    '    DoneAt2 = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                    'End If
                                                    DoneAt2 = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                    '*****************************************
                                                    DiffCompInstDoneOnValue = ObjCompMonitorInspStatusPeriod.DiffCompInstDoneOnValue
                                                Else                                                   'Freq3 <> ""
                                                    Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.FrequencyValueFormatted

                                                    If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Or (ObjCompMonitorInspStatus.IsApplicable = False And ObjCompMonitorInspStatus.IsCompleted = True) Then
                                                        ElapsedTime2 = ""
                                                        RemainingTime2 = ""
                                                        DueAsof2 = ""
                                                        AssemblyDueAsof2 = ""
                                                        'SinceNew2 = ""
                                                    Else
                                                        ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AllElapsedValueFormatted
                                                        RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.RemainingValueFormatted
                                                        '$$$$$$$$$$$$$$$$$
                                                        'DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbcrlf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                        'Added By Prashant 18-Oct-2011

                                                        'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                        'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                        'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf


                                                        If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                            DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                        Else
                                                            DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                        End If
                                                        '-----------------
                                                        AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
                                                        'SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbcrlf) & ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted
                                                        'SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbcrlf) & (ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted - ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted)
                                                        'DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbcrlf) & ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                    End If
                                                    Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.ExtensionValueFormatted

                                                    'Commented By Saylee on 13-Nov-2017 as need to show Done On/Start Value also
                                                    'If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = False) Then
                                                    '    DoneAt2 = ""
                                                    'Else
                                                    '    DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                    'End If
                                                    DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                    '**********************************
                                                    DiffCompInstDoneOnValue = DiffCompInstDoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DiffCompInstDoneOnValue
                                                End If
                                            Else                                                       'For PeriodID <> 2
                                                If Freq3 = "" Then
                                                    Freq3 = ObjCompMonitorInspStatusPeriod.FrequencyValue

                                                    If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Or (ObjCompMonitorInspStatus.IsApplicable = False And ObjCompMonitorInspStatus.IsCompleted = True) Then
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
                                                            'DoneAt2 = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                            TimeSinceNew = ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted
                                                        Else
                                                            '$$$$$$$$$$$$$$
                                                            'DueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                            'Added By Prashant 18-Oct-2011

                                                            'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                            'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                            'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf

                                                            If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
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
                                                            'DoneAt2 = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                            TimeSinceNew = ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted
                                                        End If
                                                    End If
                                                    Extension2 = ObjCompMonitorInspStatusPeriod.ExtensionValue

                                                    'Commented By Saylee on 13-Nov-2017 as need to show Done On/Start Value also
                                                    'If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = False) Then
                                                    '    DoneAt2 = ""
                                                    'Else
                                                    '    DoneAt2 = DoneAt2 & ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                    'End If
                                                    DoneAt2 = DoneAt2 & ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                    '********************************
                                                    DiffCompInstDoneOnValue = ObjCompMonitorInspStatusPeriod.DiffCompInstDoneOnValue
                                                Else                                                   'Freq3 <> ""   
                                                    Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.FrequencyValue

                                                    If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Or (ObjCompMonitorInspStatus.IsApplicable = False And ObjCompMonitorInspStatus.IsCompleted = True) Then
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
                                                            ' DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbcrlf) & ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                            TimeSinceNew = TimeSinceNew & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted
                                                        Else
                                                            '$$$$$$$$$$$$$$$$$
                                                            'DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbcrlf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                            'Added By Prashant 18-Oct-2011

                                                            'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                            'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                            'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf

                                                            If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013z
                                                                DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                            Else
                                                                DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                            End If
                                                            '-----------------
                                                            AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
                                                            If ObjCompMonitorInspStatus.MonitorTypeID = 3 And ObjCompMonitorInspStatusPeriod.DoneOnValue = "" Then
                                                                SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted
                                                            Else
                                                                SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DiffCompCurrentDoneOnValueFormatted '--------------------------
                                                            End If
                                                            ' DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbcrlf) & ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                            TimeSinceNew = TimeSinceNew & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted
                                                        End If
                                                    End If
                                                    Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.ExtensionValue

                                                    'Commented By Saylee on 13-Nov-2017 as need to show Done On/Start Value also
                                                    'If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = False) Then
                                                    '    DoneAt2 = ""
                                                    'Else
                                                    '    DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                    'End If
                                                    DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                    '***********************
                                                    DiffCompInstDoneOnValue = DiffCompInstDoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatusPeriod.DiffCompInstDoneOnValue
                                                End If
                                            End If
                                        End If
                                    Next

                                    If chkNotMonitoredValues.Checked = True Then 'Added by Saylee on 11-May-2015 for ALL11052015
                                        If DoneONValueForNoPeriod <> "" Then 'Added by Saylee on 19-Sep-2014 for ALL19092014
                                            If DoneAt2 = "" Then
                                                DoneAt2 = DoneONValueForNoPeriod
                                            Else
                                                DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & DoneONValueForNoPeriod
                                            End If
                                        End If
                                    End If

                                    'Added By Saylee on 25-May-2015 for Taj25052015
                                    'If (AppSettings("ClientCode") = "Taj" OR AppSettings("ClientCode") = "HSC" Or AppSettings("ClientCode") = "ASH" ) Then
                                    '    DoneAt2 = DoneONValueForAssembly
                                    '    DoneONValueForAssembly = ""
                                    'Else
                                    '    DoneAt2 = DoneAt2
                                    'End If

                                    AssemblyID = ObjAssemblyStatus.AssemblyID
                                    Note = ObjCompMonitorInspStatus.Notes
                                    Remark = ObjCompMonitorInspStatus.DoneRemark
                                    ExtensionDate = ObjCompMonitorInspStatus.ExtensionDate
                                    ApprovalRemark = ObjCompMonitorInspStatus.ApprovalRemark
                                    Reference = ObjCompMonitorInspStatus.Reference  'Added By Prashant 3-Apr-2013  'Indamer03042013
                                    Dim ATACode = ObjCompMonitorInspStatus.ATACode
                                    SourceDoc = ObjCompMonitorInspStatus.SourceDoc

                                    If IsExcel Then

                                        If ATACode.ToString.Length < 3 Then
                                            ATAChapter = ATACode.ToString.PadLeft(3, "0"c) + " " + "-" + " " + ObjCompMonitorInspStatus.ATANomenclature
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

                                    ''Added By Saylee on 2-Aug-2024
                                    If ObjCompMonitorInspStatus.NonMonitoringPeriodDetails <> "" Then
                                        DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatus.NonMonitoringPeriodDetails.Replace("<br>", vbCrLf)
                                    End If
                                    '*******************************************
                                    If ObjCompMonitorInspStatus.IsApplicable = False Then
                                        DueAsof = ""
                                        DueAsof1 = ""
                                        DueAsof2 = ""
                                        RemainingTime = ""
                                        RemainingTime1 = ""
                                        RemainingTime2 = ""
                                    End If

                                    ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, MonitorType, MonitorTypeCode, Note, Remark, Description,
                 , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2,
                 DueAsof, DueAsof1, DueAsof2, AssemblyModel, , , SinceNew2, , , DoneAt2, , , , ObjCompMonitorInspStatus.ATACode, , , , StartDateData, , , , , , , , , , Reference, , DoneOnDate, , , , , , AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate,
                 ApprovalRemark, , , Code, StatusMasterID.ToString, , , , , , ObjCompMonitorInspStatus.MonitorTypeID, , , , , , , TimeSinceNew, DoneONValueForAssembly:=DoneONValueForAssembly, SourceDoc:=SourceDoc, DiffCompInstDoneOnValue:=DiffCompInstDoneOnValue, RecordID:=RecordOwnID))
                                End If
                            Next
                        Next
                    End If
                Next
            Next
            'End If
            'Next
        End If

        If IsModSelect = True Then
            'For i As Integer = 0 To ListDirectiveType.Items.Count - 1
            '    If ListDirectiveType.Items.Item(i).Selected Then
            'Added By Utkarsh ON 12-Sep-2013 FOR BA11092013  parameters (ByPerdayLimit,PerDayLimits ,IsAverageRequired)
            mMachineList = MachineList.GetMachineListMonitoringStatus(AsonDate, MachineName, , , , , , , , , , IIf(chkComponent.Checked, True, False), True, , AssemblyName, , , , , , , , , ATACode, ATANomenclature, ShowCofA, , , , True, , , , , , , False, , False, , True, , , , , 0, , , True, IsAverageRequired:=mIsAverageRequired, ByPerDayLimit:=mByPerDayLimit, PerdayLimits:=mPerDayLimits, SkipIsForInventoryAircarft:=True, MonitorServiceTypeIDs:=ServiceTypeIds.ToString.TrimEnd(","), MonitorInspTypeIDs:=InspTypeIds.ToString.TrimEnd(","), MonitorModTypeIDs:=ModTypeIds.ToString.TrimEnd(","))
            For Each ObjMachine In mMachineList
                For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                    If chkAssembly.Checked Then


                        For Each ObjAssemblyMonitorModStatus In ObjAssemblyStatus.AssemblyMonitorModStatusList
                            'If (ObjAssemblyMonitorModStatus.IsApplicable = True) Or (ObjAssemblyMonitorModStatus.IsApplicable = False And ObjAssemblyMonitorModStatus.IsCompleted = True) Then
                            ATAChapter = ObjAssemblyMonitorModStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorModStatus.ATANomenclature
                            Description = ObjAssemblyMonitorModStatus.Description
                            Position = ObjAssemblyStatus.Position
                            MonitorTypeCode = ObjAssemblyMonitorModStatus.Code
                            MonitorType = ObjAssemblyMonitorModStatus.Type
                            AssemblyModel = ObjAssemblyStatus.Model
                            AssemblySerialNo = ObjAssemblyStatus.SerialNo
                            'Added By Utkarsh ON 12-Sep-2013 FOR BA11092013
                            EstimatedDate = ObjAssemblyMonitorModStatus.EstimatedDateFormatted
                            Code = ObjAssemblyMonitorModStatus.ModelMonitorModCode
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
                            DoneOnValue = ""
                            Extension = ""
                            Extension1 = ""
                            Extension2 = ""
                            SinceNew2 = ""
                            DoneAt2 = ""
                            StartDateData = ""
                            TimeSinceNew = ""
                            Periodcount = ObjAssemblyStatus.AssemblyStatusPeriodList.Count()
                            Dim IsPeriod2Exists As Boolean = False

                            'Added by Saylee on 19-Sep-2014 for ALL19092014
                            Dim mDoneONValueForNoPeriod As Period = New Period(1, DBNull.Value)
                            Dim mTSOValueForNoPeriod As Period = New Period(1, DBNull.Value)
                            Dim mPeriodID As Integer = 0
                            DoneONValueForNoPeriod = String.Empty
                            TSOValueForNoPeriod = String.Empty

                            Dim mDoneONValueForAssembly As Period = New Period(1, DBNull.Value)
                            DoneONValueForAssembly = String.Empty
                            DoneONValueForAssembly = ""

                            DiffCompInstDoneOnValue = ""

                            For Count = 0 To Periodcount - 1
                                ''If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 2 And ObjAssemblyMonitorModStatus.DoneOn = "" Then
                                ''    StartDateLabel = CType(IIf(StartDateLabel = "", StartDateLabel, StartDateLabel +  IIf(IsExcel, Chr(10), vbcrlf)), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                                ''    StartDateData = CType(IIf(StartDateData = "", StartDateData, StartDateData +  IIf(IsExcel, Chr(10), vbcrlf)), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyStartValueFormatted
                                ''Else
                                ''    StartDateLabel = CType(IIf(StartDateLabel = "", StartDateLabel, StartDateLabel +  IIf(IsExcel, Chr(10), vbcrlf)), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName 'Added by Saylee on 31-May-2010
                                ''    StartDateData = CType(IIf(StartDateData = "", StartDateData, StartDateData +  IIf(IsExcel, Chr(10), vbcrlf)), String) + ObjAssemblyMonitorModStatus.DoneOnFormatted 'Added by Saylee on 31-May-2010
                                ''End If

                                'Added By Saylee on 22-Jun-2011
                                If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 2 Then
                                    If ObjAssemblyMonitorModStatus.DoneOn = "" Then
                                        For Each tmpObjAssemblyMonitorModStatusPeriod As AssemblyMonitorModStatusPeriodInfo In ObjAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriodList
                                            If tmpObjAssemblyMonitorModStatusPeriod.PeriodID = 2 Then
                                                IsPeriod2Exists = True
                                                Exit For
                                            End If
                                        Next
                                        If IsPeriod2Exists = True Then
                                            For Each ObjAssemblyMonitorModStatusPeriod In ObjAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriodList
                                                If ObjAssemblyMonitorModStatusPeriod.PeriodID = 2 Then
                                                    StartDateLabel = CType(IIf(StartDateLabel = "", StartDateLabel, StartDateLabel + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                                                    StartDateData = CType(IIf(StartDateData = "", StartDateData, StartDateData + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted
                                                End If
                                            Next
                                        Else
                                            StartDateLabel = CType(IIf(StartDateLabel = "", StartDateLabel, StartDateLabel + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                                            StartDateData = CType(IIf(StartDateData = "", StartDateData, StartDateData + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyStartValueFormatted
                                        End If

                                    Else
                                        StartDateLabel = CType(IIf(StartDateLabel = "", StartDateLabel, StartDateLabel + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
                                        StartDateData = CType(IIf(StartDateData = "", StartDateData, StartDateData + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjAssemblyMonitorModStatus.DoneOnFormatted
                                    End If
                                End If

                                'Added by Saylee on 19-Sep-2014 for ALL19092014
                                'If No Cycle/Hour Period Present in Monitor Insp       
                                mDoneONValueForNoPeriod = New Period(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, DBNull.Value)
                                mTSOValueForNoPeriod = New Period(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, DBNull.Value)

                                mDoneONValueForAssembly = New Period(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, DBNull.Value)

                                If (ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 1 Or ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 3) And Not (ObjAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriodList.Contains(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID)) Then

                                    Dim mPeriodUnitID As Integer = 0
                                    If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 1 Then
                                        mPeriodUnitID = 1
                                    ElseIf ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID = 3 Then
                                        mPeriodUnitID = 6
                                    End If
                                    mPeriodID = ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID
                                    If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = False) Then
                                        DoneONValueForNoPeriod = ""
                                    Else
                                        If ObjAssemblyMonitorModStatus.DoneOn <> "" Then
                                            'If CurrentValue.GetCurrentValue(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).ID, ObjAssemblyMonitorModStatus.DoneOn, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID).Count > 0 Then
                                            Dim mMachineMaintenance As MachineMaintenance = MachineMaintenance.GetMachineMaintenance(ObjAssemblyMonitorModStatus.ID, MachineMaintenanceActivity.AssemblyDirective)
                                            If CurrentValueOnAsOnDate.GetCurrentValue(mMachineMaintenance.LogAssemblyStatusID, ObjAssemblyMonitorModStatus.DoneOn, mMachineMaintenance.LogNo, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID)(0).CurrentValueDec > 0 Then

                                                mDoneONValueForNoPeriod.Value = CurrentValueOnAsOnDate.GetCurrentValue(mMachineMaintenance.LogAssemblyStatusID, ObjAssemblyMonitorModStatus.DoneOn, mMachineMaintenance.LogNo, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID)(0).CurrentValue

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
                            For Each ObjAssemblyMonitorModStatusPeriod In ObjAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriodList
                                'Added By Saylee on 25-May-2015 for Taj25052015
                                'Done On As of Assembly
                                'If (AppSettings("ClientCode") = "Taj" OR AppSettings("ClientCode") = "HSC" Or AppSettings("ClientCode") = "ASH" ) Then
                                If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = False) Then
                                    DoneONValueForAssembly = ""
                                Else
                                    If ObjAssemblyMonitorModStatus.DoneOn <> "" Then
                                        mDoneONValueForAssembly = New Period(ObjAssemblyMonitorModStatusPeriod.PeriodID, DBNull.Value)

                                        Dim mMachineMaintenance As MachineMaintenance = MachineMaintenance.GetMachineMaintenance(ObjAssemblyMonitorModStatus.ID, MachineMaintenanceActivity.AssemblyDirective)
                                        If CurrentValueOnAsOnDate.GetCurrentValue(mMachineMaintenance.LogAssemblyStatusID, ObjAssemblyMonitorModStatus.DoneOn, mMachineMaintenance.LogNo, ObjAssemblyMonitorModStatusPeriod.PeriodID)(0).CurrentValueDec > 0 Then
                                            mDoneONValueForAssembly.Value = CurrentValueOnAsOnDate.GetCurrentValue(ObjAssemblyStatus.ID, ObjAssemblyMonitorModStatus.DoneOn, mMachineMaintenance.LogNo, ObjAssemblyMonitorModStatusPeriod.PeriodID)(0).CurrentValue

                                            If DoneONValueForAssembly = "" Then
                                                DoneONValueForAssembly = mDoneONValueForAssembly.TextFormatted
                                            Else
                                                DoneONValueForAssembly = DoneONValueForAssembly + IIf(IsExcel, Chr(10), vbCrLf) + mDoneONValueForAssembly.TextFormatted
                                            End If
                                        End If
                                    End If
                                End If
                                ' End If
                                '**************************
                                If ReportStatus = 0 Then 'Landscape
                                    If ObjAssemblyMonitorModStatusPeriod.PeriodID = 1 Then
                                        Freq1 = ObjAssemblyMonitorModStatusPeriod.FrequencyValue
                                        If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True) Or (ObjAssemblyMonitorModStatus.IsApplicable = False And ObjAssemblyMonitorModStatus.IsCompleted = True) Then
                                            ElapsedTime = ""
                                            RemainingTime = ""
                                            DueAsof = ""
                                            'Added By Prashant 04-Aug-2009
                                            SinceNew2 = ""
                                            '------------------------------
                                            TimeSinceNew = ""
                                        Else
                                            ElapsedTime = ObjAssemblyMonitorModStatusPeriod.AllElapsedValue
                                            RemainingTime = ObjAssemblyMonitorModStatusPeriod.RemainingValue
                                            'Added By Prashant 17-Sep-2013

                                            'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                            'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                            'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf

                                            If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                DueAsof = DueAsof + ObjAssemblyMonitorModStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame & vbCrLf
                                            Else
                                                DueAsof = DueAsof + ObjAssemblyMonitorModStatusPeriod.DueOnValue & vbCrLf
                                            End If

                                            'Added By Prashant 04-Aug-2009
                                            If ObjAssemblyMonitorModStatus.MonitorTypeID = 3 And ObjAssemblyMonitorModStatusPeriod.DoneOnValue = "" Then
                                                SinceNew2 = ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValue
                                            Else
                                                SinceNew2 = ObjAssemblyMonitorModStatusPeriod.DiffAssemblyCurrentDoneOnValue
                                            End If
                                            '------------------------------
                                            TimeSinceNew = ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValue
                                        End If
                                        DoneOnValue = DoneOnValue + ObjAssemblyMonitorModStatusPeriod.DoneOnValue & vbCrLf
                                        Extension = ObjAssemblyMonitorModStatusPeriod.ExtensionValue
                                    End If
                                    If ObjAssemblyMonitorModStatusPeriod.PeriodID = 2 Then
                                        Freq2 = ObjAssemblyMonitorModStatusPeriod.FrequencyValueFormatted


                                        If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True) Or (ObjAssemblyMonitorModStatus.IsApplicable = False And ObjAssemblyMonitorModStatus.IsCompleted = True) Then
                                            ElapsedTime1 = ""
                                            RemainingTime1 = ""
                                            DueAsof = ""
                                        Else
                                            ElapsedTime1 = ObjAssemblyMonitorModStatusPeriod.AllElapsedValueFormatted
                                            RemainingTime1 = ObjAssemblyMonitorModStatusPeriod.RemainingValueFormatted
                                            DueAsof = DueAsof + ObjAssemblyMonitorModStatusPeriod.DueOnValueFormatted & vbCrLf
                                        End If
                                        DoneOnValue = DoneOnValue + ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted & vbCrLf
                                        Extension1 = ObjAssemblyMonitorModStatusPeriod.ExtensionValueFormatted
                                    End If
									'If ObjAssemblyMonitorModStatusPeriod.PeriodID = 3 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 4 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 5 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 6 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 7 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 8 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 12 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 13 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 14 Then
									'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
									If ObjAssemblyMonitorModStatusPeriod.PeriodID >= 3 Then
										If Freq3 = "" Then
											Freq3 = ObjAssemblyMonitorModStatusPeriod.FrequencyValue


											If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True) Or (ObjAssemblyMonitorModStatus.IsApplicable = False And ObjAssemblyMonitorModStatus.IsCompleted = True) Then
												ElapsedTime2 = ""
												RemainingTime2 = ""
												DueAsof = ""
												SinceNew2 = ""
												TimeSinceNew = ""
											Else
												ElapsedTime2 = ObjAssemblyMonitorModStatusPeriod.AllElapsedValue
												RemainingTime2 = ObjAssemblyMonitorModStatusPeriod.RemainingValue
												'Added By Prashant 17-Sep-2013

												'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
												'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
												'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf

												If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
													DueAsof = DueAsof + ObjAssemblyMonitorModStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame & vbCrLf
												Else
													DueAsof = DueAsof + ObjAssemblyMonitorModStatusPeriod.DueOnValue & vbCrLf
												End If

												If ObjAssemblyMonitorModStatus.MonitorTypeID = 3 And ObjAssemblyMonitorModStatusPeriod.DoneOnValue = "" Then
													SinceNew2 = ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValue
												Else
													SinceNew2 = ObjAssemblyMonitorModStatusPeriod.DiffAssemblyCurrentDoneOnValue
												End If
												'DoneAt2 = ObjAssemblyMonitorModStatusPeriod.DoneOnValue
												TimeSinceNew = ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValue
											End If
											DoneOnValue = DoneOnValue + ObjAssemblyMonitorModStatusPeriod.DoneOnValue & vbCrLf
											Extension2 = ObjAssemblyMonitorModStatusPeriod.ExtensionValue

											'Commented By Saylee on 13-Nov-2017 as need to show Done On/Start Value also
											'If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = False) Then
											'    DoneAt2 = ""
											'Else
											'    DoneAt2 = ObjAssemblyMonitorModStatusPeriod.DoneOnValue
											'End If
											DoneAt2 = ObjAssemblyMonitorModStatusPeriod.DoneOnValue
											'*****************************
										Else                                                           'Freq3 <> ""
											Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.FrequencyValue


											If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True) Or (ObjAssemblyMonitorModStatus.IsApplicable = False And ObjAssemblyMonitorModStatus.IsCompleted = True) Then
												ElapsedTime2 = ""
												RemainingTime2 = ""
												DueAsof2 = ""
												DueAsof = ""
												AssemblyDueAsof2 = ""
												SinceNew2 = ""
												TimeSinceNew = ""
											Else
												ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.AllElapsedValue
												RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.RemainingValue
												'Added By Prashant 17-Sep-2013

												'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
												'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
												'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf

												If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
													DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
													DueAsof = DueAsof + ObjAssemblyMonitorModStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame & vbCrLf
												Else
													DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.DueOnValue
													DueAsof = DueAsof + ObjAssemblyMonitorModStatusPeriod.DueOnValue & vbCrLf
												End If

												AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.DueOnValue
												If ObjAssemblyMonitorModStatus.MonitorTypeID = 3 And ObjAssemblyMonitorModStatusPeriod.DoneOnValue = "" Then
													SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValue
												Else
													SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.DiffAssemblyCurrentDoneOnValue
												End If
												'DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbcrlf) & ObjAssemblyMonitorModStatusPeriod.DoneOnValue
												TimeSinceNew = TimeSinceNew & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValue
											End If
											DoneOnValue = DoneOnValue + ObjAssemblyMonitorModStatusPeriod.DoneOnValue & vbCrLf
											Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.ExtensionValue

											'Commented By Saylee on 13-Nov-2017 as need to show Done On/Start Value also
											'If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = False) Then
											'    DoneAt2 = ""
											'Else
											'    DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.DoneOnValue
											'End If
											DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.DoneOnValue
											'*******************************
										End If
									End If
								Else
                                    If ObjAssemblyMonitorModStatusPeriod.PeriodID = 2 Then             'StartDate
                                        If Freq3 = "" Then
                                            Freq3 = ObjAssemblyMonitorModStatusPeriod.FrequencyValueFormatted

                                            If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True) Or (ObjAssemblyMonitorModStatus.IsApplicable = False And ObjAssemblyMonitorModStatus.IsCompleted = True) Then
                                                ElapsedTime2 = ""
                                                RemainingTime2 = ""
                                                DueAsof2 = ""
                                                AssemblyDueAsof2 = ""
                                                'SinceNew2 = ""
                                            Else
                                                ElapsedTime2 = ObjAssemblyMonitorModStatusPeriod.AllElapsedValueFormatted
                                                RemainingTime2 = ObjAssemblyMonitorModStatusPeriod.RemainingValueFormatted
                                                DueAsof2 = ObjAssemblyMonitorModStatusPeriod.DueOnValueFormatted
                                                AssemblyDueAsof2 = ObjAssemblyMonitorModStatusPeriod.DueOnValueFormatted
                                                'SinceNew2 = ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValueFormatted
                                                'SinceNew2 = (ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValueFormatted - ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted)
                                                'DoneAt2 = ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted
                                            End If
                                            DoneOnValue = ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted
                                            Extension2 = ObjAssemblyMonitorModStatusPeriod.ExtensionValueFormatted


                                            'Commented By Saylee on 13-Nov-2017 as need to show Done On/Start Value also
                                            'If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = False) Then
                                            '    DoneAt2 = ""
                                            'Else
                                            '    DoneAt2 = ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted
                                            'End If
                                            DoneAt2 = ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted
                                            '******************************************
                                        Else                                                           'Freq3 <> ""
                                            Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.FrequencyValueFormatted

                                            If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True) Or (ObjAssemblyMonitorModStatus.IsApplicable = False And ObjAssemblyMonitorModStatus.IsCompleted = True) Then
                                                ElapsedTime2 = ""
                                                RemainingTime2 = ""
                                                DueAsof2 = ""
                                                AssemblyDueAsof2 = ""
                                                'SinceNew2 = ""
                                            Else
                                                ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.AllElapsedValueFormatted
                                                RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.RemainingValueFormatted
                                                'Added By Prashant 17-Sep-2013

                                                'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf

                                                If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                    DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                Else
                                                    DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.DueOnValueFormatted
                                                End If

                                                AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.DueOnValueFormatted
                                                'SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbcrlf) & ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValueFormatted
                                                'SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbcrlf) & (ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValueFormatted - ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted)
                                                'DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbcrlf) & ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted
                                            End If
                                            DoneOnValue = DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted
                                            Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.ExtensionValueFormatted

                                            'Commented By Saylee on 13-Nov-2017 as need to show Done On/Start Value also
                                            'If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = False) Then
                                            '    DoneAt2 = ""
                                            'Else
                                            '    DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted
                                            'End If
                                            DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted
                                            '*****************************
                                        End If
                                    Else                                                               'For PeriodID <> 2
                                        If Freq3 = "" Then
                                            Freq3 = ObjAssemblyMonitorModStatusPeriod.FrequencyValue

                                            If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True) Or (ObjAssemblyMonitorModStatus.IsApplicable = False And ObjAssemblyMonitorModStatus.IsCompleted = True) Then
                                                ElapsedTime2 = ""
                                                RemainingTime2 = ""
                                                DueAsof2 = ""
                                                AssemblyDueAsof2 = ""
                                                SinceNew2 = ""
                                                TimeSinceNew = ""
                                            Else
                                                ElapsedTime2 = ObjAssemblyMonitorModStatusPeriod.AllElapsedValue
                                                RemainingTime2 = ObjAssemblyMonitorModStatusPeriod.RemainingValue
                                                'Added By Prashant 17-Sep-2013

                                                'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf

                                                If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                    DueAsof2 = ObjAssemblyMonitorModStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                Else
                                                    DueAsof2 = ObjAssemblyMonitorModStatusPeriod.DueOnValue
                                                End If
                                                AssemblyDueAsof2 = ObjAssemblyMonitorModStatusPeriod.DueOnValue
                                                If ObjAssemblyMonitorModStatus.MonitorTypeID = 3 And ObjAssemblyMonitorModStatusPeriod.DoneOnValue = "" Then
                                                    SinceNew2 = ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValue
                                                Else
                                                    SinceNew2 = ObjAssemblyMonitorModStatusPeriod.DiffAssemblyCurrentDoneOnValue
                                                End If
                                                'DoneAt2 = ObjAssemblyMonitorModStatusPeriod.DoneOnValue
                                                TimeSinceNew = ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValue
                                            End If
                                            DoneOnValue = ObjAssemblyMonitorModStatusPeriod.DoneOnValue
                                            Extension2 = ObjAssemblyMonitorModStatusPeriod.ExtensionValue

                                            'Commented By Saylee on 13-Nov-2017 as need to show Done On/Start Value also
                                            'If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = False) Then
                                            '    DoneAt2 = ""
                                            'Else
                                            '    DoneAt2 = ObjAssemblyMonitorModStatusPeriod.DoneOnValue
                                            'End If
                                            DoneAt2 = ObjAssemblyMonitorModStatusPeriod.DoneOnValue

                                        Else                                                           'Freq3 <> ""
                                            Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.FrequencyValue

                                            If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True) Or (ObjAssemblyMonitorModStatus.IsApplicable = False And ObjAssemblyMonitorModStatus.IsCompleted = True) Then
                                                ElapsedTime2 = ""
                                                RemainingTime2 = ""
                                                DueAsof2 = ""
                                                AssemblyDueAsof2 = ""
                                                SinceNew2 = ""
                                                'DoneAt2 = ""
                                                TimeSinceNew = ""
                                            Else
                                                ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.AllElapsedValue
                                                RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.RemainingValue
                                                'Added By Prashant 17-Sep-2013

                                                'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf

                                                If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                    DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                Else
                                                    DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.DueOnValue
                                                End If

                                                AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.DueOnValue
                                                If ObjAssemblyMonitorModStatus.MonitorTypeID = 3 And ObjAssemblyMonitorModStatusPeriod.DoneOnValue = "" Then
                                                    SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValue
                                                Else
                                                    SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.DiffAssemblyCurrentDoneOnValue
                                                End If
                                                'DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.DoneOnValue
                                                TimeSinceNew = TimeSinceNew & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValue
                                            End If
                                            DoneOnValue = DoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.DoneOnValue
                                            Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.ExtensionValue

                                            'Commented By Saylee on 13-Nov-2017 as need to show Done On/Start Value also
                                            'If (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = False) Then
                                            '    DoneAt2 = ""
                                            'Else
                                            '    DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.DoneOnValue
                                            'End If
                                            DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatusPeriod.DoneOnValue
                                            '****************************
                                        End If
                                    End If
                                End If
                            Next

                            If chkNotMonitoredValues.Checked = True Then 'Added by Saylee on 11-May-2015 for ALL11052015
                                If DoneONValueForNoPeriod <> "" Then 'Added by Saylee on 19-Sep-2014 for ALL19092014
                                    If DoneAt2 = "" Then
                                        DoneAt2 = DoneONValueForNoPeriod
                                    Else
                                        DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & DoneONValueForNoPeriod
                                    End If
                                End If
                            End If

                            '''Added By Saylee on 25-May-2015 for Taj25052015
                            ''If (AppSettings("ClientCode") = "Taj" OR AppSettings("ClientCode") = "HSC" Or AppSettings("ClientCode") = "ASH" ) Then
                            ''    DoneAt2 = DoneONValueForAssembly
                            ''    DoneONValueForAssembly = ""
                            ''Else
                            ''    DoneAt2 = DoneAt2
                            ''End If

                            AssemblyID = ObjAssemblyStatus.AssemblyID
                            Note = ObjAssemblyMonitorModStatus.Notes
                            Remark = ObjAssemblyMonitorModStatus.DoneRemark
                            Number = ObjAssemblyMonitorModStatus.Number
                            Reference = ObjAssemblyMonitorModStatus.Reference
                            DoneOnDate = ObjAssemblyMonitorModStatus.DoneOn
                            DoneWONo = ObjAssemblyMonitorModStatus.DoneWONo
                            ExtensionDate = ObjAssemblyMonitorModStatus.ExtensionDate
                            ApprovalRemark = ObjAssemblyMonitorModStatus.ApprovalRemark
                            SourceDoc = ObjAssemblyMonitorModStatus.SourceDoc

                            Dim ATACode = ObjAssemblyMonitorModStatus.ATACode
                            If IsExcel Then
                                If ATACode.ToString.Length < 3 Then
                                    ATAChapter = ATACode.ToString.PadLeft(3, "0"c) + " " + "-" + " " + ObjAssemblyMonitorModStatus.ATANomenclature
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

                            ''Added By Saylee on 2-Aug-2024
                            If ObjAssemblyMonitorModStatus.NonMonitoringPeriodDetails <> "" Then
                                DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatus.NonMonitoringPeriodDetails.Replace("<br>", vbCrLf)
                            End If
                            '*******************************************
                            If ObjAssemblyMonitorModStatus.IsApplicable = False Then
                                DueAsof = ""
                                DueAsof1 = ""
                                DueAsof2 = ""
                                RemainingTime = ""
                                RemainingTime1 = ""
                                RemainingTime2 = ""
                            End If

                            ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, , , Position, MonitorType, MonitorTypeCode, Note, Remark, Description,
                                                                   , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2,
                                                                  AssemblyModel, , , SinceNew2, , , DoneAt2, , , , ObjAssemblyMonitorModStatus.ATACode, , , , StartDateData, , , , , , , , , Number, Reference, DoneOnValue, DoneOnDate, DoneWONo, , , , , AssemblyDueAsof2,
                                                                  Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, , , Code, , , , , , , ObjAssemblyMonitorModStatus.MonitorTypeID, , , , , , , TimeSinceNew, DoneONValueForAssembly:=DoneONValueForAssembly, SourceDoc:=SourceDoc))
                        Next
                    End If
                    If chkComponent.Checked Then


                        For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
                            For Each ObjCompMonitorModStatus In ObjCompStatus.CompMonitorModStatusList
                                'If (ObjCompMonitorModStatus.IsApplicable = True) Or (ObjCompMonitorModStatus.IsApplicable = False And ObjCompMonitorModStatus.IsCompleted = True) Then
                                ATAChapter = ObjCompMonitorModStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorModStatus.ATANomenclature
                                Description = ObjCompMonitorModStatus.Description
                                PartNo = ObjCompStatus.PartName
                                CompSerialNo = ObjCompStatus.CompSerialNo
                                Position = ObjCompStatus.Position
                                MonitorTypeCode = ObjCompMonitorModStatus.Code
                                MonitorType = ObjCompMonitorModStatus.Type
                                AssemblyModel = ObjAssemblyStatus.Model
                                AssemblySerialNo = ObjAssemblyStatus.SerialNo
                                Periodcount = ObjCompStatus.CompStatusPeriodList.Count()
                                'Added By Utkarsh ON 12-Sep-2013 FOR BA11092013
                                EstimatedDate = ObjCompMonitorModStatus.EstimatedDateFormatted
                                Code = ObjCompMonitorModStatus.PartMonitorModCode
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
                                DoneOnValue = ""
                                Extension = ""
                                Extension1 = ""
                                Extension2 = ""
                                AssemblyDueAsof2 = ""
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

                                Dim mDoneONValueForAssembly As Period = New Period(1, DBNull.Value)
                                DoneONValueForAssembly = String.Empty

                                DiffCompInstDoneOnValue = ""
                                For Count = 0 To Periodcount - 1
                                    If ObjCompStatus.CompStatusPeriodList(Count).PeriodID = 2 Then
                                        If ObjCompMonitorModStatus.DoneOn = "" Then
                                            For Each tmpObjCompMonitorModStatusPeriod As CompMonitorModStatusPeriodInfo In ObjCompMonitorModStatus.CompMonitorModStatusPeriodList
                                                If tmpObjCompMonitorModStatusPeriod.PeriodID = 2 Then
                                                    IsPeriod2Exists = True
                                                    Exit For
                                                End If
                                            Next
                                            If IsPeriod2Exists = True Then
                                                For Each ObjCompMonitorModStatusPeriod In ObjCompMonitorModStatus.CompMonitorModStatusPeriodList
                                                    If ObjCompMonitorModStatusPeriod.PeriodID = 2 Then
                                                        StartDateLabel = CType(IIf(StartDateLabel = "", StartDateLabel, StartDateLabel + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjCompStatus.CompStatusPeriodList(Count).PeriodName
                                                        'StartDateData = CType(IIf(StartDateData = "", StartDateData, StartDateData +  IIf(IsExcel, Chr(10), vbcrlf)), String) + ObjCompStatus.CompStatusPeriodList(ObjCompStatus.CompStatusPeriodList(Count).PeriodID, "").CompStartValueFormatted
                                                        StartDateData = CType(IIf(StartDateData = "", StartDateData, StartDateData + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjCompMonitorModStatusPeriod.DoneOnValueFormatted
                                                    End If
                                                Next
                                            Else
                                                StartDateLabel = CType(IIf(StartDateLabel = "", StartDateLabel, StartDateLabel + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjCompStatus.CompStatusPeriodList(Count).PeriodName
                                                StartDateData = CType(IIf(StartDateData = "", StartDateData, StartDateData + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjCompStatus.CompStatusPeriodList(ObjCompStatus.CompStatusPeriodList(Count).PeriodID, "").CompStartValueFormatted 'Added by Saylee on 31-May-2010
                                            End If
                                        Else
                                            StartDateLabel = CType(IIf(StartDateLabel = "", StartDateLabel, StartDateLabel + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjCompStatus.CompStatusPeriodList(Count).PeriodName
                                            StartDateData = CType(IIf(StartDateData = "", StartDateData, StartDateData + IIf(IsExcel, Chr(10), vbCrLf)), String) + ObjCompMonitorModStatus.DoneOnFormatted 'Added by Saylee on 31-May-2010
                                        End If
                                    End If

                                    'Added by Saylee on 19-Sep-2014 for ALL19092014
                                    'If no Cycle Period or Hour Period Present in Monitor Mod       
                                    mDoneONValueForNoPeriod = New Period(ObjCompStatus.CompStatusPeriodList(Count).PeriodID, DBNull.Value)
                                    mTSOValueForNoPeriod = New Period(ObjCompStatus.CompStatusPeriodList(Count).PeriodID, DBNull.Value)

                                    If (ObjCompStatus.CompStatusPeriodList(Count).PeriodID = 1 Or ObjCompStatus.CompStatusPeriodList(Count).PeriodID = 3) And Not (ObjCompMonitorModStatus.CompMonitorModStatusPeriodList.Contains(ObjCompStatus.CompStatusPeriodList(Count).PeriodID)) Then

                                        Dim mPeriodUnitID As Integer = 0
                                        If ObjCompStatus.CompStatusPeriodList(Count).PeriodID = 1 Then
                                            mPeriodUnitID = 1
                                        ElseIf ObjCompStatus.CompStatusPeriodList(Count).PeriodID = 3 Then
                                            mPeriodUnitID = 6
                                        End If
                                        mPeriodID = ObjCompStatus.CompStatusPeriodList(Count).PeriodID
                                        If (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted = False) Then
                                            DoneONValueForNoPeriod = ""
                                        Else
                                            If ObjCompMonitorModStatus.DoneOn <> "" Then
                                                Dim mMachineMaintenance As MachineMaintenance = MachineMaintenance.GetMachineMaintenance(ObjCompMonitorModStatus.ID, MachineMaintenanceActivity.ComponentModification)

                                                Dim mAssemblyCurrentValue As New Period(1, DBNull.Value)
                                                If CurrentValueOnAsOnDate.GetCurrentValue(mMachineMaintenance.LogAssemblyStatusID, ObjCompMonitorModStatus.DoneOn, mMachineMaintenance.LogNo, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID)(0).CurrentValueDec > 0 Then
                                                    mAssemblyCurrentValue = New Period(mPeriodID, CurrentValueOnAsOnDate.GetCurrentValue(mMachineMaintenance.LogAssemblyStatusID, ObjCompMonitorModStatus.DoneOn, mMachineMaintenance.LogNo, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID)(0).CurrentValueDec, , , , ObjMachine.HourType)
                                                Else
                                                    mAssemblyCurrentValue = New Period(mPeriodID, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).AssemblyCurrentValueInDeciaml, , , , ObjMachine.HourType)
                                                End If

                                                If CurrentValueOnAsOnDate.GetCompCurrentValue(ObjCompStatus.CompID, mMachineMaintenance.AssemblyStatusID, ObjCompMonitorModStatus.DoneOn, mMachineMaintenance.LogNo, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID).Count > 0 Then
                                                    tmpCurrentValue = New Period(mPeriodID, Period.Add(ObjCompStatus.CompStatusPeriodList(Count).PeriodID, mPeriodUnitID, CurrentValueOnAsOnDate.GetCompCurrentValue(ObjCompStatus.CompID, mMachineMaintenance.AssemblyStatusID, ObjCompMonitorModStatus.DoneOn, mMachineMaintenance.LogNo, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID)(0).CompCurrentValueDec, Period.Difference(mAssemblyCurrentValue.DBValue, CurrentValueOnAsOnDate.GetCompCurrentValue(ObjCompStatus.CompID, mMachineMaintenance.AssemblyStatusID, ObjCompMonitorModStatus.DoneOn, mMachineMaintenance.LogNo, ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID)(0).AssemblyCurrentValueDec)), mPeriodUnitID, False, , ObjMachine.HourType).DbValueDec
                                                Else
                                                    tmpCurrentValue = New Period(mPeriodID, Period.Add(mPeriodID, mPeriodUnitID, ObjCompStatus.CompStatusPeriodList(Count).CompCurrentValueInDecimal, Period.Difference(mAssemblyCurrentValue.DBValue, ObjCompStatus.CompStatusPeriodList(Count).AssemblyCurrentValueInDeciaml)), mPeriodUnitID, False, , ObjMachine.HourType).DbValueDec
                                                End If

                                                mDoneONValueForNoPeriod = New Period(mPeriodID, tmpCurrentValue, mPeriodUnitID, , , ObjMachine.HourType)

                                                If DoneONValueForNoPeriod = "" Then
                                                    DoneONValueForNoPeriod = mDoneONValueForNoPeriod.TextFormatted
                                                Else
                                                    DoneONValueForNoPeriod = DoneONValueForNoPeriod + IIf(IsExcel, Chr(10), vbCrLf) + mDoneONValueForNoPeriod.TextFormatted
                                                End If
                                            End If
                                        End If
                                    End If
                                Next
                                For Each ObjCompMonitorModStatusPeriod In ObjCompMonitorModStatus.CompMonitorModStatusPeriodList

                                    'Added By Saylee on 25-May-2015 for Taj25052015
                                    'Done On As of Assembly
                                    'If (AppSettings("ClientCode") = "Taj" OR AppSettings("ClientCode") = "HSC" Or AppSettings("ClientCode") = "ASH" ) Then
                                    If (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted = False) Then
                                        DoneONValueForAssembly = ""
                                    Else
                                        If ObjCompMonitorModStatus.DoneOn <> "" Then
                                            mDoneONValueForAssembly = New Period(ObjCompMonitorModStatusPeriod.PeriodID, DBNull.Value)
                                            If ObjCompMonitorModStatusPeriod.PeriodID <> 2 Then
                                                Dim mMachineMaintenance As MachineMaintenance = MachineMaintenance.GetMachineMaintenance(ObjCompMonitorModStatus.ID, MachineMaintenanceActivity.ComponentModification)

                                                'Dim mAssemblyCurrentValue As New Period(1, DBNull.Value)
                                                'If CurrentValueOnAsOnDate.GetCurrentValue(ObjAssemblyStatus.ID, ObjCompMonitorModStatus.DoneOn, mMachineMaintenance.LogNo, ObjCompMonitorModStatusPeriod.PeriodID)(0).CurrentValueDec > 0 Then
                                                '    mAssemblyCurrentValue = New Period(ObjCompMonitorModStatusPeriod.PeriodID, CurrentValueOnAsOnDate.GetCurrentValue(ObjAssemblyStatus.ID, ObjCompMonitorModStatus.DoneOn, mMachineMaintenance.LogNo, ObjCompMonitorModStatusPeriod.PeriodID)(0).CurrentValueDec, , , , ObjMachine.HourType)
                                                'Else
                                                '    mAssemblyCurrentValue = New Period(ObjCompMonitorModStatusPeriod.PeriodID, ObjAssemblyStatus.AssemblyStatusPeriodList(ObjCompMonitorModStatusPeriod.PeriodID, "").AssemblyCurrentValueInDeciaml, , , , ObjMachine.HourType)
                                                'End If

                                                'Dim IsAsOnDateGreater As Boolean = False
                                                'If CurrentValueOnAsOnDate.GetCompCurrentValue(ObjCompStatus.CompID, ObjAssemblyStatus.ID, ObjCompMonitorModStatus.DoneOn, mMachineMaintenance.LogNo, ObjCompMonitorModStatusPeriod.PeriodID).Count > 0 Then
                                                '    tmpCurrentValue = New Period(ObjCompMonitorModStatusPeriod.PeriodID, Period.Add(ObjCompMonitorModStatusPeriod.PeriodID, ObjCompMonitorModStatusPeriod.PeriodUnitID, CurrentValueOnAsOnDate.GetCompCurrentValue(ObjCompStatus.CompID, ObjAssemblyStatus.ID, ObjCompMonitorModStatus.DoneOn, mMachineMaintenance.LogNo, ObjCompMonitorModStatusPeriod.PeriodID)(0).CompCurrentValueDec, Period.Difference(mAssemblyCurrentValue.DBValue, CurrentValueOnAsOnDate.GetCompCurrentValue(ObjCompStatus.CompID, ObjAssemblyStatus.ID, ObjCompMonitorModStatus.DoneOn, mMachineMaintenance.LogNo, ObjCompMonitorModStatusPeriod.PeriodID)(0).AssemblyCurrentValueDec)), ObjCompMonitorModStatusPeriod.PeriodUnitID, False, , ObjMachine.HourType).DbValueDec
                                                'Else
                                                '    ''here check if compliance before AsOnDate
                                                '    If CDate(ObjCompMonitorModStatus.DoneOn.ToString) >= CDate(ObjAssemblyStatus.AsOnDate.ToString) Then
                                                '        tmpCurrentValue = New Period(ObjCompMonitorModStatusPeriod.PeriodID, Period.Add(ObjCompMonitorModStatusPeriod.PeriodID, ObjCompMonitorModStatusPeriod.PeriodUnitID, ObjCompStatus.CompStatusPeriodList(ObjCompMonitorModStatusPeriod.PeriodID, "").CompCurrentValueInDecimal, Period.Difference(mAssemblyCurrentValue.DBValue, ObjCompStatus.CompStatusPeriodList(ObjCompMonitorModStatusPeriod.PeriodID, "").AssemblyCurrentValueInDeciaml)), ObjCompMonitorModStatusPeriod.PeriodUnitID, False, , ObjMachine.HourType).DbValueDec
                                                '    Else
                                                '        tmpCurrentValue = 0
                                                '        IsAsOnDateGreater = True 'if compliance before AsOnDate
                                                '    End If
                                                'End If

                                                '    mDoneONValueForAssembly = New Period(ObjCompMonitorModStatusPeriod.PeriodID, tmpCurrentValue, ObjCompMonitorModStatusPeriod.PeriodUnitID, , , ObjMachine.HourType)

                                                'If IsAsOnDateGreater = False Then
                                                '    If DoneONValueForAssembly = "" Then
                                                '        DoneONValueForAssembly = mDoneONValueForAssembly.TextFormatted
                                                '    Else
                                                '        DoneONValueForAssembly = DoneONValueForAssembly + IIf(IsExcel, Chr(10), vbcrlf) + mDoneONValueForAssembly.TextFormatted
                                                '    End If
                                                'End If
                                                If CDate(ObjCompMonitorModStatus.DoneOn.ToString) < CDate(ObjAssemblyStatus.AsOnDate.ToString) Then
                                                    mDoneONValueForAssembly.DBValue = 0
                                                ElseIf mMachineMaintenance.LogNo <> 0 Then
                                                    If CurrentValueOnAsOnDate.GetCurrentValue(ObjAssemblyStatus.ID, ObjCompMonitorModStatus.DoneOn, mMachineMaintenance.LogNo, ObjCompMonitorModStatusPeriod.PeriodID)(0).CurrentValueDec > 0 Then
                                                        mDoneONValueForAssembly.Value = CurrentValueOnAsOnDate.GetCurrentValue(ObjAssemblyStatus.ID, ObjCompMonitorModStatus.DoneOn, mMachineMaintenance.LogNo, ObjCompMonitorModStatusPeriod.PeriodID)(0).CurrentValue
                                                    End If
                                                Else
                                                    mDoneONValueForAssembly.DBValue = 0
                                                End If


                                                If DoneONValueForAssembly = "" Then
                                                    DoneONValueForAssembly = mDoneONValueForAssembly.TextFormatted
                                                Else
                                                    DoneONValueForAssembly = DoneONValueForAssembly + IIf(IsExcel, Chr(10), vbCrLf) + mDoneONValueForAssembly.TextFormatted
                                                End If
                                            Else
                                                If DoneONValueForAssembly = "" Then
                                                    DoneONValueForAssembly = ObjCompMonitorModStatusPeriod.DoneOnValueFormatted
                                                Else
                                                    DoneONValueForAssembly = DoneONValueForAssembly + IIf(IsExcel, Chr(10), vbCrLf) + ObjCompMonitorModStatusPeriod.DoneOnValueFormatted
                                                End If
                                            End If
                                        End If
                                    End If
                                    ' End If
                                    If ReportStatus = 0 Then  'Landscape
                                        If ObjCompMonitorModStatusPeriod.PeriodID = 1 Then
                                            Freq1 = ObjCompMonitorModStatusPeriod.FrequencyValue
                                            If (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted = True) Or (ObjCompMonitorModStatus.IsApplicable = False And ObjCompMonitorModStatus.IsCompleted = True) Then
                                                ElapsedTime = ""
                                                RemainingTime = ""
                                                DueAsof = ""
                                                'Added By Prashant 04-Aug-2009
                                                SinceNew2 = ""
                                                '-----------------------------
                                                TimeSinceNew = ""
                                            Else
                                                ElapsedTime = ObjCompMonitorModStatusPeriod.AllElapsedValue
                                                RemainingTime = ObjCompMonitorModStatusPeriod.RemainingValue
                                                '$$$$$$$$$$$$$$$$
                                                'DueAsof = DueAsof + ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormatted & vbCrLf
                                                'Added By Prashant 18-Oct-2011

                                                'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf

                                                If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                    DueAsof = DueAsof + ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame & vbCrLf
                                                Else
                                                    DueAsof = DueAsof + ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormatted & vbCrLf
                                                End If
                                                '----------------

                                                'Added By Prashant 04-Aug-2009
                                                If ObjCompMonitorModStatus.MonitorTypeID = 3 And ObjCompMonitorModStatusPeriod.DoneOnValue = "" Then
                                                    SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.CompCurrentValueFormatted
                                                Else
                                                    SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.DiffCompCurrentDoneOnValueFormatted
                                                End If
                                                '-----------------------------
                                                TimeSinceNew = TimeSinceNew & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.CompCurrentValueFormatted
                                            End If

                                            If DoneOnValue = "" Then
                                                DoneOnValue = ObjCompMonitorModStatusPeriod.DoneOnValue
                                            Else
                                                DoneOnValue = DoneOnValue + IIf(IsExcel, Chr(10), vbCrLf) + ObjCompMonitorModStatusPeriod.DoneOnValue
                                            End If

                                            Extension = ObjCompMonitorModStatusPeriod.ExtensionValue
                                        End If
                                        If ObjCompMonitorModStatusPeriod.PeriodID = 2 Then             'StartDate
                                            Freq2 = ObjCompMonitorModStatusPeriod.FrequencyValueFormatted



                                            If (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted = True) Or (ObjCompMonitorModStatus.IsApplicable = False And ObjCompMonitorModStatus.IsCompleted = True) Then
                                                ElapsedTime1 = ""
                                                RemainingTime1 = ""
                                                DueAsof1 = ""
                                            Else
                                                ElapsedTime1 = ObjCompMonitorModStatusPeriod.AllElapsedValueFormatted
                                                RemainingTime1 = ObjCompMonitorModStatusPeriod.RemainingValueFormatted
                                                'DueAsof1 = DueAsof1 + ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormatted & vbCrLf
                                                'Added By Prashant 18-Oct-2011

                                                'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf

                                                If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                    DueAsof1 = DueAsof1 + ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame & vbCrLf
                                                Else
                                                    DueAsof1 = DueAsof1 + ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormatted & vbCrLf
                                                End If
                                            End If
                                            DoneOnValue = DoneOnValue + ObjCompMonitorModStatusPeriod.DoneOnValueFormatted & vbCrLf
                                            Extension1 = ObjCompMonitorModStatusPeriod.ExtensionValueFormatted
                                        End If
										'If ObjCompMonitorModStatusPeriod.PeriodID = 3 Or ObjCompMonitorModStatusPeriod.PeriodID = 4 Or ObjCompMonitorModStatusPeriod.PeriodID = 5 Or ObjCompMonitorModStatusPeriod.PeriodID = 6 Or ObjCompMonitorModStatusPeriod.PeriodID = 7 Or ObjCompMonitorModStatusPeriod.PeriodID = 8 Or ObjCompMonitorModStatusPeriod.PeriodID = 12 Or ObjCompMonitorModStatusPeriod.PeriodID = 13 Or ObjCompMonitorModStatusPeriod.PeriodID = 14 Then
										'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
										If ObjCompMonitorModStatusPeriod.PeriodID >= 3 Then
											If Freq3 = "" Then
												Freq3 = ObjCompMonitorModStatusPeriod.FrequencyValue

												If (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted = True) Or (ObjCompMonitorModStatus.IsApplicable = False And ObjCompMonitorModStatus.IsCompleted = True) Then
													ElapsedTime2 = ""
													RemainingTime2 = ""
													DueAsof2 = ""
													AssemblyDueAsof2 = ""
													SinceNew2 = ""
													DoneAt2 = ""
													TimeSinceNew = ""
												Else
													ElapsedTime2 = ObjCompMonitorModStatusPeriod.AllElapsedValue
													RemainingTime2 = ObjCompMonitorModStatusPeriod.RemainingValue
													'DueAsof2 = DueAsof2 + ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormatted & vbCrLf
													'Added By Prashant 18-Oct-2011

													'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
													'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
													'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf

													If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
														DueAsof2 = DueAsof2 + ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame & vbCrLf
													Else
														DueAsof2 = DueAsof2 + ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormatted & vbCrLf
													End If
													AssemblyDueAsof2 = AssemblyDueAsof2 + ObjCompMonitorModStatusPeriod.DueOnValueFormatted & vbCrLf
													If ObjCompMonitorModStatus.MonitorTypeID = 3 And ObjCompMonitorModStatusPeriod.DoneOnValue = "" Then
														SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.CompCurrentValueFormatted
													Else
														SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.DiffCompCurrentDoneOnValueFormatted
													End If
													DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.DoneOnValueFormatted
													TimeSinceNew = TimeSinceNew & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.CompCurrentValueFormatted
												End If
												DoneOnValue = DoneOnValue + ObjCompMonitorModStatusPeriod.DoneOnValue & vbCrLf
												Extension2 = ObjCompMonitorModStatusPeriod.ExtensionValue
												DiffCompInstDoneOnValue = ObjCompMonitorModStatusPeriod.DiffCompInstDoneOnValue
											Else                                                    'Freq3 <> ""
												Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.FrequencyValue

												If (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted = True) Or (ObjCompMonitorModStatus.IsApplicable = False And ObjCompMonitorModStatus.IsCompleted = True) Then
													ElapsedTime2 = ""
													RemainingTime2 = ""
													DueAsof2 = ""
													AssemblyDueAsof2 = ""
													SinceNew2 = ""
													DoneAt2 = ""
													TimeSinceNew = ""
												Else
													ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.AllElapsedValue
													RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.RemainingValue
													'DueAsof2 = DueAsof2 + ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormatted & vbCrLf
													'Added By Prashant 18-Oct-2011

													'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
													'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
													'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf

													If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
														DueAsof2 = DueAsof2 + ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame & vbCrLf
													Else
														DueAsof2 = DueAsof2 + ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormatted & vbCrLf
													End If
													AssemblyDueAsof2 = AssemblyDueAsof2 + ObjCompMonitorModStatusPeriod.DueOnValueFormatted & vbCrLf
													If ObjCompMonitorModStatus.MonitorTypeID = 3 And ObjCompMonitorModStatusPeriod.DoneOnValue = "" Then
														SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.CompCurrentValueFormatted
													Else
														SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.DiffCompCurrentDoneOnValueFormatted
													End If
													DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.DoneOnValueFormatted
													TimeSinceNew = TimeSinceNew & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.CompCurrentValueFormatted
												End If
												DoneOnValue = DoneOnValue + ObjCompMonitorModStatusPeriod.DoneOnValue & vbCrLf
												Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.ExtensionValue
												DiffCompInstDoneOnValue = DiffCompInstDoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.DiffCompInstDoneOnValue
											End If
										End If
									Else                                                               'Report <> 0
                                        If ObjCompMonitorModStatusPeriod.PeriodID = 2 Then             'StartDate
                                            If Freq3 = "" Then
                                                Freq3 = ObjCompMonitorModStatusPeriod.FrequencyValueFormatted

                                                If (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted = True) Or (ObjCompMonitorModStatus.IsApplicable = False And ObjCompMonitorModStatus.IsCompleted = True) Then
                                                    ElapsedTime2 = ""
                                                    RemainingTime2 = ""
                                                    DueAsof2 = ""
                                                    AssemblyDueAsof2 = ""
                                                    DoneAt2 = ""
                                                Else
                                                    ElapsedTime2 = ObjCompMonitorModStatusPeriod.AllElapsedValueFormatted
                                                    RemainingTime2 = ObjCompMonitorModStatusPeriod.RemainingValueFormatted
                                                    'DueAsof2 = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormatted
                                                    'Added By Prashant 18-Oct-2011

                                                    'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                    'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                    'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf

                                                    If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                        DueAsof2 = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                    Else
                                                        DueAsof2 = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormatted
                                                    End If
                                                    AssemblyDueAsof2 = ObjCompMonitorModStatusPeriod.DueOnValueFormatted
                                                    'SinceNew2 = ObjCompMonitorModStatusPeriod.CompCurrentValueFormatted
                                                    'SinceNew2 = (ObjCompMonitorModStatusPeriod.CompCurrentValueFormatted - ObjCompMonitorModStatusPeriod.DoneOnValueFormatted)
                                                    DoneAt2 = ObjCompMonitorModStatusPeriod.DoneOnValueFormatted
                                                End If
                                                DoneOnValue = ObjCompMonitorModStatusPeriod.DoneOnValueFormatted
                                                Extension2 = ObjCompMonitorModStatusPeriod.ExtensionValueFormatted
                                                DiffCompInstDoneOnValue = ObjCompMonitorModStatusPeriod.DiffCompInstDoneOnValue
                                            Else                                                       'Freq3 <> ""  
                                                Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.FrequencyValueFormatted

                                                If (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted = True) Or (ObjCompMonitorModStatus.IsApplicable = False And ObjCompMonitorModStatus.IsCompleted = True) Then
                                                    ElapsedTime2 = ""
                                                    RemainingTime2 = ""
                                                    DueAsof2 = ""
                                                    AssemblyDueAsof2 = ""
                                                    DoneAt2 = ""
                                                Else
                                                    ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.AllElapsedValueFormatted
                                                    RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.RemainingValueFormatted
                                                    'DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbcrlf) & ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormatted
                                                    'Added By Prashant 18-Oct-2011


                                                    'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                    'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                    'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf

                                                    If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                        DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                    Else
                                                        DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormatted
                                                    End If
                                                    AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.DueOnValueFormatted
                                                    'SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbcrlf) & ObjCompMonitorModStatusPeriod.CompCurrentValueFormatted
                                                    'SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbcrlf) & (ObjCompMonitorModStatusPeriod.CompCurrentValueFormatted - ObjCompMonitorModStatusPeriod.DoneOnValueFormatted)
                                                    DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.DoneOnValueFormatted
                                                End If
                                                DoneOnValue = ObjCompMonitorModStatusPeriod.DoneOnValue
                                                Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.ExtensionValueFormatted
                                                DiffCompInstDoneOnValue = DiffCompInstDoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.DiffCompInstDoneOnValue
                                            End If
                                        Else                                                           'For PeriodID <> 2
                                            If Freq3 = "" Then
                                                Freq3 = ObjCompMonitorModStatusPeriod.FrequencyValue


                                                If (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted = True) Or (ObjCompMonitorModStatus.IsApplicable = False And ObjCompMonitorModStatus.IsCompleted = True) Then
                                                    ElapsedTime2 = ""
                                                    RemainingTime2 = ""
                                                    DueAsof2 = ""
                                                    AssemblyDueAsof2 = ""
                                                    SinceNew2 = ""
                                                    DoneAt2 = ""
                                                    TimeSinceNew = ""
                                                Else
                                                    ElapsedTime2 = ObjCompMonitorModStatusPeriod.AllElapsedValue
                                                    RemainingTime2 = ObjCompMonitorModStatusPeriod.RemainingValue
                                                    If ObjCompMonitorModStatusPeriod.PeriodID = 9 Then
                                                        DueAsof2 = ObjCompMonitorModStatusPeriod.DueOnValueFormatted '-'
                                                        AssemblyDueAsof2 = ObjCompMonitorModStatusPeriod.DueOnValueFormatted
                                                        'SinceNew2 = ObjCompMonitorModStatusPeriod.DiffCompCurrentDoneOnValueFormatted
                                                        'Added By Prashant 03-Aug-2009
                                                        If ObjCompMonitorModStatus.MonitorTypeID = 3 And ObjCompMonitorModStatusPeriod.DoneOnValue = "" Then
                                                            SinceNew2 = ObjCompMonitorModStatusPeriod.CompCurrentValueFormatted
                                                        Else
                                                            SinceNew2 = ObjCompMonitorModStatusPeriod.DiffCompCurrentDoneOnValueFormatted
                                                        End If
                                                        '-----------------------------
                                                        DoneAt2 = ObjCompMonitorModStatusPeriod.DoneOnValueFormatted
                                                        TimeSinceNew = ObjCompMonitorModStatusPeriod.CompCurrentValueFormatted
                                                    Else
                                                        'DueAsof2 = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormatted
                                                        'Added By Prashant 18-Oct-2011

                                                        'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                        'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                        'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf

                                                        If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                            DueAsof2 = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                        Else
                                                            DueAsof2 = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormatted
                                                        End If
                                                        AssemblyDueAsof2 = ObjCompMonitorModStatusPeriod.DueOnValueFormatted
                                                        If ObjCompMonitorModStatus.MonitorTypeID = 3 And ObjCompMonitorModStatusPeriod.DoneOnValue = "" Then
                                                            SinceNew2 = ObjCompMonitorModStatusPeriod.CompCurrentValueFormatted
                                                        Else
                                                            SinceNew2 = ObjCompMonitorModStatusPeriod.DiffCompCurrentDoneOnValueFormatted
                                                        End If
                                                        DoneAt2 = ObjCompMonitorModStatusPeriod.DoneOnValueFormatted
                                                        TimeSinceNew = ObjCompMonitorModStatusPeriod.CompCurrentValueFormatted
                                                    End If
                                                End If
                                                DoneOnValue = ObjCompMonitorModStatusPeriod.DoneOnValue
                                                Extension2 = ObjCompMonitorModStatusPeriod.ExtensionValue
                                                DiffCompInstDoneOnValue = ObjCompMonitorModStatusPeriod.DiffCompInstDoneOnValue
                                            Else                                                       'Freq3 <> ""
                                                Freq3 = Freq3 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.FrequencyValue

                                                If (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted = True) Or (ObjCompMonitorModStatus.IsApplicable = False And ObjCompMonitorModStatus.IsCompleted = True) Then
                                                    ElapsedTime2 = ""
                                                    RemainingTime2 = ""
                                                    DueAsof2 = ""
                                                    AssemblyDueAsof2 = ""
                                                    SinceNew2 = ""
                                                    DoneAt2 = ""
                                                    TimeSinceNew = ""
                                                Else
                                                    ElapsedTime2 = ElapsedTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.AllElapsedValue
                                                    RemainingTime2 = RemainingTime2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.RemainingValue
                                                    If ObjCompMonitorModStatusPeriod.PeriodID = 9 Then
                                                        DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.DueOnValueFormatted  '-'
                                                        AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.DueOnValueFormatted
                                                        If ObjCompMonitorModStatus.MonitorTypeID = 3 And ObjCompMonitorModStatusPeriod.DoneOnValue = "" Then
                                                            SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.CompCurrentValueFormatted
                                                        Else
                                                            SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.DiffCompCurrentDoneOnValueFormatted
                                                        End If
                                                        DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.DoneOnValueFormatted
                                                        TimeSinceNew = TimeSinceNew & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.CompCurrentValueFormatted
                                                    Else
                                                        'DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbcrlf) & ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormatted
                                                        'Added By Prashant 18-Oct-2011

                                                        'Removed : (AppSettings("ClientCode") = "RAL") by Saylee on 7-July-2016
                                                        'As RAL does not maintain cycles period in Airframe but they maintain cycle period in engines so DueAsOf show wrong values as per Airframe
                                                        'To avoid wrong value we show only Assembly DueAsOf instead of AirframeDueAsOf

                                                        If (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then 'Client Code YA and TA added By Vikrant On 24-July-2013 For BAYATA24072013
                                                            DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                        Else
                                                            DueAsof2 = DueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormatted
                                                        End If
                                                        AssemblyDueAsof2 = AssemblyDueAsof2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.DueOnValueFormatted
                                                        If ObjCompMonitorModStatus.MonitorTypeID = 3 And ObjCompMonitorModStatusPeriod.DoneOnValue = "" Then
                                                            SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.CompCurrentValueFormatted
                                                        Else
                                                            SinceNew2 = SinceNew2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.DiffCompCurrentDoneOnValueFormatted
                                                        End If
                                                        DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.DoneOnValueFormatted
                                                        TimeSinceNew = TimeSinceNew & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.CompCurrentValueFormatted
                                                    End If
                                                End If
                                                DoneOnValue = ObjCompMonitorModStatusPeriod.DoneOnValue
                                                Extension2 = Extension2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.ExtensionValue
                                                DiffCompInstDoneOnValue = DiffCompInstDoneOnValue & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatusPeriod.DiffCompInstDoneOnValue
                                            End If
                                        End If
                                    End If
                                Next

                                If chkNotMonitoredValues.Checked = True Then 'Added by Saylee on 11-May-2015 for ALL11052015
                                    If DoneONValueForNoPeriod <> "" Then 'Added by Saylee on 19-Sep-2014 for ALL19092014
                                        If DoneAt2 = "" Then
                                            DoneAt2 = DoneONValueForNoPeriod
                                        Else
                                            DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & DoneONValueForNoPeriod
                                        End If
                                    End If
                                End If

                                '''Added By Saylee on 25-May-2015 for Taj25052015
                                ''If (AppSettings("ClientCode") = "Taj" OR AppSettings("ClientCode") = "HSC" Or AppSettings("ClientCode") = "ASH" ) Then
                                ''    DoneAt2 = DoneONValueForAssembly
                                ''    DoneONValueForAssembly = ""
                                ''Else
                                ''    DoneAt2 = DoneAt2
                                ''End If


                                AssemblyID = ObjAssemblyStatus.AssemblyID
                                Note = ObjCompMonitorModStatus.Notes
                                Remark = ObjCompMonitorModStatus.DoneRemark
                                Number = ObjCompMonitorModStatus.Number
                                Reference = ObjCompMonitorModStatus.Reference
                                DoneOnDate = ObjCompMonitorModStatus.DoneOn
                                DoneWONo = ObjCompMonitorModStatus.DoneOnWONo
                                ExtensionDate = ObjCompMonitorModStatus.ExtensionDate
                                ApprovalRemark = ObjCompMonitorModStatus.ApprovalRemark
                                Dim ATACode = ObjCompMonitorModStatus.ATACode
                                SourceDoc = ObjCompMonitorModStatus.SourceDoc

                                If IsExcel Then

                                    If ATACode.ToString.Length < 3 Then
                                        ATAChapter = ATACode.ToString.PadLeft(3, "0"c) + " " + "-" + " " + ObjCompMonitorModStatus.ATANomenclature
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

                                ''Added By Saylee on 2-Aug-2024
                                If ObjCompMonitorModStatus.NonMonitoringPeriodDetails <> "" Then
                                    DoneAt2 = DoneAt2 & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatus.NonMonitoringPeriodDetails.Replace("<br>", vbCrLf)
                                End If
                                '*******************************************
                                If ObjCompMonitorModStatus.IsApplicable = False Then
                                    DueAsof = ""
                                    DueAsof1 = ""
                                    DueAsof2 = ""
                                    RemainingTime = ""
                                    RemainingTime1 = ""
                                    RemainingTime2 = ""
                                End If

                                ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, MonitorType, MonitorTypeCode, Note, Remark, Description,
                                , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2,
                               DueAsof, DueAsof1, DueAsof2, AssemblyModel, , , SinceNew2, , , DoneAt2, , , , ObjCompMonitorModStatus.ATACode, , , , StartDateData, , , , , , , , , Number, Reference, DoneOnValue, DoneOnDate, DoneWONo, , , , , AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate,
                               ApprovalRemark, , , Code, , , , , , , ObjCompMonitorModStatus.MonitorTypeID, , , , , , , TimeSinceNew, DoneONValueForAssembly:=DoneONValueForAssembly, SourceDoc:=SourceDoc, DiffCompInstDoneOnValue:=DiffCompInstDoneOnValue))
                            Next
                        Next
                    End If
                Next
            Next
            'End If
            '    Next
        End If
        Return ReportMaintenanceDetails
    End Function
    Private Sub SetReport(Optional ByVal ByMail As Boolean = False)  'Parameter Added by Shital on 14-Sep-2016
        ReportMaintenanceDetails = New ReportMaintenanceDetailList
        ReportStatusList = New rptStatusList
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsReportMaintenanceDetail
        Dim RptCofA As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim RptCofAPortrait As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim OperatorName As String = ""
        Dim mTaskCardListByMaintenanceActivity As TaskCardListByMaintenanceActivity 'Vikrant
        Dim RptModificationStatusList As New crModificationStatusList '4


        SetValues()
        If chkTaskCard.Checked Then
            mTaskCardListByMaintenanceActivity = TaskCardListByMaintenanceActivity.GetTaskCardList(Guid.Empty.ToString)
        Else
            mTaskCardListByMaintenanceActivity = TaskCardListByMaintenanceActivity.NewList
        End If

        'Added by Saylee on 11-Aug-2011
        'If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "Indamer")) Then
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

        If AppSettings("ShowMaintenanceForNewClients") = "True" And mOpen = Open.ServiceReport Then 'Ajay Added 30-06-2023
            RptCofA = New crCofALandscapeFormBAForTaskNo

            If chkTaskCard.Checked Then
                RptCofA = New crInspectionReportWithTaskCardForMPD
            End If

        ElseIf cmbFormat.SelectedIndex = 0 Then
            If AppSettings("ClientCode") = "STR" Then
                If mOpen = Open.ServiceReport Then
                    RptCofA = New crInspectionReportWithTaskCardStarAir
                Else
                    RptCofA = New crInspectionReportStarAir
                End If

            Else
                If chkTaskCard.Checked Then
                    If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                        RptCofA = New crInspectionReportWithTaskCardForMPD
                    Else
                        RptCofA = New crInspectionReportWithTaskCard 'crCofALandscapeFormBSA
                    End If
                Else
                    If (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
                        'RptCofA = New crCofALandscapeFormInd
                        RptCofA = New crCofALandscapeFormIndamer '3
                        RptCofAPortrait = New crCofAPortraitFormInd '2
                    ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso AppSettings("ClientCode") = "Taj" Or AppSettings("ClientCode") = "HSC" Or AppSettings("ClientCode") = "ASH" Then
                        RptCofA = New crCofALandscapeFormTaj
                    Else
                        If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                            RptCofA = New crCofALandscapeFormWithTaskNo
                        Else
                            RptCofA = New crCofALandscapeForm
                            RptCofAPortrait = New crCofAPortraitForm
                        End If

                    End If
                End If
            End If
        ElseIf cmbFormat.SelectedIndex = 1 Then
            If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then 'Added By Vikrant On 14-Mar-2015 For BA14032015-1
                RptCofA = New crCofALandscapeFormTSNTSOForBA
            Else 'End
                If AppSettings("ShowMaintenanceForNewClients") = "True" Then 'Ajay Added 30-06-2023
                    RptCofA = New crCofALandscapeFormTSNTSOWithTaskNo
                Else
                    RptCofA = New crCofALandscapeFormTSNTSO '1
                    RptCofAPortrait = New crCofAPortraitForm
                End If

            End If
            'Added By Utkarsh ON 12-Sep-2013 FOR BA11092013
        ElseIf cmbFormat.SelectedIndex = 2 Then
            If mOpen = 2 Or mOpen = Open.ServiceReport Then 'APFT
                'If AppSettings("ShowMaintenanceForNewClients") = "True" Then 'Ajay Added 30-06-2023
                '    RptCofA = New crCofALandscapeFormBAForTaskNo
                'Else
                RptCofA = New crCofALandscapeFormBA
                'End If
            Else
                If AppSettings("ClientCode") = "RAL" Then
                    RptCofA = New crCofALandscapeFormRAL
                End If
            End If
        End If

        'Added By Utkarsh ON 12-Sep-2013 FOR BA11092013
        Dim searchstr1 As String
        Dim mPerDayLimit As PerDayLimit
        If cmbFormat.SelectedIndex = 2 AndAlso (mOpen = 2 Or mOpen = Open.ServiceReport) Then 'APFT
            For Each mPerDayLimit In mPerDayLimits
                If CDec(Val(mPerDayLimit.PeriodLimit)) >= 0 Then
                    If searchstr1 = "" Then
                        searchstr1 = "The tentative Date of Removal has been calculated on the basis of A/C utilization " & " " & searchstr1 & " " & mPerDayLimit.PeriodLimit & " " & mPerDayLimit.PeriodName
                    Else
                        searchstr1 = searchstr1 & ", " & mPerDayLimit.PeriodLimit & " " & mPerDayLimit.PeriodName
                    End If
                End If
            Next
            searchstr1 = searchstr1 & " per Day "
        End If
        'end


        Dim LastFlownDate As String = ""
        Dim mMaxLogNo As MaxLogNo = MaxLogNo.GetMaxLogNo(AsonDate, New Guid(MachineName), New Guid(AssemblyName))

        If mMaxLogNo.Count <> 0 Then
            LastFlownDate = mMaxLogNo(0).LogDate.ToString 'Last Flight Log Date
        End If

        ReportDetail()
        'If (AssemblyType <> "(All)") Then   'Added Code May,21,007
        '    ReportLabel = ""
        If (AssemblyType = "(All)" And cmbType.SelectedItem.ToString = "All") Or (AssemblyType = "(All)" And IsSerSelect = True And IsInsSelect = True And IsModSelect = True) Then
            ReportLabel = "Service/Inspection/Modification status"
        ElseIf (AssemblyType = "(All)" And cmbType.SelectedItem.ToString = "All") Or (AssemblyType = "(All)" And IsSerSelect = True And IsInsSelect = True And IsModSelect = True) Then
            ReportLabel = "Service/Inspection/Modification status"
        ElseIf IsSerSelect = True And AssemblyType = "(All)" And IsInsSelect = True Then
            ReportLabel = "Service/Inspection status"
        ElseIf IsSerSelect = True And AssemblyType = "(All)" And IsModSelect = True Then
            ReportLabel = "Service/Modification status"
        ElseIf IsInsSelect = True And AssemblyType = "(All)" And IsModSelect = True Then
            ReportLabel = "Inspection/Modification status"
        ElseIf (AssemblyType <> "(All)" And cmbType.SelectedItem.ToString = "All") Or (AssemblyType <> "(All)" And IsSerSelect = True And IsInsSelect = True And IsModSelect = True) Then
            ReportLabel = "Service/Inspection/Modification status of" + " " + AssemblyType
        ElseIf IsSerSelect = True And AssemblyType <> "(All)" And IsInsSelect = True Then
            ReportLabel = "Service/Inspection status of" + " " + AssemblyType
        ElseIf IsSerSelect = True And AssemblyType <> "(All)" And IsModSelect = True Then
            ReportLabel = "Service/Modification status of" + " " + AssemblyType
        ElseIf IsInsSelect = True And AssemblyType <> "(All)" And IsModSelect = True Then
            ReportLabel = "Inspection/Modification status" + " " + AssemblyType
        ElseIf AssemblyType = "(All)" And IsSerSelect = True Then
            If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                ReportLabel = "AMP Status"
            Else
                ReportLabel = "Service status"
            End If
        ElseIf AssemblyType = "(All)" And IsInsSelect = True Then
            ReportLabel = "Inspection status"
        ElseIf AssemblyType = "(All)" And IsModSelect = True Then
            ReportLabel = "Modification status"
        ElseIf AssemblyType <> "(All)" And IsSerSelect = True Then
            If AppSettings("ShowMaintenanceForNewClients") = "True" Then
                ReportLabel = "AMP Status of" + " " + AssemblyType
            Else
                ReportLabel = "Service status of" + " " + AssemblyType
            End If
        ElseIf AssemblyType <> "(All)" And IsInsSelect = True Then
            ReportLabel = "Inspection status of" + " " + AssemblyType
        ElseIf AssemblyType <> "(All)" And IsModSelect = True Then
            ReportLabel = "Modification status of" + " " + AssemblyType
        End If

        'As per the suggestion by SayleeMame
        'Added By Abhishek ON 9-Sep-2017 


        Dim ServicesShortName As String = ""
        'Added By Vikrant On 27-Feb-2020 for showing Periods Code and their long forms at bottom of report
        Dim mPeriodUnitList As PeriodUnitList
        Dim PeriodsShortName As New StringBuilder

        mPeriodUnitList = PeriodUnitList.GetPeriodUnitList()
        For i As Integer = 0 To mPeriodUnitList.Count - 1
            PeriodsShortName.Append(mPeriodUnitList(i).Code + "-" + mPeriodUnitList(i).PeriodUnitName + ", ")
        Next
        'End



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
                        ServicesShortName = IIf(Not mServiceTypeList(i, "").CodeType Is Nothing, mServiceTypeList(i, "").CodeType, "")
                    Else
                        ServicesShortName = ServicesShortName + IIf(Not mServiceTypeList(i, "").CodeType Is Nothing, ", " + mServiceTypeList(i, "").CodeType, "")
                    End If
                Next
            End If
        End If
        Dim InspsShortName As String = ""
        If IsInsSelect Then
            For i As Integer = 0 To mInspectionTypeList.Count - 1
                If InspsShortName = "" Then
                    InspsShortName = IIf(Not mInspectionTypeList(i, "").CodeType Is Nothing, mInspectionTypeList(i, "").CodeType, "")
                Else
                    InspsShortName = InspsShortName + IIf(Not mInspectionTypeList(i, "").CodeType Is Nothing, ", " + mInspectionTypeList(i, "").CodeType, "")
                End If
            Next
        End If
        Dim ModShortName As String = ""
        If IsModSelect Then
            For i As Integer = 0 To mModificationTypeList.Count - 1
                If ModShortName = "" Then
                    ModShortName = IIf(Not mModificationTypeList(i, "").CodeType Is Nothing, mModificationTypeList(i, "").CodeType, "")
                Else
                    ModShortName = ModShortName + IIf(Not mModificationTypeList(i, "").CodeType Is Nothing, ", " + mModificationTypeList(i, "").CodeType, "")
                End If
            Next
        End If
        'Added By Utkarsh ON 12-Sep-2013 FOR BA11092013 parameter searchstr1

        Report = New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
mCompanyDetail.WebSite, ReportLabel, New SmartDate(txtFromDate.Text.ToString).FormattedText, mOpen.ToString, LastFlownDate, SearchStr4,
txtBottomLine.Text, mModuleList.Item("CofA").FormRevisionNo, AppSettings("SINote"), searchstr1, OperatorName, IIf(Aircraft = "", "ALL", Aircraft),
IIf(Assembly1 = "", "ALL", Assembly1), AppSettings("Logo"), AppSettings("ClientCode"), ServicesShortName, InspsShortName, ModShortName,
SearchStr15:=IIf(chkTaskCard.Checked, "True", "False"), SearchStr16:=AppSettings("FormNoInspReport"), SearchStr17:=PeriodsShortName.ToString.Trim.TrimEnd(","),
SearchStr18:="", SearchStr19:=AMPNoStr)

        'In ReportData AppSettings("Product Version") Replace with mModuleList.Item("CofA").FormRevisionNo for Suhan

        SetSession()
        If ByMail = False Then    'If case added by shital on 14-Sep-2016
            If ReportMaintenanceDetails.Count = 0 Then
                'Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly)
                'msg1.ReplacePage = "wfSearchCriteriaForCofA.aspx?Open=" & mOpen
                'msg1.Show()
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub
            Else
                If mOpen = 1 Then
                    If Not IsExcel Then RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 718)
                ElseIf mOpen = 2 Then
                    If Not IsExcel Then RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 727)
                ElseIf mOpen = 3 Then
                    If Not IsExcel Then RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 728)
                ElseIf mOpen = Open.ServiceReport Then 'APFT
                    If Not IsExcel Then RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 1377)
                End If

            End If
        End If
        'added by shital on 14-Sep-2016
        If (ByMail = True And ReportMaintenanceDetails.Count <= 0) Then
            SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, ReportLabel, ReportLabel, "There is no record for this search criteria.", "",
                Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"),
                ReportGeneratedBy:=Session("ReportGenratedBy"),
                SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))
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
        da.Fill(ds, mTaskCardListByMaintenanceActivity) 'vikrant

        'If optLandscape.Checked = True Then 'Landscape format
        If mOpen = Open.ModificationReport Then
            RptModificationStatusList.SetDataSource(ds)
            Session("CrystalReport") = RptModificationStatusList
        Else
            RptCofA.SetDataSource(ds)
            Session("CrystalReport") = RptCofA
        End If
        'Else                                'Portrait Format
        '    RptCofAPortrait.SetDataSource(ds)
        '    Session("CrystalReport") = RptCofAPortrait
        'End If
        'added by shital on 14-Sep-2016
        If (ByMail = True) Then
            SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, ReportLabel.Replace("/", " "), ReportLabel.Replace("/", " "), " For " + lblAircraft1.Text, ,
                                      Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"),
                                      ReportGeneratedBy:=Session("ReportGenratedBy"),
                SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))
        ElseIf Not IsExcel Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        End If

        If Not (IsExcel Or ByMail) Then
            Dim Str As String
            Str = "openTranDetail();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", Str, True)
        End If

        Dim mName As String = String.Empty
        If mOpen = 1 Then
            mName = "CofA"
        ElseIf mOpen = 2 Then
            mName = "InspectionReport"
        ElseIf mOpen = 3 Then
            mName = "ModificationReport"
        ElseIf mOpen = Open.ServiceReport Then 'APFT
            mName = "ServiceReport"
        End If
        MarkLog(Util.Action.Print, mName, mCofASearchingCriteria + " " + ReportLabel, Util.ErrorType.NoError, Guid.Empty, EventLogID)
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
    'Added By Utkarsh ON 12-Sep-2013 FOR BA11092013
    Private Sub SetGridObject()
        If mPerDayLimits Is Nothing Then
            mPerDayLimits = PerDayLimits.GetPerDayLimits(New Guid(cmbAircraft.SelectedValue.ToString))
        End If
        Dim txtPerDatLimit As TextBox
        Dim i1 As Int32
        For i1 = 0 To Me.gdPerDayLimit.Items.Count - 1
            txtPerDatLimit = CType(Me.gdPerDayLimit.Items(i1).FindControl("txtLimitPerDay"), TextBox)
            'mPerDayLimits.Item(i1).PeriodLimit = CDec(Val(Trim(txtPerDatLimit.Text))) 'Commented by Saylee on 12-Nov-2012
            mPerDayLimits.Item(i1).PeriodLimit = Trim(txtPerDatLimit.Text)  'Added by Saylee on 12-Nov-2012
            PeriodLimt = PeriodLimt + ", " + Trim(txtPerDatLimit.Text)
        Next i1
        'Session("mPerDayLimits") = mPerDayLimits
    End Sub
    'End
#End Region

#Region " Data Binding "
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)

        'If custValidator.ControlToValidate = "cmbAircraft" Then                      'Aircraft
        '    If cmbAircraft.SelectedIndex = 0 Then
        '        custValidator.ErrorMessage = "Please select the Aircraft and Assembly"
        '        e.IsValid = False
        '    Else
        '        e.IsValid = True
        '    End If
        'End If
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
    Public Sub SetComboOfMachine(ByVal AOnDate As String)
        mMachineList = MachineList.GetMachineListMonitoringStatus(AOnDate, , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , True, "<SELECT>", SkipIsForInventoryAircarft:=True)
        cmbAircraft.DataSource = mMachineList
        Session("mMachineList") = mMachineList
        cmbAircraft.DataBind()
    End Sub
    Public Sub SetCombo()                                         'Added Code

        GetSession()
        mTypeListForCofA = TypeListForCofA.GetTypeListForCofA(CType(mOpen, TypeListForCofA.Open), "")
        cmbType.DataSource = mTypeListForCofA
        cmbType.DataBind()


        mServiceTypeList = PartMonitorServiceTypeList.GetPartMonitorServiceTypeList() 'ServiceType
        mInspectionTypeList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList()  'Inspection Type 

        If mOpen = Open.RoutineInspectionReport Then    'Onlu Routine Inspection Type
            mInspectionTypeList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList(ModelMonitorInspTypeList.serach.OnlyRoutineInspections)
        End If

        mModificationTypeList = ModelMonitorModTypeList.GetModelMonitorModTypeList()   'Modification Type
        '//
    End Sub
    Private Sub DataFieldBind()
        mOpen = Session("mOpen")
        mTypeListForCofA = TypeListForCofA.GetTypeListForCofA(CType(IIf(mOpen = Open.ServiceReport, 5, mOpen), TypeListForCofA.Open), "")
        cmbType.DataSource = mTypeListForCofA
        cmbType.DataBind()
        If mTypeListForCofA.Count > 0 Then cmbType.Items(0).Attributes.Add("style", "color:red")

        Select Case mOpen
            Case Open.CofAReport
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

            Case Open.RoutineInspectionReport
                mInspectionTypeList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList(ModelMonitorInspTypeList.serach.AllInspections)
                ListInspectionType.DataSource = mInspectionTypeList
                Session("mInspectionTypeList") = mInspectionTypeList

            Case Open.ModificationReport
                mModificationTypeList = ModelMonitorModTypeList.GetModelMonitorModTypeList()
                ListDirectiveType.DataSource = mModificationTypeList
                Session("mModificationTypeList") = mModificationTypeList

                'APFT
            Case Open.ServiceReport
                mServiceTypeList = PartMonitorServiceTypeList.GetPartMonitorServiceTypeList()
                ListServiceType.DataSource = mServiceTypeList
                Session("mServiceTypeList") = mServiceTypeList
        End Select

        'Added by Saylee on 20-Apr-2010
        mATAList = ATAList.GetATAList("", "(All)")
        Session("mATAList") = mATAList
        cmbATAChapter.DataSource = mATAList
        '***************************
        'Added By Utkarsh ON 12-Sep-2013 FOR BA11092013
        mPerDayLimits = PerDayLimits.GetPerDayLimits(New Guid(cmbAircraft.SelectedValue.ToString))
        gdPerDayLimit.DataSource = mPerDayLimits
        Session("mPerDayLimits") = mPerDayLimits
        'End
        DataBind()
    End Sub
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid) 'Added by Prashant on 04-Dec-2013
        If Not IsPostBack And Session("Sender") = "" Then
            mOpen = Request.QueryString("Open")   'Added Code
            hdnOpen.Value = mOpen
            Session("mOpen") = mOpen              'Added Code 
            Session("MiddleFrame") = "wfSearchCriteriaForCofA_Ajax.aspx?Open=" & mOpen
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
            'Added By Utkarsh ON 12-Sep-2013 FOR BA11092013
            If mOpen = 2 Or AppSettings("ClientCode") = "RAL" Or mOpen = Open.ServiceReport Then 'APFT
                cmbFormat.Items.Add(New ListItem("Format 3", 2))
            End If
            'Added By Vikrant On 14-March-2014 For All14032014
            If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then
                txtBottomLine.Text = "I hereby certify that the data specified above has been certified throughout : 									Technical Support Division: __________________ Date: _____________"
                chkNotMonitoredValues.Checked = True 'Added by Saylee on 11-May-2015 for ALL11052015
            ElseIf AppSettings("ClientCode") = "APFT" Or
                   AppSettings("ClientCode") = "AAP" Then 'Added By Saylee On 1-Oct-2018 
                txtBottomLine.Text = "I hereby certify that the data specified above has been verified throughout. Continuing Airworthiness Manager: __________________ Date: _____________"
            Else
                txtBottomLine.Text = "I hereby certify that the data specified above has been verified throughout : 									Planning Manager: __________________ License No.: __________ Date: _____________"
                chkNotMonitoredValues.Checked = False 'Added by Saylee on 11-May-2015 for ALL11052015
            End If
            'End
            ListServiceType.Enabled = False
            ListInspectionType.Enabled = False
            ListDirectiveType.Enabled = False

        End If
        SetSession()
        'MessageBoxResult()
        EnabledDisabledButons()                   'Added Code
        'Added By Utkarsh ON 12-Sep-2013 FOR BA11092013
        If (mOpen = 2 Or mOpen = Open.ServiceReport) AndAlso cmbFormat.SelectedIndex = 2 Then 'APFT
            Label7.Visible = True
            gdPerDayLimit.Visible = True
            'lblStep6.Text = "Step XI. Display Report"
            'Label7.Text = "Step IX.  Estimated Flying Hours"
        Else
            Label7.Visible = False
            gdPerDayLimit.Visible = False
            If AppSettings("ShowMaintenanceForNewClients") = "True" And mOpen = Open.ServiceReport Then
                'lblStep6.Text = "Step VII. Display Report"
            Else
                'lblStep6.Text = "Step VIII. Display Report"
            End If
        End If
        'End
        'Added Code
        ' SetCombo()                            'Added Code
        Select Case mOpen
            Case Open.CofAReport
                If AppSettings("ClientCode") = "BA" Then 'Added by Saylee on 22-Mar-2021 for BA22032021 as per mail on 20-Mar
                    lbltitle.Text = "MPD Task Reference Report"
                Else
                    lbltitle.Text = "Search criteria for C of A"
                End If

                chkAssembly.Text = IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Show Assembly AMPs", "Show Assembly Insps/Services") '"Show Assembly Insps/Services"
                chkComponent.Text = IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Show Component AMPs", "Show Component Insps/Services") '"Show Component Insps/Services"
            Case Open.RoutineInspectionReport
                lbltitle.Text = "Search criteria for Inspection Report"
                chkAssembly.Text = "Show Assembly Inspections"
                chkComponent.Text = "Show Component Inspections"
            Case Open.ModificationReport
                lbltitle.Text = "Search criteria for Modification Report"
                chkAssembly.Text = "Show Assembly Directives"
                chkComponent.Text = "Show Component Modifications"
                'APFT
            Case Open.ServiceReport
                lbltitle.Text = IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Search criteria for AMP Report", "Search criteria for Service Report")  '"Search criteria for Service Report"
                chkAssembly.Text = IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Show Assembly AMPs", "Show Assembly Service")  '"Show Assembly Service"
                chkComponent.Text = IIf(AppSettings("ShowMaintenanceForNewClients") = "True", "Show Component AMPs", "Show Component Service")  '"Show Component Service"
        End Select

        cmbType.DataBind()
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
        pnlCriteria.Visible = True
        upnlCurrentCriteria.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid Then
            IsExcel = False
            SetReport(False)
        Else
            upnlTitle.Update()
            Exit Sub
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mMachineList = Nothing
        mAssemblyList = Nothing
        mServiceTypeList = Nothing
        mInspectionTypeList = Nothing
        mModificationTypeList = Nothing
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub txtFromDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        AOdate = txtFromDate.Text.ToString
        If AOnDate.Equals(AOdate) Then
        Else
            SetComboOfMachine(AOdate)
        End If
    End Sub
    'P  'Private Sub optLandscape_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optLandscape.CheckedChanged
    '    If optLandscape.Checked = True Then
    '        Report = 0
    '        Session("ReportStatus") = ReportStatus
    '    Else
    '        Report = 1
    '        Session("ReportStatus") = ReportStatus
    '    End If
    'End Sub
    'Private Sub optPortrait_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optPortrait.CheckedChanged
    '    If optPortrait.Checked = True Then
    '        Report = 1
    '        Session("ReportStatus") = ReportStatus
    '    Else
    '        Report = 0
    '        Session("ReportStatus") = ReportStatus
    '    End If
    'End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        If cmbAircraft.SelectedIndex = 0 Then
            lblAssembly.Enabled = False
            cmbAssembly.Enabled = False
            AircraftIndex = cmbAircraft.SelectedIndex
            Session("AircraftIndex") = AircraftIndex
        Else
            lblAssembly.Enabled = True
            cmbAssembly.Enabled = True


            '' mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtFromDate.Text.ToString, cmbAircraft.SelectedValue, , , , , , , , , , True, , , , , , , , , , , , , , , , , , , True).Item(1), MachineInfo).AssemblyStatusList
            ''cmbAssembly.DataSource = mAssemblyStatusList
            ''Session("mAssemblyStatusList") = mAssemblyStatusList
            ''cmbAssembly.DataBind()

            Dim mAssemblylist As AssemblyList
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, txtFromDate.Text.ToString, "(All)", True)

            Session("mAssemblyList") = mAssemblylist
            cmbAssembly.DataSource = mAssemblylist
            cmbAssembly.DataBind()

            AircraftIndex = cmbAircraft.SelectedIndex
            Session("AircraftIndex") = AircraftIndex

        End If
        If cmbAircraft.Enabled = True Then
            setFocus(cmbAircraft)
        End If
        SetTypeCombo()
        DataFieldBind() 'Added By Utkarsh ON 12-Sep-2013 FOR BA11092013
        FillTypeCombo()
        'Ajay 10-Nov-2022
        If IsMarkedFavourite(HttpContext.Current.User.Identity.Name, "CofA") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "MarkFav", "MarkFav();", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "RemoveFav", "RemoveFav();", True)
        End If
        '--------------------------
        upnlTitle.Update()
    End Sub
    Private Sub cmbType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbType.SelectedIndexChanged
        Try
            Dim j As Integer
            For j = 0 To cmbType.Items.Count - 1

                'ListServiceType Enabled
                If cmbType.Items(j).Selected = True And (cmbType.Items(j).Text = "Service" Or cmbType.Items(j).Text = "MPD") Then
                    ListServiceType.Enabled = cmbType.Items.Item(cmbType.SelectedIndex).Selected
                    hdnService.Value = cmbType.Items.Item(cmbType.SelectedIndex).Selected
                    upnlServiceType.Update()
                End If

                'ListInspectionType Enabled
                If cmbType.Items(j).Selected = True And cmbType.Items(j).Text = "Inspection" Then
                    ListInspectionType.Enabled = cmbType.Items.Item(cmbType.SelectedIndex).Selected
                    hdnInspection.Value = cmbType.Items.Item(cmbType.SelectedIndex).Selected
                    upnlInspectionType.Update()

                End If

                'ListDirectiveType Enabled
                If cmbType.Items(j).Selected = True And (cmbType.Items(j).Text = "Modification" Or cmbType.Items(j).Text = "Directive") Then
                    ListDirectiveType.Enabled = cmbType.Items.Item(cmbType.SelectedIndex).Selected
                    hdnDirective.Value = cmbType.Items.Item(cmbType.SelectedIndex).Selected
                    upnlModificationType.Update()
                End If
            Next

            Dim k As Integer
            For k = 0 To cmbType.Items.Count - 1

                'cmbService Disabled
                If cmbType.Items(k).Selected = False And (cmbType.Items(k).Text = "Service" Or cmbType.Items(k).Text = "MPD") Then
                    ListServiceType.Enabled = False
                    hdnService.Value = False
                    upnlServiceType.Update()
                End If

                'cmbInspection Disabled
                If cmbType.Items(k).Selected = False And cmbType.Items(k).Text = "Inspection" Then
                    ListInspectionType.Enabled = False
                    hdnInspection.Value = False
                    upnlInspectionType.Update()
                End If

                'cmbModification Disabled
                If cmbType.Items(k).Selected = False And (cmbType.Items(k).Text = "Modification" Or cmbType.Items(k).Text = "Directive") Then
                    ListDirectiveType.Enabled = False
                    hdnDirective.Value = False
                    upnlModificationType.Update()
                End If

            Next
            upnlImgBtn.Update()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "disableEnable", "disableEnable();", True)
        Catch ex As Exception
        End Try
        upnType.Update()
    End Sub
    Private Sub ListServiceType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListServiceType.SelectedIndexChanged
        Session("SerIndex") = SerIndex
    End Sub
    Private Sub ListInspectionType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListInspectionType.SelectedIndexChanged
        Session("InspIndex") = InspIndex
    End Sub
    Private Sub ListDirectiveType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListDirectiveType.SelectedIndexChanged
        Session("ModIndex") = ModIndex
    End Sub
    Private Sub cmbShowCofA_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbShowCofA.SelectedIndexChanged
        SofAIndex = cmbShowCofA.SelectedIndex
        Session("SofAIndex") = SofAIndex

        If SofAIndex = 0 Then
            ShowCofA = False
        Else
            ShowCofA = True
        End If
        Session("ShowCofA") = ShowCofA

        If cmbShowCofA.Enabled = True Then
            setFocus(cmbShowCofA)
        End If
    End Sub
    'Added By Utkarsh ON 12-Sep-2013 FOR BA11092013
    Private Sub cmbFormat_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbFormat.SelectedIndexChanged
        If cmbFormat.SelectedValue = 2 And (mOpen = 2 Or mOpen = Open.ServiceReport) Then 'APFT
            Label7.Visible = True
            gdPerDayLimit.Visible = True
            'mPerDayLimits = PerDayLimits.GetPerDayLimits(New Guid(cmbAircraft.SelectedValue.ToString))
            'gdPerDayLimit.DataSource = mPerDayLimits
            'Session("mPerDayLimits") = mPerDayLimits
            'gdPerDayLimit.DataBind()
            'lblCMPRefHeader.InnerText = "Step X. CMP Reference"
        Else
            Label7.Visible = False
            gdPerDayLimit.Visible = False
            'lblCMPRefHeader.InnerText = "Step VIII. CMP Reference"
        End If
    End Sub
    'end
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click

        Dim PeriodColumnsForExportToExcel As New List(Of String)
        If IsValid = True Then
            IsExcel = True
            ReportMaintenanceDetails = Nothing
            Report = Nothing
            ReportStatusList = Nothing
            Dim da As New CSLA.Data.ObjectAdapter
            Dim ds As New dsReportMaintenanceDetail

            ReportStatusList = New rptStatusList
            ReportMaintenanceDetails = New ReportMaintenanceDetailList


            SetReport()

            If ReportMaintenanceDetails.Count = 0 Then
                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                Exit Sub

            End If

            ds.Clear()


            da.Fill(ds, "ExcelReportMaintenanceDetailList", ReportMaintenanceDetails)
            ' da.Fill(ds, "ExcelReportStatusList", ReportStatusList)
            da.Fill(ds, "ExcelReport", Report)

            Dim columnToRemove As String() = {
                                                "ID",
                                                "Code",
                                                "Name",
                                                "Model",
                                                "EstDate",
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
                                                "AssemblySerialNo",
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
                                                "InstalledAt",
                                                "InstalledAt1",
                                                "InstalledAt2",
                                                "TSO1",
                                                "TSO2",
                                                "RemoveAt1",
                                                "RemoveAt2",
                                                "ModificationNumber",
                                                "DoneWONo",
                                                "DetailID",
                                                "Applicability",
                                                "ComplianceRequirement",
                                                "AssemblyDueAsof",
                                                "AssemblyDueAsof1",
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
                                                "DoneOnValue",
                                                "DoneOnDate",
                                                "RemoveAt",
                                                "ATACode",
                                                "InstalledAtDate",
                                                "RemoveAtDate",
                                                "TSO",
                                                "TSN",
                                                "DoneONValueForAssembly",
                                                "RecordID",
                                                "MachineID",
                                                "ModelID",
                                                "IsMaster",
                                                "DiffCompInstDoneOnValue",
                                                "EffectiveFromAll",
                                                "MaintenanceOnExcel",
                                                "MaintenanceInformationExcel",
                                                "MaintenanceInfoExcel",
                                                "FrequencyExcel",
                                                "SinceNewAllExcel",
                                                "ElapsedAllExcel",
                                                "EffectiveFromAllExcel",
                                                "DoneAtAllExcel",
                                                "ExtensionAllExcel",
                                                "DueAsofAllExcel",
                                                "AssDueAsofAllExcel",
                                                "RemainingTimeAllExcel", "EROQtyNosForMaterialMgmtReport", "POQtyNosForMaterialMgmtReport",
                                                "PONosForMaterialMgmtReport", "POQtyForMaterialMgmtReport", "ERONosForMaterialMgmtReport",
                                                "EROQtyForMaterialMgmtReport", "UnserviceableStockQty", "ServiceableStockQty", "BinCardTotalQty",
                                                "Zone", "Freq1",
                                                "DueAsOfAssemblyOrCompForExcel", "DueAsOfAirframeForExcel", "RemainingForExcel", "DescriptionForExcel",
                                                "DueAsOfForExcel", "NoteForExcel", "ReferenceForExcel", "RemainingTimeForExcel", "DataColumn1"
                                        }
            For i As Integer = 0 To columnToRemove.Length - 1
                If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains(columnToRemove(i)) Then
                    ds.Tables("ExcelReportMaintenanceDetailList").Columns.Remove(columnToRemove(i))
                End If
            Next


            If cmbFormat.SelectedIndex = 2 AndAlso (mOpen = 2 Or mOpen = Open.ServiceReport) Then 'APFT
                'do nothing
            Else
                ds.Tables("ExcelReportMaintenanceDetailList").Columns.Remove("EstimatedDate")
            End If

            If Not (AppSettings("ClientCode") = "STR" And cmbFormat.SelectedIndex = 0 And chkTaskCard.Checked = False And (mOpen = Open.CofAReport Or mOpen = Open.RoutineInspectionReport)) Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns.Remove("AssemblyDueAsof2")
            End If



            If AppSettings("ShowMaintenanceForNewClients") = "True" Then

                columnToRemove = {"DescriptionForExcel", "Reference", "TaskNo", "MaintenanceInformationForExcel", "ApplicabilityForExcel", "CompSerialNo",
                                  "MaintenanceActivityType", "HoursFreq", "CyclesFreq", "Extension", "DaysMnthsYrsName", "DaysMnthsYrsValue", "SinceNew2",
                                  "SinceNew2", "LandingsFreq", "HoursDoneOnValue", "CyclesDoneOnValue", "DaysMnthsYrsDoneOnValue", "LandingsDoneOnValue",
                                  "PartNo", "Position", "Manufacturer", "InstallationWONo", "InstallationRemark", "InstallationDoneBy", "InstPlace",
                                  "TSNHours", "SinceNewDate", "SinceNewLandings", "CSNCycles", "InstCompHours", "InstCompStartDate", "InstCompLandings",
                                  "InstCompCycles", "AssemblyInstHours", "AssemblyInstStartDate", "AssemblyInstLandings", "AssemblyInstCycles",
                                  "PartMonitorCode", "PartDesc", "MonitorTypeWithCode", "Description", "ElapsedTime", "SourceDoc",
                                  "MonitorTypeCode", "TimeSinceNew", "Area", "IsRII", "ReqNumber", "LinkedMaintenanceActivityCount", "PartNoSerialNoforExcel",
                                  "TSO1ForExcel", "TSOForExcel", "InstalledAtForExcel", "Freq1ForExcel", "TSNForExcel", "DoneOnValueForExcel", "Skill", "SkillID",
                                  "WONoExcel", "MethodOfCompliance"
                                 }
                '"ATAChapter"
                For i As Integer = 0 To columnToRemove.Length - 1
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains(columnToRemove(i)) Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns.Remove(columnToRemove(i))
                    End If
                Next

                ds.Tables("ExcelReportMaintenanceDetailList").Columns("TaskNoExcel").SetOrdinal(0)
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("DescriptionSourceDocForExcel").SetOrdinal(1)
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("TaskReferenceForExcel").SetOrdinal(2)
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("ModelEstimatedManHours").SetOrdinal(3)

                ' ds.Tables("ExcelReportMaintenanceDetailList").Columns("Freq1").SetOrdinal(5)
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("ThresholdAccordingToTypeIDForExcel").SetOrdinal(4)
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("FrequencyAccordingToTypeIDForExcel").SetOrdinal(5)
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("DoneAt2").SetOrdinal(6)
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("DueAsof").SetOrdinal(7)
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("RemainingTime").SetOrdinal(8)
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("Note").SetOrdinal(9)

                For i As Integer = 0 To ds.Tables("ExcelReportMaintenanceDetailList").Columns.Count - 1
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "TaskNoExcel" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "TaskNo"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "DescriptionSourceDocForExcel" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Description"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "TaskReferenceForExcel" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Reference (Document)"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "ModelEstimatedManHours" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Estd.Man Hours"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "DoneAt2" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Last Carried Out / Start"
                    End If
                    'If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Freq1" Then
                    '    ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Interval"
                    'End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "ThresholdAccordingToTypeIDForExcel" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Threshold"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("FrequencyAccordingToTypeIDForExcel") Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns("FrequencyAccordingToTypeIDForExcel").ColumnName = "Frequency"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "DueAsof" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Next Due"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "RemainingTime" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Remaining"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "RemainingTime" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Remaining"
                    End If
                Next
            Else
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("CompSerialNo").SetOrdinal(3) 'shifted to 3rd column
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("ThresholdAccordingToTypeIDForExcel").SetOrdinal(5)
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("FrequencyAccordingToTypeIDForExcel").SetOrdinal(6)
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("Extension").SetOrdinal(7)
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("DoneAt2").SetOrdinal(8)
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("TimeSinceNew").SetOrdinal(11)
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("SinceNew2").SetOrdinal(12)

                Dim columnscnt As Integer = ds.Tables("ExcelReportMaintenanceDetailList").Columns.Count
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("Remark").SetOrdinal(columnscnt - 1)
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("Note").SetOrdinal(columnscnt - 2)
                ds.Tables("ExcelReportMaintenanceDetailList").Columns("Reference").SetOrdinal(columnscnt - 3)


                Dim DueLabel As String = "DueAsof"
                For i As Integer = 0 To ds.Tables("ExcelReportMaintenanceDetailList").Columns.Count - 1
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "ThresholdAccordingToTypeIDForExcel" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Threshold"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains("FrequencyAccordingToTypeIDForExcel") Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns("FrequencyAccordingToTypeIDForExcel").ColumnName = "Frequency"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "MonitorTypeCode" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Monitor Type"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "AssemblyDueAsof2" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Due As of Comp./Assembly"
                    End If
                    If (AppSettings("ClientCode") = "RAL" Or AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Or chkAirframeDueAsOf.Checked Then
                        If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "DueAsof" Then
                            ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Due As of Airframe"
                            DueLabel = "Due As of Airframe"
                        End If
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "SinceNew2" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Since Overhaul"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "DoneAt2" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Done At"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "TimeSinceNew" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Since New"
                    End If
                    If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Area" Then
                        ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Zone"
                    End If

                Next
            End If






            Dim columnToRemoveCriteria As String() = {"ReportDate", "ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite",
                                                       "ReportName", "SearchStr2", "SearchStr3", "SearchStr4", "SearchStr5", "SearchStr6", "SearchStr7",
                                                       "ProductVersion", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr10", "SearchStr11",
                                                       "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17",
                                                      "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25", "SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40", "SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47", "SearchStr48", "SearchStr49", "SearchStr50","SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95","SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"
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
                If ds.Tables("ExcelReport").Columns(i).ColumnName = "SearchStr8" Then
                    ds.Tables("ExcelReport").Columns(i).ColumnName = "Reg No."
                End If
                If ds.Tables("ExcelReport").Columns(i).ColumnName = "SearchStr9" Then
                    ds.Tables("ExcelReport").Columns(i).ColumnName = "Assembly"
                End If
            Next

            Dim dataview As DataView = ds.Tables("ExcelReportMaintenanceDetailList").DefaultView
            dataview.Sort = "ATAChapter"
            ds.Tables("ExcelReportMaintenanceDetailList").TableName = ReportLabel.Replace("/", " ")

            '  ds.Tables("ExcelReportStatusList").TableName = "Searching Criteria"
            ds.Tables("ExcelReport").TableName = "Searching Criteria"
            Session("DataTableToBeFormattedForExportToExcel") = ReportLabel.Replace("/", " ")
            Dim dsNew As New DataSet
            dsNew.Clear()


            dsNew.Merge(ds.Tables("Searching Criteria"))
            dsNew.Merge(dataview.ToTable())

			Session("ExcelFileName") = ReportLabel.Replace("/", " ")

			PeriodColumnsForExportToExcel.AddRange(New String() {"Frequency", "ElapsedTime", "RemainingTime", "DueLabel", "DoneOn Value", "Since New", "Done At", "Since Overhaul", "Due As of Comp./Assembly"})
            Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel


            'Dim list = (From c In ds.Tables("ExcelReportMaintenanceDetailList") Order By ATAChapter
            '                             Select c Order By ATAChapter).ToList

            Session("dsNew") = dsNew
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
            'Added by Prashant on 19-Jan-2021
            Dim mName As String = String.Empty
            If mOpen = 1 Then
                mName = "CofA"
            ElseIf mOpen = 2 Then
                mName = "InspectionReport"
            ElseIf mOpen = 3 Then
                mName = "ModificationReport"
            ElseIf mOpen = Open.ServiceReport Then 'APFT
                mName = "ServiceReport"
            End If
            MarkLog(Util.Action.Print, mName, "Export To Excel " + mCofASearchingCriteria + " " + ReportLabel, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        End If
    End Sub
    'Added by Shital on 14-Sep-2016
    Private Sub btnByMail_Click(sender As Object, e As System.EventArgs) Handles btnByMail.Click
        'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        ' Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail
        If mOpen = 1 Then
            Session("UserEmailID") = mModuleList.Item("CofA").SendToMailID
            Session("UserCcEmailID") = mModuleList.Item("CofA").SendCCMailID
            Session("SmtpHost") = mModuleList.Item("CofA").SmtpHost
            Session("SmtpPort") = mModuleList.Item("CofA").SmtpPort
            Session("SmtpUser") = mModuleList.Item("CofA").SmtpUser
            Session("SmtpPassword") = mModuleList.Item("CofA").SmtpPassword
        ElseIf mOpen = 2 Then
            Session("UserEmailID") = mModuleList.Item("InspectionReport").SendToMailID
            Session("UserCcEmailID") = mModuleList.Item("InspectionReport").SendCCMailID
            Session("SmtpHost") = mModuleList.Item("InspectionReport").SmtpHost
            Session("SmtpPort") = mModuleList.Item("InspectionReport").SmtpPort
            Session("SmtpUser") = mModuleList.Item("InspectionReport").SmtpUser
            Session("SmtpPassword") = mModuleList.Item("InspectionReport").SmtpPassword
        ElseIf mOpen = 3 Then
            Session("UserEmailID") = mModuleList.Item("ModificationReport").SendToMailID
            Session("UserCcEmailID") = mModuleList.Item("ModificationReport").SendCCMailID
            Session("SmtpHost") = mModuleList.Item("ModificationReport").SmtpHost
            Session("SmtpPort") = mModuleList.Item("ModificationReport").SmtpPort
            Session("SmtpUser") = mModuleList.Item("ModificationReport").SmtpUser
            Session("SmtpPassword") = mModuleList.Item("ModificationReport").SmtpPassword
        ElseIf mOpen = 5 Then
            Session("UserEmailID") = mModuleList.Item("ServiceReport").SendToMailID
            Session("UserCcEmailID") = mModuleList.Item("ServiceReport").SendCCMailID
            Session("SmtpHost") = mModuleList.Item("ServiceReport").SmtpHost
            Session("SmtpPort") = mModuleList.Item("ServiceReport").SmtpPort
            Session("SmtpUser") = mModuleList.Item("ServiceReport").SmtpUser
            Session("SmtpPassword") = mModuleList.Item("ServiceReport").SmtpPassword
        End If

        If Session("UserEmailID") = "" Then
            Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail
        End If
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
    Private Sub chkTaskCard_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkTaskCard.CheckedChanged
        If chkTaskCard.Checked Then
            cmbFormat.ClearSelection()
            cmbFormat.Enabled = False
        Else
            cmbFormat.Enabled = True
        End If
    End Sub
    'Ajay 09-Nov-2022
    Private Sub hdnBtnMarkFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnMarkFav.Click 'Ajay 08-Nov-2022
        MarkFavourite(HttpContext.Current.User.Identity.Name, "CofA")
    End Sub

    Private Sub hdnBtnRemoveFav_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles hdnBtnRemoveFav.Click 'Ajay 08-Nov-2022
        RemoveFavourite(HttpContext.Current.User.Identity.Name, "CofA")
    End Sub
    '-----
#End Region


End Class