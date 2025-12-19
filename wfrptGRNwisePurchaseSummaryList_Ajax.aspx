<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptGRNwisePurchaseSummaryList_Ajax.aspx.vb"
    Inherits="Flypal.wfrptGRNwisePurchaseSummaryList_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>GRN Wise Purchase Summary List</title>
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
    <script id="clientEventHandlersJS" language="javascript">
        function openFile() {
            str = "wfExportToExcel.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="5" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
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
                            <td colspan="2">
                                <table width="100%">
                                    <tr>
                                        <td class="clsFormHeader1Newstyle">
                                            <span id="lbltitle" class="clsFormHeader" Style="width: 100%">GRN Wise Purchase Summary List</span>
                                        </td>
                                        <td style="width: 1%" align="center">
                                            <span id="FavClk"><i id="FavIClk" runat="server" onclick="FunctionFav(this)" style="font-size: 21px;
                                                color: black; border: black; cursor: pointer" class="fa fa-star fa-spin fa-5x circle-icon"
                                                title="Mark As Favourites"></i>
                                                <%--  Ajay 09-Nov-2022--%>
                                            </span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="clsValidationSummary">
                                </asp:ValidationSummary>
                                <asp:CustomValidator ID="CustomValidator1" runat="server" CssClass="clsLabelAuto"
                                    Display="None" ControlToValidate="" ClientValidationFunction="ValidateChkList"
                                    ErrorMessage="Select at least one category."></asp:CustomValidator>
                                <asp:CustomValidator ID="cvCategory1" runat="server" CssClass="clsLabelAuto" Display="None"
                                    ControlToValidate="" ClientValidationFunction="ValidateChkListCount" ErrorMessage="Report does not allow more than 5 categories, please break categories into multiple report prints."></asp:CustomValidator>
                                <script type="text/javascript">
                                    function ValidateChkList(source, args) {
                                        args.IsValid = false;
                                        $("#<%=ChklistCategory.ClientID %>").find(":checkbox").each(function () {
                                            if ($(this).attr("checked")) {
                                                args.IsValid = true;
                                                return;
                                            }
                                        });
                                    }
                                    function ValidateChkListCount(source, args) {
                                        var count = 0;
                                        args.IsValid = false;
                                        $("#<%=ChklistCategory.ClientID %>").find(":checkbox").each(function () {
                                            if ($(this).attr("checked")) {
                                                count += 1;
                                            }
                                        });
                                        if (count <= 5) {
                                            args.IsValid = true;
                                            return;
                                        }

                                    }
                                </script>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span id="lblStep1" class="clsLabelHeader">Step I. Selection of Date</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblDateRange" class="clsLabel">Date Range</span>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upnlDateRange" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbDateRange" runat="server" AutoPostBack="True">
                                                        <asp:ListItem Value="1">(All)</asp:ListItem>
                                                        <asp:ListItem Value="2">Last Week</asp:ListItem>
                                                        <asp:ListItem Value="3">Last Month</asp:ListItem>
                                                        <asp:ListItem Value="4">Last Quarter</asp:ListItem>
                                                        <asp:ListItem Value="5">Last Year</asp:ListItem>
                                                        <asp:ListItem Value="6">Current Financial Year</asp:ListItem>
                                                        <asp:ListItem Value="7">Between Dates</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto" Visible="False">From</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtFromDate"  ClientIDMode="Static"
                                                        runat="server" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                    <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                                        Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtFromDate"
                                                        ErrorMessage="From Date Required."></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clsLabelAuto"
                                                        Display="None" ControlToValidate="txtFromDate" ErrorMessage="From Date Required."></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvToDate" runat="server" CssClass="clsLabelAuto"
                                                        ErrorMessage="To Date Required." ControlToValidate="txtToDate" Display="None"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvToDate1" runat="server" CssClass="clsLabelAuto"
                                                        Display="None" InitialValue="<%$AppSettings:DateFormat%>" ControlToValidate="txtToDate"
                                                        ErrorMessage="To Date Required."></asp:RequiredFieldValidator>
                                                    <asp:CustomValidator ID="cvCommon" runat="server" CssClass="clsLabelAuto" ErrorMessage="From Date should not be greater than To Date."
                                                        ClientValidationFunction="BetweenDatesValidation" Display="None"></asp:CustomValidator>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto" Visible="False">To</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox CssClass="clsTextBoxTagSearchDate" ID="txtToDate" Style="margin-left: 3px;"
                                                        onchange="ValidateDateText(this,'ToDate_watermarkextender');" ClientIDMode="Static"
                                                        runat="server"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                    </cc2:CalendarExtender>
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
                            <td colspan="2">
                                <span id="Label7" class="clsLabelHeader">Step II. Selection of Supplier</span>
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
                            <td colspan="2" align="left">
                                <span id="lblStep2" class="clsLabelHeader">Step III. Selection of Category</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBoxList ID="ChklistCategory" runat="server" CssClass="clsComboBox_Ajax"
                                    RepeatColumns="4" RepeatDirection="Horizontal" Width="100%" DataValueField="id"
                                    DataTextField="Name">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span id="Span1" class="clsLabelHeader">Step IV.Selection of Model</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="spanModel" class="clsLabel">Model</span>
                            </td>
                            <td>
                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbModel" runat="server" DataTextField="ModelName"
                                    DataValueField="ID">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span id="lblSelectCriteria" class="clsLabelHeader">Step V.Selection of Base,Landing,Commercial
                                    Value</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblValue" class="clsLabel">Value</span>
                            </td>
                            <td>
                                <table id="Table4" border="0" cellspacing="0" cellpadding="1">
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="rdoBase" runat="server" CssClass="clsRadioButton" Text="Base"
                                                GroupName="Gr1"></asp:RadioButton>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdoLanding" runat="server" CssClass="clsRadioButton" Text="Landing"
                                                GroupName="Gr1" Checked="True"></asp:RadioButton>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="rdoCommercial" runat="server" CssClass="clsRadioButton" Text="Commercial"
                                                GroupName="Gr1"></asp:RadioButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span id="Label1" class="clsLabelHeader">Step VI.Select Format of Report</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblFormat" class="clsLabel">Format</span>
                            </td>
                            <td>
                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbFormat" runat="server">
                                    <asp:ListItem Value="0">Format 1</asp:ListItem>
                                    <asp:ListItem Value="1">Format 2 </asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <span id="lblStep7" class="clsLabelHeader">Step VII. Display Report</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <span id="lblSummary" class="clsLabelAuto">Your selection is as follows :</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlCurrentCriteria" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblCustomerName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblCategory1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblModel" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table border="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" TabIndex="0" runat="server"
                                                        ToolTip="Click to Display Current Searching criterias" Text="Current Criteria">
                                                    </asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnExport" TabIndex="0" runat="server" 
                                                        Visible="<%$AppSettings:ShowExportToExcelButton%>" Text="Export to Excel" ValidationGroup="valGrp1"
                                                        ToolTip="Click to Export report"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" runat="server" ToolTip="Click to Display Report"
                                                        Text="Display" CausesValidation="true"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" runat="server" ToolTip="Click to close GRN Wise Purchase Summary list screen"
                                                        Text="Close" CausesValidation="False"></asp:Button>
                                                </td>
                                                <td>
                                                        <%--Ajay 09-Nov-2022--%>
                                                        <asp:Button ID="hdnBtnMarkFav" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
                                                            Style="display: none;"></asp:Button>
                                                        <asp:Button ID="hdnBtnRemoveFav" ClientIDMode="Static" runat="server" Text="----"
                                                            CausesValidation="False" Style="display: none;"></asp:Button>
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
            var index;
            index = $get("cmbDateRange").selectedIndex;
            if (index == 6) {
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
            var index;
            index = $get("cmbDateRange").selectedIndex;
            if (index == 6) {
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

        }
       
    </script>
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
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtSupplierList.ClientID%>").autocomplete('wfAutoInventoryList.aspx?Type=Supplier', {
                width: 275,
                autoFill: false,
                matchContains: true,
                delay: 0
            });
        });       
    </script>
</body>
</html>
