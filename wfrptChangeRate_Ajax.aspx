<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptChangeRate_Ajax.aspx.vb"
	EnableEventValidation="false" Inherits="Flypal.wfrptChangeRate_Ajax" %>

<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<title>Change Rate</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<!-- #include file= "LocalFunctionAjax.htm" -->
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
	<form id="wfgroup" method="post" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true" runat="server">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<div>
			<table class="clstablelistout" id="tblmain">
				<tr>
					<td>
						<asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
							<table id="tblInner" class="clstablelistin">
								<tr>
									<td colspan="2" class="clsFormHeader1Newstyle">
										<table>
											<tr>
												<td style="width: 99%" valign="middle">
													<span class="clsFormHeader">Change Part Rate</span>
												</td>
												<td>
													<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlBtns">
														<ContentTemplate>
															<asp:Button ID="btnClose" CssClass="clsbtnH clsinfoH" runat="server" Text="Close" ToolTip="Click to Close"
																CausesValidation="False"></asp:Button>
														</ContentTemplate>
														<Triggers>
															<asp:AsyncPostBackTrigger ControlID="btnFindNow" EventName="click" />
														</Triggers>
													</asp:UpdatePanel>
												</td>

											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td align="left">
										<table id="Table1" class="clsTable1">
											<tr>
												<td>
													<asp:Label ID="lblSearch" runat="server" CssClass="clsLabelAuto">Search</asp:Label>
												</td>
												<td>
													<asp:DropDownList ID="cmbSearch" onchange="ControlVisibilityForSearch();" runat="server"
														CssClass="clsTextBoxTagSearchComboNewstyle" ClientIDMode="Static">
														<asp:ListItem Value="0">(All)</asp:ListItem>
														<asp:ListItem Value="1">Part No.</asp:ListItem>
														<asp:ListItem Value="2">Location</asp:ListItem>
													</asp:DropDownList>
												</td>
												<td>
													<span id="lblFor" class="clsLabelAuto" style="display: none;">For</span>
												</td>
												<td>
													<asp:TextBox ID="txtSearchFor" runat="server" CssClass="clsTextBoxTagSearch" ClientIDMode="Static"
														Style="display: none;" MaxLength="100"></asp:TextBox>
												</td>
											</tr>
										</table>
									</td>
									<td align="right">
										<table id="Table4">
											<tr>
												<td>
													<%--<asp:Button ID="btnFindNow" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
                                                        Text="Find Now" ToolTip="Click to find as per criteria"></asp:Button>--%>
													<asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png"
														CssClass="clsSearch2btn" ToolTip="Click to find as per criteria." ValidationGroup="1"
														CausesValidation="true" />
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td>
										<br />
									</td>
								</tr>
								<tr>
									<td colspan="2" align="left">
										<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlGrid">
											<ContentTemplate>
												<div style="width: 100%; margin-bottom: 3px;">
													<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Parts :</asp:Label>
												</div>
												<div style="width: 100%">
													<asp:GridView ID="dgPartSearch" runat="server" ClientIDMode="Static" PageSize="25" ShowHeaderWhenEmpty="true"
														AutoGenerateColumns="False" CssClass="clsGridNewStyle" AllowPaging="True"
														AllowSorting="false" CellPadding="5" ForeColor="Black" GridLines="Horizontal">
														<AlternatingRowStyle CssClass="clsdgAltItem" />
														<RowStyle CssClass="clsdgItem" />
														<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
														<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
														<PagerSettings FirstPageText="First" LastPageText="Last" />
														<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
														<Columns>
															<asp:BoundField DataField="PartNoDescription" SortExpression="ItemName" HeaderText="Part No./Description" HtmlEncode="False">
																<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																<ItemStyle Wrap="true"></ItemStyle>
															</asp:BoundField>
															<%-- <asp:BoundField DataField="ItemDesc" SortExpression="ItemDesc" HeaderText="Description">
                                                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                            </asp:BoundField>--%>
															<asp:BoundField DataField="SerialNo" SortExpression="SerialNo" HeaderText="Serial No.">
																<HeaderStyle Wrap="False" HorizontalAlign="Right"></HeaderStyle>
																<ItemStyle HorizontalAlign="Right"></ItemStyle>
															</asp:BoundField>
															<asp:BoundField DataField="Location" SortExpression="Location" HeaderText="Location">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundField>
															<asp:BoundField DataField="InvoiceNo" SortExpression="InvoiceNo" HeaderText="Invoice No.">
																<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundField>
															<asp:BoundField DataField="CRate" SortExpression="CRate" HeaderText="Rate" DataFormatString="{0:#00.00}">
																<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																<ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
															</asp:BoundField>
															<asp:BoundField DataField="DateFormatted" HeaderText="Date">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundField>
															<asp:BoundField DataField="StockBalQty" SortExpression="StockBalQty" HeaderText="Qty. in Stock">
																<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																<ItemStyle HorizontalAlign="Right"></ItemStyle>
															</asp:BoundField>
															<asp:BoundField DataField="ReceiptNo" SortExpression="ReceiptNo" HeaderText="Receipt No.">
																<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundField>
															<asp:BoundField DataField="ReleaseNoteNo" SortExpression="ReleaseNoteNo" HeaderText="Release Note No.">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundField>
															<asp:BoundField DataField="CurrencyName" HeaderText="Currency"></asp:BoundField>
															<asp:BoundField DataField="ConversionFactor" HeaderText="Factor">
																<HeaderStyle HorizontalAlign="Right" />
																<ItemStyle HorizontalAlign="Right" />
															</asp:BoundField>
															<asp:BoundField Visible="False" DataField="InvoiceID" HeaderText="Invoice ID"></asp:BoundField>
															<asp:BoundField Visible="False" DataField="InvoiceItemID" HeaderText="InvoiceItem ID"></asp:BoundField>
															<asp:ButtonField Text="Change Rate" HeaderText="Change Rate" CommandName="ChangeRate">
																<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																<ItemStyle Wrap="False" ForeColor="blue"></ItemStyle>
															</asp:ButtonField>
														</Columns>
														<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
														<PagerStyle HorizontalAlign="Right" CssClass="paging" />
													</asp:GridView>
												</div>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>

							</table>
						</asp:Panel>
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
		<!-- Change Rate-->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyRate" Text="Dummy Rate" />
		</div>
		<asp:Panel runat="server" ID="pnlChangeRate" Style="display: none">
			<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlChangeRate">
				<ContentTemplate>
					<asp:Panel runat="server" ID="pnlRate" Visible="false">
						<table class="clstablelistout" id="Table5">
							<tr>
								<td>
									<table class="clstablelistin" id="Table6">
										<tr>
											<td colspan="3" class="clsFormHeader1Newstyle">
												<table width="100%">
													<tr>
														<td>
															<asp:Label ID="lblTitle" CssClass="clsFormHeader" runat="server">
                                                                Change Rate
															</asp:Label>
														</td>
													</tr>
												</table>
											</td>
										</tr>
										<tr>
											<td colspan="3">
												<asp:ValidationSummary ID="ValidationSummary2" ValidationGroup="rate" runat="server"
													CssClass="clsValidationSummary"></asp:ValidationSummary>
												<asp:RequiredFieldValidator ID="rfvCR" runat="server" ValidationGroup="rate" CssClass="clsLabelAuto"
													ControlToValidate="txtChangeRate" ErrorMessage="Enter Change Rate" Display="None"></asp:RequiredFieldValidator>
											</td>
										</tr>
										<tr>
											<td></td>
											<td>
												<span id="lblCurrentLocation" class="clsLabel">Current Rate </span>
											</td>
											<td align="left" colspan="1">
												<asp:TextBox ID="txtCurrentRate" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
													BackColor="#E0E0E0" MaxLength="12" ReadOnly="True" Width="100px"></asp:TextBox>
											</td>
										</tr>
										<tr>
											<td>
												<span id="lblChangeRate1" class="clsLabelStar">*</span>
											</td>
											<td>
												<span id="lblChangeRat" class="clsLabel">Change Rate</span>
											</td>
											<td align="left">
												<asp:TextBox ID="txtChangeRate" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" onkeypress="return validateText('D',1,event)"
													MaxLength="12" Width="100px"></asp:TextBox>
											</td>
										</tr>
										<tr>
											<td></td>
											<td></td>
											<td align="right">
												<asp:Button ID="btnRateOk" runat="server" CssClass="clsbtnH clsinfoH1" ValidationGroup="rate"
													Text="Ok" ToolTip="Click to Add New Rate"></asp:Button>
												<asp:Button ID="btnRateClose" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1"
													ToolTip="Click to Close" Text="Close" CausesValidation="False"></asp:Button>
											</td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</asp:Panel>
				</ContentTemplate>
			</asp:UpdatePanel>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopUpChangeRate" runat="server" TargetControlID="btnDummyRate"
			PopupControlID="pnlChangeRate" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<!-- End Change Location -->
		<input id="gridrowindex" type="hidden" value="" />
		<input id="gridrowaction" type="hidden" value="" />
		<script type="text/javascript">
			$(document).ready(function () {

				ControlVisibilityForSearch();
			});

			function ControlVisibilityForSearch() {
				var dd = $get("cmbSearch");
				switch (dd.selectedIndex) {
					case 0:
						$("#txtSearchFor").val('');
						$("#lblFor").css('display', 'none');
						$("#txtSearchFor").css('display', 'none');
						break;
					case 1:
						$("#txtSearchFor").val('');
						$("#lblFor").css('display', 'block');
						$("#txtSearchFor").css('display', 'block');
						break;
					case 2:
						$("#txtSearchFor").val('');
						$("#lblFor").css('display', 'block');
						$("#txtSearchFor").css('display', 'block');
						break;
				}
			}
		</script>
		<%-- Row Highlight--%>
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
					$("#dgPartSearch tr:eq(" + tempval + ")").addClass('activerow'); // add highligth class
					if (hideRowHighlight) {   //if ok or close button action was performed of child modal popup window
						var elem;
						var tempaction = $("#gridrowaction").val(); //action to be performed

						//button close of popup windows
						//remove highlight row class... and return from function
						if (tempaction == "close") {
							$("#dgPartSearch tr:eq(" + tempval + ")").removeClass('activerow');
							$("#gridrowaction").val('');
							return;
						}
						//change Rate button ok event
						//blink Rate column of the row for perticular interval
						else if (tempaction == "rate") {
							$("#dgPartSearch tr:eq(" + tempval + ")").removeClass('activerow');
							elem = $("#dgPartSearch tr:eq(" + tempval + ") td:eq(4)");
							$("#gridrowaction").val('');
						}

						else {
							return;
						}
						//blink column function
						timeoutforblink = setInterval(function () {

							if (elem.hasClass('activerow')) {
								elem.removeClass('activerow');
							}
							else {
								elem.addClass('activerow');
							}

						}, 500);
						//stop blink column
						timerId = setTimeout("TimeOut(" + tempval + ",'" + tempaction + "')", 3000);
					}


				}
			}

			function BeginRequestHandler(sender, args) {
				clearTimeout(timerId);
				element = args.get_postBackElement();
				//change location popup ok button event occur
				if (element.id == "btnRateOk") {
					hideRowHighlight = true;
					$("#gridrowaction").val('rate');
				}
				//any of change popup close button event occur 
				else if (element.id == "btnRateClose") {
					hideRowHighlight = true;
					$("#gridrowaction").val('close');
				}
				//change parttype ||change location link event occur
				//reset rowindex value if other grid event occurs
				else if (element.id == "dgPartSearch") {
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
				if (action == "rate") {
					tempelem = $("#dgPartSearch tr:eq(" + val + ") td:eq(4)");
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
		<script type="text/javascript">
			$(document).ready(function () {
				$("#dgPartSearch tr td a").live("click", function () {
					var temp = $(this).parent().parent()[0].rowIndex;
					$("#gridrowindex").val(temp);
					$("#gridrowaction").val('gridrow');
				});
			});
		</script>
	</form>
</body>
</html>
