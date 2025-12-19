<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptConsiderAsAsset_Ajax.aspx.vb"
    Inherits="Flypal.wfrptConsiderAsAsset_Ajax" %>

<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Part History</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');

        }
    </script>
    <script id="clientEventHandlersJS" type="text/javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="jquery.textchange.min.js"></script>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
    <form id="frmrptPartHitory" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table id="tblmain" class="clstablelistout">
            <tr>

               <td>
                                <table width="100%">
                                    <tr>
                                        <td  class="clsFormHeader1">
                                             <span id="lbltitle" class="clsFormHeader">Asset Items</span>
                                        </td>
                                    </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                    <ContentTemplate>
                                       
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" HeaderText="Fill Up The Following Fields"
                                            CssClass="clsValidationSummary" ValidationGroup="1"></asp:ValidationSummary>
                                        <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                            ErrorMessage="To Date Required" ControlToValidate="txtToDate" Display="None"
                                            ValidationGroup="1"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                            ClientValidationFunction="BetweenDatesValidation" Display="None" ValidationGroup="1"></asp:CustomValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td align="right">
                                <%--<asp:UpdatePanel runat="server" ID="upnlActionBtns" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnCurrentSearchCriteria" runat="server" CssClass="clsbtnH clsinfoH"
                                                        CausesValidation="false" TabIndex="0" Text="Current Criteria" ToolTip="Click to display current searching criterias" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnExport" TabIndex="0" runat="server" CssClass="clsbtnH clsinfoH" Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                        Text="Export to Excel" ToolTip="Click to Export report" Width="140px"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnDisplay" runat="server" CssClass="clsbtnH clsinfoH" TabIndex="0"
                                                        ValidationGroup="1" Text="Display" ToolTip="Click to display report" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnByMail" runat="server" CssClass="clsbtnH clsinfoH" TabIndex="25"
                                                        Text="Report By Mail" ToolTip="Click to report by mail" ValidationGroup="1" Width="140px" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                        TabIndex="0" Text="Close" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>--%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel runat="server" ID="upnlMonth" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td colspan="4">
                                        <span id="lblStep1" class="clsLabelHeader">Step I. Selection of Date Range</span>
                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        <span id="Span2" class="clsLabelAuto">From</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagDateSearch"
                                            onchange="ValidateDateText(this,'FromDate_watermarkextender');" TabIndex="2"></asp:TextBox>
                                        <asp:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></asp:CalendarExtender>
                                        <asp:TextBoxWatermarkExtender ID="FromDate_watermarkextender" runat="server" ClientIDMode="Static"
                                            TargetControlID="txtFromDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>"></asp:TextBoxWatermarkExtender>
                                    </td>
                                    <td>
                                        <span id="Span3" class="clsLabelAuto">To</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtToDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagDateSearch"
                                            onchange="ValidateDateText(this,'ToDate_watermarkextender');" Style="margin-left: 3px;"
                                            TabIndex="3"></asp:TextBox>
                                        <asp:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></asp:CalendarExtender>
                                        <asp:TextBoxWatermarkExtender ID="ToDate_watermarkextender" runat="server" ClientIDMode="Static"
                                            TargetControlID="txtToDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>"></asp:TextBoxWatermarkExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <span id="lblIsValued" class="clsLabelHeader">Step II. Selection of IsValued Store</span>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td colspan="3">
                                        <asp:CheckBox ID="chkIsValued" runat="server" Checked="True" CssClass="clsCheckBox"
                                            Text="Include Valued Stores Only" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <span id="Label2" class="clsLabelHeader">Step III. Selection of Part Number/Description</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblSearch" class="clsLabelAuto">Search</span>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBoxTagSearch" Width="275px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <span id="lblStep3" class="clsLabelHeader">Step IV. Display Report</span>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <span id="lblSummary" class="clsLabelAuto">Your selection is as follows </span>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel runat="server" ID="upnlSerachCriteria" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblDateRange" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblDesc" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                             <td align="right">
                            <asp:UpdatePanel runat="server" ID="upnlActionBtns" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnCurrentSearchCriteria" runat="server" CssClass="clsbtnH"
                                                    CausesValidation="false" TabIndex="0" Text="Current Criteria" ToolTip="Click to display current searching criterias" />
                                            </td>
                                            <td>
                                             <asp:Button ID="btnExport" TabIndex="0" runat="server" CssClass="clsbtnH"  Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                     ValidationGroup="1" Text="Export to Excel" ToolTip="Click to Export report" Width="140px"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnDisplay" runat="server" CssClass="clsbtnH" TabIndex="0"
                                                    ValidationGroup="1" Text="Display" ToolTip="Click to display report" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btnByMail" runat="server" CssClass="clsbtnH" TabIndex="25"
                                                    Text="Report By Mail" ToolTip="Click to report by mail" ValidationGroup="1" Width="140px" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH"
                                                    TabIndex="0" Text="Close" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td> 
                        </tr>
                        <!--Dummy panel to open modelpopup-->
                        <tr style="height: 0px;">
                            <td style="height: 0px;" align="right">
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
                </td>
            </tr>
        </table>
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" DynamicLayout="false" runat="server">
            <ProgressTemplate>
                <div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed; background-color: #000000; top: 0; z-index: 99999;">
                </div>
                <div style="position: fixed; top: 50%; left: 50%; margin-left: -27px; margin-top: -27px; z-index: 100000;">
                    <div class="ext-el-mask-msg x-mask-loading">
                        <div class="clsLoad_ajax">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Loader.gif" ImageAlign="Middle"
                                Height="48px" Width="48px" />
                        </div>
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <!-- Popup For by mail -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyByMail" Text="ByMail" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlByMail" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeByMail" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                scrolling="auto" allowtransparency="true"></iframe>
        </asp:Panel>
        <asp:ModalPopupExtender ID="mdlPopupByMail" runat="server" TargetControlID="btnDummyByMail"
            PopupControlID="pnlByMail" BackgroundCssClass="clsModalPopupBG">
        </asp:ModalPopupExtender>
        <script type="text/javascript">
            function OpenByMaiWindow() {
                try {
                    $("#IframeByMail").attr("src", "wfByMail_Ajax.aspx?Type=pup");
                    $("#btnDummyByMail").click();

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForSendMail() {
                var Valuationwindow1 = $find("<%=mdlPopupByMail.ClientID %>");
                //close popup window
                Valuationwindow1.hide();
                //           release resources
                $("#IframeByMail").attr("src", "JavaScript:''");
            }
            function ParentCallBackFunctionToSendMail() {
                var Valuationwindow1 = $find("<%=mdlPopupByMail.ClientID %>");
                //close popup window
                Valuationwindow1.hide();
                //           release resources
                $("#IframeByMail").attr("src", "JavaScript:''");
                //call image button
                $("#hdnimgBtnSendMail").click();
            }
        </script>
        <!---End-->
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
    </form>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtSearch.ClientID %>").autocomplete('wfAutoItemList.aspx?', {
                width: 275,
                autoFill: false,
                matchContains: true,
                delay: 0
            });
        });
    </script>
</body>
</html>
