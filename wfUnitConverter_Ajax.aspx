<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfUnitConverter_Ajax.aspx.vb"
	Inherits="Flypal.wfUnitConverter_Ajax" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagName="MSGBox" TagPrefix="uc2" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<title>Unit Converter</title>	
	<link id="MainStyle" type="text/css" rel="stylesheet">
	<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

	<script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
	<script language="javascript">
		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
		}
	</script>

	<style type="text/css">

		.displayBlock {
			width: 200px !important;
		}

	</style>

</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="0">
	<form id="wfgroup" method="post" runat="server">
		<%-- AJAX ScriptManager --%>
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
		</asp:ScriptManager>
		<%-- AJAX Update Panel FOr Message Box --%>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox runat="server" ID="MSGBoxCntrl" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table id="tblmain" class="clstablelistout">
			<tr>
				<td>
					<asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
						<table id="tblInner" class="clstablelistin">
							<tr>
								<td colspan="4" class="clsFormHeader1Newstyle">
									<table width="100%">
										<tr>
											<td>
												<asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader displayBlock">Unit Converter</asp:Label>
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
											<td align="right">
												<asp:Button ID="btnAdd" runat="server" CssClass="clsbtnH clsinfoH" CausesValidation="False"
													Text="New" ToolTip="Click to add new Unit Converter"></asp:Button>
												<asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Save"
													ToolTip="Click to save the Unit Converter Information"></asp:Button>
												<asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" CausesValidation="False"
													Text="Close" ToolTip="Click to close Unit Converter screen"></asp:Button>
											</td>
										</tr>
									</table>
								</td>
								<%--Added by Harsh on 15th July 2024 for FLYPAL 1757--%>
								<td id="tdFavICN" align="center">
									<span id="spFavICN">
										<i id="favICN" runat="server" onclick="fnMarkFavouriteUnFavourite(this)"
											class="fa fa-star fa-spin fa-5x circle-icon"></i>
									</span>
								</td>
							</tr>
							</tr>
							<tr>
								<td colspan="2">
									<asp:CustomValidator EnableClientScript="true" ID="cvBaseUnit" runat="server" CssClass="clsLabelAuto"
										ClientValidationFunction="ValidationFunction" ControlToValidate="cmbBaseUnit"
										Display="None" ErrorMessage="Select Base Unit"></asp:CustomValidator>
									<asp:CustomValidator EnableClientScript="true" ID="cvConvertUnit" Display="None"
										runat="server" CssClass="clsLabelAuto" ControlToValidate="cmbConvertUnit" ClientValidationFunction="ValidationFunction"
										ErrorMessage="Select Convert Unit"></asp:CustomValidator>
									<asp:CustomValidator EnableClientScript="true" ID="cvFactor" runat="server" CssClass="clsLabelAuto"
										ClientValidationFunction="ValidationFunction" ControlToValidate="txtFactor" Display="None"
										ErrorMessage="Factor should be Numeric and Greater than Zero" ValidateEmptyText="true"></asp:CustomValidator>
									<asp:ValidationSummary EnableClientScript="true" ID="ValidationSummary1" runat="server"
										CssClass="clsValidationSummary"></asp:ValidationSummary>
									<script type="text/javascript">
										function ValidationFunction(source, args) {
											var control = source.controltovalidate;
											if (control == "cmbBaseUnit" || control == "cmbConvertUnit") {
												args.IsValid = false;
												var dd = $get(control);
												if (dd.selectedIndex != 0) {
													args.IsValid = true;
													return;
												}
											}
											else if (control == "txtFactor") {
												args.IsValid = false;
												var val = $.trim($get(control).value);
												if (isNumeric(val) && (Number(val) > 0)) {
													args.IsValid = true;
													return;
												}

											}
										}
									</script>
								</td>
							</tr>
							<tr>
								<td colspan="2">
									<asp:UpdatePanel ID="upnlUnitDetails" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table width="100%">
												<tr>
													<td colspan="4">
														<span id="lblUnitDetails" class="clsLabelHeader">Unit Details</span>
													</td>
												</tr>
												<tr>
													<td>
														<span id="lblBaseUnit" class="clsLabelAuto">Base Unit</span>
													</td>
													<td>
														<asp:DropDownList ID="cmbBaseUnit" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="Name"
															DataValueField="ID" SelectedValue="<%# mUnitConverter.PrimaryUnitID %>" Width="128px"
															EnableViewState="false" onChange="SetBaseUnitValues()" ClientIDMode="Static">
														</asp:DropDownList>
														<asp:HiddenField ID="BaseUnitIDValue" runat="server" ClientIDMode="Static" />
														<script type="text/javascript">
															function SetBaseUnitValues() {
																var dd = $get("cmbBaseUnit");
																$get('BaseUnitIDValue').value = dd.options[dd.selectedIndex].value;

															}
														</script>
													</td>
													<td colspan="2"></td>
												</tr>
												<tr>
													<td valign="top">
														<span id="lblConvertUnit" class="clsLabelAuto">Convert Unit</span>
													</td>
													<td>
														<asp:DropDownList ID="cmbConvertUnit" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
															DataTextField="Name" DataValueField="ID" onChange="SetConvertUnitValues()" ClientIDMode="Static"
															SelectedValue="<%# mUnitConverter.ConvertUnitID %>" Width="128px" EnableViewState="false">
														</asp:DropDownList>
														<asp:HiddenField ID="ConvertUnitIDValue" runat="server" ClientIDMode="Static" />
														<script type="text/javascript">
															function SetConvertUnitValues() {
																var dd = $get("cmbConvertUnit");
																$get('ConvertUnitIDValue').value = dd.options[dd.selectedIndex].value;

															}
														</script>
													</td>
													<td colspan="2" align="right"></td>
												</tr>
												<tr>
													<td valign="top">
														<span id="lblFactor" class="clsLabelAuto">Factor</span>
													</td>
													<td valign="top">
														<asp:TextBox ID="txtFactor" runat="server" Width="70px"
															CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" MaxLength="9"
															Text="<%# mUnitConverter.Factor %>" ToolTip="Enter Factor">
														</asp:TextBox>
													</td>
													<td colspan="2"></td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td colspan="4">
									<asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table>
												<tr>
													<td align="right"></td>
												</tr>
												<tr>
													<td>
														<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
													</td>
												</tr>
												<tr>
													<td>
														<asp:GridView ID="dgUnitConverterList" runat="server" AutoGenerateColumns="False"
															AllowSorting="True" ShowHeaderWhenEmpty="true"
															EnableViewState="true" CssClass="clsGridNewStyle" GridLines="Horizontal"
															CellPadding="5" AllowPaging="True" PageSize="10">
															<AlternatingRowStyle CssClass="clsdgAltItem" />
															<RowStyle CssClass="clsdgItem" />
															<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True"
																ForeColor="black" HorizontalAlign="Left" />
															<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
															<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
															<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
															<Columns>
																<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
																<asp:BoundField Visible="False" DataField="PrimaryUnitID" HeaderText="PrimaryUnitID"></asp:BoundField>
																<asp:BoundField Visible="False" DataField="ConvertUnitID" HeaderText="ConvertUnitID"></asp:BoundField>
																<asp:BoundField DataField="PrimaryUnitName" SortExpression="PrimaryUnitName" HeaderText="Base Unit Name">
																	<HeaderStyle Width="150px"></HeaderStyle>
																	<ItemStyle Width="150px"></ItemStyle>
																</asp:BoundField>
																<asp:BoundField DataField="ConvertUnitName" SortExpression="ATANomenclature" HeaderText="Convert Unit Name">
																	<HeaderStyle Width="150px"></HeaderStyle>
																	<ItemStyle Width="150px"></ItemStyle>
																</asp:BoundField>
																<asp:BoundField DataField="Factor" SortExpression="Factor" HeaderText="Factor">
																	<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
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
																								ToolTip="Click to Delete record"
																								CommandName="DeleteRec" ImageUrl="~/images/delete.png" CausesValidation="false" />
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


		<%-- hide validation summary when server event occurs--%>
		<script type="text/javascript">
			Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(function () {
				if ((typeof (Page_ClientValidate) == 'function')) {
					if (Page_ValidationActive) {
						if (!ValidatorCommonOnSubmit()) {
							return false;
						}
						else {
							$("#ValidationSummary1").css('display', 'none');
						}
					}
				}
			});
		</script>

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
