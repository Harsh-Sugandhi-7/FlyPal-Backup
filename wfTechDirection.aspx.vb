'Created by Utkarsh on 08-Jan-2014

Imports System.Linq
Imports System.Collections.Generic

Public Class wfTechDirection
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Public mrptTechDirection As rptTechDirection
    Public mLocationList As LocationList
    'Added By Vikrant On 17-Jun-2015 For BA12062015
    Dim mMachineList As MachineList
    Dim ReportMaintenanceDetails As New ReportMaintenanceDetailList
    Dim MaintenanceActivityType As String
    Private ATAChapter As String = ""
    Private RegNo As String
    Private AssemblyType As String
    Private Model As String
    Private AssemblySerialNo As String
    Private CompSerialNo As String
    Private Position As String
    Private MonitorTypeCode As String = ""
    Private MonitorType As String = ""
    Private Note As String = ""
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
    Private DoneOnValue As String
    Private DoneOnDate As SmartDate = New SmartDate(True)
    Dim Periodcount As Integer
    Dim Count As Integer
    Dim AssemblyName As String
    Dim MachineName As String
    Dim Machine1 As String
    Dim AssemblyID As Guid
    Dim AircraftIndex As Integer   'Added Code 
    Public PartNo As String = String.Empty
    Public Description As String = String.Empty
    Dim AsonDate As String
    Dim MachineInfo, MachineID, AssemblyStatusID, CompStatusID, AssemblyInfo, CompInfo As String
    Dim PeriodUnitName, FrequencyValue, DueOnValue, FrequencyValueFormatted, DoneOnValueFormatted, DueOnValueFormatted, CurrentValueFormatted, ElapsedValueFormatted, ExtensionValueFormatted, RemainingValueFormatted, MonitorInfo, DoneOn As String
    'End
    Dim mModuleList As ModuleList
#End Region

