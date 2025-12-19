<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfnWOJobSpare_AJAX.aspx.vb"
	EnableEventValidation="false" Inherits="Flypal.wfnWOJobSpare_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>Job Spare Detail</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
	<link rel="stylesheet" type="text/css" href="popup.css" />
	<script language="javascript">
		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

		}
	</script>
	<meta name="vs_showGrid" content="True" />
	<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
	<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1" />
	<meta name="vs_defaultClientScript" content="JavaScript" />
	<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
	<link id="MainStyle" rel="stylesheet" type="text/css" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	<script id="clientEventHandlersJS" language="javascript">
		function openTranDetail() {
			str = "wfReports.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openTranDetail1() {
			str = "webform1.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openFile() {
			str = "wfFileView.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openDetail() {
			str = "wfDetail.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');

		}
	</script>
</head>
<body bottommargin="5" leftmargin="5" rightmargin="5" topmargin="5" ms_positioning="GridLayout"
	style="font-size: small">
	<form id="wfgroup" method="post" runat="server">
		<asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server" ScriptMode="Release">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table class="clstablelistout" id="tblmain" cellspacing="1" cellpadding="1" border="0">
			<tr>
				<td>
					<table class="clstablelistin" id="InnerTable" border="0">
						<tr>
							<td colspan="4">
								<asp:UpdatePanel ID="upnlSpareValidationSummary" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<tr>
											<td colspan="4" class="clsFormHeader1Newstyle">
												<table width="100%">
													<tr>
														<td>
															<asp:Label ID="Label4" runat="server" CssClass="clsFormHeader">JOB Spare Details</asp:Label>
														</td>
														<td align="right">
															<asp:UpdatePanel ID="upnlAddButtons" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<table id="Table9" cellspacing="0">
																		<tr>
																			<td>
																				<asp:Button ID="btnAdd" runat="server" CssClass="clsbtnH clsinfoH" Text="Add" ToolTip="Click to Add New Job Spare"
																					CausesValidation="true" ValidationGroup="b" BorderStyle="Solid" Enabled="<%# mnWO.WOStatusID <> 3 %>"></asp:Button>
																			</td>
																			<td>
																				<asp:Button ID="btnClose" runat="server" CssClass="clsbtnH clsinfoH" Text="Close" ToolTip="Click to close Job Spare screen"
																					CausesValidation="False" Visible="True"></asp:Button>
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
											<td colspan="4">
												<asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"
													ValidationGroup="b"></asp:ValidationSummary>
												<asp:RequiredFieldValidator ID="rfvDesc" runat="server" CssClass="clsValidationSummary"
													ControlToValidate="txtSpareDesc" ErrorMessage="Description Required" Display="None"
													ValidationGroup="b"></asp:RequiredFieldValidator>
												<asp:CustomValidator ID="cvItemList" runat="server" CssClass="clsLabelAuto" ControlToValidate="cmbItemList"
													ClientIDMode="Static" Display="None" ClientValidationFunction="validateItem"
													ErrorMessage="Select the Part name from the list" ValidationGroup="b"></asp:CustomValidator>
												<asp:RequiredFieldValidator ID="rfvQty" runat="server" CssClass="clsValidationSummary"
													ControlToValidate="txtReqQty" ErrorMessage="Qty  Required" Display="None" ValidationGroup="b"></asp:RequiredFieldValidator>
												<asp:CustomValidator ID="cvDescription" runat="server" ControlToValidate="txtSpareDesc"
													ErrorMessage="Description must not be greater than 200 characters." Display="None"
													OnServerValidate="customvalidate" ValidationGroup="b"></asp:CustomValidator>
											</td>
										</tr>
									</ContentTemplate>
								</asp:UpdatePanel>
								<script type="text/javascript">
									function validateItem(source, args) {
										args.IsValid = false;

										var dd = $get("cmbItemList");
										if (dd.selectedIndex != 0) {
											args.IsValid = true;
											return;
										}

									}
								</script>
							</td>
						</tr>
						<tr style="display: none">
							<td style="height: 24px" align="right"></td>
							<td style="height: 24px">
								<asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<asp:Label ID="lblWOLabel" runat="server" CssClass="clsLabelAuto">W. O. # </asp:Label>
									</ContentTemplate>
								</asp:UpdatePanel>
							</td>
							<td colspan="2">
								<asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<asp:Label ID="lblWO" runat="server" CssClass="clsLabelAuto"></asp:Label>
									</ContentTemplate>
								</asp:UpdatePanel>
							</td>
						</tr>
						<tr style="display: none">
							<td align="right"></td>
							<td>
								<asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<asp:Label ID="lblJob" runat="server" CssClass="clsLabelAuto">Job # </asp:Label>
									</ContentTemplate>
								</asp:UpdatePanel>
							</td>
							<td colspan="2">
								<asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<asp:Label ID="lblJobLabel" runat="server" CssClass="clsLabel"></asp:Label>
									</ContentTemplate>
								</asp:UpdatePanel>
							</td>
						</tr>

						<tr>

							<td colspan="4">
								<fieldset class="clsFieldSetNewStyle" style="border-width: 1px;">
									<legend>
										<asp:Label ID="lblJobDescription" runat="server" Text="Job Description" CssClass="clsLabelHeader"></asp:Label>
									</legend>
									<asp:TextBox ID="txtJobDescription" runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
										TextMode="MultiLine" Text="<%# mnWOJob.WOJobDescription %>" ToolTip="Job Description"
										ReadOnly="True" BackColor="#E0E0E0"></asp:TextBox>
								</fieldset>
							</td>
						</tr>
						<asp:PlaceHolder ID="InspKit" runat="server" Visible="false">
							<tr>
								<td align="right" colspan="4">
									<asp:UpdatePanel ID="unplInspKit" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:LinkButton ID="lnkSparesfromInspKit" CssClass="clsLinkButton" Text="Select Spares From Inspection Kit"
												runat="server"></asp:LinkButton>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
						</asp:PlaceHolder>
						<tr>
							<td colspan="4">
								<asp:UpdatePanel ID="upnlPart" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<table width="100%">											
											<tr>
												<td>
													<asp:Label ID="Label8" runat="server" CssClass="clsLabelStar">*</asp:Label>
												</td>
												<td>
													<asp:Label ID="lblPlaceName" runat="server" CssClass="clsLabelAuto">Part No.</asp:Label>

												</td>
												<td>
													<asp:UpdatePanel ID="OuterUpdatePanel" runat="server">
														<ContentTemplate>
															<table>
																<tr>
																	<td>
																		<asp:DropDownList ID="cmbItemList" runat="server" CssClass="clsTextBoxTagSearchComboSmall" DataTextField="Name"
																			DataValueField="ID" AutoPostBack="True" Enabled="<%# mnWO.WOStatusID <> 3 %>">
																		</asp:DropDownList>
																	</td>

																</tr>
															</table>

														</ContentTemplate>
														<Triggers>
															<asp:AsyncPostBackTrigger ControlID="cmbItemList" EventName="SelectedIndexChanged" />
														</Triggers>
													</asp:UpdatePanel>
												</td>
												<td>
													<asp:Panel ID="pnl3" runat="server">
														<asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="images/expand_blue.jpg"
															CausesValidation="False" ToolTip="Click to search Part" Enabled="<%# mnWO.WOStatusID <> 3 %>"></asp:ImageButton>
													</asp:Panel>
												</td>
												<td>
													<asp:CheckBox ID="chkGroundEquipment" runat="server" CssClass="clsLabelAuto" Text="Only Ground Equipment"
														AutoPostBack="True" ToolTip="check to select Only Ground Equipment" Enabled="<%# mnWO.WOStatusID <> 3 %>"
														Visible="false"></asp:CheckBox>&nbsp;
												</td>
											</tr>
											<tr>
												<asp:PlaceHolder ID="PlaceHolder3" runat="server">
													<td colspan="5">
														<asp:Panel ID="pnlInner" runat="server">
															<fieldset class="clsFieldSetNewStyle">
																<legend><b>Spare Search Engine </b></legend>
																<table id="Table7">
																	<tr>
																		<td></td>
																		<td>
																			<table>
																				<tr>
																					<td>
																						<asp:Label ID="lblSearch" runat="server" CssClass="clsLabelAuto">Search</asp:Label>

																					</td>
																					<td>
																						<asp:DropDownList ID="cmbSearch" runat="server" CssClass="clsTextBoxTagSearchComboSmall1" AutoPostBack="True">
																							<asp:ListItem Value="1" Selected="True">Part No.</asp:ListItem>
																							<asp:ListItem Value="2">Description</asp:ListItem>
																						</asp:DropDownList>
																					</td>
																					<td>
																						<asp:Label ID="lblFor" runat="server" CssClass="clsLabelAuto" Visible="true">For</asp:Label>
																					</td>
																					<td>
																						<asp:TextBox ID="txtSearchFor" runat="server" CssClass="clsTextBoxTagSearch" ToolTip="Enter Part No. to search"
																							Visible="true" MaxLength="100"></asp:TextBox>
																					</td>
																					<td>
																						<asp:ImageButton ID="ImgBtnFind" runat="server" ToolTip="Click to search Part as per criteria" CssClass="clsSearch2btn1"
																							CausesValidation="False" ImageUrl="~/images/Search2.png"></asp:ImageButton>
																					</td>
																				</tr>
																			</table>
																		</td>

																	</tr>
																	<tr>
																		<td colspan="2">
																			<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader"></asp:Label>
																		</td>
																	</tr>
																	<tr>
																		<td colspan="2">
																			<asp:GridView ID="dgPartSearch" runat="server" ToolTip="List of Parts as per criteria"
																				AllowSorting="True" AutoGenerateColumns="False" PageSize="5" AllowPaging="True"
																				CssClass="clsGridNewStyleFixedWidth" GridLines="Horizontal" CellPadding="5">
																				<AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
																				<RowStyle CssClass="clsdgItem"></RowStyle>
																				<HeaderStyle BackColor="White" ForeColor="Black" Font-Bold="True" />
																				<Columns>
																					<asp:BoundField DataField="ID" HeaderText="ID">
																						<HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
																						<ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
																					</asp:BoundField>
																					<asp:BoundField DataField="Name" SortExpression="Name" HeaderText="Part Number">
																						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																						<ItemStyle Wrap="False" />
																					</asp:BoundField>
																					<asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
																						<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
																						<ItemStyle Wrap="True" />
																					</asp:BoundField>
																					<asp:ButtonField Text="Select" HeaderText="Select" CommandName="Select"></asp:ButtonField>
																					<asp:ButtonField Text="Part Status" HeaderText="Part Status" CommandName="ShowPartStatus">
																						<HeaderStyle HorizontalAlign="Left" />
																					</asp:ButtonField>
																				</Columns>
																				<PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
																				<PagerStyle CssClass="paging" HorizontalAlign="Right" />
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</fieldset>
														</asp:Panel>
													</td>
												</asp:PlaceHolder>
											</tr>
											<cc2:CollapsiblePanelExtender ID="cpeSearch" runat="Server" TargetControlID="pnlInner"
												SuppressPostBack="true" CollapsedSize="0" Collapsed="true" ExpandControlID="pnl3"
												CollapseControlID="pnl3" AutoCollapse="False" AutoExpand="False" ExpandedImage="images/expand_blue.jpg"
												CollapsedImage="images/collapse_blue.jpg" ExpandDirection="Vertical" />
										</table>
									</ContentTemplate>
								</asp:UpdatePanel>
							</td>
						</tr>
						<tr>
							<td colspan="4">
								<asp:UpdatePanel ID="upnlDesc" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<table id="Table10" cellspacing="0" width="100%">
											<tr>
												<td>
													<asp:Label ID="Label9" runat="server" CssClass="clsLabelStar">*</asp:Label>
												</td>
												<td>
													<asp:Label ID="lblSearchbyDes" runat="server" CssClass="clsLabelAuto">Description</asp:Label>
												</td>
												<td>
													<asp:TextBox ID="txtSpareDesc" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle2"
														TextMode="MultiLine" Text="<%# mnWOJob.WOJobSpares.CurrentItem.Description %>"
														ToolTip="Description" MaxLength="200" Enabled="<%# mnWO.WOStatusID <> 3 %>"></asp:TextBox>
												</td>
											</tr>
											<tr>
												<td>&nbsp;
												</td>
												<td>
													<asp:Label ID="lblReqQty" runat="server" CssClass="clsLabelAuto">Required Qty.</asp:Label>
												</td>

												<td>
													<asp:TextBox ID="txtReqQty" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
														Enabled="<%# mnWO.WOStatusID <> 3 %>" MaxLength="4" TabIndex="30" Text="<%# mnWOJob.WOJobSpares.CurrentItem.RequiredQty %>"
														ToolTip="Enter Required Quantity"></asp:TextBox>
											</tr>
											<asp:PlaceHolder runat="server" ID="phHideControls">
												<tr>
													<td>&nbsp;
													</td>
													<td>
														<span id="lblRate" class="clsLabelAuto">Landing Rate</span>
													</td>
													<td>
														<asp:TextBox ID="txtEffRate" runat="server" AutoPostBack="true" CssClass="clsTextBoxTagSearchRightAlign1"
															MaxLength="12" Text="<%# mnWOJob.WOJobSpares.CurrentItem.EffRate %>"></asp:TextBox>
														<span id="Span1" class="clsLabelAuto">In Base Currency</span>
													</td>
												</tr>
												<tr>
													<td>&nbsp;
													</td>
													<td>
														<span id="lblEstimatedCost" class="clsLabelAuto">Actual Cost</span>
													</td>
													<td>
														<asp:UpdatePanel ID="upnlEstimatedCost" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
																<asp:TextBox ID="txtEstimatedCost" runat="server" CssClass="clsTextBoxTagSearchRightAlign2"
																	MaxLength="12" BackColor="#E0E0E0" ReadOnly="true" Text="<%# mnWOJob.WOJobSpares.CurrentItem.EstimatedCost %>"></asp:TextBox>
															</ContentTemplate>
														</asp:UpdatePanel>
													</td>
												</tr>
												<asp:PlaceHolder runat="server" ID="PlaceHolder1" Visible="false">
													<tr>
														<td></td>
														<td>
															<asp:Label ID="Label11" runat="server" CssClass="clsLabelAuto">Is For Billing</asp:Label>
														</td>
														<td>
															<asp:CheckBox ID="chkIsForBilling" runat="server" Checked="<%# mnWOJob.WOJobSpares.CurrentItem.IsForBilling %>"
																ToolTip="Check if this is for Billing" Enabled="<%# mnWO.WOStatusID <> 3 %>"
																CssClass="clsCheckBox"></asp:CheckBox>
														</td>
													</tr>
												</asp:PlaceHolder>
											</asp:PlaceHolder>

											<tr>
												<td></td>
												<td>
													<asp:Label ID="lblRemark" runat="server" CssClass="clsLabelAuto">Remark</asp:Label>
												</td>
												<td>
													<asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle2" TextMode="MultiLine"
														Text="<%# mnWOJob.WOJobSpares.CurrentItem.Remark %>" ToolTip="Remark"
														MaxLength="500" Enabled="<%# mnWO.WOStatusID <> 3 %>"></asp:TextBox>
												</td>
											</tr>
										</table>
									</ContentTemplate>
								</asp:UpdatePanel>
							</td>
						</tr>
						<tr>
							<td colspan="2">
								<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<asp:Label ID="lblSparelist" runat="server" CssClass="clsLabelHeader">Spare list</asp:Label>
									</ContentTemplate>
								</asp:UpdatePanel>
							</td>
							<td colspan="2" align="right">
								<asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<table id="Table8" cellspacing="0">
											<tr>
												<td>
													<asp:Button ID="btnAddTop" runat="server" CssClass="clsButton_Ajax" Text="Add" ToolTip="Click to Add New Job Spare"
														ValidationGroup="b" Enabled="<%# mnWO.WOStatusID <> 3 %>"></asp:Button>
												</td>
												<td align="right">
													<asp:Button ID="btnCloseTop" runat="server" CssClass="clsButton_Ajax" Text="Close"
														ToolTip="Click to close Job Spare screen" CausesValidation="False"></asp:Button>
												</td>
											</tr>
										</table>
									</ContentTemplate>
								</asp:UpdatePanel>
							</td>
						</tr>
						<tr>
							<td colspan="4">
								<asp:UpdatePanel ID="upnldgJobSpare" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<asp:GridView ID="dgJobSpare" runat="server" CssClass="clsGridNewStyle" ToolTip="List of parts"
											AllowSorting="True" AutoGenerateColumns="False" Width="100%" ShowHeaderWhenEmpty="true" GridLines="Horizontal" CellPadding="5">
											<AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
											<RowStyle CssClass="clsdgItem"></RowStyle>
											<HeaderStyle BackColor="White" ForeColor="Black" Font-Bold="True" />
											<Columns>
												<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
												<asp:BoundField DataField="SrNo" SortExpression="SrNo" HeaderText="Sr. No.">
													<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
												</asp:BoundField>
												<asp:BoundField DataField="PartNo" SortExpression="PartNo" HeaderText="Part No">
													<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
													<ItemStyle Wrap="False"></ItemStyle>
												</asp:BoundField>
												<asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description">
													<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
													<ItemStyle Wrap="True"></ItemStyle>
												</asp:BoundField>
												<asp:BoundField DataField="RequiredQty" HeaderText="Qty.">
													<HeaderStyle Wrap="False" HorizontalAlign="Right"></HeaderStyle>
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
												</asp:BoundField>
												<asp:TemplateField HeaderText="Is For Billing" HeaderStyle-CssClass="hideGridColumn"
													ItemStyle-CssClass="hideGridColumn">
													<ItemTemplate>
														<asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsForBilling") %>'
															Enabled="False"></asp:CheckBox>
													</ItemTemplate>
													<ItemStyle HorizontalAlign="Center" />
												</asp:TemplateField>
												<asp:BoundField DataField="EffRate" HeaderText="Landing Rate" Visible="false">
													<HeaderStyle Wrap="False" HorizontalAlign="Right"></HeaderStyle>
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
												</asp:BoundField>
												<asp:BoundField DataField="EstimatedCost" HeaderText="Actual Cost" Visible="false">
													<HeaderStyle Wrap="False" HorizontalAlign="Right"></HeaderStyle>
													<ItemStyle HorizontalAlign="Right"></ItemStyle>
												</asp:BoundField>
												<asp:BoundField DataField="UnitName" SortExpression="UnitName" HeaderText="Unit">
													<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
													<ItemStyle Wrap="False"></ItemStyle>
												</asp:BoundField>
												<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
													<ItemTemplate>

														<div class="dropdown">
															<div class="dropdownbtn-content">
																<table id="T1" class="clsGridNew_Ajax">
																	<tr>
																		<td>
																			<asp:ImageButton ID="EditView" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
																				CommandName="ViewRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
																		</td>
																		<td>
																			<asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# CType(Container, GridViewRow).RowIndex %>'
																				CommandName="DeleteRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
																		</td>

																	</tr>
																</table>
															</div>

															<asp:Image ID="lnkArrow" ImageUrl="~/images/ArrowUp.png" runat="server" CssClass="clsActionbtn"
																Style="cursor: pointer" />
														</div>
													</ItemTemplate>
													<HeaderStyle HorizontalAlign="Center" />
													<ItemStyle HorizontalAlign="Center" />
												</asp:TemplateField>
											</Columns>
											<SelectedRowStyle CssClass="clsdgHeader" />
										</asp:GridView>
									</ContentTemplate>
								</asp:UpdatePanel>
							</td>
						</tr>
						<tr>
							<td align="right" colspan="4">
								<asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<asp:Button ID="hdnBtnInspKit" runat="server" CausesValidation="False" ClientIDMode="Static"
											Style="display: none;" Text="Add" />
									</ContentTemplate>
								</asp:UpdatePanel>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<%--AJAX- Add UpdateProgress to show loading for Longer Process--%>
		<asp:UpdateProgress ID="AjaxLoader" DisplayAfter="300" DynamicLayout="false" runat="server">
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
		<!-- Select Spares Inspection Kit List popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyInspKit" Text="Insp Kit" ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlInspKit" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeInspKit" frameborder="0" height="100%" width="100%" src="JavaScript:''"
				allowtransparency="true" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupInspKit" runat="server" TargetControlID="btnDummyInspKit"
			PopupControlID="pnlInspKit" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameInspKitStateComplete() {
				$("#btnDummyInspKit").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}

			function OpenInspKitWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#IframeInspKit").attr("src", "wfSparesInspectionKitList_Ajax.aspx?Type=pup");

					if (!$.browser.msie) {
						$("#btnDummyInspKit").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}

					return false;
				} catch (e) {
					alert(e);
				}

			}
			function ParentCallBackFunctionForInspKit() {
				var InspKitwindow = $find("<%=mdlPopupInspKit.ClientID %>");
				//close Removal Reason popup window
				InspKitwindow.hide();
				//           release resources
				$("#IframeInspKit").attr("src", "JavaScript:''");
				//call image button
				$("#hdnBtnInspKit").click();
			}
		</script>
		<!-- End-->
		<%--call parent function after completing subroutine..(when page open as popup)--%>
		<script type="text/javascript">
			function CallParentCallback() {
				parent.ParentCallBackFunctionForJobSpareDetail();
				return false;
			}
			function autoWOJobSpareList() {
				window.parent.autoWOJobSparesList();
			}
			function CallCloseChildPage() {

				window.parent.CloseChildPage();
			}
		</script>
		<%--Set page layout when open as popup aspx page--%>
		<script type="text/javascript">
    <% Dim mopen As String = Request.QueryString("Type") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

			$(document).ready(function () {
				SetPageLayout();
				if ($.browser.msie) {
					parent.IFrameJobSpareDetailStateComplete();
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
		</script>
		<%--End--%>
		<!-- Popup For ShowPartNoStatus -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyShowPartNoStatus" Text="ShowPartNoStatus"
				ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlShowPartNoStatus" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeShowPartNoStatus" frameborder="0" height="100%" width="100%" src="JavaScript:''"
				scrolling="auto" allowtransparency="true"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupShowPartNoStatus" runat="server" TargetControlID="btnDummyShowPartNoStatus"
			PopupControlID="pnlShowPartNoStatus" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function OpenShowPartNoStatusWindow() {
				try {
					$("#IframeShowPartNoStatus").attr("src", "wfrptShowPartNoStatus_Ajax.aspx?Type=FromPurchaseOrder");
					$("#btnDummyShowPartNoStatus").click();

					return false;
				} catch (e) {
					alert(e);
				}

			}
			function ParentCallBackFunctionForShowPartNoStatus() {
				var ShowPartNoStatuswindow = $find("<%=mdlPopupShowPartNoStatus.ClientID %>");
				//close popup window
				ShowPartNoStatuswindow.hide();
				//           release resources
				$("#IframeShowPartNoStatus").attr("src", "JavaScript:''");
				//call image button
				$("#hdnBtnShowPartNoStatus").click();
			}
		</script>
		<!---End-->
	</form>

</body>
</html>
