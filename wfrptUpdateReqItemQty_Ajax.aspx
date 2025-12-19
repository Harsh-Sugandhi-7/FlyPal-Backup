<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptUpdateReqItemQty_Ajax.aspx.vb"
	EnableEventValidation="false" Inherits="Flypal.wfrptUpdateReqItemQty_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<title>Update Pending Requisition Item Qty.</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	<script id="clientEventHandlersJS" type="text/javascript">

		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

		}
	</script>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
	<form id="wfgroup" method="post" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table id="tblmain" class="clstablelistout" border="0">
			<tr>
				<td>
					<asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
						<table id="tblInner" class="clstablelistin" border="0">
							<tr class="clsFormHeader1Newstyle">
								<td colspan="5">
									<table width="100%">
										<tr>
											<td>
												<span id="lbltitle" class="clsFormHeader" style="display: block; width: 800px;">Remove Requisition Items from  Pending to Enquiry / Quotation / Order / Issue List
												</span>
											</td>
											<td align="right">
												<asp:Button ID="btnUpdate" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Remove Requisition Items from Pending To Issue List"
													Text="Remove" CausesValidation="False"></asp:Button>
												<asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" ToolTip="Click to Close screen"
													Text="Close" CausesValidation="False"></asp:Button>
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td colspan="2">
									<asp:ValidationSummary ID="ValidationSummary" runat="server" CssClass="clsValidationSummary"
										HeaderText="Fill Up The Following Fields" ValidationGroup="a"></asp:ValidationSummary>
									<asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
										ErrorMessage="From Date Required" ControlToValidate="txtFromDate" Display="None"
										ValidationGroup="a"></asp:RequiredFieldValidator>
									<asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtToDate"
										CssClass="clsLabelAuto" Display="None" ErrorMessage="To Date Required" ValidationGroup="a"></asp:RequiredFieldValidator>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel runat="server" ID="upnlSearch" UpdateMode="Conditional">
										<ContentTemplate>
											<table border="0" cellspacing="0">
												<tr>
													<td>
														<span id="lblFromdate" class="clsLabelAuto">From Date</span>
													</td>
													<td>
														<asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagSearchDate" Width="100px"
															onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
														<cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
															Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
														<cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
															ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
															WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
														<asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
															ClientValidationFunction="BetweenDatesValidation" ValidationGroup="a" 
															ErrorMessage="From Date should not be greater than To Date."></asp:CustomValidator>
													</td>
													<td>
														<span id="lblToDate" class="clsLabelAuto">To Date</span>
													</td>
													<td>
														<asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagSearchDate" Width="100px"
															onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
														<cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
															Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
														<cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
															ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
															WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
													</td>
												</tr>
												<tr>
													<td>
														<asp:Label ID="lblReq" runat="server" CssClass="clsLabelAuto">Requisition</asp:Label>
													</td>
													<td>
														<asp:DropDownList ID="cmbRequisitionText" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
															AutoPostBack="True" DataTextField="Text" DataValueField="Text">
														</asp:DropDownList>
													</td>
													<td>
														<asp:Label ID="lblNo" runat="server" CssClass="clsLabelAuto" Width="24px" Visible="False">No.</asp:Label>
													</td>
													<td align="left">
														<asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearch" Visible="False"
															MaxLength="8"></asp:TextBox>
													</td>
												</tr>
												<tr>
													<td>
														<span id="lblPartNo" class="clsLabelAuto">Part Name</span>
													</td>
													<td>
														<asp:TextBox ID="txtSearchFor" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="100"></asp:TextBox>
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
								<td align="right" valign="top">
									<asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png"
										CssClass="clsSearch2btn" ToolTip="Click to Search as per criteria."
										ValidationGroup="a" CausesValidation="true" />
								</td>
							</tr>
							<tr>
								<td>
									<br />
								</td>
							</tr>
							<tr>
								<td colspan="2" align="left">
									<asp:UpdatePanel runat="server" ID="upnlGrid" UpdateMode="Conditional">
										<ContentTemplate>
											<div style="width: 100%; margin-bottom: 3px;">
												<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">List of Parts :</asp:Label>
											</div>
											<div style="width: 100%;">
												<asp:GridView ID="dgReqItemList" ShowHeaderWhenEmpty="true" ClientIDMode="Static"
													runat="server" DataKeyNames="ReqID,ReqItemID" AllowPaging="True"
													AutoGenerateColumns="False" PageSize="10" CssClass="clsGridNewStyle"
													GridLines="Horizontal" CellPadding="5">
													<AlternatingRowStyle CssClass="clsdgAltItem" />
													<RowStyle CssClass="clsdgItem" />
													<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
													<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
													<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
													<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
													<Columns>
														<asp:BoundField Visible="False" DataField="ReqID" HeaderText="ReqID"></asp:BoundField>
														<asp:BoundField Visible="False" DataField="ReqItemID" HeaderText="ReqItemID"></asp:BoundField>
														<asp:TemplateField>
															<ItemTemplate>
																<asp:CheckBox ID="chkSelect" onclick="SetRow(this)" runat="server" ClientIDMode="Static"
																	Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelect") %>'></asp:CheckBox>
															</ItemTemplate>
															<HeaderTemplate>
																<asp:CheckBox ID="chkSelectAll" ClientIDMode="Static" runat="server"></asp:CheckBox>
															</HeaderTemplate>
															<ItemStyle HorizontalAlign="Center" />
															<HeaderStyle HorizontalAlign="Center" />
														</asp:TemplateField>
														<asp:BoundField DataField="RequisitionNo" SortExpression="RequisitionNo" HeaderText="Requisition No.">
															<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															<ItemStyle></ItemStyle>
														</asp:BoundField>
														<asp:BoundField DataField="DateFormatted" HeaderText="Req. Date">
															<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															<ItemStyle Wrap="False"></ItemStyle>
														</asp:BoundField>
														<asp:BoundField DataField="PartNo" SortExpression="PartNo" HeaderText="Part No.">
															<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
														</asp:BoundField>
														<asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
															<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
															<ItemStyle HorizontalAlign="Left"></ItemStyle>
														</asp:BoundField>
														<asp:BoundField DataField="RequestedQty" SortExpression="RequestedQty" HeaderText="Requested Qty">
															<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
															<ItemStyle HorizontalAlign="Right"></ItemStyle>
														</asp:BoundField>
														<asp:BoundField DataField="PendingQty" SortExpression="PendingQty" HeaderText="Pending Qty">
															<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
															<ItemStyle HorizontalAlign="Right"></ItemStyle>
														</asp:BoundField>
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
		<asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
			<ProgressTemplate>
				<div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed; background-color: #000000; top: 0; z-index: 99999;">
				</div>
				<div style="position: fixed; top: 50%; left: 50%; margin-left: -27px; margin-top: -27px; z-index: 100000;">
					<div class="ext-el-mask-msg x-mask-loading">
						<div class="clsLoad_ajax">
							<asp:Image ID="Image1" runat="server" ImageUrl="~/images/Loader.gif" ImageAlign="Middle"
								Height="48px" Width="48px" />
						</div>
					</div>
				</div>
			</ProgressTemplate>
		</asp:UpdateProgress>
		<%--Date Validations--%>
		<script type="text/javascript">

			//From Date - To Date validation
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
	<script type="text/javascript">
		Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
			$("#chkSelectAll").live("click", function () {
				var status = $("#chkSelectAll").attr("checked");
				$("#dgReqItemList tr:gt(0)").find(":checkbox").each(function () {
					if (status == "checked") {
						$(this).attr("checked", status);
						SetRow($(this));
					}
					else {
						$(this).removeAttr("checked");
						SetRow($(this));
					}

				});
			});
		});

		function SetRow(elem) {
			var status = $(elem).attr("checked");
			if (status == "checked") {
				$(elem).closest("tr").addClass('HighLightRow');
			}
			else {
				$(elem).closest("tr").removeClass('HighLightRow');
			}
		}

		function pageLoad() {
			var status;
			$("#dgReqItemList tr:gt(0)").find(":checkbox").each(function () {
				status = $(this).attr("checked");
				if (status == "checked") {
					SetRow($(this));
				}
				else {
					//$(this).removeAttr("checked");
					SetRow($(this));
				}

			});

		}
	</script>
</body>
</html>
