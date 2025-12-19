'********************************************
'Created by:    Harsh Sugandhi
'Created on:    08-April-2024
'Created for:   FLYPAL-1545 Dashboards for Discrepancy Details
'Modified by Harsh Sugandhi on 4th September 2024 for FLYPAL-1850 AOG Aircraft Details on Discrepancy Dashboard
'********************************************


Imports System.Web.Script.Serialization
Imports System.Web.Services

Imports Flypal.DiscrepancyDashboardAOGAircraftDetails

Public Class DiscrepancyDashboard
	Inherits Page

#Region " Variable(s) Declaration "

	Public DiscrepancyStatusCount As DiscrepancyStatusCountDashboard
	Public AircraftList As MachineNameValueList
	Public DiscrepancyList As MELSnagCorrectiveActionListNew

	Dim Year As String
	Shared DiscrepanciesListForCalendar As String = ""

#End Region

#Region " Helper Method(s) "

	Private Sub GetSession()

		DiscrepancyList = Session("AircraftWiseDiscrepancies")

	End Sub

	Private Sub SetSession()

		Session("MachineID") = ddlAircraft.SelectedValue
		Session("AircraftWiseDiscrepancies") = DiscrepancyList

	End Sub

	Private Sub ReportsUpdatePanel()

		upnlMonthwiseDiscrepancies.Update()
		upnlMonthwiseDetailedDiscrepancies.Update()
		upnlAircraftwiseDiscrepancies.Update()

	End Sub

	Private Sub GridsUpdatePanel()

		upnlAircraftWiseDiscrepanciesGridHeader.Update()
		upnlAircraftwiseDiscrepanciesTabularReport.Update()

	End Sub

	Private Sub ControlVisibility()

		Try

			LoadStatusCards()

			If User.IsInRole("MonthWiseDiscrepanciesGraphView") Then
				phMonthwiseDiscrepancies.Visible = True
				SetBarGraph()
			End If

			If User.IsInRole("MonthWiseDiscrepanciesDetailedGraphView") Then
				phMonthwiseDetailedDiscrepancies.Visible = True
				SetDetailedGraph()
			End If

			If User.IsInRole("AircraftWiseDiscrepanciesGraphView") Then
				phAircraftWiseDiscrepancies.Visible = True
				SetPieGraph()
			End If

			If User.IsInRole("ATAWiseDiscrepanciesGraphView") Then
				phATAWiseDiscrepancies.Visible = True
				SetATADetailedGraph()
			End If

			If User.IsInRole("AircraftWiseDiscrepanciesTabularReportView") Then
				phAircraftWiseDiscrepanciesGV.Visible = True
			End If

			If User.IsInRole("AircraftWiseDiscrepancyDetailsView") Then
				phAircraftWiseDiscrepanciesDetails.Visible = True
				SetAircraftwiseDetailedGraph()
			End If

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " Data Binding "

	Private Sub SetDropDown()

		Dim cnt As Integer
		Dim PreviousYear As Integer
		Dim NextYear As Integer

		Year = Now.Year
		PreviousYear = Year - 10
		NextYear = Year + 10

		Try

			If Not IsPostBack Then
				For cnt = PreviousYear To NextYear
					ddlYear.Items.Add(cnt)
				Next


				If ddlYear.Enabled = True Then
					SetFocus(ddlYear)
				End If

				ddlYear.DataBind()

				For k As Integer = 1 To 12
					Dim mon As String = MonthName(k, False)
					ddlMonth.Items.Add(mon)
				Next

				ddlMonth.DataBind()

				If Now.Month = 1 Then
					ddlYear.SelectedValue = Now.Year - 1
					ddlMonth.SelectedValue = MonthName(12, False)
				Else
					ddlYear.SelectedValue = Now.Year
					ddlMonth.SelectedValue = MonthName(Now.Month - 1, False)
				End If

			End If

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Public Sub DataFieldBind()

		Try

			DiscrepancyStatusCount = DiscrepancyStatusCountDashboard.GetDiscrepancyStatusCountDashboard()
			Session("DiscrepancyStatusCount") = DiscrepancyStatusCount

			AircraftList = MachineNameValueList.GetMachineList(Today.Date.ToString)
			ddlAircraft.DataSource = AircraftList
			Session("AircraftList") = AircraftList

			DataBind()
			SetDropDown()
			SetSession()
			GridBinding()

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Public Sub GridBinding()

		Dim MachineID As Guid
		Dim InvestigationStatus As Integer
		Try

			MachineID = New Guid(ddlAircraft.SelectedValue)

			InvestigationStatus = IIf(Expression:=ShowClosedDiscrepancy.Checked,
									  TruePart:=0,
									  FalsePart:=4)

			DiscrepancyList = MELSnagCorrectiveActionListNew.GetMELSnagCorrectiveActionListNew(MachineID:=MachineID.ToString(),
																							   InvestigationStatus:=InvestigationStatus)
			gvAircraftWiseDiscrepancies.DataSource = DiscrepancyList
			gvAircraftWiseDiscrepancies.DataBind()
			Session("AircraftWiseDiscrepancies") = DiscrepancyList

			lblGridText.Text = "Discrepancy Details of " + ddlAircraft.SelectedItem.Text + " Aircraft."

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " Page Event(s) "

	Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

		GetSession()
		Try

			If Not IsPostBack Then

				Session("MiddleFrame") = "DiscrepancyDashboard.aspx?"
				DataFieldBind()
				ControlVisibility()
				ReportsUpdatePanel()
				GridsUpdatePanel()

				If User.IsInRole("CalendarForDiscrepanciesView") Then
					ShowClosedDiscrepancy.Visible = True
					SetCalendar()
				End If

			End If

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub Page_Error(sender As Object, e As EventArgs) Handles MyBase.Error
		Session("Message") = Context.Server.GetLastError.Message
		Session("Source") = Context.Server.GetLastError.Source
		Session("Trace") = Context.Server.GetLastError.StackTrace
	End Sub

