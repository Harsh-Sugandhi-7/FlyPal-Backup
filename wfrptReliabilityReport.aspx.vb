'Create By Utkarsh On 10-Nov-2011

Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Partial Class wfrptReliabilityReport
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
    Private mrptReliabilityMonthlyATAWisePirepDefectCount As rptReliabilityMonthlyATAWisePirepDefectCount
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
        lblModel1.Text = IIf(mModelNames.ToString = "", "Aircraft(s) : " + mMachineNames.ToString.Trim.TrimEnd(","), "Model(s) : " + mModelNames.ToString.Trim.TrimEnd(","))
    End Sub
    Private Sub SetReportSAA(Optional ByVal ByMail As Boolean = False)
        Try

            SetValues()
            Dim da As New CSLA.Data.ObjectAdapter
            Dim mCompanyDetail As CompanyDetail
            Dim ReportName As String = String.Empty
            Dim ds As New dsReliabilityReport
            Dim mrptImage As rptImage

            Dim StartDateM As SmartDate
            Dim EndDateM As SmartDate
            Dim tmpDate As Date = DateAdd(DateInterval.Month, -2, DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, 1))

            'StartDateM = New SmartDate(DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, 1), False)
            'EndDateM = New SmartDate(CStr(DateSerial(tmpDate.Year, tmpDate.Month, DateTime.DaysInMonth(tmpDate.Year, tmpDate.Month))), False)

            StartDateM = New SmartDate(CStr(DateSerial(tmpDate.Year, tmpDate.Month, 1)), False)
            EndDateM = New SmartDate(DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, DateTime.DaysInMonth(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1)), False)


            ReportName = "RELIABILITY REPORT"
            mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
            Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
                   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
                   mCompanyDetail.WebSite, "", AppSettings("ClientCode"), cmbYear.SelectedItem.Text, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", mModelNames.ToString.Trim.TrimEnd(","), mMachineNames.ToString.Trim.TrimEnd(","), SearchStr10:=AppSettings("Logo"), SearchStr11:=CDate(StartDateM.ToString).ToString("MMMM") + " - " + CDate(EndDateM.ToString).ToString("MMMM") + " " + CDate(EndDateM.Text).ToString("yyyy"))



            'Page 1 : First Page
            Dim myReport = New crSAAFirstPage
            ds.Clear()
            da.Fill(ds, "ReportData", Report)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport
            Dim PDFNo As Integer = 1
            Dim PDFNoChild As Integer = 1
            Dim tmp As Integer
            Dim a As New Random
            Dim pageCount As Integer = 0

            Dim pdfList As New System.Collections.ArrayList

            Dim MyFile1 = ""
            Dim myExportOption As CrystalDecisions.Shared.ExportOptions
            Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

            MyFile1 = "C:\Temp\" & "SAAFirstPage" & tmp & PDFNo.ToString & ".pdf"
            myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

            myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
            myDiskOption.DiskFileName = MyFile1
            myExportOption = myReport.ExportOptions
            With myExportOption
                .DestinationOptions = myDiskOption
                .ExportDestinationType = ExportDestinationType.DiskFile
                .ExportFormatType = ExportFormatType.PortableDocFormat
            End With
            myReport.Export()
            myReport.Close()
            myReport.Dispose()
            GC.Collect()


            pdfList.Add(MyFile1)
            PDFNo = PDFNo + 1



            ''''''Page 2 Distribution List
            Dim mDistributionList As DistributionList
            myReport = New crSAADistribution

            mDistributionList = DistributionList.GetDistributionList(Guid.Empty, , , , IIf(ModelIDs.ToString = "", AircraftModelIDs.ToString, ModelIDs.ToString))



            ds.Clear()
            da.Fill(ds, "DistributionList", mDistributionList)
            da.Fill(ds, "ReportData", Report)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            MyFile1 = "C:\Temp\" & "SAADistributionPage" & tmp & PDFNo.ToString & ".pdf"
            myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

            myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
            myDiskOption.DiskFileName = MyFile1
            myExportOption = myReport.ExportOptions
            With myExportOption
                .DestinationOptions = myDiskOption
                .ExportDestinationType = ExportDestinationType.DiskFile
                .ExportFormatType = ExportFormatType.PortableDocFormat
            End With
            myReport.Export()
            myReport.Close()
            myReport.Dispose()
            GC.Collect()


            pdfList.Add(MyFile1)
            PDFNo = PDFNo + 1




            ''''''Page 2 Intoduction
            myReport = New crSAAIntoduction




            ds.Clear()

            da.Fill(ds, "ReportData", Report)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            MyFile1 = "C:\Temp\" & "SAAIntoductionPage" & tmp & PDFNo.ToString & ".pdf"
            myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

            myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
            myDiskOption.DiskFileName = MyFile1
            myExportOption = myReport.ExportOptions
            With myExportOption
                .DestinationOptions = myDiskOption
                .ExportDestinationType = ExportDestinationType.DiskFile
                .ExportFormatType = ExportFormatType.PortableDocFormat
            End With
            myReport.Export()
            myReport.Close()
            myReport.Dispose()
            GC.Collect()


            pdfList.Add(MyFile1)
            PDFNo = PDFNo + 1





            ''''''Page 4 Quarterly Operation Statistics
            myReport = New crSAAReliabilityQuarterlyOperationStatistics

            Dim mQuarterlyOperationStatistics As rptSAAReliabilityQuarterlyOperationStatistics = rptSAAReliabilityQuarterlyOperationStatistics.GetList(StartDateM.ToString, EndDateM.ToString, ModelIDs.ToString, MachineIDs.ToString)


            ds.Clear()
            da.Fill(ds, "rptSAAReliabilityQuarterlyOperationStatistics", mQuarterlyOperationStatistics)
            da.Fill(ds, "ReportData", Report)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            MyFile1 = "C:\Temp\" & "SAAQuarterlyOperationStatisticsPage" & tmp & PDFNo.ToString & ".pdf"
            myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

            myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
            myDiskOption.DiskFileName = MyFile1
            myExportOption = myReport.ExportOptions
            With myExportOption
                .DestinationOptions = myDiskOption
                .ExportDestinationType = ExportDestinationType.DiskFile
                .ExportFormatType = ExportFormatType.PortableDocFormat
            End With
            myReport.Export()
            myReport.Close()
            myReport.Dispose()
            GC.Collect()


            pdfList.Add(MyFile1)
            PDFNo = PDFNo + 1



            ''''''Page 5 :    Major Component Statistics
            Dim mMonthwisePropellerStatus As MonthwisePropellerStatus
            Dim mMonthwiseRemovedPropellerStatus As MonthwiseRemovedPropellerStatus

            mMonthwiseEngineStatus = MonthwiseEngineStatus.GetMonthwiseEngineStatus(, "", CType(cmbYear.SelectedItem.Text, Integer), Today.Date.ToString, Month(CDate(EndDateM.ToString)), ModelIDs.ToString, MachineIDs.ToString)
            mMonthwiseRemovedEngineStatus = MonthwiseRemovedEngineStatus.GetMonthwiseRemoveEngineStatus(Guid.Empty.ToString, 0, 0, ModelIDStr:=ModelIDs.ToString, MachineIDStr:=MachineIDs.ToString, FromDate:=StartDateM.ToString, EndDate:=EndDateM.ToString)

            mMonthwiseAPUStatus = MonthwiseAPUStatus.GetMonthwiseAPUStatus(4, , "", CType(cmbYear.SelectedItem.Text, Integer), Today.Date.ToString, Month(CDate(EndDateM.ToString)), ModelIDs.ToString, MachineIDs.ToString)
            mMonthwiseRemovedAPUStatus = MonthwiseRemovedAPUStatus.GetMonthwiseRemoveAPUStatus(Guid.Empty.ToString, 0, 0, ModelIDs.ToString, MachineIDs.ToString, FromDate:=StartDateM.ToString, EndDate:=EndDateM.ToString)

            mMonthwisePropellerStatus = MonthwisePropellerStatus.GetMonthwisePropellerStatus(3, Guid.Empty.ToString, "", CType(cmbYear.SelectedItem.Text, Integer), Today.Date.ToString, Month(CDate(EndDateM.ToString)), ModelIDs.ToString, MachineIDs.ToString)
            mMonthwiseRemovedPropellerStatus = MonthwiseRemovedPropellerStatus.GetMonthwiseRemovePropellerStatus(Guid.Empty.ToString, 0, 0, ModelIDs.ToString, MachineIDs.ToString, FromDate:=StartDateM.ToString, EndDate:=EndDateM.ToString)

            myReport = New crSAAMajorComponentStatistics
            ds.Clear()
            da.Fill(ds, "MonthwiseEngineStatus", mMonthwiseEngineStatus)
            da.Fill(ds, "MonthwiseRemovedEngineStatus", mMonthwiseRemovedEngineStatus)
            da.Fill(ds, "MonthwiseAPUStatus", mMonthwiseAPUStatus)
            da.Fill(ds, "MonthwiseRemovedAPUStatus", mMonthwiseRemovedAPUStatus)
            da.Fill(ds, "MonthwisePropellerStatus", mMonthwisePropellerStatus)
            da.Fill(ds, "MonthwiseRemovedPropellerStatus", mMonthwiseRemovedPropellerStatus)
            da.Fill(ds, "ReportData", Report)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)




            Session("CrystalReport") = myReport

            MyFile1 = "C:\Temp\" & "SAAMajorComponentStatisticsPage" & tmp & PDFNo.ToString & ".pdf"
            myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)


            With myReport
                If mMonthwiseRemovedEngineStatus.Count = 0 Then
                    .DetailSection2.SectionFormat.EnableSuppress = True
                Else
                    .DetailSection4.SectionFormat.EnableSuppress = True
                End If

                If mMonthwiseAPUStatus.Count = 0 Then
                    .DetailSection5.SectionFormat.EnableSuppress = True
                End If
                If mMonthwiseRemovedAPUStatus.Count = 0 Then
                    .DetailSection6.SectionFormat.EnableSuppress = True
                Else
                    .DetailSection7.SectionFormat.EnableSuppress = True
                End If

                If mMonthwisePropellerStatus.Count = 0 Then
                    .DetailSection8.SectionFormat.EnableSuppress = True
                End If

                If mMonthwiseRemovedPropellerStatus.Count = 0 Then
                    .DetailSection9.SectionFormat.EnableSuppress = True
                Else
                    .DetailSection10.SectionFormat.EnableSuppress = True
                End If


            End With

            Session("CrystalReport") = myReport

            myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
            myDiskOption.DiskFileName = MyFile1
            myExportOption = myReport.ExportOptions
            With myExportOption
                .DestinationOptions = myDiskOption
                .ExportDestinationType = ExportDestinationType.DiskFile
                .ExportFormatType = ExportFormatType.PortableDocFormat
            End With
            myReport.Export()
            myReport.Close()
            myReport.Dispose()
            GC.Collect()


            pdfList.Add(MyFile1)
            PDFNo = PDFNo + 1

            ''''''Page 6 Average Aircraft Utilization
            myReport = New crSAAverageAircraftUtilization
            Dim tmpStartDateM As SmartDate = New SmartDate(DateAdd(DateInterval.Month, -22, DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, 1)), True)

            Dim mrptKAReliabilityUtilization As rptKAReliabilityUtilization = rptKAReliabilityUtilization.GetDailyUtilizationGraph(Guid.Empty, 0, 0, ModelIDs.ToString, MachineIDs.ToString, FromDate:=tmpStartDateM.ToString, EndDate:=EndDateM.ToString)

            '  Dim tmprptKAReliabilityUtilization = (From c In mrptKAReliabilityUtilization Order By c.Month Ascending).ToList
            Dim mSAAReliabilityAverageUtilizationGraph As SAAReliabilityAverageUtilizationGraph = SAAReliabilityAverageUtilizationGraph.GetUtilizationGraph(mrptKAReliabilityUtilization, FromDate:=tmpStartDateM.ToString, EndDate:=EndDateM.ToString)
            Dim tmpAverageUtilizationGraph = (From c In mSAAReliabilityAverageUtilizationGraph Order By c.Month Ascending).ToList
            ds.Clear()
            da.Fill(ds, "SAAReliabilityAverageUtilizationGraph", tmpAverageUtilizationGraph)

            da.Fill(ds, "ReportData", Report)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            MyFile1 = "C:\Temp\" & "rptSAAverageAircraftUtilization" & tmp & PDFNo.ToString & ".pdf"
            myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

            myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
            myDiskOption.DiskFileName = MyFile1
            myExportOption = myReport.ExportOptions
            With myExportOption
                .DestinationOptions = myDiskOption
                .ExportDestinationType = ExportDestinationType.DiskFile
                .ExportFormatType = ExportFormatType.PortableDocFormat
            End With
            myReport.Export()
            myReport.Close()
            myReport.Dispose()
            GC.Collect()


            pdfList.Add(MyFile1)
            PDFNo = PDFNo + 1




            ''''''Page 6 Average Aircraft Utilization : Cycles

            myReport = New crSAAverageAircraftUtilizationforCycles

            Dim mSAAReliabilityAverageUtilizationGraphforCycles As SAAReliabilityAverageUtilizationGraph = SAAReliabilityAverageUtilizationGraph.GetUtilizationCyclesGraph(mrptKAReliabilityUtilization, FromDate:=tmpStartDateM.ToString, EndDate:=EndDateM.ToString)
            tmpAverageUtilizationGraph = (From c In mSAAReliabilityAverageUtilizationGraphforCycles Order By c.Month Ascending).ToList
            ds.Clear()
            da.Fill(ds, "SAAReliabilityAverageUtilizationGraph", tmpAverageUtilizationGraph)

            da.Fill(ds, "ReportData", Report)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            MyFile1 = "C:\Temp\" & "rptSAAverageAircraftUtilization" & tmp & PDFNo.ToString & ".pdf"
            myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

            myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
            myDiskOption.DiskFileName = MyFile1
            myExportOption = myReport.ExportOptions
            With myExportOption
                .DestinationOptions = myDiskOption
                .ExportDestinationType = ExportDestinationType.DiskFile
                .ExportFormatType = ExportFormatType.PortableDocFormat
            End With
            myReport.Export()
            myReport.Close()
            myReport.Dispose()
            GC.Collect()


            pdfList.Add(MyFile1)
            PDFNo = PDFNo + 1





            ''Reliability Defect Reported By Pilot
            mReliabilityDefectReportedByPilot = ReliabilityDefectReportedByPilot.GetReliabilityDefectReportedByPilot(, 0, 0, True, , ModelIDs.ToString, MachineIDs.ToString,
                                                                                                                     StartDateM.ToString, EndDateM.ToString)
            If mReliabilityDefectReportedByPilot.Count > 0 Then

                myReport = New crReliabilityDefectReportedByPilot
                ds.Clear()
                da.Fill(ds, mReliabilityDefectReportedByPilot)
                da.Fill(ds, "ReportData", Report)
                mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
                da.Fill(ds, mrptImage)

                myReport.SetDataSource(ds)

                Session("CrystalReport") = myReport

                MyFile1 = "C:\Temp\" & "ReliabilityDefectReportedByPilot" & tmp & PDFNo.ToString & ".pdf"
                myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

                myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
                myDiskOption.DiskFileName = MyFile1
                myExportOption = myReport.ExportOptions
                With myExportOption
                    .DestinationOptions = myDiskOption
                    .ExportDestinationType = ExportDestinationType.DiskFile
                    .ExportFormatType = ExportFormatType.PortableDocFormat
                End With
                myReport.Export()
                myReport.Close()
                myReport.Dispose()
                GC.Collect()
                pdfList.Add(MyFile1)
                PDFNo = PDFNo + 1
            End If
            ''-----------------------------------------------------------------
            ''Monthly Snag Count ATAWise Graph
            mrptMonthlySnagCountATAWise = rptMonthlySnagCountATAWise.GetrptMonthlySnagCountATAWise(cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), , True, ModelIDs.ToString, MachineIDs.ToString)
            If mrptMonthlySnagCountATAWise.Count > 0 Then
                myReport = New crSAAMonthlySnagATAWise
                Dim TemprptMonthlySnagCountATAWise = (From c In mrptMonthlySnagCountATAWise Order By c.SortOrder).ToList

                ds.Clear()
                da.Fill(ds, "rptMonthlySnagCountATAWise", TemprptMonthlySnagCountATAWise)
                da.Fill(ds, "ReportData", Report)
                mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
                da.Fill(ds, mrptImage)

                myReport.SetDataSource(ds)

                Session("CrystalReport") = myReport

                MyFile1 = "C:\Temp\" & "MonthlySnagCountATAWise" & tmp & PDFNo.ToString & ".pdf"
                myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

                myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
                myDiskOption.DiskFileName = MyFile1
                myExportOption = myReport.ExportOptions
                With myExportOption
                    .DestinationOptions = myDiskOption
                    .ExportDestinationType = ExportDestinationType.DiskFile
                    .ExportFormatType = ExportFormatType.PortableDocFormat
                End With
                myReport.Export()
                myReport.Close()
                myReport.Dispose()
                GC.Collect()
                pdfList.Add(MyFile1)
                PDFNo = PDFNo + 1
            End If
            ''-----------------------------------------------------------------

            ''Reliability Maintenance Defect Rectification
            mReliabilityMechanicalDefectRectification = ReliabilityDefectReportedByPilot.GetReliabilityDefectReportedByPilot(Guid.Empty.ToString, 0, 0, False, , ModelIDs.ToString,
                                                                                                                             MachineIDs.ToString, FromDate:=StartDateM.ToString,
                                                                                                                             EndDate:=EndDateM.ToString)
            If mReliabilityMechanicalDefectRectification.Count > 0 Then
                myReport = New crReliabilityMaintenanceDefectRectification
                ds.Clear()
                da.Fill(ds, mReliabilityMechanicalDefectRectification)
                da.Fill(ds, "ReportData", Report)
                mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
                da.Fill(ds, mrptImage)

                myReport.SetDataSource(ds)

                Session("CrystalReport") = myReport

                MyFile1 = "C:\Temp\" & "ReliabilityMechanicalDefectRectification" & tmp & PDFNo.ToString & ".pdf"
                myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

                myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
                myDiskOption.DiskFileName = MyFile1
                myExportOption = myReport.ExportOptions
                With myExportOption
                    .DestinationOptions = myDiskOption
                    .ExportDestinationType = ExportDestinationType.DiskFile
                    .ExportFormatType = ExportFormatType.PortableDocFormat
                End With
                myReport.Export()
                myReport.Close()
                myReport.Dispose()
                GC.Collect()
                pdfList.Add(MyFile1)
                PDFNo = PDFNo + 1
                ''-----------------------------------------------------------------
            End If
            ''Monthly Snag Count ATAWise For Maintenance Defect Graph
            mrptMonthlySnagCountATAWiseForMaintenanceDefect = rptMonthlySnagCountATAWiseForMaintenanceDefect.GetrptMonthlySnagCountATAWise(cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), Guid.Empty.ToString, False, ModelIDs.ToString, MachineIDs.ToString)
            If mrptMonthlySnagCountATAWiseForMaintenanceDefect.Count > 0 Then
                myReport = New crSAAMonthlySnagCountATAWiseForMainteDefect
                Dim TemprptMonthlySnagCountATAWiseForMaintenanceDefect = (From c In mrptMonthlySnagCountATAWiseForMaintenanceDefect Order By c.SortOrder).ToList
                ds.Clear()
                da.Fill(ds, "rptMonthlySnagCountATAWiseForMaintenanceDefect", TemprptMonthlySnagCountATAWiseForMaintenanceDefect)
                da.Fill(ds, "ReportData", Report)
                mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
                da.Fill(ds, mrptImage)

                myReport.SetDataSource(ds)

                Session("CrystalReport") = myReport

                MyFile1 = "C:\Temp\" & "MonthlySnagCountATAWiseForMaintenanceDefect" & tmp & PDFNo.ToString & ".pdf"
                myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

                myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
                myDiskOption.DiskFileName = MyFile1
                myExportOption = myReport.ExportOptions
                With myExportOption
                    .DestinationOptions = myDiskOption
                    .ExportDestinationType = ExportDestinationType.DiskFile
                    .ExportFormatType = ExportFormatType.PortableDocFormat
                End With
                myReport.Export()
                myReport.Close()
                myReport.Dispose()
                GC.Collect()
                pdfList.Add(MyFile1)
                PDFNo = PDFNo + 1
                ''End of Added By Prashant 10-Jun-2022-----------------------------
            End If
            ''Reliability MEL Report 
            mReliabilityDefectReportedByPilot = ReliabilityDefectReportedByPilot.GetReliabilityDefectReportedByPilot(, 0, 0, False, True, ModelIDs.ToString, MachineIDs.ToString,
                                                                                                                    StartDateM.ToString, EndDateM.ToString)
            If mReliabilityDefectReportedByPilot.Count > 0 Then
                myReport = New crReliabilityMELReport
                ds.Clear()
                da.Fill(ds, "ReliabilityDefectReportedByPilot", mReliabilityDefectReportedByPilot)
                da.Fill(ds, "ReportData", Report)
                mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
                da.Fill(ds, mrptImage)

                myReport.SetDataSource(ds)

                Session("CrystalReport") = myReport

                MyFile1 = "C:\Temp\" & "SAAReliabilityMELReport " & tmp & PDFNo.ToString & ".pdf"
                myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

                myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
                myDiskOption.DiskFileName = MyFile1
                myExportOption = myReport.ExportOptions
                With myExportOption
                    .DestinationOptions = myDiskOption
                    .ExportDestinationType = ExportDestinationType.DiskFile
                    .ExportFormatType = ExportFormatType.PortableDocFormat
                End With
                myReport.Export()
                myReport.Close()
                myReport.Dispose()
                GC.Collect()
                pdfList.Add(MyFile1)
                PDFNo = PDFNo + 1
            End If

            ''''''Page 8 Fleet Reliability Summary
            myReport = New crReliabilitySummary

            mrptReliabilitySummary = rptReliabilitySummary.GetReliabilitySummary(Year(CDate(EndDateM.ToString)), Guid.Empty, Month(CDate(EndDateM.ToString)), ModelIDs.ToString, MachineIDs.ToString, IsSyncApplication:=mCompanyDetail.IsSyncApplication)


            ds.Clear()
            da.Fill(ds, "rptReliabilitySummary", mrptReliabilitySummary)
            da.Fill(ds, "ReportData", Report)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            MyFile1 = "C:\Temp\" & "rptReliabilitySummaryPage" & tmp & PDFNo.ToString & ".pdf"
            myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

            myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
            myDiskOption.DiskFileName = MyFile1
            myExportOption = myReport.ExportOptions
            With myExportOption
                .DestinationOptions = myDiskOption
                .ExportDestinationType = ExportDestinationType.DiskFile
                .ExportFormatType = ExportFormatType.PortableDocFormat
            End With
            myReport.Export()
            myReport.Close()
            myReport.Dispose()
            GC.Collect()
            pdfList.Add(MyFile1)
            PDFNo = PDFNo + 1
            ''-------------------------------------------------------------------------------

            ''Added By Prashant 10-Jun-2022------------------------------------
            ''''''Page 9 Reliability Monthly ATAWise Pirep Rate
            ''Pirep
            mrptReliabilityMonthlyATAWisePirepDefectCount = rptReliabilityMonthlyATAWisePirepDefectCount.GetMonthlyPirepRateATAWise(Year(CDate(EndDateM.ToString)), Guid.Empty,
                                                                                                                     Month(CDate(EndDateM.ToString)), True,
                                                                                                                     ModelIDs.ToString, MachineIDs.ToString)
            If mrptReliabilityMonthlyATAWisePirepDefectCount.Count > 0 Then
                myReport = New crSAAReliabilityMonthlyATAWisePirep
                ds.Clear()
                da.Fill(ds, "rptReliabilityMonthlyATAWisePirepRate", mrptReliabilityMonthlyATAWisePirepDefectCount)
                da.Fill(ds, "ReportData", Report)
                mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
                da.Fill(ds, mrptImage)

                myReport.SetDataSource(ds)

                Session("CrystalReport") = myReport

                MyFile1 = "C:\Temp\" & "ReliabilityMonthlyATAWisePirepRate" & tmp & PDFNo.ToString & ".pdf"
                myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

                myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
                myDiskOption.DiskFileName = MyFile1
                myExportOption = myReport.ExportOptions
                With myExportOption
                    .DestinationOptions = myDiskOption
                    .ExportDestinationType = ExportDestinationType.DiskFile
                    .ExportFormatType = ExportFormatType.PortableDocFormat
                End With
                myReport.Export()
                myReport.Close()
                myReport.Dispose()
                GC.Collect()
                pdfList.Add(MyFile1)
                PDFNo = PDFNo + 1
                ''-----------------------------------------------------------------
            End If

            ''''Component Analysis by Removal Rate :
            If mrptReliabilityMonthlyATAWisePirepDefectCount.Count > 0 Then
                myReport = New crSAAReliabilityMonthlyATAWiseCompRem
                ds.Clear()
                da.Fill(ds, "rptReliabilityMonthlyATAWisePirepRate", mrptReliabilityMonthlyATAWisePirepDefectCount)
                da.Fill(ds, "ReportData", Report)
                mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
                da.Fill(ds, mrptImage)

                myReport.SetDataSource(ds)

                Session("CrystalReport") = myReport

                MyFile1 = "C:\Temp\" & "ReliabilityMonthlyATAWisePirepRate" & tmp & PDFNo.ToString & ".pdf"
                myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

                myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
                myDiskOption.DiskFileName = MyFile1
                myExportOption = myReport.ExportOptions
                With myExportOption
                    .DestinationOptions = myDiskOption
                    .ExportDestinationType = ExportDestinationType.DiskFile
                    .ExportFormatType = ExportFormatType.PortableDocFormat
                End With
                myReport.Export()
                myReport.Close()
                myReport.Dispose()
                GC.Collect()
                pdfList.Add(MyFile1)
                PDFNo = PDFNo + 1
                ''-----------------------------------------------------------------
            End If



            ''Trend of PIREP/MAREP for different ATA Systems Graph
            Dim mSAATrendofPIREPORMAREPfordifferentATASystems As SAATrendofPIREPORMAREPfordifferentATASystems
            mSAATrendofPIREPORMAREPfordifferentATASystems = SAATrendofPIREPORMAREPfordifferentATASystems.GetSAATrendofPIREPORMAREPfordifferentATASystems(cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), , True, ModelIDStr:=ModelIDs.ToString, MachineIDStr:=MachineIDs.ToString)
            If mSAATrendofPIREPORMAREPfordifferentATASystems.Count > 0 Then
                myReport = New crSAATrendofPIREPfordifferentATASystemsGraph
                ds.Clear()
                da.Fill(ds, "SAATrendofPIREPORMAREPfordifferentATASystems", mSAATrendofPIREPORMAREPfordifferentATASystems)
                da.Fill(ds, "ReportData", Report)
                mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
                da.Fill(ds, mrptImage)

                myReport.SetDataSource(ds)

                Session("CrystalReport") = myReport

                MyFile1 = "C:\Temp\" & "SAATrendofPIREPfordifferentATASystems" & tmp & PDFNo.ToString & ".pdf"
                myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

                myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
                myDiskOption.DiskFileName = MyFile1
                myExportOption = myReport.ExportOptions
                With myExportOption
                    .DestinationOptions = myDiskOption
                    .ExportDestinationType = ExportDestinationType.DiskFile
                    .ExportFormatType = ExportFormatType.PortableDocFormat
                End With
                myReport.Export()
                myReport.Close()
                myReport.Dispose()
                GC.Collect()
                pdfList.Add(MyFile1)
                PDFNo = PDFNo + 1
            End If

            ''''''Page 14 Technical Delay / Cancellation / Diversion Summary :
            myReport = New crSAADelayCancellationPage

            mFligthDelayAndCancellationList = FligthDelayAndCancellationList.GetFlightDCList(Guid.Empty, StartDateM.Text, EndDateM.Text, False, False, True, True, Guid.Empty.ToString, ModelIDs.ToString, MachineIDs.ToString)
            If mFligthDelayAndCancellationList.Count > 0 Then
                Report = New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
                   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
                   mCompanyDetail.WebSite, "", AppSettings("ClientCode"), "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", mModelNames.ToString.Trim.TrimEnd(","), mMachineNames.ToString.Trim.TrimEnd(","), SearchStr10:=AppSettings("Logo"), SearchStr11:=CDate(StartDateM.ToString).ToString("MMMM") + " - " + CDate(EndDateM.ToString).ToString("MMMM") + " " + CDate(EndDateM.Text).ToString("yyyy"))


                ds.Clear()
                da.Fill(ds, "FligthDelayAndCancellationList", mFligthDelayAndCancellationList)
                da.Fill(ds, "ReportData", Report)
                mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
                da.Fill(ds, mrptImage)

                myReport.SetDataSource(ds)

                Session("CrystalReport") = myReport

                MyFile1 = "C:\Temp\" & "rptReliabilitySummaryPage" & tmp & PDFNo.ToString & ".pdf"
                myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

                myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
                myDiskOption.DiskFileName = MyFile1
                myExportOption = myReport.ExportOptions
                With myExportOption
                    .DestinationOptions = myDiskOption
                    .ExportDestinationType = ExportDestinationType.DiskFile
                    .ExportFormatType = ExportFormatType.PortableDocFormat
                End With
                myReport.Export()
                myReport.Close()
                myReport.Dispose()
                GC.Collect()


                pdfList.Add(MyFile1)
                PDFNo = PDFNo + 1
            End If


            ''''''Page 15 Component Change List :
            myReport = New crSAAReliabilityComponentChangeList

            Dim mSAAReliabilityComponentChangeList As SAAReliabilityComponentChangeList = SAAReliabilityComponentChangeList.GetComponentChangeList(StartDateM.Text, EndDateM.Text, ModelIDs.ToString, MachineIDs.ToString)
            If mSAAReliabilityComponentChangeList.Count > 0 Then
                Report = New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
                   mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
                   mCompanyDetail.WebSite, "", AppSettings("ClientCode"), "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", mModelNames.ToString.Trim.TrimEnd(","), mMachineNames.ToString.Trim.TrimEnd(","), SearchStr10:=AppSettings("Logo"), SearchStr11:=CDate(StartDateM.ToString).ToString("MMMM") + " - " + CDate(EndDateM.ToString).ToString("MMMM") + " " + CDate(EndDateM.Text).ToString("yyyy"))


                ds.Clear()
                da.Fill(ds, "SAAReliabilityComponentChangeList", mSAAReliabilityComponentChangeList)
                da.Fill(ds, "ReportData", Report)
                mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
                da.Fill(ds, mrptImage)

                myReport.SetDataSource(ds)

                Session("CrystalReport") = myReport

                MyFile1 = "C:\Temp\" & "rptReliabilityComponentChangeListPage" & tmp & PDFNo.ToString & ".pdf"
                myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

                myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
                myDiskOption.DiskFileName = MyFile1
                myExportOption = myReport.ExportOptions
                With myExportOption
                    .DestinationOptions = myDiskOption
                    .ExportDestinationType = ExportDestinationType.DiskFile
                    .ExportFormatType = ExportFormatType.PortableDocFormat
                End With
                myReport.Export()
                myReport.Close()
                myReport.Dispose()
                GC.Collect()


                pdfList.Add(MyFile1)
                PDFNo = PDFNo + 1
            End If




            '''''END: Merge ALL reports
            Dim MergedPath As String = "C:\Temp\" & "temp_myMergedPdf.pdf"
            Dim MergedPath_WM As String = "C:\Temp\" & "temp_myMergedPdf_WM.pdf"

            Dim filesByte As New List(Of Byte())()



            For Each file__1 As String In pdfList 'files
                filesByte.Add(File.ReadAllBytes(file__1))
            Next

            File.WriteAllBytes(MergedPath, Flypal.PDFMergers.MergeFiles(filesByte))


            Dim INTRODUCTION As Integer = getPageNoBySpecificText(1, MergedPath, "INTRODUCTION") + 1
            Dim QuarterlyOperationStatistics As Integer = getPageNoBySpecificText(1, MergedPath, "Quarterly Operation Statistics") + 1 'Quarterly Operation Statistics
            Dim MajorComponentStatistics As Integer = getPageNoBySpecificText(1, MergedPath, "Major Component Statistics") + 1 'Major Component Statistics
            Dim AverageAircraftUtilization As Integer = getPageNoBySpecificText(1, MergedPath, "Average Aircraft Utilization : ") + 1 'Average Aircraft Utilization
            Dim FleetReliabilitySummary As Integer = getPageNoBySpecificText(1, MergedPath, "Fleet Reliability Summary : ") + 1 'Fleet Reliability Summary
            Dim DelayCancellation As Integer = getPageNoBySpecificText(1, MergedPath, "Technical Delay / Cancellation / Diversion Summary : ") + 1
            Dim ComponentChangeList As Integer = getPageNoBySpecificText(1, MergedPath, "Component Change List : ") + 1
            Dim PIREPSummary As Integer = getPageNoBySpecificText(1, MergedPath, "PIREPs / MAREPs Summary : ") + 1 ''PIREPs / MAREPs Summary :
            Dim ComponentAnalysisbyRemovalRate As Integer = getPageNoBySpecificText(1, MergedPath, "Component Analysis by Removal Rate : ") + 1    'Component Analysis by Removal Rate
            Dim TECHNICAL_DEPARTMENT As Integer = getPageNoBySpecificText(1, MergedPath, "PIREPS/ MAREPS/MEL : ") + 1
            Dim Trend_Of_Pireps As Integer = getPageNoBySpecificText(1, MergedPath, "Trend of PIREP/MAREP for different ATA Systems : ") + 1

            '''''''''''Page 3 INDEX

            myReport = New crSAAIndexPage

            Report = New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
                 mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
                 mCompanyDetail.WebSite, "", AppSettings("ClientCode"), "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", mModelNames.ToString.Trim.TrimEnd(","), mMachineNames.ToString.Trim.TrimEnd(","), SearchStr10:=AppSettings("Logo"), SearchStr11:=CDate(StartDateM.ToString).ToString("MMMM") + " - " + CDate(EndDateM.ToString).ToString("MMMM") + " " + CDate(EndDateM.Text).ToString("yyyy"), SearchStr14:=INTRODUCTION.ToString, SearchStr15:=QuarterlyOperationStatistics.ToString, SearchStr16:=MajorComponentStatistics.ToString, SearchStr17:=AverageAircraftUtilization.ToString, SearchStr18:=FleetReliabilitySummary.ToString, SearchStr19:=DelayCancellation.ToString, SearchStr20:=Trend_Of_Pireps.ToString, SearchStr21:=ComponentChangeList.ToString, SearchStr22:=PIREPSummary.ToString, SearchStr23:=ComponentAnalysisbyRemovalRate.ToString, SearchStr24:=TECHNICAL_DEPARTMENT.ToString)

            ds.Clear()

            da.Fill(ds, "ReportData", Report)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            MyFile1 = "C:\Temp\" & "ESRIndex" & tmp & PDFNo.ToString & ".pdf"
            myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

            myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
            myDiskOption.DiskFileName = MyFile1
            myExportOption = myReport.ExportOptions
            With myExportOption
                .DestinationOptions = myDiskOption
                .ExportDestinationType = ExportDestinationType.DiskFile
                .ExportFormatType = ExportFormatType.PortableDocFormat
            End With
            myReport.Export()
            myReport.Close()
            myReport.Dispose()
            GC.Collect()


            MergedPath = "C:\Temp\" & "temp_myMergedPdfF.pdf"
            MergedPath_WM = "C:\Temp\" & "ReliabilityReport.pdf"

            filesByte = New List(Of Byte())()


            Dim i As Integer = 1
            For Each file__1 As String In pdfList 'files
                filesByte.Add(File.ReadAllBytes(file__1))
                i = i + 1
                If i = 3 Then
                    filesByte.Add(File.ReadAllBytes(MyFile1))  'First Index Page
                End If
            Next

            File.WriteAllBytes(MergedPath, Flypal.PDFMergers.MergeFiles(filesByte))




            'AddWatermarkText(MergedPath, MergedPath_WM, mnWO.WONumber, , , iTextSharp.text.BaseColor.GRAY, , 0.0, pageCount)
            AddWatermarkText(MergedPath, MergedPath_WM, "Page ", , , iTextSharp.text.BaseColor.BLACK, , 0.0, pageCount) 'Added on 24-Jun-2019
            ''//********************************************Set Sessions*********************************************************//
            Session("CrystalReport") = MergedPath_WM
            Session("PrintReportWithAttachment") = "True"

            '//*******************************************Delete created file*********************************************************//

            'Commented and Added by Saylee on 2-Dec-2014
            ' Dim MyFile, MyFile_Ext As String
            'For j As Integer = 1 To PDFNo - 1
            '    MyFile = "C:\Temp\" & WONo & j.ToString & ".pdf"
            '    MyFile_Ext = "C:\Temp\" & WONo & j.ToString & "_Ext" & ".pdf"

            '    System.IO.File.Delete(MyFile)
            '    System.IO.File.Delete(MyFile_Ext)
            'Next

            Dim DeleteThis As String = "ESR"
            Dim Files As String() = Directory.GetFiles("C:\Temp\")

            For Each file__1 As String In Files
                If file__1.ToUpper().Contains(DeleteThis.ToUpper()) Then
                    File.Delete(file__1)
                End If
            Next
            'End

            RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 1220)

            Session("CrystalReport") = MergedPath_WM

            If ByMail = False Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
            Else
                SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, " For " + lblyear1.Text + ", " + lblModel1.Text, "",
                                          Session("ToSendMailIDs"), Session("CcSendMailIDs"), MergedPath_WM, True, Remark:=Session("SendMailRemark"),
                                          ReportGeneratedBy:=Session("ReportGenratedBy"),
                    SmtpHost:=mModuleList.Item("Reliability").SmtpHost, SmtpPort:=mModuleList.Item("Reliability").SmtpPort,
                    SmtpUser:=mModuleList.Item("Reliability").SmtpUser, SmtpPassword:=mModuleList.Item("Reliability").SmtpPassword)
            End If



        Catch ex As Exception

        End Try
    End Sub
    Private Sub SetReport(Optional ByVal ByMail As Boolean = False)
        Try
            Dim da As New CSLA.Data.ObjectAdapter
            Dim mCompanyDetail As CompanyDetail
            Dim ReportName As String = String.Empty
            Dim ds As New dsReliabilityReport 'dsReliabilityFlyingHoursRecord
            'dsDailyUtilizationGraph   '
            ReportName = "Fleet Reliability Summary"

            mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

            SetValues()

            'Added by utkarsh on 10-dec-2013
            Dim mMonthwisePropellerStatus As MonthwisePropellerStatus
            Dim mMonthwiseRemovedPropellerStatus As MonthwiseRemovedPropellerStatus
            'End
            Dim mReliabilityMELReport As ReliabilityDefectReportedByPilot 'Added By Utkarsh ON 06-Jan-2014 FOR ALL03012014


            Dim myReport = New crReliabilityReport  'crDailyUtilizationGraph
            ' Dim myReport = New crDailyUtilizationGraph


            mReliabilityFlyingHoursRecord = ReliabilityFlyingHoursRecord.GetReliabilityFlyingHoursRecord(, Today.Date.ToString, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString)
            mReliabilityFlyingHoursRecordWithAircraft = ReliabilityFlyingHoursRecordWithAircraft.GetReliabilityFlyingHoursRecordWithAircraft(, Today.Date.ToString, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString)
            'Added By Utkarsh(IsPireps criteria) ON 02-May-2013 FOR ALL2052013
            mReliabilityDefectReportedByPilot = ReliabilityDefectReportedByPilot.GetReliabilityDefectReportedByPilot(, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), True, , ModelIDs.ToString, MachineIDs.ToString)
            'End
            mrptReliabilityAircraftUtilization = rptReliabilityAircraftUtilization.GetReliabilityAircraftUtilization(Guid.Empty, CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString)
            mReliabilityFleetHoursCycles = ReliabilityFleetHoursCycles.GetReliabilityFlyingHoursRecord(, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString)
            mReliabilityFleetHoursCyclesForAllModels = ReliabilityFleetHoursCyclesForAllModels.GetReliabilityFlyingHoursRecord(cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer))
            mReliabilityOCComponentPrematureFailure = ReliabilityOCComponentPrematureFailure.GetReliabilityOCComponentPrematureFailure(, cmbMonth.SelectedIndex + 1, cmbYear.SelectedItem.Value, ModelIDs.ToString, MachineIDs.ToString)
            mReliabilityLifedComponentPrematureFailure = ReliabilityLifedComponentPrematureFailure.GetReliabilityLifedComponentPrematureFailure(, cmbMonth.SelectedIndex + 1, cmbYear.SelectedItem.Value, ModelIDs.ToString, MachineIDs.ToString)

            '''''Added by Saylee on All-23042013 to show Distribution list
            mReliabilityDistributionList = DistributionList.GetDistributionList(Guid.Empty, , , , IIf(ModelIDs.ToString = "", AircraftModelIDs.ToString, ModelIDs.ToString))
            '''''Added By Utkarsh ON 24-Apr-2013 FOR All-24042013-1
            mrptMechanicalReliability = rptMechanicalReliability.GetMechanicalReliability(Guid.Empty, CInt(cmbYear.SelectedItem.Text), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString)
            '''''End

            '''''Added By Utkash ON 03-May-2013 FOR ALL03052013
            mDailyUtilizationGraphReport = DailyUtilizationGraphReport.GetDailyUtilizationGraph(Guid.Empty, CInt(cmbYear.SelectedItem.Text), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString)
            '''''End
            mrptMonthlySnagCountATAWise = rptMonthlySnagCountATAWise.GetrptMonthlySnagCountATAWise(cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), , True, ModelIDs.ToString, MachineIDs.ToString)

            '''''Added By Shweta ON 27-May-2013 FOR ALL03052013
            mMonthwiseAircraftCurrentStatus = MonthwiseAircraftCurrentStatus.GetMonthwiseAircraftCurrentStatus(, cmbMonth.SelectedItem.Text, CType(cmbYear.SelectedItem.Text, Integer), Today.Date.ToString, cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString)
            mMonthwiseEngineStatus = MonthwiseEngineStatus.GetMonthwiseEngineStatus(, cmbMonth.SelectedItem.Text, CType(cmbYear.SelectedItem.Text, Integer), Today.Date.ToString, cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString)
            '''''changed by utkarsh on 10-dec-2013
            mMonthwiseAPUStatus = MonthwiseAPUStatus.GetMonthwiseAPUStatus(4, , cmbMonth.SelectedItem.Text, CType(cmbYear.SelectedItem.Text, Integer), Today.Date.ToString, cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString)
            mMonthwiseRemovedEngineStatus = MonthwiseRemovedEngineStatus.GetMonthwiseRemoveEngineStatus(Guid.Empty.ToString, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString)
            mMonthwiseRemovedAPUStatus = MonthwiseRemovedAPUStatus.GetMonthwiseRemoveAPUStatus(Guid.Empty.ToString, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString)
            'Added by utkarsh on 10-dec-2013
            mMonthwisePropellerStatus = MonthwisePropellerStatus.GetMonthwisePropellerStatus(3, Guid.Empty.ToString, cmbMonth.SelectedItem.Text, CType(cmbYear.SelectedItem.Text, Integer), Today.Date.ToString, cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString)
            mMonthwiseRemovedPropellerStatus = MonthwiseRemovedPropellerStatus.GetMonthwiseRemovePropellerStatus(Guid.Empty.ToString, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString)
            ''''End
            ''''Added By Utkarsh(IsPireps criteria) ON 02-May-2013 FOR ALL2052013
            mReliabilityMechanicalDefectRectification = ReliabilityDefectReportedByPilot.GetReliabilityDefectReportedByPilot(Guid.Empty.ToString, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), False, , ModelIDs.ToString, MachineIDs.ToString)
            ''''End
            ''''Added By Utkash ON 03-May-2013 FOR ALL03052013
            Dim StartDateM As New SmartDate
            Dim EndDateM As New SmartDate
            StartDateM.Text = CStr(DateAdd(DateInterval.Month, cmbMonth.SelectedIndex, DateSerial(CType(cmbYear.SelectedItem.Text, Integer), 1, 1)))
            EndDateM.Text = CStr(DateAdd("d", -1, DateAdd("m", 1, StartDateM.Date)))

            mFligthDelayAndCancellationList = FligthDelayAndCancellationList.GetFlightDCList(Guid.Empty, StartDateM.Text, EndDateM.Text, True, True, True, True, Guid.Empty.ToString, ModelIDs.ToString, MachineIDs.ToString, IsSyncApplication:=mCompanyDetail.IsSyncApplication)
            ''''End

            ''''Added By Utkarsh ON 05-Jun-2013 FOR ALL04062013
            mrptReliabilitySummary = rptReliabilitySummary.GetReliabilitySummary(CInt(cmbYear.SelectedItem.Text), Guid.Empty, cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString, IsSyncApplication:=mCompanyDetail.IsSyncApplication)
            ''''End

            ''''Added By Prashant ON 31-Jul-2013 FOR BA31072013
            mrptMonthlySnagCountATAWiseForMaintenanceDefect = rptMonthlySnagCountATAWiseForMaintenanceDefect.GetrptMonthlySnagCountATAWise(cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), Guid.Empty.ToString, False, ModelIDs.ToString, MachineIDs.ToString)
            ''''End
            ''''Added By Shweta ON 31-Jul-2013 FOR BAL31072013
            mMonthwiseAircraftOnGround = MonthwiseAircraftOnGround.GetMontMonthwiseAircraftOnGround(EndDateM.Text, , Guid.Empty.ToString, ModelIDs.ToString, MachineIDs.ToString)

            'Added By Vikrant On 31-July-2013 For BA31072013
            mrptReliabilityMonthlyATAWisePirepRate = rptReliabilityMonthlyATAWisePirepRate.GetMonthlyPirepRateATAWise(CType(cmbYear.SelectedItem.Text, Integer), Guid.Empty, cmbMonth.SelectedIndex + 1, True, ModelIDs.ToString, MachineIDs.ToString)

            mrptReliabilityMonthlyATAWiseDefectRate = rptReliabilityMonthlyATAWiseMaintenanceDefectRate.GetMonthlyDefectRateATAWise(CType(cmbYear.SelectedItem.Text, Integer), Guid.Empty, cmbMonth.SelectedIndex + 1, False, ModelIDs.ToString, MachineIDs.ToString)

            ''' 'End
            ''' 'Added By Utkarsh ON 06-Jan-2014 FOR ALL03012014
            mReliabilityMELReport = ReliabilityDefectReportedByPilot.GetReliabilityDefectReportedByPilot(, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), False, True, ModelIDs.ToString, MachineIDs.ToString)
            'End

            Dim mReliabilityRepeatitiveDefectList As ReliabilityRepeatitiveDefectList
            mReliabilityRepeatitiveDefectList = ReliabilityRepeatitiveDefectList.GetReliabilityRepeatitiveDefectList(, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString)



            Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
                     mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
                     mCompanyDetail.WebSite, "", AppSettings("ClientCode"), "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", mModelNames.ToString.Trim.TrimEnd(","), mMachineNames.ToString.Trim.TrimEnd(","), cmbMonth.SelectedItem.Text + " , " + cmbYear.SelectedItem.Text)


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
                SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, "There is no record for this search criteria.", "",
                    Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"),
                    ReportGeneratedBy:=Session("ReportGenratedBy"),
                    SmtpHost:=mModuleList.Item("Reliability").SmtpHost, SmtpPort:=mModuleList.Item("Reliability").SmtpPort,
                    SmtpUser:=mModuleList.Item("Reliability").SmtpUser, SmtpPassword:=mModuleList.Item("Reliability").SmtpPassword)
                Exit Sub
            End If
            Dim mrptImage As rptImage = rptImage.GetImage(ds)
            ds.Clear()
            da.Fill(ds, mReliabilityFlyingHoursRecord)
            da.Fill(ds, mReliabilityFlyingHoursRecordWithAircraft)
            da.Fill(ds, mReliabilityDefectReportedByPilot)
            da.Fill(ds, mrptReliabilityAircraftUtilization)
            da.Fill(ds, mReliabilityFleetHoursCycles)
            da.Fill(ds, mReliabilityFleetHoursCyclesForAllModels)
            da.Fill(ds, mReliabilityOCComponentPrematureFailure)
            da.Fill(ds, mReliabilityLifedComponentPrematureFailure)
            da.Fill(ds, mReliabilityDistributionList) 'Added by Saylee on All-23042013 to show Distribution list
            da.Fill(ds, mrptMechanicalReliability) 'Added By Utkarsh ON 24-Apr-2013 FOR All-24042013-1
            '''''''''Added By Utkash ON 03-May-2013 FOR ALL03052013
            da.Fill(ds, mDailyUtilizationGraphReport)
            da.Fill(ds, mrptMonthlySnagCountATAWise)
            da.Fill(ds, mMonthwiseAircraftCurrentStatus)
            da.Fill(ds, mMonthwiseEngineStatus)
            da.Fill(ds, mMonthwiseAPUStatus)
            da.Fill(ds, mFligthDelayAndCancellationList)

            da.Fill(ds, mrptImage)
            da.Fill(ds, mMonthwiseRemovedEngineStatus)  'Added By Shweta ON 27-May-2013 FOR ALL03052013
            da.Fill(ds, mMonthwiseRemovedAPUStatus)     'Added By Shweta ON 27-May-2013 FOR ALL03052013
            da.Fill(ds, mrptReliabilitySummary)         'Added By Utkarsh ON 05-Jun-2013 FOR ALL04062013
            da.Fill(ds, mrptMonthlySnagCountATAWiseForMaintenanceDefect) 'Added By Prashant ON 31-Jul-2013 FOR BAL31072013
            da.Fill(ds, mMonthwiseAircraftOnGround) 'Added By Shweta ON 31-Jul-2013 FOR BAL31072013
            'Added By Vikrant On 31-July-2013 For BA31072013
            da.Fill(ds, mrptReliabilityMonthlyATAWisePirepRate)
            da.Fill(ds, mrptReliabilityMonthlyATAWiseDefectRate)
            'End
            'Added by utkarsh on 10-dec-2013
            da.Fill(ds, mMonthwisePropellerStatus)
            da.Fill(ds, mMonthwiseRemovedPropellerStatus)
            '''''''''End
            da.Fill(ds, mReliabilityMechanicalDefectRectification)
            da.Fill(ds, mReliabilityMELReport) 'Added By Utkarsh ON 06-Jan-2014 FOR ALL03012014
            da.Fill(ds, Report)
            da.Fill(ds, "ReliabilityRepeatitiveDefectList", mReliabilityRepeatitiveDefectList)
            myReport.SetDataSource(ds)


            With myReport
                If mReliabilityFleetHoursCyclesForAllModels.Count = 0 Then
                    .Section7.SectionFormat.EnableSuppress = True
                End If
                If mReliabilityFlyingHoursRecordWithAircraft.Count = 0 Then
                    .Section12.SectionFormat.EnableSuppress = True
                End If
                'Added By Utkarsh ON 02-May-2013 FOR ALL2052013
                If Not mReliabilityDefectReportedByPilot.ShowPireps Then
                    .Section11.SectionFormat.EnableSuppress = True
                End If
                'End
                If mReliabilityLifedComponentPrematureFailure.Count = 0 Then
                    .Section15.SectionFormat.EnableSuppress = True
                End If
                If mReliabilityOCComponentPrematureFailure.Count = 0 Then
                    .Section16.SectionFormat.EnableSuppress = True
                End If
                If mReliabilityDistributionList.Count = 0 Then 'Added by Saylee on All-23042013 to show Distribution list
                    .Section9.SectionFormat.EnableSuppress = True
                End If
                'Added By Utkash ON 03-May-2013 FOR ALL03052013
                If mDailyUtilizationGraphReport.Count = 0 Then
                    .Section6.SectionFormat.EnableSuppress = True
                End If
                'End
                If mMonthwiseAircraftCurrentStatus.Count = 0 Then
                    .Section20.SectionFormat.EnableSuppress = True
                End If

                If mMonthwiseEngineStatus.Count = 0 Then
                    .Section21.SectionFormat.EnableSuppress = True
                End If

                If mMonthwiseAPUStatus.Count = 0 Then
                    .Section23.SectionFormat.EnableSuppress = True
                End If

                If mrptMonthlySnagCountATAWise.Count = 0 Then
                    .Section37.SectionFormat.EnableSuppress = True
                End If
                'Added By Utkarsh ON 02-May-2013 FOR ALL2052013
                If Not mReliabilityMechanicalDefectRectification.ShowDefectRectification Then
                    .Section18.SectionFormat.EnableSuppress = True
                End If
                ' End
                'Added By Utkash ON 03-May-2013 FOR ALL03052013
                If Not mFligthDelayAndCancellationList.ShowDelays Then
                    .Section35.SectionFormat.EnableSuppress = True
                End If
                If Not mFligthDelayAndCancellationList.ShowCancellations Then
                    .Section25.SectionFormat.EnableSuppress = True
                End If
                'End

                ''Added By Shweta ON 27-May-2013 FOR ALL03052013
                If mMonthwiseRemovedEngineStatus.Count = 0 Then
                    .Section26.SectionFormat.EnableSuppress = True
                Else
                    .Section28.SectionFormat.EnableSuppress = True
                End If
                If mMonthwiseRemovedAPUStatus.Count = 0 Then
                    .Section27.SectionFormat.EnableSuppress = True
                Else
                    .Section29.SectionFormat.EnableSuppress = True
                End If

                If mrptMonthlySnagCountATAWiseForMaintenanceDefect.Count = 0 Then 'Added By Prashant ON 31-Jul-2013 FOR BA31072013
                    .Section22.SectionFormat.EnableSuppress = True
                End If

                If mMonthwiseAircraftOnGround.Count = 0 Then 'Added By Shweta ON 31-Jul-2013 FOR BA31072013
                    .Section33.SectionFormat.EnableSuppress = True
                End If
                'Added by utkarsh on 10-dec-2013
                If mMonthwisePropellerStatus.Count = 0 Then
                    .Section17.SectionFormat.EnableSuppress = True
                End If
                If mMonthwiseRemovedPropellerStatus.Count = 0 Then
                    .Section19.SectionFormat.EnableSuppress = True
                Else
                    .Section24.SectionFormat.EnableSuppress = True
                End If
                'End
                'Added By Utkarsh ON 06-Jan-2014 FOR ALL03012014
                If mReliabilityMELReport.Count = 0 Then
                    .DetailSection1.SectionFormat.EnableSuppress = True
                End If
                'End
                'Added By Saylee ON 8-Jul-2022
                If mReliabilityRepeatitiveDefectList.Count = 0 Then
                    .DetailSection2.SectionFormat.EnableSuppress = True
                End If
                'End

            End With

            Session("CrystalReport") = myReport
            If ByMail = False Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
            Else
                SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, " For " + lblyear1.Text + ", " + lblModel1.Text, "",
                                          Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"),
                                          ReportGeneratedBy:=Session("ReportGenratedBy"),
                    SmtpHost:=mModuleList.Item("Reliability").SmtpHost, SmtpPort:=mModuleList.Item("Reliability").SmtpPort,
                    SmtpUser:=mModuleList.Item("Reliability").SmtpUser, SmtpPassword:=mModuleList.Item("Reliability").SmtpPassword)
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
        spnlabel.Visible = IIf(AppSettings("ClientCode") = "SAA", True, False)
        mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
    End Sub
    Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
        Display()
        SetValues()
        upnlCriteria.Update()
    End Sub
    Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If Page.IsValid Then
            If AppSettings("ClientCode") = "SAA" Then
                SetReportSAA(False)
            Else
                SetReport(False)
            End If

        Else
            upnlValidationsummary.Update()
        End If
    End Sub
    Private Sub btnByMail_Click(sender As Object, e As System.EventArgs) Handles btnByMail.Click
        If Page.IsValid Then
            'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
            'Session("UserEmailID") = SI.UTILITY.User.GetUser(User.Identity.Name).UserEmail

            Session("UserEmailID") = mModuleList.Item("Reliability").SendToMailID
            Session("UserCcEmailID") = mModuleList.Item("Reliability").SendCCMailID
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
            If AppSettings("ClientCode") = "SAA" Then
                '' SetReportSAA(False)
                Dim email As New Thread(Sub() SetReportSAA(True))
                email.IsBackground = True
                email.Start()
            Else
                Dim email As New Thread(Sub() SetReport(True))
                email.IsBackground = True
                email.Start()
            End If
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
