Public Class SearchCriteriaForMELSnagReportRegister
	Inherits Page

#Region " Variable Declaration "
	Dim mMachineNameValueList As MachineNameValueList 'Changed By Utkarsh On 19-Apr-2011
	Dim StartDate As String
	Dim EndDate As String
	Dim MachineID, ATAChapterID As String
	Dim Aircraft, ATAChapter, ATANomenclature As String
	Dim Status As String
	Dim ATACode As Integer
	Public mMELSnagCorrectiveActionRegisterReport As MELSnagCorrectiveActionRegisterReport
	Public mATAList As ATAList 'Added By Saylee on 12-Aug-2010
	Dim EventLogID As Guid 'Added by Prashant
	Dim mMELSnagReportRegisterSearchingCriteria As String = String.Empty
	Dim mIncidentTypeListMELSnagReportRegister As IncidentTypeList
	Dim TimeFormat As String ' Ajay on 25-Nov-2022
#End Region

#Region " Business Methods "
	Private Sub RemoveSession()
		Session.Remove("mMELSnagCorrectiveActionRegisterReport")
		Session.Remove("mMachineNameValueList")
		Session.Remove("mATAList")
	End Sub
	Private Overloads Sub setFocus(ByVal cntrl As WebControl)
		If cntrl.Enabled = False Or cntrl.Visible = False Then Exit Sub
		cntrl.Focus()
	End Sub
#End Region

