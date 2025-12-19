<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfVendorList_Ajax.aspx.vb"
	Inherits="Flypal.wfVendorList_Ajax" EnableEventValidation="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<title>Vendor List</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<script language="javascript">
		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
		}

		//this function takes a value (ltext) and transmits that to the left hand frame

		function tranRight(ltext) {
			parent.frames(1).document.forms("wfReceive").item("txtReceive").value = ltext;

		}
	</script>

	<!-- #include file= "LocalFunctionAjax.htm" -->
</head>
<body bottommargin="5" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="GridLayout">
	<form id="Form1" method="post" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
			runat="server">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table class="clstablelistout" id="tblmain">
			<tr>
				<td>
					<asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
						<table id="tblLedgerList" class="clstablelistin">
							<tr>
								<td>
									<table width="100%">
										<tr>
											<td class="clsFormHeader1Newstyle">
												<table width="100%">
													<tr>
														<td>
															<span id="lblLedgerList" class="clsFormHeader">Vendor List</span>
														</td>
														<td align="right">
															<asp:UpdatePanel ID="upnlAddTop" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<table>
																		<tr>
																			<td>
																				<asp:Button ID="btnAddTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Add New Vendor "
																					Text="Add New"></asp:Button>
																			</td>
																			<td align="right">
																				<asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to close Vendor List screen"
																					Text="Close"></asp:Button>
																			</td>
																		</tr>
																	</table>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>

													</tr>
												</table>
											</td>
											<td style="width: 1%" align="center">
												<span id="FavClk"><i id="FavIClk" runat="server" onclick="FunctionFav(this)"
													style="font-size: 17px; color: black; border: black; cursor: pointer"
													class="fa fa-star fa-spin fa-5x circle-icon"
													title="Mark As Favourites"></i></span>
											</td>

										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table width="100%">
												<tr>
													<td>
														<table>
															<tr>
																<td>
																	<asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBoxTagSearch"
																		ToolTip="Enter Search Criteria" AutoPostBack="true" placeholder="Search here" autocomplete="off"></asp:TextBox>
																</td>
																<td>
																	<asp:DropDownList ID="cmbLookIn" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="True" Visible="false">
																		<asp:ListItem Value="0">All</asp:ListItem>
																		<asp:ListItem Value="1">Vendor</asp:ListItem>
																		<asp:ListItem Value="2">City</asp:ListItem>
																		<asp:ListItem Value="3">State</asp:ListItem>
																		<asp:ListItem Value="4">Country</asp:ListItem>
																		<asp:ListItem Value="5">Contact Person</asp:ListItem>
																	</asp:DropDownList>
																</td>
																<td>
																	<asp:Label ID="lblFor" runat="server" CssClass="clsLabelMedium" Visible="False">For</asp:Label>
																</td>

															</tr>
														</table>
													</td>
													<td align="right">
														<table>
															<tr>
																<td align="right">
																	<asp:Button ID="btnFindNow" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to find Vendor List as per searching criteria"
																		Text="Find Now" Visible="false"></asp:Button>
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
									<asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table width="100%">
												<tr>
													<td>
														<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">As per criteria: Record(s) found.</asp:Label>
													</td>
													<td align="right">
														<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
																<asp:Label ID="Label2" runat="server" Text="Show Entries"></asp:Label>
																<asp:DropDownList CssClass="clsTextBoxTagSearchComboSmall" ID="cmbShowE" runat="server" Width="55px"
																	AutoPostBack="true" OnSelectedIndexChanged="OnSelectedIndexChanged">
																	<asp:ListItem Value="0">5</asp:ListItem>
																	<asp:ListItem Value="1">10</asp:ListItem>
																	<asp:ListItem Value="2">15</asp:ListItem>
																	<asp:ListItem Value="3">20</asp:ListItem>
																	<asp:ListItem Value="4" Selected="True">25</asp:ListItem>
																	<asp:ListItem Value="5">30</asp:ListItem>
																	<asp:ListItem Value="6">40</asp:ListItem>
																	<asp:ListItem Value="7">45</asp:ListItem>
																	<asp:ListItem Value="8">50</asp:ListItem>
																	<asp:ListItem Value="9">55</asp:ListItem>
																</asp:DropDownList>
															</ContentTemplate>
														</asp:UpdatePanel>
													</td>
												</tr>
												<tr>
													<td colspan="2">
														<asp:GridView ID="dgVendor1" runat="server" AutoGenerateColumns="False"
															DataKeyNames="ID" ShowHeaderWhenEmpty="true" AllowPaging="True"
															PageSize="25" AllowSorting="True"
															CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
															<AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
															<RowStyle CssClass="clsdgItem"></RowStyle>
															<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
															<PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
															<PagerStyle HorizontalAlign="Right" CssClass="paging" />
															<Columns>
																<%--0--%>
																<asp:BoundField Visible="False" DataField="ID" HeaderText="ID" />
																<%--1--%>
																<asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Vendor">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																</asp:BoundField>
																<%--2--%>
																<asp:BoundField DataField="Code" SortExpression="Code" HeaderText="Code">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																</asp:BoundField>
																<%--3--%>
																<asp:BoundField DataField="GSTIN" SortExpression="GSTIN" HeaderText="GSTIN">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																</asp:BoundField>
																<%--4--%>
																<asp:BoundField DataField="VendorsID" SortExpression="VendorsID" HeaderText="ID">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="false" />
																</asp:BoundField>
																<%--5--%>
																<asp:BoundField DataField="Address" SortExpression="Address" HeaderText="Address">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="True"></ItemStyle>
																</asp:BoundField>
																<%--6--%>
																<asp:BoundField DataField="CityName" SortExpression="CityName" HeaderText="City">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="False"></ItemStyle>
																</asp:BoundField>
																<%--7--%>
																<asp:BoundField DataField="Zip" SortExpression="Zip" HeaderText="Zip Code">
																	<HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
																	<ItemStyle Wrap="False"></ItemStyle>
																</asp:BoundField>
																<%--8--%>
																<asp:BoundField DataField="StateName" SortExpression="StateName" HeaderText="State">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="False"></ItemStyle>
																</asp:BoundField>
																<%--9--%>
																<asp:BoundField DataField="CountryName" SortExpression="CountryName" HeaderText="Country">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="False"></ItemStyle>
																</asp:BoundField>
																<%--10--%>
																<asp:BoundField DataField="ContactPerson" SortExpression="ContactPerson" HeaderText="Contact Person">
																	<HeaderStyle HorizontalAlign="Left" Wrap="False"></HeaderStyle>
																	<ItemStyle Wrap="False"></ItemStyle>
																</asp:BoundField>
																<%--11--%>
																<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
																	<HeaderStyle HorizontalAlign="Center" />
																	<ItemStyle HorizontalAlign="Center" />
																	<ItemTemplate>
																		<div id="dropDownImg" class="dropdown">
																			<asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn" />
																			<div id="dropdownICN-content" class="dropdownbtn-content">
																				<table id="dropdown-content" class="clsGridNew_Ajax">
																					<tr>
																						<td>
																							<asp:ImageButton ID="editICN" CssClass="actionICNS" runat="server"
																								CommandArgument='<%# Eval("ID") %>'
																								CommandName="EditRec"
																								ToolTip="Click to Edit record"
																								ImageUrl="~/images/edit.png" />
																						</td>
																						<td>
																							<asp:ImageButton ID="deleteICN" class="largerActionICNS" runat="server"
																								CommandArgument='<%# Eval("ID") %>'
																								ToolTip="Click to Delete record"
																								CommandName="DeleteRec" ImageUrl="~/images/delete.png" />
																						</td>
																					</tr>
																				</table>
																			</div>
																		</div>
																	</ItemTemplate>
																</asp:TemplateField>
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
								<td colspan="2" align="right">
									<asp:UpdatePanel ID="upnlFavIcnBtn" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table>
												<tr>
													<td>
														<asp:Button ID="hdnBtnMarkFav" ClientIDMode="Static" runat="server"
															Text="----" CausesValidation="False" Style="display: none;"></asp:Button>
														<asp:Button ID="hdnBtnRemoveFav" ClientIDMode="Static" runat="server"
															Text="----" CausesValidation="False" Style="display: none;"></asp:Button>
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
		<asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" ClientIDMode="Static"
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
			function FunctionFav(x) {
				if (x.classList.contains("fa-star")) {
					x.classList.remove("fa-star");
					x.classList.add("fa-star-o");
					x.style.color = 'black';
					x.style.border = 'black';
					$("#hdnBtnRemoveFav").click();
				}
				else {
					x.classList.remove("fa-star-o");
					x.classList.add("fa-star");
					x.style.color = '#fff';
					x.style.border = 'black';
					$("#hdnBtnMarkFav").click();
				}
			}
			function MarkFav() {
				var redstar = document.getElementById("<%=FavIClk.ClientID%>");
				redstar.classList.add("fa-star");
				redstar.classList.remove("fa-star-o");
				redstar.style.color = '#fff';
				redstar.style.border = 'black';

			}
			function RemoveFav() {
				var redstar = document.getElementById("<%=FavIClk.ClientID%>");
				redstar.classList.add("fa-star-o");
				redstar.classList.remove("fa-star");
				redstar.style.border = 'black';
			}
		</script>
	</form>
</body>
</html>
