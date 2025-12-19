<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfRole_Ajax.aspx.vb" Inherits="Flypal.wfRole_Ajax"
	ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<title>Role</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
</head>
<body>
	<form id="form1" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<div>
			<table id="tblmain" class="clstablelistout">
				<tr>
					<td>
						<asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
							<asp:UpdatePanel ID="upnlRole" runat="server" UpdateMode="Conditional">
								<ContentTemplate>
									<table id="tblInner" class="clstablelistin">
										<tr>
											<td colspan="4" class="clsFormHeader1Newstyle">
												<table width="100%">
													<tr>
														<td>
															<asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">
																Role Information [New]
															</asp:Label>
														</td>
														<td align="right">
															<table id="Table2" border="0" cellpadding="1">
																<tr>
																	<td>
																		<asp:Button CssClass="clsbtnH clsinfoH" ID="btnSave"
																			runat="server" Text="Save" ToolTip="Save the current Record" />
																	</td>
																	<td>
																		<asp:Button CssClass="clsbtnH clsinfoH" ID="btnClose"
																			runat="server" CausesValidation="False"
																			Text="Close" ToolTip="Close Role Information screen" />
																	</td>
																</tr>
															</table>
														</td>
													</tr>
												</table>
											</td>
										</tr>
										<tr>
											<td colspan="4">
												<asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary" />
												<asp:RequiredFieldValidator ID="rfvr" runat="server" ControlToValidate="txtRoleName"
													CssClass="clsLabel" Display="None" ErrorMessage="Role Name Required" Width="40px"></asp:RequiredFieldValidator>
												<asp:CustomValidator ID="cvcp" runat="server" ControlToValidate="txtRoleName" CssClass="cslLabelAuto"
													Display="None" ErrorMessage="Select " OnServerValidate="customvalidate"></asp:CustomValidator>
											</td>
										</tr>
										<tr>
											<td>
												<asp:Label ID="lblRole1" runat="server" CssClass="clsLabelStar">*</asp:Label>
											</td>
											<td>
												<table id="Table3" border="0" cellpadding="1">
													<tr>
														<td>
															<asp:Label ID="lblRole" runat="server" CssClass="clsLabel">Role</asp:Label>
														</td>
														<td>
															<asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtRoleName" runat="server" MaxLength="50"
																Text="<%# mRole.Name %>" ToolTip="Enter Role"></asp:TextBox>
														</td>
													</tr>
												</table>
											</td>
											<td></td>
										</tr>
										<tr>
											<td colspan="4">
												<asp:Label ID="lblNote" runat="server" CssClass="clsLabelHeader">
													Select the permission below for this Role
												</asp:Label>
											</td>
										</tr>
										<tr>
											<td colspan="4">
												<asp:Label ID="lbllistPermission" runat="server" CssClass="clsLabelHeader">
													List of Permission
												</asp:Label>
											</td>
										</tr>
										<tr>
											<td colspan="2">
												<table id="Table6" border="0" cellpadding="1">
													<tr>
														<td>
															<asp:CheckBox ID="chkInventory" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																Text="Inventory" />
														</td>
													</tr>
												</table>
											</td>
											<td colspan="2">
												<table id="Table7" border="0" cellpadding="1">
													<tr>
														<td>
															<asp:CheckBox ID="chkMaintenance" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																Text="Maintenance" />
														</td>
														<td>
															<asp:Label ID="Label1" runat="server" CssClass="clsLabel" Width="56px"></asp:Label>
														</td>
														<td>
															<asp:CheckBox ID="chkTool" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																Text="User / Admin Utilities" />
														</td>

													</tr>
												</table>
											</td>
										</tr>
										<tr>
											<td colspan="2" valign="top">
												<table id="Table4" border="0" cellpadding="1">
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkMasters" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.Inv_Master_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlMasters" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.Inv_Master_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblMastersSelection"
																						class="clsLabelHeader">Masters</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgMasters" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlMasters" runat="server" Visible="<%# mRole.Inv_Master_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table8" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgMasters" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Inventory Master Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllMasters" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Masters">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>

																					<asp:TemplateField HeaderText="View" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllMasters" ClientIDMode="Static" runat="server" Text="View"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderText="Print" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllMasters" ClientIDMode="Static" runat="server" Text="Print"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderText="Add" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllMasters" ClientIDMode="Static" runat="server" Text="Add"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllMasters" ClientIDMode="Static" runat="server" Text="Edit"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllMasters" ClientIDMode="Static" runat="server" Text="Delete"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpMastersBehaviour" ID="clpMasters" ClientIDMode="Static"
																runat="Server" TargetControlID="pnlMasters" ExpandControlID="ClpnlMasters" CollapseControlID="ClpnlMasters"
																Collapsed="False" ImageControlID="imgMasters" CollapsedSize="0" ExpandedText="(Hide Details...)"
																CollapsedText="(Show Details...)" ExpandedImage="~/images/collapse_blue.jpg"
																CollapsedImage="~/images/expand_blue.jpg" SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkRequisition" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.Inv_Requisition_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlRequisition" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.Inv_Requisition_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblRequisitionSelection"
																						class="clsLabelHeader">Requisition</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgRequisition" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlRequisition" runat="server" Visible="<%# mRole.Inv_Requisition_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table9" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgRequisition" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Requisition Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllRequisition" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Requisition">
																						<HeaderStyle Width="300px" />
																						<ItemStyle BackColor='' />
																					</asp:BoundField>
																					<asp:TemplateField HeaderText="View" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllRequisition" ClientIDMode="Static" runat="server" Text="View"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderText="Print" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllRequisition" ClientIDMode="Static" runat="server" Text="Print"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderText="Add" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllRequisition" ClientIDMode="Static" runat="server" Text="Add"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllRequisition" ClientIDMode="Static" runat="server" Text="Edit"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllRequisition" ClientIDMode="Static" runat="server" Text="Delete"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderText="Authorized" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAuthorizedAllRequisition" ClientIDMode="Static" runat="server"
																								Text="Authorized" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAuthorized" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedAuthorized") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpRequisitionBehaviour" ID="clpRequisition"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlRequisition" ExpandControlID="ClpnlRequisition"
																CollapseControlID="ClpnlRequisition" Collapsed="False" ImageControlID="imgRequisition"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td valign="top"></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkNewRequisition" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.Inv_New_Requisition_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlNewRequisition" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.Inv_New_Requisition_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblNewRequisitionSelection"
																						class="clsLabelHeader">New Requisition</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgNewRequisition" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlNewRequisition" runat="server" Visible="<%# mRole.Inv_New_Requisition_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table10" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgNewRequisition" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Requisition Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllNewRequisition" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="New Requisition">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderText="View" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllNewRequisition" ClientIDMode="Static" runat="server"
																								Text="View" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderText="Print" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllNewRequisition" ClientIDMode="Static" runat="server"
																								Text="Print" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderText="Add" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllNewRequisition" ClientIDMode="Static" runat="server" Text="Add"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllNewRequisition" ClientIDMode="Static" runat="server"
																								Text="Edit" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllNewRequisition" ClientIDMode="Static" runat="server"
																								Text="Delete" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderText="Authorized" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAuthorizedAllNewRequisition" ClientIDMode="Static" runat="server"
																								Text="Authorized" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAuthorized" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedAuthorized") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpNewRequisitionBehaviour" ID="clpNewRequisition"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlNewRequisition" ExpandControlID="ClpnlNewRequisition"
																CollapseControlID="ClpnlNewRequisition" Collapsed="False" ImageControlID="imgNewRequisition"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkPurchaseEnquiry" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.Inv_PurchaseEnquiries_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlPurchaseEnquiry" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.Inv_PurchaseEnquiries_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblPurchaseEnquirySelection"
																						class="clsLabelHeader">Purchase Enquiry</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgPurchaseEnquiry" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlPurchaseEnquiry" runat="server" Visible="<%# mRole.Inv_PurchaseEnquiries_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table11" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgPurchaseEnquiry" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Purchase Enquiry Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllPurchaseEnquiry" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Purchase Enquiry">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderText="View" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllPurchaseEnquiry" ClientIDMode="Static" runat="server"
																								Text="View" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderText="Print" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllPurchaseEnquiry" ClientIDMode="Static" runat="server"
																								Text="Print" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderText="Add" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllPurchaseEnquiry" ClientIDMode="Static" runat="server"
																								Text="Add" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllPurchaseEnquiry" ClientIDMode="Static" runat="server"
																								Text="Edit" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllPurchaseEnquiry" ClientIDMode="Static" runat="server"
																								Text="Delete" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderText="Authorized" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAuthorizedAllPurchaseEnquiry" ClientIDMode="Static" runat="server"
																								Text="Authorized" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAuthorized" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedAuthorized") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpPurchaseEnquiryBehaviour" ID="clpPurchaseEnquiry"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlPurchaseEnquiry" ExpandControlID="ClpnlPurchaseEnquiry"
																CollapseControlID="ClpnlPurchaseEnquiry" Collapsed="False" ImageControlID="imgPurchaseEnquiry"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkPurchaseQuotation" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.Inv_PurchaseQuotations_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlPurchaseQuotation" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.Inv_PurchaseQuotations_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblPurchaseQuotationSelection"
																						class="clsLabelHeader">Purchase Quotation</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgPurchaseQuotation" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlPurchaseQuotation" runat="server" Visible="<%# mRole.Inv_PurchaseQuotations_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table12" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgPurchaseQuotation" runat="server" AutoGenerateColumns="False"
																				ShowHeaderWhenEmpty="true" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Purchase Quotation Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllPurchaseQuotation" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Purchase Quotation">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderText="View" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllPurchaseQuotation" ClientIDMode="Static" runat="server"
																								Text="View" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderText="Print" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllPurchaseQuotation" ClientIDMode="Static" runat="server"
																								Text="Print" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderText="Add" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllPurchaseQuotation" ClientIDMode="Static" runat="server"
																								Text="Add" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllPurchaseQuotation" ClientIDMode="Static" runat="server"
																								Text="Edit" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllPurchaseQuotation" ClientIDMode="Static" runat="server"
																								Text="Delete" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderText="Authorized" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAuthorizedAllPurchaseQuotation" ClientIDMode="Static" runat="server"
																								Text="Authorized" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAuthorized" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedAuthorized") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpPurchaseQuotationBehaviour" ID="clpPurchaseQuotation"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlPurchaseQuotation" ExpandControlID="ClpnlPurchaseQuotation"
																CollapseControlID="ClpnlPurchaseQuotation" Collapsed="False" ImageControlID="imgPurchaseQuotation"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkPurchaseOrder" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.Inv_PurchaseOrders_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlPurchaseOrder" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.Inv_PurchaseOrders_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblPurchaseOrderSelection"
																						class="clsLabelHeader">Purchase Order</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgPurchaseOrder" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlPurchaseOrder" runat="server" Visible="<%# mRole.Inv_PurchaseOrders_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table13" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgPurchaseOrder" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Purchase Order Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllPurchaseOrder" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Purchase Order">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderText="View" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllPurchaseOrder" ClientIDMode="Static" runat="server" Text="View"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderText="Print" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllPurchaseOrder" ClientIDMode="Static" runat="server"
																								Text="Print" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderText="Add" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllPurchaseOrder" ClientIDMode="Static" runat="server" Text="Add"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllPurchaseOrder" ClientIDMode="Static" runat="server" Text="Edit"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllPurchaseOrder" ClientIDMode="Static" runat="server"
																								Text="Delete" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderText="Authorized" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAuthorizedAllPurchaseOrder" ClientIDMode="Static" runat="server"
																								Text="Authorized" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAuthorized" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedAuthorized") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpPurchaseOrderBehaviour" ID="ClpPurchaseOrder"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlPurchaseOrder" ExpandControlID="ClpnlPurchaseOrder"
																CollapseControlID="ClpnlPurchaseOrder" Collapsed="False" ImageControlID="imgPurchaseOrder"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkGoodsReceipt" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.Inv_GoodsReceipts_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlGoodsReceipt" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.Inv_GoodsReceipts_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblGoodsReceiptSelection"
																						class="clsLabelHeader">Material In</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgGoodsReceipt" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlGoodsReceipt" runat="server" Visible="<%# mRole.Inv_GoodsReceipts_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table14" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgGoodsReceipt" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Material In Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllGoodsReceipt" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Material In">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderText="View" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllGoodsReceipt" ClientIDMode="Static" runat="server" Text="View"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderText="Print" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllGoodsReceipt" ClientIDMode="Static" runat="server" Text="Print"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllGoodsReceipt" ClientIDMode="Static" runat="server" Text="Add"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllGoodsReceipt" ClientIDMode="Static" runat="server" Text="Edit"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllGoodsReceipt" ClientIDMode="Static" runat="server"
																								Text="Delete" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Authorized">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAuthorizedAllGoodsReceipt" ClientIDMode="Static" runat="server"
																								Text="Authorized" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAuthorized" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedAuthorized") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpGoodsReceiptBehaviour" ID="clpGoodsReceipt"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlGoodsReceipt" ExpandControlID="ClpnlGoodsReceipt"
																CollapseControlID="ClpnlGoodsReceipt" Collapsed="False" ImageControlID="imgGoodsReceipt"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkGoodsIssue" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.Inv_GoodsIssues_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlGoodsIssue" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.Inv_GoodsIssues_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblGoodsIssueSelection"
																						class="clsLabelHeader">Material Out</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgGoodsIssue" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlGoodsIssue" runat="server" Visible="<%# mRole.Inv_GoodsIssues_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table15" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgGoodsIssue" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights Material Out Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllGoodsIssue" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Material Out">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllGoodsIssue" ClientIDMode="Static" runat="server" Text="View"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllGoodsIssue" ClientIDMode="Static" runat="server" Text="Print"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllGoodsIssue" ClientIDMode="Static" runat="server" Text="Add"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllGoodsIssue" ClientIDMode="Static" runat="server" Text="Edit"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllGoodsIssue" ClientIDMode="Static" runat="server" Text="Delete"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Authorized">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAuthorizedAllGoodsIssue" ClientIDMode="Static" runat="server"
																								Text="Authorized" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAuthorized" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedAuthorized") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpGoodsIssueBehaviour" ID="clpGoodsIssue"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlGoodsIssue" ExpandControlID="ClpnlGoodsIssue"
																CollapseControlID="ClpnlGoodsIssue" Collapsed="False" ImageControlID="imgGoodsIssue"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkPurchaseInvoice" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.Inv_PurchaseInvoices_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlPurchaseInvoice" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.Inv_PurchaseInvoices_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblPurchaseInvoiceSelection"
																						class="clsLabelHeader">Purchase Invoice</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgPurchaseInvoice" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlPurchaseInvoice" runat="server" Visible="<%# mRole.Inv_PurchaseInvoices_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table16" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgPurchaseInvoice" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Purchase Invoice Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllPurchaseInvoice" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Purchase Invoice">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllPurchaseInvoice" ClientIDMode="Static" runat="server"
																								Text="View" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllPurchaseInvoice" ClientIDMode="Static" runat="server"
																								Text="Print" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllPurchaseInvoice" ClientIDMode="Static" runat="server"
																								Text="Add" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllPurchaseInvoice" ClientIDMode="Static" runat="server"
																								Text="Edit" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllPurchaseInvoice" ClientIDMode="Static" runat="server"
																								Text="Delete" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Authorized">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAuthorizedAllPurchaseInvoice" ClientIDMode="Static" runat="server"
																								Text="Authorized" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAuthorized" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedAuthorized") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpPurchaseInvoiceBehaviour" ID="clPurchaseInvoice"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlPurchaseInvoice" ExpandControlID="ClpnlPurchaseInvoice"
																CollapseControlID="ClpnlPurchaseInvoice" Collapsed="False" ImageControlID="imgPurchaseInvoice"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkPaymentAdvice" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.PaymentAdvice_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlPaymentAdvice" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.PaymentAdvice_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblPaymentAdviceSelection"
																						class="clsLabelHeader">Payment Advice</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgPaymentAdvice" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlPaymentAdvice" runat="server" Visible="<%# mRole.PaymentAdvice_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table37" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgPaymentAdvice" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Purchase Invoice Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllPaymentAdvice" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Purchase Advice">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllPaymentAdvice" ClientIDMode="Static" runat="server" Text="View"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllPaymentAdvice" ClientIDMode="Static" runat="server"
																								Text="Print" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllPaymentAdvice" ClientIDMode="Static" runat="server" Text="Add"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllPaymentAdvice" ClientIDMode="Static" runat="server" Text="Edit"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllPaymentAdvice" ClientIDMode="Static" runat="server"
																								Text="Delete" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Authorized">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAuthorizedAllPaymentAdvice" ClientIDMode="Static" runat="server"
																								Text="Authorized" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAuthorized" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedAuthorized") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpPaymentAdviceBehaviour" ID="clPaymentAdvice"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlPaymentAdvice" ExpandControlID="ClpnlPaymentAdvice"
																CollapseControlID="ClpnlPaymentAdvice" Collapsed="False" ImageControlID="imgPaymentAdvice"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkSalesModules" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.Inv_SalesModules_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlSalesModules" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.Inv_SalesModules_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblSalesModulesSelection"
																						class="clsLabelHeader">Sales Modules</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgSalesModules" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlSalesModules" runat="server" Visible="<%# mRole.Inv_SalesModules_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table31" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgSalesModules" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Sales Modules Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllSalesModules" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Sales Modules">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllSalesModules" ClientIDMode="Static" runat="server" Text="View"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllSalesModules" ClientIDMode="Static" runat="server" Text="Print"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllSalesModules" ClientIDMode="Static" runat="server" Text="Add"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllSalesModules" ClientIDMode="Static" runat="server" Text="Edit"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllSalesModules" ClientIDMode="Static" runat="server"
																								Text="Delete" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Authorized">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAuthorizedAllSalesModules" ClientIDMode="Static" runat="server"
																								Text="Authorized" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAuthorized" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedAuthorized") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpSalesModulesBehaviour" ID="clpSalesModules"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlSalesModules" ExpandControlID="ClpnlSalesModules"
																CollapseControlID="ClpnlSalesModules" Collapsed="False" ImageControlID="imgSalesModules"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td valign="top"></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkCalibration" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.Calibration_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlCalibration" runat="server" CssClass="clsCollapsePnl" Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblCalibrationSelection"
																						class="clsLabelHeader">Calibration</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgCalibration" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlCalibration" runat="server" Visible="<%# mRole.Calibration_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table30" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgCalibration" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Calibration Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllCalibration" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Calibration">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllCalibration" ClientIDMode="Static" runat="server" Text="View"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllCalibration" ClientIDMode="Static" runat="server" Text="Print"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllCalibration" ClientIDMode="Static" runat="server" Text="Add"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllCalibration" ClientIDMode="Static" runat="server" Text="Edit"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllCalibration" ClientIDMode="Static" runat="server" Text="Delete"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpCalibrationBehaviour" ID="clpCalibration"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlCalibration" ExpandControlID="ClpnlCalibration"
																CollapseControlID="ClpnlCalibration" Collapsed="False" ImageControlID="imgCalibration"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td valign="top"></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="ckWorkInvoice" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.Inv_WorkInvoice_Modules.Count > 0 %>" Enabled="false" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="clpnlWorkInvoice" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.Inv_WorkInvoice_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblWorkInvoiceSelection"
																						class="clsLabelHeader">Work Invoice</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgWorkInvoice" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlWorkInvoice" runat="server" Visible="<%# mRole.Inv_WorkInvoice_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table29" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgWorkInvoice" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				Enabled="false" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Work Invoice Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllWorkInvoice" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Work Invoice">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllWorkInvoice" ClientIDMode="Static" runat="server" Text="View"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllWorkInvoice" ClientIDMode="Static" runat="server" Text="Print"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllWorkInvoice" ClientIDMode="Static" runat="server" Text="Add"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllWorkInvoice" ClientIDMode="Static" runat="server" Text="Edit"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllWorkInvoice" ClientIDMode="Static" runat="server" Text="Delete"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Authorized">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAuthorizedAllWorkInvoice" ClientIDMode="Static" runat="server"
																								Text="Authorized" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAuthorized" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedAuthorized") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpWorkInvoiceBehaviour" ID="clpWorkInvoice"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlWorkInvoice" ExpandControlID="ClpnlWorkInvoice"
																CollapseControlID="ClpnlWorkInvoice" Collapsed="False" ImageControlID="imgWorkInvoice"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td valign="top"></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkLineMaintenance" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.Inv_LineMaintenance_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlLineMaintenance" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.Inv_LineMaintenance_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblLineMaintenanceSelection"
																						class="clsLabelHeader">Line Maintenance</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgLineMaintenance" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlLineMaintenance" runat="server" Visible="<%# mRole.Inv_LineMaintenance_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table28" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgLineMaintenance" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights From Line Maintenance Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllLineMaintenance" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Line Maintenance">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllLineMaintenance" ClientIDMode="Static" runat="server"
																								Text="View" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllLineMaintenance" ClientIDMode="Static" runat="server"
																								Text="Print" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllLineMaintenance" ClientIDMode="Static" runat="server"
																								Text="Add" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllLineMaintenance" ClientIDMode="Static" runat="server"
																								Text="Edit" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllLineMaintenance" ClientIDMode="Static" runat="server"
																								Text="Delete" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Authorized">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAuthorizedAllLineMaintenance" ClientIDMode="Static" runat="server"
																								Text="Authorized" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAuthorized" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedAuthorized") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpLineMaintenanceBehaviour" ID="clpLineMaintenance"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlLineMaintenance" ExpandControlID="ClpnlLineMaintenance"
																CollapseControlID="ClpnlLineMaintenance" Collapsed="False" ImageControlID="imgLineMaintenance"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td valign="top"></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkExportInvoice" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.Inv_ExportInvoice_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlExportInvoice" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.Inv_ExportInvoice_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblExportInvoiceSelection"
																						class="clsLabelHeader">Export Invoice</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgExportInvoice" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlExportInvoice" runat="server" Visible="<%# mRole.Inv_ExportInvoice_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table27" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgExportInvoice" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights From Export Invoice Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllExportInvoice" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Export Invoice">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllExportInvoice" ClientIDMode="Static" runat="server" Text="View"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllExportInvoice" ClientIDMode="Static" runat="server"
																								Text="Print" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllExportInvoice" ClientIDMode="Static" runat="server" Text="Add"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllExportInvoice" ClientIDMode="Static" runat="server" Text="Edit"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllExportInvoice" ClientIDMode="Static" runat="server"
																								Text="Delete" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Authorized">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAuthorizedAllExportInvoice" ClientIDMode="Static" runat="server"
																								Text="Authorized" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAuthorized" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedAuthorized") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpExportInvoiceBehaviour" ID="clpExportInvoice"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlExportInvoice" ExpandControlID="ClpnlExportInvoice"
																CollapseControlID="ClpnlExportInvoice" Collapsed="False" ImageControlID="imgExportInvoice"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td valign="top"></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkReliability" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.Inv_Reliability_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="clpnlReliability" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.Inv_Reliability_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblReliabilitySelection"
																						class="clsLabelHeader">Reliability</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgReliability" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlReliability" runat="server" Visible="<%# mRole.Inv_Reliability_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table26" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgReliability" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Reliability Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllReliability" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Reliability">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllReliability" ClientIDMode="Static" runat="server" Text="View"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllReliability" ClientIDMode="Static" runat="server" Text="Print"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllReliability" ClientIDMode="Static" runat="server" Text="Add"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllReliability" ClientIDMode="Static" runat="server" Text="Edit"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllReliability" ClientIDMode="Static" runat="server" Text="Delete"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpReliabilityBehaviour" ID="clpReliability"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlReliability" ExpandControlID="ClpnlReliability"
																CollapseControlID="ClpnlReliability" Collapsed="False" ImageControlID="imgReliability"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td valign="top"></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkInventoryReports" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.Inv_Reports_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlInventoryReports" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.Inv_Reports_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblInventoryReportsSelection"
																						class="clsLabelHeader">Inventory Reports</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgInventoryReports" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlInventoryReports" runat="server" Visible="<%# mRole.Inv_Reports_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table25" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgInventoryReports" runat="server" AutoGenerateColumns="False"
																				ShowHeaderWhenEmpty="true" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Reports Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<%--  <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkSingleAllInventoryReports" runat="server" onclick="CheckUncheckSingleRow(this)" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>--%>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Reports">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllInventoryReports" ClientIDMode="Static" runat="server"
																								Text="View" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpInventoryReportsBehaviour" ID="clpInventoryReports"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlInventoryReports" ExpandControlID="ClpnlInventoryReports"
																CollapseControlID="ClpnlInventoryReports" Collapsed="False" ImageControlID="imgInventoryReports"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkDocumentLocker" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.DocumentLocker_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlDocumentLocker" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.DocumentLocker_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblDocumentLockerSelection"
																						class="clsLabelHeader">Document Locker</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgDocumentLocker" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlDocumentLocker" runat="server" Visible="<%# mRole.DocumentLocker_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table43" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgDocumentLocker" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Document Locker Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllDocumentLocker" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Document Locker">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllDocumentLocker" ClientIDMode="Static" runat="server"
																								Text="View" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllDocumentLocker" ClientIDMode="Static" runat="server"
																								Text="Print" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllDocumentLocker" ClientIDMode="Static" runat="server" Text="Add"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllDocumentLocker" ClientIDMode="Static" runat="server"
																								Text="Edit" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllDocumentLocker" ClientIDMode="Static" runat="server"
																								Text="Delete" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpDocumentLockerBehaviour" ID="clpDocumentLocker"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlDocumentLocker" ExpandControlID="ClpnlDocumentLocker"
																CollapseControlID="ClpnlDocumentLocker" Collapsed="False" ImageControlID="imgDocumentLocker"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkADSBReviewMeeting" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.ADSBReviewMeeting_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlADSBReviewMeeting" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.ADSBReviewMeeting_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblADSBReviewMeetingSelection"
																						class="clsLabelHeader">AD/SB Review Meeting</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgADSBReviewMeeting" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlADSBReviewMeeting" runat="server" Visible="<%# mRole.ADSBReviewMeeting_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table47" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgADSBReviewMeeting" runat="server" AutoGenerateColumns="False"
																				ShowHeaderWhenEmpty="true" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from ADSB Review Meeting Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllADSBReviewMeeting" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="AD/SB Review Meeting">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllADSBReviewMeeting" ClientIDMode="Static" runat="server"
																								Text="View" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllADSBReviewMeeting" ClientIDMode="Static" runat="server"
																								Text="Print" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllADSBReviewMeeting" ClientIDMode="Static" runat="server"
																								Text="Add" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllADSBReviewMeeting" ClientIDMode="Static" runat="server"
																								Text="Edit" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllADSBReviewMeeting" ClientIDMode="Static" runat="server"
																								Text="Delete" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Authorized">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAuthorizedADSBReviewMeeting" ClientIDMode="Static" runat="server"
																								Text="Authorized" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAuthorized" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedAuthorized") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpADSBReviewMeetingBehaviour" ID="clpADSBReviewMeeting"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlADSBReviewMeeting" ExpandControlID="ClpnlADSBReviewMeeting"
																CollapseControlID="ClpnlADSBReviewMeeting" Collapsed="False" ImageControlID="imgADSBReviewMeeting"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkEmpCAAuthorization" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.EmpCAAuthorization_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlEmpCAAuthorization" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.EmpCAAuthorization_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblEmpCAAuthorizationSelection"
																						class="clsLabelHeader">
																						<asp:Label ID="Label3" runat="server" Text="Emp. CA. Authorization"></asp:Label>
																					</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgEmpCAAuthorization" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlEmpCAAuthorization" runat="server" Visible="<%# mRole.EmpCAAuthorization_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table461" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgEmpCAAuthorization" runat="server" AutoGenerateColumns="False"
																				ShowHeaderWhenEmpty="true" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Emp CA Authorization Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllEmpCAAuthorization" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Emp. CA. Authorization">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllEmpCAAuthorization" ClientIDMode="Static" runat="server"
																								Text="View" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllEmpCAAuthorization" ClientIDMode="Static" runat="server"
																								Text="Print" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllEmpCAAuthorization" ClientIDMode="Static" runat="server"
																								Text="Add" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllEmpCAAuthorization" ClientIDMode="Static" runat="server"
																								Text="Edit" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllEmpCAAuthorization" ClientIDMode="Static" runat="server"
																								Text="Delete" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>

																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpEmpCAAuthorization" ID="clpEmpCAAuthorization"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlEmpCAAuthorization" ExpandControlID="ClpnlEmpCAAuthorization"
																CollapseControlID="ClpnlEmpCAAuthorization" Collapsed="False" ImageControlID="imgEmpCAAuthorization"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkDueJobPlanning" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.DueJobPlanning_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlDueJobPlanning" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.DueJobPlanning_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblDueJobPlanningSelection"
																						class="clsLabelHeader">
																						<asp:Label ID="Label4" runat="server" Text="Due Job Planning"></asp:Label>
																					</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgDueJobPlanning" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlDueJobPlanning" runat="server" Visible="<%# mRole.DueJobPlanning_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table462" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgDueJobPlanning" runat="server" AutoGenerateColumns="False"
																				ShowHeaderWhenEmpty="true" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Work Order Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAlDueJobPlanning" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Due Job Planning">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllDueJobPlanning" ClientIDMode="Static" runat="server"
																								Text="View" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllDueJobPlanning" ClientIDMode="Static" runat="server"
																								Text="Print" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllDueJobPlanning" ClientIDMode="Static" runat="server"
																								Text="Add" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllDueJobPlanning" ClientIDMode="Static" runat="server"
																								Text="Edit" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllDueJobPlanning" ClientIDMode="Static" runat="server"
																								Text="Delete" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>

																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpDueJobPlanning" ID="clpDueJobPlanning"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlDueJobPlanning" ExpandControlID="ClpnlDueJobPlanning"
																CollapseControlID="ClpnlDueJobPlanning" Collapsed="False" ImageControlID="imgDueJobPlanning"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
												</table>
											</td>
											<td colspan="2" valign="top">
												<table id="Table5" border="0" cellpadding="1">
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkMaintMasters" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.Maint_Master_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlMasters1" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.Maint_Master_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="Span1" class="clsLabelHeader">Masters</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgMasters1" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlMasters1" runat="server" Visible="<%# mRole.Maint_Master_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table17" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgMaintMasters" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Maintenance Master Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllMaintMasters" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Masters">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllMaintMasters" ClientIDMode="Static" runat="server" Text="View"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllMaintMasters" ClientIDMode="Static" runat="server" Text="Print"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllMaintMasters" ClientIDMode="Static" runat="server" Text="Add"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllMaintMasters" ClientIDMode="Static" runat="server" Text="Edit"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllMaintMasters" ClientIDMode="Static" runat="server"
																								Text="Delete" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpMasters1Behaviour" ID="clpMasters1"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlMasters1" ExpandControlID="ClpnlMasters1"
																CollapseControlID="ClpnlMasters1" Collapsed="False" ImageControlID="imgMasters1"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td valign="top"></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkMainteance" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.Maint_Maintenance_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlMaintenance" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.Maint_Maintenance_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblMaintenanceSelection"
																						class="clsLabelHeader">Maintenance</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgMaintenance" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlMaintenance" runat="server" Visible="<%# mRole.Maint_Maintenance_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table18" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgMaintenance" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Maintenance Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllMaintenance" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Maintenance">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllMaintenance" ClientIDMode="Static" runat="server" Text="View"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllMaintenance" ClientIDMode="Static" runat="server" Text="Print"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllMaintenance" ClientIDMode="Static" runat="server" Text="Add"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllMaintenance" ClientIDMode="Static" runat="server" Text="Edit"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllMaintenance" ClientIDMode="Static" runat="server" Text="Delete"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Authorized">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAuthorizedAllMaintenance" ClientIDMode="Static" runat="server"
																								Text="Authorized" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAuthorized" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedAuthorized") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpMaintenanceBehaviour" ID="clpMaintenance"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlMaintenance" ExpandControlID="ClpnlMaintenance"
																CollapseControlID="ClpnlMaintenance" Collapsed="False" ImageControlID="imgMaintenance"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td valign="top"></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkSpareMaint" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.SpareMaint_Maintenance_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlSpareMaint" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.SpareMaint_Maintenance_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="Span7" class="clsLabelHeader">Build Assembly Maint</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgSpareMaint" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlSpareMaint" runat="server" Visible="<%# mRole.SpareMaint_Maintenance_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table40" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgSpareMaint" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Spare Assembly Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left" />
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllSpareMaint" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Spare Assembly">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllSpareMaint" ClientIDMode="Static" runat="server" Text="View"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllSpareMaint" ClientIDMode="Static" runat="server" Text="Print"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllSpareMaint" ClientIDMode="Static" runat="server" Text="Add"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllSpareMaint" ClientIDMode="Static" runat="server" Text="Edit"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllSpareMaint" ClientIDMode="Static" runat="server" Text="Delete"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Authorized">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAuthorizedAllSpareMaint" ClientIDMode="Static" runat="server"
																								Text="Authorized" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAuthorized" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedAuthorized") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Completed">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkCompletedAllSpareMaint" ClientIDMode="Static" runat="server"
																								Text="Completed" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkCompleted" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedCompleted") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpSpareMaintBehaviour" ID="clpSpareMaint"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlSpareMaint" ExpandControlID="ClpnlSpareMaint"
																CollapseControlID="ClpnlSpareMaint" Collapsed="False" ImageControlID="imgSpareMaint"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td valign="top"></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkManual" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.Manual_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlManual" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.Manual_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblManualSelection" class="clsLabelHeader">Tech Library</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgManual" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlManual" runat="server" Visible="<%# mRole.Manual_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table19" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgManual" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Tech Library Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllManual" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Tech Library">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllManual" ClientIDMode="Static" runat="server" Text="View"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllManual" ClientIDMode="Static" runat="server" Text="Print"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllManual" ClientIDMode="Static" runat="server" Text="Add"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllManual" ClientIDMode="Static" runat="server" Text="Edit"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllManual" ClientIDMode="Static" runat="server" Text="Delete"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpManualBehaviour" ID="clpManual" ClientIDMode="Static"
																runat="Server" TargetControlID="pnlManual" ExpandControlID="ClpnlManual" CollapseControlID="ClpnlManual"
																Collapsed="False" ImageControlID="imgManual" CollapsedSize="0" ExpandedText="(Hide Details...)"
																CollapsedText="(Show Details...)" ExpandedImage="~/images/collapse_blue.jpg"
																CollapsedImage="~/images/expand_blue.jpg" SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td valign="top"></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkWorkOrder" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.Inv_WorkOrder_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlWorkOrder" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.Inv_WorkOrder_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblWorkOrderSelection"
																						class="clsLabelHeader">
																						<asp:Label ID="lblWorkOrder" runat="server" Text="Work Order"></asp:Label>
																					</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgWorkOrder" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlWorkOrder" runat="server" Visible="<%# mRole.Inv_WorkOrder_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table20" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgWorkOrder" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Work Order Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllWorkOrder" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Work Order">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllWorkOrder" ClientIDMode="Static" runat="server" Text="View"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllWorkOrder" ClientIDMode="Static" runat="server" Text="Print"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllWorkOrder" ClientIDMode="Static" runat="server" Text="Add"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllWorkOrder" ClientIDMode="Static" runat="server" Text="Edit"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllWorkOrder" ClientIDMode="Static" runat="server" Text="Delete"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Authorized">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAuthorizedAllWorkOrder" ClientIDMode="Static" runat="server"
																								Text="Authorized" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAuthorized" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedAuthorized") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Completed">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkCompletedAllWorkOrder" ClientIDMode="Static" runat="server"
																								Text="Completed" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkCompleted" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedCompleted") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpWorkOrderBehaviour" ID="clpWorkOrder"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlWorkOrder" ExpandControlID="ClpnlWorkOrder"
																CollapseControlID="ClpnlWorkOrder" Collapsed="False" ImageControlID="imgWorkOrder"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td valign="top"></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkProject" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.Inv_WorkOrder_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="cpnlProject" runat="server" CssClass="clsCollapsePnl" 
																			Visible="<%# mRole.Inv_WorkOrder_Modules.Count > 0 %>">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblWorkOrderSelection"
																						class="clsLabelHeader">
																						<asp:Label ID="lblProjectPanelTitle" runat="server" Text="Project"></asp:Label>
																					</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgProject" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlProject" runat="server" Visible="<%# mRole.Inv_WorkOrder_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table20" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="GV_Project" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
																				ToolTip="Select Permissions for Project Module.">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle BackColor="white" CssClass="clsdgHeader"
																					Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
																				<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
																				<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
																				<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
																				<Columns>
																					<%--0--%>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllProject" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<%--1--%>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Project">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<%--2--%>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllProject" ClientIDMode="Static" runat="server" Text="View"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<%--3--%>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllProject" ClientIDMode="Static" runat="server" Text="Print"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<%--4--%>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllProject" ClientIDMode="Static" runat="server" Text="Add"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<%--5--%>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllProject" ClientIDMode="Static" runat="server" Text="Edit"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<%--6--%>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllProject" ClientIDMode="Static" runat="server" Text="Delete"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<%--7--%>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Authorized">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAuthorizedAllProject" ClientIDMode="Static" runat="server"
																								Text="Authorized" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAuthorized" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedAuthorized") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<%--8--%>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpBehaviourProject" ID="cpnlExtenderProject"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlProject" ExpandControlID="cpnlProject"
																CollapseControlID="cpnlProject" Collapsed="False" ImageControlID="imgProject"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td valign="top"></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkMROContract" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.MROContract_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlMROContract" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.MROContract_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblMROContractSelection"
																						class="clsLabelHeader">MRO Contract</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgMROContract" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlMROContract" runat="server" Visible="<%# mRole.MROContract_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table46" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgMROContract" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from MRO Contract Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllMROContract" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="MRO Contract">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllMROContract" ClientIDMode="Static" runat="server" Text="View"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllMROContract" ClientIDMode="Static" runat="server" Text="Print"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllMROContract" ClientIDMode="Static" runat="server" Text="Add"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllMROContract" ClientIDMode="Static" runat="server" Text="Edit"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllMROContract" ClientIDMode="Static" runat="server" Text="Delete"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Authorized">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAuthorizedAllMROContract" ClientIDMode="Static" runat="server"
																								Text="Authorized" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAuthorized" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedAuthorized") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpMROContractBehaviour" ID="clpMROContract"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlMROContract" ExpandControlID="ClpnlMROContract"
																CollapseControlID="ClpnlMROContract" Collapsed="False" ImageControlID="imgMROContract"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td valign="top"></td>
													</tr>
													<tr>
														<%-- ============== Ajay =================--%>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkWorkOrderInvoice" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.Inv_nWOInvoice_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlWorkOrderInvoice" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.Inv_nWOInvoice_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblWorkOrderInvoiceSelection"
																						class="clsLabelHeader">
																						<asp:Label ID="lblWorkOrderInvoice" runat="server" Text="Work Order Invoice"></asp:Label>
																					</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgWorkOrderInvoice" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlWorkOrderInvoice" runat="server" Visible="<%# mRole.Inv_nWOInvoice_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table45" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgWorkOrderInvoice" runat="server" AutoGenerateColumns="False"
																				ShowHeaderWhenEmpty="true" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Work Order Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllWorkOrderInvoice" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Work Order">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllWorkOrderInvoice" ClientIDMode="Static" runat="server"
																								Text="View" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllWorkOrderInvoice" ClientIDMode="Static" runat="server"
																								Text="Print" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllWorkOrderInvoice" ClientIDMode="Static" runat="server"
																								Text="Add" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllWorkOrderInvoice" ClientIDMode="Static" runat="server"
																								Text="Edit" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllWorkOrderInvoice" ClientIDMode="Static" runat="server"
																								Text="Delete" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Authorized">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAuthorizedAllWorkOrderInvoice" ClientIDMode="Static" runat="server"
																								Text="Authorized" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAuthorized" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedAuthorized") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpWorkOrderInvoiceBehaviour" ID="clpWorkOrderInvoice"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlWorkOrderInvoice" ExpandControlID="ClpnlWorkOrderInvoice"
																CollapseControlID="ClpnlWorkOrderInvoice" Collapsed="False" ImageControlID="imgWorkOrderInvoice"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
													<%-- ============ Ajay ====================--%>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkMSP" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.Inv_nMSP_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlMSP" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.Inv_nMSP_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblMSPSelection"
																						class="clsLabelHeader">
																						<asp:Label ID="Label2" runat="server" Text="MSP"></asp:Label>
																					</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgMSP" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlMSP" runat="server" Visible="<%# mRole.Inv_nMSP_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table460" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgMSP" runat="server" AutoGenerateColumns="False"
																				ShowHeaderWhenEmpty="true" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from MSP Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllMSP" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="MSP">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllMSP" ClientIDMode="Static" runat="server"
																								Text="View" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllMSP" ClientIDMode="Static" runat="server"
																								Text="Print" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllMSP" ClientIDMode="Static" runat="server"
																								Text="Add" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllMSP" ClientIDMode="Static" runat="server"
																								Text="Edit" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllMSP" ClientIDMode="Static" runat="server"
																								Text="Delete" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>

																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpMSP" ID="clpMSP"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlMSP" ExpandControlID="ClpnlMSP"
																CollapseControlID="ClpnlMSP" Collapsed="False" ImageControlID="imgMSP"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>

													<%--  ========== End =========================--%>
													<tr>
														<td valign="top"></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkAudit1" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.QA_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlQualityAudit" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.QA_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblQualityAuditSelection"
																						class="clsLabelHeader">Quality Assurance</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgQualityAudit" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlQualityAudit" runat="server" Visible="<%# mRole.QA_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table21" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgAudit" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Quality Assurance Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllAudit" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Quality Assurance">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllAudit" ClientIDMode="Static" runat="server" Text="View"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllAudit" ClientIDMode="Static" runat="server" Text="Print"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllAudit" ClientIDMode="Static" runat="server" Text="Add"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllAudit" ClientIDMode="Static" runat="server" Text="Edit"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllAudit" ClientIDMode="Static" runat="server" Text="Delete"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpQualityAuditBehaviour" ID="clpQualityAudit"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlQualityAudit" ExpandControlID="ClpnlQualityAudit"
																CollapseControlID="ClpnlQualityAudit" Collapsed="False" ImageControlID="imgQualityAudit"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td valign="top"></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkMEL" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.MEL_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlMELSnag" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.MEL_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<asp:Label CssClass="clsLabelHeader" ID="lblMELSnagSelection" Style="vertical-align: middle; margin-left: 2px;"
																						runat="server" Text='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "ADD/Defect", "MEL/Snag") %>'></asp:Label>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgMELSnag" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlMELSnag" runat="server" Visible="<%# mRole.MEL_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table22" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgMEL" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights MEL Snag Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllMEL" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="MEL/Snag">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllMEL" ClientIDMode="Static" runat="server" Text="View"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllMEL" ClientIDMode="Static" runat="server" Text="Print"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllMEL" ClientIDMode="Static" runat="server" Text="Add" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllMEL" ClientIDMode="Static" runat="server" Text="Edit"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllMEL" ClientIDMode="Static" runat="server" Text="Delete"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpMELSnagBehaviour" ID="clpMELSnag" ClientIDMode="Static"
																runat="Server" TargetControlID="pnlMELSnag" ExpandControlID="ClpnlMELSnag" CollapseControlID="ClpnlMELSnag"
																Collapsed="False" ImageControlID="imgMELSnag" CollapsedSize="0" ExpandedText="(Hide Details...)"
																CollapsedText="(Show Details...)" ExpandedImage="~/images/collapse_blue.jpg"
																CollapsedImage="~/images/expand_blue.jpg" SuppressPostBack="false" />
														</td>
													</tr>

													<tr>
														<td valign="top"></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkDiscrepancy" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.Discrepancy_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlDiscrepancy" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.Discrepancy_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<asp:Label CssClass="clsLabelHeader" ID="lblDiscrepancy" Style="vertical-align: middle; margin-left: 2px;"
																						runat="server" Text="Discrepancy"></asp:Label>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgDiscrepancy" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>

													<tr>
														<td valign="top">
															<asp:Panel ID="pnlDiscrepancy" runat="server" Visible="<%# mRole.Discrepancy_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="TableDiscrepancy" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgDiscrepancy" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights MEL Snag Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllDiscrepancy" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Discrepancy">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllDiscrepancy" ClientIDMode="Static" runat="server" Text="View"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllDiscrepancy" ClientIDMode="Static" runat="server" Text="Print"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllDiscrepancy" ClientIDMode="Static" runat="server" Text="Add" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllDiscrepancy" ClientIDMode="Static" runat="server" Text="Edit"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllDiscrepancy" ClientIDMode="Static" runat="server" Text="Delete"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpDiscrepancyBehaviour" ID="CollapsiblePanelExtender2" ClientIDMode="Static"
																runat="Server" TargetControlID="pnlDiscrepancy" ExpandControlID="ClpnlDiscrepancy" CollapseControlID="ClpnlDiscrepancy"
																Collapsed="False" ImageControlID="imgDiscrepancy" CollapsedSize="0" ExpandedText="(Hide Details...)"
																CollapsedText="(Show Details...)" ExpandedImage="~/images/collapse_blue.jpg"
																CollapsedImage="~/images/expand_blue.jpg" SuppressPostBack="false" />
														</td>
													</tr>
													                                                    <%-- Sankalp 29/7/25 --%>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkCabinDefect" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.CabinDefect_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlCabinDefect" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.CabinDefect_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblCabinDefectSelection"
																						class="clsLabelHeader">
																						<asp:Label ID="Label5" runat="server" Text="Cabin Defect"></asp:Label>
																					</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgCabinDefect" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlCabinDefect" runat="server" Visible="<%# mRole.CabinDefect_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table462" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgCabinDefect" runat="server" AutoGenerateColumns="False"
																				ShowHeaderWhenEmpty="true" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Work Order Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleCabinDefect" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Cabin Defect">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllCabinDefect" ClientIDMode="Static" runat="server"
																								Text="View" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllCabinDefect" ClientIDMode="Static" runat="server"
																								Text="Print" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllCabinDefect" ClientIDMode="Static" runat="server"
																								Text="Add" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllCabinDefect" ClientIDMode="Static" runat="server"
																								Text="Edit" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllCabinDefect" ClientIDMode="Static" runat="server"
																								Text="Delete" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>

																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpCabinDefect" ID="clpCabinDefect"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlCabinDefect" ExpandControlID="ClpnlCabinDefect"
																CollapseControlID="ClpnlCabinDefect" Collapsed="False" ImageControlID="imgCabinDefect"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
                                                    <%-- Sankalp End --%>
													<tr>
														<td valign="top"></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkMPD" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.Maint_MPD_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlMPD" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.Maint_MPD_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="Span2" class="clsLabelHeader">MPD</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgMPD" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlMPD" runat="server" Visible="<%# mRole.Maint_MPD_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table32" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgMPD" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from MPD Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllMPD" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="MPD">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllMPD" ClientIDMode="Static" runat="server" Text="View"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllMPD" ClientIDMode="Static" runat="server" Text="Print"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllMPD" ClientIDMode="Static" runat="server" Text="Add" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllMPD" ClientIDMode="Static" runat="server" Text="Edit"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllMPD" ClientIDMode="Static" runat="server" Text="Delete"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpMPDBehaviour" ID="clpMPD" ClientIDMode="Static"
																runat="Server" TargetControlID="pnlMPD" ExpandControlID="ClpnlMPD" CollapseControlID="ClpnlMPD"
																Collapsed="False" ImageControlID="imgMPD" CollapsedSize="0" ExpandedText="(Hide Details...)"
																CollapsedText="(Show Details...)" ExpandedImage="~/images/collapse_blue.jpg"
																CollapsedImage="~/images/expand_blue.jpg" SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td valign="top"></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkCWP" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.Maint_CWP_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlCWP" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.Maint_CWP_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="Span3" class="clsLabelHeader">Component WorkPack</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgCWP" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlCWP" runat="server" Visible="<%# mRole.Maint_CWP_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table33" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgCWP" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from CWP Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllCWP" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Component WorkPack">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllCWP" ClientIDMode="Static" runat="server" Text="View"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllCWP" ClientIDMode="Static" runat="server" Text="Print"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllCWP" ClientIDMode="Static" runat="server" Text="Add" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllCWP" ClientIDMode="Static" runat="server" Text="Edit"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllCWP" ClientIDMode="Static" runat="server" Text="Delete"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpCWPBehaviour" ID="clpCWP" ClientIDMode="Static"
																runat="Server" TargetControlID="pnlCWP" ExpandControlID="ClpnlCWP" CollapseControlID="ClpnlCWP"
																Collapsed="False" ImageControlID="imgCWP" CollapsedSize="0" ExpandedText="(Hide Details...)"
																CollapsedText="(Show Details...)" ExpandedImage="~/images/collapse_blue.jpg"
																CollapsedImage="~/images/expand_blue.jpg" SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td valign="top"></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkDentBuckleChart" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.DentBuckleChart_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlDentBuckleChart" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.DentBuckleChart_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="Span4" class="clsLabelHeader">Dent and Buckle Chart</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgDentBuckleChart" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlDentBuckleChart" runat="server" Visible="<%# mRole.DentBuckleChart_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table34" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgDentBuckleChart" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Dent and Buckle Chart Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllDentBuckleChart" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Dent and Buckle Chart">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllDentBuckleChart" ClientIDMode="Static" runat="server"
																								Text="View" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllDentBuckleChart" ClientIDMode="Static" runat="server"
																								Text="Print" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllDentBuckleChart" ClientIDMode="Static" runat="server"
																								Text="Add" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllDentBuckleChart" ClientIDMode="Static" runat="server"
																								Text="Edit" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllDentBuckleChart" ClientIDMode="Static" runat="server"
																								Text="Delete" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Authorized">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAuthorizedAllDentBuckleChart" ClientIDMode="Static" runat="server"
																								Text="Authorized" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAuthorized" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedAuthorized") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpDentBuckleChartBehaviour" ID="clpDentBuckleChart"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlDentBuckleChart" ExpandControlID="ClpnlDentBuckleChart"
																CollapseControlID="ClpnlDentBuckleChart" Collapsed="False" ImageControlID="imgDentBuckleChart"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td valign="top"></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkHangar" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.Hangar_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlHangar" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.Hangar_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="Span6" class="clsLabelHeader">Hangar Planning</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgHangar" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlHangar" runat="server" Visible="<%# mRole.Hangar_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table36" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgHangar" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Hangar Planning Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllHangar" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Hangar">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllHangar" ClientIDMode="Static" runat="server" Text="View"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllHangar" ClientIDMode="Static" runat="server" Text="Print"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllHangar" ClientIDMode="Static" runat="server" Text="Add"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllHangar" ClientIDMode="Static" runat="server" Text="Edit"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllHangar" ClientIDMode="Static" runat="server" Text="Delete"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Authorized">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAuthorizedAllHangar" ClientIDMode="Static" runat="server" Text="Authorized"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAuthorized" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedAuthorized") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpHangarBehaviour" ID="clpHangar" ClientIDMode="Static"
																runat="Server" TargetControlID="pnlHangar" ExpandControlID="ClpnlHangar" CollapseControlID="ClpnlHangar"
																Collapsed="False" ImageControlID="imgHangar" CollapsedSize="0" ExpandedText="(Hide Details...)"
																CollapsedText="(Show Details...)" ExpandedImage="~/images/collapse_blue.jpg"
																CollapsedImage="~/images/expand_blue.jpg" SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td valign="top"></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkCompanyDocument" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.CompanyDocument_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlCompanyDocument" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.CompanyDocument_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="Span8" class="clsLabelHeader">Organisation Approval</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgCompanyDocument" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlCompanyDocument" runat="server" Visible="<%# mRole.CompanyDocument_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table41" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgCompanyDocument" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Organisation Approval Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllCompanyDocument" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Organisation Approval">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllCompanyDocument" ClientIDMode="Static" runat="server"
																								Text="View" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllCompanyDocument" ClientIDMode="Static" runat="server"
																								Text="Print" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllCompanyDocument" ClientIDMode="Static" runat="server"
																								Text="Add" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllCompanyDocument" ClientIDMode="Static" runat="server"
																								Text="Edit" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllCompanyDocument" ClientIDMode="Static" runat="server"
																								Text="Delete" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Authorized">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAuthorizedAllCompanyDocument" ClientIDMode="Static" runat="server"
																								Text="Authorized" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAuthorized" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedAuthorized") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpCompanyDocumentBehaviour" ID="clpCompanyDocument"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlCompanyDocument" ExpandControlID="ClpnlCompanyDocument"
																CollapseControlID="ClpnlCompanyDocument" Collapsed="False" ImageControlID="imgCompanyDocument"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td valign="top"></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkMaintenanceReports" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.Maint_Reports_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="clpnlMaintenanceReports" runat="server" Visible="<%# mRole.Maint_Reports_Modules.Count > 0 %>"
																			CssClass="clsCollapsePnl" Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblMaintenanceReportsSelection"
																						class="clsLabelHeader">Maintenance Reports</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgMaintenanceReports" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlMaintenanceReports" runat="server" Visible="<%# mRole.Maint_Reports_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table23" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgMaintenanceReports" runat="server" AutoGenerateColumns="False"
																				ShowHeaderWhenEmpty="true" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Reports Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Reports">
																						<HeaderStyle Width="420px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllMaintenanceReports" ClientIDMode="Static" runat="server"
																								Text="View" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpMaintenanceReportsBehaviour" ID="clpMaintenanceReports"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlMaintenanceReports"
																ExpandControlID="ClpnlMaintenanceReports" CollapseControlID="ClpnlMaintenanceReports"
																Collapsed="False" ImageControlID="imgMaintenanceReports" CollapsedSize="0" ExpandedText="(Hide Details...)"
																CollapsedText="(Show Details...)" ExpandedImage="~/images/collapse_blue.jpg"
																CollapsedImage="~/images/expand_blue.jpg" SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td valign="top"></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkTools" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.Tool_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlTools" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.Tool_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblToolsSelection" class="clsLabelHeader">User Utilities</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgTools" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlTools" runat="server" Visible="<%# mRole.Tool_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table24" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgTools" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from User Utilities Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="User Utilities">
																						<HeaderStyle Width="420px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllTools" ClientIDMode="Static" runat="server" Text="View"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpToolsBehaviour" ID="clpTools" ClientIDMode="Static"
																runat="Server" TargetControlID="pnlTools" ExpandControlID="ClpnlTools" CollapseControlID="ClpnlTools"
																Collapsed="False" ImageControlID="imgTools" CollapsedSize="0" ExpandedText="(Hide Details...)"
																CollapsedText="(Show Details...)" ExpandedImage="~/images/collapse_blue.jpg"
																CollapsedImage="~/images/expand_blue.jpg" SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td valign="top"></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkAdminUtilitiess" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.AdminUtilities_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlAdminUtilitiess" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.AdminUtilities_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblAdminUtilitiessSelection"
																						class="clsLabelHeader">Admin Utilities</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgAdminUtilitiess" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlAdminUtilitiess" runat="server" Visible="<%# mRole.AdminUtilities_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table44" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgAdminUtilitiess" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Admin Utilities Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Admin Utilities">
																						<HeaderStyle Width="420px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllAdminUtilitiess" ClientIDMode="Static" runat="server"
																								Text="View" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpAdminUtilitiessBehaviour" ID="clpAdminUtilitiess"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlAdminUtilitiess" ExpandControlID="ClpnlAdminUtilitiess"
																CollapseControlID="ClpnlAdminUtilitiess" Collapsed="False" ImageControlID="imgAdminUtilitiess"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td valign="top"></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkInfoDisplay" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.InfoDisplay_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlInfoDisplay" runat="server" CssClass="clsCollapsePnl" Visible="<%# mRole.InfoDisplay_Modules.Count > 0 %>"
																			Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="Span5" class="clsLabelHeader">Info Display</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgInfoDisplay" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlInfoDisplay" runat="server" Visible="<%# mRole.InfoDisplay_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table35" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgInfoDisplay" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																				CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Info Display Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllInfoDisplay" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Info Display">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllInfoDisplay" ClientIDMode="Static" runat="server" Text="View"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllInfoDisplay" ClientIDMode="Static" runat="server" Text="Print"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllInfoDisplay" ClientIDMode="Static" runat="server" Text="Add"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllInfoDisplay" ClientIDMode="Static" runat="server" Text="Edit"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllInfoDisplay" ClientIDMode="Static" runat="server" Text="Delete"
																								onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Authorized">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAuthorizedAllInfoDisplay" ClientIDMode="Static" runat="server"
																								Text="Authorized" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAuthorized" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedAuthorized") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpInfoDisplayBehaviour" ID="clpInfoDisplay"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlInfoDisplay" ExpandControlID="ClpnlInfoDisplay"
																CollapseControlID="ClpnlInfoDisplay" Collapsed="False" ImageControlID="imgInfoDisplay"
																CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
																ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
																SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td valign="top"></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkMaintenanceDashboards" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.MaintenanceDashboard_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlMaintenanceDashboards" runat="server" CssClass="clsCollapsePnl"
																			Visible="<%# mRole.MaintenanceDashboard_Modules.Count > 0 %>" Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblMaintenanceDashboardsSelection"
																						class="clsLabelHeader">Maintenance Dashboards</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgMaintenanceDashboards" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlMaintenanceDashboard" runat="server" Visible="<%# mRole.MaintenanceDashboard_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table38" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgMaintenanceDashboard" runat="server" AutoGenerateColumns="False"
																				ShowHeaderWhenEmpty="true" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Maintenance Dashboard Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Maintenance Dashboard">
																						<HeaderStyle Width="420px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllMaintenanceDashboard" ClientIDMode="Static" runat="server"
																								Text="View" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpMaintenanceDashboardBehaviour" ID="clpMaintenanceDashboard"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlMaintenanceDashboard"
																ExpandControlID="ClpnlMaintenanceDashboards" CollapseControlID="ClpnlMaintenanceDashboards"
																Collapsed="False" ImageControlID="imgMaintenanceDashboard" CollapsedSize="0"
																ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)" ExpandedImage="~/images/collapse_blue.jpg"
																CollapsedImage="~/images/expand_blue.jpg" SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td valign="top"></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkInventoryDashboards" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.InventoryDashboard_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="ClpnlInventoryDashboards" runat="server" CssClass="clsCollapsePnl"
																			Visible="<%# mRole.InventoryDashboard_Modules.Count > 0 %>" Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="lblInventoryDashboardsSelection"
																						class="clsLabelHeader">Inventory Dashboards</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgInventoryDashboards" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="pnlInventoryDashboard" runat="server" Visible="<%# mRole.InventoryDashboard_Modules.Count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table39" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgInventoryDashboard" runat="server" AutoGenerateColumns="False"
																				ShowHeaderWhenEmpty="true" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Inventory Dashboard Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Inventory Dashboard">
																						<HeaderStyle Width="420px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllInventoryDashboard" ClientIDMode="Static" runat="server"
																								Text="View" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="clpInventoryDashboardBehaviour" ID="clpInventoryDashboard"
																ClientIDMode="Static" runat="Server" TargetControlID="pnlInventoryDashboard"
																ExpandControlID="ClpnlInventoryDashboards" CollapseControlID="ClpnlInventoryDashboards"
																Collapsed="False" ImageControlID="imgInventoryDashboard" CollapsedSize="0" ExpandedText="(Hide Details...)"
																CollapsedText="(Show Details...)" ExpandedImage="~/images/collapse_blue.jpg"
																CollapsedImage="~/images/expand_blue.jpg" SuppressPostBack="false" />
														</td>
													</tr>
													<tr>
														<td valign="top"></td>
													</tr>
													<tr>
														<td valign="top">
															<table border="0" cellpadding="0" cellspacing="0" width="100%">
																<tr class="clsCollapsePnl">
																	<td width="25px">
																		<asp:CheckBox ID="chkComponentReservation" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
																			Visible="<%# mRole.ComponentReservation_Modules.Count > 0 %>" Text="" />
																	</td>
																	<td width="100%">
																		<asp:Panel ID="pnlcplComponentReservation" runat="server" CssClass="clsCollapsePnl"
																			Visible="<%# mRole.ComponentReservation_Modules.Count > 0 %>" Style="border: none;">
																			<div>
																				<div style="float: left; vertical-align: middle;">
																					<span style="vertical-align: middle; margin-left: 2px;" id="Span9" class="clsLabelHeader">Component Reservation</span>
																				</div>
																				<div style="float: right; vertical-align: middle; margin-right: 5px;">
																					<image id="imgComponentReservation" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
																				</div>
																			</div>
																		</asp:Panel>
																	</td>
																</tr>
															</table>
														</td>
													</tr>
													<tr>
														<td valign="top">
															<asp:Panel ID="cplComponentReservation" runat="server" Visible="<%# mRole.ComponentReservation_Modules.count > 0 %>"
																Style="max-height: 200px; overflow-y: auto; overflow: auto; overflow-x: hidden;">
																<table id="Table42" cellpadding="0" cellspacing="0" border="0" width="100%">
																	<tr>
																		<td>
																			<asp:GridView ID="dgComponentReservation" runat="server" AutoGenerateColumns="False"
																				ShowHeaderWhenEmpty="true" CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" ToolTip="Select Rights from Component Reservation Permission">
																				<AlternatingRowStyle CssClass="clsdgAltItem" />
																				<RowStyle CssClass="clsdgItem" />
																				<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																				<Columns>
																					<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:CheckBox ID="chkSingleAllComponentReservation" runat="server" onclick="CheckUncheckSingleRow(this)" />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="ModuleDescription" HeaderText="Component Reservation">
																						<HeaderStyle Width="300px" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="View">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkViewAllCompanyDocument" ClientIDMode="Static" runat="server"
																								Text="View" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkView" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedView") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Print">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkPrintAllCompanyDocument" ClientIDMode="Static" runat="server"
																								Text="Print" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkPrint" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedPrint") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Add">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkAddAllCompanyDocument" ClientIDMode="Static" runat="server"
																								Text="Add" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkAdd" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedNew") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Edit">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkEditAllCompanyDocument" ClientIDMode="Static" runat="server"
																								Text="Edit" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkEdit" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedEdit") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
																						HeaderText="Delete">
																						<HeaderTemplate>
																							<asp:CheckBox ID="chkDeleteAllCompanyDocument" ClientIDMode="Static" runat="server"
																								Text="Delete" onclick="CheckUncheck(this);" />
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="chkDelete" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsSelectedDelete") %>' />
																						</ItemTemplate>
																					</asp:TemplateField>
																					<asp:BoundField DataField="IsHideOnUI" HeaderStyle-CssClass="hideGridColumn" HeaderText="IsHideOnUI"
																						ItemStyle-CssClass="hideGridColumn">
																						<HeaderStyle CssClass="hideGridColumn" />
																						<ItemStyle CssClass="hideGridColumn" />
																					</asp:BoundField>
																				</Columns>
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</asp:Panel>
															<cc2:CollapsiblePanelExtender BehaviorID="cplComponentReservation" ID="CollapsiblePanelExtender1"
																ClientIDMode="Static" runat="Server" TargetControlID="cplComponentReservation"
																ExpandControlID="pnlcplComponentReservation" CollapseControlID="pnlcplComponentReservation"
																Collapsed="False" ImageControlID="imgComponentReservation" CollapsedSize="0"
																ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)" ExpandedImage="~/images/collapse_blue.jpg"
																CollapsedImage="~/images/expand_blue.jpg" SuppressPostBack="false" />
														</td>
													</tr>
												</table>
											</td>
										</tr>
									</table>
								</ContentTemplate>
							</asp:UpdatePanel>
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

	<script type="text/javascript">
		Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

			$("#dgMasters tr").each(function () {
				if ($(this).find("td:eq(6)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});

			$("#dgRequisition tr").each(function () {
				if ($(this).find("td:eq(7)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});
			$("#dgNewRequisition tr").each(function () {
				if ($(this).find("td:eq(7)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});
			$("#dgPurchaseEnquiry tr").each(function () {
				if ($(this).find("td:eq(7)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});

			$("#dgPurchaseQuotation tr").each(function () {
				if ($(this).find("td:eq(7)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});

			$("#dgPurchaseOrder tr").each(function () {
				if ($(this).find("td:eq(7)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});

			$("#dgGoodsReceipt tr").each(function () {
				if ($(this).find("td:eq(7)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});

			$("#dgGoodsIssue tr").each(function () {
				if ($(this).find("td:eq(7)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});

			$("#dgPurchaseInvoice tr").each(function () {
				if ($(this).find("td:eq(7)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});
			$("#dgPaymentAdvice tr").each(function () {
				if ($(this).find("td:eq(7)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});
			$("#dgSalesModules tr").each(function () {
				if ($(this).find("td:eq(7)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});

			$("#dgCalibration tr").each(function () {
				if ($(this).find("td:eq(6)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});

			$("#dgWorkInvoice tr").each(function () {
				if ($(this).find("td:eq(7)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});

			$("#dgLineMaintenance tr").each(function () {
				if ($(this).find("td:eq(7)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});

			$("#dgExportInvoice tr").each(function () {
				if ($(this).find("td:eq(7)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});

			$("#dgReliability tr").each(function () {
				if ($(this).find("td:eq(6)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});
			$("#dgInventoryReports tr").each(function () {
				if ($(this).find("td:eq(2)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");

				}
			});
			$("#dgDocumentLocker tr").each(function () {
				if ($(this).find("td:eq(6)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");

				}
			});
			$("#dgADSBReviewMeeting tr").each(function () {
				if ($(this).find("td:eq(7)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");

				}
			});

			$("#dgMaintMasters tr").each(function () {
				if ($(this).find("td:eq(6)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});
			$("#dgMaintenance tr").each(function () {
				if ($(this).find("td:eq(7)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});
			$("#dgManual tr").each(function () {
				if ($(this).find("td:eq(6)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});
			$("#dgSpareMaint tr").each(function () {
				if ($(this).find("td:eq(8)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});
			$("#dgWorkOrder tr").each(function () {
				if ($(this).find("td:eq(8)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});
			$("#dgMROContract tr").each(function () {
				if ($(this).find("td:eq(7)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});
			$("#dgWorkOrderInvoice tr").each(function () {
				if ($(this).find("td:eq(7)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});
			$("#dgAudit tr").each(function () {
				if ($(this).find("td:eq(6)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});

			$("#dgMEL tr").each(function () {
				if ($(this).find("td:eq(6)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});

			$("#dgMPD tr").each(function () {
				if ($(this).find("td:eq(6)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});

			$("#dgCWP tr").each(function () {
				if ($(this).find("td:eq(6)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});

			$("#dgDentBuckleChart tr").each(function () {
				if ($(this).find("td:eq(7)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});
			$("#dgHangar tr").each(function () {
				if ($(this).find("td:eq(7)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});
			$("#dgCompanyDocument tr").each(function () {
				if ($(this).find("td:eq(7)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});
			$("#dgMaintenanceReports tr").each(function () {
				if ($(this).find("td:eq(2)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});

			$("#dgTools tr").each(function () {
				if ($(this).find("td:eq(2)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
					//$(this).closest('tr').find('input[type="checkbox"]').prop('disabled', true);
					//$(this).closest('tr').find('input[type="checkbox"]').attr('checked', false);
					//$(this).closest('tr').css('display', 'none');
				}
			});
			$("#dgAdminUtilitiess tr").each(function () {
				if ($(this).find("td:eq(2)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});
			$("#dgInfoDisplay tr").each(function () {
				if ($(this).find("td:eq(7)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});
			$("#dgMaintenanceDashboard tr").each(function () {
				if ($(this).find("td:eq(2)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});
			$("#dgInventoryDashboard tr").each(function () {
				if ($(this).find("td:eq(2)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});
			$("#dgComponentReservation tr").each(function () {
				if ($(this).find("td:eq(6)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});
			$("#dgMSP tr").each(function () {
				if ($(this).find("td:eq(6)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});
			$("#dgEmpCAAuthorization tr").each(function () {
				if ($(this).find("td:eq(6)").text() == "True") {
					$(this).css("background-color", "orange");
					$(this).css("text-decoration", "line-through");
				}
			});
		});
	</script>
	<script type="text/javascript">

		function CheckUncheck(chkBoxAll) {

			var str = chkBoxAll.id;
			var status = $("#" + str).is(":checked");

			console.log("Parent Checkbox ID:", str);
			console.log("Parent Checkbox Status:", status);

			// Extract the gridViewId by finding the index of "All" and taking the preceding substring
			var gridViewId = str.substring(0, str.indexOf("All"));
			console.log("Extracted gridViewId:", gridViewId);

			// Find the closest table row (<tr>) to the checkbox
			var gridViewRow = $(chkBoxAll).closest("tr");
			console.log("Closest Row:", gridViewRow);

			// Find all subsequent rows within the same table
			var subsequentRows = gridViewRow.nextAll("tr");
			console.log("Subsequent Rows:", subsequentRows);

			subsequentRows.find(":checkbox[id*='" + gridViewId + "']").each(function () {
				console.log("Processing Child Checkbox:", this);
				if (status) {
					$(this).prop("checked", true);
					console.log("Child Checkbox Checked");
				} else {
					$(this).prop("checked", false);
					console.log("Child Checkbox Unchecked");
				}
			});

		}

		function CheckUncheckSingleRow(chkBoxAll) {

			var str = chkBoxAll.id;
			var status = $("#" + str).attr("checked");
			var row = $(chkBoxAll).closest("tr");

			row.find("td").find('input[type="checkbox"]:checked').each(function () {
				if (status == "checked") {
					row.find("td").find('input[type="checkbox"]').attr("checked", status);
				}
				else {
					row.find("td").find('input[type="checkbox"]').removeAttr("checked");
				}
			});

		}

	</script>

</body>
</html>
