<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfLineMaintenanceOrderTerm_Ajax.aspx.vb"
	Inherits="Flypal.wfLineMaintenanceOrderTerm_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>List of Service Order Terms</title>
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
										<span id="lblListQuotation" class="clsFormHeader">
											List of Service Order Terms
										</span>
									</td>
								</tr>
								<tr>
									<td align="right">
										<table>
											<tr>
												<td valign="middle">
													<span id="lblNewTerm" class="clsLabelAuto">Add New Term.</span>
												</td>
												<td valign="middle">
													<asp:ImageButton ID="imgbtnTerm" runat="server"
														CssClass="addRecordICN" ToolTip="Add New Term."
														CausesValidation="False" ImageUrl="~/images/plus1.png" />
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td>
										<asp:UpdatePanel runat="server" ID="upnlTerm" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:GridView ID="dgTerm" runat="server" AllowPaging="True"
													AllowSorting="True" AutoGenerateColumns="False"
													PageSize="25" ShowHeaderWhenEmpty="True"
													CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
													<AlternatingRowStyle CssClass="clsdgAltItem" />
													<RowStyle CssClass="clsdgItem" />
													<HeaderStyle BackColor="white" CssClass="clsdgHeader" 
														Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
													<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
													<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
													<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
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
								<tr>
									<td align="right">
										<asp:Button ID="btnOK" runat="server" CssClass="clsbtnH clsinfoH1" Text="Ok" />
									</td>
								</tr>
							</table>
						</asp:Panel>
					</td>
				</tr>
			</table>
		</div>
		<!-- Term Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyTerm" Text="Dummy Term" ClientIDMode="Static" />
			<asp:Button ID="hdnimgBtnTerm" ClientIDMode="Static" runat="server" Text="..." CausesValidation="False"
				Style="display: none;"></asp:Button>
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
			}
			$(document).ready(function () {
				$("#imgbtnTerm").live("click", function () {
					try {
						$("#iPopupTerm").attr("src", "wfTerm_Ajax.aspx?Type=pup&OpenFrom=7");
						if (!$.browser.msie) {
							$("#btnDummyTerm").click();
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
	</form>
</body>
</html>
