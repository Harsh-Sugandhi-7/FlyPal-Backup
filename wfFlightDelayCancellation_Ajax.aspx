<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfFlightDelayCancellation_Ajax.aspx.vb"
    Inherits="Flypal.wfFlightDelayCancellation_Ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>Flight Delay/Cancellation Details</title>
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link    id="MainStyle" type="text/css" rel="stylesheet" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" src="tooltip.js"></script>
    <link rel="stylesheet" type="text/css" href="tooltip.css" />
    <link rel="stylesheet" type="text/css" href="popup.css" />
    <script type="text/javascript" src="AlertMessage1.1.js"></script>
    <script type="text/javascript">
        function openFile() {
            str = "wfFileView.aspx"
            window.open(str, "", 'toolbar=yes,status=yes,scrollbars=yes,titlebar=yes,resizable=yes');
        }
    </script>
</head>
<body bottommargin="5" leftmargin="0" rightmargin="0" topmargin="5" ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" runat="server"
        EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblMain" class="clstablelistout">
        <tr>
            <td>
                <asp:Panel ID="pnlMain" runat="server" CssClass="clsPanel1">
                    <table id="tblinner" class="clstablelistin">
                        <tr>
                            <td colspan="2" class="clsFormHeader1Newstyle">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblTitle" runat="server" CssClass="clsFormHeader">Flight Delay/Cancellation Details</asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>

                                        <td colspan="2" align="right">
                                            <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Save" ToolTip="Click to save current record"
                                                                    CausesValidation="True"></asp:Button>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" ToolTip="Back to Previous Page"
                                                                    CausesValidation="True"></asp:Button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>

                                    </tr>
                                </table>

                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlValidationSummary" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="Validationsummary2" runat="server" CssClass="clsValidationSummary"
                                            HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                        <asp:CustomValidator ID="cvCommon" runat="server" Display="None"></asp:CustomValidator>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlDelayDate" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table3" border="0">
                                            <tr>
                                                <td>
                                                    <span id="Label9" class="clsLabelStar">*</span>
                                                </td>
                                                <td>
                                                    <span id="lblDate" class="clsLabelAuto">Date</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDate" runat="server" ClientIDMode="Static" CssClass="clsTextBoxTagSearchDate"
                                                        AutoPostBack="true" onchange="ValidateDateText(this,'Date_watermarkextender','true');"
                                                        Text="" Width="100px"></asp:TextBox>
                                                    <cc2:CalendarExtender ID="Date_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                        Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtDate">
                                                    </cc2:CalendarExtender>
                                                    <cc2:TextBoxWatermarkExtender ID="Date_watermarkextender" runat="server" TargetControlID="txtDate"
                                                        WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                    </cc2:TextBoxWatermarkExtender>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkDelay" runat="server" CssClass="clsLabelAuto" AutoPostBack="True"
                                                        Text="Delay" Checked="<%# mFligthDelayAndCancellation.IsDelay %>"></asp:CheckBox>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkCancel" runat="server" CssClass="clsLabelAuto" AutoPostBack="True"
                                                        Text="Cancel" Checked="<%# mFligthDelayAndCancellation.IsCancel%>"></asp:CheckBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlFlightDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset style="top: 8px; left: 3px" class="clsFieldSetNewStyle">
                                            <legend><b>Flight Details</b> </legend>
                                            <table id="Table8" border="0" width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblLogStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <span id="lblLog" class="clsLabelAuto">Log</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbLogNo" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
                                                            SelectedValue="<%# mFligthDelayAndCancellation.LogID %>" DataValueField="LogID"
                                                            DataTextField="LogNo">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblRoute" class="clsLabelAuto">Route</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRoute" runat="server" CssClass="clsTextBoxTagSearch" Text="<%# mFligthDelayAndCancellation.Route %>"
                                                            ToolTip="Enter Route">
                                                        </asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblClassification" class="clsLabelAuto">Classification</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbFlightLogClassification" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                            DataValueField="ID" DataTextField="Name" SelectedValue="<%# mFligthDelayAndCancellation.FlightLogClassificationID %>">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblFlightNo" class="clsLabelAuto">Flight No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtFlightNo" runat="server" CssClass="clsTextBoxTagSearchSmall" Text="<%# mFligthDelayAndCancellation.FlightNo %>"
                                                            ToolTip="Enter Flight No.">
                                                        </asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblLogPageNo" class="clsLabelAuto">Log Page No.</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtLogPageNo" runat="server" CssClass="clsTextBoxTagSearchSmall" Text="<%# mFligthDelayAndCancellation.LogPageNo %>"
                                                            ToolTip="Enter Log Page No.">
                                                        </asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblPICStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <span id="lblPIC" class="clsLabelAuto">PIC</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbPIC" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                            DataTextField="EmpNoName" SelectedValue="<%# mFligthDelayAndCancellation.PICID %>">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblSTDStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <span id="lblSTD" class="clsLabelAuto">STD</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtSTDDate" CssClass="clsTextBoxTagSearchDate" 
                                                            AutoPostBack="true" onchange="ValidateDateText(this,'txtSTDDate_watermarkextender','false');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtSTDDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Format="<%$AppSettings:DateFormat%>" TargetControlID="txtSTDDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtSTDDate" ID="txtSTDDate_watermarkextender"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                            WatermarkCssClass="clsDateTextBox">
                                                        </cc2:TextBoxWatermarkExtender>
                                                        <asp:TextBox ID="txtSTDTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
                                                            MaxLength="10" ToolTip="Enter STD Time."></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblATDStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <span id="lblATD" class="clsLabelAuto">ATD</span>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkATD" runat="server" CssClass="clsLabelAuto" AutoPostBack="True"
                                                            ToolTip="Check to change Actual Time of Departure Date" Visible="False"></asp:CheckBox>
                                                        <asp:TextBox runat="server" ID="txtATDDate" CssClass="clsTextBoxTagSearchDate"
                                                            AutoPostBack="true" onchange="ValidateDateText(this,'txtATDDate_watermarkextender','false');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtATDDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Format="<%$AppSettings:DateFormat%>" TargetControlID="txtATDDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtATDDate" ID="txtATDDate_watermarkextender"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                            WatermarkCssClass="clsDateTextBox">
                                                        </cc2:TextBoxWatermarkExtender>
                                                        <asp:TextBox ID="txtATDTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
                                                            MaxLength="10" ToolTip="Enter Departure Time."></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblALE" class="clsLabelAuto">ALE</span>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbALE" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                            DataTextField="EmpNoName" SelectedValue="<%# mFligthDelayAndCancellation.ALEID %>">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlDelayDetails" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset class="clsFieldSetNewStyle">
                                            <legend><b>Delay Details</b> </legend>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <span id="lblAircraftReadyAt" class="clsLabelAuto">Aircraft Ready At</span>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkAircraftReadyAt" runat="server" CssClass="clsLabelAuto" AutoPostBack="True"
                                                            ToolTip="Check to change Aircraft Ready At Date" Visible="False"></asp:CheckBox>
                                                        <asp:TextBox runat="server" ID="txtAircraftReadyAtDate" CssClass="clsTextBoxTagSearchDate"
                                                            Width="100px" AutoPostBack="true" onchange="ValidateDateText(this,'txtAircraftReadyAtDate_watermarkextender','false');"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="txtAircraftReadyAtDate_CalendarExtender" runat="server"
                                                            CssClass="cal_Theme1" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtAircraftReadyAtDate">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender TargetControlID="txtAircraftReadyAtDate" ID="txtAircraftReadyAtDate_watermarkextender"
                                                            ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                            WatermarkCssClass="clsDateTextBox">
                                                        </cc2:TextBoxWatermarkExtender>
                                                        <asp:TextBox ID="txtAircraftReadyAtTime" runat="server" AutoPostBack="True" CssClass="clsTextBoxTagSearchSmall"
                                                            MaxLength="10" ToolTip="Enter STD Time."></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblTechDelayStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <span id="lblTechDelay" class="clsLabelAuto">Tech. Delay</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtTechDelay" runat="server" CssClass="clsTextBoxTagSearchSmall" Text="<%# mFligthDelayAndCancellation.TechDelay %>"
                                                            ToolTip="Ener Technical Delay" MaxLength="10"></asp:TextBox>&nbsp;(HH:MM)
                                                    </td>
                                                    <td>
                                                        <span id="lblOtherDelay" class="clsLabelAuto">Other Delay</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtOtherDelay" runat="server" CssClass="clsTextBoxTagSearchSmall" Text="<%# mFligthDelayAndCancellation.OtherDelay %>"
                                                            ToolTip="Enter Other Delay" MaxLength="10"></asp:TextBox>&nbsp;(HH:MM)
                                                    </td>
                                                    <td>
                                                        <span id="lblTotalDelay" class="clsLabelAuto">Total Delay</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtTotalDelay" runat="server" CssClass="clsTextBoxTagSearchSmall" Text="<%# mFligthDelayAndCancellation.TotalDelay %>"
                                                            ToolTip="Enter Total Delay" ReadOnly="True" BackColor="Gainsboro" MaxLength="10">
                                                        </asp:TextBox>&nbsp;(HH:MM)
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="ChkReliability" runat="server" CssClass="clsLabelAuto" Text="Consider in Reliability"
                                    Checked="<%# mFligthDelayAndCancellation.ConsiderInReliability %>"></asp:CheckBox>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upnlFileupload" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table id="Table5" border="0" cellspacing="1" cellpadding="1">
                                            <tr>
                                                <td>
                                                    <span id="lblAttachFile" class="clsLabelAuto">Attach File</span>
                                                </td>
                                                <td>
                                                    <input type="button" id="btnSelectFile" value="Select File" 
                                                        runat="server" class="clsbtnH clsinfoH1" causesvalidation="False" />
                                                </td>
                                                <td style="padding-left: 3px;">
                                                    <asp:Button ID="btnDelAttch" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Remove Attachment"
                                                        Text="Remove Attachment" Enabled="False" ></asp:Button>
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
                        <tr>
                            <td colspan="2">
                                <span id="lblDCCause" class="clslabelHeader">Delay/Cancellation Cause And Effect Details</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <span id="lblATAChapter" class="clsLabelAuto">ATA Chapter</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbATAChapter" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong"
                                                SelectedValue="<%# mFligthDelayAndCancellation.ATAID %>" DataValueField="ID"
                                                DataTextField="ATAChapter">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span id="lblCauseofOtherDC" class="clsLabelAuto">Cause of other Delay/Cancellation</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:TextBox ID="txtCauseofOtherDC" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                    Width="99%" Text="<%# mFligthDelayAndCancellation.CauseOfOtherDC %>" MaxLength="500"
                                    TextMode="MultiLine">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlCausenEffectMaster" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <table cellspacing="1" cellpadding="1">
                                                        <tr>
                                                            <td>
                                                                <span id="Label1" class="clsLabelStar">*</span>
                                                            </td>
                                                            <td>
                                                                <span id="lblCauseAndEffect" class="clsLabelAuto">Cause And Effect</span>
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="btnCauseAndEffect" runat="server" Height="22px" Width="24px"
                                                                    ToolTip="Click to add new Cause and Effect" ImageUrl="~/images/plus1.png" CausesValidation="false" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <span id="lblOthers" class="clsLabelAuto">Others(Cause And Effect)</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <asp:CheckBoxList ID="ChklistCauseAndEffect" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong"
                                                        ClientIDMode="Static" DataValueField="ID" DataTextField="ShortCode" RepeatColumns="3" Width="100%"
                                                        RepeatDirection="Horizontal">
                                                    </asp:CheckBoxList>
                                                </td>
                                                <td valign="top">
                                                    <asp:TextBox ID="txtOthers" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                        Width="98%" Text="<%# mFligthDelayAndCancellation.OtherCauseAndEffect %>" MaxLength="500"
                                                        TextMode="MultiLine">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span id="lblPrimaryCause" class="clsLabelAuto">Primary Cause</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:TextBox ID="txtPrimaryCause" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                    Width="99%" Text="<%# mFligthDelayAndCancellation.PrimaryCause %>" MaxLength="500"
                                    TextMode="MultiLine">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="upnlSecCauseMaster" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <table cellspacing="1" cellpadding="1">
                                                        <tr>
                                                            <td>
                                                                <span id="lblSecondaryCause" class="clsLabelAuto">Secondary Cause</span>
                                                            </td>
                                                            <td>
                                                                <asp:ImageButton ID="btnSecondaryCause" runat="server" Height="22px" Width="24px"
                                                                    ToolTip="Click to add new Secondary Cause" ImageUrl="~/images/plus1.png" CausesValidation="false" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBoxList ID="chkListSecondaryCause" runat="server" CssClass="clsTextBoxTagSearchComboNewstyleLong"
                                                        ClientIDMode="Static" DataValueField="ID" DataTextField="ShortCode" RepeatDirection="Horizontal"
                                                        RepeatColumns="3" Width="100%">
                                                    </asp:CheckBoxList>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <span id="lblInvestiagation" class="clsLabelAuto">Investigation</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:TextBox ID="txtInvestigation" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                    Width="99%" Text="<%# mFligthDelayAndCancellation.Investigation %>" MaxLength="500"
                                    TextMode="MultiLine">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span id="lblPreventiveMeasere" class="clsLabelAuto">Preventive Measure</span>
                            </td>
                            
                            <td>
                                <span id="lblRemarks" class="clsLabelAuto">Remarks</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtPreventiveMeasure" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                    Text="<%# mFligthDelayAndCancellation.PreventiveMeasure %>" Width="98%" ToolTip="Enter Preventive Measure."
                                    MaxLength="500" TextMode="MultiLine"></asp:TextBox>
                            </td>
                            
                            <td>
                                <asp:TextBox ID="txtRemark" runat="server" CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                    Width="98%" Text="<%# mFligthDelayAndCancellation.Remarks %>" ToolTip="Enter Remarks"
                                    MaxLength="500" TextMode="MultiLine">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <span id="lblInvestigatedBy" class="clsLabelAuto">Investigated By</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbInvestigatedBy" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                DataValueField="ID" DataTextField="EmpNoName" SelectedValue="<%# mFligthDelayAndCancellation.InvestigatedByID %>">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <span id="lblApprovedBy" class="clsLabelAuto">Approved By</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="cmbApprovedBy" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle"
                                                DataValueField="ID" DataTextField="EmpNoName" SelectedValue="<%# mFligthDelayAndCancellation.ApprovedByID %>">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <%--<tr>
                            <td colspan="2" align="right">
                                <asp:UpdatePanel ID="upnlActionBtn" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH" Text="Save" ToolTip="Click to save current record"
                                                        CausesValidation="True"></asp:Button>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Back" ToolTip="Back to Previous Page"
                                                        CausesValidation="True"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>--%>
                        <tr style="height: 0px;">
                            <td colspan="2" style="height: 0px;">
                                <asp:UpdatePanel runat="server" ID="upnlBtnFileUpload" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnCauseAndEffectMaster" ClientIDMode="Static" runat="server"
                                            Text="----" CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnSecCauseMaster" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
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
    <%--Date Validations--%>
    <script type="text/javascript">
        //Date validations
        function ValidateDateText(elem, extenderid, ToBeReset) {

            var datevalue = $(elem).val();
            var params = { 'Date': datevalue, 'SetDefault': ToBeReset };
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
    <!-- Cause And Effect Master Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyCauseAndEffectMaster" Text="Cause And Effect Master"
            ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlCauseAndEffectMaster" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeCauseAndEffectMaster" frameborder="0" height="100%" width="100%"
            src="JavaScript:''" allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupCauseAndEffectMaster" runat="server" TargetControlID="btnDummyCauseAndEffectMaster"
        PopupControlID="pnlCauseAndEffectMaster" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameCauseAndEffectMasterStateComplete() {
            $("#btnDummyCauseAndEffectMaster").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenCauseAndEffectMasterWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeCauseAndEffectMaster").attr("src", "wfDCCauseAndEffect_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyCauseAndEffectMaster").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForCauseAndEffectMaster() {
            var CauseAndEffectMasterwindow = $find("<%=mdlPopupCauseAndEffectMaster.ClientID %>");
            //close CauseAndEffectMaster window
            CauseAndEffectMasterwindow.hide();
            //           release resources
            $("#IframeCauseAndEffectMaster").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnCauseAndEffectMaster").click();
        }
    </script>
    <!-- End-->
    <!-- Sec Cause Master Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummySecCauseMaster" Text="Sec Cause Master" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlSecCauseMaster" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeSecCauseMaster" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupSecCauseMaster" runat="server" TargetControlID="btnDummySecCauseMaster"
        PopupControlID="pnlSecCauseMaster" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameSecCauseMasterStateComplete() {
            $("#btnDummySecCauseMaster").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenSecCauseMasterWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeSecCauseMaster").attr("src", "wfDCSecondaryCause_Ajax.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummySecCauseMaster").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForSecCauseMaster() {
            var SecCauseMasterwindow = $find("<%=mdlPopupSecCauseMaster.ClientID %>");
            //close SecCauseMaster popup window
            SecCauseMasterwindow.hide();
            //           release resources
            $("#IframeSecCauseMaster").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnSecCauseMaster").click();
        }
    </script>
    <!-- End-->
    <script type="text/javascript">
		 Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(){
            $("label[for*='ChklistCauseAndEffect']").mouseover(function () {
				 var CauseAndEffectListObject=new Object(); //Stores value in object["Key"]= "value" format...
				 var tempDescription='';
				<% For i As Integer = 0 To mDCCauseAndEffectList.Count - 1%>
						tempDescription='<%=mDCCauseAndEffectList(i).CauseAndEffect.Replace(Environment.NewLine,"¿") %>';	//REplace Line break with custom char....
						tempDescription=tempDescription.replace(new RegExp('¿','g'), '<br />');									//Replace all custom char(if exists) with new line char of javascript to show exactly same as entered
						CauseAndEffectListObject['<%=mDCCauseAndEffectList(i).ShortCode %>']= tempDescription;
				<%  Next %>
				               		
           $(this).attr('title',CauseAndEffectListObject[$(this).text()]); //Returns short code(text) of the current mouse hover item(View HTML for CheckBoxList)
            });
        });
    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("label[for*='ChklistCauseAndEffect']").tooltip({
                //borderColor:"#009DD9",
                borderColor: "DarkGrey",
                borderSize: 1,
                tooltipPadding: 5
                //tooltipBGColor:'WhiteSmoke'
            });
        });
    </script>
    <script type="text/javascript">
		 Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(){
            $("label[for*='chkListSecondaryCause']").mouseover(function () {
				var SecondaryCauseListObject=new Object();  //Stores value in object["Key"]= "value" format...
				var tempDescription='';
				<% For i As Integer = 0 To mDCSecondaryCauseList.Count - 1%>
					tempDescription='<%= mDCSecondaryCauseList(i).SecondaryCause.Replace(Environment.NewLine,"¿") %>';	//REplace Line break with custom char....
					tempDescription=tempDescription.replace(new RegExp('¿','g'), '<br />') ;									//Replace all custom char(if exists) with new line char of javascript to show exactly same as entered
					SecondaryCauseListObject['<%= mDCSecondaryCauseList(i).ShortCode%>']=tempDescription;
				<%  Next %>
				         
           $(this).attr('title',SecondaryCauseListObject[$(this).text()]); //Returns short code(text) of the current mouse hover item(View HTML for CheckBoxList)
				
			});
        });
    </script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("label[for*='chkListSecondaryCause']").tooltip({
                //borderColor:"#009DD9",
                borderColor: "DarkGrey",
                borderSize: 1,
                tooltipPadding: 5
                //tooltipBGColor:'WhiteSmoke'
            });
        });
    </script>
    </form>
</body>
</html>
