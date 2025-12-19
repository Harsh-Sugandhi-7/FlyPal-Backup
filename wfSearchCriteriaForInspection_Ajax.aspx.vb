Imports System.Text
Partial Class wfSearchCriteriaForInspection_Ajax
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "Variable Declaration"
    Dim ReportMaintenanceDetails As New ReportMaintenanceDetailList
    Dim mAssemblylist As AssemblyList
    Dim mInspTypeList As InspTypeList
    Dim mInspectionTypeList As ModelMonitorInspTypeList
    Dim ReportStatusList As New rptStatusList
    Dim mMachineList As MachineList
    Dim ReportLabel As String
    Dim AOnDate As String
    Dim Report As Integer = 1
    Dim ShowCofA As Boolean = False
    Dim AsonDate As String = ""
    Dim Periodcount As Integer
    Dim Count As Integer
    Dim AssemblyName As String
    Dim MachineName As String
    Dim AssemblyID As Guid
    Private ATAChapter As String
    Private AssemblyType As String
    Private AssemblySerialNo As String
    Private PartNo As String
    Private CompSerialNo As String
    Private Position As String
    Private MonitorTypeCode As String
    Private MonitorType As String
    Private Note As String
    Private Description As String
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
    Private Reference As String
    Private DoneOnValue As String
    Private DoneOnDate As String
    Private DoneWONo As String
    Private Remark As String
    Private Extension As String
    Private Extension1 As String
    Private Extension2 As String
    Private ExtensionDate As String
    Private ApprovalRemark As String
    Dim AssemblyDueAsof2 As String
    Private Inspection As String
    Private InspectionName, SerialNoPostion As String

    Dim searchstr7 As String = "" 'Added By Utkarsh On 07-Apr-2011
    Dim mSearchCriteriaForEventLog As String = ""
    Dim EventLogID As Guid
    Dim mMachineNameValueList As MachineNameValueList

    'Added By Saylee On 10-Nov-2014
    Dim AirframeDueAsof As String
    Dim AirframeDueAsof1 As String
    Dim AirframeDueAsof2 As String
    'End
#End Region

