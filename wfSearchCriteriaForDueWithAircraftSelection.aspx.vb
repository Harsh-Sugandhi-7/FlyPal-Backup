'Pallavi  - 27-07-2006

Partial Class wfSearchCriteriaForDueWithAircraftSelection
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "
    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownList1 As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtFromDate As SIControls.SICalendar
    Protected WithEvents lblSelectionType As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "Enumeration"                   'Added Code By Girish 25,April,2007
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
    Dim mMachineListForDue As MachineListForDue

    Dim mmMachineList As MachineList

    Dim mtmpMachineList As tmpMachineList
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

    Dim mMachineNames As MachineNames

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
    Dim DueStatus As Integer
    Dim searchstr7 As String = ""
#End Region

#Region " Helper Methods "
    Private Sub addAttributes()
        txtAvgMnths.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtAvgMnths').value,event)")
        txtForecastingLimit.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtForecastingLimit').value,event)")
    End Sub
    Private Sub SetGridObject()
        Dim txtLimit As TextBox
        Dim i As Int32
        For i = 0 To Me.dgDuePeriodLimits.Items.Count - 1
            txtLimit = CType(Me.dgDuePeriodLimits.Items(i).FindControl("txtLimit"), TextBox)
            mDueLimits.Item(i).PeriodLimit = CDec(Val(Trim(txtLimit.Text)))
        Next i
        Session("mDueLimits") = mDueLimits

        Dim txtPerDatLimit As TextBox
        Dim i1 As Int32
        For i1 = 0 To Me.gdPerDayLimit.Items.Count - 1
            txtPerDatLimit = CType(Me.gdPerDayLimit.Items(i1).FindControl("txtLimitPerDay"), TextBox)
            mPerDayLimits.Item(i1).PeriodLimit = CDec(Val(Trim(txtPerDatLimit.Text)))
        Next i1
        Session("mPerDayLimits") = mPerDayLimits

    End Sub
    Private Sub GetSession()
        mMachineListForDue = CType(Session("mMachineListForDue"), MachineListForDue)
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
        mMachineNames = CType(Session("mMachineNames"), MachineNames)
    End Sub
    Private Sub SetSession()
        Session("mMachineListForDue") = mMachineListForDue
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
        Session("mMachineNames") = mMachineNames
    End Sub
    Private Sub ClearAll()
        DueType = Session("DueType")
        If Session("MiddleFrame") <> "wfSearchCriteriaForDueWithAircraftSelection.aspx?DueType=" & DueType Then
            Session.Remove("mMachineListForDue")
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
            Session.Remove("mMachineNames")
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
        lblAvgMnths1.Visible = (DueType = 1)
        lblDateRangeFrom.Visible = True
        lblPercent.Visible = (DueType = 1)
        lblAssembly1.Visible = True
        ''lblType1.Visible = True
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
        'If (cmbAircraft.SelectedItem.Text = "(All)") Or (cmbAircraft.SelectedItem.Text = "<SELECT>") Then
        '    MachineName = "{00000000-0000-0000-0000-000000000000}"
        '    AssemblyName = "{00000000-0000-0000-0000-000000000000}"
        '    Assembly1 = ""
        '    lblAssembly1.Text = ""
        'Else
        '    MachineName = cmbAircraft.SelectedValue.ToString

        '    'Added by Saylee on 12-Feb-2009
        '    If cmbAssembly.SelectedItem.Text = "(All)" Then
        '        AssemblyName = "{00000000-0000-0000-0000-000000000000}"
        '        Assembly1 = ""
        '        AssemblyType = "(All)"
        '        lblAssembly1.Text = "Assembly Name  : All"          'Added Code
        '    Else
        '        AssemblyType = mAssemblyList(cmbAssembly.SelectedIndex).AssemblyType
        '        AssemblyName = cmbAssembly.SelectedValue.ToString
        '        Assembly1 = cmbAssembly.SelectedItem.Text
        '        lblAssembly1.Text = "Assembly Name : " & Assembly1  'Added Code
        '    End If
        'End If
        Average = txtAvgMnths.Text
        If Not (txtFromDate.IsDateValue) Then
            AsonDate = ""
        Else
            AsonDate = txtFromDate.Value.ToString
        End If
        'Aircraft = IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "")
        ' TypeName = IIf(cmbType.SelectedIndex > 0, cmbType.SelectedItem.Text, "")

        If AsonDate <> "" Then
            lblDateRangeFrom.Text = "As On Date : " & New SmartDate(txtFromDate.Value.ToString).FormattedText
        Else
            lblDateRangeFrom.Text = "As On Date : " & "All"
        End If

        'lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "All")
        lblAvgMnths1.Text = "Average Months : " & IIf(Average <> "", Average, "All")
        percent = txtPercentage.Text
        lblPercent.Text = "Percent : " & IIf(percent <> "", percent, "All")
        'lblType1.Text = "Type : " & IIf(TypeName <> "", TypeName, "All")


        If cmbType.Items.Count <> 0 Then
            ' If so, loop through all checked items and print results.
            Dim x As Integer
            For x = 0 To cmbType.Items.Count - 1
                'info = mTypeListForCofA.Item(x)
                'If info.Name = "Service" Then   'Service
                If cmbType.Items(x).Selected = True And cmbType.Items(x).Text = "Service" Then   'Added by Prashant 7/12/07  For showing report name and to set correct  'selected' value
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
    Private Sub ResetValues()
        MachineName = "{00000000-0000-0000-0000-000000000000}"
        'CNDC
        'txtFromDate.Value = AsonDate
        If AsonDate <> "" Then
            txtFromDate.Value = AsonDate
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
    End Sub
    Private Sub AddAircraft()
        Dim item As DataGridItem
        Dim chkBox As CheckBox
        Dim RegNo As String
        Dim RecordNo, PageItems As Integer
        Dim i As Integer
        PageItems = dgMachineList.Items.Count - 1
        For i = 0 To PageItems

            RecordNo = i + dgMachineList.PageSize * dgMachineList.CurrentPageIndex
            item = dgMachineList.Items(i)
            RegNo = item.Cells(2).Text
            chkBox = CType(item.FindControl("chkSelect"), CheckBox)
            mMachineNames(RegNo).IsSelected = chkBox.Checked
        Next
        Session("mMachineNames") = mMachineNames
    End Sub
    Private Sub ForCount()

    End Sub
    Public Function ReportDetail() As ReportMaintenanceDetailList


        If rbdPercent.Checked Then mDueLimits.SetPercentageWise(True, CDec(Val(txtPercentage.Text)))
        AddAircraft()
        mMachineListForDue = MachineListForDue.GetMachineListForDue(AsonDate, mDueLimits, , , AvgMnths, rbdSpecifyValues.Checked, mPerDayLimits, , , , , , , , True, True, True, Val(txtForecastingLimit.Text), mMachineNames)

        'For k As Integer = 0 To mMachineNames.Count - 1
        '    If mMachineNames(k).IsSelected = True Then
        '        '        mtmpMachineList = tmpMachineList.GetMachineList(, mMachineNames(k).RegNo, , , , , True, AsonDate)
        '        '        For i As Integer = 0 To mtmpMachineList.Count - 1
        '        '            ReportStatusList.Add(New rptStatus(mtmpMachineList(i).ID.ToString, 1, mtmpMachineList(i).RegNo, , , , , , , , , , , , , , , mtmpMachineList(i).Cycles, , , Year(New SmartDate(txtFromDate.Value.ToString).FormattedText).ToString, , mtmpMachineList(i).RegNo, mtmpMachineList(i).ModelName, mtmpMachineList(i).Type, mtmpMachineList(i).SerialNo, mtmpMachineList(i).ManufacturerName, , mtmpMachineList(i).ManufacturingDate, mtmpMachineList(i).Hours, mtmpMachineList(i).Landings))
        '        '            Session("AircraftAsOnDate") = mtmpMachineList(i).ManufacturingDateFormatted
        '        '        Next
        '        Session("mMachineListForDue") = mMachineListForDue
        '    End If
        'Next

        ReportMaintenanceDetails.Add(mMachineListForDue, Report)

        Return ReportMaintenanceDetails
    End Function
    Private Sub SetReport()

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

        ReportDetail()

        'Code Added by Deven on 02-Mar-20098*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/*/***/*/*/*/*/*/*/*/
        Dim mloglist As LogList
        mloglist = LogList.GetLogList(Guid.Empty, , AsonDate)
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
        searchstr = searchstr & ", " & "As On Date:" & New SmartDate(txtFromDate.Value.ToString).FormattedText
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
        'Code Added By Deven on 07/04/2008------------
        Dim rptDueDetail As CrystalDecisions.CrystalReports.Engine.ReportClass
        If DueType = 1 Then
            '' rptSnagCorrectiveActionListForDue = SnagCorrectiveActionListForDue.GetSnagCorrectiveActionListForDue(New Guid(cmbAircraft.SelectedValue.ToString), AsonDate)  'Added By Prashant 20-Nov-2009
            If AppSettings("TimeFormat") = "HH:mm" Or AppSettings("TimeFormat") = "hh:mm" Then
                rptSnagCorrectiveActionListForDue = MELSnagCorrectiveActionListForDue.GetMELSnagCorrectiveActionListForDue(AsonDate, Guid.Empty, Guid.Empty, 0, 0, "HH:mm")
            Else
                rptSnagCorrectiveActionListForDue = MELSnagCorrectiveActionListForDue.GetMELSnagCorrectiveActionListForDue(AsonDate, Guid.Empty, Guid.Empty, 0, 0)
            End If

            'If Not cmbAircraft.SelectedItem.ToString = "(All)" Then
            '    'Added by Saylee on 25-Sep-2008 for showing Due Certificates
            '    rptMachineCertificates = MachineCertificateList.GetMachineCertificateList(Guid.Empty, AsonDate)

            '    If rptMachineCertificates.Count = 0 Then
            '        If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "Indamer")) Then
            '            rptDueDetail = New crDueReportDetailLandscapePerAircraftIndamar 'This change is applied to Indamar 
            '        ElseIf ((Not AppSettings("ClientCode") Is Nothing) AndAlso (((AppSettings("ClientCode") = "TAAL" OR AppSettings("ClientCode") = "GlobalJet")))) Or ((Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "KamAir"))) Then
            '            If rptSnagCorrectiveActionListForDue.Count <> 0 Then
            '                rptDueDetail = New crDueReportDetailLandscapePerAircraftTaal
            '            Else
            '                rptDueDetail = New crDueReportDetailLandscapePerAircraftWithoutSnagTaal
            '            End If
            '        ElseIf ((Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "Deccan") AndAlso ((AppSettings("ClientCode") = "ADeccan"))) Then
            '            rptDueDetail = New crDueReportDetailLandscapePerAircraftForDeccan '--------------------------------
            '            'rptDueDetail = New crDueReportDetailLandscapePerAircraft
            '        ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "Heligo")) Then
            '            rptDueDetail = New crDueReportDetailLandscapePerAircraftHeligo  'This change is applied to Heligo

            '        Else
            '            rptDueDetail = New crDueReportDetailLandscapePerAircraft
            '        End If
            '    Else
            '        If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "Indamer")) Then
            '            rptDueDetail = New crDueReportDetailLandscapePerAircraftCertificatesIndamar 'This change is applied to Indamar and Heligo
            '        ElseIf ((Not AppSettings("ClientCode") Is Nothing) AndAlso (((AppSettings("ClientCode") = "TAAL" OR AppSettings("ClientCode") = "GlobalJet")))) Or ((Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "KamAir"))) Then
            '            If rptSnagCorrectiveActionListForDue.Count <> 0 Then
            '                rptDueDetail = New crDueReportDetailLandscapePerAircraftCertificatesTaal
            '            Else
            '                rptDueDetail = New crDueReportDetailLandscapePerAircraftCertificatesWithoutSnagTaal
            '            End If
            '        ElseIf ((Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "Deccan") AndAlso ((AppSettings("ClientCode") = "ADeccan"))) Then
            '            rptDueDetail = New crDueReportDetailLandscapePerAircraftCertificatesForDeccan
            '            'rptDueDetail = New crDueReportDetailLandscapePerAircraftCertificates
            '        ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "Heligo")) Then
            '            rptDueDetail = New crDueReportDetailLandscapePerAircraftCertificatesHeligo  'This change is applied to Heligo
            '        Else
            '            rptDueDetail = New crDueReportDetailLandscapePerAircraftCertificates
            '        End If

            '    End If
            'Else
            rptDueDetail = New crDueReportDetailInd
            'End If

            If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
                If (cmbAircraft.SelectedItem.Text = "(All)") Or (cmbAircraft.SelectedItem.Text = "<SELECT>") Then
                    ReportName = "Work Order List"
                Else
                    ReportName = "Work Order List Number " + "__________________________" + " / " + cmbAircraft.SelectedItem.Text + " / " + MonthName(Month(New SmartDate(txtFromDate.Value.ToString).FormattedText), True).ToString + "." + " / " + Year(New SmartDate(txtFromDate.Value.ToString).FormattedText).ToString + " ."
                End If

            Else
                ReportName = "Maintenance Status Report"
            End If


        Else
            'If cmbSordBy.SelectedIndex = 0 Then
            '    rptDueDetail = New crMaintenanceAdvicePPC
            'Else
            '    rptDueDetail = New crMaintenanceAdvice
            'End If

            'ReportName = "Maintenance Advice From QC"
        End If
        '-------------------------------------------
        Dim x As String
        If mloglist.Count > 0 Then
            x = mloglist(0).LogDate.ToShortDateString
        Else
            x = txtFromDate.Value.ToString
        End If

        '--------------------------------------------------------
        Dim LastFlownDate As String = ""
        Dim LastMaintenanceActivityDate As String = ""
        'Dim mMaxLogNo As MaxLogNo = MaxLogNo.GetMaxLogNo(AsonDate, New Guid(MachineName), New Guid(AssemblyName))

        'If mMaxLogNo.Count <> 0 Then
        '    LastFlownDate = mMaxLogNo(0).LogDate.ToString 'Last Flight Log Date
        'Else
        LastFlownDate = CType(Session("AircraftAsOnDate"), String)  'New SmartDate(txtFromDate.Value.ToString).FormattedText
        'End If

        'Added by Saylee on 2-Aug-2011
        ''Last Maintenance Activity
        'If Not cmbAircraft.SelectedItem.ToString = "(All)" Then
        Dim mLastMachineMaintenanceActivity As LastMachineMaintenanceActivity = LastMachineMaintenanceActivity.GetLastMaintenanceActivity(AsonDate, Guid.Empty, Guid.Empty)
        If Not mLastMachineMaintenanceActivity.ID.Equals(Guid.Empty) Then
            LastMaintenanceActivityDate = ", Last Maintenance Done on  " + "( " + mLastMachineMaintenanceActivity.Date.ToString + " )"
            searchstr8 = mLastMachineMaintenanceActivity.Date.ToString
        End If
        ''***************************************
        'End If

        If ((Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ")) Then ' SPZ Code added by Saylee on 13-Jun-2022
            searchstr6 = "Flying Hours updated till " + "( " + LastFlownDate + " ) " + LastMaintenanceActivityDate + " & Work Order Number - _______________________"
        Else
            searchstr6 = LastFlownDate 'Mostly on Heligo Report
        End If

        'Added by vikrant on 11-Aug-2011
        'If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "Indamer")) Then
        '    Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue))
        '    If cmbAircraft.SelectedIndex > 0 Then
        '        If mMachineOperatorName.OperatorName <> "" Then OperatorName = mMachineOperatorName.OperatorName
        '    End If
        'ElseIf (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "Deccan") AndAlso ((AppSettings("ClientCode") = "ADeccan")) Then
        OperatorName = searchstr7
        'End If
        '--------------------------------------------------------

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
    mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
    mCompanyDetail.WebSite, ReportName, searchstr, searchstr1, Assembly1, cmbSordBy.SelectedIndex.ToString, "Aircraft is flown up to: " & New SmartDate(x).FormattedText, AppSettings("Product Version"), AppSettings("SINote"), searchstr6, OperatorName, searchstr8)
        If ReportMaintenanceDetails.Count = 0 Then
            Dim msg1 As New SIMsgBox(Page, SIMsgBox.Message_title.NoRecocordFound, SIMsgBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OKOnly)
            msg1.ReplacePage = "wfSearchCriteriaForDueWithAircraftSelection.aspx?Backpage=" & "&DueType=" & DueType
            msg1.Show()
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 719)
        End If

        '11-Sep-2008-------------------------------
        If Not mIsPreview Then
            ds.Clear()
            da.Fill(ds, ReportMaintenanceDetails)

            'Added by Saylee on 25-Sep-2008 for showing Due Certificates
            If DueType = 1 Then
                'If Not cmbAircraft.SelectedItem.ToString = "(All)" Then
                'If rptMachineCertificates.Count <> 0 Then da.Fill(ds, rptMachineCertificates)
                'End If
            End If

            '===================================
            da.Fill(ds, Report)
            da.Fill(ds, ReportStatusList)
            da.Fill(ds, rptSnagCorrectiveActionListForDue) 'Added By Prashant 20-Nov-2009
            rptDueDetail.SetDataSource(ds)
            Session("CrystalReport") = rptDueDetail

            Dim Str As String
            Str = "<script language=Javascript>openTranDetail();</script>"
            ClientScript.RegisterStartupScript(Me.GetType(), "openTranDetail", Str)
            ResetValues()

            'Saving Periods Limits
            Try
                SetGridObject()
                mDueLimits = CType(mDueLimits.Save, DueLimits)
                Session("mDueLimits") = mDueLimits
                DataFieldBind()
                GridBindWithSession()
            Catch ex As Exception
                '
            End Try
        Else
            Session("ReportMaintenanceDetails") = ReportMaintenanceDetails
            Session("wfSearchCriteriaForDueWithAircraftSelection") = "wfSearchCriteriaForDueWithAircraftSelection"
            Dim str As String
            str = "<script language='javascript'>openledgersame('wfDueResult.aspx?'); </script>"
            ClientScript.RegisterStartupScript(Me.GetType(), "OpenScript", str)
        End If
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = CType(Request.QueryString("MsgResult"), MsgBoxResult)
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If CType(Session("sender"), String) = "Continue" Then
                        Try
                            Session("Sender") = ""
                            txtFromDate.Value = Now.Date
                            GridBindWithSession()
                            SetGridObject()
                            DataFieldBind()
                            SetReport()
                        Catch ex As Exception

                        End Try
                    End If
                Case MsgBoxResult.No
                    Session("Sender") = ""
                    Response.Redirect("wfSearchCriteriaForDueWithAircraftSelection.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&DueType=" & DueType)
                Case MsgBoxResult.OK
                    Session("Sender") = ""
                    Response.Redirect("wfSearchCriteriaForDueWithAircraftSelection.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&DueType=" & DueType)
                Case Else
                    '
            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
            Response.Redirect("wfSearchCriteriaForDueWithAircraftSelection.aspx?MsgResult=0&BackPage=" & Request.QueryString("BackPage") & "&DueType=" & DueType)
        End If
    End Sub
    Private Sub Controltovisibility()
        If DueType = 1 Then
            lblSortBy.Visible = False
            cmbSordBy.Visible = False
            lbltitle.Text = "Search criteria for Due"
            If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
                Label4.Visible = False
                lblLimit.Visible = False
                txtForecastingLimit.Visible = False
                lblStep7.Text = "Step VI. Display Report"
            End If
        Else
            lblSortBy.Visible = True
            cmbSordBy.Visible = True
            lblStep6.Visible = False
            Label2.Visible = False
            rbdAvrageMonths.Visible = False
            rbdSpecifyValues.Visible = False
            lblAvgMnths.Visible = False
            txtAvgMnths.Visible = False
            lblMonths.Visible = False
            lblInfo.Visible = False
            lblAvgMnths1.Visible = False
            gdPerDayLimit.Visible = False
            lblStep7.Text = "Step V. Display Report"
            lbltitle.Text = "Search criteria for Maintenance Advice From QC"
        End If
    End Sub
