<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDentAndRepair_Ajax.aspx.vb"
	Inherits="Flypal.DentAndRepairDetailPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<title>Dent & Repair Chart</title>
	<meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
	<link id="MainStyle" type="text/css" rel="stylesheet" />

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

	<script type="text/javascript" type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>

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
		<table class="clstablelistout Table-MaxWidth" id="tblMain">
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
														<asp:Label ID="lblTitle" runat="server"
															CssClass="clsFormHeader" />
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
											<td align="right">
												<asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
													<ContentTemplate>
														<table>
															<tr>
																<td>
																	<asp:Button ID="btnAdd" TabIndex="0" 
																		runat="server"
																		Text="Add" CssClass="clsbtnH clsinfoH" 
																		ToolTip="Click to add Items" />
																	<asp:Button ID="btnAuthorized" 
																		runat="server" Text="Authorize"
																		CssClass="clsbtnH clsinfoH"
																		ToolTip="Click to authorize" />
																	<asp:Button ID="btnPrint" runat="server"
																		Text="Print"
																		CssClass="clsbtnH clsinfoH"
																		ToolTip="Click to Print"
																		ClientIDMode="Static"
																		Enabled="<%# Not mDentBuckle.IsNew %>" />
																	<asp:Button ID="btnSave" 
																		runat="server" Text="Save"
																		CssClass="clsbtnH clsinfoH"
																		ToolTip="Click to Save" />
																	<asp:Button ID="btnBack" runat="server" 
																		Text="Close"
																		CssClass="clsbtnH clsinfoH" 
																		ToolTip="Click to go back to the previous page"
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
									<asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:ValidationSummary ID="Validationsummary2" 
												runat="server" CssClass="clsValidationSummary"
												HeaderText="Fill Up The Following Fields" />
											<asp:CustomValidator ID="cvATA" runat="server" 
												ControlToValidate="cmbMachineList"
												ValidateEmptyText="true" 
												ClientValidationFunction="ValidateMachine" 
												Display="None" ErrorMessage="Select Aircraft from the list" />
											<asp:CustomValidator ID="CustomValidator4" runat="server"
												CssClass="clsLabelAuto" Display="None"
												ErrorMessage="Revision No. should not be greater than 50 characters"
												ControlToValidate="txtRevNo" ClientValidationFunction="ValidateName" />
											<asp:RequiredFieldValidator ID="RequiredFieldValidator1"
												runat="server" CssClass="clsLabelAuto"
												ErrorMessage="Enter No." ControlToValidate="txtText"
												Display="None" />

											<script type="text/javascript">

												function ValidateMachine(source, args) {

													args.IsValid = false;
													var dd = $get("cmbMachineList");
													if (dd.selectedIndex != 0) {
														args.IsValid = true;
														return;
													}

												}

												function ValidateName(source, args) {

													var ControlName = source.controltovalidate;
													switch (ControlName) {

														case 'txtRevNo':
															var Value = $get(ControlName).value.length;
															if (Value > 50) {
																args.IsValid = false;
																return;
															}
															break;
													}
												}

											</script>

										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td align="right">
									<asp:UpdatePanel runat="server" ID="upnlStatusName" UpdateMode="Conditional">
										<ContentTemplate>
											<table style="width: 100%;">
												<tr>
													<td align="left" style="vertical-align: top;">
														<div id="CreatedByUpdatedByDetails" runat="server">
															<table id="tblCreatedByUpdatedByDetails">
																<tr>
																	<td>
																		<asp:Label ID="lblCreatedBy"
																			CssClass="clsLabelAuto" runat="server"
																			Font-Size="Small" ForeColor="Navy"
																			Text='<%# " Created By: " & mDentBuckle.CreatedBy %>' />
																	</td>
																	<td>
																		<asp:Label ID="lblCreatedByDateTime"
																			CssClass="clsLabelAuto" runat="server"
																			Font-Size="Small" ForeColor="Navy"
																			Text='<%# " On: " & mDentBuckle.CreateDateTimeStampFormatted %>' />
																	</td>
																	<td>
																		<asp:Label ID="lblLastUpdatedBy"
																			CssClass="clsLabelAuto" runat="server"
																			Font-Size="Small" ForeColor="Navy"
																			Text='<%# " Last Updated By: " & mDentBuckle.LastUpdatedBy %>' />
																	</td>
																	<td>
																		<asp:Label ID="lblLastUpdatedByDateTime"
																			CssClass="clsLabelAuto" runat="server"
																			Font-Size="Small" ForeColor="Navy"
																			Text='<%#" On: " & mDentBuckle.UpdateDateTimeStampFormatted %>' />
																	</td>
																</tr>
															</table>
														</div>
													</td>
													<td align="right" style="vertical-align: top;">
														<asp:Label ID="lblStatus" runat="server"
															Text="<%# mDentBuckle.StatusName %>"
															CssClass="clsLabelHeader" />
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td valign="top">
									<table width="100%">
										<tr>
											<td>
												<fieldset class="clsFieldSetNewStyle">
													<legend><b>Dent & Repair Details</b></legend>
													<asp:UpdatePanel runat="server" ID="upnlDetails" UpdateMode="Conditional">
														<ContentTemplate>
															<table width="100%">
																<tr>
																	<td>
																		<span id="lblDateStar" class="clsLabelStar">*</span>
																	</td>
																	<td>
																		<span id="lblDate" class="clsLabelAuto">Date</span>
																	</td>
																	<td>
																		<asp:TextBox ID="txtReportDate" runat="server" 
																			ClientIDMode="Static" CssClass="clsTextBoxTagSearchDate"
																			AutoPostBack="true" OnTextChanged="ReportDateChanged" 
																			onchange="ValidateDateText(this,'Date_watermarkextender','true');"
																			Text="" Width="100px" />
																		<cc2:CalendarExtender ID="txtReportDate_CalendarExtender" 
																			runat="server" CssClass="cal_Theme1"
																			Enabled="true" Format="<%$AppSettings:DateFormat%>" 
																			TargetControlID="txtReportDate" />
																		<cc2:TextBoxWatermarkExtender ID="txtReportDateWatermarkExtender"
																			runat="server"
																			TargetControlID="txtReportDate" 
																			WatermarkCssClass="clsDateTextBox"
																			WatermarkText="<%$AppSettings:DateFormat%>" />
																	</td>
																	<td>
																		<span id="lblNoStar" class="clsLabelStar">*</span>
																	</td>
																	<td>
																		<span id="lblNo" class="clsLabelAuto">No.</span>
																	</td>
																	<td>
																		<asp:TextBox ID="txtText" runat="server"
																			CssClass="clsTextBoxTagSearch" MaxLength="25"
																			onfocus="SetContextKey()" Text="<%# mDentBuckle.Text %>"
																			ToolTip="Enter No." />
																		<cc2:AutoCompleteExtender ID="txtText_Autocomplete" 
																			runat="server"
																			ClientIDMode="Static"
																			CompletionInterval="1"
																			CompletionSetCount="20"
																			DelimiterCharacters="" Enabled="True"
																			MinimumPrefixLength="0" 
																			ServiceMethod="GetDistinctTextListAutoComplete"
																			ServicePath="wfPurchaseOrder_Ajax.aspx"
																			TargetControlID="txtText" UseContextKey="False" />
																		<asp:TextBox ID="txtNo" runat="server" 
																			ClientIDMode="Static" 
																			CssClass="clsTextBoxTagSearchSmall" Enabled="True"
																			MaxLength="8" Text="<%# mDentBuckle.No %>" />
																		<script type="text/jscript">

																			function SetContextKey() {
																				var autoComplete = $find('txtText_Autocomplete');
																				var TransTypeID = 'TransTypeID=<%=mDentBuckle.TransTypeID%>¿OrderDate=<%=mDentBuckle.ReportDate%>';
																				autoComplete.set_contextKey(TransTypeID);
																			}

																		</script>
																	</td>
																</tr>
																<tr>
																	<td>
																		<span id="Span1" class="clsLabelStar">*</span>
																	</td>
																	<td>
																		<span class="clsLabelAuto">Aircraft</span>
																	</td>
																	<td>
																		<asp:DropDownList ID="cmbMachineList" runat="server" 
																			AutoPostBack="True" CssClass="clsTextBoxTagSearchComboNewstyle"
																			DataTextField="RegNo" DataValueField="ID" 
																			Width="130px" />
																	</td>
																	<td></td>
																	<td>
																		<span class="clsLabelAuto">Log</span>
																	</td>
																	<td>
																		<asp:DropDownList ID="cmbLogList" runat="server" 
																			CssClass="clsTextBoxTagSearchComboNewstyle"
																			DataTextField="LogNo"
																			DataValueField="LogID" Width="130px" />
																	</td>
																</tr>
																<tr>
																	<td></td>
																	<td colspan="2">
																		<asp:LinkButton ID="lnkViewChart" runat="server"
																			CssClass="clsLinkButton" Text="View Chart" />
																	</td>
																	<td></td>
																	<td>
																		<span class="clsLabelAuto">Revision No. & Date</span>
																	</td>
																	<td>
																		<asp:TextBox ID="txtRevNo" runat="server"
																			CssClass="clsTextBoxTagSearch"
																			Enabled="<%# mDentBuckle.StatusID = 1 %>"
																			MaxLength="50" Text="<%# mDentBuckle.RevNo %>" 
																			ToolTip="Enter Revision No." />
																		<asp:TextBox ID="txtRevDate" runat="server" 
																			ClientIDMode="Static"
																			CssClass="clsTextBoxTagSearchDate"
																			onchange="ValidateDateText(this,'Date_watermarkextender','false');"
																			Text="<%# mDentBuckle.RevDateFormatted %>"
																			ToolTip="Enter Revision Date" Width="100px" />
																		<cc2:CalendarExtender ID="txtRevDate_CalendarExtender" 
																			runat="server" CssClass="cal_Theme1"
																			Enabled="true" Format="<%$AppSettings:DateFormat%>" 
																			TargetControlID="txtRevDate" />
																		<cc2:TextBoxWatermarkExtender ID="txtQuotationDateWatermarkExtender"
																			runat="server" TargetControlID="txtRevDate" 
																			WatermarkText="<%$AppSettings:DateFormat%>" />
																	</td>
																</tr>
																<tr>
																	<td></td>
																	<td>
																		<span id="lblAttachFile" class="clsLabelAuto">Attach File</span>
																	</td>
																	<td colspan="3">
																		<asp:UpdatePanel ID="upnlFileupload" runat="server"
																			UpdateMode="Conditional">
																			<ContentTemplate>
																				<table border="0" cellpadding="0" cellspacing="0">
																					<tr>
																						<td>
																							<input type="button" id="btnSelectFile"
																								value="Select File"
																								style="width: 120px; margin-inline: 10px;"
																								runat="server" class="clsbtnH clsinfoH1" />
																						</td>
																						<td>
																							<asp:Button ID="btnDelAttach" runat="server"
																								CssClass="clsbtnH clsinfoH1" Enabled="False"
																								Text="Remove Attachment" Width="140px"
																								ToolTip="Click to Remove Attachment" />
																						</td>
																						<td>
																							<asp:ImageButton ID="ImageButton1" runat="server"
																								CausesValidation="False" Height="20px"
																								ImageUrl="icons/CLIP01.ICO" Width="20px" />
																						</td>
																					</tr>
																				</table>
																			</ContentTemplate>
																		</asp:UpdatePanel>
																	</td>
																</tr>
															</table>
														</ContentTemplate>
													</asp:UpdatePanel>
												</fieldset>
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel runat="server" ID="upnlItems" UpdateMode="Conditional">
										<ContentTemplate>
											<table width="100%">
												<tr>
													<td>
														<asp:Label runat="server" ID="lblOrderItems"
															CssClass="clsLabelHeader"
															Text="Dent & Repair Report(s)" />
													</td>
												</tr>
												<tr>
													<td>
														<asp:GridView ID="dgItems" runat="server" ShowHeaderWhenEmpty="True"
															AutoGenerateColumns="False" CssClass="clsGridNewStyle" 
															GridLines="Horizontal" CellPadding="5">
															<AlternatingRowStyle CssClass="clsdgAltItem" />
															<RowStyle CssClass="clsdgItem" />
															<HeaderStyle BackColor="white" CssClass="clsdgHeader" 
																Font-Bold="True" ForeColor="black" HorizontalAlign="Left" />
															<FooterStyle BackColor="#CCCC99" ForeColor="Black" />
															<PagerSettings Mode="NumericFirstLast" FirstPageText="First"
																LastPageText="Last" />
															<PagerStyle BackColor="White" CssClass="paging" 
																ForeColor="Black" HorizontalAlign="Right" />
															<Columns>
																<%--0--%>
																<asp:BoundField DataField="SrNo" HeaderText="Sr.No.">
																	<HeaderStyle HorizontalAlign="Left" />
																</asp:BoundField>
																<%--1--%>
																<asp:BoundField DataField="ItemNo" HeaderText="Item No.">
																	<HeaderStyle Wrap="True" HorizontalAlign="Left" />
																	<ItemStyle Wrap="True" />
																</asp:BoundField>
																<%--2--%>
																<asp:BoundField DataField="Description" HeaderText="Description">
																	<HeaderStyle Wrap="True" HorizontalAlign="Left" Width="150px" />
																	<ItemStyle Wrap="True" Width="150px" />
																</asp:BoundField>
																<%--3--%>
																<asp:BoundField DataField="ATACode" HeaderText="ATA">
																	<HeaderStyle Wrap="True" HorizontalAlign="Left" />
																	<ItemStyle Wrap="True" />
																</asp:BoundField>
																<%--4--%>
																<asp:BoundField DataField="AcceptanceByName" HeaderText="Acceptance By">
																	<HeaderStyle Wrap="True" HorizontalAlign="Left" />
																	<ItemStyle Wrap="True" />
																</asp:BoundField>
																<%--5--%>
																<asp:BoundField DataField="ReportedByName" HeaderText="Reported By">
																	<HeaderStyle Wrap="True" HorizontalAlign="Left" />
																	<ItemStyle Wrap="True" />
																</asp:BoundField>
																<%--6--%>
																<asp:BoundField DataField="ActionTakenByName" HeaderText="Action Taken By">
																	<HeaderStyle Wrap="True" HorizontalAlign="Left" />
																	<ItemStyle Wrap="True" />
																</asp:BoundField>
																<%--7--%>
																<asp:BoundField DataField="ItemStatusName" HeaderText="Item Status">
																	<HeaderStyle Wrap="True" HorizontalAlign="Left" />
																	<ItemStyle Wrap="True" />
																</asp:BoundField>
																<%--8--%>
																<asp:TemplateField HeaderStyle-HorizontalAlign="Center"
																	HeaderText="Action" ItemStyle-HorizontalAlign="Center">
																	<HeaderStyle HorizontalAlign="Center" />
																	<ItemStyle HorizontalAlign="Center" />
																	<ItemTemplate>
																		<div id="dropDownImg" class="dropdown">
																			<asp:Image ID="arrowICN" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn" />
																			<div id="dropdownICN-content" class="dropdownbtn-content">
																				<table id="dropdown-content" class="clsGridNew_Ajax">
																					<tr>
																						<td>
																							<asp:ImageButton ID="editICN" 
																								CssClass="actionICNS" runat="server"
																								CommandArgument='<%# Eval("SrNo") %>' 
																								CommandName="EditView"
																								ToolTip="Click to Edit record" 
																								CausesValidation="false"
																								ImageUrl="~/images/edit.png" />
																						</td>
																						<td>
																							<asp:ImageButton ID="deleteICN" 
																								CssClass="actionICNS  largerActionICNS"
																								runat="server" 
																								CommandArgument='<%# Eval("SrNo") %>'
																								ToolTip="Click to Delete record" 
																								CausesValidation="false"
																								CommandName="DeleteRecord" 
																								ImageUrl="~/images/delete.png" />
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
							</tr>
							<tr style="height: 0px;">
								<td style="height: 0px;">
									<asp:UpdatePanel runat="server" ID="upnlBtnFileUpload" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
												CausesValidation="False" Style="display: none;" />
											<asp:Button ID="hdnimgBtnItems" ClientIDMode="Static" runat="server" Text="----"
												CausesValidation="False" Style="display: none;" />
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

		<!-- Items Popup Window -->
		<div style="display: none">
			<asp:Button runat="server" ID="btnDummyItems" Text="Dummy Items" ClientIDMode="Static" />
		</div>
		<asp:Panel runat="server" ID="pnlPopupItems" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
			<iframe id="iPopupItems" frameborder="0" allowtransparency="true" height="100%" width="100%"
				src="JavaScript:''" scrolling="auto"></iframe>
		</asp:Panel>
		<cc2:ModalPopupExtender ID="mdlPopupItems" runat="server" TargetControlID="btnDummyItems"
			PopupControlID="pnlPopupItems" BackgroundCssClass="clsModalPopupBG">
		</cc2:ModalPopupExtender>
		<script type="text/javascript">
			function IFrameItemsStateComplete() {
				$("#btnDummyItems").click();
				$get("AjaxLoader").style.visibility = "hidden";
			}

			function OpenItemsWindow() {
				try {

					$get("AjaxLoader").style.visibility = "visible";
					$("#iPopupItems").attr("src", "wfDentBuckelItems_Ajax.aspx?Type=pup");
					if (!$.browser.msie) {
						$("#btnDummyItems").click();
						$get("AjaxLoader").style.visibility = "hidden";
					}

					return false;
				} catch (e) {
					alert(e);
				}
			}
		</script>
		
		<script type="text/javascript">
			function ParentCallBackFunctionForItems() {
				var ItemsWindow = $find("<%=mdlPopupItems.ClientID %>");
				//close Items popup window
				ItemsWindow.hide();
				$("#iPopupItems").attr("src", "JavaScript:''");
				//call ata image button
				$("#hdnimgBtnItems").click();
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

</body>
</html>
