<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMaintenanceProgram_Ajax.aspx.vb"
	Inherits="Flypal.wfMaintenanceProgram_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Maintenance Program</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<link id="MainStyle" type="text/css" rel="stylesheet" />

	<script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

</head>
<body>
	<form id="formMaintenanceProgram" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
			EnablePageMethods="true">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table class="clstablelistout" id="tblmain">
			<tr>
				<td>
					<asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
						<table id="tblInner" class="clstablelistin">
							<tr>
								<td class="clsFormHeader1Newstyle">
									<table width="100%">
										<tr>
											<td>
												<asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">
															Maintenance Program [New]
														</asp:Label>
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
											<td align="right" colspan="4">
												<asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<asp:Button ID="btnAdd" runat="server" CssClass="clsbtnH clsinfoH"
															ToolTip="Add the new Maintenance Program."
															Text="New" CausesValidation="False"></asp:Button>

														<asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH"
															ToolTip="Save the Maintenance Program Information."
															ValidationGroup="1" Text="Save"></asp:Button>

														<asp:Button ID="btnClose" runat="server"
															CssClass="clsbtnH clsinfoH"
															ToolTip="Close Maintenance Program screen."
															Text="Close" CausesValidation="False"></asp:Button>
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table>
												<tr id="ValidationSummary">
													<td colspan="4">
														<asp:ValidationSummary ID="ValidationSummary1" runat="server" 
															CssClass="clsValidationSummary" ValidationGroup="1">
														</asp:ValidationSummary>
														<asp:RequiredFieldValidator ID="rfvName" runat="server" 
															CssClass="clsLabelAuto" ErrorMessage="Maintenance Program Name Required."
															Display="None" ControlToValidate="txtMaintenanceProgramName" ValidationGroup="1">
														</asp:RequiredFieldValidator>
														<asp:CustomValidator ID="cvName" runat="server" Display="None" 
															ControlToValidate="txtMaintenanceProgramName"
															ErrorMessage="Maintenance Program Name should not be greater Than 250 Characters ."
															ClientValidationFunction="validateName" ValidationGroup="1">
														</asp:CustomValidator>
														<script type="text/javascript">

															function validateName(source, args) {

																var Value = $get("txtMaintenanceProgramName").value.length;
																if (Value > 250) {
																	args.IsValid = false;
																	return;
																}
															}

														</script>
													</td>
												</tr>
												<tr>
													<td colspan="4">
														<span id="lblMaintenanceProgramDetails" 
															class="clsLabelHeader">Maintenance Program Details</span>
													</td>
												</tr>
												<tr>
													<td>
														<span id="lblName1" class="clsLabelStar">*</span>
													</td>
													<td>
														<span id="lblName" class="clsLabelAuto">Name</span>
													</td>
													<td colspan="2">
														<asp:TextBox ID="txtMaintenanceProgramName" runat="server"
															CssClass="clsTextBoxTagSearchMultilineNewstyle"
															ToolTip="Enter Maintenance Program's Name"
															Text="<%# mMaintenanceProgram.Name %>"
															MaxLength="250" TextMode="MultiLine"
															Width="370px" Height="35px">
														</asp:TextBox>
													</td>
												</tr>
												<tr>
													<td>
														<br />
													</td>
												</tr>
												<tr>
													<td colspan="4">
														<asp:Label ID="lblSearch" runat="server" CssClass="clsLabelHeader">
															Maintenance Program List
														</asp:Label>
													</td>
												</tr>
												<tr>
													<td colspan="4">
														<asp:GridView ID="dgMaintenanceProgram" runat="server" 
															CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
															ToolTip="Maintenance Program list" DataKeyNames="ID" 
															ShowHeaderWhenEmpty="true" AutoGenerateColumns="False">
															<AlternatingRowStyle CssClass="clsdgAltItem" />
															<RowStyle CssClass="clsdgItem" />
															<HeaderStyle BackColor="white" CssClass="clsdgHeader" 
																Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
															<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
															<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
															<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
															<Columns>
																<%--0--%>
																<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
																<%--1--%>
																<asp:BoundField DataField="Name" HeaderText="Name">
																	<HeaderStyle HorizontalAlign="Left" />
																	<ItemStyle Wrap="true" CssClass="TextBreak" />
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
																							<asp:ImageButton ID="editICN" class="actionICNS" runat="server"
																								CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																								ToolTip="Click to Edit record." CausesValidation="false"
																								CommandName="View" ImageUrl="~/images/edit.png" />
																						</td>
																						<td>
																							<asp:ImageButton ID="deleteICN" class="actionICNS  largerActionICNS" runat="server"
																								CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																								ToolTip="Click to Delete record." CommandName="DeleteRec"
																								ImageUrl="~/images/delete.png" CausesValidation="false" />
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

		<div>

			<!-- Maint Program Master Popup Window -->

			<%--call parent function after completing subroutine..(when page open as popup)--%>
			<script type="text/javascript">

				function CallParentCallback() {
					parent.ParentCallBackFunctionForMaintProgramMaster();
					return false;
				}

			</script>

			<%--Set page layout when open as popup aspx page--%>
			<script type="text/javascript">

				<% Dim mopen As String = Request.QueryString("Type") %>
				<% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

					$(document).ready(function () {
						SetPageLayout();
						if ($.browser.msie) {
							parent.IFrameMaintProgramMasterStateComplete();
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
					var tempMargtop = $("body #tblmain:eq(0),html #tblmain:eq(0)").outerHeight();
					var windowheight = $(window).height();

					if (tempMargtop >= windowheight) {
						$("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto' });
					}
					else {
						var margintop = (windowheight / 2) - (tempMargtop / 2);
						$("body #tblmain:eq(0),html #tblmain:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
					}

				}
			</script>
			<%--End--%>

		</div>

	</form>

	<script language="JavaScript" type="text/javascript">

		function CallParentFunction() {
			window.parent.autoResizeMaintPolicyList();
		}

	</script>
</body>
</html>
