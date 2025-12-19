<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DashBoardWO.aspx.vb" Inherits="Flypal.DashBoardWO" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SI.UTILITY" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>

<%--Modified by Harsh on 13th May 2024 => FLYPAL 1630 -- Updating the themes of Chart, Graphs, etc.--%>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Dashboard</title>
	<meta content="False" name="vs_showGrid" />
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<script src="js/jquery-1.8.3.js" type="text/javascript"></script>
	<%-- FusionCharts --%>
	<script src="FusionCharts/fusioncharts.js" type="text/javascript"></script>
	<script src="FusionCharts/fusioncharts.charts.js" type="text/javascript"></script>
	<script src="FusionCharts/themes/fusioncharts.theme.zune.js" type="text/javascript"></script>
	<script src="VALIDATEFUNCTIONS.js" type="text/javascript"></script>
	<style type="text/css">
        .contentStickyNote {
            position: absolute;
            top: 40px;
            padding: 0px;
            margin: 0px;
            height: 300px;
            left: 300px;
        }

        span.cellWithoutBackground {
            display: block;
            background-color: red;
            margin-right: -2px;
            margin-left: -2px;
            height: 14px;
            padding: 4px;
            font-weight: bold;
        }
    </style>
	<%-- JQGrid --%>
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
	<link rel="stylesheet" href="fullcalendar/fullcalendar.print.css" type="text/css"
		media="print" />
	<script src="js/demo-rtl.js" type="text/javascript"></script>
	<link rel="stylesheet" type="text/css" href="css/libs/nanoscroller.css" />
	<link rel="stylesheet" type="text/css" href="css/libs/font-awesome.css" />
	<link rel="stylesheet" type="text/css" href="css/libs/ns-default.css" />
	<link rel="stylesheet" type="text/css" href="css/libs/ns-style-bar.css" />
	<link rel="stylesheet" type="text/css" href="css/libs/ns-style-attached.css" />
	<link rel="stylesheet" type="text/css" href="css/libs/ns-style-other.css" />
	<link rel="stylesheet" type="text/css" href="css/libs/ns-style-theme.css" />
	<link rel="stylesheet" href="css/libs/daterangepicker.css" type="text/css" />
	<link rel="stylesheet" type="text/css" href="css/libs/magnific-popup.css" />
	<link rel="stylesheet" href="/resources/demos/style.css" />
	<link href="Styles.css" id="MainStyle" type="text/css" rel="stylesheet" />
	<script type="text/javascript">
		if ("<%=Not HttpContext.Current.Session("StyleSheet") Is Nothing %>" == "True") {
			$("#MainStyle").attr('href',"<%= HttpContext.Current.Session("StyleSheet") %>");
		}
	</script>
	<link href="StickyNote/css/style.css" rel="stylesheet" type="text/css" />
	<style type="text/css">

        #upnlTitle,
        #upnlGridText{
            text-align: center;
            padding-block: 20px;
        }

        .row{
            margin-bottom: 10px;
        }

		.Counter-OpenWO-Label{
			position: absolute !important;
			top: 80px !important;
			left: 30px !important;
		}

		.Counter-AuthorizedWO-Label, 
		.Counter-CompletedWO-Label{
			position: absolute !important;
			top: 80px !important;
			left: 27px !important;
		}

		.Counter-CancelledWO-Label{
			position: absolute !important;
			top: 80px !important;
			left: 3px !important;
		}

        #lblGridText{
            font-size: 2rem !important;
			color: black !important;
        }

        #script-warning {
            display: none;
            background: #eee;
            border-bottom: 1px solid #ddd;
            padding: 0 10px;
            line-height: 40px;
            text-align: center;
            font-weight: bold;
            font-size: 12px;
            color: red;
        }

        #loading {
            display: none;
            position: absolute;
            top: 10px;
            right: 10px;
        }

        #calendar .dot-event {
            width: 0.1em;
            height: 0.1em;
            border-radius: 50%;
            display: inline-block;
            margin-left: 5px;
            vertical-align: text-bottom;
        }

        .modal-header {
            padding: 9px 15px;
            border-bottom: 1px solid #eee;
            background-color: #0480be;
            -webkit-border-top-left-radius: 5px;
            -webkit-border-top-right-radius: 5px;
            -moz-border-radius-topleft: 5px;
            -moz-border-radius-topright: 5px;
            border-top-left-radius: 5px;
            border-top-right-radius: 5px;
        }

        .modal-header.primary {
            color: #fff;
            background-color: #337ab7;
            border-color: #337ab7;
        }

    </style>

	<script type="text/javascript">
		function FusionChartPieFunc(PieGraphValues) {
			var revenueChart = new FusionCharts({
				"type": "Pie3D",
				"renderAt": "MyPieChart",
				//"width": "450",
				"width": "375",
				"height": "300",
				"dataFormat": "json",
				"dataSource": {
					"chart": {
						"caption": "Total Work Order(s) till Date",
						//"subCaption": $("#cmbYear :selected").text(),
						"startingAngle": "120",
						"showLabels": "0",
						"showLegend": "1",
						"enableMultiSlicing": "0",
						"slicingDistance": "15",
						//"showValues": "1",
						//To show the values in percentage
						//   "showPercentValues": "1",
						//  "showPercentInTooltip": "0",
						"exportEnabled": "1",
						"plotTooltext": "Status : $label<br>No of Work Orders : $datavalue",
						"theme": "zune"

					},
					"data": JSON.parse(PieGraphValues)

				}

			});
			revenueChart.render();
		}

	</script>
	<script type="text/javascript">
		function MonthlyWorkOrder(GraphWOPlannedListValues) {
			var revenueChart = new FusionCharts({
				"type": "Column2D",
				"renderAt": "MonthlyWorkOrderDiv",
				"width": "375",
				"height": "300",
				"dataFormat": "json",
				"dataSource": {
					"chart": {
						"caption": "Monthly Work Orders",
						"subCaption": $("#cmbYear :selected").text(),
						"xAxisName": "Month",
						"yAxisName": "No of Work Order(s)",
						"theme": "zune",
						"exportEnabled": "1"

					},
					"data": JSON.parse(GraphWOPlannedListValues)
				}
			});
			revenueChart.render();
		}
	</script>
	<script type="text/javascript">
		function MonthlyEmployeeWiseWorkDoneValues(GraphMonthlyEmployeeWiseWorkDoneValues) {
			var revenueChart = new FusionCharts({
				"type": "Line",
				"renderAt": "MonthlyEmployeeWiseWorkDoneValuesDiv",
				"width": "375",
				"height": "250",
				"dataFormat": "json",
				"dataSource": {
					"chart": {
						"caption": "Employee-wise Monthly Work Done <br>" + new Date().getFullYear(),
						"subCaption": $("#cmbYear :selected").text(),
						"xAxisName": "Month",
						"yAxisName": "No of Hour(s)",
						"theme": "zune",
						"exportEnabled": "1",
						//Attributes to configure scale                    
						"formatNumberScale": "1",
						//Set scale to 60 (60 seconds: 1 minute)
						"numberScaleValue": "60",
						//Set the scale unit to minutes
						"numberScaleUnit": " hrs",
						//Since all data is provided in seconds, default scale is seconds
						//  "defaultNumberScale": " secs"
					},
					"data": JSON.parse(GraphMonthlyEmployeeWiseWorkDoneValues)
				}
			});
			revenueChart.render();
		}
	</script>
	<script src="jquery.tooltip.min.js" type="text/javascript"></script>
	<script type="text/javascript">
		function FullCalendarDueFunc() {
			//  var $jq = jQuery.noConflict();

			$('#calendar').fullCalendar({
				//   schedulerLicenseKey: 'GPL-My-Project-Is-Open-Source',

				header: {
					left: 'prev,next today',
					center: 'title',
					right: ''
					//   right: 'month,agendaWeek,agendaDay'
				},

				defaultView: 'month',
				defaultDate: new Date(),
				editable: true,
				navLinks: false, // can click day/week names to navigate views
				height: 350,
				selectable: true,
				//  selectHelper: true,
				//slotMinutes: 15,
				allDayDefault: false,
				buttonText: {
					today: 'today',
					month: 'month',
					week: 'week',
					day: 'day'
				},

				events: function (start, end, timezone, callback) {
					WOStatusID = document.getElementById("hdnStatus").value;
					CustomerID = document.getElementById("hdnCustomer").value;
					var date = new Date($('#calendar').fullCalendar('getDate'));
					var month_int = date.getMonth();
					var year_int = date.getFullYear();

					$.ajax({
						type: "POST",
						//  data: "{ 'WOStatusID': '" + WOStatusID + "'}",
						//  data: "{ 'WOStatusID': '" + WOStatusID + "', 'CustomerID': '" + CustomerID + "' }",
						data: "{ 'WOStatusID': '" + WOStatusID + "', 'CustomerID': '" + CustomerID + "', 'month': '" + month_int + "', 'year': '" + year_int + "' }",

						url: "DashBoardWO.aspx/TestOnWebService",
						dataType: 'json',
						contentType: "application/json",

						success: function (data) {
							var events = [];
							var obj = jQuery.parseJSON(data.d);
							$(obj).each(function () {
								//var nowdate = new Date($(this).attr('start')).toDateString("yyyy-MM-dd");
								events.push({
									title: $(this).attr('title'),
									start: $(this).attr('start'), // will be parsed
									WOStatus: $(this).attr('WOStatus'),
									WOID: $(this).attr('id'),
									WOStatusid: $(this).attr('WOStatusid'),
									DescriptionCalender: $(this).attr('DescriptionCalender'),
									CustomerName: $(this).attr('CustomerName'),
									IsBillingRequiredStatus: $(this).attr('IsBillingRequiredStatus'),
									IsCAMOUpdatedStatus: $(this).attr('IsCAMOUpdatedStatus'),
									IsQCStatusApprovedStatus: $(this).attr('IsQCStatusApprovedStatus'),
									IsQCStatusApproved: $(this).attr('IsQCStatusApproved'),
									BillingRequired: $(this).attr('BillingRequired'),
									StatusId: $(this).attr('StatusId'),
									className: ["user_block", "bday_block"],
									tooltip: $(this).attr('WOStatus'),
									// color: '#BEEABE',
									allday: true,
									// eventLimit: 6
								});
							});
							callback(events);
						},
						error: function (xhr, status, error) {

							alert(xhr.responseText + "i am in error");

						}

					});


				}, //events ends
				displayEventTime: false,
				eventRender: function (event, element) {
					$(element).tooltip({
						title: event.DescriptionCalender,
						placement: "top",
						trigger: "hover",
						container: "body",
						html: true,
						animation: true

					});
					// element.attr('title', event.tooltip);
					element.css("font-size", "0.7em");
					element.css("color", "white");
					element.find('.fc-title').html(event.title);
					element.css("padding", "5px");
					//                     element.css('background-color', '#FF0000');
					//                        if (event.WOStatusid == "1") {
					//                            element.css('background-color', '#d934d1'); //pink : Open
					//                        }
					//                        else if (event.WOStatusid == "3") {
					//                            element.css('background-color', '#014501'); //dark green : Complete
					//                        }
					//                        else if (event.WOStatusid == "4") {
					//                            element.css('background-color', '#ccc62b'); //yellow : Planned
					//                        }
					//                        else if (event.WOStatusid == "5") {
					//                            element.css('background-color', '#21c416'); //Light Green : QC Approved
					//                        }
					//                        else if (event.WOStatusid == "6") {
					//                            element.css('background-color', '#FF0000'); //red : QC Rejected
					//                        }
					//                        else if (event.WOStatusid == "8") {
					//                            element.css('background-color', '#662bcc'); //Purple :  CAMO Updated
					//                            // element.css('background-color', '#008000');

					//                        }
					if (event.IsBillingRequiredStatus == "" && event.IsCAMOUpdatedStatus == "" && event.IsQCStatusApprovedStatus == "") {
						if (event.WOStatusid == "1") {

							if (event.WOStatus == "Authorized") {
								element.css('background-color', '#d934d1'); //pink : Authorized
							}
							else {
								element.css('background-color', '#7d5c7c'); //light pink : Open
							}
						}
						else if (event.WOStatusid == "3") {
							element.css('background-color', '#014501'); //dark green : PPC Complete
						}
						else if (event.WOStatusid == "4") {
							element.css('background-color', '#ccc62b'); //yellow : Planned
						}
						else if (event.WOStatusid == "7") {
							element.css('background-color', '#21c416'); //dark green : AME Complete
						}
					}
					else {
						if (event.IsQCStatusApprovedStatus == "QC Rejected") {
							element.css('background-color', '#f20707'); //red : QC Rejected                                
						}
						else if (event.BillingRequired == "2") {
							element.css('background-color', '#05f7c3'); //Billing Not Required
						}
						else {
							element.css('background-color', '#662bcc'); //Purple :  QC Approved,CAMO Updated or Billing done
						}

					}

				}, //eventRender ends
				eventClick: function (event, jsEvent, view) {
					OpenToAddWODetail(event.WOID);
					//                            $("#successModal").modal("show");
					//                            // $("#successModal .modal-body p").text(' \n<h3>Reg No. :</h3> ' + event.RegNo + ' \nStatus: ' + event.WOStatus);
					//                            $("#successModal .modal-body p").html(event.DescriptionCalender);
					//                            $("#successModal .modal-title p").html(event.title);


					return false;
				}

			});
			//calendar ends
			//            $('#cmbStatus').on('change', function () {
			//                //  console.log("Event");
			//                alert('abc');
			//               // $('#calendar').fullCalendar('rerenderEvents');
			//            });

		}
	</script>
