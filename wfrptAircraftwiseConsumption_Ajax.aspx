<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfrptAircraftwiseConsumption_Ajax.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfrptAircraftwiseConsumption_Ajax" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Part Information</title>
    <script type="text/jscript" language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/font-awesome@4.7.0/css/font-awesome.min.css" />
    <%-- Ajay 08-Nov-2022--%>
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
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
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <style type="text/css">
        .clsCollapsePnl {
            background: url("css/img/BGLink.png") repeat-x #ccc;
            font-family: Verdana; /*font-size: 14pt; */
            font-size: 12pt;
            color: White;
            font-weight: 500;
            width: 100%;
            display: inline-block;
            border: 1px solid #ccc;
        }
    </style>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
    <form id="wfgroup" method="post" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <table class="clstablelistout" id="tblmain" border="0">
            <tr>
                <td>
                    <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                        <table id="tblInner" class="clstablelistin" border="0">
                            <tr>
                                <td colspan="3">
                                    <table width="100%">
                                        <tr>

                                            <td class="clsFormHeader1">
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <span id="lbltitle" class="clsFormHeader" style="width: 100%">Aircraftwise Consumption Report</span>
                                                        </td>
                                                        <td align="right" colspan="3">
                                                            <%--<asp:UpdatePanel ID="upnlActionBtns" runat="server" UpdateMode="Conditional ">
                                                                <ContentTemplate>
                                                                    <asp:Panel ID="pnlButton" runat="server" CssClass="clspanel1">
                                                                        <table border="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Button ID="btnCurrentSearchCriteria" runat="server" ClientIDMode="Static" CssClass="clsbtnH clsinfoH"
                                                                                        TabIndex="0" Text="Current Criteria" ToolTip="Click to display current searching criterias" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Button ID="btnExport" runat="server" ClientIDMode="Static" CssClass="clsbtnH clsinfoH"
                                                                                        Visible="<%$AppSettings:ShowExportToExcelButton%>" TabIndex="0" Text="Export to Excel"
                                                                                        ToolTip="Click to Export report" Width="140px" ValidationGroup="a" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Button ID="btnDisplay" runat="server" ClientIDMode="Static" CssClass="clsbtnH clsinfoH"
                                                                                        TabIndex="0" Text="Display" ToolTip="Click to display report" ValidationGroup="a" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Button ID="btnByMail" runat="server" CssClass="clsbtnH clsinfoH" Text="Report By Mail"
                                                                                        ValidationGroup="a" ToolTip="Click to report by mail" Width="140px" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH clsinfoH"
                                                                                        TabIndex="0" Text="Close" ToolTip="Click to Close Aircraft Consumption Report screen" />
                                                                                </td>
                                                                                <td>
                                                                                    <%--Ajay 08-Nov-2022 
                                                                                    <asp:Button ID="hdnBtnMarkFav" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
                                                                                        Style="display: none;"></asp:Button>
                                                                                    <asp:Button ID="hdnBtnRemoveFav" ClientIDMode="Static" runat="server" Text="----"
                                                                                        CausesValidation="False" Style="display: none;"></asp:Button>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td style="width: 1%" align="center">
                                                <span id="FavClk"><i id="FavIClk" runat="server" onclick="FunctionFav(this)" style="font-size: 21px; color: black; border: black; cursor: pointer"
                                                    class="fa fa-star fa-spin fa-5x circle-icon"
                                                    title="Mark As Favourites"></i>
                                                    <%--  Ajay 07-Nov-2022--%>
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                        HeaderText="Fill Up The Following Fields" ValidationGroup="a"></asp:ValidationSummary>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <span id="Span2" class="clsLabelHeader">Step I. Selection for Details Or Graph Report</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="Span3" class="clsLabelAuto">Type</span>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upnlType" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rbDetails" runat="server" CssClass="clsRadioButton" GroupName="b"
                                                            Checked="true" Text="Details" />
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rbGraph" runat="server" CssClass="clsRadioButton" GroupName="b"
                                                            Text="Graph" />
                                                    </td>
                                                    <td></td>
                                                    <td style="padding-left: 3px;">&nbsp;
                                                    </td>
                                                    <td style="padding-left: 3px;"></td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <span id="lblStep1" class="clsLabelHeader">Step II. Selection of Date</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span id="lblDateRange" class="clsLabelAuto">Date Range</span>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upnlDateCriteria" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbDateRange" runat="server" AutoPostBack="True">
                                                            <asp:ListItem Value="(All)">(All)</asp:ListItem>
                                                            <asp:ListItem Value="Last Week">Last Week</asp:ListItem>
                                                            <asp:ListItem Value="Last Month">Last Month</asp:ListItem>
                                                            <asp:ListItem Value="Last Quarter">Last Quarter</asp:ListItem>
                                                            <asp:ListItem Value="Last Year">Last Year</asp:ListItem>
                                                            <asp:ListItem Value="Current Financial Year">Current Financial Year</asp:ListItem>
                                                            <asp:ListItem Value="Between Dates">Between Dates</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="padding-left: 3px;">
                                                        <asp:Label ID="lblFromDate" runat="server" CssClass="clsLabelAuto" Visible="False">From</asp:Label>
                                                    </td>
                                                    <td style="padding-left: 3px;">
                                                        <asp:TextBox ID="txtFromDate" runat="server" CausesValidation="true" ClientIDMode="Static"
                                                            CssClass="clsTextBoxTagDateSearch" onchange="ValidateDateText(this);"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="calFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate"></cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ID="Calender_watermarkextender" runat="server" TargetControlID="txtFromDate"
                                                            WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                        <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" CssClass="clsLabelAuto"
                                                            ErrorMessage="From Date Required" ControlToValidate="txtFromDate" Display="None"
                                                            ValidationGroup="a"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td style="padding-left: 3px;">
                                                        <asp:Label ID="lblToDate" runat="server" CssClass="clsLabelAuto" Visible="False">To</asp:Label>
                                                    </td>
                                                    <td style="padding-left: 3px;">
                                                        <asp:TextBox ID="txtToDate" runat="server" CausesValidation="true" ClientIDMode="Static"
                                                            CssClass="clsTextBoxTagDateSearch" onchange="ValidateDateText(this);"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="calToDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate"></cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtToDate"
                                                            WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>"></cc2:TextBoxWatermarkExtender>
                                                        <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtToDate"
                                                            CssClass="clsLabelAuto" Display="None" ErrorMessage="To Date Required" ValidationGroup="a"></asp:RequiredFieldValidator>
                                                        <asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
                                                            ClientValidationFunction="BetweenDatesValidation" ValidationGroup="a" ErrorMessage="From Date should not be greater than To Date "></asp:CustomValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    <span id="lblStep2" class="clsLabelHeader">Step III. Selection of Aircraft</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr class="clsCollapsePnl">
                                            <td width="25px">
                                                <input style="vertical-align: bottom;" type="checkbox" id="chkSelectAllAircraft" />
                                            </td>
                                            <td width="100%">
                                                <asp:Panel ID="ClpnlAircraftList" runat="server" CssClass="clsCollapsePnl" Style="border: none;">
                                                    <div>
                                                        <div style="float: left; vertical-align: middle;">
                                                            <span id="lblAicraftlist" class="clsLabelHeader" style="vertical-align: middle; margin-left: 2px;">Aircraft List</span>
                                                        </div>
                                                        <div style="float: right; vertical-align: middle; margin-right: 5px;">
                                                            <image id="imgbtnClpnl" alternatetext="(Show Details...)" src="images/collapse_blue.jpg" />
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Panel ID="pnlAircraftlist" runat="server" ClientIDMode="Static" Visible="true">
                                                    <table id="Table1" border="0" width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBoxList ID="ChklistAircraft" runat="server" ClientIDMode="Static" CssClass="clsComboBox_Ajax"
                                                                    DataTextField="RegNo" DataValueField="ID" EnableViewState="false" RepeatColumns="4"
                                                                    RepeatDirection="Horizontal" Width="100%">
                                                                </asp:CheckBoxList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                    <cc2:CollapsiblePanelExtender ID="clpAircraftlist" runat="Server" BehaviorID="clpAircraftlistBehaviour"
                                        ClientIDMode="Static" CollapseControlID="ClpnlAircraftList" Collapsed="True"
                                        CollapsedImage="~/images/expand_blue.jpg" CollapsedText="(Show Details...)" ExpandControlID="ClpnlAircraftList"
                                        ExpandedImage="~/images/collapse_blue.jpg" ExpandedText="(Hide Details...)" ImageControlID="imgbtnClpnl"
                                        SkinID="CollapsiblePanelDemo" SuppressPostBack="false" TargetControlID="pnlAircraftlist" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="3">
                                    <span id="lblStep3" class="clsLabelHeader">Step IV. Selection of Category</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <span id="lblCategory" class="clsLabelAuto">Category</span>
                                </td>
                                <td align="left">
                                    <asp:DropDownList CssClass="clsTextBoxTagSearchComboNewstyle" ID="cmbCategory" runat="server" DataTextField="Name"
                                        DataValueField="ID">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="3">
                                    <span id="Label3" class="clsLabelHeader">Step V. Selection of Valued Store</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left"></td>
                                <td align="left" colspan="2">
                                    <asp:CheckBox ID="chkIsValued" runat="server" Checked="True" CssClass="clsCheckBox"
                                        Text="Include Valued Stores Only" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="3">
                                    <span id="Span1" class="clsLabelHeader">Step VI. Selection to show only valued parts
                                    with landing rate greater than entered value </span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">&nbsp;
                                </td>
                                <td align="left" colspan="2">
                                    <asp:UpdatePanel runat="server" ID="upnlHighValue" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:CheckBox ID="chkHighValue" runat="server" CssClass="clsCheckBox" Text="Show only valued parts with landing rate greater than "
                                                AutoPostBack="true" />
                                            <asp:TextBox ID="txtCEffectiveRate" runat="server" CssClass="clsTextBoxTagSearch"
                                                Enabled="false" MaxLength="12"></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="3">
                                    <span id="lblStep4" class="clsLabelHeader">Step VII. Selection of Part Number</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <span id="lblSearch" class="clsLabelAuto">Search</span>
                                </td>
                                <td align="left" colspan="2">
                                    <asp:TextBox CssClass="clsTextBoxTagSearch" ID="txtSearch" runat="server" Width="520px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    <span id="lblStep5" class="clsLabelHeader">Step VIII. Display Report</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    <span id="lblSummary" class="clsLabelAuto">Your selection is as follows </span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:UpdatePanel runat="server" ID="upnlSearchingCriteria" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDateRangeFrom" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="3">
                                                        <asp:Label ID="lblAircraftName" runat="server" CssClass="clsLabelAuto" Width="50%"
                                                            Visible="False"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblCategoryName" runat="server" CssClass="clsLabelAuto" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="3">
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
                                <td align="right" colspan="3">
                                <asp:UpdatePanel ID="upnlActionBtns" runat="server" UpdateMode="Conditional ">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlButton" runat="server" CssClass="clspanel1">
                                            <table border="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnCurrentSearchCriteria" runat="server" ClientIDMode="Static" CssClass="clsbtnH"
                                                            TabIndex="0" Text="Current Criteria" ToolTip="Click to display current searching criterias" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnExport" runat="server" ClientIDMode="Static" CssClass="clsbtnH"
                                                            Visible="<%$AppSettings:ShowExportToExcelButton%>" TabIndex="0" Text="Export to Excel"
                                                            ToolTip="Click to Export report" Width="140px" ValidationGroup="a" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnDisplay" runat="server" ClientIDMode="Static" CssClass="clsbtnH"
                                                            TabIndex="0" Text="Display" ToolTip="Click to display report" ValidationGroup="a" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnByMail" runat="server" CssClass="clsbtnH" Text="Report By Mail"
                                                            ValidationGroup="a" ToolTip="Click to report by mail" Width="140px" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsbtnH"
                                                            TabIndex="0" Text="Close" ToolTip="Click to Close Aircraft Consumption Report screen" />
                                                    </td>
                                                    <td>
                                                        <%--<%--Ajay 08-Nov-2022 --%>
                                                        <asp:Button ID="hdnBtnMarkFav" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
                                                            Style="display: none;"></asp:Button>
                                                        <asp:Button ID="hdnBtnRemoveFav" ClientIDMode="Static" runat="server" Text="----"
                                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
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
        <asp:HiddenField ID="hdnAircraftList" runat="server" ClientIDMode="Static" />
        <script type="text/javascript">
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                $("#<%=txtSearch.ClientID %>").autocomplete('wfAutoItemList.aspx?', {
                    width: 520,
                    autoFill: false,
                    matchContains: true,
                    delay: 0
                });

            });
        </script>
        <script type="text/javascript">

            //check all/ uncheck all checkbox of aircraft list
            $(document).ready(function () {

                $("#chkSelectAllAircraft").click(function () {
                    var status = $("#chkSelectAllAircraft").attr("checked");
                    $("#ChklistAircraft").find(":checkbox").each(function () {
                        if (status == "checked") {
                            $(this).attr("checked", status);
                        }
                        else {
                            $(this).removeAttr("checked");
                        }

                    });
                });

                $("#btnExport,#btnDisplay,#btnCurrentSearchCriteria,#btnByMail,hdnimgBtnSendMail").live('click', function () {
                    try {
                        SetSelectedAircrafts();
                    } catch (e) {
                        alert(e.Message);
                    }
                    return true;
                });
            });
            //set aircraft list text(i.e. aircraft name) to hidden field to access from code behind
            function SetSelectedAircrafts() {
                var aircraftlist = new Array();
                $("#ChklistAircraft :checked").each(function (i) {
                    aircraftlist.push($(this).next().text());
                });

                $("#hdnAircraftList").val('');
                $("#hdnAircraftList").val(aircraftlist);
            }

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
        <!--Ajay S 08-Nov-2022 -->
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
</body>
</html>
