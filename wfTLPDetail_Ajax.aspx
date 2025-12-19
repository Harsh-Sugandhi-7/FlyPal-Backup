<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfTLPDetail_Ajax.aspx.vb"
    Inherits="Flypal.wfTLPDetail_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>TLP Details</title>
    <meta name="vs_showGrid" content="True" />
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1" />
    <meta name="vs_defaultClientScript" content="JavaScript" />
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
    <link id="MainStyle" type="text/css" rel="stylesheet" />

    <asp:PlaceHolder runat="server">
        <%--AJAX- Replaced "LocalFunction.htm" to "LocalFunctionAjax.htm"--%>
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>

    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>

    <script id="clientEventHandlersJS" type="text/javascript">
        function openTranDetail() {
            str = "wfReports.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openTranDetail1() {
            str = "webform1.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
        function openDetail() {
            str = "wfDetail.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>

    <link rel="stylesheet" type="text/css" href="Calander\css\start\jquery-ui-1.8.14.custom.css" />
    <link rel="stylesheet" type="text/css" href="Calander\css\demos.css" />
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
</head>
<body bottommargin="5" leftmargin="5" rightmargin="5" topmargin="5">
    <form id="Form1" method="post" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
            runat="server">
        </asp:ScriptManager>
        <script language="javascript" type="text/javascript">

            var g_CurrentTextBox;
            var g_isTabPressed;

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
            function endRequestHandler() {

                try {

                    //if (g_isTabPressed == 1) {
                    $get(g_CurrentTextBox).focus();
                    $get(g_CurrentTextBox).select();

                    g_isTabPressed = 0;
                    //}


                }
                catch (Error) { }

            }


            function onTextFocus() {
                g_CurrentTextBox = event.srcElement.id;

            }

            function onkeyPressed(keycode, obj) {

                if (keycode == 9) {

                    g_isTabPressed = 1;
                }

            }

        </script>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:msgbox id="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <table class="clstablelistout">
                <tr>
                    <td>
                        <asp:Panel ID="pnlMain" CssClass="clsPanel1" runat="server">
                            <table class="clstablelistin">
                                <tr>
                                    <td class="clsFormHeader1Newstyle" colspan="2">
                                        <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">TLP Details</asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="upnlErrorList" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                    HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                                <asp:CustomValidator ID="cvCustom" runat="server" OnServerValidate="customvalidate1"
                                                    Display="None"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cvDepartureDateTime" runat="server" OnServerValidate="customvalidate"
                                                    Display="None" ErrorMessage="Departure date should be in date time format." ControlToValidate="calDeparture"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cvArrivalDateTime" runat="server" OnServerValidate="customvalidate"
                                                    Display="None" ErrorMessage="Arrival date should be in date time format." ControlToValidate="calArrival"></asp:CustomValidator>
                                                <asp:RequiredFieldValidator ID="rfvDate" runat="server" Display="None" ErrorMessage="Log Date Required."
                                                    ControlToValidate="calDateTime" CssClass="clsLabelAuto"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cvAirBornTime" runat="server" OnServerValidate="customvalidate"
                                                    Display="None" ErrorMessage="Not be Nigative." ControlToValidate="txtAirBorneTime"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cvPlace1" runat="server" OnServerValidate="customvalidate"
                                                    Display="None" ErrorMessage="Enter correct Source name." ControlToValidate="Place1"></asp:CustomValidator>
                                                <asp:CustomValidator ID="cvPlace2" runat="server" OnServerValidate="customvalidate"
                                                    Display="None" ErrorMessage="Enter correct Destination name." ControlToValidate="Place2"></asp:CustomValidator>
                                                <asp:CustomValidator ID="CvTime" runat="server" Display="None" ErrorMessage="Invalid Time Format."
                                                    ControlToValidate=""></asp:CustomValidator>

                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="upnlDetails" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px">
                                                                            <legend style="font-weight: bold"><b>Log Details</b> </legend>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblDateTime" runat="server" CssClass="clsLabelAuto" Text='<%# IIf(mLog.IsUTC = True, "Date (UTC)", "Date") %>'>Date</asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="calDateTime" runat="server" CssClass="clsTextBoxTagSearch" BackColor="Gainsboro"
                                                                                            ReadOnly="True" Width="100px" TabIndex="1"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblFlightNo" runat="server" CssClass="clsLabelAuto">Flight No.</asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtFlightNo" runat="server" CssClass="clsTextBoxTagSearchSmall" ToolTip="Enter Flight No."
                                                                                            Text="<%# mLogDetail.FlightNo %>" MaxLength="10" TabIndex="2"></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblLogPageNo" runat="server" CssClass="clsLabelAuto">TLP No.</asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtLogPageNo" runat="server" CssClass="clsTextBoxTagSearchSmall" BackColor="Gainsboro"
                                                                                            ReadOnly="True" ToolTip="Log Page No." Text="<%# mLog.LogPageNoFormatted %>" TabIndex="3"
                                                                                            MaxLength="9"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </fieldset>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table width="97%">
                                                                <tr>
                                                                    <td valign="top">
                                                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px; height: 120px">
                                                                            <legend style="font-weight: bold"><b>Departure Info</b> </legend>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblPalceStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblDepPlace" runat="server" CssClass="clsLabelAuto">Place</asp:Label>
                                                                                    </td>
                                                                                    <td>&nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="Place1" runat="server" CssClass="clsTextBoxTagSearch" Width="150px"
                                                                                            Text="<%# mLogDetail.SourceName %>" TabIndex="4"></asp:TextBox>
                                                                                        &nbsp;
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblDateTimeStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                                        <asp:Label ID="lblUTCDateTimeStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblDepDateTime" runat="server" CssClass="clsLabelAuto">Date/Time</asp:Label>
                                                                                        <asp:Label ID="lblUTCDateTime" runat="server" CssClass="clsLabelAuto">UTC Date/Time</asp:Label>
                                                                                    </td>
                                                                                    <td>&nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="calDeparture" CssClass="clsTextBoxTagSearch" Width="90px"
                                                                                            BackColor="#E0E0E0" ReadOnly="true" AutoPostBack="true" onchange="ValidateDateText(this,'DateTime_watermarkextender','false');"></asp:TextBox>
                                                                                        <cc2:calendarextender id="calDeparture_CalendarExtender" runat="server" cssclass="cal_Theme1"
                                                                                            enabled="false" format="<%$AppSettings:DateFormat%>" targetcontrolid="calDeparture">
                                                                                        </cc2:calendarextender>
                                                                                        <cc2:textboxwatermarkextender targetcontrolid="calDeparture" id="DateTime_watermarkextender"
                                                                                            enabled="false" clientidmode="Static" runat="server" watermarktext="<%$AppSettings:DateFormat%>">
                                                                                        </cc2:textboxwatermarkextender>
                                                                                        <asp:TextBox runat="server" ID="CalUTCDateTime" CssClass="clsTextBoxTagSearch" Width="90px"
                                                                                            BackColor="#E0E0E0" ReadOnly="true" AutoPostBack="True" CausesValidation="True"
                                                                                            onchange="ValidateDateText(this,'CalUTCDateTime_watermarkextender');"></asp:TextBox>
                                                                                        <cc2:calendarextender id="CalUTCDateTime_CalendarExtender" runat="server" cssclass="cal_Theme1"
                                                                                            enabled="false" format="<%$AppSettings:DateFormat%>" targetcontrolid="CalUTCDateTime">
                                                                                        </cc2:calendarextender>
                                                                                        <cc2:textboxwatermarkextender targetcontrolid="CalUTCDateTime" id="CalUTCDateTime_watermarkextender"
                                                                                            clientidmode="Static" runat="server" watermarktext="<%$AppSettings:DateFormat%>"
                                                                                            watermarkcssclass="clsDateTextBox">
                                                                                        </cc2:textboxwatermarkextender>
                                                                                        <asp:TextBox ID="txtDepartureTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
                                                                                            MaxLength="10" ReadOnly="True" ToolTip="Enter Departure Time." Width="65px"></asp:TextBox>
                                                                                        <asp:TextBox ID="txtUTCDepartureTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
                                                                                            MaxLength="10" TabIndex="6" ToolTip="Enter Departure Time." Width="65px"></asp:TextBox>
                                                                                        <cc2:maskededitextender id="txtDepartureTimeMaskedEditExtender" targetcontrolid="txtDepartureTime"
                                                                                            autocomplete="true" mask="99:99" masktype="Time" culturename="en-us" messagevalidatortip="true"
                                                                                            runat="server">
                                                                                        </cc2:maskededitextender>
                                                                                        <cc2:maskededitextender id="txtUTCDepartureTimeMaskedEditExtender" targetcontrolid="txtUTCDepartureTime"
                                                                                            autocomplete="true" mask="99:99" masktype="Time" culturename="en-us" messagevalidatortip="true"
                                                                                            runat="server">
                                                                                        </cc2:maskededitextender>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblTakeOffStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                                        <asp:Label ID="lblUTCTakeOffStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblTakeOffLocalDateTime" runat="server" CssClass="clsLabelAuto">Take Off Date/Time</asp:Label>
                                                                                        <asp:Label ID="lblUTCTakeOffDateTime" runat="server" CssClass="clsLabelAuto">UTC Take Off Date/Time</asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:CheckBox ID="chkTakeOff" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
                                                                                            ToolTip="Check to enable Take Off Date." />
                                                                                        <asp:CheckBox ID="chkUTCTakeOff" runat="server" AutoPostBack="True" CssClass="clsCheckBox"
                                                                                            ToolTip="Check to enable Take Off Date." />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="calTakeOffLocalDateTime" CssClass="clsTextBoxTagSearch"
                                                                                            Width="90px" AutoPostBack="True" CausesValidation="True" BackColor="#E0E0E0"
                                                                                            ReadOnly="true" onchange="ValidateDateText(this,'calTakeOffLocalDateTime_watermarkextender');"></asp:TextBox>
                                                                                        <cc2:calendarextender id="calTakeOffLocalDateTime_CalendarExtender" runat="server"
                                                                                            cssclass="cal_Theme1" enabled="false" format="<%$AppSettings:DateFormat%>" targetcontrolid="calTakeOffLocalDateTime">
                                                                                        </cc2:calendarextender>
                                                                                        <cc2:textboxwatermarkextender targetcontrolid="calTakeOffLocalDateTime" id="calTakeOffLocalDateTime_watermarkextender"
                                                                                            clientidmode="Static" runat="server" watermarktext="<%$AppSettings:DateFormat%>"
                                                                                            watermarkcssclass="clsDateTextBox">
                                                                                        </cc2:textboxwatermarkextender>
                                                                                        <asp:TextBox runat="server" ID="calUTCTakeOffDateTime" CssClass="clsTextBoxTagSearch"
                                                                                            BackColor="#E0E0E0" ReadOnly="true" Width="90px" AutoPostBack="True" CausesValidation="True"
                                                                                            onchange="ValidateDateText(this,'calUTCTakeOffDateTime_watermarkextender');"></asp:TextBox>
                                                                                        <cc2:calendarextender id="calUTCTakeOffDateTime_CalendarExtender" runat="server"
                                                                                            cssclass="cal_Theme1" enabled="false" format="<%$AppSettings:DateFormat%>" targetcontrolid="calUTCTakeOffDateTime">
                                                                                        </cc2:calendarextender>
                                                                                        <cc2:textboxwatermarkextender targetcontrolid="calUTCTakeOffDateTime" id="calUTCTakeOffDateTime_watermarkextender"
                                                                                            clientidmode="Static" runat="server" watermarktext="<%$AppSettings:DateFormat%>"
                                                                                            watermarkcssclass="clsDateTextBox">
                                                                                        </cc2:textboxwatermarkextender>
                                                                                        <asp:TextBox ID="txtTakeOffLocalTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
                                                                                            MaxLength="10" ToolTip="Enter Take Off Time." Width="65px"></asp:TextBox>
                                                                                        <asp:TextBox ID="txtUTCTakeOffTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
                                                                                            MaxLength="10" TabIndex="7" ToolTip="Enter Take Off Time." Width="65px"></asp:TextBox>
                                                                                        <cc2:maskededitextender id="txtTakeOffLocalTimeMaskededitextender" targetcontrolid="txtTakeOffLocalTime"
                                                                                            autocomplete="true" mask="99:99" masktype="Time" culturename="en-us" messagevalidatortip="true"
                                                                                            runat="server">
                                                                                        </cc2:maskededitextender>
                                                                                        <cc2:maskededitextender id="txtUTCTakeOffTimeMaskededitextender" targetcontrolid="txtUTCTakeOffTime"
                                                                                            autocomplete="true" mask="99:99" masktype="Time" culturename="en-us" messagevalidatortip="true"
                                                                                            runat="server">
                                                                                        </cc2:maskededitextender>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </fieldset>
                                                                    </td>
                                                                    <td valign="top">
                                                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px; height: 120px">
                                                                            <legend style="font-weight: bold"><b>Arrival Info</b></legend>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblPlaceStar2" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblArrPlace" runat="server" CssClass="clsLabelAuto">Place</asp:Label>
                                                                                    </td>
                                                                                    <td></td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="Place2" runat="server" CssClass="clsTextBoxTagSearch" Width="150px"
                                                                                            Text="<%# mLogDetail.DestinationName %>" TabIndex="5"></asp:TextBox>
                                                                                        <asp:ImageButton ID="btnAddPlaces" runat="server" CausesValidation="False" Height="22px"
                                                                                            ImageUrl="~/images/plus1.png" ToolTip="Click to Add new Places" Width="24px" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="style3">
                                                                                        <asp:Label ID="lblUTCDateTimeStar2" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                                        <asp:Label ID="lblDateTimeStar2" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblArrDate" runat="server" CssClass="clsLabelAuto">Date/Time</asp:Label>
                                                                                        <asp:Label ID="lblUTCArrivalDateTime" runat="server" CssClass="clsLabelAuto">UTC DateTime</asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:CheckBox ID="chkArrival" runat="server" AutoPostBack="True" ToolTip="Check to enable Arrival Date"
                                                                                            CssClass="clsCheckBox" />
                                                                                        <asp:CheckBox ID="chkUTCArrival" runat="server" AutoPostBack="True" ToolTip="Check to enable Arrival Date"
                                                                                            CssClass="clsCheckBox" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="calArrival" CssClass="clsTextBoxTagSearch" Width="90px"
                                                                                            BackColor="#E0E0E0" ReadOnly="true" AutoPostBack="True" onchange="ValidateDateText(this,'calArrival_watermarkextender');"
                                                                                            CausesValidation="True" onfocus="onTextFocus();"></asp:TextBox>
                                                                                        <cc2:calendarextender id="calArrival_CalendarExtender" runat="server" cssclass="cal_Theme1"
                                                                                            enabled="false" format="<%$AppSettings:DateFormat%>" targetcontrolid="calArrival">
                                                                                        </cc2:calendarextender>
                                                                                        <cc2:textboxwatermarkextender targetcontrolid="calArrival" id="calArrival_watermarkextender"
                                                                                            clientidmode="Static" runat="server" watermarktext="<%$AppSettings:DateFormat%>"
                                                                                            watermarkcssclass="clsDateTextBox">
                                                                                        </cc2:textboxwatermarkextender>
                                                                                        <asp:TextBox runat="server" ID="CalUTCArrival" CssClass="clsTextBoxTagSearch" Width="90px"
                                                                                            BackColor="#E0E0E0" ReadOnly="true" AutoPostBack="True" CausesValidation="True"
                                                                                            onchange="ValidateDateText(this,'CalUTCArrival_watermarkextender');"></asp:TextBox>
                                                                                        <cc2:calendarextender id="CalUTCArrival_CalendarExtender" runat="server" cssclass="cal_Theme1"
                                                                                            enabled="false" format="<%$AppSettings:DateFormat%>" targetcontrolid="CalUTCArrival">
                                                                                        </cc2:calendarextender>
                                                                                        <cc2:textboxwatermarkextender targetcontrolid="CalUTCArrival" id="CalUTCArrival_watermarkextender"
                                                                                            clientidmode="Static" runat="server" watermarktext="<%$AppSettings:DateFormat%>"
                                                                                            watermarkcssclass="clsDateTextBox">
                                                                                        </cc2:textboxwatermarkextender>
                                                                                        <asp:TextBox ID="txtArrivalTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
                                                                                            MaxLength="10" ToolTip="Enter Arrival Time." Width="65px"></asp:TextBox>
                                                                                        <asp:TextBox ID="txtUTCArrivalTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
                                                                                            MaxLength="10" TabIndex="9" ToolTip="Enter Arrival Time." Width="65px"></asp:TextBox>
                                                                                        <cc2:maskededitextender id="txtArrivalTimeMaskedEditExtender" targetcontrolid="txtArrivalTime"
                                                                                            autocomplete="true" mask="99:99" masktype="Time" culturename="en-us" messagevalidatortip="true"
                                                                                            runat="server">
                                                                                        </cc2:maskededitextender>
                                                                                        <cc2:maskededitextender id="txtUTCArrivalTimeMaskedEditExtender" targetcontrolid="txtUTCArrivalTime"
                                                                                            autocomplete="true" mask="99:99" masktype="Time" culturename="en-us" messagevalidatortip="true"
                                                                                            runat="server">
                                                                                        </cc2:maskededitextender>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="style3">
                                                                                        <asp:Label ID="lblTouchDownStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                                        <asp:Label ID="lblUTCTouchDownStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblTouchDownLocalDateTime" runat="server" CssClass="clsLabelAuto">Touch Down Date/Time</asp:Label>
                                                                                        <asp:Label ID="lblUTCTouchDownDateTime" runat="server" CssClass="clsLabelAuto">UTC Touch Down Date/Time</asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:CheckBox ID="chkTouchDown" runat="server" AutoPostBack="True" ToolTip="Check to enable Touch Down Date."
                                                                                            CssClass="clsCheckBox" />
                                                                                        <asp:CheckBox ID="chkUTCTouchDown" runat="server" AutoPostBack="True" ToolTip="Check to enable Touch Down Date."
                                                                                            CssClass="clsCheckBox" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox runat="server" ID="calTouchDownLocalDateTime" CssClass="clsTextBoxTagSearch"
                                                                                            BackColor="#E0E0E0" ReadOnly="true" Width="90px" AutoPostBack="True" CausesValidation="True"
                                                                                            onchange="ValidateDateText(this,'calTouchDownLocalDateTime_watermarkextender');"></asp:TextBox>
                                                                                        <cc2:calendarextender id="calTouchDownLocalDateTime_CalendarExtender" runat="server"
                                                                                            cssclass="cal_Theme1" enabled="false" format="<%$AppSettings:DateFormat%>" targetcontrolid="calTouchDownLocalDateTime">
                                                                                        </cc2:calendarextender>
                                                                                        <cc2:textboxwatermarkextender targetcontrolid="calTouchDownLocalDateTime" id="calTouchDownLocalDateTime_watermarkextender"
                                                                                            clientidmode="Static" runat="server" watermarktext="<%$AppSettings:DateFormat%>"
                                                                                            watermarkcssclass="clsDateTextBox">
                                                                                        </cc2:textboxwatermarkextender>
                                                                                        <asp:TextBox runat="server" ID="calUTCTouchDownDateTime" CssClass="clsTextBoxTagSearch"
                                                                                            BackColor="#E0E0E0" ReadOnly="true" Width="90px" AutoPostBack="True" CausesValidation="True"
                                                                                            onchange="ValidateDateText(this,'calUTCTouchDownDateTime_watermarkextender');"></asp:TextBox>
                                                                                        <cc2:calendarextender id="calUTCTouchDownDateTime_CalendarExtender" runat="server"
                                                                                            cssclass="cal_Theme1" enabled="false" format="<%$AppSettings:DateFormat%>" targetcontrolid="calUTCTouchDownDateTime">
                                                                                        </cc2:calendarextender>
                                                                                        <cc2:textboxwatermarkextender targetcontrolid="calUTCTouchDownDateTime" id="calUTCTouchDownDateTime_watermarkextender"
                                                                                            clientidmode="Static" runat="server" watermarktext="<%$AppSettings:DateFormat%>"
                                                                                            watermarkcssclass="clsDateTextBox">
                                                                                        </cc2:textboxwatermarkextender>
                                                                                        <asp:TextBox ID="txtTouchDownLocalTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
                                                                                            MaxLength="10" ToolTip="Enter Touch Down Time." Width="65px"></asp:TextBox>
                                                                                        <asp:TextBox ID="txtUTCTouchDownTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
                                                                                            MaxLength="10" TabIndex="8" ToolTip="Enter Touch Down Time." Width="65px"></asp:TextBox>
                                                                                        <cc2:maskededitextender id="txtTouchDownLocalTimeMaskedEditExtender" targetcontrolid="txtTouchDownLocalTime"
                                                                                            autocomplete="true" mask="99:99" masktype="Time" culturename="en-us" messagevalidatortip="true"
                                                                                            runat="server">
                                                                                        </cc2:maskededitextender>
                                                                                        <cc2:maskededitextender id="txtUTCTouchDownTimeMaskedEditExtender" targetcontrolid="txtUTCTouchDownTime"
                                                                                            autocomplete="true" mask="99:99" masktype="Time" culturename="en-us" messagevalidatortip="true"
                                                                                            runat="server">
                                                                                        </cc2:maskededitextender>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </fieldset>
                                                                    </td>
                                                                    <td valign="top" align="right">
                                                                        <fieldset class="clsFieldSetNewStyle" style="border-width: 1px; height: 120px">
                                                                            <legend style="font-weight: bold"><b>Totals</b></legend>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblairfly" runat="server" CssClass="clsLabelAuto">Block Time</asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtBlockTime" runat="server" CssClass="clsTextBoxTagSearchSmall" Text="<%# mLogDetail.BlockTime %>"
                                                                                            Visible="False" Width="65px" TabIndex="10"></asp:TextBox>
                                                                                        <asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto">Hrs</asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblAirBorneTime" runat="server" CssClass="clsLabelAuto">Air Time </asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtAirBorneTime" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                                                            Text="<%# mLogDetail.TimeInAir %>" Visible="False" Width="65px" TabIndex="11"></asp:TextBox>
                                                                                        <asp:Label ID="Label2" runat="server" CssClass="clsLabelAuto">Hrs</asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblLandings" runat="server" CssClass="clsLabelAuto">Landings </asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtLandings" runat="server" CssClass="clsTextBoxTagSearchSmall" Text="<%# mLogDetail.Landings %>"
                                                                                            Visible="False" Width="65px" TabIndex="12"></asp:TextBox>
                                                                                        <asp:Label ID="Label3" runat="server" CssClass="clsLabelAuto">No(s)</asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </fieldset>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3">
                                                                        <cc2:tabcontainer id="tabLogDetailsContainer" runat="server"
                                                                            autopostback="true">
                                                                            <cc2:tabpanel id="tabLogFuel" runat="server" cssclass="clsPanel1" clientidmode="Static" width="100%">
                                                                                <headertemplate>
                                                                                    <asp:Label runat="server" Text="Fuel Info ( KG / LBS )" ID="Label7"></asp:Label>
                                                                                </headertemplate>
                                                                                <contenttemplate>
                                                                                    <table width="70%">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="lblFuelOnDeparture" runat="server" CssClass="clsLabelAuto">Fuel On Board</asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtFuelOnDeparture" runat="server" CssClass="clsTextBoxTagSearchSmall" AutoPostBack="true"
                                                                                                    Text="<%# mLogDetail.FuelOnDeparture %>" TabIndex="13"></asp:TextBox>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="Label4" runat="server" CssClass="clsLabelAuto">Fuel Added</asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtFuelOnAdded" runat="server" CssClass="clsTextBoxTagSearchSmall" TabIndex="14"
                                                                                                    Text="<%# mLogDetail.FuelOnAdded %>"></asp:TextBox>&nbsp;<asp:Label ID="lblFuelWt"
                                                                                                        runat="server" CssClass="clsLabelAuto">(Litre)</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lblFuelUplifted" runat="server" CssClass="clsLabelAuto">Fuel Uplifted</asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtFuelUplifted" runat="server" CssClass="clsTextBoxTagSearchSmall" AutoPostBack="true"
                                                                                                    Text="<%# mLogDetail.FuelUplifted %>"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="lblTotalFuelOnDeparture" runat="server" CssClass="clsLabelAuto">Fuel On Departure</asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtTotalFuelOnDeparture" runat="server" CssClass="clsTextBoxTagSearchSmall" AutoPostBack="true" TabIndex="15"
                                                                                                    Text="<%# mLogDetail.TotalFuelOnDeparture %>"></asp:TextBox>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lblFuelOnArrival" runat="server" CssClass="clsLabelAuto">Fuel On Arrival</asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtFuelOnArrival" runat="server" CssClass="clsTextBoxTagSearchSmall" AutoPostBack="true" TabIndex="16"
                                                                                                    Text="<%# mLogDetail.FuelOnArrival %>"></asp:TextBox>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lblFuelConsumption" runat="server" CssClass="clsLabelAuto">Fuel Consumption</asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtFuelConsumption" runat="server" CssClass="clsTextBoxTagSearchSmall" BackColor="Gainsboro"
                                                                                                    ReadOnly="True" Text="<%# mLogDetail.FuelConsumption %>"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </contenttemplate>
                                                                            </cc2:tabpanel>
                                                                            <cc2:tabpanel id="tabWeight" runat="server" cssclass="clsPanel1" clientidmode="Static" width="100%">
                                                                                <headertemplate>
                                                                                    Weight Info
                                                                                </headertemplate>
                                                                                <contenttemplate>
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="lblPaxAdult" runat="server" CssClass="clsLabelAuto">Pax Adult</asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtPaxAdult" runat="server" CssClass="clsTextBoxTagSearchSmall" MaxLength="6"
                                                                                                    Text="<%# mLogDetail.PaxAdult %>"></asp:TextBox>
                                                                                            </td>
                                                                                            <td>
                                                                                                <div class="clsLabelAuto">
                                                                                                    No(s)
                                                                                                </div>
                                                                                            </td>
                                                                                            <td>&nbsp; &nbsp; &nbsp; &nbsp;  &nbsp; &nbsp; 
                                                                                                <asp:Label ID="lblPaxChild" runat="server" Width="50px" CssClass="clsLabelAuto">Pax Child</asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtPaxChild" runat="server" CssClass="clsTextBoxTagSearchSmall" MaxLength="4"
                                                                                                    Text="<%# mLogDetail.PaxChild %>"></asp:TextBox>
                                                                                            </td>
                                                                                            <td>
                                                                                                <div class="clsLabelAuto">
                                                                                                    No(s)
                                                                                                </div>
                                                                                            </td>
                                                                                            <td>&nbsp; &nbsp; &nbsp; &nbsp;  &nbsp; &nbsp;
                                                                                                <asp:Label ID="lblPaxInfant" runat="server" CssClass="clsLabelAuto">Pax Infant</asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtPaxInfant" runat="server" CssClass="clsTextBoxTagSearchSmall" MaxLength="4"
                                                                                                    Text="<%# mLogDetail.PaxInfant %>"></asp:TextBox>
                                                                                            </td>
                                                                                            <td>
                                                                                                <div class="clsLabelAuto">
                                                                                                    No(s)
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="lblCargoWeight" runat="server" Width="90px"
                                                                                                    CssClass="clsLabelAuto" Text="Cargo Weight" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtCargoWeight" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                                                                    MaxLength="10" Text="<%# mLogDetail.CargoWeight %>" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <div class="clsLabelAuto">
                                                                                                    KG / LBS
                                                                                                </div>
                                                                                            </td>
                                                                                            <td>&nbsp; &nbsp; &nbsp; &nbsp;  &nbsp; &nbsp;   
                                                                                                <asp:Label ID="lblTakeOffWeight" runat="server"
                                                                                                    CssClass="clsLabelAuto" Text="TakeOff Weight" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtTakeOffWeight" runat="server"
                                                                                                    CssClass="clsTextBoxTagSearchSmall"
                                                                                                    MaxLength="10" Text="<%# mLogDetail.TakeOffWeight %>" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <div class="clsLabelAuto">
                                                                                                    KG / LBS
                                                                                                </div>
                                                                                            </td>
                                                                                            <td>&nbsp; &nbsp; &nbsp; &nbsp;  &nbsp; &nbsp;   
                                                                                                <asp:Label ID="lblExtraBaggage" runat="server"
                                                                                                    CssClass="clsLabelAuto" Text="Extra Baggage (EB)" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtExtraBaggage" runat="server"
                                                                                                    CssClass="clsTextBoxTagSearchSmall"
                                                                                                    MaxLength="10" Text="<%# mLogDetail.ExtraBaggage %>" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <div class="clsLabelAuto">
                                                                                                    KG / LBS
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>

                                                                                </contenttemplate>
                                                                            </cc2:tabpanel>
                                                                            <cc2:tabpanel id="TabEngineOil" runat="server" cssclass="clsPanel1" clientidmode="Static" width="100%">
                                                                                <headertemplate>
                                                                                    Engine Oil
                                                                                </headertemplate>
                                                                                <contenttemplate>
                                                                                    <table>
                                                                                        <tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Label ID="Label5" runat="server" CssClass="clsLabelAuto">LH ENGINE</asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtLHEngineOil" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                                                                        Text="<%# mLogDetail.LHEngineOil %>"></asp:TextBox>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="Label6" runat="server" CssClass="clsLabelAuto">RH ENGINE</asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtRHEngineOil" runat="server" CssClass="clsTextBoxTagSearchSmall"
                                                                                                        Text="<%# mLogDetail.RHEngineOil %>"></asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                            </tr>
                                                                                        </tr>
                                                                                    </table>

                                                                                </contenttemplate>
                                                                            </cc2:tabpanel>
                                                                            <cc2:tabpanel id="TabPFI" runat="server" cssclass="clsPanel1" clientidmode="Static" width="100%">
                                                                                <headertemplate>
                                                                                    PFI (Pre-Flight Inspection)
                                                                                </headertemplate>
                                                                                <contenttemplate>
                                                                                    <asp:UpdatePanel ID="upnlEmp" runat="server" UpdateMode="Conditional">
                                                                                        <contenttemplate>
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:ValidationSummary ID="Validationsummary1" runat="server" CssClass="clsValidationSummary" ValidationGroup="a"
                                                                                                            HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                                                                                        <asp:CustomValidator ID="cvSearch" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtEmployee" ValidationGroup="a"
                                                                                                            ValidateEmptyText="true" Display="None" ErrorMessage="Enter Done By Employee"
                                                                                                            OnServerValidate="customvalidate"></asp:CustomValidator>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <span class="clsLabelAuto">PFI Done</span>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:CheckBox ID="chkIsPFIDone" runat="server" ToolTip="Check if PFI is done" CssClass="clsCheckBox"
                                                                                                            AutoPostBack="true" OnCheckedChanged="chkIsPFI_CheckChanged" ClientIDMode="Static"
                                                                                                            Checked="<%# mLogDetail.IsPFIDone  %>" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <span class="clsLabelAuto">PFI Done By</span>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="txtEmployee" runat="server" AutoComplete="off" ClientIDMode="Static"
                                                                                                            AutoPostBack="true" OnTextChanged="txtEmployee_TextChanged" CssClass="clsTextBoxTagSearch"></asp:TextBox>
                                                                                                        <cc2:autocompleteextender clientidmode="Static" id="txtEmployee_Autocomplete" runat="server"
                                                                                                            delimitercharacters="" enabled="True" completionsetcount="20" minimumprefixlength="0"
                                                                                                            completioninterval="1" servicepath="" servicemethod="GetEmployeeList" targetcontrolid="txtEmployee"
                                                                                                            usecontextkey="False" contextkey="" completionlistcssclass="ac_results_Main"
                                                                                                            completionlistitemcssclass="ac_results_li" completionlisthighlighteditemcssclass="ac_over_Main"
                                                                                                            onclientpopulated="ClientPopulated" onclientpopulating="ClientPopulating" onclienthiding="ClientHiding"
                                                                                                            onclientshown="ClientHiding" onclientshowing="ClientShowing">
                                                                                                        </cc2:autocompleteextender>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </contenttemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </contenttemplate>
                                                                            </cc2:tabpanel>
                                                                        </cc2:tabcontainer>
                                                                    </td>

                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblTLPGridTitle" runat="server" CssClass="clsLabelHeader">TLP Details</asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td align="right">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Button ID="btnAdd" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Add new record" TabIndex="17"
                                                    Text="Add"></asp:Button>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:GridView ID="dgLogTLPDetails" runat="server" CellPadding="5" ForeColor="Black" GridLines="Horizontal" CssClass="clsGridNewStyle" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true">
                                                    <AlternatingRowStyle CssClass="clsdgAltItem"></AlternatingRowStyle>
                                                    <RowStyle CssClass="clsdgItem"></RowStyle>
                                                    <HeaderStyle BackColor="white" CssClass="clsdgHeader" Font-Bold="True" ForeColor="black" />
                                                    <Columns>
                                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="ID "></asp:BoundField>
                                                        <asp:BoundField DataField="SrNo" HeaderText="Sr No."></asp:BoundField>
                                                        <asp:BoundField DataField="FlightNo" SortExpression="FlightNo" HeaderText="Flight No."></asp:BoundField>
                                                        <asp:BoundField DataField="SourceName" SortExpression="SourceName" HeaderText="From"></asp:BoundField>
                                                        <asp:BoundField DataField="DestinationName" SortExpression="DestinationName" HeaderText="To"></asp:BoundField>
                                                        <asp:BoundField DataField="SouLocalDateTimeFormatted" SortExpression="SouLocalDateTimeFormatted"
                                                            HeaderText="Chocks Off">
                                                            <ItemStyle Wrap="false" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SouUniverseDateTimeFormatted" SortExpression="SouUniverseDateTimeFormatted"
                                                            HeaderText="UTC Chocks Off">
                                                            <ItemStyle Wrap="false" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="DesLocalDateTimeFormatted" SortExpression="DesLocalDateTimeFormatted"
                                                            HeaderText="Chocks On">
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="DesUniverseDateTimeFormatted" SortExpression="DesUniverseDateTimeFormatted"
                                                            HeaderText="UTC Chocks On">
                                                            <ItemStyle Wrap="false" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="BlockTime" HeaderText="Block Time"></asp:BoundField>
                                                        <asp:BoundField DataField="TakeOffLocalDateTimeFormatted" SortExpression="TakeOffLocalDateTimeFormatted"
                                                            HeaderText="Take Off">
                                                            <ItemStyle Wrap="false" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="TakeOffUniverseDateTimeFormatted" SortExpression="TakeOffUniverseDateTimeFormatted"
                                                            HeaderText="UTC Take Off">
                                                            <ItemStyle Wrap="false" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="TouchDownLocalDateTimeFormatted" SortExpression="TouchDownLocalDateTimeFormatted"
                                                            HeaderText="Touch Down">
                                                            <ItemStyle Wrap="false" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="TouchDownUniverseDateTimeFormatted" SortExpression="TouchDownUniverseDateTimeFormatted"
                                                            HeaderText="UTC Touch Down">
                                                            <ItemStyle Wrap="false" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="TimeInAir" HeaderText="Flight Time"></asp:BoundField>
                                                        <asp:BoundField DataField="Landings" SortExpression="Landings" HeaderText="Landings"></asp:BoundField>
                                                        <asp:BoundField DataField="FuelOnDeparture" HeaderText="Fuel On Board"></asp:BoundField>
                                                        <asp:BoundField DataField="FuelUplifted" HeaderText="Fuel Uplifted"></asp:BoundField>
                                                        <asp:BoundField DataField="FuelOnArrival" HeaderText="Fuel Arr."></asp:BoundField>
                                                        <asp:BoundField DataField="Pax" HeaderText="Pax" HeaderStyle-CssClass="hideGridColumn"
                                                            ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                        <asp:BoundField DataField="CargoWeight" HeaderText="Cargo" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                        <asp:BoundField DataField="TakeOffWeight" HeaderText="Take Off Weight" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                        <asp:TemplateField HeaderText="PFI Done" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn">
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "IsPFIDone") %>'
                                                                    Enabled="False"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="PFIDoneByEmpNoName" SortExpression="PFIDoneByEmpNoName" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"
                                                            HeaderText="PFI Done By">
                                                            <ItemStyle Wrap="false" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                            <%--13--%>
                                                            <ItemTemplate>

                                                                <div class="dropdown">

                                                                    <asp:Image ID="lnkArrow" ImageUrl="~/images/Arrowup.png" runat="server" CssClass="clsActionbtn"
                                                                        Style="cursor: pointer;" />
                                                                    <div class="dropdownbtn-content">
                                                                        <table id="T1" class="clsGridNew_Ajax" style="z-index: 7; position: relative;">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:ImageButton ID="EditRec" runat="server" CommandArgument='<%# Eval("SrNo") %>'
                                                                                        CommandName="EditRec" Style="height: 15px; width: 15px" ImageUrl="~/images/edit.png"
                                                                                        CausesValidation="false" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:ImageButton ID="DeleteRecord" runat="server" CommandArgument='<%# Eval("SrNo") %>'
                                                                                        CommandName="RemoveRec" Style="height: 20px; width: 20px" ImageUrl="~/images/delete.png"
                                                                                        CausesValidation="false" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>

                                                </asp:GridView>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2">
                                        <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH1" Text="Print" Visible="False"
                                                                CausesValidation="False"></asp:Button>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Back to Previous Page" TabIndex="18"
                                                                Text="Back" CausesValidation="false"></asp:Button>
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

            <div id="divSpinner">

                <asp:UpdateProgress ID="AjaxLoader" DisplayAfter="600" DynamicLayout="false" runat="server">
                    <ProgressTemplate>
                        <div class="clsAjaxLoader">
                        </div>
                        <div class="divAjaxLoader">
                            <div class="ext-el-mask-msg x-mask-loading">
                                <div class="clsLoad_ajax">
                                    <asp:Image ID="ajaxloadergif" runat="server" ImageUrl="~/images/Loader.gif"
                                        ImageAlign="Middle" CssClass="ajax-loader-gif" />
                                </div>
                            </div>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>

            </div>

        </div>

        <!-- Place Popup -->
        <div id="popup">
            
            <div style="display: none">
                <asp:Button runat="server" ID="btnDummyPlace" Text="Dummy Place" ClientIDMode="Static" />
            </div>
            <asp:Panel runat="server" ID="pnlPlace" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
                <iframe id="iPopupPlace" frameborder="0" allowtransparency="true" height="100%"
                    width="100%" src="JavaScript:''" scrolling="auto"></iframe>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="mdlPopupPlace" runat="server" TargetControlID="btnDummyPlace"
                PopupControlID="pnlPlace" BackgroundCssClass="clsModalPopupBG">
            </cc2:ModalPopupExtender>
            <script type="text/javascript">
                function IFramePlaceComplete() {
                    $("#btnDummyPlace").click();
                    $get("AjaxLoader").style.visibility = "hidden";
                }

                function OpenPlaceWindow(BackPagetmp) {
                    try {
                        $get("AjaxLoader").style.visibility = "visible";
                        $("#iPopupPlace").attr("src", "wfPlace_Ajax.aspx?Type=Place&AddType=2&BackPage=BackPagetmp&BackPage1=wfLogSOP_Ajax.aspx&Typepup=pup");
                        if (!$.browser.msie) {
                            $("#btnDummyPlace").click();
                            $get("AjaxLoader").style.visibility = "hidden";
                        }

                        return false;
                    } catch (e) {
                        alert(e);
                    }

                }
            </script>
            <script type="text/javascript">
                function ParentCallBackPlaceFunction() {
                    var atawindow = $find("<%=mdlPopupPlace.ClientID %>");
                    //close ata popup window
                    atawindow.hide();
                    $("#iPopupPlace").attr("src", "JavaScript:''");
                    //call ata image button
                    $("#hdnimgBtnPlace").click();
                }
            </script>
            <!-------------------->

        </div>

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
            }
        </script>
        <%--End--%>

        <script type="text/javascript">
            var Enable = function () {
                var IsPFIDoneChecked = $get("chkIsPFIDone").checked;
                if (IsPFIDoneChecked) {
                    $("[id$='txtEmployee']").attr('disabled', false);

                }
                else {
                    $("[id$='txtEmployee']").attr('disabled', true);
                    $("[id$='txtEmployee']").val('');

                }
            }
        </script>

    </form>

    <!--  For Arrival,Departure,Take off,Touch down Date Time   -->
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            var savedlog1;
            if ("<%=mLogDetail.IsNew%>" == "True") {
                if ("<%=mLog.IsUTC%>" == "True" && document.getElementById('chkUTCArrival').checked) {
                    savedlog1 = "button";
                }
                else if ("<%=mLog.IsUTC%>" == "False" && document.getElementById('chkArrival').checked) {
                    savedlog1 = "button";
                }
                else {
                    savedlog1 = "";
                }
            }
            else {
                savedlog1 = "";
            }
        });

    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            var savedlog1;
            if ("<%=mLogDetail.IsNew%>" == "True") {
                if ("<%=mLog.IsUTC%>" == "True" && document.getElementById('chkUTCTouchDown').checked == true) {
                    savedlog1 = "button";
                }
                else if ("<%=mLog.IsUTC%>" == "False" && document.getElementById('chkTouchDown').checked == true) {
                    savedlog1 = "button";
                }
                else {
                    savedlog1 = "";
                }
            }
            else {
                savedlog1 = "";
            }



        }); 
		});

    </script>

    <!-- Autocomplete for Source and Destination Place   -->
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=Place1.ClientID%>,#<%=Place2.ClientID%>").autocomplete('wfAutoPilotPlace.aspx?Type=Place', {
                width: 250,
                autoFill: true,
                matchContains: true,
                delay: 0

            });
        });
    </script>
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

</body>
</html>