</head>
<body>
	<form id="form1" runat="server">
		<script src="bootstrap/bootstrap.min.js" type="text/javascript"></script>
		<script src="FullCalendar/moment.min.js" type="text/javascript"></script>
		<script src="FullCalendar/jquery.min.js" type="text/javascript"></script>
		<script src="FullCalendar/jquery-ui.min.js" type="text/javascript"></script>
		<script src="FullCalendar/fullcalendar.min.js" type="text/javascript"></script>
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
			EnablePageMethods="true">
		</asp:ScriptManager>
		<asp:UpdatePanel runat="server" ID="upnlWO" UpdateMode="Conditional">
			<ContentTemplate>
				<div class="row">
					<div>
						<asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
							<ContentTemplate>
								<h1>
									<span style="font-size: 22px; font-weight: bold" class="text-info">WORK ORDER &nbsp;&nbsp; DASHBOARD
									</span>
								</h1>
							</ContentTemplate>
						</asp:UpdatePanel>
					</div>
				</div>
				<div class="container">
					<div class="row">
						<div class="col-md-3 col-sm-6">
							<div class="counter open">
								<span class="counter-value"><%# mnWOStatusCountDashboard.OpenCnt %></span>
								<div class="counter-content">
									<h3 class="Counter-OpenWO-Label">Open WO's</h3>
								</div>
							</div>
						</div>
						<div class="col-md-3 col-sm-6">
							<div class="counter authorized">
								<span class="counter-value"><%# mnWOStatusCountDashboard.AuthorizedCnt %></span>
								<div class="counter-content">
									<h3 class="Counter-AuthorizedWO-Label">Authorized</h3>
								</div>
							</div>
						</div>
						<div class="col-md-3 col-sm-6">
							<div class="counter green">
								<span class="counter-value"><%# mnWOStatusCountDashboard.CompletionCnt %></span>
								<div class="counter-content">
									<h3 class="Counter-CompletedWO-Label">Completion</h3>
								</div>
							</div>
						</div>
						<div class="col-md-3 col-sm-6">
							<div class="counter red">
								<span class="counter-value"><%# mnWOStatusCountDashboard.CancelCnt %></span>
								<div class="counter-content">
									<h3 class="Counter-CancelledWO-Label">Cancelled WO's</h3>
								</div>
							</div>
						</div>
					</div>
				</div>
				<div class="row">
					<div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
						<div class="col-md-12 col-sm-6 col-xs-12">
							<div class="main-box infographic-box" style="background: whitesmoke;">
								<table width="100%" style="display: none">
								</table>
								<div style="height: 9px;">
								</div>
								<div style="display: inline-block;" id="calendar">
									<button id="prev-year" style="display: none">
										Prev year</button>
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
								<p>
									<div>
										<asp:Label class="clsTextBoxSmall_Ajax" runat="server" ID="Label4" BackColor="#7d5c7c"
											ForeColor="#7d5c7c">OPEN</asp:Label>
										<span class="clsTextBox_Ajax">OPEN</span>
										<asp:Label class="clsTextBoxSmall_Ajax" runat="server" ID="lbl1" BackColor="#d934d1"
											ForeColor="#d934d1">OPEN</asp:Label>
										<span class="clsTextBox_Ajax">Authorized</span>
										<asp:Label class="clsTextBoxSmall_Ajax" runat="server" ID="Label7" BackColor="#014501"
											ForeColor="014501">OPEN</asp:Label>
										<span class="clsTextBox_Ajax">Completed</span>
										<asp:Label class="clsTextBoxSmall_Ajax" runat="server" ID="Label2" BackColor="#ccc62b"
											ForeColor="#ccc62b">OPEN</asp:Label>
										<span class="clsTextBox_Ajax">Cancelled</span>
									</div>

									<p>
									</p>
									<%--Bootstrap Modal POPUP--%>
									<div id="successModal" aria-hidden="true" aria-labelledby="successModalLabel" class="modal fade"
										role="dialog" tabindex="-1">
										<div class="modal-dialog" role="dialog">
											<div class="modal-content">
												<div class="modal-header">
													<button aria-label="Close" class="close" data-dismiss="modal" type="button">
														<span aria-hidden="true">×</span>
													</button>
													<h4 class="modal-title">
														<p>
														</p>
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
												<%-- <div class="modal-footer">
                            <button type="button" class="btn btn-info" data-dismiss="modal">
                                Plan</button>
                        </div>--%>
											</div>
											<!-- /.modal-content -->
										</div>
										<!-- /.modal-dialog -->
									</div>
									<!-- /.modal -->
									<p>
									</p>
									<p>
									</p>
									<p>
									</p>
									<p>
									</p>
									<p>
									</p>
									<p>
									</p>
									<p>
									</p>
									<p>
									</p>
									<p>
									</p>
									<p>
									</p>
									<p>
									</p>
									<p>
									</p>
									<p>
									</p>
									<p>
									</p>
									<p>
									</p>
									<p>
									</p>
									<p>
									</p>
									<p>
									</p>
									<p>
									</p>
									<p>
									</p>
									<p>
									</p>
									<p>
									</p>
									<p>
									</p>
									<p>
									</p>
									<p>
									</p>
									<p>
									</p>
									<p>
									</p>
									<p>
									</p>
									<p>
									</p>
									<p>
									</p>
									<p>
									</p>
									<p>
									</p>
								</p>
							</div>
						</div>
					</div>
				</div>
				<div class="row">
					<div class="col-md-4 col-sm-6 col-xs-12">
						<div class="main-box small-graph-box red-bg">
							<asp:Label class="value" runat="server" ID="lblWOCount" ForeColor="White"></asp:Label>
							<span class="headline">Work Order(s)</span>
							<div class="progress">
								<div style="width: 60%;" aria-valuemax="100" aria-valuemin="0" aria-valuenow="60"
									role="progressbar" class="progress-bar">
								</div>
							</div>
							<i id="icIconWOIncrease" runat="server" class="glyphicon glyphicon-upload"></i><i
								id="icIconWODecrease" runat="server" class="glyphicon glyphicon-download"></i>
							<span class="subinfo" runat="server" id="spnWOSubInfo"></span>
						</div>
					</div>
					<div class="col-md-4 col-sm-6 col-xs-12">
						<div class="main-box small-graph-box yellow-bg">
							<asp:Label class="value" runat="server" ID="lblRequisitionCount" ForeColor="White"></asp:Label>
							<span class="headline">Spares Requisition(s)</span>
							<div class="progress">
								<div style="width: 60%;" aria-valuemax="100" aria-valuemin="0" aria-valuenow="60"
									role="progressbar" class="progress-bar">
								</div>
							</div>
							<i id="icIconReqIncrease" runat="server" class="glyphicon glyphicon-upload"></i>
							<i id="icIconReqDecrease" runat="server" class="glyphicon glyphicon-download"></i>
							<span class="subinfo" runat="server" id="spnReqSubInfo"></span>
						</div>
					</div>
					<div class="col-md-4 col-sm-6 col-xs-12 hidden-sm">
						<div class="main-box small-graph-box green-bg">
							<asp:Label class="value" runat="server" ID="lblIssueCount" ForeColor="White"></asp:Label>
							<span class="headline">Issued Spares Against Requisition(s)</span>
							<div class="progress">
								<div style="width: 60%;" aria-valuemax="100" aria-valuemin="0" aria-valuenow="60"
									role="progressbar" class="progress-bar">
									<span class="sr-only">42% Complete</span>
								</div>
							</div>
							<i id="icIconIssueIncrease" runat="server" class="glyphicon glyphicon-upload"></i>
							<i id="icIconIssueDecrease" runat="server" class="glyphicon glyphicon-download"></i><span class="subinfo" runat="server" id="spnIssueSubInfo"></span>
						</div>
					</div>
				</div>
				<div class="row">
					<div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
						<div class="col-md-4 col-sm-6 col-xs-12">
							<asp:Panel ID="pnlPie" runat="server">
								<fieldset id="fdsMyPieChart" style="border-width: 1px;">
									<asp:UpdatePanel ID="upnlMyPieChart" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<div id="MyPieChart">
											</div>
										</ContentTemplate>
									</asp:UpdatePanel>
								</fieldset>
							</asp:Panel>
						</div>
						<div class="col-md-4 col-sm-6 col-xs-12">
							<asp:Panel ID="pnlMonthlyWorkOrder" runat="server">
								<fieldset id="fdsMonthlyWorkOrder" style="border-width: 1px">
									<asp:UpdatePanel ID="upnlMonthlyWorkOrder" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<div id="MonthlyWorkOrderDiv">
											</div>
										</ContentTemplate>
									</asp:UpdatePanel>
								</fieldset>
							</asp:Panel>
						</div>
						<div class="col-md-4 col-sm-6 col-xs-12">
							<asp:Panel ID="Panel1" runat="server">
								<fieldset id="Fieldset2" style="border-width: 1px">
									<asp:UpdatePanel ID="upnlMonthlyEmployeeWiseWorkDoneValues" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<h1 class="center">
												<div>
													<asp:Label ID="lblEmployee" runat="server" CssClass="clsLabelHeader">Employee</asp:Label>
												</div>
												<div>
													<asp:DropDownList ID="cmbEmployee" runat="server" CssClass="clsTextBoxTagSearchComboSmall" DataValueField="ID"
														AutoPostBack="true" DataTextField="EmpNoName">
													</asp:DropDownList>
												</div>
											</h1>
											<div id="MonthlyEmployeeWiseWorkDoneValuesDiv">
											</div>
										</ContentTemplate>
									</asp:UpdatePanel>
								</fieldset>
							</asp:Panel>
						</div>
					</div>
				</div>
				<div class="row">
					<div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
						<div class="col-md-12 col-sm-6 col-xs-12">
							<div class="row'">
								<div class="col-md-12 col-sm-6 col-xs-12">
									<asp:Panel ID="pnlRequisitionItemStatus" runat="server">
										<%--<fieldset id="Fieldset1" class="clsFieldSet" style="border-width: 1px;">
                                            <legend id="ldwodetail" class="clsFieldSet1" runat="server"><b>Spares Requisition List</b></legend>--%>

										<asp:UpdatePanel ID="upnlGridText" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:Label ID="lblGridText" runat="server" CssClass="clsLabelHeader" Font-Bold="true">
                                                        Spares Requisition List
												</asp:Label>
											</ContentTemplate>
										</asp:UpdatePanel>

										<asp:UpdatePanel ID="upnlRequisitionItemStatus" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:UpdatePanel ID="upnlRequisitionCriteria" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<div class="row">
															<div class="col-xs-12 col-sm-12 col-md-12 col-lg-12">
																<div class="col-md-6 col-sm-6 col-xs-12">
																	<span id="Span1" class="control-label">Show "NOT ISSUED" Items</span>
																	<asp:CheckBox ID="chkNotIssuedItems" runat="server" CssClass="clsCheckBox" ToolTip='Check to get Not Issued Items'
																		AutoPostBack="true" TextAlign="Left" Text=""></asp:CheckBox>
																</div>
																<div class="col-md-6 col-sm-6 col-xs-12">
																	<span id="lbl12" class="control-label">Show "NOT RECEIVED" Items</span>
																	<asp:CheckBox ID="chkNotReceivedItems" runat="server" CssClass="clsCheckBox" ToolTip='Check to get Not Received Items'
																		AutoPostBack="true" TextAlign="Left" Text=""></asp:CheckBox>
																</div>
															</div>
														</div>
													</ContentTemplate>
												</asp:UpdatePanel>
												<table id="JQGridRequisitionItem" cellspacing="0" cellpadding="0" border="0">
												</table>
												<div id="jqGridPager">
												</div>
											</ContentTemplate>
										</asp:UpdatePanel>
										<%--</fieldset>--%>
									</asp:Panel>
								</div>
							</div>
						</div>
					</div>
				</div>
			</ContentTemplate>
		</asp:UpdatePanel>
		<script type="text/javascript">
			// change month
			$('#months-tab').on('change', function () {
				// get month from the tab. Get the year from the current fullcalendar date
				var month = $(this).find(":selected").attr('data-month'),
					year = $("#calendar").fullCalendar('getDate').format('YYYY');

				var m = moment([year, month, 1]).format('YYYY-MM-DD');

				$('#calendar').fullCalendar('gotoDate', m);
			});

			// go to prev year
			$("#prev-year").on('click', function () {
				//  alert('ss');
				$('#calendar').fullCalendar('prevYear');

			});

			// set the month as the current month
			// has to be -1 because this is 0 based
			var month = $(this).find(":selected").attr('data-month') - 1;  //$("#calendar").fullCalendar('getDate').format('MM') - 1;
			// set the correct month selected
			$("#months-tab").find('option[data-month=' + month + ']').prop('selected', true);

		</script>
		<script type="text/javascript">
			Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
				FuncRequisitionDet('false', 'false');
			});
		</script>
		<script type="text/javascript">
			function FuncRequisitionDet(mNotIssued, mNotReceived) {
				$('#JQGridRequisitionItem').jqGrid({
					url: 'DashBoardWO.aspx/RequisitionItemStatusList',
					datatype: "json",
					mtype: 'POST',
					postData:
					{
						NotIssued: mNotIssued,
						NotReceived: mNotReceived
					},
					serializeGridData: function (postData) {
						return JSON.stringify(postData);
					},

					ajaxGridOptions: { contentType: "application/json" },
					loadonce: true,
					colNames: ['Requisition No.', 'Date', 'WO No', 'Req. Personnel', 'P/N', 'Description', 'QTY.', 'PO Details', 'GRN/GRO/DATE', 'Issue Details', 'Remarks'],
					colModel: [
						{ name: 'RequisitionTextNo', index: 'RequisitionTextNo', width: 150 },
						{ name: 'DateFormatted', index: 'DateFormatted', width: 160 },
						{ name: 'WONo', index: 'WONo', width: 150 },
						{ name: 'EmployeeName', index: 'EmployeeName', width: 290 },
						{ name: 'PartNo', index: 'PartNo', width: 300 },
						{ name: 'PartDescription', index: 'PartDescription', width: 580 },
						{ name: 'ReqQty', index: 'ReqQty', width: 90 },
						{ name: 'OrderDetails', index: 'OrderDetails', width: 70 },
						{ name: 'ReceiptDetails', index: 'ReceiptDetails', width: 170 },
						{ name: 'IssueDetails', index: 'IssueDetails', width: 470 },
						{ name: 'Remark', index: 'Remark', width: 400 }
					],

					viewrecords: true, // show the current page, data rang and total records on the toolbar
					// width: 1000,
					autowidth: true,
					height: 250,
					rowNum: 10,
					loadonce: true, // this is just for the demo
					pager: "#jqGridPager",
					jsonReader: {
						page: function (obj) { return 1; },
						total: function (obj) { return 1; },
						records: function (obj) { return obj.d.length; },
						root: function (obj) { return obj.d; },
						repeatitems: false,
						id: "0"
					},
					caption: "Spares Requisition Status",
					grouping: false
					//                groupingView: {
					//                    groupField: ["RequisitionTextNo"],
					//                    groupColumnShow: [false],
					//                    groupDataSorted: true,
					//                    groupOrder: ["asc"],
					//                    groupSummary: [false],
					//                    groupSummaryPos: ['header'],
					//                    groupText: ['<b>{0}</b>'],
					//                    groupCollapse: false,
					//                    minusicon: 'fa-plus'
					//   }
				});

			}

		</script>
		<%--AJAX- Add UpdateProgress to show loading for Longer Process--%>
		<asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
			<ProgressTemplate>
				<div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed; background-color: #000000; top: 0; z-index: 99999;">
				</div>
				<div style="position: fixed; top: 50%; left: 50%; margin-left: -27px; margin-top: -27px; z-index: 100000;">
					<div class="ext-el-mask-msg x-mask-loading">
						<div class="clsLoad_ajax">
							<asp:Image ID="Image1" runat="server" ImageUrl="~/images/Loader.gif" ImageAlign="Middle"
								Height="48px" Width="48px" />
						</div>
					</div>
				</div>
			</ProgressTemplate>
		</asp:UpdateProgress>
		<script type="text/javascript">
			function OpenGreetingsWindow() {
				window.open("wfGreetings.aspx", "Open", "top=30,left=200,width=960,height=690,toolbar=no,menubar=no,location=no,toolbar=no");
				return true;
			}

		</script>
		<!-- WO Detail Popup Window Added By Prashant 16-Aug-2019-->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyWODetail" Text="Dummy WODetail" ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlPopupWODetail" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="iPopupWODetail" frameborder="0" allowtransparency="true" height="100%"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupWODetail" runat="server" TargetControlID="btnDummyWODetail"
			PopupControlID="pnlPopupWODetail" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
            function IFrameWODetailStateComplete() {
                $("#btnDummyWODetail").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }
            function OpenToAddWODetail(mWOID) {
                try {

                    $.ajax({
                        url: "DashBoardWO.aspx/GetWODet",
                        data: "{ 'WOID': '" + mWOID + "' }",
                        type: "POST",
                        cache: false,
                        headers: { "cache-control": "no-cache" },
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            if (msg.d == true) {
                                $get("AjaxLoader").style.visibility = "visible";
                                $("#iPopupWODetail").attr("src", "wfnWODetail_AJAX.aspx?Type=pup");
                                // if (!$.browser.msie) {
                                $("#btnDummyWODetail").click();
                                $get("AjaxLoader").style.visibility = "hidden";
                                //    }
                            }
                            // Do Something
                            else {
                                // return false;
                                $("#successModal").modal("show");
                                $("#successModal .modal-body p").text('\nYou are not Authorised to view this Work Order!');
                                //$("#successModal .modal-body p").html(event.DescriptionCalender);
                                $("#successModal .modal-title p").html('OOPS!');
                            }
                        },
                        error: function (xhr, status, error) {
                            //DebugAlert("Error: " + xhr.responseText);
                        }
                    });



                    return false;
                } catch (e) {
                    alert(e);
                }
            }
		</script>
		<script type="text/javascript">
            function ParentCallBackFunctionForWODetail() {
                var WODetailWindow = $find("<%=mdlPopupWODetail.ClientID %>");
                //close WODetail popup window
                WODetailWindow.hide();
                $("#iPopupWODetail").attr("src", "JavaScript:''");
                //call ata image button
                $("#hdnBtnAddWODetail").click();
            }
		</script>
		<!-- End-->
		<asp:HiddenField ID="hdnStatus" runat="server" />
		<asp:HiddenField ID="hdnCustomer" runat="server" />


	</form>
	<script type="text/javascript" src="https://code.jquery.com/jquery-1.12.0.min.js"></script>
	<script>
        $(document).ready(function () {
            //  pqr();
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
	<script type="text/javascript">
		//        $("#cmbStatus").change(function () {
		//            //  $('#calendar').fullCalendar('rerenderEvents');
		//            document.getElementById("hdnStatus").value = $("#cmbStatus").val();
		//            //  $('#calendar').fullCalendar('refetchEvents');
		//            FullCalendarDueFunc();

		//            $('#calendar').fullCalendar('refetchEvents');
		//            //  alert('abc');
		//        });
		//        $("#cmbCustomerList").change(function () {
		//            //  $('#calendar').fullCalendar('rerenderEvents');
		//            document.getElementById("hdnCustomer").value = $("#cmbCustomerList").val();
		//            //  $('#calendar').fullCalendar('refetchEvents');
		//            FullCalendarDueFunc();

		//            $('#calendar').fullCalendar('refetchEvents');
		//            //  alert('abc');
		//        });
	</script>
	<script type="text/javascript" src="js%20Bootstrap/demo-skin-changer.js"></script>
	<script type="text/javascript" src="js%20Bootstrap/jquery.js"></script>
	<script src="json2.js" type="text/javascript"></script>
	<script src="JQGridReq/jqueryui/1.8.23/jquery-ui.js" type="text/javascript"></script>
	<script src="JQGridReq/jquery/1.8.1/jquery.js" type="text/javascript"></script>
	<script src="JQGridReq/jquery-1.9.0.min.js" type="text/javascript"></script>
	<script src="JQGridReq/grid.locale-en.js" type="text/javascript"></script>
	<script src="JQGridReq/jquery.jqGrid.js" type="text/javascript"></script>
	<script src="JQGridReq/jquery.ui.selectable.js" type="text/javascript"></script>
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
</html>