#End Region

#Region " Control Event(s) "

	Private Sub DropDownChangeEvent() Handles ddlAircraft.SelectedIndexChanged,
											  ddlYear.SelectedIndexChanged,
											  ddlMonth.SelectedIndexChanged

		GetSession()
		Try

			GridBinding()
			ControlVisibility()
			ReportsUpdatePanel()
			GridsUpdatePanel()
			upnlAircraftWiseDiscrepanciesGridHeader.Update()

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub CheckboxChangeEvent() Handles ShowClosedDiscrepancy.CheckedChanged

		GetSession()
		Try

			GridBinding()
			ControlVisibility()
			GridsUpdatePanel()

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " Grid View Event(s) "

	Private Sub PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvAircraftWiseDiscrepancies.PageIndexChanging

		Try

			gvAircraftWiseDiscrepancies.PageIndex = e.NewPageIndex
			gvAircraftWiseDiscrepancies.DataSource = DiscrepancyList
			Session("AircraftWiseDiscrepancies") = DiscrepancyList
			gvAircraftWiseDiscrepancies.DataBind()

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub Sorting(sender As Object, e As GridViewSortEventArgs) Handles gvAircraftWiseDiscrepancies.Sorting

		Try

			DiscrepancyList.Sort(e.SortExpression, ComponentModel.ListSortDirection.Ascending)
			Session("AircraftWiseDiscrepancies") = DiscrepancyList
			gvAircraftWiseDiscrepancies.DataSource = DiscrepancyList
			gvAircraftWiseDiscrepancies.DataBind()

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	'Added by Harsh Sugandhi on 4th September 2024 for FLYPAL-1850 AOG Aircraft Details on Discrepancy Dashboard
	Private Sub GVAircraftWiseDiscrepanciesRowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvAircraftWiseDiscrepancies.RowCreated

		Try

			If e.Row.RowType = DataControlRowType.Header Then

				For Each Cell As TableCell In e.Row.Cells

					If Cell.HasControls() Then

						Dim sortLinkButton As LinkButton = TryCast(Cell.Controls(0), LinkButton)

						If sortLinkButton IsNot Nothing Then

							Cell.ToolTip = "Click to sort by " & sortLinkButton.Text

						End If

					End If

				Next

			End If

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Private Sub GVAircraftWiseDiscrepanciesRowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvAircraftWiseDiscrepancies.RowDataBound

		Try

			If e.Row.RowType = DataControlRowType.DataRow Then

				Dim statusCell As TableCell = e.Row.Cells(7)

				Dim status As String = statusCell.Text.ToLower()

				Select Case status
					Case "open"
						statusCell.CssClass = "GV_StatusOpen"
					Case "deferred"
						statusCell.CssClass = "GV_StatusDeferred"
					Case "closed"
						statusCell.CssClass = "GV_StatusClosed"
					Case "aog"
						statusCell.CssClass = "GV_StatusAOG"
					Case Else
						statusCell.CssClass = "GV_StatusDefault"
				End Select

			End If

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " Graph(s) "

	Private Sub LoadStatusCards()

		Try

			Dim AOGList As New DiscrepancyDashboardAOGAircraftDetails

			AOGList = GetAOGAircraftDetails_DiscrepancyDashboard()

			For Each AOG As DiscrepancyDashboardAOGAircraftDetailsInfo In AOGList

				Dim cardDiv As New Literal With {
					.Text = $"<div class='card'>" &
							$"<div class='card-header'>{AOG.Aircraft}</div>" &
							$"<div class='card-status'>{AOG.Status}</div>" &
							$"<div class='card-details'>" &
							$"<p>Location: {AOG.Sector}</p>" &
							$"<p>Reason: {AOG.Remark}</p>" &
							$"<p>Discrepancy No: {AOG.DiscrepancyNo}</p>" &
							$"<p>Discrepancy: {AOG.Discrepancy}</p>" &
							$"<p>Log No: {AOG.LogNo}</p>" &
							$"<p>Log Page No: {AOG.LogPageNo}</p>" &
							$"<p>Log Date: {AOG.LogDateFormatted}</p>" &
							$"</div></div>"
				}

				statusCards.Controls.Add(cardDiv)

			Next

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Public Sub SetCalendar()

		Try
			ScriptManager.RegisterStartupScript(page:=Me,
												type:=[GetType],
												key:="CalendarForDiscrepancy",
												script:="fn_CalendarForDiscrepancy();",
												addScriptTags:=True)
		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Public Sub SetBarGraph()

		Dim mDiscrepancyMonthWiseGraphReport As DiscrepancyMonthWiseGraphReport

		Try

			mDiscrepancyMonthWiseGraphReport = DiscrepancyMonthWiseGraphReport.GetDiscrepancyMonthWiseGraphReport(Year:=IIf(Expression:=ddlYear.SelectedIndex > -1,
																															CInt(ddlYear.SelectedItem.Text),
																															""))

			Dim BarGraphValues As String = New JavaScriptSerializer().Serialize(mDiscrepancyMonthWiseGraphReport)
			BarGraphValues = BarGraphValues.Replace("MonthName", "label").Replace("DiscrepancyCount", "value")

			ScriptManager.RegisterStartupScript(page:=Me,
												type:=[GetType],
												key:="FusionChart",
												script:="fn_FusionChart('" + BarGraphValues.ToString() + "');",
												addScriptTags:=True)
		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Public Sub SetDetailedGraph()

		Dim mOpenDiscrepanciesCount,
			mDeferredDiscrepanciesCount,
			mClosedDiscrepanciesCount As DiscrepancyStatusCountGraph

		Try

			mOpenDiscrepanciesCount = DiscrepancyStatusCountGraph.GetDiscrepancyStatusCountGraph(Year:=IIf(ddlYear.SelectedIndex > -1,
																										   CInt(ddlYear.SelectedItem.Text),
																										   ""),
																								 MachineID:=ddlAircraft.SelectedValue.ToString(),
																								 Type:="Open")
			mDeferredDiscrepanciesCount = DiscrepancyStatusCountGraph.GetDiscrepancyStatusCountGraph(Year:=IIf(ddlYear.SelectedIndex > -1,
																											   CInt(ddlYear.SelectedItem.Text),
																											   ""),
																									 MachineID:=ddlAircraft.SelectedValue.ToString(),
																									 Type:="Deferred")
			mClosedDiscrepanciesCount = DiscrepancyStatusCountGraph.GetDiscrepancyStatusCountGraph(Year:=IIf(ddlYear.SelectedIndex > -1,
																											 CInt(ddlYear.SelectedItem.Text),
																											 ""),
																								   MachineID:=ddlAircraft.SelectedValue.ToString(),
																								   Type:="Closed")

			Dim OpenDiscrepanciesCount As String = New JavaScriptSerializer().Serialize(mOpenDiscrepanciesCount)
			OpenDiscrepanciesCount = OpenDiscrepanciesCount.Replace("DiscrepancyCount", "value")

			Dim DeferredDiscrepanciesCount As String = New JavaScriptSerializer().Serialize(mDeferredDiscrepanciesCount)
			DeferredDiscrepanciesCount = DeferredDiscrepanciesCount.Replace("DiscrepancyCount", "value")

			Dim ClosedDiscrepanciesCount As String = New JavaScriptSerializer().Serialize(mClosedDiscrepanciesCount)
			ClosedDiscrepanciesCount = ClosedDiscrepanciesCount.Replace("DiscrepancyCount", "value")

			ScriptManager.RegisterStartupScript(page:=Me,
												type:=[GetType],
												key:="FusionChartDiscrepancies",
												script:="fn_FusionChartDiscrepancies('" + OpenDiscrepanciesCount.ToString() + "', '" +
																						  DeferredDiscrepanciesCount.ToString() + "', '" +
																						  ClosedDiscrepanciesCount.ToString() + "');",
												addScriptTags:=True)
		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Public Sub SetPieGraph()

		Dim AircraftWiseDiscrepancies As AircraftWiseDiscrepancies

		Try

			AircraftWiseDiscrepancies = AircraftWiseDiscrepancies.GetAnnualDiscrepancyForGraph(Year:=IIf(Expression:=ddlYear.SelectedIndex > -1,
																										 TruePart:=CInt(ddlYear.SelectedItem.Text),
																										 FalsePart:=""))

			Dim PieGraphValues As String = New JavaScriptSerializer().Serialize(AircraftWiseDiscrepancies)

			PieGraphValues = PieGraphValues.Replace("RegNo", "label").Replace("DiscrepancyCount", "value")

			ScriptManager.RegisterStartupScript(page:=Me,
												type:=[GetType],
												key:="FusionChartPie",
												script:="fn_FusionChartPie('" + PieGraphValues.ToString() + "');",
												addScriptTags:=True)
		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	Public Sub SetATADetailedGraph()

		Dim mATAWiseOpenDiscrepanciesCount As DiscrepancyATAWiseGraph
		Dim mATAWiseDeferredDiscrepanciesCount As DiscrepancyATAWiseGraph
		Try

			mATAWiseOpenDiscrepanciesCount = DiscrepancyATAWiseGraph.GetDiscrepancyForPieGraph(DiscrepancyType:="Open")

			mATAWiseDeferredDiscrepanciesCount = DiscrepancyATAWiseGraph.GetDiscrepancyForPieGraph(DiscrepancyType:="Deferred")

			Dim ATAWiseOpenDiscrepanciesCount As String = New JavaScriptSerializer().Serialize(mATAWiseOpenDiscrepanciesCount)
			ATAWiseOpenDiscrepanciesCount = ATAWiseOpenDiscrepanciesCount.Replace("ATACodeForCalendar", "label").
																		  Replace("DiscrepancyCount", "value")

			Dim ATAWiseDeferredDiscrepanciesCount As String = New JavaScriptSerializer().Serialize(mATAWiseDeferredDiscrepanciesCount)
			ATAWiseDeferredDiscrepanciesCount = ATAWiseDeferredDiscrepanciesCount.Replace("ATACodeForCalendar", "label").
																				  Replace("DiscrepancyCount", "value")

			ScriptManager.RegisterStartupScript(page:=Me,
												type:=[GetType],
												key:="FusionChartDiscrepancies_ATA",
												script:="fn_FusionChartDiscrepancies_ATA('" + ATAWiseOpenDiscrepanciesCount.ToString() + "', '" +
																							  ATAWiseDeferredDiscrepanciesCount.ToString() + "');",
												addScriptTags:=True)

		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

	'Added by Harsh Sugandhi on 4th September 2024 for FLYPAL-1850 AOG Aircraft Details on Discrepancy Dashboard
	Public Sub SetAircraftWiseDetailedGraph()

		Dim mOpenDiscrepanciesCount,
			mDeferredDiscrepanciesCount,
			mClosedDiscrepanciesCount As AircraftWiseDetailedDiscrepancies

		Dim AircraftList As String = New JavaScriptSerializer().Serialize(Me.AircraftList)

		Try

			mOpenDiscrepanciesCount =
				AircraftWiseDetailedDiscrepancies.
					GetAircraftWise_DetailedDiscrepanciesCount(Year:=IIf(ddlYear.SelectedIndex > -1,
																		 CInt(ddlYear.SelectedItem.Text),
																		 ""),
															   AircraftList:=Me.AircraftList,
															   Type:="Open")

			mDeferredDiscrepanciesCount =
				AircraftWiseDetailedDiscrepancies.
					GetAircraftWise_DetailedDiscrepanciesCount(Year:=IIf(ddlYear.SelectedIndex > -1,
																		 CInt(ddlYear.SelectedItem.Text),
																		 ""),
															   AircraftList:=Me.AircraftList,
															   Type:="Deferred")

			mClosedDiscrepanciesCount =
				AircraftWiseDetailedDiscrepancies.
					GetAircraftWise_DetailedDiscrepanciesCount(Year:=IIf(ddlYear.SelectedIndex > -1,
																		 CInt(ddlYear.SelectedItem.Text),
																		 ""),
															   AircraftList:=Me.AircraftList,
															   Type:="Closed")

			Dim OpenDiscrepanciesCount As String = New JavaScriptSerializer().Serialize(mOpenDiscrepanciesCount)
			OpenDiscrepanciesCount = OpenDiscrepanciesCount.Replace("DiscrepancyCount", "value")

			Dim DeferredDiscrepanciesCount As String = New JavaScriptSerializer().Serialize(mDeferredDiscrepanciesCount)
			DeferredDiscrepanciesCount = DeferredDiscrepanciesCount.Replace("DiscrepancyCount", "value")

			Dim ClosedDiscrepanciesCount As String = New JavaScriptSerializer().Serialize(mClosedDiscrepanciesCount)
			ClosedDiscrepanciesCount = ClosedDiscrepanciesCount.Replace("DiscrepancyCount", "value")

			ScriptManager.RegisterStartupScript(page:=Me,
												type:=[GetType],
												key:="FusionChartDetailedDiscrepancies",
												script:="fn_FusionChartDetailedDiscrepancies('" + OpenDiscrepanciesCount.ToString() + "', '" +
																								  DeferredDiscrepanciesCount.ToString() + "', '" +
																								  ClosedDiscrepanciesCount.ToString() + "', '" +
																								  AircraftList.ToString() + "');",
												addScriptTags:=True)
		Catch ex As Exception
			ex.GetBaseException()
		End Try

	End Sub

