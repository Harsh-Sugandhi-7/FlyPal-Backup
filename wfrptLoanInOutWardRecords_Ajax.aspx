<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptLoanInOutWardRecords_Ajax.aspx.vb"
    Inherits="Flypal.wfrptLoanInOutWardRecords_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Other InWard/OutWard</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="jquery.textchange.min.js"></script>
    <script type="text/javascript" id="clientEventHandlersJS" language="javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body>
    <form id="frmrptPartHitory" method="post" runat="server">
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
                    <asp:Panel ID="pnlmain" CssClass="clspanel1" runat="server">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td class="clsFormHeader1" colspan="4">
                                    <span class="clsFormHeader" id="lbltitle">Other InWard/OutWard</span>
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
                                <td colspan="2">
                                    <span id="lblStepDate" class="clsLabelHeader">Step I. Selection of Date</span>
                                </td>
                                <td colspan="2">
                                    <span id="lblStepSupplier" class="clsLabelHeader">Step V. Selection of Supplier</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblFrom" class="clsLabelAuto">From</span>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:TextBox CssClass="clsTextBoxTagDateSearch" Width="100px" runat="server" ID="txtFromDate"
                                                    onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                    WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                            </td>
                                            <td>
                                                <span id="lblTo" class="clsLabelAuto">To</span>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="clsTextBoxTagDateSearch" Width="100px" runat="server" ID="txtToDate"
                                                    onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
                                                <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                    WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <span id="lblSupplier" class="clsLabelAuto">Supplier</span>
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbSupplier" runat="server" TabIndex="8"
                                        DataTextField="Name" DataValueField="ID">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span id="lblStepCustomer" class="clsLabelHeader">Step II. Selection of Customer</span>
                                </td>
                                <td colspan="2">
                                    <span id="lblStepAircraft" class="clsLabelHeader">Step VI. Selection of Aircraft</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlchkCustomerStock" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:CheckBox ID="chkCustomerStock" runat="server" AutoPostBack="True" CssClass="clsLabelAuto"
                                                TabIndex="4" Text="Check Customer Stock" TextAlign="Left" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <span id="lblAircraft" class="clsLabelAuto">Aircraft</span>
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAircraft" runat="server" TabIndex="8"
                                        DataTextField="RegNo" DataValueField="ID">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblCustomer" class="clsLabel">Customer</span>
                                </td>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlCustomer" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbCustomer" runat="server" AutoPostBack="True"
                                                TabIndex="5" Enabled="False" DataValueField="ID" DataTextField="Name">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td colspan="2">
                                    <span id="lblStepWorkShop" class="clsLabelHeader">Step VII. Selection of WorkShop</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span id="lblReceivingStore" class="clsLabelHeader">Step III. Selection of Store</span>
                                </td>
                                <td>
                                    <span id="lblWorkShop" class="clsLabelAuto">WorkShop</span>
                                </td>
                                <td>
                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbWorkShop" runat="server" TabIndex="8"
                                        DataTextField="LocationWorkShop" DataValueField="ID">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td width="96px"></td>
                                <td>
                                    <asp:Label ID="lblStoreCount" ForeColor="DarkBlue" runat="server" Font-Size="XX-Small" Font-Bold="true" class="clsLabelAuto"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblStore" class="clsLabel">Store</span>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upnlStore" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbStore" runat="server"
                                                DataTextField="LocationStore" DataValueField="ID">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td colspan="2">
                                    <span id="lblStepCategory" class="clsLabelHeader">Step VIII. Selection of 
                                                        Category</span>
                                </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <span id="lblStepPartNumberDescription" class="clsLabelHeader">Step IV. 
                                Selection of Part Number/Description</span>
                                    </td>
                                    <td>
                                        <span id="lblCategory" class="clsLabelAuto">Category</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbCategory" runat="server"
                                            DataTextField="Name" DataValueField="ID" TabIndex="8">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="lblSearch" class="clsLabel">Search</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBoxTagSearch"
                                            TabIndex="10" Width="275px"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <span id="lblStepFormat" class="clsLabelHeader">Step IX. Selection of Format</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:CheckBox ID="chkShowInValuation" runat="server" Checked="True"
                                            CssClass="clsCheckBox" TabIndex="22" Text="Consider Show in Valuation Only"
                                            TextAlign="Left" />
                                    </td>
                                    <td>
                                        <span id="lblFormat" class="clsLabelAuto">Format</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbFormat" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle">
                                            <asp:ListItem Selected="True" Value="1">Other Outwards</asp:ListItem>
                                            <asp:ListItem Value="2">Other Inwards</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <span id="lblStepDisplayReport" class="clsLabelHeader">Step X. Display Report</span>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:UpdatePanel ID="upnlSerachCriteria" runat="server"
                                            UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td align="left" colspan="2">
                                                            <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto"
                                                                Text="Your selection is as follows" Visible="false"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto"
                                                                Visible="false"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto"
                                                                Visible="false"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="lblCust" runat="server" CssClass="clsLabelAuto" Visible="false"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="lblSupp" runat="server" CssClass="clsLabelAuto" Visible="false"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="lblRecevingStore" runat="server" CssClass="clsLabelAuto"
                                                                Visible="false"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="lblAir" runat="server" CssClass="clsLabelAuto" Visible="false"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="lblWoShop" runat="server" CssClass="clsLabelAuto"
                                                                Visible="false"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="lblCate" runat="server" CssClass="clsLabelAuto" Visible="false"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabelAuto"
                                                                Visible="false"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="lblPartDescription" runat="server" CssClass="clsLabelAuto"
                                                                Visible="false"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="4">
                                        <asp:UpdatePanel ID="upnlActionBtns" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH" ID="btnCurrentSearchCriteria" runat="server"
                                                                TabIndex="23" Text="Current Criteria"
                                                                ToolTip="Click to display current searching criterias" />
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH" ID="btnExport" runat="server" TabIndex="0" Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                                Text="Export to Excel" ToolTip="Click to Export report" />
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH" ID="btnDisplay" runat="server"
                                                                Text="Display" ToolTip="Click to display report" />
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH" ID="btnCloseTop" runat="server" CausesValidation="False"
                                                                Text="Close" ToolTip="Click to close" />
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
</body>
</html>
