<%@ Page Language="vb" AutoEventWireup="false" Codebehind="wfnWOReturnIssueList.aspx.vb" Inherits="Flypal.wfnWOReturnIssueList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxTlkkt" %>

<!DOCTYPE  html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"> 
<html>
<head runat="server">
	<title>Issue List</title>		
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<link id="MainStyle" type="text/css" rel="stylesheet" />

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

	<script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
	<script type="text/javascript">
		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
		}
	</script>

	<style type="text/css">

        #lblTitle{
            display: block;
            min-width: 850px;  
        }

    </style>

</head>
	<body>
		<form id="frmIssueList" method="post" runat="server">
			<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
				EnablePageMethods="true">
			</asp:ScriptManager>

			<table class="clstablelistout" id="tblMain">
				<tr>
					<td>
						<asp:panel id="pnlMain" Runat="server" CssClass="clsPanel1">
							<table id="tblInner" class="clstablelistin">
								<tr>
									<td colspan="2" class="clsFormHeader1Newstyle">
										<table width="100%">
											<tr>
												<td>
													<asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">
														List Of Issue
													</asp:Label>
												</td>
												<td align="right">
													<asp:Button ID="BtnPrint" runat="server" CssClass="clsbtnH clsinfoH"
														ToolTip="Click to print list of Issues"
														Text="Print" CausesValidation="False">
													</asp:Button>
													<asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH"
														ToolTip="Click to close List of Issue screen"
														Text="Close" CausesValidation="False">
													</asp:Button>
												</td>
											</tr>
										</table>
									</td>									
								</tr>
								<tr>
									<td colspan="3">
										<asp:ValidationSummary id="Validationsummary" Runat="server" 
											HeaderText="Fill Up The Following Information" Cssclass="clsValidationSummary">
										</asp:ValidationSummary>
									</td>
								</tr>
								<tr>
									<td colspan="3">
										<table id="Table1">
											<tr>
												<td>
													<asp:Label id="lblSearch" runat="server" CssClass="clsLabel" Width="55px" Height="8px">Search</asp:Label></td>
												<td>
													<asp:dropdownlist id="cmbSearch" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True">
														<asp:ListItem Value="0" Selected="True">All</asp:ListItem>
														<asp:ListItem Value="1">Date</asp:ListItem>
														<asp:ListItem Value="2">Issue</asp:ListItem>
														<asp:ListItem Value="3">Receipt</asp:ListItem>
														<asp:ListItem Value="4">Part Number</asp:ListItem>
														<asp:ListItem Value="5">From Store</asp:ListItem>
														<asp:ListItem Value="6">Supplier</asp:ListItem>
														<asp:ListItem Value="7">Aircraft</asp:ListItem>
														<asp:ListItem Value="9">Release Note No.</asp:ListItem>
														<asp:ListItem Value="10">Serial No.</asp:ListItem>
														<asp:ListItem Value="11">Status</asp:ListItem>
														<asp:ListItem Value="12">WorkShop</asp:ListItem>
														<asp:ListItem Value="13">WorkOrder</asp:ListItem>
													</asp:dropdownlist></td>
												<td>
													<asp:Label id="L1" runat="server" CssClass="clsLabel" Width="20px"></asp:Label></td>
												<td>
													<asp:DropDownList id="cmbDate" runat="server" 
														CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True" Visible="False">
														<asp:ListItem Value="0">(All)</asp:ListItem>
														<asp:ListItem Value="1">Last 1 Week</asp:ListItem>
														<asp:ListItem Value="2">Last 1 Month</asp:ListItem>
														<asp:ListItem Value="3">Last 1 Quarter</asp:ListItem>
														<asp:ListItem Value="4">Last 1 Year</asp:ListItem>
														<asp:ListItem Value="5">Current Financial Year</asp:ListItem>
														<asp:ListItem Value="6">Between Dates</asp:ListItem>
													</asp:DropDownList>
													<asp:DropDownList id="cmbStatus" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Visible="False">
														<asp:ListItem Value="0">(All)</asp:ListItem>
														<asp:ListItem Value="1">Opened</asp:ListItem>
														<asp:ListItem Value="2">Authorized</asp:ListItem>
														<asp:ListItem Value="4">Canceled</asp:ListItem>
													</asp:DropDownList>
													<asp:DropDownList id="cmbToType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Height="24px" Visible="False"
														DataTextField="Text" DataValueField="Text">
														<asp:ListItem Value="0">(All)</asp:ListItem>
														<asp:ListItem Value="1">Supplier</asp:ListItem>
														<asp:ListItem Value="2">Aircraft</asp:ListItem>
														<asp:ListItem Value="8">Store</asp:ListItem>
														<asp:ListItem Value="7">Discard</asp:ListItem>
														<asp:ListItem Value="15">Customer</asp:ListItem>
														<asp:ListItem Value="16">WorkShop</asp:ListItem>
														<asp:ListItem Value="17">WorkOrder</asp:ListItem>
													</asp:DropDownList>
													<asp:DropDownList id="cmbIssueText" runat="server" 
														CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True" Visible="False"
														DataTextField="Text" DataValueField="Text">
													</asp:DropDownList>
													<asp:DropDownList id="cmbReceiptText" runat="server" 
														CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True" Visible="False"
														DataTextField="Text" DataValueField="Text">
													</asp:DropDownList>
													<asp:DropDownList id="cmbWoText" runat="server" 
														CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True" Visible="False"
														DataTextField="WOText" DataValueField="WOText">
													</asp:DropDownList>
													<asp:TextBox id="txtName" runat="server" CssClass="clsTextBoxTagSearch" Visible="False" MaxLength="100"></asp:TextBox>
												</td>

												<td valign="middle">
													<asp:Label id="lblNo" runat="server" CssClass="clsLabel" Width="32px" Height="8px" Visible="False">
														No.
													</asp:Label>
												</td>
												<td>
													<asp:TextBox ID="txtNo" runat="server" CssClass="clsTextBoxTagSearchSmall" 
														Visible="False" MaxLength="10">
													</asp:TextBox>
												</td>
												<td>
													<table>
														<tr>
															<td>
																<asp:Label ID="lblFromDate" runat="server" CssClass="clsLabel" Visible="False">
																	From Date 
																</asp:Label>
															</td>
															<td>
																<asp:TextBox runat="server" ID="FromDate_Txt" CssClass="clsTextBoxTagSearchDate"
																	Width="100px" onchange="ValidateDateText(this,'FromDate_watermarkextender');">
																</asp:TextBox>
																<ajaxTlkkt:CalendarExtender id="txtFromDate_CalendarExtender" runat="server"
																	cssclass="cal_Theme1" enabled="true" format="<%$AppSettings:DateFormat%>"
																	targetcontrolid="FromDate_Txt">
																</ajaxTlkkt:CalendarExtender>
																<ajaxTlkkt:TextBoxWatermarkExtender targetcontrolid="FromDate_Txt"
																	id="FromDate_watermarkextender" clientidmode="Static" runat="server"
																	watermarktext="<%$AppSettings:DateFormat%>"
																	watermarkcssclass="clsDateTextBox">
																</ajaxTlkkt:TextBoxWatermarkExtender>
															</td>
															<td>
																<asp:Label ID="lblToDate" runat="server" CssClass="clsLabel" Visible="False">
																	To Date 
																</asp:Label>
															</td>
															<td>
																<asp:TextBox runat="server" ID="ToDate_Txt" CssClass="clsTextBoxTagSearchDate"
																	Width="100px" onchange="ValidateDateText(this,'ToDate_WatermarkExtender');">
																</asp:TextBox>
																<ajaxTlkkt:CalendarExtender id="txtToDate_CalendarExtender" runat="server"
																	cssclass="cal_Theme1" enabled="true" format="<%$AppSettings:DateFormat%>"
																	targetcontrolid="ToDate_Txt">
																</ajaxTlkkt:CalendarExtender>
																<ajaxTlkkt:TextBoxWatermarkExtender TargetControlID="ToDate_Txt"
																	ID="ToDate_WatermarkExtender" ClientIDMode="Static" runat="server"
																	WatermarkText="<%$AppSettings:DateFormat%>"
																	WatermarkCssClass="clsDateTextBox">
																</ajaxTlkkt:TextBoxWatermarkExtender>
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
										<asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png"
											ToolTip="Click to search as per searching Criteria."
											ValidationGroup="1" CausesValidation="false" class="clsSearch2btn" />										
									</td>
								</tr>
								<tr>
									<td>
										<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">
											List of Issue as per criteria :  Record(s) found.
										</asp:Label>
									</td>
								</tr>
								<tr>
									<td colspan="3">
										<asp:GridView ID="gvIssueList" runat="server" AutoGenerateColumns="False"
											CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" 
											AllowPaging="True"  PageSize="10" ShowHeaderWhenEmpty="true">
											<AlternatingRowStyle CssClass="clsdgAltItem" />
											<RowStyle CssClass="clsdgItem" />
											<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
											<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
											<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
											<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
											<Columns>
												<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
												<asp:BoundField DataField="ILDateFormatted" HeaderText="Date">
													<HeaderStyle Wrap="False"></HeaderStyle>
													<ItemStyle Wrap="False"></ItemStyle>
												</asp:BoundField>
												<asp:BoundField DataField="IssueNo" SortExpression="IssueNo" HeaderText="Number">
													<HeaderStyle Wrap="False"></HeaderStyle>
													<ItemStyle Wrap="False"></ItemStyle>
												</asp:BoundField>
												<asp:BoundField DataField="IssueType" SortExpression="IssueType" HeaderText="Issue Type">
													<HeaderStyle Wrap="False"></HeaderStyle>
												</asp:BoundField>
												<asp:BoundField DataField="StoreName" SortExpression="StoreName" HeaderText="Store">
													<HeaderStyle Wrap="False"></HeaderStyle>
													<ItemStyle Wrap="False"></ItemStyle>
												</asp:BoundField>
												<asp:BoundField DataField="Destination" SortExpression="Destination" HeaderText="Issue To">
													<HeaderStyle Wrap="False"></HeaderStyle>
													<ItemStyle Wrap="False"></ItemStyle>
												</asp:BoundField>
												<asp:BoundField DataField="StatusName" SortExpression="StatusName" HeaderText="Status">
													<HeaderStyle Wrap="False"></HeaderStyle>
												</asp:BoundField>
												<asp:BoundField DataField="UserName" SortExpression="UserName" HeaderText="Created By">
													<HeaderStyle Wrap="False"></HeaderStyle>
												</asp:BoundField>
												<asp:BoundField DataField="AuthorizedByName" SortExpression="AuthorizedByName" HeaderText="Authorized By ">
													<HeaderStyle Wrap="False"></HeaderStyle>
												</asp:BoundField>
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
																			<asp:ImageButton ID="editICN" class="actionICNS" runat="server"
																				CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
																				ToolTip="Click to Edit record"
																				CommandName="EditRecord" ImageUrl="~/images/edit.png" />
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
							</table>
						</asp:panel>
					</td>
				</tr>
			</table>

			<!-- Ajax Loader -->
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

			<script type="text/javascript" id="dateValidation">

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
