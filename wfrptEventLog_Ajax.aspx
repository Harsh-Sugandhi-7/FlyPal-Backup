<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptEventLog_Ajax.aspx.vb"
    Inherits="Flypal.wfrptEventLog_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Event Log</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link id="MainStyle" rel="stylesheet" type="text/css">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script id="clientEventHandlersJS" language="javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css">
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="jquery.textchange.min.js"></script>
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
    <%--Added on 20-Sep-2016--%>
    <div>
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td colspan="2" class="clsFormHeader1Newstyle">
                                    <span id="lbltitle" class="clsFormHeader">Event Log</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                        HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                    <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                        ErrorMessage="To Date Required" ControlToValidate="txtToDate" Display="None"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvToDate1" runat="server" CssClass="clsLabelAuto"
                                        Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtToDate"
                                        ErrorMessage="To Date Required"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                        Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtFromDate"
                                        ErrorMessage="From Date Required"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
                                        Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date Required"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span id="lblStep2" class="clsLabelHeader">Step I. Selection of Date</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblDateRange" runat="server" CssClass="clsLabel">Date Range</asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upnlDateCriteria" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbDateRange" runat="server" AutoPostBack="True">
                                                            <asp:ListItem Value="(All)">(All)</asp:ListItem>
                                                            <asp:ListItem Value="Last Week">Last Week</asp:ListItem>
                                                            <asp:ListItem Value="Last Month">Last Month</asp:ListItem>
                                                            <asp:ListItem Value="Last Quarter">Last Quarter</asp:ListItem>
                                                            <asp:ListItem Value="Last Year">Last Year</asp:ListItem>
                                                            <asp:ListItem Value="Current Financial Year">Current Financial Year</asp:ListItem>
                                                            <asp:ListItem Value="Between Dates">Between Dates</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto" Visible="False">From</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtFromDate" ClientIDMode="Static"
                                                            runat="server" CausesValidation="true" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                            WatermarkCssClass="clsDateTextBox">
                                                        </cc2:TextBoxWatermarkExtender>
                                                        <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                                            ClientValidationFunction="BetweenDatesValidation" Display="None"></asp:CustomValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearchSmall" ID="txtFromTime" runat="server" AutoPostBack="True"
                                                            MaxLength="10" ToolTip="Enter Time" Width="65px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto" Visible="False">To</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtToDate" Style="margin-left: 3px;"
                                                            onchange="ValidateDateText(this,'ToDate_watermarkextender');" ClientIDMode="Static"
                                                            runat="server" CausesValidation="true"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                            WatermarkCssClass="clsDateTextBox">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearchSmall" ID="txtToTime" runat="server" AutoPostBack="True"
                                                            MaxLength="10" ToolTip="Enter Time" Width="65px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span id="Span1" class="clsLabelHeader">Step II. Enter User Name</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblUserName" runat="server" CssClass="clsLabel">User Name</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtUserName" runat="server" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span id="Span2" class="clsLabelHeader">Step III. Enter IP Address</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblIPAddress" runat="server" CssClass="clsLabel">IP Address</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtIPAddress" runat="server" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span id="Span3" class="clsLabelHeader">Step IV. Enter Machine Name</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblMachineName" runat="server" CssClass="clsLabel">Machine Name</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtMachineName" runat="server" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                      <asp:Label ID="lblStep6" runat="server" CssClass="clsLabelHeader">Step V. Select Login Entry / Activity/ For BTPL Admin</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkLoginEntriesOnly" runat="server" CssClass="clsCheckBox" AutoPostBack="True"
                                        Text="Show login entries only"></asp:CheckBox>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkByBTPLAdminUser" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
                                        Text="Show with BTPL Admin" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span id="Span5" class="clsLabelHeader">Step VI. Select Action </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="Span6" class="clsLabel">Action </span>
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAction" runat="server" DataValueField="ID"
                                        DataTextField="Name">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span id="Span7" class="clsLabelHeader">Step VII. Select Module Name </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="Span8" class="clsLabel">Module Name </span>
                                </td>
                                <td>
                                    <asp:TextBox CssClass="clsTextBoxSearch_Ajax" ID="txtModuleName" runat="server" ToolTip="Select Module Name"
                                        Width="520px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblStep8" runat="server" CssClass="clsLabelHeader">Step VIII. Display Report</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows :</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="upnlDisplaySearchCriteria" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblAction1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblUserName1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblIPAddress1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblMachineName1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblErrorType1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblModuleName1" runat="server" CssClass="clsLabelAuto"></asp:Label>
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
                                            <asp:Panel ID="pnlButton" CssClass="clspanel1" runat="server">
                                                <table cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" 
                                                                Text="Current Criteria" ToolTip="Click to display Current Searching criterias">
                                                            </asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" TabIndex="0" runat="server" Text="Display"
                                                                ToolTip="Click to display report"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnByMail" runat="server" Text="Report By Mail"
                                                                ToolTip="Click to receive Report through mail" ValidationGroup="1" />
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" TabIndex="0" runat="server" Text="Close"
                                                                CausesValidation="False" ToolTip="Click to Close"></asp:Button>
                                                        </td>
                                                    </tr>
                                                    <!--Dummy panel to open modelpopup 20-Sep-2016-->
                                                    <tr style="height: 0px;">
                                                        <td style="height: 0px;" colspan="2" align="right" colspan="4">
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
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" ClientIDMode="Static"
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
    </div>
    <!-- Popup For Report By Mail 20-Sep-2016-->
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
    </script>
    <!---End-->
    <%--Date Validations--%>
    <script type="text/javascript">
        //From Date -To Date validation
        function BetweenDatesValidation(source, args) {
            var selectedDateIndex = $get("cmbDateRange").selectedIndex;
            if (selectedDateIndex == 6) {
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
    </form>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtModuleName.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=ModuleName', {
                width: 520,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });
        });
    </script>
</body>
</html>
