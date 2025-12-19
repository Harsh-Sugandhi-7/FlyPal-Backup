<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfTerm_Ajax.aspx.vb" Inherits="Flypal.wfTerm_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<title>Term</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<link id="MainStyle" type="text/css" rel="stylesheet">
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
</head>
<body ms_positioning="GridLayout" bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5">
	<form id="Form1" method="post" runat="server">
		<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table class="clstablelistout" id="tblmain">
			<tr>
				<td>
					<asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
						<table class="clstablelistin" id="tblInner">
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlTermMasterDetails" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table width="100%">
												<tr>
													<td class="clsFormHeader1Newstyle">
														<table width="100%">
															<tr>
																<td align="Left">
																	<asp:Label ID="lblTitle" CssClass="clsFormHeader" runat="server">Term [New]</asp:Label>
																</td>
																<td align="Right">
																	<table>
																		<tr>
																			<td>
																				<asp:Button ID="btnAdd" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to add new Term"
																					Text="New" CausesValidation="False"></asp:Button>
																			</td>
																			<td>
																				<asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH" runat="server" ToolTip="Click to Save the Term Information"
																					Text="Save"></asp:Button>
																			</td>
																			<td>
																				<asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
																					Text="Close" CausesValidation="False"></asp:Button>
																			</td>
																		</tr>
																	</table>
																</td>
															</tr>
														</table>
													</td>
												</tr>
												<tr>
													<td>
														<asp:ValidationSummary ID="Validationsummary1" runat="server" CssClass="clsValidationSummary"></asp:ValidationSummary>
														<asp:CustomValidator ID="cvName" runat="server" ErrorMessage="Term text should not be greater than 500 Character."
															ControlToValidate="txtName" Display="None" ClientValidationFunction="validateName"></asp:CustomValidator>
														<asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="clsLabelAuto" ErrorMessage="Name Required."
															ControlToValidate="txtname" Display="None"></asp:RequiredFieldValidator>
														<script type="text/javascript">
															function validateName(source, args) {
																var textLen = $get("txtName").value.length;
																if (textLen > 500) {
																	args.IsValid = false;
																	return;
																}

															}
														</script>
													</td>
												</tr>
												<tr>
													<td>
														<table>
															<tr>
																<td>
																	<span id="lblName1" class="clsLabelStar">*</span>
																</td>
																<td>
																	<span id="lblName" class="clsLabelAuto">Name </span>
																</td>
																<td>
																	<asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Name"
																		Text="<%# mTerm.Terms %>" MaxLength="500" Height="50px" Width="500px" TextMode="MultiLine">
																	</asp:TextBox>
																</td>
															</tr>
														</table>
													</td>
												</tr>
												<tr>
													<td>
														<span id="lblSearch" class="clsLabelHeader">Term List</span>
													</td>
												</tr>
												<tr>
													<td>
														<asp:GridView ID="dgTerm" runat="server" CellPadding="5" GridLines="Horizontal"
															CssClass="clsGridNewStyle" AllowSorting="True" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
															DataKeyNames="ID">
															<AlternatingRowStyle CssClass="clsdgAltItem" />
															<RowStyle CssClass="clsdgItem" />
															<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
															<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
															<PagerSettings FirstPageText="First" LastPageText="Last" />
															<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
															<Columns>
																<%-- 0--%>
																<asp:BoundField Visible="False" DataField="ID" HeaderText="TermID"></asp:BoundField>
																<%--1--%>
																<asp:BoundField DataField="Terms" SortExpression="Terms" HeaderText="Terms">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Width="500px" Wrap="true" CssClass="TextBreak" />
																</asp:BoundField>
																<%--2--%>
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
																								CommandName="EditRec" CausesValidation="false"
																								ToolTip="Click to Edit record"
																								ImageUrl="~/images/edit.png" />
																						</td>
																						<td>
																							<asp:ImageButton ID="deleteICN" class="largerActionICNS" runat="server"
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
				parent.ParentCallBackFunctionForTerm();
				return false;
			}
		</script>
		<%--End--%>
		<%--Set page layout when open as popup aspx page--%>
		<script type="text/javascript">

			<% Dim mopen As String = Request.QueryString("Type") %>
			<% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

				$(document).ready(function () {
					SetPageLayout();
					if ($.browser.msie) {
						parent.IFrameTermStateComplete();
					}

				});
			<% End if %>
			Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
			function endRequestHandler() {
				SetPageLayout();
			}

			function SetPageLayout() {

				<% Dim mopenas As String = Request.QueryString("Type") %>
				<% If Not mopenas Is Nothing AndAlso mopenas = "pup" Then %>  
					ReSetPageLayout();
					onResize();
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
