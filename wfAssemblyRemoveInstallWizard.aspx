<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfAssemblyRemoveInstallWizard.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfAssemblyRemoveInstallWizard" %>

<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBoxNew" Src="MSGBoxNew.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <meta name="viewport" content="width=device-width, initial-scale=1,shrink-to-fit=no" />
    <title></title>
    <link id="MainStyle" type="text/css" rel="stylesheet" href="Styles.css" />
    <link rel="stylesheet" type="text/css" href="popup.css" />
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <!-- CSS -->
    <link rel="stylesheet" href="wizard/css/style.css" />
    <link rel="stylesheet" href="http://fonts.googleapis.com/css?family=Roboto:400,100,300,500" />
    <link rel="stylesheet" href="wizard/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="wizard/font-awesome/css/font-awesome.min.css" />
    <link rel="stylesheet" href="wizard/css/form-elements.css" />
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
            <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
            <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
        <![endif]-->
    <!-- Favicon and touch icons -->
    <link rel="shortcut icon" href="wizard/ico/favicon.png" />
    <link rel="apple-touch-icon-precomposed" sizes="144x144" href="wizard/ico/apple-touch-icon-144-precomposed.png" />
    <link rel="apple-touch-icon-precomposed" sizes="114x114" href="wizard/ico/apple-touch-icon-114-precomposed.png" />
    <link rel="apple-touch-icon-precomposed" sizes="72x72" href="wizard/ico/apple-touch-icon-72-precomposed.png" />
    <link rel="apple-touch-icon-precomposed" href="wizard/ico/apple-touch-icon-57-precomposed.png" />
    <style type="text/css">
        .clsCursorStyle
        {
            cursor: pointer;
        }
    </style>
    <script type="text/javascript" src="wizard/js/jquery-1.11.1.min.js"></script>
    <%--  <script src="JQGridReq/jquery/1.8.1/jquery.js" type="text/javascript"></script>--%>
    <script type="text/javascript" src="wizard/bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="wizard/js/jquery.backstretch.min.js"></script>
    <script type="text/javascript" src="wizard/js/retina-1.1.0.min.js"></script>
    <script type="text/javascript" src="wizard/js/scripts.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <%-- <script type="text/javascript" src='http://cdnjs.cloudflare.com/ajax/libs/jquery/2.1.3/jquery.min.js'></script>--%>
    <script type="text/javascript" src="wizard/js/UploadFile.js"></script>
    <style type="text/css">
        .textheight
        {
            height: 34px;
            font-size: 8pt;
        }
    </style>
    <style type="text/css">
        .ajax__calendar
        {
            z-index: 5000;
        }
    </style>
    <script type="text/javascript">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
    <style type="text/css">
        .fileUpload1 input
        {
            position: absolute;
            top: 0;
            right: 0;
            margin: 0;
            padding: 0;
            font-size: 20px;
            cursor: pointer;
            opacity: 0;
            filter: alpha(opacity=0);
            width: 90px;
        }
        .uploadbtn
        {
            background: #E7E7E7;
            color: black;
            font-weight: 500;
            width: 90px;
            border: 1px solid gray;
            text-align: center;
            font-family: Calibri;
            font-size: 11pt;
            width: 90px;
            border: 1px solid gray;
            position: relative;
        }
    </style>
