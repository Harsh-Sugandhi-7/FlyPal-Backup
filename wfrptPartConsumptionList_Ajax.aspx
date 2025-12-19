<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptPartConsumptionList_Ajax.aspx.vb"
    Inherits="Flypal.wfrptPartConsumptionList_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Part Consumption Report</title>
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
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" runat="server" ID="ScriptManager1"
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
                            <td colspan="2"> 
                            <table width="100%"> 
                            <tr> 
                                <td colspan="2" class="clsFormHeader1Newstyle">
                                    <span id="lbltitle" class="clstitle1" Style="width: 100%">Part Consumption Report</span>
                                </td>
                                <td style="width: 1%" align="center">
                                            <span id="FavClk"><i id="FavIClk" runat="server" onclick="FunctionFav(this)" style="font-size: 21px;
                                                color: black; border: black; cursor: pointer" class="fa fa-star fa-spin fa-5x circle-icon"
                                                title="Mark As Favourites"></i>
                                                <%--  Ajay 09-Nov-2022--%>
                                            </span>
                                        </td>
                                </tr>
                                </td>
                                </table>
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
                                            <asp:CustomValidator ID="CustomValidator1" runat="server" CssClass="clsLabelAuto"
                                                ValidationGroup="a" Display="None" ControlToValidate="" ClientValidationFunction="ValidateChkList"
                                                ErrorMessage="Select at least one category."></asp:CustomValidator>
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
                                    <span id="lblStep1" class="clsLabelHeader">Selection of Date</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlDateRange" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <td>
                                                    <span id="lblDateRange" class="clsLabel">Date Range</span>
                                                </td>
                                                <td>
                                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbDateRange" runat="server"  AutoPostBack="True"
                                                        onchange="showTextField(this);">
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
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblStep2" class="clsLabelHeader">Selection of Category</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlCategory" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <input type="checkbox" id="chkSelectAll" style="margin-left: 4px">
                                                        <span class="clsLabelAuto">Select All</span>
                                                        <script type="text/javascript">
                                                            $(document).ready(function () {
                                                                $("#chkSelectAll").click(function () {
                                                                    var status = $("#chkSelectAll").attr("checked");
                                                                    $("#<%=ChklistCategory.ClientID %>").find(":checkbox").each(function () {
                                                                        if (status == "checked") {
                                                                            $(this).attr("checked", status);
                                                                        }
                                                                        else {
                                                                            $(this).removeAttr("checked");
                                                                        }

                                                                    });
                                                                });
                                                                return false;
                                                            });
                                                        </script>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBoxList ID="ChklistCategory" runat="server" CssClass="clsComboBox" DataTextField="Name"
                                                            DataValueField="ID" RepeatColumns="4" RepeatDirection="Horizontal" ToolTip="Category List"
                                                            Visible="True" Width="100%">
                                                        </asp:CheckBoxList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblStep6" class="clsLabelHeader">Selection of Issue</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlIssueSelection" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span id="lblAircraft" class="clsLabel">Issued To</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbDocType" runat="server" AutoPostBack="True">
                                                            <asp:ListItem Value="0">(All)</asp:ListItem>
                                                            <asp:ListItem Value="1">Aircraft</asp:ListItem>
                                                            <asp:ListItem Value="2">WorkShop</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblCostCenter" runat="server" CssClass="clsLabel" Visible="False">Aircraft</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbMachine" runat="server"  Visible="False"
                                                            DataTextField="RegNo" DataValueField="MachineID">
                                                        </asp:DropDownList>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbWorkShop" runat="server"  Visible="False"
                                                            DataTextField="LocationWorkShop" DataValueField="ID">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="Label2" class="clsLabelHeader">Selection of Assembly/Model</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlModelSelection" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span id="lblAssembly" class="clsLabel">Assembly </span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbAssemblyType" ClientIDMode="Static" runat="server"
                                                            AutoPostBack="False" DataValueField="ID" DataTextField="Name">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <span id="lblModel" class="clsLabelAuto">Model </span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxSearch_Ajax" ID="txtModelList" runat="server" 
                                                            AutoPostBack="True" onfocus="GetAssemblyTypeID()"></asp:TextBox>
                                                        <cc2:AutoCompleteExtender runat="server" ID="txtModelList_AutoCompleteExtender" TargetControlID="txtModelList"
                                                            ServiceMethod="GetCompletionList" MinimumPrefixLength="0" EnableCaching="true"
                                                            CompletionSetCount="20" CompletionInterval="1000" UseContextKey="True" CompletionListCssClass="ac_results_Main"
                                                            CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main">
                                                        </cc2:AutoCompleteExtender>
                                                        <script type="text/javascript">
                                                            function GetAssemblyTypeID() {
                                                                var autoComplete = $find('txtModelList_AutoCompleteExtender');

                                                                var str = $("#cmbAssemblyType :selected").val();
                                                                autoComplete.set_contextKey(str);
                                                            }
                                                        </script>
                                                        <%--TargetControlID - The TextBox control where the user types content to be automatically completed.

EnableCaching- Caching is turned on, so typing the same prefix multiple times results in only one call to the web service.

MinimumPrefixLength- Minimum number of characters that must be entered before getting suggestions from the web service.

CompletionInterval - Time in milliseconds when the timer will kick in to get suggestions using the web service.

