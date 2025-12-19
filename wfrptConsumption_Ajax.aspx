<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptConsumption_Ajax.aspx.vb"
    Inherits="Flypal.wfrptConsumption_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head id="Head1" runat="server">
    <title>Consumption Report</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script language="javascript" src="VALIDATEFUNCTIONS.js">
		
    </script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" id="clientEventHandlersJS">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
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
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table id="tblInner" class="clstablelistin">
                            <tr>
                                <td class="clsFormHeader1Newstyle">
                                    <span id="lbltitle" class="clsFormHeader">Consumption Report</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlValidationsummary" UpdateMode="Conditional">
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
                                    <span id="lblStep2" class="clsLabelHeader">Step I. Selection of Date</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlDateSelection" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td width="96px">
                                                        <span id="lblDateRange" class="clsLabel">Date Range</span>
                                                    </td>
                                                    <td width="270px">
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
                                                    <td width="45px">
                                                        <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto" Width="45px">From</asp:Label>
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
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto">To</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtToDate" ClientIDMode="Static"
                                                            runat="server" CausesValidation="true" onchange="ValidateDateText(this,'ToDate_watermarkextender');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtToDateCalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
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
                                    <span id="lblStep1" class="clsLabelHeader">Step II. Selection of Issue Type</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlcmbIssue" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td width="96px">
                                                        <span id="lblIssue" class="clsLabel">Issue Type</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbIssue" runat="server" AutoPostBack="True"
                                                           >
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblStep3" runat="server" CssClass="clsLabelHeader"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblType" runat="server" CssClass="clsLabel">To Type</asp:Label>
                                                    </td>
                                                    <td width="270px">
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbType" runat="server">
                                                            <asp:ListItem Value="0">(All)</asp:ListItem>
                                                            <asp:ListItem Value="1">Vendor</asp:ListItem>
                                                            <asp:ListItem Value="2">Aircraft</asp:ListItem>
                                                            <asp:ListItem Value="8">Store</asp:ListItem>
                                                            <asp:ListItem Value="7">Discard</asp:ListItem>
                                                            <asp:ListItem Value="16">WorkShop</asp:ListItem>
                                                            <asp:ListItem Value="17">WorkOrder</asp:ListItem>
                                                            <asp:ListItem Value="18">Requisition</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblType1" runat="server" CssClass="clsLabelAuto" Visible="False" Width="75px">Vendor</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtCustomer" runat="server" 
                                                            Visible="False" Width="250px"></asp:TextBox>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtSupplier" runat="server" 
                                                            Visible="False" Width="250px"></asp:TextBox>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbStore" runat="server" DataTextField="LocationStore"
                                                            DataValueField="ID" Width="250px">
                                                        </asp:DropDownList>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtWorkShop" runat="server"
                                                            Visible="False" Width="250px"></asp:TextBox>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtAircraft" runat="server"
                                                            Visible="False" Width="250px"></asp:TextBox>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtWorkOrder" runat="server" ></asp:TextBox>
                                                        <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtWONo" runat="server" Visible="False"
                                                            MaxLength="8"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="Label1" class="clsLabelHeader">Step IV. Selection of Category</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td width="96px">
                                                <span id="lblCategory" class="clsLabelAuto">Category</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbCategory" runat="server" DataTextField="Name"
                                                    DataValueField="ID">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label5" runat="server" CssClass="clsLabelHeader">Step V. Selection of Store/Customer</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlFromStore" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td width="96px">
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkCustomerStock" runat="server" CssClass="clsLabelAuto" AutoPostBack="True"
                                                            TabIndex="4" Text="Check Customer Stock"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="96px">
                                                        <asp:Label ID="lblCustomer" runat="server" CssClass="clsLabel">Customer</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbCustomer" runat="server" AutoPostBack="True"
                                                            DataTextField="Name" DataValueField="ID" Enabled="False" TabIndex="5">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="96px">
                                                        
                                                    </td>
                                                    <td>
                                                       <asp:Label ID="lblStoreCount" ForeColor="DarkBlue" runat="server" Font-Size="XX-Small" Font-Bold="true" class="clsLabelAuto"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="96px">
                                                        <span id="Span1" class="clsLabelAuto">From Store</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbFromStore" runat="server" DataTextField="LocationStore"
                                                            DataValueField="ID">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <span id="Label6" class="clsLabelHeader">Step VI. Selection of IsValued Store</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td width="96px">
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkIsValued" runat="server" CssClass="clsCheckBox" Text="Include Valued Stores Only"
                                                    Checked="True"></asp:CheckBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblStep8" class="clsLabelHeader">Step VII. Selection of Part Number/Description</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td width="96px">
                                                <asp:Label ID="lblSearch" runat="server" CssClass="clsLabelAuto" Width="80px">Search</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="clsTextBoxSearch_Ajax" ID="txtSearch" runat="server"
                                                    AutoPostBack="False"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <span id="lblSelectCriteria" class="clsLabelHeader">Step VIII.Selection of Base,Landing,Commercial
                                        Value</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td width="96px">
                                                <span id="lblValue" class="clsLabel">Value</span>
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdoBase" runat="server" CssClass="clsRadioButton" Text="Base"
                                                    onclick="Enable();" ClientIDMode="Static" GroupName="Gr1"></asp:RadioButton>
                                                <asp:RadioButton ID="rdoLanding" runat="server" CssClass="clsRadioButton" Text="Landing"
                                                    onclick="Enable();" ClientIDMode="Static" Checked="True" GroupName="Gr1"></asp:RadioButton>
                                                <asp:RadioButton ID="rdoCommercial" runat="server" CssClass="clsRadioButton" Text="Commercial"
                                                    onclick="Enable();" ClientIDMode="Static" GroupName="Gr1"></asp:RadioButton>
                                                <asp:CheckBox ID="chkWithGST" runat="server" Checked="true" CssClass="clsCheckBox"
                                                    ClientIDMode="Static" Text="With GST" Visible='<%# AppSettings("IsGSTApplicable")="True" %>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <span id="lblStep9" class="clsLabelHeader">Step IX.Selection of Format</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlFormat" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td width="96px">
                                                        <span id="Span3" class="clsLabel">Format</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbFormat" runat="server" AutoPostBack="true">
                                                            <asp:ListItem Value="0">Format 1</asp:ListItem>
                                                            <asp:ListItem Value="1">Format 2</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:Label runat="server" CssClass="clsLabelHeader" Visible="false" ID="lblGROValuesInfo">GRO values gets consider in this format.</asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="Span2" class="clsLabelHeader">Step X. Enter text to be display at bottom line
                                        of report</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td width="96px">
                                                <span id="Span4" class="clsLabel">Text</span>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="clsTextBoxSearch_Ajax" ID="txtBottomLine" runat="server" AutoPostBack="False"
                                                    Text='<%# " Submitted By : " + User.Identity.Name %>' Width="520px" MaxLength="100"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblStep10" runat="server" CssClass="clsLabelHeader">Step XI. Display Report</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlCurrentSearchCriteria" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <asp:Label ID="lblSummary" runat="server" CssClass="clsLabelAuto">Your selection is as follows </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblVendor" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblCategoryName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblFromStore" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
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
                                <td align="right">
                                    <asp:UpdatePanel ID="upnlButton" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" runat="server" 
                                                            TabIndex="0" Text="Current Criteria" ToolTip="Click to display Current Searching criterias" />
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnExport" TabIndex="0" runat="server" Visible="<%$AppSettings:ShowExportToExcelButton%>"
                                                            ToolTip="Click to Export report" Text="Export to Excel">
                                                        </asp:Button>
                                                       
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" runat="server" TabIndex="0"
                                                            ValidationGroup="a" Text="Display" ToolTip="Click to display report" />
                                                    </td>
                                                    <td>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" CausesValidation="False"
                                                            TabIndex="0" Text="Close" ToolTip="Click to close the Consumption Report screen" />
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
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
        function endRequestHandler() {
            var dd = document.getElementById("cmbDateRange");
            showTextField(dd);
        }    
    </script>
    </form>
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtSearch.ClientID %>").autocomplete('wfAutoItemList.aspx?', {
                width: 522,
                autoFill: false,
                matchContains: true,
                delay: 0
            });
            $("#<%=txtCustomer.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Customer', {
                width: 252,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });
            $("#<%=txtSupplier.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Supplier', {
                width: 252,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });
            $("#<%=txtAircraft.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Aircraft', {
                width: 252,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });

            $("#<%=txtWorkShop.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=WorkShop', {
                width: 252,
                autoFill: false,
                matchContains: true,

                delay: 0
            });
            $("#<%=txtWorkOrder.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Text&TextType=16', {
                width: 252,
                autoFill: false,
                matchContains: true,
                mustMatch: true,
                delay: 0
            });
        });
    </script>
    <script type="text/javascript">
        var Enable = function () {
            var LandingChecked = $get("rdoLanding").checked;
            if (LandingChecked) {
                $("#chkWithGST").css('visibility', 'visible');
                $("#chkWithGST").next().css('visibility', 'visible');
                $("#chkWithGST").attr('checked', true);
            }
            else {

                $("#chkWithGST").css('visibility', 'hidden');
                $("#chkWithGST").next().css('visibility', 'hidden');
            }
        }
    </script>
</body>
</html>
