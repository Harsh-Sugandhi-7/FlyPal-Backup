
'Created By : Saylee
'Dated:     : 28-Dec-2015



Imports System.Linq
Imports System.Collections.Generic

Public Class wfSearchCriteriaForDueRemovedAssembly_Ajax
    Inherits System.Web.UI.Page

#Region "Enumeration"
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
    Dim Periodcount As Integer
    Dim MachineName As String
    Dim AsonDate As String
    Dim Type As Integer = 1
    Dim AssemblyID As Guid
    Dim Count As Integer
    Dim mDueLimit As DueLimit

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

    Dim mRemovedAssemblyListForDue As RemovedAssemblyListForCombo
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    Dim IsSpareAssembly As Boolean = False

#End Region

#Region " Helper Methods "
    Private Sub addAttributes()
        txtForecastingLimit.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtForecastingLimit').value,event)")
    End Sub
    Private Sub SetGridObject()
        Dim txtLimit As TextBox
        Dim i As Int32
        For i = 0 To Me.gdvDuePeriodLimits.Rows.Count - 1
            txtLimit = CType(Me.gdvDuePeriodLimits.Rows(i).FindControl("txtLimit"), TextBox)
            'mDueLimits.Item(i).PeriodLimit = CDec(Val(Trim(txtLimit.Text))) 'Commented by Saylee on 12-Nov-2012
            mDueLimits.Item(i).PeriodLimit = Trim(txtLimit.Text) 'Added by Saylee on 12-Nov-2012
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
        DueType = Session("DueType")

        'Added by Saylee on 12-Feb-2009
        mAssemblyList = CType(Session("mAssemblyList"), AssemblyList)
        mServiceTypeList = CType(Session("mServiceTypeList"), PartMonitorServiceTypeList)
        mInspectionTypeList = CType(Session("mInspectionTypeList"), ModelMonitorInspTypeList)
        mModificationTypeList = CType(Session("mModificationTypeList"), ModelMonitorModTypeList)

        mRemovedAssemblyListForDue = Session("mRemovedAssemblyListForDue")
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
    End Sub
    Private Sub SetSession()
        Session("mMachineList") = mMachineList
        Session("mDueLimits") = mDueLimits
        Session("mPerDayLimits") = mPerDayLimits
        Session("AOnDate") = AOnDate
        Session("Report") = Report
        Session("Type") = Type
        Session("DueType") = DueType

        'Added by Saylee on 12-Feb-2009
        Session("mAssemblyList") = mAssemblyList
        Session("SerIndex") = SerIndex
        Session("InspIndex") = InspIndex
        Session("ModIndex") = ModIndex
        Session("mServiceTypeList") = mServiceTypeList
        Session("mInspectionTypeList") = mInspectionTypeList
        Session("mModificationTypeList") = mModificationTypeList

        Session("mRemovedAssemblyListForDue") = mRemovedAssemblyListForDue
    End Sub
    Private Sub ClearAll()
        DueType = Session("DueType")
        If Session("MiddleFrame") <> "wfSearchCriteriaForDueRemovedComp_Ajax.aspx?DueType=" & DueType Then
            Session.Remove("mMachineList")
            Session.Remove("mDueLimits")
            Session.Remove("mPerDayLimits")
            Session.Remove("AOnDate")
            Session.Remove("Report")
            Session.Remove("Type")

            'Added by Saylee on 12-Feb-2009
            Session.Remove("mAssemblyList")
            Session.Remove("SerIndex")
            Session.Remove("InspIndex")
            Session.Remove("ModIndex")

            Session.Remove("mRemovedAssemblyListForDue")
            Session.Remove("mServiceTypeList")
            Session.Remove("mInspectionTypeList")
            Session.Remove("mModificationTypeList")
        End If
        mIsExcel = False
    End Sub
    Private Overloads Sub setFocus(ByVal cntrl As WebControl)
        If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
        cntrl.Focus()
    End Sub
    Private Sub Display()
        lblDateRangeFrom.Visible = True
        lblPercent.Visible = (DueType = 1)
        lblAssembly1.Visible = True
        ''lblType1.Visible = True
        upnlSearchingCriteria.Update()
    End Sub
    Private Sub SetValues()
        If (cmbAssembly.SelectedItem.Text = "(All)") Or (cmbAssembly.SelectedItem.Text = "(SELECT)") Then
            AssemblyName = "{00000000-0000-0000-0000-000000000000}"
            Assembly1 = ""
            lblAssembly1.Text = ""
            IsSpareAssembly = False
        Else
            AssemblyType = mRemovedAssemblyListForDue(New Guid(cmbAssembly.SelectedValue.ToString)).AssemblyTypeName
            AssemblyName = mRemovedAssemblyListForDue(New Guid(cmbAssembly.SelectedValue.ToString)).AssemblyID.ToString
            Assembly1 = mRemovedAssemblyListForDue(New Guid(cmbAssembly.SelectedValue.ToString)).ModelSerialNo
            lblAssembly1.Text = "Assembly Name : " & Assembly1  'Added Code
            IsSpareAssembly = mRemovedAssemblyListForDue(New Guid(cmbAssembly.SelectedValue.ToString)).IsSpareAssembly

        End If
        If Not IsDate(txtFromDate.Text.Trim) Then
            AsonDate = ""
        Else
            AsonDate = txtFromDate.Text.Trim
        End If

        If AsonDate <> "" Then
            lblDateRangeFrom.Text = "As On Date : " & txtFromDate.Text.Trim
        Else
            lblDateRangeFrom.Text = "As On Date : " & "All"
        End If

        percent = txtPercentage.Text
        lblPercent.Text = "Percent : " & IIf(percent <> "", percent, "All")
        ''lblType1.Text = "Type : " & IIf(TypeName <> "", TypeName, "All")

        'Set Service/Inspection/Directive checkbox list values
        'Service
        'If chkService.Checked Then
        'IsSerSelect = True
        ServiceTypeID = (From c As System.Web.UI.WebControls.ListItem In ListServiceType.Items
                     Where c.Selected = True
                     Select CInt(c.Value)).ToArray
        If ServiceTypeID.Length > 0 Then
            IsSerSelect = True
        End If
        'End If
        'Inspection
        'If chkInspection.Checked Then
        'IsInsSelect = True

        InspectionTypeID = (From c As System.Web.UI.WebControls.ListItem In ListInspectionType.Items
                     Where c.Selected = True
                     Select CInt(c.Value)).ToArray
        If InspectionTypeID.Length > 0 Then
            IsInsSelect = True
        End If
        'End If
        'Directive
        'If chkDirective.Checked Then
        'IsModSelect = True
        ModificationTypeID = (From c As System.Web.UI.WebControls.ListItem In ListDirectiveType.Items
                     Where c.Selected = True
                     Select CInt(c.Value)).ToArray
        If InspectionTypeID.Length > 0 Then
            IsModSelect = True
        End If
        'End If
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
        ' Format = IIf(chkwithWONo.Checked, cmbFormat.SelectedItem.Text & " : " & chkwithWONo.Text, cmbFormat.SelectedItem.Text)
        mEventLogDetails = lblDateRangeFrom.Text + ", " + lblAssembly1.Text + ", " + DueLimits
    End Sub
    Private Sub ResetValues()
        MachineName = "{00000000-0000-0000-0000-000000000000}"
        'CNDC
        'txtFromDate.Value = AsonDate
        If AsonDate <> "" Then
            txtFromDate.Text = Format(AsonDate, AppSettings("DateFormat"))
        End If
        AsonDate = ""
        IsSerSelect = False
        IsInsSelect = False
        IsModSelect = False
        ServiceTypeID(0) = 0
        InspectionTypeID(0) = 0
        ModificationTypeID(0) = 0
        AssemblyName = "{00000000-0000-0000-0000-000000000000}"
        btnDisplay.Enabled = True
    End Sub
    Public Function ReportDetail(IsExcel As Boolean) As ReportMaintenanceDetailList
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

            mMachineList = MachineList.GetMachineListDueMonitoringStatusForRemoveAssembly(AsonDate, mDueLimits, , AssemblyName, 0, , mPerDayLimits, , IsSerSelect, IsInsSelect, IsModSelect, , , , IsSerSelect, IsInsSelect, IsModSelect, Val(txtForecastingLimit.Text), True, SkipIsForInventoryAircarft:=True, IsSpareAssembly:=IsSpareAssembly)

            Dim LHLabel2 As String = ""
            Dim LHData2 As String = ""

            'If Not cmbAircraft.SelectedItem.ToString = "(All)" Then
            '    For Each ObjMachine In mMachineList
            '        For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
            '            Periodcount = ObjAssemblyStatus.AssemblyStatusPeriodList.Count()
            '            LHLabel2 = ""
            '            LHData2 = ""
            '            For Count = 0 To Periodcount - 1
            '                If ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID <> 2 Then
            '                    LHLabel2 = CType(IIf(LHLabel2 = "", LHLabel2, LHLabel2 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodName
            '                    LHData2 = CType(IIf(LHData2 = "", LHData2, LHData2 + vbNewLine), String) + ObjAssemblyStatus.AssemblyStatusPeriodList(ObjAssemblyStatus.AssemblyStatusPeriodList(Count).PeriodID, "").AssemblyCurrentValue
            '                End If
            '            Next
            '            AssemblyID = ObjAssemblyStatus.AssemblyID
            '        Next
            '    Next
            'End If

            'If Not cmbAircraft.SelectedItem.ToString = "(All)" Then
            '    mtmpMachineList = tmpMachineList.GetMachineList(, Aircraft, , , , , True, AsonDate)
            '    Dim mOtherPeriodExists As String = "False"

            '    For i As Integer = 0 To mtmpMachineList.Count - 1
            '        If mtmpMachineList(i).AllPeriods <> "" Then
            '            mOtherPeriodExists = "True"
            '            Exit For
            '        End If
            '    Next

            '    For i As Integer = 0 To mtmpMachineList.Count - 1
            '        searchstr7 = mtmpMachineList(i).Owner.ToString  ' Changed By Utkarsh On 11-Apr-2011 '"Owner/Operator :- " +
            '        ReportStatusList.Add(New rptStatus(mtmpMachineList(i).ID.ToString, 1, , , , , mtmpMachineList(i).TSO, , mtmpMachineList(i).CSO, , , , , , , , , mtmpMachineList(i).Cycles, mtmpMachineList(i).AllPeriods.Replace("<BR>", vbCrLf), mOtherPeriodExists, Year(txtFromDate.Text).ToString, , mtmpMachineList(i).RegNo, mtmpMachineList(i).ModelName, mtmpMachineList(i).Type, mtmpMachineList(i).SerialNo, mtmpMachineList(i).ManufacturerName, , mtmpMachineList(i).ManufacturingDate, mtmpMachineList(i).Hours, mtmpMachineList(i).Landings))
            '        Session("AircraftAsOnDate") = mtmpMachineList(i).ManufacturingDateFormatted
            '    Next
            'End If

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
                                        AssemblySerialNo = ObjAssemblyStatus.SerialNo & IIf(IsExcel, Chr(10), vbCrLf)
                                        Position = ""
                                        MonitorTypeCode = ObjAssemblyMonitorServiceStatus.Code
                                        EstimatedDate = ObjAssemblyMonitorServiceStatus.EstimatedDateFormatted
                                        MinimumRemainingValue = ObjAssemblyMonitorServiceStatus.MinimumRemainingValue
                                        AssemblyTypeID = ObjAssemblyStatus.AssemblyTypeID
                                        StatusMasterID = ObjAssemblyMonitorServiceStatus.ModelMonitorServiceID  '11-Sep-2008
                                        DueStatus = ObjAssemblyMonitorServiceStatus.DueStatus
                                        DocumentTypeForID = 0
                                        '  Remark = ObjAssemblyMonitorServiceStatus.DoneRemark  'Added By Saylee on 20-08-2008
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
                                        RequiredManHours = ObjAssemblyMonitorServiceStatus.RequiredManHours
                                        Customer = ObjMachine.Customer
                                        AssemblyType = ObjAssemblyStatus.AssemblyType
                                        'Commented and Added by Saylee on 10-Oct-2013 for ALL10102013
                                        'MaintenanceEvent = ObjAssemblyMonitorServiceStatus.Type
                                        If ObjAssemblyMonitorServiceStatus.Reference <> "" Then
                                            MaintenanceEvent = ObjAssemblyMonitorServiceStatus.Type & " (" & ObjAssemblyMonitorServiceStatus.MonitorType & ")" & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorServiceStatus.Reference
                                        Else
                                            MaintenanceEvent = ObjAssemblyMonitorServiceStatus.Type & " (" & ObjAssemblyMonitorServiceStatus.MonitorType & ")"
                                        End If
                                        'Added by Saylee 04-08-2008
                                        ExtensionDate = ObjAssemblyMonitorServiceStatus.ExtensionDate
                                        ApprovalRemark = ObjAssemblyMonitorServiceStatus.ApprovalRemark
                                        StatusID = ObjAssemblyMonitorServiceStatus.ID  'Added by Saylee on 6-May-2013 for ALL06052013-1


                                        'If ObjAssemblyMonitorServiceStatus.AssemblyMonitorServiceStatusPeriodList.Count > 0 Then
                                        ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, , AssemblySerialNo, ATAChapter, , , Position, ObjAssemblyMonitorServiceStatus.MonitorType, MonitorTypeCode, Note, Remark, Description, _
                  , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, _
                  SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, AssemblyTypeID, MaintenanceEvent, , , , , , , , , , , , , , , , , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, _
                  AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , , StatusID.ToString _
                  , , , , , , , DueStatus, , , nWONumber))
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
                                            AssemblySerialNo = ObjAssemblyStatus.SerialNo & IIf(IsExcel, Chr(10), vbCrLf)
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
                                            RequiredManHours = ObjCompMonitorServiceStatus.RequiredManHours
                                            Customer = ObjMachine.Customer
                                            Note = ObjCompMonitorServiceStatus.Notes
                                            'Commented and Added by Saylee on 10-Oct-2013 for ALL10102013
                                            'MaintenanceEvent = ObjCompMonitorServiceStatus.Type
                                            If ObjCompMonitorServiceStatus.Reference <> "" Then
                                                MaintenanceEvent = ObjCompMonitorServiceStatus.Type & " (" & ObjCompMonitorServiceStatus.MonitorType & ")" & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorServiceStatus.Reference
                                            Else
                                                MaintenanceEvent = ObjCompMonitorServiceStatus.Type & " (" & ObjCompMonitorServiceStatus.MonitorType & ")"
                                            End If

                                            'Added by Saylee 04-08-2008
                                            ExtensionDate = ObjCompMonitorServiceStatus.ExtensionDate
                                            ApprovalRemark = ObjCompMonitorServiceStatus.ApprovalRemark

                                            StatusID = ObjCompMonitorServiceStatus.ID  'Added by Saylee on 6-May-2013 for ALL06052013-1


                                            'If ObjCompMonitorServiceStatus.CompMonitorServiceStatusPeriodList.Count > 0 Then
                                            ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, MaintenanceEvent, AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, ObjCompMonitorServiceStatus.MonitorType, MonitorTypeCode, Note, Remark, Description, _
                                                                  , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, _
                                                                  AssemblyTypeID, MaintenanceEvent, , , , , , , , , , , , , , , , , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , , StatusID.ToString, , , , , , , DueStatus, , , nWONumber))
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
                                        AssemblySerialNo = ObjAssemblyStatus.SerialNo & IIf(IsExcel, Chr(10), vbCrLf)
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
                                        RequiredManHours = ObjAssemblyMonitorInspStatus.RequiredManHours
                                        Customer = ObjMachine.Customer
                                        Note = ObjAssemblyMonitorInspStatus.Notes
                                        'Commented and Added by Saylee on 10-Oct-2013 for ALL10102013
                                        'MaintenanceEvent = ObjAssemblyMonitorInspStatus.Type
                                        If ObjAssemblyMonitorInspStatus.Reference <> "" Then
                                            MaintenanceEvent = ObjAssemblyMonitorInspStatus.Type & " (" & ObjAssemblyMonitorInspStatus.MonitorType & ")" & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorInspStatus.Reference
                                        Else
                                            MaintenanceEvent = ObjAssemblyMonitorInspStatus.Type & " (" & ObjAssemblyMonitorInspStatus.MonitorType & ")"
                                        End If


                                        'Added by Saylee 04-08-2008
                                        ExtensionDate = ObjAssemblyMonitorInspStatus.ExtensionDate
                                        ApprovalRemark = ObjAssemblyMonitorInspStatus.ApprovalRemark

                                        StatusID = ObjAssemblyMonitorInspStatus.ID 'Added by Saylee on 6-May-2013 for ALL06052013-1


                                        'If ObjAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriodList.Count > 0 Then
                                        ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, , AssemblySerialNo, ATAChapter, , , Position, ObjAssemblyMonitorInspStatus.MonitorType, MonitorTypeCode, Note, Remark, Description, _
                                           , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, _
                                           SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, _
                                           AssemblyTypeID, MaintenanceEvent, , , , , , , , , , , , , , , , , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , , StatusID.ToString, , , , , , , DueStatus, , , nWONumber))
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
                                            AssemblySerialNo = ObjAssemblyStatus.SerialNo & IIf(IsExcel, Chr(10), vbCrLf)
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
                                            RequiredManHours = ObjCompMonitorInspStatus.RequiredManHours
                                            Customer = ObjMachine.Customer

                                            Note = ObjCompMonitorInspStatus.Notes

                                            'Commented and Added by Saylee on 10-Oct-2013 for ALL10102013
                                            'MaintenanceEvent = ObjCompMonitorInspStatus.Type
                                            If ObjCompMonitorInspStatus.Reference <> "" Then
                                                MaintenanceEvent = ObjCompMonitorInspStatus.Type & " (" & ObjCompMonitorInspStatus.MonitorType & ")" & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorInspStatus.Reference
                                            Else
                                                MaintenanceEvent = ObjCompMonitorInspStatus.Type & " (" & ObjCompMonitorInspStatus.MonitorType & ")"
                                            End If

                                            '*********************************
                                            'Added By Saylee on 04-08-2008
                                            ExtensionDate = ObjCompMonitorInspStatus.ExtensionDate
                                            ApprovalRemark = ObjCompMonitorInspStatus.ApprovalRemark

                                            StatusID = ObjCompMonitorInspStatus.ID 'Added by Saylee on 6-May-2013 for ALL06052013-1


                                            'If ObjCompMonitorInspStatus.CompMonitorInspStatusPeriodList.Count > 0 Then
                                            ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, MaintenanceEvent, AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, ObjCompMonitorInspStatus.MonitorType, MonitorTypeCode, Note, Remark, Description, _
                                                                 , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, _
                                                                 AssemblyTypeID, MaintenanceEvent, , , , , , , , , , , , , , , , , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , , StatusID.ToString, , , , , , , DueStatus, , , nWONumber))
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
                                        'Commented and changed by Saylee on 10-Oct-2013 for ALL10102013
                                        'Description = ObjAssemblyMonitorModStatus.Description & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatus.Number & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatus.Reference
                                        Description = ObjAssemblyMonitorModStatus.Description & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatus.Number
                                        '****************************
                                        AssemblyModel = ObjAssemblyStatus.Model
                                        AssemblySerialNo = ObjAssemblyStatus.SerialNo & IIf(IsExcel, Chr(10), vbCrLf)
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
                                        RequiredManHours = ObjAssemblyMonitorModStatus.RequiredManHours
                                        Customer = ObjMachine.Customer

                                        Note = ObjAssemblyMonitorModStatus.Notes
                                        'Added by Saylee on 10-Oct-2013 for ALL10102013
                                        'MaintenanceEvent = ObjAssemblyMonitorModStatus.Type 
                                        If ObjAssemblyMonitorModStatus.Reference <> "" Then
                                            MaintenanceEvent = ObjAssemblyMonitorModStatus.Type & " (" & ObjAssemblyMonitorModStatus.MonitorType & ")" & IIf(IsExcel, Chr(10), vbCrLf) & ObjAssemblyMonitorModStatus.Reference
                                        Else
                                            MaintenanceEvent = ObjAssemblyMonitorModStatus.Type & " (" & ObjAssemblyMonitorModStatus.MonitorType & ")"
                                        End If


                                        '*************************
                                        'Added By Saylee on 04-08-2008
                                        ExtensionDate = ObjAssemblyMonitorModStatus.ExtensionDate
                                        ApprovalRemark = ObjAssemblyMonitorModStatus.ApprovalRemark

                                        StatusID = ObjAssemblyMonitorModStatus.ID 'Added by Saylee on 6-May-2013 for ALL06052013-1


                                        'If ObjAssemblyMonitorModStatus.AssemblyMonitorModStatusPeriodList.Count > 0 Then
                                        ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, , AssemblySerialNo, ATAChapter, , , Position, ObjAssemblyMonitorModStatus.MonitorType, MonitorTypeCode, Note, Remark, Description, _
                                           , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, _
                                           SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, AssemblyTypeID, MaintenanceEvent, , , , , , , , , , , , , , , , , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , , StatusID.ToString, , , , , , , DueStatus, , , nWONumber))
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
                                            'Commented and Added by Saylee on 10-Oct-2013 for ALL10102013
                                            'Description = ObjCompMonitorModStatus.Description & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatus.Number & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatus.Reference
                                            Description = ObjCompMonitorModStatus.Description & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatus.Number
                                            '**********************************
                                            PartNo = ObjCompStatus.PartName
                                            CompSerialNo = ObjCompStatus.CompSerialNo
                                            Position = ObjCompStatus.Position
                                            MonitorTypeCode = ObjCompMonitorModStatus.Code
                                            EstimatedDate = ObjCompMonitorModStatus.EstimatedDateFormatted
                                            AssemblyModel = ObjAssemblyStatus.Model
                                            AssemblySerialNo = ObjAssemblyStatus.SerialNo & IIf(IsExcel, Chr(10), vbCrLf)
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
                                            RequiredManHours = ObjCompMonitorModStatus.RequiredManHours
                                            Customer = ObjMachine.Customer

                                            Note = ObjCompMonitorModStatus.Notes

                                            'Commented and Added by Saylee on 10-Oct-2013 for ALL10102013
                                            'MaintenanceEvent = ObjCompMonitorModStatus.Type
                                            If ObjCompMonitorModStatus.Reference <> "" Then
                                                MaintenanceEvent = ObjCompMonitorModStatus.Type & " (" & ObjCompMonitorModStatus.MonitorType & ")" & IIf(IsExcel, Chr(10), vbCrLf) & ObjCompMonitorModStatus.Reference
                                            Else
                                                MaintenanceEvent = ObjCompMonitorModStatus.Type & " (" & ObjCompMonitorModStatus.MonitorType & ")"
                                            End If

                                            '***************************************
                                            'Added By Saylee on 04-08-2008
                                            ExtensionDate = ObjCompMonitorModStatus.ExtensionDate
                                            ApprovalRemark = ObjCompMonitorModStatus.ApprovalRemark

                                            StatusID = ObjCompMonitorModStatus.ID 'Added by Saylee on 6-May-2013 for ALL06052013-1

                                            'If ObjCompMonitorModStatus.CompMonitorModStatusPeriodList.Count > 0 Then
                                            ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, RegNo, AssemblyType, MaintenanceEvent, AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, ObjCompMonitorModStatus.MonitorType, MonitorTypeCode, Note, Remark, Description, _
                                                                  , EstimatedDate, , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2, DueAsof, DueAsof1, DueAsof2, AssemblyModel, SinceNew, SinceNew1, SinceNew2, DoneAt, DoneAt1, DoneAt2, MinimumRemainingValue, _
                                                                  AssemblyTypeID, MaintenanceEvent, , , , , , , , , , , , , , , , , DoneOnDate, , , , AssemblyDueAsof, AssemblyDueAsof1, AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark, RequiredManHours, Customer, Code, StatusMasterID.ToString, DocumentTypeForID, , , , StatusID.ToString, , , , , , , DueStatus, , , nWONumber))
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
                                                  "WONumber", _
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
                                                  "MaintenanceOnExcel", _
                                                  "SinceNewAllExcel", _
                                                  "EstimatedDate", _
                                                  "AssDueAsofAllExcel", _
                                                  "MachineID", _
                                                  "ModelID", _
                                                  "DiffCompInstDoneOnValue", _
                                                  "ThresholdAccordingToTypeIDForExcel", "FrequencyAccordingToTypeIDForExcel", "DueAsOfAssemblyOrCompForExcel", "DueAsOfAirframeForExcel", "RemainingForExcel"
                                            }

        For i As Integer = 0 To columnToRemove.Length - 1
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns.Contains(columnToRemove(i)) Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns.Remove(columnToRemove(i))
            End If
        Next
        Dim columnscnt As Integer = ds.Tables("ExcelReportMaintenanceDetailList").Columns.Count

        'set Column Sequence
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("MaintenanceInfoExcel").SetOrdinal(0)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("FrequencyExcel").SetOrdinal(1)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("ElapsedAllExcel").SetOrdinal(2)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("EffectiveFromAllExcel").SetOrdinal(3)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("DoneAtAllExcel").SetOrdinal(4)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("ExtensionAllExcel").SetOrdinal(5)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("DueAsofAllExcel").SetOrdinal(6)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("RemainingTimeAllExcel").SetOrdinal(7)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("Note").SetOrdinal(8)
        ds.Tables("ExcelReportMaintenanceDetailList").Columns("Remark").SetOrdinal(9)


        For i As Integer = 0 To ds.Tables("ExcelReportMaintenanceDetailList").Columns.Count - 1
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "ModificationNumber" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Directive No"
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "FrequencyExcel" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Frequency"
            End If

            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "ElapsedAllExcel" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Elapsed"
            End If
            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "DueAsofAllExcel" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Due At"
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

            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "MaintenanceInfoExcel" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Maintenance Info"
            End If

            If ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "ExtensionAllExcel" Then
                ds.Tables("ExcelReportMaintenanceDetailList").Columns(i).ColumnName = "Extension"
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
                                               "SearchStr2", _
                                               "SearchStr4", _
                                               "SearchStr5", _
                                               "SearchStr6", _
                                               "SearchStr7", _
                                               "SearchStr8", _
                                               "SearchStr9", _
                                               "ProductVersion", _
                                               "SINote", _
                                               "CurrencyName", _
                                               "CurrencySymbol", _
                                               "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", _
                                                 "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", _
                                                 "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", _
                                                 "SearchStr25"
                                            }

        For i As Integer = 0 To columnToRemoveCriteria.Length - 1
            If ds.Tables("ExcelReport").Columns.Contains(columnToRemoveCriteria(i)) Then
                ds.Tables("ExcelReport").Columns.Remove(columnToRemoveCriteria(i))
            End If
        Next

        For i As Integer = 0 To ds.Tables("ExcelReport").Columns.Count - 1
            If ds.Tables("ExcelReport").Columns(i).ColumnName = "SearchStr1" Then
                ds.Tables("ExcelReport").Columns(i).ColumnName = "Due Limit"
            End If
            If ds.Tables("ExcelReport").Columns(i).ColumnName = "SearchStr3" Then
                ds.Tables("ExcelReport").Columns(i).ColumnName = "Assembly"
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
		PeriodColumnsForExportToExcel.AddRange(New String() {"Frequency", "Elapsed", "Remaining", "Due At", "Done At", "AssemblySerialNo", "Extension", "Maintenance Info", "Effective From"})
        Session("PeriodColumnsForExportToExcel") = PeriodColumnsForExportToExcel
        Session("dsNew") = dsNew
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openFilel", "openFile();", True)
        'Added by Prashant on 19-Jan-2021
        MarkLog(Util.Action.Print, "Due-RemovedAssembly", "Export To Excel " + mEventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
    End Sub
    Private Sub SetReport(Optional ByVal ByMail As Boolean = False, Optional ByVal ByExcel As Boolean = False)
        Try
            ReportMaintenanceDetails = New ReportMaintenanceDetailList
            ReportStatusList = New rptStatusList
            Dim da As New CSLA.Data.ObjectAdapter
            Dim ds As New dsReportMaintenanceDetail

            Dim mCompanyDetail As New CompanyDetail
            Dim searchstr As String = ""
            Dim searchstr6 As String = ""
            Dim searchstr8 As String = ""
            Dim OperatorName As String = ""


            SetValues()

            ReportDetail(mIsExcel)


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

            Dim ReportName As String
            'Code Added By Deven on 07/04/2008------------
            Dim rptDueDetail As CrystalDecisions.CrystalReports.Engine.ReportClass
            If DueType = 1 Then

                rptDueDetail = New crDueReportDetailForRemovedAssemblyComp


                If ((AppSettings("ClientCode") = "Heligo")) Then
                    ReportName = "Maintenance Forecast for Removed Assemblies"
                Else
                    ReportName = "Maintenance Status Report for Removed Assemblies"
                End If

            End If




            Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
       mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
       mCompanyDetail.WebSite, ReportName, searchstr, "Assembly", Assembly1, AppSettings("ClientCode"), "", AppSettings("Product Version"), AppSettings("SINote"), searchstr6, OperatorName, searchstr8, "", AppSettings("Logo"), AppSettings("FormNo"), mModuleList.Item("Due-RemovedAssembly").FormRevisionNo)
            'Replace AppSettings("RevisionNo") with mModuleList.Item("Due-RemovedAssembly").FormRevisionNo in Report Data  by Shital

            If ByMail = False Then
                If ReportMaintenanceDetails.Count = 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 1324)

                End If
            End If
            If (ByMail = True And ReportMaintenanceDetails.Count <= 0) Then
                SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, "There is no record for this search criteria.", "", _
                    Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                    ReportGeneratedBy:=Session("ReportGenratedBy"), _
                     SmtpHost:=mModuleList.Item("Due-RemovedAssembly").SmtpHost, SmtpPort:=mModuleList.Item("Due-RemovedAssembly").SmtpPort, _
                SmtpUser:=mModuleList.Item("Due-RemovedAssembly").SmtpUser, SmtpPassword:=mModuleList.Item("Due-RemovedAssembly").SmtpPassword)
                Exit Sub
            End If
            '11-Sep-2008-------------------------------
            If Not mIsPreview Then
                ds.Clear()
                Dim mrptImage As rptImage = rptImage.GetImage(ds)
                da.Fill(ds, ReportMaintenanceDetails)

                da.Fill(ds, Report)
                da.Fill(ds, ReportStatusList)
                da.Fill(ds, mrptImage)
                rptDueDetail.SetDataSource(ds)
                Session("CrystalReport") = rptDueDetail

                If ByMail Then
                    SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, _
                                              " For " + lblDateRangeFrom.Text + ", " + lblAssembly1.Text, "", Session("ToSendMailIDs"), _
                                              Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                                              ReportGeneratedBy:=Session("ReportGenratedBy"), _
                     SmtpHost:=mModuleList.Item("Due-RemovedAssembly").SmtpHost, SmtpPort:=mModuleList.Item("Due-RemovedAssembly").SmtpPort, _
                SmtpUser:=mModuleList.Item("Due-RemovedAssembly").SmtpUser, SmtpPassword:=mModuleList.Item("Due-RemovedAssembly").SmtpPassword)
                ElseIf ByExcel Then
                    SetExcel(ReportMaintenanceDetails, Report, ReportName)
                Else
                    Dim Str As String
                    Str = "openTranDetail();"
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "openTranDetail", Str, True)
                    MarkLog(Util.Action.Print, "Due-RemovedAssembly", mEventLogDetails, Util.ErrorType.NoError, Guid.Empty, EventLogID)
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
                Dim reportmaintdetailslist As List(Of ReportMaintenanceDetail) = New List(Of ReportMaintenanceDetail)

                reportmaintdetailslist = (From c As ReportMaintenanceDetail In ReportMaintenanceDetails.AsParallel
                                         Order By c.MinimumRemainingValue, c.RegNo, c.AssemblyType, c.Model, c.AssemblySerialNo, c.MaintenanceEvent, c.Description, c.PartNo
                                         Select c).ToList
                Session("ReportMaintenanceDetails") = ReportMaintenanceDetails
                Session("reportmaintdetailslist") = reportmaintdetailslist
                Dim str As String
                str = "openledgersame('wfDueResult_Ajax.aspx?');"
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "OpenScript", str, True)
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

    Private Sub SetTitle()
        If DueType = 1 Then
            lbltitle.Text = "Search criteria for Due"
        End If
        upnlTitle.Update()
    End Sub
