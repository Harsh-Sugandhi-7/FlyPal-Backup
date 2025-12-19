'Created by :   Saylee
'Date       :   24-June-2009

Partial Class wfMultiCompliance
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents rfvFromDate As System.Web.UI.WebControls.RequiredFieldValidator
    '  Protected WithEvents lblAvgMnths As System.Web.UI.WebControls.Label
    '   Protected WithEvents txtAvgMnths As System.Web.UI.WebControls.TextBox
    ' Protected WithEvents lblMonths As System.Web.UI.WebControls.Label
    Protected WithEvents txtAsOnDate As SIControls.SICalendar
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "Enumeration"
    Enum Open
        CofAReport = 1
        RoutineInspectionReport = 2
        ModificationReport = 3
        DueReport = 4
    End Enum

    Enum StatusType
        AssemblyService = 1
        AssemblyInspection = 2
        AssemblyDirective = 3
        ComponentService = 4
        ComponentInspection = 5
        ComponentDirective = 6
    End Enum

#End Region

#Region " Variable Declaration "
    Dim mDueLimits As DueLimits

    Dim mPerDayLimits As PerDayLimits

    Dim ReportStatusList As New rptStatusList
    Dim mMachineList As MachineList

    Dim mtmpMachineList As tmpMachineList
    Dim ReportMaintenanceDetails As New ReportMaintenanceDetailList
    Dim mReportMaintenanceDetail As New ReportMaintenanceDetail

    Dim ObjMachineList As MachineList
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
    Dim mAssemblyStatusList As AssemblyStatusList
    Dim AssemblyName As String
    Dim Assembly1 As String
    Dim TypeName As String
    Public mOpen As Open
    Dim mTypeListForCofA As TypeListForCofA
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

    Public mStatusType As StatusType
    Private AssemblyStatusID As String
    Private ModelID As String
    Dim CompStatusID As Guid
    Dim StatusID As Guid
    Dim LogId As Guid
    Dim LogDate As String
    Dim AssemblyStatusPeriodList As AssemblyStatusPeriodList
    Dim tmpAssemblyStatusID As Guid

#End Region

