'********************************************
'Created by:    Harsh
'Created on:    12th March 2024
'Created for:   Discrepancy Register Report
'Modified by Harsh Sugandhi on 18th September 2024 for FLYPAL-2619 Cabin Defect Module.
'********************************************

Imports System.Collections.Generic

Public Class DiscrepancyRegister
	Inherits Page

#Region " Varriable(s) "

	Public MachineNameValueList As MachineNameValueList
	Public ATAList As ATAList

	Dim FromDate As String
	Dim ToDate As String
	Dim Aircraft As String
	Dim ATAChapter As String
	Dim MELSnagCategory As Integer
	Dim InvestigationStatus As Integer
	Dim IsMajorMinor As Integer
	Dim Discrepancy As String
	Dim PirepsOrMaintenanceDefectType As Integer

	Dim rptParaAircraft As String
	Dim rptParaATAChapter As String
	Dim rptParaMELSnagCategory As String
	Dim rptParaStatus As String
	Dim rptParaMajorMinor As String
	Dim rptParamPirepsOrMaintenanceDefectType As String
	Dim SearchCriteria As String = String.Empty
	Dim TransTypeID As Integer

#End Region

#Region " Session Method(s) "

	Private Sub GetSession()

		MachineNameValueList = CType(Session("MachineNameValueList"), MachineNameValueList)
		ATAList = CType(Session("ATAList"), ATAList)
		TransTypeID = Session("TransTypeID")

	End Sub

	Private Sub RemoveSession()

		Session.Remove("MachineNameValueList")
		Session.Remove("ATAList")

	End Sub

#End Region

