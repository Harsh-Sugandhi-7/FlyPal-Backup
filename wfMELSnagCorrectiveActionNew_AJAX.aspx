<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfMELSnagCorrectiveActionNew_AJAX.aspx.vb"
    EnableEventValidation="false" Inherits="Flypal.wfMELSnagCorrectiveActionNew_AJAX" %>

<%--UI & StyleSheets Changes by Harsh--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc1" TagName="MSGBox" Src="MSGBox.ascx" %>
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title>MEL Snag Corrective Action List</title>
    <link id="MainStyle" type="text/css" rel="stylesheet" />

    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <link rel="stylesheet" type="text/css" href="popup.css" />
    <script language="javascript" type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <script type="text/javascript" src="AlertMessage.js"></script>
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script language="javascript" type="text/javascript" id="clientEventHandlersJS">
        function openledgersame(FileName) {
            window.open(FileName, "_top", 'fullscreen=yes,toolbar=no,status=no,menubar=no,scrollbars=no,resizable=no,directories=no,location=no,width=auto,height=auto');
        }

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
    <style type="text/css">
        .clsCursorStyle {
            cursor: pointer;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager AsyncPostBackTimeout="600" ID="ScriptManager1" EnablePageMethods="true"
            runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc1:MSGBox ID="MSGBoxCtrl" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <table id="tblMain" class="clstablelistout" width="100%">
                <tr>
                    <td>
                        <asp:Panel ID="pnlMain" runat="server" CssClass="clspnl1">
                            <table id="tblinner" class="clsTablelistin">
                                <tr>
                                    <td class="clsFormHeader1Newstyle">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lblSnagCorrectiveActionInfo" runat="server"
                                                                CssClass="clsFormHeader">
																MEL / Snag Defect Corrective Action
                                                            </asp:Label>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td align="right">
                                                    <asp:UpdatePanel runat="server" ID="upnlHeaderButtons" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Button ID="btnSave" runat="server" CssClass="clsbtnH clsinfoH"
                                                                Text="Save" ValidationGroup="a"
                                                                ToolTip='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "Click to Save ADD / Defect Corrective Action",
																			"Click to Save MEL / Snag Corrective Action") %>'
                                                                CausesValidation="true"></asp:Button>
                                                            <asp:Button ID="btnSendMail" runat="server"
                                                                CssClass="clsbtnH clsinfoH" Text="Send Mail"
                                                                Visible="<%#Not mMELSnagCorrectiveAction.IsNew %>"
                                                                ToolTip='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "Click to send mail if ADD is added",
																			"Click to send mail if MEL is added") %>'></asp:Button>
                                                            <asp:Button ID="btnPrint" runat="server" CssClass="clsbtnH clsinfoH" Text="Print"
                                                                ToolTip='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "Click to Print ADD / Defect Corrective Action",
																			"Click to Print MEL / Snag Corrective Action") %>'
                                                                Enabled="<%# Not mMELSnagCorrectiveAction.IsNew %>"></asp:Button>
                                                            <asp:Button ID="btnBack" runat="server" CssClass="clsbtnH clsinfoH" Text="Close"
                                                                ToolTip='<%#IIf(AppSettings("MELSnagNomenclature") = "True",
																"Click to Close ADD / Defect Corrective Action screen",
																"Click to Close MEL / Snag Corrective Action screen") %>'
                                                                CausesValidation="False"></asp:Button>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:UpdatePanel ID="upnlErrorList" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ValidationSummary ID="vsDiscrepencyDetails" CssClass="clsValidationSummary" runat="server"
                                                    ValidationGroup="a" HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                                <asp:CustomValidator ID="cvDefectList" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtDefect"
                                                    ValidationGroup="a" ErrorMessage="Defect is Required." Display="None" OnServerValidate="CustomValidate">
                                                </asp:CustomValidator>
                                                <asp:RequiredFieldValidator ID="rfvDefect" runat="server" CssClass="clsLabelAuto"
                                                    ValidationGroup="a" ControlToValidate="txtDefect" ErrorMessage="Defect Required."
                                                    Display="None">
                                                </asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cvLogNo" runat="server" CssClass="clsLabelAuto" ControlToValidate="cmbLogNo"
                                                    ValidationGroup="a" ErrorMessage="Please select the Log." Display="None" OnServerValidate="CustomValidate">
                                                </asp:CustomValidator>
                                                <asp:RequiredFieldValidator ID="rfvText" runat="server" ControlToValidate="txtDefectReportNo"
                                                    CssClass="clsLabelAuto" Display="None" ErrorMessage="Defect Text is Required." ValidationGroup="a">
                                                </asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvNo" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtNo"
                                                    ValidationGroup="a" ErrorMessage="Defect No is Required." Display="None">
                                                </asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cvOccDate" runat="server" CssClass="clsLabelAuto" ControlToValidate="txtDateofoccurrence"
                                                    ValidationGroup="a" Display="None" OnServerValidate="CustomValidate" ErrorMessage="Date of Occurrence is Required..">
                                                </asp:CustomValidator>
                                                <asp:RequiredFieldValidator ID="rfDateofoccurrence" runat="server" CssClass="clsLabelAuto"
                                                    ControlToValidate="txtDateofoccurrence" ErrorMessage="Date of Occurrence is Required.." Display="None"
                                                    ValidationGroup="a">
                                                </asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cvDiscrepancyDetails" runat="server" CssClass="clsLabelAuto" ValidationGroup="a"
                                                    ControlToValidate="txtDefectReportNo" Display="None" ErrorMessage="">
                                                </asp:CustomValidator>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upnlMELSnagDetails" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table border="0" width="100%">
                                                    <tr>
                                                        <td colspan="2">
                                                            <fieldset class="clsFieldSetNewStyle" style="width: 97.5%;">
                                                                <legend id="lgnDiscrepencyDetails" runat="server" style="font-weight: bold">MEL / Snag Details</legend>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblDefectReportNoStar" runat="server"
                                                                                CssClass="clsLabelStar">
																				*
                                                                            </asp:Label>
                                                                        </td>
                                                                        <td colspan="4">
                                                                            <asp:Label ID="lblDefectReportNo" runat="server"
                                                                                CssClass="clsLabelAuto">
																				Defect No.
                                                                            </asp:Label>
                                                                        </td>
                                                                        <td colspan="3">
                                                                            <asp:TextBox ID="txtDefectReportNo" runat="server"
                                                                                CssClass="clsTextBoxTagSearch" Width="100px"
                                                                                Text="<%# mMELSnagCorrectiveAction.DefectReportNo %>"
                                                                                MaxLength="25" ToolTip="Enter Defect Text"
                                                                                Enabled="<%# mMELSnagCorrectiveAction.IsNew %>">
                                                                            </asp:TextBox>
                                                                            <asp:TextBox ID="txtNo" runat="server"
                                                                                CssClass="clsTextBoxTagSearchSmall" Width="30px"
                                                                                Text="<%# mMELSnagCorrectiveAction.No %>"
                                                                                MaxLength="4" ToolTip="Enter Defect No."
                                                                                Enabled="<%# mMELSnagCorrectiveAction.IsNew %>">
                                                                            </asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblDatePlaceofoccuranceStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblDatePlaceofoccurance" runat="server" CssClass="clsLabel">Date of Occurrence</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:UpdatePanel ID="upnlOccurranceDate" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:TextBox ID="txtDateofOccurrence" runat="server" AutoPostBack="True" autocomplete="off"
                                                                                        CssClass="clsTextBoxTagSearchDate" CausesValidation="true" Width="100px"
                                                                                        onchange="ValidateDateText(this,'wmeDateOfOccurence');"
                                                                                        Enabled="<%# mMELSnagCorrectiveAction.IsNew %>">
                                                                                    </asp:TextBox>
                                                                                    <cc2:CalendarExtender ID="txtDateofOccurrence_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                        Enabled="True" Format="<%$AppSettings:DateFormat%>"
                                                                                        TargetControlID="txtDateofOccurrence"></cc2:CalendarExtender>
                                                                                    <cc2:TextBoxWatermarkExtender ID="wmeDateOfOccurence" runat="server" TargetControlID="txtDateofOccurrence"
                                                                                        WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                        <td style="width: 0px !important">
                                                                            <asp:Label ID="lblLogNoStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblLogNo" runat="server" CssClass="clsLabel">Log No.</asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:UpdatePanel ID="upnlLogNo" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:DropDownList ID="cmbLogNo" runat="server"
                                                                                        CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="LogID"
                                                                                        DataTextField="LogNoLogPageNo" AutoPostBack="True"
                                                                                        Enabled="<%# mMELSnagCorrectiveAction.IsNew %>">
                                                                                    </asp:DropDownList>
                                                                                    <asp:LinkButton ID="lnkCheckStatus" runat="server"
                                                                                        CssClass="clsLinkButton" ToolTip="Click to View Log Info."
                                                                                        CausesValidation="False" Enabled="False">
																						View
                                                                                    </asp:LinkButton>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="14">
                                                                            <asp:UpdatePanel ID="upnlSnagType" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <table id="tblSnagType" border="0" width="100%">
                                                                                        <tr>
																							<%--Sankalp 28-10-25--%>
																							<asp:PlaceHolder ID="MELSnagType" runat="server" Visible='<%# IIf(AppSettings("ClientCode") = "CVA", False, True ) %>'>
                                                                                            <td style="width: 33%;">
                                                                                                <fieldset style="font-size: 9pt" class="clsFieldSetNewStyle">
                                                                                                    <legend>
                                                                                                        <b>
                                                                                                            <asp:Label
                                                                                                                Text='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "ADD / Defect Type", "MEL / Snag Type") %>'
                                                                                                                runat="server" ID="lblMELLabType"> 
                                                                                                            </asp:Label>
                                                                                                        </b>
                                                                                                    </legend>
                                                                                                    <table id="tblSnagTypeRadio">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:RadioButton ID="rbMajor" runat="server"
                                                                                                                    CssClass="clsRadioButton" Text="Major" Width="60px"
                                                                                                                    Checked="<%# mMELSnagCorrectiveAction.IsMajor %>" GroupName="a"></asp:RadioButton>
                                                                                                            </td>
                                                                                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:RadioButton ID="rbMinor" runat="server"
                                                                                                                    CssClass="clsRadioButton" Text="Minor" Width="60px"
                                                                                                                    Checked="<%# mMELSnagCorrectiveAction.IsMinor %>" GroupName="a"></asp:RadioButton>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </fieldset>
                                                                                            </td>
																							</asp:PlaceHolder>
                                                                                            <td style="width: 33%;">
                                                                                                <fieldset style="font-size: 9pt" class="clsFieldSetNewStyle">
                                                                                                    <legend>
                                                                                                        <b>Defect Type
                                                                                                        </b>
                                                                                                    </legend>
                                                                                                    <table id="tblDefectTypeRadio">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:RadioButton ID="rbPireps" runat="server"
                                                                                                                    CssClass="clsRadioButton" Text="Pireps" Width="60px"
                                                                                                                    Checked="<%# mMELSnagCorrectiveAction.IsPireps %>" GroupName="b"></asp:RadioButton>
                                                                                                            </td>
                                                                                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:RadioButton ID="rbMaintenanceDefect" runat="server"
                                                                                                                    CssClass="clsRadioButton" Width="136px"
                                                                                                                    Text="Maintenance Defect"
                                                                                                                    Checked="<%# mMELSnagCorrectiveAction.IsMaintenanceDefect %>"
                                                                                                                    GroupName="b"></asp:RadioButton>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </fieldset>
                                                                                            </td>
                                                                                            <td style="width: 33%;">
                                                                                                <fieldset style="font-size: 9pt" class="clsFieldSetNewStyle">
                                                                                                    <legend>
                                                                                                        <b>Reliability
                                                                                                        </b>
                                                                                                    </legend>
                                                                                                    <table id="tblReliabilityChkbx">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:CheckBox ID="chkIsInReliability" runat="server"
                                                                                                                    CssClass="clsCheckBox" Width="152px"
                                                                                                                    Text="Consider In Reliability"
                                                                                                                    Checked="<%# mMELSnagCorrectiveAction.IsInReliability %>"></asp:CheckBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </fieldset>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td></td>
                                                                        <td colspan="4">
                                                                            <asp:Label ID="lblSector" runat="server"
                                                                                CssClass="clsLabel" Width="165px">
																				Sector / Place
                                                                            </asp:Label>
                                                                        </td>
                                                                        <td colspan="3">
                                                                            <asp:UpdatePanel ID="upnlSector" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:TextBox ID="txtSector" runat="server"
                                                                                        CssClass="clsTextBoxTagSearch" Width="278px"
                                                                                        Text="<%# mMELSnagCorrectiveAction.Sector %>" MaxLength="50"
                                                                                        ToolTip="Enter Sector / Place"></asp:TextBox>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                        <td></td>
                                                                        <td>
                                                                            <asp:Label ID="lblSnagReportedBy" runat="server"
                                                                                CssClass="clsLabelAuto"
                                                                                Text='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "Defect reported by", "Snag reported by") %>'>
                                                                            </asp:Label>
                                                                        </td>
                                                                        <td colspan="2">
                                                                            <asp:TextBox ID="txtSnagReportedBy" runat="server"
                                                                                CssClass="clsTextBoxTagSearch" Width="215px"
                                                                                Text="<%# mMELSnagCorrectiveAction.SnagReportedBy %>"
                                                                                MaxLength="50" ToolTip='<%# IIf(AppSettings("MELSnagNomenclature") = "True", "Enter name of Defect reporter", "Enter name of Snag reporter") %>'>
                                                                            </asp:TextBox>
                                                                        </td>
                                                                        <td colspan="2">
                                                                            <asp:Label ID="lblSnagReportedByEg" runat="server" CssClass="clsLabelAuto">
																				(e.g.Pilot / AME / Passenger)
                                                                            </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td></td>
                                                                        <td colspan="4">
                                                                            <asp:Label ID="lblAircraftHrsLandingsTSNsincelastmajorcheck"
                                                                                runat="server" CssClass="clsLabelAuto" Width="165px">
																				Aircraft Hrs. / Landings (TSN since last major check)
                                                                            </asp:Label>
                                                                        </td>
                                                                        <td colspan="3">
                                                                            <asp:UpdatePanel ID="upnlLastMajorCheck" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:TextBox ID="txtLastMajorCheck" runat="server"
                                                                                        CssClass="clsTextBoxTagSearchSmall" Width="100px"
                                                                                        Text="<%# mMELSnagCorrectiveAction.LastMajorCheckHour %>"
                                                                                        MaxLength="50" ToolTip="Enter Aircraft Hrs. / Landings">
                                                                                    </asp:TextBox>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                        <td></td>
                                                                        <td>
                                                                            <asp:Label ID="lblReportedBy" runat="server" Width="165px" CssClass="clsLabelAuto">
																				Name of Pilot / AME & License No. / Observed By
                                                                            </asp:Label>
                                                                        </td>
                                                                        <td colspan="5">
                                                                            <asp:TextBox ID="txtReportedBy"
                                                                                runat="server"
                                                                                CssClass="clsTextBoxTagSearch"
                                                                                MaxLength="150" Width="215px"
                                                                                Text="<%# mMELSnagCorrectiveAction.ReportedBy %>"
                                                                                ToolTip="Enter Name of Pilot">
                                                                            </asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblDefectStar" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                                        </td>
                                                                        <td colspan="4">
                                                                            <asp:Label ID="lblDefect" runat="server"
                                                                                CssClass="clsLabel" Width="157px">
																				Defect
                                                                            </asp:Label>
                                                                        </td>
                                                                        <td colspan="3">
                                                                            <asp:TextBox ID="txtDefect" runat="server" Width="278px"
                                                                                CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                                                Text="<%# mMELSnagCorrectiveAction.Defect %>"
                                                                                MaxLength="1000" ToolTip="Enter Defect Description"
                                                                                TextMode="MultiLine">
                                                                            </asp:TextBox>
                                                                        </td>
                                                                        <td></td>
                                                                        <td>
                                                                            <asp:Label ID="lblDefectOn" runat="server"
                                                                                CssClass="clsLabel" Width="200px">
																				Defect On
                                                                            </asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="cmbAssembly" runat="server" Width="278px"
                                                                                CssClass="clsTextBoxTagSearchCombo" DataTextField="ModelSerialNoPostion"
                                                                                SelectedValue="<%# mMELSnagCorrectiveAction.AssemblyStatusID %>"
                                                                                DataValueField="AssemblyStatusID" Style="width: 220px">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td colspan="3" align="left" style="padding-left: 10px;">
                                                                            <asp:UpdatePanel ID="upnlCreateWO" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:LinkButton ID="lnkbtnCreateWorkOrder" CssClass="clsLinkButton" runat="server"
                                                                                        Visible="<%# Not mMELSnagCorrectiveAction.InvestigationStatus And Not mMELSnagCorrectiveAction.IsNew %>"
                                                                                        Text="Create Work Order">
                                                                                    </asp:LinkButton>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <fieldset class="clsFieldSetNewStyle" style="width: 98.8%;">
                                                                <legend>
                                                                    <b>
                                                                        <span class="clsLabelHeader">Minimum Equipment Detail
                                                                        </span>
                                                                    </b>
                                                                </legend>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBox ID="chkShowMEL" runat="server" CssClass="clsCheckBox" Text='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "ADD (check if ADD)", "MEL (check if MEL)") %>'
                                                                                Checked="<%# mMELSnagCorrectiveAction.IsMEL %>" AutoPostBack="True"></asp:CheckBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:UpdatePanel ID="upnlTabs" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <cc2:TabContainer ID="tabMELLogDetailsContainer" runat="server" class="clstablelistin" AutoPostBack="False">
                                                                        <cc2:TabPanel ID="pnlVerificationDetails" runat="server" CssClass="clsPanel1" ClientIDMode="Static">
                                                                            <HeaderTemplate>
                                                                                <asp:Label runat="server" Text="Verification Details" ID="lblVerficationDeatils"></asp:Label>
                                                                            </HeaderTemplate>
                                                                            <ContentTemplate>
                                                                                <asp:UpdatePanel ID="upnlMMELDetails" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <fieldset class="clsFieldSetNewStyle">
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td colspan="4">
                                                                                                        <asp:UpdatePanel ID="upnlVerificationDetailsErrors" runat="server" UpdateMode="Conditional">
                                                                                                            <ContentTemplate>
                                                                                                                <asp:ValidationSummary ID="vsVerificationDetails" runat="server"
                                                                                                                    HeaderText="Fill Up The Following Fields" CssClass="clsValidationSummary"
                                                                                                                    ValidationGroup="VerificationDetails" />
                                                                                                                <asp:CustomValidator ID="cvComponent" runat="server"
                                                                                                                    ControlToValidate="cmbMELCategory" Display="None"
                                                                                                                    CssClass="clsLabelAuto" ErrorMessage=""
                                                                                                                    OnServerValidate="CustomValidate" ValidationGroup="VerificationDetails">
                                                                                                                </asp:CustomValidator>
                                                                                                                <asp:CustomValidator ID="cvFrequencyInHours" runat="server"
                                                                                                                    CssClass="clsLabelAuto" Display="None"
                                                                                                                    ValidationGroup="VerificationDetails" ControlToValidate="txtFrequencyInHours"
                                                                                                                    ErrorMessage="Please select MEL Category."
                                                                                                                    OnServerValidate="CustomValidate">
                                                                                                                </asp:CustomValidator>
                                                                                                                <asp:CustomValidator ID="cvFrequencyInDay" runat="server"
                                                                                                                    CssClass="clsLabelAuto" ValidationGroup="VerificationDetails"
                                                                                                                    ControlToValidate="txtFrequencyInDay" Display="None"
                                                                                                                    ErrorMessage="Please select the Due Date."
                                                                                                                    OnServerValidate="CustomValidate">
                                                                                                                </asp:CustomValidator>
                                                                                                                <asp:CustomValidator ID="cvDueDate" runat="server" Display="None"
                                                                                                                    CssClass="clsLabelAuto" ControlToValidate="txtDueDate"
                                                                                                                    ValidationGroup="VerificationDetails" OnServerValidate="CustomValidate">
                                                                                                                </asp:CustomValidator>
                                                                                                                <asp:CustomValidator ID="cvEx" runat="server" Display="None"
                                                                                                                    ControlToValidate="txtExtensionInDays" ValidationGroup="VerificationDetails"
                                                                                                                    OnServerValidate="CustomValidate" CssClass="clslabelauto">
                                                                                                                </asp:CustomValidator>
                                                                                                                <asp:CustomValidator ID="cvATA" runat="server" CssClass="clslabelauto"
                                                                                                                    Display="None" ValidationGroup="VerificationDetails"
                                                                                                                    OnServerValidate="CustomValidate" ControlToValidate="cmbATAChapter">
                                                                                                                </asp:CustomValidator>
                                                                                                                <asp:CustomValidator ID="cvVerificationDetails" runat="server"
                                                                                                                    CssClass="clsLabelAuto" ValidationGroup="VerificationDetails"
                                                                                                                    ControlToValidate="cmbATAChapter" Display="None" ErrorMessage="">
                                                                                                                </asp:CustomValidator>
                                                                                                            </ContentTemplate>
                                                                                                        </asp:UpdatePanel>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td colspan="4">
                                                                                                        <asp:Label ID="lblNote" runat="server" CssClass="clsLabelAuto">
																									Please select part, if not present then type in Part No. field.
                                                                                                        </asp:Label>
                                                                                                    </td>
                                                                                                    <td colspan="4"></td>
                                                                                                    <td colspan="4" align="right">
                                                                                                        <asp:LinkButton ID="lnkMELDetail" runat="server" CssClass="clsHyperlink1"
                                                                                                            Text='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "View ADD Detail", "View MEL Detail") %>'
                                                                                                            ToolTip='<%# iif(AppSettings("MELSnagNomenclature") = "True", "Click to view ADD details", "Click to view MEL details") %>'
                                                                                                            Visible="<%# not mMELSnagCorrectiveAction.IsNew And Not mMELSnagCorrectiveAction.MELID.Equals(Guid.Empty)%>">
                                                                                                        </asp:LinkButton>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="width: 9px;"></td>
                                                                                                    <td style="width: 127px;">
                                                                                                        <asp:Label ID="lblComponent" runat="server" CssClass="clsLabelAuto">Component</asp:Label>
                                                                                                    </td>
                                                                                                    <td style="width: 360px;">
                                                                                                        <asp:UpdatePanel ID="upnlShowMEL" runat="server" UpdateMode="Conditional">
                                                                                                            <ContentTemplate>
                                                                                                                <asp:DropDownList ID="cmbPartNo" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
                                                                                                                    SelectedValue="<%# mMELSnagCorrectiveAction.PartID %>" DataValueField="CompID"
                                                                                                                    DataTextField="PartNoSerialNo">
                                                                                                                </asp:DropDownList>
                                                                                                            </ContentTemplate>
                                                                                                        </asp:UpdatePanel>
                                                                                                    </td>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lblPartNo" runat="server" CssClass="clsLabelAuto">Part No.</asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:UpdatePanel ID="upnlPartNo" runat="server" UpdateMode="Conditional">
                                                                                                            <ContentTemplate>
                                                                                                                <asp:TextBox ID="txtPartNo" runat="server" Width="100px"
                                                                                                                    CssClass="clsTextBoxTagSearchSmall"
                                                                                                                    Text="<%# mMELSnagCorrectiveAction.PartNo %>"
                                                                                                                    ToolTip="Part No." MaxLength="50"></asp:TextBox>
                                                                                                            </ContentTemplate>
                                                                                                        </asp:UpdatePanel>
                                                                                                    </td>
                                                                                                    <td></td>
                                                                                                    <td colspan="2">
                                                                                                        <asp:Label ID="lblSerial" runat="server" CssClass="clsLabelAuto">Serial No</asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:UpdatePanel ID="upnlSerialNo" runat="server" UpdateMode="Conditional">
                                                                                                            <ContentTemplate>
                                                                                                                <asp:TextBox ID="txtSerialNo" runat="server" Width="100px"
                                                                                                                    CssClass="clsTextBoxTagSearchSmall"
                                                                                                                    Text="<%# mMELSnagCorrectiveAction.PartSerialNo %>"
                                                                                                                    ToolTip="Serial No." MaxLength="50"></asp:TextBox>
                                                                                                            </ContentTemplate>
                                                                                                        </asp:UpdatePanel>
                                                                                                    </td>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lblDescription" runat="server" CssClass="clsLabelAuto">Description</asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:UpdatePanel ID="upnlDesc" runat="server" UpdateMode="Conditional">
                                                                                                            <ContentTemplate>
                                                                                                                <asp:TextBox ID="txtDescription" runat="server"
                                                                                                                    CssClass="clsTextBoxTagSearch"
                                                                                                                    Text="<%# mMELSnagCorrectiveAction.Description %>"
                                                                                                                    ToolTip="Description" MaxLength="50">
                                                                                                                </asp:TextBox>
                                                                                                            </ContentTemplate>
                                                                                                        </asp:UpdatePanel>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <asp:Label Width="100px" ID="lblATAChapter" runat="server" CssClass="clsLabelAuto">ATA Chapter</asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:UpdatePanel ID="upnlATA" runat="server" UpdateMode="Conditional">
                                                                                                            <ContentTemplate>
                                                                                                                <asp:DropDownList ID="cmbATAChapter" runat="server" CssClass="clsTextBoxTagSearchComboNewstyle" AutoPostBack="True"
                                                                                                                    SelectedValue="<%# mMELSnagCorrectiveAction.ATAChapterID %>" DataValueField="ID"
                                                                                                                    DataTextField="ATAChapter">
                                                                                                                </asp:DropDownList>
                                                                                                            </ContentTemplate>
                                                                                                        </asp:UpdatePanel>
                                                                                                    </td>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lblSubATAChapter" runat="server" CssClass="clsLabelAuto" Width="100px">Sub-ATA Chapter</asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:UpdatePanel ID="upnlSubATA" runat="server" UpdateMode="Conditional">
                                                                                                            <ContentTemplate>
                                                                                                                <asp:DropDownList ID="cmbSubATAList" runat="server"
                                                                                                                    CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="ID"
                                                                                                                    DataTextField="SubATAChapter">
                                                                                                                </asp:DropDownList>
                                                                                                            </ContentTemplate>
                                                                                                        </asp:UpdatePanel>
                                                                                                    </td>
                                                                                                    <td></td>
                                                                                                    <td colspan="2">
                                                                                                        <asp:Label ID="lblMELCategory" Width="100px" runat="server"
                                                                                                            CssClass="clsLabelAuto"
                                                                                                            Text='<%#IIf(AppSettings("MELSnagNomenclature") = "True", "ADD Category", "MEL Category") %>'>
                                                                                                        </asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:UpdatePanel ID="upnlMELCategory" runat="server" UpdateMode="Conditional">
                                                                                                            <ContentTemplate>
                                                                                                                <asp:DropDownList ID="cmbMELCategory" runat="server"
                                                                                                                    CssClass="clsTextBoxTagSearchComboSmall" Width="106px"
                                                                                                                    DataValueField="ID" DataTextField="Name" AutoPostBack="True"
                                                                                                                    SelectedValue="<%# mMELSnagCorrectiveAction.MELCategoryID %>">
                                                                                                                </asp:DropDownList>
                                                                                                            </ContentTemplate>
                                                                                                        </asp:UpdatePanel>
                                                                                                    </td>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lblHrsofComp" Width="100px" runat="server"
                                                                                                            CssClass="clsLabelAuto">
																									Hrs. of Comp
                                                                                                        </asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="txtHrsofComp" runat="server"
                                                                                                            CssClass="clsTextBoxTagSearchSmall"
                                                                                                            Width="100px" Text="<%# mMELSnagCorrectiveAction.ComponentHour %>"
                                                                                                            ToolTip="Enter Component Hours"
                                                                                                            MaxLength="50">
                                                                                                        </asp:TextBox>
                                                                                                    </td>
                                                                                                    <td></td>

                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <asp:Label ID="lblFrequency" runat="server"
                                                                                                            CssClass="clsLabelAuto">
																									Frequency
                                                                                                        </asp:Label>
                                                                                                    </td>
                                                                                                    <td colspan="4">
                                                                                                        <asp:UpdatePanel ID="upnlFreq" runat="server" UpdateMode="Conditional">
                                                                                                            <ContentTemplate>
                                                                                                                <asp:TextBox ID="txtFrequencyInDay" runat="server" CssClass="clsTextBoxTagSearchRightAlign1"
                                                                                                                    Text="<%# mMELSnagCorrectiveAction.FrequencyInDays %>" ToolTip="Enter Frequency In Days"
                                                                                                                    AutoPostBack="True" MaxLength="4" Enabled="False"></asp:TextBox>
                                                                                                                <asp:Label ID="lblDays" runat="server" CssClass="clsLabelAuto">In Days</asp:Label>
                                                                                                                <asp:TextBox ID="txtFrequencyInHours" runat="server" CssClass="clsTextBoxTagSearchRightAlign1"
                                                                                                                    Text="<%# mMELSnagCorrectiveAction.FrequencyInHours %>" ToolTip="Enter Frequency In Hours"
                                                                                                                    AutoPostBack="True" MaxLength="5" Enabled="False"></asp:TextBox>
                                                                                                                <asp:Label ID="lblHours" runat="server" CssClass="clsLabelAuto">Hours</asp:Label>
                                                                                                                <asp:CheckBox ID="chkIsInHours" runat="server" CssClass="clsCheckBox" Text="(Select if Freq. in Hours)"
                                                                                                                    Checked="<%# mMELSnagCorrectiveAction.IsHours %>" AutoPostBack="True" Enabled="False"></asp:CheckBox>
                                                                                                            </ContentTemplate>
                                                                                                        </asp:UpdatePanel>
                                                                                                    </td>
                                                                                                    <td></td>
                                                                                                    <td colspan="2">
                                                                                                        <asp:Label ID="lblDueDate" runat="server"
                                                                                                            CssClass="clsLabelAuto">
																									Due Date
                                                                                                        </asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:UpdatePanel ID="upnlDueDate" runat="server" UpdateMode="Conditional">
                                                                                                            <ContentTemplate>
                                                                                                                <asp:TextBox ID="txtDueDate" runat="server" CssClass="clsTextBoxTagSearchDate" AutoPostBack="true"
                                                                                                                    onchange="ValidateDateText(this,'wmeDueDate');" Width="130px"></asp:TextBox>
                                                                                                                <cc2:CalendarExtender ID="CalExt_txtDueDate" runat="server" Enabled="True" TargetControlID="txtDueDate"
                                                                                                                    CssClass="cal_Theme1" Format="<%$AppSettings:DateFormat%>"></cc2:CalendarExtender>
                                                                                                                <cc2:TextBoxWatermarkExtender TargetControlID="txtDueDate" ID="wmeDueDate"
                                                                                                                    ClientIDMode="Static" runat="server" WatermarkText="<%$AppSettings:DateFormat%>"
                                                                                                                    WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                                                            </ContentTemplate>
                                                                                                        </asp:UpdatePanel>
                                                                                                    </td>
                                                                                                    <td></td>
                                                                                                    <td colspan="1">
                                                                                                        <asp:Label ID="lblIncidentType" runat="server"
                                                                                                            CssClass="clsLabelAuto">
																									Incident Type
                                                                                                        </asp:Label>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:DropDownList ID="cmbIncidentType" runat="server"
                                                                                                            CssClass="clsTextBoxTagSearchComboNewstyle" Width="130px"
                                                                                                            DataValueField="ID" DataTextField="Name"
                                                                                                            SelectedValue="<%# mMELSnagCorrectiveAction.IncidentTypeID %>">
                                                                                                        </asp:DropDownList>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <span id="lblExtensionApplied" class="clsLabelAuto">Extension</span>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:UpdatePanel ID="upnlExtension" runat="server" UpdateMode="Conditional">
                                                                                                            <ContentTemplate>
                                                                                                                <asp:CheckBox ID="chkExtensionApplied" runat="server"
                                                                                                                    CssClass="clsCheckBox"
                                                                                                                    Checked="<%# mMELSnagCorrectiveAction.ExtensionApplied %>"
                                                                                                                    AutoPostBack="True" Enabled="False"></asp:CheckBox>
                                                                                                                <asp:TextBox ID="txtExtensionInDays" runat="server"
                                                                                                                    CssClass="clsTextBoxTagSearchRightAlign1"
                                                                                                                    Text="<%# mMELSnagCorrectiveAction.ExtensionInDays %>"
                                                                                                                    ToolTip="Enter Frequency In Days"
                                                                                                                    AutoPostBack="True" MaxLength="4" Enabled="False">
                                                                                                                </asp:TextBox>
                                                                                                                <span id="lblInDays" class="clsLabelAuto">In Days</span>
                                                                                                            </ContentTemplate>
                                                                                                        </asp:UpdatePanel>
                                                                                                    </td>
                                                                                                    <td></td>
                                                                                                    <td>
                                                                                                        <span id="lblExtensionApprovalNo" runat="server"
                                                                                                            cssclass="clsLabelAuto">Approval Details
                                                                                                        </span>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="txtExtensionApprovalNo" runat="server"
                                                                                                            CssClass="clsTextBoxTagSearchSmall" Width="100px"
                                                                                                            Text="<%# mMELSnagCorrectiveAction.ExtensionApprovalNo %>"
                                                                                                            ToolTip="Enter Extension Approval No"
                                                                                                            MaxLength="100" Enabled="False">
                                                                                                        </asp:TextBox>
                                                                                                    </td>
                                                                                                    <td>&nbsp;
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </fieldset>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </ContentTemplate>
                                                                        </cc2:TabPanel>
                                                                        <cc2:TabPanel ID="tabRectificationDetails" runat="server" ClientIDMode="Static" CssClass="clsPanel1">
                                                                            <HeaderTemplate>
                                                                                <asp:Label runat="server" Text="Rectification Details" ID="lblRectificationDetails"></asp:Label>
                                                                            </HeaderTemplate>
                                                                            <ContentTemplate>
                                                                                <fieldset class="clsFieldSetNewStyle">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td colspan="4">
                                                                                                <asp:UpdatePanel runat="server" ID="upnlRectificationDetailsErrors" UpdateMode="Conditional">
                                                                                                    <ContentTemplate>
                                                                                                        <asp:ValidationSummary ID="RectificationDetailsErrors"
                                                                                                            CssClass="clsValidationSummary" runat="server" ValidationGroup="RectificationDetails"
                                                                                                            HeaderText="Fill Up The Following Fields"></asp:ValidationSummary>
                                                                                                        <asp:CustomValidator ID="cvActionList" runat="server"
                                                                                                            CssClass="clsLabelAuto" ControlToValidate="txtAction"
                                                                                                            ValidationGroup="RectificationDetails" ErrorMessage="Action  required."
                                                                                                            Display="None" OnServerValidate="CustomValidate">
                                                                                                        </asp:CustomValidator>
                                                                                                        <asp:CustomValidator ID="cvRectifiedLogNo" runat="server"
                                                                                                            CssClass="clsLabelAuto" ValidationGroup="RectificationDetails"
                                                                                                            ControlToValidate="cmbRectifiedLogNo" ErrorMessage="Log required"
                                                                                                            Display="None" OnServerValidate="CustomValidate">
                                                                                                        </asp:CustomValidator>
                                                                                                        <asp:CustomValidator ID="cvRectifiedDate" runat="server"
                                                                                                            CssClass="clsLabelAuto" ValidationGroup="RectificationDetails"
                                                                                                            ControlToValidate="txtRectifiedDate" Display="None"
                                                                                                            OnServerValidate="CustomValidate">
                                                                                                        </asp:CustomValidator>
                                                                                                        <asp:CustomValidator ID="cvRectificationDetails" runat="server"
                                                                                                            CssClass="clsLabelAuto" ValidationGroup="RectificationDetails"
                                                                                                            ControlToValidate="txtRectifiedDate" Display="None" ErrorMessage="">
                                                                                                        </asp:CustomValidator>
                                                                                                    </ContentTemplate>
                                                                                                </asp:UpdatePanel>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:UpdatePanel ID="upnlClose" runat="server" UpdateMode="Conditional">
                                                                                                    <ContentTemplate>
                                                                                                        <asp:CheckBox ID="chkClose" runat="server" CssClass="clsCheckBox"
                                                                                                            Checked="<%# mMELSnagCorrectiveAction.InvestigationStatus %>"
                                                                                                            AutoPostBack="True"></asp:CheckBox>
                                                                                                    </ContentTemplate>
                                                                                                </asp:UpdatePanel>
                                                                                            </td>
                                                                                            <td colspan="2">
                                                                                                <asp:Label ID="lblStatus" runat="server" CssClass="clsLabelAuto">Investigation Status (Closed)</asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <asp:PlaceHolder ID="phRectificationDetails" runat="server">
                                                                                        <table>
                                                                                            <tr>
                                                                                                <td style="width: 9px;"></td>
                                                                                                <td colspan="2">
                                                                                                    <asp:Label ID="lblIsRepetitive" runat="server" CssClass="clsLabel">Is Repetitive</asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:UpdatePanel ID="upnlIsRepetitive" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <asp:CheckBox ID="chkIsRepetitive" runat="server" CssClass="clsCheckBox"
                                                                                                                Checked="<%# mMELSnagCorrectiveAction.IsRepetitive %>"></asp:CheckBox>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                </td>
                                                                                                <td></td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblRectifiedDate" runat="server" CssClass="clsLabelAuto">Rectified Date</asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:UpdatePanel ID="upnlRectifiedDate" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <asp:TextBox ID="txtRectifiedDate" runat="server" AutoPostBack="false" CssClass="clsTextBoxTagSearchDate"
                                                                                                                onchange="ValidateDateText(this,'wmeRectifiedDate');" Width="100px"></asp:TextBox>
                                                                                                            <cc2:CalendarExtender ID="txtRectifiedDate_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                                                                                Enabled="True" Format="<%$AppSettings:DateFormat%>" TargetControlID="txtRectifiedDate"></cc2:CalendarExtender>
                                                                                                            <cc2:TextBoxWatermarkExtender ID="wmeRectifiedDate" runat="server" TargetControlID="txtRectifiedDate"
                                                                                                                WatermarkText="<%$AppSettings:DateFormat%>" WatermarkCssClass="clsDateTextBox"></cc2:TextBoxWatermarkExtender>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                </td>
                                                                                                <td></td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblRectifiedLogNo" runat="server" CssClass="clsLabel">Rectified Log No.</asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:UpdatePanel ID="upnlRectifiedCombo" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <asp:DropDownList ID="cmbRectifiedLogNo" runat="server"
                                                                                                                CssClass="clsTextBoxTagSearchComboNewstyle" DataValueField="LogID"
                                                                                                                DataTextField="LogNoLogPageNo" AutoPostBack="True" Enabled="False">
                                                                                                            </asp:DropDownList>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td></td>
                                                                                                <td colspan="2">
                                                                                                    <asp:Label ID="lblAction" runat="server" CssClass="clsLabelAuto">
																								Action
                                                                                                    </asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtAction" runat="server"
                                                                                                        class="clsTextBoxTagSearchMultilineNewstyle" Width="278px"
                                                                                                        Text="<%# mMELSnagCorrectiveAction.Action %>"
                                                                                                        ToolTip="Enter Action" MaxLength="1000" TextMode="MultiLine">
                                                                                                    </asp:TextBox>
                                                                                                </td>
                                                                                                <td></td>
                                                                                                <td>
                                                                                                    <asp:Label Width="100px" ID="lblCauseOfDefect" runat="server"
                                                                                                        CssClass="clsLabelAuto">
																								Cause of defect
                                                                                                    </asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtCauseofDefect" runat="server" Width="278px"
                                                                                                        CssClass="clsTextBoxTagSearchMultilineNewstyle"
                                                                                                        Text="<%# mMELSnagCorrectiveAction.CauseOfDefect %>"
                                                                                                        ToolTip="Enter causes of Defect"
                                                                                                        MaxLength="1000" TextMode="MultiLine">
                                                                                                    </asp:TextBox>
                                                                                                </td>
                                                                                                <td></td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblPreventiveMeasuresTaken" runat="server"
                                                                                                        CssClass="clsLabelAuto"
                                                                                                        DESIGNTIMEDRAGDROP="872" Width="120px">
																								Preventive Measures Taken
                                                                                                    </asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtPreventiveMeasuresTaken" runat="server"
                                                                                                        class="clsTextBoxTagSearchMultilineNewstyle" Width="278px"
                                                                                                        Text="<%# mMELSnagCorrectiveAction.PreventionTaken %>"
                                                                                                        ToolTip="Enter preventive measures to be taken"
                                                                                                        MaxLength="1000" TextMode="MultiLine">
                                                                                                    </asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td></td>
                                                                                                <td colspan="2">
                                                                                                    <asp:Label Width="145px" ID="lblActionTakenAgainstStaff" runat="server"
                                                                                                        CssClass="clsLabelAuto" DESIGNTIMEDRAGDROP="876">
																								Action taken against eng. staff
                                                                                                    </asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtActionTakenAganistEngStaff" runat="server"
                                                                                                        CssClass="clsTextBoxTagSearch" Width="278px"
                                                                                                        Text="<%# mMELSnagCorrectiveAction.ActionAgainstStaff %>"
                                                                                                        ToolTip="Enter actions against Staff"
                                                                                                        MaxLength="50">
                                                                                                    </asp:TextBox>
                                                                                                </td>
                                                                                                <td></td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblRectificationSector" runat="server"
                                                                                                        CssClass="clsLabelAuto">
																								Sector / Place
                                                                                                    </asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtRectificationSector" runat="server"
                                                                                                        CssClass="clsTextBoxTagSearch" Width="278px"
                                                                                                        Text="<%# mMELSnagCorrectiveAction.RectifiedStation %>"
                                                                                                        ToolTip="Enter Sector / Place"
                                                                                                        MaxLength="50">
                                                                                                    </asp:TextBox>
                                                                                                </td>
                                                                                                <td></td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblRectificationMechanic" runat="server"
                                                                                                        CssClass="clsLabelAuto">
																								Mechanic / Rectification By
                                                                                                    </asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:UpdatePanel ID="upnlLicenceNo" runat="server" UpdateMode="Conditional">
                                                                                                        <ContentTemplate>
                                                                                                            <table>
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                        <asp:TextBox ID="txtLicenceNo" runat="server"
                                                                                                                            ToolTip="Enter name of Mechanic"
                                                                                                                            CssClass="clsTextBoxTagSearch"
                                                                                                                            AutoComplete="off" ClientIDMode="Static"
                                                                                                                            OnTextChanged="txtLicenceNo_TextChanged"
                                                                                                                            AutoPostBack="true" MaxLength="200">
                                                                                                                        </asp:TextBox>
                                                                                                                        <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtLicenceNo_Autocomplete"
                                                                                                                            runat="server" DelimiterCharacters="" Enabled="True" CompletionSetCount="20"
                                                                                                                            MinimumPrefixLength="0" CompletionInterval="1"
                                                                                                                            ServicePath="wfLogDefectActionList_Ajax.aspx" ServiceMethod="GetLicenceList"
                                                                                                                            TargetControlID="txtLicenceNo" UseContextKey="False" ContextKey=""
                                                                                                                            CompletionListCssClass="ac_results_Main" CompletionListItemCssClass="ac_results_li"
                                                                                                                            CompletionListHighlightedItemCssClass="ac_results_Main"
                                                                                                                            OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating"
                                                                                                                            OnClientHiding="ClientHiding"
                                                                                                                            OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                                                                                                        </cc2:AutoCompleteExtender>
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        <asp:ImageButton ID="imgbtnEmployeeLicence" runat="server"
                                                                                                                            ImageUrl="~/images/plus1.png" Height="22px" Width="24px"
                                                                                                                            ToolTip="Click to add multiple Mechanics" CausesValidation="true" />
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td colspan="2">
                                                                                                                        <asp:Label ID="lblLicenceCount" runat="server"
                                                                                                                            Visible="<%# mMELSnagCorrectiveAction.MaintenanceDoneByEmployees.Count > 1 %>"
                                                                                                                            ToolTip="<%# mMELSnagCorrectiveAction.AllLicenceNos%>"
                                                                                                                            Text="and More" CssClass="clsLabelHeader clsCursorStyle">
                                                                                                                        </asp:Label>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td></td>
                                                                                                <td colspan="2">
                                                                                                    <asp:Label ID="lblRemark" runat="server" CssClass="clsLabelAuto">Remark</asp:Label>
                                                                                                </td>
                                                                                                <td colspan="4">
                                                                                                    <asp:TextBox ID="txtRemark" runat="server" Width="278px" Height="22px"
                                                                                                        CssClass="clsTextBoxTagSearchMultilineNewStyleLong"
                                                                                                        Text="<%# mMELSnagCorrectiveAction.Remark %>"
                                                                                                        ToolTip="Enter Remark" MaxLength="500">
                                                                                                    </asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </asp:PlaceHolder>
                                                                                </fieldset>
                                                                            </ContentTemplate>
                                                                        </cc2:TabPanel>
                                                                        <cc2:TabPanel ID="tabFileAttachment" runat="server" ClientIDMode="Static" CssClass="clsPanel1">
                                                                            <HeaderTemplate>
                                                                                <asp:Label runat="server" Text="File Attachment" ID="lblFileAttachment"></asp:Label>
                                                                            </HeaderTemplate>
                                                                            <ContentTemplate>
                                                                                <fieldset class="clsFieldSetNewStyle">
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:UpdatePanel ID="upnlFileupload" runat="server" UpdateMode="Conditional">
                                                                                                    <ContentTemplate>
                                                                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                                                                            <tr>
                                                                                                                <td style="width: 9px;"></td>
                                                                                                                <td style="width: 127px;">
                                                                                                                    <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <input type="button" id="btnSelectFile" value="Select File" style="width: 115px;"
                                                                                                                        class="clsbtnH clsinfoH1" />
                                                                                                                </td>
                                                                                                                <td style="padding-left: 3px;">
                                                                                                                    <asp:Button ID="btnDelAttach" runat="server" CssClass="clsbtnH clsinfoH1" ToolTip="Click to Remove Attachment"
                                                                                                                        Text="Remove Attachment" Enabled="False" Width="140px"></asp:Button>
                                                                                                                </td>
                                                                                                                <td style="padding-left: 2px;">
                                                                                                                    <asp:ImageButton ID="attachmentICN" runat="server" CausesValidation="False"
                                                                                                                        ImageUrl="icons/CLIP01.ICO" Visible="false"
                                                                                                                        Height="20px" Width="20px"></asp:ImageButton>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </ContentTemplate>
                                                                                                </asp:UpdatePanel>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </fieldset>
                                                                            </ContentTemplate>
                                                                        </cc2:TabPanel>
                                                                    </cc2:TabContainer>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table id="tblButton">
                                                                        <tr>
                                                                            <!--Dummy panel to open modelpopup-->
                                                                            <td align="right">
                                                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel1">
                                                                                    <ContentTemplate>
                                                                                        <asp:Button ID="hdnBtnFileUpload" ClientIDMode="Static" runat="server" Text="Add"
                                                                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                            <td align="right">
                                                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2">
                                                                                    <ContentTemplate>
                                                                                        <asp:Button ID="hdnBtnSelectLog" ClientIDMode="Static" runat="server" Text="Add"
                                                                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                                                                        <asp:Button ID="hdnImgBtnSendMail" ClientIDMode="Static" runat="server" Text="----"
                                                                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                            <!--End -->
                                                                        </tr>
                                                                    </table>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 0px;">
                                                        <td style="height: 0px;">
                                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel3">
                                                                <ContentTemplate>
                                                                    <td>
                                                                        <asp:Button ID="hdnBtnMaintDoneBy" ClientIDMode="Static" runat="server" Text="----"
                                                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                                                        <asp:Button ID="hdnimgBtnMELMasterChapter" ClientIDMode="Static" runat="server" Text="----"
                                                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                                                        <asp:Button ID="hdnBtnMELDetail" ClientIDMode="Static" runat="server" Text="----"
                                                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                                                    </td>
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
            <script type="text/javascript">
                Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
                    $("#<%=txtReportedBy.ClientID%>").autocomplete('wfAutoEmpLicenseNo.aspx?WithoutLicenseNoAlso=1', {
                        width: 278,
                        autoFill: false,
                        matchContains: true,
                        max: 30,
                        delay: 0
                    });

                    $("#<%=txtActionTakenAganistEngStaff.ClientID%>").autocomplete('wfAutoEmpLicenseNo.aspx?WithoutLicenseNoAlso=1', {
                        width: 278,
                        autoFill: false,
                        mustMatch: false,
                        matchContains: true,
                        max: 30,
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
        </div>
        <!-- File Upload Modal Dialog-->
        <div style="display: none">
            <asp:HiddenField runat="server" ID="btnDummyFileUpload" />
        </div>
        <asp:Panel runat="server" ID="pnlFileUpload" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
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
                        //                        $("#IFileUpload").ready(function () {
                        //                            $("#btnDummyFileUpload").click();
                        //                            $get("AjaxLoader").style.visibility = 'hidden';
                        //                        });
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
        <!-- Select LogInfo popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummySelectLog" Text="Maintenance Activity" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlSelectLog" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeSelectLog" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupSelectLog" runat="server" TargetControlID="btnDummySelectLog"
            PopupControlID="pnlSelectLog" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameSelectLogStateComplete() {
                $("#btnDummySelectLog").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenSelectLogWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeSelectLog").attr("src", "wfMELSnagCorrectiveActionLogInfo_AJAX.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummySelectLog").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForSelectLog() {
                var SelectLogwindow = $find("<%=mdlPopupSelectLog.ClientID %>");
                //close Task Card Tool popup window
                SelectLogwindow.hide();
                //           release resources
                $("#IframeSelectLog").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnSelectLog").click();
            }
        </script>
        <!-- End-->
        <!-- MEL Detail Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyMELDetail" Text="MEL Detail" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlMELDetail" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeMELDetail" frameborder="0" height="100%" width="100%" src="JavaScript:''"
                allowtransparency="true" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupMELDetail" runat="server" TargetControlID="btnDummyMELDetail"
            PopupControlID="pnlMELDetail" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameMELDetailStateComplete() {
                $("#btnDummyMELDetail").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenMELDetail() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeMELDetail").attr("src", "wfMELDetail_Ajax.aspx?Type=pup&OpenFrom=Snag");

                    if (!$.browser.msie) {
                        $("#btnDummyMELDetail").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForMELDetail() {
                var MELDetailwindow = $find("<%=mdlPopupMELDetail.ClientID %>");
                //close MEL Detail popup window
                MELDetailwindow.hide();
                //           release resources
                $("#IframeMELDetail").attr("src", "JavaScript:''");
                //call image button
                $("#hdnBtnMELDetail").click();
            }
        </script>
        <!-- End-->
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
        <!-- Assembly Insp Maintenance Done By Employee Dialog-->
        <div style="display: none">
            <asp:HiddenField runat="server" ID="btnDummyMaintDoneBy" />
        </div>
        <asp:Panel runat="server" ID="pnlMaintDoneBy" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
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
                    $("#IMaintDoneBy").attr("src", "wfMaintenanceDoneByEmployee_Ajax.aspx?Type=pup&MaintTypeID=11"); //11=MelSnag

                    if (!$.browser.msie) {
                        $("#btnDummyMaintDoneBy").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }


            }

        </script>
        <script type="text/javascript">
            function ParentCallBackFunctionForMaintDoneBy() {
                var MaintDoneBywindow = $find("<%=mdlPopupMaintDoneBy.ClientID %>");
                //close Ass Insp Maint Done By Emp popup window
                MaintDoneBywindow.hide();
                //Free resources
                $("#IMaintDoneBy").attr("src", "JavaScript:''");
                $("#hdnBtnMaintDoneBy").click();

            }
        </script>
        <!-- End -->
        <!-- MELMaster Popup Window -->
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyMELMaster" Text="Dummy MELMaster" ClientIDMode="Static" />
        </div>
        <asp:Panel runat="server" ID="pnlPopupMELMaster" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="iPopupMELMaster" frameborder="0" allowtransparency="true" height="100%"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupMELMaster" runat="server" TargetControlID="btnDummyMELMaster"
            PopupControlID="pnlPopupMELMaster" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameMELMasterStateComplete() {
                $("#btnDummyMELMaster").click();
                $get("AjaxLoader").style.visibility = "hidden";
            }
            function OpenMELMasterWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#iPopupMELMaster").attr("src", "wfMELSelectList_Ajax.aspx?Type=pup");

                    if (!$.browser.msie) {
                        $("#btnDummyMELMaster").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }

                    return false;
                } catch (e) {
                    alert(e);
                }

            }
        </script>
        <script type="text/javascript">
            function ParentCallBackFunction() {
                var MELMasterwindow = $find("<%=mdlPopupMELMaster.ClientID %>");
                //close MELMaster popup window
                MELMasterwindow.hide();
                $("#iPopupMELMaster").attr("src", "JavaScript:''");
                //call MELMaster image button
                $("#hdnimgBtnMELMasterChapter").click();
            }
        </script>

        <!-- Send Email Modal PopUp-->
        <%--Added By Harsh on 20th Feb 2024--%>
        <div style="display: none">
            <asp:HiddenField runat="server" ID="hdnBtnSendMail" />
        </div>
        <asp:Panel runat="server" ID="pnlSendMail" HorizontalAlign="Center" Style="height: 100%; width: 100%;">
            <iframe id="ISendMail" allowtransparency="true" frameborder="0" height="100%" width="100%"
                src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupSendMail" runat="server" TargetControlID="hdnBtnSendMail"
            PopupControlID="pnlSendMail" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameSendMailComplete() {
                $("#hdnBtnSendMail").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            $(document).ready(function () {
                $("#btnSendMail").live("click", function () {
                    try {
                        $get("AjaxLoader").style.visibility = 'visible';
                        $("#ISendMail").attr("src", "wfByMail_Ajax.aspx?Type=pup");
                        if (!$.browser.msie) {
                            $("#hdnBtnSendMail").click();
                            $get("AjaxLoader").style.visibility = 'hidden';
                        }
                        return false;
                    } catch (e) {
                        alert(e);
                    }
                });
            });

            function ParentCallBackFunctionForSendMail() {
                var sendEmailWindow = $find("<%=mdlPopupSendMail.ClientID %>");
                sendEmailWindow.hide();
                $("#ISendMail").attr("src", "JavaScript:''");
            }

            function ParentCallBackFunctionToSendMail() {
                var sendEmailWindow = $find("<%=mdlPopupSendMail.ClientID %>");
                sendEmailWindow.hide();
                $("#IframeReceipt").attr("src", "JavaScript:''");
                $("#hdnImgBtnSendMail").click();
            }
        </script>
        <!-- End -->
    </form>
</body>
</html>
