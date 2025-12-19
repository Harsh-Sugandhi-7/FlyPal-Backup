<%@ Page Language="vb" AutoEventWireup="false"
	CodeBehind="wfDiscrepancyTroubleShootView_Ajax.aspx.vb"
	Inherits="Flypal.DiscrepancyTroubleShootView" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<title>Discrepancy TroubleShoot</title>
	<meta http-equiv="x-ua-compatible" content="IE=9" />
	<meta name="vs_showGrid" content="True">
	<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
	<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
	<meta name="vs_defaultClientScript" content="JavaScript">
	<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	<link id="MainStyle" rel="stylesheet" type="text/css">

	<script language="javascript" src="VALIDATEFUNCTIONS.js" />
	<script language="javascript">
		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
		}
	</script>

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>	

</head>
<body>
	<form id="Form1" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
			runat="server">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<div>
			<asp:UpdatePanel ID="upnlMaint" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
					<asp:Panel ID="pnlMain" runat="server" CssClass="clspnl1">
						<table id="tblmain" class="clstablelistout" style="width: 60%">
							<tr>
								<td class="clsFormHeader1Newstyle">
									<table width="100%">
										<tr>
											<td>
												<asp:UpdatePanel ID="upnlTitle" runat="server"
													UpdateMode="Conditional">
													<ContentTemplate>
														<asp:Label ID="lblTitle" runat="server"
															CssClass="clsFormHeader"
															Text="Discrepancy Troubleshoot" />
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
											<td align="right">
												<asp:UpdatePanel ID="upnlAdd" runat="server"
													UpdateMode="Conditional">
													<ContentTemplate>
														<asp:Button ID="btnBack" runat="server"
															CausesValidation="False" Text="Close"
															CssClass="clsbtnH clsinfoH"
															ToolTip="Click to close" />
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<fieldset class="clsFieldSetNewStyle" id="fldTroubleShootList">

												<legend style="font-weight: bold">TroubleShooting List</legend>
												<br />

												<asp:GridView ID="dgDiscrepancyTroubleShootList"
													runat="server" DataKeyNames="ID"
													ShowHeaderWhenEmpty="True" AllowPaging="True"
													AutoGenerateColumns="False" PageSize="10"
													CssClass="clsGridNewStyle"
													GridLines="Horizontal" CellPadding="5">
													<AlternatingRowStyle CssClass="clsdgAltItem" />
													<RowStyle CssClass="clsdgItem" />
													<HeaderStyle BackColor="white"
														CssClass="clsdgHeader" Font-Bold="True"
														ForeColor="black" HorizontalAlign="Left" />
													<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
													<PagerSettings Mode="NumericFirstLast"
														FirstPageText="First" LastPageText="Last" />
													<PagerStyle BackColor="White" CssClass="paging"
														ForeColor="Black" HorizontalAlign="Right" />
													<Columns>
														<%--0--%>
														<asp:BoundField DataField="ID" HeaderText="ID"
															Visible="false" />
														<%--1--%>
														<asp:BoundField DataField="LogID"
															HeaderText="LogID" Visible="false">
															<HeaderStyle HorizontalAlign="Left" />
														</asp:BoundField>
														<%--2--%>
														<asp:BoundField DataField="RecordCount"
															SortExpression="RecordCount" HeaderText="Sr.No."
															HtmlEncode="False">
															<HeaderStyle HorizontalAlign="Center" />
															<ItemStyle HorizontalAlign="Center" />
														</asp:BoundField>
														<%--3--%>
														<asp:BoundField DataField="LogDateFormatted"
															HeaderText="Log Date">
															<HeaderStyle HorizontalAlign="Left" />
															<ItemStyle Wrap="False" />
														</asp:BoundField>
														<%--4--%>
														<asp:BoundField DataField="LogTextNo"
															HeaderText="Log No.">
															<HeaderStyle HorizontalAlign="Left" />
															<ItemStyle Wrap="False" HorizontalAlign="Left" />
														</asp:BoundField>
														<%--5--%>
														<asp:BoundField DataField="LogNo"
															HeaderText="Log No."
															HtmlEncode="False" Visible="false">
															<HeaderStyle HorizontalAlign="Left" />
															<ItemStyle Wrap="False" />
														</asp:BoundField>
														<%--6--%>
														<asp:BoundField DataField="Maintenance"
															HeaderText="Troubleshooting Steps">
															<HeaderStyle HorizontalAlign="Left" />
														</asp:BoundField>
														<%--7--%>
														<asp:BoundField DataField="NRCWONO" HeaderText="NRC/WO No">
															<ItemStyle Wrap="False" />
														</asp:BoundField>
														<%--8--%>
														<asp:BoundField DataField="DoneByName"
															HeaderText="Work Carried Out By">
															<HeaderStyle HorizontalAlign="Left"
																Wrap="False" />
														</asp:BoundField>
													</Columns>
												</asp:GridView>

											</fieldset>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
						</table>
					</asp:Panel>
				</ContentTemplate>
			</asp:UpdatePanel>

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

		</div>

		<div>

			<script type="text/javascript">

				<% Dim Open As String = Request.QueryString("Type") %>
				<% If Open IsNot Nothing AndAlso Open = "pup" Then %>  

						$(document).ready(function () {

							SetPageLayout();

							if ($.browser.msie) {
								parent.IframeDiscrepancyTroubleShootViewStateComplete();
							}

						});

				 <% End if %>

				Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);

				function endRequestHandler() {
					SetPageLayout();
				}

				function SetPageLayout() {

					<% Dim OpenAs As String = Request.QueryString("Type") %>

					<% If Not OpenAs Is Nothing AndAlso OpenAs = "pup" Then %>  

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

			<script type="text/javascript">

				function CallParentCallback() {
					parent.ParentCallBackFunctionForDiscrepancyTroubleshootView();
					return false;
				}

				function CallautoResize() {
					parent.autoResizeMaintActivity();
					return false;
				}

			</script>

		</div>


	</form>
</body>
</html>

