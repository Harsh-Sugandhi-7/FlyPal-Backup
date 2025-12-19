<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfUserMappingwithStore_Ajax.aspx.vb"
	Inherits="Flypal.wfUserMappingwithStore_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="HEAD1" runat="server">
	<title>User Mapping with Store</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
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
					<asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
						<table id="tblLedgerList" class="clstablelistin">
							<tr>
								<td class="clsFormHeader1Newstyle">
									<table width="100%">
										<tr>
											<td>
												<span id="lblTitle" class="clsFormHeader">User Mapping with Store
												</span>
											</td>
											<td align="right">
												<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlTopButton">
													<ContentTemplate>
														<table>
															<tr>
																<td>
																	<asp:Button ID="btnSaveTop" runat="server" 
																		CssClass="clsbtnH clsinfoH" 
																		Text="Save And Close" Width="120px"></asp:Button>
																</td>
																<td align="right">
																	<asp:Button ID="btnCloseTop" runat="server"
																		CssClass="clsbtnH clsinfoH" Text="Close"
																		CausesValidation="false" Visible="false"></asp:Button>
																</td>
															</tr>
														</table>
													</ContentTemplate>
													<Triggers>
														<asp:AsyncPostBackTrigger ControlID="btnSaveBottom" EventName="click" />
														<asp:AsyncPostBackTrigger ControlID="btnCloseBottom" EventName="click" />
													</Triggers>
												</asp:UpdatePanel>
											</td>
										</tr>
									</table>									
								</td>
							</tr>
							<tr>
								<td align="left">
									<asp:UpdatePanel runat="server" ID="upnlStore" UpdateMode="Conditional">
										<ContentTemplate>
											<div style="width: 100%">
												<br />
												<asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True">
													List of User
												</asp:Label>
											</div>
											<div style="width: 100%">
												<asp:GridView ID="dgUserList" runat="server" AllowSorting="True"
													AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
													CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
													>
													<AlternatingRowStyle CssClass="clsdgAltItem" />
													<RowStyle CssClass="clsdgItem" />
													<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True"
																 ForeColor="black" HorizontalAlign="Left" />
													<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
													<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
													<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
													<Columns>
														<asp:BoundField DataField="ID" HeaderText="ID" HeaderStyle-CssClass="hideGridColumn"
															ItemStyle-CssClass="hideGridColumn">
															<HeaderStyle HorizontalAlign="Left" />
															<ItemStyle HorizontalAlign="Left" />
														</asp:BoundField>
														<asp:TemplateField HeaderText="Select">
															<HeaderTemplate>
																<asp:CheckBox ID="chkSelectUser" ClientIDMode="Static" runat="server" Text="Select"
																	onclick="CheckUncheck(this);" />
															</HeaderTemplate>
															<ItemTemplate>
																<asp:CheckBox ID="chkSelect" runat="server" ClientIDMode="Static" CssClass="clsCheckBox"
																	Checked='<%# DataBinder.Eval(Container.DataItem,"IsSelected") %>' />
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Left" />
															<ItemStyle HorizontalAlign="Left" />
														</asp:TemplateField>
														<asp:BoundField DataField="UserName" HeaderText="User">
															<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
														</asp:BoundField>
														<asp:BoundField DataField="UserID" HeaderText="UserID" HeaderStyle-CssClass="hideGridColumn"
															ItemStyle-CssClass="hideGridColumn">
															<HeaderStyle HorizontalAlign="Left" />
															<ItemStyle HorizontalAlign="Left" />
														</asp:BoundField>
													</Columns>
												</asp:GridView>
											</div>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
								<!--End-->
							</tr>
							<tr>
								<td align="right">
									<table>
										<tr>
											<td>
												<asp:Button ID="btnSaveBottom" runat="server"
													CssClass="clsbtnH clsinfoH" Text="Save And Close"
													Width="100px" Visible="false">
												</asp:Button>
											</td>
											<td align="right">
												<asp:Button ID="btnCloseBottom" runat="server" 
													CausesValidation="false" CssClass="clsbtnH clsinfoH"
													Text="Close" Visible="false"></asp:Button>
											</td>
										</tr>
									</table>
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
				parent.ParentCallBackFunctionForUserMappingwithStore();
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
						parent.IFrameUserMappingwithStoreStateComplete();
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
	<script type="text/javascript">
		function CheckUncheck(chkBoxAll) {
			var str = chkBoxAll.id;
			var status = $("#chkSelectUser").attr("checked");
			$("#dgUserList tr:gt(0)").find(":checkbox").each(function () {
				if (status == "checked") {
					$(this).attr("checked", status);
				}
				else {
					$(this).removeAttr("checked");
				}
			});
		}
	</script>
</body>
</html>