#End Region

#Region " Web Method(s) "

	<WebMethod(EnableSession:=True)>
	Public Shared Function DiscrepanciesForCalendar(Month As String,
													Year As String) As String

		Dim mDiscrepancyListForCalendar As DiscrepancyListForCalendar

		Try

			Dim ToDate As New SmartDate(Value:=DateSerial(Year:=Today.Date.Year,
										Month:=Today.Date.Month,
										Day:=Date.DaysInMonth(year:=Today.Date.Year,
															  month:=Today.Date.Month)),
										EmptyIsMin:=False)

			Dim FromDate As New SmartDate(Value:=Today.AddYears(value:=-1).ToString)

			mDiscrepancyListForCalendar = DiscrepancyListForCalendar.GetDiscrepancyListForCalendar(FromDate:=FromDate.ToString(),
																								   ToDate:=ToDate.ToString(),
																								   InvestigationStatus:=4)

			DiscrepanciesListForCalendar = New JavaScriptSerializer().Serialize(mDiscrepancyListForCalendar)

			DiscrepanciesListForCalendar = DiscrepanciesListForCalendar.Replace("HeaderText", "title").
																		Replace("DateForCalendar", "start").
																		Replace("ID", "id")

			Return DiscrepanciesListForCalendar

		Catch ex As Exception
			Throw ex
		End Try

	End Function

	<WebMethod(EnableSession:=True)>
	Public Shared Function DiscrepancyDetails(MELSnagCorrectiveActionID As String) As Boolean

		Dim mMELSnagCorrectiveAction As MELSnagCorrectiveAction

		Try

			mMELSnagCorrectiveAction = MELSnagCorrectiveAction.GetMELSnagCorrectiveAction(ID:=New Guid(MELSnagCorrectiveActionID))

			HttpContext.Current.Session("MachineID") = mMELSnagCorrectiveAction.MachineID
			HttpContext.Current.Session("mDiscrepancyCorrectiveAction") = mMELSnagCorrectiveAction

			If (Not HttpContext.Current.User.IsInRole("DiscrepancyActionView") And
				Not HttpContext.Current.User.IsInRole("DiscrepancyActionEdit")) Then
				Return False
			Else
				Return True
			End If

		Catch ex As Exception
			Throw ex
		End Try

	End Function

#End Region

End Class