<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMSPAssemblySelection_Ajax.aspx.vb" Inherits="Flypal.wfMSPAssemblySelection_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Applicable MSP Assembly</title>
	<script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

	<script language="javascript">
		function openFilel() {
			str = "wfFileView.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
	</script>

</head>
<body>
	<form id="form1" runat="server">
		<asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox id="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table class="clstablelistout" id="tblmain">
			<tr>
				<td>
					<asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
						<table id="tblInner" class="clstablelistin">
							<tr>
								<td class="clsFormHeader1Newstyle">
									<table width="100%">
										<tr>
											<td>
												<asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
													<ContentTemplate>
														<asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">
															Select Maintenance Support Plan 
														</asp:Label>
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
											<td align="right">
												<asp:UpdatePanel runat="server" ID="upnlButton" UpdateMode="Conditional">
													<ContentTemplate>
														<table>
															<tr>
																<td>
																	<asp:Button ID="btnBack" runat="server" Text="Close"
																		class="clsbtnH clsinfoH" ToolTip="Click to close" />
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
								<td>
									<fieldset class="clsFieldSetNewStyle">
										<asp:UpdatePanel runat="server" ID="upnlMSPAssembly" UpdateMode="Conditional">
											<ContentTemplate>
												<table width="100%">
													<tr>
														<td>
															<br />
														</td>
													</tr>
													<tr>
														<td>
															<asp:GridView ID="dgMSPAssembly" runat="server" ShowHeaderWhenEmpty="True"
																AutoGenerateColumns="False" ForeColor="Black" OnDataBound="OnDataBound"
																CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
																<AlternatingRowStyle CssClass="clsdgAltItem" />
																<RowStyle CssClass="clsdgItem" />
																<HeaderStyle BackColor="white" CssClass="clsdgHeader"
																	Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
																<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
																<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
																<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
																<Columns>

																	<%--0--%>
																	<asp:BoundField DataField="DateFormatted" HeaderText="Date">
																		<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>

																	<%--1--%>
																	<asp:BoundField DataField="ContractNo" HeaderText="Contract No.">
																		<HeaderStyle Wrap="False" HorizontalAlign="Left" />
																		<ItemStyle Wrap="False" HorizontalAlign="Left" />
																		<FooterStyle Wrap="False" />
																	</asp:BoundField>

																	<%--2--%>
																	<asp:BoundField DataField="VendorName" HeaderText="Vendor">
																		<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>

																	<%--3--%>
																	<asp:BoundField DataField="PlanName" HeaderText="Plan Name">
																		<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>

																	<%--4--%>
																	<asp:TemplateField HeaderStyle-HorizontalAlign="Center"
																		HeaderText="View" ItemStyle-HorizontalAlign="Center">
																		<ItemTemplate>
																			<asp:ImageButton ID="viewICN" class="attachmentICNS" runat="server"
																				CommandArgument='<%# Eval("MSPID") %>'
																				ToolTip="Click to View Attachment" CommandName="View"
																				ImageUrl="icons/CLIP01.ICO"
																				Visible='<%#  Eval("IsAttachmentAdded")%>' />
																		</ItemTemplate>
																		<HeaderStyle HorizontalAlign="Center" />
																		<ItemStyle HorizontalAlign="Center" />
																	</asp:TemplateField>

																	<%--5--%>
																	<asp:BoundField DataField="AssemblyName" HeaderText="Applicable To">
																		<HeaderStyle Wrap="False" HorizontalAlign="Left" />
																		<ItemStyle Wrap="False" HorizontalAlign="Left" />
																		<FooterStyle Wrap="False" />
																	</asp:BoundField>

																	<%--6--%>
																	<asp:ButtonField CommandName="Select" HeaderText="Select" Text="Select">
																		<HeaderStyle Wrap="False" HorizontalAlign="Left" />
																		<ItemStyle ForeColor="Blue" Wrap="False" />
																	</asp:ButtonField>

																	<%--7--%>
																	<asp:BoundField DataField="ID" HeaderText="ID"
																		HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />

																	<%--8--%>
																	<asp:BoundField DataField="MSPID" HeaderText="MSPID"
																		HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />

																	<%--9--%>
																	<asp:BoundField DataField="AssemblyID" HeaderText="AssemblyID" Visible="False" />

																	<%--10--%>
																	<asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded"
																		HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />

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
									</fieldset>
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

		<!--call parent function after completing subroutine..(when page open as popup)-->
		<script type="text/javascript">
			function CallParentCallback() {
				parent.ParentCallBackFunctionForMSPAssemblySelection();
				return false;
			}
		</script>


		<%--Set page layout when open as popup aspx page--%>
		<script type="text/javascript">

			 <% Dim openAs As String = Request.QueryString("Type") %>
			 <% If openAs IsNot Nothing AndAlso (openAs = "FromWO" Or openAs = "FromPurchaseOrder" Or openAs = "FromLineMaintenanceOrder") Then %>
					$(document).ready(function () {
						SetPageLayout();
						if ($.browser.msie) {
							parent.IframeMSPAssemblySelection();
						}
					});
			 <% End if %>

			Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);
			function endRequestHandler() {
				SetPageLayout();
			}

			function SetPageLayout() {

				<% Dim calledFrom As String = Request.QueryString("Type") %>
				<% If calledFrom IsNot Nothing AndAlso (calledFrom = "FromWO" Or calledFrom = "FromPurchaseOrder" Or calledFrom = "FromLineMaintenanceOrder") Then %>
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

