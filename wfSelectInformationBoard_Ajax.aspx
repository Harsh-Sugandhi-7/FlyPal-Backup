<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSelectInformationBoard_Ajax.aspx.vb"
	Inherits="Flypal.wfSelectInformationBoard_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<title>Information Board Selection</title>
	<link id="MainStyle" type="text/css" rel="stylesheet">
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
</head>
<body>
	<form id="form1" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
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
						<table class="clstablelistin" id="tblInner">
							<tr>
								<td class="clsFormHeader1Newstyle">
									<span id="lbltitle" class="clsFormHeader">Information Board Selection</span>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table width="100%">
												<tr>
													<td>
														<asp:Label ID="lblSelectInformation" runat="server" CssClass="clsLabelHeader"></asp:Label>
													</td>
													<td align="right">
														<table id="tblActionButtonsTop">
															<tr>
																<td>
																	<asp:Button ID="btnDoneTop" runat="server" 
																		CssClass="clsbtnH clsinfoH1"
																		Text="Done" CausesValidation="False">
																	</asp:Button>
																</td>
																<td>
																	<asp:Button ID="btnCloseTop" runat="server" 
																		CssClass="clsbtnH clsinfoH1"
																		Text="Close" CausesValidation="False" 
																		ToolTip="Click to go back to the previous page">
																	</asp:Button>
																</td>
															</tr>
														</table>
													</td>
												</tr>
												<tr>
													<td colspan="2">
														<asp:GridView ID="dgSelectInformationList" runat="server" AllowSorting="True"
															AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
															CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" >
															<AlternatingRowStyle CssClass="clsdgAltItem" />
															<RowStyle CssClass="clsdgItem" />
															<HeaderStyle BackColor="white" CssClass="clsdgHeader" 
																Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
															<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
															<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
															<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" 
																HorizontalAlign="Right" />
															<Columns>
																<%--0--%>
																<asp:BoundField DataField="Reference" SortExpression="Reference" HeaderText="Reference">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																</asp:BoundField>

																<%--1--%>
																<asp:BoundField Visible="False" DataField="MachineInfo" SortExpression="MachineInfo" HeaderText="Machine Info.">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																</asp:BoundField>

																<%--2--%>
																<asp:BoundField Visible="False" DataField="AssemblyType" SortExpression="AssemblyType" HeaderText="Assembly Type">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																</asp:BoundField>

																<%--3--%>
																<asp:BoundField Visible="False" DataField="AssemblyInfo" SortExpression="AssemblyInfo" HeaderText="Assembly Info.">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																</asp:BoundField>

																<%--4--%>
																<asp:BoundField DataField="MonitorInfo" SortExpression="MonitorInfo" HeaderText="Monitor Info">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																</asp:BoundField>

																<%--5--%>
																<asp:BoundField DataField="MonitorType" SortExpression="MonitorType" HeaderText="Monitor Type">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																</asp:BoundField>

																<%--6--%>
																<asp:BoundField DataField="Desc" SortExpression="Description" HeaderText="Description">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																</asp:BoundField>

																<%--7--%>
																<asp:BoundField DataField="DoneRemark" SortExpression="DoneRemark" HeaderText="Remark">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																</asp:BoundField>

																<%--8--%>
																<asp:BoundField DataField="DueOnValueFormatted" HeaderText="Due At" HtmlEncode="false">
																	<ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
																	<HeaderStyle HorizontalAlign="Right" />
																</asp:BoundField>

																<%--9--%>
																<asp:ButtonField Visible="False" Text="Select" HeaderText="Select" CommandName="Select"></asp:ButtonField>

																<%--10--%>
																<asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Left">
																	<ItemTemplate>
																		<asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>
																	</ItemTemplate>
																</asp:TemplateField>
															</Columns>
														</asp:GridView>
													</td>
												</tr>
												<tr>
													<td colspan="2">
														<asp:GridView ID="dgCertificateList" runat="server" Visible="False" 
															ToolTip="Aircraft Certificate List." ShowHeaderWhenEmpty="true" 
															AllowSorting="True" AutoGenerateColumns="False" 
															CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
															<AlternatingRowStyle CssClass="clsdgAltItem" />
															<RowStyle CssClass="clsdgItem" />
															<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" 
																ForeColor="black" HorizontalAlign="Left" />
															<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
															<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
															<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
															<Columns>
																<%--0--%>
																<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>

																<%--1--%>
																<asp:BoundField Visible="False" DataField="SerialNo" HeaderText="Sr. No.">
																	<HeaderStyle HorizontalAlign="Left" />
																	<ItemStyle Wrap="False"></ItemStyle>
																</asp:BoundField>

																<%--2--%>
																<asp:BoundField DataField="CertificateName" SortExpression="CertificateName" HeaderText="Name">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="False"></ItemStyle>
																</asp:BoundField>

																<%--3--%>
																<asp:BoundField DataField="CertificateNo" SortExpression="CertificateNo" HeaderText="No.">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="False"></ItemStyle>
																</asp:BoundField>

																<%--4--%>
																<asp:BoundField DataField="IssueDateFormatted" HeaderText="Issue Date">
																	<HeaderStyle HorizontalAlign="Left" />
																	<ItemStyle Wrap="False"></ItemStyle>
																</asp:BoundField>

																<%--5--%>
																<asp:BoundField DataField="ExpiryDateFormatted" HeaderText="Expiry Date">
																	<HeaderStyle HorizontalAlign="Left" />
																	<ItemStyle Wrap="False"></ItemStyle>
																</asp:BoundField>

																<%--6--%>
																<asp:BoundField DataField="ElapsedDays" HeaderText="Elapsed Days">
																	<HeaderStyle HorizontalAlign="Left" />
																</asp:BoundField>

																<%--7--%>
																<asp:BoundField DataField="RemainingDays" HeaderText="Remaining Days">
																	<HeaderStyle HorizontalAlign="Left" />
																</asp:BoundField>

																<%--8--%>
																<asp:BoundField DataField="Remark" SortExpression="Remark" HeaderText="Remark">
																	<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																</asp:BoundField>

																<%--9--%>
																<asp:ButtonField Visible="False" Text="Select" HeaderText="Select" CommandName="Select"></asp:ButtonField>

																<%--10--%>
																<asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Left">
																	<ItemTemplate>
																		<asp:CheckBox ID="chkSelectCertificate" runat="server"></asp:CheckBox>
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
								<td align="right">
									<asp:UpdatePanel ID="upnlActionButtons" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table id="tblActionButtons">
												<tr>
													<td>
														<asp:Button ID="btnDoneBottom" runat="server" 
															CssClass="clsbtnH clsinfoH1" Text="Done" CausesValidation="False">
														</asp:Button>
													</td>
													<td>
														<asp:Button ID="btnCloseBottom" runat="server" CssClass="clsbtnH clsinfoH1"
															Text="Close" CausesValidation="False"
															ToolTip="Click to go back to the previous page">
														</asp:Button>
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

		<!-- Board Info Master Popup Window -->

		<%--call parent function after completing subroutine..(when page open as popup)--%>
		<script type="text/javascript">
			function CallParentCallback() {
				parent.ParentCallBackFunctionForBoardInfoMaster();
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
						parent.IFrameBoardInfoMasterStateComplete();
					}
				});

			<% End if %>

			Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);

			function endRequestHandler() {
				SetPageLayout();
			}

			function SetPageLayout() {

				<% Dim mOpenAs As String = Request.QueryString("Type") %>

				<% If mOpenAs IsNot Nothing AndAlso mOpenAs = "pup" Then %>

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