#Region " Helper Methods "
    Private Sub SetGridObject()
        Dim txtLimit As TextBox
        Dim i As Int32
        For i = 0 To Me.dgDuePeriodLimits.Items.Count - 1
            txtLimit = CType(Me.dgDuePeriodLimits.Items(i).FindControl("txtLimit"), TextBox)
            mDueLimits.Item(i).PeriodLimit = CDec(Val(Trim(txtLimit.Text)))
        Next i
        Session("mDueLimits") = mDueLimits

    End Sub
    Private Sub GetSession()
        mMachineList = CType(Session("mMachineListForCompliance"), MachineList)
        mDueLimits = CType(Session("mDueLimits"), DueLimits)
        mPerDayLimits = CType(Session("mPerDayLimits"), PerDayLimits)

        AOnDate = Session("AOnDate")
        Report = Session("Report")
        Type = Session("Type")
        AvgMnths = Session("AvgMnths")

        DueType = Session("DueType")

        mAssemblyStatusList = CType(Session("mAssemblyStatusList"), AssemblyStatusList)
        mServiceTypeList = CType(Session("mServiceTypeList"), PartMonitorServiceTypeList)
        mInspectionTypeList = CType(Session("mInspectionTypeList"), ModelMonitorInspTypeList)
        mModificationTypeList = CType(Session("mModificationTypeList"), ModelMonitorModTypeList)

        AsonDate = Session("AsonDate")
        MachineName = Session("AircraftId")
        AssemblyName = Session("AssemblyId")
        ReportMaintenanceDetails = Session("ReportMaintenanceDetails")
        AssemblyStatusPeriodList = Session("AssemblyStatusPeriodList")
    End Sub
    Private Sub SetSession()
        Session("mMachineListForCompliance") = mMachineList
        Session("mDueLimits") = mDueLimits
        Session("mPerDayLimits") = mPerDayLimits
        Session("AOnDate") = AOnDate
        Session("Report") = Report
        Session("Type") = Type
        Session("AvgMnths") = AvgMnths
        Session("DueType") = DueType

        'Added by Saylee on 12-Feb-2009
        Session("mAssemblyStatusList") = mAssemblyStatusList
        Session("SerIndex") = SerIndex
        Session("InspIndex") = InspIndex
        Session("ModIndex") = ModIndex
        Session("mServiceTypeList") = mServiceTypeList
        Session("mInspectionTypeList") = mInspectionTypeList
        Session("mModificationTypeList") = mModificationTypeList
    End Sub
    Private Sub ClearAll()
        DueType = Session("DueType")
        If Session("MiddleFrame") <> "wfMultiCompliance.aspx?" Then
            Session.Remove("mMachineListForCompliance")
            Session.Remove("mDueLimits")
            Session.Remove("mPerDayLimits")
            Session.Remove("AOnDate")
            Session.Remove("Report")
            Session.Remove("Type")
            Session.Remove("AvgMnths")

            Session.Remove("mAssemblyStatusList")
            Session.Remove("SerIndex")
            Session.Remove("InspIndex")
            Session.Remove("ModIndex")
            Session.Remove("LogId")
            Session.Remove("OpenFindNowSelectLogForm")
            Session.Remove("AssemblyStatusPeriodList")
            Session.Remove("AircraftId")
            Session.Remove("mLogList")
        End If
    End Sub
    Private overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        Dim str As String
        str = "<script language='javascript'>  document.getElementById('" + cntrl.ClientID + "').focus();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "focusscript", str)
    End Sub

    Private Sub FillTypeCombo()
        Dim j As Integer

        For j = 0 To cmbType.Items.Count - 1
            cmbType.Items(j).Selected = True
        Next

        For j = 0 To cmbType.Items.Count - 1

            'cmbServiceType Enabled
            If cmbType.Items(j).Selected = True And cmbType.Items(j).Text = "Service" Then
                cmbServiceType.Enabled = cmbType.Items.Item(cmbType.SelectedIndex).Selected
                For i As Integer = 0 To cmbServiceType.Items.Count - 1
                    cmbServiceType.Items.Item(i).Selected = cmbServiceType.Enabled
                Next
            End If

            'cmbInspectionType Enabled
            If cmbType.Items(j).Selected = True And cmbType.Items(j).Text = "Inspection" Then
                cmbInspectionType.Enabled = cmbType.Items.Item(cmbType.SelectedIndex).Selected
                For i As Integer = 0 To cmbInspectionType.Items.Count - 1
                    cmbInspectionType.Items.Item(i).Selected = cmbInspectionType.Enabled
                Next
            End If

            'cmbModificationType Enabled
            If cmbType.Items(j).Selected = True And (cmbType.Items(j).Text = "Modification" Or cmbType.Items(j).Text = "Directive") Then
                cmbModificationType.Enabled = cmbType.Items.Item(cmbType.SelectedIndex).Selected
                For i As Integer = 0 To cmbModificationType.Items.Count - 1
                    cmbModificationType.Items.Item(i).Selected = cmbModificationType.Enabled
                Next
            End If
        Next

        Dim k As Integer
        For k = 0 To cmbType.Items.Count - 1

            'cmbService Disabled
            If cmbType.Items(k).Selected = False And cmbType.Items(k).Text = "Service" Then
                cmbServiceType.Enabled = False
                For l As Integer = 0 To cmbServiceType.Items.Count - 1
                    cmbServiceType.Items.Item(l).Selected = cmbServiceType.Enabled = False
                Next
            End If

            'cmbInspection Disabled
            If cmbType.Items(k).Selected = False And cmbType.Items(k).Text = "Inspection" Then
                cmbInspectionType.Enabled = False
                For l As Integer = 0 To cmbInspectionType.Items.Count - 1
                    cmbInspectionType.Items.Item(l).Selected = cmbInspectionType.Enabled = False
                Next
            End If

            'cmbModification Disabled
            If cmbType.Items(k).Selected = False And (cmbType.Items(k).Text = "Modification" Or cmbType.Items(k).Text = "Directive") Then
                cmbModificationType.Enabled = False
                For l As Integer = 0 To cmbModificationType.Items.Count - 1
                    cmbModificationType.Items.Item(l).Selected = cmbModificationType.Enabled = False
                Next
            End If
        Next
    End Sub
    Private Sub SetValues()
        If (cmbAircraft.SelectedItem.Text = "(All)") Or (cmbAircraft.SelectedItem.Text = "<SELECT>") Then
            MachineName = "{00000000-0000-0000-0000-000000000000}"
            'AssemblyName = "{00000000-0000-0000-0000-000000000000}"
            AssemblyName = Guid.Empty.ToString
            Assembly1 = ""
        Else
            MachineName = cmbAircraft.SelectedValue.ToString

            If cmbAssembly.SelectedItem.Text = "(All)" Or (cmbAssembly.SelectedItem.Text = "<SELECT>") Then
                'AssemblyName = "{00000000-0000-0000-0000-000000000000}"
                AssemblyName = Guid.Empty.ToString
                Assembly1 = ""
                AssemblyType = "(All)"
                AssemblyStatusID = "{00000000-0000-0000-0000-000000000000}"

                If Session("LogId") <> "" Or Not Session("LogId") Is Nothing Then
                    '' SetLog()
                    'do nothing
                Else
                    Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtAsOnDate.Value.ToString, cmbAircraft.SelectedValue, , , , , , , , , , True, , , , "Airframe").Item(0), MachineInfo).AssemblyStatusList
                    AssemblyStatusPeriodList = tmpAssemblyStatusList(0).AssemblyStatusPeriodList
                    Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
                    tmpAssemblyStatusList = Nothing
                End If

            Else
                AssemblyType = mAssemblyStatusList(cmbAssembly.SelectedIndex).AssemblyType
                AssemblyName = cmbAssembly.SelectedValue.ToString
                Assembly1 = cmbAssembly.SelectedItem.Text
                AssemblyStatusID = (mAssemblyStatusList(cmbAssembly.SelectedIndex).ID).ToString
                ModelID = (mAssemblyStatusList(cmbAssembly.SelectedIndex).ModelID).ToString

                If Session("LogId") <> "" Or Not Session("LogId") Is Nothing Then
                    'do nothing
                Else
                    AssemblyStatusPeriodList = mAssemblyStatusList(cmbAssembly.SelectedIndex).AssemblyStatusPeriodList
                    Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
                End If

            End If

            dgDoneOnValue.DataSource = AssemblyStatusPeriodList
            dgDoneOnValue.DataBind()

        End If
        ''Average = txtAvgMnths.Text
        If Not (txtAsOnDate.IsDateValue) Then
            AsonDate = ""
            AOnDate = ""
        Else
            AsonDate = txtAsOnDate.Value.ToString
            AOnDate = txtAsOnDate.Value.ToString
        End If
        Aircraft = IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "")

        Session("AsonDate") = AsonDate
        Session("AonDate") = AOnDate
        Session("AircraftId") = MachineName
        Session("AssemblyId") = AssemblyName
        Session("AssemblyType") = AssemblyType
        Session("Aircraft") = Aircraft

        If cmbType.Items.Count <> 0 Then
            ' If so, loop through all checked items and print results.
            Dim x As Integer

            For x = 0 To cmbType.Items.Count - 1
                'info = mTypeListForCofA.Item(x)
                'If info.Name = "Service" Then   'Service
                If cmbType.Items(x).Selected = True And cmbType.Items(x).Text = "Service" Then   'For showing report name and to set correct  'selected' value
                    IsSerSelect = True
                    For K As Integer = 0 To cmbServiceType.Items.Count - 1
                        If cmbServiceType.Items.Item(k).Selected Then
                            ServiceTypeID(k) = cmbServiceType.Items.Item(k).Value
                        End If
                    Next
                    'End If
                End If      'Added Code

                'info = mTypeListForCofA.Item(x)
                'If info.Name = "Inspection" Then   'Inspection
                If cmbType.Items(x).Selected = True And cmbType.Items(x).Text = "Inspection" Then
                    IsInsSelect = True

                    'Dim b As Integer = 0
                    'If cmbInspectionType.Items.Item(b).Selected Then           'Added Code
                    'InspectionTypeID(0) = 0
                    'Else

                    For K As Integer = 0 To cmbInspectionType.Items.Count - 1
                        If cmbInspectionType.Items.Item(k).Selected Then
                            ''Dim info1 As ModelMonitorInspTypeList.ModelMonitorInspTypeInfo
                            ''info1 = mInspectionTypeList.Item(k)
                            ''InspectionTypeID(k) = info1.ID

                            InspectionTypeID(k) = cmbInspectionType.Items.Item(k).Value

                        End If
                    Next
                    'End If
                End If                       'Added Code

                'info = mTypeListForCofA.Item(x)
                'If info.Name = "Modification" Then   'Modification
                If cmbType.Items(x).Selected = True And (cmbType.Items(x).Text = "Modification" Or cmbType.Items(x).Text = "Directive") Then
                    IsModSelect = True
                    '  Dim a As Integer = 0
                    '  If cmbModificationType.Items.Item(a).Selected Then
                    ' ModificationTypeID(0) = 0
                    'Else

                    For K As Integer = 0 To cmbModificationType.Items.Count - 1
                        If cmbModificationType.Items.Item(k).Selected Then
                            'Dim info1 As ModelMonitorModTypeList.ModelMonitorModTypeInfo
                            'info1 = mModificationTypeList.Item(k)
                            'ModificationTypeID(k) = info1.ID

                            ModificationTypeID(k) = cmbModificationType.Items.Item(k).Value

                        End If
                    Next
                    ' End If
                End If                  'Added Code

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
    End Sub
    Private Sub SetLog()
        'If Val(Request.QueryString("Type")) = -1 Then
        If Session("LogId") <> "" Or Not Session("LogId") Is Nothing Then

            LogId = New Guid(CType(Session("LogId"), String))
            Session("LogId") = CType(Session("LogId"), String)

            Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(AsonDate, MachineName, , , , , , , , , , True, , , AssemblyName, , , , , , , , , , , , , , , , , , LogId.ToString).Item(0), MachineInfo).AssemblyStatusList
            AssemblyStatusPeriodList = tmpAssemblyStatusList(0).AssemblyStatusPeriodList
            Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
            dgDoneOnValue.DataSource = AssemblyStatusPeriodList
            dgDoneOnValue.DataBind()
            tmpAssemblyStatusList = Nothing
        Else
        End If
    End Sub
    Private Sub ResetValues()
        MachineName = "{00000000-0000-0000-0000-000000000000}"
        'CNDC
        'txtFromDate.Value = AsonDate
        If AsonDate <> "" Then
            txtAsOnDate.Value = AsonDate
        End If
        AsonDate = ""
        AvgMnths = 0

        IsSerSelect = False
        IsInsSelect = False
        IsModSelect = False
        ServiceTypeID(0) = 0
        InspectionTypeID(0) = 0
        ModificationTypeID(0) = 0
        'AssemblyName = "{00000000-0000-0000-0000-000000000000}"
        AssemblyName = Guid.Empty.ToString
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

        '' If rbdPercent.Checked Then mDueLimits.SetPercentageWise(True, CDec(Val(txtPercentage.Text)))

        mMachineList = MachineList.GetMachineListDueMonitoringStatus(AsonDate, mDueLimits, MachineName, AssemblyName)
        Dim LHLabel2 As String = ""
        Dim LHData2 As String = ""
        If Not cmbAircraft.SelectedItem.ToString = "(All)" Then
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
                    AssemblyStatusID = ObjAssemblyStatus.ID.ToString
                    'ReportStatusList.Add(New rptStatus(AssemblyID.ToString, 0, , ObjAssemblyStatus.AssemblyType + " " + "Model", ObjAssemblyStatus.Model, _
                    '    "Serial No.", ObjAssemblyStatus.SerialNo, , , , , , , , , , , , , , , , LHLabel2, LHData2))
                Next
            Next
        End If

        If Not cmbAircraft.SelectedItem.ToString = "(All)" Then
            mtmpMachineList = tmpMachineList.GetMachineList(, Aircraft, , , , , True, AsonDate)
            For i As Integer = 0 To mtmpMachineList.Count - 1
                ReportStatusList.Add(New rptStatus(mtmpMachineList(i).ID.ToString, 1, , , , , , , , , , , , , , , , mtmpMachineList(i).Cycles, , , , , mtmpMachineList(i).RegNo, mtmpMachineList(i).ModelName, mtmpMachineList(i).Type, mtmpMachineList(i).SerialNo, mtmpMachineList(i).ManufacturerName, , mtmpMachineList(i).ManufacturingDate, mtmpMachineList(i).Hours, mtmpMachineList(i).Landings))
            Next
        End If
        If pnlAdvancedSearch.Visible = True Then

            If IsSerSelect = True Then
                For i As Integer = 0 To cmbServiceType.Items.Count - 1
                    If cmbServiceType.Items.Item(i).Selected Then
                        mMachineList = MachineList.GetMachineListDueMonitoringStatus(AsonDate, mDueLimits, MachineName, AssemblyName, AvgMnths, , , , True, False, False, ServiceTypeID(i))
                        For Each ObjMachine In mMachineList
                            For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                                For Each ObjAssemblyMonitorServiceStatus In ObjAssemblyStatus.AssemblyMonitorServiceStatusList
                                    If (ObjAssemblyMonitorServiceStatus.IsApplicable = True) And (Not (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True)) Then
                                        ATAChapter = ObjAssemblyMonitorServiceStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorServiceStatus.ATANomenclature
                                        Description = ObjAssemblyMonitorServiceStatus.Description
                                        AssemblyModel = ObjAssemblyStatus.Model
                                        AssemblySerialNo = ObjAssemblyStatus.SerialNo & vbCrLf & ObjAssemblyStatus.Position
                                        Position = ""
                                        MonitorTypeCode = ObjAssemblyMonitorServiceStatus.Code
                                        EstimatedDate = ObjAssemblyMonitorServiceStatus.EstimatedDateFormatted
                                        MinimumRemainingValue = ObjAssemblyMonitorServiceStatus.MinimumRemainingValue
                                        AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID

                                        StatusMasterID = ObjAssemblyMonitorServiceStatus.ModelMonitorServiceID  '11-Sep-2008
                                        DocumentTypeForID = 0

                                        '  Remark = ObjAssemblyMonitorServiceStatus.DoneRemark  'Added By Saylee on 20-08-2008
                                        Remark = ObjAssemblyStatus.InstallationRemark + " " + ObjAssemblyMonitorServiceStatus.DoneRemark
                                        Code = ObjAssemblyMonitorServiceStatus.ModelMonitorServiceCode  'Added By Saylee on 28-08-2008
                                        StatusID = ObjAssemblyMonitorServiceStatus.ID
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
                                                    AssemblyDueAsof = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
                                                    SinceNew = ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValue
                                                    DoneAt = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
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
												'If ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 3 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 4 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 5 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 6 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 7 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 8 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 9 Then
												'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
												If ObjAssemblyMonitorServiceStatusPeriod.PeriodID >= 3 Then
													If Freq3 = "" Then
														Freq3 = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue
														ElapsedTime2 = ObjAssemblyMonitorServiceStatusPeriod.ElapsedValue
														RemainingTime2 = ObjAssemblyMonitorServiceStatusPeriod.RemainingValue
														DueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
														AssemblyDueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
														SinceNew2 = ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValue
														DoneAt2 = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue

														'Added by Saylee 04-08-2008
														Extension2 = ObjAssemblyMonitorServiceStatusPeriod.ExtensionValue
													Else
														Freq3 = Freq3 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue
														ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.ElapsedValue
														RemainingTime2 = RemainingTime2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.RemainingValue
														DueAsof2 = DueAsof2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
														AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
														SinceNew2 = SinceNew2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValue
														DoneAt2 = DoneAt2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
														'Added by Saylee 04-08-2008
														Extension2 = Extension2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.ExtensionValue
													End If
												End If
											Else
                                                If ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 2 Then
                                                    If Freq3 = "" Then
                                                        Freq3 = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValueFormatted
                                                        ElapsedTime2 = ObjAssemblyMonitorServiceStatusPeriod.ElapsedValueFormatted
                                                        RemainingTime2 = ObjAssemblyMonitorServiceStatusPeriod.RemainingValueFormatted
                                                        DueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted
                                                        AssemblyDueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted 'Added By DEVEN On 14/06/2008
                                                        SinceNew2 = ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValueFormatted
                                                        DoneAt2 = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValueFormatted

                                                        'Added by Saylee 04-08-2008
                                                        Extension2 = ObjAssemblyMonitorServiceStatusPeriod.ExtensionValueFormatted
                                                    Else
                                                        Freq3 = Freq3 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.FrequencyValueFormatted
                                                        ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.ElapsedValueFormatted
                                                        RemainingTime2 = RemainingTime2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.RemainingValueFormatted
                                                        DueAsof2 = DueAsof2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted
                                                        AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.DueOnValueFormatted 'Added By DEVEN On 14/06/2008
                                                        SinceNew2 = SinceNew2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValueFormatted
                                                        DoneAt2 = DoneAt2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                        'Added by Saylee 04-08-2008
                                                        Extension2 = Extension2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.ExtensionValueFormatted
                                                    End If
                                                Else
                                                    If Freq3 = "" Then
                                                        Freq3 = ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue
                                                        ElapsedTime2 = ObjAssemblyMonitorServiceStatusPeriod.ElapsedValue
                                                        RemainingTime2 = ObjAssemblyMonitorServiceStatusPeriod.RemainingValue
                                                        DueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
                                                        AssemblyDueAsof2 = ObjAssemblyMonitorServiceStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
                                                        SinceNew2 = ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValue
                                                        DoneAt2 = ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
                                                        'Added by Saylee 04-08-2008
                                                        Extension2 = ObjAssemblyMonitorServiceStatusPeriod.ExtensionValue
                                                    Else
                                                        Freq3 = Freq3 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.FrequencyValue
                                                        ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.ElapsedValue
                                                        RemainingTime2 = RemainingTime2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.RemainingValue
                                                        DueAsof2 = DueAsof2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.DueOnValue
                                                        AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
                                                        SinceNew2 = SinceNew2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.AssemblyCurrentValue
                                                        DoneAt2 = DoneAt2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.DoneOnValue
                                                        'Added by Saylee 04-08-2008
                                                        Extension2 = Extension2 & vbCrLf & ObjAssemblyMonitorServiceStatusPeriod.ExtensionValue
                                                    End If
                                                End If

                                            End If
                                        Next
                                        AssemblyID = ObjAssemblyStatus.AssemblyID
                                        Note = ObjAssemblyMonitorServiceStatus.Notes
                                        RegNo = ObjMachine.RegNo

                                        RequiredManHours = ObjAssemblyMonitorServiceStatus.RequiredManHours
                                        Customer = ObjMachine.Customer

                                        AssemblyType = ObjAssemblyStatus.AssemblyType
                                        MaintenanceEvent = ObjAssemblyMonitorServiceStatus.Type

                                        ExtensionDate = ObjAssemblyMonitorServiceStatus.ExtensionDate
                                        ApprovalRemark = ObjAssemblyMonitorServiceStatus.ApprovalRemark
                                        tmpAssemblyStatusID = ObjAssemblyStatus.ID

                                        If ObjAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriodList.Count > 0 Then
                                            ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, , AssemblySerialNo, ATAChapter, , , Position, , MonitorTypeCode, Note, Remark, Description, _
                      , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, _
                      SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, AssemblyTypeID, MaintenanceEvent, , , , , , , , , , , , , , , , , , , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , , StatusID.ToString, StatusType.AssemblyService, , tmpAssemblyStatusID.ToString))
                                        End If
                                    End If
                                Next
                                For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
                                    For Each ObjCompMonitorServiceStatus In ObjCompStatus.CompMonitorServiceStatusList
                                        If (ObjCompMonitorServiceStatus.IsApplicable = True) And (Not (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True)) Then
                                            ATAChapter = ObjCompMonitorServiceStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorServiceStatus.ATANomenclature
                                            Description = ObjCompMonitorServiceStatus.Description
                                            PartNo = ObjCompStatus.PartName
                                            CompSerialNo = ObjCompStatus.CompSerialNo
                                            Position = ObjCompStatus.Position
                                            MonitorTypeCode = ObjCompMonitorServiceStatus.Code
                                            EstimatedDate = ObjCompMonitorServiceStatus.EstimatedDateFormatted
                                            AssemblyModel = ObjAssemblyStatus.Model
                                            AssemblySerialNo = ObjAssemblyStatus.SerialNo & vbCrLf & ObjAssemblyStatus.Position
                                            MinimumRemainingValue = ObjCompMonitorServiceStatus.MinimumRemainingValue
                                            AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID

                                            StatusMasterID = ObjCompMonitorServiceStatus.PartMonitorServiceID  '11-Sep-2008
                                            DocumentTypeForID = 0

                                            'Remark = ObjCompMonitorServiceStatus.DoneRemark  'Added By Saylee on 20-08-2008

                                            Remark = ObjCompStatus.InstallationRemark + " " + ObjCompMonitorServiceStatus.DoneRemark  'Added By Saylee on 20-08-2008
                                            Code = ObjCompMonitorServiceStatus.PartMonitorServiceCode
                                            CompStatusID = ObjCompStatus.ID
                                            StatusID = ObjCompMonitorServiceStatus.ID

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
                                                        AssemblyDueAsof = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
                                                        DueAsof = ObjCompMonitorServiceStatusPeriod.DueOnValue
                                                        SinceNew = ObjCompMonitorServiceStatusPeriod.CompCurrentValue
                                                        DoneAt = ObjCompMonitorServiceStatusPeriod.DoneOnValue
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
													'If ObjCompMonitorServiceStatusPeriod.PeriodID = 3 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 4 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 5 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 6 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 7 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 8 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Then
													'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
													If ObjCompMonitorServiceStatusPeriod.PeriodID >= 3 Then
														If Freq3 = "" Then
															Freq3 = ObjCompMonitorServiceStatusPeriod.FrequencyValue
															ElapsedTime2 = ObjCompMonitorServiceStatusPeriod.ElapsedValue
															RemainingTime2 = ObjCompMonitorServiceStatusPeriod.RemainingValue
															AssemblyDueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
															DueAsof2 = ObjCompMonitorServiceStatusPeriod.DueOnValue
															SinceNew2 = ObjCompMonitorServiceStatusPeriod.CompCurrentValue
															DoneAt2 = ObjCompMonitorServiceStatusPeriod.DoneOnValue
															'Added by Saylee 04-08-2008
															Extension2 = ObjCompMonitorServiceStatusPeriod.ExtensionValue
														Else
															Freq3 = Freq3 & vbCrLf & ObjCompMonitorServiceStatusPeriod.FrequencyValue
															ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.ElapsedValue
															RemainingTime2 = RemainingTime2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.RemainingValue
															AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
															DueAsof2 = DueAsof2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
															SinceNew2 = SinceNew2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.CompCurrentValue
															DoneAt2 = DoneAt2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.DoneOnValue
															'Added by Saylee 04-08-2008
															Extension2 = Extension2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.ExtensionValue
														End If
													End If
												Else
                                                    If ObjCompMonitorServiceStatusPeriod.PeriodID = 2 Then
                                                        If Freq3 = "" Then
                                                            Freq3 = ObjCompMonitorServiceStatusPeriod.FrequencyValueFormatted
                                                            ElapsedTime2 = ObjCompMonitorServiceStatusPeriod.ElapsedValueFormatted
                                                            RemainingTime2 = ObjCompMonitorServiceStatusPeriod.RemainingValueFormatted
                                                            AssemblyDueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted 'Added By DEVEN On 14/06/2008
                                                            DueAsof2 = ObjCompMonitorServiceStatusPeriod.DueOnValue
                                                            SinceNew2 = ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted
                                                            DoneAt2 = ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                            'Added by Saylee 04-08-2008
                                                            Extension2 = ObjCompMonitorServiceStatusPeriod.ExtensionValueFormatted
                                                        Else
                                                            Freq3 = Freq3 & vbCrLf & ObjCompMonitorServiceStatusPeriod.FrequencyValueFormatted
                                                            ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.ElapsedValueFormatted
                                                            RemainingTime2 = RemainingTime2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.RemainingValueFormatted
                                                            AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueTextFormatted 'Added By DEVEN On 14/06/2008
                                                            DueAsof2 = DueAsof2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                            SinceNew2 = SinceNew2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.CompCurrentValueFormatted
                                                            DoneAt2 = DoneAt2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                                            'Added by Saylee 04-08-2008
                                                            Extension2 = Extension2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.ExtensionValueFormatted
                                                        End If
                                                    Else
                                                        If Freq3 = "" Then
                                                            Freq3 = ObjCompMonitorServiceStatusPeriod.FrequencyValue
                                                            ElapsedTime2 = ObjCompMonitorServiceStatusPeriod.ElapsedValue
                                                            RemainingTime2 = ObjCompMonitorServiceStatusPeriod.RemainingValue
                                                            AssemblyDueAsof2 = ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
                                                            DueAsof2 = ObjCompMonitorServiceStatusPeriod.DueOnValue
                                                            SinceNew2 = ObjCompMonitorServiceStatusPeriod.CompCurrentValue
                                                            DoneAt2 = ObjCompMonitorServiceStatusPeriod.DoneOnValue
                                                            'Added by Saylee 04-08-2008
                                                            Extension2 = ObjCompMonitorServiceStatusPeriod.ExtensionValue
                                                        Else
                                                            Freq3 = Freq3 & vbCrLf & ObjCompMonitorServiceStatusPeriod.FrequencyValue
                                                            ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.ElapsedValue
                                                            RemainingTime2 = RemainingTime2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.RemainingValue
                                                            AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
                                                            DueAsof2 = DueAsof2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                                            SinceNew2 = SinceNew2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.CompCurrentValue
                                                            DoneAt2 = DoneAt2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.DoneOnValue
                                                            'Added by Saylee 04-08-2008
                                                            Extension2 = Extension2 & vbCrLf & ObjCompMonitorServiceStatusPeriod.ExtensionValue
                                                        End If
                                                    End If

                                                End If
                                            Next
                                            AssemblyID = ObjAssemblyStatus.AssemblyID
                                            AssemblyType = ObjAssemblyStatus.AssemblyType
                                            RegNo = ObjMachine.RegNo
                                            'Rajnish 08-08-2008
                                            RequiredManHours = ObjCompMonitorServiceStatus.RequiredManHours
                                            Customer = ObjMachine.Customer
                                            Note = ObjCompMonitorServiceStatus.Notes
                                            MaintenanceEvent = ObjCompMonitorServiceStatus.Type

                                            'Added by Saylee 04-08-2008
                                            ExtensionDate = ObjCompMonitorServiceStatus.ExtensionDate
                                            ApprovalRemark = ObjCompMonitorServiceStatus.ApprovalRemark
                                            tmpAssemblyStatusID = ObjAssemblyStatus.ID

                                            If ObjCompMonitorServiceStatus.CompMonitorServiceStatusPeriodList.Count > 0 Then
                                                ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, MaintenanceEvent, AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, , MonitorTypeCode, Note, Remark, Description, _
                                                                      , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, _
                                                                      AssemblyTypeID, MaintenanceEvent, , , , , , , , , , , , , , , , , , , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , , StatusID.ToString, StatusType.ComponentService, CompStatusID.ToString, tmpAssemblyStatusID.ToString))
                                            End If
                                        End If
                                    Next
                                Next
                            Next
                        Next
                    End If
                Next
            End If

            If IsInsSelect = True Then
                For i As Integer = 0 To cmbInspectionType.Items.Count - 1
                    If cmbInspectionType.Items.Item(i).Selected Then
                        mMachineList = MachineList.GetMachineListDueMonitoringStatus(AsonDate, mDueLimits, MachineName, AssemblyName, AvgMnths, , , , False, True, False, , InspectionTypeID(i))
                        For Each ObjMachine In mMachineList
                            For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                                For Each ObjAssemblyMonitorInspStatus In ObjAssemblyStatus.AssemblyMonitorInspStatusList
                                    If (ObjAssemblyMonitorInspStatus.IsApplicable = True) And (Not (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True)) Then
                                        ATAChapter = ObjAssemblyMonitorInspStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorInspStatus.ATANomenclature
                                        Description = ObjAssemblyMonitorInspStatus.Description
                                        AssemblyModel = ObjAssemblyStatus.Model
                                        AssemblySerialNo = ObjAssemblyStatus.SerialNo & vbCrLf & ObjAssemblyStatus.Position
                                        Position = ""
                                        MonitorTypeCode = ObjAssemblyMonitorInspStatus.Code
                                        EstimatedDate = ObjAssemblyMonitorInspStatus.EstimatedDateFormatted
                                        MinimumRemainingValue = ObjAssemblyMonitorInspStatus.MinimumRemainingValue
                                        AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID

                                        StatusMasterID = ObjAssemblyMonitorInspStatus.ModelMonitorInspID  '11-Sep-2008
                                        DocumentTypeForID = 9

                                        Code = ObjAssemblyMonitorInspStatus.ModelMonitorInspCode
                                        'Remark = ObjAssemblyMonitorInspStatus.DoneRemark  'Added By Saylee on 20-08-2008
                                        Remark = ObjAssemblyStatus.InstallationRemark + " " + ObjAssemblyMonitorInspStatus.DoneRemark  'Added By Saylee on 20-08-2008
                                        StatusID = ObjAssemblyMonitorInspStatus.ID
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
                                                    AssemblyDueAsof = ObjAssemblyMonitorInspStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
                                                    SinceNew = ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue
                                                    DoneAt = ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
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
												'If ObjAssemblyMonitorInspStatusPeriod.PeriodID = 3 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 4 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 5 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 6 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 7 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 8 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 9 Then
												'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
												If ObjAssemblyMonitorInspStatusPeriod.PeriodID >= 3 Then
													If Freq3 = "" Then
														Freq3 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValue
														ElapsedTime2 = ObjAssemblyMonitorInspStatusPeriod.ElapsedValue
														RemainingTime2 = ObjAssemblyMonitorInspStatusPeriod.RemainingValue
														DueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValue
														AssemblyDueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
														SinceNew2 = ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue
														DoneAt2 = ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
														'Added by Saylee 04-08-2008
														Extension2 = ObjAssemblyMonitorInspStatusPeriod.ExtensionValue
													Else
														Freq3 = Freq3 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.FrequencyValue
														ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.ElapsedValue
														RemainingTime2 = RemainingTime2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.RemainingValue
														DueAsof2 = DueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DueOnValue
														AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
														SinceNew2 = SinceNew2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue
														DoneAt2 = DoneAt2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
														'Added by Saylee 04-08-2008
														Extension2 = Extension2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.ExtensionValue
													End If
												End If
											Else
                                                If ObjAssemblyMonitorInspStatusPeriod.PeriodID = 2 Then
                                                    If Freq3 = "" Then
                                                        Freq3 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValueFormatted
                                                        ElapsedTime2 = ObjAssemblyMonitorInspStatusPeriod.ElapsedValueFormatted
                                                        RemainingTime2 = ObjAssemblyMonitorInspStatusPeriod.RemainingValueFormatted
                                                        DueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
                                                        AssemblyDueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted 'Added By DEVEN On 14/06/2008
                                                        SinceNew2 = ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValueFormatted
                                                        DoneAt2 = ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted
                                                        'Added by Saylee 04-08-2008
                                                        Extension2 = ObjAssemblyMonitorInspStatusPeriod.ExtensionValueFormatted
                                                    Else
                                                        Freq3 = Freq3 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.FrequencyValueFormatted
                                                        ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.ElapsedValueFormatted
                                                        RemainingTime2 = RemainingTime2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.RemainingValueFormatted
                                                        DueAsof2 = DueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
                                                        AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted 'Added By DEVEN On 14/06/2008
                                                        SinceNew2 = SinceNew2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValueFormatted
                                                        DoneAt2 = DoneAt2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted
                                                        'Added by Saylee 04-08-2008
                                                        Extension2 = Extension2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.ExtensionValueFormatted
                                                    End If
                                                Else
                                                    If Freq3 = "" Then
                                                        Freq3 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValue
                                                        ElapsedTime2 = ObjAssemblyMonitorInspStatusPeriod.ElapsedValue
                                                        RemainingTime2 = ObjAssemblyMonitorInspStatusPeriod.RemainingValue
                                                        DueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValue
                                                        AssemblyDueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
                                                        SinceNew2 = ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue
                                                        DoneAt2 = ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
                                                        'Added by Saylee 04-08-2008
                                                        Extension2 = ObjAssemblyMonitorInspStatusPeriod.ExtensionValue
                                                    Else
                                                        Freq3 = Freq3 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.FrequencyValue
                                                        ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.ElapsedValue
                                                        RemainingTime2 = RemainingTime2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.RemainingValue
                                                        DueAsof2 = DueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DueOnValue
                                                        AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
                                                        SinceNew2 = SinceNew2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.AssemblyCurrentValue
                                                        DoneAt2 = DoneAt2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
                                                        'Added by Saylee 04-08-2008
                                                        Extension2 = Extension2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.ExtensionValue
                                                    End If
                                                End If

                                            End If
                                        Next
                                        AssemblyID = ObjAssemblyStatus.AssemblyID
                                        AssemblyType = ObjAssemblyStatus.AssemblyType
                                        RegNo = ObjMachine.RegNo
                                        'Rajnish 08-08-2008
                                        RequiredManHours = ObjAssemblyMonitorInspStatus.RequiredManHours
                                        Customer = ObjMachine.Customer
                                        Note = ObjAssemblyMonitorInspStatus.Notes
                                        MaintenanceEvent = ObjAssemblyMonitorInspStatus.Type

                                        'Added by Saylee 04-08-2008
                                        ExtensionDate = ObjAssemblyMonitorInspStatus.ExtensionDate
                                        ApprovalRemark = ObjAssemblyMonitorInspStatus.ApprovalRemark
                                        tmpAssemblyStatusID = ObjAssemblyStatus.ID

                                        If ObjAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriodList.Count > 0 Then
                                            ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, , AssemblySerialNo, ATAChapter, , , Position, , MonitorTypeCode, Note, Remark, Description, _
                                               , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, _
                                               SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, _
                                               AssemblyTypeID, MaintenanceEvent, , , , , , , , , , , , , , , , , , , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , , StatusID.ToString, StatusType.AssemblyInspection, , tmpAssemblyStatusID.ToString))
                                        End If
                                    End If
                                Next
                                For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
                                    For Each ObjCompMonitorInspStatus In ObjCompStatus.CompMonitorInspStatusList
                                        If (ObjCompMonitorInspStatus.IsApplicable = True) And (Not (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True)) Then
                                            ATAChapter = ObjCompMonitorInspStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorInspStatus.ATANomenclature
                                            Description = ObjCompMonitorInspStatus.Description
                                            PartNo = ObjCompStatus.PartName
                                            CompSerialNo = ObjCompStatus.CompSerialNo
                                            Position = ObjCompStatus.Position
                                            MonitorTypeCode = ObjCompMonitorInspStatus.Code
                                            EstimatedDate = ObjCompMonitorInspStatus.EstimatedDateFormatted
                                            AssemblyModel = ObjAssemblyStatus.Model
                                            AssemblySerialNo = ObjAssemblyStatus.SerialNo & vbCrLf & ObjAssemblyStatus.Position
                                            MinimumRemainingValue = ObjCompMonitorInspStatus.MinimumRemainingValue
                                            AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID

                                            StatusMasterID = ObjCompMonitorInspStatus.PartMonitorInspID  '11-Sep-2008
                                            DocumentTypeForID = 11

                                            'Remark = ObjCompMonitorInspStatus.DoneRemark  'Added By Saylee on 20-08-2008
                                            Remark = ObjCompStatus.InstallationRemark + " " + ObjCompMonitorInspStatus.DoneRemark  'Added By Saylee on 20-08-2008
                                            Code = ObjCompMonitorInspStatus.PartMonitorInspCode
                                            CompStatusID = ObjCompStatus.ID
                                            StatusID = ObjCompMonitorInspStatus.ID

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
                                                        AssemblyDueAsof = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
                                                        DueAsof = ObjCompMonitorInspStatusPeriod.DueOnValue
                                                        SinceNew = ObjCompMonitorInspStatusPeriod.CompCurrentValue
                                                        DoneAt = ObjCompMonitorInspStatusPeriod.DoneOnValue
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
													'If ObjCompMonitorInspStatusPeriod.PeriodID = 3 Or ObjCompMonitorInspStatusPeriod.PeriodID = 4 Or ObjCompMonitorInspStatusPeriod.PeriodID = 5 Or ObjCompMonitorInspStatusPeriod.PeriodID = 6 Or ObjCompMonitorInspStatusPeriod.PeriodID = 7 Or ObjCompMonitorInspStatusPeriod.PeriodID = 8 Or ObjCompMonitorInspStatusPeriod.PeriodID = 9 Then
													'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
													If ObjCompMonitorInspStatusPeriod.PeriodID >= 3 Then
														If Freq3 = "" Then
															Freq3 = ObjCompMonitorInspStatusPeriod.FrequencyValue
															ElapsedTime2 = ObjCompMonitorInspStatusPeriod.ElapsedValue
															RemainingTime2 = ObjCompMonitorInspStatusPeriod.RemainingValue
															AssemblyDueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
															DueAsof2 = ObjCompMonitorInspStatusPeriod.DueOnValue
															SinceNew2 = ObjCompMonitorInspStatusPeriod.CompCurrentValue
															DoneAt2 = ObjCompMonitorInspStatusPeriod.DoneOnValue
															'Added By Saylee on 04-08-2008
															Extension2 = ObjCompMonitorInspStatusPeriod.ExtensionValue
														Else
															Freq3 = Freq3 & vbCrLf & ObjCompMonitorInspStatusPeriod.FrequencyValue
															ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjCompMonitorInspStatusPeriod.ElapsedValue
															RemainingTime2 = RemainingTime2 & vbCrLf & ObjCompMonitorInspStatusPeriod.RemainingValue
															AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
															DueAsof2 = DueAsof2 & vbCrLf & ObjCompMonitorInspStatusPeriod.DueOnValue
															SinceNew2 = SinceNew2 & vbCrLf & ObjCompMonitorInspStatusPeriod.CompCurrentValue
															DoneAt2 = DoneAt2 & vbCrLf & ObjCompMonitorInspStatusPeriod.DoneOnValue
															'Added By Saylee on 04-08-2008
															Extension2 = Extension2 & vbCrLf & ObjCompMonitorInspStatusPeriod.ExtensionValue
														End If
													End If
												Else
                                                    If ObjCompMonitorInspStatusPeriod.PeriodID = 2 Then
                                                        If Freq3 = "" Then
                                                            Freq3 = ObjCompMonitorInspStatusPeriod.FrequencyValueFormatted
                                                            ElapsedTime2 = ObjCompMonitorInspStatusPeriod.ElapsedValueFormatted
                                                            RemainingTime2 = ObjCompMonitorInspStatusPeriod.RemainingValueFormatted
                                                            AssemblyDueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted 'Added By DEVEN On 14/06/2008
                                                            DueAsof2 = ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
                                                            SinceNew2 = ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted
                                                            DoneAt2 = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                            'Added By Saylee on 04-08-2008
                                                            Extension2 = ObjCompMonitorInspStatusPeriod.ExtensionValueFormatted
                                                        Else
                                                            Freq3 = Freq3 & vbCrLf & ObjCompMonitorInspStatusPeriod.FrequencyValueFormatted
                                                            ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjCompMonitorInspStatusPeriod.ElapsedValueFormatted
                                                            RemainingTime2 = RemainingTime2 & vbCrLf & ObjCompMonitorInspStatusPeriod.RemainingValueFormatted
                                                            AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted 'Added By DEVEN On 14/06/2008
                                                            DueAsof2 = DueAsof2 & vbCrLf & ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
                                                            SinceNew2 = SinceNew2 & vbCrLf & ObjCompMonitorInspStatusPeriod.CompCurrentValueFormatted
                                                            DoneAt2 = DoneAt2 & vbCrLf & ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                                            'Added By Saylee on 04-08-2008
                                                            Extension2 = Extension2 & vbCrLf & ObjCompMonitorInspStatusPeriod.ExtensionValueFormatted
                                                        End If
                                                    Else
                                                        If Freq3 = "" Then
                                                            Freq3 = ObjCompMonitorInspStatusPeriod.FrequencyValue
                                                            ElapsedTime2 = ObjCompMonitorInspStatusPeriod.ElapsedValue
                                                            RemainingTime2 = ObjCompMonitorInspStatusPeriod.RemainingValue
                                                            AssemblyDueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
                                                            DueAsof2 = ObjCompMonitorInspStatusPeriod.DueOnValue
                                                            SinceNew2 = ObjCompMonitorInspStatusPeriod.CompCurrentValue
                                                            DoneAt2 = ObjCompMonitorInspStatusPeriod.DoneOnValue
                                                            'Added By Saylee on 04-08-2008
                                                            Extension2 = ObjCompMonitorInspStatusPeriod.ExtensionValue
                                                        Else
                                                            Freq3 = Freq3 & vbCrLf & ObjCompMonitorInspStatusPeriod.FrequencyValue
                                                            ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjCompMonitorInspStatusPeriod.ElapsedValue
                                                            RemainingTime2 = RemainingTime2 & vbCrLf & ObjCompMonitorInspStatusPeriod.RemainingValue
                                                            AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
                                                            DueAsof2 = DueAsof2 & vbCrLf & ObjCompMonitorInspStatusPeriod.DueOnValue
                                                            SinceNew2 = SinceNew2 & vbCrLf & ObjCompMonitorInspStatusPeriod.CompCurrentValue
                                                            DoneAt2 = DoneAt2 & vbCrLf & ObjCompMonitorInspStatusPeriod.DoneOnValue
                                                            'Added By Saylee on 04-08-2008
                                                            Extension2 = Extension2 & vbCrLf & ObjCompMonitorInspStatusPeriod.ExtensionValue
                                                        End If
                                                    End If

                                                End If
                                            Next
                                            AssemblyID = ObjAssemblyStatus.AssemblyID
                                            AssemblyType = ObjAssemblyStatus.AssemblyType
                                            RegNo = ObjMachine.RegNo
                                            'Rajnish 08-08-2008
                                            RequiredManHours = ObjCompMonitorInspStatus.RequiredManHours
                                            Customer = ObjMachine.Customer

                                            Note = ObjCompMonitorInspStatus.Notes
                                            MaintenanceEvent = ObjCompMonitorInspStatus.Type

                                            'Added By Saylee on 04-08-2008
                                            ExtensionDate = ObjCompMonitorInspStatus.ExtensionDate
                                            ApprovalRemark = ObjCompMonitorInspStatus.ApprovalRemark
                                            tmpAssemblyStatusID = ObjAssemblyStatus.ID

                                            If ObjCompMonitorInspStatus.CompMonitorInspStatusPeriodList.Count > 0 Then
                                                ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, MaintenanceEvent, AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, , MonitorTypeCode, Note, Remark, Description, _
                                                                     , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, _
                                                                     AssemblyTypeID, MaintenanceEvent, , , , , , , , , , , , , , , , , , , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , , StatusID.ToString, StatusType.ComponentInspection, CompStatusID.ToString, tmpAssemblyStatusID.ToString))
                                            End If
                                        End If
                                    Next
                                Next
                            Next
                        Next
                    End If
                Next
            End If

            If IsModSelect = True Then
                For i As Integer = 0 To cmbModificationType.Items.Count - 1
                    If cmbModificationType.Items.Item(i).Selected Then
                        mMachineList = MachineList.GetMachineListDueMonitoringStatus(AsonDate, mDueLimits, MachineName, AssemblyName, AvgMnths, , , , False, False, True, , , ModificationTypeID(i))
                        For Each ObjMachine In mMachineList
                            For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                                For Each ObjAssemblyMonitorModStatus In ObjAssemblyStatus.AssemblyMonitorModStatusList
                                    If (ObjAssemblyMonitorModStatus.IsApplicable = True) And (Not (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True)) Then
                                        ATAChapter = ObjAssemblyMonitorModStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorModStatus.ATANomenclature
                                        Description = ObjAssemblyMonitorModStatus.Description & vbCrLf & ObjAssemblyMonitorModStatus.Number & vbCrLf & ObjAssemblyMonitorModStatus.Reference
                                        AssemblyModel = ObjAssemblyStatus.Model
                                        AssemblySerialNo = ObjAssemblyStatus.SerialNo & vbCrLf & ObjAssemblyStatus.Position
                                        Position = ""
                                        MonitorTypeCode = ObjAssemblyMonitorModStatus.Code
                                        EstimatedDate = ObjAssemblyMonitorModStatus.EstimatedDateFormatted
                                        MinimumRemainingValue = ObjAssemblyMonitorModStatus.MinimumRemainingValue
                                        AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID

                                        StatusMasterID = ObjAssemblyMonitorModStatus.ModelMonitorModID  '11-Sep-2008                        
                                        DocumentTypeForID = 8

                                        'Remark = ObjAssemblyMonitorModStatus.DoneRemark  'Added By Saylee on 20-08-2008
                                        Remark = ObjAssemblyStatus.InstallationRemark + " " + ObjAssemblyMonitorModStatus.DoneRemark 'Added By Saylee on 20-08-2008
                                        Code = ObjAssemblyMonitorModStatus.ModelMonitorModCode
                                        StatusID = ObjAssemblyMonitorModStatus.ID

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
                                                    AssemblyDueAsof = ObjAssemblyMonitorModStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
                                                    SinceNew = ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValue
                                                    DoneAt = ObjAssemblyMonitorModStatusPeriod.DoneOnValue
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
												'If ObjAssemblyMonitorModStatusPeriod.PeriodID = 3 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 4 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 5 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 6 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 7 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 8 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 9 Then
												'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
												If ObjAssemblyMonitorModStatusPeriod.PeriodID >= 3 Then
													If Freq3 = "" Then
														Freq3 = ObjAssemblyMonitorModStatusPeriod.FrequencyValue
														ElapsedTime2 = ObjAssemblyMonitorModStatusPeriod.ElapsedValue
														RemainingTime2 = ObjAssemblyMonitorModStatusPeriod.RemainingValue
														DueAsof2 = ObjAssemblyMonitorModStatusPeriod.DueOnValue
														AssemblyDueAsof2 = ObjAssemblyMonitorModStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
														SinceNew2 = ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValue
														DoneAt2 = ObjAssemblyMonitorModStatusPeriod.DoneOnValue
														'Added By Saylee on 04-08-2008
														Extension2 = ObjAssemblyMonitorModStatusPeriod.ExtensionValue
													Else
														Freq3 = Freq3 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.FrequencyValue
														ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.ElapsedValue
														RemainingTime2 = RemainingTime2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.RemainingValue
														DueAsof2 = DueAsof2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.DueOnValue
														AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
														SinceNew2 = SinceNew2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValue
														DoneAt2 = DoneAt2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.DoneOnValue
														'Added By Saylee on 04-08-2008
														Extension2 = Extension2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.ExtensionValue
													End If
												End If
											Else
                                                If ObjAssemblyMonitorModStatusPeriod.PeriodID = 2 Then
                                                    If Freq3 = "" Then
                                                        Freq3 = ObjAssemblyMonitorModStatusPeriod.FrequencyValueFormatted
                                                        ElapsedTime2 = ObjAssemblyMonitorModStatusPeriod.ElapsedValueFormatted
                                                        RemainingTime2 = ObjAssemblyMonitorModStatusPeriod.RemainingValueFormatted
                                                        DueAsof2 = ObjAssemblyMonitorModStatusPeriod.DueOnValueFormatted
                                                        AssemblyDueAsof2 = ObjAssemblyMonitorModStatusPeriod.DueOnValueFormatted 'Added By DEVEN On 14/06/2008
                                                        SinceNew2 = ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValueFormatted
                                                        DoneAt2 = ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted
                                                        'Added By Saylee on 04-08-2008
                                                        Extension2 = ObjAssemblyMonitorModStatusPeriod.ExtensionValueFormatted
                                                    Else
                                                        Freq3 = Freq3 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.FrequencyValueFormatted
                                                        ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.ElapsedValueFormatted
                                                        RemainingTime2 = RemainingTime2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.RemainingValueFormatted
                                                        DueAsof2 = DueAsof2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.DueOnValueFormatted
                                                        AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.DueOnValueFormatted 'Added By DEVEN On 14/06/2008
                                                        SinceNew2 = SinceNew2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValueFormatted
                                                        DoneAt2 = DoneAt2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.DoneOnValueFormatted
                                                        'Added By Saylee on 04-08-2008
                                                        Extension2 = Extension2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.ExtensionValueFormatted
                                                    End If
                                                Else
                                                    If Freq3 = "" Then
                                                        Freq3 = ObjAssemblyMonitorModStatusPeriod.FrequencyValue
                                                        ElapsedTime2 = ObjAssemblyMonitorModStatusPeriod.ElapsedValue
                                                        RemainingTime2 = ObjAssemblyMonitorModStatusPeriod.RemainingValue
                                                        DueAsof2 = ObjAssemblyMonitorModStatusPeriod.DueOnValue
                                                        AssemblyDueAsof2 = ObjAssemblyMonitorModStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
                                                        SinceNew2 = ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValue
                                                        DoneAt2 = ObjAssemblyMonitorModStatusPeriod.DoneOnValue
                                                        'Added By Saylee on 04-08-2008
                                                        Extension2 = ObjAssemblyMonitorModStatusPeriod.ExtensionValue
                                                    Else
                                                        Freq3 = Freq3 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.FrequencyValue
                                                        ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.ElapsedValue
                                                        RemainingTime2 = RemainingTime2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.RemainingValue
                                                        DueAsof2 = DueAsof2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.DueOnValue
                                                        AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.DueOnValue 'Added By DEVEN On 14/06/2008
                                                        SinceNew2 = SinceNew2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.AssemblyCurrentValue
                                                        DoneAt2 = DoneAt2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.DoneOnValue
                                                        'Added By Saylee on 04-08-2008
                                                        Extension2 = Extension2 & vbCrLf & ObjAssemblyMonitorModStatusPeriod.ExtensionValue
                                                    End If
                                                End If

                                            End If
                                        Next
                                        AssemblyID = ObjAssemblyStatus.AssemblyID
                                        AssemblyType = ObjAssemblyStatus.AssemblyType
                                        RegNo = ObjMachine.RegNo
                                        'Rajnish 08-08-2008
                                        RequiredManHours = ObjAssemblyMonitorModStatus.RequiredManHours
                                        Customer = ObjMachine.Customer

                                        Note = ObjAssemblyMonitorModStatus.Notes
                                        MaintenanceEvent = ObjAssemblyMonitorModStatus.Type

                                        'Added By Saylee on 04-08-2008
                                        ExtensionDate = ObjAssemblyMonitorModStatus.ExtensionDate
                                        ApprovalRemark = ObjAssemblyMonitorModStatus.ApprovalRemark
                                        tmpAssemblyStatusID = ObjAssemblyStatus.ID

                                        If ObjAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriodList.Count > 0 Then
                                            ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, , AssemblySerialNo, ATAChapter, , , Position, , MonitorTypeCode, Note, Remark, Description, _
                                               , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, _
                                               SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, AssemblyTypeID, MaintenanceEvent, , , , , , , , , , , , , , , , , , , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , , StatusID.ToString, StatusType.AssemblyDirective, , tmpAssemblyStatusID.ToString))
                                        End If
                                    End If
                                Next
                                For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
                                    For Each ObjCompMonitorModStatus In ObjCompStatus.CompMonitorModStatusList
                                        If (ObjCompMonitorModStatus.IsApplicable = True) And (Not (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted)) Then
                                            ATAChapter = ObjCompMonitorModStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorModStatus.ATANomenclature
                                            Description = ObjCompMonitorModStatus.Description & vbCrLf & ObjCompMonitorModStatus.Number & vbCrLf & ObjCompMonitorModStatus.Reference
                                            PartNo = ObjCompStatus.PartName
                                            CompSerialNo = ObjCompStatus.CompSerialNo
                                            Position = ObjCompStatus.Position
                                            MonitorTypeCode = ObjCompMonitorModStatus.Code
                                            EstimatedDate = ObjCompMonitorModStatus.EstimatedDateFormatted
                                            AssemblyModel = ObjAssemblyStatus.Model
                                            AssemblySerialNo = ObjAssemblyStatus.SerialNo & vbCrLf & ObjAssemblyStatus.Position
                                            MinimumRemainingValue = ObjCompMonitorModStatus.MinimumRemainingValue
                                            AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID

                                            StatusMasterID = ObjCompMonitorModStatus.PartMonitorModID  '11-Sep-2008                        
                                            DocumentTypeForID = 10

                                            'Remark = ObjCompMonitorModStatus.DoneRemark  'Added By Saylee on 20-08-2008
                                            Remark = ObjCompStatus.InstallationRemark + " " + ObjCompMonitorModStatus.DoneRemark    'Added By Saylee on 20-08-2008
                                            Code = ObjCompMonitorModStatus.PartMonitorModCode

                                            CompStatusID = ObjCompStatus.ID
                                            StatusID = ObjCompMonitorModStatus.ID

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
                                                        AssemblyDueAsof = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
                                                        DueAsof = ObjCompMonitorModStatusPeriod.DueOnValue
                                                        SinceNew = ObjCompMonitorModStatusPeriod.CompCurrentValue
                                                        DoneAt = ObjCompMonitorModStatusPeriod.DoneOnValue
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
													'If ObjCompMonitorModStatusPeriod.PeriodID = 3 Or ObjCompMonitorModStatusPeriod.PeriodID = 4 Or ObjCompMonitorModStatusPeriod.PeriodID = 5 Or ObjCompMonitorModStatusPeriod.PeriodID = 6 Or ObjCompMonitorModStatusPeriod.PeriodID = 7 Or ObjCompMonitorModStatusPeriod.PeriodID = 8 Or ObjCompMonitorModStatusPeriod.PeriodID = 9 Then
													'Above line Commented by on 22-Aug-2025 as for new periods to reflct on reports
													If ObjCompMonitorModStatusPeriod.PeriodID >= 3 Then
														If Freq3 = "" Then
															Freq3 = ObjCompMonitorModStatusPeriod.FrequencyValue
															ElapsedTime2 = ObjCompMonitorModStatusPeriod.ElapsedValue
															RemainingTime2 = ObjCompMonitorModStatusPeriod.RemainingValue
															AssemblyDueAsof2 = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
															DueAsof2 = ObjCompMonitorModStatusPeriod.DueOnValue
															SinceNew2 = ObjCompMonitorModStatusPeriod.CompCurrentValue
															DoneAt2 = ObjCompMonitorModStatusPeriod.DoneOnValue
															'Added By Saylee on 04-08-2008
															Extension2 = ObjCompMonitorModStatusPeriod.ExtensionValue
														Else
															Freq3 = Freq3 & vbCrLf & ObjCompMonitorModStatusPeriod.FrequencyValue
															ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjCompMonitorModStatusPeriod.ElapsedValue
															RemainingTime2 = RemainingTime2 & vbCrLf & ObjCompMonitorModStatusPeriod.RemainingValue
															AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorModStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
															DueAsof2 = DueAsof2 & vbCrLf & ObjCompMonitorModStatusPeriod.DueOnValue
															SinceNew2 = SinceNew2 & vbCrLf & ObjCompMonitorModStatusPeriod.CompCurrentValue
															DoneAt2 = DoneAt2 & vbCrLf & ObjCompMonitorModStatusPeriod.DoneOnValue
															'Added By Saylee on 04-08-2008
															Extension2 = Extension2 & vbCrLf & ObjCompMonitorModStatusPeriod.ExtensionValue
														End If
													End If
												Else
                                                    If ObjCompMonitorModStatusPeriod.PeriodID = 2 Then
                                                        If Freq3 = "" Then
                                                            Freq3 = ObjCompMonitorModStatusPeriod.FrequencyValueFormatted
                                                            ElapsedTime2 = ObjCompMonitorModStatusPeriod.ElapsedValueFormatted
                                                            RemainingTime2 = ObjCompMonitorModStatusPeriod.RemainingValueFormatted
                                                            AssemblyDueAsof2 = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormatted 'Added By DEVEN On 14/06/2008
                                                            DueAsof2 = ObjCompMonitorModStatusPeriod.DueOnValueFormatted
                                                            SinceNew2 = ObjCompMonitorModStatusPeriod.CompCurrentValueFormatted
                                                            DoneAt2 = ObjCompMonitorModStatusPeriod.DoneOnValueFormatted
                                                            'Added By Saylee on 04-08-2008
                                                            Extension2 = ObjCompMonitorModStatusPeriod.ExtensionValueFormatted
                                                        Else
                                                            Freq3 = Freq3 & vbCrLf & ObjCompMonitorModStatusPeriod.FrequencyValueFormatted
                                                            ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjCompMonitorModStatusPeriod.ElapsedValueFormatted
                                                            RemainingTime2 = RemainingTime2 & vbCrLf & ObjCompMonitorModStatusPeriod.RemainingValueFormatted
                                                            AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorModStatusPeriod.AssemblyDueOnValueTextFormatted 'Added By DEVEN On 14/06/2008
                                                            DueAsof2 = DueAsof2 & vbCrLf & ObjCompMonitorModStatusPeriod.DueOnValueFormatted
                                                            SinceNew2 = SinceNew2 & vbCrLf & ObjCompMonitorModStatusPeriod.CompCurrentValueFormatted
                                                            DoneAt2 = DoneAt2 & vbCrLf & ObjCompMonitorModStatusPeriod.DoneOnValueFormatted
                                                            'Added By Saylee on 04-08-2008
                                                            Extension2 = Extension2 & vbCrLf & ObjCompMonitorModStatusPeriod.ExtensionValueFormatted
                                                        End If
                                                    Else
                                                        If Freq3 = "" Then
                                                            Freq3 = ObjCompMonitorModStatusPeriod.FrequencyValue
                                                            ElapsedTime2 = ObjCompMonitorModStatusPeriod.ElapsedValue
                                                            RemainingTime2 = ObjCompMonitorModStatusPeriod.RemainingValue
                                                            AssemblyDueAsof2 = ObjCompMonitorModStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
                                                            DueAsof2 = ObjCompMonitorModStatusPeriod.DueOnValue
                                                            SinceNew2 = ObjCompMonitorModStatusPeriod.CompCurrentValue
                                                            DoneAt2 = ObjCompMonitorModStatusPeriod.DoneOnValue
                                                            'Added By Saylee on 04-08-2008
                                                            Extension2 = ObjCompMonitorModStatusPeriod.ExtensionValue
                                                        Else
                                                            Freq3 = Freq3 & vbCrLf & ObjCompMonitorModStatusPeriod.FrequencyValue
                                                            ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjCompMonitorModStatusPeriod.ElapsedValue
                                                            RemainingTime2 = RemainingTime2 & vbCrLf & ObjCompMonitorModStatusPeriod.RemainingValue
                                                            AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorModStatusPeriod.AssemblyDueOnValueText 'Added By DEVEN On 14/06/2008
                                                            DueAsof2 = DueAsof2 & vbCrLf & ObjCompMonitorModStatusPeriod.DueOnValue
                                                            SinceNew2 = SinceNew2 & vbCrLf & ObjCompMonitorModStatusPeriod.CompCurrentValue
                                                            DoneAt2 = DoneAt2 & vbCrLf & ObjCompMonitorModStatusPeriod.DoneOnValue
                                                            'Added By Saylee on 04-08-2008
                                                            Extension2 = Extension2 & vbCrLf & ObjCompMonitorModStatusPeriod.ExtensionValue
                                                        End If
                                                    End If

                                                End If
                                            Next
                                            AssemblyID = ObjAssemblyStatus.AssemblyID
                                            AssemblyType = ObjAssemblyStatus.AssemblyType
                                            RegNo = ObjMachine.RegNo

                                            RequiredManHours = ObjCompMonitorModStatus.RequiredManHours
                                            Customer = ObjMachine.Customer

                                            Note = ObjCompMonitorModStatus.Notes
                                            MaintenanceEvent = ObjCompMonitorModStatus.Type

                                            ExtensionDate = ObjCompMonitorModStatus.ExtensionDate
                                            ApprovalRemark = ObjCompMonitorModStatus.ApprovalRemark
                                            tmpAssemblyStatusID = ObjAssemblyStatus.ID

                                            If ObjCompMonitorModStatus.CompMonitorModStatusPeriodList.Count > 0 Then
                                                ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, MaintenanceEvent, AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, , MonitorTypeCode, Note, Remark, Description, _
                                                                      , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, _
                                                                      AssemblyTypeID, MaintenanceEvent, , , , , , , , , , , , , , , , , , , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , , StatusID.ToString, StatusType.ComponentDirective, CompStatusID.ToString, tmpAssemblyStatusID.ToString))
                                            End If
                                        End If
                                    Next
                                Next
                            Next
                        Next
                    End If
                Next
            End If
        Else
            ReportMaintenanceDetails.Add(mMachineList, 1)
        End If
        Session("ReportMaintenanceDetails") = ReportMaintenanceDetails
        Return ReportMaintenanceDetails
    End Function
    Private Sub SetReport()

        ReportMaintenanceDetails = New ReportMaintenanceDetailList
        ReportStatusList = New rptStatusList

        Dim mCompanyDetail As New CompanyDetail
        Dim searchstr As String = ""

        SetValues()
        ReportDetail()


        Dim mloglist As LogList
        mloglist = LogList.GetLogList(New Guid(cmbAircraft.SelectedValue.ToString), , AsonDate)


        'Added By Rajnish on 26-11-2007
        searchstr = searchstr & ", " & "As On Date:" & New SmartDate(txtAsOnDate.Value.ToString).FormattedText
        '------------------------------

        'code added By Deven on 11-04-2008 ====================

        Dim x As String
        If mloglist.Count > 0 Then
            x = mloglist(0).LogDate.ToShortDateString
        Else
            x = txtAsOnDate.Value.ToString
        End If

        '    Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        'mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        ''mCompanyDetail.WebSite, ReportName, searchstr, searchstr1, Assembly1, "", "Aircraft is flown up to: " & New SmartDate(x).FormattedText, AppSettings("Product Version"), AppSettings("SINote"))

        If ReportMaintenanceDetails.Count = 0 Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
            msg1.ReplacePage = "wfMultiCompliance.aspx?Backpage="
            msg1.Show()
            Exit Sub
        End If

        If mIsPreview Then
            Session("ReportMaintenanceDetails") = ReportMaintenanceDetails
            'Saving Periods Limits
            Try
                SetGridObject()
                mDueLimits = CType(mDueLimits.Save, DueLimits)
                Session("mDueLimits") = mDueLimits
                DataFieldBind()
            Catch ex As Exception
                '
            End Try
        End If
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
                Case MsgBoxResult.OK
                    Session("Sender") = ""
                    SetCombo()
                    SetComboOfMachine(AOnDate)
                    SetFocus(cmbAircraft)
                    SetTypeCombo()
                    DataFieldBind()
                    Response.Redirect("wfMultiCompliance.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
            Response.Redirect("wfMultiCompliance.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage"))
        End If
    End Sub
