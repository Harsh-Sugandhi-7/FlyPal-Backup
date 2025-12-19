<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfModelCreation_Ajax.aspx.vb"
	Inherits="Flypal.wfModelCreation_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<title></title>
	<link id="MainStyle" type="text/css" rel="stylesheet" />
	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>
	<script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
	<script language="javascript" id="clientEventHandlersJS">
		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
		}
		function openTranDetail() {
			str = "wfReports.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openTranDetail1() {
			str = "webform1.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openFilel() {
			str = "wfFileView.aspx";
			window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
		}
		function openDetail() {
			str = "wfDetail.aspx";
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
		<table class="clstablelistout">
			<tr>
				<td>
					<asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
						<table id="tblLedgerList" class="clstablelistin">
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:Label ID="lblTitle" CssClass="clstitle1" runat="server">Model Information [New]</asp:Label>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="up" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<cc2:TabContainer ID="TabContainer1" runat="server" class="clstablelistin" AutoPostBack="true"
												ClientIDMode="Static">
												<cc2:TabPanel ID="TabPanel1" runat="server" CssClass="clsPanel1">
													<HeaderTemplate>
														Model Details
													</HeaderTemplate>
													<ContentTemplate>
														<table id="tblmain" class="clstablelistin">
															<tr>
																<td>
																	<asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
																		<ContentTemplate>
																			<asp:ValidationSummary ID="Validationsummary1" ValidationGroup="1" runat="server"
																				HeaderText="Fill Up The Following Fields" CssClass="clsValidationSummary"></asp:ValidationSummary>
																			<asp:CustomValidator ValidationGroup="1" ID="cvManufac" runat="server" ErrorMessage="Select Manufacturer from the list."
																				ControlToValidate="cmbManufacturerList" Display="None" ClientValidationFunction="ValidateManufacturerList"
																				CssClass="clsLabelAuto"></asp:CustomValidator>
																			<asp:RequiredFieldValidator ID="rfvName" runat="server" ValidationGroup="1" CssClass="clsLabelAuto"
																				ErrorMessage="Name Required" ControlToValidate="txtName" Display="None"></asp:RequiredFieldValidator>
																			<script type="text/javascript">
																				function ValidateManufacturerList(source, args) {
																					args.IsValid = false;
																					var dd = $get("cmbManufacturerList");
																					if (dd.selectedIndex != 0) {
																						args.IsValid = true;
																						return;
																					}
																				}
																			</script>
																		</ContentTemplate>
																	</asp:UpdatePanel>
																</td>
															</tr>
															<tr>
																<td>
																	<table id="Table7" class="clsTable1" border="0" cellspacing="1" cellpadding="1">
																		<tr>
																			<td valign="top">
																				<asp:UpdatePanel ID="upnlModelInformation" runat="server" UpdateMode="Conditional">
																					<ContentTemplate>
																						<table id="Table10" border="0" cellspacing="1" cellpadding="1">
																							<tr>
																								<td>
																									<span id="lblStarManufacturer" class="clsLabelStar">*</span>
																								</td>
																								<td>
																									<span id="lblManufacturer" class="clsLabelAuto">Manufacturer</span>
																								</td>
																								<td>
																									<table cellspacing="0" cellpadding="0">
																										<tr>
																											<td>
																												<asp:DropDownList ID="cmbManufacturerList" runat="server" CssClass="clsComboBox_Ajax"
																													ClientIDMode="Static" DataValueField="ID" DataTextField="Name" SelectedValue="<%# mModel.ManufacturerID %>">
																												</asp:DropDownList>
																											</td>
																											<td>
																												<asp:ImageButton ID="imgbtnManufacturer" runat="server" ImageUrl="~/images/plus1.png"
																													Height="22px" Width="24px" ToolTip="Click to Add New Model" CausesValidation="False"></asp:ImageButton>
																											</td>
																										</tr>
																									</table>
																								</td>
																							</tr>
																							<tr>
																								<td>
																									<span id="lblModelNameStar1" class="clsLabelStar">*</span>
																								</td>
																								<td>
																									<span id="lblName" class="clsLabelAuto">Name</span>
																								</td>
																								<td>
																									<table cellspacing="0" cellpadding="0">
																										<tr>
																											<td>
																												<asp:TextBox ID="txtName" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mModel.Name %>"
																													ToolTip="Enter Name" MaxLength="25" Width="179px"></asp:TextBox>
																											</td>
																										</tr>
																									</table>
																								</td>
																							</tr>
																							<tr>
																								<td></td>
																								<td>
																									<span id="lblForAssembly" class="clsLabelAuto">For Assembly</span>
																								</td>
																								<td>
																									<table cellspacing="0" cellpadding="0">
																										<tr>
																											<td>
																												<asp:DropDownList ID="cmbForAssemblyList" AutoPostBack="true"
																													runat="server" CssClass="clsComboBox_Ajax"
																													SelectedValue="<%# mModel.AssemblyTypeID %>" 
																													DataValueField="ID" DataTextField="Name">
																												</asp:DropDownList>
																											</td>
																										</tr>
																									</table>
																								</td>
																							</tr>
																							<asp:PlaceHolder runat="server" ID="PrimaryModelPlaceHolder">
																								<tr>	
																									<td></td>
																									<td>
																										<asp:Label ID="lblPrimaryModel" runat="server" CssClass="clsLabel">Primary Model</asp:Label>
																									</td>
																									<td colspan="2">
																										<table cellspacing="0" cellpadding="0">
																											<tr>
																												<td>
																													<asp:DropDownList ID="cmbPrimaryModelList" runat="server"
																														CssClass="clsComboBox_Ajax"
																														DataValueField="ID" DataTextField="Name" 
																														SelectedValue="<%# mModel.PrimaryModelID %>">
																													</asp:DropDownList>
																												</td>
																												<td>
																													<asp:ImageButton ID="imgbtnPrimaryModel" runat="server" 
																														ImageUrl="~/images/plus1.png"
																														Height="22px" Width="24px" ToolTip="Click to Add New record"
																														CausesValidation="False"></asp:ImageButton>
																												</td>
																											</tr>
																										</table>
																									</td>
																								</tr>
																							</asp:PlaceHolder>
																						</table>
																					</ContentTemplate>
																				</asp:UpdatePanel>
																			</td>
																		</tr>
																	</table>
																</td>
															</tr>
															<tr>
																<td align="right">
																	<asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
																		<ContentTemplate>
																			<table cellspacing="0">
																				<tr>
																					<td>
																						<asp:Button ID="btnSave" CssClass="clsButton_Ajax" ValidationGroup="1" runat="server"
																							Text="Save" ToolTip="Click to save the Model Information"></asp:Button>
																					</td>
																					<td>
																						<asp:Button ID="btnClose" ValidationGroup="1" TabIndex="0" runat="server" CssClass="clsButton_Ajax"
																							Text="Close" ToolTip="Click to close Model Information screen" CausesValidation="False"></asp:Button>
																					</td>
																				</tr>
																			</table>
																		</ContentTemplate>
																	</asp:UpdatePanel>
																</td>
															</tr>
														</table>
													</ContentTemplate>
												</cc2:TabPanel>
												<cc2:TabPanel ID="tabModelServiceList" runat="server" Visible="<%# Not mModel.IsNew %>">
													<HeaderTemplate>
														Service Capability
													</HeaderTemplate>
													<ContentTemplate>
														<asp:UpdatePanel ID="upnlModelServiceList" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
															</ContentTemplate>
														</asp:UpdatePanel>
													</ContentTemplate>
												</cc2:TabPanel>
												<cc2:TabPanel ID="tabInspectionList" runat="server" ClientIDMode="Static" Visible="<%# Not mModel.IsNew %>">
													<HeaderTemplate>
														Inspection Capability
													</HeaderTemplate>
													<ContentTemplate>
														<asp:UpdatePanel ID="upnlModelInspectionList" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
															</ContentTemplate>
														</asp:UpdatePanel>
													</ContentTemplate>
												</cc2:TabPanel>
												<cc2:TabPanel ID="tabDirectiveList" runat="server" ClientIDMode="Static" Visible="<%# Not mModel.IsNew %>">
													<HeaderTemplate>
														Directive Capability
													</HeaderTemplate>
													<ContentTemplate>
														<asp:UpdatePanel ID="upnlModelModList" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
															</ContentTemplate>
														</asp:UpdatePanel>
													</ContentTemplate>
												</cc2:TabPanel>
											</cc2:TabContainer>
										</ContentTemplate>
										<Triggers>
											<asp:AsyncPostBackTrigger ControlID="TabContainer1" />
										</Triggers>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr style="height: 0px;">
								<td style="height: 0px;">
									<asp:UpdatePanel runat="server" ID="upnlBtnFileUpload" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:Button ID="htnBtnManufacturer" ValidationGroup="1" ClientIDMode="Static" runat="server"
												Text="..." CausesValidation="False" Style="display: none;"></asp:Button>
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

		<!-- Select Manufacturer popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyManufacturer" Text="TaskCard Tool" ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlManufacturer" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeManufacturer" frameborder="0" height="100%" width="100%" src="JavaScript:''"
				allowtransparency="true" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupManufacturer" runat="server" TargetControlID="btnDummyManufacturer"
			PopupControlID="pnlManufacturer" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameManufacturerStateComplete() {
				$("#btnDummyManufacturer").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}

			function OpenManufacturerWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#IframeManufacturer").attr("src", "wfManufacturer_Ajax.aspx?Type=pup");

					if (!$.browser.msie) {
						$("#btnDummyManufacturer").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}

					return false;
				} catch (e) {
					alert(e);
				}

			}
			function ParentCallBackFunctionForManufacturer() {
				var Manufacturerwindow = $find("<%=mdlPopupManufacturer.ClientID %>");
				//close Task Card Tool popup window
				Manufacturerwindow.hide();
				//           release resources
				$("#IframeManufacturer").attr("src", "JavaScript:''");
				//call image button
				$("#htnBtnManufacturer").click();
			}
		</script>
		<!-- End-->
		<!--Model Monitor Service List Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyModelMonitorServiceList" Text="Model Service Master"
				ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlModelMonitorServiceList" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeModelMonitorServiceList" frameborder="0" height="100%" allowtransparency="true"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupModelMonitorServiceList" runat="server" TargetControlID="btnDummyModelMonitorServiceList"
			PopupControlID="pnlModelMonitorServiceList" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameModelMonitorServiceListStateComplete() {
				$("#btnDummyModelMonitorServiceList").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}

			function OpenModelMonitorServiceListWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#IframeModelMonitorServiceList").attr("src", "wfModelMonitorServiceList_Ajax.aspx?Type=pup");

					if (!$.browser.msie) {
						$("#btnDummyModelMonitorServiceList").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}
					//});
					return false;
				} catch (e) {
					alert(e);
				}
			}
			function ParentCallBackFunctionForModelMonitorServiceList() {
				var ModelMonitorServiceListwindow = $find("<%=mdlPopupModelMonitorServiceList.ClientID %>");
			//close Model Service Master popup window
			ModelMonitorServiceListwindow.hide();
			//           release resources
			$("#IframeModelMonitorServiceList").attr("src", "JavaScript:''");
			var tabContainer = $get('<%=TabContainer1.ClientID%>');
				tabContainer.control.set_activeTabIndex(0);

				//            alert(tc.ActiveTabIndex);
				//call Model Service Master image button
				//            $("#hdnBtnModelMonitorServiceList").click();
			}
		</script>
		<!-- Model Monitor Service List Popup Window End -->
		<!--Model Monitor Insp List Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyModelMonitorInspList" Text="Model Service Master"
				ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlModelMonitorInspList" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeModelMonitorInspList" frameborder="0" height="100%" allowtransparency="true"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupModelMonitorInspList" runat="server" TargetControlID="btnDummyModelMonitorInspList"
			PopupControlID="pnlModelMonitorInspList" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameModelMonitorInspListStateComplete() {
				$("#btnDummyModelMonitorInspList").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}

			function OpenModelMonitorInspListWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#IframeModelMonitorInspList").attr("src", "wfModelMonitorInspList_Ajax.aspx?Type=pup");

					if (!$.browser.msie) {
						$("#btnDummyModelMonitorInspList").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}
					//});
					return false;
				} catch (e) {
					alert(e);
				}
			}
			function ParentCallBackFunctionForModelMonitorInspList() {
				var ModelMonitorInspListwindow = $find("<%=mdlPopupModelMonitorInspList.ClientID %>");
			//close Model Service Master popup window
			ModelMonitorInspListwindow.hide();
			//           release resources
			$("#IframeModelMonitorInspList").attr("src", "JavaScript:''");
			var tabContainer = $get('<%=TabContainer1.ClientID%>');
				tabContainer.control.set_activeTabIndex(0);
				//call Model Service Master image button
				//            $("#hdnBtnModelMonitorInspList").click();
			}
		</script>
		<!-- Model Monitor Insp List Popup Window End -->
		<!--Model Monitor Mod List Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyModelMonitorModList" Text="Model Service Master"
				ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlModelMonitorModList" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframeModelMonitorModList" frameborder="0" height="100%" allowtransparency="true"
				width="100%" src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupModelMonitorModList" runat="server" TargetControlID="btnDummyModelMonitorModList"
			PopupControlID="pnlModelMonitorModList" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameModelMonitorModListStateComplete() {
				$("#btnDummyModelMonitorModList").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}

			function OpenModelMonitorModListWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#IframeModelMonitorModList").attr("src", "wfModelMonitorModList_Ajax.aspx?Type=pup");

					if (!$.browser.msie) {
						$("#btnDummyModelMonitorModList").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}
					//});
					return false;
				} catch (e) {
					alert(e);
				}
			}
			function ParentCallBackFunctionForModelMonitorModList() {
				var ModelMonitorModListwindow = $find("<%=mdlPopupModelMonitorModList.ClientID %>");
			//close Model Service Master popup window
			ModelMonitorModListwindow.hide();
			//           release resources
			$("#IframeModelMonitorModList").attr("src", "JavaScript:''");
			//call Model Service Master image button
			var tabContainer = $get('<%=TabContainer1.ClientID%>');
				tabContainer.control.set_activeTabIndex(0);
			}

			function CallZerothActiveTabIndex() {
				var tabContainer = $get('<%=TabContainer1.ClientID%>');
				tabContainer.control.set_activeTabIndex(0);
			}
		</script>
		<!-- Model Monitor Mod List Popup Window End -->

		<%--Added by Harsh on 23rd July 2024--%>
		<!-- Select PrimaryModel popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyPrimaryModel" Text="TaskCard Tool" ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlPrimaryModel" ClientIDMode="Static" HorizontalAlign="Center"
			Style="height: 100%; width: 100%;">
			<iframe id="IframePrimaryModel" frameborder="0" height="100%" width="100%" src="JavaScript:''"
				allowtransparency="true" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupPrimaryModel" runat="server" TargetControlID="btnDummyPrimaryModel"
			PopupControlID="pnlPrimaryModel" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFramePrimaryModelStateComplete() {
				$("#btnDummyPrimaryModel").click();
				$get("AjaxLoader").style.visibility = 'hidden';
			}

			function OpenPrimaryModelWindow() {
				try {

					$get("AjaxLoader").style.visibility = 'visible';
					$("#IframePrimaryModel").attr("src", "wfPrimaryModel_Ajax.aspx?Type=pup");

					if (!$.browser.msie) {
						$("#btnDummyPrimaryModel").click();
						$get("AjaxLoader").style.visibility = 'hidden';
					}

					return false;
				} catch (e) {
					alert(e);
				}

			}
			function ParentCallBackFunctionForPrimaryModel() {
				var PrimaryModelwindow = $find("<%=mdlPopupPrimaryModel.ClientID %>");
				//close Task Card Tool popup window
				PrimaryModelwindow.hide();
				//           release resources
				$("#IframePrimaryModel").attr("src", "JavaScript:''");
				//call image button
				$("#htnBtnPrimaryModel").click();
			}
		</script>
		<!-- End-->
	</form>
</body>
</html>
