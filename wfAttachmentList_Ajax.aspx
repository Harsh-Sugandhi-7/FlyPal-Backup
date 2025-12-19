<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfAttachmentList_Ajax.aspx.vb"
	Inherits="Flypal.AttachmentListPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>Multiple Attachments</title>
	<script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<link id="MainStyle" type="text/css" rel="stylesheet" />

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

</head>
<body>
	<form id="form1" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
			EnablePageMethods="true">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:msgbox id="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table class="clstablelistout" id="tblmain" width="100%">
			<tr>
				<td>
					<table id="tblInner" class="clstablelistin">
						<tr>
							<td class="clsFormHeader1Newstyle">
								<table width="100%">
									<tr>
										<td>
											<asp:Label ID="lblTitle" runat="server"
												CssClass="clsFormHeader" Text="Multiple Attachments" />
										</td>
										<td align="right">
											<asp:UpdatePanel ID="upnlButton" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
													<asp:Button ID="btnBack" runat="server"
														CssClass="clsbtnH clsinfoH"
														Text="Close" ToolTip="Close screen." />
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
								</table>
							</td>
						</tr>
						<tr>
							<td>
								<asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<asp:ValidationSummary ID="Validationsummary"
											runat="server" HeaderText="Fill Up The Following Fields"
											DisplayMode="BulletList"
											ValidationGroup="a" CssClass="clsValidationSummary" />
									</ContentTemplate>
								</asp:UpdatePanel>
							</td>
						</tr>
						<tr>
							<td>
								<asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<table width="100%">
											<tr>
												<td style="height: 10px"></td>
											</tr>
											<tr>
												<td></td>
												<td>
													<asp:Label ID="lblNo2" runat="server"
														CssClass="clsLabelHeader" Text="No." />
												</td>
												<td>
													<asp:Label ID="lblNo1" runat="server"
														CssClass="clsLabelAuto" />
												</td>
											</tr>
											<tr>
												<td>
													<asp:Label ID="lblRecords" runat="server" Font-Bold="true"/>
												</td>
											</tr>
											<tr>
												<td colspan="3">
													<fieldset class="clsFieldSetNewStyle">
														<asp:UpdatePanel ID="upnlManAttachment" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
																<table width="100%">
																	<tr>
																		<td>
																			<br />
																		</td>
																	</tr>
																	<tr>
																		<td>
																			<asp:GridView ID="dgAttachment" ToolTip="List of Attachment(s)"
																				runat="server" CssClass="clsGridNewStyle" DataKeyNames="ID"
																				ShowHeaderWhenEmpty="true" AllowSorting="True" GridLines="Horizontal"
																				CellPadding="5" AllowPaging="False" AutoGenerateColumns="false">
																				<AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left" />
																				<RowStyle CssClass="clsdgItem" HorizontalAlign="Left" />
																				<HeaderStyle BackColor="White" ForeColor="Black" Font-Bold="True" />
																				<Columns>
																					<asp:BoundField Visible="False" DataField="ID" HeaderText="ID" />
																					<asp:BoundField DataField="FileName" HeaderText="File Name">
																						<HeaderStyle Wrap="False" HorizontalAlign="Left" />
																					</asp:BoundField>
																					<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View"
																						HeaderStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:ImageButton ID="View" runat="server"
																								CommandArgument='<%# Eval("SrNo") %>'
																								CommandName="View" ToolTip="View Attachment."
																								CssClass="FileAttachmentICN"
																								ImageUrl="icons/CLIP01.ICO" />
																						</ItemTemplate>
																						<HeaderStyle HorizontalAlign="Center" />
																						<ItemStyle HorizontalAlign="Center" />
																					</asp:TemplateField>
																				</Columns>
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
									</ContentTemplate>
								</asp:UpdatePanel>
							</td>
						</tr>
					</table>
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
				parent.ParentCallBackFunctionForAttach();
				return false;
			}
		</script>

		<%--Set page layout when open as popup aspx page--%>
		<script type="text/javascript">

			 <% Dim Open As String = Request.QueryString("Type") %>
			 <% If Open IsNot Nothing AndAlso Open = "pup" Then %>  

				$(document).ready(function () {
					SetPageLayout();

					if ($.browser.msie) {
						parent.IFrameAttachStateComplete();
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
	</form>
</body>
</html>
