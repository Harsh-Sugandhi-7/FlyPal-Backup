<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchPartListForSalesInvoice_Ajax.aspx.vb"
	Inherits="Flypal.wfSearchPartListForSalesInvoice_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Part List For Sales Invoice</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
</head>
<body>
	<form id="Form1" method="post" runat="server">
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
						<asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
							<table id="tblLedgerList" class="clstablelistin">
								<tr>
									<td colspan="2" class="clsFormHeader1Newstyle">
										<table width="100%">
											<tr>
												<td>
													<span id="lblPartList" class="clstitle1">Part List For Goods Receipt
													</span>
												</td>
												<td colspan="2" align="right">
													<asp:Button ID="btnClose" TabIndex="0" runat="server"
														CssClass="clsbtnH clsinfoH"
														ToolTip="Click to go back to the previous page"
														Text="Back" CausesValidation="False"></asp:Button>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td>
										<asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
											<ContentTemplate>
												<table>
													<tr>
														<td>
															<span id="lblSearch" class="clsLabel">Part No.</span>
														</td>
														<td>
															<asp:TextBox ID="txtName" runat="server" 
																CssClass="clsTextBoxTagSearch" MaxLength="50"></asp:TextBox>
														</td>
														<td>
															<span id="lblDescription" class="clsLabel">Description</span>
														</td>
														<td>
															<asp:TextBox ID="txtDescription" runat="server" 
																CssClass="clsTextBoxTagSearch" MaxLength="200"></asp:TextBox>
														</td>
													</tr>
												</table>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
									<td valign="top" align="right">
										<table id="tblSearch">
											<tr>
												<td>
													<asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/images/Search2.png"
														ToolTip="Click to find the list of Part as per searching criteria."
														CausesValidation="false" class="clsSearch2btn" />
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td colspan="2" align="left">
										<asp:UpdatePanel runat="server" ID="upnlgrid" UpdateMode="Conditional">
											<ContentTemplate>
												<div style="width: 100%">
													<asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True"></asp:Label>
												</div>
												<div style="width: 100%">
													<asp:GridView ID="gdvItem" EnableViewState="false" runat="server"
														AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" AllowPaging="True"
														PageSize="10" AllowSorting="True" CssClass="clsGridNewStyle" 
														GridLines="Horizontal" CellPadding="5">
														<AlternatingRowStyle CssClass="clsdgAltItem" />
														<RowStyle CssClass="clsdgItem" />
														<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
														<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
														<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
														<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
														<Columns>
															<%--0--%>
															<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
															<%--1--%>
															<asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Part No.">
																<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
															</asp:BoundField>
															<%--2--%>
															<asp:BoundField DataField="Description" SortExpression="Description" 
																HeaderText="Description">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																<ItemStyle HorizontalAlign="Left" />
															</asp:BoundField>
															<%--3--%>
															<asp:BoundField DataField="SerializedYesNo" SortExpression="SerializedYesNo"
																HeaderText="Is Serialized">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																<ItemStyle HorizontalAlign="Left" />
															</asp:BoundField>
															<%--4--%>
															<asp:ButtonField Text="Select" HeaderText="Select" CommandName="SelectRec">
																<HeaderStyle HorizontalAlign="Left" />
																<ItemStyle HorizontalAlign="Left" />
															</asp:ButtonField>
															<%--5--%>
															<asp:BoundField Visible="False" DataField="QtyRemovedFromAircraft"
																HeaderText="QtyRemovedFromAircraft">
															</asp:BoundField>
														</Columns>
													</asp:GridView>
												</div>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
									<!--End-->
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

		</div>
	</form>

</body>
</html>
