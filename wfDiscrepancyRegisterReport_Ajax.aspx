<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfDiscrepancyRegisterReport_Ajax.aspx.vb"
	Inherits="Flypal.DiscrepancyRegister" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagName="MSGBox" Src="MSGBox.ascx" TagPrefix="msgBox" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Register Report</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link href="Styles.css" id="MainStyle" type="text/css" rel="stylesheet" />

    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>

	<script type="text/javascript" src="modules/jquery/jquery-2.2.4.min.js"></script>

</head>
<body>
    <form id="discrepancyRegisterReportForm" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager" runat="server"
            EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <msgBox:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="mainDiv">
            <table id="mainTbl" class="clstablelistout">
                <tr>
                    <td>
                        <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                            <table id="contentTbl" class="clstablelistin">
                                <tr>
                                    <td colspan="2" class="clsFormHeader1Newstyle">
                                        <asp:Label CssClass="clsFormHeader lblHeader" 
											runat="server" ID="lblHeader" >
                                        </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="upnlValidationErrors" UpdateMode="Conditional" runat="server">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="vsDiscrepancyRegister" 
													runat="server" ValidationGroup="DiscrepancyRegister"
                                                    CssClass="clsValidationSummary"
													HeaderText="Please resolve following Issue(s)." />
                                                <asp:RequiredFieldValidator ID="rfvFromDate" 
													runat="server" CssClass="clsLabelAuto"
                                                    ErrorMessage="From Date is Required." 
													ControlToValidate="txtFromDate" Display="None"
                                                    ValidationGroup="DiscrepancyRegister" />
                                                <asp:CustomValidator ID="cvFromDate" runat="server" 
													CssClass="clsLabelAuto" Display="None"
                                                    ClientValidationFunction="BetweenDatesValidation" 
													ValidationGroup="DiscrepancyRegister"
                                                    ErrorMessage="From Date should not be greater than To Date." />
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
                                                        <td colspan="5">
                                                            <asp:Label runat="server" ID="lblSelectionOfDateRange" 
																CssClass="clsLabelHeader"
                                                                Text="Step I. Selection of Dates" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblStarFromDate"
                                                                CssClass="clsLabelStar ReuiredICON" Text="*" />
                                                        </td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblFromDate" CssClass="clsLabelAuto"
                                                                Text="From" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtFromDate" 
																CssClass="clsTextBoxTagSearchDate" Width="100px"
                                                                onchange="ValidateDateText(this,'FromDate_watermarkextender');" />
                                                            <cc2:CalendarExtender ID="txtFromDate_CalendarExtender"
																runat="server" CssClass="cal_Theme1"
                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>"
																TargetControlID="txtFromDate" />
                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate"
																ID="FromDate_watermarkextender"
                                                                ClientIDMode="Static" runat="server"
																WatermarkText="<%$AppSettings:DateFormat%>"
                                                                WatermarkCssClass="clsDateTextBox" />
                                                        </td>
                                                        <td></td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblToDate" CssClass="clsLabelAuto" Text="To" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtToDate" 
																CssClass="clsTextBoxTagSearchDate" Width="100px"
                                                                onchange="ValidateDateText(this,'ToDate_WatermarkExtender');" />
                                                            <cc2:CalendarExtender ID="ToDate_CalendarExtender"
																runat="server" CssClass="cal_Theme1"
                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" 
																TargetControlID="txtToDate" />
                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate"
																ID="ToDate_WatermarkExtender"
                                                                ClientIDMode="Static" runat="server"
																WatermarkText="<%$AppSettings:DateFormat%>"
                                                                WatermarkCssClass="clsDateTextBox" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td colspan="5">
                                                    <asp:Label runat="server" ID="lblSelectionOfAircraft"
                                                        CssClass="clsLabelHeader" Text="Step II. Selection of Aircraft" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td class="lblControls">
                                                    <asp:Label ID="lblRegisterAircraft" runat="server" CssClass="clsLabelAuto"
                                                        Text="Aircraft" />
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel runat="server" ID="upnlAircraft" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="ddlAircraft" runat="server"
                                                                CssClass="clsTextBoxTagSearchComboNewstyle" 
																DataValueField="ID"
                                                                ClientIDMode="Static" DataTextField="RegNo" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td colspan="5">
                                                    <asp:Label runat="server" ID="lblSelectionOfATA"
                                                        CssClass="clsLabelHeader" 
														Text="Step III. Selection of ATA Chapter" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td class="lblControls">
                                                    <asp:Label ID="lblATAChapter" runat="server" 
														CssClass="clsLabelAuto" Text="ATA Chapter" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlATAChapter" runat="server"
                                                        CssClass="clsTextBoxTagSearchComboNewstyle" 
														DataValueField="ID"
                                                        DataTextField="ATAChapter" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
								<asp:PlaceHolder runat="server" ID="phMelOrDeviationCategory">
									<tr>
										<td>
											<table width="100%">
												<tr>
													<td colspan="5">
														<asp:Label runat="server" ID="lblSelectionOfCategory"
															CssClass="clsLabelHeader"
															Text="Step IV. Selection of Category" />
													</td>
												</tr>
												<tr>
													<td></td>
													<td class="lblControls">
														<asp:Label ID="lblCategory" runat="server"
															CssClass="clsLabelAuto" Text="MEL / Deviation" />
													</td>
													<td align="left">
														<asp:DropDownList ID="ddlMELSnag" runat="server"
															CssClass="clsTextBoxTagSearchComboNewstyle">
															<asp:ListItem Value="0">(ALL)</asp:ListItem>
															<asp:ListItem Value="1">MEL</asp:ListItem>
															<asp:ListItem Value="2">Deviation</asp:ListItem>
														</asp:DropDownList>
													</td>
												</tr>
											</table>
										</td>
									</tr>
								</asp:PlaceHolder>
								<tr>
									<td>
										<table width="100%">
											<tr>
												<td colspan="5">
													<asp:Label runat="server"
														ID="lblSelectionOfStatus"
														CssClass="clsLabelHeader" />
												</td>
											</tr>
											<tr>
												<td></td>
												<td class="lblControls">
													<asp:Label ID="lblStatus" runat="server"
														CssClass="clsLabelAuto" Text="Status" />
												</td>
												<td>
													<asp:DropDownList ID="ddlStatus" runat="server"
														CssClass="clsTextBoxTagSearchComboNewstyle">
														<asp:ListItem Value="0">(ALL)</asp:ListItem>
														<asp:ListItem Value="2">Open</asp:ListItem>
														<asp:ListItem Value="3">Deferred</asp:ListItem>
														<asp:ListItem Value="1">Closed</asp:ListItem>
													</asp:DropDownList>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<asp:PlaceHolder runat="server" ID="phDiscrepancyCategory">
									<tr>
										<td>
											<table width="100%">
												<tr>
													<td>
														<asp:Label ID="lblSelectionOfDiscrepancyCategory"
															CssClass="clsLabelHeader" runat="server"
															Text="Step VI. Selection of Discrepancy Category" />
													</td>
												</tr>
												<tr>
													<td>
														<asp:RadioButton ID="rbAll" runat="server"
															CssClass="clsRadioButton" GroupName="a"
															Text="All" Checked="True" />
														<asp:RadioButton ID="rbMajor" runat="server"
															CssClass="clsRadioButton" GroupName="a"
															Text="Major" />
														<asp:RadioButton ID="rbMinor" runat="server"
															CssClass="clsRadioButton" GroupName="a"
															Text="Minor" />
														<asp:RadioButton ID="rbIncident" runat="server"
															CssClass="clsRadioButton" GroupName="a"
															Text="Incident" />
													</td>
												</tr>
											</table>
										</td>
									</tr>
								</asp:PlaceHolder>
								<tr>
									<td>
										<table width="100%">
											<tr>
												<td colspan="5">
													<asp:Label runat="server" ID="lblSelectionOfDiscrepancy"
														CssClass="clsLabelHeader" />
												</td>
											</tr>
											<tr>
												<td></td>
												<td class="lblControls">
													<asp:Label ID="lblDiscrepancy" runat="server"
														CssClass="clsLabelAuto" />
												</td>
												<td>
													<asp:TextBox ID="txtDiscrepancy"
														runat="server" Width="278px"
														CssClass="clsTextBoxTagSearchMultilineNewstyle"
														ToolTip="Enter Discrepancy"
														TextMode="MultiLine" />
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<asp:PlaceHolder runat="server" ID="phDefectType">
									<tr>
										<td>
											<table width="100%">
												<tr>
													<td colspan="5">
														<asp:Label runat="server" ID="lblSelectionOfDefectType"
															CssClass="clsLabelHeader"
															Text="Step VIII. Selection of Defect Type" />
													</td>
												</tr>
												<tr>
													<td>
														<table width="100%">
															<tr>
																<td>
																	<asp:RadioButton ID="rbAllDefectType" runat="server"
																		Checked="True" CssClass="clsRadioButton"
																		GroupName="c" Text="All" />
																</td>
																<td>
																	<asp:RadioButton ID="rbIsPireps" runat="server"
																		CssClass="clsRadioButton" GroupName="c"
																		Text="Pireps" />
																</td>
																<td>
																	<asp:RadioButton ID="rbMaintenanceDefect" runat="server"
																		CssClass="clsRadioButton" GroupName="c"
																		Text="Maintenance Defect" />
																</td>
															</tr>
														</table>
													</td>
												</tr>
											</table>
										</td>
									</tr>
								</asp:PlaceHolder>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblDisplayReport" runat="server"
                                            CssClass="clsLabelHeader" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblSummary" runat="server"
                                                                CssClass="clsLabelAuto" Visible="false"
                                                                Text="Your Selections are as follows."
                                                                Font-Bold="true" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblSearchCriteriaFromDate" runat="server"
                                                                CssClass="clsLabelAuto" Visible="false" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblSearchCriteriaToDate" runat="server"
                                                                CssClass="clsLabelAuto" Visible="false" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblSearchCriteriaAircraft" runat="server"
                                                                CssClass="clsLabelAuto" Visible="false" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblSearchCriteriaATAChapter" runat="server"
                                                                CssClass="clsLabelAuto" Visible="false" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblSearchCriteriaMELSnag" runat="server"
                                                                CssClass="clsLabelAuto" Visible="false" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblSearchCriteriaStatus" runat="server"
                                                                CssClass="clsLabelAuto" Visible="false" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblSearchCriteriaDiscrepancy" runat="server"
                                                                CssClass="clsLabelAuto" Visible="false" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblSearchCriteriaDiscreapancyCategory" runat="server"
                                                                CssClass="clsLabelAuto" Visible="false" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblSearchCriteriaDefectType" runat="server"
                                                                CssClass="clsLabelAuto" Visible="false" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2">
                                        <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnSearchCriteria" runat="server"
                                                                CssClass="clsbtnH clsinfoH1" Text="Current Criteria"
                                                                ToolTip="Display Current Searching criterias" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnExportToExcel" runat="server" CssClass="clsbtnH clsinfoH1"
                                                                ValidationGroup="DiscrepancyRegister" CausesValidation="true"
                                                                Text="Export To Excel" ToolTip="Open Report in Excel Fromat"
                                                                Visible="<%$AppSettings:ShowExportToExcelButton%>" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnDisplay" runat="server" CssClass="clsbtnH clsinfoH1"
                                                                ValidationGroup="DiscrepancyRegister" CausesValidation="true"
                                                                Text="Display" ToolTip="Display the Report" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnClose" runat="server" CausesValidation="False"
                                                                CssClass="clsbtnH clsinfoH1" Text="Close"
                                                                ToolTip="Close Discrepancy Register screen" />
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
        </div>

        <asp:HiddenField runat="server" ClientIDMode="Static" ID="EmployeeID" />

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

    </form>

    <script id="clientEventHandlersJS" type="text/javascript">

        //#region Open Report Page

        function openTranDetail() {
            str = "wfReports.aspx";
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }

        //#endregion

        //#region Date Validations

        //Date Range Validation
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

        //Date Validation
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

        //#endregion

        //#region AutoComplete DropDown

        //#region Set ID

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
            if (source._id == "LicenceNo_AutoComplete") {
                textbox = document.getElementById('EmployeeID');
            }
            textbox.value = value;
        }

        //#endregion

        //#region Text Change Event

        // if id found, set id to hiddenfield and return ,else clear the hidden field value..
        function SetEmployeeIdonChange(source, extenderid) {
            var popup = $find(extenderid);
            var complist = popup.get_completionList();
            var text = $(source).val().toLowerCase();
            for (var i = 0; i < complist.childNodes.length; i++) {
                var texttocompare = complist.childNodes[i].innerText.toLowerCase();
                if (text == texttocompare) {
                    var val = complist.childNodes[i]._value;

                    if (extenderid == "LicenceNo_AutoComplete") {
                        textbox = document.getElementById('EmployeeID');
                    }
                    textbox.value = val;
                    return;
                }
            }

            if (extenderid == "LicenceNo_AutoComplete") {
                document.getElementById('EmployeeID').value = '';
            }
        }

        //#endregion

        //#region CSS for Dropdown		

        //bold input value in list...
        function ClientPopulated(source, eventArgs) {
            $("#" + source._element.id).removeClass("ac_loading");
        }

        //Alternate item style
        function ClientShowing(source, eventArgs) {
            $.elements = $(source.get_completionList());
            $.elements.find(".ac_results_li").each(function (i) {
                if (i % 2 == 0) { }
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
            //#endregion

        //#endregion

    </script>
</body>
</html>
