Imports System.Linq
Imports System.Text

'Create By Utkarsh On 10-Nov-2011
Partial Class wfrptACAAReliabilityReport
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
    Public mModelList As ModelList
    Public mReliabilityFlyingHoursRecord As ReliabilityFlyingHoursRecord
    Public mReliabilityFlyingHoursRecordWithAircraft As ReliabilityFlyingHoursRecordWithAircraft
    Dim mReliabilityDefectReportedByPilot As ReliabilityDefectReportedByPilot
    Dim mrptReliabilityAircraftUtilization As rptReliabilityAircraftUtilization
    Dim mReliabilityFleetHoursCycles As ReliabilityFleetHoursCycles
    Dim mReliabilityFleetHoursCyclesForAllModels As ReliabilityFleetHoursCyclesForAllModels
    Dim mReliabilityOCComponentPrematureFailure As ReliabilityOCComponentPrematureFailure
    Dim mReliabilityLifedComponentPrematureFailure As ReliabilityLifedComponentPrematureFailure

    Public mReliabilityDistributionList As DistributionList
    Dim mrptMechanicalReliability As rptMechanicalReliability 'Added By Utkarsh ON 24-Apr-2013 FOR All-24042013-1
    Dim mDailyUtilizationGraphReport As DailyUtilizationGraphReport 'Added By Utkash ON 03-May-2013 FOR ALL03052013
    Dim mrptMonthlySnagCountATAWise As rptMonthlySnagCountATAWise


    Dim mMonthwiseAircraftCurrentStatus As MonthwiseAircraftCurrentStatus
    Dim mMonthwiseEngineStatus As MonthwiseEngineStatus
    Dim mMonthwiseAPUStatus As MonthwiseAPUStatus
    Dim mMonthwiseRemovedEngineStatus As MonthwiseRemovedEngineStatus
    Dim mMonthwiseRemovedAPUStatus As MonthwiseRemovedAPUStatus

    Dim mReliabilityMechanicalDefectRectification As ReliabilityDefectReportedByPilot 'Added By Utkarsh) ON 02-May-2013 FOR ALL2052013

    Public mFligthDelayAndCancellationList As FligthDelayAndCancellationList 'Added By Utkash ON 03-May-2013 FOR ALL03052013
    Dim mrptReliabilitySummary As rptReliabilitySummary 'Added By Utkarsh ON 05-Jun-2013 FOR ALL04062013
    'Added By Prashant ON 31-Jul-2013 FOR BA31072013
    Public mrptMonthlySnagCountATAWiseForMaintenanceDefect As rptMonthlySnagCountATAWiseForMaintenanceDefect
    Dim mMonthwiseAircraftOnGround As MonthwiseAircraftOnGround 'Added By Shweta on 30-July-2013 for BA31072013
    'Added By Vikrant On 31-July-2013 For BA31072013
    Private mrptReliabilityMonthlyATAWisePirepRate As rptReliabilityMonthlyATAWisePirepRate
    Private mrptReliabilityMonthlyATAWiseDefectRate As rptReliabilityMonthlyATAWiseMaintenanceDefectRate
    'End 

    Dim ChkModelIDs As String()
    Dim ModelIDs As New StringBuilder
    Dim ChkRegNos As String()
    Dim MachineIDs As New StringBuilder

    Dim ChkModelNames As String()
    Dim mModelNames As New StringBuilder
    Dim ChkMachineNames As String()
    Dim mMachineNames As New StringBuilder
    Dim mMachineNameValueList As MachineNameValueList

    Dim ChkAircraftModelIDs As String()
    Dim AircraftModelIDs As New StringBuilder
    Dim mModuleList As ModuleList 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 

#End Region

