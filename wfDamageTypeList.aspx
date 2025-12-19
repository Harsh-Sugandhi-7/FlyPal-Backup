<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDamageTypeList.aspx.vb"
	Inherits="Flypal.wfDamageTypeList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Damage Type</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

	<style type="text/css">

		.displayBlock{
			width: 250px;
		}

	</style>
</head>
<body>
	<form id="form1" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1"
			EnablePageMethods="true">
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
						<asp:Panel ID="pnlmain" runat="server" CssClass="clsPanel1">
							<asp:UpdatePanel runat="server" ID="upnlValidationSummary" UpdateMode="Conditional">
								<ContentTemplate>
									<table width="100%">
										<tr>
											<td class="clsFormHeader1Newstyle">
												<table width="100%">
													<tr>
														<td>
															<asp:Label ID="lblTitle" CssClass="clsFormHeader displayBlock" runat="server">Damage Type [New]</asp:Label>
														</td>
														<td align="right">
															<asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
																<ContentTemplate>
																	<table align="right">
																		<tr>
																			<td align="right">
																				<asp:Button ID="btnAdd" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
																					Text="New" ToolTip="Click to add new Damage Type"
																					CausesValidation="False"></asp:Button>
																			</td>
																			<td>
																				<asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH" runat="server"
																					Text="Save" ToolTip="Click to save the Damage Type Information"
																					ValidationGroup="a"></asp:Button>
																			</td>
																			<td>
																				<asp:Button ID="btnClose" CssClass="clsbtnH clsinfoH"
																					runat="server" Text="Close" ToolTip="Click to close Damage Type  screen"
																					CausesValidation="False"></asp:Button>
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
											<td colspan="2">
												<asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
													CssClass="clsValidationSummary" ValidationGroup="a"></asp:ValidationSummary>
												<asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
													ErrorMessage="Name Required." Display="None" ValidationGroup="a" CssClass="clsLabelAuto"></asp:RequiredFieldValidator>
											</td>
										</tr>
									</table>
								</ContentTemplate>
							</asp:UpdatePanel>
							<asp:UpdatePanel runat="server" ID="upnlDamageType" UpdateMode="Conditional">
								<ContentTemplate>
									<table width="100%">
										<tr>
											<td colspan="4">
												<span id="lblDamageType" class="clsLabelHeader">Damage Type</span>
											</td>
										</tr>
										<tr>
											<td>
												<table width="100%">
													<tr>
														<td>
															<span id="lblNameStar1" class="clsLabelStar">*</span>
														</td>
														<td>
															<span id="lblName" class="clsLabel">Name</span>
														</td>
														<td>
															<asp:TextBox ID="txtName" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter Damage Type Name"
																MaxLength="50">
															</asp:TextBox>
														</td>
														<td></td>
													</tr>
												</table>
											</td>
											<td></td>
											<td colspan="2"></td>
										</tr>
									</table>
								</ContentTemplate>
							</asp:UpdatePanel>
							<asp:UpdatePanel runat="server" ID="upnlGridView" UpdateMode="Conditional">
								<ContentTemplate>
									<table width="100%">
										<tr>
											<td>
												<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
											</td>
										</tr>
										<tr>
											<td>
												<asp:GridView ID="dgDamageTypeList" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
													CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" AllowPaging="True" PageSize="10">
													<AlternatingRowStyle CssClass="clsdgAltItem" />
													<RowStyle CssClass="clsdgItem" />
													<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
													<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
													<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
													<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
													<Columns>
														<%--0--%>
														<asp:BoundField DataField="ID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ID"
															ItemStyle-CssClass="hideGridColumn">
															<HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
															<ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
														</asp:BoundField>
														<%--1--%>
														<asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name">
															<HeaderStyle HorizontalAlign="Left" />
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
																						ToolTip="Click to Edit record" CausesValidation="false"
																						CommandName="EditView" ImageUrl="~/images/edit.png" />
																				</td>

																				<td>
																					<asp:ImageButton ID="deleteICN" class="actionICNS  largerActionICNS" runat="server"
																						CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																						ToolTip="Click to Delete record" CausesValidation="false"
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
						</asp:Panel>
					</td>
				</tr>
			</table>
			<%--AJAX- Add UpdateProgress to show loading for Longer Process--%>
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
		<!--call parent function after completing subroutine..(when page open as popup)-->
		<script type="text/javascript">
			function CallParentCallback() {
				parent.ParentCallBackFunctionForDamageTypeList();
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
					parent.IFrameDamageTypeListStateComplete();
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