#Region " Helper Methods "
    Private Sub GetSession()
        mMachineNameValueList = Session("mMachineNameValueList")
        mAssemblylist = CType(Session("mAssemblylist"), AssemblyList)
        AOnDate = Session("AOnDate")
        Report = Session("Report")
        ShowCofA = Session("ShowCofA")
        mInspTypeList = Session("mInspTypeList")
    End Sub
    Private Sub SetSession()
        Session("mAssemblylist") = mAssemblylist
        Session("AOnDate") = AOnDate
        Session("Report") = Report
        Session("ShowCofA") = ShowCofA
        Session("mInspTypeList") = mInspTypeList
        Session("mMachineNameValueList") = mMachineNameValueList
    End Sub
    Private Sub ClearAll()
        If Session("MiddleFrame") <> "wfSearchCriteriaForInspection_Ajax.aspx?" Then
            Session.Remove("mAssemblylist")
            Session.Remove("AOnDate")
            Session.Remove("Report")
            Session.Remove("mMachineNameValueList")
            Session.Remove("mInspTypeList")
            Session.Remove("ShowCofA")
        End If
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mAssemblylist")
        Session.Remove("AOnDate")
        Session.Remove("Report")
        Session.Remove("mMachineNameValueList")
        Session.Remove("mInspTypeList")
        Session.Remove("ShowCofA")
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
        upnlSearchCriteria.Update()
    End Sub
    Private Sub SetValues()
        If cmbAircraft.SelectedItem.Text = "(Select)" Then
            lblAssembly1.Text = "Assembly Name : "
            lblAircraft1.Text = "Aircraft Name : All "
        Else
            AssemblyType = mAssemblylist(cmbAssembly.SelectedIndex).AssemblyType
            AssemblyName = cmbAssembly.SelectedValue.ToString
            lblAssembly1.Text = "Assembly Name : " & cmbAssembly.SelectedItem.Text
            MachineName = cmbAircraft.SelectedValue.ToString
            lblAircraft1.Text = "Aircraft Name : " & cmbAircraft.SelectedItem.Text
        End If

        If Not IsDate(txtAsOnDate.Text) Then      'AsOnDate
            AsonDate = ""
        Else
            AsonDate = txtAsOnDate.Text
            lblDateRange.Text = "AsonDate : " & New SmartDate(txtAsOnDate.Text).FormattedText
        End If

        If cmbType.SelectedItem.Text = "<SELECT>" Then     'Inspection
            Inspection = ""
            lblType1.Text = ""
        Else
            InspectionName = mInspTypeList(cmbType.SelectedIndex).Name
            Inspection = cmbType.SelectedItem.Text
            lblType1.Text = "Inspection Name : " & Inspection
        End If
        mSearchCriteriaForEventLog = lblDateRange.Text + ", " + lblAircraft1.Text + ", " + lblAssembly1.Text + ", " + lblType1.Text + "," + IIf(chkAirframeDueAsOf.Checked, chkAirframeDueAsOf.Text, "")
    End Sub
    Private Sub ResetValues()
        'cnbAdType.SelectedIndex = 0
        AssemblyName = "{00000000-0000-0000-0000-000000000000}"

        ShowCofA = False 'True
        Session("ShowCofA") = ShowCofA

        AssemblyType = ""
        MachineName = "{00000000-0000-0000-0000-000000000000}"
        'CNDC
        txtAsOnDate.Text = AsonDate
        If AsonDate <> "" Then
            txtAsOnDate.Text = AsonDate
        End If
    End Sub
    Public Function ReportDetail() As ReportMaintenanceDetailList
        Dim ObjMachine As MachineInfo
        Dim ObjAssemblyStatus As AssemblyStatusInfo
        Dim ObjCompStatus As CompStatusInfo

        Dim ObjAssemblyMonitorInspStatus As AssemblyMonitorInspStatusInfo
        Dim ObjAssemblyMonitorInspStatusPeriod As AssemblyMonitorInspStatusPeriodInfo

        Dim ObjCompMonitorInspStatus As CompMonitorInspStatusInfo
        Dim ObjCompMonitorInspStatusPeriod As CompMonitorInspStatusPeriodInfo


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
                'Added By Utkarsh On 11-Aug-2011 for IND11082011 , "Operator :" 
                If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "Indamer" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
                    searchstr7 = ObjMachine.Owner.ToString  'Added By Utkarsh On 07-Apr-2011 '"Owner/Operator : " +
                Else
                    searchstr7 = ""
                End If
                'End  "Due As of " & ObjAssemblyStatus.AssemblyType
                AssemblyID = ObjAssemblyStatus.AssemblyID
                ReportStatusList.Add(New rptStatus(AssemblyID.ToString, ObjAssemblyStatus.AssemblyTypeID, , "Reg No.", ObjMachine.RegNo, ObjAssemblyStatus.AssemblyType + " " + "Model", ObjAssemblyStatus.Model,
                   "Serial No.", SerialNoPostion, IIf(chkAirframeDueAsOf.Checked, "Next Due (Airframe Values)", "Next Due"), , , , , , , , , , , , , LHLabel2, LHData2))
            Next
        Next

        mInspectionTypeList = ModelMonitorInspTypeList.GetModelMonitorInspTypeList()

        For i As Integer = 0 To mInspectionTypeList.Count - 1
            If mInspectionTypeList.Item(mInspectionTypeList(i, "").ID).InspTypeID = cmbType.SelectedValue Then
                mMachineList = MachineList.GetMachineListMonitoringStatus(AsonDate, MachineName, , , , , , , , , , True, True, , AssemblyName, , , , , , , , , , , ShowCofA, , , True, , , , , , , , False, , False, , True, , , , mInspectionTypeList(i, "").ID, , , True, SkipIsForInventoryAircarft:=True)
                For Each ObjMachine In mMachineList
                    For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                        '********************************
                        For Each ObjAssemblyMonitorInspStatus In ObjAssemblyStatus.AssemblyMonitorInspStatusList
                            If (ObjAssemblyMonitorInspStatus.IsApplicable = True) Then 'Or (ObjAssemblyMonitorInspStatus.IsApplicable = False And ObjAssemblyMonitorInspStatus.IsCompleted = True) Then
                                ATAChapter = ObjAssemblyMonitorInspStatus.ATACode.ToString + " " + "-" + " " + ObjAssemblyMonitorInspStatus.ATANomenclature
                                Description = ObjAssemblyMonitorInspStatus.Description
                                Position = ObjAssemblyStatus.Position
                                MonitorTypeCode = ObjAssemblyMonitorInspStatus.Code
                                MonitorType = ObjAssemblyMonitorInspStatus.Type
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
                                Extension = ""
                                Extension1 = ""
                                Extension2 = ""
                                DoneOnValue = ""

                                'Added By Saylee On 10-Nov-2017 
                                AirframeDueAsof = ""
                                AirframeDueAsof1 = ""
                                AirframeDueAsof2 = ""
                                'End
                                For Each ObjAssemblyMonitorInspStatusPeriod In ObjAssemblyMonitorInspStatus.AssemblyMonitorInspStatusPeriodList
                                    If ObjAssemblyMonitorInspStatusPeriod.PeriodID = 2 Then
                                        If Freq3 = "" Then
                                            Freq3 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValueFormatted
                                            If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False And ObjAssemblyMonitorInspStatus.IsCompleted = True) Then
                                                ElapsedTime2 = ""
                                                RemainingTime2 = ""
                                                DueAsof2 = ""
                                                AssemblyDueAsof2 = ""
                                                AirframeDueAsof2 = ""  'Added By Saylee On 10-Nov-2017 
                                            Else
                                                ElapsedTime2 = ObjAssemblyMonitorInspStatusPeriod.ElapsedValueFormatted
                                                RemainingTime2 = ObjAssemblyMonitorInspStatusPeriod.RemainingValueFormatted
                                                DueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
                                                AssemblyDueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
                                                AirframeDueAsof2 = ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueTextByAirFrame 'Added By Saylee On 10-Nov-2017 
                                            End If
                                            Extension2 = ObjAssemblyMonitorInspStatusPeriod.ExtensionValueFormatted
                                            ''DoneOnValue = ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted
                                        Else
                                            Freq3 = Freq3 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.FrequencyValueFormatted
                                            If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False And ObjAssemblyMonitorInspStatus.IsCompleted = True) Then
                                                ElapsedTime2 = ""
                                                RemainingTime2 = ""
                                                DueAsof2 = ""
                                                AssemblyDueAsof2 = ""
                                                AirframeDueAsof2 = "" 'Added By Saylee On 10-Nov-2017 
                                            Else
                                                ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.ElapsedValueFormatted
                                                RemainingTime2 = RemainingTime2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.RemainingValue
                                                DueAsof2 = DueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
                                                AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DueOnValueFormatted
                                                AirframeDueAsof2 = AirframeDueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame 'Added By Saylee On 10-Nov-2017
                                            End If
                                            Extension2 = Extension2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.ExtensionValueFormatted
                                            ''DoneOnValue = DoneOnValue & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DoneOnValueFormatted
                                        End If
                                    Else
                                        If Freq3 = "" Then
                                            Freq3 = ObjAssemblyMonitorInspStatusPeriod.FrequencyValue
                                            If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False And ObjAssemblyMonitorInspStatus.IsCompleted = True) Then
                                                ElapsedTime2 = ""
                                                RemainingTime2 = ""
                                                DueAsof2 = ""
                                                AssemblyDueAsof2 = ""
                                                AirframeDueAsof2 = "" 'Added By Saylee On 10-Nov-2017 
                                            Else
                                                ElapsedTime2 = ObjAssemblyMonitorInspStatusPeriod.ElapsedValue
                                                RemainingTime2 = ObjAssemblyMonitorInspStatusPeriod.RemainingValue
                                                DueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValue
                                                AssemblyDueAsof2 = ObjAssemblyMonitorInspStatusPeriod.DueOnValue
                                                AirframeDueAsof2 = AirframeDueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame 'Added By Saylee On 10-Nov-2017
                                            End If
                                            Extension2 = ObjAssemblyMonitorInspStatusPeriod.ExtensionValue
                                            DoneOnValue = ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
                                        Else
                                            Freq3 = Freq3 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.FrequencyValue
                                            If (ObjAssemblyMonitorInspStatus.MonitorTypeID = 1 And ObjAssemblyMonitorInspStatus.IsCompleted = True) Or (ObjAssemblyMonitorInspStatus.IsApplicable = False And ObjAssemblyMonitorInspStatus.IsCompleted = True) Then
                                                ElapsedTime2 = ""
                                                RemainingTime2 = ""
                                                DueAsof2 = ""
                                                AssemblyDueAsof2 = ""
                                                AirframeDueAsof2 = ""
                                            Else
                                                ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.ElapsedValue
                                                RemainingTime2 = RemainingTime2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.RemainingValue
                                                DueAsof2 = DueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DueOnValue
                                                AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DueOnValue
                                                AirframeDueAsof2 = AirframeDueAsof2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame 'Added By Saylee On 10-Nov-2017
                                            End If
                                            Extension2 = Extension2 & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.ExtensionValue
                                            DoneOnValue = DoneOnValue & vbCrLf & ObjAssemblyMonitorInspStatusPeriod.DoneOnValue
                                        End If
                                    End If


                                Next
                                AssemblyID = ObjAssemblyStatus.AssemblyID
                                Note = ObjAssemblyMonitorInspStatus.Notes
                                Remark = ObjAssemblyMonitorInspStatus.DoneRemark
                                ExtensionDate = ObjAssemblyMonitorInspStatus.ExtensionDate
                                ApprovalRemark = ObjAssemblyMonitorInspStatus.ApprovalRemark

                                'DoneOnDate = ObjAssemblyMonitorInspStatus.DoneOn
                                Reference = ObjAssemblyMonitorInspStatus.Reference
                                DoneWONo = ObjAssemblyMonitorInspStatus.DoneWONo
                                DoneOnDate = ObjAssemblyMonitorInspStatus.DoneOnFormatted

                                'Added By Saylee On 10-Nov-2017 For ALL12022014
                                If chkAirframeDueAsOf.Checked Then
                                    DueAsof = AirframeDueAsof
                                    DueAsof1 = AirframeDueAsof1
                                    DueAsof2 = AirframeDueAsof2
                                End If
                                'End

                                ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, , , Position, MonitorType, MonitorTypeCode, Note, Remark, Description,
        , , , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2,
        DueAsof, DueAsof1, DueAsof2, AssemblyModel, , , , , , , , , , , , , , , , , , , , , , , , Reference, DoneOnValue, DoneOnDate, DoneWONo, , , , , AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark))
                            End If
                        Next
                        '********************************
                        For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
                            For Each ObjCompMonitorInspStatus In ObjCompStatus.CompMonitorInspStatusList
                                If (ObjCompMonitorInspStatus.IsApplicable = True) Then 'Or (ObjCompMonitorInspStatus.IsApplicable = False And ObjCompMonitorInspStatus.IsCompleted = True) Then
                                    ATAChapter = ObjCompMonitorInspStatus.ATACode.ToString + " " + "-" + " " + ObjCompMonitorInspStatus.ATANomenclature
                                    Description = ObjCompMonitorInspStatus.Description
                                    PartNo = ObjCompStatus.PartName
                                    CompSerialNo = ObjCompStatus.CompSerialNo
                                    Position = ObjCompStatus.Position
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
                                    Extension = ""
                                    Extension1 = ""
                                    Extension2 = ""
                                    AssemblyDueAsof2 = ""
                                    DoneOnValue = ""
                                    AirframeDueAsof = ""
                                    AirframeDueAsof1 = ""
                                    AirframeDueAsof2 = ""
                                    For Each ObjCompMonitorInspStatusPeriod In ObjCompMonitorInspStatus.CompMonitorInspStatusPeriodList
                                        If ObjCompMonitorInspStatusPeriod.PeriodID = 2 Then
                                            If Freq3 = "" Then
                                                Freq3 = ObjCompMonitorInspStatusPeriod.FrequencyValueFormatted
                                                If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Or (ObjCompMonitorInspStatus.IsApplicable = False And ObjCompMonitorInspStatus.IsCompleted = True) Then
                                                    ElapsedTime2 = ""
                                                    RemainingTime2 = ""
                                                    DueAsof2 = ""
                                                    AssemblyDueAsof2 = ""
                                                    AirframeDueAsof2 = ""
                                                Else
                                                    ElapsedTime2 = ObjCompMonitorInspStatusPeriod.ElapsedValueFormatted
                                                    RemainingTime2 = ObjCompMonitorInspStatusPeriod.RemainingValueFormatted
                                                    DueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                    AssemblyDueAsof2 = ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
                                                    AirframeDueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                End If
                                                Extension2 = ObjCompMonitorInspStatusPeriod.ExtensionValueFormatted
                                                '' DoneOnValue = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                            Else
                                                Freq3 = Freq3 & vbCrLf & ObjCompMonitorInspStatusPeriod.FrequencyValueFormatted
                                                If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Or (ObjCompMonitorInspStatus.IsApplicable = False And ObjCompMonitorInspStatus.IsCompleted = True) Then
                                                    ElapsedTime2 = ""
                                                    RemainingTime2 = ""
                                                    DueAsof2 = ""
                                                    AssemblyDueAsof2 = ""
                                                    AirframeDueAsof2 = ""
                                                Else
                                                    ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjCompMonitorInspStatusPeriod.ElapsedValueFormatted
                                                    RemainingTime2 = RemainingTime2 & vbCrLf & ObjCompMonitorInspStatusPeriod.RemainingValueFormatted
                                                    DueAsof2 = DueAsof2 & vbCrLf & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                    AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
                                                    AirframeDueAsof2 = AirframeDueAsof2 & vbCrLf & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                End If
                                                Extension2 = Extension2 & vbCrLf & ObjCompMonitorInspStatusPeriod.ExtensionValueFormatted
                                                '' DoneOnValue = DoneOnValue & vbCrLf & ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                            End If
                                        Else
                                            If Freq3 = "" Then
                                                Freq3 = ObjCompMonitorInspStatusPeriod.FrequencyValue
                                                If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Or (ObjCompMonitorInspStatus.IsApplicable = False And ObjCompMonitorInspStatus.IsCompleted = True) Then
                                                    ElapsedTime2 = ""
                                                    RemainingTime2 = ""
                                                    DueAsof2 = ""
                                                    AssemblyDueAsof2 = ""
                                                    AirframeDueAsof2 = ""
                                                Else
                                                    ElapsedTime2 = ObjCompMonitorInspStatusPeriod.ElapsedValue
                                                    RemainingTime2 = ObjCompMonitorInspStatusPeriod.RemainingValue
                                                    If ObjCompMonitorInspStatusPeriod.PeriodID = 9 Then
                                                        DueAsof2 = ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
                                                        AssemblyDueAsof2 = ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
                                                        AirframeDueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                    Else
                                                        DueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                        AssemblyDueAsof2 = ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
                                                        AirframeDueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                    End If
                                                End If
                                                Extension2 = ObjCompMonitorInspStatusPeriod.ExtensionValue
                                                DoneOnValue = ObjCompMonitorInspStatusPeriod.DoneOnValue
                                            Else
                                                Freq3 = Freq3 & vbCrLf & ObjCompMonitorInspStatusPeriod.FrequencyValue
                                                If (ObjCompMonitorInspStatus.MonitorTypeID = 1 And ObjCompMonitorInspStatus.IsCompleted = True) Or (ObjCompMonitorInspStatus.IsApplicable = False And ObjCompMonitorInspStatus.IsCompleted = True) Then
                                                    ElapsedTime2 = ""
                                                    RemainingTime2 = ""
                                                    DueAsof2 = ""
                                                    AssemblyDueAsof2 = ""
                                                    AirframeDueAsof2 = ""
                                                Else
                                                    ElapsedTime2 = ElapsedTime2 & vbCrLf & ObjCompMonitorInspStatusPeriod.ElapsedValue
                                                    RemainingTime2 = RemainingTime2 & vbCrLf & ObjCompMonitorInspStatusPeriod.RemainingValue
                                                    If ObjCompMonitorInspStatusPeriod.PeriodID = 9 Then
                                                        DueAsof2 = DueAsof2 & vbCrLf & ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
                                                        AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
                                                        AirframeDueAsof2 = ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                    Else
                                                        DueAsof2 = DueAsof2 & vbCrLf & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormatted
                                                        AssemblyDueAsof2 = AssemblyDueAsof2 & vbCrLf & ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
                                                        AirframeDueAsof2 = AirframeDueAsof2 & vbCrLf & ObjCompMonitorInspStatusPeriod.AssemblyDueOnValueTextFormattedByAirFrame
                                                    End If
                                                End If
                                                Extension2 = Extension2 & vbCrLf & ObjCompMonitorInspStatusPeriod.ExtensionValue
                                                DoneOnValue = DoneOnValue & vbCrLf & ObjCompMonitorInspStatusPeriod.DoneOnValue
                                            End If
                                        End If


                                    Next
                                    AssemblyID = ObjAssemblyStatus.AssemblyID
                                    Note = ObjCompMonitorInspStatus.Notes
                                    Remark = ObjCompMonitorInspStatus.DoneRemark
                                    ExtensionDate = ObjCompMonitorInspStatus.ExtensionDate
                                    ApprovalRemark = ObjCompMonitorInspStatus.ApprovalRemark
                                    Reference = ObjCompMonitorInspStatus.Reference
                                    DoneWONo = ObjCompMonitorInspStatus.DoneOnWONo
                                    DoneOnDate = ObjCompMonitorInspStatus.DoneOnFormatted

                                    'Added By Saylee On 10-Nov-2017 
                                    If chkAirframeDueAsOf.Checked Then
                                        DueAsof = AirframeDueAsof
                                        DueAsof1 = AirframeDueAsof1
                                        DueAsof2 = AirframeDueAsof2
                                    End If

                                    ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, MonitorType, MonitorTypeCode, Note, Remark, Description,
                 , , , , Freq1, Freq2, Freq3, ElapsedTime, ElapsedTime1, ElapsedTime2, RemainingTime, RemainingTime1, RemainingTime2,
                 DueAsof, DueAsof1, DueAsof2, AssemblyModel, , , , , , , , , , , , , , , , , , , , , , , , Reference, DoneOnValue, DoneOnDate, DoneWONo, , , , , AssemblyDueAsof2, Extension, Extension1, Extension2, ExtensionDate, ApprovalRemark))
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
        Dim RptInspectionStatusList As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim mCompanyDetail As New CompanyDetail
        Dim OperatorName As String = String.Empty

        If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
            RptInspectionStatusList = New crInspectionStatusListForTAAL
            'Added By Utkarsh On 07-Apr-2011
        ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
            RptInspectionStatusList = New crInspectionStatusListForDeccan
            '******************************
        Else
            RptInspectionStatusList = New crInspectionStatusList
        End If

        SetValues()
        ReportDetail()

        ReportLabel = AssemblyType + " " + InspectionName

        'Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        'mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        'mCompanyDetail.WebSite, ReportLabel, New SmartDate(txtFromDate.Value.ToString).FormattedText,  mOpen.ToString, "", "", "", AppSettings("Product Version"), AppSettings("SINote"))

        If (AppSettings("ClientCode") IsNot Nothing) AndAlso ((AppSettings("ClientCode") = "Indamer")) Then
            Dim mMachineOperatorName As MachineOperatorName = MachineOperatorName.GetMachineOperatorName(New Guid(cmbAircraft.SelectedValue))
            If mMachineOperatorName.OperatorName <> "" Then OperatorName = mMachineOperatorName.OperatorName
        ElseIf (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Deccan" Or AppSettings("ClientCode") = "ADeccan" Or AppSettings("ClientCode") = "IIC" Or AppSettings("ClientCode") = "SPZ") Then ' SPZ Code added by Saylee on 13-Jun-2022
            OperatorName = searchstr7
        End If

        Dim InspsShortName As String = ""
        'Added By Vikrant On 27-Feb-2020 for showing Periods Code and their long forms at bottom of report
        Dim mPeriodUnitList As PeriodUnitList
        Dim PeriodsShortName As New StringBuilder

        mPeriodUnitList = PeriodUnitList.GetPeriodUnitList()
        For i As Integer = 0 To mPeriodUnitList.Count - 1
            PeriodsShortName.Append(mPeriodUnitList(i).Code + "-" + mPeriodUnitList(i).PeriodUnitName + ", ")
        Next
        'End

        For i As Integer = 0 To mInspectionTypeList.Count - 1
            If InspsShortName = "" Then
                InspsShortName = IIf(Not mInspectionTypeList(i, "").CodeType Is Nothing, mInspectionTypeList(i, "").CodeType, "")
            Else
                InspsShortName = InspsShortName + IIf(Not mInspectionTypeList(i, "").CodeType Is Nothing, ", " + mInspectionTypeList(i, "").CodeType, "")
            End If
        Next

        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
        mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
        mCompanyDetail.WebSite, ReportLabel, New SmartDate(txtAsOnDate.Text).FormattedText, "", "", "", txtBottomLine.Text, AppSettings("Product Version"), AppSettings("SINote"), "", OperatorName, "", "", AppSettings("Logo"), , , InspsShortName, SearchStr17:=PeriodsShortName.ToString.Trim.TrimEnd(",")) 'Changed By Utkarsh For Report Logo.

        SetSession()
        If ReportMaintenanceDetails.Count = 0 Then
            MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
            Exit Sub
        Else
            RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1154)
        End If
        ds.Clear()
        '-----------Added by Utkarsh for Report Logo---------------
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        '----------------------------------------------------------
        da.Fill(ds, ReportMaintenanceDetails)
        da.Fill(ds, Report)
        da.Fill(ds, ReportStatusList)
        da.Fill(ds, mrptImage) 'Added by Utkarsh for Report Logo
        RptInspectionStatusList.SetDataSource(ds)
        Session("CrystalReport") = RptInspectionStatusList
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        MarkLog(Util.Action.Print, "InspectionStatus", mSearchCriteriaForEventLog, Util.ErrorType.NoError, Guid.Empty, EventLogID)
        ResetValues()
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

            End Select
        ElseIf Result1 = -1 Then
            Session("Sender") = ""
            'Response.Redirect("wfSearchCriteriaForInspection_Ajax.aspx?Open=" & mOpen)
        End If
    End Sub