#Region "Business Methods"
    Private Sub SetSession()
        Session("mModelList") = mModelList
    End Sub
    Private Sub GetSession()
        mModelList = Session("mModelList")
    End Sub
    Private Sub MessageBoxResult()
        Dim Result1 As MsgBoxResult
        Result1 = MSGBoxCtrl.Result
        If Result1 > 0 Then
            Select Case Result1
                Case MsgBoxResult.Yes
                    If MSGBoxCtrl.Sender = "Delete" Then
                        Try
                            MSGBoxCtrl.show(MSGBox.Message_title.ReferenceDelete, MSGBox.Message_text.ReferenceDelete, "", MsgBoxStyle.OkOnly, "")
                            Exit Sub
                        Catch ex As SqlException

                        End Try
                    End If
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
    Private Sub AddAttributes()
        btnDisplay.Attributes("onclick") = "javascript: document.body.style.cursor = 'wait';"
    End Sub
#End Region

#Region "Data Binding"
    Private Sub SetCombo()
        If cmbYear.Items.Count = 0 Or cmbYear.SelectedValue = "" Then
            For i As Integer = -10 To 10
                cmbYear.Items.Add(DateAdd(DateInterval.Year, i, Today.Date).Year)
            Next
            cmbYear.SelectedIndex = 10
        End If

        For k As Integer = 1 To 12
            Dim mon As String = MonthName(k, False)
            cmbMonth.Items.Add(mon)
        Next
    End Sub
    Private Sub DataFieldBinding()
        'Commented and added by Shweta o 29-August-2013 for -ALL29082013-1
        'mModelList = ModelList.GetModelList(1, "", , , "(SELECT)")
        mModelList = ModelList.GetAirframeModelList()
        'end
        ListModel.DataSource = mModelList
        'cmbModel.DataSource = mModelList
        Session("mModelList") = mModelList
        'cmbModel.DataBind()
        ListModel.DataBind()

        mMachineNameValueList = MachineNameValueList.GetMachineList(Now.Date.ToString(AppSettings("DateFormat")), , , , , , , , , , True)
        ListRegNo.DataSource = mMachineNameValueList
        Session("mMachineNameValueList") = mMachineNameValueList
        ListRegNo.DataBind()

    End Sub
    Private Sub RemoveSession()
        Session.Remove("mModelList")
    End Sub
    Private Sub Display()
        lblSummary.Visible = True
        lblyear1.Visible = True
        lblModel1.Visible = True
    End Sub
    Private Sub SetValues()
        'For i As Integer = 0 To ListModel.Items.Count - 1
        '    ListModel.Items(i).Selected = True
        'Next

        'For i As Integer = 0 To ListRegNo.Items.Count - 1
        '    ListRegNo.Items(i).Selected = True
        'Next


        ChkModelIDs = (From c As System.Web.UI.WebControls.ListItem In ListModel.Items
                       Where c.Selected = True
                       Select (c.Value)).ToArray

        ChkModelNames = (From c As System.Web.UI.WebControls.ListItem In ListModel.Items
                      Where c.Selected = True
                      Select (c.Text)).ToArray
        If ChkModelIDs.Length > 0 Then


            ModelIDs.Append("<ModelID>")
            For i As Integer = 0 To ChkModelIDs.Count - 1
                ModelIDs.Append("<id>")
                ModelIDs.Append(ChkModelIDs(i))
                ModelIDs.Append("</id>")


                mModelNames.Append(ChkModelNames(i))
                mModelNames.Append(",")
                mModelNames.Append(" ")
            Next
            ModelIDs.Append("</ModelID>")


        End If


        ChkRegNos = (From c As System.Web.UI.WebControls.ListItem In ListRegNo.Items
                       Where c.Selected = True
                       Select (c.Value)).ToArray

        ChkMachineNames = (From c As System.Web.UI.WebControls.ListItem In ListRegNo.Items
                       Where c.Selected = True
                       Select (c.Text)).ToArray
        If ChkRegNos.Length > 0 Then
            MachineIDs.Append("<MachineID>")
            For i As Integer = 0 To ChkRegNos.Count - 1
                MachineIDs.Append("<id>")
                MachineIDs.Append(ChkRegNos(i))
                MachineIDs.Append("</id>")

                mMachineNames.Append(ChkMachineNames(i))
                mMachineNames.Append(",")
                mMachineNames.Append(" ")
            Next
            MachineIDs.Append("</MachineID>")


            Dim ModelListAsPerAircraft As ModelList = ModelList.GetAirframeModelList(, MachineIDs.ToString)  'Used for Distribution List

            ChkAircraftModelIDs = (From c As ModelList.ModelInfo In ModelListAsPerAircraft
                         Select (c.ID.ToString)).ToArray

            If ChkAircraftModelIDs.Length > 0 Then


                AircraftModelIDs.Append("<ModelID>")
                For i As Integer = 0 To ChkAircraftModelIDs.Count - 1
                    AircraftModelIDs.Append("<id>")
                    AircraftModelIDs.Append(ChkAircraftModelIDs(i))
                    AircraftModelIDs.Append("</id>")
                Next
                AircraftModelIDs.Append("</ModelID>")
            End If

        End If

        lblyear1.Text = "Month and Year : " & IIf((cmbYear.SelectedIndex >= 0 And cmbMonth.SelectedIndex >= 0), cmbMonth.SelectedItem.Text + " , " + cmbYear.SelectedItem.Text, "")
        '''lblModel1.Text = "Model : " & IIf(cmbModel.SelectedIndex > 0, cmbModel.SelectedItem.Text, "")
        lblModel1.Text = "Model : " & IIf(mModelNames.ToString = "", mMachineNames.ToString.Trim.TrimEnd(","), mModelNames.ToString.Trim.TrimEnd(","))

    End Sub
    Private Sub SetReport(Optional ByVal ByMail As Boolean = False)
        Try
            Dim da As New CSLA.Data.ObjectAdapter
            Dim mCompanyDetail As CompanyDetail
            Dim ReportName As String = String.Empty
            Dim ds As New dsKAReliability 'dsReliabilityFlyingHoursRecord
            ReportName = "Fleet Reliability Summary"
            SetValues()

          
            Dim mrptKAReliabilityUtilization As rptKAReliabilityUtilization
            Dim mrptKAReliabilityThreeMonthHours As rptKAReliabilityThreeMonthHours
            Dim mrptKAReliabilityHoursPerLandingGraph As rptKAReliabilityHoursPerLandingGraph

            Dim mrptKAReliabilityDispatchReliability As rptKAReliabilityDispatchReliability
            Dim mrptKAReliabilityDispatchReliabilityMonthWise As rptKAReliabilityDispatchReliabilityMonthWise
            Dim mrptKAReliabilityThreeMonthlyDispatchReliabilityMonthWise As rptKAReliabilityDispatchReliabilityMonthWise
            Dim mrptKAReliabilityDispatchReliabilityATAWise As rptKAReliabilityDispatchReliabilityATAWise

            mrptKAReliabilityUtilization = rptKAReliabilityUtilization.GetDailyUtilizationGraph(Guid.Empty, CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString)
            mrptKAReliabilityThreeMonthHours = rptKAReliabilityThreeMonthHours.GetList(mrptKAReliabilityUtilization, cmbMonth.SelectedIndex + 1, IIf(cmbMonth.SelectedIndex = 0, 12, cmbMonth.SelectedIndex), IIf(cmbMonth.SelectedIndex = 0 Or cmbMonth.SelectedIndex = 1, cmbMonth.SelectedIndex - 1 + 12, cmbMonth.SelectedIndex - 1), CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString)
            mrptKAReliabilityHoursPerLandingGraph = rptKAReliabilityHoursPerLandingGraph.GetGraph(Guid.Empty, CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString)

            'mrptKAReliabilityUtilization = rptKAReliabilityUtilization.GetDailyUtilizationGraph(Guid.Empty, CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString)
            'mrptKAReliabilityThreeMonthHours = rptKAReliabilityThreeMonthHours.GetList(mrptKAReliabilityUtilization, cmbMonth.SelectedIndex + 1, IIf(cmbMonth.SelectedIndex = 0, 12, cmbMonth.SelectedIndex), IIf(cmbMonth.SelectedIndex = 0 Or cmbMonth.SelectedIndex = 1, cmbMonth.SelectedIndex - 1 + 12, cmbMonth.SelectedIndex - 1), CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString)
            'mrptKAReliabilityHoursPerLandingGraph = rptKAReliabilityHoursPerLandingGraph.GetGraph(Guid.Empty, CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString)

            mrptKAReliabilityDispatchReliability = rptKAReliabilityDispatchReliability.GetList(Guid.Empty, CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString)
            mrptKAReliabilityDispatchReliabilityMonthWise = rptKAReliabilityDispatchReliabilityMonthWise.GetList(Guid.Empty, CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString, False)
            mrptKAReliabilityThreeMonthlyDispatchReliabilityMonthWise = rptKAReliabilityDispatchReliabilityMonthWise.GetList(Guid.Empty, CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString, True)
            mrptKAReliabilityDispatchReliabilityATAWise = rptKAReliabilityDispatchReliabilityATAWise.GetList(Guid.Empty, CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString, True)
            'mrptKAReliabilityThreeMonthlyDispatchReliabilityMonthWise = rptKAReliabilityDispatchReliabilityMonthWise.GetList(Guid.Empty, CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString, False)
            'End

            Dim mrptKAReliabilityAverageDailyUtilisation As rptKAReliabilityThreeMonthHours = rptKAReliabilityThreeMonthHours.GetAverageDailyUtilisation(mrptKAReliabilityUtilization, cmbMonth.SelectedIndex + 1, IIf(cmbMonth.SelectedIndex = 0, 12, cmbMonth.SelectedIndex), IIf(cmbMonth.SelectedIndex = 0 Or cmbMonth.SelectedIndex = 1, cmbMonth.SelectedIndex - 1 + 12, cmbMonth.SelectedIndex - 1), CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString)
            Dim mrptKAReliabilityTotalHoursLandings As rptKAReliabilityThreeMonthHours = rptKAReliabilityThreeMonthHours.GetTotalHoursAndLandings(mrptKAReliabilityUtilization, cmbMonth.SelectedIndex + 1, IIf(cmbMonth.SelectedIndex = 0, 12, cmbMonth.SelectedIndex), IIf(cmbMonth.SelectedIndex = 0 Or cmbMonth.SelectedIndex = 1, cmbMonth.SelectedIndex - 1 + 12, cmbMonth.SelectedIndex - 1), CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString)
            'Dim myReport = New crReliabilityReport  'crDailyUtilizationGraph
            'Dim myReport = New crptKAReliabilityMELCountPerAircraft
            'myReport = New crptKAReliabilityMELCountPerATA
            'myReport = New crptKAReliabilityMELAllCategories
            'myReport = New crptKAReliabilityExtensionCountPerAircraft
            'myReport = New crptKAReliabilityClosedMELAllCategories
            'myReport = New crptKAReliabilityAverageDurationPerAircraft

            Dim myReport = New crptKAReliabilityMainReport

            mrptReliabilityAircraftUtilization = rptReliabilityAircraftUtilization.GetReliabilityAircraftUtilization(Guid.Empty, CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString)
           
            mrptMonthlySnagCountATAWise = rptMonthlySnagCountATAWise.GetrptMonthlySnagCountATAWise(cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), , True, ModelIDs.ToString, MachineIDs.ToString)

           
            Dim StartDateM As New SmartDate
            Dim EndDateM As New SmartDate
            StartDateM.Text = CStr(DateAdd(DateInterval.Month, cmbMonth.SelectedIndex, DateSerial(CType(cmbYear.SelectedItem.Text, Integer), 1, 1)))
            EndDateM.Text = CStr(DateAdd("d", -1, DateAdd("m", 1, StartDateM.Date)))

          

            mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
            Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address, mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email, mCompanyDetail.WebSite, "", AppSettings("ClientCode"), IIf(chkUtilization.Checked, "True", "False"), IIf(chkDispatchReliability.Checked, "True", "False"), IIf(chkMEL.Checked, "True", "False"), IIf(ChkMAREP.Checked, "True", "False"), AppSettings("Product Version"), AppSettings("SINote"), IIf(ChkPIREP.Checked, "True", "False"), "", mModelNames.ToString.Trim.TrimEnd(","), mMachineNames.ToString.Trim.TrimEnd(",").TrimEnd(" "), cmbMonth.SelectedItem.Text + " , " + cmbYear.SelectedItem.Text, SearchStr11:=IIf(ChkRemoval.Checked, "True", "False"), SearchStr12:=IIf(chkExecutiveSummary.Checked, "True", "False"))


            'myReport.SetDataSource(ds)
            If ByMail = False Then
                If mrptReliabilityAircraftUtilization.TotalNoOfAircraft = 0 Then
                    MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
                    Exit Sub
                Else
                    RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 1220)
                End If
            End If
            If (ByMail = True And mrptReliabilityAircraftUtilization.TotalNoOfAircraft <= 0) Then
                SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, "There is no record for this search criteria.", "", _
                    Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                    ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                           SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))
                Exit Sub
            End If
            Dim mMELCategoryList As MELCategoryList = MELCategoryList.GetMELCategoryList("")
            Dim mrptKAReliabilityMEL As rptKAReliabilityMEL = rptKAReliabilityMEL.GetrptKAReliabilityMEL(, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString)
            Dim mrptKAReliabilityMELCountPerAircraft As rptKAReliabilityMELCount = rptKAReliabilityMELCount.GetReliabilityMELCountPerAircraft(mrptKAReliabilityMEL, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString, mMELCategoryList)
            Dim mrptKAReliabilityMELCountATA As rptKAReliabilityMELCount = rptKAReliabilityMELCount.GetReliabilityMELCountPerATA(mrptKAReliabilityMEL, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString, mMELCategoryList)
            Dim mrptKAReliabilityExtensionCountPerAircraft As rptKAReliabilityExtensionCount = rptKAReliabilityExtensionCount.GetReliabilityExtensionCountPerAircraft(mrptKAReliabilityMEL, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString, mMELCategoryList)
            Dim mrptKAReliabilityAverageDuration As rptKAReliabilityAverageDuration = rptKAReliabilityAverageDuration.GetReliabilityAverageDurationPerAircraft(cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString, mMELCategoryList)




            'Added By Shital On 23-Aug-2019
            Dim mrptKAReliabilityATAChapterExceedence As rptKAReliabilityATAChapterExceedence = rptKAReliabilityATAChapterExceedence.GetListOfPirepATAWiseExceedences(Guid.Empty, CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString)
            Dim mrptKAReliabilityATAChapterExceedenceForMonthForMreps As rptKARelibiltyATAChapterExceedenceForMonth = rptKARelibiltyATAChapterExceedenceForMonth.GetListOfPirepATAWiseExceedencesForMonth(mrptKAReliabilityATAChapterExceedence, Guid.Empty, CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString, False, True)
            Dim mrptKAReliabilityATAChapterExceedenceForThreeMonthForMreps As rptKAReliabilityATAChapterExceedenceForThreeMonth = rptKAReliabilityATAChapterExceedenceForThreeMonth.GetListOfPirepATAWiseExceedencesForThreeMonth(mrptKAReliabilityATAChapterExceedence, Guid.Empty, CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString, False, False, True)
            Dim mrptKAReliabilityAircraftExceedenceForMonthForMreps As rptKARelibiltyATAChapterExceedenceForMonth = rptKARelibiltyATAChapterExceedenceForMonth.GetListOfPirepATAWiseExceedencesForMonth(mrptKAReliabilityATAChapterExceedence, Guid.Empty, CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString, False, False)
            Dim mrptKAReliabilityAircraftExceedenceForThreeMonthForMreps As rptKAReliabilityATAChapterExceedenceForThreeMonth = rptKAReliabilityATAChapterExceedenceForThreeMonth.GetListOfPirepATAWiseExceedencesForThreeMonth(mrptKAReliabilityATAChapterExceedence, Guid.Empty, CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString, False, False, False) 'This Object is also used in ATA wise Removals

            Dim mrptKAReliabilityATAWisePirepsAnalysis As rptKAReliabilityATAWisePirepsAnalysis = rptKAReliabilityATAWisePirepsAnalysis.GetReliabilityATAWisePirepsAnalysis(mrptKAReliabilityMEL, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString)
            Dim mrptKAReliabilityATAWise3MonthsPirepsAnalysis As rptKAReliabilityATAWise3MonthsPirepsAnalysis = rptKAReliabilityATAWise3MonthsPirepsAnalysis.GetReliabilityATAWise3MonthsPirepsAnalysis(mrptKAReliabilityMEL, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString)
            Dim mrptKAReliabilityThreeMonthPirepMrepCount As rptKAReliabilityThreeMonthPirepMrepCount = rptKAReliabilityThreeMonthPirepMrepCount.GetList(Guid.Empty, CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString, False)

            Dim mrptKAReliabilityPirepListATAWise As rptKAReliabilityPirepListATAWise = rptKAReliabilityPirepListATAWise.GetListOfPirepATAWise(Guid.Empty, CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString, False)


            Dim mrptKAReliabilityATAChapterExceedenceForMonthForPireps As rptKARelibiltyATAChapterExceedenceForMonth = rptKARelibiltyATAChapterExceedenceForMonth.GetListOfPirepATAWiseExceedencesForMonth(mrptKAReliabilityATAChapterExceedence, Guid.Empty, CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString, True, True)
            Dim mrptKAReliabilityATAChapterExceedenceForThreeMonthForPireps As rptKAReliabilityATAChapterExceedenceForThreeMonth = rptKAReliabilityATAChapterExceedenceForThreeMonth.GetListOfPirepATAWiseExceedencesForThreeMonth(mrptKAReliabilityATAChapterExceedence, Guid.Empty, CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString, False, True, True) 'This Object is also used in ATA wise Removals


            Dim mrptKAReliabilityATAChapterExceedenceForMonth As rptKARelibiltyATAChapterExceedenceForMonth = rptKARelibiltyATAChapterExceedenceForMonth.GetListOfPirepATAWiseExceedencesForMonth(mrptKAReliabilityATAChapterExceedence, Guid.Empty, CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString, , True)
            Dim mrptKAReliabilityAircraftExceedenceForMonth As rptKARelibiltyATAChapterExceedenceForMonth = rptKARelibiltyATAChapterExceedenceForMonth.GetListOfPirepATAWiseExceedencesForMonth(mrptKAReliabilityATAChapterExceedence, Guid.Empty, CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString, , False)

            'Dim mrptKAReliabilityAircraftExceedenceForThreeMonthForRemovals As rptKAReliabilityATAChapterExceedenceForThreeMonth = rptKAReliabilityATAChapterExceedenceForThreeMonth.GetListOfPirepATAWiseExceedencesForThreeMonth(mrptKAReliabilityATAChapterExceedence, Guid.Empty, CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString, False, True, False)

            Dim mrptKAReliabilitySummaryofInformation As rptKAReliabilitySummaryofInformation = rptKAReliabilitySummaryofInformation.GetList(Guid.Empty, CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString)

            ds.Clear()
            Dim mrptImage As rptImage = rptImage.GetImage(ds, , "rptImage")
            da.Fill(ds, mrptImage)
            'Added By Shital On 23-Aug-2019
            da.Fill(ds, "rptKAReliabilityATAChapterExceedence", mrptKAReliabilityATAChapterExceedence)
            da.Fill(ds, "rptKAReliabilityATAChapterExceedenceForMonthForMreps", mrptKAReliabilityATAChapterExceedenceForMonthForMreps)
            da.Fill(ds, "rptKAReliabilityATAChapterExceedenceForThreeMonthForMreps", mrptKAReliabilityATAChapterExceedenceForThreeMonthForMreps)
            da.Fill(ds, "rptKAReliabilityAircraftExceedenceForMonthForMreps", mrptKAReliabilityAircraftExceedenceForMonthForMreps)
            da.Fill(ds, "rptKAReliabilityAircraftExceedenceForThreeMonthForMreps", mrptKAReliabilityAircraftExceedenceForThreeMonthForMreps)

            da.Fill(ds, "rptKAReliabilityATAWisePirepsAnalysis", mrptKAReliabilityATAWisePirepsAnalysis)
            da.Fill(ds, "rptKAReliabilityATAWise3MonthsPirepsAnalysis", mrptKAReliabilityATAWise3MonthsPirepsAnalysis)

            da.Fill(ds, "rptKAReliabilityPirepListATAWise", mrptKAReliabilityPirepListATAWise)
            da.Fill(ds, "rptKAReliabilityThreeMonthPirepMrepCount", mrptKAReliabilityThreeMonthPirepMrepCount)
            da.Fill(ds, "rptKAReliabilityATAChapterExceedenceForMonth", mrptKAReliabilityATAChapterExceedenceForMonthForPireps)
            da.Fill(ds, "rptKAReliabilityATAChapterExceedenceForThreeMonth", mrptKAReliabilityATAChapterExceedenceForThreeMonthForPireps)
            da.Fill(ds, "rptKAReliabilityAircraftExceedenceForThreeMonthForRemoval", mrptKAReliabilityAircraftExceedenceForThreeMonthForMreps)
            '------------------------------


            da.Fill(ds, "rptKAReliabilityMEL", mrptKAReliabilityMEL)
            da.Fill(ds, "MELCategoryList", mMELCategoryList)
            da.Fill(ds, "rptKAReliabilityMELCountPerAircraft", mrptKAReliabilityMELCountPerAircraft)
            da.Fill(ds, "rptKAReliabilityMELCountPerATA", mrptKAReliabilityMELCountATA)
            da.Fill(ds, "rptKAReliabilityExtensionCount", mrptKAReliabilityExtensionCountPerAircraft)
            da.Fill(ds, "rptKAReliabilityAverageDuration", mrptKAReliabilityAverageDuration)
            'Added by Vikrant On 18-Jul-2019 For KAM Air Reliability
            da.Fill(ds, mrptKAReliabilityUtilization)
            da.Fill(ds, mrptKAReliabilityThreeMonthHours)
            da.Fill(ds, mrptKAReliabilityHoursPerLandingGraph)
            da.Fill(ds, mrptKAReliabilityUtilization)
            da.Fill(ds, mrptKAReliabilityThreeMonthHours)
            da.Fill(ds, mrptKAReliabilityHoursPerLandingGraph)

            da.Fill(ds, mrptKAReliabilityDispatchReliability)
            da.Fill(ds, mrptKAReliabilityDispatchReliabilityMonthWise)
            da.Fill(ds, "rptKAReliabilityDispatchReliabilityMonthWiseCopyForThreeMonthlyData", mrptKAReliabilityThreeMonthlyDispatchReliabilityMonthWise)
            da.Fill(ds, mrptKAReliabilityDispatchReliabilityATAWise)
            'End
            da.Fill(ds, mrptKAReliabilityATAChapterExceedence)

            da.Fill(ds, "rptKAReliabilityATAChapterExceedenceForMonth", mrptKAReliabilityATAChapterExceedenceForMonth)
            da.Fill(ds, "rptKAReliabilityAircraftExceedenceForMonth", mrptKAReliabilityAircraftExceedenceForMonth)

            da.Fill(ds, "rptKAReliabilityAverageDailyUtilisation", mrptKAReliabilityAverageDailyUtilisation)
            da.Fill(ds, "rptKAReliabilityTotalHoursLandings", mrptKAReliabilityTotalHoursLandings)
            da.Fill(ds, "rptKAReliabilitySummaryofInformation", mrptKAReliabilitySummaryofInformation)

            da.Fill(ds, mrptMonthlySnagCountATAWise)
           
            da.Fill(ds, Report)

            myReport.SetDataSource(ds)

          
            Session("CrystalReport") = myReport
            If ByMail = False Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
            Else
                SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, " For " + lblyear1.Text + ", " + lblModel1.Text, "", _
                                          Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"), _
                                          ReportGeneratedBy:=Session("ReportGenratedBy"), _
                                           SmtpHost:=Session("SmtpHost"), SmtpPort:=Session("SmtpPort"), SmtpUser:=Session("SmtpUser"), SmtpPassword:=Session("SmtpPassword"))
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
    Public Sub CustomValidate(ByVal s As Object, ByVal e As ServerValidateEventArgs)
        Dim custValidate As CustomValidator
        custValidate = CType(s, CustomValidator)
        If custValidate.ControlToValidate = "cmbModel" Then
            'If cmbModel.SelectedIndex <= 0 Then
            '    custValidate.ErrorMessage = "Select the Model"
            '    e.IsValid = False
            'Else
            '    e.IsValid = True
            'End If
        End If
    End Sub
#End Region

#Region "Events"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'AddAttributes()
        If Not Page.IsPostBack Then
            SetCombo()
            DataFieldBinding()

        End If
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
        upnlCriteria.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If Page.IsValid Then
            SetReport(False)
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnByMail_Click(sender As Object, e As System.EventArgs) Handles btnByMail.Click
        If Page.IsValid Then
            'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
            ' Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail
            Session("UserEmailID") = mModuleList.Item("ACAAReliability").SendToMailID
            Session("UserCcEmailID") = mModuleList.Item("ACAAReliability").SendCCMailID
            Session("SmtpHost") = mModuleList.Item("ACAAReliability").SmtpHost
            Session("SmtpPort") = mModuleList.Item("ACAAReliability").SmtpPort
            Session("SmtpUser") = mModuleList.Item("ACAAReliability").SmtpUser
            Session("SmtpPassword") = mModuleList.Item("ACAAReliability").SmtpPassword
            '--------------------------
            Dim Str As String
            Str = "OpenByMaiWindow();"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OpenByMaiWindow", Str, True)
        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub hdnimgBtnSendMail_Click(sender As Object, e As System.EventArgs) Handles hdnimgBtnSendMail.Click
        Try
            Dim email As New Thread(Sub() SetReport(True))
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
    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        RemoveSession()
        Session("MiddleFrame") = ""
        Response.Redirect("Dashboard.aspx")
    End Sub
    Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
        MSGBoxCtrl.HideControl()
        MessageBoxResult()
    End Sub
#End Region

    Private Sub ChkAircraftwise_CheckedChanged(sender As Object, e As System.EventArgs) Handles ChkAircraftwise.CheckedChanged
        If ChkAircraftwise.Checked Then
            lblAircraft.Visible = True
            ListRegNo.Visible = True
            upnlsearch.Update()
        Else
            lblAircraft.Visible = False
            ListRegNo.Visible = False
            upnlsearch.Update()
        End If
    End Sub
End Class
