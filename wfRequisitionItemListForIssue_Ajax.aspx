<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfRequisitionItemListForIssue_Ajax.aspx.vb"
	EnableEventValidation="false" Inherits="Flypal.wfRequisitionItemListForIssue_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<title>List Of Requisition Items</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<link id="MainStyle" type="text/css" rel="stylesheet">
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	<script language="javascript">
		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

		}

		//this function takes a value (ltext) and transmits that to the left hand frame

		function tranRight(ltext) {
			parent.frames(1).document.forms("wfReceive").item("txtReceive").value = ltext;

		}
	</script>
</head>
<body>
	<form id="Form1" method="post" runat="server">
		<asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table id="tblmain" class="clstablelistout">
			<tr>
				<td>
					<asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
						<table id="tblLedgerList" class="clstablelistin">
							<tr>
								<td class="clsFormHeader1Newstyle">
									<span id="lblTitle" class="clsFormHeader" style="display: block; margin-inline-start: 0.5rem;">List Of Requisition Items
									</span>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlReqListDetails" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table>
												<tr>
													<td>
														<table>
															<tr>
																<td>
																	<span id="lblDate" class="clsLabelAuto">Issue Date </span>
																</td>
																<td>
																	<asp:TextBox runat="server" ID="txtDate" CssClass="clsTextBoxTagSearch" Width="100px"
																		AutoPostBack="true" onchange="ValidateDateText(this,'txtDate_watermarkextender');">
																	</asp:TextBox>
																	<cc2:CalendarExtender ID="txtDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																		Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate"></cc2:CalendarExtender>
																	<cc2:TextBoxWatermarkExtender TargetControlID="txtDate" ID="txtDate_watermarkextender"
																		ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																		WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
																</td>
															</tr>
														</table>
													</td>
												</tr>
												<tr>
													<td>
														<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">
                                                        As per criteria :0 Record(s) found.
														</asp:Label>
													</td>
												</tr>
												<tr>
													<td>
														<asp:GridView ID="dgRequisitionList" runat="server" CellPadding="5"
															CssClass="clsGridNewStyle" GridLines="Horizontal" AllowPaging="True"
															ShowHeaderWhenEmpty="true" AutoGenerateColumns="False" ToolTip="List of Requisition(s)"
															PageSize="10" DataKeyNames="ID" EnableViewState="false" AllowSorting="True">
															<AlternatingRowStyle CssClass="clsdgAltItem" />
															<RowStyle CssClass="clsdgItem" />
															<FooterStyle BackColor="#CCCC99" ForeColor="#4d4d4d" />
															<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" />
															<PagerSettings FirstPageText="First" LastPageText="Last" />
															<PagerStyle BackColor="White" CssClass="paging" ForeColor="#4d4d4d" HorizontalAlign="Right" />
															<Columns>
																<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
																<asp:BoundField DataField="RequisitionTextNo" SortExpression="RequisitionTextNo"
																	HeaderText="Requisition No.">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="false" />
																</asp:BoundField>
																<asp:BoundField DataField="ReqTransTypeName" SortExpression="ReqTransTypeName"
																	HeaderText="Requisition Type">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="false" />
																</asp:BoundField>
																<asp:BoundField DataField="DateFormatted" HeaderText="Date">
																	<HeaderStyle Width="100px" HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="false" />
																</asp:BoundField>
																<asp:BoundField DataField="WONo" SortExpression="WONo" HeaderText="WO No.">
																	<HeaderStyle Wrap="false" HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="false" />
																</asp:BoundField>
																<asp:BoundField DataField="LocationName" SortExpression="LocationName"
																	HeaderText="Location">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="false" />
																</asp:BoundField>
																<asp:BoundField DataField="ReqTypeName" SortExpression="ReqTypeName" HeaderText="Type">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="false" />
																</asp:BoundField>
																<asp:ButtonField Text="Select" HeaderText="Select" CommandName="Select">
																	<HeaderStyle Width="100px" HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="false" />
																</asp:ButtonField>
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
								<td>
									<asp:UpdatePanel ID="upnlReqItemListDetails" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table width="100%">
												<tr>
													<td>
														<asp:Label ID="lblResult1" runat="server" CssClass="clsLabelHeader"></asp:Label>
													</td>
												</tr>
												<tr>
													<td>
														<asp:GridView ID="dgItemsList" runat="server" CellPadding="5" CssClass="clsGridNewStyle"
															GridLines="Horizontal" AutoGenerateColumns="False"
															ShowHeaderWhenEmpty="true" EnableViewState="false" ToolTip="List of Spares for W.O. Job"
															AllowSorting="True" PageSize="3">
															<AlternatingRowStyle CssClass="clsdgAltItem" />
															<RowStyle CssClass="clsdgItem" />
															<FooterStyle BackColor="#CCCC99" ForeColor="#4d4d4d" />
															<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" />
															<PagerSettings FirstPageText="First" LastPageText="Last" />
															<PagerStyle BackColor="White" CssClass="paging" ForeColor="#4d4d4d" HorizontalAlign="Right" />
															<Columns>
																<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
																<asp:BoundField Visible="False" DataField="SrNo" HeaderText="Sr.No."></asp:BoundField>
																<asp:BoundField DataField="PartNo" SortExpression="PartNo" HeaderText="Part No.">
																	<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																	<ItemStyle Wrap="False"></ItemStyle>
																</asp:BoundField>
																<asp:BoundField DataField="Description" SortExpression="Description"
																	HeaderText="Description">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																</asp:BoundField>
																<asp:BoundField DataField="RegNo" SortExpression="RegNo" HeaderText="Reg No.">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																</asp:BoundField>
																<asp:BoundField DataField="WorkShopName" SortExpression="WorkShopName"
																	HeaderText="WorkShop">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																</asp:BoundField>
																<asp:BoundField DataField="IssueBalQty" HeaderText="Qty.">
																	<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																	<ItemStyle HorizontalAlign="Right"></ItemStyle>
																</asp:BoundField>
																<asp:BoundField DataField="ReqItemUnitName" SortExpression="ReqItemUnitName"
																	HeaderText="Unit">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																</asp:BoundField>
																<asp:ButtonField Text="Select" HeaderText="Select" CommandName="Select">
																	<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																</asp:ButtonField>
																<asp:BoundField Visible="False" DataField="ReqID" HeaderText="Req  ID"></asp:BoundField>
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
									<asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table align="right">
												<tr>
													<td>
														<asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH1"
															ToolTip="Click To Go Back To Issue List screen"
															Text="Back"></asp:Button>
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

		<%--Date Validations--%>
		<script type="text/javascript">
			//Date validations
			function ValidateDateText(elem, extenderid) {

				var datevalue = $(elem).val();
				var params = { 'Date': datevalue, 'SetDefault': 'true' };
				$.ajax({
					type: "POST",
					url: "DateValidationHandler.ashx",
					cache: false,
					async: false,
					data: params,
					beforeSend: OnBeforeSend,
					success: onSuccess,
					error: onError
				});
				return false;
				function onSuccess(result) {
					$(elem).removeClass('ac_loading');
					$(elem).val(result);
					$find(extenderid).set_Text(result);
				}

				function onError(result) {
					$(elem).removeClass('ac_loading');
					$(elem).val('');
					$find(extenderid).set_Text('');
				}
				function OnBeforeSend() {
					$(elem).addClass('ac_loading');
				}
			}
		</script>
	</form>
</body>
</html>
