<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptGROExpenseReport_AJAX.aspx.vb"
    Inherits="Flypal.wfrptGROExpenseReport_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>GRO Expense Report</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />
    <%-- Ajay 09-Nov-2022--%>
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script type="text/javascript" src="jquery.textchange.min.js"></script>
    <script id="clientEventHandlersJS" language="javascript">
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
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
            <table id="tblmain" class="clstablelistout" border="0">
                <tr>
                    <td>
                        <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                            <table id="tblInner" class="clstablelistin" border="0">
                                <tr>
                                    <td>
                                        <table width="100%" class="clsFormHeader1Newstyle">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbltitle" CssClass="clstitle1" runat="server">GRO Expense Report</asp:Label>
                                                </td>
                                                <%--  <td style="width: 1%" align="center">
                                                <span id="FavClk"><i id="FavIClk" runat="server" onclick="FunctionFav(this)" style="font-size: 21px;
                                                    color: black; border: black; cursor: pointer" class="fa fa-star fa-spin fa-5x circle-icon"
                                                    title="Mark As Favourites"></i>
                                                    <%--  Ajay 09-Nov-2022
                                                </span>
                                            </td>--%>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <%--<td style="width: 1%" align="center">
                                                <span id="FavClk"><i id="FavIClk" runat="server" onclick="FunctionFav(this)" style="font-size: 21px;
                                                    color: black; border: black; cursor: pointer" class="fa fa-star fa-spin fa-5x circle-icon"
                                                    title="Mark As Favourites"></i>
                                                </span>
                                            </td>--%>

                                                <td style="width: 1%" align="center">
                                                    <span id="FavClk"><i id="FavIClk" runat="server" onclick="FunctionFav(this)" style="font-size: 17px; color: black; border: black; cursor: pointer"
                                                        class="fa fa-star fa-spin fa-5x circle-icon"
                                                        title="Mark As Favourites"></i></span>
                                                </td>


                                            </tr>
                                        </table>
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
                                                        <td width="75px">
                                                            <span id="lblDateRange" class="clsLabel">Date Range</span>
                                                        </td>
                                                        <td>
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
                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto">To</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox CssClass="clsTextBoxTagSearchDate" runat="server" ID="txtToDate"
                                                                onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
                                                            <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                                            <cc2:TextBoxWatermarkExtender TargetControlID="txtToDate" ID="ToDate_watermarkextender"
                                                                ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel runat="server" ID="upnlSelectionExpense" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="lblStep12" runat="server" CssClass="clsLabelHeader">Step II. Selection Of Expenses</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblExpenses" runat="server" class="clsLabelAuto">Expenses</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbExpenses" runat="server">
                                                                <asp:ListItem Value="0">All</asp:ListItem>
                                                                <asp:ListItem Value="1">Schedule Expenses</asp:ListItem>
                                                                <asp:ListItem Value="2">Nonschedule Expenses</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <span id="Label7" class="clsLabelHeader">Step III. Selection of Supplier</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span id="lblSupplier" class="clsLabelAuto">Supplier</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox CssClass="clsTextBoxSearch_Ajax" ID="txtSupplierList" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader">Step IV. Selection of Aircraft</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 75px" align="left">
                                                            <span id="lblAircraft" runat="server" class="clsLabelAuto">Aircraft</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox CssClass="clsTextBoxSearch_Ajax" ID="txtAircraft" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <span id="Label2" class="clsLabelHeader">Step V. Selection of Model</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 75px" align="left">
                                                            <span id="spanModel" class="clsLabel">Model</span>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel runat="server" ID="upnlModelSelection" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbModel" runat="server" DataTextField="ModelName"
                                                                        DataValueField="ID">
                                                                    </asp:DropDownList>
                                                                    <asp:CheckBox ID="chkCommonOrApplicability" runat="server" AutoPostBack="true" CssClass="clsCheckBox"
                                                                        Text="Common/No Applicability" ToolTip="Common/No Applicability" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="lblStep10" runat="server" CssClass="clsLabelHeader">Step VI. Selection of Part Number/Description</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 75px" align="left">
                                                            <span id="lblSearch" runat="server" class="clsLabelAuto">Search</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox CssClass="clsTextBoxSearch_Ajax" ID="txtPartDescription" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="lblValuedStores" runat="server" CssClass="clsLabelHeader" Visible="false">Step VII. Selection For Valued, Non-Valued Stores</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblType" runat="server" CssClass="clsLabelAuto">Type</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbStoreType" runat="server">
                                                                <asp:ListItem Value="0">(All)</asp:ListItem>
                                                                <asp:ListItem Value="1">Valued</asp:ListItem>
                                                                <asp:ListItem Value="2">Non-Valued</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="lblGST" runat="server" CssClass="clsLabelHeader" Visible='<%# AppSettings("IsGSTApplicable")="True" %>'>Step VIII. Selection For Values With/Without GST</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkWithGST" runat="server" Checked="true" CssClass="clsCheckBox"
                                                                ClientIDMode="Static" Text="With GST" Visible='<%# AppSettings("IsGSTApplicable")="True" %>' />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="lblCustomerStore" runat="server" CssClass="clsLabelHeader">Step IX. Selection of Store/Customer</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkCustomerStock" runat="server" CssClass="clsLabelAuto" AutoPostBack="True"
                                                                TabIndex="4" Text="Check Customer Stock"></asp:CheckBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblCustomer" runat="server" CssClass="clsLabel">Customer</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbCustomer" runat="server" AutoPostBack="True"
                                                                DataTextField="Name" DataValueField="ID" Enabled="False" TabIndex="5">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="96px"></td>
                                                        <td>
                                                            <asp:Label ID="lblStoreCount" ForeColor="DarkBlue" runat="server" Font-Size="XX-Small"
                                                                Font-Bold="true" class="clsLabelAuto"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label20" runat="server" CssClass="clsLabel" Width="90px">Store</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbStore" runat="server" AutoPostBack="True"
                                                                DataTextField="LocationStore" DataValueField="ID">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td></td>
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
                                                        <td align="left">
                                                            <asp:Label ID="lblStep11" runat="server" CssClass="clsLabelHeader">Step X. Display Report</asp:Label>
                                                        </td>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows :</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="lblExpenses1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblCustomerName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="lblAircraft1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="lblModel1" runat="server" CssClass="clsLabelAuto" Visible="false"></asp:Label>
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
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="lblStoreType" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2">
                                        <asp:UpdatePanel runat="server" ID="upnlButtons" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table border="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" runat="server"
                                                                TabIndex="0" Text="Current Criteria" ToolTip="Click to display Current Searching criterias" />
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnExport" TabIndex="0" runat="server"
                                                                Visible="<%$AppSettings:ShowExportToExcelButton%>" Text="Export to Excel" ValidationGroup="a"
                                                                ToolTip="Click to Export report"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" runat="server" TabIndex="0"
                                                                Text="Display" ToolTip="Click to display report" ValidationGroup="a" />
                                                        </td>
                                                        <td>
                                                            <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" CausesValidation="False"
                                                                TabIndex="0" Text="Close" ToolTip="Click to close the GRO Expense Report screen" />
                                                        </td>
                                                        <td>
                                                            <%--Ajay 09-Nov-2022--%>
                                                            <asp:Button ID="hdnBtnMarkFav" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
                                                                Style="display: none;"></asp:Button>
                                                            <asp:Button ID="hdnBtnRemoveFav" ClientIDMode="Static" runat="server" Text="----"
                                                                CausesValidation="False" Style="display: none;"></asp:Button>
                                                        </td>
                                                    </tr>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="200" ClientIDMode="Static" DynamicLayout="false"
                runat="server">
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
        </div>
        <!--Ajay S 09-Nov-2022 -->
        <script type="text/javascript">
            function FunctionFav(x) {
                if (x.classList.contains("fa-star")) {
                    x.classList.remove("fa-star");
                    x.classList.add("fa-star-o");
                    x.style.color = 'black';
                    x.style.border = 'black';
                    $("#hdnBtnRemoveFav").click();
                }
                else {
                    x.classList.remove("fa-star-o");
                    x.classList.add("fa-star");
                    x.style.color = '#fff';
                    x.style.border = 'black';
                    $("#hdnBtnMarkFav").click();
                }
            }
            function MarkFav() {
                var redstar = document.getElementById("<%=FavIClk.ClientID%>");
                redstar.classList.add("fa-star");
                redstar.classList.remove("fa-star-o");
                redstar.style.color = '#fff';
                redstar.style.border = 'black';

            }
            function RemoveFav() {
                var redstar = document.getElementById("<%=FavIClk.ClientID%>");
                redstar.classList.add("fa-star-o");
                redstar.classList.remove("fa-star");
                redstar.style.border = 'black';
            }
        </script>
        <!--Ajay E -->
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
        $(document).ready(function () {
            $("#<%=txtPartDescription.ClientID%>").autocomplete('wfAutoItemList.aspx?', {
                width: 520,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });
            $("#<%=txtAircraft.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=OrderAircraftReg', {
                width: 252,
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
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtSupplierList.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Supplier', {
                width: 275,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });
        });
    </script>
</body>
</html>
