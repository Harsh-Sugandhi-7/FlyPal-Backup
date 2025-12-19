<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfStore_Ajax.aspx.vb"
	EnableViewState="True" Inherits="Flypal.wfStore_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<title>Store Information</title>
	<link id="MainStyle" type="text/css" rel="stylesheet">
	<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	<script language="javascript">
		function OpenLocation(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
		}
	</script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
	<form id="wfgroup" method="post" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1">
		</asp:ScriptManager>
		<%--AJAX- Add MSGBox Control--%>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<table class="clstablelistout" id="tblmain">
			<tr>
				<td>
					<asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
						<table id="tblInner" class="clstablelistin">
							<tr>
								<td class="clsFormHeader1Newstyle">
									<table width="100%">
										<tr>
											<td>
												<asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">
															Store Information [New]
														</asp:Label>
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
											<td align="right">
												<table>
													<tr>
														<td>
															<asp:Button ID="btnAdd" runat="server" CausesValidation="False"
																CssClass="clsbtnH clsinfoH"
																Text="New" ToolTip="Click to add the new Store" />
														</td>														
														<td>
															<asp:UpdatePanel ID="upnlSave" runat="server" UpdateMode="Conditional">
																<ContentTemplate>
																	<asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH"
																		Text="Save" ToolTip="Click to save the Store Information" />
																</ContentTemplate>
															</asp:UpdatePanel>
														</td>
														<td>
															<asp:Button ID="btnBack" runat="server" CausesValidation="False"
																CssClass="clsbtnH clsinfoH" Text="Close"
																ToolTip="Click to close Store Information screen" />
														</td>
													</tr>
												</table>
											</td>
										</tr>
									</table>
								</td>
								<%--Added by Harsh on 15th July 2024 for FLYPAL 1745--%>
								<td id="tdFavICN" align="center">
									<span id="spFavICN">
										<i id="favICN" runat="server" onclick="fnMarkFavouriteUnFavourite(this)"
											class="fa fa-star fa-spin fa-5x circle-icon"></i>
									</span>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary"></asp:ValidationSummary>
											<asp:CustomValidator ID="cvLocation" runat="server" CssClass="clsLabelAuto" ControlToValidate="cmbLocation"
												Display="None" ErrorMessage="Location Required" OnServerValidate="customvalidate"></asp:CustomValidator>
											<asp:CustomValidator ID="cvName" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtName"
												Display="None" ErrorMessage="Store Name Required " OnServerValidate="customvalidate"
												ValidateEmptyText="true"></asp:CustomValidator>
											<asp:CustomValidator ID="cvVendor" runat="server" CssClass="clsLabelAuto" ControlToValidate="cmbVendorList"
												Display="None" ErrorMessage="Vendor Required " OnServerValidate="customvalidate"></asp:CustomValidator>
											<asp:CustomValidator ID="cvDate" runat="server" CssClass="clsLabelAuto" Display="None"
												ErrorMessage=""></asp:CustomValidator>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlStoreDetails" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table width="100%">
												<tr>
													<td colspan="2">
														<fieldset class="clsFieldSet" style="border-width: 1px">
															<table>
																<tr>
																	<td align="left">
																		<span id="lblName1" class="clsLabelStar">*</span>
																	</td>
																	<td align="left" style="width: 60px;">
																		<asp:Label ID="lblName" runat="server" CssClass="clsLabelAuto" Width="61px">Name</asp:Label>
																	</td>
																	<td>
																		<table id="Table2">
																			<tr>
																				<td>
																					<asp:TextBox ID="txtName" runat="server" CssClass="clsTextBoxTagSearch" MaxLength="50"
																						Text="<%# mStore.Name %>" ToolTip="Enter Store Name" Width="272px"></asp:TextBox>
																				</td>
																			</tr>
																		</table>
																	</td>
																</tr>
																<tr>
																	<td align="left">
																		<span id="lblLocation1" class="clsLabelStar">*</span>
																	</td>
																	<td align="left" style="width: 60px;">
																		<span id="lblLocation" class="clsLabelAuto">Station</span>
																	</td>
																	<td>
																		<table>
																			<tr>
																				<td>
																					<asp:DropDownList ID="cmbLocation" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="Name"
																						DataValueField="ID" SelectedValue="<%# mStore.LocationID %>" Width="275px">
																					</asp:DropDownList>
																				</td>
																				<td>
																					<asp:ImageButton ID="imgLocation" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
																						Width="24px" ToolTip="Click to Add New Location" CausesValidation="True"></asp:ImageButton>
																				</td>
																			</tr>
																		</table>
																	</td>
																</tr>
																<tr>
																	<td></td>
																	<td style="width: 60px;"></td>
																	<td>
																		<asp:CheckBox ID="chkIsValued" runat="server" Checked="<%# mStore.IsValued %>" CssClass="clsLabelAuto"
																			Text="Is this Valued Store ?" />
																	</td>
																</tr>
																<tr>
																	<td></td>
																	<td colspan="2" align="left">
																		<asp:UpdatePanel ID="upnlCustomer" runat="server" UpdateMode="Conditional">
																			<ContentTemplate>
																				<table>
																					<tr>
																						<td style="width: auto;"></td>
																						<td>
																							<asp:CheckBox ID="chkIsOwnedByCustomer" runat="server" Checked="<%# mStore.IsOwnedByCustomer %>"
																								CssClass="clsLabelAuto" Text="Is this Store owned by Customer ?" AutoPostBack="True" />
																						</td>
																					</tr>
																					<tr>
																						<td align="left" style="width: 60px;">
																							<span id="Label1" class="clsLabelAuto" width="58px">Customer</span>
																						</td>
																						<td>
																							<table>
																								<tr>
																									<td>
																										<asp:DropDownList ID="cmbVendorList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
																											DataTextField="Name" DataValueField="ID" Enabled="<%# mStore.IsOwnedByCustomer %>"
																											SelectedValue="<%# mStore.VendorID %>" Width="275px">
																										</asp:DropDownList>
																									</td>
																									<td align="left">
																										<asp:ImageButton ID="ImgVendor" runat="server" ImageUrl="~/images/plus1.png" Height="22px"
																											Width="24px" ToolTip="Click to Add New Customer" CausesValidation="True"></asp:ImageButton>
																									</td>
																								</tr>
																							</table>
																						</td>
																					</tr>
																				</table>
																			</ContentTemplate>
																		</asp:UpdatePanel>
																	</td>
																</tr>
																<tr>
																	<td></td>
																	<td colspan="2" align="left">
																		<asp:UpdatePanel ID="upnlNotInUseDetails" runat="server" UpdateMode="Conditional">
																			<ContentTemplate>
																				<table>
																					<tr>
																						<td style="width: auto;"></td>
																						<td>
																							<asp:CheckBox ID="chkNotInUse" runat="server" AutoPostBack="True" Checked="<%# mStore.NotInUse %>"
																								CssClass="clsLabelAuto" Text="Store not in use" Width="184px" />
																						</td>
																					</tr>
																					<tr>
																						<td>
																							<asp:Label ID="lblDate" runat="server" CssClass="clsLabelAuto" Width="58px">Date</asp:Label>
																						</td>
																						<td>
																							<table>
																								<tr>
																									<td>
																										<asp:TextBox ID="txtNotInUseDate" CssClass="clsTextBoxTagSearchDate" ClientIDMode="Static"
																											runat="server"></asp:TextBox>
																										<cc2:CalendarExtender ID="calNotInUse_CalendarExtender" runat="server" CssClass="cal_Theme1"
																											Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtNotInUseDate"></cc2:CalendarExtender>
																										<cc2:TextBoxWatermarkExtender TargetControlID="txtNotInUseDate" ID="NotInUseDate_watermarkextender"
																											ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
																									</td>
																								</tr>
																							</table>
																						</td>
																					</tr>
																				</table>
																			</ContentTemplate>
																		</asp:UpdatePanel>
																	</td>
																</tr>
															</table>
														</fieldset>
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td>
									<table>
										<tr>
											<td>
												<asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<table>
															<tr>
																<td>
																	<asp:Label ID="lblResult" runat="server" CssClass="clsLabelHeader">Store List</asp:Label>
																</td>
															</tr>
															<tr>
																<td>
																	<asp:GridView ID="dgStore" runat="server" AllowSorting="True"
																		AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
																		CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5"
																		AllowPaging="True" PageSize="10">
																		<AlternatingRowStyle CssClass="clsdgAltItem" />
																		<RowStyle CssClass="clsdgItem" />
																		<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
																		<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
																		<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
																		<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
																		<Columns>
																			<asp:BoundField DataField="Id" HeaderText="Id" Visible="False"></asp:BoundField>
																			<asp:BoundField DataField="Name" HeaderText="Store" SortExpression="Name">
																				<HeaderStyle HorizontalAlign="Left" />
																			</asp:BoundField>
																			<asp:BoundField DataField="LocationName" HeaderText="Station" SortExpression="LocationName">
																				<HeaderStyle HorizontalAlign="Left" />
																			</asp:BoundField>
																			<asp:TemplateField HeaderText="Valued Store">
																				<HeaderStyle Wrap="true" Width="20px" />
																				<ItemTemplate>
																					<asp:CheckBox ID="ChkValued" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsValued") %>'
																						Enabled="False" />
																				</ItemTemplate>
																				<ItemStyle HorizontalAlign="Center" />
																			</asp:TemplateField>
																			<asp:TemplateField HeaderText="Owned By Customer?">
																				<HeaderStyle Wrap="true" Width="20px" HorizontalAlign="Left" />
																				<ItemTemplate>
																					<asp:CheckBox ID="ChkCustomer" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsOwnedByCustomer") %>'
																						Enabled="False" />
																				</ItemTemplate>
																				<ItemStyle HorizontalAlign="Center" />
																			</asp:TemplateField>
																			<asp:BoundField DataField="VendorName" HeaderText="Customer" SortExpression="VendorName">
																				<HeaderStyle HorizontalAlign="Left" />
																			</asp:BoundField>
																			<asp:TemplateField HeaderText="Not In Use">
																				<HeaderStyle Wrap="true" Width="40px" HorizontalAlign="Left" />
																				<ItemTemplate>
																					<asp:CheckBox ID="ChkNotInUse" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "NotInUse") %>'
																						Enabled="False" />
																				</ItemTemplate>
																				<ItemStyle HorizontalAlign="Center" />
																			</asp:TemplateField>
																			<asp:BoundField DataField="NotInUseDateFormatted" HeaderText="Not In Use Date">
																				<HeaderStyle HorizontalAlign="Left" Wrap="true" Width="60px" />
																				<ItemStyle Wrap="false" />
																			</asp:BoundField>
																			<asp:ButtonField CommandName="StoreTag" DataTextField="StoreTagsCountDisp" HeaderText="Add/Edit Store Tag"></asp:ButtonField>
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
																										<asp:ImageButton ID="editICN" class="actionICNS" runat="server"
																											CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																											ToolTip="Click to Edit record"
																											CommandName="EditRec" ImageUrl="~/images/edit.png" />
																									</td>

																									<td>
																										<asp:ImageButton ID="deleteICN" class="actionICNS  largerActionICNS" runat="server"
																											CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																											ToolTip="Click to Delete record"
																											CommandName="DeleteRec" ImageUrl="~/images/delete.png" />
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
													</ContentTemplate>
												</asp:UpdatePanel>

											</td>
										</tr>
										<tr>
											<td align="right">
												<asp:UpdatePanel ID="upnlClose" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<table>
															<tr>
																<td>
																	<asp:Button ID="hdnBtnAddUserMappingwithStore" ClientIDMode="Static" runat="server"
																		Text="..." CausesValidation="False" Style="display: none;"></asp:Button>
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
								<td colspan="2" align="right">
									<asp:UpdatePanel ID="upnlFavIcnBtn" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table>
												<tr>
													<%--Added by Harsh on 15th July 2024 for FLYPAL 1745--%>
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

		<!-- Store Tag Modal PopUp -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyStoreTag" Text="Dummy Store Tag" />
		</div>
		<asp:Panel runat="server" ID="pnlPopUp" Style="display: none">
			<div>
				<table class="clstablelistout" id="Table1">
					<tr>
						<td>
							<asp:UpdatePanel runat="server" ID="upnlStoreTag" UpdateMode="Conditional">
								<ContentTemplate>
									<table id="TABLE1" class="clstablelistin">
										<tr>
											<td class="clsFormHeader1Newstyle" colspan="4">
												<table width="100%">
													<tr>
														<td>
															<asp:Label ID="lblTitleStoreTag" CssClass="clsFormHeader" runat="server">Store Tag [New]</asp:Label>
														</td>
														<td align="right">
															<asp:Button ID="btnSaveStoreTag" CssClass="clsbtnH clsinfoH"
																runat="server" ToolTip="Click to save the store tag information"
																Text="Add" ValidationGroup="valGrpChild"></asp:Button>
															<asp:Button ID="btnCloseStoreTag" CssClass="clsbtnH clsinfoH"
																runat="server" ToolTip="Click to close store tag screen"
																Text="Close" CausesValidation="False"></asp:Button>
														</td>
													</tr>
												</table>
											</td>
										</tr>
										<tr>
											<td colspan="4">
												<asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="clsValidationSummary"
													ValidationGroup="valGrpChild"></asp:ValidationSummary>
												<asp:CustomValidator ID="cvItemTag" runat="server" ClientValidationFunction="ValidateCurrency"
													ValidationGroup="valGrpChild" Display="None" ControlToValidate="cmbItemTag" ErrorMessage="Select tag from the list."></asp:CustomValidator>
												<script type="text/javascript">
													function ValidateCurrency(source, args) {
														args.IsValid = false;
														var dd = $get("cmbItemTag");
														if (dd.selectedIndex != 0) {
															args.IsValid = true;
															return;
														}
													}

												</script>
											</td>
										</tr>
										<tr>
											<td colspan="4">
												<span id="lblStoreTags" class="clsLabelHeader">Store Tags</span>
											</td>
										</tr>
										<tr>
											<td>
												<span id="lblTagStar" class="clsLabelStar">*</span>
											</td>
											<td>
												<span id="lblTag" class="clsLabelAuto">Tag</span>
											</td>
											<td>
												<asp:DropDownList ID="cmbItemTag" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
													ClientIDMode="Static" DataTextField="Name">
												</asp:DropDownList>
											</td>
											<td></td>
										</tr>
										<tr>
											<td colspan="3"></td>
										</tr>
										<tr>
											<td colspan="4">
												<asp:Label ID="lblResultStoreTag" runat="server" CssClass="clsLabelHeader"></asp:Label>
											</td>
										</tr>
										<tr>
											<td colspan="4">
												<asp:GridView ID="dgStoreTagList" runat="server" AutoGenerateColumns="False"
													ShowHeaderWhenEmpty="true" AllowPaging="True" PageSize="10"
													CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5">
													<AlternatingRowStyle CssClass="clsdgAltItem" />
													<RowStyle CssClass="clsdgItem" />
													<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
													<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
													<PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
													<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
													<Columns>
														<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
														<asp:BoundField Visible="False" DataField="StoreID" SortExpression="StoreID" HeaderText="StoreID">
															<HeaderStyle></HeaderStyle>
														</asp:BoundField>
														<asp:BoundField DataField="ItemTagName" HeaderText="Tag">
															<HeaderStyle CssClass="TextBreak" HorizontalAlign="left" Width="300px"></HeaderStyle>
															<ItemStyle CssClass="TextBreak" HorizontalAlign="left" Width="300px" Wrap="true" />
														</asp:BoundField>
														<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
															<HeaderStyle HorizontalAlign="Center" CssClass="TextBreak" Width="100px"/>
															<ItemStyle HorizontalAlign="Center" Wrap="true" Width="100px" />
															<ItemTemplate>
																<div id="dropDownImg" class="dropdown">
																	<asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn" />
																	<div id="dropdownICN-content" class="dropdownbtn-content">
																		<table id="dropdown-content" class="clsGridNew_Ajax">
																			<tr>
																				<td>
																					<asp:ImageButton ID="editICN" class="actionICNS" runat="server"
																						CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																						ToolTip="Click to Edit record"
																						CommandName="EditRec" ImageUrl="~/images/edit.png" />
																				</td>

																				<td>
																					<asp:ImageButton ID="deleteICN" class="actionICNS  largerActionICNS" runat="server"
																						CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
																						ToolTip="Click to Delete record"
																						CommandName="DeleteRec" ImageUrl="~/images/delete.png" />
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
											<td colspan="4" align="right">
												<table id="Table3" border="0" cellspacing="1" cellpadding="1">
													<tr>
														<td></td>
													</tr>
												</table>
											</td>
										</tr>
									</table>
								</ContentTemplate>
							</asp:UpdatePanel>
						</td>
					</tr>
				</table>
			</div>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="lnkStoreTagCount_ModalPopupExtender" runat="server" TargetControlID="btnDummyStoreTag"
			PopupControlID="pnlPopUp" BackgroundCssClass="clsModalPopupBG" BehaviorID="ModalBehaviourID">
		</cc2:ModalPopupExtender>
		<%--call parent function after completing subroutine..(when page open as popup)--%>
		<script type="text/javascript">
			function CallParentCallback() {
				parent.ParentCallBackFunction();
				return false;
			}
		</script>
		<%--End--%>
		<%--Set page layout when open as popup aspx page--%>
		<script type="text/javascript">
    <% Dim mopen As String = Request.QueryString("Type") %>
     <% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

			$(document).ready(function () {
				SetPageLayout();
				if ($.browser.msie) {
					parent.IFrameATAStateComplete();
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
		<!-- User Mapping With Store Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyUserMappingwithStore" Text="Dummy User Mapping With Store"
				ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlPopupUserMappingwithStore" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="iPopupUserMappingwithStore" frameborder="0" allowtransparency="true" height="100%"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupUserMappingwithStore" runat="server" TargetControlID="btnDummyUserMappingwithStore"
			PopupControlID="pnlPopupUserMappingwithStore" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameUserMappingwithStoreStateComplete() {
				$("#btnDummyUserMappingwithStore").click();
			}
			function OpenToAddUserMappingwithStore() {
				try {
					$("#iPopupUserMappingwithStore").attr("src", "wfUserMappingwithStore_Ajax.aspx?Type=pup");
					$("#btnDummyUserMappingwithStore").click();
					return false;
				} catch (e) {
					alert(e);
				}
			}
		</script>
		<script type="text/javascript">
			function ParentCallBackFunctionForUserMappingwithStore() {
				var UserMappingwithStoreWindow = $find("<%=mdlPopupUserMappingwithStore.ClientID %>");
				//close Training Detail popup window
				UserMappingwithStoreWindow.hide();
				$("#iPopupUserMappingwithStore").attr("src", "JavaScript:''");
				//call ata image button
				$("#hdnBtnAddUserMappingwithStore").click();
			}
		</script>
		<!-- End-->

		<%--Added by Harsh on 15th July 2024 for FLYPAL 1745--%>
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

		<!-- Highlight DropDownList Item Color-->
		<script type="text/javascript">
			Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
				var ddCustomer = document.getElementById("cmbVendorList");
				if (ddCustomer != null) {
					if (ddCustomer.disabled == false) {
						var j = 0;
              <% For Each item2 In mVendorList%>
                <% If item2.NotInUse = "True" Then%>
						ddCustomer[j].style.cssText = "font-weight: bold;background-color: #FF0000;color: #FFFFFF;"
                <% End If%>
						j = j + 1;
             <% Next%>
					}
				}
			});
		</script>
		<!-- End Highlight DropDownList Item Color-->

	</form>

</body>
</html>
