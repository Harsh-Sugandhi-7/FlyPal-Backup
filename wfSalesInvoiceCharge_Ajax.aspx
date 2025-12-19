<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSalesInvoiceCharge_Ajax.aspx.vb"
	Inherits="Flypal.wfSalesInvoiceCharge_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>Sales Invoice Charge</title>
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
													<asp:Label ID="lblTitle" CssClass="clsFormHeader" runat="server" Width="250px">Sales Invoice Charge</asp:Label>
												</td>
												<td align="right">
													<asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
														<ContentTemplate>
															<table align="right">
																<tr>
																	<td>
																		<asp:Button ID="btnOK" runat="server" CssClass="clsbtnH clsinfoH" Text="Ok"></asp:Button>
																	</td>
																	<td>
																		<asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to go back to the previous page"
																			CausesValidation="False" Text="Back"></asp:Button>
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
										<asp:UpdatePanel runat="server" ID="upnlValidationSummary" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:ValidationSummary ID="Validationsummary1" runat="server" CssClass="clsValidationSummary"
													HeaderText="Fill Up The Following Information"></asp:ValidationSummary>
												<asp:CustomValidator ID="cvCharge" runat="server" CssClass="clsLabelAuto" OnServerValidate="customvalidate"
													Display="None" ErrorMessage="Charge Name Required" ControlToValidate="cmbCharge"></asp:CustomValidator><asp:CustomValidator
														ID="cvPercentage" runat="server" CssClass="clsLabelAuto" OnServerValidate="customvalidate"
														Display="None" ErrorMessage="Percentage should  be Greater than 0" ControlToValidate="txtPercentage"></asp:CustomValidator><asp:CustomValidator
															ID="cvAmount" runat="server" CssClass="clsLabelAuto" OnServerValidate="customvalidate"
															Display="None" ErrorMessage="Amount should be Greater than 0" ControlToValidate="txtChargeAmount"></asp:CustomValidator>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td>
										<asp:UpdatePanel runat="server" ID="upnlOtherChargeDetails" UpdateMode="Conditional">
											<ContentTemplate>
												<table>
													<tr>
														<td colspan="4">
															<span id="lblOtherChargeDetails" class="clsLabelHeader">Other Charge Details</span>
														</td>
													</tr>
													<tr>
														<td>
															<span id="lblStarCharge" class="clsLabelStar">*</span>
														</td>
														<td>
															<span id="lblChargeName" class="clsLabelAuto">Charge Name</span>
														</td>
														<td>
															<asp:DropDownList ID="cmbCharge" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
																DataTextField="Name" AutoPostBack="True">
															</asp:DropDownList>
														</td>
														<td>
															<asp:ImageButton ID="imgbtnCharge" runat="server" ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
																ToolTip="Click to Add New Charge" CausesValidation="False"></asp:ImageButton>
														</td>
													</tr>
													<tr>
														<td>&nbsp;
														</td>
														<td>
															<span id="lblPercentage" class="clsLabel">Percentage </span>
														</td>
														<td>
															<asp:TextBox ID="txtPercentage" runat="server" CssClass="clsTextBoxTagSearch" Style="text-align: right"
																ToolTip="Enter Percentage" Text="<%# mSalesInvoice.SalesInvoiceCharges.currentItem.Percentage %>"
																MaxLength="12" BackColor="#E0E0E0" ReadOnly="True"></asp:TextBox>
														</td>
														<td>&nbsp;
														</td>
													</tr>
													<tr>
														<td>&nbsp;
														</td>
														<td>
															<span id="lblChargeAmount" class="clsLabelAuto">Charge Amount </span>
														</td>
														<td>
															<asp:TextBox ID="txtChargeAmount" runat="server" CssClass="clsTextBoxTagSearch" Style="text-align: right"
																ToolTip="Enter Charge Amount" Text="<%# mSalesInvoice.SalesInvoiceCharges.CurrentItem.CChargeAmount %>"
																MaxLength="12" BackColor="#E0E0E0" ReadOnly="True"></asp:TextBox>
														</td>
														<td>&nbsp;
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

		<%--call parent function after completing subroutine..(when page open as popup)--%>
		<script type="text/javascript">

			function CallParentCallback() {
				parent.ParentCallBackFunctionForSalesInvoiceCharge();
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
