<%@ Page EnableEventValidation="false" Language="vb" AutoEventWireup="false" CodeBehind="wfTLPEdit_Ajax.aspx.vb"
	Inherits="Flypal.wfTLPEdit_Ajax" %>

<%@ Import Namespace="Flypal.LogList" %>
<%@ Import Namespace="Flypal.Log" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%--AJAX- Changed DOCTYPE from 4.0 to 1.0--%><%--AJAX- Register "AjaxControlToolkit & User Control "MSGBOX"--%>
<html>
<head runat="server">
	<title>Log Details</title>
	<meta name="vs_showGrid" content="True">
	<meta http-equiv="x-ua-compatible" content="IE=9">
	<script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
	<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
	<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
	<meta name="vs_defaultClientScript" content="JavaScript">
	<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	<link id="MainStyle" type="text/css" rel="stylesheet">
	<asp:PlaceHolder runat="server">
		<%--AJAX- Replaced "LocalFunction.htm" to "LocalFunctionAjax.htm"--%>
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	<style type="text/css">

		.text-right{
			text-align:right;
		}

		#arrowICN {
			cursor: pointer;
		}

		#dropdown-content {
			z-index: 7;
			position: relative;
		}

		.actionICNS {
			height: 15px;
			width: 15px;
		}

		.largerActionICNS {
			height: 20px;
			width: 20px;
		}

		.actionICNSAlignment {
			margin-top: 5px;
		}
	</style>
	<script id="clientEventHandlersJS" language="javascript">
		function openTranDetail() {
			str = "wfReports.aspx"
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openTranDetail1() {
			str = "webform1.aspx"
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openFile() {
			str = "wfFileView.aspx"
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openDetail() {
			str = "wfDetail.aspx"
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
	</script>
	<link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css">
	<script type="text/javascript" src="jquery-1.6.1.min.js"></script>
	<script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
	<script src="StickyNote/js/jquery.cookie.js" type="text/javascript"></script>
</head>
<body bottommargin="5" leftmargin="5" rightmargin="5" topmargin="5">
	<form id="Form1" method="post" runat="server">
		<%--AJAX- Replaced "LocalFunction.htm" to "LocalFunctionAjax.htm"--%>
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
			runat="server">
		</asp:ScriptManager>
		<%--AJAX- Replaced "LocalFunction.htm" to "LocalFunctionAjax.htm"--%>
		<script language="javascript" type="text/javascript">

			var g_CurrentTextBox;
			var g_isTabPressed;

			Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
			function endRequestHandler() {

				try {

					//if (g_isTabPressed == 1) {
					$get(g_CurrentTextBox).focus();
					$get(g_CurrentTextBox).select();

					g_isTabPressed = 0;
					//}


				}
				catch (Error) { }

			}


			function onTextFocus() {
				g_CurrentTextBox = event.srcElement.id;

			}

			function onkeyPressed(keycode, obj) {

				if (keycode == 9) {

					g_isTabPressed = 1;
				}

			}

		</script>
		<%--AJAX- Replaced "LocalFunction.htm" to "LocalFunctionAjax.htm"--%>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<div>
			<table class="clstablelistout" id="tblMain">
				<tr>
					<td>
						<table id="tblinner" class="clsTablelistin" border="0" cellpadding="0">
							<tr>
								<td class="clsFormHeader1Newstyle">
									<table width="100%">
										<tr>
											<td>
												<%--AJAX- ScriptManager Added--%>
												<asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Log Details</asp:Label>
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
											<td align="right">
												<%--AJAX- Add UpdatePanel for APU Grid--%>
												<asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<table>
															<tr>
																<td>
																	<asp:Button ID="btnAddRoute" runat="server" CssClass="clsbtnH clsinfoH" Text="Add Route"
																		ToolTip="Click to Add new Route" Width="111px" />
																</td>
																<td>
																	<asp:Button ID="btnAddNew" runat="server" ToolTip="Click to Save the Log and add New Log"
																		Visible="False" CssClass="clsbtnH clsinfoH" Text="Save &amp; New"></asp:Button>
																</td>
																<td>
																	<asp:Button ID="btnSave" runat="server" ToolTip="Click to Save the Record" CssClass="clsbtnH clsinfoH"
																		Text="Save"></asp:Button>
																</td>
																<td>
																	<asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH" Text="Print" CausesValidation="False"
																		Visible="False"></asp:Button>
																</td>
																<td>
																	<asp:Button ID="btnBack" runat="server" ToolTip="Back to Previous Page" CssClass="clsbtnH clsinfoH"
																		Text="Back"></asp:Button>
																</td>
															</tr>
														</table>
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<%--AJAX- New function added as Focus gets Lost when we use tabs in Grid--%>
									<asp:UpdatePanel ID="upnlTabs" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table width="100%">
												<tr>
													<td>
														<table>
															<tr>
																<td>
																	<asp:Label ID="lblLogDetails" runat="server" CssClass="clsLabelButton" ToolTip="Log details">Log details</asp:Label>
																</td>
															</tr>
														</table>
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td>
									<%--AJAX- Add MSGBox Control--%>
									<asp:UpdatePanel ID="upnlErrorList" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
												HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
											<asp:CustomValidator ID="cvRemark" runat="server" ErrorMessage="Remark Can't be greater than 200 chars"
												ControlToValidate="txtRemark" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
											<asp:CustomValidator ID="cvAirFrame" runat="server" Display="None" OnServerValidate="customvalidate1"></asp:CustomValidator>
											<asp:CustomValidator ID="cvGroundRunTime" runat="server" ErrorMessage="Departure date should be in date time format."
												ControlToValidate="txtGroundRunTime" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
											<asp:CustomValidator ID="cvAirBornTime" runat="server" ErrorMessage="Not be Nigative."
												ControlToValidate="txtAirBorneTime" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
											<asp:CustomValidator ID="cvPilot1" runat="server" ErrorMessage="Enter correct Pilot1 name."
												ControlToValidate="Pilot1" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
											<asp:CustomValidator ID="cvPilot2" runat="server" ErrorMessage="Enter correct Pilot2 name."
												ControlToValidate="Pilot2" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
											<asp:CustomValidator ID="cvPlace1" runat="server" ErrorMessage="Enter correct Source name."
												ControlToValidate="Place1" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
											<asp:CustomValidator ID="cvPlace2" runat="server" ErrorMessage="Enter correct Destination name."
												ControlToValidate="Place2" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
											<asp:CustomValidator ID="cvTLPNo" runat="server" ErrorMessage="Enter TLP No." ControlToValidate="txtLogPageNo"
												Display="None" OnServerValidate="customvalidate" ValidateEmptyText="true"></asp:CustomValidator>
											<asp:CustomValidator ID="cvFlightClassification" runat="server" ControlToValidate="cmbFlightLogClassification"
												Display="None" OnServerValidate="customvalidate" ErrorMessage="Please Select Classification."></asp:CustomValidator>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td style="width: 100%">
									<%--AJAX- Add UpdatePanel for lblTitle Page--%>
									<asp:UpdatePanel ID="upnlLogDetails" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<fieldset id="Fieldset3" class="clsFieldSetNewStyle">
												<table width="100%">
													<tr>
														<td>
															<table>
																<tr>
																	<td>
																		<asp:Label ID="lblCalDate" runat="server" CssClass="clsLabelStar">*</asp:Label>
																	</td>
																	<td>
																		<asp:Label ID="lblDateTime" runat="server" CssClass="clsLabelAuto">Date</asp:Label>
																	</td>
																	<td>
																		<asp:TextBox runat="server" ID="calDateTime" CssClass="clsTextBoxTagSearchDate" Width="100px"
																			AutoPostBack="true" onchange="ValidateDateText(this,'DateTime_watermarkextender','true');"></asp:TextBox>
																		<cc2:CalendarExtender ID="calDateTime_CalendarExtender" runat="server" CssClass="cal_Theme1"
																			Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="calDateTime"></cc2:CalendarExtender>
																		<cc2:TextBoxWatermarkExtender TargetControlID="calDateTime" ID="DateTime_watermarkextender"
																			ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
																	</td>
																	<td>
																		<asp:Label ID="lblLogNo" runat="server" CssClass="clsLabelAuto">Log No.</asp:Label>
																	</td>
																	<td>
																		<asp:TextBox ID="txtLogText" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Log Number"
																			Text="<%# mLog.LogText %>" ReadOnly="True" BackColor="#E0E0E0"></asp:TextBox>
																		<asp:TextBox ID="txtLogNo" runat="server" CssClass="clsTextBoxTagSearchSmall" Text="<%# mLog.LogNo %>"
																			ReadOnly="True" BackColor="#E0E0E0"></asp:TextBox>
																	</td>
																</tr>
																<tr>
																	<td>
																		<asp:Label ID="lblPilotStar1" runat="server" CssClass="clsLabelStar" Visible="<%#Not mLog.IsHobbs %>">*</asp:Label>
																	</td>
																	<td>
																		<asp:Label ID="lblPilotComm" runat="server" CssClass="clsLabelAuto">Pilot in Command</asp:Label>
																	</td>
																	<td colspan="3">
																		<asp:TextBox ID="Pilot1" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mLog.Pilot1Name %>"
																			Width="200px"></asp:TextBox>
																	</td>
																</tr>
																<tr>
																	<td></td>
																	<td>
																		<span id="lblAttachFile" class="clsLabel">Attach File</span>
																	</td>
																	<td colspan="3">
																		<asp:UpdatePanel ID="upnlFileupload" runat="server" UpdateMode="Conditional">
																			<ContentTemplate>
																				<table border="0" cellpadding="0" cellspacing="0">
																					<tr>
																						<td>
																							<input type="button" id="btnSelectFile" value="Select File" style="width: 100px;"
																								runat="server" class="clsbtnH clsinfoH1" causesvalidation="False" tabindex="13" />
																						</td>
																						<td style="padding-left: 3px;">
																							<asp:Button ID="btnDelAttach" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Remove Attachment"
																								Text="Remove Attachment" Enabled="False" TabIndex="14"></asp:Button>
																						</td>
																						<td style="padding-left: 2px;">
																							<asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
																								Height="20px" Width="15px" TabIndex="15"></asp:ImageButton>
																						</td>
																					</tr>
																				</table>
																			</ContentTemplate>
																		</asp:UpdatePanel>
																	</td>
																</tr>
																<tr>
																	<td>&nbsp;
																	</td>
																	<td>
																		<asp:Label ID="lblDepPlace" runat="server" CssClass="clsLabelAuto">Departure Place</asp:Label>
																	</td>
																	<td colspan="3">
																		<asp:TextBox ID="Place1" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearch"
																			ReadOnly="True" Text="<%# mLog.SourceName %>" Width="250px"></asp:TextBox>
																	</td>
																</tr>
															</table>
														</td>
														<td>
															<table width="100%">
																<tr>
																	<td>
																		<asp:Label ID="Label3" runat="server" CssClass="clsLabelStar">*</asp:Label>
																	</td>
																	<td>
																		<asp:Label ID="lblLogPageNo" runat="server" CssClass="clsLabelAuto">TLP No.</asp:Label>
																	</td>
																	<td>
																		<asp:TextBox ID="txtLogPageNo" runat="server" CssClass="clsTextBoxTagSearchSmall" ToolTip="Enter Log Page No."
																			Text="<%# mLog.LogPageNoFormatted %>" MaxLength="9"></asp:TextBox>
																	</td>
																</tr>
																<tr>
																	<td></td>
																	<td>
																		<asp:Label ID="lblCo" runat="server" CssClass="clsLabelAuto">Co-Pilot</asp:Label>
																	</td>
																	<td>
																		<asp:TextBox ID="Pilot2" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mLog.Pilot2Name %>"
																			Width="250px"></asp:TextBox>
																		<asp:ImageButton ID="btnAddPilot" runat="server" CausesValidation="False" Height="22px"
																			Visible="false" ImageUrl="~/images/plus1.png" ToolTip="Click to Add new pilot"
																			Width="24px" CssClass="clsbtnH clsinfoH" />
																	</td>
																</tr>
																<tr>
																	<td>
																		<asp:Label ID="lblClassificationStar" runat="server" CssClass="clsLabelStar" Visible="false">*</asp:Label>
																	</td>
																	<td>
																		<asp:Label ID="lblFlightLogClassification" runat="server" CssClass="clsLabelAuto">Classification</asp:Label>
																	</td>
																	<td>
																		<asp:DropDownList ID="cmbFlightLogClassification" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
																			Width="258px" DataTextField="Name" DataValueField="ID">
																		</asp:DropDownList>
																		<asp:ImageButton ID="btnFlightLogClassification" runat="server" CausesValidation="False"
																			Visible="false" Height="22px" ImageUrl="~/images/plus1.png" ToolTip="Click to Add new Classification"
																			Width="24px" CssClass="clsbtnH clsinfoH" />
																	</td>
																</tr>
																<tr>
																	<td></td>
																	<td>
																		<asp:Label ID="lblArrPlace" runat="server" CssClass="clsLabelAuto">Arrival Place</asp:Label>
																	</td>
																	<td>
																		<asp:TextBox ID="Place2" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearch"
																			ReadOnly="True" Text="<%# mLog.DestinationName %>" Width="250px"></asp:TextBox>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
												</table>
											</fieldset>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td>
									<br />
								</td>
							</tr>
							<tr>
								<td>
									<asp:Label ID="lblTLPGridTitle" runat="server" CssClass="clsLabelHeader">TLP Details</asp:Label>
								</td>
							</tr>
							<tr>
								<td>
									<%--AJAX- Add UpdatePanel for tabs buttons --%>
									<asp:UpdatePanel ID="upnlLogDetailsGrid" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:DataGrid ID="dgLogDetails" runat="server" AutoGenerateColumns="False"
												CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
												<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
												<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
												<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
												<Columns>
													<asp:BoundColumn Visible="False" DataField="ID" HeaderText="ID "></asp:BoundColumn>
													<asp:BoundColumn DataField="SrNo" HeaderText="Sr No."></asp:BoundColumn>
													<asp:BoundColumn DataField="FlightNo" SortExpression="FlightNo" HeaderText="Flight No."></asp:BoundColumn>
													<asp:BoundColumn DataField="SourceName" SortExpression="SourceName" HeaderText="From"></asp:BoundColumn>
													<asp:BoundColumn DataField="DestinationName" SortExpression="DestinationName" HeaderText="To"></asp:BoundColumn>
													<asp:BoundColumn DataField="SouLocalDateTimeFormatted" SortExpression="SouLocalDateTimeFormatted"
														HeaderText="Chocks Off">
														<ItemStyle Wrap="false" />
													</asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="SouUniverseDateTimeFormatted" SortExpression="SouUniverseDateTimeFormatted"
														HeaderText="UTC Chocks Off">
														<ItemStyle Wrap="false" />
													</asp:BoundColumn>
													<asp:BoundColumn DataField="DesLocalDateTimeFormatted" SortExpression="DesLocalDateTimeFormatted"
														HeaderText="Chocks On">
														<ItemStyle Wrap="False"></ItemStyle>
													</asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="DesUniverseDateTimeFormatted" SortExpression="DesUniverseDateTimeFormatted"
														HeaderText="UTC Chocks On">
														<ItemStyle Wrap="false" />
													</asp:BoundColumn>
													<asp:BoundColumn DataField="BlockTime" HeaderText="Block Time"></asp:BoundColumn>
													<asp:BoundColumn DataField="TakeOffLocalDateTimeFormatted" SortExpression="TakeOffLocalDateTimeFormatted"
														HeaderText="Take Off">
														<ItemStyle Wrap="false" />
													</asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="TakeOffUniverseDateTimeFormatted" SortExpression="TakeOffUniverseDateTimeFormatted"
														HeaderText="UTC Take Off">
														<ItemStyle Wrap="false" />
													</asp:BoundColumn>
													<asp:BoundColumn DataField="TouchDownLocalDateTimeFormatted" SortExpression="TouchDownLocalDateTimeFormatted"
														HeaderText="Touch Down">
														<ItemStyle Wrap="false" />
													</asp:BoundColumn>
													<asp:BoundColumn Visible="False" DataField="TouchDownUniverseDateTimeFormatted" SortExpression="TouchDownUniverseDateTimeFormatted"
														HeaderText="UTC Touch Down">
														<ItemStyle Wrap="false" />
													</asp:BoundColumn>
													<asp:BoundColumn DataField="TimeInAir" HeaderText="Flight Time"></asp:BoundColumn>
													<asp:BoundColumn DataField="Landings" SortExpression="Landings" HeaderText="Landings"></asp:BoundColumn>
													<asp:BoundColumn DataField="FuelOnDeparture" HeaderText="Fuel Dep."></asp:BoundColumn>
													<asp:BoundColumn DataField="FuelUplifted" HeaderText="Fuel Add."></asp:BoundColumn>
													<asp:BoundColumn DataField="FuelOnArrival" HeaderText="Fuel Arr."></asp:BoundColumn>
													<asp:BoundColumn DataField="Pax" HeaderText="Pax"></asp:BoundColumn>
													<asp:BoundColumn DataField="CargoWeight" HeaderText="Cargo"></asp:BoundColumn>
													<asp:BoundColumn DataField="TakeOffWeight" HeaderText="Take Off Weight"></asp:BoundColumn>
													<asp:TemplateColumn HeaderText="Action" ItemStyle-HorizontalAlign="Center"
														HeaderStyle-HorizontalAlign="Center">
														<ItemTemplate>
															<div id="dropDownImg" class="dropdown">
																<asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn" />
																<div id="dropdownICN-content" class="dropdownbtn-content">
																	<table id="dropdown-content" class="clsGridNew_Ajax">
																		<tr>
																			<td>
																				<asp:ImageButton ID="editICN" class="actionICNS" runat="server"
																					CommandArgument='<%# Eval("SrNo") %>'
																					ToolTip="Click to Edit record" CausesValidation="false"
																					CommandName="Edit" ImageUrl="~/images/edit.png" />
																			</td>
																			<td>
																				<asp:ImageButton ID="deleteICN" class="actionICNS" runat="server"
																					CommandArgument='<%# Eval("SrNo") %>' Visible='<%# mLog.LogDetails.Count > 1 %>'
																					ToolTip="Click to Delete record" CausesValidation="false"
																					CommandName="Remove" ImageUrl="~/images/delete.png" />
																			</td>
																		</tr>
																	</table>
																</div>
															</div>
														</ItemTemplate>
													</asp:TemplateColumn>
												</Columns>
											</asp:DataGrid>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td>
									<br />
								</td>
							</tr>
							<%--AJAX- Add UpdatePanel for ValidationSummary or ErrorList --%>
							<tr>
								<td>
									<%--AJAX- Add UpdatePanel for log Details --%>
									<asp:UpdatePanel ID="upnlFlightSummary" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<fieldset id="Fieldset1" class="clsFieldSetNewStyle">
												<legend id="Legend3"><b>Aircraft Flying Hours as per Flight Log book or HOBBS</b></legend>
												<table width="100%">
													<tr>
														<td>
															<table width="100%">
																<tr>
																	<td>
																		<asp:Panel ID="pnlHours" runat="server" CssClass="clsPanel1" Visible="False">
																			<table>
																				<tr>
																					<td>
																						<asp:Label ID="lblTotalBlockTime" runat="server" CssClass="clsLabelAuto">Block Time</asp:Label>
																					</td>
																					<td>
																						<asp:TextBox ID="txtBlockTime" runat="server" BackColor="Gainsboro" CssClass="clsTextBoxTagSearchSmall"
																							Enabled="False" Visible="False"></asp:TextBox>
																					</td>
																					<td>
																						<asp:Label ID="lblAirBorneTime" runat="server" CssClass="clsLabelAuto">Airborne 
                                                                                Time </asp:Label>
																					</td>
																					<td>
																						<asp:TextBox ID="txtAirBorneTime" runat="server" CssClass="clsTextBoxTagSearchSmall"
																							ReadOnly="<%# mLog.ShowTimeTextBoxes Or Not mLog.IsNew %>" Text="<%# mLog.TimeInAir %>"
																							Visible="False"></asp:TextBox>
																					</td>
																					<td>
																						<asp:Label ID="lblGroundRunTime" runat="server" CssClass="clsLabelAuto">Ground 
                                                                                Run Time </asp:Label>
																					</td>
																					<td>
																						<asp:TextBox ID="txtGroundRunTime" runat="server" CssClass="clsTextBoxTagSearchSmall"
																							ReadOnly="<%# mLog.ShowTimeOnGround Or Not mLog.IsNew %>" Text="<%# mLog.TimeOnGround %>"
																							Visible="False"></asp:TextBox>
																					</td>
																					<td>
																						<asp:Label ID="lblPercentTimeOnGround" runat="server" CssClass="clsLabelAuto">%Ground 
                                                                                Run Time </asp:Label>
																					</td>
																					<td>
																						<asp:TextBox ID="txtPercentTimeOnGround" runat="server" CssClass="clsTextBoxTagSearchSmall"
																							ReadOnly="<%# Not mLog.IsNew %>" Text="<%# mLog.PercentTimeOnGround %>" Visible="False"></asp:TextBox>
																					</td>
																					<td>
																						<asp:Label Style="z-index: 0" ID="lblTotalLandings" runat="server" CssClass="clsLabelAuto">Total Landings</asp:Label>
																					</td>
																					<td>
																						<asp:TextBox Style="z-index: 0" ID="txtTotalLandings" runat="server" CssClass="clsTextBoxTagSearchSmall"
																							Text="<%# mLog.TotalLandings %>" ReadOnly="<%# mlog.ShowTimeOnGround Or Not mLog.IsNew %>"
																							Visible="False"></asp:TextBox>
																					</td>
																				</tr>
																			</table>
																		</asp:Panel>
																	</td>
																</tr>
																<tr>
																	<td>
																		<asp:Panel ID="pnlDecimal" runat="server" CssClass="clsPanel1" Visible="False">
																			<table>
																				<tr>
																					<td>
																						<table>
																							<tr>
																								<td>
																									<asp:Label ID="lblHobbsread" runat="server" CssClass="clsLabelAuto">HOBBS READING :  </asp:Label>
																								</td>
																								<td>
																									<asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto">Previous Value :</asp:Label>
																								</td>
																								<td>
																									<asp:Label ID="lblHobbsPrevVal" runat="server" CssClass="clsLabelauto">Offset
																									</asp:Label>
																								</td>
																								<td>
																									<asp:TextBox ID="txtPrevHobbsOffset" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearchSmall"
																										ReadOnly="True" Text="<%# mLog.PrevHobbsOffsetValue %>" Visible="False"></asp:TextBox>
																								</td>
																								<td>
																									<asp:Label ID="lblHobbsCurrentReading" runat="server" CssClass="clsLabelauto">Reading
																									</asp:Label>
																								</td>
																								<td>
																									<asp:TextBox ID="txtPrevHobbsValue" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearchSmall"
																										ReadOnly="True" Text="<%# mLog.PrevHobbsValue %>" Visible="False"></asp:TextBox>
																								</td>
																							</tr>
																						</table>
																						<%-- </fieldset>--%>
																					</td>
																					<td>
																						<%-- <fieldset style="padding: 4px; height: 50px;">
                                                                                  
                                                                                    <legend><b>Current Value</b></legend>--%>
																						<table>
																							<tr>
																								<td>
																									<asp:Label ID="Label2" runat="server" CssClass="clsLabelAuto">Current Value :</asp:Label>
																								</td>
																								<td>
																									<asp:Label ID="lblOffsetPreVal" runat="server" CssClass="clsLabelauto">Offset
																									</asp:Label>
																								</td>
																								<td>
																									<asp:TextBox ID="txtCurrentHobbsOffset" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxTagSearchSmall"
																										ReadOnly="True" Text="<%# mLog.CurrentHobbsOffsetValue %>" Visible="False"></asp:TextBox>
																								</td>
																								<td>
																									<asp:Label ID="lblOffsetCurrentVal" runat="server" CssClass="clsLabelauto">Reading
																									</asp:Label>
																								</td>
																								<td>
																									<asp:TextBox ID="txtCurrentHobbsValue" runat="server" CssClass="clsTextBoxTagSearchSmall"
																										Text="<%# mLog.CurrentHobbsValue %>" Visible="False"></asp:TextBox>
																								</td>
																							</tr>
																						</table>
																						<%-- </fieldset>--%>
																					</td>
																				</tr>
																			</table>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
														<td rowspan="2" align="right">
															<table>
																<tr>
																	<td>
																		<asp:Label ID="lblTotalTime" runat="server" CssClass="clsLabelAuto">Total Time</asp:Label>
																	</td>
																	<td>
																		<asp:TextBox ID="txtTotalTime" runat="server" CssClass="clsTextBoxTagSearchSmall" Text="<%# mLog.TotalTime %>"
																			ReadOnly="True" BackColor="#E0E0E0" ForeColor="Black"></asp:TextBox>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
												</table>
											</fieldset>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td height="17px">&nbsp;
								</td>
							</tr>
							<tr>
								<td>
									<table width="100%">
										<tr>
											<td>
												<asp:Label ID="lblAirframePeriod" runat="server" CssClass="clsLabelHeader" Height="17px">Airframe Period</asp:Label>
											</td>
											<td align="right">
												<asp:LinkButton ID="lnkAllAssembly" runat="server" CssClass="clsLinkButton" Font-Italic="true"
													Font-Size="9pt" ToolTip="Click to go on All Assembly screen" ClientIDMode="Static"
													Visible="<%#  (mLog.IsShowAssemblyRequired) %>">Show All Assembly</asp:LinkButton>
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<%-- </fieldset>--%>
									<asp:UpdatePanel ID="upnlAirframeDetail" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:GridView ID="dgAFPeriods" runat="server" AutoGenerateColumns="False" Width="100%"
												BorderStyle="Solid" CssClass="clsGridNewStyle" GridLines="Horizontal"
												CellPadding="5" AlternatingRowStyle-CssClass="alt" RowStyle-Wrap="false" HeaderStyle-Wrap="false"
												SelectedRowStyle-BackColor="ButtonShadow" ShowHeaderWhenEmpty="True" PageSize="3">
												<AlternatingRowStyle CssClass="clsdgAltItem" />
												<RowStyle CssClass="clsdgItem" />
												<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
												<SelectedRowStyle BackColor="ControlDark" />
												<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
												<PagerSettings Mode="NextPreviousFirstLast" FirstPageText="First" LastPageText="Last" />
												<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
												<Columns>
													<asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
													<asp:BoundField DataField="ModelName" HeaderText="Model">
														<HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="150px" />
														<ItemStyle HorizontalAlign="Left" Wrap="false" Width="150px" />
													</asp:BoundField>
													<asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
														<HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="100px" />
														<ItemStyle HorizontalAlign="Left" Wrap="false" Width="100px" />
													</asp:BoundField>
													<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Hours">
														<ItemTemplate>
															<asp:TextBox ID="txtAirFrameHours" runat="server" CssClass="clsTextBoxTagSearchSmall text-right"
																Width="93%" ReadOnly="<%# Not mLog.IsNew %>" Text='<%# DataBinder.Eval(Container.DataItem, "Hours") %>'
																ToolTip="Enter the Hours." AutoPostBack="true" OnTextChanged="txtAirFrameHours_TextChanged"
																onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"></asp:TextBox>
														</ItemTemplate>
														<HeaderStyle HorizontalAlign="Right" Width="75px" />
														<ItemStyle HorizontalAlign="Right" Width="75px" />
													</asp:TemplateField>
													<asp:BoundField DataField="FinalHours" HeaderText="Final Hours">
														<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
														<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
													</asp:BoundField>
													<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Landings">
														<ItemTemplate>
															<asp:TextBox ID="txtAirFrameLandings" runat="server" CssClass="clsTextBoxTagSearchSmall"
																Width="93%" Text='<%# DataBinder.Eval(Container.DataItem, "Landings") %>' ToolTip="Enter the Landing."
																AutoPostBack="true" OnTextChanged="txtAirFrameLandings_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																onfocus="onTextFocus();">
															</asp:TextBox>
														</ItemTemplate>
														<HeaderStyle HorizontalAlign="Right" Width="75px" />
														<ItemStyle HorizontalAlign="Right" Width="75px" />
													</asp:TemplateField>
													<asp:BoundField DataField="FinalLandings" HeaderText="Final Landings">
														<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
														<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
													</asp:BoundField>
													<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Cycles">
														<ItemTemplate>
															<asp:TextBox ID="txtAirFrameCycles" runat="server" CssClass="clsTextBoxTagSearchSmall text-right"
																Width="93%" Text='<%# DataBinder.Eval(Container.DataItem, "Cycles") %>' ToolTip="Enter Cycles."
																AutoPostBack="true" OnTextChanged="txtAirFrameCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																onfocus="onTextFocus();">
															</asp:TextBox>
														</ItemTemplate>
														<HeaderStyle HorizontalAlign="Right" Width="75px" />
														<ItemStyle HorizontalAlign="Right" Width="75px" />
													</asp:TemplateField>
													<asp:BoundField DataField="FinalCycles" HeaderText="Final Cycles">
														<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
														<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
													</asp:BoundField>
													<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Starts">
														<ItemTemplate>
															<asp:TextBox ID="txtAirFrameStarts" runat="server" CssClass="clsTextBoxTagSearchSmall"
																Width="93%" Text='<%# DataBinder.Eval(Container.DataItem, "Starts") %>' ToolTip="Enter Start Time."
																AutoPostBack="true" OnTextChanged="txtAirFrameStarts_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																onfocus="onTextFocus();">
															</asp:TextBox>
														</ItemTemplate>
														<HeaderStyle HorizontalAlign="Right" Width="75px" />
														<ItemStyle HorizontalAlign="Right" Width="75px" />
													</asp:TemplateField>
													<asp:BoundField DataField="FinalStarts" HeaderText="Final Starts">
														<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
														<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
													</asp:BoundField>
													<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="NG Cycles">
														<ItemTemplate>
															<asp:TextBox ID="txtAirFrameNGCycles" runat="server" CssClass="clsTextBoxTagSearchSmall"
																Width="93%" Text='<%# DataBinder.Eval(Container.DataItem, "NGCycles") %>' ToolTip="Enter NG Cycles"
																AutoPostBack="true" OnTextChanged="txtAirFrameNGCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																onfocus="onTextFocus();">
															</asp:TextBox>
														</ItemTemplate>
														<HeaderStyle HorizontalAlign="Right" Width="75px" />
														<ItemStyle HorizontalAlign="Right" Width="75px" />
													</asp:TemplateField>
													<asp:BoundField DataField="FinalNGCycles" HeaderText="Final NG Cycles">
														<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
														<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
													</asp:BoundField>
													<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="NF Cycles">
														<ItemTemplate>
															<asp:TextBox ID="txtAirFrameNFCycles" runat="server" CssClass="clsTextBoxTagSearchSmall"
																Width="93%" Text='<%# DataBinder.Eval(Container.DataItem, "NFCycles") %>' ToolTip="Enter NF Cycles"
																AutoPostBack="true" OnTextChanged="txtAirFrameNFCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																onfocus="onTextFocus();">
															</asp:TextBox>
														</ItemTemplate>
														<HeaderStyle HorizontalAlign="Right" Width="75px" />
														<ItemStyle HorizontalAlign="Right" Width="75px" />
													</asp:TemplateField>
													<asp:BoundField DataField="FinalNFCycles" HeaderText="Final NF Cycles">
														<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
														<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
													</asp:BoundField>
													<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="RINS">
														<ItemTemplate>
															<asp:TextBox ID="txtAirFrameRins" runat="server" CssClass="clsTextBoxTagSearchSmall"
																Width="93%" Text='<%# DataBinder.Eval(Container.DataItem, "RINS") %>' ToolTip="Enter RINS"
																AutoPostBack="true" OnTextChanged="txtAirFrameRins_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																onfocus="onTextFocus();">
															</asp:TextBox>
														</ItemTemplate>
														<HeaderStyle HorizontalAlign="Right" Width="75px" />
														<ItemStyle HorizontalAlign="Right" Width="75px" />
													</asp:TemplateField>
													<asp:BoundField DataField="FinalRINS" HeaderText="Final RINS">
														<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
														<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
													</asp:BoundField>
													<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Bleeds">
														<ItemTemplate>
															<asp:TextBox ID="txtAirFrameBleeds" runat="server" CssClass="clsTextBoxTagSearchSmall"
																Width="93%" Text='<%# DataBinder.Eval(Container.DataItem, "Bleeds") %>' ToolTip="Enter Bleeds"
																AutoPostBack="true" OnTextChanged="txtAirFrameBleeds_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																onfocus="onTextFocus();">
															</asp:TextBox>
														</ItemTemplate>
														<HeaderStyle HorizontalAlign="Right" Width="75px" />
														<ItemStyle HorizontalAlign="Right" Width="75px" />
													</asp:TemplateField>
													<asp:BoundField DataField="FinalBleeds" HeaderText="Final Bleeds">
														<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
														<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
													</asp:BoundField>
													<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Impeller Cycles">
														<ItemTemplate>
															<asp:TextBox ID="txtAirFrameImpellerCycles" runat="server" CssClass="clsTextBoxTagSearchSmall"
																Width="93%" Text='<%# DataBinder.Eval(Container.DataItem, "ImpellerCycles") %>'
																ToolTip="Enter Impeller Cycles" AutoPostBack="true" OnTextChanged="txtAirFrameImpellerCycles_TextChanged"
																onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
															</asp:TextBox>
														</ItemTemplate>
														<HeaderStyle HorizontalAlign="Right" Width="75px" />
														<ItemStyle HorizontalAlign="Right" Width="75px" />
													</asp:TemplateField>
													<asp:BoundField DataField="FinalImpellerCycles" HeaderText="Final Impeller Cycles">
														<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
														<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
													</asp:BoundField>
													<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="CT Cycles">
														<ItemTemplate>
															<asp:TextBox ID="txtAirFrameCTCycles" runat="server" CssClass="clsTextBoxTagSearchSmall"
																Width="93%" Text='<%# DataBinder.Eval(Container.DataItem, "CTCycles") %>' ToolTip="Enter CT Cycles"
																AutoPostBack="true" OnTextChanged="txtAirFrameCTCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																onfocus="onTextFocus();">
															</asp:TextBox>
														</ItemTemplate>
														<HeaderStyle HorizontalAlign="Right" Width="75px" />
														<ItemStyle HorizontalAlign="Right" Width="75px" />
													</asp:TemplateField>
													<asp:BoundField DataField="FinalCTCycles" HeaderText="Final CT Cycles">
														<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
														<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
													</asp:BoundField>
													<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="PT Cycles">
														<ItemTemplate>
															<asp:TextBox ID="txtAirFramePTCycles" runat="server" CssClass="clsTextBoxTagSearchSmall"
																Width="93%" Text='<%# DataBinder.Eval(Container.DataItem, "PTCycles") %>' ToolTip="Enter PT Cycles"
																AutoPostBack="true" OnTextChanged="txtAirFramePTCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																onfocus="onTextFocus();">
															</asp:TextBox>
														</ItemTemplate>
														<HeaderStyle HorizontalAlign="Right" Width="75px" />
														<ItemStyle HorizontalAlign="Right" Width="75px" />
													</asp:TemplateField>
													<asp:BoundField DataField="FinalPTCycles" HeaderText="Final PT Cycles">
														<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
														<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
													</asp:BoundField>
													<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Generator Mods">
														<ItemTemplate>
															<asp:TextBox ID="txtAirframeGeneratorMods" runat="server" CssClass="clsTextBoxTagSearchSmall"
																Width="93%" Text='<%# DataBinder.Eval(Container.DataItem, "GeneratorMods") %>'
																ToolTip="Enter the Generator Mods." AutoPostBack="true" OnTextChanged="txtAirframeGeneratorMods_TextChanged"
																onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
															</asp:TextBox>
														</ItemTemplate>
														<HeaderStyle HorizontalAlign="Right" Width="75px" />
														<ItemStyle HorizontalAlign="Right" Width="75px" />
													</asp:TemplateField>
													<asp:BoundField DataField="FinalGeneratorMods" HeaderText="Final Generator Mods">
														<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
														<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
													</asp:BoundField>
													<asp:BoundField HeaderText=""></asp:BoundField>
												</Columns>												
											</asp:GridView>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td height="17px">&nbsp; &nbsp;
								</td>
							</tr>
							<tr>
								<td>
									<%-- </fieldset>--%>
									<asp:UpdatePanel ID="upnlEngineDetail" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<div style="width: 100%">
												<asp:Label ID="lblEnginePeriod" runat="server" CssClass="clsLabelHeader" Height="17px">Engine Period</asp:Label>
											</div>
											<div style="width: 100%">
												<asp:GridView ID="dgEnginePeriods" runat="server" AutoGenerateColumns="False" Width="100%"
												BorderStyle="Solid" CssClass="clsGridNewStyle" GridLines="Horizontal"
												CellPadding="5" AlternatingRowStyle-CssClass="alt" RowStyle-Wrap="false" HeaderStyle-Wrap="false"
												SelectedRowStyle-BackColor="ButtonShadow" ShowHeaderWhenEmpty="True" PageSize="3">
												<AlternatingRowStyle CssClass="clsdgAltItem" />
												<SelectedRowStyle BackColor="ControlDark" />
												<RowStyle CssClass="clsdgItem" />
												<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
												<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
												<PagerSettings Mode="NextPreviousFirstLast" FirstPageText="First" LastPageText="Last" />
												<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
													<Columns>
														<asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
														<asp:BoundField DataField="ModelName" HeaderText="Model">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="150px" />
															<ItemStyle HorizontalAlign="Left" Wrap="false" Width="150px" />
														</asp:BoundField>
														<asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="100px" />
															<ItemStyle HorizontalAlign="Left" Wrap="false" Width="100px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Hours">
															<ItemTemplate>
																<asp:TextBox ID="txtEngineHours" runat="server" CssClass="clsTextBoxTagSearchSmall text-right"
																	ReadOnly="<%# Not mLog.IsNew %>" Text='<%# DataBinder.Eval(Container.DataItem, "Hours") %>'
																	ToolTip="Enter the Hours." Width="93%" AutoPostBack="true" OnTextChanged="txtEngineHours_TextChanged"
																	onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalHours" HeaderText="Final Hours">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Landings">
															<ItemTemplate>
																<asp:TextBox ID="txtEngineLandings" runat="server" CssClass="clsTextBoxTagSearchSmall"
																	Width="93%" Text='<%# DataBinder.Eval(Container.DataItem, "Landings") %>' ToolTip="Enter the Landing."
																	AutoPostBack="true" OnTextChanged="txtEngineLandings_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																	onfocus="onTextFocus();">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalLandings" HeaderText="Final Landings">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Cycles">
															<ItemTemplate>
																<asp:TextBox ID="txtEngineCycles" runat="server" CssClass="clsTextBoxTagSearchSmall text-right"
																	Width="93%" Text='<%# DataBinder.Eval(Container.DataItem, "Cycles") %>' ToolTip="Enter Cycles."
																	AutoPostBack="true" OnTextChanged="txtEngineCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																	onfocus="onTextFocus();">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalCycles" HeaderText="Final Cycles">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Starts">
															<ItemTemplate>
																<asp:TextBox ID="txtEngineStarts" runat="server" CssClass="clsTextBoxTagSearchSmall"
																	Text='<%# DataBinder.Eval(Container.DataItem, "Starts") %>' ToolTip="Enter Start Time."
																	AutoPostBack="true" OnTextChanged="txtEngineStarts_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																	onfocus="onTextFocus();" Width="93%">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalStarts" HeaderText="Final Starts">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="NG Cycles">
															<ItemTemplate>
																<asp:TextBox ID="txtEngineNGCycles" runat="server" CssClass="clsTextBoxTagSearchSmall"
																	Width="93%" Text='<%# DataBinder.Eval(Container.DataItem, "NGCycles") %>' ToolTip="Enter NG Cycles"
																	AutoPostBack="true" OnTextChanged="btnEngineNGCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																	onfocus="onTextFocus();">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalNGCycles" HeaderText="Final NG Cycles">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="NF Cycles">
															<ItemTemplate>
																<asp:TextBox ID="txtEngineNFCycles" runat="server" CssClass="clsTextBoxTagSearchSmall"
																	Width="93%" Text='<%# DataBinder.Eval(Container.DataItem, "NFCycles") %>' ToolTip="Enter NF Cycles"
																	AutoPostBack="true" OnTextChanged="txtEngineNFCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																	onfocus="onTextFocus();">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalNFCycles" HeaderText="Final NFCycles">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="RINS">
															<ItemTemplate>
																<asp:TextBox ID="txtEngineRins" runat="server" CssClass="clsTextBoxTagSearchSmall"
																	Text='<%# DataBinder.Eval(Container.DataItem, "RINS") %>' ToolTip="Enter RINS"
																	AutoPostBack="true" OnTextChanged="txtEngineRins_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																	onfocus="onTextFocus();" Width="93%">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalRINS" HeaderText="Final RINS">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Contingency Factor">
															<ItemTemplate>
																<asp:TextBox ID="txtEngineCFactors" runat="server" CssClass="clsTextBoxTagSearchSmall"
																	Width="97%" Text='<%# DataBinder.Eval(Container.DataItem, "CFactor") %>' ToolTip="Enter Contingency Factor."
																	AutoPostBack="true" OnTextChanged="txtEngineCFactors_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																	onfocus="onTextFocus();">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalCFactor" HeaderText="Final Contingency Factor">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Bleeds">
															<ItemTemplate>
																<asp:TextBox ID="txtEngineBleeds" runat="server" CssClass="clsTextBoxTagSearchSmall"
																	Text='<%# DataBinder.Eval(Container.DataItem, "Bleeds") %>' ToolTip="Enter Bleeds"
																	AutoPostBack="true" OnTextChanged="txtEngineBleeds_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																	onfocus="onTextFocus();" Width="93%">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalBleeds" HeaderText="Final Bleeds">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Impeller Cycles">
															<ItemTemplate>
																<asp:TextBox ID="txtEngineImpellerCycles" runat="server" CssClass="clsTextBoxTagSearchSmall"
																	Width="93%" Text='<%# DataBinder.Eval(Container.DataItem, "ImpellerCycles") %>'
																	ToolTip="Enter Impeller Cycles" AutoPostBack="true" OnTextChanged="txtEngineImpellerCycles_TextChanged"
																	onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalImpellerCycles" HeaderText="Final Impeller Cycles">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="CT Cycles">
															<ItemTemplate>
																<asp:TextBox ID="txtEngineCTCycles" runat="server" CssClass="clsTextBoxTagSearchSmall"
																	Width="93%" Text='<%# DataBinder.Eval(Container.DataItem, "CTCycles") %>' ToolTip="Enter CT Cycles"
																	AutoPostBack="true" OnTextChanged="txtEngineCTCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																	onfocus="onTextFocus();">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalCTCycles" HeaderText="Final CT Cycles">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="PT Cycles">
															<ItemTemplate>
																<asp:TextBox ID="txtEnginePTCycles" runat="server" CssClass="clsTextBoxTagSearchSmall"
																	Width="93%" Text='<%# DataBinder.Eval(Container.DataItem, "PTCycles") %>' ToolTip="Enter PT Cycles"
																	AutoPostBack="true" OnTextChanged="txtEnginePTCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																	onfocus="onTextFocus();">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalPTCycles" HeaderText="Final PT Cycles">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Generator Mods">
															<ItemTemplate>
																<asp:TextBox ID="txtEngineGeneratorMods" runat="server" CssClass="clsTextBoxTagSearchSmall"
																	Width="93%" Text='<%# DataBinder.Eval(Container.DataItem, "GeneratorMods") %>'
																	ToolTip="Enter the Generator Mods." AutoPostBack="true" OnTextChanged="txtEngineGeneratorMods_TextChanged"
																	onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalGeneratorMods" HeaderText="Final Generator Mods">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderText="Rapid Take Off">
															<HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
															<ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
															<ItemTemplate>
																<asp:TextBox ID="txtEngineRapidTakeOffFactor" runat="server" ToolTip="Enter Rapid Take Off."
																	CssClass="clsTextBoxTagSearchSmall" Text='<%# DataBinder.Eval(Container.DataItem, "RapidTakeOffFactor") %>'
																	AutoPostBack="true" OnTextChanged="txtEngineRapidTakeOffFactor_TextChanged" Width="97%"
																	onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
																</asp:TextBox>
															</ItemTemplate>
														</asp:TemplateField>
														<asp:BoundField DataField="FinalRapidTakeOffFactor" HeaderText="Final Rapid Take Off">
															<HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
															<ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
														</asp:BoundField>
													</Columns>
												</asp:GridView>
											</div>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td>
									<%-- </fieldset>--%>
									<asp:UpdatePanel ID="upnlAPUDetail" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<div style="width: 100%">
												<asp:Label ID="lblAPUPeriod" runat="server" CssClass="clsLabelHeader">APU Period</asp:Label>
											</div>
											<div style="width: 100%">
												<asp:GridView ID="dgAPUPeriods"  runat="server" AutoGenerateColumns="False" Width="100%"
												BorderStyle="Solid" CssClass="clsGridNewStyle" GridLines="Horizontal"
												CellPadding="5" AlternatingRowStyle-CssClass="alt" RowStyle-Wrap="false" HeaderStyle-Wrap="false"
												SelectedRowStyle-BackColor="ButtonShadow" ShowHeaderWhenEmpty="True" PageSize="3">
												<AlternatingRowStyle CssClass="clsdgAltItem" />
												<SelectedRowStyle BackColor="ControlDark" />
												<RowStyle CssClass="clsdgItem" />
												<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
												<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
												<PagerSettings Mode="NextPreviousFirstLast" FirstPageText="First" LastPageText="Last" />
												<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
													<Columns>
														<asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
														<asp:BoundField DataField="ModelName" HeaderText="Model">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="150px" />
															<ItemStyle HorizontalAlign="Left" Wrap="false" Width="150px" />
														</asp:BoundField>
														<asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="100px" />
															<ItemStyle HorizontalAlign="Left" Wrap="false" Width="100px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Hours">
															<ItemTemplate>
																<asp:TextBox ID="txtAPUHours" runat="server" CssClass="clsTextBoxTagSearchSmall text-right"
																	Text='<%# DataBinder.Eval(Container.DataItem, "Hours") %>' ToolTip="Enter the Hours."
																	Width="93%" AutoPostBack="true" OnTextChanged="txtAPUHours_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																	onfocus="onTextFocus();">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalHours" HeaderText="Final Hours">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Landings">
															<ItemTemplate>
																<asp:TextBox ID="txtAPULandings" runat="server" CssClass="clsTextBoxTagSearchSmall"
																	Text='<%# DataBinder.Eval(Container.DataItem, "Landings") %>' ToolTip="Enter the Landing."
																	AutoPostBack="true" OnTextChanged="txtAPULandings_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																	onfocus="onTextFocus();" Width="93%">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalLandings" HeaderText="Final Landings">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Cycles">
															<ItemTemplate>
																<asp:TextBox ID="txtAPUCycles" runat="server" CssClass="clsTextBoxTagSearchSmall text-right"
																	Text='<%# DataBinder.Eval(Container.DataItem, "Cycles") %>' ToolTip="Enter Cycles."
																	AutoPostBack="true" OnTextChanged="txtAPUCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																	onfocus="onTextFocus();" Width="93%">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalCycles" HeaderText="Final Cycles">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Starts">
															<ItemTemplate>
																<asp:TextBox ID="txtAPUStarts" runat="server" CssClass="clsTextBoxTagSearchSmall"
																	Text='<%# DataBinder.Eval(Container.DataItem, "Starts") %>' ToolTip="Enter Start Time."
																	AutoPostBack="true" OnTextChanged="txtAPUStarts_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																	onfocus="onTextFocus();" Width="93%">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalStarts" HeaderText="Final Starts">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="NG Cycles">
															<ItemTemplate>
																<asp:TextBox ID="txtAPUNGCycles" runat="server" CssClass="clsTextBoxTagSearchSmall"
																	Text='<%# DataBinder.Eval(Container.DataItem, "NGCycles") %>' ToolTip="Enter NG Cycles"
																	AutoPostBack="true" OnTextChanged="txtAPUNGCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																	onfocus="onTextFocus();" Width="93%">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalNGCycles" HeaderText="Final NG Cycles">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="NF Cycles">
															<ItemTemplate>
																<asp:TextBox ID="txtAPUNFCycles" runat="server" CssClass="clsTextBoxTagSearchSmall"
																	Text='<%# DataBinder.Eval(Container.DataItem, "NFCycles") %>' ToolTip="Enter NF Cycles"
																	AutoPostBack="true" OnTextChanged="txtAPUNFCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																	onfocus="onTextFocus();" Width="93%">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalNFCycles" HeaderText="Final NF Cycles">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="RINS">
															<ItemTemplate>
																<asp:TextBox ID="txtAPURins" runat="server" CssClass="clsTextBoxTagSearchSmall" Text='<%# DataBinder.Eval(Container.DataItem, "RINS") %>'
																	ToolTip="Enter RINS." AutoPostBack="true" OnTextChanged="txtAPURins_TextChanged"
																	onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
																	Width="93%">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalRINS" HeaderText="Final RINS">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Bleeds">
															<ItemTemplate>
																<asp:TextBox ID="txtAPUBleeds" runat="server" CssClass="clsTextBoxTagSearchSmall"
																	Text='<%# DataBinder.Eval(Container.DataItem, "Bleeds") %>' ToolTip="Enter Bleeds"
																	AutoPostBack="true" OnTextChanged="txtAPUBleeds_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																	onfocus="onTextFocus();" Width="93%">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalBleeds" HeaderText="Final Bleeds">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Impeller Cycles">
															<ItemTemplate>
																<asp:TextBox ID="txtAPUImpellerCycles" runat="server" CssClass="clsTextBoxTagSearchSmall"
																	Text='<%# DataBinder.Eval(Container.DataItem, "ImpellerCycles") %>' ToolTip="Enter Impeller Cycles"
																	AutoPostBack="true" OnTextChanged="txtAPUImpellerCycles_TextChanged" Width="93%"
																	onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalImpellerCycles" HeaderText="Final Impeller Cycles">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="CT Cycles">
															<ItemTemplate>
																<asp:TextBox ID="txtAPUCTCycles" runat="server" CssClass="clsTextBoxTagSearchSmall"
																	Text='<%# DataBinder.Eval(Container.DataItem, "CTCycles") %>' ToolTip="Enter CT Cycles"
																	AutoPostBack="true" OnTextChanged="txtAPUCTCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																	onfocus="onTextFocus();" Width="93%">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalCTCycles" HeaderText="Final CT Cycles">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="PT Cycles">
															<ItemTemplate>
																<asp:TextBox ID="txtAPUPTCycles" runat="server" CssClass="clsTextBoxTagSearchSmall"
																	Text='<%# DataBinder.Eval(Container.DataItem, "PTCycles") %>' ToolTip="Enter PT Cycles"
																	AutoPostBack="true" OnTextChanged="txtAPUPTCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																	onfocus="onTextFocus();" Width="93%">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalPTCycles" HeaderText="Final PT Cycles">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Generator Mods">
															<ItemTemplate>
																<asp:TextBox ID="txtAPUGeneratorMods" runat="server" CssClass="clsTextBoxTagSearchSmall"
																	Width="93%" Text='<%# DataBinder.Eval(Container.DataItem, "GeneratorMods") %>'
																	ToolTip="Enter the Generator Mods." AutoPostBack="true" OnTextChanged="txtAPUGeneratorMods_TextChanged"
																	onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalGeneratorMods" HeaderText="Final Generator Mods">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:BoundField HeaderText=""></asp:BoundField>
													</Columns>
												</asp:GridView>
											</div>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td>
									<%--AJAX- Add UpdatePanel for Airframe Grid--%>
									<asp:UpdatePanel ID="upnlCGBDetail" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<div style="width: 100%">
												<asp:Label ID="lblCGBPeriod" runat="server" CssClass="clsLabelHeader">Air Condition Period</asp:Label>
											</div>
											<div style="width: 100%">
												<asp:GridView ID="dgCGBPeriods" runat="server" AutoGenerateColumns="False" Width="100%"
												BorderStyle="Solid" CssClass="clsGridNewStyle" GridLines="Horizontal"
												CellPadding="5" AlternatingRowStyle-CssClass="alt" RowStyle-Wrap="false" HeaderStyle-Wrap="false"
												SelectedRowStyle-BackColor="ButtonShadow" ShowHeaderWhenEmpty="True" PageSize="3">
												<AlternatingRowStyle CssClass="clsdgAltItem" />
												<SelectedRowStyle BackColor="ControlDark" />
												<RowStyle CssClass="clsdgItem" />
												<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
												<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
												<PagerSettings Mode="NextPreviousFirstLast" FirstPageText="First" LastPageText="Last" />
												<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
													<Columns>
														<asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
														<asp:BoundField DataField="ModelName" HeaderText="Model">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="150px" />
															<ItemStyle HorizontalAlign="Left" Wrap="false" Width="150px" />
														</asp:BoundField>
														<asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="100px" />
															<ItemStyle HorizontalAlign="Left" Wrap="false" Width="100px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Hours">
															<ItemTemplate>
																<asp:TextBox ID="txtCGBHours" runat="server" CssClass="clsTextBoxTagSearchSmall text-right"
																	ReadOnly="<%# Not mLog.IsNew %>" Text='<%# DataBinder.Eval(Container.DataItem, "Hours") %>'
																	ToolTip="Enter the Hours." Width="93%" AutoPostBack="true" OnTextChanged="txtCGBHours_TextChanged"
																	onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalHours" HeaderText="Final Hours">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Landings">
															<ItemTemplate>
																<asp:TextBox ID="txtCGBLandings" runat="server" CssClass="clsTextBoxTagSearchSmall"
																	Text='<%# DataBinder.Eval(Container.DataItem, "Landings") %>' ToolTip="Enter the Landing."
																	AutoPostBack="true" OnTextChanged="txtCGBLandings_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																	onfocus="onTextFocus();" Width="93%">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalLandings" HeaderText="Final Landings">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Cycles">
															<ItemTemplate>
																<asp:TextBox ID="txtCGBCycles" runat="server" CssClass="clsTextBoxTagSearchSmall text-right"
																	Text='<%# DataBinder.Eval(Container.DataItem, "Cycles") %>' ToolTip="Enter Cycles."
																	AutoPostBack="true" OnTextChanged="txtCGBCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																	onfocus="onTextFocus();" Width="93%">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalCycles" HeaderText="Final Cycles">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Starts">
															<ItemTemplate>
																<asp:TextBox ID="txtCGBStarts" runat="server" CssClass="clsTextBoxTagSearchSmall"
																	Text='<%# DataBinder.Eval(Container.DataItem, "Starts") %>' ToolTip="Enter Start Time."
																	AutoPostBack="true" OnTextChanged="txtCGBStarts_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																	onfocus="onTextFocus();" Width="93%">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalStarts" HeaderText="Final Starts">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="NG Cycles">
															<ItemTemplate>
																<asp:TextBox ID="txtCGBNGCycles" runat="server" CssClass="clsTextBoxTagSearchSmall"
																	Text='<%# DataBinder.Eval(Container.DataItem, "NGCycles") %>' ToolTip="Enter NG Cycles"
																	AutoPostBack="true" OnTextChanged="txtCGBNGCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																	onfocus="onTextFocus();" Width="93%">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalNGCycles" HeaderText="Final NG Cycles">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="NF Cycles">
															<ItemTemplate>
																<asp:TextBox ID="txtCGBNFCycles" runat="server" CssClass="clsTextBoxTagSearchSmall"
																	Text='<%# DataBinder.Eval(Container.DataItem, "NFCycles") %>' ToolTip="Enter NF Cycles"
																	AutoPostBack="true" OnTextChanged="txtCGBNFCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																	onfocus="onTextFocus();" Width="93%">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalNFCycles" HeaderText="Final NF Cycles">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="RINS">
															<ItemTemplate>
																<asp:TextBox ID="txtCGBRINS" runat="server" CssClass="clsTextBoxTagSearchSmall" Text='<%# DataBinder.Eval(Container.DataItem, "RINS") %>'
																	ToolTip="Enter RINS" AutoPostBack="true" OnTextChanged="txtCGBRINS_TextChanged"
																	onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
																	Width="93%">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalRINS" HeaderText="Final RINS">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Bleeds">
															<ItemTemplate>
																<asp:TextBox ID="txtCGBBleeds" runat="server" CssClass="clsTextBoxTagSearchSmall"
																	Text='<%# DataBinder.Eval(Container.DataItem, "Bleeds") %>' ToolTip="Enter Bleeds"
																	AutoPostBack="true" OnTextChanged="txtCGBBleeds_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																	onfocus="onTextFocus();" Width="93%">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalBleeds" HeaderText="Final Bleeds">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Impeller Cycles">
															<ItemTemplate>
																<asp:TextBox ID="txtCGBImpellerCycles" runat="server" CssClass="clsTextBoxTagSearchSmall"
																	Text='<%# DataBinder.Eval(Container.DataItem, "ImpellerCycles") %>' ToolTip="Enter Impeller Cycles"
																	AutoPostBack="true" OnTextChanged="txtCGBImpellerCycles_TextChanged" Width="93%"
																	onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalImpellerCycles" HeaderText="Final Impeller Cycles">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="CT Cycles">
															<ItemTemplate>
																<asp:TextBox ID="txtCGBCTCycles" runat="server" CssClass="clsTextBoxTagSearchSmall"
																	Text='<%# DataBinder.Eval(Container.DataItem, "CTCycles") %>' ToolTip="Enter CT Cycles"
																	AutoPostBack="true" OnTextChanged="txtCGBCTCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																	onfocus="onTextFocus();" Width="93%">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalCTCycles" HeaderText="Final CT Cycles">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="PT Cycles">
															<ItemTemplate>
																<asp:TextBox ID="txtCGBPTCycles" runat="server" CssClass="clsTextBoxTagSearchSmall"
																	Text='<%# DataBinder.Eval(Container.DataItem, "PTCycles") %>' ToolTip="Enter PT Cycles"
																	AutoPostBack="true" OnTextChanged="txtCGBPTCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
																	onfocus="onTextFocus();" Width="93%">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalPTCycles" HeaderText="Final PT Cycles">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Generator Mods">
															<ItemTemplate>
																<asp:TextBox ID="txtCGBGeneratorMods" runat="server" CssClass="clsTextBoxTagSearchSmall"
																	Text='<%# DataBinder.Eval(Container.DataItem, "GeneratorMods") %>' ToolTip="Enter the Generator Mods."
																	AutoPostBack="true" OnTextChanged="txtCGBGeneratorMods_TextChanged" Width="93%"
																	onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
																</asp:TextBox>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Right" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Width="75px" />
														</asp:TemplateField>
														<asp:BoundField DataField="FinalGeneratorMods" HeaderText="Final Generator Mods">
															<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
															<ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
														</asp:BoundField>
														<asp:BoundField HeaderText=""></asp:BoundField>
													</Columns>
												</asp:GridView>
											</div>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td>
									<br>
								</td>
							</tr>
							<tr>
								<td>
									<%--AJAX- Add UpdatePanel for Engine Grid--%>
									<asp:UpdatePanel ID="upnlRemark" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:Label ID="lblRemark" runat="server" CssClass="clsLabelAuto">Remark</asp:Label>
											<br />
											<asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
												MaxLength="500" Text="<%# mLog.Remark %>" TextMode="MultiLine" ToolTip="Enter Remark"
												Width="400px" onfocus="onTextFocus();"></asp:TextBox>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr style="height: 0px;">
					<td>
						<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
							<ContentTemplate>
								<asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
									CausesValidation="False" Style="display: none;"></asp:Button>
							</ContentTemplate>
						</asp:UpdatePanel>
					</td>
				</tr>
			</table>
			<%-- <%--AJAX- Add UpdateProgress to show loading for Longer Process--%>
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
		</div>
		<%--AJAX- Add UpdateProgress to show loading for Longer Process--%>
		<!-- File Upload Modal Dialog-->
		<div style="display: none">
			<asp:HiddenField runat="server" ID="btnDummyFileUpload" />
		</div>
		<asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="IFileUpload" allowtransparency="true" frameborder="0" height="100%" width="100%"
				src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupFileUpload" runat="server" TargetControlID="btnDummyFileUpload"
			PopupControlID="pnlFileUpload" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameFileUploadStateComplete() {
				$("#btnDummyFileUpload").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}

			$(document).ready(function () {
				$("#btnSelectFile").live("click", function () {
					try {
						$get("AjaxLoader").style.visibility = 'visible';
						$("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx");
						if (!$.browser.msie) {
							$("#btnDummyFileUpload").click();
							$get("AjaxLoader").style.visibility = 'hidden';
						}

						return false;
					} catch (e) {
						alert(e);
					}
				});
			});
		</script>
		<script type="text/javascript">
			function ParentCallBackFunctionForFileUpload(fileattached) {
				var FileUpwindow = $find("<%=mdlPopupFileUpload.ClientID %>");
				//close File Upload popup window
				FileUpwindow.hide();
				//Free resources
				$("#IFileUpload").attr("src", "JavaScript:''");
				if (fileattached) {
					//call hidden button to set file upload content to object
					$("#hdnBtnFileUpload").click();
				}
			}
		</script>
		<!-- End -->
		<div id="InfoMessagepanel" class="clsInfoMessage1" style="display: none; z-index: 100"
			draggable="true">
			<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlLogInfo">
				<ContentTemplate>
					<table class="zui-table zui-table-rounded" style="z-index: 100" draggable="true">
						<thead style="z-index: 100">
							<tr>
								<td colspan="4">
									<span><b>List of Logs on selected date : </b></span><span><b>
										<%= mLogListOnDate.Count%></b></span> <span><b>Record(s)</b></span>
									<%--<a class="close-btn" href="#" onclick="CloseLastDet();return false;">X</a>--%>
								</td>
							</tr>
						</thead>
						<thead style="z-index: 100">
							<tr>
								<th>
									<span>Log No. &</span>
									<br />
									<span>Log Page No.</span>
								</th>
								<th>
									<span>Departure Info</span>
								</th>
								<th>
									<span>Arrival Info</span>
								</th>
								<th>
									<span>Airborne Time</span>
								</th>
							</tr>
						</thead>
						<tbody style="z-index: 100">
							<% Dim Child3 As LogInfo%>
							<% For Each Child3 In mLogListOnDate%>
							<tr>
								<td>
									<span>
										<%= Child3.LogTextNo %></span>
									<br />
									<span>
										<%= Child3.LogPageNoFormatted %></span>
								</td>
								<td>
									<% If mMachine.IsUTC Then%>
									<span>
										<%= Child3.SouUniverseDateTimeFormatted%></span>
									<% Else%>
									<span>
										<%= Child3.SouLocalDateTimeFormatted%></span>
									<%End If%>
									<% If Child3.LogTypeID = 1 Then%>
									<span>
										<br />
										<%= Child3.SouPlaceName %></span>
									<%End If%>
								</td>
								<td>
									<% If mMachine.IsUTC Then%>
									<span>
										<%= Child3.DesUniverseDateTimeFormatted%></span>
									<% Else%>
									<span>
										<%= Child3.DesLocalDateTimeFormatted%></span>
									<%End If%>
									<% If Child3.LogTypeID = 1 Then%>
									<span>
										<br />
										<%= Child3.DesPlaceName %></span>
									<%End If%>
								</td>
								<td>
									<span>
										<%= Child3.TimeInAir %></span>
								</td>
							</tr>
							<% Next%>
						</tbody>
					</table>
				</ContentTemplate>
			</asp:UpdatePanel>
		</div>
		<div id="pnlAllAssemblypanel" class="clsInfoMessage1" style="display: none; z-index: 100;"
			draggable="true">
			<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlAssemblyInfo">
				<ContentTemplate>
					<div style="width: 90%">
						<table style="width: 100%">
							<tr>
								<td>
									<asp:Label ID="Label4" runat="server" CssClass="clsLabelHeader">ALL Assemblies</asp:Label>
								</td>
								<td align="right">
									<span><a class="close-btn1" style="font-size: medium; color: Black" href="#" onclick="CloseAssemblyDet();return false;">X</a> </span>
								</td>
							</tr>
						</table>
					</div>
					<div style="width: 90%; overflow: scroll">
						<asp:GridView ID="grdAllAssemblies" runat="server" AutoGenerateColumns="False" Width="100%"
							BorderStyle="Solid" CellPadding="0" ForeColor="#333333" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
							RowStyle-Wrap="false" HeaderStyle-Wrap="false" SelectedRowStyle-BackColor="ButtonShadow"
							ShowHeaderWhenEmpty="True" PageSize="3" PagerSettings-Mode="NextPreviousFirstLast">
							<RowStyle CssClass="clsdgItem" />
							<HeaderStyle ForeColor="White" />
							<Columns>
								<asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
								<asp:BoundField DataField="ModelName" HeaderText="Model">
									<HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="150px" />
									<ItemStyle HorizontalAlign="Left" Wrap="false" Width="150px" />
								</asp:BoundField>
								<asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
									<HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="100px" />
									<ItemStyle HorizontalAlign="Left" Wrap="false" Width="100px" />
								</asp:BoundField>
								<asp:BoundField DataField="AssemblyTypeCode" HeaderText="Type">
									<HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="100px" />
									<ItemStyle HorizontalAlign="Left" Wrap="false" Width="100px" />
								</asp:BoundField>
								<asp:BoundField DataField="Hours" HeaderText="Hours">
									<HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="100px" />
									<ItemStyle HorizontalAlign="Left" Wrap="false" Width="100px" />
								</asp:BoundField>
								<asp:BoundField DataField="FinalHours" HeaderText="Final Hours">
									<HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="100px" />
									<ItemStyle HorizontalAlign="Left" Wrap="false" Width="100px" />
								</asp:BoundField>
								<asp:BoundField DataField="Landings" HeaderText="Landings">
									<HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="100px" />
									<ItemStyle HorizontalAlign="Left" Wrap="false" Width="100px" />
								</asp:BoundField>
								<asp:BoundField DataField="FinalLandings" HeaderText="Final Landings">
									<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
									<ItemStyle HorizontalAlign="Right" Wrap="false" />
								</asp:BoundField>
								<asp:BoundField DataField="Cycles" HeaderText="Cycles">
									<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
									<ItemStyle HorizontalAlign="Right" Wrap="false" />
								</asp:BoundField>
								<asp:BoundField DataField="FinalCycles" HeaderText="Final Cycles">
									<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
									<ItemStyle HorizontalAlign="Right" Wrap="false" />
								</asp:BoundField>
								<asp:BoundField DataField="Starts" HeaderText="Starts">
									<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
									<ItemStyle HorizontalAlign="Right" Wrap="false" />
								</asp:BoundField>
								<asp:BoundField DataField="FinalStarts" HeaderText="Final Starts">
									<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
									<ItemStyle HorizontalAlign="Right" Wrap="false" />
								</asp:BoundField>
								<asp:BoundField DataField="NGCycles" HeaderText="NG Cycles">
									<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
									<ItemStyle HorizontalAlign="Right" Wrap="false" />
								</asp:BoundField>
								<asp:BoundField DataField="FinalNGCycles" HeaderText="Final NG Cycles">
									<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
									<ItemStyle HorizontalAlign="Right" Wrap="false" />
								</asp:BoundField>
								<asp:BoundField DataField="NFCycles" HeaderText="NF Cycles">
									<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
									<ItemStyle HorizontalAlign="Right" Wrap="false" />
								</asp:BoundField>
								<asp:BoundField DataField="FinalNFCycles" HeaderText="Final NFCycles">
									<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
									<ItemStyle HorizontalAlign="Right" Wrap="false" />
								</asp:BoundField>
								<asp:BoundField DataField="RINS" HeaderText="RINS">
									<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
									<ItemStyle HorizontalAlign="Right" Wrap="false" />
								</asp:BoundField>
								<asp:BoundField DataField="FinalRINS" HeaderText="Final RINS">
									<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
									<ItemStyle HorizontalAlign="Right" Wrap="false" />
								</asp:BoundField>
								<asp:BoundField DataField="Bleeds" HeaderText="Bleeds">
									<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
									<ItemStyle HorizontalAlign="Right" Wrap="false" />
								</asp:BoundField>
								<asp:BoundField DataField="FinalBleeds" HeaderText="Final Bleeds">
									<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
									<ItemStyle HorizontalAlign="Right" Wrap="false" />
								</asp:BoundField>
								<asp:BoundField DataField="ImpellerCycles" HeaderText="ImpellerCycles">
									<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
									<ItemStyle HorizontalAlign="Right" Wrap="false" />
								</asp:BoundField>
								<asp:BoundField DataField="FinalImpellerCycles" HeaderText="Final Impeller Cycles">
									<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
									<ItemStyle HorizontalAlign="Right" Wrap="false" />
								</asp:BoundField>
								<asp:BoundField DataField="CTCycles" HeaderText="CT Cycles">
									<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
									<ItemStyle HorizontalAlign="Right" Wrap="false" />
								</asp:BoundField>
								<asp:BoundField DataField="FinalCTCycles" HeaderText="Final CT Cycles">
									<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
									<ItemStyle HorizontalAlign="Right" Wrap="false" />
								</asp:BoundField>
								<asp:BoundField DataField="PTCycles" HeaderText="PT Cycles">
									<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
									<ItemStyle HorizontalAlign="Right" Wrap="false" />
								</asp:BoundField>
								<asp:BoundField DataField="FinalPTCycles" HeaderText="Final PT Cycles">
									<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
									<ItemStyle HorizontalAlign="Right" Wrap="false" />
								</asp:BoundField>
								<asp:BoundField DataField="GeneratorMods" HeaderText="Generator Mods">
									<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
									<ItemStyle HorizontalAlign="Right" Wrap="false" />
								</asp:BoundField>
								<asp:BoundField DataField="FinalGeneratorMods" HeaderText="Final Generator Mods">
									<HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
									<ItemStyle HorizontalAlign="Right" Wrap="false" />
								</asp:BoundField>
							</Columns>
							<SelectedRowStyle BackColor="ControlDark" />
							<AlternatingRowStyle CssClass="clsdgAltItem" />
						</asp:GridView>
					</div>
				</ContentTemplate>
			</asp:UpdatePanel>
		</div>
		<script type="text/javascript">
			function delete_cookie() {
				$.cookie('HideInfoMessagepanel', false);

			}
			function ShowLastDet() {
				$pos = $("#<%=lblDepPlace.ClientID%>").position();
				var top = $pos.top;
				var left = $pos.left;
				var searchHeight = $("#<%=lblDepPlace.ClientID%>").height();
				var margin = top + searchHeight;

				var height = $("#tblMain").outerHeight();
				var h = margin - height;


				if ($.cookie('HideInfoMessagepanel') == 'true') $("#InfoMessagepanel").hide();
				else {
					$.cookie('HideInfoMessagepanel', true);
					$("#InfoMessagepanel").css("display", "block");
					$("#InfoMessagepanel").animate({ marginTop: h, marginLeft: left - 5 }, 100, 'swing', function () {
						$("#InfoMessagepanel").delay(9000).fadeOut();

					});
				}
			}
		</script>
		<script type="text/javascript">
			function ShowAssembly() {

				$pos = $("#<%=lnkAllAssembly.ClientID%>").position();
				var top = $pos.top;
				var left = $pos.left - 600;
				var searchHeight = $("#<%=lnkAllAssembly.ClientID%>").height();
				var margin = top + searchHeight;

				var height = $("#tblMain").outerHeight();
				var h = margin - height;
				$("#pnlAllAssemblypanel").css("display", "block");
				$("#pnlAllAssemblypanel").animate({ marginTop: h, marginLeft: left - 5 }, 100, 'swing', function () {
					//   $("#pnlAllAssemblypanel").delay(9000).fadeOut();
				});
			}
		</script>
		<script type="text/javascript">
			function CloseAssemblyDet() {

				$("#pnlAllAssemblypanel").hide();
			}
		</script>
		<script type="text/javascript">
			function delete_cookie() {
				$.cookie('HideInfoMessagepanel', null);
			}
		</script>
	</form>
	<!-- Autocomplete for Source and Destination Place   -->
	<script type="text/javascript">
		//AJAX- Replaced "$(document).ready(function(){" by " Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(){" as it gets fired only after complete PostBack
		Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
			$("#<%=Place1.ClientID%>,#<%=Place2.ClientID%>").autocomplete('wfAutoPilotPlace.aspx?Type=Place', {
				width: 250,
				autoFill: true,
				matchContains: true,
				delay: 0
			});
		});
	</script>
	<!-- Autocomplete for Pilot1 and Pilot2    -->
	<script type="text/javascript">
		//AJAX- Replaced "$(document).ready(function(){" by " Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(){" as it gets fired only after complete PostBack
		Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
			$("#<%=Pilot1.ClientID%>").autocomplete('wfAutoPilotPlace.aspx?Type=Pilot', {
				autoFill: true,
				width: 252,
				mustMatch: true,
				matchContains: true,
				delay: 0
			});
		});
	</script>
	<script type="text/javascript">
		//AJAX- Replaced "$(document).ready(function(){" by " Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(){" as it gets fired only after complete PostBack
		Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
			$("#<%=Pilot2.ClientID%>").autocomplete('wfAutoPilotPlace.aspx?Type=Pilot', {
				autoFill: true,
				width: 256,
				mustMatch: true,
				matchContains: true,
				delay: 0
			});
		});
	</script>
	<script type="text/javascript">
		//AJAX- New JavaScript function added to Show/Hidw JQuery Date Control
		function AfterSave(IsShowDateCntrl) {

			if (IsShowDateCntrl == "True") {
				savedlog1 = "button";
			}
			else {
				savedlog1 = "";
			}


		}

	</script>
	<script type="text/javascript">
		//Date validations
		function ValidateDateText(elem, extenderid) {

			var datevalue = $(elem).val();
			var params = { 'Date': datevalue, 'SetDefault': 'false' };
			$.ajax({
				type: "POST",
				url: "DateValidationHandler.ashx",
				cache: false,
				async: false,
				data: params,
				beforeSend: OnBeforeSend,
				success: onSuccess,
				error: onError
			});
			return false;
			function onSuccess(result) {
				$(elem).removeClass('ac_loading');
				$(elem).val(result);
				$find(extenderid).set_Text(result);
			}

			function onError(result) {
				$(elem).removeClass('ac_loading');
				$(elem).val('');
				$find(extenderid).set_Text('');
			}
			function OnBeforeSend() {
				$(elem).addClass('ac_loading');
			}
		}
	</script>
</body>
</html>
