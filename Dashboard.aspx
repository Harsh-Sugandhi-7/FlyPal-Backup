<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Dashboard.aspx.vb" Inherits="Flypal.Dashboard" %>

<%@ Import Namespace="SI.UTILITY" %>
<%@ Import Namespace="Flypal.TransactionwisePendingOrders" %>
<%@ Import Namespace="Flypal.rptExpiredItemsCount" %>
<%@ Import Namespace="Flypal.RootCauseCount" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>

<%--Modified by Harsh on 13th May 2024 => FLYPAL 1630 -- Updating the themes of Chart, Graphs, etc.--%>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Dashboard</title>
	<meta content="False" name="vs_showGrid" />
	<meta http-equiv="X-UA-Compatible" content="IE=edge,7,8,9" />
	<meta charset="utf-8" content="" />
	<meta name="viewport" content="width=device-width, initial-scale=1" />
	<link href="bootstrap/bootstrap.min.css" rel="stylesheet" type="text/css" />
	<%--<link rel="stylesheet" href="css/classic.css">--%>
	<script src="json2.js" type="text/javascript"></script>
	<script src="JQGridReq/jquery/1.8.1/jquery.js" type="text/javascript"></script>
	<script src="JQGridReq/jqueryui/1.8.23/jquery-ui.js" type="text/javascript"></script>
	<script src="JQGridReq/jquery.ui.selectable.js" type="text/javascript"></script>
	<script src="JQGridReq/jquery-1.9.0.min.js" type="text/javascript"></script>
	<script src="JQGridReq/jquery.jqGrid.js" type="text/javascript"></script>
	<script src="JQGridReq/grid.locale-en.js" type="text/javascript"></script>
	<%--<script src="JQGridReq/jquery.ui.mouse.js" type="text/javascript"></script>--%>
	<link href="JQGridReq/jqueryui/1.8.23/jquery-ui.css" rel="stylesheet" type="text/css" />
	<link href="JQGridReq/Site.css" rel="stylesheet" type="text/css" />
	<link href="JQGridReq/ui.jqgrid.css" rel="stylesheet" type="text/css" />
	<link href="JQGridReq/jquery-ui-1.9.2.custom.css" rel="stylesheet" type="text/css" />
	<script type="text/javascript" src="jquery-1.6.1.min.js"></script>
	<%-- End JQGrid --%>
	<%-- FusionCharts --%>
	<script src="FusionCharts/fusioncharts.js" type="text/javascript"></script>
	<script src="FusionCharts/fusioncharts.charts.js" type="text/javascript"></script>
	<script src="FusionCharts/themes/fusioncharts.theme.zune.js" type="text/javascript"></script>
	<%--End FusionCharts --%>
	<link rel="stylesheet" href="/resources/demos/style.css" />
	<script src="VALIDATEFUNCTIONS.js" type="text/javascript"></script>
	<script id="clientEventHandlersJS" type="text/javascript">
		function openTranDetail() {
			str = "wfReports.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openTranDetail1() {
			str = "webform1.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openDetail() {
			str = "wfDetail.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
	</script>
	<%--Mic Testing 123--%>
	<link href="images/favicon.ico" rel="shortcut icon" type="image/x-icon" />
	<link href="Styles.css" id="MainStyle" type="text/css" rel="stylesheet" />
	<script type="text/javascript">
		if ("<%=Not HttpContext.Current.Session("StyleSheet") Is Nothing %>" == "True") {
			$("#MainStyle").attr('href',"<%= HttpContext.Current.Session("StyleSheet") %>");
		}
	</script>
	<link href="StickyNote/css/style.css" rel="stylesheet" type="text/css" />
	<style type="text/css">
        .contentStickyNote {
            position: absolute;
            top: 40px;
            padding: 0px;
            margin: 0px;
            height: 300px;
            left: 300px;
        }

        .span.cellWithoutBackground {
            display: block;
            background-color: red;
            margin-right: -2px;
            margin-left: -2px;
            height: 14px;
            padding: 4px;
            font-weight: bold;
        }

        #lblYear,
        #lblAircraft, 
        #lblAircraftforAircraftUtilizationGraph,
        #lblPeriod{
            font-size: 1.25rem;
        }

    </style>
	<script src="StickyNote/js/jquery.stickynote.js" type="text/javascript"></script>
	<script src="StickyNote/js/ui.core.js" type="text/javascript"></script>
	<script src="StickyNote/js/ui.draggable.js" type="text/javascript"></script>
	<script src="StickyNote/js/jquery.cookie.js" type="text/javascript"></script>
	<script type="text/javascript">
		function displyStickyNote() {

			var str_PendingOrder = $("#lblPendingOrder").text();
			/*var cnt = str_PendingOrder.substring(0, 3);
			var text = str_PendingOrder.substring(3, str_PendingOrder.length);
			str_PendingOrder = '<font color="Red">' + cnt + '</font>' + text;*/
			str_PendingOrder = '<font color="Red">' + str_PendingOrder + '</font>';


			var str_CalibrationDueReport = $("#lblCalibrationDueReport").text();
			str_CalibrationDueReport = '<font color="Red">' + str_CalibrationDueReport + '</font>';


			var str_ExpiredItems = $("#lblExpiredItems").text();
			str_ExpiredItems = '<font color="Red">' + str_ExpiredItems + '</font>';


			var str_ItemsToExpire = $("#lblItemsToExpire").text();
			str_ItemsToExpire = '<font color="Red">' + str_ItemsToExpire + '</font>';

			var str_CoreUnitDue = $("#lblCoreUnitDue").text();
			str_CoreUnitDue = '<font color="Red">' + str_CoreUnitDue + '</font>';




			jQuery("#FlyPalstickynote").stickynote({
				size: 'large',
				ontop: false,
				//                text: str_PendingOrder + '<BR/><BR/>' +
				//                      str_CalibrationDueReport + '<BR/><BR/>' +
				//                      str_ExpiredItems + '<BR/><BR/>' +
				//                      str_Expired
				text: getStr()
			});


			function getStr() {

				var str;


				str = str_PendingOrder + '<BR/><BR/>' +
					str_CalibrationDueReport + '<BR/><BR/>' +
					str_ExpiredItems + '<BR/><BR/>' +
					str_ItemsToExpire + '<BR/><BR/>' +
					str_CoreUnitDue;


				return str;
			}



			////            if ($.cookie('noShowInvStickynote') == 'true') $('.contentStickyNote').hide();
			////            else {
			////                $.cookie('noShowInvStickynote', true);
			////                jQuery("#FlyPalstickynote").trigger('click');
			////                setTimeout(function () {
			////                    $(".contentStickyNote").fadeOut(1500);
			////                }, 7000);
			////            }
		}
	</script>
	<script type="text/javascript">
		function AircraftConsumptionGraph(AircraftConsumption) {
			var revenueChart = new FusionCharts({
				"type": "bar2d",
				"renderAt": "AircraftConsumptionDiv",
				"width": "520",
				"height": "340",
				"dataFormat": "json",
				"dataSource": {
					"chart": {
						"caption": "Aircraft Inventory Consumption",
						"subCaption": $("#cmbYear :selected").text(),
						"xAxisName": "Month",
						"yAxisName": "Amount In Base Currency",
						"theme": "zune",
						"exportEnabled": "1",
						"paletteColors": "#0075c2",
						"bgColor": "#ffffff",
						"showBorder": "0",
						"showCanvasBorder": "0",
						"canvasbgColor": "#009dd9",
						"usePlotGradientColor": "0",
						"plotBorderAlpha": "10",
						"placeValuesInside": "1",
						"valueFontColor": "#ffffff",
						"valueFontBold": "1",
						"showAxisLines": "1",
						"axisLineAlpha": "25",
						"divLineAlpha": "10",
						"alignCaptionWithCanvas": "0",
						"showAlternateVGridColor": "0",
						"captionFontSize": "14",
						"subcaptionFontSize": "14",
						"subcaptionFontBold": "0",
						"toolTipColor": "#ffffff",
						"toolTipBorderThickness": "0",
						"toolTipBgColor": "#000000",
						"toolTipBgAlpha": "80",
						"toolTipBorderRadius": "2",
						"toolTipPadding": "5",
						"xAxisNameFontColor": "#000000",
						"yAxisNameFontColor": "#000000"

					},
					"data": JSON.parse(AircraftConsumption)
				}
			});
			revenueChart.render();
		}
	</script>
	<script type="text/javascript">
		function TransactionwisePendingOrdersFunc() {
			var getTabularData = function () {
				var table = document.getElementById('T2'), // ‘T2’ here is the table ID
					rows = table.children[0].children,
					row,
					i,
					length,
					data = [];
				// get the table element and iterate over its children to extract the data
				for (i = 1, length = rows.length; i < length; i++) {
					row = rows[i];
					data.push({
						label: row.children[0].innerHTML,
						value: row.children[1].innerHTML
					});
				}
				return data;
			};
			//            document.getElementById('convert').onclick = function () {
			// on click, create the chart using the data obtained by calling the getTabularData() function
			var revenueChart = new FusionCharts({
				type: 'column2d',
				renderAt: 'TransactionwisePendingOrders',
				width: '520',
				height: '300',
				dataFormat: 'json',
				id: 'chart1',
				dataSource: {
					"chart": {
						"caption": "Orders are Pending for Receipts",
						//                            "subCaption": "Harry's SuperMart",
						"xAxisName": "Order Type",
						"yAxisName": "Count",
						//                            "numberPrefix": "$",
						"theme": "zune",
						"rotateValues": "1",
						"exportEnabled": "1",
						"placeValuesInside": "0",
						"valuefontcolor": "074868",
						"rotateValues": "0"
					},
					"data": getTabularData()
				}
			});
			revenueChart.render();
			//            }
		}
	</script>
	<script type="text/javascript">
		function FusionChartFunc(MELGraphValues) {
			var revenueChart = new FusionCharts({
				"type": "Column2D",
				"renderAt": "MyMELChart",
				"width": "520",
				"height": "300",
				"dataFormat": "json",
				"dataSource": {
					"chart": {
						"caption": "Total Log Defect / Pireps Count per Month <br> (FOR ALL AIRCRAFTS)",
						"subCaption": $("#cmbYear :selected").text(),
						"xAxisName": "Month",
						"yAxisName": "Log Defect / Pireps Count",
						"exportEnabled": "1",
						"theme": "zune"
					},
					"data": JSON.parse(MELGraphValues)
				}
			});
			revenueChart.render();
		}
	</script>
	<script type="text/javascript">
		function FusionChartPieFunc(PieGraphValues) {
			var revenueChart = new FusionCharts({
				"type": "Pie3D",
				"renderAt": "MyPieChart",
				"width": "520",
				"height": "300",
				"dataFormat": "json",
				"dataSource": {
					"chart": {
						"caption": "Total Flying (Hrs) per Year",
						"subCaption": $("#cmbYear :selected").text(),
						"startingAngle": "120",
						"showLabels": "0",
						"showLegend": "1",
						"enableMultiSlicing": "0",
						"slicingDistance": "15",
						//"showValues": "1",
						//To show the values in percentage
						"showPercentValues": "1",
						"showPercentInTooltip": "0",
						"exportEnabled": "1",
						"plotTooltext": "Reg No : $label<br>Total Flying : $datavalue",
						"theme": "zune"

					},
					"data": JSON.parse(PieGraphValues)

				}

			});
			revenueChart.render();
		}

	</script>
	<script type="text/javascript">
		function FusionChartLineFunc(LineGraphValues) {
			var revenueChart = new FusionCharts({
				"type": "Line",
				"renderAt": "LineGraph",
				"width": "520",
				"height": "300",
				"dataFormat": "json",
				"dataSource": {
					"chart": {
						"caption": "Total Time (Hrs) per Month",
						"subCaption": $("#cmbYear :selected").text() + " for " + $("#cmbAircraft :selected").text(),
						"xAxisName": "Month",
						"yAxisName": "Total Time (Hrs)",
						"exportEnabled": "1",
						//Cosmetics
						"lineThickness": "2",
						"paletteColors": "#0075c2",
						//      "baseFontColor": "#333333",
						// "baseFont": "Helvetica Neue,Arial",
						// "captionFontSize": "14",
						//"subcaptionFontSize": "14",
						//  "subcaptionFontBold": "0",
						//    "showBorder": "0",
						//    "bgColor": "#ffffff",
						"showShadow": "0",
						//"canvasBgColor": "#ffffff",
						//"canvasBorderAlpha": "0",
						"divlineAlpha": "100",
						"divlineColor": "#999999",
						"divlineThickness": "1",
						"divLineIsDashed": "1",
						"divLineDashLen": "1",
						"divLineGapLen": "1",
						"showXAxisLine": "1",
						"xAxisLineThickness": "1",
						"xAxisLineColor": "#999999",
						"showAlternateHGridColor": "0",
						"theme": "zune"

					},
					"data": JSON.parse(LineGraphValues)

				}

			});
			revenueChart.render();
		}

	</script>
	<style type="text/css">
        .styled-button-2 {
            -webkit-box-shadow: rgba(0,0,0,0.2) 0 1px 0 0;
            -moz-box-shadow: rgba(0,0,0,0.2) 0 1px 0 0;
            box-shadow: rgba(0,0,0,0.2) 0 1px 0 0;
            border-bottom-color: #333;
            border: 1px solid #61c4ea;
            background-color: #7cceee;
            border-radius: 5px;
            -moz-border-radius: 5px;
            -webkit-border-radius: 5px;
            color: #333;
            font-family: 'Verdana',Arial,sans-serif;
            font-size: 14px;
            text-shadow: #b2e2f5 0 1px 0;
            padding: 5px;
        }
    </style>
	<style type="text/css">
        * {
            margin: 0;
        }

        #panel {
            position: fixed;
            background: whitesmoke;
            color: #fff;
            height: 100%;
            width: 300px;
            right: -300px;
            transition: right 0.4s ease-in-out;
            -o-transition: right 0.4s ease-in-out;
            -ms-transition: right 0.4s ease-in-out;
            -moz-transition: right 0.4s ease-in-out;
            -webkit-transition: right 0.4s ease-in-out;
            z-index: 9999;
        }

            #panel h2, #panel p {
                padding: 25px;
            }

        #panelCaller {
            position: absolute;
            top: 50px;
            right: 300px;
            padding: 10px 10px;
            background: rgb(0,157,217);
        }

        #panel:hover {
            right: 0px;
        }
    </style>
	<style type="text/css">
        #feedback {
            font-size: 1.4em;
        }

        #selectable-1 .ui-selecting {
            background: #FECA40;
        }

        #selectable-1 .ui-selected {
            background: rgb(249,200,125);
            color: white;
        }

        #selectable-1 {
            list-style-type: none;
            margin: 0;
            padding: 0;
            width: 450px;
        }

            #selectable-1 li {
                margin: 3px;
                padding: 1px;
                float: left;
                width: 100px;
                height: 80px;
                font-size: 1em;
                text-align: center;
            }
    </style>
	<%--Added by Harsh on 25th Jan 2024 For TataSteel Dashboards--%>
	<style type="text/css">
        .ui-jqgrid tr.jqgrow td:not(:first-child) {
            word-wrap: break-word; /* IE 5.5+ and CSS3 */
            white-space: pre-wrap; /* CSS3 */
            white-space: -moz-pre-wrap; /* Mozilla, since 1999 */
            white-space: -pre-wrap; /* Opera 4-6 */
            white-space: -o-pre-wrap; /* Opera 7 */
            overflow: hidden;
            height: auto;
            vertical-align: middle;
        }

        th.ui-th-column:not(:first-child) div {
            word-wrap: break-word; /* IE 5.5+ and CSS3 */
            white-space: pre-wrap; /* CSS3 */
            white-space: -moz-pre-wrap; /* Mozilla, since 1999 */
            white-space: -pre-wrap; /* Opera 4-6 */
            white-space: -o-pre-wrap; /* Opera 7 */
            overflow: hidden;
            height: auto;
            vertical-align: middle;
        }
    </style>
	<div style="display: none">
		<table id="T1">
			<tr class="">
				<td>Store
				</td>
				<td>Expired
				</td>
				<td>Expiry within 7 days
				</td>
				<td>Expiry within 15 days
				</td>
				<td>Expiry above 15 days
				</td>
			</tr>
			<% If (AppSettings("ShowDashBoard") = "True") Then%>
			<% Dim Child3 As rptExpiredItemsCountInfo%>
			<% For Each Child3 In mrptExpiredItemsCount%>
			<tr>
				<td>
					<%= Child3.StoreName %>
				</td>
				<td>
					<%= Child3.RedCount%>
				</td>
				<td>
					<%= Child3.YellowCount%>
				</td>
				<td>
					<%= Child3.BlueCount%>
				</td>
				<td>
					<%= Child3.GreenCount%>
				</td>
			</tr>
			<% Next%>
			<% End If%>
		</table>
		<table id="T2">
			<tr>
				<td>Order Type
				</td>
				<td>No. of Pending Orders
				</td>
			</tr>
			<%  If (AppSettings("ShowDashBoard") = "True") Then%>
			<% Dim Child4 As PendingOrdersInfo%>
			<% For Each Child4 In mTransactionwisePendingOrders%>
			<tr>
				<td>
					<%= Child4.TransTypeName%>
				</td>
				<td>
					<%= Child4.PendingOrdersCount%>
				</td>
			</tr>
			<% Next%>
			<% End If%>
		</table>
		<table id="T3">
			<tr>
				<td>Root Cause
				</td>
				<td>Count Of Cause
				</td>
			</tr>
			<% Dim Child5 As RootCauseCountInfo%>
			<% For Each Child5 In mRootCauseCount%>
			<tr>
				<td>
					<%= Child5.RootCause%>
				</td>
				<td>
					<%= Child5.RootCauseCount%>
				</td>
			</tr>
			<% Next%>
		</table>
	</div>
	<script type="text/javascript">
		function RootCauseCountFunc() {
			var getTabularData = function () {
				var table = document.getElementById('T3'), // ‘T3’ here is the table ID
					rows = table.children[0].children,
					row,
					i,
					length,
					data = [];
				// get the table element and iterate over its children to extract the data
				for (i = 1, length = rows.length; i < length; i++) {
					row = rows[i];
					data.push({
						label: row.children[0].innerHTML,
						value: row.children[1].innerHTML
					});
				}
				return data;
			};
			//            document.getElementById('convert').onclick = function () {
			// on click, create the chart using the data obtained by calling the getTabularData() function
			var revenueChart = new FusionCharts({
				type: 'column2d',
				renderAt: 'RootCauseCount',
				width: '520',
				height: '300',
				dataFormat: 'json',
				id: 'chart3',
				dataSource: {
					"chart": {
						"caption": "Root Cause Analysis",
						//                            "subCaption": "Harry's SuperMart",
						"xAxisName": "Root Cause",
						"yAxisName": "Count",
						//                            "numberPrefix": "$",
						"theme": "zune",
						"rotateValues": "1",
						"exportEnabled": "1",
						"placeValuesInside": "0",
						"valuefontcolor": "074868",
						"rotateValues": "0"
					},
					"data": getTabularData()
				}
			});
			revenueChart.render();
			//            }
		}
	</script>


	<script type="text/javascript">
		function AircraftUtilizationGraphFunc(AircraftUtilizationGraphValues) {
			var comboval = document.getElementById('cmbPeriod').value;
			var yAxisname = "";
			if (comboval == "0") {
				yAxisname = "Airborne Time";
			}
			else {
				yAxisname = "Landings";
			}

			var revenueChart = new FusionCharts({
				"type": "column2d",
				"renderAt": "AircraftUtilizationGraph",
				"width": "520",
				"height": "300",
				"dataFormat": "json",
				"dataSource": {
					"chart": {
						"caption": "Aircraft Utilization Graph",
						"xAxisName": "Aircraft",
						"yAxisName": yAxisname,
						"theme": "carbon",
						"exportEnabled": "1",
						"paletteColors": "#0075c2",
						"bgColor": "#ffffff",
						"showBorder": "0",
						"showCanvasBorder": "0",
						"canvasbgColor": "#ffffff",
						"usePlotGradientColor": "0",
						"plotBorderAlpha": "10",
						"placeValuesInside": "1",
						"valueFontColor": "#ffffff",
						"valueFontBold": "1",
						"showAxisLines": "1",
						"axisLineAlpha": "25",
						"divLineAlpha": "10",
						"alignCaptionWithCanvas": "0",
						"showAlternateVGridColor": "0",
						"captionFontSize": "14",
						"subcaptionFontSize": "14",
						"subcaptionFontBold": "0",
						"toolTipColor": "#ffffff",
						"toolTipBorderThickness": "0",
						"toolTipBgColor": "#000000",
						"toolTipBgAlpha": "80",
						"toolTipBorderRadius": "2",
						"toolTipPadding": "5",
						"xAxisNameFontColor": "#000000",
						"yAxisNameFontColor": "#000000"

					},
					"data": JSON.parse(AircraftUtilizationGraphValues)
				}
			});
			revenueChart.render();
		}
	</script>
