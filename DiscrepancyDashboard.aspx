<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DiscrepancyDashboard.aspx.vb" Inherits="Flypal.DiscrepancyDashboard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SI.UTILITY" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

	<title>Discrepancy Dashboard</title>
	<meta content="False" name="vs_showGrid" />
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />

	<script src="js/jquery-1.8.3.js" type="text/javascript"></script>
	<%-- FusionCharts Scripts--%>
	<script src="FusionCharts/fusioncharts.js" type="text/javascript"></script>
	<script src="FusionCharts/fusioncharts.charts.js" type="text/javascript"></script>
	<script src="FusionCharts/themes/fusioncharts.theme.zune.js" type="text/javascript"></script>
	<script src="VALIDATEFUNCTIONS.js" type="text/javascript"></script>
	<%-- JQGrid Styles--%>
	<link href="JQGridReq/jqueryui/1.8.23/jquery-ui.css" rel="stylesheet" type="text/css" />
	<link href="JQGridReq/Site.css" rel="stylesheet" type="text/css" />
	<link href="JQGridReq/ui.jqgrid.css" rel="stylesheet" type="text/css" />
	<link href="JQGridReq/jquery-ui-1.9.2.custom.css" rel="stylesheet" type="text/css" />
	<%-- End JQGrid --%>
	<link href="bootstrap.cosmo.css" rel="stylesheet" type="text/css" />
	<link href="bootstrap.cosmo.min.css" rel="stylesheet" type="text/css" />
	<link href="bootstrap-theme.css" rel="stylesheet" type="text/css" />
	<link href="bootstrap/bootstrap.min.css" rel="stylesheet" type="text/css" />
	<link rel="stylesheet" href="fullcalendar/fullcalendar.css" type="text/css" />
	<link rel="stylesheet" href="fullcalendar/fullcalendar.print.css" type="text/css" media="print" />
	<link rel="stylesheet" type="text/css" href="css/libs/nanoscroller.css" />
	<link rel="stylesheet" type="text/css" href="css/libs/font-awesome.css" />
	<link rel="stylesheet" type="text/css" href="css/libs/ns-default.css" />
	<link rel="stylesheet" type="text/css" href="css/libs/ns-style-bar.css" />
	<link rel="stylesheet" type="text/css" href="css/libs/ns-style-attached.css" />
	<link rel="stylesheet" type="text/css" href="css/libs/ns-style-other.css" />
	<link rel="stylesheet" type="text/css" href="css/libs/ns-style-theme.css" />
	<link rel="stylesheet" href="css/libs/daterangepicker.css" type="text/css" />
	<link rel="stylesheet" type="text/css" href="css/libs/magnific-popup.css" />
	<link href="Styles.css" id="MainStyle" type="text/css" rel="stylesheet" />

	<%--Scripts to be loaded before DOM--%>
	<script type="text/javascript" id="startupScripts">

		if ("<%=Not HttpContext.Current.Session("StyleSheet") Is Nothing %>" == "True") {
			$("#MainStyle").attr('href',"<%= HttpContext.Current.Session("StyleSheet") %>");
		}

		document.addEventListener('DOMContentLoaded', function () {
			const calendarEl = document.getElementById('Calendar');
			const calendar = new FullCalendar.Calendar(calendarEl, {
				initialView: 'dayGridMonth'
			});
			calendar.render();
		});

		$(document).ready(function () {

			$("#barReport").addClass("selected-option-for-report");

			$('.counter-value').each(function () {
				$(this).prop('Counter', 0).animate({
					Counter: $(this).text()
				}, {
					duration: 3500,
					easing: 'swing',
					step: function (now) {
						$(this).text(Math.ceil(now));
					}
				});
			});
		});

	</script>

	<script src="jquery.tooltip.min.js" type="text/javascript"></script>

	<%-- JS Functions for Calendar, Graph, Pie --%>
	<script type="text/javascript">

		/*Function for Calendar*/
		function fn_CalendarForDiscrepancy() {

			$('#calendar').fullCalendar({
				header: {
					left: 'prev,next today',
					center: 'title',
					right: ''
				},

				defaultView: 'month',
				defaultDate: new Date(),
				editable: true,
				navLinks: false,
				height: 350,
				selectable: true,
				allDayDefault: false,
				buttonText: {
					today: 'today',
					month: 'month',
					week: 'week',
					day: 'day'
				},

				events: function (start, end, timezone, callback) {

					var date = new Date($('#calendar').fullCalendar('getDate'));
					var month = date.getMonth();
					var year = date.getFullYear();

					$.ajax({
						type: "POST",
						data: "{ 'Month': '" + month + "', 'Year': '" + year + "' }",

						url: "DiscrepancyDashboard.aspx/DiscrepanciesForCalendar",
						dataType: 'json',
						contentType: "application/json",

						success: function (data) {
							var events = [];
							var obj = jQuery.parseJSON(data.d);
							$(obj).each(function () {
								events.push({
									title: $(this).attr('title'),
									start: $(this).attr('start'),
									InvestigationStatusDiscrepancyText: $(this).attr('InvestigationStatusDiscrepancyText'),
									MELSnagCorrectiveActionID: $(this).attr('id'),
									DiscrepancyDetailsForCalendar: $(this).attr('DiscrepancyDetailsForCalendar'),
									className: ["user_block", "bday_block"],
									tooltip: $(this).attr('DiscrepancyDetailsForCalendar'),
									allday: true,
								});
							});
							//console.log("Data recieved from webmethod vm_DiscrepanciesForCalendar() in JSON format. ");
							//console.log(events);
							callback(events);
							//console.log("callback function executed succesfully !!");
						},
						error: function (xhr, status, error) {
							console.error("Error Occured after call for WebMethod. Following is the Reponse Text : " + xhr.responseText);
							console.error("Error Occured after call for WebMethod. Following is the Error Text : " + error);
							console.error("Error Occured after call for WebMethod. Following is the Error status : " + status);
						}
					});
				},
				displayEventTime: false,
				eventRender: function (event, element) {
					$(element).tooltip({
						title: event.DiscrepancyDetailsForCalendar,
						placement: "top",
						trigger: "hover",
						container: "body",
						html: true,
						animation: true,
					});

					element.css("font-size", "1rem");
					element.css("color", "white");
					element.css("padding", "5px");
					element.find('.fc-title').html(event.title);
					if (event.InvestigationStatusDiscrepancyText != "") {
						if (event.InvestigationStatusDiscrepancyText == "Open") {
							element.css('background-color', '#1C7AC0');				// Blue : Open
						}
						else if (event.InvestigationStatusDiscrepancyText == "Deferred") {
							element.css('background-color', '#d934d1');				// Pink : Deferred
						}
					}
				},
				eventClick: function (event, jsEvent, view) {
					ViewDiscrepancyDetail(event.MELSnagCorrectiveActionID);
					return false;
				}
			});
		}

		/*Function for BarGraph*/
		function fn_FusionChart(BarGraphValues) {
			var revenueChart = new FusionCharts({
				"type": "Column2D",
				"renderAt": "MonthwiseDiscrepancies",
				"width": "550",
				"height": "350",
				"dataFormat": "json",
				"dataSource": {
					"chart": {
						"caption": "Total Discrepancies per Month <br> (Of all Aircrafts)",
						"subCaption": "<br>" + $("#ddlYear :selected").text(),
						"xAxisName": "Month",
						"yAxisName": "Count",
						"exportEnabled": "1",
						"theme": "zune",
						"plotTooltext": "<b> $label : </b> $datavalue"
					},
					"data": JSON.parse(BarGraphValues)
				}
			});
			revenueChart.render();
		}

		/*Function for DetailedBarGraph*/
		function fn_FusionChartDiscrepancies(OpenDiscrepanciesCount, DeferredDiscrepanciesCount, ClosedDiscrepanciesCount) {

			var openColor = "#1C7AC0";
			var deferredColor = "#D934D1";
			var closedColor = "#2c970d";

			// Parse and add color to the data points
			var openData = JSON.parse(OpenDiscrepanciesCount).map(function (dataPoint) {
				return { ...dataPoint, "color": openColor };
			});

			var deferredData = JSON.parse(DeferredDiscrepanciesCount).map(function (dataPoint) {
				return { ...dataPoint, "color": deferredColor };
			});

			var closedData = JSON.parse(ClosedDiscrepanciesCount).map(function (dataPoint) {
				return { ...dataPoint, "color": closedColor };
			});

			var revenueChart = new FusionCharts({
				"type": "MSColumn2D",
				"renderAt": "MonthwiseDetailedDiscrepancies",
				"width": "550",
				"height": "350",
				"dataFormat": "json",
				"dataSource": {
					"chart": {
						"caption": "Total Discrepancies per Month <br> " + " of " + $("#ddlAircraft :selected").text() + " Aircraft",
						"subCaption": "<br>" + $("#ddlYear :selected").text(),
						"xAxisName": "Month",
						"yAxisName": "Count",
						"exportEnabled": "1",
						"theme": "zune"
					},
					"categories": [{
						"category": [{
							"label": "Jan"
						}, {
							"label": "Feb"
						}, {
							"label": "Mar"
						}, {
							"label": "Apr"
						}, {
							"label": "May"
						}, {
							"label": "Jun"
						}, {
							"label": "Jul"
						}, {
							"label": "Aug"
						}, {
							"label": "Sep"
						}, {
							"label": "Oct"
						}, {
							"label": "Nov"
						}, {
							"label": "Dec"
						}]
					}],
					"dataset": [{
						"seriesname": "Open",
						"color": openColor, // Legend color
						"data": openData
					},
					{
						"seriesname": "Deferred",
						"color": deferredColor,
						"data": deferredData
					},
					{
						"seriesname": "Closed",
						"color": closedColor,
						"data": closedData
					}]
				}
			});
			revenueChart.render();
		}

		/*Function for PieGraph*/
		function fn_FusionChartPie(PieGraphValues) {
			var revenueChart = new FusionCharts({
				"type": "Pie3D",
				"renderAt": "AircraftwiseDiscrepancies",
				"width": "550",
				"height": "350",
				"dataFormat": "json",
				"dataSource": {
					"chart": {
						"caption": "Total Discrepancies per Year <br> (Of all Aircrafts)",
						"subCaption": "<br>" + $("#ddlYear :selected").text(),
						"startingAngle": "120",
						"showLabels": "0",
						"showLegend": "1",
						"enableMultiSlicing": "0",
						"slicingDistance": "15",
						"showPercentValues": "1",
						"showPercentInTooltip": "0",
						"exportEnabled": "1",
						"plotTooltext": "<b> Reg No : </b> $label <br> <b> Total Count : </b> $datavalue",
						"theme": "zune"
					},
					"data": JSON.parse(PieGraphValues)
				}
			});
			revenueChart.render();
		}

		/*Function for ATA DetailedBarGraph*/
		function fn_FusionChartDiscrepancies_ATA(ATAWiseOpenDiscrepanciesCount, ATAWiseDeferredDiscrepanciesCount) {

			if (!ATAWiseOpenDiscrepanciesCount || !ATAWiseDeferredDiscrepanciesCount) {
				console.error("Error: Invalid or missing data provided. Ensure 'ATAWiseOpenDiscrepanciesCount' and 'ATAWiseDeferredDiscrepanciesCount' contain valid JSON data with 'label' properties.");
				return;
			}

			const categories = JSON.parse(ATAWiseDeferredDiscrepanciesCount).map((item) => {
				if (!item.label) {
					console.warn("Warning: An item in 'ATAWiseDeferredDiscrepanciesCount' is missing or has an invalid 'label' property. Using a placeholder label.");
					return { error: 'Missing Label' };
				}
				return { label: item.label };
			});

			var openColor = "#1C7AC0";
			var deferredColor = "#D934D1";

			// Parse and add color to the data points
			var openData = JSON.parse(ATAWiseOpenDiscrepanciesCount).map(function (dataPoint) {
				return { ...dataPoint, "color": openColor };
			});

			var deferredData = JSON.parse(ATAWiseDeferredDiscrepanciesCount).map(function (dataPoint) {
				return { ...dataPoint, "color": deferredColor };
			});

			const dataSource = {
				chart: {
					caption: "Total Discrepancies per ATA <br> (Till Today) ",
					subCaption: "<br>" + $("#ddlYear :selected").text(),
					xAxisName: "ATA",
					yAxisName: "Count",
					exportEnabled: "1",
					theme: "zune	"
				},
				categories: [{ category: categories }],
				dataset: [
					{
						"seriesname": "Open",
						"color": openColor, // Legend color
						"data": openData
					},
					{
						"seriesname": "Deferred",
						"color": deferredColor,
						"data": deferredData
					}
				]
			};
			var revenueChart = new FusionCharts({
				"type": "MSColumn2D",
				"renderAt": "ATAWiseDiscrepancies",
				"width": "550",
				"height": "350",
				"dataFormat": "json",
				"dataSource": dataSource
			});
			revenueChart.render();

			//console.log("Values recieved in categories attribute:");
			//console.log(categories);
		}

		/*Function for Aircraftwise Detailed Bar Graph*/
		function fn_FusionChartDetailedDiscrepancies(OpenDiscrepanciesCount, DeferredDiscrepanciesCount, ClosedDiscrepanciesCount, AircraftList) {

			if (typeof AircraftList === 'string') {
				AircraftList = JSON.parse(AircraftList);
			}

			// Create an array of category objects based on AircraftList
			var categoriesArray = AircraftList.map(function (aircraft) {
				return { "label": aircraft.RegNo };
			});

			var openColor = "#1C7AC0";
			var deferredColor = "#D934D1";
			var closedColor = "#2c970d";

			// Parse and add color to the data points
			var openData = JSON.parse(OpenDiscrepanciesCount).map(function (dataPoint) {
				return { ...dataPoint, "color": openColor };
			});

			var deferredData = JSON.parse(DeferredDiscrepanciesCount).map(function (dataPoint) {
				return { ...dataPoint, "color": deferredColor };
			});

			var closedData = JSON.parse(ClosedDiscrepanciesCount).map(function (dataPoint) {
				return { ...dataPoint, "color": closedColor };
			});

			var revenueChart = new FusionCharts({
				"type": "MSColumn2D",
				"renderAt": "AircraftWiseDetailedDiscrepancies",
				"width": "1150",
				"height": "500",
				"dataFormat": "json",
				"dataSource": {
					"chart": {
						"caption": "Total Discrepancies per Aircraft <br> (for Current Year)",
						"subCaption": "<br>" + $("#ddlYear :selected").text(),
						"xAxisName": "Aircrafts",
						"yAxisName": "Count",
						"exportEnabled": "1",
						"theme": "zune"
					},
					"categories": [{
						"category": categoriesArray
					}],
					"dataset": [{
						"seriesname": "Open",
						"color": openColor, // Legend color
						"data": openData
					},
					{
						"seriesname": "Deferred",
						"color": deferredColor,
						"data": deferredData
					},
					{
						"seriesname": "Closed",
						"color": closedColor,
						"data": closedData
					}]
				}
			});
			revenueChart.render();
		}

	</script>

