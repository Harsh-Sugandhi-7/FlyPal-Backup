'************************************
'AJAX Conversion By Vikrant On 31-Jan-2014
' Modified by Harsh Sugandhi on 4th August 2025 for FLYPAL-2611
'************************************


Imports System.Linq


Public Class wfnrptWOSummary_Ajax
	Inherits Page

#Region " Variable Declaration "

	Public DistinctWOText As nDistinctWOText
	Public CustomerList As VendorList
	Public CompanyDetail As New CompanyDetail
	Public WOSummary As nrptWOSummary
	Public WOStatusList As nWOStatusList
	Public WOJobTypeList As nWOJobTypeList

	Dim WOText As String = ""
	Dim WONo As Integer
	Dim FromDate As String = ""
	Dim ToDate As String = ""
	Dim RegNo As String = ""
	Dim Model As String = ""
	Dim SerialNo As String = ""
	Dim Supplier As String = ""
	Dim CompPartNo As String = ""
	Dim CompSerialNo As String = ""
	Dim SearchStr1 As String
	Dim SearchStr3 As String
	Dim SearchStr6, SearchStr9 As String
	Dim EventLogDetail As String = String.Empty
	' Added By Abhishek on 26-SEP-2017
	Dim da As New ObjectAdapter
	Dim ds As New dsnWOSummary
	Dim ReportName As String
	Dim WOJobTypeIDList As Object = Nothing
	Dim TransTypeID As Integer = 0

#End Region

