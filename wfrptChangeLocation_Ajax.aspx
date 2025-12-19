<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptChangeLocation_Ajax.aspx.vb"
	EnableEventValidation="false" Inherits="Flypal.wfrptChangeLocation_Ajax" %>

<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<title>Change Part Location/Type/Store</title>
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
							elem = $("#<%=gdPartSearch.ClientID %> tr:eq(" + tempval + ") td:eq(4),#<%=gdPartSearch.ClientID %> tr:eq(" + tempval + ") td:eq(8)");
							$("#gridrowaction").val('');
						}
						else {
							return;
						}
						//blink column function
						//                    timeoutforblink = setInterval(function () {

						//                        if (elem.hasClass('activerow')) {
						//                            elem.removeClass('activerow');
						//                        }
						//                        else {
						//                            elem.addClass('activerow');
						//                        }

						//                    }, 500);
						//                    //stop blink column
						//                    timerId = setTimeout("TimeOut(" + tempval + ",'" + tempaction + "')", 3000);
					}


				}
			}

			function BeginRequestHandler(sender, args) {
				clearTimeout(timerId);
				element = args.get_postBackElement();
				//change location popup ok button event occur
				if (element.id == "btnLocationOk") {
					hideRowHighlight = true;
					$("#gridrowaction").val('location');
				}
				//change part/store popup ok button event occur 
				else if (element.id == "btnChangePartOk") {
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
					tempelem = $("#<%=gdPartSearch.ClientID %> tr:eq(" + val + ") td:eq(4),#<%=gdPartSearch.ClientID %> tr:eq(" + val + ") td:eq(8)");
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
								<tr>
									<td colspan="2" class="clsFormHeader1Newstyle">
										<table>
											<tr>
												<td style="width: 99%" valign="middle">
													<span class="clsFormHeader">Change Part Location / Type / Store</span>
												</td>
												<td>
													<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlBtns">
														<ContentTemplate>
															<table>
																<tr>
																	<td>
																		<asp:Button ID="btnExport" CssClass="clsbtnH clsinfoH" runat="server" Text="Export to Excel"
																			ToolTip="Click to Export to Excel" CausesValidation="False"></asp:Button>
																	</td>
																	<td>
																		<asp:Button ID="btnClose" CssClass="clsbtnH clsinfoH" runat="server" Text="Close"
																			ToolTip="Click to Close" CausesValidation="False"></asp:Button>
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
									<td align="left">
										<asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
											<ContentTemplate>
												<table id="Table1">
													<tr>
														<td width="96px" colspan="3"></td>
														<td colspan="5">
															<asp:Label ID="lblStoreCount" ForeColor="DarkBlue" runat="server" Font-Size="XX-Small" Font-Bold="true" class="clsLabelAuto"></asp:Label>
														</td>
													</tr>
													<tr>
														<td>
															<span id="lblSearch" class="clsLabelAuto">Search</span>
														</td>
														<td>
															<asp:DropDownList ID="cmbSearch" runat="server" CssClass="clsTextBoxTagSearchComboSmall1" AutoPostBack="True">
																<asp:ListItem Value="0">(All)</asp:ListItem>
																<asp:ListItem Value="1">Part No.</asp:ListItem>
																<asp:ListItem Value="2">Location</asp:ListItem>
																<asp:ListItem Value="3">Part Type</asp:ListItem>
																<asp:ListItem Value="4">Store</asp:ListItem>
															</asp:DropDownList>
														</td>
														<td>
															<asp:Label ID="lblFor" runat="server" CssClass="clsLabelAuto" Visible="False">For</asp:Label>
														</td>
														<td>
															<asp:TextBox ID="txtSearchFor" runat="server" CssClass="clsTextBoxTagSearch" Visible="False"
																EnableViewState="false" MaxLength="100"></asp:TextBox>
															<asp:DropDownList ID="cmbPartType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Visible="False"
																EnableViewState="false" DataValueField="ID" DataTextField="Name">
																<asp:ListItem Value="0">(All)</asp:ListItem>
																<asp:ListItem Value="1">New</asp:ListItem>
																<asp:ListItem Value="2">Overhaul</asp:ListItem>
																<asp:ListItem Value="3">Factory New</asp:ListItem>
																<asp:ListItem Value="4">Serviceable</asp:ListItem>
																<asp:ListItem Value="5">As Removed</asp:ListItem>
																<asp:ListItem Value="6">Repairable</asp:ListItem>
																<asp:ListItem Value="7">New Surplus</asp:ListItem>
																<asp:ListItem Value="8">On Request</asp:ListItem>
																<asp:ListItem Value="9">Repaired</asp:ListItem>
															</asp:DropDownList>
															<asp:DropDownList ID="cmbStore" ClientIDMode="Static" runat="server"
																CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="Name" DataValueField="ID"
																Visible="False" onChange="SetStoreValue()">
															</asp:DropDownList>
														</td>
														<td>
															<asp:CheckBox ID="chkBlankLocation" runat="server" CssClass="clsCheckBox" Text="Show Items Without Location" Checked="true" />
														</td>
														<td>&nbsp&nbsp
														</td>
														<td>
															<span id="lblCategory" class="clsLabelAuto">Category</span>
														</td>
														<td>
															<asp:DropDownList ID="cmbCategory" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
																DataTextField="Name">
															</asp:DropDownList>
														</td>
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
												ValidationGroup="1" CausesValidation="false" />
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
														AutoGenerateColumns="False" CssClass="clsGridNewStyle" AllowPaging="True"
														CellPadding="5" ForeColor="Black" GridLines="Horizontal" ShowHeaderWhenEmpty="True"
														AllowSorting="True" OnPageIndexChanging="gdPartSearch_PageIndexChanging">
														<AlternatingRowStyle CssClass="clsdgAltItem" />
														<RowStyle CssClass="clsdgItem" />
														<PagerSettings FirstPageText="First" LastPageText="Last" />
														<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
														<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
														<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
														<Columns>
															<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
															<asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="Part No.">
																<HeaderStyle Wrap="False" HorizontalAlign="Left" Width="125px"></HeaderStyle>
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundField>
															<asp:BoundField DataField="ItemDesc" SortExpression="ItemDesc" HeaderText="Description">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundField>
															<asp:BoundField DataField="SerialNo" SortExpression="SerialNo" HeaderText="Serial No.">
																<HeaderStyle Wrap="False" HorizontalAlign="Right"></HeaderStyle>
																<ItemStyle HorizontalAlign="Right"></ItemStyle>
															</asp:BoundField>
															<asp:BoundField DataField="Location" SortExpression="Location" HeaderText="Location">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundField>
															<asp:BoundField DataField="ItemTypeStatus" SortExpression="ItemTypeStatus" HeaderText="Part Type (Part Status)">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundField>
															<asp:BoundField DataField="DateFormatted" HeaderText="Receipt Date">
																<HeaderStyle Wrap="False"></HeaderStyle>
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
															<asp:BoundField DataField="Store" HeaderText="Store" SortExpression="Store">
																<HeaderStyle Font-Bold="True" HorizontalAlign="Left" Font-Italic="False" Font-Overline="False"
																	Font-Strikeout="False" Font-Underline="False" />
															</asp:BoundField>
															<asp:BoundField DataField="ReleaseNoteNo" SortExpression="ReleaseNoteNo" HeaderText="Release Note No.">
																<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundField>
															<asp:BoundField DataField="Category" SortExpression="Category" HeaderText="Category">
																<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundField>
															<asp:ButtonField Text="Change Location" HeaderText="Change Location" CommandName="ChangeLocation">
																<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																<ItemStyle Wrap="False" ForeColor="blue"></ItemStyle>
															</asp:ButtonField>
															<asp:ButtonField Text="Change Part Status / Store" HeaderText="Change Part Status / Store"
																CommandName="ChangePartType">
																<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																<ItemStyle Wrap="False" ForeColor="blue"></ItemStyle>
															</asp:ButtonField>
															<asp:BoundField Visible="False" DataField="ItemTypeID" HeaderText="ItemTypeID"></asp:BoundField>
															<asp:BoundField DataField="IsStoreChangeble" HeaderText="IsStoreChangeble" Visible="False"></asp:BoundField>
															<asp:BoundField DataField="StoreID" HeaderText="StoreID" Visible="False"></asp:BoundField>
														</Columns>
														<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
														<PagerStyle CssClass="paging" HorizontalAlign="Right" />
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
		</div>
		<!-- Change Location -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyLocation" Text="Dummy Location" />
		</div>
		<asp:Panel runat="server" ID="pnlChangeLocation" Style="display: none">
			<div>
				<table class="clstablelistout" id="Table2">
					<tr>
						<td align="right">
							<asp:UpdatePanel runat="server" ID="upnlLocation" UpdateMode="Conditional">
								<ContentTemplate>
									<table class="clstablelistin" id="Table3">
										<tr>
											<td colspan="3" align="left" class="clsFormHeader1Newstyle">
												<table width="100%">
													<tr>
														<td>
															<span id="lblTitle" class="clsFormHeader">Change Part Location </span>
														</td>
														<td align="right">
															<asp:Button ID="btnLocationOk" ValidationGroup="1"
																runat="server" CssClass="clsbtnH clsinfoH"
																Text="Ok" ToolTip="Click to Add New Location"></asp:Button>
															<asp:Button ID="btnLocationClose" TabIndex="0"
																runat="server" CssClass="clsbtnH clsinfoH"
																Text="Close" ToolTip="Click to Close"
																CausesValidation="False"></asp:Button>
														</td>
													</tr>
												</table>

											</td>
										</tr>
										<tr>
											<td colspan="3" align="left">
												<asp:ValidationSummary ID="ValidationSummary2" ValidationGroup="1" runat="server"
													CssClass="clsValidationSummary"></asp:ValidationSummary>
												<asp:RequiredFieldValidator ValidateEmptyText="true" ErrorMessage="Enter Change Location"
													CssClass="clsLabelAuto" ControlToValidate="txtChangedLocation" Display="None"
													ValidationGroup="1" runat="server" />
											</td>
										</tr>
										<tr>
											<td></td>
											<td>
												<span id="lblCurrentLocation" class="clsLabel">Current Location </span>
											</td>
											<td colspan="1">
												<asp:TextBox ID="txtCurrentLocation" runat="server" CssClass="clsTextBoxTagSearch" ReadOnly="True"
													MaxLength="50" BackColor="#E0E0E0"></asp:TextBox>
											</td>
										</tr>
										<tr>
											<td>
												<span id="lblChangeLocation1" class="clsLabelStar">*</span>
											</td>
											<td>
												<span id="lblChangeLocation" class="clsLabel">Change Location</span>
											</td>
											<td>
												<asp:TextBox ID="txtChangedLocation" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"></asp:TextBox>
											</td>
										</tr>
									</table>
								</ContentTemplate>
							</asp:UpdatePanel>
						</td>
					</tr>
				</table>
			</div>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopUpChangeLocation" runat="server" TargetControlID="btnDummyLocation"
			PopupControlID="pnlChangeLocation" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<!-- End Change Location -->
		<!-- Part Type -->
		<div style="display: none">
			<asp:Button runat="server" ID="btndummyPartStore" Text="Dummy Part Type" />
		</div>
		<asp:Panel runat="server" ID="pnlChangePartStore" Style="display: none">
			<div>
				<table class="clstablelistout" id="Table5">
					<tr>
						<td align="left" class="style1">
							<asp:UpdatePanel ID="upnlChangePartStore" UpdateMode="Conditional" runat="server">
								<ContentTemplate>
									<table class="clstablelistin" id="Table6">
										<tr>
											<td colspan="2" class="clsFormHeader1Newstyle">
												<table width="100%">
													<tr>
														<td>
															<span id="lblChangePartStatus" class="clsFormHeader">Change Part Status / Store
															</span>
														</td>
														<td align="right">
															<asp:Button ID="btnChangePartOk" runat="server" 
																CssClass="clsbtnH clsinfoH" Text="Ok"
																ToolTip="Click to Add New Part Type">
															</asp:Button>
															<asp:Button ID="btnChangePartClose" TabIndex="0" 
																runat="server" CssClass="clsbtnH clsinfoH"
																Text="Close" ToolTip=" Click to Close" 
																CausesValidation="False">
															</asp:Button>
														</td>
													</tr>
												</table>
											</td>
										</tr>
										<tr>
											<td colspan="2">
												<span id="Label2" class="clsLabelHeader">Change Receipt Item&#39;s Part Type
												</span>
											</td>
										</tr>
										<tr>
											<td>
												<span id="lblCurrentPT" class="clsLabelAuto">Current Part Type (Part Status) </span>
											</td>
											<td align="left">
												<asp:TextBox ID="txtCurrentPT" runat="server" CssClass="clsTextBoxTagSearch" ReadOnly="True"
													MaxLength="50" BackColor="#E0E0E0"></asp:TextBox>
											</td>
										</tr>
										<tr>
											<td>
												<span id="lblChangePT" class="clsLabelAuto">Change Part Type (Part Status)</span>
											</td>
											<td align="left">
												<asp:DropDownList ID="cmbPT" runat="server" ClientIDMode="Static" onChange="SetChagnePartTypeValue()"
													CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID" EnableViewState="false" DataTextField="ItemTypeStatus">
												</asp:DropDownList>
											</td>
										</tr>
										<tr>
											<td>&nbsp;
											</td>
											<td align="left">&nbsp;
											</td>
										</tr>
										<tr>
											<td colspan="2">
												<span id="lblResultChangeStore" class="clsLabelHeader">Change Receipt Item&#39;s Store</span>
											</td>
										</tr>
										<tr>
											<td>
												<span id="lblCurrentStore" class="clsLabelAuto">Current Store</span>
											</td>
											<td align="left">
												<asp:TextBox ID="txtCurrentStore" runat="server" CssClass="clsTextBoxTagSearch" ReadOnly="True"
													MaxLength="50" BackColor="#E0E0E0"></asp:TextBox>
											</td>
										</tr>
										<tr>
											<td>
												<span id="lblChangeStore" class="clsLabelAuto">Change Store</span>
											</td>
											<td align="left">
												<asp:DropDownList ID="cmbChangeStore" ClientIDMode="Static" onChange="SetChangeStoreValue()"
													runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID" EnableViewState="false"
													DataTextField="Name">
												</asp:DropDownList>
											</td>
										</tr>
									</table>
									<asp:HiddenField ID="ChangeStoreValue" runat="server" ClientIDMode="Static" />
									<asp:HiddenField ID="ChangeItemTypeValue" runat="server" ClientIDMode="Static" />
									<asp:HiddenField ID="ChangeStoreName" runat="server" ClientIDMode="Static" />
									<asp:HiddenField ID="ChangeItemTypeName" runat="server" ClientIDMode="Static" />
								</ContentTemplate>
							</asp:UpdatePanel>
						</td>
					</tr>
					<!--Change Store value set-->
					<script type="text/javascript">
						function SetChangeStoreValue() {
							var dd = $get("cmbChangeStore");
							$get('ChangeStoreValue').value = dd.options[dd.selectedIndex].value;
							$get('ChangeStoreName').value = dd.options[dd.selectedIndex].text;
						}
					</script>
					<!--Change Part Type value set-->
					<script type="text/javascript">
						function SetChagnePartTypeValue() {
							var dd = $get("cmbPT");
							$get('ChangeItemTypeValue').value = dd.options[dd.selectedIndex].value;
							$get('ChangeItemTypeName').value = dd.options[dd.selectedIndex].text;
						}
					</script>
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
		<cc2:ModalPopupExtender ID="mdlPopUpChangePartStore" runat="server" TargetControlID="btndummyPartStore"
			PopupControlID="pnlChangePartStore" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<!--End Part Type -->
		<script type="text/javascript">
			function SetStoreValue() {
				var dd = $get("cmbStore");
				$get('StoreValue').value = dd.options[dd.selectedIndex].value;
			}
		</script>
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
