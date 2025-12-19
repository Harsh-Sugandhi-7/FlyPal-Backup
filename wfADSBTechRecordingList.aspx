<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfADSBTechRecordingList.aspx.vb"
    Inherits="Flypal.wfADSBTechRecordingList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="FlyPal" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AD/SB Technical Recording</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <script language="javascript" type="text/javascript">

        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,toolbar=0;resizable=no,directories=no,location=no,width=auto,height=auto');

        }


    </script>
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script language="javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script src="jquery-1.11.1.min.js" type="text/javascript"></script>
    <link id="MainStyle" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    +
    <link href="images/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="StickyNote/css/style.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap.cosmo.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap.cosmo.min.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap-theme.css" rel="stylesheet" type="text/css" />
    <link href="Styles.css" id="Link1" type="text/css" rel="stylesheet" />
    <%--   <style>
    /*-- Table Striped CSS --*/
    table > tbody > tr:nth-of-type(odd) {
        background-color: #ddd;
    }
    </style>--%>
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
                                <td>
                                    <asp:UpdatePanel ID="upnltitle" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <span id="lbltitle" runat="server" style="font-size: 16px; font-weight: 100;" class="text-text-primary clstitle1">
                                                AD/SB Recording List</span>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlError" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:ValidationSummary ID="Validationsummary" runat="server" CssClass="text-danger clsValidationSummary"
                                                ValidationGroup="a" HeaderText="Fill Up The Following Information"></asp:ValidationSummary>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlSearchCriteria" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%" style="margin-top: 5px; margin-left: 5px;">
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblADSBRecordingNo" runat="server" CssClass="control-label">AD/SB Recording No.</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:UpdatePanel ID="upnlADSBRecording" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:DropDownList ID="cmbADSBRecording" runat="server" CssClass="input-sm" DataTextField="Text"
                                                                                            DataValueField="Text">
                                                                                            <asp:ListItem Value="0">(ALL)</asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                            <td>
                                                                                <asp:UpdatePanel ID="upnlADSBRecordinglblNo" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:Label ID="lblNo" runat="server" CssClass="control-label" Style="margin-left: 50px;">No.</asp:Label>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                            <td>
                                                                                <asp:UpdatePanel ID="upnlADSBRecordingNo" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:TextBox ID="txtNo" runat="server" CssClass="input-sm" MaxLength="7" onchange="setattr(this);"
                                                                                            ToolTip="Enter Number">0</asp:TextBox>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblDate" runat="server" CssClass="control-label">Date</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:UpdatePanel ID="upnlDate" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <table style="margin-top: 5px;">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="cmbDate" runat="server" CssClass="input-sm" AutoPostBack="true">
                                                                                            <asp:ListItem Value="0">(ALL)</asp:ListItem>
                                                                                            <asp:ListItem Value="1">Last 1 Week</asp:ListItem>
                                                                                            <asp:ListItem Value="2">Last 1 Month</asp:ListItem>
                                                                                            <asp:ListItem Value="3">Last 1 Quarter</asp:ListItem>
                                                                                            <asp:ListItem Value="4">Last 1 Year</asp:ListItem>
                                                                                            <asp:ListItem Value="5">Current Financial Year</asp:ListItem>
                                                                                            <asp:ListItem Value="6">Between Dates</asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblFromDate" runat="server" CssClass="control-label" Width="78px"
                                                                                            Style="margin-left: 5px;">From Date</asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtFromDate" runat="server" CausesValidation="true" ClientIDMode="Static"
                                                                                            CssClass="input-sm" onchange="ValidateDateText(this,'FromDate_watermarkextender');"
                                                                                            ValidationGroup="a" Width="100px"></asp:TextBox>
                                                                                        <cc2:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtFromDate">
                                                                                        </cc2:CalendarExtender>
                                                                                        <cc2:TextBoxWatermarkExtender ID="FromDate_watermarkextender" runat="server" ClientIDMode="Static"
                                                                                            TargetControlID="txtFromDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                                                        </cc2:TextBoxWatermarkExtender>
                                                                                        <asp:CustomValidator ID="cvFromDate" runat="server" CssClass="clsLabelAuto" Display="None"
                                                                                            ClientValidationFunction="BetweenDatesValidation" ValidationGroup="a" ErrorMessage="From Date should not be grater than To Date "></asp:CustomValidator>
                                                                                    </td>
                                                                                    <td align="right">
                                                                                        &nbsp;&nbsp;
                                                                                        <asp:Label ID="lblToDate" runat="server" CssClass="control-label" DESIGNTIMEDRAGDROP="19"
                                                                                            Width="78px">To Date </asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtToDate" runat="server" CausesValidation="true" ClientIDMode="Static"
                                                                                            Style="margin-left: 5px;" CssClass="input-sm" onchange="ValidateDateText(this,'ToDate_watermarkextender');"
                                                                                            ValidationGroup="a" Width="100px"></asp:TextBox>
                                                                                        <cc2:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtToDate">
                                                                                        </cc2:CalendarExtender>
                                                                                        <cc2:TextBoxWatermarkExtender ID="ToDate_watermarkextender" runat="server" ClientIDMode="Static"
                                                                                            TargetControlID="txtToDate" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                                                        </cc2:TextBoxWatermarkExtender>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="right" valign="top">
                                                        <asp:UpdatePanel ID="upnlFindNow" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table id="Table3" border="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnFindNow" runat="server" CssClass="btn btn-sm" TabIndex="0" ValidationGroup="a"
                                                                                Text="Find Now" ToolTip="Click to Find records" Style="height: 100%; border-color: black;
                                                                                border-top-left-radius: 4px; border-top-right-radius: 4px; border-bottom-left-radius: 4px;
                                                                                border-bottom-right-radius: 4px; margin-right: 8px;" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
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
                                <td>
                                    <asp:Panel ID="ClpnlAdvancedSearch" runat="server" Style="margin-top: 5px;" class="clsCollapsePnl">
                                        <div style="float: left; vertical-align: middle; width: 100%; margin-left: 5px">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <span style="vertical-align: middle; margin-left: 2px; width: 100%" id="lblMastersSelection"
                                                            class="control-label">More Search</span>
                                                    </td>
                                                    <td align="right">
                                                        <div style="float: right; vertical-align: middle; margin-right: 5px; width: 100%">
                                                            <image id="imgMasters" src="images/collapse_blue.jpg" alternatetext="(Show Details...)" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Panel ID="pnlAdvancedSearch" runat="server" Style="max-height: 200px; overflow-y: auto;
                                        margin-top: 5px; overflow: auto; overflow-x: hidden;">
                                        <table style="width: 100%; margin-left: 5px">
                                            <tr>
                                                <td>
                                                    <span id="lblStatus" class="control-label" runat="server">Status</span>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="upnlStatus" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="cmbStatus" runat="server" CssClass="input-sm" DataValueField="ID"
                                                                DataTextField="Name">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <span id="lblADSBNumber" class="control-label">AD/SB No</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtADSBNumber" runat="server" CssClass="input-sm" ToolTip="Enter Number"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSubject" runat="server" CssClass="control-label">AD/SB Subject</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSubject" runat="server" CssClass="input-sm" ToolTip="Enter Subject"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%" style="margin-top: 15px; margin-left: 5px">
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel ID="upnlResult" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Label ID="lblResult" runat="server" CssClass="control-label">List of ADSBRecording as per criteria :  Record(s) found.</asp:Label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnApplicability1" runat="server" CssClass="btn btn-primary btn-xs"
                                                            Enabled="false" Text="1" UseSubmitBehavior="false" ToolTip="Applicability" Style="border-top-left-radius: 7px;
                                                            border-top-right-radius: 7px; border-bottom-left-radius: 7px; border-bottom-right-radius: 7px;" />
                                                        <span id="lblApplicabilityButton" class="control-label" runat="server">: Applicability</span>
                                                        &nbsp;
                                                        <asp:Button ID="btnReview1" runat="server" CssClass="btn btn-primary btn-xs" Text="2"
                                                            Enabled="false" UseSubmitBehavior="false" ToolTip="Meeting Review" Style="border-top-left-radius: 7px;
                                                            border-top-right-radius: 7px; border-bottom-left-radius: 7px; border-bottom-right-radius: 7px;" />
                                                        <span id="lblMeetingReviewButton" class="control-label" runat="server">: Meeting Review</span>&nbsp;
                                                        <asp:Button ID="btnPlanned1" runat="server" CommandName="Planned" CssClass="btn btn-primary btn-xs"
                                                            Enabled="false" Text="3" UseSubmitBehavior="false" ToolTip="Planned" Style="border-top-left-radius: 7px;
                                                            border-top-right-radius: 7px; border-bottom-left-radius: 7px; border-bottom-right-radius: 7px;" />
                                                        <span id="lblPlannedButton" class="control-label" runat="server">: Planned</span>&nbsp;
                                                        <asp:Button ID="btnMonitoring1" runat="server" ToolTip="Monitoring" CssClass="btn btn-primary btn-xs"
                                                            Enabled="false" Text="4" UseSubmitBehavior="false" Style="border-top-left-radius: 7px;
                                                            border-top-right-radius: 7px; border-bottom-left-radius: 7px; border-bottom-right-radius: 7px;" />
                                                        <span id="lblMonitoringButton" class="control-label" runat="server">: Monitoring</span>&nbsp;
                                                        <asp:Button ID="btnSamplingClosure1" runat="server" ToolTip="Sampling/Closure" CssClass="btn btn-primary btn-xs"
                                                            Enabled="false" Text="5" UseSubmitBehavior="false" Style="border-top-left-radius: 7px;
                                                            border-top-right-radius: 7px; border-bottom-left-radius: 7px; border-bottom-right-radius: 7px;"
                                                            Visible="false" />
                                                        <span id="Span5" class="control-label"></span>&nbsp;
                                                    </td>
                                                    <td colspan="1" align="right">
                                                        <asp:UpdatePanel ID="upnlActionBtnTop" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnAddNewTop" runat="server" Text="Add New" ToolTip="Click to Add New"
                                                                                CssClass="btn btn-sm" Style="border-color: black; border-top-left-radius: 4px;
                                                                                border-top-right-radius: 4px; border-bottom-left-radius: 4px; border-bottom-right-radius: 4px;
                                                                                margin-right: 3px;"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnPrintTop" runat="server" CssClass="btn btn-sm" ToolTip="Click to Print the list of ADSBRecording"
                                                                                Visible="false" Text="Print" CausesValidation="True" Style="height: 100%; border-color: black;
                                                                                border-top-left-radius: 4px; border-top-right-radius: 4px; border-bottom-left-radius: 4px;
                                                                                border-bottom-right-radius: 4px; margin-right: 3px;"></asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnExcelTop" runat="server" CssClass="btn btn-sm" ValidationGroup="1"
                                                                                Visible="false" Width="100px" Text="Export To Excel" ToolTip="Click to Export"
                                                                                Style="height: 100%; border-color: black; border-top-left-radius: 4px; border-top-right-radius: 4px;
                                                                                border-bottom-left-radius: 4px; border-bottom-right-radius: 4px; margin-right: 3px;" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnCloseTop" runat="server" CssClass="btn btn-sm" ToolTip="Click to close List of ADSBRecording screen"
                                                                                Text="Close" CausesValidation="False" Style="height: 100%; border-color: black;
                                                                                border-top-left-radius: 4px; border-top-right-radius: 4px; border-bottom-left-radius: 4px;
                                                                                border-bottom-right-radius: 4px; margin-right: 8px;"></asp:Button>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <div class="row">
                                                            <div class="col-lg-12" style="width: 99%">
                                                                <div class="table-responsive">
                                                                    <asp:UpdatePanel ID="upnlGridView" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:GridView ID="dgADSBRecordingList" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                                                GridLines="Both" DataKeyNames="ID" EnableViewState="True" ShowHeaderWhenEmpty="True"
                                                                                CssClass="table table-striped table-bordered table-hover" PageSize="25" AllowPaging="true"
                                                                                Style="margin-right: 8px; margin-top: 5px;">
                                                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" />
                                                                                <PagerStyle HorizontalAlign="Right" CssClass="paging" />
                                                                                <RowStyle CssClass="table table-striped table-bordered table-hover" />
                                                                                <HeaderStyle CssClass="clsdgHeader" />
                                                                                <AlternatingRowStyle CssClass="table table-striped table-bordered table-hover" />
                                                                                <Columns>
                                                                                    <%--0--%>
                                                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                                                    <%--1--%>
                                                                                    <asp:BoundField DataField="ADSBDateFormatted" HeaderText="Date">
                                                                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                                                            Font-Underline="False" Wrap="False" />
                                                                                        <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                                    </asp:BoundField>
                                                                                    <%--2--%>
                                                                                    <asp:BoundField DataField="ADSBRecordingText" HeaderText="AD/SB Recording" SortExpression="ADSBRecordingText">
                                                                                        <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemStyle Wrap="false" />
                                                                                    </asp:BoundField>
                                                                                    <%--3--%>
                                                                                    <asp:TemplateField HeaderText="AD/SB No" HeaderStyle-ForeColor="black">
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton runat="server" ID="LnkSubject" CommandArgument='<%# Eval("ID") %>'
                                                                                                CommandName="EditRec" Text='<%# Eval("ADSBNo") %>' Font-Size="13px" Font-Bold="true" />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                    </asp:TemplateField>
                                                                                    <%--4--%>
                                                                                    <asp:BoundField DataField="ADSBSubject" HeaderText="Subject" SortExpression="ADSBSubject">
                                                                                        <HeaderStyle ForeColor="black" HorizontalAlign="Left"></HeaderStyle>
                                                                                        <ItemStyle Wrap="true" />
                                                                                    </asp:BoundField>
                                                                                    <%--5--%>
                                                                                    <asp:TemplateField HeaderText="">
                                                                                        <ItemTemplate>
                                                                                            <asp:Button ID="btnApplicability" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                                Enabled='<%#  Eval("ADSBStepsID")>=1 and Eval("StatusID") = 2  %>' CommandName="Applicability"
                                                                                                CssClass="btn btn-primary btn-xs" Text="1" UseSubmitBehavior="false" ToolTip="Applicability"
                                                                                                Style="border-top-left-radius: 7px; border-top-right-radius: 7px; border-bottom-left-radius: 7px;
                                                                                                border-bottom-right-radius: 7px;" />
                                                                                            <asp:Button ID="btnReview" runat="server" CommandArgument='<%# Eval("ID") %>' Enabled='<%#  Eval("ADSBStepsID")>=2 and Eval("StatusID") = 2 and Eval("IsMeetingRequired")=1   %>'
                                                                                                CommandName="Review" CssClass="btn btn-primary btn-xs" Text="2" UseSubmitBehavior="false"
                                                                                                ToolTip="Meeting Review" Style="border-top-left-radius: 7px; border-top-right-radius: 7px;
                                                                                                border-bottom-left-radius: 7px; border-bottom-right-radius: 7px;" />
                                                                                            <asp:Button ID="btnPlanned" runat="server" CommandArgument='<%# Eval("ID") %>' Enabled='<%# Eval("ADSBStepsID")>=3 and Eval("StatusID") = 2  and Eval("IsMeetingRequired")=1 %>'
                                                                                                CommandName="Planned" CssClass="btn btn-primary btn-xs" Text="3" UseSubmitBehavior="false"
                                                                                                ToolTip="Planned" Style="border-top-left-radius: 7px; border-top-right-radius: 7px;
                                                                                                border-bottom-left-radius: 7px; border-bottom-right-radius: 7px;" />
                                                                                            <asp:Button ID="btnMonitoring" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                                Enabled='<%#  Eval("ADSBStepsID")>=4 and Eval("StatusID") = 2 and Eval("IsMeetingApprovedByAll") = 1 and Eval("IsMeetingRequired")=1 %>'
                                                                                                CommandName="Monitoring" ToolTip="Monitoring" CssClass="btn btn-primary btn-xs"
                                                                                                Text="4" UseSubmitBehavior="false" Style="border-top-left-radius: 7px; border-top-right-radius: 7px;
                                                                                                border-bottom-left-radius: 7px; border-bottom-right-radius: 7px;" />
                                                                                            <asp:Button ID="btnSamplingClosure" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                                Visible="false" Enabled='<%#  Eval("StatusID") = 2 %>' CommandName="SamplingClosure"
                                                                                                ToolTip="Sampling/Closure" CssClass="btn btn-primary btn-xs" Text="5" UseSubmitBehavior="false"
                                                                                                Style="border-top-left-radius: 7px; border-top-right-radius: 7px; border-bottom-left-radius: 7px;
                                                                                                border-bottom-right-radius: 7px;" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <%--6--%>
                                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center"
                                                                                        HeaderStyle-ForeColor="black">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="Delete" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="DeleteRec"
                                                                                                Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png" />
                                                                                        </ItemTemplate>
                                                                                        <%--Visible='<%#  Eval("ADSBStepsID") <=2 %>'--%>
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <%--7--%>
                                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Approval" HeaderStyle-HorizontalAlign="Center"
                                                                                        HeaderStyle-ForeColor="black">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton ID="ApprovedStatus" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                                                                CommandName="Approval" Style="height: 30px; width: 30px" ImageUrl="~/images/approved.png" />
                                                                                            <%--https://www.flaticon.com/free-icons/approval--%>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <%--8--%>
                                                                                    <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                        DataField="ADSBStepsID" HeaderText="ADSBSteps"></asp:BoundField>
                                                                                    <%--9--%>
                                                                                    <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                        DataField="StatusID" HeaderText="StatusID"></asp:BoundField>
                                                                                    <%--10--%>
                                                                                    <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                        DataField="Applicability" HeaderText="Applicability"></asp:BoundField>
                                                                                    <%--10--%>
                                                                                    <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                        DataField="IsMeetingRequired" HeaderText="IsMeetingRequired">
                                                                                    </asp:BoundField>
                                                                                    <%--11--%>
                                                                                    <asp:BoundField HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                                                        DataField="IsMeetingApprovedByAll" HeaderText="IsMeetingApprovedByAll"></asp:BoundField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="right">
                                                        <asp:UpdatePanel ID="upnlActionBtnBottom" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnAddNew" runat="server" CssClass="btn btn-sm" ToolTip="Click to Add New ADSBRecording"
                                                                                Text="Add New" CausesValidation="False" Style="height: 100%; margin-top: -20px;
                                                                                border-color: black; border-top-left-radius: 4px; border-top-right-radius: 4px;
                                                                                border-bottom-left-radius: 4px; border-bottom-right-radius: 4px; margin-right: 3px;">
                                                                            </asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-sm" ToolTip="Click to Print the list of ADSBRecording"
                                                                                Visible="false" Text="Print" CausesValidation="True" Style="height: 100%; margin-top: -20px;
                                                                                border-color: black; border-top-left-radius: 4px; border-top-right-radius: 4px;
                                                                                border-bottom-left-radius: 4px; border-bottom-right-radius: 4px; margin-right: 3px;">
                                                                            </asp:Button>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnExcel" runat="server" CssClass="btn btn-sm" ValidationGroup="1"
                                                                                Visible="false" Width="100px" Text="Export To Excel" ToolTip="Click to Export"
                                                                                Style="height: 100%; margin-top: -20px; border-color: black; border-top-left-radius: 4px;
                                                                                border-top-right-radius: 4px; border-bottom-left-radius: 4px; border-bottom-right-radius: 4px;
                                                                                margin-right: 3px;" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnClose" runat="server" CssClass="btn btn-sm" ToolTip="Click to close List of ADSBRecording  screen"
                                                                                Text="Close" CausesValidation="False" Style="height: 100%; margin-top: -20px;
                                                                                border-color: black; border-top-left-radius: 4px; border-top-right-radius: 4px;
                                                                                border-bottom-left-radius: 4px; border-bottom-right-radius: 4px; margin-right: 8px;">
                                                                            </asp:Button>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
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
        <cc2:CollapsiblePanelExtender BehaviorID="clpMastersBehaviour" ID="clpAdvancedSearch"
            ClientIDMode="Static" runat="Server" TargetControlID="pnlAdvancedSearch" ExpandControlID="ClpnlAdvancedSearch"
            CollapseControlID="ClpnlAdvancedSearch" Collapsed="True" ImageControlID="imgMasters"
            CollapsedSize="0" ExpandedText="(Hide Details...)" CollapsedText="(Show Details...)"
            ExpandedImage="~/images/collapse_blue.jpg" CollapsedImage="~/images/expand_blue.jpg"
            SuppressPostBack="false" />
        <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" DynamicLayout="false" runat="server">
            <ProgressTemplate>
                <div class="clsAjaxLoader" style="height: 100%; width: 100%; left: 0; position: fixed;
                    background-color: #000000; top: 0; z-index: 99999;">
                </div>
                <div style="position: fixed; top: 50%; left: 50%; margin-left: -27px; margin-top: -27px;
                    z-index: 100000;">
                    <div class="ext-el-mask-msg x-mask-loading">
                        <div class="clsLoad_ajax">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Loader.gif" ImageAlign="Middle" />
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
    <script type="text/javascript">
        $(document).keypress(function (e) {
            if (e.which == 13) {
                $("input[id=btnFindNow]").click();
            }
        });
    </script>
    <script type="text/javascript">
        function FireOnClickButton(e) {
            if (e.keyCode == 13 || e.keyCode == 9) {
                document.getElementById("btnFindNow").click();
            }
        }
    </script>
    </form>
</body>
</html>
