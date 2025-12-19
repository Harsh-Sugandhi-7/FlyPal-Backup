<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMELSnagCorrectiveActionListNew_AJAX.aspx.vb"
	Inherits="Flypal.wfMELSnagCorrectiveActionListNew_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtlkt" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<title>MEL / Snag Corrective Action List</title>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	<script language="javascript" type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
	<script src="js/query-1.7.1.js" type="text/javascript"></script>
	<script language="javascript" type="text/javascript" id="clientEventHandlersJS">
		function openLedgerSame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
		}

		function openTranDetail() {
			str = "wfReports.aspx"
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}

		function openFile() {
			str = "wfFileView.aspx"
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openDetail() {
			str = "wfDetail.aspx"
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
	</script>

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
			<table class="clstablelistout" id="tblMain">
				<tr>
					<td>
						<asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
							<table id="tblInner" class="clstablelistin" width="100%">
								<tr>
									<td colspan="2">
										<table width="100%">
											<tr>
												<td colspan="2" class="clsFormHeader1Newstyle">
													<%--Added by Harsh on 7th Feb 2024--%>
													<table width="100%">
														<tr>
															<td>
																<asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader" Text='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "ADD / Defect Corrective Action List", "MEL / Snag Corrective Action List") %>'></asp:Label>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</td>
															<td colspan="2" align="right">
																<asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<asp:Button ID="btnAddNew" runat="server" CssClass="clsbtnH clsinfoH" ToolTip='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "Click to Add New ADD/Defect", "Click to Add New MEL/Snag") %>'
																			Text="Add New" CausesValidation="False"></asp:Button>
																		<asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" ToolTip='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "Click to close List of ADD/Defect screen", "Click to close List of MEL/Snag screen") %>'
																			Text="Close" CausesValidation="False"></asp:Button>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</td>
														</tr>
													</table>
												</td>
												<td id="tdFavICN" align="center">
													<span id="spFavICN">
														<i id="favICN" runat="server" onclick="fnMarkFavouriteUnFavourite(this)"
															class="fa fa-star fa-spin fa-5x circle-icon"></i>
													</span>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td colspan="2">
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
									<td colspan="2">
										<asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<table width="100%">
													<tr>
														<td width="105px">
															<asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto">Aircraft </asp:Label>
														</td>
														<td style="width: 0px;">
															<asp:DropDownList ID="cmbAircraft" runat="server" 
																CssClass="clsTextBoxTagSearchComboNewstyle" 
																DataValueField="ID" DataTextField="RegNo" 
																AutoPostBack="True">
															</asp:DropDownList>
														</td>
														<td></td>
														<td width="70px">
															<asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto">From Date</asp:Label>
														</td>
														<td style="width: 0px;">
															<asp:TextBox runat="server" ID="txtFromDate" CssClass="clsTextBoxTagSearchDate" Width="100px"
																onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
															<ajaxtlkt:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></ajaxtlkt:CalendarExtender>
															<ajaxtlkt:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
																ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																WatermarkCssClass="clsDateTextBox"></ajaxtlkt:TextBoxWatermarkExtender>
															<asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
																ClientValidationFunction="BetweenDatesValidation" ValidationGroup="a" ErrorMessage="From Date should not be greater than To Date "></asp:CustomValidator>
														</td>
														<td></td>
														<td width="60px">
															<asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto">To Date</asp:Label>
														</td>
														<td>
															<asp:TextBox runat="server" ID="txtToDate" CssClass="clsTextBoxTagSearchDate" Width="100px"
																onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
															<ajaxtlkt:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
																Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></ajaxtlkt:CalendarExtender>
															<ajaxtlkt:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
																ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
																WatermarkCssClass="clsDateTextBox"></ajaxtlkt:TextBoxWatermarkExtender>
														</td>
														<td>&nbsp;</td>
														<td>&nbsp;</td>
														<td>&nbsp;</td>
														<td>&nbsp;</td>
														<td align="right">
															<asp:ImageButton ID="btnFindNow" runat="server" ImageUrl="~/images/Search2.png"
																ToolTip='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "Click to find list of ADD / Defect as per searching criteria", "Click to find list of MEL / Snag as per searching criteria") %>'
																ValidationGroup="1" CausesValidation="false" class="clsSearch2btn" />
														</td>
													</tr>
												</table>
											</ContentTemplate>
										</asp:UpdatePanel>
										<asp:Label ID="lblInfo" runat="server" CssClass="clsLabelAuto"></asp:Label>
									</td>
								</tr>
								<tr>
									<td valign="top" colspan="2">
										<asp:UpdatePanel runat="server" ID="upnlCollapsiblePnl" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:Panel ID="pnlAdvancedSearch" runat="server" CssClass="clsCollapsePnl">
													<div>
														<div id="divCollapsiblePnl">
															<table width="100%">
																<tr>
																	<td>
																		<span id="lblMastersSelection" class="clsLabelHeader">Advance Search
																		</span>
																	</td>
																	<td align="right">
																		<div id="divCollapsiblePnlImg">
																			<image id="imgMasters" src="images/collapse_blue.jpg"
																				alternatetext="(Show Details...)" />
																		</div>
																	</td>
																</tr>
															</table>
														</div>
													</div>
												</asp:Panel>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td valign="top">
										<asp:UpdatePanel runat="server" ID="upnlAvanceSearchContent" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:Panel ID="pnlAdvancedSearchContent" runat="server">
													<table>
														<tr>
															<td>
																<asp:Label ID="Assembly" runat="server" CssClass="clsLabelAuto">Assembly</asp:Label>
															</td>
															<td>
																<asp:DropDownList ID="cmbAssembly" runat="server" 
																	CssClass="clsTextBoxTagSearchComboNewstyleLong" 
																	DataTextField="ModelSerialNoPostion"
																	DataValueField="AssemblyStatusID"
																	Width="225px">
																</asp:DropDownList>
															</td>
															<td>&nbsp;&nbsp;
															</td>
															<td>
																<asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto">ATA Chapter </asp:Label>
															</td>
															<td>
																<asp:DropDownList ID="cmbATAChapter" runat="server" 
																	CssClass="clsTextBoxTagSearchComboNewstyleLong"
																	DataValueField="ID" DataTextField="ATAChapter"
																	Width="210px">
																</asp:DropDownList>
															</td>
															<td>&nbsp;&nbsp;
															</td>
															<td>
																<asp:Label ID="lblStatus" runat="server" CssClass="clsLabelAuto">Status</asp:Label>
															</td>
															<td>
																<asp:DropDownList ID="cmbStatus" runat="server" CssClass="clsTextBoxTagSearchComboSmall1">
																	<asp:ListItem Value="0">(All)</asp:ListItem>
																	<asp:ListItem Value="2">Open</asp:ListItem>
																	<asp:ListItem Value="1">Closed</asp:ListItem>
																</asp:DropDownList>
															</td>
															<td></td>
															<td>
																<span id="lblIncidentType" class="clsLabelAuto">Incident Type</spanl>
															</td>
															<td>
																<asp:DropDownList ID="cmbIncidentType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
																	DataTextField="Name">
																</asp:DropDownList>
															</td>
														</tr>
														<tr>
															<td>
																<asp:Label ID="Label2" class="clsLabelAuto" runat="server" Text='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "ADD / Defect", "MEL / Snag") %>'></asp:Label>
															</td>
															<td>
																<asp:DropDownList ID="cmbMELSnag" runat="server" CssClass="clsTextBoxTagSearchComboSmall1">
																	<asp:ListItem Value="0">(All)</asp:ListItem>
																	<asp:ListItem Value="1">MEL</asp:ListItem>
																	<asp:ListItem Value="2">Snag</asp:ListItem>
																</asp:DropDownList>
															</td>
															<td></td>
															<td>
																<span id="lblExtensionApplied" class="clsLabelAuto">Extension</spanl>
															</td>
															<td>
																<asp:DropDownList ID="cmbExtensionApplied" runat="server" CssClass="clsTextBoxTagSearchComboSmall1">
																	<asp:ListItem Value="0">(All)</asp:ListItem>
																	<asp:ListItem Value="1">Yes</asp:ListItem>
																	<asp:ListItem Value="2">No</asp:ListItem>
																</asp:DropDownList>
															</td>
															<td></td>
															<td>
																<span id="lblIsInReliability" class="clsLabelAuto">Is In Reliability</spanl>
															</td>
															<td>
																<asp:DropDownList ID="cmbIsInReliability" runat="server" CssClass="clsTextBoxTagSearchComboSmall1">
																	<asp:ListItem Value="0">(All)</asp:ListItem>
																	<asp:ListItem Value="1">Yes</asp:ListItem>
																	<asp:ListItem Value="2">No</asp:ListItem>
																</asp:DropDownList>
															</td>
															<td>&nbsp;
															</td>
															<td>
																<span id="lblDefectType" class="clsLabelAuto">Defect Type</spanl>
															</td>
															<td>
																<asp:DropDownList ID="cmbDefectType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle">
																	<asp:ListItem Value="0">(All)</asp:ListItem>
																	<asp:ListItem Value="1">Pireps</asp:ListItem>
																	<asp:ListItem Value="2">Maintenance Defect</asp:ListItem>
																</asp:DropDownList>
															</td>
														</tr>
													</table>
												</asp:Panel>
												<ajaxtlkt:CollapsiblePanelExtender BehaviorID="clpBehaviour" ID="clpextAdvancedSearch"
													ClientIDMode="Static" runat="Server" TargetControlID="pnlAdvancedSearchContent"
													ExpandControlID="pnlAdvancedSearch" CollapseControlID="pnlAdvancedSearch"
													Collapsed="True" ImageControlID="imgMasters"
													CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
													ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
													SuppressPostBack="false" />
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td></td>
								</tr>
								<tr>
									<td style="height: 43px">
										<asp:UpdatePanel ID="upnlResult" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">
													List of MEL Snag / Defect Corrective Action as per criteria : &nbsp; Record(s) found.</asp:Label>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
									<td align="right">
										<asp:UpdatePanel ID="upnlShowEntriesDDL" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:Label ID="lblShowEntriesDDL" runat="server" Text="Show Entries"></asp:Label>
												<asp:DropDownList CssClass="clsTextBoxTagSearchComboSmall" ID="ddlShowEntries" runat="server" Width="55px"
													AutoPostBack="true" OnSelectedIndexChanged="ddlShowEntriesIndexChanged">
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
								</tr>
								<tr>
									<td colspan="2" align="left">
										<asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<div>
													<asp:GridView ID="dgSnagCorrectiveActionList" runat="server" DataKeyNames="ID"
														ShowHeaderWhenEmpty="True" EnableViewState="False" AllowSorting="True" AllowPaging="True"
														AutoGenerateColumns="False" PageSize="25" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
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
															<asp:BoundField Visible="False" DataField="SerialNo" SortExpression="SerialNo" HeaderText="Sr. No.">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundField>
															<%--2--%>
															<asp:BoundField DataField="DefectNo" SortExpression="DefectReportNo" HeaderText="Defect No.">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																<ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
															</asp:BoundField>
															<%--3--%>
															<asp:BoundField DataField="DateOfOccurrenceFormatted" HeaderText="Date Of Occurrence">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundField>
															<%--4--%>
															<asp:BoundField DataField="LogNoPageNo" SortExpression="LogNo" HeaderText="Log No."
																HtmlEncode="False">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundField>
															<%--5--%>
															<asp:BoundField DataField="PartNoSerialNo" SortExpression="PartNoSerialNo" HeaderText="Component"
																HtmlEncode="False" Visible="False">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundField>
															<%--6--%>
															<asp:BoundField DataField="Defect" SortExpression="Defect" HeaderText="Defect" HtmlEncode="False">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundField>
															<%--7--%>
															<asp:BoundField DataField="Sector" SortExpression="Sector" HeaderText="Sector" Visible="False">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundField>
															<%--8--%>
															<asp:BoundField DataField="MajorMinorTag" SortExpression="MajorMinorTag" HeaderText="Major/Minor">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundField>
															<%--9--%>
															<asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description"
																Visible="False">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundField>
															<%--10--%>
															<asp:BoundField Visible="False" DataField="RegNo" SortExpression="RegNo" HeaderText="Reg No.">
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundField>
															<%--11--%>
															<asp:BoundField Visible="False" DataField="LogDate" HeaderText="Log Date">
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundField>
															<%--12--%>
															<asp:BoundField DataField="FrequencyInDays" SortExpression="FrequencyInDays" HeaderText="Freq. In Days"
																HtmlEncode="False" Visible="False">
																<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																<ItemStyle HorizontalAlign="Right"></ItemStyle>
															</asp:BoundField>
															<%--13--%>
															<asp:BoundField DataField="FrequencyInHours" SortExpression="FrequencyInHours" HeaderText="Freq. In Hours"
																HtmlEncode="False" Visible="False">
																<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																<ItemStyle HorizontalAlign="Right"></ItemStyle>
															</asp:BoundField>
															<%--14--%>
															<asp:BoundField DataField="ExtensionInDays" SortExpression="ExtensionInDays" HeaderText="Exten. In Days"
																HtmlEncode="False" Visible="False">
																<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																<ItemStyle HorizontalAlign="Right"></ItemStyle>
															</asp:BoundField>
															<%--15--%>
															<asp:BoundField DataField="DateTimeOfDue" HeaderText="Due Date" Visible="False">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundField>
															<%--16--%>
															<asp:BoundField DataField="Action" SortExpression="Action" HeaderText="Action">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundField>
															<%--17--%>
															<asp:BoundField DataField="InvestigationStatusText" SortExpression="InvestigationStatusText"
																HeaderText="Investigation Status">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
															</asp:BoundField>
															<%--18--%>
															<asp:BoundField DataField="DateTimeOfRectified" HeaderText="Rectified Date" Visible="False">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundField>
															<%--19--%>
															<asp:BoundField DataField="MELTag" SortExpression="MELTag" HeaderText="Is MEL">
																<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																<ItemStyle Wrap="False"></ItemStyle>
															</asp:BoundField>
															<%--20--%>
															<asp:ButtonField Visible="False" Text="Edit/View" HeaderText="Edit/View" CommandName="EditRec"></asp:ButtonField>
															<%--21--%>
															<asp:ButtonField Visible="False" Text="Delete" HeaderText="Delete" CommandName="DeleteRec"></asp:ButtonField>
															<%--22--%>
															<asp:ButtonField Visible="False" Text="Print" HeaderText="Print" CommandName="PrintRec"></asp:ButtonField>
															<%--23--%>
															<asp:ButtonField Visible="False" CommandName="AttachRec" HeaderText="View" Text="View">
																<HeaderStyle HorizontalAlign="Left" />
																<ItemStyle HorizontalAlign="Left" />
															</asp:ButtonField>
															<%--24--%>
															<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
																<HeaderStyle HorizontalAlign="Center" />
																<ItemStyle HorizontalAlign="Center" />
																<ItemTemplate>
																	<div id="dropDownImg" class="dropdown">
																		<asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn" />
																		<div id="dropdownICN-content" class="dropdownbtn-content">
																			<table id="dropdown-content" class="clsGridNew_Ajax">
																				<tr>
																					<td>
																						<asp:ImageButton ID="editICN" CssClass="actionICNS" runat="server"
																							CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																							ToolTip="Click to Edit record"
																							CommandName="EditRec" ImageUrl="~/images/edit.png" />
																					</td>
																					<td>
																						<asp:ImageButton ID="deleteICN" class="largerActionICNS" runat="server"
																							CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
																							ToolTip="Click to Delete record"
																							CommandName="DeleteRec" ImageUrl="~/images/delete.png" />
																					</td>
																					<td>
																						<asp:ImageButton ID="printICN" class="actionICNS actionICNSAlignment" runat="server"
																							CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
																							ToolTip="Click to Print record"
																							CommandName="PrintRec" ImageUrl="~/images/print.png" />
																					</td>
																					<td>
																						<asp:ImageButton ID="viewICN" class="actionICNS actionICNSAlignment" runat="server"
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
															<asp:BoundField DataField="IsAttachmentAdded" HeaderText="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
																ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
														</Columns>
													</asp:GridView>
												</div>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td colspan="2" align="right">
										<asp:UpdatePanel ID="upnlFavIcnBtn" runat="server" UpdateMode="Conditional">
											<ContentTemplate>
												<table>
													<tr>
														<%--Added by Harsh on 7th Feb 2024--%>
														<td>
															<asp:Button ID="hdnBtnMarkFavourite" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
																Style="display: none;"></asp:Button>
															<asp:Button ID="hdnBtnRemoveFavourite" ClientIDMode="Static" runat="server" Text="----"
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

			<%--Added by Harsh on 7th Feb 2024--%>
			<script type="text/javascript">
				function fnMarkFavouriteUnFavourite(x) {
					if (x.classList.contains("fa-star")) {
						x.classList.remove("fa-star");
						x.classList.add("fa-star-o");
						x.style.color = 'black';
						x.style.border = 'black';
						$("#hdnBtnRemoveFavourite").click();
					}
					else {
						x.classList.remove("fa-star-o");
						x.classList.add("fa-star");
						x.style.color = '#fff';
						x.style.border = 'black';
						$("#hdnBtnMarkFavourite").click();
					}
				}
				function MarkAsFavourite() {
					var redstar = document.getElementById("<%=favICN.ClientID%>");
					redstar.classList.add("fa-star");
					redstar.classList.remove("fa-star-o");
					redstar.style.color = '#fff';
					redstar.style.border = 'black';

				}
				function RemoveFromFavourite() {
					var redstar = document.getElementById("<%=favICN.ClientID%>");
					redstar.classList.add("fa-star-o");
					redstar.classList.remove("fa-star");
					redstar.style.border = 'black';
				}
			</script>
		</div>
	</form>
</body>
</html>