#Region " Business Methods "

	Private Sub GetSession()
		DistinctWOText = CType(Session("DistinctWOText"), nDistinctWOText)
		CustomerList = CType(Session("CustomerList"), VendorList)
		WOStatusList = Session("WOStatusList")
	End Sub

	Private Sub SetSession()
		Session("DistinctWOText") = DistinctWOText
		Session("CustomerList") = CustomerList
		Session("WOStatusList") = WOStatusList
	End Sub

	Private Sub RemoveSession()
		Session.Remove("DistinctWOText")
		Session.Remove("CustomerList")
		Session.Remove("WOStatusList")
	End Sub

	Private Overloads Sub SetFocus(control As WebControl)
		If control.Enabled = False Or control.Visible = False Then Exit Sub
		control.Focus()
	End Sub

	Private Sub ControlVisibilityDateRange(Index As Int16)

		Try

			lblFromDate.Visible = IIf(Index <> 0, True, False)
			lblToDate.Visible = IIf(Index <> 0, True, False)

			If Index = 6 Then

				txtFromDate.Visible = True
				txtToDate.Visible = True
				txtFromDate.Enabled = True
				txtToDate.Enabled = True

			ElseIf Index = 1 Or Index = 2 Or Index = 3 Or Index = 4 Or Index = 5 Then

				txtFromDate.Visible = True
				txtToDate.Visible = True
				txtFromDate.Enabled = False
				txtToDate.Enabled = False

			Else
				txtFromDate.Visible = False
				txtToDate.Visible = False
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ControlVisibilitySearchCriteria()

		Try

			lblDateRangeFrom.Visible = True
			lblWONo1.Visible = True
			lblReportType1.Visible = True
			lblVendor.Visible = True
			lblRegNo1.Visible = True
			lblModel1.Visible = True
			lblSerialNo1.Visible = True
			lblStatus1.Visible = True
			lblWoJobType1.Visible = True
			lblFMC.Visible = IIf(AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA", True, False)
			upnlCurrentCriteria.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub ControlVisibilityPageLabels()

		Try

			If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
				lbltitle.Text = "Engineering Order Summary"
				lblStep4.Text = "Selection of E. O. No."
				lblWONo.Text = "E. O. No."
			Else
				lbltitle.Text = "Work Order Summary"
				lblStep4.Text = "Selection of W. O. No."
				lblWONo.Text = "W. O. No."
			End If

			upnlTitle.Update()
			upnlWO.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetDatePeriod(Index As Int32)

		Try

			Select Case Index
				Case 0 'All   
					txtFromDate.Text = CDate("01-01-1900").ToString(AppSettings("DateFormat"))
					txtToDate.Text = CDate("01-01-2200").ToString(AppSettings("DateFormat"))
				Case 1 'Last 1 Week
					txtFromDate.Text = CDate(Today.AddDays(-6)).ToString(AppSettings("DateFormat"))
					txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
				Case 2 'Last 1 Month
					txtFromDate.Text = CDate(Today.AddDays(1).AddMonths(-1)).ToString(AppSettings("DateFormat"))
					txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
				Case 3 'Last 1 Quarter

					Select Case Today.Month
						Case 1, 2, 3
							txtFromDate.Text = CDate("01-Oct-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat"))
							txtToDate.Text = CDate("31-Dec-" + CStr(Today.Year - 1)).ToString(AppSettings("DateFormat"))
						Case 4, 5, 6
							txtFromDate.Text = CDate("01-Jan-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
							txtToDate.Text = CDate("31-Mar-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
						Case 7, 8, 9
							txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
							txtToDate.Text = CDate("30-Jun-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
						Case 10, 11, 12
							txtFromDate.Text = CDate("01-Jul-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
							txtToDate.Text = CDate("30-Sep-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))
					End Select

				Case 4 'Last 1 Year
					txtFromDate.Text = CDate(Today.AddDays(1).AddYears(-1)).ToString(AppSettings("DateFormat"))
					txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
				Case 5 'Current Financial Year
					If Today.Month <= 3 Then  'Jan|Feb|Mar
						txtFromDate.Text = CDate("01-Apr-" + CStr(Today.AddYears(-1).Year)).ToString(AppSettings("DateFormat"))
					Else
						txtFromDate.Text = CDate("01-Apr-" + CStr(Today.Year)).ToString(AppSettings("DateFormat"))    '31-Mar-2006
					End If
					txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
				Case 6 'Between Dates
					txtFromDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
					txtToDate.Text = Today.Date.ToString(AppSettings("DateFormat"))
			End Select

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetValues()

		Try

			If cmbDateRange.SelectedIndex = 0 Then
				FromDate = New SmartDate("01-01-1900").FormattedText
				ToDate = New SmartDate("01-01-2200").FormattedText
				lblDateRangeFrom.Text = "Date Range     : All"
			Else
				FromDate = txtFromDate.Text
				ToDate = txtToDate.Text
				lblDateRangeFrom.Text = "Date Range     : " & FromDate & " To " & ToDate & " ( " & cmbDateRange.SelectedItem.Text & ")"
			End If

			If cmbCustomer.SelectedIndex = 0 Then
				Supplier = ""
			Else
				Supplier = cmbCustomer.SelectedItem.Text
			End If

			lblVendor.Text = "Customer  : " & Supplier
			WOText = IIf(cmbWO.SelectedIndex > 0, Trim(cmbWO.SelectedItem.Text), "")
			WONo = CInt(Val(txtWONo.Text))

			If WOText <> "" Then

				If WONo <> 0 Then

					If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
						lblWONo1.Text = "E. O. No. : " & WOText + "-" + WONo.ToString
					Else
						lblWONo1.Text = "W. O. No. : " & WOText + "-" + WONo.ToString
					End If

				Else

					If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
						lblWONo1.Text = "E. O. No. : " & WOText
					Else
						lblWONo1.Text = "W. O. No. : " & WOText
					End If

				End If

			Else

				If (AppSettings("ClientCode") IsNot Nothing) AndAlso (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
					lblWONo1.Text = "E. O. No. : " & "All"
				Else
					lblWONo1.Text = "W. O. No. : " & "All"
				End If

			End If

			RegNo = txtRegNo.Text.Trim
			lblRegNo1.Text = "Reg. No.  :" & RegNo
			Model = txtModel.Text.Trim
			lblModel1.Text = "Model  : " & Model
			SerialNo = txtSerialNo.Text.Trim
			lblSerialNo1.Text = "Serial No.  : " & SerialNo
			lblCompPartNo1.Text = "Comp Part No.  : " & CompPartNo
			lblCompSerialNo1.Text = "Comp Serial No.  : " & CompSerialNo
			lblStatus1.Text = "Status :" & IIf(AppSettings("ShowNewWOFlow") = "True", IIf(cmbStatus.SelectedIndex > 0, Trim(cmbStatus.SelectedItem.Text), "All"), IIf(cmbWOStatusList.SelectedIndex > 0, Trim(cmbWOStatusList.SelectedItem.Text), "All"))
			lblWoJobType1.Text = "WO. Job Type : " & IIf(cmbWOJobType.SelectedIndex > 0, Trim(cmbWOJobType.SelectedItem.Text), "All")

			If cmbDateRange.SelectedIndex = 0 Then
				SearchStr1 = ""
			ElseIf cmbDateRange.SelectedIndex = 6 Then
				SearchStr1 = "By" + " " + cmbDateRange.SelectedItem.Text + " " + ":" + " " + lblFromDate.Text + " " + txtFromDate.Text + " " + lblToDate.Text + " " + txtToDate.Text
			Else
				SearchStr1 = "By" + " " + cmbDateRange.SelectedItem.Text + " " + ":" + " " + lblFromDate.Text + " " + txtFromDate.Text + " " + lblToDate.Text + " " + txtToDate.Text
			End If

			If WONo = 0 Then
				SearchStr3 = WOText
			Else
				SearchStr3 = WOText + " - " + WONo.ToString
			End If

			If cmbWOStatusList.SelectedIndex = 0 And cmbStatus.SelectedIndex = 0 Then
				SearchStr6 = "All"
			Else
				SearchStr6 = IIf(AppSettings("ShowNewWOFlow") = "True", Trim(cmbStatus.SelectedItem.Text), Trim(cmbWOStatusList.SelectedItem.Text))  'Trim(cmbWOStatusList.SelectedItem.Text)
			End If

			If cmbIsFMC.SelectedIndex = 0 Then
				SearchStr9 = "All"
			Else
				SearchStr9 = Trim(cmbIsFMC.SelectedItem.Text)
			End If

			lblFMC.Text = IIf(True, "FMC Work Order : " & cmbIsFMC.SelectedItem.ToString, "")
			EventLogDetail = lblDateRangeFrom.Text + "," + lblVendor.Text + "," + lblWONo1.Text + "," + lblRegNo1.Text + "," + lblModel1.Text + "," + lblSerialNo1.Text + "," + lblStatus1.Text + ", " + lblWoJobType1.Text + ", " + lblFMC.Text

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub SetReport(Optional IsExcel As Boolean = False)

		Try

			Session("IsExcel") = IsExcel
			SetValues()
			Dim myReport As Engine.ReportClass
			Dim da As New ObjectAdapter
			Dim ds As New dsnWOSummary
			Dim CompanyDetail As New CompanyDetail
			myReport = New crnWOSummary

			Dim ReportName As String

			If (AppSettings("ClientCode") IsNot Nothing) AndAlso
			   (AppSettings("ClientCode") = "TAAL" Or AppSettings("ClientCode") = "GlobalJet") Then
				ReportName = "E. O. Summary Report"
			ElseIf AppSettings("ClientCode") = "Novo" Then  'NOVO WO
				ReportName = "CAMO W.O ISSUE REGISTER"
			ElseIf AppSettings("ClientCode") = "KLP" Then  'KLP WO
				ReportName = "Work Order Register"
			Else

				If AppSettings("ShowMaintenanceForNewClients") = "True" Then
					ReportName = "Work Order Register"
				Else
					ReportName = "W. O. Summary Report"
				End If

			End If

			Dim IsFMC As Integer = IIf(AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA", cmbIsFMC.SelectedIndex, 0)
			Dim IsForBilling As Boolean = IIf(AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA", chkForBillingPurpose.Checked, 0)

			If AppSettings("ShowCAMOOnlyForNewClients") = "True" And AppSettings("ShowAMOOnlyForNewClients") = "False" Then
				TransTypeID = 89 'CAMO
			ElseIf AppSettings("ShowCAMOOnlyForNewClients") = "False" And AppSettings("ShowAMOOnlyForNewClients") = "True" Then
				TransTypeID = 88 'Third party
			ElseIf AppSettings("ShowCAMOOnlyForNewClients") = "True" And AppSettings("ShowAMOOnlyForNewClients") = "True" Then
				TransTypeID = Val(cmbTransType.SelectedValue.ToString)
			Else
				TransTypeID = Val(cmbTransType.SelectedValue.ToString)
			End If

			WOSummary = nrptWOSummary.GetWOSummary(WOText,
												   WONo,
												   FromDate,
												   ToDate,
												   RegNo,
												   Model, ,
												   IIf(AppSettings("ShowNewWOFlow") = "True", cmbStatus.SelectedValue, cmbWOStatusList.SelectedValue),
												   cmbCustomer.SelectedValue.ToString, SerialNo,
												   "",
												   WOJobTypeID:=cmbWOJobType.SelectedValue,
												   IsFMC:=IsFMC,
												   IsForBilling:=IsForBilling,
												   TasksRequired:=IIf(AppSettings("ClientCode") = "Novo", True, False),
												   TransTypeID:=TransTypeID,
												   SortBy:=cmbSortBy.SelectedIndex,
												   AscDesc:=cmbOrderBy.SelectedIndex,
												   OtherJob:=IIf(chkOtherJob.Checked = True, 1, -1))

			Dim Report As New ReportData(CompanyDetail.CompanyName,
										 CompanyDetail.Address,
										 CompanyDetail.Tel1,
										 CompanyDetail.Tel2,
										 CompanyDetail.Fax,
										 CompanyDetail.Email,
										 CompanyDetail.WebSite,
										 ReportName, SearchStr1,
										 Supplier,
										 SearchStr3,
										 RegNo,
										 Model,
										 AppSettings("Product Version"),
										 AppSettings("SINote"),
										 SearchStr6,
										 AppSettings("Logo"),
										 SearchStr8:=cmbWOJobType.SelectedItem.Text,
										 SearchStr9:=SearchStr9,
										 SearchStr10:=AppSettings("ClientCode"))

			If WOSummary.Count = 0 Then

				If AppSettings("ClientCode") = "Novo" Then

					If cmbReportType.SelectedValue = 0 Then         'Work Order Register
						myReport = New crnWOSummaryNOVOBlank
					Else
						myReport = New crnWOSummaryNOVOBlankFormat2 'Work Order Register with Jobs and Tasks
					End If

				ElseIf AppSettings("ClientCode") = "STR" Then 'Added By Vikrant On 30-Jan-2020 For ALL30012020
					myReport = New crnWOSummaryBlankSTR
				Else

					If chkForBillingPurpose.Checked Then
						myReport = New crnWOSummaryForBillingBlank ''Print Blank Report for NILL Record's (Bird Aviation changes)-
					Else
						myReport = New crnWOSummaryBlank ''Print Blank Report for NILL Record's (Bird Aviation changes)-By Saylee on 15-July-2016
					End If

				End If

			ElseIf WOSummary.Count > 0 Then

				RecentMenuEvent.RecentMenuItemEvent(User.Identity.Name, 723)

				If AppSettings("ClientCode") = "Novo" Then

					If cmbReportType.SelectedValue = 0 Then         'Work Order Register
						myReport = New crnWOSummaryNOVO
					Else
						myReport = New crnWOSummaryNOVOFormat2      'Work Order Register with Jobs and Tasks
					End If

				ElseIf AppSettings("ClientCode") = "STR" Then 'Added By Vikrant On 30-Jan-2020 For ALL30012020
					myReport = New crnWOSummarySTR
				Else

					If chkForBillingPurpose.Checked Then
						myReport = New crnWOSummaryForBilling ''Print For Billing Purpose (Bird Aviation changes)
					Else
						myReport = New crnWOSummary
					End If

				End If

			End If

			Dim companyLogo As rptImage = rptImage.GetImage(ds) 'Added by Shweta on 20-Feb-2012
			da.Fill(ds, companyLogo) 'Added by Shweta on 20-Feb-2012
			da.Fill(ds, Report)

			If IsExcel = True Then

				If WOSummary.Count = 0 Then

					MSGBoxCtrl.Show(MSGBox.Message_title.NoRecordFound,
									MSGBox.Message_text.NoRecordFound,
									"",
									MsgBoxStyle.OkOnly,
									"")
					Exit Sub

				End If

				da.Fill(ds, "ExcelnrptWOSummary", WOSummary)
				Dim columnToRemove1 As String()

				If IsForBilling = True Then

					columnToRemove1 = {"ID", "AllJobsDescriptionofWO", "AllJobsDueAsofWO", "WOText", "WONo", "WODate", "StatusID", "MachineID",
									   "CustomerID", "WOStartDate", "WOCloseDate", "WOPlanedDate", "WOStatusID", "HourType", "LogID", "IsThirdParty",
									   "InHouseThirdParty", "WOTotalActualTime", "LogText", "LogNo", "WOJobTypeID", "WOJobStatusID", "MonitorTypeID",
									   "OnTypeID", "MonitorInfoType", "WOSubmittedDateTime", "WOSubmittedDateTimeFormatted", "WOCompletedDateTime",
									   "WOCompletedDateTimeFormatted", "WOJobStartDate", "WOJobStartDateFormatted", "WOJobAMEActualTime",
									   "WOJobTECHActualTime", "EmployeeName", "CustomerWONo", "IsForBilling", "BillingRemark", "WOJobDescription",
									   "BillingRequired", "IsCAMOUpdated", "IsQCStatusApproved", "WOJobID", "JObWOJobID", "IsWOJobID"}

					For i As Integer = 0 To columnToRemove1.Length - 1

						If ds.Tables("ExcelnrptWOSummary").Columns.Contains(columnToRemove1(i)) Then
							ds.Tables("ExcelnrptWOSummary").Columns.Remove(columnToRemove1(i))
						End If

					Next

					ds.Tables("ExcelnrptWOSummary").Columns("RegNo").SetOrdinal(0)
					ds.Tables("ExcelnrptWOSummary").Columns("WOJobDescriptionExcel").SetOrdinal(1)
					ds.Tables("ExcelnrptWOSummary").Columns("CustomerWONo").SetOrdinal(2)
					ds.Tables("ExcelnrptWOSummary").Columns("DueAsOf").SetOrdinal(3)
					ds.Tables("ExcelnrptWOSummary").Columns("WODateFormatted").SetOrdinal(4)
					ds.Tables("ExcelnrptWOSummary").Columns("WONumber").SetOrdinal(5)
					ds.Tables("ExcelnrptWOSummary").Columns("WOJobStartDateFormatted").SetOrdinal(6)
					ds.Tables("ExcelnrptWOSummary").Columns("WOStatusName").SetOrdinal(7)
					ds.Tables("ExcelnrptWOSummary").Columns("WOCloseDateFormatted").SetOrdinal(8)
					ds.Tables("ExcelnrptWOSummary").Columns("WOJobAMEActualTime").SetOrdinal(9)
					ds.Tables("ExcelnrptWOSummary").Columns("WOJobTECHActualTime").SetOrdinal(10)
					ds.Tables("ExcelnrptWOSummary").Columns("WOJobActualTime").SetOrdinal(11)
					ds.Tables("ExcelnrptWOSummary").Columns("BillingRemark").SetOrdinal(12)

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("RegNo") Then
						ds.Tables("ExcelnrptWOSummary").Columns("RegNo").ColumnName = "A/C REG. NO."
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WOJobDescriptionExcel") Then
						ds.Tables("ExcelnrptWOSummary").Columns("WOJobDescriptionExcel").ColumnName = "WO DESCRIPTION"
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("CustomerWONo") Then
						ds.Tables("ExcelnrptWOSummary").Columns("CustomerWONo").ColumnName = "OPERATOR WO NO"
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("DueAsOf") Then
						ds.Tables("ExcelnrptWOSummary").Columns("DueAsOf").ColumnName = "Due As Of"
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WODateFormatted") Then
						ds.Tables("ExcelnrptWOSummary").Columns("WODateFormatted").ColumnName = "Date"
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WONumber") Then
						ds.Tables("ExcelnrptWOSummary").Columns("WONumber").ColumnName = "W.O.No."
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WOJobStartDateFormatted") Then
						ds.Tables("ExcelnrptWOSummary").Columns("WOJobStartDateFormatted").ColumnName = "Start Date"
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WOStatusName") Then
						ds.Tables("ExcelnrptWOSummary").Columns("WOStatusName").ColumnName = "W.O.Status"
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WOCloseDateFormatted") Then
						ds.Tables("ExcelnrptWOSummary").Columns("WOCloseDateFormatted").ColumnName = "WO Closing Date"
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WOJobAMEActualTime") Then
						ds.Tables("ExcelnrptWOSummary").Columns("WOJobAMEActualTime").ColumnName = "AME MH"
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WOJobTECHActualTime") Then
						ds.Tables("ExcelnrptWOSummary").Columns("WOJobTECHActualTime").ColumnName = "TECH MH"
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WOJobActualTime") Then
						ds.Tables("ExcelnrptWOSummary").Columns("WOJobActualTime").ColumnName = "TOTAL MH"
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("BillingRemark") Then
						ds.Tables("ExcelnrptWOSummary").Columns("BillingRemark").ColumnName = "BILLABLE REMARKS"
					End If

				Else

					columnToRemove1 = {"ID", "AllJobsDescriptionofWO", "AllJobsDueAsofWO", "WOText", "WONo", "WODate", "StatusID",
									   "MachineID", "CustomerID", "WOStartDate", "WOCloseDate", "WOPlanedDate", "WOStatusID", "HourType", "LogID",
									   "IsThirdParty", "InHouseThirdParty", "WOTotalActualTime", "LogText", "LogNo", "WOJobTypeID", "WOJobStatusID",
									   "MonitorTypeID", "OnTypeID", "MonitorInfoType", "WOSubmittedDateTime", "WOSubmittedDateTimeFormatted",
									   "WOCompletedDateTime", "WOCompletedDateTimeFormatted", "WOJobStartDate", "WOJobStartDateFormatted",
									   "WOJobAMEActualTime", "WOJobTECHActualTime", "EmployeeName", "CustomerWONo", "IsForBilling", "BillingRemark",
									   "WOJobDescription", "BillingRequired", "IsCAMOUpdated", "IsQCStatusApproved", "WOJobID", "JObWOJobID",
									   "IsWOJobID", "TaskNo", "TaskSourceRef", "Publication", "Skill"}

					For i As Integer = 0 To columnToRemove1.Length - 1

						If ds.Tables("ExcelnrptWOSummary").Columns.Contains(columnToRemove1(i)) Then
							ds.Tables("ExcelnrptWOSummary").Columns.Remove(columnToRemove1(i))
						End If

					Next

					ds.Tables("ExcelnrptWOSummary").Columns("WOJobDescriptionExcel").SetOrdinal(1)
					ds.Tables("ExcelnrptWOSummary").Columns("ServiceProvider").SetOrdinal(5)


					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WODateFormatted") Then
						ds.Tables("ExcelnrptWOSummary").Columns("WODateFormatted").ColumnName = "Date"
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WONumber") Then
						ds.Tables("ExcelnrptWOSummary").Columns("WONumber").ColumnName = "W.O.No."
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WOPlanedDateFormatted") Then
						ds.Tables("ExcelnrptWOSummary").Columns("WOPlanedDateFormatted").ColumnName = "Plan Date"
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WOBy") Then
						ds.Tables("ExcelnrptWOSummary").Columns("WOBy").ColumnName = "Created By"
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("RegNo") Then
						ds.Tables("ExcelnrptWOSummary").Columns("RegNo").ColumnName = "Reg.No."
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("ModelName") Then
						ds.Tables("ExcelnrptWOSummary").Columns("ModelName").ColumnName = "Model No."
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("SerialNo") Then
						ds.Tables("ExcelnrptWOSummary").Columns("SerialNo").ColumnName = "Serial No."
					End If
					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("CustomerName") Then
						ds.Tables("ExcelnrptWOSummary").Columns("CustomerName").ColumnName = "Customer"
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("LogNumber") Then
						ds.Tables("ExcelnrptWOSummary").Columns("LogNumber").ColumnName = "Log No."
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WOStartDateFormatted") Then
						ds.Tables("ExcelnrptWOSummary").Columns("WOStartDateFormatted").ColumnName = "Start Date"
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WOCloseDateFormatted") Then
						ds.Tables("ExcelnrptWOSummary").Columns("WOCloseDateFormatted").ColumnName = "WO Closing Date"
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WOActualTime") Then
						ds.Tables("ExcelnrptWOSummary").Columns("WOActualTime").ColumnName = "Time Taken"
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("IsInHouse") Then
						ds.Tables("ExcelnrptWOSummary").Columns("IsInHouse").ColumnName = "In House / Third"
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WOStatusName") Then
						ds.Tables("ExcelnrptWOSummary").Columns("WOStatusName").ColumnName = "W.O.Status"
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("StatusName") Then
						ds.Tables("ExcelnrptWOSummary").Columns("StatusName").ColumnName = "Doc Status"
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WOJobSrNo") Then
						ds.Tables("ExcelnrptWOSummary").Columns("WOJobSrNo").ColumnName = "Sr.No."
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WOJobTypeName") Then
						ds.Tables("ExcelnrptWOSummary").Columns("WOJobTypeName").ColumnName = "Job Type"
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WOJobDescriptionExcel") Then
						ds.Tables("ExcelnrptWOSummary").Columns("WOJobDescriptionExcel").ColumnName = "Description"
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WOJobAction") Then
						ds.Tables("ExcelnrptWOSummary").Columns("WOJobAction").ColumnName = "Action"
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("DueAsOf") Then
						ds.Tables("ExcelnrptWOSummary").Columns("DueAsOf").ColumnName = "Due As Of"
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WOJobEstimatedTime") Then
						ds.Tables("ExcelnrptWOSummary").Columns("WOJobEstimatedTime").ColumnName = "Estimated Time"
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WOJobActualTime") Then
						ds.Tables("ExcelnrptWOSummary").Columns("WOJobActualTime").ColumnName = "Actual Time"
					End If

					If ds.Tables("ExcelnrptWOSummary").Columns.Contains("WOJobStatusName") Then
						ds.Tables("ExcelnrptWOSummary").Columns("WOJobStatusName").ColumnName = "Job Status"
					End If

				End If

				Dim columnToRemove2 As String() = {"ID", "CompanyName", "Address", "Tel1", "Tel2", "Fax", "Email", "Website", "SupplierName", "ReportName", "BranchName", "Aircraft", "KitName", "RelNoteNo", "CurrencySymbol", "currencyName", "ProductVersion", "SINote", "TransTypeID", "ReportDate", "FromStore", "WorkShop", "WorkOrderText", "WorkOrderNo", "SearchStr7", "SearchStr9", "SearchStr10", "SearchStr11", "SearchStr12", "SearchStr13", "SearchStr14", "SearchStr15", "shortName", "SearchStr16", "SearchStr17", "SearchStr18", "SearchStr19", "SearchStr20", "SearchStr21", "SearchStr22", "SearchStr23", "SearchStr24", "SearchStr25", "SearchStr26", "SearchStr27", "SearchStr28", "SearchStr29", "SearchStr30", "SearchStr31", "SearchStr32", "SearchStr33", "SearchStr34", "SearchStr35", "SearchStr36", "SearchStr37", "SearchStr38", "SearchStr39", "SearchStr40", "SearchStr41", "SearchStr42", "SearchStr43", "SearchStr44", "SearchStr45", "SearchStr46", "SearchStr47", "SearchStr48", "SearchStr49", "SearchStr50", "SearchStr51", "SearchStr52", "SearchStr53", "SearchStr54", "SearchStr55", "SearchStr56", "SearchStr57", "SearchStr58", "SearchStr59", "SearchStr60", "SearchStr61", "SearchStr62", "SearchStr63", "SearchStr64", "SearchStr65", "SearchStr66", "SearchStr67", "SearchStr68", "SearchStr69", "SearchStr70", "SearchStr71", "SearchStr72", "SearchStr73", "SearchStr74", "SearchStr75", "SearchStr76", "SearchStr77", "SearchStr78", "SearchStr79", "SearchStr80", "SearchStr81", "SearchStr82", "SearchStr83", "SearchStr84", "SearchStr85", "SearchStr86", "SearchStr87", "SearchStr88", "SearchStr89", "SearchStr90", "SearchStr91", "SearchStr92", "SearchStr93", "SearchStr94", "SearchStr95", "SearchStr96", "SearchStr97", "SearchStr98", "SearchStr99", "SearchStr100"}

				For i As Integer = 0 To columnToRemove2.Length - 1

					If ds.Tables("ReportData").Columns.Contains(columnToRemove2(i)) Then
						ds.Tables("ReportData").Columns.Remove(columnToRemove2(i))
					End If

				Next

				If ds.Tables("ReportData").Columns.Contains("SearchStr1") Then
					ds.Tables("ReportData").Columns("SearchStr1").ColumnName = "Date"
				End If

				If ds.Tables("ReportData").Columns.Contains("SearchStr2") Then
					ds.Tables("ReportData").Columns("SearchStr2").ColumnName = "Customer"
				End If

				If ds.Tables("ReportData").Columns.Contains("SearchStr3") Then
					ds.Tables("ReportData").Columns("SearchStr3").ColumnName = "W.O Report Type"
				End If

				If ds.Tables("ReportData").Columns.Contains("SearchStr4") Then
					ds.Tables("ReportData").Columns("SearchStr4").ColumnName = "RegNo"
				End If

				If ds.Tables("ReportData").Columns.Contains("SearchStr5") Then
					ds.Tables("ReportData").Columns("SearchStr5").ColumnName = "Model"
				End If

				If ds.Tables("ReportData").Columns.Contains("SearchStr6") Then
					ds.Tables("ReportData").Columns("SearchStr6").ColumnName = "Status"
				End If

				If ds.Tables("ReportData").Columns.Contains("SearchStr8") Then
					ds.Tables("ReportData").Columns("SearchStr8").ColumnName = " W.O. JobType"
				End If

				Dim dsNew As New DataSet
				dsNew.Clear()

				dsNew.Merge(ds.Tables("ReportData"))
				dsNew.Merge(ds.Tables("ExcelnrptWOSummary"))
				dsNew.Tables("ReportData").TableName = "Searching Criteria"
				dsNew.Tables("ExcelnrptWOSummary").TableName = "W. O. Summary Report"

				Session("ExcelFileName") = "W. O. Summary Report"
				Session("dsNew") = dsNew
				Session("DataTableToBeFormattedForExportToExcel") = "W. O. Summary Report"

				ScriptManager.RegisterStartupScript(Me,
													[GetType],
													"openFile",
													"openFile();",
													True)
				MarkLog(Action.Print,
						"WorkOrderReport",
						"Export To excel " + EventLogDetail,
						ErrorType.NoError,
						Guid.Empty,
						EventLogID) 'Added by Shital on 18-Jan-2021

			Else

				da.Fill(ds, WOSummary)
				myReport.SetDataSource(ds)
				Session("CrystalReport") = myReport

				ScriptManager.RegisterStartupScript(Me,
													[GetType],
													"openTranDetail",
													"openTranDetail();",
													True)

				MarkLog(Action.Print,
						"WorkOrderReport",
						EventLogDetail,
						ErrorType.NoError,
						Guid.Empty,
						EventLogID)

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub AddAttributes()
		txtWONo.Attributes.Add("onKeyPress", "validateText(('D'),document.getElementById('txtWONo').value,event)")
	End Sub

#End Region

#Region " Data Binding "

	Private Sub DataFieldBind()

		Try

			If AppSettings("ShowCAMOOnlyForNewClients") = "True" And AppSettings("ShowAMOOnlyForNewClients") = "False" Then
				TransTypeID = 89 'CAMO
			ElseIf AppSettings("ShowCAMOOnlyForNewClients") = "False" And AppSettings("ShowAMOOnlyForNewClients") = "True" Then
				TransTypeID = 88 'Third party
			ElseIf AppSettings("ShowCAMOOnlyForNewClients") = "True" And AppSettings("ShowAMOOnlyForNewClients") = "True" Then
				TransTypeID = Val(cmbTransType.SelectedValue.ToString)
			Else
				TransTypeID = Val(cmbTransType.SelectedValue.ToString)
			End If

			DistinctWOText = nDistinctWOText.GetDistinctWOText("(SELECT)", TransTypeID:=TransTypeID)
			cmbWO.DataSource = DistinctWOText
			Session("DistinctWOText") = DistinctWOText

			'Customer
			CustomerList = VendorList.GetVendorstList(0, , , , , , "(SELECT)", True, False, False)
			cmbCustomer.DataSource = CustomerList
			Session("CustomerList") = CustomerList
			WOJobTypeList = nWOJobTypeList.GetWOJobTypeList("(ALL)")

			WOStatusList = nWOStatusList.GetWOStatusListList(, "(ALL)")
			cmbStatus.DataSource = WOStatusList
			Session("WOStatusList") = WOStatusList

			If AppSettings("ShowCAMOOnlyForNewClients") = "True" And AppSettings("ShowAMOOnlyForNewClients") = "False" Then

				WOJobTypeIDList = (From c As nWOJobTypeList.nWOJobTypeListInfo In WOJobTypeList
								   Where {0, 1, 2, 3}.Contains(c.ID)
								   Select c).ToList
				cmbWOJobType.DataSource = WOJobTypeIDList
				Session("WOJobTypeList") = WOJobTypeIDList

				phCustomer.Visible = False
				lblSelectionofCAMOThirdParty.Visible = False
				lblCAMOThirdParty.Visible = False
				phWOType.Visible = False

			ElseIf AppSettings("ShowCAMOOnlyForNewClients") = "False" And AppSettings("ShowAMOOnlyForNewClients") = "True" Then

				WOJobTypeIDList = (From c As nWOJobTypeList.nWOJobTypeListInfo In WOJobTypeList
								   Where {0, 1, 5}.Contains(c.ID)
								   Select c).ToList
				cmbWOJobType.DataSource = WOJobTypeIDList
				Session("WOJobTypeList") = WOJobTypeIDList
				phCustomer.Visible = True
				lblSelectionofCAMOThirdParty.Visible = False
				lblCAMOThirdParty.Visible = False
				phWOType.Visible = False

			ElseIf AppSettings("ShowCAMOOnlyForNewClients") = "True" And AppSettings("ShowAMOOnlyForNewClients") = "True" Then

				WOJobTypeIDList = (From c As nWOJobTypeList.nWOJobTypeListInfo In WOJobTypeList
								   Where {0, 1, 2, 3, 5}.Contains(c.ID)
								   Select c).ToList
				cmbWOJobType.DataSource = WOJobTypeIDList
				Session("WOJobTypeList") = WOJobTypeIDList
				phCustomer.Visible = False
				lblSelectionofCAMOThirdParty.Visible = True
				lblCAMOThirdParty.Visible = True
				phWOType.Visible = True

			Else

				WOJobTypeIDList = (From c As nWOJobTypeList.nWOJobTypeListInfo In WOJobTypeList
								   Select c).ToList
				cmbWOJobType.DataSource = WOJobTypeIDList
				Session("WOJobTypeList") = WOJobTypeIDList
				phCustomer.Visible = True
				phWOType.Visible = True

			End If

			DataBind()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Events "

	Private Sub Page_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		Try

			GetSession()
			AddAttributes()
			EventLogID = CType(Session("EventLogID"), Guid)

			If Not IsPostBack Then

				RemoveSession()

				If cmbDateRange.Enabled = True Then
					setFocus(cmbDateRange)
				End If

				DataFieldBind()
				ControlVisibilityDateRange(6)
				SetDatePeriod(6)
				cmbDateRange.SelectedIndex = 6
				ControlVisibilityPageLabels()
				lblStep7.Text = IIf(AppSettings("ClientCode") = "BRD" Or AppSettings("ClientCode") = "LAMA", "Display Report", "Display Report")
				lblSortLabel.Text = "Selection of Sorting" 'Added By Vikrant on 10-Jul-2020 For All10072020

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub CurrentSearchCriteria(sender As Object, e As EventArgs) Handles btnCurrentSearchCriteria.Click

		Try

			ControlVisibilitySearchCriteria()
			SetValues()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub DisplayReport(sender As Object, e As EventArgs) Handles btnDisplay.Click

		Try

			SetReport()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	' Added By Abhishek on 26-SEP-2017
	Private Sub ExportToExcel(sender As Object, e As EventArgs) Handles btnExport.Click

		Try

			If IsValid Then

				SetValues()
				SetReport(True)

			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub CloseScreen(sender As Object, e As EventArgs) Handles btnClose.Click

		Try

			RemoveSession()
			Session("MiddleFrame") = ""
			Response.Redirect("Dashboard.aspx")

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try
	End Sub

	Private Sub WOChanged(sender As Object, e As EventArgs) Handles cmbWO.SelectedIndexChanged

		Try

			txtWONo.Text = ""
			txtWONo.Visible = IIf(cmbWO.SelectedIndex > 0, True, False)
			If cmbWO.Enabled = True Then
				setFocus(cmbWO)
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub DateRangeChanged(sender As Object, e As EventArgs) Handles cmbDateRange.SelectedIndexChanged

		Try

			Dim Index As Int16 = IIf(cmbDateRange.SelectedIndex <= 0, 0, cmbDateRange.SelectedIndex)
			ControlVisibilityDateRange(Index)
			SetDatePeriod(Index)

			If cmbDateRange.Enabled = True Then
				setFocus(cmbDateRange)
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub TransTypeChanged(sender As Object, e As EventArgs) Handles cmbTransType.SelectedIndexChanged

		Try

			DistinctWOText = nDistinctWOText.GetDistinctWOText(AddTopItem:="(SELECT)",
																 TransTypeID:=Val(cmbTransType.SelectedValue.ToString))
			cmbWO.DataSource = DistinctWOText
			Session("DistinctWOText") = DistinctWOText

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

	Private Sub WOJobTypeChanged(sender As Object, e As EventArgs) Handles cmbWOJobType.SelectedIndexChanged

		Try

			If AppSettings("ClientCode") = "STR" Then

				If cmbWOJobType.SelectedValue = "1" Or cmbWOJobType.SelectedIndex = 0 Then
					chkOtherJob.Visible = True
				Else
					chkOtherJob.Visible = False
					chkOtherJob.Checked = False
				End If

			Else
				chkOtherJob.Visible = False
				chkOtherJob.Checked = False
			End If

			upnlWOJobType.Update()

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Sub

#End Region

#Region " Service Methods "

	<Services.WebMethod(), Script.Services.ScriptMethod()>
	Public Shared Function GetRegTextList(prefixText As String, count As Integer, contextKey As String) As String()

		Dim DistinctTextList As DistinctTextListAutoComplete
		Try

			DistinctTextList = DistinctTextListAutoComplete.GetDistinctTextList(prefixText, 28)

			If count = 0 Then
				Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In DistinctTextList
						Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).ToArray
			Else
				Return (From c As DistinctTextListAutoComplete.DistinctTextListAutoCompleteInfo In DistinctTextList
						Select AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Text, c.Text)).Take(count).ToArray
			End If

		Catch ex As Exception
			Throw ex.GetBaseException
		End Try

	End Function

#End Region

End Class