<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptValuationAnalysis_Ajax.aspx.vb"
    Inherits="Flypal.wfrptValuationAnalysis_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Valuation Analysis</title>
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
                                <span id="lbltitle" class="clstitle1">Valuation Analysis</span>
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
                                <span id="lblStep1" class="clsLabelHeader">Step I. Selection of Date</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td width="80px">
                                            <span id="lblDateRange" class="clsLabel">Date Range</span>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="upnlDateRange" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="cmbDateRange" runat="server" CssClass="clsComboBox_Ajax" AutoPostBack="true"
                                                        TabIndex="1">
                                                        <asp:ListItem Value="0">(All)</asp:ListItem>
                                                        <asp:ListItem Value="1">Last Week</asp:ListItem>
                                                        <asp:ListItem Value="2">Last Month</asp:ListItem>
                                                        <asp:ListItem Value="3">Last Quarter</asp:ListItem>
                                                        <asp:ListItem Value="4">Last Year</asp:ListItem>
                                                        <asp:ListItem Value="5">Current Financial Year</asp:ListItem>
                                                        <asp:ListItem Value="6">Between Dates</asp:ListItem>
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upnlDates" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto" Visible="false">From</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFromDate" CssClass="clsTextBoxDate_Ajax" ClientIDMode="Static"
                                                        TabIndex="2" runat="server" onchange="ValidateDateText(this,'FromDate_watermarkextender');"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="txtFromDate" ID="FromDate_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox">
                                                    </cc2:TextBoxWatermarkExtender>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto" Visible="false">To</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtToDate" Style="margin-left: 3px;" CssClass="clsTextBoxDate_Ajax"
                                                        TabIndex="3" onchange="ValidateDateText(this,'ToDate_watermarkextender');" ClientIDMode="Static"
                                                        runat="server"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
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
                                <asp:UpdatePanel ID="upnlCustomer" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td align="left" colspan="3">
                                                    <span id="lblSelectionofCustomer" class="clsLabelHeader">Step II. Selection of Customer</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="80px">
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkCustomerStock" runat="server" CssClass="clsLabelAuto" AutoPostBack="True"
                                                        TabIndex="4" Text="Check Customer Stock"></asp:CheckBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" width="80px">
                                                    <asp:Label ID="lblCustomer" runat="server" CssClass="clsLabel">Customer</asp:Label>
                                                </td>
                                                <td>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbCustomer" runat="server" CssClass="clsComboBox3_Ajax" AutoPostBack="True"
                                                        TabIndex="5" Enabled="False" DataValueField="ID" DataTextField="Name">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="3">
                                                    <span id="lblStep2" class="clsLabelHeader">Step III. Selection of Store</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblStoreCount" ForeColor="DarkBlue" runat="server" Font-Size="XX-Small"
                                                        Font-Bold="true" class="clsLabelAuto"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <span id="lblStore" class="clsLabel">Store</span>
                                                </td>
                                                <td>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="cmbStore" runat="server" CssClass="clsComboBox3_Ajax" TabIndex="6"
                                                        DataTextField="LocationStore" DataValueField="ID">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlAssembly" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td align="left" colspan="3">
                                                                <span id="Label2" class="clsLabelHeader">Step VIII. Selection of Model</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" width="80px">
                                                                <asp:Label ID="lblAssembly" runat="server" CssClass="clsLabel">Assembly </asp:Label>
                                                            </td>
                                                            <td align="left">
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropDownList ID="cmbAssemblyType" runat="server" CssClass="clsComboBox3_Ajax"
                                                                    AutoPostBack="True" TabIndex="11" DataValueField="ID" DataTextField="Name">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" width="80px">
                                                                <span id="lblModel" class="clsLabelAuto">Model </span>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td align="left">
                                                                <asp:DropDownList ID="cmbModelType" runat="server" CssClass="clsComboBox3_Ajax" DataValueField="ID"
                                                                    TabIndex="12" DataTextField="ModelName">
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
                                            <span id="Label3" class="clsLabelHeader">Step IX. Selection of Base,Landing,Commercial
                                                Value</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td align="left" width="80px">
                                                        <span id="Label4" runat="server" class="clsLabelAuto">Value</span>
                                                    </td>
                                                    <td align="left">
                                                    </td>
                                                    <td align="left">
                                                        <asp:RadioButton ID="rbBase" runat="server" CssClass="clsRadioButton" Text="Base"
                                                            onclick="Enable();" ClientIDMode="Static" TabIndex="13" GroupName="b"></asp:RadioButton>
                                                        <asp:RadioButton ID="rbLanding" runat="server" CssClass="clsRadioButton" Text="Landing"
                                                            onclick="Enable();" ClientIDMode="Static" TabIndex="14" GroupName="b"></asp:RadioButton>
                                                        <asp:RadioButton ID="rbCommercial" runat="server" CssClass="clsRadioButton" Text="Commercial"
                                                            onclick="Enable();" ClientIDMode="Static" TabIndex="15" GroupName="b"></asp:RadioButton>
                                                        <asp:CheckBox ID="chkWithGST" runat="server" Checked="false" CssClass="clsCheckBox"
                                                            ClientIDMode="Static" Text="With GST" Visible='<%# AppSettings("IsGSTApplicable")="True" %>' />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblStep3" class="clsLabelHeader">Step IV. Selection of Category</span>
                            </td>
                            <td>
                                <span id="Label5" class="clsLabelHeader">Step X. Selection of Open/Authorized Transaction</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td width="80px">
                                            <span id="lblCategory" class="clsLabel">Category</span>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbCategory" runat="server" CssClass="clsComboBox3_Ajax" TabIndex="7"
                                                DataTextField="Name" DataValueField="ID">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td width="80px">
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="optAll" runat="server" Checked="True" CssClass="clsLabel" GroupName="a"
                                                TabIndex="16" Text="All" />
                                            <asp:RadioButton ID="optOnlyAuthorized" runat="server" CssClass="clsLabel" GroupName="a"
                                                TabIndex="17" Text="Only Authorized" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblStep4" class="clsLabelHeader">Step V. Selection of Supplier</span>
                            </td>
                            <td>
                                <span id="lblSortBy" class="clsLabelHeader">Step XI. Sort By</span>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <table>
                                    <tr>
                                        <td width="80px">
                                            <span id="lblVendor" class="clsLabel">Supplier</span>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbVendor" runat="server" CssClass="clsComboBox3_Ajax" TabIndex="8"
                                                DataTextField="Name" DataValueField="ID">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td rowspan="3" valign="top">
                                <asp:UpdatePanel ID="upnlSortBy" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td width="80px">
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="chkCategoryWise" runat="server" CssClass="clsLabelAuto" Text="Category Wise"
                                                                    OnClick="setValues();" TabIndex="18"></asp:CheckBox>
                                                            </td>
                                                            <td>
                                                                <asp:RadioButton ID="rdoCategorySummary" runat="server" CssClass="clsLabel" TabIndex="19"
                                                                    Text="Summary" GroupName="x" Checked="True" ClientIDMode="Static"></asp:RadioButton>
                                                                <asp:RadioButton ID="rdoCategoryDetail" runat="server" CssClass="clsLabel" TabIndex="20"
                                                                    Text="Detail" GroupName="x" ClientIDMode="Static"></asp:RadioButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="80px">
                                                    <span id="lblSortBy1" class="clsLabelAuto">Sort By</span>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbSortBy" runat="server" CssClass="clsComboBox_Ajax" TabIndex="21">
                                                        <asp:ListItem Value="0">Part No.</asp:ListItem>
                                                        <asp:ListItem Value="1">Description</asp:ListItem>
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
                                <span id="lblStep5" class="clsLabelHeader">Step VI. Selection of Nomenclature</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td width="80px">
                                            <span id="lblNomenclature" class="clsLabel">Nomenclature</span>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbNomenclature" runat="server" CssClass="clsComboBox3_Ajax"
                                                TabIndex="9" DataTextField="Name" DataValueField="ID">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblStep6" class="clsLabelHeader">Step VII. Selection of Part Number/Description</span>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td width="80px">
                                            <asp:Label ID="lblSearch" runat="server" CssClass="clsLabel">Search</asp:Label>
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtSearch" runat="server" CssClass="clsTextBoxRemark_Ajax" Width="520px"
                                                            TabIndex="10"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkShowInValuation" runat="server" Checked="True" TabIndex="22"
                                                            CssClass="clsCheckBox" Text="Consider Show in Valuation Only" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <span id="lblStep7" class="clsLabelHeader">Step XII. Display Report</span>
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
                                        <table width="100%">
                                            <tr>
                                                <td colspan="2" align="left">
                                                    <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="left">
                                                    <asp:Label ID="lblCustomerName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblStoreName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblCategoryName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblVendorName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblNomenclatureName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblAssembly1" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblModel1" runat="server" CssClass="clsLabelAuto"></asp:Label>
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
                                                    <asp:Button ID="btnExport" runat="server" CssClass="clsButton_Ajax" TabIndex="24"
                                                        Text="Export to Excel" ToolTip="Click to Export report" ValidationGroup="1" Visible="<%$AppSettings:ShowExportToExcelButton%>" />
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
                                                        TabIndex="26" Text="Close" ToolTip="Click to close Valuation Analysis screen" />
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
            var selectedSearchIndex = $get("cmbDateRange").selectedIndex;
            if (selectedSearchIndex == 6) {
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
    <!-- Popup For Valuation -->
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
    <script type="text/javascript">
        function setValues() {
            var status = $("#chkCategoryWise").attr('checked');
            if (status == "checked") {
                $("#cmbSortBy").attr('disabled', 'disabled');
                $("#cmbSortBy").val('0')
                $("#rdoCategorySummary").removeAttr('disabled');
                $("#rdoCategoryDetail").removeAttr('disabled');

            }
            else {
                $("#cmbSortBy").removeAttr('disabled');
                $("#rdoCategorySummary").attr('disabled', 'disabled');
                $("#rdoCategoryDetail").attr('disabled', 'disabled');
                $("#rdoCategorySummary").attr('checked', true);
                $("#rdoCategoryDetail").attr('checked', false);
            }
        }
    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=txtSearch.ClientID %>").autocomplete('wfAutoItemList.aspx?', {
                width: 520,
                autoFill: false,
                matchContains: true,
                max: 100,
                delay: 0
            });
        });
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            setValues();
        });  
    </script>
    <script type="text/javascript">
        var Enable = function () {
            var LandingChecked = $get("rbLanding").checked;
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
