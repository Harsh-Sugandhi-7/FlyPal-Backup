<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCalibrationFrequencyChange_Ajax.aspx.vb"
	EnableEventValidation="false" Inherits="Flypal.wfCalibrationFrequencyChange_Ajax" %>

<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<title>Calibration Interval Revision</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<!-- #include file= "LocalFunctionAjax.htm" -->
	<script id="clientEventHandlersJS" type="text/javascript">

		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

		}
	</script>
	<script type="text/javascript" id="Script1" language="javascript">
		function openTranDetail() {
			str = "wfReports.aspx"
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openFile() {
			str = "wfExportToExcel.aspx"
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
	</script>
	<style type="text/css">
		.activerow {
			/* yellow*/
			background-color: rgb(255, 203, 96); /* red 
           background-color: #ffd9eb  ;*/
		}

		.pagingclass {
			margin-top: 2px;
			padding: 1px;
			border: 1px solid #ddd;
		}
	</style>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
	<form id="frmChangeLocation" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
			runat="server">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<script type="text/javascript">
			//event handler for end request i.e last event in client page cycle.
			Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
			//event handler for begin request i.e before sending request to the server
			Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);

			var element;
			var timerId;
			var timeoutforblink;
			var hideRowHighlight = false;

			function endRequestHandler(sender, args) {
				var tempval = parseInt($("#gridrowindex").val()); //row number ..0 is header row..
				if (tempval) {
					$("#<%=gdPartSearch.ClientID %> tr:eq(" + tempval + ")").addClass('activerow'); // add highligth class
					if (hideRowHighlight) {   //if ok or close button action was performed of child modal popup window
						var elem;
						var tempaction = $("#gridrowaction").val(); //action to be performed

						//button close of popup windows
						//remove highlight row class... and return from function
						if (tempaction == "close") {
							$("#<%=gdPartSearch.ClientID %> tr:eq(" + tempval + ")").removeClass('activerow');
							$("#gridrowaction").val('');
							return;
						}
						//change location button ok event
						//blink location column of the row for perticular interval
						else if (tempaction == "location") {
							$("#<%=gdPartSearch.ClientID %> tr:eq(" + tempval + ")").removeClass('activerow');
							elem = $("#<%=gdPartSearch.ClientID %> tr:eq(" + tempval + ") td:eq(3)");
							$("#gridrowaction").val('');
						}

						//change part/store button ok event
						//blink partType and Store columns of the row for perticular interval
						else if (tempaction == "partstore") {
							$("#<%=gdPartSearch.ClientID %> tr:eq(" + tempval + ")").removeClass('activerow');
							elem = $("#<%=gdPartSearch.ClientID %> tr:eq(" + tempval + ") td:eq(3),#<%=gdPartSearch.ClientID %> tr:eq(" + tempval + ") td:eq(4)");
							$("#gridrowaction").val('');
						}
						else {
							return;
						}

					}


				}
			}

			function BeginRequestHandler(sender, args) {
				clearTimeout(timerId);
				element = args.get_postBackElement();
				//change part/store popup ok button event occur 
				if (element.id == "btnChangePartOk") {
					hideRowHighlight = true;
					$("#gridrowaction").val('partstore');
				}
				//any of change popup close button event occur 
				else if (element.id == "btnChangePartClose" || element.id == "btnLocationClose") {
					hideRowHighlight = true;
					$("#gridrowaction").val('close');
				}
				//change parttype ||change location link event occur
				//reset rowindex value if other grid event occurs
				else if (element.id == "gdPartSearch") {
					if ($("#gridrowaction").val() != "gridrow") {
						$("#gridrowindex").val('');
					}
				}
				//any other events
				else {
					$("#gridrowindex").val('');
				}
			}

			//stop blinking
			function TimeOut(val, action) {
				var tempelem;
				if (action == "location") {
					tempelem = $("#<%=gdPartSearch.ClientID %> tr:eq(" + val + ") td:eq(3)");
					tempelem.removeClass('activerow');

				}
				else if (action == "partstore") {
					tempelem = $("#<%=gdPartSearch.ClientID %> tr:eq(" + val + ") td:eq(4),#<%=gdPartSearch.ClientID %> tr:eq(" + val + ") td:eq(4)");
					tempelem.removeClass('activerow');
				}
				else {
					return;
				}
				$("#gridrowindex").val('');
				hideRowHighlight = false;
				clearInterval(timeoutforblink);
			}
		</script>
		<div>
			<table id="tblmain" class="clstablelistout">
				<tr>
					<td>
						<asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
							<table id="tblInner" class="clstablelistin">
								<tr class="clsFormHeader1Newstyle">
									<td colspan="2">
										<table width="100%">
											<tr>
												<td>
													<span class="clsFormHeader">Calibration Interval Revision</span>
												</td>
												<td align="right">
													<asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to close"
														CausesValidation="False"></asp:Button>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td align="left">
										<asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
											<ContentTemplate>
												<table id="Table1">
													<tr>
														<td width="96px" colspan="3"></td>
														<td colspan="5">
															<asp:Label ID="lblStoreCount" ForeColor="DarkBlue" runat="server" Font-Size="XX-Small"
																Font-Bold="true" class="clsLabelAuto"></asp:Label>
														</td>
													</tr>
													<tr>
														<td>
															<span id="lblSearch" class="clsLabelAuto">Part No. / Description</span>
														</td>
														<td>
															<asp:TextBox ID="txtSearchFor" runat="server" CssClass="clsTextBoxTagSearch" EnableViewState="false"
																MaxLength="100"></asp:TextBox>
														</td>
														<td></td>
														<td></td>
														<td></td>
														<td></td>
														<td></td>
														<td></td>
													</tr>
												</table>
												<asp:HiddenField ID="StoreValue" runat="server" ClientIDMode="Static" />
											</ContentTemplate>
											<Triggers>
												<asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="click" />
											</Triggers>
										</asp:UpdatePanel>
									</td>
									<td align="right">
										<div>
											<asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/Search2.png"
												CssClass="clsSearch2btn" ToolTip="Click to Search as per criteria."
												ValidationGroup="1" CausesValidation="true" />
										</div>
									</td>
								</tr>
								<tr>
									<td>
										<br />
									</td>
								</tr>
								<tr>
									<td align="left" colspan="2">
										<asp:UpdatePanel runat="server" ID="upnlgrid" UpdateMode="Conditional">
											<ContentTemplate>
												<div style="width: 100%">
													<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Parts :</asp:Label>
												</div>
												<div style="width: 100%">
													<asp:GridView ID="gdPartSearch" EnableViewState="false" runat="server" PageSize="25"
														AutoGenerateColumns="False" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
														ShowHeaderWhenEmpty="True" AllowPaging="True"
														AllowSorting="True" OnPageIndexChanging="gdPartSearch_PageIndexChanging">
														<AlternatingRowStyle CssClass="clsdgAltItem" />
														<RowStyle CssClass="clsdgItem" />
														<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
														<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
														<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
														<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />

														<Columns>
															<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
															<asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="Part No.">
																<HeaderStyle Wrap="False" HorizontalAlign="Left" Width="125px"></HeaderStyle>
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundField>
															<asp:BoundField DataField="ItemDescription" SortExpression="ItemDescription" HeaderText="Description">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundField>
															<asp:BoundField DataField="BenchmarkMonths" SortExpression="BenchmarkMonths" HeaderText="Interval">
																<HeaderStyle Wrap="False" HorizontalAlign="Right"></HeaderStyle>
																<ItemStyle HorizontalAlign="Right"></ItemStyle>
															</asp:BoundField>
															<asp:BoundField DataField="CalibrationPeriodIn" SortExpression="CalibrationPeriodIn"
																HeaderText="Period">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundField>
															<asp:ButtonField Text="Update" HeaderText="Update" CommandName="ChangeCalibrationItemFrequency">
																<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:ButtonField>
														</Columns>
														<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
														<PagerStyle CssClass="paging" HorizontalAlign="Right" />
													</asp:GridView>
												</div>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td align="left"></td>
								</tr>
							</table>
						</asp:Panel>
					</td>
				</tr>
			</table>
		</div>
		<!-- Part Type -->
		<div style="display: none">
			<asp:Button runat="server" ID="btndummyPartStore" Text="Dummy Part Type" />
		</div>
		<asp:Panel runat="server" ID="pnlChangeCalibrationItemFrequency">
			<div>
				<table class="clstablelistout" id="Table5">
					<tr>
						<td align="left" class="style1">
							<asp:UpdatePanel ID="upnlChangeCalibrationItemFrequency" UpdateMode="Conditional"
								runat="server">
								<ContentTemplate>
									<table class="clstablelistin" id="Table6">
										<tr class="clsFormHeader1Newstyle">
											<td colspan="3">
												<table width="100%">
													<tr>
														<td style="width:400px;">
															<span id="lblHeader" class="clsFormHeader">Calibration Interval Revision</span>
														</td>
														<td align="right">
															<asp:Button ID="btnChangePartOk" runat="server" CssClass="clsbtnH clsinfoH" Text="Ok"
																ToolTip="Click to Save Changes"></asp:Button>
															<asp:Button ID="btnChangePartClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
																Text="Close" ToolTip=" Click to Close" CausesValidation="False"></asp:Button>
														</td>
													</tr>
												</table>

											</td>
										</tr>
										<tr>
											<td>
												<span id="lblPartNo" class="clsLabel">Part No. / Description</span>
											</td>
											<td>
												<asp:Label ID="lblPartNumber" runat="server" CssClass="clsLabelHeader"></asp:Label>
											</td>
											<td>
												<asp:Label ID="lblPartDescription" runat="server" CssClass="clsLabelHeader"></asp:Label>
											</td>
										</tr>
										<tr>
											<td>
												<span id="lblCurrentPT" class="clsLabelAuto">Current Interval </span>
											</td>
											<td align="left">
												<asp:TextBox ID="txtCurrentCalibrationItemFrequency" runat="server" ClientIDMode="Static"
													CssClass="clsTextBoxRightAlignSmall_Ajax" MaxLength="4" Width="38px" ReadOnly="True"
													BackColor="#E0E0E0"></asp:TextBox>
											</td>
											<td align="left">
												<asp:TextBox ID="txtCurrentCalibrationItemFrequencyIn" runat="server" CssClass="clsTextBoxTagSearch"
													ReadOnly="True" MaxLength="50" BackColor="#E0E0E0"></asp:TextBox>
											</td>
										</tr>
										<tr>
											<td></td>
											<td></td>
											<td></td>
										</tr>
										<tr>
											<td>
												<span id="lblCurrentStore" class="clsLabelAuto">Change Interval</span>
											</td>
											<td align="left">
												<asp:TextBox ID="txtBenchmarkMonths" runat="server" ClientIDMode="Static" CssClass="clsTextBoxRightAlignSmall_Ajax"
													MaxLength="4" Width="38px"></asp:TextBox>
											</td>
											<td align="left">
												<asp:DropDownList ID="cmbCalibrationPeriodIn" runat="server" ClientIDMode="Static"
													CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="Name" DataValueField="ID" Width="100px">
												</asp:DropDownList>
											</td>
										</tr>
									</table>
								</ContentTemplate>
							</asp:UpdatePanel>
						</td>
					</tr>
				</table>
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
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopUpChangeCalibrationItemFrequency" runat="server"
			TargetControlID="btndummyPartStore" PopupControlID="pnlChangeCalibrationItemFrequency"
			BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<!--End Part Type -->
		<input id="gridrowindex" type="hidden" value="" />
		<input id="gridrowaction" type="hidden" value="" />
		<script type="text/javascript">
			$(document).ready(function () {
				$("#<%=gdPartSearch.ClientID %> tr td a").live("click", function () {
					var temp = $(this).parent().parent()[0].rowIndex;
					$("#gridrowindex").val(temp);
					$("#gridrowaction").val('gridrow');
				});
			});
		</script>
	</form>
</body>
</html>
