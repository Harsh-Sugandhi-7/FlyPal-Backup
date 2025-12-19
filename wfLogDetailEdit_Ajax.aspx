<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfLogDetailEdit_Ajax.aspx.vb"
   Inherits="Flypal.wfLogDetailEdit_Ajax" %>

<%@ Register TagPrefix="uc1" TagName="SICalendar" Src="SICalendar.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="Flypal.LogList" %>
<%@ Import Namespace="Flypal.Log" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%--AJAX- Changed DOCTYPE from 4.0 to 1.0--%>
<%--AJAX- Register "AjaxControlToolkit & User Control "MSGBOX"--%>
<html>
<head runat="server">
    <title>Log Details</title>
    <meta http-equiv="x-ua-compatible" content="IE=9">
    <meta name="vs_showGrid" content="True" />
    <script language="javascript" src="VALIDATEFUNCTIONS.js" type="text/javascript"></script>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1" />
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1" />
    <meta name="vs_defaultClientScript" content="JavaScript" />
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
    <link id="MainStyle" type="text/css" rel="stylesheet">
    <asp:PlaceHolder runat="server">
        <%--AJAX- Replaced "LocalFunction.htm" to "LocalFunctionAjax.htm"--%>
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script src="StickyNote/js/jquery.cookie.js" type="text/javascript"></script>
    <script id="clientEventHandlersJS" language="javascript" type="text/javascript">
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
</head>
<body bottommargin="5" leftmargin="5" rightmargin="5" topmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
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
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <table id="tblMain" class="clstablelistout">
            <tr>
                <td>
                    <asp:Panel ID="pnlMain" CssClass="clspnl1" runat="server">
                        <table id="tblinner" class="clsTablelistin" cellpadding="0">
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
                                    <%--AJAX- Add UpdatePanel for tabs buttons --%>
                                    <asp:UpdatePanel ID="upnlTabs" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblLogDetails" runat="server" CssClass="clsLabelButton" ToolTip="Log details">Log details</asp:Label>
                                                                </td>
                                                               <%-- <td>
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
                                            <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                                HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                            <asp:CustomValidator ID="cvRemark" runat="server" ErrorMessage="Remark Can't be greater than 200 chars"
                                                ControlToValidate="txtRemark" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvAirFrame" runat="server" Display="None" OnServerValidate="customvalidate1"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvDepartureDateTime" runat="server" ErrorMessage="Departure date should be in date time format."
                                                ControlToValidate="calDeparture" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvArrivalDateTime" runat="server" ErrorMessage="Arrival date should be in date time format."
                                                ControlToValidate="calArrival" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvGroundRunTime" runat="server" ErrorMessage="Departure date should be in date time format."
                                                ControlToValidate="txtGroundRunTime" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                            <asp:CustomValidator ID="cvAirBornTime" runat="server" ErrorMessage="Not be Nigative."
                                                ControlToValidate="txtAirBorneTime" Display="None" OnServerValidate="customvalidate"></asp:CustomValidator>
                                            <asp:RequiredFieldValidator ID="rfvDate" runat="server" CssClass="clsLabelAuto" ErrorMessage="Log Date Required."
                                                ControlToValidate="calDateTime" Display="None"></asp:RequiredFieldValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%--AJAX- Add UpdatePanel for log Details --%>
                                    <asp:UpdatePanel ID="upnlLogDetails" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table id="Table4" class="clsTable1" border="0" cellspacing="1" cellpadding="1" width="100%">
                                                <tr>
                                                    <td>
                                                        <table id="Table6" border="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblCalDate" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblDateTime" runat="server" CssClass="clsLabelAuto">Date</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <uc1:SICalendar ID="calDateTime" runat="server"></uc1:SICalendar>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblLogNo" runat="server" CssClass="clsLabelAuto">Log No.</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtLogText" runat="server" CssClass="clsTextBoxDate_Ajax" ToolTip="Log Number"
                                                                        Text="<%# mLog.LogText %>" ReadOnly="True" BackColor="#E0E0E0"></asp:TextBox>
                                                                    <asp:TextBox ID="txtLogNo" runat="server" CssClass="clsTextBoxMedium_Ajax" Text="<%# mLog.LogNo %>"
                                                                        ReadOnly="True" BackColor="#E0E0E0"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <span id="SIDate" runat="server"></span>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
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
                                                                    <asp:TextBox ID="txtPilot1" runat="server" CssClass="clsTextBoxDate_Ajax" ToolTip="Pilot #1 Name"
                                                                        Text="<%# mLog.Pilot1Name %>" ReadOnly="True" BackColor="#E0E0E0"></asp:TextBox>
                                                                    <asp:ImageButton ID="imgbtnPilot1" runat="server" CssClass="clsButtonImg_Ajax" ToolTip="Select Pilot #1"
                                                                        CausesValidation="False" ImageUrl="ICONS/ADD.ICO"></asp:ImageButton>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
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
                                                                                            runat="server" class="clsButton_Ajax" causesvalidation="False" tabindex="13" />
                                                                                    </td>
                                                                                    <td style="padding-left: 3px;">
                                                                                        <asp:Button ID="btnDelAttch" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Remove Attachment"
                                                                                            Text="Remove Attachment" Enabled="False" Width="120px" TabIndex="14"></asp:Button>
                                                                                    </td>
                                                                                    <td style="padding-left: 2px;">
                                                                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" ImageUrl="icons/CLIP01.ICO"
                                                                                            Height="20px" Width="20px" TabIndex="15"></asp:ImageButton>
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
                                                        <table id="Table20" border="0" width="100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblLogPageNo" runat="server" CssClass="clsLabelAuto">Page No.</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <table id="Table3" border="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox ID="txtLogPageNo" runat="server" CssClass="clsTextBoxMedium_Ajax" ToolTip="Enter Log Page No."
                                                                                    Text="<%# mLog.LogPageNoFormatted %>" MaxLength="9"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblFlightNo" runat="server" CssClass="clsLabelAuto">Flight No.</asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtFlightNo" runat="server" CssClass="clsTextBoxMedium_Ajax" ToolTip="Enter Flight No."
                                                                                    Text="<%# mLog.FlightNo %>" MaxLength="10"></asp:TextBox>
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
                                                                    <asp:TextBox ID="txtPilot2" runat="server" CssClass="clsTextBoxDate_Ajax" ToolTip="Pilot #2 Name"
                                                                        Text="<%# mLog.Pilot2Name %>" ReadOnly="True" BackColor="#E0E0E0"></asp:TextBox>
                                                                    <asp:ImageButton ID="imgbtnPilot2" runat="server" CssClass="clsButtonImg_Ajax" ToolTip="Select Pilot #2"
                                                                        CausesValidation="False" ImageUrl="ICONS/ADD.ICO"></asp:ImageButton>
                                                                    <asp:Button ID="btnAddPilot" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add new pilot"
                                                                        Text="Add Pilots"></asp:Button>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblFlightLogClassification" runat="server" CssClass="clsLabelAuto">Classification</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbFlightLogClassification" runat="server" CssClass="clsComboBox_Ajax"
                                                                        Width="250px" DataTextField="Name" DataValueField="ID">
                                                                    </asp:DropDownList>
                                                                    <asp:Button ID="btnFlightLogClassification" runat="server" CssClass="clsButton_Ajax" Visible="false" 
                                                                        ToolTip="Click to Add new Classification" CausesValidation="False" Text="Add Classification"
                                                                        Width="111px"></asp:Button>
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
                                    <%--AJAX- Add UpdatePanel for Flight Details --%>
                                    <asp:UpdatePanel ID="upnlFlightDetails" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <table width="100%">
                                                            <tr>
                                                                <td colspan="5">
                                                                    <asp:Label ID="lblDeparture" runat="server" CssClass="clstitle2_ajax">Departure</asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblPalceStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblDepPlace" runat="server" CssClass="clsLabelAuto">Place</asp:Label>
                                                                </td>
                                                                <td colspan="3">
                                                                    <asp:TextBox ID="txtDepPlace" runat="server" CssClass="clsTextBoxDate_Ajax" ToolTip="Place "
                                                                        Text="<%# mLog.SourceName %>" ReadOnly="True" BackColor="#E0E0E0" Width="168px"></asp:TextBox>
                                                                    <asp:Button ID="btnAddDepPlace" runat="server" CssClass="clsButtonGrid_Ajax" Style="z-index: 0"
                                                                        Text="..." ToolTip="Click To Add Place" Visible="False" />
                                                                    <asp:ImageButton ID="imgbtnDepPlace" runat="server" CssClass="clsButtonImg_Ajax"
                                                                        ToolTip="Select Place" CausesValidation="False" ImageUrl="ICONS/ADD.ICO" Enabled="<%# mLog.IsNew %>">
                                                                    </asp:ImageButton>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblDateTimeStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblDepDateTime" runat="server" CssClass="clsLabelAuto">Date/Time</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <uc1:SICalendar ID="calDeparture" runat="server" CausesValidation="True"></uc1:SICalendar>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblDepDayLightTime" runat="server" CssClass="clsLabelAuto">D/L Time</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbDepartureDayLightTime" runat="server" CssClass="clsComboBoxsmall_Ajax"
                                                                        DataTextField="Name" DataValueField="ID" SelectedValue="<%# mLog.SouDayLightTime %>">
                                                                        <asp:ListItem Value="-12:00" Selected="True">-12:00</asp:ListItem>
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
                                                                    <asp:Label ID="lblUTCDateTimeStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblUTCDateTime" runat="server" CssClass="clsLabelAuto">UTC Date/Time</asp:Label>
                                                                </td>
                                                                <td colspan="3">
                                                                    <uc1:SICalendar ID="CalUTCDateTime" runat="server" CausesValidation="True"></uc1:SICalendar>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>
                                                        <table width="100%">
                                                            <tr>
                                                                <td colspan="5">
                                                                    <asp:Label ID="lblArival" runat="server" CssClass="clstitle2_ajax">Arrival</asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblPlaceStar2" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblArrPlace" runat="server" CssClass="clsLabelAuto">Place</asp:Label>
                                                                </td>
                                                                <td colspan="3">
                                                                    <asp:TextBox ID="txtArrPlace" runat="server" CssClass="clsTextBoxDate_Ajax" ToolTip="Place "
                                                                        Text="<%# mLog.DestinationName %>" ReadOnly="True" BackColor="#E0E0E0" Width="168px"></asp:TextBox>
                                                                    <asp:Button ID="btnAddArrPlace" runat="server" CssClass="clsButtonGrid_Ajax" Style="z-index: 0"
                                                                        Text="..." ToolTip="Click To Add Place" Visible="False" />
                                                                    <asp:ImageButton ID="imgbtnArrPlace" runat="server" CssClass="clsButtonImg_Ajax"
                                                                        ToolTip="Select Place" CausesValidation="False" ImageUrl="ICONS/ADD.ICO" Enabled="<%# mLog.IsNew %>">
                                                                    </asp:ImageButton>
                                                                    <asp:Button ID="btnAddPlaces" runat="server" CssClass="clsButton_Ajax" ToolTip="Click to Add new Place" Visible="false" 
                                                                        Text="Add Places"></asp:Button>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblDateTimeStar2" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblArrDate" runat="server" CssClass="clsLabelAuto">Date/Time</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <uc1:SICalendar ID="calArrival" runat="server" CausesValidation="True"></uc1:SICalendar>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblArrDayLightTime" runat="server" CssClass="clsLabelAuto">D/L Time</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbArrivalDayLightTime" runat="server" CssClass="clsComboBoxsmall_Ajax"
                                                                        DataTextField="Name" DataValueField="ID" SelectedValue="<%# mLog.DesDayLightTime %>">
                                                                        <asp:ListItem Value="-12:00" Selected="True">-12:00</asp:ListItem>
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
                                                                    <asp:Label ID="lblUTCDateTimeStar2" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblUTCArrivalDateTime" runat="server" CssClass="clsLabelAuto">UTC DateTime</asp:Label>
                                                                </td>
                                                                <td colspan="3">
                                                                    <uc1:SICalendar ID="CalUTCArrival" runat="server" CausesValidation="True"></uc1:SICalendar>
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
                                                                        <table id="tabAircraft" border="0" cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblairfly" runat="server" CssClass="clsLabelAuto">Block Time</asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtBlockTime" runat="server" CssClass="clsTextBoxMedium_Ajax" Text="<%# mLog.DiffTime %>"
                                                                                        AutoPostBack="true" Visible="False" Enabled='<%# iif(AppSettings("SetBlockTime") = "True",True,False) %>'></asp:TextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblAirBorneTime" runat="server" CssClass="clsLabelAuto">Airborne Time </asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtAirBorneTime" runat="server" CssClass="clsTextBoxMedium_Ajax"
                                                                                        Text="<%# mLog.TimeInAir %>" ReadOnly="<%# mLog.ShowTimeTextBoxes %>"
                                                                                        Visible="False" AutoPostBack="True"></asp:TextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblGroundRunTime" runat="server" CssClass="clsLabelAuto">Ground Run Time </asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtGroundRunTime" runat="server" CssClass="clsTextBoxMedium_Ajax"
                                                                                        Enabled='<%# iif(AppSettings("SetBlockTime") = "True",False,True) %>' Text="<%# mLog.TimeOnGround %>"
                                                                                        ReadOnly="<%# mlog.ShowTimeOnGround  %>" Visible="False" AutoPostBack="True"></asp:TextBox>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblPercentTimeOnGround" runat="server" CssClass="clsLabelAuto">%Ground Run Time </asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtPercentTimeOnGround" runat="server" CssClass="clsTextBoxMedium_Ajax"
                                                                                        Text="<%# mLog.PercentTimeOnGround %>"  Visible="False"
                                                                                        AutoPostBack="True"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Panel ID="pnlDecimal" runat="server" CssClass="clsPanel1" Visible="False">
                                                                        <table id="tabHobbs" border="0" cellspacing="0" cellpadding="0">
                                                                            <%-- <tr>
                                                                                <td colspan="2">
                                                                                    <asp:Label ID="lblHobbsread" runat="server" CssClass="clsLabelAuto">HOBBS READING :  </asp:Label>
                                                                                </td>
                                                                            </tr>--%>
                                                                            <tr>
                                                                                <td>
                                                                                    <%--  <fieldset style="padding: 4px; height: 50px;">
                                                                                        <legend><b>Previous Value</b></legend>--%>
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="lblHobbsread" runat="server" CssClass="clsLabelAuto">HOBBS 
                                                                                                READING :&nbsp;&nbsp;  </asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto">Previous Value :</asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lblHobbsPrevVal" runat="server" CssClass="clsLabelAuto">Offset </asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtPrevHobbsOffset" runat="server" CssClass="clsTextBoxMedium_Ajax"
                                                                                                    Text="<%# mLog.PrevHobbsOffsetValue %>" ReadOnly="True" BackColor="#E0E0E0" Visible="False"></asp:TextBox>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lblHobbsCurrentReading" runat="server" CssClass="clsLabelAuto">Reading </asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtPrevHobbsValue" runat="server" CssClass="clsTextBoxMedium_Ajax"
                                                                                                    Text="<%# mLog.PrevHobbsValue %>" ReadOnly="True" BackColor="#E0E0E0" Visible="False"
                                                                                                    Enabled="False"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <%--</fieldset>--%>
                                                                                </td>
                                                                                <td>
                                                                                    <%--<fieldset style="padding: 4px; height: 50px;">
                                                                                        <legend><b>Current Value</b></legend>--%>
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="Label2" runat="server" CssClass="clsLabelAuto">Current Value :</asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lblOffsetPreVal" runat="server" CssClass="clsLabelAuto">Offset </asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtCurrentHobbsOffset" runat="server" CssClass="clsTextBoxMedium_Ajax"
                                                                                                    Text="<%# mLog.CurrentHobbsOffsetValue %>" ReadOnly="True" BackColor="#E0E0E0"
                                                                                                    Visible="False" Enabled="False"></asp:TextBox>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lblOffsetCurrentVal" runat="server" CssClass="clsLabelAuto">Reading </asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtCurrentHobbsValue" runat="server" CssClass="clsTextBoxMedium_Ajax"
                                                                                                    Text="<%# mLog.CurrentHobbsValue %>" Visible="False" AutoPostBack="True"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <%-- </fieldset>--%>
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
                                            <td align="right">
                                                <asp:LinkButton ID="lnkAllAssembly" runat="server" CssClass="clsLinkButton" Font-Italic="true"
                                                    Font-Size="9pt" ToolTip="Click to go on All Assembly screen" ClientIDMode="Static"
                                                    Visible="<%#  (mLog.IsShowAssemblyRequired) %>">Show All Assembly</asp:LinkButton>
                                            </td>
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
                                                    <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                    <asp:BoundField DataField="ModelName" HeaderText="Model">
                                                        <HeaderStyle Width="150px" HorizontalAlign="Left"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Left" Width="150px"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
                                                        <HeaderStyle Width="100px"></HeaderStyle>
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Hours">
                                                        <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtAirFrameHours" runat="server" ToolTip="Enter the Hours." CssClass="clsTextBoxMegaSmall_Ajax"
                                                                Text='<%# DataBinder.Eval(Container.DataItem,"Hours") %>' ReadOnly="<%# Not mLog.IsNew %>"
                                                                AutoPostBack="true" OnTextChanged="txtAirFrameHours_TextChanged" Width="93%"
                                                                onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            </asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="FinalHours" HeaderText="Final Hours">
                                                        <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Landings">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtAirFrameLandings" runat="server" ToolTip="Enter the Landing."
                                                                CssClass="clsTextBoxMegaSmall_Ajax" Text='<%# DataBinder.Eval(Container.DataItem,"Landings") %>'
                                                                AutoPostBack="true" OnTextChanged="txtAirFrameLandings_TextChanged" Width="93%"
                                                                onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            </asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                                        <ItemStyle HorizontalAlign="Right" Width="75px" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="FinalLandings" HeaderText="Final Landings">
                                                        <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Cycles">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtAirFrameCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                                ToolTip="Enter Cycles." Text='<%# DataBinder.Eval(Container.DataItem,"Cycles") %>'
                                                                AutoPostBack="true" OnTextChanged="txtAirFrameCycles_TextChanged" Width="93%"
                                                                onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            </asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Right" Width="75px" />
                                                        <ItemStyle HorizontalAlign="Right" Width="75px" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="FinalCycles" HeaderText="Final Cycles">
                                                        <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Starts">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtAirFrameStarts" runat="server" ToolTip="Enter Start Time." CssClass="clsTextBoxMegaSmall_Ajax"
                                                                Text='<%# DataBinder.Eval(Container.DataItem,"Starts") %>' AutoPostBack="true"
                                                                OnTextChanged="txtAirFrameStarts_TextChanged" Width="93%" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                                onfocus="onTextFocus();">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            </asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="FinalStarts" HeaderText="Final Starts">
                                                        <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="NG Cycles">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtAirFrameNGCycles" runat="server" ToolTip="Enter Start Time."
                                                                CssClass="clsTextBoxMegaSmall_Ajax" Text='<%# DataBinder.Eval(Container.DataItem,"NGCycles") %>'
                                                                AutoPostBack="true" OnTextChanged="txtAirFrameNGCycles_TextChanged" Width="93%"
                                                                onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            </asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="FinalNGCycles" HeaderText="Final NGCycles">
                                                        <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="NF Cycles">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtAirFrameNFCycles" runat="server" ToolTip="Enter Start Time."
                                                                CssClass="clsTextBoxMegaSmall_Ajax" Text='<%# DataBinder.Eval(Container.DataItem,"NFCycles") %>'
                                                                AutoPostBack="true" OnTextChanged="txtAirFrameNFCycles_TextChanged" Width="93%"
                                                                onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            </asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="FinalNFCycles" HeaderText="Final NFCycles">
                                                        <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="RINS">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtAirFrameRins" runat="server" ToolTip="Enter Start Time." CssClass="clsTextBoxMegaSmall_Ajax"
                                                                Text='<%# DataBinder.Eval(Container.DataItem,"RINS") %>' AutoPostBack="true"
                                                                OnTextChanged="txtAirFrameRins_TextChanged" Width="93%" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                                onfocus="onTextFocus();">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            </asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="FinalRINS" HeaderText="Final RINS">
                                                        <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Bleeds">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtAirFrameBleeds" runat="server" ToolTip="Enter Bleeds" CssClass="clsTextBoxMegaSmall_Ajax"
                                                                Text='<%# DataBinder.Eval(Container.DataItem,"Bleeds") %>' AutoPostBack="true"
                                                                OnTextChanged="txtAirFrameBleeds_TextChanged" Width="93%" onkeydown="onkeyPressed(window.event.keyCode,this);"
                                                                onfocus="onTextFocus();">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            </asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="FinalBleeds" HeaderText="Final Bleeds">
                                                        <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Impeller Cycles">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtAirFrameImpellerCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                                ToolTip="Enter Impeller Cycles" Text='<%# DataBinder.Eval(Container.DataItem,"ImpellerCycles") %>'
                                                                AutoPostBack="true" OnTextChanged="txtAirFrameImpellerCycles_TextChanged" Width="93%"
                                                                onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            </asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="FinalImpellerCycles" HeaderText="Final Impeller Cycles">
                                                        <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="CT Cycles">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtAirFrameCTCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                                ToolTip="Enter CT Cycles" Text='<%# DataBinder.Eval(Container.DataItem,"CTCycles") %>'
                                                                AutoPostBack="true" OnTextChanged="txtAirFrameCTCycles_TextChanged" Width="93%"
                                                                onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            </asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="FinalCTCycles" HeaderText="Final CT Cycles">
                                                        <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="PT Cycles">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtAirFramePTCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                                ToolTip="Enter PT Cycles" Text='<%# DataBinder.Eval(Container.DataItem,"PTCycles") %>'
                                                                AutoPostBack="true" OnTextChanged="txtAirFramePTCycles_TextChanged" Width="93%"
                                                                onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            </asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="FinalPTCycles" HeaderText="Final PT Cycles">
                                                        <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Generator Mods">
                                                        <ItemTemplate>
                                                            <asp:TextBox Style="z-index: 0" ID="txtAirframeGeneratorMods" runat="server" ToolTip="Enter the Generator Mods."
                                                                CssClass="clsTextBoxMegaSmall_Ajax" Text='<%# DataBinder.Eval(Container.DataItem,"GeneratorMods") %>'
                                                                AutoPostBack="true" OnTextChanged="txtAirframeGeneratorMods_TextChanged" Width="93%"
                                                                onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            </asp:TextBox>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="FinalGeneratorMods" HeaderText="Final Generator Mods">
                                                        <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
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
                                    <%--AJAX- Add UpdatePanel for Engine Grid--%>
                                    <asp:UpdatePanel ID="upnlEngineDetail" runat="server" UpdateMode="Conditional">
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
                                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                        <asp:BoundField DataField="ModelName" HeaderText="Model">
                                                            <HeaderStyle Width="150px" HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Left" Width="150px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
                                                            <HeaderStyle Width="100px"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Hours">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtEngineHours" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                                    ToolTip="Enter the Engine Generator Mods." Text='<%# DataBinder.Eval(Container.DataItem,"Hours") %>'
                                                                    ReadOnly="<%# Not mLog.IsNew %>" AutoPostBack="true" OnTextChanged="txtEngineHours_TextChanged"
                                                                    Width="93%" onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                                            
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                                                           
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalHours" HeaderText="Final Hours">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Landings">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtEngineLandings" runat="server" ToolTip="Enter the Landing." CssClass="clsTextBoxMegaSmall_Ajax"
                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"Landings") %>' AutoPostBack="true"
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    OnTextChanged="txtEngineLandings_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalLandings" HeaderText="Final Landings">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Cycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtEngineCycles" runat="server" ToolTip="Enter Cycles." CssClass="clsTextBoxMegaSmall_Ajax"
                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"Cycles") %>' AutoPostBack="true"
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    OnTextChanged="txtEngineCycles_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalCycles" HeaderText="Final Cycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Starts">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtEngineStarts" runat="server" ToolTip="Enter Start Time." CssClass="clsTextBoxMegaSmall_Ajax"
                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"Starts") %>' AutoPostBack="true"
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    OnTextChanged="txtEngineStarts_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalStarts" HeaderText="Final Starts">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="NG Cycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtEngineNGCycles" runat="server" ToolTip="Enter Start Time." CssClass="clsTextBoxMegaSmall_Ajax"
                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"NGCycles") %>' AutoPostBack="true"
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    OnTextChanged="btnEngineNGCycles_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalNGCycles" HeaderText="Final NGCycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="NF Cycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtEngineNFCycles" runat="server" ToolTip="Enter Start Time." CssClass="clsTextBoxMegaSmall_Ajax"
                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"NFCycles") %>' AutoPostBack="true"
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    OnTextChanged="txtEngineNFCycles_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalNFCycles" HeaderText="Final NFCycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="RINS">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtEngineRins" runat="server" ToolTip="Enter Start Time." CssClass="clsTextBoxMegaSmall_Ajax"
                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"RINS") %>' AutoPostBack="true"
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    OnTextChanged="txtEngineRins_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalRINS" HeaderText="Final RINS">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Contingency Factor">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtEngineCFactors" runat="server" ToolTip="Enter Contingency Factor."
                                                                    CssClass="clsTextBoxMegaSmall_Ajax" Text='<%# DataBinder.Eval(Container.DataItem,"CFactor") %>'
                                                                    AutoPostBack="true" OnTextChanged="txtEngineCFactors_TextChanged" Width="97%"
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalCFactor" HeaderText="Final Contingency Factor">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Bleeds">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtEngineBleeds" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                                    ToolTip="Enter Bleeds" Text='<%# DataBinder.Eval(Container.DataItem,"Bleeds") %>'
                                                                    AutoPostBack="true" onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    OnTextChanged="txtEngineBleeds_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalBleeds" HeaderText="Final Bleeds">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Impeller Cycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtEngineImpellerCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                                    ToolTip="Enter Impeller Cycles" Text='<%# DataBinder.Eval(Container.DataItem,"ImpellerCycles") %>'
                                                                    AutoPostBack="true" OnTextChanged="txtEngineImpellerCycles_TextChanged" Width="93%"
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalImpellerCycles" HeaderText="Final Impeller Cycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="CT Cycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtEngineCTCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                                    ToolTip="Enter CT Cycles" Text='<%# DataBinder.Eval(Container.DataItem,"CTCycles") %>'
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    AutoPostBack="true" OnTextChanged="txtEngineCTCycles_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalCTCycles" HeaderText="Final CT Cycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="PT Cycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtEnginePTCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                                    ToolTip="Enter PT Cycles" Text='<%# DataBinder.Eval(Container.DataItem,"PTCycles") %>'
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    AutoPostBack="true" OnTextChanged="txtEnginePTCycles_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalPTCycles" HeaderText="Final PT Cycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Generator Mods">
                                                            <ItemTemplate>
                                                                <asp:TextBox Style="z-index: 0" ID="txtEngineGeneratorMods" runat="server" ToolTip="Enter the Generator Mods."
                                                                    CssClass="clsTextBoxMegaSmall_Ajax" Text='<%# DataBinder.Eval(Container.DataItem,"GeneratorMods") %>'
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    AutoPostBack="true" OnTextChanged="txtEngineGeneratorMods_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalGeneratorMods" HeaderText="Final Generator Mods">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
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
                                    <%--AJAX- Add UpdatePanel for APU Grid--%>
                                    <asp:UpdatePanel ID="upnlAPUDetail" runat="server" UpdateMode="Conditional">
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
                                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                        <asp:BoundField DataField="ModelName" HeaderText="Model">
                                                            <HeaderStyle Width="150px" HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Left" Width="150px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
                                                            <HeaderStyle Width="100px"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Hours">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtAPUHours" runat="server" ToolTip="Enter the Hours." CssClass="clsTextBoxMegaSmall_Ajax"
                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"Hours") %>' AutoPostBack="true"
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    OnTextChanged="txtAPUHours_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalHours" HeaderText="Final Hours">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Landings">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtAPULandings" runat="server" ToolTip="Enter the Landing." CssClass="clsTextBoxMegaSmall_Ajax"
                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"Landings") %>' AutoPostBack="true"
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    OnTextChanged="txtAPULandings_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalLandings" HeaderText="Final Landings">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Cycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtAPUCycles" runat="server" ToolTip="Enter Cycles." CssClass="clsTextBoxMegaSmall_Ajax"
                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"Cycles") %>' AutoPostBack="true"
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    OnTextChanged="txtAPUCycles_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalCycles" HeaderText="Final Cycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Starts">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtAPUStarts" runat="server" ToolTip="Enter Start Time." CssClass="clsTextBoxMegaSmall_Ajax"
                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"Starts") %>' AutoPostBack="true"
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    OnTextChanged="txtAPUStarts_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalStarts" HeaderText="Final Starts">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="NG Cycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtAPUNGCycles" runat="server" ToolTip="Enter Start Time." CssClass="clsTextBoxMegaSmall_Ajax"
                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"NGCycles") %>' AutoPostBack="true"
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    OnTextChanged="txtAPUNGCycles_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalNGCycles" HeaderText="Final NGCycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="NF Cycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtAPUNFCycles" runat="server" ToolTip="Enter Start Time." CssClass="clsTextBoxMegaSmall_Ajax"
                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"NFCycles") %>' AutoPostBack="true"
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    OnTextChanged="txtAPUNFCycles_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalNFCycles" HeaderText="Final NFCycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="RINS">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtAPURins" runat="server" ToolTip="Enter Start Time." CssClass="clsTextBoxMegaSmall_Ajax"
                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"RINS") %>' AutoPostBack="true"
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    OnTextChanged="txtAPURins_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalRINS" HeaderText="Final RINS">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Bleeds">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtAPUBleeds" runat="server" ToolTip="Enter Bleeds" CssClass="clsTextBoxMegaSmall_Ajax"
                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"Bleeds") %>' AutoPostBack="true"
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    OnTextChanged="txtAPUBleeds_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalBleeds" HeaderText="Final Bleeds">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Impeller Cycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtAPUImpellerCycles" runat="server" ToolTip="Enter Impeller Cycles"
                                                                    CssClass="clsTextBoxMegaSmall_Ajax" Text='<%# DataBinder.Eval(Container.DataItem,"ImpellerCycles") %>'
                                                                    AutoPostBack="true" OnTextChanged="txtAPUImpellerCycles_TextChanged" Width="93%"
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalImpellerCycles" HeaderText="Final Impeller Cycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="CT Cycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtAPUCTCycles" runat="server" ToolTip="Enter CT Cycles" CssClass="clsTextBoxMegaSmall_Ajax"
                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"CTCycles") %>' AutoPostBack="true"
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    OnTextChanged="txtAPUCTCycles_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalCTCycles" HeaderText="Final CT Cycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="PT Cycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtAPUPTCycles" runat="server" ToolTip="Enter PT Cycles" CssClass="clsTextBoxMegaSmall_Ajax"
                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"PTCycles") %>' AutoPostBack="true"
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    OnTextChanged="txtAPUPTCycles_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalPTCycles" HeaderText="Final PT Cycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Generator Mods">
                                                            <ItemTemplate>
                                                                <asp:TextBox Style="z-index: 0" ID="txtAPUGeneratorMods" runat="server" ToolTip="Enter the Generator Mods."
                                                                    CssClass="clsTextBoxMegaSmall_Ajax" Text='<%# DataBinder.Eval(Container.DataItem,"GeneratorMods") %>'
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    AutoPostBack="true" OnTextChanged="txtAPUGeneratorMods_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalGeneratorMods" HeaderText="Final Generator Mods">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
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
                                    <%--AJAX- Add UpdatePanel for CGB Grid--%>
                                    <asp:UpdatePanel ID="upnlCGBDetail" runat="server" UpdateMode="Conditional">
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
                                                        <asp:BoundField Visible="False" DataField="ID" HeaderText="ID"></asp:BoundField>
                                                        <asp:BoundField DataField="ModelName" HeaderText="Model">
                                                            <HeaderStyle Width="150px" HorizontalAlign="Left"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Left" Width="150px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="SerialNo" HeaderText="Serial No.">
                                                            <HeaderStyle Width="100px"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Hours">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtCGBHours" runat="server" ToolTip="Enter the Hours." CssClass="clsTextBoxMegaSmall_Ajax"
                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"Hours") %>' Width="93%" AutoPostBack="true"
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    OnTextChanged="txtCGBHours_TextChanged">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalHours" HeaderText="Final Hours">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Landings">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtCGBLandings" runat="server" ToolTip="Enter the Landing." CssClass="clsTextBoxMegaSmall_Ajax"
                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"Landings") %>' AutoPostBack="true"
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    OnTextChanged="txtCGBLandings_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalLandings" HeaderText="Final Landings">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Cycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtCGBCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                                    ToolTip="Enter Cycles." Text='<%# DataBinder.Eval(Container.DataItem,"Cycles") %>'
                                                                    AutoPostBack="true" onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    OnTextChanged="txtCGBCycles_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalCycles" HeaderText="Final Cycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Starts">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtCGBStarts" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                                    ToolTip="Enter Start Time." Text='<%# DataBinder.Eval(Container.DataItem,"Starts") %>'
                                                                    AutoPostBack="true" onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    OnTextChanged="txtCGBStarts_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalStarts" HeaderText="Final Starts">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="NG Cycles">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtCGBNGCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                                    ToolTip="Enter Start Time." Text='<%# DataBinder.Eval(Container.DataItem,"NGCycles") %>'
                                                                    AutoPostBack="true" onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    OnTextChanged="txtCGBNGCycles_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalNGCycles" HeaderText="Final NGCycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="NF Cycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtCGBNFCycles" runat="server" CssClass="clsTextBoxMegaSmall_Ajax"
                                                                    ToolTip="Enter Start Time." Text='<%# DataBinder.Eval(Container.DataItem,"NFCycles") %>'
                                                                    AutoPostBack="true" onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    OnTextChanged="txtCGBNFCycles_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalNFCycles" HeaderText="Final NFCycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="RINS">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtCGBRins" runat="server" ToolTip="Enter Start Time." CssClass="clsTextBoxMegaSmall_Ajax"
                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"RINS") %>' AutoPostBack="true"
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    OnTextChanged="txtCGBRINS_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalRINS" HeaderText="Final RINS">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Bleeds">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtCGBBleeds" runat="server" ToolTip="Enter Bleeds" CssClass="clsTextBoxMegaSmall_Ajax"
                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"Bleeds") %>' AutoPostBack="true"
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    OnTextChanged="txtCGBBleeds_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalBleeds" HeaderText="Final Bleeds">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Impeller Cycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtCGBImpellerCycles" runat="server" ToolTip="Enter Impeller Cycles"
                                                                    CssClass="clsTextBoxMegaSmall_Ajax" Text='<%# DataBinder.Eval(Container.DataItem,"ImpellerCycles") %>'
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    AutoPostBack="true" OnTextChanged="txtCGBImpellerCycles_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalImpellerCycles" HeaderText="Final Impeller Cycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="CT Cycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtCGBCTCycles" runat="server" ToolTip="Enter CT Cycles" CssClass="clsTextBoxMegaSmall_Ajax"
                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"CTCycles") %>' AutoPostBack="true"
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    OnTextChanged="txtCGBCTCycles_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalCTCycles" HeaderText="Final CT Cycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="PT Cycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtCGBPTCycles" runat="server" ToolTip="Enter PT Cycles" CssClass="clsTextBoxMegaSmall_Ajax"
                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"PTCycles") %>' AutoPostBack="true"
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    OnTextChanged="txtCGBPTCycles_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalPTCycles" HeaderText="Final PT Cycles">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Generator Mods">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
                                                            <ItemTemplate>
                                                                <asp:TextBox Style="z-index: 0" ID="txtCGBGeneratorMods" runat="server" ToolTip="Enter the Generator Mods."
                                                                    CssClass="clsTextBoxMegaSmall_Ajax" Text='<%# DataBinder.Eval(Container.DataItem,"GeneratorMods") %>'
                                                                    onkeydown="onkeyPressed(window.event.keyCode,this);" onfocus="onTextFocus();"
                                                                    AutoPostBack="true" OnTextChanged="txtCGBGeneratorMods_TextChanged" Width="93%">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FinalGeneratorMods" HeaderText="Final Generator Mods">
                                                            <HeaderStyle HorizontalAlign="Right" Width="75px"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Right" Width="75px"></ItemStyle>
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
                                    <%--AJAX- Add UpdatePanel for Remark--%>
                                    <asp:UpdatePanel ID="upnlRemark" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblRemark" runat="server" CssClass="clsLabelAuto">Remark</asp:Label><br />
                                            <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxMultiLineLong_Ajax"
                                                MaxLength="500" Text="<%# mLog.Remark %>" TextMode="MultiLine" ToolTip="Enter Remark"
                                                Width="700px"></asp:TextBox>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <%--AJAX- Add UpdatePanel for Save, SaveNew, Print and Back Grid--%>
                    <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
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
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnBack" />
                        </Triggers>
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
                                    <%= Child3.DesPlaceName%></span>
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
    <div id="pnlAllAssemblypanel" class="clsInfoMessage1" style="display: none; z-index: 100;
         draggable="true">
          <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upnlAssemblyInfo">
            <ContentTemplate>
                <div  style="width: 90%">
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
                <div style="width: 90%;overflow:scroll">
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
         function CloseAssemblyDet() {

             $("#pnlAllAssemblypanel").hide();
         }
    </script>
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
    <script type="text/javascript">
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
                //   $("#pnlAllAssemblypanel").delay(9000).fadeOut();
            });
        }
    </script>
    <script type="text/javascript">
        function CloseLastDet() {

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
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("div[id*='Panel1']").each(function () {
                $(this).find(":text").attr('class', 'clsTextBoxDate_Ajax');
                $(this).find(":image").css({ 'vertical-align': 'top' });
            });
        });
    </script>
</body>
</html>