#Region " Data Binding "

	Private Sub DataFieldBind()

		Try

			MachineNameValueList = MachineNameValueList.GetMachineList(CurrentDate:=Today.Date.ToString, , , , , , ,
																	   IsTagRequired:=True,
																	   TagText:="(ALL)", ,
																	   SkipIsForInventoryAircarft:=True)

			ATAList = ATAList.GetATAList(ATANomenclature:="",
										 AddTopItem:="(ALL)")

			ddlAircraft.DataSource = MachineNameValueList
			ddlATAChapter.DataSource = ATAList
			ddlAircraft.DataBind()
			ddlATAChapter.DataBind()

			Session("MachineNameValueList") = MachineNameValueList
			Session("ATAList") = ATAList

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " Helper Method(s) "

	Private Sub SetSearchCriteriaLabels()

		Try

			If Not IsDate(txtFromDate.Text) Then
				FromDate = New SmartDate(Today.ToString()).FormattedText
				lblSearchCriteriaFromDate.Text = $"From Date : "
			Else
				FromDate = txtFromDate.Text.ToString()
				lblSearchCriteriaFromDate.Text = $"From Date :  {New SmartDate(txtFromDate.Text.ToString()).FormattedText}"
			End If

			If Not IsDate(txtToDate.Text) Then
				ToDate = New SmartDate(Today.ToString()).FormattedText
				lblSearchCriteriaToDate.Text = $"To Date : "
			Else
				ToDate = txtToDate.Text.ToString()
				lblSearchCriteriaToDate.Text = $"To Date : {New SmartDate(txtToDate.Text.ToString()).FormattedText}"
			End If

			If ddlAircraft.SelectedIndex = 0 Then
				Aircraft = "00000000-0000-0000-0000-000000000000"
				rptParaAircraft = "ALL"
				lblSearchCriteriaAircraft.Text = $"Aircraft : ALL"
			Else
				Aircraft = ddlAircraft.SelectedValue.ToString()
				rptParaAircraft = ddlAircraft.SelectedItem.Text.ToString()
				lblSearchCriteriaAircraft.Text = $"Aircraft : {ddlAircraft.SelectedItem.Text.ToString()}"
			End If

			If ddlATAChapter.SelectedIndex = 0 Then
				ATAChapter = "00000000-0000-0000-0000-000000000000"
				rptParaATAChapter = "ALL"
				lblSearchCriteriaATAChapter.Text = $"ATA Chapter : ALL"
			Else
				ATAChapter = ddlATAChapter.SelectedValue.ToString()
				rptParaATAChapter = ddlATAChapter.SelectedItem.Text.ToString()
				lblSearchCriteriaATAChapter.Text = $"ATA Chapter : {ddlATAChapter.SelectedItem.Text.ToString()}"
			End If

			If ddlMELSnag.SelectedIndex = 0 Then
				MELSnagCategory = 0
				rptParaMELSnagCategory = "ALL"
				lblSearchCriteriaMELSnag.Text = $"MEL / Deviation : ALL"
			Else
				MELSnagCategory = ddlMELSnag.SelectedValue
				rptParaMELSnagCategory = ddlMELSnag.SelectedItem.Text.ToString()
				lblSearchCriteriaMELSnag.Text = $"MEL / Deviation : {ddlMELSnag.SelectedItem.Text.ToString()}"
			End If

			If ddlStatus.SelectedIndex = 0 Then
				InvestigationStatus = 0
				rptParaStatus = "ALL"
				lblSearchCriteriaStatus.Text = $"Status : ALL"
			Else
				InvestigationStatus = ddlStatus.SelectedValue
				rptParaStatus = ddlStatus.SelectedItem.Text.ToString()
				lblSearchCriteriaStatus.Text = $"Status : {ddlStatus.SelectedItem.Text.ToString()}"
			End If

			If txtDiscrepancy.Text = "" Then
				Discrepancy = ""
				lblSearchCriteriaDiscrepancy.Text = IIf(TransTypeID = 116, "Cabin Defect :", "Discrepancy :")
			Else
				Discrepancy = txtDiscrepancy.Text
				lblSearchCriteriaDiscrepancy.Text = $"{IIf(TransTypeID = 116, "Cabin Defect :", "Discrepancy :")} {txtDiscrepancy.Text}"
			End If

			rptParaMajorMinor = ""

			If rbAll.Checked = True Then

				IsMajorMinor = 0  'ALL MAJOR AND MINOR
				rptParaMajorMinor = "ALL"
				lblSearchCriteriaDiscreapancyCategory.Text = $"Discrepancy Category : {rptParaMajorMinor}"

			ElseIf rbMajor.Checked = True Then

				IsMajorMinor = 1  'MAJOR
				rptParaMajorMinor = "Major"
				lblSearchCriteriaDiscreapancyCategory.Text = $"Discrepancy Category : {rptParaMajorMinor}"

			ElseIf rbMinor.Checked = True Then

				IsMajorMinor = 2  'MINOR
				rptParaMajorMinor = "Minor"
				lblSearchCriteriaDiscreapancyCategory.Text = $"Discrepancy Category : {rptParaMajorMinor}"

			ElseIf rbIncident.Checked = True Then

				IsMajorMinor = 3  'INCIDENT
				rptParaMajorMinor = "Incident"
				lblSearchCriteriaDiscreapancyCategory.Text = $"Discrepancy Category : {rptParaMajorMinor}"

			Else

				IsMajorMinor = 0  'ALL MAJOR, MINOR & INCIDENT
				rptParaMajorMinor = "ALL"
				lblSearchCriteriaDiscreapancyCategory.Text = $"Discrepancy Category : {rptParaMajorMinor}"

			End If

			If rbAllDefectType.Checked = True Then

				PirepsOrMaintenanceDefectType = 0  'All Pireps & Defect Type
				rptParamPirepsOrMaintenanceDefectType = "ALL"
				lblSearchCriteriaDefectType.Text = $"Pireps Or Maintenance : ALL"

			ElseIf rbIsPireps.Checked = True Then

				PirepsOrMaintenanceDefectType = 1  'Pireps
				rptParamPirepsOrMaintenanceDefectType = "Pireps"
				lblSearchCriteriaDefectType.Text = $"Pireps Or Maintenance : Pireps"

			Else

				PirepsOrMaintenanceDefectType = 2  'DEFECT type
				rptParamPirepsOrMaintenanceDefectType = "Maintenance Defect"
				lblSearchCriteriaDefectType.Text = $"Pireps Or Maintenance : Maintenance Defect"

			End If

			SearchCriteria = lblSearchCriteriaFromDate.Text.Trim() +
							 ", " + lblSearchCriteriaToDate.Text.Trim() +
							 ", " + lblSearchCriteriaAircraft.Text.Trim() +
							 ", " + lblSearchCriteriaATAChapter.Text.Trim() +
							 ", " + lblSearchCriteriaMELSnag.Text.Trim() +
							 ", " + lblSearchCriteriaStatus.Text.Trim() +
							 ", " + lblSearchCriteriaDiscrepancy.Text.Trim() +
							 ", " + lblSearchCriteriaDiscreapancyCategory.Text.Trim() +
							 ", " + lblSearchCriteriaDefectType.Text.Trim()
		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub DisplaySearchCriteriaLabels()

		Try

			lblSummary.Visible = True
			lblSearchCriteriaFromDate.Visible = True
			lblSearchCriteriaToDate.Visible = True
			lblSearchCriteriaAircraft.Visible = True
			lblSearchCriteriaATAChapter.Visible = True
			lblSearchCriteriaStatus.Visible = True
			lblSearchCriteriaDiscrepancy.Visible = True

			lblSearchCriteriaMELSnag.Visible = IIf(TransTypeID = 116, False, True)
			lblSearchCriteriaDiscreapancyCategory.Visible = IIf(TransTypeID = 116, False, True)
			lblSearchCriteriaDefectType.Visible = IIf(TransTypeID = 116, False, True)

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub SetReport(Optional ReportInExcel As Boolean = False)

		Dim dataSet As New dsDiscrepancyRegister
		Dim dataSetExcel As New DataSet
		Dim dataAdapter As New ObjectAdapter
		Dim CompanyDetail As New CompanyDetail
		Dim DiscrepancyRegisterReport As DiscrepancyRegisterReport
		Dim ReportAndModuleName As String = IIf(TransTypeID = 116, "Cabin Defect Register", "Discrepancy Register")
		Dim CrystalReport As Engine.ReportClass = IIf(TransTypeID = 116, New CabinDefectRegister, New crptDiscrepancyRegister)
		Dim ExcelTableAndSheetName As String = IIf(TransTypeID = 116, "Cabin Defect Register Report", "Discrepancy Register Report")

		Try

			SetSearchCriteriaLabels()

			DiscrepancyRegisterReport = DiscrepancyRegisterReport.GetDiscrepancyRegisterReport(FromDate:=FromDate,
																							   ToDate:=ToDate,
																							   Aircraft:=Aircraft,
																							   ATAChapter:=ATAChapter,
																							   MELSnagCategory:=MELSnagCategory,
																							   InvestigationStatus:=InvestigationStatus,
																							   Discrepancy:=Discrepancy,
																							   IsMajor:=IsMajorMinor,
																							   IsPireps:=PirepsOrMaintenanceDefectType,
																							   IsCabinDefect:=(TransTypeID = 116))

			If DiscrepancyRegisterReport.Count > 0 Then
				RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1550)
			Else

				MSGBoxCtrl.Show(MSGBox.Message_Title.NoRecordFound,
								MSGBox.Message_Text.NoRecordFound,
								"No records found for this Criteria.",
								MsgBoxStyle.OkOnly,
								"")
				Exit Sub

			End If

			Dim ReportData As New ReportData(CompanyName:=CompanyDetail.CompanyName,
											 Address:=CompanyDetail.Address,
											 Tel1:=CompanyDetail.Tel1,
											 Tel2:=CompanyDetail.Tel2,
											 Fax:=CompanyDetail.Fax,
											 Email:=CompanyDetail.Email,
											 WebSite:=CompanyDetail.WebSite,
											 ReportName:=ReportAndModuleName,
											 ProductVersion:=AppSettings("Product Version"),
											 SINote:=AppSettings("SINote"),
											 SearchStr1:=FromDate,
											 SearchStr2:=ToDate,
											 SearchStr3:=rptParaAircraft,
											 SearchStr4:=rptParaATAChapter,
											 SearchStr5:=rptParaMELSnagCategory,
											 SearchStr6:=rptParaStatus,
											 SearchStr7:=rptParaMajorMinor,
											 SearchStr8:=Discrepancy,
											 SearchStr9:=rptParamPirepsOrMaintenanceDefectType,
											 SearchStr10:="",
											 SearchStr11:="",
											 SearchStr12:="",
											 SearchStr13:=AppSettings("MELSnagNomenclature").ToString,
											 SearchStr14:=AppSettings("Logo"),
											 SearchStr15:=AppSettings("ClientCode"))

			dataSet.Clear()
			dataSetExcel.Clear()

			dataAdapter.Fill(dataSet, TableName:="ReportData", ReportData)
			dataAdapter.Fill(dataSet, TableName:="DiscrepancyRegister", DiscrepancyRegisterReport)

			If Not ReportInExcel Then

				Dim CompanyLogo As rptImage = rptImage.GetImage(dataSet)
				dataAdapter.Fill(dataSet, CompanyLogo)

				CrystalReport.SetDataSource(dataSet)

				Session("CrystalReport") = CrystalReport

			Else

				Dim ExcelColumnsToRemove As String()
				Dim CriteriaColumnToRemove As String()
				Dim ColumnOrdinal As String()
				Dim CriteriaColumnOrdinal As String()
				Dim ColumnRenames As New Dictionary(Of String, String)
				Dim CriteriaColumnRenames As New Dictionary(Of String, String)

				If TransTypeID = 116 Then

					CriteriaColumnToRemove = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite",
											  "CurrencyName", "CurrencySymbol", "SearchStr6", "SearchStr8",
											  "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13",
											  "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18",
											  "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23",
											  "SearchStr24", "SearchStr25", "SearchStr26", "SearchStr27", "SearchStr28",
											  "SearchStr29", "ProductVersion", "SINote", "SearchStr30",
											  "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35",
											  "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40",
											  "SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45",
											  "SearchStr46", "SearchStr47", "SearchStr48", "SearchStr49", "SearchStr50",
											  "SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",
											  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",
											  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",
											  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",
											  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75",
											  "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80",
											  "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85",
											  "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90",
											  "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95",
											  "SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100",
											  "ApprovalNo", "ShortName"}

					ExcelColumnsToRemove = {"ID", "MELSnagCorrectiveActionID", "MELLogID", "ATAChapterID", "LogID",
											"TroubleShootID", "CauseOfDefect", "ActionAgainstStaff", "InvestigationStatus",
											"LastMajorCheckHour", "SnagReportedBy", "MELCategoryName", "MELCategoryFrequency",
											"FrequencyInDays", "IsHours", "FrequencyInHours", "IsMEL", "IsRepetitive",
											"SubATADescription", "IsPireps", "IsInReliability", "SubCode", "WorkCarriedOutBy",
											"ExtensionApplied", "ExtensionInDays", "ExtensionApprovalNo", "FrequencyInCycles",
											"DueInCycles", "DeviationDescription", "DiscrepancyLogID", "DueInHours", "LogNo",
											"DueDateFormatted", "RectifiedDateFormatted", "DiscATACode", "DiscSubATACode",
											"PirepsMaintenanceDefectTag", "LogDateFormatted", "NextDue", "TroubleshootCount",
											"IsDeviationList", "RectifiedLogPageNo", "MELItemNo", "CDLItemSequenceNo", "ItemNo",
											"SerialNo", "Sector", "PartDescription", "ATAChapter", "ComponentHour", "PreventionTaken",
											"IsMajor", "DueDate", "RectifiedDate", "Remark", "SubATACode", "SubATANomenclature", "DeviationCategory",
											"LogText", "LogPageNo", "Place", "RecordNo", "ATASubATACode", "DefectReportNo", "No",
											"Status", "DateOfOccurrenceFormatted", "LogTextNo", "ATANomenclature", "Frequency",
											"IsIncident", "RectifiedLogText", "IsAOG", "MajorMinorOrIncident", "MELOrDeviation"}

					CriteriaColumnOrdinal = {"ReportName", "SearchStr1", "SearchStr2",
											 "SearchStr3", "SearchStr4", "SearchStr5",
											 "SearchStr7"}

					ColumnOrdinal = {"RegNo", "ATACode", "DiscrepancyLogNo",
									 "LogNoLogPageNo", "DateOfOccurrence", "Defect", "ReportedBy",
									 "Action", "LogDate", "RectifiedBy", "PartNo", "PartSerialNo",
									 "DiscrepancyStatusText", "Maintenance",
									 "NRCWONO", "DiscrepancyTextNo"}

					CriteriaColumnRenames.Add("ReportName", "Report Name")
					CriteriaColumnRenames.Add("SearchStr1", "From Date")
					CriteriaColumnRenames.Add("SearchStr2", "To Date")
					CriteriaColumnRenames.Add("SearchStr3", "Aircraft")
					CriteriaColumnRenames.Add("SearchStr4", "ATA Chapter")
					CriteriaColumnRenames.Add("SearchStr5", "Status")
					CriteriaColumnRenames.Add("SearchStr7", "Cabin Defect")

					ColumnRenames.Add("RegNo", "Aircraft Type")
					ColumnRenames.Add("ATACode", "ATA chapter")
					ColumnRenames.Add("DiscrepancyLogNo", "Cabin Defect Log No.")
					ColumnRenames.Add("LogNoLogPageNo", "Troubleshoot Log No.")
					ColumnRenames.Add("DateOfOccurrence", "Occurrence Date")
					ColumnRenames.Add("Defect", "Cabin Defect")
					ColumnRenames.Add("ReportedBy", "Reported Person")
					ColumnRenames.Add("LogDate", "Troubleshooting Date")
					ColumnRenames.Add("RectifiedBy", "Rectified Person")
					ColumnRenames.Add("PartNo", "Part No")
					ColumnRenames.Add("PartSerialNo", "Serial No")
					ColumnRenames.Add("DiscrepancyStatusText", "Cabin Defect Status")
					ColumnRenames.Add("Maintenance", "Troubleshooting Steps")
					ColumnRenames.Add("NRCWONO", "WO / NRC No.")
					ColumnRenames.Add("DiscrepancyTextNo", "Cabin Defect Text No")

				Else

					CriteriaColumnToRemove = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite",
											  "CurrencyName", "CurrencySymbol",
											  "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13",
											  "SearchStr14", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18",
											  "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23",
											  "SearchStr24", "SearchStr25", "SearchStr26", "SearchStr27", "SearchStr28",
											  "SearchStr29", "ProductVersion", "SINote", "SearchStr30",
											  "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35",
											  "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40",
											  "SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45",
											  "SearchStr46", "SearchStr47", "SearchStr48", "SearchStr49", "SearchStr50",
											  "SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55",
											  "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60",
											  "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65",
											  "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70",
											  "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75",
											  "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80",
											  "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85",
											  "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90",
											  "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95",
											  "SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100",
											  "ApprovalNo", "ShortName"}

					ExcelColumnsToRemove = {"ID", "MELSnagCorrectiveActionID", "MELLogID", "ATAChapterID", "LogID",
											"TroubleShootID", "CauseOfDefect", "ActionAgainstStaff", "InvestigationStatus",
											"LastMajorCheckHour", "SnagReportedBy", "MELCategoryName", "MELCategoryFrequency",
											"FrequencyInDays", "IsHours", "FrequencyInHours", "IsMEL", "IsRepetitive",
											"SubATADescription", "IsPireps", "IsInReliability", "SubCode", "WorkCarriedOutBy",
											"ExtensionApplied", "ExtensionInDays", "ExtensionApprovalNo", "FrequencyInCycles",
											"DueInCycles", "DeviationDescription", "DiscrepancyLogID", "DueInHours", "LogNo",
											"DueDateFormatted", "RectifiedDateFormatted", "DiscATACode", "DiscSubATACode",
											"PirepsMaintenanceDefectTag", "LogDateFormatted", "NextDue", "TroubleshootCount",
											"IsDeviationList", "RectifiedLogPageNo", "MELItemNo", "CDLItemSequenceNo", "ItemNo",
											"SerialNo", "Sector", "PartDescription", "ATAChapter", "ComponentHour", "PreventionTaken",
											"IsMajor", "DueDate", "RectifiedDate", "Remark", "SubATACode", "SubATANomenclature", "DeviationCategory",
											"LogText", "LogPageNo", "Place", "RecordNo", "ATASubATACode", "DefectReportNo", "No",
											"Status", "DateOfOccurrenceFormatted", "LogTextNo", "ATANomenclature", "Frequency",
											"IsIncident", "RectifiedLogText", "IsAOG"}

					CriteriaColumnOrdinal = {"ReportName", "SearchStr1", "SearchStr2", "SearchStr3",
											 "SearchStr4", "SearchStr5", "SearchStr6", "SearchStr8",
											 "SearchStr7"}

					ColumnOrdinal = {"RegNo", "MajorMinorOrIncident", "ATACode", "DiscrepancyLogNo",
									 "LogNoLogPageNo", "DateOfOccurrence", "Defect", "ReportedBy",
									 "Action", "LogDate", "RectifiedBy", "PartNo", "PartSerialNo",
									 "DiscrepancyStatusText", "MELOrDeviation", "Maintenance",
									 "NRCWONO", "DiscrepancyTextNo"}

					CriteriaColumnRenames.Add("ReportName", "Report Name")
					CriteriaColumnRenames.Add("SearchStr1", "From Date")
					CriteriaColumnRenames.Add("SearchStr2", "To Date")
					CriteriaColumnRenames.Add("SearchStr3", "Aircraft")
					CriteriaColumnRenames.Add("SearchStr4", "ATA Chapter")
					CriteriaColumnRenames.Add("SearchStr5", "MEL / Deviation")
					CriteriaColumnRenames.Add("SearchStr6", "Status")
					CriteriaColumnRenames.Add("SearchStr7", "Discrepancy")
					CriteriaColumnRenames.Add("SearchStr8", "Discrepancy Category")

					ColumnRenames.Add("RegNo", "Aircraft Type")
					ColumnRenames.Add("MajorMinorOrIncident", "Type of Defect")
					ColumnRenames.Add("ATACode", "ATA chapter")
					ColumnRenames.Add("DiscrepancyLogNo", "Discrepancy Log No.")
					ColumnRenames.Add("LogNoLogPageNo", "Troubleshoot Log No.")
					ColumnRenames.Add("DateOfOccurrence", "Occurrence Date")
					ColumnRenames.Add("Defect", "Discrepancy")
					ColumnRenames.Add("ReportedBy", "Reported Person")
					ColumnRenames.Add("LogDate", "Troubleshooting Date")
					ColumnRenames.Add("RectifiedBy", "Rectified Person")
					ColumnRenames.Add("PartNo", "Part No")
					ColumnRenames.Add("PartSerialNo", "Serial No")
					ColumnRenames.Add("DiscrepancyStatusText", "Discrepancy Status")
					ColumnRenames.Add("MELOrDeviation", "Category of Deferred")
					ColumnRenames.Add("Maintenance", "Troubleshooting Steps")
					ColumnRenames.Add("NRCWONO", "WO / NRC No.")
					ColumnRenames.Add("DiscrepancyTextNo", "Discrepancy Text No")

				End If

				For Each ColumnName As String In CriteriaColumnToRemove

					If dataSet.Tables("ReportData").Columns.Contains(ColumnName) Then
						dataSet.Tables("ReportData").Columns.Remove(ColumnName)
					End If

				Next

				For Each ColumnName As String In ExcelColumnsToRemove

					If dataSet.Tables("DiscrepancyRegister").Columns.Contains(ColumnName) Then
						dataSet.Tables("DiscrepancyRegister").Columns.Remove(ColumnName)
					End If

				Next

				For j As Integer = 0 To CriteriaColumnOrdinal.Length - 1

					If dataSet.Tables("ReportData").Columns.Contains(CriteriaColumnOrdinal(j)) Then
						dataSet.Tables("ReportData").Columns(CriteriaColumnOrdinal(j)).SetOrdinal(j)
					End If

				Next

				For i As Integer = 0 To ColumnOrdinal.Length - 1

					If dataSet.Tables("DiscrepancyRegister").Columns.Contains(ColumnOrdinal(i)) Then
						dataSet.Tables("DiscrepancyRegister").Columns(ColumnOrdinal(i)).SetOrdinal(i)
					End If

				Next

				For Each KVP As KeyValuePair(Of String, String) In CriteriaColumnRenames

					If dataSet.Tables("ReportData").Columns.Contains(KVP.Key) Then
						dataSet.Tables("ReportData").Columns(KVP.Key).ColumnName = KVP.Value
					End If

				Next

				For Each KVP As KeyValuePair(Of String, String) In ColumnRenames

					If dataSet.Tables("DiscrepancyRegister").Columns.Contains(KVP.Key) Then
						dataSet.Tables("DiscrepancyRegister").Columns(KVP.Key).ColumnName = KVP.Value
					End If

				Next

				dataSetExcel.Merge(dataSet.Tables("ReportData"))
				dataSetExcel.Merge(dataSet.Tables("DiscrepancyRegister"))

				dataSetExcel.Tables("ReportData").TableName = "Searching Criteria"
				dataSetExcel.Tables("DiscrepancyRegister").TableName = ExcelTableAndSheetName

				Session("ExcelFileName") = ExcelTableAndSheetName
				Session("dsNew") = dataSetExcel

			End If

			Dim key As String = IIf(ReportInExcel, "Display Excel Report", "Display Report")
			Dim script As String = IIf(ReportInExcel, "displayReportInExcel()", "displayReportInPDF()")
			Dim Detail As String = IIf(ReportInExcel, $" Export To Excel {SearchCriteria}", SearchCriteria)

			ScriptManager.RegisterStartupScript(Me,
												[GetType],
												key,
												script,
												True)

			MarkLog(Action.Print,
					ReportAndModuleName,
					Detail,
					ErrorType.NoError,
					Guid.Empty,
					EventLogID)

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub SetLabelsAndVisibility()

		Try

			lblHeader.Text = IIf(TransTypeID = 116, "Cabin Defect Register", "Discrepancy Register")
			lblDiscrepancy.Text = IIf(TransTypeID = 116, "Cabin Defect", "Discrepancy")

			lblSelectionOfStatus.Text = IIf(TransTypeID = 116, "Step IV. Selection of Status", "Step V. Selection of Status")
			lblSelectionOfDiscrepancy.Text = IIf(TransTypeID = 116, "Step V. Enter the keyword to search Cabin Defect", "Step VII. Enter the keyword to search Discrepancy")
			lblDisplayReport.Text = IIf(TransTypeID = 116, "Step VI. Display Report", "Step VIII. Display Report")

			phMelOrDeviationCategory.Visible = IIf(TransTypeID = 116, False, True)
			phDiscrepancyCategory.Visible = IIf(TransTypeID = 116, False, True)
			phDefectType.Visible = IIf(TransTypeID = 116, False, True)

		Catch ex As Exception
			Throw ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " Page Event(s) "

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		Try

			EventLogID = CType(Session("EventLogID"), Guid)
			GetSession()

			TransTypeID = IIf(Request.QueryString("TransTypeID") IsNot Nothing,
							  CInt(Request.QueryString("TransTypeID")),
							  115)

			Session("TransTypeID") = TransTypeID
			SetLabelsAndVisibility()

			If Not IsPostBack Then

				txtFromDate.Text = Now.Date.ToString(AppSettings("DateFormat").ToString())
				txtToDate.Text = Now.Date.ToString(AppSettings("DateFormat").ToString())

				DataFieldBind()

			End If

			MessageBoxResult()

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " Click Event(s) "

	Private Sub MSGBoxCtrl_UserControlButtonClicked(sender As Object, e As EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked

		Try

			MSGBoxCtrl.HideControl()
			MessageBoxResult()

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub DisplaySearchCriteria(sender As Object, e As EventArgs) Handles btnSearchCriteria.Click

		Try

			SetSearchCriteriaLabels()
			DisplaySearchCriteriaLabels()
			upnlSearchCriteria.Update()

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub DisplayReport(sender As Object, e As EventArgs) Handles btnDisplay.Click

		Try

			If Not IsValid Then

				upnlValidationErrors.Update()
				Exit Sub

			End If

			SetReport()

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub ExportToExcel(sender As Object, e As EventArgs) Handles btnExportToExcel.Click

		Try

			If Not IsValid Then

				upnlValidationErrors.Update()
				Exit Sub

			End If

			SetReport(ReportInExcel:=True)

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub CloseScreen(sender As Object, e As EventArgs) Handles btnClose.Click

		Try

			Session("MiddleFrame") = ""
			Response.Redirect("Dashboard.aspx")
			RemoveSession()

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " Message Box Event "

	Private Sub MessageBoxResult()

		Dim result As MsgBoxResult

		Try

			result = MSGBoxCtrl.Result

			If result >= 0 Then

				Select Case result
					Case MsgBoxResult.Ok

						DataFieldBind()

				End Select

			End If

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

#End Region

End Class