<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfCharge_Ajax.aspx.vb"
	Inherits="Flypal.wfCharge_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagName="MSGBox" TagPrefix="uc2" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<title>Charge Information</title>
	<script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
	<link id="MainStyle" type="text/css" rel="stylesheet">
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
</head>
<body>
	<form id="wfgroup" method="post" runat="server">
		<%-- AJAX ScriptManager --%>
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
		</asp:ScriptManager>
		<%-- AJAX Update Panel FOr Message Box --%>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox runat="server" ID="MSGBoxCtrl" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table id="tblmain" class="clstablelistout">
			<tr>
				<td>
					<asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
						<table id="tblInner" class="clstablelistin">
							<tr>
								<td colspan="2" class="clsFormHeader1Newstyle">
									<table width="100%">
										<tr>
											<td>
												<asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">Charge Information</asp:Label>
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
											<td align="right">
												<asp:Button ID="btnAdd" class="clsbtnH clsinfoH"
													runat="server" Text="New"
													ToolTip="Click to Add New Charge "
													CausesValidation="False"></asp:Button>

												<asp:Button ID="btnSave" runat="server"
													class="clsbtnH clsinfoH" Text="Save"
													ToolTip="Click to Save Charge"></asp:Button>

												<asp:Button ID="btnClose" runat="server" class="clsbtnH clsinfoH" 
													CausesValidation="False" Text="Close" 
													ToolTip="Click to close Charge Information screen">
												</asp:Button>

											</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td colspan="2">
									<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlValidations">
										<ContentTemplate>
											<asp:ValidationSummary ID="Validationsummary1" runat="server" HeaderText="Fill Up The Following Information"
												CssClass="clsValidationSummary"></asp:ValidationSummary>
											<asp:RequiredFieldValidator ID="rfvChargeName" runat="server" CssClass="clsLabelAuto"
												ErrorMessage="Name Required." ControlToValidate="txtChargeName" Display="None"></asp:RequiredFieldValidator>
											<asp:CustomValidator ID="cvName" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtChargeName"
												Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
											<asp:CustomValidator ID="cvPercentage" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtPercentage"
												Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
											<asp:RequiredFieldValidator ID="rfvPercentage" runat="server" CssClass="clsLabelAuto"
												ErrorMessage="Percentage Required." ControlToValidate="txtPercentage" Display="None"></asp:RequiredFieldValidator>
											<asp:CustomValidator ID="cvChargeType" runat="server" CssClass="clsLabelAuto" Display="None"
												ControlToValidate="cmbChargeType" OnServerValidate="customvalidate"></asp:CustomValidator>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td colspan="2">
									<asp:UpdatePanel ID="upnlChargeDetails" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table width="100%">
												<tr>
													<td colspan="3">
														<asp:UpdatePanel runat="server" ID="upnlCharge" UpdateMode="Conditional">
															<ContentTemplate>
																<fieldset id="fdsPartDet" class="clsFieldSetNewStyle" 
																	runat="server" style="border-width: 1px; position: relative">
																	<legend id="ledChargeDet"><b>Charge Details</b>
																	</legend>

																	<table width="100%">
																		<tr>
																			<td>
																				<span id="lblChargeNameStar" class="clsLabelStar">*</span>
																			</td>
																			<td>
																				<span id="lblChargeName" class="clsLabelAuto">Charge Name</span>
																			</td>
																			<td>
																				<asp:TextBox ID="txtChargeName" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mCharge.Name %>"
																					ToolTip="Enter Charge Name" MaxLength="50">
																				</asp:TextBox>
																			</td>
																		</tr>
																		<tr>
																			<td></td>
																			<td valign="top">
																				<span id="lblGLCode" class="clsLabelAuto">GL Code</span>
																			</td>
																			<td>
																				<asp:TextBox ID="txtGLCode" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter GL Code"
																					Text="<%# mCharge.GLCode %>" MaxLength="4">
																				</asp:TextBox>
																			</td>
																		</tr>
																		<tr>
																			<td valign="top">
																				<span id="lblStarPercentType" class="clsLabelStar">*</span>
																			</td>
																			<td valign="top">
																				<span id="lblPercentType" class="clsLabelAuto">Percent Type</span>
																			</td>
																			<td valign="top">
																				<asp:DropDownList ID="cmbPercentType" runat="server" CssClass="clsTextBoxTagSearchComboSmall1"
																					AutoPostBack="True" DataValueField="ID" DataTextField="PercentName">
																				</asp:DropDownList>
																			</td>
																		</tr>
																		<tr>
																			<td valign="top">
																				<span id="lblStarChargeType" class="clsLabelStar">*</span>
																			</td>
																			<td valign="top">
																				<span id="lblChargeType" class="clsLabelAuto">Charge Type </span>
																			</td>
																			<td valign="top">
																				<asp:DropDownList ID="cmbChargeType" runat="server" CssClass="clsTextBoxTagSearchComboSmall"
																					DataTextField="ChargeName" DataValueField="ID">
																				</asp:DropDownList>
																			</td>
																		</tr>
																		<tr>
																			<td>&nbsp;
																			</td>
																			<td valign="top">
																				<span id="lblSign" class="clsLabelAuto">Sign </span>
																			</td>
																			<td valign="top">
																				<asp:DropDownList ID="cmbSign" runat="server" CssClass=" clsTextBoxTagSearchComboSmall1">
																					<asp:ListItem Value="0">+</asp:ListItem>
																					<asp:ListItem Value="1">-</asp:ListItem>
																				</asp:DropDownList>
																			</td>
																		</tr>
																		<tr>
																			<td valign="top">
																				<span id="lblStarPercent" class="clsLabelStar">*</span>
																			</td>
																			<td valign="top">
																				<span id="lblPercentage" class="clsLabelAuto">Percentage </span>
																			</td>
																			<td valign="top">
																				<asp:TextBox ID="txtPercentage" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mCharge.Percentage %>"
																					ToolTip="Enter Percentage" MaxLength="8" ReadOnly="True">
																				</asp:TextBox>
																			</td>
																		</tr>
																	</table>
																</fieldset>
															</ContentTemplate>
														</asp:UpdatePanel>
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>

							<tr>
								<td colspan="2">
									<asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table>
												<tr>
													<td align="right"></td>
												</tr>
												<tr>
													<td>
														<asp:Label ID="lblSearch" runat="server" CssClass="clsLabelHeader">Charge List</asp:Label>
													</td>
												</tr>
												<tr>
													<td>
														<asp:GridView ID="dgCharge" runat="server" CssClass="clsGridNewStyle" AllowPaging="True"
															PageSize="5" AutoGenerateColumns="False" AllowSorting="True" ShowHeaderWhenEmpty="true"
															EnableViewState="false" CellPadding="5" ForeColor="Black" GridLines="Horizontal">
															<AlternatingRowStyle CssClass="clsdgAltItem" />
															<RowStyle CssClass="clsdgItem" />
															<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
															<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
															<PagerSettings FirstPageText="First" LastPageText="Last" />
															<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
															<Columns>
																<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
																<asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Name">
																	<HeaderStyle></HeaderStyle>
																</asp:BoundField>
																<asp:BoundField DataField="GLCode" SortExpression="GLCode" HeaderText="GL Code">
																	<HeaderStyle></HeaderStyle>
																</asp:BoundField>
																<asp:BoundField DataField="PercentageType" SortExpression="PercentageType" HeaderText="Percent Type">
																	<HeaderStyle></HeaderStyle>
																</asp:BoundField>
																<asp:BoundField DataField="ChargeType" SortExpression="ChargeType" HeaderText="Charge Type">
																	<HeaderStyle></HeaderStyle>
																</asp:BoundField>
																<asp:BoundField DataField="SignName" SortExpression="SignName" HeaderText="Sign ">
																	<HeaderStyle></HeaderStyle>
																</asp:BoundField>
																<asp:BoundField DataField="Percentage" SortExpression="Percentage" HeaderText="Percentage">
																	<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																	<ItemStyle HorizontalAlign="Right"></ItemStyle>
																</asp:BoundField>
																<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
																	<ItemTemplate>
																		<div class="dropdown">
																			<div class="dropdownbtn-content">
																				<table id="T1" class="clsGridNew_Ajax">
																					<tr>
																						<td>
																							<asp:ImageButton ID="ImgEditView" runat="server" CommandName="EditRec" Style="height: 15px; width: 15px"
																								CausesValidation="false" ImageUrl="~/images/edit.png" CommandArgument='<%# Eval("ID") %>' />
																						</td>
																						<td>
																							<asp:ImageButton ID="ImgDeleteRecord" runat="server" CommandName="DeleteRec" Style="height: 20px; width: 20px"
																								CausesValidation="false" ImageUrl="~/images/delete.png" CommandArgument='<%# Eval("ID") %>' />
																						</td>
																					</tr>
																				</table>
																			</div>
																			<asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
																				Style="cursor: pointer" />
																		</div>
																	</ItemTemplate>
																	<HeaderStyle HorizontalAlign="Center" />
																	<ItemStyle HorizontalAlign="Center" />
																</asp:TemplateField>
															</Columns>
															<SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
															<SortedAscendingCellStyle BackColor="#F7F7F7" />
															<SortedAscendingHeaderStyle BackColor="#4B4B4B" />
															<SortedDescendingCellStyle BackColor="#E5E5E5" />
															<SortedDescendingHeaderStyle BackColor="#242121" />
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

		<%--call parent function after completing subroutine..(when page open as popup)--%>
		<script type="text/javascript">
			function CallParentCallback() {
				parent.ParentCallBackFunctionForChargeList();
				return false;
			}
		</script>
		<%--End--%>
		<%--Set page layout when open as popup aspx page--%>
		<script type="text/javascript">

			<% Dim mopen As String = Request.QueryString("Typepup") %>

			<% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

					$(document).ready(function () {
						SetPageLayout();
						if ($.browser.msie) {
							parent.IFrameChargeListStateComplete();
						}

					});

			<% End if %>

			Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
			function endRequestHandler() {
				SetPageLayout();
			}

			function SetPageLayout() {
				<% Dim mopenas As String = Request.QueryString("Typepup") %>
				<% If Not mopenas Is Nothing AndAlso mopenas = "pup" Then %>  
					ReSetPageLayout();
					onResize();//for Top bottom link
				<% End if %>
			}
			function ReSetPageLayout() {
				$("body,html").css({ 'background-color': 'transparent' });
				var tempMargtop = $("body #tblmain:eq(0)").outerHeight();
				var windowheight = $(window).height();
				if (tempMargtop >= windowheight) {
					$("body #tblmain:eq(0)").css({ 'margin': 'auto' });
				}
				else {
					var margintop = (windowheight / 2) - (tempMargtop / 2);
					$("body #tblmain:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
				}

			}
		</script>
		<%--End--%>
	</form>
</body>
</html>