#End Region

#Region " Data Binding "
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidator As CustomValidator
        custValidator = CType(s, CustomValidator)
        If DueType = 2 Then
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
        mDueLimits = DueLimits.GetDueLimits(Guid.Empty)
        dgDuePeriodLimits.DataSource = mDueLimits
        Session("mDueLimits") = mDueLimits

        mPerDayLimits = PerDayLimits.GetPerDayLimits(Guid.Empty)
        gdPerDayLimit.DataSource = mPerDayLimits
        Session("mPerDayLimits") = mPerDayLimits

        'mMachineNames = MachineNames.GetMachineList(Today.Date.ToString)
        'dgMachineList.DataSource = mMachineNames
        'Session("mMachineNames") = mMachineNames

        DataBind()
    End Sub
    Public Sub GridBind()
        mMachineNames = MachineNames.GetMachineList(Today.Date.ToString)
        dgMachineList.DataSource = mMachineNames
        Session("mMachineNames") = mMachineNames

        DataBind()
    End Sub
    Public Sub GridBindWithSession()
        mMachineNames = Session("mMachineNames")
        dgMachineList.DataSource = mMachineNames
        DataBind()
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
        If DueType = 1 Then
            mmMachineList = MachineList.GetMachineListMonitoringStatus(AsonDate, , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , True, "(All)")
        Else
            mmMachineList = MachineList.GetMachineListMonitoringStatus(AsonDate, , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , True, "<SELECT>")
        End If

        cmbAircraft.DataSource = mMachineListForDue
        Session("mMachineListForDue") = mMachineListForDue
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
        addAttributes()
        If Not IsPostBack And Session("Sender") = "" Then
            DueType = Request.QueryString("DueType")
            Session("DueType") = DueType
            Session("MiddleFrame") = "wfSearchCriteriaForDueWithAircraftSelection.aspx?DueType=" & DueType
            ResetValues()
            SetCombo()
            'SetFocus(txtFromDate)
            lblAssembly.Enabled = False
            cmbAssembly.Enabled = False
            txtFromDate.Value = Now.Date
            AOnDate = Now.Date
            SetComboOfMachine(AOnDate)
            SetFocus(cmbAircraft)
            SetTypeCombo()
            DataFieldBind()
            GridBind()
            pnlAdvancedSearch.Visible = False
            Report = 1
        End If
        cmbType.DataBind()
        Controltovisibility()
        addAttributes()
        SetSession()
        MessageBoxResult()
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        If IsValid = True Then
            Display()
            SetValues()
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        Dim chkBox As CheckBox
        Dim i As Integer
        Dim Count As Integer = 0
        For i = 0 To dgMachineList.Items.Count - 1
            chkBox = CType(dgMachineList.Items.Item(i).Cells(1).FindControl("chkSelect"), CheckBox)
            If chkBox.Checked = True Then
                Count = Count + 1
            End If
        Next
        If Count > 5 Then
            ' ClientScript.RegisterStartupScript(Me.GetType(),"OpenScript", MessageBox.Show("You can select 5 aircrafts only..."))
            'Exit Sub
            AddAircraft()
            Dim msg1 As New SIMsgBox(Page, "Alert!", "System may not display due of more than 5 aircrafts at one attempt.<BR> <BR> Do you want to continue? ", "", MsgBoxStyle.YesNo)
            msg1.ReplacePage = "wfSearchCriteriaForDueWithAircraftSelection.aspx?Backpage=" & "&DueType=" & DueType
            Session("sender") = "Continue"
            msg1.Show()
        Else
            If IsValid = True Then
                SetReport()
            End If
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mMachineListForDue = Nothing
        mDueLimits = Nothing
        mAssemblyList = Nothing
        'Added By Saylee on 20-Feb-2009
        mServiceTypeList = Nothing
        mInspectionTypeList = Nothing
        mModificationTypeList = Nothing
        '=============================
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub txtFromDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFromDate.TextChanged
        AOdate = txtFromDate.Value.ToString
        If AOnDate = AOdate Then
        Else
            SetComboOfMachine(AOdate)
        End If
    End Sub
    Private Sub txtAvgMnths_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAvgMnths.TextChanged
        If txtAvgMnths.Text = "" Then
        Else
            AvgMnths = CInt(txtAvgMnths.Text)
            Session("AvgMnths") = AvgMnths
        End If
    End Sub
    Private Sub txtFromDate_CalendarVisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFromDate.CalendarVisibleChanged
        Me.cmbAircraft.Visible = Not CType(sender, Boolean)
        Me.cmbAssembly.Visible = Not CType(sender, Boolean)
        Me.cmbType.Visible = Not CType(sender, Boolean)

        If DueType <> 1 Then
            Me.cmbSordBy.Visible = Not CType(sender, Boolean)
        End If
    End Sub
    Private Sub rbdPercent_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbdPercent.CheckedChanged
        txtPercentage.Enabled = True
        txtPercentage.Text = "10"
        mDueLimits.SetPercentageWise(True, CDec(Val(txtPercentage.Text)))
        Dim txtLimit As TextBox
        Dim i As Int32
        For i = 0 To Me.dgDuePeriodLimits.Items.Count - 1
            txtLimit = CType(Me.dgDuePeriodLimits.Items(i).FindControl("txtLimit"), TextBox)
            txtLimit.Enabled = False
        Next i
    End Sub
    Private Sub rbdDueLimits_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbdDueLimits.CheckedChanged
        txtPercentage.Enabled = False
        txtPercentage.Text = ""
        mDueLimits.UnSetPercentageWise()
        Dim txtLimit As TextBox
        Dim i As Int32
        For i = 0 To Me.dgDuePeriodLimits.Items.Count - 1
            txtLimit = CType(Me.dgDuePeriodLimits.Items(i).FindControl("txtLimit"), TextBox)
            txtLimit.Enabled = True
        Next i
    End Sub
    '11-Sep-2008--------------------
    Private Sub btnPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        mIsPreview = True
        If IsValid = True Then
            SetReport()
        End If
    End Sub
    '-------------------------------
    Private Sub rbdAvrageMonths_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbdAvrageMonths.CheckedChanged
        lblAvgMnths.Visible = True
        txtAvgMnths.Visible = True
        lblMonths.Visible = True
        pnlAvragePeriod.Visible = False
        lblInfo.Visible = False
    End Sub
    Private Sub rbdSpecifyValues_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbdSpecifyValues.CheckedChanged
        lblAvgMnths.Visible = False
        txtAvgMnths.Visible = False
        lblMonths.Visible = False
        pnlAvragePeriod.Visible = True
        lblInfo.Visible = True
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

            ''MachineName = cmbAircraft.SelectedValue.ToString
            ''mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(AsonDate, MachineName, , , , , , , , , , True, , , , , , , , , , , , , , , , , , , True).Item(1), MachineInfo).AssemblyStatusList
            'mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtFromDate.Value.ToString, cmbAircraft.SelectedValue, , , , , , , , , , True, , , , , , , , , , , , , , , , , , , True).Item(1), MachineInfo).AssemblyStatusList
            'cmbAssembly.DataSource = mAssemblyStatusList
            'Session("mAssemblyStatusList") = mAssemblyStatusList
            'cmbAssembly.DataBind()

            Dim mAssemblylist As AssemblyList
            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, txtFromDate.Value.ToString, "(All)", True)
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
        DataFieldBind()
        FillTypeCombo()
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
    Private Sub cmbServiceType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbServiceType.SelectedIndexChanged
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
            lblStep4.Text = "Step IV. Selection of Type"
            lblStep5.Text = "Step V. Selection of Due Limits / Percentage Life Remaining"
            lblStep6.Text = "Step VI. Estimated Flying Hours."
            Label4.Text = "Step VII. Enter The Limit For Forecasting "
            If DueType = 1 Then
                lblStep7.Text = "Step VIII. Display Report"
            Else
                lblStep7.Text = "Step VII. Display Report"
            End If
        ElseIf pnlAdvancedSearch.Visible = True Then
            pnlAdvancedSearch.Visible = False
            lblStep5.Text = "Step IV. Selection of Due Limits / Percentage Life Remaining"
            lblStep6.Text = "Step V. Estimated Flying Hours."
            Label4.Text = "Step VI. Enter The Limit For Forecasting "
            If DueType = 1 Then
                lblStep7.Text = "Step VII. Display Report"
            Else
                lblStep7.Text = "Step VI. Display Report"
            End If
        End If
    End Sub
#End Region

End Class