</head>
<body>

	<form id="frmDiscrepancyDashboard" runat="server">

		<%--Scripts For Calendar--%>
		<script src="bootstrap/bootstrap.min.js" type="text/javascript"></script>
		<script src="FullCalendar/moment.min.js" type="text/javascript"></script>
		<script src="FullCalendar/jquery.min.js" type="text/javascript"></script>
		<script src="FullCalendar/jquery-ui.min.js" type="text/javascript"></script>
		<script src="FullCalendar/fullcalendar.min.js" type="text/javascript"></script>

		<asp:ScriptManager AsyncPostBackTimeout="600" ID="smDiscrepancyDashboard" runat="server" EnablePageMethods="true">
		</asp:ScriptManager>
		<asp:UpdatePanel runat="server" ID="upnlDiscrepancyDashboard" UpdateMode="Conditional">
			<ContentTemplate>

				<div class="row" id="pageHeader">

					<asp:UpdatePanel ID="upnlDiscrepancyDashboardTitle" runat="server" UpdateMode="Conditional">
						<ContentTemplate>

							<span class="text-info">DASHBOARD</span>

						</ContentTemplate>
					</asp:UpdatePanel>

				</div>

				<br />
				<br />

				<%--Added by Harsh Sugandhi on 4th September 2024 for FLYPAL-1850 AOG Aircraft Details on Discrepancy Dashboard--%>
				<div id="statusCards" runat="server">
					
				</div>

				<br />
				<br />

				<div id="divDiscrepancyStatusCount">

					<asp:UpdatePanel ID="upnlDiscrepancyStatusCount" runat="server" UpdateMode="Conditional">
						<ContentTemplate>

							<div class="container">
								<div class="row">
									<div class="col-md-2 col-sm-6">
										<div class="counter blue">
											<span class="counter-value counter-value-blue">
												<%# DiscrepancyStatusCount.OpenDispcreancyCount %>
											</span>
											<div class="counter-content counter-content-blue">
												<h3 class="Counter-OpenDiscrepancies-Label">Open</h3>
											</div>
										</div>
									</div>
									<div class="col-md-2 col-sm-6">
										<div class="counter authorized">
											<span class="counter-value counter-value-pink">
												<%# DiscrepancyStatusCount.MELDeferralDispcreancyCount %>
											</span>
											<div class="counter-content counter-content-pink">
												<h3 class="Counter-MEL-Deferred-Label">MEL Deferred</h3>
											</div>
										</div>
									</div>
									<div class="col-md-2 col-sm-6">
										<div class="counter DarkPink">
											<span class="counter-value counter-value-darkPink">
												<%# DiscrepancyStatusCount.OtherDeferredDispcreancyCount %>
											</span>
											<div class="counter-content counter-content-darkPink">
												<h3 class="Counter-OtherDeferred-Label">Other Deferred</h3>
											</div>
										</div>
									</div>
									<div class="col-md-2 col-sm-6">
										<div class="counter red">
											<span class="counter-value counter-value-red">
												<%# DiscrepancyStatusCount.AOGDispcreancyCount %>
											</span>
											<div class="counter-content counter-content-red">
												<h3 class="Counter-AOG-Label">AOG</h3>
											</div>
										</div>
									</div>
									<div class="col-md-2 col-sm-6">
										<div class="counter green">
											<span class="counter-value counter-value-green">
												<%# DiscrepancyStatusCount.ClosedDispcreancyCount %>
											</span>
											<div class="counter-content counter-content-green">
												<h3 class="Counter-Closed-Label">Closed</h3>
											</div>
										</div>
									</div>
								</div>
							</div>

						</ContentTemplate>
					</asp:UpdatePanel>

				</div>

				<br />
				<br />

				<div id="divCalendar">

					<asp:UpdatePanel ID="upnlDiscrepancyCalendar" runat="server" UpdateMode="Conditional">
						<ContentTemplate>

							<div class="row">

								<div class="main-box infographic-box" style="background: whitesmoke;">

									<table width="100%" style="display: none">
									</table>
									<div style="height: 9px;">
									</div>
									<div style="display: inline-block;" id="calendar">
										<button id="prev-year" style="display: none">
											Prev year
										</button>
										<select id="months-tab" style="display: none">
											<option data-month="0">January</option>
											<option data-month="1">February</option>
											<option data-month="2">March</option>
											<option data-month="3">April</option>
											<option data-month="4">May</option>
											<option data-month="5">June</option>
											<option data-month="6">July</option>
											<option data-month="7">August</option>
											<option data-month="8">September</option>
											<option data-month="9">October</option>
											<option data-month="10">November</option>
											<option data-month="11">December</option>
										</select>
									</div>

									<div id="indexForCalender">
										<p>
											<asp:Label CssClass="clsTextBoxTagSearchSmall" 
												runat="server" ID="lblIndexOpen" BackColor="#1C7AC0" />

											<asp:Label CssClass="clsTextBoxTagSearch" runat="server" 
												ID="lblOpen" ForeColor="#1C7AC0" Text="Open" /> 

											&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
											<asp:Label class="clsTextBoxTagSearchSmall" 
												runat="server" ID="lblIndexOtherDeferred" BackColor="#D934D1" />

											<asp:Label CssClass="clsTextBoxTagSearch" runat="server" 
												ID="lblOtherDeferred" ForeColor="#D934D1" Text="Deferred" />
										</p>
									</div>

									<div id="successModal" aria-hidden="true" aria-labelledby="successModalLabel" 
										class="modal fade" role="dialog" tabindex="-1">
										<div class="modal-dialog" role="dialog">
											<div class="modal-content">
												<div class="modal-header">
													<button aria-label="Close" class="close" data-dismiss="modal" type="button">
														<span aria-hidden="true">×</span>
													</button>
													<h4 class="modal-title">
														<p></p>
														<h4></h4>
														<h4></h4>
														<h4></h4>
														<h4></h4>
														<h4></h4>
														<h4></h4>
													</h4>
												</div>
												<div class="modal-body primary">
													<span aria-hidden="true"></span>
													<p>
													</p>
												</div>
											</div>
										</div>
									</div>

								</div>

							</div>

						</ContentTemplate>
					</asp:UpdatePanel>

				</div>

				<br />
				<br />

				<div id="YearandAircraftSelection">

					<asp:UpdatePanel ID="upnlYearandAircraft" runat="server" UpdateMode="Conditional">
						<ContentTemplate>

							<table width="100%">
								<tr>
									<td id="yearMonthSelection" align="center">
										<asp:Label ID="lblYear" runat="server" CssClass="clsLabelAuto" Font-Bold="true">
											Year
										</asp:Label>
										<asp:DropDownList ID="ddlYear" runat="server" ClientIDMode="Static"
											CssClass="clsTextBoxTagSearchComboSmall" AutoPostBack="true"
											Width="100px" ToolTip="Select a Year." /> 
										<asp:DropDownList ID="ddlMonth" runat="server" ClientIDMode="Static"
											CssClass="clsTextBoxTagSearchComboSmall" AutoPostBack="true"
											Width="100px" ToolTip="Select a Month." />
									</td>
									<td id="aircraftSelection" align="center">
										<asp:Label ID="lblDashboardAircraft" runat="server" 
											CssClass="clsLabelAuto" Font-Bold="true" Text="Aircraft" /> 
										<asp:DropDownList ID="ddlAircraft" runat="server"
											CssClass="clsTextBoxTagSearchComboSmall clsDropDown" 
											DataTextField="RegNo" AutoPostBack="true" 
											DataValueField="ID" ToolTip="Select an Aircraft." />
									</td>
								</tr>
								<tr>
									<td>
										<br />
									</td>
								</tr>
							</table>

						</ContentTemplate>
					</asp:UpdatePanel>

				</div>

				<br />
				<br />

				<asp:UpdatePanel ID="upnlReports" runat="server" UpdateMode="Conditional">
					<ContentTemplate>

						<table id="Reports" width="100%">

							<tr id="MonthwiseDiscrepanciesSection">

								<td>

									<div class="row" id="divMonthwiseDiscrepancies">

										<div class="col-md-12 col-sm-6 col-xs-12">
											<asp:PlaceHolder ID="phMonthwiseDiscrepancies" runat="server" Visible="false">
												<fieldset id="fdsMonthwiseDiscrepancies">
													<asp:UpdatePanel ID="upnlMonthwiseDiscrepancies" runat="server" UpdateMode="Conditional">
														<ContentTemplate>
															<div id="MonthwiseDiscrepancies">
															</div>
														</ContentTemplate>
													</asp:UpdatePanel>
												</fieldset>
											</asp:PlaceHolder>
										</div>

									</div>

								</td>

								<td>

									<div class="row" id="divMonthwiseDetailedDiscrepancies">

										<div class="col-md-12 col-sm-6 col-xs-12">
											<asp:PlaceHolder ID="phMonthwiseDetailedDiscrepancies" runat="server" Visible="false">
												<fieldset id="fdsMonthwiseDetailedDiscrepancies">
													<asp:UpdatePanel ID="upnlMonthwiseDetailedDiscrepancies" runat="server" UpdateMode="Conditional">
														<ContentTemplate>
															<div id="MonthwiseDetailedDiscrepancies">
															</div>
														</ContentTemplate>
													</asp:UpdatePanel>
												</fieldset>
											</asp:PlaceHolder>
										</div>

									</div>

								</td>

							</tr>

							<tr id="AircraftandATAwiseDiscrepanciesSection">

								<td>

									<div class="row" id="divAircraftwiseDiscrepancies">

										<div class="col-md-12 col-sm-6 col-xs-12">
											<asp:PlaceHolder ID="phAircraftWiseDiscrepancies" runat="server" Visible="false">
												<fieldset id="fdsAircraftwiseDiscrepancies">
													<asp:UpdatePanel ID="upnlAircraftwiseDiscrepancies" runat="server" UpdateMode="Conditional">
														<ContentTemplate>
															<div id="AircraftwiseDiscrepancies">
															</div>
														</ContentTemplate>
													</asp:UpdatePanel>
												</fieldset>
											</asp:PlaceHolder>
										</div>

									</div>

								</td>

								<td colspan="2">

									<div class="row" id="divATAWiseDiscrepancies">

										<div class="col-md-12 col-sm-6 col-xs-12">
											<asp:PlaceHolder ID="phATAWiseDiscrepancies" runat="server" Visible="false">
												<fieldset id="fdsATAWiseDiscrepancies">
													<asp:UpdatePanel ID="upnlATAWiseDiscrepancies" runat="server" UpdateMode="Conditional">
														<ContentTemplate>
															<div id="ATAWiseDiscrepancies">
															</div>
														</ContentTemplate>
													</asp:UpdatePanel>
												</fieldset>
											</asp:PlaceHolder>
										</div>

									</div>

								</td>

							</tr>

							<tr id="AircraftWiseDiscrepanciesSection">

								<td colspan="2">

									<div class="row" id="divAircraftWiseDiscrepancies">

										<div class="col-md-12 col-sm-6 col-xs-12">
											<asp:PlaceHolder ID="phAircraftWiseDiscrepanciesDetails" runat="server" Visible="false">
												<fieldset id="fdsAircraftWiseDiscrepancies">
													<asp:UpdatePanel ID="upnlAircraftWiseDetailedDiscrepancies" runat="server" UpdateMode="Conditional">
														<ContentTemplate>
															<div id="AircraftWiseDetailedDiscrepancies">
															</div>
														</ContentTemplate>
													</asp:UpdatePanel>
												</fieldset>
											</asp:PlaceHolder>
										</div>

									</div>

								</td>

							</tr>

							<tr id="AircraftWiseDiscrepanciesTabularReportSection">

								<td colspan="2">

									<br />
									<br />

									<asp:UpdatePanel ID="upnlAircraftwiseDiscrepanciesTabularReport" runat="server" UpdateMode="Conditional">
										<ContentTemplate>

											<div class="closedDiscrepancyChxbox">

												<asp:Label ID="lblCheckboxText" runat="server" 
													Text="Show 'Closed' Discrepancies" /> 
												<asp:CheckBox runat="server" ID="ShowClosedDiscrepancy"
													CssClass="clsCheckBox"
													ToolTip='Check to show Closed Discrepancies.' 
													AutoPostBack="true" Visible="false" />
											</div>

											<div id="divAircraftWiseDiscrepancies">

												<asp:PlaceHolder ID="phAircraftWiseDiscrepanciesGV" runat="server" Visible="false">

													<asp:UpdatePanel ID="upnlAircraftWiseDiscrepanciesGridHeader"
														runat="server" UpdateMode="Conditional">
														<ContentTemplate>
															<asp:Label ID="lblGridText" runat="server" />
														</ContentTemplate>
													</asp:UpdatePanel>

													<asp:GridView ID="gvAircraftWiseDiscrepancies" runat="server" 
														DataKeyNames="ID" ShowHeaderWhenEmpty="True" EnableViewState="False" 
														AllowSorting="True" AllowPaging="True"
														AutoGenerateColumns="False" PageSize="10" CssClass="clsGridNewStyle" 
														GridLines="Horizontal" CellPadding="5" >
														<AlternatingRowStyle CssClass="clsdgAltItem" />
														<RowStyle CssClass="clsdgItem" Height="40px" />
														<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True"
															ForeColor="black" Height="50px" />
														<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
														<PagerSettings Mode="NumericFirstLast" 
															FirstPageText="First" LastPageText="Last" />
														<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black"
															HorizontalAlign="Right" Height="30px" />
														<Columns>
															<%--0--%>
															<asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
															<%--1--%>
															<asp:BoundField DataField="DefectNo" SortExpression="DefectReportNo"
																HeaderText="Discrepancy No">
																<HeaderStyle Width="120px" />
																<ItemStyle Wrap="False" Width="120px" />
															</asp:BoundField>
															<%--2--%>
															<asp:BoundField DataField="DateOfOccurenceFormatted" 
																HeaderText="Date Of Occurrence">
																<HeaderStyle Width="120px" />
																<ItemStyle Wrap="False" Width="120px" HorizontalAlign="Center" />
															</asp:BoundField>
															<%--3--%>
															<asp:BoundField DataField="LogNoPageNo" SortExpression="LogNo" 
																HeaderText="Log No."
																HtmlEncode="False">
																<HeaderStyle Width="120px" />
																<ItemStyle Wrap="False" Width="120px" />
															</asp:BoundField>
															<%--4--%>
															<asp:BoundField DataField="ATACodeSubATACode" 
																SortExpression="ATACodeSubATACode" HeaderText="ATA">
																<HeaderStyle Width="50px" />
																<ItemStyle Wrap="False" Width="50px" />
															</asp:BoundField>
															<%--5--%>
															<asp:BoundField DataField="MELOrCDLTag"
																SortExpression="MELOrCDLTag" HeaderText="Category"
																Visible="False">
																<ItemStyle Wrap="False" />
															</asp:BoundField>
															<%--6--%>
															<asp:BoundField DataField="Defect" SortExpression="Defect" 
																HeaderText="Discrepancy"
																HtmlEncode="False">
																<ItemStyle Wrap="True" Width="690px" />
															</asp:BoundField>
															<%--7--%>
															<asp:BoundField DataField="InvestigationStatusDiscrepancyText"
																SortExpression="InvestigationStatusDiscrepancyText" 
																HeaderText="Status">
																<ItemStyle Wrap="True" Width="60px" />
															</asp:BoundField>
															<%--8--%>
															<asp:BoundField DataField="NextDue" HeaderText="Due" 
																HtmlEncode="false" Visible="False">
																<ItemStyle Wrap="False" />
															</asp:BoundField>
															<%--9--%>
															<asp:BoundField DataField="RectifiedDateFormatted" 
																HeaderText="Close Date"
																Visible="False">
																<ItemStyle Wrap="False" />
															</asp:BoundField>
															<%--10--%>
															<asp:BoundField DataField="RectifiedLogText" 
																SortExpression="RectifiedLogText"
																HeaderText="Rectified Log No." 
																HtmlEncode="False" Visible="False">
																<ItemStyle Wrap="False" />
															</asp:BoundField>
															<%--11--%>
															<asp:BoundField DataField="PreventionTaken" 
																HeaderText="Watchlist Instruction" Visible="False">
																<ItemStyle Wrap="True" />
															</asp:BoundField>
															<%--12--%>
															<asp:BoundField DataField="IsAttachmentAdded" 
																HeaderText="IsAttachmentAdded"
																HeaderStyle-CssClass="hideGridColumn" 
																ItemStyle-CssClass="hideGridColumn" />
															<%--13--%>
															<asp:BoundField DataField="TotalTroubleShootCount" 
																HeaderText="TotalTroubleShootCount"
																HeaderStyle-CssClass="hideGridColumn" 
																ItemStyle-CssClass="hideGridColumn" />
														</Columns>

													</asp:GridView>

												</asp:PlaceHolder>

											</div>

										</ContentTemplate>
									</asp:UpdatePanel>

								</td>

							</tr>

						</table>

					</ContentTemplate>
				</asp:UpdatePanel>

			</ContentTemplate>
		</asp:UpdatePanel>

		<!-- Ajax Loader -->
		<div id="divSpinner">

			<asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" DynamicLayout="false" runat="server">
				<ProgressTemplate>
					<div class="clsAjaxLoader">
					</div>
					<div class="divAjaxLoader">
						<div class="ext-el-mask-msg x-mask-loading">
							<div class="clsLoad_ajax">
								<asp:Image ID="ajaxloadergif" runat="server" ImageUrl="~/images/Loader.gif"
									ImageAlign="Middle" CssClass="ajax-loader-gif" />
							</div>
						</div>
					</div>
				</ProgressTemplate>
			</asp:UpdateProgress>

		</div>

		<!-- Discrepancy Details -->
		<div id="Discrepancy_Details">

			<div style="display: none">
				<asp:Button runat="server" ID="btnDummyDiscrepancyDetail" Text="Discrepancy Deatils" ClientIDMode="Static" />
			</div>

			<asp:Panel runat="server" ID="pnlDiscrepancyDetail" HorizontalAlign="Center">
				<iframe id="IframeDiscrepancyDetail" frameborder="0" allowtransparency="true" height="100%" width="100%"
					src="JavaScript:''" scrolling="auto"></iframe>
			</asp:Panel>

			<cc2:ModalPopupExtender ID="mdlPopupDiscrepancyDetail" runat="server" TargetControlID="btnDummyDiscrepancyDetail"
				PopupControlID="pnlDiscrepancyDetail" BackgroundCssClass="clsModalPopupBG">
			</cc2:ModalPopupExtender>

		</div>

		<!-- Year & Month change, Iframe, callback functions for Calendar  -->
		<script type="text/javascript" id="CalendarMonthandYearChangeEvents">

			$('#months-tab').on('change', function () {
				var month = $(this).find(":selected").attr('data-month'),
					year = $("#calendar").fullCalendar('getDate').format('YYYY');
				var m = moment([year, month, 1]).format('YYYY-MM-DD');
				$('#calendar').fullCalendar('gotoDate', m);
			});

			$("#prev-year").on('click', function () {
				$('#calendar').fullCalendar('prevYear');
			});

			var month = $(this).find(":selected").attr('data-month') - 1;
			$("#months-tab").find('option[data-month=' + month + ']').prop('selected', true);

		</script>

		<script type="text/javascript" id="CalendarIframe&OpenPage">

			function IFrameDiscrepancyDetailComplete() {
				$("#btnDummyDiscrepancyDetail").click();
				$get("AjaxLoader").style.visibility = "hidden";
			}

			function ViewDiscrepancyDetail(MELSnagCorrectiveActionID) {
				try {

					$.ajax({
						url: "DiscrepancyDashboard.aspx/DiscrepancyDetails",
						data: "{ 'MELSnagCorrectiveActionID': '" + MELSnagCorrectiveActionID + "' }",
						type: "POST",
						cache: false,
						headers: { "cache-control": "no-cache" },
						contentType: "application/json; charset=utf-8",
						dataType: "json",
						success: function (msg) {
							if (msg.d == true) {
								$get("AjaxLoader").style.visibility = "visible";
								$("#IframeDiscrepancyDetail").attr("src", "wfDiscrepancyCorrectiveAction.aspx?Type=pup");
								$("#btnDummyDiscrepancyDetail").click();
								$get("AjaxLoader").style.visibility = "hidden";
							}
							else {
								$("#successModal").modal("show");
								$("#successModal .modal-body p").text('\n You are not authorized to view discrepancy details.');
								$("#successModal .modal-title p").html('Sorry');
							}
						},
						error: function (xhr, status, error) {
							console.error("Error Occured after call for WebMethod. Following is the Reponse Text : " + xhr.responseText);
							console.error("Error Occured after call for WebMethod. Following is the Error Text : " + error);
							console.error("Error Occured after call for WebMethod. Following is the Error status : " + status);
						}
					});
					return false;
				} catch (e) {
					console.error("Exception Occured in function ViewDiscrepancyDetail(). Following is the Exception : " + e);
				}
			}

		</script>

		<script type="text/javascript" id="CalendarCallback">

			function ParentCallBackFunctionForDiscrepancyDetail() {
				var DiscrepancyDetailwindow = $find("<%=mdlPopupDiscrepancyDetail.ClientID %>");
				DiscrepancyDetailwindow.hide();
				$("#IframeDiscrepancyDetail").attr("src", "JavaScript:''");
				$("#hdnBtnDiscrepancyDetail").click();
			}

		</script>

	</form>

	<script type="text/javascript" src="https://code.jquery.com/jquery-1.12.0.min.js"></script>
	<script type="text/javascript" src="js%20Bootstrap/demo-skin-changer.js"></script>
	<script type="text/javascript" src="js%20Bootstrap/jquery.js"></script>
	<script src="json2.js" type="text/javascript"></script>
	<script src="JQGridReq/jqueryui/1.8.23/jquery-ui.js" type="text/javascript"></script>
	<script src="JQGridReq/jquery/1.8.1/jquery.js" type="text/javascript"></script>
	<script src="JQGridReq/jquery-1.9.0.min.js" type="text/javascript"></script>
	<script src="JQGridReq/grid.locale-en.js" type="text/javascript"></script>
	<script src="JQGridReq/jquery.jqGrid.js" type="text/javascript"></script>
	<script type="text/javascript" src="js%20Bootstrap/bootstrap.js"></script>
	<script type="text/javascript" src="js%20Bootstrap/jquery.nanoscroller.min.js"></script>
	<script type="text/javascript" src="js%20Bootstrap/demo.js"></script>
	<script type="text/javascript" src="js%20Bootstrap/jquery-ui.custom.min.js"></script>
	<script type="text/javascript" src="js%20Bootstrap/fullcalendar.min.js"></script>
	<script type="text/javascript" src="js%20Bootstrap/jquery.slimscroll.min.js"></script>
	<script type="text/javascript" src="js%20Bootstrap/raphael-min.js"></script>
	<script type="text/javascript" src="js%20Bootstrap/morris.min.js"></script>
	<script type="text/javascript" src="js%20Bootstrap/moment.min.js"></script>
	<script type="text/javascript" src="js%20Bootstrap/daterangepicker.js"></script>
	<script type="text/javascript" src="js%20Bootstrap/jquery-jvectormap-1.2.2.min.js"></script>
	<script type="text/javascript" src="js%20Bootstrap/jquery-jvectormap-world-merc-en.js"></script>
	<script type="text/javascript" src="js%20Bootstrap/gdp-data.js"></script>
	<script type="text/javascript" src="js%20Bootstrap/flot/jquery.flot.js"></script>
	<script type="text/javascript" src="js%20Bootstrap/flot/jquery.flot.min.js"></script>
	<script type="text/javascript" src="js%20Bootstrap/flot/jquery.flot.pie.min.js"></script>
	<script type="text/javascript" src="js%20Bootstrap/flot/jquery.flot.stack.min.js"></script>
	<script type="text/javascript" src="js%20Bootstrap/flot/jquery.flot.resize.min.js"></script>
	<script type="text/javascript" src="js%20Bootstrap/flot/jquery.flot.time.min.js"></script>
	<script type="text/javascript" src="js%20Bootstrap/flot/jquery.flot.threshold.js"></script>
	<script type="text/javascript" src="js%20Bootstrap/jquery.countTo.js"></script>
	<script type="text/javascript" src="js%20Bootstrap/scripts.js"></script>
	<script type="text/javascript" src="js%20Bootstrap/pace.min.js"></script>

</body>

<%-- This script is to set the first HTML element at the top when page loads --%>
<script type="text/javascript" id="SetTopPostion">

	$(document).ready(function () {
		var calendarTop = $("#statusCards").offset().top;

		if (calendarTop < $(window).height()) {
			$(window).scrollTop(0);
		}
	});

</script>

</html>

