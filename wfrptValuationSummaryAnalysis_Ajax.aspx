<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptValuationSummaryAnalysis_Ajax.aspx.vb"
    Inherits="Flypal.wfrptValuationSummaryAnalysis_Ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Valuation Summary Analysis</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css">
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script id="clientEventHandlersJS" type="text/javascript">
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" topmargin="5" rightmargin="0" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table class="clstablelistout" id="tblmain" border="0">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                    <table id="tblInner" class="clstablelistin" border="0">
                        <tr>
                            <td colspan="2">
                                <span id="lbltitle" class="clstitle1">Valuation Summary Analysis</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                    CssClass="clsValidationSummary" ValidationGroup="1"></asp:ValidationSummary>
                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                    Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtFromDate"
                                    ErrorMessage="From Date Required." ValidationGroup="1"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
                                    Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date Required."
                                    ValidationGroup="1"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                    ErrorMessage="To Date Required." ControlToValidate="txtToDate" Display="None"
                                    ValidationGroup="1"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvToDate1" runat="server" CssClass="clsLabelAuto"
                                    Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtToDate"
                                    ErrorMessage="To Date Required." ValidationGroup="1"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                    ClientValidationFunction="BetweenDatesValidation" Display="None" ValidationGroup="1"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span id="lblStep1" class="clsLabelHeader">Step I. Selection of Month and Year</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel runat="server" ID="upnlMonth" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <span id="Span2" class="clsLabelAuto">From</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbFromMonth" runat="server" AutoPostBack="true" CssClass="clsComboBox1_Ajax">
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="cmbFromYear" runat="server" AutoPostBack="true" CssClass="clsComboBox1_Ajax"
                                                        Width="112px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <span id="Span3" class="clsLabelAuto">To</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtToMonth" runat="server" ClientIDMode="Static" CssClass="clsComboBox1_Ajax"
                                                        Enabled="false" Height="14px"></asp:TextBox>
                                                    <asp:TextBox ID="txtToYear" runat="server" ClientIDMode="Static" CssClass="clsComboBox1_Ajax"
                                                        Enabled="false" Height="14px" Width="112px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFromDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxDate_Ajax"
                                                        Enabled="false" onchange="ValidateDateText(this,'FromDate_watermarkextender');"
                                                        TabIndex="2"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender ID="FromDate_watermarkextender" runat="server" ClientIDMode="Static"
                                                        TargetControlID="txtFromDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                    </cc2:TextBoxWatermarkExtender>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtToDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxDate_Ajax"
                                                        Enabled="false" onchange="ValidateDateText(this,'ToDate_watermarkextender');"
                                                        Style="margin-left: 3px;" TabIndex="3"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender ID="ToDate_watermarkextender" runat="server" ClientIDMode="Static"
                                                        TargetControlID="txtToDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                    </cc2:TextBoxWatermarkExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span id="Span1" class="clsLabelHeader">Step II. Selection of Store</span>
                            </td>
                        </tr>
                        <tr>
                        <td></td>
                            <td>
                                <asp:Label ID="lblStoreCount" ForeColor="DarkBlue" runat="server" Font-Size="XX-Small"
                                    Font-Bold="true" class="clsLabelAuto"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblStore" class="clsLabel">Store</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbStore" runat="server" CssClass="clsComboBox3_Ajax" DataTextField="LocationStore"
                                    DataValueField="ID" TabIndex="6">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span id="Label3" class="clsLabelHeader">Step III. Selection of Base,Landing,Commercial
                                    Value</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="Label4" class="clsLabelAuto" runat="server">Value</span>
                            </td>
                            <td>
                                <asp:RadioButton ID="rbBase" runat="server" CssClass="clsRadioButton" GroupName="b"
                                    TabIndex="13" Text="Base" />
                                <asp:RadioButton ID="rbLanding" runat="server" CssClass="clsRadioButton" GroupName="b"
                                    TabIndex="14" Text="Landing" />
                                <asp:RadioButton ID="rbCommercial" runat="server" CssClass="clsRadioButton" GroupName="b"
                                    TabIndex="15" Text="Commercial" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span id="Label5" class="clsLabelHeader">Step IV. Selection of Open/Authorized Transaction</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:RadioButton ID="optAll" runat="server" Checked="True" CssClass="clsLabel" GroupName="a"
                                    TabIndex="16" Text="All" />
                                <asp:RadioButton ID="optOnlyAuthorized" runat="server" CssClass="clsLabel" GroupName="a"
                                    TabIndex="17" Text="Only Authorized" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span id="lblStep7" class="clsLabelHeader">Step V. Display Report</span>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span id="lblSummary" class="clsLabelAuto">Your selection is as follows :</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlCurrentCriteria" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblStoreName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:UpdatePanel ID="upnlBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnCurrentSearchCriteria" runat="server" CssClass="clsButtonLong_Ajax"
                                                        TabIndex="23" Text="Current Criteria" ToolTip="Click to Display Current Searching criterias" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnDisplay" runat="server" CssClass="clsButton_Ajax" TabIndex="25"
                                                        Text="Display" ToolTip="Click to Display Report" ValidationGroup="1" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnByMail" runat="server" CssClass="clsButton_Ajax" TabIndex="25"
                                                        Text="Report By Mail" ToolTip="Click to report by mail" ValidationGroup="1" Width="96px" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                        TabIndex="26" Text="Close" ToolTip="Click to close Valuation Summary Analysis screen" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <!--Dummy panel to open modelpopup-->
                        <tr style="height: 0px;">
                            <td style="height: 0px;" colspan="2" align="right">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnimgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <!--End -->
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
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
    <%--Date Validations--%>
    <script type="text/javascript">

        //From Date -To Date validation
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

        //Date validations
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
    <!-- Popup For Valuation Summary -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyValuation1" Text="Valuation1" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlValuation1" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeValuation1" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            scrolling="auto" allowtransparency="true"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupValuation1" runat="server" TargetControlID="btnDummyValuation1"
        PopupControlID="pnlValuation1" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function OpenByMaiWindow() {
            try {
                $("#IframeValuation1").attr("src", "wfByMail_Ajax.aspx?Type=pup");
                $("#btnDummyValuation1").click();

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForSendMail() {
            var Valuationwindow1 = $find("<%=mdlPopupValuation1.ClientID %>");
            //close popup window
            Valuationwindow1.hide();
            //           release resources
            $("#IframeValuation1").attr("src", "JavaScript:''");
        }
        function ParentCallBackFunctionToSendMail() {
            var Valuationwindow1 = $find("<%=mdlPopupValuation1.ClientID %>");
            //close popup window
            Valuationwindow1.hide();
            //           release resources
            $("#IframeValuation1").attr("src", "JavaScript:''");
            //call image button
            $("#hdnimgBtnSendMail").click();
        }
    </script>
    <!---End-->
    </form>
</body>
</html>