</head>
<body ms_positioning="GridLayout">
	<form id="Form1" method="post" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
		</asp:ScriptManager>
		<asp:Panel ID="pnlDashBoard" Style="z-index: 102; left: 16px; position: absolute; top: 8px"
			runat="server">

			<table id="Table1" cellspacing="1" cellpadding="1" width="810" border="0">
				<tr>
					<td>
						<asp:UpdatePanel ID="pnlReports" runat="server" UpdateMode="Conditional">
							<ContentTemplate>
								<table id="Table2" cellspacing="1" cellpadding="1" border="0">
									<tr>
										<td align="center">
											<asp:UpdatePanel ID="upnlYear" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
													<table>
														<tr>
															<td>
																<asp:Label ID="lblYear" runat="server" CssClass="clsLabelAuto" Font-Bold="true"
																	Visible="false">Year</asp:Label>
															</td>
															<td>
																<asp:DropDownList ID="cmbYear" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
																	AutoPostBack="True" Width="100px" ClientIDMode="Static" Visible="false">
																</asp:DropDownList>
																<asp:DropDownList ID="cmbMonth" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
																	AutoPostBack="True" Width="100px" ClientIDMode="Static" Visible="false">
																</asp:DropDownList>
															</td>
														</tr>
													</table>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
									<tr>
										<td>
											<table>
												<tr>
													<asp:UpdatePanel ID="upnlMyChart" runat="server" UpdateMode="Conditional">
														<ContentTemplate>
															<asp:PlaceHolder ID="phMEL" runat="server" Visible="false">
																<td>
																	<fieldset id="fdsMyChart" style="border-width: 1px;">
																		<div id="MyMELChart">
																		</div>
																	</fieldset>
																</td>
															</asp:PlaceHolder>
														</ContentTemplate>
													</asp:UpdatePanel>
													<asp:UpdatePanel ID="upnlMyPieChart" runat="server" UpdateMode="Conditional">
														<ContentTemplate>
															<asp:PlaceHolder ID="phPie" runat="server" Visible="false">
																<td>
																	<fieldset id="fdsMyPieChart" style="border-width: 1px;">
																		<div id="MyPieChart">
																		</div>
																	</fieldset>
																</td>
															</asp:PlaceHolder>
														</ContentTemplate>
													</asp:UpdatePanel>
												</tr>
												<tr>
													<td valign="top">
														<asp:UpdatePanel ID="upnlLineGraph" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
																<asp:PlaceHolder ID="phFlyingLine" runat="server" Visible="false">
																	<fieldset id="fdsLineGraph" style="border-width: 1px;">
																		<table>
																			<tr>
																				<td align="left" style="margin-left: 40px">
																					<asp:Label ID="lblAircraft" runat="server" Font-Bold="true"
																						CssClass="clsLabelAuto">Aircraft</asp:Label>
																				</td>
																				<td align="left">
																					<asp:DropDownList ID="cmbAircraft" runat="server" CssClass="clsTextBoxTagSearchComboSmall" DataTextField="RegNo"
																						AutoPostBack="true" DataValueField="ID">
																					</asp:DropDownList>
																				</td>
																			</tr>
																			<tr>
																				<td colspan="2">
																					<div id="LineGraph">
																					</div>
																				</td>
																			</tr>
																		</table>
																	</fieldset>
																</asp:PlaceHolder>
															</ContentTemplate>
														</asp:UpdatePanel>
													</td>
													<td>
														<asp:PlaceHolder ID="phCurrentStatus" runat="server" Visible="false">
															<fieldset id="Fieldset1" style="border-width: 1px;">
																<asp:UpdatePanel ID="upnlCurrentStatus" runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<table id="JQGridDemo" cellspacing="0" cellpadding="0" border="0">
																		</table>
																		<div id="jqGridPager">
																		</div>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</fieldset>
														</asp:PlaceHolder>
													</td>
												</tr>
												<tr>
													<asp:PlaceHolder ID="phAuditDetails" runat="server" Visible="false">
														<td valign="top">
															<fieldset id="Fieldset6" style="border-width: 1px;">
																<asp:UpdatePanel ID="upnlAuditDetails" runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<table id="JQGridForAuditDetails" cellspacing="0" cellpadding="0" border="0">
																		</table>
																		<div id="jqGridPagerForAuditDetails">
																		</div>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</fieldset>
														</td>
													</asp:PlaceHolder>
													<asp:PlaceHolder ID="phRootCauseCount" runat="server" Visible="false">
														<td>
															<fieldset id="fdsRootCauseCount" style="border-width: 1px;">
																<asp:UpdatePanel ID="upnlRootCauseCount" runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<div id="RootCauseCount">
																		</div>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</fieldset>
														</td>
													</asp:PlaceHolder>
												</tr>
												<tr>
													<asp:PlaceHolder ID="phMELPirepsChart" runat="server" Visible="false">
														<td>
															<fieldset id="fdsMELPirepsChart" style="border-width: 1px;">
																<asp:UpdatePanel ID="upnlMELPirepsChart" runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<div id="MELPirepsChart">
																		</div>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</fieldset>
														</td>
													</asp:PlaceHolder>

													<td>
														<asp:PlaceHolder ID="phAircraftUtilizationGraph" runat="server" Visible="false">
															<asp:UpdatePanel ID="upnlAircraftUtilizationGraph" runat="server" UpdateMode="Conditional">
																<ContentTemplate>


																	<fieldset id="fdAircraftUtilizationGraph" style="border-width: 1px;">
																		<table width="100%">
																			<tr>
																				<td align="left" style="margin-left: 40px">
																					<asp:Label ID="lblAircraftforAircraftUtilizationGraph" runat="server" Font-Bold="true"
																						CssClass="clsLabelAuto">Aircraft</asp:Label>
																				</td>
																				<td align="left">
																					<asp:DropDownList ID="cmbAircraftforAircraftUtilizationGraph" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="RegNo"
																						AutoPostBack="true" DataValueField="ID">
																					</asp:DropDownList>
																				</td>

																				<td>
																					<asp:Label ID="lblPeriod" runat="server" Font-Bold="true"
																						CssClass="clsLabelAuto">Period</asp:Label>
																				</td>

																				<td>
																					<asp:DropDownList ID="cmbPeriod" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="true">
																						<asp:ListItem Value="0" Text="Hour"></asp:ListItem>
																						<asp:ListItem Value="1" Text="Landings"></asp:ListItem>
																					</asp:DropDownList>
																				</td>

																			</tr>
																			<tr>
																				<td colspan="4">
																					<div id="AircraftUtilizationGraph">
																					</div>
																				</td>
																			</tr>
																		</table>
																	</fieldset>


																</ContentTemplate>
															</asp:UpdatePanel>
														</asp:PlaceHolder>
													</td>
												</tr>
												<tr>
													<asp:PlaceHolder ID="phLogDet" runat="server" Visible="false">
														<td>
															<fieldset id="Fieldset4" style="border-width: 1px;">
																<table>
																	<tr>
																		<td>
																			<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
																				<ContentTemplate>
																					<table>
																						<tr>
																							<td>
																								<input id="Tabular" type="submit" runat="server" class="styled-button-2" value="Tabular" />
																							</td>
																							<td>
																								<input id="Line" type="submit" runat="server" class="styled-button-2" value="Line" />
																							</td>
																						</tr>
																					</table>
																				</ContentTemplate>
																			</asp:UpdatePanel>
																		</td>
																	</tr>
																	<tr>
																		<td colspan="1">
																			<asp:UpdatePanel ID="upnlJQGridLogDet" runat="server" UpdateMode="Conditional">
																				<ContentTemplate>
																					<asp:PlaceHolder ID="phJQgrid" runat="server" Visible="false">
																						<table id="Table12" runat="server">
																							<tr>
																								<td>
																									<%--<fieldset id="Fieldset2" style="border-width: 1px;">--%>
																									<table id="JQGridLogDet" cellspacing="0" cellpadding="0" border="0">
																									</table>
																									<div id="JQGridLogDetPager">
																									</div>
																									<%--</fieldset>--%>
																								</td>
																							</tr>
																						</table>
																					</asp:PlaceHolder>
																				</ContentTemplate>
																			</asp:UpdatePanel>
																			<asp:UpdatePanel ID="upnlLogDetLineGraph" runat="server" UpdateMode="Conditional">
																				<ContentTemplate>
																					<asp:PlaceHolder ID="phLogDetLine" runat="server" Visible="false">
																						<table id="Table3" runat="server">
																							<tr>
																								<td>
																									<fieldset id="fdsLogDetLineGraph" style="border-width: 1px;">
																										<div id="LogDetLineGraph">
																										</div>
																									</fieldset>
																								</td>
																							</tr>
																						</table>
																					</asp:PlaceHolder>
																				</ContentTemplate>
																			</asp:UpdatePanel>
																		</td>
																	</tr>
																</table>
															</fieldset>
														</td>
													</asp:PlaceHolder>
													<asp:PlaceHolder ID="phWOLIst" runat="server" Visible="false">
														<td>
															<fieldset id="fdsWOlist" style="border-width: 1px">
																<asp:UpdatePanel ID="upnlWOList" runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<table id="JQGridForWOList" cellspacing="0" cellpadding="0" border="0">
																		</table>
																		<div id="jqGridPagerForWOList">
																		</div>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</fieldset>
														</td>
													</asp:PlaceHolder>
												</tr>
												<tr>
													<asp:PlaceHolder ID="phExpiredItems" runat="server" Visible="false">
														<td>
															<fieldset id="Fieldset5" style="border-width: 1px;">
																<table>
																	<tr>
																		<td>
																			<asp:UpdatePanel ID="upnlExpiredItems" runat="server" UpdateMode="Conditional">
																				<ContentTemplate>
																					<table>
																						<tr>
																							<td>
																								<input id="BarExpiredItems" type="submit" runat="server" class="styled-button-2"
																									value="Bar" />
																							</td>
																							<td>
																								<input id="btnExpiredItemsDetails" type="submit" runat="server" class="styled-button-2"
																									value="Details" />
																							</td>
																							<td>
																								<input id="TabularExpiredItems" type="submit" runat="server" class="styled-button-2"
																									value="Tabular" />
																							</td>
																						</tr>
																					</table>
																				</ContentTemplate>
																			</asp:UpdatePanel>
																		</td>
																	</tr>
																	<tr>
																		<td colspan="1">
																			<asp:UpdatePanel ID="upnlExpiredItemsCountForReport" runat="server" UpdateMode="Conditional">
																				<ContentTemplate>
																					<asp:PlaceHolder ID="phExpiredItemsCountForReport" runat="server" Visible="False">
																						<%--<fieldset id="Fieldset3" style="border-width: 1px;">--%>
																							<table id="JQGridExpiredItemsCountForReport" cellspacing="0" cellpadding="0" border="0">
																							</table>
																							<div id="JQGridExpiredItemsCountPager">
																							</div>
																						<%--</fieldset>--%>
																					</asp:PlaceHolder>
																				</ContentTemplate>
																			</asp:UpdatePanel>
																			<asp:UpdatePanel ID="upnlExpiryDateReport" runat="server" UpdateMode="Conditional">
																				<ContentTemplate>
																					<asp:PlaceHolder ID="phExpiryDateReport" runat="server" Visible="False">
																						<%--<fieldset id="fdsExpiryDateReport" style="border-width: 1px;">--%>
																							<table id="JQGridExpiryDateReport" cellspacing="0" cellpadding="0" border="0">
																							</table>
																							<div id="ExpiryDateReport">
																							</div>
																						<%--</fieldset>--%>
																					</asp:PlaceHolder>
																				</ContentTemplate>
																			</asp:UpdatePanel>
																			<asp:UpdatePanel ID="upnlExpiredItemsInmscolumn2d" runat="server" UpdateMode="Conditional">
																				<ContentTemplate>
																					<asp:PlaceHolder ID="phExpiredItemsInmscolumn2d" runat="server" Visible="False">
																						<%--<fieldset id="fdsExpiredItemsInmscolumn2d" style="border-width: 1px;">--%>
																							<div id="ExpiredItemsInmscolumn2d">
																							</div>
																						<%--</fieldset>--%>
																					</asp:PlaceHolder>
																				</ContentTemplate>
																			</asp:UpdatePanel>
																		</td>
																		<td colspan="1"></td>
																	</tr>
																</table>
															</fieldset>
														</td>
													</asp:PlaceHolder>
													<asp:PlaceHolder ID="phAircraftConsumption" runat="server" Visible="false">
														<td valign="top">
															<fieldset id="fdsAircraftConsumption" style="border-width: 1px">
																<asp:UpdatePanel ID="upnlAircraftConsumption" runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<div id="AircraftConsumptionDiv">
																		</div>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</fieldset>
														</td>
													</asp:PlaceHolder>
												</tr>
												<tr>
													<asp:PlaceHolder ID="phPendingPurchaseQuotationItems" runat="server" Visible="false">
														<td>
															<fieldset id="fdsPendingPurchaseQuotationItems" style="border-width: 1px;">
																<asp:UpdatePanel ID="upnlPendingPurchaseQuotationItems" runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<table id="JQGridPendingPurchaseQuotationItems" cellspacing="0" cellpadding="0" border="0">
																		</table>
																		<div id="PendingPurchaseQuotationItems">
																		</div>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</fieldset>
														</td>
													</asp:PlaceHolder>
													<asp:PlaceHolder ID="phCalibrationDue" runat="server" Visible="false">
														<td>
															<fieldset id="fdsCalibrationDue" style="border-width: 1px;">
																<asp:UpdatePanel ID="upnlCalibrationDue" runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<table id="JQGridCalibrationDue" cellspacing="0" cellpadding="0" border="0">
																		</table>
																		<div id="CalibrationDue">
																		</div>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</fieldset>
														</td>
													</asp:PlaceHolder>
												</tr>
												<tr>
													<asp:PlaceHolder ID="phRequisitionPendingForPurchaseOrder" runat="server" Visible="false">
														<td>
															<fieldset id="fdsRequisitionPendingForPurchaseOrder" style="border-width: 1px;">
																<asp:UpdatePanel ID="upnlRequisitionPendingForPurchaseOrder" runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<table id="JQGridRequisitionPendingForPurchaseOrder" cellspacing="0" cellpadding="0"
																			border="0">
																		</table>
																		<div id="RequisitionPendingForPurchaseOrder">
																		</div>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</fieldset>
														</td>
													</asp:PlaceHolder>
													<asp:PlaceHolder ID="phMinLevelItemReport" runat="server" Visible="false">
														<td>
															<fieldset id="fdsMinLevelItemReport" style="border-width: 1px;">
																<asp:UpdatePanel ID="upnlMinLevelItemReport" runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<table id="JQGridMinLevelItemReport" cellspacing="0" cellpadding="0" border="0">
																		</table>
																		<div id="MinLevelItemReport">
																		</div>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</fieldset>
														</td>
													</asp:PlaceHolder>
												</tr>
												<tr>
													<asp:PlaceHolder ID="phPendingOrders" runat="server" Visible="false">
														<td>
															<fieldset id="Fieldset8" style="border-width: 1px;">
																<table>
																	<tr>
																		<td>
																			<asp:UpdatePanel ID="upnlPendingOrder" runat="server" UpdateMode="Conditional">
																				<ContentTemplate>
																					<table>
																						<tr>
																							<td>
																								<input id="btnTransactionwisePendingOrders" type="submit" runat="server" class="styled-button-2"
																									value="Bar" />
																							</td>
																							<td>
																								<input id="btnPendingPurchaseOrders" type="submit" runat="server" class="styled-button-2"
																									value="Tabular" />
																							</td>
																						</tr>
																					</table>
																				</ContentTemplate>
																			</asp:UpdatePanel>
																		</td>
																	</tr>
																	<tr>
																		<td>
																			<asp:UpdatePanel ID="upnlPendingPurchaseOrders" runat="server" UpdateMode="Conditional">
																				<ContentTemplate>
																					<asp:PlaceHolder ID="phPendingPurchaseOrders" runat="server" Visible="false">
																						<%--<fieldset id="fdsPendingPurchaseOrders" style="border-width: 1px;">--%>
																							<table id="JQGridPendingPurchaseOrders" cellspacing="0" cellpadding="0" border="0">
																							</table>
																							<div id="PendingPurchaseOrders">
																							</div>
																						<%--</fieldset>--%>
																					</asp:PlaceHolder>
																				</ContentTemplate>
																			</asp:UpdatePanel>
																			<asp:UpdatePanel ID="upnlTransactionwisePendingOrders" runat="server" UpdateMode="Conditional">
																				<ContentTemplate>
																					<asp:PlaceHolder ID="phTransactionwisePendingOrders" runat="server" Visible="false">
																						<%--<fieldset id="fdsTransactionwisePendingOrders" style="border-width: 1px;">--%>
																							<div id="TransactionwisePendingOrders">
																							</div>
																						<%--</fieldset>--%>
																					</asp:PlaceHolder>
																				</ContentTemplate>
																			</asp:UpdatePanel>
																		</td>
																	</tr>
																</table>
															</fieldset>
														</td>
													</asp:PlaceHolder>
													<asp:PlaceHolder ID="phPendingToolsToReceiveFromEmployee" runat="server" Visible="false">
														<td>
															<asp:UpdatePanel ID="upnlPendingToolsToReceiveFromEmployee" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<asp:Panel ID="pnlPendingToolsToReceiveFromEmployee" runat="server">
																		<fieldset id="fdsPendingToolsToReceiveFromEmployee" style="border-width: 1px;">
																			<table id="JQGridPendingToolsToReceiveFromEmployee" border="0" cellpadding="0" cellspacing="0">
																			</table>
																			<div id="PendingToolsToReceiveFromEmployee">
																			</div>
																		</fieldset>
																	</asp:Panel>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
													</asp:PlaceHolder>
												</tr>
												<tr>
													<asp:PlaceHolder ID="phPendingToReceiptsFromOtherStore" runat="server" Visible="false">
														<td>
															<asp:UpdatePanel ID="upnlPendingToReceiptsFromOtherStore" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<asp:Panel ID="pnlPendingToReceiptsFromOtherStore" runat="server">
																		<fieldset id="fdsPendingToReceiptsFromOtherStore" style="border-width: 1px;">
																			<table id="JQGridPendingToReceiptsFromOtherStore" cellspacing="0" cellpadding="0"
																				border="0">
																			</table>
																			<div id="PendingToReceiptsFromOtherStore">
																			</div>
																		</fieldset>
																	</asp:Panel>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
													</asp:PlaceHolder>
													<asp:PlaceHolder ID="phReceivedFromAircraftAsCoreUnitReturn" runat="server" Visible="false">
														<td>
															<asp:UpdatePanel ID="upnlReceivedFromAircraftAsCoreUnitReturn" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<asp:Panel ID="pnlReceivedFromAircraftAsCoreUnitReturn" runat="server">
																		<fieldset id="fdsReceivedFromAircraftAsCoreUnitReturn" style="border-width: 1px;">
																			<table id="JQGridReceivedFromAircraftAsCoreUnitReturn" border="0" cellpadding="0"
																				cellspacing="0">
																			</table>
																			<div id="ReceivedFromAircraftAsCoreUnitReturn">
																			</div>
																		</fieldset>
																	</asp:Panel>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
													</asp:PlaceHolder>
												</tr>
												<tr>
													<asp:PlaceHolder ID="phReceivedUnserviceablePart" runat="server" Visible="false">
														<td>
															<asp:UpdatePanel ID="upnlReceivedUnserviceablePart" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<asp:Panel ID="pnlReceivedUnserviceablePart" runat="server">
																		<fieldset id="fdsReceivedUnserviceablePart" style="border-width: 1px;">
																			<table id="JQGridReceivedUnserviceablePart" border="0" cellpadding="0" cellspacing="0">
																			</table>
																			<div id="ReceivedUnserviceablePart">
																			</div>
																		</fieldset>
																	</asp:Panel>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
													</asp:PlaceHolder>
													<asp:PlaceHolder ID="phLoanInWardRecord" runat="server" Visible="false">
														<td>
															<asp:UpdatePanel ID="upnlLoanInWardRecord" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<asp:Panel ID="pnlLoanInWardRecord" runat="server">
																		<fieldset id="fdsLoanInWardRecord" style="border-width: 1px;">
																			<table id="JQGridLoanInWardRecord" border="0" cellpadding="0" cellspacing="0">
																			</table>
																			<div id="LoanInWardRecord">
																			</div>
																		</fieldset>
																	</asp:Panel>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
													</asp:PlaceHolder>
												</tr>
												<tr>
													<asp:PlaceHolder ID="phReOrderLevelItemReport" runat="server" Visible="false">
														<td>
															<fieldset id="fdsReOrderLevelItemReport" style="border-width: 1px;">
																<asp:UpdatePanel ID="upnlReOrderLevelItemReport" runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<table id="JQGridReOrderLevelItemReport" cellspacing="0" cellpadding="0" border="0">
																		</table>
																		<div id="ReOrderLevelItemReport">
																		</div>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</fieldset>
														</td>
													</asp:PlaceHolder>
													<asp:PlaceHolder ID="phPendingReturnableExchangeRepairIssueToVendorItemReport" runat="server"
														Visible="false">
														<td>
															<fieldset id="fdsPendingReturnableExchangeRepairIssueToVendorItemReport" style="border-width: 1px;">
																<asp:UpdatePanel ID="upnlPendingReturnableExchangeRepairIssueToVendorItemReport"
																	runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<table id="JQGridPendingReturnableExchangeRepairIssueToVendorItemReport" cellspacing="0"
																			cellpadding="0" border="0">
																		</table>
																		<div id="PendingReturnableExchangeRepairIssueToVendorItemReport">
																		</div>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</fieldset>
														</td>
													</asp:PlaceHolder>
												</tr>
												<tr>
													<asp:PlaceHolder ID="phLoanOutWardReport" runat="server" Visible="false">
														<td>
															<fieldset id="fdsLoanOutWardReport" style="border-width: 1px;">
																<asp:UpdatePanel ID="upnlLoanOutWardReport" runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<table id="JQGridLoanOutWardReport" cellspacing="0" cellpadding="0" border="0">
																		</table>
																		<div id="LoanOutWardReport">
																		</div>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</fieldset>
														</td>
													</asp:PlaceHolder>

													<%--Added by Sachin on 25th Jan 2024 For TataSteel Dashboards--%>

													<asp:PlaceHolder ID="phAircraftCertificate" runat="server" Visible="false"><%--Visible="false"--%>
														<td>
															<fieldset id="fdsAircraftCertificate" style="border-width: 1px;">
																<asp:UpdatePanel ID="upnlAircraftCertificate" runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<table id="JQGridAircraftCertificate" cellspacing="0" cellpadding="0" border="0">
																		</table>
																		<div id="AircraftCertificate">
																		</div>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</fieldset>
														</td>
													</asp:PlaceHolder>
												</tr>
												<%--Added by Harsh on 25th Jan 2024 For TataSteel Dashboards--%>
												<tr>
													<asp:PlaceHolder ID="phPreFlightAuthorization" runat="server" Visible="false">
														<td>
															<fieldset id="fdsPreFlightAuthorization" style="border-width: 1px;">
																<asp:UpdatePanel ID="upnlPreFlightAuthorization" runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<table id="JQGridPreFlightAuthorization" cellspacing="0" cellpadding="0" border="0">
																		</table>
																		<div id="PreFlightAuthorization">
																		</div>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</fieldset>
														</td>
													</asp:PlaceHolder>
													<asp:PlaceHolder ID="phAMECertification" runat="server" Visible="false">
														<td>
															<fieldset id="fdsAMECertification" style="border-width: 1px;">
																<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<table id="JQGridAMECertification" cellspacing="0" cellpadding="0" border="0">
																		</table>
																		<div id="AMECertification">
																		</div>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</fieldset>
														</td>
													</asp:PlaceHolder>
												</tr>

											</table>
										</td>
									</tr>
								</table>
							</ContentTemplate>
						</asp:UpdatePanel>
					</td>
				</tr>
			</table>
			<table id="Table4" cellspacing="1" cellpadding="1" width="810" border="0">
				<tr>
					<td>
						<asp:Panel ID="pnlInventoryLinks" runat="server" Width="912px">
							&nbsp;
                        <table id="tblLinks" cellspacing="1" cellpadding="1" border="0">
							<tr>
								<td>
									<asp:LinkButton ID="lnkPendingOrder" runat="server" CausesValidation="False" CssClass="clsLinkButton"
										ForeColor="#0000C0" Visible="False">
									</asp:LinkButton>
								</td>
								<td>
									<asp:Label ID="L1" runat="server" Visible="False" Width="8px"></asp:Label>
								</td>
								<td>
									<asp:LinkButton ID="lnkCalibrationDueReport" runat="server" CausesValidation="False"
										CssClass="clsLinkButton" ForeColor="#0000C0" Visible="False">
									</asp:LinkButton>
								</td>
								<td>
									<asp:Label ID="L2" runat="server" Visible="False" Width="8px"></asp:Label>
								</td>
								<td>
									<asp:LinkButton ID="lnkExpiredItems" runat="server" CausesValidation="False" CssClass="clsLinkButton"
										ForeColor="#0000C0" Visible="False">
									</asp:LinkButton>
								</td>
								<td>
									<asp:Label ID="L3" runat="server" Visible="False" Width="8px"></asp:Label>
								</td>
								<td>
									<asp:LinkButton ID="lnkItemsToExpire" runat="server" CausesValidation="False" CssClass="clsLinkButton"
										ForeColor="#0000C0" Visible="False">
									</asp:LinkButton>
								</td>
								<td>
									<asp:Label ID="L4" runat="server" Visible="False" Width="8px"></asp:Label>
								</td>
								<td>
									<asp:LinkButton ID="lnkCoreUnitDue" runat="server" CausesValidation="False" CssClass="clsLinkButton"
										ForeColor="#0000C0" Visible="False">
									</asp:LinkButton>
								</td>
								<td></td>
							</tr>
						</table>
						</asp:Panel>
					</td>
				</tr>
				<tr>
					<td>
						<div id="FlyPalstickynote" style="display: none;">
							<asp:Label ID="lblPendingOrder" runat="server" Text=""></asp:Label>
							<asp:Label ID="lblCalibrationDueReport" runat="server" Text=""></asp:Label>
							<asp:Label ID="lblExpiredItems" runat="server" Text=""></asp:Label>
							<asp:Label ID="lblItemsToExpire" runat="server" Text=""></asp:Label>
							<asp:Label ID="lblCoreUnitDue" runat="server" Text=""></asp:Label>
							<asp:Label ID="lblCrossDue" runat="server" Text=""></asp:Label>
							<asp:Label ID="lblApproachingDue" runat="server" Text=""></asp:Label>
							<asp:Label ID="lblForecasting" runat="server" Text=""></asp:Label>
						</div>
						<div id="content" class="contentStickyNote">
						</div>
					</td>
				</tr>
				<tr>
					<td>
						<asp:Panel ID="pnlAircraftInfoBoard" runat="server" Visible="False">
							<div class="container-fluid bg-3 text-center">
								<div style="width: 100%">
									<h1>
										<b>
											<p style="background-color: lavender; font-weight: bold; color: Black; font-size: medium; font-variant: normal;">
												<asp:Label ID="lblInfoBoard" runat="server" CssClass="">AIRCRAFT INFORMATION BOARD</asp:Label>
											</p>
										</b>
									</h1>
								</div>
								<div style="width: 100%;">
									<asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
								</div>
							</div>
						</asp:Panel>
					</td>
				</tr>
				<tr>
					<td align="center"></td>
				</tr>
			</table>
		</asp:Panel>
		<script type="text/javascript">
			$.noConflict();
			arrtSetting = function (rowId, val, rawObject, cm) {
				var attr = rawObject.attr[cm.name], result;
				if (attr.rowspan) {
					result = ' rowspan=' + '"' + attr.rowspan + '"';
				} else if (attr.display) {
					result = ' style="display:' + attr.display + '"';
				}
				return result;
			};

			Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
				$('#JQGridDemo').jqGrid({
					url: 'DashBoard.aspx/AircraftCurrentStatusList',
					datatype: "json",
					mtype: 'POST',
					serializeGridData: function (postData) {
						return JSON.stringify(postData);
					},

					ajaxGridOptions: { contentType: "application/json" },
					loadonce: true,
					colNames: ['RegNo', 'Type', 'Manufacturer', 'Model Name', 'SerialNoPosition', 'Manufacturing Date', 'Hrs', 'Lndngs', 'Cyc', 'Other Periods', 'Since OH', 'Last Flown'],
					colModel: [
						{ name: 'RegNo', index: 'RegNo', width: 70 },
						{ name: 'Type', index: 'Type', width: 40 },
						{ name: 'ManufacturerName', index: 'ManufacturerName', width: 90 },
						{ name: 'ModelName', index: 'ModelName', width: 80 },
						{ name: 'SerialNoPosition', index: 'SerialNoPosition', width: 80 },
						{ name: 'ManufacturingDateFormatted', index: 'ManufacturingDateFormatted', width: 90 },
						{ name: 'Hours', index: 'Hours', width: 70 },
						{ name: 'Landings', index: 'Landings', width: 50 },
						{ name: 'Cycles', index: 'Cycles', width: 50 },
						{ name: 'AllPeriods', index: 'AllPeriods', width: 100 },
						{ name: 'SinceOH', index: 'SinceOH', width: 50 },
						{ name: 'LastFlownDateFormatted', index: 'LastFlownDateFormatted', width: 70 }

					],

					viewecords: true, // show the current page, data rang and total records on the toolbar
					width: 550,
					height: 230,
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
					caption: "Aircraft Current status",
					grouping: true,
					groupingView: {
						groupField: ["RegNo"],
						groupColumnShow: [false],
						groupDataSorted: true,
						groupOrder: ["asc"],
						groupSummary: [false],
						groupSummaryPos: ['header'],
						groupText: ['<b>{0}</b>'],
						groupCollapse: false,
						minusicon: 'fa-plus'
					}
				});
			});
		</script>
		<script type="text/javascript">
			Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
				$('#JQGridForAuditDetails').jqGrid({
					url: 'DashBoard.aspx/AuditDetails',
					datatype: "json",
					mtype: 'POST',
					serializeGridData: function (postData) {
						return JSON.stringify(postData);
					},
					ajaxGridOptions: { contentType: "application/json" },
					loadonce: true,
					colNames: ['Schedule Date', 'Audit No.', 'Status', 'Pending Days'],
					colModel: [
						{ name: 'AuditScheduleDateFormatted', index: 'Audit Date', align: 'left' },
						{ name: 'AuditNo', index: 'AuditNo', align: 'left' },
						{
							name: 'AuditStatusName', index: 'Status', align: 'left', formatter: function (cellvalue) {
								var color;
								var val = cellvalue;
								if (val == 'Schedule') {
									color = 'yellow';
								} else if (val == 'Open') {
									color = '#80BFFF';
								} else {
									color = 'green';
								}
								return '<span class="cellWithoutBackground" style="background-color:' + color + ';font-weight: bold;font-size: 14px;">' + cellvalue + '</span>';
							}
						},
						{ name: 'PendingDays', index: 'Pending Days', align: 'right' }
					],
					viewecords: true, // show the current page, data rang and total records on the toolbar
					width: 500,
					height: 230,
					rowNum: 10,
					loadonce: true, // this is just for the demo
					pager: "#jqGridPagerForAuditDetails",
					jsonReader: {
						page: function (obj) { return 1; },
						total: function (obj) { return 1; },
						records: function (obj) { return obj.d.length; },
						root: function (obj) { return obj.d; },
						repeatitems: false,
						id: "0"
					},
					caption: "Audit Details"
				});
			});
		</script>
		<script type="text/javascript">
			function FuncOpenWOList(mMachineID) {
				$('#JQGridForWOList').jqGrid({
					url: 'DashBoard.aspx/WODetails',
					datatype: "json",
					mtype: 'POST',
					postData:
					{
						MachineID: mMachineID //$.trim($("[id*=MachineID]").val())
					},
					serializeGridData: function (postData) {
						return JSON.stringify(postData);
					},
					ajaxGridOptions: { contentType: "application/json" },
					loadonce: true,
					colNames: ['W.O. Date', 'W.O. No.', 'RegNo', 'ModelName', 'SerialNo', 'Customer WO No', 'Customer', 'WOStartDate', 'WOBy'],
					colModel: [
						{ name: 'WODateFormatted', index: 'Date', align: 'left' },
						{ name: 'WONumber', index: 'WONumber', align: 'left' },
						{ name: 'RegNo', index: 'RegNo', align: 'left' },
						{ name: 'ModelName', index: 'ModelName', align: 'left' },
						{ name: 'SerialNo', index: 'SerialNo', align: 'left' },
						{ name: 'CustomerWONo', index: 'CustomerWONo', align: 'left' },
						{ name: 'CustomerName', index: 'CustomerName', align: 'left' },
						{ name: 'WOStartDateFormatted', index: 'WOStartDateFormatted', align: 'left' },
						{ name: 'WOBy', index: 'WOBy', align: 'left' }
					],
					viewecords: true, // show the current page, data rang and total records on the toolbar
					width: 500,
					height: 250,
					rowNum: 10,
					loadonce: true, // this is just for the demo
					pager: "#jqGridPagerForWOList",
					jsonReader: {
						page: function (obj) { return 1; },
						total: function (obj) { return 1; },
						records: function (obj) { return obj.d.length; },
						root: function (obj) { return obj.d; },
						repeatitems: false,
						id: "0"
					},
					caption: "Open Work Order Details"
				});
			}
		</script>
		<script type="text/javascript">
			function FuncLastLogDet(mMachineID) {
				$('#JQGridLogDet').jqGrid({
					url: 'Dashboard.aspx/LogDetails',
					mtype: 'POST',
					datatype: "json",
					postData:
					{
						MachineID: mMachineID //$.trim($("[id*=MachineID]").val())
					},
					serializeGridData: function (postData) {
						return JSON.stringify(postData);
					},
					ajaxGridOptions: { contentType: "application/json" },
					loadonce: true,
					colNames: ['Log No', 'LogDate', 'Log Type', 'Departure', 'Departure Date/Time', 'Arrival', 'Arrival Date/time', 'Airborne Time'],
					colModel: [
						{ name: 'LogTextNo', index: 'LogTextNo', width: 70 },
						{ name: 'DateFormatted', index: 'DateFormatted', width: 70 },
						{ name: 'LogTypeName', index: 'LogTypeName', width: 60 },
						{ name: 'SouPlaceName', index: 'SouPlaceName', width: 90 },
						{ name: 'SouUniverseDateTimeFormatted', index: 'SouUniverseDateTimeFormatted', width: 80 },
						{ name: 'DesPlaceName', index: 'DesPlaceName', width: 80 },
						{ name: 'DesUniverseDateTimeFormatted', index: 'DesUniverseDateTimeFormatted', width: 90 },
						{ name: 'TimeInAir', index: 'TimeInAir', width: 90 }

					],

					viewecords: true, // show the current page, data rang and total records on the toolbar
					width: 520,
					height: 210,
					rowNum: 10,
					exportEnabled: 1,
					// loadonce: true, // this is just for the demo
					pager: "#JQGridLogDetPager",
					jsonReader: {
						page: function (obj) { return 1; },
						total: function (obj) { return 1; },
						records: function (obj) { return obj.d.length; },
						root: function (obj) { return obj.d; },
						repeatitems: true,
						id: "0"
					},
					caption: $("#cmbAircraft :selected").text() + " Tech Log Status (Last 10 Logs)"
				});

			}
		</script>
		<script type="text/javascript">
			function FusionChartLogDetLineFunc(LogDetLineGraphValues) {
				var revenueChart = new FusionCharts({
					"type": "Line",
					"renderAt": "LogDetLineGraph",
					"width": "520",
					"height": "260",
					"dataFormat": "json",
					"dataSource": {
						"chart": {
							"caption": "Airborne Time Last 10 Logs",
							"subCaption": $("#cmbAircraft :selected").text(),
							"xAxisName": "Log Date",
							"yAxisName": "Airborne Time",
							"labelDisplay": "Rotate",
							//Cosmetics
							"exportEnabled": "1",
							"lineThickness": "2",
							"paletteColors": "#0075c2",
							//      "baseFontColor": "#333333",
							// "baseFont": "Helvetica Neue,Arial",
							// "captionFontSize": "14",
							//"subcaptionFontSize": "14",
							//  "subcaptionFontBold": "0",
							//    "showBorder": "0",
							//    "bgColor": "#ffffff",
							"showShadow": "0",
							//"canvasBgColor": "#ffffff",
							//"canvasBorderAlpha": "0",
							"divlineAlpha": "100",
							"divlineColor": "#999999",
							"divlineThickness": "1",
							"divLineIsDashed": "1",
							"divLineDashLen": "1",
							"divLineGapLen": "1",
							"showXAxisLine": "1",
							"xAxisLineThickness": "1",
							"xAxisLineColor": "#999999",
							"showAlternateHGridColor": "0",
							"theme": "zune",

						},
						"data": JSON.parse(LogDetLineGraphValues)

					}

				});
				revenueChart.render();
			}

		</script>
		<script type="text/javascript">
            function FusionChartPierpsMELFunc(PirepsCount, MELCount, MDCount) {
                var revenueChart = new FusionCharts({
                    "type": "MSColumn2D",
                    "renderAt": "MELPirepsChart",
                    "width": "520",
                    "height": "330",
                    "dataFormat": "json",
                    "dataSource": {
                        "chart": {
                            "caption": "Total Activities per Year",
                            "subCaption": $("#cmbYear :selected").text() + ' for ' + $("#cmbAircraft :selected").text(),
                            "xAxisName": "Month",
                            "yAxisName": "Activity Count",
                            "exportEnabled": "1",
                            "theme": "zune"
                        },
                        //"data": JSON.parse(PierpsMELGraphCount)
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
                            "seriesname": "Pireps",
                            "data": JSON.parse(PirepsCount)

                        },
                            {
                                "seriesname": "MEL",
                                "data": JSON.parse(MELCount)

                            },
                            {
                                "seriesname": "MD",
                                "data": JSON.parse(MDCount)

                            }
                        ]
                    }

                });

                revenueChart.render();
            }

		</script>
		<asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
			runat="server">
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
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

                $('.cbSelectRow').change(function () {
                    // detect if the checkbox is checked
                    var checked = $(this).prop('checked');
                    // gets the table row indiect parent
                    var trParent = $(this).parents('tr');
                    // add or remove the css class according to the check state
                    if (checked == true)
                        //                  $("td", $(this).closest("tr")).addClass('clslightColor')
                        $("td", $(this).closest("tr")).addClass('clslightColor');
                    else
                        $("td", $(this).closest("tr")).removeClass('clslightColor');
                })
                    // the each is used when postback is triggered with checked rows
                    .each(function (index, element) {
                        var checked = $(element).prop('checked');
                        if (checked == true)
                            //                    $("td", $(this).closest("tr")).addClass('clslightColor');
                            $("td", $(this).closest("tr")).addClass('clslightColor');
                        else
                            $("td", $(this).closest("tr")).removeClass('clslightColor');
                    });
                // select all click
                $("#chkSelectAll").change(function () {
                    var checked = $(this).prop('checked');
                    $('.cbSelectRow').prop('checked', checked).trigger('change');
                });

            });
		</script>
		<script type="text/javascript">
            function ExpiredItemsReport() {
                $('#JQGridExpiredItemsCountForReport').jqGrid({
                    url: 'Dashboard.aspx/ExpiredItemsCountForReport',
                    datatype: "json",
                    mtype: 'POST',
                    serializeGridData: function (postData) {
                        return JSON.stringify(postData);
                    },
                    ajaxGridOptions: { contentType: "application/json" },
                    loadonce: true,
                    colNames: ['Store', 'Expired', 'Expiry within 7 days', 'Expiry within 15 days', 'Expiry above 15 days'],
                    colModel: [
                        { name: 'StoreName', index: 'StoreName', width: 70 },
                        { name: 'RedCount', index: 'RedCount', width: 40 },
                        { name: 'YellowCount', index: 'YellowCount', width: 90 },
                        { name: 'BlueCount', index: 'BlueCount', width: 80 },
                        { name: 'GreenCount', index: 'GreenCount', width: 80 }
                    ],
                    viewecords: true, // show the current page, data rang and total records on the toolbar
                    width: 520,
                    height: 220,
                    rowNum: 10,
                    loadonce: true, // this is just for the demo
                    pager: "#JQGridExpiredItemsCountPager",
                    jsonReader: {
                        page: function (obj) { return 1; },
                        total: function (obj) { return 1; },
                        records: function (obj) { return obj.d.length; },
                        root: function (obj) { return obj.d; },
                        repeatitems: false,
                        id: "0"
                    },
                    caption: "No. of shelf life inventory due / approaching due between 0 days - 3 months"
                });
            }
		</script>
		<script type="text/javascript">
            function FusionBarChart() {
                FusionCharts.ready(function () {
                    // The getTabularData() returns the categories as well as the datasets, that will be used for creating the charts
                    var getTabularData = function () {
                        var table = document.getElementById('T1'),
                            rows = table.children[0].children,
                            row,
                            i,
                            length,
                            data2011 = [],
                            data2012 = [],
                            data2013 = [],
                            data2014 = []
                        categories = [];
                        // get the table element and iterate over its children and extract the data
                        // start scraping data from the data rows in the table, ignoring the header rows
                        for (i = 1, length = rows.length; i < length; i++) {
                            row = rows[i];

                            categories.push({
                                label: row.children[0].innerHTML
                            });
                            data2011.push({
                                value: row.children[1].innerHTML
                            });
                            data2012.push({
                                value: row.children[2].innerHTML
                            });
                            data2013.push({
                                value: row.children[3].innerHTML
                            });
                            data2014.push({
                                value: row.children[4].innerHTML
                            });
                        }
                        return {
                            categories: categories,
                            dataset: [{
                                seriesname: rows[0].children[1].innerHTML,
                                data: data2011
                            }, {
                                seriesname: rows[0].children[2].innerHTML,
                                data: data2012
                            },
                            {
                                seriesname: rows[0].children[3].innerHTML,
                                data: data2013
                            },
                            {
                                seriesname: rows[0].children[4].innerHTML,
                                data: data2014
                            }]
                        };
                    };
                    var data = getTabularData();
                    // on click, create the chart using the data obtained by calling the getTabularData function
                    var revenueChart = new FusionCharts({
                        type: 'mscolumn2d',
                        renderAt: 'ExpiredItemsInmscolumn2d',
                        width: '520',
                        height: '300',
                        dataFormat: 'json',
                        id: 'revenue-chart',
                        dataSource: {
                            "chart": {
                                "caption": "No. of shelf life inventory due / approaching due",
                                "subCaption": "Between 0 days - 3 months",
                                "xAxisName": "Store",
                                "yAxisName": "Count",
                                "paletteColors": "#ff0000,#ffff00,#0000ff,#009900",
                                "theme": "zune",
                                "exportEnabled": "1",
                                "placeValuesInside": "0",
                                "valuefontcolor": "074868",
                                "rotateValues": "0",
                            },
                            "categories": [{
                                "category": data.categories
                            }],
                            "dataset": data.dataset
                        }
                    });
                    revenueChart.render();
                });
            }
		</script>
		<script type="text/javascript">
            function OpenGreetingsWindow() {
                window.open("wfGreetings.aspx", "Open", "top=30,left=200,width=960,height=690,toolbar=no,menubar=no,location=no,toolbar=no");
                return true;
            }

		</script>
		<script type="text/javascript">
            function ExpiryDateReport() {
                $('#JQGridExpiryDateReport').jqGrid({
                    url: 'Dashboard.aspx/ExpiryDateReport',
                    datatype: "json",
                    mtype: 'POST',
                    serializeGridData: function (postData) {
                        return JSON.stringify(postData);
                    },
                    ajaxGridOptions: { contentType: "application/json" },
                    loadonce: true,
                    colNames: ['', 'Part No.', 'Description', 'Serial No.', 'Store', 'Exp. Info.'],
                    colModel: [
                        {
                            name: 'DateDifference', index: 'DateDifference', width: 10, align: 'left', formatter: function (cellvalue) {
                                var color;
                                var val = cellvalue;
                                if (val <= 0) {
                                    color = 'red';
                                } else if (val > 0 || val <= 7) {
                                    color = '#80BFFF';
                                }
                                return '<span class="cellWithoutBackground" style="background-color:' + color + ';color:' + color + ';font-weight: bold;font-size: 14px;">' + cellvalue + '</span>';
                            }
                        },
                        { name: 'PartName', index: 'PartName', width: 70 },
                        { name: 'PartDescription', index: 'PartDescription', width: 90 },
                        { name: 'SerialNo', index: 'SerialNo', width: 40 },
                        { name: 'StoreNameLocation', index: 'StoreNameLocation', width: 90 },
                        { name: 'ExpInfo', index: 'ExpInfo', width: 80 }
                    ],
                    viewecords: true, // show the current page, data rang and total records on the toolbar
                    width: 520,
                    height: 220,
                    rowNum: 10,
                    loadonce: true, // this is just for the demo
                    pager: "#ExpiryDateReport",
                    jsonReader: {
                        page: function (obj) { return 1; },
                        total: function (obj) { return 1; },
                        records: function (obj) { return obj.d.length; },
                        root: function (obj) { return obj.d; },
                        repeatitems: false,
                        id: "0"
                    },
                    caption: "No. of shelf life inventory due / approaching due between 0 days - 3 months"
                });
            }
		</script>
		<script type="text/javascript">
            function CalibrationDueReport() {
                $('#JQGridCalibrationDue').jqGrid({
                    url: 'Dashboard.aspx/CalibrationDueReport',
                    datatype: "json",
                    mtype: 'POST',
                    serializeGridData: function (postData) {
                        return JSON.stringify(postData);
                    },
                    ajaxGridOptions: { contentType: "application/json" },
                    loadonce: true,
                    colNames: ['', 'Part No.', 'SerialNo', 'Category', 'Last Calib.', 'Due Date', 'Insp. Interval'],
                    colModel: [
                        {
                            name: 'RemainingDays', index: 'RemainingDays', width: 10, align: 'left', formatter: function (cellvalue) {
                                var color;
                                var val = cellvalue;
                                if (val <= 0) {
                                    color = 'red';
                                } else if (val > 0 || val <= 7) {
                                    color = 'yellow';
                                }
                                return '<span class="cellWithoutBackground" style="background-color:' + color + ';color:' + color + ';font-weight: bold;font-size: 14px;">' + cellvalue + '</span>';
                            }
                        },
                        { name: 'ItemName', index: 'ItemName', width: 70 },
                        { name: 'SerialNo', index: 'SerialNo', width: 40 },
                        { name: 'Category', index: 'Category', width: 90 },
                        { name: 'DoneOnDate', index: 'DoneOnDate', width: 80 },
                        { name: 'NextDueDate', index: 'NextDueDate', width: 80 },
                        { name: 'Frequency', index: 'Frequency', width: 20 }
                    ],
                    viewecords: true, // show the current page, data rang and total records on the toolbar
                    width: 500,
                    height: 140,
                    rowNum: 10,
                    loadonce: true, // this is just for the demo
                    pager: "#CalibrationDue",
                    jsonReader: {
                        page: function (obj) { return 1; },
                        total: function (obj) { return 1; },
                        records: function (obj) { return obj.d.length; },
                        root: function (obj) { return obj.d; },
                        repeatitems: false,
                        id: "0"
                    },
                    caption: "Calibration Due Report (Sensitive and Precision Equipment)s"
                });
            }
		</script>
		<script type="text/javascript">
            function MinLevelItemReport() {
                $('#JQGridMinLevelItemReport').jqGrid({
                    url: 'Dashboard.aspx/MinLevelItemReport',
                    datatype: "json",
                    mtype: 'POST',
                    serializeGridData: function (postData) {
                        return JSON.stringify(postData);
                    },
                    ajaxGridOptions: { contentType: "application/json" },
                    loadonce: true,
                    colNames: ['Part No.', 'Description', 'Min. Level', 'Stock Qty.'],
                    colModel: [
                        { name: 'PartName', index: 'PartName', width: 90 },
                        { name: 'PartDescription', index: 'PartDescription', width: 120 },
                        { name: 'MinStockLevel', index: 'MinStockLevel', width: 45 },
                        { name: 'QtyStock', index: 'QtyStock', width: 45 }
                    ],
                    viewecords: true, // show the current page, data rang and total records on the toolbar
                    width: 500,
                    height: 215,
                    rowNum: 10,
                    loadonce: true, // this is just for the demo
                    pager: "#MinLevelItemReport",
                    jsonReader: {
                        page: function (obj) { return 1; },
                        total: function (obj) { return 1; },
                        records: function (obj) { return obj.d.length; },
                        root: function (obj) { return obj.d; },
                        repeatitems: false,
                        id: "0"
                    },
                    caption: "Parts on Minimum Level"
                });
            }
		</script>
		<script type="text/javascript">
            function PendingPurchaseOrders() {
                $('#JQGridPendingPurchaseOrders').jqGrid({
                    url: 'DashBoard.aspx/PendingPurchaseOrders',
                    datatype: "json",
                    mtype: 'POST',
                    serializeGridData: function (postData) {
                        return JSON.stringify(postData);
                    },
                    ajaxGridOptions: { contentType: "application/json" },
                    loadonce: true,
                    colNames: ['Order Date', 'Order No.', 'Supplier', 'Part No.', 'Description', 'Bal. Qty.'],
                    colModel: [
                        { name: 'OrderDate', index: 'OrderDate', width: 70 },
                        { name: 'OrderTextNo', index: 'OrderTextNo', width: 90 },
                        { name: 'SupplierName', index: 'SupplierName', width: 90 },
                        { name: 'PartName', index: 'PartName', width: 90 },
                        { name: 'PartDescription', index: 'PartDescription', width: 90 },
                        { name: 'BalQty', index: 'BalQty', width: 90 }
                    ],
                    viewecords: true, // show the current page, data rang and total records on the toolbar
                    width: 520,
                    height: 230,
                    rowNum: 10,
                    loadonce: true, // this is just for the demo
                    pager: "#PendingPurchaseOrders",
                    jsonReader: {
                        page: function (obj) { return 1; },
                        total: function (obj) { return 1; },
                        records: function (obj) { return obj.d.length; },
                        root: function (obj) { return obj.d; },
                        repeatitems: false,
                        id: "0"
                    },
                    caption: "Pending Purchase Orders"
                });
            }
		</script>
		<script type="text/javascript">
            function PendingPurchaseQuotationItems() {
                $('#JQGridPendingPurchaseQuotationItems').jqGrid({
                    url: 'DashBoard.aspx/GetPendingPurchaseQuotationItem',
                    datatype: "json",
                    mtype: 'POST',
                    serializeGridData: function (postData) {
                        return JSON.stringify(postData);
                    },
                    ajaxGridOptions: { contentType: "application/json" },
                    loadonce: true,
                    colNames: ['Quotation Date', 'Quotation No.', 'Part No.', 'Description', 'Qty.'],
                    colModel: [
                        { name: 'QuotationDateFormatted', index: 'QuotationDateFormatted', width: 70 },
                        { name: 'QuotationTextNo', index: 'QuotationTextNo', width: 90 },
                        { name: 'ItemName', index: 'ItemName', width: 90 },
                        { name: 'ItemDescription', index: 'ItemDescription', width: 90 },
                        { name: 'QuotationQty', index: 'QuotationQty', width: 30 }
                    ],
                    viewecords: true, // show the current page, data rang and total records on the toolbar
                    width: 500,
                    height: 200,
                    rowNum: 10,
                    loadonce: true, // this is just for the demo
                    pager: "#PendingPurchaseQuotationItems",
                    jsonReader: {
                        page: function (obj) { return 1; },
                        total: function (obj) { return 1; },
                        records: function (obj) { return obj.d.length; },
                        root: function (obj) { return obj.d; },
                        repeatitems: false,
                        id: "0"
                    },
                    caption: "Quotation Items Pending For Purchase"
                });
            }
		</script>
		<script type="text/javascript">
            function RequisitionPendingForPurchaseOrder() {
                $('#JQGridRequisitionPendingForPurchaseOrder').jqGrid({
                    url: 'DashBoard.aspx/GetRequisitionPendingForPurchaseOrder',
                    datatype: "json",
                    mtype: 'POST',
                    serializeGridData: function (postData) {
                        return JSON.stringify(postData);
                    },
                    ajaxGridOptions: { contentType: "application/json" },
                    loadonce: true,
                    colNames: ['Req. Date', 'Req. No.', 'Part No.', 'Description', 'Bal. Qty.'],
                    colModel: [
                        { name: 'ReqDateFormatted', index: 'ReqDateFormatted', width: 90 },
                        { name: 'RequisitionNo', index: 'RequisitionNo', width: 90 },
                        { name: 'PartNo', index: 'PartNo', width: 70 },
                        { name: 'Description', index: 'Description', width: 90 },
                        { name: 'OrderBalQty', index: 'OrderBalQty', width: 30 }
                    ],
                    viewecords: true, // show the current page, data rang and total records on the toolbar
                    width: 500,
                    height: 200,
                    rowNum: 10,
                    loadonce: true, // this is just for the demo
                    pager: "#RequisitionPendingForPurchaseOrder",
                    jsonReader: {
                        page: function (obj) { return 1; },
                        total: function (obj) { return 1; },
                        records: function (obj) { return obj.d.length; },
                        root: function (obj) { return obj.d; },
                        repeatitems: false,
                        id: "0"
                    },
                    caption: "Requisition Pending For Purchase Order"
                });
            }
		</script>
		<%--Added By Prashant  22-May-2020 ALL22052020--%>
		<script type="text/javascript">
            function PendingToReceiptsFromOtherStore() {
                $('#JQGridPendingToReceiptsFromOtherStore').jqGrid({
                    url: 'DashBoard.aspx/PendingToReceiptsFromOtherStore',
                    datatype: "json",
                    mtype: 'POST',
                    serializeGridData: function (postData) {
                        return JSON.stringify(postData);
                    },
                    ajaxGridOptions: { contentType: "application/json" },
                    loadonce: true,
                    colNames: ['Issue Date', 'Issue No.', 'Part Number', 'Description', 'Receipt Bal. Qty.'],
                    colModel: [
                        { name: 'IssueDate', index: 'IssueDate', width: 70 },
                        { name: 'IssueNumber', index: 'IssueNumber', width: 90 },
                        { name: 'PartName', index: 'PartName', width: 90 },
                        { name: 'Description', index: 'Description', width: 90 },
                        { name: 'ReceiptBalQty', index: 'ReceiptBalQty', width: 90 }
                    ],
                    viewecords: true, // show the current page, data rang and total records on the toolbar
                    width: 500,
                    height: 200,
                    rowNum: 10,
                    loadonce: true, // this is just for the demo
                    pager: "#PendingToReceiptsFromOtherStore",
                    jsonReader: {
                        page: function (obj) { return 1; },
                        total: function (obj) { return 1; },
                        records: function (obj) { return obj.d.length; },
                        root: function (obj) { return obj.d; },
                        repeatitems: false,
                        id: "0"
                    },
                    caption: "Pending to receipts from another Store(Store Transfer Only)"
                });
            }
		</script>
		<script type="text/javascript">
            function PendingToolsToReceiveFromEmployee() {
                $('#JQGridPendingToolsToReceiveFromEmployee').jqGrid({
                    url: 'DashBoard.aspx/PendingToolsToReceiveFromEmployeeRecords',
                    datatype: "json",
                    mtype: 'POST',
                    serializeGridData: function (postData) {
                        return JSON.stringify(postData);
                    },
                    ajaxGridOptions: { contentType: "application/json" },
                    loadonce: true,
                    colNames: ['Issue Date', 'Issue No.', 'Part Number', 'Employee', 'Store'],
                    colModel: [
                        { name: 'IssueDateFormatted', index: 'IssueDateFormatted', width: 70 },
                        { name: 'IssueTextNo', index: 'IssueTextNo', width: 90 },
                        { name: 'ItemName', index: 'ItemName', width: 90 },
                        { name: 'IssueToEmployeeName', index: 'IssueToEmployeeName', width: 90 },
                        { name: 'FromStoreWithLocation', index: 'FromStoreWithLocation', width: 90 }
                    ],
                    viewecords: true, // show the current page, data rang and total records on the toolbar
                    width: 500,
                    height: 267,
                    rowNum: 10,
                    loadonce: true, // this is just for the demo
                    pager: "#PendingToolsToReceiveFromEmployee",
                    jsonReader: {
                        page: function (obj) { return 1; },
                        total: function (obj) { return 1; },
                        records: function (obj) { return obj.d.length; },
                        root: function (obj) { return obj.d; },
                        repeatitems: false,
                        id: "0"
                    },
                    caption: "Tools Issued to Employee Pending to Receive "
                });
            }
		</script>
		<script type="text/javascript">
            function ReceivedUnserviceablePart() {
                $('#JQGridReceivedUnserviceablePart').jqGrid({
                    url: 'DashBoard.aspx/ReceivedUnserviceablePartRecords',
                    datatype: "json",
                    mtype: 'POST',
                    serializeGridData: function (postData) {
                        return JSON.stringify(postData);
                    },
                    ajaxGridOptions: { contentType: "application/json" },
                    loadonce: true,
                    colNames: ['Date', 'Receipt No.', 'Part No.', 'Serial No.', 'Qty', 'Store', 'Location'],
                    colModel: [
                        { name: 'DateFormatted', index: 'DateFormatted', width: 70 },
                        { name: 'ReceiptNo', index: 'ReceiptNo', width: 90 },
                        { name: 'ItemName', index: 'ItemName', width: 90 },
                        { name: 'SerialNo', index: 'SerialNo', width: 90 },
                        { name: 'DisplayQty', index: 'DisplayQty', width: 30 },
                        { name: 'Store', index: 'Store', width: 70 },
                        { name: 'Location', index: 'Location', width: 70 }
                    ],
                    viewecords: true, // show the current page, data rang and total records on the toolbar
                    width: 500,
                    height: 200,
                    rowNum: 10,
                    loadonce: true, // this is just for the demo
                    pager: "#ReceivedUnserviceablePart",
                    jsonReader: {
                        page: function (obj) { return 1; },
                        total: function (obj) { return 1; },
                        records: function (obj) { return obj.d.length; },
                        root: function (obj) { return obj.d; },
                        repeatitems: false,
                        id: "0"
                    },
                    caption: "Received Unserviceable Part"
                });
            }
		</script>
		<script type="text/javascript">
            function ReceivedFromAircraftAsCoreUnitReturn() {
                $('#JQGridReceivedFromAircraftAsCoreUnitReturn').jqGrid({
                    url: 'DashBoard.aspx/ReceivedFromAircraftAsCoreUnitReturnRecords',
                    datatype: "json",
                    mtype: 'POST',
                    serializeGridData: function (postData) {
                        return JSON.stringify(postData);
                    },
                    ajaxGridOptions: { contentType: "application/json" },
                    loadonce: true,
                    colNames: ['IssueDate', 'Issue No.', 'Part No.', 'Reg. No.'],
                    colModel: [
                        { name: 'IssueDate', index: 'IssueDate', width: 90 },
                        { name: 'IssueNo', index: 'IssueNo', width: 90 },
                        { name: 'ItemName', index: 'ItemName', width: 90 },
                        { name: 'MachineName', index: 'MachineName', width: 70 }
                    ],
                    viewecords: true, // show the current page, data rang and total records on the toolbar
                    width: 500,
                    height: 200,
                    rowNum: 10,
                    loadonce: true, // this is just for the demo
                    pager: "#ReceivedFromAircraftAsCoreUnitReturn",
                    jsonReader: {
                        page: function (obj) { return 1; },
                        total: function (obj) { return 1; },
                        records: function (obj) { return obj.d.length; },
                        root: function (obj) { return obj.d; },
                        repeatitems: false,
                        id: "0"
                    },
                    caption: "Pending to receipt from Aircraft as Core Unit Return"
                });
            }
		</script>
		<script type="text/javascript">
            //Loan Taken but not return records
            function LoanInWardRecord() {
                $('#JQGridLoanInWardRecord').jqGrid({
                    url: 'DashBoard.aspx/LoanInWard',
                    datatype: "json",
                    mtype: 'POST',
                    serializeGridData: function (postData) {
                        return JSON.stringify(postData);
                    },
                    ajaxGridOptions: { contentType: "application/json" },
                    loadonce: true,
                    colNames: ['Date', 'Receipt No.', 'Part No.', 'Qty.', 'From Whom'],
                    colModel: [
                        { name: 'ReceiptDateFormatted', index: 'ReceiptDateFormatted', width: 70 },
                        { name: 'ReceiptNo', index: 'ReceiptNo', width: 90 },
                        { name: 'ItemName', index: 'ItemName', width: 70 },
                        { name: 'ReceiptItemQty', index: 'ReceiptItemQty', width: 30 },
                        { name: 'ReceiptFrom', index: 'ReceiptFrom', width: 90 }
                    ],
                    viewecords: true, // show the current page, data rang and total records on the toolbar
                    width: 500,
                    height: 200,
                    rowNum: 10,
                    loadonce: true, // this is just for the demo
                    pager: "#LoanInWardRecord",
                    jsonReader: {
                        page: function (obj) { return 1; },
                        total: function (obj) { return 1; },
                        records: function (obj) { return obj.d.length; },
                        root: function (obj) { return obj.d; },
                        repeatitems: false,
                        id: "0"
                    },
                    caption: "Loan taken from Supplier/Store/Customer.. but not return"
                });
            }
		</script>
		<%--End of Added By Prashant  22-May-2020 ALL22052020--%>
		<script type="text/javascript">
            function ReOrderLevelItemReport() {
                $('#JQGridReOrderLevelItemReport').jqGrid({
                    url: 'Dashboard.aspx/ReOrderLevelItemReport',
                    datatype: "json",
                    mtype: 'POST',
                    serializeGridData: function (postData) {
                        return JSON.stringify(postData);
                    },
                    ajaxGridOptions: { contentType: "application/json" },
                    loadonce: true,
                    colNames: ['Part No.', 'Description', 'Re-Order Level', 'Stock Qty.'],
                    colModel: [
                        { name: 'PartName', index: 'PartName', width: 90 },
                        { name: 'PartDescription', index: 'PartDescription', width: 120 },
                        { name: 'MinReOrderLevel', index: 'MinReOrderLevel', width: 60 },
                        { name: 'TotalStockQty', index: 'TotalStockQty', width: 45 }
                    ],
                    viewecords: true, // show the current page, data rang and total records on the toolbar
                    width: 500,
                    height: 215,
                    rowNum: 10,
                    loadonce: true, // this is just for the demo
                    pager: "#ReOrderLevelItemReport",
                    jsonReader: {
                        page: function (obj) { return 1; },
                        total: function (obj) { return 1; },
                        records: function (obj) { return obj.d.length; },
                        root: function (obj) { return obj.d; },
                        repeatitems: false,
                        id: "0"
                    },
                    caption: "Parts on Re-Order Level"
                });
            }
		</script>
		<script type="text/javascript">
            function LoanOutWardReport() {
                $('#JQGridLoanOutWardReport').jqGrid({
                    url: 'Dashboard.aspx/LoanOutWardReport',
                    datatype: "json",
                    mtype: 'POST',
                    serializeGridData: function (postData) {
                        return JSON.stringify(postData);
                    },
                    ajaxGridOptions: { contentType: "application/json" },
                    loadonce: true,
                    colNames: ['Issue Date', 'Issue No.', 'Part No.', 'Qty.', 'Issued To'],
                    colModel: [
                        { name: 'IssueDateFormatted', index: 'IssueDateFormatted', width: 70 },
                        { name: 'IssueNo', index: 'IssueNo', width: 90 },
                        { name: 'ItemName', index: 'ItemName', width: 90 },
                        { name: 'IssueItemQty', index: 'IssueItemQty', width: 30 },
                        { name: 'IssueTo', index: 'IssueTo', width: 150 }
                    ],
                    viewecords: true, // show the current page, data rang and total records on the toolbar
                    width: 500,
                    height: 200,
                    rowNum: 10,
                    loadonce: true, // this is just for the demo
                    pager: "#LoanOutWardReport",
                    jsonReader: {
                        page: function (obj) { return 1; },
                        total: function (obj) { return 1; },
                        records: function (obj) { return obj.d.length; },
                        root: function (obj) { return obj.d; },
                        repeatitems: false,
                        id: "0"
                    },
                    caption: "Loan given to Store/Aircraft/Vendor.. but not received back"
                });
            }
		</script>
		<script type="text/javascript">
            function PendingReturnableExchangeRepairIssueToVendorItemReport() {
                $('#JQGridPendingReturnableExchangeRepairIssueToVendorItemReport').jqGrid({
                    url: 'Dashboard.aspx/PendingReturnableExchangeRepairIssueToVendorItemReport',
                    datatype: "json",
                    mtype: 'POST',
                    serializeGridData: function (postData) {
                        return JSON.stringify(postData);
                    },
                    ajaxGridOptions: { contentType: "application/json" },
                    loadonce: true,
                    colNames: ['Issue Date', 'Issue No.', 'Vendor', 'Part No.', 'SerialNo', 'Issued Qty.'],
                    colModel: [
                        { name: 'IssueDate', index: 'IssueDate', width: 90 },
                        { name: 'IssueNumber', index: 'IssueNumber', width: 120 },
                        { name: 'ToVendorName', index: 'ToVendorName', width: 150 },
                        { name: 'PartName', index: 'PartName', width: 90 },
                        { name: 'SerialNo', index: 'SerialNo', width: 90 },
                        { name: 'LoanQty', index: 'LoanQty', width: 80 }
                    ],
                    viewecords: true, // show the current page, data rang and total records on the toolbar
                    width: 500,
                    height: 200,
                    rowNum: 10,
                    loadonce: true, // this is just for the demo
                    pager: "#PendingReturnableExchangeRepairIssueToVendorItemReport",
                    jsonReader: {
                        page: function (obj) { return 1; },
                        total: function (obj) { return 1; },
                        records: function (obj) { return obj.d.length; },
                        root: function (obj) { return obj.d; },
                        repeatitems: false,
                        id: "0"
                    },
                    caption: "Pending Returnable Exchange Repair Issue To Vendor"
                });
            }
		</script>
		<%--Added by Harsh on 25th Jan 2024 For TataSteel Dashboards--%>
		<script type="text/javascript">
            function PreFlightAuthorizationReport() {
                $('#JQGridPreFlightAuthorization').jqGrid({
                    url: 'Dashboard.aspx/PreFlightAuthorizationReport',
                    datatype: "json",
                    mtype: 'POST',
                    serializeGridData: function (postdata) {
                        return JSON.stringify(postdata);
                    },
                    ajaxGridOptions: { contentType: "application/json" },
                    loadonce: true,
                    colNames: ['', 'Employee', 'Designation', 'Document', 'Document No.',
                        'Date of Issue', 'Place of Issue', 'Validity (Months)', 'Expiry Date', 'Issuing Authority', 'Remaninig Days'],
                    colModel: [
                        {
                            name: 'WarningDays', index: 'WarningDays', width: 10, align: 'left', formatter: function (cellvalue) {
                                var color;
                                var val = cellvalue;
                                if (val <= 0) {
                                    color = 'red';
                                } else if (val > 0 || val <= 7) {
                                    color = 'yellow';
                                }
                                return '<span class="cellWithoutBackground" style="background-color:' + color + ';color:' + color + ';font-weight: bold;font-size: 14px;">' + cellvalue + '</span>';
                            }
                        },
                        { name: 'EmployeeName', index: 'EmployeeName', width: 100, align: 'Left' },
                        { name: 'DesignationName', index: 'DesignationName', width: 50, align: 'Left' },
                        { name: 'DocumentName', index: 'DocumentName', width: 105, align: 'Left' },
                        { name: 'DocNo', index: 'DocNo', width: 60, align: 'Left' },
                        { name: 'DateOfIssue', index: 'DateOfIssue', width: 130, align: 'Left' },
                        { name: 'PlaceOfIssue', index: 'PlaceOfIssue', width: 50, align: 'Left' },
                        { name: 'Validity', index: 'Validity', width: 52, align: 'Left' },
                        { name: 'DateOfExpiry', index: 'DateOfExpiry', width: 130, align: 'Left' },
                        { name: 'IssuingAuthority', index: 'IssuingAuthority', width: 50, align: 'Left' },
                        { name: 'WarningDays', index: 'WarningDays', width: 50, align: 'Left' }
                    ],
                    viewecords: true,
                    width: 500,
                    height: 200,
                    rowNum: 10,
                    loadonce: true,
                    pager: '#PreFlightAuthorization',
                    jsonReader: {
                        page: function (obj) { return 1; },
                        total: function (obj) { return 1; },
                        records: function (obj) { return obj.d.length; },
                        root: function (obj) { return obj.d; },
                        repeatitems: false,
                        id: "0"
                    },
                    caption: "Pre-Flight Authorization Details"
                });
            }

		</script>
		<script type="text/javascript">
            function AMECertificationReport() {
                $('#JQGridAMECertification').jqGrid({
                    url: 'Dashboard.aspx/AMECertificationReport',
                    datatype: "json",
                    mtype: 'POST',
                    serializeGridData: function (postdata) {
                        return JSON.stringify(postdata);
                    },
                    ajaxGridOptions: { contentType: "application/json" },
                    loadonce: true,
                    colNames: ['', 'Employee', 'Designation', 'Document', 'Document No.',
                        'Date of Issue', 'Place of Issue', 'Validity (Months)', 'Expiry Date', 'Issuing Authority', 'Remaninig Days'],
                    colModel: [
                        {
                            name: 'WarningDays', index: 'WarningDays', width: 10, align: 'left', formatter: function (cellvalue) {
                                var color;
                                var val = cellvalue;
                                if (val <= 0) {
                                    color = 'red';
                                } else if (val > 0 || val <= 7) {
                                    color = 'yellow';
                                }
                                return '<span class="cellWithoutBackground" style="background-color:' + color + ';color:' + color + ';font-weight: bold;font-size: 14px;">' + cellvalue + '</span>';
                            }
                        },
                        { name: 'EmployeeName', index: 'EmployeeName', width: 100, align: 'Left' },
                        { name: 'DesignationName', index: 'DesignationName', width: 50, align: 'Left' },
                        { name: 'DocumentName', index: 'DocumentName', width: 105, align: 'Left' },
                        { name: 'DocNo', index: 'DocNo', width: 60, align: 'Left' },
                        { name: 'DateOfIssue', index: 'DateOfIssue', width: 130, align: 'Left' },
                        { name: 'PlaceOfIssue', index: 'PlaceOfIssue', width: 50, align: 'Left' },
                        { name: 'Validity', index: 'Validity', width: 52, align: 'Left' },
                        { name: 'DateOfExpiry', index: 'DateOfExpiry', width: 130, align: 'Left' },
                        { name: 'IssuingAuthority', index: 'IssuingAuthority', width: 50, align: 'Left' },
                        { name: 'WarningDays', index: 'WarningDays', width: 50, align: 'Left' }
                    ],
                    viewecords: true,
                    width: 500,
                    height: 200,
                    rowNum: 10,
                    loadonce: true,
                    pager: '#AMECertification',
                    jsonReader: {
                        page: function (obj) { return 1; },
                        total: function (obj) { return 1; },
                        records: function (obj) { return obj.d.length; },
                        root: function (obj) { return obj.d; },
                        repeatitems: false,
                        id: "0"
                    },
                    caption: "AME Certification Details"
                });
            }

		</script>
		<%--End--%>

		<%--Added by Sachin on 25th Jan 2024 For TataSteel Dashboards--%>
		<script type="text/javascript">
			function AircraftCertificate() {
				$('#JQGridAircraftCertificate').jqGrid({
					url: 'Dashboard.aspx/AircraftCertificate',
					datatype: "json",
					mtype: 'POST',

					serializeGridData: function (postData) {

						return JSON.stringify(postData);
					},
					ajaxGridOptions: { contentType: "application/json" },
					loadonce: true,
					colNames: ['Reg No.', 'Name', 'No.', 'Issue Date', 'Expiry Date', 'Remaining Days'],
					colModel: [
						{ name: 'RegNo', index: 'RegNo', width: 100 },
						{ name: 'CertificateName', index: 'CertificateName', width: 190 },
						{ name: 'CertificateNo', index: 'CertificateNo', width: 40 },
						{ name: 'IssueDateFormatted', index: 'IssueDateFormatted', width: 100 },
						{ name: 'ExpiryDateFormatted', index: 'ExpiryDateFormatted', width: 100 },
						{ name: 'RemDays', index: 'RemDays', width: 120 }
					],
					viewecords: true, // show the current page, data rang and total records on the toolbar
					width: 500,
					height: 200,
					rowNum: 10,
					pager: "#AircraftCertificate",
					jsonReader: {
						page: function (obj) { return 1; },
						total: function (obj) { return 1; },
						records: function (obj) { return obj.d.length; },
						root: function (obj) { return obj.d; },
						repeatitems: false,
						id: "0"

					},

					caption: "Aircraft Certificate List"
				});
			}
		</script>
		<%--End--%>
	</form>
</body>
</html>