#Region "Business Methods"
    Private Sub GetSession()
        mrptTechDirection = Session("mrptTechDirection")
        mLocationList = Session("mLocationList")
        mModuleList = Session("mModuleList")
        'ReportMaintenanceDetails = CType(Session("ReportMaintenanceDetails"), ReportMaintenanceDetailList)
    End Sub
    Private Sub RemoveSession()
        Session.Remove("mrptTechDirection")
        Session.Remove("mLocationList")
    End Sub
    Private Sub ControlVisibilityForButtons()
        btnPrint.Enabled = Not mrptTechDirection.IsNew
        txtFromDate.Enabled = mrptTechDirection.IsNew
        chkIsNoteRequired.Enabled = mrptTechDirection.IsNew
        upnlButtons.Update()
    End Sub
    Private Sub SetObject()
        If IsDate(txtFromDate.Text.Trim) Then
            mrptTechDirection.Date = txtFromDate.Text.Trim
        Else
            mrptTechDirection.Date = System.DBNull.Value
        End If
        mrptTechDirection.Text = txtText.Text.Trim
        mrptTechDirection.No = txtNo.Text.Trim
        mrptTechDirection.RemovalReason = txtRemovalReason.Text.Trim
        mrptTechDirection.WorkRequired = txtWorkRequired.Text.Trim
        mrptTechDirection.ReportsRequired = txtReportsRequired.Text.Trim
        mrptTechDirection.From = txtFrom.Text.Trim
        mrptTechDirection.To = txtTo.Text.Trim
        'Added By Vikrant On 15-Jun-2015 For BA12062015
        mrptTechDirection.Note = Trim(txtNote.Text)
        mrptTechDirection.IsNoteRequired = chkIsNoteRequired.Checked
        'End
        mrptTechDirection.Remark = Trim(txtRemark.Text) 'Added By Saylee on 27-Mar-2017
        Session("mrptTechDirection") = mrptTechDirection
    End Sub
    Private Function customvalidate() As Boolean
        Dim str As String = String.Empty
        If Not mrptTechDirection.IsValid Then
            str = String.Join("<BR>", From c In mrptTechDirection.GetBrokenRulesCollection Select c.Description)
        End If
        If str.Trim.Length > 0 Then
            cvCommon.IsValid = False
            cvCommon.ErrorMessage = str
            Return False
        Else
            Return True
        End If
    End Function
    Private Sub ControlVisiblity()
        lblTimeSince.Text = mrptTechDirection.InstallationStatusName
    End Sub
    
    Public Function ReportDetail() As ReportMaintenanceDetailList
        Dim ObjMachine As MachineInfo
        Dim ObjAssemblyStatus As AssemblyStatusInfo
        Dim ObjCompStatus As CompStatusInfo
        Dim ObjCompMonitorServiceStatus As CompMonitorServiceStatusInfo
        Dim ObjCompMonitorInspStatus As CompMonitorInspStatusInfo
        Dim ObjCompMonitorModStatus As CompMonitorModStatusInfo
        Dim ObjCompMonitorServiceStatusPeriod As CompMonitorServiceStatusPeriodInfo
        Dim ObjCompMonitorInspStatusPeriod As CompMonitorInspStatusPeriodInfo
        Dim ObjCompMonitorModStatusPeriod As CompMonitorModStatusPeriodInfo

        Dim LogID As String = CType(Session("TechLog"), String)
        mMachineList = MachineList.GetMachineListComplianceComponentsMonitoringStatusForRemovedComp(txtFromDate.Text, , txtPartNo.Text, , , , txtSerialNo.Text, , , , , , , , , , , True, True, True, , , txtAircaft.Text, LogID, SkipIsForInventoryAircarft:=True)

        For Each ObjMachine In mMachineList
            For Each ObjAssemblyStatus In ObjMachine.AssemblyStatusList
                For Each ObjCompStatus In ObjAssemblyStatus.CompStatusList
                    If ObjCompStatus.CompMonitorServiceStatusList.Count > 0 Then
                        For Each ObjCompMonitorServiceStatus In ObjCompStatus.CompMonitorServiceStatusList
                            PeriodUnitName = ""
                            FrequencyValue = ""
                            DueOnValue = ""
                            FrequencyValueFormatted = ""
                            DoneOnValueFormatted = ""
                            DueOnValueFormatted = ""
                            MonitorInfo = ""
                            DoneOn = ""
                            CurrentValueFormatted = ""
                            ElapsedValueFormatted = ""
                            ExtensionValueFormatted = ""
                            RemainingValueFormatted = ""

                            MaintenanceActivityType = ObjCompMonitorServiceStatus.Type
                            MonitorInfo = ObjCompMonitorServiceStatus.Code
                            DoneOn = ObjCompMonitorServiceStatus.DoneOnFormatted

                            For Each ObjCompMonitorServiceStatusPeriod In ObjCompMonitorServiceStatus.CompMonitorServiceStatusPeriodList
                                If PeriodUnitName = "" Then
                                    PeriodUnitName = ObjCompMonitorServiceStatusPeriod.PeriodUnitName
                                    FrequencyValue = ObjCompMonitorServiceStatusPeriod.FrequencyValue
                                    DoneOnValue = ObjCompMonitorServiceStatusPeriod.DoneOnValue
                                    DueOnValue = ObjCompMonitorServiceStatusPeriod.DueOnValue
                                    FrequencyValueFormatted = ObjCompMonitorServiceStatusPeriod.FrequencyValueFormatted
                                    DoneOnValueFormatted = ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                    DueOnValueFormatted = ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                    CurrentValueFormatted = ObjCompMonitorServiceStatusPeriod.CurrentValueFormatted
                                    ElapsedValueFormatted = ObjCompMonitorServiceStatusPeriod.ElapsedAtRemovalFormattedForOCComponents
                                    ExtensionValueFormatted = ObjCompMonitorServiceStatusPeriod.ExtensionValueFormatted
                                    RemainingValueFormatted = ObjCompMonitorServiceStatusPeriod.RemainingValueFormatted
                                Else
                                    PeriodUnitName = PeriodUnitName & "</BR>" & ObjCompMonitorServiceStatusPeriod.PeriodUnitName
                                    FrequencyValue = FrequencyValue & "</BR>" & ObjCompMonitorServiceStatusPeriod.FrequencyValue
                                    DoneOnValue = DoneOnValue & "</BR>" & ObjCompMonitorServiceStatusPeriod.DoneOnValue
                                    DueOnValueFormatted = DueOnValue & "</BR>" & ObjCompMonitorServiceStatusPeriod.DueOnValueFormatted
                                    FrequencyValueFormatted = FrequencyValueFormatted & "</BR>" & ObjCompMonitorServiceStatusPeriod.FrequencyValueFormatted
                                    DoneOnValueFormatted = DoneOnValueFormatted & "</BR>" & ObjCompMonitorServiceStatusPeriod.DoneOnValueFormatted
                                    CurrentValueFormatted = CurrentValueFormatted & "</BR>" & ObjCompMonitorServiceStatusPeriod.CurrentValueFormatted
                                    ElapsedValueFormatted = ElapsedValueFormatted & "</BR>" & ObjCompMonitorServiceStatusPeriod.ElapsedAtRemovalFormattedForOCComponents
                                    ExtensionValueFormatted = ExtensionValueFormatted & "</BR>" & ObjCompMonitorServiceStatusPeriod.ExtensionValueFormatted
                                    RemainingValueFormatted = RemainingValueFormatted & "</BR>" & ObjCompMonitorServiceStatusPeriod.RemainingValueFormatted
                                End If
                            Next
                            ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, MaintenanceActivityType, MonitorInfo, Note, DoneRemrk, Description, _
                           , EstimatedDate, , , FrequencyValueFormatted, Freq2, Freq3, ElapsedValueFormatted, ElapsedTime1, ElapsedTime2, RemainingValueFormatted, RemainingTime1, RemainingTime2, DueOnValueFormatted, DueAsof1, DueAsof2, AssemblyModel, PeriodUnitName, , , , , _
                           , , , , ATACode, InstalledAt, InstalledAt1, InstalledAt2, CurrentValueFormatted, TSO, TSO1, TSO2, RemoveAt, RemoveAt1, RemoveAt2, InstalledAtDate.Date.ToString("g"), RemoveAtDate.Date.ToString("g"), , , DoneOnValueFormatted, DoneOn, , , , , , , ExtensionValueFormatted))
                        Next
                    End If

                    If ObjCompStatus.CompMonitorInspStatusList.Count > 0 Then
                        For Each ObjCompMonitorInspStatus In ObjCompStatus.CompMonitorInspStatusList
                            PeriodUnitName = ""
                            FrequencyValue = ""
                            DueOnValue = ""
                            FrequencyValueFormatted = ""
                            DoneOnValueFormatted = ""
                            DueOnValueFormatted = ""
                            MonitorInfo = ""
                            DoneOn = ""
                            CurrentValueFormatted = ""
                            ElapsedValueFormatted = ""
                            ExtensionValueFormatted = ""
                            RemainingValueFormatted = ""

                            MaintenanceActivityType = ObjCompMonitorInspStatus.Type
                            MonitorInfo = ObjCompMonitorInspStatus.Code
                            DoneOn = ObjCompMonitorInspStatus.DoneOnFormatted

                            For Each ObjCompMonitorInspStatusPeriod In ObjCompMonitorInspStatus.CompMonitorInspStatusPeriodList
                                If PeriodUnitName = "" Then
                                    PeriodUnitName = ObjCompMonitorInspStatusPeriod.PeriodUnitName
                                    FrequencyValue = ObjCompMonitorInspStatusPeriod.FrequencyValue
                                    DoneOnValue = ObjCompMonitorInspStatusPeriod.DoneOnValue
                                    DueOnValue = ObjCompMonitorInspStatusPeriod.DueOnValue
                                    FrequencyValueFormatted = ObjCompMonitorInspStatusPeriod.FrequencyValueFormatted
                                    DoneOnValueFormatted = ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                    DueOnValueFormatted = ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
                                    CurrentValueFormatted = ObjCompMonitorInspStatusPeriod.CurrentValueFormatted
                                    ElapsedValueFormatted = ObjCompMonitorInspStatusPeriod.ElapsedValueFormatted
                                    ExtensionValueFormatted = ObjCompMonitorInspStatusPeriod.ExtensionValueFormatted
                                    RemainingValueFormatted = ObjCompMonitorInspStatusPeriod.RemainingValueFormatted
                                Else
                                    PeriodUnitName = PeriodUnitName & "</BR>" & ObjCompMonitorInspStatusPeriod.PeriodUnitName
                                    FrequencyValue = FrequencyValue & "</BR>" & ObjCompMonitorInspStatusPeriod.FrequencyValue
                                    DoneOnValue = DoneOnValue & "</BR>" & ObjCompMonitorInspStatusPeriod.DoneOnValue
                                    DueOnValue = DueOnValue & "</BR>" & ObjCompMonitorInspStatusPeriod.DueOnValue
                                    FrequencyValueFormatted = FrequencyValueFormatted & "</BR>" & ObjCompMonitorInspStatusPeriod.FrequencyValueFormatted
                                    DoneOnValueFormatted = DoneOnValueFormatted & "</BR>" & ObjCompMonitorInspStatusPeriod.DoneOnValueFormatted
                                    DueOnValueFormatted = DueOnValueFormatted & "</BR>" & ObjCompMonitorInspStatusPeriod.DueOnValueFormatted
                                    CurrentValueFormatted = CurrentValueFormatted & "</BR>" & ObjCompMonitorInspStatusPeriod.CurrentValueFormatted
                                    ElapsedValueFormatted = ElapsedValueFormatted & "</BR>" & ObjCompMonitorInspStatusPeriod.ElapsedValueFormatted
                                    ExtensionValueFormatted = ExtensionValueFormatted & "</BR>" & ObjCompMonitorInspStatusPeriod.ExtensionValueFormatted
                                    RemainingValueFormatted = RemainingValueFormatted & "</BR>" & ObjCompMonitorInspStatusPeriod.RemainingValueFormatted
                                End If
                            Next
                            ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, MaintenanceActivityType, MonitorInfo, Note, DoneRemrk, Description, _
                          , EstimatedDate, , , FrequencyValueFormatted, Freq2, Freq3, ElapsedValueFormatted, ElapsedTime1, ElapsedTime2, RemainingValueFormatted, RemainingTime1, RemainingTime2, DueOnValueFormatted, DueAsof1, DueAsof2, AssemblyModel, PeriodUnitName, , , , , _
                          , , , , ATACode, InstalledAt, InstalledAt1, InstalledAt2, CurrentValueFormatted, TSO, TSO1, TSO2, RemoveAt, RemoveAt1, RemoveAt2, InstalledAtDate.Date.ToString("g"), RemoveAtDate.Date.ToString("g"), , , DoneOnValueFormatted, DoneOn, , , , , , , ExtensionValueFormatted))
                        Next
                    End If

                    If ObjCompStatus.CompMonitorModStatusList.Count > 0 Then
                        For Each ObjCompMonitorModStatus In ObjCompStatus.CompMonitorModStatusList
                            PeriodUnitName = ""
                            FrequencyValue = ""
                            DueOnValue = ""
                            FrequencyValueFormatted = ""
                            DoneOnValueFormatted = ""
                            DueOnValueFormatted = ""
                            MonitorInfo = ""
                            DoneOn = ""
                            CurrentValueFormatted = ""
                            ElapsedValueFormatted = ""
                            ExtensionValueFormatted = ""
                            RemainingValueFormatted = ""

                            MaintenanceActivityType = ObjCompMonitorModStatus.Type
                            MonitorInfo = ObjCompMonitorModStatus.Code
                            DoneOn = ObjCompMonitorModStatus.DoneOnFormatted

                            For Each ObjCompMonitorModStatusPeriod In ObjCompMonitorModStatus.CompMonitorModStatusPeriodList
                                If PeriodUnitName = "" Then
                                    PeriodUnitName = ObjCompMonitorModStatusPeriod.PeriodUnitName
                                    FrequencyValue = ObjCompMonitorModStatusPeriod.FrequencyValue
                                    DoneOnValue = ObjCompMonitorModStatusPeriod.DoneOnValue
                                    DueOnValue = ObjCompMonitorModStatusPeriod.DueOnValue
                                    FrequencyValueFormatted = ObjCompMonitorModStatusPeriod.FrequencyValueFormatted
                                    DoneOnValueFormatted = ObjCompMonitorModStatusPeriod.DoneOnValueFormatted
                                    DueOnValueFormatted = ObjCompMonitorModStatusPeriod.DueOnValueFormatted
                                    CurrentValueFormatted = ObjCompMonitorModStatusPeriod.CurrentValueFormatted
                                    ElapsedValueFormatted = ObjCompMonitorModStatusPeriod.ElapsedValueFormatted
                                    ExtensionValueFormatted = ObjCompMonitorModStatusPeriod.ExtensionValueFormatted
                                    RemainingValueFormatted = ObjCompMonitorModStatusPeriod.RemainingValueFormatted
                                Else
                                    PeriodUnitName = PeriodUnitName & "</BR>" & ObjCompMonitorModStatusPeriod.PeriodUnitName
                                    FrequencyValue = FrequencyValue & "</BR>" & ObjCompMonitorModStatusPeriod.FrequencyValue
                                    DoneOnValue = DoneOnValue & "</BR>" & ObjCompMonitorModStatusPeriod.DoneOnValue
                                    DueOnValue = DueOnValue & "</BR>" & ObjCompMonitorModStatusPeriod.DueOnValue
                                    FrequencyValueFormatted = FrequencyValueFormatted & "</BR>" & ObjCompMonitorModStatusPeriod.FrequencyValueFormatted
                                    DoneOnValueFormatted = DoneOnValueFormatted & "</BR>" & ObjCompMonitorModStatusPeriod.DoneOnValueFormatted
                                    DueOnValueFormatted = DueOnValueFormatted & "</BR>" & ObjCompMonitorModStatusPeriod.DueOnValueFormatted
                                    CurrentValueFormatted = CurrentValueFormatted & "</BR>" & ObjCompMonitorModStatusPeriod.CurrentValueFormatted
                                    ElapsedValueFormatted = ElapsedValueFormatted & "</BR>" & ObjCompMonitorModStatusPeriod.ElapsedValueFormatted
                                    ExtensionValueFormatted = ExtensionValueFormatted & "</BR>" & ObjCompMonitorModStatusPeriod.ExtensionValueFormatted
                                    RemainingValueFormatted = RemainingValueFormatted & "</BR>" & ObjCompMonitorModStatusPeriod.RemainingValueFormatted
                                End If
                            Next
                            ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(AssemblyID, , , , AssemblySerialNo, ATAChapter, PartNo, CompSerialNo, Position, MaintenanceActivityType, MonitorInfo, Note, DoneRemrk, Description, _
                        , EstimatedDate, , , FrequencyValueFormatted, Freq2, Freq3, ElapsedValueFormatted, ElapsedTime1, ElapsedTime2, RemainingValueFormatted, RemainingTime1, RemainingTime2, DueOnValueFormatted, DueAsof1, DueAsof2, AssemblyModel, PeriodUnitName, , , , , _
                        , , , , ATACode, InstalledAt, InstalledAt1, InstalledAt2, CurrentValueFormatted, TSO, TSO1, TSO2, RemoveAt, RemoveAt1, RemoveAt2, InstalledAtDate.Date.ToString("g"), RemoveAtDate.Date.ToString("g"), , , DoneOnValueFormatted, DoneOn, , , , , , , ExtensionValueFormatted))
                        Next
                    End If
                Next
            Next
        Next
        Session("ReportMaintenanceDetails") = ReportMaintenanceDetails
        Return ReportMaintenanceDetails
    End Function
    Private Sub GridBind()
        ReportDetail()
        dgMaintenanceActivityList.DataSource = ReportMaintenanceDetails
        dgMaintenanceActivityList.DataBind()
    End Sub
    'End
