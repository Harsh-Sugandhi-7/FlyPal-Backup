<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfProjectList_Ajax.aspx.vb" Inherits="Flypal.ProjectListPage" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtlkt" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<title>Project  List</title>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
	
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
			<table class="clstablelistout Table-MaxWidth" id="tblMain">
				<tr>
					<td>
						<asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
							<table id="tblInner" class="clstablelistin" width="100%">
								<tr>
									<td colspan="3">
										<table width="100%">
											<tr>
												<td class="clsFormHeader1Newstyle">
													<table width="100%">
														<tr>
															<td>
																<asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader"
																			Text='<%# IIf(TransTypeID = 101, "Work-Pack List", "Project List") %>' />
																	</ContentTemplate>
																</asp:UpdatePanel>
															</td>
															<td align="right">
																<asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<asp:Button ID="btnAddNewTop" runat="server" CssClass="clsbtnH clsinfoH"
																			ToolTip='<%# IIf(TransTypeID = 101, "Add new Work-Pack.", "Add new Project,") %>'
																			Text="Add New" CausesValidation="False" />
																		<asp:Button ID="btnCloseTop" runat="server" CssClass="clsbtnH clsinfoH"
																			ToolTip='<%# IIf(TransTypeID = 101, "Close Work-Pack List screen.", "Close Project List screen.") %>'
																			Text="Close" CausesValidation="False" />
																	</ContentTemplate>
																</asp:UpdatePanel>
															</td>
														</tr>
													</table>
												</td>
												<td style="width: 1%" align="center">
													<span id="FavClk"><i id="FavIClk" runat="server" onclick="FunctionFav(this)"
														style="font-size: 21px; color: black; border: black; cursor: pointer"
														class="fa fa-star fa-spin fa-5x circle-icon"
														title="Mark As Favourites"></i>
													</span>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td colspan="3">
										<asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
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
										<asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<table width="100%">
													<tr>
														<td>
															<asp:Label ID="lblFromDate" runat="server"
																CssClass="clsLabelAuto" Text="From Date"
																Font-Bold="true" />
														</td>
														<td>
															<asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagSearchDate" Width="100px"
																onchange="ValidateDateText(this,'FromDate_watermarkextender');" autocomplete="off"></asp:TextBox>
															<ajaxtlkt:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></ajaxtlkt:CalendarExtender>
															<ajaxtlkt:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
																ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																WatermarkCssClass="clsDateTextBox"></ajaxtlkt:TextBoxWatermarkExtender>
															<asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
																ClientValidationFunction="BetweenDatesValidation" ValidationGroup="a" ErrorMessage="From Date should not be greater than To Date "></asp:CustomValidator>
														</td>
														<td>
															<asp:Label ID="lblToDate" runat="server"
																CssClass="clsLabelAuto" Text="To Date"
																Font-Bold="true" />
														</td>
														<td>
															<asp:TextBox runat="server" ID="txtToDate"
																CssClass="clsTextBoxTagSearchDate" Width="100px"
																onchange="ValidateDateText(this,'ToDate_watermarkextender');"
																autocomplete="off"></asp:TextBox>
															<ajaxtlkt:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
																Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></ajaxtlkt:CalendarExtender>
															<ajaxtlkt:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
																ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																WatermarkCssClass="clsDateTextBox"></ajaxtlkt:TextBoxWatermarkExtender>
														</td>
														<td>
															<asp:Label ID="lblProjectNo" runat="server" CssClass="clsLabelAuto"
																Text='<%# IIf(TransTypeID = 101, "Work-Pack No", "Project No")%>'
																Font-Bold="true"/>
														</td>
														<td>
															<asp:DropDownList ID="cmbProjectText" runat="server" 
																CssClass="clsTextBoxTagSearchComboNewstyle"
																AutoPostBack="True" DataTextField="Text" 
																DataValueField="Text" />

															<asp:TextBox ID="txtNo" runat="server" 
																CssClass="clsTextBoxTagSearch" Width="40px"
																MaxLength="8" />
														</td>

													</tr>
												</table>
											</ContentTemplate>
										</asp:UpdatePanel>
										<asp:Label ID="lblInfo" runat="server" CssClass="clsLabelAuto"></asp:Label>
									</td>
									<td align="right" colspan="2">
										<asp:ImageButton ID="btnSearchRecords" 
											runat="server" ImageUrl="~/images/Search2.png"
											ValidationGroup="1" 
											CausesValidation="false" 
											class="clsSearch2btn" />
									</td>
								</tr>
								<tr>
									<td>
										<br />
									</td>
								</tr>
								<tr>
									<td colspan="3">
										<span id="Info">
											<b>NOTE :</b>
											<asp:Label ID="lblNote" runat="server" Text='<%# IIf(TransTypeID = 101,
																				"Once the Work Order is Submitted, Work-Pack cannot be Deleted.",
																				"Once the Work Order is Submitted, Project cannot be Deleted.") %>' />
										</span>
									</td>
								</tr>
								<tr>
									<td>
										<asp:UpdatePanel ID="upnlResult" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">
													List of Work-Pack as per criteria : &nbsp; Record(s) found.</asp:Label>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>

									<td align="right">
										<asp:UpdatePanel ID="upnlSearchBox" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<table>
													<tr>
														<td>
															<asp:UpdatePanel ID="upnlShowEntriesDDL" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<asp:Label ID="lblShowEntriesDDL" runat="server" Text="Show Entries"></asp:Label>
																	<asp:DropDownList CssClass="clsTextBoxTagSearchComboSmall"
																		ID="cmbShowE" runat="server" Width="55px"
																		AutoPostBack="true" OnSelectedIndexChanged="ShowNumberOfRecords">
																		<asp:ListItem Value="0">5</asp:ListItem>
																		<asp:ListItem Value="1">10</asp:ListItem>
																		<asp:ListItem Value="2">15</asp:ListItem>
																		<asp:ListItem Value="3">20</asp:ListItem>
																		<asp:ListItem Value="4" Selected="True">25</asp:ListItem>
																		<asp:ListItem Value="5">30</asp:ListItem>
																		<asp:ListItem Value="6">40</asp:ListItem>
																		<asp:ListItem Value="7">45</asp:ListItem>
																		<asp:ListItem Value="8">50</asp:ListItem>
																		<asp:ListItem Value="9">55</asp:ListItem>
																	</asp:DropDownList>
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
														<td>
															<asp:TextBox ID="txtSearchBox" runat="server" 
																CssClass="clsTextBoxTagSearch" 
																placeholder="Search here"
																AutoPostBack="true" />
														</td>
													</tr>
												</table>

											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td colspan="3" align="left">
										<asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<div>
													<asp:GridView ID="dgProjectList" runat="server" DataKeyNames="ID"
														ShowHeaderWhenEmpty="True" AllowSorting="True" AllowPaging="True"
														AutoGenerateColumns="False" PageSize="25" CssClass="clsGridNewStyle"
														GridLines="Horizontal" CellPadding="5">
														<AlternatingRowStyle CssClass="clsdgAltItem" />
														<RowStyle CssClass="clsdgItem" />
														<HeaderStyle BackColor="white" CssClass="clsdgHeader"
															Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
														<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
														<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
														<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
														<Columns>
															<%--0--%>
															<asp:BoundField Visible="False" DataField="ID" HeaderText="ID">
																<HeaderStyle HorizontalAlign="Left" />
															</asp:BoundField>
															<%--1--%>
															<asp:BoundField DataField="ProjectDateFormatted" HeaderText="Date">
																<HeaderStyle HorizontalAlign="Left" />
																<ItemStyle Wrap="False" />
															</asp:BoundField>
															<%--2--%>
															<asp:BoundField DataField="ProjectNumber" SortExpression="ProjectNumber"
																HeaderText="Work-Pack No.">
																<HeaderStyle HorizontalAlign="Left" />
																<ItemStyle Wrap="False" HorizontalAlign="Left" />
															</asp:BoundField>
															<%--3--%>
															<asp:BoundField DataField="CustomerName" SortExpression="CustomerName"
																HeaderText="Customer" HtmlEncode="False">
																<HeaderStyle HorizontalAlign="Left" />
																<ItemStyle Wrap="False" />
															</asp:BoundField>
															<%--4--%>
															<asp:BoundField DataField="Description" SortExpression="Description"
																HeaderText="Description">
																<HeaderStyle HorizontalAlign="Left" />
																<ItemStyle Wrap="true" />
															</asp:BoundField>
															<%--5--%>
															<asp:BoundField DataField="ReceivingDateFormatted" HeaderText="Receiving Date">
																<HeaderStyle HorizontalAlign="Left" />
																<ItemStyle Wrap="False" />
															</asp:BoundField>
															<%--6--%>
															<asp:BoundField DataField="InspectionDateFormatted" HeaderText="Inspection Date"
																HeaderStyle-CssClass="hideGridColumn"
																ItemStyle-CssClass="hideGridColumn">
																<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																<ItemStyle Wrap="False" />
															</asp:BoundField>
															<%--7--%>
															<asp:BoundField DataField="RegNo" HeaderText="Reg No.">
																<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																<ItemStyle Wrap="False" />
															</asp:BoundField>
															<%--8--%>
															<asp:BoundField DataField="ModelName" HeaderText="Model">
																<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																<ItemStyle Wrap="False" />
															</asp:BoundField>
															<%--9--%>
															<asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
																<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																<ItemStyle Wrap="False" />
															</asp:BoundField>
															<%--10--%>
															<asp:BoundField DataField="CustomerContractNo" SortExpression="ContractNo"
																HeaderText="Contract No." HeaderStyle-CssClass="hideGridColumn"
																ItemStyle-CssClass="hideGridColumn">
																<ItemStyle Wrap="False" />
															</asp:BoundField>
															<%--11--%>
															<asp:BoundField DataField="CreatedBy" SortExpression="CreatedBy"
																HeaderText="Created By">
																<HeaderStyle HorizontalAlign="Left" />
															</asp:BoundField>
															<%--12--%>
															<asp:BoundField DataField="StatusName" HeaderText="Status">
																<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																<ItemStyle Wrap="False" />
															</asp:BoundField>
															<%--13--%>
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
																						<asp:ImageButton ID="EditView" runat="server"
																							CommandArgument='<%# Eval("ID") %>'
																							CommandName="EditRec" ImageUrl="~/images/edit.png"
																							CssClass="actionICNS" ToolTip="Edit this record." />
																					</td>
																					<td>
																						<asp:ImageButton ID="deleteICN" CssClass="largerActionICNS"
																							runat="server" CommandArgument='<%# Eval("ID") %>'
																							ToolTip="Delete record" CommandName="DeleteRec"
																							Visible='<%#  Eval("IsAuthorizedSubmittedWorkOrderCount")%>'
																							ImageUrl="~/images/delete.png" />
																					</td>
																					<td>
																						<asp:ImageButton ID="printICN" CssClass="hideGridColumn"
																							runat="server" CommandArgument='<%# Eval("ID") %>'
																							ToolTip="Print Report" Visible="false"
																							CommandName="PrintRec" ImageUrl="~/images/print.png" />
																					</td>
																					<td>
																						<asp:ImageButton ID="viewICN" CssClass="FileAttachmentICN"
																							runat="server" CommandArgument='<%# Eval("ID") %>'
																							ToolTip="View Attachment added"
																							CommandName="AttachRec" ImageUrl="icons/CLIP01.ICO"
																							Visible='<%#  Eval("IsAttachmentAdded")%>' />
																					</td>
																				</tr>
																			</table>
																		</div>
																	</div>
																</ItemTemplate>
															</asp:TemplateField>
															<%--14--%>
															<asp:TemplateField HeaderText="Work-Pack Completion" ItemStyle-Width="100px">
																<ItemTemplate>
																	<div class="progress progress-striped active">
																		<div id="prgbar" runat="server" aria-valuemax="100"
																			aria-valuemin="0" aria-valuenow="33"
																			class="progress-bar progress-bar-striped bg-success"
																			role="progressbar" style="width: 50%">
																			<span id="lblPercentage" runat="server"></span>
																		</div>
																	</div>
																</ItemTemplate>
															</asp:TemplateField>
															<%--15--%>
															<asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded"
																HeaderStyle-CssClass="hideGridColumn"
																ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
															<%--16--%>
															<asp:BoundField DataField="TaskCompletionPercentage" HeaderStyle-CssClass="hideGridColumn"
																HeaderText="TaskCompletionPercentage" ItemStyle-CssClass="hideGridColumn" />
															<%--17--%>
															<asp:BoundField DataField="IsAuthorizedSubmittedWorkOrderCount"
																HeaderText="IsAuthorizedSubmittedWorkOrderCount" HeaderStyle-CssClass="hideGridColumn"
																ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
															<%--18--%>
															<asp:BoundField DataField="TransTypeID" HeaderText="TransTypeID" HeaderStyle-CssClass="hideGridColumn"
																ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
														</Columns>
													</asp:GridView>
												</div>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td colspan="3" align="right">
										<asp:UpdatePanel ID="upnlFavIcnBtn" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<table>
													<tr>
														<td>
															<asp:Button ID="hdnBtnMarkFav" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
																Style="display: none;"></asp:Button>
															<asp:Button ID="hdnBtnRemoveFav" ClientIDMode="Static" runat="server" Text="----"
																CausesValidation="False" Style="display: none;"></asp:Button>
															<asp:Button ID="hdnBtnImportCRSLogs" ClientIDMode="Static" runat="server" Text="----"
																CausesValidation="False" Style="display: none;"></asp:Button>
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
			<asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" ClientIDMode="Static" DynamicLayout="false"
				runat="server">
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

			<script type="text/javascript">
				function FunctionFav(x) {
					if (x.classList.contains("fa-star")) {
						x.classList.remove("fa-star");
						x.classList.add("fa-star-o");
						x.style.color = 'black';
						x.style.border = 'black';
						$("#hdnBtnRemoveFav").click();
					}
					else {
						x.classList.remove("fa-star-o");
						x.classList.add("fa-star");
						x.style.color = '#fff';
						x.style.border = 'black';
						$("#hdnBtnMarkFav").click();
					}
				}
				function MarkFav() {
					var redstar = document.getElementById("<%=FavIClk.ClientID%>"); redstar.classList.add("fa-star");
					redstar.classList.remove("fa-star-o");
					redstar.style.color = '#fff';
					redstar.style.border = 'black';

				}
				function RemoveFav() {
					var redstar = document.getElementById("<%=FavIClk.ClientID%>");
					redstar.classList.add("fa-star-o");
					redstar.classList.remove("fa-star");
					redstar.style.border = 'black';
				}
			</script>
		</div>
	</form>
</body>
</html>
