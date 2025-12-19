<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDiscrepancyCorrectiveActionListFromLog.aspx.vb"
	Inherits="Flypal.DiscrepancyCorrectiveActionListFromLog" %>

<!DOCTYPE html>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtlkt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<title>Discrepancy Corrective Action List</title>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />
	
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

	<script language="javascript" type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
	<script src="js/query-1.7.1.js" type="text/javascript"></script>

</head>
<body>
	<form id="form1" runat="server">
		<div>
			<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
				EnablePageMethods="true">
			</asp:ScriptManager>
			<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
					<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
				</ContentTemplate>
			</asp:UpdatePanel>
		</div>
		<div>
			<table class="clstablelistout" id="tblMain" style="width: 1200px !important">
				<tr>
					<td>
						<asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
							<table id="tblInner" class="clstablelistin" width="100%">
								<tr>
									<td class="clsFormHeader1Newstyle">
										<table width="100%">
											<tr>
												<td>
													<%--Added by Harsh on 7th Feb 2024--%>
													<table width="100%">
														<tr>
															<td>
																<asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<asp:Label ID="lblTitle" runat="server" 
																			CssClass="clsFormHeader" Text="Discrepancy List" />
																	</ContentTemplate>
																</asp:UpdatePanel>
															</td>
														</tr>
													</table>
												</td>
												<td align="right">
													<asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
														<ContentTemplate>
															<asp:Button ID="btnAddNew" runat="server" 
																CssClass="clsbtnH clsinfoH" 
																Text="Add New" CausesValidation="False" />
															<asp:Button ID="btnClose" runat="server" 
																CssClass="clsbtnH clsinfoH" 
																Text="Back" CausesValidation="False" />
														</ContentTemplate>
													</asp:UpdatePanel>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td align="left">
										<asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<div>
													<asp:GridView ID="dgSnagCorrectiveActionList"
														runat="server" DataKeyNames="ID"
														ShowHeaderWhenEmpty="True" CellPadding="5"
														AllowSorting="True" AllowPaging="True"
														AutoGenerateColumns="False" PageSize="5"
														CssClass="clsGridNewStyle" GridLines="Horizontal">
														<AlternatingRowStyle CssClass="clsdgAltItem" />
														<RowStyle CssClass="clsdgItem" />
														<HeaderStyle BackColor="white" CssClass="clsdgHeader" 
															Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
														<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
														<PagerSettings Mode="NumericFirstLast" 
															FirstPageText="First" LastPageText="Last" />
														<PagerStyle BackColor="White" CssClass="paging" 
															ForeColor="Black" HorizontalAlign="Right" />
														<Columns>
															<%--0--%>
															<asp:BoundField Visible="False" DataField="ID" HeaderText="ID">
																<HeaderStyle HorizontalAlign="Left" />
															</asp:BoundField>
															<%--1--%>
															<asp:BoundField DataField="DefectNo" 
																SortExpression="DefectReportNo" HeaderText="Discrepancy No.">
																<HeaderStyle HorizontalAlign="Left" />
																<ItemStyle Wrap="False" HorizontalAlign="Left" />
															</asp:BoundField>
															<%--2--%>
															<asp:BoundField DataField="DateOfOccurrenceFormatted"
																HeaderText="Date Of Occurrence">
																<HeaderStyle HorizontalAlign="Left" />
																<ItemStyle Wrap="False" />
															</asp:BoundField>
															<%--3--%>
															<asp:BoundField DataField="LogNoPageNo" 
																SortExpression="LogNo" HeaderText="Log No."
																HtmlEncode="False">
																<HeaderStyle HorizontalAlign="Left" />
																<ItemStyle Wrap="False" />
															</asp:BoundField>
															<%--4--%>
															<asp:BoundField DataField="ATACodeSubATACode" 
																SortExpression="ATACodeSubATACode" HeaderText="ATA">
																<HeaderStyle HorizontalAlign="Left" />
																<ItemStyle Wrap="False" />
															</asp:BoundField>
															<%--5--%>
															<asp:BoundField DataField="MELOrCDLTag" 
																SortExpression="MELOrCDLTag" HeaderText="Category">
																<HeaderStyle HorizontalAlign="Left" />
																<ItemStyle Wrap="False" />
															</asp:BoundField>
															<%--6--%>
															<asp:BoundField DataField="Defect" 
																SortExpression="Defect" HeaderText="Discrepancy" 
																HtmlEncode="False">
																<HeaderStyle HorizontalAlign="Left" />
															</asp:BoundField>
															<%--7--%>
															<asp:BoundField DataField="InvestigationStatusDiscrepancyText" 
																SortExpression="InvestigationStatusDiscrepancyText"
																HeaderText="Status">
																<HeaderStyle HorizontalAlign="Left" />
															</asp:BoundField>
															<%--8--%>
															<asp:BoundField DataField="ItemSequenceNo" 
																SortExpression="ItemSequenceNo"
																HeaderText="Item Sequence No.">
																<HeaderStyle HorizontalAlign="Left" />
																<ItemStyle Wrap="False" Font-Bold="true" />
															</asp:BoundField>
															<%--9--%>
															<asp:BoundField DataField="NextDue" HeaderText="Due" HtmlEncode="false">
																<HeaderStyle HorizontalAlign="Left" />
																<ItemStyle Wrap="False" />
															</asp:BoundField>

															<%--10--%>
															<asp:TemplateField HeaderStyle-HorizontalAlign="Center" 
																HeaderText="Action" ItemStyle-HorizontalAlign="Center">
																<HeaderStyle HorizontalAlign="Center" />
																<ItemStyle HorizontalAlign="Center" />
																<ItemTemplate>
																	<div id="dropDownImg" class="dropdown">
																		<asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" 
																			runat="server" CssClass="clsActionbtn" />
																		<div id="dropdownICN-content" class="dropdownbtn-content">
																			<table id="dropdown-content" class="clsGridNew_Ajax">
																				<tr>
																					<td>
																						<asp:ImageButton ID="editICN" CssClass="actionICNS"
																							runat="server" ToolTip="Edit this record."
																							CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																							CommandName="EditRec" ImageUrl="~/images/edit.png" />
																					</td>
																					<td>
																						<asp:ImageButton ID="deleteICN" CssClass="largerActionICNS" 
																							runat="server" Enabled='<%# not Eval("IsSyncFromCRS")%>'
																							CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
																							ToolTip="Click to Delete record" 
																							Visible='<%#  Eval("TotalTroubleShootCount") = 0 And
																										Not Eval("IsSyncFromCRS") %>'
																							CommandName="DeleteRec" ImageUrl="~/images/delete.png" />
																					</td>
																					<td>
																						<asp:ImageButton ID="printICN" CssClass="hideGridColumn" runat="server"
																							CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
																							ToolTip="Click to Print record"
																							CommandName="PrintRec" ImageUrl="~/images/print.png" />
																					</td>
																					<td>
																						<asp:ImageButton ID="viewICN" 
																							CssClass="FileAttachmentICN" runat="server"
																							CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
																							ToolTip="Click to View Attachment"
																							CommandName="AttachRec" ImageUrl="icons/CLIP01.ICO"
																							Visible='<%#  Eval("IsAttachmentAdded")%>' />
																					</td>
																				</tr>
																			</table>
																		</div>
																	</div>
																</ItemTemplate>
															</asp:TemplateField>
															<%--11--%>
															<asp:TemplateField HeaderText="Troubleshooting" 
																ItemStyle-VerticalAlign="middle" 
																HeaderStyle-HorizontalAlign="Center" 
																ItemStyle-Wrap="false">
																<ItemTemplate>
																	<asp:LinkButton ID="lnkEdit" runat="server" 
																		CausesValidation="false"
																		Text='<%#Eval("TroubleShootWithCount")%>'
																		CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>' 
																		CommandName="TroubleShootRec" />
																</ItemTemplate>
															</asp:TemplateField>
															<%--12--%>
															<asp:BoundField DataField="IsAttachmentAdded" 
																HeaderText="IsAttachmentAdded" 
																HeaderStyle-CssClass="hideGridColumn"
																ItemStyle-CssClass="hideGridColumn" />
															<%--13--%>
															<asp:BoundField DataField="TotalTroubleShootCount" 
																HeaderText="TotalTroubleShootCount"
																HeaderStyle-CssClass="hideGridColumn"
																ItemStyle-CssClass="hideGridColumn" />
														</Columns>
													</asp:GridView>
												</div>
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

			<%--Date Validations--%>
			<script type="text/javascript">

				//From Date -To Date validation
				function BetweenDatesValidation(source, args) {

					args.IsValid = false;
					var fromdate = $("#txtFromDate").val();
					var todate = $("#txtToDate").val();

					if (!todate) {
						rfvToDate.isvalid = false;
						return;
					}

					if (!fromdate) {
						rfvFromDate.isvalid = false;
						return;
					}

					var param = { 'FromDate': fromdate, 'ToDate': todate };
					$.ajax({
						type: "POST",
						url: "BetweenDateValidationHandler.ashx",
						cache: false,
						data: param,
						async: false,
						beforeSend: OnBeforeSnd,
						success: onSuces,
						error: onErr
					});

					function onSuces(result) {

						$get("AjaxLoader").style.visibility = 'hidden';

						if (result == "True") {
							args.IsValid = true;
							return;
						}

					}

					function onErr(result) {

						$get("AjaxLoader").style.visibility = 'hidden';
						source.errormessage = result;

						return;
					}

					function OnBeforeSnd() {
						$get("AjaxLoader").style.visibility = 'visible';
					}

				}

				//Date validations
				function ValidateDateText(elem, extenderid) {

					var datevalue = $(elem).val();
					var params = { 'Date': datevalue, 'SetDefault': 'false' };
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

		</div>

		<script language="JavaScript" type="text/javascript">

			function CallParentDiscrepancy() {
				window.parent.OpenDiscrepancyDetailWindow();
			}

			function CallParentFunctionautoResizeDiscrepanciesReporting() {
				window.parent.autoResizeDiscrepancyReporting();
			}


			function CallParentDiscrepancyTroubleShootWindow() {
				window.parent.OpenDiscrepancyTroubleShootWindow();
			}

			function CallParentFunction() {
				window.parent.autoResizeDeferredDiscrepancies();
			}

			function CallParentFunctionautoResize() {
				window.parent.autoResizeDeferredDiscrepancy();
			}


			function CallParentFunctionForCabinDefect() {

				try {

					console.log("CallParentFunctionForCabinDefect() called from DiscrepancyfromLog page");

					window.parent.OpenCabinDefectDetailWindow();

					console.log("CallParentFunctionForCabinDefect() called from DiscrepancyfromLog page");

				} catch (e) {
					console.error("Error ocuured in CallParentFunctionForCabinDefect(). Refer the Error " + e);
					alert(e);
				}

			}

			function CallParentFunctionautoResizeforCabinDefect() {

				try {

					console.log("CallParentFunctionautoResizeforCabinDefect() called from DiscrepancyfromLog page");

					window.parent.autoResizeCabinDefectList();

					console.log("CallParentFunctionautoResizeforCabinDefect() called from DiscrepancyfromLog");

				} catch (e) {
					console.error("Error ocuured in CallParentFunctionautoResizeforCabinDefect(). Refer the Error " + e);
					alert(e);
				}

			}


			function CallCloseChildPage() {
				window.parent.CloseChildPage();
			}

		</script>

	</form>
</body>
</html>

