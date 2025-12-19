'******************************************************
'Created by : Saylee 
'Dated      : 28-Feb-2025
'******************************************************


Imports System.Collections.Generic
Imports System.Linq
Imports System.Text


Public Class wfrptReliabilityReportNEW
    Inherits System.Web.UI.Page

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
    Dim mrptMechanicalReliability As rptMechanicalReliability
    Dim mDailyUtilizationGraphReport As DailyUtilizationGraphReport
    Dim mrptMonthlySnagCountATAWise As rptMonthlySnagCountATAWise


    Dim mMonthwiseAircraftCurrentStatus As MonthwiseAircraftCurrentStatus
    Dim mMonthwiseEngineStatus As MonthwiseEngineStatus
    Dim mMonthwiseAPUStatus As MonthwiseAPUStatus
    Dim mMonthwiseRemovedEngineStatus As MonthwiseRemovedEngineStatus
    Dim mMonthwiseRemovedAPUStatus As MonthwiseRemovedAPUStatus
    Dim mMonthwisePropellerStatus As MonthwisePropellerStatus
    Dim mMonthwiseRemovedPropellerStatus As MonthwiseRemovedPropellerStatus

    Dim mReliabilityMechanicalDefectRectification As ReliabilityDefectReportedByPilot

    Public mFligthDelayList As FligthDelayAndCancellationList
    Public mFligthCancellationList As FligthDelayAndCancellationList
    Dim mrptReliabilitySummary As rptReliabilitySummary

    Public mrptMonthlySnagCountATAWiseForMaintenanceDefect As rptMonthlySnagCountATAWiseForMaintenanceDefect
    Dim mMonthwiseAircraftOnGround As MonthwiseAircraftOnGround

    Private mrptReliabilityMonthlyATAWisePirepRate As rptReliabilityMonthlyATAWisePirepRate
    Private mrptReliabilityMonthlyATAWiseDefectRate As rptReliabilityMonthlyATAWiseMaintenanceDefectRate

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
    Dim mModuleList As ModuleList

    Public PDFNo As Integer = 1
    Public pdfList As New System.Collections.ArrayList
    Dim StartDateM As SmartDate
    Dim EndDateM As SmartDate
    Dim mReliabilityMELReport As ReliabilityDefectReportedByPilot
    Dim mReliabilityRepeatitiveDefectList As ReliabilityRepeatitiveDefectList
    Public mPrimaryModelList As PrimaryModelList 'Sankalp 
    Dim PrimaryModelIDs As New StringBuilder
    Dim mMELOpenClosedCount As MELOpenClosedCount

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
		'Sankalp
		mPrimaryModelList = PrimaryModelList.GetPrimaryModelList(AddTopItem:="(SELECT)")
		cmbPrimaryModel.DataSource = mPrimaryModelList
        cmbPrimaryModel.DataBind()
        'END

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
	Private Sub SetValues(Optional IsSyncApplication As Boolean = False)
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

			If IsSyncApplication = True Then
				PrimaryModelIDs.Append("<ModelID>")
			End If

			ModelIDs.Append("<ModelID>")

			For i As Integer = 0 To ChkModelIDs.Count - 1

				If IsSyncApplication = True Then
					PrimaryModelIDs.Append("<id>")
					PrimaryModelIDs.Append(mModelList(New Guid(ChkModelIDs(i))).PrimaryModelID)
					PrimaryModelIDs.Append("</id>")
				End If
				'Else
				ModelIDs.Append("<id>")
				ModelIDs.Append(ChkModelIDs(i))
				ModelIDs.Append("</id>")


				mModelNames.Append(ChkModelNames(i))
				mModelNames.Append(",")
				mModelNames.Append(" ")
				'	End If
			Next


			If IsSyncApplication = True Then
				PrimaryModelIDs.Append("</ModelID>")
			End If
			'Else
			'	ModelIDs.Append("</ModelID>")
			'End If
			ModelIDs.Append("</ModelID>")

		End If


		ChkRegNos = (From c As System.Web.UI.WebControls.ListItem In ListRegNo.Items
					 Where c.Selected = True
					 Select (c.Value)).ToArray

		ChkMachineNames = (From c As System.Web.UI.WebControls.ListItem In ListRegNo.Items
						   Where c.Selected = True
						   Select (c.Text)).ToArray
		If ChkRegNos.Length > 0 Then
			PrimaryModelIDs = New StringBuilder
			ModelIDs = New StringBuilder
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
				PrimaryModelIDs = New StringBuilder
				ModelIDs = New StringBuilder

				AircraftModelIDs.Append("<ModelID>")
				'  PrimaryModelIDs.Append("<ModelID>")


				For i As Integer = 0 To ChkAircraftModelIDs.Count - 1
					AircraftModelIDs.Append("<id>")
					AircraftModelIDs.Append(ChkAircraftModelIDs(i))
					AircraftModelIDs.Append("</id>")

					'PrimaryModelIDs.Append("<id>")
					'PrimaryModelIDs.Append(ModelListAsPerAircraft(New Guid(ChkAircraftModelIDs(i))).PrimaryModelID)
					'PrimaryModelIDs.Append("</id>")

				Next
				AircraftModelIDs.Append("</ModelID>")
				'   PrimaryModelIDs.Append("</ModelID>")
				'   PrimaryModelIDs.Append("</ModelID>")
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

                MyFile1 = "C:\Temp\" & "ReliabilityTempDefectReportedByPilot" & tmp & PDFNo.ToString & ".pdf"
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

                MyFile1 = "C:\Temp\" & "ReliabilityTempMechanicalDefectRectification" & tmp & PDFNo.ToString & ".pdf"
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

                MyFile1 = "C:\Temp\" & "ReliabilityTempMonthlyATAWisePirepRate" & tmp & PDFNo.ToString & ".pdf"
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

                MyFile1 = "C:\Temp\" & "ReliabilityTempMonthlyATAWisePirepRate" & tmp & PDFNo.ToString & ".pdf"
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
            Dim mFligthDelayAndCancellationList As FligthDelayAndCancellationList
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

                SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name,
                                          ReportName,
                                          ReportName,
                                          " For " + lblyear1.Text + ", " + lblModel1.Text,
                                          "",
                                          Session("ToSendMailIDs"),
                                          Session("CcSendMailIDs"),
                                          MergedPath_WM,
                                          True,
                                          Remark:=Session("SendMailRemark"),
                                          ReportGeneratedBy:=Session("ReportGenratedBy"),
                                          SmtpHost:=mModuleList.Item("Reliability").SmtpHost,
                                          SmtpPort:=mModuleList.Item("Reliability").SmtpPort,
                                          SmtpUser:=mModuleList.Item("Reliability").SmtpUser,
                                          SmtpPassword:=mModuleList.Item("Reliability").SmtpPassword)
            End If



        Catch ex As Exception

        End Try
    End Sub
    Public Sub ShowFleetHoursAndCycles()

        Dim da As New CSLA.Data.ObjectAdapter
        Dim mCompanyDetail As CompanyDetail
        Dim ReportName As String = String.Empty
        Dim ds As New dsReliabilityReport
        Dim mrptImage As rptImage

        Dim tmpDate As Date = DateAdd(DateInterval.Month, -2, DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, 1))

        'StartDateM = New SmartDate(DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, 1), False)
        'EndDateM = New SmartDate(CStr(DateSerial(tmpDate.Year, tmpDate.Month, DateTime.DaysInMonth(tmpDate.Year, tmpDate.Month))), False)

        StartDateM = New SmartDate(CStr(DateSerial(tmpDate.Year, tmpDate.Month, 1)), False)
        EndDateM = New SmartDate(DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, DateTime.DaysInMonth(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1)), False)

        mReliabilityFleetHoursCycles = ReliabilityFleetHoursCycles.GetReliabilityFlyingHoursRecord(, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString)
        mReliabilityFleetHoursCyclesForAllModels = ReliabilityFleetHoursCyclesForAllModels.GetReliabilityFlyingHoursRecord(cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer))


        If mReliabilityFleetHoursCyclesForAllModels.Count > 0 Then


            ReportName = "RELIABILITY REPORT"
            mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
            Dim Report As New ReportData(mCompanyDetail.CompanyName,
                                             mCompanyDetail.Address,
                                             mCompanyDetail.Tel1,
                                             mCompanyDetail.Tel2,
                                             mCompanyDetail.Fax,
                                             mCompanyDetail.Email,
                                             mCompanyDetail.WebSite,
                                             "",
                                             AppSettings("ClientCode"),
                                             "",
                                             "",
                                             "",
                                             "",
                                             AppSettings("Product Version"),
                                             AppSettings("SINote"),
                                             "",
                                             "",
                                             mModelNames.ToString.Trim.TrimEnd(","),
                                             mMachineNames.ToString.Trim.TrimEnd(","),
                                             cmbMonth.SelectedItem.Text + " , " + cmbYear.SelectedItem.Text)


            Dim myReport = New crReliabilityFleetHoursCycles
            ds.Clear()
            da.Fill(ds, "ReportData", Report)
            da.Fill(ds, mReliabilityFleetHoursCycles)
            da.Fill(ds, mReliabilityFleetHoursCyclesForAllModels)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            Dim MyFile1 = ""
            Dim myExportOption As CrystalDecisions.Shared.ExportOptions
            Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

            MyFile1 = "C:\Temp\" & "ReliabilityTempFleetHoursandCycles" & "_" & PDFNo.ToString & ".pdf"
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
    End Sub

    Public Sub ShowAircraftOnGroundStatusReport()

        Dim da As New CSLA.Data.ObjectAdapter
        Dim mCompanyDetail As CompanyDetail
        Dim ReportName As String = String.Empty
        Dim ds As New dsReliabilityReport
        Dim mrptImage As rptImage

        Dim tmpDate As Date = DateAdd(DateInterval.Month, -2, DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, 1))

        'StartDateM = New SmartDate(DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, 1), False)
        'EndDateM = New SmartDate(CStr(DateSerial(tmpDate.Year, tmpDate.Month, DateTime.DaysInMonth(tmpDate.Year, tmpDate.Month))), False)

        StartDateM = New SmartDate(CStr(DateSerial(tmpDate.Year, tmpDate.Month, 1)), False)
        EndDateM = New SmartDate(DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, DateTime.DaysInMonth(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1)), False)
        mMonthwiseAircraftOnGround = MonthwiseAircraftOnGround.GetMontMonthwiseAircraftOnGround(EndDateM.Text, , Guid.Empty.ToString, ModelIDs.ToString, MachineIDs.ToString)


        If mMonthwiseAircraftOnGround.Count > 0 Then

            ReportName = "RELIABILITY REPORT"
            mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
            Dim Report As New ReportData(mCompanyDetail.CompanyName,
                                             mCompanyDetail.Address,
                                             mCompanyDetail.Tel1,
                                             mCompanyDetail.Tel2,
                                             mCompanyDetail.Fax,
                                             mCompanyDetail.Email,
                                             mCompanyDetail.WebSite,
                                             "",
                                             AppSettings("ClientCode"),
                                             "",
                                             "",
                                             "",
                                             "",
                                             AppSettings("Product Version"),
                                             AppSettings("SINote"),
                                             "",
                                             SearchStr7:="",
                                             SearchStr8:=mModelNames.ToString.Trim.TrimEnd(","),
                                             SearchStr9:=mMachineNames.ToString.Trim.TrimEnd(","),
                                             SearchStr10:=cmbMonth.SelectedItem.Text + " , " + cmbYear.SelectedItem.Text)



            Dim myReport = New crMonthwiseAircraftOnGround
            ds.Clear()
            da.Fill(ds, Report)
            da.Fill(ds, "MonthwiseAircraftOnGround", mMonthwiseAircraftOnGround)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            Dim MyFile1 = ""
            Dim myExportOption As CrystalDecisions.Shared.ExportOptions
            Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

            MyFile1 = "C:\Temp\" & "ReliabilityTempMonthwiseAircraftOnGround" & "_" & PDFNo.ToString & ".pdf"
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
    End Sub

    Public Sub ShowReliabiltiySummaryReport()

        Dim da As New CSLA.Data.ObjectAdapter
        Dim mCompanyDetail As CompanyDetail
        Dim ReportName As String = String.Empty
        Dim ds As New dsReliabilityReport
        Dim mrptImage As rptImage

        Dim tmpDate As Date = DateAdd(DateInterval.Month, -2, DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, 1))
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        'StartDateM = New SmartDate(DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, 1), False)
        'EndDateM = New SmartDate(CStr(DateSerial(tmpDate.Year, tmpDate.Month, DateTime.DaysInMonth(tmpDate.Year, tmpDate.Month))), False)

        StartDateM = New SmartDate(CStr(DateSerial(tmpDate.Year, tmpDate.Month, 1)), False)
        EndDateM = New SmartDate(DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, DateTime.DaysInMonth(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1)), False)
		mrptReliabilitySummary = rptReliabilitySummary.GetReliabilitySummary(CInt(cmbYear.SelectedItem.Text),
																			 Guid.Empty,
																			 cmbMonth.SelectedIndex + 1,
																			 ModelIDs.ToString,
																			 MachineIDs.ToString,
																			 IsSyncApplication:=mCompanyDetail.IsSyncApplication,
																			 PrimaryModelIDs.ToString)




		ReportName = "RELIABILITY REPORT"

        Dim Report As New ReportData(mCompanyDetail.CompanyName,
                                         mCompanyDetail.Address,
                                         mCompanyDetail.Tel1,
                                         mCompanyDetail.Tel2,
                                         mCompanyDetail.Fax,
                                         mCompanyDetail.Email,
                                         mCompanyDetail.WebSite,
                                         "",
                                         AppSettings("ClientCode"),
                                         "",
                                         "",
                                         "",
                                         "",
                                         AppSettings("Product Version"),
                                         AppSettings("SINote"),
                                         "",
                                         SearchStr7:="",
                                         SearchStr8:=mModelNames.ToString.Trim.TrimEnd(","),
                                         SearchStr9:=mMachineNames.ToString.Trim.TrimEnd(","),
                                         SearchStr10:=cmbMonth.SelectedItem.Text + " , " + cmbYear.SelectedItem.Text)



        Dim myReport = New crReliabilitySummary
        ds.Clear()
        da.Fill(ds, "rptReliabilitySummary", mrptReliabilitySummary)
        da.Fill(ds, "ReportData", Report)
        mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
        da.Fill(ds, mrptImage)

        myReport.SetDataSource(ds)

        Session("CrystalReport") = myReport

        Dim MyFile1 = ""
        Dim myExportOption As CrystalDecisions.Shared.ExportOptions
        Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

        MyFile1 = "C:\Temp\" & "ReliabilityTempSummary" & "_" & PDFNo.ToString & ".pdf"
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

    End Sub
    Public Sub ShowFlyingHoursRecord()

        Dim da As New CSLA.Data.ObjectAdapter
        Dim mCompanyDetail As CompanyDetail
        Dim ReportName As String = String.Empty
        Dim ds As New dsReliabilityReport
        Dim mrptImage As rptImage

        Dim tmpDate As Date = DateAdd(DateInterval.Month, -2, DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, 1))
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        'StartDateM = New SmartDate(DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, 1), False)
        'EndDateM = New SmartDate(CStr(DateSerial(tmpDate.Year, tmpDate.Month, DateTime.DaysInMonth(tmpDate.Year, tmpDate.Month))), False)

        StartDateM = New SmartDate(CStr(DateSerial(tmpDate.Year, tmpDate.Month, 1)), False)
        EndDateM = New SmartDate(DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, DateTime.DaysInMonth(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1)), False)

        mReliabilityFlyingHoursRecord = ReliabilityFlyingHoursRecord.GetReliabilityFlyingHoursRecord(, Today.Date.ToString, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString)
        mReliabilityFlyingHoursRecordWithAircraft = ReliabilityFlyingHoursRecordWithAircraft.GetReliabilityFlyingHoursRecordWithAircraft(, Today.Date.ToString, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString)


        If mReliabilityFlyingHoursRecordWithAircraft.Count > 0 Then

            ReportName = "RELIABILITY REPORT"

            Dim Report As New ReportData(mCompanyDetail.CompanyName,
                                             mCompanyDetail.Address,
                                             mCompanyDetail.Tel1,
                                             mCompanyDetail.Tel2,
                                             mCompanyDetail.Fax,
                                             mCompanyDetail.Email,
                                             mCompanyDetail.WebSite,
                                             "",
                                             AppSettings("ClientCode"),
                                             "",
                                             "",
                                             "",
                                             "",
                                             AppSettings("Product Version"),
                                             AppSettings("SINote"),
                                             "",
                                             SearchStr7:="",
                                             SearchStr8:=mModelNames.ToString.Trim.TrimEnd(","),
                                             SearchStr9:=mMachineNames.ToString.Trim.TrimEnd(","),
                                             SearchStr10:=cmbMonth.SelectedItem.Text + " , " + cmbYear.SelectedItem.Text)



            Dim myReport = New crFlyingHoursRecord
            ds.Clear()
            da.Fill(ds, "ReliabilityFlyingHoursRecord", mReliabilityFlyingHoursRecord)
            da.Fill(ds, "ReliabilityFlyingHoursRecordWithAircraft", mReliabilityFlyingHoursRecordWithAircraft)
            da.Fill(ds, "ReportData", Report)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            Dim MyFile1 = ""
            Dim myExportOption As CrystalDecisions.Shared.ExportOptions
            Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

            MyFile1 = "C:\Temp\" & "ReliabilityTempHoursRecordWithAircraft" & "_" & PDFNo.ToString & ".pdf"
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
    End Sub


    Public Sub ShowAircraftStatusRecord()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim mCompanyDetail As CompanyDetail
        Dim ReportName As String = String.Empty
        Dim ds As New dsReliabilityReport
        Dim mrptImage As rptImage

        Dim tmpDate As Date = DateAdd(DateInterval.Month, -2, DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, 1))
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        'StartDateM = New SmartDate(DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, 1), False)
        'EndDateM = New SmartDate(CStr(DateSerial(tmpDate.Year, tmpDate.Month, DateTime.DaysInMonth(tmpDate.Year, tmpDate.Month))), False)

        StartDateM = New SmartDate(CStr(DateSerial(tmpDate.Year, tmpDate.Month, 1)), False)
        EndDateM = New SmartDate(DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, DateTime.DaysInMonth(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1)), False)


        Dim Report As New ReportData(mCompanyDetail.CompanyName,
                                             mCompanyDetail.Address,
                                             mCompanyDetail.Tel1,
                                             mCompanyDetail.Tel2,
                                             mCompanyDetail.Fax,
                                             mCompanyDetail.Email,
                                             mCompanyDetail.WebSite,
                                             "",
                                             AppSettings("ClientCode"),
                                             "",
                                             "",
                                             "",
                                             "",
                                             AppSettings("Product Version"),
                                             AppSettings("SINote"),
                                             "",
                                             SearchStr7:="",
                                             SearchStr8:=mModelNames.ToString.Trim.TrimEnd(","),
                                             SearchStr9:=mMachineNames.ToString.Trim.TrimEnd(","),
                                             SearchStr10:=cmbMonth.SelectedItem.Text + " , " + cmbYear.SelectedItem.Text)




		mMonthwiseAircraftCurrentStatus = MonthwiseAircraftCurrentStatus.GetMonthwiseAircraftCurrentStatus(, cmbMonth.SelectedItem.Text, CType(cmbYear.SelectedItem.Text, Integer), Today.Date.ToString, cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString)
		mMonthwiseEngineStatus = MonthwiseEngineStatus.GetMonthwiseEngineStatus(, cmbMonth.SelectedItem.Text, CType(cmbYear.SelectedItem.Text, Integer), Today.Date.ToString, cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString)
        mMonthwiseAPUStatus = MonthwiseAPUStatus.GetMonthwiseAPUStatus(4, , cmbMonth.SelectedItem.Text, CType(cmbYear.SelectedItem.Text, Integer), Today.Date.ToString, cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString)
        mMonthwisePropellerStatus = MonthwisePropellerStatus.GetMonthwisePropellerStatus(3, Guid.Empty.ToString, cmbMonth.SelectedItem.Text, CType(cmbYear.SelectedItem.Text, Integer), Today.Date.ToString, cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString)

        'Removal Status
        mMonthwiseRemovedEngineStatus = MonthwiseRemovedEngineStatus.GetMonthwiseRemoveEngineStatus(Guid.Empty.ToString, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString)
        mMonthwiseRemovedAPUStatus = MonthwiseRemovedAPUStatus.GetMonthwiseRemoveAPUStatus(Guid.Empty.ToString, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString)
        mMonthwiseRemovedPropellerStatus = MonthwiseRemovedPropellerStatus.GetMonthwiseRemovePropellerStatus(Guid.Empty.ToString, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString)


        ReportName = "RELIABILITY REPORT"
        Dim myReport = New crReliabilityAircraftStatus
        ds.Clear()
        da.Fill(ds, "MonthwiseAircraftCurrentStatus", mMonthwiseAircraftCurrentStatus)
        da.Fill(ds, "MonthwiseEngineStatus", mMonthwiseEngineStatus)
        da.Fill(ds, "MonthwiseAPUStatus", mMonthwiseAPUStatus)
        da.Fill(ds, "MonthwisePropellerStatus", mMonthwisePropellerStatus)
        da.Fill(ds, "MonthwiseRemovedEngineStatus", mMonthwiseRemovedEngineStatus)
        da.Fill(ds, "MonthwiseRemovedAPUStatus", mMonthwiseRemovedAPUStatus)
        da.Fill(ds, "MonthwiseRemovedPropellerStatus", mMonthwiseRemovedPropellerStatus)

        da.Fill(ds, "ReportData", Report)
        mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
        da.Fill(ds, mrptImage)

        myReport.SetDataSource(ds)

        With myReport
            If mMonthwiseRemovedEngineStatus.Count = 0 Then
                .DetailSection2.SectionFormat.EnableSuppress = True
            Else
                .DetailSection3.SectionFormat.EnableSuppress = True
            End If

            If mMonthwiseAPUStatus.Count = 0 Then
                .DetailSection4.SectionFormat.EnableSuppress = True
            End If

            If mMonthwiseRemovedAPUStatus.Count = 0 Then
                .DetailSection5.SectionFormat.EnableSuppress = True
            Else
                .DetailSection6.SectionFormat.EnableSuppress = True
            End If

            If mMonthwisePropellerStatus.Count = 0 Then
                .DetailSection9.SectionFormat.EnableSuppress = True
            End If

            If mMonthwiseRemovedPropellerStatus.Count = 0 Then
                .DetailSection8.SectionFormat.EnableSuppress = True
            Else
                .DetailSection7.SectionFormat.EnableSuppress = True
            End If

            If AppSettings("ClientCode") = "7AR" Then
                .DetailSection7.SectionFormat.EnableSuppress = True
                .DetailSection8.SectionFormat.EnableSuppress = True
                .DetailSection9.SectionFormat.EnableSuppress = True
            End If

        End With

        Session("CrystalReport") = myReport

        Dim MyFile1 = ""
        Dim myExportOption As CrystalDecisions.Shared.ExportOptions
        Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

        MyFile1 = "C:\Temp\" & "ReliabilityTempAircraftStatus" & "_" & PDFNo.ToString & ".pdf"
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




		'''AIRFRAME
		''If mMonthwiseAircraftCurrentStatus.Count > 0 Then
		''    ReportName = "RELIABILITY REPORT"
		''    Dim myReport = New crMonthwiseAircraftCurrentStatus
		''    ds.Clear()
		''    da.Fill(ds, "MonthwiseAircraftCurrentStatus", mMonthwiseAircraftCurrentStatus)
		''    da.Fill(ds, "ReportData", Report)
		''    mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
		''    da.Fill(ds, mrptImage)

		''    myReport.SetDataSource(ds)

		''    Session("CrystalReport") = myReport

		''    Dim MyFile1 = ""
		''    Dim myExportOption As CrystalDecisions.Shared.ExportOptions
		''    Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

		''    MyFile1 = "C:\Temp\" & "ReliabilityMonthwiseAircraftCurrentStatus" & "_" & PDFNo.ToString & ".pdf"
		''    myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

		''    myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
		''    myDiskOption.DiskFileName = MyFile1
		''    myExportOption = myReport.ExportOptions
		''    With myExportOption
		''        .DestinationOptions = myDiskOption
		''        .ExportDestinationType = ExportDestinationType.DiskFile
		''        .ExportFormatType = ExportFormatType.PortableDocFormat
		''    End With
		''    myReport.Export()
		''    myReport.Close()
		''    myReport.Dispose()
		''    GC.Collect()
		''    pdfList.Add(MyFile1)
		''    PDFNo = PDFNo + 1
		''End If

		'''ENGINE
		''If mMonthwiseEngineStatus.Count > 0 Then
		''    ReportName = "RELIABILITY REPORT"
		''    Dim myReport = New crMonthwiseEngineStatus
		''    ds.Clear()
		''    da.Fill(ds, "MonthwiseEngineStatus", mMonthwiseEngineStatus)
		''    da.Fill(ds, "ReportData", Report)
		''    mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
		''    da.Fill(ds, mrptImage)

		''    myReport.SetDataSource(ds)

		''    Session("CrystalReport") = myReport

		''    Dim MyFile1 = ""
		''    Dim myExportOption As CrystalDecisions.Shared.ExportOptions
		''    Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

		''    MyFile1 = "C:\Temp\" & "ReliabilityMonthwiseEngineStatus" & "_" & PDFNo.ToString & ".pdf"
		''    myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

		''    myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
		''    myDiskOption.DiskFileName = MyFile1
		''    myExportOption = myReport.ExportOptions
		''    With myExportOption
		''        .DestinationOptions = myDiskOption
		''        .ExportDestinationType = ExportDestinationType.DiskFile
		''        .ExportFormatType = ExportFormatType.PortableDocFormat
		''    End With
		''    myReport.Export()
		''    myReport.Close()
		''    myReport.Dispose()
		''    GC.Collect()
		''    pdfList.Add(MyFile1)
		''    PDFNo = PDFNo + 1
		''End If

		'''APU
		''If mMonthwiseAPUStatus.Count > 0 Then
		''    Dim myReport = New crMonthwiseAPUStatus
		''    ds.Clear()
		''    da.Fill(ds, "MonthwiseAPUStatus", mMonthwiseAPUStatus)
		''    da.Fill(ds, "ReportData", Report)
		''    mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
		''    da.Fill(ds, mrptImage)

		''    myReport.SetDataSource(ds)

		''    Session("CrystalReport") = myReport

		''    Dim MyFile1 = ""
		''    Dim myExportOption As CrystalDecisions.Shared.ExportOptions
		''    Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

		''    MyFile1 = "C:\Temp\" & "ReliabilityMonthwiseAPUStatus" & "_" & PDFNo.ToString & ".pdf"
		''    myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

		''    myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
		''    myDiskOption.DiskFileName = MyFile1
		''    myExportOption = myReport.ExportOptions
		''    With myExportOption
		''        .DestinationOptions = myDiskOption
		''        .ExportDestinationType = ExportDestinationType.DiskFile
		''        .ExportFormatType = ExportFormatType.PortableDocFormat
		''    End With
		''    myReport.Export()
		''    myReport.Close()
		''    myReport.Dispose()
		''    GC.Collect()
		''    pdfList.Add(MyFile1)
		''    PDFNo = PDFNo + 1
		''End If

		'''Propeller
		''If mMonthwisePropellerStatus.Count > 0 Then
		''    Dim myReport = New crMonthwisePropellerStatus
		''    ds.Clear()
		''    da.Fill(ds, "MonthwisePropellerStatus", mMonthwisePropellerStatus)
		''    da.Fill(ds, "ReportData", Report)
		''    mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
		''    da.Fill(ds, mrptImage)

		''    myReport.SetDataSource(ds)

		''    Session("CrystalReport") = myReport

		''    Dim MyFile1 = ""
		''    Dim myExportOption As CrystalDecisions.Shared.ExportOptions
		''    Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

		''    MyFile1 = "C:\Temp\" & "ReliabilityMonthwisePropellerStatus" & "_" & PDFNo.ToString & ".pdf"
		''    myReport = CType(Session("CrystalReport"), CrystalDecisions.CrystalReports.Engine.ReportClass)

		''    myDiskOption = New CrystalDecisions.Shared.DiskFileDestinationOptions
		''    myDiskOption.DiskFileName = MyFile1
		''    myExportOption = myReport.ExportOptions
		''    With myExportOption
		''        .DestinationOptions = myDiskOption
		''        .ExportDestinationType = ExportDestinationType.DiskFile
		''        .ExportFormatType = ExportFormatType.PortableDocFormat
		''    End With
		''    myReport.Export()
		''    myReport.Close()
		''    myReport.Dispose()
		''    GC.Collect()
		''    pdfList.Add(MyFile1)
		''    PDFNo = PDFNo + 1
		''End If
	End Sub


    Public Sub ShowUtilization()

        Dim da As New CSLA.Data.ObjectAdapter
        Dim mCompanyDetail As CompanyDetail
        Dim ReportName As String = String.Empty
        Dim ds As New dsReliabilityReport
        Dim mrptImage As rptImage

        Dim tmpDate As Date = DateAdd(DateInterval.Month, -2, DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, 1))
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        'StartDateM = New SmartDate(DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, 1), False)
        'EndDateM = New SmartDate(CStr(DateSerial(tmpDate.Year, tmpDate.Month, DateTime.DaysInMonth(tmpDate.Year, tmpDate.Month))), False)

        StartDateM = New SmartDate(CStr(DateSerial(tmpDate.Year, tmpDate.Month, 1)), False)
        EndDateM = New SmartDate(DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, DateTime.DaysInMonth(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1)), False)

        Dim Report As New ReportData(mCompanyDetail.CompanyName,
                                             mCompanyDetail.Address,
                                             mCompanyDetail.Tel1,
                                             mCompanyDetail.Tel2,
                                             mCompanyDetail.Fax,
                                             mCompanyDetail.Email,
                                             mCompanyDetail.WebSite,
                                             "",
                                             AppSettings("ClientCode"),
                                             "",
                                             "",
                                             "",
                                             "",
                                             AppSettings("Product Version"),
                                             AppSettings("SINote"),
                                             "",
                                             SearchStr7:="",
                                             SearchStr8:=mModelNames.ToString.Trim.TrimEnd(","),
                                             SearchStr9:=mMachineNames.ToString.Trim.TrimEnd(","),
                                             SearchStr10:=cmbMonth.SelectedItem.Text + " , " + cmbYear.SelectedItem.Text)

        mrptReliabilityAircraftUtilization = rptReliabilityAircraftUtilization.GetReliabilityAircraftUtilization(Guid.Empty, CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString)
		mDailyUtilizationGraphReport = DailyUtilizationGraphReport.GetDailyUtilizationGraph(Guid.Empty, CInt(cmbYear.SelectedItem.Text), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString, IsSyncApplication:=mCompanyDetail.IsSyncApplication)


		If mrptReliabilityAircraftUtilization.TotalNoOfAircraft <> 0 Then

            ReportName = "RELIABILITY REPORT"
            Dim myReport = New crReliabilityAircraftUtilization
            ds.Clear()
            da.Fill(ds, "rptReliabilityAircraftUtilization", mrptReliabilityAircraftUtilization)

            da.Fill(ds, "ReportData", Report)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            Dim MyFile1 = ""
            Dim myExportOption As CrystalDecisions.Shared.ExportOptions
            Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

            MyFile1 = "C:\Temp\" & "ReliabilityTempAircraftUtilization" & "_" & PDFNo.ToString & ".pdf"
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

        If mDailyUtilizationGraphReport.Count > 0 Then

            ReportName = "RELIABILITY REPORT"
            Dim myReport = New crDailyUtilizationGraph
            ds.Clear()
            da.Fill(ds, "DailyUtilizationGraphReport", mDailyUtilizationGraphReport)

            da.Fill(ds, "ReportData", Report)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            Dim MyFile1 = ""
            Dim myExportOption As CrystalDecisions.Shared.ExportOptions
            Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

            MyFile1 = "C:\Temp\" & "ReliabilityTempDailyUtilization" & "_" & PDFNo.ToString & ".pdf"
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
    End Sub

    Public Sub ShowDelayCancellation()

        Dim da As New CSLA.Data.ObjectAdapter
        Dim mCompanyDetail As CompanyDetail
        Dim ReportName As String = String.Empty
        Dim ds As New dsReliabilityReport
        Dim mrptImage As rptImage

        Dim tmpDate As Date = DateAdd(DateInterval.Month, -2, DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, 1))
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
		'StartDateM = New SmartDate(DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, 1), False)
		'EndDateM = New SmartDate(CStr(DateSerial(tmpDate.Year, tmpDate.Month, DateTime.DaysInMonth(tmpDate.Year, tmpDate.Month))), False)

		If mCompanyDetail.IsSyncApplication Then
			StartDateM = New SmartDate(DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, 1), False)
		Else
			StartDateM = New SmartDate(CStr(DateSerial(tmpDate.Year, tmpDate.Month, 1)), False)
		End If

		EndDateM = New SmartDate(DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, DateTime.DaysInMonth(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1)), False)

        Dim Report As New ReportData(mCompanyDetail.CompanyName,
                                             mCompanyDetail.Address,
                                             mCompanyDetail.Tel1,
                                             mCompanyDetail.Tel2,
                                             mCompanyDetail.Fax,
                                             mCompanyDetail.Email,
                                             mCompanyDetail.WebSite,
                                             "",
                                             AppSettings("ClientCode"),
                                             "",
                                             "",
                                             "",
                                             "",
                                             AppSettings("Product Version"),
                                             AppSettings("SINote"),
                                             "",
                                             SearchStr7:="",
                                             SearchStr8:=mModelNames.ToString.Trim.TrimEnd(","),
                                             SearchStr9:=mMachineNames.ToString.Trim.TrimEnd(","),
                                             SearchStr10:=cmbMonth.SelectedItem.Text + " , " + cmbYear.SelectedItem.Text)

		mrptMechanicalReliability = rptMechanicalReliability.GetMechanicalReliability(Guid.Empty, CInt(cmbYear.SelectedItem.Text), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString, IsSyncApplication:=mCompanyDetail.IsSyncApplication)
		If mCompanyDetail.IsSyncApplication Then
			mFligthDelayList = FligthDelayAndCancellationList.GetFlightDCList(Guid.Empty, StartDateM.Text, EndDateM.Text, IsDelay:=True, IsCancel:=False, ConsiderInReliability:=True, True, Guid.Empty.ToString, PrimaryModelIDs.ToString, MachineIDs.ToString, IsSyncApplication:=mCompanyDetail.IsSyncApplication)
			mFligthCancellationList = FligthDelayAndCancellationList.GetFlightDCList(Guid.Empty, StartDateM.Text, EndDateM.Text, IsDelay:=False, IsCancel:=True, ConsiderInReliability:=True, True, Guid.Empty.ToString, PrimaryModelIDs.ToString, MachineIDs.ToString, IsSyncApplication:=mCompanyDetail.IsSyncApplication)

		Else
			mFligthDelayList = FligthDelayAndCancellationList.GetFlightDCList(Guid.Empty, StartDateM.Text, EndDateM.Text, IsDelay:=True, IsCancel:=False, ConsiderInReliability:=True, True, Guid.Empty.ToString, ModelIDs.ToString, MachineIDs.ToString, IsSyncApplication:=mCompanyDetail.IsSyncApplication)
			mFligthCancellationList = FligthDelayAndCancellationList.GetFlightDCList(Guid.Empty, StartDateM.Text, EndDateM.Text, IsDelay:=False, IsCancel:=True, ConsiderInReliability:=True, True, Guid.Empty.ToString, ModelIDs.ToString, MachineIDs.ToString, IsSyncApplication:=mCompanyDetail.IsSyncApplication)

		End If


		''1) FlightCompletion
		ReportName = "RELIABILITY REPORT"
        Dim myReport = New crMechanicalReliabilityFlightCompletion
        ds.Clear()
        da.Fill(ds, "rptMechanicalReliability", mrptMechanicalReliability)


        da.Fill(ds, "ReportData", Report)
        mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
        da.Fill(ds, mrptImage)

        myReport.SetDataSource(ds)

        Session("CrystalReport") = myReport

        Dim MyFile1 = ""
        Dim myExportOption As CrystalDecisions.Shared.ExportOptions
        Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

        MyFile1 = "C:\Temp\" & "ReliabilityTempFlightCompletion" & "_" & PDFNo.ToString & ".pdf"
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


        ''2) OnTime Departure
        ReportName = "RELIABILITY REPORT"
        myReport = New crMechanicalReliabilityOnTimeDeparture
        ds.Clear()
        da.Fill(ds, "rptMechanicalReliability", mrptMechanicalReliability)


        da.Fill(ds, "ReportData", Report)
        mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
        da.Fill(ds, mrptImage)

        myReport.SetDataSource(ds)

        Session("CrystalReport") = myReport

        MyFile1 = ""
        MyFile1 = "C:\Temp\" & "ReliabilityTempOnTimeDeparture" & "_" & PDFNo.ToString & ".pdf"
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

        '3) Flight Delay
        If mFligthDelayList.Count > 0 Then


            ReportName = "RELIABILITY REPORT"
            myReport = New crReliabilityFlightDelays
            ds.Clear()
            da.Fill(ds, "FligthDelayAndCancellationList", mFligthDelayList)

            da.Fill(ds, "ReportData", Report)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            MyFile1 = ""

            MyFile1 = "C:\Temp\" & "ReliabilityTempFlightDelays" & "_" & PDFNo.ToString & ".pdf"
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

        '4) Flight Cancellation
        If mFligthCancellationList.Count > 0 Then


            ReportName = "RELIABILITY REPORT"
            myReport = New crReliabilityFlightCancellations
            ds.Clear()
            da.Fill(ds, "FligthDelayAndCancellationList", mFligthCancellationList)

            da.Fill(ds, "ReportData", Report)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            MyFile1 = ""

            MyFile1 = "C:\Temp\" & "ReliabilityTempFlightCancellation" & "_" & PDFNo.ToString & ".pdf"
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

    End Sub

    Public Sub ShowPirepsMaintDefect()

        Dim da As New CSLA.Data.ObjectAdapter
        Dim mCompanyDetail As CompanyDetail
        Dim ReportName As String = String.Empty
        Dim ds As New dsReliabilityReport
        Dim mrptImage As rptImage

        Dim tmpDate As Date = DateAdd(DateInterval.Month, -2, DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, 1))
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        'StartDateM = New SmartDate(DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, 1), False)
        'EndDateM = New SmartDate(CStr(DateSerial(tmpDate.Year, tmpDate.Month, DateTime.DaysInMonth(tmpDate.Year, tmpDate.Month))), False)

        StartDateM = New SmartDate(CStr(DateSerial(tmpDate.Year, tmpDate.Month, 1)), False)
        EndDateM = New SmartDate(DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, DateTime.DaysInMonth(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1)), False)

        Dim Report As New ReportData(mCompanyDetail.CompanyName,
                                             mCompanyDetail.Address,
                                             mCompanyDetail.Tel1,
                                             mCompanyDetail.Tel2,
                                             mCompanyDetail.Fax,
                                             mCompanyDetail.Email,
                                             mCompanyDetail.WebSite,
                                             "",
                                             AppSettings("ClientCode"),
                                             "",
                                             "",
                                             "",
                                             "",
                                             AppSettings("Product Version"),
                                             AppSettings("SINote"),
                                             "",
                                             SearchStr7:="",
                                             SearchStr8:=mModelNames.ToString.Trim.TrimEnd(","),
                                             SearchStr9:=mMachineNames.ToString.Trim.TrimEnd(","),
                                             SearchStr10:=cmbMonth.SelectedItem.Text + " , " + cmbYear.SelectedItem.Text)

        mrptReliabilityMonthlyATAWisePirepRate = rptReliabilityMonthlyATAWisePirepRate.GetMonthlyPirepRateATAWise(CType(cmbYear.SelectedItem.Text, Integer), Guid.Empty, cmbMonth.SelectedIndex + 1, True, ModelIDs.ToString, MachineIDs.ToString)
        Dim myReport As CrystalDecisions.CrystalReports.Engine.ReportClass
        '1) ReliabilityMonthlyATAWisePirepRate
        ReportName = "RELIABILITY REPORT"





        If chkPIREP.Checked Then
            mReliabilityDefectReportedByPilot = ReliabilityDefectReportedByPilot.GetReliabilityDefectReportedByPilot(, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), True, , ModelIDs.ToString, MachineIDs.ToString)
            mrptMonthlySnagCountATAWise = rptMonthlySnagCountATAWise.GetrptMonthlySnagCountATAWise(cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), , True, ModelIDs.ToString, MachineIDs.ToString)


            If mrptReliabilityMonthlyATAWisePirepRate.Count > 0 Then

                myReport = New crptReliabilityMonthlyATAWisePirepRate
                ds.Clear()
                da.Fill(ds, "rptReliabilityMonthlyATAWisePirepRate", mrptReliabilityMonthlyATAWisePirepRate)

                da.Fill(ds, "ReportData", Report)
                mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
                da.Fill(ds, mrptImage)

                myReport.SetDataSource(ds)

                Session("CrystalReport") = myReport

                Dim MyFile1 = ""
                Dim myExportOption As CrystalDecisions.Shared.ExportOptions
                Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

                MyFile1 = "C:\Temp\" & "ReliabilityTempMonthlyATAWisePirepRate" & "_" & PDFNo.ToString & ".pdf"
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
            '2) mReliabilityDefectReportedByPilot
            If mReliabilityDefectReportedByPilot.Count > 0 Then

                ReportName = "RELIABILITY REPORT"
                myReport = New crReliabilityDefectReportedByPilot
                ds.Clear()
                da.Fill(ds, "ReliabilityDefectReportedByPilot", mReliabilityDefectReportedByPilot)

                da.Fill(ds, "ReportData", Report)
                mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
                da.Fill(ds, mrptImage)

                myReport.SetDataSource(ds)

                Session("CrystalReport") = myReport

                Dim MyFile1 = ""
                Dim myExportOption As CrystalDecisions.Shared.ExportOptions
                Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

                MyFile1 = "C:\Temp\" & "ReliabilityTempDefectReportedByPilot" & "_" & PDFNo.ToString & ".pdf"
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

            If AppSettings("ClientCode") = "7AR" Then 'Added by Saylee on 31-Jul-2025 as 7AR does not have PIREP Graph
                GoTo SkipPirepGraph
            End If

            '3) mReliabilityDefectReportedByPilot
            If mReliabilityDefectReportedByPilot.Count > 0 Then

                ReportName = "RELIABILITY REPORT"
                myReport = New crMonthlySnagATAWise
                ds.Clear()
                da.Fill(ds, "rptMonthlySnagCountATAWise", mrptMonthlySnagCountATAWise)

                da.Fill(ds, "ReportData", Report)
                mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
                da.Fill(ds, mrptImage)

                myReport.SetDataSource(ds)

                Session("CrystalReport") = myReport

                Dim MyFile1 = ""
                Dim myExportOption As CrystalDecisions.Shared.ExportOptions
                Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

                MyFile1 = "C:\Temp\" & "ReliabilityTempMonthlySnagCountATAWise" & "_" & PDFNo.ToString & ".pdf"
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
        End If

