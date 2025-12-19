<%@ Page Language="vb" AutoEventWireup="false"
	CodeBehind="wfDiscrepancyTroubleShoot.aspx.vb"
	Inherits="Flypal.DiscrepancyTroubleShoot" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
	<title>Discrepancy TroubleShoot</title>
	<meta http-equiv="x-ua-compatible" content="IE=9" />
	<meta name="vs_showGrid" content="True">
	<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
	<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
	<meta name="vs_defaultClientScript" content="JavaScript">
	<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	<link id="MainStyle" rel="stylesheet" type="text/css">

	<script language="javascript" src="VALIDATEFUNCTIONS.js" />
	<script language="javascript">
		function openledgersame(FileName) {
			window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
		}
	</script>

	<asp:PlaceHolder runat="server">
		<!-- #include file= "LocalFunctionAjax.htm" -->
	</asp:PlaceHolder>

</head>
<body>
	<form id="Form1" runat="server">
		<asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
			runat="server">
		</asp:ScriptManager>
		<asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
			</ContentTemplate>
		</asp:UpdatePanel>
		<div>
			<asp:UpdatePanel ID="upnlMaint" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
					<asp:Panel ID="pnlMain" runat="server" CssClass="clspnl1">
						<table id="tblmain" class="clstablelistout" style="width: 75%">
							<tr>
								<td class="clsFormHeader1Newstyle">
									<table width="100%">
										<tr>
											<td>
												<asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<asp:Label ID="lblTitle" runat="server"
															CssClass="clsFormHeader" />
													</ContentTemplate>
												</asp:UpdatePanel>
											</td>
											<td align="right">
												<asp:UpdatePanel ID="upnlAdd" runat="server" UpdateMode="Conditional">
													<ContentTemplate>
														<table>
															<tr>
																<td>
																	<asp:Button ID="btnSaveAndClose"
																		TabIndex="0" runat="server"
																		Text="Save & Close"
																		CausesValidation="true" Visible="false"
																		ValidationGroup="a"
																		CssClass="clsbtnH clsinfoH"
																		ToolTip="Save & Close" />
																</td>
																<td>
																	<asp:Button ID="btnSave" runat="server" Text="Save"
																		CssClass="clsbtnH clsinfoH"
																		ToolTip="Save current Record"
																		ValidationGroup="a" />
																</td>
																<td>
																	<asp:Button ID="btnBack" TabIndex="0" runat="server"
																		CausesValidation="False" Text="Back"
																		CssClass="clsbtnH clsinfoH"
																		ToolTip="Go to Previous page" />
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
									<asp:UpdatePanel ID="upnlErrorList" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:ValidationSummary ID="Validationsummary2" runat="server"
												CssClass="clsValidationSummary"
												ValidationGroup="a"
												HeaderText="Fill Up The Following Fields" />
											<asp:CustomValidator Style="z-index: 0" ID="cvMainActivityList"
												runat="server" CssClass="clsValidationSummary"
												ValidationGroup="a" ControlToValidate="txtMainActivity"
												Display="None" OnServerValidate="CustomValidation" />
											<asp:RequiredFieldValidator ID="rfvDescription" runat="server"
												CssClass="clsValidationSummary"
												ErrorMessage="Description required"
												ControlToValidate="txtMainActivity"
												OnServerValidate="CustomValidate_Description"
												Display="None" />
											<asp:CustomValidator Style="z-index: 0" ID="cvControlValidator"
												runat="server" CssClass="clsValidationSummary"
												ValidationGroup="a"
												Display="None" ControlToValidate="txtMainActivity" />
											<asp:CustomValidator Style="z-index: 0" ID="cvDoneBy"
												runat="server" CssClass="clsValidationSummary"
												ValidationGroup="a" ControlToValidate="txtLicenceNo"
												Display="None" OnServerValidate="CustomValidation" />
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td>
									<asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
										<ContentTemplate>
											<table width="100%">
												<tr>
													<td>
														<asp:Label ID="Label1" runat="server"
															CssClass="clsLabelStar" Style="z-index: 0">*</asp:Label></td>
													<td>
														<asp:Label Style="z-index: 0" ID="lblDate"
															runat="server" CssClass="clsLabelAuto">Log No.</asp:Label>
													</td>
													<td>
														<asp:UpdatePanel ID="upnlLog" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
																<table>
																	<tr>
																		<td>
																			<asp:DropDownList ID="cmbLog" runat="server"
																				CssClass="clsTextBoxTagSearchComboNewstyle"
																				DataTextField="LogNoLogPageNo"
																				DataValueField="ID" AutoPostBack="true" />
																			<asp:TextBox Style="z-index: 0" ID="txtLogNoDet"
																				runat="server" CssClass="clsTextBoxTagSearch"
																				BackColor="Gainsboro" ReadOnly="True" />
																		</td>
																		<td>
																			<asp:Label ID="lblLogDate" runat="server"
																				CssClass="clsLabelAuto"
																				Text='<%#IIf(AppSettings("ClientCode") = "7AR", "Log Date (UTC)", "Log Date") %>'>
																										Log Date</asp:Label>
																		</td>
																		<td>
																			<asp:TextBox Style="z-index: 0" ID="txtLogDate"
																				runat="server" CssClass="clsTextBoxTagSearch"
																				Enabled="false"
																				Width="100px" BackColor="White" />
																		</td>
																		<asp:PlaceHolder ID="phMaintOn" runat="server" Visible="false">

																			<td>
																				<span id="lblMaintOn" class="clsLabelAuto">Maintenance On</span>
																			</td>
																			<td>
																				<asp:DropDownList ID="cmbAssembly" runat="server"
																					CssClass="clsTextBoxTagSearchCombo"
																					DataTextField="ModelSerialNoPostion"
																					DataValueField="AssemblyStatusID" Width="200px">
																				</asp:DropDownList>
																			</td>
																		</asp:PlaceHolder>
																	</tr>
																</table>
															</ContentTemplate>
														</asp:UpdatePanel>
													</td>
													<td align="right">
														<asp:Label ID="lblTroubleCount" runat="server"
															CssClass="clsLabelHeader" Style="z-index: 0" />
													</td>
												</tr>
												<tr>
													<td>
														<asp:Label ID="lblStar" runat="server"
															CssClass="clsLabelStar"
															Style="z-index: 0">*</asp:Label>
													</td>
													<td>
														<asp:Label ID="lblDescription" runat="server"
															CssClass="clsLabelAuto" Style="z-index: 0">
																					Troubleshooting Steps</asp:Label>
													</td>
													<td colspan="2">
														<asp:TextBox Style="z-index: 0" ID="txtMainActivity"
															runat="server" CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
															Width="600px" BackColor="White" TextMode="MultiLine" />
													</td>
												</tr>
												<tr>
													<td>&nbsp;
													</td>
													<td>
														<asp:Label ID="Label2" runat="server" CssClass="clsLabelAuto"
															Style="z-index: 0">NRC / W.O. No</asp:Label>
													</td>
													<td colspan="2">
														<table>
															<tr>
																<td>
																	<asp:TextBox ID="txtNCRNo" runat="server"
																		BackColor="White" CssClass="clsTextBoxTagSearch"
																		MaxLength="50" Style="z-index: 0" />
																</td>
																<td>&nbsp;
																</td>
																<td>
																	<asp:Label ID="lblDoneBy" runat="server"
																		CssClass="clsLabelAuto" Style="z-index: 0">
																								Work Carried Out By</asp:Label>
																</td>
																<td>
																	<asp:UpdatePanel ID="upnlLicenceNo" runat="server"
																		UpdateMode="Conditional">
																		<ContentTemplate>
																			<table>
																				<tr>
																					<td>
																						<asp:TextBox ID="txtLicenceNo" runat="server"
																							CssClass="clsTextBoxTagSearch"
																							ToolTip="Enter License No."
																							AutoComplete="off" ClientIDMode="Static"
																							onchange="SetEmployeeIdonChange(this,'txtLicenceNo_AutoComplete')"
																							AutoPostBack="true" MaxLength="200" />
																						<cc2:AutoCompleteExtender ClientIDMode="Static"
																							ID="txtLicenceNo_AutoComplete" runat="server"
																							DelimiterCharacters="" Enabled="True"
																							CompletionSetCount="20" MinimumPrefixLength="1"
																							CompletionInterval="0" ServicePath=""
																							ServiceMethod="GetEmployeeList"
																							TargetControlID="txtLicenceNo"
																							UseContextKey="True" ContextKey=""
																							CompletionListCssClass="ac_results_Main"
																							CompletionListItemCssClass="ac_results_li"
																							CompletionListHighlightedItemCssClass="ac_over_Main" OnClientPopulated="ClientPopulated"
																							OnClientPopulating="ClientPopulating"
																							OnClientHiding="ClientHiding"
																							OnClientItemSelected="SetID"
																							OnClientShown="ClientHiding"
																							OnClientShowing="ClientShowing">
																						</cc2:AutoCompleteExtender>
																					</td>
																					<td>
																						<asp:ImageButton ID="imgbtnEmployeeLicence"
																							runat="server" ImageUrl="~/images/plus1.png"
																							Visible="false" Height="22px" Width="24px"
																							ToolTip="Click to select multiple Licence No."
																							CausesValidation="false" />
																					</td>
																					<td>
																						<asp:Label ID="lblLicenceCount" runat="server"
																							Text="and More" Visible="false"
																							CssClass="clsLabelHeader clsCursorStyle" />
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
													<td>&nbsp;
													</td>
													<td>
														<asp:Label ID="lblClose" runat="server" CssClass="clsLabelAuto">Close</asp:Label>
													</td>
													<td colspan="2">
														<table>
															<tr>
																<td>
																	<asp:UpdatePanel ID="upnlClose" runat="server" UpdateMode="Conditional">
																		<ContentTemplate>
																			<asp:CheckBox ID="chkClose" runat="server"
																				CssClass="clsCheckBox" />
																		</ContentTemplate>
																	</asp:UpdatePanel>
																</td>
																<td>&nbsp;
																</td>
																<asp:PlaceHolder ID="phClosedDate" runat="server" Visible="false">
																	<td>
																		<asp:Label ID="lblClosedDate" runat="server"
																			CssClass="clsLabelAuto">Closed Date</asp:Label>
																	</td>
																	<td>
																		<asp:TextBox ID="calClosedDate" runat="server"
																			AutoPostBack="True" CssClass="clsTextBoxTagSearch"
																			Width="100px" />
																		<cc2:CalendarExtender ID="calClosedDate_CalendarExtender"
																			runat="server" CssClass="cal_Theme1"
																			Enabled="True" Format="<%$AppSettings:DateFormat%>"
																			TargetControlID="calClosedDate" />
																		<cc2:TextBoxWatermarkExtender ID="calClosedDateWatermarkExtender"
																			runat="server" TargetControlID="calClosedDate"
																			WatermarkCssClass="clsDateTextBox"
																			WatermarkText="<%$AppSettings:DateFormat%>" />
																	</td>
																</asp:PlaceHolder>
															</tr>
														</table>
													</td>
												</tr>
												<tr>
													<td>&nbsp;
													</td>
													<td>
														<asp:Label ID="lblAttachFile1" runat="server"
															CssClass="clsLabelAuto">Attach File</asp:Label>
													</td>
													<td colspan="2">
														<asp:UpdatePanel ID="upnlAttach" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
																<table border="0" cellpadding="0" cellspacing="0">
																	<tr>
																		<td>
																			<input type="button" runat="server"
																				id="btnSelectFile" value="Select File"
																				style="width: 100px;"
																				class="clsbtnH clsinfoH1" />
																		</td>
																		<td style="padding-left: 3px;">
																			<asp:Button ID="btnDelAttach" runat="server"
																				CssClass="clsbtnH clsinfoH1" Enabled="False"
																				Text="Remove Attachment"
																				ToolTip="Remove the Attachment added."
																				Width="140px" CausesValidation="false" />
																		</td>
																		<td style="padding-left: 3px;">
																			<asp:ImageButton ID="ImageButton2" runat="server"
																				CausesValidation="False"
																				Height="20px" Visible="false"
																				ImageUrl="icons/CLIP01.ICO" Width="20px" />
																		</td>
																	</tr>
																</table>
															</ContentTemplate>
														</asp:UpdatePanel>
													</td>
												</tr>
												<tr>
													<td>
														<br />
													</td>
												</tr>
												<tr>
													<td colspan="4">
														<asp:UpdatePanel ID="upnlRecCnt" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
																<asp:Label ID="lblRecCount" runat="server"
																	CssClass="clsLabelHeader" Style="z-index: 0" />
															</ContentTemplate>
														</asp:UpdatePanel>
													</td>

												</tr>
												<tr>
													<td colspan="4">
														<asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
															<ContentTemplate>
																<div>
																	<asp:GridView ID="dgDiscrepancyTroubleShootList"
																		runat="server" DataKeyNames="ID"
																		ShowHeaderWhenEmpty="True" AllowSorting="True"
																		AllowPaging="True"
																		AutoGenerateColumns="False" PageSize="10"
																		CssClass="clsGridNewStyle" GridLines="Horizontal"
																		CellPadding="5">
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
																			<asp:BoundField DataField="ID" HeaderText="ID"
																				Visible="false"></asp:BoundField>
																			<%--1--%>
																			<asp:BoundField DataField="LogID" SortExpression="LogID"
																				HeaderText="LogID" Visible="false">
																				<HeaderStyle HorizontalAlign="Left" />
																			</asp:BoundField>
																			<%--2--%>
																			<asp:BoundField DataField="RecordCount"
																				SortExpression="RecordCount" HeaderText="Sr.No."
																				HtmlEncode="False"></asp:BoundField>
																			<%--3--%>
																			<asp:BoundField DataField="LogDateFormatted"
																				HeaderText="Log Date">
																				<HeaderStyle HorizontalAlign="Left" />
																				<ItemStyle Wrap="False" />
																			</asp:BoundField>
																			<%--4--%>
																			<asp:BoundField DataField="LogNoPageNo"
																				SortExpression="LogNoPageNo" 
																				HeaderText="Log No."
																				HtmlEncode="False">
																				<ItemStyle Wrap="False" />
																			</asp:BoundField>
																			<%--5--%>
																			<asp:BoundField DataField="Maintenance"
																				SortExpression="Maintenance"
																				HeaderText="Troubleshooting Steps">
																				<HeaderStyle HorizontalAlign="Left" />
																			</asp:BoundField>
																			<%--6--%>
																			<asp:BoundField DataField="NRCWONO"
																				SortExpression="NRCWONO" 
																				HeaderText="NRC / W.O. No">
																				<ItemStyle Wrap="False" />
																			</asp:BoundField>
																			<%--7--%>
																			<asp:BoundField DataField="DoneByName"
																				SortExpression="DoneByName"
																				HeaderText="Work Carried Out By">
																				<HeaderStyle HorizontalAlign="Left" Wrap="False" />
																			</asp:BoundField>
																			<%--8--%>
																			<asp:BoundField DataField="Place" HeaderText="Place"
																				HeaderStyle-CssClass="hideGridColumn"
																				ItemStyle-CssClass="hideGridColumn">
																				<ItemStyle Wrap="False" />
																			</asp:BoundField>
																			<%--9--%>
																			<asp:BoundField DataField="SrNo" SortExpression="SrNo"
																				HeaderText="Sr No."
																				HtmlEncode="False" 
																				HeaderStyle-CssClass="hideGridColumn"
																				ItemStyle-CssClass="hideGridColumn">
																				<HeaderStyle HorizontalAlign="Right" />
																				<ItemStyle HorizontalAlign="Right" />
																			</asp:BoundField>
																			<%--10--%>
																			<asp:BoundField DataField="AssemblyStatusID"
																				HeaderText="AssemblyStatusID" Visible="false">
																				<HeaderStyle HorizontalAlign="Left" />
																				<ItemStyle Wrap="False" />
																			</asp:BoundField>
																			<%--11--%>
																			<asp:BoundField DataField="LogTypeId"
																				SortExpression="LogTypeId" HeaderText="Log Type Id"
																				Visible="false">
																				<HeaderStyle HorizontalAlign="Left" />
																			</asp:BoundField>
																			<%--12--%>
																			<asp:TemplateField HeaderStyle-HorizontalAlign="Center"
																				HeaderText="Action" ItemStyle-HorizontalAlign="Center">
																				<HeaderStyle HorizontalAlign="Center" />
																				<ItemStyle HorizontalAlign="Center" />
																				<ItemTemplate>
																					<div id="dropDownImg" class="dropdown">
																						<asp:Image ID="arrowICN"
																							ImageUrl="~/images/Arrowup.png" 
																							runat="server"
																							CssClass="clsActionbtn" />
																						<div id="dropdownICN-content"
																							class="dropdownbtn-content">
																							<table id="dropdown-content"
																								class="clsGridNew_Ajax">
																								<tr>
																									<td>
																										<asp:ImageButton ID="editICN"
																											CssClass="actionICNS"
																											runat="server"
																											CommandArgument='<%# Eval("ID") %>'
																											CommandName="EditRec"
																											ToolTip="Edit this record."
																											CausesValidation="false"
																											ImageUrl="~/images/edit.png" />
																									</td>

																									<td>
																										<asp:ImageButton ID="deleteICN"
																											runat="server"
																											CssClass="actionICNS  largerActionICNS"
																											CommandArgument='<%# Eval("ID") %>'
																											ToolTip="Delete this record."
																											CommandName="DeleteRec"
																											ImageUrl="~/images/delete.png"
																											CausesValidation="false" />
																									</td>
																									<td>
																										<asp:ImageButton ID="viewICN"
																											CssClass="attachmentICNS"
																											runat="server"
																											CommandArgument='<%# Eval("ID") %>'
																											ToolTip="View the Attachment Added."
																											CommandName="ViewRec"
																											ImageUrl="icons/CLIP01.ICO"
																											CausesValidation="false"
																											Visible='<%#  Eval("ImageSize") > 0 %>' />
																									</td>
																								</tr>
																							</table>
																						</div>
																					</div>
																				</ItemTemplate>
																			</asp:TemplateField>

																		</Columns>
																	</asp:GridView>
																</div>
															</ContentTemplate>
														</asp:UpdatePanel>
													</td>
												</tr>
												<!--Dummy panel to open modelpopup for FileUpload-->
												<tr style="height: 0px;">
													<td style="height: 0px;">
														<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
															<ContentTemplate>
																<asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static"
																	runat="server" Text="----"
																	CausesValidation="False" Style="display: none;" />
																<asp:Button ID="hdnBtnMaintDoneBy" ClientIDMode="Static"
																	runat="server" Text="----"
																	CausesValidation="False" Style="display: none;" />
															</ContentTemplate>
														</asp:UpdatePanel>
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
						</table>
					</asp:Panel>
				</ContentTemplate>
			</asp:UpdatePanel>

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

		<script type="text/javascript">

			function CallParentCallback() {
				parent.ParentCallBackFunctionForDiscrepancyTroubleShoot();
				return false;
			}

			function CallautoResize() {
				parent.autoResizeMaintActivity();
				return false;
			}

		</script>

		<!-- File Upload Modal Dialog-->
		<div>

			<div style="display: none">
				<asp:HiddenField runat="server" ID="btnDummyFileUpload" />
			</div>
			<asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
				<iframe id="IFileUpload" frameborder="0" height="100%" width="100%" allowtransparency="true"
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
							$("#IFileUpload").attr("src", "wfFileUpload.aspx");
							$("#IFileUpload").ready(function () {
								$("#btnDummyFileUpload").click();
								$get("AjaxLoader").style.visibility = 'hidden';
							});

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

		</div>
		<!-- End File Upload Modal Dialog-->

		<!--  Maintenance Done By Employee Dialog-->
		<div>

			<div style="display: none">
				<asp:HiddenField runat="server" ID="btnDummyMaintDoneBy" />
			</div>
			<asp:Panel runat="server" ID="pnlMaintDoneBy" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
				<iframe id="IMaintDoneBy" allowtransparency="true" frameborder="0" height="100%"
					width="100%" src="JavaScript:''" scrolling="auto"></iframe>
			</asp:Panel>
			<cc2:ModalPopupExtender ID="mdlPopupMaintDoneBy" runat="server" TargetControlID="btnDummyMaintDoneBy"
				PopupControlID="pnlMaintDoneBy" BackgroundCssClass="clsModalPopupBG">
			</cc2:ModalPopupExtender>
			<script type="text/javascript">
				function IFrameMaintDoneByStateComplete() {
					$("#btnDummyMaintDoneBy").click();
					$get("AjaxLoader").style.visibility = 'hidden';
				}


				function AddEmployeeLicNo() {
					try {
						$get("AjaxLoader").style.visibility = 'visible';
						$("#IMaintDoneBy").attr("src", "wfMaintenanceDoneByEmployee_Ajax.aspx?Type=pup&MaintTypeID=12");

						if (!$.browser.msie) {
							$("#btnDummyMaintDoneBy").click();
							$get("AjaxLoader").style.visibility = 'hidden';
						}

						return false;
					} catch (e) {
						alert(e);
					}


				}

			</script>

			<script type="text/javascript">
				function ParentCallBackFunctionForMaintDoneBy() {
					var MaintDoneBywindow = $find("<%=mdlPopupMaintDoneBy.ClientID %>");
					//close Ass Insp Maint Done By Emp popup window
					MaintDoneBywindow.hide();
					//Free resources
					$("#IMaintDoneBy").attr("src", "JavaScript:''");
					$("#hdnBtnMaintDoneBy").click();

				}
			</script>

		</div>
		<!-- End -->

		<!-- Preventive Measures -->
		<div>

			<div style="display: none">
				<asp:Button runat="server" ID="btnDummyPreventiveMeasures" Text="Preventive Measures" />
			</div>
			<asp:Panel runat="server" ID="pnlPreventiveMeasures" Style="display: none">
				<div>
					<table class="clstablelistout" id="Table1">
						<tr>
							<td align="right">
								<asp:UpdatePanel runat="server" ID="upnlPreventiveMeasures" UpdateMode="Conditional">
									<ContentTemplate>
										<table class="clstablelistin" id="TablePreventiveMeasures">
											<tr>
												<td align="left" class="clsFormHeader1Newstyle">
													<table width="100%">
														<tr>
															<td>
																<span id="Span1" class="clsFormHeader">Watchlist</span>
															</td>
															<td valign="top" align="right">
																<table id="Table4" cellspacing="1" cellpadding="1">
																	<tr>
																		<td>
																			<asp:Button ID="btnWatchlisteSave"
																				ValidationGroup="1" runat="server"
																				CssClass="clsbtnH clsinfoH"
																				Text="Save" ToolTip="Save details" />
																		</td>
																		<td>
																			<asp:Button ID="btnWatchlisteClose" runat="server"
																				CssClass="clsbtnH clsinfoH"
																				Text="Close" ToolTip="Close this screen"
																				CausesValidation="False" />
																		</td>
																	</tr>
																</table>
															</td>
														</tr>
													</table>

												</td>
											</tr>
											<tr>
												<td align="left">
													<asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="1" runat="server"
														CssClass="clsValidationSummary" />
													<asp:RequiredFieldValidator ID="rfvPreventiveMeasures"
														runat="server" CssClass="clsValidationSummary"
														ErrorMessage="Watchlist Instructions required"
														ControlToValidate="txtPreventiveMeasures" Display="None" />
												</td>
											</tr>
											<tr>
												<td>
													<table width="100%">
														<tr>
															<td></td>
															<td>
																<span id="lblPreventiveMeasures" class="clsLabel">Watchlist Instructions</span>
															</td>
															<td colspan="1">
																<asp:TextBox ID="txtPreventiveMeasures" runat="server"
																	CssClass=" clsTextBoxTagSearchMultilineNewstyle"
																	TextMode="MultiLine" />
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
				</div>
			</asp:Panel>


			<cc2:ModalPopupExtender ID="mdlPopUpPreventiveMeasures" runat="server" TargetControlID="btnDummyPreventiveMeasures"
				PopupControlID="pnlPreventiveMeasures" BackgroundCssClass="clsModalPopupBG">
			</cc2:ModalPopupExtender>

		</div>
		<!-- End -->

		<asp:HiddenField runat="server" ClientIDMode="Static" ID="EmployeeID" />

		<%-- Autocomplete functions to set id--%>
		<script type="text/javascript">

			function SetID(source, e) {

				//get id from autocomplete list
				var node;
				var value = e.get_value();

				if (value) node = e.get_item();
				else {
					value = e.get_item().parentNode._value;
					node = e.get_item().parentNode;
				}
				//Set id to relevent hidden field 
				var textbox;
				if (source._id == "txtLicenceNo_AutoComplete") {
					textbox = document.getElementById('EmployeeID');
				}
				textbox.value = value;
			}

			//text change function : if id found,set id to hiddenfield and return ,else clear the hidden field value..
			function SetEmployeeIdonChange(source, extenderid) {
				var popup = $find(extenderid);
				var complist = popup.get_completionList();
				var text = $(source).val().toLowerCase();
				for (var i = 0; i < complist.childNodes.length; i++) {
					var texttocompare = complist.childNodes[i].innerText.toLowerCase();
					if (text == texttocompare) {
						var val = complist.childNodes[i]._value;

						if (extenderid == "txtLicenceNo_AutoComplete") {
							textbox = document.getElementById('EmployeeID');
						}
						textbox.value = val;
						return;
					}

				}

				if (extenderid == "txtLicenceNo_AutoComplete") {
					document.getElementById('EmployeeID').value = '';
				}
			}

		</script>

		<%--autocomplete css functions--%>
		<script type="text/javascript">

			//bold input value in list...
			function ClientPopulated(source, eventArgs) {
				$("#" + source._element.id).removeClass("ac_loading");
			}

			//Alternate item style
			function ClientShowing(source, eventArgs) {
				$.elements = $(source.get_completionList());
				$.elements.find(".ac_results_li").each(function (i) {
					if (i % 2 == 0) {
						//$(this).addClass("ac_even");
					}
					else {
						$(this).addClass("ac_odd");
					}
				});
			}

			//add loader to textbox
			function ClientPopulating(source, e) {
				$("#" + source._element.id).addClass("ac_loading");
			}

			//remove loader from textbox
			function ClientHiding(source, eventArgs) {
				$("#" + source._element.id).removeClass("ac_loading");
			}

		</script>
		<%--End--%>

		<div>

			<script type="text/javascript">

				<% Dim mopen As String = Request.QueryString("Type") %>
				<% If Not mopen Is Nothing AndAlso mopen = "pup" Then %>  

				$(document).ready(function () {

					SetPageLayout();
					if ($.browser.msie) {
						parent.IframeDiscrepancyTroubleShootStateComplete();
					}

				});

				<% End if %>

				Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(endRequestHandler);

				function endRequestHandler() {
					SetPageLayout();
				}

				function SetPageLayout() {

					<% Dim OpenAs As String = Request.QueryString("Type") %>
					<% If OpenAs IsNot Nothing AndAlso OpenAs = "pup" Then %>  

						ReSetPageLayout();
						onResize();//for Top bottom link

					<% End if %>

				}
				function ReSetPageLayout() {

					$("body,html").css({ 'background-color': 'transparent' });
					var tempMargtop = $("body #tblmain:eq(0)").outerHeight();
					var windowheight = $(window).height();

					if (tempMargtop >= windowheight) {
						$("body #tblmain:eq(0)").css({ 'margin': 'auto' });
					}
					else {
						var margintop = (windowheight / 2) - (tempMargtop / 2);
						$("body #tblmain:eq(0)").css({ 'margin': 'auto', 'margin-top': margintop + 'px' });
					}

				}
			</script>

		</div>

	</form>
</body>
</html>

