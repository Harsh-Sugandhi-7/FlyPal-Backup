<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDentAndRepairRectificationList_Ajax.aspx.vb"
	Inherits="Flypal.DentAndRepairRectificationListPage" %>

<%@ Import Namespace="System.Configuration.ConfigurationSettings" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<title>Dent & Repair Rectification List</title>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

</head>
<body>
	<form id="form1" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
			EnablePageMethods="true">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table class="clstablelistout Table-MaxWidth" id="tblmain">
			<tr>
				<td>
					<asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
						<table id="tblInner" class="clstablelistin">
							<tr>
								<td class="clsFormHeader1Newstyle">
									<table width="100%">
										<tr>
											<td>
												<asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">
													List of Dent & Repair Chart Items
												</asp:Label>
											</td>
											<td align="right">
												<asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<asp:Button ID="btnBack" runat="server"
															CausesValidation="False" CssClass="clsbtnH clsinfoH"
															Text="Close" ToolTip="Click to close screen" />
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<table width="100%">
										<tr>
											<td>
												<asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
													<ContentTemplate>
														<table id="Table1">
															<tr>
																<td>
																	<asp:Label ID="Label1" runat="server" CssClass="clsLabel" Width="78px">Date</asp:Label>
																</td>
																<td>
																	<asp:DropDownList ID="cmbDate" runat="server" 
																		CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True">
																		<asp:ListItem Value="0">(ALL)</asp:ListItem>
																		<asp:ListItem Value="1">Last 1 Week</asp:ListItem>
																		<asp:ListItem Value="2">Last 1 Month</asp:ListItem>
																		<asp:ListItem Value="3">Last 1 Quarter</asp:ListItem>
																		<asp:ListItem Value="4">Last 1 Year</asp:ListItem>
																		<asp:ListItem Value="5">Current Financial Year</asp:ListItem>
																		<asp:ListItem Value="6">Between Dates</asp:ListItem>
																	</asp:DropDownList>
																</td>
																<td>
																	<asp:Label ID="lblFromDate" runat="server" CssClass="clsLabel" Width="78px">From Date</asp:Label>
																</td>
																<td>
																	<asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagSearchDate" Width="100px"
																		CausesValidation="true" ValidationGroup="a" ClientIDMode="Static"
																		onchange="ValidateDateText(this,'FromDate_watermarkextender');"
																		AutoPostBack="True" />
																	<cc2:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
																		Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate" />
																	<cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="TextBoxWatermarkExtender1"
																		ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																		WatermarkCssClass="clsDateTextBox" />
																	<asp:CustomValidator ID="cvFromDate" runat="server" 
																		CssClass="clsLabelAuto" Display="None"
																		ClientValidationFunction="BetweenDatesValidation" ValidationGroup="a"
																		ErrorMessage="From Date should not be greater than To Date " />
																</td>
																<td align="right">&nbsp;&nbsp;
                                                                <asp:Label ID="lblToDate" CssClass="clsLabel" runat="server" Width="78px" DESIGNTIMEDRAGDROP="19">To Date </asp:Label>
																</td>
																<td>
																	<asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagSearchDate" Width="100px"
																		CausesValidation="true" ValidationGroup="a" ClientIDMode="Static" 
																		onchange="ValidateDateText(this,'ToDate_watermarkextender');"
																		AutoPostBack="True" />
																	<cc2:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="cal_Theme1"
																		Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate" />
																	<cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="TextBoxWatermarkExtender2"
																		ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																		WatermarkCssClass="clsDateTextBox" />
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
								<td valign="top" colspan="2">
									<asp:UpdatePanel runat="server" ID="upnlCollapsiblePnl" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:Panel ID="cpnlAdvancedSearch" runat="server" CssClass="clsCollapsePnl">
												<div>
													<div id="divCollapsiblePnl">
														<table width="100%">
															<tr>
																<td>
																	<span id="lblMastersSelection" class="clsLabelHeader">Advance Search
																	</span>
																</td>
																<td align="right">
																	<div id="divCollapsiblePnlImg">
																		<image id="imgMasters" src="images/collapse_blue.jpg"
																			alternatetext="(Show Details...)" />
																	</div>
																</td>
															</tr>
														</table>
													</div>
												</div>
											</asp:Panel>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td valign="top">
									<asp:Panel ID="pnlAdvancedSearch" runat="server" 
											Style="max-height: 200px; overflow-y: auto; 
												   overflow: auto; overflow-x: hidden;">
										<asp:UpdatePanel runat="server" ID="upnlMoreSearch" UpdateMode="Conditional">
											<ContentTemplate>
												<table>
													<tr>
														<td>
															<span id="lblCWPNo" class="clsLabelAuto">Dent & Repair No.</span>
														</td>
														<td>
															<asp:DropDownList ID="cmbDentBuckelNo" runat="server" 
																CssClass="clsTextBoxTagSearchComboNewstyle"
																AutoPostBack="True" DataTextField="Text" DataValueField="Text">
																<asp:ListItem Value="0">(ALL)</asp:ListItem>
															</asp:DropDownList>
														</td>
														<td>
															<asp:Label ID="lblNo" runat="server" CssClass="clsLabelAuto">No.</asp:Label>
														</td>
														<td>
															<asp:TextBox ID="txtNo" runat="server" 
																CssClass="clsTextBoxTagSearchSmall" MaxLength="4"
																ClientIDMode="Static" ToolTip="Enter Number"
																AutoPostBack="True">0</asp:TextBox>
														</td>
														<td>
															<span id="lblPart" class="clsLabelAuto">Aircraft</span>
														</td>
														<td>
															<asp:TextBox ID="txtRegNo" runat="server" 
																CssClass="clsTextBoxTagSearch" ToolTip="Enter Reg No."
																AutoPostBack="True" />
														</td>
														<td>
															<span id="lblStatus" class="clsLabelAuto">Status</span>
														</td>
														<td>
															<asp:DropDownList ID="cmbDentBuckelStatus" runat="server" 
																AutoPostBack="True" CssClass="clsTextBoxTagSearchComboNewstyle">
																<asp:ListItem Text="(ALL)" Value="0"></asp:ListItem>
																<asp:ListItem Text="Open" Value="1"></asp:ListItem>
																<asp:ListItem Text="Temporary Action" Value="2"></asp:ListItem>
																<asp:ListItem Text="Permanant Action" Value="3"></asp:ListItem>
															</asp:DropDownList>
														</td>
													</tr>
													<tr>
														<td>
															<span id="lblATA" class="clsLabel">Item No.</span>
														</td>
														<td>
															<asp:TextBox ID="txtItemNo" runat="server"
																CssClass="clsTextBoxTagSearch" ToolTip="Enter Item No."
																AutoPostBack="True" />
														</td>
														<td>
															<span id="Span1" class="clsLabel">Description</span>
														</td>
														<td colspan="5">
															<asp:TextBox ID="txtDescription" runat="server" 
																CssClass="clsTextBoxTagSearchMultilineNewstyle"
																ToolTip="Enter Description" TextMode="MultiLine" 
																Width="550px" AutoPostBack="True" />
														</td>
													</tr>
												</table>
											</ContentTemplate>
										</asp:UpdatePanel>
									</asp:Panel>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlgrid" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table width="100%">
												<tr>
													<td>
														<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
													</td>
												</tr>
												<tr>
													<td>
														<asp:GridView ID="dgItems" runat="server" ShowHeaderWhenEmpty="True" PageSize="10"
															AllowSorting="true" DataKeyNames="ID,DentbuckleID" AutoGenerateColumns="False"
															CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AllowPaging="True">
															<AlternatingRowStyle CssClass="clsdgAltItem" />
															<RowStyle CssClass="clsdgItem" />
															<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" 
																		 HorizontalAlign="Left" />
															<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
															<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
															<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
															<Columns>
																<%--0--%>
																<asp:BoundField DataField="ID" HeaderText="ID" HeaderStyle-CssClass="hideGridColumn"
																	ItemStyle-CssClass="hideGridColumn" />
																<%--1--%>
																<asp:BoundField DataField="DentbuckleID" 
																	HeaderText="DentbuckleID" HeaderStyle-CssClass="hideGridColumn"
																	ItemStyle-CssClass="hideGridColumn" />
																<%--2--%>
																<asp:BoundField DataField="ReportDateFormatted" HeaderText="Report Date">
																	<HeaderStyle Wrap="True" HorizontalAlign="Left" Width="80px"/>
																	<ItemStyle Wrap="True" Width="80px" />
																</asp:BoundField>
																<%--3--%>
																<asp:BoundField DataField="TextNo" HeaderText="Dent Repair No." SortExpression="TextNo">
																	<HeaderStyle Wrap="False" HorizontalAlign="Left" />
																	<ItemStyle Wrap="False" />
																</asp:BoundField>
																<%--4--%>
																<asp:BoundField DataField="RegNo" HeaderText="Aircraft" SortExpression="RegNo">
																	<HeaderStyle HorizontalAlign="Left" />
																	<ItemStyle Wrap="false" />
																</asp:BoundField>
																<%--6--%>
																<asp:BoundField DataField="LogTextNo" HeaderText="Log">
																	<HeaderStyle HorizontalAlign="Left" Wrap="false" />
																	<ItemStyle Wrap="false" />
																</asp:BoundField>
																<%--7--%>
																<asp:BoundField DataField="ItemNo" HeaderText="Item No." 
																	SortExpression="ItemNo">
																	<HeaderStyle Wrap="True" HorizontalAlign="Left" Width="30px" />
																	<ItemStyle Wrap="True" Width="30px" />
																</asp:BoundField>
																<%--8--%>
																<asp:BoundField DataField="Description" HeaderText="Description"
																	SortExpression="Description">
																	<HeaderStyle Wrap="True" HorizontalAlign="Left" />
																	<ItemStyle Wrap="True" />
																</asp:BoundField>
																<%--9--%>
																<asp:BoundField DataField="ATACode" HeaderText="ATA">
																	<HeaderStyle Wrap="True" HorizontalAlign="Left" />
																	<ItemStyle Wrap="True" />
																</asp:BoundField>
																<%--10--%>
																<asp:BoundField DataField="AcceptanceByName" HeaderText="Acceptance By">
																	<HeaderStyle Wrap="True" HorizontalAlign="Left" />
																	<ItemStyle Wrap="True" />
																</asp:BoundField>
																<%--11--%>
																<asp:BoundField DataField="ReportedByName" HeaderText="Reported By">
																	<HeaderStyle Wrap="True" HorizontalAlign="Left" />
																	<ItemStyle Wrap="True" />
																</asp:BoundField>
																<%--12--%>
																<asp:BoundField DataField="ActionTakenByName" HeaderText="Action Taken By">
																	<HeaderStyle Wrap="True" HorizontalAlign="Left" />
																	<ItemStyle Wrap="True" />
																</asp:BoundField>
																<%--13--%>
																<asp:BoundField DataField="ItemStatusName" HeaderText="Status">
																	<HeaderStyle Wrap="True" HorizontalAlign="Left" />
																	<ItemStyle Wrap="True" />
																</asp:BoundField>
																<%--14--%>
																<asp:ButtonField CommandName="Rectify" HeaderText="Rectify" Text="Rectify">
																	<HeaderStyle HorizontalAlign="Left" />
																</asp:ButtonField>
																<%--15--%>
																<asp:BoundField DataField="ItemStatusID" HeaderText="ItemStatusID" 
																	HeaderStyle-CssClass="hideGridColumn"
																	ItemStyle-CssClass="hideGridColumn" />
															</Columns>
														</asp:GridView>
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
							</tr>
						</table>
					</asp:Panel>
				</td>
			</tr>
		</table>
		<cc2:CollapsiblePanelExtender BehaviorID="clpMastersBehaviour" ID="clpAdvancedSearch"
			ClientIDMode="Static" runat="Server" TargetControlID="pnlAdvancedSearch" ExpandControlID="cpnlAdvancedSearch"
			CollapseControlID="cpnlAdvancedSearch" Collapsed="True" ImageControlID="imgMasters"
			CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
			ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
			SuppressPostBack="false" />

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


		<script type="text/javascript">
			//From Date -To Date validation
			function BetweenDatesValidation(source, args) {
				args.IsValid = false;
				var fromdate = $("#txtFromDate").val();
				var todate = $("#txtToDate").val();
				if (!todate) {
					rfvToDate.isvalid = false;
					return;
				}
				if (!fromdate) {
					rfvFromDate.isvalid = false;
					return;
				}
				var param = { 'FromDate': fromdate, 'ToDate': todate };
				$.ajax({
					type: "POST",
					url: "BetweenDateValidationHandler.ashx",
					cache: false,
					data: param,
					async: false,
					beforeSend: OnBeforeSnd,
					success: onSuces,
					error: onErr
				});

				function onSuces(result) {
					$get("AjaxLoader").style.visibility = 'hidden';
					if (result == "True") {
						args.IsValid = true;
						return;
					}

				}

				function onErr(result) {
					$get("AjaxLoader").style.visibility = 'hidden';
					source.errormessage = result;
					return;
				}
				function OnBeforeSnd() {
					$get("AjaxLoader").style.visibility = 'visible';
				}

			}
			//Date validations
			function ValidateDateText(elem, extenderid) {

				var datevalue = $(elem).val();
				var params = { 'Date': datevalue, 'SetDefault': 'true' };
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
	</form>
</body>
</html>