SkipPirepGraph: If chkMaintenanceDefect.Checked Then

            mReliabilityMechanicalDefectRectification = ReliabilityDefectReportedByPilot.GetReliabilityDefectReportedByPilot(Guid.Empty.ToString, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), False, , ModelIDs.ToString, MachineIDs.ToString)

            mrptMonthlySnagCountATAWiseForMaintenanceDefect = rptMonthlySnagCountATAWiseForMaintenanceDefect.GetrptMonthlySnagCountATAWise(cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), Guid.Empty.ToString, False, ModelIDs.ToString, MachineIDs.ToString)

            mReliabilityRepeatitiveDefectList = ReliabilityRepeatitiveDefectList.GetReliabilityRepeatitiveDefectList(, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString)

            If mrptReliabilityMonthlyATAWisePirepRate.Count > 0 Then
                myReport = New crptReliabilityMonthlyATAWiseMaintDefectRate

                ds.Clear()
                da.Fill(ds, "rptReliabilityMonthlyATAWisePirepRate", mrptReliabilityMonthlyATAWisePirepRate)

                da.Fill(ds, "ReportData", Report)
                mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
                da.Fill(ds, mrptImage)

                myReport.SetDataSource(ds)

                Session("CrystalReport") = myReport

                Dim MyFile1 = ""
                Dim myExportOption As CrystalDecisions.Shared.ExportOptions
                Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

                MyFile1 = "C:\Temp\" & "ReliabilityTempMonthlyATAWisePirepRate" & "_" & PDFNo.ToString & ".pdf"
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
            '2) ReliabilityMaintenanceDefectRectification
            If mReliabilityMechanicalDefectRectification.Count > 0 Then

                ReportName = "RELIABILITY REPORT"
                myReport = New crReliabilityMaintenanceDefectRectification
                ds.Clear()
                da.Fill(ds, "ReliabilityDefectReportedByPilot", mReliabilityMechanicalDefectRectification)

                da.Fill(ds, "ReportData", Report)
                mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
                da.Fill(ds, mrptImage)

                myReport.SetDataSource(ds)

                Session("CrystalReport") = myReport

                Dim MyFile1 = ""
                Dim myExportOption As CrystalDecisions.Shared.ExportOptions
                Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

                MyFile1 = "C:\Temp\" & "ReliabilityTempMechanicalDefectRectification" & "_" & PDFNo.ToString & ".pdf"
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


            If AppSettings("ClientCode") = "7AR" Then  'Added by Saylee on 31-Jul-2025 as 7AR does not have Maintenance Defect Graph
                GoTo SkipMaintenanceDefectGraph
            End If

            '3) MonthlySnagCountATAWiseForMainteDefect
            If mrptMonthlySnagCountATAWiseForMaintenanceDefect.Count > 0 Then

                ReportName = "RELIABILITY REPORT"
                myReport = New crMonthlySnagCountATAWiseForMainteDefect
                ds.Clear()
                da.Fill(ds, "rptMonthlySnagCountATAWiseForMaintenanceDefect", mrptMonthlySnagCountATAWiseForMaintenanceDefect)

                da.Fill(ds, "ReportData", Report)
                mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
                da.Fill(ds, mrptImage)

                myReport.SetDataSource(ds)

                Session("CrystalReport") = myReport

                Dim MyFile1 = ""
                Dim myExportOption As CrystalDecisions.Shared.ExportOptions
                Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

                MyFile1 = "C:\Temp\" & "ReliabilityTempMonthlySnagCountATAWiseForMaintenanceDefect" & "_" & PDFNo.ToString & ".pdf"
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

