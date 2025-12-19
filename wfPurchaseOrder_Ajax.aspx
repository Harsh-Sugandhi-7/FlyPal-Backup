<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfPurchaseOrder_Ajax.aspx.vb"
	EnableEventValidation="false" Inherits="Flypal.wfPurchaseOrder_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>Purchase Order</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<script type="text/javascript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
	<script language="javascript">
		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
		}
		function openFile() {
			str = "wfFileView.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		//Sankalp 25-08-25
		function OpenFileUploadWindow() {
			try {
				$get("AjaxLoader").style.visibility = 'visible';
				$("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx");
				return false;
			} catch (e) {
				alert(e);
			}
		}
	</script>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	<style type="text/css">
		.style1 {
			height: 21px;
		}
	</style>
	<link href="AutoComplete\jquery.autocomplete.css" type="text/css" rel="stylesheet" />
	<script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
</head>
<body>
	<form id="form1" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
			EnablePageMethods="true">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<div>
			<table class="clstablelistout" id="tblMain" style="width: 100%;">
				<tr>
					<td>
						<asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
							<table id="tblInner" class="clstablelistin">
								<tr>
									<td colspan="2" class="clsFormHeader1Newstyle">
										<asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Purchase Order [New]</asp:Label>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td colspan="2">
										<asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
													HeaderText="Fill Up The Following Fields" ValidationGroup="a"></asp:ValidationSummary>
												<asp:CustomValidator ID="cvAmend" runat="server" OnServerValidate="CustomValidate"
													ControlToValidate="cmbVendorList" Display="None" ValidationGroup="a"></asp:CustomValidator>
												<asp:CustomValidator ID="cvOrderDate" runat="server" Display="None" ControlToValidate="calOrderDate"
													ErrorMessage="Select Order Date" OnServerValidate="CustomValidate" ValidationGroup="a"></asp:CustomValidator>
												<asp:CustomValidator ID="cvCurrency" runat="server" Display="None" ControlToValidate="cmbCurrencyList"
													ErrorMessage="Select Currency from the list." OnServerValidate="CustomValidate"
													ValidationGroup="a"></asp:CustomValidator>
												<asp:CustomValidator ID="cvFactor" runat="server" Display="None" ControlToValidate="txtConversionFactor"
													ErrorMessage="Currency factor must be greater than zero." OnServerValidate="CustomValidate"
													ValidationGroup="a"></asp:CustomValidator>
												<asp:RequiredFieldValidator ID="rfvFactor" runat="server" Display="None" ControlToValidate="txtConversionFactor"
													ErrorMessage="Currency factor must be greater than zero." ValidationGroup="a"></asp:RequiredFieldValidator>
												<asp:CustomValidator ID="CustValidator" runat="server" OnServerValidate="CustomValidate"
													ErrorMessage="Select Order Date" ControlToValidate="calOrderDate" Display="None"
													ValidationGroup="a"></asp:CustomValidator>
												<asp:CustomValidator ID="cvRemark" runat="server" OnServerValidate="CustomValidate"
													ErrorMessage="Remark is too long" ControlToValidate="txtOrderRemark" Display="None"
													ValidationGroup="a"></asp:CustomValidator>
												<asp:RequiredFieldValidator ID="rfvAircraft" runat="server" Display="None" CssClass="clsLabelAuto"
													ErrorMessage="Enter Aircraft" ControlToValidate="txtAircraftReg" ValidationGroup="a"></asp:RequiredFieldValidator>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td colspan="2" align="right">
										<asp:UpdatePanel runat="server" ID="upnlStatusName" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:Label ID="lblStatus" runat="server" Text="<%# mOrder.StatusName %>" CssClass="clsLabelHeader"> </asp:Label>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td valign="top">
										<asp:UpdatePanel runat="server" ID="upnlOrderDetails" UpdateMode="Conditional">
											<ContentTemplate>
												<fieldset id="fdsOrderDetails" class="clsFieldSetNewStyle" style="border-width: 1px; position: relative">
													<legend id="ledOrderDetails" class="clsLabelHeader">Order Details</legend>
													<table>
														<tr>
															<td>
																<span id="lblDateStar" class="clsLabelStar">*</span>
															</td>
															<td>
																<span id="lblDate" class="clsLabel">Date</span>
															</td>
															<td colspan="3">
																<asp:TextBox ID="calOrderDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearch"
																	AutoPostBack="true" onchange="ValidateDateText(this,'Date_watermarkextender','true');"
																	Text="" Width="100px"></asp:TextBox>
																<cc2:CalendarExtender ID="calOrderDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																	Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="calOrderDate"></cc2:CalendarExtender>
																<cc2:TextBoxWatermarkExtender ID="calOrderDateWatermarkExtender" runat="server" TargetControlID="calOrderDate"
																	WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
															</td>
														</tr>
														<tr>
															<td>
																<span id="lblNoStar" class="clsLabelStar">*</span>
															</td>
															<td>
																<span id="lblNo" class="clsLabel">No.</span>
															</td>
															<td>
																<asp:TextBox ID="txtText" runat="server" Text="<%# mOrder.Text %>" CssClass="clsTextBoxTagSearch"
																	onfocus="SetContextKey()" ToolTip="Enter No." MaxLength="25" Width="208px"> </asp:TextBox>
																<cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtText_Autocomplete" runat="server"
																	DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
																	CompletionInterval="1" ServicePath="wfPurchaseOrder_Ajax.aspx" ServiceMethod="GetDistinctTextListAutoComplete"
																	TargetControlID="txtText" UseContextKey="False">
																</cc2:AutoCompleteExtender>
																<script type="text/jscript">
																	function SetContextKey() {
																		var autoComplete = $find('txtText_Autocomplete');
																		var TransTypeID = 'TransTypeID=<%=mOrder.TransTypeID%>¿OrderDate=<%=mOrder.OrderDate%>';
																		autoComplete.set_contextKey(TransTypeID);
																	}
																</script>
															</td>
															<td colspan="2">
																<asp:TextBox ID="txtNo" runat="server" Text="<%# mOrder.No %>" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																	MaxLength="8"> </asp:TextBox>
																<asp:TextBox ID="txtAmend" runat="server" Text="<%# mOrder.Amend %>" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																	MaxLength="2"> </asp:TextBox>
															</td>
														</tr>
														<tr>
															<td></td>
															<td>
																<span id="lblIntOrderNo" class="clsLabel">Int. Order No.</span>
															</td>
															<td>
																<asp:TextBox ID="txtIntOrderNo" runat="server" Text="<%# mOrder.IntOrderNo %>" CssClass="clsTextBoxTagSearch"
																	ToolTip="Enter Internal Order No." MaxLength="50" Enabled="<%# mOrder.StatusID = 1 %>"
																	Width="208px"> </asp:TextBox>
															</td>
															<td colspan="2">
																<asp:Button ID="btnShipBill" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH1"
																	ClientIDMode="Static" Text="Shipping / Billing Details" ToolTip="Click to see Details" />
															</td>
														</tr>
														<tr>
															<td>
																<asp:Label ID="lblForAircraftStar" runat="server" CssClass="clsLabelStar" Visible='<%#IIf(AppSettings("ClientCode") = "7AR", True, False) %>'>*</asp:Label>
															</td>
															<td>
																<asp:Label ID="lblForAircraft" runat="server" CssClass="clsLabel" Text='<%#IIf(AppSettings("ClientCode") = "CE", "AC Tail", "For Aircraft") %>'> </asp:Label>
															</td>
															<td>
																<asp:TextBox ID="txtAircraftReg" runat="server" CssClass="clsTextBoxTagSearch" Enabled="<%# mOrder.StatusID = 1 %>"
																	Width="208px" MaxLength="20" Text="<%# mOrder.AircraftReg %>" ToolTip="Enter Aircraft Reg. No."> </asp:TextBox>
															</td>
															<td>&nbsp;
															</td>
															<td>&nbsp;
															</td>
														</tr>
														<tr>
															<td class="style1">&nbsp;
															</td>
															<td class="style1">
																<span id="lblDeliveryIn" class="clsLabel">Delivery In</span>
															</td>
															<td class="style1">
																<asp:TextBox ID="txtDeliveryWithinDays" runat="server" Text="<%# mOrder.DeliveryWithinDays %>"
																	CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" ToolTip="Delivery Within" MaxLength="8"
																	Enabled="<%# mOrder.StatusID = 1 %>"> </asp:TextBox>
																<span id="lblDays" class="clsLabel">Days</span>
															</td>
															<td colspan="2">
																<asp:CheckBox ID="chkIsCalibrationOrder" runat="server" CssClass="clsLabelAuto" Text="Calibration Order"
																	Checked="<%# mOrder.IsCalibrationOrder %>" TextAlign="Right" Visible='<%# iif(mOrder.TransTypeID = 38 And mOrder.IsOverhaul = False, True, False) %>'></asp:CheckBox>
															</td>
														</tr>

														<tr>
															<td>&nbsp;
															</td>
															<td>
																<span id="lblRemark" class="clsLabel">Remark </span>
															</td>
															<td colspan="3">
																<asp:TextBox ID="txtOrderRemark" runat="server" CssClass="clsTextBoxMultiLineLong_Ajax"
																	TextMode="MultiLine" Enabled="<%# mOrder.StatusID = 1 %>" MaxLength="500" Text="<%# mOrder.Remark %>"
																	ToolTip="Enter Remark" Style="width: 402px; height: 35px;"> </asp:TextBox>
															</td>
														</tr>
													</table>
												</fieldset>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
									<td valign="top">
										<asp:UpdatePanel runat="server" ID="upnlSupplierDetails" UpdateMode="Conditional">
											<ContentTemplate>
												<fieldset id="fdsSupplierDetails" class="clsFieldSetNewStyle" style="border-width: 1px; position: relative">
													<legend id="ledSupplierDetails" class="clsLabelHeader">Supplier Details</legend>
													<table>

														<tr>
															<td>
																<span id="lblNameStar" class="clsLabelStar">*</span>
															</td>
															<td>
																<span id="lblName" class="clsLabel">Name</span>
															</td>
															<td>
																<asp:DropDownList ID="cmbVendorList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
																	Width="200px" Enabled="<%# mOrder.IsNew %>" DataTextField="Name" DataValueField="ID"
																	SelectedValue="<%# mOrder.VendorID %>" AutoPostBack="True">
																</asp:DropDownList>
															</td>
															<td colspan="2">
																<asp:Button ID="btnAddress" runat="server" Text="Details" CssClass="clsbtnH clsinfoH1"
																	ToolTip="Click to see Details" CausesValidation="False"></asp:Button>
															</td>
														</tr>
														<tr>
															<td></td>
															<td>
																<span id="lblQuotationNo" class="clsLabel">Quotation No./Date</span>
															</td>
															<td>
																<asp:TextBox ID="txtQuotationNo" runat="server" Text="<%# mOrder.QuotationNo %>"
																	CssClass="clsTextBoxTagSearch" ToolTip="Enter Quotation No." MaxLength="50" Enabled="<%# mOrder.StatusID = 1 %>"> </asp:TextBox>
															</td>
															<td colspan="2">
																<asp:TextBox ID="txtQuotationDate" runat="server" CssClass="clsTextBoxTagSearch"
																	Width="100px" onchange="ValidateDateText(this,'Date_watermarkextender','false');"
																	ClientIDMode="Static" Text="<%# mOrder.QuotationDateFormatted %>"></asp:TextBox>
																<cc2:CalendarExtender ID="txtQuotationDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
																	Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtQuotationDate"></cc2:CalendarExtender>
																<cc2:TextBoxWatermarkExtender TargetControlID="txtQuotationDate" ID="txtQuotationDateWatermarkExtender"
																	runat="server" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
															</td>
														</tr>
														<tr>
															<td></td>
															<td>
																<span id="lblOrderConfNo" class="clsLabel">Order Conf. No.</span>
															</td>
															<td colspan="3">
																<asp:TextBox ID="txtOrderConfirmationNo" runat="server" Text="<%# mOrder.OrderConfirmationNo %>"
																	CssClass="clsTextBoxTagSearch" ToolTip="Enter Order Confirmation No." MaxLength="50"
																	Enabled="<%# mOrder.StatusID = 1 %>"> </asp:TextBox>
															</td>
														</tr>
														<tr>
															<td>
																<span id="lblCurrencyStar" class="clsLabelStar">*</span>
															</td>
															<td>
																<span id="lblCurrency" class="clsLabel">Currency / Factor</span>
															</td>
															<td>
																<asp:DropDownList ID="cmbCurrencyList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
																	Enabled="<%# mOrder.IsNew %>" DataTextField="Name" DataValueField="ID" SelectedValue="<%# mOrder.CurrencyID %>"
																	AutoPostBack="True" Width="200px">
																</asp:DropDownList>
															</td>
															<td colspan="2">
																<asp:TextBox ID="txtConversionFactor" runat="server" Text="<%# mOrder.ConversionFactor %>"
																	CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" ToolTip="Enter Conversion Factor"
																	MaxLength="9"> </asp:TextBox>
															</td>
														</tr>
														<tr>
															<td></td>
															<td>
																<asp:Label ID="lblShipInVia" runat="server" CssClass="clsLabel" Text="Ship In Via"
																	Visible='<%#IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PTW", True, False) %>'> </asp:Label>
															</td>
															<td>
																<asp:TextBox ID="txtShipInVia" runat="server" Text="<%# mOrder.ShipInVia %>" CssClass="clsTextBoxTagSearch"
																	Width="208px" MaxLength="50" Enabled="<%# mOrder.StatusID = 1 %>" Visible='<%# iif(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PTW", True, False) %>'> </asp:TextBox>
															</td>
															<td>
																<asp:Label ID="lblShipOutVia" runat="server" CssClass="clsLabel" Text="Ship Out Via"
																	Visible='<%#IIf(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PTW", True, False) %>'> </asp:Label>
															</td>
															<td>
																<asp:TextBox ID="txtShipOutVia" runat="server" Text="<%# mOrder.ShipOutVia %>" CssClass="clsTextBoxTagSearch"
																	Width="208px" MaxLength="50" Enabled="<%# mOrder.StatusID = 1 %>" Visible='<%# iif(AppSettings("ClientCode") = "BA" Or AppSettings("ClientCode") = "PTW", True, False) %>'> </asp:TextBox>
															</td>
														</tr>
														<tr>
															<td></td>
															<td>
																<asp:Label ID="lblPOTowards" runat="server" CssClass="clsLabel" Text="PO. Towards"
																	Visible='<%#IIf(mOrder.TransTypeID = 5 And AppSettings("ClientCode") = "CE", True, False) %>'> </asp:Label>
															</td>
															<td colspan='3"'>
																<asp:DropDownList ID="cmbPOTowards" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" Visible='<%#IIf(mOrder.TransTypeID = 5 And AppSettings("ClientCode") = "CE", True, False) %>'
																	SelectedValue="<%# mOrder.POTowardsID %>" Enabled="<%# mOrder.StatusID = 1 %>"
																	DataTextField="Name" DataValueField="ID">
																</asp:DropDownList>
															</td>
														</tr>
														<tr>
															<td>&nbsp;
															</td>
															<td>
																<span id="lblRoundOffRequire" class="clsLabel">Round Off Required</span>
															</td>
															<td colspan="3">
																<table>
																	<tr>
																		<td>
																			<asp:CheckBox ID="chkIsRoundOff" runat="server" CssClass="clsLabelAuto" AutoPostBack="True"
																				Checked="<%# mOrder.IsRoundOff %>" TextAlign="Right"></asp:CheckBox>
																		</td>
																		<td>
																			<span id="lblIsPBHPurchase" class="clsLabel">Maintenance Support Plan / PBH Purchase</span>
																		</td>
																		<td>
																			<asp:CheckBox ID="chkIsPBHPurchase" runat="server" CssClass="clsLabelAuto" Checked="<%# mOrder.IsPBHPurchase %>"
																				Enabled="<%# mOrder.StatusID = 1 %>" TextAlign="Right" AutoPostBack="true"></asp:CheckBox>

																		</td>

																		<td>
																			<asp:Label ID="lblContractNo" runat="server" Text="<%# mOrder.ContractNo  %>" CssClass="clsLabelHeader"> </asp:Label>
																		</td>
																	</tr>
																</table>
															</td>
														</tr>
													</table>
												</fieldset>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<%-- Sankalp 25-08-25 --%>
								<tr align="right">
									<td colspan="1" valign="top">
										<fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
											<legend class="clsFieldSet1"><b>File Attachments</b></legend>
											<asp:UpdatePanel ID="upnlItemAttachment" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
													<table>
														<tr>
															<td style="height: 15px">
																<asp:UpdatePanel ID="upnldgItemAttachment" runat="server" UpdateMode="Conditional">
																	<ContentTemplate>
																		<asp:GridView ID="dgItemAttachment" ToolTip="List of File Attachment(s)" runat="server"
																			CssClass="clsGridNewStyle" GridLines="Horizontal" CellPadding="5" DataKeyNames="ID" ShowHeaderWhenEmpty="true" AllowSorting="True"
																			AllowPaging="False" AutoGenerateColumns="false">
																			<AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left"></AlternatingRowStyle>
																			<RowStyle CssClass="clsdgItem" HorizontalAlign="Left"></RowStyle>
																			<HeaderStyle CssClass="clsdgHeader" BackColor="White" ForeColor="Black" Font-Bold="True" HorizontalAlign="Left"></HeaderStyle>
																			<Columns>
																				<%-- 0 --%>
																				<asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
																				<%-- 1 --%>
																				<asp:BoundField Visible="False" DataField="WOID" HeaderText="WOID"></asp:BoundField>
																				<%-- 2 --%>
																				<asp:BoundField DataField="SrNo" HeaderText="Sr. No.">
																					<HeaderStyle Width="10px"></HeaderStyle>
																				</asp:BoundField>
																				<%-- 3 --%>
																				<asp:BoundField Visible="False" DataField="FileName" SortExpression="FileName" HeaderText="File Name">
																					<HeaderStyle Wrap="False" HorizontalAlign="Left"></HeaderStyle>
																				</asp:BoundField>
																				<%-- 4 --%>
																				<asp:TemplateField HeaderText="File Name">
																					<HeaderStyle Width="200px" HorizontalAlign="Left"></HeaderStyle>
																					<ItemTemplate>
																						<asp:TextBox ID="txtFileName" runat="server" 
																							CssClass="clsTextBoxTagSearch" 
																							MaxLength="100"
																							ClientIDMode="Static"
																							ToolTip="Enter File Name To Be Attached" 
																							Text='<%# DataBinder.Eval(Container.DataItem,"FileName") %>'
																							Width="350px" />
																					</ItemTemplate>
																				</asp:TemplateField>
																				<%-- 5 --%>
																				<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
																					<ItemTemplate>
																						<%-- <span id="button">Login</span>--%>
																						<div class="dropdown">
																							<div id="divd" class="dropdownbtn-content" runat="server">
																								<table id="T1" class="clsGridNew_Ajax">
																									<tr>

																										<td>
																											<asp:ImageButton ID="View" runat="server"
																												CommandArgument='<%# Eval("SrNo") %>'
																												CommandName="View"
																												CssClass="FileAttachmentICN"
																												ImageUrl="icons/CLIP01.ICO" />
																										</td>

																										<td>
																											<asp:ImageButton ID="Remove" runat="server"
																												CommandArgument='<%# Eval("SrNo") %>'
																												CausesValidation="false"
																												CommandName="Remove"
																												CssClass="largerActionICNS"
																												ImageUrl="~/images/delete.png"
																												Visible='<%# IIf(mOrder.StatusID = 2 Or mOrder.StatusID = 4 Or mOrder.ReceiptCount > 0 Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange", False, True) %>' />
																										</td>
																									</tr>
																								</table>
																							</div>
																							<asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
																								Style="cursor: pointer" />
																						</div>
																					</ItemTemplate>
																					<HeaderStyle HorizontalAlign="Center" />
																					<ItemStyle HorizontalAlign="Center" />
																				</asp:TemplateField>
																			</Columns>
																		</asp:GridView>
																	</ContentTemplate>
																</asp:UpdatePanel>
															</td>
															<td valign="top">
																<asp:ImageButton ID="btnSelectFiles" runat="server" ImageUrl="~/images/plus1.png"
																	Height="22px" Width="24px" ToolTip="Click to Add New Attachment" CausesValidation="false"></asp:ImageButton>
															</td>
														</tr>
													</table>
												</ContentTemplate>
											</asp:UpdatePanel>
										</fieldset>
									</td>

									<td colspan="1" valign="top">
										<fieldset id="fdsOtherDetails" class="clsFieldSetNewStyle" style="border-width: 1px; position: relative">
											<legend id="ledEmpOtherDetails" class="clsLabelHeader">Other Details</legend>
											<asp:UpdatePanel runat="server" ID="upnlOpeningLine" UpdateMode="Conditional">
												<ContentTemplate>
													<table>
														<tr>
															<td></td>

															<%-- Comment by Sankalp 25-08-25 --%>
															<%--<td class="clsInnerTable">
                                                                <span id="lblAttachFile" class="clsLabel">Attach</span>
                                                            </td>
                                                            <td>
                                                                <table id="Table12" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:UpdatePanel ID="upnlFileupload" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <input type="button" id="btnSelectFile" value="Select File" style="width: 120px;"
                                                                                                    runat="server" class="clsbtnH clsinfoH1" />
                                                                                            </td>
                                                                                            <td style="padding-left: 3px;">
                                                                                                <asp:Button ID="btnDelAttach" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Remove Attachment"
                                                                                                    Text="Remove Attachment" Enabled="False" Width="140px"></asp:Button>
                                                                                            </td>
                                                                                            <td style="padding-left: 2px;">
                                                                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
                                                                                                    Height="20px" Width="20px"></asp:ImageButton>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>--%>

															<td width="80px">
																<span id="lblOpeningLine" class="clsLabel">Opening Line</span>
															</td>
															<td colspan="2">
																<asp:TextBox ID="txtOpeningLine" runat="server" CssClass="clsTextBoxMultilineOpening_Ajax" Width="500px"
																	Height="27px" MaxLength="1000" Rows="20" Text="<%# mOrder.OpeningLine %>" TextMode="MultiLine"
																	ToolTip="Enter Opening Line for an order" Enabled="<%# mOrder.StatusID = 1 %>"> </asp:TextBox>
															</td>
														</tr>
													</table>
												</ContentTemplate>
											</asp:UpdatePanel>
										</fieldset>
									</td>

								</tr>
								<%-- End --%>
								<tr>
									<td colspan="2">
										<asp:UpdatePanel runat="server" ID="upnlOrderItems" UpdateMode="Conditional">
											<ContentTemplate>
												<fieldset id="fdsOrderItemDetails" class="clsFieldSetNewStyle" runat="server" style="border-width: 1px; position: relative">
													<legend id="ledOrderItemDetails">
														<table>
															<tr>
																<td>
																	<span id="lblOrderItems" class="clsLabelHeader">Order Item(s):</span>
																</td>
																<td>
																	<asp:Button ID="btnAdd" TabIndex="0" runat="server" Text="Add" CssClass="clsbtnH clsinfoH1"
																		ValidationGroup="a" ToolTip="Click to add Order Items"></asp:Button>
																</td>
															</tr>
														</table>
													</legend>
													<table width="100%">
														<tr>
															<td>
																<asp:GridView ID="dgOrderItems" runat="server" CssClass="clsGridNewStyle" ShowHeaderWhenEmpty="True"
																	AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="3">
																	<AlternatingRowStyle CssClass="clsdgAltItem" />
																	<RowStyle CssClass="clsdgItem" />
																	<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
																	<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
																	<PagerSettings FirstPageText="First" LastPageText="Last" />
																	<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
																	<Columns>
																		<%--0--%>
																		<asp:BoundField DataField="SrNo" HeaderText="Sr.No." />
																		<%--1--%>
																		<%-- <asp:BoundField DataField="ItemName" HeaderText="Part No.">
                                                                    <HeaderStyle Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                    <FooterStyle Wrap="False" />
                                                                </asp:BoundField>--%>
																		<asp:ButtonField DataTextField="ItemName" HeaderText="Part #" CommandName="PartStatus">
																			<HeaderStyle HorizontalAlign="Left" Wrap="false" />
																			<ItemStyle HorizontalAlign="Left" Wrap="false" />
																		</asp:ButtonField>
																		<%--2--%>
																		<asp:BoundField DataField="ItemDescription" HeaderText="Description" />
																		<%--3--%>
																		<asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
																			<HeaderStyle HorizontalAlign="left" Wrap="false" />
																			<ItemStyle HorizontalAlign="left" />
																		</asp:BoundField>
																		<%--4--%>
																		<asp:BoundField DataField="FromNo" HeaderText="S.O./Req. No " Visible="False" />
																		<%--5--%>
																		<asp:BoundField DataField="FromDate" HeaderText="S.O./Req. Date" Visible="False" />
																		<%--6--%>
																		<asp:TemplateField HeaderText="Qty.">
																			<ItemTemplate>
																				<asp:TextBox ID="txtQty" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																					Enabled="<%# mOrder.AgainstTypeID <> 3 %>" OnTextChanged="AddAttributesForGridControls"
																					MaxLength="8" Text='<%# DataBinder.Eval(Container.DataItem, "Qty") %>'></asp:TextBox>
																				<asp:CustomValidator ID="cvBrokenRules" runat="server" ControlToValidate="txtQty"
																					Display="None"></asp:CustomValidator>
																			</ItemTemplate>
																			<HeaderStyle HorizontalAlign="Right" />
																			<ItemStyle HorizontalAlign="Right" />
																		</asp:TemplateField>
																		<%--7--%>
																		<asp:BoundField DataField="UnitName" HeaderText="Unit" />
																		<%--8--%>
																		<asp:TemplateField HeaderText="Rate">
																			<ItemTemplate>
																				<asp:TextBox ID="txtRate" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																					OnTextChanged="AddAttributesForGridControls" MaxLength="12" Text='<%# DataBinder.Eval(Container.DataItem, "CRate") %>'> </asp:TextBox>
																			</ItemTemplate>
																			<HeaderStyle HorizontalAlign="Right" />
																			<ItemStyle HorizontalAlign="Right" />
																		</asp:TemplateField>
																		<%--9--%>
																		<asp:TemplateField HeaderText="Discount(%)">
																			<ItemTemplate>
																				<asp:TextBox ID="txtDiscount" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																					OnTextChanged="AddAttributesForGridControls" MaxLength="5" Text='<%# DataBinder.Eval(Container.DataItem, "PerDiscount") %>'> </asp:TextBox>
																				<asp:CustomValidator ID="cvBrokenRules1" runat="server" ControlToValidate="txtDiscount"
																					Display="None" OnServerValidate="CustomValidate1"></asp:CustomValidator>
																			</ItemTemplate>
																			<HeaderStyle HorizontalAlign="Right" />
																			<ItemStyle HorizontalAlign="Right" />
																		</asp:TemplateField>
																		<%--10--%>
																		<asp:BoundField DataField="NetRate" HeaderText="Net Rate">
																			<HeaderStyle HorizontalAlign="Right" Wrap="false" />
																			<ItemStyle HorizontalAlign="Right" />
																		</asp:BoundField>
																		<%--11--%>
																		<asp:BoundField DataField="CAmount" HeaderText="Amount">
																			<HeaderStyle HorizontalAlign="Right" />
																			<ItemStyle HorizontalAlign="Right" />
																		</asp:BoundField>
																		<%--12--%>
																		<asp:TemplateField HeaderText="Bill Back Rate">
																			<ItemTemplate>
																				<asp:TextBox ID="txtBillBackRate" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																					OnTextChanged="AddAttributesForGridControls" MaxLength="12" Text='<%# DataBinder.Eval(Container.DataItem, "CBillBackRate") %>'> </asp:TextBox>
																			</ItemTemplate>
																			<HeaderStyle HorizontalAlign="Right" />
																			<ItemStyle HorizontalAlign="Right" />
																		</asp:TemplateField>
																		<%--13--%>
																		<asp:TemplateField HeaderText="Delivery Days">
																			<ItemTemplate>
																				<asp:TextBox ID="txtDelInDays" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																					OnTextChanged="AddAttributesForGridControls" MaxLength="8" Text='<%# DataBinder.Eval(Container.DataItem, "DeliveryInDays") %>'> </asp:TextBox>
																			</ItemTemplate>
																			<HeaderStyle HorizontalAlign="Right" Wrap="False" />
																			<ItemStyle HorizontalAlign="Right" />
																		</asp:TemplateField>
																		<%--14--%>
																		<asp:TemplateField HeaderText="Priority">
																			<ItemTemplate>
																				<asp:DropDownList ID="cmbPriority" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
																					Width="70px" DataSource="<%# mPriorityList %>" DataTextField="Name" DataValueField="ID"
																					SelectedValue='<%# DataBinder.Eval(Container.DataItem, "PriorityID") %>'>
																				</asp:DropDownList>
																			</ItemTemplate>
																			<HeaderStyle HorizontalAlign="Left" />
																			<ItemStyle HorizontalAlign="Left" />
																		</asp:TemplateField>
																		<%--15--%>
																		<asp:BoundField DataField="ModelName" HeaderText="Applicable To" Visible="False" />
																		<%--16--%>
																		<asp:TemplateField HeaderText="Remark">
																			<ItemTemplate>
																				<asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxMultiLineLong_Ajax"
																					TextMode="MultiLine" Text='<%# DataBinder.Eval(Container.DataItem, "Remark") %>'> </asp:TextBox>
																			</ItemTemplate>
																		</asp:TemplateField>
																		<%--17--%>
																		<asp:TemplateField HeaderText="Note" Visible="False">
																			<ItemTemplate>
																				<asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxDate_Ajax" Text='<%# DataBinder.Eval(Container.DataItem, "Note") %>'> </asp:TextBox>
																			</ItemTemplate>
																		</asp:TemplateField>
																		<%--18--%>
																		<asp:BoundField DataField="HSNACSCode" HeaderText="HSN/SAC Code">
																			<HeaderStyle HorizontalAlign="left"></HeaderStyle>
																			<ItemStyle HorizontalAlign="left"></ItemStyle>
																		</asp:BoundField>
																		<%--19--%>
																		<asp:TemplateField HeaderText="CGST Per.">
																			<ItemTemplate>
																				<asp:TextBox ID="txtCGSTPer" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																					Enabled='<%#IIf(mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange" Or AppSettings("ChangeGSTPercentage") = "False", False, True) %>'
																					OnTextChanged="AddAttributesForGridControls" MaxLength="8" Text='<%# DataBinder.Eval(Container.DataItem, "CGSTPercentage") %>'></asp:TextBox>
																				<asp:CustomValidator ID="cvCGSTPer" runat="server" ControlToValidate="txtCGSTPer"
																					Display="None"></asp:CustomValidator>
																			</ItemTemplate>
																			<HeaderStyle HorizontalAlign="Right" />
																			<ItemStyle HorizontalAlign="Right" />
																		</asp:TemplateField>
																		<%--20--%>
																		<asp:BoundField DataField="CGSTCAmount" HeaderText="CGST Amount">
																			<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																			<ItemStyle HorizontalAlign="Right"></ItemStyle>
																		</asp:BoundField>
																		<%--21--%>
																		<asp:TemplateField HeaderText="SGST Per.">
																			<ItemTemplate>
																				<asp:TextBox ID="txtSGSTPer" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																					OnTextChanged="AddAttributesForGridControls" MaxLength="8" Text='<%# DataBinder.Eval(Container.DataItem, "SGSTPercentage") %>'
																					Enabled="false"></asp:TextBox>
																			</ItemTemplate>
																			<HeaderStyle HorizontalAlign="Right" />
																			<ItemStyle HorizontalAlign="Right" />
																		</asp:TemplateField>
																		<%--22--%>
																		<asp:BoundField DataField="SGSTCAmount" HeaderText="SGST Amount">
																			<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																			<ItemStyle HorizontalAlign="Right"></ItemStyle>
																		</asp:BoundField>
																		<%--23--%>
																		<asp:TemplateField HeaderText="IGST Per.">
																			<ItemTemplate>
																				<asp:TextBox ID="txtIGSTPer" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																					Enabled='<%#IIf(mOrder.StatusID >= 2 Or mOrder.ReceiptCount > 0 Or Session("ToOpenOrderForRateChange") = "ToOpenOrderForRateChange" Or AppSettings("ChangeGSTPercentage") = "False", False, True) %>'
																					OnTextChanged="AddAttributesForGridControls" MaxLength="8" Text='<%# DataBinder.Eval(Container.DataItem, "IGSTPercentage") %>'></asp:TextBox>
																				<asp:CustomValidator ID="cvIGSTPer" runat="server" ControlToValidate="txtIGSTPer"
																					Display="None"></asp:CustomValidator>
																			</ItemTemplate>
																			<HeaderStyle HorizontalAlign="Right" />
																			<ItemStyle HorizontalAlign="Right" />
																		</asp:TemplateField>
																		<%--24--%>
																		<asp:BoundField DataField="IGSTCAmount" HeaderText="IGST Amount">
																			<HeaderStyle HorizontalAlign="Right"></HeaderStyle>
																			<ItemStyle HorizontalAlign="Right"></ItemStyle>
																		</asp:BoundField>
																		<%--25--%>
																		<asp:BoundField DataField="RequisitionTextNo" HeaderText="Requisition No.">
																			<HeaderStyle HorizontalAlign="left"></HeaderStyle>
																			<ItemStyle HorizontalAlign="left"></ItemStyle>
																		</asp:BoundField>
																		<%--26 here this ReceiptBalanceQty column is two times--%>
																		<asp:BoundField DataField="ReceiptBalanceQtyToShowOnGrid" HeaderText="Pending Qty."
																			Visible="false">
																			<HeaderStyle HorizontalAlign="right" />
																			<ItemStyle HorizontalAlign="right" />
																		</asp:BoundField>
																		<%--27--%>
																		<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
																			<ItemTemplate>
																				<div class="dropdown">
																					<div class="dropdownbtn-content">
																						<table id="T1" class="clsGridNew_Ajax">
																							<tr>
																								<td>
																									<asp:ImageButton ID="EditView" runat="server"
																										CommandArgument='<%# Container.DataItemIndex %>'
																										class="actionICNS" ToolTip="Click to Edit record."
																										CommandName="EditView" ImageUrl="~/images/edit.png" />
																								</td>
																								<td>
																									<asp:ImageButton ID="DeleteRecord" runat="server"
																										CommandArgument='<%# Container.DataItemIndex %>'
																										class="actionICNS  largerActionICNS"
																										ToolTip="Click to Delete record."
																										CommandName="DeleteRecord" ImageUrl="~/images/delete.png" />
																								</td>
																							</tr>
																						</table>
																					</div>
																					<asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
																						Style="cursor: pointer" />
																				</div>
																			</ItemTemplate>
																			<HeaderStyle HorizontalAlign="Center" />
																			<ItemStyle HorizontalAlign="Center" />
																		</asp:TemplateField>
																		<%--28--%>
																		<asp:BoundField DataField="ReceiptBalanceQty" HeaderStyle-CssClass="hideGridColumn"
																			HeaderText="ReceiptBalanceQty" ItemStyle-CssClass="hideGridColumn">
																			<HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
																			<ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
																		</asp:BoundField>
																		<%--29--%>
																		<asp:TemplateField HeaderText="View TD" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
																			<ItemTemplate>
																				<asp:ImageButton ID="ViewTechDirectionID" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
																					CommandName="ViewTechDirection" Style="height: 15px; width: 15px" ImageUrl="~/icons/iconfinder_-_Eye-Show-View-Watch-See_3844411.ico"
																					Visible='<%#  DataBinder.Eval(Container.DataItem, "TechDirectionCount")  %>' />
																			</ItemTemplate>
																			<HeaderStyle HorizontalAlign="Center" Wrap="false" />
																			<ItemStyle HorizontalAlign="Center" />
																		</asp:TemplateField>
																		<%--30--%>
																		<asp:BoundField DataField="TechDirectionCount" HeaderText="TechDirectionCount">
																			<HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
																			<ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
																		</asp:BoundField>
																		<%--31--%>
																		<asp:ButtonField Text="Part Status" HeaderText="Part Status" CommandName="ShowPartStatus">
																			<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																			<ItemStyle HorizontalAlign="Left" Wrap="False" />
																		</asp:ButtonField>
																	</Columns>
																	<SelectedRowStyle BackColor="ControlDark" />
																</asp:GridView>
															</td>
														</tr>
													</table>
												</fieldset>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td colspan="2" align="right">
										<asp:UpdatePanel runat="server" ID="upnlTotalAmount" UpdateMode="Conditional">
											<ContentTemplate>
												<span id="lblTotalAmount" class="clsLabel">Total Amount</span>
												<asp:TextBox ID="txtTotalAmt" runat="server" Text="<%# mOrder.CTotalAmount %>" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
													ReadOnly="True" BackColor="#E0E0E0" Width="150px"> </asp:TextBox>
												</td>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td valign="top">
										<asp:UpdatePanel runat="server" ID="upnlOrderTerms" UpdateMode="Conditional">
											<ContentTemplate>
												<fieldset id="fdsOrderTerms" class="clsFieldSetNewStyle" runat="server" style="border-width: 1px; position: relative">
													<legend id="ledOrderTerms">
														<table>
															<tr>
																<td>
																	<span id="lblOrderTerms" class="clsLabelHeader">Order Term(s)</span>
																</td>
																<td>
																	<asp:Button ID="btnAddTerm" runat="server" CssClass="clsbtnH clsinfoH1" Text="Add"
																		ToolTip="Click To Add Term"></asp:Button>
																</td>
																<td>
																	<asp:Button ID="btnAddSupplierSpecificTerms" runat="server" CssClass="clsbtnH clsinfoH1"
																		Width="200px" Text="Add Supplier Specific Terms" ToolTip="Click To Add Supplier Specific Terms"></asp:Button>
																</td>
															</tr>
														</table>
													</legend>
													<table width="100%">
														<tr>
															<td>
																<asp:GridView ID="dgOrderTerms" runat="server" AutoGenerateColumns="False" Width="100%"
																	CssClass="clsGridNewStyle" ShowHeaderWhenEmpty="True" GridLines="Horizontal"
																	CellPadding="3">
																	<AlternatingRowStyle CssClass="clsdgAltItem" />
																	<RowStyle CssClass="clsdgItem" />
																	<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
																	<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
																	<PagerSettings FirstPageText="First" LastPageText="Last" />
																	<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
																	<Columns>
																		<asp:BoundField DataField="SrNo" HeaderText="Sr.No." />
																		<%--0--%>
																		<asp:BoundField DataField="Terms" HeaderText="Terms and Conditions">
																			<ItemStyle CssClass="TextBreak" Width="500px" />
																		</asp:BoundField>
																		<%--1--%>
																		<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
																			<ItemTemplate>
																				<div class="dropdown">
																					<div class="dropdownbtn-content">
																						<table id="T1" class="clsGridNew_Ajax">
																							<tr>
																								<td>
																									<asp:ImageButton ID="DeleteTerm" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
																										CommandName="DeleteTerm" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
																								</td>
																							</tr>
																						</table>
																					</div>
																					<asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
																						Style="cursor: pointer" />
																				</div>
																			</ItemTemplate>
																			<HeaderStyle HorizontalAlign="Center" />
																			<ItemStyle HorizontalAlign="Center" />
																		</asp:TemplateField>
																		<%--2--%>
																	</Columns>
																	<SelectedRowStyle BackColor="ControlDark" />
																</asp:GridView>
															</td>
														</tr>
													</table>
												</fieldset>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
									<td valign="top">
										<asp:UpdatePanel runat="server" ID="upnlOrderCharges" UpdateMode="Conditional">
											<ContentTemplate>
												<fieldset id="fdsOrderCharges" class="clsFieldSetNewStyle" runat="server" style="border-width: 1px; position: relative">
													<legend id="ledOrderCharges">
														<table>
															<tr>
																<td>
																	<span id="lblOrderCharges" class="clsLabelHeader">Order Charge(s)</span>
																</td>
																<td>
																	<asp:Button ID="btnAddCharges" runat="server" CssClass="clsbtnH clsinfoH1" Text="Add"
																		ToolTip="Click To Add Charge"></asp:Button>
																</td>
															</tr>
														</table>
													</legend>
													<table width="100%">

														<tr>
															<td>
																<asp:GridView ID="dgOrderCharges" runat="server" AutoGenerateColumns="False" Width="100%"
																	CssClass="clsGridNewStyle" ShowHeaderWhenEmpty="True" GridLines="Horizontal"
																	CellPadding="3">
																	<AlternatingRowStyle CssClass="clsdgAltItem" />
																	<RowStyle CssClass="clsdgItem" />
																	<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
																	<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
																	<PagerSettings FirstPageText="First" LastPageText="Last" />
																	<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
																	<Columns>
																		<asp:BoundField DataField="SrNo" HeaderText="Sr.No." />
																		<%--0--%>
																		<asp:BoundField DataField="ChargeName" HeaderText="Charge Name" />
																		<%--1--%>
																		<asp:BoundField DataField="Percentage" HeaderText="Percentage">
																			<HeaderStyle HorizontalAlign="Right" />
																			<ItemStyle HorizontalAlign="Right" />
																			<FooterStyle HorizontalAlign="Right" />
																		</asp:BoundField>
																		<%--2--%>
																		<asp:BoundField DataField="CChargeAmount" HeaderText="Charge Amount">
																			<HeaderStyle HorizontalAlign="Right" />
																			<ItemStyle HorizontalAlign="Right" />
																			<FooterStyle HorizontalAlign="Right" />
																		</asp:BoundField>
																		<%--3--%>
																		<asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
																			<ItemTemplate>
																				<div class="dropdown">
																					<div class="dropdownbtn-content">
																						<table id="T1" class="clsGridNew_Ajax">
																							<tr>
																								<td>
																									<asp:ImageButton ID="EditCharge" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
																										CommandName="EditCharge" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png" />
																								</td>
																							</tr>
																							<tr>
																								<td>
																									<asp:ImageButton ID="DeleteCharge" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
																										CommandName="DeleteCharge" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
																								</td>
																							</tr>
																						</table>
																					</div>
																					<asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
																						Style="cursor: pointer" />
																				</div>
																			</ItemTemplate>
																			<HeaderStyle HorizontalAlign="Center" />
																			<ItemStyle HorizontalAlign="Center" />
																		</asp:TemplateField>
																		<%--4--%>
																	</Columns>
																	<SelectedRowStyle BackColor="ControlDark" />
																</asp:GridView>
															</td>
														</tr>
													</table>
												</fieldset>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td colspan="2" align="right">
										<asp:UpdatePanel runat="server" ID="upnlGrandTotal" UpdateMode="Conditional">
											<ContentTemplate>
												<table>
													<tr>
														<td>
															<asp:Label ID="lblTotalCGST" runat="server" CssClass="clsLabel">Total CGST</asp:Label>
														</td>
														<td>
															<asp:TextBox ID="txtTotalCGST" runat="server"
																CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Text="<%# mOrder.CTotalCGSTAmount %>"
																ReadOnly="True" BackColor="#E0E0E0" Width="150px">
															</asp:TextBox>
														</td>
													</tr>
													<tr>
														<td>
															<asp:Label ID="lblTotalSGST" runat="server" CssClass="clsLabel">Total SGST</asp:Label>
														</td>
														<td>
															<asp:TextBox ID="txtTotalSGST" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																Text="<%# mOrder.CTotalSGSTAmount %>" ReadOnly="True" BackColor="#E0E0E0" Width="150px">
															</asp:TextBox>
														</td>
													</tr>
													<tr>
														<td>
															<asp:Label ID="lblTotalIGST" runat="server" CssClass="clsLabel">Total IGST</asp:Label>
														</td>
														<td>
															<asp:TextBox ID="txtTotalIGST" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																Text="<%# mOrder.CTotalIGSTAmount %>" ReadOnly="True" BackColor="#E0E0E0" Width="150px">
															</asp:TextBox>
														</td>
													</tr>
													<tr>
														<td>
															<span id="lblGrandTotal" class="clsLabelAuto">Grand Total</span>
														</td>
														<td>
															<asp:TextBox ID="txtGrandTotal" runat="server" Text="<%# mOrder.CGrandTotal %>"
																CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																ReadOnly="True" BackColor="#E0E0E0" Width="150px"></asp:TextBox>
														</td>
													</tr>
													<tr>
														<td>
															<asp:Label ID="lblAdvancePayment" runat="server" CssClass="clsLabelAuto"
																Visible="<%$ AppSettings:AdvancePayment %>">Advance Payment</asp:Label>
														</td>
														<td>
															<asp:TextBox ID="txtAdvancePayment" runat="server" Text="<%# mOrder.CAdvancePayment %>"
																CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Visible="<%$ AppSettings:AdvancePayment %>"
																MaxLength="12" Width="150px"></asp:TextBox>
														</td>
													</tr>
													<tr>
														<td>
															<asp:Label ID="lblRemaining" runat="server" CssClass="clsLabelAuto"
																Visible="<%$ AppSettings:AdvancePayment %>">Balance Paymnet</asp:Label>
														</td>
														<td>
															<asp:TextBox ID="txtRemaining" runat="server" Text="<%# mOrder.Remaining %>"
																CssClass="clsTextBoxTagSearchRightAlignQty_Ajax"
																Visible="<%$ AppSettings:AdvancePayment %>" ReadOnly="True" BackColor="#E0E0E0"
																Width="150px"></asp:TextBox>
														</td>
													</tr>
												</table>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr>
									<td colspan="2" align="right">
										<asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
											<ContentTemplate>
												<table>
													<tr>
														<td>
															<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="clsbtnH clsinfoH1"
																ClientIDMode="Static" ToolTip="Click to Cancel the Purchase Order"></asp:Button>
															<asp:Button ID="btnChangeRate" runat="server" Text="Change Info" CssClass="clsbtnH clsinfoH1"
																ClientIDMode="Static" ToolTip="Click to Change Rate of the Purchase Order"></asp:Button>
															<asp:Button ID="btnAmend" runat="server" Text="Amend" CssClass="clsbtnH clsinfoH1"
																ToolTip="Click to Amend the Purchase Order" ClientIDMode="Static"></asp:Button>
															<asp:Button ID="btnAuthorized" runat="server" Text="Authorize" CssClass="clsbtnH clsinfoH1"
																ToolTip="Click to authorize Purchase Order"></asp:Button>
															<asp:Button ID="btnSendMail" runat="server" CssClass="clsbtnH clsinfoH1" Text="Send Mail"
																ClientIDMode="Static" ToolTip="Click to Send PO Print by Mail"></asp:Button>
															<asp:Button ID="btnShopWorkOrder" runat="server" Text="Shop WorkOrder" CssClass="clsbtnH clsinfoH1"
																Visible='<%#IIf(((mOrder.TransTypeID = 31 Or mOrder.TransTypeID = 38) And AppSettings("ClientCode") = "BA"), True, False) %>'
																Enabled="<%# Not mOrder.IsNew %>" ToolTip="Click to Print Shop WorkOrder" ClientIDMode="Static" />
															<asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="clsbtnH clsinfoH1"
																ToolTip="Click to Print Purchase Order" ClientIDMode="Static" Enabled="<%# Not mOrder.IsNew %>"></asp:Button>
															<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="clsbtnH clsinfoH1"
																ToolTip="Click to Save Purchase Order" ValidationGroup="a"></asp:Button>
															<asp:Button ID="btnPrintPROCUREMENTANDPAYMENTFORM" runat="server" Text="PROCUREMENT AND PAYMENT FORM" CssClass="clsbtnH clsinfoH1"
																ToolTip="Click to Print Purchase Order" ClientIDMode="Static" Enabled="<%# Not mOrder.IsNew %>" Visible='<%#IIf((AppSettings("ClientCode") = "KAS"), True, False) %>'></asp:Button>
															<asp:Button ID="btnBack" runat="server" Text="Close" CssClass="clsbtnH clsinfoH1"
																ToolTip="Click to go back to the previous page" CausesValidation="False"></asp:Button>
															<asp:Button ID="btnlRequestForDigitalSignature" runat="server" Text="Request for Digital Signature" CssClass="clsbtnH clsinfoH1"
																ClientIDMode="Static" Visible="false" />
															<asp:Button ID="btnViewDSFile" runat="server" Text="View DS File" CssClass="clsbtnH clsinfoH1"
																ClientIDMode="Static" Visible="false" />
														</td>
													</tr>
												</table>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
								<tr style="height: 0px;">
									<td colspan="2" style="height: 0px;">
										<asp:UpdatePanel runat="server" ID="upnlBtnFileUpload" UpdateMode="Conditional">
											<ContentTemplate>
												<asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
													CausesValidation="False" Style="display: none;"></asp:Button>
												<asp:Button ID="hdnimgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
													CausesValidation="False" Style="display: none;"></asp:Button>
												<asp:Button ID="hdnBtnDigitalSignatureRequest" ClientIDMode="Static" runat="server" Text="----"
													CausesValidation="False" Style="display: none;"></asp:Button>
											</ContentTemplate>
										</asp:UpdatePanel>
									</td>
								</tr>
							</table>
						</asp:Panel>
					</td>
				</tr>
			</table>

			<%--Vendor Details--%>
			<asp:Panel runat="server" ID="pnlVendorDetails" CssClass="clsPanel1">
				<div style="display: none">
					<asp:Button runat="server" ID="btnDummyVendorDetails" Text="Vendor Details" />
				</div>
				<div style="width: 100%">
					<asp:UpdatePanel runat="server" ID="upnlVendorDetails" UpdateMode="Conditional">
						<ContentTemplate>
							<table class="clstablelistout" id="Table1">
								<tr>
									<td>
										<table id="Table2" class="clstablelistin">
											<tr>
												<td>
													<table>
														<tr>
															<td class="clsFormHeader1Newstyle">
																<span id="lblAddressDetails" class="clsFormHeader">Supplier Details</span>
															</td>
														</tr>
														<tr>
															<td>
																<fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
																	<legend><b>Details</b></legend>
																	<table>
																		<tr>
																			<td>
																				<span id="lblAddress" class="clsLabelAuto">Address</span>
																			</td>
																			<td>
																				<asp:TextBox ID="txtAddress" runat="server" CssClass="clsTextBoxTagSearch"
																					Width="300" Height="46px" Text="<%# mOrder.Address %>" MaxLength="500" BackColor="#E0E0E0"
																					ReadOnly="True" TextMode="MultiLine"></asp:TextBox>
																			</td>
																		</tr>
																		<tr>
																			<td>
																				<span id="lblKindAttention" class="clsLabelAuto">Kind Attention</span>
																			</td>
																			<td>
																				<asp:TextBox ID="txtAttention" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mOrder.Attention %>"
																					Width="300" MaxLength="50" Enabled="<%# mOrder.StatusID = 1 %>" ToolTip="Enter Kind Attention">
																				</asp:TextBox>
																			</td>
																		</tr>
																	</table>
																</fieldset>
															</td>
														</tr>
														<tr>
															<td>
																<fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
																	<legend><b>Supplier Approval List.</b></legend>
																	<table width="100%">
																		<tr>
																			<td>
																				<asp:GridView ID="dgApprovalList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
																					CssClass="clsGridNewStyle" ShowHeaderWhenEmpty="true" ToolTip="Vendor Approval List."
																					GridLines="Horizontal" CellPadding="3">
																					<AlternatingRowStyle CssClass="clsdgAltItem" />
																					<RowStyle CssClass="clsdgItem" />
																					<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
																					<HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
																					<PagerSettings FirstPageText="First" LastPageText="Last" />
																					<PagerStyle BackColor="White" CssClass="paging" ForeColor="Black" HorizontalAlign="Right" />
																					<Columns>
																						<asp:BoundField DataField="ID" HeaderStyle-CssClass="hideGridColumn" HeaderText="ID"
																							ItemStyle-CssClass="hideGridColumn">
																							<HeaderStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
																							<ItemStyle CssClass="hideGridColumn" HorizontalAlign="Left" />
																						</asp:BoundField>
																						<asp:BoundField DataField="ApprovalNo" HeaderText="Approval No.">
																							<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																							<ItemStyle Wrap="False" />
																						</asp:BoundField>
																						<asp:BoundField DataField="Name" HeaderText="Name">
																							<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																							<ItemStyle Wrap="False" />
																						</asp:BoundField>
																						<asp:BoundField DataField="FromDateFormatted" HeaderText="From Date">
																							<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																							<ItemStyle Wrap="False" />
																						</asp:BoundField>
																						<asp:BoundField DataField="ToDateFormatted" HeaderText="To Date">
																							<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																							<ItemStyle Wrap="False" />
																						</asp:BoundField>
																						<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" HeaderStyle-HorizontalAlign="Center">
																							<ItemTemplate>
																								<asp:ImageButton ID="View" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="ViewRec"
																									Style="height: 20px; width: 13px" ImageUrl="icons/CLIP01.ICO" Visible='<%#  Eval("IsAttachmentAdded")%>' />
																							</ItemTemplate>
																							<HeaderStyle HorizontalAlign="Center" />
																							<ItemStyle HorizontalAlign="Center" />
																						</asp:TemplateField>
																						<asp:BoundField DataField="IsAttachmentAdded" HeaderStyle-CssClass="hideGridColumn"
																							HeaderText="Size" ItemStyle-CssClass="hideGridColumn" />
																					</Columns>
																				</asp:GridView>
																			</td>
																		</tr>
																	</table>
																</fieldset>
															</td>
														</tr>
													</table>
												</td>
											</tr>
											<tr>
												<td align="right">
													<asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
														<ContentTemplate>
															<table id="Table6" cellspacing="1" cellpadding="1" border="0">
																<tr>
																	<td>
																		<asp:Button ID="btnOK" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1" Text="Apply"></asp:Button>
																	</td>
																	<td>
																		<asp:Button ID="btnVendorDetailsBack" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH1"
																			Text="Back"></asp:Button>
																	</td>
																</tr>
															</table>
														</ContentTemplate>
													</asp:UpdatePanel>
												</td>
											</tr>
											<!--Dummy panel to open modelpopup-->
											<tr style="height: 0px;">
												<td style="height: 0px;">
													<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
														<ContentTemplate>
															<asp:Button ID="hdnBtnManualPropertyValue" ClientIDMode="Static" runat="server" Text="----"
																CausesValidation="False" Style="display: none;"></asp:Button>
															<asp:Button ID="hdnBtnFileAttachmentAndOtherInfo" ClientIDMode="Static" runat="server"
																Text="Add" CausesValidation="False" Style="display: none;"></asp:Button>
															<asp:Button ID="hdnBtnMSPAssemblySelection" ClientIDMode="Static" runat="server"
																Text="Add" CausesValidation="False" Style="display: none;"></asp:Button>
														</ContentTemplate>
													</asp:UpdatePanel>
												</td>
											</tr>
											<!--End -->
										</table>
									</td>
								</tr>
							</table>
							</td> </tr> </table>
						</ContentTemplate>
					</asp:UpdatePanel>
				</div>
			</asp:Panel>
			<cc2:ModalPopupExtender runat="server" ID="mdeVendorDetails" TargetControlID="btnDummyVendorDetails"
				PopupControlID="pnlVendorDetails" BackgroundCssClass="clsModalPopupBGForSecondPage">
			</cc2:ModalPopupExtender>
			<%--End Vendor Details--%>


			<%--Ship Bill Details--%>
			<asp:Panel runat="server" ID="pnlShipBillDetails" CssClass="clsPanel1">
				<div style="display: none">
					<asp:Button runat="server" ID="btnDummyShipBillDetails" Text="Ship Bill Details" />
				</div>
				<div>
					<asp:UpdatePanel runat="server" ID="upnlShipBillDetails" UpdateMode="Conditional">
						<ContentTemplate>
							<table class="clstablelistout" id="Table3">
								<tr>
									<td>
										<asp:Panel ID="Panel1" runat="server" CssClass="clsPanel1">
											<table id="Table4">
												<tr>
													<td colspan="2" class="clsFormHeader1Newstyle">
														<span id="lblBillingShippingDetails" class="clsFormHeader">Billing/Shipping Details</span>
													</td>
												</tr>
												<tr>
													<td colspan="2">
														<asp:ValidationSummary ID="Validationsummary1" runat="server" HeaderText="Fill Up The Following Fields"
															CssClass="clsValidationSummary"></asp:ValidationSummary>
													</td>
												</tr>
												<tr>
													<td colspan="2">
														<asp:CustomValidator ID="cvBillingAddress" runat="server" ErrorMessage="Billing address length must not be  greater than 250 character. "
															Display="None" ControlToValidate="txtBillingAddress" OnServerValidate="CustomValidate"
															CssClass="clsValidationSummary"></asp:CustomValidator>
														<asp:CustomValidator ID="cvShippingAddress" runat="server" ErrorMessage="Shipping address length must not be  greater than 250 character. "
															Display="None" ControlToValidate="txtShippingAddress" OnServerValidate="CustomValidate"
															CssClass="clsValidationSummary"></asp:CustomValidator>
													</td>
												</tr>
												<tr>
													<td>
														<fieldset id="lblBillingDetails" class="clsFieldSetNewStyle" style="border-width: 1px">
															<legend><b>Billing Details</b></legend>
															<table>
																<tr>
																	<td>
																		<span id="lblBillType" class="clsLabel">Bill To</span>
																	</td>
																	<td>
																		<asp:DropDownList ID="cmbBillType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
																			SelectedValue="<%# mOrder.BillToTypeID %>" DataValueField="ID" DataTextField="Name" Width="200px">
																		</asp:DropDownList>
																	</td>
																</tr>
																<tr>
																	<td>
																		<span id="lblBillingAdd" class="clsLabel">Address</span>
																	</td>
																	<td>
																		<asp:TextBox ID="txtBillingAddress" runat="server" CssClass="clsTextBoxTagSearch"
																			Width="300" Height="46px" Text="<%# mOrder.BillingAddress %>" ToolTip="Enter Billing Address"
																			MaxLength="250" TextMode="MultiLine" Rows="5">
																		</asp:TextBox>
																	</td>
																</tr>
															</table>
														</fieldset>
													</td>
													<td>
														<fieldset id="lblShippingDetails" class="clsFieldSetNewStyle" style="border-width: 1px">
															<legend><b>Shipping Details</b></legend>
															<table>
																<tr>
																	<td>
																		<span id="lblShipType" class="clsLabel">Ship To</span>
																	</td>
																	<td>
																		<asp:DropDownList ID="cmbShipType" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
																			SelectedValue="<%# mOrder.ShipToTypeID %>" DataValueField="ID" DataTextField="Name" Width="200px">
																		</asp:DropDownList>
																	</td>
																</tr>
																<tr>
																	<td align="left">
																		<span id="lblShippingAdd" class="clsLabelAuto">Address</span>
																	</td>
																	<td>
																		<asp:TextBox ID="txtShippingAddress" runat="server" CssClass="clsTextBoxTagSearch"
																			Width="300" Height="46px" Text="<%# mOrder.ShippingAddress %>" ToolTip="Enter Shipping Address"
																			MaxLength="250" TextMode="MultiLine">
																		</asp:TextBox>
																	</td>
																</tr>
															</table>
														</fieldset>
													</td>
												</tr>
												<tr>
													<td colspan="2">
														<fieldset id="lblLocationCustomerDetails" class="clsFieldSetNewStyle" style="border-width: 1px">
															<legend>
																<b>Location / Customer Details</b>
															</legend>
															<table>
																<tr>
																	<td>
																		<span id="lblLocation" class="clsLabel">Location</span>
																	</td>
																	<td>
																		<asp:DropDownList ID="cmbLocation" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
																			SelectedValue="<%# mOrder.LocationID %>" DataValueField="ID" DataTextField="Name"
																			Enabled="False" Width="200px">
																		</asp:DropDownList>
																		<span id="Label4" class="clsLabelAuto">(Select Location for Bill To or Ship To)
																		</span>
																		<asp:CustomValidator ID="cvCustomer" runat="server" ErrorMessage="Customer Required"
																			Display="None" ControlToValidate="cmbCustomerList" OnServerValidate="CustomValidate"
																			CssClass="clsValidationSummary"></asp:CustomValidator>
																		<asp:CustomValidator ID="cvLocation" runat="server" ErrorMessage="Location Required"
																			Display="None" ControlToValidate="cmbLocation" OnServerValidate="CustomValidate"
																			CssClass="clsValidationSummary"></asp:CustomValidator>
																	</td>
																</tr>
																<tr>
																	<td>
																		<span id="lblCustomer" class="clsLabel">Customer</span>
																	</td>
																	<td>
																		<asp:DropDownList ID="cmbCustomerList" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
																			AutoPostBack="True" SelectedValue="<%# mOrder.CustomerID %>" DataValueField="ID"
																			DataTextField="Name" Enabled="False" Width="200px">
																		</asp:DropDownList>
																		<span id="Label10" class="clsLabelAuto">(Select Customer for Bill To or Ship To)
																		</span>
																	</td>
																</tr>
															</table>
														</fieldset>
													</td>
												</tr>
												<tr>
													<td align="right" colspan="2">
														<table id="Table5" cellspacing="1" cellpadding="1" border="0">
															<tr>
																<td>
																	<asp:Button ID="btnOkShipBillDetails" TabIndex="0"
																		runat="server" CssClass="clsbtnH clsinfoH1" Text="Apply"></asp:Button>
																</td>
																<td>
																	<asp:Button ID="btnBackShipBillDetails" TabIndex="0"
																		runat="server" CssClass="clsbtnH clsinfoH1" Text="Back"></asp:Button>
																</td>
															</tr>
														</table>
													</td>
												</tr>
											</table>
										</asp:Panel>
									</td>
								</tr>
							</table>
						</ContentTemplate>
					</asp:UpdatePanel>
				</div>
			</asp:Panel>

			<cc2:ModalPopupExtender runat="server" ID="mdeShipBillDetails" TargetControlID="btnDummyShipBillDetails"
				PopupControlID="pnlShipBillDetails" BackgroundCssClass="clsModalPopupBGForSecondPage">
			</cc2:ModalPopupExtender>
			<%--End Ship Bill Details--%>
		</div>

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

		<!-- File Upload Modal Dialog-->
		<div style="display: none">
			<asp:HiddenField runat="server" ID="btnDummyFileUpload" />
		</div>
		<asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="IFileUpload" allowtransparency="true" frameborder="0" height="100%" width="100%"
				src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupFileUpload" runat="server" TargetControlID="btnDummyFileUpload"
			PopupControlID="pnlFileUpload" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameFileUploadStateComplete() {
				$("#btnDummyFileUpload").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}
			$(document).ready(function () {
				$("#btnSelectFile").live("click", function () {
					try {
						$get("AjaxLoader").style.visibility = 'visible';
						$("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx");
						if (!$.browser.msie) {
							$("#btnDummyFileUpload").click();
							$get("AjaxLoader").style.visibility = 'hidden';
						}

						return false;
					} catch (e) {
						alert(e);
					}
				});
			});
		</script>
		<script type="text/javascript">
			function ParentCallBackFunctionForFileUpload(fileattached) {
				var FileUpwindow = $find("<%=mdlPopupFileUpload.ClientID %>");
				//close File Upload popup window
				FileUpwindow.hide();
				//Free resources
				$("#IFileUpload").attr("src", "JavaScript:''");
				if (fileattached) {
					//call hidden button to set file upload content to object
					$("#hdnBtnFileUpload").click();
				}
			}
		</script>
		<!-- End -->
		<!-- Popup For By Mail -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyForByMail" Text="ForByMail" ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlForByMail" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeForByMail" frameborder="0" height="100%" width="100%" src="JavaScript:''"
				scrolling="auto" allowtransparency="true"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupForByMail" runat="server" TargetControlID="btnDummyForByMail"
			PopupControlID="pnlForByMail" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function OpenByMaiWindow() {
				try {
					$("#IframeForByMail").attr("src", "wfByMail_Ajax.aspx?Type=pup");
					$("#btnDummyForByMail").click();

					return false;
				} catch (e) {
					alert(e);
				}
			}
			function ParentCallBackFunctionForSendMail() {
				var ForByMailwindow = $find("<%=mdlPopupForByMail.ClientID %>");
				//close popup window
				ForByMailwindow.hide();
				//           release resources
				$("#IframeForByMail").attr("src", "JavaScript:''");
			}
			function ParentCallBackFunctionToSendMail() {
				var ForByMailwindow = $find("<%=mdlPopupForByMail.ClientID %>");
				//close popup window
				ForByMailwindow.hide();
				//           release resources
				$("#IframeForByMail").attr("src", "JavaScript:''");
				//call image button
				$("#hdnimgBtnSendMail").click();
			}
		</script>
		<!---End-->
		<!-- File Attachment And Other Info Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyFileAttachmentAndOtherInfo" Text="VendorApprova"
				ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlFileAttachmentAndOtherInfo" ClientIDMode="Static"
			HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="IframeFileAttachmentAndOtherInfo" frameborder="0" height="100%" width="100%"
				src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupFileAttachmentAndOtherInfo" runat="server" TargetControlID="btnDummyFileAttachmentAndOtherInfo"
			PopupControlID="pnlFileAttachmentAndOtherInfo" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameFileAttachmentAndOtherInfoStateComplete() {
				$("#btnDummyFileAttachmentAndOtherInfo").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}

			function OpenFileAttachmentAndOtherInfoWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#IframeFileAttachmentAndOtherInfo").attr("src", "wfFileAttachmentAndOtherInfo_Ajax.aspx?Type=pup");
					//                if (!$.browser.msie) {
					$("#btnDummyFileAttachmentAndOtherInfo").click();
					$get("AjaxLoader").style.visibility = "hidden";
					//                }
					return false;
				} catch (e) {
					alert(e);
				}
			}
			function ParentCallBackFunctionForFileAttachmentAndOtherInfo() {
				var FileAttachmentAndOtherInfowindow = $find("<%=mdlPopupFileAttachmentAndOtherInfo.ClientID %>");
				//close Vendor Approval popup window
				FileAttachmentAndOtherInfowindow.hide();
				//release resources
				$("#IframeFileAttachmentAndOtherInfo").attr("src", "JavaScript:''");
				//call image button
				$("#hdnBtnFileAttachmentAndOtherInfo").click();
			}
		</script>
		<!-- End-->
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
		<!-- Popup For PartStatus -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyPartStatus" Text="PartStatus" ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlPartStatus" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframePartStatus" frameborder="0" height="100%" width="100%" src="JavaScript:''"
				scrolling="auto" allowtransparency="true"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupPartStatus" runat="server" TargetControlID="btnDummyPartStatus"
			PopupControlID="pnlPartStatus" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFramePartStatusStateComplete() {

				if (Page_IsValid) {
					$("#btnDummyPartStatus").click();
					$get("AjaxLoader").style.visibility = "hidden";
				}
				else {

					$get("AjaxLoader").style.visibility = "hidden";
				}
			}

			function OpenPartStatusWindow() {
				try {
					$("#IframePartStatus").attr("src", "wfPartStatus.aspx?Type=FromPurchaseOrder");
					$("#btnDummyPartStatus").click();

					return false;
				} catch (e) {
					alert(e);
				}

			}
			function ParentCallBackFunctionForPartStatus() {
				var PartStatuswindow = $find("<%=mdlPopupPartStatus.ClientID %>");
				//close popup window
				PartStatuswindow.hide();
				//           release resources
				$("#IframePartStatus").attr("src", "JavaScript:''");
				//call image button
				$("#hdnBtnPartStatus").click();
			}
		</script>
		<!---End-->
		<!-- Popup For MSPAssemblySelection -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyMSPAssemblySelection" Text="MSPAssemblySelection"
				ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlMSPAssemblySelection" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeMSPAssemblySelection" frameborder="0" height="100%" width="100%" src="JavaScript:''"
				scrolling="auto" allowtransparency="true"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupMSPAssemblySelection" runat="server" TargetControlID="btnDummyMSPAssemblySelection"
			PopupControlID="pnlMSPAssemblySelection" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function OpenMSPAssemblySelectionWindow() {
				try {
					$("#IframeMSPAssemblySelection").attr("src", "wfMSPAssemblySelection_Ajax.aspx?Type=FromPurchaseOrder");
					$("#btnDummyMSPAssemblySelection").click();

					return false;
				} catch (e) {
					alert(e);
				}

			}
			function ParentCallBackFunctionForMSPAssemblySelection() {
				var MSPAssemblySelectionwindow = $find("<%=mdlPopupMSPAssemblySelection.ClientID %>");
				//close popup window
				MSPAssemblySelectionwindow.hide();
				//           release resources
				$("#IframeMSPAssemblySelection").attr("src", "JavaScript:''");
				//call image button
				$("#hdnBtnMSPAssemblySelection").click();
			}
		</script>
		<!---End-->

		<!--DigitalSignatureRequest Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyDigitalSignatureRequest" Text="DigitalSignatureRequest" CausesValidation="false"
				ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlDigitalSignatureRequest" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeDigitalSignatureRequest" frameborder="0" height="100%" allowtransparency="true"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupDigitalSignatureRequest" runat="server" TargetControlID="btnDummyDigitalSignatureRequest"
			PopupControlID="pnlDigitalSignatureRequest" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameDigitalSignatureRequestStateComplete() {
				$("#btnDummyDigitalSignatureRequest").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}

			function OpenDigitalSignatureRequestWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#IframeDigitalSignatureRequest").attr("src", "wfDigitalSignatureRequest.aspx?Type=pup");

					$("#btnDummyDigitalSignatureRequest").click();
					$get("AjaxLoader").style.visibility = 'hidden';

					return false;
				} catch (e) {
					alert(e);
				}
			}
			function ParentCallBackFunctionForDigitalSignatureRequest() {
				var DigitalSignatureRequestwindow = $find("<%=mdlPopupDigitalSignatureRequest.ClientID %>");
				//close popup window
				DigitalSignatureRequestwindow.hide();
				//release resources
				$("#IframeDigitalSignatureRequest").attr("src", "JavaScript:''");
				//call button click
				$("#hdnBtnDigitalSignatureRequest").click();
			}
		</script>
		<!-- End-->
	</form>

	<script type="text/javascript">

		//Date validations
		function ValidateDateText(elem, extenderid, TobeReset) {

			var datevalue = $(elem).val();
			var resetTodaysDate = TobeReset;
			var params = { 'Date': datevalue, 'SetDefault': resetTodaysDate };
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

		Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            <% Dim mOpenFrom As String = Request.QueryString("Type") %>
			<% If Not mOpenFrom Is Nothing AndAlso mOpenFrom = "FromReqItemStatusReport" Then %>  

			$('#btnCancel').attr('disabled', 'disabled');
			$('#btnChangeRate').attr('disabled', 'disabled');
			$('#btnAmend').attr('disabled', 'disabled');
			$('#btnPrint').attr('disabled', 'disabled');
			$('#btnShipBill').attr('disabled', 'disabled');
			$('#btnShopWorkOrder').attr('disabled', 'disabled');
			$('#btnSendMail').attr('disabled', 'disabled');

              <% End if %>  
		});

	</script>

	<script type="text/javascript">

		Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

			$("#<%=txtAircraftReg.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=OrderAircraftReg', {
				width: 208,
				autoFill: false,
				matchContains: true,
				mustMatch: false,
				delay: 0
			});

		});

	</script>

	<!-- Highlight DropDownList Item Color-->
	<script type="text/javascript">

		Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

			var ddSupplier = document.getElementById("cmbVendorList");
			if (ddSupplier != null) {
				var i = 0;
				if (ddSupplier.disabled == false) {

					<% For Each item1 In mVendorList%>

						<% If item1.NotInUse = "True" Then%>
					ddSupplier[i].style.cssText = "font-weight: bold;background-color: #FF0000;color: #FFFFFF;"

						<% End If%>

					i = i + 1;

					<% Next%>
				}
			}

			var ddCustomer = document.getElementById("cmbCustomerList");

			if (ddCustomer != null) {

				if (ddCustomer.disabled == false) {

					var j = 0;
					<% For Each item2 In mCustomerList%>

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
</body>
</html>
