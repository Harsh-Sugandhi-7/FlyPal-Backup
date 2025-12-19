<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPrimaryModel_Ajax.aspx.vb"
	Inherits="Flypal.wfPrimaryModel_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<title>Model Information</title>
	<link id="MainStyle" type="text/css" rel="stylesheet">
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
</head>
<body>
	<form id="wfgroup" method="post" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:msgbox id="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<div>
			<table class="clstablelistout" id="tblmain">
				<tr>
					<td>
						<asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
							<asp:UpdatePanel ID="upnlPrimaryModel" runat="server" UpdateMode="Conditional">
								<ContentTemplate>
									<table class="clstablelistin" id="tblInner">
										<tr>
											<td colspan="2" class="clsFormHeader1Newstyle">
												<table width="100%">
													<tr>
														<td>
															<asp:Label ID="lblTitle" CssClass="clsFormHeader displayBlock" runat="server">
                                                                Primary Model Information [New]
															</asp:Label>
														</td>
														<td align="right">
															<asp:Button ID="btnAdd" ValidationGroup="1" runat="server"
																CssClass="clsbtnH clsinfoH"
																Text="New" ToolTip="Click to add new record" CausesValidation="False"></asp:Button>
															<asp:Button ID="btnSave" CssClass="clsbtnH clsinfoH" ValidationGroup="1" runat="server"
																Text="Save" ToolTip="Click to save"></asp:Button>
															<asp:Button ID="btnBack" ValidationGroup="1" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH"
																Text="Close" ToolTip="Click to close" CausesValidation="False"></asp:Button>
														</td>
													</tr>
												</table>
											</td>
										</tr>
										<tr>
											<td colspan="2">
												<asp:ValidationSummary ID="Validationsummary1" ValidationGroup="1" runat="server"
													HeaderText="Fill Up The Following Fields" CssClass="clsValidationSummary"></asp:ValidationSummary>
												<asp:RequiredFieldValidator ID="rfvName" runat="server" ValidationGroup="1" CssClass="clsLabelAuto"
													ErrorMessage="Name Required" ControlToValidate="txtName" Display="None"></asp:RequiredFieldValidator>
											</td>
										</tr>
										<tr>
											<td colspan="2">
												<fieldset id="fdsModelInfo" class="clsFieldSetNewStyle">
													<legend id="lblModel">
														<b>Details </b>
													</legend>
													<table>
														<tr>
															<td align="right">
																<asp:Label ID="lblModelNameStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
															</td>
															<td>
																<asp:Label ID="lblName" runat="server" CssClass="clsLabelAuto">Name</asp:Label>
															</td>
															<td>
																<asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mPrimaryModel.Name %>"
																	ToolTip="Enter Name" MaxLength="50" Width="179px"></asp:TextBox>
															</td>
															<td align="right"></td>
														</tr>

														<tr>
															<td></td>
															<td></td>
															<td>
																<asp:RadioButton ID="rdbFixedWing" CssClass="clsRadioButton" runat="server" Text="Fixed Wing" Checked="true"
																	GroupName="a" />
																<asp:RadioButton ID="rdbRotaryWing" CssClass="clsRadioButton" runat="server" Text="Rotary Wing"
																	GroupName="a" />
															</td>
															<td align="right"></td>
														</tr>
													</table>
												</fieldset>
											</td>
										</tr>
										<tr>
											<td>
												<asp:Label ID="lblSearch" runat="server" CssClass="clsLabelHeader">Model List</asp:Label>
											</td>
											<td align="right"></td>
										</tr>
										<tr>
											<td colspan="2">


												<asp:GridView ID="dgPrimaryModel" runat="server" CellPadding="5" GridLines="Horizontal"
													CssClass="clsGridNewStyle" AllowSorting="True" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
													DataKeyNames="ID">
													<AlternatingRowStyle CssClass="clsdgAltItem" />
													<RowStyle CssClass="clsdgItem" />
													<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
													<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
													<PagerSettings FirstPageText="First" LastPageText="Last" />
													<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />

													<Columns>
														<asp:BoundField DataField="Id" HeaderText="Id" Visible="False"></asp:BoundField>
														<asp:BoundField DataField="Name" HeaderText="Model" SortExpression="Name">
															<HeaderStyle HorizontalAlign="Left" />
														</asp:BoundField>
														<asp:BoundField DataField="RotaryFixedWing" HeaderText="Rotary/Fixed Wing" SortExpression="RotaryFixedWing">
															<HeaderStyle HorizontalAlign="Left" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
															<ItemTemplate>
																<div class="dropdown">
																	<div class="dropdownbtn-content">
																		<table id="T1" class="clsGridNew_Ajax" style="z-index: 7; position: relative;">
																			<tr>
																				<td>
																					<asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container,GridViewRow).RowIndex %>'
																						CommandName="EditView" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
																				</td>
																				<td>
																					<asp:ImageButton ID="DeleteRec" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
																						CommandName="Remove" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
																				</td>
																			</tr>
																		</table>
																	</div>
																	<asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
																		Style="cursor: pointer" />
																</div>
															</ItemTemplate>
															<HeaderStyle HorizontalAlign="Center" />
															<ItemStyle HorizontalAlign="Center" />
														</asp:TemplateField>
													</Columns>
												</asp:GridView>
											</td>
										</tr>
										<tr>
											<td colspan="2" align="right"></td>
										</tr>
									</table>
									<asp:Button ID="htnBtnManufacturer" ValidationGroup="1" ClientIDMode="Static" runat="server"
										Text="..." CausesValidation="False" Style="display: none;"></asp:Button>
								</ContentTemplate>
							</asp:UpdatePanel>
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

		</div>

		<!-- Select Manufacturer popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyManufacturer" Text="TaskCard Tool" ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlManufacturer" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeManufacturer" frameborder="0" height="100%" width="100%" src="JavaScript:''"
				allowtransparency="true" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:modalpopupextender id="mdlPopupManufacturer" runat="server" targetcontrolid="btnDummyManufacturer"
			popupcontrolid="pnlManufacturer" backgroundcssclass="clsModalPopupBG">
		</cc2:modalpopupextender>
		<script type="text/javascript">
			function IFrameManufacturerStateComplete() {
				$("#btnDummyManufacturer").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}

			function OpenManufacturerWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#IframeManufacturer").attr("src", "wfManufacturer_Ajax.aspx?Type=pup");

					if (!$.browser.msie) {
						$("#btnDummyManufacturer").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}

					return false;
				} catch (e) {
					alert(e);
				}

			}
			function ParentCallBackFunctionForManufacturer() {
				var Manufacturerwindow = $find("<%=mdlPopupManufacturer.ClientID %>");
				//close Task Card Tool popup window
				Manufacturerwindow.hide();
				//           release resources
				$("#IframeManufacturer").attr("src", "JavaScript:''");
				//call image button
				$("#htnBtnManufacturer").click();
			}
		</script>
		<!-- End-->
	</form>

	<%--call parent function after completing subroutine..(when page open as popup)--%>

	<%--Set page layout when open as popup aspx page--%>
	<script type="text/javascript">

		 <% Dim mopen As String = Request.QueryString("Type") %>
		 <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

		$(document).ready(function () {
			SetPageLayout();
			if ($.browser.msie) {
				parent.IFramePrimaryModelStateComplete();
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

		function CallParentCallback() {
			parent.ParentCallBackFunctionForPrimaryModel();
			return false;
		}

	</script>
	<%--End--%>
</body>
</html>