#End Region

#Region "Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetSession()
        If Not Page.IsPostBack Then
            If AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Then
                mrptTechDirection.From = "BUDDHA AIR PVT. LTD" & Environment.NewLine & "PO BOX 13598" & Environment.NewLine & "KATHMANDU NEPAL"
                'mrptTechDirection.To = "WORK SHOP (BATTERY)" & Environment.NewLine & "BUDDHA AIR (P) LTD." & Environment.NewLine & "ENGINEERING DEPARTMENT" & Environment.NewLine & "SINAMANGAL,KATHMANDU"
            End If
            mLocationList = LocationList.GetLocationList(0, , , , , , True)
            Session("mLocationList") = mLocationList
            cmbLocation.DataSource = mLocationList
            'Added By Vikrant On 12-Jun-2015 For BA12062015
            'If AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo"  Then
            '    If mrptTechDirection.Note = "" Then
            '        mrptTechDirection.Note = "1. Mandatory SB's, AD's to be complied." + Environment.NewLine + "2. If PMA parts are to be used the party shall get concurrence from Buddha Air (P) Ltd." + Environment.NewLine + "3. Back to birth tracability documents to be provided for LLP."
            '    End If
            'End If
            'End
            ControlVisiblity() 'Added By Vikrant On 02-Apr-2015 For All31032015
            DataBind()
            GridBind() 'Added By Vikrant On 12-Jun-2015 For BA12062015
            ControlVisibilityForButtons()
        End If
    End Sub
    Private Sub txtFromDate_TextChanged(sender As Object, e As System.EventArgs) Handles txtFromDate.TextChanged, chkIsNoteRequired.CheckedChanged
        If IsDate(txtFromDate.Text.Trim) Then
            mrptTechDirection.Date = txtFromDate.Text.Trim
        End If
        mrptTechDirection.IsNoteRequired = chkIsNoteRequired.Checked
        txtText.Text = mrptTechDirection.Text
        Session("mrptTechDirection") = mrptTechDirection
        If chkIsNoteRequired.Checked = True Then
            txtNote.Enabled = True
        Else
            txtNote.Enabled = False
        End If
    End Sub
    Private Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        SetObject()
        If customvalidate() Then
            Try
                Dim mrptTechDirectionClone As rptTechDirection = CType(mrptTechDirection.Clone, rptTechDirection)
                mrptTechDirection.Save()
                mrptTechDirection = rptTechDirection.GetTechDirection(mrptTechDirection.StatusID, mrptTechDirection.TypeID)
                'set from clone
                mrptTechDirection.ATA = mrptTechDirectionClone.ATA
                mrptTechDirection.PartNo = mrptTechDirectionClone.PartNo
                mrptTechDirection.Description = mrptTechDirectionClone.Description
                mrptTechDirection.SerialNo = mrptTechDirectionClone.SerialNo
                mrptTechDirection.ModelName = mrptTechDirectionClone.ModelName
                mrptTechDirection.AircaftName = mrptTechDirectionClone.AircaftName
                mrptTechDirection.AircaftSrNo = mrptTechDirectionClone.AircaftSrNo
                mrptTechDirection.TimeSinceNew = mrptTechDirectionClone.TimeSinceNew
                mrptTechDirection.IsRemUnschedule = mrptTechDirectionClone.IsRemUnschedule
                mrptTechDirection.RemovalDate = mrptTechDirectionClone.RemovalDate
                mrptTechDirection.Position = mrptTechDirectionClone.Position
                mrptTechDirection.TimeSinceOverhaul = mrptTechDirectionClone.TimeSinceOverhaul
                ' mrptTechDirection.OHFreq = mrptTechDirectionClone.OHFreq
                ' mrptTechDirection.DueOn = mrptTechDirectionClone.DueOn
                'End
                ControlVisibilityForButtons()
                upnlDate.Update()
                DataBind()
                ReportMaintenanceDetails = Session("ReportMaintenanceDetails")
                dgMaintenanceActivityList.DataSource = ReportMaintenanceDetails
                dgMaintenanceActivityList.DataBind()
                Session("mrptTechDirection") = mrptTechDirection
                mrptTechDirectionClone = Nothing
            Catch ex As SqlException
                If ex.Number = 8145 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.ProcedureError, ex.Procedure, MsgBoxStyle.OkOnly, "")
                ElseIf ex.Number = 2627 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.DataBaseError, MSGBox.Message_text.Duplicate, ex.Procedure, MsgBoxStyle.OkOnly, "")
                Else
                    MSGBoxCtrl.show(MSGBox.Message_title.DatabaseException, MSGBox.Message_text.DatabaseException, ex.Message, MsgBoxStyle.OkOnly, "")
                End If
            Catch ex As Exception
                MSGBoxCtrl.show(MSGBox.Message_title.DatabaseException, MSGBox.Message_text.DatabaseException, ex.Message, MsgBoxStyle.OkOnly, "")
            End Try
        Else
            upnlValidations.Update()
        End If
    End Sub
    Protected Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        Dim da As New CSLA.Data.ObjectAdapter
        Dim ds As New dsTechDirection
        Dim mCompanyDetail As New CompanyDetail
        If AppSettings("ClientCode") = "SAA" Then
            myReport = New crptTechDirectionSauryaAirlines
        Else
            myReport = New crptTechDirection
        End If

        'Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
        '      mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
        '      mCompanyDetail.WebSite, "", mrptTechDirection.RemovalDateFormatted, "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", "", "", AppSettings("Logo"))
        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, _
              mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, _
              mCompanyDetail.WebSite, "", mrptTechDirection.RemovalDateFormatted, SearchStr2:=mModuleList.Item("TechnicalDirection").FormRevisionNo, SearchStr3:="", SearchStr4:="", SearchStr5:="", _
              ProductVersion:=AppSettings("Product Version"), SINote:=AppSettings("SINote"), SearchStr6:="", SearchStr7:="", SearchStr8:="", SearchStr9:="", SearchStr10:=AppSettings("Logo"))

        ds.Clear()
        ReportMaintenanceDetails = Session("ReportMaintenanceDetails")
        Dim mrptImage As rptImage = rptImage.GetImage(ds)
        da.Fill(ds, mrptImage)
        da.Fill(ds, mrptTechDirection)
        da.Fill(ds, Report)
        If ReportMaintenanceDetails.Count <= 0 Then 'If mrptTechDirection.TypeID = 1 or  Then
            ReportMaintenanceDetails.Add(New ReportMaintenanceDetail(Guid.Empty, DoneOnDate:=txtFromDate.Text))
        End If
        da.Fill(ds, ReportMaintenanceDetails)


        myReport.SetDataSource(ds)
        Session("CrystalReport") = myReport
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
        If DirectCast(ReportMaintenanceDetails(0), Flypal.ReportMaintenanceDetail).MonitorType = "" Then
            ReportMaintenanceDetails = New ReportMaintenanceDetailList
            Session("ReportMaintenanceDetails") = ReportMaintenanceDetails
        End If
    End Sub
    Private Sub btnBack_Click(sender As Object, e As System.EventArgs) Handles btnBack.Click
        RemoveSession()
        Response.Redirect(Request.QueryString("BackPage") & "?BackPage=" & Request.QueryString("BackPage1"))
    End Sub
    Private Sub cmbLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbLocation.SelectedIndexChanged
        mrptTechDirection.To = mLocationList(cmbLocation.SelectedIndex).Address
        txtTo.DataBind()
    End Sub
#End Region
    

  
  
End Class