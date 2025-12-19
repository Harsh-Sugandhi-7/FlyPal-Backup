<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDeferredDueDiscrepancy.aspx.vb"
	Inherits="Flypal.DeferredDueDiscrepancyReport" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Deferred Due Discrepancy Report</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
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
            <table id="tblmain" class="clstablelistout">
                <tr>
                    <td>
                        <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                            <table id="tblInner" class="clstablelistin">
                                <tr>
                                    <td class="clsFormHeader1Newstyle">
                                        <asp:Label runat="server" ID="lbltitle"
											CssClass="clsFormHeader"
											Text="Deferred Due Report" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="Validationsummary2" 
													runat="server" CssClass="clsValidationSummary"
                                                    HeaderText="Fill Up The Following Fields" 
													ValidationGroup="a" />
                                                <asp:RequiredFieldValidator ID="rfvAsOnDate" 
													runat="server" CssClass="clsLabelAuto"
                                                    ErrorMessage="As On Date Required" 
													ControlToValidate="txtAsOnDate" Display="None"
                                                    ValidationGroup="a" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="upnlDateRange" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td colspan="3">
                                                            <asp:Label runat="server" ID="lblStep1" 
																CssClass="clsLabelHeader" 
																Text="Step I. Selection of As On Date" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td width="96px">
                                                            <asp:Label runat="server" ID="lblFromDate"
																CssClass="clsLabelAuto" Text="As On Date" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtAsOnDate" runat="server" 
																AutoPostBack="true" CssClass="clsTextBoxTagSearchDate"
                                                                Width="100px" onchange="ValidateDateText(this,'FromDate_watermarkextender');" />
                                                            <cc2:CalendarExtender ID="txtAsOnDate_CalendarExtender" 
																runat="server" CssClass="cal_Theme1"
                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" 
																TargetControlID="txtAsOnDate" />
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
                                                <td colspan="3">
                                                     <asp:Label runat="server" ID="lblStep2"
														 CssClass="clsLabelHeader"
														 Text="Step II. Selection of Aircraft" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td width="96px">
													<asp:Label runat="server" ID="lblAircraft" 
														CssClass="clsLabelAuto" Text="Aircraft" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbAircraft" runat="server" 
														CssClass="clsTextBoxTagSearchComboNewstyleLong" 
														DataValueField="ID"
                                                        ClientIDMode="Static" DataTextField="RegNo">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label runat="server" ID="lblStep3"
														CssClass="clsLabelHeader"
														Text="Step III. Selection of ATA Chapter" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblATAChapter"
														CssClass="clsLabel" Text="ATA Chapter" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbATAChapter" runat="server"
														CssClass="clsTextBoxTagSearchComboNewstyleLong"
                                                        DataValueField="ID" DataTextField="ATAChapter" />
                                                </td>
                                            </tr>
                                            <asp:PlaceHolder runat="server" ID="phDiscrepancyCategoryAndType">
												<tr>
													<td colspan="3">
														<asp:Label ID="lblStep4" runat="server"
															CssClass="clsLabelHeader"
															Text="Step IV. Selection of Discrepancy Category" />
														&nbsp;
													</td>
												</tr>
												<tr>
													<td>&nbsp;
													</td>
													<td colspan="2">
														<asp:RadioButton ID="rbAll" runat="server" CssClass="clsRadioButton" GroupName="a"
															Text="All" />
														<asp:RadioButton ID="rbMajor" runat="server" CssClass="clsRadioButton" GroupName="a"
															Text="Major" />
														<asp:RadioButton ID="rbMinor" runat="server" CssClass="clsRadioButton" GroupName="a"
															Text="Minor" />
														<asp:RadioButton ID="rbIncident" runat="server" CssClass="clsRadioButton" GroupName="a"
															Text="Incident" />
													</td>
												</tr>
												<tr>
													<td colspan="3">
														<asp:Label runat="server" ID="lblStep5" 
															CssClass="clsLabelHeader" 
															Text="Step V. Selection of Discrepancy Type" />
													</td>
												</tr>
												<tr>
													<td>&nbsp;
													</td>
													<td colspan="2">
														<table width="100%">
															<tr>
																<td>
																	<asp:RadioButton ID="rbAllDefectType" runat="server"
																		Checked="True" CssClass="clsRadioButton"
																		GroupName="c" Text="All" />
																	<asp:RadioButton ID="rbIsPireps" runat="server"
																		CssClass="clsRadioButton" GroupName="c"
																		Text="Pireps" />
																	<asp:RadioButton ID="rbMaintenanceDefect" runat="server"
																		CssClass="clsRadioButton"
																		GroupName="c" Text="Maintenance Defect" />
																</td>
															</tr>
														</table>
													</td>
												</tr>
											</asp:PlaceHolder>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblStep6" 
											CssClass="clsLabelHeader" 
											Text="Step VI. Display Report" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="upnlselection" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
													<tr>
														<td>
															<asp:Label runat="server" ID="lblSummary"
																CssClass="clsLabelAuto"
																Text="Your selection are as follows" />
														</td>
													</tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="lblDateRangeSearchCriteria" runat="server"
																CssClass="clsLabelAuto" Visible="False" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblAircraftSearchCriteria" runat="server"
																CssClass="clsLabelAuto" Visible="False" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblATAChapterSearchCriteria" runat="server"
																CssClass="clsLabelAuto" Visible="False" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnCurrentSearchCriteria" 
																runat="server" CausesValidation="False"
                                                                CssClass="clsbtnH clsinfoH1" TabIndex="0" 
																Text="Current Criteria" 
																ToolTip="Display Current Searching criterias" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnDisplay" runat="server" 
																CssClass="clsbtnH clsinfoH1" TabIndex="0"
                                                                Text="Display" 
																ToolTip="Display Report" 
																ValidationGroup="a" />
                                                        </td>
                                                        <%--6-Sep-2016--%>
                                                        <td>
                                                            <asp:Button ID="btnByMail" runat="server" 
																CssClass="clsbtnH clsinfoH1" 
																Text="Report By Mail"
                                                                ToolTip="Click to receive Report through mail" 
																ValidationGroup="1" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnClose" runat="server"
																CausesValidation="False" 
																CssClass="clsbtnH clsinfoH1"
                                                                TabIndex="0" Text="Close" 
																ToolTip="Click to close Deferred Due Report screen" />
                                                        </td>
                                                    </tr>
                                                    <!--Dummy panel to open modelpopup-->
                                                    <tr style="height: 0px;">
                                                        <td style="height: 0px;" colspan="2" align="right">
                                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                                                <ContentTemplate>
                                                                    <asp:Button ID="hdnimgMELBtnSendMail"
																		ClientIDMode="Static" runat="server" Text="----"
                                                                        CausesValidation="False" 
																		Style="display: none;" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <!--End -->
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

        <!-- Popup For Report By Mail 6-Sep-2016-->
		<div>

			<div style="display: none">
				<asp:Button runat="server" ID="btnDummyReceipt1" Text="Receipt1" ClientIDMode="Static" />
			</div>
			<asp:Panel runat="server" ID="pnlReceipt1" ClientIDMode="Static" HorizontalAlign="Center"
				Style="height: 100%; width: 100%;">
				<iframe id="IframeReceipt1" frameborder="0" height="100%" width="100%" src="JavaScript:''"
					scrolling="auto" allowtransparency="true"></iframe>
			</asp:Panel>
			<cc2:ModalPopupExtender ID="mdlPopupReceipt1" runat="server" TargetControlID="btnDummyReceipt1"
				PopupControlID="pnlReceipt1" BackgroundCssClass="clsModalPopupBG">
			</cc2:ModalPopupExtender>
			<script type="text/javascript">

				function OpenByMaiWindow() {

					try {

						$("#IframeReceipt1").attr("src", "wfByMail_Ajax.aspx?Type=pup");
						$("#btnDummyReceipt1").click();

						return false;

					} catch (e) {
						console.error("Exception Occured in function OpenByMaiWindow(). Following is the Exception : " + e);
					}

				}

				function ParentCallBackFunctionForSendMail() {

					var Receiptwindow1 = $find("<%=mdlPopupReceipt1.ClientID %>");
					Receiptwindow1.hide();
					$("#IframeReceipt1").attr("src", "JavaScript:''");

				}
				function ParentCallBackFunctionToSendMail() {

					var Receiptwindow1 = $find("<%=mdlPopupReceipt1.ClientID %>");
					Receiptwindow1.hide();
					$("#IframeReceipt1").attr("src", "JavaScript:''");
					$("#hdnimgMELBtnSendMail").click();

				}

				/*Added by Harsh for Date Validation on 23rd Feb 2024*/
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

		</div>
		<!---End-->

    </form>
</body>
</html>