SkipMaintenanceDefectGraph: End If

    End Sub


    Public Sub ShowMaintenanceDefects()

        Dim da As New CSLA.Data.ObjectAdapter
        Dim mCompanyDetail As CompanyDetail
        Dim ReportName As String = String.Empty
        Dim ds As New dsReliabilityReport
        Dim mrptImage As rptImage

        Dim tmpDate As Date = DateAdd(DateInterval.Month, -2, DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, 1))
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        'StartDateM = New SmartDate(DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, 1), False)
        'EndDateM = New SmartDate(CStr(DateSerial(tmpDate.Year, tmpDate.Month, DateTime.DaysInMonth(tmpDate.Year, tmpDate.Month))), False)

        StartDateM = New SmartDate(CStr(DateSerial(tmpDate.Year, tmpDate.Month, 1)), False)
        EndDateM = New SmartDate(DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, DateTime.DaysInMonth(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1)), False)

        Dim Report As New ReportData(mCompanyDetail.CompanyName,
                                             mCompanyDetail.Address,
                                             mCompanyDetail.Tel1,
                                             mCompanyDetail.Tel2,
                                             mCompanyDetail.Fax,
                                             mCompanyDetail.Email,
                                             mCompanyDetail.WebSite,
                                             "",
                                             AppSettings("ClientCode"),
                                             "",
                                             "",
                                             "",
                                             "",
                                             AppSettings("Product Version"),
                                             AppSettings("SINote"),
                                             "",
                                             SearchStr7:="",
                                             SearchStr8:=mModelNames.ToString.Trim.TrimEnd(","),
                                             SearchStr9:=mMachineNames.ToString.Trim.TrimEnd(","),
                                             SearchStr10:=cmbMonth.SelectedItem.Text + " , " + cmbYear.SelectedItem.Text)

        mrptReliabilityMonthlyATAWiseDefectRate = rptReliabilityMonthlyATAWiseMaintenanceDefectRate.GetMonthlyDefectRateATAWise(CType(cmbYear.SelectedItem.Text, Integer), Guid.Empty, cmbMonth.SelectedIndex + 1, False, ModelIDs.ToString, MachineIDs.ToString)


        mReliabilityMechanicalDefectRectification = ReliabilityDefectReportedByPilot.GetReliabilityDefectReportedByPilot(Guid.Empty.ToString, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), False, , ModelIDs.ToString, MachineIDs.ToString)

        mrptMonthlySnagCountATAWiseForMaintenanceDefect = rptMonthlySnagCountATAWiseForMaintenanceDefect.GetrptMonthlySnagCountATAWise(cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), Guid.Empty.ToString, False, ModelIDs.ToString, MachineIDs.ToString)


        mReliabilityRepeatitiveDefectList = ReliabilityRepeatitiveDefectList.GetReliabilityRepeatitiveDefectList(, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString)

        '1) ReliabilityMonthlyATAWisePirepRate
        If mrptReliabilityMonthlyATAWiseDefectRate.Count > 0 Then

            ReportName = "RELIABILITY REPORT"
            Dim myReport = New crptReliabilityMonthlyATAWiseMaintDefectRate
            ds.Clear()
            da.Fill(ds, mrptReliabilityMonthlyATAWiseDefectRate)

            da.Fill(ds, "ReportData", Report)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            Dim MyFile1 = ""
            Dim myExportOption As CrystalDecisions.Shared.ExportOptions
            Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

            MyFile1 = "C:\Temp\" & "ReliabilityTempMonthlyATAWiseDefectRate" & "_" & PDFNo.ToString & ".pdf"
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

        '2) ReliabilityMaintenanceDefectRectification
        If mReliabilityMechanicalDefectRectification.Count > 0 Then

            ReportName = "RELIABILITY REPORT"
            Dim myReport = New crReliabilityMaintenanceDefectRectification
            ds.Clear()
            da.Fill(ds, "ReliabilityDefectReportedByPilot", mReliabilityMechanicalDefectRectification)

            da.Fill(ds, "ReportData", Report)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            Dim MyFile1 = ""
            Dim myExportOption As CrystalDecisions.Shared.ExportOptions
            Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

            MyFile1 = "C:\Temp\" & "ReliabilityTempMechanicalDefectRectification" & "_" & PDFNo.ToString & ".pdf"
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

        '3) MonthlySnagCountATAWiseForMainteDefect
        If mrptMonthlySnagCountATAWiseForMaintenanceDefect.Count > 0 Then

            ReportName = "RELIABILITY REPORT"
            Dim myReport = New crMonthlySnagCountATAWiseForMainteDefect
            ds.Clear()
            da.Fill(ds, "rptMonthlySnagCountATAWiseForMaintenanceDefect", mrptMonthlySnagCountATAWiseForMaintenanceDefect)

            da.Fill(ds, "ReportData", Report)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            Dim MyFile1 = ""
            Dim myExportOption As CrystalDecisions.Shared.ExportOptions
            Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

            MyFile1 = "C:\Temp\" & "ReliabilityTempMonthlySnagCountATAWiseForMaintenanceDefect" & "_" & PDFNo.ToString & ".pdf"
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
    End Sub
    Public Sub ShowMEL()
        Dim da As New CSLA.Data.ObjectAdapter
        Dim mCompanyDetail As CompanyDetail
        Dim ReportName As String = String.Empty
        Dim ds As New dsReliabilityReport
        Dim mrptImage As rptImage

        Dim tmpDate As Date = DateAdd(DateInterval.Month, -2, DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, 1))
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        'StartDateM = New SmartDate(DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, 1), False)
        'EndDateM = New SmartDate(CStr(DateSerial(tmpDate.Year, tmpDate.Month, DateTime.DaysInMonth(tmpDate.Year, tmpDate.Month))), False)

        StartDateM = New SmartDate(CStr(DateSerial(tmpDate.Year, tmpDate.Month, 1)), False)
        EndDateM = New SmartDate(DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, DateTime.DaysInMonth(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1)), False)

        Dim Report As New ReportData(mCompanyDetail.CompanyName,
                                             mCompanyDetail.Address,
                                             mCompanyDetail.Tel1,
                                             mCompanyDetail.Tel2,
                                             mCompanyDetail.Fax,
                                             mCompanyDetail.Email,
                                             mCompanyDetail.WebSite,
                                             "",
                                             AppSettings("ClientCode"),
                                             "",
                                             "",
                                             "",
                                             "",
                                             AppSettings("Product Version"),
                                             AppSettings("SINote"),
                                             "",
                                             SearchStr7:="",
                                             SearchStr8:=mModelNames.ToString.Trim.TrimEnd(","),
                                             SearchStr9:=mMachineNames.ToString.Trim.TrimEnd(","),
                                             SearchStr10:=cmbMonth.SelectedItem.Text + " , " + cmbYear.SelectedItem.Text)


        mReliabilityMELReport = ReliabilityDefectReportedByPilot.GetReliabilityDefectReportedByPilot(, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), False, True, ModelIDs.ToString, MachineIDs.ToString)

        '1) Repeatitive DefectList
        If mReliabilityMELReport.Count > 0 Then

            ReportName = "RELIABILITY REPORT"
            Dim myReport = New crReliabilityMELReport
            ds.Clear()
            da.Fill(ds, mReliabilityMELReport)

            da.Fill(ds, "ReportData", Report)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            Dim MyFile1 = ""
            Dim myExportOption As CrystalDecisions.Shared.ExportOptions
            Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

            MyFile1 = "C:\Temp\" & "ReliabilityTempRepeatitiveDefectList" & "_" & PDFNo.ToString & ".pdf"
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

        '2) AverageTime MELOpen
        If AppSettings("ClientCode") = "7AR" Then 'Added by Saylee on 5-Aug-2025 
            Dim mrptReliabilityAverageTimeMELOpen As rptReliabilityAverageTimeMELOpen
            mrptReliabilityAverageTimeMELOpen = rptReliabilityAverageTimeMELOpen.GetMELOpenClosedCount(cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), , ModelIDs.ToString, MachineIDs.ToString)

            If mrptReliabilityAverageTimeMELOpen.Count > 0 Then

                ReportName = "RELIABILITY REPORT"
                Dim myReport = New crptReliabilityAverageTimeMELOpen
                ds.Clear()
                da.Fill(ds, "rptReliabilityAverageTimeMELOpen", mrptReliabilityAverageTimeMELOpen)

                da.Fill(ds, "ReportData", Report)
                mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
                da.Fill(ds, mrptImage)

                myReport.SetDataSource(ds)

                Session("CrystalReport") = myReport

                Dim MyFile1 = ""
                Dim myExportOption As CrystalDecisions.Shared.ExportOptions
                Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

                MyFile1 = "C:\Temp\" & "ReliabilityAverageTimeList" & "_" & PDFNo.ToString & ".pdf"
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

        End If
        mMELOpenClosedCount = MELOpenClosedCount.GetMELOpenClosedCount(cmbMonth.SelectedIndex + 1,
                                                                       CType(cmbYear.SelectedItem.Text, Integer),
                                                                       Guid.Empty.ToString,
                                                                       ModelIDs.ToString,
                                                                       MachineIDs.ToString,
                                                                       Daily:=0)


        '3) MELOpenClosedCount
        If mMELOpenClosedCount.Count > 0 Then

            ReportName = "12 MONTH FLEET MEL OPEN / CLOSED"
            Dim myReport = New crMELOpenClosedCount
            ds.Clear()
            da.Fill(ds, "MELOpenClosedCount", mMELOpenClosedCount)
            da.Fill(ds, "ReportData", Report)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            Dim MyFile1 = ""
            Dim myExportOption As CrystalDecisions.Shared.ExportOptions
            Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

            MyFile1 = "C:\Temp\" & "MELOpenClosedCount" & "_" & PDFNo.ToString & ".pdf"
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


        mMELOpenClosedCount = MELOpenClosedCount.GetMELOpenClosedCount(cmbMonth.SelectedIndex + 1,
                                                                       CType(cmbYear.SelectedItem.Text, Integer),
                                                                       Guid.Empty.ToString,
                                                                       ModelIDs.ToString,
                                                                       MachineIDs.ToString,
                                                                       Daily:=1)


        '4) DAILY MEL RATE MELOpenClosedCountDaily
        If mMELOpenClosedCount.Count > 0 Then

            ReportName = "DAILY MEL RATE"
            Dim myReport = New crMELOpenClosedDailyCount
            ds.Clear()
            da.Fill(ds, "MELOpenClosedCount", mMELOpenClosedCount)
            da.Fill(ds, "ReportData", Report)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            Dim MyFile1 = ""
            Dim myExportOption As CrystalDecisions.Shared.ExportOptions
            Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

            MyFile1 = "C:\Temp\" & "MELOpenClosedCountDaily" & "_" & PDFNo.ToString & ".pdf"
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

    End Sub
    Public Sub ShowRepeatitiveDefects()

        Dim da As New CSLA.Data.ObjectAdapter
        Dim mCompanyDetail As CompanyDetail
        Dim ReportName As String = String.Empty
        Dim ds As New dsReliabilityReport
        Dim mrptImage As rptImage

        Dim tmpDate As Date = DateAdd(DateInterval.Month, -2, DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, 1))
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        'StartDateM = New SmartDate(DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, 1), False)
        'EndDateM = New SmartDate(CStr(DateSerial(tmpDate.Year, tmpDate.Month, DateTime.DaysInMonth(tmpDate.Year, tmpDate.Month))), False)

        StartDateM = New SmartDate(CStr(DateSerial(tmpDate.Year, tmpDate.Month, 1)), False)
        EndDateM = New SmartDate(DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, DateTime.DaysInMonth(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1)), False)

        Dim Report As New ReportData(mCompanyDetail.CompanyName,
                                             mCompanyDetail.Address,
                                             mCompanyDetail.Tel1,
                                             mCompanyDetail.Tel2,
                                             mCompanyDetail.Fax,
                                             mCompanyDetail.Email,
                                             mCompanyDetail.WebSite,
                                             "",
                                             AppSettings("ClientCode"),
                                             "",
                                             "",
                                             "",
                                             "",
                                             AppSettings("Product Version"),
                                             AppSettings("SINote"),
                                             "",
                                             SearchStr7:="",
                                             SearchStr8:=mModelNames.ToString.Trim.TrimEnd(","),
                                             SearchStr9:=mMachineNames.ToString.Trim.TrimEnd(","),
                                             SearchStr10:=cmbMonth.SelectedItem.Text + " , " + cmbYear.SelectedItem.Text)


        mReliabilityRepeatitiveDefectList = ReliabilityRepeatitiveDefectList.GetReliabilityRepeatitiveDefectList(, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString)

        '1) Repeatitive DefectList
        If mReliabilityRepeatitiveDefectList.Count > 0 Then

            ReportName = "RELIABILITY REPORT"
            Dim myReport = New crReliabilityRepeatitiveDefectList
            ds.Clear()
            da.Fill(ds, "ReliabilityRepeatitiveDefectList", mReliabilityRepeatitiveDefectList)

            da.Fill(ds, "ReportData", Report)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport

            Dim MyFile1 = ""
            Dim myExportOption As CrystalDecisions.Shared.ExportOptions
            Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

            MyFile1 = "C:\Temp\" & "ReliabilityTempRepeatitiveDefectList" & "_" & PDFNo.ToString & ".pdf"
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

    End Sub
    Public Sub ShowLifedOnConditionItems()

        Dim da As New CSLA.Data.ObjectAdapter
        Dim mCompanyDetail As CompanyDetail
        Dim ReportName As String = String.Empty
        Dim ds As New dsReliabilityReport
        Dim mrptImage As rptImage

        Dim tmpDate As Date = DateAdd(DateInterval.Month, -2, DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, 1))
        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
        'StartDateM = New SmartDate(DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, 1), False)
        'EndDateM = New SmartDate(CStr(DateSerial(tmpDate.Year, tmpDate.Month, DateTime.DaysInMonth(tmpDate.Year, tmpDate.Month))), False)

        StartDateM = New SmartDate(CStr(DateSerial(tmpDate.Year, tmpDate.Month, 1)), False)
        EndDateM = New SmartDate(DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, DateTime.DaysInMonth(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1)), False)

        ReportName = "RELIABILITY REPORT"

        Dim Report As New ReportData(mCompanyDetail.CompanyName,
                                             mCompanyDetail.Address,
                                             mCompanyDetail.Tel1,
                                             mCompanyDetail.Tel2,
                                             mCompanyDetail.Fax,
                                             mCompanyDetail.Email,
                                             mCompanyDetail.WebSite,
                                             "",
                                             AppSettings("ClientCode"),
                                             "",
                                             "",
                                             "",
                                             "",
                                             AppSettings("Product Version"),
                                             AppSettings("SINote"),
                                             "",
                                             SearchStr7:="",
                                             SearchStr8:=mModelNames.ToString.Trim.TrimEnd(","),
                                             SearchStr9:=mMachineNames.ToString.Trim.TrimEnd(","),
                                             SearchStr10:=cmbMonth.SelectedItem.Text + " , " + cmbYear.SelectedItem.Text)


        If AppSettings("ClientCode") = "7AR" Then 'Added by Saylee on 31-Jul-2025 as 7AR does not have Lifed On Condition Items

            Dim mReliabilityUnscheduleComponentRemovalList As rptReliabilityUnscheduleCompRemovalList
            mReliabilityUnscheduleComponentRemovalList = rptReliabilityUnscheduleCompRemovalList.GetReliabilityUnscheduleCompRemoval(cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString)

            If mReliabilityUnscheduleComponentRemovalList.Count > 0 Then
                Dim myReport = New crptReliabilityUnscheduleCompRemovalList
                ds.Clear()
                da.Fill(ds, "rptReliabilityUnscheduleCompRemovalList", mReliabilityUnscheduleComponentRemovalList)
                da.Fill(ds, "ReportData", Report)
                mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
                da.Fill(ds, mrptImage)
                myReport.SetDataSource(ds)
                Session("CrystalReport") = myReport
                Dim MyFile1 = ""
                Dim myExportOption As CrystalDecisions.Shared.ExportOptions
                Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions
                MyFile1 = "C:\Temp\" & "ReliabilityUnscheduleCompRemovalList" & "_" & PDFNo.ToString & ".pdf"
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

        Else

            mReliabilityLifedComponentPrematureFailure = ReliabilityLifedComponentPrematureFailure.GetReliabilityLifedComponentPrematureFailure(, cmbMonth.SelectedIndex + 1, cmbYear.SelectedItem.Value, ModelIDs.ToString, MachineIDs.ToString)
            mReliabilityOCComponentPrematureFailure = ReliabilityOCComponentPrematureFailure.GetReliabilityOCComponentPrematureFailure(, cmbMonth.SelectedIndex + 1, cmbYear.SelectedItem.Value, ModelIDs.ToString, MachineIDs.ToString)

            If mReliabilityLifedComponentPrematureFailure.Count > 0 Then


                Dim myReport = New crReliabilityLifedConditionCompFailure
                ds.Clear()
                da.Fill(ds, "ReliabilityLifedComponentPrematureFailure", mReliabilityLifedComponentPrematureFailure)

                da.Fill(ds, "ReportData", Report)
                mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
                da.Fill(ds, mrptImage)

                myReport.SetDataSource(ds)

                Session("CrystalReport") = myReport

                Dim MyFile1 = ""
                Dim myExportOption As CrystalDecisions.Shared.ExportOptions
                Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

                MyFile1 = "C:\Temp\" & "ReliabilityTempLifedComponentPrematureFailure" & "_" & PDFNo.ToString & ".pdf"
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

            If mReliabilityOCComponentPrematureFailure.Count > 0 Then


                Dim myReport = New crReliabilityOnConditionCompFailure
                ds.Clear()

                da.Fill(ds, "ReliabilityOCComponentPrematureFailure", mReliabilityOCComponentPrematureFailure)
                da.Fill(ds, "ReportData", Report)
                mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
                da.Fill(ds, mrptImage)

                myReport.SetDataSource(ds)

                Session("CrystalReport") = myReport

                Dim MyFile1 = ""
                Dim myExportOption As CrystalDecisions.Shared.ExportOptions
                Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

                MyFile1 = "C:\Temp\" & "ReliabilityTempLifedComponentPrematureFailure" & "_" & PDFNo.ToString & ".pdf"
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

        End If

    End Sub

    Public Sub SetSelectedReports()

        'Fleet Hours and Cycles
        If chkFleetHoursAndCycles.Checked Then
            ShowFleetHoursAndCycles()
        End If

        If chkAircraftOnGroundStatusReport.Checked Then
            ShowAircraftOnGroundStatusReport()
        End If
        If chkReliabilitySummary.Checked Then
            ShowReliabiltiySummaryReport()
        End If

        If chkFlyingHoursRecord.Checked Then
            ShowFlyingHoursRecord()
        End If
        If chkAircraftStatus.Checked Then
            ShowAircraftStatusRecord()
        End If

        If chkUtilization.Checked Then
            ShowUtilization()
        End If

        If chkDelayCancellation.Checked Then
            ShowDelayCancellation()
        End If

        If chkPIREP.Checked Or chkMaintenanceDefect.Checked Then
            ShowPirepsMaintDefect()
        End If

        'If chkMaintenanceDefect.Checked Then
        '    ShowMaintenanceDefects()
        'End If

        If chkRepeatitive.Checked Then
            ShowRepeatitiveDefects()
        End If

        If chkMEL.Checked Then
            ShowMEL()
        End If
        If chkLifedOnConditionItems.Checked Then
            ShowLifedOnConditionItems
        End If
    End Sub
    Private Sub SetReportNew(Optional ByVal ByMail As Boolean = False)
        Try
            ''     ScriptManager.RegisterStartupScript(Me, Me.GetType(), "redirectToDinoGame", "redirectToDinoGame();", True)


            Dim da As New CSLA.Data.ObjectAdapter
            Dim mCompanyDetail As CompanyDetail
            Dim ReportName As String = String.Empty
            Dim ds As New dsReliabilityReport
            Dim mrptImage As rptImage


            Dim tmpDate As Date = DateAdd(DateInterval.Month, -2, DateSerial(CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, 1))

            Dim StartDateM As New SmartDate(False)
            Dim EndDateM As New SmartDate(False)
            StartDateM.Text = CStr(DateAdd(DateInterval.Month, cmbMonth.SelectedIndex, DateSerial(CType(cmbYear.SelectedItem.Text, Integer), 1, 1)))
            EndDateM.Text = CStr(DateAdd("d", -1, DateAdd("m", 1, StartDateM.Date)))



            ReportName = "RELIABILITY REPORT"
            mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")
			'Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
			'       mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
			'       mCompanyDetail.WebSite, "", AppSettings("ClientCode"), cmbYear.SelectedItem.Text, "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", mModelNames.ToString.Trim.TrimEnd(","), mMachineNames.ToString.Trim.TrimEnd(","), SearchStr10:=AppSettings("Logo"), SearchStr11:=CDate(StartDateM.ToString).ToString("MMMM") + " - " + CDate(EndDateM.ToString).ToString("MMMM") + " " + CDate(EndDateM.Text).ToString("yyyy"))

			SetValues(IsSyncApplication:=mCompanyDetail.IsSyncApplication)

			Dim Report As New ReportData(mCompanyDetail.CompanyName,
                                         mCompanyDetail.Address,
                                         mCompanyDetail.Tel1,
                                         mCompanyDetail.Tel2,
                                         mCompanyDetail.Fax,
                                         mCompanyDetail.Email,
                                         mCompanyDetail.WebSite,
                                         "",
                                         AppSettings("ClientCode"),
                                         SearchStr2:="",
                                         SearchStr3:="",
                                         SearchStr4:="",
                                         SearchStr5:="",
                                         AppSettings("Product Version"),
                                         AppSettings("SINote"),
                                         SearchStr6:="",
                                         SearchStr7:="",
                                         SearchStr8:=mMachineNames.ToString.Trim.TrimEnd(","),
                                         SearchStr9:=mModelNames.ToString.Trim.TrimEnd(","),
                                         SearchStr10:=cmbMonth.SelectedItem.Text + " , " + cmbYear.SelectedItem.Text,
                                         SearchStr11:=mCompanyDetail.IsSyncApplication.ToString)



            'Page 1 : First Page

            Dim myReport = New crReliabilityReportNewFirstPage  'crDailyUtilizationGraph

            mReliabilityDistributionList = DistributionList.GetDistributionList(Guid.Empty, , , , IIf(ModelIDs.ToString = "", AircraftModelIDs.ToString, ModelIDs.ToString))

            ds.Clear()
            da.Fill(ds, "ReportData", Report)
            da.Fill(ds, mReliabilityDistributionList)
            mrptImage = rptImage.GetImage(ds, DataTableName:="rptImage")
            da.Fill(ds, mrptImage)

            myReport.SetDataSource(ds)

            Session("CrystalReport") = myReport
            PDFNo = 1
            '  PDFNoChild = 1
            Dim tmp As Integer
            Dim a As New Random
            Dim pageCount As Integer = 0

            pdfList = New System.Collections.ArrayList

            Dim MyFile1 = ""
            Dim myExportOption As CrystalDecisions.Shared.ExportOptions
            Dim myDiskOption As CrystalDecisions.Shared.DiskFileDestinationOptions

            MyFile1 = "C:\Temp\" & "ReliabilityTempNewFirstPage" & "_" & PDFNo.ToString & ".pdf"
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


            SetSelectedReports()




            '''''END: Merge ALL reports
            '''
            Dim MergedPath As String = "C:\Temp\" & "temp_myMergedPdf.pdf"
            '   Dim MergedPath_WM As String = "C:\Temp\" & "temp_myMergedPdf_WM.pdf"
            Dim MergedPath_WM As String = "C:\Temp\" & "Reliability Report.pdf"
            Dim filesByte As New List(Of Byte())()



            For Each file__1 As String In pdfList 'files
                filesByte.Add(File.ReadAllBytes(file__1))
            Next

            File.WriteAllBytes(MergedPath, Flypal.PDFMergers.MergeFiles(filesByte))


            'AddWatermarkText(MergedPath, MergedPath_WM, mnWO.WONumber, , , iTextSharp.text.BaseColor.GRAY, , 0.0, pageCount)
            AddWatermarkText(MergedPath, MergedPath_WM, "Page ", , , iTextSharp.text.BaseColor.BLACK, , 0.0, pageCount) 'Added on 24-Jun-2019
            ''//********************************************Set Sessions*********************************************************//
            Session("CrystalReport") = MergedPath_WM
            ' Session("PrintReportWithAttachment") = "True"

            '//*******************************************Delete created file*********************************************************//

            Dim DeleteThis As String = "ReliabilityTemp"
            Dim Files As String() = Directory.GetFiles("C:\Temp\")
            For Each file__1 As String In Files
                If file__1.ToUpper().Contains(DeleteThis.ToUpper()) Then
                    File.Delete(file__1)
                End If
            Next

            RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 1220)

            Session("CrystalReport") = MergedPath_WM
            Session("PrintReportWithAttachment") = "True"
            'myReport.SetDataSource(ds)
            If ByMail = False Then
                RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 1220)
            End If
            'If (ByMail = True And mrptReliabilityAircraftUtilization.TotalNoOfAircraft <= 0) Then
            '    SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, "There is no record for this search criteria.", "",
            '        Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"),
            '        ReportGenratedBy:=Session("ReportGenratedBy"),
            '        SmtpHost:=mModuleList.Item("Reliability").SmtpHost, SmtpPort:=mModuleList.Item("Reliability").SmtpPort,
            '        SmtpUser:=mModuleList.Item("Reliability").SmtpUser, SmtpPassword:=mModuleList.Item("Reliability").SmtpPassword)
            '    Exit Sub
            'End If


            If ByMail = False Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
            Else
                SendMailFile.SendMailFile(Nothing, Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, " For " + lblyear1.Text + ", " + lblModel1.Text, "",
                                          Session("ToSendMailIDs"), Session("CcSendMailIDs"), Session("CrystalReport"), True, Remark:=Session("SendMailRemark"),
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
    'Private Sub SetReport(Optional ByVal ByMail As Boolean = False)
    '    Try
    '        Dim da As New CSLA.Data.ObjectAdapter
    '        Dim mCompanyDetail As CompanyDetail
    '        Dim ReportName As String = String.Empty
    '        Dim ds As New dsReliabilityReport 'dsReliabilityFlyingHoursRecord
    '        'dsDailyUtilizationGraph   '
    '        ReportName = "Fleet Reliability Summary"

    '        mCompanyDetail = CompanyDetail.GetCompanyDetail("", "", "", "", "", "", "")

    '        SetValues()

    '        'Added by utkarsh on 10-dec-2013
    '        Dim mMonthwisePropellerStatus As MonthwisePropellerStatus
    '        Dim mMonthwiseRemovedPropellerStatus As MonthwiseRemovedPropellerStatus
    '        'End
    '        Dim mReliabilityMELReport As ReliabilityDefectReportedByPilot 'Added By Utkarsh ON 06-Jan-2014 FOR ALL03012014


    '        Dim myReport = New crReliabilityReport  'crDailyUtilizationGraph
    '        ' Dim myReport = New crDailyUtilizationGraph


    '        mReliabilityFlyingHoursRecord = ReliabilityFlyingHoursRecord.GetReliabilityFlyingHoursRecord(, Today.Date.ToString, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString)
    '        mReliabilityFlyingHoursRecordWithAircraft = ReliabilityFlyingHoursRecordWithAircraft.GetReliabilityFlyingHoursRecordWithAircraft(, Today.Date.ToString, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString)
    '        'Added By Utkarsh(IsPireps criteria) ON 02-May-2013 FOR ALL2052013
    '        mReliabilityDefectReportedByPilot = ReliabilityDefectReportedByPilot.GetReliabilityDefectReportedByPilot(, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), True, , ModelIDs.ToString, MachineIDs.ToString)
    '        'End
    '        mrptReliabilityAircraftUtilization = rptReliabilityAircraftUtilization.GetReliabilityAircraftUtilization(Guid.Empty, CType(cmbYear.SelectedItem.Text, Integer), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString)
    '        mReliabilityFleetHoursCycles = ReliabilityFleetHoursCycles.GetReliabilityFlyingHoursRecord(, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString)
    '        mReliabilityFleetHoursCyclesForAllModels = ReliabilityFleetHoursCyclesForAllModels.GetReliabilityFlyingHoursRecord(cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer))
    '        mReliabilityOCComponentPrematureFailure = ReliabilityOCComponentPrematureFailure.GetReliabilityOCComponentPrematureFailure(, cmbMonth.SelectedIndex + 1, cmbYear.SelectedItem.Value, ModelIDs.ToString, MachineIDs.ToString)
    '        mReliabilityLifedComponentPrematureFailure = ReliabilityLifedComponentPrematureFailure.GetReliabilityLifedComponentPrematureFailure(, cmbMonth.SelectedIndex + 1, cmbYear.SelectedItem.Value, ModelIDs.ToString, MachineIDs.ToString)

    '        '''''Added by Saylee on All-23042013 to show Distribution list
    '        mReliabilityDistributionList = DistributionList.GetDistributionList(Guid.Empty, , , , IIf(ModelIDs.ToString = "", AircraftModelIDs.ToString, ModelIDs.ToString))
    '        '''''Added By Utkarsh ON 24-Apr-2013 FOR All-24042013-1
    '        mrptMechanicalReliability = rptMechanicalReliability.GetMechanicalReliability(Guid.Empty, CInt(cmbYear.SelectedItem.Text), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString)
    '        '''''End

    '        '''''Added By Utkash ON 03-May-2013 FOR ALL03052013
    '        mDailyUtilizationGraphReport = DailyUtilizationGraphReport.GetDailyUtilizationGraph(Guid.Empty, CInt(cmbYear.SelectedItem.Text), cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString)
    '        '''''End
    '        mrptMonthlySnagCountATAWise = rptMonthlySnagCountATAWise.GetrptMonthlySnagCountATAWise(cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), , True, ModelIDs.ToString, MachineIDs.ToString)

    '        '''''Added By Shweta ON 27-May-2013 FOR ALL03052013
    '        mMonthwiseAircraftCurrentStatus = MonthwiseAircraftCurrentStatus.GetMonthwiseAircraftCurrentStatus(, cmbMonth.SelectedItem.Text, CType(cmbYear.SelectedItem.Text, Integer), Today.Date.ToString, cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString)
    '        mMonthwiseEngineStatus = MonthwiseEngineStatus.GetMonthwiseEngineStatus(, cmbMonth.SelectedItem.Text, CType(cmbYear.SelectedItem.Text, Integer), Today.Date.ToString, cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString)
    '        '''''changed by utkarsh on 10-dec-2013
    '        mMonthwiseAPUStatus = MonthwiseAPUStatus.GetMonthwiseAPUStatus(4, , cmbMonth.SelectedItem.Text, CType(cmbYear.SelectedItem.Text, Integer), Today.Date.ToString, cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString)
    '        mMonthwiseRemovedEngineStatus = MonthwiseRemovedEngineStatus.GetMonthwiseRemoveEngineStatus(Guid.Empty.ToString, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString)
    '        mMonthwiseRemovedAPUStatus = MonthwiseRemovedAPUStatus.GetMonthwiseRemoveAPUStatus(Guid.Empty.ToString, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString)
    '        'Added by utkarsh on 10-dec-2013
    '        mMonthwisePropellerStatus = MonthwisePropellerStatus.GetMonthwisePropellerStatus(3, Guid.Empty.ToString, cmbMonth.SelectedItem.Text, CType(cmbYear.SelectedItem.Text, Integer), Today.Date.ToString, cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString)
    '        mMonthwiseRemovedPropellerStatus = MonthwiseRemovedPropellerStatus.GetMonthwiseRemovePropellerStatus(Guid.Empty.ToString, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString)
    '        ''''End
    '        ''''Added By Utkarsh(IsPireps criteria) ON 02-May-2013 FOR ALL2052013
    '        mReliabilityMechanicalDefectRectification = ReliabilityDefectReportedByPilot.GetReliabilityDefectReportedByPilot(Guid.Empty.ToString, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), False, , ModelIDs.ToString, MachineIDs.ToString)
    '        ''''End
    '        ''''Added By Utkash ON 03-May-2013 FOR ALL03052013
    '        Dim StartDateM As New SmartDate
    '        Dim EndDateM As New SmartDate
    '        StartDateM.Text = CStr(DateAdd(DateInterval.Month, cmbMonth.SelectedIndex, DateSerial(CType(cmbYear.SelectedItem.Text, Integer), 1, 1)))
    '        EndDateM.Text = CStr(DateAdd("d", -1, DateAdd("m", 1, StartDateM.Date)))

    '        mFligthDelayAndCancellationList = FligthDelayAndCancellationList.GetFlightDCList(Guid.Empty, StartDateM.Text, EndDateM.Text, True, True, True, True, Guid.Empty.ToString, ModelIDs.ToString, MachineIDs.ToString, IsSyncApplication:=mCompanyDetail.IsSyncApplication)
    '        ''''End

    '        ''''Added By Utkarsh ON 05-Jun-2013 FOR ALL04062013
    '        mrptReliabilitySummary = rptReliabilitySummary.GetReliabilitySummary(CInt(cmbYear.SelectedItem.Text), Guid.Empty, cmbMonth.SelectedIndex + 1, ModelIDs.ToString, MachineIDs.ToString, IsSyncApplication:=mCompanyDetail.IsSyncApplication)
    '        ''''End

    '        ''''Added By Prashant ON 31-Jul-2013 FOR BA31072013
    '        mrptMonthlySnagCountATAWiseForMaintenanceDefect = rptMonthlySnagCountATAWiseForMaintenanceDefect.GetrptMonthlySnagCountATAWise(cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), Guid.Empty.ToString, False, ModelIDs.ToString, MachineIDs.ToString)
    '        ''''End
    '        ''''Added By Shweta ON 31-Jul-2013 FOR BAL31072013
    '        mMonthwiseAircraftOnGround = MonthwiseAircraftOnGround.GetMontMonthwiseAircraftOnGround(EndDateM.Text, , Guid.Empty.ToString, ModelIDs.ToString, MachineIDs.ToString)

    '        'Added By Vikrant On 31-July-2013 For BA31072013
    '        mrptReliabilityMonthlyATAWisePirepRate = rptReliabilityMonthlyATAWisePirepRate.GetMonthlyPirepRateATAWise(CType(cmbYear.SelectedItem.Text, Integer), Guid.Empty, cmbMonth.SelectedIndex + 1, True, ModelIDs.ToString, MachineIDs.ToString)

    '        mrptReliabilityMonthlyATAWiseDefectRate = rptReliabilityMonthlyATAWiseMaintenanceDefectRate.GetMonthlyDefectRateATAWise(CType(cmbYear.SelectedItem.Text, Integer), Guid.Empty, cmbMonth.SelectedIndex + 1, False, ModelIDs.ToString, MachineIDs.ToString)

    '        ''' 'End
    '        ''' 'Added By Utkarsh ON 06-Jan-2014 FOR ALL03012014
    '        mReliabilityMELReport = ReliabilityDefectReportedByPilot.GetReliabilityDefectReportedByPilot(, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), False, True, ModelIDs.ToString, MachineIDs.ToString)
    '        'End

    '        Dim mReliabilityRepeatitiveDefectList As ReliabilityRepeatitiveDefectList
    '        mReliabilityRepeatitiveDefectList = ReliabilityRepeatitiveDefectList.GetReliabilityRepeatitiveDefectList(, cmbMonth.SelectedIndex + 1, CType(cmbYear.SelectedItem.Text, Integer), ModelIDs.ToString, MachineIDs.ToString)



    '        Dim Report As New ReportData(mCompanyDetail.CompanyName, mCompanyDetail.Address,
    '                 mCompanyDetail.Tel1, mCompanyDetail.Tel2, mCompanyDetail.Fax, mCompanyDetail.Email,
    '                 mCompanyDetail.WebSite, "", AppSettings("ClientCode"), "", "", "", "", AppSettings("Product Version"), AppSettings("SINote"), "", "", mModelNames.ToString.Trim.TrimEnd(","), mMachineNames.ToString.Trim.TrimEnd(","), cmbMonth.SelectedItem.Text + " , " + cmbYear.SelectedItem.Text)


    '        'myReport.SetDataSource(ds)
    '        If ByMail = False Then
    '            If mrptReliabilityAircraftUtilization.TotalNoOfAircraft = 0 Then
    '                MSGBoxCtrl.show(MSGBox.Message_title.NoRecordFound, MSGBox.Message_text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
    '                Exit Sub
    '            Else
    '                RecentMenuEvent.RecentMenuItemEvent(Thread.CurrentPrincipal.Identity.Name, 1220)
    '            End If
    '        End If
    '        If (ByMail = True And mrptReliabilityAircraftUtilization.TotalNoOfAircraft <= 0) Then
    '            SendMailFile.SendMailFile(, Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, "There is no record for this search criteria.", "",
    '                Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"),
    '                ReportGenratedBy:=Session("ReportGenratedBy"),
    '                SmtpHost:=mModuleList.Item("Reliability").SmtpHost, SmtpPort:=mModuleList.Item("Reliability").SmtpPort,
    '                SmtpUser:=mModuleList.Item("Reliability").SmtpUser, SmtpPassword:=mModuleList.Item("Reliability").SmtpPassword)
    '            Exit Sub
    '        End If
    '        Dim mrptImage As rptImage = rptImage.GetImage(ds)
    '        ds.Clear()
    '        da.Fill(ds, mReliabilityFlyingHoursRecord)
    '        da.Fill(ds, mReliabilityFlyingHoursRecordWithAircraft)
    '        da.Fill(ds, mReliabilityDefectReportedByPilot)
    '        da.Fill(ds, mrptReliabilityAircraftUtilization)
    '        da.Fill(ds, mReliabilityFleetHoursCycles)
    '        da.Fill(ds, mReliabilityFleetHoursCyclesForAllModels)
    '        da.Fill(ds, mReliabilityOCComponentPrematureFailure)
    '        da.Fill(ds, mReliabilityLifedComponentPrematureFailure)
    '        da.Fill(ds, mReliabilityDistributionList) 'Added by Saylee on All-23042013 to show Distribution list
    '        da.Fill(ds, mrptMechanicalReliability) 'Added By Utkarsh ON 24-Apr-2013 FOR All-24042013-1
    '        '''''''''Added By Utkash ON 03-May-2013 FOR ALL03052013
    '        da.Fill(ds, mDailyUtilizationGraphReport)
    '        da.Fill(ds, mrptMonthlySnagCountATAWise)
    '        da.Fill(ds, mMonthwiseAircraftCurrentStatus)
    '        da.Fill(ds, mMonthwiseEngineStatus)
    '        da.Fill(ds, mMonthwiseAPUStatus)
    '        da.Fill(ds, mFligthDelayAndCancellationList)

    '        da.Fill(ds, mrptImage)
    '        da.Fill(ds, mMonthwiseRemovedEngineStatus)  'Added By Shweta ON 27-May-2013 FOR ALL03052013
    '        da.Fill(ds, mMonthwiseRemovedAPUStatus)     'Added By Shweta ON 27-May-2013 FOR ALL03052013
    '        da.Fill(ds, mrptReliabilitySummary)         'Added By Utkarsh ON 05-Jun-2013 FOR ALL04062013
    '        da.Fill(ds, mrptMonthlySnagCountATAWiseForMaintenanceDefect) 'Added By Prashant ON 31-Jul-2013 FOR BAL31072013
    '        da.Fill(ds, mMonthwiseAircraftOnGround) 'Added By Shweta ON 31-Jul-2013 FOR BAL31072013
    '        'Added By Vikrant On 31-July-2013 For BA31072013
    '        da.Fill(ds, mrptReliabilityMonthlyATAWisePirepRate)
    '        da.Fill(ds, mrptReliabilityMonthlyATAWiseDefectRate)
    '        'End
    '        'Added by utkarsh on 10-dec-2013
    '        da.Fill(ds, mMonthwisePropellerStatus)
    '        da.Fill(ds, mMonthwiseRemovedPropellerStatus)
    '        '''''''''End
    '        da.Fill(ds, mReliabilityMechanicalDefectRectification)
    '        da.Fill(ds, mReliabilityMELReport) 'Added By Utkarsh ON 06-Jan-2014 FOR ALL03012014
    '        da.Fill(ds, Report)
    '        da.Fill(ds, "ReliabilityRepeatitiveDefectList", mReliabilityRepeatitiveDefectList)
    '        myReport.SetDataSource(ds)


    '        With myReport
    '            If mReliabilityFleetHoursCyclesForAllModels.Count = 0 Then
    '                .Section7.SectionFormat.EnableSuppress = True
    '            End If
    '            If mReliabilityFlyingHoursRecordWithAircraft.Count = 0 Then
    '                .Section12.SectionFormat.EnableSuppress = True
    '            End If
    '            'Added By Utkarsh ON 02-May-2013 FOR ALL2052013
    '            If Not mReliabilityDefectReportedByPilot.ShowPireps Then
    '                .Section11.SectionFormat.EnableSuppress = True
    '            End If
    '            'End
    '            If mReliabilityLifedComponentPrematureFailure.Count = 0 Then
    '                .Section15.SectionFormat.EnableSuppress = True
    '            End If
    '            If mReliabilityOCComponentPrematureFailure.Count = 0 Then
    '                .Section16.SectionFormat.EnableSuppress = True
    '            End If
    '            If mReliabilityDistributionList.Count = 0 Then 'Added by Saylee on All-23042013 to show Distribution list
    '                .Section9.SectionFormat.EnableSuppress = True
    '            End If
    '            'Added By Utkash ON 03-May-2013 FOR ALL03052013
    '            If mDailyUtilizationGraphReport.Count = 0 Then
    '                .Section6.SectionFormat.EnableSuppress = True
    '            End If
    '            'End
    '            If mMonthwiseAircraftCurrentStatus.Count = 0 Then
    '                .Section20.SectionFormat.EnableSuppress = True
    '            End If

    '            If mMonthwiseEngineStatus.Count = 0 Then
    '                .Section21.SectionFormat.EnableSuppress = True
    '            End If

    '            If mMonthwiseAPUStatus.Count = 0 Then
    '                .Section23.SectionFormat.EnableSuppress = True
    '            End If

    '            If mrptMonthlySnagCountATAWise.Count = 0 Then
    '                .Section37.SectionFormat.EnableSuppress = True
    '            End If
    '            'Added By Utkarsh ON 02-May-2013 FOR ALL2052013
    '            If Not mReliabilityMechanicalDefectRectification.ShowDefectRectification Then
    '                .Section18.SectionFormat.EnableSuppress = True
    '            End If
    '            ' End
    '            'Added By Utkash ON 03-May-2013 FOR ALL03052013
    '            If Not mFligthDelayAndCancellationList.ShowDelays Then
    '                .Section35.SectionFormat.EnableSuppress = True
    '            End If
    '            If Not mFligthDelayAndCancellationList.ShowCancellations Then
    '                .Section25.SectionFormat.EnableSuppress = True
    '            End If
    '            'End

    '            ''Added By Shweta ON 27-May-2013 FOR ALL03052013
    '            If mMonthwiseRemovedEngineStatus.Count = 0 Then
    '                .Section26.SectionFormat.EnableSuppress = True
    '            Else
    '                .Section28.SectionFormat.EnableSuppress = True
    '            End If
    '            If mMonthwiseRemovedAPUStatus.Count = 0 Then
    '                .Section27.SectionFormat.EnableSuppress = True
    '            Else
    '                .Section29.SectionFormat.EnableSuppress = True
    '            End If

    '            If mrptMonthlySnagCountATAWiseForMaintenanceDefect.Count = 0 Then 'Added By Prashant ON 31-Jul-2013 FOR BA31072013
    '                .Section22.SectionFormat.EnableSuppress = True
    '            End If

    '            If mMonthwiseAircraftOnGround.Count = 0 Then 'Added By Shweta ON 31-Jul-2013 FOR BA31072013
    '                .Section33.SectionFormat.EnableSuppress = True
    '            End If
    '            'Added by utkarsh on 10-dec-2013
    '            If mMonthwisePropellerStatus.Count = 0 Then
    '                .Section17.SectionFormat.EnableSuppress = True
    '            End If
    '            If mMonthwiseRemovedPropellerStatus.Count = 0 Then
    '                .Section19.SectionFormat.EnableSuppress = True
    '            Else
    '                .Section24.SectionFormat.EnableSuppress = True
    '            End If
    '            'End
    '            'Added By Utkarsh ON 06-Jan-2014 FOR ALL03012014
    '            If mReliabilityMELReport.Count = 0 Then
    '                .DetailSection1.SectionFormat.EnableSuppress = True
    '            End If
    '            'End
    '            'Added By Saylee ON 8-Jul-2022
    '            If mReliabilityRepeatitiveDefectList.Count = 0 Then
    '                .DetailSection2.SectionFormat.EnableSuppress = True
    '            End If
    '            'End

    '        End With

    '        Session("CrystalReport") = myReport
    '        If ByMail = False Then
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openTranDetail", "openTranDetail();", True)
    '        Else
    '            SendMailFile.SendMailFile(Session("CrystalReport"), Thread.CurrentPrincipal.Identity.Name, ReportName, ReportName, " For " + lblyear1.Text + ", " + lblModel1.Text, "",
    '                                      Session("ToSendMailIDs"), Session("CcSendMailIDs"), "", True, Remark:=Session("SendMailRemark"),
    '                                      ReportGenratedBy:=Session("ReportGenratedBy"),
    '                SmtpHost:=mModuleList.Item("Reliability").SmtpHost, SmtpPort:=mModuleList.Item("Reliability").SmtpPort,
    '                SmtpUser:=mModuleList.Item("Reliability").SmtpUser, SmtpPassword:=mModuleList.Item("Reliability").SmtpPassword)
    '        End If
    '    Catch ex As Exception
    '        Dim Day, Month, Year As String
    '        Day = Format(Today.Date.Day, "0#")
    '        Month = Format(Today.Date.Month, "0#")
    '        Year = Format(Today.Date.Year, "0#")
    '        Dim todaydate As String = Day & Month & Year
    '        Dim Path As String = AppSettings("DOCPath") & todaydate
    '        FileOpen(1, Path, OpenMode.Append, OpenAccess.ReadWrite)
    '        FileSystem.WriteLine(1, Date.Now.ToString + " Mail service (SetReport Sub Method): " + ex.GetBaseException.Message + vbLf)
    '        FileClose(1)
    '    End Try
    'End Sub
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
        mModelList = Session("mModelList")
        If Not Page.IsPostBack Then
            SetCombo()
            DataFieldBinding()

        End If
        spnlabel.Visible = IIf(AppSettings("ClientCode") = "SAA", True, False)

        phhideselection.Visible = IIf(AppSettings("ClientCode") = "SAA", False, True)
        lblStep4.InnerText = IIf(AppSettings("ClientCode") = "SAA", "Step III. Display Report", "Step IV. Display Report")
        chkLifedOnConditionItems.Text = IIf(AppSettings("ClientCode") = "7AR", "Un-Scheduled Component Removed Items", "Lifed & On Condition Items")
		chkAircraftOnGroundStatusReport.Visible = IIf(AppSettings("ClientCode") = "7AR", False, True)
		mModuleList = Session("mModuleList") 'Added by shital on 06-Nov-2019 for Add EMailIDs field in csTransType 
		phModel.Visible = IIf(AppSettings("ClientCode") = "7AR", False, True)
		lblPrimaryStar1.Visible = IIf(AppSettings("ClientCode") = "7AR", True, False)
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
                'SetReport(False)
                SetReportNew(False)
                ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "clearTimeout", "onReportGenerationComplete();", True)
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
                Dim email As New Thread(Sub() SetReportNew(True))
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

    Private Sub chkAll_CheckedChanged(sender As Object, e As EventArgs) Handles chkAll.CheckedChanged
        If chkAll.Checked Then
            chkFleetHoursAndCycles.Checked = True
            chkAircraftOnGroundStatusReport.Checked = True
            chkReliabilitySummary.Checked = True
            chkFlyingHoursRecord.Checked = True
            chkAircraftStatus.Checked = True
            chkUtilization.Checked = True
            chkDelayCancellation.Checked = True
            ChkPIREP.Checked = True
            chkMaintenanceDefect.Checked = True
            chkLifedOnConditionItems.Checked = True
            chkRepeatitive.Checked = True
            chkMEL.Checked = True
        Else
            chkFleetHoursAndCycles.Checked = False
            chkAircraftOnGroundStatusReport.Checked = False
            chkReliabilitySummary.Checked = False
            chkFlyingHoursRecord.Checked = False
            chkAircraftStatus.Checked = False
            chkUtilization.Checked = False
            chkDelayCancellation.Checked = False
            ChkPIREP.Checked = False
            chkMaintenanceDefect.Checked = False
            chkLifedOnConditionItems.Checked = False
            chkRepeatitive.Checked = False
            chkMEL.Checked = False
        End If
        upnlCheckBoxSelection.Update()
    End Sub

    Private Sub cmbPrimaryModel_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPrimaryModel.SelectedIndexChanged
        If sender.ID = "cmbPrimaryModel" Then
            'For Model
            mModelList = ModelList.GetAirframeModelList(PrimaryModelId:=cmbPrimaryModel.SelectedValue)
            'end
            ListModel.DataSource = mModelList
            'cmbModel.DataSource = mModelList
            Session("mModelList") = mModelList
            'cmbModel.DataBind()
            ListModel.DataBind()
			If ListModel.Items.Count > 0 Then
				For Each ListModelItem As ListItem In ListModel.Items
					ListModelItem.Selected = True
				Next
			End If

			'For AirCraft
			mMachineNameValueList = MachineNameValueList.GetMachineList(Now.Date.ToString(AppSettings("DateFormat")), , , , , , , , , , True, PrimaryModelID:=cmbPrimaryModel.SelectedValue)
            ListRegNo.DataSource = mMachineNameValueList
            Session("mMachineNameValueList") = mMachineNameValueList
            ListRegNo.DataBind()
        End If
    End Sub
#End Region

End Class