#End Region

#Region " Data Binding "
    Public Sub DataFieldBind()
        mDueLimits = DueLimits.GetDueLimits(Guid.Empty)
        gdvDuePeriodLimits.DataSource = mDueLimits
        Session("mDueLimits") = mDueLimits
        upnlDueLimits.Update()

        mRemovedAssemblyListForDue = RemovedAssemblyListForCombo.GetAssemblyList(txtFromDate.Text, "(ALL)")
        cmbAssembly.DataSource = mRemovedAssemblyListForDue
        Session("mRemovedAssemblyListForDue") = mRemovedAssemblyListForDue

        DataBind()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New DataSet()
        da.Fill(ds, mRemovedAssemblyListForDue)
        Dim dv As DataView = ds.Tables(0).DefaultView
        dv.RowFilter = "IsSpareAssembly='True'"
        For Each dr As DataRowView In dv
            For Each item As ListItem In cmbAssembly.Items
                If dr("AssemblyStatusID").ToString() = item.Value.ToString() Then
                    item.Attributes.Add("style", "background-color:#ffbf00;color:black;font-weight:bold;")
                End If
            Next
        Next
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

#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ClearAll()
        GetSession()
        addAttributes()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("Sender") = "" Then
            DueType = Request.QueryString("DueType")
            Session("DueType") = DueType
            Session("MiddleFrame") = "wfSearchCriteriaForDueRemovedComp_Ajax.aspx?DueType=" & DueType
            ResetValues()
            ''SetFocus(txtFromDate)
            txtFromDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            AOnDate = Now.Date.ToString(AppSettings("DateFormat"))
            setFocus(txtFromDate)
            DataFieldBind()
            SetTypeCombo()
            Report = 1
            SetSession()
        End If
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        If IsValid = True Then
            Display()
            SetValues()
        End If
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If IsValid = True Then
            mIsExcel = False
            SetReport(, mIsExcel)
        End If
    End Sub
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
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
    '11-Sep-2008--------------------
    Private Sub btnPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        mIsPreview = True
        If IsValid = True Then
            SetReport()
        End If
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
                mAssemblyList = Nothing
                Session("mAssemblyList") = mAssemblyList
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
            FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (hdnimgBtnSendMail.Click): " + ex.GetBaseException.Message + vbLf)
            FileClose(1)
        End Try
    End Sub
    Protected Sub btnByMail_Click(sender As Object, e As EventArgs) Handles btnByMail.Click
        ' Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
        'Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail

        Session("UserEmailID") = mModuleList.Item("Due-RemovedAssembly").SendToMailID
        Session("UserCcEmailID") = mModuleList.Item("Due-RemovedAssembly").SendCCMailID
        '--------------------------
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