<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfLogSOPEdit_Ajax.aspx.vb"
    Inherits="Flypal.wfLogSOPEdit_Ajax" %>

<%@ Import Namespace="Flypal.LogList" %>
<%@ Import Namespace="Flypal.Log" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%--AJAX- Changed DOCTYPE from 4.0 to 1.0--%>
<%--AJAX- Register "AjaxControlToolkit & User Control "MSGBOX"--%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Log Details</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <asp:PlaceHolder runat="server">
        <%--AJAX- Replaced "LocalFunction.htm" to "LocalFunctionAjax.htm"--%>
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script id="clientEventHandlersJS" language="javascript">
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
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script src="StickyNote/js/jquery.cookie.js" type="text/javascript"></script>
</head>
<body bottommargin="5" leftmargin="5" rightmargin="5" topmargin="5">
    <form id="form1" runat="server" enctype="multipart/form-data" method="post">
    <%--AJAX- ScriptManager Added--%>
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
        runat="server">
    </asp:ScriptManager>
    <%--AJAX- New function added as Focus gets Lost when we use tabs in Grid--%>
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
    <%--AJAX- Add MSGBox Control--%>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table class="clstablelistout" id="tblMain">
            <tr>
                <td>
                    <%--AJAX- Add UpdatePanel for lblTitle Page--%>
                    <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblTitle" runat="server" CssClass="clstitle1">Log Details</asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlTabs" runat="server" UpdateMode="Conditional">
                        <%--AJAX- Add UpdatePanel for tabs buttons --%>
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblLogDetails" runat="server" CssClass="clsLabelButton" ToolTip="Log details">Log details</asp:Label>
                                                </td>
                                                <%--<td>
                                                    <asp:Button ID="btnFuelOil" runat="server" CssClass="clsButtonLong_Ajax" CausesValidation="False"
                                                        Text="Fuel Oil"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnDefectActionList" runat="server" CssClass="clsButtonLong_Ajax"
                                                        CausesValidation="False" Text='<%# iif(AppSettings("MELSnagNomenclature") = "True","Defect Reporting","Snag Reporting") %>'>
                                                    </asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnParameterList" runat="server" CssClass="clsButtonLong_Ajax" CausesValidation="False"
                                                        Text="Parameter List"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnLogPax" runat="server" CssClass="clsButtonLong_Ajax" CausesValidation="False"
                                                        Visible='<%# iif(AppSettings("ShowExtraLogTabs") = "True",True,False) %>' Text="Passenger Log">
                                                    </asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnHobbsOffset" runat="server" CssClass="clsButtonLong_Ajax" CausesValidation="False"
                                                        Visible='<%# iif(AppSettings("ShowExtraLogTabs") = "True",True,False) %>' Text="Hobbs Offset">
                                                    </asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnFlightCrew" runat="server" CssClass="clsButtonLong_Ajax" CausesValidation="False"
                                                        Text="Flight Crew"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button Style="z-index: 0" ID="btnMaintenanceAcitvity" runat="server" CssClass="clsButtonLong_Ajax"
                                                        CausesValidation="False" Text="Maintenance Activity"></asp:Button>
                                                </td>--%>
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
                    <%--AJAX- Add UpdatePanel for ValidationSummary or ErrorList --%>
                    <asp:UpdatePanel ID="upnlErrorList" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel runat="server">
                                <asp:ValidationSummary ID="Validationsummary2" CssClass="clsValidationSummary" runat="server"
                                    HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                <asp:CustomValidator ID="cvRemark" runat="server" ErrorMessage="Remark Can't be greater than 200 chars"
                                    ControlToValidate="txtRemark" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvAirFrame" runat="server" Display="None" OnServerValidate="customvalidate1"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvGroundRunTime" runat="server" ErrorMessage="Departure date should be in date time format."
                                    ControlToValidate="txtGroundRunTime" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvAirBornTime" runat="server" ErrorMessage="Not be Nigative."
                                    ControlToValidate="txtAirBorneTime" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvPilot1" runat="server" ErrorMessage="Enter correct Pilot1 name."
                                    ControlToValidate="Pilot1" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvPilot2" runat="server" ErrorMessage="Enter correct Pilot2 name."
                                    ControlToValidate="Pilot2" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvPlace1" runat="server" ErrorMessage="Enter correct Source name."
                                    ControlToValidate="Place1" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                <asp:CustomValidator ID="cvPlace2" runat="server" ErrorMessage="Enter correct Destination name."
                                    ControlToValidate="Place2" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <%--AJAX- Add UpdatePanel for log Details --%>
                    <asp:UpdatePanel ID="upnlLogDetails" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblCalDate" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDateTime" runat="server" CssClass="clsLabelAuto">Date</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="calDateTime" CssClass="clsTextBox_Ajax" Width="100px"
                                                        AutoPostBack="true" onchange="ValidateDateText(this,'calDateTime_watermarkextender');"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="calDateTime_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="calDateTime">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="calDateTime" ID="calDateTime_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox">
                                                    </cc2:TextBoxWatermarkExtender>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblLogNo" runat="server" CssClass="clsLabelAuto">Log No.</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtLogText" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxDate_Ajax"
                                                        ReadOnly="True" Text="<%# mLog.LogText %>" ToolTip="Log Number"></asp:TextBox>
                                                    <asp:TextBox ID="txtLogNo" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxMedium_Ajax"
                                                        ReadOnly="True" Text="<%# mLog.LogNo %>"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblPilotStar1" runat="server" CssClass="clsLabelStar" Visible="<%# not mLog.IsHobbs %>">*</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblPilotComm" runat="server" CssClass="clsLabelAuto">Pilot in Command</asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="Pilot1" runat="server" CssClass="clsTextBoxDate_Ajax" Text="<%# mLog.Pilot1Name %>"
                                                        Width="250px"></asp:TextBox>
                                                    <asp:TextBox ID="txtPilot1" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxDate_Ajax"
                                                        ReadOnly="True" Text="<%# mLog.Pilot1Name %>" ToolTip="Pilot #1 Name" Visible="False"
                                                        Width="250px"></asp:TextBox>
                                                    <asp:ImageButton ID="imgbtnPilot1" runat="server" CausesValidation="False" CssClass="clsButtonImg"
                                                        ImageUrl="ICONS/ADD.ICO" ToolTip="Select Pilot #1" Visible="False" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="icons/CLIP01.ICO" Visible="False"
                                                        Width="24px" CssClass="clsButtonImg_Ajax" />
                                                </td>
                                                <td>
                                                    <span id="lblAttachFile" class="clsLabelAuto">Attach File</span>
                                                </td>
                                                <td colspan="3">
                                                    <asp:UpdatePanel ID="upnlFileupload" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <input type="button" id="btnSelectFile" value="Select File" style="width: 100px;"
                                                                            runat="server" class="clsButton_Ajax" causesvalidation="False" />
                                                                    </td>
                                                                    <td style="padding-left: 3px;">
                                                                        <asp:Button ID="btnDelAttch" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Remove Attachment"
                                                                            Text="Remove Attachment" Enabled="False" Width="120px"></asp:Button>
                                                                    </td>
                                                                    <td style="padding-left: 2px;">
                                                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
                                                                            Height="20px" Width="20px"></asp:ImageButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblLogPageNo" runat="server" CssClass="clsLabelAuto">Page No.</asp:Label>
                                                </td>
                                                <td valign="middle">
                                                    <table width="250px">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtLogPageNo" runat="server" CssClass="clsTextBoxMedium_Ajax" MaxLength="9"
                                                                    Text="<%# mLog.LogPageNoFormatted %>" ToolTip="Enter Log Page No."></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblFlightNo" runat="server" CssClass="clsLabelAuto">Flight No.</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtFlightNo" runat="server" CssClass="clsTextBoxMedium_Ajax" MaxLength="10"
                                                                    Text="<%# mLog.FlightNo %>" ToolTip="Enter Flight No."></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblCo" runat="server" CssClass="clsLabelAuto">Co-Pilot</asp:Label>
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="Pilot2" runat="server" CssClass="clsTextBoxDate_Ajax" Width="254px"
                                                                    Text="<%# mLog.Pilot2Name %>" ToolTip="Pilot #2 Name"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="btnAddPilots" runat="server" CausesValidation="False" Height="20px" Visible="false" 
                                                                    ImageUrl="~/images/plus1.png" ToolTip="Click to Add new pilot" Width="24px" />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="imgbtnPilot2" runat="server" CausesValidation="False" CssClass="clsButtonImg_Ajax"
                                                                    ImageUrl="ICONS/ADD.ICO" ToolTip="Select Pilot #2 Name" Visible="False" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblFlightLogClassification" runat="server" CssClass="clsLabelAuto">Classification</asp:Label>
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="cmbFlightLogClassification" runat="server" CssClass="clsComboBox_Ajax"
                                                                    DataTextField="Name" DataValueField="ID" Width="258px">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="btnFlightLogClassifications" runat="server" CausesValidation="False" Visible="false" 
                                                                    Height="20px" ImageUrl="~/images/plus1.png" ToolTip="Click to Add new Classification"
                                                                    Width="24px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <%--As Page partialy PostBack Object(Log) value doesn't reflects in HTML. So Put Object values in Hidden Field and use it in HTML JQuery--%>
                            <input type="hidden" id="LogObjValue" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <%--AJAX- Add UpdatePanel for Flight Details --%>
                    <asp:UpdatePanel ID="upnlFlightDetails" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td valign="top">
                                        <table width="100%">
                                            <tr>
                                                <td colspan="5">
                                                    <asp:Label ID="lblDeparture" runat="server" CssClass="clstitle2_ajax">Departure</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDepPlace" runat="server" CssClass="clsLabelAuto">Place</asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:TextBox ID="Place1" runat="server" CssClass="clsTextBoxDate_Ajax" Width="200px"
                                                        Text="<%# mLog.SourceName %>"></asp:TextBox>
                                                    <asp:TextBox ID="txtDepPlace" runat="server" CssClass="clsTextBoxDate_Ajax" Width="168px"
                                                        BackColor="#E0E0E0" ReadOnly="True" Text="<%# mLog.SourceName %>" ToolTip="Place"
                                                        Visible="False"></asp:TextBox>
                                                    <asp:Button ID="btnAddDepPlace" runat="server" CssClass="clsButtonGrid_Ajax" Text="..."
                                                        ToolTip="Click to add New Place" Visible="False" />
                                                    <asp:ImageButton ID="imgbtnDepPlace" runat="server" CausesValidation="False" CssClass="clsButtonImg_Ajax"
                                                        Enabled="<%# mLog.IsNew %>" ImageUrl="ICONS/ADD.ICO" ToolTip="Select Place" Visible="False" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDepDateTime" runat="server" CssClass="clsLabelAuto">Date/Time</asp:Label>
                                                    <asp:Label ID="lblUTCDateTime" runat="server" CssClass="clsLabelAuto">UTC 
                                                                Date/Time</asp:Label>
                                                </td>
                                                <td>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:TextBox runat="server" ID="calDeparture" CssClass="clsTextBox_Ajax" Width="100px"
                                                        BackColor="#E0E0E0" ReadOnly="true" AutoPostBack="true" CausesValidation="True"
                                                        onchange="ValidateDateText(this,'calDeparture_watermarkextender');"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="calDeparture_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="false" Format="<%$AppSettings:DateFormat%>" TargetControlID="calDeparture">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="calDeparture" ID="calDeparture_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox">
                                                    </cc2:TextBoxWatermarkExtender>
                                                    <asp:TextBox runat="server" ID="CalUTCDateTime" CssClass="clsTextBox_Ajax" Width="100px"
                                                        BackColor="#E0E0E0" ReadOnly="true" AutoPostBack="True" CausesValidation="True"
                                                        onchange="ValidateDateText(this,'CalUTCDateTime_watermarkextender');"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="CalUTCDateTime_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="false" Format="<%$AppSettings:DateFormat%>" TargetControlID="CalUTCDateTime">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="CalUTCDateTime" ID="CalUTCDateTime_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox">
                                                    </cc2:TextBoxWatermarkExtender>
                                                    <asp:TextBox ID="txtDepartureTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxMedium_Ajax"
                                                        MaxLength="10" ToolTip="Enter Departure Time." onfocus="onTextFocus();"></asp:TextBox>
                                                    <asp:TextBox ID="txtUTCDepartureTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxMedium_Ajax"
                                                        MaxLength="10" ToolTip="Enter UTC Departure Time."></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDepDayLightTime" runat="server" CssClass="clsLabelAuto" Visible="False">D/L Time</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbDepartureDayLightTime" runat="server" CssClass="clsComboBoxsmall_Ajax"
                                                        DataTextField="Name" DataValueField="ID" SelectedValue="<%# mLog.SouDayLightTime %>"
                                                        Visible="False">
                                                        <asp:ListItem Selected="True" Value="-12:00">-12:00</asp:ListItem>
                                                        <asp:ListItem Value="-11:45">-11:45</asp:ListItem>
                                                        <asp:ListItem Value="-11:30">-11:30</asp:ListItem>
                                                        <asp:ListItem Value="-11:15">-11:15</asp:ListItem>
                                                        <asp:ListItem Value="-11:00">-11:00</asp:ListItem>
                                                        <asp:ListItem Value="-10:45">-10:45</asp:ListItem>
                                                        <asp:ListItem Value="-10:30">-10:30</asp:ListItem>
                                                        <asp:ListItem Value="-10:15">-10:15</asp:ListItem>
                                                        <asp:ListItem Value="-10:00">-10:00</asp:ListItem>
                                                        <asp:ListItem Value="-09:45">-09:45</asp:ListItem>
                                                        <asp:ListItem Value="-09:30">-09:30</asp:ListItem>
                                                        <asp:ListItem Value="-09:15">-09:15</asp:ListItem>
                                                        <asp:ListItem Value="-09:00">-09:00</asp:ListItem>
                                                        <asp:ListItem Value="-08:45">-08:45</asp:ListItem>
                                                        <asp:ListItem Value="-08:30">-08:30</asp:ListItem>
                                                        <asp:ListItem Value="-08:15">-08:15</asp:ListItem>
                                                        <asp:ListItem Value="-08:00">-08:00</asp:ListItem>
                                                        <asp:ListItem Value="-07:45">-07:45</asp:ListItem>
                                                        <asp:ListItem Value="-07:30">-07:30</asp:ListItem>
                                                        <asp:ListItem Value="-07:15">-07:15</asp:ListItem>
                                                        <asp:ListItem Value="-07:00">-07:00</asp:ListItem>
                                                        <asp:ListItem Value="-06:45">-06:45</asp:ListItem>
                                                        <asp:ListItem Value="-06:30">-06:30</asp:ListItem>
                                                        <asp:ListItem Value="-06:15">-06:15</asp:ListItem>
                                                        <asp:ListItem Value="-06:00">-06:00</asp:ListItem>
                                                        <asp:ListItem Value="-05:45">-05:45</asp:ListItem>
                                                        <asp:ListItem Value="-05:30">-05:30</asp:ListItem>
                                                        <asp:ListItem Value="-05:15">-05:15</asp:ListItem>
                                                        <asp:ListItem Value="-05:00">-05:00</asp:ListItem>
                                                        <asp:ListItem Value="-04:45">-04:45</asp:ListItem>
                                                        <asp:ListItem Value="-04:30">-04:30</asp:ListItem>
                                                        <asp:ListItem Value="-04:15">-04:15</asp:ListItem>
                                                        <asp:ListItem Value="-04:00">-04:00</asp:ListItem>
                                                        <asp:ListItem Value="-03:45">-03:45</asp:ListItem>
                                                        <asp:ListItem Value="-03:30">-03:30</asp:ListItem>
                                                        <asp:ListItem Value="-03:15">-03:15</asp:ListItem>
                                                        <asp:ListItem Value="-03:00">-03:00</asp:ListItem>
                                                        <asp:ListItem Value="-02:45">-02:45</asp:ListItem>
                                                        <asp:ListItem Value="-02:30">-02:30</asp:ListItem>
                                                        <asp:ListItem Value="-02:15">-02:15</asp:ListItem>
                                                        <asp:ListItem Value="-02:00">-02:00</asp:ListItem>
                                                        <asp:ListItem Value="-01:45">-01:45</asp:ListItem>
                                                        <asp:ListItem Value="-01:30">-01:30</asp:ListItem>
                                                        <asp:ListItem Value="-01:15">-01:15</asp:ListItem>
                                                        <asp:ListItem Value="-01:00">-01:00</asp:ListItem>
                                                        <asp:ListItem Value="-00:45">-00:45</asp:ListItem>
                                                        <asp:ListItem Value="-00:30">-00:30</asp:ListItem>
                                                        <asp:ListItem Value="-00:15">-00:15</asp:ListItem>
                                                        <asp:ListItem Value="+00:00">+00:00</asp:ListItem>
                                                        <asp:ListItem Value="+00:15">+00:15</asp:ListItem>
                                                        <asp:ListItem Value="+00:30">+00:30</asp:ListItem>
                                                        <asp:ListItem Value="+00:45">+00:45</asp:ListItem>
                                                        <asp:ListItem Value="+01:00">+01:00</asp:ListItem>
                                                        <asp:ListItem Value="+01:15">+01:15</asp:ListItem>
                                                        <asp:ListItem Value="+01:30">+01:30</asp:ListItem>
                                                        <asp:ListItem Value="+01:45">+01:45</asp:ListItem>
                                                        <asp:ListItem Value="+02:00">+02:00</asp:ListItem>
                                                        <asp:ListItem Value="+02:15">+02:15</asp:ListItem>
                                                        <asp:ListItem Value="+02:30">+02:30</asp:ListItem>
                                                        <asp:ListItem Value="+02:45">+02:45</asp:ListItem>
                                                        <asp:ListItem Value="+03:00">+03:00</asp:ListItem>
                                                        <asp:ListItem Value="+03:15">+03:15</asp:ListItem>
                                                        <asp:ListItem Value="+03:30">+03:30</asp:ListItem>
                                                        <asp:ListItem Value="+03:45">+03:45</asp:ListItem>
                                                        <asp:ListItem Value="+04:00">+04:00</asp:ListItem>
                                                        <asp:ListItem Value="+04:15">+04:15</asp:ListItem>
                                                        <asp:ListItem Value="+04:30">+04:30</asp:ListItem>
                                                        <asp:ListItem Value="+04:45">+04:45</asp:ListItem>
                                                        <asp:ListItem Value="+05:00">+05:00</asp:ListItem>
                                                        <asp:ListItem Value="+05:15">+05:15</asp:ListItem>
                                                        <asp:ListItem Value="+05:30">+05:30</asp:ListItem>
                                                        <asp:ListItem Value="+05:45">+05:45</asp:ListItem>
                                                        <asp:ListItem Value="+06:00">+06:00</asp:ListItem>
                                                        <asp:ListItem Value="+06:15">+06:15</asp:ListItem>
                                                        <asp:ListItem Value="+06:30">+06:30</asp:ListItem>
                                                        <asp:ListItem Value="+06:45">+06:45</asp:ListItem>
                                                        <asp:ListItem Value="+07:00">+07:00</asp:ListItem>
                                                        <asp:ListItem Value="+07:15">+07:15</asp:ListItem>
                                                        <asp:ListItem Value="+07:30">+07:30</asp:ListItem>
                                                        <asp:ListItem Value="+07:45">+07:45</asp:ListItem>
                                                        <asp:ListItem Value="+08:00">+08:00</asp:ListItem>
                                                        <asp:ListItem Value="+08:15">+08:15</asp:ListItem>
                                                        <asp:ListItem Value="+08:30">+08:30</asp:ListItem>
                                                        <asp:ListItem Value="+08:45">+08:45</asp:ListItem>
                                                        <asp:ListItem Value="+09:00">+09:00</asp:ListItem>
                                                        <asp:ListItem Value="+09:15">+09:15</asp:ListItem>
                                                        <asp:ListItem Value="+09:30">+09:30</asp:ListItem>
                                                        <asp:ListItem Value="+09:45">+09:45</asp:ListItem>
                                                        <asp:ListItem Value="+10:00">+10:00</asp:ListItem>
                                                        <asp:ListItem Value="+10:15">+10:15</asp:ListItem>
                                                        <asp:ListItem Value="+10:30">+10:30</asp:ListItem>
                                                        <asp:ListItem Value="+10:45">+10:45</asp:ListItem>
                                                        <asp:ListItem Value="+11:00">+11:00</asp:ListItem>
                                                        <asp:ListItem Value="+11:15">+11:15</asp:ListItem>
                                                        <asp:ListItem Value="+11:30">+11:30</asp:ListItem>
                                                        <asp:ListItem Value="+11:45">+11:45</asp:ListItem>
                                                        <asp:ListItem Value="+12:00">+12:00</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTakeOffLocalDateTime" runat="server" CssClass="clsLabelAuto">Take 
                                                                Off Date/Time</asp:Label>
                                                    <asp:Label ID="lblUTCTakeOffDateTime" runat="server" CssClass="clsLabelAuto">UTC 
                                                                Take Off Date/Time</asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:CheckBox ID="chkTakeOff" runat="server" AutoPostBack="True" ToolTip="Check to enable Take Off Date" />
                                                    <asp:TextBox runat="server" ID="calTakeOffLocalDateTime" CssClass="clsTextBox_Ajax"
                                                        BackColor="#E0E0E0" ReadOnly="true" Width="100px" AutoPostBack="True" CausesValidation="True"
                                                        onchange="ValidateDateText(this,'calTakeOffLocalDateTime_watermarkextender');"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="calTakeOffLocalDateTime_CalendarExtender" runat="server"
                                                        CssClass="cal_Theme1" Enabled="false" Format="<%$AppSettings:DateFormat%>" TargetControlID="calTakeOffLocalDateTime">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="calTakeOffLocalDateTime" ID="calTakeOffLocalDateTime_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox">
                                                    </cc2:TextBoxWatermarkExtender>
                                                    <asp:TextBox runat="server" ID="calUTCTakeOffDateTime" CssClass="clsTextBox_Ajax"
                                                        BackColor="#E0E0E0" ReadOnly="true" Width="100px" AutoPostBack="True" CausesValidation="True"
                                                        onchange="ValidateDateText(this,'calUTCTakeOffDateTime_watermarkextender');"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="calUTCTakeOffDateTime_CalendarExtender" runat="server"
                                                        CssClass="cal_Theme1" Enabled="false" Format="<%$AppSettings:DateFormat%>" TargetControlID="calUTCTakeOffDateTime">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="calUTCTakeOffDateTime" ID="calUTCTakeOffDateTime_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox">
                                                    </cc2:TextBoxWatermarkExtender>
                                                    <asp:TextBox ID="txtTakeOffLocalTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxMedium_Ajax"
                                                        MaxLength="10" ToolTip="Enter Take Off Time."></asp:TextBox>
                                                    <asp:TextBox ID="txtUTCTakeOffTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxMedium_Ajax"
                                                        MaxLength="10" ToolTip="Enter UTC Take Off Time."></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top">
                                        <table width="100%">
                                            <tr>
                                                <td colspan="5">
                                                    <asp:Label ID="lblArival" runat="server" CssClass="clstitle2_ajax">Arrival</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblArrPlace" runat="server" CssClass="clsLabelAuto">Place</asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <table>
                                                        <tr>
                                                            <td style="width: 13px;">
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="Place2" runat="server" CssClass="clsTextBoxDate_Ajax" Text="<%# mLog.DestinationName %>"
                                                                    Width="200px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="btnAddPlace" runat="server" CausesValidation="False" Height="20px" Visible="false" 
                                                                    ImageUrl="~/images/plus1.png" ToolTip="Click to Add new Place" Width="24px" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtArrPlace" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxDate_Ajax"
                                                                    ReadOnly="True" Text="<%# mLog.DestinationName %>" ToolTip="Place" Visible="False"
                                                                    Width="168px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnAddArrPlace" runat="server" CssClass="clsButtonGrid_Ajax" Text="..."
                                                                    ToolTip="Click to add New Place" Visible="False" />
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="imgbtnArrPlace" runat="server" CausesValidation="False" CssClass="clsButtonImg_Ajax"
                                                                    Enabled="<%# mLog.IsNew %>" ImageUrl="ICONS/ADD.ICO" ToolTip="Select Place" Visible="False" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblArrDate" runat="server" CssClass="clsLabelAuto">Date/Time</asp:Label>
                                                    <asp:Label ID="lblUTCArrivalDateTime" runat="server" CssClass="clsLabelAuto">UTC 
                                                                DateTime</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkArrival" runat="server" AutoPostBack="True" ToolTip="Check to enable Arrival Date" />
                                                    <asp:TextBox runat="server" ID="calArrival" CssClass="clsTextBox_Ajax" Width="100px"
                                                        BackColor="#E0E0E0" ReadOnly="true" AutoPostBack="True" onchange="ValidateDateText(this,'calArrival_watermarkextender');"
                                                        CausesValidation="True"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="calArrival_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="false" Format="<%$AppSettings:DateFormat%>" TargetControlID="calArrival">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="calArrival" ID="calArrival_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox">
                                                    </cc2:TextBoxWatermarkExtender>
                                                    <asp:TextBox runat="server" ID="CalUTCArrival" CssClass="clsTextBox_Ajax" Width="100px"
                                                        BackColor="#E0E0E0" ReadOnly="true" AutoPostBack="false" CausesValidation="True"
                                                        onchange="ValidateDateText(this,'CalUTCArrival_watermarkextender');"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="CalUTCArrival_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="false" Format="<%$AppSettings:DateFormat%>" TargetControlID="CalUTCArrival">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="CalUTCArrival" ID="CalUTCArrival_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox">
                                                    </cc2:TextBoxWatermarkExtender>
                                                    <asp:TextBox ID="txtArrivalTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxMedium_Ajax"
                                                        MaxLength="10" ToolTip="Enter Arrival Time."></asp:TextBox>
                                                    <asp:TextBox ID="txtUTCArrivalTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxMedium_Ajax"
                                                        MaxLength="10" ToolTip="Enter UTC Arrival Time."></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblArrDayLightTime" runat="server" CssClass="clsLabelAuto" Visible="False">D/L Time</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbArrivalDayLightTime" runat="server" CssClass="clsComboBoxsmall_Ajax"
                                                        DataTextField="Name" DataValueField="ID" SelectedValue="<%# mLog.DesDayLightTime %>"
                                                        Visible="False">
                                                        <asp:ListItem Selected="True" Value="-12:00">-12:00</asp:ListItem>
                                                        <asp:ListItem Value="-11:45">-11:45</asp:ListItem>
                                                        <asp:ListItem Value="-11:30">-11:30</asp:ListItem>
                                                        <asp:ListItem Value="-11:15">-11:15</asp:ListItem>
                                                        <asp:ListItem Value="-11:00">-11:00</asp:ListItem>
                                                        <asp:ListItem Value="-10:45">-10:45</asp:ListItem>
                                                        <asp:ListItem Value="-10:30">-10:30</asp:ListItem>
                                                        <asp:ListItem Value="-10:15">-10:15</asp:ListItem>
                                                        <asp:ListItem Value="-10:00">-10:00</asp:ListItem>
                                                        <asp:ListItem Value="-09:45">-09:45</asp:ListItem>
                                                        <asp:ListItem Value="-09:30">-09:30</asp:ListItem>
                                                        <asp:ListItem Value="-09:15">-09:15</asp:ListItem>
                                                        <asp:ListItem Value="-09:00">-09:00</asp:ListItem>
                                                        <asp:ListItem Value="-08:45">-08:45</asp:ListItem>
                                                        <asp:ListItem Value="-08:30">-08:30</asp:ListItem>
                                                        <asp:ListItem Value="-08:15">-08:15</asp:ListItem>
                                                        <asp:ListItem Value="-08:00">-08:00</asp:ListItem>
                                                        <asp:ListItem Value="-07:45">-07:45</asp:ListItem>
                                                        <asp:ListItem Value="-07:30">-07:30</asp:ListItem>
                                                        <asp:ListItem Value="-07:15">-07:15</asp:ListItem>
                                                        <asp:ListItem Value="-07:00">-07:00</asp:ListItem>
                                                        <asp:ListItem Value="-06:45">-06:45</asp:ListItem>
                                                        <asp:ListItem Value="-06:30">-06:30</asp:ListItem>
                                                        <asp:ListItem Value="-06:15">-06:15</asp:ListItem>
                                                        <asp:ListItem Value="-06:00">-06:00</asp:ListItem>
                                                        <asp:ListItem Value="-05:45">-05:45</asp:ListItem>
                                                        <asp:ListItem Value="-05:30">-05:30</asp:ListItem>
                                                        <asp:ListItem Value="-05:15">-05:15</asp:ListItem>
                                                        <asp:ListItem Value="-05:00">-05:00</asp:ListItem>
                                                        <asp:ListItem Value="-04:45">-04:45</asp:ListItem>
                                                        <asp:ListItem Value="-04:30">-04:30</asp:ListItem>
                                                        <asp:ListItem Value="-04:15">-04:15</asp:ListItem>
                                                        <asp:ListItem Value="-04:00">-04:00</asp:ListItem>
                                                        <asp:ListItem Value="-03:45">-03:45</asp:ListItem>
                                                        <asp:ListItem Value="-03:30">-03:30</asp:ListItem>
                                                        <asp:ListItem Value="-03:15">-03:15</asp:ListItem>
                                                        <asp:ListItem Value="-03:00">-03:00</asp:ListItem>
                                                        <asp:ListItem Value="-02:45">-02:45</asp:ListItem>
                                                        <asp:ListItem Value="-02:30">-02:30</asp:ListItem>
                                                        <asp:ListItem Value="-02:15">-02:15</asp:ListItem>
                                                        <asp:ListItem Value="-02:00">-02:00</asp:ListItem>
                                                        <asp:ListItem Value="-01:45">-01:45</asp:ListItem>
                                                        <asp:ListItem Value="-01:30">-01:30</asp:ListItem>
                                                        <asp:ListItem Value="-01:15">-01:15</asp:ListItem>
                                                        <asp:ListItem Value="-01:00">-01:00</asp:ListItem>
                                                        <asp:ListItem Value="-00:45">-00:45</asp:ListItem>
                                                        <asp:ListItem Value="-00:30">-00:30</asp:ListItem>
                                                        <asp:ListItem Value="-00:15">-00:15</asp:ListItem>
                                                        <asp:ListItem Value="+00:00">+00:00</asp:ListItem>
                                                        <asp:ListItem Value="+00:15">+00:15</asp:ListItem>
                                                        <asp:ListItem Value="+00:30">+00:30</asp:ListItem>
                                                        <asp:ListItem Value="+00:45">+00:45</asp:ListItem>
                                                        <asp:ListItem Value="+01:00">+01:00</asp:ListItem>
                                                        <asp:ListItem Value="+01:15">+01:15</asp:ListItem>
                                                        <asp:ListItem Value="+01:30">+01:30</asp:ListItem>
                                                        <asp:ListItem Value="+01:45">+01:45</asp:ListItem>
                                                        <asp:ListItem Value="+02:00">+02:00</asp:ListItem>
                                                        <asp:ListItem Value="+02:15">+02:15</asp:ListItem>
                                                        <asp:ListItem Value="+02:30">+02:30</asp:ListItem>
                                                        <asp:ListItem Value="+02:45">+02:45</asp:ListItem>
                                                        <asp:ListItem Value="+03:00">+03:00</asp:ListItem>
                                                        <asp:ListItem Value="+03:15">+03:15</asp:ListItem>
                                                        <asp:ListItem Value="+03:30">+03:30</asp:ListItem>
                                                        <asp:ListItem Value="+03:45">+03:45</asp:ListItem>
                                                        <asp:ListItem Value="+04:00">+04:00</asp:ListItem>
                                                        <asp:ListItem Value="+04:15">+04:15</asp:ListItem>
                                                        <asp:ListItem Value="+04:30">+04:30</asp:ListItem>
                                                        <asp:ListItem Value="+04:45">+04:45</asp:ListItem>
                                                        <asp:ListItem Value="+05:00">+05:00</asp:ListItem>
                                                        <asp:ListItem Value="+05:15">+05:15</asp:ListItem>
                                                        <asp:ListItem Value="+05:30">+05:30</asp:ListItem>
                                                        <asp:ListItem Value="+05:45">+05:45</asp:ListItem>
                                                        <asp:ListItem Value="+06:00">+06:00</asp:ListItem>
                                                        <asp:ListItem Value="+06:15">+06:15</asp:ListItem>
                                                        <asp:ListItem Value="+06:30">+06:30</asp:ListItem>
                                                        <asp:ListItem Value="+06:45">+06:45</asp:ListItem>
                                                        <asp:ListItem Value="+07:00">+07:00</asp:ListItem>
                                                        <asp:ListItem Value="+07:15">+07:15</asp:ListItem>
                                                        <asp:ListItem Value="+07:30">+07:30</asp:ListItem>
                                                        <asp:ListItem Value="+07:45">+07:45</asp:ListItem>
                                                        <asp:ListItem Value="+08:00">+08:00</asp:ListItem>
                                                        <asp:ListItem Value="+08:15">+08:15</asp:ListItem>
                                                        <asp:ListItem Value="+08:30">+08:30</asp:ListItem>
                                                        <asp:ListItem Value="+08:45">+08:45</asp:ListItem>
                                                        <asp:ListItem Value="+09:00">+09:00</asp:ListItem>
                                                        <asp:ListItem Value="+09:15">+09:15</asp:ListItem>
                                                        <asp:ListItem Value="+09:30">+09:30</asp:ListItem>
                                                        <asp:ListItem Value="+09:45">+09:45</asp:ListItem>
                                                        <asp:ListItem Value="+10:00">+10:00</asp:ListItem>
                                                        <asp:ListItem Value="+10:15">+10:15</asp:ListItem>
                                                        <asp:ListItem Value="+10:30">+10:30</asp:ListItem>
                                                        <asp:ListItem Value="+10:45">+10:45</asp:ListItem>
                                                        <asp:ListItem Value="+11:00">+11:00</asp:ListItem>
                                                        <asp:ListItem Value="+11:15">+11:15</asp:ListItem>
                                                        <asp:ListItem Value="+11:30">+11:30</asp:ListItem>
                                                        <asp:ListItem Value="+11:45">+11:45</asp:ListItem>
                                                        <asp:ListItem Value="+12:00">+12:00</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTouchDownLocalDateTime" runat="server" CssClass="clsLabelAuto">Touch Down Date/Time</asp:Label>
                                                    <asp:Label ID="lblUTCTouchDownDateTime" runat="server" CssClass="clsLabelAuto">UTC 
                                                                Touch Down Date/Time</asp:Label>
                                                </td>
                                                <td colspan="3">
                                                    <asp:CheckBox ID="chkTouchDown" runat="server" AutoPostBack="True" ToolTip="Check to enable Touch Down Date." />
                                                    <asp:TextBox runat="server" ID="calTouchDownLocalDateTime" CssClass="clsTextBox_Ajax"
                                                        BackColor="#E0E0E0" ReadOnly="true" Width="100px" AutoPostBack="True" CausesValidation="True"
                                                        onchange="ValidateDateText(this,'calTouchDownLocalDateTime_watermarkextender');"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="calTouchDownLocalDateTime_CalendarExtender" runat="server"
                                                        CssClass="cal_Theme1" Enabled="false" Format="<%$AppSettings:DateFormat%>" TargetControlID="calTouchDownLocalDateTime">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="calTouchDownLocalDateTime" ID="calTouchDownLocalDateTime_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox">
                                                    </cc2:TextBoxWatermarkExtender>
                                                    <asp:TextBox runat="server" ID="calUTCTouchDownDateTime" CssClass="clsTextBox_Ajax"
                                                        BackColor="#E0E0E0" ReadOnly="true" Width="100px" AutoPostBack="True" CausesValidation="True"
                                                        onchange="ValidateDateText(this,'calUTCTouchDownDateTime_watermarkextender');"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="calUTCTouchDownDateTime_CalendarExtender" runat="server"
                                                        CssClass="cal_Theme1" Enabled="false" Format="<%$AppSettings:DateFormat%>" TargetControlID="calUTCTouchDownDateTime">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender TargetControlID="calUTCTouchDownDateTime" ID="calUTCTouchDownDateTime_watermarkextender"
                                                        ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                        WatermarkCssClass="clsDateTextBox">
                                                    </cc2:TextBoxWatermarkExtender>
                                                    <asp:TextBox ID="txtTouchDownLocalTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxMedium_Ajax"
                                                        MaxLength="10" ToolTip="Enter Touch Down Time."></asp:TextBox>
                                                    <asp:TextBox ID="txtUTCTouchDownTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxMedium_Ajax"
                                                        MaxLength="10" ToolTip="Enter UTC Touch Down Time."></asp:TextBox>
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
                    <asp:Label ID="lblAir" runat="server" CssClass="clstitle2_ajax">Aircraft Flying Hours as per Flight Log book or HOBBS</asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <%--AJAX- Add UpdatePanel for Flight Summary --%>
                    <asp:UpdatePanel ID="upnlFlightSummary" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="pnlHours" runat="server" CssClass="clsPanel1" Visible="False">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblairfly" runat="server" CssClass="clsLabelAuto">Block Time</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtBlockTime" runat="server" BackColor="Gainsboro" CssClass="clsTextBoxMedium_Ajax"
                                                                        Enabled="false" Text="<%# mLog.DiffTime %>" Visible="False"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblAirBorneTime" runat="server" CssClass="clsLabelAuto">Airborne 
                                                                                Time </asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtAirBorneTime" runat="server" CssClass="clsTextBoxMedium_Ajax"
                                                                        AutoPostBack="true" ReadOnly="<%# mLog.ShowTimeTextBoxes or Not mLog.IsNew %>"
                                                                        Text="<%# mLog.TimeInAir %>" Visible="False" onfocus="onTextFocus();"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblGroundRunTime" runat="server" CssClass="clsLabelAuto">Ground 
                                                                                Run Time </asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtGroundRunTime" runat="server" CssClass="clsTextBoxMedium_Ajax"
                                                                        ReadOnly="<%# mlog.ShowTimeOnGround or Not mLog.IsNew %>" Text="<%# mLog.TimeOnGround %>"
                                                                        Visible="False" onfocus="onTextFocus();" AutoPostBack="True"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblPercentTimeOnGround" runat="server" CssClass="clsLabelAuto">%Ground 
                                                                                Run Time </asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtPercentTimeOnGround" runat="server" CssClass="clsTextBoxMedium_Ajax"
                                                                        ReadOnly="<%# Not mLog.IsNew %>" Text="<%# mLog.PercentTimeOnGround %>" Visible="False"
                                                                        AutoPostBack="True" onfocus="onTextFocus();"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="pnlDecimal" runat="server" CssClass="clsPanel1" Visible="False">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblHobbsread" runat="server" CssClass="clsLabelAuto">HOBBS READING :  </asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto">Previous Value :</asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblHobbsPrevVal" runat="server" CssClass="clsLabelauto">Offset
                                                                                </asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtPrevHobbsOffset" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxMedium_Ajax"
                                                                                    ReadOnly="True" Text="<%# mLog.PrevHobbsOffsetValue %>" Visible="False"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblHobbsCurrentReading" runat="server" CssClass="clsLabelauto">Reading
                                                                                </asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtPrevHobbsValue" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxMedium_Ajax"
                                                                                    ReadOnly="True" Text="<%# mLog.PrevHobbsValue %>" Visible="False"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="Label2" runat="server" CssClass="clsLabelAuto">Current Value :</asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblOffsetPreVal" runat="server" CssClass="clsLabelauto">Offset
                                                                                </asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtCurrentHobbsOffset" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxMedium_Ajax"
                                                                                    ReadOnly="True" Text="<%# mLog.CurrentHobbsOffsetValue %>" Visible="False"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblOffsetCurrentVal" runat="server" CssClass="clsLabelauto">Reading
                                                                                </asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtCurrentHobbsValue" runat="server" CssClass="clsTextBoxMedium_Ajax"
                                                                                    AutoPostBack="true" Text="<%# mLog.CurrentHobbsValue %>" Visible="False"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td rowspan="2" align="right">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblTotalTime" runat="server" CssClass="clsLabelAuto">Total Time</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTotalTime" runat="server" CssClass="clsTextBoxMedium_Ajax" Text="<%# mLog.TotalTime %>"
                                                        ReadOnly="True" BackColor="#E0E0E0" Enabled="False"></asp:TextBox>
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
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblAirframePeriod" runat="server" CssClass="clsLabelHeader">Airframe Period</asp:Label>
                            </td>
                            <%--<td align="right">
                                <asp:LinkButton ID="lnkAllAssembly" runat="server" CssClass="clsLinkButton" Font-Italic="true"
                                    Font-Size="9pt" ToolTip="Click to go on All Assembly screen" ClientIDMode="Static"
                                   >Show All Assembly</asp:LinkButton>
                            </td>--%>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <%--AJAX- Add UpdatePanel for Airframe Grid--%>
                    <asp:UpdatePanel ID="upnlAirframeDetail" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="dgAFPeriods" runat="server" AutoGenerateColumns="False" Width="100%"
                                BorderStyle="Solid" CellPadding="0" ForeColor="#333333" CssClass="clsGridLog"
                                AlternatingRowStyle-CssClass="alt" RowStyle-Wrap="false" HeaderStyle-Wrap="false"
                                SelectedRowStyle-BackColor="ButtonShadow" ShowHeaderWhenEmpty="True" PageSize="3"
                                PagerSettings-Mode="NextPreviousFirstLast">
                                <RowStyle CssClass="clsdgItem" />
                                <HeaderStyle CssClass="clsdgHeader" />
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
                                    <asp:BoundField DataField="ModelName" HeaderText="Model">
                                        <HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="150px" />
                                        <ItemStyle HorizontalAlign="Left" Wrap="false" Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
                                        <HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="100px" />
                                        <ItemStyle HorizontalAlign="Left" Wrap="false" Width="100px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Hours">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAirFrameHours" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                Width="93%" ReadOnly="<%# Not mLog.IsNew %>" Text='<%# DataBinder.Eval(Container.DataItem,"Hours") %>'
                                                ToolTip="Enter the Hours." AutoPostBack="true" OnTextChanged="txtAirFrameHours_TextChanged"
                                                onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
                                            </asp:TextBox>
                                            <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                        <ItemStyle HorizontalAlign="Right" Width="75px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="FinalHours" HeaderText="Final Hours">
                                        <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Landings">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAirFrameLandings" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                Width="93%" Text='<%# DataBinder.Eval(Container.DataItem,"Landings") %>' ToolTip="Enter the Landing."
                                                AutoPostBack="true" OnTextChanged="txtAirFrameLandings_TextChanged" onkeypress="onkeyPressed(window.event.keyCode,this);"
                                                onfocus="onTextFocus();">  <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                            </asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                        <ItemStyle HorizontalAlign="Right" Width="75px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="FinalLandings" HeaderText="Final Landings">
                                        <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Cycles">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAirFrameCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                Width="93%" Text='<%# DataBinder.Eval(Container.DataItem,"Cycles") %>' ToolTip="Enter Cycles."
                                                AutoPostBack="true" OnTextChanged="txtAirFrameCycles_TextChanged" onkeypress="onkeyPressed(window.event.keyCode,this);"
                                                onfocus="onTextFocus();">  <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                            </asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                        <ItemStyle HorizontalAlign="Right" Width="75px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="FinalCycles" HeaderText="Final Cycles">
                                        <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Starts">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAirFrameStarts" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                Width="93%" Text='<%# DataBinder.Eval(Container.DataItem,"Starts") %>' ToolTip="Enter Start Time."
                                                AutoPostBack="true" OnTextChanged="txtAirFrameStarts_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                onfocus="onTextFocus();">  <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                            </asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                        <ItemStyle HorizontalAlign="Right" Width="75px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="FinalStarts" HeaderText="Final Starts">
                                        <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="NG Cycles">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAirFrameNGCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                Width="93%" Text='<%# DataBinder.Eval(Container.DataItem,"NGCycles") %>' ToolTip="Enter NG Cycles"
                                                AutoPostBack="true" OnTextChanged="txtAirFrameNGCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                onfocus="onTextFocus();">    <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                            </asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                        <ItemStyle HorizontalAlign="Right" Width="75px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="FinalNGCycles" HeaderText="Final NG Cycles">
                                        <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="NF Cycles">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAirFrameNFCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                Width="93%" Text='<%# DataBinder.Eval(Container.DataItem,"NFCycles") %>' ToolTip="Enter NF Cycles"
                                                AutoPostBack="true" OnTextChanged="txtAirFrameNFCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                onfocus="onTextFocus();">    <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                            </asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                        <ItemStyle HorizontalAlign="Right" Width="75px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="FinalNFCycles" HeaderText="Final NF Cycles">
                                        <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="RINS">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAirFrameRins" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                Width="93%" Text='<%# DataBinder.Eval(Container.DataItem,"RINS") %>' ToolTip="Enter RINS"
                                                AutoPostBack="true" OnTextChanged="txtAirFrameRins_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                onfocus="onTextFocus();">    <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                            </asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                        <ItemStyle HorizontalAlign="Right" Width="75px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="FinalRINS" HeaderText="Final RINS">
                                        <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Bleeds">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAirFrameBleeds" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                Width="93%" Text='<%# DataBinder.Eval(Container.DataItem,"Bleeds") %>' ToolTip="Enter Bleeds"
                                                AutoPostBack="true" OnTextChanged="txtAirFrameBleeds_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                onfocus="onTextFocus();">    <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                            </asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                        <ItemStyle HorizontalAlign="Right" Width="75px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="FinalBleeds" HeaderText="Final Bleeds">
                                        <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Impeller Cycles">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAirFrameImpellerCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                Width="93%" Text='<%# DataBinder.Eval(Container.DataItem,"ImpellerCycles") %>'
                                                ToolTip="Enter Impeller Cycles" AutoPostBack="true" OnTextChanged="txtAirFrameImpellerCycles_TextChanged"
                                                onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">   <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                            </asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                        <ItemStyle HorizontalAlign="Right" Width="75px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="FinalImpellerCycles" HeaderText="Final Impeller Cycles">
                                        <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="CT Cycles">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAirFrameCTCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                Width="93%" Text='<%# DataBinder.Eval(Container.DataItem,"CTCycles") %>' ToolTip="Enter CT Cycles"
                                                AutoPostBack="true" OnTextChanged="txtAirFrameCTCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                onfocus="onTextFocus();">    <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                            </asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                        <ItemStyle HorizontalAlign="Right" Width="75px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="FinalCTCycles" HeaderText="Final CT Cycles">
                                        <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="PT Cycles">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAirFramePTCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                Width="93%" Text='<%# DataBinder.Eval(Container.DataItem,"PTCycles") %>' ToolTip="Enter PT Cycles"
                                                AutoPostBack="true" OnTextChanged="txtAirFramePTCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                onfocus="onTextFocus();">    <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                            </asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                        <ItemStyle HorizontalAlign="Right" Width="75px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="FinalPTCycles" HeaderText="Final PT Cycles">
                                        <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Generator Mods">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAirframeGeneratorMods" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                Width="93%" Text='<%# DataBinder.Eval(Container.DataItem,"GeneratorMods") %>'
                                                ToolTip="Enter the Generator Mods." AutoPostBack="true" OnTextChanged="txtAirframeGeneratorMods_TextChanged"
                                                onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">   <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                            </asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                        <ItemStyle HorizontalAlign="Right" Width="75px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="FinalGeneratorMods" HeaderText="Final Generator Mods">
                                        <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText=""></asp:BoundField>
                                </Columns>
                                <SelectedRowStyle BackColor="ControlDark" />
                                <AlternatingRowStyle CssClass="clsdgAltItem" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlEngineDetail" runat="server" UpdateMode="Conditional">
                        <%--Add UpdatePanel for Engine Grid--%>
                        <ContentTemplate>
                            <div style="width: 100%">
                                <asp:Label ID="lblEnginePeriod" runat="server" CssClass="clsLabelHeader">Engine Period</asp:Label>
                            </div>
                            <div style="width: 100%">
                                <asp:GridView ID="dgEnginePeriods" runat="server" AutoGenerateColumns="False" Width="100%"
                                    BorderStyle="Solid" CellPadding="0" ForeColor="#333333" CssClass="clsGridLog"
                                    AlternatingRowStyle-CssClass="alt" RowStyle-Wrap="false" HeaderStyle-Wrap="false"
                                    SelectedRowStyle-BackColor="ButtonShadow" ShowHeaderWhenEmpty="True" PageSize="3"
                                    PagerSettings-Mode="NextPreviousFirstLast">
                                    <RowStyle CssClass="clsdgItem" />
                                    <HeaderStyle CssClass="clsdgHeader" />
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
                                        <asp:BoundField DataField="ModelName" HeaderText="Model">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="150px" />
                                            <ItemStyle HorizontalAlign="Left" Wrap="false" Width="150px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="100px" />
                                            <ItemStyle HorizontalAlign="Left" Wrap="false" Width="100px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Hours">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtEngineHours" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    ReadOnly="<%# Not mLog.IsNew %>" Text='<%# DataBinder.Eval(Container.DataItem,"Hours") %>'
                                                    ToolTip="Enter the Hours." Width="93%" AutoPostBack="true" OnTextChanged="txtEngineHours_TextChanged"
                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">    <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalHours" HeaderText="Final Hours">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Landings">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtEngineLandings" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Width="93%" Text='<%# DataBinder.Eval(Container.DataItem,"Landings") %>' ToolTip="Enter the Landing."
                                                    AutoPostBack="true" OnTextChanged="txtEngineLandings_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                    onfocus="onTextFocus();">    <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalLandings" HeaderText="Final Landings">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Cycles">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtEngineCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Width="93%" Text='<%# DataBinder.Eval(Container.DataItem,"Cycles") %>' ToolTip="Enter Cycles."
                                                    AutoPostBack="true" OnTextChanged="txtEngineCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                    onfocus="onTextFocus();">    <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalCycles" HeaderText="Final Cycles">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Starts">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtEngineStarts" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"Starts") %>' ToolTip="Enter Start Time."
                                                    AutoPostBack="true" OnTextChanged="txtEngineStarts_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                    onfocus="onTextFocus();" Width="93%"> 
                                                </asp:TextBox>
                                                <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalStarts" HeaderText="Final Starts">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="NG Cycles">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtEngineNGCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Width="93%" Text='<%# DataBinder.Eval(Container.DataItem,"NGCycles") %>' ToolTip="Enter NG Cycles"
                                                    AutoPostBack="true" OnTextChanged="btnEngineNGCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                    onfocus="onTextFocus();"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalNGCycles" HeaderText="Final NG Cycles">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="NF Cycles">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtEngineNFCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Width="93%" Text='<%# DataBinder.Eval(Container.DataItem,"NFCycles") %>' ToolTip="Enter NF Cycles"
                                                    AutoPostBack="true" OnTextChanged="txtEngineNFCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                    onfocus="onTextFocus();"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalNFCycles" HeaderText="Final NFCycles">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="RINS">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtEngineRins" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"RINS") %>' ToolTip="Enter RINS"
                                                    AutoPostBack="true" OnTextChanged="txtEngineRins_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                    onfocus="onTextFocus();" Width="93%"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalRINS" HeaderText="Final RINS">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Contingency Factor">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtEngineCFactors" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Width="97%" Text='<%# DataBinder.Eval(Container.DataItem,"CFactor") %>' ToolTip="Enter Contingency Factor."
                                                    AutoPostBack="true" OnTextChanged="txtEngineCFactors_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                    onfocus="onTextFocus();"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalCFactor" HeaderText="Final Contingency Factor">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Bleeds">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtEngineBleeds" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"Bleeds") %>' ToolTip="Enter Bleeds"
                                                    AutoPostBack="true" OnTextChanged="txtEngineBleeds_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                    onfocus="onTextFocus();" Width="93%"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalBleeds" HeaderText="Final Bleeds">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Impeller Cycles">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtEngineImpellerCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Width="93%" Text='<%# DataBinder.Eval(Container.DataItem,"ImpellerCycles") %>'
                                                    ToolTip="Enter Impeller Cycles" AutoPostBack="true" OnTextChanged="txtEngineImpellerCycles_TextChanged"
                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
                                                </asp:TextBox>
                                                <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalImpellerCycles" HeaderText="Final Impeller Cycles">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="CT Cycles">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtEngineCTCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Width="93%" Text='<%# DataBinder.Eval(Container.DataItem,"CTCycles") %>' ToolTip="Enter CT Cycles"
                                                    AutoPostBack="true" OnTextChanged="txtEngineCTCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                    onfocus="onTextFocus();"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalCTCycles" HeaderText="Final CT Cycles">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="PT Cycles">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtEnginePTCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Width="93%" Text='<%# DataBinder.Eval(Container.DataItem,"PTCycles") %>' ToolTip="Enter PT Cycles"
                                                    AutoPostBack="true" OnTextChanged="txtEnginePTCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                    onfocus="onTextFocus();"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalPTCycles" HeaderText="Final PT Cycles">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Generator Mods">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtEngineGeneratorMods" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Width="93%" Text='<%# DataBinder.Eval(Container.DataItem,"GeneratorMods") %>'
                                                    ToolTip="Enter the Generator Mods." AutoPostBack="true" OnTextChanged="txtEngineGeneratorMods_TextChanged"
                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
                                                </asp:TextBox>
                                                <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalGeneratorMods" HeaderText="Final Generator Mods">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Rapid Take Off">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtEngineRapidTakeOffFactor" runat="server" ToolTip="Enter Rapid Take Off."
                                                                    CssClass="clsTextBoxMegaSmall_Ajax" Text='<%# DataBinder.Eval(Container.DataItem,"RapidTakeOffFactor") %>'
                                                                    AutoPostBack="true" OnTextChanged="txtEngineRapidTakeOffFactor_TextChanged" Width="97%"
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalRapidTakeOffFactor" HeaderText="Final Rapid Take Off">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                    </Columns>
                                    <SelectedRowStyle BackColor="ControlDark" />
                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlAPUDetail" runat="server" UpdateMode="Conditional">
                        <%--Add UpdatePanel for APU Grid--%>
                        <ContentTemplate>
                            <div style="width: 100%">
                                <asp:Label ID="lblAPUPeriod" runat="server" CssClass="clsLabelHeader">APU Period</asp:Label>
                            </div>
                            <div style="width: 100%">
                                <asp:GridView ID="dgAPUPeriods" runat="server" AutoGenerateColumns="False" Width="100%"
                                    BorderStyle="Solid" CellPadding="0" ForeColor="#333333" CssClass="clsGridLog"
                                    AlternatingRowStyle-CssClass="alt" RowStyle-Wrap="false" HeaderStyle-Wrap="false"
                                    SelectedRowStyle-BackColor="ButtonShadow" ShowHeaderWhenEmpty="True" PageSize="3"
                                    PagerSettings-Mode="NextPreviousFirstLast">
                                    <RowStyle CssClass="clsdgItem" />
                                    <HeaderStyle CssClass="clsdgHeader" />
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
                                        <asp:BoundField DataField="ModelName" HeaderText="Model">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="150px" />
                                            <ItemStyle HorizontalAlign="Left" Wrap="false" Width="150px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="100px" />
                                            <ItemStyle HorizontalAlign="Left" Wrap="false" Width="100px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Hours">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAPUHours" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"Hours") %>' ToolTip="Enter the Hours."
                                                    Width="93%" AutoPostBack="true" OnTextChanged="txtAPUHours_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                    onfocus="onTextFocus();"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalHours" HeaderText="Final Hours">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Landings">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAPULandings" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"Landings") %>' ToolTip="Enter the Landing."
                                                    AutoPostBack="true" OnTextChanged="txtAPULandings_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                    onfocus="onTextFocus();" Width="93%"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalLandings" HeaderText="Final Landings">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Cycles">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAPUCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"Cycles") %>' ToolTip="Enter Cycles."
                                                    AutoPostBack="true" OnTextChanged="txtAPUCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                    onfocus="onTextFocus();" Width="93%"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalCycles" HeaderText="Final Cycles">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Starts">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAPUStarts" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"Starts") %>' ToolTip="Enter Start Time."
                                                    AutoPostBack="true" OnTextChanged="txtAPUStarts_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                    onfocus="onTextFocus();" Width="93%"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalStarts" HeaderText="Final Starts">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="NG Cycles">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAPUNGCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"NGCycles") %>' ToolTip="Enter NG Cycles"
                                                    AutoPostBack="true" OnTextChanged="txtAPUNGCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                    onfocus="onTextFocus();" Width="93%"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalNGCycles" HeaderText="Final NG Cycles">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="NF Cycles">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAPUNFCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"NFCycles") %>' ToolTip="Enter NF Cycles"
                                                    AutoPostBack="true" OnTextChanged="txtAPUNFCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                    onfocus="onTextFocus();" Width="93%"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalNFCycles" HeaderText="Final NF Cycles">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="RINS">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAPURins" runat="server" CssClass="clsTextBoxMegaSmall_Ajax" Text='<%# DataBinder.Eval(Container.DataItem,"RINS") %>'
                                                    ToolTip="Enter RINS." AutoPostBack="true" OnTextChanged="txtAPURins_TextChanged"
                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                    Width="93%"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalRINS" HeaderText="Final RINS">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Bleeds">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAPUBleeds" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"Bleeds") %>' ToolTip="Enter Bleeds"
                                                    AutoPostBack="true" OnTextChanged="txtAPUBleeds_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                    onfocus="onTextFocus();" Width="93%"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalBleeds" HeaderText="Final Bleeds">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Impeller Cycles">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAPUImpellerCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"ImpellerCycles") %>' ToolTip="Enter Impeller Cycles"
                                                    AutoPostBack="true" OnTextChanged="txtAPUImpellerCycles_TextChanged" Width="93%"
                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
                                                </asp:TextBox>
                                                <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalImpellerCycles" HeaderText="Final Impeller Cycles">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="CT Cycles">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAPUCTCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"CTCycles") %>' ToolTip="Enter CT Cycles"
                                                    AutoPostBack="true" OnTextChanged="txtAPUCTCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                    onfocus="onTextFocus();" Width="93%"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalCTCycles" HeaderText="Final CT Cycles">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="PT Cycles">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAPUPTCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"PTCycles") %>' ToolTip="Enter PT Cycles"
                                                    AutoPostBack="true" OnTextChanged="txtAPUPTCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                    onfocus="onTextFocus();" Width="93%"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalPTCycles" HeaderText="Final PT Cycles">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Generator Mods">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAPUGeneratorMods" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Width="93%" Text='<%# DataBinder.Eval(Container.DataItem,"GeneratorMods") %>'
                                                    ToolTip="Enter the Generator Mods." AutoPostBack="true" OnTextChanged="txtAPUGeneratorMods_TextChanged"
                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
                                                </asp:TextBox>
                                                <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalGeneratorMods" HeaderText="Final Generator Mods">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText=""></asp:BoundField>
                                    </Columns>
                                    <SelectedRowStyle BackColor="ControlDark" />
                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlCGBDetail" runat="server" UpdateMode="Conditional">
                        <%--Add UpdatePanel for CGB Grid--%>
                        <ContentTemplate>
                            <div style="width: 100%">
                                <asp:Label ID="lblCGBPeriod" runat="server" CssClass="clsLabelHeader">Air Condition Period</asp:Label>
                            </div>
                            <div style="width: 100%">
                                <asp:GridView ID="dgCGBPeriods" runat="server" AutoGenerateColumns="False" Width="100%"
                                    BorderStyle="Solid" CellPadding="0" ForeColor="#333333" CssClass="clsGridLog"
                                    AlternatingRowStyle-CssClass="alt" RowStyle-Wrap="false" HeaderStyle-Wrap="false"
                                    SelectedRowStyle-BackColor="ButtonShadow" ShowHeaderWhenEmpty="True" PageSize="3"
                                    PagerSettings-Mode="NextPreviousFirstLast">
                                    <RowStyle CssClass="clsdgItem" />
                                    <HeaderStyle CssClass="clsdgHeader" />
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
                                        <asp:BoundField DataField="ModelName" HeaderText="Model">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="150px" />
                                            <ItemStyle HorizontalAlign="Left" Wrap="false" Width="150px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="100px" />
                                            <ItemStyle HorizontalAlign="Left" Wrap="false" Width="100px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Hours">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCGBHours" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"Hours") %>' ToolTip="Enter the Hours."
                                                    Width="93%" AutoPostBack="true" OnTextChanged="txtCGBHours_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                    onfocus="onTextFocus();"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalHours" HeaderText="Final Hours">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Landings">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCGBLandings" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"Landings") %>' ToolTip="Enter the Landing."
                                                    AutoPostBack="true" OnTextChanged="txtCGBLandings_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                    onfocus="onTextFocus();" Width="93%"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalLandings" HeaderText="Final Landings">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Cycles">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCGBCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"Cycles") %>' ToolTip="Enter Cycles."
                                                    AutoPostBack="true" OnTextChanged="txtCGBCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                    onfocus="onTextFocus();" Width="93%"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalCycles" HeaderText="Final Cycles">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Starts">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCGBStarts" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"Starts") %>' ToolTip="Enter Start Time."
                                                    AutoPostBack="true" OnTextChanged="txtCGBStarts_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                    onfocus="onTextFocus();" Width="93%"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalStarts" HeaderText="Final Starts">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="NG Cycles">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCGBNGCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"NGCycles") %>' ToolTip="Enter NG Cycles"
                                                    AutoPostBack="true" OnTextChanged="txtCGBNGCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                    onfocus="onTextFocus();" Width="93%"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalNGCycles" HeaderText="Final NG Cycles">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="NF Cycles">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCGBNFCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"NFCycles") %>' ToolTip="Enter NF Cycles"
                                                    AutoPostBack="true" OnTextChanged="txtCGBNFCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                    onfocus="onTextFocus();" Width="93%"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalNFCycles" HeaderText="Final NF Cycles">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="RINS">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCGBRINS" runat="server" CssClass="clsTextBoxMegaSmall_Ajax" Text='<%# DataBinder.Eval(Container.DataItem,"RINS") %>'
                                                    ToolTip="Enter RINS" AutoPostBack="true" OnTextChanged="txtCGBRINS_TextChanged"
                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                    Width="93%"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalRINS" HeaderText="Final RINS">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Bleeds">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCGBBleeds" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"Bleeds") %>' ToolTip="Enter Bleeds"
                                                    AutoPostBack="true" OnTextChanged="txtCGBBleeds_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                    onfocus="onTextFocus();" Width="93%"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalBleeds" HeaderText="Final Bleeds">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Impeller Cycles">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCGBImpellerCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"ImpellerCycles") %>' ToolTip="Enter Impeller Cycles"
                                                    AutoPostBack="true" OnTextChanged="txtCGBImpellerCycles_TextChanged" Width="93%"
                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
                                                </asp:TextBox>
                                                <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalImpellerCycles" HeaderText="Final Impeller Cycles">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="CT Cycles">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCGBCTCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"CTCycles") %>' ToolTip="Enter CT Cycles"
                                                    AutoPostBack="true" OnTextChanged="txtCGBCTCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                    onfocus="onTextFocus();" Width="93%"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalCTCycles" HeaderText="Final CT Cycles">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="PT Cycles">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCGBPTCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"PTCycles") %>' ToolTip="Enter PT Cycles"
                                                    AutoPostBack="true" OnTextChanged="txtCGBPTCycles_TextChanged" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                    onfocus="onTextFocus();" Width="93%"> <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                                </asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalPTCycles" HeaderText="Final PT Cycles">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Font-Bold="true" HeaderText="Generator Mods">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCGBGeneratorMods" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                    Text='<%# DataBinder.Eval(Container.DataItem,"GeneratorMods") %>' ToolTip="Enter the Generator Mods."
                                                    AutoPostBack="true" OnTextChanged="txtCGBGeneratorMods_TextChanged" Width="93%"
                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
                                                </asp:TextBox>
                                                <%--"Refresh" buttons removed from Grid and AutoPostBack, OnTextChanged, onkeydown, onfocus added--%>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Width="75px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FinalGeneratorMods" HeaderText="Final Generator Mods">
                                            <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" Width="75px" />
                                            <ItemStyle HorizontalAlign="Right" Wrap="false" Width="75px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText=""></asp:BoundField>
                                    </Columns>
                                    <SelectedRowStyle BackColor="ControlDark" />
                                    <AlternatingRowStyle CssClass="clsdgAltItem" />
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="upnlRemark" runat="server" UpdateMode="Conditional">
                        <%--Add UpdatePanel for Remark--%>
                        <ContentTemplate>
                            <asp:Label ID="lblRemark" runat="server" CssClass="clsLabelAuto">Remark</asp:Label>
                            <br />
                            <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxMultiLineLong_Ajax"
                                MaxLength="500" Text="<%# mLog.Remark %>" TextMode="MultiLine" ToolTip="Enter Remark"
                                Width="700px" onfocus="onTextFocus();"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                        <%--AJAX- Add UpdatePanel for Save, SaveNew, Print and Back Grid--%>
                        <ContentTemplate>
                            <table id="Table7" border="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Button ID="btnAddNew" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Save the Log and add New Log" Visible="False"
                                            Text="Save &amp; New"></asp:Button>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Save the Record"
                                            Text="Save"></asp:Button>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnPrint" runat="server" CssClass="clsButton_Ajax" CausesValidation="False"
                                            Text="Print" Visible="False"></asp:Button>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnBack" runat="server" CssClass="clsButton_Ajax" ToolTip="Back to Previous Page"
                                            Text="Back"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr style="height: 0px;">
                <td>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
                        <ContentTemplate>
                            <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                CausesValidation="False" Style="display: none;"></asp:Button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <%--AJAX- Add UpdateProgress to show loading for Longer Process--%>
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
        <!-- File Upload Modal Dialog-->
        <div style="display: none">
            <asp:HiddenField runat="server" ID="btnDummyFileUpload" />
        </div>
        <asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%;
            width: 100%;">
            <iframe id="IFileUpload" allowtransparency="true" frameborder="0" height="100%" width="100%"
                src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupFileUpload" runat="server" TargetControlID="btnDummyFileUpload"
            PopupControlID="pnlFileUpload" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameFileUploadStateComplete() {
                $("#btnDummyFileUpload").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            $(document).ready(function () {
                $("#btnSelectFile").live("click", function () {
                    try {
                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#IFileUpload").attr("src", "wfFileUploadForSeparateTable.aspx");
                        if (!$.browser.msie) {
                            $("#btnDummyFileUpload").click();
                            $get("AjaxLoader").style.visibility = 'hidden';
                        }

                        return false;
                    } catch (e) {
                        alert(e);
                    }
                });
            }); 
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
        <!-- End -->
    </div>
    <div id="InfoMessagepanel" class="clsInfoMessage1" style="display: none; z-index: 100"
        draggable="true">
        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlLogInfo">
            <ContentTemplate>
                <table class="zui-table zui-table-rounded" style="z-index: 100" draggable="true">
                    <thead style="z-index: 100">
                        <tr>
                            <td colspan="4">
                                <span><b>List of Logs on selected date : </b></span><span><b>
                                    <%= mLogListOnDate.Count%></b></span> <span><b>Record(s)</b></span>
                                <%--<a class="close-btn" href="#" onclick="CloseLastDet();return false;">X</a>--%>
                            </td>
                        </tr>
                    </thead>
                    <thead style="z-index: 100">
                        <tr>
                            <th>
                                <span>Log No. &</span>
                                <br />
                                <span>Log Page No.</span>
                            </th>
                            <th>
                                <span>Departure Info</span>
                            </th>
                            <th>
                                <span>Arrival Info</span>
                            </th>
                            <th>
                                <span>Airborne Time</span>
                            </th>
                        </tr>
                    </thead>
                    <tbody style="z-index: 100">
                        <% Dim Child3 As LogInfo%>
                        <% For Each Child3 In mLogListOnDate%>
                        <tr>
                            <td>
                                <span>
                                    <%= Child3.LogTextNo %></span>
                                <br />
                                <span>
                                    <%= Child3.LogPageNoFormatted %></span>
                            </td>
                            <td>
                                <% If mMachine.IsUTC Then%>
                                <span>
                                    <%= Child3.SouUniverseDateTimeFormatted%></span>
                                <% Else%>
                                <span>
                                    <%= Child3.SouLocalDateTimeFormatted%></span>
                                <%End If%>
                                <% If Child3.LogTypeID = 1 Then%>
                                <span>
                                    <br />
                                    <%= Child3.SouPlaceName %></span>
                                <%End If%>
                            </td>
                            <td>
                                <% If mMachine.IsUTC Then%>
                                <span>
                                    <%= Child3.DesUniverseDateTimeFormatted%></span>
                                <% Else%>
                                <span>
                                    <%= Child3.DesLocalDateTimeFormatted%></span>
                                <%End If%>
                                <% If Child3.LogTypeID = 1 Then%>
                                <span>
                                    <br />
                                    <%= Child3.DesPlaceName %></span>
                                <%End If%>
                            </td>
                            <td>
                                <span>
                                    <%= Child3.TimeInAir %></span>
                            </td>
                        </tr>
                        <% Next%>
                    </tbody>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    
     <div id="pnlAllAssemblypanel" class="clsInfoMessage1" style="display: none; z-index: 100;"
        draggable="true">
        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlAssemblyInfo">
            <ContentTemplate>
                <div style="width: 100%">
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" CssClass="clsLabelHeader">ALL Assemblies</asp:Label>
                            </td>
                            <td align="right">
                                <span><a class="close-btn1" style="font-size: medium; color: Black" href="#" onclick="CloseAssemblyDet();return false;">
                                    X</a> </span>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="width: 100%">
                    <asp:GridView ID="grdAllAssemblies" runat="server" AutoGenerateColumns="False" Width="100%"
                        BorderStyle="Solid" CellPadding="0" ForeColor="#333333" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                        RowStyle-Wrap="false" HeaderStyle-Wrap="false" SelectedRowStyle-BackColor="ButtonShadow"
                        ShowHeaderWhenEmpty="True" PageSize="3" PagerSettings-Mode="NextPreviousFirstLast">
                        <RowStyle CssClass="clsdgItem" />
                        <HeaderStyle ForeColor="White" />
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="ID" Visible="False"></asp:BoundField>
                            <asp:BoundField DataField="ModelName" HeaderText="Model">
                                <HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="150px" />
                                <ItemStyle HorizontalAlign="Left" Wrap="false" Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
                                <HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="100px" />
                                <ItemStyle HorizontalAlign="Left" Wrap="false" Width="100px" />
                            </asp:BoundField>
                              <asp:BoundField DataField="AssemblyTypeCode" HeaderText="Type">
                                <HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="100px" />
                                <ItemStyle HorizontalAlign="Left" Wrap="false" Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Hours" HeaderText="Hours">
                                <HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="100px" />
                                <ItemStyle HorizontalAlign="Left" Wrap="false" Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FinalHours" HeaderText="Final Hours">
                                <HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="100px" />
                                <ItemStyle HorizontalAlign="Left" Wrap="false" Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Landings" HeaderText="Landings">
                                <HeaderStyle Font-Bold="true" HorizontalAlign="Left" Wrap="false" Width="100px" />
                                <ItemStyle HorizontalAlign="Left" Wrap="false" Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FinalLandings" HeaderText="Final Landings">
                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
                                <ItemStyle HorizontalAlign="Right" Wrap="false" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Cycles" HeaderText="Cycles">
                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
                                <ItemStyle HorizontalAlign="Right" Wrap="false" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FinalCycles" HeaderText="Final Cycles">
                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
                                <ItemStyle HorizontalAlign="Right" Wrap="false" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Starts" HeaderText="Starts">
                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
                                <ItemStyle HorizontalAlign="Right" Wrap="false" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FinalStarts" HeaderText="Final Starts">
                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
                                <ItemStyle HorizontalAlign="Right" Wrap="false" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NGCycles" HeaderText="NG Cycles">
                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
                                <ItemStyle HorizontalAlign="Right" Wrap="false" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FinalNGCycles" HeaderText="Final NG Cycles">
                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
                                <ItemStyle HorizontalAlign="Right" Wrap="false" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NFCycles" HeaderText="NF Cycles">
                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
                                <ItemStyle HorizontalAlign="Right" Wrap="false" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FinalNFCycles" HeaderText="Final NFCycles">
                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
                                <ItemStyle HorizontalAlign="Right" Wrap="false" />
                            </asp:BoundField>
                            <asp:BoundField DataField="RINS" HeaderText="RINS">
                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
                                <ItemStyle HorizontalAlign="Right" Wrap="false" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FinalRINS" HeaderText="Final RINS">
                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
                                <ItemStyle HorizontalAlign="Right" Wrap="false" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Bleeds" HeaderText="Bleeds">
                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
                                <ItemStyle HorizontalAlign="Right" Wrap="false" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FinalBleeds" HeaderText="Final Bleeds">
                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
                                <ItemStyle HorizontalAlign="Right" Wrap="false" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ImpellerCycles" HeaderText="ImpellerCycles">
                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
                                <ItemStyle HorizontalAlign="Right" Wrap="false" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FinalImpellerCycles" HeaderText="Final Impeller Cycles">
                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
                                <ItemStyle HorizontalAlign="Right" Wrap="false" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CTCycles" HeaderText="CT Cycles">
                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
                                <ItemStyle HorizontalAlign="Right" Wrap="false" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FinalCTCycles" HeaderText="Final CT Cycles">
                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
                                <ItemStyle HorizontalAlign="Right" Wrap="false" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PTCycles" HeaderText="PT Cycles">
                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
                                <ItemStyle HorizontalAlign="Right" Wrap="false" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FinalPTCycles" HeaderText="Final PT Cycles">
                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
                                <ItemStyle HorizontalAlign="Right" Wrap="false" />
                            </asp:BoundField>
                            <asp:BoundField DataField="GeneratorMods" HeaderText="Generator Mods">
                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
                                <ItemStyle HorizontalAlign="Right" Wrap="false" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FinalGeneratorMods" HeaderText="Final Generator Mods">
                                <HeaderStyle Font-Bold="true" HorizontalAlign="Right" Wrap="false" />
                                <ItemStyle HorizontalAlign="Right" Wrap="false" />
                            </asp:BoundField>
                        </Columns>
                        <SelectedRowStyle BackColor="ControlDark" />
                        <AlternatingRowStyle CssClass="clsdgAltItem" />
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
   
    <script type="text/javascript">
        function delete_cookie() {
            $.cookie('HideInfoMessagepanel', false);

        }
        function ShowLastDet() {
            $pos = $("#<%=lblDepPlace.ClientID%>").position();
            var top = $pos.top;
            var left = $pos.left;
            var searchHeight = $("#<%=lblDepPlace.ClientID%>").height();
            var margin = top + searchHeight;

            var height = $("#tblMain").outerHeight();
            var h = margin - height;
            if ($.cookie('HideInfoMessagepanel') == 'true') $("#InfoMessagepanel").hide();
            else {
                $.cookie('HideInfoMessagepanel', true);
                $("#InfoMessagepanel").css("display", "block");
                $("#InfoMessagepanel").animate({ marginTop: h, marginLeft: left - 5 }, 100, 'swing', function () {
                    $("#InfoMessagepanel").delay(9000).fadeOut();

                });
            }

        }
    </script>
   <%-- <script type="text/javascript">
        function ShowAssembly() {

            $pos = $("#<%=lnkAllAssembly.ClientID%>").position();
            var top = $pos.top;
            var left = $pos.left - 400;
            var searchHeight = $("#<%=lnkAllAssembly.ClientID%>").height();
            var margin = top + searchHeight;

            var height = $("#tblMain").outerHeight();
            var h = margin - height;
            $("#pnlAllAssemblypanel").css("display", "block");
            $("#pnlAllAssemblypanel").animate({ marginTop: h, marginLeft: left - 5 }, 100, 'swing', function () {
                //$("#pnlAllAssemblypanel").delay(9000).fadeOut();
            });
        }
    </script>--%>
    <script type="text/javascript">
        function CloseAssemblyDet() {

            $("#pnlAllAssemblypanel").hide();
        }
    </script>
    <script type="text/javascript">
        function delete_cookie() {
            $.cookie('HideInfoMessagepanel', null);
        }
    </script>
    </form>
    <script type="text/javascript">
        //AJAX- Replaced "$(document).ready(function(){" by " Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(){" as it gets fired only after complete PostBack
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            var logdate;
            //AJAX- Hidden Field value used here
            if (document.getElementById("LogObjValue").value == "True") {
                logdate = "button";
            }
            else {
                logdate = "";
            }
        });
    </script>
    <!--  For Arrival,Departure,Take off,Touch down Date Time   -->
    <script type="text/javascript">
        //AJAX- Replaced "$(document).ready(function(){" by " Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(){" as it gets fired only after complete PostBack
		Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(){
		var str="<%=System.Configuration.ConfigurationManager.AppSettings("TimeFormatLOG").ToString()%>";
        var bool;
        var savedlog;
         //AJAX- Hidden Field value used here
       	if  (document.getElementById("LogObjValue").value == "True")
		{
			savedlog="button";
		}
		else
		{
			savedlog="";
		}
        if (str.search("TT")=== -1 && str.search("tt")=== -1)
        {
			bool=false;
        }
		else
		{
			bool=true;
		}
		
		 
		});
		
    </script>
    <script type="text/javascript">
        //AJAX- Replaced "$(document).ready(function(){" by " Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(){" as it gets fired only after complete PostBack
		Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(){
		var str="<%=System.Configuration.ConfigurationManager.AppSettings("TimeFormatLOG").ToString()%>";
        var bool1;
        var savedlog1;
         //AJAX- Hidden Field value used here
       	if  (document.getElementById("LogObjValue").value == "True")
		{
			savedlog1="button";
		}
		else
		{
			savedlog1="";
		}
        if (str.search("TT")=== -1 && str.search("tt")=== -1)
        {
			bool1=false;
        }
		else
		{
			bool1=true;
		}
	});
    </script>
    <!-- Autocomplete for Source and Destination Place   -->
    <script type="text/javascript">
        //AJAX- Replaced "$(document).ready(function(){" by " Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(){" as it gets fired only after complete PostBack
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=Place1.ClientID%>,#<%=Place2.ClientID%>").autocomplete('wfAutoPilotPlace.aspx?Type=Place', {
                width: 200,
                autoFill: true,
                matchContains: true,
                delay: 0


            });
        });
    </script>
    <script type="text/javascript">
        //AJAX- Replaced "$(document).ready(function(){" by " Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(){" as it gets fired only after complete PostBack
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=Pilot1.ClientID%>").autocomplete('wfAutoPilotPlace.aspx?Type=Pilot', {
                autoFill: true,
                width: 252,
                mustMatch: true,
                matchContains: true,
                delay: 0
            });
        });       
    </script>
    <script type="text/javascript">
        //AJAX- Replaced "$(document).ready(function(){" by " Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(){" as it gets fired only after complete PostBack
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%=Pilot2.ClientID%>").autocomplete('wfAutoPilotPlace.aspx?Type=Pilot', {
                autoFill: true,
                width: 256,
                mustMatch: true,
                matchContains: true,
                delay: 0
            });
        });       
    </script>
    <script type="text/javascript">
        function AfterSave(IsShowDateCntrl) {
        }
    
    </script>
    <script type="text/javascript">
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
        function CloseLastDet() {
            $("#InfoMessagepanel").delay(9000).fadeOut();
        }
    </script>
</body>
</html>
