<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSelectPeriod_Ajax.aspx.vb"
	Inherits="Flypal.wfSelectPeriod_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<title>Select Periods</title>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5" ms_positioning="GridLayout">
	<form id="Form1" method="post" runat="server">
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
					<asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server" Width="250px">
						<table class="clstablelistin" id="tblLedgerList">
							<tr>
								<td class="clsFormHeader1Newstyle">
									<table width="100%">
										<tr>
											<td>
												<span id="lblTitle" class="clsFormHeader">Select Periods</span>
											</td>
											<td align="right">
												<asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<asp:Button ID="btnDone" runat="server" CssClass="clsbtnH clsinfoH"
															ToolTip="Click to add checked periods to the previous page."
															Text="Done" CausesValidation="False">
														</asp:Button>
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlPeriodsList" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table id="Table1" width="100%">
												<tr>
													<td>
														<br />
													</td>
												</tr>
												<tr>
													<td>
														<span id="lblPeriodList" class="clsLabelHeader">Periods List</span>
													</td>
												</tr>
												<tr>
													<td>
														<asp:GridView ID="dgSelectPeriod" runat="server" CssClass="clsGridNewStyle" ShowHeaderWhenEmpty="true"
															AutoGenerateColumns="False" AllowSorting="True" CellPadding="5" ForeColor="Black"
															GridLines="Horizontal">
															<AlternatingRowStyle CssClass="clsdgAltItem" />
															<RowStyle CssClass="clsdgItem" />
															<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
															<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
															<PagerSettings FirstPageText="First" LastPageText="Last" />
															<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
															<Columns>
																<asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Left">
																	<ItemTemplate>
																		<asp:CheckBox ID="chkSelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelected") %>' />
																	</ItemTemplate>
																</asp:TemplateField>
																<asp:BoundField DataField="PeriodName" HeaderText="Periods">
																	<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="False"></ItemStyle>
																</asp:BoundField>
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
				parent.ParentCallBackFunctionForAddPeriod();
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
							parent.IFrameStateComplete();
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

	</form>
</body>
</html>
