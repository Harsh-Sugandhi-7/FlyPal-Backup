'Added by Utkarsh On 28-Jan-2014
Imports System.Linq
Imports System.Collections.Generic
Public Class wfSearchCriteriaForMaintenanceAdviceFromQC_Ajax
    Inherits System.Web.UI.Page

#Region " Variable Declaration "
    Dim mDueLimits As DueLimits
    Dim mMachineNameValueList As MachineNameValueList

    Dim mPerDayLimits As PerDayLimits
    Dim ReportStatusList As New rptStatusList
    Dim mMachineList As MachineList

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

    Private Flag As Int16
    Dim AOdate As String
    Dim AOnDate As String
    Dim Average As String
    Dim Aircraft As String
    Dim Report As Integer = 1
    Dim Periodcount, ATACode As Integer
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

    Dim EventLogDetails As String = String.Empty
    Dim ModelEstimatedManHrs As String = String.Empty
    Dim StatusID As Guid
    Dim nWONumber As String = ""
    Dim mnWOListForDueJobs As nWOListForDueJobs
    Private Zone, Area As String
    Private IsRII As Boolean
#End Region

#Region " Helper Methods "
    Private Sub SetGridObject()
        Dim txtLimit As TextBox
        Dim i As Int32
        For i = 0 To Me.dgDuePeriodLimits.Rows.Count - 1
            txtLimit = CType(Me.dgDuePeriodLimits.Rows(i).FindControl("txtLimit"), TextBox)
            mDueLimits.Item(i).PeriodLimit = CDec(Val(Trim(txtLimit.Text)))
        Next i
        Session("mDueLimits") = mDueLimits
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
        mAssemblyStatusList = CType(Session("mAssemblyStatusList"), AssemblyStatusList)
        mServiceTypeList = CType(Session("mServiceTypeList"), PartMonitorServiceTypeList)
        mInspectionTypeList = CType(Session("mInspectionTypeList"), ModelMonitorInspTypeList)
        mModificationTypeList = CType(Session("mModificationTypeList"), ModelMonitorModTypeList)
        mMachineNameValueList = Session("mMachineNameValueList")
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
        Session("mAssemblyStatusList") = mAssemblyStatusList
        Session("SerIndex") = SerIndex
        Session("InspIndex") = InspIndex
        Session("ModIndex") = ModIndex
        Session("mServiceTypeList") = mServiceTypeList
        Session("mInspectionTypeList") = mInspectionTypeList
        Session("mModificationTypeList") = mModificationTypeList

        Session("mMachineNameValueList ") = mMachineNameValueList
    End Sub
    Private Sub ClearAll()
        DueType = Session("DueType")
        If Session("MiddleFrame") <> "wfSearchCriteriaForMaintenanceAdviceFromQC_Ajax.aspx?DueType=" & DueType Then
            Session.Remove("mMachineList")
            Session.Remove("mDueLimits")
            Session.Remove("mPerDayLimits")
            Session.Remove("AOnDate")
            Session.Remove("Report")
            Session.Remove("Type")
            Session.Remove("AvgMnths")

            'Added by Saylee on 12-Feb-2009
            Session.Remove("mAssemblyStatusList")
            Session.Remove("SerIndex")
            Session.Remove("InspIndex")
            Session.Remove("ModIndex")
        End If
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub Display()
        lblAircraft1.Visible = True
        lblAvgMnths1.Visible = (DueType = 1)
        lblDateRangeFrom.Visible = True
        lblPercent.Visible = (DueType = 1)
        lblAssembly1.Visible = True
        ''lblType1.Visible = True
        upnlCriteria.Update()
    End Sub
    Private Sub FillTypeCombo()
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
    Private Sub SetValues()
        If (cmbAircraft.SelectedItem.Text = "(All)") Or (cmbAircraft.SelectedItem.Text = "(SELECT)") Then
            MachineName = "{00000000-0000-0000-0000-000000000000}"
            AssemblyName = "{00000000-0000-0000-0000-000000000000}"
            Assembly1 = ""
            lblAssembly1.Text = ""
        Else
            MachineName = cmbAircraft.SelectedValue.ToString

            'Added by Saylee on 12-Feb-2009
            If cmbAssembly.SelectedItem.Text = "(All)" Then
                AssemblyName = "{00000000-0000-0000-0000-000000000000}"
                Assembly1 = ""
                AssemblyType = "(All)"
                lblAssembly1.Text = "Assembly Name  : All"          'Added Code
            Else
                AssemblyType = mAssemblyStatusList(cmbAssembly.SelectedIndex).AssemblyType
                AssemblyName = cmbAssembly.SelectedValue.ToString
                Assembly1 = cmbAssembly.SelectedItem.Text
                lblAssembly1.Text = "Assembly Name : " & Assembly1  'Added Code
            End If
        End If
        If Not IsDate(txtDate.Text.Trim) Then
            AsonDate = ""
        Else
            AsonDate = txtDate.Text.Trim
        End If
        Aircraft = IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, "")
        '' TypeName = IIf(cmbType.SelectedIndex > 0, cmbType.SelectedItem.Text, "")

        If AsonDate <> "" Then
            lblDateRangeFrom.Text = "As On Date : " & txtDate.Text.Trim
        Else
            lblDateRangeFrom.Text = "As On Date : " & "All"
        End If

        lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "All")
        lblAvgMnths1.Text = "Average Months : " & IIf(Average <> "", Average, "All")
        percent = txtPercentage.Text
        lblPercent.Text = "Percent : " & IIf(percent <> "", percent, "All")
        ''lblType1.Text = "Type : " & IIf(TypeName <> "", TypeName, "All")


        'Set Service/Inspection/Directive checkbox list values
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

        'If cmbType.Items.Item(x).ToString = "All" Then
        '    IsSerSelect = True
        '    IsInsSelect = True
        '    IsModSelect = True
        '    ServiceTypeID(0) = 0
        '    InspectionTypeID(0) = 0
        '    ModificationTypeID(0) = 0
        'End If
        '    Next x
        'End If
        Dim DueLimits As String = String.Empty
        Dim status As String = String.Empty
        status = IIf(rbdDueLimits.Checked, rbdDueLimits.Text, rbdPercent.Text)
        If rbdDueLimits.Checked Then
            DueLimits = status & " : " & String.Join(", ", (From c As DueLimit In mDueLimits
                        Select c.PeriodName & " : " & c.PeriodLimitFormatted).ToArray)
        Else
            DueLimits = status & " : " & txtPercentage.Text.Trim
        End If
        EventLogDetails = lblDateRangeFrom.Text + ", " + lblAircraft1.Text + ", " + lblAssembly1.Text + ", Sort By : " + cmbSordBy.SelectedItem.Text + ", Format : " + cmbFormat.SelectedItem.Text.Trim + ", " + DueLimits
    End Sub
    Private Sub ResetValues()
        MachineName = "{00000000-0000-0000-0000-000000000000}"
        'CNDC
        'txtFromDate.Value = AsonDate
        If AsonDate <> "" Then
            txtDate.Text = AsonDate
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
    Public Function ReportDetail(Optional ByVal IsPreviewClicked As Boolean = False) As ReportMaintenanceDetailList

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
        mMachineList = MachineList.GetMachineListDueMonitoringStatus(AsonDate, mDueLimits, MachineName, AssemblyName, AvgMnths, , mPerDayLimits, , IsSerSelect, IsInsSelect, IsModSelect, , , , IsSerSelect, IsInsSelect, IsModSelect, SkipIsForInventoryAircarft:=True)
        'mMachineList = MachineList.GetMachineListDueMonitoringStatus(AsonDate, mDueLimits, MachineName, AssemblyName, AvgMnths, , mPerDayLimits)
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
                Next
            Next
        End If

        If Not cmbAircraft.SelectedItem.ToString = "(All)" Then
            mtmpMachineList = tmpMachineList.GetMachineList(, Aircraft, , , , , True, AsonDate)
            If cmbFormat.SelectedIndex = 0 Then
                Dim x As Integer
                x = mtmpMachineList.Count
                If x = 1 Then
                    ReportStatusList.Add(New rptStatus(mtmpMachineList(0).ID.ToString, 1, , , , , , , , , , , , , , , , mtmpMachineList(0).Cycles, txtDate.Text.Trim, , , , mtmpMachineList(0).RegNo, mtmpMachineList(0).ModelName, mtmpMachineList(0).Type, mtmpMachineList(0).SerialNo, mtmpMachineList(0).ManufacturerName, , mtmpMachineList(0).ManufacturingDate, mtmpMachineList(0).Hours, mtmpMachineList(0).Landings, mtmpMachineList(0).Cycles, mtmpMachineList(0).RINS))
                ElseIf x = 2 Then
                    ReportStatusList.Add(New rptStatus(mtmpMachineList(0).ID.ToString, 1, mtmpMachineList(1).Hours, mtmpMachineList(1).Cycles, mtmpMachineList(1).Landings, , , , , , , , , , mtmpMachineList(1).Type, , , mtmpMachineList(0).Cycles, txtDate.Text.Trim, , , , mtmpMachineList(0).RegNo, mtmpMachineList(0).ModelName, mtmpMachineList(0).Type, mtmpMachineList(0).SerialNo, mtmpMachineList(0).ManufacturerName, , mtmpMachineList(0).ManufacturingDate, mtmpMachineList(0).Hours, mtmpMachineList(0).Landings, mtmpMachineList(0).Cycles, mtmpMachineList(0).RINS, mtmpMachineList(1).RINS))
                ElseIf x = 3 Then
                    ReportStatusList.Add(New rptStatus(mtmpMachineList(0).ID.ToString, 1, mtmpMachineList(1).Hours, mtmpMachineList(1).Cycles, mtmpMachineList(1).Landings, mtmpMachineList(2).Hours, mtmpMachineList(2).Cycles, mtmpMachineList(2).Landings, , , , , , , mtmpMachineList(1).Type, mtmpMachineList(2).Type, , mtmpMachineList(0).Cycles, txtDate.Text.Trim, , , , mtmpMachineList(0).RegNo, mtmpMachineList(0).ModelName, mtmpMachineList(0).Type, mtmpMachineList(0).SerialNo, mtmpMachineList(0).ManufacturerName, , mtmpMachineList(0).ManufacturingDate, mtmpMachineList(0).Hours, mtmpMachineList(0).Landings, mtmpMachineList(0).Cycles, mtmpMachineList(0).RINS, mtmpMachineList(1).RINS, mtmpMachineList(2).RINS))
                ElseIf x = 4 Then
                    ReportStatusList.Add(New rptStatus(mtmpMachineList(0).ID.ToString, 1, mtmpMachineList(1).Hours, mtmpMachineList(1).Cycles, mtmpMachineList(1).Landings, mtmpMachineList(2).Hours, mtmpMachineList(2).Cycles, mtmpMachineList(2).Landings, mtmpMachineList(3).Hours, mtmpMachineList(3).Cycles, mtmpMachineList(3).Landings, , , , mtmpMachineList(1).Type, mtmpMachineList(2).Type, mtmpMachineList(3).Type, mtmpMachineList(0).Cycles, txtDate.Text.Trim, mtmpMachineList(3).Type, , , mtmpMachineList(0).RegNo, mtmpMachineList(0).ModelName, mtmpMachineList(0).Type, mtmpMachineList(0).SerialNo, mtmpMachineList(0).ManufacturerName, , mtmpMachineList(0).ManufacturingDate, mtmpMachineList(0).Hours, mtmpMachineList(0).Landings, mtmpMachineList(0).Cycles, mtmpMachineList(0).RINS, mtmpMachineList(1).RINS, mtmpMachineList(2).RINS, mtmpMachineList(3).RINS))
                ElseIf x = 5 Then
                    ReportStatusList.Add(New rptStatus(mtmpMachineList(0).ID.ToString, 1, mtmpMachineList(1).Hours, mtmpMachineList(1).Cycles, mtmpMachineList(1).Landings, mtmpMachineList(2).Hours, mtmpMachineList(2).Cycles, mtmpMachineList(2).Landings, mtmpMachineList(3).Hours, mtmpMachineList(3).Cycles, mtmpMachineList(3).Landings, mtmpMachineList(4).Hours, mtmpMachineList(4).Cycles, mtmpMachineList(4).Landings, mtmpMachineList(1).Type, mtmpMachineList(2).Type, mtmpMachineList(3).Type, mtmpMachineList(0).Cycles, txtDate.Text.Trim, mtmpMachineList(4).Type, , , mtmpMachineList(0).RegNo, mtmpMachineList(0).ModelName, mtmpMachineList(0).Type, mtmpMachineList(0).SerialNo, mtmpMachineList(0).ManufacturerName, , mtmpMachineList(0).ManufacturingDate, mtmpMachineList(0).Hours, mtmpMachineList(0).Landings, mtmpMachineList(0).Cycles, mtmpMachineList(0).RINS, mtmpMachineList(1).RINS, mtmpMachineList(2).RINS, mtmpMachineList(3).RINS, mtmpMachineList(4).RINS))
                Else
                    For i As Integer = 0 To mtmpMachineList.Count - 1
                        ReportStatusList.Add(New rptStatus(mtmpMachineList(i).ID.ToString, 1, , , , , , , , , , , , , , , , mtmpMachineList(i).Cycles, , , , , mtmpMachineList(i).RegNo, mtmpMachineList(i).ModelName, mtmpMachineList(i).Type, mtmpMachineList(i).SerialNo, mtmpMachineList(i).ManufacturerName, , mtmpMachineList(i).ManufacturingDate, mtmpMachineList(i).Hours, mtmpMachineList(i).Landings))
                    Next
                End If
            Else
                For i As Integer = 0 To mtmpMachineList.Count - 1
                    ReportStatusList.Add(New rptStatus(mtmpMachineList(i).ID.ToString, 1, , , , , , , , , , , , , , , , mtmpMachineList(i).Cycles, , , , , mtmpMachineList(i).RegNo, mtmpMachineList(i).ModelName, mtmpMachineList(i).Type, mtmpMachineList(i).SerialNo, mtmpMachineList(i).ManufacturerName, , mtmpMachineList(i).ManufacturingDate, mtmpMachineList(i).Hours, mtmpMachineList(i).Landings))
                Next
            End If
        End If
        If IsSerSelect = True Then
            For Each ObjMachine In mMachineList
                For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                    For Each ObjAssemblyMonitorServiceStatus In ObjAssemblyStatus.AssemblyMonitorServiceStatusList
                        'loop through selected monitory types
                        If ServiceTypeID.Contains(ObjAssemblyMonitorServiceStatus.MonitorTypeID) Then
                            If ObjAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriodList.Count > 0 Then
                                If (ObjAssemblyMonitorServiceStatus.IsApplicable = True) And (Not (ObjAssemblyMonitorServiceStatus.MonitorTypeID = 1 And ObjAssemblyMonitorServiceStatus.IsCompleted = True)) Then
                                    ATAChapter = ObjAssemblyMonitorServiceStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorServiceStatus.ATANomenclature
                                    ATACode = ObjAssemblyMonitorServiceStatus.ATACode
                                    'Commented and added By Prashant 3-Apr-2013  'Indamer03042013
                                    'Description = ObjAssemblyMonitorServiceStatus.Description 
                                    Description = ObjAssemblyMonitorServiceStatus.Description & vbCrLf & IIf(ObjAssemblyMonitorServiceStatus.Reference <> "", "Task Code/Ref. : ", "") & ObjAssemblyMonitorServiceStatus.Reference
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
                                    If ObjAssemblyStatus.InstallationRemark = "" And ObjAssemblyMonitorServiceStatus.DoneRemark = "" Then Remark = "----------"
                                    Code = ObjAssemblyMonitorServiceStatus.ModelMonitorServiceCode  'Added By Saylee on 28-08-2008

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

                                    Dim tmpModelMonitorService As ModelMonitorService = ModelMonitorService.GetModelMonitorService(ObjAssemblyMonitorServiceStatus.ModelMonitorServiceID)
                                    ModelEstimatedManHrs = tmpModelMonitorService.RequiredManHours

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
											'Added PeriodID=11 By Vikrant For ALL 21062012
											'If ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 3 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 4 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 5 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 6 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 7 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 8 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 9 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 12 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 13 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 14 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 15 Or ObjAssemblyMonitorServiceStatusPeriod.PeriodID = 11 Then
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
                                    'Rajnish 08-08-2008
                                    RequiredManHours = ObjAssemblyMonitorServiceStatus.RequiredManHours
                                    Customer = ObjMachine.Customer

                                    AssemblyType = ObjAssemblyStatus.AssemblyType
                                    MaintenanceEvent = ObjAssemblyMonitorServiceStatus.Type

                                    'Added by Saylee 04-08-2008
                                    ExtensionDate = ObjAssemblyMonitorServiceStatus.ExtensionDate
                                    ApprovalRemark = ObjAssemblyMonitorServiceStatus.ApprovalRemark

                                    StatusID = ObjAssemblyMonitorServiceStatus.ID  'Added by Saylee on 6-May-2013 for ALL06052013-1
                                    If IsPreviewClicked Then 'Added by Saylee on 6-May-2013 for ALL06052013-1
                                        mnWOListForDueJobs = nWOListForDueJobs.GetWOListForDueJobs(StatusID)
                                        If mnWOListForDueJobs.Count > 0 Then
                                            nWONumber = mnWOListForDueJobs(0).WONumber + vbCrLf + mnWOListForDueJobs(0).WODateFormatted
                                        Else
                                            nWONumber = ""
                                        End If
                                    End If
                                    Zone = ObjAssemblyMonitorServiceStatus.Zone
                                    Area = ObjAssemblyMonitorServiceStatus.Area
                                    IsRII = ObjAssemblyMonitorServiceStatus.IsRII

                                    ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, , AssemblySerialNo, ATAChapter, , , Position, , MonitorTypeCode, Note, Remark, Description, _
              , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, _
              SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, AssemblyTypeID, MaintenanceEvent, ATACode, , , , , , , , , , , , , , , , , , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, ModelEstimatedManHours:=ModelEstimatedManHrs, WONumber:=nWONumber, Zone:=Zone, Area:=Area, IsRII:=IsRII, StatusID:=StatusID.ToString))
                                End If
                            End If
                        End If
                    Next

                    For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
                        For Each ObjCompMonitorServiceStatus In ObjCompStatus.CompMonitorServiceStatusList
                            If ObjCompMonitorServiceStatus.CompMonitorServiceStatusPeriodList.Count > 0 Then
                                If (ObjCompMonitorServiceStatus.IsApplicable = True) And (Not (ObjCompMonitorServiceStatus.MonitorTypeID = 1 And ObjCompMonitorServiceStatus.IsCompleted = True)) Then
                                    ATAChapter = ObjCompMonitorServiceStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorServiceStatus.ATANomenclature
                                    ATACode = ObjCompMonitorServiceStatus.ATACode
                                    'Commented and added By Prashant 3-Apr-2013  'Indamer03042013
                                    'Description = ObjCompMonitorServiceStatus.Description
                                    Description = ObjCompMonitorServiceStatus.Description & vbCrLf & IIf(ObjCompMonitorServiceStatus.Reference <> "", "Task Code/Ref. : ", "") & ObjCompMonitorServiceStatus.Reference
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
                                    If ObjCompStatus.InstallationRemark = "" And ObjCompMonitorServiceStatus.DoneRemark = "" Then Remark = "----------"
                                    Code = ObjCompMonitorServiceStatus.PartMonitorServiceCode
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

                                    Dim tmpPartMonitorService As PartMonitorService = PartMonitorService.GetPartMonitorService(ObjCompMonitorServiceStatus.PartMonitorServiceID)
                                    ModelEstimatedManHrs = tmpPartMonitorService.RequiredManHours

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
											'Added PeriodID=11 By Vikrant For ALL 21062012
											'If ObjCompMonitorServiceStatusPeriod.PeriodID = 3 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 4 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 5 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 6 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 7 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 8 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 9 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 12 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 13 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 14 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 15 Or ObjCompMonitorServiceStatusPeriod.PeriodID = 11 Then
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
                                    StatusID = ObjCompMonitorServiceStatus.ID  'Added by Saylee on 6-May-2013 for ALL06052013-1

                                    If IsPreviewClicked Then 'Added by Saylee on 6-May-2013 for ALL06052013-1
                                        mnWOListForDueJobs = nWOListForDueJobs.GetWOListForDueJobs(StatusID)
                                        If mnWOListForDueJobs.Count > 0 Then
                                            nWONumber = mnWOListForDueJobs(0).WONumber + vbCrLf + mnWOListForDueJobs(0).WODateFormatted
                                        Else
                                            nWONumber = ""
                                        End If
                                    End If
                                    Zone = ""
                                    Area = ""
                                    IsRII = False

                                    ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, MaintenanceEvent, AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, , MonitorTypeCode, Note, Remark, Description, _
                                                          , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, _
                                                          AssemblyTypeID, MaintenanceEvent, ATACode, , , , , , , , , , , , , , , , , , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, ModelEstimatedManHours:=ModelEstimatedManHrs, WONumber:=nWONumber, Zone:=Zone, Area:=Area, IsRII:=IsRII, StatusID:=StatusID.ToString))
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
                        If InspectionTypeID.Contains(ObjAssemblyMonitorInspStatus.MonitorTypeID) Then
                            If ObjAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriodList.Count > 0 Then
                                If (ObjAssemblyMonitorInspStatus.IsApplicable = True) And (Not (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True)) Then
                                    ATAChapter = ObjAssemblyMonitorInspStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorInspStatus.ATANomenclature
                                    ATACode = ObjAssemblyMonitorInspStatus.ATACode
                                    'Commented and added By Prashant 3-Apr-2013  'Indamer03042013
                                    'Description = ObjAssemblyMonitorInspStatus.Description
                                    Description = ObjAssemblyMonitorInspStatus.Description & vbCrLf & IIf(ObjAssemblyMonitorInspStatus.Reference <> "", "Task Code/Ref. : ", "") & ObjAssemblyMonitorInspStatus.Reference
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
                                    If ObjAssemblyStatus.InstallationRemark = "" And ObjAssemblyMonitorInspStatus.DoneRemark = "" Then Remark = "----------"
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

                                    Dim tmpModelMonitorInsp As ModelMonitorInsp = ModelMonitorInsp.GetModelMonitorInsp(ObjAssemblyMonitorInspStatus.ModelMonitorInspID)
                                    ModelEstimatedManHrs = tmpModelMonitorInsp.RequiredManHours

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
											'Added PeriodID=11 By Vikrant For ALL 21062012
											'If ObjAssemblyMonitorInspStatusPeriod.PeriodID = 3 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 4 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 5 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 6 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 7 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 8 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 9 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 12 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 13 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 14 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 15 Or ObjAssemblyMonitorInspStatusPeriod.PeriodID = 11 Then
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

                                    StatusID = ObjAssemblyMonitorInspStatus.ID  'Added by Saylee on 6-May-2013 for ALL06052013-1
                                    If IsPreviewClicked Then 'Added by Saylee on 6-May-2013 for ALL06052013-1
                                        mnWOListForDueJobs = nWOListForDueJobs.GetWOListForDueJobs(StatusID)
                                        If mnWOListForDueJobs.Count > 0 Then
                                            nWONumber = mnWOListForDueJobs(0).WONumber + vbCrLf + mnWOListForDueJobs(0).WODateFormatted
                                        Else
                                            nWONumber = ""
                                        End If
                                    End If
                                    Zone = ObjAssemblyMonitorInspStatus.Zone
                                    Area = ObjAssemblyMonitorInspStatus.Area
                                    IsRII = ObjAssemblyMonitorInspStatus.IsRII

                                    ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, , AssemblySerialNo, ATAChapter, , , Position, , MonitorTypeCode, Note, Remark, Description, _
                                           , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, _
                                           SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, _
                                           AssemblyTypeID, MaintenanceEvent, ATACode, , , , , , , , , , , , , , , , , , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, ModelEstimatedManHours:=ModelEstimatedManHrs, WONumber:=nWONumber, Zone:=Zone, Area:=Area, IsRII:=IsRII, StatusID:=StatusID.ToString))
                                End If
                            End If
                        End If
                    Next
                    For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
                        For Each ObjCompMonitorInspStatus In ObjCompStatus.CompMonitorInspStatusList
                            If ObjCompMonitorInspStatus.CompMonitorInspStatusPeriodList.Count > 0 Then
                                If (ObjCompMonitorInspStatus.IsApplicable = True) And (Not (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True)) Then
                                    ATAChapter = ObjCompMonitorInspStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorInspStatus.ATANomenclature
                                    ATACode = ObjCompMonitorInspStatus.ATACode
                                    'Commented and added By Prashant 3-Apr-2013  'Indamer03042013
                                    'Description = ObjCompMonitorInspStatus.Description
                                    Description = ObjCompMonitorInspStatus.Description & vbCrLf & IIf(ObjCompMonitorInspStatus.Reference <> "", "Task Code/Ref. : ", "") & ObjCompMonitorInspStatus.Reference
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
                                    If ObjCompStatus.InstallationRemark = "" And ObjCompMonitorInspStatus.DoneRemark = "" Then Remark = "----------"
                                    Code = ObjCompMonitorInspStatus.PartMonitorInspCode

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

                                    Dim tmpPartMonitorInsp As PartMonitorInsp = PartMonitorInsp.GetPartMonitorInsp(ObjCompMonitorInspStatus.PartMonitorInspID)
                                    ModelEstimatedManHrs = tmpPartMonitorInsp.RequiredManHours

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
											'Added PeriodID=11 By Vikrant For ALL 21062012
											'If ObjCompMonitorInspStatusPeriod.PeriodID = 3 Or ObjCompMonitorInspStatusPeriod.PeriodID = 4 Or ObjCompMonitorInspStatusPeriod.PeriodID = 5 Or ObjCompMonitorInspStatusPeriod.PeriodID = 6 Or ObjCompMonitorInspStatusPeriod.PeriodID = 7 Or ObjCompMonitorInspStatusPeriod.PeriodID = 8 Or ObjCompMonitorInspStatusPeriod.PeriodID = 9 Or ObjCompMonitorInspStatusPeriod.PeriodID = 12 Or ObjCompMonitorInspStatusPeriod.PeriodID = 13 Or ObjCompMonitorInspStatusPeriod.PeriodID = 14 Or ObjCompMonitorInspStatusPeriod.PeriodID = 15 Or ObjCompMonitorInspStatusPeriod.PeriodID = 11 Then
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
                                    StatusID = ObjCompMonitorInspStatus.ID  'Added by Saylee on 6-May-2013 for ALL06052013-1
                                    If IsPreviewClicked Then 'Added by Saylee on 6-May-2013 for ALL06052013-1
                                        mnWOListForDueJobs = nWOListForDueJobs.GetWOListForDueJobs(StatusID)
                                        If mnWOListForDueJobs.Count > 0 Then
                                            nWONumber = mnWOListForDueJobs(0).WONumber + vbCrLf + mnWOListForDueJobs(0).WODateFormatted
                                        Else
                                            nWONumber = ""
                                        End If
                                    End If
                                    Zone = ""
                                    Area = ""
                                    IsRII = False
                                    'If ObjCompMonitorInspStatus.CompMonitorInspStatusPeriodList.Count > 0 Then
                                    ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, MaintenanceEvent, AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, , MonitorTypeCode, Note, Remark, Description, _
                                                         , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, _
                                                         AssemblyTypeID, MaintenanceEvent, ATACode, , , , , , , , , , , , , , , , , , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, ModelEstimatedManHours:=ModelEstimatedManHrs, WONumber:=nWONumber, Zone:=Zone, Area:=Area, IsRII:=IsRII, StatusID:=StatusID.ToString))
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
                        If ModificationTypeID.Contains(ObjAssemblyMonitorModStatus.MonitorTypeID) Then
                            If ObjAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriodList.Count > 0 Then
                                If (ObjAssemblyMonitorModStatus.IsApplicable = True) And (Not (ObjAssemblyMonitorModStatus.MonitorTypeID = 1 And ObjAssemblyMonitorModStatus.IsCompleted = True)) Then
                                    ATAChapter = ObjAssemblyMonitorModStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorModStatus.ATANomenclature
                                    ATACode = ObjAssemblyMonitorModStatus.ATACode
                                    Description = ObjAssemblyMonitorModStatus.Description & vbCrLf & ObjAssemblyMonitorModStatus.Number & vbCrLf & IIf(ObjAssemblyMonitorModStatus.Reference <> "", "Task Code/Ref. : ", "") & ObjAssemblyMonitorModStatus.Reference
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
                                    If ObjAssemblyStatus.InstallationRemark = "" And ObjAssemblyMonitorModStatus.DoneRemark = "" Then Remark = "----------"
                                    Code = ObjAssemblyMonitorModStatus.ModelMonitorModCode

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

                                    Dim tmpModelMonitorMod As ModelMonitorMod = ModelMonitorMod.GetModelMonitorMod(ObjAssemblyMonitorModStatus.ModelMonitorModID)
                                    ModelEstimatedManHrs = tmpModelMonitorMod.RequiredManHours

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
											'Added PeriodID=11 By Vikrant For ALL 21062012
											'If ObjAssemblyMonitorModStatusPeriod.PeriodID = 3 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 4 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 5 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 6 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 7 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 8 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 9 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 12 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 13 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 14 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 15 Or ObjAssemblyMonitorModStatusPeriod.PeriodID = 11 Then
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
                                    StatusID = ObjAssemblyMonitorModStatus.ID  'Added by Saylee on 6-May-2013 for ALL06052013-1
                                    If IsPreviewClicked Then 'Added by Saylee on 6-May-2013 for ALL06052013-1
                                        mnWOListForDueJobs = nWOListForDueJobs.GetWOListForDueJobs(StatusID)
                                        If mnWOListForDueJobs.Count > 0 Then
                                            nWONumber = mnWOListForDueJobs(0).WONumber + vbCrLf + mnWOListForDueJobs(0).WODateFormatted
                                        Else
                                            nWONumber = ""
                                        End If
                                    End If
                                    Zone = ObjAssemblyMonitorModStatus.Zone
                                    Area = ObjAssemblyMonitorModStatus.Area
                                    IsRII = ObjAssemblyMonitorModStatus.IsRII
                                    'If ObjAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriodList.Count > 0 Then
                                    ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, , AssemblySerialNo, ATAChapter, , , Position, , MonitorTypeCode, Note, Remark, Description, _
                                       , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, _
                                       SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, AssemblyTypeID, MaintenanceEvent, ATACode, , , , , , , , , , , , , , , , , , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, ModelEstimatedManHours:=ModelEstimatedManHrs, WONumber:=nWONumber, Zone:=Zone, Area:=Area, IsRII:=IsRII, StatusID:=StatusID.ToString))
                                End If
                            End If
                        End If
                    Next
                    For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
                        For Each ObjCompMonitorModStatus In ObjCompStatus.CompMonitorModStatusList
                            If ObjCompMonitorModStatus.CompMonitorModStatusPeriodList.Count > 0 Then
                                If (ObjCompMonitorModStatus.IsApplicable = True) And (Not (ObjCompMonitorModStatus.MonitorTypeID = 1 And ObjCompMonitorModStatus.IsCompleted)) Then
                                    ATAChapter = ObjCompMonitorModStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorModStatus.ATANomenclature
                                    ATACode = ObjCompMonitorModStatus.ATACode
                                    Description = ObjCompMonitorModStatus.Description & vbCrLf & ObjCompMonitorModStatus.Number & vbCrLf & IIf(ObjCompMonitorModStatus.Reference <> "", "Task Code/Ref. : ", "") & ObjCompMonitorModStatus.Reference
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
                                    If ObjCompStatus.InstallationRemark = "" And ObjCompMonitorModStatus.DoneRemark = "" Then Remark = "----------"
                                    Code = ObjCompMonitorModStatus.PartMonitorModCode

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

                                    Dim tmpPartMonitorMod As PartMonitorMod = PartMonitorMod.GetPartMonitorMod(ObjCompMonitorModStatus.PartMonitorModID)
                                    ModelEstimatedManHrs = tmpPartMonitorMod.RequiredManHours

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
											'Added PeriodID=11 By Vikrant For ALL 21062012
											'If ObjCompMonitorModStatusPeriod.PeriodID = 3 Or ObjCompMonitorModStatusPeriod.PeriodID = 4 Or ObjCompMonitorModStatusPeriod.PeriodID = 5 Or ObjCompMonitorModStatusPeriod.PeriodID = 6 Or ObjCompMonitorModStatusPeriod.PeriodID = 7 Or ObjCompMonitorModStatusPeriod.PeriodID = 8 Or ObjCompMonitorModStatusPeriod.PeriodID = 9 Or ObjCompMonitorModStatusPeriod.PeriodID = 12 Or ObjCompMonitorModStatusPeriod.PeriodID = 13 Or ObjCompMonitorModStatusPeriod.PeriodID = 14 Or ObjCompMonitorModStatusPeriod.PeriodID = 15 Or ObjCompMonitorModStatusPeriod.PeriodID = 11 Then
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
                                    'Rajnish 08-08-2008
                                    RequiredManHours = ObjCompMonitorModStatus.RequiredManHours
                                    Customer = ObjMachine.Customer

                                    Note = ObjCompMonitorModStatus.Notes
                                    MaintenanceEvent = ObjCompMonitorModStatus.Type
                                    'Added By Saylee on 04-08-2008
                                    ExtensionDate = ObjCompMonitorModStatus.ExtensionDate
                                    ApprovalRemark = ObjCompMonitorModStatus.ApprovalRemark
                                    StatusID = ObjCompMonitorModStatus.ID  'Added by Saylee on 6-May-2013 for ALL06052013-1
                                    If IsPreviewClicked Then 'Added by Saylee on 6-May-2013 for ALL06052013-1
                                        mnWOListForDueJobs = nWOListForDueJobs.GetWOListForDueJobs(StatusID)
                                        If mnWOListForDueJobs.Count > 0 Then
                                            nWONumber = mnWOListForDueJobs(0).WONumber + vbCrLf + mnWOListForDueJobs(0).WODateFormatted
                                        Else
                                            nWONumber = ""
                                        End If
                                    End If
                                    Zone = ""
                                    Area = ""
                                    IsRII = False
                                    'If ObjCompMonitorModStatus.CompMonitorModStatusPeriodList.Count > 0 Then
                                    ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, MaintenanceEvent, AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, , MonitorTypeCode, Note, Remark, Description, _
                                                          , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, _
                                                          AssemblyTypeID, MaintenanceEvent, ATACode, , , , , , , , , , , , , , , , , , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, ModelEstimatedManHours:=ModelEstimatedManHrs, WONumber:=nWONumber, Zone:=Zone, Area:=Area, IsRII:=IsRII, StatusID:=StatusID.ToString))
                                End If
                            End If
                        Next
                    Next
                Next
            Next
        End If

        'Else
        '    ReportMaintenanceDetails.Add(mMachineList, Report, , , True)
        'End If
        Return ReportMaintenanceDetails
    End Function
    Private Sub SetReport(Optional ByVal IsPreviewClicked As Boolean = False)
        Dim mTaskCardListByMaintenanceActivity As TaskCardListByMaintenanceActivity 'ALL14122015
        ReportMaintenanceDetails = New ReportMaintenanceDetailList
        ReportStatusList = New rptStatusList
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsReportMaintenanceDetail

        Dim mCompanyDetail As New CompanyDetail
        Dim searchstr As String = ""

        SetValues()
        mTaskCardListByMaintenanceActivity = TaskCardListByMaintenanceActivity.GetTaskCardList(Guid.Empty.ToString) 'ALL14122015
        ReportDetail(IsPreviewClicked)

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

        searchstr = searchstr '& ", " & "As On Date:" & New SmartDate(txtFromDate.Value.ToString).FormattedText
        Dim searchstr1 As String
        'Dim mPerDayLimit As PerDayLimit
        'If rbdSpecifyValues.Checked = True Then
        '    For Each mPerDayLimit In mPerDayLimits
        '        If CDec(Val(mPerDayLimit.PeriodLimit)) >= 0 Then
        '            If searchstr1 = "" Then
        '                searchstr1 = "Estimated Due Date as" & " " & searchstr1 & " " & mPerDayLimit.PeriodLimit & " " & mPerDayLimit.PeriodName
        '            Else
        '                searchstr1 = searchstr1 & ", " & mPerDayLimit.PeriodLimit & " " & mPerDayLimit.PeriodName
        '            End If
        '        End If
        '    Next
        '    searchstr1 = searchstr1 & " per Day "
        'Else
        '    If CDec(Val(txtAvgMnths.Text)).ToString <> "" Then
        '        searchstr1 = "Estimated Due Date as Per Average of Last" & " " & CDec(Val(txtAvgMnths.Text)).ToString & " Months"
        '    Else
        '        searchstr1 = ""
        '    End If
        'End If
        '===========================================
        Dim ReportName As String
        Dim rptDueDetail As CrystalDecisions.CrystalReports.Engine.ReportClass
        'If DueType = 1 Then

        '    If Not cmbAircraft.SelectedItem.ToString = "(All)" Then
        '        rptMachineCertificates = MachineCertificateList.GetMachineCertificateList(New Guid(cmbAircraft.SelectedValue.ToString), AsonDate)
        '        If rptMachineCertificates.Count = 0 Then
        '            If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "Indamer") Or (AppSettings("ClientCode") = "Heligo") Or (AppSettings("ClientCode") = "UHPL")) Then 'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
        '                rptDueDetail = New crDueReportDetailLandscapePerAircraftIndamar 'This change is applied to Indamar and Heligo
        '            Else
        '                rptDueDetail = New crDueReportDetailLandscapePerAircraft
        '            End If

        '        Else
        '            If (Not AppSettings("ClientCode") Is Nothing) AndAlso ((AppSettings("ClientCode") = "Indamer") Or (AppSettings("ClientCode") = "Heligo") Or (AppSettings("ClientCode") = "UHPL")) Then 'Client Code UHPL Added By Vikrant On 26-Feb-2013 For UHPL26022013
        '                rptDueDetail = New crDueReportDetailLandscapePerAircraftCertificatesIndamar 'This change is applied to Indamar and Heligo
        '            Else
        '                rptDueDetail = New crDueReportDetailLandscapePerAircraftCertificates
        '            End If

        '        End If
        '    Else
        '        rptDueDetail = New crDueReportDetailLandscape
        '    End If
        '    ReportName = "Maintenance Status Report"

        '    If rbEngineeringOrder.Checked Then
        '        rptDueDetail = New crMaintenanceAdvicePPCTAAL
        '        ReportName = "WORK ORDER LIST"
        '    End If
        'Else


        If cmbFormat.SelectedIndex = 0 Then
            If cmbSordBy.SelectedIndex = 0 Then
                rptDueDetail = New crMaintenanceAdvicePPCPortrait
            Else
                rptDueDetail = New crMaintenanceAdviceFromQCDetail
            End If

        Else
            If cmbSordBy.SelectedIndex = 0 Then
                rptDueDetail = New crMaintenanceAdvicePPC
            Else
                rptDueDetail = New crMaintenanceAdvice
            End If
        End If
        ReportName = "MAINTENANCE ADVICE FROM CAMO" 'Changed By Utkarsh On 31-12-2010 From 'QC Cell' to 'CAMO'

        If rbEngineeringOrder.Checked Then
            rptDueDetail = New crMaintenanceAdvicePPCTAAL
            ReportName = "WORK ORDER LIST"
        End If
        'End If
        '-------------------------------------------
        Dim x As String
        If mloglist.Count > 0 Then
            x = mloglist(0).LogDate.ToShortDateString
        Else
            x = txtDate.Text.Trim
        End If

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
    mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
    mCompanyDetail.WebSite, ReportName, searchstr, searchstr1, Assembly1, cmbSordBy.SelectedIndex.ToString, New SmartDate(x).FormattedText, AppSettings("Product Version"), AppSettings("SINote"), chkTaskCard.Checked.ToString, "", "", "", AppSettings("Logo"))
        If ReportMaintenanceDetails.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1107)
        End If

        '11-Sep-2008-------------------------------
        If Not mIsPreview Then
            ds.Clear()
            da.Fill(ds, ReportMaintenanceDetails)

            ''Added by Saylee on 25-Sep-2008 for showing Due Certificates
            'If DueType = 1 Then
            '    If Not cmbAircraft.SelectedItem.ToString = "(All)" Then
            '        If rptMachineCertificates.Count <> 0 Then da.Fill(ds, rptMachineCertificates)
            '    End If
            'End If

            '===================================
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            da.Fill(ds, mrptImage)
            da.Fill(ds, Report)
            da.Fill(ds, ReportStatusList)
            da.Fill(ds, mTaskCardListByMaintenanceActivity) 'ALL14122015
            rptDueDetail.SetDataSource(ds)
            Session("CrystalReport") = rptDueDetail

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
            MarkLog(Util.Action.Print, "MaintenanceAdviceFromQC", EventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
            'ResetValues()
            'Saving Periods Limits
            Try
                SetGridObject()
                mDueLimits = CType(mDueLimits.Save, DueLimits)
                Session("mDueLimits") = mDueLimits
                'DataFieldBind()
                Controltovisibility()
            Catch ex As Exception
                '
            End Try
        Else
            Dim reportmaintdetailslist As List(Of ReportMaintenanceDetail) = New List(Of ReportMaintenanceDetail)

            reportmaintdetailslist = (From c As ReportMaintenanceDetail In ReportMaintenanceDetails.AsParallel
                                     Order By c.MinimumRemainingValue, c.RegNo, c.AssemblyType, c.Model, c.AssemblySerialNo, c.MaintenanceEvent, c.Description, c.PartNo
                                     Select c).ToList
            Session("reportmaintdetailslist") = reportmaintdetailslist
            Session("ReportMaintenanceDetails") = ReportMaintenanceDetails
            'Added By Vikrant on 14-Jun-2018 For ALL14062018
            Session("AsOnDateForWOCreation") = txtDate.Text
            Session("MachineIDForWOCreation") = cmbAircraft.SelectedValue.ToString
            'End
            Session("wfSearchCriteriaForMaintenanceAdviceFromQC") = "wfSearchCriteriaForMaintenanceAdviceFromQC"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenScript", "openledgersame('wfDueResult_Ajax.aspx?');", True)
        End If
    End Sub
    Private Sub Controltovisibility()
        'If DueType = 1 Then
        '    lblSortBy.Visible = False
        '    cmbSordBy.Visible = False
        '    lbltitle.Text = "Search criteria for Due"
        '    lblFormat.Visible = False
        '    cmbFormat.Visible = False
        'Else
        'lblSortBy.Visible = True
        'cmbSordBy.Visible = True
        'lblStep6.Visible = False
        'Label2.Visible = False
        'rbdAvrageMonths.Visible = False
        'rbdSpecifyValues.Visible = False
        'lblAvgMnths.Visible = False
        'txtAvgMnths.Visible = False
        'lblMonths.Visible = False
        'lblInfo.Visible = False
        'lblAvgMnths1.Visible = False
        'gdPerDayLimit.Visible = False
        'lblStep7.Text = "Step V. Display Report"
        'lbltitle.Text = "Search criteria for Maintenance Advice From QC"
        'lblFormat.Visible = True
        'cmbFormat.Visible = True
        'End If
    End Sub
#End Region

#Region " Data Binding "
    Public Sub DataFieldBind()
        mDueLimits = DueLimits.GetDueLimits(New Guid(cmbAircraft.SelectedValue.ToString))
        dgDuePeriodLimits.DataSource = mDueLimits
        Session("mDueLimits") = mDueLimits
        upnlGrid.Update()
        mPerDayLimits = PerDayLimits.GetPerDayLimits(New Guid(cmbAircraft.SelectedValue.ToString))
        Session("mPerDayLimits") = mPerDayLimits
        dgDuePeriodLimits.DataBind()
    End Sub
    Public Sub SetComboOfMachine(ByVal AsonDate As String)
        mMachineNameValueList = MachineNameValueList.GetMachineList(AsonDate, , , , , , , True, "(SELECT)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList ") = mMachineNameValueList
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
    Private Sub ControlVisibility()
        Dim txtLimit As TextBox
        Dim i As Int32
        For i = 0 To Me.dgDuePeriodLimits.Rows.Count - 1
            txtLimit = CType(Me.dgDuePeriodLimits.Rows(i).FindControl("txtLimit"), TextBox)
            If rbdDueLimits.Checked Then
                txtLimit.Enabled = True
            ElseIf rbdPercent.Checked Then
                txtLimit.Enabled = False
            End If
        Next i
    End Sub
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("Sender") = "" Then
            DueType = Request.QueryString("DueType")
            Session("DueType") = DueType
            Session("MiddleFrame") = "wfSearchCriteriaForMaintenanceAdviceFromQC_Ajax.aspx?DueType=" & DueType
            ResetValues()
            FillTypeCombo()
            ''SetFocus(txtFromDate)
            cmbAssembly.Enabled = False
            txtDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            AOnDate = txtDate.Text
            SetComboOfMachine(AOnDate)
            setFocus(cmbAircraft)
            DataFieldBind()
            Report = 1
            rbForCustomer.Checked = True
        End If
        'Controltovisibility()
        SetSession()
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        If IsValid = True Then
            Display()
            SetValues()
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid = True Then
            SetReport()
        Else
            upnlValidations.Update()
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        mMachineList = Nothing
        mDueLimits = Nothing
        mAssemblyStatusList = Nothing
        'Added By Saylee on 20-Feb-2009
        mServiceTypeList = Nothing
        mInspectionTypeList = Nothing
        mModificationTypeList = Nothing
        '=============================
        Session("MiddleFrame") = ""
        ClearAll()
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub txtDate_TextChanged(sender As Object, e As System.EventArgs) Handles txtDate.TextChanged
        AOdate = txtDate.Text.Trim
        If AOnDate = AOdate Then
        Else
            Dim tmpdate As Date
            If Date.TryParse(txtDate.Text.Trim, tmpdate) Then
                SetComboOfMachine(AOdate)
                cmbAssembly.Enabled = False
                mAssemblyStatusList = Nothing
                Session("mAssemblyStatusList") = mAssemblyStatusList
                cmbAssembly.ClearSelection()
                cmbAssembly.DataSource = mAssemblyStatusList
                cmbAssembly.DataBind()
                DataFieldBind()
                Controltovisibility()
            End If
        End If
    End Sub
    Private Sub rbdPercent_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbdPercent.CheckedChanged
        txtPercentage.Enabled = True
        txtPercentage.Text = "10"
        mDueLimits.SetPercentageWise(True, CDec(Val(txtPercentage.Text)))
        Dim txtLimit As TextBox
        Dim i As Int32
        For i = 0 To Me.dgDuePeriodLimits.Rows.Count - 1
            txtLimit = CType(Me.dgDuePeriodLimits.Rows(i).FindControl("txtLimit"), TextBox)
            txtLimit.Enabled = False
        Next i
    End Sub
    Private Sub rbdDueLimits_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbdDueLimits.CheckedChanged
        txtPercentage.Enabled = False
        txtPercentage.Text = ""
        mDueLimits.UnSetPercentageWise()
        Dim txtLimit As TextBox
        Dim i As Int32
        For i = 0 To Me.dgDuePeriodLimits.Rows.Count - 1
            txtLimit = CType(Me.dgDuePeriodLimits.Rows(i).FindControl("txtLimit"), TextBox)
            txtLimit.Enabled = True
        Next i
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        If cmbAircraft.SelectedIndex = 0 Then
            cmbAssembly.Enabled = False
            AircraftIndex = cmbAircraft.SelectedIndex
            Session("AircraftIndex") = AircraftIndex

            cmbAssembly.SelectedIndex = 0

        Else
            cmbAssembly.Enabled = True

            ''MachineName = cmbAircraft.SelectedValue.ToString
            ''mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(AsonDate, MachineName, , , , , , , , , , True, , , , , , , , , , , , , , , , , , , True).Item(1), MachineInfo).AssemblyStatusList
            mAssemblyStatusList = CType(MachineList.GetMachineListWithInstallation(txtDate.Text.Trim, cmbAircraft.SelectedValue, , , , , , , , , , True, , , , , , , , , , , , , , , , , , , True).Item(1), MachineInfo).AssemblyStatusList

            cmbAssembly.DataSource = mAssemblyStatusList
            Session("mAssemblyStatusList") = mAssemblyStatusList
            cmbAssembly.DataBind()
            AircraftIndex = cmbAircraft.SelectedIndex
            Session("AircraftIndex") = AircraftIndex
        End If
        If cmbAircraft.Enabled = True Then
            setFocus(cmbAircraft)
        End If
        DataFieldBind()
        Controltovisibility()
    End Sub
    Private Sub btnPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        mIsPreview = True
        If IsValid = True Then
            SetReport(IsPreviewClicked:=True)
        Else
            upnlValidations.Update()
        End If
    End Sub
#End Region
End Class