<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMELSnagCorrectiveActionLogInfo_AJAX.aspx.vb"
	Inherits="Flypal.wfMELSnagCorrectiveActionLogInfo_AJAX" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" />
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<title>Select Log</title>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

</head>
<body>
	<form id="form1" runat="server">
		<div>
			<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
				EnablePageMethods="true">
			</asp:ScriptManager>
			<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
					<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
				</ContentTemplate>
			</asp:UpdatePanel>
		</div>
		<table class="clstablelistout" id="tblmain" Style="width: 60%">
			<tr>
				<td>
					<asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
						<table id="tblInner" class="clstablelistin">
							<tr>
								<td colspan="3" class="clsFormHeader1Newstyle">
									<table width="100%">
										<tr>
											<td>
												<asp:Label ID="lbltitle" CssClass="clsFormHeader" 
													runat="server" Text="Flight Log Info." />
											</td>
											<td align="right">
												<asp:Button ID="btnClose" 
													CssClass="clsbtnH clsinfoH" 
													runat="server" 
													ToolTip="Close Flight Log Info. screen"
													CausesValidation="False" 
													Text="Close" />
											</td>
										</tr>
									</table>

								</td>
							</tr>
							<tr>
								<td colspan="3">
									<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlResult">
										<ContentTemplate>
											<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td>
									<br />
								</td>
							</tr>
							<tr>
								<td colspan="3">
									<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlGrid">
										<ContentTemplate>
											<asp:GridView ID="dgLogList" runat="server" ToolTip="List of logs."
												AutoGenerateColumns="False" AllowSorting="True" 
												CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
												<AlternatingRowStyle CssClass="clsdgAltItem" />
												<RowStyle CssClass="clsdgItem" />
												<HeaderStyle BackColor="white" CssClass="clsdgHeader" 
													Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
												<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
												<PagerSettings Mode="NumericFirstLast" 
													FirstPageText="First" LastPageText="Last" />
												<PagerStyle BackColor="White" CssClass="paging" 
													ForeColor="Black" HorizontalAlign="Right" />
												<Columns>
													<asp:BoundField Visible="False" DataField="LogID" HeaderText="ID" />
													<asp:BoundField DataField="LogDate" HeaderText="Log Date">
														<ItemStyle Wrap="false" />
													</asp:BoundField>
													<asp:BoundField DataField="LogTextNo" 
														SortExpression="LogTextNo" HeaderText="Log No.">
														<ItemStyle Wrap="false" />
													</asp:BoundField>
													<asp:BoundField DataField="LogPageNo" 
														SortExpression="LogPageNo" HeaderText="Log Page No.">
													</asp:BoundField>
													<asp:BoundField DataField="FlightNo" 
														SortExpression="FlightNo" HeaderText="Flight No.">
													</asp:BoundField>
													<asp:BoundField DataField="FinalHours"
														SortExpression="FinalHours" HeaderText="Hours final">
														<HeaderStyle HorizontalAlign="Right" />
														<ItemStyle HorizontalAlign="Right" />
													</asp:BoundField>
													<asp:BoundField DataField="FinalCyclesLandings" 
														SortExpression="FinalCyclesLandings" HeaderText="Landings final">
														<HeaderStyle HorizontalAlign="Right" />
														<ItemStyle HorizontalAlign="Right" />
													</asp:BoundField>
												</Columns>
											</asp:GridView>
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
				parent.ParentCallBackFunctionForSelectLog();
				return false;
			}

		</script>
		<%--End--%>

		<%--Set page layout when open as popup aspx page--%>
		<div>

			<script type="text/javascript">

				<% Dim OpenAs As String = Request.QueryString("Type") %>
				<% If OpenAs IsNot Nothing AndAlso OpenAs = "pup" Then %>  

					$(document).ready(function () {
						SetPageLayout();

						if ($.browser.msie) {
							parent.IFrameSelectLogStateComplete();
						}

					});

				<% End if %>

				Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);

				function endRequestHandler() {
					SetPageLayout();
				}

				function SetPageLayout() {

					<% Dim Type As String = Request.QueryString("Type") %>
					<% If Type IsNot Nothing AndAlso Type = "pup" Then %>  
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

		</div>

	</form>
</body>
</html>
