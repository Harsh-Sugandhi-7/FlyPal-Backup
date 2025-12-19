<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSalesInvoiceTerm_Ajax.aspx.vb"
	Inherits="Flypal.wfSalesInvoiceTerm_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>List of Sales Invoice Terms</title>
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
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<div>
			<table class="clstablelistout" id="tblmain">
				<tr>
					<td>
						<asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
							<table class="clstablelistin" id="tblLedgerList">
								<tr>
									<td class="clsFormHeader1Newstyle">
										<table width="100%">
											<tr>
												<td>
													<span id="lblListSalesInvoice" class="clsFormHeader">
														List of Sales Invoice Terms
													</span>
												</td>
												<td align="right">
													<table align="right">
														<tr>
															<td>
																<asp:Button ID="btnOK" runat="server" 
																	CssClass="clsbtnH clsinfoH" Text="Ok">
																</asp:Button>
															</td>
															<td>
																<asp:Button ID="btnClose" runat="server" 
																	CssClass="clsbtnH clsinfoH" Text="Back" 
																	ToolTip="Click to go back to the previous page">
																</asp:Button>
															</td>
														</tr>
													</table>
												</td>
											</tr>
										</table>

									</td>
								</tr>
								<tr>
									<td align="right">
										<asp:UpdatePanel runat="server" ID="upnlAddNewTerm" UpdateMode="Conditional">
											<ContentTemplate>
												<table>
													<tr>
														<td>
															<span id="lblNewTerm" class="clsLabelAuto">Add New Term : </span>
														</td>
														<td>
															<asp:ImageButton ID="imgbtnTerm" runat="server" 
																ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
																ToolTip="Click to Add New Term" CausesValidation="False"></asp:ImageButton>

														</td>
													</tr>
												</table>
												<asp:Button ID="hdnimgBtnTerm" ClientIDMode="Static" runat="server" Text="..." CausesValidation="False"
													Style="display: none;"></asp:Button>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td>
										<asp:UpdatePanel runat="server" ID="upnlTerm" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:GridView ID="dgTerm" runat="server" AllowPaging="True" AllowSorting="True"
													AutoGenerateColumns="False" CssClass="clsGridNewStyle" 
													GridLines="Horizontal" CellPadding="5" PageSize="25" 
													ShowHeaderWhenEmpty="True">
													<AlternatingRowStyle CssClass="clsdgAltItem" />
													<PagerStyle HorizontalAlign="Right" CssClass="paging" />
													<RowStyle CssClass="clsdgItem" />
													<HeaderStyle CssClass="clsdgHeader" BackColor="White" 
														ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
													<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
													<Columns>
														<%--0--%>
														<asp:TemplateField HeaderText="Select">
															<ItemTemplate>
																<asp:CheckBox ID="chkSelect" runat="server" 
																	Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelected") %>' />
															</ItemTemplate>
															<ItemStyle HorizontalAlign="Center" />
														</asp:TemplateField>
														<%--1--%>
														<asp:BoundField DataField="Terms" HeaderText="Terms">
															<ItemStyle Width="500px" />
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
		</div>

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

		<!-- Term Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyTerm" Text="Dummy Term" ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlPopupTerm" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="iPopupTerm" frameborder="0" allowtransparency="true" height="100%" width="100%"
				src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupTerm" runat="server" TargetControlID="btnDummyTerm"
			PopupControlID="pnlPopupTerm" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>

		<script type="text/javascript">

			function IFrameTermStateComplete() {
				$("#btnDummyTerm").click();
				//            $get("AjaxLoader").style.visibility = "hidden";
			}
			$(document).ready(function () {
				$("#imgbtnTerm").live("click", function () {
					try {
						//                    $get("AjaxLoader").style.visibility = "visible";
						$("#iPopupTerm").attr("src", "wfTerm_Ajax.aspx?Type=pup&OpenFrom=9");
						if (!$.browser.msie) {
							$("#btnDummyTerm").click();
							//                        $get("AjaxLoader").style.visibility = "hidden";
						}

						return false;
					} catch (e) {
						alert(e);
					}
				});
			});

		</script>

		<script type="text/javascript">

			function ParentCallBackFunctionForTerm() {
				var TermWindow = $find("<%=mdlPopupTerm.ClientID %>");
				//close Term popup window
				TermWindow.hide();
				$("#iPopupTerm").attr("src", "JavaScript:''");
				//call ata image button
				$("#hdnimgBtnTerm").click();
			}

		</script>
		<!-- End-->

		<%--call parent function after completing subroutine..(when page open as popup)--%>
		<script type="text/javascript">

			function CallParentCallback() {
				parent.ParentCallBackFunctionForSalesInvoiceTerm();
				return false;
			}

		</script>
		<%--End--%>

		<%--Set page layout when open as popup aspx page--%>
		<script type="text/javascript">

			<% Dim mopen As String = Request.QueryString("Typepup") %>

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

				<% Dim openAs As String = Request.QueryString("Typepup") %>

				<% If Not openAs Is Nothing AndAlso openAs = "pup" Then %>

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

	</form>
</body>
</html>