#Region " Helper Methods "
	Private Sub MessageBoxResult()
		Dim Result1 As MsgBoxResult
		Result1 = MSGBoxCtrl.Result
		If Result1 > 0 Then
			Select Case Result1
				Case MsgBoxResult.Ok
					DataFieldBind()
			End Select
		End If
	End Sub
	Private Sub Display()
		lblAircraft1.Visible = True
		lblDateRangeFrom.Visible = True
		lblDateRangeTo.Visible = True
		lblStatus1.Visible = True
		lblATAChapter1.Visible = True
		lblDefectType1.Visible = True
	End Sub

	Private Sub SetValues()

		Try

			If Not IsDate(txtFromDate.Text) Then
				StartDate = ""
			Else
				StartDate = txtFromDate.Text.ToString
			End If
			If Not IsDate(txtToDate.Text) Then
				EndDate = ""
			Else
				EndDate = txtToDate.Text.ToString
			End If

			Aircraft = IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedItem.Text, IIf(chkIsRepetitive.Checked = True, "All", ""))
			MachineID = IIf(cmbAircraft.SelectedIndex > 0, cmbAircraft.SelectedValue.ToString, Guid.Empty.ToString)   'cmbAircraft.SelectedValue.ToString
			Status = IIf(cmbStatus.SelectedIndex > 0, cmbStatus.SelectedItem.Text, "")
			ATAChapter = IIf(cmbATAChapter.SelectedIndex > 0, cmbATAChapter.SelectedItem.Text, "")
			ATAChapterID = cmbATAChapter.SelectedValue.ToString
			mATAList = CType(Session("mATAList"), ATAList)
			ATANomenclature = mATAList(New Guid(cmbATAChapter.SelectedValue)).ATANomenclature
			ATACode = mATAList(New Guid(cmbATAChapter.SelectedValue)).ATACode
			lblDateRangeFrom.Text = "From Date : " & IIf(StartDate <> "", New SmartDate(StartDate).FormattedText, "")
			lblDateRangeTo.Text = "To Date : " & IIf(EndDate <> "", New SmartDate(EndDate).FormattedText, "")
			lblAircraft1.Text = "Aircraft : " & IIf(Aircraft <> "", Aircraft, "")
			lblStatus1.Text = "Status : " & IIf(Status <> "", Status, "")
			lblATAChapter1.Text = "ATA Chapter : " & IIf(ATAChapter <> "", ATAChapter, "")
			lblDefectType1.Text = "Defect Type : " & IIf(rbAllDefectType.Checked = True, "All", IIf(rbIsPireps.Checked = True, "Pireps", IIf(rbMaintenanceDefect.Checked = True, "Maintenance Defect", "")))
			mMELSnagReportRegisterSearchingCriteria = lblDateRangeFrom.Text.Trim + ", " + lblDateRangeTo.Text.Trim + ", " + lblAircraft1.Text.Trim + ", " + lblATAChapter1.Text.Trim + ", " + lblStatus1.Text.Trim + ", " + lblDefectType1.Text.Trim

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetReport(Optional IsExcel As Boolean = False)

		Dim dataAdapter As New ObjectAdapter
		Dim crystalReport As Engine.ReportClass
		Dim companyDetail As New CompanyDetail
		Dim SearchStr8 As String = "" 'Added By Utkarsh On 11-Aug-2011 for IND11082011 , "Operator :" 
		Dim IsMajorMinor As Integer
		Dim IsSnagMEL As Integer
		Dim MajorMinor, SnagMEL, MajorMinorStr, SnagMELStr As String
		Dim ReportName As String
		Dim IsPirepsDefectType, LogCriteria As Integer
		Dim PirepsDefectType, PirepsDefectTypeStr As String

		Try

			SetValues()

			If rbAll.Checked = True Then
				IsMajorMinor = 0  'ALL MAJOR AND MINOR
				MajorMinor = 0
				MajorMinorStr = "All"
			ElseIf rbMajor.Checked = True Then
				IsMajorMinor = 1  'MAJOR
				MajorMinor = 1    'To Show on report MAJOR/MINOR/ALL
				MajorMinorStr = "Major"
			Else
				IsMajorMinor = 2  'MINOR
				MajorMinor = 2
				MajorMinorStr = "Minor"
			End If

			Dim rptName As String = String.Empty '"MEL/Snag Register"

			If rbAllSnagMEL.Checked = True Then
				IsSnagMEL = 0  'ALL Snag AND MEL
				SnagMEL = 0
				SnagMELStr = "All"
			ElseIf rbSnag.Checked = True Then
				IsSnagMEL = 1  'Snag
				SnagMEL = 1
				SnagMELStr = IIf(AppSettings("MELSnagNomenclature") = "True", "Defect", "Snag") 'AppSettings Added By Vikrant On 07-Sep-2020 For ALL07092020
			Else
				IsSnagMEL = 2  'MEL
				SnagMEL = 2
				SnagMELStr = "MEL"
				rptName = IIf(AppSettings("MELSnagNomenclature") = "True", "ADD Register", "MEL Register") 'AppSettings Added By Vikrant On 07-Sep-2020 For ALL07092020
			End If

			'Added By Shweta On 30-April-2013 For ALL29042013-3
			If rbAllDefectType.Checked = True Then
				IsPirepsDefectType = 0  'ALL Pireps And Defect Type
				PirepsDefectType = 0
				PirepsDefectTypeStr = "All"
				If IsSnagMEL = 0 Or IsSnagMEL = 1 Then rptName = IIf(AppSettings("MELSnagNomenclature") = "True", "Defect Register", "Snag Register") 'AppSettings Added By Vikrant On 07-Sep-2020 For ALL07092020
			ElseIf rbIsPireps.Checked = True Then
				IsPirepsDefectType = 1  'Pireps
				PirepsDefectType = 1    'To Show on report Pireps/Defect Type/ALL
				PirepsDefectTypeStr = "Pireps"
				If IsSnagMEL = 0 Or IsSnagMEL = 1 Then rptName = "Pireps Register"
			Else
				IsPirepsDefectType = 2  'DEFECT type
				PirepsDefectType = 2
				PirepsDefectTypeStr = "Maintenance Defect"
				If IsSnagMEL = 0 Or IsSnagMEL = 1 Then rptName = "Maintenance Defect Register"
			End If

			If rbAllLog.Checked Then
				LogCriteria = 0
			ElseIf rbNILLog.Checked Then
				LogCriteria = 1
			ElseIf rbWithoutNilLog.Checked Then
				LogCriteria = 2

			End If

			mMELSnagCorrectiveActionRegisterReport =
				MELSnagCorrectiveActionRegisterReport.
					GetMELSnagCorrectiveActionRegisterReport(FromDate:=StartDate,
															 ToDate:=EndDate,
															 MachineID:=MachineID,
															 InvestigationStatus:=cmbStatus.SelectedValue,
															 IsMajor:=IsMajorMinor,
															 ATAChapterID:=ATAChapterID,
															 Defect:=txtDefect.Text,
															 IsLogNo:=IIf(chkLogNo.Visible = True, chkLogNo.Checked, False),
															 IsLogPageNo:=IIf(chkLogPageNo.Visible = True, chkLogPageNo.Checked, True),
															 IsFlightNo:=IIf(chkFlightNo.Visible = True, chkFlightNo.Checked, False),
															 IsMEL:=IsSnagMEL,
															 IsPireps:=IsPirepsDefectType,
															 IsRepetitive:=chkIsRepetitive.Checked,
															 IsInReliability:=cmbIsInReliability.SelectedValue,
															 LogCriteria:=LogCriteria,
															 IncidentTypeID:=CInt(cmbIncidentType.SelectedValue))

			If mMELSnagCorrectiveActionRegisterReport.Count = 0 Then 'Added By Vikrant On 28-Mar-2014 For ALL01042014
				crystalReport = New crptBlankDefectReport
			Else

				If AppSettings("ClientCode") IsNot Nothing AndAlso
				   (
						AppSettings("ClientCode") = "BA" Or
						AppSettings("ClientCode") = "PAS" Or
						AppSettings("ClientCode") = "Novo" Or
						AppSettings("ClientCode") = "YA" Or
						AppSettings("ClientCode") = "TA"
				   ) Then

					If cmbBA.SelectedIndex = 1 Then 'Ajay 04/04/2023
						crystalReport = New crptDefectReportBA2
					Else
						crystalReport = New crptDefectReportBA 'Added by Saylee on 18-Feb-2014 for BA18022014
					End If

				Else

					If cmbFormat.SelectedIndex = 0 Then

						If AppSettings("ClientCode") = "GEP" Then
							crystalReport = New crptMELSnagCorrectiveActionRegisterReportForGEP
						Else
							crystalReport = New crptMELSnagCorrectiveActionRegisterReport
						End If

					ElseIf cmbFormat.SelectedIndex = 1 Then
						crystalReport = New crptDmiControlList
						'Added by Utkarsh ON 07-02-2013 FOR Heligo07022013
					ElseIf cmbFormat.SelectedIndex = 2 Then

						If AppSettings("ClientCode") = "TSL" Then
							crystalReport = New crptDefectReportForTSL
						Else
							crystalReport = New crptDefectReport
						End If

					ElseIf cmbFormat.SelectedIndex = 3 Then   'Ajay 24-Nov-2022
						crystalReport = New crptDefectReportForSPZ
					End If

					RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 1180)

				End If

			End If
			'End

			If chkIsRepetitive.Checked Then
				ReportName = $"Repetitive {rptName}" '"Repetitive Defect Register" 'Added by Saylee on 24-Jul-2015 for ALL24072015 for Heligo
			Else

				If AppSettings("ClientCode") = "BA" Then
					ReportName = IIf(cmbBA.SelectedIndex = 0, rptName, "Defect Report")
				Else
					ReportName = IIf(cmbFormat.SelectedIndex = 0, rptName, IIf(cmbFormat.SelectedIndex = 1, "DMI Control List", "Defect Report"))
				End If

			End If

			'Added By Utkarsh On 11-Aug-2011 for IND11082011 , "Operator :" 
			If (AppSettings("ClientCode") IsNot Nothing) AndAlso AppSettings("ClientCode") = "Indamer" Then
				SearchStr8 = MachineOperatorName.GetMachineOperatorName(MachineID:=New Guid(MachineID)).OperatorName
			End If

			'Modified by Harsh on 29th Jan 2024 Client Enhancement
			Dim Report As New ReportData(companyDetail.CompanyName,
										 companyDetail.Address,
										 companyDetail.Tel1,
										 companyDetail.Tel2,
										 companyDetail.Fax,
										 companyDetail.Email,
										 companyDetail.WebSite,
										 ReportName:=ReportName,
										 ProductVersion:=AppSettings("Product Version"),
										 SINote:=AppSettings("SINote"),
										 SearchStr1:=New SmartDate(StartDate).FormattedText,
										 SearchStr2:=New SmartDate(EndDate).FormattedText,
										 SearchStr3:=IIf(IsExcel, MajorMinorStr, MajorMinor),
										 SearchStr4:=Status,
										 SearchStr5:=ATAChapter,
										 SearchStr6:=Aircraft,
										 SearchStr7:=IIf(IsExcel, SnagMELStr, SnagMEL),
										 SearchStr8:=SearchStr8,
										 SearchStr9:=IIf(IsExcel, PirepsDefectTypeStr, PirepsDefectType),
										 SearchStr10:=AppSettings("Logo"),
										 SearchStr11:=IIf(chkIsRepetitive.Checked, "True", "False"),
										 SearchStr12:=IIf(cmbIsInReliability.SelectedIndex = 0, "", cmbIsInReliability.SelectedItem.Text),
										 SearchStr13:=AppSettings("MELSnagNomenclature").ToString,
										 SearchStr14:=cmbIncidentType.SelectedItem.Text,
										 SearchStr15:=AppSettings("ClientCode"))

			If Not IsExcel Then

				Dim dataSet As New dsMELSnagCorrectiveActionRegisterReport
				Dim companyLogo As rptImage = rptImage.GetImage(dataSet)

				dataAdapter.Fill(dataSet, mMELSnagCorrectiveActionRegisterReport)
				dataAdapter.Fill(dataSet, companyLogo)
				dataAdapter.Fill(dataSet, Report)

				crystalReport.SetDataSource(dataSet)
				Session("CrystalReport") = crystalReport

				ScriptManager.RegisterStartupScript(Me, [GetType], "openTranDetail", "openTranDetail();", True)

				MarkLog(Action.Print,
						"MELSnagRegister",
						mMELSnagReportRegisterSearchingCriteria,
						ErrorType.NoError,
						Guid.Empty,
						EventLogID)

			ElseIf IsExcel Then  'Excel format

				'Added by Prashant 20-Nov-2015 for Export to excel
				If mMELSnagCorrectiveActionRegisterReport.Count = 0 Then
					MSGBoxCtrl.Show(MSGBox.Message_Title.NoRecordFound, MSGBox.Message_Text.NoRecordFound, "There is no record for this search criteria", MsgBoxStyle.OkOnly, "")
					Exit Sub
				End If
				Dim ds As New dsExcelMELSnagCorrectiveActionRegisterReport
				ds.Clear()
				dataAdapter.Fill(ds, "ReportData", Report)
				dataAdapter.Fill(ds, "MELSnagCorrectiveActionRegisterReport", mMELSnagCorrectiveActionRegisterReport)

				Dim columnToRemove2 As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "WebSite", "ProductVersion", "SINote", "CurrencyName", "CurrencySymbol", "SearchStr8", "SearchStr10", "SearchStr13", "SearchStr15", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25", "SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40", "SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47", "SearchStr48", "SearchStr49", "SearchStr50", "SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55", "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60", "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65", "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70", "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95", "SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}

				For i As Integer = 0 To columnToRemove2.Length - 1
					If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
						ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
					End If
				Next

				Dim columnToRemove As String() = {"ID", "LogID", "LogDate", "SerialNo", "PartName", "DefectReportText", "DefectReportNo", "SnagReportedBy", "Remark", "DispATACode", "LogDateFormatted", "LogText", "LogNo", "MachineID", "PartID", "Description", "ATAChapterID", "InvestigationStatus", "DateOfOccurenceFormatted", "LastMajorCheckHour", "IsInReliability", "PirepsMaintenanceDefectTag", "IsPireps", "ATACode", "DispSubATACode", "SubATANomenclature", "SubATACode", "ReferencedDocumentsHeading", "ReferencedDocuments", "IsFlightNo", "IsLogPageNo", "IsLogNo", "IsRepetitive", "IsMEL", "RectifiedDate", "MELCategoryFrequency", "MELCategoryID", "DueDate", "IsMajor", "PreventionTaken", "ActionAgainstStaff", "CauseOfDefect", "ComponentHour"}


				For i As Integer = 0 To columnToRemove.Length - 1
					If ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns.Contains(columnToRemove(i)) Then
						ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns.Remove(columnToRemove(i))
					End If
				Next

				If ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns.Contains("DefectReportTextNo") Then
					ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("DefectReportTextNo").ColumnName = "Defect No."
				End If
				If ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns.Contains("MELCategoryName") Then
					ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("MELCategoryName").ColumnName = "Category"
				End If
				'FrequencyInDays
				If ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns.Contains("FrequencyInDays") Then
					ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("FrequencyInDays").ColumnName = "Interval In Days"
				End If
				If ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns.Contains("FrequencyInHours") Then
					ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("FrequencyInHours").ColumnName = "Interval In Hours"
				End If
				If ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns.Contains("SubATACodeDisplay") Then
					ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("SubATACodeDisplay").ColumnName = "Sub ATA Chapter"
				End If
				If ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns.Contains("RectifiedDateFormatted") Then
					ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("RectifiedDateFormatted").ColumnName = "Rectified Date"
				End If
				'
				If ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns.Contains("DueDateFormatted") Then
					ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("DueDateFormatted").ColumnName = "Due Date"
				End If
				If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
					ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "From Date"
				End If
				If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
					ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "To Date"
				End If
				If ds.Tables("ReportData").Columns.Contains("SearchStr3") Then
					ds.Tables("ReportData").Columns("SearchStr3").ColumnName = IIf(AppSettings("MELSnagNomenclature") = "True", "ADD/Defect Type", "MEL/Snag Type")
				End If
				If ds.Tables("ReportData").Columns.Contains("SearchStr4") Then
					ds.Tables("ReportData").Columns("SearchStr4").ColumnName = "Status"
				End If
				If ds.Tables("ReportData").Columns.Contains("SearchStr5") Then
					ds.Tables("ReportData").Columns("SearchStr5").ColumnName = "ATA Chapter"
				End If
				If ds.Tables("ReportData").Columns.Contains("SearchStr6") Then
					ds.Tables("ReportData").Columns("SearchStr6").ColumnName = "Aircraft"
				End If
				If ds.Tables("ReportData").Columns.Contains("SearchStr7") Then
					ds.Tables("ReportData").Columns("SearchStr7").ColumnName = IIf(AppSettings("MELSnagNomenclature") = "True", "ADD/Defect Part", "MEL/Snag Part")
				End If
				If ds.Tables("ReportData").Columns.Contains("SearchStr9") Then
					ds.Tables("ReportData").Columns("SearchStr9").ColumnName = "Defect Type"
				End If
				If ds.Tables("ReportData").Columns.Contains("SearchStr11") Then
					ds.Tables("ReportData").Columns("SearchStr11").ColumnName = "Is Repetitive"
				End If

				If ds.Tables("ReportData").Columns.Contains("SearchStr12") Then
					ds.Tables("ReportData").Columns("SearchStr12").ColumnName = "Is In Reliability"
				End If

				If ds.Tables("ReportData").Columns.Contains("SearchStr14") Then
					ds.Tables("ReportData").Columns("SearchStr14").ColumnName = "Incident Type"
				End If

				If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
					If ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns.Contains("Sub ATA Chapter") Then
						ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("Sub ATA Chapter").ColumnName = "ATA Section"
					End If
					If ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns.Contains("ReportedBy") Then
						ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("ReportedBy").ColumnName = "Observed By"
					End If
					If ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns.Contains("Action") Then
						ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("Action").ColumnName = "Corrective Action"
					End If
					If ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns.Contains("RectifiedMechanic") Then
						ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("RectifiedMechanic").ColumnName = "Rectified By"
					End If

					If ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns.Contains("LogPageNo") Then
						ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("LogPageNo").ColumnName = "Tech Log Entry"
					End If

					If ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns.Contains("DateOfOccurence") Then
						ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("DateOfOccurence").ColumnName = "Date"
					End If

					If ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns.Contains("Defect") Then
						ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("Defect").ColumnName = "Cause Text"
					End If

					ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("AirlineName").SetOrdinal(1)
					ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("Date").SetOrdinal(2)
					ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("MSN").SetOrdinal(3)
					ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("RegNo").SetOrdinal(4)
					ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("Tech Log Entry").SetOrdinal(5)
					ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("Defect No.").SetOrdinal(6)
					ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("ATAChapter").SetOrdinal(7)
					ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("ATA Section").SetOrdinal(8)
					ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("Cause Text").SetOrdinal(9)
					ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("Observed By").SetOrdinal(10)
					ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("Corrective Action").SetOrdinal(11)
					ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("Rectified By").SetOrdinal(12)
					ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("LogTextNo").SetOrdinal(13)


					If cmbBA.SelectedIndex = 1 Then 'Ajay 04/04/2023

						Dim columnToRemove4 As String() = {"AirlineName", "MSN", "RegNo", "Tech Log Entry", "ATAChapter", "ATA Section", "Observed By", "LogTextNo", "ATAChapter", "Observed By", "Rectified By", "Status", "FlightNo", "PartNo", "PartSerialNo", "Sector", "Due Date", "Category", "Interval In Days", "Interval In Hours", "InvokeTime", "RevokeTime", "TimeFormat", "DateTimeOfOccurence", "DateTimeOfRectified"}

						For i As Integer = 0 To columnToRemove4.Length - 1
							If ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns.Contains(columnToRemove4(i)) Then
								ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns.Remove(columnToRemove4(i))
							End If
						Next

						ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("Date").SetOrdinal(0)
						ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("Defect No.").SetOrdinal(1)
						ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("Cause Text").SetOrdinal(2)
						ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("Corrective Action").SetOrdinal(3)
						ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("Rectified Date").SetOrdinal(4)
						ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("RemarkBlank").SetOrdinal(5)

						If ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns.Contains("Defect No.") Then
							ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("Defect No.").ColumnName = "Reference (Work Order / NRCs)"
						End If
						If ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns.Contains("Cause Text") Then
							ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("Cause Text").ColumnName = "Description"
						End If
						If ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns.Contains("Corrective Action") Then
							ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("Corrective Action").ColumnName = "Defect Rectification"
						End If
						If ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns.Contains("Rectified Date") Then
							ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("Rectified Date").ColumnName = "Rectification Date"
						End If
						If ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns.Contains("RemarkBlank") Then
							ds.Tables("MELSnagCorrectiveActionRegisterReport").Columns("RemarkBlank").ColumnName = "Remark"
						End If
					End If
					'-----------------------------
				End If

				Dim dsNew As New DataSet
				dsNew.Clear()
				ds.Tables("ReportData").TableName = "Searching Criteria"
				ds.Tables("MELSnagCorrectiveActionRegisterReport").TableName = ReportName
				Session("ExcelFileName") = ReportName
				dsNew = ds
				Session("dsNew") = dsNew
				ScriptManager.RegisterStartupScript(Me, [GetType], "Display Report In Excel", "displayReportInExcel();", True)
				'Added by Prashant on 19-Jan-2021
				MarkLog(Action.Print, "MELSnagRegister", "Export To Excel " + mMELSnagReportRegisterSearchingCriteria, ErrorType.NoError, Guid.Empty, EventLogID)

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ControlVisibility()

		'Modified by Harsh on 29th Jan 2024 Client Enhancement
		If (Not AppSettings("ClientCode") Is Nothing) AndAlso (AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo" Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA") Then
			'lblStep5.Visible = False
			cmbFormat.Visible = False
			lblRefDoc.Visible = False
			chkLogNo.Visible = False
			chkLogPageNo.Visible = False
			chkFlightNo.Visible = False
			lblStep6.InnerText = "Step XIII. Display Report"
		Else
			'lblStep5.Visible = True
			cmbFormat.Visible = True
			lblRefDoc.Visible = True
			chkLogNo.Visible = True
			chkLogPageNo.Visible = True
			chkFlightNo.Visible = True
			lblStep6.InnerText = "Step XIV. Display Report"
		End If

		If rbNILLog.Checked = True Then
			chkIsRepetitive.Enabled = False
			rbAllDefectType.Enabled = False
			rbIsPireps.Enabled = False
			rbMaintenanceDefect.Enabled = False
			rbAll.Enabled = False
			rbMajor.Enabled = False
			rbMinor.Enabled = False
			rbAllSnagMEL.Enabled = False
			rbSnag.Enabled = False
			rbMEL.Enabled = False
			chkLogNo.Enabled = False
			chkLogPageNo.Enabled = False
			chkFlightNo.Enabled = False
		Else
			chkIsRepetitive.Enabled = True
			rbAllDefectType.Enabled = True
			rbIsPireps.Enabled = True
			rbMaintenanceDefect.Enabled = True
			rbAll.Enabled = True
			rbMajor.Enabled = True
			rbMinor.Enabled = True
			rbAllSnagMEL.Enabled = True
			rbSnag.Enabled = True
			rbMEL.Enabled = True
			chkLogNo.Enabled = True
			chkLogPageNo.Enabled = True
			chkFlightNo.Enabled = True
		End If
		upnlRepeatitive.Update()
		upnlMELCriteria.Update()

	End Sub

