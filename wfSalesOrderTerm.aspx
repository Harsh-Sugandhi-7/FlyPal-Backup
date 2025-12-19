<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSalesOrderTerm.aspx.vb"
	Inherits="Flypal.wfSalesOrderTerm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head runat="server">
	<title>Sales Order Terms</title>
	<link id="MainStyle" type="text/css" rel="stylesheet" />

	<script language="javascript">
		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
		}

		//this function takes a value (ltext) and transmits that to the left hand frame
		function tranRight(ltext) {
			parent.frames(1).document.forms("wfReceive").item("txtReceive").value = ltext;
		}
	</script>

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

</head>
<body ms_positioning="GridLayout" bottommargin="5" leftmargin="5" topmargin="5" rightmargin="5">
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
					<asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
						<table class="clstablelistin" id="tblLedgerList">
							<tr>
								<td class="clsFormHeader1Newstyle">
									<table width="100%">
										<tr>
											<td>
												<asp:Label ID="lblListSalesOrder" runat="server" CssClass="clsFormHeader">List Of Sales Order Terms</asp:Label>
											</td>
											<td align="right">
												<table class="clstableButton" align="right">
													<tr>
														<td>
															<asp:Button ID="btnOK" runat="server" CssClass="clsbtnH clsinfoH" Text="Ok"></asp:Button>
														</td>
														<td>
															<asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" ToolTip="Click to go back to the previous page"></asp:Button>
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
									<table>
										<tr>
											<td align="right">
												<asp:Label ID="lblNewTerm" runat="server" CssClass="clsLabelAuto">Add New Term : </asp:Label>
											</td>
											<td align="right" colspan="1">
												<asp:ImageButton ID="imgbtnTerm" runat="server" ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
													ToolTip="Click to Add New Term" CausesValidation="False"></asp:ImageButton>

											</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td align="right">
									<asp:DataGrid ID="dgTerm" runat="server" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" PageSize="3" AutoGenerateColumns="False">
										<AlternatingItemStyle CssClass="clsdgAltItem"></AlternatingItemStyle>
										<ItemStyle CssClass="clsdgItem"></ItemStyle>
										<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
										<Columns>
											<asp:TemplateColumn HeaderText="Select">
												<ItemTemplate>
													<asp:CheckBox ID="chkSelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelected") %>'></asp:CheckBox>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:BoundColumn DataField="Terms" HeaderText="Term"></asp:BoundColumn>
										</Columns>
										<PagerStyle NextPageText="Next" PrevPageText="Prev" HorizontalAlign="Right"></PagerStyle>
									</asp:DataGrid>
								</td>
							</tr>
						</table>
					</asp:Panel>
					<asp:Panel ID="pnlMessageBox" runat="server">
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
						$("#iPopupTerm").attr("src", "wfTerm_Ajax.aspx?Type=pup&OpenFrom=11");
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
				parent.ParentCallBackFunctionForTerm();
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

			   <% Dim mopenas As String = Request.QueryString("Typepup") %>
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
	</form>
</body>
</html>
