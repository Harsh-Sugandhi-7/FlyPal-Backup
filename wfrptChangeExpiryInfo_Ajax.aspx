<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptChangeExpiryInfo_Ajax.aspx.vb"
	Inherits="Flypal.wfrptChangeExpiryInfo_Ajax" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<title>Change Part Expiry Info</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
</head>
<style type="text/css">

	/*Added By Harsh to change th location of Top & Bottom Navigation Link to make it Visible */
	#top {
		margin-top: 645px !important;
		margin-left: 1140px !important;
	}

	#bottom {
		margin-top: 100px !important;
		margin-left: 1124px !important;
	}
</style>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
	<form id="wfgroup" method="post" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
			runat="server">
		</asp:ScriptManager>
		<div>
			<table class="clstablelistout" id="tblmain">
				<tr>
					<td>
						<asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
							<table class="clstablelistin" id="tblInner">
								<tr class="clsFormHeader1Newstyle">
									<td colspan="2">
										<table width="100%">
											<tr>
												<td>
													<span id="lbltitle" class="clsFormHeader">Change Part Expiry Info</span>
												</td>
												<td align="right">
													<asp:Button ID="btnClose" runat="server"
														CssClass="clsbtnH clsinfoH" ToolTip="Click to Close"
														Text="Close" CausesValidation="False"></asp:Button>
												</td>
											</tr>
										</table>

									</td>
								</tr>
								<tr>
									<td align="left">
										<asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<table id="Table1">
													<tr>
														<td>
															<span id="lblSearch" class="clsLabelAuto">Search</span>
														</td>
														<td>
															<asp:DropDownList ID="cmbSearch" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" onChange="ControlVisibilityForSearch();">
																<asp:ListItem Value="0">(All)</asp:ListItem>
																<asp:ListItem Value="1">Part No.</asp:ListItem>
															</asp:DropDownList>
														</td>
														<td>
															<span id="lblFor" class="clsLabelAuto" style="display	: none;">For</span>
														</td>
														<td>
															<asp:TextBox ID="txtSearchFor" runat="server" CssClass="clsTextBoxTagSearch" Style="display	: none;"
													MaxLength="100"></asp:TextBox>
														</td>
													</tr>
												</table>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
									<td align="right">
										<asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<table id="Table4">
													<tr>
														<td>
															<asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/Search2.png"
																CssClass="clsSearch2btn" ToolTip="Click to Search as per criteria."
																ValidationGroup="1" CausesValidation="true" />
														</td>
													</tr>
												</table>
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
									<td colspan="2">
										<asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<table width="100%">
													<tr>
														<td align="left">
															<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Parts :</asp:Label>
														</td>
													</tr>
													<tr>
														<td align="left">
															<asp:GridView ID="dgPartSearch" runat="server" AllowSorting="True" AllowPaging="True"
																DataKeyNames="ID" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
																AutoGenerateColumns="False" PageSize="10" ClientIDMode="Static" EnableViewState="false" ShowHeaderWhenEmpty="true">
																<AlternatingRowStyle CssClass="clsdgAltItem" />
																<RowStyle CssClass="clsdgItem" />
																<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
																<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
																<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
																<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
																<Columns>
																	<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
																	<asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="Part No.">
																		<HeaderStyle Wrap="False" Width="125px" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="False"></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="ItemDesc" SortExpression="ItemDesc" HeaderText="Description">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="SerialNo" SortExpression="SerialNo" HeaderText="Serial No.">
																		<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="StockBalQty" SortExpression="StockBalQty" HeaderText="Qty. in Stock">
																		<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																		<ItemStyle HorizontalAlign="Right"></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="DateFormatted" HeaderText="Receipt Date">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="ReceiptNo" SortExpression="ReceiptNo" HeaderText="Receipt No.">
																		<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="False"></ItemStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="ReleaseNoteNo" SortExpression="ReleaseNoteNo" HeaderText="Release Note No.">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="StartDateFormatted" HeaderText="Cure Date">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="ExpiryDateFormatted" HeaderText="Expiry Date">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="CureQtrYear" SortExpression="CureQtrYear" HeaderText="Cure Qtrs.">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="ExpQtrYear" SortExpression="ExpQtrYear" HeaderText="Expiry Qtrs.">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="ExpiryNA" SortExpression="ExpiryNA" HeaderText="Expiry NA">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:BoundField DataField="ExpiryUnlimited" SortExpression="ExpiryUnlimited" HeaderText="Expiry Unlimited">
																		<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	</asp:BoundField>
																	<asp:ButtonField Text="Change Expiry Info" HeaderText="Change Expiry Info" CommandName="ChangeExpiryInfo">
																		<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																		<ItemStyle Wrap="False"></ItemStyle>
																	</asp:ButtonField>
																</Columns>
															</asp:GridView>
														</td>
													</tr>
												</table>
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
					<div class="clsAjaxLoader" style="height:		100%;
		width: 100%;
		left: 0;
		position: fixed;
		background-color: #000000;
		top: 0;
		z-index: 99999;
		">
			</div>
					<div style="positio		n: fixed;
		top: 50%;
		left: 50%;
		margin-left: -27px;
		margin-top: -27px;
		z-index: 100000;
		">
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
		<!-- Change Expiry Info-->
		<div style="display		: none">
	<asp:Button runat="server" ID="btnDummyChangeExpiryInfo" Text="Dummy Change Expiry Info" />
		</div>
		<asp:Panel runat="server" ID="pnlChangeExpiryInfo" Style="display		: none">
	<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlChangeExpiryInfo">
				<ContentTemplate>
					<asp:Panel runat="server" ID="pnlExpiryInfo" Visible="false">
						<table class="clstablelistout" id="Table5">
							<tr>
								<td>
									<table class="clstablelistin" id="Table6" width="100%">
										<tr class="clsFormHeader1Newstyle">
											<td colspan="5">
												<table width="100%">
													<tr>
														<td>
															<span id="lblChangePartExpiryInfoPopUp" class="clsFormHeader">Change Part Expiry Info</span>
														</td>
														<td align="right">
															<asp:Button ID="btnOk" runat="server" CssClass="clsbtnH clsinfoH"
																Text="Ok" ToolTip="Click to Change Expiry Information"></asp:Button>
															<asp:Button ID="btnCloseChangeExpiryInfo" TabIndex="0" runat="server"
																CssClass="clsbtnH clsinfoH" Text="Close"
																ToolTip="Click to Close" CausesValidation="False"></asp:Button>
														</td>
													</tr>
												</table>
											</td>
										</tr>
										<tr>
											<td colspan="5">
												<asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="clsValidationSummary"></asp:ValidationSummary>
												<asp:CustomValidator ID="cvStartDate" runat="server" OnServerValidate="customvalidate"
													Display="None" ErrorMessage="Expiry Date should be Later to Start Date." ControlToValidate="txtStartDate"></asp:CustomValidator>
												<asp:CustomValidator ID="cvExpiryDate" runat="server" OnServerValidate="customvalidate"
													Display="None" ErrorMessage="Expiry Date should be Later to Start Date." ControlToValidate="txtExpiryDate"></asp:CustomValidator>
												<asp:CustomValidator ID="cvcureqtrs" runat="server" OnServerValidate="CustomValidate"
													Display="None" ErrorMessage="." ControlToValidate="txtCureQtrs"></asp:CustomValidator>
												<asp:CustomValidator ID="cvCureYrs" runat="server" OnServerValidate="CustomValidate"
													Display="None" ErrorMessage="." ControlToValidate="txtCureYear"></asp:CustomValidator>
												<asp:CustomValidator ID="cvExpQtrs" runat="server" OnServerValidate="CustomValidate"
													Display="None" ErrorMessage="." ControlToValidate="txtExpQrts"></asp:CustomValidator>
												<asp:CustomValidator ID="cvExpYrs" runat="server" OnServerValidate="CustomValidate"
													Display="None" ErrorMessage="." ControlToValidate="txtExpYear"></asp:CustomValidator>
											</td>
										</tr>
										<tr>
											<td></td>
											<td></td>
											<td>
												<asp:Label ID="lblExpPeriod" runat="server" CssClass="clsLabelAuto" Text="<%# mReceiptInfo.ExpiryPeriod %>">
												</asp:Label>
											</td>
											<td></td>
											<td></td>
										</tr>
										<tr>
											<td></td>
											<td></td>
											<td>
												<asp:CheckBox ID="chkIsExpiryNA" runat="server" AutoPostBack="True" Checked="<%# mReceiptInfo.IsExpiryNA %>"
													CssClass="clsCheckBox" Text="N/A" Visible='<%# iif(mReceiptInfo.ExpiryMonth=0,True,False) %>' />
											</td>
											<td>
												<asp:CheckBox ID="chkIsExpiryUnlimited" runat="server" AutoPostBack="True" Checked="<%# mReceiptInfo.IsExpiryUnlimited %>"
													CssClass="clsCheckBox" Text="Unlimited" Visible='<%# iif(mReceiptInfo.ExpiryMonth=0,True,False) %>' />
											</td>
											<td></td>
										</tr>
										<tr>
											<td></td>
											<td>
												<span id="lblStartDate" class="clsLabel">Cure Date</span>
											</td>
											<td>
												<table id="Table7">
													<tr>
														<td>
															<%--<uc1:SICalendar ID="txtStartDate" runat="server"></uc1:SICalendar>--%>
															<asp:TextBox ID="txtStartDate" CssClass="clsTextBoxTagSearchDate" ClientIDMode="Static"
																runat="server" CausesValidation="true" AutoPostBack="true"></asp:TextBox>
															<cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtStartDate"></cc2:CalendarExtender>
															<cc2:TextBoxWatermarkExtender TargetControlID="txtStartDate" ID="FromDate_watermarkextender"
																ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
														</td>
													</tr>
												</table>
											</td>
											<td>
												<span id="lstExpiryDate" class="clsLabel">Expiry Date</span>
											</td>
											<td>
												<%--<uc1:SICalendar ID="txtExpiryDate" runat="server"></uc1:SICalendar>--%>
												<asp:TextBox ID="txtExpiryDate" CssClass="clsTextBoxTagSearchDate" ClientIDMode="Static"
													runat="server" CausesValidation="true" AutoPostBack="true"></asp:TextBox>
												<cc2:CalendarExtender ID="calExpiryDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
													Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtExpiryDate"></cc2:CalendarExtender>
												<cc2:TextBoxWatermarkExtender TargetControlID="txtExpiryDate" ID="ExpiryDate_watermarkextender"
													ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
													WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
											</td>
										</tr>
										<tr>
											<td></td>
											<td>
												<span id="Label3" class="clsLabel">Cure Quarter</span>
											</td>
											<td>
												<table id="Table23">
													<tr>
														<td>
															<asp:TextBox ID="txtCureQtrs" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																Enabled="<%# (mReceiptInfo.ExpiryQuarter > 0) Or (mReceiptInfo.ExpiryMonth = 0 And mReceiptInfo.ExpiryQuarter = 0) %>"
																Width="24px" AutoPostBack="True" Text="<%# mReceiptInfo.CureQtrs %>" ToolTip="Enter Quarter."
																MaxLength="1">
															</asp:TextBox>
															<asp:Label ID="Label5" runat="server">/</asp:Label>
															<asp:TextBox ID="txtCureYear" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																Enabled="<%# (mReceiptInfo.ExpiryQuarter > 0) Or (mReceiptInfo.ExpiryMonth = 0 And mReceiptInfo.ExpiryQuarter = 0) %>"
																Width="56px" AutoPostBack="True" Text="<%# mReceiptInfo.CureYear %>" ToolTip="Enter Cure Year."
																MaxLength="4">
															</asp:TextBox>
														</td>
													</tr>
												</table>
											</td>
											<td>
												<span id="Label4" class="clsLabel">Expiry Quarter</span>
											</td>
											<td>
												<asp:TextBox ID="txtExpQrts" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
													Enabled="<%# (mReceiptInfo.ExpiryQuarter > 0) Or (mReceiptInfo.ExpiryMonth = 0 And mReceiptInfo.ExpiryQuarter = 0) %>"
													Width="24px" Text="<%# mReceiptInfo.ExpQtrs %>" ToolTip="Enter Expiry Quarter."
													MaxLength="1" AutoPostBack="True">
												</asp:TextBox><asp:Label ID="Label6" runat="server">/</asp:Label>
												<asp:TextBox ID="txtExpYear" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
													Enabled="<%# (mReceiptInfo.ExpiryQuarter > 0) Or (mReceiptInfo.ExpiryMonth = 0 And mReceiptInfo.ExpiryQuarter = 0) %>"
													Width="56px" Text="<%# mReceiptInfo.ExpYear %>" ToolTip="Enter Expiry Year."
													MaxLength="4" AutoPostBack="True">
												</asp:TextBox>
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
		<cc2:ModalPopupExtender ID="mdlPopUpChangeExpiryInfo" runat="server" TargetControlID="btnDummyChangeExpiryInfo"
			PopupControlID="pnlChangeExpiryInfo" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<!-- End Change Part Expity Info -->
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
					case 3:
						$("#txtSearchFor").val('');
						$("#lblFor").css('display', 'none');
						$("#txtSearchFor").css('display', 'none');
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
					$("#<%=dgPartSearch.ClientID %> tr:eq(" + tempval + ")").addClass('activerow'); // add highligth class
					if (hideRowHighlight) {   //if ok or close button action was performed of child modal popup window
						var elem;
						var tempaction = $("#gridrowaction").val(); //action to be performed

						if (tempaction == "close") {
							$("#dgPartSearch tr:eq(" + tempval + ")").removeClass('activerow');
							$("#gridrowaction").val('');
							return;
						}
						//change Expiry Info button ok event
						//blink Expiry columns of the row for perticular interval
						else if (tempaction == "ExpiryInfo") {
							$("#<%=dgPartSearch.ClientID %> tr:eq(" + tempval + ")").removeClass('activerow');
							elem = $("#<%=dgPartSearch.ClientID %> tr:eq(" + tempval + ") td:eq(7),#<%=dgPartSearch.ClientID %> tr:eq(" + tempval + ") td:eq(8),#<%=dgPartSearch.ClientID %> tr:eq(" + tempval + ") td:eq(9),#<%=dgPartSearch.ClientID %> tr:eq(" + tempval + ") td:eq(10)");
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

				//change Expiry Info popup ok button event occur 
				if (element.id == "btnOk") {
					hideRowHighlight = true;
					$("#gridrowaction").val('ExpiryInfo');
				}
				//any of change popup close button event occur 
				else if (element.id == "btnCloseChangeExpiryInfo") {
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

				if (action == "ExpiryInfo") {
					tempelem = $("#<%=dgPartSearch.ClientID %> tr:eq(" + val + ") td:eq(7),#<%=dgPartSearch.ClientID %> tr:eq(" + val + ") td:eq(8),#<%=dgPartSearch.ClientID %> tr:eq(" + val + ") td:eq(9),#<%=dgPartSearch.ClientID %> tr:eq(" + val + ") td:eq(10)");
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