CompletionSetCount - Number of suggestions to be retrieved from the web service.--%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblSelectCriteria" class="clsLabelHeader">Selection of Base,Landing,Commercial
                                        Value</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlValue" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span id="lblValue" class="clsLabel">Value</span>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rdoBase" runat="server" CssClass="clsRadioButton" GroupName="Gr1"
                                                            Text="Base"></asp:RadioButton>
                                                        <asp:RadioButton ID="rdoLanding" runat="server" CssClass="clsRadioButton" GroupName="Gr1"
                                                            Text="Landing" Checked="True"></asp:RadioButton>
                                                        <asp:RadioButton ID="rdoCommercial" runat="server" CssClass="clsRadioButton" GroupName="Gr1"
                                                            Text="Commercial"></asp:RadioButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="Span1" class="clsLabelHeader">Selection to show only valued parts with landing
                                        rate greater than entered value </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlHighValue" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <%--<span id="Span2" class="clsLabel">Value</span>--%>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkHighValue" runat="server" CssClass="clsCheckBox" Text="Show only valued parts with landing rate greater than "
                                                            AutoPostBack="true" />
                                                        <asp:TextBox CssClass="clsTextBoxTagSearchSmall" ID="txtCEffectiveRate" runat="server" 
                                                            Enabled="false" MaxLength="12"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlPartSearch" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td colspan="2">
                                                        <span id="lblStep8" class="clsLabelHeader">Selection of Part Number/Description</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="Span4" class="clsLabel">Search&nbsp</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox CssClass="clsTextBoxSearch_Ajax" ID="txtSearch" runat="server" 
                                                            AutoPostBack="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td colspan="2">
                                                        <span id="Span2" class="clsLabelHeader">Selection of Supplier</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lblSupplier" class="clsLabel">Supplier</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbSupplier" ClientIDMode="Static" runat="server"
                                                            DataValueField="ID" DataTextField="Name">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlStore" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td colspan="2">
                                                        <span id="Span3" class="clsLabelHeader">Selection of Store</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="Span5" class="clsLabel">Store&nbsp&nbsp&nbsp&nbsp</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbStore" ClientIDMode="Static" runat="server" 
                                                            DataValueField="ID" DataTextField="LocationStore">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlSelectionOfFormat" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td colspan="3">
                                                        <span id="Label4" class="clsLabelHeader">Selection of Format</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="Label5" class="clsLabel">Format&nbsp&nbsp</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbFormat" runat="server" AutoPostBack="True">
                                                            <asp:ListItem Value="1">Format 1</asp:ListItem>
                                                            <asp:ListItem Value="2">Format 2</asp:ListItem>
                                                            <asp:ListItem Value="3">Format 3</asp:ListItem>
                                                            <asp:ListItem Value="4">Format 4</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkPartwise" runat="server" CssClass="clsCheckBox" Text="Show Partwise Consumption "
                                                            AutoPostBack="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:Label ID="lblNote" runat="server" CssClass="clsLabelHeader" Visible="False"
                                                            Width="500">Note:Issue to Discard Transactions are included in this format.</asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblValuedStores" runat="server" CssClass="clsLabelHeader" Visible="false">Selection For Valued, Non-Valued Store</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table id="Table1" runat="server" visible='<%# AppSettings("ClientCode")="Deccan" %>'>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto" Width="90px">Type</asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbStoreType" runat="server">
                                                    <asp:ListItem Value="0">(All)</asp:ListItem>
                                                    <asp:ListItem Value="1">Valued</asp:ListItem>
                                                    <asp:ListItem Value="2">Non-Valued</asp:ListItem>
                                                </asp:DropDownList>
                                                <br />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblStep4" class="clsLabelHeader">Display Report</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlSelection" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table>
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
                                                        <asp:Label ID="lblCategory1" runat="server" CssClass="clsLabelAuto" Visible="False"
                                                            Width="700px"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblAircraftCrit" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblWorkShopCrit" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblAssembly1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblModel1" runat="server" CssClass="clsLabelAuto"></asp:Label>
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
                                            <table>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnCurrentSearchCriteria" TabIndex="0" runat="server" 
                                                            ToolTip="Click to Display Current Searching criterias" Text="Current Criteria"
                                                            CausesValidation="False"></asp:Button>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnExport" TabIndex="0" runat="server" 
                                                            Visible="<%$AppSettings:ShowExportToExcelButton%>" ValidationGroup="a" ToolTip="Click to Export report"
                                                            Text="Export to Excel" ></asp:Button>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnDisplay" TabIndex="0" runat="server" 
                                                            ToolTip="Click to Display Report" ValidationGroup="a" Text="Display"></asp:Button>
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnByMail" runat="server" Text="Report By Mail"
                                                            ValidationGroup="a" ToolTip="Click to report by mail"  />
                                                        <asp:Button CssClass="clsbtnH clsinfoH1" ID="btnClose" TabIndex="0" runat="server"  ToolTip="Click to close Part Consumption Report screen"
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
                            <!--Dummy panel to open modelpopup-->
                            <tr style="height: 0px;">
                                <td style="height: 0px;" align="right">
                                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                        <ContentTemplate>
                                            <asp:Button ID="hdnimgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
                                                CausesValidation="false" Style="display: none;"></asp:Button>
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
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
        function endRequestHandler() {
            var dd = document.getElementById("cmbDateRange");
            showTextField(dd);
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
