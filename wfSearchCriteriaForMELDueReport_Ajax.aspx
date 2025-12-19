<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForMELDueReport_Ajax.aspx.vb"
    Inherits="Flypal.wfSearchCriteriaForMELDueReport_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>MEL Due Report</title>
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
                                    <asp:Label runat="server" ID="lbltitle" CssClass="clsFormHeader"
                                        Text='<%# IIf(AppSettings("MELSnagNomenclature") = "True", "ADD Due Report", "MEL Due Report") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                HeaderText="Fill Up The Following Fields" ValidationGroup="a"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvAsOnDate" runat="server" CssClass="clsLabelAuto"
                                                ErrorMessage="As On Date Required" ControlToValidate="txtAsOnDate" Display="None"
                                                ValidationGroup="a"></asp:RequiredFieldValidator>
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
                                                        <span id="lblStep1" class="clsLabelHeader">Step I. Selection of As On Date</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td width="96px">
                                                        <span id="lblFromDate" class="clsLabelAuto">As On Date</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAsOnDate" runat="server" AutoPostBack="true" CssClass="clsTextBoxTagSearchDate"
                                                            Width="100px" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtAsOnDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtAsOnDate">
                                                        </cc2:CalendarExtender>
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
                                                <span id="lblStep3" class="clsLabelHeader">Step II. Selection of Aircraft</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td width="96px">
                                                <span id="lblAircraft" class="clsLabelAuto">Aircraft </span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cmbAircraft" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong" DataValueField="ID"
                                                    ClientIDMode="Static" DataTextField="RegNo">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <span id="Label5" class="clsLabelHeader">Step III. Selection of ATA Chapter</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <span id="Label3" class="clsLabel">ATA Chapter </span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cmbATAChapter" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong"
                                                    DataValueField="ID" DataTextField="ATAChapter">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <span></span>
                                                <asp:Label ID="Label2" runat="server" CssClass="clsLabelHeader" Text='<%# IIf(AppSettings("MELSnagNomenclature") = "True", "Step IV. Selection of ADD Category", "Step IV. Selection of MEL Category") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMELCategory" runat="server" class="clsLabelAuto" Text='<%# IIf(AppSettings("MELSnagNomenclature") = "True", "ADD Category", "MEL Category") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cmbMELCategory" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong"
                                                    DataValueField="ID" DataTextField="Name">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:Label ID="lblStep2" runat="server" class="clsLabelHeader" Text='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "Step V. Selection of ADD / Defect Type", "Step V. Selection of MEL / Snag Type") %>'></asp:Label>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td colspan="2">
                                                <asp:RadioButton ID="rbAll" runat="server" CssClass="clsRadioButton" GroupName="a"
                                                    Text="All" />
                                                <asp:RadioButton ID="rbMajor" runat="server" CssClass="clsRadioButton" GroupName="a"
                                                    Text="Major" />
                                                <asp:RadioButton ID="rbMinor" runat="server" CssClass="clsRadioButton" GroupName="a"
                                                    Text="Minor" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <span id="lbDefectType" class="clsLabelHeader">Step VI. Selection of Defect Type</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td colspan="2">
                                                <asp:RadioButton ID="rbAllDefectType" runat="server" Checked="True" CssClass="clsRadioButton"
                                                    GroupName="c" Text="All" />
                                                <asp:RadioButton ID="rbIsPireps" runat="server" CssClass="clsRadioButton" GroupName="c"
                                                    Text="Pireps" />
                                                <asp:RadioButton ID="rbMaintenanceDefect" runat="server" CssClass="clsRadioButton"
                                                    GroupName="c" Text="Maintenance Defect" Width="136px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblStep6" class="clsLabelHeader">Step VII. Display Report</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblSummary" class="clsLabelAuto">Your selection is as follows </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlselection1" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAircraft1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblATAChapter1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblMElCategory1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
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
                                                        <asp:Button ID="btnCurrentSearchCriteria" runat="server" CausesValidation="False"
                                                            CssClass="clsbtnH clsinfoH1" TabIndex="0" Text="Current Criteria" ToolTip="Click to Display Current Searching criterias" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnDisplay" runat="server" CssClass="clsbtnH clsinfoH1" TabIndex="0"
                                                            Text="Display" ToolTip="Click to Display Report" ValidationGroup="a" />
                                                    </td>
                                                    <%--6-Sep-2016--%>
                                                    <td>
                                                        <asp:Button ID="btnByMail" runat="server" CssClass="clsbtnH clsinfoH1" Text="Report By Mail"
                                                            ToolTip="Click to receive Report through mail" ValidationGroup="1" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH1"
                                                            TabIndex="0" Text="Close" ToolTip='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "Click to close ADD Due Report screen", "Click to close MEL Due Report screen") %>' />
                                                    </td>
                                                </tr>
                                                <!--Dummy panel to open modelpopup-->
                                                <tr style="height: 0px;">
                                                    <td style="height: 0px;" colspan="2" align="right">
                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                                            <ContentTemplate>
                                                                <asp:Button ID="hdnimgMELBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
                                                                    CausesValidation="False" Style="display: none;"></asp:Button>
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
    <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
        runat="server">
        <ProgressTemplate>
            <div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed;
                background-color: #000000; top: 0; z-index: 99999;">
            </div>
            <div style="position: fixed; top: 50%; left: 50%; margin-left: -27px; margin-top: -27px;
                z-index: 100000;">
                <div class="ext-el-mask-msg x-mask-loading">
                    <div class="clsLoad_ajax">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Loader.gif" ImageAlign="Middle"
                            Height="48px" Width="48px" />
                    </div>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <!-- Popup For Report By Mail 6-Sep-2016-->
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
                alert(e);
            }
        }
		function ParentCallBackFunctionForSendMail() {
            var Receiptwindow1 = $find("<%=mdlPopupReceipt1.ClientID %>");
            //close popup window
            Receiptwindow1.hide();
            //           release resources
            $("#IframeReceipt1").attr("src", "JavaScript:''");
        }
		function ParentCallBackFunctionToSendMail() {
            var Receiptwindow1 = $find("<%=mdlPopupReceipt1.ClientID %>");
            //close popup window
            Receiptwindow1.hide();
            //           release resources
            $("#IframeReceipt1").attr("src", "JavaScript:''");
            //call image button
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
    <!---End-->
    </form>
</body>
</html>
