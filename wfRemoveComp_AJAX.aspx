<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="wfRemoveComp_AJAX.aspx.vb"
    Inherits="Flypal.wfRemoveComp_AJAX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="MSGBox" Src="MSGBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Configuration.ConfigurationManager" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Removal of Component</title>
    <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <link id="MainStyle" rel="stylesheet" type="text/css" />
    <asp:PlaceHolder runat="server">
        <!-- #include file= "LocalFunctionAjax.htm" -->
    </asp:PlaceHolder>
    <script type="text/javascript" src="VALIDATEFUNCTIONS.js"></script>
    <link rel="stylesheet" type="text/css" href="AutoComplete\jquery.autocomplete.css" />
    <script type="text/javascript" src="AutoComplete\jquery.autocomplete.js"></script>
    <script id="clientEventHandlersJS" type="text/javascript" language="javascript">

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
        .style1
        {
            height: 5px;
        }
        .clsCursorStyle
        {
            cursor: pointer;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMSGBox" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc2:MSGBox ID="MSGBoxCtrl" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="tblmain" class="clstablelistout" border="0">
        <tr>
            <td>
                <asp:Panel ID="pnlmain" runat="server" CssClass="clspanel1">
                    <table id="tblInner" class="clstablelistin" border="0">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlTitle" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lbltitle" CssClass="clstitle1" runat="server">Remove Assembly</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
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
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <fieldset id="Fieldset1" class="clsFieldSet" style="border-width: 1px">
                                    <legend id="lblEngineInfo" style="font-weight: bold" runat="server"><b>Assembly and
                                        Component Info</b></legend>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upnlEngineInfo" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="L1" runat="server" CssClass="clsLabelAuto"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblATAChapter" runat="server" CssClass="clsLabel">ATA Chapter</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtATAChapter" runat="server" CssClass="clsTextBox_Ajax" BackColor="#E0E0E0"
                                                                        ReadOnly="True" Text="<%# mCompStatus.ATAChapter %>"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblPart" runat="server" CssClass="clsLabel">Part</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtPart" runat="server" CssClass="clsTextBox_Ajax" BackColor="#E0E0E0"
                                                                        ReadOnly="True" Text="<%#  mCompStatus.Comp.PartName %>"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblSerialNo" runat="server" CssClass="clsLabel">Serial No.</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSerialNo" runat="server" CssClass="clsTextBox_Ajax" BackColor="#E0E0E0"
                                                                        ReadOnly="True" Text="<%# mCompStatus.Comp.SerialNo %>"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblDescription" runat="server" CssClass="clsLabelAuto">Description</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDescription" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxMultiLine"
                                                                        ReadOnly="True" Text="<%# mCompStatus.Comp.Description %>" TextMode="MultiLine"
                                                                        ToolTip="Description">
                                                                    </asp:TextBox>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblCode" runat="server" CssClass="clsLabel">Code</asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtCode" runat="server" CssClass="clsTextBox_Ajax" Text="<%# mCompStatus.Comp.Code %>"
                                                                        ToolTip="Code" ReadOnly="True" BackColor="#E0E0E0"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblPosition" runat="server" CssClass="clsLabel">Position</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtPosition" runat="server" BackColor="#E0E0E0" CssClass="clsTextBoxDate_Ajax"
                                                                        ReadOnly="True" Text="<%# mCompStatus.Position %>"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblManufacturer" runat="server" CssClass="clsLabel">Manufacturer</asp:Label>
                                                                </td>
                                                                <td align="right">
                                                                    <table id="Table6" cellspacing="0" cellpadding="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:DropDownList ID="cmbManufacturerList" runat="server" CssClass="clsComboBox"
                                                                                    SelectedValue="<%# mCompStatus.ManufacturerID %>" Enabled="False" DataValueField="ID"
                                                                                    DataTextField="Name">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td align="left">
                                                                                <td>
                                                                                    <asp:ImageButton ID="imgManufacturer" runat="server" ImageUrl="~/images/plus1.png"
                                                                                        Height="22px" Width="24px" CausesValidation="False" ToolTip="Click to Add Manufacturer"
                                                                                        Visible="False" />
                                                                                </td>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" colspan="6" style="height: 40px">
                                                                    <asp:UpdatePanel ID="upnlHistoryCard" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:LinkButton ID="lnkHistoryCard" runat="server" CssClass="clsLinkButton" Font-Italic="true"
                                                                                            Font-Size="8pt">View History Card</asp:LinkButton>
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td align="right">
                                                                                        <img width="25px" height="25px" style="border: 0" alt="" src="images/HistoryCard.png" />
                                                                                    </td>
                                                                                    <td>
                                                                                        &nbsp;&nbsp;
                                                                                        <asp:LinkButton ID="lnkPrintLogBookEntry" runat="server" CssClass="clsLinkButton"
                                                                                            Font-Italic="true" Font-Size="8pt"  >View Log Book Entry</asp:LinkButton>
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td align="right">
                                                                                        <img width="25px" height="25px" style="border: 0" alt="" src="images/HistoryCard.png" />
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
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <fieldset id="fdsRemovalInfo" class="clsFieldSet" style="border-width: 1px">
                                    <legend id="lblRemovalInfo" runat="server" style="font-weight: bold"><b>Removal Information
                                        of the []</b></legend>
                                    <asp:UpdatePanel ID="upnlRemovalInfo" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td align="right" colspan="4">
                                                        <table>
                                                            <tr>
                                                                <td align="right" colspan="2">
                                                                    <asp:UpdatePanel ID="upnlSelectLog" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:Button ID="btnSelectLog" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                                                Text="Select Log" ToolTip="Click to open Select Log screen" />
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblRemovedOnStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblRemovedOn" runat="server" CssClass="clsLabel">Removed On</asp:Label>
                                                    </td>
                                                    <td colspan="1">
                                                        <table id="Table1" border="0" cellpadding="0" cellspacing="0">
                                                        </table>
                                                        <%-- <uc1:sicalendar id="calStartDate" runat="server"></uc1:sicalendar>--%>
                                                        <asp:TextBox ID="calRemove" runat="server" AutoPostBack="true" CssClass="clsTextBox_Ajax"
                                                            onchange="ValidateDateText(this,'FromDate_watermarkextender');" TabIndex="1"
                                                            Width="90px"></asp:TextBox>
                                                        <cc2:CalendarExtender ID="calRemove_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                                            Enabled="true" Format="<%$AppSettings:DateFormat%>" TargetControlID="calRemove">
                                                        </cc2:CalendarExtender>
                                                        <cc2:TextBoxWatermarkExtender ID="FromDate_watermarkextender" runat="server" ClientIDMode="Static"
                                                            TargetControlID="calRemove" WatermarkCssClass="clsDateTextBox" WatermarkText="<%$AppSettings:DateFormat%>">
                                                        </cc2:TextBoxWatermarkExtender>
                                                    </td>
                                                    <td valign="bottom">
                                                        <asp:Label ID="lblValuesAtRemoval" runat="server" CssClass="clsLabelHeader">Values at Removal</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblSubATA" runat="server" CssClass="clsLabel">Sub ATA Chapter</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="cmbSubATAChpater" runat="server" CssClass="clsComboBox" DataTextField="SubATAChapter"
                                                            DataValueField="ID">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td rowspan="3" valign="top">
                                                        <asp:GridView ID="dgRemovalValue" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                            CssClass="clsGrid" DataKeyNames="CompStatusID" ShowHeaderWhenEmpty="True" TabIndex="7">
                                                            <AlternatingRowStyle CssClass="clsdgAltItem" HorizontalAlign="Left" />
                                                            <RowStyle CssClass="clsdgItem" HorizontalAlign="Left" />
                                                            <HeaderStyle CssClass="clsdgHeader" HorizontalAlign="Left" />
                                                            <Columns>
                                                                <asp:BoundField DataField="PeriodName" HeaderText="Period " HtmlEncode="False" />
                                                                <asp:BoundField DataField="CompRemovalValueFormatted" HeaderText="Component" HtmlEncode="False" />
                                                                <asp:BoundField DataField="AssemblyRemovalValueFormatted" HeaderText="Assembly" HtmlEncode="False" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblWorkOrderNo" runat="server" CssClass="clsLabelAuto">Work Order No.</asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtWorkOrderNo" runat="server" CssClass="clsTextBoxDate_Ajax" MaxLength="25"
                                                            TabIndex="2" Text="<%# mCompStatus.RemovalWONO %>" ToolTip="Enter Work Order Number"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblReasonStar1" runat="server" CssClass="clsLabelStar">*</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblReason" runat="server" CssClass="clsLabel">Reason</asp:Label>
                                                    </td>
                                                    <td>
                                                        <table id="Table2" border="0" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbReason" runat="server" CssClass="clsComboBox_Ajax" DataTextField="Name"
                                                                        DataValueField="ID" TabIndex="3">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:ImageButton ID="imgReason" runat="server" CausesValidation="False" Height="22px"
                                                                        ImageUrl="~/images/plus1.png" ToolTip="Click to Add Reason" Width="24px" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td align="left" colspan="2">
                                                        <table id="Table3" border="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIsRemUnscheduled" runat="server" Checked="<%# mCompStatus.IsRemUnschedule %>"
                                                                        CssClass="clsCheckBox" Text="Un-Schedule(for reliability monitoring)" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblNote" runat="server" CssClass="clsLabel">Note</asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtNote" runat="server" CssClass="clsTextBoxMultiLine" MaxLength="200"
                                                            TabIndex="6" Text="<%# mCompStatus.RemovalRemark %>" TextMode="MultiLine" ToolTip="Enter Note"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td style="width: 101px">
                                                        <asp:Label ID="Label1" runat="server" CssClass="clsLabelAuto">Expired</asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="chkExpired" runat="server" Checked="<%# mCompStatus.IsExpired %>"
                                                            Enabled="<%# mCompStatus.IsExpiredEnabled %>" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td style="width: 101px">
                                                        <asp:Label ID="lblDoneByAgency" runat="server" CssClass="clsLabel">Done By Agency</asp:Label>
                                                    </td>
                                                    <td align="left" colspan="2">
                                                        <asp:TextBox ID="txtRemDoneBy" runat="server" CssClass="clsTextBox_Ajax" MaxLength="100"
                                                            Text="<%# mCompStatus.RemDoneBy %>" ToolTip="Enter Done By Agency Name"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblLicenceNo" runat="server" CssClass="clsLabelAuto" Width="72px">License No.</asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:UpdatePanel ID="upnlLicenceNo" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txtLicenceNo" runat="server" CssClass="clsTextBox_Ajax" ToolTip="Enter License No."
                                                                                AutoComplete="off" ClientIDMode="Static" OnTextChanged="txtLicenceNo_TextChanged"
                                                                                AutoPostBack="true" MaxLength="200"></asp:TextBox>
                                                                            <cc2:AutoCompleteExtender ClientIDMode="Static" ID="txtLicenceNo_Autocomplete" runat="server"
                                                                                DelimiterCharacters="" Enabled="True" CompletionSetCount="20" MinimumPrefixLength="0"
                                                                                CompletionInterval="1" ServicePath="wfComplyAssemblyMonitorInspStatus_Ajax.aspx"
                                                                                ServiceMethod="GetLicenseNoList" TargetControlID="txtLicenceNo" OnClientItemSelected="SetLicenceNo"
                                                                                UseContextKey="False" ContextKey="" CompletionListCssClass="ac_results_Main"
                                                                                CompletionListItemCssClass="ac_results_li" CompletionListHighlightedItemCssClass="ac_over_Main"
                                                                                OnClientPopulated="ClientPopulated" OnClientPopulating="ClientPopulating" OnClientHiding="ClientHiding"
                                                                                OnClientShown="ClientHiding" OnClientShowing="ClientShowing">
                                                                            </cc2:AutoCompleteExtender>
                                                                        </td>
                                                                        <td>
                                                                            <asp:ImageButton ID="imgbtnEmployeeLicence" runat="server" ImageUrl="~/images/plus1.png"
                                                                                Height="22px" Width="24px" ToolTip="Click to Add New Licence No." CausesValidation="true" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <asp:Label ID="lblLicenceCount" runat="server" Visible="<%# mCompStatus.MaintenanceDoneByEmployees.Count > 1 %>"
                                                                                ToolTip="<%# mCompStatus.AllLicenceNos%>" Text="and More" CssClass="clsLabelHeader clsCursorStyle"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblPlace" runat="server" CssClass="clsLabelAuto" Width="72px">Place</asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtPlace" runat="server" CssClass="clsTextBox_Ajax" MaxLength="25"
                                                            TabIndex="5" Text="<%# mCompStatus.RemPlace %>" ToolTip="Enter Place"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <span id="lblAttachFile" class="clsLabel">Attach File</span>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:UpdatePanel ID="upnlAttach" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table id="Table12" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <input type="button" id="btnSelectFile" value="Select File" style="width: 100px;"
                                                                                            runat="server" class="clsButton_Ajax" tabindex="8" />
                                                                                    </td>
                                                                                    <td style="padding-left: 3px;">
                                                                                        <asp:Button ID="btnDelAttach" runat="server" CssClass="clsButton_Ajax" Enabled="False"
                                                                                            Text="Remove Attachment" ToolTip="Click to Remove Attachment" Width="120px" />
                                                                                    </td>
                                                                                    <td style="padding-left: 2px;">
                                                                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" Height="20px"
                                                                                            ImageUrl="icons/CLIP01.ICO" Width="20px" />
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
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </fieldset>
                            </td>
                        </tr>
                        <!--Dummy panel to open modelpopup-->
                        <tr>
                            <td align="right" class="style1">
                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel1">
                                    <ContentTemplate>
                                        <asp:Button ID="hdnBtnSelectLog" ClientIDMode="Static" runat="server" Text="Add"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnManufacturer" ClientIDMode="Static" runat="server" Text="Add"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnRemovalReason" ClientIDMode="Static" runat="server" Text="Add"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                        <asp:Button ID="hdnBtnMaintDoneBy" ClientIDMode="Static" runat="server" Text="----"
                                            CausesValidation="False" Style="display: none;"></asp:Button>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <!--End -->
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlScrollNote" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblScrollNote" runat="server" CssClass="clsLabelHeader" Visible="false">Note: To change Removal Date, Please Revert the Removal and do the Removal again.</asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:UpdatePanel ID="upnlButtons" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table border="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="hdnBtnFileUpload" runat="server" CausesValidation="False" ClientIDMode="Static"
                                                        Style="display: none;" Text="----" />
                                                </td>
                                                <td>
                                                    <td>
                                                        <asp:Button ID="btnTechDirection" runat="server" ValidationGroup="a" CssClass="clsButtonLong1"
                                                            Text="Technical Direction" ToolTip="Click to print Technical Direction" Visible='<%# iif(AppSettings("ClientCode") = "BA" OR AppSettings("ClientCode") = "PAS" Or AppSettings("ClientCode") = "Novo"  Or AppSettings("ClientCode") = "YA" Or AppSettings("ClientCode") = "TA" Or AppSettings("ClientCode") = "Demo" Or AppSettings("ClientCode") = "SAA",True,False) %>'
                                                            Width="120px" />
                                                    </td>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnSave" runat="server" CssClass="clsButton_Ajax" TabIndex="9" Text="Save"
                                                        ToolTip="Click to save information of Removal Component" ValidationGroup="a" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnPrint" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                        TabIndex="10" Text="Print" ToolTip="Click to Print the Removed Component" ValidationGroup="a" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="clsButton_Ajax"
                                                        TabIndex="11" Text="Back" ToolTip="Click to go Back to Previous page" />
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
    <%--Date Validations--%>
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
    <!-- Select SelectSelectLog popup Window -->
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
                $("#IframeSelectLog").attr("src", "wfSelectLog_Ajax.aspx?Type=pup");

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
    <!-- Select SelectManufacturer popup Window -->
    <div>
        <div style="display: none">
            <asp:Button runat="server" ID="btnDummyManufacturer" Text="Dummy Manufacturer" ClientIDMode="Static">
            </asp:Button>
        </div>
        <asp:Panel runat="server" ID="pnlManufacturer" ClientIDMode="Static" HorizontalAlign="Center"
            Style="height: 100%; width: 100%;">
            <iframe id="IframeManufacturer" frameborder="0" height="100%" allowtransparency="true"
                width="100%" src="JavaScript:''" scrolling="auto"></iframe>
        </asp:Panel>
        <cc2:ModalPopupExtender ID="mdlPopupManufacturer" runat="server" TargetControlID="btnDummyManufacturer"
            PopupControlID="pnlManufacturer" BackgroundCssClass="clsModalPopupBG">
        </cc2:ModalPopupExtender>
        <script type="text/javascript">
            function IFrameStateComplete() {
                $("#btnDummyManufacturer").click();
                $get("AjaxLoader").style.visibility = 'hidden';
            }

            function OpenManufacturerWindow() {
                try {

                    $get("AjaxLoader").style.visibility = 'visible';
                    $("#IframeReportAll").attr("src", "wfManufacturer_Ajax.aspx?Type=pup");
                    // $("#IframeReportAll").load(function () {
                    //                    var doc = IframeManufacturer.window;
                    //                    IframeManufacturer.SetPageLayout();

                    if (!$.browser.msie) {
                        $("#btnDummyManufacturer").click();
                        $get("AjaxLoader").style.visibility = 'hidden';
                    }


                    //});


                    return false;
                } catch (e) {
                    alert(e);
                }

            }
            function ParentCallBackFunctionForManufacturer() {
                var Manufacturerwindow = $find("<%=mdlPopupManufacturer.ClientID %>");
                //close Manufacturer popup window
                Manufacturerwindow.hide();
                //           release resources
                $("#IframeManufacturer").attr("src", "JavaScript:''");
                //call Manufacturer image button
                $("#hdnimgbtnManufacturer").click();
            }
        </script>
        <!-- End-->
    </div>
    <!-- Removal Reason Popup Window -->
    <div style="display: none">
        <asp:Button runat="server" ID="btnDummyRemovalReason" Text="Removal Reason" ClientIDMode="Static" />
    </div>
    <asp:Panel runat="server" ID="pnlRemovalReason" ClientIDMode="Static" HorizontalAlign="Center"
        Style="height: 100%; width: 100%;">
        <iframe id="IframeRemovalReason" frameborder="0" height="100%" width="100%" src="JavaScript:''"
            allowtransparency="true" scrolling="auto"></iframe>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="mdlPopupRemovalReason" runat="server" TargetControlID="btnDummyRemovalReason"
        PopupControlID="pnlRemovalReason" BackgroundCssClass="clsModalPopupBG">
    </cc2:ModalPopupExtender>
    <script type="text/javascript">
        function IFrameRemovalReasonStateComplete() {
            $("#btnDummyRemovalReason").click();
            $get("AjaxLoader").style.visibility = 'hidden';
        }

        function OpenRemovalReasonWindow() {
            try {

                $get("AjaxLoader").style.visibility = 'visible';
                $("#IframeRemovalReason").attr("src", "wfRemovalReason_AJAX.aspx?Type=pup");

                if (!$.browser.msie) {
                    $("#btnDummyRemovalReason").click();
                    $get("AjaxLoader").style.visibility = 'hidden';
                }

                return false;
            } catch (e) {
                alert(e);
            }

        }
        function ParentCallBackFunctionForRemovalReason() {
            var RemovalReasonwindow = $find("<%=mdlPopupRemovalReason.ClientID %>");
            //close Removal Reason popup window
            RemovalReasonwindow.hide();
            //           release resources
            $("#IframeRemovalReason").attr("src", "JavaScript:''");
            //call image button
            $("#hdnBtnRemovalReason").click();
        }
    </script>
    <!-- End-->
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
                $("#IMaintDoneBy").attr("src", "wfMaintenanceDoneByEmployee_Ajax.aspx?Type=pup&MaintTypeID=4");

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
    <script type="text/javascript">
        function SetLicenceNo(source, e) {
            //get id from autocomplete list
            var node;
            var value = e.get_value();

            if (value) node = e.get_item();
            else {
                value = e.get_item().parentNode._value;
                node = e.get_item().parentNode;
            }

            var text = (node.innerText) ? node.innerText : (node.textContent) ? node.textContent : node.innerHtml;
            source.get_element().value = text;

            //Set id to relevent hidden field 
            var textbox;
            if (source._id == "txtLicenceNo_Autocomplete") {
                textbox = document.getElementById('hdnLicenceNo');
            }


            textbox.value = value.toString();
        }
        //text change function : if id found,set id to hiddenfield and return ,else clear the hidden field value..
    </script>
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
    </form>
</body>
</html>
