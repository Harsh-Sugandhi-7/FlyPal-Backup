<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfWorkShop_Ajax.aspx.vb" Inherits="Flypal.wfWorkShop_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<title>Work Shop</title>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />

	<!-- #include file= "LocalFunctionAjax.htm" -->

	<script language="javascript">
		function OpenLocation(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
		}
	</script>

	<style type="text/css">
		.hideGridColumn {
			display: none;
		}
	</style>

</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="5" ms_positioning="GridLayout">
	<form id="form1" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1" EnablePageMethods="true">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<div>
			<table id="tblmain" class="clstablelistout">
				<tr>
					<td>
						<asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
							<table id="tblInner" class="clstablelistin">
								<tr>
									<td>
										<asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
											<ContentTemplate>
												<table width="100%">
													<tr>
														<td colspan="4" class="clsFormHeader1Newstyle">
															<table width="100%">
																<tr>
																	<td>
																		<asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader displayBlock">
                                                                        Work Shop [New]
																		</asp:Label>
																	</td>
																	<td align="right">
																		<asp:Button ID="btnAdd" runat="server" CssClass="clsbtnH clsinfoH"
																			Text="New" ToolTip="Click to add the new Work Shop"
																			CausesValidation="False"></asp:Button>
																		<asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH"
																			Text="Save" ToolTip="Click to save Work Shop Information"></asp:Button>
																		<asp:Button ID="btnBackBottom" TabIndex="0" runat="server"
																			CssClass="clsbtnH clsinfoH" Text="Close"
																			ToolTip="Click to close Work Shop screen" CausesValidation="False"></asp:Button>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td>
															<asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
																ValidationGroup="a" />
															<asp:CustomValidator ID="cvLocation" runat="server" ControlToValidate="cmbLocation"
																CssClass="clsLabelAuto" Display="None" ErrorMessage="Location Required" OnServerValidate="customvalidate"
																ValidationGroup="a"></asp:CustomValidator>
															<asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" ErrorMessage="Name Required."
																Display="None" ControlToValidate="txtName" ValidationGroup="a"></asp:RequiredFieldValidator>
														</td>
													</tr>
												</table>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
									<%--Added by Harsh on 15th July 2024 for FLYPAL 1757--%>
									<td id="tdFavICN" align="center">
										<span id="spFavICN">
											<i id="favICN" runat="server" onclick="fnMarkFavouriteUnFavourite(this)"
												class="fa fa-star fa-spin fa-5x circle-icon"></i>
										</span>
									</td>
								</tr>
								<tr>
									<td colspan="2">
										<asp:UpdatePanel runat="server" ID="upnlDetails" UpdateMode="Conditional">
											<ContentTemplate>
												<table width="100%">
													<tr>
														<td>
															<span id="spnName1" class="clsLabelStar">*</span>
														</td>
														<td>
															<span id="spnName" class="clsLabelAuto">Name</span>
														</td>
														<td>
															<table>
																<tr>
																	<td>
																		<asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mWorkShop.Name %>"
				ToolTip="Enter WorkShop Name" MaxLength="50"></asp:TextBox>
																	</td>
																</tr>
															</table>
															
														</td>
														<td></td>
													</tr>
													<tr>
														<td>
															<span id="spnLocationStar" class="clsLabelStar">*</span>
														</td>
														<td>
															<span id="spnLocation" class="clsLabelAuto">Location</span>
														</td>
														<td>
															<table>
																<tr>
																	<td>
																		<asp:DropDownList ID="cmbLocation" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
																			SelectedValue="<%# mWorkShop.LocationID %>"
																			DataValueField="ID" DataTextField="Name">
																		</asp:DropDownList>
																	</td>
																	<td>
																		<asp:ImageButton ID="addLocation" runat="server" ImageUrl="~/images/plus1.png"
																			Height="22px" Width="24px" ToolTip="Click to Add New Location."
																			CausesValidation="False"></asp:ImageButton>
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
									<td colspan="2">
										<asp:UpdatePanel runat="server" ID="upnlGridView" UpdateMode="Conditional">
											<ContentTemplate>
												<table width="100%">
													<tr>
														<td>
															<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">Work Shop List</asp:Label>
														</td>
													</tr>
													<tr>
														<td>
															<asp:GridView ID="dgWorkShopList" runat="server" ClientIDMode="Static"
																CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
																AllowPaging="True" PageSize="10" AutoGenerateColumns="False"
																EnableViewState="true" AllowSorting="True">
																<AlternatingRowStyle CssClass="clsdgAltItem" />
																<RowStyle CssClass="clsdgItem" />
																<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True"
																	ForeColor="black" HorizontalAlign="Left" />
																<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
																<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
																<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
																<Columns>
																	<asp:BoundField DataField="Id" HeaderText="Id" Visible="False" />
																	<asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name">
																		<HeaderStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<asp:BoundField DataField="LocationName" HeaderText="Location" SortExpression="LocationName">
																		<HeaderStyle HorizontalAlign="Left" />
																	</asp:BoundField>
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
																								<asp:ImageButton ID="editICN" class="actionICNS" runat="server"
																									CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																									ToolTip="Click to Edit record" CausesValidation="False"
																									CommandName="EditView" ImageUrl="~/images/edit.png" />
																							</td>
																							<td>
																								<asp:ImageButton ID="deleteICN" class="actionICNS  largerActionICNS" runat="server"
																									CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																									ToolTip="Click to Delete record" CausesValidation="False"
																									CommandName="Remove" ImageUrl="~/images/delete.png" />
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
														<%--Added by Harsh on 15th July 2024 for FLYPAL 1757--%>
														<td>
															<asp:Button ID="hdnBtnMarkFavourite" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
																Style="display: none;"></asp:Button>
															<asp:Button ID="hdnBtnRemoveFavourite" ClientIDMode="Static" runat="server" Text="----"
																CausesValidation="False" Style="display: none;"></asp:Button>
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
		</div>

		<%--Added by Harsh on 15th July 2024 for FLYPAL 1757--%>
		<script type="text/javascript">
			function fnMarkFavouriteUnFavourite(x) {
				if (x.classList.contains("fa-star")) {
					x.classList.remove("fa-star");
					x.classList.add("fa-star-o");
					x.style.color = 'black';
					x.style.border = 'black';
					$("#hdnBtnRemoveFavourite").click();
				}
				else {
					x.classList.remove("fa-star-o");
					x.classList.add("fa-star");
					x.style.color = '#fff';
					x.style.border = 'black';
					$("#hdnBtnMarkFavourite").click();
				}
			}
			function MarkAsFavourite() {
				var redstar = document.getElementById("<%=favICN.ClientID%>");
				redstar.classList.add("fa-star");
				redstar.classList.remove("fa-star-o");
				redstar.style.color = '#fff';
				redstar.style.border = 'black';

			}
			function RemoveFromFavourite() {
				var redstar = document.getElementById("<%=favICN.ClientID%>");
				redstar.classList.add("fa-star-o");
				redstar.classList.remove("fa-star");
				redstar.style.border = 'black';
			}
		</script>
	</form>
</body>
</html>
