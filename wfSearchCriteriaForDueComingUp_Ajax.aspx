<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfSearchCriteriaForDueComingUp_Ajax.aspx.vb"
    Inherits="Flypal.wfSearchCriteriaForDueComingUp_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Due Periodwise Report</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link href="bootstrapt/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="bootstrapt/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <link href="//netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap-glyphicons.css"
        rel="stylesheet" />
    <script src="bootstrapt/jquery-1.8.3.min.js" type="text/javascript"></script>
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
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <style type="text/css">
        .style1
        {
            width: 62px;
            height: 23px;
        }
        .btn
        {
            padding: 1px;
            font-size: 8pt;
        }
        .TextBox
        {
            box-sizing: Content-box;
        }
        .label
        {
            font-weight: normal !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="1200" ID="ScriptManager1" runat="server">
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
                        <table id="tblInner" class="clstablelistin" border="0">
                            <tr>
                                <td class="clsFormHeader1">
                                    <asp:UpdatePanel runat="server" ID="upnlTitle" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lbltitle" runat="server" CssClass="clsFormHeader">Extended Forecast Due</asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlValidations" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                HeaderText="Fill Up The Following Fields" ValidationGroup="1"></asp:ValidationSummary>
                                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clslabelauto"
                                                InitialValue="<%$AppSettings:DateFormat%>" ErrorMessage="As On Date Required"
                                                ControlToValidate="txtFromDate" Display="None" ValidationGroup="1"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvFromDate1" runat="server" CssClass="clslabelauto"
                                                ErrorMessage="As On Date Required" validateEmptyText="true" ControlToValidate="txtFromDate"
                                                Display="None" ValidationGroup="1"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvType" runat="server" CssClass="clsLabelAuto" Display="None"
                                                ValidationGroup="1"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvSelection" runat="server" CssClass="clsLabelAuto" Display="None"
                                                ControlToValidate="cmbAircraft" ErrorMessage="Please select at least One Service,Inspection or Directive"
                                                ValidationGroup="1" ClientValidationFunction="validateSelection"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvPeriodLimitsValue" runat="server" Display="None" ErrorMessage="CustomValidator"
                                                OnServerValidate="CustomValidate1" ValidationGroup="1"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvPeriodLimitsValuePerDay" runat="server" Display="None"
                                                ErrorMessage="CustomValidator" OnServerValidate="CustomValidate1" ValidationGroup="1"></asp:CustomValidator>
                                            <script type="text/javascript">
                                                function validateSelection(source, args) {
                                                    args.IsValid = false;
                                                    //                                                    var status;
                                                    //                                                    $('#chkListServiceType').find(":checkbox").each(function () {
                                                    //                                                        status = $(this).attr('checked');
                                                    //                                                        if (status == "checked") {
                                                    //                                                            args.IsValid = true;
                                                    //                                                            return;
                                                    //                                                        }
                                                    //                                                    });
                                                    //                                                    $('#chkListInspectionType').find(":checkbox").each(function () {
                                                    //                                                        status = $(this).attr('checked');
                                                    //                                                        if (status == "checked") {
                                                    //                                                            args.IsValid = true;
                                                    //                                                            return;
                                                    //                                                        }
                                                    //                                                    });
                                                    //                                                    $('#chkListDirectiveType').find(":checkbox").each(function () {
                                                    //                                                        status = $(this).attr('checked');
                                                    //                                                        if (status == "checked") {
                                                    //                                                            args.IsValid = true;
                                                    //                                                            return;
                                                    //                                                        }
                                                    //                                                    });
                                                    var ServStatus = document.getElementById("chkService");
                                                    var InspStatus = document.getElementById("chkInspection");
                                                    var DirStatus = document.getElementById("chkDirective");
                                                    var $items = $('.active').length;

                                                    if ((ServStatus.checked || InspStatus.checked || DirStatus.checked) && ($items > 0)) {
                                                        args.IsValid = true;
                                                        return;
                                                    }
                                                }
                                            </script>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlDetails" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td colspan="2">
                                                        <span id="lblStep1" class="clsLabelHeader">Step I. Selection of As On Date</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="150px">
                                                        <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto">As On Date</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtFromDate" runat="server" AutoPostBack="true" CssClass="clsTextBoxTagDateSearch" Height ="24px"
                                                            ClientIDMode="Static" onchange="ValidateDateText(this,'Calender_watermarkextender');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ClientIDMode="Static" TargetControlID="txtFromDate"
                                                            ID="Calender_watermarkextender" runat="server" WatermarkText="<%$AppSettings:DateFormat%>">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <span id="lblStep2" class="clsLabelHeader">Step II. Selection of Aircraft</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblAircraft" runat="server" CssClass="clsLabelAuto">Aircraft</asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList ID="cmbAircraft" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
                                                            Visible="false" DataTextField="RegNo" DataValueField="ID">
                                                        </asp:DropDownList>
                                                        <asp:UpdatePanel runat="server" ID="upnlAircraft" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:ListBox ID="ListRegNo" runat="server" ClientIDMode="Static" SelectionMode="Multiple"
                                                                    AutoPostBack="true" DataTextField="RegNo" DataValueField="ID"></asp:ListBox>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <span id="Label3" class="clsLabelHeader">Step III. Selection of Assembly</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblAssembly" runat="server" CssClass="clsLabelAuto">Assembly</asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:UpdatePanel runat="server" ID="upnlAssembly" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="cmbAssembly" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataTextField="ModelSerialNoPostion"
                                                                    DataValueField="ID">
                                                                </asp:DropDownList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <span id="Span1" class="clsLabelHeader">Step IV. Selection of Type</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <table id="Table1" border="0" width="100%">
                                        <tr>
                                            <td width="225px">
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox Text="" ID="chkService" runat="server" ClientIDMode="Static" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td width="100%">
                                                            <asp:ListBox ID="ListServiceType" runat="server" ClientIDMode="Static" SelectionMode="Multiple"
                                                                DataTextField="CodeType" DataValueField="ID"></asp:ListBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td width="225px">
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox Text="" ID="chkInspection" runat="server" ClientIDMode="Static" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:ListBox ID="ListInspectionType" runat="server" ClientIDMode="Static" SelectionMode="Multiple"
                                                                DataTextField="CodeType" DataValueField="ID"></asp:ListBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td width="225px">
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox Text="" ID="chkDirective" runat="server" ClientIDMode="Static" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:ListBox ID="ListDirectiveType" runat="server" ClientIDMode="Static" SelectionMode="Multiple"
                                                                DataTextField="CodeType" DataValueField="ID"></asp:ListBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblStep5" runat="server" CssClass="clsLabelHeader">Step V. Selection of Due Limits / Percentage Life Remaining</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlDueLimits" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td align="left" width="150px">
                                                        <asp:RadioButton ID="rbdDueLimits" runat="server" CssClass="clsRadioButton" AutoPostBack="True"
                                                            GroupName="StepIII" Font-Bold="True" Checked="True" Text="Due Limits"></asp:RadioButton>
                                                    </td>
                                                    <td align="left">
                                                        <asp:RadioButton ID="rbdPercent" runat="server" CssClass="clsRadioButton" AutoPostBack="True"
                                                            GroupName="StepIII" Font-Bold="True" Text="Percent Life Remaining"></asp:RadioButton>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:TextBox ID="txtPercentage" runat="server" CssClass="clsTextBoxTagSearchSmall" Height="24px"
                                                            MaxLength="4" ToolTip="Enter Percentage" Enabled="False"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <asp:Panel ID="Panel1" runat="server" CssClass="clspanel1">
                                                            <asp:GridView ID="gdvDuePeriodLimits" runat="server" CssClass="clsGridNewStyle" AutoGenerateColumns="False" GridLines="Horizontal" CellPadding="5">
                                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black"/>
                                                                <Columns>
                                                                    <asp:BoundField DataField="PeriodName" HeaderText="Period">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Limit">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtLimit" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Width ="200px" Height ="24px"
                                                                                Text='<%# DataBinder.Eval(Container.DataItem,"PeriodLimit") %>' ToolTip="Enter corresponding Limit Value."
                                                                                BackColor="White"> </asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblStep6" runat="server" CssClass="clsLabelHeader">Step VI. Estimated Flying Hours.</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label2" runat="server" CssClass="clsLabelAuto">(For Estimated Due-Dates Calculation)</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlAvrgperiod" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td align="left">
                                                        <asp:RadioButton ID="rbdAvrageMonths" runat="server" CssClass="clsRadioButton" AutoPostBack="True"
                                                            GroupName="StepIV" Font-Bold="True" Text="Average in Months"></asp:RadioButton>
                                                    </td>
                                                    <td align="left">
                                                        <asp:RadioButton ID="rbdSpecifyValues" runat="server" CssClass="clsRadioButton" AutoPostBack="True"
                                                            GroupName="StepIV" Font-Bold="True" Text="Specify Values"></asp:RadioButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" width="150px">
                                                        <asp:Label ID="lblAvgMnths" runat="server" CssClass="clsLabelAuto">Average for last</asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtAvgMnths" runat="server" CssClass="clsTextBoxTagSearchSmall"  Height="24px"
                                                            MaxLength="4" ToolTip="Enter Average Months"></asp:TextBox>
                                                        <asp:Label ID="lblMonths" runat="server" CssClass="clsLabelAuto">Months</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <asp:Label ID="lblInfo" runat="server" CssClass="clsLabelAuto" Visible="False">Enter per day Values of Following Periods</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <asp:Panel ID="pnlAvragePeriod" runat="server" CssClass="clspanel1" Visible="False">
                                                            <asp:GridView ID="gdvPerDayLimit" runat="server" CssClass="clsGridNewStyle" AutoGenerateColumns="False" CellPadding="5" GridLines="Horizontal">
                                                                <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                                <RowStyle CssClass="clsdgItem"></RowStyle>
                                                                <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black"/>
                                                                <Columns>
                                                                    <asp:BoundField Visible="False" DataField="PeriodID" HeaderText="PeriodID"></asp:BoundField>
                                                                    <asp:BoundField DataField="PeriodName" HeaderText="Period">
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Limit">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtLimitPerDay" runat="server" CssClass="clsTextBoxTagSearchRightAlignQty_Ajax" Width ="200px" Height="24px"
                                                                                Text='<%# DataBinder.Eval(Container.DataItem,"PeriodLimit") %>' ToolTip="Enter corresponding Limit Value."
                                                                                BackColor="White"> </asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label4" runat="server" CssClass="clsLabelHeader">Step VII. Enter The Limit For Forecasting</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td align="left" width="142px">
                                                <asp:Label ID="lblLimit" runat="server" CssClass="clsLabelAuto">Limit</asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtForecastingLimit" runat="server" CssClass="clsTextBoxTagSearchSmall" Height ="24px"
                                                    MaxLength="4" ToolTip="Enter Limit">30</asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblStep8" runat="server" CssClass="clsLabelHeader" Visible="False">Step VII. Enter refrence no.</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td align="left" width="142px">
                                                <asp:Label ID="lblRefNo" runat="server" CssClass="clsLabelAuto" Visible="False">Refrence No.</asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtRefNo" runat="server" CssClass="clsTextBoxTagSearchSmall" Visible="False"
                                                    MaxLength="50" ToolTip="Enter Refrence No."></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td align="left">
                                    <asp:Label ID="Label5" runat="server" CssClass="clsLabelHeader">Step VIII. Selection of Format</asp:Label>
                                </td>
                            </tr>
                            <tr style="display: none">
                                <td>
                                    <table>
                                        <tr>
                                            <td align="left" width="142px">
                                                <asp:Label ID="lblFormat" runat="server" CssClass="clsLabelAuto">Format</asp:Label>
                                            </td>
                                            <td align="left">
                                                <table id="Table6" border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td>
                                                            <asp:DropDownList ID="cmbFormat" ClientIDMode="Static" runat="server" CssClass="clsTextBoxTagSearchComboSmall1">
                                                                <asp:ListItem Value="0">Format 1</asp:ListItem>
                                                                <asp:ListItem Value="1">Format 2</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="right">
                                                            <asp:CheckBox ID="chkSignature" ClientIDMode="Static" runat="server" CssClass="clsCheckBox"
                                                                Text="With Signature" Style="visibility: hidden;"></asp:CheckBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblStep7" runat="server" CssClass="clsLabelHeader">Step IX. Display Report</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <span id="lblSummary" class="clsLabelAuto">Your selection is as follows :</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel runat="server" ID="upnlSearchingCriteria" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblAircraft1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblAssembly1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblType1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblAvgMnths1" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblPercent" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:UpdatePanel runat="server" ID="upnlActionBtns" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnCurrentSearchCriteria" runat="server" CausesValidation="true"
                                                            CssClass="clsbtnH" TabIndex="0" Text="Current Criteria" ToolTip="Click to display Current Searching criterias." />
                                                    </td>
                                                    <td style="display: none">
                                                        <asp:Button ID="btnPreview" runat="server" CausesValidation="true" CssClass="clsbtnH"
                                                            ValidationGroup="1" TabIndex="0" Text="Preview" ToolTip="Click to Preview Report"
                                                            Visible="False" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnDisplay" runat="server" CssClass="clsbtnH" TabIndex="0"
                                                            Text="Display" ToolTip="Click to Display Report" ValidationGroup="1" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnByMail" runat="server" CssClass="clsbtnH" TabIndex="25"
                                                            Text="Report By Mail" ToolTip="Click to receive Report through mail" ValidationGroup="1"
                                                            Width="140px" Visible="True" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnByExcel" runat="server" CssClass="clsbtnH" TabIndex="25"
                                                            Text="Export to Excel" ToolTip="Click to Export to Excel" ValidationGroup="1"
                                                            Width="140px" Visible="False" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH"
                                                            TabIndex="0" Text="Close" ToolTip="Back to Previous Page" />
                                                    </td>
                                                </tr>
                                                <!--Dummy panel to open modelpopup-->
                                                <tr style="height: 0px;">
                                                    <td style="height: 0px;" colspan="2" align="right">
                                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlImgBtn">
                                                            <ContentTemplate>
                                                                <asp:Button ID="hdnimgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
                                                                    CausesValidation="False" Style="display: none;"></asp:Button>
                                                                <asp:Button ID="hdnAircraft" ClientIDMode="Static" runat="server" Text="Add" CausesValidation="False"
                                                                    Style="display: none;"></asp:Button>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <!--End -->
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
            //Date validations
            function ValidateDateText(elem, extenderid) {

                var datevalue = $(elem).val();
                var params = { 'Date': datevalue, 'SetDefault': 'true' };
                $.ajax({
                    type: "POST",
                    url: "DateValidationHandler.ashx",
                    //        contentType: "application/json",
                    cache: false,
                    data: params,
                    async: false,
                    beforeSend: OnBeforeSend,
                    //                beforeSend: function (xhr, settings) {
                    //                    $("[id$=processing]").dialog();
                    //                },
                    success: onSuccess,
                    error: onError
                });

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
            //function for wo no checkbox visibility
            function ControlvisibilityForWONo(flag) {
                if (flag == "True") {
                    $("#chkwithWONo").css('visibility', 'visible');
                    $("#chkwithWONo").next().css('visibility', 'visible');
                    ControlVisibilityForFormat($("#chkwithWONo"));
                }
                else {
                    $("#chkwithWONo").css('visibility', 'hidden');
                    $("#chkwithWONo").next().css('visibility', 'hidden');
                    $("#chkwithWONo").removeAttr('checked');
                    ControlVisibilityForFormat($("#chkwithWONo"));
                }

            }
            //Service/inspection/Directive list checking
            //            function ControlvisibilityForCheckboxlist(elem, childid) {
            //                //if selected then enable and select checkboxlist else uncheck and disable list
            //                var status = $(elem).attr('checked');
            //                if (status == "checked") {
            //                    $('#' + childid).removeAttr('disabled');
            //                }
            //                else {
            //                    $('#' + childid).attr('disabled', 'disabled');
            //                }

            //                $('#' + childid).find(":checkbox").each(function () {
            //                    if (status == "checked") {
            //                        $(this).attr("checked", status);
            //                        $(this).removeAttr('disabled');
            //                    }
            //                    else {
            //                        $(this).removeAttr("checked");
            //                        $(this).attr('disabled', 'disabled');
            //                    }
            //                });
            //            }

            //wo no checkbox status change event
            function ControlVisibilityForFormat(elem) {
                var status = $(elem).attr('checked');
                if (status == "checked") {
                    $('#cmbFormat').attr('disabled', 'disabled');
                    $('#cmbFormat').val('0');

                    if ('<%# AppSettings("ClientCode") %>' == "ADeccan") {
                        $("#chkSignature").css('visibility', 'visible');
                        $("#chkSignature").next().css('visibility', 'visible');
                    }
                }
                else {
                    $('#cmbFormat').removeAttr('disabled');

                    if ('<%# AppSettings("ClientCode") %>' == "ADeccan") {
                        $("#chkSignature").css('visibility', 'hidden');
                        $("#chkSignature").next().css('visibility', 'hidden');
                        $("#chkSignature").removeAttr('checked');
                    }
                }
            }
        </script>
    </div>
    <!-- Popup For Report By Mail -->
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
            $("#hdnimgBtnSendMail").click();
        }
    </script>
    <!---End-->
    </form>
    <script src="bootstrapt/bootstrap.min.js" type="text/javascript"></script>
    <script src="bootstrapt/bootstrap-multiselect.js" type="text/javascript"></script>
    <script type="text/javascript">

        $("#chkService").live("click", function () {

            var status = $(this).attr('checked');
            if (status) {
                $('[id*=ListServiceType]').multiselect('enable', true);                       // * Enable the multiselect ListBOx
                $('[id*=ListServiceType]').multiselect('selectAll', false);
                $('[id*=ListServiceType]').multiselect('updateButtonText');
            }
            else {
                $('[id*=ListServiceType]').multiselect('clearSelection', true);
                $('[id*=ListServiceType]').multiselect('disable', false);
            }

        });
        $("#chkInspection").live("click", function () {
            var status = $(this).attr('checked');
            if (status) {
                $('[id*=ListInspectionType]').multiselect('enable', true);
                $('[id*=ListInspectionType]').multiselect('selectAll', false);
                $('[id*=ListInspectionType]').multiselect('updateButtonText');
            }
            else {
                $('[id*=ListInspectionType]').multiselect('clearSelection', true);
                $('[id*=ListInspectionType]').multiselect('disable', false);
            }
        });
        $("#chkDirective").live("click", function () {
            var status = $(this).attr('checked');
            if (status) {
                $('[id*=ListDirectiveType]').multiselect('enable', true);
                $('[id*=ListDirectiveType]').multiselect('selectAll', false);
                $('[id*=ListDirectiveType]').multiselect('updateButtonText');
            }
            else {
                $('[id*=ListDirectiveType]').multiselect('clearSelection', true);
                $('[id*=ListDirectiveType]').multiselect('disable', false);
            }
        });

    </script>
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $('[id*=ListServiceType]').multiselect({
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                includeSelectAllOption: true,
                disableIfEmpty: true,
                maxHeight: 180,
                nonSelectedText: 'Services',
                selectAllJustVisible: false,
                buttonWidth: '185px',
                allSelectedText: 'Services',
                nSelectedText: 'Services'

            });
            $(".caret").css('float', 'right');
            $(".caret").css('margin', '8px 0');
            $(".caret").css('cssclass', 'form-control');

        });
    </script>
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $('[id*=ListDirectiveType]').multiselect({

                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                includeSelectAllOption: true,
                disableIfEmpty: true,
                maxHeight: 180,
                nonSelectedText: 'Directive',
                selectAllJustVisible: false,
                buttonWidth: '185px',
                buttonHeight: '120px',
                allSelectedText: 'Directive',
                nSelectedText: 'Directive'

            });
            $(".caret").css('float', 'right');
            $(".caret").css('margin', '8px 0');
        });
    </script>
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $('[id*=ListInspectionType]').multiselect({

                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                includeSelectAllOption: true,
                disableIfEmpty: true,
                maxHeight: 180,
                nonSelectedText: 'Inspection',
                selectAllJustVisible: false,
                buttonWidth: '185px',
                allSelectedText: 'Inspection',
                nSelectedText: 'Inspection'


            });
            $(".caret").css('float', 'right');
            $(".caret").css('margin', '8px 0');
        });
    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

            RegNoMultiSelect();
            //            $("#hdnAircraft").click();
        });
    </script>
    <script type="text/javascript">

        //  Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
        function RegNoMultiSelect() {
            $('[id*=ListRegNo]').multiselect({
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                includeSelectAllOption: true,
                disableIfEmpty: true,
                maxHeight: 180,
                nonSelectedText: 'Aircraft',
                selectAllJustVisible: true,
                buttonWidth: '185px',
                allSelectedText: 'Aircraft',
                nSelectedText: 'Aircraft'

            });
            $(".caret").css('float', 'right');
            $(".caret").css('margin', '8px 0');
            $(".caret").css('cssclass', 'form-control');

            // });
        }
    </script>
</body>
</html>
