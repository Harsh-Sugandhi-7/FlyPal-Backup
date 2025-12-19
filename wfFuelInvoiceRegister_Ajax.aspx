<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfFuelInvoiceRegister_Ajax.aspx.vb"
    Inherits="Flypal.wfFuelInvoiceRegister_Ajax" %>
    <%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Fuel Invoice Register</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script language="javascript" src="VALIDATEFUNCTIONS.js">
    </script>
    <link    id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" id="clientEventHandlersJS">
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
    <link href="AutoComplete\jquery.autocomplete.css" type="text/css" rel="stylesheet">
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
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
    <div>
        <table id="tblmain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td colspan="4" class="clsFormHeader1Newstyle">
                                    <span id="lbltitle" class="clsFormHeader">Fuel Invoice Register</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:UpdatePanel runat="server" ID="upnlValidations" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                HeaderText="Fill Up The Following Fields" ValidationGroup="a"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                                ErrorMessage="From Date Required" ControlToValidate="txtFromDate" Display="None"
                                                ValidationGroup="a"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtToDate"
                                                CssClass="clsLabelAuto" Display="None" ErrorMessage="To Date Required" ValidationGroup="a"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
                                                ClientValidationFunction="BetweenDatesValidation" ValidationGroup="a" ErrorMessage="From Date should not be greater than To Date "></asp:CustomValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblStep2" runat="server" CssClass="clsLabelHeader">Step I. Selection of Date</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblFromDate" class="clsLabelAuto">From</span>
                                </td>
                                <td>
                                    <asp:TextBox CssClass="clsTextBoxTagSearchDate" runat="server" ID="txtFromDate"
                                        onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                    <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                    </cc2:CalendarExtender>
                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                        WatermarkCssClass="clsDateTextBox">
                                    </cc2:TextBoxWatermarkExtender>
                                </td>
                                <td>
                                    <span id="lblToDate" class="clsLabelAuto">To</span>
                                </td>
                                <td>
                                    <asp:TextBox CssClass="clsTextBoxTagSearchDate" runat="server" ID="txtToDate"
                                        onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
                                    <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                    </cc2:CalendarExtender>
                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                        WatermarkCssClass="clsDateTextBox">
                                    </cc2:TextBoxWatermarkExtender>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="Label1" runat="server" CssClass="clsLabelHeader">Step II. Selection of Supplier</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblSupplier" class="clsLabelAuto">Supplier</span>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox CssClass="clsTextBoxSearch_Ajax" ID="txtSupplier" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <span id="lblStep4" class="clsLabelHeader">Step III. Selection of Fuel Invoice No.</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblFuelInvoiceNo" class="clsLabelAuto">Fuel Invoice No.</span>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtFuelInvoicTextList" runat="server"></asp:TextBox>
                                    <asp:TextBox CssClass="clsTextBoxTagSearchSmall" ID="txtFuelInvoicNo" runat="server"
                                        MaxLength="8"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblStep7" runat="server" CssClass="clsLabelHeader">Step IV. Display Report</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:UpdatePanel ID="upnlDisplaySearchCriteria" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows :</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblVendor" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblFuelInvoiceNumber" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="4">
                                    <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlButton" runat="server" CssClass="clspanel1">
                                                <table cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" 
                                                                Text="Current Criteria" ToolTip="Click to display Current Searching criterias">
                                                            </asp:Button>
                                                        </td>
                                                        <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnExport" TabIndex="0" runat="server"  Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                        Text="Export to Excel" ToolTip="Click to Export report"></asp:Button>


                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" TabIndex="0" runat="server" 
                                                                ValidationGroup="a" Text="Display" ToolTip="Click to display report"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnByMail" runat="server" Text="Report By Mail"
                                                                ToolTip="Click to report by mail"  />
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" TabIndex="0" runat="server" Text="Close"
                                                                ToolTip="Click to close" CausesValidation="False"></asp:Button>
                                                            <asp:Button  ID="hdnimgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
                                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                                        </td>
                                                    </tr>
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
    </form>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtSupplier.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Supplier', {
                width: 252,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });
            $("#<%=txtFuelInvoicTextList.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Text&TextType=25', {
                width: 187,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });

        });
    </script>
</body>
</html>
