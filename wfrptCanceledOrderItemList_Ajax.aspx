<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptCanceledOrderItemList_Ajax.aspx.vb"
    Inherits="Flypal.wfrptCanceledOrderItemList_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Canceled Order Item</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="jquery.textchange.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout ="600" ID="ScriptManager1" runat="server" EnablePageMethods="true">
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
                    <table id="tblInner" class="clstablelistin">
                        <tr>
                            <td class="clsFormHeader1Newstyle">
                                <span id="lbltitle" class="clsFormHeader">Canceled Order Item List</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlValidationsummary">
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
                                      
                                        <script type="text/javascript">
                                            function showTextField(elem) {

                                                var txtFromDateobj = document.getElementById("<%= txtFromDate.ClientID %>");
                                                var txtToDateobj = document.getElementById("<%= txtToDate.ClientID %>");
                                                var lblFromDateobj = document.getElementById("<%= lblFromDate.ClientID %>");
                                                var lblToDateobj = document.getElementById("<%= lblToDate.ClientID %>");
                                                if (elem.selectedIndex == 0) {
                                                    txtFromDateobj.style.display = 'none';
                                                    txtToDateobj.style.display = 'none';
                                                    lblFromDateobj.style.display = 'none';
                                                    lblToDateobj.style.display = 'none';
                                                }

                                            }
                                        </script>
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
                                                <td colspan="6">
                                                    <span id="lblStep1" class="clsLabelHeader">Step I. Selection of Date</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="96px">
                                                    <span id="lblDateRange" class="clsLabel">Date Range</span>
                                                </td>
                                                <td width="185px">
                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbDateRange" runat="server" AutoPostBack="True"
                                                        onchange="showTextField(this);">
                                                        <asp:ListItem Value="0">(All)</asp:ListItem>
                                                        <asp:ListItem Value="1">Last Week</asp:ListItem>
                                                        <asp:ListItem Value="2">Last Month</asp:ListItem>
                                                        <asp:ListItem Value="3">Last Quarter</asp:ListItem>
                                                        <asp:ListItem Value="4">Last Year</asp:ListItem>
                                                        <asp:ListItem Value="5">Current Financial Year</asp:ListItem>
                                                        <asp:ListItem Value="6">Between Dates</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto">From</asp:Label>
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
                                                    <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto">To</asp:Label>
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
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlSelectionOfOrder" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td colspan="2">
                                                    <span id="spanStepII" class="clsLabelHeader">Step II. Selection of Order No.</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="96px">
                                                    <span id="lblOrderTextNo" class="clsLabelAuto">Order No.</span>
                                                </td>
                                                <td>
                                                    <table id="Table9" cellspacing="0" cellpadding="0" width="85%">
                                                        <tr>
                                                            <td align="left">
                                                                <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtOrderTextList" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox CssClass="clsTextBoxTagSearchSmall" ID="txtOrderNo" runat="server"  MaxLength="8"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox CssClass="clsTextBoxTagSearchSmall" ID="txtAmend" runat="server"  MaxLength="4"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <span id="lblStep8" class="clsLabelHeader">Step III. Selection of Part Number/Description</span>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span id="Span4" class="clsLabel">Search</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox CssClass="clsTextBoxSearch_Ajax" ID="txtSearch" runat="server" AutoPostBack="False"
                                                        ></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel runat="server" ID="upnlSelection" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <span id="Span2" class="clsLabelHeader">Step IV. Display Report</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="lblOrderNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDesc" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" 
                                                        Text="Current Criteria" ToolTip="Click to display Current Searching criterias">
                                                    </asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" TabIndex="0" runat="server" 
                                                        Text="Display" ToolTip="Click to display report" ValidationGroup="a"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" TabIndex="0" runat="server" Text="Close"
                                                        ToolTip="Click to close the Canceled Order Item List Report screen" CausesValidation="False">
                                                    </asp:Button>
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
    </form>
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
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtOrderTextList.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Text&TextType=1', {
                width: 250,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });
        });       
    </script>
     <script type="text/javascript">
         Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
         function endRequestHandler() {
             var dd = document.getElementById("cmbDateRange");
             showTextField(dd);
         }    
    </script>
</body>
</html>