#End Region

#Region " Data Binding "
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If DueType = 1 Then
            If custValidator.ControlToValidate = "cmbAircraft" Then
                If cmbAircraft.SelectedIndex <= 0 Then
                    custValidator.ErrorMessage = "Aircraft Required"
                    e.IsValid = False
                Else
                    e.IsValid = True
                End If
            End If
        End If
        If custValidator.ControlToValidate = "cmbType" Then  ''  Or custValidator.ControlToValidate = ""                   'Aircraft
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
    Public Sub DataFieldBind()
        mDueLimits = DueLimits.GetDueLimits(New Guid(cmbAircraft.SelectedValue.ToString))
        dgDuePeriodLimits.DataSource = mDueLimits
        Session("mDueLimits") = mDueLimits

        mPerDayLimits = PerDayLimits.GetPerDayLimits(New Guid(cmbAircraft.SelectedValue.ToString))
        '' gdPerDayLimit.DataSource = mPerDayLimits
        Session("mPerDayLimits") = mPerDayLimits

        DataBind()

        If CType(Session("OpenFindNowSelectLogForm"), Boolean) = True Then
            If Not IsNothing(MachineName) Or Not MachineName = Guid.Empty.ToString Then
                cmbAircraft.SelectedValue = MachineName
                cmbAssembly.DataSource = mAssemblyStatusList
                Session("mAssemblyStatusList") = mAssemblyStatusList
                cmbAssembly.DataBind()
            End If
            If Not IsNothing(AssemblyName) Or (Not New Guid(AssemblyName).Equals(Guid.Empty)) Then cmbAssembly.SelectedValue = AssemblyName

            dgDoneOnValue.DataSource = AssemblyStatusPeriodList
            dgDoneOnValue.DataBind()

            txtAsOnDate.Value = AsonDate
        End If

    End Sub
    Public Sub SetTypeCombo()
        If mTypeListForCofA Is Nothing Then
            mTypeListForCofA = TypeListForCofA.GetTypeListForCofA(CType(Open.DueReport, TypeListForCofA.Open), "")
        End If

        cmbType.DataSource = mTypeListForCofA
        cmbType.DataBind()

        If mServiceTypeList Is Nothing Then
            mServiceTypeList = PartMonitorServiceTypeList.GetPartMonitorServiceTypeList(, True)
        End If
        cmbServiceType.DataSource = mServiceTypeList
        Session("mServiceTypeList") = mServiceTypeList

        If mInspectionTypeList Is Nothing Then
            mInspectionTypeList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList()    ''ModelMonitorInspTypeList.serach.ExludingRoutineInspections)
        End If
        cmbInspectionType.DataSource = mInspectionTypeList
        Session("mInspectionTypeList") = mInspectionTypeList

        If mModificationTypeList Is Nothing Then
            mModificationTypeList = ModelMonitorModTypeList.GetModelMonitorModTypeList(, True)
        End If

        cmbModificationType.DataSource = mModificationTypeList
        Session("mModificationTypeList") = mModificationTypeList
        DataBind()
    End Sub
    Public Sub SetComboOfMachine(ByVal AsonDate As String)
        ''If DueType = 1 Then
        mMachineList = MachineList.GetMachineListMonitoringStatus(AsonDate, , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , True, "<SELECT>")
        'Else
        '    mMachineList = MachineList.GetMachineListMonitoringStatus(AsonDate, , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , True, "<SELECT>")
        ''End If

        cmbAircraft.DataSource = mMachineList
        Session("mMachineListForCompliance") = mMachineList
        cmbAircraft.DataBind()
    End Sub
    Public Sub SetCombo()
        GetSession()
        mTypeListForCofA = TypeListForCofA.GetTypeListForCofA(CType(Open.DueReport, TypeListForCofA.Open), "")
        cmbType.DataSource = mTypeListForCofA
        cmbType.DataBind()

        For i As Integer = 0 To cmbType.Items.Count - 1
            cmbType.Items.Item(i).Selected = True
        Next

        mServiceTypeList = PartMonitorServiceTypeList.GetPartMonitorServiceTypeList(, True)   'ServiceType
        mInspectionTypeList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList()   'Inspection Type 
        mModificationTypeList = ModelMonitorModTypeList.GetModelMonitorModTypeList(, True)   'Modification Type

        lblServiceType.Enabled = False
        cmbServiceType.Enabled = False

        lblModificationType.Enabled = False
        cmbModificationType.Enabled = False

        lblInspectionType.Enabled = False
        cmbInspectionType.Enabled = False

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
#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        If Not IsPostBack And Session("Sender") = "" Then
            DueType = 1
            Session("DueType") = DueType
            Session("MiddleFrame") = "wfMultiCompliance.aspx?"
            If (CType(Session("OpenFindNowSelectLogForm"), Boolean) = False) Then
                ResetValues()
                lblAssembly.Enabled = False
                cmbAssembly.Enabled = False
                txtAsOnDate.Value = Today.Date
                AOnDate = Today.Date
            End If
            SetCombo()
            SetComboOfMachine(AOnDate)
            SetFocus(cmbAircraft)
            SetTypeCombo()
            DataFieldBind()
            pnlAdvancedSearch.Visible = False
            Session("mLogList") = Nothing
            Report = 1
            Session("Report") = Report
            SetLog()
        End If
        cmbType.DataBind()
        SetSession()
        MessageBoxResult()
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mMachineList = Nothing
        mDueLimits = Nothing
        mAssemblyStatusList = Nothing
        mServiceTypeList = Nothing
        mInspectionTypeList = Nothing
        mModificationTypeList = Nothing
        ReportMaintenanceDetails = Nothing
        AssemblyStatusPeriodList = Nothing
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub txtAsOnDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAsOnDate.TextChanged
        AOdate = txtAsOnDate.Value.ToString
        If AOnDate = AOdate Then
        Else
            SetComboOfMachine(AOdate)
            lblAssembly.Enabled = False
            cmbAssembly.Enabled = False

            AircraftIndex = cmbAircraft.SelectedIndex
            Session("AircraftIndex") = AircraftIndex
            cmbAssembly.SelectedIndex = 0
        End If
    End Sub
    Private Sub txtAsOnDate_CalendarVisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAsOnDate.CalendarVisibleChanged
        Me.cmbAircraft.Visible = Not CType(sender, Boolean)
        Me.cmbAssembly.Visible = Not CType(sender, Boolean)
        Me.cmbType.Visible = Not CType(sender, Boolean)
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        If cmbAircraft.SelectedIndex = 0 Then
            lblAssembly.Enabled = False
            cmbAssembly.Enabled = False
            AircraftIndex = cmbAircraft.SelectedIndex
            Session("AircraftIndex") = AircraftIndex

            cmbAssembly.SelectedIndex = 0
        Else
            lblAssembly.Enabled = True
            cmbAssembly.Enabled = True

            mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtAsOnDate.Value.ToString, cmbAircraft.SelectedValue, , , , , , , , , , True, , , , , , , , , , , , , , , , , , , True).Item(1), MachineInfo).AssemblyStatusList
            ''mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtAsOnDate.Value.ToString, cmbAircraft.SelectedValue, , , , , , , , , , True, , , , "Airframe").Item(0), MachineInfo).AssemblyStatusList
            cmbAssembly.DataSource = mAssemblyStatusList
            Session("mAssemblyStatusList") = mAssemblyStatusList

            AircraftIndex = cmbAircraft.SelectedIndex
            Session("AircraftIndex") = AircraftIndex

            AssemblyStatusPeriodList = mAssemblyStatusList(0).AssemblyStatusPeriodList
            Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList

            cmbAssembly.DataBind()
            SetValues()
        End If
        Session.Remove("OpenFindNowSelectLogForm")
        If cmbAircraft.Enabled = True Then
            SetFocus(cmbAircraft)
        End If
        SetTypeCombo()
        DataFieldBind()
        FillTypeCombo()
    End Sub
    Private Sub cmbAssembly_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAssembly.SelectedIndexChanged
        'If cmbAssembly.SelectedItem.Text = "(All)" Or (cmbAssembly.SelectedItem.Text = "<SELECT>") Then
        If cmbAssembly.SelectedIndex = 0 Then
            Dim tmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtAsOnDate.Value.ToString, cmbAircraft.SelectedValue, , , , , , , , , , True, , , , "Airframe").Item(0), MachineInfo).AssemblyStatusList
            AssemblyStatusPeriodList = tmpAssemblyStatusList(0).AssemblyStatusPeriodList
            Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
            tmpAssemblyStatusList = Nothing
        Else
            AssemblyStatusPeriodList = mAssemblyStatusList(cmbAssembly.SelectedIndex).AssemblyStatusPeriodList
            Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
        End If
        dgDoneOnValue.DataSource = AssemblyStatusPeriodList
        dgDoneOnValue.DataBind()
    End Sub
    Private Sub cmbType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbType.SelectedIndexChanged
        Try
            '' FillTypeCombo()
            Dim j As Integer
            For j = 0 To cmbType.Items.Count - 1

                'cmbServiceType Enabled
                If cmbType.Items(j).Selected = True And cmbType.Items(j).Text = "Service" Then
                    cmbServiceType.Enabled = cmbType.Items.Item(cmbType.SelectedIndex).Selected
                    For i As Integer = 0 To cmbServiceType.Items.Count - 1
                        cmbServiceType.Items.Item(i).Selected = cmbServiceType.Enabled
                    Next
                End If

                'cmbInspectionType Enabled
                If cmbType.Items(j).Selected = True And cmbType.Items(j).Text = "Inspection" Then
                    cmbInspectionType.Enabled = cmbType.Items.Item(cmbType.SelectedIndex).Selected
                    For i As Integer = 0 To cmbInspectionType.Items.Count - 1
                        cmbInspectionType.Items.Item(i).Selected = cmbInspectionType.Enabled
                    Next
                End If

                'cmbModificationType Enabled
                If cmbType.Items(j).Selected = True And (cmbType.Items(j).Text = "Modification" Or cmbType.Items(j).Text = "Directive") Then
                    cmbModificationType.Enabled = cmbType.Items.Item(cmbType.SelectedIndex).Selected
                    For i As Integer = 0 To cmbModificationType.Items.Count - 1
                        cmbModificationType.Items.Item(i).Selected = cmbModificationType.Enabled
                    Next
                End If
            Next

            Dim k As Integer
            For k = 0 To cmbType.Items.Count - 1

                'cmbService Disabled
                If cmbType.Items(k).Selected = False And cmbType.Items(k).Text = "Service" Then
                    cmbServiceType.Enabled = False
                    For l As Integer = 0 To cmbServiceType.Items.Count - 1
                        cmbServiceType.Items.Item(l).Selected = False
                    Next
                End If

                'cmbInspection Disabled
                If cmbType.Items(k).Selected = False And cmbType.Items(k).Text = "Inspection" Then
                    cmbInspectionType.Enabled = False
                    For l As Integer = 0 To cmbInspectionType.Items.Count - 1
                        cmbInspectionType.Items.Item(l).Selected = False
                    Next
                End If

                'cmbModification Disabled
                If cmbType.Items(k).Selected = False And (cmbType.Items(k).Text = "Modification" Or cmbType.Items(k).Text = "Directive") Then
                    cmbModificationType.Enabled = False
                    For l As Integer = 0 To cmbModificationType.Items.Count - 1
                        cmbModificationType.Items.Item(l).Selected = False
                    Next
                End If
            Next
        Catch ex As Exception
        End Try
    End Sub
    Private Sub cmbServiceType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbServiceType.SelectedIndexChanged
        ' SerIndex = cmbServiceType.SelectedIndex
        Session("SerIndex") = SerIndex
        ' lblServiceType1.Text = "Service type : " & cmbServiceType.SelectedItem.Text
    End Sub
    Private Sub cmbInspectionType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbInspectionType.SelectedIndexChanged
        ' InspIndex = cmbInspectionType.SelectedIndex
        Session("InspIndex") = InspIndex
        'lblInspectionType1.Text = "Inspection Type : " & cmbInspectionType.SelectedItem.Text
    End Sub
    Private Sub cmbModificationType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbModificationType.SelectedIndexChanged
        '  ModIndex = cmbModificationType.SelectedIndex
        Session("ModIndex") = ModIndex
        ' lblModificationType1.Text = "Modification Type : " & cmbModificationType.SelectedItem.Text
    End Sub
    Private Sub lbtnAdvancedSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtnAdvancedSearch.Click
        If pnlAdvancedSearch.Visible = False Then
            pnlAdvancedSearch.Visible = True
            FillTypeCombo()
            lblStep4.Visible = True
            lblStep4.Text = "Step IV. Selection of Type"
            lblStep5.Text = "Step V. Selection of Due Limits"
        ElseIf pnlAdvancedSearch.Visible = True Then
            pnlAdvancedSearch.Visible = False
            lblStep4.Visible = False
            lblStep5.Text = "Step IV. Selection of Due Limits"
        End If
    End Sub
    Private Sub btnFindNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        mIsPreview = True
        If IsValid = True Then
            SetReport()
            Session("LogId") = CType(Session("LogId"), String)
            Session("OpenFindNowSelectLogForm") = True
            Session("AssemblyStatusPeriodList") = AssemblyStatusPeriodList
            Dim str As String
            str = "<script language='javascript'>openledgersame('wfMultiComplanceList.aspx?ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&BackPage=Index.aspx" & "&DoneOn=" & CStr(IIf(txtAsOnDate.Value.ToString = "", Today.Date.ToShortDateString, txtAsOnDate.Value)) & "&MachineId=" & MachineName & "&HourType=" & mMachineList(New Guid(MachineName)).HourType & "&AssemblyID=" & AssemblyName.ToString & "'); </script>"
            ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)
        End If
    End Sub
    Private Sub btnSelectLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectLog.Click

        If IsValid = True Then
            SetSession()
            Session("OpenFindNowSelectLogForm") = True
            SetValues()
            Dim mtmpAssemblyStatusList As AssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(AsonDate, MachineName, , , , , , , , , , True, , , , "Airframe").Item(0), MachineInfo).AssemblyStatusList

            Dim str As String
            'str = "<script language='javascript'>openledgersame('wfSelectLog.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&BackPage6=Index.aspx" & "&FromType=3&DoneOn=" & CStr(IIf(txtAsOnDate.Value.ToString = "", Today.Date.ToShortDateString, txtAsOnDate.Value)) & "&MachineId=" & MachineName & "&AssemblyStatusID=" & AssemblyStatusID.ToString & "&AssemblyID=" & AssemblyName.ToString & "'); </script>"
            str = "<script language='javascript'>openledgersame('wfSelectLog.aspx?BackPage=" & Request.QueryString("BackPage") & "&ChildPage=" & Request.QueryString("ChildPage") & "&GChildPage=" & Request.QueryString("GChildPage") & "&GChildPage1=" & Request.QueryString("GChildPage1") & "&GChildPage2=" & Request.QueryString("GChildPage2") & "&BackPage6=index.aspx" & "&FromType=3&DoneOn=" & CStr(IIf(AsonDate = "", Today.Date.ToShortDateString, AsonDate)) & "&MachineId=" & MachineName & "&AssemblyStatusID=" & mtmpAssemblyStatusList(0).ID.ToString & "&AssemblyID=" & mtmpAssemblyStatusList(0).AssemblyID.ToString & "'); </script>"
            ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)

        End If
    End Sub
#End Region
End Class