</head>
<body style="background-color: #eee">
    <div class="row">
        <div class="col-sm-10 col-sm-offset-1 col-md-8 col-md-offset-2 col-lg-6 col-lg-offset-3 form-box">
            <a href="Dashboard.aspx" class="close">
                <img src="images/remove.jpg" style="position: relative; margin: -58 -57 0 782; height: 30px;
                    width: 30px" class="btn_close" title="Close" alt="Close" /></a>
            <form role="form" action="#" method="post" class="f1" runat="server" enctype="multipart/form-data">
            <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
                ScriptMode="Debug" EnablePageMethods="true">
            </asp:ScriptManager>
            <h3>
                Navigate for Assembly Removal/Installation</h3>
            <div class="f1-steps">
                <div class="f1-progress">
                    <div class="f1-progress-line" data-now-value="16.66" data-number-of-steps="3" style="width: 16.66%;">
                    </div>
                </div>
                <div class="f1-step active">
                    <div class="f1-step-icon">
                        <i class="fa fa-user"></i>
                    </div>
                    <p>
                        Removal</p>
                </div>
                <div class="f1-step">
                    <div class="f1-step-icon">
                        <i class="fa fa-key"></i>
                    </div>
                    <p>
                        Installation</p>
                </div>
                <div class="f1-step">
                    <div class="f1-step-icon">
                        <i class="fa fa-file-archive-o"></i>
                    </div>
                    <p>
                        Logs Updation & Submit</p>
                </div>
            </div>
            <fieldset id="fld1">
                <h4>
                    Tell us details of Assembly to be removed :</h4>
                <div class="form-group">
                    <div class="controlplace">
                        <label>
                            Removal Date</label>
                        <%--   <input type="date" name="f1-last-name" placeholder="Removal Date..." class="f1-last-name form-control"
                            style="width: 195px;" id="f1-Rem-Date">--%>
                        <asp:TextBox runat="server" ID="calDate" CssClass="form-control textheight" AutoPostBack="false"
                            AutoComplete="off" Width="190px" onchange="ValidateDateText(this,'FromDate_watermarkextender');"
                            TabIndex="1"></asp:TextBox>
                        <cc2:CalendarExtender ID="calDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                            ClientIDMode="Static" Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="calDate">
                        </cc2:CalendarExtender>
                        <cc2:TextBoxWatermarkExtender TargetControlID="calDate" ID="FromDate_watermarkextender"
                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                            WatermarkCssClass="form-control textheight">
                        </cc2:TextBoxWatermarkExtender>
                    </div>
                    <div class="controlplace">
                        <asp:UpdatePanel ID="upnlAircraft" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <label>
                                    Aircraft</label>
                                <asp:DropDownList ID="cmbAircraft" runat="server" CssClass="f1-last-name form-control clsComboBox_Ajax"
                                    DataValueField="ID" DataTextField="RegNo" AutoPostBack="True">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <table width="80%">
                            <tr style="height: 15px">
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpnlInstalledAssemblyList" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgInstalledAssemblyList" runat="server" AutoGenerateColumns="False"
                                                ClientIDMode="Static" EmptyDataText="No data found." ShowHeader="true" GridLines="None"
                                                DataKeyNames="AssemblyStatusID" CssClass="table table-striped table-bordered table-hover"
                                                PagerSettings-Mode="NumericFirstLast" TabIndex="5">
                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left" />
                                                <RowStyle CssClass="clsdgItem" HorizontalAlign="Left" />
                                                <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black" />
                                                <Columns>
                                                    <asp:BoundField DataField="AssemblyStatusID" HeaderText="ID" Visible="False"></asp:BoundField>
                                                    <asp:BoundField DataField="MachineInfo" HeaderText="Reg No." SortExpression="MachineInfo">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AssemblyType" HeaderText="Assembly Type" SortExpression="AssemblyType">
                                                        <HeaderStyle HorizontalAlign="Left" Wrap="true" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ATACode" HeaderText="ATA" SortExpression="ATACode">
                                                        <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AssemblyInfo" HeaderText="Assembly Info." SortExpression="AssemblyInfo">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="InstalledOnFormatted" HeaderText="Installed On">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PeriodNameForweb" HeaderText="Period" SortExpression="PeriodNameForweb"
                                                        HtmlEncode="False">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ValueFormatted" HeaderText="Value" SortExpression="ValueFormatted"
                                                        HtmlEncode="False">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TSNFormatted" HeaderText="TSN" SortExpression="TSNFormatted"
                                                        HtmlEncode="False">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TSOFormatted" HeaderText="TSO" SortExpression="TSOFormatted"
                                                        HtmlEncode="False">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:ButtonField CommandName="DeleteRec" HeaderText="Remove" Text="Remove">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:ButtonField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <asp:UpdatePanel ID="upnlRembtn" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="f1-buttons">
                    <button type="button" id="btnRem" class="btn btn-next" runat="server">
                        Next</button>
                </div>
            </fieldset>
            <fieldset id="fld2">
                <h4>
                    Now Let's take Installation details</h4>
                <div class="form-group">
                    <div class="controlplace">
                        <label>
                            Installation Date</label>
                        <asp:UpdatePanel ID="upnlInstallationDate" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox runat="server" ID="txtInstallationDate" CssClass="clsTextBox_Ajax form-control textheight"
                                    Width="170px" AutoPostBack="true" onchange="ValidateDateText(this,'txtInstallationDate_watermarkextender');"></asp:TextBox>
                                <cc2:CalendarExtender ID="txtInstallationDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtInstallationDate">
                                </cc2:CalendarExtender>
                                <cc2:TextBoxWatermarkExtender TargetControlID="txtInstallationDate" ID="txtInstallationDate_watermarkextender"
                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                    WatermarkCssClass="form-control textheight">
                                </cc2:TextBoxWatermarkExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="f1-buttons">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:LinkButton ID="lnkInstallNew" runat="server" CssClass="btn" Style="background-color: #f35b3f"
                                    ValidationGroup="1" ForeColor="white" Text="Install" ToolTip="Click to Install new Assembly"></asp:LinkButton>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <table>
                            <tr style="height: 15px">
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpnlRemovedAssemblyList" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgRemovedAssemblyList" runat="server" AutoGenerateColumns="False"
                                                ClientIDMode="Static" EmptyDataText="No data found." ShowHeader="true" GridLines="None"
                                                DataKeyNames="AssemblyStatusID" CssClass="table table-striped table-bordered table-hover"
                                                PagerSettings-Mode="NumericFirstLast" TabIndex="5">
                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left" />
                                                <RowStyle CssClass="clsdgItem" HorizontalAlign="Left" />
                                                <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black" />
                                                <Columns>
                                                    <asp:BoundField DataField="AssemblyStatusID" HeaderText="ID" Visible="False"></asp:BoundField>
                                                    <asp:BoundField DataField="MachineInfo" HeaderText="Reg No." SortExpression="MachineInfo">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AssemblyType" HeaderText="Assembly Type" SortExpression="AssemblyType">
                                                        <HeaderStyle HorizontalAlign="Left" Wrap="true" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ATACode" HeaderText="ATA" SortExpression="ATACode">
                                                        <HeaderStyle HorizontalAlign="Left" Wrap="True" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AssemblyInfo" HeaderText="Assembly Info." SortExpression="AssemblyInfo">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="RemovedOnFormatted" HeaderText="Removed On">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="PeriodNameForweb" HeaderText="Period" SortExpression="PeriodNameForweb"
                                                        HtmlEncode="False">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ValueFormatted" HeaderText="Value" SortExpression="ValueFormatted"
                                                        HtmlEncode="False">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TSOFormatted" HeaderText="TSO" SortExpression="TSOFormatted"
                                                        HtmlEncode="False">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:ButtonField Text="Install Selected" ValidationGroup="1" CausesValidation="true"
                                                        HeaderText="Install Selected" CommandName="InstallSelected">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:ButtonField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="f1-buttons">
                    <button type="button" class="btn btn-previous">
                        Previous</button>
                    <button type="button" class="btn btn-next">
                        Next</button>
                </div>
            </fieldset>
            <fieldset id="fld3">
                <h4>
                    Now Let's see Log details</h4>
                <div class="form-group">
                    <div class="row">
                        <table>
                            <tr style="height: 15px">
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpnlLogList" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="dgLogList" runat="server" AutoGenerateColumns="False" EmptyDataText="No data found."
                                                ShowHeader="true" GridLines="None" CssClass="table table-striped table-bordered table-hover"
                                                PagerSettings-Mode="NumericFirstLast" TabIndex="5">
                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                                <PagerStyle CssClass="paging" HorizontalAlign="Right" />
                                                <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left" />
                                                <RowStyle CssClass="clsdgItem" HorizontalAlign="Left" />
                                                <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="Black" />
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
                                                    <asp:BoundField DataField="DateFormatted" HeaderText="Date">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="LogNoLogPageNo" HeaderText="Log Detail(s)" SortExpression="LogNoLogPageNo">
                                                        <HeaderStyle Wrap="true" HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="true" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FlightNo" HeaderText="Flight No." SortExpression="FlightNo">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SouLocalDateTimeFormatted" HeaderText="Departure (Date Time)"
                                                        SortExpression="SouLocalDateTimeFormatted">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="True" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SouUniverseDateTimeFormatted" HeaderText="Departure UTC (Date Time)"
                                                        SortExpression="SouUniverseDateTimeFormatted">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="True" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SouPlaceName" HeaderText="From" SortExpression="SouPlaceName">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DesLocalDateTimeFormatted" HeaderText="Arrival (Date Time)"
                                                        SortExpression="DesLocalDateTimeFormatted">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="True" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DesUniverseDateTimeFormatted" HeaderText="Arrival UTC (Date Time)"
                                                        SortExpression="DesUniverseDateTimeFormatted">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle Wrap="True" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DesPlaceName" HeaderText="To" SortExpression="DesPlaceName">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TimeInAir" HeaderText="Airborne Time" SortExpression="TimeInAir">
                                                        <HeaderStyle HorizontalAlign="Right" Wrap="true" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="AirframeTotalCyclesOrLandings" HeaderText="Cyc./ Lndgs."
                                                        SortExpression="AirframeTotalCyclesOrLandings">
                                                        <HeaderStyle HorizontalAlign="Right" Wrap="true" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="f1-buttons">
                    <button type="button" class="btn btn-previous">
                        Previous</button>
                    <button type="button" class="btn btn-submit" id="btnsubmit" runat="server">
                        Submit</button>
                </div>
            </fieldset>
            <%--Modal POP up Remove--%>
            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyRem" Text="Dummy Rem" />
            </div>
            <asp:Panel runat="server" ID="pnlAssemblyRem" Style="z-index: 100000; display: none;
                background-color: #eee; width: 80%;">
                <div class="form-group">
                    <asp:UpdatePanel ID="upnlAssemblyRem" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="pnlRem" Visible="false" CssClass="f1">
                                <div class="row">
                                    <h3>
                                        Assembly Removal</h3>
                                    <div class="col-xs-12 col-sm-12 col-md-12">
                                        <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="a"
                                                    CssClass="clsValidationSummary"></asp:ValidationSummary>
                                                <asp:CustomValidator ID="cvAssemblyValue" runat="server" Display="None" ValidationGroup="a"
                                                    OnServerValidate="CustomValidate1" CssClass="clsLabel"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cvNote" runat="server" Display="None" OnServerValidate="customvalidate"
                                                    ValidationGroup="a" ErrorMessage="Remark Can't be greater than 200 chars" ControlToValidate="txtNote"
                                                    CssClass="clsLabel"></asp:CustomValidator>
                                                <asp:RequiredFieldValidator ID="rfvReason" runat="server" CssClass="clsLabelAuto"
                                                    ValidationGroup="a" Display="None" ErrorMessage="Reason Required" ControlToValidate="cmbReason"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cvReason" runat="server" CssClass="clsLabelAuto" Display="None"
                                                    ValidationGroup="a" OnServerValidate="customvalidate" ErrorMessage="Select Reason from the list."
                                                    ControlToValidate="cmbReason"></asp:CustomValidator>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-8 col-sm-8 col-md-8">
                                        <div class="row">
                                            <div class="col-xs-4 col-sm-4 col-md-4">
                                                <label class="sr-only" for="f1-email">
                                                    Aircraft</label>
                                                <asp:TextBox ID="txtReg" runat="server" CssClass="form-control textheight clsTextBox1_Ajax"
                                                    MaxLength="25" ReadOnly="True" BackColor="#E0E0E0" placeholder="Aircraft" ToolTip="Enter Aircraft"
                                                    TabIndex="2"></asp:TextBox>
                                            </div>
                                            <div class="col-xs-3 col-sm-3 col-md-3">
                                                <label class="sr-only required" for="f1-email">
                                                    Removed On</label>
                                                <asp:TextBox runat="server" ID="txtRemovedOn" CssClass="clsTextBox_Ajax form-control textheight"
                                                    Enabled="false" ReadOnly="true"></asp:TextBox>
                                                <cc2:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtRemovedOn">
                                                </cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtRemovedOn" ID="txtRemovedOn_watermarkextender"
                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>">
                                                </cc2:TextBoxWatermarkExtender>
                                            </div>
                                            <div class="col-xs-4 col-sm-4 col-md-4">
                                                <label class="sr-only required" for="f1-email">
                                                    ATA</label>
                                                <asp:DropDownList ID="cmbATA" runat="server" CssClass="form-control textheight" DataValueField="ID"
                                                    BackColor="#E0E0E0" Enabled="false" DataTextField="ATAChapter">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-xs-4 col-sm-4 col-md-4">
                                                <label class="sr-only required" for="f1-email">
                                                    Manufacturer</label>
                                                <asp:TextBox ID="txtRemManufacturer" runat="server" CssClass="form-control textheight"
                                                    placeholder="Manufacturer" ToolTip="Manufacturer's Name" Style="margin-top: 5px;"
                                                    MaxLength="50" ReadOnly="True" BackColor="#E0E0E0"></asp:TextBox>
                                            </div>
                                            <div class="col-xs-3 col-sm-3 col-md-3">
                                                <label class="sr-only" for="f1-email">
                                                    Model</label>
                                                <asp:TextBox ID="txtRemModel" runat="server" CssClass="form-control textheight" ToolTip="Model Name"
                                                    Style="margin-top: 5px;" ReadOnly="True" MaxLength="50" BackColor="#E0E0E0"></asp:TextBox>
                                            </div>
                                            <div class="col-xs-4 col-sm-4 col-md-4">
                                                <label class="sr-only" for="f1-email">
                                                    Serial No.</label>
                                                <asp:TextBox ID="txtRemSerialNo" runat="server" CssClass="form-control textheight"
                                                    ReadOnly="True" BackColor="#E0E0E0" Style="margin-top: 5px;" ToolTip="Serial No."
                                                    placeholder="Serial No."></asp:TextBox>
                                            </div>
                                            <%--   <div class="col-xs-3 col-sm-3 col-md-3">
                                                <label class="sr-only" for="f1-email">
                                                    Position</label>
                                                <asp:TextBox ID="txtRemoPosition" runat="server" CssClass="form-control textheight"
                                                    Style="margin-top: 5px;" ToolTip="Position" placeholder="Position" ReadOnly="True"
                                                    MaxLength="50" BackColor="#E0E0E0"></asp:TextBox>
                                            </div>--%>
                                        </div>
                                        <div class="row">
                                        </div>
                                        <div class="row">
                                            <div class="col-xs-9 col-sm-9 col-md-9">
                                                <label class="sr-only required" for="f1-email">
                                                    Reason</label>
                                                <asp:DropDownList ID="cmbReason" runat="server" CssClass="form-control required textheight"
                                                    Style="margin-top: 5px;" DataTextField="Name" DataValueField="ID" placeholder="Reason"
                                                    TabIndex="3">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-xs-3 col-sm-3 col-md-3" style="margin-top: 5px;">
                                                <asp:CheckBox ID="chkIsRemUnscheduled" runat="server" CssClass="clsCheckBox" Text="Un-Schedule" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-xs-4 col-sm-4 col-md-4">
                                                <label class="sr-only" for="f1-email">
                                                    Work Order No.</label>
                                                <asp:TextBox ID="txtWorkOrderNo" runat="server" CssClass="form-control textheight"
                                                    Style="margin-top: 5px;" MaxLength="25" placeholder="Work Order No." ToolTip="Enter Work Order Number"
                                                    TabIndex="2"></asp:TextBox>
                                            </div>
                                            <div class="col-xs-3 col-sm-3 col-md-3">
                                                <label class="sr-only" for="txtPlace">
                                                    Place</label>
                                                <asp:TextBox ID="txtPlace" runat="server" CssClass="form-control textheight" MaxLength="25"
                                                    placeholder="Place" Style="margin-top: 5px;" ToolTip="Enter Place" TabIndex="5"></asp:TextBox>
                                            </div>
                                            <asp:UpdatePanel ID="upnlLicenceNo" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div class="col-xs-4 col-sm-4 col-md-4">
                                                        <label class="sr-only" for="f1-email">
                                                            License No.</label>
                                                        <asp:TextBox ID="txtLicenceNo" runat="server" CssClass="form-control textheight"
                                                            Style="margin-top: 5px;" AutoComplete="off" ClientIDMode="Static" AutoPostBack="true"
                                                            MaxLength="200" OnTextChanged="txtLicenceNo_TextChanged" placeholder="License No."
                                                            ToolTip="Enter Licence No." TabIndex="2 "></asp:TextBox>
                                                        <asp:CustomValidator ID="cvLicenceNo" runat="server" ControlToValidate="txtLicenceNo"
                                                            CssClass="clsLabelAuto" Display="None" ErrorMessage="Enter correct License No"
                                                            OnServerValidate="customvalidate"></asp:CustomValidator>
                                                        <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtLicenceNo_Autocomplete" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                            CompletionInterval="1" ServicePath="wfAssemblyRemoveInstallWizard.aspx" ServiceMethod="GetLicenceList"
                                                            TargetControlID="txtLicenceNo" UseContextKey="False" ContextKey="" CompletionListCssClass="ac_results_Main"
                                                            CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                            OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                                            OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                                        </cc2:AutoCompleteExtender>
                                                    </div>
                                                    <div class="col-xs-1 col-sm-1 col-md-1">
                                                        <asp:ImageButton ID="imgbtnEmployeeLicence" runat="server" ImageUrl="~/images/plus1.png"
                                                            Visible="false" Style="margin-top: 10px;" Height="22px" ToolTip="Click to select multiple Licence No."
                                                            CausesValidation="true" />
                                                    </div>
                                                    <div class="col-xs-3 col-sm-3 col-md-3" style="margin-top: 5px;">
                                                        <asp:Label ID="lblLicenceCount" runat="server" Text="and More" CssClass="form-control clsCursorStyle"
                                                            Visible="false" Style="border-style: none"></asp:Label></div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="row">
                                        </div>
                                        <div class="row">
                                            <div class="col-xs-12 col-sm-12 col-md-12">
                                                <label class="sr-only" for="txtNote">
                                                    Note</label>
                                                <asp:TextBox ID="txtNote" runat="server" CssClass="form-control textheight" Style="margin-top: 5px;"
                                                    placeholder="Note" MaxLength="200" TextMode="MultiLine" ToolTip="Enter Note"
                                                    TabIndex="6"></asp:TextBox>
                                            </div>
                                        </div>
                                        <%--  <div class="row">
                                            <asp:UpdatePanel ID="upnlAttach" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div class="col-xs-5 col-sm-5 col-md-5" style="text-align: left">
                                                        <asp:Button ID="btnSelectRemovalFile" runat="server" CssClass="form-control" Style="margin-top: 5px;"
                                                            Text="Select File" />
                                                    </div>
                                                    <div class="col-xs-5 col-sm-5 col-md-5" style="text-align: left">
                                                        <asp:Button ID="btnDelAttach" runat="server" CssClass="form-control" Enabled="False"
                                                            Style="margin-top: 5px; width: 100%;" Text="Remove Attachment" ToolTip="Click to Remove Attachment" />
                                                    </div>
                                                    <div class="col-xs-2 col-sm-2 col-md-2" style="text-align: left">
                                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" Height="20px"
                                                            Visible="false" Style="margin-top: 10px;" ImageUrl="icons/CLIP01.ICO" />
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>--%>
                                        <%--  <div>
                                            <div class="jsFile-upload">
                                                <input type="file" id="Myfile" runat="server" name="uploadFIle[]" multiple />
                                            </div>
                                        </div>--%>
                                    </div>
                                    <div class="col-xs-4 col-sm-4 col-md-4">
                                        <asp:GridView ID="dgRemovalValue" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                            GridLines="None" CssClass="table table-striped table-bordered table-hover" DataKeyNames="AssemblyStatusID"
                                            ShowHeaderWhenEmpty="True" TabIndex="7">
                                            <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left" />
                                            <RowStyle CssClass="clsdgItem" HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="black" />
                                            <Columns>
                                                <asp:BoundField DataField="PeriodName" HeaderText="Period " HtmlEncode="False" />
                                                <asp:BoundField DataField="AssemblyRemovalValueFormatted" HeaderText="Assembly" HtmlEncode="False" />
                                                <asp:BoundField DataField="MachineRemovalValueFormatted" HeaderText="Airframe" HtmlEncode="False" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div class="f1-buttons">
                                    <asp:Button ID="btnRemOk" runat="server" CssClass="btn btn-next" Style="background-color: #f35b3f"
                                        ForeColor="white" ValidationGroup="Rem" Text="Ok" ToolTip="Click to add new Rem">
                                    </asp:Button>
                                    <asp:Button ID="btnRemClose" TabIndex="0" runat="server" CssClass="btn btn-next"
                                        Style="background-color: #f35b3f" ForeColor="white" ToolTip="Click to close Change Rem screen"
                                        Text="Close" CausesValidation="False"></asp:Button>
                                    <asp:Button ID="hdnBtnMaintDoneBy" ClientIDMode="Static" runat="server" Text="----"
                                        CausesValidation="False" Style="display: none;"></asp:Button>
                                    <asp:Button ID="hdnBtnFileUpload" runat="server" CausesValidation="False" ClientIDMode="Static"
                                        Style="display: none;" Text="----" />
                                    <asp:Button ID="hdnBtnSubmit" ClientIDMode="Static" runat="server" Text="----" CausesValidation="False"
                                        Style="display: none;"></asp:Button>
                                    <asp:Button ID="hdnSelectRemoveAssembly" ClientIDMode="Static" runat="server" Text="----"
                                        CausesValidation="False" Style="display: none;"></asp:Button>
                                    <asp:Button ID="hdnSelectInstAssembly" ClientIDMode="Static" runat="server" Text="----"
                                        CausesValidation="False" Style="display: none;"></asp:Button>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="mdlPopUpAssemblyRem" runat="server" TargetControlID="btnDummyRem"
                PopupControlID="pnlAssemblyRem" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>
            <%--End Modal POP up Remove--%>
            <input id="hdnRemAssembly" type="hidden" runat="server" value="" />
            <input id="hdnInstAssembly" type="hidden" runat="server" value="" />
            <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" ClientIDMode="Static" DynamicLayout="false"
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
            <%--autocomplete css functions--%>
            <script type="text/javascript">
                //bold input value in list...
                function ClientPopulated(source, eventArgs) {
                    $("#" + source._element.id).removeClass("ac_loading");
                }
                //Alternate item style
                function ClientShowing(source, eventArgs) {
                    $.elements = $(source.get_completionList());
                    $.elements.find(".ac_results_li").each(function (i) {
                        if (i % 2 == 0) {
                            //$(this).addClass("ac_even");
                        }
                        else {
                            $(this).addClass("ac_odd");
                        }
                    });

                }
                //add loader to textbox
                function ClientPopulating(source, e) {
                    $("#" + source._element.id).addClass("ac_loading");
                }
                //remove loader from textbox
                function ClientHiding(source, eventArgs) {
                    $("#" + source._element.id).removeClass("ac_loading");
                    source._completionListElement.style.zIndex = 10000001;
                }
            </script>
            <script type="text/javascript">
                function showfirstwiz() {
                    $('.f1 fieldset:first').fadeIn('slow');
                }
                function showsecondwiz() {

                    var parent_fieldset = $("#fld2").parents('fieldset');
                    var current_active_step = $("#fld2").parents('.f1').find('.f1-step.active');
                    var progress_line = $("#fld2").parents('.f1').find('.f1-progress-line');

                    // change icons
                    //   current_active_step.removeClass('active').addClass('activated').next().addClass('active');
                    // progress bar
                    bar_progress(progress_line, 'right');
                    // show next step
                    $("#fld2").fadeIn('slow');
                    $("#fld1").hide();
                    // scroll window to beginning of the form
                    scroll_to_class($('.f1'), 20);

                }
                function showthirdwiz() {

                    //                    var parent_fieldset = $("#fld3").parents('fieldset');
                    //                    var current_active_step = $("#fld3").parents('.f1').find('.f1-step.active');
                    //                    var progress_line = $("#fld3").parents('.f1').find('.f1-progress-line');
                    //                    bar_progress(progress_line, 'right');

                    //                    $("#fld3").fadeIn('slow');
                    //                    $("#fld1").hide();
                    //                    $("#fld2").hide();
                    $("#fld3").fadeIn('slow');
                }
                //                function showfourthwiz() {
                //                    $("#fld4").fadeIn('slow');
                //                }
            </script>
            <%--Modal POP up Install--%>
            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyInstall" Text="Dummy Install" />
            </div>
            <asp:Panel runat="server" ID="pnlInstallAssembly" Style="z-index: 100000; background-color: #eee;
                width: 80%;">
                <div class="form-group">
                    <asp:UpdatePanel ID="upnlInstallAssembly" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="pnlInst" Visible="false" CssClass="f1" BorderStyle="Ridge">
                                <div class="row">
                                    <h3>
                                        Assembly Installation</h3>
                                    <div class="col-xs-12 col-sm-12 col-md-12">
                                        <asp:UpdatePanel ID="upnlValidationSummaryInst" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                                    HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                                <asp:CustomValidator ID="cvLicenseNo" runat="server" OnServerValidate="customvalidate"
                                                    ControlToValidate="txtLicenceNo" ErrorMessage="Enter correct License No" Display="None"
                                                    CssClass="clsLabelAuto"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cvATAChapter" runat="server" OnServerValidate="CustomValidate"
                                                    ControlToValidate="cmbATAChapter" ErrorMessage="Select ATA Chapter From List."
                                                    Display="None" CssClass="clsLabelAuto"></asp:CustomValidator>
                                                <asp:RequiredFieldValidator ID="rfvSerialNo" runat="server" ControlToValidate="txtSerialNo"
                                                    ErrorMessage="Serial No Required." Display="None" CssClass="clsLabelAuto"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cvInstallationReasonLen" runat="server" OnServerValidate="CustomValidate"
                                                    ControlToValidate="txtInstallationReason" ErrorMessage="Installation Reason Too Long."
                                                    Display="None" CssClass="clsLabelAuto"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cvInstallationRemark" runat="server" OnServerValidate="CustomValidate"
                                                    ControlToValidate="txtNote" ErrorMessage="Installation Remark Too Long." Display="None"
                                                    CssClass="clsLabelAuto"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cvAssemblyInstallationValue" runat="server" Display="None"
                                                    OnServerValidate="CustomValidate1"></asp:CustomValidator>
                                                <asp:CustomValidator ID="CustomValidator1" runat="server" OnServerValidate="CustomValidate"
                                                    ControlToValidate="txtInstalledOnDate" ValidateEmptyText="true" ErrorMessage="Installation Remark Too Long."
                                                    Display="None" CssClass="clsLabelAuto"></asp:CustomValidator>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-7 col-sm-7 col-md-7">
                                        <div class="row">
                                            <div class="col-xs-6 col-sm-6 col-md-6">
                                                <label class="sr-only" for="f1-email">
                                                    Aircraft</label>
                                                <asp:TextBox ID="txtAircraft" runat="server" CssClass="form-control textheight clsTextBox1_Ajax"
                                                    MaxLength="25" ReadOnly="True" BackColor="#E0E0E0" placeholder="Aircraft" ToolTip="Enter Aircraft"
                                                    TabIndex="2"></asp:TextBox>
                                            </div>
                                            <div class="col-xs-6 col-sm-6 col-md-6">
                                                <label class="sr-only required" for="f1-email">
                                                    Installed On</label>
                                                <asp:TextBox runat="server" ID="txtInstalledOnDate" CssClass="clsTextBox_Ajax form-control textheight"
                                                    Enabled="false" ReadOnly="true" onchange="ValidateDateText(this,'InstalledOnDate_watermarkextender','true');"></asp:TextBox>
                                                <cc2:CalendarExtender ID="txtInstalledOnDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                    Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtInstalledOnDate">
                                                </cc2:CalendarExtender>
                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtInstalledOnDate" ID="InstalledOnDate_watermarkextender"
                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>">
                                                </cc2:TextBoxWatermarkExtender>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-xs-6 col-sm-6 col-md-6">
                                                <label class="sr-only required" for="f1-email">
                                                    Manufacturer</label>
                                                <asp:TextBox ID="txtManufacturer" runat="server" CssClass="form-control textheight"
                                                    ToolTip="Manufacturer's Name" Style="margin-top: 5px;" MaxLength="50" ReadOnly="True"
                                                    BackColor="#E0E0E0"></asp:TextBox>
                                            </div>
                                            <div class="col-xs-6 col-sm-6 col-md-6">
                                                <label class="sr-only" for="f1-email">
                                                    Model</label>
                                                <asp:TextBox ID="txtModel" runat="server" CssClass="form-control textheight" ToolTip="Model Name"
                                                    Style="margin-top: 5px;" ReadOnly="True" MaxLength="50" BackColor="#E0E0E0"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-xs-6 col-sm-6 col-md-6">
                                                <label class="sr-only required" for="f1-email">
                                                    ATA</label>
                                                <asp:DropDownList ID="cmbATAChapter" runat="server" CssClass="form-control textheight"
                                                    DataValueField="ID" Style="margin-top: 5px;" DataTextField="ATAChapter">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-xs-6 col-sm-6 col-md-6">
                                                <label class="sr-only" for="f1-email">
                                                    Serial No.</label>
                                                <asp:TextBox ID="txtSerialNo" runat="server" CssClass="form-control textheight" ToolTip="Serial No."
                                                    placeholder="Serial No." Style="margin-top: 5px;"></asp:TextBox>
                                            </div>
                                            <div class="col-xs-6 col-sm-6 col-md-6">
                                                <label class="sr-only" for="f1-email">
                                                    Position</label>
                                                <asp:TextBox ID="txtPosition" runat="server" CssClass="form-control textheight" ToolTip="Position"
                                                    placeholder="Position" Style="margin-top: 5px;" ReadOnly="True" MaxLength="50"
                                                    BackColor="#E0E0E0"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-xs-6 col-sm-6 col-md-6">
                                                <label class="sr-only" for="txtWorkOrNo">
                                                    Work Order No.</label>
                                                <asp:TextBox ID="txtWorkOrNo" runat="server" CssClass="form-control textheight" ToolTip="Enter Installation Work Order Number"
                                                    Style="margin-top: 5px;" MaxLength="25" placeholder="Work Order No."></asp:TextBox>
                                            </div>
                                            <asp:UpdatePanel ID="upnlLicenceNoInst" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div class="col-xs-6 col-sm-6 col-md-6">
                                                        <label class="sr-only" for="f1-email">
                                                            License No.</label>
                                                        <asp:TextBox ID="txtLicenceNoInst" runat="server" CssClass="form-control textheight"
                                                            AutoComplete="off" Style="margin-top: 5px;" ClientIDMode="Static" AutoPostBack="true"
                                                            MaxLength="200" OnTextChanged="txtLicenceNo_TextChanged" Width="100%" placeholder="License No."
                                                            ToolTip="Enter Licence No." TabIndex="2 "></asp:TextBox>
                                                        <asp:CustomValidator ID="CustomValidator4" runat="server" ControlToValidate="txtLicenceNoInst"
                                                            CssClass="clsLabelAuto" Display="None" ErrorMessage="Enter correct License No"
                                                            OnServerValidate="customvalidate"></asp:CustomValidator>
                                                        <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtLicenceNoInst_Extender" runat="server"
                                                            DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                            CompletionInterval="1" ServicePath="wfAssemblyRemoveInstallWizard.aspx" ServiceMethod="GetLicenceList"
                                                            TargetControlID="txtLicenceNoInst" UseContextKey="False" ContextKey="" CompletionListCssClass="ac_results_Main"
                                                            CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                            OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                                            OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                                        </cc2:AutoCompleteExtender>
                                                    </div>
                                                    <div class="col-xs-1 col-sm-1 col-md-1">
                                                        <asp:ImageButton ID="imgbtnEmployeeLicenceInst" runat="server" ImageUrl="~/images/plus1.png"
                                                            Visible="false" Style="margin-top: 10px;" Height="22px" ToolTip="Click to select multiple Licence No."
                                                            CausesValidation="true" />
                                                    </div>
                                                    <div class="col-xs-3 col-sm-3 col-md-3" style="margin-top: 5px; border-style: none">
                                                        <asp:Label ID="lblLicenceCountInst" runat="server" Text="and More" CssClass="clsCursorStyle"
                                                            Visible="false"></asp:Label>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="row">
                                            <div class="col-xs-6 col-sm-6 col-md-6">
                                                <label class="sr-only" for="txtInstallationReason">
                                                    Installation Reason</label>
                                                <asp:TextBox ID="txtInstallationReason" runat="server" CssClass="form-control" ToolTip="Enter Installation Reason"
                                                    Height="40px" Style="margin-top: 5px;" placeholder="Installation Reason" MaxLength="1000"
                                                    TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                            <div class="col-xs-6 col-sm-6 col-md-6">
                                                <label class="sr-only" for="txtNoteInst">
                                                    Note</label>
                                                <asp:TextBox ID="txtNoteInst" runat="server" CssClass="form-control clsTextBoxMultiLine"
                                                    Height="40px" Style="margin-top: 5px;" placeholder="Note" TextMode="MultiLine"
                                                    ToolTip="Enter Note" TabIndex="6"></asp:TextBox>
                                            </div>
                                        </div>
                                        <%-- <div class="row">
                                            <asp:UpdatePanel ID="upnlAttachInst" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div class="col-xs-6 col-sm-6 col-md-6" style="text-align: left">
                                                        <asp:Button ID="btnSelectFileInst" runat="server" CssClass="form-control" Style="width: 100%;
                                                            margin-top: 5px;" Text="Select File" />
                                                    </div>
                                                    <div class="col-xs-4 col-sm-4 col-md-4" style="text-align: left">
                                                        <asp:Button ID="btnRemoveFileInst" runat="server" CssClass="form-control" Enabled="False"
                                                            Style="width: 100%; margin-top: 5px;" Text="Remove Attachment" ToolTip="Click to Install Attachment" />
                                                    </div>
                                                    <div class="col-xs-2 col-sm-2 col-md-2" style="text-align: left">
                                                        <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="False" Height="20px"
                                                            Visible="false" Style="margin-top: 10px;" ImageUrl="icons/CLIP01.ICO" />
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>--%>
                                        <%-- <div class="row">
                                            <div class="col-xs-3 col-sm-3 col-md-3" style="text-align: left">
                                                <div class="fileUpload1 uploadbtn">
                                                    <span>Browse...</span>
                                                    <asp:FileUpload ID="FileUpload1" runat="server" onchange="showfilepath(this);" />
                                                </div>
                                            </div>
                                            <div class="col-xs-5 col-sm-5 col-md-5" id="filepath" runat="server" style="display: inline-block;
                                                left: 0; position: relative; font-family: Segoe UI; white-space: nowrap; color: gray;
                                                font-style: italic;">
                                                No file selected
                                                <asp:TextBox runat="server" ID="hdnfilepath" />
                                            </div>
                                            <div class="col-xs-4 col-sm-4 col-md-4" style="text-align: left">
                                                <asp:Button ID="btnupload" runat="server" Text="Attach" ToolTip="click to attach selected file"
                                                    CssClass="clsButton_Ajax" />
                                            </div>
                                        </div>--%>
                                    </div>
                                    <div class="col-xs-5 col-sm-5 col-md-5">
                                        <asp:UpdatePanel ID="upnlInstallationValue" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:GridView ID="dgInstallationValue" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                    GridLines="None" CssClass="table table-striped table-bordered table-hover" DataKeyNames="AssemblyStatusID"
                                                    ShowHeaderWhenEmpty="True" TabIndex="7">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left" />
                                                    <RowStyle CssClass="clsdgItem" HorizontalAlign="Left" />
                                                    <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" ForeColor="black" />
                                                    <Columns>
                                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                        <asp:BoundField DataField="PeriodName" HeaderText="Period ">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Assembly" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtAssemblyInstallationValue" runat="server" CssClass="form-control clsTextBoxRightAlignSmall_Ajax"
                                                                    AutoPostBack="false" CausesValidation="true" ClientIDMode="Static" Text='<%# DataBinder.Eval(Container.DataItem,"AssemblyInstallationValueFormatted") %>'>
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                            <%--OnTextChanged="txtAssemblyInstallationValue_TextChanged"--%>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="MachineInstallationValueFormatted" HeaderText="Airframe">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle Wrap="false" />
                                                        </asp:BoundField>
                                                        <asp:ButtonField Text="Remove" HeaderText="Remove" CommandName="DeleteRec" Visible="false">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:ButtonField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div class="f1-buttons">
                                    <asp:Button ID="btnInstOk" runat="server" CssClass="btn
                                btn-next" Style="background-color: #f35b3f" ForeColor="white" Text="Ok" CausesValidation="true"
                                        ToolTip="Click to add new Rem"></asp:Button>
                                    <asp:Button ID="btnInstClose" TabIndex="0" runat="server" CssClass="btn btn-next"
                                        Style="background-color: #f35b3f" ForeColor="white" ToolTip="Click to close Change Rem screen"
                                        Text="Close" CausesValidation="False"></asp:Button>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                        <%-- <Triggers>
                            <asp:PostBackTrigger ControlID="btnupload" />
                        </Triggers>--%>
                    </asp:UpdatePanel>
                </div>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="mdlPopUpInstallAssembly" runat="server" TargetControlID="btnDummyInstall"
                PopupControlID="pnlInstallAssembly" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>
            <%--End Modal POP up Install--%>
            <!-- Assembly Insp Maintenance Done By Employee Dialog-->
            <div style="display: none">
                <asp:HiddenField runat="server" ID="btnDummyMaintDoneBy" />
            </div>
            <asp:Panel runat="server" ID="pnlMaintDoneBy" HorizontalAlign="Center" Style="height: 100%;
                width: 100%;">
                <iframe id="IMaintDoneBy" allowtransparency="true" frameborder="0" height="100%"
                    width="100%" src="JavaScript:''" scrolling="auto"></iframe>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="mdlPopupMaintDoneBy" runat="server" TargetControlID="btnDummyMaintDoneBy"
                PopupControlID="pnlMaintDoneBy" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>
            <script type="text/javascript">
                function IFrameMaintDoneByStateComplete() {
                    $("#btnDummyMaintDoneBy").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }


                function AddEmployeeLicNo() {
                    try {
                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#IMaintDoneBy").attr("src", "wfMaintenanceDoneByEmployee_Ajax.aspx?Type=pup&MaintTypeID=2");

                        //                if (!$.browser.msie) {
                        $("#btnDummyMaintDoneBy").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                        //                }

                        return false;
                    } catch (e) {
                        alert(e);
                    }


                }
                function AddEmployeeLicNoInst() {
                    try {
                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#IMaintDoneBy").attr("src", "wfMaintenanceDoneByEmployee_Ajax.aspx?Type=pup&MaintTypeID=1");

                        // if (!$.browser.msie) {
                        $("#btnDummyMaintDoneBy").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                        //    }

                        return false;
                    } catch (e) {
                        alert(e);
                    }


                }
            </script>
            <%-- <script type="text/javascript">
                function ParentCallBackFunctionForMaintDoneBy() {
                    var MaintDoneBywindow = $find("<%=mdlPopupMaintDoneBy.ClientID %>");
                    //close Ass Insp Maint Done By Emp popup window
                    MaintDoneBywindow.hide();
                    //Free resources
                    $("#IMaintDoneBy").attr("src", "JavaScript:''");
                    $("#hdnBtnMaintDoneBy").click();

                }
            </script>--%>
            <!-- End -->
            <!-- File Upload Modal Dialog-->
            <div style="display: none">
                <asp:HiddenField runat="server" ID="btnDummyFileUpload" />
            </div>
            <asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%;
                width: 100%;">
                <iframe id="IFileUpload" allowtransparency="true" frameborder="0" height="100%" width="100%"
                    src="JavaScript:''" scrolling="auto"></iframe>
                <iframe id="IFileInstUpload" allowtransparency="true" frameborder="0" height="100%"
                    width="100%" src="JavaScript:''" scrolling="auto"></iframe>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="mdlPopupFileUpload" runat="server" TargetControlID="btnDummyFileUpload"
                PopupControlID="pnlFileUpload" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>
            <script type="text/javascript">
                function IFrameFileUploadStateComplete() {
                    $("#btnDummyFileUpload").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                    $("#hdnBtnFileUpload").click();
                }

                function OpenFileDialog() {
                    try {
                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#IFileInstUpload").attr("src", "wfFileUploadForSeparateTable.aspx");
                        //  if (!$.browser.msie) {
                        $("#btnDummyFileUpload").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                        //   }

                        return false;
                    } catch (e) {
                        alert(e);
                    }

                }

                //                $(document).ready(function () {
                //                    $("#btnSelectFileInst").on("click", function () {
                //                        try {

                //                            $get("AjaxLoader").style.visibility = 'visible';
                //                            $("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx");
                //                            //                        $("#IFileUpload").ready(function () {
                //                            //                            $("#btnDummyFileUpload").click();
                //                            //                            $get("AjaxLoader").style.visibility = 'hidden';
                //                            //                        });
                //                            //  if (!$.browser.msie) {
                //                            $("#btnDummyFileUpload").click();
                //                            $get("AjaxLoader").style.visibility = 'hidden';
                //                            //  }

                //                            return false;
                //                        } catch (e) {
                //                            alert(e);
                //                        }


                //                    });
                //                }); 
            </script>
            <script type="text/javascript">
                function ParentCallBackFunctionForFileUpload(fileattached) {
                    var FileUpwindow = $find("<%=mdlPopupFileUpload.ClientID %>");
                    //close File Upload popup window
                    FileUpwindow.hide();
                    //Free resources
                    $("#IFileUpload").attr("src", "JavaScript:''");
                    if (fileattached) {
                        //call hidden button to set file upload content to object
                        $("#hdnBtnFileUpload").click();
                    }
                }
            </script>
            <%--<script type="text/javascript">
                $(document).ready(function () {
                    $("#<%=btnupload.ClientID %>").on("click", function () {
                        var tempval = document.getElementById("FileUpload1").value;


                        if (tempval) {

                            // parent.submitChildForm();
                            $('#fileuploadform').submit();
                            document.getElementById("FileUpload1").value = tempval;
                            return true;
                        }
                        else {
                            return false;
                        }
                    });
                });

                function onuploadcomplete(fileattached) {
                    parent.ParentCallBackFunctionForFileUpload(fileattached);
                    return false;
                }
                var timeout;
                var duration;
                var marginleft;
                function showfilepath(elem) {
                    $("#<%=btnupload.ClientID %>").removeAttr('disabled');
                    $("#filepath").clearQueue().stop();
                    $("div:animated").stop(true, true);
                    $("#filepath").html('');
                    $("#filepath").html(elem.value);
                    $("#filepath").attr("title", elem.value);
                    $("#filepath").css({ 'left': '0', 'font-style': 'normal', 'color': '#1C1F24' });
                    //var marginleft = $("body #tblmain:eq(0)").css('margin-left');
                    marginleft = $("#filepath").parent().width() - $("#filepath").width();
                    if (marginleft < 0) {
                        duration = ((-1 * marginleft) / 100) * 2000;
                        Marquee(marginleft, duration);
                    }
                    //$("#filepath")

                    // $("#hdnfilepath").attr("text", elem.value);
                    //  document.getElementById("<%= hdnfilepath.ClientID %>").value = elem.value;
                    //alert(elem.value);
                   
                }
                function Marquee(margin, dur) {
                    $("#filepath").delay(2000).animate({ 'left': margin }, dur, 'linear', function () {
                        $("#filepath").delay(2000).animate({ 'left': 0 }, 0, 'linear');
                        Marquee(marginleft, duration);
                    });

                }
            </script>--%>
            <!-- End -->
           
            <asp:UpdatePanel ID="upnlMSGBoxNew" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc2:MSGBoxNew ID="MSGBoxCtrlNEW" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
            </form>
        </div>
        <!-- Javascript -->
        <!--[if lt IE 10]>
            <script src="assets/js/placeholder.js"></script>
        <![endif]-->
        <%--Date Validations--%>
        <script type="text/javascript">
            //Date validations
            function ValidateDateText(elem, extenderid) {

                var datevalue = $(elem).val();
                var params = { 'Date': datevalue, 'SetDefault': 'false' };
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
    </div>
</body>
</html>
