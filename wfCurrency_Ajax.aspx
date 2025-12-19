<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCurrency_Ajax.aspx.vb"
	Inherits="Flypal.wfCurrency_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<title>Currency Information</title>
	<script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
	<link id="MainStyle" type="text/css" rel="stylesheet">
	<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />
</head>

<asp:PlaceHolder runat="server">
	<!-- #include file= "LocalFunctionAjax.htm" -->
</asp:PlaceHolder>

<body bottommargin="5" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="GridLayout">
	<form id="wfgroup" method="post" runat="server">
		<%--AJAX- ScriptManager Added--%>
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
		</asp:ScriptManager>
		<%--AJAX- Add MSGBox Control--%>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table class="clstablelistout" id="tblmain">
			<tr>
				<td align="left" colspan="2">
					<table class="clstablelistin" id="tblInner">
						<tr>
							<td colspan="4" class="clsFormHeader1Newstyle">
								<table width="100%">
									<tr>
										<td>
											<asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
													<asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader displayBlock">
                                                    Currency Information [New]
													</asp:Label>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
										<td align="right">
											<asp:Button ID="btnAdd" runat="server" CssClass="clsbtnH clsinfoH"
												Text="New" ToolTip="Click to add the new Currency"
												CausesValidation="False"></asp:Button>
											<asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH"
												runat="server" Text="Save" ToolTip="Click to save the current record"
												ValidationGroup="grp1"></asp:Button>
											<asp:Button ID="btnClose" runat="server"
												CssClass="clsbtnH clsinfoH" CausesValidation="False"
												ToolTip="Click to close Currency screen" Text="Close"></asp:Button>
										</td>										
									</tr>
								</table>
							</td>
							<%--Added by Harsh on 15th July 2024 for FLYPAL 1745--%>
							<td id="tdFavICN" align="center">
								<span id="spFavICN">
									<i id="favICN" runat="server" onclick="fnMarkFavouriteUnFavourite(this)"
										class="fa fa-star fa-spin fa-5x circle-icon"></i>
								</span>
							</td>
						</tr>
						<tr>
							<td colspan="4">
								<asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<asp:ValidationSummary ID="Validationsummary1" runat="server" HeaderText="Fill Up The Following Information"
											CssClass="clsValidationSummary" ValidationGroup="grp1">
										</asp:ValidationSummary>
										<asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" Display="None"
											ControlToValidate="txtCurrencyName" ErrorMessage=" Currency Name Required." ValidationGroup="grp1">
										</asp:RequiredFieldValidator>
										<asp:CustomValidator ID="cvName" runat="server" ValidationGroup="grp1" CssClass="clsLabelAuto"
											Display="None" ControlToValidate="txtCurrencyName" ErrorMessage="Currency Name should not  be Greater than 50 characters."
											OnServerValidate="customvalidate">
										</asp:CustomValidator>
										<asp:RequiredFieldValidator ID="rfvConvFactor" runat="server" ValidationGroup="grp1"
											CssClass="clsLabel" Display="None" ControlToValidate="txtConvFactor" ErrorMessage="Conversion Factor Required.">
										</asp:RequiredFieldValidator>
										<asp:CustomValidator ID="cvConvFactor" runat="server" ValidationGroup="grp1" CssClass="clslabel1"
											Display="None" ControlToValidate="txtConvFactor" ErrorMessage="Conversion Factor Should be Greater than Zero."
											OnServerValidate="customvalidate">
										</asp:CustomValidator>
										<asp:RequiredFieldValidator ID="rfvSymbol" runat="server" CssClass="clsLabelAuto"
											Display="None" ControlToValidate="txtSymbol" ErrorMessage="Symbol Required."
											ValidationGroup="grp1">
										</asp:RequiredFieldValidator>
									</ContentTemplate>
								</asp:UpdatePanel>
							</td>
						</tr>
						<tr>
							<td>
								<asp:UpdatePanel ID="upnlCurrencyDetails" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<table>
											<tr>
												<td>
													<span id="lblCurrencyNameStar1" class="clsLabelStar">*</span>
												</td>
												<td>
													<span id="lblCurrencyName" class="clsLabelAuto">Name Before Decimal </span>
												</td>
												<td align="left">
													<table id="Table5">
														<tr>
															<td>
																<asp:TextBox ID="txtCurrencyName" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Currency Name"
																	Text="<%# mCurrency.Name %>" MaxLength="50">
																</asp:TextBox>
															</td>
														</tr>
													</table>
												</td>
												<td align="left"></td>
											</tr>
											<tr>
												<td></td>
												<td>
													<span id="lblNameAfterDecimal" class="clsLabelAuto">Name After Decimal </span>
												</td>
												<td align="left">
													<table id="Table3">
														<tr>
															<td>
																<asp:TextBox ID="txtNameAfterDecimal" runat="server" 
																	CssClass="clsTextBoxTagSearch" 
																	Text="<%# mCurrency.NameAfterDecimal %>"
																	MaxLength="50">
																</asp:TextBox>
															</td>
														</tr>
													</table>
												</td>
												<td align="left"></td>
											</tr>
											<tr>
												<td>
													<span id="lblSymbolStar1" class="clsLabelStar">*</span>
												</td>
												<td>
													<span id="lblSymbol" class="clsLabelAuto">Symbol</span>
												</td>
												<td align="left">
													<table id="Table6">
														<tr>
															<td>
																<asp:TextBox ID="txtSymbol" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Symbol"
																	Text="<%# mCurrency.Symbol %>" MaxLength="5">
																</asp:TextBox>
															</td>
														</tr>
													</table>
												</td>
												<td align="left"></td>
											</tr>
											<tr>
												<td>
													<span id="lblConversionFactor1" class="clsLabelStar">*</span>
												</td>
												<td>
													<span id="lblConvFactor" class="clsLabelAuto">Conversion Factor </span>
												</td>
												<td>
													<table id="Table7">
														<tr>
															<td>
																<asp:TextBox ID="txtConvFactor" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																	ToolTip="Enter Conversion Factor" Text="<%# mCurrency.ConversionFactor %>" MaxLength="9"
																	Enabled="<%# mCurrency.ConversionFactor <> 1 %>">
																</asp:TextBox>
															</td>
														</tr>
													</table>
												</td>
												<td></td>
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
													<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">Currency List</asp:Label>
												</td>
												<td align="right"></td>
											</tr>
											<tr>
												<td colspan="2">
													<asp:GridView ID="dgCurrency" runat="server" AllowSorting="True" AutoGenerateColumns="False"
														ShowHeaderWhenEmpty="true" EnableViewState="true" CellPadding="5" AllowPaging="True" PageSize="10"
														CssClass="clsGridNewStyle" GridLines="Horizontal">
														<AlternatingRowStyle CssClass="clsdgAltItem" />
														<RowStyle CssClass="clsdgItem" />
														<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" 
																	 ForeColor="black" HorizontalAlign="Left" />
														<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
														<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
														<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
														<Columns>
															<asp:BoundField Visible="False" DataField="ID" HeaderText="CurrencyID"></asp:BoundField>
															<asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Name">
																<HeaderStyle ></HeaderStyle>
															</asp:BoundField>
															<asp:BoundField DataField="NameAfterDecimal" SortExpression="NameAfterDecimal" HeaderText="Name After Decimal ">
																<HeaderStyle ></HeaderStyle>
															</asp:BoundField>
															<asp:BoundField DataField="Symbol" SortExpression="Symbol" HeaderText="Symbol ">
																<HeaderStyle ></HeaderStyle>
															</asp:BoundField>
															<asp:BoundField DataField="ConversionFactor" SortExpression="ConversionFactor" HeaderText="Conv.Factor">
																<HeaderStyle HorizontalAlign="Right" ></HeaderStyle>
																<ItemStyle HorizontalAlign="Right"></ItemStyle>
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
																							ToolTip="Click to Edit record" CausesValidation="false"
																							CommandName="EditRec" ImageUrl="~/images/edit.png" />
																					</td>
																					<td>
																						<asp:ImageButton ID="deleteICN" class="actionICNS  largerActionICNS" runat="server"
																							CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																							ToolTip="Click to Delete record" CausesValidation="false"
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
												<%--Added by Harsh on 15th July 2024 for FLYPAL 1745--%>
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
				</td>
			</tr>
		</table>

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

		<%--Added by Harsh on 15th July 2024 for FLYPAL 1745--%>
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