#End Region

#Region " Data Binding "
	Private Sub DataFieldBind()

		Try

			mMachineNameValueList = MachineNameValueList.GetMachineList(CurrentDate:="", , , , , , , IsTagRequired:=True, TagText:="(SELECT)", , SkipIsForInventoryAircarft:=True) 'Added By Utkarsh On 19-Apr-2011
			cmbAircraft.DataSource = mMachineNameValueList
			Session("mMachineNameValueList") = mMachineNameValueList
			mATAList = ATAList.GetATAList(ATANomenclature:="", AddTopItem:="(ALL)") 'Added By Saylee on 12-Aug-2010
			Session("mATAList") = mATAList
			cmbATAChapter.DataSource = mATAList
			mIncidentTypeListMELSnagReportRegister = IncidentTypeList.GetIncidentTypeList("(All)")
			cmbIncidentType.DataSource = mIncidentTypeListMELSnagReportRegister

			DataBind()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Events "

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		Try

			EventLogID = CType(Session("EventLogID"), Guid) 'Added by Prashant 

			If Not IsPostBack Then

				txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
				txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat").ToString)
				rbAll.Checked = True
				rbAllSnagMEL.Checked = True
				rbAllDefectType.Checked = True

				DataFieldBind()

				If cmbAircraft.Enabled = True Then
					setFocus(cmbAircraft)
				End If

			End If

			ControlVisibility()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub btnCurrentSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSearchCriteria.Click
		Display()
		SetValues()
		upnlselection1.Update()
		upnlselection2.Update()
	End Sub

	Private Sub btnDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
		If IsValid Then
			SetReport(False)
		Else
			upnlValidationsummary.Update()
		End If
	End Sub

	Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
		If IsValid Then
			SetReport(True)
		Else
			upnlValidationsummary.Update()
		End If
	End Sub

	Private Sub IsRepetitive(sender As Object, e As EventArgs) Handles chkIsRepetitive.CheckedChanged 'Added by Saylee on 24-Jul-2015 for ALL24072015 -to show proper Report Name for Repetitive defects and allow report for ALL aircrafts

		Try

			If chkIsRepetitive.Checked = True Then
				mMachineNameValueList = MachineNameValueList.GetMachineList(CurrentDate:="", , , , , , , IsTagRequired:=True, TagText:="(ALL)", , SkipIsForInventoryAircarft:=True)
				cmbFormat.SelectedIndex = 2
				cmbFormat.Enabled = False
				lblAircraftStar1.Visible = False
			Else
				mMachineNameValueList = MachineNameValueList.GetMachineList(CurrentDate:="", , , , , , , IsTagRequired:=True, TagText:="(SELECT)", , SkipIsForInventoryAircarft:=True)
				cmbFormat.Enabled = True
				cmbFormat.SelectedIndex = 0
				lblAircraftStar1.Visible = True
			End If

			cmbAircraft.DataSource = mMachineNameValueList
			Session("mMachineNameValueList") = mMachineNameValueList
			cmbAircraft.DataBind()
			upnlAircraftCombo.Update()
			upnlFormat.Update()
			upnlAircraftStar.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub cmbAircraft_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAircraft.SelectedIndexChanged
		If chkIsRepetitive.Checked = True Then
			If cmbAircraft.SelectedIndex > 0 Then
				cmbFormat.SelectedIndex = 0
				cmbFormat.Enabled = True
			ElseIf cmbAircraft.SelectedIndex = 0 Then
				cmbFormat.SelectedIndex = 2
				cmbFormat.Enabled = False
			End If
		End If
		upnlFormat.Update()
	End Sub

	Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
		Session("MiddleFrame") = ""
		RemoveSession()
		Response.Redirect("Dashboard.aspx")
	End Sub

	Private Sub MSGBoxCtrl_UserControlButtonClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles MSGBoxCtrl.UserControlButtonClicked
		MSGBoxCtrl.HideControl()
		MessageBoxResult()
	End Sub

#End Region

End Class