#End Region

#Region " Data Binding "
    Public Sub SetComboOfMachine(ByVal Machine_AsOnDate As String)
        mMachineNameValueList = MachineNameValueList.GetMachineList(Machine_AsOnDate, , , , , , , True, "(Select)", , True)
        cmbAircraft.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        cmbAircraft.DataBind()
    End Sub
    Private Sub DataFieldBind()
        mInspTypeList = InspTypeList.GetInspTypeList(True)
        cmbType.DataSource = mInspTypeList
        Session("mInspTypeList") = mInspTypeList
        DataBind()
    End Sub

#End Region

#Region " Events "
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearAll()
        GetSession()
        EventLogID = CType(Session("EventLogID"), Guid)
        If Not IsPostBack And Session("Sender") = "" Then
            Session("MiddleFrame") = "wfSearchCriteriaForInspection_Ajax.aspx?"
            ResetValues()
            lblAssembly.Enabled = False
            cmbAssembly.Enabled = False
            txtAsOnDate.Text = Now.Date.ToString(AppSettings("DateFormat"))
            AOnDate = Now.Date
            Session("AOnDate") = AOnDate
            SetComboOfMachine(AOnDate)
            DataFieldBind()
            Report = 1
            Session("Report") = Report

            If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "Indamer") Then
                txtBottomLine.Text = "Date:" + vbCrLf + vbCrLf + "Place:" + vbCrLf + vbCrLf + "Prepared By:                                                                                                                       Checked By:                                                                                                                                                 Approved By:"
            ElseIf AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then
                txtBottomLine.Text = "I hereby certify that the data specified above has been certified throughout : 									Technical Support Division: __________________ Date: _____________"
            ElseIf AppSettings("ClientCode") = "APFT" Or
                   AppSettings("ClientCode") = "AAP" Then 'Added By Saylee On 1-Oct-2018 
                txtBottomLine.Text = "I hereby certify that the data specified above has been verified throughout. Continuing Airworthiness Manager: __________________ Date: _____________"

            Else
                txtBottomLine.Text = "I hereby certify that the data specified above has been certified throughout : Engineering Department Manager : ____________________   Date : __________ "
            End If

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
    Private Sub txtFromDate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAsOnDate.TextChanged 'CHK
        If Not IsDate(txtAsOnDate.Text) Then
            txtAsOnDate.Text = ""
            Exit Sub
        End If
        AOnDate = txtAsOnDate.Text
        Session("AOnDate") = AOnDate
        If Not AOnDate.Equals(txtAsOnDate.Text) Then
            SetComboOfMachine(txtAsOnDate.Text)
        End If
    End Sub
    Private Sub cmbAircraft_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
        If cmbAircraft.SelectedIndex = 0 Then
            lblAssembly.Enabled = False
            cmbAssembly.Enabled = False
        Else
            lblAssembly.Enabled = True
            cmbAssembly.Enabled = True

            mAssemblylist = AssemblyList.GetAssemblyListForComboBox(0, cmbAircraft.SelectedValue, txtAsOnDate.Text, "(All)", True)
            Session("mAssemblyList") = mAssemblylist
            cmbAssembly.DataSource = mAssemblylist
            cmbAssembly.DataBind()
        End If
        If cmbAircraft.Enabled = True Then
            setFocus(cmbAircraft)
        End If
    End Sub
#End Region



End Class
