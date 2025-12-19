<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfLineMaintenanceOrderList_Ajax.aspx.vb"
	Inherits="Flypal.wfLineMaintenanceOrderList_Ajax" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Src="MSGBox.ascx" TagPrefix="uc2" TagName="MSGBox" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Service Order List</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script type="text/javascript">

        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }
        function viewAttachment() {
            str = "wfFileView.aspx";
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }

    </script>
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
		<div>
			<table class="clstablelistout" id="Table-MaxWidth">
				<tr>
					<td>
						<asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
							<table id="tblInner" class="clstablelistin">
								<tr>
									<td class="clsFormHeader1Newstyle">
										<table width="100%">
											<tr>
												<td>
													<asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
														<ContentTemplate>
															<asp:Label ID="lblLineMaintOrderList" runat="server" CssClass="clsFormHeader">
																List of Service Orders
															</asp:Label>
														</ContentTemplate>
													</asp:UpdatePanel>
												</td>
												<td align="right">
													<asp:UpdatePanel runat="server" ID="upnActionButtons" UpdateMode="Conditional">
														<ContentTemplate>
															<table>
																<tr>
																	<td align="right">
																		<asp:Button ID="btnAddNew" runat="server" 
																			CssClass="clsbtnH clsinfoH" Text="Add New"
																			ToolTip="Click to Add New Service Order"
																			CausesValidation="False" />
																	</td>
																	<td>
																		<asp:Button ID="btnPrint" runat="server" 
																			CssClass="clsbtnH clsinfoH" Text="Print"
																			ToolTip="Click to Print Service Order List" 
																			CausesValidation="False" />
																	</td>
																	<td align="right">
																		<asp:Button ID="btnClose" runat="server" 
																			CssClass="clsbtnH clsinfoH" Text="Close"
																			ToolTip="Click to close List of Service Order screen."
																			CausesValidation="False" />
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
										<asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
											<ContentTemplate>
												<table>
													<tr>
														<td>
															<span id="lblSearch" class="clsLabel">Search</span>
														</td>
														<td>
															<asp:DropDownList ID="cmbSearchCriteria" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
																AutoPostBack="True">
																<asp:ListItem Value="0" Selected="True">All</asp:ListItem>
																<asp:ListItem Value="1">Date</asp:ListItem>
																<asp:ListItem Value="2">Order</asp:ListItem>
																<asp:ListItem Value="3">Aircraft</asp:ListItem>
																<asp:ListItem Value="4">Supplier</asp:ListItem>
																<asp:ListItem Value="5">Quotation No.</asp:ListItem>
																<asp:ListItem Value="6">Status</asp:ListItem>
															</asp:DropDownList>
														</td>
														<td>
															<asp:DropDownList ID="cmbPeriod" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
																Visible="False">
																<asp:ListItem Value="0">(All)</asp:ListItem>
																<asp:ListItem Value="1">Last 1 Week</asp:ListItem>
																<asp:ListItem Value="2">Last 1 Month</asp:ListItem>
																<asp:ListItem Value="3">Last 1 Quarter</asp:ListItem>
																<asp:ListItem Value="4">Last 1 Year</asp:ListItem>
																<asp:ListItem Value="5">Current Financial Year</asp:ListItem>
																<asp:ListItem Value="6">Between Dates</asp:ListItem>
															</asp:DropDownList>
															<asp:DropDownList ID="cmbStatus" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Visible="False">
																<asp:ListItem Value="0">(All)</asp:ListItem>
																<asp:ListItem Value="1">Opened</asp:ListItem>
																<asp:ListItem Value="2">Authorized</asp:ListItem>
																<asp:ListItem Value="4">Canceled</asp:ListItem>
															</asp:DropDownList>
															<asp:DropDownList ID="cmbOrderText" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
																Visible="False" DataValueField="Text" DataTextField="Text">
															</asp:DropDownList>
															<asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" Visible="False"
																MaxLength="100"></asp:TextBox>
														</td>
														<td>
															<asp:Label ID="lblNo" runat="server" CssClass="clsLabelAuto" Visible="False">No.</asp:Label>
														</td>
														<td align="left">
															<asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearch" Visible="False"
																MaxLength="6"></asp:TextBox>
														</td>
														<td align="left"></td>
														<td align="right">
															<asp:Label ID="lblFromDate" CssClass="clsLabel" runat="server" Visible="False">From Date </asp:Label>
														</td>
														<td>
															<asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagSearch" Width="100px"
																onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
															<cc2:calendarextender id="txtFromDate_CalendarExtender" runat="server" cssclass="cal_Theme1"
																enabled="true" format="<%$AppSettings:DateFormat%>" targetcontrolid="txtFromDate">
															</cc2:calendarextender>
															<cc2:textboxwatermarkextender targetcontrolid="txtFromDate" id="FromDate_watermarkextender"
																clientidmode="Static" runat="server" watermarktext="<%$AppSettings:DateFormat%>"
																watermarkcssclass="clsDateTextBox">
															</cc2:textboxwatermarkextender>
														</td>
														<td align="right">
															<asp:Label ID="lblToDate" CssClass="clsLabel" runat="server" Visible="False">To Date </asp:Label>
														</td>
														<td>
															<asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagSearch" Width="100px"
																onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
															<cc2:calendarextender id="txtToDate_CalendarExtender1" runat="server" cssclass="cal_Theme1"
																enabled="true" format="<%$AppSettings:DateFormat%>" targetcontrolid="txtToDate">
															</cc2:calendarextender>
															<cc2:textboxwatermarkextender targetcontrolid="txtToDate" id="ToDate_watermarkextender"
																clientidmode="Static" runat="server" watermarktext="<%$AppSettings:DateFormat%>"
																watermarkcssclass="clsDateTextBox">
															</cc2:textboxwatermarkextender>
														</td>
													</tr>
												</table>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td align="right">
										<asp:UpdatePanel runat="server" ID="upnlFindNow" UpdateMode="Conditional">
											<ContentTemplate>
												<table width="100%">
													<tr>
														<td align="left">
															<asp:Label ID="lblResult" runat="server" CssClass="clsLabelAuto" Font-Bold="True">
																List of order as per criteria : Record(s) found
															</asp:Label>
														</td>
														<td align="right">
															<asp:ImageButton ID="btnSearch" runat="server" 
																ImageUrl="~/images/Search2.png"
																ToolTip="Search as per Criteria."
																CausesValidation="false" class="clsSearch2btn" />
														</td>
													</tr>
												</table>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td>
										<asp:UpdatePanel runat="server" ID="upnlGridView" UpdateMode="Conditional">
											<ContentTemplate>
												<table width="100%">
													<tr>
														<td align="right">
															<asp:GridView ID="dgOrderList" runat="server" AllowPaging="True" AllowSorting="True"
																AutoGenerateColumns="False" PageSize="10" ShowHeaderWhenEmpty="True"
																CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
																<AlternatingRowStyle CssClass="clsdgAltItem" />
																<RowStyle CssClass="clsdgItem" />
																<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" 
                                                                    HorizontalAlign="Left" />
																<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
																<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
																<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
																<Columns>
																	<%-- 0 --%>
																	<asp:BoundField DataField="ID" HeaderText="ID" Visible="False">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<%-- 1 --%>
																	<asp:BoundField DataField="OrderDateFormatted" HeaderText="Date">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<%-- 2 --%>
																	<asp:BoundField DataField="OrderNo" HeaderText="Number" SortExpression="OrderNo">
																		<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<%-- 3 --%>
																	<asp:BoundField DataField="MachineName" HeaderText="Aircraft" SortExpression="MachineName">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<%-- 4 --%>
																	<asp:BoundField DataField="VendorName" HeaderText="Supplier" SortExpression="VendorName">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Left" />
																	</asp:BoundField>
																	<%-- 5 --%>
																	<asp:BoundField DataField="KindAttn" HeaderText="Kind Attention" SortExpression="KindAttn">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<%-- 6 --%>
																	<asp:BoundField DataField="QuotationNo" HeaderText="Quotation No" SortExpression="QuotationNo">
																		<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<%-- 7 --%>
																	<asp:BoundField DataField="QuotationDateFormatted" HeaderText="Quotation Date">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<%-- 8 --%>
																	<asp:BoundField DataField="CGrandTotal" HeaderText="Grand Total" SortExpression="CGrandTotal">
																		<HeaderStyle HorizontalAlign="Right" />
																		<ItemStyle HorizontalAlign="Right" Wrap="False" />
																	</asp:BoundField>
																	<%-- 9 --%>
																	<asp:BoundField DataField="CurrencyName" HeaderText="Currency" SortExpression="CurrencyName">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<%-- 10 --%>
																	<asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<%-- 11 --%>
																	<asp:BoundField DataField="UserName" HeaderText="Created By" SortExpression="UserName">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<%-- 12 --%>
																	<asp:BoundField DataField="AuthorizedBy" HeaderText="Authorized By" SortExpression="AuthorizedBy">
																		<HeaderStyle HorizontalAlign="Left" />
																		<ItemStyle HorizontalAlign="Left" Wrap="False" />
																	</asp:BoundField>
																	<%-- 13 --%>
																	<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" 
																		ItemStyle-HorizontalAlign="Center">
																		<HeaderStyle HorizontalAlign="Center" />
																		<ItemStyle HorizontalAlign="Center" />
																		<ItemTemplate>
																			<div id="dropDownImg" class="dropdown">
																				<asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server"
																					CssClass="clsActionbtn" />
																				<div id="dropdownICN-content" class="dropdownbtn-content">
                                                                                    <table id="dropdown-content" class="clsGridNew_Ajax">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:ImageButton ID="editICN" CssClass="actionICNS" runat="server"
                                                                                                    CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                                    ToolTip="Edit record." CausesValidation="false"
                                                                                                    CommandName="EditView" ImageUrl="~/images/edit.png" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:ImageButton ID="deleteICN" CssClass="actionICNS  largerActionICNS"
                                                                                                    runat="server" ToolTip="Delete record." CausesValidation="false"
                                                                                                    CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                                    CommandName="DeleteRecord" ImageUrl="~/images/delete.png" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:ImageButton ID="viewICN" class="attachmentICNS" runat="server"
                                                                                                    CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                                                                    ToolTip="Open / Download the added Attachment." CommandName="View" 
                                                                                                    ImageUrl="icons/CLIP01.ICO" CausesValidation="false"
                                                                                                    Visible='<%# Eval("IsAttachmentAdded") %>'/>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
																				</div>
																			</div>
																		</ItemTemplate>
																	</asp:TemplateField>
																</Columns>
															</asp:GridView>
														</td>
													</tr>
													<tr>
														<td></td>
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

		</div>
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
